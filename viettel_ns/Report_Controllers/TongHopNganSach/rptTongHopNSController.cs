﻿using System.Web.Mvc;

namespace VIETTEL.Controllers
{
    public class rptTongHopNSController : AppController
    {
        //
        // GET: /Report/
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            string view = "~/Views/Report_Views/TongHopNganSach/Index.cshtml";
            return View(view);
        }
    }
}
