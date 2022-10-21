using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    public interface IUKN_giamsatRepository
    {
        List<KN_GIAMSAT> GetAll();
        List<KN_GIAMSAT> GetAll(Dictionary<string, object> dictionary);
        KN_GIAMSAT GetByID(int ID);
        List<KN_GIAMSAT> GetByKienNghiID(int ID);
        KN_GIAMSAT AddNew(KN_GIAMSAT kngsInput);
        Boolean Update(KN_GIAMSAT kngsInput);
        Boolean Delete(KN_GIAMSAT kngsInput);
    }
    public class KN_giamsatRepository : BaseRepository, IUKN_giamsatRepository
    {
        public List<KN_GIAMSAT> GetAll()
        {
            List<KN_GIAMSAT> resule = new List<KN_GIAMSAT>();
            resule = base.GetAll<KN_GIAMSAT>();

            return resule;

        }
        
        public List<KN_GIAMSAT> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_GIAMSAT> resule = new List<KN_GIAMSAT>();
            resule = base.GetAll<KN_GIAMSAT>(dictionary);

            return resule;
        }
        public KN_GIAMSAT GetByID(int ID)
        {
            return base.GetItem<KN_GIAMSAT>("IGIAMSAT", ID);
        }
        public List<KN_GIAMSAT> GetByKienNghiID(int ID)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("IKIENNGHI", ID);
            return base.GetAll<KN_GIAMSAT>(dictionary);
        }
        public KN_GIAMSAT AddNew(KN_GIAMSAT actionInput)
        {

            decimal ID = 0;
            ID = base.GetNextValSeq("KN_GIAMSAT_SEQ");
            if (ID != 0)
            {
                actionInput.IGIAMSAT = ID;
                if (base.InsertItem<KN_GIAMSAT>(actionInput))
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
        public Boolean Update(KN_GIAMSAT kngsInput)
        {

            if (base.UpdateItem<KN_GIAMSAT>(kngsInput, "IGIAMSAT"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KN_GIAMSAT kngsInput)
        {

            if (base.DeleteItem<KN_GIAMSAT>(kngsInput, "IGIAMSAT"))
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
