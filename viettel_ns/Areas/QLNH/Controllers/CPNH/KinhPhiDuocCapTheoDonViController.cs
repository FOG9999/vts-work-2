using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIETTEL.Controllers;
using Viettel.Domain.DomainModel;
using Viettel.Models.CPNH;
using Viettel.Services;
using DapperExtensions;
using VIETTEL.Helpers;
using System.Data;
using FlexCel.Core;
using FlexCel.Report;
using VIETTEL.Flexcel;
using FlexCel.XlsAdapter;
using System.Text;
using System.Globalization;
using DomainModel;

namespace VIETTEL.Areas.QLNH.Controllers.CPNH
{
    public class KinhPhiDuocCapTheoDonViController : FlexcelReportController
    {
        private readonly ICPNHService _cpnhService = CPNHService.Default;
        private readonly IQLNguonNganSachService _nnsService = QLNguonNganSachService.Default;
        private const string sFilePathBaoCao1 = "/Report_ExcelFrom/QLNH/rpt_KinhPhiDuocCapTheoDonVi.xlsx";
        private int _columnCountBC1 = 7;
        private const string sControlName = "KinhPhiDuocCapTheoDonVi";

        // GET: QLVonDauTu/QLDMTyGia
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            CPNHThucHienNganSach_ModelPaging vm = new CPNHThucHienNganSach_ModelPaging();

            List<NS_DonVi> lstDonViQuanLy = _cpnhService.GetDonviListByYear(PhienLamViec.NamLamViec).ToList();
            ViewBag.ListDonViQuanLy = lstDonViQuanLy.ToSelectList("iID_Ma", "sMoTa");

            List<CPNHThucHienNganSach_Model> list = _cpnhService.getListKinhPhiDuocCapTheoDonViModels(null, null, lstDonViQuanLy[0].iID_Ma).ToList();
            List<CPNHThucHienNganSach_Model> listData = getList(list);
            vm.Items = listData;


            return View(vm);
        }

        [HttpPost]
        public Microsoft.AspNetCore.Mvc.ActionResult KinhPhiDuocCapTheoDonViSearch(DateTime? dTuNgay, DateTime? dDenNgay, Guid? iDonvi)
        {
            CPNHThucHienNganSach_ModelPaging vm = new CPNHThucHienNganSach_ModelPaging();
            List<CPNHThucHienNganSach_Model> list = _cpnhService.getListKinhPhiDuocCapTheoDonViModels(dTuNgay, dDenNgay, iDonvi).ToList();
            List<CPNHThucHienNganSach_Model> listData = getList(list);
            vm.Items = listData;

            List<NS_DonVi> lstDonViQuanLy = _cpnhService.GetDonviListByYear(PhienLamViec.NamLamViec).ToList();
            ViewBag.ListDonViQuanLy = lstDonViQuanLy.ToSelectList("iID_Ma", "sMoTa");

            return PartialView("_list", vm);
        }

        public Microsoft.AspNetCore.Mvc.ActionResult ExportExcelBaoCao(string ext = "xls", int dvt = 1, int to = 1, DateTime? dTuNgay = null, DateTime? dDenNgay = null, Guid? iDonvi = null)
        {
            if (Request.QueryString["dTuNgay"].ToString() + "" == "")
            {
                dTuNgay = null;
            }
            else
            {
                dTuNgay = Convert.ToDateTime(Request.QueryString["dTuNgay"]);
            }
            if (Request.QueryString["dDenNgay"].ToString() + "" == "")
            {
                dDenNgay = null;
            }
            else
            {
                dDenNgay = Convert.ToDateTime(Request.QueryString["dDenNgay"]);
            }
            NS_DonVi lstDonViQuanLy = _cpnhService.GetDonviListByYear(PhienLamViec.NamLamViec).ToList().Where(x => x.iID_Ma == iDonvi).FirstOrDefault();
            var DonVi = lstDonViQuanLy != null ? lstDonViQuanLy.sTen + " - " + lstDonViQuanLy.sMoTa : "";
            string fileName = string.Format("{0}.{1}", "BaoCaoKinhPhiDuocCapTheoDonVi", ext);
            List<CPNHThucHienNganSach_Model> list = _cpnhService.getListKinhPhiDuocCapTheoDonViModels(dTuNgay, dDenNgay, iDonvi).ToList();
            ExcelFile xls = null;
            xls = TaoFileBaoCao1(dvt, to, list, dTuNgay, dDenNgay, DonVi);
            return Print(xls, ext, fileName);
        }

        public ExcelFile TaoFileBaoCao1(int dvt = 1, int to = 1 , List<CPNHThucHienNganSach_Model> list = null , DateTime? dTuNgay = null, DateTime? dDenNgay = null, string DonVi = null)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePathBaoCao1));
            FlexCelReport fr = new FlexCelReport();
            var TuNgay = dTuNgay != null ? dTuNgay.Value.ToString("dd/MM/yyyy") : "";
            var DenNgay = dTuNgay != null ? dDenNgay.Value.ToString("dd/MM/yyyy") : "";
            int columnStart = _columnCountBC1 * (to - 1);
            List<CPNHThucHienNganSach_Model> listData = getList(list);
            fr.AddTable<CPNHThucHienNganSach_Model>("dt", listData);
            fr.SetValue(new
            {
                dvt = dvt.ToStringDvt(),
                To = to,
                iQuy = 0,
                iNam = 0,
                DonVi = DonVi,
                TuNgay = TuNgay,
                DenNgay = DenNgay
            });
            fr.UseChuKy(Username)
                .UseChuKyForController(sControlName)
                .UseForm(this).Run(Result);


            return Result;
        }
        private string convertLetter(int input)
        {
            StringBuilder res = new StringBuilder((input - 1).ToString());
            for (int j = 0; j < res.Length; j++)
                res[j] += (char)(17); // '0' is 48, 'A' is 65
            return res.ToString();
        }
        private List<CPNHThucHienNganSach_Model> getList(List<CPNHThucHienNganSach_Model> list)
        {
            List<CPNHThucHienNganSach_Model> listData = new List<CPNHThucHienNganSach_Model>().ToList();
            int SttLoai = 0;
            int SttHopDong = 0;
            int SttDuAn = 0;
            int SttChuongTrinh = 0;
            Guid? idDuAn = null;
            Guid? idHopDong = null;
            Guid? idChuongTrinh = null;
            int? idLoai = null;
            int sttTong = 0;
            CPNHThucHienNganSach_Model DataTong = new CPNHThucHienNganSach_Model();
            DataTong.sTenNoiDungChi = "Tổng Cộng: ";
            if (list != null)
            {
                foreach (var item in list)
                {
                    item.TongKinhPhiUSD = item.KinhPhiUSD + item.KinhPhiToYUSD;
                    item.TongKinhPhiVND = item.KinhPhiVND + item.KinhPhiToYVND;

                    item.TongKinhPhiDaChiUSD = item.KinhPhiDaChiUSD + item.KinhPhiDaChiToYUSD;
                    item.TongKinhPhiDaChiVND = item.KinhPhiDaChiVND + item.KinhPhiDaChiToYVND;

                    item.KinhPhiDuocCapChuaChiUSD = item.TongKinhPhiUSD - item.TongKinhPhiDaChiUSD;
                    item.KinhPhiDuocCapChuaChiVND = item.TongKinhPhiVND - item.TongKinhPhiDaChiVND;
                    item.QuyGiaiNganTheoQuy = item.NhiemVuChi - item.TongKinhPhiUSD;
                    sttTong++;
                    if (item.IDNhiemVuChi != idChuongTrinh && item.IDNhiemVuChi != Guid.Empty)
                    {
                        SttChuongTrinh++;
                        SttDuAn = 0;
                        SttLoai = 0;
                        SttDuAn = 0;
                        idDuAn = null;
                        idLoai = null;
                        idHopDong = null;
                        CPNHThucHienNganSach_Model DataCha = new CPNHThucHienNganSach_Model();
                        List<CPNHThucHienNganSach_Model> listDataCha = list.Where(x => x.IDNhiemVuChi == item.IDNhiemVuChi).ToList();
                        DataCha.HopDongUSD = listDataCha.Sum(x => x.HopDongUSD);
                        DataCha.HopDongVND = listDataCha.Sum(x => x.HopDongVND);

                        DataCha.KinhPhiUSD = listDataCha.Sum(x => x.KinhPhiUSD);
                        DataCha.KinhPhiVND = listDataCha.Sum(x => x.KinhPhiVND);
                        DataCha.KinhPhiToYUSD = listDataCha.Sum(x => x.KinhPhiToYUSD);
                        DataCha.KinhPhiToYVND = listDataCha.Sum(x => x.KinhPhiToYVND);
                        DataCha.KinhPhiDaChiUSD = listDataCha.Sum(x => x.KinhPhiDaChiUSD);
                        DataCha.KinhPhiDaChiVND = listDataCha.Sum(x => x.KinhPhiDaChiVND);
                        DataCha.KinhPhiDaChiToYUSD = listDataCha.Sum(x => x.KinhPhiDaChiToYUSD);
                        DataCha.KinhPhiDaChiToYVND = listDataCha.Sum(x => x.KinhPhiDaChiToYVND);


                        DataCha.NhiemVuChi = listDataCha.Sum(x => x.NhiemVuChi);

                        DataCha.TongKinhPhiUSD = DataCha.KinhPhiUSD + DataCha.KinhPhiToYUSD;
                        DataCha.TongKinhPhiVND = DataCha.KinhPhiVND + DataCha.KinhPhiToYVND;

                        DataCha.TongKinhPhiDaChiUSD = DataCha.KinhPhiDaChiUSD + DataCha.KinhPhiDaChiToYUSD;
                        DataCha.TongKinhPhiDaChiVND = DataCha.KinhPhiDaChiVND + DataCha.KinhPhiDaChiToYVND;

                        DataCha.KinhPhiDuocCapChuaChiUSD = DataCha.TongKinhPhiUSD - DataCha.TongKinhPhiDaChiUSD;
                        DataCha.KinhPhiDuocCapChuaChiVND = DataCha.TongKinhPhiVND - DataCha.TongKinhPhiDaChiVND;
                        DataCha.QuyGiaiNganTheoQuy = DataCha.NhiemVuChi - DataCha.TongKinhPhiUSD;

                        DataCha.sTenNoiDungChi = item.sTenNhiemVuChi;
                        DataCha.depth = convertLetter(SttChuongTrinh);
                        DataCha.isTitle = "font-bold-red";
                        idChuongTrinh = item.IDNhiemVuChi;
                        listData.Add(DataCha);
                    }
                    if (item.IDDuAn != idDuAn && item.IDDuAn != Guid.Empty)
                    {
                        SttDuAn++;
                        SttLoai = 0;
                        SttHopDong = 0;
                        idLoai = null;
                        idHopDong = null;
                        CPNHThucHienNganSach_Model DataCha = new CPNHThucHienNganSach_Model();
                        List<CPNHThucHienNganSach_Model> listDataCha = list.Where(x => x.IDDuAn == item.IDDuAn && x.IDNhiemVuChi == item.IDNhiemVuChi).ToList();
                        DataCha.HopDongUSD = listDataCha.Sum(x => x.HopDongUSD);
                        DataCha.HopDongVND = listDataCha.Sum(x => x.HopDongVND);

                        DataCha.KinhPhiUSD = listDataCha.Sum(x => x.KinhPhiUSD);
                        DataCha.KinhPhiVND = listDataCha.Sum(x => x.KinhPhiVND);
                        DataCha.KinhPhiToYUSD = listDataCha.Sum(x => x.KinhPhiToYUSD);
                        DataCha.KinhPhiToYVND = listDataCha.Sum(x => x.KinhPhiToYVND);
                        DataCha.KinhPhiDaChiUSD = listDataCha.Sum(x => x.KinhPhiDaChiUSD);
                        DataCha.KinhPhiDaChiVND = listDataCha.Sum(x => x.KinhPhiDaChiVND);
                        DataCha.KinhPhiDaChiToYUSD = listDataCha.Sum(x => x.KinhPhiDaChiToYUSD);
                        DataCha.KinhPhiDaChiToYVND = listDataCha.Sum(x => x.KinhPhiDaChiToYVND);


                        DataCha.NhiemVuChi = listDataCha.Sum(x => x.NhiemVuChi);

                        DataCha.TongKinhPhiUSD = DataCha.KinhPhiUSD + DataCha.KinhPhiToYUSD;
                        DataCha.TongKinhPhiVND = DataCha.KinhPhiVND + DataCha.KinhPhiToYVND;

                        DataCha.TongKinhPhiDaChiUSD = DataCha.KinhPhiDaChiUSD + DataCha.KinhPhiDaChiToYUSD;
                        DataCha.TongKinhPhiDaChiVND = DataCha.KinhPhiDaChiVND + DataCha.KinhPhiDaChiToYVND;

                        DataCha.KinhPhiDuocCapChuaChiUSD = DataCha.TongKinhPhiUSD - DataCha.TongKinhPhiDaChiUSD;
                        DataCha.KinhPhiDuocCapChuaChiVND = DataCha.TongKinhPhiVND - DataCha.TongKinhPhiDaChiVND;
                        DataCha.QuyGiaiNganTheoQuy = DataCha.NhiemVuChi - DataCha.TongKinhPhiUSD;

                        DataCha.sTenNoiDungChi = item.sTenDuAn;
                        DataCha.isTitle = "font-bold";
                        DataCha.isDuAn = true;
                        DataCha.depth = _cpnhService.GetSTTLAMA(SttDuAn);
                        idDuAn = item.IDDuAn;
                        listData.Add(DataCha);
                    }
                    if (item.iLoaiNoiDungChi != idLoai && item.iLoaiNoiDungChi != 0)
                    {
                        SttLoai++;
                        SttHopDong = 0;
                        idHopDong = null;
                        CPNHThucHienNganSach_Model DataCha = new CPNHThucHienNganSach_Model();
                        List<CPNHThucHienNganSach_Model> listDataCha = list.Where(x => x.iLoaiNoiDungChi == item.iLoaiNoiDungChi && x.IDDuAn == item.IDDuAn && x.IDNhiemVuChi == item.IDNhiemVuChi).ToList();
                        DataCha.HopDongUSD = listDataCha.Sum(x => x.HopDongUSD);
                        DataCha.HopDongVND = listDataCha.Sum(x => x.HopDongVND);

                        DataCha.KinhPhiUSD = listDataCha.Sum(x => x.KinhPhiUSD);
                        DataCha.KinhPhiVND = listDataCha.Sum(x => x.KinhPhiVND);
                        DataCha.KinhPhiToYUSD = listDataCha.Sum(x => x.KinhPhiToYUSD);
                        DataCha.KinhPhiToYVND = listDataCha.Sum(x => x.KinhPhiToYVND);
                        DataCha.KinhPhiDaChiUSD = listDataCha.Sum(x => x.KinhPhiDaChiUSD);
                        DataCha.KinhPhiDaChiVND = listDataCha.Sum(x => x.KinhPhiDaChiVND);
                        DataCha.KinhPhiDaChiToYUSD = listDataCha.Sum(x => x.KinhPhiDaChiToYUSD);
                        DataCha.KinhPhiDaChiToYVND = listDataCha.Sum(x => x.KinhPhiDaChiToYVND);


                        DataCha.NhiemVuChi = listDataCha.Sum(x => x.NhiemVuChi);

                        DataCha.TongKinhPhiUSD = DataCha.KinhPhiUSD + DataCha.KinhPhiToYUSD;
                        DataCha.TongKinhPhiVND = DataCha.KinhPhiVND + DataCha.KinhPhiToYVND;

                        DataCha.TongKinhPhiDaChiUSD = DataCha.KinhPhiDaChiUSD + DataCha.KinhPhiDaChiToYUSD;
                        DataCha.TongKinhPhiDaChiVND = DataCha.KinhPhiDaChiVND + DataCha.KinhPhiDaChiToYVND;

                        DataCha.KinhPhiDuocCapChuaChiUSD = DataCha.TongKinhPhiUSD - DataCha.TongKinhPhiDaChiUSD;
                        DataCha.KinhPhiDuocCapChuaChiVND = DataCha.TongKinhPhiVND - DataCha.TongKinhPhiDaChiVND;
                        DataCha.QuyGiaiNganTheoQuy = DataCha.NhiemVuChi - DataCha.TongKinhPhiUSD;

                        if (item.iLoaiNoiDungChi == 1)
                        {
                            DataCha.sTenNoiDungChi = "Chi ngoại tệ";
                        }
                        else if (item.iLoaiNoiDungChi == 2)
                        {
                            DataCha.sTenNoiDungChi = "Chi trong nước";
                        }
                        else
                        {
                            DataCha.sTenNoiDungChi = "Chi khác";
                        }
                        DataCha.depth = SttLoai.ToString();
                        DataCha.isTitle = "font-bold";
                        idLoai = item.iLoaiNoiDungChi;
                        listData.Add(DataCha);
                    }
                    if (item.IDHopDong != idHopDong && item.IDHopDong != Guid.Empty)
                    {
                        SttHopDong++;
                        CPNHThucHienNganSach_Model DataCha = new CPNHThucHienNganSach_Model();
                        List<CPNHThucHienNganSach_Model> listDataCha = list.Where(x => x.IDHopDong == item.IDHopDong && x.iLoaiNoiDungChi == item.iLoaiNoiDungChi && x.IDDuAn == item.IDDuAn && x.IDNhiemVuChi == item.IDNhiemVuChi).ToList();
                        DataCha.HopDongUSD = listDataCha.Sum(x => x.HopDongUSD);
                        DataCha.HopDongVND = listDataCha.Sum(x => x.HopDongVND);

                        DataCha.KinhPhiUSD = listDataCha.Sum(x => x.KinhPhiUSD);
                        DataCha.KinhPhiVND = listDataCha.Sum(x => x.KinhPhiVND);
                        DataCha.KinhPhiToYUSD = listDataCha.Sum(x => x.KinhPhiToYUSD);
                        DataCha.KinhPhiToYVND = listDataCha.Sum(x => x.KinhPhiToYVND);
                        DataCha.KinhPhiDaChiUSD = listDataCha.Sum(x => x.KinhPhiDaChiUSD);
                        DataCha.KinhPhiDaChiVND = listDataCha.Sum(x => x.KinhPhiDaChiVND);
                        DataCha.KinhPhiDaChiToYUSD = listDataCha.Sum(x => x.KinhPhiDaChiToYUSD);
                        DataCha.KinhPhiDaChiToYVND = listDataCha.Sum(x => x.KinhPhiDaChiToYVND);


                        DataCha.NhiemVuChi = listDataCha.Sum(x => x.NhiemVuChi);

                        DataCha.TongKinhPhiUSD = DataCha.KinhPhiUSD + DataCha.KinhPhiToYUSD;
                        DataCha.TongKinhPhiVND = DataCha.KinhPhiVND + DataCha.KinhPhiToYVND;

                        DataCha.TongKinhPhiDaChiUSD = DataCha.KinhPhiDaChiUSD + DataCha.KinhPhiDaChiToYUSD;
                        DataCha.TongKinhPhiDaChiVND = DataCha.KinhPhiDaChiVND + DataCha.KinhPhiDaChiToYVND;

                        DataCha.KinhPhiDuocCapChuaChiUSD = DataCha.TongKinhPhiUSD - DataCha.TongKinhPhiDaChiUSD;
                        DataCha.KinhPhiDuocCapChuaChiVND = DataCha.TongKinhPhiVND - DataCha.TongKinhPhiDaChiVND;
                        DataCha.QuyGiaiNganTheoQuy = DataCha.NhiemVuChi - DataCha.TongKinhPhiUSD;

                        DataCha.sTenNoiDungChi = item.sTenHopDong;
                        DataCha.HopDongUSD = item.HopDongUSD;
                        DataCha.HopDongVND = item.HopDongVND;
                        DataCha.isHopDong = true;
                        DataCha.depth = SttLoai.ToString() + "." + SttHopDong.ToString();
                        idHopDong = item.IDHopDong;
                        listData.Add(DataCha);
                    }

                    DataTong.HopDongUSD += item.HopDongUSD;
                    DataTong.HopDongVND += item.HopDongVND;
                    DataTong.NCVTTCP += item.NCVTTCP;
                    DataTong.NhiemVuChi += item.NhiemVuChi;
                    DataTong.KinhPhiUSD += item.KinhPhiUSD;
                    DataTong.KinhPhiVND += item.KinhPhiVND;
                    DataTong.KinhPhiToYUSD += item.KinhPhiToYUSD;
                    DataTong.KinhPhiToYVND += item.KinhPhiToYVND;
                    DataTong.KinhPhiDaChiUSD += item.KinhPhiDaChiUSD;
                    DataTong.KinhPhiDaChiVND += item.KinhPhiDaChiVND;
                    DataTong.KinhPhiDaChiToYUSD += item.KinhPhiDaChiToYUSD;
                    DataTong.KinhPhiDaChiToYVND += item.KinhPhiDaChiToYVND;
                    DataTong.KinhPhiDuocCapChuaChiUSD += item.KinhPhiDuocCapChuaChiUSD;

                    DataTong.TongKinhPhiUSD += item.TongKinhPhiUSD;
                    DataTong.TongKinhPhiVND += item.TongKinhPhiVND;

                    DataTong.TongKinhPhiDaChiUSD += item.TongKinhPhiDaChiUSD;
                    DataTong.TongKinhPhiDaChiVND += item.TongKinhPhiDaChiVND;
                    DataTong.QuyGiaiNganTheoQuy += item.QuyGiaiNganTheoQuy;

                    DataTong.fLuyKeKinhPhiDuocCap_USD += item.fLuyKeKinhPhiDuocCap_USD;
                    DataTong.fLuyKeKinhPhiDuocCap_VND += item.fLuyKeKinhPhiDuocCap_VND;
                    DataTong.fDeNghiQTNamNay_USD += item.fDeNghiQTNamNay_USD;
                    DataTong.fDeNghiQTNamNay_VND += item.fDeNghiQTNamNay_VND;

                    if (sttTong == list.Count())
                    {
                        DataTong.isDuAn = true;
                        DataTong.isTitle = "font-bold";
                        DataTong.isSum = true;
                        listData.Add(DataTong);
                    }
                }
            }

            return listData;
        }
        public List<Dropdown_ByYear_ThucHienNganSach> GetListNamKeHoach()
        {
            List<Dropdown_ByYear_ThucHienNganSach> listNam = new List<Dropdown_ByYear_ThucHienNganSach>();
            int namHienTai = DateTime.Now.Year + 1;
            for (int i = 20; i > 0; i--)
            {
                namHienTai -= 1;
                Dropdown_ByYear_ThucHienNganSach namKeHoachOpt = new Dropdown_ByYear_ThucHienNganSach()
                {
                    Value = namHienTai,
                    Text = "Năm " + namHienTai.ToString()
                };
                listNam.Add(namKeHoachOpt);
            }
            return listNam;
        }
    }
}