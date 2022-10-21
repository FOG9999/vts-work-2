using System;

namespace Utilities.ReportForm.Entity
{
    /// <summary>
    /// A form field
    /// </summary>
    [Serializable]
    public class FField
    {
        public string FieldName { get; set; }

        public string FieldData{ get; set; }

        public int? ColumnSpan { get; set; }

        /// <summary>
        /// Thuộc tính lưu index field cần tính tổng
        /// </summary>
        public int SumIndex { get; set; }

        public int? RowSpan { get; set; }

        public double? Width { get; set; }

        public int Level { get; set; }

        public string ParentValue { get; set; }

        public string ToTdTag()
        {
            var cs = ColumnSpan.HasValue ? " colspan='" + ColumnSpan.Value.ToString() + "'" : "";
            var rs = RowSpan.HasValue ? " rowspan='" + RowSpan.Value.ToString() + "'" : "";
            var w = Width.HasValue ? " width='" + Width.Value.ToString() + "px'" : "" ;
            var pr = !string.IsNullOrEmpty(ParentValue) ? " alt='" + ParentValue+"'" : "";
            var cn = !string.IsNullOrEmpty(ClassName) ? " class='" + ClassName + "'" : "";
            return "<td" + cs + rs + w + pr + cn + ">" + FieldName + "</td>";
        }

        public string ToThTag()
        {
            var cs = ColumnSpan.HasValue ? " colspan='" + ColumnSpan.Value.ToString() + "'" : "";
            var rs = RowSpan.HasValue ? " rowspan='" + RowSpan.Value.ToString() + "'" : "";
            var w = Width.HasValue ? " width='" + Width.Value.ToString() + "px'" : "";
            var pr = !string.IsNullOrEmpty(ParentValue) ? " alt='" + ParentValue + "'" : "";
            var cn = !string.IsNullOrEmpty(ClassName) ? " class='" + ClassName + "'" : "";
            return "<th" + cs + rs + w + pr + cn + ">" + FieldName + "</th>";
        }

        #region "Thuộc tính dùng cho Export Excel"

        public int ColPosition { get; set; }

        public int RowPosition { get; set; }

        #endregion

        #region "Thuộc tính dùng cho gen Html"

        public string ClassName { get; set; }

        #endregion
    }
}