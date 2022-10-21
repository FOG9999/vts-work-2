using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KN_CHUONGTRINH_DAIBIEU
    {
        public decimal ID { get; set; }
        public Nullable<decimal> ICHUONGTRINH { get; set; }
        public Nullable<decimal> IUSER_DAIBIEU { get; set; }
        public Nullable<decimal> IUSER_COQUAN { get; set; }
    }
}
