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
namespace KienNghi.Controllers
{
    public class BaocaokntcController : BaseController
    {
        //
        // GET: /Baocao/
        Funtions func = new Funtions();
        Khieunai kn = new Khieunai();
        KntcReport kntcrpt = new KntcReport();
        KntcBusineess kntc = new KntcBusineess();
        BaseBusineess _base = new BaseBusineess();
        ThietlapBusineess _thietlap = new ThietlapBusineess();
        Log log = new Log();
        public ActionResult Ajax_LoadLinhVucNoiDung(int iLinhVuc)
        {

            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                string str = "<select name='iNoiDung' id='iNoiDung' class='input-medium chosen-select' style='width:100%' onchange='LoadOpTinhChat()'>" +
                                            "<option value='0'>Chọn tất cả</option>" +
                                            "" + kn.Option_NoiDungDon_ThuocLinhVuc(0, iLinhVuc) + "" +
                                            "</select>";
                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Load lĩnh vực nội dung ");
                return null;
            }
        }
        public ActionResult Ajax_LoadTinhChatNoiDung(int iNoiDung)
        {

            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {

                string str = "<select name='iTinhChat' id='iTinhChat' class='input-medium chosen-select' style='width:100%'>" +
                                            "<option value='0'>Chọn tất cả</option>" +
                                            "" + kn.Option_TinhChatDon_ThuocNguonDon(0, iNoiDung) + "" + "</select>";
                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Load tính chất nội dung ");
                //Handle Exception;
                return null;
            }
        }
        public ActionResult Ajax_LoadLinhVucByLoaiDon(int iLoaiDon)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                string str = "<select name='iLinhVuc' id='iLinhVuc' class='input-medium chosen-select' onchange=\"LoadLinhVuc()\" style='width:100%'>" +
                                            "<option value='0'>Chọn tất cả</option>" +
                                            "" + kn.Option_LinhVuc_LoaiDon(0, iLoaiDon) + "" + "</select>";

                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Load tính chất nội dung ");
                //Handle Exception;
                return null;
            }
        }
        public ActionResult Loaikhieuto()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan,0, 0, 0, 0);
            
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Loaikhieuto(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidung = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            ViewData["data"] = kn.Thongkeloaikhieuto(tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi,inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongkeloaikhieuto");

        }
        

        public ActionResult Loaikhieuto_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var list = kntcrpt.getReportBaoBaoThongKeLoaiKhieuTo("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_LOAIKHIEUTO", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list == null)
                {
                    list = new List<LOAIKHIEUTO>();
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("LaiKhieuTo");
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(4).Width = 40;
                wb.Column(5).Width = 10;
                wb.Column(6).Width = 10;
                wb.PageSetup.FitToPages(1, 1);
                wb.Rows("8").Height = 20;
                wb.Cell(2, 2).Value = "Ban Dân Nguyện";
                wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(2, 4).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(2, 4, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.SetBold(true);
                wb.Cell(3, 2).Value = "VỤ I";
                wb.Range(3, 2, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 4).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(3, 4, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(5, 2).Value = "KẾT QUẢ THỐNG KÊ THEO LOẠI KHIẾU TỐ";
                wb.Range(5, 2, 5, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                wb.Range(6, 2, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(8, 2).Value = "STT";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 3).Value = "Loại khiếu tố";

                wb.Range(8, 3, 8, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(8, 5).Value = "Số lượng";
                wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 6).Value = "Tỷ lệ (%)";
                wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Rows().AdjustToContents();

                int j = 9;
                decimal soluong = 0;
                decimal tyle = 0;
                foreach (var t in list)
                {

                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 3).Value = Server.HtmlDecode(t.CTEN);
                    wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 5).Value = t.SOLUONG;
                    wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 6).Value = t.TYLE;
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    soluong += t.SOLUONG;
                    tyle += t.TYLE;
                    j++;
                }
                wb.Cell(j, 2).Value = "";
                wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 3).Value = "TỔNG SỐ";
                wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 5).Value = soluong;
                wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 6).Value = tyle;
                wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j + 2, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
                wb.Range(j + 2, 4, j + 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(j + 3, 4).Value = "NGƯỜI LẬP BÁO CÁO";
                wb.Range(j + 3, 4, j + 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Loaikhieuto_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo khiếu nại tố cáo theo loại khiếu tố");
                return null;
            }
        }
        //new Báo cáo đơn thư hàng tuần
        public ActionResult Baocaodonthuhangtuan()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            int iKyHop = ID_KyHop_HienTai();
            ViewData["opt-nam"] = Get_Option_Nam();

            return View();
        }

        public string Get_Option_Nam()
        {
            string str = "";
            int Ynow = DateTime.Now.Year;
            for(int i = Ynow- 20; i<= Ynow + 20; i++)
            {
                str += "<option value='" + i + "'>Năm " + i + "</option>";
            }
            return str;
        }

        [HttpPost]
        public ActionResult Ajax_Xembaocao_Baocaodonthuhangtuan(FormCollection fc)
        {
            UserInfor u = GetUserInfor();

            DateTime tungay = DateTime.MinValue; if (fc["dTuNgay"] != null && fc["dTuNgay"] != "")
            {
                tungay = Convert.ToDateTime(func.ConvertDateToSql(fc["dTuNgay"]));
            }

            DateTime denngay = DateTime.MaxValue; if (fc["dDenNgay"] != null && fc["dDenNgay"] != "")
            {
                denngay = Convert.ToDateTime(func.ConvertDateToSql(fc["dDenNgay"]));
            }
            int iNam = Convert.ToInt32(Request["iNam"]);
            ViewData["data"] = kn.Baocaodonthuhangtuan(tungay, denngay, iNam);
            return PartialView("../Ajax/Baocao/Kntc_Baocaodonthuhangtuan");

        }

        public ActionResult Baocaodonthuhangtuan_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                DateTime tungay = DateTime.MinValue; if (Request["tungay"] != null && Request["tungay"] != "")
                {
                    tungay = Convert.ToDateTime(func.ConvertDateToSql(Request["tungay"]));
                }

                DateTime denngay = DateTime.MaxValue; if (Request["denngay"] != null && Request["denngay"] != "")
                {
                    denngay = Convert.ToDateTime(func.ConvertDateToSql(Request["denngay"]));
                }

                string nam = denngay.ToString("yyyy");
                string thang = denngay.ToString("MM");

                int iNam = Convert.ToInt32(Request["iNam"]);
                var list = kntcrpt.getReportBaocaodonthuhangtuan("PKG_KNTC_BAOCAO.PRO_BAOCAO_DONTHUHANGTUAN", tungay, denngay, iNam);
                if(list == null)
                {
                    list = new List<BAOCAODONTHUHANGTUAN>();
                }
                
                var list1 = list.Where(x => x.ITINHTRANGXULY == 6).ToList();
                var list1_vanbantraloi = list.Where(x => x.CTENCOQUAN2 == "Văn phòng Đoàn đại biểu Quốc hội tỉnh và HĐND tỉnh").ToList();
                var list1_vanbanchuyen = list.Where(x => x.CTENCOQUAN2 != "Văn phòng Đoàn đại biểu Quốc hội tỉnh và HĐND tỉnh").ToList();
                var list2 = list.Where(x => x.ITINHTRANGXULY == 3 && x.IDOITUONGGUI == 0).ToList();
                var list3 = list.Where(x => x.ITINHTRANGXULY == 3 && x.IDOITUONGGUI == 1).ToList();
                XLWorkbook w_b = new XLWorkbook();
                var wb_chung = w_b.Worksheets.Add("Chung");
                var wb_dsdvtraloi = w_b.Worksheets.Add("DSDVTRALOI");
                var wb_dbqh = w_b.Worksheets.Add("DBQH");
                var wb_hdnd = w_b.Worksheets.Add("HDND");


                wb_chung.Column(1).Width = 10;
                wb_chung.Column(2).Width = 30;
                wb_chung.Column(3).Width = 30;
                wb_chung.Column(4).Width = 30;
                wb_chung.Column(5).Width = 40;
                wb_chung.Column(6).Width = 40;
                wb_chung.Column(7).Width = 30;

                wb_chung.Style.Font.FontSize = 13;
                wb_chung.Style.Font.FontName = "Times New Roman";
                wb_chung.PageSetup.FitToPages(1, 1);
                wb_chung.Rows("8").Height = 20;
                wb_chung.Cell(2, 1).Value = "VĂN PHÒNG ĐOÀN ĐBQH VÀ";
                wb_chung.Range(2, 1, 2, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(3, 1).Value = "HĐND TỈNH THANH HÓA";
                wb_chung.Range(3, 1, 3, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(4, 1).Value = "Số:...../BC-VP";
                wb_chung.Range(4, 1, 4, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetUnderline()
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(2, 5).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb_chung.Range(2, 5, 3, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(4, 5).Value = "Độc lập - Tự do - Hạnh phúc";
                wb_chung.Range(4, 5, 4, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetUnderline().Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(5, 5).Value = "Thanh Hóa, ngày...tháng...năm 2022";
                wb_chung.Range(5, 5, 5, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetItalic(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(6, 1).Value = "BÁO CÁO";
                wb_chung.Range(6, 1, 6, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(7, 1).Value = "Kết quả tiếp công dân, tiếp nhận, xử lý đơn khiếu nại, tố cáo từ ngày " + tungay.ToString("dd/MM/yyyy") + " đến ngày " + denngay.ToString("dd/MM/yyyy") + " của Đoàn đại biểu Quốc hội và";
                wb_chung.Range(7, 1, 7, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(8, 1).Value = "Thường trực HĐND Tỉnh Thanh Hóa";
                wb_chung.Range(8, 1, 8, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                
                wb_chung.Cell(9, 1).Value = "1. Tình hình tiếp công dân tại trụ sở tiếp công dân tỉnh";
                wb_chung.Range(9, 1, 9, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(10, 1).Value = "1.1. Đoàn đại biểu Quốc hội tỉnh";
                wb_chung.Range(10, 1, 10, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                    .Font.SetBold(true).Font.SetItalic(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(11, 1).Value = "1.2. Thường trực Hội đồng nhân dân tỉnh ";
                wb_chung.Range(11, 1, 11, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                    .Font.SetBold(true).Font.SetItalic(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(12, 1).Value = "2. Về kết quả tiếp nhận, phân loại, xử lý đơn, theo dõi việc giải quyết đơn khiếu nại, tố cáo, phản ánh, kiến nghị của công dân";
                wb_chung.Range(12, 1, 12, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(13, 1).Value = "2.1. Đoàn đại biểu Quốc hội tỉnh";
                wb_chung.Range(13, 1, 13, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                    .Font.SetBold(true).Font.SetItalic(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(14, 1).Value = "2.2. Thường trực Hội đồng nhân dân tỉnh";
                wb_chung.Range(14, 1, 14, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                    .Font.SetBold(true).Font.SetItalic(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_chung.Cell(15, 1).Value = "3. Các vụ việc nổi cộm, phức tạp";
                wb_chung.Range(15, 1, 15, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Column(1).Width = 10;
                wb_dsdvtraloi.Column(2).Width = 30;
                wb_dsdvtraloi.Column(3).Width = 30;
                wb_dsdvtraloi.Column(4).Width = 30;
                wb_dsdvtraloi.Column(5).Width = 40;
                wb_dsdvtraloi.Column(6).Width = 40;
                wb_dsdvtraloi.Column(7).Width = 30;
                wb_dsdvtraloi.Row(6).Height = 40;
                wb_dsdvtraloi.Style.Font.FontSize = 13;
                wb_dsdvtraloi.Style.Font.FontName = "Times New Roman";
                wb_dsdvtraloi.PageSetup.FitToPages(1, 1);
                wb_dsdvtraloi.Rows("8").Height = 20;
                wb_dsdvtraloi.Cell(1, 1).Value = "VĂN PHÒNG ĐOÀN ĐBQH VÀ";
                wb_dsdvtraloi.Range(1, 1, 1, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Cell(2, 1).Value = "HĐND TỈNH THANH HÓA";
                wb_dsdvtraloi.Range(2, 1, 2, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Cell(1, 5).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb_dsdvtraloi.Range(1, 5, 1, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Cell(2, 5).Value = "Độc lập - Tự do - Hạnh phúc";
                wb_dsdvtraloi.Range(2, 5, 2, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetUnderline().Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb_dsdvtraloi.Cell(4, 1).Value = "DANH SÁCH CÁC ĐƠN VỊ ĐÃ TRẢ LỜI ĐƠN CHUYỂN CỦA ĐOÀN ĐBQH TỈNH VÀ THƯỜNG TRỰC HĐND TỈNH";
                wb_dsdvtraloi.Range(4, 1, 4, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Cell(4, 1).Value = "(Kèm theo Báo cáo số:....../BC-VP ngày....tháng "+ thang +" năm "+ nam +" của Văn phòng Đoàn ĐBQH và HĐND tỉnh Thanh Hóa)";
                wb_dsdvtraloi.Range(4, 1, 4, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetItalic(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb_dsdvtraloi.Cell(6, 1).Value = "TT";
                wb_dsdvtraloi.Cell(6, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Cell(6, 2).Value = "Tên đơn vị";
                wb_dsdvtraloi.Cell(6, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Cell(6, 3).Value = "Số ký hiệu văn bản";
                wb_dsdvtraloi.Cell(6, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Cell(6, 4).Value = "Ngày, tháng, năm văn bản";
                wb_dsdvtraloi.Cell(6, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Cell(6, 5).Value = "Nội dung trả lời đơn";
                wb_dsdvtraloi.Cell(6, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Cell(6, 6).Value = "Số CV, ngày tháng năm Đoàn ĐBQH chuyển đơn";
                wb_dsdvtraloi.Cell(6, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Cell(6, 7).Value = "Ghi chú";
                wb_dsdvtraloi.Cell(6, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_dsdvtraloi.Rows().AdjustToContents();

                int j = 0;
                int count_donthuhangtuan = 1;
                if (list != null)
                {
                    j = 7;
                    foreach (var d in list1_vanbantraloi)
                    {

                        wb_dsdvtraloi.Cell(j, 1).Value = count_donthuhangtuan;
                        wb_dsdvtraloi.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_dsdvtraloi.Cell(j, 2).Value = Server.HtmlDecode(d.CTENCOQUAN2);
                        wb_dsdvtraloi.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_dsdvtraloi.Cell(j, 3).Value = Server.HtmlDecode(d.CSOVANBAN);
                        wb_dsdvtraloi.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        if (d.DNGAYBANHANH != null) 
                            wb_dsdvtraloi.Cell(j, 4).Value = Server.HtmlDecode(func.ConvertDateVN(d.DNGAYBANHANH.ToString()));
                        wb_dsdvtraloi.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_dsdvtraloi.Cell(j, 5).Value = Server.HtmlDecode(d.CNOIDUNGVB);
                        wb_dsdvtraloi.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        var vanbanchuyen = list1_vanbanchuyen.Where(x => x.IDON == d.IDON).ToList();
                        foreach(var item in vanbanchuyen)
                        {

                            wb_dsdvtraloi.Cell(j, 6).Value += item.CSOVANBAN;
                            if (d.DNGAYBANHANH != null)
                                wb_dsdvtraloi.Cell(j, 6).Value += " ngày " + func.ConvertDateVN(item.DNGAYBANHANH.ToString());
                             wb_dsdvtraloi.Cell(j, 6).Value +=  " " + item.CTENCOQUAN2;
                            if (vanbanchuyen.Last() == item)
                                wb_dsdvtraloi.Cell(j, 6).Value += ".";
                            else
                            {
                                wb_dsdvtraloi.Cell(j, 6).Value += ", ";
                            }
                        }
                        wb_dsdvtraloi.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_dsdvtraloi.Cell(j, 7).Value = Server.HtmlDecode(d.GHICHU_XULY);
                        wb_dsdvtraloi.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);

                        j++;
                        count_donthuhangtuan++;
                    }
                    j = 1;
                    wb_dbqh.Column(1).Width = 10;
                    wb_dbqh.Column(2).Width = 30;
                    wb_dbqh.Column(3).Width = 30;
                    wb_dbqh.Column(4).Width = 30;
                    wb_dbqh.Column(5).Width = 40;
                    wb_dbqh.Column(6).Width = 40;
                    wb_dbqh.Column(7).Width = 30;

                    wb_dbqh.Style.Font.FontSize = 13;
                    wb_dbqh.Style.Font.FontName = "Times New Roman";
                    wb_dbqh.PageSetup.FitToPages(1, 1);
                    wb_dbqh.Rows("8").Height = 20;
                    wb_dbqh.Cell(j, 1).Value = "DANH SÁCH CHUYỂN ĐƠN THÁNG "+ thang +" NĂM "+ nam +" CỦA ĐOÀN ĐBQH TỈNH";
                    wb_dbqh.Range(j, 1, j, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Font.SetBold(true)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    j++;

                    wb_dbqh.Cell(j, 1).Value = "TT";
                    wb_dbqh.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_dbqh.Cell(j, 2).Value = "Ngày ban hành";
                    wb_dbqh.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_dbqh.Cell(j, 3).Value = "Số ký hiệu văn bản";
                    wb_dbqh.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_dbqh.Cell(j, 4).Value = "Công dân gửi đơn";
                    wb_dbqh.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_dbqh.Cell(j, 5).Value = "Trích yếu";
                    wb_dbqh.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_dbqh.Cell(j, 6).Value = "Nơi nhận VB chuyển của Đoàn ĐBQH tỉnh";
                    wb_dbqh.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_dbqh.Cell(j, 7).Value = "Ghi chú";
                    wb_dbqh.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    int count_donthuhangtuan2 = 1;
                    j++;
                    foreach (var d in list2)
                    {

                        wb_dbqh.Cell(j, 1).Value = count_donthuhangtuan2;
                        wb_dbqh.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        if(d.DNGAYBANHANH != null)
                            wb_dbqh.Cell(j, 2).Value = Server.HtmlDecode(func.ConvertDateVN(d.DNGAYBANHANH.ToString()));
                        wb_dbqh.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_dbqh.Cell(j, 3).Value = Server.HtmlDecode(d.CSOVANBAN);
                        wb_dbqh.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_dbqh.Cell(j, 4).Value = d.CNGUOIGUI_TEN +", "+ d.CNGUOIGUI_DIACHI;
                        wb_dbqh.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_dbqh.Cell(j, 5).Value = Server.HtmlDecode(d.CNOIDUNG);
                        wb_dbqh.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_dbqh.Cell(j, 6).Value = Server.HtmlDecode(d.CTENCOQUAN3);
                        wb_dbqh.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_dbqh.Cell(j, 7).Value = Server.HtmlDecode(d.CGHICHU);
                        wb_dbqh.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);

                        j++;
                        count_donthuhangtuan2++;
                    }
                    j = 1;
                    wb_hdnd.Column(1).Width = 10;
                    wb_hdnd.Column(2).Width = 30;
                    wb_hdnd.Column(3).Width = 30;
                    wb_hdnd.Column(4).Width = 30;
                    wb_hdnd.Column(5).Width = 40;
                    wb_hdnd.Column(6).Width = 40;
                    wb_hdnd.Column(7).Width = 30;

                    wb_hdnd.Style.Font.FontSize = 13;
                    wb_hdnd.Style.Font.FontName = "Times New Roman";
                    wb_hdnd.PageSetup.FitToPages(1, 1);
                    wb_hdnd.Rows("8").Height = 20;
                    wb_hdnd.Cell(j, 1).Value = "DANH SÁCH CHUYỂN ĐƠN THÁNG "+ thang +" NĂM "+ thang +" CỦA THƯỜNG TRỰC HĐND TỈNH";
                    wb_hdnd.Range(j, 1, j, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Font.SetBold(true)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    j++;
                    

                    wb_hdnd.Cell(j, 1).Value = "TT";
                    wb_hdnd.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_hdnd.Cell(j, 2).Value = "Ngày ban hành";
                    wb_hdnd.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_hdnd.Cell(j, 3).Value = "Số ký hiệu văn bản";
                    wb_hdnd.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_hdnd.Cell(j, 4).Value = "Công dân gửi đơn";
                    wb_hdnd.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_hdnd.Cell(j, 5).Value = "Trích yếu";
                    wb_hdnd.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_hdnd.Cell(j, 6).Value = "Nơi nhận VB chuyển của Đoàn ĐBQH tỉnh";
                    wb_hdnd.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb_hdnd.Cell(j, 7).Value = "Ghi chú";
                    wb_hdnd.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    int count_donthuhangtuan3 = 1;
                    j++;
                    foreach (var d in list3)
                    {

                        wb_hdnd.Cell(j, 1).Value = count_donthuhangtuan2;
                        wb_hdnd.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        if(d.DNGAYBANHANH != null)
                            wb_hdnd.Cell(j, 2).Value = Server.HtmlDecode(func.ConvertDateVN(d.DNGAYBANHANH.ToString()));
                        wb_hdnd.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_hdnd.Cell(j, 3).Value = Server.HtmlDecode(d.CSOVANBAN);
                        wb_hdnd.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_hdnd.Cell(j, 4).Value = d.CNGUOIGUI_TEN + ", " + d.CNGUOIGUI_DIACHI;
                        wb_hdnd.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_hdnd.Cell(j, 5).Value = Server.HtmlDecode(d.CNOIDUNG);
                        wb_hdnd.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_hdnd.Cell(j, 6).Value = Server.HtmlDecode(d.CTENCOQUAN3);
                        wb_hdnd.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_hdnd.Cell(j, 7).Value = Server.HtmlDecode(d.CGHICHU);
                        wb_hdnd.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);

                        j++;
                        count_donthuhangtuan3++;
                    }

                }


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Báo cáo đơn thư hàng tuần.xlsx");
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
                log.Log_Error(ex, "Báo cáo đơn thư hàng tuần");
                return null;
            }
        }

        //new Tổng hợp theo dõi giải quyết đơn long
        public ActionResult Theodoigiaiquyetdon()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            int iKyHop = ID_KyHop_HienTai();
            ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);

            return View();
        }
        public string Get_Option_KyHop(int iKyHop = 0)
        {
            List<QUOCHOI_KHOA> khoa = kntc.GetAll_KhoaHop().Where(x => x.ILOAI == 0).OrderBy(x => x.DBATDAU).ToList();
            return kn.Option_Khoa_KyHop(khoa, iKyHop);
        }

        [HttpPost]
        public ActionResult Ajax_Xembaocao_Theodoigiaiquyetdon(FormCollection fc)
        {
            UserInfor u = GetUserInfor();

            DateTime tungay = DateTime.MinValue; if (fc["dTuNgay"] != null && fc["dTuNgay"] != "")
            {
                tungay = Convert.ToDateTime(func.ConvertDateToSql(fc["dTuNgay"]));
            }

            DateTime denngay = DateTime.MaxValue; if (fc["dDenNgay"] != null && fc["dDenNgay"] != "")
            {
                denngay = Convert.ToDateTime(func.ConvertDateToSql(fc["dDenNgay"]));
            }
            int iKyHop = Convert.ToInt32(Request["iKyHop"]);
            int iLoai = Convert.ToInt32(Request["iLoai"]);
            ViewData["data"] = kn.Theodoigiaiquyetdon(tungay, denngay, iKyHop, iLoai);
            return PartialView("../Ajax/Baocao/Kntc_Tonghopcongvanchuyendon");

        }

        public ActionResult Theodoigiaiquyetdon_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                DateTime tungay = DateTime.MinValue; if (Request["tungay"] != null && Request["tungay"] != "")
                {
                    tungay = Convert.ToDateTime(func.ConvertDateToSql(Request["tungay"]));
                }

                DateTime denngay = DateTime.MaxValue; if (Request["denngay"] != null && Request["denngay"] != "")
                {
                    denngay = Convert.ToDateTime(func.ConvertDateToSql(Request["denngay"]));
                }
                
                int iKyHop = Convert.ToInt32(Request["iKyHop"]);
                int iloai = Convert.ToInt32(Request["iloai"]);
                string tenKhoa = kn.Get_TenKhoaHop_By_IDKyHop(iKyHop);
                var list = kntcrpt.getReportTheodoigiaiquyetdon("PKG_KNTC_BAOCAO.PRO_BAOCAO_THEODOIGQD", tungay, denngay, iKyHop, iloai);
                if (list == null)
                {
                    list = new List<Theodoigiaiquyetdon>();
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb_TheodoiGQD = w_b.Worksheets.Add("Theo dõi GQĐ");


                wb_TheodoiGQD.Column(1).Width = 10;
                wb_TheodoiGQD.Column(2).Width = 30;
                wb_TheodoiGQD.Column(3).Width = 30;
                wb_TheodoiGQD.Column(4).Width = 30;
                wb_TheodoiGQD.Column(5).Width = 40;
                wb_TheodoiGQD.Column(6).Width = 40;
                wb_TheodoiGQD.Column(7).Width = 30;
                wb_TheodoiGQD.Column(8).Width = 40;

                wb_TheodoiGQD.Style.Font.FontSize = 13;
                wb_TheodoiGQD.Style.Font.FontName = "Times New Roman";
                wb_TheodoiGQD.PageSetup.FitToPages(1, 1);
                wb_TheodoiGQD.Rows("8").Height = 20;
                wb_TheodoiGQD.Cell(2, 1).Value = "TỔNG HỢP CÔNG VĂN CHUYỂN ĐƠN GỬI ĐẾN ĐOÀN ĐBQH TỈNH "+ tenKhoa.ToUpper().Replace("KHóA","KHÓA");
                wb_TheodoiGQD.Range(2, 1, 2, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(13);
                wb_TheodoiGQD.Cell(3, 1).Value = "(Từ ngày "+ tungay.ToString("dd/MM/yyyy") + " đến ngày "+ denngay.ToString("dd/MM/yyyy") + " )";
                wb_TheodoiGQD.Range(3, 1, 3, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetItalic(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb_TheodoiGQD.Cell(5, 1).Value = "TT";
                wb_TheodoiGQD.Cell(5, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_TheodoiGQD.Cell(5, 2).Value = "Ngày ban hành";
                wb_TheodoiGQD.Cell(5, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_TheodoiGQD.Cell(5, 3).Value = "Số ký hiệu văn bản";
                wb_TheodoiGQD.Cell(5, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_TheodoiGQD.Cell(5, 4).Value = "Nơi nhận văn bản";
                wb_TheodoiGQD.Cell(5, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_TheodoiGQD.Cell(5, 5).Value = "Công dân gửi đơn";
                wb_TheodoiGQD.Cell(5, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_TheodoiGQD.Cell(5, 6).Value = "Trích yếu";
                wb_TheodoiGQD.Cell(5, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_TheodoiGQD.Cell(5, 7).Value = "Đã trả lời";
                wb_TheodoiGQD.Cell(5, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_TheodoiGQD.Cell(5, 8).Value = "Ghi chú";
                wb_TheodoiGQD.Cell(5, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_TheodoiGQD.Rows().AdjustToContents();
                
                int j = 0;
                int count_TheodoiGQD = 1;
                if (list != null)
                {
                    var list_theodoiGQD = list.ToList();
                    j = 6;
                    foreach (var d in list_theodoiGQD)
                    {

                        wb_TheodoiGQD.Cell(j, 1).Value = count_TheodoiGQD;
                        wb_TheodoiGQD.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        if(d.DNGAYBANHANH != null)
                            wb_TheodoiGQD.Cell(j, 2).Value = Server.HtmlDecode(func.ConvertDateVN(d.DNGAYBANHANH.ToString()));
                        wb_TheodoiGQD.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_TheodoiGQD.Cell(j, 3).Value = Server.HtmlDecode(d.CSOVANBAN);
                        wb_TheodoiGQD.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_TheodoiGQD.Cell(j, 4).Value = Server.HtmlDecode(d.CTENCOQUAN1);
                        wb_TheodoiGQD.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_TheodoiGQD.Cell(j, 5).Value = d.CNGUOIGUI_TEN + ", " + d.CNGUOIGUI_DIACHI;
                        wb_TheodoiGQD.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_TheodoiGQD.Cell(j, 6).Value = Server.HtmlDecode(d.CNOIDUNG);
                        wb_TheodoiGQD.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_TheodoiGQD.Cell(j, 7).Value = d.CSOVANBAN + " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString()) + " " + d.CTENCOQUAN2;
                        wb_TheodoiGQD.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_TheodoiGQD.Cell(j, 8).Value = d.GHICHU_XULY;
                        wb_TheodoiGQD.Cell(j, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);

                        j++;
                        count_TheodoiGQD ++;
                    }
                }


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Theo dõi giải quyết đơn.xlsx");
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
                log.Log_Error(ex, "Tổng hợp theo dõi giải quyết đơn");
                return null;
            }
        }

        //new công văn chuyển đơn long
        public ActionResult Congvanchuyendon()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);

            return View();
        }

        [HttpPost]
        public ActionResult Ajax_Xembaocao_Congvanchuyendon(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            //string tungay = "", denngay = "";
            //if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            //{
            //    tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            //}
            //if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            //{
            //    denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            //}

            DateTime tungay = DateTime.MinValue; if (fc["dTuNgay"] != null && fc["dTuNgay"] != "")
            {
                tungay = Convert.ToDateTime(func.ConvertDateToSql(fc["dTuNgay"]));
            }

            DateTime denngay = DateTime.MaxValue; if (fc["dDenNgay"] != null && fc["dDenNgay"] != "")
            {
                denngay = Convert.ToDateTime(func.ConvertDateToSql(fc["dDenNgay"]));
            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int iLoai = Convert.ToInt32(Request["iLoai"]);
            ViewData["data"] = kn.Congvanchuyendon(tungay, denngay, iloaidon, iLoai);
            return PartialView("../Ajax/Baocao/Kntc_Tonghopcongvanchuyendon");

        }

        

        public ActionResult Congvanchuyendon_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                DateTime tungay = DateTime.MinValue; if (Request["tungay"] != null && Request["tungay"] != "")
                {
                    tungay = Convert.ToDateTime(func.ConvertDateToSql(Request["tungay"]));
                }

                DateTime denngay = DateTime.MaxValue; if (Request["denngay"] != null && Request["denngay"] != "")
                {
                    denngay = Convert.ToDateTime(func.ConvertDateToSql(Request["denngay"]));
                }
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int iloai = Convert.ToInt32(Request["iloai"]);
                var list = kntcrpt.getReportBaoBaoCongvanchuyendon("PKG_KNTC_BAOCAO.PRO_BAOCAO_CVCHUYENDON", tungay, denngay, iloaidon, iloai);
                if (list == null)
                {
                    list = new List<CONGVANCHUYENDON>();
                }
                int count_congvanchuyendon = 1;
                int count_congvandondoc = 1;
                int count_congvantraloi = 1;
                int count_donluu = 1;
                XLWorkbook w_b = new XLWorkbook();
                var wb_congvanchuyendon = w_b.Worksheets.Add("CV chuyển đơn");
                var wb_congvandondoc = w_b.Worksheets.Add("Công văn đôn đốc");
                var wb_congvantraloi = w_b.Worksheets.Add("Văn bản trả lời công dân");
                var wb_donluu = w_b.Worksheets.Add("Đơn lưu");

                wb_congvanchuyendon.Column(1).Width = 10;
                wb_congvanchuyendon.Column(2).Width = 40;
                wb_congvanchuyendon.Column(3).Width = 60;
                wb_congvanchuyendon.Column(4).Width = 10;
                wb_congvanchuyendon.Column(5).Width = 10;
                wb_congvanchuyendon.Column(6).Width = 10;
                wb_congvanchuyendon.Column(7).Width = 10;
                wb_congvanchuyendon.Column(8).Width = 15;
                wb_congvanchuyendon.Column(9).Width = 10;
                wb_congvanchuyendon.Column(10).Width = 10;
                wb_congvanchuyendon.Column(11).Width = 25;
                wb_congvanchuyendon.Column(12).Width = 40;
                wb_congvanchuyendon.Column(13).Width = 10;

                wb_congvandondoc.Column(1).Width = 10;
                wb_congvandondoc.Column(2).Width = 40;
                wb_congvandondoc.Column(3).Width = 60;
                wb_congvandondoc.Column(4).Width = 10;
                wb_congvandondoc.Column(5).Width = 10;
                wb_congvandondoc.Column(6).Width = 10;
                wb_congvandondoc.Column(7).Width = 10;
                wb_congvandondoc.Column(8).Width = 15;
                wb_congvandondoc.Column(9).Width = 10;
                wb_congvandondoc.Column(10).Width = 10;
                wb_congvandondoc.Column(11).Width = 25;
                wb_congvandondoc.Column(12).Width = 40;
                wb_congvandondoc.Column(13).Width = 10;

                wb_congvantraloi.Column(1).Width = 10;
                wb_congvantraloi.Column(2).Width = 40;
                wb_congvantraloi.Column(3).Width = 60;
                wb_congvantraloi.Column(4).Width = 10;
                wb_congvantraloi.Column(5).Width = 10;
                wb_congvantraloi.Column(6).Width = 10;
                wb_congvantraloi.Column(7).Width = 10;
                wb_congvantraloi.Column(8).Width = 15;
                wb_congvantraloi.Column(9).Width = 10;
                wb_congvantraloi.Column(10).Width = 10;
                wb_congvantraloi.Column(11).Width = 25;
                wb_congvantraloi.Column(12).Width = 40;

                wb_donluu.Column(1).Width = 10;
                wb_donluu.Column(2).Width = 40;
                wb_donluu.Column(3).Width = 25;
                wb_donluu.Column(4).Width = 60;
                wb_donluu.Column(5).Width = 10;
                wb_donluu.Column(6).Width = 10;
                wb_donluu.Column(7).Width = 10;
                wb_donluu.Column(8).Width = 10;
                wb_donluu.Column(9).Width = 10;
                wb_donluu.Column(10).Width = 15;
                wb_donluu.Column(11).Width = 10;
                wb_donluu.Column(12).Width = 10;
                wb_donluu.Column(13).Width = 40;

                wb_congvanchuyendon.Style.Font.FontSize = 13;
                wb_congvanchuyendon.Style.Font.FontName = "Times New Roman";
                wb_congvanchuyendon.PageSetup.FitToPages(1, 1);
                wb_congvanchuyendon.Row(4).Height = 60;
                wb_congvanchuyendon.Cell(2, 1).Value = "CÔNG VĂN CHUYỂN ĐƠN CỦA VĂN PHÒNG ĐOÀN ĐBQH VÀ HĐND TỈNH";
                wb_congvanchuyendon.Range(2, 1, 2, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(13);

                //wb_congvanchuyendon.Cell(3, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                //wb_congvanchuyendon.Range(3, 2, 3, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(4, 1).Value = "STT";
                wb_congvanchuyendon.Range(4, 1 , 5 , 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(4, 2).Value = "Họ và tên, địa chỉ của công dân";
                wb_congvanchuyendon.Range(4, 2 , 5 , 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb_congvanchuyendon.Cell(4, 3).Value = "Nội dung đơn";
                wb_congvanchuyendon.Range(4, 3 , 5, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(4, 4).Value = "Phân loại đơn";
                wb_congvanchuyendon.Range(4, 4 , 4 , 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(5, 4).Value = "KN";
                wb_congvanchuyendon.Cell(5, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(5, 5).Value = "TC";
                wb_congvanchuyendon.Cell(5, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(5, 6).Value = "PAKN";
                wb_congvanchuyendon.Cell(5, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(4, 7).Value = "Lĩnh vực";
                wb_congvanchuyendon.Range(4, 7, 4, 10).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(5, 7).Value = "Đất đai";
                wb_congvanchuyendon.Cell(5, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(5, 8).Value = "Chính sách XH";
                wb_congvanchuyendon.Cell(5, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(5, 9).Value = "Tư pháp";
                wb_congvanchuyendon.Cell(5, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(5, 10).Value = "Khác";
                wb_congvanchuyendon.Cell(5, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(4, 11).Value = "Số công văn, ngày tháng chuyển đơn đến cơ quan có thẩm quyền";
                wb_congvanchuyendon.Range(4, 11 , 5 , 11).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(4, 12).Value = "Văn bản trả lời";
                wb_congvanchuyendon.Range(4, 12 , 5 , 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Cell(4, 13).Value = "Tổng số";
                wb_congvanchuyendon.Range(4, 13, 5 , 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvanchuyendon.Rows().AdjustToContents();


                wb_congvandondoc.Style.Font.FontSize = 13;
                wb_congvandondoc.Style.Font.FontName = "Times New Roman";
                wb_congvandondoc.PageSetup.FitToPages(1, 1);
                wb_congvandondoc.Rows("8").Height = 20;
                wb_congvandondoc.Row(5).Height = 40;
                wb_congvandondoc.Cell(2, 1).Value = "CÔNG VĂN ĐÔN ĐỐC CỦA VĂN PHÒNG HĐND TỈNH";
                wb_congvandondoc.Range(2, 1, 2, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(13);

                //wb_congvandondoc.Cell(3, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                //wb_congvandondoc.Range(3, 2, 3, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb_congvandondoc.Cell(4, 1).Value = "STT";
                wb_congvandondoc.Range(4, 1, 5, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(4, 2).Value = "Họ và tên, địa chỉ của công dân";
                wb_congvandondoc.Range(4, 2, 5, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb_congvandondoc.Cell(4, 3).Value = "Nội dung đơn";
                wb_congvandondoc.Range(4, 3, 5, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(4, 4).Value = "Phân loại đơn";
                wb_congvandondoc.Range(4, 4, 4, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(5, 4).Value = "KN";
                wb_congvandondoc.Cell(5, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(5, 5).Value = "TC";
                wb_congvandondoc.Cell(5, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(5, 6).Value = "PAKN";
                wb_congvandondoc.Cell(5, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(4, 7).Value = "Lĩnh vực";
                wb_congvandondoc.Range(4, 7, 4, 10).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(5, 7).Value = "Đất đai";
                wb_congvandondoc.Cell(5, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(5, 8).Value = "Chính sách XH";
                wb_congvandondoc.Cell(5, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(5, 9).Value = "Tư pháp";
                wb_congvandondoc.Cell(5, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(5, 10).Value = "Khác";
                wb_congvandondoc.Cell(5, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(4, 11).Value = "Số công văn đôn đốc, ngày tháng chuyển đơn đến cơ quan có thẩm quyền";
                wb_congvandondoc.Range(4, 11, 5, 11).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(4, 12).Value = "Văn bản trả lời";
                wb_congvandondoc.Range(4, 12, 5, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Cell(4, 13).Value = "Tổng số";
                wb_congvandondoc.Range(4, 13, 5, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvandondoc.Rows().AdjustToContents();

                wb_congvantraloi.Style.Font.FontSize = 13;
                wb_congvantraloi.Style.Font.FontName = "Times New Roman";
                wb_congvantraloi.PageSetup.FitToPages(1, 1);
                wb_congvantraloi.Rows("8").Height = 20;
                wb_congvantraloi.Cell(2, 1).Value = "CÔNG VĂN TRẢ LỜI, HƯỚNG DẪN CÔNG DÂN";
                wb_congvantraloi.Range(2, 1, 2, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(13);

                //wb_congvantraloi.Cell(3, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                //wb_congvantraloi.Range(3, 2, 3, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb_congvantraloi.Cell(4, 1).Value = "STT";
                wb_congvantraloi.Range(4, 1, 5, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(4, 2).Value = "Họ và tên, địa chỉ của công dân";
                wb_congvantraloi.Range(4, 2, 5, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb_congvantraloi.Cell(4, 3).Value = "Nội dung đơn";
                wb_congvantraloi.Range(4, 3, 5, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(4, 4).Value = "Phân loại đơn";
                wb_congvantraloi.Range(4, 4, 4, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(5, 4).Value = "KN";
                wb_congvantraloi.Cell(5, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(5, 5).Value = "TC";
                wb_congvantraloi.Cell(5, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(5, 6).Value = "PAKN";
                wb_congvantraloi.Cell(5, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(4, 7).Value = "Lĩnh vực";
                wb_congvantraloi.Range(4, 7, 4, 10).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(5, 7).Value = "Đất đai";
                wb_congvantraloi.Cell(5, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(5, 8).Value = "Chính sách XH";
                wb_congvantraloi.Cell(5, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(5, 9).Value = "Tư pháp";
                wb_congvantraloi.Cell(5, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(5, 10).Value = "Khác";
                wb_congvantraloi.Cell(5, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(4, 11).Value = "Số công văn, ngày tháng chuyển đơn đến cơ quan có thẩm quyền";
                wb_congvantraloi.Range(4, 11, 5, 11).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Cell(4, 12).Value = "Ghi chú";
                wb_congvantraloi.Range(4, 12, 5, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_congvantraloi.Rows().AdjustToContents();


                wb_donluu.Style.Font.FontSize = 13;
                wb_donluu.Style.Font.FontName = "Times New Roman";
                wb_donluu.PageSetup.FitToPages(1, 1);
                wb_donluu.Rows("8").Height = 20;
                wb_donluu.Cell(2, 1).Value = "ĐƠN LƯU";
                wb_donluu.Range(2, 1, 2, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(13);

                //wb_donluu.Cell(3, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                //wb_donluu.Range(3, 2, 3, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb_donluu.Cell(4, 1).Value = "STT";
                wb_donluu.Cell(4, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_donluu.Cell(4, 2).Value = "Họ tên công dân";
                wb_donluu.Cell(4, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb_donluu.Cell(4, 3).Value = "Ngày tháng nhận đơn";
                wb_donluu.Cell(4, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_donluu.Cell(4, 4).Value = "Nội dung đơn";
                wb_donluu.Cell(4, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                
                wb_donluu.Cell(4, 5).Value = "Số đơn gửi";
                wb_donluu.Cell(4, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_donluu.Cell(4, 6).Value = "Khiếu nại";
                wb_donluu.Cell(4, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_donluu.Cell(4, 7).Value = "Tố cáo";
                wb_donluu.Cell(4, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_donluu.Cell(4, 8).Value = "Phản ánh, KN";
                wb_donluu.Cell(4, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_donluu.Cell(4, 9).Value = "Đất đai";
                wb_donluu.Cell(4, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_donluu.Cell(4, 10).Value = "Chính sách XH";
                wb_donluu.Cell(4, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_donluu.Cell(4, 11).Value = "Tư pháp";
                wb_donluu.Cell(4, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb_donluu.Cell(4, 12).Value = "Lĩnh vực khác";
                wb_donluu.Cell(4, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb_donluu.Cell(4, 13).Value = "Ghi chú";
                wb_donluu.Cell(4, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb_donluu.Rows().AdjustToContents();

                int j = 0;

                if (list != null)
                {
                    var list_congvanchuyendon_chuacotraloi = list.Where(x => x.ICOQUANBANHANH == 4 && x.ITINHTRANGXULY == 3 && x.CLOAI == "chuyenxuly_noibo").ToList();
                    var list_congvanchuyendon_dacotraloi = list.Where(x => x.ICOQUANBANHANH == 4 && x.ITINHTRANGXULY == 6).ToList();
                    var list_congvandondoc = list.Where(x => x.ITINHTRANGXULY == 3).ToList();
                    var list_congvandondoc_1 = list_congvandondoc.Where(x => x.CLOAI != "vanbandondocthuchien").ToList();
                    var list_congvantraloi = list.Where(x => x.ITINHTRANGXULY == 6 && x.CLOAI == "huongdan_traloi").ToList();
                    var list_donluu = list.Where(x => x.ITINHTRANGXULY == 5).ToList();

                    j = 6;
                    // Truong hop chua co cau tra loi
                    //foreach (var d in list_congvanchuyendon_chuacotraloi)
                    //{
                    //    string KN = "";
                    //    string TC = "";
                    //    string PAKN = "";
                    //    string DatDai = "";
                    //    string ChinhSacXH = "";
                    //    string TuPhap = "";
                    //    string Khac = "";
                    //    int TongSo = 0;

                    //    //var vanBanTraLoi = list_congvanchuyendon.Where(x => x.ICOQUANBANHANH != 4 && x.IDON == d.IDON && x.CLOAI == "hoanthanh").FirstOrDefault();
                    //    //if (vanBanTraLoi == null)
                    //    //    continue;
                    //    if (d.ILOAIDON == 1)
                    //    {
                    //        KN = "1";
                    //        TongSo++;
                    //    }
                    //    else if (d.ILOAIDON == 2)
                    //    {
                    //        TC = "1";
                    //        TongSo++;
                    //    }
                    //    else if (d.ILOAIDON == 3)
                    //    {
                    //        PAKN = "1";
                    //        TongSo++;
                    //    }

                    //    if (d.ILINHVUC == 35) DatDai = "1";
                    //    else if (d.ILINHVUC == 4) ChinhSacXH = "1";
                    //    else if (d.ILINHVUC == 5) TuPhap = "1";
                    //    else if (d.ILINHVUC == 6) Khac = "1";

                        
                    //    wb_congvanchuyendon.Cell(j, 1).Value = count_congvanchuyendon;
                    //    wb_congvanchuyendon.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 2).Value = Server.HtmlDecode(d.CNGUOIGUI_TEN);
                    //    wb_congvanchuyendon.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    //                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 3).Value = Server.HtmlDecode(d.CNOIDUNG);
                    //    wb_congvanchuyendon.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    //                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 4).Value = KN;
                    //    wb_congvanchuyendon.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 5).Value = TC;
                    //    wb_congvanchuyendon.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 6).Value = PAKN;
                    //    wb_congvanchuyendon.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 7).Value = DatDai;
                    //    wb_congvanchuyendon.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 8).Value = ChinhSacXH;
                    //    wb_congvanchuyendon.Cell(j, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 9).Value = TuPhap;
                    //    wb_congvanchuyendon.Cell(j, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 10).Value = Khac;
                    //    wb_congvanchuyendon.Cell(j, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 11).Value = "Số " + d.CSOVANBAN + " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString()) + " chuyển " + d.CTEN;
                    //    wb_congvanchuyendon.Cell(j, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 12).Value = "Chưa có văn bản trả lời";
                    //    wb_congvanchuyendon.Cell(j, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    //    wb_congvanchuyendon.Cell(j, 13).Value = TongSo;
                    //    wb_congvanchuyendon.Cell(j, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    //                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    //                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);


                    //    j++;
                    //    count_congvanchuyendon++;
                    //}

                    foreach (var d in list_congvanchuyendon_dacotraloi)
                    {
                        string KN = "";
                        string TC = "";
                        string PAKN = "";
                        string DatDai = "";
                        string ChinhSacXH = "";
                        string TuPhap = "";
                        string Khac = "";
                        int TongSo = 0;

                        var vanBanTraLoi = list.Where(x => x.ICOQUANBANHANH != 4 && x.IDON == d.IDON && x.CLOAI == "hoanthanh" && x.ITINHTRANGXULY == 6).ToList();
                        if (vanBanTraLoi.Count() == 0)
                            continue;
                        if (d.ILOAIDON == 1)
                        {
                            KN = "1";
                            TongSo++;
                        }
                        else if (d.ILOAIDON == 2)
                        {
                            TC = "1";
                            TongSo++;
                        }
                        else if (d.ILOAIDON == 3)
                        {
                            PAKN = "1";
                            TongSo++;
                        }

                        if (d.ILINHVUC == 35) DatDai = "1";
                        else if (d.ILINHVUC == 4) ChinhSacXH = "1";
                        else if (d.ILINHVUC == 5) TuPhap = "1";
                        else if (d.ILINHVUC == 6) Khac = "1";


                        wb_congvanchuyendon.Cell(j, 1).Value = count_congvanchuyendon;
                        wb_congvanchuyendon.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 2).Value = Server.HtmlDecode(d.CNGUOIGUI_TEN);
                        wb_congvanchuyendon.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 3).Value = Server.HtmlDecode(d.CNOIDUNG);
                        wb_congvanchuyendon.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 4).Value = KN;
                        wb_congvanchuyendon.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 5).Value = TC;
                        wb_congvanchuyendon.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 6).Value = PAKN;
                        wb_congvanchuyendon.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 7).Value = DatDai;
                        wb_congvanchuyendon.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 8).Value = ChinhSacXH;
                        wb_congvanchuyendon.Cell(j, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 9).Value = TuPhap;
                        wb_congvanchuyendon.Cell(j, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 10).Value = Khac;
                        wb_congvanchuyendon.Cell(j, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 11).Value = "Số " + d.CSOVANBAN;
                        if (d.DNGAYBANHANH != null)
                            wb_congvanchuyendon.Cell(j, 11).Value += " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString());
                        wb_congvanchuyendon.Cell(j, 11).Value += " chuyển " + d.CTEN;
                        wb_congvanchuyendon.Cell(j, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        string str = "";
                        foreach (var item in vanBanTraLoi)
                        {
                            str += "Số " + item.CSOVANBAN;
                            if (d.DNGAYBANHANH != null)
                                str += " ngày " + func.ConvertDateVN(item.DNGAYBANHANH.ToString());
                            str += " từ " + item.CTEN;
                            if (item == vanBanTraLoi.Last())
                                str += ".";
                            else
                                str += ", ";
                        }
                            
                        wb_congvanchuyendon.Cell(j, 12).Value = str;
                        wb_congvanchuyendon.Cell(j, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvanchuyendon.Cell(j, 13).Value = TongSo;
                        wb_congvanchuyendon.Cell(j, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);


                        j++;
                        count_congvanchuyendon++;
                    }



                    j = 6;
                    foreach (var d in list_congvandondoc_1)
                    {
                        string KN = "";
                        string TC = "";
                        string PAKN = "";
                        string DatDai = "";
                        string ChinhSacXH = "";
                        string TuPhap = "";
                        string Khac = "";
                        int TongSo = 0;
                        var vanBanTraLoi = list.Where(x => x.ITINHTRANGXULY == 3 && x.IDON == d.IDON && x.CLOAI == "vanbandondocthuchien").ToList();
                        if (vanBanTraLoi.Count == 0)// Khong phai la van ban don doc
                            continue;
                        if (d.ILOAIDON == 1)
                        {
                            KN = "1";
                            TongSo++;
                        }
                        else if (d.ILOAIDON == 2)
                        {
                            TC = "1";
                            TongSo++;
                        }
                        else if (d.ILOAIDON == 3)
                        {
                            PAKN = "1";
                            TongSo++;
                        }

                        if (d.ILINHVUC == 35) DatDai = "1";
                        else if (d.ILINHVUC == 4) ChinhSacXH = "1";
                        else if (d.ILINHVUC == 5) TuPhap = "1";
                        else if (d.ILINHVUC == 6) Khac = "1";

                        wb_congvandondoc.Cell(j, 1).Value = count_congvandondoc;
                        wb_congvandondoc.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 2).Value = Server.HtmlDecode(d.CNGUOIGUI_TEN);
                        wb_congvandondoc.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 3).Value = Server.HtmlDecode(d.CNOIDUNG);
                        wb_congvandondoc.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 4).Value = KN;
                        wb_congvandondoc.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 5).Value = TC;
                        wb_congvandondoc.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 6).Value = PAKN;
                        wb_congvandondoc.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 7).Value = DatDai;
                        wb_congvandondoc.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 8).Value = ChinhSacXH;
                        wb_congvandondoc.Cell(j, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 9).Value = TuPhap;
                        wb_congvandondoc.Cell(j, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 10).Value = Khac;
                        wb_congvandondoc.Cell(j, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 11).Value = "Số " + d.CSOVANBAN;
                        if(d.DNGAYBANHANH != null)
                            wb_congvandondoc.Cell(j, 11).Value += " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString());
                        wb_congvandondoc.Cell(j, 11).Value += " chuyển " + d.CTEN;
                        wb_congvandondoc.Cell(j, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        string str = "";
                        foreach (var item in vanBanTraLoi)
                        {
                            str += "Số " + item.CSOVANBAN;
                            if (item.DNGAYBANHANH != null)
                                str += " ngày " + func.ConvertDateVN(item.DNGAYBANHANH.ToString());
                            str+= " từ " + item.CTEN;
                            if (item == vanBanTraLoi.Last())
                                str += ".";
                            else
                                str += ", ";
                        }
                            wb_congvandondoc.Cell(j, 12).Value = str;
                        //else
                        //    wb_congvandondoc.Cell(j, 12).Value = "Chưa có trả lời";
                        wb_congvandondoc.Cell(j, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvandondoc.Cell(j, 13).Value = TongSo;
                        wb_congvandondoc.Cell(j, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);


                        j++;
                        count_congvandondoc++;
                    }

                    j = 6;
                    foreach (var d in list_congvantraloi)
                    {
                        string KN = "";
                        string TC = "";
                        string PAKN = "";
                        string DatDai = "";
                        string ChinhSacXH = "";
                        string TuPhap = "";
                        string Khac = "";
                        int TongSo = 0;

                        if (d.ILOAIDON == 1)
                        {
                            KN = "1";
                            TongSo++;
                        }
                        else if (d.ILOAIDON == 2)
                        {
                            TC = "1";
                            TongSo++;
                        }
                        else if (d.ILOAIDON == 3)
                        {
                            PAKN = "1";
                            TongSo++;
                        }

                        if (d.ILINHVUC == 35) DatDai = "1";
                        else if (d.ILINHVUC == 4) ChinhSacXH = "1";
                        else if (d.ILINHVUC == 5) TuPhap = "1";
                        else if (d.ILINHVUC == 6) Khac = "1";

                        wb_congvantraloi.Cell(j, 1).Value = count_congvantraloi;
                        wb_congvantraloi.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 2).Value = Server.HtmlDecode(d.CNGUOIGUI_TEN);
                        wb_congvantraloi.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 3).Value = Server.HtmlDecode(d.CNOIDUNG);
                        wb_congvantraloi.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 4).Value = KN;
                        wb_congvantraloi.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 5).Value = TC;
                        wb_congvantraloi.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 6).Value = PAKN;
                        wb_congvantraloi.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 7).Value = DatDai;
                        wb_congvantraloi.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 8).Value = ChinhSacXH;
                        wb_congvantraloi.Cell(j, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 9).Value = TuPhap;
                        wb_congvantraloi.Cell(j, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 10).Value = Khac;
                        wb_congvantraloi.Cell(j, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 11).Value = "Số " + d.CSOVANBAN;
                        if (d.DNGAYBANHANH != null)
                            wb_congvantraloi.Cell(j, 11).Value += " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString());
                        wb_congvantraloi.Cell(j, 11).Value += " " + d.CTEN;
                        wb_congvantraloi.Cell(j, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_congvantraloi.Cell(j, 12).Value = Server.HtmlDecode(d.GHICHU_XULY);
                        wb_congvantraloi.Cell(j, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);


                        j++;
                        count_congvantraloi++;
                    }

                    j = 5;
                    foreach (var d in list_donluu)
                    {
                        string KN = "";
                        string TC = "";
                        string PAKN = "";
                        string DatDai = "";
                        string ChinhSacXH = "";
                        string TuPhap = "";
                        string Khac = "";
                        int TongSo = 0;

                        if (d.ILOAIDON == 1)
                        {
                            KN = "1";
                            TongSo++;
                        }
                        else if (d.ILOAIDON == 2)
                        {
                            TC = "1";
                            TongSo++;
                        }
                        else if (d.ILOAIDON == 3)
                        {
                            PAKN = "1";
                            TongSo++;
                        }

                        if (d.ILINHVUC == 35)
                        {
                            DatDai = "1";
                            TongSo++;
                        }
                        else if (d.ILINHVUC == 4)
                        {
                            ChinhSacXH = "1";
                            TongSo++;
                        }
                        else if (d.ILINHVUC == 5)
                        {
                            TuPhap = "1";
                            TongSo++;
                        }
                        else if (d.ILINHVUC == 6)
                        {
                            Khac = "1";
                            TongSo++;
                        }

                        wb_donluu.Cell(j, 1).Value = count_donluu;
                        wb_donluu.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 2).Value = Server.HtmlDecode(d.CNGUOIGUI_TEN);
                        wb_donluu.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        if(d.DNGAYNHAN != null)
                            wb_donluu.Cell(j, 3).Value = func.ConvertDateVN(d.DNGAYNHAN.ToString());
                        wb_donluu.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 4).Value = Server.HtmlDecode(d.CNOIDUNG);
                        wb_donluu.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 5).Value = TongSo;
                        wb_donluu.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 6).Value = KN;
                        wb_donluu.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 7).Value = TC;
                        wb_donluu.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 8).Value = PAKN;
                        wb_donluu.Cell(j, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 9).Value = DatDai;
                        wb_donluu.Cell(j, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 10).Value = ChinhSacXH;
                        wb_donluu.Cell(j, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 11).Value = TuPhap;
                        wb_donluu.Cell(j, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 12).Value = Khac;
                        wb_donluu.Cell(j, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);
                        wb_donluu.Cell(j, 13).Value = Server.HtmlDecode(d.CLUUTHEODOI_LYDO);
                        wb_donluu.Cell(j, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                        .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                        .Alignment.SetWrapText(true).Font.SetFontSize(11);


                        j++;
                        count_donluu++;
                    }
                }


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Tổng hợp đơn (Công văn chuyển đơn).xlsx");
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
                log.Log_Error(ex, "Tổng hợp đơn (Công văn chuyển đơn)");
                return null;
            }
        }
        public ActionResult Coquanthamquyen()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
     
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan, 0, 0, 0, 0);
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Coquanthamquyen(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            int Loai = Convert.ToInt32(fc["iLoaiBaoCao"]);
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidung = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            ViewData["data"] = kn.Thongkecoquanthamquyen(tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongkecoquanthamquyen");

        }
        public ActionResult Coquanthamquyen_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var list = kntcrpt.getReportBaoBaoThongKeCoquanthamquyen("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_TQGQ", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list == null)
                {
                    list = new List<THAMQUYENGIAIQUYET>();
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("LaiKhieuTo");
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(4).Width = 40;
                wb.Column(5).Width = 10;
                wb.Column(6).Width = 10;
                wb.PageSetup.FitToPages(1, 1);
                wb.Cell(2, 2).Value = "Ban Dân Nguyện";
                wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(2, 4).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(2, 4, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 2).Value = "VỤ I";
                wb.Range(3, 2, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 4).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(3, 4, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(11);
                wb.Cell(5, 2).Value = "KẾT QUẢ THỐNG KÊ THEO THẨM QUYỀN GIẢI QUYẾT";
                wb.Range(5, 2, 5, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                wb.Range(6, 2, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(11);
                wb.Cell(8, 2).Value = "STT";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 3).Value = "Thẩm quyền giải quyết	";
                wb.Range(8, 3, 8, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 5).Value = "Số lượng";
                wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 6).Value = "Tỷ lệ (%)";
                wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Rows().AdjustToContents();

                int j = 9;
                decimal soluong = 0;
                decimal tyle = 0;
                foreach (var t in list)
                {
                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 3).Value = Server.HtmlDecode(t.CTEN);
                    wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 5).Value = t.SOLUONG;
                    wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 6).Value = t.TYLE;
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    soluong += t.SOLUONG;
                    tyle += t.TYLE;
                    j++;
                }
                wb.Cell(j, 2).Value = "";
                wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 3).Value = "TỔNG SỐ";
                wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 5).Value = soluong;
                wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 6).Value = tyle;
                wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j + 2, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
                wb.Range(j + 2, 4, j + 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(j + 3, 4).Value = "NGƯỜI LẬP BÁO CÁO";
                wb.Range(j + 3, 4, j + 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Coquanthamquyen_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo khiếu nại tố cáo theo cơ quan thẩm quyền");
                return null;
            }
        }
        public ActionResult Noiguidon()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
       
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan,0, 0, 0, 0);
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Noiguidon(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            int Loai = Convert.ToInt32(fc["iLoaiBaoCao"]);
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidung = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            ViewData["data"] = kn.Thongkenoiguidon(tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongkenoiguidon");

        }
        public ActionResult Noiguidon_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var list = kntcrpt.getReportBaoBaoThongKeNoiGuiDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_NOIGUIDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list == null)
                {
                    list = new List<NOIGUIDON>();
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("NguoiGuiDon");
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(4).Width = 40;
                wb.Column(5).Width = 10;
                wb.Column(6).Width = 10;
                wb.PageSetup.FitToPages(1, 1);
                wb.Cell(2, 2).Value = "Ban Dân Nguyện";
                wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                wb.Cell(2, 4).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(2, 4, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 2).Value = "VỤ I";
                wb.Range(3, 2, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 4).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(3, 4, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);

                wb.Cell(5, 2).Value = "KẾT QUẢ THỐNG KÊ THEO NƠI GỬI ĐƠN";
                wb.Range(5, 2, 5, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                wb.Range(6, 2, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(8, 2).Value = "STT";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 3).Value = "Nơi gửi đơn";
                wb.Range(8, 3, 8, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 5).Value = "Số lượng";
                wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 6).Value = "Tỷ lệ (%)";
                wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Rows().AdjustToContents();

                int j = 9;
                decimal soluong = 0;
                decimal tyle = 0;
                foreach (var t in list)
                {

                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 3).Value = Server.HtmlDecode(t.CTEN);
                    wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 5).Value = t.SOLUONG;
                    wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 6).Value = t.TYLE;
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    soluong += t.SOLUONG;
                    tyle += t.TYLE;
                    j++;
                }
                wb.Cell(j, 2).Value = "";
                wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 3).Value = "TỔNG SỐ";
                wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 5).Value = soluong;
                wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 6).Value = tyle;
                wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j + 2, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
                wb.Range(j + 2, 4, j + 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(j + 3, 4).Value = "NGƯỜI LẬP BÁO CÁO";
                wb.Range(j + 3, 4, j + 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Noiguidon_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo khiếu nại tố cáo theo nơi gửi đơn");
                return null;
            }
        }
        public ActionResult Nguoinhapdon()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
       
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan,0, 0, 0, 0);
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Nguoinhapdon(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            int Loai = Convert.ToInt32(fc["iLoaiBaoCao"]);
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidung = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            ViewData["data"] = kn.Thongkenguoinhapdon(tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongkenguoinhapdon");

        }
        public ActionResult Nguoinhapdon_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var list = kntcrpt.getReportBaoBaoThongKeNguoiNhapDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_NGUOINHAPDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list == null)
                {
                    list = new List<NGUOINHAPDON>();
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("NguoiNhapDon");
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";

                wb.Column(4).Width = 40;
                wb.Column(5).Width = 10;
                wb.Column(6).Width = 10;
                wb.PageSetup.FitToPages(1, 1);
                wb.Cell(2, 2).Value = "Ban Dân Nguyện";
                wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                wb.Cell(2, 4).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(2, 4, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 2).Value = "VỤ I";
                wb.Range(3, 2, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 4).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(3, 4, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);

                wb.Cell(5, 2).Value = "KẾT QUẢ THỐNG KÊ THEO NGƯỜI NHẬP ĐƠN";
                wb.Range(5, 2, 5, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                wb.Range(6, 2, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(8, 2).Value = "STT";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 3).Value = "Cán bộ nhập đơn";
                wb.Range(8, 3, 8, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 5).Value = "Số lượng";
                wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 6).Value = "Tỷ lệ (%)";
                wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Rows().AdjustToContents();

                int j = 9;
                decimal soluong = 0;
                decimal tyle = 0;
                foreach (var t in list)
                {

                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 3).Value = Server.HtmlDecode(t.CTEN);
                    wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 5).Value = t.SOLUONG;
                    wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 6).Value = t.TYLE;
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    soluong += t.SOLUONG;
                    tyle += t.TYLE;
                    j++;
                }
                wb.Cell(j, 2).Value = "";
                wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 3).Value = "TỔNG SỐ";
                wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 5).Value = soluong;
                wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 6).Value = tyle;
                wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j + 2, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
                wb.Range(j + 2, 4, j + 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(j + 3, 4).Value = "NGƯỜI LẬP BÁO CÁO";
                wb.Range(j + 3, 4, j + 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Nguoinhapdon_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo khiếu nại tố cáo theo người nhập đơn");
                return null;
            }
        }
        public ActionResult Nguoixuly()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
          
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan,0, 0, 0, 0);
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Nguoixuly(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            int Loai = Convert.ToInt32(fc["iLoaiBaoCao"]);
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidung = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            ViewData["data"] = kn.Thongkenguoixuly(tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongkenguoixuly");

        }
        public ActionResult Nguoixuly_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var list = kntcrpt.getReportBaoBaoThongKeNguoiXuLy("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_NGUOIXULY", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list == null)
                {
                    list = new List<NGUOIXULY>();
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("NguoiXuLy");
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(4).Width = 40;
                wb.Column(5).Width = 10;
                wb.Column(6).Width = 10;
                wb.PageSetup.FitToPages(1, 1);
                wb.Cell(2, 2).Value = "Ban Dân Nguyện";
                wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                wb.Cell(2, 4).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(2, 4, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 2).Value = "VỤ I";
                wb.Range(3, 2, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 4).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(3, 4, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);

                wb.Cell(5, 2).Value = "KẾT QUẢ THỐNG KÊ THEO NGƯỜI XỬ LÝ ĐƠN";
                wb.Range(5, 2, 5, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                wb.Range(6, 2, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(8, 2).Value = "STT";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 3).Value = "Cán bộ xử lý";
                wb.Range(8, 3, 8, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 5).Value = "Số lượng";
                wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 6).Value = "Tỷ lệ (%)";
                wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Rows().AdjustToContents();

                int j = 9;
                decimal soluong = 0;
                decimal tyle = 0;
                foreach (var t in list)
                {

                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 3).Value = Server.HtmlDecode(t.CTEN);
                    wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 5).Value = t.SOLUONG;
                    wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 6).Value = t.TYLE;
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    soluong += t.SOLUONG;
                    tyle += t.TYLE;
                    j++;
                }
                wb.Cell(j, 2).Value = "";
                wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 3).Value = "TỔNG SỐ";
                wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 5).Value = soluong;
                wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 6).Value = tyle;
                wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);

                wb.Cell(j + 2, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
                wb.Range(j + 2, 4, j + 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(j + 3, 4).Value = "NGƯỜI LẬP BÁO CÁO";
                wb.Range(j + 3, 4, j + 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Nguoixuly_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo khiếu nại tố cáo theo nơi gửi đơn");
                return null;
            }
        }
        public ActionResult Coquanchuyendon()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
          
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan,0, 0, 0, 0);
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Coquanchuyendon(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            int Loai = Convert.ToInt32(fc["iLoaiBaoCao"]);
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidung = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            ViewData["data"] = kn.Thongkecoquanchuyendon(tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongkecoquanchuyendon");

        }
        public ActionResult Coquanchuyendon_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var list = kntcrpt.getReportBaoBaoThongKecoquanchuyendon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_COQUANCHUYENDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list == null)
                {
                    list = new List<COQUANCHUYENDON>();
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Coquanchuyendon");
                wb.Style.Font.FontName = "Times New Roman";
                wb.Style.Font.FontSize = 13;
                wb.Column(4).Width = 40;
                wb.Column(5).Width = 10;
                wb.Column(6).Width = 10;
                wb.PageSetup.FitToPages(1, 1);

                wb.Cell(2, 2).Value = "Ban Dân Nguyện";
                wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                wb.Cell(2, 4).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(2, 4, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 2).Value = "VỤ I";
                wb.Range(3, 2, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 4).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(3, 4, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);

                wb.Cell(5, 2).Value = "KẾT QUẢ THỐNG KÊ THEO CƠ QUAN CHUYỂN ĐƠN";
                wb.Range(5, 2, 5, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                wb.Range(6, 2, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(8, 2).Value = "STT";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 3).Value = "Cơ quan chuyển đơn";
                wb.Range(8, 3, 8, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 5).Value = "Số lượng";
                wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(8, 6).Value = "Tỷ lệ (%)";
                wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Rows().AdjustToContents();

                int j = 9;
                decimal soluong = 0;
                decimal tyle = 0;
                foreach (var t in list)
                {

                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 3).Value = Server.HtmlDecode(t.CTEN);
                    wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 5).Value = t.SOLUONG;
                    wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    wb.Cell(j, 6).Value = t.TYLE;
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true).Font.SetFontSize(11);
                    soluong += t.SOLUONG;
                    tyle += t.TYLE;
                    j++;
                }
                wb.Cell(j, 2).Value = "";
                wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 3).Value = "TỔNG SỐ";
                wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 5).Value = soluong;
                wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 6).Value = "100";
                wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j + 2, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
                wb.Range(j + 2, 4, j + 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(j + 3, 4).Value = "NGƯỜI LẬP BÁO CÁO";
                wb.Range(j + 3, 4, j + 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Coquanchuyendon_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo khiếu nại tố cáo theo cơ quan chuyển đơn");
                return null;
            }
        }
        public ActionResult Trungdon()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
          
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan,0, 0, 0, 0);
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Trungdon(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            int Loai = Convert.ToInt32(fc["iLoaiBaoCao"]);
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidung = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            ViewData["data"] = kn.Thongketrungdon(tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongketrungdon");

        }
        public ActionResult Trungdon_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var list = kntcrpt.getReportBaoBaoThongKeTrungDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_TRUNGDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list == null)
                {
                    list = new List<TRUNGDON>();
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Trungdon");
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(4).Width = 20;
                wb.Column(5).Width = 10;
                wb.Column(6).Width = 40;
                wb.Column(7).Width = 10;
                wb.PageSetup.FitToPages(1, 1);
                wb.Cell(2, 2).Value = "Ban Dân Nguyện";
                wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                wb.Cell(2, 4).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(2, 4, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 2).Value = "VỤ I";
                wb.Range(3, 2, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 4).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(3, 4, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);

                wb.Cell(5, 2).Value = "BÁO CÁO THỐNG KÊ ĐƠN TRÙNG";
                wb.Range(5, 2, 5, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                wb.Range(6, 2, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(8, 2).Value = "STT";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 3).Value = "Tên địa danh";
                wb.Range(8, 3, 8, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 5).Value = "Số đơn trùng";
                wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 6).Value = "Nội dung đơn";
                wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 7).Value = "Số lần trùng";
                wb.Cell(8, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Rows().AdjustToContents();

                int j = 9;
                decimal soluong = 0;
                decimal soluongtrung = 0;

                foreach (var t in list)
                {

                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                    wb.Cell(j, 3).Value = Server.HtmlDecode(t.CTENDIADANH);
                    wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 5).Value = t.SODONTRUNG;
                    wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 6).Value = Server.HtmlDecode(t.CNOIDUNGDON);
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 7).Value = t.SOLANTRUNG;
                    wb.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    soluong += t.SODONTRUNG;
                    soluongtrung += t.SOLANTRUNG;

                    j++;
                }
                wb.Cell(j, 2).Value = "";
                wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 3).Value = "TỔNG SỐ";
                wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 5).Value = soluong;
                wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 6).Value = "";
                wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 7).Value = soluongtrung;
                wb.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);

                wb.Cell(j + 2, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
                wb.Range(j + 2, 4, j + 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(j + 3, 4).Value = "NGƯỜI LẬP BÁO CÁO";
                wb.Range(j + 3, 4, j + 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Trungdon_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo khiếu nại tố cáo theo đơn trùng");
                return null;
            }
        }
        public ActionResult Tongsodon()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
          
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan,0, 0, 0, 0);
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Tongsodon(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            int Loai = Convert.ToInt32(fc["iLoaiBaoCao"]);
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidung = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            ViewData["data"] = kn.Thongketongsodon(tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongketongsodon");

        }
        public ActionResult Tongsodon_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var list = kntcrpt.getReportBaoBaoThongKeTongSoDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_TONGSODON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list == null)
                {
                    list = new List<TONGSODON>();
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Tongsodon");
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(4).Width = 40;
                wb.Column(5).Width = 20;
                wb.Column(6).Width = 10;
                wb.Column(7).Width = 10;
                wb.PageSetup.FitToPages(1, 1);
                wb.Cell(2, 2).Value = "Ban Dân Nguyện";
                wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                wb.Cell(2, 4).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(2, 4, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 2).Value = "VỤ I";
                wb.Range(3, 2, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 4).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(3, 4, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);

                wb.Cell(5, 2).Value = "BÁO CÁO TỔNG SỐ ĐƠN";
                wb.Range(5, 2, 5, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                wb.Range(6, 2, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(8, 2).Value = "STT";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 3).Value = "Loại khiếu tố";
                wb.Range(8, 3, 8, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 5).Value = "Số đơn đủ điều kiện";
                wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 6).Value = "Số lượng trùng";
                wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 7).Value = "Tỷ lệ (%)";
                wb.Cell(8, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Rows().AdjustToContents();

                int j = 9;
                decimal soluong = 0;
                decimal soluongtrung = 0;
                decimal tyle = 0;
                foreach (var t in list)
                {

                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                    wb.Cell(j, 3).Value = Server.HtmlDecode(t.CLOAIKHIEUTO);
                    wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 5).Value = t.SODONDUDIEUKIEN;
                    wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 6).Value = t.SODONTRUG;
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 7).Value = t.TYLE;
                    wb.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    soluong += t.SODONDUDIEUKIEN;
                    soluongtrung += t.SODONTRUG;
                    j++;
                }
                tyle = soluong == 0 ? 0 : (soluongtrung / soluong) * 100;
                wb.Cell(j, 2).Value = "";
                wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 3).Value = "TỔNG SỐ";
                wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 5).Value = soluong;
                wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 6).Value = soluongtrung;
                wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                wb.Cell(j, 7).Value = tyle;
                wb.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                    .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);

                wb.Cell(j + 2, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
                wb.Range(j + 2, 4, j + 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(j + 3, 4).Value = "NGƯỜI LẬP BÁO CÁO";
                wb.Range(j + 3, 4, j + 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Tongsodon_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo khiếu nại tố cáo theo tổng số đơn");
                return null;
            }
        }

        public ActionResult Chitietdon()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
          
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan,0, 0, 0, 0);
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Chitietdon(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            int Loai = Convert.ToInt32(fc["iLoaiBaoCao"]);
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidung = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            ViewData["data"] = kn.Thongkechitietdon(tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongkechitietdon");

        }
        public ActionResult Chitietdon_Exl()
        {
            UserInfor a = GetUserInfor();
            int itiepnhan = (int)a.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", a.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var list = kntcrpt.getReportBaoBaoThongKeChiTietDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_CHIITETDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list == null)
                {
                    list = new List<CHIITETDON>();
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Chi tiết đơn");
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(2).Width = 5;
                wb.Column(3).Width = 15;
                wb.Column(4).Width = 20;
                wb.Column(5).Width = 25;
                wb.Column(6).Width = 10;
                wb.Column(7).Width = 10;
                wb.Column(8).Width = 35;
                wb.Column(9).Width = 30;
                wb.Column(9).Width = 50;
                wb.Column(10).Width = 30;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb.Cell(2, 2).Value = "Ban Dân Nguyện";
                wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                wb.Cell(2, 6).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(2, 6, 2, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 2).Value = "VỤ I";
                wb.Range(3, 2, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 6).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(3, 6, 3, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(5, 2).Value = "BÁO CÁO CHI TIẾT XỬ LÝ ĐƠN THƯ";
                wb.Range(5, 2, 5, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                wb.Range(6, 2, 6, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(8, 2).Value = "STT";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);

                wb.Cell(8, 3).Value = "Ngày vào sổ";
                wb.Cell(8, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 4).Value = "Họ và tên";
                wb.Cell(8, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 5).Value = "Địa chỉ";
                wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 6).Value = "Số người";
                wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 7).Value = "Số lần";
                wb.Cell(8, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 8).Value = "Nội dung";
                wb.Cell(8, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 9).Value = "Loại đơn";
                wb.Cell(8, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 10).Value = "Chuyển đến";
                wb.Cell(8, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Rows().AdjustToContents();

                int j = 9;

                foreach (var t in list)
                {

                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);

                    wb.Cell(j, 3).Value = func.ConvertDateVN(t.NGAYVAOSO.ToShortDateString());
                    wb.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 4).Value = Server.HtmlDecode(t.HOVATEN);
                    wb.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 5).Value = Server.HtmlDecode(t.DIACHI);
                    wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    int songuoi = (int) t.SONGUOI; if (t.SONGUOI == 0) { songuoi = 1; }
                    wb.Cell(j, 6).Value = songuoi;
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 7).Value = t.SOLAN;
                    wb.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 8).Value = Server.HtmlDecode(t.NOIDUNG);
                    wb.Cell(j, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 9).Value = Server.HtmlDecode(t.LOAIDON);
                    wb.Cell(j, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 10).Value = Server.HtmlDecode(t.CHUYENDEN);
                    wb.Cell(j, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    j++;
                }


                wb.Cell(j + 2, 8).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
                wb.Range(j + 2, 8, j + 2, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(j + 3, 8).Value = "NGƯỜI LẬP BÁO CÁO";
                wb.Range(j + 3, 8, j + 3, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Chitietdon_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo khiếu nại tố cáo theo chi tiết đơn");
                return null;
            }
        }
        public ActionResult Theodiaban()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
          
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan,0, 0, 0, 0);
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Theodiaban(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            int Loai = Convert.ToInt32(fc["iLoaiBaoCao"]);
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidung = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            ViewData["data"] = kn.Thongkediaban1(tungay, denngay, iloaidon, ilinhvuc, inoidung, itinhchat, idonvi, inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongkediaban");
        }
        public ActionResult Theodiaban_Exl()
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var list = kntcrpt.getReportBaoBaoThongKeDiaBan1("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_DIABAN1", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list == null)
                {
                    list = new List<DIABAN>();
                }
                XLWorkbook w_b = new XLWorkbook();

                var wb = w_b.Worksheets.Add("Theodiaban");
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.PageSetup.FitToPages(1, 1);
                wb.Column(4).Width = 30;
                wb.Column(5).Width = 40;

                wb.Cell(2, 2).Value = "Ban Dân Nguyện";
                wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                wb.Cell(2, 4).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(2, 4, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 2).Value = "VỤ I";
                wb.Range(3, 2, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(3, 4).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(3, 4, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(5, 2).Value = "BÁO CÁO THỐNG KÊ THEO THEO ĐỊA BÀN";
                wb.Range(5, 2, 5, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(6, 2).Value = "Từ ngày " + Request["tungay"] + " đến ngày " + Request["denngay"] + "";
                wb.Range(6, 2, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(8, 2).Value = "STT";
                wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 3).Value = "Họ và tên";
                wb.Range(8, 3, 8, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 5).Value = "Địa bàn";
                wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Cell(8, 6).Value = "Số lượng";
                wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                wb.Rows().AdjustToContents();


                int j = 9;

                foreach (var t in list)
                {

                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                    wb.Cell(j, 3).Value = Server.HtmlDecode(t.CTEN);
                    wb.Range(j, 3, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);

                    wb.Cell(j, 5).Value = Server.HtmlDecode(t.CDIABAN);
                    wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 6).Value = t.SOLUONG;
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);

                    j++;
                }
                j += 2;
                var list2 = kntcrpt.getReportBaoBaoThongKeDiaBan2("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_DIABAN2", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (list2 == null)
                {
                    list2 = new List<DIABANTONG>();
                }
                foreach (var t in list2)
                {

                    wb.Cell(j, 2).Value = t.STT;
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                    wb.Cell(j, 3).Value = Server.HtmlDecode(t.CTEN);
                    wb.Range(j, 3, j, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Font.SetFontSize(11)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true);
                    wb.Cell(j, 6).Value = t.SOLUONG;
                    wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                  .Alignment.SetWrapText(true);

                    j++;
                }

                wb.Cell(j + 2, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
                wb.Range(j + 2, 4, j + 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(j + 3, 4).Value = "NGƯỜI LẬP BÁO CÁO";
                wb.Range(j + 3, 4, j + 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Theodiaban_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo khiếu nại tố cáo theo địa bàn");
                return null;
            }
        }

        public ActionResult Solieudon()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("47", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
            ViewData["opt-noidung"] = kn.Option_NoiDungDon(0, 0);
            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0, 0);
            ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0, 0);
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
            Dictionary<string, object> condition = new Dictionary<string, object>();
          
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
            ViewData["opt-donvi"] = "<option value=\"0\">- - - Chọn tất cả </option>" + kn.OptionCoQuan(coquan,0, 0, 0, 0);
            return View();
        }
        [HttpPost]
        public ActionResult Ajax_Xembaocao_Solieudon(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            int itiepnhan = (int)u.user_login.IDONVI;
            int Loai = Convert.ToInt32(fc["iLoaiBaoCao"]);
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            }
            int iloaidon = Convert.ToInt32(fc["iLoaidon"]);
            int ilinhvuc = Convert.ToInt32(fc["iLinhVuc"]);
            int inoidung = Convert.ToInt32(fc["iNoiDung"]);
            int itinhchat = Convert.ToInt32(fc["iTinhChat"]);
            int idonvi = Convert.ToInt32(Request["iDonVi"]);
            int type = Convert.ToInt32(fc["itype"]);
            int inguondon = Convert.ToInt32(Request["iNguonDon"]);
            //ViewData["title"] = kn.TieuDeBaoCao();
            ViewData["data"] = kn.TieuDeBaoCao(tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
            return PartialView("../Ajax/Baocao/Kntc_Thongkesolieudon");

        }
        public ActionResult TongHopSoLieu_Exl()
        {
            UserInfor user = GetUserInfor();
            int itiepnhan = (int)user.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", user.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Request["tungay"]);
                string denngay = func.ConvertDateToSql(Request["denngay"]);
                int iloaidon = Convert.ToInt32(Request["iLoaidon"]);
                int ilinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidung = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int idonvi = Convert.ToInt32(Request["iDonVi"]);
                int inguondon = Convert.ToInt32(Request["iNguonDon"]);
                var linhvuc = kntcrpt.getReportNhomLinhVuc("PKG_KNTC_BAOCAO.PRO_LIST_NHOMLINHVUC");
                if (linhvuc == null)
                {
                    linhvuc = new List<LINHVUC>();
                }
                var nguondon = kntcrpt.getReportNguonDon("PKG_KNTC_BAOCAO.PRO_LIST_NGUONDON");
                if (nguondon == null)
                {
                    nguondon = new List<KNTC_NGUONDON>();
                }
                var don = kntcrpt.getReportDonKhieuTo("PKG_KNTC_BAOCAO.PRO_LIST_ALL_DON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon,itiepnhan);
                if (don == null)
                {
                    don = new List<DONKHIEUTO>();
                }
                List<decimal> list_id_chuyenvien = new List<decimal>();
                foreach (var l in don)
                {
                    if (!list_id_chuyenvien.Contains((decimal)l.IUSER_DUOCGIAOXULY)) { if (l.IUSER_DUOCGIAOXULY != 0) { list_id_chuyenvien.Add((decimal)l.IUSER_DUOCGIAOXULY); } }
                }


                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Tổng hợp số liệu");

                // Merge a row
                wb.Style.Font.FontSize = 13;
                wb.Style.Font.FontName = "Times New Roman";
                wb.Column(1).Width = 30;
                wb.PageSetup.FitToPages(1, 1);
                wb.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                // Dòng 1
                // Biến số cột 

                int iMerge_LinhVuc = linhvuc.Count;
                int iMerge_NguonDon = 0;
                foreach (var d in nguondon.Where(v => v.IPARENT == 0 && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(v => v.IVITRI))
                {
                    if (nguondon.Where(v => v.IPARENT == d.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).Count() > 0)
                    {
                        iMerge_NguonDon += 1 + nguondon.Where(v => v.IPARENT == d.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).Count() * 4;
                    }
                    else
                    {
                        iMerge_NguonDon += 8;
                    }
                }
                int tongsocot = 6 + iMerge_LinhVuc + iMerge_NguonDon;




                wb.Cell(1, 1).Value = "CÔNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb.Range(1, 1, 1, tongsocot).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(2, 1).Value = "Độc lập - Tự do - Hạnh phúc";
                wb.Range(2, 1, 2, tongsocot).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(11);
                wb.Cell(3, 1).Value = "";
                wb.Range(3, 1, 3, tongsocot).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                    .Font.Bold = true;
                wb.Cell(4, 1).Value = "TỔNG HỢP SỐ LIỆU ĐƠN (Từ " + Request["tungay"] + " đến ngày " + Request["denngay"] + " )";
                wb.Range(4, 1, 4, tongsocot).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
                wb.Cell(5, 1).Value = "";
                wb.Range(5, 1, 5, tongsocot).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);


                wb.Cell(6, 1).Value = "Chuyên viên";
                wb.Range(6, 1, 12, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);

                wb.Cell(6, 2).Value = "Tổng đơn nhận";
                wb.Range(6, 2, 12, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                int x = 3;

                wb.Cell(6, x).Value = "Lĩnh vực";
                wb.Range(6, x, 6, x + iMerge_LinhVuc - 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                x = x + iMerge_LinhVuc;
                foreach (var n in nguondon.Where(v => v.IPARENT == 0 && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(v => v.IVITRI))
                {
                    if (nguondon.Where(v => v.IPARENT == n.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).Count() <= 0)
                    {
                        wb.Cell(6, x).Value = Server.HtmlDecode(n.CTEN).ToUpper();
                        wb.Range(6, x, 6, x + 8 - 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        x = x + 8;
                    }
                    else
                    {

                        wb.Cell(6, x).Value = Server.HtmlDecode(n.CTEN).ToUpper();
                        wb.Range(6, x, 6, x + 1 + nguondon.Where(v => v.IPARENT == n.INGUONDON).Count() * 4 - 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        x = x + 1 + nguondon.Where(v => v.IPARENT == n.INGUONDON).Count() * 4;
                    }

                }

                wb.Cell(6, x).Value = "Tổng xử lý";
                wb.Range(6, x, 12, x).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                x = x + 1;
                wb.Cell(6, x).Value = "Tổng chuyền";
                wb.Range(6, x, 12, x).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                x = x + 1;
                wb.Cell(6, x).Value = "Tổng đang nghiên cứu";
                wb.Range(6, x, 12, x).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                x = x + 1;
                wb.Cell(6, x).Value = "Tổng lưu";
                wb.Range(6, x, 12, x).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                // Dòng 2
                int y = 3;
                foreach (var l in linhvuc)
                {
                    wb.Cell(7, y).Value = Server.HtmlDecode(l.CTEN);
                    wb.Range(7, y, 12, y).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                          .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                    y++;
                }
                foreach (var n in nguondon.Where(v => v.IPARENT == 0 && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(v => v.IVITRI))
                {
                    if (nguondon.Where(v => v.IPARENT == n.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).Count() <= 0)
                    {
                        wb.Cell(7, y).Value = "Tổng đơn nhận";
                        wb.Range(7, y, 12, y).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        y = y + 1;
                        wb.Cell(7, y).Value = "Đã xử lý";
                        wb.Range(7, y, 7, y + 6 - 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                            .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        y = y + 6;
                        wb.Cell(7, y).Value = "Chưa xử lý";
                        wb.Range(7, y, 12, y).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);

                    }
                    else
                    {
                        y = y + 1;
                        wb.Cell(7, y).Value = "Tổng";
                        wb.Range(7, y, 12, y).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        y = y + 1;
                        foreach (var d in nguondon.Where(v => v.IPARENT == n.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(v => v.IVITRI))
                        {

                            wb.Cell(7, y).Value = Server.HtmlDecode(d.CTEN);
                            wb.Range(7, y, 7, y + 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                            y = y + 4;
                        }
                    }

                }



                int z = 7;

                // dòng 3 
                foreach (var n in nguondon.Where(v => v.IPARENT == 0 && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(v => v.IVITRI))
                {
                    if (nguondon.Where(v => v.IPARENT == n.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).Count() <= 0)
                    {

                        wb.Cell(8, z).Value = "Tổng xử lý";
                        wb.Range(8, z, 12, z).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        z = z + 1;
                        wb.Cell(8, z).Value = "Đủ ĐK";
                        wb.Range(8, z, 8, z + 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        z = z + 4;
                        wb.Cell(8, z).Value = "Loại sơ bộ";
                        wb.Range(8, z, 12, z).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);


                    }
                    else
                    {
                        z = z + 3;
                        foreach (var d in nguondon.Where(v => v.IPARENT == n.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(v => v.IVITRI))
                        {

                            wb.Cell(8, z).Value = "Số đơn";
                            wb.Range(8, z, 12, z).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                            z = z + 1;
                            wb.Cell(8, z).Value = "Chuyển";
                            wb.Range(8, z, 12, z).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                            z = z + 1;
                            wb.Cell(8, z).Value = "Đang nghiên cứu";
                            wb.Range(8, z, 12, z).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                            z = z + 1;
                            wb.Cell(8, z).Value = "Không chuyển";
                            wb.Range(8, z, 12, z).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                            z = z + 1;
                        }
                        z = z + 1;
                    }

                }
                int e = 8;
                foreach (var n in nguondon.Where(v => v.IPARENT == 0 && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(v => v.IVITRI))
                {
                    if (nguondon.Where(v => v.IPARENT == n.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).Count() <= 0)
                    {
                        // Dòng 4 

                        wb.Cell(9, e).Value = "Số đơn";
                        wb.Range(9, e, 12, e).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        e = e + 1;
                        wb.Cell(9, e).Value = "Chuyển";
                        wb.Range(9, e, 12, e).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        e = e + 1;
                        wb.Cell(9, e).Value = "Không chuyển";
                        wb.Range(9, e, 12, e).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        e = e + 1;
                        wb.Cell(9, e).Value = "Đang nghiên cứu";
                        wb.Range(9, e, 12, e).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                          .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11);
                        e = e + 8;
                    }
                    else
                    {
                        e = e + nguondon.Where(v => v.IPARENT == n.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).Count() * 4 - 2;
                    }

                }

                int j = 13;
                foreach (var c in list_id_chuyenvien)
                {
                    USERS u = kntc.Get_User(Convert.ToInt32(c));

                    wb.Cell(j, 1).Value = Server.HtmlDecode(u.CTEN);
                    wb.Cell(j, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetFontSize(11)
                                                    .Alignment.SetWrapText(true);
                    wb.Cell(j, 2).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c).Count();
                    wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true);
                    int m = 3;
                    foreach (var l in linhvuc.OrderBy(v => v.IVITRI))
                    {

                        wb.Cell(j, m).Value = don.Where(a => a.ID_PARENT_LINHVUC == l.ILINHVUC && a.IUSER_DUOCGIAOXULY == c).Count();
                        wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                      .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                      .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                      .Alignment.SetWrapText(true);
                        m = m + 1;
                    }
                    foreach (var n in nguondon.Where(v => v.IPARENT == 0 && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(v => v.IVITRI))
                    {
                        if (nguondon.Where(v => v.IPARENT == n.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).Count() <= 0)
                        {

                            wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.ID_PARENT_NGUONDON == n.INGUONDON).Count();
                            wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                          .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                          .Alignment.SetWrapText(true);
                            m = m + 1;
                            wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.ID_PARENT_NGUONDON == n.INGUONDON && a.ITINHTRANGXULY == 6).Count();
                            wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                          .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                          .Alignment.SetWrapText(true);
                            m = m + 1;
                            wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.ID_PARENT_NGUONDON == n.INGUONDON && a.ITINHTRANGXULY == 6 && a.IDUDIEUKIEN == 1).Count();
                            wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                          .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                          .Alignment.SetWrapText(true);
                            m = m + 1;
                            wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.ID_PARENT_NGUONDON == n.INGUONDON && a.IDUDIEUKIEN == 1 && a.IDUDIEUKIEN_KETQUA == 1).Count();
                            wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                          .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                          .Alignment.SetWrapText(true);
                            m = m + 1;
                            wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.ID_PARENT_NGUONDON == n.INGUONDON && a.IDUDIEUKIEN == 1 && a.IDUDIEUKIEN_KETQUA == 0).Count();
                            wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                          .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                          .Alignment.SetWrapText(true);
                            m = m + 1;
                            wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.ID_PARENT_NGUONDON == n.INGUONDON && a.IDUDIEUKIEN == 1 && a.IDUDIEUKIEN_KETQUA == 2).Count();
                            wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                          .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                          .Alignment.SetWrapText(true);
                            m = m + 1;
                            wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.ID_PARENT_NGUONDON == n.INGUONDON && a.ITINHTRANGXULY == 2 && a.IDUDIEUKIEN != 1).Count();
                            wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                          .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                          .Alignment.SetWrapText(true);
                            m = m + 1;
                            wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.ID_PARENT_NGUONDON == n.INGUONDON && a.ITINHTRANGXULY == 2).Count();
                            wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                          .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                          .Alignment.SetWrapText(true);


                        }
                        else
                        {
                            m = m + 1;
                            wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.ID_PARENT_NGUONDON == n.INGUONDON).Count();
                            wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                          .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                          .Alignment.SetWrapText(true);
                            foreach (var d in nguondon.Where(v => v.IPARENT == n.INGUONDON && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(v => v.IVITRI))
                            {
                                m = m + 1;
                                wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.INGUONDON == d.INGUONDON).Count();
                                wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                              .Alignment.SetWrapText(true);
                                m = m + 1;
                                wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.INGUONDON == d.INGUONDON && a.IDUDIEUKIEN == 1 && a.IDUDIEUKIEN_KETQUA == 1).Count();
                                wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                              .Alignment.SetWrapText(true);
                                m = m + 1;
                                wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.INGUONDON == d.INGUONDON && a.IDUDIEUKIEN == 1 && a.IDUDIEUKIEN_KETQUA == 0).Count();
                                wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                              .Alignment.SetWrapText(true);
                                m = m + 1;
                                wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.INGUONDON == d.INGUONDON && a.IDUDIEUKIEN == 1 && a.IDUDIEUKIEN_KETQUA == 2).Count();
                                wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                              .Alignment.SetWrapText(true);

                            }
                        }
                    }
                    m = m + 1;
                    wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.ITINHTRANGXULY == 6).Count();
                    wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true);
                    m = m + 1;
                    wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.IDUDIEUKIEN == 1 && a.IDUDIEUKIEN_KETQUA == 1).Count();
                    wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true);
                    m = m + 1;
                    wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.IDUDIEUKIEN == 1 && a.IDUDIEUKIEN_KETQUA == 0).Count();
                    wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true);
                    m = m + 1;
                    wb.Cell(j, m).Value = don.Where(a => a.IUSER_DUOCGIAOXULY == c && a.IDUDIEUKIEN == 1 && a.IDUDIEUKIEN_KETQUA == 2).Count();
                    wb.Cell(j, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetFontSize(11)
                                                  .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                  .Alignment.SetWrapText(true);
                    j++;
                }
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=TongHopSoLieu_Exl" + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Xuất báo cáo tổng hợp số liệu khiếu nại tố cáo");
                return null;
            }
        }
        /*
         */
        public string Get_Option_Thang()
        {
            string str = "";
            for (int i = 1; i <= 12; i++)
            {
                str += "<option  value='" + i + "'>Tháng " + i + "</option>";
            }
            return str;
        }
        public ActionResult Baocaothang()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u = GetUserInfor();
            ViewData["opt-nam"] = Get_Option_Nam();
            ViewData["opt-thang"] = Get_Option_Thang();
            return View();
        }
        /*  Xem truoc bao cao
         */
        public ActionResult Ajax_XemBaoCaoThang(FormCollection fc)
        {
            UserInfor u = GetUserInfor();
            //string tungay = "", denngay = "";
            //if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            //{
            //    tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

            //}
            //if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            //{
            //    denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

            //}
            String tungay = "", denngay = "";
            if (fc["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));

            }
            if (fc["dDenNgay"] != null && Request["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));

            }
            ViewData["data"] = kn.Xemtruoc_baocaothang(tungay, denngay);
            return PartialView("../Ajax/Baocao/Kntc_Baocaothang");

        }
        /*  Xuat Excel bao cao thang
         */
        public ActionResult Baocaothang_Exl()
        {
            UserInfor user = GetUserInfor();
            int itiepnhan = (int)user.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_("47", user.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                //Mac dinh la gia tri Min va Max trong truong hop nguoi dung bo trong khong dien se tu dong tim kiem cao nhat co the
                string tungay = DateTime.MinValue.ToString(), denngay = DateTime.MaxValue.ToString();
                if (Request["tungay"] != null && Request["tungay"].ToString() != "")
                {
                    tungay = func.ConvertDateToSql(func.RemoveTagInput(Request["tungay"]));

                }
                if (Request["denngay"] != null && Request["denngay"].ToString() != "")
                {
                    denngay = func.ConvertDateToSql(func.RemoveTagInput(Request["denngay"]));

                }
                var linhvuc = kntcrpt.getReportBaoCaoThang("PKG_KNTC_BAOCAO.PRO_BAOCAO_THANG", tungay, denngay);
                if (linhvuc == null)
                {
                    linhvuc = new List<BAOCAOTHANG>();
                }
                DateTime temp = Convert.ToDateTime(tungay);
                string nam = temp.Year.ToString();
                string thang = temp.Month.ToString();
                XLWorkbook w_b = new XLWorkbook();
                var wb_chung = w_b.Worksheets.Add("Chung");
                var wb_dsdvtraloi = w_b.Worksheets.Add("DSDVTRALOI");
                var wb_dbqh = w_b.Worksheets.Add("DBQH");
                var wb_hdnd = w_b.Worksheets.Add("HDND");
                wb_chung.Style.Font.FontSize = 12;
                wb_chung.Style.Font.FontName = "Times New Roman";
                wb_chung.PageSetup.FitToPages(1, 1);
                wb_chung.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb_dsdvtraloi.Style.Font.FontSize = 12;
                wb_dsdvtraloi.Style.Font.FontName = "Times New Roman";
                wb_dsdvtraloi.PageSetup.FitToPages(1, 1);
                wb_dsdvtraloi.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb_dbqh.Style.Font.FontSize = 12;
                wb_dbqh.Style.Font.FontName = "Times New Roman";
                wb_dbqh.PageSetup.FitToPages(1, 1);
                wb_dbqh.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                wb_hdnd.Style.Font.FontSize = 12;
                wb_hdnd.Style.Font.FontName = "Times New Roman";
                wb_hdnd.PageSetup.FitToPages(1, 1);
                wb_hdnd.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                //Phan header cho sheet chung
                wb_chung.Row(1).Height = 40;
                wb_chung.Column(2).Width = 14;
                wb_chung.Column(3).Width = 19;
                wb_chung.Column(4).Width = 11;
                wb_chung.Column(5).Width = 23;
                wb_chung.Column(6).Width = 28;
                wb_chung.Column(7).Width = 25;
                wb_chung.Column(8).Width = 24;
                wb_chung.Column(9).Width = 24;
                wb_dsdvtraloi.Column(2).Width = 14;
                wb_dsdvtraloi.Column(3).Width = 19;
                wb_dsdvtraloi.Column(4).Width = 11;
                wb_dsdvtraloi.Column(5).Width = 23;
                wb_dsdvtraloi.Column(6).Width = 28;
                wb_dsdvtraloi.Column(7).Width = 25;
                wb_dsdvtraloi.Column(8).Width = 24;
                wb_dsdvtraloi.Column(9).Width = 24;
                wb_dbqh.Column(2).Width = 14;
                wb_dbqh.Column(3).Width = 19;
                wb_dbqh.Column(4).Width = 11;
                wb_dbqh.Column(5).Width = 23;
                wb_dbqh.Column(6).Width = 28;
                wb_dbqh.Column(7).Width = 25;
                wb_dbqh.Column(8).Width = 24;
                wb_dbqh.Column(9).Width = 24;
                wb_hdnd.Column(2).Width = 14;
                wb_hdnd.Column(3).Width = 19;
                wb_hdnd.Column(4).Width = 11;
                wb_hdnd.Column(5).Width = 23;
                wb_hdnd.Column(6).Width = 28;
                wb_hdnd.Column(7).Width = 25;
                wb_hdnd.Column(8).Width = 24;
                wb_hdnd.Column(9).Width = 24;
                wb_chung.Range(1, 2, 1, 3).Merge().Style.Font.SetBold(true)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Alignment.SetWrapText(true);
                wb_chung.Cell(1,2).Value = "VĂN PHÒNG ĐOÀN ĐBQH VÀ HĐND TỈNH THANH HÓA";
                wb_chung.Range(1, 5, 1, 9).Merge().Style.Font.SetBold(true)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                wb_chung.Cell(1, 5).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                wb_chung.Range(2, 2, 2, 3).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    .Font.SetUnderline();
                wb_chung.Cell(2, 2).Value = "Số:...../BC-VP";
                wb_chung.Range(2, 5, 2, 9).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                   .Font.SetUnderline()
                   .Font.SetBold(true);
                wb_chung.Cell(2, 5).Value = "Độc lập - Tự do - Hạnh phúc";
                wb_chung.Range(3, 7, 3, 8).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                  .Font.SetUnderline()
                  .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                  .Font.SetItalic(true);
                wb_chung.Cell(3, 7).Value = "Thanh Hóa, ngày...tháng...năm 2022";
                wb_chung.Range(5, 2, 5, 8).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                  .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                  .Font.SetBold(true);
                wb_chung.Cell(5, 2).Value = "BÁO CÁO";
                wb_chung.Row(6).Height = 40;
                wb_chung.Range(6, 2, 6, 8).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                 .Alignment.SetWrapText(true)
                 .Font.SetBold(true);
                wb_chung.Cell(6, 2).Value = "Kết quả tiếp công dân, tiếp nhận, xử lý đơn khiếu nại, tố cáo của công dân tháng " + thang + " năm " + nam + " của Đoàn đại biểu Quốc hội và Thường trực HĐND Tỉnh Thanh Hóa";
                wb_chung.Range(8, 2, 8, 5).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                 .Font.SetBold(true);
                wb_chung.Cell(8, 2).Value = "1. Tình hình tiếp công dân tại trụ sở tiếp công dân tỉnh";
                wb_chung.Range(9, 2, 9, 4).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                 .Font.SetBold(true);
                wb_chung.Cell(9, 2).Value = "1.1. Đoàn đại biểu Quốc hội tỉnh";
                wb_chung.Range(10, 2, 10, 4).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                 .Font.SetBold(true); 
                wb_chung.Cell(10, 2).Value = "1.2.Thường trực Hội đồng nhân dân tỉnh";
                wb_chung.Range(11, 2, 11, 7).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                 .Font.SetBold(true);
                wb_chung.Cell(11, 2).Value = "2. Về kết quả tiếp nhận, phân loại, xử lý đơn, theo dõi việc giải quyết đơn khiếu nại, tố cáo, phản ánh, kiến nghị của công dân";
                wb_chung.Range(12, 2, 12, 4).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                 .Font.SetBold(true);
                wb_chung.Cell(12, 2).Value = "2.1. Đoàn đại biểu Quốc hội tỉnh";
                // Het header

                //Tao cac bang
                //Bang 2.1
                List<BAOCAOTHANG> List1 = linhvuc.Where(x => x.IDOITUONGGUI == 0).ToList();
                wb_chung.Range(14, 2, 15, 2).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                 .Font.SetBold(true)
                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(14,2).Value = "STT";
                wb_chung.Range(14, 3, 14, 5).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                 .Font.SetBold(true)
                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(14, 3).Value = "Số lượng đơn tiếp nhận";
                wb_chung.Range(14, 6, 15, 6).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                 .Font.SetBold(true)
                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(14, 6).Value = "Số đơn đã chuyển";
                wb_chung.Range(14, 7, 15, 7).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                 .Font.SetBold(true)
                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(14, 7).Value = "Số đơn lưu";
                wb_chung.Range(14, 8, 15, 9).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                 .Font.SetBold(true)
                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(14, 8).Value = "Số đơn được giải quyết";
                wb_chung.Row(15).Height = 40;
                wb_chung.Cell(15, 3).Value = "Qua bưu điện";
                wb_chung.Cell(15, 3).Style.Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(15, 4).Style.Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(15, 4).Value = "Qua trụ sở tiếp dân";
                wb_chung.Cell(15, 5).Value = "Khác";
                wb_chung.Cell(15, 5).Style.Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(16, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(16, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(16, 3).Value = List1.Where(x=>x.INGUONDON == 9).Count();
                wb_chung.Cell(16, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(16, 4).Value = List1.Where(x => x.INGUONDON == 25).Count();
                wb_chung.Cell(16, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(16, 5).Value = List1.Where(x => x.INGUONDON == 7).Count();
                wb_chung.Cell(16, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(16, 6).Value = List1.Where(x => x.ITINHTRANGXULY == 3).Count();
                wb_chung.Cell(16, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(16, 7).Value = List1.Where(x => x.ITINHTRANGXULY == 5).Count();
                wb_chung.Range(16,8,16,9).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(16, 8).Value = List1.Where(x => x.ITINHTRANGXULY == 6).Count();

                wb_chung.Cell(18, 2).Value = "2.2. Thường trực Hội đồng nhân dân tỉnh";
                // Het header

                //Tao cac bang
                //Bang 2.2
                List<BAOCAOTHANG> List2 = linhvuc.Where(x => x.IDOITUONGGUI == 1).ToList();
                wb_chung.Range(20, 2, 21, 2).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                 .Font.SetBold(true)
                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(20, 2).Value = "STT";
                wb_chung.Range(20, 3, 20, 5).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                 .Font.SetBold(true)
                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(20, 3).Value = "Số lượng đơn tiếp nhận";
                wb_chung.Range(20, 6, 21, 6).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                 .Font.SetBold(true)
                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(20, 6).Value = "Số đơn đã chuyển";
                wb_chung.Range(20, 7, 21, 7).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                 .Font.SetBold(true)
                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(20, 7).Value = "Số đơn đã hướng dẫn";
                wb_chung.Range(20, 8, 21, 8).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                 .Font.SetBold(true)
                 .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(20, 8).Value = "Số đơn lưu";
                wb_chung.Range(20, 9, 21, 9).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Bottom)
                .Font.SetBold(true)
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(20, 9).Value = "Số đơn đã giải quyết";
                wb_chung.Row(21).Height = 40;
                wb_chung.Cell(21, 3).Value = "Qua bưu điện";
                wb_chung.Cell(21, 3).Style.Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(21, 4).Style.Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(21, 4).Value = "Qua trụ sở tiếp dân";
                wb_chung.Cell(21, 5).Value = "Khác";
                wb_chung.Cell(21, 5).Style.Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_chung.Cell(22, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(22, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(22, 3).Value = List2.Where(x => x.INGUONDON == 9).Count();
                wb_chung.Cell(22, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(22, 4).Value = List2.Where(x => x.INGUONDON == 25).Count();
                wb_chung.Cell(22, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(22, 5).Value = List2.Where(x => x.INGUONDON == 7).Count();
                wb_chung.Cell(22, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(22, 6).Value = List2.Where(x => x.ITINHTRANGXULY == 3).Count();
                wb_chung.Cell(22, 8).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(22, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(22, 8).Value = List2.Where(x => x.ITINHTRANGXULY == 5).Count();
                wb_chung.Cell(22, 9).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                     .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                wb_chung.Cell(22, 9).Value = List2.Where(x => x.ITINHTRANGXULY == 6).Count();
                wb_chung.Range(24, 2, 24, 4).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                 .Font.SetBold(true);
                wb_chung.Cell(24, 2).Value = "3. Các vụ việc nổi cộm, phức tạp";
                //Bang 3

                wb_dsdvtraloi.Row(1).Height = 40;
                wb_dsdvtraloi.Range(1, 2, 1, 3).Merge().Style.Font.SetBold(true)
                   .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                   .Alignment.SetWrapText(true);
                wb_dsdvtraloi.Cell(1, 2).Value = "VĂN PHÒNG ĐOÀN ĐBQH VÀ HĐND TỈNH THANH HÓA";
                wb_dsdvtraloi.Range(1, 5, 1, 9).Merge().Style.Font.SetBold(true)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                wb_dsdvtraloi.Cell(1, 5).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";

                wb_dsdvtraloi.Range(2, 5, 2, 9).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                   .Font.SetUnderline()
                   .Font.SetBold(true);
                wb_dsdvtraloi.Cell(2, 5).Value = "Độc lập - Tự do - Hạnh phúc";
                wb_dsdvtraloi.Range(4, 2, 4, 8).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                  .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                  .Font.SetBold(true);
                wb_dsdvtraloi.Cell(5, 2).Value = "DANH SÁCH CÁC ĐƠN VỊ ĐÃ TRẢ LỜI ĐƠN CHUYỂN CỦA ĐOÀN ĐBQH TỈNH VÀ THƯỜNG TRỰC HĐND TỈNH";
                wb_dsdvtraloi.Range(5, 2, 5, 8).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                  .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                  .Font.SetItalic(true);
                wb_dsdvtraloi.Cell(5, 2).Value = "(Kèm theo Báo cáo số:....../BC-VP ngày....tháng " + thang + " năm " + nam + " của Văn phòng Đoàn ĐBQH và HĐND tỉnh Thanh Hóa)";
                wb_dsdvtraloi.Row(7).Height = 40;
                wb_dsdvtraloi.Cell(7, 2).Value = "TT";
                wb_dsdvtraloi.Cell(7, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dsdvtraloi.Cell(7, 3).Value = "Tên đơn vị";
                wb_dsdvtraloi.Cell(7, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dsdvtraloi.Cell(7, 4).Value = "Số ký hiệu văn bản";
                wb_dsdvtraloi.Cell(7, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dsdvtraloi.Cell(7, 5).Value = "Ngày, tháng, năm văn bản";
                wb_dsdvtraloi.Cell(7, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    .Alignment.SetWrapText(true);
                wb_dsdvtraloi.Cell(7, 6).Value = "Nội dung trả lời đơn";
                wb_dsdvtraloi.Cell(7, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dsdvtraloi.Cell(7, 7).Value = "Số CV, ngày tháng năm Đoàn ĐBQH chuyển đơn";
                wb_dsdvtraloi.Cell(7, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dsdvtraloi.Cell(7, 8).Value = "Ghi chú";
                wb_dsdvtraloi.Cell(7, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dsdvtraloi.Row(4).Height = 40;
                wb_dsdvtraloi.Range(4, 2, 4, 8).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                 .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                 .Font.SetBold(true);
                wb_dsdvtraloi.Cell(4, 2).Value = "DANH SÁCH CÁC ĐƠN VỊ ĐÃ TRẢ LỜI ĐƠN CHUYỂN CỦA ĐOÀN ĐBQH TỈNH VÀ THƯỜNG TRỰC HĐND TỈNH";
                List<BAOCAOTHANG> list3 = linhvuc.Where(x => x.ITINHTRANGXULY == 6).ToList();
                List<BAOCAOTHANG> list3_1 = list3.Where(x => x.ICOQUANBANHANH != 4).ToList();
                int currentRow = 8;
                int rowIncremental = 0;
                foreach (var item in list3_1)
                {
                    wb_dsdvtraloi.Row(currentRow + rowIncremental).Height = 70;
                    var tempItem = list3.FirstOrDefault(x => x.ICOQUANBANHANH == 4 && x.IDON_VANBAN == item.IDON_VANBAN);
                    if (tempItem == null)
                        continue;
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                        .Alignment.SetWrapText(true);
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 2).Value = rowIncremental + 1;
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 3).Value = item.COQUANBANHANH;
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 4).Value = item.CSOVANBAN;
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 5).Value = String.Format("{0:dd/MM/yyyy}", item.DNGAYBANHANH);
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Alignment.SetWrapText(true)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 6).Value = item.NOIDUNGVANBAN;
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Alignment.SetWrapText(true)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    String ngaybanhanh = "";
                    if (tempItem.DNGAYBANHANH != null)
                        ngaybanhanh = String.Format("{0:dd/MM/yyyy}", tempItem.DNGAYBANHANH);
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 7).Value = "Số " + tempItem.CSOVANBAN + " ngày " + ngaybanhanh + " của " + tempItem.COQUANBANHANH;
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 8).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Alignment.SetWrapText(true)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_dsdvtraloi.Cell(currentRow + rowIncremental, 8).Value = item.GHICHUVANBAN;
                    rowIncremental++;
                }
                currentRow = currentRow + rowIncremental + 1;
                rowIncremental = 0;

                //Bang 4
                currentRow = 1;
                wb_dbqh.Range(currentRow, 2, currentRow, 8).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                  .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                  .Font.SetBold(true);
                wb_dbqh.Cell(currentRow, 2).Value = "DANH SÁCH CHUYỂN ĐƠN THÁNG " + thang + " NĂM " + nam + " CỦA ĐOÀN ĐBQH TỈNH";
                currentRow = currentRow + 1;
                List<BAOCAOTHANG> list4 = linhvuc.Where(x => x.ITINHTRANGXULY == 3 && x.IDOITUONGGUI == 0 && x.ICOQUANBANHANH == 4).ToList();
                //Header bang 4
                wb_dbqh.Row(currentRow).Height = 40;
                wb_dbqh.Cell(currentRow, 2).Value = "TT";
                wb_dbqh.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dbqh.Cell(currentRow, 3).Value = "Ngày ban hành";
                wb_dbqh.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dbqh.Cell(currentRow, 4).Value = "Số ký hiệu văn bản";
                wb_dbqh.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dbqh.Cell(currentRow, 5).Value = "Công dân gửi đơn";
                wb_dbqh.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dbqh.Cell(currentRow, 6).Value = "Trích yếu";
                wb_dbqh.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dbqh.Cell(currentRow, 7).Value = "Nơi nhận VB chuyển của Đoàn ĐBQH tỉnh";
                wb_dbqh.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_dbqh.Cell(currentRow, 8).Value = "Ghi chú";
                wb_dbqh.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                currentRow++;
                foreach (var item in list4)
                {
                    wb_dbqh.Row(currentRow + rowIncremental).Height = 70;
                    wb_dbqh.Cell(currentRow + rowIncremental, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_dbqh.Cell(currentRow + rowIncremental, 2).Value = rowIncremental + 1;
                    wb_dbqh.Cell(currentRow + rowIncremental, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    String ngaybanhanh = "";
                    if (item.DNGAYBANHANH != null)
                        ngaybanhanh = String.Format("{0:dd/MM/yyyy}", item.DNGAYBANHANH);
                    wb_dbqh.Cell(currentRow + rowIncremental, 3).Value = ngaybanhanh;
                    wb_dbqh.Cell(currentRow + rowIncremental, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_dbqh.Cell(currentRow + rowIncremental, 4).Value = item.CSOVANBAN;
                    wb_dbqh.Cell(currentRow + rowIncremental, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                        .Alignment.SetWrapText(true);
                    wb_dbqh.Cell(currentRow + rowIncremental, 5).Value = item.CNGUOIGUI_TEN + ", " + item.DIACHI + ".";
                    wb_dbqh.Cell(currentRow + rowIncremental, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Alignment.SetWrapText(true)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_dbqh.Cell(currentRow + rowIncremental, 6).Value = item.NOIDUNGDON;
                    wb_dbqh.Cell(currentRow + rowIncremental, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Alignment.SetWrapText(true)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    
                    
                    wb_dbqh.Cell(currentRow + rowIncremental, 7).Value = item.CDONVIXULY;
                    wb_dbqh.Cell(currentRow + rowIncremental, 8).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Alignment.SetWrapText(true)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_dbqh.Cell(currentRow + rowIncremental, 8).Value = item.GHICHUDON;
                    rowIncremental++;
                }
                currentRow = currentRow + rowIncremental + 1;
                rowIncremental = 0;

                //Bang 5
                currentRow = 1;
                wb_hdnd.Range(currentRow, 2, currentRow, 8).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                  .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                  .Font.SetBold(true);
                wb_hdnd.Cell(currentRow, 2).Value = "DANH SÁCH CHUYỂN ĐƠN THÁNG " + thang + " NĂM " + nam + " THƯỜNG TRỰC HĐND TỈNH";
                currentRow = currentRow + 1;
                List<BAOCAOTHANG> list5 = linhvuc.Where(x => x.ITINHTRANGXULY == 3 && x.IDOITUONGGUI == 1 && x.ICOQUANBANHANH == 4).ToList();
                //Header bang 5
                wb_hdnd.Row(currentRow).Height = 40;
                wb_hdnd.Cell(currentRow, 2).Value = "TT";
                wb_hdnd.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_hdnd.Cell(currentRow, 3).Value = "Ngày ban hành";
                wb_hdnd.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_hdnd.Cell(currentRow, 4).Value = "Số ký hiệu văn bản";
                wb_hdnd.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_hdnd.Cell(currentRow, 5).Value = "Công dân gửi đơn";
                wb_hdnd.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_hdnd.Cell(currentRow, 6).Value = "Trích yếu";
                wb_hdnd.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_hdnd.Cell(currentRow, 7).Value = "Nơi nhận VB chuyển của Đoàn ĐBQH tỉnh";
                wb_hdnd.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Alignment.SetWrapText(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb_hdnd.Cell(currentRow, 8).Value = "Ghi chú";
                wb_hdnd.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.SetBold(true)
                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                currentRow++;
                foreach (var item in list5)
                {
                    wb_hdnd.Row(currentRow + rowIncremental).Height = 70;
                    wb_hdnd.Cell(currentRow + rowIncremental, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_hdnd.Cell(currentRow + rowIncremental, 2).Value = rowIncremental + 1;
                    wb_hdnd.Cell(currentRow + rowIncremental, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    String ngaybanhanh = "";
                    if (item.DNGAYBANHANH != null)
                        ngaybanhanh = String.Format("{0:dd/MM/yyyy}", item.DNGAYBANHANH);
                    wb_hdnd.Cell(currentRow + rowIncremental, 3).Value = ngaybanhanh;
                    wb_hdnd.Cell(currentRow + rowIncremental, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_hdnd.Cell(currentRow + rowIncremental, 4).Value = item.CSOVANBAN;
                    wb_hdnd.Cell(currentRow + rowIncremental, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_hdnd.Cell(currentRow + rowIncremental, 5).Value = item.CNGUOIGUI_TEN + ", " + item.DIACHI + ".";
                    wb_hdnd.Cell(currentRow + rowIncremental, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Alignment.SetWrapText(true)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    wb_hdnd.Cell(currentRow + rowIncremental, 6).Value = item.NOIDUNGDON;
                    wb_hdnd.Cell(currentRow + rowIncremental, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Alignment.SetWrapText(true)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                    wb_hdnd.Cell(currentRow + rowIncremental, 7).Value = item.CDONVIXULY;
                    wb_hdnd.Cell(currentRow + rowIncremental, 8).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                        ;
                    wb_hdnd.Cell(currentRow + rowIncremental, 8).Value = item.GHICHUDON;
                    rowIncremental++;
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Báo cáo đơn thư hàng tháng " + DateTime.Now.Date + ".xlsx");
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
                log.Log_Error(ex, "Báo cáo đơn thư hàng tháng");
                return null;
            }
        }
    }
}
