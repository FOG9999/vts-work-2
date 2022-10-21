using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Entities.Models;
using System.Web.Configuration;
using Oracle.ManagedDataAccess.Client;
using Utilities;
namespace DataAccess
{
    public class OracleDataAccess : BaseRepository
    {
        Log log = new Log();
        public OracleDataAccess()
        {
            try
            {
                if (Conn == null)
                {
                    String conStr = WebConfigurationManager.ConnectionStrings["Context"].ToString();
                    this.Conn = new OracleConnection(conStr);
                }
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "OracleDataAccess", 1);
                throw;
            }

        }
        public OracleDataAccess(OracleConnection conn)
        {
            if (conn != null)
                Conn = conn;
            else
            {
                var context = new Context();
                Conn = (OracleConnection)context.Database.Connection;
            }
        }
        public bool ExecuteSqlNoQuery(CommandType type, string mySql, List<OracleParameter> commandParameters)
        {
            bool resule = true;
            try
            {
                using (var cmd = Conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = mySql;
                    cmd.CommandType = type;
                    cmd.CommandTimeout = 50;
                    if (commandParameters != null)
                    {
                        for (int i = 0; i < commandParameters.Count(); i++)
                        {
                            cmd.Parameters.Add(commandParameters[i]);
                        }
                    }
                    if (Conn != null && Conn.State == ConnectionState.Closed)
                    {
                        this.intConn();
                    }
                    var temp = cmd.ExecuteNonQuery();
                    if (temp > 0)
                    {
                        resule = true;
                    }

                    //resule = false;
                }
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExecuteSqlNoQuery", 1);
                resule = false;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return resule;
        }
        public bool ExecuteSqlTextNoQuery(string mySql, List<OracleParameter> commandParameters)
        {
            bool resule = true;
            try
            {
                if (Conn != null && Conn.State == ConnectionState.Closed)
                {
                    this.intConn();
                }
                using (var cmd = Conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = mySql;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 50;
                    if (commandParameters != null)
                    {
                        for (int i = 0; i < commandParameters.Count(); i++)
                        {
                            cmd.Parameters.Add(commandParameters[i]);
                        }
                    }

                    if (Conn != null && Conn.State == ConnectionState.Closed)
                    {
                        this.intConn();
                    }
                    var temp = cmd.ExecuteNonQuery();
                    if (temp > 0)
                    {
                        resule = true;
                    }

                    //resule = false;
                }
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExecuteSqlTextNoQuery", 1);
                resule = false;
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return resule;
        }
        public DataTable ExecuteSqlDataTable(CommandType type, string mySql, object commandParameters)
        {
            DataTable dt = new DataTable();
            try
            {
                if (Conn != null && Conn.State == ConnectionState.Closed) // Em phải để ở đây, phải mở kết nối trước rồi mới
                // khởi tạo cmd sau nếu không nó sẽ không thực thi cmd được;
                {
                    this.intConn();
                }
                using (var cmd = Conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = mySql;
                    cmd.CommandType = type;
                    cmd.CommandTimeout = 50;
                    if (commandParameters != null)
                    {
                        if (commandParameters.GetType() == typeof(OracleParameter[]))
                        {
                            cmd.Parameters.AddRange((OracleParameter[])commandParameters);
                        }
                        else if (commandParameters.GetType() == typeof(List<OracleParameter>))
                        {
                            foreach (var item in (List<OracleParameter>)commandParameters)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        else
                        {
                            return null;

                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExecuteSqlDataTable", 1);
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
                commandParameters = null;
            }
            return dt;
        }
        public DataTable ExecuteSqlTextDataTable(string mySql, object commandParameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                if (Conn != null && Conn.State == ConnectionState.Closed)
                {
                    this.intConn();
                }
                using (var cmd = this.Conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = mySql;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 50;
                    if (commandParameters != null)
                    {

                        if (commandParameters.GetType() == typeof(OracleParameter[]))
                        {
                            cmd.Parameters.AddRange((OracleParameter[])commandParameters);
                        }
                        else if (commandParameters.GetType() == typeof(List<OracleParameter>))
                        {
                            foreach (var item in (List<OracleParameter>)commandParameters)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        else
                        {
                            return null;

                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExecuteSqlTextDataTable", 1);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
                commandParameters = null;
            }
            return dt;
        }
        public bool IsExist(string tenbang, string tencot, string giatri)
        {
            bool isExist = false;
            try
            {
                string strQuery = @"SELECT COUNT(*) TOTAL FROM ";
                if (Conn != null && Conn.State == ConnectionState.Closed)
                {
                    this.intConn();
                }
                if (!string.IsNullOrEmpty(tenbang))
                {
                    strQuery += tenbang;
                }
                if (!string.IsNullOrEmpty(tencot) && !string.IsNullOrEmpty(giatri))
                {
                    strQuery += " WHERE " + tencot + " = :p_giatri";
                }
                List<OracleParameter> lstParam = new List<OracleParameter>();
                lstParam.Add(new OracleParameter("p_giatri", giatri));
                DataTable dTableValue = ExecuteSqlTextDataTable(strQuery, lstParam);
                Int64 exist = Int64.Parse(dTableValue.Rows[0]["TOTAL"].ToString());
                if (exist > 0)
                {
                    isExist = true;
                }
            }
            catch (Exception ex)
            {
                
                isExist = false;
                log.Log_Data_access(ex, null, "IsExist", 1);
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return isExist;
        }
        public DataSet ExecuteSqlDataSet(CommandType type, string mySql, List<OracleParameter> commandParameters)
        {
            var ds = new DataSet();
            try
            {
                if (Conn != null && Conn.State == ConnectionState.Closed)
                {
                    this.intConn();
                }
                using (var cmd = Conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = mySql;
                    cmd.CommandType = type;
                    cmd.CommandTimeout = 50;
                    if (commandParameters != null)
                    {
                        for (int i = 0; i < commandParameters.Count(); i++)
                        {
                            cmd.Parameters.Add(commandParameters[i]);
                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (true)
                        {
                            var dt = new DataTable();
                            dt.Load(reader);

                            if (dt.Columns.Count == 0)
                                break;

                            ds.Tables.Add(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExecuteSqlDataSet", 1);
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return ds;
        }
        public DataTable ExecuteSqlTextDataTableWithTrans(OracleTransaction transObj, string mySql, object commandParameters)
        {

            DataTable dt = new DataTable();

            try
            {

                if (transObj != null)
                {
                    if (transObj.Connection != null && transObj.Connection.State == ConnectionState.Closed)
                    {
                        transObj.Connection.Open();
                    }
                    if (Conn == null)
                    {

                        this.intConn();

                    }
                    using (var cmd = transObj.Connection.CreateCommand())
                    {

                        cmd.BindByName = true;

                        cmd.Transaction = transObj;

                        cmd.CommandText = mySql;

                        cmd.CommandType = CommandType.Text;

                        cmd.CommandTimeout = 50;

                        if (commandParameters != null)
                        {



                            if (commandParameters.GetType() == typeof(OracleParameter[]))
                            {

                                cmd.Parameters.AddRange((OracleParameter[])commandParameters);

                            }

                            else if (commandParameters.GetType() == typeof(List<OracleParameter>))
                            {

                                foreach (var item in (List<OracleParameter>)commandParameters)
                                {

                                    cmd.Parameters.Add(item);

                                }

                            }

                            else
                            {

                                return null;



                            }

                        }

                        if (transObj.Connection != null && transObj.Connection.State == ConnectionState.Closed) { transObj.Connection.Open(); }

                        using (var reader = cmd.ExecuteReader())
                        {

                            dt.Load(reader);

                        }

                    }



                }

            }

            catch (Exception ex)
            {

                log.Log_Data_access(ex, null, "ExecuteSqlTextDataTableWithTrans", 1);
                throw;

            }



            return dt;

        }
        public bool ExecuteSqlTextNoQueryWithTrans(OracleTransaction transObj, string mySql, List<OracleParameter> commandParameters)
        {

            bool resule = true;

            try
            {

                if (Conn != null && Conn.State == ConnectionState.Closed)
                {

                    this.intConn();

                }



                //connection.BeginTransaction(IsolationLevel.ReadCommitted);



                if (transObj != null)
                {

                    using (var cmd = transObj.Connection.CreateCommand())
                    {

                        cmd.Transaction = transObj;

                        cmd.BindByName = true;

                        cmd.CommandText = mySql;

                        cmd.CommandType = CommandType.Text;

                        cmd.CommandTimeout = 50;

                        if (commandParameters != null)
                        {

                            for (int i = 0; i < commandParameters.Count(); i++)
                            {

                                cmd.Parameters.Add(commandParameters[i]);

                            }

                        }

                        if (transObj.Connection != null && transObj.Connection.State == ConnectionState.Closed) { transObj.Connection.Open(); }

                        var temp = cmd.ExecuteNonQuery();

                        if (temp > 0)
                        {

                            resule = true;

                        }

                        else { resule = false; }



                        //resule = false;

                    }



                }

                else { resule = false; }



            }

            catch (Exception ex)
            {

                resule = false;
                log.Log_Data_access(ex, null, "ExecuteSqlTextNoQueryWithTrans", 1);
                throw;

            }



            return resule;

        }

        public bool ExecuteSqlTextNoQueryWithTrans(string mySql, List<OracleParameter> commandParameters)
        {
            bool resule = true;
            OracleTransaction transObj = null;
            try
            {
                if (Conn != null && Conn.State == ConnectionState.Closed)
                {
                    this.intConn();
                }
                transObj = Conn.BeginTransaction(IsolationLevel.ReadCommitted);
                if (transObj != null)
                {
                    using (var cmd = transObj.Connection.CreateCommand())
                    {
                        cmd.Transaction = transObj;
                        cmd.BindByName = true;
                        cmd.CommandText = mySql;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 50;
                        if (commandParameters != null)
                        {
                            for (int i = 0; i < commandParameters.Count(); i++)
                            {
                                cmd.Parameters.Add(commandParameters[i]);
                            }
                        }
                        if (transObj.Connection != null && transObj.Connection.State == ConnectionState.Closed) { transObj.Connection.Open(); }
                        var temp = cmd.ExecuteNonQuery();
                        if (temp > 0)
                        {
                            resule = true;
                        }
                        else { resule = false; }
                        cmd.Transaction.Commit();
                    }
                }
                else { resule = false; }
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExecuteSqlTextNoQueryWithTrans", 1);
                if (transObj != null) transObj.Rollback();
                return false;
            }
            finally
            {
                if (transObj != null) transObj.Dispose();
            }
            return resule;
        }
        public DataTable ExecuteSqlProcedureDataTable(string proName, object commandParameters)
        {
            DataTable dt = new DataTable();
            try
            {
                if (Conn != null && Conn.State == ConnectionState.Closed)
                {
                    this.intConn();
                }
                using (var cmd = this.Conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = proName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 50;
                    cmd.Parameters.Add(new OracleParameter("res", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;
                    if (commandParameters != null)
                    {

                        if (commandParameters.GetType() == typeof(OracleParameter[]))
                        {
                            cmd.Parameters.AddRange((OracleParameter[])commandParameters);
                        }
                        else if (commandParameters.GetType() == typeof(List<OracleParameter>))
                        {
                            foreach (var item in (List<OracleParameter>)commandParameters)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        else
                        {
                            return null;

                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);

                    }
                }
            }
            catch (Exception ex)
            {
                log.Log_Data_access(ex, null, "ExecuteSqlTextNoQueryWithTrans", 1);
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
                commandParameters = null;
            }
            return dt;
        }
        public bool ExecuteNoQueryProcedure(string proName, object commandParameters)
        {
            bool result = true;
            try
            {
                if (Conn != null && Conn.State == ConnectionState.Closed)
                {
                    this.intConn();
                }
                using (var cmd = Conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = proName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 50;
                   // cmd.Parameters.Add(new OracleParameter("res", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;
                    if (commandParameters != null)
                    {

                        if (commandParameters.GetType() == typeof(OracleParameter[]))
                        {
                            cmd.Parameters.AddRange((OracleParameter[])commandParameters);
                        }
                        else if (commandParameters.GetType() == typeof(List<OracleParameter>))
                        {
                            foreach (var item in (List<OracleParameter>)commandParameters)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                       
                    }
                    var temp = cmd.ExecuteNonQuery();
                    if (temp > 0)
                    {
                        result = true;
                    }

                    //resule = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return result;
        }  //Thực thi lệnh insert, update, delete
    }
}
