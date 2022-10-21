using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.Models;
namespace DataAccess.Dao
{

    public interface IUUser_ChucvuRepository
    {
        List<USER_CHUCVU> GetAll();
        List<USER_CHUCVU> GetAllWhere_Paging(Dictionary<string, object> dictionary);
        USER_CHUCVU GetByID(int ID);
        USER_CHUCVU AddNew(USER_CHUCVU Input);
        Boolean Update(USER_CHUCVU Input);
        Boolean Delete(USER_CHUCVU Input);
        List<USER_CHUCVU> GetAll(Dictionary<string, object> condition);
    }
    public class User_ChucvuRepository : BaseRepository, IUUser_ChucvuRepository
    {
        public List<USER_CHUCVU> GetAll()
        {
            List<USER_CHUCVU> resule = new List<USER_CHUCVU>();
            resule = base.GetAll<USER_CHUCVU>();
            return resule;
        }
        public List<USER_CHUCVU> GetAll(Dictionary<string, object> condition)
        {
            List<USER_CHUCVU> resule = new List<USER_CHUCVU>();
            resule = base.GetAll<USER_CHUCVU>(condition);
            return resule;
        }

        public List<USER_CHUCVU> GetAllWhere_Paging(Dictionary<string, object> dictionary)
        {
            List<USER_CHUCVU> resule = new List<USER_CHUCVU>();
            resule = base.GetAll<USER_CHUCVU>(dictionary);
            return resule;
        }
        public USER_CHUCVU GetByID(int ID)
        {
            return base.GetItem<USER_CHUCVU>("ICHUCVU", ID);
        }
        public USER_CHUCVU AddNew(USER_CHUCVU Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("USER_CHUCVU_SEQ");
            if (ID != 0)
            {
                Input.ICHUCVU = ID;
                if (base.InsertItem<USER_CHUCVU>(Input))
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
        public Boolean Update(USER_CHUCVU Input)
        {

            if (base.UpdateItem<USER_CHUCVU>(Input, "ICHUCVU"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(USER_CHUCVU Input)
        {

            if (base.DeleteItem<USER_CHUCVU>(Input, "ICHUCVU"))
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
