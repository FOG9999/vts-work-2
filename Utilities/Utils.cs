using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Utilities
{
    public static class Utils
    {
        //Biến lấy định dạng ngôn ngữ
        public static string strLanguageFormat = ConfigurationManager.AppSettings["LanguageFormat"];
        //Biến lưu giá trị URL đầu tiên
        public static string getUrlDefault = "";
        // Biến lưu giá trị lấy giám định
        public static string giamDinhBV = " AND GIAMDINHBV like '3%'";
        public static string giamDinhBV_2X = " AND GIAMDINHBV like '2%'";

        /// <summary>
        /// Chuyển đổi ngày tháng từ string mm/dd/yyyy sang date
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime? ConvetDateVNToDate(string strDate)
        {
            if (strDate == null || strDate == "")
            {
                return null;
            }
            else
            {
                try
                {
                    strDate = strDate.Trim();
                    if (String.IsNullOrEmpty(strDate)) return DateTime.Today;
                    string[] strs = strDate.Split("/".ToCharArray());

                    int day = Convert.ToInt32(strs[0]);
                    int month = Convert.ToInt32(strs[1]);
                    int year = Convert.ToInt32(strs[2]);
                    return new DateTime(year, month, day);
                }
                catch
                {
                    return null;
                }

            }
        }
        public static DateTime ConvetDateVNToDateTrue(string strDate)
        {
            if (strDate == null || strDate == "")
            {
                return DateTime.Now;
            }
            else
            {
                try
                {
                    strDate = strDate.Trim();
                    if (String.IsNullOrEmpty(strDate)) return DateTime.Today;
                    string[] strs = strDate.Split("/".ToCharArray());

                    int day = Convert.ToInt32(strs[0]);
                    int month = Convert.ToInt32(strs[1]);
                    int year = Convert.ToInt32(strs[2]);
                    return new DateTime(year, month, day);
                }
                catch
                {
                    return DateTime.Now;
                }

            }
        }
        public static string ToString(object obj)
        {
            try
            {
                return Convert.ToString(obj);
            }
            catch
            {
                return "";
            }
        }
        public static DateTime ToDateTime(object obj, string strFormat)
        {
            try
            {
                string dtString = ToString(obj);
                string[] arr = new string[2];
                DateTime dt = DateTime.MinValue;

                if (dtString.IndexOf("/") > -1)
                {
                    arr = dtString.Split('/');
                }
                else if (dtString.IndexOf("-") > -1)
                {
                    arr = dtString.Split('-');
                }
                else
                {
                    return dt;
                }

                switch (strFormat)
                {
                    case "dd/mm/yyyy":
                    case "dd-mm-yyyy":
                        dt = ToDateTime(arr[1] + "/" + arr[0] + "/" + arr[2]);
                        break;
                    case "yyyy/mm/dd":
                    case "yyyy-mm-dd":
                        dt = ToDateTime(arr[1] + "/" + arr[2] + "/" + arr[0]);
                        break;
                    case "yyyy/dd/mm":
                    case "yyyy-dd-mm":
                        dt = ToDateTime(arr[2] + "/" + arr[1] + "/" + arr[0]);
                        break;
                    default:
                        dt = ToDateTime(obj);
                        break;
                }
                return dt;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime ToDateTime(object obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return DateTime.Today;
            }
        }
        /// <summary>
        /// Chuyển đổi ngày tháng từ string dd/mm/yyyy sang date
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime? ConvetDateVNToDates(string strDate)
        {
            if (strDate == null || strDate == "")
            {
                return null;
            }
            else
            {
                try
                {
                    strDate = strDate.Trim();
                    if (String.IsNullOrEmpty(strDate)) return DateTime.Today;
                    string[] strs = strDate.Split("/".ToCharArray());

                    int day = Convert.ToInt32(strs[0]);
                    int month = Convert.ToInt32(strs[1]);
                    int year = Convert.ToInt32(strs[2]);
                    return new DateTime(year, month, day);
                }
                catch
                {
                    return null;
                }

            }
        }
        public static DateTime? sysDate()
        {
            return DateTime.Now;
        }
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public static string Get_String_LoaiKcb(string alias, string param)
        {
            return " (" + param + "=1 and " + alias + ".Loaikcb = 3) or(" + param + " =2 and " + alias + ".Loaikcb in (1,2)) ";
        }

        /// <summary>
        /// hoangnv30 - chuyển từ datarow sang đối tượng
        /// </summary>
        /// <param name="m"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        /// 
        //public static object SetObjectFromDataRow(object m, DataRow dr)
        //{
        //    var lstColumn = m.GetType().GetProperties();
        //    try
        //    {
        //        foreach (var items in lstColumn)
        //        {
        //            string name = items.Name;
        //            object objNew;
        //            var drObj = dr[name];
        //            if (drObj.ToString() != "")
        //            {
        //                string type = String.Empty;
        //                string typeS = items.PropertyType.Name;
        //                if (typeS.ToUpper().Contains("NULL"))
        //                {
        //                    type = items.PropertyType.GetProperties()[1].PropertyType.Name;
        //                    if (type.ToUpper() == "STRING" || type.ToUpper() == "CHAR")
        //                    {
        //                        objNew = drObj.ToString().Trim();
        //                    }
        //                    else if (type.ToUpper().Contains("DATE"))
        //                    {
        //                        objNew = Convert.ToDateTime(drObj);
        //                    }
        //                    else
        //                    {
        //                        objNew = drObj;
        //                    }
        //                }
        //                else
        //                {
        //                    type = typeS;

        //                    if (type.ToUpper() == "STRING" || type.ToUpper() == "CHAR")
        //                    {
        //                        objNew = drObj.ToString().Trim();
        //                    }
        //                    else if (type.ToUpper().Contains("DATE"))
        //                    {
        //                        objNew = Convert.ToDateTime(drObj);
        //                    }
        //                    else
        //                    {
        //                        objNew = drObj;
        //                    }
        //                }
        //                //objNew = drObj;
        //            }
        //            else
        //            {
        //                string type = String.Empty;
        //                string typeS = items.PropertyType.Name;
        //                if (typeS.ToUpper().Contains("NULL"))
        //                {
        //                    type = items.PropertyType.GetProperties()[1].PropertyType.Name;
        //                    objNew = null;
        //                }
        //                else
        //                {
        //                    type = typeS;

        //                    if (type.ToUpper() == "STRING" || type.ToUpper() == "CHAR")
        //                    {
        //                        objNew = "";
        //                    }
        //                    else if (type.ToUpper().Contains("DATE"))
        //                    {
        //                        objNew = DateTime.Now;
        //                    }
        //                    else
        //                    {
        //                        objNew = 0;
        //                    }
        //                }
        //            }

        //            m.GetType().GetProperty(name).SetValue(m, objNew, null);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //throw;
        //    }
        //    return m;
        //}

        /// <summary>
        /// hoangnv30 - chuyển từ datatable thành list object
        /// </summary>
        /// <param name="m"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        //public static List<object> SetObjectFromDataTable(object m, DataTable dt)
        //{
        //    var lst = new List<object>();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {

        //        var dr = dt.Rows[i];

        //        var lstColumn = m.GetType().GetProperties();
        //        foreach (var items in lstColumn)
        //        {
        //            string name = items.Name;
        //            object objNew;
        //            var drObj = dr[name];
        //            if (drObj.ToString() != "")
        //            {
        //                string type = String.Empty;
        //                string typeS = items.PropertyType.Name;
        //                if (typeS.ToUpper().Contains("NULL"))
        //                {
        //                    type = items.PropertyType.GetProperties()[1].PropertyType.Name;
        //                    if (type.ToUpper() == "STRING" || type.ToUpper() == "CHAR")
        //                    {
        //                        objNew = drObj.ToString();
        //                    }
        //                    else if (type.ToUpper().Contains("DATE"))
        //                    {
        //                        objNew = Convert.ToDateTime(drObj);
        //                    }
        //                    else
        //                    {
        //                        objNew = drObj;
        //                    }
        //                }
        //                else
        //                {
        //                    type = typeS;

        //                    if (type.ToUpper() == "STRING" || type.ToUpper() == "CHAR")
        //                    {
        //                        objNew = drObj.ToString();
        //                    }
        //                    else if (type.ToUpper().Contains("DATE"))
        //                    {
        //                        objNew = Convert.ToDateTime(drObj);
        //                    }
        //                    else
        //                    {
        //                        objNew = drObj;
        //                    }
        //                }
        //                //objNew = drObj;
        //            }
        //            else
        //            {
        //                var type = String.Empty;
        //                var typeS = items.PropertyType.Name;
        //                if (typeS.ToUpper().Contains("NULL"))
        //                {
        //                    type = items.PropertyType.GetProperties()[1].PropertyType.Name;
        //                    objNew = null;
        //                }
        //                else
        //                {
        //                    type = typeS;

        //                    if (type.ToUpper() == "STRING" || type.ToUpper() == "CHAR")
        //                    {
        //                        objNew = "";
        //                    }
        //                    else if (type.ToUpper().Contains("DATE"))
        //                    {
        //                        objNew = DateTime.Now;
        //                    }
        //                    else
        //                    {
        //                        objNew = 0;
        //                    }
        //                }
        //            }

        //            m.GetType().GetProperty(name).SetValue(m, objNew, null);
        //        }
        //        lst.Add(m);
        //    }
        //    return lst;
        //}

        /// <summary>
        /// quannd8 - chuyển từ datarow sang đối tượng
        /// </summary>
        /// <param name="m"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T SetObjectFromDataRow<T>(T m, DataRow dr, bool checkExist = false)
        {
            //if (m == null)
            if ( object.Equals(m,default(T)))
                m = (T)Activator.CreateInstance(typeof(T));

            var properties = m.GetType().GetProperties();
            Type type = null;
            Type baseType = null;

            foreach (var items in properties)
            {
                if (checkExist)
                {
                    if (!dr.Table.Columns.Contains(items.Name))
                        continue;
                }
                type = items.PropertyType;
                baseType = Nullable.GetUnderlyingType(items.PropertyType);

                if (baseType != null)
                {
                    type = baseType;
                }

                if (dr[items.Name] != DBNull.Value)
                    items.SetValue(m, Convert.ChangeType(dr[items.Name], type), null);
            }
            return m;
        }

        /// <summary>
        /// quannd8 - chuyển từ datatable thành list object
        /// </summary>
        /// <param name="m"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> SetObjectFromDataTable<T>(T m, DataTable dt, bool checkExist = false)
        {
            var lst = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                obj = SetObjectFromDataRow(obj, dt.Rows[i], checkExist);
                lst.Add(obj);
            }
            return lst;
        }

        public static bool CheckNumber(string number)
        {
            try
            {
                Decimal.Parse(number.Trim());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ThienDL.
        /// Hàm chuyển giá trị toán tử của Grid khi filter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetComparisonByFilter(string value)
        {
            string comparison;
            switch (value)
            {
                case "lt":
                    comparison = "<"; break;
                case "gt":
                    comparison = ">"; break;
                case "eq":
                    comparison = "="; break;
                default: comparison = "="; break;
            }
            return comparison;
        }

        //public static string GetValSeq(string tenSeq)
        //{
        //    DataTable dt = new DataTable();
        //    string nextVal = string.Empty;
        //    string sql = @"SELECT " + tenSeq + ".nextval FROM dual";
        //    dt = OracleHelper.ExecuteSqlDataTable(CommandType.Text, sql, null);
        //    if (dt != null && dt.Rows.Count > 0)
        //        nextVal = dt.Rows[0][0].ToString();
        //    dt.Dispose();
        //    return nextVal.Trim().Replace(" ", "").Replace(";", ",");
        //}
    }
}