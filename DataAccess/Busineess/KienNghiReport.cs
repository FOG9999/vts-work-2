using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dao;
using Entities.Models;
using Entities.Objects;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using Utilities;
using FlexCel.Core;
using Utilities.Enums;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using System.Web;

namespace DataAccess.Busineess
{
    public class KienNghiReport: BaseRepository
    {
        KN_KiennghiRepository kN_KiennghiRepository = new KN_KiennghiRepository();
        Log log = new Log();
        public List<KIENNGHIPHULUC1> getReportBaoCaoThongKePhuLuc1(String procedurename, int pram01, int pram02,int pram03)
        {

            List<KIENNGHIPHULUC1> resule = new List<KIENNGHIPHULUC1>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", pram03));
            try
            {
                resule = GetListObjetReport<KIENNGHIPHULUC1>(procedurename, lisparam);
            }

            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        
        public List<KIENNGHIPHULUC2> getReportBaoCaoThongKePhuLuc2(String procedurename, int pram01, int pram02, int pram03)
        {

            List<KIENNGHIPHULUC2> resule = new List<KIENNGHIPHULUC2>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", pram03));
            try
            {
                resule = GetListObjetReport<KIENNGHIPHULUC2>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<KIENNGHIPHULUC3> getReportBaoCaoThongKePhuLuc3(String procedurename, int pram01, int pram02,int pram03)
        {

            List<KIENNGHIPHULUC3> resule = new List<KIENNGHIPHULUC3>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", pram03));
            try
            {
                resule = GetListObjetReport<KIENNGHIPHULUC3>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<KIENNGHIPHULUC4> getReportBaoCaoThongKePhuLuc4(String procedurename,int pram01,int pram02)
        {

            List<KIENNGHIPHULUC4> resule = new List<KIENNGHIPHULUC4>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            try
            {
                resule = GetListObjetReport<KIENNGHIPHULUC4>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<KIENNGHIPHULUC5> getReportBaoCaoThongKePhuLuc5(String procedurename, 
            string pram01, string pram02,int pram03,int pram04)
        {

            List<KIENNGHIPHULUC5> resule = new List<KIENNGHIPHULUC5>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(tungay);
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", pram03));
            lisparam.Add(new OracleParameter("pram04", pram04));

            try
            {
                resule = GetListObjetReport<KIENNGHIPHULUC5>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<KIENNGHIPHULUC5B> getReportBaoCaoThongKePhuLuc5B(String procedurename, int pram01, int pram02, int pram03)
        {

            List<KIENNGHIPHULUC5B> resule = new List<KIENNGHIPHULUC5B>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", pram03));


            try
            {
                resule = GetListObjetReport<KIENNGHIPHULUC5B>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<KIENNGHIPHULUC6> getReportBaoCaoThongKePhuLuc6(String procedurename, string pram01, string pram02, int pram03)
        {

            List<KIENNGHIPHULUC6> resule = new List<KIENNGHIPHULUC6>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(tungay);
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", pram03));

            try
            {
                resule = GetListObjetReport<KIENNGHIPHULUC6>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<KIENNGHIPHULUC7> getReportBaoCaoThongKePhuLuc7(String procedurename, int pram01, int pram02)
        {

            List<KIENNGHIPHULUC7> resule = new List<KIENNGHIPHULUC7>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            try
            {
                resule = GetListObjetReport<KIENNGHIPHULUC7>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }

        public List<KIENNGHITRALOIKNTC> getReportBaoCaoThongKeTraLoiKNTC(String procedurename, int pram01, int pram02, int pram03)
        {

            List<KIENNGHITRALOIKNTC> resule = new List<KIENNGHITRALOIKNTC>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", pram03));
            try
            {
                resule = GetListObjetReport<KIENNGHITRALOIKNTC>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }

        public List<KIENNGHITRALOIKNDBQH> getReportBaoCaoThongKeTraLoiKN_DENDBQH(String procedurename, int pram01, string pram02, int pram03)
        {

            List<KIENNGHITRALOIKNDBQH> resule = new List<KIENNGHITRALOIKNDBQH>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", pram03));
            try
            {
                resule = GetListObjetReport<KIENNGHITRALOIKNDBQH>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }

        public List<KIENNGHIPHANLOAI> getReportBaoCaoThongKePhanLoai(String procedurename,int pram01, int pram02, int pram03)
        {

            List<KIENNGHIPHANLOAI> resule = new List<KIENNGHIPHANLOAI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", pram03));
            try
            {
                resule = GetListObjetReport<KIENNGHIPHANLOAI>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        public List<KIENNGHITRALOI> getReportBaoCaoThongKeTraLoi(String procedurename, int pram01, int pram02, int pram03)
        {

            List<KIENNGHITRALOI> resule = new List<KIENNGHITRALOI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("pram01", pram01));
            lisparam.Add(new OracleParameter("pram02", pram02));
            lisparam.Add(new OracleParameter("pram03", pram03));

            try
            {
                resule = GetListObjetReport<KIENNGHITRALOI>(procedurename, lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }

        /*  HaiPN16
         *  L?y danh s�ch k?t qu?
         */
        public List<KIENNGHI_KETQUA> getReportBaoCaoKetQua(int iKyHop, int iTruocKyHop, int iDonViTiepNhan, int iLinhVuc, int iDoiTuong)
        {
            var param = new List<OracleParameter>();
            string sql = "SELECT  (T4.CTYPE ||' ' || T4.CTEN) as TEN_DIA_PHUONG , T1.CNOIDUNG, T2.CTEN as TEN_LINH_VUC FROM ((KN_KIENNGHI T1 LEFT JOIN LINHVUC_COQUAN T2 " +
                "on T1.ILINHVUC = T2.ILINHVUC) " +
                "LEFT JOIN KN_CHUONGTRINH T3 " +
                "on T1.ICHUONGTRINH = T3.ICHUONGTRINH) " +
                "LEFT JOIN DIAPHUONG T4 " +
                "on T1.IDIAPHUONG1 = T4.IDIAPHUONG " +
                "Where T1.IKYHOP =:param01";
            param.Add(new OracleParameter("param01", iKyHop));
            sql += " AND T1.ITRUOCKYHOP =:param02";
            param.Add(new OracleParameter("param02", iTruocKyHop));
            if (iDonViTiepNhan != 0)
            {
                sql += " AND T1.IDIAPHUONG1 =:param03";
                param.Add(new OracleParameter("param03", iDonViTiepNhan));
            }
            if (iLinhVuc != -1)
            {
                sql += " AND T1.ILINHVUC =:param04";
                param.Add(new OracleParameter("param04", iLinhVuc));
            }
            sql += " AND T3.IDOITUONG =:param05";
            param.Add(new OracleParameter("param05", iDoiTuong));
            // Gioi han cac ket qua ve dia phuong hien tai
            //sql += " And T1.IDIAPHUONG0 = " + AppConfig.IDIAPHUONG;
            sql += " ORDER BY T4.CTEN, T1.ILINHVUC";
            List<KIENNGHI_KETQUA> resule = new List<KIENNGHI_KETQUA>();

            try
            {
                resule = base.GetList<KIENNGHI_KETQUA>(sql, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }

        public List<KIENNGHI_REPORT_TONGHOPHUYEN> getReportTongHopHuyen(string listKyHop, int iThamQuyenDonVi, string listHuyen_Xa_ThanhPho, string listLinhVuc, int iTenBaoCao, int keydata, int p_IUSER)
        {
            List<KIENNGHI_REPORT_TONGHOPHUYEN> resule = new List<KIENNGHI_REPORT_TONGHOPHUYEN>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("listKyHop", listKyHop));
            lisparam.Add(new OracleParameter("listLinhVuc", listLinhVuc));
            lisparam.Add(new OracleParameter("iDonViThamQuyen", iThamQuyenDonVi));
            lisparam.Add(new OracleParameter("listHuyen_Xa_ThanhPho", listHuyen_Xa_ThanhPho));
            lisparam.Add(new OracleParameter("iTenBaoCao", iTenBaoCao));
            lisparam.Add(new OracleParameter("keydata", keydata));
            lisparam.Add(new OracleParameter("p_IUSER", p_IUSER));
            try
            {
                resule = GetListObjetReport<KIENNGHI_REPORT_TONGHOPHUYEN>("RPT_KN_QH_BAOCAO_TONGHOP_HUYEN", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;

        }
        /*public List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> getReportKienNghiTongHop_BanDanNguyen(int iDonViXuLy, string listLinhVuc, int iTenBaoCao, string listKyHop, string listHuyen_Xa_ThanhPho)
        {

            List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> result = new List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("iDonViXuLy", iDonViXuLy));
            lisparam.Add(new OracleParameter("listLinhVuc", listLinhVuc));
            lisparam.Add(new OracleParameter("iTenBaoCao", iTenBaoCao));
            lisparam.Add(new OracleParameter("listKyHop", listKyHop));
            lisparam.Add(new OracleParameter("listHuyen_Xa_ThanhPho", listHuyen_Xa_ThanhPho));

            try
            {
                result = GetListObjetReport<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG>("RPT_KN_QH_BAOCAO_KNCT_THUOC_TRUNGUONG", lisparam);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result;
        }*/
        public List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> getReportKienNghiTongHop_BanDanNguyen(int iDonViXuLy, string listKyHop, string iNguonKienNghi, string listLinhVuc, int iTenBaoCao, string listHuyen_Xa_ThanhPho, int p_IUSER)
        {

            List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> result = new List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("iDonViXuLy", iDonViXuLy));
            lisparam.Add(new OracleParameter("listLinhVuc", listLinhVuc));
            lisparam.Add(new OracleParameter("iTenBaoCao", iTenBaoCao));
            lisparam.Add(new OracleParameter("listKyHop", listKyHop));
            lisparam.Add(new OracleParameter("iNguonKienNghi", iNguonKienNghi));
            lisparam.Add(new OracleParameter("listHuyen_Xa_ThanhPho", listHuyen_Xa_ThanhPho));
            lisparam.Add(new OracleParameter("p_IUSER", p_IUSER));

            try
            {
                result = GetListObjetReport<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG>("RPT_KN_QH_BAOCAO_KNCT_THUOC_TRUNGUONG", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }


        public List<KIENNGHICUTRI_1A> getReportKienNghiCuTriBaoCao1A(string lstKyHop, string lstNguonKN, string lstLinhVuc, int loaibaocao, int p_IUSER)
        {
            List<KIENNGHICUTRI_1A> resule = new List<KIENNGHICUTRI_1A>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("lstKyHop", lstKyHop));
            lisparam.Add(new OracleParameter("lstNguonKN", lstNguonKN));
            lisparam.Add(new OracleParameter("lstLinhVuc", lstLinhVuc));
            lisparam.Add(new OracleParameter("loaibaocao", loaibaocao));
            lisparam.Add(new OracleParameter("p_IUSER", p_IUSER));

            try
            {
                resule = GetListObjetReport<KIENNGHICUTRI_1A>("RPT_KN_HDND_DANHMUCCUTRI_1A", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;
        }

        public List<KIENNGHI_REPORT_TONGHOP_TINH> GetReportKienNghiHoiDongNhanDan(string storeProcdureName, int iTenBaoCao, string listKyHop, string listLinhVuc, string listHuyenXaTP, int p_IUSER)
        {
            var param = new List<OracleParameter>
            {
                new OracleParameter("listKyHop", listKyHop),
                new OracleParameter("listLinhVuc", listLinhVuc),
                new OracleParameter("listHuyen_Xa_ThanhPho", listHuyenXaTP),
                new OracleParameter("iTenBaoCao", iTenBaoCao),
                new OracleParameter("p_IUSER", p_IUSER)
            };
            List<KIENNGHI_REPORT_TONGHOP_TINH> result = new List<KIENNGHI_REPORT_TONGHOP_TINH>();
            try
            {
                result = GetListObjetReport<KIENNGHI_REPORT_TONGHOP_TINH>(storeProcdureName, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> getReportKienNghiTapHop_Dacotraloi(int iTruocKyHop, int iLoaiBaoCao, string listLinhVuc, string listKyHop, string listHuyen_Xa_ThanhPho, int iUser)
        {

            List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> resule = new List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("iTruocKyHop", iTruocKyHop));
            lisparam.Add(new OracleParameter("iLoaiBaoCao", iLoaiBaoCao));
            lisparam.Add(new OracleParameter("listLinhVuc", listLinhVuc));
            lisparam.Add(new OracleParameter("p_USER", iUser));
            lisparam.Add(new OracleParameter("listKyHop", listKyHop));
            lisparam.Add(new OracleParameter("listHuyen_Xa_ThanhPho", listHuyen_Xa_ThanhPho));

            try
            {
                resule = GetListObjetReport<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG>("RPT_KN_QH_BAOCAO_KNCT_TAPHOP_DACOTRALOI", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                resule = null;
            }
            return resule;
        }

        public List<KIENNGHICUTRI_1A> getReportTongHopYKienCuTri1B(string listKyHop, string lstNguonKN, string lstLinhVuc, int iDoiTuong, int iuser)
        {
            var param = new List<OracleParameter>
            {
                new OracleParameter("lstKyHop", listKyHop),
                new OracleParameter("lstNguonKN", lstNguonKN),
                new OracleParameter("lstLinhVuc", lstLinhVuc),
                new OracleParameter("iDoiTuong", iDoiTuong),
                new OracleParameter("p_IUSER", iuser)
            };
            List<KIENNGHICUTRI_1A> result = new List<KIENNGHICUTRI_1A>();
            try
            {
                result = GetListObjetReport<KIENNGHICUTRI_1A>("RPT_KN_BAOCAO_TONGHOPYKIEN_1B ", param);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }

        public List<KIENNGHICUTRI_1B1> getReportTongHopYKienCuTri1B1(string listKyHop, string lstNguonKN, string lstLinhVuc, int iDoiTuong, int iuser)
        {
            var param = new List<OracleParameter>
            {
                new OracleParameter("lstKyHop", listKyHop),
                new OracleParameter("lstNguonKN", lstNguonKN),
                new OracleParameter("lstLinhVuc", lstLinhVuc),
                new OracleParameter("iDoiTuong", iDoiTuong),
                new OracleParameter("iUserType", iuser)
            };
            List<KIENNGHICUTRI_1B1> result = new List<KIENNGHICUTRI_1B1>();
            try
            {
                result = GetListObjetReport<KIENNGHICUTRI_1B1>("RPT_KN_BAOCAO_TONGHOPYKIEN_1B1", param);
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
