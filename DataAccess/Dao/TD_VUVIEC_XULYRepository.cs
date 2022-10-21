using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    public interface ITD_VUVIEC_XULYRepository
    {
        TD_VUVIEC_XULY GetByID(int ID);
        TD_VUVIEC_XULY AddNew(TD_VUVIEC_XULY UserInput);
        Boolean Update(TD_VUVIEC_XULY UserInput);
        Boolean Delete(TD_VUVIEC_XULY UserInput);
      
         List<TD_VUVIEC_XULY> GetAll();
        List<TD_VUVIEC_XULY> GetAll(Dictionary<string, object> condition);
    }
    public class TD_VUVIEC_XULYRepository : BaseRepository, ITD_VUVIEC_XULYRepository
    { 
        public List<TD_VUVIEC_XULY> GetAll()
        {
            List<TD_VUVIEC_XULY> resule = new List<TD_VUVIEC_XULY>();
            resule = base.GetAll<TD_VUVIEC_XULY>();

            return resule;

        }
        public TD_VUVIEC_XULY GetByID(int ID)
        {
            return base.GetItem<TD_VUVIEC_XULY>("IXULY", ID);
        }
        public List<TD_VUVIEC_XULY> GetAll(Dictionary<string, object> condition)
        {
            List<TD_VUVIEC_XULY> resule = new List<TD_VUVIEC_XULY>();
            resule = base.GetAll<TD_VUVIEC_XULY>(condition);
            return resule;
        }

        public TD_VUVIEC_XULY AddNew(TD_VUVIEC_XULY actionInput)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("TD_VUVIEC_XULY_SEQ");
            if (ID != 0)
            {
                actionInput.IXULY = ID;
                if (base.InsertItem<TD_VUVIEC_XULY>(actionInput))
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
        public Boolean Update(TD_VUVIEC_XULY actionInput)
        {

            if (base.UpdateItem<TD_VUVIEC_XULY>(actionInput, "IXULY"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(TD_VUVIEC_XULY actionInput)
        {

            if (base.DeleteItem<TD_VUVIEC_XULY>(actionInput, "IXULY"))
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
