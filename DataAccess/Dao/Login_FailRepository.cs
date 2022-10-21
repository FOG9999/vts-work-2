    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.Models;
using Oracle.ManagedDataAccess.Client;
namespace DataAccess.Dao
{
    public interface IULogin_FailRepository
    {
        LOGIN_FAIL GetByID(int ID);
        LOGIN_FAIL GetByIP(String IP);
        LOGIN_FAIL GetByDATE(String DATE);
        LOGIN_FAIL AddNew(LOGIN_FAIL UserInput);
        Boolean Update(LOGIN_FAIL UserInput);
        Boolean Delete(LOGIN_FAIL UserInput);
    }
    public class Login_FailRepository : BaseRepository, IULogin_FailRepository
    {
        public List<LOGIN_FAIL> GetAll()
        {
            List<LOGIN_FAIL> resule = new List<LOGIN_FAIL>();
            resule = base.GetAll<LOGIN_FAIL>();
            return resule;
        }
        public List<LOGIN_FAIL> GetAll(Dictionary<string,object> condition)
        {
            List<LOGIN_FAIL> resule = new List<LOGIN_FAIL>();
            resule = base.GetAll<LOGIN_FAIL>(condition);
            return resule;
        }
        public LOGIN_FAIL GetByID(int ID)
        {
            return base.GetItem<LOGIN_FAIL>("ID", ID);
        }
        public LOGIN_FAIL GetByIP(String IP)
        {
            return base.GetItem<LOGIN_FAIL>("IP", IP);
        }
        public LOGIN_FAIL GetByDATE(String DATE)
        {
            return base.GetItemBet<LOGIN_FAIL>("DDATE", DATE);
        }
        public LOGIN_FAIL AddNew(LOGIN_FAIL Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("LOGIN_FAIL_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<LOGIN_FAIL>(Input))
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
        public Boolean Update(LOGIN_FAIL actionInput)
        {

            if (base.UpdateItem<LOGIN_FAIL>(actionInput, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(LOGIN_FAIL actionInput)
        {

            if (base.DeleteItem<LOGIN_FAIL>(actionInput, "ID"))
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

