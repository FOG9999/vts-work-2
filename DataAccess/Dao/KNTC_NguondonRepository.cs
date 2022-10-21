using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUKNTC_NguondonRepository
    {
        List<KNTC_NGUONDON> GetAll();
        KNTC_NGUONDON GetByID(int ID);
        KNTC_NGUONDON AddNew(KNTC_NGUONDON Input);
        Boolean Update(KNTC_NGUONDON Input);
        Boolean Delete(KNTC_NGUONDON Input);
    }
    public class KNTC_NguondonRepository : BaseRepository, IUKNTC_NguondonRepository
    {
        public List<KNTC_NGUONDON> GetAll()
        {
            List<KNTC_NGUONDON> resule = new List<KNTC_NGUONDON>();
            resule = base.GetAll<KNTC_NGUONDON>();
            return resule;
        }
        public List<KNTC_NGUONDON> GetAll(Dictionary<string,object> condition)
        {
            List<KNTC_NGUONDON> resule = new List<KNTC_NGUONDON>();
            resule = base.GetAll<KNTC_NGUONDON>(condition);
            return resule;
        }
        public List<KNTC_NGUONDON> Getlist(String sql)
        {
            List<KNTC_NGUONDON> resule = new List<KNTC_NGUONDON>();
            resule = base.GetList<KNTC_NGUONDON>(sql);
            return resule;
        }
        public KNTC_NGUONDON GetByID(int ID)
        {
            return base.GetItem<KNTC_NGUONDON>("INGUONDON", ID);
        }   
        public KNTC_NGUONDON AddNew(KNTC_NGUONDON Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KNTC_NGUONDON_SEQ");
            if (ID != 0)
            {
                Input.INGUONDON = ID;
                if (base.InsertItem<KNTC_NGUONDON>(Input))
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
        public Boolean Update(KNTC_NGUONDON Input)
        {

            if (base.UpdateItem<KNTC_NGUONDON>(Input, "INGUONDON"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KNTC_NGUONDON Input)
        {

            if (base.DeleteItem<KNTC_NGUONDON>(Input, "INGUONDON"))
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
