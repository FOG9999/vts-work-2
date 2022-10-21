using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    public interface IUKN_chuongtrinh_diaphuongRepository
    {
        List<KN_CHUONGTRINH_DIAPHUONG> GetAll();
        List<KN_CHUONGTRINH_DIAPHUONG> GetAll(Dictionary<string, object> condition);
        KN_CHUONGTRINH_DIAPHUONG AddNew(KN_CHUONGTRINH_DIAPHUONG knctdpInput);
        Boolean Update(KN_CHUONGTRINH_DIAPHUONG knctdpInput);
        Boolean Delete(KN_CHUONGTRINH_DIAPHUONG knctdpInput);
    }
    public class KN_Chuongtrinh_diaphuongRepository : BaseRepository, IUKN_chuongtrinh_diaphuongRepository
    {
        public List<KN_CHUONGTRINH_DIAPHUONG> GetAll()
        {
            List<KN_CHUONGTRINH_DIAPHUONG> resule = new List<KN_CHUONGTRINH_DIAPHUONG>();
            resule = base.GetAll<KN_CHUONGTRINH_DIAPHUONG>();
            return resule;
        }
        public List<KN_CHUONGTRINH_DIAPHUONG> GetAll(Dictionary<string, object> condition)
        {
            List<KN_CHUONGTRINH_DIAPHUONG> resule = new List<KN_CHUONGTRINH_DIAPHUONG>();
            resule = base.GetAll<KN_CHUONGTRINH_DIAPHUONG>(condition);
            return resule;
        }
        public KN_CHUONGTRINH_DIAPHUONG GetByID(int ID)
        {
            return base.GetItem<KN_CHUONGTRINH_DIAPHUONG>("ID", ID);
        }
      
        public KN_CHUONGTRINH_DIAPHUONG AddNew(KN_CHUONGTRINH_DIAPHUONG Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KN_CHUONGTRINH_DIAPHUONG_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<KN_CHUONGTRINH_DIAPHUONG>(Input))
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
        public Boolean Update(KN_CHUONGTRINH_DIAPHUONG knctdpInput)
        {

            if (base.UpdateItem<KN_CHUONGTRINH_DIAPHUONG>(knctdpInput, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_CHUONGTRINH_DIAPHUONG knctdpInput)
        {

            if (base.DeleteItem<KN_CHUONGTRINH_DIAPHUONG>(knctdpInput, "ID"))
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
