using System.Web.Mvc;

namespace VIETTEL.Controllers
{
    public class rptTongQuyetToanController : AppController
    {
        //
        // GET: /Report/
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            string view = "~/Views/Report_Views/TongHopNganSach/Index_TongQuyetToan.cshtml";
            return View(view);
        }
    }
}
