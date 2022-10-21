using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class USER_PHONGBAN
    {
        public decimal IPHONGBAN { get; set; }
        public Nullable<decimal> IDONVI { get; set; }
        public string CTEN { get; set; }
        public Nullable<decimal> IVITRI { get; set; }
        public decimal IHIENTHI { get; set; }
        public decimal IDELETE { get; set; }
        public decimal IPARENT { get; set; }
    }
}
