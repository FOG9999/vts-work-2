using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUQuoctichRepository
    {
        List<QUOCTICH> GetAll();
        List<QUOCTICH> GetAll(Dictionary<string, object> condition);
        QUOCTICH GetByID(int ID);
        QUOCTICH AddNew(QUOCTICH UserInput);
        Boolean Update(QUOCTICH UserInput);
        Boolean Delete(QUOCTICH UserInput);
    }
    public class QuoctichRepository : BaseRepository, IUQuoctichRepository
    {
        public List<QUOCTICH> GetList(String sql)
        {
            List<QUOCTICH> resule = new List<QUOCTICH>();
            resule = base.GetList<QUOCTICH>(sql);
            return resule;
        }
        public List<QUOCTICH> GetAll()
        {
            List<QUOCTICH> resule = new List<QUOCTICH>();
            resule = base.GetAll<QUOCTICH>();
            return resule;
        }
        public List<QUOCTICH> GetAll(Dictionary<string,object> condition)
        {
            List<QUOCTICH> resule = new List<QUOCTICH>();
            resule = base.GetAll<QUOCTICH>(condition);
            return resule;
        }
        public QUOCTICH GetByID(int ID)
        {
            return base.GetItem<QUOCTICH>("IQUOCTICH", ID);
        }
        public QUOCTICH AddNew(QUOCTICH actionInput)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("QUOCTICH_SEQ");
            if (ID != 0)
            {
                actionInput.IQUOCTICH = ID;
                if (base.InsertItem<QUOCTICH>(actionInput))
                {
                    return actionInput;
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
        public Boolean Update(QUOCTICH actionInput)
        {

            if (base.UpdateItem<QUOCTICH>(actionInput, "IQUOCTICH"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(QUOCTICH actionInput)
        {

            if (base.DeleteItem<QUOCTICH>(actionInput, "IQUOCTICH"))
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
