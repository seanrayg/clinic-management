using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static clinic_management.Helpers.Sessions;

namespace clinic_management.Controllers
{
    [AuthorizationFilter]
    public class DeletesController : Controller
    {
        // GET: Deletes
        public ActionResult Admin()
        {
            return View();
        }

        // GET: Deletes
        public ActionResult Regular()
        {
            return View();
        }
    }
}