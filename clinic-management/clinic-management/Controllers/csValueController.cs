using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using static clinic_management.Helpers.Sessions;

namespace clinic_management.Controllers
{
    [AuthorizationFilter]
    public class csValueController : Controller
    {
        // GET: csValue
        public ActionResult Index()
        {
            //Get the critical stock
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "/config.conf";

            FileStream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var critical_stock_value = 0;
            using (StreamReader sr = new StreamReader(reader))
            {
                critical_stock_value = int.Parse(sr.ReadToEnd());
            }
            reader.Close();

            ViewBag.CriticalStockValue = critical_stock_value;

            return View();
        }

        //GET: csValue/Change
        public ActionResult Change()
        {
            return View();
        }

        //POST: csValue/Change/newValue
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change(int newValue)
        {
            //Get the critical stock
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "/config.conf";

            FileStream reader = new FileStream(filePath, FileMode.Truncate, FileAccess.Write);
            byte[] bdata = Encoding.Default.GetBytes(newValue.ToString());
            reader.Write(bdata, 0, bdata.Length);
            reader.Close();

            System.Diagnostics.Debug.WriteLine(newValue);
            return RedirectToAction("Index");
        }
    }
}