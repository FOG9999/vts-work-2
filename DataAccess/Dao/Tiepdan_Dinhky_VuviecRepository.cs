using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUTiepdan_Dinhky_VuviecRepository
    {
        TIEPDAN_DINHKY_VUVIEC GetByID(int ID);
        List<TIEPDAN_DINHKY_VUVIEC> GetAll();
        List<TIEPDAN_DINHKY_VUVIEC> GetAll(Dictionary<string, object> dictionary);
        TIEPDAN_DINHKY_VUVIEC AddNew(TIEPDAN_DINHKY_VUVIEC UserInput);
        Boolean Update(TIEPDAN_DINHKY_VUVIEC UserInput);
        Boolean Delete(TIEPDAN_DINHKY_VUVIEC UserInput);
    }
    public class Tiepdan_Dinhky_VuviecRepository : BaseRepository, IUTiepdan_Dinhky_VuviecRepository
    {
        public List<TIEPDAN_DINHKY_VUVIEC> GetAll()
        {
            List<TIEPDAN_DINHKY_VUVIEC> resule = new List<TIEPDAN_DINHKY_VUVIEC>();
            resule = base.GetAll<TIEPDAN_DINHKY_VUVIEC>();
            return resule;
        }
        public List<TIEPDAN_DINHKY_VUVIEC> GetAll(Dictionary<string, object> dictionary)
        {
            List<TIEPDAN_DINHKY_VUVIEC> resule = new List<TIEPDAN_DINHKY_VUVIEC>();
            resule = base.GetAll<TIEPDAN_DINHKY_VUVIEC>(dictionary);

            return resule;
        }
        public TIEPDAN_DINHKY_VUVIEC GetByID(int ID)
        {
            return base.GetItem<TIEPDAN_DINHKY_VUVIEC>("IVUVIEC", ID);
        }
        public TIEPDAN_DINHKY_VUVIEC AddNew(TIEPDAN_DINHKY_VUVIEC actionInput)
        {

            if (base.InsertItem<TIEPDAN_DINHKY_VUVIEC>(actionInput))
            {
                return actionInput;
            }
            else
            {
                return null;
            }
        }
        public Boolean Update(TIEPDAN_DINHKY_VUVIEC actionInput)
        {

            if (base.UpdateItem<TIEPDAN_DINHKY_VUVIEC>(actionInput, "IVUVIEC"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(TIEPDAN_DINHKY_VUVIEC actionInput)
        {

            if (base.DeleteItem<TIEPDAN_DINHKY_VUVIEC>(actionInput, "IVUVIEC"))
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
