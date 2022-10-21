using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.ReportForm.Entity
{
    /// <summary>
    /// A form table
    /// </summary>
    [Serializable]
    public class FTable
    {
        #region Fields

        private List<FRow> _rows;

        #endregion

        #region Children
        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public List<FField> Columns
        {
            get
            {
                var result = new List<FField>();
                if (Rows.Any())
                {
                    foreach (var row in Rows)
                    {
                        result.AddRange(row.Cells);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public List<FRow> Rows
        {
            get { return _rows ?? (_rows = new List<FRow>()); }
        } 
        #endregion

        /// <summary>
        /// Make a new row.
        /// </summary>
        /// <returns></returns>
        public FRow NewRow()
        {
            return new FRow
            {
                Cells = Columns
                        .Select(k => new FField
                        {
                            FieldName = k.FieldName,
                            FieldData = k.FieldData,
                            ColumnSpan = k.ColumnSpan,
                            RowSpan = k.RowSpan,
                            Level = k.Level,
                            ParentValue = k.ParentValue
                        })
                        .ToList()
            };
        }

        public string HtmlTable { get; set; }
    }
}