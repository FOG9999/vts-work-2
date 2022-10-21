using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataAccess
{
    /// <summary>
    /// Class mở rộng của DBContext
    /// Hỗ trợ lấy tên bảng,
    /// </summary>
    public static class ContextExtensions
    {
        /// <summary>
        /// Lấy tên bảng từ đối tượng T truyền vào
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableName<T>();
        }

        /// <summary>
        /// Lấy tên khóa chính từ đối tượng T truyền vào
        /// </summary>
        /// <typeparam name="T">T class mapping từ CSDL</typeparam>
        /// <param name="context">context</param>
        /// <returns>Trả về tên khóa chính của bảng đang ánh xạ đến đối tượng T trong csdl</returns>
        public static string GetTableKeyName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableKeyName<T>();
        }

        public static string GetTableKeyName<T>(this ObjectContext context) where T : class
        {
            ObjectSet<T> set = context.CreateObjectSet<T>();
            return set.EntitySet.ElementType.KeyMembers.Select(k => k.Name).FirstOrDefault();
        }

        /// <summary>
        /// Lấy tên bảng đang ánh xạ đến đối tượng T
        /// </summary>
        /// <typeparam name="T">Đối tượng T</typeparam>
        /// <param name="context"></param>
        /// <returns>Trả về tên bảng đang ánh xạ đến đối tượng T trong csdl</returns>
        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }
}
