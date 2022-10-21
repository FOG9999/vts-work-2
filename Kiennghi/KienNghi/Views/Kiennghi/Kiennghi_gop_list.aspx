<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Danh sách kiến nghị cùng nội dung đã gộp
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
				    <span>Danh sách kiến nghị cùng nội dung đã gộp</span>
			    </li>
		    </ul>
	    </div>
        <% KN_KIENNGHI th = (KN_KIENNGHI)ViewData["kiennghi"];
            KN_CL d = (KN_CL)ViewData["detail"]; %>
        <div class="breadcrumbs" style="padding:5px 15px">
            <p><strong>Nội dung kiến nghị gộp:</strong> <%=th.CNOIDUNG.Replace("\r\n", "<br /><br />") %></p>
            <p><strong>Thẩm quyền:</strong> <%=d.donvi_thamquyen%>;
                <strong>Lĩnh vực:</strong> <%=d.linhvuc%>
            </p>
        </div>
        <div id="search_place" class="nomargin"></div>        
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Danh sách kiến nghị cùng nội dung đã gộp</h3>
				    </div>
				    <div class="box-content nopadding">                    
                         <table class="table table-condensed table-bordered table-striped">
                            <%--<tr>
                                <th width="3%" nowrap>STT</th>
                                <th nowrap>Nội dung kiến nghị</th>
                                <th width="15%" class="tcenter" nowrap>Tiếp nhận</th>
                                <th width="10%" class="tcenter" nowrap>Chức năng</th>
                            </tr>--%>
                            <%=ViewData["list"] %>
                        </table>                    
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
  <script type="text/javascript">
      <% if (ViewData["themkiennghi"].ToString() == "1") {%>
         ShowTimKiem_Conf('id=<%=ViewData["id"] %>', '/Kiennghi/Ajax_Themkiennghi_gop/', 'search_place');
      <% }%>
            
      function TimKienNghi() {
          ShowPopUp('' + $("#form_chon").serialize() + '', '/Kiennghi/Ajax_Themkiennghi_gop_search');
      return false;
    }
  </script>
</asp:Content>
