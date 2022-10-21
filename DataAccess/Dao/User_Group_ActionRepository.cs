using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;


namespace DataAccess.Dao
{
    public interface IUUser_Group_ActionRepository
    {
        List<USER_GROUP_ACTION> GetAll();
        USER_GROUP_ACTION GetByID(int ID);
        USER_GROUP_ACTION AddNew(USER_GROUP_ACTION Input);
        Boolean Update(USER_GROUP_ACTION Input);
        Boolean Delete(USER_GROUP_ACTION Input);

    }
    public class User_Group_ActionRepository : BaseRepository, IUUser_Group_ActionRepository
    {
        public List<USER_GROUP_ACTION> GetAll()
        {
            List<USER_GROUP_ACTION> resule = new List<USER_GROUP_ACTION>();
            resule = base.GetAll<USER_GROUP_ACTION>();
            return resule;
        }
        public List<USER_GROUP_ACTION> GetAll(Dictionary<string, object> dictionary)
        {
            List<USER_GROUP_ACTION> resule = new List<USER_GROUP_ACTION>();
            resule = base.GetAll<USER_GROUP_ACTION>(dictionary);
            return resule;
        }
        public USER_GROUP_ACTION GetByID(int ID)
        {
            return base.GetItem<USER_GROUP_ACTION>("ID", ID);
        }
        public USER_GROUP_ACTION AddNew(USER_GROUP_ACTION Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("USER_GROUP_ACTION_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<USER_GROUP_ACTION>(Input))
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
        public Boolean Update(USER_GROUP_ACTION Input)
        {

            if (base.UpdateItem<USER_GROUP_ACTION>(Input, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(USER_GROUP_ACTION Input)
        {

            if (base.DeleteItem<USER_GROUP_ACTION>(Input, "ID"))
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
