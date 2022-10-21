using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class KN_KIENNGHI_IMPORT
    {
        public decimal ID { get; set; }
        public Nullable<decimal> IDONVITIEPNHAN { get; set; }
        public Nullable<System.DateTime> DNGAYBANHANH { get; set; }
        public string CNOIDUNG { get; set; }
        public string CCONGVAN { get; set; }
        public string CSOCONGVAN { get; set; }
        public Nullable<decimal> IDONVITHAMQUYEN { get; set; }
        public Nullable<decimal> ILINHVUC { get; set; }
        public Nullable<decimal> ID_KIENNGHI { get; set; }
        public Nullable<decimal> ID_TONGHOP_BDN { get; set; }
        public Nullable<decimal> ID_IMPORT { get; set; }
        public Nullable<decimal> IKYHOP { get; set; }
        public Nullable<decimal> ITRUOCKYHOP { get; set; }
        public Nullable<decimal> ICHUONGTRINH { get; set; }
    }
}
