﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/QuyetToan/QuyetToan_ChungTuKeToan.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
    String ParentID = "Edit";
    String iID_MaMucLucNganSach = Convert.ToString(ViewData["iID_MaMucLucNganSach"]);
    String iQuy = Convert.ToString(ViewData["iQuy"]);

    DataTable dtNguoiDung = QLDA_TongDauTuModels.Get_NguoiTaoQuyetDinhTongDauTu();
    SelectOptionList slNguoiDung = new SelectOptionList(dtNguoiDung, "sID_MaNguoiDungTao", "sTen");
    dtNguoiDung.Dispose();

    DataTable dtMLNS = MucLucNganSachModels.dt_ChiTietMucLucNganSach(iID_MaMucLucNganSach);
    String sMotTa = Convert.ToString(dtMLNS.Rows[0]["sMoTa"]);
    dtMLNS.Dispose();
%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 10%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTuKeToan"), "Nhập chứng từ lẻ")%>
            </div>
        </td>
    </tr>
</table>
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="custom_css box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span><%=sMotTa %></span>
                        </td>
                        <td align="right">
                            <span>F2: Thêm hàng -- DELETE: Xóa Hàng -- F10: Lưu thông tin</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <%Html.RenderPartial("~/Views/QuyetToan/ChungTuKeToan/QuyetToan_ChungTuKeToan_Detail_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>



