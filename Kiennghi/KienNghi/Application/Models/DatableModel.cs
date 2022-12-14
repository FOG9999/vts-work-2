using System.Data;
using System.Web.Mvc;

namespace  KienNghi.Models
{
    public class CheckListDataTableModel
    {
        public CheckListDataTableModel()
        {

        }

        public CheckListDataTableModel(string id, DataTable dt, string selectedItems = null)
        {
            Id = id;
            Data = dt;
            SelectedItems = selectedItems;
        }

        public string Id { get; set; }

        public string GroupId { get; set; }
        public DataTable Data { get; set; }
        public string SelectedItems { get; set; }
    }
}
