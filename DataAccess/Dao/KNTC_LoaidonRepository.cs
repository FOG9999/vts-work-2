using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUKNTC_LoaidonRepository
    {
        KNTC_LOAIDON GetByID(int ID);
        List<KNTC_LOAIDON> GetAll();
        KNTC_LOAIDON AddNew(KNTC_LOAIDON Input);
        Boolean Update(KNTC_LOAIDON Input);
        Boolean Delete(KNTC_LOAIDON Input);
    }
    public class KNTC_LoaidonRepository : BaseRepository, IUKNTC_LoaidonRepository
    {
        public List<KNTC_LOAIDON> GetAll()
        {
            List<KNTC_LOAIDON> resule = new List<KNTC_LOAIDON>();
            resule = base.GetAll<KNTC_LOAIDON>();
            return resule;
        }
        public List<KNTC_LOAIDON> GetAll(Dictionary<string,object> condition)
        {
            List<KNTC_LOAIDON> resule = new List<KNTC_LOAIDON>();
            resule = base.GetAll<KNTC_LOAIDON>(condition);
            return resule;
        }
        public List<KNTC_LOAIDON> Getlist(String sql)
        {
            List<KNTC_LOAIDON> resule = new List<KNTC_LOAIDON>();
            resule = base.GetList<KNTC_LOAIDON>(sql);
            return resule;
        }
        public KNTC_LOAIDON GetByID(int ID)
        {
            return base.GetItem<KNTC_LOAIDON>("ILOAIDON", ID);
        }
        public KNTC_LOAIDON AddNew(KNTC_LOAIDON Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KNTC_LOAIDON_SEQ");
            if (ID != 0)
            {
                Input.ILOAIDON = ID;
                if (base.InsertItem<KNTC_LOAIDON>(Input))
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
        public Boolean Update(KNTC_LOAIDON Input)
        {

            if (base.UpdateItem<KNTC_LOAIDON>(Input, "ILOAIDON"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KNTC_LOAIDON Input)
        {

            if (base.DeleteItem<KNTC_LOAIDON>(Input, "ILOAIDON"))
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
