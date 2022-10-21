using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{


    public class KN_Doangiamsat_ykienRepository : BaseRepository
    {
        public List<KN_DOANGIAMSAT_YKIEN> GetAll()
        {
            List<KN_DOANGIAMSAT_YKIEN> resule = new List<KN_DOANGIAMSAT_YKIEN>();
            resule = base.GetAll<KN_DOANGIAMSAT_YKIEN>();
            return resule;
        }
        public List<KN_DOANGIAMSAT_YKIEN> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_DOANGIAMSAT_YKIEN> resule = new List<KN_DOANGIAMSAT_YKIEN>();
            resule = base.GetAll<KN_DOANGIAMSAT_YKIEN>(dictionary);
            return resule;
        }
        public List<KN_DOANGIAMSAT_YKIEN> GetList(String sql)
        {
            List<KN_DOANGIAMSAT_YKIEN> resule = new List<KN_DOANGIAMSAT_YKIEN>();
            resule = base.GetList<KN_DOANGIAMSAT_YKIEN>(sql);
            return resule;
        }
        public KN_DOANGIAMSAT_YKIEN GetByID(int ID)
        {
            return base.GetItem<KN_DOANGIAMSAT_YKIEN>("IYKIEN", ID);
        }

        public KN_DOANGIAMSAT_YKIEN GetByUseName(String actionInput)
        {
            return base.GetItem<KN_DOANGIAMSAT_YKIEN>("IYKIEN", actionInput);
        }
        public KN_DOANGIAMSAT_YKIEN AddNew(KN_DOANGIAMSAT_YKIEN actionInput)
        {

            decimal ID = 0;
            ID = base.GetNextValSeq("KN_DOANGIAMSAT_YKIEN_SEQ");
            if (ID != 0)
            {
                actionInput.IYKIEN = ID;
                if (base.InsertItem<KN_DOANGIAMSAT_YKIEN>(actionInput))
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
        public Boolean Update(KN_DOANGIAMSAT_YKIEN actionInput)
        {

            if (base.UpdateItem<KN_DOANGIAMSAT_YKIEN>(actionInput, "IYKIEN"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_DOANGIAMSAT_YKIEN actionInput)
        {

            if (base.DeleteItem<KN_DOANGIAMSAT_YKIEN>(actionInput, "IYKIEN"))
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
