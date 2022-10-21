using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IKNTC_GIAMSATRepository
    {
        List<KNTC_GIAMSAT> GetList(String sql);
        KNTC_GIAMSAT GetByID(int ID);
        KNTC_GIAMSAT GetByUseName(String Username);
        KNTC_GIAMSAT AddNew(KNTC_GIAMSAT UserInput);
        Boolean Update(KNTC_GIAMSAT UserInput);
        Boolean Delete(KNTC_GIAMSAT UserInput);
        List<KNTC_GIAMSAT> GetAll(Dictionary<string, object> dictionary);
    }
    public class KNTC_GiamsatRepository : BaseRepository, IKNTC_GIAMSATRepository
    {
        public List<KNTC_GIAMSAT> GetList(String sql)
        {
            List<KNTC_GIAMSAT> resule = new List<KNTC_GIAMSAT>();
            resule = base.GetList<KNTC_GIAMSAT>(sql);
            return resule;
        }
        public List<KNTC_GIAMSAT> GetAll()
        {
            List<KNTC_GIAMSAT> resule = new List<KNTC_GIAMSAT>();
            resule = base.GetAll<KNTC_GIAMSAT>();

            return resule;

        }
        public List<KNTC_GIAMSAT> GetAll(Dictionary<string, object> dictionary)
        {
            List<KNTC_GIAMSAT> resule = new List<KNTC_GIAMSAT>();
            resule = base.GetAll<KNTC_GIAMSAT>(dictionary);

            return resule;
        }
        public KNTC_GIAMSAT GetByID(int ID)
        {
            return base.GetItem<KNTC_GIAMSAT>("IGIAMSAT", ID);
        }

        public KNTC_GIAMSAT GetByUseName(String actionInput)
        {
            return base.GetItem<KNTC_GIAMSAT>("IGIAMSAT", actionInput);
        }
        public KNTC_GIAMSAT AddNew(KNTC_GIAMSAT actionInput)
        {

            decimal ID = 0;
            ID = base.GetNextValSeq("KNTC_GIAMSAT_SEQ");
            if (ID != 0)
            {
                actionInput.IGIAMSAT = ID;
                if (base.InsertItem<KNTC_GIAMSAT>(actionInput))
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
        public Boolean Update(KNTC_GIAMSAT actionInput)
        {

            if (base.UpdateItem<KNTC_GIAMSAT>(actionInput, "IGIAMSAT"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KNTC_GIAMSAT actionInput)
        {

            if (base.DeleteItem<KNTC_GIAMSAT>(actionInput, "IGIAMSAT"))
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
