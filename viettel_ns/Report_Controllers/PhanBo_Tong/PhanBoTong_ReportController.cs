﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIETTEL.Report_Controllers.PhanBo_Tong
{
    public class PhanBoTong_ReportController : Microsoft.AspNetCore.Mvc.Controller
    {
        //
        // GET: /PhanBo_Report/

        public string sViewPath = "~/Report_Views/PhanBo_Tong/";
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View(sViewPath + "Report_Index.aspx");
        }

    }
}
