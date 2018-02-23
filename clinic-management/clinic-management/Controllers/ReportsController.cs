using clinic_management.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;

namespace clinic_management.Controllers
{
    public class ReportsController : Controller
    {
        private dbClinicManagementEntities db = new dbClinicManagementEntities();

        // GET: Reports
        public ActionResult GenerateLogbook(DateTime? DateFrom, DateTime? DateTo)
        {
            ReportViewer reportViewer = new ReportViewer();
            ReportDataSource rds = new ReportDataSource();
            dsReports _dsrep = new dsReports();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);

            if (DateFrom != null && DateTo != null )
            {
                rds.Name = "DTR";
                var x = db.MedChecks.Include(a => a.Patient).Include(b => b.Patient.PCollege).Where(c => c.DateTimeOfVisit >= DateFrom && c.DateTimeOfVisit <= DateTo);

                foreach (var item in x)
                {
                    _dsrep.DTR.AddDTRRow(item.DateTimeOfVisit.ToShortDateString(), item.Patient.PatientLast + ", " + item.Patient.PatientFirst, item.Patient.PCollege.CollegeName, item.Complaint, item.Treatment, item.Time_in.Value.ToShortTimeString(), item.Time_out.Value.ToShortTimeString(), item.Diagnosis);
                }

                rds.Value = _dsrep.DTR;
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Reports\Logbook.rdlc";

                ReportParameter[] _parameter = new ReportParameter[2];
                _parameter[0] = new ReportParameter("DateFrom", DateFrom.Value.ToShortDateString());
                _parameter[1] = new ReportParameter("DateTo", DateTo.Value.ToShortDateString());

                reportViewer.LocalReport.SetParameters(_parameter);
                reportViewer.LocalReport.DataSources.Add(rds);
                reportViewer.LocalReport.Refresh();
                ViewBag.ReportViewer = reportViewer;
                return View();
            }

            rds.Name = "DTR";

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Reports\Logbook.rdlc";
            rds.Value = _dsrep.MedHistory;
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.Refresh();
            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public ActionResult MedHistory(int? PatientName)
        {
            var patient = db.Patients
            .Select(s => new
            {
                Text = s.PatientFirst + " " + s.PatientMid + " " + s.PatientLast,
                Value = s.PatientID
            })
            .ToList();
            ViewBag.ddlPatient = new SelectList(patient, "Value", "Text");

            ReportViewer reportViewer = new ReportViewer();
            ReportDataSource rds = new ReportDataSource();
            dsReports _dsrep = new dsReports();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);

            if (PatientName != null)
            {
                rds.Name = "MedHistory";
                var x1 = db.MedChecks.Include(a => a.Patient).Include(b => b.Staff).Where(c => c.PatientID == PatientName.Value);
                foreach (var item in x1)
                {
                    _dsrep.MedHistory.AddMedHistoryRow(item.DateTimeOfVisit.ToShortDateString(), item.Complaint, item.Diagnosis, item.Treatment, item.Time_in.Value.ToShortTimeString(), item.Time_out.Value.ToShortTimeString(), item.Staff.StaffLast + ", " + item.Staff.StaffFirst);
                }


                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Reports\MedHistory.rdlc";
                rds.Value = _dsrep.MedHistory;
                reportViewer.LocalReport.DataSources.Add(rds);
                reportViewer.LocalReport.Refresh();
                ViewBag.ReportViewer = reportViewer;

                return View();
            }

            rds.Name = "MedHistory";

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Reports\MedHistory.rdlc";
            rds.Value = _dsrep.MedHistory;
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.Refresh();
            ViewBag.ReportViewer = reportViewer;

            return View();
        }
    }
}