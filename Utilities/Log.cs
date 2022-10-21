using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utilities
{
    public class Log
    {

        public Boolean Log_Login(string log_type, string user_name, int success)
        {
            bool result = true;
            try
            {
                var line = Environment.NewLine + Environment.NewLine;
                string strPath = HttpContext.Current.Server.MapPath("~/Log/Login/"+DateTime.Now.Year+"/"+DateTime.Now.Month+"/");
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                strPath = strPath + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(strPath))
                {
                    File.Create(strPath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(strPath))
                {
                    string error = log_type + "|" +
                        HttpContext.Current.Request.Url + ":" +
                        HttpContext.Current.Request.Url.Port + "|KNCT|" +
                        DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss:ms") + "|null|" +
                        user_name + "|" +
                        HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] +
                        "|Home/Login|" + success + "|0";
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Log_Error(ex, "Không ghi log được phiên LOGIN|LOGOUT dữ liệu");
            }

            return result;
        }
        public Boolean Log_Error(Exception ex, string err_content = "null", string CLASS_NAME = "unknow", int error_level = 1)
        {
            bool result = true;
            try
            {
                var line = Environment.NewLine + Environment.NewLine;
                string strPath = HttpContext.Current.Server.MapPath("~/Log/Log_Error/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/");

                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                strPath = strPath + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(strPath))
                {
                    File.Create(strPath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(strPath))
                {
                    //ERROR|10.65.65.45:8170|DMS.DOMESCO|29/08/2017 16:34:40.000| CLASS_NAME| ERR_Message|err_content|error_level
                    string error = "ERROR|" +
                        HttpContext.Current.Request.Url + ":" +
                        HttpContext.Current.Request.Url.Port + "|KNCT|" +
                        DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss:ms") + "|" +
                        CLASS_NAME + "|" + ex.Message + ": " + ex.StackTrace + "|" + err_content + "|" + error_level;
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
                //không ghi log được lỗi ...
                result = false;
            }
            return result;
        }
        public Boolean LogInfo(string user_name, string function_code = "", string function_type = "",
            Dictionary<string, object> obj = null, bool success = true)
        {
            //LogInfoVO4J log = new LogInfoVO4J();
            bool result = true;
            try
            {
                var line = Environment.NewLine + Environment.NewLine;
                string strPath = HttpContext.Current.Server.MapPath("~/Log/Log_Info/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/");
                
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                strPath = strPath + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(strPath))
                {
                    File.Create(strPath).Dispose();
                }
                string param = "";
                if (obj != null)
                {
                    param = "Param: ";
                    foreach (var t in obj)
                    {
                        param += t.Key + ":" + t.Value + ";";
                    }
                }
                int success_ = 1;
                if (success == false) { success_ = 0; }
                using (StreamWriter sw = File.AppendText(strPath))
                {
                    string error = "INFO|" +
                            HttpContext.Current.Request.Url + ":" +
                            HttpContext.Current.Request.Url.Port + "|KNCT|" +
                            DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss:ms") + "|" +
                            DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss:ms") + "|" +
                            user_name + "|" +
                            HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + "|" +
                            function_code + "|" + function_type + "|" +
                            HttpContext.Current.Request.Url.AbsoluteUri + "|" +
                            param + "|" + success_ + "|0";
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Log_Error(ex, "Không ghi log_info được phiên làm việc");
                result = false;
            }
            return result;
            //return log;
        }
        public Boolean Log_Data_access(Exception ex, string err_content = "null", string CLASS_NAME = "unknow", int error_level = 1)
        {
            bool result = true;
            try
            {
                var line = Environment.NewLine + Environment.NewLine;
                string strPath = HttpContext.Current.Server.MapPath("~/Log/Log_DataAccess/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/");

                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                strPath = strPath + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(strPath))
                {
                    File.Create(strPath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(strPath))
                {
                    //ERROR|10.65.65.45:8170|DMS.DOMESCO|29/08/2017 16:34:40.000| CLASS_NAME| ERR_Message|err_content|error_level
                    string error = "ERROR|" +
                        HttpContext.Current.Request.Url + ":" +
                        HttpContext.Current.Request.Url.Port + "|KNCT|" +
                        DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss:ms") + "|" +
                        CLASS_NAME + "|" + ex.Message + ": " + ex.StackTrace + "|" + err_content + "|" + error_level;
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
                //không ghi log được lỗi ...
                result = false;
            }
            return result;
        }
    }
}
