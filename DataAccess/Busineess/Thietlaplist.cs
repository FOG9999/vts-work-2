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


namespace DataAccess.Busineess
{
    public class Thietlaplist:BaseRepository
    {
        public List<THIETLAP_NGUOIDUNG> GetNguoiDung(String procedurename,string ctukhoa)
        {

            List<THIETLAP_NGUOIDUNG> resule = new List<THIETLAP_NGUOIDUNG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var noidung = new OracleParameter("p_TUKHOA", OracleDbType.NVarchar2);
            noidung.Value = ctukhoa;
            lisparam.Add(noidung);
            
            try
            {
                resule = GetListObjetReport<THIETLAP_NGUOIDUNG>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;
        }
        public List<THIETLAP_NGUOIDUNG> GetNguoiDung_OPT(String procedurename, string ctukhoa,int page,int pagesize)
        {

            List<THIETLAP_NGUOIDUNG> resule = new List<THIETLAP_NGUOIDUNG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var noidung = new OracleParameter("p_TUKHOA", OracleDbType.NVarchar2);
            noidung.Value = ctukhoa;
            lisparam.Add(noidung);
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", pagesize));
            try
            {
                resule = GetListObjetReport<THIETLAP_NGUOIDUNG>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;
        }
        public List<THIETLAP_DAIBIEU_PHANTRANG> Get_Daibieu_Phantrang(String procedurename, int iDaibieu, int iKhoa, int iLoai, int page, int pagesize)
        {
            List<THIETLAP_DAIBIEU_PHANTRANG> result = new List<THIETLAP_DAIBIEU_PHANTRANG> ();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("p_DAIBIEU", iDaibieu));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_KHOA", iKhoa));
            lisparam.Add(new OracleParameter("p_LOAIDAIBIEU", iLoai));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", pagesize));
            try
            {
                result = GetListObjetReport<THIETLAP_DAIBIEU_PHANTRANG>(procedurename, lisparam);
            }
            catch
            {
                result = null;
            }
            return result;
        }
        public List<THIETLAP_COQUAN> GET_THIETLAPCOQUANKHAC(String procedurename, int idparent)
        {

            List<THIETLAP_COQUAN> resule = new List<THIETLAP_COQUAN>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("IDPARENT", idparent));
            try
            {
                resule = GetListObjetReport<THIETLAP_COQUAN>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;
        }
        public List<THIETLAP_COQUAN> GET_THIETLAPCOQUANKHAC_PHANTRANG(String procedurename, int idparent,int page,int pagesize)
        {

            List<THIETLAP_COQUAN> resule = new List<THIETLAP_COQUAN>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("IDPARENT", idparent));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", pagesize));
            try
            {
                resule = GetListObjetReport<THIETLAP_COQUAN>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;
        }
        public List<THIETLAP_COQUAN> GET_THIETLAPCOQUANGOC(String procedurename)
        {

            List<THIETLAP_COQUAN> resule = new List<THIETLAP_COQUAN>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            try
            {
                resule = GetListObjetReport<THIETLAP_COQUAN>(procedurename,lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;
        }
        public List<THIETLAP_COQUAN_PHANTRANG> GET_THIETLAPCOQUAN_PHANTRANG(String procedurename, string ctukhoa, int page, int pagesize)
        {

            List<THIETLAP_COQUAN_PHANTRANG> resule = new List<THIETLAP_COQUAN_PHANTRANG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var noidung = new OracleParameter("p_TUKHOA", OracleDbType.NVarchar2);
            noidung.Value = ctukhoa;
            lisparam.Add(noidung);
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", pagesize));
            try
            {
                resule = GetListObjetReport<THIETLAP_COQUAN_PHANTRANG>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;
        }
        public List<THIETLAP_DIAPHUONG_PHANTRANG> THIETLAPDIAPHUONG_PHANTRANG(String procedurename, string tukhoa, int page, int pagesize)
        {

            List<THIETLAP_DIAPHUONG_PHANTRANG> resule = new List<THIETLAP_DIAPHUONG_PHANTRANG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", pagesize));
            lisparam.Add(new OracleParameter("p_TUKHOA", tukhoa));
            try
            {
                resule = GetListObjetReport<THIETLAP_DIAPHUONG_PHANTRANG>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;
        }
        public List<THIETLAP_DIAPHUONG_PHANTRANG> THIETLAPDIAPHUONG(String procedurename)
        {

            List<THIETLAP_DIAPHUONG_PHANTRANG> resule = new List<THIETLAP_DIAPHUONG_PHANTRANG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
          
            try
            {
                resule = GetListObjetReport<THIETLAP_DIAPHUONG_PHANTRANG>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;
        }

        public List<TRACKING> GET_LISTLICHSUTIMKIEM(String procedurename, string ctukhoa, string dtungay = "", string ddenngay = "")
        {

            List<TRACKING> resule = new List<TRACKING>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var noidung = new OracleParameter("CTUKHOA", OracleDbType.NVarchar2);
            noidung.Value = ctukhoa;
            lisparam.Add(noidung);
            var tungay = new OracleParameter("NGAYBATDAU", OracleDbType.NVarchar2);
            tungay.Value = dtungay;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("NGAYKETHUC", OracleDbType.NVarchar2);
            denngay.Value = ddenngay;
            lisparam.Add(denngay);

            try
            {
                resule = GetListObjetReport<TRACKING>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;

        }
        public List<USERS> GET_LISTLICHSUTIMKIEMUSER(String procedurename, string ctukhoa, int idonvi  =0)
        {

            List<USERS> resule = new List<USERS>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var noidung = new OracleParameter("CTUKHOA", OracleDbType.NVarchar2);
            noidung.Value = ctukhoa;
            lisparam.Add(noidung);
            lisparam.Add(new OracleParameter("IMADONVI", idonvi));

            try
            {
                resule = GetListObjetReport<USERS>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;

        }
        public List<THIETLAP_LICHSU> LICHSU_PHANTRANG(String procedurename, string dtungay = "", string ddenngay = "", string ctukhoanoidung = "", int idonvi = 0, string ctukhoadonvi = "", int page = 0, int pagesize = 0)
        {

            List<THIETLAP_LICHSU> resule = new List<THIETLAP_LICHSU>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var noidungdonvi = new OracleParameter("CTUKHOANGUOIDUNG", OracleDbType.NVarchar2);
            noidungdonvi.Value = ctukhoadonvi;
            lisparam.Add(noidungdonvi);
            lisparam.Add(new OracleParameter("IMADONVI", idonvi));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", pagesize));
            var noidung = new OracleParameter("CTUKHOANOIDUNG", OracleDbType.NVarchar2);
            noidung.Value = ctukhoanoidung;
            lisparam.Add(noidung);
            var tungay = new OracleParameter("NGAYBATDAU", OracleDbType.NVarchar2);
            tungay.Value = dtungay;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("NGAYKETHUC", OracleDbType.NVarchar2);
            denngay.Value = ddenngay;
            lisparam.Add(denngay);
            try
            {
                resule = GetListObjetReport<THIETLAP_LICHSU>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;
        }
        public List<PHANTRANG_VANBAN> VANBAN(String procedurename, string dtungay = "", string ddenngay = "", string ctukhoa = "", int idonvi = 0, int ilinhvuc = 0,int trangthai = 0 ,int loaivb =0,int kyhop =0, int page = 0, int pagesize = 0)
        {

            List<PHANTRANG_VANBAN> resule = new List<PHANTRANG_VANBAN>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var noidungdonvi = new OracleParameter("CTUKHOA", OracleDbType.NVarchar2);
            noidungdonvi.Value = ctukhoa;
            lisparam.Add(noidungdonvi);
            lisparam.Add(new OracleParameter("IMADONVI", idonvi));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", pagesize));
          
            var tungay = new OracleParameter("NGAYBATDAU", OracleDbType.NVarchar2);
            tungay.Value = dtungay;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("NGAYKETTHUC", OracleDbType.NVarchar2);
            denngay.Value = ddenngay;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("LINHVUC", ilinhvuc));
            lisparam.Add(new OracleParameter("TRANGTHAI", trangthai));
            lisparam.Add(new OracleParameter("LOAIVANBAN", loaivb));
            lisparam.Add(new OracleParameter("KYHOP", kyhop));
            try
            {
                resule = GetListObjetReport<PHANTRANG_VANBAN>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;
        }
    }
}
