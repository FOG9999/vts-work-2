using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUser_TypeRepository
    {
        USER_TYPE GetByID(int ID);
        USER_TYPE GetByUseName(String Username);
        USER_TYPE AddNew(USER_TYPE UserInput);
        Boolean Update(USER_TYPE UserInput);
        Boolean Delete(USER_TYPE UserInput);
    }
    public class UsertypeRepository : BaseRepository, IUser_TypeRepository
    {
        public List<USER_TYPE> Getlist(String sql)
        {
            List<USER_TYPE> resule = new List<USER_TYPE>();
            resule = base.GetList<USER_TYPE>(sql);
            return resule;
        }
        public List<USER_TYPE> GetAll(Dictionary<string, object> condition)
        {
            List<USER_TYPE> resule = new List<USER_TYPE>();
            resule = base.GetAll<USER_TYPE>(condition);
            return resule;
        }
        public List<USER_TYPE> GetAll()
        {
            List<USER_TYPE> resule = new List<USER_TYPE>();
            resule = base.GetAll<USER_TYPE>();
            return resule;
        }
        public USER_TYPE GetByID(int ID)
        {
            return base.GetItem<USER_TYPE>("IUSER_TYPE", ID);
        }

        public USER_TYPE GetByUseName(String actionInput)
        {
            return base.GetItem<USER_TYPE>("IUSER_TYPE", actionInput);
        }
        public USER_TYPE AddNew(USER_TYPE actionInput)
        {

            if (base.InsertItem<USER_TYPE>(actionInput))
            {
                return actionInput;
            }
            else
            {
                return null;
            }
        }
        public Boolean Update(USER_TYPE actionInput)
        {

            if (base.UpdateItem<USER_TYPE>(actionInput, "IUSER_TYPE"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(USER_TYPE actionInput)
        {

            if (base.DeleteItem<USER_TYPE>(actionInput, "IUSER_TYPE"))
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

