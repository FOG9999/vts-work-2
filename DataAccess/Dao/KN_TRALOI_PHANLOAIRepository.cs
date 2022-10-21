using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utilities;
namespace DataAccess.Dao
{
    
    public class KN_TRALOI_PHANLOAIRepository : BaseRepository
    {
        public List<KN_TRALOI_PHANLOAI> GetAll()
        {
            List<KN_TRALOI_PHANLOAI> resule = new List<KN_TRALOI_PHANLOAI>();
            resule = base.GetAll<KN_TRALOI_PHANLOAI>();

            return resule;

        }
        public List<KN_TRALOI_PHANLOAI> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_TRALOI_PHANLOAI> resule = new List<KN_TRALOI_PHANLOAI>();
            resule = base.GetAll<KN_TRALOI_PHANLOAI>(dictionary);

            return resule;
        }
       
        public KN_TRALOI_PHANLOAI GetByID(int ID)
        {
            return base.GetItem<KN_TRALOI_PHANLOAI>("IPHANLOAI", ID);
        }

        public KN_TRALOI_PHANLOAI AddNew(KN_TRALOI_PHANLOAI Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KN_TRALOI_PHANLOAI_SEQ");
            if (ID != 0)
            {
                Input.IPHANLOAI = ID;
                if (base.InsertItem<KN_TRALOI_PHANLOAI>(Input))
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
        public Boolean Update(KN_TRALOI_PHANLOAI actionInput)
        {

            if (base.UpdateItem<KN_TRALOI_PHANLOAI>(actionInput, "IPHANLOAI"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KN_TRALOI_PHANLOAI actionInput)
        {

            if (base.DeleteItem<KN_TRALOI_PHANLOAI>(actionInput, "IPHANLOAI"))
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
