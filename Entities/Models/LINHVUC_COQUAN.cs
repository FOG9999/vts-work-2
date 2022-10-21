using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class LINHVUC_COQUAN
    {
        public  Nullable<decimal>   ILINHVUC { get; set; }
        public decimal ICOQUAN { get; set; }
        public string CTEN { get; set; }
        public decimal IHIENTHI { get; set; }
        public decimal IDELETE { get; set; }
        public string CCODE { get; set; }
        public decimal IVITRI { get; set; }
        public decimal IPARENT { get; set; }
        public string MALINHVUC { get; set; }

    }
}
