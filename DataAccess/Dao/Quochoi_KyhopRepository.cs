using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUQuochoi_KyhopRepository
    {
        List<QUOCHOI_KYHOP> GetAll();
        List<QUOCHOI_KYHOP> GetAll(Dictionary<string, object> condition);
        List<QUOCHOI_KYHOP> Getlist(String sql);
        QUOCHOI_KYHOP GetByID(int ID);    
        QUOCHOI_KYHOP AddNew(QUOCHOI_KYHOP Input);
        Boolean Update(QUOCHOI_KYHOP Input);
        Boolean Delete(QUOCHOI_KYHOP Input);
      
    }
    public class Quochoi_KyhopRepository : BaseRepository, IUQuochoi_KyhopRepository
    {
        public List<QUOCHOI_KYHOP> GetAll()
        {
            List<QUOCHOI_KYHOP> resule = new List<QUOCHOI_KYHOP>();
            resule = base.GetAll<QUOCHOI_KYHOP>();
            return resule;
        }
        public List<QUOCHOI_KYHOP> Getlist(String sql)
        {
            List<QUOCHOI_KYHOP> resule = new List<QUOCHOI_KYHOP>();
            resule = base.GetList<QUOCHOI_KYHOP>(sql);
            return resule;
        }
        public List<QUOCHOI_KYHOP> GetAll(Dictionary<string, object> condition)
        {
            List<QUOCHOI_KYHOP> resule = new List<QUOCHOI_KYHOP>();
            resule = base.GetAll<QUOCHOI_KYHOP>(condition);
            return resule;
        }
        public QUOCHOI_KYHOP GetByID(int ID)
        {
            return base.GetItem<QUOCHOI_KYHOP>("IKYHOP", ID);
        }       
        public QUOCHOI_KYHOP AddNew(QUOCHOI_KYHOP Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("QUOCHOI_KYHOP_SEQ");
            if (ID != 0)
            {
                Input.IKYHOP = ID;
                if (base.InsertItem<QUOCHOI_KYHOP>(Input))
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
        public Boolean Update(QUOCHOI_KYHOP Input)
        {

            if (base.UpdateItem<QUOCHOI_KYHOP>(Input, "IKYHOP"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(QUOCHOI_KYHOP Input)
        {

            if (base.DeleteItem<QUOCHOI_KYHOP>(Input, "IKYHOP"))
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
