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
    public class PCollegesController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();

        // GET: PColleges
        public ActionResult Index()
        {
            return View(db.PColleges.ToList());
        }

        // GET: PColleges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PCollege pCollege = db.PColleges.Find(id);
            if (pCollege == null)
            {
                return HttpNotFound();
            }
            return View(pCollege);
        }

        // GET: PColleges/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PColleges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CollegeID,CollegeCode,CollegeName,deleted")] PCollege pCollege)
        {
            if (ModelState.IsValid)
            {
                db.PColleges.Add(pCollege);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pCollege);
        }

        // GET: PColleges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PCollege pCollege = db.PColleges.Find(id);
            if (pCollege == null)
            {
                return HttpNotFound();
            }
            return View(pCollege);
        }

        // POST: PColleges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CollegeID,CollegeCode,CollegeName,deleted")] PCollege pCollege)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pCollege).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pCollege);
        }

        // GET: PColleges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PCollege pCollege = db.PColleges.Find(id);
            if (pCollege == null)
            {
                return HttpNotFound();
            }
            return View(pCollege);
        }

        // POST: PColleges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PCollege pCollege = db.PColleges.Find(id);
            db.PColleges.Remove(pCollege);
            db.SaveChanges();
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
