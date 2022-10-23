using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIETTEL.Areas.QLVonDauTu.Controllers.DanhMucQLVonDauTu
{
    public class DefaultController : Microsoft.AspNetCore.Mvc.Controller
    {
        // GET: QLVonDauTu/Default
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View();
        }
    }
}