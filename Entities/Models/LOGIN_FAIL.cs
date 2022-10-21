using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class LOGIN_FAIL
    {
        public decimal ID { get; set; }
        public string IP { get; set; }
        public decimal IFAILED { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
    }
}
