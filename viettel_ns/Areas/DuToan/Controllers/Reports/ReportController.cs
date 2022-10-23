using System.Web.Mvc;

namespace VIETTEL.Areas.DuToan.Controllers
{
    public class ReportController : Microsoft.AspNetCore.Mvc.Controller
    {
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View();
        }
        public Microsoft.AspNetCore.Mvc.ActionResult KiemTra()
        {
            return View();
        }
    }
}
