using System.Web.Mvc;

namespace VIETTEL.Report_Controllers.CapPhat
{
    public class CapPhat_ReportController : Microsoft.AspNetCore.Mvc.Controller
    {
        public string sViewPath = "~/Views/Report_Views/CapPhat/";

        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View(sViewPath + "Report_Index.cshtml");
        }
    }
}
