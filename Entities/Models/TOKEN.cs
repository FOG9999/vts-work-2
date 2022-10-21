using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class TOKEN
    {
        public decimal ID { get; set; }
        public string CONTROLLER { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<System.DateTime> DTIME { get; set; }
        public string TOKENACTION { get; set; }
        public string CSALT { get; set; }
    }
}
