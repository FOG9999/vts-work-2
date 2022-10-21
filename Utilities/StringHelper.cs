using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    public static class StringHelper
    {
        /// <summary>
        /// Convert to short name of people
        /// </summary>
        /// Author: Hungcd1
        /// Description: Lấy tên viết tắt.
        public static string ConvertToShortName(this string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            //Remove Duplicate space character           
            name = Regex.Replace(name, @"\s+", " ");
            var lst = name.Trim().Split(' ');
            name = lst[lst.Length - 1];
            name = name.Substring(0, 1).ToUpper() + name.Remove(0, 1);
            for (int i = 0; i < lst.Length - 1; i++)
                name = lst[i].Substring(0, 1).ToUpper() + name;
            return name;
        }


        /// <summary>
        /// Remove unicode character of Vietnam-VI
        /// </summary>
        /// Author: Hungcd1
        /// Description: Loại dấu trong tiếng việt
        public static string ConvertToUnsign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string ReplaceWhileCharByOne(string data)
        {
            var arrayStr = data.Split(' ').Where(o => !string.IsNullOrEmpty(o.Trim())).ToArray();
            return string.Join(" ", arrayStr);
        }
        public static bool IsNullOrEmpty(string data)
        {
            data = data.Trim();
            data = ReplaceWhileCharByOne(data);
            return string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data);
        }

        public static int ToInt32OrDefault(this string value, int defaultValue = 0)
        {
            int result;
            return int.TryParse(value, out result) ? result : defaultValue;
        }
    }
}
