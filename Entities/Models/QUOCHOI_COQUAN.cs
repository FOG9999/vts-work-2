using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class QUOCHOI_COQUAN
    {
        public decimal ICOQUAN { get; set; }
        public string CTEN { get; set; }
        public Nullable<decimal> IPARENT { get; set; }
        public string CCODE { get; set; }
        public Nullable<System.DateTime> DKETTHUC { get; set; }
        public Nullable<decimal> IMACDINH { get; set; }
        public Nullable<decimal> IDIAPHUONG { get; set; }
        public decimal IGROUP { get; set; }
        public decimal IVITRI { get; set; }
        public decimal IUSE { get; set; }
        public decimal IHIENTHI { get; set; }
        public Nullable<decimal> IDELETE { get; set; }
        public string CTYPE { get; set; }
    }
}
