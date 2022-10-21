using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class ListHelper
    {
        /// <summary>
        /// Kiểm tra xem tất cả các phần tử của b có nằm trong a hay không?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool ContainsAllItems<T>(List<decimal> a, List<decimal> b)
        {
            return !b.Except(a).Any();
        }
    }
}
