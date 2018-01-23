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
    public class PatientTypesController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();

        // GET: PatientTypes
        public ActionResult Index()
        {
            return View(db.PatientTypes.ToList().Where(pt => pt.deleted == "0"));
        }

        // GET: PatientTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientType patientType = db.PatientTypes.Find(id);
            if (patientType == null)
            {
                return HttpNotFound();
            }
            return View(patientType);
        }

        // GET: PatientTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeID,TypeName,deleted")] PatientType patientType)
        {
            if (ModelState.IsValid)
            {
                patientType.deleted = "0";
                db.PatientTypes.Add(patientType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patientType);
        }

        // GET: PatientTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientType patientType = db.PatientTypes.Find(id);
            if (patientType == null)
            {
                return HttpNotFound();
            }
            return View(patientType);
        }

        // POST: PatientTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeID,TypeName,deleted")] PatientType patientType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patientType);
        }

        // GET: PatientTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientType patientType = db.PatientTypes.Find(id);
            if (patientType == null)
            {
                return HttpNotFound();
            }
            return View(patientType);
        }

        // POST: PatientTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //PatientType patientType = db.PatientTypes.Find(id);
            //db.PatientTypes.Remove(patientType);
            //db.SaveChanges();

            var result = db.PatientTypes.SingleOrDefault(pt => pt.TypeID == id);
            if (result != null)
            {
                result.deleted = "1";
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
