using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using VIETTEL.Models;
using System.IO;
//using DomainModel.Controls;
//using DomainModel.Abstract;
//using System.Collections.Specialized;
//using VIETTEL.Models;

namespace VIETTEL.Controllers
{
    [HandleError]
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        [Authorize]
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            String MaND = User.Identity.Name;
            NguoiDungCauHinhModels.MaNguoiDung = MaND;
            NguoiDungCauHinhModels.iNamLamViec = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec");
            return View();
        }

        [Authorize]
        public Microsoft.AspNetCore.Mvc.ActionResult About()
        {
            return View();
        }

        public Microsoft.AspNetCore.Mvc.ActionResult Blank()
        {
            return View();
        }

        public Microsoft.AspNetCore.Mvc.ActionResult CloneLayout()
        {
            return View();
        }

        public string RenderViewToString() 
        { 
            //if (model == null) return string.Empty; 
            //ViewData.Model = model; 
            using (StringWriter sw = new StringWriter()) 
            { 
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, "CloneLayout", "_LayoutWebNew"); 
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw); 
                viewResult.View.Render(viewContext, sw); return sw.GetStringBuilder().ToString(); } 
        }
    }
}
