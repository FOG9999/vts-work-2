using System.Web.Mvc;

namespace  KienNghi.Models
{
    public class ChecklistModel
    {


        public string Id { get; set; }

        public SelectList List { get; set; }

        public ChecklistModel(string id, SelectList list)
        {
            Id = id;
            List = list;
        }
        private ChecklistModel()
        {
            List = new SelectList("");
        }

        public static ChecklistModel Default
        {
            get { return new ChecklistModel(); }
        }
    }
}
