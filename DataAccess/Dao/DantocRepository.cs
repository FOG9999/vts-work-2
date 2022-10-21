using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DataAccess.Dao
{
   
    public interface IUDantocRepository
    {
        DANTOC GetByID(int ID);
        DANTOC AddNew(DANTOC UserInput);
        Boolean Update(DANTOC UserInput);
        Boolean Delete(DANTOC UserInput);
        List<DANTOC> GetAll();
        List<DANTOC> GetAll(Dictionary<string, object> condition);
    }
    public class DantocRepository : BaseRepository, IUDantocRepository
    {
        public List<DANTOC> GetAll()
        {
            List<DANTOC> resule = new List<DANTOC>();
            resule = base.GetAll<DANTOC>();
            return resule;
        }
        public List<DANTOC> GetList(String sql)
        {
            List<DANTOC> resule = new List<DANTOC>();
            resule = base.GetList<DANTOC>(sql);
            return resule;
        }
        public List<DANTOC> GetAll(Dictionary<string,object> condition)
        {
            List<DANTOC> resule = new List<DANTOC>();
            resule = base.GetAll<DANTOC>(condition);
            return resule;
        }
        public DANTOC GetByID(int ID)
        {
            return base.GetItem<DANTOC>("IDANTOC", ID);
        }
        public DANTOC AddNew(DANTOC Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("DANTOC_SEQ");
            if (ID != 0)
            {
                Input.IDANTOC = ID;
                if (base.InsertItem<DANTOC>(Input))
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
        public Boolean Update(DANTOC Input)
        {

            if (base.UpdateItem<DANTOC>(Input, "IDANTOC"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(DANTOC Input)
        {

            if (base.DeleteItem<DANTOC>(Input, "IDANTOC"))
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
