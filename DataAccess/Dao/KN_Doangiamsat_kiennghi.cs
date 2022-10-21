using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{


    public class KN_Doangiamsat_kiennghiRepository : BaseRepository
    {
        public List<KN_DOANGIAMSAT_KIENNGHI> GetAll()
        {
            List<KN_DOANGIAMSAT_KIENNGHI> resule = new List<KN_DOANGIAMSAT_KIENNGHI>();
            resule = base.GetAll<KN_DOANGIAMSAT_KIENNGHI>();
            return resule;
        }
        public List<KN_DOANGIAMSAT_KIENNGHI> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_DOANGIAMSAT_KIENNGHI> resule = new List<KN_DOANGIAMSAT_KIENNGHI>();
            resule = base.GetAll<KN_DOANGIAMSAT_KIENNGHI>(dictionary);
            return resule;
        }
        public List<KN_DOANGIAMSAT_KIENNGHI> GetList(String sql)
        {
            List<KN_DOANGIAMSAT_KIENNGHI> resule = new List<KN_DOANGIAMSAT_KIENNGHI>();
            resule = base.GetList<KN_DOANGIAMSAT_KIENNGHI>(sql);
            return resule;
        }
        public KN_DOANGIAMSAT_KIENNGHI GetByID(int ID)
        {
            return base.GetItem<KN_DOANGIAMSAT_KIENNGHI>("ID", ID);
        }

        public KN_DOANGIAMSAT_KIENNGHI GetByUseName(String actionInput)
        {
            return base.GetItem<KN_DOANGIAMSAT_KIENNGHI>("ID", actionInput);
        }
        public KN_DOANGIAMSAT_KIENNGHI AddNew(KN_DOANGIAMSAT_KIENNGHI actionInput)
        {

            decimal ID = 0;
            ID = base.GetNextValSeq("KN_DOANGIAMSAT_KIENNGHI_SEQ");
            if (ID != 0)
            {
                actionInput.ID = ID;
                if (base.InsertItem<KN_DOANGIAMSAT_KIENNGHI>(actionInput))
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
        public Boolean Update(KN_DOANGIAMSAT_KIENNGHI actionInput)
        {

            if (base.UpdateItem<KN_DOANGIAMSAT_KIENNGHI>(actionInput, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_DOANGIAMSAT_KIENNGHI actionInput)
        {

            if (base.DeleteItem<KN_DOANGIAMSAT_KIENNGHI>(actionInput, "ID"))
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
