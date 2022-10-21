/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

namespace Utilities.Paging
{
    /// <summary>
    /// Interface thông tin của 1 phân trang
    /// </summary>
    public interface IPagedList
    {
        /// <summary>
        /// Tổng số trang
        /// </summary>
        int PageCount { get; }
        
        /// <summary>
        /// Tổng số item của query
        /// </summary>
        int TotalItemCount { get; }
        
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        int PageIndex { get; }
        
        /// <summary>
        /// Số phần tử trên 1 trang
        /// </summary>
        int PageSize { get; }
    }
}
