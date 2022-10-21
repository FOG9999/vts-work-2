/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System;
using System.Linq;

namespace Utilities.Paging
{

    /// <summary>
    /// Hàm phân trang    
    /// </summary>
    public static class PageQuery
    {
        /// <summary>
        /// IQueryable.Paginate();
        /// </summary>
        /// <typeparam name="T">model</typeparam>
        /// <param name="query">Câu truy vấn, chú ý order trước khi gọi phân trang</param>
        /// <param name="pageIndex">trang muốn lấy</param>
        /// <param name="pageSize">số phần tử trên 1 trang</param>
        /// <param name="totalItemCount">tổng số phần tử, truyền hoặc ko truyền</param>
        /// <returns></returns>
        public static PagedList<T> Paginate<T>(this IQueryable<T> query, int pageIndex, int pageSize, int totalItemCount = 0){
            if (totalItemCount == 0)
            {
                totalItemCount = query.Count();
            }
            return query.Skip(Math.Max(pageSize * pageIndex, 0)).Take(pageSize).ToPagedList(totalItemCount, pageIndex, pageSize);
        }
    }
}
