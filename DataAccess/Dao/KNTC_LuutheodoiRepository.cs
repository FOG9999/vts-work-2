using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    public interface IKNTC_LuutheodoiRepository
    {
        KNTC_LUUTHEODOI GetByID(int ID);
        List<KNTC_LUUTHEODOI> GetAll();
        
        KNTC_LUUTHEODOI AddNew(KNTC_LUUTHEODOI Input);
        Boolean Update(KNTC_LUUTHEODOI Input);
        Boolean Delete(KNTC_LUUTHEODOI Input);
    }
    public class KNTC_LuutheodoiRepository : BaseRepository, IKNTC_LuutheodoiRepository
    {
        public List<KNTC_LUUTHEODOI> GetAll()
        {
            List<KNTC_LUUTHEODOI> resule = new List<KNTC_LUUTHEODOI>();
            resule = base.GetAll<KNTC_LUUTHEODOI>();
            return resule;
        }
        public List<KNTC_LUUTHEODOI> GetAll(Dictionary<string, object> condition)
        {
            List<KNTC_LUUTHEODOI> resule = new List<KNTC_LUUTHEODOI>();
            resule = base.GetAll<KNTC_LUUTHEODOI>(condition);
            return resule;
        }
        public List<KNTC_LUUTHEODOI> Getlist(String sql)
        {
            List<KNTC_LUUTHEODOI> resule = new List<KNTC_LUUTHEODOI>();
            resule = base.GetList<KNTC_LUUTHEODOI>(sql);
            return resule;
        }

        public KNTC_LUUTHEODOI GetByID(int ID)
        {
            return base.GetItem<KNTC_LUUTHEODOI>("ID", ID);
        }
        
        public KNTC_LUUTHEODOI AddNew(KNTC_LUUTHEODOI Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KNTC_LUUTHEODOI_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<KNTC_LUUTHEODOI>(Input))
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
        public Boolean Update(KNTC_LUUTHEODOI Input)
        {

            if (base.UpdateItem<KNTC_LUUTHEODOI>(Input, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KNTC_LUUTHEODOI Input)
        {

            if (base.DeleteItem<KNTC_LUUTHEODOI>(Input, "ID"))
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
