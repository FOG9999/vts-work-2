using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class TRACKING
    {
        public decimal ID { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public string CACTION { get; set; }
        public Nullable<decimal> IDON { get; set; }
        public Nullable<decimal> IKIENNGHI { get; set; }
        public decimal ITONGHOP { get; set; }
        public decimal ITIEPDAN_DINHKY { get; set; }
        public decimal ITIEPDAN_THUONGXUYEN { get; set; }
        public Nullable<decimal> IVANBAN { get; set; }
    }
}
