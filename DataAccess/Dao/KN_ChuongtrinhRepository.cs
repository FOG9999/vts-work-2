using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
namespace DataAccess.Dao
{
    
    public interface IUKN_ChuongtrinhRepository
    {
        List<KN_CHUONGTRINH> GetAll();
        List<KN_CHUONGTRINH> GetAll(Dictionary<string, object> dictionary);
        KN_CHUONGTRINH AddNew(KN_CHUONGTRINH kn_chuongtrinhInput);
        Boolean Update(KN_CHUONGTRINH kn_chuongtrinhInput);
        Boolean Delete(KN_CHUONGTRINH kn_chuongtrinhInput);
    }
    public class KN_ChuongtrinhRepository : BaseRepository, IUKN_ChuongtrinhRepository
    {
        Log log = new Log();
        public List<KN_CHUONGTRINH> GetAll()
        {
            List<KN_CHUONGTRINH> resule = new List<KN_CHUONGTRINH>();
            resule = base.GetAll<KN_CHUONGTRINH>();
            return resule;
        }
        public List<KN_CHUONGTRINH> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_CHUONGTRINH> resule = new List<KN_CHUONGTRINH>();
            resule = base.GetAll<KN_CHUONGTRINH>(dictionary);

            return resule;
        }

        public KN_CHUONGTRINH GetByID(int ID)
        {
            return base.GetItem<KN_CHUONGTRINH>("ICHUONGTRINH", ID);
        }

        public KN_CHUONGTRINH GetByUseName(String kn_chuongtrinhInput)
        {
            return base.GetItem<KN_CHUONGTRINH>("ICHUONGTRINH", kn_chuongtrinhInput);
        }
        public List<KN_CHUONGTRINH> Tracuu(KN_CHUONGTRINH input)
        {
            List<KN_CHUONGTRINH> listObj = new List<KN_CHUONGTRINH>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from KN_CHUONGTRINH where IKYHOP >= 0 "; // input.ITINHTRANGXULY + "";
                if (input.CNOIDUNG != "")
                {
                    sql += " and  (UPPER(CNOIDUNG) like '%' || upper(:param02) || '%' or UPPER(CKEHOACH) like '%' || upper(:param02) || '%')";
                    //OracleParameter noidung = new OracleParameter;
                    var noidung = new OracleParameter("param02", OracleDbType.NVarchar2);
                    noidung.Value = input.CNOIDUNG.Trim().ToUpper();
                    param.Add(noidung);
                }
                if (input.ITRUOCKYHOP != null)
                {
                    sql += " and ITRUOCKYHOP = " + ":truockyhop";
                    param.Add(new OracleParameter("truockyhop", input.ITRUOCKYHOP));
                }
                if (input.IKYHOP != null && input.IKYHOP != 0)
                {
                    sql += " and IKYHOP = " + ":kyhop";
                    param.Add(new OracleParameter("kyhop", input.IKYHOP));
                }
                if (input.IDONVI != null && input.IDONVI != 0)
                {
                    sql += " and IDONVI = " + ":pr3";
                    param.Add(new OracleParameter("pr3", input.IDONVI));
                }
                if (input.DBATDAU != null)
                {
                    sql += " and DBATDAU >= " + ":pr4";
                    param.Add(new OracleParameter("pr4", input.DBATDAU));
                }
                if (input.DKETTHUC != null)
                {
                    sql += " and DKETTHUC <= " + ":pr5";
                    param.Add(new OracleParameter("pr5", input.DKETTHUC));
                }
                sql += " order by DBATDAU desc";
                listObj = base.GetList<KN_CHUONGTRINH>(sql, param);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm chương trình tiếp xúc cử tri");
                throw;
            }
            return listObj;

        }
        public KN_CHUONGTRINH AddNew(KN_CHUONGTRINH Input)
        {
            decimal IUse = 0;
            IUse = base.GetNextValSeq("KN_CHUONGTRINH_SEQ");
            if (IUse != 0)
            {
                Input.ICHUONGTRINH = IUse;
                if (base.InsertItem<KN_CHUONGTRINH>(Input))
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
        public Boolean Update(KN_CHUONGTRINH kn_chuongtrinhInput)
        {

            if (base.UpdateItem<KN_CHUONGTRINH>(kn_chuongtrinhInput, "ICHUONGTRINH"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_CHUONGTRINH kn_chuongtrinhInput)
        {

            if (base.DeleteItem<KN_CHUONGTRINH>(kn_chuongtrinhInput, "ICHUONGTRINH"))
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
