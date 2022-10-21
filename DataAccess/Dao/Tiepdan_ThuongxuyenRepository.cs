using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{


    public interface ITIEPDAN_THUONGXUYENRepository
    {
        TIEPDAN_THUONGXUYEN GetByID(int ID);
        List<TIEPDAN_THUONGXUYEN> GetAll(Dictionary<string, object> condition);
        List<TIEPDAN_THUONGXUYEN> GetAll();
        TIEPDAN_THUONGXUYEN AddNew(TIEPDAN_THUONGXUYEN UserInput);
        Boolean Update(TIEPDAN_THUONGXUYEN UserInput);
        Boolean Delete(TIEPDAN_THUONGXUYEN UserInput);
    }
    public class Tiepdan_ThuongxuyenRepository : BaseRepository, ITIEPDAN_THUONGXUYENRepository
    {
        public List<TIEPDAN_THUONGXUYEN> GetAll()
        {
            List<TIEPDAN_THUONGXUYEN> resule = new List<TIEPDAN_THUONGXUYEN>();
            resule = base.GetAll<TIEPDAN_THUONGXUYEN>();
            return resule;
        }
        public List<TIEPDAN_THUONGXUYEN> GetAll(Dictionary<string,object> condition)
        {
            List<TIEPDAN_THUONGXUYEN> resule = new List<TIEPDAN_THUONGXUYEN>();
            resule = base.GetAll<TIEPDAN_THUONGXUYEN>(condition);
            return resule;
        }
        public TIEPDAN_THUONGXUYEN GetByID(int ID)
        {
            return base.GetItem<TIEPDAN_THUONGXUYEN>("ITHUONGXUYEN", ID);
        }
        public TIEPDAN_THUONGXUYEN AddNew(TIEPDAN_THUONGXUYEN Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("TIEPDAN_THUONGXUYEN_SEQ");
            if (ID != 0)
            {
                Input.ITHUONGXUYEN = ID;
                if (base.InsertItem<TIEPDAN_THUONGXUYEN>(Input))
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
        public Boolean Update(TIEPDAN_THUONGXUYEN actionInput)
        {

            if (base.UpdateItem<TIEPDAN_THUONGXUYEN>(actionInput, "ITHUONGXUYEN"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(TIEPDAN_THUONGXUYEN actionInput)
        {

            if (base.DeleteItem<TIEPDAN_THUONGXUYEN>(actionInput, "ITIEPDAN_THUONGXUYEN"))
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
