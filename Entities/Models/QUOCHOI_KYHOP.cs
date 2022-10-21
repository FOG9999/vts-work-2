using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class QUOCHOI_KYHOP
    {
        public decimal IKYHOP { get; set; }
        public Nullable<decimal> IKHOA { get; set; }
        public string CTEN { get; set; }
        public Nullable<System.DateTime> DBATDAU { get; set; }
        public Nullable<System.DateTime> DKETTHUC { get; set; }
        public Nullable<decimal> IMACDINH { get; set; }
        public decimal IHIENTHI { get; set; }
        public decimal IDELETE { get; set; }
        public string  CCODE { get; set; }
        public decimal IVITRI { get; set; }
    }
}
