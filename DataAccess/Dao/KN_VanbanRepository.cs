using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    
    public interface IKN_VANBANRepository
    {
        KN_VANBAN GetByID(int ID);
        KN_VANBAN GetByUseName(String Username);
        KN_VANBAN AddNew(KN_VANBAN UserInput);
        Boolean Update(KN_VANBAN UserInput);
        Boolean Delete(KN_VANBAN UserInput);
    }
    public class KN_VanbanRepository : BaseRepository, IKN_VANBANRepository
    {
        public List<KN_VANBAN> GetAll()
        {
            List<KN_VANBAN> resule = new List<KN_VANBAN>();
            resule = base.GetAll<KN_VANBAN>();
            return resule;
        }
        public List<KN_VANBAN> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_VANBAN> resule = new List<KN_VANBAN>();
            resule = base.GetAll<KN_VANBAN>(dictionary);

            return resule;
        }
        public List<KN_VANBAN> GetList(String sql)
        {
            List<KN_VANBAN> resule = new List<KN_VANBAN>();
            resule = base.GetList<KN_VANBAN>(sql);
            return resule;
        }

        public KN_VANBAN GetByID(int ID)
        {
            return base.GetItem<KN_VANBAN>("IVANBAN", ID);
        }

        public KN_VANBAN GetByUseName(String actionInput)
        {
            return base.GetItem<KN_VANBAN>("IVANBAN", actionInput);
        }
        public KN_VANBAN AddNew(KN_VANBAN Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KN_VANBAN_SEQ");
            if (ID != 0)
            {
                Input.IVANBAN = ID;
                if (base.InsertItem<KN_VANBAN>(Input))
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
        public Boolean Update(KN_VANBAN actionInput)
        {

            if (base.UpdateItem<KN_VANBAN>(actionInput, "IVANBAN"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_VANBAN actionInput)
        {

            if (base.DeleteItem<KN_VANBAN>(actionInput, "IVANBAN"))
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
