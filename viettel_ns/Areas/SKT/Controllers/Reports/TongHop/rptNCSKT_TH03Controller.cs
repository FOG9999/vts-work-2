﻿using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Viettel.Data;
using Viettel.Services;
using VIETTEL.Areas.SKT.Models;
using VIETTEL.Controllers;
using VIETTEL.Flexcel;
using VIETTEL.Models;

namespace VIETTEL.Areas.SKT.Controllers
{
    public class rptNCSKT_TH03ViewModel
    {
        public SelectList NganhList { get; set; }
        public SelectList PhongBanList { get; set; }
        public SelectList DonViList { get; set; }
    }
    public class rptNCSKT_TH03Controller : FlexcelReportController
    {
        public string _viewPath = "~/Areas/SKT/Views/Reports/TongHop/";
        private const string _filePath = "~/Areas/SKT/FlexcelForm/TongHop/rptNCSKT_TH03/rptNCSKT_TH03.xls";
        private const string _filePath_dv = "~/Areas/SKT/FlexcelForm/TongHop/rptNCSKT_TH03/rptNCSKT_TH03_dv.xls";

        private readonly IConnectionFactory _connectionFactory = ConnectionFactory.Default;
        private readonly ISKTService _SKTService = SKTService.Default;
        private readonly INganSachService _nganSachService = NganSachService.Default;

        private int _dvt;
        private int _loai;
        private int _loaiBC;
        private string _nganh;
        private string _donvi;
        private string _phongban;

        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            if (User.Identity.Name.EndsWith("b2"))
            {
                var dNganh = _nganSachService.ChuyenNganh_GetAll(PhienLamViec.iNamLamViec);
                var phongbanList = (PhienLamViec.iID_MaPhongBan != "02" && PhienLamViec.iID_MaPhongBan != "11") ? new SelectList(_nganSachService.GetBql(Username), "sKyHieu", "sMoTa") : _nganSachService.GetPhongBanQuanLyNS("06,07,08,10,17").ToSelectList("iID_MaPhongBan", "sMoTa", "-1", "-- Chọn phòng ban --"); ;
                var donviList = _nganSachService.GetDonviListByUser(Username, PhienLamViec.iNamLamViec);

                var vm = new rptNCSKT_TH03ViewModel()
                {
                    PhongBanList = phongbanList,
                    NganhList = dNganh.ToSelectList("Id","Nganh", "-1","-- Chọn chuyên ngành --"),
                    DonViList = donviList.ToSelectList("iID_MaDonVi", "sMoTa", "-1", "-- Chọn đơn vị --"),
                };

                var view = _viewPath + "rptNCSKT_TH03.cshtml";

                return View(view, vm);

            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        public JsonResult Ds_DonVi(string phongban)
        {
            var data = _SKTService.Get_DonVi_ExistData(PhienLamViec.iNamLamViec, 1, PhienLamViec.iID_MaDonVi, phongban == "-1" ? phongban : PhienLamViec.iID_MaPhongBan, phongban);
            var vm = new ChecklistModel("Id_DonVi", data.ToSelectList("Id", "Ten", "-1", "-- Chọn đơn vị --"));

            return ToDropdownList(vm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_phongban"></param>
        /// <param name="id_donvi"></param>
        /// <param name="loai">1: NSSD, 2: NSBĐ ngành</param>
        /// <param name="ext"></param>
        /// <param name="dvt"></param>
        /// <returns></returns>
        public Microsoft.AspNetCore.Mvc.ActionResult Print(
            string nganh,
            string donvi,
            string phongban,
            int loai = 1,
            int loaiBC = 1, 
            string ext = "pdf",
            int dvt = 1000)
        {
            // chi cho phep in donvi duoc quan ly           
            this._dvt = dvt;
            this._loai = loai;
            this._loaiBC = loaiBC;
            this._nganh = nganh;
            this._donvi = donvi;
            this._phongban = phongban;
            var xls = createReport();           

            var filename = $"Báo_cáo_K5_{DateTime.Now.GetTimeStamp()}.{ext}";
            return Print(xls, ext, filename);
        }
        
        #region private methods

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <returns></returns>
        private ExcelFile createReport()
        {
            var fr = new FlexCelReport();
            loadData(fr);

            var xls = new XlsFile(true);
            xls.Open(Server.MapPath(_loai == 2 ? _filePath : _filePath_dv));
            
            fr.SetValue(new
            {
                headerr = $"Đơn vị tính: {_dvt.ToHeaderMoney()}",
                nam = PhienLamViec.NamLamViec - 1,
                namn = PhienLamViec.NamLamViec,
                phongban = "Tài liệu báo cáo Bộ",
            });

            fr.UseCommonValue()
              .UseChuKyForController(this.ControllerName())
              .UseForm(this).Run(xls);
            return xls;
        }

        private void loadData(FlexCelReport fr)
        {
            var DonVi = getDataSet().Tables[0];
            var Muc = DonVi.SelectDistinct("ChiTiet", "X1,X2,X3,X4,KyHieu,MoTa");          
            var PhongBan = DonVi.SelectDistinct("PhongBan", "X1,X2,X3,X4,KyHieu,Id_PhongBan,TenPhongBan");          
            _SKTService.FillDataTable_NC(fr, Muc);
            fr.AddTable("Muc", DonVi);
            fr.AddTable("PhongBan", PhongBan);
            fr.AddRelationship("ChiTiet", "PhongBan", "X1,X2,X3,X4,KyHieu".Split(','), "X1,X2,X3,X4,KyHieu".Split(','));
            fr.AddRelationship("PhongBan", "Muc", "X1,X2,X3,X4,KyHieu,Id_PhongBan".Split(','), "X1,X2,X3,X4,KyHieu,Id_PhongBan".Split(','));

            var DoanhNghiep = getDataSet().Tables[1];
            fr.AddTable("DN", DoanhNghiep);
        }

        private DataSet getDataSet()
        {
            using (var conn = _connectionFactory.GetConnection())
            { 
                var cmd = new SqlCommand("sp_ncskt_report_th03", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                return cmd.GetDataset(new
                {
                    nganh = _nganh.ToParamString(),
                    phongban = _phongban.ToParamString(),
                    donvi = _donvi.ToParamString(),
                    nam = PhienLamViec.NamLamViec,
                    loai = _loaiBC.ToParamString(),
                    dvt = _dvt.ToParamString(),
                });
            }
        }        
        #endregion
    }
}
