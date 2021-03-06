﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using clinic_management.Models;
using System.Data.SqlClient;
using static clinic_management.Helpers.Sessions;

namespace clinic_management.Controllers
{
    [AuthorizationFilter]
    public class MedChecksController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();


        // GET: MedChecks
        public ActionResult Index()
        {
            string staff = Session["staffid"].ToString();

            if (Convert.ToInt32(Session["usertype"].ToString()) == 3)
            { 
            var medChecks = db.MedChecks.Include(m => m.Patient).Include(m => m.Staff).Where(p => p.deleted == "0");
                return View(medChecks);
            }

            if (Convert.ToInt32(Session["usertype"].ToString()) == 2)
            {
                var medChecks = db.MedChecks.Include(m => m.Patient).Include(m => m.Staff).Where(p => p.deleted == "0").Where(m => m.StaffID == staff);
                return View(medChecks);
            }

            var medChecksAll = db.MedChecks.Include(m => m.Patient).Include(m => m.Staff);
            return View(medChecksAll);
        }

        // GET: MedChecks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelContainer container = new ModelContainer();
            container.medcheck = db.MedChecks.Find(id);

            //string strTreatment = container.medcheck.Treatment;
            //string[] arrMedicine = strTreatment.Split(',');

            //if(arrMedicine.Length == 1)
            //{
            //    string[] arrMedDetails = arrMedicine[0].Split('-');
            //    container.ItemList = db.Items.Where(i => i.ItemName == arrMedDetails[1]).ToList();
            //}else if(arrMedicine.Length == 2)
            //{
            //    string[] arrMedDetails;
            //    for(var i = 0; i < arrMedicine.Length; i++)
            //    {
            //        arrMedDetails[i] = arrMedicine[i].Split('-');
            //    }
            //}

            container.ItemList = db.Items.ToList();
            container.MedCheckItem = db.MedCheckItems.Where(mi => mi.MedCheckID == id).ToList();
            if (container == null)
            {
                return HttpNotFound();
            }
            return View(container);
        }

        // POST: MedChecks/Checkout
        public ActionResult Checkout(int MedCheckID, string[][] arrData)
        {
            if (arrData == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            for(var i = 0; i < arrData.Length; i++)
            {
                if(arrData[i][3].ToString() != "")
                {
                    System.Diagnostics.Debug.WriteLine(MedCheckID + " " + arrData[i][0] + " " + arrData[i][3]);

                    MedCheckItem medcheckitem = new MedCheckItem();
                    medcheckitem.MedCheckID = MedCheckID;
                    medcheckitem.ItemID = arrData[i][0].ToString();
                    medcheckitem.Quantity = int.Parse(arrData[i][3].ToString());
                    db.MedCheckItems.Add(medcheckitem);
                    db.SaveChanges();

                    var result = db.Items.Find(medcheckitem.ItemID);
                    if (result != null)
                    {
                        result.ItemQuantity -= (Int16)(medcheckitem.Quantity);
                        db.SaveChanges();
                    }

                    var medcheck = db.MedChecks.Find(MedCheckID);
                    if(result != null)
                    {
                        medcheck.MedCheckStatus = 2;
                        db.SaveChanges();
                    }
                }
            }

            return Json(new { status = true });
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
                medCheck.Time_in = DateTime.Now;
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
            ViewBag.Items = new SelectList(db.Items, "ItemID", "ItemName");

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
                if (Convert.ToInt32(Session["usertype"].ToString()) == 3)
                {
                    MedCheck md = db.MedChecks.Find(medCheck.MedCheckID);
                    md.StaffID = medCheck.StaffID;
                    md.DateTimeOfVisit = medCheck.DateTimeOfVisit;
                    md.MedCheckType = medCheck.MedCheckType;
                    md.Complaint = medCheck.Complaint;
                    db.Entry(md).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (Convert.ToInt32(Session["usertype"].ToString()) == 2)
                {
                    MedCheck md2 = db.MedChecks.Find(medCheck.MedCheckID) ;
                    md2.StaffID = medCheck.StaffID;
                    md2.DateTimeOfVisit = medCheck.DateTimeOfVisit;
                    md2.MedCheckType = medCheck.MedCheckType;
                    md2.Complaint = medCheck.Complaint;
                    md2.Diagnosis = medCheck.Diagnosis;
                    md2.Treatment = medCheck.Treatment;
                    md2.Remarks = medCheck.Remarks;
                    md2.MedCheckStatus = medCheck.MedCheckStatus;
                    md2.Time_out = DateTime.Now;
                    db.Entry(md2).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
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