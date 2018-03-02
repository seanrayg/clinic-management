using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using clinic_management.Models;
using static clinic_management.Helpers.Sessions;

namespace clinic_management.Controllers
{
    [AuthorizationFilter]
    public class PatientsController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();

        // GET: Patients
        public ActionResult Index()
        {
            var patients = db.Patients.Include(p => p.PatientType).Include(p => p.PCollege).Where(p => p.deleted == "0");
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
            ViewBag.TypeID = new SelectList(db.PatientTypes, "TypeID", "TypeName");
            ViewBag.CollegeID = new SelectList(db.PColleges, "CollegeID", "CollegeName");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientID,PatientLast,PatientFirst,PatientMid,PatientGender,PatientBDate,PatientAddrss,TypeID,PatientClass,CollegeID")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.deleted = "0";
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeID = new SelectList(db.PatientTypes, "TypeID", "TypeName", patient.TypeID);
            ViewBag.CollegeID = new SelectList(db.PColleges, "CollegeID", "CollegeCode", patient.CollegeID);
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
            ViewBag.TypeID = new SelectList(db.PatientTypes, "TypeID", "TypeName", patient.TypeID);
            ViewBag.CollegeID = new SelectList(db.PColleges, "CollegeID", "CollegeName", patient.CollegeID);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientID,PatientLast,PatientFirst,PatientMid,PatientGender,PatientBDate,PatientAddrss,TypeID,PatientClass,CollegeID,deleted")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeID = new SelectList(db.PatientTypes, "TypeID", "TypeName", patient.TypeID);
            ViewBag.CollegeID = new SelectList(db.PColleges, "CollegeID", "CollegeCode", patient.CollegeID);
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
            //Patient patient = db.Patients.Find(id);
            //db.Patients.Remove(patient);
            //db.SaveChanges();

            var result = db.Patients.SingleOrDefault(ut => ut.PatientID == id);
            if (result != null)
            {
                result.deleted = "1";
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: PatientTypes/Recover
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                return View(db.Patients.ToList().Where(p => p.deleted == "1"));
            }

            var result = db.Patients.SingleOrDefault(p => p.PatientID == id);
            if (result != null)
            {
                result.deleted = "0";
                db.SaveChanges();
            }

            return View(db.Patients.ToList().Where(p => p.deleted == "1"));
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
