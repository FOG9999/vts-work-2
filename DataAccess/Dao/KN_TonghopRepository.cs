using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
namespace DataAccess.Dao
{
   
    public interface IUKN_TonghopRepository
    {
        KN_TONGHOP GetByID(int ID);
        List<KN_TONGHOP> GetAll();
        List<KN_TONGHOP> GetAll(Dictionary<string, object> dictionary);
        List<KN_TONGHOP> GetList(String sql);
        KN_TONGHOP AddNew(KN_TONGHOP UserInput);
        Boolean Update(KN_TONGHOP UserInput);
        Boolean Delete(KN_TONGHOP UserInput);
    }
    public class KN_TonghopRepository : BaseRepository, IUKN_TonghopRepository  
    {
        Log log = new Log();
        public List<KN_TONGHOP> GetAll()
        {
            List<KN_TONGHOP> resule = new List<KN_TONGHOP>();
            resule = base.GetAll<KN_TONGHOP>();
            return resule;
        }
        public List<KN_TONGHOP> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_TONGHOP> resule = new List<KN_TONGHOP>();
            resule = base.GetAll<KN_TONGHOP>(dictionary);
            return resule;
        }
        public List<KN_TONGHOP> GetList(String sql)
        {
            List<KN_TONGHOP> resule = new List<KN_TONGHOP>();
            resule = base.GetList<KN_TONGHOP>(sql);
            return resule;
        }
        public KN_TONGHOP GetByID(int ID)
        {
            return base.GetItem<KN_TONGHOP>("ITONGHOP", ID);
        }

        public KN_TONGHOP GetByUseName(String actionInput)
        {
            return base.GetItem<KN_TONGHOP>("ITONGHOP", actionInput);
        }
        public KN_TONGHOP AddNew(KN_TONGHOP actionInput)
        {

            decimal ID = 0;
            ID = base.GetNextValSeq("KN_TONGHOP_SEQ");
            if (ID != 0)
            {
                actionInput.ITONGHOP = ID;
                if (base.InsertItem<KN_TONGHOP>(actionInput))
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
        public Boolean Update(KN_TONGHOP actionInput)
        {

            if (base.UpdateItem<KN_TONGHOP>(actionInput, "ITONGHOP"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_TONGHOP actionInput)
        {

            if (base.DeleteItem<KN_TONGHOP>(actionInput, "ITONGHOP"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<KN_TONGHOP> Tracuu(KN_TONGHOP input)
        {
            List<KN_TONGHOP> listObj = new List<KN_TONGHOP>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from KN_TONGHOP where IKYHOP >= 0 "; // input.ITINHTRANGXULY + "";
                if (input.CNOIDUNG != "" && input.CNOIDUNG!=null)
                {
                    sql += " and  (UPPER(CNOIDUNG) like '%' || upper(:param02) || '%' or UPPER(CTUKHOA) like '%' || upper(:param02) || '%')";
                    //OracleParameter noidung = new OracleParameter;
                    var noidung = new OracleParameter("param02", OracleDbType.NVarchar2);
                    noidung.Value = input.CNOIDUNG.Trim().ToUpper();
                    param.Add(noidung);
                }
                if (input.ITINHTRANG != null && input.ITINHTRANG != -1)
                {
                    sql += " and ITINHTRANG = " + ":tinhtrang";
                    param.Add(new OracleParameter("tinhtrang", input.ITINHTRANG));
                }
                if (input.ITRUOCKYHOP != null && input.ITRUOCKYHOP != -1)
                {
                    sql += " and ITRUOCKYHOP = " + ":truockyhop";
                    param.Add(new OracleParameter("truockyhop", input.ITRUOCKYHOP));
                }
                if (input.IKYHOP != null && input.IKYHOP != 0)
                {
                    sql += " and IKYHOP = " + ":kyhop";
                    param.Add(new OracleParameter("kyhop", input.IKYHOP));
                }
                if (input.ITHAMQUYENDONVI != null && input.ITHAMQUYENDONVI != -1)
                {
                    sql += " and ITHAMQUYENDONVI = " + ":pr3";
                    param.Add(new OracleParameter("pr3", input.ITHAMQUYENDONVI));
                }
                if (input.IDONVITONGHOP != null && input.IDONVITONGHOP!=0)
                {
                    sql += " and IDONVITONGHOP = " + ":pr4";
                    param.Add(new OracleParameter("pr4", input.IDONVITONGHOP));
                }
                if (input.ILINHVUC != null && input.ILINHVUC != -1)
                {
                    sql += " and ILINHVUC = " + ":pr5";
                    param.Add(new OracleParameter("pr5", input.ILINHVUC));
                }
                sql += " order by ITONGHOP desc";
                listObj = base.GetList<KN_TONGHOP>(sql, param);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm tổng hợp kiến nghị");
                throw;
            }
            return listObj;

        }
    }
}
