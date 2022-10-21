using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KN_CHUONGTRINH_CHITIET
    {
        public decimal ID { get; set; }
        public Nullable<decimal> ICHUONGTRINH { get; set; }
        public Nullable<decimal> ITODAIBIEU { get; set; }
        public Nullable<decimal> IDIAPHUONG { get; set; }
        public Nullable<decimal> IDIAPHUONG2 { get; set; }
        public string CDIACHI { get; set; }
        public DateTime DNGAYTIEP { get; set; }
    }
}
