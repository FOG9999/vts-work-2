using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace DataAccess
{
    public static class ObjectHelper
    {
        public static List<TSource> ToListData<TSource>(this DataTable dataTable) where TSource : class,  new()
        {
            var dataList = new List<TSource>();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                 select new { Name = aProp.Name, Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType }).ToList();
            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                     select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();


            var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();


            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var aTSource = new TSource();
                foreach (var aField in commonFields)
                {
                    PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);

                    var value = (dataRow[aField.Name] == DBNull.Value) ?
                        null
                        : dataRow[aField.Name]; //if database field is nullable

                    propertyInfos.SetValue(aTSource, value, null);
                }
                dataList.Add(aTSource);
            }
            return dataList;
        }

        public static string BuildSqlInsert<T>(this T t, out List<OracleParameter> param) where T : class,  new()
        {
            var properties = t.GetType().GetProperties();
            param = new List<OracleParameter>();
            var tablename = t.GetType().Name;
            var sql = "INSERT INTO " + tablename + " ( \n";
            var sqlParam = " VALUES (\n";

            var allowProperties =
                properties.Where(
                    o => !o.GetType().IsGenericType && !o.GetGetMethod().IsVirtual)
                    .ToArray();

            foreach (var propertyInfo in allowProperties)
            {
                var propName = propertyInfo.Name;
                var paramName = "P_" + propName;
                var value = t.GetType().GetProperty(propName).GetValue(t, null);
                sql += propName + ", \n";
                sqlParam += ":" + paramName + ", \n";
                param.Add(new OracleParameter(paramName, value));
            }
            var lastIndex = sql.LastIndexOf(',');
            sql = sql.Remove(lastIndex, 1) + ")\n";

            lastIndex = sqlParam.LastIndexOf(',');
            sqlParam = sqlParam.Remove(lastIndex, 1) + ")\n";

            return sql + sqlParam;
        }
        public static string BuildSqlUpdate<T>(this T t, out List<OracleParameter> param, params string[] arCondition) where T : class, new()
        {
            var properties = t.GetType().GetProperties();
            param = new List<OracleParameter>();
            var tablename = t.GetType().Name;
            var sql = "UPDATE " + tablename + " SET \n";
            var sqlWhere = "WHERE 1 = 1";

            var allowProperties =
                properties.Where(
                    o => !o.GetType().IsGenericType && !o.GetGetMethod().IsVirtual)
                    .ToArray();

            foreach (var propertyInfo in allowProperties)
            {
                var propName = propertyInfo.Name.ToUpper();
                var paramName = "P_" + propName;
                var value = t.GetType().GetProperty(propName).GetValue(t, null);
                sql += propName + " = :" + paramName + ",\n";
                if (arCondition.Contains(propName))
                {
                    sqlWhere += " AND " + propName + " = :" + paramName + "\n";
                }
                param.Add(new OracleParameter(paramName, value));
            }
            var lastIndex = sql.LastIndexOf(',');
            sql = sql.Remove(lastIndex, 1);

            return sql + sqlWhere;
        }


        public static string BuildSqlDelete<T>(this T t, out List<OracleParameter> param, params string[] arCondition) where T : class, new()
        {
            var properties = t.GetType().GetProperties();
            param = new List<OracleParameter>();
            var tablename = t.GetType().Name;
            var sql = "DELETE " + tablename + " \n";
            var sqlWhere = "WHERE 1 = 1";

            var allowProperties =
                properties.Where(
                    o => !o.GetType().IsGenericType && !o.GetGetMethod().IsVirtual)
                    .ToArray();

            foreach (var propertyInfo in allowProperties)
            {
                var propName = propertyInfo.Name.ToUpper();
                var paramName = "P_" + propName;
                var value = t.GetType().GetProperty(propName).GetValue(t, null);
               
                if (arCondition.Contains(propName))
                {
                    sqlWhere += " AND " + propName + " = :" + paramName + "\n";
                }
                param.Add(new OracleParameter(paramName, value));
            }
            sql += ",\n";
            var lastIndex = sql.LastIndexOf(',');
            sql = sql.Remove(lastIndex, 1);

            return sql + sqlWhere;
        }

    }
}
