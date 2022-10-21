using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class QUOCHOI_KHOA
    {
        public decimal IKHOA { get; set; }
        public decimal ILOAI { get; set; }
        public string CTEN { get; set; }
        public Nullable<System.DateTime> DBATDAU { get; set; }
        public Nullable<System.DateTime> DKETTHUC { get; set; }
        public Nullable<decimal> IMACDINH { get; set; }
        public decimal IHIENTHI { get; set; }
        public Nullable<decimal> IDELETE { get; set; }
        public string CCODE { get; set; }
        public decimal IVITRI { get; set; }
    }
}
