using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KNTC_NGUONDON
    {
        public decimal INGUONDON { get; set; }
        public string CTEN { get; set; }
        public string CCODE { get; set; }
        public decimal IHIENTHI { get; set; }
        public Nullable<decimal> IDELETE { get; set; }
        public decimal INHOMDONVI { get; set; }
        public decimal IPARENT { get; set; }
        public decimal IVITRI { get; set; }
        public decimal ILOAI { get; set; }
    
    }
}
