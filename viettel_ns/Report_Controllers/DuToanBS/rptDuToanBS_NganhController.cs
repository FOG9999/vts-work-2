﻿using DomainModel;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Viettel.Data;
using Viettel.Extensions;
using Viettel.Services;
using VIETTEL.Controllers;
using VIETTEL.Flexcel;
using VIETTEL.Helpers;
using VIETTEL.Models;

namespace VIETTEL.Models
{
    public class rptDuToanBS_NganhViewModel
    {
        public string iNamLamViec { get; set; }
        public SelectList DotList { get; set; }
        public SelectList PhongList { get; set; }
        public SelectList NganhList { get; set; }
    }
}

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToanBS_NganhController : FlexcelReportController
    {
        #region file xls

        private const string _filePath1 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_207_Nganh_1.xls";
        private const string _filePath1_TrinhKy = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_207_Nganh_1_TrinhKy.xls";

        private const string _filePath2 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_207_Nganh_2.xls";
        private const string _filePath2_TrinhKy = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_207_Nganh_2_TrinhKy.xls";


        private const string _filePath1_nsbd = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_1040100_Nganh_1.xls";
        private const string _filePath1_nsbd_TrinhKy = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_1040100_Nganh_1_TrinhKy.xls";

        private const string _filePath2_nsbd = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_1040100_Nganh_2.xls";
        private const string _filePath2_nsbd_TrinhKy = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_1040100_Nganh_2_TrinhKy.xls";

        #endregion

        #region ctor


        private readonly INganSachService _nganSachService = NganSachService.Default;
        private readonly IDuToanBsService _bsService = DuToanBsService.Default;

        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {

            var namLamViec = PhienLamViec.iNamLamViec;

            //var dotList = DuToanBsService.Default.GetDots(namLamViec, Username, "207%")
            //.ToSelectList("iID_MaDot", "iID_MaDot", "-1", "-- Chọn đợt --");


            //var dotList = CacheService.Default.CachePerRequest(this.ControllerName() + "_Dot",
            //                        () => DuToanBS_ReportModels.LayDSDot(namLamViec, Username, "207"),
            //                        Viettel.Domain.DomainModel.CacheTimes.OneMinute)
            //    .ToSelectList("iID_MaDot", "iID_MaDot", "-1", "-- Chọn đợt --");


            var phongBanList = _nganSachService
                .GetPhongBans()
                .Select(x => new
                {
                    value = x.sKyHieu,
                    text = x.sKyHieu + " - " + x.sMoTa,
                })
                .ToSelectList();

            var vm = new rptDuToanBS_NganhViewModel
            {
                iNamLamViec = namLamViec,
                //DotList = dotList,
                PhongList = phongBanList,
            };

            var _viewPath = "~/Views/Report_Views/DuToanBS/";
            var view = _viewPath + this.ControllerName() + ".cshtml";

            return View(view, vm);
        }

        #endregion


        public Microsoft.AspNetCore.Mvc.ActionResult Print(
            string iID_MaNganh,
            string toSo,
            string sLNS,
            string iID_MaPhongBan,
            string iID_MaDot,
            string ext = "pdf")
        {
            var filePath = "";
            if (string.IsNullOrWhiteSpace(iID_MaPhongBan) || iID_MaPhongBan == "-1")
            {
                filePath = toSo == "1" ? _filePath1 : _filePath2;
            }
            else
            {
                filePath = toSo == "1" ? _filePath1_TrinhKy : _filePath2_TrinhKy;
            }

            HamChung.Language();
            var xls = createReport(Server.MapPath(filePath), iID_MaNganh, toSo, sLNS, iID_MaPhongBan, iID_MaDot);
            var filename = string.Format("{0}_{1}.{2}", this.ControllerName(), DateTime.Now.GetTimeStamp(), ext);

            return Print(xls, ext);
        }

        #region public methods

        public JsonResult Ds_Nganh(string Nganh, string iID_MaDot)
        {
            var data = DuToanBS_ReportModels.dtNganh_NhaNuoc(iID_MaDot, User.Identity.Name);
            var vm = new ChecklistModel("Nganh", data.ToSelectList("iID_MaNganh", "sTenNganh"));
            return ToDropdownList(vm);
        }
        //public JsonResult Ds_PhongBan(string iID_MaNganh, string iID_MaDot, string iID_MaPhongBan)
        //{
        //    iID_MaNganh = Convert.ToString(CommonFunction.LayTruong("NS_MucLucNganSach_Nganh", "iID_MaNganh", iID_MaNganh, "iID"));
        //    var data = DuToan_ReportModels.dtPhongBanInBaoDamBS(iID_MaDot, iID_MaNganh, Username);

        //    var vm = new ChecklistModel("Nganh", data.ToSelectList("iID_MaPhongBan", "iID_MaPhongBan", "-1"));
        //    return ToDropdownList(vm);
        //}

        public JsonResult Ds_ToIn(
            string iID_MaNganh,
            string toSo,
            string sLNS,
            string iID_MaPhongBan,
            string iID_MaDot)
        {
            try
            {
                var data = getData(iID_MaNganh, toSo, "207", iID_MaPhongBan, iID_MaDot);
                return ds_ToIn(data.ColumnsCount);
            }
            catch (Exception ex)
            {
                //throw;
            }

            return ds_ToIn(0);
        }

        #endregion

        #region private methods

        private ExcelFile createReport(String path, String Nganh, String ToSo, String sLNS, String iID_MaPhongBan, String iID_MaDot)
        {
            var xls = new XlsFile(true);
            xls.Open(path);
            FlexCelReport fr = new FlexCelReport();

            var reportVM = getData(Nganh, ToSo, sLNS, iID_MaPhongBan, iID_MaDot);
            DataTable data = reportVM.dtDuLieu;
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();


            #region mota

            int i = 1;
            foreach (object obj in reportVM.arrMoTa1)
            {
                fr.SetValue("MoTa1_" + i, obj);
                i++;
            }
            i = 1;
            foreach (object obj in reportVM.arrMoTa2)
            {
                fr.SetValue("MoTa2_" + i, obj);
                i++;
            }
            i = 1;
            foreach (object obj in reportVM.arrMoTa3)
            {
                fr.SetValue("MoTa3_" + i, obj);
                i++;
            }

            #endregion

            string sTenPhongBan, sTenDonVi;

            string sql = String.Format(@"SELECT * FROM NS_MucLucNganSach_Nganh WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 AND iID_MaNganh = '" + Nganh + "'");
            SqlCommand cmdNganh = new SqlCommand(sql);
            cmdNganh.Parameters.AddWithValue("@iNamLamViec", PhienLamViec.iNamLamViec);
            DataTable dtNganhChon = Connection.GetDataTable(cmdNganh);

            sTenDonVi = Convert.ToString(dtNganhChon.Rows[0]["sTenNganh"]);
            sTenPhongBan = _nganSachService.GetPhongBanById(iID_MaPhongBan).sMoTa;

            fr.SetValue("sTenPhongBan", sTenPhongBan);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("ToSo", ToSo);
            fr.SetValue("DotNgay", iID_MaDot.ToParamDate().ToStringNgay().ToLower());

            fr.UseCommonValue()
                .UseChuKy(Username, iID_MaPhongBan)
                .UseChuKyForController(this.ControllerName())
                .UseForm(this)
                .Run(xls);

            return xls;
        }


        private ReportDataModel getData(String Nganh, String ToSo, String sLNS, String iID_MaPhongBan, String iID_MaDot)
        {
            DataTable dtNG, dt;

            #region params

            string
                iNamLamViec,
                iID_MaNamNganSach,
                iID_MaNguonNganSach,
                sNG,
                iID_MaChungTu;

            //var config = NguoiDungCauHinhModels.LayCauHinh(Username).Rows[0];
            iNamLamViec = PhienLamViec.iNamLamViec;
            iID_MaNamNganSach = PhienLamViec.iID_MaNamNganSach.ToString();
            iID_MaNguonNganSach = PhienLamViec.iID_MaNguonNganSach.ToString();

            string sqlNganh = String.Format(@"SELECT * FROM NS_MucLucNganSach_Nganh WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 AND iID_MaNganh = '" + Nganh + "'");
            SqlCommand cmdNganh = new SqlCommand(sqlNganh);
            cmdNganh.Parameters.AddWithValue("@iNamLamViec", PhienLamViec.iNamLamViec);
            DataTable dtNganhChon = Connection.GetDataTable(cmdNganh);

            sNG = Convert.ToString(dtNganhChon.Rows[0]["iID_MaNganhMLNS"]);

            #endregion

            #region sql - nganh

            var sql = FileHelpers.GetSqlQuery("dtbs_nganh_nganh.sql");
            sql = sql.Replace("@sLNS", sLNS.ToParamLikeStartWith("sLNS"));

            using (var conn = ConnectionFactory.Default.GetConnection())
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                    cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                    cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                    cmd.Parameters.AddWithValue("@sNG", sNG);

                    dtNG = cmd.GetTable();
                }
            }

            #endregion

            #region sql - du lieu

            sql = FileHelpers.GetSqlQuery("dtbs_nganh.sql");
            sql = sql.Replace("@sLNS", sLNS.ToParamLikeStartWith("sLNS"));

            var sLNS_pc = string.Empty;
            if (sLNS.Contains("104") || sLNS.Contains("109"))
                sLNS_pc = "1020100";
            else
                sLNS_pc = sLNS;

            sql = sql.Replace("@sLNS_pc", sLNS_pc.ToParamLikeStartWith("sLNS"));

            iID_MaChungTu = DuToanBS_ReportModels.GetChungTuList_TheoDot(iID_MaDot, Username);
            using (var conn = ConnectionFactory.Default.GetConnection())
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                    cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                    cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    cmd.Parameters.AddWithValue("@sNG", sNG);
                    cmd.Parameters.AddWithValue("@dvt", 1000);

                    dt = cmd.GetTable();
                }
            }

            #endregion

            return getReportData(dtNG, dt, ToSo);
        }

        private ReportDataModel getReportData(DataTable dtNG, DataTable dt, string ToSo)
        {
            var dtDonVi = HamChung.SelectDistinct("dtDonVi", dt, "iID_MaDonVi", "iID_MaDonVi,TenDonVi");

            var i = 0;

            //cs = 3;//tờ 1 4 cột
            dtDonVi.Columns.Add("TongTuChi", typeof(Decimal));
            dtDonVi.Columns.Add("TongHienVat", typeof(Decimal));
            while (i < dtNG.Rows.Count)
            {
                if (dtDonVi.Columns.IndexOf(dtNG.Rows[i]["NG"].ToString() + "_TuChi") < 0)
                    dtDonVi.Columns.Add(dtNG.Rows[i]["NG"].ToString() + "_TuChi", typeof(Decimal));
                if (dtDonVi.Columns.IndexOf(dtNG.Rows[i]["NG"].ToString() + "_HienVat") < 0)
                    dtDonVi.Columns.Add(dtNG.Rows[i]["NG"].ToString() + "_HienVat", typeof(Decimal));
                i = i + 1;
            }

            i = 0;
            String MaDonVi, MaDonVi1, TenCot;
            for (i = 0; i < dtDonVi.Rows.Count; i++)
            {
                MaDonVi = Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]).Trim();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    MaDonVi1 = Convert.ToString(dt.Rows[j]["iID_MaDonVi"]).Trim();
                    TenCot = Convert.ToString(dt.Rows[j]["NG"]).Trim();
                    if (MaDonVi == MaDonVi1 && dtDonVi.Columns.IndexOf(TenCot + "_TuChi") >= 0)
                    {
                        dtDonVi.Rows[i][TenCot + "_TuChi"] = dt.Rows[j]["rTuChi"];
                        dtDonVi.Rows[i][TenCot + "_HienVat"] = dt.Rows[j]["rHienVat"];
                        dt.Rows.RemoveAt(j);
                        j = j - 1;
                    }
                }
            }
            i = 0;
            //j=4 vì trừ cột madv, đơn vị và 2 cột tổng cộng
            Double Tong = 0;
            for (int j = 4; j < dtDonVi.Columns.Count; j++)
            {
                Tong = 0;
                for (i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    if (dtDonVi.Rows[i][j] != DBNull.Value)
                    {
                        Tong = Tong + Convert.ToDouble(dtDonVi.Rows[i][j]);
                    }
                }
                if (Tong == 0)
                {
                    dtDonVi.Columns.RemoveAt(j);
                    if (j == 1) j = 1;
                    else j = j - 1;
                }
            }
            Double TongHienVat = 0, TongTuChi = 0;
            for (i = 0; i < dtDonVi.Rows.Count; i++)
            {
                TongHienVat = 0; TongTuChi = 0;
                //j=4 vì trừ cột MaDV, đơn vị và 2 cột tổng cộng
                for (int j = 4; j < dtDonVi.Columns.Count; j++)
                {
                    if (dtDonVi.Rows[i][j] != DBNull.Value)
                    {
                        if (dtDonVi.Columns[j].ColumnName.IndexOf("_HienVat") >= 0)
                        {
                            TongHienVat = TongHienVat + Convert.ToDouble(dtDonVi.Rows[i][j]);
                        }
                        else
                        {
                            TongTuChi = TongTuChi + Convert.ToDouble(dtDonVi.Rows[i][j]);
                        }
                    }
                }
                dtDonVi.Rows[i]["iID_MaDonVi"] = (i + 1).ToString();
                dtDonVi.Rows[i]["TongHienVat"] = TongHienVat;
                dtDonVi.Rows[i]["TongTuChi"] = TongTuChi;
            }
            DataTable _dtDonVi = new DataTable();
            DataTable _dtDonVi1 = new DataTable();

            int TongSoCot = 0;
            int SoTrang = 1;
            int SoCotCanThem = 0;
            if ((dtDonVi.Columns.Count - 4) == 0)
            {
                SoCotCanThem = 4;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
            }
            else if ((dtDonVi.Columns.Count - 4) <= 4)
            {

                int SoCotDu = ((dtDonVi.Columns.Count - 4)) % 4;
                if (SoCotDu != 0)
                    SoCotCanThem = 4 - SoCotDu;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (dtDonVi.Columns.Count - 4 - 4) % 6;
                if (SoCotDu != 0)
                    SoCotCanThem = 6 - SoCotDu;
                TongSoCot = (dtDonVi.Columns.Count - 4) + SoCotCanThem;
                SoTrang = 1 + (TongSoCot - 4) / 6;
            }
            for (i = 0; i < SoCotCanThem; i++)
            {
                dtDonVi.Columns.Add();
            }
            int _ToSo = Convert.ToInt16(ToSo);
            int SoCotTrang1 = 4;
            int SoCotTrangLonHon1 = 6;
            _dtDonVi = dtDonVi.Copy();
            int _CS = 0;
            String BangTien_HienVat = "";
            //Mổ tả xâu nối mã
            ArrayList arrMoTa1 = new ArrayList();
            //Mỏ tả ngành
            ArrayList arrMoTa2 = new ArrayList();
            //Bằng Tiền hay bằng hiện vật
            ArrayList arrMoTa3 = new ArrayList();
            if (ToSo == "1")
            {

                for (i = 4; i < 4 + SoCotTrang1; i++)
                {
                    TenCot = _dtDonVi.Columns[i].ColumnName;
                    _CS = TenCot.IndexOf("_");
                    //Thêm dữ liệu arrMota1 va 2
                    if (_CS == -1)
                    {
                        arrMoTa1.Add("");
                        arrMoTa2.Add("");
                    }
                    else
                    {
                        arrMoTa1.Add(Convert.ToString(TenCot.Substring(0, _CS)));
                        DataRow[] R = dtNG.Select("NG='" + TenCot.Substring(0, _CS) + "'");
                        arrMoTa2.Add(Convert.ToString(R[0]["sMoTa"]));
                    }
                    //Thêm dữ liệu arrmota 3
                    if (TenCot.IndexOf("_TuChi") >= 0) BangTien_HienVat = "Bằng tiền";
                    else if (TenCot.IndexOf("_HienVat") >= 0) BangTien_HienVat = "Bằng hiện vật";
                    else BangTien_HienVat = "";
                    arrMoTa3.Add(BangTien_HienVat);

                    //Đổi tên cột
                    _dtDonVi.Columns[i].ColumnName = "Cot" + (i - 3);
                }
            }
            else
            {
                int tg = 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 2);
                int dem = 1;
                for (i = 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 2); i < 4 + SoCotTrang1 + SoCotTrangLonHon1 * (_ToSo - 1); i++)
                {
                    if (i < 0)
                        break;
                    TenCot = _dtDonVi.Columns[i].ColumnName;
                    _CS = TenCot.IndexOf("_");
                    //Thêm dữ liệu arrMota1 va 2
                    if (_CS == -1)
                    {
                        arrMoTa1.Add("");
                        arrMoTa2.Add("");
                    }
                    else
                    {
                        arrMoTa1.Add(Convert.ToString(TenCot.Substring(0, _CS)));
                        DataRow[] R = dtNG.Select("NG='" + TenCot.Substring(0, _CS) + "'");
                        arrMoTa2.Add(Convert.ToString(R[0]["sMoTa"]));
                    }
                    //Thêm dữ liệu arrmota 3
                    if (TenCot.IndexOf("_TuChi") >= 0) BangTien_HienVat = "Bằng tiền";
                    else if (TenCot.IndexOf("_HienVat") >= 0) BangTien_HienVat = "Bằng hiện vật";
                    else BangTien_HienVat = "";
                    arrMoTa3.Add(BangTien_HienVat);

                    //Đổi tên cột
                    _dtDonVi.Columns[i].ColumnName = "Cot" + dem;
                    dem++;
                }
            }

            var vm = new ReportDataModel();
            vm.dtDuLieu = _dtDonVi;
            vm.arrMoTa1 = arrMoTa1;
            vm.arrMoTa2 = arrMoTa2;
            vm.arrMoTa3 = arrMoTa3;
            vm.dtDuLieuAll = dtDonVi;
            vm.ColumnsCount = TongSoCot + 2;
            return vm;
        }

        #endregion

    }
}
