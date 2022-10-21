using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    
    public interface ITRACKINGRepository
    {
        TRACKING GetByID(int ID);
        List<TRACKING> GetAll();
        List<TRACKING> GetAll(Dictionary<string, object> condition);
        TRACKING GetByUseName(String Username);
        TRACKING AddNew(TRACKING UserInput);
        Boolean Update(TRACKING UserInput);
        Boolean Delete(TRACKING UserInput);
        List<TRACKING> Getlist(String sql);
    }
    public class TrackingRepository : BaseRepository, ITRACKINGRepository
    {
        public List<TRACKING> GetAll()
        {

            List<TRACKING> resule = new List<TRACKING>();
            resule = base.GetAll<TRACKING>();
            return resule;
        }
        public List<TRACKING> GetList_TRACKING_search(string key, string tungay, string denngay)
        {
            List<TRACKING> listObj = new List<TRACKING>();
            var param = new List<OracleParameter>();
            try
            {
                
                string sql = "select * from TRACKING where  1=1 ";
                if (key.Trim() != "")
                {
                    sql+=" and UPPER(CACTION) like N'%' || upper(:param01) || '%' ";
                    var noidungkey = new OracleParameter("param01", OracleDbType.NVarchar2);
                    noidungkey.Value = key.Trim().ToUpper();
                    param.Add(noidungkey);
                }               
                if (tungay != null && tungay != "")
                {
                    sql += "and DDATE >= :param03 ";
                    var noidung = new OracleParameter("param03", OracleDbType.Date);
                    noidung.Value = tungay;
                    param.Add(noidung);
                }
                if (denngay != null && denngay != "")
                {
                    sql += "and DDATE <= :param04 ";
                    var noidung = new OracleParameter("param04", OracleDbType.Date);
                    noidung.Value = denngay;
                    param.Add(noidung);
                }
               
                listObj = base.GetList<TRACKING>(sql, param);
            }
            catch (Exception)
            {
                // ghi log lỗi tại đây
                throw;

            }
            return listObj;

        }
        public List<TRACKING> GetAll(Dictionary<string, object> condition)
        {

            List<TRACKING> resule = new List<TRACKING>();
            resule = base.GetAll<TRACKING>(condition);
            return resule;
        }
        public List<TRACKING> Getlist(String sql)
        {
            List<TRACKING> resule = new List<TRACKING>();
            resule = base.GetList<TRACKING>(sql);
            return resule;
        }
        public TRACKING GetByID(int ID)
        {
            return base.GetItem<TRACKING>("ITRACKING", ID);
        }
        public TRACKING GetByUseName(String actionInput)
        {
            return base.GetItem<TRACKING>("ITRACKING", actionInput);
        }
        public TRACKING AddNew(TRACKING actionInput)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("TRACKING_SEQ");
            if (ID != 0)
            {
                actionInput.ID = ID;
                if (base.InsertItem<TRACKING>(actionInput))
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
        public Boolean Update(TRACKING actionInput)
        {

            if (base.UpdateItem<TRACKING>(actionInput, "ITRACKING"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(TRACKING actionInput)
        {

            if (base.DeleteItem<TRACKING>(actionInput, "ITRACKING"))
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
