﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;


namespace VIETTEL.Report_Controllers.CongSan
{
    public class rptKTCS_TongHopMau10Controller : Microsoft.AspNetCore.Mvc.Controller
    {
        //
        // GET: /rptKTCS_TongHopMau10/

        public string sViewPath = "~/Report_Views/";
        public Microsoft.AspNetCore.Mvc.ActionResult Index(String LoaiBaoCao = "", String KhoGiay = "")
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                if (LoaiBaoCao == "0")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10A_A3.xls";
                }
                else if (LoaiBaoCao == "1")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10B_A3.xls";
                }
                else if (LoaiBaoCao == "2")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10C_A3.xls";
                }
               
            }
            else
            {
                if (LoaiBaoCao == "0")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10A.xls";
                }
                else if (LoaiBaoCao == "1")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10B.xls";
                }
                else if (LoaiBaoCao == "2")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10C.xls";
                }
               
            }

            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_TongHopMau10.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Action thực hiện việc load dữ liệu theo các điều kiện 
        /// </summary>
        /// <returns></returns>
        public Microsoft.AspNetCore.Mvc.ActionResult EditSubmit(String ParentID)
        {
            String NamChungTu = Request.Form[ParentID + "_NamChungTu"];
            String LoaiBaoCao = Request.Form[ParentID + "_LoaiBaoCao"];
            String iID_MaLoaiTaiSan = Request.Form[ParentID + "_iID_MaLoaiTaiSan"];
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TongHopDonVi = Convert.ToString(Request.Form[ParentID + "_TongHopDonVi"]);
            String TongHopLTS = Convert.ToString(Request.Form[ParentID + "_TongHopLTS"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            return RedirectToAction("Index", new { NamChungTu = NamChungTu, LoaiBaoCao = LoaiBaoCao, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
        }
        /// <summary>
        /// Hiển thị báo cáo
        /// </summary>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String NamChungTu, String LoaiBaoCao, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            //Lấy tên đơn vị
            String tendv = "";
            DataTable teN = TenDonVi(iID_MaDonVi);
            if (teN.Rows.Count > 0)
            {
                tendv = teN.Rows[0][0].ToString();
            }
            /// <summary>
            /// lấy dữ liệu cho ngày tháng năm hiện tài
            /// </summary>
            /// <returns></returns>
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            /// <summary>
            /// Hàm đổi số tiền từ số ra chữ
            /// </summary>
            /// <returns></returns>
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTCS_TongHopMau10");
            LoadData(fr, NamChungTu, LoaiBaoCao, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
            fr.SetValue("Nam", NamChungTu);
            if (TongHopDonVi == "on")
            {
                fr.SetValue("TenDV", "Tổng hợp đơn vị");
            }
            else
            {
                fr.SetValue("TenDV", tendv);
            }
            fr.SetValue("iID_MaDonVi", iID_MaDonVi);
            String MaND = User.Identity.Name;
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1,MaND));
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2,MaND));
            fr.SetValue("Ngay", NgayThang);
            fr.Run(Result);
            return Result;

        }
        /// <summary>
        /// Tạo ra 1 dataTable cho commbox loại báo cáo
        /// </summary>
        /// <returns></returns>
        public static DataTable DTLoaiBaoCao()
        {

            DataTable dtloaiBC = new DataTable();
            dtloaiBC.Columns.Add("MaLoaiBaoCao", (typeof(string)));
            dtloaiBC.Columns.Add("TenLoaiBaoCao", (typeof(string)));

            DataRow r = dtloaiBC.NewRow();
            r["MaLoaiBaoCao"] = "0";
            r["TenLoaiBaoCao"] = "Báo Cáo Tổng hợp tình hình tăng giảm TSCĐ - 10A";
            dtloaiBC.Rows.InsertAt(r, 0);

            DataRow r1 = dtloaiBC.NewRow();
            r1["MaLoaiBaoCao"] = "1";
            r1["TenLoaiBaoCao"] = "Báo Cáo Tổng hợp tình hình tăng giảm TSCĐ - 10B";
            dtloaiBC.Rows.InsertAt(r1, 1);

            DataRow r2 = dtloaiBC.NewRow();
            r2["MaLoaiBaoCao"] = "2";
            r2["TenLoaiBaoCao"] = "Báo Cáo Tổng hợp tình hình tăng giảm TSCĐ - 10C";
            dtloaiBC.Rows.InsertAt(r2, 2);
            return dtloaiBC;
        }
        /// <summary>
        /// DataTable lấy dữ liệu cho báo cáo
        /// </summary>
        /// <returns></returns>
        public DataTable rptKT_BaoCao10(String NamChungTu, String LoaiBaoCao, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            DataTable dt = null;
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }

            String DKDonVi = "iID_MaDonVi='-111'", DKNhomTaiSan = "";
            SqlCommand cmd = new SqlCommand();
            if (TongHopDonVi == "on")
            {
                DataTable dtDonVi = KTCS_ReportModel.ListDonVi();
                if (dtDonVi.Rows.Count > 0)
                {
                    DKDonVi = "";
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        DKDonVi += "iID_MaDonVi = '" + dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + "'";
                        if (i < dtDonVi.Rows.Count - 1)
                            DKDonVi += " OR ";

                    }
                    dtDonVi.Dispose();
                }
            }
            else
            {
                DKDonVi = "";
                DKDonVi = " iID_MaDonVi LIKE N'" + iID_MaDonVi + "%'";
            }
            if (TongHopLTS == "on")
            {
                DataTable dtNhomTaiSan = KTCS_ReportModel.DT_LoaiTS();
                for (int i = 0; i < dtNhomTaiSan.Rows.Count; i++)
                {
                    DKNhomTaiSan += "iID_MaNhomTaiSan like N'" + dtNhomTaiSan.Rows[i]["iID_MaNhomTaiSan"].ToString() + "%'";
                    if (i < dtNhomTaiSan.Rows.Count - 1)
                        DKNhomTaiSan += " OR ";
                }
            }
            else
            {
                DKNhomTaiSan = " iID_MaNhomTaiSan LIKE N'" + iID_MaLoaiTaiSan + "%'";
            }
            String SQL = String.Format(@"SELECT a.iID_MaTaiSan,sTenTaiSan,Tang,Giam,iLoaiTS,rNguyenGia,rDienTich,rSoLuong FROM (
SELECT iID_MaTaiSan,
Tang=SUM(CASE WHEN (sTinhChat='1' OR sTinhChat='T') THEN rSoTien/1000 ELSE 0 END),
Giam=SUM(CASE WHEN (sTinhChat='2' OR sTinhChat='G') THEN rSoTien/1000 ELSE 0 END)
FROM KTCS_ChungTuChiTiet
WHERE iNamLamViec=@iNamLamViec
GROUP BY iID_MaTaiSan) as A

INNER JOIN (
SELECT iID_MaDonVi,iLoaiTS,iID_MaNhomTaiSan,iID_MaTaiSan,sTenTaiSan,SUM(rNguyenGia)/1000 as rNguyenGia,SUM(rSoLuong) as rSoLuong
FROM KTCS_TaiSan
WHERE iNamLamViec=@iNamLamViec AND ({0})
GROUP BY iID_MaDonVi,iID_MaNhomTaiSan,iLoaiTS,iID_MaTaiSan,sTenTaiSan) as B
ON a.iID_MaTaiSan=B.iID_MaTaiSan

INNER JOIN (
SELECT iID_MaTaiSan,SUM(rDTLamNhaLamViec+rDTLamCoSoHDSN+rDTLamNhaO+rDTChoThue+rDTBoTrong+rDTBiLanChiem+rDTKhac) as rDienTich
FROM KTCS_TaiSanChiTiet
GROUP BY iID_MaTaiSan) as C
ON a.iID_MaTaiSan=c.iID_MaTaiSan
INNER JOIN(
SELECT iID_MaNhomTaiSan
FROM KTCS_NhomTaiSan
WHERE iTrangThai=1 AND bLahangCha=0 AND ({1})) as d
ON b.iID_MaNhomTaiSan=d.iID_MaNhomTaiSan
INNER JOIN (SELECT iID_MaDonVi,iID_MaLoaiDonVi,sTen as sTenDonVi FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec) as e
ON B.iID_MaDonVi=e.iID_MaDonVi
INNER JOIN (SELECT iID_MaDanhMuc,sTen  as sTenLoaiDonVi FROM DC_DanhMuc
WHERE iID_MaLoaiDanhMuc='454902b4-485c-476c-9611-9715bef082bb') as f
ON e.iID_MaLoaiDonVi=f.iID_MaDanhMuc

", DKDonVi,DKNhomTaiSan);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamChungTu);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();


            //CUoi ky
            String SQL_CK = String.Format(@"SELECT a.iID_MaTaiSan,sTenTaiSan,Tang,Giam,iLoaiTS,rNguyenGia,rDienTich,rSoLuong,e.iID_MaDonVi,sTenDonVi,f.iID_MaDanhMuc,sTenLoaiDonVi FROM (
SELECT iID_MaTaiSan,
Tang=SUM(CASE WHEN (sTinhChat='1' OR sTinhChat='T') THEN rSoTien/1000 ELSE 0 END),
Giam=SUM(CASE WHEN (sTinhChat='2' OR sTinhChat='G') THEN rSoTien/1000 ELSE 0 END)
FROM KTCS_ChungTuChiTiet
WHERE iNamLamViec<=@iNamLamViec
GROUP BY iID_MaTaiSan) as A

INNER JOIN (
SELECT iID_MaDonVi,iLoaiTS,iID_MaNhomTaiSan,iID_MaTaiSan,sTenTaiSan,SUM(rNguyenGia)/1000 as rNguyenGia,SUM(rSoLuong) as rSoLuong
FROM KTCS_TaiSan
WHERE iNamLamViec<=@iNamLamViec AND ({0})
GROUP BY iID_MaDonVi,iID_MaNhomTaiSan,iLoaiTS,iID_MaTaiSan,sTenTaiSan) as B
ON a.iID_MaTaiSan=B.iID_MaTaiSan

INNER JOIN (
SELECT iID_MaTaiSan,SUM(rDTLamNhaLamViec+rDTLamCoSoHDSN+rDTLamNhaO+rDTChoThue+rDTBoTrong+rDTBiLanChiem+rDTKhac) as rDienTich
FROM KTCS_TaiSanChiTiet
GROUP BY iID_MaTaiSan) as C
ON a.iID_MaTaiSan=c.iID_MaTaiSan
INNER JOIN(
SELECT iID_MaNhomTaiSan
FROM KTCS_NhomTaiSan
WHERE iTrangThai=1 AND bLahangCha=0 AND ({1})) as d
ON b.iID_MaNhomTaiSan=d.iID_MaNhomTaiSan
INNER JOIN (SELECT iID_MaDonVi,iID_MaLoaiDonVi,sTen as sTenDonVi FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec) as e
ON B.iID_MaDonVi=e.iID_MaDonVi
INNER JOIN (SELECT iID_MaDanhMuc,sTen  as sTenLoaiDonVi FROM DC_DanhMuc
WHERE iID_MaLoaiDanhMuc='454902b4-485c-476c-9611-9715bef082bb') as f
ON e.iID_MaLoaiDonVi=f.iID_MaDanhMuc
", DKDonVi, DKNhomTaiSan);
            cmd = new SqlCommand();
            cmd.CommandText = SQL_CK;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamChungTu);
            DataTable dt_CK = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //ghep dt vao dt_ck
            dt_CK.Columns.Add("rSoLuong_TrongKy",typeof(decimal));
            dt_CK.Columns.Add("rDienTich_TrongKy", typeof(decimal));
            dt_CK.Columns.Add("rTang_TrongKy", typeof(decimal));
            dt_CK.Columns.Add("rGiam_TrongKy", typeof(decimal));

            foreach (DataRow rCK in dt_CK.Rows)
            {
                foreach (DataRow rTrongKy in dt.Rows)
                {
                    if (rCK["iID_MaTaiSan"].ToString() == rTrongKy["iID_MaTaiSan"].ToString())
                    {
                        if (rTrongKy["rSoLuong"] != DBNull.Value)
                        {
                            rCK["rSoLuong_TrongKy"] = Convert.ToDecimal(rTrongKy["rSoLuong"]);
                        }
                       
                        if (rTrongKy["rDienTich"] != DBNull.Value)
                        {
                            rCK["rDienTich_TrongKy"] = Convert.ToDecimal(rTrongKy["rDienTich"]);
                        }
                        if (rTrongKy["Tang"] != DBNull.Value)
                        {
                            rCK["rTang_TrongKy"] = Convert.ToDecimal(rTrongKy["Tang"]);
                        }
                        if (rTrongKy["Giam"] != DBNull.Value)
                        {
                            rCK["rGiam_TrongKy"] = Convert.ToDecimal(rTrongKy["Giam"]);
                        }
                       
                    }
                }
            }
            return dt_CK;
        }
        /// <summary>
        /// DataTable lấy tên đơn vị
        /// </summary>
        /// <returns></returns>
        public DataTable TenDonVi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Hàm Load Dữ liệu ra báo cáo
        /// </summary>
        /// <returns></returns>
        private void LoadData(FlexCelReport fr, String NamChungTu, String LoaiBaoCao, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
            {
                iID_MaLoaiTaiSan = Guid.Empty.ToString();
            }
            DataTable data = rptKT_BaoCao10(NamChungTu, LoaiBaoCao, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
            if (LoaiBaoCao == "0")
            {
                fr.AddTable("ChiTiet", data);
                data.TableName = "ChiTiet";
                DataTable dtLoai;
                dtLoai = HamChung.SelectDistinct("LoaiTS", data, "iLoaiTS", "iLoaiTS", "");
                dtLoai.Columns.Add("sTenTS", typeof(String));
                foreach (DataRow r in dtLoai.Rows)
                {
                    if (r["iLoaiTS"].ToString() == "1")
                    {
                        r["sTenTs"] = "1.Đất,khuôn viên";
                    }
                    else if (r["iLoaiTS"].ToString() == "2")
                    {
                        r["sTenTs"] = "2.Nhà";
                    }
                    else if (r["iLoaiTS"].ToString() == "3")
                    {
                        r["sTenTs"] = "3.Xe ô tô";
                    }
                    else
                    {
                        r["sTenTs"] = "4.Tài sản có giá trị trên 500 Tr.đ";
                    }
                }
                fr.AddTable("LoaiTS", dtLoai);
                dtLoai.Dispose();
            }
            else if (LoaiBaoCao == "1")
            {
                fr.AddTable("ChiTiet", data);
                data.TableName = "ChiTiet";
                DataTable dtLoai;
                dtLoai = HamChung.SelectDistinct("LoaiTS", data, "iLoaiTS,iID_MaDanhMuc", "iLoaiTS,sTenLoaiDonVi,iID_MaDanhMuc", "");
                dtLoai.Columns.Add("sTenTS", typeof(String));
                foreach (DataRow r in dtLoai.Rows)
                {
                    if (r["iLoaiTS"].ToString() == "1")
                    {
                        r["sTenTs"] = "1.Đất,khuôn viên";
                    }
                    else if (r["iLoaiTS"].ToString() == "2")
                    {
                        r["sTenTs"] = "2.Nhà";
                    }
                    else if (r["iLoaiTS"].ToString() == "3")
                    {
                        r["sTenTs"] = "3.Xe ô tô";
                    }
                    else
                    {
                        r["sTenTs"] = "4.Tài sản có giá trị trên 500 Tr.đ";
                    }
                }
                fr.AddTable("LoaiTS", dtLoai);
                DataTable dtLoaiDV;
                dtLoaiDV = HamChung.SelectDistinct("LoaiDV", dtLoai, "iID_MaDanhMuc", "iID_MaDanhMuc,sTenLoaiDonVi", "");
                fr.AddTable("LoaiDV", dtLoaiDV);
                dtLoai.Dispose();
                dtLoaiDV.Dispose();
            }
            else
            {
                fr.AddTable("ChiTiet", data);
                data.TableName = "ChiTiet";
                DataTable dtLoai;
                dtLoai = HamChung.SelectDistinct("LoaiTS", data, "iLoaiTS,iID_MaDonVi", "iLoaiTS,sTenDonVi,iID_MaDonVi", "");
                dtLoai.Columns.Add("sTenTS", typeof(String));
                foreach (DataRow r in dtLoai.Rows)
                {
                    if (r["iLoaiTS"].ToString() == "1")
                    {
                        r["sTenTs"] = "1.Đất,khuôn viên";
                    }
                    else if (r["iLoaiTS"].ToString() == "2")
                    {
                        r["sTenTs"] = "2.Nhà";
                    }
                    else if (r["iLoaiTS"].ToString() == "3")
                    {
                        r["sTenTs"] = "3.Xe ô tô";
                    }
                    else
                    {
                        r["sTenTs"] = "4.Tài sản có giá trị trên 500 Tr.đ";
                    }
                }
                fr.AddTable("LoaiTS", dtLoai);
                DataTable dtLoaiDV;
                dtLoaiDV = HamChung.SelectDistinct("LoaiDV", dtLoai, "iID_MaDonVi", "iID_MaDonVi,sTenDonVi", "");
                fr.AddTable("LoaiDV", dtLoaiDV);
                dtLoai.Dispose();
                dtLoaiDV.Dispose();
            }
            data.Dispose();

        }
        /// <summary>
        /// Xuất dữ liệu ra file PDF
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String NamChungTu, String LoaiBaoCao, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                if (LoaiBaoCao == "0")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10A_A3.xls";
                }
                else if (LoaiBaoCao == "1")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10B_A3.xls";
                }
                else if (LoaiBaoCao == "2")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10C_A3.xls";
                }

            }
            else
            {
                if (LoaiBaoCao == "0")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10A.xls";
                }
                else if (LoaiBaoCao == "1")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10B.xls";
                }
                else if (LoaiBaoCao == "2")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10C.xls";
                }

            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, LoaiBaoCao, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "Test.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }

            }
        }
        /// <summary>
        /// Action xem báo cáo
        /// </summary>
        /// <returns></returns>
        public Microsoft.AspNetCore.Mvc.ActionResult ViewPDF(String NamChungTu, String LoaiBaoCao, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                if (LoaiBaoCao == "0")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10A_A3.xls";
                }
                else if (LoaiBaoCao == "1")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10B_A3.xls";
                }
                else if (LoaiBaoCao == "2")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10C_A3.xls";
                }

            }
            else
            {
                if (LoaiBaoCao == "0")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10A.xls";
                }
                else if (LoaiBaoCao == "1")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10B.xls";
                }
                else if (LoaiBaoCao == "2")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10C.xls";
                }

            }
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, LoaiBaoCao, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "BaoCao");
                    pdf.EndExport();
                    ms.Position = 0;
                    return File(ms.ToArray(), "application/pdf");
                }

            }
            return null;
        }
        /// <summary>
        /// Action thực hiện Xuất dữ liệ ra file excel
        /// </summary>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String NamChungTu, String LoaiBaoCao, String iID_MaLoaiTaiSan, String iID_MaDonVi, String TongHopDonVi, String TongHopLTS, String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                if (LoaiBaoCao == "0")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10A_A3.xls";
                }
                else if (LoaiBaoCao == "1")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10B_A3.xls";
                }
                else if (LoaiBaoCao == "2")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10C_A3.xls";
                }

            }
            else
            {
                if (LoaiBaoCao == "0")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10A.xls";
                }
                else if (LoaiBaoCao == "1")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10B.xls";
                }
                else if (LoaiBaoCao == "2")
                {
                    sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_10C.xls";
                }

            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), NamChungTu, LoaiBaoCao, iID_MaLoaiTaiSan, iID_MaDonVi, TongHopDonVi, TongHopLTS, KhoGiay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "BaoCaoTongHop.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public static DataTable ListDonVi(String NamChungTu)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT iID_MaDonVi,sTenDonVi FROM KTCS_ChungTuChiTiet WHERE iTrangThai=1 AND iNamLamViec=@NamChungTu ORDER BY iID_MaDonVi");
            cmd.Parameters.AddWithValue("@NamChungTu", NamChungTu);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Lấy danh sách loại tài sản
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable DT_LoaiTS()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand(@"SELECT iID_MaLoaiTaiSan,iID_MaLoaiTaiSan+' - '+sTen as TenHT 
                                                FROM KTCS_LoaiTaiSan WHERE iTrangThai=1 
                                                ORDER BY iSTT");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}
