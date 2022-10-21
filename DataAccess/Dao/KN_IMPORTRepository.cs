using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    public class KN_IMPORTRepository : BaseRepository
    {
        public List<KN_IMPORT> GetAll()
        {
            List<KN_IMPORT> resule = new List<KN_IMPORT>();
            resule = base.GetAll<KN_IMPORT>();
            return resule;
        }
        public List<KN_IMPORT> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_IMPORT> resule = new List<KN_IMPORT>();
            resule = base.GetAll<KN_IMPORT>(dictionary);
            return resule;
        }
        public List<KN_IMPORT> GetAll_Paging(Dictionary<string, object> dictionary,int page,int page_size)
        {
            int from = page * page_size + 1;
            int to = (page - 1) * page_size + 1;
            List<KN_IMPORT> resule = new List<KN_IMPORT>();
            resule = base.GetAll_PageList<KN_IMPORT>(dictionary, from,to);
            return resule;
        }
        public KN_IMPORT GetByID(int ID)
        {
            return base.GetItem<KN_IMPORT>("ID", ID);
        }
        public KN_IMPORT AddNew(KN_IMPORT Input)
        {
            decimal IUse = 0;
            IUse = base.GetNextValSeq("KN_IMPORT_SEQ");
            if (IUse != 0)
            {
                Input.ID = IUse;
                if (base.InsertItem<KN_IMPORT>(Input))
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
        public Boolean Update(KN_IMPORT KN)
        {
            if (base.UpdateItem<KN_IMPORT>(KN, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KN_IMPORT KN)
        {

            if (base.DeleteItem<KN_IMPORT>(KN, "ID"))
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
