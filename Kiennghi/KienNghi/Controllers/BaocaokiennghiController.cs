using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using KienNghi.App_Code;
using System.IO;
using Entities.Models;
using Utilities;
using DataAccess.Busineess;
using ClosedXML.Excel;
using Entities.Objects;
using Utilities.Enums;
using System.Configuration;

namespace KienNghi.Controllers
{
    public class BaocaokiennghiController : BaseController
    {
        //
        // GET: /Baocaokiennghi/
        Log log = new Log();
        KienNghiReport kn_report = new KienNghiReport();
        KiennghiBusineess kiennghi = new KiennghiBusineess();
        Kiennghi_cl _kn = new Kiennghi_cl();
        Funtions func = new Funtions();
        public ActionResult Traloi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                ViewData["opt-coquan"] = Get_Option_Coquanxuly();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Bản tập hợp trả lời kiến nghị cử tri");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult Traloi_word()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                //var list = kn_report.getReportBaoCaoThongKePhuLuc2("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC2", iKyHop, 0);
                ViewData["list"] = _kn.BAOCAO_TK_PHULUC_TRALOI(iKyHop, iDonVi, iLinhVuc);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/force-download;";
                Response.AddHeader("Content-Disposition", "attachment; filename=traloi.doc");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "";
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "File word: Bản tập hợp trả lời kiến nghị cử tri");
                return View("../Home/Error_Exception");
            }
        }
        [HttpPost]
        public ActionResult Ajax_Traloi(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(fc["iKyHop"]);
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(fc["iLinhVuc"]);
                ViewData["list"] = _kn.BAOCAO_TK_PHULUC_TRALOI(iKyHop, iDonVi, iLinhVuc);
                return PartialView("../Ajax/Baocaokiennghi/Traloi");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Bản tập hợp trả lời kiến nghị cử tri");
                throw;
            }

        }
        public ActionResult Phanloai()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                ViewData["opt-coquan"] = Get_Option_Coquanxuly();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Mẫu phân loại sơ bộ");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_phanloai(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(fc["iKyHop"]);
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(fc["iLinhVuc"]);
                //List<KIENNGHIPHULUC5B> dt = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, 0);
                //ViewData["kyhop"] = _kn.Get_TenKyHop(iKyHop) + ", " + _kn.Get_TenKhoaHop_By_IDKyHop(iKyHop);
                ViewData["list"] = _kn.BAOCAO_TK_PHULUC_PHANLOAI(iKyHop, iDonVi, iLinhVuc);
                return PartialView("../Ajax/Baocaokiennghi/Phanloai");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Mẫu phân loại sơ bộ");
                throw;
            }

        }
        public ActionResult Kiennghi_phanloai_word()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                //var list = kn_report.getReportBaoCaoThongKePhuLuc2("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC2", iKyHop, 0);
                ViewData["list"] = _kn.BAOCAO_TK_PHULUC_PHANLOAI(iKyHop, iDonVi, iLinhVuc);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/force-download;";
                Response.AddHeader("Content-Disposition", "attachment; filename=phanloai.doc");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "";
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Word: Mẫu phân loại sơ bộ");
                return View("../Home/Error_Exception");
            }

        }
        public string Get_Option_LinhVuc_By_ID_CoQuan(int id_coquan = 0, int id_linhvuc = 0)
        {
            string str = "<option value='0'>- - - Chưa xác định</option>";
            if (id_coquan == 0)
            {
                UserInfor u_info = GetUserInfor();
                if (u_info.tk_action.is_chuyenvien)
                {
                    var linhvuc = kiennghi.GetAll_LinhVuc_CoQuan_By_IDCoQuan((int)u_info.user_login.IDONVI);
                    return str + _kn.Option_LinhVuc_CoQuan(linhvuc, id_linhvuc);
                    //return "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
                }
                return str;
            }
            else
            {
                var linhvuc = kiennghi.GetAll_LinhVuc_CoQuan_By_IDCoQuan(id_coquan);
                return str + _kn.Option_LinhVuc_CoQuan(linhvuc, id_linhvuc);
            }
        }
        public ActionResult Phuluc2()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                ViewData["opt-coquan"] = Get_Option_Coquanxuly();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Phuluc2");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_phuluc2(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(fc["iKyHop"]);
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(fc["iLinhVuc"]);
                //List<KIENNGHIPHULUC5B> dt = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, 0);
                ViewData["kyhop"] = _kn.Get_TenKyHop(iKyHop) + ", " + _kn.Get_TenKhoaHop_By_IDKyHop(iKyHop);
                ViewData["list"] = _kn.BAOCAO_TK_PHULUC2(iKyHop, iDonVi, iLinhVuc);
                return PartialView("../Ajax/Baocaokiennghi/Kiennghi_2");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Phuluc2");
                throw;
            }

        }
        public ActionResult Kiennghi_phuluc2_excel()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                var list = kn_report.getReportBaoCaoThongKePhuLuc2("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC2",
                                                                    iKyHop, iDonVi, iLinhVuc);
                list = list.OrderBy(x => x.VITRI).ToList();
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Phụ lục 2");
                // Merge a row
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(2).Width = 30;
                wb.Column(3).Width = 20;
                wb.Column(4).Width = 50;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Cell(1, 1).Value = "PHỤ LỤC 2";
                wb.Range(1, 1, 1, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                     .Font.SetBold(true).Font.SetFontSize(14);
                wb.Cell(2, 1).Value = list.Count() + " văn bản pháp luật đã ban hành có nội dung liên quan tới việc tiếp thu, giải quyết kiến nghị của cử tri gửi " +
                     Server.HtmlDecode(_kn.Get_TenKyHop(iKyHop)) + ", " + Server.HtmlDecode(_kn.Get_TenKhoaHop_By_IDKyHop(iKyHop));
                wb.Range(2, 1, 2, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Alignment.SetWrapText(true)
                                                    .Font.SetBold(true).Font.SetFontSize(14);
                wb.Cell(3, 1).Value = "";
                wb.Range(3, 1, 3, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;
                wb.Cell(4, 1).Value = "";

                wb.Cell(5, 1).Value = "STT";
                wb.Cell(5, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 2).Value = "Số hiệu văn bản";
                wb.Cell(5, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 3).Value = "Ngày";
                wb.Cell(5, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 4).Value = "Trích yếu nội dung";
                wb.Cell(5, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                int cell = 6;
                var list_donvi = kiennghi.GetAll_CoQuanByParam(null);
                List<decimal> list_id_donvi = new List<decimal>();
                foreach (var l in list) { if (!list_id_donvi.Contains(l.ID_PARENT)) { list_id_donvi.Add(l.ID_PARENT); } }
                int count0 = 1;
                foreach (var d in list_id_donvi)
                {
                    var kn_donvi = list.Where(x => x.ID_PARENT == d).ToList();
                    wb.Cell(cell, 1).Value = Server.HtmlDecode(count0 + ". " + list_donvi.Where(x => x.ICOQUAN == (int)d).FirstOrDefault().CTEN);
                    wb.Range(cell, 1, cell, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Font.Bold = true;

                    cell++;
                    int count = 1;
                    foreach (var k in kn_donvi)
                    {
                        wb.Cell(cell, 1).Value = count;
                        wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(cell, 2).Value = Server.HtmlDecode(k.SOHIEUVANBAN);
                        wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(cell, 3).Value = func.ConvertDateVN(k.NGAY.ToString());
                        wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(cell, 4).Value = Server.HtmlDecode(k.TRICHYEU);
                        wb.Cell(cell, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                        count++;
                        cell++;
                    }
                    count0++;
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=phuluc2.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: phụ lục 2");
                return View("../Home/Error_Exception");
            }
        }
        public string Get_CheckBox_TruocKyHop(int iTruocKyHop = -1)
        {

            string str = "";
            if (iTruocKyHop == -1)//chưa chọn, mới cập nhật
            {
                if (Request.Cookies["truockyhop"] != null)//chưa có cookie trước đó
                {
                    iTruocKyHop = Convert.ToInt32(Request.Cookies["truockyhop"].Value);
                }
                else// set mặc định là trước kỳ họp
                {
                    iTruocKyHop = 1;
                }
            }
            var hinhthuc = new List<TruocKyHop>
            {
                new TruocKyHop { ten = "Trước kỳ họp", value = 1, class_span = "span5" },
                new TruocKyHop { ten = "Sau kỳ họp", value = 0, class_span = "span4" },
                new TruocKyHop { ten = "Khác", value = 2, class_span = "span3" }
            };
            foreach (var h in hinhthuc)
            {
                string check = "";
                if (h.value == iTruocKyHop) { check = " checked "; }
                str += "<span class='" + h.class_span + "'><input class='nomargin' type='radio' name='iTruocKyHop' " + check + " id='iTruocKyHop' value='" + h.value + "' /> " +
                     "<label for='h_" + h.class_span + "'>" + h.ten + "</label> </span>";
            }
            return str;
        }
        public ActionResult Phuluc9()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-coquan"] = Get_Option_DonViTiepNhan();
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: phụ lục 9");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Kiennghi_phuluc9_excel()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);
                string tenkyhop = "Bản thống kê thời gian nhận được báo cáo tổng hợp ý kiến, kiến nghị của cử tri ";
                if (iTruocKyHop == 1) { tenkyhop += "trước "; } else { tenkyhop += "sau "; }
                tenkyhop += Server.HtmlDecode(_kn.Get_TenKyHop(iKyHop)) + ", " + Server.HtmlDecode(_kn.Get_TenKhoaHop_By_IDKyHop(iKyHop));
                var list = kn_report.getReportBaoCaoThongKePhuLuc7("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC7", iKyHop, iTruocKyHop);
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Phụ lục 6");
                // Merge a rowcheck
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Cell(1, 1).Value = "PHỤ LỤC 9";
                wb.Column(1).Width = 10;
                wb.Column(2).Width = 30;
                wb.Column(3).Width = 20;
                wb.Column(4).Width = 10;
                wb.Column(5).Width = 30;
                wb.Column(6).Width = 20;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Range(1, 1, 1, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(2, 1).Value = tenkyhop;
                wb.Range(2, 1, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;

                wb.Cell(5, 1).Value = "STT";
                wb.Cell(5, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 2).Value = "Tỉnh/Thành phố";
                wb.Cell(5, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 3).Value = "Ngày nhận BC";
                wb.Cell(5, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true);
                wb.Cell(5, 4).Value = "STT";
                wb.Cell(5, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 5).Value = "Tỉnh/Thành phố";
                wb.Cell(5, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 6).Value = "Ngày nhận BC";
                wb.Cell(5, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true);
                int cell = 6;
                list = list.OrderBy(x => x.TEN_TINHTHANH).ToList();
                List<decimal> list_id_donvi = new List<decimal>();
                //foreach (var l in list)
                //{
                //    if (!list_id_donvi.Contains((int)l.ID_DONVI)) { list_id_donvi.Add((int)l.ID_DONVI); }
                //}
                int count = 1;
                foreach (var d in list)
                {
                    if (!list_id_donvi.Contains(d.ID_DONVI))
                    {
                        list_id_donvi.Add(d.ID_DONVI);
                        int x = 1;
                        if (count > 1)
                        {//chưa cuối
                            if (count % 2 == 1)
                            {
                                //str += "</tr><tr>";
                                cell++;
                            }
                            else
                            {
                                x = 4;
                            }
                        }
                        wb.Cell(cell, x).Value = count;
                        wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                   .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                   .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        x++;
                        wb.Cell(cell, x).Value = Server.HtmlDecode(d.TEN_TINHTHANH);
                        wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);
                        x++;
                        string ngay = String.Format("{0:dd/MM}", Convert.ToDateTime(d.NGAYNHAN));
                        wb.Cell(cell, x).Value = ngay;
                        wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                        .DateFormat.SetFormat("dd/MM");
                        //x++;
                        //cell++;
                        if (count > 1 && count % 2 == 0)
                        {
                            cell++;
                        }
                        count++;
                    }
                    /*
                    //int x = 1;
                    KIENNGHIPHULUC7 kn = list.Where(k => k.ID_DONVI == d).FirstOrDefault();                    
                    wb.Cell(cell, 1).Value = count;
                    wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    //x++;
                    wb.Cell(cell, 2).Value = Server.HtmlDecode(kn.TEN_TINHTHANH);
                    wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);
                    //x++;
                    string ngay = String.Format("{0:dd/MM}", Convert.ToDateTime(kn.NGAYNHAN));
                    wb.Cell(cell, 3).Value = ngay;
                    wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                    .DateFormat.SetFormat("dd/MM");
                    cell++;
                    */
                    //x++;

                }


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=phuluc9.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: phụ lục 9");
                return View("../Home/Error_Exception");
            }
        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_phuluc9(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(fc["iKyHop"]);
                int iTruocKyHop = Convert.ToInt32(fc["iTruocKyHop"]);
                ViewData["list"] = _kn.BAOCAO_TK_PHULUC9(iKyHop, iTruocKyHop);
                return PartialView("../Ajax/Baocaokiennghi/Kiennghi_9");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax: phụ lục 9"); throw;
            }

        }
        public ActionResult Phuluc5b()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-linhvuc"] = Get_Option_DonVi_Linhvuc();
                ViewData["opt-coquan"] = Get_Option_DonViTiepNhan();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: phụ lục 9");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_phuluc5b(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(fc["iKyHop"]);
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(fc["iLinhVuc"]);
                //List<KIENNGHIPHULUC5B> dt = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, 0);

                ViewData["list"] = _kn.BAOCAO_TK_PHULUC5B(iKyHop, iDonVi, iLinhVuc);
                return PartialView("../Ajax/Baocaokiennghi/Kiennghi_5b");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax: phụ lục 5b");
                throw;
            }

        }
        public ActionResult Kiennghi_phuluc5B_excel()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                var list = kn_report.getReportBaoCaoThongKePhuLuc5B("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC5B", iKyHop, iDonVi, iLinhVuc);
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Phụ lục5b");
                // Merge a row
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Cell(1, 1).Value = "PHỤ LỤC 5B";
                wb.Column(2).Width = 80;
                wb.Column(3).Width = 20;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Range(1, 1, 1, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Font.SetFontSize(14)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(2, 1).Value = "Danh mục " + list.Count() + " kiến nghị cử tri gửi tới " + Server.HtmlDecode(_kn.Get_TenKyHop(iKyHop)) + ", " + Server.HtmlDecode(_kn.Get_TenKhoaHop_By_IDKyHop(iKyHop)) + " đang trong quá trình giải quyết";
                wb.Range(2, 1, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Font.SetFontSize(14)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(3, 1).Value = "";
                int cell = 4;
                List<decimal> list_id_phanloai = new List<decimal>();
                int count_phanloai = 1;
                var list1 = list.Where(x => x.ID_GOP <= 0).ToList();
                foreach (var p in list1)
                {
                    if (!list_id_phanloai.Contains(p.ID_PHANLOAI_TRALOI))
                    {
                        list_id_phanloai.Add(p.ID_PHANLOAI_TRALOI);
                        var list2 = list1.Where(x => x.ID_PHANLOAI_TRALOI == p.ID_PHANLOAI_TRALOI).ToList();
                        wb.Cell(cell, 1).Value = "Bảng " + count_phanloai + ": " + list2.FirstOrDefault().TEN_PHANLOAI_TRALOI + " (" + list2.GroupBy(x => x.ID_KIENNGHI).Count() + ")";
                        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                            .Font.SetBold(true).Alignment.SetWrapText(true);
                        cell++;
                        wb.Cell(cell, 1).Value = "";
                        cell++;
                        wb.Cell(cell, 1).Value = "STT";
                        wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                            .Alignment.SetWrapText(true);
                        wb.Cell(cell, 2).Value = "Nội dung kiến nghị";
                        if (p.ID_PHANLOAI_TRALOI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                        {
                            wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                           .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                           .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                           .Alignment.SetWrapText(true);
                        }
                        else
                        {
                            wb.Range(cell, 2, cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                           .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                           .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                           .Alignment.SetWrapText(true);
                        }

                        if (p.ID_PHANLOAI_TRALOI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                        {
                            wb.Cell(cell, 3).Value = "Lộ trình giải quyết";
                            wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                .Alignment.SetWrapText(true);
                        }

                        cell++;
                        count_phanloai++;
                        List<decimal> list_id_donvi = new List<decimal>();
                        int count_donvi = 1;
                        foreach (var l in list2)
                        {
                            if (!list_id_donvi.Contains(l.ID_THAMQUYENCOQUAN))
                            {
                                var list_donvi = list2.Where(x => x.ID_THAMQUYENCOQUAN == l.ID_THAMQUYENCOQUAN).ToList();
                                list_id_donvi.Add(l.ID_THAMQUYENCOQUAN);
                                wb.Cell(cell, 1).Value = count_donvi + ". " + Server.HtmlDecode(list_donvi.FirstOrDefault().TEN_THAMQUYENCOQUAN) + " (" + list_donvi.GroupBy(x => x.ID_KIENNGHI).Count() + ")";
                                wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                    .Alignment.SetWrapText(true);
                                cell++;
                                List<decimal> list_id_linhvuc = new List<decimal>();
                                int count_linhvuc = 1;
                                foreach (var m in list_donvi)
                                {
                                    if (!list_id_linhvuc.Contains(m.ID_LINHVUC))
                                    {
                                        list_id_linhvuc.Add(m.ID_LINHVUC);
                                        var list_linhvuc = list_donvi.Where(x => x.ID_LINHVUC == m.ID_LINHVUC).ToList();
                                        string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                                        if (m.ID_LINHVUC != 0) { tenlinhvuc = list_linhvuc.FirstOrDefault().TEN_LINHVUC; }
                                        wb.Cell(cell, 1).Value = count_donvi + "." + count_linhvuc + " " + Server.HtmlDecode(tenlinhvuc) + " (" + list_linhvuc.GroupBy(x => x.ID_KIENNGHI).Count() + ")";
                                        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                            .Alignment.SetWrapText(true);
                                        cell++;
                                        int count2 = 1;
                                        List<decimal> list_id_kiennghi = new List<decimal>();
                                        foreach (var k in list_linhvuc)
                                        {
                                            if (!list_id_kiennghi.Contains(k.ID_KIENNGHI))
                                            {
                                                list_id_kiennghi.Add(k.ID_KIENNGHI);
                                                string donvitiepnhan = "";
                                                int count_donvi_gop = 0;
                                                var kn_gop = list.Where(x => x.ID_KIENNGHI_PARENT_GOP == l.ID_KIENNGHI).ToList();
                                                if (kn_gop.Count() > 0)
                                                {
                                                    List<string> list_donvi_gop = new List<string>();
                                                    foreach (var g in kn_gop)
                                                    {
                                                        if (!list_donvi_gop.Contains(g.TENDONVITIEPNHAN_GOP))
                                                        {
                                                            if (count_donvi_gop > 0) { donvitiepnhan += ", "; }
                                                            donvitiepnhan += g.TENDONVITIEPNHAN_GOP;
                                                            list_donvi_gop.Add(g.TENDONVITIEPNHAN_GOP);
                                                            count_donvi_gop++;
                                                        }
                                                    }
                                                }
                                                if (donvitiepnhan == "")
                                                {
                                                    donvitiepnhan = l.TEN_DONVITIEPNHAN;
                                                }
                                                donvitiepnhan = " (Cử tri " + donvitiepnhan + ")";

                                                string ngaygiaiquyet = "";
                                                if (k.NGAY_DUKIENGIAIQUYET != Convert.ToDateTime("0001-01-01"))
                                                {
                                                    ngaygiaiquyet = func.ConvertDateVN(k.NGAY_DUKIENGIAIQUYET.ToString());
                                                }
                                                wb.Cell(cell, 1).Value = count2;
                                                wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                                wb.Cell(cell, 2).Value = k.NOIDUNG_KIENNGHI + donvitiepnhan;
                                                wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                                if (p.ID_PHANLOAI_TRALOI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                                                {
                                                    wb.Cell(cell, 3).Value = Server.HtmlDecode(ngaygiaiquyet);
                                                    wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                                }
                                                else
                                                {
                                                    wb.Range(cell, 2, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                                }
                                                cell++;
                                                count2++;
                                            }
                                        }
                                        count_linhvuc++;
                                    }
                                }
                                count_donvi++;
                            }
                        }
                    }
                }
                /*
                var list_colotrinh = list.Where(x => x.TINHTRANG_TRALOI == 4).ToList();
                var list_kolotrinh = list.Where(x => x.TINHTRANG_TRALOI == 5).ToList();
                List<int> list_id_donvi = new List<int>();
                foreach (var l in list)
                {
                    if (!list_id_donvi.Contains((int)l.ID_DONVI)) { list_id_donvi.Add((int)l.ID_DONVI); }
                }
                int count = 1;
                wb.Cell(cell, 1).Value = "Bảng 1: Các văn bản có lộ trình giải quyết (" + list_colotrinh.Count() + ")";
                wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true);
                cell++;
                wb.Cell(cell, 1).Value = "";
                cell++;
                wb.Cell(cell, 1).Value = "STT";
                wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(cell, 2).Value = "Nội dung kiến nghị";
                wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(cell, 3).Value = "Lộ trình giải quyết";
                wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                cell++;
                foreach (var l in list_id_donvi)//id_donvi
                {
                    var list_kn_donvi = list_colotrinh.Where(x => x.ID_DONVI == l).ToList();
                    if (list_kn_donvi.Count() > 0)
                    {
                        wb.Cell(cell, 1).Value = count + ". " + Server.HtmlDecode(list_kn_donvi.Where(x => x.ID_DONVI == l).FirstOrDefault().TEN_DONVI) + " (" + list_kn_donvi.Count() + ")";
                        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                            .Alignment.SetWrapText(true);
                        cell++;
                        foreach (var l1 in list_kn_donvi)//list_kiến nghị đơn vị
                        {
                            List<int> list_id_linhvuc = new List<int>();
                            if (!list_id_linhvuc.Contains((int)l1.ID_LINHVUC)) { list_id_linhvuc.Add((int)l1.ID_LINHVUC); }
                            int count1 = 1;
                            foreach (var lv in list_id_linhvuc)
                            {
                                var list_kn_linhvuc = list_kn_donvi.Where(x => x.ID_LINHVUC == lv).ToList();
                                if (list_kn_linhvuc.Count() > 0)
                                {
                                    wb.Cell(cell, 1).Value = count + "." + count1 + " " + Server.HtmlDecode(list_kn_linhvuc.Where(x => x.ID_LINHVUC == lv).FirstOrDefault().TENLINNHVUC) + " (" + list_kn_linhvuc.Count() + ")";
                                    wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                        .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                        .Alignment.SetWrapText(true);
                                    cell++;
                                    int count2 = 1;
                                    foreach (var k in list_kn_linhvuc)
                                    {
                                        wb.Cell(cell, 1).Value = count2;
                                        wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                        wb.Cell(cell, 2).Value = Server.HtmlDecode(k.NOIDUNG_KIENNGHI);
                                        wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                        wb.Cell(cell, 3).Value = Server.HtmlDecode(k.TRALOI_KIENNGHI);
                                        wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                        cell++;
                                        count2++;
                                    }
                                    count1++;
                                }

                            }
                        }
                        count++;
                    }
                }
                wb.Cell(cell, 1).Value = "";
                cell++;
                count = 1;
                wb.Cell(cell, 1).Value = "Bảng 2: Các văn bản chưa có lộ trình giải quyết (" + list_kolotrinh.Count() + ")";
                wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true);
                cell++;
                wb.Cell(cell, 1).Value = "";
                cell++;
                wb.Cell(cell, 1).Value = "STT";
                wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(cell, 2).Value = "Nội dung kiến nghị";
                wb.Range(cell, 2, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                   .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                   .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                   .Alignment.SetWrapText(true);
                cell++;
                foreach (var l in list_id_donvi)//id_donvi
                {
                    var list_kn_donvi = list_kolotrinh.Where(x => x.ID_DONVI == l).ToList();
                    if (list_kn_donvi.Count() > 0)
                    {
                        wb.Cell(cell, 1).Value = count + ". " + Server.HtmlDecode(list_kn_donvi.Where(x => x.ID_DONVI == l).FirstOrDefault().TEN_DONVI) + " (" + list_kn_donvi.Count() + ")";
                        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                            .Alignment.SetWrapText(true);
                        cell++;
                        foreach (var l1 in list_kn_donvi)//list_kiến nghị đơn vị
                        {
                            List<int> list_id_linhvuc = new List<int>();
                            if (!list_id_linhvuc.Contains((int)l1.ID_LINHVUC)) { list_id_linhvuc.Add((int)l1.ID_LINHVUC); }
                            int count1 = 1;
                            foreach (var lv in list_id_linhvuc)
                            {
                                var list_kn_linhvuc = list_kn_donvi.Where(x => x.ID_LINHVUC == lv).ToList();
                                if (list_kn_linhvuc.Count() > 0)
                                {
                                    wb.Cell(cell, 1).Value = count + "." + count1 + " " + Server.HtmlDecode(list_kn_linhvuc.Where(x => x.ID_LINHVUC == lv).FirstOrDefault().TENLINNHVUC) + " (" + list_kn_linhvuc.Count() + ")";
                                    wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                        .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                        .Alignment.SetWrapText(true);
                                    cell++;
                                    int count2 = 1;
                                    foreach (var k in list_kn_linhvuc)
                                    {
                                        wb.Cell(cell, 1).Value = count2;
                                        wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                        wb.Cell(cell, 2).Value = Server.HtmlDecode(k.NOIDUNG_KIENNGHI);
                                        wb.Range(cell, 2, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                                        cell++;
                                        count2++;
                                    }
                                    count1++;
                                }

                            }
                        }
                        count++;
                    }
                }
                */
                // Chút anh thêm 
                // wb.Cell(j, 1);
                // wb.Cell(j, 2);
                // wb.Cell(j, 10);
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=phuluc5b.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: phụ lục 5B");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Phuluc4()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-linhvuc"] = Get_Option_DonVi_Linhvuc();
                ViewData["opt-coquan"] = Get_Option_DonViTiepNhan();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Phụ lục 4");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Phuluc5()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-nam"] = Get_Option_Year();
                ViewData["opt-quy"] = Get_Option_Quy();
                ViewData["opt-thang"] = Get_Option_Thang();
                ViewData["opt-linhvuc"] = Get_Option_DonVi_Linhvuc();
                ViewData["opt-coquan"] = Get_Option_DonViTiepNhan();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Phụ lục 5");
                return View("../Home/Error_Exception");
            }

        }
        public string Get_Option_Year()
        {
            int year_now = DateTime.Now.Year;
            string str = "";
            for (int i = year_now + 5; i > 2000; i--)
            {
                string select = ""; if (i == year_now) { select = "selected"; }
                str += "<option " + select + " value='" + i + "'>" + i + "</option>";
            }
            return str;
        }
        public string Get_Option_Thang()
        {
            string str = "";
            for (int i = 1; i <= 12; i++)
            {
                str += "<option  value='" + i + "'>Tháng " + i + "</option>";
            }
            return str;
        }
        public string Get_Option_Quy()
        {
            string str = "";
            for (int i = 1; i <= 4; i++)
            {
                str += "<option  value='" + i + "'>Quý " + _kn.Convert_to_RomanNumber(i) + "</option>";
            }
            return str;
        }
        public ActionResult Phuluc6()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-nam"] = Get_Option_Year();
                ViewData["opt-quy"] = Get_Option_Quy();
                ViewData["opt-thang"] = Get_Option_Thang();
                ViewData["opt-coquan"] = Get_Option_Coquanxuly();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Phụ lục 6");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Kiennghi_phuluc6_excel()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int quy_giaiquyet = Convert.ToInt32(Request["quy_giaiquyet"]);
                int nam_giaiquyet = Convert.ToInt32(Request["nam_giaiquyet"]);
                int thang_giaiquyet = Convert.ToInt32(Request["thang_giaiquyet"]);
                string type = Request["type"];
                List<string> tungay_denngay = Get_TuNgay_DenNgay(type, nam_giaiquyet, quy_giaiquyet, thang_giaiquyet);
                string thoigian_giaiquyet_dutdiem = "";
                if (type == "nam")
                {
                    thoigian_giaiquyet_dutdiem = " năm " + nam_giaiquyet;
                }
                if (type == "quy")
                {
                    thoigian_giaiquyet_dutdiem = " quý " + _kn.Convert_to_RomanNumber(quy_giaiquyet) + " năm " + nam_giaiquyet;
                }
                if (type == "thang")
                {
                    thoigian_giaiquyet_dutdiem = " tháng " + thang_giaiquyet + " năm " + nam_giaiquyet;
                }
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);

                var list = kn_report.getReportBaoCaoThongKePhuLuc6("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC6",
                    func.ConvertDateToSql(tungay_denngay[0]), func.ConvertDateToSql(tungay_denngay[1]), iDonVi);
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Phụ lục 6");
                // Merge a row
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Cell(1, 1).Value = "PHỤ LỤC 6";
                wb.Column(2).Width = 30;
                wb.Column(3).Width = 70;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Range(1, 1, 1, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(2, 1).Value = list.Count() + " chuyên đề giám sát " + thoigian_giaiquyet_dutdiem;
                wb.Range(2, 1, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(3, 1).Value = "";
                wb.Range(3, 1, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;
                wb.Cell(4, 1).Value = "";
                wb.Cell(5, 1).Value = "";
                wb.Cell(6, 1).Value = "STT";
                wb.Cell(6, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Cơ quan giám sát";
                wb.Cell(6, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(6, 3).Value = "Nội dung giám sát";
                wb.Cell(6, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true);
                int cell = 7;
                list = list.OrderBy(x => x.ID_DONVI).ToList();
                List<int> list_id_donvi = new List<int>();
                foreach (var l in list)
                {
                    if (!list_id_donvi.Contains((int)l.ID_DONVI)) { list_id_donvi.Add((int)l.ID_DONVI); }
                }
                int count = 1;
                foreach (var d in list_id_donvi)
                {
                    var list_donvi = list.Where(x => x.ID_DONVI == d).ToList();
                    if (list_donvi.Count() == 1)
                    {
                        wb.Cell(cell, 1).Value = count;
                        wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(cell, 2).Value = Server.HtmlDecode(list_donvi[0].TEN_DONVI);
                        wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(cell, 3).Value = Server.HtmlDecode(list_donvi[0].KEHOACH + ". " + list_donvi[0].NOIDUNG);
                        wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        cell++;
                        count++;
                    }
                    else
                    {
                        int count1 = 1;
                        foreach (var l in list_donvi)
                        {
                            if (count1 == 1)
                            {
                                int merge_x = cell + list_donvi.Count() - 1;
                                wb.Cell(cell, 1).Value = count;
                                wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                 .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                wb.Cell(cell, 2).Value = Server.HtmlDecode(l.TEN_DONVI);
                                wb.Range(cell, 2, merge_x, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                wb.Cell(cell, 3).Value = Server.HtmlDecode(l.KEHOACH + ". " + l.NOIDUNG);
                                wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                cell++;
                                count++;
                            }
                            else
                            {
                                wb.Cell(cell, 1).Value = count;
                                wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                wb.Cell(cell, 3).Value = Server.HtmlDecode(l.KEHOACH + ". " + l.NOIDUNG);
                                wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                cell++;
                                count++;
                            }
                            count1++;
                        }
                    }
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=phuluc6.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: phụ lục 6");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_phuluc6(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int quy_giaiquyet = Convert.ToInt32(fc["quy_giaiquyet"]);
                int nam_giaiquyet = Convert.ToInt32(fc["nam_giaiquyet"]);
                int thang_giaiquyet = Convert.ToInt32(fc["thang_giaiquyet"]);
                string type = fc["type"];
                List<string> tungay_denngay = Get_TuNgay_DenNgay(type, nam_giaiquyet, quy_giaiquyet, thang_giaiquyet);
                string thoigian_giamsat = "";
                if (type == "nam")
                {
                    thoigian_giamsat = " năm " + nam_giaiquyet;
                }
                if (type == "quy")
                {
                    thoigian_giamsat = " quý " + _kn.Convert_to_RomanNumber(quy_giaiquyet) + " năm " + nam_giaiquyet;
                }
                if (type == "thang")
                {
                    thoigian_giamsat = " tháng " + thang_giaiquyet + " năm " + nam_giaiquyet;
                }
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                //List<KIENNGHIPHULUC1> dt = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, 0);
                //ViewData["thoigian_giamsat"] = thoigian_giamsat;
                ViewData["list"] = _kn.BAOCAO_TK_PHULUC6(tungay_denngay[0], tungay_denngay[1], iDonVi, thoigian_giamsat);
                return PartialView("../Ajax/Baocaokiennghi/Kiennghi_6");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax: Phụ lục 6"); throw;
            }

        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_phuluc5(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //int iKyHop = Convert.ToInt32(fc["iKyHop"]);


                int quy_giaiquyet = Convert.ToInt32(fc["quy_giaiquyet"]);
                int nam_giaiquyet = Convert.ToInt32(fc["nam_giaiquyet"]);
                int thang_giaiquyet = Convert.ToInt32(fc["thang_giaiquyet"]);
                string type = fc["type"];
                List<string> tungay_denngay = Get_TuNgay_DenNgay(type, nam_giaiquyet, quy_giaiquyet, thang_giaiquyet);
                string thoigian_giaiquyet_dutdiem = "";
                if (type == "nam")
                {
                    thoigian_giaiquyet_dutdiem = " năm " + nam_giaiquyet;
                }
                if (type == "quy")
                {
                    thoigian_giaiquyet_dutdiem = " quý " + _kn.Convert_to_RomanNumber(quy_giaiquyet) + " năm " + nam_giaiquyet;
                }
                if (type == "thang")
                {
                    thoigian_giaiquyet_dutdiem = " tháng " + thang_giaiquyet + " năm " + nam_giaiquyet;
                }
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(fc["iLinhVuc"]);
                //List<KIENNGHIPHULUC1> dt = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, 0);

                ViewData["list"] = _kn.BAOCAO_TK_PHULUC5(tungay_denngay[0], tungay_denngay[1], iDonVi, iLinhVuc, thoigian_giaiquyet_dutdiem);
                return PartialView("../Ajax/Baocaokiennghi/Kiennghi_5");

            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax: Phụ lục 5"); throw;
            }

        }
        public List<string> Get_TuNgay_DenNgay(string type, int nam, int quy, int thang)
        {
            List<string> str = new List<string>();
            if (type == "nam")
            {
                str.Add("01/01/" + nam);
                str.Add("31/12/" + nam);
                return str;
            }
            if (type == "quy")
            {
                if (quy == 1)
                {
                    str.Add("01/01/" + nam);
                    str.Add("31/03/" + nam);
                }
                if (quy == 2)
                {
                    str.Add("01/04/" + nam);
                    str.Add("30/06/" + nam);
                }
                if (quy == 3)
                {
                    str.Add("01/07/" + nam);
                    str.Add("30/09/" + nam);
                }
                if (quy == 4)
                {
                    str.Add("01/10/" + nam);
                    str.Add("31/12/" + nam);
                }
            }
            if (type == "thang")
            {
                var startDate = new DateTime(nam, thang, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                str.Add(startDate.ToString("dd/MM/yyyy"));
                str.Add(endDate.ToString("dd/MM/yyyy"));
            }
            return str;
        }
        public ActionResult Kiennghi_phuluc5_excel()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string tungay = "", denngay = "";
                int quy_giaiquyet = Convert.ToInt32(Request["quy_giaiquyet"]);
                int nam_giaiquyet = Convert.ToInt32(Request["nam_giaiquyet"]);
                int thang_giaiquyet = Convert.ToInt32(Request["thang_giaiquyet"]);
                string type = Request["type"];
                List<string> tungay_denngay = Get_TuNgay_DenNgay(type, nam_giaiquyet, quy_giaiquyet, thang_giaiquyet);
                string thoigian_giaiquyet_dutdiem = "";
                if (type == "nam")
                {
                    thoigian_giaiquyet_dutdiem = " năm " + nam_giaiquyet;
                }
                if (type == "quy")
                {
                    thoigian_giaiquyet_dutdiem = " quý " + _kn.Convert_to_RomanNumber(quy_giaiquyet) + " năm " + nam_giaiquyet;
                }
                if (type == "thang")
                {
                    thoigian_giaiquyet_dutdiem = " tháng " + thang_giaiquyet + " năm " + nam_giaiquyet;
                }
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);

                var dt = kn_report.getReportBaoCaoThongKePhuLuc5("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC5",
                    func.ConvertDateToSql(tungay_denngay[0]), func.ConvertDateToSql(tungay_denngay[1]), iDonVi, iLinhVuc);
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Phụ lục 5");
                // Merge a row
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Cell(1, 1).Value = "PHỤ LỤC 5";
                wb.Column(2).Width = 80;
                wb.Column(3).Width = 20;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Range(1, 1, 1, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(2, 1).Value = "Danh mục " + dt.Count() + " kiến nghị tồn đọng qua nhiều kỳ họp đang trong quá trình giải quyết ";
                wb.Range(2, 1, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(14)
                                                    .Alignment.SetWrapText(true)
                                                    .Font.Bold = true;
                wb.Cell(3, 1).Value = "";
                wb.Range(3, 1, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;
                wb.Cell(4, 1).Value = "";
                wb.Range(4, 1, 4, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                int cell = 5;
                var list1 = dt.Where(x => x.ID_GOP <= 0 && x.ID_PHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH).ToList();
                wb.Cell(cell, 1).Value = "Bảng 1: " + list1.Count() + " kiến nghị có lộ trình giải quyết đến " + thoigian_giaiquyet_dutdiem;
                wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Alignment.SetWrapText(true).Font.SetBold(true);
                cell++;
                wb.Cell(cell, 1).Value = "";
                cell++;
                wb.Cell(cell, 1).Value = "STT";
                wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(cell, 2).Value = "Nội dung kiến nghị/địa phương kiến nghị";
                wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(cell, 3).Value = "Thời điểm kiến nghị";
                wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                cell++;

                List<decimal> list_id_coquan = new List<decimal>();
                int count_donvi = 0;
                foreach (var d in list1)
                {
                    if (!list_id_coquan.Contains(d.ID_THAMQUYENCOQUAN))
                    {
                        list_id_coquan.Add(d.ID_THAMQUYENCOQUAN); count_donvi++;
                        var list2 = list1.Where(x => x.ID_THAMQUYENCOQUAN == d.ID_THAMQUYENCOQUAN).ToList();
                        wb.Cell(cell, 1).Value = "" + count_donvi + ". " + list2.Where(x => x.ID_THAMQUYENCOQUAN == d.ID_THAMQUYENCOQUAN).FirstOrDefault().TEN_THAMQUYENCOQUAN + " (" + list2.Count() + ")";
                        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                            .Alignment.SetWrapText(true);
                        cell++;
                        int count1 = 1;
                        List<decimal> list_id_kiennghi = new List<decimal>();
                        foreach (var l in list2)
                        {
                            if (!list_id_kiennghi.Contains(l.ID_KIENNGHI))
                            {
                                list_id_kiennghi.Add(l.ID_KIENNGHI);
                                string donvitiepnhan = "";
                                int count_donvi_gop = 0;
                                var kn_gop = dt.Where(x => x.ID_KIENNGHI_PARENT_GOP == l.ID_KIENNGHI).ToList();
                                if (kn_gop.Count() > 0)
                                {
                                    List<string> list_donvi_gop = new List<string>();
                                    foreach (var g in kn_gop)
                                    {
                                        if (!list_donvi_gop.Contains(g.TENDONVITIEPNHAN_GOP))
                                        {
                                            if (count_donvi_gop > 0) { donvitiepnhan += ", "; }
                                            donvitiepnhan += g.TENDONVITIEPNHAN_GOP;
                                            list_donvi_gop.Add(g.TENDONVITIEPNHAN_GOP);
                                            count_donvi_gop++;
                                        }
                                    }
                                }
                                if (donvitiepnhan == "")
                                {
                                    donvitiepnhan = l.TEN_DONVITIEPNHAN;
                                }
                                donvitiepnhan = " (Cử tri " + donvitiepnhan + ")";
                                wb.Cell(cell, 1).Value = count1;
                                wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                    .Alignment.SetWrapText(true);
                                wb.Cell(cell, 2).Value = l.NOIDUNG_KIENNGHI + donvitiepnhan;
                                wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                    .Alignment.SetWrapText(true);
                                wb.Cell(cell, 3).Value = l.TEN_KYHOP + ", " + l.TEN_KHOAHOP;
                                wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                    .Alignment.SetWrapText(true);
                                count1++;
                                cell++;
                            }
                        }
                    }
                }
                //wb.Cell(cell, 1).Value = "";
                //cell++;
                //var list_colotrinh = dt.Where(x => x.TINHTRANG_TRALOI == 4).ToList();
                //var list_chuagiaiquyet = dt.Where(x => x.TINHTRANG_TRALOI == 6).ToList();
                //var list_nguonluc = dt.Where(x => x.TINHTRANG_TRALOI == 7).ToList();
                //List<int> id_donvi_colotrinh = new List<int>();
                //foreach (var l in list_colotrinh)
                //{
                //    if (!id_donvi_colotrinh.Contains((int)l.ID_DONVI)) { id_donvi_colotrinh.Add((int)l.ID_DONVI); }
                //}
                //List<int> id_donvi_chuagiaiquyet = new List<int>();
                //foreach (var l in list_chuagiaiquyet)
                //{
                //    if (!id_donvi_chuagiaiquyet.Contains((int)l.ID_DONVI)) { id_donvi_chuagiaiquyet.Add((int)l.ID_DONVI); }
                //}
                //List<int> id_nguonluc = new List<int>();
                //foreach (var l in list_nguonluc)
                //{
                //    if (!id_nguonluc.Contains((int)l.ID_DONVI)) { id_nguonluc.Add((int)l.ID_DONVI); }
                //}

                //wb.Cell(cell, 1).Value = "Bảng 1: " + list_colotrinh.Count() + " kiến nghị có lộ trình giải quyết đến " + thoigian_giaiquyet_dutdiem;
                //wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Alignment.SetWrapText(true).Font.SetBold(true);
                //cell++;
                //wb.Cell(cell, 1).Value = "";
                //cell++;
                //wb.Cell(cell, 1).Value = "STT";
                //wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(cell, 2).Value = "Nội dung kiến nghị/địa phương kiến nghị";
                //wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(cell, 3).Value = "Thời điểm kiến nghị";
                //wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);

                //cell++;
                //if (id_donvi_colotrinh != null)
                //{
                //    int count = 1;
                //    foreach (var d in id_donvi_colotrinh)
                //    {
                //        var list_colotrinh_donvi = list_colotrinh.Where(x => x.ID_DONVI == d).ToList();
                //        wb.Cell(cell, 1).Value = "" + count + ". " + list_colotrinh.Where(x => x.ID_DONVI == d).FirstOrDefault().TEN_DONVI + " (" + list_colotrinh_donvi.Count() + ")";
                //        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                            .Alignment.SetWrapText(true);
                //        cell++;
                //        int count1 = 1;
                //        foreach (var l in list_colotrinh_donvi)
                //        {
                //            wb.Cell(cell, 1).Value = count1;
                //            wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            wb.Cell(cell, 2).Value = Server.HtmlDecode(l.NOIDUNG_KIENNGHI);
                //            wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            wb.Cell(cell, 3).Value = Server.HtmlDecode(l.TEN_KYHOP) + ", " + Server.HtmlDecode(l.TEN_KHOAHOP);
                //            wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            count1++;
                //            cell++;
                //        }
                //        cell++;
                //    }

                //}
                //wb.Cell(cell, 1).Value = "Bảng 2: " + list_chuagiaiquyet.Count() + " kiến nghị chưa thể giải quyết ngay";
                //wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Alignment.SetWrapText(true).Font.SetBold(true);
                //cell++;
                //wb.Cell(cell, 1).Value = "";
                //cell++;
                //wb.Cell(cell, 1).Value = "STT";
                //wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(cell, 2).Value = "Nội dung kiến nghị/địa phương kiến nghị";
                //wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(cell, 3).Value = "Thời điểm kiến nghị";
                //wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);

                //cell++;
                //if (id_donvi_chuagiaiquyet != null)
                //{
                //    int count = 1;
                //    foreach (var d in id_donvi_chuagiaiquyet)
                //    {
                //        var list_colotrinh_donvi = list_chuagiaiquyet.Where(x => x.ID_DONVI == d).ToList();
                //        wb.Cell(cell, 1).Value = "" + count + ". " + list_colotrinh.Where(x => x.ID_DONVI == d).FirstOrDefault().TEN_DONVI + " (" + list_colotrinh_donvi.Count() + ")";
                //        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                            .Alignment.SetWrapText(true);
                //        cell++;
                //        int count1 = 1;
                //        foreach (var l in list_colotrinh_donvi)
                //        {
                //            wb.Cell(cell, 1).Value = count1;
                //            wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            wb.Cell(cell, 2).Value = Server.HtmlDecode(l.NOIDUNG_KIENNGHI);
                //            wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            wb.Cell(cell, 3).Value = Server.HtmlDecode(l.TEN_KYHOP) + ", " + Server.HtmlDecode(l.TEN_KHOAHOP);
                //            wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            count1++;
                //            cell++;
                //        }
                //        cell++;
                //    }

                //}
                //cell++;
                ////bảng 3: ko có nguồn lực
                //wb.Cell(cell, 1).Value = "Bảng 3: " + list_nguonluc.Count() + " kiến nghị chưa có nguồn lực để giải quyết";
                //wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true)
                //                                    .Alignment.SetWrapText(true);
                //cell++;
                //wb.Cell(cell, 1).Value = "";
                //cell++;
                //wb.Cell(cell, 1).Value = "STT";
                //wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(cell, 2).Value = "Nội dung kiến nghị/địa phương kiến nghị";
                //wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(cell, 3).Value = "Thời điểm kiến nghị";
                //wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);

                //cell++;
                //if (id_nguonluc != null)
                //{
                //    int count = 1;
                //    foreach (var d in id_nguonluc)
                //    {
                //        var list_colotrinh_donvi = list_nguonluc.Where(x => x.ID_DONVI == d).ToList();
                //        wb.Cell(cell, 1).Value = "" + count + ". " + list_colotrinh.Where(x => x.ID_DONVI == d).FirstOrDefault().TEN_DONVI + " (" + list_colotrinh_donvi.Count() + ")";
                //        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                            .Alignment.SetWrapText(true);
                //        cell++;
                //        int count1 = 1;
                //        foreach (var l in list_colotrinh_donvi)
                //        {
                //            wb.Cell(cell, 1).Value = count1;
                //            wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            wb.Cell(cell, 2).Value = Server.HtmlDecode(l.NOIDUNG_KIENNGHI);
                //            wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            wb.Cell(cell, 3).Value = Server.HtmlDecode(l.TEN_KYHOP) + ", " + Server.HtmlDecode(l.TEN_KHOAHOP);
                //            wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            count1++;
                //            cell++;
                //        }
                //        cell++;
                //    }

                //}
                //cell++;


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=phuluc5.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: phụ lục 5");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Kiennghi_phuluc4_excel()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                var dt = kn_report.getReportBaoCaoThongKePhuLuc4("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC4", iDonVi, iLinhVuc);
                dt = dt.Where(x => x.TONGKIENNGHI_DAGIAIQUYET > 0 || x.TONGKIENNGHI_DANGGIAIQUYET > 0 && x.ID_CAPCOQUAN == (decimal)ID_Capcoquan.Bobannganh).ToList();
                int tong_dagiaiquyet = (int)dt.Sum(x => x.TONGKIENNGHI_DAGIAIQUYET);
                int tong_danggiaiquyet = (int)dt.Sum(x => x.TONGKIENNGHI_DANGGIAIQUYET);
                int tong = tong_dagiaiquyet + tong_danggiaiquyet;
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Phụ lục 4");
                // Merge a row
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Cell(1, 1).Value = "PHỤ LỤC 4";
                wb.Column(2).Width = 40;
                wb.Column(3).Width = 20;
                wb.Column(4).Width = 20;
                wb.Column(5).Width = 20;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Range(1, 1, 1, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(2, 1).Value = "Kết quả giải quyết đối với " + tong + " kiến nghị tồn đọng qua nhiều kỳ họp ";
                wb.Range(2, 1, 2, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(3, 1).Value = "(" + tong_dagiaiquyet + " đã giải quyết xong; " +
                                            tong_danggiaiquyet + " kiến nghị chưa giải quyết)";
                wb.Range(3, 1, 3, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(4, 1).Value = "";
                wb.Range(4, 1, 4, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(5, 1).Value = "";
                wb.Range(5, 1, 5, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(6, 1).Value = "STT";
                wb.Range(6, 1, 6, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Tên bộ, ngành";
                wb.Cell(6, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(6, 3).Value = "Tổng số kiến nghị";
                wb.Cell(6, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true);
                wb.Cell(6, 4).Value = "Kiến nghị đã giải quyết xong";
                wb.Cell(6, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                     .Font.SetBold(true).Alignment.SetWrapText(true);
                wb.Cell(6, 5).Value = "Kiến nghị đang giải quyết";
                wb.Cell(6, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true);

                int cell = 7;
                int count = 1;

                foreach (var t in dt)
                {
                    int tongkiennghi = (int)t.TONGKIENNGHI_DAGIAIQUYET + (int)t.TONGKIENNGHI_DANGGIAIQUYET;
                    if (tongkiennghi > 0)
                    {
                        wb.Cell(cell, 1).Value = count;
                        wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                        wb.Cell(cell, 2).Value = Server.HtmlDecode(t.TEN_DONVI);
                        wb.Cell(cell, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                        wb.Cell(cell, 3).Value = tongkiennghi;
                        wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                        wb.Cell(cell, 4).Value = t.TONGKIENNGHI_DAGIAIQUYET;
                        wb.Cell(cell, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                        wb.Cell(cell, 5).Value = t.TONGKIENNGHI_DANGGIAIQUYET;
                        wb.Cell(cell, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                        count++; cell++;
                        tong += tongkiennghi;
                        tong_dagiaiquyet += (int)t.TONGKIENNGHI_DAGIAIQUYET;
                        tong_danggiaiquyet += (int)t.TONGKIENNGHI_DANGGIAIQUYET;
                    }
                }
                wb.Cell(cell, 1).Value = "";
                wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                            .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                            .Alignment.SetWrapText(true);
                wb.Cell(cell, 2).Value = "Tổng";
                wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                            .Alignment.SetWrapText(true).Font.SetBold(true);
                wb.Cell(cell, 3).Value = tong;
                wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                            .Alignment.SetWrapText(true).Font.SetBold(true);
                wb.Cell(cell, 4).Value = tong_dagiaiquyet;
                wb.Cell(cell, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                            .Alignment.SetWrapText(true).Font.SetBold(true);
                wb.Cell(cell, 5).Value = tong_danggiaiquyet;
                wb.Cell(cell, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                            .Alignment.SetWrapText(true).Font.SetBold(true);
                cell++;
                int tyle = 100;
                double tyle_dagiaiquyet = 0;
                double tyle_danggiaiquyet = 0;
                if (tong > 0)
                {
                    tyle_dagiaiquyet = Math.Round(Convert.ToDouble(tong_dagiaiquyet) * 100 / Convert.ToDouble(tong), 2);
                    tyle_danggiaiquyet = 100 - tyle_dagiaiquyet;
                }



                wb.Cell(cell, 1).Value = "";
                wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                            .Alignment.SetWrapText(true);
                wb.Cell(cell, 2).Value = "Tỷ lệ ";
                wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                            .Alignment.SetWrapText(true).Font.SetBold(true);
                wb.Cell(cell, 3).Value = tyle + " %";
                wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                            .Alignment.SetWrapText(true).Font.SetBold(true);
                wb.Cell(cell, 4).Value = tyle_dagiaiquyet + " %";
                wb.Cell(cell, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                            .Alignment.SetWrapText(true).Font.SetBold(true);
                wb.Cell(cell, 5).Value = tyle_danggiaiquyet + " %";
                wb.Cell(cell, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                            .Alignment.SetWrapText(true).Font.SetBold(true);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=phuluc4.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: phụ lục 4");
                return View("../Home/Error_Exception");
            }
        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_phuluc4(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //int iKyHop = Convert.ToInt32(fc["iKyHop"]);
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(fc["iLinhVuc"]);
                ViewData["list"] = _kn.BAOCAO_TK_PHULUC4(iDonVi, iLinhVuc);
                return PartialView("../Ajax/Baocaokiennghi/Kiennghi_4");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax: phụ lục 4");
                throw;
            }

        }
        //public ActionResult Phuluc2()
        //{
        //    if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
        //    try
        //    {
        //        int iKyHop = ID_KyHop_HienTai();
        //        ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    return View();
        //}
        public ActionResult Phuluc3()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-linhvuc"] = Get_Option_DonVi_Linhvuc();
                ViewData["opt-coquan"] = Get_Option_DonViTiepNhan();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Phụ lục 3");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_phuluc3(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(fc["iKyHop"]);
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(fc["iLinhVuc"]);
                ViewData["list"] = _kn.BAOCAO_TK_PHULUC3(iKyHop, iDonVi, iLinhVuc);
                ViewData["kyhop"] = _kn.Get_TenKyHop(iKyHop) + ", " + _kn.Get_TenKhoaHop_By_IDKyHop(iKyHop);
                return PartialView("../Ajax/Baocaokiennghi/Kiennghi_3");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax: phụ lục 2");
                throw;
            }

        }
        public ActionResult Kiennghi_phuluc3_excel()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                var dt = kn_report.getReportBaoCaoThongKePhuLuc3("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC3", iKyHop, iDonVi, iLinhVuc);
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Phụ lục 3");
                string kyhop_khoahop = _kn.Get_TenKyHop(iKyHop) + ", " + _kn.Get_TenKhoaHop_By_IDKyHop(iKyHop);
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Cell(1, 1).Value = "Phụ lục 3";
                wb.Column(2).Width = 80;
                wb.Column(3).Width = 20;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Range(1, 1, 1, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Alignment.SetWrapText(true).Font.SetBold(true).Font.SetFontSize(14);
                wb.Cell(2, 1).Value = dt.Count() + " văn bản cử tri kiến nghị sửa đổi (đang được xem xét để sửa đổi, bổ sung)";
                wb.Range(2, 1, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Alignment.SetWrapText(true).Font.SetBold(true).Font.SetFontSize(14);
                wb.Cell(3, 1).Value = "(Các kiến nghị gửi tới " + Server.HtmlDecode(kyhop_khoahop) + ")";
                wb.Range(3, 1, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Alignment.SetWrapText(true).Font.SetBold(true).Font.SetFontSize(14);
                wb.Cell(4, 1).Value = "";

                int cell = 5;
                List<decimal> list_id_phanloai = new List<decimal>();
                int count = 0;
                foreach (var l in dt)
                {
                    if (!list_id_phanloai.Contains(l.ID_PHANLOAI))
                    {
                        list_id_phanloai.Add(l.ID_PHANLOAI); count++;
                        var list1 = dt.Where(x => x.ID_PHANLOAI == l.ID_PHANLOAI).ToList();
                        string col_lotrinh = "";
                        if (l.ID_PHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                        {
                            col_lotrinh = "Lộ trình giải quyết";
                        }
                        wb.Cell(cell, 1).Value = "Bảng " + count + ": " + l.TENPHANLOAI + "(" + list1.Count() + ")";
                        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                            .Font.SetBold(true).Font.SetFontSize(14)
                                                            .Alignment.SetWrapText(true);
                        cell++;
                        wb.Cell(cell, 1).Value = "STT";
                        wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                            .Alignment.SetWrapText(true);
                        wb.Cell(cell, 2).Value = "Nội dung";
                        wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                            .Alignment.SetWrapText(true);
                        wb.Cell(cell, 3).Value = col_lotrinh;
                        wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                            .Alignment.SetWrapText(true);
                        cell++;
                        int count1 = 1;
                        foreach (var k in list1)
                        {
                            string col_lotrinh1 = "";
                            if (l.ID_PHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                            {
                                DateTime firstdate = Convert.ToDateTime("0001-01-01");
                                if (k.NGAYDUKIEN != firstdate)
                                {
                                    col_lotrinh1 = func.ConvertDateVN(k.NGAYDUKIEN.ToString());
                                }
                            }
                            wb.Cell(cell, 1).Value = count1;
                            wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                .Alignment.SetWrapText(true);
                            wb.Cell(cell, 2).Value = k.NOIDUNG_KIENNGHI;
                            wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)

                                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                .Alignment.SetWrapText(true);
                            wb.Cell(cell, 3).Value = col_lotrinh1;
                            wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                                .Alignment.SetWrapText(true);
                            count1++;
                            cell++;
                        }
                        cell++;
                    }
                }
                /////////////
                //var list_colotrinh = dt.Where(x => x.TINHTRANG_TRALOI == 4).ToList();
                //var list_kolotrinh = dt.Where(x => x.TINHTRANG_TRALOI == 5).ToList();
                //List<int> id_donvi_colotrinh = new List<int>();
                //foreach (var l in list_colotrinh)
                //{
                //    if (!id_donvi_colotrinh.Contains((int)l.ID_DONVI)) { id_donvi_colotrinh.Add((int)l.ID_DONVI); }
                //}
                //wb.Cell(6, 1).Value = "Bảng 1: Các văn bản có lộ trình giải quyết (" + list_colotrinh.Count() + ")";
                //wb.Range(6, 1, 6, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Font.SetFontSize(14)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(8, 1).Value = "STT";
                //wb.Cell(7, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(8, 2).Value = "Nội dung";
                //wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(8, 3).Value = "Lộ trình giải quyết";
                //wb.Cell(8, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //int cell = 9;
                //if (id_donvi_colotrinh != null)
                //{
                //    int count = 1;
                //    foreach (var d in id_donvi_colotrinh)
                //    {
                //        var list_colotrinh_donvi = list_colotrinh.Where(x => x.ID_DONVI == d).ToList();
                //        wb.Cell(cell, 1).Value = "" + count + ". " + Server.HtmlDecode(list_colotrinh.Where(x => x.ID_DONVI == d).FirstOrDefault().TEN_DONVI) + " (" + list_colotrinh_donvi.Count() + ")";
                //        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                            .Alignment.SetWrapText(true);
                //        cell++;
                //        int count1 = 1;
                //        foreach (var l in list_colotrinh_donvi)
                //        {
                //            wb.Cell(cell, 1).Value = count1;
                //            wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            wb.Cell(cell, 2).Value = Server.HtmlDecode(l.NOIDUNG_KIENNGHI);
                //            wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)

                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            wb.Cell(cell, 3).Value = Server.HtmlDecode(l.LOTRINH);
                //            wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            count1++;
                //            cell++;
                //        }

                //    }

                //}
                //cell++;
                //wb.Cell(cell, 1).Value = "Bảng 2: Các văn bản có chưa trình giải quyết (" + list_kolotrinh.Count() + ")";
                //wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Font.SetFontSize(14)
                //                                    .Alignment.SetWrapText(true);
                //cell++;
                //wb.Cell(cell, 1).Value = "STT";
                //wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(cell, 2).Value = "Nội dung";
                //wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //wb.Cell(cell, 3).Value = "";
                //wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                    .Alignment.SetWrapText(true);
                //cell++;
                //List<int> id_donvi_kolotrinh = new List<int>();
                //foreach (var l in list_kolotrinh)
                //{
                //    if (!id_donvi_kolotrinh.Contains((int)l.ID_DONVI)) { id_donvi_kolotrinh.Add((int)l.ID_DONVI); }
                //}
                //if (id_donvi_kolotrinh != null)
                //{
                //    int count = 1;
                //    foreach (var d in id_donvi_kolotrinh)
                //    {
                //        var list_kolotrinh_donvi = list_kolotrinh.Where(x => x.ID_DONVI == d).ToList();
                //        wb.Cell(cell, 1).Value = "" + count + ". " + Server.HtmlDecode(list_kolotrinh.Where(x => x.ID_DONVI == d).FirstOrDefault().TEN_DONVI) + " (" + list_kolotrinh_donvi.Count() + ")";
                //        wb.Range(cell, 1, cell, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                            .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                            .Alignment.SetWrapText(true);
                //        cell++;
                //        int count1 = 1;
                //        foreach (var l in list_kolotrinh_donvi)
                //        {
                //            wb.Cell(cell, 1).Value = count1;
                //            wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            wb.Cell(cell, 2).Value = Server.HtmlDecode(l.NOIDUNG_KIENNGHI);
                //            wb.Cell(cell, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)

                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            wb.Cell(cell, 3).Value = "";
                //            wb.Cell(cell, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                //                                                .Alignment.SetWrapText(true);
                //            count1++;
                //            cell++;
                //        }

                //    }
                //}

                //
                wb.Rows().AdjustToContents();
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=phuluc3.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: phụ lục 3");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Phuluc1()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-linhvuc"] = Get_Option_DonVi_Linhvuc();
                ViewData["opt-coquan"] = Get_Option_DonViTiepNhan(); return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Phụ lục 1");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult TraloiKNTC()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-linhvuc"] = Get_Option_DonVi_Linhvuc();
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                ViewData["opt-coquan"] = Get_Option_DonViTiepNhan(); return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tổng hợp trả lời KNCT của Bộ Ngành");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_TraloiKNTC(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(fc["iKyHop"]);
                int iLoai = Convert.ToInt32(fc["iLoai"]);
                int iHinhThuc = Convert.ToInt32(fc["iTruocKyHop"]);
                //List<KIENNGHIPHULUC1> dt = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, 0);
                ViewData["kyhop"] = _kn.Get_TenKyHop(iKyHop).ToUpper();
                ViewData["kyhopkhoa"] = _kn.Get_TenKhoaHop_By_IDKyHop(iKyHop).ToUpper();
                string loai = "";
                if (iLoai == 0)
                {
                    loai = "QUỐC HỘI";
                }
                else
                {
                    loai = "HỘI ĐỒNG NHÂN DÂN";
                }
                ViewData["kyhop"] = _kn.Get_TenKyHop(iKyHop).ToUpper() + " " + loai + " " + _kn.Get_TenKhoaHop_By_IDKyHop(iKyHop).ToUpper();

                ViewData["list"] = _kn.BAOCAO_TK_TRALOI_KNTC(iKyHop, iLoai, iHinhThuc);
                return PartialView("../Ajax/Baocaokiennghi/TraloiKNTC");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax: Tổng hợp trả lời KNCT của Bộ Ngành"); throw;
            }

        }

        public ActionResult Kiennghi_TraloiKNTC_excel()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iLoai = Convert.ToInt32(Request["iLoai"]);
                int iHinhThuc = Convert.ToInt32(Request["iHinhThuc"]);
                string loai = "";
                if(iLoai == 0) { loai = " QUỐC HỘI " ; } else { loai = " HỘI ĐỒNG NHÂN DÂN " ; }
                string tenkyhop = "BẢNG THEO DÕI TRẢ LỜI Ý KIẾN, KIẾN NGHỊ CỬ TRI CỦA BỘ, NGÀNH ";
                if (iHinhThuc == 1) { tenkyhop += "TRƯỚC "; } else { tenkyhop += "SAU "; }
                string tenhoa = Server.HtmlDecode(_kn.Get_TenKhoaHop_By_IDKyHop(iKyHop).ToUpper()).Replace("KHóA","KHÓA");
                tenkyhop += Server.HtmlDecode(_kn.Get_TenKyHop(iKyHop).ToUpper()) + loai + tenhoa;
                var list = kn_report.getReportBaoCaoThongKeTraLoiKNTC("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_TRALOIKNTC", iLoai, iKyHop, iHinhThuc);
                list = list.ToList();
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Phụ Lục 6");
                // Merge a rowcheck
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(1).Width = 10;
                wb.Column(2).Width = 30;
                wb.Column(3).Width = 50;
                wb.Column(4).Width = 40;
                wb.Column(5).Width = 30;
                wb.Column(6).Width = 20;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Range(1, 1, 1, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(2, 1).Value = tenkyhop;
                wb.Range(2, 1, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;

                wb.Cell(5, 1).Value = "STT";
                wb.Cell(5, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 2).Value = "Cử tri";
                wb.Cell(5, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 3).Value = "Câu hỏi";
                wb.Cell(5, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true);
                wb.Cell(5, 4).Value = "Bộ/ngành trả lời";
                wb.Cell(5, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 5).Value = "Văn bản trả lời";
                wb.Cell(5, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(5, 6).Value = "Nội dung trả lời";
                wb.Cell(5, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true);
                int cell = 6;
                List<decimal> list_id_donvi = new List<decimal>();
                //foreach (var l in list)
                //{
                //    if (!list_id_donvi.Contains((int)l.ID_DONVI)) { list_id_donvi.Add((int)l.ID_DONVI); }
                //}
                int count = 1;
                foreach (var d in list)
                {
                    int x = 1;
                    if (count > 1)
                    {//chưa cuối
                        cell++;
                    }
                    wb.Cell(cell, x).Value = count;
                    wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    x++;
                    wb.Cell(cell, x).Value = Server.HtmlDecode(d.CUTRI);
                    wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);
                    x++;
                    wb.Cell(cell, x).Value = Server.HtmlDecode(d.CNOIDUNG);
                    wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);

                    x++;
                    wb.Cell(cell, x).Value = Server.HtmlDecode(d.COQUANTHAMQUYEN);
                    wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);

                    x++;
                    wb.Cell(cell, x).Value = Server.HtmlDecode("Văn bản số "+d.CSOVANBAN+" ngày "+ func.ConvertDateVN(d.DNGAYBANHANH.ToString()));
                    wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);

                    x++;
                    wb.Cell(cell, x).Value = Server.HtmlDecode(d.CTRALOI);
                    wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);
                    //x++;
                    //cell++;
                    count++;
                }


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=traloiKNTC.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: Tổng hợp trả lời KNCT của Bộ Ngành");
                return View("../Home/Error_Exception");
            }
        }

        public ActionResult TraloiKN_DenDBQH(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop_TheoKhoa = Convert.ToInt32(fc["iKhoa"]);
                int iKyHop = ID_KyHop_HienTai();
                ViewData["khoa"] = Get_Option_Khoa(iKyHop , 0);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tổng hợp kiến nghị cử tri gửi đến đoàn ĐBQH tỉnh khóa XV");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_TraloiKN_DenDBQH(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                
                string iKyHop = Request["iKyHop"];
                int iLoai = Convert.ToInt32(fc["iLoai"]);
                int iKhoa = Convert.ToInt32(fc["iKhoa"]);
                //List<KIENNGHIPHULUC1> dt = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, 0);

                ViewData["list"] = _kn.BAOCAO_TK_TraloiKN_DenDBQH(iKyHop, iLoai, iKhoa);
                return PartialView("../Ajax/Baocaokiennghi/TraloiKNTC");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax: Tổng hợp kiến nghị cử tri gửi đến đoàn ĐBQH tỉnh khóa XV"); throw;
            }

        }

        public ActionResult Kiennghi_TraloiKN_DenDBQH_excel()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKhoa = Convert.ToInt32(Request["iKhoa"]);
                int iLoai = Convert.ToInt32(Request["iLoai"]);
                string iKyHop = Request["iKyHop"];
                string str = "";
                var list = kn_report.getReportBaoCaoThongKeTraLoiKN_DENDBQH("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_TRALOIKN_DENDBQH", iLoai, iKyHop, iKhoa);
                var listIDKhoa = list.OrderBy(x => x.IKYHOP).ToList();
                List<int> list_id_donvi = new List<int>();
                string loai = "";
                string tenKhoa = "";
                QUOCHOI_KHOA khoa = kiennghi.Get_Khoa_QuocHoi((int)iKhoa);
                if (khoa != null)
                {
                    tenKhoa = khoa.CTEN;
                }
                if (iLoai == 0) { loai = " QUỐC HỘI "; } else { loai = " HỘI ĐỒNG NHÂN DÂN "; }

                string tenkyhop = "TỔNG HỢP KIẾN NGHỊ CỬ TRI GỬI ĐẾN ĐOÀN ĐẠI BIỂU" + loai + "TỈNH ";
                tenkyhop += tenKhoa.ToUpper();

                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Phụ Lục 6");
                // Merge a rowcheck
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                
                
                
                wb.Column(1).Width = 10;
                wb.Column(2).Width = 50;
                wb.Column(3).Width = 50;
                wb.Column(4).Width = 30;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Cell(1, 1).Value = "VĂN PHÒNG ĐOÀN ĐBQH VÀ";
                wb.Range(1, 1, 1, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(2, 1).Value = "HĐND TỈNH THANH HÓA";
                wb.Range(2, 1, 2, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(3, 1).Value = "Phòng Tổng hợp, Thông tin, Dân nguyện";
                wb.Range(3, 1, 3, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(1, 3).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(1, 3, 2, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(3, 3).Value = "Độc lập – Tự do – Hạnh phúc";
                wb.Range(3, 3, 3, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(14)
                                                    .Font.Bold = true;
                wb.Cell(6, 1).Value = tenkyhop;
                wb.Range(6, 1, 6, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;

                wb.Cell(8, 1).Value = "STT";
                wb.Cell(8, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(8, 2).Value = "NỘI DUNG KIẾN NGHỊ";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                wb.Cell(8, 3).Value = "VĂN BẢN TRẢ LỜI";
                wb.Cell(8, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true);
                wb.Cell(8, 4).Value = "GHI CHÚ";
                wb.Cell(8, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Font.Bold = true;
                //foreach (var l in list)
                //{
                //    if (!list_id_donvi.Contains((int)l.ID_DONVI)) { list_id_donvi.Add((int)l.ID_DONVI); }
                //}
                int count = 1;
                int cell = 9;
                decimal GroupIDKhoa = 0;
                foreach (var kn_IDKhoa in listIDKhoa)
                {
                    if (GroupIDKhoa != kn_IDKhoa.IKYHOP)
                    {
                        var listIDKyHop = list.Where(x => x.IKYHOP == kn_IDKhoa.IKYHOP).OrderBy(x => x.IKYHOP).ToList();
                        wb.Cell(cell, 1).Value = kn_IDKhoa.CTEN;
                        wb.Range(cell, 1, cell, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                            .Font.Bold = true;
                        cell++;
                        foreach (var d in listIDKyHop)
                        {
                            int x = 1;
                            wb.Cell(cell, x).Value = count;
                            wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                       .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                       .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            x++;
                            wb.Cell(cell, x).Value = Server.HtmlDecode(d.CNOIDUNG);
                            wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);
                            x++;
                            wb.Cell(cell, x).Value = Server.HtmlDecode("Văn bản số " + d.CSOVANBAN + " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString()) + " của " + d.CCOQUANTRALOI);
                            wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);

                            x++;
                            wb.Cell(cell, x).Value = Server.HtmlDecode(d.GHICHU_KQ);
                            wb.Cell(cell, x).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);
                            //x++;
                            //cell++;
                            count++;
                            cell++;
                        }
                    }
                    GroupIDKhoa = kn_IDKhoa.IKYHOP;
                }


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=tonghopKNCT.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: Tổng hợp kiến nghị cử tri gửi đến các đoàn đại biểu");
                return View("../Home/Error_Exception");
            }
        }
        public string Get_Option_Khoa(int iKyHop = 0, int Loai = 0)
        {
            List<QUOCHOI_KHOA> khoa = kiennghi.GetAll_KhoaHop().Where(x => x.ILOAI == Loai).OrderBy(x => x.DBATDAU).ToList();
            return "<option value='0'>Chọn khóa họp</option>" + _kn.Option_Khoa(khoa, iKyHop);
        }
        public string Get_Option_KhoaTheoLoai(int iKyHop = 0, int Loai = 0)
        {
            List<QUOCHOI_KHOA> khoa = kiennghi.GetAll_KhoaHop().Where(x => x.ILOAI == Loai).OrderBy(x => x.DBATDAU).ToList();
            return "<select name='iKhoa' id='iKhoa' class='input-medium chosen-select' onchange=\"ChangeKhoa(this.value)\" style='width:100%'>" +
                                            "<option value='0'>Chọn khóa họp</option>" +
                                            "" + _kn.Option_Khoa(khoa, iKyHop) +"" + "</select>";
        }

        public ActionResult Ajax_Change_Khoa_option(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                if (id != 0)
                {
                    List<QUOCHOI_KYHOP> kyhop = kiennghi.GetAll_KyHop().OrderBy(x => x.DBATDAU).ToList();
                    if (kyhop.Count() > 0)
                    {
                        Response.Write( _kn.Option_KyHop_TheoKhoa(kyhop, 0 , id));
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax thay đổi Phân loại kết quả trả lời.");
                return null;
            }

        }

        public ActionResult Ajax_Change_KhoaTheoLoai_option(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                if (id != 2)
                {
                    Response.Write(Get_Option_KhoaTheoLoai(iKyHop,id));
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax thay đổi Phân loại kết quả trả lời.");
                return null;
            }

        }

        public ActionResult Ajax_Change_KyHopTheoLoai_option(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                if (id != 2)
                {
                    Response.Write(Get_Option_KyHop_TheoLoai(iKyHop, id));
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax thay đổi Phân loại kết quả trả lời.");
                return null;
            }

        }

        public string Get_Option_KyHop_TheoLoai(int iKyHop = 0 ,int loai = 0)
        {
            List<QUOCHOI_KHOA> khoa = kiennghi.GetAll_KhoaHop().Where(x => x.ILOAI == loai).OrderBy(x => x.DBATDAU).ToList();
            List<QUOCHOI_KYHOP> kyhop = kiennghi.GetAll_KyHop().OrderBy(x => x.DBATDAU).ToList();
            return "<select name='iKyHop' id='iKyHop' class='input-medium chosen-select' style='width:100%'>" +
                                            "<option value='0'>Chọn kỳ họp</option>" +
                                            "" + _kn.Option_Khoa_KyHop(khoa, kyhop, iKyHop) + "" + "</select>";
        }

        public string Get_Option_KyHop(int iKyHop = 0)
        {
            List<QUOCHOI_KHOA> khoa = kiennghi.GetAll_KhoaHop().Where(x => x.ILOAI == 0).OrderBy(x => x.DBATDAU).ToList();
            List<QUOCHOI_KYHOP> kyhop = kiennghi.GetAll_KyHop().OrderBy(x => x.DBATDAU).ToList();
            return "<option value='0'>Chọn kỳ họp</option>" + _kn.Option_Khoa_KyHop(khoa, kyhop, iKyHop);
        }

        public string Get_Option_DonVi_Linhvuc()
        {
            //UserInfor u_info = GetUserInfor();
            //if (u_info.tk_action.is_chuyenvien)
            //{
            //    return Get_Option_LinhVuc_By_ID_CoQuan((int)u_info.user_login.IDONVI);
            //    //return "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
            //}
            List<QUOCHOI_COQUAN> coquan = kiennghi.GetAll_CoQuanByParam(null);
            //List<LINHVUC_COQUAN> linhvuc = kiennghi.GetAll_Coquan_linhvuc();
            List<LINHVUC_COQUAN> linhvuc = kiennghi.GetAll_Coquan_linhvuc_SortedByParent();
            return _kn.Option_Coquan_LinhVuc(coquan, linhvuc);
        }
        public string Get_Option_DonViTiepNhan(int iDonVi = 0)
        {
            UserInfor u_info = GetUserInfor();
            if (u_info.tk_action.is_dbqh)
            {
                return "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
            }
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            donvi.Add("IPARENT", ID_Coquan_doandaibieu);
            coquan = kiennghi.GetAll_CoQuanByParam(donvi);
            string opt_bandannguyen = "<option value='0'>Chọn tất cả</option><option value='" + ID_Ban_DanNguyen + "'>Ban Dân nguyện</option>";
            return opt_bandannguyen + _kn.OptionCoQuanXuLy(coquan, ID_Coquan_doandaibieu, 0, iDonVi, 0);
        }
        public string Get_Option_Coquanxuly()
        {
            UserInfor u_info = GetUserInfor();
            if (u_info.tk_action.is_chuyenvien)
            {
                return "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
            }
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            coquan = kiennghi.GetAll_CoQuanByParam(donvi).Where(x => x.IHIENTHI == 1).ToList();
            return "<option value='0'>Chọn tất cả</option>" + _kn.OptionCoQuanXuLy(coquan, 0, 0, 0, 0);
        }
        public ActionResult Kiennghi_phuluc1_excel()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                List<KIENNGHIPHULUC1> dt = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, iDonVi, iLinhVuc);
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Phụ lục 1");
                string kyhop_khoahop = _kn.Get_TenKyHop(iKyHop) + ", " + _kn.Get_TenKhoaHop_By_IDKyHop(iKyHop);

                // Merge a row
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Cell(1, 1).Value = "Phụ lục 1";
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Column(2).Width = 40;
                wb.Column(3).Width = 15;
                wb.Column(4).Width = 15;
                wb.Column(5).Width = 15;
                wb.Column(6).Width = 15;
                wb.Column(7).Width = 15;

                //wb.Cells().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Range(1, 1, 1, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Font.SetFontSize(14)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(2, 1).Value = "Bảng tổng hợp kết quả giải quyết, trả lời kiến nghị của cử tri";
                wb.Range(2, 1, 2, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Font.SetFontSize(14)
                                                    .Alignment.SetWrapText(true);

                wb.Cell(3, 1).Value = "Tại " + Server.HtmlDecode(kyhop_khoahop) + " của các bộ, ngành";
                wb.Range(3, 1, 3, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Font.SetFontSize(14)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(4, 1).Value = "";
                wb.Cell(5, 1).Value = "";
                wb.Row(6).Style.Font.SetBold(true);
                wb.Row(7).Style.Font.SetBold(true);
                wb.Cell(6, 1).Value = "STT";
                wb.Range(6, 1, 7, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);

                wb.Cell(6, 2).Value = "Tên cơ quan, đơn vị";
                wb.Range(6, 2, 7, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);

                wb.Cell(6, 3).Value = "Tổng số kiến nghị";
                wb.Range(6, 3, 7, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);

                wb.Cell(6, 4).Value = "Tổng số KN đã trả lời";
                wb.Range(6, 4, 7, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);

                wb.Cell(6, 5).Value = "Kết quả giải quyết";
                wb.Range(6, 5, 6, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);

                wb.Cell(7, 5).Value = "Đã giải quyết xong";
                wb.Cell(7, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(7, 5).Style.Alignment.WrapText = true;

                wb.Cell(7, 6).Value = "Đang giải quyết";
                wb.Cell(7, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);

                wb.Cell(7, 7).Value = "Giải trình, thông tin";
                wb.Cell(7, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);


                var donvi_level0 = dt.Where(X => X.ID_PARENT == 0).OrderBy(x => x.TEN_DONVI).ToList();
                int count0 = 1;
                string tong_baocao = "Cộng ";
                int cell = 8;
                int TONGKIENNGHI_ = 0; int TONGKIENNGHI_TRALOI_ = 0; int TONGKIENNGHI_DAGIAIQUYET_ = 0;
                int TONGKIENNGHI_DANGGIAIQUYET_ = 0; int TONGKIENNGHI_GIAITRINH_ = 0;
                foreach (var d0 in donvi_level0)
                {

                    string roman_num = _kn.Convert_to_RomanNumber(count0);
                    if (count0 > 1) { tong_baocao += " + "; }
                    tong_baocao += roman_num;
                    wb.Cell(cell, 1).Value = roman_num + ". " + Server.HtmlDecode(d0.TEN_DONVI);
                    wb.Range(cell, 1, cell, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).
                                                Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).
                                                Font.SetBold(true).Alignment.SetWrapText(true)
                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                    var donvi1 = dt.Where(X => X.ID_PARENT == d0.ID_DONVI).OrderBy(x => x.TEN_DONVI).ToList();
                    int count1 = 1;

                    cell++;
                    int TONGKIENNGHI = 0; int TONGKIENNGHI_TRALOI = 0; int TONGKIENNGHI_DAGIAIQUYET = 0;
                    int TONGKIENNGHI_DANGGIAIQUYET = 0; int TONGKIENNGHI_GIAITRINH = 0;
                    foreach (var l in donvi1)
                    {
                        wb.Cell(cell, 1).Value = count1;
                        wb.Cell(cell, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                        wb.Cell(cell, 2).Value = Server.HtmlDecode(l.TEN_DONVI);
                        wb.Cell(cell, 2).Style.Alignment.SetWrapText(false).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(cell, 3).Value = l.TONGKIENNGHI.ToString("#,##0").Replace(",", ".");
                        wb.Cell(cell, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(cell, 4).Value = l.TONGKIENNGHI_TRALOI.ToString("#,##0").Replace(",", ".");
                        wb.Cell(cell, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(cell, 5).Value = l.TONGKIENNGHI_DAGIAIQUYET.ToString("#,##0").Replace(",", ".");
                        wb.Cell(cell, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(cell, 6).Value = l.TONGKIENNGHI_DANGGIAIQUYET.ToString("#,##0").Replace(",", ".");
                        wb.Cell(cell, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(cell, 7).Value = l.TONGKIENNGHI_GIAITRINH.ToString("#,##0").Replace(",", ".");
                        wb.Cell(cell, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        TONGKIENNGHI += (int)l.TONGKIENNGHI;
                        TONGKIENNGHI_TRALOI += (int)l.TONGKIENNGHI_TRALOI;
                        TONGKIENNGHI_DAGIAIQUYET += (int)l.TONGKIENNGHI_DAGIAIQUYET;
                        TONGKIENNGHI_DANGGIAIQUYET += (int)l.TONGKIENNGHI_DANGGIAIQUYET;
                        TONGKIENNGHI_GIAITRINH += (int)l.TONGKIENNGHI_GIAITRINH;
                        count1++; cell++;
                    }
                    int TONGKIENNGHI2_ = TONGKIENNGHI;
                    int TONGKIENNGHI_TRALOI2_ = TONGKIENNGHI_TRALOI;
                    int TONGKIENNGHI_DAGIAIQUYET2_ = TONGKIENNGHI_DAGIAIQUYET;
                    int TONGKIENNGHI_DANGGIAIQUYET2_ = TONGKIENNGHI_DANGGIAIQUYET;
                    int TONGKIENNGHI_GIAITRINH2_ = TONGKIENNGHI_GIAITRINH;

                    TONGKIENNGHI_ += TONGKIENNGHI;
                    TONGKIENNGHI_TRALOI_ += TONGKIENNGHI_TRALOI;
                    TONGKIENNGHI_DAGIAIQUYET_ += TONGKIENNGHI_DAGIAIQUYET;
                    TONGKIENNGHI_DANGGIAIQUYET_ += TONGKIENNGHI_DANGGIAIQUYET;
                    TONGKIENNGHI_GIAITRINH_ += TONGKIENNGHI_GIAITRINH;
                    wb.Row(cell).Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    wb.Cell(cell, 1).Value = "Cộng " + roman_num + "";
                    wb.Range(cell, 1, cell, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.
                                        SetHorizontal(XLAlignmentHorizontalValues.Center).
                                        Border.SetOutsideBorder(XLBorderStyleValues.Thin).
                                        Font.SetBold(true).Alignment.SetWrapText(true);
                    wb.Cell(cell, 3).Value = TONGKIENNGHI2_.ToString("#,##0").Replace(",", ".");
                    wb.Cell(cell, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(cell, 4).Value = TONGKIENNGHI_TRALOI2_.ToString("#,##0").Replace(",", ".");
                    wb.Cell(cell, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(cell, 5).Value = TONGKIENNGHI_DAGIAIQUYET2_.ToString("#,##0").Replace(",", ".");
                    wb.Cell(cell, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(cell, 6).Value = TONGKIENNGHI_DANGGIAIQUYET2_.ToString("#,##0").Replace(",", ".");
                    wb.Cell(cell, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(cell, 7).Value = TONGKIENNGHI_GIAITRINH2_.ToString("#,##0").Replace(",", ".");
                    wb.Cell(cell, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    cell++;

                    int TONGKIENNGHI_tyle = 100; double TONGKIENNGHI_TRALOI_tyle = 0; double TONGKIENNGHI_DAGIAIQUYET_tyle = 0;
                    double TONGKIENNGHI_DANGGIAIQUYET_tyle = 0; double TONGKIENNGHI_GIAITRINH_tyle = 0;
                    if (TONGKIENNGHI > 0)
                    {
                        TONGKIENNGHI_TRALOI_tyle = Math.Round(Convert.ToDouble(TONGKIENNGHI_TRALOI) * 100 / Convert.ToDouble(TONGKIENNGHI), 2);
                        TONGKIENNGHI_DAGIAIQUYET_tyle = Math.Round(Convert.ToDouble(TONGKIENNGHI_DAGIAIQUYET) * 100 / Convert.ToDouble(TONGKIENNGHI), 2);
                        TONGKIENNGHI_DANGGIAIQUYET_tyle = Math.Round(Convert.ToDouble(TONGKIENNGHI_DANGGIAIQUYET) * 100 / Convert.ToDouble(TONGKIENNGHI), 2);
                        TONGKIENNGHI_GIAITRINH_tyle = Math.Round(Convert.ToDouble(TONGKIENNGHI_GIAITRINH) * 100 / Convert.ToDouble(TONGKIENNGHI), 2);
                    }
                    wb.Row(cell).Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    wb.Cell(cell, 1).Value = "Tỷ lệ";
                    wb.Range(cell, 1, cell, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).
                                Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).
                                Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(cell, 3).Value = TONGKIENNGHI_tyle + " %";
                    wb.Cell(cell, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(cell, 4).Value = TONGKIENNGHI_TRALOI_tyle + " %";
                    wb.Cell(cell, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(cell, 5).Value = TONGKIENNGHI_DAGIAIQUYET_tyle + " %";
                    wb.Cell(cell, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(cell, 6).Value = TONGKIENNGHI_DANGGIAIQUYET_tyle + " %";
                    wb.Cell(cell, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(cell, 7).Value = TONGKIENNGHI_GIAITRINH_tyle + " %";
                    wb.Cell(cell, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    cell++;
                    count0++;
                }
                wb.Row(cell).Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(cell, 1).Value = tong_baocao;
                wb.Range(cell, 1, cell, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).
                        Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(true).Alignment.SetWrapText(true)
                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(cell, 3).Value = TONGKIENNGHI_.ToString("#,##0").Replace(",", ".");
                wb.Cell(cell, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(cell, 4).Value = TONGKIENNGHI_TRALOI_.ToString("#,##0").Replace(",", ".");
                wb.Cell(cell, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(cell, 5).Value = TONGKIENNGHI_DAGIAIQUYET_.ToString("#,##0").Replace(",", ".");
                wb.Cell(cell, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(cell, 6).Value = TONGKIENNGHI_DANGGIAIQUYET_.ToString("#,##0").Replace(",", ".");
                wb.Cell(cell, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(cell, 7).Value = TONGKIENNGHI_GIAITRINH_.ToString("#,##0").Replace(",", ".");
                wb.Cell(cell, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                cell++;
                int TONGKIENNGHI_tyle_ = 100; double TONGKIENNGHI_TRALOI_tyle_ = 0; double TONGKIENNGHI_DAGIAIQUYET_tyle_ = 0;
                double TONGKIENNGHI_DANGGIAIQUYET_tyle_ = 0; double TONGKIENNGHI_GIAITRINH_tyle_ = 0;
                if (TONGKIENNGHI_ > 0)
                {
                    TONGKIENNGHI_TRALOI_tyle_ = Math.Round(Convert.ToDouble(TONGKIENNGHI_TRALOI_) * 100 / Convert.ToDouble(TONGKIENNGHI_), 2);
                    TONGKIENNGHI_DAGIAIQUYET_tyle_ = Math.Round(Convert.ToDouble(TONGKIENNGHI_DAGIAIQUYET_) * 100 / Convert.ToDouble(TONGKIENNGHI_), 2);
                    TONGKIENNGHI_DANGGIAIQUYET_tyle_ = Math.Round(Convert.ToDouble(TONGKIENNGHI_DANGGIAIQUYET_) * 100 / Convert.ToDouble(TONGKIENNGHI_), 2);
                    TONGKIENNGHI_GIAITRINH_tyle_ = Math.Round(Convert.ToDouble(TONGKIENNGHI_GIAITRINH_) * 100 / Convert.ToDouble(TONGKIENNGHI_), 2);
                }
                wb.Row(cell).Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(cell, 1).Value = "Tỷ lệ ";
                wb.Range(cell, 1, cell, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(true).Alignment.SetWrapText(true);
                wb.Cell(cell, 3).Value = TONGKIENNGHI_tyle_ + " %";
                wb.Cell(cell, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(cell, 4).Value = TONGKIENNGHI_TRALOI_tyle_ + " %";
                wb.Cell(cell, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(cell, 5).Value = TONGKIENNGHI_DAGIAIQUYET_tyle_ + " %";
                wb.Cell(cell, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(cell, 6).Value = TONGKIENNGHI_DANGGIAIQUYET_tyle_ + " %";
                wb.Cell(cell, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(cell, 7).Value = TONGKIENNGHI_GIAITRINH_tyle_ + " %";
                wb.Cell(cell, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                cell++;
                wb.Rows().AdjustToContents();
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=phuluc1.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: phụ lục 1");
                return View("../Home/Error_Exception");
            }
        }
        [HttpPost]
        public ActionResult Ajax_Kiennghi_phuluc1(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(fc["iKyHop"]);
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                int iLinhVuc = Convert.ToInt32(fc["iLinhVuc"]);
                //List<KIENNGHIPHULUC1> dt = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, 0);
                ViewData["kyhop"] = _kn.Get_TenKyHop(iKyHop) + ", " + _kn.Get_TenKhoaHop_By_IDKyHop(iKyHop);
                ViewData["list"] = _kn.BAOCAO_TK_PHULUC1(iKyHop, iDonVi, iLinhVuc);
                return PartialView("../Ajax/Baocaokiennghi/Kiennghi_1");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax: phụ lục 1"); throw;
            }

        }

        /* Tra ve list cac dia phuong 1 (con cua dia phuong 0 co IDIAPHUONG nhu dau vao)
         */
        public string Get_Option_DiaPhuong1(int iparent = 0)
        {
            string str = "<option  value='0'>Chọn huyện/thành phố/thị xã </option>";
            if (iparent == 0)//thêm mới -> load địa phương gắn vs đơn vị của tài khoản
            {
                UserInfor u = GetUserInfor();
                int iDonVi_User = (int)u.user_login.IDONVI;
                QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(iDonVi_User);
                if (coquan.IDIAPHUONG != null && coquan.IDIAPHUONG != 0)
                {
                    iparent = (int)coquan.IDIAPHUONG;
                }
            }
            if (iparent == 0) { return str; }
            var diaphuong = _kiennghi.GetAll_DiaPhuong().Where(x => x.IHIENTHI == 1 && x.IPARENT == iparent).OrderBy(x => x.CTEN).ToList();
            foreach (var d in diaphuong)
            {
                str += "<option value='" + d.IDIAPHUONG + "'>" + d.CTYPE + " " + Server.HtmlEncode(d.CTEN) + "</option>";
            }
            return str;
        }

        /*  HaiPN16
         *  Tao ra string cho 1 checkbox HTML de chon Loai don (Quoc hoi/ Hoi dong nhan dan)
         */
        public string Get_CheckBox_LoaiDon()
        {

            string str = "";
            str += "<span class = span5'><input class='nomargin' type='radio' onclick='ChangeDoiTuongGui()' name ='iLoaiDon' id ='iLoaiDon0' value = '0' checked/>" +
                "<label> Quốc hội </label> </span>";
            str += "<input class='nomargin' type='radio' name='iLoaiDon' onclick='ChangeDoiTuongGui()' id ='iLoaiDon1' value = '1'/>" +
                "<label> Hội đồng nhân dân </label>";
            return str;
        }

        /*  HaiPN16
         *  Controller cho trang lựa chọn đầu vào để xuất báo cáo hiển thị
         */
        public ActionResult Ketqua()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-linhvuc"] = Get_Option_DonVi_Linhvuc();
                // Gioi han dropdown don vi ve dia phuong hien tai
                int iDiaPhuong = AppConfig.IDIAPHUONG;
                ViewData["opt-coquan"] = Get_Option_DiaPhuong1(iDiaPhuong);
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                ViewData["check-loai"] = Get_CheckBox_LoaiDon();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Kết quả hoạt động");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult Ajax_Kiennghi_ketqua(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);
                int iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);
                int iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                int iLoaiDon = Convert.ToInt32(Request["iLoaiDon"]);
                List<KIENNGHI_KETQUA> list = kn_report.getReportBaoCaoKetQua(iKyHop, iTruocKyHop, iDonVi, iLinhVuc, iLoaiDon);
                String tenKhoa = _kn.Get_TenKhoaHop_By_IDKyHop(iKyHop);
                //List<KIENNGHIPHULUC1> dt = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, 0);
                ViewData["kyhop"] = _kn.Get_TenKyHop(iKyHop).ToUpper();
                ViewData["khoa"] = _kn.Get_TenKhoaHop_By_IDKyHop(iKyHop).ToUpper();
                if (iLoaiDon == 0)
                    ViewData["loaidon"] = "Quốc hội";
                else
                    ViewData["loaidon"] = "Thường trực Hội đồng nhân dân";
                String str = "";
                int soThuTu = 1;
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        str += "<tr>" +
                            "<td>";
                        str += soThuTu++;

                        str += "</td>" +
                            "<td>" +
                            item.TEN_DIA_PHUONG +
                            "</td>" +
                            "<td>" +
                            item.CNOIDUNG +
                            "</td>" +
                            "<td>" +
                            item.TEN_LINH_VUC +
                            "</td>" +
                            "</tr>";
                    }
                }
                ViewData["list"] = str;
                return PartialView("../Ajax/Baocaokiennghi/Ketqua_preview");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax: Kết quả hoạt động"); throw;
            }

        }

        /* HaiPN16
         * Tạo file excel
         */
        public ActionResult Kiennghi_Ketqua_excel()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iDonVi = Convert.ToInt32(Request["iDonVi"]);
                int iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);
                int iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                int iLoaiDon = Convert.ToInt32(Request["iLoaiDon"]);
                String tenKhoa = _kn.Get_TenKhoaHop_By_IDKyHop_KhongEncode(iKyHop).ToUpper();
                List<KIENNGHI_KETQUA> list = kn_report.getReportBaoCaoKetQua(iKyHop, iTruocKyHop, iDonVi, iLinhVuc, iLoaiDon);
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Kết quả hoạt động");
                wb.Style.Font.FontSize = 12;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(1).Width = 10;
                wb.Column(2).Width = 30;
                wb.Column(3).Width = 50;
                wb.Column(4).Width = 25;
                wb.Row(5).Height = 42;
                wb.Row(3).Height = 30;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Cell(1, 1).Value = "HỘI ĐỒNG NHÂN DÂN";
                wb.Range(1, 1, 1, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                     .Font.SetBold(true);
                wb.Cell(1, 3).Value = "CỘNG HOÀ XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(1, 3, 1, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                     .Font.SetBold(true);
                wb.Cell(2, 1).Value = "TỈNH THANH HÓA";
                wb.Range(2, 1, 2, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                     .Font.SetBold(true);
                wb.Cell(2, 3).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(2, 3, 2, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                     .Font.SetBold(true);
                wb.Cell(3, 3).Value = "Thanh Hóa, ngày....tháng....năm...";
                wb.Range(3, 3,3,4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                     .Font.SetItalic(true);
                String str1 = "";
                str1 = "TỔNG HỢP CHI TIẾT Ý KIẾN, KIẾN NGHỊ CỦA CỬ TRI VÀ NHÂN DÂN TẠI CÁC HUYỆN, THỊ XÃ, THÀNH PHỐ GỬI TỚI ";
                String redStr = _kn.Get_TenKyHop(iKyHop).ToUpper() + ", ";
                if (iLoaiDon == 0)
                    redStr += "QUỐC HỘI";
                else
                    redStr += "HỘI ĐỒNG NHÂN DÂN";
                String str2 = " " + tenKhoa;
                wb.Cell(5, 1).Value = str1 + redStr + str2;
                wb.Range(5, 1, 5, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                     .Font.SetBold(true).Alignment.SetWrapText(true);

                str1 = "(Kèm theo Công văn số        /......... ngày.....tháng.....năm 20.... của ";
                if (iLoaiDon == 0)
                    str1 += " Quốc hội tỉnh Thanh Hoá)";
                else
                    str1 += "Thường trực Hội đồng nhân dân tỉnh Thanh Hoá)";


                wb.Cell(6, 1).Value = str1;
                wb.Range(6, 1, 6, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                     .Font.SetItalic(true).Alignment.SetWrapText(true);
                wb.Cell(8, 1).Value = "STT";
                wb.Cell(8, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(8, 2).Value = "Đơn vị";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(8, 3).Value = "Nội dung";
                wb.Cell(8, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(8, 4).Value = "Lĩnh vực";
                wb.Cell(8, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                int rowIncremental = 0;
                int soThuTu = 1;
                foreach (var item in list)
                {
                    wb.Cell(9 + rowIncremental, 1).Value = soThuTu++;
                    wb.Cell(9 + rowIncremental, 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                   .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                   .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    wb.Cell(9 + rowIncremental, 2).Value = item.TEN_DIA_PHUONG;
                    wb.Cell(9 + rowIncremental, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).
                        Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left); ;
                    wb.Cell(9 + rowIncremental, 3).Value = item.CNOIDUNG;
                    wb.Cell(9 + rowIncremental, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);
                    wb.Cell(9 + rowIncremental, 4).Value = item.TEN_LINH_VUC;
                    wb.Cell(9 + rowIncremental, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left); ;
                    rowIncremental++;
                }
                int track = 0;
                String temp = wb.Cell(9, 2).GetString();
                for (int i = 1; i < rowIncremental; i++)
                {
                    if(wb.Cell(9 + i, 2).GetString() != temp)
                    {
                        wb.Range(9 + track, 2 , 9 + i - 1, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        track = i;
                        temp = wb.Cell(9 + track, 2).GetString();
                    }

                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ketqua.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Excel: Kết quả");
                return View("../Home/Error_Exception");
            }
        }
    }
}
