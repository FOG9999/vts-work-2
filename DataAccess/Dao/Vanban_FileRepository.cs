using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utilities;
using System.Threading.Tasks;
namespace DataAccess.Dao
{
    public interface IUVB_Vanban_FileRepository
    {
        VB_FILE_VANBAN GetByID(int ID);
        List<VB_FILE_VANBAN> GetAll();
        List<VB_FILE_VANBAN> GetAll(Dictionary<string, object> dictionary);
        VB_FILE_VANBAN AddNew(VB_FILE_VANBAN UserInput);
        Boolean Update(VB_FILE_VANBAN UserInput);
        Boolean Delete(VB_FILE_VANBAN UserInput);
        List<VB_FILE_VANBAN> GetList(String sql);

    }
    public class Vanban_FileRepository : BaseRepository, IUVB_Vanban_FileRepository
    {
        public List<VB_FILE_VANBAN> GetAll()
        {
            List<VB_FILE_VANBAN> resule = new List<VB_FILE_VANBAN>();
            resule = base.GetAll<VB_FILE_VANBAN>();

            return resule;

        }
        public List<VB_FILE_VANBAN> GetAll(Dictionary<string, object> dictionary)
        {
            List<VB_FILE_VANBAN> resule = new List<VB_FILE_VANBAN>();
            resule = base.GetAll<VB_FILE_VANBAN>(dictionary);

            return resule;
        }
        public List<VB_FILE_VANBAN> GetList(String sql)
        {
            List<VB_FILE_VANBAN> resule = new List<VB_FILE_VANBAN>();
            resule = base.GetList<VB_FILE_VANBAN>(sql);
            return resule;
        }
        
        public VB_FILE_VANBAN GetByID(int ID)
        {
            return base.GetItem<VB_FILE_VANBAN>("IFILE", ID);
        }
        public VB_FILE_VANBAN AddNew(VB_FILE_VANBAN Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("VB_FILE_VANBAN_SEQ");
            if (ID != 0)
            {
                Input.IFILE = ID;
                if (base.InsertItem<VB_FILE_VANBAN>(Input))
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
        public Boolean Update(VB_FILE_VANBAN actionInput)
        {

            if (base.UpdateItem<VB_FILE_VANBAN>(actionInput, "IFILE"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(VB_FILE_VANBAN actionInput)
        {

            if (base.DeleteItem<VB_FILE_VANBAN>(actionInput, "IFILE"))
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
