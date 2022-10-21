using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
namespace DataAccess.Dao
{
    
    public interface IUTiepda_DinhkyRepository
    {
        TIEPDAN_DINHKY GetByID(int ID);
        List<TIEPDAN_DINHKY> GetAll();
        List<TIEPDAN_DINHKY> GetAll(Dictionary<string, object> dictionary);
        TIEPDAN_DINHKY AddNew(TIEPDAN_DINHKY UserInput);
        Boolean Update(TIEPDAN_DINHKY UserInput);
        Boolean Delete(TIEPDAN_DINHKY UserInput);
    }
    public class Tiepdan_DinhkyRepository : BaseRepository, IUTiepda_DinhkyRepository
    {
        Log log = new Log();
        public List<TIEPDAN_DINHKY> GetAll()
        {
            List<TIEPDAN_DINHKY> resule = new List<TIEPDAN_DINHKY>();
            resule = base.GetAll<TIEPDAN_DINHKY>();
            return resule;
        }
        public List<TIEPDAN_DINHKY> GetList_KiemTraTrung(string dngay, string cdiadiem)
        {
            List<TIEPDAN_DINHKY> listObj = new List<TIEPDAN_DINHKY>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from TIEPDAN_DINHKY where DNGAYTIEP = :param01 ";
                var noidung = new OracleParameter("param01", OracleDbType.Date);
                noidung.Value = dngay;
                param.Add(noidung);
                sql += " AND  UPPER(CDIADIEM) = UPPER(:param02) ";
                var noidungkey = new OracleParameter("param02", OracleDbType.NVarchar2);
                noidungkey.Value = cdiadiem.Trim();
                param.Add(noidungkey);

                listObj = base.GetList<TIEPDAN_DINHKY>(sql, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Kiểm tra lịch tiếp định kỳ ");
                return null;
            }
            return listObj;
        }
        public List<TIEPDAN_DINHKY> GetList_SQLTimkiemten(string cten)
        {
         
            List<TIEPDAN_DINHKY> listObj = new List<TIEPDAN_DINHKY>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from TIEPDAN_DINHKY where UPPER(CDIADIEM) LIKE '%' || UPPER(:param01) || '%'";
                var noidungkey = new OracleParameter("param01", OracleDbType.NVarchar2);
                noidungkey.Value = cten.Trim();
                param.Add(noidungkey);

                listObj = base.GetList<TIEPDAN_DINHKY>(sql, param);
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Kiểm tra tên lịch tiếp định kỳ ");
                return null;
            }
            return listObj;
        }
        public List<TIEPDAN_DINHKY> GetAll(Dictionary<string, object> dictionary)
        {
            List<TIEPDAN_DINHKY> resule = new List<TIEPDAN_DINHKY>();
            resule = base.GetAll<TIEPDAN_DINHKY>();
            return resule;
        }
        public TIEPDAN_DINHKY GetByID(int ID)
        {
            return base.GetItem<TIEPDAN_DINHKY>("IDINHKY", ID);
        }
        public TIEPDAN_DINHKY AddNew(TIEPDAN_DINHKY Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("TIEPDAN_DINHKY_SEQ");
            if (ID != 0)
            {
                Input.IDINHKY = ID;
                if (base.InsertItem<TIEPDAN_DINHKY>(Input))
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
        public Boolean Update(TIEPDAN_DINHKY actionInput)
        {

            if (base.UpdateItem<TIEPDAN_DINHKY>(actionInput, "IDINHKY"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(TIEPDAN_DINHKY actionInput)
        {

            if (base.DeleteItem<TIEPDAN_DINHKY>(actionInput, "IDINHKY"))
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
