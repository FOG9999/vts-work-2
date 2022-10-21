using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
   
    public interface IUQuochoi_KhoaRepository
    {
        List<QUOCHOI_KHOA> GetAll();
        List<QUOCHOI_KHOA> GetAll(Dictionary<string, object> condition);
        QUOCHOI_KHOA GetByID(int ID);
        QUOCHOI_KHOA GetByIMACDINH(int IMACDINH);
        QUOCHOI_KHOA AddNew(QUOCHOI_KHOA Input);
        Boolean Update(QUOCHOI_KHOA Input);
        Boolean Delete(QUOCHOI_KHOA Input);

    }
    public class Quochoi_KhoaRepository : BaseRepository, IUQuochoi_KhoaRepository
    {
        public List<QUOCHOI_KHOA> GetAll()
        {
            List<QUOCHOI_KHOA> resule = new List<QUOCHOI_KHOA>();
            resule = base.GetAll<QUOCHOI_KHOA>();
            return resule;
        }
        public List<QUOCHOI_KHOA> GetList(String sql)
        {
            List<QUOCHOI_KHOA> resule = new List<QUOCHOI_KHOA>();
            resule = base.GetList<QUOCHOI_KHOA>(sql);
            return resule;
        }
        public List<QUOCHOI_KHOA> GetAll(Dictionary<string, object> condition)
        {
            List<QUOCHOI_KHOA> resule = new List<QUOCHOI_KHOA>();
            resule = base.GetAll<QUOCHOI_KHOA>(condition);
            return resule;
        }
        public QUOCHOI_KHOA GetByID(int ID)
        {
            return base.GetItem<QUOCHOI_KHOA>("IKHOA", ID);
        }
        public QUOCHOI_KHOA GetByIMACDINH(int IMACDINH)
        {
            return base.GetItem<QUOCHOI_KHOA>("IMACDINH", IMACDINH);
        }
        public QUOCHOI_KHOA AddNew(QUOCHOI_KHOA Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("QUOCHOI_KHOA_SEQ");
            if (ID != 0)
            {
                Input.IKHOA = ID;
                if (base.InsertItem<QUOCHOI_KHOA>(Input))
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
        public Boolean Update(QUOCHOI_KHOA Input)
        {

            if (base.UpdateItem<QUOCHOI_KHOA>(Input, "IKHOA"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(QUOCHOI_KHOA Input)
        {

            if (base.DeleteItem<QUOCHOI_KHOA>(Input, "IKHOA"))
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
