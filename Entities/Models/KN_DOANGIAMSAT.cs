using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KN_DOANGIAMSAT
    {
        public decimal IDOAN { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<decimal> IKYHOP { get; set; }
        public Nullable<decimal> IDONVI { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public string CTEN { get; set; }
        public string CNOIDUNG { get; set; }
        public Nullable<System.DateTime> DNGAYBATDAU { get; set; }
    }
}
