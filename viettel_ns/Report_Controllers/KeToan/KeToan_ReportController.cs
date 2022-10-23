using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIETTEL.Models;
namespace VIETTEL.Report_Controllers.KeToan
{
    public class KeToan_ReportController : Microsoft.AspNetCore.Mvc.Controller
    {
        //
        // GET: /KeToan_Report/

        public string sViewPath = "~/Report_Views/KeToan/";
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                return View(sViewPath + "Report_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

    }
}
