/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess
{
    /// <summary>
    /// Interface bao gồm các hàm cơ bản tương tác đến CSDL
    /// </summary>
    /// <typeparam name="T">model ánh xạ từ các bảng trong CSDL</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Xóa một bản ghi
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Thêm mới danh sách bản ghi
        /// </summary>
        /// <param name="entity"></param>
        void AddWithoutSave(T entity);
        
        /// <summary>
        /// Cập nhập bản ghi đã tồn tại
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Chuyển dữ liệu lên CSDL
        /// </summary>
        /// <returns></returns>
        /// 
        int Save();
    }
}
