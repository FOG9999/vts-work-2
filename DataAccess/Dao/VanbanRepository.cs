using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utilities;
namespace DataAccess.Dao
{
   
    public interface IUVanbanRepository
    {
        VB_VANBAN GetByID(int ID);
        List<VB_VANBAN> GetAll();
        List<VB_VANBAN> GetAll(Dictionary<string, object> dictionary);
        VB_VANBAN AddNew(VB_VANBAN UserInput);
        Boolean Update(VB_VANBAN UserInput);
        Boolean Delete(VB_VANBAN UserInput);
        List<VB_VANBAN> GetList(String sql);

    }
    public class VanbanRepository : BaseRepository, IUVanbanRepository
    {
        Log log = new Log();
        public List<VB_VANBAN> GetAll()
        {
            List<VB_VANBAN> resule = new List<VB_VANBAN>();
            resule = base.GetAll<VB_VANBAN>();
            return resule;
        }
        public List<VB_VANBAN> GetAll(Dictionary<string, object> dictionary)
        {
            List<VB_VANBAN> resule = new List<VB_VANBAN>();
            resule = base.GetAll<VB_VANBAN>(dictionary);

            return resule;
        }
        public List<VB_VANBAN> GetList(String sql)
        {
            List<VB_VANBAN> resule = new List<VB_VANBAN>();
            resule = base.GetList<VB_VANBAN>(sql);
            return resule;
        }

        public List<VB_VANBAN> GetList_vanban_search_ten(string cten)
        {
            List<VB_VANBAN> listObj = new List<VB_VANBAN>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from VB_VANBAN where IHIENTHI != " + " :param01";
                param.Add(new OracleParameter("param01", 5));
                sql += " and ( UPPER(CTIEUDE) like '%' || UPPER(:param02) || '%' or  UPPER(CTRICHYEU) like  '%' || UPPER(:param02) ||'%' )";
                var noidungkey = new OracleParameter("param02", OracleDbType.NVarchar2);
                noidungkey.Value = cten.Trim();
                param.Add(noidungkey);
                listObj = base.GetList<VB_VANBAN>(sql, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Văn bản tra cứu tên");
                return null;
            }
            return listObj;
        }
        public List<VB_VANBAN> GetList_vanban_search(VB_VANBAN d, int idonvi, string tungay, string denngay)
        {
            List<VB_VANBAN> listObj = new List<VB_VANBAN>();
            var param = new List<OracleParameter>();
            try
            {
                int idonvi_ = idonvi;
                string sql = "select distinct VB_VANBAN.* from ";
                if (idonvi != -1)
                {
                    sql += " VB_VANBAN inner join  VB_DONVI_VANBAN on VB_VANBAN.IVANBAN = VB_DONVI_VANBAN.IVANBAN ";
                    sql += " and VB_DONVI_VANBAN.IDONVI = :param07 ";
                    param.Add(new OracleParameter("param07", idonvi_));
                }
                else
                {
                    sql += " VB_VANBAN WHERE 1 = 1";
                }

                sql += " AND  ( UPPER(VB_VANBAN.CTIEUDE) like N'%' || upper(:param01) || '%' or  UPPER(VB_VANBAN.CTRICHYEU) like N'%' || upper(:param01) || '%')";
                var noidungkey = new OracleParameter("param01", OracleDbType.NVarchar2);
                noidungkey.Value = d.CTIEUDE.Trim().ToUpper();
                param.Add(noidungkey);
                if (d.IHIENTHI == 2)
                {
                    sql += " and VB_VANBAN.IHIENTHI != :param02 ";
                    param.Add(new OracleParameter("param02", 2));
                }
                else
                {
                    sql += " and VB_VANBAN.IHIENTHI = :param02 ";
                    param.Add(new OracleParameter("param02", d.IHIENTHI));
                }
                if (tungay != null && tungay != "")
                {
                    sql += "and VB_VANBAN.DDATE >= :param03 ";
                    var noidung = new OracleParameter("param03", OracleDbType.Date);
                    noidung.Value = tungay;
                    param.Add(noidung);
                }
                if (denngay != null && denngay != "")
                {

                    sql += "and VB_VANBAN.DDATE <= :param04 ";
                    var noidung = new OracleParameter("param04", OracleDbType.Date);
                    noidung.Value = denngay;
                    param.Add(noidung);
                }
                if (d.ILOAI > 0)
                {
                    sql += " and VB_VANBAN.ILOAI = :param05 ";
                    param.Add(new OracleParameter("param05", d.ILOAI));
                }
                if (d.ILINHVUC > 0)
                {
                    sql += " and VB_VANBAN.ILINHVUC = :param06 ";
                    param.Add(new OracleParameter("param06", d.ILINHVUC));
                }
                if (idonvi > 0)
                {
                    sql += " and VB_DONVI_VANBAN.IDONVI = :param07 ";
                    param.Add(new OracleParameter("param07", idonvi_));
                }
                if (d.IKYHOP > 0)
                {
                    sql += " and VB_VANBAN.IKYHOP = :param08 ";
                    param.Add(new OracleParameter("param08", d.IKYHOP));
                }

                listObj = base.GetList<VB_VANBAN>(sql, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Văn bản tra cứu tên");
                return null;

            }
            return listObj;

        }

        public VB_VANBAN GetByID(int ID)
        {
            return base.GetItem<VB_VANBAN>("IVANBAN", ID);
        }
        public VB_VANBAN AddNew(VB_VANBAN Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("VB_VANBAN_SEQ");
            if (ID != 0)
            {
                Input.IVANBAN = ID;
                if (base.InsertItem<VB_VANBAN>(Input))
                {

                    return Input;
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
        public Boolean Update(VB_VANBAN actionInput)
        {

            if (base.UpdateItem<VB_VANBAN>(actionInput, "IVANBAN"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(VB_VANBAN actionInput)
        {

            if (base.DeleteItem<VB_VANBAN>(actionInput, "IVANBAN"))
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
