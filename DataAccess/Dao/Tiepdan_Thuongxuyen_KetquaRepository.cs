using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface ITIEPDAN_THUONGXUYEN_KETQUARepository
    {
        TIEPDAN_THUONGXUYEN_KETQUA GetByID(int ID);
        List<TIEPDAN_THUONGXUYEN_KETQUA> GetAll();
        List<TIEPDAN_THUONGXUYEN_KETQUA> GetAll(Dictionary<string, object> condition);
        TIEPDAN_THUONGXUYEN_KETQUA AddNew(TIEPDAN_THUONGXUYEN_KETQUA UserInput);
        Boolean Update(TIEPDAN_THUONGXUYEN_KETQUA UserInput);
        Boolean Delete(TIEPDAN_THUONGXUYEN_KETQUA UserInput);
    }
    public class Tiepdan_Thuongxuyen_KetquaRepository : BaseRepository, ITIEPDAN_THUONGXUYEN_KETQUARepository
    {
        public List<TIEPDAN_THUONGXUYEN_KETQUA> GetAll()
        {
            List<TIEPDAN_THUONGXUYEN_KETQUA> resule = new List<TIEPDAN_THUONGXUYEN_KETQUA>();
            resule = base.GetAll<TIEPDAN_THUONGXUYEN_KETQUA>();
            return resule;
        }
        public List<TIEPDAN_THUONGXUYEN_KETQUA> GetAll(Dictionary<string,object> condition)
        {
            List<TIEPDAN_THUONGXUYEN_KETQUA> resule = new List<TIEPDAN_THUONGXUYEN_KETQUA>();
            resule = base.GetAll<TIEPDAN_THUONGXUYEN_KETQUA>();
            return resule;
        }
        public TIEPDAN_THUONGXUYEN_KETQUA GetByID(int ID)
        {
            return base.GetItem<TIEPDAN_THUONGXUYEN_KETQUA>("IKETQUA", ID);
        }

        public TIEPDAN_THUONGXUYEN_KETQUA GetByUseName(String actionInput)
        {
            return base.GetItem<TIEPDAN_THUONGXUYEN_KETQUA>("IKETQUA", actionInput);
        }
        public TIEPDAN_THUONGXUYEN_KETQUA AddNew(TIEPDAN_THUONGXUYEN_KETQUA Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("TIEPDAN_THUONGXUYEN_KETQUA_SEQ");
            if (ID != 0)
            {
                Input.ITHUONGXUYEN = ID;
                if (base.InsertItem<TIEPDAN_THUONGXUYEN_KETQUA>(Input))
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
        public Boolean Update(TIEPDAN_THUONGXUYEN_KETQUA actionInput)
        {

            if (base.UpdateItem<TIEPDAN_THUONGXUYEN_KETQUA>(actionInput, "IKETQUA"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(TIEPDAN_THUONGXUYEN_KETQUA actionInput)
        {

            if (base.DeleteItem<TIEPDAN_THUONGXUYEN_KETQUA>(actionInput, "IKETQUA"))
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
