using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class TIEPDAN_DINHKY
    {
        public decimal IDINHKY { get; set; }
        public Nullable<decimal> ILUOT { get; set; }
        public string CDIADIEM { get; set; }
        public Nullable<decimal> IVUVIEC { get; set; }
        public Nullable<decimal> IDOAN { get; set; }
        public Nullable<System.DateTime> DNGAYTIEP { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public Nullable<decimal> IUSER { get; set; }
    }
}
