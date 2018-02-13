using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using clinic_management.Models;

namespace clinic_management.Controllers
{
    public class InventoryController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();

        // GET: Items
        public ActionResult Index()
        {
            ModelContainer modelcontainer = new ModelContainer();

            modelcontainer.Medicine = db.Items.Where(i => i.ItemType == "Medicine").Where(i => i.deleted == "0").ToList();
            modelcontainer.Utensil = db.Items.Where(i => i.ItemType == "Utensil").Where(i => i.deleted == "0").ToList();

            ViewBag.OutOfStock = db.Items.Where(i => i.ItemQuantity == 0).Where(i => i.deleted == "0").Count();
            ViewBag.CriticalStock = db.Items.Where(i => i.ItemQuantity <= 10).Where(i => i.deleted == "0").Count();

            if (TempData.ContainsKey("isUtensil"))
            {
                if (TempData["isUtensil"].ToString() == "T")
                {
                    ViewBag.isUtensil = true;
                }
                else
                {
                    ViewBag.isUtensil = false;
                }
            }

            return View(modelcontainer);
        }

        // GET: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getItemID()
        {
            var intMedicineCount = db.Items.Where(s => s.ItemType == "Medicine").Count();
            var medicineID = GetID("MD", intMedicineCount);

            var intUtensilCount = db.Items.Where(s => s.ItemType == "Utensil").Count();
            var utensilID = GetID("UT", intUtensilCount);

            return Json(new { mid = medicineID, uid = utensilID });
        }

        // Returns the required ID
        public string GetID(string format, int count)
        {
            string id = format;
            int idCount = count + 1;

            for (int i = idCount.ToString().Count(); i < 4; i++)
            {
                id += '0';
            }

            return id + idCount;
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,ItemName,ItemQuantity,ItemPurpose,ItemType")] Item item)
        {
            if (ModelState.IsValid)
            {
                TempData["isUtensil"] = "F";

                item.ItemQuantity = 0;
                item.deleted = "0";

                db.Items.Add(item);
                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine("Medicine Added");

                if(item.ItemType == "Utensil")
                {
                    TempData["isUtensil"] = "T";
                }

                return RedirectToAction("Index");
            }

            System.Diagnostics.Debug.WriteLine("Medicine Not Added");

            return RedirectToAction("Index");
        }

        // GET: Items/Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,ItemName,ItemQuantity,ItemType,ItemPurpose,deleted")] Item item)
        {
            if (ModelState.IsValid)
            {
                TempData["isUtensil"] = "F";

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

                if (item.ItemType == "Utensil")
                {
                    TempData["isUtensil"] = "T";
                }

                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string[] id)
        {
            for (int i = 0; i < id.Length; i++)
            {
                var selectedID = id[i];
                var result = db.Items.SingleOrDefault(it => it.ItemID == selectedID);
                if (result != null)
                {
                    result.deleted = "1";
                    db.SaveChanges();
                }
            }

            return Json(new { status = true });
        }

        // GET: Inventory/Supply/{id}
        public ActionResult Supply(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelContainer container = new ModelContainer();
            container.Item = db.Items.Find(id);
            container.SupplyList = db.Supplies.Where(s => s.ItemID == id).ToList();
            if (container.Item == null)
            {
                return HttpNotFound();
            }
            return View(container);
        }

        // GET: Inventory/AddSupply/{id}
        public ActionResult AddSupply(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ItemID = id;

            return View();
        }

        // POST: Inventory/AddSupply/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSupply([Bind(Include = "SupplyID,ItemID,SupplyQuantity,ReceivedDate,ExpirationDate")] Supply supply)
        {
            if (ModelState.IsValid)
            {
                db.Supplies.Add(supply);
                db.SaveChanges();

                var result = db.Items.SingleOrDefault(it => it.ItemID == supply.ItemID);
                if (result != null)
                {
                    result.ItemQuantity = (Int16)(result.ItemQuantity + supply.SupplyQuantity);
                    db.SaveChanges();
                }

                return RedirectToAction("Supply", new { id = supply.ItemID });
            }
            return RedirectToAction("Supply", new { id = supply.ItemID });
        }

        // GET: Inventory/EditSupply/{id}
        public ActionResult EditSupply(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supply supply = db.Supplies.Find(id);
            if (supply == null)
            {
                return HttpNotFound();
            }
            return View(supply);
        }

        // POST: Inventory/EditSupply/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSupply([Bind(Include = "SupplyID,ItemID,SupplyQuantity,ReceivedDate,ExpirationDate")] Supply supply, int oldSupplyQuantity)
        {
            if (ModelState.IsValid)
            {

                db.Entry(supply).State = EntityState.Modified;
                db.SaveChanges();

                var result = db.Items.SingleOrDefault(it => it.ItemID == supply.ItemID);

                if (supply.SupplyQuantity != oldSupplyQuantity)
                {
                    if (oldSupplyQuantity > supply.SupplyQuantity)
                    {
                        result.ItemQuantity = (Int16)(result.ItemQuantity - (oldSupplyQuantity - supply.SupplyQuantity));
                    }
                    else
                    {
                        result.ItemQuantity = (Int16)(result.ItemQuantity + (supply.SupplyQuantity - oldSupplyQuantity));
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("Supply", new { id = supply.ItemID });
            }
            return RedirectToAction("Supply", new { id = supply.ItemID });
        }
    }
}