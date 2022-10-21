using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IKNTC_VANBANRepository
    {
        KNTC_VANBAN GetByID(int ID);
        KNTC_VANBAN GetByUseName(String Username);
        KNTC_VANBAN AddNew(KNTC_VANBAN UserInput);
        Boolean Update(KNTC_VANBAN UserInput);
        Boolean Delete(KNTC_VANBAN UserInput);
        List<KNTC_VANBAN> GetList(string sql);
        List<KNTC_VANBAN> GetListByDictionary(Dictionary<string, object> _don);
        List<KNTC_VANBAN> GetAll();
    }
    public class KNTC_VanbanRepository : BaseRepository, IKNTC_VANBANRepository
    {
        public List<KNTC_VANBAN> GetList(string sql)
        {
            
            List<KNTC_VANBAN> resule = new List<KNTC_VANBAN>();
            resule = base.GetList<KNTC_VANBAN>(sql);
            return resule;
        }
        public List<KNTC_VANBAN> GetAll()
        {

            List<KNTC_VANBAN> resule = new List<KNTC_VANBAN>();
            resule = base.GetAll<KNTC_VANBAN>();
            return resule;
        }
        public List<KNTC_VANBAN> GetListByDictionary(Dictionary<string, object> _don)
        {

            List<KNTC_VANBAN> resule = new List<KNTC_VANBAN>();
            resule = base.GetAll<KNTC_VANBAN>(_don);
            return resule;
        }
        public KNTC_VANBAN GetByID(int ID)
        {
            return base.GetItem<KNTC_VANBAN>("IVANBAN", ID);
        }
        public int GetLastID_Vanban_KNTC()
        {
            int IVANBAN = (int)base.GetList<KNTC_VANBAN>("select * from KNTC_VANBAN order by IVANBAN desc").FirstOrDefault().IDON;
            return IVANBAN;
        }
        public KNTC_VANBAN GetByUseName(String actionInput)
        {
            return base.GetItem<KNTC_VANBAN>("IVANBAN", actionInput);
        }
        public KNTC_VANBAN AddNew(KNTC_VANBAN actionInput)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KNTC_VANBAN_SEQ");
            if (ID != 0)
            {
                actionInput.IVANBAN = ID;
                if (base.InsertItem<KNTC_VANBAN>(actionInput))
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
        public Boolean Update(KNTC_VANBAN actionInput)
        {

            if (base.UpdateItem<KNTC_VANBAN>(actionInput, "IVANBAN"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KNTC_VANBAN actionInput)
        {

            if (base.DeleteItem<KNTC_VANBAN>(actionInput, "IVANBAN"))
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
