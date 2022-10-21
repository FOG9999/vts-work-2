<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Danh sách kiến nghị trong tổng hợp
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <%: Html.Partial("../Shared/_Left_Knct") %>
<div id="main">
              <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                <li>
                    <span>Kiến nghị cử tri <i class="icon-angle-right"></i></span>                    
                </li>
                <li>
				    <span>Danh sách kiến nghị trong tổng hợp kiến nghị</span>
			    </li>
		    </ul>
	    </div>        
        <% KN_TONGHOP th = (KN_TONGHOP)ViewData["tonghop"];
        TongHop_Kiennghi d = (TongHop_Kiennghi)ViewData["detail"]; %>
        <div class="breadcrumbs" style="padding:5px 15px">
            <p><strong>Ghi chú:</strong> <%=Server.HtmlEncode(th.CNOIDUNG) %> <%=ViewData["file"] %></p>
            <p><strong>Đoàn tổng hợp:</strong> <%=d.donvi_tonghop%>; 
                <strong>Thẩm quyền:</strong> <%=d.donvi_thamquyen%>;
                <strong>Lĩnh vực:</strong> <%=d.linhvuc%>
            </p>
        </div>    
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Danh sách kiến nghị trong tổng hợp kiến nghị</h3>
				    </div>
				    <div class="box-content nopadding">                    
                         <table class="table table-condensed table-bordered nomargin table-striped">
                            <tr>
                                <th width="3%" nowrap>STT</th>
                                <th width="50%" nowrap>Ghi chú</th>
                                <th nowrap>Kết quả xử lý</th>
                                <th width="3%" nowrap>Chức năng</th>
                            </tr>
                            <%=ViewData["list"] %>
                        </table>                           
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
  <%--<script type="text/javascript">
      function TimKienNghi() {
        ShowPopUp('' + $("#form_chon").serialize() + '', '/Kiennghi/Ajax_Themkiennghi_tonghop_search');

    }
  </script>--%>
</asp:Content>
