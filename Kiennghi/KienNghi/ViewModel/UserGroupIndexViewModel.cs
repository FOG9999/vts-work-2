using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KienNghi.ViewModel
{
    public class UserGroupIndexViewModel : ViewModelBase
    {
        public decimal IGROUP { get; set; }
        public string CTEN { get; set; }
        public string CMOTA { get; set; }
        public Nullable<decimal> IDELETE { get; set; }
        public Dictionary<string, List<ActionViewModel>> ActionListGroup { get; set; }
    }
}
