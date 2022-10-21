using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Entities.Models;
using Entities.Objects;
using Oracle.ManagedDataAccess.Client;
using Utilities;
namespace DataAccess.Dao
{
    public interface IUsserRepository
    {

        List<USERS> GetAll();
        List<USERS> GetAll(Dictionary<string, object> condition);
        USERS GetByID(int ID);

        USERS GetByUseName(String Username);
        USERS AddNew(USERS UserInput);
        Boolean Update(USERS UserInput);
        Boolean Delete(USERS UserInput);
        List<USERS> Getlist(String sql);
    }
    public class UsserRepository : BaseRepository, IUsserRepository
    {
        Log log = new Log();
        public USERS GetByID(int ID)
        {

            return base.GetItem<USERS>("IUSER", ID);

        }
        public USERS GetByUseName(String Username)
        {
            return base.GetItem<USERS>("CUSERNAME", Username);

        }
        public List<USERS> GetAll()
        {

            List<USERS> resule = new List<USERS>();
            resule = base.GetAll<USERS>();
            return resule;
        }
        public List<USERS> GetList_SQLTimkiemten(string cten)
        {

            List<USERS> listObj = new List<USERS>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from USERS where UPPER(CUSERNAME) LIKE '%' || UPPER(:param01) || '%' OR UPPER(CTEN) LIKE '%' || UPPER(:param01) || '%'";
                var noidungkey = new OracleParameter("param01", OracleDbType.NVarchar2);
                noidungkey.Value = cten.Trim();
                param.Add(noidungkey);

                listObj = base.GetList<USERS>(sql, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tra cứu tài khoản theo tên");
                return null;
            }
            return listObj;
        }
        public List<USERS> GetlistByCondition(Dictionary<string, object> dic)
        {
            List<USERS> resule = new List<USERS>();
            resule = base.GetAll<USERS>(dic);
            return resule;
        }
        public List<USERS> Getlist(String sql)
        {
            List<USERS> resule = new List<USERS>();
            resule = base.GetList<USERS>(sql);
            return resule;
        }
        public List<USERS> GetList_coquanList(int icoquan)
        {

            List<USERS> listObj = new List<USERS>();
            var param = new List<OracleParameter>();
            try
            {
                int type = -1;
                int idonvi_ = icoquan;
                string sql = "SELECT * from USERS where IDONVI =  :param01   ";
                param.Add(new OracleParameter("param01", idonvi_));
                sql += "and ITYPE  !=     :param02   ";
                param.Add(new OracleParameter("param02", type));
                //sql += " and  ICOQUAN !=:param03";
                //param.Add(new OracleParameter("param03", id));
                listObj = base.GetList<USERS>(sql, param);


            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tra cứu tài khoản theo tên");
                return null;
            }
            return listObj;
        }
        public List<USERS> GetList_UserCheck(string username, int icoquan)
        {


            List<USERS> listObj = new List<USERS>();
            var param = new List<OracleParameter>();
            try
            {
                int idonvi_ = icoquan;
                string sql = " select * from USERS where IUSER != " + " :param01";

                param.Add(new OracleParameter("param01", idonvi_));
                sql += "AND CUSERNAME = :param02 ";
                var noidungkey = new OracleParameter("param02", OracleDbType.NVarchar2);
                noidungkey.Value = username.Trim();
                param.Add(noidungkey);
                listObj = base.GetList<USERS>(sql, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tra cứu tài khoản theo tên");
                return null;
            }
            return listObj;
        }
        public List<ID_Parent> GetListID_Parent_SQL_CheckAction(int id_parent, int id_tk)
        {
            List<ID_Parent> listObj = new List<ID_Parent>();
            var param = new List<OracleParameter>();
            try
            {

                int idonvi_ = id_tk;
                int ipra = id_parent;
                string sql = "select  * from USER_ACTION, ACTION WHERE USER_ACTION.IACTION = ACTION.IACTION " +
              "  and ACTION.IPARENT =:param01 ";
                param.Add(new OracleParameter("param01", id_parent));
                sql += " and USER_ACTION.IUSER = :param02";
                param.Add(new OracleParameter("param02", idonvi_));
                listObj = base.GetList<ID_Parent>(sql, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Kiểm tra action");
                return null;
            }
            return listObj;
        }
        // 
        public List<ID_Parent> GetListID_Parent_SQL(int id_tk)
        {
            List<ID_Parent> listObj = new List<ID_Parent>();
            var param = new List<OracleParameter>();
            try
            {

                int idonvi_ = id_tk;
                string sql = "select  * from USER_ACTION, ACTION WHERE USER_ACTION.IACTION = ACTION.IACTION  " +
           " and USER_ACTION.IUSER = :param01 ";
                param.Add(new OracleParameter("param01", idonvi_));
                listObj = base.GetList<ID_Parent>(sql, param);


            }
            catch (Exception e)
            {
                log.Log_Error(e, "Kiểm tra id parent");
                return null;
            }
            return listObj;
        }
        // 

        public List<USERS> GetAll(Dictionary<string, object> condition)
        {
            List<USERS> resule = new List<USERS>();
            resule = base.GetAll<USERS>(condition);
            return resule;
        }
        public USERS AddNew(USERS UserInput)
        {
            decimal IUse = 0;
            IUse = base.GetNextValSeq("USERS_SEQ");
            if (IUse != 0)
            {
                UserInput.IUSER = IUse;
                if (base.InsertItem<USERS>(UserInput))
                {
                    return UserInput;
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
        public Boolean Update(USERS UserInput)
        {

            if (base.UpdateItem<USERS>(UserInput, "IUSER"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(USERS UserInput)
        {

            if (base.DeleteItem<USERS>(UserInput, "IUSER"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<USERS> GetList_NguoiNhap(int icoquan)
        {

            List<USERS> listObj = new List<USERS>();
            var param = new List<OracleParameter>();
            try
            {
                int idonvi_ = icoquan;
                string sql = "SELECT USERS.* FROM ( SELECT KNTC_DON.IUSER FROM KNTC_DON WHERE KNTC_DON.IDONVITIEPNHAN= :param01  GROUP BY KNTC_DON.IUSER) KNTC_DON  INNER JOIN USERS ON KNTC_DON.IUSER = USERS.IUSER ";
                param.Add(new OracleParameter("param01", idonvi_));
                listObj = base.GetList<USERS>(sql, param);


            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tra cứu tài khoản theo tên");
                return null;
            }
            return listObj;
        }
    }
}
