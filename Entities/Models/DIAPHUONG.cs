using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class DIAPHUONG
    {
        public decimal IDIAPHUONG { get; set; }
        public string CTEN { get; set; }
        public string CTYPE { get; set; }
        public Nullable<decimal> IPARENT { get; set; }
        public string CCODE { get; set; }
        public decimal IHIENTHI { get; set; }
        public Nullable<decimal> IDELETE { get; set; }
    }
}
