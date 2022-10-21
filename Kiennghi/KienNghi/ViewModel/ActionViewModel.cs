using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KienNghi.ViewModel
{
    public class ActionViewModel : ViewModelBase
    {
        public decimal IACTION { get; set; }
        public string CTEN { get; set; }
        public Nullable<decimal> IPARENT { get; set; }
        public Nullable<decimal> IVITRI { get; set; }
        public string ActionParentName { get; set; }
    }
}
