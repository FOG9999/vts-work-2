using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using KienNghi.Application.Flexcel;
using Utilities;

namespace  KienNghi.Flexcel
{
    public static class FlexcelExtension
    {
        public static FlexCelReport AddTable(this FlexCelReport fr, DataTable dt)
        {
            fr.AddTable(dt.TableName, dt);
            return fr;
        }

        public static FlexCelReport AddTableEmpty(this FlexCelReport fr, int count, int limit = 10)
        {
            var dt = new DataTable("empty");
            dt.Columns.Add("id");
            dt.Columns.Add("text");

            if (count < limit)
            {
                var delta = limit - count;
                for (int i = 0; i < delta; i++)
                {
                    var row = dt.NewRow();
                    row["id"] = i + 1;
                    dt.Rows.Add(row);
                }
            }

            fr.AddTable(dt);
            return fr;
        }

        public static FlexCelReport SetValues(this FlexCelReport fr, Dictionary<string, object> values)
        {
            values.ToList()
                .ForEach(x =>
                {
                    fr.SetValue(x.Key, x.Value);
                });

            return fr;
        }

        public static FlexCelReport SetValue(this FlexCelReport fr, dynamic param)
        {
            var dic = new Dictionary<string, object>();
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(param))
            {
                var value = prop.GetValue(param);
                var name = prop.Name;
                dic.Add(name, value);
            }
            dic.ToList()
                .ForEach(x =>
                {
                    fr.SetValue(x.Key, x.Value);
                });

            return fr;
        }

        public static FlexCelReport UseCommonValue(this FlexCelReport fr,
            FlexcelModel model = null, int dvt = 1000)
        {
            if (model == null)
            {
                model = new FlexcelModel();
            }

            model.ToFlexcel(fr);
            return fr;
        }


     

        #region flexexcel form

        public static FlexCelReport UseForm(this FlexCelReport fr, Controller controller)
        {
            var Request = controller.Request;

            var r = Request.GetQueryStringValue("r", 140);
            r = r == 0 ? 140 : r;
            fr.SetExpression("fRow", "<#Row height(Autofit;" + r + ")>");

            fr.SetExpression("fRow0", "<#Row height(0)>");
            fr.SetExpression("fRowFit", "<#Row height(Autofit)>");
            fr.SetExpression("fRowF", "<#Row height(Autofit)>");
            fr.SetExpression("autoP", "<#auto page breaks>");

            return fr;
        }

        public static void MergeH(this XlsFile xls, int row, int col, int length)
        {
            var x = col;
            var y = row;

            var x_to = x;
            var x_from = x;

            var dic = new Dictionary<int, int>();

            var x_to_temp = x_from;

            for (int i = 0; i < length; i++)
            {
                var cell1 = xls.GetCellValue(y, x + i);
                var cell2 = xls.GetCellValue(y, x + i + 1);

                if (cell1 == cell2 && cell2 != null && cell1 != null)
                {
                    x_to = x_to + 1;
                    //x_to_temp += 1;
                }
                else
                {
                    if (x_to > x_from)
                    {
                        // merge cells
                        //xls.MergeCells(y, x_from, y, x_to);

                        dic.Add(x_from, x_to);
                    }

                    x_from = x + i + 1;
                    x_to = x_from;
                }

            }


            dic.ToList()
                .ForEach(p =>
                {
                    xls.MergeCells(y, p.Key, y, p.Value);

                    var value = xls.GetCellValue(y, p.Key);
                    //if (value != null && value.ToString() == "(+)")
                    //{
                    //    TFlxApplyFormat ApplyFormat = new TFlxApplyFormat();
                    //    ApplyFormat.SetAllMembers(true);
                    //    //ApplyFormat.Borders.SetAllMembers(true);  //We will only apply the borders to the existing cell formats
                    //    TFlxFormat fmt = xls.GetDefaultFormat;
                    //    //fmt.Font.Style = TFlxFontStyles.Bold;
                    //    //fmt.Font.Size20 = 172;

                    //    fmt.HAlignment = THFlxAlignment.center;
                    //    fmt.VAlignment = TVFlxAlignment.center;

                    //    fmt.Borders.SetAllBorders(TFlxBorderStyle.Thin, TExcelColor.Automatic);
                    //    fmt.Borders.Diagonal.Color = TExcelColor.FromArgb(100);

                    //    xls.SetCellFormat(y, p.Key, y, p.Value, fmt, ApplyFormat, true);
                    //}
                });

            //if (true)
            //    //{
            //    //    xls.SetCellValue(y + 1, x + i, $"Cộng");
            //    //    xls.MergeCells(y + 1, x + i, y + 2, x + i);


            //    //    //Add a rectangle around the cells
            //    //    TFlxApplyFormat ApplyFormat = new TFlxApplyFormat();
            //    //    ApplyFormat.SetAllMembers(true);
            //    //    //ApplyFormat.Borders.SetAllMembers(true);  //We will only apply the borders to the existing cell formats
            //    //    TFlxFormat fmt = xls.GetDefaultFormat;
            //    //    fmt.Font.Style = TFlxFontStyles.Bold;
            //    //    fmt.Font.Size20 = 172;

            //    //    fmt.HAlignment = THFlxAlignment.center;
            //    //    fmt.VAlignment = TVFlxAlignment.center;

            //    //    fmt.Borders.SetAllBorders(TFlxBorderStyle.Thin, TExcelColor.Automatic);
            //    //    fmt.Borders.Diagonal.Color = TExcelColor.FromArgb(100);

            //    //    xls.SetCellFormat(y + 1, x + i, y + 1, x + i, fmt, ApplyFormat, true);  //Set last parameter to true so it draws a box.


            //    //    //// merge cells
            //    //    //xls.SetCellValue(y, x + sumFrom, _nganSachService.GetMLNS_MoTa(PhienLamViec.iNamLamViec, c.sLNS));
            //    //    //xls.MergeCells(y, x + sumFrom, y, x + i);
            //}
        }

        public static void MergeC(this XlsFile xls, int rowStart, int rowEnd, int colStart, int length)
        {
            List<MergeInfo> listMerge = new List<MergeInfo>();
            var x = colStart;
            var y = rowStart;

            var y_to = rowStart;
            var y_from = rowStart;


            var x_to_temp = y_from;

            for (int i = 0; i < length; i++)
            {
                for (int j = rowStart; j <= rowEnd; j++)
                {
                    if(j + 1 <= rowEnd) {
                        var cell1 = xls.GetCellValue(j, x + i);
                        var cell2 = xls.GetCellValue(j + 1, x + i);

                        if (cell1 == cell2 && cell2 != null && cell1 != null)
                        {
                            y_to = y_to + 1;
                        }
                        else
                        {
                            if (y_to > y_from)
                            {
                                listMerge.Add(new MergeInfo
                                {
                                    xFrom = x + i,
                                    yFrom = y_from,
                                    xTo = x + i,
                                    yTo = y_to
                                });
                                //dic.Add(y_from, y_to);
                            }

                            y_from = j + 1;
                            y_to = y_from;
                        }
                    } else
                    {
                        if (y_to > y_from)
                        {
                            listMerge.Add(new MergeInfo
                            {
                                xFrom = x + i,
                                yFrom = y_from,
                                xTo = x + i,
                                yTo = y_to
                            });
                            //dic.Add(y_from, y_to);
                        }

                        y_from = rowStart;
                        y_to = y_from;
                    }
                    
                }
            }

            listMerge
                 .ForEach(p =>
                {
                    xls.MergeCells(p.yFrom, p.xFrom, p.yTo, p.xTo);
                    //var value = xls.GetCellValue(y, p.Key);
                });
        }

        public static void Run(this FlexCelReport fr, XlsFile xls, int to)
        {
            if (to != 1)
            {
                var header = xls.GetPageHeaderAndFooter();
                header.DiffFirstPage = false;
                xls.SetPageHeaderAndFooter(header);
            }
            fr.SetValue("To", to);
            fr.Run(xls);
        }
        #endregion

        public class MergeInfo
        {
            public int xFrom { get; set; }
            public int yFrom { get; set; }
            public int xTo { get; set; }
            public int yTo { get; set; }
        }
    }
}
