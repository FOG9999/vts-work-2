/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System;

namespace Utilities.Cache
{
    /// <summary>
    /// Class trực tiếp sử dụng để lưu cache
    /// </summary>
    public static class CacheExtensions
    {

        public static T Get<T>(this ICacheProvider cacheProvider, string key, Func<T> acquire)
        {
            return Get(cacheProvider, key, 60, acquire);
        }

        public static T Get<T>(this ICacheProvider cacheProvider, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheProvider.IsStored(key))
            {
                return cacheProvider.Get<T>(key);
            }
            else
            {
                var result = acquire();
                cacheProvider.Set(key, result, cacheTime);
                return result;
            }
        }
    }
}
