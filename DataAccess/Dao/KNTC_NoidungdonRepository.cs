using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUKNTC_NoidungdonRepository
    {
        List<KNTC_NOIDUNGDON> GetAll();
        KNTC_NOIDUNGDON GetByID(int ID);
        KNTC_NOIDUNGDON AddNew(KNTC_NOIDUNGDON UserInput);
        Boolean Update(KNTC_NOIDUNGDON UserInput);
        Boolean Delete(KNTC_NOIDUNGDON UserInput);
    }
    public class KNTC_NoidungdonRepository : BaseRepository, IUKNTC_NoidungdonRepository
    {
        public List<KNTC_NOIDUNGDON> GetAll()
        {
            List<KNTC_NOIDUNGDON> resule = new List<KNTC_NOIDUNGDON>();
            resule = base.GetAll<KNTC_NOIDUNGDON>();
            return resule;
        }
        public List<KNTC_NOIDUNGDON> GetAll(Dictionary<string,object> condition)
        {
            List<KNTC_NOIDUNGDON> resule = new List<KNTC_NOIDUNGDON>();
            resule = base.GetAll<KNTC_NOIDUNGDON>(condition);
            return resule;
        }
        public List<KNTC_NOIDUNGDON> GetList(String sql)
        {
            List<KNTC_NOIDUNGDON> resule = new List<KNTC_NOIDUNGDON>();
            resule = base.GetList<KNTC_NOIDUNGDON>(sql);
            return resule;
        }
        public KNTC_NOIDUNGDON GetByID(int ID)
        {
            return base.GetItem<KNTC_NOIDUNGDON>("INOIDUNG", ID);
        }     
        public KNTC_NOIDUNGDON AddNew(KNTC_NOIDUNGDON Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KNTC_NOIDUNGDON_SEQ");
            if (ID != 0)
            {
                Input.INOIDUNG = ID;
                if (base.InsertItem<KNTC_NOIDUNGDON>(Input))
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
        public Boolean Update(KNTC_NOIDUNGDON Input)
        {

            if (base.UpdateItem<KNTC_NOIDUNGDON>(Input, "INOIDUNG"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KNTC_NOIDUNGDON Input)
        {

            if (base.DeleteItem<KNTC_NOIDUNGDON>(Input, "INOIDUNG"))
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
