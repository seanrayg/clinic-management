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

            ViewBag.OutOfStock = db.Items.Where(i => i.ItemQuantity == 0).Count();
            ViewBag.CriticalStock = db.Items.Where(i => i.ItemQuantity <= 10).Count();

            if (TempData.ContainsKey("isUtensil"))
            {
                ViewBag.isUtensil = true;
            }

            return View(modelcontainer);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            return View();
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
                item.ItemQuantity = 0;
                item.deleted = "0";

                db.Items.Add(item);
                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine("Medicine Added");

                if(item.ItemType == "Utensil")
                {
                    TempData["isUtensil"] = true;
                }

                return RedirectToAction("Index");
            }

            System.Diagnostics.Debug.WriteLine("Medicine Not Added");

            return RedirectToAction("Index");
        }

        // GET: Items/Edit
        public ActionResult Edit(int? id)
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
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int[] id)
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
        public ActionResult Supply(int? id)
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