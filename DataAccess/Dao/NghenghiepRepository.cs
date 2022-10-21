using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
   
    public interface IUNghenghiepRepository
    {
       
        List<NGHENGHIEP> GetAll();
        List<NGHENGHIEP> GetAllWhere_Paging(Dictionary<string, object> dictionary);
        NGHENGHIEP GetByID(int ID);
        NGHENGHIEP AddNew(NGHENGHIEP Input);
        Boolean Update(NGHENGHIEP Input);
        Boolean Delete(NGHENGHIEP Input);
      
    }
    public class NghenghiepRepository : BaseRepository, IUNghenghiepRepository
    {
        public List<NGHENGHIEP> GetList(String sql)
        {
            List<NGHENGHIEP> resule = new List<NGHENGHIEP>();
            resule = base.GetList<NGHENGHIEP>(sql);
            return resule;
        }
        public List<NGHENGHIEP> GetAll()
        {
            List<NGHENGHIEP> resule = new List<NGHENGHIEP>();
            resule = base.GetAll<NGHENGHIEP>();
            return resule;
        }
        public List<NGHENGHIEP> GetAllWhere_Paging(Dictionary<string, object> dictionary)
        {
            List<NGHENGHIEP> resule = new List<NGHENGHIEP>();
            resule = base.GetAll<NGHENGHIEP>(dictionary);
            return resule;
        }
        public NGHENGHIEP GetByID(int ID)
        {
            return base.GetItem<NGHENGHIEP>("INGHENGHIEP", ID);
        }
        public NGHENGHIEP AddNew(NGHENGHIEP Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("NGHENGHIEP_SEQ");
            if (ID != 0)
            {
                Input.INGHENGHIEP = ID;
                if (base.InsertItem<NGHENGHIEP>(Input))
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
        public Boolean Update(NGHENGHIEP Input)
        {

            if (base.UpdateItem<NGHENGHIEP>(Input, "INGHENGHIEP"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(NGHENGHIEP Input)
        {

            if (base.DeleteItem<NGHENGHIEP>(Input, "INGHENGHIEP"))
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
