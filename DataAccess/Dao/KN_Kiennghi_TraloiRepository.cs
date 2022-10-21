using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IKN_KIENNGHI_TRALOIRepository
    {
        List<KN_KIENNGHI_TRALOI> GetAll();
        List<KN_KIENNGHI_TRALOI> GetAll(Dictionary<string, object> condition);
        KN_KIENNGHI_TRALOI AddNew(KN_KIENNGHI_TRALOI UserInput);
        Boolean Update(KN_KIENNGHI_TRALOI UserInput);
        Boolean Delete(KN_KIENNGHI_TRALOI UserInput);
    }
    public class KN_Kiennghi_TraloiRepository : BaseRepository, IKN_KIENNGHI_TRALOIRepository
    {
        public List<KN_KIENNGHI_TRALOI> GetAll()
        {
            List<KN_KIENNGHI_TRALOI> resule = new List<KN_KIENNGHI_TRALOI>();
            resule = base.GetAll<KN_KIENNGHI_TRALOI>();
            return resule;
        }
        public List<KN_KIENNGHI_TRALOI> GetAll(Dictionary<string,object> condition)
        {
            List<KN_KIENNGHI_TRALOI> resule = new List<KN_KIENNGHI_TRALOI>();
            resule = base.GetAll<KN_KIENNGHI_TRALOI>(condition);
            return resule;
        }

        public KN_KIENNGHI_TRALOI GetByID(int ID)
        {
            return base.GetItem<KN_KIENNGHI_TRALOI>("ITRALOI", ID);
        }

        public KN_KIENNGHI_TRALOI GetByUseName(String actionInput)
        {
            return base.GetItem<KN_KIENNGHI_TRALOI>("ITRALOI", actionInput);
        }
        public Boolean Delete_Traloi_byIDKIENNGHI(int id_kiennghi)
        {
            bool result = true;
            try
            {
                var param = new List<OracleParameter>();
                string sql = "delete from KN_KIENNGHI_TRALOI where IKIENNGHI=:id_kiennghi";
                param.Add(new OracleParameter("id_kiennghi", id_kiennghi));
                result = base.ExcuteSQL(sql, param);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public KN_KIENNGHI_TRALOI AddNew(KN_KIENNGHI_TRALOI actionInput)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KN_KIENNGHI_TRALOI_SEQ");
            if (ID != 0)
            {
                actionInput.ITRALOI = ID;
                if (base.InsertItem<KN_KIENNGHI_TRALOI>(actionInput))
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
        public Boolean Update(KN_KIENNGHI_TRALOI actionInput)
        {

            if (base.UpdateItem<KN_KIENNGHI_TRALOI>(actionInput, "ITRALOI"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_KIENNGHI_TRALOI actionInput)
        {

            if (base.DeleteItem<KN_KIENNGHI_TRALOI>(actionInput, "ITRALOI"))
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
