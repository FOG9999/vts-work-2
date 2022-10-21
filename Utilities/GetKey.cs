using System;

namespace Utilities
{
    public static class GetKey
    {
        public static Decimal get_id_seq(string id_vien, int v_length)
        {
            Decimal areturn = 0;
            try
            {
                string atmp = id_vien;
                atmp += DateTime.Now.ToString("fffssmmHHddMMyyyy");// f_get_data("", "select to_char(sysdate,'yyyymmddhhmmss') as id").Tables[0].Rows[0][0].ToString().Trim();
                if (atmp.Length < v_length)
                {
                    atmp = atmp.PadRight(v_length - atmp.Length, '0');
                }
                areturn = System.Convert.ToDecimal(atmp.Substring(0, v_length));
            }
            catch
            {
            }
            return areturn;
        }
        public static long ConvertToTimestamp(DateTime value)
        {
            TimeZoneInfo NYTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime NyTime = TimeZoneInfo.ConvertTime(value, NYTimeZone);
            TimeZone localZone = TimeZone.CurrentTimeZone;
            System.Globalization.DaylightTime dst = localZone.GetDaylightChanges(NyTime.Year);
            NyTime = NyTime.AddHours(-1);
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            TimeSpan span = (NyTime - epoch);
            return (long)Convert.ToDouble(span.TotalSeconds);
        }

        /// <summary>
        /// Hàm tạo 1 chuỗi làm id (chuỗi các số)
        /// </summary>
        /// <param name="prefix">tiền tố</param>
        /// <param name="keyLength">độ dài chuỗi</param>
        /// <param name="randomLength">độ dài chuỗi ngẫu nhiên</param>
        /// <param name="formatDate">format Chuoi thoi gian can lay (Mac dinh la "yyMMddHHmmssfff")</param>
        /// <returns></returns>
        /// quannd8
        public static string GetIdString(string prefix, int keyLength, int randomLength = 6, string formatDate = "yyMMddHHmmssfff")
        {
            string strKeyId = prefix;
            try
            {
                string strMaxRandom = "9";
                string strMinRandom = "1";

                for (int i = 0; i < randomLength - 1; i++)
                {
                    strMaxRandom += "9";
                    strMinRandom += "0";
                }

                Random ran = new Random();
                strKeyId += ran.Next(Convert.ToInt32(strMinRandom), Convert.ToInt32(strMaxRandom)).ToString();
                strKeyId += DateTime.Now.ToString(formatDate);
                if (strKeyId.Length < keyLength)
                {
                    strKeyId += DateTime.Now.Millisecond.ToString().PadLeft(keyLength - strKeyId.Length, '0');
                }

                return strKeyId.Substring(0, keyLength);
            }
            catch
            {
                return null;
            }
        }
    }
}
