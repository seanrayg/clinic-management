using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using clinic_management.Models;

namespace ClinicManagement.Controllers
{
    public class PatientsController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();

        // GET: Patients
        public ActionResult Index()
        {
            var patients = db.Patients.Include(p => p.PCollege).Include(p => p.PatientType);
            return View(patients.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            ViewBag.CollegeID = new SelectList(db.PColleges, "CollegeID", "CollegeName");
            ViewBag.TypeID = new SelectList(db.PatientTypes, "TypeID", "TypeName");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientID,PatientLast,PatientFirst,PatientMid,PatientGender,PatientBDate,PatientAddrss,PatientClass,CollegeID,TypeID")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CollegeID = new SelectList(db.PColleges, "CollegeID", "CollegeCode", patient.CollegeID);
            ViewBag.TypeID = new SelectList(db.PatientTypes, "TypeID", "TypeName", patient.TypeID);
            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.CollegeID = new SelectList(db.PColleges, "CollegeID", "CollegeName", patient.CollegeID);
            ViewBag.TypeID = new SelectList(db.PatientTypes, "TypeID", "TypeName", patient.TypeID);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientID,PatientLast,PatientFirst,PatientMid,PatientGender,PatientBDate,PatientAddrss,PatientClass,CollegeID,TypeID")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CollegeID = new SelectList(db.PColleges, "CollegeID", "CollegeName", patient.CollegeID);
            ViewBag.TypeID = new SelectList(db.PatientTypes, "TypeID", "TypeName", patient.TypeID);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
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
