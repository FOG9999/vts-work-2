using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using KienNghi.Flexcel;
using KienNghi.Models;
using Utilities;

namespace KienNghi.Controllers
{
    public class ReportBaseController : BaseController
    {
        public ReportBaseController()
        {
            //string lang = "vi-VN";
            //System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            //System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
        }

        //protected virtual JsonResult ToCheckboxList(ChecklistModel vm)
        //{
        //    var view = "~/Views/Shared/_CheckboxList.cshtml";
        //    var result = HamChung.RenderPartialViewToStringLoad(view, vm, this);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //protected virtual JsonResult ToCheckboxListJson(ChecklistModel vm)
        //{
        //    var view = "~/Views/Shared/_CheckboxList.cshtml";
        //    var result = HamChung.RenderPartialViewToStringLoad(view, vm, this);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}



        protected virtual JsonResult ds_ToIn(int count = 0, int colCount = 6, string id = "To")
        {
            var list = new List<SelectListItem>();

            if (count > 0)
            {
                var count_to = count <= colCount ? 1 : (count % colCount) == 0 ? (count / colCount) : (count / colCount) + 1;
                for (int i = 1; i <= count_to; i++)
                {
                    var to = i.ToString();
                    list.Add(new SelectListItem()
                    {
                        Value = to,
                        Text = "Tờ " + to,
                    });
                }
            }

            var vm = new ChecklistModel(id, new SelectList(list, "Value", "Text"));
            return ToCheckboxList(vm);
        }

        protected JsonResult Ds_ToIn(DataTable dt)
        {
            return ds_ToIn(dt.Rows.Count);
        }

        protected virtual string ViewFolder()
        {
            return null;
        }

        protected virtual string ViewFileName()
        {
            return $"{ViewFolder()}/{this.ControllerName()}.cshtml";
        }
    }

    public class FlexcelReportController : ReportBaseController
    {
        protected virtual ActionResult Print(ExcelFile xls, string ext = "pdf", string filename = null)
        {
            filename = filename ?? string.Format("{0}_{1}.{2}", this.ControllerName(), DateTime.Now.GetTimeStamp(), ext);
            return
                string.IsNullOrWhiteSpace(ext) || ext == "pdf" ?
                    xls.ToPdfContentResult(filename) :
                    xls.ToFileResult(filename);
        }


        protected virtual ActionResult PrintEmpty(string msg)
        {
            var file = Server.MapPath(@"~/Report_ExcelFrom/_core/rptEmpty.xls");
            var xls = new XlsFile();
            xls.Open(file);

            var fr = new FlexCelReport();
            fr.UseCommonValue()
                .SetValue(new { ThongBao = msg })
                .Run(xls);
            return Print(xls);
        }

    }
}
       
