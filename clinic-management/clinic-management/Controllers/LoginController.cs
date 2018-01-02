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
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "StaffID, StaffPassword")] Staff staff)
        {
            using (var context = new dbClinicManagementEntities())
            {
                context.Database.Connection.Open();

                var query = from q in context.Staffs
                            where q.StaffID == staff.StaffID && q.StaffPassword == staff.StaffPassword
                            select q;

                if (query.Any())
                {
                    ViewBag.ErrorMessage = "";

                    System.Diagnostics.Debug.WriteLine("Login Success");

                    return RedirectToAction("Index", "Patients");
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