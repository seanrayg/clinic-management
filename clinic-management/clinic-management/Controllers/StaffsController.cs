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
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeDesc");
            return View();
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
        public ActionResult Edit([Bind(Include = "StaffID,StaffLast,StaffFirst,StaffMid,StaffGender,StaffPassword,StaffJoinedDate,UserTypeID")] Staff staff, string confirm_password)
        {
            if (ModelState.IsValid)
            {
                if(staff.StaffPassword == confirm_password)
                {
                    db.Entry(staff).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }else
                {
                    ViewBag.ErrorMessage = "Passwords do not match";
                    ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeDesc", staff.UserTypeID);
                    return View(staff);
                }
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
    }
}
