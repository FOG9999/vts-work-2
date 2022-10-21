using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KN_VANBAN
    {
        public decimal IVANBAN { get; set; }
        public string CSOVANBAN { get; set; }
        public Nullable<System.DateTime> DNGAYBANHANH { get; set; }
        public DateTime DNGAYDUKIENHOANTHANH { get; set; }
        public string CNGUOIKY { get; set; }
        public string CNOIDUNG { get; set; }
        public string CFILE { get; set; }
        public Nullable<decimal> ICOQUANBANHANH { get; set; }
        public Nullable<decimal> ICOQUANNHAN { get; set; }
        public Nullable<decimal> IKIENNGHI { get; set; }
        public Nullable<decimal> ITONGHOP { get; set; }
        public string CLOAI { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public Nullable<System.DateTime> DNGAYBAOCAO { get; set; }
    }
}
