using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
namespace DataAccess.Dao
{

   
    public class KN_DoangiamsatRepository : BaseRepository
    {
        Log log = new Log();
        public List<KN_DOANGIAMSAT> GetAll()
        {
            List<KN_DOANGIAMSAT> resule = new List<KN_DOANGIAMSAT>();
            resule = base.GetAll<KN_DOANGIAMSAT>();
            return resule;
        }
        public List<KN_DOANGIAMSAT> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_DOANGIAMSAT> resule = new List<KN_DOANGIAMSAT>();
            resule = base.GetAll<KN_DOANGIAMSAT>(dictionary);
            return resule;
        }
        public List<KN_DOANGIAMSAT> GetList(String sql)
        {
            List<KN_DOANGIAMSAT> resule = new List<KN_DOANGIAMSAT>();
            resule = base.GetList<KN_DOANGIAMSAT>(sql);
            return resule;
        }
        public KN_DOANGIAMSAT GetByID(int ID)
        {
            return base.GetItem<KN_DOANGIAMSAT>("IDOAN", ID);
        }

        public KN_DOANGIAMSAT GetByUseName(String actionInput)
        {
            return base.GetItem<KN_DOANGIAMSAT>("IDOAN", actionInput);
        }
        public KN_DOANGIAMSAT AddNew(KN_DOANGIAMSAT actionInput)
        {

            decimal ID = 0;
            ID = base.GetNextValSeq("KN_DOANGIAMSAT_SEQ");
            if (ID != 0)
            {
                actionInput.IDOAN = ID;
                if (base.InsertItem<KN_DOANGIAMSAT>(actionInput))
                {
                    return actionInput;
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
        public List<KN_DOANGIAMSAT> Search_Giamsat(KN_DOANGIAMSAT giamsat)
        {
            List<KN_DOANGIAMSAT> listObj = new List<KN_DOANGIAMSAT>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "select * from KN_DOANGIAMSAT where IDONVI!=0";
                if (giamsat.IDONVI != 0)
                {
                    sql += " and IDONVI=:donvi ";

                    param.Add(new OracleParameter("donvi", giamsat.IDONVI));
                }
                
                sql += " and  (UPPER(CNOIDUNG) like '%' || upper(:param02) || '%' or UPPER(CTEN) like '%' || upper(:param02) || '%')";
                var noidung = new OracleParameter("param02", OracleDbType.NVarchar2);
                noidung.Value = giamsat.CNOIDUNG.Trim().ToUpper();
                param.Add(noidung);
                listObj = base.GetList<KN_DOANGIAMSAT>(sql, param);
            }
            catch (Exception ex)
            {
                // ghi log lỗi tại đây
                log.Log_Error(ex, "Tìm kế hoạch giám sát");
                throw;
            }
            return listObj;
        }
        public Boolean Update(KN_DOANGIAMSAT actionInput)
        {

            if (base.UpdateItem<KN_DOANGIAMSAT>(actionInput, "IDOAN"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_DOANGIAMSAT actionInput)
        {

            if (base.DeleteItem<KN_DOANGIAMSAT>(actionInput, "IDOAN"))
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
