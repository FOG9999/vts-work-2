using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class TIEPDAN_THUONGXUYEN
    {
        public decimal ITHUONGXUYEN { get; set; }
        public Nullable<System.DateTime> DNGAYTIEP { get; set; }
        public string CDIADIEM { get; set; }
        public Nullable<decimal> ICOQUANTIEPDAN { get; set; }
        public string CNGUOITIEP { get; set; }
        public string CNGUOIGUI_TEN { get; set; }
        public string CNGUOIGUI_DIACHI { get; set; }
        public Nullable<decimal> IDOAN { get; set; }
        public Nullable<decimal> IDOAN_NGUOI { get; set; }
        public string CNOIDUNG { get; set; }
        public Nullable<decimal> ILOAI { get; set; }
        public Nullable<decimal> ILINHVUC { get; set; }
        public Nullable<decimal> INOIDUNG { get; set; }
        public Nullable<decimal> ITINHCHAT { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public string CCODE { get; set; }
        public Nullable<decimal> ITHUONGXUYEN_TRUNG { get; set; }
        public Nullable<decimal> IKIEMTRUNG { get; set; }
    }
}
