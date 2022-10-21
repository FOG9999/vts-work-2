using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utilities;
using Entities.Objects;
namespace DataAccess.Dao
{

    public interface IUQuochoi_CoquanRepository
    {
        List<QUOCHOI_COQUAN> GetAll();
        List<QUOCHOI_COQUAN> GetAll_By(string CCODE);
        List<QUOCHOI_COQUAN> GetAll_By(string CCODE, int ICOQUAN);
        List<QUOCHOI_COQUAN> GetAll_ByIparent(int IPARENT);
        List<QUOCHOI_COQUAN> GetAll(Dictionary<string, object> condition);
        QUOCHOI_COQUAN GetByID(int ID);
        QUOCHOI_COQUAN AddNew(QUOCHOI_COQUAN Input);
        Boolean Update(QUOCHOI_COQUAN Input);
        Boolean Delete(QUOCHOI_COQUAN Input);
        List<QUOCHOI_COQUAN> GetList(String sql);
        List<QUOCHOI_COQUAN> GetQuocHoiCoQuanTreeList();
    }
    public class Quochoi_CoquanRepository : BaseRepository, IUQuochoi_CoquanRepository
    {
        public List<QUOCHOI_COQUAN> GetAll()
        {
            List<QUOCHOI_COQUAN> resule = new List<QUOCHOI_COQUAN>();
            resule = base.GetAll<QUOCHOI_COQUAN>();
            return resule;
        }
        public List<QUOCHOI_COQUAN> GetList(String sql)
        {
            List<QUOCHOI_COQUAN> resule = new List<QUOCHOI_COQUAN>();
            resule = base.GetList<QUOCHOI_COQUAN>(sql);
            return resule;
        }
       
        public List<QUOCHOI_COQUAN> GetList_CheckMaCoQuan_Update(string code, int id)
        {

            List<QUOCHOI_COQUAN> listObj = new List<QUOCHOI_COQUAN>();
            var param = new List<OracleParameter>();
            try
            {
                int iconquan = id;
                string sql = "SELECT * from QUOCHOI_COQUAN where ICOQUAN != " + " :param01";
                param.Add(new OracleParameter("param01", iconquan));
                sql += " and  UPPER(CCODE) =:param02";
                var noidungkey = new OracleParameter("param02", OracleDbType.NVarchar2);
                noidungkey.Value = code.Trim().ToUpper();
                param.Add(noidungkey);
                //sql += " and  ICOQUAN !=:param03";
                //param.Add(new OracleParameter("param03", id));
                listObj = base.GetList<QUOCHOI_COQUAN>(sql, param);

               
            }
            catch (Exception)
            {
                throw;
            }
            return listObj;
        }
       
        public List<QUOCHOI_COQUAN> GetList_IParentList(int ipraent, int idonvi)
        {

            List<QUOCHOI_COQUAN> listObj = new List<QUOCHOI_COQUAN>();
            var param = new List<OracleParameter>();
            try
            {
                int ipraent_ = ipraent;
                int idonvi_ = idonvi;
                string sql = "SELECT * from QUOCHOI_COQUAN where IPARENT =  :param01 ";
                param.Add(new OracleParameter("param01", ipraent_));
                sql += " and ICOQUAN !=  :param02";
                param.Add(new OracleParameter("param02", idonvi_));
              
                listObj = base.GetList<QUOCHOI_COQUAN>(sql, param);


            }
            catch (Exception)
            {
                throw;
            }
            return listObj;
        }
        public List<QUOCHOI_COQUAN> GetList_CheckMaCoQuan_Insert(string code)
        {
            List<QUOCHOI_COQUAN> listObj = new List<QUOCHOI_COQUAN>();
            var param = new List<OracleParameter>();
            try
            {
                int idel = 0;
                string sql = "SELECT * from QUOCHOI_COQUAN where IDELETE = :param01";
                param.Add(new OracleParameter("param01", idel));
                sql += " and  UPPER(CCODE) = :param02";
                var noidungkey = new OracleParameter("param02", OracleDbType.NVarchar2);
                noidungkey.Value = code.Trim().ToUpper() ;
                param.Add(noidungkey);
                listObj = base.GetList<QUOCHOI_COQUAN>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
            return listObj;
        }
        public List<ID_Parent> GetListIDParent(String sql)
        {
            List<ID_Parent> resule = new List<ID_Parent>();
            resule = base.GetList<ID_Parent>(sql);
            return resule;
        }
        public List<QUOCHOI_COQUAN> GetAll(Dictionary<string, object> condition)
        {
            List<QUOCHOI_COQUAN> resule = new List<QUOCHOI_COQUAN>();
            resule = base.GetAll<QUOCHOI_COQUAN>(condition);
            return resule;
        }
        public List<QUOCHOI_COQUAN> GetQuocHoiCoQuanTreeList()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM QUOCHOI_COQUAN WHERE 1=1 AND IDELETE = 0");
            sql.Append("START WITH ICOQUAN in (SELECT ICOQUAN FROM QUOCHOI_COQUAN WHERE IPARENT=0 ) ");
            sql.Append("CONNECT BY PRIOR ICOQUAN=IPARENT ");

            if (Conn == null || OracleAccess == null)
            {
                intConn();
            }
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql.ToString());
            if (dt == null || dt.Rows.Count == 0)
                return new List<QUOCHOI_COQUAN>();
            var lstObj = ObjectHelper.ToListData<QUOCHOI_COQUAN>(dt);
            dt.Dispose();
            return lstObj;
        }
        public List<QUOCHOI_COQUAN> GetAll_By(string CCODE)
        {
            var sql = @"SELECT * FROM QUOCHOI_COQUAN WHERE 1 = 1";
            var param = new List<OracleParameter>();
            if (!string.IsNullOrEmpty(CCODE))
            {
                sql += " and CCODE = :code ";
                param.Add(new OracleParameter("code", CCODE));
            }       
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
            if (dt == null || dt.Rows.Count == 0)
                return new List<QUOCHOI_COQUAN>();
            var lstObj = ObjectHelper.ToListData<QUOCHOI_COQUAN>(dt);
            dt.Dispose();
            return lstObj;
        }
        public List<QUOCHOI_COQUAN> GetAll_By(string CCODE, int ICOQUAN)
        {
            var sql = @"SELECT * FROM QUOCHOI_COQUAN WHERE 1 = 1";
            var param = new List<OracleParameter>();
            if (!string.IsNullOrEmpty(CCODE))
            {
                sql += " and CCODE = :code ";
                param.Add(new OracleParameter("code", CCODE));

            }
            if (ICOQUAN != null)
            {
                sql += " and ICOQUAN <> :icoquan ";
                param.Add(new OracleParameter("icoquan", ICOQUAN));
            }
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
            if (dt == null || dt.Rows.Count == 0)
                return new List<QUOCHOI_COQUAN>();
            var lstObj = ObjectHelper.ToListData<QUOCHOI_COQUAN>(dt);
            dt.Dispose();
            return lstObj;
        }
        public List<QUOCHOI_COQUAN> GetAll_ByIparent(int IPARENT)
        {
            var sql = @"SELECT * FROM QUOCHOI_COQUAN WHERE 1 = 1";
            var param = new List<OracleParameter>();
            if (IPARENT!=null)
            {
                sql += " and IPARENT = :iparent ";
                param.Add(new OracleParameter("iparent", IPARENT));
            }
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
            if (dt == null || dt.Rows.Count == 0)
                return new List<QUOCHOI_COQUAN>();
            var lstObj = ObjectHelper.ToListData<QUOCHOI_COQUAN>(dt);
            dt.Dispose();
            return lstObj;
        }
        public QUOCHOI_COQUAN GetByID(int ID)
        {
            return base.GetItem<QUOCHOI_COQUAN>("ICOQUAN", ID);
        }
        public QUOCHOI_COQUAN AddNew(QUOCHOI_COQUAN Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("QUOCHOI_COQUAN_SEQ");
            if (ID != 0)
            {
                Input.ICOQUAN = ID;
                if (base.InsertItem<QUOCHOI_COQUAN>(Input))
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
        public Boolean Update(QUOCHOI_COQUAN Input)
        {

            if (base.UpdateItem<QUOCHOI_COQUAN>(Input, "ICOQUAN"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(QUOCHOI_COQUAN Input)
        {

            if (base.DeleteItem<QUOCHOI_COQUAN>(Input, "ICOQUAN"))
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
