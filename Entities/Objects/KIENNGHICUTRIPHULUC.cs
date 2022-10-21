using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Objects
{
    /* báo cáo Kiên nghị cử tri, hội đồng nhân dân, danh mục cử tri 1A */
    public class KIENNGHICUTRI_1A
    {
        public string TT { get; set; }
        public string DIAPHUONG { get; set; }
        public string SOKIENNGHI { get; set; }
        public string NOIDUNGKIENNGHI { get; set; }

        public string COCHE_CHINHSACH { get; set; }

        public string KINHTE_NGANSACH { get; set; }
        public string VANHOA_XAHOI { get; set; }

        public string TUPHAP { get; set; }

        public string ANQP_KHAC { get; set; }
        public string GHICHU { get; set; }
        public decimal ILINHVUC { get; set; }

        public int ISBOLD { get; set; }
        public Boolean ISMERGE { get; set; }

        public int ISTITLE { get; set; }

    }

    public class KIENNGHICUTRI_1B1
    {
        public string TT { get; set; }
        public string DIAPHUONG { get; set; }

        public int TONGSOKIENNGHI { get; set; }
        public string TYLEKIENNGHI { get; set; }
        public string GHICHU { get; set; }
        public decimal ILINHVUC { get; set; }

        public int ISBOLD { get; set; }
        public Boolean ISMERGE { get; set; }

        public int ISTITLE { get; set; }

    }
}
