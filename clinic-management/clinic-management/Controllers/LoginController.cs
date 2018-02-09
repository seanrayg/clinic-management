using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using clinic_management.Models;

namespace ClinicManagement.Controllers
{
    
    public class LoginController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "StaffID, StaffPassword")] Staff staff)
        {
            var usertype = db.Staffs.Where(a => a.StaffID == staff.StaffID && a.StaffPassword == staff.StaffPassword).FirstOrDefault();
            
            using (var context = new dbClinicManagementEntities())
            {
                context.Database.Connection.Open();

                var query = from q in context.Staffs
                            where q.StaffID == staff.StaffID && q.StaffPassword == staff.StaffPassword
                            select q;



                if (query.Any())
                {
                    try
                    {
                        Session["usertype"] = usertype.UserTypeID;
                        Session["fname"] = usertype.StaffFirst;
                        Session["lname"] = usertype.StaffLast;
                    }
                    catch (Exception e) { }
                    System.Diagnostics.Debug.WriteLine("Login Success");

                    if (Convert.ToInt32(usertype.UserTypeID.ToString()) == 2 || Convert.ToInt32(usertype.UserTypeID.ToString()) == 3)
                    {
                        return RedirectToAction("Index", "Patients");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Staffs");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Credentials";
                }
            }

            return View();
        }
    }
}