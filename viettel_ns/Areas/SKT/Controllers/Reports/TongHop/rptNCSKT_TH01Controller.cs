﻿using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Viettel.Data;
using Viettel.Services;
using VIETTEL.Areas.SKT.Models;
using VIETTEL.Controllers;
using VIETTEL.Flexcel;
using VIETTEL.Models;

namespace VIETTEL.Areas.SKT.Controllers
{
    public class rptNCSKT_TH01ViewModel
    {
        public SelectList PhongBanList { get; set; }
        public SelectList LoaiList { get; set; }
    }
    public class rptNCSKT_TH01Controller : FlexcelReportController
    {
        public string _viewPath = "~/Areas/SKT/Views/Reports/TongHop/";
        private string _filePath = "~/Areas/SKT/FlexcelForm/TongHop/rptNCSKT_TH01/rptNCSKT_TH01.xls";
        private string _filePath_b2 = "~/Areas/SKT/FlexcelForm/TongHop/rptNCSKT_TH01/rptNCSKT_TH01_b2.xls";

        private readonly IConnectionFactory _connectionFactory = ConnectionFactory.Default;
        private readonly ISKTService _SKTService = SKTService.Default;
        private readonly INganSachService _nganSachService = NganSachService.Default;

        private string _id_phongban;
        private string _id_donvi;
        private string _loaiBC;
        private int _loai;
        private int _dvt;

        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            var phongbanList = _nganSachService.GetPhongBansQuanLyNS(GetPhongBanId(PhienLamViec.iID_MaPhongBan));
            var loaiList = SharedModels.GetLoaiHinhSKT().Select("Loai in ('1','4')").CopyToDataTable();

            var vm = new rptNCSKT_TH01ViewModel()
            {
                PhongBanList = phongbanList.ToSelectList("sKyHieu", "sMoTa"),
                LoaiList = loaiList.ToSelectList("Loai", "Ten"),
            };

            var view = _viewPath + "rptNCSKT_TH01.cshtml";

            return View(view, vm);
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
            string id_phongban,
            string id_donvi,
            string ext = "pdf",
            string loaiBC = "chitiet",
            int loai = 1,
            int dvt = 1000)
        {           
            this._id_phongban = id_phongban;
            this._id_donvi = id_donvi.IsEmpty() ? PhienLamViec.iID_MaDonVi : id_donvi;
            this._loai = loai;
            this._loaiBC = loaiBC;
            this._dvt = dvt;

            var xls = createReport();

            if (xls.ContainsKey(false))
            {
                return new EmptyResult();
            }
            else
            {

                var donvi = id_donvi.IsEmpty() || id_donvi.Contains(",")
                  ? string.Empty
                  : _nganSachService.GetDonVi(PhienLamViec.iNamLamViec, id_donvi).sTen;

                var filename = $"{(donvi.IsEmpty() ? id_phongban : id_donvi + "-" + donvi.ToStringAccent().Replace(" ", ""))}_TH01_{DateTime.Now.GetTimeStamp()}.{ext}";
                return Print(xls[true], ext, filename);
            }
        }

        #region public
        public JsonResult Ds_DonVi(string id_phongban)
        {
            var data = _SKTService.Get_DonVi_ExistData(PhienLamViec.iNamLamViec, 4, PhienLamViec.iID_MaDonVi, PhienLamViec.iID_MaPhongBan, id_phongban);
            var vm = new ChecklistModel("Id_DonVi", data.ToSelectList("Id", "Ten"));
            return ToCheckboxList(vm);
        }
        #endregion

        #region private methods

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <returns></returns>
        private Dictionary<bool, ExcelFile> createReport()
        {
            var fr = new FlexCelReport();
            var xls = new XlsFile(true);
            var check = loadData(fr);

            if (check.ContainsKey(false))
            {
                return new Dictionary<bool, ExcelFile>
                {
                    {false, xls}
                };
            }
            else
            {
                var path = (PhienLamViec.iID_MaPhongBan != "02" && PhienLamViec.iID_MaPhongBan != "11") ? _filePath : _filePath_b2; 
                xls.Open(Server.MapPath(path));
                                
                var donvi = _id_donvi.IsEmpty() || _id_donvi.Contains(",")
                    ? string.Empty
                    : _nganSachService.GetDonVi(PhienLamViec.iNamLamViec, _id_donvi).sTen;

                fr.SetValue(new
                {
                    headerl = donvi.IsEmpty() ? "Tổng hợp đơn vị" : $"Đơn vị: {_nganSachService.GetDonVi(PhienLamViec.iNamLamViec, _id_donvi).sTen}",
                    headerr = $"Đơn vị tính: {_dvt.ToHeaderMoney()}",
                    nam = int.Parse(PhienLamViec.iNamLamViec),
                    TenPhongBan = _id_phongban == "-1" ? "" : _nganSachService.GetPhongBanById(_id_phongban).sMoTa,
                    PL = _loai == 1 ? "KT-ĐV" : "KT-ĐV-TH",
                    date = DateTime.Now.ToStringNgay(),
                });

                fr.UseCommonValue()
                  .UseChuKyForController(this.ControllerName())
                  .UseForm(this).Run(xls);
                
                return new Dictionary<bool, ExcelFile>
                {
                    {true, xls}
                };
            }
        }

        private Dictionary<bool, FlexCelReport> loadData(FlexCelReport fr)
        {
            var data = getDataSet();
            //var loai = "2";
            if (data.Tables[1].Rows.Count == 0)
            {
                return new Dictionary<bool, FlexCelReport>
                {
                    { false, fr }
                };
            }
            else
            {
                fr.SetValue(new
                {
                    Cot1 = data.Tables[0].Rows[0]["N_1"],
                    Cot2 = data.Tables[0].Rows[0]["N_2"],
                    Cot3 = data.Tables[0].Rows[0]["N_3"],
                });
                
                if (PhienLamViec.iID_MaPhongBan != "02" && PhienLamViec.iID_MaPhongBan != "11") { 
                    var dt = data.Tables[1];
                    fr.AddTable("DonVi", dt);
                    fr.AddRelationship("ChiTiet", "DonVi", "X1,X2,X3,X4,KyHieu".Split(','), "X1,X2,X3,X4,KyHieu".Split(','));
                    _SKTService.FillDataTable_NC(fr, dt.SelectDistinct("ChiTiet", "X1,X2,X3,X4,KyHieu"));
                }
                else
                {
                    var dt = data.Tables[1];
                    _SKTService.FillDataTable_NC(fr, dt);
                }

                //if (_loaiBC == "chitiet")
                //{
                //    var ghiChu = getTable_GhiChu();
                //    var dtNganhGhiChu = ghiChu.SelectDistinct("Nganh", "Nganh,TenNganh");
                //    fr.AddTable("dtGhiChu", ghiChu);
                //    fr.AddTable("Nganh", dtNganhGhiChu);
                //    fr.AddRelationship("Nganh", "dtGhiChu", "Nganh".Split(','), "Nganh".Split(','));
                //    if (ghiChu.Rows.Count > 0)
                //        loai = "1";
                //}
                //else
                //{
                //    var ghiChu = getTable_GhiChu().Clone();
                //    var dtNganhGhiChu = ghiChu.SelectDistinct("Nganh", "Nganh,TenNganh");
                //    fr.AddTable("dtGhiChu", ghiChu);
                //    fr.AddTable("Nganh", dtNganhGhiChu);
                //    fr.AddRelationship("Nganh", "dtGhiChu", "Nganh".Split(','), "Nganh".Split(','));
                //}

                //fr.SetValue("Loai", loai);
                return new Dictionary<bool, FlexCelReport>
                {
                    { true, fr }
                };
            }
        }

        private DataSet getDataSet()
        {
            var sql = "";
            if (PhienLamViec.iID_MaPhongBan != "02" && PhienLamViec.iID_MaPhongBan != "11")
            {                
                sql = "sp_ncskt_report_th01";               
            }
            else
            {
                sql = "sp_ncskt_report_th01_b2";
            }
            using (var conn = _connectionFactory.GetConnection())
            {
                var cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (sql == "sp_ncskt_report_th01") { 
                    return cmd.GetDataset(new
                    {
                        nam = PhienLamViec.NamLamViec,
                        dvt = _dvt.ToParamString(),
                        donvi = _id_donvi.ToParamString(),
                        phongban = _id_phongban.ToParamString(),
                        loai = _loaiBC == "tonghop-bv" ? 1 : 2,
                        loaiDL = _loai,
                    });
                }
                else
                {
                    return cmd.GetDataset(new
                    {
                        nam = PhienLamViec.NamLamViec,
                        dvt = _dvt.ToParamString(),
                        donvi = _id_donvi.ToParamString(),
                        phongban = _id_phongban.ToParamString(),
                        loaiDL = _loai,
                    });
                }
            }
        }

        private DataTable getTable_GhiChu()
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                var dt = conn.GetTable("sp_ncskt_report_ghichu", new
                {
                    nam = PhienLamViec.iNamLamViec,
                    phongban = PhienLamViec.iID_MaPhongBan,
                    phongbandich = _id_phongban.ToParamString(),
                    donvi = _id_donvi.ToParamString(),
                    loai = 1,
                }, CommandType.StoredProcedure);
                return dt;
            }
        }
        #endregion
    }
}
