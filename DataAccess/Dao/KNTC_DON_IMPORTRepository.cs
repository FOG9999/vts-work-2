using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    public class KNTC_DON_IMPORTRepository : BaseRepository
    {
        public List<KNTC_DON_IMPORT> GetAll()
        {
            List<KNTC_DON_IMPORT> resule = new List<KNTC_DON_IMPORT>();
            resule = base.GetAll<KNTC_DON_IMPORT>();
            return resule;
        }
        public List<KNTC_DON_IMPORT> GetAll(Dictionary<string, object> dictionary)
        {
            List<KNTC_DON_IMPORT> resule = new List<KNTC_DON_IMPORT>();
            resule = base.GetAll<KNTC_DON_IMPORT>(dictionary);
            return resule;
        }
        public List<KNTC_DON_IMPORT> GetAll_Paging(Dictionary<string, object> dictionary,int page,int page_size)
        {
            int from = page * page_size + 1;
            int to = (page - 1) * page_size + 1;
            List<KNTC_DON_IMPORT> resule = new List<KNTC_DON_IMPORT>();
            resule = base.GetAll_PageList<KNTC_DON_IMPORT>(dictionary, from,to);
            return resule;
        }
        public KNTC_DON_IMPORT GetByID(int ID)
        {
            return base.GetItem<KNTC_DON_IMPORT>("ID", ID);
        }
        public KNTC_DON_IMPORT AddNew(KNTC_DON_IMPORT Input)
        {
            decimal IUse = 0;
            IUse = base.GetNextValSeq("KNTC_DON_IMPORT_SEQ");
            if (IUse != 0)
            {
                Input.ID = IUse;
                if (base.InsertItem<KNTC_DON_IMPORT>(Input))
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
        public Boolean Update(KNTC_DON_IMPORT KN)
        {
            if (base.UpdateItem<KNTC_DON_IMPORT>(KN, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KNTC_DON_IMPORT KN)
        {

            if (base.DeleteItem<KNTC_DON_IMPORT>(KN, "ID"))
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
