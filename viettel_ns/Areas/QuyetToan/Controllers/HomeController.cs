using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIETTEL.Areas.QuyetToan.Controllers
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        // GET: QuyetToan/Home
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View();
        }
        // GET: QuyetToan/Home
        public Microsoft.AspNetCore.Mvc.ActionResult BaoCao()
        {
            return View();
        }

    }
}