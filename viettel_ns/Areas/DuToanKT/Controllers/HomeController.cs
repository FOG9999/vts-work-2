using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIETTEL.Areas.DuToanKT.Controllers
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        // GET: DuToan/Home
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View();
        }
    }
}