using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dao;
using Entities.Models;

using System.Data;
using Oracle.ManagedDataAccess.Client;
using Entities.Objects;
using Utilities;

namespace DataAccess.Busineess
{
    public class KntcReport : BaseRepository
    {
        Log log = new Log();
        public List<LOAIKHIEUTO> getReportBaoBaoThongKeLoaiKhieuTo(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi ,int inguondon,int itiepnhan)
        {

            List<LOAIKHIEUTO> resule = new List<LOAIKHIEUTO>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));
            try
            {
                resule = GetListObjetReport<LOAIKHIEUTO>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<THAMQUYENGIAIQUYET> getReportBaoBaoThongKeCoquanthamquyen(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<THAMQUYENGIAIQUYET> resule = new List<THAMQUYENGIAIQUYET>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));
            try
            {
                resule = GetListObjetReport<THAMQUYENGIAIQUYET>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<NOIGUIDON> getReportBaoBaoThongKeNoiGuiDon(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<NOIGUIDON> resule = new List<NOIGUIDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));

            try
            {
                resule = GetListObjetReport<NOIGUIDON>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<NGUOINHAPDON> getReportBaoBaoThongKeNguoiNhapDon(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<NGUOINHAPDON> resule = new List<NGUOINHAPDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));
            try
            {
                resule = GetListObjetReport<NGUOINHAPDON>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<NGUOIXULY> getReportBaoBaoThongKeNguoiXuLy(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<NGUOIXULY> resule = new List<NGUOIXULY>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));
            try
            {
                resule = GetListObjetReport<NGUOIXULY>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<COQUANCHUYENDON> getReportBaoBaoThongKecoquanchuyendon(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<COQUANCHUYENDON> resule = new List<COQUANCHUYENDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);

            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));
            try
            {
                resule = GetListObjetReport<COQUANCHUYENDON>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<TRUNGDON> getReportBaoBaoThongKeTrungDon(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<TRUNGDON> resule = new List<TRUNGDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));
            try
            {
                resule = GetListObjetReport<TRUNGDON>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<TONGSODON> getReportBaoBaoThongKeTongSoDon(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<TONGSODON> resule = new List<TONGSODON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));

            try
            {
                resule = GetListObjetReport<TONGSODON>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<CHIITETDON> getReportBaoBaoThongKeChiTietDon(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<CHIITETDON> resule = new List<CHIITETDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));

            try
            {
                resule = GetListObjetReport<CHIITETDON>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<DIABAN> getReportBaoBaoThongKeDiaBan1(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<DIABAN> resule = new List<DIABAN>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));

            try
            {
                resule = GetListObjetReport<DIABAN>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<DIABANTONG> getReportBaoBaoThongKeDiaBan2(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<DIABANTONG> resule = new List<DIABANTONG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));

            try
            {
                resule = GetListObjetReport<DIABANTONG>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<SOLIEUDON> getReportBaoBaoThongKeSoLieuDon(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<SOLIEUDON> resule = new List<SOLIEUDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));

            try
            {
                resule = GetListObjetReport<SOLIEUDON>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<LINHVUC> getReportNhomLinhVuc(String procedurename)
        {

            List<LINHVUC> resule = new List<LINHVUC>();
            List<OracleParameter> lisparam = new List<OracleParameter>();

            try
            {
                resule = GetListObjetReport<LINHVUC>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<KNTC_NGUONDON> getReportNguonDon(String procedurename)
        {

            List<KNTC_NGUONDON> resule = new List<KNTC_NGUONDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();

            try
            {
                resule = GetListObjetReport<KNTC_NGUONDON>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<DONKHIEUTO> getReportDonKhieuTo(String procedurename, string pram01, string pram02, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {

            List<DONKHIEUTO> resule = new List<DONKHIEUTO>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", itinhchat));
            lisparam.Add(new OracleParameter("pram05", inoidung));
            lisparam.Add(new OracleParameter("pram06", ilinhvuc));
            lisparam.Add(new OracleParameter("pram07", idonvi));
            lisparam.Add(new OracleParameter("pram08", inguondon));
            lisparam.Add(new OracleParameter("pram09", itiepnhan));
             
            try
            {
                resule = GetListObjetReport<DONKHIEUTO>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<CONGVANCHUYENDON> getReportBaoBaoCongvanchuyendon(String procedurename, DateTime pram01, DateTime pram02, int iloaidon, int iLoai)
        {

            List<CONGVANCHUYENDON> resule = new List<CONGVANCHUYENDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", iloaidon));
            lisparam.Add(new OracleParameter("pram04", iLoai));
            try
            {
                resule = GetListObjetReport<CONGVANCHUYENDON>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<Theodoigiaiquyetdon> getReportTheodoigiaiquyetdon(String procedurename, DateTime pram01, DateTime pram02, int iKyHop, int iLoai)
        {

            List<Theodoigiaiquyetdon> resule = new List<Theodoigiaiquyetdon>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", iKyHop));
            lisparam.Add(new OracleParameter("pram04", iLoai));
            try
            {
                resule = GetListObjetReport<Theodoigiaiquyetdon>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        /*  Ham de lay cac du lieu (KNTC_DON left join KNTC_VANBAN) phuc vu cho xuat bao cao thang
         *  Tim kiem KNTC_DON trong khoang thoi gian giua 2 param DateTime
         */
        public List<BAOCAOTHANG> getReportBaoCaoThang(String procedurename, String p_tuNgay, String p_denNgay)
        {

            List<BAOCAOTHANG> resule = new List<BAOCAOTHANG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("p_tuNgay", p_tuNgay));
            lisparam.Add(new OracleParameter("p_denNgay", p_denNgay));
            try
            {
                resule = GetListObjetReport<BAOCAOTHANG>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<BAOCAODONTHUHANGTUAN> getReportBaocaodonthuhangtuan(String procedurename, DateTime pram01, DateTime pram02, int iNam)
        {

            List<BAOCAODONTHUHANGTUAN> resule = new List<BAOCAODONTHUHANGTUAN>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", iNam));
            try
            {
                resule = GetListObjetReport<BAOCAODONTHUHANGTUAN>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }

        public List<RPT_KNTC_DONCHOXULIPHANLOAI> GetReportKNTCDonChoXuLiPhanLoai(string procedureName, int loaibaocao, string tungay, string denngay, int user)
        {

            List<RPT_KNTC_DONCHOXULIPHANLOAI> result = new List<RPT_KNTC_DONCHOXULIPHANLOAI>();
            List<OracleParameter> paramList = new List<OracleParameter>();
            paramList.Add(new OracleParameter("iLoaiBaoCao", loaibaocao));
            paramList.Add(new OracleParameter("dTuNgay", tungay));
            paramList.Add(new OracleParameter("dDenNgay", denngay));
            paramList.Add(new OracleParameter("p_IUSER", user));

            try
            {
                result = GetListObjetReport<RPT_KNTC_DONCHOXULIPHANLOAI>(procedureName, paramList);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;

        }

        public List<RPT_KNTC_DONVIDATRALOI_4A> getReportDanhSachCacDonViDaTL_4A(int ikhoa, int iLoaiBaoCao, int iCoQuanTraLoi, DateTime? dTuNgay, DateTime? dDenNgay, int p_IUSER)
        {
            
            List<RPT_KNTC_DONVIDATRALOI_4A> resule = new List<RPT_KNTC_DONVIDATRALOI_4A>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("iKhoa", ikhoa));
            lisparam.Add(new OracleParameter("iLoaiBaoCao", iLoaiBaoCao));
            lisparam.Add(new OracleParameter("iCoQuanTraLoi", iCoQuanTraLoi));
            lisparam.Add(new OracleParameter("iTuNgay", dTuNgay));
            lisparam.Add(new OracleParameter("iDenNgay", dDenNgay));
            lisparam.Add(new OracleParameter("p_IUSER", p_IUSER));

            try
            {
                resule = GetListObjetReport<RPT_KNTC_DONVIDATRALOI_4A>("RPT_KNTC_BAOCAO_DANHSACHCACDONVIDACOTRALOI_4A", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }

            return mergeReportDanhSachCacDonViDaTL_4A(resule);
        }

        List<RPT_KNTC_DONVIDATRALOI_4A> mergeReportDanhSachCacDonViDaTL_4A(List<RPT_KNTC_DONVIDATRALOI_4A> listData)
        {
            List<RPT_KNTC_DONVIDATRALOI_4A> resule = new List<RPT_KNTC_DONVIDATRALOI_4A>();
            RPT_KNTC_DONVIDATRALOI_4A dataSend = new RPT_KNTC_DONVIDATRALOI_4A();
            
            for (int i= listData.Count()-1 ; i>=0; i--)
            {
                if(listData[i].LOAIVANBAN == "chuyenxulylaidon" || listData[i].LOAIVANBAN == "chuyenxuly_noibo"){
                    dataSend = listData[i]; 
                }else
                {
                    if(dataSend != null)
                    {
                        listData[i].SOCONGVANCHUYEN = dataSend.SOCONGVANCHUYEN;
                        listData[i].COQUANCHUYENDEN = dataSend.COQUANTRALOI;
                        listData[i].NGAYBANHANH = String.Format("{0:dd/MM/yyyy}", listData[i].NGAYBANHANHTRALOI);
                        resule.Insert(0, listData[i]);
                    }
                }
            }
            for (int i = 0; i < resule.Count(); i++) resule[i].TT = (i + 1).ToString();
            return resule;
        }

        
        public List<RPT_KNTC_CONGVANDONDOC_3F> getReportDanhSachCongVanDonDoc_3F(int iLoaiBaoCao, DateTime? dTuNgay, DateTime? dDenNgay, int p_IUSER)
        {

            List<RPT_KNTC_CONGVANDONDOC_3F> resule = new List<RPT_KNTC_CONGVANDONDOC_3F>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("iLoaiBaoCao", iLoaiBaoCao));
            lisparam.Add(new OracleParameter("iTuNgay", dTuNgay));
            lisparam.Add(new OracleParameter("iDenNgay", dDenNgay));
            lisparam.Add(new OracleParameter("p_IUSER", p_IUSER));

            try
            {
                resule = GetListObjetReport<RPT_KNTC_CONGVANDONDOC_3F>("RPT_KNTC_BAOCAO_DANHSACHDONDOC_3F", lisparam);
                for (int i = 0; i < resule.Count(); i++) resule[i].NGAYBANHANHTRALOISTRING = String.Format("{0:dd/MM/yyyy}", resule[i].NGAYBANHANHTRALOI);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;
        }

        public List<RPT_KNTC_DONDATRALOI_4B> GetReportTongHopKetQuaXuLyDon_4B(int iLoaiBaoCao, DateTime? dTuNgay, DateTime? dDenNgay, DateTime? dTuNgayKyTruoc, DateTime? dDenNgayKyTruoc, decimal? iUserType)
        {

            List<RPT_KNTC_DONDATRALOI_4B> result;
            List<OracleParameter> paramsList = new List<OracleParameter>
            {
                new OracleParameter("iLoaiBaoCao", iLoaiBaoCao),
                new OracleParameter("iTuNgay", dTuNgay),
                new OracleParameter("iDenNgay", dDenNgay),
                new OracleParameter("iTuNgayKyTruoc", dTuNgayKyTruoc),
                new OracleParameter("iDenNgayKyTruoc", dDenNgayKyTruoc),
                new OracleParameter("iUserType", iUserType)
            };

            try
            {
                result = GetListObjetReport<RPT_KNTC_DONDATRALOI_4B>("RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4B", paramsList);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }

            return result;
        }
        
        public List<RPT_KNTC_DONCHITIET_4F> GetReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4F(DateTime? dTuNgay, DateTime? dDenNgay, DateTime? dTuNgayKyTruoc, DateTime? dDenNgayKyTruoc, int iDoiTuong, string listDiaPhuong, int iUser)
        {

            List<RPT_KNTC_DONCHITIET_4F> result;
            List<OracleParameter> paramsList = new List<OracleParameter>
            {
                new OracleParameter("iTuNgay", dTuNgay),
                new OracleParameter("iDenNgay", dDenNgay),
                new OracleParameter("iTuNgayKyTruoc", dTuNgayKyTruoc),
                new OracleParameter("iDenNgayKyTruoc", dDenNgayKyTruoc),
                new OracleParameter("iUserID", iUser),
                new OracleParameter("iDoiTuong", iDoiTuong),
                new OracleParameter("listDiaPhuongTimKiem", listDiaPhuong)
            };

            try
            {
                result = GetListObjetReport<RPT_KNTC_DONCHITIET_4F>("RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4F", paramsList);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }

            return result;
        }
        public List<RPT_KNTC_DONCHITIET_4G> GetReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4G(DateTime? dTuNgay, DateTime? dDenNgay, DateTime? dTuNgayKyTruoc, DateTime? dDenNgayKyTruoc, int iDoiTuong, string listDiaPhuong, int iUser)
        {

            List<RPT_KNTC_DONCHITIET_4G> result;
            List<OracleParameter> paramsList = new List<OracleParameter>
            {
                new OracleParameter("iTuNgay", dTuNgay),
                new OracleParameter("iDenNgay", dDenNgay),
                new OracleParameter("iTuNgayKyTruoc", dTuNgayKyTruoc),
                new OracleParameter("iDenNgayKyTruoc", dDenNgayKyTruoc),
                new OracleParameter("iUserID", iUser),
                new OracleParameter("iDoiTuong", iDoiTuong),
                new OracleParameter("listDiaPhuongTimKiem", listDiaPhuong)
            };

            try
            {
                result = GetListObjetReport<RPT_KNTC_DONCHITIET_4G>("RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4G", paramsList);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }

            return result;
        }
        public List<RPT_KNTC_DONDATRALOI_4C> GetReportTongHopKetQuaXuLyDon_4C(int iLoaiBaoCao, DateTime? dTuNgay, DateTime? dDenNgay, DateTime? dTuNgayKyTruoc, DateTime? dDenNgayKyTruoc, decimal? iUserType)
        {

            List<RPT_KNTC_DONDATRALOI_4C> result;
            List<OracleParameter> paramsList = new List<OracleParameter>
            {
                new OracleParameter("iLoaiBaoCao", iLoaiBaoCao),
                new OracleParameter("iTuNgay", dTuNgay),
                new OracleParameter("iDenNgay", dDenNgay),
                new OracleParameter("iTuNgayKyTruoc", dTuNgayKyTruoc),
                new OracleParameter("iDenNgayKyTruoc", dDenNgayKyTruoc),
                new OracleParameter("iUserType", iUserType),
            };

            try
            {
                result = GetListObjetReport<RPT_KNTC_DONDATRALOI_4C>("RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4C", paramsList);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }

            return result;
        }

        public List<RPT_KNTC_DONDATRALOI_4C> GetReportTongHopKetQuaXuLyDon_4D(int iLoaiBaoCao, DateTime? dTuNgay, DateTime? dDenNgay, DateTime? dTuNgayKyTruoc, DateTime? dDenNgayKyTruoc, decimal? iUserType)
        {

            List<RPT_KNTC_DONDATRALOI_4C> result;
            List<OracleParameter> paramsList = new List<OracleParameter>
            {
                new OracleParameter("iLoaiBaoCao", iLoaiBaoCao),
                new OracleParameter("iTuNgay", dTuNgay),
                new OracleParameter("iDenNgay", dDenNgay),
                new OracleParameter("iTuNgayKyTruoc", dTuNgayKyTruoc),
                new OracleParameter("iDenNgayKyTruoc", dDenNgayKyTruoc),
                new OracleParameter("iUserType", iUserType),
            };

            try
            {
                result = GetListObjetReport<RPT_KNTC_DONDATRALOI_4C>("RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4D", paramsList);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }

        public List<RPT_KNTC_DONDATRALOI_4E> GetReportTiepNhanXuLyGiamSat_4E(int iLoaiBaoCao, DateTime? dTuNgay, DateTime? dDenNgay, decimal? iUserType)
        {

            List<RPT_KNTC_DONDATRALOI_4E> result;
            List<OracleParameter> paramsList = new List<OracleParameter>
            {
                new OracleParameter("iLoaiBaoCao", iLoaiBaoCao),
                new OracleParameter("iTuNgay", dTuNgay),
                new OracleParameter("iDenNgay", dDenNgay),
                new OracleParameter("iUserType", iUserType)
            };

            try
            {
                result = GetListObjetReport<RPT_KNTC_DONDATRALOI_4E>("RPT_KNTC_BAOCAO_TIEPNHANXULYGIAMSAT_4E", paramsList);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }

            return result;
        }
    }
}
