using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.DataAccess;
namespace DataAccess.Dao
{
    public interface IULocalRepository
    {
        List<DIAPHUONG> GetAll();
        DIAPHUONG GetByID(int ID);
        DIAPHUONG AddNew(DIAPHUONG localInput);
        Boolean Update(DIAPHUONG localInput);
        Boolean Delete(DIAPHUONG localInput);
        
      
    }
    public class LocalRepository : BaseRepository, IULocalRepository
    {
        public List<DIAPHUONG> GetAll()
        {
            List<DIAPHUONG> resule = new List<DIAPHUONG>();
            resule = base.GetAll<DIAPHUONG>();
            return resule;
        }
   
        public DIAPHUONG GetByID(int ID)
        {
            return base.GetItem<DIAPHUONG>("IDIAPHUONG", ID);
        }
       
        public DIAPHUONG AddNew(DIAPHUONG Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("DIAPHUONG_SEQ");
            if (ID != 0)
            {
                Input.IDIAPHUONG = ID;
                if (base.InsertItem<DIAPHUONG>(Input))
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
        public Boolean Update(DIAPHUONG Input)
        {

            if (base.UpdateItem<DIAPHUONG>(Input, "IDIAPHUONG"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(DIAPHUONG Input)
        {

            if (base.DeleteItem<DIAPHUONG>(Input, "IDIAPHUONG"))
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
