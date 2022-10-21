using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Utilities.ReportForm.Entity;

namespace Utilities.ReportForm
{
    /// <summary>
    /// Các hàm tiện ích xử lý thông tin báo cáo tổng hợp
    /// </summary>
    public static class FUtils
    {
        #region ATTRIBUTE
        private const string _htmlTableBegin = "<div class='table_kqtk fll invibox'><table class='Grid' width='100%' cellspacing='0' cellpadding='3' border='1'><tbody>";
        private const string _htmlTableEnd = "</tbody></table></div>";
        #endregion

        #region "Convert To Html"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeHeader"></param>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        public static string GetHtmlHeaderTable(TreeItem tree, bool? setWidthAuto = null, int? positionGroup=null, string beginTag = "", string endTag ="")
        {
            var maxLevel = GetLevelMax(tree) - 1;
            var ffields = ConvertToFField(tree, maxLevel).Where(t => t.Level != 0);
            return ffields.Any() ? GetHtmlHeaderTable(ffields, maxLevel, setWidthAuto,positionGroup, beginTag, endTag) : "";
        }

        public static string GetHtmlHeaderTable(IEnumerable<FField> ffields, int maxLevel, bool? setWidthAuto = null, int? positionGroup = null, string beginTag = "", string endTag = "")
        {
            var htmlTable = new StringBuilder(string.IsNullOrEmpty(beginTag) ? "" : beginTag);
            if (setWidthAuto.HasValue)
            {
                if (setWidthAuto.Value)
                {
                    SetWidthCols(ffields);
                }
            }
            else
            {
                SetWidthCols(ffields);
            }
            for (var i = 1; i <= maxLevel; i++)
            {
                var row = new FRow();
                row.IsHeader = true;
                row.Cells = ffields.Where(t => t.Level == i).ToList();
                if (positionGroup.HasValue)
                {
                    row.Cells[positionGroup.Value].ColumnSpan = 2;
                }
                htmlTable.Append(row.ToTrTag());
            }
            htmlTable.Append(string.IsNullOrEmpty(endTag) ? "" : endTag);

            return htmlTable.ToString();
        }

        /// <summary>
        /// Hàm convert từ đối tượng FReport sang dạng chuỗi Html Table
        /// </summary>
        /// <param name="obj">Đối tượng FReport</param>
        /// <returns></returns>
        public static string GetHtmlTable(FReport obj)
        {
            return GetHtmlTable(obj.HeaderItem, obj.DataContent);
        }

        /// <summary>
        /// Hàm convert sang dạng chuỗi Html Table
        /// </summary>
        /// <param name="treeHeader">Đối tượng TreeItem(Header Table)</param>
        /// <param name="dtContent">DataTable content</param>
        /// <returns></returns>
        public static string GetHtmlTable(TreeItem treeHeader, DataTable dtContent)
        {
            var maxLevel = GetLevelMax(treeHeader)-1;
            var ffields = ConvertToFField(treeHeader,maxLevel).Where(t => t.Level != 0);
            if (ffields.Any())
            {
                var htmlTable = new StringBuilder(_htmlTableBegin);
                //Fill header
                htmlTable.Append(GetHtmlHeaderTable(ffields, maxLevel));

                var rowStt = new FRow();
                rowStt.IsHeader = true;
                for (var i = 0; i < dtContent.Columns.Count; i++)
                {
                    rowStt.Cells.Add(new FField { FieldName = "(" + (i + 1).ToString() + ")", FieldData = "(" + (i + 1).ToString() + ")" });
                }
                htmlTable.Append(rowStt.ToTrTag());

                for (var i = 0; i < dtContent.Rows.Count; i++)
                {
                    var row = new FRow();
                    for(var j=0;j<dtContent.Columns.Count;j++)
                    {
                        row.Cells.Add(new FField
                        {
                            FieldName = dtContent.Rows[i][j].ToString(),
                            FieldData = dtContent.Rows[i][j].ToString(),
                            ParentValue = dtContent.Columns[j].ColumnName
                        });
                    }

                    if (i == dtContent.Rows.Count - 1)
                    {
                        row.Cells.RemoveAt(0);
                        row.Cells[0].ColumnSpan = 2;
                        row.Cells[0].FieldName = "Tổng số";
                    }

                    htmlTable.Append(row.ToTrTag());
                }

                htmlTable.Append(_htmlTableEnd);

                return htmlTable.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Hàm convert sang dạng chuỗi Html Table
        /// </summary>
        /// <param name="treeHeader"></param>
        /// <param name="dtContent"></param>
        /// <param name="positionHeaderGroup"></param>
        /// <param name="rowGroup"></param>
        /// <returns></returns>
        public static string GetHtmlTable(TreeItem treeHeader, DataTable dtContent, int positionHeaderGroup,List<string> rowGroup)
        {
            var maxLevel = GetLevelMax(treeHeader) - 1;
            var ffields = ConvertToFField(treeHeader, maxLevel).Where(t => t.Level != 0);
            if (ffields.Any())
            {
                var htmlTable = new StringBuilder(_htmlTableBegin);
                //Fill header
                htmlTable.Append(GetHtmlHeaderTable(ffields.ToList(), maxLevel));

                for (var i = 0; i < dtContent.Rows.Count; i++)
                {
                    var row = new FRow();
                    for (var j = 0; j < dtContent.Columns.Count; j++)
                    {
                        row.Cells.Add(new FField
                        {
                            FieldName = dtContent.Rows[i][j].ToString(),
                            FieldData = dtContent.Rows[i][j].ToString()
                        });
                    }

                    if (i == dtContent.Rows.Count - 1)
                    {
                        row.Cells.RemoveAt(0);
                        row.Cells[0].ColumnSpan = 2;
                        row.Cells[0].FieldName = "Tổng số";
                    }
                    htmlTable.Append(row.ToTrTag());
                }

                htmlTable.Append(_htmlTableEnd);

                return htmlTable.ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region "Convert To FField"

        /// <summary>
        /// Hàm chuyển đối tượng TreeItem sang dạng FField
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        public static List<FField> ConvertToFField(TreeItem tree, int maxLevel)
        {
            var result = new List<FField>();
            result.Add(new FField
            {
                FieldName = tree.Name,
                FieldData = tree.Name,
                ColumnSpan = GetColumnSpan(tree) == 1 || GetColumnSpan(tree) == 0 ? (int?)null : GetColumnSpan(tree),
                RowSpan = tree.Level == maxLevel || tree.Children.Count() >= 1 ? (int?)null : maxLevel + 1 - tree.Level,
                Level = tree.Level,
                ParentValue = tree.ParentName
            });
            foreach (var t in tree.Children)
            {
                result.AddRange(ConvertToFField(t, maxLevel));
            }
            return result;
        }

        /// <summary>
        /// Hàm chuyển dữ liệu dạng tree về FField trong cấu trúc FTable
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static List<FField> ConvertToFField(TreeItem tree)
        {
            var levelMax = GetLevelMax(tree) -1;
            var result = new List<FField>();
            if (levelMax != 0)
            {
                var ff = ConvertToFField(tree, levelMax).Where(t => t.Level != 0);
                if (ff.Any())
                {
                    result = ff.ToList();
                }
            }
            return result;
        }

        #endregion

        #region "Utils Method"

        /// <summary>
        /// Lấy thông tin Column Span của 1 TreeItem, dùng để generate html table
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static int GetColumnSpan(TreeItem tree)
        {
            var result = 0;
            foreach (var t in tree.Children)
            {
                if (t.Children.Count == 0)
                {
                    result++;
                }
                else
                {
                    if (t.Children.Count == 1)
                    {
                        result += 2;
                    }
                    else
                    {
                        result += GetColumnSpan(t);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy max level(deep) của cây(tree) truyền vào
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static int GetLevelMax(TreeItem tree)
        {
            if (tree == null) return 0;
            if (tree.Children.Any())
            {
                return 1 + tree.Children.Select(t=>GetLevelMax(t)).Max();
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Lấy max level(deep) của cây(tree) truyền vào(không sử dụng Node Root)
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static int GetLevelMaxNotRoot(TreeItem tree)
        {
            return GetLevelMax(tree) - 1;
        }

        /// <summary>
        /// Trường hợp thiết lập độ rộng(auto) cho các cột trong table
        /// </summary>
        /// <param name="ff"></param>
        public static void SetWidthCols(IEnumerable<FField> ff)
        {
            var cols = ff.Where(t => t.ColumnSpan == null || t.ColumnSpan == 0);
            if (cols.Count() > 2)
            {
                double tableWidth = 80,colWidth = 0;
                cols.ToList()[0].Width = 3;
                cols.ToList()[1].Width = 17;
                colWidth = tableWidth / (cols.Count() - 2);
                for (var i = 2; i < cols.Count(); i++)
                {
                    cols.ToList()[i].Width = colWidth;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ff"></param>
        public static void SetWidthFirstRow(List<FField> ff)
        {
            if (ff.Count() > 3)
            {
                double colWidth = 0;
                ff[0].Width = 3;
                ff[1].Width = 10;
                ff[2].Width = 47;
                colWidth = 40 / (ff.Count- 3);
                for (var i = 3; i < ff.Count; i++)
                {
                    ff[i].Width = colWidth;
                }
            }
        }

        #endregion
    }
}