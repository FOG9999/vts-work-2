﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;
namespace VIETTEL.Report_Controllers.KeToan.TienMat
{
    public class rptKTTienMat_PhieuChiController : Microsoft.AspNetCore.Mvc.Controller
    {
        //
        // GET: /rptKTTienMat_PhieuChi/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePathPT1 = "/Report_ExcelFrom/KeToan/TienMat/rptKTTienMat_PhieuChi1.xls";
        private const String sFilePathPT2 = "/Report_ExcelFrom/KeToan/TienMat/rptKTTienMat_PhieuChi2.xls";
        private const String sFilePathPT3 = "/Report_ExcelFrom/KeToan/TienMat/rptKTTienMat_PhieuChi3.xls";
        //in khổ A5
        private const String sFilePathPT4 = "/Report_ExcelFrom/KeToan/TienMat/rptKTTienMat_PhieuChi4.xls";
         public static String soCT = "";
        public static String ngayGS = "";
        public static String ten = "";
        public static String diachi = "";
        public static String ghichu = "";
        public static String NoiDung = "";
        public static String NameFile = "";
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/TienMat/rptKTTienMat_PhieuChi.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        
        public Microsoft.AspNetCore.Mvc.ActionResult EditSubmit(String ParentID, String iNamLamViec, String iThang, int ChiSo)
        {
            String sMaChungTuChiTiet = Request.Form[ParentID + "_iID_MaChungTuChiTiet"];
            if (String.IsNullOrEmpty(sMaChungTuChiTiet))
                return RedirectToAction("Index", "KeToanChiTietTienMat");
            else
            {
                String[] arrMaChungTuChiTiet = sMaChungTuChiTiet.Split(',');
                String iID_MaChungTuChiTiet = arrMaChungTuChiTiet[ChiSo];
                String LoaiBaoCao = Request.Form[ParentID + "_LoaiBaoCao"];
                String iID_MaChungTu = Request.Form[ParentID + "_iID_MaChungTu"];
                String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
                ViewData["siID_MaChungTuChiTiet"] = sMaChungTuChiTiet;
                ViewData["ChiSo"] = ChiSo;
                ViewData["iNamLamViec"] = iNamLamViec;
                ViewData["iThang"] = iThang;
                ViewData["LoaiBaoCao"] = LoaiBaoCao;
                ViewData["iID_MaChungTu"] = iID_MaChungTu;
                ViewData["path"] = "~/Report_Views/KeToan/TienMat/rptKTTienMat_PhieuChi.aspx";
                return View(sViewPath + "ReportView.aspx");
            }

        }
        public ExcelFile CreateReport(String path, String iNamLamViec, String iThang, String iID_MaChungTuChiTiet, String LoaiBaoCao, String iID_MaChungTu)
        {

            FlexCelReport fr = new FlexCelReport();
            String MaND = User.Identity.Name;
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1,MaND);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2,MaND);
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //Load data
            DuLieu _DuLieu = new DuLieu();
            _DuLieu = PhieuThu(iNamLamViec, iThang, iID_MaChungTuChiTiet, iID_MaChungTu);
            String TK1 = "", TK2 = "";
            DataTable dtNo = _DuLieu.dtNo;
            DataTable dtCo = _DuLieu.dtCo;
            if (_DuLieu.dtNo.Rows.Count == 1 && _DuLieu.dtCo.Rows.Count >= 1)
            {
                TK1 = "Nợ";
                TK2 = "Có";
              
            }
            else if (_DuLieu.dtNo.Rows.Count >= 1 && _DuLieu.dtCo.Rows.Count == 1)
            {
                TK2 = "Nợ";
                TK1 = "Có";
                dtNo = _DuLieu.dtCo;
                dtCo = _DuLieu.dtNo;
            }

            fr.AddTable("dtNo1", dtNo);
            fr.AddTable("dtCo1", dtCo);

            fr.AddTable("dtNo", dtNo);
            fr.AddTable("dtCo", dtCo);
            fr.SetValue("TenNguoiNhan", _DuLieu.TenNguoiNhan);
            fr.SetValue("DiaChi", _DuLieu.DiaChi);
            fr.SetValue("Lydo", _DuLieu.Lydo);
            fr.SetValue("GhiChu", _DuLieu.GhiChu);
            fr.SetValue("SoTien", _DuLieu.SoTien);
            fr.SetValue("NgayCT", _DuLieu.NgayCT);
            fr.SetValue("ThangCT", _DuLieu.ThangCT);
            fr.SetValue("SoPT", _DuLieu.SoPT);
            String DuongDan = "";
            if (LoaiBaoCao == "PC1")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "PC2")
            {
                DuongDan = sFilePathPT2;
            }
            else if (LoaiBaoCao == "PC3")
            {
                DuongDan = sFilePathPT3;
            }
            else
            {
                DuongDan = sFilePathPT4;
            }

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(DuongDan));

            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTienMat_PhieuChi");
            fr.SetValue("TK1", TK1);
            fr.SetValue("TK2", TK2);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Thang", iThang);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("Ngay", String.Format("{0:dd}", dt));
            fr.SetValue("Thangs", String.Format("{0:MM}", dt));
            fr.SetValue("Nams", DateTime.Now.Year);
            fr.SetValue("Tien", CommonFunction.TienRaChu(long.Parse(_DuLieu.SoTien.ToString())));
            fr.Run(Result);
            return Result;
        }
        public long tong = 0;


        public clsExcelResult ExportToPDF(String iNamLamViec, String iThang, String iID_MaChungTuChiTiet, String LoaiBaoCao, String iID_MaChungTu)
        {
            String DuongDan = "";
            if (LoaiBaoCao == "PC1")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "PC2")
            {
                DuongDan = sFilePathPT2;
            }

            else
            {
                DuongDan = sFilePathPT3;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNamLamViec, iThang, iID_MaChungTuChiTiet, LoaiBaoCao,iID_MaChungTu);

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
        public clsExcelResult ExportToExcel(String iNamLamViec, String iThang, String iID_MaChungTuChiTiet, String LoaiBaoCao, String iID_MaChungTu)
        {
            HamChung.Language();
            String DuongDan = "";
            if (LoaiBaoCao == "PC1")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "PC2")
            {
                DuongDan = sFilePathPT2;
            }
            else if (LoaiBaoCao == "PC3")
            {
                DuongDan = sFilePathPT3;
            }
            else
            {
                DuongDan = sFilePathPT4;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNamLamViec, iThang, iID_MaChungTuChiTiet, LoaiBaoCao, iID_MaChungTu);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ChoDoanhnghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }

        public Microsoft.AspNetCore.Mvc.ActionResult ViewPDF(string MaND, String iID_MaChungTuChiTiet, String LoaiBaoCao, String iID_MaChungTu)
        {
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            dtCauHinh.Dispose();
            //String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
            String DuongDan = "";
            if (LoaiBaoCao == "PC1")
            {
                DuongDan = sFilePathPT1;
            }
            else if (LoaiBaoCao == "PC2")
            {
                DuongDan = sFilePathPT2;
            }
            else if (LoaiBaoCao == "PC3")
            {
                DuongDan = sFilePathPT3;
            }
            else
            {
                DuongDan = sFilePathPT4;
            }
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), iNam, iThang, iID_MaChungTuChiTiet, LoaiBaoCao, iID_MaChungTu);
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

        //private void LoadData(FlexCelReport fr, String iNamLamViec, String iThang, String iID_MaChungTuChiTiet, String iID_MaChungTu)
        //{
        //    DuLieu _DuLieu = new DuLieu();
        //    _DuLieu = PhieuThu(iNamLamViec, iThang, iID_MaChungTuChiTiet, iID_MaChungTu);
        //    DataTable dtNo = _DuLieu.dtNo;
        //    DataTable dtCo = _DuLieu.dtCo;

        //    fr.AddTable("dtNo", dtNo);
        //    fr.AddTable("dtCo", dtCo);
        //    fr.SetValue("TenNguoiNhan", _DuLieu.TenNguoiNhan);
        //    fr.SetValue("DiaChi", _DuLieu.DiaChi);
        //    fr.SetValue("Lydo", _DuLieu.Lydo);
        //    fr.SetValue("GhiChu", _DuLieu.GhiChu);
        //    fr.SetValue("NgayCT", _DuLieu.NgayCT);
        //    fr.SetValue("ThangCT", _DuLieu.ThangCT);
        //    fr.SetValue("SoPT", _DuLieu.SoPT);

        //}
        public class DuLieu
        {
            public string TenNguoiNhan { get; set; }
            public string DiaChi { get; set; }
            public string Lydo { get; set; }
            public string GhiChu { get; set; }
            public string NgayCT { get; set; }
            public string ThangCT { get; set; }
            public string SoPT { get; set; }
            public Decimal SoTien { get; set; }
            public DataTable dtNo { get; set; }
            public DataTable dtCo { get; set; }
        }

        public DuLieu PhieuThu(String iNamLamViec, String iThang, String iID_MaChungTuChiTiet, String iID_MaChungTu)
        {
            DuLieu _DuLieu = new DuLieu();
           // String SQL = String.Format("SELECT ISNULL(SUM(rSoTien),0) As rSoTien FROM KTTM_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet={1} AND iID_MaChungTu='{0}'", iID_MaChungTuChiTiet, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            String SQL = String.Format("SELECT ISNULL(SUM(rSoTien),0) As rSoTien FROM KTTM_ChungTuChiTiet WHERE iTrangThai=1  AND iID_MaChungTu='{0}' AND sSoChungTuChiTiet='{1}'", iID_MaChungTu, iID_MaChungTuChiTiet);
         
            _DuLieu.SoTien = Convert.ToDecimal(Connection.GetValue(SQL, 0));

            //SQL = String.Format("SELECT Top 1 sTenNguoiThuChi,sDiaChi,sNoiDung,sGhiChu,iNgayCT,iThangCT,sSoChungTuChiTiet FROM KTTM_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet={1} AND iID_MaChungTu='{0}' ORDER BY iSTT,dNgayTao", iID_MaChungTuChiTiet, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            SQL = String.Format("SELECT Top 1 sTenNguoiThuChi,sDiaChi,sNoiDung,sGhiChu,iNgayCT,iThangCT,sSoChungTuChiTiet FROM KTTM_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu='{0}'AND sSoChungTuChiTiet='{1}' ORDER BY iSTT,dNgayTao", iID_MaChungTu, iID_MaChungTuChiTiet);
          
            DataTable dt = Connection.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                _DuLieu.TenNguoiNhan = Convert.ToString(dt.Rows[0]["sTenNguoiThuChi"]);
                _DuLieu.DiaChi = Convert.ToString(dt.Rows[0]["sDiaChi"]);
                _DuLieu.Lydo = Convert.ToString(dt.Rows[0]["sNoiDung"]);
                _DuLieu.GhiChu = Convert.ToString(dt.Rows[0]["sGhiChu"]);
                _DuLieu.NgayCT = Convert.ToString(dt.Rows[0]["iNgayCT"]);
                _DuLieu.ThangCT = Convert.ToString(dt.Rows[0]["iThangCT"]);
                _DuLieu.SoPT = Convert.ToString(dt.Rows[0]["sSoChungTuChiTiet"]);
            }
            else
            {
                _DuLieu.TenNguoiNhan = "";
                _DuLieu.DiaChi = "";
                _DuLieu.Lydo = "";
                _DuLieu.GhiChu = "";
                _DuLieu.NgayCT = "";
                _DuLieu.ThangCT = "";
                _DuLieu.SoPT = "";
            }
            SQL = "SELECT  iID_MaTaiKhoan_No AS TK,iID_MaDonVi_No AS DV,SUM(rSoTien) As rST  FROM KTTM_ChungTuChiTiet";
            //SQL += " WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet={1} AND iID_MaChungTu='{0}'";
            SQL += " WHERE iTrangThai=1  AND sSoChungTuchiTiet='{1}' AND iID_MaChungTu='{0}'";
            SQL += " GROUP By sSoChungTuchiTiet,iID_MaTaiKhoan_No,iID_MaDonVi_No";
            SQL = String.Format(SQL,iID_MaChungTu,iID_MaChungTuChiTiet);
            //SQL = String.Format(SQL, iID_MaChungTuChiTiet, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            //_DuLieu.dtNo = Connection.GetDataTable(SQL);
            DataTable dtNo = Connection.GetDataTable(SQL);
            SQL = "SELECT  iID_MaTaiKhoan_Co AS TK,iID_MaDonVi_Co AS DV,SUM(rSoTien) As rST   FROM KTTM_ChungTuChiTiet";
            //SQL += " WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet={1} AND iID_MaChungTu='{0}'";
            SQL += " WHERE iTrangThai=1 AND  sSoChungTuchiTiet='{1}' AND iID_MaChungTu='{0}'";
            SQL += " GROUP By sSoChungTuchiTiet,iID_MaTaiKhoan_Co,iID_MaDonVi_Co";
            SQL = String.Format(SQL, iID_MaChungTu, iID_MaChungTuChiTiet);
            //SQL = String.Format(SQL, iID_MaChungTuChiTiet, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            //_DuLieu.dtCo = Connection.GetDataTable(SQL);
            DataTable dtCo = Connection.GetDataTable(SQL);
            Boolean CoNhieuTK_No = false;
            for (int i = 0; i < dtNo.Rows.Count; i++)
            {
                String TK, TK1;
                TK = Convert.ToString(dtNo.Rows[0]["TK"]);
                TK1 = Convert.ToString(dtNo.Rows[i]["TK"]);
                if (TK != TK1)
                {
                    CoNhieuTK_No = true;
                }
            }
            Boolean CoNhieuTK_Co = false;
            for (int i = 0; i < dtCo.Rows.Count; i++)
            {
                String TK, TK1;
                TK = Convert.ToString(dtCo.Rows[0]["TK"]);
                TK1 = Convert.ToString(dtCo.Rows[i]["TK"]);
                if (TK != TK1)
                {
                    CoNhieuTK_Co = true;
                }
            }
            if (CoNhieuTK_No == false)
            {
                dtNo.Rows[0]["rST"] = _DuLieu.SoTien;
                for (int i = 1; i < dtNo.Rows.Count; i++)
                {
                    dtNo.Rows.RemoveAt(i);
                }
            }
            else if (CoNhieuTK_Co == false)
            {
                dtCo.Rows[0]["rST"] = _DuLieu.SoTien;
                for (int i = 1; i < dtCo.Rows.Count; i++)
                {
                    dtCo.Rows.RemoveAt(i);
                }
            }
            else
            {
                dtNo.Rows[0]["rST"] = 0;
                dtNo.Rows[0]["TK"] = "";
                dtNo.Rows[0]["DV"] = "";
                for (int i = 1; i < dtNo.Rows.Count; i++)
                {
                    dtNo.Rows.RemoveAt(i);
                }
                dtCo.Rows[0]["rST"] = 0;
                dtCo.Rows[0]["TK"] = "";
                dtCo.Rows[0]["DV"] = "";
                for (int i = 1; i < dtCo.Rows.Count; i++)
                {
                    dtCo.Rows.RemoveAt(i);
                }
            }
            _DuLieu.dtCo = dtCo;
            _DuLieu.dtNo = dtNo;

            return _DuLieu;
        }

        public static DataTable Lay_SoPT(String iNamLamViec, String iThang, String iID_MaChungTu,String UserID)
        {
            String DK = "";
            String DK1 = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iThang) == false)
            {
                DK += " AND iThangCT=@iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }
            if (LuongCongViecModel.KiemTra_TroLyPhongBan(UserID))
            {
                DK1 += " AND sID_MaNguoiDungTao =@MaND";
                cmd.Parameters.AddWithValue("@MaND", UserID);
            }
            //String SQL = "SELECT Top 1 iID_MaChungTuChiTiet,sSoChungTuchiTiet,sNoiDung FROM KTTM_ChungTuChiTiet WHERE bChi='True' AND iTrangThai =1 And iID_MaChungTu=@iID_MaChungTu {0} And iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet ORDER BY iSTT, dNgayTao";
            //SQL = String.Format(SQL, DK);
            //cmd.CommandText = SQL;
            //cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            String SQL = @"SELECT  DISTINCT sSoChungTuchiTiet, 
                (SELECT Top 1 iID_MaChungTuChiTiet FROM KTTM_ChungTuChiTiet WHERE bChi='True' AND iTrangThai =1 AND sSoChungTuChiTiet = T1.sSoChungTuChiTiet And iID_MaChungTu=@iID_MaChungTu ORDER BY iSTT, dNgayTao) AS iID_MaChungTuChiTiet,
                (SELECT Top 1 sNoiDung FROM KTTM_ChungTuChiTiet WHERE bChi='True' AND iTrangThai =1 AND sSoChungTuChiTiet = T1.sSoChungTuChiTiet And iID_MaChungTu=@iID_MaChungTu ORDER BY iSTT, dNgayTao) AS  sNoiDung
                FROM KTTM_ChungTuChiTiet AS T1 WHERE bChi='True' AND iTrangThai =1 And iID_MaChungTu=@iID_MaChungTu {0} {1}";
            SQL = String.Format(SQL, DK,DK1);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);           
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        public static DataTable DanhSach_LoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof(String));
            dt.Columns.Add("TenLoai", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "PC1";
            R1[1] = "in tren giay A4";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "PC2";
            R2[1] = "in tren 1/2 giay A4";
            DataRow R3 = dt.NewRow();
            dt.Rows.Add(R3);
            R3[0] = "PC3";
            R3[1] = "in 2 phieu tren giay A4";
            dt.Dispose();
            return dt;
        }
    }
}
