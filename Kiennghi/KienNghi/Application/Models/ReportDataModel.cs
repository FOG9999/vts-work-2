using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace  KienNghi.Models
{
    public class ReportDataModel
    {
        public ReportDataModel()
        {
            arrMoTa1 = new ArrayList();
            arrMoTa2 = new ArrayList();
            arrMoTa3 = new ArrayList();

            Values = new Dictionary<string, object>();
        }

        public DataTable dtDuLieu { get; set; }
        public DataTable dtDuLieuAll { get; set; }

        public DataTable dtGhiChu { get; set; }

        public ArrayList arrMoTa1 { get; set; }
        public ArrayList arrMoTa2 { get; set; }
        public ArrayList arrMoTa3 { get; set; }

        public int ColumnsCount { get; set; }
        public double Sum { get; set; }
        public Dictionary<string, object> Values { get; private set; }
    }
}
