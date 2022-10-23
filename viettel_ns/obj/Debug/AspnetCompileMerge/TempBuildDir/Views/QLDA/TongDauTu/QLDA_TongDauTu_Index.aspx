﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/QLDA/jsBang_QuanLyDauTu.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
    String ParentID = "Edit";
    String sSoQuyetDinh = Request.QueryString["sSoQuyetDinh"];
    String iID_MaTongDauTu_QuyetDinh = Request.QueryString["iID_MaTongDauTu_QuyetDinh"];

    DataTable dtNguoiDung = QLDA_TongDauTuModels.Get_NguoiTaoQuyetDinhTongDauTu();
    SelectOptionList slNguoiDung = new SelectOptionList(dtNguoiDung, "sID_MaNguoiDungTao", "sTen");
    dtNguoiDung.Dispose();

    NameValueCollection data = QLDA_TongDauTuModels.LayThongTin(sSoQuyetDinh);
    String dNgayQuyetDinh = CommonFunction.LayXauNgay(Convert.ToDateTime(data["dNgayQuyetDinh"]));
    String dNgayLap = CommonFunction.LayXauNgay(Convert.ToDateTime(data["dNgayLap"]));
    
    String sSoQuyetDinh_Cu = data["sSoQuyetDinh_Cu"];

    if(sSoQuyetDinh_Cu != null && sSoQuyetDinh_Cu != ""){
        if (QLDA_TongDauTuModels.CheckSoQuyetDinhTrongTongDauTu(sSoQuyetDinh) == 0 )
        {
            QLDA_TongDauTuModels.DienDuLieuCuaSoQuyetDinhCu(sSoQuyetDinh, sSoQuyetDinh_Cu, User.Identity.Name, Request.UserHostAddress);
        }
    }
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
                <%=MyHtmlHelper.ActionLink(Url.Action("List", "QLDA_TongDauTu"), "Danh sách quyết định tổng đầu tư")%>
            </div>
        </td>
          <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#pHeader').click(function () {
            $('#dvContent').slideToggle('slow');
        });
    });
    $(document).ready(function () {
        $("DIV.ContainerPanel > DIV.collapsePanelHeader > DIV.ArrowExpand").toggle(
            function () {
                $(this).parent().next("div.Content").show("slow");
                $(this).attr("class", "ArrowClose");
            },
            function () {
                $(this).parent().next("div.Content").hide("slow");
                $(this).attr("class", "ArrowExpand");
            });
    });            
</script>
<div id="ContainerPanel" class="ContainerPanel">
    <div id="pHeader" class="collapsePanelHeader"> 
        <div id="dvHeaderText" class="HeaderContent" style="width: 80%;">
            <div style="width: 100%; float: left;">
                <span><%=NgonNgu.LayXau("Tìm kiếm thông tin tổng đầu tư")%></span>
            </div>
        </div>
        <div id="dvArrow" class="ArrowExpand"></div>
    </div>
    <div id="dvContent" class="Content" style="display:none">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" align="left" style="width: 50%;">
                    <div id="nhapform">
                        <div id="form2">
                            <%
                            using (Html.BeginForm("SearchSubmit", "QLDA_TongDauTu", new { ParentID = ParentID }))
                            {       
                            %>
                                <table cellpadding="0" cellspacing="0" border="0" class="table_form2" width="50%">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Số quyết định</b></div></td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <script type="text/javascript">
                                                    function sSoPheDuyet_UpperCase(ctl) {
                                                        ctl.value = ctl.value.toUpperCase();
                                                    }
                                                </script>
                                                <%=MyHtmlHelper.TextBox(ParentID, "", "sSoQuyetDinh", "", "onblur=\"sSoPheDuyet_UpperCase(this);\" class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Người nhập</b></div></td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                 <%=MyHtmlHelper.DropDownList(ParentID, slNguoiDung, "", "sNguoiDung", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Tìm từ ngày</b></div></td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dTuNgay", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Tìm đến ngày</b></div></td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dDenNgay", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="td_form2_td1" align="right">
            	                            <div style="text-align:right; float:right; width:100%">
                                                <input type="submit" class="button4" value="Tìm kiếm" style="float:right; margin-left:10px;"/>
            	                            </div> 
            	                        </td>
                                    </tr>
                                    <tr><td class="td_form2_td1" align="right" colspan="2"></td></tr>
                                </table>
                            <%} %>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<br />
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Thông tin tổng mức đầu tư</span>
                        </td>
                        <td align="right">
                            <span>F2: Thêm hàng -- DELETE: Xóa Hàng -- F10: Lưu thông tin-- Space:Sửa thông tin</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <table cellpadding="5" cellspacing="5" width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 10%">
                                <div><b>Số quyết định</b>&nbsp;<span  style="color:Red;">*</span></div>
                            </td>
                            <td class="td_form2_td5" style="width: 15%">
                                <div>
                                    <%=data["sSoQuyetDinh"]%>
                                </div>
                            </td>
                            <td class="td_form2_td1"  style="width: 10%">
                                <div><b>Ngày quyết định</b>&nbsp;<span  style="color:Red;">*</span></div>
                            </td>
                            <td class="td_form2_td5" style="width: 15%">
                                <div>
                                    <%=dNgayQuyetDinh %>
                                </div>
                            </td>
                            <td class="td_form2_td1"  style="width: 10%">
                                <div><b>Ngày lập</b>&nbsp;<span  style="color:Red;">*</span></div>
                            </td>
                            <td class="td_form2_td5" style="width: 15%">
                                <div>
                                    <%=dNgayLap%>
                                </div>
                            </td>
                            <td class="td_form2_td1" style="width: 10%">
                                <div><b>Cấp phê duyệt</b>&nbsp;<span  style="color:Red;">*</span></div>
                            </td>
                            <td class="td_form2_td5" style="width: 15%">
                                <div>
                                   <%=data["sCapPheDuyet"]%>
                                </div>
                            </td>
                        </tr>
                    </table>                    
                    <%Html.RenderPartial("~/Views/QLDA/TongDauTu/QLDA_TongDauTu_Index_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>


