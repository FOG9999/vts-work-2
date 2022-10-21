using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Utilities.Enums
{
    public static class EnumHelper<T> where T : struct
    {
        /// <summary>
        /// Lấy toàn bộ các enum - item của 1 enum để đưa vào list
        /// </summary>
        /// <returns></returns>
        public static List<T> GetValues()
        {
            //System.Diagnostics.Debug.Assert(typeof(T).IsEnum);
            var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
            return fields.Select(t => (T)t.GetValue(null)).ToList();
        }

        /// <summary>
        /// Lấy toàn bộ các enum - item và string mô tả của nó để đưa vào 1 list dạng key - value
        /// </summary>
        /// <returns></returns>
        public static List<KeyTextItem> ConvertToKeyValueList()
        {
            var list = GetValues();
            var result = new List<KeyTextItem>();
            if (null == list)
            {
                return result;
            }
            var stringEnum = new StringEnum(typeof(T));

            result.AddRange(list.Select(item => new KeyTextItem()
                                                    {
                                                        Id = item.ToString(),
                                                        Text = stringEnum.GetStringValue(item.ToString()) ?? item.ToString()
                                                    }));
            return result;
        }

        /// <summary>
        /// Lấy toàn bộ các enum - item và string mô tả của nó để đưa vào 1 list dạng key - value
        /// </summary>
        /// <returns></returns>
        public static List<KeyTextItem> GetListHashCode()
        {
            var list = GetValues();
            var result = new List<KeyTextItem>();
            if (null == list)
            {
                return result;
            }
            var stringEnum = new StringEnum(typeof(T));

            result.AddRange(list.Select(item => new KeyTextItem()
            {
                Id = item.GetHashCode().ToString(),
                Text = stringEnum.GetStringValue(item.ToString()) ?? item.ToString()
            }));
            return result;
        }

        public static List<SelectListItem> ConvertToSelectListItem(bool selectAll = false, string text = "-- Tất cả --")
        {
            var list = GetValues();
            var result = new List<SelectListItem>();
            if (null == list)
            {
                return result;
            }
            var stringEnum = new StringEnum(typeof(T));

            result.AddRange(list.Select(item => new SelectListItem()
            {
                // Lấy giá trị của Enum value
                Value = ((int)Enum.Parse(typeof(T), item.ToString())).ToString(),
                Text = stringEnum.GetStringValue(item.ToString()) ?? item.ToString()
            }));
            if (selectAll)
            {
                var item = new SelectListItem()
                {
                    Value = "-1",
                    Text = text
                };
                result.Insert(0, item);
            }
            return result;
        }

        public static T GetByValue(string value)
        {
            var field = GetValues();
            return field.FirstOrDefault(o => o.ToString().ToUpper() == value.ToUpper());
        }

        public static T GetByPosition(int position)
        {
            var field = GetValues();
            T t = field.FirstOrDefault(o => field.IndexOf(o) == position);
            return t;
        }

        /// <summary>
        /// Hàm lấy StringValue của Ennum
        /// </summary>
        /// <param name="val">Giá trị của Enum</param>
        /// Author: Hungcd1.
        public static string GetStringName(int val)
        {
            var item = Enum.GetName(typeof(T), val);
            var stringEnum = new StringEnum(typeof(T));
            return stringEnum.GetStringValue(item.ToString()) ?? item.ToString();
        }
    }
}
