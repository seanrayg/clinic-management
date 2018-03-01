using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using clinic_management.Models;
using System.IO;

namespace clinic_management.Controllers
{
    public class InventoryController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();

        // GET: Items
        public ActionResult Index()
        {
            //Get the critical stock
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "/config.conf";

            FileStream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var critical_stock_value = 0;
            using (StreamReader sr = new StreamReader(reader))
            {
                critical_stock_value = int.Parse(sr.ReadToEnd());
            }
            reader.Close();
            System.Diagnostics.Debug.WriteLine(critical_stock_value);

            var supply = db.Supplies.Where(s => s.ExpirationDate <= DateTime.Now).Where(s => s.removed == 0).GroupBy(s => s.ItemID).Select(g => new { ItemID = g.Key, SupplyQuantity = g.Sum(ss => ss.SupplyQuantity) }).ToList();

            foreach (var s in supply)
            {
                var supplies = db.Supplies.Where(sp => sp.ItemID == s.ItemID).Where(sp => sp.ExpirationDate <= DateTime.Now).ToList();
                if(supplies != null)
                {
                    foreach(var x in supplies)
                    {
                        x.removed = 1;
                        db.SaveChanges();
                    }
                }
                System.Diagnostics.Debug.WriteLine(s.ItemID + ' ' + s.SupplyQuantity);
                var item = db.Items.SingleOrDefault(i => i.ItemID == s.ItemID);
                if (item != null)
                {
                    item.ItemQuantity -= (Int16)(s.SupplyQuantity);
                    db.SaveChanges();
                }
            }

            ModelContainer modelcontainer = new ModelContainer();

            modelcontainer.Medicine = db.Items.Where(i => i.ItemType == "Medicine").Where(i => i.deleted == "0").ToList();
            modelcontainer.Utensil = db.Items.Where(i => i.ItemType == "Utensil").Where(i => i.deleted == "0").ToList();

            ViewBag.CriticalStock = db.Items.Where(i => i.ItemQuantity <= critical_stock_value).Where(i => i.ItemQuantity > 0).Where(i => i.deleted == "0").Count();
            ViewBag.OutOfStock = db.Items.Where(i => i.ItemQuantity == 0).Where(i => i.deleted == "0").Count();
            ViewBag.ItemsUsed = db.MedCheckItems.Where(s => s.returned == 0).Where(s => s.Item.ItemType == "Utensil").GroupBy(s => s.ItemID).Select(g => new { ItemID = g.Key, QuantityUsed = g.Sum(ss => ss.Quantity) }).Count();
            ViewBag.ExpiredStocks = db.Supplies.Where(s => s.ExpirationDate <= DateTime.Now).Count();

            modelcontainer.CriticalStock = db.Items.Where(i => i.ItemQuantity <= critical_stock_value).Where(i => i.ItemQuantity > 0).Where(i => i.deleted == "0").ToList();
            modelcontainer.OutOfStock = db.Items.Where(i => i.ItemQuantity == 0).Where(i => i.deleted == "0").ToList();
            modelcontainer.MedCheckItem = db.MedCheckItems.SqlQuery("SELECT MedCheckItems.* FROM dbo.MedCheckItems, dbo.MedChecks WHERE ItemID LIKE 'UT%' AND returned = 0 AND MedCheckItems.MedCheckID = MedChecks.MedCheckID AND MedChecks.Time_out BETWEEN DATEADD(DAY, -7, GETDATE()) AND GETDATE()").ToList();
            modelcontainer.SupplyList = db.Supplies.Where(s => s.ExpirationDate <= DateTime.Now).ToList();

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
        public ActionResult AddSupply(string id)
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
                supply.removed = 0;
                db.Supplies.Add(supply);
                db.SaveChanges();

                var result = db.Items.SingleOrDefault(it => it.ItemID == supply.ItemID);
                if (result != null)
                {
                    result.ItemQuantity = (Int16)(result.ItemQuantity + supply.SupplyQuantity);
                    db.SaveChanges();
                }

                var supply2 = db.Supplies.Where(s => s.ExpirationDate <= DateTime.Now).Where(s => s.removed == 0).GroupBy(s => s.ItemID).Select(g => new { ItemID = g.Key, SupplyQuantity = g.Sum(ss => ss.SupplyQuantity) }).ToList();

                foreach (var s in supply2)
                {
                    var supplies = db.Supplies.Where(sp => sp.ItemID == s.ItemID).Where(sp => sp.ExpirationDate <= DateTime.Now).ToList();
                    if (supplies != null)
                    {
                        foreach (var x in supplies)
                        {
                            x.removed = 1;
                            db.SaveChanges();
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(s.ItemID + ' ' + s.SupplyQuantity);
                    var item = db.Items.SingleOrDefault(i => i.ItemID == s.ItemID);
                    if (item != null)
                    {
                        item.ItemQuantity -= (Int16)(s.SupplyQuantity);
                        db.SaveChanges();
                    }
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
            ModelContainer container = new ModelContainer();
            container.Supply = db.Supplies.Find(id);
            if (container.Supply == null)
            {
                return HttpNotFound();
            }
            if (TempData.ContainsKey("error"))
            {
                if (TempData["error"].Equals(true))
                {
                    ViewBag.ErrorMessage = "Change Reason is required!";
                }else
                {
                    ViewBag.ErrorMessage = "";
                }
            }
            return View(container);
        }

        // POST: Inventory/EditSupply/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSupply([Bind(Include = "SupplyID,ItemID,SupplyQuantity,ReceivedDate,ExpirationDate")] Supply supply, int oldSupplyQuantity, string ChangeReason)
        {
            if (ModelState.IsValid)
            {
                if(ChangeReason != "")
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

                    SupplyChanges supplychanges = new SupplyChanges();
                    supplychanges.SupplyID = supply.SupplyID;
                    supplychanges.DateChange = DateTime.Now;
                    supplychanges.ChangeReason = oldSupplyQuantity + " -> " + supply.SupplyQuantity + ": " + ChangeReason;

                    db.SupplyChanges.Add(supplychanges);
                    db.SaveChanges();

                    return RedirectToAction("Supply", new { id = supply.ItemID });
                }
                else
                {
                    TempData["error"] = true;
                    return RedirectToAction("EditSupply", new { id = supply.SupplyID });
                }
            }
            return RedirectToAction("Supply", new { id = supply.ItemID });
        }

        //GET: Inventory/ViewEditsSupply/id
        public ActionResult ViewEditsSupply(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = db.SupplyChanges.FirstOrDefault(sc => sc.SupplyID == id);
            if(result != null)
            {
                ViewBag.ItemName = result.Supply.Item.ItemName;
            }
            return View(db.SupplyChanges.Where(sc => sc.SupplyID == id).ToList());
        }

        //POST: Inventory/ReturnItem
        public ActionResult ReturnItem(int? MedCheckID, string ItemID, int? Quantity)
        {
            if(MedCheckID == null || ItemID == null || Quantity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = db.MedCheckItems.Where(mci => mci.MedCheckID == MedCheckID).Where(mci => mci.ItemID == ItemID).FirstOrDefault();
            if(result != null)
            {
                result.returned = 1;
                db.SaveChanges();

                var items = db.Items.FirstOrDefault(i => i.ItemID == ItemID);
                if(items != null)
                {
                    items.ItemQuantity += (Int16)Quantity;
                    db.SaveChanges();
                }
            }
            return Json(new { status = true });
        }
    }
}