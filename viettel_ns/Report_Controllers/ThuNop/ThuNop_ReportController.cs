using System.Web.Mvc;

namespace VIETTEL.Report_Controllers.ThuNop {
    public class ThuNop_ReportController : Microsoft.AspNetCore.Mvc.Controller
    {
        //
        // GET: /ThuNop_Report/
        public string sViewPath = "~/Views/Report_Views/ThuNop/";
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View(sViewPath + "Report_Index.cshtml");
        }

    }
}
