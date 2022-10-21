using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KN_CHUONGTRINH
    {
        public decimal ICHUONGTRINH { get; set; }
        public Nullable<decimal> IKYHOP { get; set; }
        public Nullable<decimal> IKHOA { get; set; }
        public Nullable<decimal> IDOITUONG { get; set; }
        public Nullable<decimal> ITRUOCKYHOP { get; set; }
        public Nullable<System.DateTime> DBATDAU { get; set; }
        public Nullable<System.DateTime> DKETTHUC { get; set; }
        public string CKEHOACH { get; set; }
        public string CNOIDUNG { get; set; }
        public string CDIACHI { get; set; }
        public string CFILE { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public Nullable<decimal> IDONVI { get; set; }
        public Nullable<System.DateTime> DNGAYBANHANH { get; set; }
    }
}
