using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    public interface IUKN_Chuongtrinh_daibieuRepository
    {
        List<KN_CHUONGTRINH_DAIBIEU> GetAll();
        List<KN_CHUONGTRINH_DAIBIEU> GetAll(Dictionary<string, object> condition);
        KN_CHUONGTRINH_DAIBIEU AddNew(KN_CHUONGTRINH_DAIBIEU kn_chuongtrinh_daibieuInput);
        Boolean Update(KN_CHUONGTRINH_DAIBIEU kn_chuongtrinh_daibieuInput);
        Boolean Delete(KN_CHUONGTRINH_DAIBIEU kn_chuongtrinh_daibieuInput);
    }
    public class KN_Chuongtrinh_DaibieuRepository : BaseRepository, IUKN_Chuongtrinh_daibieuRepository
    {
        public List<KN_CHUONGTRINH_DAIBIEU> GetAll()
        {
            List<KN_CHUONGTRINH_DAIBIEU> resule = new List<KN_CHUONGTRINH_DAIBIEU>();
            resule = base.GetAll<KN_CHUONGTRINH_DAIBIEU>();
            return resule;
        }
        public List<KN_CHUONGTRINH_DAIBIEU> GetAll(Dictionary<string, object> condition)
        {
            List<KN_CHUONGTRINH_DAIBIEU> resule = new List<KN_CHUONGTRINH_DAIBIEU>();
            resule = base.GetAll<KN_CHUONGTRINH_DAIBIEU>(condition);
            return resule;
        }
        
        public KN_CHUONGTRINH_DAIBIEU GetByID(int ID)
        {
            return base.GetItem<KN_CHUONGTRINH_DAIBIEU>("ID", ID);
        }

   
        public KN_CHUONGTRINH_DAIBIEU AddNew(KN_CHUONGTRINH_DAIBIEU Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KN_CHUONGTRINH_DAIBIEU_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<KN_CHUONGTRINH_DAIBIEU>(Input))
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

        public Boolean Update(KN_CHUONGTRINH_DAIBIEU kn_chuongtrinh_daibieuInput)
        {

            if (base.UpdateItem<KN_CHUONGTRINH_DAIBIEU>(kn_chuongtrinh_daibieuInput, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_CHUONGTRINH_DAIBIEU kn_chuongtrinh_daibieuInput)
        {

            if (base.DeleteItem<KN_CHUONGTRINH_DAIBIEU>(kn_chuongtrinh_daibieuInput, "ID"))
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
