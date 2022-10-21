using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IKN_TUKHOARepository
    {
        KN_TUKHOA GetByID(int ID);
        KN_TUKHOA GetByUseName(String Username);
        KN_TUKHOA AddNew(KN_TUKHOA UserInput);
        Boolean Update(KN_TUKHOA UserInput);
        Boolean Delete(KN_TUKHOA UserInput);
    }
    public class KN_TukhoaRepository : BaseRepository, IKN_TUKHOARepository
    {
        public List<KN_TUKHOA> GetList(String sql)
        {
            List<KN_TUKHOA> resule = new List<KN_TUKHOA>();
            resule = base.GetList<KN_TUKHOA>(sql);
            return resule;
        }
        public List<KN_TUKHOA> GetAll(Dictionary<string, object> condition)
        {
            List<KN_TUKHOA> resule = new List<KN_TUKHOA>();
            resule = base.GetAll<KN_TUKHOA>(condition);
            return resule;
        }
        public KN_TUKHOA GetByID(int ID)
        {
            return base.GetItem<KN_TUKHOA>("ID", ID);
        }

        public KN_TUKHOA GetByUseName(String actionInput)
        {
            return base.GetItem<KN_TUKHOA>("ID", actionInput);
        }
        
        public KN_TUKHOA AddNew(KN_TUKHOA Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KN_TUKHOA_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<KN_TUKHOA>(Input))
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
        public Boolean Update(KN_TUKHOA actionInput)
        {

            if (base.UpdateItem<KN_TUKHOA>(actionInput, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_TUKHOA actionInput)
        {

            if (base.DeleteItem<KN_TUKHOA>(actionInput, "ID"))
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
