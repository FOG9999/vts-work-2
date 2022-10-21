using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Oracle.ManagedDataAccess.Client;

namespace DataAccess
{
    public class InformationColumn
    {
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public InformationColumn() { }
    }
    class OracleDataProvider : AbstractDataProvider
    {        
        private string connectionString;
        public OracleDataProvider(string connectionStringName)
        {
            this.connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;            
        }
        #region Phần ExecuteNonQuery
        public override int ExecuteNonQuery(string spName, params object[] inputparameterValues)
        {
            return OracleHelper.ExecuteNonQuery(connectionString, spName, inputparameterValues);
        }

        public override int ExecuteNonQueryInsert<T>(T obj)
        {
            string DataTableName = typeof(T).Name;
            return ExecuteNonQueryInsert(obj, DataTableName);  
        }

        public override int ExecuteNonQueryInsert<T>(T obj, string DataTableName)
        {
            var fieldcolumn = typeof(T).GetProperties();
            string insqry = "INSERT INTO " + DataTableName + " (";
            for (int i = 0; i < fieldcolumn.Length; i++)
            {
                if (i == fieldcolumn.Length - 1)
                    insqry += fieldcolumn[i].Name + ")";
                else
                    insqry += fieldcolumn[i].Name + ",";
            }
            insqry += "VALUES (";
            for (int i = 0; i < fieldcolumn.Length; i++)
            {
                if (i == fieldcolumn.Length - 1)
                    insqry += ":" + fieldcolumn[i].Name + ")";
                else
                    insqry += ":" + fieldcolumn[i].Name + ",";
            }

            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.InsertCommand = new OracleCommand(insqry, conn);
            ad.InsertCommand.Parameters.Clear();
            for (int i = 0; i < fieldcolumn.Length; i++)
            {
                ad.InsertCommand.Parameters.Add(new OracleParameter(":" + fieldcolumn[i].Name, obj.GetType().GetProperty(fieldcolumn[i].Name).GetValue(obj, null)));
            }
            ad.InsertCommand.ExecuteNonQuery();
            try
            {
                trans.Commit();
                conn.Close();
                return 1;
            }
            catch
            {
                trans.Rollback();
                conn.Close();
                return 0;
            }
        }

        public override int ExecuteNonQueryUpdate<T>(T obj, params string[] fieldCompare)
        {
            string fieldColumnUpdate = string.Empty;
            var fieldcolumns = typeof(T).GetProperties();
            for (int i = 0; i < fieldcolumns.Length; i++)
            {
                if (i < fieldcolumns.Length - 1)
                    fieldColumnUpdate += fieldcolumns[i].Name + ",";
                else
                    fieldColumnUpdate += fieldcolumns[i].Name;
            }
            return ExecuteNonQueryUpdate(fieldColumnUpdate, obj, fieldCompare);
        }

        public override int ExecuteNonQueryUpdate<T>(string fieldColumnUpdate, T obj, params string[] fieldCompare)
        {
            string DataTableName = typeof(T).Name;
            return ExecuteNonQueryUpdate(DataTableName,fieldColumnUpdate, obj, fieldCompare);                 
        }        

        public override int ExecuteNonQueryUpdate<T>(string DataTableName, string fieldColumnUpdate, T obj, params string[] fieldCompare)
        {
            var fieldvalues = fieldColumnUpdate.Split(',');
            List<string> ColumnNameUpdate = new List<string>();
            List<string> PropertiesNameSet = new List<string>();
            List<string> ColumnNameCompare = new List<string>();
            List<string> PropertiesCompareSet = new List<string>();
            int fieldCount = fieldvalues[0].Split('=').Length;
            foreach (var values in fieldvalues)
            {
                if (fieldCount != values.Split('=').Length)
                {
                    throw new ArgumentException("Cấu trúc dữ liệu update không đồng nhất");
                }
                else
                {
                    var valuestr = values.Split('=');
                    fieldCount = valuestr.Length;
                    if (valuestr.Length > 1)
                    {
                        PropertiesNameSet.Add(valuestr[1].Trim());
                        ColumnNameUpdate.Add(valuestr[0].Trim());
                    }
                    else
                    {
                        ColumnNameUpdate.Add(valuestr[0].Trim());
                        PropertiesNameSet.Add(valuestr[0].Trim());
                    }
                }
            }
            fieldCount = fieldCompare[0].Split('=').Length;
            foreach (var values in fieldCompare)
            {
                if (fieldCount != values.Split('=').Length)
                {
                    throw new ArgumentException("Cấu trúc dữ liệu so sánh không đồng nhất");
                }
                else
                {
                    var valuestr = values.Split('=');
                    fieldCount = valuestr.Length;
                    if (valuestr.Length > 1)
                    {
                        ColumnNameCompare.Add(valuestr[0].Trim());
                        PropertiesCompareSet.Add(valuestr[1].Trim());
                    }
                    else
                    {
                        ColumnNameCompare.Add(valuestr[0].Trim());
                        PropertiesCompareSet.Add(valuestr[0].Trim());
                    }
                }
            }

            return UpdateList(DataTableName, ColumnNameUpdate, PropertiesNameSet, ColumnNameCompare, PropertiesCompareSet, obj, fieldCompare);
        }

        public int UpdateList<T>(string DataTableName, List<string> ColumnNameUpdate, List<string> PropertiesNameSet, List<string> ColumnNameCompare, List<string> PropertiesCompareSet,
            T obj, params string[] fieldCompare)
        {            
            string updatequery = "UPDATE " + DataTableName + " ";
            if (ColumnNameUpdate.Count > 1)
            {
                for (int i = 0; i < ColumnNameUpdate.Count; i++)
                {
                    if (i == 0)
                        updatequery += " SET " + ColumnNameUpdate[i] + " = :" + PropertiesNameSet[i]  + ",";
                    else if (i == ColumnNameUpdate.Count - 1)
                        updatequery += ColumnNameUpdate[i] + " = :" + PropertiesNameSet[i] ;
                    else
                        updatequery += ColumnNameUpdate[i] + " = :" + PropertiesNameSet[i]  + ",";
                }
            }
            else
            {
                updatequery += " SET " + ColumnNameUpdate[0] + " = :" + PropertiesNameSet[0]  ;
            }

            if (ColumnNameCompare.Count > 1)
            {
                for (int i = 0; i < ColumnNameCompare.Count; i++)
                {
                    if (i == 0)
                        updatequery += " WHERE " + ColumnNameCompare[i] + " = :" + PropertiesCompareSet[i] ;                   
                    else
                        updatequery += " AND " + ColumnNameCompare[i] + " = :" + PropertiesCompareSet[i] ;
                }
            }
            else
            {
                updatequery += " WHERE " + ColumnNameCompare[0] + " = :" + PropertiesCompareSet[0];
            }            
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            var ColumnResult = GetInformationColumn(conn, DataTableName);
            if (ColumnResult.Count == 0)
            {
                conn.Close();
                throw new ArgumentException("Table "+ DataTableName+" không có trong dữ liệu hoặc chưa được khai báo các cột!");
            }
            foreach (var column in ColumnNameUpdate)
            {
                if (ColumnResult.Where(t => t.ColumnName == column).FirstOrDefault() == null)
                {
                    conn.Close();
                    throw new ArgumentException("Các trường Update không xuất hiện trong DataTable !");                   
                }
            }
            OracleCommand cmd = new OracleCommand(updatequery, conn);
            OracleTransaction trans = conn.BeginTransaction();
            for (int i = 0; i < ColumnNameUpdate.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(":" + ColumnNameUpdate[i], obj.GetType().GetProperty(PropertiesNameSet[i]).GetValue(obj, null)));
            }
            for (int i = 0; i <ColumnNameCompare.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(":" + ColumnNameCompare[i], obj.GetType().GetProperty(PropertiesCompareSet[i]).GetValue(obj, null)));
            }
            cmd.ExecuteNonQuery();
            try
            {
                trans.Commit();
                conn.Close();
                return 1;
            }
            catch
            {
                trans.Rollback();
                conn.Close();
                return 0;
            }

        }

        public override int ExecuteNonQueryDelete<T>(params string[] fieldCompare)
        {
            string deletequery = "DELETE " + typeof(T).Name;
            List<string> ColumnNameCompare = new List<string>();
            List<object> ValueCompareSet = new List<object>();
            int fieldCount = fieldCompare[0].Split('=').Length;
            foreach (var values in fieldCompare)
            {
                if (fieldCount != values.Split('=').Length)
                {
                    throw new ArgumentException("Cấu trúc dữ liệu so sánh không đồng nhất");
                }
                else
                {
                    var valuestr = values.Split('=');
                    fieldCount = valuestr.Length;
                    if (valuestr.Length > 1)
                    {
                        ColumnNameCompare.Add(valuestr[0].Trim());
                        ValueCompareSet.Add(valuestr[1].Trim());
                    }
                    else
                    {
                        ColumnNameCompare.Add(valuestr[0].Trim());
                        ValueCompareSet.Add(valuestr[0].Trim());
                    }
                }
            }
            if (ColumnNameCompare.Count > 1)
            {
                for (int i = 0; i < ColumnNameCompare.Count; i++)
                {
                    if (i == 0)
                        deletequery += " WHERE " + ColumnNameCompare[i] + " = :" + ColumnNameCompare[i];
                    else
                        deletequery += " AND " + ColumnNameCompare[i] + " = :" + ColumnNameCompare[i];
                }
            }
            else
            {
                deletequery += " WHERE " + ColumnNameCompare[0] + " = :" + ColumnNameCompare[0];
            } 
            
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(deletequery, conn);
            OracleTransaction trans = conn.BeginTransaction();
            for (int i = 0; i < ColumnNameCompare.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(":" + ColumnNameCompare[i], ValueCompareSet[i]));
            }
            cmd.ExecuteNonQuery();
            try
            {
                trans.Commit();
                conn.Close();
                return 1;
            }
            catch
            {
                trans.Rollback();
                conn.Close();
                return 0;
            }
        }


        public override int ExecuteNonQueryDeleteAll<T>()
        {
            string TableName = typeof(T).Name;
            return ExecuteNonQueryDeleteAll<T>(TableName);
        }

        public override int ExecuteNonQueryDeleteAll<T>(string DataTableName)
        {

            string deletequery = "DELETE " + DataTableName;  
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(deletequery, conn);
            OracleTransaction trans = conn.BeginTransaction();
            
            cmd.ExecuteNonQuery();
            try
            {
                trans.Commit();
                conn.Close();
                return 1;
            }
            catch
            {
                trans.Rollback();
                conn.Close();
                return 0;
            }
        }
        
        public override int ExecuteNonQueryDelete<T>(string DataTableName, T obj, params string[] fieldCompare)
        {            
            string deletequery = "DELETE " + DataTableName + " ";            
            for (int i = 0; i < fieldCompare.Length; i++)
            {
                if(i == 0)
                    deletequery += " WHERE " + fieldCompare[i] + " = :" + fieldCompare[i];
                else
                    deletequery += " AND " + fieldCompare[i] + " = :" + fieldCompare[i];
            }
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(deletequery, conn);
            OracleTransaction trans = conn.BeginTransaction();
            for (int i = 0; i < fieldCompare.Length; i++)
            {
                cmd.Parameters.Add(new OracleParameter(":" + fieldCompare[i], obj.GetType().GetProperty(fieldCompare[i]).GetValue(obj, null)));
            }
            cmd.ExecuteNonQuery();
            try
            {
                trans.Commit();
                conn.Close();
                return 1;
            }
            catch
            {
                trans.Rollback();
                conn.Close();
                return 0;
            }
        }

        public override int ExecuteNonQueryOrtherDelete<T>(string connString, string DataTableName, T obj, params string[] fieldCompare)
        {
            string deletequery = "DELETE " + DataTableName + " ";
            for (int i = 0; i < fieldCompare.Length; i++)
            {
                if (i == 0)
                    deletequery += " WHERE " + fieldCompare[i] + " = :" + fieldCompare[i];
                else
                    deletequery += " AND " + fieldCompare[i] + " = :" + fieldCompare[i];
            }
            OracleConnection conn = new OracleConnection(connString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(deletequery, conn);
            OracleTransaction trans = conn.BeginTransaction();
            for (int i = 0; i < fieldCompare.Length; i++)
            {
                cmd.Parameters.Add(new OracleParameter(":" + fieldCompare[i], obj.GetType().GetProperty(fieldCompare[i]).GetValue(obj, null)));
            }
            cmd.ExecuteNonQuery();
            try
            {
                trans.Commit();
                conn.Close();
                return 1;
            }
            catch
            {
                trans.Rollback();
                conn.Close();
                return 0;
            }
        }


        public override object ExecuteNonQuery(out object outputparameterValue, string spName, params object[] inputparameterValues)
        {
            throw new NotImplementedException();
        }

        public override object ExecuteNonQuery(out List<object> ListoutputparameterValues, string spName, params object[] inputparameterValues)
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQueryUsingCommandText(string CommandText)
        {
            return OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, CommandText);
        }

        public override bool ExecuteNonQueryUsingCommandText(string CommandText, params object[] inputparameterValues)
        {
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(CommandText, conn);            
            List<string> paramaterinput = new List<string>();
            var splitquery = CommandText.Split(' ', '%', '(', ')');
            for (int i = 0; i < splitquery.Length; i++)
            {
                if (splitquery[i].StartsWith(":") && paramaterinput.Count == 0)
                {
                    paramaterinput.Add(splitquery[i]);
                }
                else if (splitquery[i].StartsWith(":") && paramaterinput.Count > 0 && paramaterinput.Where(t => t.ToUpper().Trim().Equals(splitquery[i].ToUpper().Trim())).FirstOrDefault() == null)
                {
                    paramaterinput.Add(splitquery[i]);
                }
            }
            if (inputparameterValues.Length != paramaterinput.Count)
            {
                throw new ArgumentException("Số biến truyền vào không đúng so với câu truy vấn");
            }
            for (int i = 0; i < paramaterinput.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(paramaterinput[i], inputparameterValues[i]));
            }
            cmd.BindByName = true;
            var rs = cmd.ExecuteNonQuery();
            try
            {                
                trans.Commit();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                conn.Close();
                return false;
            }            
           
        }

        public override bool ExecuteNonQueryUsingCommandTextOrtherConnect(string Connectstr, string CommandText, params object[] inputparameterValues)
        {
            OracleConnection conn = new OracleConnection(Connectstr);
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(CommandText, conn);
            List<string> paramaterinput = new List<string>();
            var splitquery = CommandText.Split(' ', '%', '(', ')');
            for (int i = 0; i < splitquery.Length; i++)
            {
                if (splitquery[i].StartsWith(":") && paramaterinput.Count == 0)
                {
                    paramaterinput.Add(splitquery[i]);
                }
                else if (splitquery[i].StartsWith(":") && paramaterinput.Count > 0 && paramaterinput.Where(t => t.ToUpper().Trim().Equals(splitquery[i].ToUpper().Trim())).FirstOrDefault() == null)
                {
                    paramaterinput.Add(splitquery[i]);
                }
            }
            if (inputparameterValues.Length != paramaterinput.Count)
            {
                throw new ArgumentException("Số biến truyền vào không đúng so với câu truy vấn");
            }
            for (int i = 0; i < paramaterinput.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(paramaterinput[i], inputparameterValues[i]));
            }
            cmd.BindByName = true;
            var rs = cmd.ExecuteNonQuery();
            try
            {
                trans.Commit();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                conn.Close();
                return false;
            }

        }

        #endregion

        #region Phần ExecuteDataset
        public override DataSet ExecuteDataset(string spName, params object[] inputparameterValues)
        {
            return OracleHelper.ExecuteDataset(connectionString, spName, inputparameterValues);
        }
       
        #endregion

        #region Phần ExecuteScalar
        public override object ExecuteScalar(string spName, params object[] inputparameterValues)
        {
            return OracleHelper.ExecuteScalar(connectionString, spName, inputparameterValues);
        }

        public override object ExecuteScalarMaxValue<T>(string inputColumnName)
        {
            string query = "SELECT MAX(" + inputColumnName + ") FROM " + typeof(T).Name;            
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(query, conn);            
            try
            {
                object rs = cmd.ExecuteScalar();
                conn.Close();
                return rs;
            }
            catch
            {                
                conn.Close();
                return -1;
            }
        }

        public override object ExecuteScalarUsingCommandText(string CommandText)
        {
            return OracleHelper.ExecuteScalar(connectionString, CommandType.Text, CommandText);
        }

        public override object ExecuteScalarUsingCommandText(string CommandText, params object[] inputparameterValues)
        {
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleCommand cmd = new OracleCommand(CommandText, conn);
            
            List<string> paramaterinput = new List<string>();
            var splitquery = CommandText.Split(' ', '%', '(', ')');
            for (int i = 0; i < splitquery.Length; i++)
            {
                if (splitquery[i].StartsWith(":") && paramaterinput.Count == 0)
                {
                    paramaterinput.Add(splitquery[i]);
                }
                else if (splitquery[i].StartsWith(":") && paramaterinput.Count > 0 && paramaterinput.Where(t => t.ToUpper().Trim().Equals(splitquery[i].ToUpper().Trim())).FirstOrDefault() == null)
                {
                    paramaterinput.Add(splitquery[i]);
                }
            }
            if (inputparameterValues.Length != paramaterinput.Count)
            {
                throw new ArgumentException("Số biến truyền vào không đúng so với câu truy vấn");
            }
            for (int i = 0; i < paramaterinput.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(paramaterinput[i], inputparameterValues[i]));
            }
            var rs = cmd.ExecuteScalar();
            return rs;
        }
        #endregion

        #region Phần ExecuteReader

        public override IDataReader ExecuteReader(string spName, params object[] inputparameterValues)
        {
            return OracleHelper.ExecuteReader(connectionString, spName, inputparameterValues);
        }

        public override IDataReader ExecuteReader(out object outputparameterValue, string spName, params object[] inputparameterValues)
        {
            return OracleHelper.ExecuteReader(out outputparameterValue, connectionString, spName, inputparameterValues); 
        }

        public override IDataReader ExecuteReader(out List<object> ListoutputparameterValues, string spName, params object[] inputparameterValues)
        {
            return OracleHelper.ExecuteReader(out ListoutputparameterValues, connectionString, spName, inputparameterValues);            
        }

        public override IDataReader ExecuteReaderUsingCommandText(string CommandText)
        {
            return OracleHelper.ExecuteReader(connectionString, CommandType.Text, CommandText);
        }

        public override IDataReader ExecuteReaderUsingCommandText(string CommandText, params object[] inputparameterValues)
        {
            OracleConnection conn = new OracleConnection(connectionString);            
            conn.Open();           
            OracleCommand cmd = new OracleCommand(CommandText,conn); 
            List<string> paramaterinput = new List<string>();
            var splitquery = CommandText.Split(' ', '%', '(', ')', ',', '\r','\n');
            for (int i = 0; i < splitquery.Length; i++)
            {
                if (splitquery[i].StartsWith(":") && paramaterinput.Count == 0 )
                {                    
                    paramaterinput.Add(splitquery[i]);
                }
                else if (splitquery[i].StartsWith(":") && paramaterinput.Count > 0 && paramaterinput.Where(t => t.ToUpper().Trim().Equals(splitquery[i].ToUpper().Trim())).FirstOrDefault() == null)
                {
                    paramaterinput.Add(splitquery[i]);
                }
            }
            if (inputparameterValues.Length != paramaterinput.Count)
            {
                throw new ArgumentException("Số biến truyền vào không đúng so với câu truy vấn");     
            }
            for (int i = 0; i < paramaterinput.Count;i++ )
            {
                cmd.Parameters.Add(new OracleParameter(paramaterinput[i], inputparameterValues[i]));
            }
            OracleDataReader dr;
            cmd.BindByName = true;
            dr = cmd.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));
            return (OracleDataReader)dr;
        }

        private static OracleDbType GetOracleDbType(object o)
        {
            if (o is string) return OracleDbType.Varchar2;
            if (o is DateTime) return OracleDbType.Date;
            if (o is Int64) return OracleDbType.Int64;
            if (o is Int32) return OracleDbType.Int32;
            if (o is Int16) return OracleDbType.Int16;
            if (o is short) return OracleDbType.Int16;
            if (o is sbyte) return OracleDbType.Byte;
            if (o is byte) return OracleDbType.Int16;
            if (o is decimal) return OracleDbType.Decimal;
            if (o is float) return OracleDbType.Single;
            if (o is double) return OracleDbType.Double;
            if (o is byte[]) return OracleDbType.Blob;
            return OracleDbType.Varchar2;
        }

        public override IDataReader ExecuteReaderUsingCommandText(out object outputparameterValue, string CommandText)
        {
            return OracleHelper.ExecuteReader(out outputparameterValue , connectionString, CommandType.Text, CommandText);
        }

        public override IDataReader ExecuteReaderUsingCommandText(out List<object> ListoutputparameterValues, string CommandText)
        {
            return OracleHelper.ExecuteReader(out ListoutputparameterValues, connectionString, CommandType.Text, CommandText);
        }
        #endregion

        public override bool InsertUsingTransactionOrtherConnection(string ConnectionStr, DataTable datainsert, string DataTableName)
        {
            var fieldcolumn = datainsert.Columns;
            string insqry = "INSERT INTO " + DataTableName + " (";
            for (int i = 0; i < fieldcolumn.Count; i++)
            {
                if (i == fieldcolumn.Count - 1)
                    insqry += fieldcolumn[i].ColumnName + ")";
                else
                    insqry += fieldcolumn[i].ColumnName + ",";
            }
            insqry += "VALUES (";
            for (int i = 0; i < fieldcolumn.Count; i++)
            {
                if (i == fieldcolumn.Count - 1)
                    insqry += ":" + fieldcolumn[i].ColumnName + ")";
                else
                    insqry += ":" + fieldcolumn[i].ColumnName + ",";
            }

            OracleConnection conn = new OracleConnection(ConnectionStr);
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.InsertCommand = new OracleCommand(insqry, conn);
            foreach (DataRow drrow in datainsert.Rows)
            {
                ad.InsertCommand.Parameters.Clear();
                for (int i = 0; i < fieldcolumn.Count; i++)
                {
                    ad.InsertCommand.Parameters.Add(new OracleParameter(":" + fieldcolumn[i].ColumnName, drrow[i]));
                }
                ad.InsertCommand.ExecuteNonQuery();

            }
            try
            {
                trans.Commit();
                conn.Close();
                return true;
            }
            catch(Exception ex)
            {
                trans.Rollback();
                conn.Close();
                return false;
            }
        }

        public override bool InsertUsingTransaction(DataTable datainsert, string DataTableName)
        {            
            var fieldcolumn = datainsert.Columns;
            string insqry = "INSERT INTO " + DataTableName + " (";
            for (int i = 0; i < fieldcolumn.Count; i++)
            {
                if (i == fieldcolumn.Count - 1)
                    insqry += fieldcolumn[i].ColumnName + ")";
                else
                    insqry += fieldcolumn[i].ColumnName + ",";
            }
            insqry += "VALUES (";
            for (int i = 0; i < fieldcolumn.Count; i++)
            {
                if (i == fieldcolumn.Count - 1)
                    insqry += ":" + fieldcolumn[i].ColumnName + ")";
                else
                    insqry += ":" + fieldcolumn[i].ColumnName + ",";
            }           

            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.InsertCommand = new OracleCommand(insqry, conn);            
            foreach (DataRow drrow in datainsert.Rows)
            {
                ad.InsertCommand.Parameters.Clear();
                for (int i = 0; i < fieldcolumn.Count; i++)
                {
                    ad.InsertCommand.Parameters.Add(new OracleParameter(":" + fieldcolumn[i].ColumnName, drrow[i]));
                }                   
                ad.InsertCommand.ExecuteNonQuery();
               
            }
            try
            {
                trans.Commit();
                conn.Close();
                return true;
            }
            catch
            {
                trans.Rollback();
                conn.Close();
                return false;
            }
        }

        public override bool InsertUsingTransactionConnectString<T>(List<T> datainsert, string DataTableName, string constr)
        {

            var fieldcolumn = typeof(T).GetProperties();
            string insqry = "INSERT INTO " + DataTableName + " (";
            for (int i = 0; i < fieldcolumn.Length; i++)
            {
                if (i == fieldcolumn.Length - 1)
                    insqry += fieldcolumn[i].Name + ")";
                else
                    insqry += fieldcolumn[i].Name + ",";
            }
            insqry += "VALUES (";
            for (int i = 0; i < fieldcolumn.Length; i++)
            {
                if (i == fieldcolumn.Length - 1)
                    insqry += ":" + fieldcolumn[i].Name + ")";
                else
                    insqry += ":" + fieldcolumn[i].Name + ",";
            }

            OracleConnection conn = new OracleConnection(constr);
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.InsertCommand = new OracleCommand(insqry, conn);
            foreach (var item in datainsert)
            {
                ad.InsertCommand.Parameters.Clear();
                for (int i = 0; i < fieldcolumn.Length; i++)
                {
                    ad.InsertCommand.Parameters.Add(new OracleParameter(":" + fieldcolumn[i].Name, item.GetType().GetProperty(fieldcolumn[i].Name).GetValue(item, null)));
                }
                ad.InsertCommand.ExecuteNonQuery();

            }
            try
            {
                trans.Commit();
                conn.Close();
                return true;
            }
            catch
            {
                trans.Rollback();
                conn.Close();
                return false;
            }
        }

        public override bool InsertUsingTransaction<T>(List<T> datainsert, string DataTableName)
        {            
            
            var fieldcolumn = typeof(T).GetProperties();
            string insqry = "INSERT INTO " + DataTableName + " (";
            for (int i = 0; i < fieldcolumn.Length; i++)
            {
                if (i == fieldcolumn.Length - 1)
                    insqry += fieldcolumn[i].Name + ")";
                else
                    insqry += fieldcolumn[i].Name + ",";
            }
            insqry += "VALUES (";
            for (int i = 0; i < fieldcolumn.Length; i++)
            {
                if (i == fieldcolumn.Length - 1)
                    insqry += ":" + fieldcolumn[i].Name + ")";
                else
                    insqry += ":" + fieldcolumn[i].Name + ",";
            }

            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.InsertCommand = new OracleCommand(insqry, conn); 
            foreach (var item in datainsert)
            {
                ad.InsertCommand.Parameters.Clear();
                for (int i = 0; i < fieldcolumn.Length; i++)
                {                    
                    ad.InsertCommand.Parameters.Add(new OracleParameter(":" + fieldcolumn[i].Name, item.GetType().GetProperty(fieldcolumn[i].Name).GetValue(item, null)));
                }
                ad.InsertCommand.ExecuteNonQuery();

            }
            try
            {
                trans.Commit();
                conn.Close();
                return true;
            }
            catch
            {
                trans.Rollback();
                conn.Close();
                return false;
            }
        }

        public override int ExecuteNonQueryUpdate<T>(List<T> dataupdate, params string[] fieldCompare)
        {
            string fieldColumnUpdate = string.Empty;
            var fieldcolumns = typeof(T).GetProperties();
            for (int i = 0; i < fieldcolumns.Length; i++)
            {
                if (i < fieldcolumns.Length - 1)
                    fieldColumnUpdate += fieldcolumns[i].Name+ ",";
                else
                    fieldColumnUpdate += fieldcolumns[i].Name;
            }
            return ExecuteNonQueryUpdate(fieldColumnUpdate, dataupdate, fieldCompare);
        }

        public override int ExecuteNonQueryUpdate<T>(string fieldColumnUpdate, List<T> dataupdate, params string[] fieldCompare)
        {
            string DataTableName =  typeof(T).Name;
            return ExecuteNonQueryUpdate(DataTableName, fieldColumnUpdate, dataupdate, fieldCompare);
        }

        public override int ExecuteNonQueryUpdate<T>(string DataTableName, string fieldColumnUpdate, List<T> dataupdate, params string[] fieldCompare)
        {
            var fieldvalues = fieldColumnUpdate.Split(',');
            List<string> ColumnNameUpdate = new List<string>();
            List<string> PropertiesNameSet = new List<string>();
            List<string> ColumnNameCompare = new List<string>();
            List<string> PropertiesCompareSet = new List<string>();
            int fieldCount = fieldvalues[0].Split('=').Length;
            foreach (var values in fieldvalues)
            {
                if (fieldCount != values.Split('=').Length)
                {
                    throw new ArgumentException("Cấu trúc dữ liệu update không đồng nhất");
                }
                else
                {
                    var valuestr = values.Split('=');
                    fieldCount = valuestr.Length;
                    if (valuestr.Length > 1)
                    {
                        PropertiesNameSet.Add(valuestr[1].Trim());
                        ColumnNameUpdate.Add(valuestr[0].Trim());
                    }
                    else
                    {
                        ColumnNameUpdate.Add(valuestr[0].Trim());
                        PropertiesNameSet.Add(valuestr[0].Trim());
                    }
                }
            }
            fieldCount = fieldCompare[0].Split('=').Length;
            foreach (var values in fieldCompare)
            {
                if (fieldCount != values.Split('=').Length)
                {
                    throw new ArgumentException("Cấu trúc dữ liệu so sánh không đồng nhất");
                }
                else
                {
                    var valuestr = values.Split('=');
                    fieldCount = valuestr.Length;
                    if (valuestr.Length > 1)
                    {
                        ColumnNameCompare.Add(valuestr[0].Trim());
                        PropertiesCompareSet.Add(valuestr[1].Trim());
                    }
                    else
                    {
                        ColumnNameCompare.Add(valuestr[0].Trim());
                        PropertiesCompareSet.Add(valuestr[0].Trim());
                    }
                }
            }
            
            int maxlength = 100;
            var countupdate = dataupdate.Count % maxlength == 0 ? dataupdate.Count / maxlength : dataupdate.Count / maxlength + 1;
            int rs = 0;
            for (int i = 0; i < countupdate; i++)
            {
                int limitrowtake = dataupdate.Count - (i + 1) * maxlength > 0 ? maxlength : dataupdate.Count - (i) * maxlength;
                var insertaccess = dataupdate.Skip(i * maxlength).Take(limitrowtake).ToList();
                rs = UpdateList(DataTableName, ColumnNameUpdate, PropertiesNameSet, ColumnNameCompare, PropertiesCompareSet ,insertaccess, fieldCompare);
            }
            return rs;
        }

        public int UpdateList<T>(string DataTableName, List<string> ColumnNameUpdate, List<string> PropertiesNameSet, List<string> ColumnNameCompare, List<string> PropertiesCompareSet,
            List<T> datainsert, params string[] fieldCompare)
        {            
            string updatequery = string.Empty;
            long objindex = 1;
            foreach (var item in datainsert)
            {
                updatequery += objindex == 1 ? "BEGIN " : string.Empty;
                updatequery += "UPDATE " + DataTableName;
                if (ColumnNameUpdate.Count > 1)
                {
                    for (int i = 0; i < ColumnNameUpdate.Count; i++)
                    {
                        if (i == 0)
                            updatequery += " SET " + ColumnNameUpdate[i] + " = :" + PropertiesNameSet[i] + objindex + ",";
                        else if (i == ColumnNameUpdate.Count - 1)
                            updatequery += ColumnNameUpdate[i] + " = :" + PropertiesNameSet[i] + objindex;
                        else
                            updatequery += ColumnNameUpdate[i] + " = :" + PropertiesNameSet[i] + objindex + ",";
                    }
                }
                else
                {
                    updatequery += " SET " + ColumnNameUpdate[0] + " = :" + PropertiesNameSet[0] + objindex;;
                }

                if (ColumnNameCompare.Count > 1)
                {
                    for (int i = 0; i < ColumnNameCompare.Count; i++)
                    {
                        if (i == 0)
                            updatequery += " WHERE " + ColumnNameCompare[i] + " = :" + PropertiesCompareSet[i] + objindex;
                        else if (i == fieldCompare.Length - 1)
                            updatequery += " AND " + ColumnNameCompare[i] + " = :" + PropertiesCompareSet[i] + objindex + " ; ";
                        else
                            updatequery += " AND " + ColumnNameCompare[i] + " = :" + PropertiesCompareSet[i] + objindex;
                    }
                }
                else
                {
                    updatequery += " WHERE " + ColumnNameCompare[0] + " = :" + PropertiesCompareSet[0] + objindex + " ; ";
                }
                updatequery += objindex == datainsert.Count ? " END ;" : string.Empty;
                objindex++;

            }
            objindex = 1;
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            var ColumnResult = GetInformationColumn(conn, DataTableName);
            if (ColumnResult.Count == 0)
            {
                conn.Close();
                throw new ArgumentException("Table "+ DataTableName+" không có trong dữ liệu hoặc chưa được khai báo các cột!");
            }
            foreach (var column in ColumnNameUpdate)
            {
                if (ColumnResult.Where(t => t.ColumnName == column).FirstOrDefault() == null)
                {
                    conn.Close();
                    throw new ArgumentException("Các trường Update không xuất hiện trong DataTable !");                   
                }
            }
            OracleCommand cmd = new OracleCommand(updatequery, conn);
            OracleTransaction trans = conn.BeginTransaction();
            foreach (var item in datainsert)
            {
                for (int i = 0; i < ColumnNameUpdate.Count; i++)
                {
                    cmd.Parameters.Add(new OracleParameter(":" + ColumnNameUpdate[i] + objindex, item.GetType().GetProperty(PropertiesNameSet[i]).GetValue(item, null)));
                }
                for (int i = 0; i < ColumnNameCompare.Count; i++)
                {
                    cmd.Parameters.Add(new OracleParameter(":" + ColumnNameCompare[i] + objindex, item.GetType().GetProperty(PropertiesCompareSet[i]).GetValue(item, null)));
                }
                objindex++;
            }
            cmd.ExecuteNonQuery();
            try
            {
                trans.Commit();
                trans.Dispose();
                conn.Close();
                return 1;
            }
            catch
            {
                trans.Rollback();
                trans.Dispose();
                conn.Close();
                return 0;
            }
            
        }
        public List<InformationColumn> GetInformationColumn(OracleConnection conn , string DataTableName)
        {
            string getinfocolumnquery = " select column_name AS COLUMN_NAME, data_type|| case "
            + "when data_precision is not null and nvl(data_scale,0)>0 then '('||data_precision||','||data_scale||')' "
            + "when data_precision is not null and nvl(data_scale,0)=0 then '('||data_precision||')' "
            + "when data_precision is null and data_scale is not null then '(*,'||data_scale||')' "
            + "when char_length>0 then '('||char_length|| case char_used "
            + " when 'B' then ' Byte'   when 'C' then ' Char' else null end||')' end||decode(nullable, 'N', ' NOT NULL') AS COLUMN_TYPE from user_tab_columns "
            + " where table_name = '"+DataTableName+"'";
            OracleCommand cmd = new OracleCommand(getinfocolumnquery, conn);
            DataTable dataresult = new DataTable();
            var a = cmd.ExecuteReader();
            dataresult.Load(cmd.ExecuteReader());
            return dataresult.AsEnumerable().Select(o => new InformationColumn {
                ColumnName = o.Field<string>("COLUMN_NAME"),
                ColumnType = o.Field<string>("COLUMN_TYPE")
            }).ToList();
        }

        public override bool InsertUsingTransaction<T>(List<T> datainsert, string DataTableName, string constring)
        {
            var fieldcolumn = typeof(T).GetProperties();
            string insqry = "INSERT INTO " + DataTableName + " (";
            for (int i = 0; i < fieldcolumn.Length; i++)
            {
                if (i == fieldcolumn.Length - 1)
                    insqry += fieldcolumn[i].Name + ")";
                else
                    insqry += fieldcolumn[i].Name + ",";
            }
            insqry += "VALUES (";
            for (int i = 0; i < fieldcolumn.Length; i++)
            {
                if (i == fieldcolumn.Length - 1)
                    insqry += ":" + fieldcolumn[i].Name + ")";
                else
                    insqry += ":" + fieldcolumn[i].Name + ",";
            }

            OracleConnection conn = new OracleConnection(constring);
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.InsertCommand = new OracleCommand(insqry, conn);
            foreach (var item in datainsert)
            {
                ad.InsertCommand.Parameters.Clear();
                for (int i = 0; i < fieldcolumn.Length; i++)
                {
                    ad.InsertCommand.Parameters.Add(new OracleParameter(":" + fieldcolumn[i].Name, item.GetType().GetProperty(fieldcolumn[i].Name).GetValue(item, null)));
                }
                ad.InsertCommand.ExecuteNonQuery();

            }
            try
            {
                trans.Commit();
                conn.Close();
                return true;
            }
            catch
            {
                trans.Rollback();
                conn.Close();
                return false;
            }
        }

        public override int ExecuteNonQueryUpdateConnectionOther<T>(string ConnectStr, string DataTableName, string fieldColumnUpdate, List<T> dataupdate, params string[] fieldCompare)
        {
            var fieldvalues = fieldColumnUpdate.Split(',');
            List<string> ColumnNameUpdate = new List<string>();
            List<string> PropertiesNameSet = new List<string>();
            List<string> ColumnNameCompare = new List<string>();
            List<string> PropertiesCompareSet = new List<string>();
            int fieldCount = fieldvalues[0].Split('=').Length;
            foreach (var values in fieldvalues)
            {
                if (fieldCount != values.Split('=').Length)
                {
                    throw new ArgumentException("Cấu trúc dữ liệu update không đồng nhất");
                }
                else
                {
                    var valuestr = values.Split('=');
                    fieldCount = valuestr.Length;
                    if (valuestr.Length > 1)
                    {
                        PropertiesNameSet.Add(valuestr[1].Trim());
                        ColumnNameUpdate.Add(valuestr[0].Trim());
                    }
                    else
                    {
                        ColumnNameUpdate.Add(valuestr[0].Trim());
                        PropertiesNameSet.Add(valuestr[0].Trim());
                    }
                }
            }
            fieldCount = fieldCompare[0].Split('=').Length;
            foreach (var values in fieldCompare)
            {
                if (fieldCount != values.Split('=').Length)
                {
                    throw new ArgumentException("Cấu trúc dữ liệu so sánh không đồng nhất");
                }
                else
                {
                    var valuestr = values.Split('=');
                    fieldCount = valuestr.Length;
                    if (valuestr.Length > 1)
                    {
                        ColumnNameCompare.Add(valuestr[0].Trim());
                        PropertiesCompareSet.Add(valuestr[1].Trim());
                    }
                    else
                    {
                        ColumnNameCompare.Add(valuestr[0].Trim());
                        PropertiesCompareSet.Add(valuestr[0].Trim());
                    }
                }
            }

            int maxlength = 100;
            var countupdate = dataupdate.Count % maxlength == 0 ? dataupdate.Count / maxlength : dataupdate.Count / maxlength + 1;
            int rs = 0;
            for (int i = 0; i < countupdate; i++)
            {
                int limitrowtake = dataupdate.Count - (i + 1) * maxlength > 0 ? maxlength : dataupdate.Count - (i) * maxlength;
                var insertaccess = dataupdate.Skip(i * maxlength).Take(limitrowtake).ToList();
                rs = UpdateList(ConnectStr,DataTableName, ColumnNameUpdate, PropertiesNameSet, ColumnNameCompare, PropertiesCompareSet, insertaccess, fieldCompare);
            }
            return rs;
        }

        public int UpdateList<T>(string ConnectStr, string DataTableName, List<string> ColumnNameUpdate, List<string> PropertiesNameSet, List<string> ColumnNameCompare, List<string> PropertiesCompareSet,
            List<T> datainsert, params string[] fieldCompare)
        {
            string updatequery = string.Empty;
            long objindex = 1;
            foreach (var item in datainsert)
            {
                updatequery += objindex == 1 ? "BEGIN " : string.Empty;
                updatequery += "UPDATE " + DataTableName;
                if (ColumnNameUpdate.Count > 1)
                {
                    for (int i = 0; i < ColumnNameUpdate.Count; i++)
                    {
                        if (i == 0)
                            updatequery += " SET " + ColumnNameUpdate[i] + " = :" + PropertiesNameSet[i] + objindex + ",";
                        else if (i == ColumnNameUpdate.Count - 1)
                            updatequery += ColumnNameUpdate[i] + " = :" + PropertiesNameSet[i] + objindex;
                        else
                            updatequery += ColumnNameUpdate[i] + " = :" + PropertiesNameSet[i] + objindex + ",";
                    }
                }
                else
                {
                    updatequery += " SET " + ColumnNameUpdate[0] + " = :" + PropertiesNameSet[0] + objindex; ;
                }

                if (ColumnNameCompare.Count > 1)
                {
                    for (int i = 0; i < ColumnNameCompare.Count; i++)
                    {
                        if (i == 0)
                            updatequery += " WHERE " + ColumnNameCompare[i] + " = :" + PropertiesCompareSet[i] + objindex;
                        else if (i == fieldCompare.Length - 1)
                            updatequery += " AND " + ColumnNameCompare[i] + " = :" + PropertiesCompareSet[i] + objindex + " ; ";
                        else
                            updatequery += " AND " + ColumnNameCompare[i] + " = :" + PropertiesCompareSet[i] + objindex;
                    }
                }
                else
                {
                    updatequery += " WHERE " + ColumnNameCompare[0] + " = :" + PropertiesCompareSet[0] + objindex + " ; ";
                }
                updatequery += objindex == datainsert.Count ? " END ;" : string.Empty;
                objindex++;

            }
            objindex = 1;
            OracleConnection conn = new OracleConnection(ConnectStr);
            conn.Open();
            var ColumnResult = GetInformationColumn(conn, DataTableName);
            if (ColumnResult.Count == 0)
            {
                conn.Close();
                throw new ArgumentException("Table " + DataTableName + " không có trong dữ liệu hoặc chưa được khai báo các cột!");
            }
            foreach (var column in ColumnNameUpdate)
            {
                if (ColumnResult.Where(t => t.ColumnName == column).FirstOrDefault() == null)
                {
                    conn.Close();
                    throw new ArgumentException("Các trường Update không xuất hiện trong DataTable !");
                }
            }
            OracleCommand cmd = new OracleCommand(updatequery, conn);
            OracleTransaction trans = conn.BeginTransaction();
            foreach (var item in datainsert)
            {
                for (int i = 0; i < ColumnNameUpdate.Count; i++)
                {
                    cmd.Parameters.Add(new OracleParameter(":" + ColumnNameUpdate[i] + objindex, item.GetType().GetProperty(PropertiesNameSet[i]).GetValue(item, null)));
                }
                for (int i = 0; i < ColumnNameCompare.Count; i++)
                {
                    cmd.Parameters.Add(new OracleParameter(":" + ColumnNameCompare[i] + objindex, item.GetType().GetProperty(PropertiesCompareSet[i]).GetValue(item, null)));
                }
                objindex++;
            }
            cmd.ExecuteNonQuery();
            try
            {
                trans.Commit();
                trans.Dispose();
                conn.Close();
                return 1;
            }
            catch
            {
                trans.Rollback();
                trans.Dispose();
                conn.Close();
                return 0;
            }
        }

        public override IDataReader ExecuteReaderUsingCommandTextOrtherConnection(string ConnectStr, string CommandText, params object[] inputparameterValues)
        {
            OracleConnection conn = new OracleConnection(ConnectStr);
            conn.Open();
            OracleCommand cmd = new OracleCommand(CommandText, conn);
            List<string> paramaterinput = new List<string>();
            var splitquery = CommandText.Split(' ', '%', '(', ')');
            for (int i = 0; i < splitquery.Length; i++)
            {
                if (splitquery[i].StartsWith(":") && paramaterinput.Count == 0)
                {
                    paramaterinput.Add(splitquery[i]);
                }
                else if (splitquery[i].StartsWith(":") && paramaterinput.Count > 0 && paramaterinput.Where(t => t.ToUpper().Trim().Equals(splitquery[i].ToUpper().Trim())).FirstOrDefault() == null)
                {
                    paramaterinput.Add(splitquery[i]);
                }
            }
            if (inputparameterValues.Length != paramaterinput.Count)
            {
                throw new ArgumentException("Số biến truyền vào không đúng so với câu truy vấn");
            }
            for (int i = 0; i < paramaterinput.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(paramaterinput[i], inputparameterValues[i]));
            }
            OracleDataReader dr;
            cmd.BindByName = true;
            dr = cmd.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));
            return (OracleDataReader)dr;
        }

        public override IDataReader ExecuteReaderUsingCommandText<T>(List<T> listdata, params string[] fieldCompare)
        {
            string DataTableName = typeof(T).Name;
            return ExecuteReaderUsingCommandText(DataTableName, listdata, fieldCompare);            
        }

        public override IDataReader ExecuteReaderUsingCommandText<T>(string DataTableName, List<T> listdata, params string[] fieldCompare)
        {            
            OracleConnection conn = new OracleConnection(connectionString);
            return ExecuteReaderUsingCommandTextWithConnectString(conn, DataTableName, listdata, fieldCompare);
        }

        public override IDataReader ExecuteReaderUsingCommandTextOrtherConnection<T>(string ConnectStr, List<T> listdata, params string[] fieldCompare)
        {
            string DataTableName = typeof(T).Name;
            return ExecuteReaderUsingCommandTextOrtherConnection(ConnectStr, DataTableName, listdata, fieldCompare);
        }

        public override IDataReader ExecuteReaderUsingCommandTextOrtherConnection<T>(string ConnectStr, string DataTableName, List<T> listdata, params string[] fieldCompare)
        {            
            OracleConnection conn = new OracleConnection(ConnectStr);
            return ExecuteReaderUsingCommandTextWithConnectString(conn, DataTableName, listdata, fieldCompare);
        }

        public IDataReader ExecuteReaderUsingCommandTextWithConnectString<T>(OracleConnection conn , string DataTableName, List<T> listdata, params string[] fieldCompare)
        {
            if (fieldCompare.Length > 0 && listdata.Count > 0)
            {                
                conn.Open();               
                string selectquery = string.Empty;
                long objindex = 1;
                selectquery = "SELECT * FROM " + DataTableName;
                for (int index = 0; index < listdata.Count; index++)
                {
                    if (index == 0)
                        selectquery += " WHERE ";
                    if (index != 0)
                        selectquery += " OR ";
                    for (int i = 0; i < fieldCompare.Length; i++)
                    {
                        if (i == 0)
                            selectquery += " (" + fieldCompare[i] + " = :" + fieldCompare[i] + objindex;
                        else if (i == fieldCompare.Length - 1)
                            selectquery += " AND " + fieldCompare[i] + " = :" + fieldCompare[i] + objindex + " ) ";
                        else
                            selectquery += " AND " + fieldCompare[i] + " = :" + fieldCompare[i] + objindex;

                    }
                    objindex++;
                }
                OracleCommand cmd = new OracleCommand(selectquery, conn);
                objindex = 1;
                foreach (var item in listdata)
                {
                    for (int i = 0; i < fieldCompare.Length; i++)
                    {
                        cmd.Parameters.Add(new OracleParameter(":" + fieldCompare[i] + objindex, item.GetType().GetProperty(fieldCompare[i]).GetValue(item, null)));
                    }
                    objindex++;
                }
                OracleDataReader dr;
                cmd.BindByName = true;
                dr = cmd.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));
                return (OracleDataReader)dr;
            }            
            return null;           
        }

        public override int ExecuteNonQueryDelete<T>(List<T> data, params string[] fieldCompare)
        {
            string DataTableName = typeof(T).Name;
            return ExecuteNonQueryDelete(DataTableName, data, fieldCompare);
        }

        public override int ExecuteNonQueryDelete<T>(string DataTableName, List<T> data, params string[] fieldCompare)
        {
            OracleConnection conn = new OracleConnection(connectionString);
            return ExecuteNonQueryDelete(conn, DataTableName, data, fieldCompare);
        }

        public override int ExecuteNonQueryOrtherConnectDelete<T>(string connString, List<T> data, params string[] fieldCompare)
        {
            string DataTableName = typeof(T).Name;
            return ExecuteNonQueryOrtherConnectDelete(connString, DataTableName, data, fieldCompare);
        }

        public override int ExecuteNonQueryOrtherConnectDelete<T>(string connString, string DataTableName, List<T> data, params string[] fieldCompare)
        {
            OracleConnection conn = new OracleConnection(connString);
            return ExecuteNonQueryDelete(conn, DataTableName, data, fieldCompare);
        }

        public int ExecuteNonQueryDelete<T>(OracleConnection conn, string DataTableName, List<T> data, params string[] fieldCompare)
        {
            if (fieldCompare.Length > 0 && data.Count > 0)
            {
                conn.Open();
                string deletequery = "DELETE " + DataTableName;
                long objindex = 1;
                for (int index = 0; index < data.Count; index++)
                {
                    if (index == 0)
                        deletequery += " WHERE ";
                    if (index != 0)
                        deletequery += " OR ";
                    if (fieldCompare.Length > 1)
                    {
                        for (int i = 0; i < fieldCompare.Length; i++)
                        {
                            if (i == 0)
                                deletequery += " (" + fieldCompare[i] + " = :" + fieldCompare[i] + objindex;
                            else if (i == fieldCompare.Length - 1)
                                deletequery += " AND " + fieldCompare[i] + " = :" + fieldCompare[i] + objindex + " ) ";
                            else
                                deletequery += " AND " + fieldCompare[i] + " = :" + fieldCompare[i] + objindex;
                        }
                    }
                    else
                    {
                        deletequery += fieldCompare[0] + " = :" + fieldCompare[0] + objindex;
                    }
                    objindex++;
                }
                OracleCommand cmd = new OracleCommand(deletequery, conn);
                OracleTransaction trans = conn.BeginTransaction();

                objindex = 1;

                foreach (var item in data)
                {
                    for (int i = 0; i < fieldCompare.Length; i++)
                    {
                        cmd.Parameters.Add(new OracleParameter(":" + fieldCompare[i] + objindex, item.GetType().GetProperty(fieldCompare[i]).GetValue(item, null)));
                    }
                    objindex++;
                }

                cmd.ExecuteNonQuery();
                try
                {
                    trans.Commit();
                    conn.Close();
                    return 1;
                }
                catch
                {
                    trans.Rollback();
                    conn.Close();
                    return -1;
                }
            }
            return -1;
        }

        public override IDataReader ExecuteReaderUsingCommandTextFillterColumn(object inputFillterColumn, string CommandText, params object[] inputparameterValues)
        {
            var fieldcolumns = inputFillterColumn.GetType().GetProperties();
            string dieukienloc = string.Empty;
            List<object> inputparameterValues_Final = new List<object>();
            int index = 0;
            foreach (var column in fieldcolumns)
            {
                var value_obj = inputFillterColumn.GetType().GetProperty(column.Name).GetValue(inputFillterColumn, null);
                if (value_obj != null && value_obj != string.Empty)
                {
                    if (index == 0)
                        dieukienloc += @" UPPER(TRIM(" + column.Name + ")) like UPPER(TRIM( :P_" + column.Name + ")) || '%' ";
                    else
                        dieukienloc += @" AND UPPER(TRIM(" + column.Name + ")) like UPPER(TRIM( :P_" + column.Name + "))|| '%'  ";
                    index++;
                    inputparameterValues_Final.Add(value_obj);
                }
            }

            inputparameterValues_Final.AddRange(inputparameterValues.ToList());
            if (inputparameterValues_Final.Count > inputparameterValues.Length)
            {
                CommandText = CommandText.Replace("--DIEUKIENLOC--", dieukienloc + " AND ");
            }
            else
            {
                CommandText = CommandText.Replace("--DIEUKIENLOC--", dieukienloc);
            }
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(CommandText, conn);
            List<string> paramaterinput = new List<string>();
            var splitquery = CommandText.Split(' ', '%', '(', ')', ',', '\r', '\n');
            for (int i = 0; i < splitquery.Length; i++)
            {
                if (splitquery[i].StartsWith(":") && paramaterinput.Count == 0)
                {
                    paramaterinput.Add(splitquery[i]);
                }
                else if (splitquery[i].StartsWith(":") && paramaterinput.Count > 0 && paramaterinput.Where(t => t.ToUpper().Trim().Equals(splitquery[i].ToUpper().Trim())).FirstOrDefault() == null)
                {
                    paramaterinput.Add(splitquery[i]);
                }
            }
            if (inputparameterValues_Final.Count != paramaterinput.Count)
            {
                throw new ArgumentException("Số biến truyền vào không đúng so với câu truy vấn");
            }
            for (int i = 0; i < paramaterinput.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(paramaterinput[i], inputparameterValues_Final[i]));
            }
            OracleDataReader dr;
            cmd.BindByName = true;
            dr = cmd.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));
            return (OracleDataReader)dr;
        }

        public override IDataReader ExecuteReaderUsingCommandTextFillterColumnAndOr(object inputFillterColumn, string CommandText, params object[] inputparameterValues)
        {            
            var fieldcolumns = inputFillterColumn.GetType().GetProperties();
            string dieukienloc = string.Empty;           
            List<object> inputparameterValues_Final = new List<object>();
            int index = 0;
            foreach (var column in fieldcolumns)
            {
                var value_obj = inputFillterColumn.GetType().GetProperty(column.Name).GetValue(inputFillterColumn, null);
                if (value_obj != null && value_obj != string.Empty)
                {
                    if (index == 0)
                        dieukienloc += @" UPPER(TRIM(" + column.Name + ")) like UPPER(TRIM( :P_" + column.Name + ")) || '%' ";
                    else
                        dieukienloc += @" AND UPPER(TRIM(" + column.Name + ")) like UPPER(TRIM( :P_" + column.Name + "))|| '%'  ";
                    index++;
                    inputparameterValues_Final.Add(value_obj);
                }
            }
            var index1 = 0;
            foreach (var input in inputparameterValues)
            {
                if (input != null && (input.ToString().Contains('&') || input.ToString().Contains('+') || input.ToString().Contains('|') || input.ToString().Contains(';')))
                {
                    index1--;
                    var arr_keyval = input.ToString().Split('&', '+', '|', ';');
                    foreach (var data in arr_keyval)
                    {
                        inputparameterValues_Final.Add(data);
                        index1++;
                    }                    
                }
                else
                {
                    inputparameterValues_Final.Add(input);
                }
            }
            if (inputparameterValues_Final.Count > (inputparameterValues.Length + index1))
            {
                CommandText = CommandText.Replace("--DIEUKIENLOC--", dieukienloc + " AND ");
            }
            else
            {
                CommandText = CommandText.Replace("--DIEUKIENLOC--", dieukienloc);
            }
            CommandText = CommandText.Replace("  ", " ");
            CommandText = CommandText.Replace(" WHERE AND ", " WHERE ");
            CommandText = CommandText.Replace(" AND AND ", " AND ");            
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(CommandText, conn);
            List<string> paramaterinput = new List<string>();
            var splitquery = CommandText.Split(' ', '%', '(', ')', ',', '\r', '\n');
            for (int i = 0; i < splitquery.Length; i++)
            {
                if (splitquery[i].StartsWith(":") && paramaterinput.Count == 0)
                {
                    paramaterinput.Add(splitquery[i]);
                }
                else if (splitquery[i].StartsWith(":") && paramaterinput.Count > 0 && paramaterinput.Where(t => t.ToUpper().Trim().Equals(splitquery[i].ToUpper().Trim())).FirstOrDefault() == null)
                {
                    paramaterinput.Add(splitquery[i]);
                }
            }
            if (inputparameterValues_Final.Count != paramaterinput.Count)
            {
                throw new ArgumentException("Số biến truyền vào không đúng so với câu truy vấn");
            }
            for (int i = 0; i < paramaterinput.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(paramaterinput[i], inputparameterValues_Final[i]));
            }
            OracleDataReader dr;
            cmd.BindByName = true;
            dr = cmd.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));
            return (OracleDataReader)dr;
        }

        public override IDataReader ExecuteReaderUsingCommandTextFillterColumnAndOrOtherConnection(string Connectstr, object inputFillterColumn, string CommandText, params object[] inputparameterValues)
        {
            var fieldcolumns = inputFillterColumn.GetType().GetProperties();
            string dieukienloc = string.Empty;
            List<object> inputparameterValues_Final = new List<object>();
            int index = 0;
            foreach (var column in fieldcolumns)
            {
                var value_obj = inputFillterColumn.GetType().GetProperty(column.Name).GetValue(inputFillterColumn, null);
                if (value_obj != null && value_obj != string.Empty)
                {
                    if (index == 0)
                        dieukienloc += @" UPPER(TRIM(" + column.Name + ")) like UPPER(TRIM( :P_" + column.Name + " )) || '%' ";
                    else
                        dieukienloc += @" AND UPPER(TRIM(" + column.Name + ")) like UPPER(TRIM( :P_" + column.Name + " ))|| '%'  ";
                    index++;
                    inputparameterValues_Final.Add(value_obj);
                }
            }
            var index1 = 0;
            foreach (var input in inputparameterValues)
            {
                if (input.ToString().Contains('&') || input.ToString().Contains('+') || input.ToString().Contains('|') || input.ToString().Contains(';'))
                {
                    index1--;
                    var arr_keyval = input.ToString().Split('&', '+', '|', ';');
                    foreach (var data in arr_keyval)
                    {
                        inputparameterValues_Final.Add(data);
                        index1++;
                    }
                }
                else
                {
                    inputparameterValues_Final.Add(input);
                }
            }
            if (inputparameterValues_Final.Count > (inputparameterValues.Length + index1))
            {
                CommandText = CommandText.Replace("--DIEUKIENLOC--", dieukienloc + " AND ");
            }
            else
            {
                CommandText = CommandText.Replace("--DIEUKIENLOC--", dieukienloc);
            }
            CommandText = CommandText.Replace("  ", " ");
            CommandText = CommandText.Replace(" WHERE AND ", " WHERE ");
            CommandText = CommandText.Replace(" AND AND ", " AND ");
            OracleConnection conn = new OracleConnection(Connectstr);
            conn.Open();
            OracleCommand cmd = new OracleCommand(CommandText, conn);
            List<string> paramaterinput = new List<string>();
            var splitquery = CommandText.Split(' ', '%', '(', ')', ',', '\r', '\n');
            for (int i = 0; i < splitquery.Length; i++)
            {
                if (splitquery[i].StartsWith(":") && paramaterinput.Count == 0)
                {
                    paramaterinput.Add(splitquery[i]);
                }
                else if (splitquery[i].StartsWith(":") && paramaterinput.Count > 0 && paramaterinput.Where(t => t.ToUpper().Trim().Equals(splitquery[i].ToUpper().Trim())).FirstOrDefault() == null)
                {
                    paramaterinput.Add(splitquery[i]);
                }
            }
            if (inputparameterValues_Final.Count != paramaterinput.Count)
            {
                throw new ArgumentException("Số biến truyền vào không đúng so với câu truy vấn");
            }
            for (int i = 0; i < paramaterinput.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(paramaterinput[i], inputparameterValues_Final[i]));
            }
            OracleDataReader dr;
            cmd.BindByName = true;
            dr = cmd.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));
            return (OracleDataReader)dr;
        }

        public override IDataReader ExecuteReaderUsingCommandTextFillterContainColumn(object inputFillterColumn, string CommandText, params object[] inputparameterValues)
        {
            var fieldcolumns = inputFillterColumn.GetType().GetProperties();
            string dieukienloc = string.Empty;
            List<object> inputparameterValues_Final = new List<object>();
            int index = 0;
            foreach (var column in fieldcolumns)
            {
                var value_obj = inputFillterColumn.GetType().GetProperty(column.Name).GetValue(inputFillterColumn, null);
                if (value_obj != null && value_obj != string.Empty)
                {
                    if (index == 0)
                        dieukienloc += @" UPPER(TRIM(" + column.Name + ")) like '%' || UPPER(TRIM( :P_" + column.Name + ")) || '%' ";
                    else
                        dieukienloc += @" AND UPPER(TRIM(" + column.Name + ")) like '%' || UPPER(TRIM( :P_" + column.Name + "))|| '%'  ";
                    index++;
                    inputparameterValues_Final.Add(value_obj);
                }
            }

            inputparameterValues_Final.AddRange(inputparameterValues.ToList());
            if (inputparameterValues_Final.Count > inputparameterValues.Length)
            {
                CommandText = CommandText.Replace("--DIEUKIENLOC--", dieukienloc + " AND ");
            }
            else
            {
                CommandText = CommandText.Replace("--DIEUKIENLOC--", dieukienloc);
            }
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(CommandText, conn);
            List<string> paramaterinput = new List<string>();
            var splitquery = CommandText.Split(' ', '%', '(', ')', ',', '\r', '\n');
            for (int i = 0; i < splitquery.Length; i++)
            {
                if (splitquery[i].StartsWith(":") && paramaterinput.Count == 0)
                {
                    paramaterinput.Add(splitquery[i]);
                }
                else if (splitquery[i].StartsWith(":") && paramaterinput.Count > 0 && paramaterinput.Where(t => t.ToUpper().Trim().Equals(splitquery[i].ToUpper().Trim())).FirstOrDefault() == null)
                {
                    paramaterinput.Add(splitquery[i]);
                }
            }
            if (inputparameterValues_Final.Count != paramaterinput.Count)
            {
                throw new ArgumentException("Số biến truyền vào không đúng so với câu truy vấn");
            }
            for (int i = 0; i < paramaterinput.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(paramaterinput[i], inputparameterValues_Final[i]));
            }
            OracleDataReader dr;
            cmd.BindByName = true;
            dr = cmd.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));
            return (OracleDataReader)dr;
        }

        public override IDataReader ExecuteReaderUsingCommandTextFillterContainColumnAndOr(object inputFillterColumn, string CommandText, params object[] inputparameterValues)
        {
            var fieldcolumns = inputFillterColumn.GetType().GetProperties();
            string dieukienloc = string.Empty;
            List<object> inputparameterValues_Final = new List<object>();
            int index = 0;
            foreach (var column in fieldcolumns)
            {
                var value_obj = inputFillterColumn.GetType().GetProperty(column.Name).GetValue(inputFillterColumn, null);
                if (value_obj != null && value_obj != string.Empty)
                {
                    if (index == 0)
                        dieukienloc += @" UPPER(TRIM(" + column.Name + ")) like '%' || UPPER(TRIM( :P_" + column.Name + ")) || '%' ";
                    else
                        dieukienloc += @" AND UPPER(TRIM(" + column.Name + ")) like '%' || UPPER(TRIM( :P_" + column.Name + "))|| '%'  ";
                    index++;
                    inputparameterValues_Final.Add(value_obj);
                }
            }
            var index1 = 0;
            foreach (var input in inputparameterValues)
            {
                if (input.ToString().Contains('&') || input.ToString().Contains('+') || input.ToString().Contains('|') || input.ToString().Contains(';'))
                {
                    index1--;
                    var arr_keyval = input.ToString().Split('&', '+', '|', ';');
                    foreach (var data in arr_keyval)
                    {
                        inputparameterValues_Final.Add(data);
                        index1++;
                    }
                }
                else
                {
                    inputparameterValues_Final.Add(input);
                }
            }
            if (inputparameterValues_Final.Count > (inputparameterValues.Length + index1))
            {
                CommandText = CommandText.Replace("--DIEUKIENLOC--", dieukienloc + " AND ");
            }
            else
            {
                CommandText = CommandText.Replace("--DIEUKIENLOC--", dieukienloc);
            }
            CommandText = CommandText.Replace("  ", " ");
            CommandText = CommandText.Replace(" WHERE AND ", " WHERE ");
            CommandText = CommandText.Replace(" AND AND ", " AND ");
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand(CommandText, conn);
            List<string> paramaterinput = new List<string>();
            var splitquery = CommandText.Split(' ', '%', '(', ')', ',', '\r', '\n');
            for (int i = 0; i < splitquery.Length; i++)
            {
                if (splitquery[i].StartsWith(":") && paramaterinput.Count == 0)
                {
                    paramaterinput.Add(splitquery[i]);
                }
                else if (splitquery[i].StartsWith(":") && paramaterinput.Count > 0 && paramaterinput.Where(t => t.ToUpper().Trim().Equals(splitquery[i].ToUpper().Trim())).FirstOrDefault() == null)
                {
                    paramaterinput.Add(splitquery[i]);
                }
            }
            if (inputparameterValues_Final.Count != paramaterinput.Count)
            {
                throw new ArgumentException("Số biến truyền vào không đúng so với câu truy vấn");
            }
            for (int i = 0; i < paramaterinput.Count; i++)
            {
                cmd.Parameters.Add(new OracleParameter(paramaterinput[i], inputparameterValues_Final[i]));
            }
            OracleDataReader dr;
            cmd.BindByName = true;
            dr = cmd.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));
            return (OracleDataReader)dr;
        }
    }
}