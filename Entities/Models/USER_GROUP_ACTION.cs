using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class USER_GROUP_ACTION
    {
        public decimal ID { get; set; }
        public Nullable<decimal> IGROUP { get; set; }
        public Nullable<decimal> IACTION { get; set; }
        public Nullable<decimal> IDELETE { get; set; }
    }
}
