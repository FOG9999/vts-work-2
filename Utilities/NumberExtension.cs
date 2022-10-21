
using System;

namespace Utilities
{
    public static class NumberExtension
    {
        #region to text money
        public static string ToHeaderMoney(this int money)
        {
            switch (money)
            {
                case 1000:
                    return "1.000 đồng";
                case 1000000:
                    return "triệu đồng";
                default:
                    return "đồng";
            }
        }

        public static string ToStringMoney(this int money)
        {
            return ToStringMoney((long)money);
        }

        public static string ToStringMoney(this double money)
        {
            return ToStringMoney((long)money);
        }
        public static string ToStringMoney(this decimal money)
        {
            return ToStringMoney((long)money);
        }
        //public static string ToStringMoney2(this long Tien)
        //{
        //    var reader = new NumberReader();
        //    return reader.DocTienBangChu(Tien, "đồng");
        //}

        public static string ToStringMoney(this long Tien)
        {
            string text = "";
            if (Tien < 0)
            {
                Tien = -1 * Tien;
            }
            string text2 = "";
            long num = 0L;
            string text3 = "không,một,hai,ba,bốn,năm,sáu,bảy,tám,chín";
            //string text4 = ",nghìn,triệu,tỉ,nghìn tỉ, triệu tỉ, tỉ tỉ";
            string text4 = ",nghìn,triệu,tỷ,nghìn, triệu, tỷ";
            string[] array = text3.Split(',');
            string[] array2 = text4.Split(',');
            do
            {
                long num2 = Tien % 10;
                Tien = (Tien - num2) / 10;
                long num3 = Tien % 10;
                Tien = (Tien - num3) / 10;
                long num4 = Tien % 10;
                Tien = (Tien - num4) / 10;
                if (num2 != 0 || num3 != 0 || num4 != 0)
                {
                    text2 = "";
                    if (num4 != 0 || Tien != 0)
                    {
                        text2 = array[num4] + " trăm";
                    }
                    switch (num3)
                    {
                        case 0L:
                            if (text2 != "" && num2 != 0)
                            {
                                text2 += " linh";
                            }
                            break;
                        case 1L:
                            text2 += " mười";
                            break;
                        default:
                            text2 = text2 + " " + array[num3] + " mươi";
                            break;
                    }
                    int num5;
                    switch (num2)
                    {
                        case 1L:
                            num5 = ((num3 < 2) ? 1 : 0);
                            goto IL_0171;
                        default:
                            num5 = 1;
                            goto IL_0171;
                        case 0L:
                            break;
                            IL_0171:
                            text2 = ((num5 != 0) ? ((num2 != 5 || num3 < 1) ? (text2 + " " + array[num2]) : (text2 + " lăm")) : (text2 + " mốt"));
                            break;
                    }
                    text2 = text2.Trim();
                    if (text2 != "")
                    {
                        //text = text2 + " " + array2[num] + " " + text.Trim();

                        if (array2[num] == "tỷ" && !string.IsNullOrWhiteSpace(text))
                        {
                            text = text2 + " " + array2[num] + ", " + text.Trim();

                        }
                        else
                        {
                            text = text2 + " " + array2[num] + " " + text.Trim();
                        }

                    }
                }
                num++;
            }
            while (Tien != 0);
            text = text.Trim();
            if (text == "")
            {
                text = "không";
            }
            text = text.Substring(0, 1).ToUpper() + text.Substring(1);
            return text + " đồng";
        }

        public static string ToStringNumber(this int number)
        {
            return number.ToString("###,##0");
        }
        #endregion

        #region Convert number
        public static string NToR(int number)
        {
            string roMan = "";
            if ((number < 0) || (number > 3999))
            {
                throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
            }
            else if (number < 1)
            {
                roMan = "";
            }
            else if (number >= 1000)
            {
                roMan = "M" + NToR(number - 1000);
            }
            else if (number >= 900)
            {
                roMan = "CM" + NToR(number - 900);
            }
            else if (number >= 500)
            {
                roMan = "D" + NToR(number - 500);
            }
            else if (number >= 400)
            {
                roMan = "CD" + NToR(number - 400);
            }
            else if (number >= 100)
            {
                roMan = "C" + NToR(number - 100);
            }
            else if (number >= 90)
            {
                roMan = "XC" + NToR(number - 90);
            }
            else if (number >= 50)
            {
                roMan = "L" + NToR(number - 50);
            }
            else if (number >= 40)
            {
                roMan = "XL" + NToR(number - 40);
            }
            else if (number >= 10)
            {
                roMan = "X" + NToR(number - 10);
            }
            else if (number >= 9)
            {
                roMan = "IX" + NToR(number - 9);
            }
            else if (number >= 5)
            {
                roMan = "V" + NToR(number - 5);
            }
            else if (number >= 4)
            {
                roMan = "IV" + NToR(number - 4);
            }
            else if (number >= 1)
            {
                roMan = "I" + NToR(number - 1);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Value must be between 1 and 3999.");
            }
            return roMan;
        }

        public static string ToStringRoman(this int number)
        {
            return NToR(number);
        }
        #endregion

        public static string ToStringDvt(this int dvt)
        {
            if (dvt == 0 || dvt == 1)
            {
                return "đồng";
            }
            else if (dvt == 1000)
            {
                return "1.000 đồng";
            }
            else if (dvt == 1000000)
            {
                return "triệu đồng";
            }
            else if (dvt == 1000000000)
            {
                return "tỷ đồng";
            }
            else
            {
                return $"{dvt.ToString("###.###")} đồng";
            }

        }

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        public static int ToPageCount(this int count, int page_size = 10)
        {
            var r = count / page_size + (count % page_size == 0 ? 0 : 1);
            return r;
        }


        #region bytes

        public static string ToStringBytes(this long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 3);
            return $"{(Math.Sign(byteCount) * num)} {suf[place]}";
        }

        public static string ToStringBytes(this int byteCount)
        {
            return ((long)byteCount).ToStringBytes();
        }
        #endregion
    }
}
