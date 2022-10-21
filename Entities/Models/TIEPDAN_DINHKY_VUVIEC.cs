using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class TIEPDAN_DINHKY_VUVIEC
    {
        public decimal IVUVIEC { get; set; }
        public Nullable<decimal> IDINHKY { get; set; }
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
        public string CTRALOI { get; set; }
    }
}
