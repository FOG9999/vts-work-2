using System.Web.Mvc;

namespace VIETTEL.Report_Controllers.QuyetToan
{
    public class DuToanBS_ReportController : Microsoft.AspNetCore.Mvc.Controller
    {
        // GET: /DuToanBS_Report/
        public string sViewPath = "~/Views/Report_Views/DuToanBS/";
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View(sViewPath + "Report_Index.cshtml");
        }
    }
}
