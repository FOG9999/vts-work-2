using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utilities;
namespace DataAccess.Dao
{
    public interface IUDONVILINHVUCRepository
    {
        DONVI_LINHVUC GetByID_DONVILINHVUC(int ID);
        List<DONVI_LINHVUC> GetAll();
        List<DONVI_LINHVUC> GetAll(Dictionary<string, object> dictionary);
        DONVI_LINHVUC AddNew(DONVI_LINHVUC UserInput);
        Boolean Update(DONVI_LINHVUC UserInput);
        Boolean Delete(DONVI_LINHVUC UserInput);
        List<DONVI_LINHVUC> GetList(String sql);
    }

    public class DONVILINHVUCRepository : BaseRepository, IUDONVILINHVUCRepository
    {
        public List<DONVI_LINHVUC> GetAll()
        {
            List<DONVI_LINHVUC> resule = new List<DONVI_LINHVUC>();
            resule = base.GetAll<DONVI_LINHVUC>();

            return resule;

        }
        public List<DONVI_LINHVUC> GetAll(Dictionary<string, object> dictionary)
        {
            List<DONVI_LINHVUC> resule = new List<DONVI_LINHVUC>();
            resule = base.GetAll<DONVI_LINHVUC>(dictionary);

            return resule;
        }
        public List<DONVI_LINHVUC> GetList(String sql)
        {
            List<DONVI_LINHVUC> resule = new List<DONVI_LINHVUC>();
            resule = base.GetList<DONVI_LINHVUC>(sql);
            return resule;
        }
        public DONVI_LINHVUC GetByID_DONVILINHVUC(int ID)
        {


            return base.GetItem<DONVI_LINHVUC>("ID", ID);
        }

        public DONVI_LINHVUC AddNew(DONVI_LINHVUC Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("DONVI_LINHVUC_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<DONVI_LINHVUC>(Input))
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
        public Boolean Update(DONVI_LINHVUC actionInput)
        {

            if (base.UpdateItem<DONVI_LINHVUC>(actionInput, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(DONVI_LINHVUC actionInput)
        {

            if (base.DeleteItem<DONVI_LINHVUC>(actionInput, "ID"))
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
