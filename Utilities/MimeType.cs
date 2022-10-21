using System.Collections.Generic;

namespace Utilities
{
    public static class MimeType
    {
        /// <summary>
        /// http://www.freeformatter.com/mime-types-list.html
        /// </summary>
        private static Dictionary<string, string> mime = new Dictionary<string, string>()
                                                             {
                                                                 {".pdf","application/pdf"},
                                                                 {".doc","application/msword"},
                                                                 {".docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                                                                 {".ppt","application/vnd.ms-powerpoint"},
                                                                 {".pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                                                                 {".xls","application/vnd.ms-excel"},
                                                                 {".xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                                                                 {".txt","text/plain"},
                                                                 {".xml","application/xml"},
                                                                 {".jpg","image/jpeg"},
                                                                 {".jpeg","image/jpeg"},
                                                                 {".png","image/png"},
                                                                 {".bmp","image/bmp"}
                                                             };

        public const string DefaultMimeType = @"application/octet-stream";

        /// <summary>
        /// Lấy ra mime type của file đó để trả về client
        /// </summary>
        /// <param name="fileExtension">phẩn mở rộng của tệp cần lấy mime-type</param>
        /// <returns></returns>
        public static string GetMime(string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                return string.Empty;
            }
            if (!fileExtension.StartsWith("."))
            {
                fileExtension = string.Format(".{0}", fileExtension);
            }
            fileExtension = fileExtension.ToLower();
            if (mime.ContainsKey(fileExtension))
            {
                return mime[fileExtension];
            }
            return DefaultMimeType;
        }
    }
}
