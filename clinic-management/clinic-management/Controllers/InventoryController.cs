﻿using System;
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
        public ActionResult Create([Bind(Include = "ItemID,ItemName,ItemQuantity,ItemType")] Item item)
        {
            if (ModelState.IsValid)
            {
                item.ItemQuantity = "0";
                item.deleted = "0";

                db.Items.Add(item);
                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine("Medicine Added");

                return RedirectToAction("Index");
            }

            System.Diagnostics.Debug.WriteLine("Medicine Not Added");

            return RedirectToAction("Index");
        }

        public string testing(string value)
        {
            var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = jsonSerializer.Serialize(value);
        }
    }
}