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
            reportViewer.Width = Unit.Percentage(75);
            reportViewer.Height = Unit.Percentage(100);

            var name = db.Patients.FirstOrDefault(b => b.PatientID == b.PatientID);

            if (PatientName != null)
            {
                rds.Name = "MedHistory";
                var x = db.MedChecks.Include(a => a.Patient).Include(b => b.Staff).Where(c => c.PatientID == PatientName);
                var y = db.Patients.Find(PatientName);

                foreach (var item in x)
                {
                    _dsrep.MedHistory.AddMedHistoryRow(item.DateTimeOfVisit.ToShortDateString(), item.Complaint, item.Diagnosis, item.Treatment, item.Time_in.Value.ToShortTimeString(), item.Time_out.Value.ToShortTimeString(), item.Staff.StaffLast + ", " + item.Staff.StaffFirst);
                }

                rds.Value = _dsrep.MedHistory;
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Reports\MedHistory.rdlc";

                ReportParameter[] _parameter = new ReportParameter[2];
                _parameter[0] = new ReportParameter("PatientName", y.PatientFirst + " " + y.PatientMid + " " + y.PatientLast);
                _parameter[1] = new ReportParameter("College", y.PCollege.CollegeName);

                reportViewer.LocalReport.SetParameters(_parameter);
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

        public ActionResult ExcuseLetter(int? PatientName, DateTime? CheckUpDate)
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
            reportViewer.Width = Unit.Percentage(75);
            reportViewer.Height = Unit.Percentage(100);

            var name = db.Patients.FirstOrDefault(b => b.PatientID == b.PatientID);

            if (PatientName != null)
            {
                rds.Name = "ExcuseLetter";
                var x = db.MedChecks.Include(a => a.Patient).Include(b => b.Staff).Where(c => c.PatientID == PatientName);
                var y = db.Patients.Find(PatientName);

                foreach (var item in x)
                {
                    _dsrep.ExcuseLetter.AddExcuseLetterRow(item.Diagnosis, item.Staff.StaffFirst + " " + item.Staff.StaffLast, item.Time_in.Value.ToShortTimeString(), item.Time_out.Value.ToShortTimeString());
                }

                rds.Value = _dsrep.ExcuseLetter;
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Reports\ExcuseLetter.rdlc";

                ReportParameter[] _parameter = new ReportParameter[3];
                _parameter[0] = new ReportParameter("PatientName", y.PatientFirst + " " + y.PatientMid + " " + y.PatientLast);
                _parameter[1] = new ReportParameter("CheckUpDate", CheckUpDate.Value.ToShortDateString());
                _parameter[2] = new ReportParameter("DateToday", DateTime.Now.ToShortDateString());

                reportViewer.LocalReport.SetParameters(_parameter);
                reportViewer.LocalReport.DataSources.Add(rds);
                reportViewer.LocalReport.Refresh();
                ViewBag.ReportViewer = reportViewer;

                return View();
            }

            rds.Name = "ExcuseLetter";

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Reports\ExcuseLetter.rdlc";
            rds.Value = _dsrep.ExcuseLetter;
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.Refresh();
            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public ActionResult Clearance_PF(int? PatientName)
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
            reportViewer.Width = Unit.Percentage(75);
            reportViewer.Height = Unit.Percentage(100);

            var name = db.Patients.FirstOrDefault(b => b.PatientID == b.PatientID);

            if (PatientName != null)
            {
                rds.Name = "Clearance_PF";
                string staff = Session["staffid"].ToString();

                var x = db.MedChecks.Include(a => a.Patient).Include(b => b.Staff).Where(c => c.PatientID == PatientName).Where(m => m.StaffID == staff);
                var y = db.Patients.Find(PatientName);

                foreach (var item in x)
                {
                    _dsrep.Clearance_PF.AddClearance_PFRow(item.Patient.PatientFirst + " " + " " + item.Patient.PatientMid + " " + item.Patient.PatientLast,item.Staff.StaffFirst + " " + item.Staff.StaffLast);
                }

                rds.Value = _dsrep.Clearance_PF;
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Reports\Clearance_PF.rdlc";

                ReportParameter[] _parameter = new ReportParameter[1];
                _parameter[0] = new ReportParameter("IssuedDate", DateTime.Now.ToShortDateString());

                reportViewer.LocalReport.SetParameters(_parameter);
                reportViewer.LocalReport.DataSources.Add(rds);
                reportViewer.LocalReport.Refresh();
                ViewBag.ReportViewer = reportViewer;

                return View();
            }

            rds.Name = "Clearance_PF";

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Views\Reports\Clearance_PF.rdlc";
            rds.Value = _dsrep.Clearance_PF;
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.Refresh();
            ViewBag.ReportViewer = reportViewer;

            return View();
        }
    }
}