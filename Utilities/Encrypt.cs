/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public static class Encrypt
    {
        /// <summary>
        /// Mã hóa sha2 chuỗi string và convert chuỗi mã hóa sang kiểu hex 0 -> F
        /// </summary>
        /// <param name="input">Chuỗi cần mã hóa</param>
        /// <returns></returns>
        public static string Sha2HashWithHex(string input)
        {
            var hash = new SHA256Managed();
            Byte[] inputAsByte = Encoding.UTF8.GetBytes(input);
            Byte[] encrypte = hash.ComputeHash(inputAsByte);
            hash.Clear();
            hash.Dispose();
            string delimitedHexHash = BitConverter.ToString(encrypte);
            return delimitedHexHash.Replace("-", "").ToLower();
        }

        /// <summary>
        /// Mã hóa sha1 chuỗi string truyền vào và convert chuỗi mã hóa sang kiểu hex 0 -> F
        /// </summary>
        /// <param name="input">Chuỗi cần mã hóa</param>
        /// <returns></returns>
        public static string Sha1HashWithHex(string input)
        {
            var contentBytes = Encoding.UTF8.GetBytes(input);
            SHA1 sha1 = new SHA1Managed();
            byte[] hashValue = sha1.ComputeHash(contentBytes);
            string delimitedHexHash = BitConverter.ToString(hashValue);
            string vR = delimitedHexHash.Replace("-", "").ToLower();
            sha1.Dispose();
            return vR;
        }

        /// <summary>
        /// Mã hóa sha1 và convert dữ liệu sang kiểu base64
        /// </summary>
        /// <param name="input">Chuỗi cần mã hóa</param>
        /// <returns></returns>
        public static string Sha1Hash(string input)
        {
            var contentBytes = Encoding.UTF8.GetBytes(input);
            SHA1 sha1 = new SHA1Managed();
            byte[] hashValue = sha1.ComputeHash(contentBytes);
            string delimitedHexHash = ToBase64(hashValue);
            sha1.Dispose();
            return delimitedHexHash;
        }

        /// <summary>
        /// Chuyển byte array thành chuỗi string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToBase64(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return string.Empty;
            }
            return Convert.ToBase64String(data);
        }

        public static string DecodeBase64(string encodeString)
        {
            byte[] data = Convert.FromBase64String(encodeString);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        /// <summary>
        /// Chuyển chuỗi string sang kiểu bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


        private static readonly Random Rng = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        /// <summary>
        /// Tạo chuỗi ngẫu nhiên với độ dài truyền vào
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string RandomString(int size)
        {
            var buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = Chars[Rng.Next(Chars.Length)];
            }
            return new string(buffer);
        }
    }
}
