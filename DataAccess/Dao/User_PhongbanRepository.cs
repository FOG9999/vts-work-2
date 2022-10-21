using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{  
    public interface IUUser_PhongbanRepository
    {
        List<USER_PHONGBAN> GetAll();
        List<USER_PHONGBAN> GetAll(Dictionary<string, object> dictionary);
        USER_PHONGBAN GetByID(int ID);     
        USER_PHONGBAN AddNew(USER_PHONGBAN Input);
        List<USER_PHONGBAN> GetToDaiBieuList();
        List<USER_PHONGBAN> GetPhongBanTreeList();
        Boolean Update(USER_PHONGBAN Input);
        Boolean Delete(USER_PHONGBAN Input);
    }
    public class User_PhongbanRepository : BaseRepository, IUUser_PhongbanRepository
    {
        public List<USER_PHONGBAN> GetAll()
        {
            List<USER_PHONGBAN> resule = new List<USER_PHONGBAN>();
            resule = base.GetAll<USER_PHONGBAN>();
            return resule;
        }
        public List<USER_PHONGBAN> GetList(String sql)
        {
            List<USER_PHONGBAN> resule = new List<USER_PHONGBAN>();
            resule = base.GetList<USER_PHONGBAN>(sql);
            return resule;
        }

        public List<USER_PHONGBAN> GetAll(Dictionary<string, object> dictionary)
        {
            List<USER_PHONGBAN> resule = new List<USER_PHONGBAN>();
            resule = base.GetAll<USER_PHONGBAN>(dictionary);
            return resule;
        }
        public USER_PHONGBAN GetByID(int ID)
        {
            return base.GetItem<USER_PHONGBAN>("IPHONGBAN", ID);
        }
        public List<USER_PHONGBAN> GetToDaiBieuList()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT pb.* FROM USER_PHONGBAN pb INNER JOIN QUOCHOI_COQUAN dv ON pb.IDONVI = dv.ICOQUAN ");
            sql.Append("WHERE dv.ICOQUAN=137 AND dv.IDELETE = 0 AND dv.IHIENTHI = 1 ");
            sql.Append("AND pb.IDELETE = 0 AND pb.IHIENTHI = 1 AND pb.IPARENT <> 0 ");

            if (Conn == null || OracleAccess == null)
            {
                intConn();
            }
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql.ToString());
            if (dt == null || dt.Rows.Count == 0)
                return new List<USER_PHONGBAN>();
            var lstObj = ObjectHelper.ToListData<USER_PHONGBAN>(dt);
            dt.Dispose();
            return lstObj;
        }

        public List<USER_PHONGBAN> GetPhongBanTreeList()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM USER_PHONGBAN WHERE IDELETE = 0 AND IHIENTHI = 1 ");
            sql.AppendLine("START WITH IPHONGBAN IN (SELECT pb1.IPHONGBAN FROM USER_PHONGBAN pb1 INNER JOIN QUOCHOI_COQUAN dv ON pb1.IDONVI = dv.ICOQUAN ");
            sql.AppendLine("    WHERE dv.IDELETE = 0 AND dv.IHIENTHI = 1 AND (pb1.IPARENT =0 OR pb1.IPARENT IS NULL) AND pb1.IDELETE = 0 AND pb1.IHIENTHI = 1) ");
            sql.AppendLine("CONNECT BY PRIOR IPHONGBAN=IPARENT ");

            if (Conn == null || OracleAccess == null)
            {
                intConn();
            }
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql.ToString());
            if (dt == null || dt.Rows.Count == 0)
                return new List<USER_PHONGBAN>();
            var lstObj = ObjectHelper.ToListData<USER_PHONGBAN>(dt);
            dt.Dispose();
            return lstObj;
        }

        public USER_PHONGBAN AddNew(USER_PHONGBAN Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("USER_PHONGBAN_SEQ");
            if (ID != 0)
            {
                Input.IPHONGBAN = ID;
                if (base.InsertItem<USER_PHONGBAN>(Input))
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
        public Boolean Update(USER_PHONGBAN Input)
        {

            if (base.UpdateItem<USER_PHONGBAN>(Input, "IPHONGBAN"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(USER_PHONGBAN actionInput)
        {

            if (base.DeleteItem<USER_PHONGBAN>(actionInput, "IPHONGBAN"))
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
