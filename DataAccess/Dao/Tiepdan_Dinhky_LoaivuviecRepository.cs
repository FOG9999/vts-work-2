using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IUTiepdan_Dinhky_LoaivuviecRepository
    {
        TIEPDAN_DINHKY_LOAIVUVIEC GetByID(int ID);
        List<TIEPDAN_DINHKY_LOAIVUVIEC> GetAll();
        List<TIEPDAN_DINHKY_LOAIVUVIEC> GetAll(Dictionary<string, object> dictionary);
        TIEPDAN_DINHKY_LOAIVUVIEC AddNew(TIEPDAN_DINHKY_LOAIVUVIEC UserInput);
        Boolean Update(TIEPDAN_DINHKY_LOAIVUVIEC UserInput);
        Boolean Delete(TIEPDAN_DINHKY_LOAIVUVIEC UserInput);
    }
    public class Tiepdan_Dinhky_LoaivuviecRepository : BaseRepository, IUTiepdan_Dinhky_LoaivuviecRepository
    {
        public List<TIEPDAN_DINHKY_LOAIVUVIEC> GetAll()
        {
            List<TIEPDAN_DINHKY_LOAIVUVIEC> resule = new List<TIEPDAN_DINHKY_LOAIVUVIEC>();
            resule = base.GetAll<TIEPDAN_DINHKY_LOAIVUVIEC>();
            return resule;
        }
        public List<TIEPDAN_DINHKY_LOAIVUVIEC> GetAll(Dictionary<string, object> dictionary)
        {
            List<TIEPDAN_DINHKY_LOAIVUVIEC> resule = new List<TIEPDAN_DINHKY_LOAIVUVIEC>();
            resule = base.GetAll<TIEPDAN_DINHKY_LOAIVUVIEC>(dictionary);

            return resule;
        }
        public TIEPDAN_DINHKY_LOAIVUVIEC GetByID(int ID)
        {
            return base.GetItem<TIEPDAN_DINHKY_LOAIVUVIEC>("ID", ID);
        }
        public TIEPDAN_DINHKY_LOAIVUVIEC AddNew(TIEPDAN_DINHKY_LOAIVUVIEC Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("TIEPDAN_DINHKY_LOAIVUVIEC_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<TIEPDAN_DINHKY_LOAIVUVIEC>(Input))
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
        public Boolean Update(TIEPDAN_DINHKY_LOAIVUVIEC actionInput)
        {

            if (base.UpdateItem<TIEPDAN_DINHKY_LOAIVUVIEC>(actionInput, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(TIEPDAN_DINHKY_LOAIVUVIEC actionInput)
        {

            if (base.DeleteItem<TIEPDAN_DINHKY_LOAIVUVIEC>(actionInput, "ID"))
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
