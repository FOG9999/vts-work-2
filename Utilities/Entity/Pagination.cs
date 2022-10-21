using System;

namespace Utilities.Entity
{
    [Serializable]
    public class Pagination
    {
        public Pagination()
        {
            // set data default for attribute
            this.MaxPages = 10;
            this.PageSize = 10;
            this.CurrentPage = 0;
            this.TotalRows = 10;

        }
        /// <summary>
        /// Number page bar
        /// </summary>
        public int MaxPages { get; set; }

        public int TotalRows { get; set; }
        /// <summary>
        /// Total pages
        /// </summary
        public int TotalPages
        {
            get
            {
                var count = this.TotalRows / this.PageSize;
                if (this.TotalRows - this.PageSize * count == 0)
                    return count;
                else
                    return count + 1;
            }
        }

        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage { get; set; }

        public int CurrentRow { get; set; }
        /// <summary>
        /// Number recorde per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Mode name of MVC
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Action name of Model (Method in Model)
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Id of input control search to get value search
        /// </summary>
        private string _inputSearchId;

        public string InputSearchId
        {
            get
            {
                if (string.IsNullOrEmpty(this._inputSearchId))
                    return string.Empty;

                if (this._inputSearchId[0].Equals('#'))
                    return this._inputSearchId;
                else
                    return "#" + this._inputSearchId;
            }

            set { _inputSearchId = value; }
        }

        private string _tagetReLoadId;

        /// <summary>
        /// Id div or region to reload ajax
        /// </summary>
        public string TagetReLoadId
        {
            get
            {
                if (string.IsNullOrEmpty(this._tagetReLoadId))
                    return string.Empty;

                if (this._tagetReLoadId[0].Equals('#'))
                    return this._tagetReLoadId;
                else
                    return "#" + this._tagetReLoadId;
            }
            set { _tagetReLoadId = value; }
        }

        /// <summary>
        /// Category type 
        /// </summary>
        public int CtgType { get; set; }
    }
}
