using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Configuration;
using Oracle.ManagedDataAccess.Client;
using Utilities;

namespace DataAccess
{
    public class BaseRepository
    {
        public OracleConnection Conn;
        public OracleTransaction Trans;
        public OracleDataAccess OracleAccess;
        Log log = new Log();
        public BaseRepository()
        {


        }
        protected Boolean intConn()
        {
            try
            {
                String conStr = WebConfigurationManager.ConnectionStrings["Context"].ToString();

                Conn = new OracleConnection(conStr);
                Conn.Open();
                OracleAccess = new OracleDataAccess(this.Conn);

                return true;
            }
            catch (Exception ex)
            {
                return false;
                log.Log_Data_access(ex, null, "intConn", 1);
                throw;
            }
        }
        protected void CommitChange()
        {
            if (this.Conn == null || this.Conn.State != ConnectionState.Open)
            {
                return;
            }
            if (this.Trans != null)
            {
                Trans.Commit();
                this.Trans.Dispose();
            }
            this.Conn.Close();
            this.Conn.Dispose();
        }
        protected void RollbackChange()
        {
            if (this.Conn == null || this.Conn.State != ConnectionState.Open)
            {
                return;
            }
            if (this.Trans != null)
            {
                Trans.Rollback();
                this.Trans.Dispose();
            }
            this.Conn.Close();
            this.Conn.Dispose();
        }
        protected void Close()
        {
            if (this.Conn == null || this.Conn.State != ConnectionState.Open)
            {
                return;
            }
            this.Conn.Close();
            this.Conn.Dispose();
        }
        protected bool InsertItem<T>(T t) where T : class,  new()
        {
            List<OracleParameter> param;
            var sqlInsert = t.BuildSqlInsert(out param);
            if (Conn == null)
            {
                this.intConn();
            }
            bool exe = OracleAccess.ExecuteSqlTextNoQuery(sqlInsert, param);
            Conn.Close();
            Conn.Dispose();
            return exe;
        }
        protected bool UpdateItem<T>(T t, string key) where T : class,  new()
        {
            List<OracleParameter> param;
            var sqlUpdate = t.BuildSqlUpdate(out param, key);
            if (Conn == null)
            {
                this.intConn();
            }
            bool exe = OracleAccess.ExecuteSqlTextNoQuery(sqlUpdate, param);
            Conn.Close();
            Conn.Dispose();
            return exe;
        }
        protected bool DeleteItem<T>(T t, string key) where T : class,  new()
        {
            List<OracleParameter> param;
            var sqlDelete = t.BuildSqlDelete(out param, key);
            if (Conn == null)
            {
                this.intConn();
            }
            bool exe = OracleAccess.ExecuteSqlTextNoQuery(sqlDelete, param);
            Conn.Close();
            Conn.Dispose();
            return exe;
        }
        protected bool ExcuteSQL(string sql)
        {
            List<OracleParameter> param = null;
            if (Conn == null)
            {
                this.intConn();
            }
            bool exe = OracleAccess.ExecuteSqlTextNoQuery(sql, param);
            Conn.Close();
            Conn.Dispose();
            return exe;
        }
        protected T GetItem<T>(string key, object value) where T : class, new()
        {
            var t = new T();
            var tableName = t.GetType().Name;

            var checkHasProp = t.GetType().GetProperty(key) != null;
            if (!checkHasProp)
                return null;

            var sql = "select * from " + tableName + " where " + key + " = :p_" + key;
            var param = new List<OracleParameter>()
            {
                new OracleParameter("p_"+key, value)
            };
            if (Conn == null)
            {
                this.intConn();
            }
            try
            {
                var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
                if (dt == null || dt.Rows.Count == 0)
                    return null;
                var lstObj = ObjectHelper.ToListData<T>(dt);
                Conn.Close();
                Conn.Dispose();
                if (lstObj == null || lstObj.Count == 0)
                    return null;
                return lstObj[0];
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExcuteSQLWithTrans", 1);
                return null;
            }


        }
        protected decimal GetNextValSeq(string seqName)
        {
            var sql = "Select " + seqName + ".Nextval from Dual";
            if (Conn == null)
            {

                this.intConn();

            }
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql, null);
            if (dt == null || dt.Rows.Count == 0)
                return 0;
            var id = Convert.ToDecimal(dt.Rows[0][0]);
            Conn.Close();
            Conn.Dispose();
            return id;
        }
        protected decimal GetTotal(string table)
        {
            var sql = "Select count(*) from " + table;
            if (Conn == null)
            {

                this.intConn();

            }
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql, null);
            if (dt == null || dt.Rows.Count == 0)
                return 0;
            var id = Convert.ToDecimal(dt.Rows[0][0]);
            Conn.Close();
            Conn.Dispose();
            return id;
        }
        public List<T> GetAll<T>() where T : class, new()
        {
            return GetAll<T>(null);
        }
        public List<T> GetAll<T>(Dictionary<string, object> condition) where T : class, new()
        {
            var sql = "";
            try
            {
                var t = new T();
                var tableName = t.GetType().Name;
                sql += "select * from " + tableName + " where 1 = 1";
                var param = new List<OracleParameter>();
                if (condition != null && condition.Count > 0)
                {
                    foreach (var item in condition)
                    {
                        var key = item.Key;
                        var value = item.Value;
                        var prop = key.Trim().ToUpper();
                        if (prop.Contains("LOWER"))
                        {
                            prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                        }
                        if (prop.Contains("UPPER"))
                        {
                            prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                        }
                        var checkHasProp = t.GetType().GetProperty(prop) != null;
                        if (!checkHasProp)
                            return null;
                        sql += " and " + key + " = :p_" + prop;
                        param.Add(new OracleParameter("p_" + prop, value));
                    }
                }
                if (Conn == null)
                {
                    this.intConn();
                }
                var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
                var lstObj = ObjectHelper.ToListData<T>(dt);
                Conn.Close();
                Conn.Dispose();
                return lstObj;
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, sql, 1);
                return null;
            }
        }
        public bool DeleteAll<T>(Dictionary<string, object> condition) where T : class, new()
        {
            try
            {
                bool kq = false;
                var t = new T();
                var tableName = t.GetType().Name;
                var sql = "delete " + tableName + " where 1 = 1";
                var param = new List<OracleParameter>();
                if (condition != null && condition.Count > 0)
                {
                    foreach (var item in condition)
                    {
                        var key = item.Key;
                        var value = item.Value;
                        var prop = key.Trim().ToUpper();
                        if (prop.Contains("LOWER"))
                        {
                            prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                        }
                        if (prop.Contains("UPPER"))
                        {
                            prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                        }
                        var checkHasProp = t.GetType().GetProperty(prop) != null;
                        if (!checkHasProp)
                            return false;
                        sql += " and " + key + " = :p_" + prop;
                        param.Add(new OracleParameter("p_" + prop, value));
                    }
                }
                if (Conn == null)
                {
                    this.intConn();
                }
                bool exe = OracleAccess.ExecuteSqlTextNoQuery(sql, param);
                Conn.Close();
                Conn.Dispose();
                return exe;
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExcuteSQLWithTrans", 1);
                return false;
            }
        }
        public List<T> GetList<T>(String Select, List<OracleParameter> lisparam) where T : class, new()
        {
            var lst = new List<T>();
            try
            {
                var t = new T();
                var tableName = t.GetType().Name;
                if (Conn == null)
                {
                    this.intConn();
                }
                var dt = OracleAccess.ExecuteSqlTextDataTable(Select, lisparam);
                lst = ObjectHelper.ToListData<T>(dt);
            }
            catch (Exception ex)
            {
                lst = null;
                log.Log_Data_access(ex, null, "GetList", 1);
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return lst;
        }
        public DataTable GetList_DataTable(String Select, List<OracleParameter> lisparam)
        {
            DataTable dt = new DataTable();
            try
            {

                if (Conn == null)
                {
                    this.intConn();
                }
                dt = OracleAccess.ExecuteSqlTextDataTable(Select, lisparam);

            }
            catch (Exception ex)
            {
                dt = null;
                log.Log_Data_access(ex, null, "GetList_DataTable", 1);
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return dt;
        }
        protected T GetItemBet<T>(string key, object value) where T : class, new()
        {
            var t = new T();
            var tableName = t.GetType().Name;

            var checkHasProp = t.GetType().GetProperty(key) != null;
            if (!checkHasProp)
                return null;

            var sql = "select * from " + tableName + " where " + key + " < :p_" + key;
            var param = new List<OracleParameter>()
            {
                new OracleParameter("p_"+key, value)
            };
            if (Conn == null)
            {
                this.intConn();
            }
            try
            {
                var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
                if (dt == null || dt.Rows.Count == 0)
                    return null;
                var lstObj = ObjectHelper.ToListData<T>(dt);
                Conn.Close();
                Conn.Dispose();
                if (lstObj == null || lstObj.Count == 0)
                    return null;
                return lstObj[0];
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExcuteSQLWithTrans", 1);
                return null;
            }

        }
        public T GetItem<T>(Dictionary<string, object> condition) where T : class, new()
        {

            var lstItem = GetAll<T>(condition);
            if (lstItem == null || lstItem.Count == 0)

                return null;

            return lstItem[0];

        }
        public List<T> GetList<T>(String where) where T : class, new()
        {
            try
            {
                var t = new T();
                var tableName = t.GetType().Name;
                var sql = "";
                var param = new List<OracleParameter>();
                if (!string.IsNullOrEmpty(where))
                {
                    sql = where;
                }
                if (Conn == null)
                {
                    this.intConn();
                }

                var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
                var lst = ObjectHelper.ToListData<T>(dt);
                Conn.Close();
                Conn.Dispose();
                return lst;
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExcuteSQLWithTrans", 1);
                return null;
            }
        }
        protected bool ExcuteSQLWithTrans(OracleTransaction transObj, string sql, List<OracleParameter> param) // có thể dùng co các câu lệnh delete hoặc update
        {

            bool resule = false;
            try
            {
                resule = OracleAccess.ExecuteSqlTextNoQueryWithTrans(transObj, sql, param);

            }
            catch (Exception ex)
            {

                resule = false;
                log.Log_Data_access(ex, null, "ExcuteSQLWithTrans", 1);
                throw;
                /// ghi log tại đây

            }
            return resule;

        }
        protected decimal GetNextValSeqWithTrans(OracleTransaction transObj, string seqName)
        {

            decimal id = 0;

            try
            {

                var sql = "Select " + seqName + ".Nextval from Dual";
                if (Conn == null)
                {
                    this.intConn();
                }

                var dt = OracleAccess.ExecuteSqlTextDataTableWithTrans(transObj, sql, null);

                if (dt == null || dt.Rows.Count == 0)

                    return 0;

                id = Convert.ToDecimal(dt.Rows[0][0]);

            }

            catch (Exception ex)
            {

                id = 0;
                log.Log_Data_access(ex, null, "GetNextValSeqWithTrans", 1);

            }

            return id;

        }
        protected bool InsertItemWithTrans<T>(OracleTransaction transObj, T t) where T : class,  new()
        {

            bool resule = false;

            try
            {

                List<OracleParameter> param;
                var sqlInsert = t.BuildSqlInsert(out param);
                resule = OracleAccess.ExecuteSqlTextNoQueryWithTrans(transObj, sqlInsert, param);

            }
            catch (Exception ex)
            {

                resule = false;
                log.Log_Data_access(ex, null, "GetNextValSeqWithTrans", 1);

            }



            return resule;

        }

        protected bool InsertItemWithTrans<T>(T t) where T : class, new()
        {

            bool resule = false;

            try
            {

                List<OracleParameter> param;
                var sqlInsert = t.BuildSqlInsert(out param);
                resule = OracleAccess.ExecuteSqlTextNoQueryWithTrans(sqlInsert, param);

            }
            catch (Exception ex)
            {

                resule = false;
                log.Log_Data_access(ex, null, "GetNextValSeqWithTrans", 1);

            }



            return resule;

        }
        protected bool ExcuteSQL(string sql, List<OracleParameter> param) // có thể dùng co các câu lệnh delete hoặc update
        {
            bool resule = false;

            try
            {
                if (Conn == null)
                {
                    this.intConn();
                }
                resule = OracleAccess.ExecuteSqlTextNoQuery(sql, param);
            }

            catch (Exception ex)
            {
                resule = false;
                throw;
                /// ghi log tại đây
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return resule;
        }
        public Boolean Backup_Demo()
        {
            bool result = true;
            try
            {
                if (Conn == null)
                {
                    this.intConn();
                }
                OracleCommand cmd = new OracleCommand("backup database KIENNGHI to disk='C:\\db_kiennghi.Bak'", Conn);
                cmd.ExecuteNonQuery();
            }

            catch (Exception)
            {
                result = false;
                throw;
                /// ghi log tại đây
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            //return resule;
            //OracleCommand cmd = new OracleCommand();
            //OledbCommand cmd = new OledbCommand("backup database databasename to disk ='C:\databasename.bak'", con);
            //con.open();
            //cmd.ExecutenonQuery();
            //con.close();
            return result;
        }
        protected decimal Phuc_GetTotal_(string sql, List<OracleParameter> param)
        {
            if (Conn == null)
            {

                this.intConn();

            }
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
            if (dt == null || dt.Rows.Count == 0)
                return 0;
            var id = Convert.ToDecimal(dt.Rows[0][0]);
            Conn.Close();
            Conn.Dispose();
            return id;
        }
        public List<T> GetListPageList<T>(String Select, List<OracleParameter> lisparam, int page, int pageSize) where T : class, new()
        {
            string sql = "SELECT * FROM ( SELECT k.*, ROWNUM r_ FROM (";
            var lst = new List<T>();
            try
            {
                var t = new T();
                var tableName = t.GetType().Name;
                if (Conn == null)
                {
                    this.intConn();
                }
                sql += Select;
                sql += " ) k WHERE ROWNUM < ((:pageNumber * :pageSize) + 1 )) WHERE r_>=	(((:pageNumber-1) * :pageSize) + 1);";
                lisparam.Add(new OracleParameter("pageNumber", page));
                lisparam.Add(new OracleParameter("pageSize", pageSize));
                var dt = OracleAccess.ExecuteSqlTextDataTable(Select, lisparam);
                lst = ObjectHelper.ToListData<T>(dt);
            }
            catch (Exception ex)
            {
                lst = null;
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return lst;
        }
        public List<T> GetAll_PageList<T>(Dictionary<string, object> condition, int page, int pageSize) where T : class, new()
        {
            var t = new T();
            var tableName = t.GetType().Name;
            var sql = "SELECT * FROM ( SELECT k.*, ROWNUM r_ FROM (";
            sql += " select * from " + tableName + " where 1 = 1";
            var param = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    var checkHasProp = t.GetType().GetProperty(prop) != null;
                    if (!checkHasProp)
                        return null;
                    sql += " and " + key + " = :p_" + prop;
                    param.Add(new OracleParameter("p_" + prop, value));
                }
            }
            if (Conn == null)
            {
                this.intConn();
            }
            sql += " ) k WHERE ROWNUM < " + page + " ) WHERE r_ >=" + pageSize + " ;";
            //param.Add(new OracleParameter("pageNumber", page));
            //param.Add(new OracleParameter("pageSize", pageSize));
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
            var lstObj = ObjectHelper.ToListData<T>(dt);
            Conn.Close();
            Conn.Dispose();
            return lstObj;
        }
        public List<T> GetListObjetReport<T>(String procedureName, List<OracleParameter> lisparam) where T : class, new()
        {
            var lst = new List<T>();
            try
            {
                var t = new T();
                var tableName = t.GetType().Name;
                if (Conn == null)
                {
                    this.intConn();
                }
                var dt = OracleAccess.ExecuteSqlProcedureDataTable(procedureName, lisparam);
                lst = ObjectHelper.ToListData<T>(dt);
            }
            catch (Exception ex)
            {
                lst = null;
                log.Log_Data_access(ex, null, "GetListObjetReport", 1);
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return lst;
        }
        public bool ExecuteProcedure(string procedureName, List<OracleParameter> lisparam)
        {
            bool resule = true;
            try
            {
                if (Conn == null)
                {
                    this.intConn();
                }
                resule = OracleAccess.ExecuteNoQueryProcedure(procedureName, lisparam);
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExecuteProcedure", 1);
                throw;
            }
            return resule;
        }
    }
}
