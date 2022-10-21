/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

namespace Utilities.Cache
{
    /// <summary>
    /// Interface chứa các hàm cơ bản để thao tác với cache
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        void Set(string key, object data, int cacheTime = 60);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsStored(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 
        /// </summary>
        void Clear();

        /// <summary>
        /// Xóa bỏ cache có chứa term truyền vào
        /// </summary>
        /// <param name="term"></param>
        void RemoveByTerm(string term);
    }
}
