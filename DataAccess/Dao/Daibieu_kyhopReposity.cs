using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Oracle.ManagedDataAccess.Client;
namespace DataAccess.Dao
{
    public interface IUDaibieu_KyhopRepository
    {
        List<DAIBIEU_KYHOP> GetAll();
        DAIBIEU_KYHOP GetByID(int ID);
        List<DAIBIEU_KYHOP> GetAll(Dictionary<string, object> condition);
        DAIBIEU_KYHOP AddNew(DAIBIEU_KYHOP UserInput);
        Boolean Update(DAIBIEU_KYHOP UserInput);
        Boolean Delete(DAIBIEU_KYHOP UserInput);
        List<DAIBIEU_KYHOP> Getlist(String sql);
    }
    public class Daibieu_kyhopReposity : BaseRepository, IUDaibieu_KyhopRepository
    {
        public DAIBIEU_KYHOP GetByID(int ID)
        {
            return base.GetItem<DAIBIEU_KYHOP>("ID", ID);
        }
        public List<DAIBIEU_KYHOP> GetAll()
        {
            List<DAIBIEU_KYHOP> resule = new List<DAIBIEU_KYHOP>();
            resule = base.GetAll<DAIBIEU_KYHOP>();
            return resule;
        }
        public List<DAIBIEU_KYHOP> GetAll(Dictionary<string, object> condition)
        {
            List<DAIBIEU_KYHOP> resule = new List<DAIBIEU_KYHOP>();
            resule = base.GetAll<DAIBIEU_KYHOP>(condition);
            return resule;
        }
        public List<DAIBIEU_KYHOP> Getlist(String sql)
        {
            List<DAIBIEU_KYHOP> resule = new List<DAIBIEU_KYHOP>();
            resule = base.GetList<DAIBIEU_KYHOP>(sql);
            return resule;
        }

        public DAIBIEU_KYHOP AddNew(DAIBIEU_KYHOP UserInput)
        {
            decimal IUse = 0;
            IUse = base.GetNextValSeq("DAIBIEU_KYHOP_SEQ");
            if (IUse != 0)
            {
                UserInput.ID = IUse;
                if (base.InsertItem<DAIBIEU_KYHOP>(UserInput))
                {
                    return UserInput;
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
        public Boolean Update(DAIBIEU_KYHOP UserInput)
        {

            if (base.UpdateItem<DAIBIEU_KYHOP>(UserInput, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(DAIBIEU_KYHOP UserInput)
        {

            if (base.DeleteItem<DAIBIEU_KYHOP>(UserInput, "ID"))
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
