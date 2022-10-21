using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class AppConfig
    {
        public static string dir_path_upload => System.Configuration.ConfigurationManager.AppSettings["path_upload"].ToString();
        public static string dir_path_download => System.Configuration.ConfigurationManager.AppSettings["path_download"].ToString();
        public static string path_template_import_don_kntc => System.Configuration.ConfigurationManager.AppSettings["path_template_import_don_kntc"].ToString();
        public static string key => System.Configuration.ConfigurationManager.AppSettings["key_encript"].ToString();
        public static int IQUOCHOI_COQUAN => StringHelper.ToInt32OrDefault(System.Configuration.ConfigurationManager.AppSettings["IQUOCHOI_COQUAN"].ToString());
        public static int IQUOCHOI_COQUAN_DOANDBQH => StringHelper.ToInt32OrDefault(System.Configuration.ConfigurationManager.AppSettings["IQUOCHOI_COQUAN_DOANDBQH"].ToString());
        public static int IQUOCHOI_COQUAN_HDNDTINH => StringHelper.ToInt32OrDefault(System.Configuration.ConfigurationManager.AppSettings["IQUOCHOI_COQUAN_HDNDTINH"].ToString());
        public static int IDIAPHUONG => StringHelper.ToInt32OrDefault(System.Configuration.ConfigurationManager.AppSettings["IDIAPHUONG"].ToString());
        public static string TEN_DIA_PHUONG => System.Configuration.ConfigurationManager.AppSettings["TEN_DIA_PHUONG"].ToString();
        public static int ID_BAN_DAN_NGUYEN_NEW => StringHelper.ToInt32OrDefault(System.Configuration.ConfigurationManager.AppSettings["ID_BAN_DAN_NGUYEN_NEW"].ToString());
        public static int ID_UY_BAN_NHAN_DAN => StringHelper.ToInt32OrDefault(System.Configuration.ConfigurationManager.AppSettings["ID_UY_BAN_NHAN_DAN"].ToString());
        public static int ID_BAN_DAN_NGUYEN_NEW_PARENT => StringHelper.ToInt32OrDefault(System.Configuration.ConfigurationManager.AppSettings["ID_BAN_DAN_NGUYEN_NEW_PARENT"].ToString());
    }
}
