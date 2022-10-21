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
    public class TiepdanReport:BaseRepository
    {
        public List<TIEPDAN_PHULUC> getReportBaoBaoThongKeTiepDanPhuLuc(String procedurename, string pram01, string pram02, int iLoaiDon, int iLinhVuc,int iDonvi)
        {

            List<TIEPDAN_PHULUC> resule = new List<TIEPDAN_PHULUC>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iLoaiDon));
            lisparam.Add(new OracleParameter("pram04", iLinhVuc));
            lisparam.Add(new OracleParameter("pram05", iDonvi));
            try
            {
                resule = GetListObjetReport<TIEPDAN_PHULUC>(procedurename, lisparam);
            }

            catch
            {
                resule = null;

            }
            return resule;

        }
        public List<TIEPDAN_SOLIEUTINH> getReportBaoBaoThongKeSoLieuTinh(String procedurename, string pram01, string pram02, int iLoaiDon, int iLinhVuc, int iDonvi)
        {
            List<TIEPDAN_SOLIEUTINH> resule = new List<TIEPDAN_SOLIEUTINH>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("pram01", OracleDbType.Date);
            tungay.Value = pram01;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("pram02", OracleDbType.Date);
            denngay.Value = pram02;
            lisparam.Add(denngay);
            lisparam.Add(new OracleParameter("pram03", iLoaiDon));
            lisparam.Add(new OracleParameter("pram04", iLinhVuc));
            lisparam.Add(new OracleParameter("pram05", iDonvi));

            try
            {
                resule = GetListObjetReport<TIEPDAN_SOLIEUTINH>(procedurename, lisparam);
            }

            catch
            {
                resule = null;

            }
            return resule;

        }
        public List<TIEPCONGDAN_DANHMUC> getListDanhSachTiepCongDan(String procedurename)
        {
            List<OracleParameter> lisparam = new List<OracleParameter>();
            List<TIEPCONGDAN_DANHMUC> resule = new List<TIEPCONGDAN_DANHMUC>();
            try
            {
                resule = GetListObjetReport<TIEPCONGDAN_DANHMUC>(procedurename, lisparam);
            }

            catch
            {
                resule = null;

            }
            return resule;

        }
        public List<TIEPCONGDAN_TRACUU_VUVIEC> GetListTraCuu(String procedurename, string ctukhoa)
        {

            List<TIEPCONGDAN_TRACUU_VUVIEC> resule = new List<TIEPCONGDAN_TRACUU_VUVIEC>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var noidung = new OracleParameter("CTUKHOA", OracleDbType.NVarchar2);
            noidung.Value = ctukhoa;
            lisparam.Add(noidung);
            try
            {
                resule = GetListObjetReport<TIEPCONGDAN_TRACUU_VUVIEC>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;

        }
        public List<TIEPCONGDAN_LICHTIEPDINHKY> GetListLichTiepDinhKy(String procedurename, string ctukhoa,int page,int pagesize, int donvi,int iuser)
        {

            List<TIEPCONGDAN_LICHTIEPDINHKY> resule = new List<TIEPCONGDAN_LICHTIEPDINHKY>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var noidung = new OracleParameter("CTUKHOA", OracleDbType.NVarchar2);
            noidung.Value = ctukhoa;
            lisparam.Add(noidung);
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", pagesize));
            lisparam.Add(new OracleParameter("MADONVI", donvi));
            lisparam.Add(new OracleParameter("NGUOIDUNG", iuser));
            try
            {
                resule = GetListObjetReport<TIEPCONGDAN_LICHTIEPDINHKY>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;

        }

        public List<TIEPCONGDAN_TRACUU_VUVIEC> GetListTraCuu_NangCao(String procedurename, TD_VUVIEC vuviec,string dtungay="",string ddenngay ="")
        {

            List<TIEPCONGDAN_TRACUU_VUVIEC> resule = new List<TIEPCONGDAN_TRACUU_VUVIEC>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("NGAYBATDAU", OracleDbType.NVarchar2);
            tungay.Value = dtungay;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("NGAYKETHUC", OracleDbType.NVarchar2);
            denngay.Value = ddenngay;
            lisparam.Add(denngay);
            var tencongdan = new OracleParameter("CTENCONGDAN", OracleDbType.NVarchar2);
            tencongdan.Value = vuviec.CNGUOIGUI_TEN;
            lisparam.Add(tencongdan);
            var diachi = new OracleParameter("CDIACHI", OracleDbType.NVarchar2);
            diachi.Value = vuviec.CNGUOIGUI_DIACHI;
            lisparam.Add(diachi);
            var tomtat = new OracleParameter("CTOMTAT", OracleDbType.NVarchar2);
            tomtat.Value = vuviec.CNOIDUNG;
            lisparam.Add(tomtat);
            lisparam.Add(new OracleParameter("IDOANDONGNGUOITRACUU", vuviec.IDOANDONGNGUOI));
            lisparam.Add(new OracleParameter("ICOQUANTIEP", vuviec.IDONVI));
            lisparam.Add(new OracleParameter("ILOAIDONTRACUU", vuviec.ILOAIDON));
            lisparam.Add(new OracleParameter("ILINHVUCTRACUU",vuviec.ILINHVUC));
            lisparam.Add(new OracleParameter("INHOMNOIDUNGTRACUU", vuviec.INOIDUNG));
            lisparam.Add(new OracleParameter("ITINHCHATVUVIEC",vuviec.ITINHCHAT));
            lisparam.Add(new OracleParameter("ITINHTRANGXULYTRACUU", vuviec.ITINHTRANGXULY));
            lisparam.Add(new OracleParameter("IKIEMTRUNG",vuviec.IVUVIECTRUNG));
            lisparam.Add(new OracleParameter("ILOAIVUVIEC", vuviec.IDINHKY));
            lisparam.Add(new OracleParameter("TIEPDOTXUAT", vuviec.ITIEPDOTXUAT));

            try
            {
                resule = GetListObjetReport<TIEPCONGDAN_TRACUU_VUVIEC>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;

        }

        public List<TIEPCONGDAN_TRACUU_VUVIEC> ListTraCuu(String procedurename, TD_VUVIEC vuviec, string dtungay = "", string ddenngay = "", int page=0, int pagesize=0)
        {

            List<TIEPCONGDAN_TRACUU_VUVIEC> resule = new List<TIEPCONGDAN_TRACUU_VUVIEC>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            var tungay = new OracleParameter("NGAYBATDAU", OracleDbType.NVarchar2);
            tungay.Value = dtungay;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("NGAYKETHUC", OracleDbType.NVarchar2);
            denngay.Value = ddenngay;
            lisparam.Add(denngay);
            var tencongdan = new OracleParameter("CTENCONGDAN", OracleDbType.NVarchar2);
            tencongdan.Value = vuviec.CNGUOIGUI_TEN;
            lisparam.Add(tencongdan);
            var diachi = new OracleParameter("CDIACHI", OracleDbType.NVarchar2);
            diachi.Value = vuviec.CNGUOIGUI_DIACHI;
            lisparam.Add(diachi);
            var tomtat = new OracleParameter("CTOMTAT", OracleDbType.NVarchar2);
            tomtat.Value = vuviec.CNOIDUNG;
            lisparam.Add(tomtat);
            lisparam.Add(new OracleParameter("IDOANDONGNGUOITRACUU", vuviec.IDOANDONGNGUOI));
            lisparam.Add(new OracleParameter("ICOQUANTIEP", vuviec.IDONVI));
            lisparam.Add(new OracleParameter("ILOAIDONTRACUU", vuviec.ILOAIDON));
            lisparam.Add(new OracleParameter("ILINHVUCTRACUU", vuviec.ILINHVUC));
            lisparam.Add(new OracleParameter("INHOMNOIDUNGTRACUU", vuviec.INOIDUNG));
            lisparam.Add(new OracleParameter("ITINHCHATVUVIEC", vuviec.ITINHCHAT));
            lisparam.Add(new OracleParameter("ITINHTRANGXULYTRACUU", vuviec.ITINHTRANGXULY));
            lisparam.Add(new OracleParameter("IKIEMTRUNG", vuviec.IVUVIECTRUNG));
            lisparam.Add(new OracleParameter("ILOAIVUVIEC", vuviec.IDINHKY));
            lisparam.Add(new OracleParameter("TIEPDOTXUAT", vuviec.ITIEPDOTXUAT));
            lisparam.Add(new OracleParameter("IDUSER", vuviec.IUSER));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", pagesize));

            try
            {
                resule = GetListObjetReport<TIEPCONGDAN_TRACUU_VUVIEC>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;

        }





        public List<TIEPCONGDAN_DANHMUC> GETLISTTIEPCONGDAN(String procedurename, TD_VUVIEC vuviec, string dtungay = "", string ddenngay = "", int page = 0, int pagesize = 0,string ctukhoa="",int iloaivuviec = 0,int ikiemtrung = 0,int idinhky = 0,int iuser= 0,int idonvi= 0,int tiepdotxuat= 0)
        {

            List<TIEPCONGDAN_DANHMUC> resule = new List<TIEPCONGDAN_DANHMUC>();
            List<OracleParameter> lisparam = new List<OracleParameter>();

            //
            var tungay = new OracleParameter("NGAYBATDAU", OracleDbType.NVarchar2);
            tungay.Value = dtungay;
            lisparam.Add(tungay);
            var denngay = new OracleParameter("NGAYKETHUC", OracleDbType.NVarchar2);
            denngay.Value = ddenngay;
            lisparam.Add(denngay);
            //
            lisparam.Add(new OracleParameter("IDOANDONGNGUOITRACUU", -1));
            lisparam.Add(new OracleParameter("ICOQUANTIEP",idonvi));
            //
            var tencongdan = new OracleParameter("CTENCONGDAN", OracleDbType.NVarchar2);
            tencongdan.Value = vuviec.CNGUOIGUI_TEN;
            lisparam.Add(tencongdan);
            var diachi = new OracleParameter("CDIACHI", OracleDbType.NVarchar2);
            diachi.Value = vuviec.CNGUOIGUI_DIACHI;
            lisparam.Add(diachi);
            var tomtat = new OracleParameter("CTOMTAT", OracleDbType.NVarchar2);
            tomtat.Value = vuviec.CNOIDUNG;
            lisparam.Add(tomtat);
            var tukhoa = new OracleParameter("CTUKHOA", OracleDbType.NVarchar2);
            tukhoa.Value = ctukhoa;
            lisparam.Add(tukhoa);
            //
            lisparam.Add(new OracleParameter("ILOAIDONTRACUU", vuviec.ILOAIDON));
            lisparam.Add(new OracleParameter("ILINHVUCTRACUU", vuviec.ILINHVUC));
            lisparam.Add(new OracleParameter("INHOMNOIDUNGTRACUU", vuviec.INOIDUNG));
            lisparam.Add(new OracleParameter("ITINHCHATVUVIEC", vuviec.ITINHCHAT));
            lisparam.Add(new OracleParameter("ITINHTRANGXULYTRACUU", vuviec.ITINHTRANGXULY));
            lisparam.Add(new OracleParameter("IKIEMTRUNG", ikiemtrung));
            lisparam.Add(new OracleParameter("ILOAIVUVIEC", iloaivuviec));
            lisparam.Add(new OracleParameter("TIEPDOTXUAT", tiepdotxuat));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", pagesize));
            lisparam.Add(new OracleParameter("NGUOIDUNG", iuser));
            lisparam.Add(new OracleParameter("MADINHKY", idinhky));
          

          
          
          
            try
            {
                resule = GetListObjetReport<TIEPCONGDAN_DANHMUC>(procedurename, lisparam);
            }
            catch
            {
                resule = null;
            }
            return resule;

        }

    }
}
