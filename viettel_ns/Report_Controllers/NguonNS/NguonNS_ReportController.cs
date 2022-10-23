using System.Web.Mvc;

namespace VIETTEL.Report_Controllers.NguonNS {
    public class NguonNS_ReportController : Microsoft.AspNetCore.Mvc.Controller
    {        
        public string sViewPath = "~/Report_Views/NguonNS/";
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View(sViewPath + "Report_Index.aspx");
        }

    }
}
