using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class USER
    {
        public decimal IUSER { get; set; }
        public string CUSERNAME { get; set; }
        public string CPASSWORD { get; set; }
        public string CTEN { get; set; }
        public Nullable<decimal> IPHONGBAN { get; set; }
        public string CEMAIL { get; set; }
        public string CSDT { get; set; }
        public Nullable<decimal> ISTATUS { get; set; }
        public Nullable<decimal> IDONVI { get; set; }
        public Nullable<decimal> ICHUCVU { get; set; }
        public string CARRGROUP { get; set; }
        public Nullable<decimal> ITYPE { get; set; }
        public string CSALT { get; set; }
        public Nullable<System.DateTime> DLASTCHANGEPASS { get; set; }
        public string CAUTHTOKEN { get; set; }
        public decimal ILOGINFAIL { get; set; }
    }
}
