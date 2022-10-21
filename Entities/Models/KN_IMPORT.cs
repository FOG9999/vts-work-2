using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class KN_IMPORT
    {
        public decimal ID { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public string CGHICHU { get; set; }
        public string CFILE { get; set; }
        public Nullable<decimal> IKYHOP { get; set; }
        public Nullable<decimal> ITRUOCKYHOP { get; set; }
        public Nullable<decimal> ISOKIENNGHI { get; set; }
        public Nullable<decimal> ITINHTRANG { get; set; }
    }
}
