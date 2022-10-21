using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DataAccess.Dao
{
    public interface IUTokenRepository
    {
        List<TOKEN> GetAll();
        TOKEN GetByID(int ID);
        TOKEN AddNew(TOKEN Input);
        Boolean Update(TOKEN Input);
        Boolean Delete(TOKEN Input);
        List<TOKEN> GetAllWhere(Dictionary<string, object> dictionary);
        List<TOKEN> GetList(String where);
    }
    public class TokenRepository : BaseRepository, IUTokenRepository
    {
        public List<TOKEN> GetAll()
        {
            List<TOKEN> resule = new List<TOKEN>();
            resule = base.GetAll<TOKEN>();
            return resule;
        }
        public List<TOKEN> GetList(String where)
        {
            List<TOKEN> resule = new List<TOKEN>();
            resule = base.GetList<TOKEN>(where);
            return resule;
        }
        public List<TOKEN> GetAllWhere(Dictionary<string, object> dictionary)
        {
            List<TOKEN> resule = new List<TOKEN>();
            resule = base.GetAll<TOKEN>(dictionary);
            return resule;
        }
        public TOKEN GetByID(int ID)
        {
            return base.GetItem<TOKEN>("ID", ID);
        }
        public TOKEN AddNew(TOKEN Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("TOKEN_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<TOKEN>(Input))
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
        public Boolean Update(TOKEN Input)
        {

            if (base.UpdateItem<TOKEN>(Input, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(TOKEN Input)
        {

            if (base.DeleteItem<TOKEN>(Input, "ID"))
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
