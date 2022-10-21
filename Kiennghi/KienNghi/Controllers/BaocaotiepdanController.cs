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
using Utilities.Enums;
using Entities.Objects;

using ClosedXML.Excel;
namespace KienNghi.Controllers
{
    public class BaocaotiepdanController : BaseController
    {
        //
        // GET: /Baocaotiepdan/
        Tiepdan tiepdan = new Tiepdan();
        Funtions func = new Funtions();
        Tiepdan td = new Tiepdan();
        Thietlap tl = new Thietlap();
        TiepdanBusineess _tiepdan = new TiepdanBusineess();
        TiepdanReport tiepdanreport = new TiepdanReport();
        ThietlapBusineess _thietlap = new ThietlapBusineess();
        Khieunai kn = new Khieunai();
        Log log = new Log();
        BaseBusineess ba_se = new BaseBusineess();
        public string Get_Option_DonViTiepNhan(int iDonVi = 0)
        {

            List<QUOCHOI_COQUAN> coquan;
            UserInfor u_info = GetUserInfor();
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            donvi.Add("IPARENT", ID_Coquan_doandaibieu);
            coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
            string opt_bandannguyen = "";
            if (u_info.tk_action.is_lanhdao)
            {
                opt_bandannguyen = "<option value='" + u_info.user_login.IDONVI + "'>Chọn đơn vị tiếp nhận</option>";
                if (u_info.user_login.IDONVI == iDonVi)
                {
                    opt_bandannguyen += "<option selected value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
                }
                else
                {
                    opt_bandannguyen += "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
                }
            }
            return opt_bandannguyen + tiepdan.OptionCoQuanXuLy(coquan, ID_Coquan_doandaibieu, 0, iDonVi, 0);
        }
        public ActionResult Phuluc()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u_info = GetUserInfor();
            if (!ba_se.ActionMulty_Redirect_("48", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            int iUser = u_info.tk_action.iUser;
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            List<KNTC_LOAIDON> loaidon = _thietlap.Get_Loaidon();
            ViewData["opt-linhvuc"] = tl.Option_LinhVuc(linhvuc);
            ViewData["opt-loaidon"] = tl.Option_LoaiDon(loaidon, 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
            //  ViewData["opt-coquantiepdan"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, 4);


            int iDonViTiepNhan = (int)u_info.user_login.IDONVI;

            Dictionary<string, object> donvi = new Dictionary<string, object>();
            if (u_info.tk_action.is_lanhdao)
            {
                donvi.Add("IPARENT", ID_Coquan_doandaibieu);
                coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
                ViewData["opt-coquantiepdan"] = "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>" +
                                           tiepdan.OptionCoQuan_BaoCao(coquan, 0, 0, 0, 0);
            }
            else
            {
                donvi.Add("ICOQUAN", iDonViTiepNhan);
                coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
                ViewData["opt-coquantiepdan"] = "<option value='" + iDonViTiepNhan + "'>" + u_info.tk_action.tendonvi + "</option>";
            }
            // ViewData["opt-coquantiepdan"] = Get_Option_DonViTiepNhan(iDonViTiepNhan);


            return View();
        }
        public ActionResult Solieu()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u_info = GetUserInfor();
            if (!ba_se.ActionMulty_Redirect_("48", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            int iUser = u_info.tk_action.iUser;
            ViewData["Baocaotieude"] = td.Tieudebaocaosolieutinh();
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            List<KNTC_LOAIDON> loaidon = _thietlap.Get_Loaidon();
            List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
            List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
            ViewData["opt-linhvuc"] = tl.Option_LinhVuc(linhvuc);
            ViewData["opt-loaidon"] = tl.Option_LoaiDon(loaidon, 0);
            ViewData["opt_nguondon"] = tl.Option_NoiDungDon(noidungdon, 0);
            //  ViewData["opt-coquantiepdan"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, 4);

          
            //  ViewData["opt-coquantiepdan"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, 4);


            int iDonViTiepNhan = (int)u_info.user_login.IDONVI;

            Dictionary<string, object> donvi = new Dictionary<string, object>();
            if (u_info.tk_action.is_lanhdao)
            {
                donvi.Add("IPARENT", ID_Coquan_doandaibieu);
                coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
                ViewData["opt-coquantiepdan"] = "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>" +
                                           tiepdan.OptionCoQuan_BaoCao(coquan, 0, 0, 0, 0);
            }
            else
            {
                donvi.Add("ICOQUAN", iDonViTiepNhan);
                coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
                ViewData["opt-coquantiepdan"] = "<option value='" + iDonViTiepNhan + "'>" + u_info.tk_action.tendonvi + "</option>";
            }
            return View();
        }

        public ActionResult Ajax_Tiepcongdan_Phuluc(FormCollection fc)
        {
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(Server.HtmlEncode(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(Server.HtmlEncode(fc["dDenNgay"]));

            }

            int iLinhvuc = Convert.ToInt32(fc["iLinhVuc"]);
            int iLoaidon = Convert.ToInt32(fc["iLoaidon"]);
            int iDonvi = Convert.ToInt32(fc["iDonvi"]);
            List<TD_VUVIEC> vuviec = _tiepdan.Get_TDVuviec();
            List<KNTC_LOAIDON> loaidon = _thietlap.Get_Loaidon();
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["tieude"] = td.Tieudebaocaophuluc();
            ViewData["data"] = td.Thongkephuluc2(tungay, denngay, vuviec, iLinhvuc, iLoaidon, iDonvi, loaidon, linhvuc);
            return PartialView("../Ajax/Baocao/TD_Phuluc");
        }
        public ActionResult Ajax_Tiepcongdan_solieu(FormCollection fc)
        {
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(Server.HtmlEncode(fc["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(Server.HtmlEncode(fc["dDenNgay"]));

            }
            int iLinhvuc = Convert.ToInt32(fc["iLinhVuc"]);
            int iLoaidon = Convert.ToInt32(fc["iLoaidon"]);
            int inoidung = Convert.ToInt32(fc["iNoiDung"]);
            int iDonvi = Convert.ToInt32(fc["iDonvi"]);
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            ViewData["Baocaotieude"] = td.Tieudebaocaosolieutinh();
            List<TD_VUVIEC> vuviec = _tiepdan.Get_TDVuviec();
            List<KNTC_LOAIDON> loaidon = _thietlap.Get_Loaidon();
            List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            ViewData["data"] = td.Thongkesolieutinh2(tungay, denngay, iLoaidon, iLinhvuc, inoidung, iDonvi, vuviec, loaidon, noidungdon, linhvuc);
            return PartialView("../Ajax/Baocao/TD_Solieutinh");
        }
        public ActionResult Ajax_LoadLinhVucByLoaiDon(int iLoaiDon)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string str = "<select name='iLinhVuc' id='iLinhVuc' class='input-medium chosen-select' onchange=\"LoadLinhVuc()\" style='width:100%'>" +
                                            "<option value='0'>Chọn tất cả</option>" +
                                            "" + tiepdan.Option_LinhVuc_LoaiDon(0, iLoaiDon) + "" + "</select>";

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
        public string tachnam(string date)
        {
            string str = "";
            string ngay = Convert.ToString(date);
            string datetime;
            string[] a = ngay.Split(' ');
            datetime = a[0];
            string[] date_split = datetime.Split('-');
            str = date_split[2].Trim();

            return str;
        }
        public ActionResult Download_Baocaotiepcongdanphuluc()
        {
            if (!CheckAuthToken()) { return null; }
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && Request["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(Server.HtmlEncode(Request["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && Request["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(Server.HtmlEncode(Request["dDenNgay"]));

            }
            int iLinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int iLoaidon = Convert.ToInt32(Request["iLoaidon"]);
            int iDonvi = Convert.ToInt32(Request["iDonvi"]);
            string str1 = "";
            var thongtinloaidon = _thietlap.Get_Loaidon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            int demhangtiepnhan = 0;

            foreach (var t in thongtinloaidon)
            {
                str1 += "<th rowspan='2' style='text-align:center'>" + t.CTEN + "</th>";
            }
            string str2 = "";
            var thongtinlinhvuc = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();

            foreach (var x in thongtinlinhvuc)
            {
                str2 += "<th rowspan='2' style='text-align:center'>" + x.CTEN + "</th>";
            }



            demhangtiepnhan = 9 + thongtinloaidon.Count() + thongtinlinhvuc.Count();
            var list = tiepdanreport.getReportBaoBaoThongKeTiepDanPhuLuc("PKG_TCD_BAOCAO.PRO_BAOCAO_TK_PHULUC", tungay, denngay, iLoaidon, iLinhvuc, iDonvi);
            XLWorkbook w_b = new XLWorkbook();
            var wb = w_b.Worksheets.Add("Phụ lục");
            // Merge a row
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 13;
            wb.Column(1).Width = 13;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Cell(1, 1).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            wb.Range(1, 1, 1, demhangtiepnhan).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(2, 1).Value = "Độc lập - Tự do - Hạnh phúc";
            wb.Range(2, 1, 2, demhangtiepnhan).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(3, 1).Value = "BÁO CÁO PHỤ LỤC TIẾP CÔNG DÂN ";
            wb.Range(3, 1, 3, demhangtiepnhan).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(4, 1).Value = "Từ ngày " + func.ConvertDateVN(tungay) + " đến ngày " + func.ConvertDateVN(denngay) + "";
            wb.Range(4, 1, 4, demhangtiepnhan).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            wb.Style.Font.FontSize = 11;
            wb.Cell(5, 1).Value = "Thời gian";
            wb.Range(5, 1, 11, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Style.Font.FontName = "Times New Roman";
            wb.Cell(5, 1).Value = "Thời gian";
            wb.Range(5, 1, 11, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            wb.Cell(5, 2).Value = "TÌNH HÌNH TIẾP CÔNG DÂN";
            wb.Range(5, 2, 5, (demhangtiepnhan - 3)).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            wb.Cell(5, (demhangtiepnhan - 2)).Value = "KẾT QUẢ TIẾP CÔNG DÂN";
            wb.Range(5, (demhangtiepnhan - 2), 5, demhangtiepnhan).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            wb.Cell(6, 2).Value = "Tổng số lượt tiếp";
            wb.Range(6, 2, 6, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 5).Value = "Phân loại tiếp công dân";
            wb.Range(6, 5, 6, (demhangtiepnhan - 3)).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, (demhangtiepnhan - 2)).Value = "Hướng dẫn bằng văn bản";
            wb.Range(6, (demhangtiepnhan - 2), 11, (demhangtiepnhan - 2)).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, (demhangtiepnhan - 1)).Value = "Hướng dẫn giải thích trực tiếp";
            wb.Range(6, (demhangtiepnhan - 1), 11, (demhangtiepnhan - 1)).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, (demhangtiepnhan)).Value = "Chuyển đơn đến cơ quan có thẩm quyền ";
            wb.Range(6, (demhangtiepnhan), 11, (demhangtiepnhan)).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            //
            wb.Cell(7, 2).Value = "Tổng số";
            wb.Range(7, 2, 11, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            wb.Cell(7, 3).Value = "Tiếp công dân theo lĩnh vực phụ trách";
            wb.Range(7, 3, 11, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                   .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 4).Value = "Tiếp công dân của cá nhân đoàn ĐBQH";
            wb.Range(7, 4, 11, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 5).Value = "Số vụ việc";
            wb.Range(7, 5, 11, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 6).Value = "Theo loại đơn";
            wb.Range(7, 6, 7, (6 + thongtinloaidon.Count() - 1)).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, (6 + thongtinloaidon.Count())).Value = "Theo lĩnh vực";
            wb.Range(7, 6 + thongtinloaidon.Count(), 7, demhangtiepnhan - 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, demhangtiepnhan - 3).Value = "Đoàn đông người";
            wb.Range(7, demhangtiepnhan - 3, 11, demhangtiepnhan - 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            //

            int tongdon = 5;
            foreach (var t in thongtinloaidon)
            {
                tongdon++;

                wb.Cell(8, tongdon).Value = "" + Server.HtmlDecode(t.CTEN) + "";
                wb.Range(8, tongdon, 11, tongdon).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                 .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            }
            foreach (var t in thongtinlinhvuc)
            {
                tongdon++;
                wb.Cell(8, tongdon).Value = "" + Server.HtmlDecode(t.CTEN) + "";
                wb.Range(8, tongdon, 11, tongdon).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                 .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            }
            List<TD_VUVIEC> tiepcongdan = _tiepdan.Get_TDVuviec();
            string namtungay = tachnam(tungay);
            string namdenngay = tachnam(denngay);
            int hieucacnam = Convert.ToInt32(namdenngay) - Convert.ToInt32(namtungay);
            int j = 12;
            decimal tongso = 0; decimal tcdlinhvuc = 0; decimal tcddbqh = 0; decimal sovuviec = 0; decimal khieunai = 0; decimal tocao = 0; decimal kiennghipa = 0;
            decimal hanhchinh = 0; decimal tuphap = 0; decimal doandongnguoi = 0; decimal huongdanbangvanban = 0; decimal huongdangiaithich = 0; decimal chuyencoquan = 0;
            foreach (var t in list)
            {
                if (hieucacnam == 0)
                {
                    if (list.Count() > 0)
                    {

                        foreach (var d in list)
                        {

                            int tongdonbaocao = 5;
                            var thongtinloaidonbaocao = _thietlap.Get_Loaidon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                            if (iLoaidon == 0)
                            {
                                foreach (var k in thongtinloaidonbaocao)
                                {
                                    tongdonbaocao++;

                                    var thongtintiepcongdanloaidon = tiepcongdan.Where(x => x.ILOAIDON == k.ILOAIDON && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                    wb.Cell(j, tongdonbaocao).Value = thongtintiepcongdanloaidon.Count();
                                    wb.Range(j, tongdonbaocao, j, tongdonbaocao).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                }
                            }
                            else
                            {
                                foreach (var k in thongtinloaidonbaocao)
                                {
                                    tongdonbaocao++;
                                    if (iLoaidon == k.ILOAIDON)
                                    {
                                        var thongtintiepcongdanloaidon = tiepcongdan.Where(x => x.ILOAIDON == iLoaidon && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                        wb.Cell(j, tongdonbaocao).Value = thongtintiepcongdanloaidon.Count();
                                        wb.Range(j, tongdonbaocao, j, tongdonbaocao).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                    }
                                    else
                                    {
                                        wb.Cell(j, tongdonbaocao).Value = 0;
                                    }

                                }
                            }
                            var thongtinlinhvucbaocao = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
                            foreach (var k in thongtinlinhvucbaocao)
                            {
                                tongdonbaocao++;
                                if (iLinhvuc == 0)
                                {
                                    int tonggiatrilinhvuc = 0;
                                    var thongtinlinhvucbaocao2 = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == k.ILINHVUC).ToList();
                                    foreach (var g in thongtinlinhvucbaocao2)
                                    {
                                        var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == g.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                        tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                                    }

                                    wb.Cell(j, tongdonbaocao).Value = tonggiatrilinhvuc;
                                    wb.Range(j, tongdonbaocao, j, tongdonbaocao).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                    tonggiatrilinhvuc = 0;
                                }
                                else
                                {
                                    int tonggiatrilinhvuc = 0;
                                    var thongtinlinhvucbaocao2 = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == k.ILINHVUC).ToList();
                                    foreach (var g in thongtinlinhvucbaocao2)
                                    {
                                        if (iLinhvuc == g.ILINHVUC)
                                        {
                                            var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == iLinhvuc && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                            tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                                        }

                                    }
                                    wb.Cell(j, tongdonbaocao).Value = tonggiatrilinhvuc;
                                    wb.Range(j, tongdonbaocao, j, tongdonbaocao).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                    tonggiatrilinhvuc = 0;
                                }
                            }

                            wb.Cell(j, 1).Value = t.THOIGIAN;
                            wb.Range(j, 1, j, 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, 2).Value = t.TONGSO;
                            wb.Range(j, 2, j, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, 3).Value = t.TIEPCONGDANTHEOLINHVUC;
                            wb.Range(j, 3, j, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, 4).Value = t.TIEPCONGDANCUACANHANDBQH;
                            wb.Range(j, 4, j, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, 5).Value = t.SOVUVIEC;
                            wb.Range(j, 5, j, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);


                            wb.Cell(j, demhangtiepnhan - 3).Value = t.DOANDONGNGUOI;
                            wb.Range(j, demhangtiepnhan - 3, j, demhangtiepnhan - 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, demhangtiepnhan - 2).Value = t.HUONGDANBANGVANBAN;
                            wb.Range(j, demhangtiepnhan - 2, j, demhangtiepnhan - 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, demhangtiepnhan - 1).Value = t.HUONGDANGIAITHICHTRUCTIEP;
                            wb.Range(j, demhangtiepnhan - 1, j, demhangtiepnhan - 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, demhangtiepnhan).Value = t.CHUYENDENCOQUANCOTHAMQUYEN;
                            wb.Range(j, demhangtiepnhan, j, demhangtiepnhan).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            j++;


                        }

                    }
                    int tongdonbaocao_ = 5;
                    var thongtinloaidonbaocao_ = _thietlap.Get_Loaidon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                    foreach (var k in thongtinloaidonbaocao_)
                    {
                        tongdonbaocao_++;
                        if (iLoaidon == 0)
                        {
                            var thongtintiepcongdanloaidon_ = tiepcongdan.Where(x => x.ILOAIDON == k.ILOAIDON && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                            wb.Cell(j, tongdonbaocao_).Value = thongtintiepcongdanloaidon_.Count();
                            wb.Range(j, tongdonbaocao_, j, tongdonbaocao_).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                        }
                        else
                        {
                            if (iLoaidon == k.ILOAIDON)
                            {
                                var thongtintiepcongdanloaidon_ = tiepcongdan.Where(x => x.ILOAIDON == iLoaidon && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                wb.Cell(j, tongdonbaocao_).Value = thongtintiepcongdanloaidon_.Count();
                                wb.Range(j, tongdonbaocao_, j, tongdonbaocao_).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            }
                            else
                            {
                                wb.Cell(j, tongdonbaocao_).Value = 0;
                                wb.Range(j, tongdonbaocao_, j, tongdonbaocao_).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            }
                        }


                    }

                    var thongtinlinhvucbaocao_ = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
                    foreach (var k in thongtinlinhvucbaocao_)
                    {
                        tongdonbaocao_++;
                        if (iLinhvuc == 0)
                        {
                            int tonggiatrilinhvuc = 0;
                            var thongtinlinhvucbaocao2 = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == k.ILINHVUC).ToList();
                            foreach (var g in thongtinlinhvucbaocao2)
                            {
                                var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == g.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                            }
                            //   var thongtintiepcongdanlinhvuc_ = tiepcongdan.Where(x => x.ILINHVUC == k.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay)).ToList();
                            wb.Cell(j, tongdonbaocao_).Value = tonggiatrilinhvuc;
                            wb.Range(j, tongdonbaocao_, j, tongdonbaocao_).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            tonggiatrilinhvuc = 0;
                        }
                        else
                        {
                            int tonggiatrilinhvuc = 0;
                            var thongtinlinhvucbaocao2 = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == k.ILINHVUC).ToList();
                            foreach (var g in thongtinlinhvucbaocao2)
                            {
                                if (iLinhvuc == g.ILINHVUC)
                                {
                                    var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == iLinhvuc && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                    tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                                }
                            }
                            //   var thongtintiepcongdanlinhvuc_ = tiepcongdan.Where(x => x.ILINHVUC == k.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay)).ToList();
                            wb.Cell(j, tongdonbaocao_).Value = tonggiatrilinhvuc;
                            wb.Range(j, tongdonbaocao_, j, tongdonbaocao_).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            tonggiatrilinhvuc = 0;
                        }



                    }

                    wb.Cell(j, 1).Value = "TỔNG CỘNG";
                    wb.Range(j, 1, j, 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(j, 2).Value = t.TONGSO;
                    wb.Range(j, 2, j, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(j, 3).Value = t.TIEPCONGDANTHEOLINHVUC;
                    wb.Range(j, 3, j, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(j, 4).Value = t.TIEPCONGDANCUACANHANDBQH;
                    wb.Range(j, 4, j, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(j, 5).Value = t.SOVUVIEC;
                    wb.Range(j, 5, j, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);


                    wb.Cell(j, demhangtiepnhan - 3).Value = t.DOANDONGNGUOI;
                    wb.Range(j, demhangtiepnhan - 3, j, demhangtiepnhan - 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(j, demhangtiepnhan - 2).Value = t.HUONGDANBANGVANBAN;
                    wb.Range(j, demhangtiepnhan - 2, j, demhangtiepnhan - 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(j, demhangtiepnhan - 1).Value = t.HUONGDANGIAITHICHTRUCTIEP;
                    wb.Range(j, demhangtiepnhan - 1, j, demhangtiepnhan - 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(j, demhangtiepnhan).Value = t.CHUYENDENCOQUANCOTHAMQUYEN;
                    wb.Range(j, demhangtiepnhan, j, demhangtiepnhan).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                }
                else
                {
                    int i = 2017;
                    int dem = 1;
                    string tungaycuanam = "";
                    string denngaycuanam = "";
                    for (i = Convert.ToInt32(namtungay); i <= Convert.ToInt32(namdenngay); i++)
                    {
                        if (dem == 1 && Convert.ToInt32(namdenngay) - i > 0)
                        {
                            tungaycuanam = tungay;
                            denngaycuanam = "31-Dec-" + i + "";
                        }
                        else if (Convert.ToInt32(namdenngay) - i == 0)
                        {
                            tungaycuanam = "01-Jan-" + i + "";
                            denngaycuanam = denngay;


                        }
                        else
                        {
                            tungaycuanam = "01-Jan-" + i + "";
                            denngaycuanam = "31-Dec-" + i + "";
                        }
                        var list2 = tiepdanreport.getReportBaoBaoThongKeTiepDanPhuLuc("PKG_TCD_BAOCAO.PRO_BAOCAO_TK_PHULUC", tungaycuanam, denngaycuanam, iLoaidon, iLinhvuc, iDonvi);
                        if (list2.Count() > 0)
                        {
                            foreach (var d in list2)
                            {
                                int tongdonbaocao = 5;
                                var thongtinloaidonbaocao = _thietlap.Get_Loaidon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                                foreach (var k in thongtinloaidonbaocao)
                                {
                                    tongdonbaocao++;
                                    if (iLoaidon == 0)
                                    {
                                        var thongtintiepcongdanloaidon = tiepcongdan.Where(x => x.ILOAIDON == k.ILOAIDON && x.DNGAYNHAN >= Convert.ToDateTime(tungaycuanam) && x.DNGAYNHAN <= Convert.ToDateTime(denngaycuanam) && x.IDONVI == iDonvi).ToList();
                                        wb.Cell(j, tongdonbaocao).Value = thongtintiepcongdanloaidon.Count();
                                        wb.Range(j, tongdonbaocao, j, tongdonbaocao).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                    }
                                    else
                                    {
                                        if (iLoaidon == k.ILOAIDON)
                                        {
                                            var thongtintiepcongdanloaidon = tiepcongdan.Where(x => x.ILOAIDON == iLoaidon && x.DNGAYNHAN >= Convert.ToDateTime(tungaycuanam) && x.DNGAYNHAN <= Convert.ToDateTime(denngaycuanam) && x.IDONVI == iDonvi).ToList();
                                            wb.Cell(j, tongdonbaocao).Value = thongtintiepcongdanloaidon.Count();
                                            wb.Range(j, tongdonbaocao, j, tongdonbaocao).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                        }
                                        else
                                        {
                                            wb.Cell(j, tongdonbaocao).Value = 0;
                                            wb.Range(j, tongdonbaocao, j, tongdonbaocao).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                        }
                                    }

                                }
                                var thongtinlinhvucbaocao = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
                                foreach (var k in thongtinlinhvucbaocao)
                                {
                                    tongdonbaocao++;
                                    if (iLinhvuc == 0)
                                    {
                                        int tonggiatrilinhvuc = 0;
                                        var thongtinlinhvucbaocao2 = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == k.ILINHVUC).ToList();
                                        foreach (var g in thongtinlinhvucbaocao2)
                                        {
                                            var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == g.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungaycuanam) && x.DNGAYNHAN <= Convert.ToDateTime(denngaycuanam) && x.IDONVI == iDonvi).ToList();
                                            tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                                        }

                                        wb.Cell(j, tongdonbaocao).Value = tonggiatrilinhvuc;
                                        wb.Range(j, tongdonbaocao, j, tongdonbaocao).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                        tonggiatrilinhvuc = 0;
                                    }
                                    else
                                    {
                                        int tonggiatrilinhvuc = 0;
                                        var thongtinlinhvucbaocao2 = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == k.ILINHVUC).ToList();
                                        foreach (var g in thongtinlinhvucbaocao2)
                                        {
                                            if (iLinhvuc == g.ILINHVUC)
                                            {
                                                var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == iLinhvuc && x.DNGAYNHAN >= Convert.ToDateTime(tungaycuanam) && x.DNGAYNHAN <= Convert.ToDateTime(denngaycuanam) && x.IDONVI == iDonvi).ToList();
                                                tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                                            }

                                        }
                                        wb.Cell(j, tongdonbaocao).Value = tonggiatrilinhvuc;
                                        wb.Range(j, tongdonbaocao, j, tongdonbaocao).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                        tonggiatrilinhvuc = 0;
                                    }
                                }

                                wb.Cell(j, 1).Value = d.THOIGIAN;
                                wb.Range(j, 1, j, 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                wb.Cell(j, 2).Value = d.TONGSO;
                                wb.Range(j, 2, j, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                wb.Cell(j, 3).Value = d.TIEPCONGDANTHEOLINHVUC;
                                wb.Range(j, 3, j, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                wb.Cell(j, 4).Value = d.TIEPCONGDANCUACANHANDBQH;
                                wb.Range(j, 4, j, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                wb.Cell(j, 5).Value = d.SOVUVIEC;
                                wb.Range(j, 5, j, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);


                                wb.Cell(j, demhangtiepnhan - 3).Value = d.DOANDONGNGUOI;
                                wb.Range(j, demhangtiepnhan - 3, j, demhangtiepnhan - 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                wb.Cell(j, demhangtiepnhan - 2).Value = d.HUONGDANBANGVANBAN;
                                wb.Range(j, demhangtiepnhan - 2, j, demhangtiepnhan - 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                wb.Cell(j, demhangtiepnhan - 1).Value = d.HUONGDANGIAITHICHTRUCTIEP;
                                wb.Range(j, demhangtiepnhan - 1, j, demhangtiepnhan - 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                wb.Cell(j, demhangtiepnhan).Value = d.CHUYENDENCOQUANCOTHAMQUYEN;
                                wb.Range(j, demhangtiepnhan, j, demhangtiepnhan).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                j++;


                            }
                            int tongdonbaocao_ = 5;
                            var thongtinloaidonbaocao_ = _thietlap.Get_Loaidon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                            foreach (var k in thongtinloaidonbaocao_)
                            {
                                tongdonbaocao_++;
                                if (iLoaidon == 0)
                                {
                                    var thongtintiepcongdanloaidon_ = tiepcongdan.Where(x => x.ILOAIDON == k.ILOAIDON && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                    wb.Cell(j, tongdonbaocao_).Value = thongtintiepcongdanloaidon_.Count();
                                    wb.Range(j, tongdonbaocao_, j, tongdonbaocao_).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                                }
                                else
                                {
                                    if (iLoaidon == k.ILOAIDON)
                                    {
                                        var thongtintiepcongdanloaidon_ = tiepcongdan.Where(x => x.ILOAIDON == iLoaidon && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                        wb.Cell(j, tongdonbaocao_).Value = thongtintiepcongdanloaidon_.Count();
                                        wb.Range(j, tongdonbaocao_, j, tongdonbaocao_).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                    }
                                    else
                                    {
                                        wb.Cell(j, tongdonbaocao_).Value = 0;
                                        wb.Range(j, tongdonbaocao_, j, tongdonbaocao_).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                    }
                                }


                            }

                            var thongtinlinhvucbaocao_ = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
                            foreach (var k in thongtinlinhvucbaocao_)
                            {
                                tongdonbaocao_++;
                                if (iLinhvuc == 0)
                                {
                                    int tonggiatrilinhvuc = 0;
                                    var thongtinlinhvucbaocao2 = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == k.ILINHVUC).ToList();
                                    foreach (var g in thongtinlinhvucbaocao2)
                                    {
                                        var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == g.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                        tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                                    }
                                    //   var thongtintiepcongdanlinhvuc_ = tiepcongdan.Where(x => x.ILINHVUC == k.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay)).ToList();
                                    wb.Cell(j, tongdonbaocao_).Value = tonggiatrilinhvuc;
                                    wb.Range(j, tongdonbaocao_, j, tongdonbaocao_).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                    tonggiatrilinhvuc = 0;
                                }
                                else
                                {
                                    int tonggiatrilinhvuc = 0;
                                    var thongtinlinhvucbaocao2 = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == k.ILINHVUC).ToList();
                                    foreach (var g in thongtinlinhvucbaocao2)
                                    {
                                        if (iLinhvuc == g.ILINHVUC)
                                        {
                                            var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == iLinhvuc && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                            tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                                        }
                                    }
                                    //   var thongtintiepcongdanlinhvuc_ = tiepcongdan.Where(x => x.ILINHVUC == k.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay)).ToList();
                                    wb.Cell(j, tongdonbaocao_).Value = tonggiatrilinhvuc;
                                    wb.Range(j, tongdonbaocao_, j, tongdonbaocao_).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                    tonggiatrilinhvuc = 0;
                                }



                            }
                            wb.Cell(j, 1).Value = "TỔNG CỘNG";
                            wb.Range(j, 1, j, 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, 2).Value = t.TONGSO;
                            wb.Range(j, 2, j, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, 3).Value = t.TIEPCONGDANTHEOLINHVUC;
                            wb.Range(j, 3, j, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, 4).Value = t.TIEPCONGDANCUACANHANDBQH;
                            wb.Range(j, 4, j, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, 5).Value = t.SOVUVIEC;
                            wb.Range(j, 5, j, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);


                            wb.Cell(j, demhangtiepnhan - 3).Value = t.DOANDONGNGUOI;
                            wb.Range(j, demhangtiepnhan - 3, j, demhangtiepnhan - 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, demhangtiepnhan - 2).Value = t.HUONGDANBANGVANBAN;
                            wb.Range(j, demhangtiepnhan - 2, j, demhangtiepnhan - 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, demhangtiepnhan - 1).Value = t.HUONGDANGIAITHICHTRUCTIEP;
                            wb.Range(j, demhangtiepnhan - 1, j, demhangtiepnhan - 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            wb.Cell(j, demhangtiepnhan).Value = t.CHUYENDENCOQUANCOTHAMQUYEN;
                            wb.Range(j, demhangtiepnhan, j, demhangtiepnhan).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        }
                    }

                }




            }


            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=baocaotiepcongdanphuluc.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                w_b.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null;
        }


        public ActionResult Download_Baocaotiepcongdansolieutinh2()
        {
            if (!CheckAuthToken()) { return null; }
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && Request["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(Server.HtmlEncode(Request["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && Request["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(Server.HtmlEncode(Request["dDenNgay"]));

            }

            var thongtinloaidon = _thietlap.Get_Loaidon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            var thongtinnoidungdon = _thietlap.Get_Noidungdon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            int demhangtiepnhan = 21 + thongtinloaidon.Count() + thongtinnoidungdon.Count();
            int iLinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int iLoaidon = Convert.ToInt32(Request["iLoaidon"]);
            int iDonvi = Convert.ToInt32(Request["iDonvi"]);
            var list = tiepdanreport.getReportBaoBaoThongKeSoLieuTinh("PKG_TCD_BAOCAO.PRO_BAOCAO_TK_SOLIEUTINH", tungay, denngay, iLoaidon, iLinhvuc, iDonvi);
            XLWorkbook w_b = new XLWorkbook();

            var wb = w_b.Worksheets.Add("Số liệu tỉnh");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 13;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Cell(1, 1).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            wb.Range(1, 1, 1, 29).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(2, 1).Value = "Độc lập - Tự do - Hạnh phúc";
            wb.Range(2, 1, 2, 29).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(3, 1).Value = "BÁO CÁO SỐ LIỆU VỀ KẾT QUẢ TIẾP CÔNG DÂN, TIẾP NHÂN ĐƠN,THƯ VÀ KẾT QUẢ XỬ LÝ CỦA ĐOÀN ĐẠI BIỂU QUỐC HỘI CÁC TỈNH ";
            wb.Range(3, 1, 3, 29).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(4, 1).Value = "Từ ngày " + func.ConvertDateVN(tungay) + " đến ngày " + func.ConvertDateVN(denngay) + "";
            wb.Range(4, 1, 4, 29).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            // dong 1
            wb.Style.Font.FontSize = 11;
            wb.Cell(5, 1).Value = "STT";
            wb.Range(5, 1, 10, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 2).Value = "Địa phương";
            wb.Range(5, 2, 10, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 3).Value = "TIẾP CÔNG DÂN";
            wb.Range(5, 3, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 7).Value = "TIẾP NHẬN ĐƠN THƯ ";
            wb.Range(5, 7, 5, 19).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 20).Value = "KẾT QUẢ XỬ LÝ";
            wb.Range(5, 20, 5, 26).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 27).Value = "GIÁM SÁT";
            wb.Range(5, 27, 6, 29).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            // dong 2
            wb.Cell(7, 3).Value = "Số buổi TCD";
            wb.Range(7, 3, 10, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 4).Value = "Lượt người";
            wb.Range(7, 4, 10, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 5).Value = "Số vụ viêc";
            wb.Range(7, 5, 10, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 6).Value = "Đoàn đông người";
            wb.Range(7, 6, 10, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 7).Value = "Tổng số nhận";
            wb.Range(6, 7, 10, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 8).Value = "Khiếu nại ";
            wb.Range(6, 8, 10, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 9).Value = "Tố cáo ";
            wb.Range(6, 9, 10, 9).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 10).Value = "Kiến nghị, phản ánh";
            wb.Range(6, 10, 10, 10).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 11).Value = "Đơn trùng";
            wb.Range(6, 11, 10, 11).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 12).Value = "Phân loại theo nội dung ";
            wb.Range(6, 12, 6, 16).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 17).Value = "Phân loại theo lĩnh vực  ";
            wb.Range(6, 17, 6, 19).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 20).Value = "Đang nghiên cứu   ";
            wb.Range(6, 20, 10, 20).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 21).Value = "Số đơn lưu theo dõi ";
            wb.Range(6, 21, 10, 21).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 22).Value = "Số vụ việc đã chuyển";
            wb.Range(6, 22, 10, 22).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 23).Value = "Số vụ việc đã được thông tin trả lời";
            wb.Range(6, 23, 10, 23).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 24).Value = "Hướng dẫn, giải thích trả lời";
            wb.Range(6, 24, 10, 24).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 25).Value = "Tỉ lệ xử lý/ nhận";
            wb.Range(6, 25, 10, 25).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(6, 26).Value = "Đơn đôn đốc vụ việc cụ thể ";
            wb.Range(6, 26, 10, 26).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            //wb.Cell(6, 27).Value = "GIÁM SÁT  ";
            //wb.Range(6, 27, 7, 29).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            //                                   .Font.SetBold(true).Alignment.SetWrapText(true);
            // Dòng 3 
            wb.Cell(7, 12).Value = "Đất đai  ";
            wb.Range(7, 12, 10, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 13).Value = "Chính sách XH";
            wb.Range(7, 13, 10, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 14).Value = "Vi phạm pháp luật, tham nhũng ";
            wb.Range(7, 14, 10, 14).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 15).Value = "Quản lý kinh tế, XH";
            wb.Range(7, 15, 10, 15).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 16).Value = "Khác";
            wb.Range(7, 16, 10, 16).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 17).Value = "Tư pháp";
            wb.Range(7, 17, 10, 17).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 18).Value = "Hành chính";
            wb.Range(7, 18, 10, 18).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 19).Value = "Khác";
            wb.Range(7, 19, 10, 19).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 27).Value = "Chuyên đề";
            wb.Range(7, 27, 10, 27).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 28).Value = "Lồng ghép";
            wb.Range(7, 28, 10, 28).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 29).Value = "Vụ việc cụ thể";
            wb.Range(7, 29, 10, 29).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            int j = 11;
            int dem = 1;
            foreach (var t in list)
            {
                decimal tyle = 0;
                if (t.TYLE == 0)
                { tyle = 0; }
                else if (t.TYLE == 0 && t.SOVUVIEC == 0)
                {
                    { tyle = 100; }
                }
                else
                {
                    tyle = (t.TYLE / t.SOVUVIEC) * 100;
                }

                wb.Cell(j, 1).Value = dem;
                wb.Range(j, 1, j, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);


                wb.Cell(j, 2).Value = t.DIAPHUONG;
                wb.Range(j, 2, j, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                wb.Cell(j, 3).Value = t.SOBUOITCD;
                wb.Range(j, 3, j, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 4).Value = t.LUOTNGUOI;
                wb.Range(j, 4, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 5).Value = t.SOVUVIEC;
                wb.Range(j, 5, j, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 6).Value = t.DOANDONGNGUOI;
                wb.Range(j, 6, j, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 7).Value = t.TONGDONNHAN;
                wb.Range(j, 7, j, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 8).Value = t.KHIEUNAI;
                wb.Range(j, 8, j, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 9).Value = t.TOCAO;
                wb.Range(j, 9, j, 9).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 10).Value = t.KIENNGHIPHANANH;
                wb.Range(j, 10, j, 10).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 11).Value = t.DONTRUNG;
                wb.Range(j, 11, j, 11).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 12).Value = t.DATDAI;
                wb.Range(j, 12, j, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 13).Value = t.CHINHSACHXH;
                wb.Range(j, 13, j, 13).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 14).Value = t.VIPHAMPLTHAMNHUNG;
                wb.Range(j, 14, j, 14).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 15).Value = t.QUANLIKINHTEXH;
                wb.Range(j, 15, j, 15).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 16).Value = t.KHAC;
                wb.Range(j, 16, j, 16).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 17).Value = t.TUPHAP;
                wb.Range(j, 17, j, 17).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 18).Value = t.HANHCHINH;
                wb.Range(j, 18, j, 18).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 19).Value = t.LINHVUCKHAC;
                wb.Range(j, 19, j, 19).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 20).Value = t.DANGNGHIENCUU;
                wb.Range(j, 20, j, 20).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 21).Value = t.SODONLUUTHEODOI;
                wb.Range(j, 21, j, 21).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 22).Value = t.SOVUVIECDACHUYEN;
                wb.Range(j, 22, j, 22).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 23).Value = t.SOVUVUDATRALOI;
                wb.Range(j, 23, j, 23).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 24).Value = t.HUONGDANTRALOI;
                wb.Range(j, 24, j, 24).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 25).Value = Math.Round(tyle, 2);
                wb.Range(j, 25, j, 25).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 26).Value = t.DONDOCVUVIECCUTHE;
                wb.Range(j, 26, j, 26).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 27).Value = t.CHUYENDE;
                wb.Range(j, 27, j, 27).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 28).Value = t.LONGGHEP;
                wb.Range(j, 28, j, 28).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 29).Value = t.VUVIECCUTHE;
                wb.Range(j, 29, j, 29).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                j++;
                dem++;
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=baocaotiepcongdantheosolieuketqua.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                w_b.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null;

        }

        public int kiemtracapcha(int iparent)
        {
            int kiemtra = 0;
            var thongtinlinhvuc = _thietlap.Get_Linhvuc().Where(x => x.IPARENT == iparent || x.ILINHVUC == iparent).ToList();
            if (thongtinlinhvuc.Count() > 0)
            {
                kiemtra = 1;
            }
            return kiemtra;
        }

        public ActionResult Download_Baocaotiepcongdansolieutinh()
        {
            List<TD_VUVIEC> vuviec = _tiepdan.Get_TDVuviec();
            List<KNTC_LOAIDON> loaidon = _thietlap.Get_Loaidon();
            List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
            List<LINHVUC> Linhvuc = _thietlap.Get_Linhvuc();
            if (!CheckAuthToken()) { return null; }
            string tungay = "", denngay = "";
            if (Request["dTuNgay"] != null && Request["dTuNgay"].ToString() != "")
            {
                tungay = func.ConvertDateToSql(Server.HtmlEncode(Request["dTuNgay"]));

            }
            if (Request["dDenNgay"] != null && Request["dDenNgay"].ToString() != "")
            {
                denngay = func.ConvertDateToSql(Server.HtmlEncode(Request["dDenNgay"]));

            }
            var thongtinloaidon = _thietlap.Get_Loaidon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            var thongtinnoidungdon = _thietlap.Get_Noidungdon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            var thongtinlinhvuc = _thietlap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
            int demhangtiepnhan = 18 + thongtinloaidon.Count() + thongtinnoidungdon.Count() + thongtinlinhvuc.Count();
            int demhang = thongtinloaidon.Count() + thongtinnoidungdon.Count() - 1;

            int iLinhvuc = Convert.ToInt32(Request["iLinhVuc"]);
            int iLoaidon = Convert.ToInt32(Request["iLoaidon"]);
            int iDonvi = Convert.ToInt32(Request["iDonvi"]);
            int iNoiDung = Convert.ToInt32(Request["iNoiDung"]);
            var list = tiepdanreport.getReportBaoBaoThongKeSoLieuTinh("PKG_TCD_BAOCAO.PRO_BAOCAO_TK_SOLIEUTINH", tungay, denngay, iLoaidon, iLinhvuc, iDonvi);
            XLWorkbook w_b = new XLWorkbook();

            var wb = w_b.Worksheets.Add("Số liệu tỉnh");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 13;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Cell(1, 1).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            wb.Range(1, 1, 1, demhangtiepnhan).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(2, 1).Value = "Độc lập - Tự do - Hạnh phúc";
            wb.Range(2, 1, 2, demhangtiepnhan).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(3, 1).Value = "BÁO CÁO SỐ LIỆU VỀ KẾT QUẢ TIẾP CÔNG DÂN, TIẾP NHÂN ĐƠN,THƯ VÀ KẾT QUẢ XỬ LÝ CỦA ĐOÀN ĐẠI BIỂU QUỐC HỘI CÁC TỈNH ";
            wb.Range(3, 1, 3, demhangtiepnhan).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(4, 1).Value = "Từ ngày " + func.ConvertDateVN(tungay) + " đến ngày " + func.ConvertDateVN(denngay) + "";
            wb.Range(4, 1, 4, demhangtiepnhan).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            // dong 1
            wb.Style.Font.FontSize = 11;
            wb.Cell(5, 1).Value = "STT";
            wb.Range(5, 1, 10, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 2).Value = "Địa phương";
            wb.Range(5, 2, 10, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 3).Value = "TIẾP CÔNG DÂN";
            wb.Range(5, 3, 6, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 7).Value = "TIẾP NHẬN ĐƠN THƯ ";
            wb.Range(5, 7, 5, (11 + demhang)).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, (12 + demhang)).Value = "KẾT QUẢ XỬ LÝ";
            wb.Range(5, (12 + demhang), 5, (18 + demhang)).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 19 + demhang).Value = "GIÁM SÁT";
            wb.Range(5, 19 + demhang, 6, demhangtiepnhan - 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            // dong 2
            wb.Cell(7, 3).Value = "Số buổi TCD";
            wb.Range(7, 3, 10, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 4).Value = "Lượt người";
            wb.Range(7, 4, 10, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 5).Value = "Số vụ viêc";
            wb.Range(7, 5, 10, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, 6).Value = "Đoàn đông người";
            wb.Range(7, 6, 10, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            int sohangngangdong2 = 7;
            var thongtinloaidonexcl = _thietlap.Get_Loaidon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var t in thongtinloaidon)
            {
                wb.Cell(6, sohangngangdong2).Value = "" + Server.HtmlDecode(t.CTEN) + "";
                wb.Range(6, sohangngangdong2, 10, sohangngangdong2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                sohangngangdong2++;
            }
            //wb.Cell(6, 8).Value = "Khiếu nại ";
            //wb.Range(6, 8, 10, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            //                                     .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            //wb.Cell(6, 9).Value = "Tố cáo ";
            //wb.Range(6, 9, 10, 9).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            //                                      .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            //wb.Cell(6, 10).Value = "Kiến nghị, phản ánh";
            //wb.Range(6, 10, 10, 10).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            //  
            wb.Cell(6, sohangngangdong2).Value = "Đơn trùng";
            wb.Range(6, sohangngangdong2, 10, sohangngangdong2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            sohangngangdong2++;
            wb.Cell(6, sohangngangdong2).Value = "Phân loại theo nội dung ";
            wb.Range(6, sohangngangdong2, 6, (sohangngangdong2 + thongtinnoidungdon.Count() - 1)).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            sohangngangdong2 = sohangngangdong2 + thongtinnoidungdon.Count();
            wb.Cell(6, sohangngangdong2).Value = "Phân loại theo lĩnh vực  ";
            wb.Range(6, sohangngangdong2, 6, (sohangngangdong2 + 2)).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            sohangngangdong2 = sohangngangdong2 + 3;
            wb.Cell(6, sohangngangdong2).Value = "Đang nghiên cứu   ";
            wb.Range(6, sohangngangdong2, 10, sohangngangdong2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            sohangngangdong2++;
            wb.Cell(6, sohangngangdong2).Value = "Số đơn lưu theo dõi ";
            wb.Range(6, sohangngangdong2, 10, sohangngangdong2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            sohangngangdong2++;
            wb.Cell(6, sohangngangdong2).Value = "Số vụ việc đã chuyển";
            wb.Range(6, sohangngangdong2, 10, sohangngangdong2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            sohangngangdong2++;
            wb.Cell(6, sohangngangdong2).Value = "Số vụ việc đã được thông tin trả lời";
            wb.Range(6, sohangngangdong2, 10, sohangngangdong2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            sohangngangdong2++;
            wb.Cell(6, sohangngangdong2).Value = "Hướng dẫn, giải thích trả lời";
            wb.Range(6, sohangngangdong2, 10, sohangngangdong2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            sohangngangdong2++;
            wb.Cell(6, sohangngangdong2).Value = "Tỉ lệ xử lý/ nhận";
            wb.Range(6, sohangngangdong2, 10, sohangngangdong2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            sohangngangdong2++;
            wb.Cell(6, sohangngangdong2).Value = "Đơn đôn đốc vụ việc cụ thể ";
            wb.Range(6, sohangngangdong2, 10, sohangngangdong2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            //wb.Cell(6, 27).Value = "GIÁM SÁT  ";
            //wb.Range(6, 27, 7, 29).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            //                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            //                                   .Font.SetBold(true).Alignment.SetWrapText(true);
            // Dòng 3 
            int sohangngangdong3 = 8 + thongtinloaidon.Count();
            var thongtinnoidungdonexcl = noidungdon.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var t in thongtinnoidungdonexcl)
            {
                wb.Cell(7, sohangngangdong3).Value = "" + Server.HtmlDecode(t.CTEN) + "";
                wb.Range(7, sohangngangdong3, 10, sohangngangdong3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                sohangngangdong3++;
            }
            var thongtinlinhvucexcl = Linhvuc.Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var t in thongtinlinhvucexcl)
            {
                wb.Cell(7, sohangngangdong3).Value = "" + Server.HtmlDecode(t.CTEN) + "";
                wb.Range(7, sohangngangdong3, 10, sohangngangdong3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                      .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                sohangngangdong3++;
            }


            wb.Cell(7, demhangtiepnhan - 3).Value = "Chuyên đề";
            wb.Range(7, demhangtiepnhan - 3, 10, demhangtiepnhan - 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, demhangtiepnhan - 2).Value = "Lồng ghép";
            wb.Range(7, demhangtiepnhan - 2, 10, demhangtiepnhan - 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                 .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(7, demhangtiepnhan - 1).Value = "Vụ việc cụ thể";
            wb.Range(7, demhangtiepnhan - 1, 10, demhangtiepnhan - 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            int j = 11;
            int dem = 1;
            foreach (var t in list)
            {
                decimal tyle = 0;
                if (t.TYLE == 0)
                { tyle = 0; }
                else if (t.TYLE == 0 && t.SOVUVIEC == 0)
                {
                    { tyle = 100; }
                }
                else
                {
                    tyle = (t.TYLE / t.SOVUVIEC) * 100;
                }

                wb.Cell(j, 1).Value = dem;
                wb.Range(j, 1, j, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);


                wb.Cell(j, 2).Value = t.DIAPHUONG;
                wb.Range(j, 2, j, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                wb.Cell(j, 3).Value = t.SOBUOITCD;
                wb.Range(j, 3, j, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 4).Value = t.LUOTNGUOI;
                wb.Range(j, 4, j, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 5).Value = t.SOVUVIEC;
                wb.Range(j, 5, j, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(j, 6).Value = t.DOANDONGNGUOI;
                wb.Range(j, 6, j, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                int vitri = 7;
                loaidon = loaidon.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                foreach (var x in loaidon)
                {
                    //int igiatriloaidon = 0;
                    if (iLoaidon == 0)
                    {
                        var vuviec1 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.ILOAIDON == x.ILOAIDON && v.IDONVI == iDonvi).ToList();
                        wb.Cell(j, vitri).Value = "" + vuviec1.Count() + "";
                        wb.Range(j, vitri, j, vitri).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    }
                    else
                    {
                        if (iLoaidon == x.ILOAIDON)
                        {
                            var vuviec1 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.ILOAIDON == iLoaidon && v.IDONVI == iDonvi).ToList();
                            wb.Cell(j, vitri).Value = "" + vuviec1.Count() + "";
                            wb.Range(j, vitri, j, vitri).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        }
                        else
                        {
                            wb.Cell(j, vitri).Value = " 0 ";
                            wb.Range(j, vitri, j, vitri).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        }

                    }
                    vitri++;
                }

                wb.Cell(j, vitri).Value = t.DONTRUNG;
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                vitri++;
                noidungdon = noidungdon.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                foreach (var x in noidungdon)
                {
                    int igiatrinoidung = 0;
                    if (iNoiDung == 0)
                    {
                        var vuviec2 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.INOIDUNG == x.INOIDUNG && v.IDONVI == iDonvi && v.ILOAIDON == iLoaidon && v.ILINHVUC == iLinhvuc).ToList();
                        if (iLinhvuc == 0 && iLoaidon != 0)
                        {
                            vuviec2 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.INOIDUNG == x.INOIDUNG && v.IDONVI == iDonvi && v.ILOAIDON == iLoaidon).ToList();
                        }
                        if (iLinhvuc != 0 && iLoaidon == 0)
                        {
                            vuviec2 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.INOIDUNG == x.INOIDUNG && v.IDONVI == iDonvi && v.ILINHVUC == iLinhvuc).ToList();
                        }
                        if (iLinhvuc == 0 && iLoaidon == 0)
                        {
                            vuviec2 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.INOIDUNG == x.INOIDUNG && v.IDONVI == iDonvi).ToList();
                        }

                        igiatrinoidung = vuviec2.Count();
                    }
                    else
                    {
                        if (x.INOIDUNG == iNoiDung)
                        {
                            var vuviec2 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.INOIDUNG == iNoiDung && v.IDONVI == iDonvi && v.ILOAIDON == iLoaidon).ToList();
                            igiatrinoidung = vuviec2.Count();
                        }
                    }

                    wb.Cell(j, vitri).Value = "" + igiatrinoidung + "";
                    wb.Range(j, vitri, j, vitri).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    vitri++;
                    igiatrinoidung = 0;
                }

                var thongtinlinhvucbaocao = Linhvuc.Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
                foreach (var x in thongtinlinhvucbaocao)
                {
                    int tongconglinhvuc = 0;
                    if (iLinhvuc == 0)
                    {
                        var thongtinlinhvuctongcong = Linhvuc.Where(v => v.IDELETE == 0 && v.IPARENT == x.ILINHVUC).ToList();
                        if (iLoaidon == 0)
                        {

                            foreach (var g in thongtinlinhvuctongcong)
                            {
                                var vuviec3 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.ILINHVUC == g.ILINHVUC && v.IDONVI == iDonvi).ToList();
                                tongconglinhvuc += vuviec3.Count();
                            }
                        }
                        else
                        {

                            foreach (var g in thongtinlinhvuctongcong)
                            {
                                var vuviec3 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.ILINHVUC == g.ILINHVUC && v.IDONVI == iDonvi && v.ILOAIDON == iLoaidon).ToList();
                                tongconglinhvuc += vuviec3.Count();
                            }
                        }
                    }
                    else
                    {
                        var thongtinlinhvuctongcong = Linhvuc.Where(v => v.IDELETE == 0 && v.IPARENT == x.ILINHVUC).ToList();
                        if (iLoaidon == 0)
                        {


                            foreach (var g in thongtinlinhvuctongcong)
                            {
                                var vuviec3 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.ILINHVUC == iLinhvuc && v.IDONVI == iDonvi).ToList();
                                tongconglinhvuc += vuviec3.Count();
                            }
                        }
                        else
                        {

                            foreach (var g in thongtinlinhvuctongcong)
                            {
                                var vuviec3 = vuviec.Where(v => v.IDIAPHUONG_0 == t.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.ILINHVUC == iLinhvuc && v.IDONVI == iDonvi && v.ILOAIDON == iLoaidon).ToList();
                                tongconglinhvuc += vuviec3.Count();
                            }
                        }

                    }



                    wb.Cell(j, vitri).Value = "" + tongconglinhvuc + "";
                    wb.Range(j, vitri, j, vitri).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    vitri++;
                    tongconglinhvuc = 0;
                }


                wb.Cell(j, vitri).Value = t.DANGNGHIENCUU;
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                vitri++;
                wb.Cell(j, vitri).Value = t.SODONLUUTHEODOI;
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                vitri++;
                wb.Cell(j, vitri).Value = t.SOVUVIECDACHUYEN;
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                vitri++;
                wb.Cell(j, vitri).Value = t.SOVUVUDATRALOI;
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                vitri++;
                wb.Cell(j, vitri).Value = t.HUONGDANTRALOI;
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                vitri++;
                wb.Cell(j, vitri).Value = Math.Round(tyle, 2);
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                vitri++;
                wb.Cell(j, vitri).Value = t.DONDOCVUVIECCUTHE;
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                vitri++;
                wb.Cell(j, vitri).Value = t.CHUYENDE;
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                vitri++;
                wb.Cell(j, vitri).Value = t.LONGGHEP;
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                vitri++;
                wb.Cell(j, vitri).Value = t.VUVIECCUTHE;
                wb.Range(j, vitri, j, vitri).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                j++;
                dem++;
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=baocaotiepcongdantheosolieuketqua.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                w_b.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null;

        }


        public ActionResult Ajax_LoadLinhVucNoiDung(int iLinhVuc)
        {

            if (!CheckAuthToken()) { return null; }
            try
            {


                string str = "<select name='iNoiDung' id='iNoiDung' class='input-medium chosen-select' style='width:100%' onchange='LoadOpTinhChat()'>" +
                                            "<option value='0'>Chọn tất cả</option>" +
                                            "" + tiepdan.Option_NoiDungDon_ThuocLinhVuc_BaoCao(0, iLinhVuc) + "" +
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







        // End Báo cáo Hiếu
       
                
        
            
        

        public ActionResult Download_exceltracuu()
        {
            if (!CheckAuthToken()) { return null; }
            UserInfor u_info = GetUserInfor();
            int iUser_KQ = u_info.tk_action.iUser;
            int iUser = 0;
            int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
            if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

            if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
            {
                iUser = u_info.tk_action.iUser;
                //iDonViTiepNhan = 0;
            }
            if (u_info.tk_action.is_admin)
            {
                iUser = 0;
                iDonViTiepNhan = 0;
            }

            int loaivuviec = Convert.ToInt32(Request["iLoaiVuViec"]);
            string dTuNgay = DateTime.MinValue.ToString("dd-MMM-yyyy");
            string dDenNgay = DateTime.MaxValue.ToString("dd-MMM-yyyy");
            if (Request["dTuNgay"].ToString() != "")
            {
                dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));

            }
            if (Request["dDenNgay"].ToString() != "")
            {
                dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));

            }
            int tracuudoan = Convert.ToInt32(Request["iDoan_Tracuu"]);
            int idonvitracuu = Convert.ToInt32(Request["iDonVi"]);
            string ctennguoiguitracuu = Request["cNguoiGui_Ten"];
            string cdiachinguoigui = Request["cNguoiGui_DiaChi"];
            string cnoidungtracuu = Request["cNoiDung"];
            int iloaidontracuu = Convert.ToInt32(Request["iLoai"]);
            int ilinhvuctracuu = Convert.ToInt32(Request["iLinhVuc"]);
            int inoidungtracuu = Convert.ToInt32(Request["iNoiDung"]);
            int itinhchattracuu = Convert.ToInt32(Request["iTinhChat"]);
            int itinhtrangxuly = Convert.ToInt32(Request["iHinhThuc"]);
            int ikiemtratrung = Convert.ToInt32(Request["iVuViecTrung"]);
            int itaikhoan = Convert.ToInt32(Request["iTaiKhoan"]);
            List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
            TD_VUVIEC VUVIEC = new TD_VUVIEC();
            if (loaivuviec == 2)
            {

                VUVIEC.ITIEPDOTXUAT = 1;
                VUVIEC.IDINHKY = 0;
            }
            else
            {
                VUVIEC.ITIEPDOTXUAT = 0;
                VUVIEC.IDINHKY = loaivuviec;
            }
            if(itaikhoan != -1)
            {
              iUser = itaikhoan;
            }
            VUVIEC.IDOANDONGNGUOI = tracuudoan;
            VUVIEC.IDONVI = idonvitracuu;
            VUVIEC.CNGUOIGUI_DIACHI = cdiachinguoigui;
            VUVIEC.CNGUOIGUI_TEN = ctennguoiguitracuu;
            VUVIEC.CNOIDUNG = cnoidungtracuu;
            VUVIEC.ILOAIDON = iloaidontracuu;
            VUVIEC.ILINHVUC = ilinhvuctracuu;
            VUVIEC.INOIDUNG = inoidungtracuu;
            VUVIEC.ITINHCHAT = itinhchattracuu;
            VUVIEC.ITINHTRANGXULY = itinhtrangxuly;
            VUVIEC.IVUVIECTRUNG = ikiemtratrung;
            VUVIEC.IUSER = iUser;
            var thongtinvuviec = tiepdanreport.GetListTraCuu_NangCao("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIEC_TRACUU", VUVIEC, dTuNgay, dDenNgay).OrderByDescending(x => x.NGAYNHAN).ToList();
            if (iUser != 0 && iDonViTiepNhan != 0)
            {
                thongtinvuviec = tiepdanreport.GetListTraCuu_NangCao("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIEC_TRACUU", VUVIEC, dTuNgay, dDenNgay).Where(x => x.IDONVI == iDonViTiepNhan && x.USER_ID == iUser).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (iUser == 0 && iDonViTiepNhan != 0)
            {
                thongtinvuviec = tiepdanreport.GetListTraCuu_NangCao("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIEC_TRACUU", VUVIEC, dTuNgay, dDenNgay).Where(x => x.IDONVI == iDonViTiepNhan).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            XLWorkbook w_b = new XLWorkbook();
            var wb = w_b.Worksheets.Add("Tra cứu tiếp dân");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 13;
            wb.Column(1).Width = 5;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Cell(1, 1).Value = "TRA CỨU TIẾP DÂN";
            wb.Range(1, 1, 1,12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(2, 1).Value = "";
            wb.Range(2, 1, 2, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;


            // TIÊU ĐỀ
            wb.Column(1).Width = 5;
            wb.Column(2).Width = 15;
            wb.Column(3).Width = 30;
            wb.Column(4).Width = 60;
            wb.Column(5).Width = 30;
            wb.Column(6).Width = 60;
            wb.Column(7).Width = 15;
            wb.Column(8).Width = 20;
            wb.Column(9).Width = 20;
            wb.Column(10).Width = 20;
            wb.Column(11).Width = 20;
            wb.Column(12).Width = 20;
            wb.Cell(3, 1).Value = "STT";
            wb.Range(3, 1, 3, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(3, 2).Value = "Ngày nhập";
            wb.Range(3, 2, 3, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(3, 3).Value = "Người gửi";
            wb.Range(3, 3, 3, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(3, 4).Value = "Địa chỉ";
            wb.Range(3, 4, 3, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(3, 5).Value = "Người tiếp";
            wb.Range(3, 5, 3, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(3,6).Value = "Nội dung";
            wb.Range(3,6, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(3,7).Value = "Loại đơn";
            wb.Range(3, 7, 3, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(3, 8).Value = "Lĩnh vực";
            wb.Range(3,8, 3, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(3, 9).Value = "Nhóm nội dung";
            wb.Range(3, 9, 3, 9).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(3, 10).Value = "Tính chất vụ việc";
            wb.Range(3, 10, 3, 10).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            wb.Cell(3, 11).Value = "Hình thức xử lý";
            wb.Range(3, 11, 3, 11).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(3, 12).Value = "Kết quả trả lời";
            wb.Range(3, 12, 3, 12).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            // END TIÊU ĐỀ
            int i = 4;
            int stt = 1;
            if (thongtinvuviec != null && thongtinvuviec.Count() > 0)
            {
                string trangthaixuly = "";
                foreach (var x in thongtinvuviec)
                {
                    if (x.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                    {
                        trangthaixuly = "" + StringEnum.GetStringValue(TrangThaiXuLy.DangNghienCuu) + "";
                    }
                    if (x.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                    {

                        if (x.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                        {
                            trangthaixuly = "" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "";
                        }
                        else if (x.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                        {
                            trangthaixuly = "" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "";
                        }
                        else if (x.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            trangthaixuly = "" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "";
                        }
                    }
                   // string id_encr = HashUtil.Encode_ID(x.IDVUVIEC.ToString(), Request.Cookies["url_key"].Value);
                   // DinhKy_VuViec t = td.DinhKy_VuViec_Detail((int)x.IDVUVIEC, id_encr);
                    string thongtintinh = "";
                    string thongtinhuyen = "";
                    string diachinguoigui = "";
                    if (x.TENTINH != null)
                    {
                        thongtintinh = x.TENTINH + "  ";
                    }
                    if (x.TENHUYEN != null)
                    {
                        thongtinhuyen = x.TENHUYEN + " , ";
                    }
                    if (x.DIACHINGUOIGUI != null)
                    {
                        diachinguoigui = "" + x.DIACHINGUOIGUI + " , ";
                    }
                    string ketqua = "";
                    var thongtintraloixl = Xuly.Where(p => p.CLOAI == "traloichuyenxuly" && p.IVUVIEC == x.IDVUVIEC && x.TRANGTHAIXULY == 3).ToList();
                    if (thongtintraloixl.Count() > 0)
                    {
                        ketqua = "Đã có trả lời";
                    }
                    wb.Cell(i, 1).Value = stt;
                    wb.Range(i, 1, i, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                            .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(i, 2).Value = func.ConvertDateVN(x.NGAYNHAN.ToString());
                    wb.Range(i, 2, i, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                           .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                           .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(i, 3).Value = Server.HtmlDecode(x.TENNGUOIGUI);
                    wb.Range(i, 3, i, 3).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(i, 4).Value = Server.HtmlDecode(diachinguoigui + thongtinhuyen + thongtintinh);
                    wb.Range(i, 4, i, 4).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(i, 5).Value = Server.HtmlDecode(x.TENNGUOITIEP);
                    wb.Range(i, 5, i, 5).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(i, 6).Value = Server.HtmlDecode(x.NOIDUNGVUVIEC);
                    wb.Range(i, 6, i, 6).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                    wb.Cell(i,7).Value = Server.HtmlDecode(x.LOAIDON);
                    wb.Range(i, 7, i,7).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(i, 8).Value = Server.HtmlDecode(x.TENLINHVUC);
                    wb.Range(i, 8, i, 8).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(i, 9).Value = Server.HtmlDecode(x.NHOMNOIDUNG);
                    wb.Range(i, 9, i, 9).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(i, 10).Value = Server.HtmlDecode(x.TINHCHAT);
                    wb.Range(i, 10, i, 10).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                   
                    wb.Cell(i, 11).Value = trangthaixuly;
                    wb.Range(i, 11, i, 11).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                            .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin); 
                    wb.Cell(i, 12).Value = Server.HtmlDecode(ketqua);
                    wb.Range(i, 12, i, 12).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    i++;
                    stt++;
                    trangthaixuly = "";
                    ketqua = "";
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=TracuuTiepcongdan.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                w_b.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null; 
        }

        
    }
}
