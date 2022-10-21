/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System.Collections.Generic;

namespace Utilities.Paging
{
    public static class PagedListExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> list, int totalItemCount, int pageIndex, int pageSize)
        {
            return new PagedList<T>(list, totalItemCount, pageIndex, pageSize);
        }
    }
}
