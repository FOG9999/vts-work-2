using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class DAIBIEU
    {
        public decimal IDAIBIEU { get; set; }
        public Nullable<decimal> ITRUONGDOAN { get; set; }
        public string CTEN { get; set; }
        public Nullable<decimal> IDIAPHUONG { get; set; }
        public string CEMAIL { get; set; }
        public string CSDT { get; set; }
        public Nullable<decimal> IHIENTHI { get; set; }
        public decimal IDELETE { get; set; }
        public string CCODE { get; set; }
        public string CDONVIBAUCUSO { get; set; }
        public decimal IVITRI { get; set; }
        public decimal IGIOITINH { get; set; }
        public Nullable<System.DateTime> DNGAYSINH { get; set; }
        public string CDOANDB { get; set; }
        public string CNOILAMVIEC { get; set; }
        public string CCHUCVUDAYDU { get; set; }
        public string CCOQUAN { get; set; }
        public decimal ILOAIDAIBIEU { get; set; }
        public Nullable<decimal> ITOTRUONG { get; set; }
    }
}
