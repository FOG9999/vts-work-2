using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class NGHENGHIEP
    {
        public decimal INGHENGHIEP { get; set; }
        public string CTEN { get; set; }
        public Nullable<decimal> IHIENTHI { get; set; }
        public decimal IDELETE { get; set; }
        public string CCODE { get; set; }
        public decimal IVITRI { get; set; }
    }
}
