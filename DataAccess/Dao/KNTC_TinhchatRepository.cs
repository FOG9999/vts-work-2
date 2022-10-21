using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUKNTC_TinhchatRepository
    {
        List<KNTC_TINHCHAT> GetAll();    
        KNTC_TINHCHAT GetByID(int ID);
        KNTC_TINHCHAT AddNew(KNTC_TINHCHAT UserInput);
        Boolean Update(KNTC_TINHCHAT UserInput);
        Boolean Delete(KNTC_TINHCHAT UserInput);
    }
    public class KNTC_TinhchatRepository : BaseRepository, IUKNTC_TinhchatRepository
    {
        public List<KNTC_TINHCHAT> GetAll()
        {
            List<KNTC_TINHCHAT> resule = new List<KNTC_TINHCHAT>();
            resule = base.GetAll<KNTC_TINHCHAT>();
            return resule;
        }
        public List<KNTC_TINHCHAT> Getlist(String sql)
        {
            List<KNTC_TINHCHAT> resule = new List<KNTC_TINHCHAT>();
            resule = base.GetList<KNTC_TINHCHAT>(sql);
            return resule;
        }
        public List<KNTC_TINHCHAT> GetAll(Dictionary<string,object> condition)
        {
            List<KNTC_TINHCHAT> resule = new List<KNTC_TINHCHAT>();
            resule = base.GetAll<KNTC_TINHCHAT>(condition);
            return resule;
        }
        public KNTC_TINHCHAT GetByID(int ID)
        {
            return base.GetItem<KNTC_TINHCHAT>("ITINHCHAT", ID);
        }      
        public KNTC_TINHCHAT AddNew(KNTC_TINHCHAT Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KNTC_TINHCHAT_SEQ");
            if (ID != 0)
            {
                Input.ITINHCHAT = ID;
                if (base.InsertItem<KNTC_TINHCHAT>(Input))
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
        public Boolean Update(KNTC_TINHCHAT Input)
        {

            if (base.UpdateItem<KNTC_TINHCHAT>(Input, "ITINHCHAT"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KNTC_TINHCHAT Input)
        {

            if (base.DeleteItem<KNTC_TINHCHAT>(Input, "ITINHCHAT"))
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
