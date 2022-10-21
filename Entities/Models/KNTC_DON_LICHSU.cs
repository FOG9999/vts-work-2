using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KNTC_DON_LICHSU
    {
        public decimal ID { get; set; }
        public Nullable<decimal> IDON { get; set; }
        public Nullable<decimal> IDONVIGUI { get; set; }
        public Nullable<decimal> IDONVITIEPNHAN { get; set; }
        public Nullable<decimal> IDONVIXULY { get; set; }
        public DateTime DNGAYCHUYEN { get; set; }
        public decimal IUSER { get; set; }
        public decimal ITRANGTHAI { get; set; }
        public string CNOIDUNG { get; set; }
        public decimal ICHUYENXULY { get; set; }
        public decimal IVANBAN { get; set; }
    }
}
