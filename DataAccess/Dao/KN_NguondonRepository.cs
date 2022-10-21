using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUKN_NguondonRepository
    {
        List<KN_NGUONDON> GetAll();
        KN_NGUONDON GetByID(int ID);
        KN_NGUONDON AddNew(KN_NGUONDON Input);
        Boolean Update(KN_NGUONDON Input);
        Boolean Delete(KN_NGUONDON Input);
    }
    public class KN_NguondonRepository : BaseRepository, IUKN_NguondonRepository
    {
        public List<KN_NGUONDON> GetAll()
        {
            List<KN_NGUONDON> resule = new List<KN_NGUONDON>();
            resule = base.GetAll<KN_NGUONDON>();
            return resule;
        }
        public List<KN_NGUONDON> GetAll(Dictionary<string, object> condition)
        {
            List<KN_NGUONDON> resule = new List<KN_NGUONDON>();
            resule = base.GetAll<KN_NGUONDON>(condition);
            return resule;
        }
        public List<KN_NGUONDON> Getlist(String sql)
        {
            List<KN_NGUONDON> resule = new List<KN_NGUONDON>();
            resule = base.GetList<KN_NGUONDON>(sql);
            return resule;
        }
        public KN_NGUONDON GetByID(int ID)
        {
            return base.GetItem<KN_NGUONDON>("INGUONDON", ID);
        }
        public KN_NGUONDON AddNew(KN_NGUONDON Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KN_NGUONDON_SEQ");
            if (ID != 0)
            {
                Input.INGUONDON = ID;
                if (base.InsertItem<KN_NGUONDON>(Input))
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
        public Boolean Update(KN_NGUONDON Input)
        {

            if (base.UpdateItem<KN_NGUONDON>(Input, "INGUONDON"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KN_NGUONDON Input)
        {

            if (base.DeleteItem<KN_NGUONDON>(Input, "INGUONDON"))
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
