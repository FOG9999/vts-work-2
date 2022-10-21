using System;
using System.Data;

namespace Utilities.ReportForm.Entity
{
    /// <summary>
    /// A form
    /// </summary>
    [Serializable]
    public class FReport
    {
        private DataTable _dataContent;
        private TreeItem _treeItem;
        public FReport()
        {
            this.HeaderCommon = new FHeader();
        }

        #region Children

        public string SheetName { get; set; }

        public int TotalLevel { get; set; }

        public string FileName { get; set; }

        public FHeader HeaderCommon { get; set; }

        public DataTable DataContent
        {
            get
            {
                return _dataContent ?? new DataTable();
            }
            set { _dataContent = value; }
        }

        public TreeItem HeaderItem {
            get { return _treeItem ?? new TreeItem(); } 
            set {_treeItem = value;} 
        }       

        public FTable TableContent { get; set; }

        #endregion

        #region Attribute By Tony
        /// <summary>
        /// Date begin search
        /// </summary>
        public DateTime DateFrom { get; set; }
        /// <summary>
        /// Date finish search
        /// </summary>
        public DateTime DateTo { get; set; }
        /// <summary>
        /// current Date search
        /// </summary>
        public DateTime CurrentDate { get; set; }
        #endregion

    }
}