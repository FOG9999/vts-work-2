using System.Collections.Generic;
using System.Web.Mvc;
using KienNghi.Models;

namespace  KienNghi.Helpers
{
    public static class DataHelper
    {
        public static SelectList ToSelectList(this IEnumerable<KeyViewModel> source)
        {
            return new SelectList(source, "Key", "Text");
        }
    }
}
