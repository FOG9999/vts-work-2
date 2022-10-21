using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
   
    public class KN_Chuongtrinh_ChitietRepository : BaseRepository
    {
        public List<KN_CHUONGTRINH_CHITIET> GetAll()
        {
            List<KN_CHUONGTRINH_CHITIET> resule = new List<KN_CHUONGTRINH_CHITIET>();
            resule = base.GetAll<KN_CHUONGTRINH_CHITIET>();
            return resule;
        }
        public List<KN_CHUONGTRINH_CHITIET> GetAll(Dictionary<string, object> condition)
        {
            List<KN_CHUONGTRINH_CHITIET> resule = new List<KN_CHUONGTRINH_CHITIET>();
            resule = base.GetAll<KN_CHUONGTRINH_CHITIET>(condition);
            return resule;
        }

        public KN_CHUONGTRINH_CHITIET GetByID(int ID)
        {
            return base.GetItem<KN_CHUONGTRINH_CHITIET>("ID", ID);
        }


        public KN_CHUONGTRINH_CHITIET AddNew(KN_CHUONGTRINH_CHITIET Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KN_CHUONGTRINH_CHITIET_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<KN_CHUONGTRINH_CHITIET>(Input))
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

        public Boolean Update(KN_CHUONGTRINH_CHITIET kn_chuongtrinh_daibieuInput)
        {

            if (base.UpdateItem<KN_CHUONGTRINH_CHITIET>(kn_chuongtrinh_daibieuInput, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_CHUONGTRINH_CHITIET kn_chuongtrinh_daibieuInput)
        {

            if (base.DeleteItem<KN_CHUONGTRINH_CHITIET>(kn_chuongtrinh_daibieuInput, "ID"))
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
