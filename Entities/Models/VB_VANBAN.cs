using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class VB_VANBAN
    {
        public decimal IVANBAN { get; set; }
        public string CTIEUDE { get; set; }
        public string CTRICHYEU { get; set; }
        public Nullable<decimal> IHIENTHI { get; set; }
        public Nullable<decimal> ILINHVUC { get; set; }
        public Nullable<decimal> ILOAI { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<System.DateTime> DDATECREATE { get; set; }
        public Nullable<decimal> IUSERDUYET { get; set; }
        public decimal IKYHOP { get; set; }
    }
    
}
