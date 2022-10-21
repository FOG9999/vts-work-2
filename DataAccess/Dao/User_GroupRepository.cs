using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.Objects;
namespace DataAccess.Dao
{
  
    public interface IUUser_GroupRepository
    {
        List<USER_GROUP> GetAll();
        USER_GROUP GetByID(int ID);      
        USER_GROUP AddNew(USER_GROUP Input);
        Boolean Update(USER_GROUP Input);
        Boolean Delete(USER_GROUP Input);
        List<ID_Parent> GetList(String sql);
        List<USER_GROUP> GetList_(String sql);
       
    }
    public class User_GroupRepository : BaseRepository, IUUser_GroupRepository
    {
        public List<USER_GROUP> GetAll()
        {
            List<USER_GROUP> resule = new List<USER_GROUP>();
            resule = base.GetAll<USER_GROUP>();
            return resule;
        }

        public List<USER_GROUP> GetAll(Dictionary<string,object> condition)
        {
            List<USER_GROUP> resule = new List<USER_GROUP>();
            resule = base.GetAll<USER_GROUP>(condition);
            return resule;
        }
        public List<USER_GROUP> GetList_(String sql)
        {
            List<USER_GROUP> resule = new List<USER_GROUP>();
            resule = base.GetList<USER_GROUP>(sql);
            return resule;
        }
        

        public List<ID_Parent> GetList(String sql)
        {
            List<ID_Parent> resule = new List<ID_Parent>();
            resule = base.GetList<ID_Parent>(sql);
            return resule;
        }
      
        public USER_GROUP GetByID(int ID)
        {
            return base.GetItem<USER_GROUP>("IGROUP", ID);
        }
        public USER_GROUP AddNew(USER_GROUP Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("USER_GROUP_SEQ");
            if (ID != 0)
            {
                Input.IGROUP = ID;
                if (base.InsertItem<USER_GROUP>(Input))
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
        public Boolean Update(USER_GROUP Input)
        {

            if (base.UpdateItem<USER_GROUP>(Input, "IGROUP"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(USER_GROUP Input)
        {

            if (base.DeleteItem<USER_GROUP>(Input, "IGROUP"))
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
