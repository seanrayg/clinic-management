using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using clinic_management.Models;
using System.Data.SqlClient;

namespace clinic_management.Controllers
{
    public class MedChecksController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();


        // GET: MedChecks
        public ActionResult Index()
        {
            var medChecks = db.MedChecks.Include(m => m.Patient).Include(m => m.Staff).Where(p => p.deleted == "0");
            
            return View(medChecks.ToList());
        }

        // GET: MedChecks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedCheck medCheck = db.MedChecks.Find(id);
            if (medCheck == null)
            {
                return HttpNotFound();
            }
            return View(medCheck);
        }

        // GET: MedChecks/Create
        public ActionResult Create()
        {
            var staff = db.Staffs.Where(x => (x.UserTypeID != 1) && (x.UserTypeID != 3))
                        .Select(s => new
                        {
                            Text = s.StaffFirst + " " + s.StaffLast,
                            Value = s.StaffID
                        })
                        .ToList();
            ViewBag.ddlStaff = new SelectList(staff, "Value", "Text");

            var patient = db.Patients
            .Select(s => new
            {
                Text = s.PatientFirst + " " + s.PatientMid + " " + s.PatientLast,
                Value = s.PatientID
            })
            .ToList();
            ViewBag.ddlPatient = new SelectList(patient, "Value", "Text");

            
            //ViewBag.staffName = Session["fname"].ToString() + Session["lname"].ToString();
            //ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatientLast");
            //ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "StaffLast");
            return View();
        }

        // POST: MedChecks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedCheckID,StaffID,PatientID,DateTimeOfVisit,Complaint,Diagnosis,Treatment,Remarks,MedCheckType,MedCheckStatus,deleted")] MedCheck medCheck)
        {
            if (ModelState.IsValid)
            {
                medCheck.deleted = "0";
                medCheck.Diagnosis = " ";
                medCheck.Treatment = " ";
                medCheck.Remarks = " ";
                medCheck.MedCheckStatus = '1';

                db.MedChecks.Add(medCheck);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatientLast", medCheck.PatientID);
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "StaffLast", medCheck.StaffID);
            return View(medCheck);
        }

        // GET: MedChecks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedCheck medCheck = db.MedChecks.Find(id);
            if (medCheck == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatientLast", medCheck.PatientID);
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "StaffLast", medCheck.StaffID);
            return View(medCheck);
        }

        // POST: MedChecks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedCheckID,StaffID,PatientID,DateTimeOfVisit,Complaint,Diagnosis,Treatment,Remarks,MedCheckType,MedCheckStatus,deleted")] MedCheck medCheck)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medCheck).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatientLast", medCheck.PatientID);
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "StaffLast", medCheck.StaffID);
            return View(medCheck);
        }

        // GET: MedChecks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedCheck medCheck = db.MedChecks.Find(id);
            if (medCheck == null)
            {
                return HttpNotFound();
            }
            return View(medCheck);
        }

        // POST: MedChecks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //MedCheck medCheck = db.MedChecks.Find(id);
            //db.MedChecks.Remove(medCheck);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            var result = db.MedChecks.SingleOrDefault(ut => ut.MedCheckID == id);
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