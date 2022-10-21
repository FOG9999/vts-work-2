using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using System.Web.UI;
using Entities.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Utilities
{
    public class Funtions
    {
        private Random rnd = new Random();

        public void SetCookies(string cookie, string value)
        {
            HttpCookie cookieset = new HttpCookie(cookie);
            cookieset.Value = value;
            cookieset.Expires = DateTime.Now.AddHours(5);
            HttpContext.Current.Response.Cookies.Add(cookieset);
        }
        public string TomTatNoiDung(string noidung, string id_noidung)
        {
            if (noidung != null)
            {
                noidung = noidung.Replace("\n", "<br /><br />");

                if (noidung.Length >= 500)
                {
                    //noidung = 
                    string noidung_cut = noidung.Substring(0, 500);
                    string[] str_split = noidung_cut.Split(new char[] { ' ' });
                    string noidung_new = "";
                    for (int i = 0; i < str_split.Length - 1; i++)
                    {
                        if (str_split[i] != null)
                        {
                            noidung_new += str_split[i].ToString() + " ";
                        }
                    }
                    //string noidung_new = "";
                    noidung_new = "<p id='des_" + id_noidung + "'>" + noidung_new + " <em onclick=\"$('#full_" + id_noidung + ",#des_" + id_noidung + "').toggle()\" class='f-blue'>[Xem thêm]</em></p>" +
                                    "<p id='full_" + id_noidung + "' style='display:none;'>" + noidung + " </p>";
                    return noidung_new;
                }
                else
                {
                    return noidung;
                }
            }
            return noidung;

        }

        public void Set_Url_keycookie()
        {
            string cookies = Guid.NewGuid().ToString().Substring(0, 4);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("url_key", cookies));
            //SetCookies("url_key", cookies);
        }
        public string Get_Url_keycookie()
        {
            string cookies = "";
            if (HttpContext.Current.Response.Cookies["url_key"] == null)
            {
                SetCookies("url_key", cookies);
            }
            else
            {
                cookies = HttpContext.Current.Response.Cookies["url_key"].Value;
            }
            return cookies;
        }
        public void RemoveCookies(string cookie)
        {
            //HttpContext.Current.Response.Cookies[cookie].Value = string.Empty;
            //HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddHours(-10);

            HttpCookie cookieset = new HttpCookie(cookie);
            cookieset.Value = string.Empty;
            cookieset.Expires = DateTime.Now.AddHours(-10);
            HttpContext.Current.Response.Cookies.Add(cookieset);
        }

        public void Redirect_Current(string url)
        {
            HttpContext.Current.Response.Redirect(url);
        }
        public static string SetValueCheck(Page page)
        {
            Guid antiforgeryToken = Guid.NewGuid();
            page.Session["AntiforgeryToken"] = antiforgeryToken;
            var strValue = antiforgeryToken.ToString();
            return strValue.Remove('-');
        }
        public string ConvertDateVN(string date)
        {
            string str = "";
            str = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(date));
            return str;
        }
        public string Checkbox_Checked(int status)
        {
            if (status == 0) { return ""; } else { return " checked "; }
        }
        public string ConvertDateToSql(string date)
        {
            string str = "";
            if (date.IndexOf("-") > 0)
            {
                string[] str_split = date.Split('-');
                str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
            }
            else if (date.IndexOf("/") > 0)
            {
                string[] str_split = date.Split('/');
                str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
            }
            DateTime date_orc = Convert.ToDateTime(str);
            str = String.Format("{0:dd-MMM-yyyy}", date_orc);
            return str;
        }
        public string RemoveTagInput(string str = "")
        {
            if (str != null)
            {
                if (str != "")
                {

                   // string pattern = "<[^>]*(>|$)";
                   // string replacement = " ";
                   // Regex rgx = new Regex(pattern);
                   // str = rgx.Replace(str, replacement);

                   // //str = str.Replace("<", "&lt;");
                   // //str = str.Replace(">", "&gt;");
                   //// str = str.Replace("alert(", "");
                }
            }
            return str;
        }
       
        public string ConvertVn(string text)
        {
            string[] pattern = new string[7];
            pattern[0] = "a|(á|ả|à|ạ|ã|ă|ắ|ẳ|ằ|ặ|ẵ|â|ấ|ẩ|ầ|ậ|ẫ)";
            pattern[1] = "o|(ó|ỏ|ò|ọ|õ|ô|ố|ổ|ồ|ộ|ỗ|ơ|ớ|ở|ờ|ợ|ỡ)";
            pattern[2] = "e|(é|è|ẻ|ẹ|ẽ|ê|ế|ề|ể|ệ|ễ)";
            pattern[3] = "u|(ú|ù|ủ|ụ|ũ|ư|ứ|ừ|ử|ự|ữ)";
            pattern[4] = "i|(í|ì|ỉ|ị|ĩ)";
            pattern[5] = "y|(ý|ỳ|ỷ|ỵ|ỹ)";
            pattern[6] = "d|đ";
            //pattern[7] = "_|(/|!|@|#|$)";
            for (int i = 0; i < pattern.Length; i++)
            {
                // kí tự sẽ thay thế
                char replaceChar = pattern[i][0];
                MatchCollection matchs = Regex.Matches(text, pattern[i]);
                foreach (Match m in matchs)
                {
                    text = text.Replace(m.Value[0], replaceChar);
                }
            }
            text.Replace(" ", "").Replace("\"", "");
            return text;
        }
        public string GenerateBase64CaptchaImage(int width, int height, string captchaText)
        {
            //First declare a bitmap and declare graphic from this bitmap
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            //And create a rectangle to delegete this image graphic 
            Rectangle rect = new Rectangle(0, 0, width, height);
            //And create a brush to make some drawings
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.DottedGrid, Color.Aqua, Color.White);
            g.FillRectangle(hatchBrush, rect);

            //here we make the text configurations
            GraphicsPath graphicPath = new GraphicsPath();
            //add this string to image with the rectangle delegate
            graphicPath.AddString(captchaText, FontFamily.GenericMonospace, (int)FontStyle.Bold, 90, rect, null);
            //And the brush that you will write the text
            hatchBrush = new HatchBrush(HatchStyle.Percent20, Color.Black, Color.Black);
            g.FillPath(hatchBrush, graphicPath);
            //We are adding the dots to the image
            for (int i = 0; i < (int)(rect.Width * rect.Height / 50F); i++)
            {
                int x = rnd.Next(width);
                int y = rnd.Next(height);
                int w = rnd.Next(10);
                int h = rnd.Next(10);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }
            //Remove all of variables from the memory to save resource
            hatchBrush.Dispose();
            g.Dispose();
            //return the image to the related component

            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            var imgCaptcha = "data:image/png;base64," + Convert.ToBase64String(byteImage); // Get Base64
            return imgCaptcha;
        }
        public byte[] FileToByteArray(string filePath)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(filePath,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(filePath).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }
    }
}
