using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
namespace DataAccess.Dao
{
    public class KN_KIENNGHI_IMPORTRepository : BaseRepository
    {
        Log log = new Log();
        public List<KN_KIENNGHI_IMPORT> GetAll()
        {
            List<KN_KIENNGHI_IMPORT> resule = new List<KN_KIENNGHI_IMPORT>();
            resule = base.GetAll<KN_KIENNGHI_IMPORT>();
            return resule;
        }
        public List<KN_KIENNGHI_IMPORT> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_KIENNGHI_IMPORT> resule = new List<KN_KIENNGHI_IMPORT>();
            resule = base.GetAll<KN_KIENNGHI_IMPORT>(dictionary);
            return resule;
        }
        public KN_KIENNGHI_IMPORT GetByID(int ID)
        {
            return base.GetItem<KN_KIENNGHI_IMPORT>("ID", ID);
        }
        public KN_KIENNGHI_IMPORT AddNew(KN_KIENNGHI_IMPORT Input)
        {
            decimal IUse = 0;
            IUse = base.GetNextValSeq("KN_KIENNGHI_IMPORT_SEQ");
            if (IUse != 0)
            {
                Input.ID = IUse;
                if (base.InsertItem<KN_KIENNGHI_IMPORT>(Input))
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
        public Boolean Update(KN_KIENNGHI_IMPORT KN)
        {
            if (base.UpdateItem<KN_KIENNGHI_IMPORT>(KN, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete_Kiennghi_By_ID_Import(int id_import)
        {
            var param = new List<OracleParameter>();
            bool result= false;
            try
            {
                string sql = "DELETE from KN_KIENNGHI_IMPORT where ID_IMPORT = " + ":ID_IMPORT";
                param.Add(new OracleParameter("ID_IMPORT", id_import));
                
                result = base.ExcuteSQL(sql, param);
            }
            catch(Exception ex)
            {
                log.Log_Error(ex, "Xóa các kiến nghị đã import");
                throw;
            }
            return result;
        }
        
        public Boolean Delete(KN_KIENNGHI_IMPORT KN)
        {

            if (base.DeleteItem<KN_KIENNGHI_IMPORT>(KN, "ID"))
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
