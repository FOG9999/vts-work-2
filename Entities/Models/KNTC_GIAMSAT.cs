using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KNTC_GIAMSAT
    {
        public decimal IGIAMSAT { get; set; }
        public Nullable<decimal> IDON { get; set; }
        public Nullable<decimal> IDONVI { get; set; }
        public string CKEHOACH { get; set; }
        public string CCHUYENDE { get; set; }
        public string CNOIDUNG { get; set; }
        public string CFILE { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
    }
}
