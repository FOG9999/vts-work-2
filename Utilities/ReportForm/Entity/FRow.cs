using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.ReportForm.Entity
{
    /// <summary>
    /// A table row
    /// </summary>
    [Serializable]
    public class FRow
    {

        public FRow()
        {
            this.IsHeader = false;
        }

        /// <summary>
        /// Key of the row index row
        /// </summary>
        public int RowIndex { get; set; }

        public bool IsHeader { get; set; }

        #region Fields

        private List<FField> _cells; 
        #endregion

        #region Children
        /// <summary>
        /// Gets or sets the cells.
        /// </summary>
        /// <value>The cells.</value>
        public List<FField> Cells
        {
            get
            {
                return _cells ?? (_cells  = new List<FField>());
            }
            set { _cells = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToTrTag()
        {
            if (Cells.Any())
            {
                var result = new StringBuilder("<tr " + (this.IsHeader ? "class='GridHeader'" : "class='GridItem'") + ">");
                for (var i = 0; i < Cells.Count; i++)
                {
                    result.Append( this.IsHeader ? Cells[i].ToThTag(): Cells[i].ToTdTag());
                }
                result.Append("</tr>");
                return result.ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion

    }
}