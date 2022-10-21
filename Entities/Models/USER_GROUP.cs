using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class USER_GROUP
    {
        public decimal IGROUP { get; set; }
        public string CTEN { get; set; }
        public string CMOTA { get; set; }
        public Nullable<decimal> IDELETE { get; set; }
    }
}
