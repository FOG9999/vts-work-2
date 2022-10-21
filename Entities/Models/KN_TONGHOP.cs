using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KN_TONGHOP
    {
        public decimal ITONGHOP { get; set; }
        public Nullable<decimal> IDONVITONGHOP { get; set; }
        public Nullable<decimal> IKYHOP { get; set; }
        public Nullable<decimal> ITRUOCKYHOP { get; set; }
        public Nullable<decimal> ICHUONGTRINH { get; set; }
        public Nullable<decimal> ITHAMQUYENDONVI { get; set; }
        public Nullable<decimal> ILINHVUC { get; set; }
        public string CNOIDUNG { get; set; }
        public string CTUKHOA { get; set; }
        public string CFILE { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public Nullable<decimal> ITINHTRANG { get; set; }
        public Nullable<decimal> ID_IMPORT { get; set; }
        public string CMATONGHOP { get; set; }
    }
}
