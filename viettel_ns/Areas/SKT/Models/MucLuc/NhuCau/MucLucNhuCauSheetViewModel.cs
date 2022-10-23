﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using VIETTEL.Models;

namespace VIETTEL.Areas.SKT.Models
{
    public class MucLucNhuCauSheetTable : SheetTable
    {
        public MucLucNhuCauSheetTable(bool parentRowEditable = false) : base(false, parentRowEditable)
        {
        }

        public MucLucNhuCauSheetTable(Dictionary<string, string> filters, bool parentRowEditable = false) : this(parentRowEditable)
        {
            _filters = filters ?? new Dictionary<string, string>();
        }

        public MucLucNhuCauSheetTable(NameValueCollection query, bool parentRowEditable = false) : this(parentRowEditable)
        {
            _filters = getFilters(query);
        }

        public void FillSheet(DataTable dt)
        {
            dtChiTiet = dt.Copy();
            dtChiTiet_Cu = dtChiTiet.Copy();

            ColumnNameId = "Id";
            ColumnNameParentId = "Id_Parent";
            ColumnNameIsParent = "IsParent";

            updateColumnIDs("Id");
            updateColumns();
            updateColumnsParent();
            updateCellsEditable();
            updateCellsValue();
            updateChanges();
        }

        private Dictionary<string, string> getFilters(NameValueCollection query)
        {
            var filters = new Dictionary<string, string>();
            ColumnsSearch.ToList()
                .ForEach(c =>
                {
                    filters.Add(c.ColumnName, query[c.ColumnName]);
                });
            return filters;
        }

        #region Columns

        protected override IEnumerable<SheetColumn> GetColumns()
        {
            var items = new List<SheetColumn>()
                {
                    // cot nhap
                    new SheetColumn(columnName: "Loai", header: "Loại nhập", columnWidth:100, align: "center", hasSearch: true, isReadonly: false),
                    new SheetColumn(columnName: "Nganh_Parent", header: "Ngành quản lý", columnWidth:120, align: "center", hasSearch: true, isReadonly: false),
                    new SheetColumn(columnName: "Nganh", header: "Mã ngành", columnWidth:100, align: "center", hasSearch: true, isReadonly: false),
                    new SheetColumn(columnName: "M", header: "Mã mục", columnWidth:100, align: "center", hasSearch: true, isReadonly: false),
                    new SheetColumn(columnName: "KyHieu", header: "Mã mục lục", columnWidth:120, hasSearch: true, isReadonly: false),
                    new SheetColumn(columnName: "STT", header: "Ký hiệu", columnWidth:120, align: "center", isReadonly: false),
                    new SheetColumn(columnName: "MoTa", header: "Nội dung", columnWidth:240, isReadonly: false),
                    new SheetColumn(columnName: "KyHieuCha", header: "Mã mục lục cha", columnWidth:120, hasSearch: false, isReadonly: false),
                    new SheetColumn(columnName: "STTBC", header: "STT in báo cáo", columnWidth:120, align: "left", isReadonly: false),

                    // cot khac
                    new SheetColumn(columnName: "Id", isHidden: true),
                    new SheetColumn(columnName: "sMauSac", isHidden: true),
                    new SheetColumn(columnName: "sFontColor", isHidden: true),
                    new SheetColumn(columnName: "sFontBold",isHidden: true),
                };

            return items;
        }

        #endregion

        protected override string getCellValue(DataRow r, int i, string c, IEnumerable<string> columnsName)
        {
            if (r.Field<bool>(ColumnNameIsParent))
            {
                if (c == "sMauSac")
                {
                    return "#EFEFEF";
                }
                else if (c == "sFontColor")
                {
                    return "OrangeRed";
                }
                else if (c == "sFontBold")
                {
                    return "bold";
                }
            }

            return base.getCellValue(r, i, c, columnsName);
        }

    }

    public class MucLucNhuCauViewModel
    {
        public MucLucNhuCauViewModel()
        {
            FilterOptions = new Dictionary<string, string>();
        }
        public SheetViewModel Sheet { get; set; }
        public Dictionary<string, string> FilterOptions { get; set; }
    }
}