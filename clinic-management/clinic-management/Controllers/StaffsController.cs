﻿using System;
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
    public class StaffsController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();

        // GET: Staffs
        public ActionResult Index()
        {
            var staffs = db.Staffs.Include(s => s.UserType).Where(s => s.deleted == "0");
            return View(staffs.ToList());
        }

        // GET: Staffs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Staffs/Create
        public ActionResult Create()
        {
            var intAdminCount = db.Staffs.Where(s => s.UserType.TypeDesc == "Admin").Count();
            ViewBag.AdminID = GetID("AD", intAdminCount);

            var intNurseCount = db.Staffs.Where(s => s.UserType.TypeDesc == "Nurse").Count();
            ViewBag.NurseID = GetID("NS", intNurseCount);

            var intDoctorCount = db.Staffs.Where(s => s.UserType.TypeDesc == "Doctor").Count();
            ViewBag.DoctorID = GetID("DC", intDoctorCount);

            var intReceptionistCount = db.Staffs.Where(s => s.UserType.TypeDesc == "Receptionist").Count();
            ViewBag.ReceptionistID = GetID("NR", intReceptionistCount);

            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeDesc");
            return View();
        }

        public string GetID(string format, int count)
        {
            string id = format;
            int idCount = count + 1;

            for(int i = idCount.ToString().Count(); i < 4; i++)
            {
                id += '0';
            }

            return id + idCount;
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffID,StaffLast,StaffFirst,StaffMid,StaffGender,StaffPassword,StaffJoinedDate,UserTypeID")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                staff.deleted = "0";
                db.Staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeDesc", staff.UserTypeID);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeDesc", staff.UserTypeID);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffID,StaffLast,StaffFirst,StaffMid,StaffGender,StaffPassword,StaffJoinedDate,UserTypeID,deleted")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeDesc", staff.UserTypeID);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //Staff staff = db.Staffs.Find(id);
            //db.Staffs.Remove(staff);
            //db.SaveChanges();

            var result = db.Staffs.SingleOrDefault(s => s.StaffID == id);
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

        // GET: Staffs/ChangePassword
        public ActionResult ChangePassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "StaffID,StaffPassword")] Staff staff, string confirm_password)
        {
            if (staff.StaffPassword == confirm_password)
            {
                var result = db.Staffs.SingleOrDefault(s => s.StaffID == staff.StaffID);
                if (result != null)
                {
                    result.StaffPassword = confirm_password;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Passwords do not match";
                ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeDesc", staff.UserTypeID);
                return View(staff);
            }
        }

        // GET: PatientTypes/Recover
        public ActionResult Recover(string id)
        {
            if (id == null)
            {
                return View(db.Staffs.ToList().Where(s => s.deleted == "1"));
            }

            var result = db.Staffs.SingleOrDefault(s => s.StaffID == id);
            if (result != null)
            {
                result.deleted = "0";
                db.SaveChanges();
            }

            return View(db.Staffs.ToList().Where(s => s.deleted == "1"));
        }
    }
}
