using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class LINHVUC
    {
        public decimal ILINHVUC { get; set; }
        public decimal IPARENT { get; set; }
        public string CTEN { get; set; }
        public string CCODE { get; set; }
        public decimal IHIENTHI { get; set; }
        public decimal IDELETE { get; set; }
        public decimal INHOM { get; set; }
        public decimal ILOAIDON { get; set; }
        public decimal IVITRI { get; set; }
    }
}
