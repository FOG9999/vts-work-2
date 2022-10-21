/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System;
using System.Collections.Generic;

namespace Utilities.Paging
{
    /// <summary>
    /// 
    /// </summary>
    public class PagedList<T> : List<T>, IPagedList
    {
        public int PageCount
        {
            get;
            protected set;
        }

        public int TotalItemCount
        {
            get;
            protected set;
        }

        public int PageIndex
        {
            get;
            protected set;
        }

        public int PageSize
        {
            get;
            protected set;
        }

        public bool IsFirstPage
        {
            get { return this.PageIndex < 1; }
        }

        public bool IsLastPage
        {
            get { return this.PageIndex + 1 == PageCount; }
        }

        public int GetNumberOfPages(int totalItemCount, int pageSize)
        {
            return (totalItemCount % pageSize == 0) ? (totalItemCount / pageSize) : (totalItemCount / pageSize + 1);
        }

        /// <summary>
        /// Tạo object chứa thông tin phân trang
        /// Kết quả thực thi của hàm phân trang
        /// </summary>
        /// <param name="list"></param>
        /// <param name="totalItemCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagedList(IEnumerable<T> list, int totalItemCount, int pageIndex, int pageSize)
        :base(list ?? new List<T>()){
            if (pageIndex < 0 || pageSize < 1) {
                throw new ArgumentOutOfRangeException(@"PageIndex >= 0, PageSize >=1");
            }
            this.PageCount = GetNumberOfPages(totalItemCount, pageSize);
            this.TotalItemCount = totalItemCount;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// Default ctor
        /// </summary>
        public PagedList() : this(new List<T>(), 0, 0, 10) { }
    }
}
