using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class ACTION
    {
        public decimal IACTION { get; set; }
        public string CTEN { get; set; }
        public Nullable<decimal> IPARENT { get; set; }
        public Nullable<decimal> IVITRI { get; set; }
    }
}
