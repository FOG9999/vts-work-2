using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUUser_ActionRepository
    {
        USER_ACTION GetByID(int ID);
        List<USER_ACTION> GetAll();
        List<USER_ACTION> GetAll(Dictionary<string, object> dictionary);
        List<USER_ACTION> GetList(String sql);
        USER_ACTION AddNew(USER_ACTION UserInput);
        Boolean Update(USER_ACTION UserInput);
        Boolean Delete(USER_ACTION UserInput);
    }
    public class User_ActionRepository : BaseRepository, IUUser_ActionRepository
    {
        public List<USER_ACTION> GetAll()
        {
            List<USER_ACTION> resule = new List<USER_ACTION>();
            resule = base.GetAll<USER_ACTION>();
            return resule;
        }
        public List<USER_ACTION> GetList(String sql)
        {
            List<USER_ACTION> resule = new List<USER_ACTION>();
            resule = base.GetList<USER_ACTION>(sql);
            return resule;
        }
        public List<USER_ACTION> GetAll(Dictionary<string, object> dictionary)
        {
            List<USER_ACTION> resule = new List<USER_ACTION>();
            resule = base.GetAll<USER_ACTION>(dictionary);
            return resule;
        }
        public List<USER_ACTION> GetList_user_action(string arr, int id_action)
        {

            List<USER_ACTION> listObj = new List<USER_ACTION>();
            var param = new List<OracleParameter>();
            try
            {
                int id = id_action;
                string sql = "select USER_GROUP_ACTION.* from USER_GROUP_ACTION where IGROUP in ( ";
                var list = arr.Split(',');
                foreach (var d in list)
                {
                    if (d != "")
                        sql += ":param_" + d + ",";
                    param.Add(new OracleParameter(":param_" + d, d));
                }
                sql += ")";
                var lastIndex = sql.LastIndexOf(',');
                sql = sql.Remove(lastIndex, 1) + "\n";
                sql += " and IACTION = :param ";
                param.Add(new OracleParameter("param", id));
                //sql += " and  ICOQUAN !=:param03";
                //param.Add(new OracleParameter("param03", id));
                listObj = base.GetList<USER_ACTION>(sql, param);


            }
            catch (Exception)
            {
                throw;
            }
            return listObj;
        }
        public USER_ACTION GetByID(int ID)
        {
            return base.GetItem<USER_ACTION>("ID", ID);
        }
        public USER_ACTION AddNew(USER_ACTION Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("USER_ACTION_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<USER_ACTION>(Input))
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
        public Boolean Update(USER_ACTION Input)
        {

            if (base.UpdateItem<USER_ACTION>(Input, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(USER_ACTION Input)
        {

            if (base.DeleteItem<USER_ACTION>(Input, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean DellAll(Dictionary<string, object> dictionary)
        {
            return base.DeleteAll<USER_ACTION>(dictionary); ;
        }
    }
}
