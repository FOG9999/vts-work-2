using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KN_KIENNGHI_TRALOI
    {
        public decimal ITRALOI { get; set; }
        public Nullable<decimal> IKIENNGHI { get; set; }
        public string CTRALOI { get; set; }
        public string GHICHU_KQ { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<decimal> IPHANLOAI { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public string CFILE { get; set; }
        public string CLOTRINH { get; set; }
        public Nullable<System.DateTime> DNGAY_DUKIEN { get; set; }
        public Nullable<decimal> ITINHTRANG { get; set; }
        public string CSOVANBAN { get; set; }
        public Nullable<System.DateTime> DNGAYBANHANH { get; set; }
        public string CNGUOIKY { get; set; }
        public string CCOQUANTRALOI { get; set; }
        public Nullable<decimal> ICOQUANTRALOI { get; set; }
    }
}
