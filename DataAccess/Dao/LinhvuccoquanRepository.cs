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
    public interface IULinhvuccoquanRepository
    {
        LINHVUC_COQUAN GetByID(int ID);
        List<LINHVUC_COQUAN> GetAll();
        List<LINHVUC_COQUAN> GetAll(Dictionary<string, object> dictionary);
        LINHVUC_COQUAN AddNew(LINHVUC_COQUAN UserInput);
        Boolean Update(LINHVUC_COQUAN UserInput);
        Boolean Delete(LINHVUC_COQUAN UserInput);
        List<LINHVUC_COQUAN> GetList(String sql);

    }
    public class LinhvuccoquanRepository : BaseRepository, IULinhvuccoquanRepository
    {
        public List<LINHVUC_COQUAN> GetAll()
        {
            List<LINHVUC_COQUAN> resule = new List<LINHVUC_COQUAN>();
            resule = base.GetAll<LINHVUC_COQUAN>();

            return resule;

        }
        public List<LINHVUC_COQUAN> GetAll(Dictionary<string, object> dictionary)
        {
            List<LINHVUC_COQUAN> resule = new List<LINHVUC_COQUAN>();
            resule = base.GetAll<LINHVUC_COQUAN>(dictionary);

            return resule;
        }
        public List<LINHVUC_COQUAN> GetList(String sql)
        {
            List<LINHVUC_COQUAN> resule = new List<LINHVUC_COQUAN>();
            resule = base.GetList<LINHVUC_COQUAN>(sql);
            return resule;
        }
        public LINHVUC_COQUAN GetByID(int ID)
        {


            return base.GetItem<LINHVUC_COQUAN>("ILINHVUC", ID);
        }

        public LINHVUC_COQUAN AddNew(LINHVUC_COQUAN Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("LINHVUC_COQUAN_SEQ");
            if (ID != 0)
            {
                Input.ILINHVUC = ID;
                if (base.InsertItem<LINHVUC_COQUAN>(Input))
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
        public Boolean Update(LINHVUC_COQUAN actionInput)
        {

            if (base.UpdateItem<LINHVUC_COQUAN>(actionInput, "ILINHVUC"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(LINHVUC_COQUAN actionInput)
        {

            if (base.DeleteItem<LINHVUC_COQUAN>(actionInput, "ILINHVUC"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /* Lấy danh sách lĩnh vực cơ quan sắp xếp theo cha-con
         */
        public List<LINHVUC_COQUAN> GetAllSorted()
        {
            List<LINHVUC_COQUAN> resule = new List<LINHVUC_COQUAN>();
            try
            {
                string sql = "SELECT * FROM LINHVUC_COQUAN WHERE IDELETE = 0 AND IHIENTHI = 1 " +
                        "start WITH IPARENT = 0 " +
                        "CONNECT by PRIOR ILINHVUC = IPARENT";
                resule = base.GetList<LINHVUC_COQUAN>(sql);
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
            return resule;

        }

        public List<LINHVUC_COQUAN> GetAllChild(int id)
        {
            List<LINHVUC_COQUAN> resule = new List<LINHVUC_COQUAN>();
            try
            {
                var param = new List<OracleParameter>();
                string sql = "SELECT * FROM LINHVUC_COQUAN " +
                             "start WITH IPARENT = " + " :param01";
                param.Add(new OracleParameter("param01", id));
                sql += " CONNECT by PRIOR ILINHVUC = IPARENT";
                resule = base.GetList<LINHVUC_COQUAN>(sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

            return resule;

        }
    }
}
