using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{

    public interface IULinhvucRepository
    {
        LINHVUC GetByID(int ID);
        List<LINHVUC> GetAll();
        List<LINHVUC> GetAll(Dictionary<string, object> dictionary);
        LINHVUC AddNew(LINHVUC Input);
        Boolean Update(LINHVUC Input);
        Boolean Delete(LINHVUC Input);
        List<LINHVUC> GetList(String sql);
    }
    public class LinhvucRepository : BaseRepository, IULinhvucRepository
    {
        public List<LINHVUC> GetAll()
        {
            List<LINHVUC> resule = new List<LINHVUC>();
            resule = base.GetAll<LINHVUC>();
            return resule;
        }
        public List<LINHVUC> GetAll(Dictionary<string, object> dictionary)
        {
            List<LINHVUC> resule = new List<LINHVUC>();
            resule = base.GetAll<LINHVUC>(dictionary);
            return resule;
        }
        public List<LINHVUC> GetList(String sql)
        {
            List<LINHVUC> resule = new List<LINHVUC>();
            resule = base.GetList<LINHVUC>(sql);
            return resule;
        }
        public List<LINHVUC> GetList_CheckMaLinhVuc_Update(string code, int id)
        {
            List<LINHVUC> listObj = new List<LINHVUC>();
            var param = new List<OracleParameter>();
            try
            {
                int idel = id;
                string sql = "SELECT * from LINHVUC where ILINHVUC != " + " :param01";
                param.Add(new OracleParameter("param01", idel));
                sql += " and  UPPER(CCODE) =:param02";
                var noidungkey = new OracleParameter("param02", OracleDbType.NVarchar2);
                noidungkey.Value = code.Trim();
                param.Add(noidungkey);
                //sql += " and  ICOQUAN !=:param03";
                //param.Add(new OracleParameter("param03", id));
                listObj = base.GetList<LINHVUC>(sql, param);


            }
            catch (Exception)
            {
                throw;
            }
            return listObj;
        }
        public List<LINHVUC> GetList_CheckMaLinhVuc_Insert(string code)
        {
            List<LINHVUC> listObj = new List<LINHVUC>();
            var param = new List<OracleParameter>();
            try
            {
                int idel = 0;
                string sql = "SELECT * from LINHVUC where IDELETE = " + " :param01";
                param.Add(new OracleParameter("param01", idel));

                sql += " and  UPPER(CCODE) =:param02";
                var noidungkey = new OracleParameter("param02", OracleDbType.NVarchar2);
                noidungkey.Value = code.Trim();
                param.Add(noidungkey);
                listObj = base.GetList<LINHVUC>(sql, param);
            }
            catch (Exception)
            {
                throw;
            }
            return listObj;
        }
        public LINHVUC GetByID(int ID)
        {
            return base.GetItem<LINHVUC>("ILINHVUC", ID);
        }    
        public LINHVUC AddNew(LINHVUC Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("LINHVUC_SEQ");
            if (ID != 0)
            {
                Input.ILINHVUC = ID;
                if (base.InsertItem<LINHVUC>(Input))
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
        public Boolean Update(LINHVUC actionInput)
        {

            if (base.UpdateItem<LINHVUC>(actionInput, "ILINHVUC"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(LINHVUC actionInput)
        {

            if (base.DeleteItem<LINHVUC>(actionInput, "ILINHVUC"))
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
