﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "Edit";
    String MaND = User.Identity.Name;
    String iLoaiKeHoachVon = Request.QueryString["iLoaiKeHoachVon"];
    String dTuNgay = Request.QueryString["dTuNgay"];
    String dDenNgay = Request.QueryString["dDenNgay"];
    String page = Request.QueryString["page"];

    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dtDuAn = QLDA_DanhMucDuAnModels.ddl_DanhMucDuAn(true);
    SelectOptionList slDuAn = new SelectOptionList(dtDuAn, "iID_MaDanhMucDuAn", "TenHT");
    dtDuAn.Dispose();

    DataTable dtLoaiKeHoachVon = QLDA_KeHoachVonModels.DT_LoaiKeHoachVon();
    SelectOptionList slLoaiKeHoachVon = new SelectOptionList(dtLoaiKeHoachVon, "iID_MaLoaiKeHoachVon", "sTen");
    dtLoaiKeHoachVon.Dispose();

    DataTable dt = QLDA_KeHoachVonModels.Get_DanhSach(iLoaiKeHoachVon, dTuNgay, dDenNgay, CurrentPage, Globals.PageSize);

    double nums = QLDA_KeHoachVonModels.Get_DanhSach_Count(iLoaiKeHoachVon, dTuNgay, dDenNgay);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Search", "QLDA_KeHoachVon", new {iLoaiKeHoachVon = iLoaiKeHoachVon, TuNgay = dTuNgay, DenNgay = dDenNgay, page = x }));

    using (Html.BeginForm("SearchSubmit", "QLDA_KeHoachVon", new { ParentID = ParentID }))
    {
%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Search", "QLDA_KeHoachVon"), "Tìm kiếm danh sách kế hoạch vốn")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Thông tin tìm kiếm</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0" cellspacing="0" border="0" class="table_form2" width="100%">
                <tr>
                    <td class="td_form2_td1" style="width: 10%;"><div><b>Loại kế hoạch vốn</b></div></td>
                    <td class="td_form2_td5" style="width: 40%;">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slLoaiKeHoachVon, iLoaiKeHoachVon, "iLoaiKeHoachVon_Search", "", "class=\"input1_2\"")%>
                        </div>
                    </td>
                    <td class="td_form2_td1" style="width: 10%;"><div><b></b></div></td>
                    <td class="td_form2_td5" style="width: 40%;">
                        <div>
                            
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" style="width: 10%;"><div><b>Tìm từ ngày</b></div></td>
                    <td class="td_form2_td5" style="width: 40%;">
                        <div>
                            <%=MyHtmlHelper.DatePicker(ParentID, dTuNgay, "dTuNgay", "", "class=\"input1_2\"")%>
                        </div>
                    </td>
                    <td class="td_form2_td1" style="width: 10%;"><div><b>Tìm đến ngày</b></div></td>
                    <td class="td_form2_td5" style="width: 40%;">
                        <div>
                            <%=MyHtmlHelper.DatePicker(ParentID, dDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                        </div>
                    </td>
                </tr>
                <tr>
            	    <td class="td_form2_td5" colspan="4">
            	        <div style="text-align:right; float:right; width:100%">
                            <input type="submit" class="button4" value="Tìm kiếm" style="float:right; margin-left:10px;"/>
            	        </div> 
            	    </td>
                </tr>
                <tr><td class="td_form2_td1" align="right" colspan="4"></td></tr>
            </table>
        </div>
    </div>
</div>
<%  } %>
<br />
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                    <span>Danh sách tìm kiếm hợp đồng</span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 15%;" align="center">Loại kế hoạch vốn</th>
            <th style="width: 62%;" align="center">Tên dự án</th>
            <th style="width: 15%;" align="center">Ngày tạo</th>
        </tr>
        <%
        int i;
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;
            String sTrangThai = "";
            String strColor = "";
            
            //Lấy tên dự án
            String strTenDoanhNghiep = "";
            DataTable dtDanhMucDuAn = QLDA_DanhMucDuAnModels.Row_DanhMucDuAn(Convert.ToString(R["iID_MaDanhMucDuAn"]));
            if (dtDanhMucDuAn.Rows.Count > 0)
            {
                strTenDoanhNghiep = Convert.ToString(dtDanhMucDuAn.Rows[0]["TenHT"]);
            }
            dtDanhMucDuAn.Dispose();

            //Loại kế hoạch vốn
            String strLoaiKeHoachVon = "";
            DataTable dtLoaiKeHoachVonHT = QLDA_KeHoachVonModels.Get_Row_LoaiKeHoachVon(Convert.ToString(R["iLoaiKeHoachVon"]));
            if (dtLoaiKeHoachVonHT.Rows.Count > 0)
            {
                strLoaiKeHoachVon = Convert.ToString(dtLoaiKeHoachVonHT.Rows[0]["sTen"]);
            }
            dtLoaiKeHoachVonHT.Dispose();
            
            //Ngày tạo tổng đầu tư 
            String dNgayKeHoachVon = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayKeHoachVon"]));
        
            %>
            <tr <%=strColor %>>
                <td align="center"><%=R["rownum"]%></td>    
                <td align="center"><b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_KeHoachVon", new { iID_MaDanhMucDuAn = R["iID_MaDanhMucDuAn"], iLoaiKeHoachVon = R["iLoaiKeHoachVon"], dNgayKeHoachVon = dNgayKeHoachVon }).ToString(), strLoaiKeHoachVon, "Detail", "")%></b></td>
                <td align="left">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_KeHoachVon", new { iID_MaDanhMucDuAn = R["iID_MaDanhMucDuAn"], iLoaiKeHoachVon = R["iLoaiKeHoachVon"], dNgayKeHoachVon = dNgayKeHoachVon }).ToString(), strTenDoanhNghiep, "Detail", "")%></b>
                </td>
                <td align="center"><%=dNgayKeHoachVon%></td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="4" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%  
dt.Dispose();
%>
<script type="text/javascript">
    $(document).ready(function () {
        //Hide the div tag when page is loading
        $('#dvText').hide();

        //For Show the div or any HTML element
        $("#btnSearch").click(function () {
            $('#dvText').show();
            $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
            $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer 
        });

        //For hide the div or any HTML element
        $("#aHide").click(function () {
            $('#dvText').hide();
        });

        $(window).resize(function () {
            $('.popup_block').css({
                position: 'absolute',
                left: ($(window).width() - $('.popup_block').outerWidth()) / 2,
                top: ($(window).height() - $('.popup_block').outerHeight()) / 2
            });
        });
        // To initially run the function:
        $(window).resize();
        //Fade in Background
    });                                 
</script>
<div id="dvText" class="popup_block">
    <img src="../../../Content/ajax-loader.gif"/><br />
    <p>Hệ thống đang thực hiện yêu cầu...</p>
</div>
</asp:Content>


