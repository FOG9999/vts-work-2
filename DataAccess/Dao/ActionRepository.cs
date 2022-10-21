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

    public interface IUActionRepository
    {
        ACTION GetByID(int ID);
        List<ACTION> GetAll();
        List<ACTION> GetAll(Dictionary<string, object> dictionary);
        ACTION AddNew(ACTION UserInput);
        Boolean Update(ACTION UserInput);
        Boolean Delete(ACTION UserInput);
        List<ACTION> GetList(String sql);
      
    }
    public class ActionRepository : BaseRepository, IUActionRepository
    {
        public List<ACTION> Get_List_Action_ByIDGroup(int id_group, int id_parent)
        {
            List<ACTION> listObj = new List<ACTION>();
            var param = new List<OracleParameter>();
            try
            {
                if (id_parent != 0)
                {
                    int igr = id_group;
                    int ipa = id_parent;
                    string sql = "SELECT ACTION .* FROM ACTION inner join  USER_GROUP_ACTION on " +
                " ACTION.IACTION = USER_GROUP_ACTION.IACTION and ACTION.IPARENT = :param01";
                    param.Add(new OracleParameter("param01", ipa));
                    sql += " and  USER_GROUP_ACTION.IGROUP = :param02";
                    param.Add(new OracleParameter("param02", igr));

                    listObj = base.GetList<ACTION>(sql, param);
                }
                else
                {
                    int igr = id_group;
                 
                    string sql = "SELECT distinct ACTION.IPARENT FROM ACTION inner join  USER_GROUP_ACTION on " +
                " ACTION.IACTION = USER_GROUP_ACTION.IACTION and  USER_GROUP_ACTION.IGROUP =" + " :param01";
                    param.Add(new OracleParameter("param01", igr));
                    listObj = base.GetList<ACTION>(sql, param);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return listObj;
        }
        public List<ACTION> GetAll()
        {
            List<ACTION> resule = new List<ACTION>();
            resule = base.GetAll<ACTION>();

            return resule;

        }
        public List<ACTION> GetAll(Dictionary<string, object> dictionary)
        {
            List<ACTION> resule = new List<ACTION>();
            resule = base.GetAll<ACTION>(dictionary);

            return resule;
        }
        public List<ACTION> GetList(String sql)
        {
            List<ACTION> resule = new List<ACTION>();
            resule = base.GetList<ACTION>(sql);
            return resule;
        }
        public ACTION GetByID(int ID)
        {


            return base.GetItem<ACTION>("IACTION", ID);
        }
        public ACTION GetByID_IPARENT(int ID)
        {


            return base.GetItem<ACTION>("IPARENT", ID);
        }
        public ACTION AddNew(ACTION Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("ACTION_SEQ");
            if (ID != 0)
            {
                Input.IACTION = ID;
                if (base.InsertItem<ACTION>(Input))
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
        public Boolean Update(ACTION actionInput)
        {

            if (base.UpdateItem<ACTION>(actionInput, "IACTION"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(ACTION actionInput)
        {

            if (base.DeleteItem<ACTION>(actionInput, "IACTION"))
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
