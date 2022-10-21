using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
namespace DataAccess.Dao
{

    public interface IUTD_VuviecRepository
    {
        List<TD_VUVIEC> GetAll();
        List<TD_VUVIEC> GetAll(Dictionary<string, object> condition);
        TD_VUVIEC GetByID(int ID);
        TD_VUVIEC AddNew(TD_VUVIEC UserInput);
        Boolean Update(TD_VUVIEC UserInput);
        Boolean Delete(TD_VUVIEC UserInput);
    }
    public class TD_VuviecRepository : BaseRepository, IUTD_VuviecRepository
    {
        Log log = new Log();
        public List<TD_VUVIEC> GetAll()
        {
            List<TD_VUVIEC> resule = new List<TD_VUVIEC>();
            resule = base.GetAll<TD_VUVIEC>();
            return resule;
        }
        public List<TD_VUVIEC> GetAll(Dictionary<string, object> condition)
        {
            List<TD_VUVIEC> resule = new List<TD_VUVIEC>();
            resule = base.GetAll<TD_VUVIEC>(condition);
            return resule;
        }
        public TD_VUVIEC GetByIDDon(int ID)
        {
            return base.GetItem<TD_VUVIEC>("IKNTC_DON", ID);
        }
        public List<TD_VUVIEC> GetList_Vuviec_Trung(int id_vuviec_check)
        {
            List<TD_VUVIEC> listObj = new List<TD_VUVIEC>();
            var param = new List<OracleParameter>();
            try
            {
                TD_VUVIEC v = GetByID(id_vuviec_check);
                string sql = "SELECT * FROM TD_VUVIEC WHERE IDIAPHUONG_0 = :param01 ";
                param.Add(new OracleParameter("param01", v.IDIAPHUONG_0));
                if(v.CNOIDUNG != "" && v.CNOIDUNG != null)
                {
                    sql += " AND ( UPPER(CNOIDUNG) like '%' || UPPER(:param02) || '%'";
                    var noidung = new OracleParameter("param02", OracleDbType.NVarchar2);
                    noidung.Value = v.CNOIDUNG.Trim();
                    param.Add(noidung);

                    sql += " OR UPPER( CNGUOIGUI_TEN ) LIKE '%' || UPPER(:param03) || '%' )";
                    var nguoigui = new OracleParameter("param03", OracleDbType.NVarchar2);
                    nguoigui.Value = v.CNGUOIGUI_TEN.Trim();
                    param.Add(nguoigui);
                }
                else
                {
                    sql += " AND UPPER( CNGUOIGUI_TEN ) LIKE '%' || UPPER(:param03) || '%' ";
                    var nguoigui = new OracleParameter("param03", OracleDbType.NVarchar2);
                    nguoigui.Value = v.CNGUOIGUI_TEN.Trim();
                    param.Add(nguoigui);
                }
               

                sql += " AND IVUVIEC <> :param04";
                param.Add(new OracleParameter("param04", id_vuviec_check));

                listObj = GetList<TD_VUVIEC>(sql, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Kiểm tra vụ việc trùng ");
                return null;
            }
            return listObj;
        }
        public List<KNTC_DON> List_DonTrung_KiemTrungKNTC(string tennguoi = "", int idiaphuong = 0, string noidung = "")
        {
            List<KNTC_DON> listObj = new List<KNTC_DON>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from KNTC_DON where 1 =1 ";

                if (tennguoi != "")
                {
                    sql += " and  UPPER(CNGUOIGUI_TEN) like '%' || UPPER(:param03) || '%' ";
                    var tennguoigui = new OracleParameter("param03", OracleDbType.NVarchar2);
                    tennguoigui.Value = tennguoi.Trim();
                    param.Add(tennguoigui);
                }
                if (noidung != "")
                {
                    sql += " and  UPPER(CNOIDUNG) like '%' || UPPER(:param02) || '%' ";
                    var tennoidung = new OracleParameter("param02", OracleDbType.NVarchar2);
                    tennoidung.Value = noidung.Trim();
                    param.Add(tennoidung);
                }

                if (idiaphuong != 0)
                {

                    sql += " and IDIAPHUONG_0 = " + " :param04";
                    param.Add(new OracleParameter("param04", idiaphuong));
                }
                int trangthai = 5;
                sql += " and ITINHTRANGXULY <> " + " :param05";
                param.Add(new OracleParameter("param05", trangthai));
                sql += " order by DDATE";
                listObj = base.GetList<KNTC_DON>(sql, param);
                return listObj;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách đơn khiếu nại tố cáo trùng");
                return null;
            }
        }
        public List<TD_VUVIEC> GetList_Vuviec_TrungNhanh(int idiaphuong,string noidungvao, string nguoiguivao)
        {
            List<TD_VUVIEC> listObj = new List<TD_VUVIEC>();
            var param = new List<OracleParameter>();
            try
            {
               
                string sql = "SELECT * FROM TD_VUVIEC WHERE IDIAPHUONG_0 = :param01 ";
                param.Add(new OracleParameter("param01", idiaphuong));
                if (noidungvao != "")
                {
                    sql += " AND ( UPPER(CNOIDUNG) like '%' || UPPER(:param02) || '%'";
                    var noidung = new OracleParameter("param02", OracleDbType.NVarchar2);
                    noidung.Value = noidungvao.Trim();
                    param.Add(noidung);

                    sql += " OR UPPER( CNGUOIGUI_TEN ) LIKE '%' || UPPER(:param03) || '%' )";
                    var nguoigui = new OracleParameter("param03", OracleDbType.NVarchar2);
                    nguoigui.Value = nguoiguivao.Trim();
                    param.Add(nguoigui);
                }
                else
                {
                    sql += " AND UPPER( CNGUOIGUI_TEN ) LIKE '%' || UPPER(:param03) || '%' ";
                    var nguoigui = new OracleParameter("param03", OracleDbType.NVarchar2);
                    nguoigui.Value = nguoiguivao.Trim();
                    param.Add(nguoigui);
                }
               
                listObj = GetList<TD_VUVIEC>(sql, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Kiểm tra vụ việc trùng ");
                return null;
            }
            return listObj;
        }
        public List<TD_VUVIEC> GetList_Vuviec_Trung_search(string noidung)
        {
            List<TD_VUVIEC> listObj = new List<TD_VUVIEC>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from TD_VUVIEC where UPPER(CNOIDUNG) LIKE '%' || UPPER(:param01) || '%'";
                var noidungkey = new OracleParameter("param01", OracleDbType.NVarchar2);
                noidungkey.Value = noidung.Trim();
                param.Add(noidungkey);

                listObj = base.GetList<TD_VUVIEC>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
            return listObj;
        }


        public List<TD_VUVIEC> GetList_SQL_searchdinhky(TD_VUVIEC vuviec, string tungay, string denngay)
        {
            List<TD_VUVIEC> listObj = new List<TD_VUVIEC>();
            var param = new List<OracleParameter>();
            TD_VUVIEC td = new TD_VUVIEC();
            //decimal idonvi = (decimal)vuviec.IDONVI;
            try
            {
                string sql = "select * from TD_VUVIEC where 1=1 ";
                if (vuviec.IDINHKY == 0)
                {
                    sql += "AND IDINHKY = :param01 ";
                    param.Add(new OracleParameter("param01", vuviec.IDINHKY));
                }
                else
                {
                    int IDINHKY = 0;
                    sql += "AND IDINHKY <> :param01 ";
                    param.Add(new OracleParameter("param01", IDINHKY));
                }

                if (vuviec.IDONVI != null && vuviec.IDONVI != -1)
                {
                    sql += " and IDONVI = :param02 ";
                    param.Add(new OracleParameter("param02", vuviec.IDONVI));
                }
                if (vuviec.CNGUOIGUI_TEN != null)
                {
                    sql += " and  UPPER(CNGUOIGUI_TEN) like '%' || UPPER(:param03) || '%'";
                    var noidung = new OracleParameter("param03", OracleDbType.NVarchar2);
                    noidung.Value = vuviec.CNGUOIGUI_TEN.Trim();
                    param.Add(noidung);
                }
                if (vuviec.CNGUOIGUI_DIACHI != null)
                {
                    sql += " and UPPER(CNGUOIGUI_DIACHI) like '%' || UPPER(:param04) || '%'";
                    var noidung = new OracleParameter("param04", OracleDbType.NVarchar2);
                    noidung.Value = vuviec.CNGUOIGUI_DIACHI.Trim();
                    param.Add(noidung);
                }
                if (vuviec.CNOIDUNG != null)
                {
                    sql += " and UPPER(CNOIDUNG) like '%' || UPPER(:param05) || '%'";
                    var noidung = new OracleParameter("param05", OracleDbType.NVarchar2);
                    noidung.Value = vuviec.CNOIDUNG.Trim();
                    param.Add(noidung);
                }
                if (vuviec.ILOAIDON != null)
                {
                    sql += " and ILOAIDON = :param06 ";
                    param.Add(new OracleParameter("param06", vuviec.ILOAIDON));
                }
                if (vuviec.ILINHVUC != null)
                {
                    sql += " and ILINHVUC = :param07 ";
                    param.Add(new OracleParameter("param07", vuviec.ILINHVUC));
                }
                if (vuviec.INOIDUNG != null)
                {
                    sql += " and INOIDUNG = :param08 ";
                    param.Add(new OracleParameter("param08", vuviec.INOIDUNG));
                }
                if (vuviec.ITINHCHAT != null)
                {
                    sql += " and ITINHCHAT = :param08 ";
                    param.Add(new OracleParameter("param08", vuviec.ITINHCHAT));
                }
                if (vuviec.ITINHTRANGXULY != null)
                {
                    sql += " and ITINHTRANGXULY = :param09 ";
                    param.Add(new OracleParameter("param09", vuviec.ITINHTRANGXULY));
                }
                if (vuviec.IVUVIECTRUNG != null)
                {

                    sql += " and IVUVIECTRUNG <> :param10 ";
                    param.Add(new OracleParameter("param10", vuviec.IVUVIECTRUNG));
                }
                if (tungay != "")
                {
                    sql += " and DDATE >= :param11 ";
                    var noidung = new OracleParameter("param11", OracleDbType.Date);
                    noidung.Value = tungay;
                    param.Add(noidung);
                }
                if (tungay != "")
                {
                    sql += " and DNGAYNHAN >= :param11 ";
                    var noidung = new OracleParameter("param11", OracleDbType.Date);
                    noidung.Value = tungay;
                    param.Add(noidung);
                }
                if (denngay != "")
                {
                    sql += " and DNGAYNHAN <= :param12 ";
                    var noidung2 = new OracleParameter("param12", OracleDbType.Date);
                    noidung2.Value = denngay;
                    param.Add(noidung2);

                }
                listObj = base.GetList<TD_VUVIEC>(sql, param);
            }
            catch (Exception ex)
            {

            }
            return listObj;
        }
        public List<TD_VUVIEC> GetList_SQL_searchdotxuat(TD_VUVIEC vuviec, string tungay, string denngay)
        {
            List<TD_VUVIEC> listObj = new List<TD_VUVIEC>();
            var param = new List<OracleParameter>();
            TD_VUVIEC td = new TD_VUVIEC();
            //decimal idonvi = (decimal)vuviec.IDONVI;
            try
            {
                string sql = "select * from TD_VUVIEC where 1=1 AND ITIEPDOTXUAT <> 0 ";
                if (vuviec.IDINHKY == 0)
                {
                    sql += "AND IDINHKY = :param01 ";
                    param.Add(new OracleParameter("param01", vuviec.IDINHKY));
                }
                else
                {
                    int IDINHKY = 0;
                    sql += "AND IDINHKY <> :param01 ";
                    param.Add(new OracleParameter("param01", IDINHKY));
                }

                if (vuviec.IDONVI != null && vuviec.IDONVI != -1)
                {
                    sql += " and IDONVI = :param02 ";
                    param.Add(new OracleParameter("param02", vuviec.IDONVI));
                }
                if (vuviec.CNGUOIGUI_TEN != null)
                {
                    sql += " and  UPPER(CNGUOIGUI_TEN) like '%' || UPPER(:param03) || '%'";
                    var noidung = new OracleParameter("param03", OracleDbType.NVarchar2);
                    noidung.Value = vuviec.CNGUOIGUI_TEN.Trim();
                    param.Add(noidung);
                }
                if (vuviec.CNGUOIGUI_DIACHI != null)
                {
                    sql += " and UPPER(CNGUOIGUI_DIACHI) like '%' || UPPER(:param04) || '%'";
                    var noidung = new OracleParameter("param04", OracleDbType.NVarchar2);
                    noidung.Value = vuviec.CNGUOIGUI_DIACHI.Trim();
                    param.Add(noidung);
                }
                if (vuviec.CNOIDUNG != null)
                {
                    sql += " and UPPER(CNOIDUNG) like '%' || UPPER(:param05) || '%'";
                    var noidung = new OracleParameter("param05", OracleDbType.NVarchar2);
                    noidung.Value = vuviec.CNOIDUNG.Trim();
                    param.Add(noidung);
                }
                if (vuviec.ILOAIDON != null)
                {
                    sql += " and ILOAIDON = :param06 ";
                    param.Add(new OracleParameter("param06", vuviec.ILOAIDON));
                }
                if (vuviec.ILINHVUC != null)
                {
                    sql += " and ILINHVUC = :param07 ";
                    param.Add(new OracleParameter("param07", vuviec.ILINHVUC));
                }
                if (vuviec.INOIDUNG != null)
                {
                    sql += " and INOIDUNG = :param08 ";
                    param.Add(new OracleParameter("param08", vuviec.INOIDUNG));
                }
                if (vuviec.ITINHCHAT != null)
                {
                    sql += " and ITINHCHAT = :param08 ";
                    param.Add(new OracleParameter("param08", vuviec.ITINHCHAT));
                }
                if (vuviec.ITINHTRANGXULY != null)
                {
                    sql += " and ITINHTRANGXULY = :param09 ";
                    param.Add(new OracleParameter("param09", vuviec.ITINHTRANGXULY));
                }
                if (vuviec.IVUVIECTRUNG != null)
                {

                    sql += " and IVUVIECTRUNG <> :param10 ";
                    param.Add(new OracleParameter("param10", vuviec.IVUVIECTRUNG));
                }
                if (tungay != "")
                {
                    sql += " and DDATE >= :param11 ";
                    var noidung = new OracleParameter("param11", OracleDbType.Date);
                    noidung.Value = tungay;
                    param.Add(noidung);
                }
                if (tungay != "")
                {
                    sql += " and DNGAYNHAN >= :param11 ";
                    var noidung = new OracleParameter("param11", OracleDbType.Date);
                    noidung.Value = tungay;
                    param.Add(noidung);
                }
                if (denngay != "")
                {
                    sql += " and DNGAYNHAN <= :param12 ";
                    var noidung2 = new OracleParameter("param12", OracleDbType.Date);
                    noidung2.Value = denngay;
                    param.Add(noidung2);

                }
                listObj = base.GetList<TD_VUVIEC>(sql, param);
            }
            catch (Exception ex)
            {

            }
            return listObj;
        }
        public List<TD_VUVIEC> GetList_SQL_searchdinhky_search2(TD_VUVIEC vuviec, string tungay, string denngay)
        {
            List<TD_VUVIEC> listObj = new List<TD_VUVIEC>();
            var param = new List<OracleParameter>();
            TD_VUVIEC td = new TD_VUVIEC();
            //decimal idonvi = (decimal)vuviec.IDONVI;
            try
            {
                string sql = "select * from TD_VUVIEC where 1=1 ";
                if (vuviec.IDINHKY == 0)
                {
                    sql += "AND IDINHKY = :param01 ";
                    param.Add(new OracleParameter("param01", vuviec.IDINHKY));
                }
                else
                {
                    decimal IDINHKY = (decimal)vuviec.IDINHKY;
                    sql += "AND IDINHKY = :param01 ";
                    param.Add(new OracleParameter("param01", vuviec.IDINHKY));
                }

                if (vuviec.IDONVI != null && vuviec.IDONVI != -1)
                {
                    sql += " and IDONVI = :param02 ";
                    param.Add(new OracleParameter("param02", vuviec.IDONVI));
                }
                if (vuviec.CNGUOIGUI_TEN != null)
                {
                    sql += " and  UPPER(CNGUOIGUI_TEN) like '%' || UPPER(:param03) || '%'";
                    var noidung = new OracleParameter("param03", OracleDbType.NVarchar2);
                    noidung.Value = vuviec.CNGUOIGUI_TEN.Trim();
                    param.Add(noidung);
                }
                if (vuviec.CNGUOIGUI_DIACHI != null)
                {
                    sql += " and UPPER(CNGUOIGUI_DIACHI) like '%' || UPPER(:param04) || '%'";
                    var noidung = new OracleParameter("param04", OracleDbType.NVarchar2);
                    noidung.Value = vuviec.CNGUOIGUI_DIACHI.Trim();
                    param.Add(noidung);
                }
                if (vuviec.CNOIDUNG != null)
                {
                    sql += " and UPPER(CNOIDUNG) like '%' || UPPER(:param05) || '%'";
                    var noidung = new OracleParameter("param05", OracleDbType.NVarchar2);
                    noidung.Value = vuviec.CNOIDUNG.Trim();
                    param.Add(noidung);
                }
                if (vuviec.ILOAIDON != null)
                {
                    sql += " and ILOAIDON = :param06 ";
                    param.Add(new OracleParameter("param06", vuviec.ILOAIDON));
                }
                if (vuviec.ILINHVUC != null)
                {
                    sql += " and ILINHVUC = :param07 ";
                    param.Add(new OracleParameter("param07", vuviec.ILINHVUC));
                }
                if (vuviec.INOIDUNG != null)
                {
                    sql += " and INOIDUNG = :param08 ";
                    param.Add(new OracleParameter("param08", vuviec.INOIDUNG));
                }
                if (vuviec.ITINHCHAT != null)
                {
                    sql += " and ITINHCHAT = :param08 ";
                    param.Add(new OracleParameter("param08", vuviec.ITINHCHAT));
                }
                if (vuviec.ITINHTRANGXULY != null)
                {
                    sql += " and ITINHTRANGXULY = :param09 ";
                    param.Add(new OracleParameter("param09", vuviec.ITINHTRANGXULY));
                }
                if (vuviec.IVUVIECTRUNG != null)
                {

                    sql += " and IVUVIECTRUNG <> :param10 ";
                    param.Add(new OracleParameter("param10", vuviec.IVUVIECTRUNG));
                }
                if (tungay != "")
                {
                    sql += " and DDATE >= :param11 ";
                    var noidung = new OracleParameter("param11", OracleDbType.Date);
                    noidung.Value = tungay;
                    param.Add(noidung);
                }
                if (tungay != "")
                {
                    sql += " and DNGAYNHAN >= :param11 ";
                    var noidung = new OracleParameter("param11", OracleDbType.Date);
                    noidung.Value = tungay;
                    param.Add(noidung);
                }
                if (denngay != "")
                {
                    sql += " and DNGAYNHAN <= :param12 ";
                    var noidung2 = new OracleParameter("param12", OracleDbType.Date);
                    noidung2.Value = denngay;
                    param.Add(noidung2);

                }
                listObj = base.GetList<TD_VUVIEC>(sql, param);
            }
            catch (Exception ex)
            {

            }
            return listObj;
        }
       
        public List<TD_VUVIEC> List_Dinhky()
        {
            string str = "select  TD_VUVIEC.* from TD_VUVIEC where IDINHKY <> 0";

            List<TD_VUVIEC> resule = base.GetList<TD_VUVIEC>(str);
            return resule;
        }
        public List<TD_VUVIEC> Delete_Vuviec(int id)
        {
            string str = "delete from TD_VUVIEC where IVUVIEC=" + id + "";

            List<TD_VUVIEC> resule = base.GetList<TD_VUVIEC>(str);
            return resule;
        }

        public TD_VUVIEC GetByID(int ID)
        {
            return base.GetItem<TD_VUVIEC>("IVUVIEC", ID);
        }
        public TD_VUVIEC AddNew(TD_VUVIEC actionInput)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("TD_VUVIEC_SEQ");
            if (ID != 0)
            {
                actionInput.IVUVIEC = ID;
                if (base.InsertItem<TD_VUVIEC>(actionInput))
                {
                    return actionInput;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        public Boolean Update(TD_VUVIEC actionInput)
        {

            if (base.UpdateItem<TD_VUVIEC>(actionInput, "IVUVIEC"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(TD_VUVIEC actionInput)
        {

            if (base.DeleteItem<TD_VUVIEC>(actionInput, "IVUVIEC"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}