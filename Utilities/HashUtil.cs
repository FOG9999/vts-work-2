using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using Entities.Models;
using Utilities;
namespace Utilities
{
    public static class HashUtil
    {
        public static string HashString(string inputString, string hashName)
        {
            var algorithm = HashAlgorithm.Create(hashName);
            if (algorithm == null)
            {
                throw new ArgumentException("Unrecognized hash name", "hashName");
            }
            var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            return Convert.ToBase64String(hash);
            //return Encoding.UTF8.GetString(hash);
        }

        public static string GetHash(string text, HashType hashType)
        {
            HashAlgorithm algorithm;
            switch (hashType)
            {
                
                case HashType.SHA256:
                    algorithm = SHA256.Create();
                    break;
                case HashType.SHA512:
                    algorithm = SHA512.Create();
                    break;
                default:
                    throw new ArgumentException("Invalid hash type", "hashType");
            }
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            byte[] hash = algorithm.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
        public enum HashType : int
        {
            
            SHA256,
            SHA512
        }
        public static string Encode_Action(int iUser,string controller,int id_action)
        {
            //byte[] encoded = UTF8Encoding.UTF8.GetBytes(iUser + "|" + controller+"|"+ key_url);
            string str_encrt = iUser + "|" + controller+"|"+ id_action;
            return HashUtil.Encrypt(str_encrt, Convert.ToBoolean(HashUtil.HashType.SHA512));
        }
        //public static TokenAction Decode_Action(string token)
        //{
        //    TokenAction t= new TokenAction();
        //    //byte[] encoded = HttpServerUtility.UrlTokenDecode(token);
        //    string decode = HashUtil.Decrypt(token, Convert.ToBoolean(HashUtil.HashType.SHA512));
        //    string[] decode_ = decode.Split('|');
        //    t.iUser = Convert.ToInt32(decode_[0]);
        //    t.controller = decode_[1];
        //    t.id_action = Convert.ToInt32(decode_[2]);
            
        //    //t.key_url = decode_[2];
        //    return t;
        //}
        public static string Encode_ID(string encodeMe,string key_cookie="")
        {
            //tạo key cookie ngẫu nhiên cho mỗi phiên mã hóa
            //string key_cookie = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 4).ToLower(); 

            //HttpCookie cookieset = new HttpCookie("url_key");
            //cookieset.Value = key_cookie;
            //cookieset.Expires = DateTime.Now.AddHours(1);
            //HttpContext.Current.Response.Cookies.Add(cookieset);
            key_cookie = AppConfig.key;
            byte[] encoded = UTF8Encoding.UTF8.GetBytes(encodeMe + "|" + key_cookie);
            return HttpServerUtility.UrlTokenEncode(encoded);
        }

        public static string Decode_ID(string decodeMe, string key_cookie_url="")
        {
            key_cookie_url = AppConfig.key;
            byte[] encoded = HttpServerUtility.UrlTokenDecode(decodeMe);
            string decode= UTF8Encoding.UTF8.GetString(encoded);
            string id = decode.Split('|')[0];
            string key_cookie= decode.Split('|')[1];
            if (!key_cookie.Equals(key_cookie_url))
            {
                id = "0";
            }
            return id;
        }
        public static string Encrypt_ID(int id)
        {
            
            return Encrypt(id.ToString()+"|"+ AppConfig.key, Convert.ToBoolean(HashType.SHA512));
        }
        public static int Decrypt_ID(string str)
        {
            string dec = Decrypt(str, Convert.ToBoolean(HashType.SHA512));
            int id = Convert.ToInt32(dec.Split('|')[0]);
            return id;
        }
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file

            
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(AppConfig.key));
                //Always release the resources and flush data
                //of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
                hashmd5.Dispose();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(AppConfig.key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            tdes.Dispose();
            
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            //string key = "kntc"; //(string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(AppConfig.key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
                hashmd5.Dispose();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(AppConfig.key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            tdes.Dispose();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}