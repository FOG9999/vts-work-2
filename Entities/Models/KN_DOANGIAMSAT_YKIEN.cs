using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KN_DOANGIAMSAT_YKIEN
    {
        public decimal IYKIEN { get; set; }
        public string CTEN { get; set; }
        public Nullable<decimal> IDOANGIAMSAT { get; set; }
        public string CNOIDUNG { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public Nullable<System.DateTime> DNGAYLAMVIEC { get; set; }
        public Nullable<decimal> IKIENNGHI { get; set; }
    }
}
