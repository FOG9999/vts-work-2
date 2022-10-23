﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    int i;
    String ParentID = "PhongBan";
    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    SqlCommand cmd;
        
    if(String.IsNullOrEmpty(page) == false){
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = DanhMucModels.GetPhongBan(CurrentPage, Globals.PageSize);

    double nums = DanhMucModels.GetPhongBan_Count();
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {page  = x}));
    String strThemMoi = Url.Action("Edit", "PhongBan");    
%>

<div class="custom_css box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách phòng ban</span>
                </td>
                <td>
                    <button id="Button1" class="btn btn-primary pull-right" onclick="javascript:location.href='<%=strThemMoi %>'" ><i class="fa fa-plus" aria-hidden="true"></i>Thêm mới</button>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 20%;" align="center">Tên phòng ban</th>
          
            <th  align="left">Mô tả</th>

            <th style="width: 10%;" align="center">Thao tác</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            //String MaLoaiDonVi = Convert.ToString(R["iID_MaLoaiDonVi"]);
            //String MaNhomDonVi = Convert.ToString(R["iID_MaNhomDonVi"]);
            //String MaKhoiDonVi = Convert.ToString(R["iID_MaKhoiDonVi"]);
            //String sLoaiDonVi = Convert.ToString(DanhMucModels.GetRow_DanhMuc(MaLoaiDonVi).Rows[0]["sTen"]);
            //String sNhomDonVi = Convert.ToString(DanhMucModels.GetRow_DanhMuc(MaNhomDonVi).Rows[0]["sTen"]);
            //String sKhoiDonVi = Convert.ToString(DanhMucModels.GetRow_DanhMuc(MaKhoiDonVi).Rows[0]["sTen"]);
            String classtr = "";            
            if (i % 2 == 0)
            {
                classtr = "class=\"alt\"";
            }
            %>
            <tr <%=classtr %>>
                <td align="center"><%=R["rownum"]%></td>     
               
                <td align="left"><%=dt.Rows[i]["sTen"]%></td>
                <td align="left"><%=dt.Rows[i]["sMoTa"]%></td>
                
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "PhongBan", new { Code = R["iID_MaPhongBan"] }).ToString(), "<i class='fa fa-pencil-square-o fa-lg color-icon-edit'></i>", "Edit", "")%> &nbsp;
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "PhongBan", new { Code = R["iID_MaPhongBan"] }).ToString(), "<i class='fa fa-trash-o fa-lg color-icon-delete'></i>", "Delete", "")%>
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="9" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
</asp:Content>
