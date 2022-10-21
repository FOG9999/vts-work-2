using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DataAccess.Dao
{

    public interface IULicSuDonRepository
    {

        KNTC_DON_LICHSU AddNew(KNTC_DON_LICHSU UserInput);
        List<KNTC_DON_LICHSU> GetAll();
    }
    public class KNTC_LichsudonRepository : BaseRepository, IULicSuDonRepository
    {
        public KNTC_DON_LICHSU AddNew(KNTC_DON_LICHSU Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("DANTOC_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<KNTC_DON_LICHSU>(Input))
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
        public List<KNTC_DON_LICHSU> GetAll()
        {
            List<KNTC_DON_LICHSU> resule = new List<KNTC_DON_LICHSU>();
            resule = base.GetAll<KNTC_DON_LICHSU>();
            return resule;
        }

    }
}
