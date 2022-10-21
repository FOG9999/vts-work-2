using System;
using System.Collections.Generic;

namespace Utilities.ReportForm.Entity
{
    /// <summary>
    /// FForm utilities
    /// </summary>
    [Serializable]
    public class TreeItem
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string ParentName { get; set; }
        public string ParentPath { get; set; }
        private List<TreeItem> _children;
        public List<TreeItem> Children
        {
            get
            {
                return _children ?? (_children = new List<TreeItem>());
            }
            set { _children = value; }
        }
        public TreeItem()
        {
        }
        public TreeItem(string val, int level, string parentName = "", List<TreeItem> children = null)
        {
            this.Level = level;
            this.Name = val;
            this.Children = children;
            this.ParentName = parentName;
        }

        /// <summary>
        /// Đặt giá trị Level cho các phần từ và lấy về giá trị lớn nhất
        /// </summary>
        /// <param name="tree"> Cây gốc truyền vào để Set</param>
        /// <param name="level">Level bắt đầu của cây gốc, giá trị truyền vào =0 </param>
        /// <param name="maxLevel"> Level cao nhất được trả về, giá trị truyền = 0</param>
        public void SetLevel(TreeItem tree, int level, ref int maxLevel)
        {
            if (tree == null)
                return;

            tree.Level = level;
            if (maxLevel < level)
                maxLevel = level;
            if (tree.Children != null)
                for (int i = 0; i < tree.Children.Count; i++)
                    SetLevel(tree.Children[i], level + 1, ref maxLevel);
        }

    }
}