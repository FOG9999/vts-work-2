<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Danh sách kiến nghị đã chọn đưa vào giám sát
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
				    <span>Danh sách kiến nghị đã chọn đưa vào giám sát</span>
			    </li>
                
		    </ul>
		    <%--<div class="function_chung">
                <a <%=ViewData["btn-add"] %> onclick="ShowTimKiem_Conf('id=<%=ViewData["id"] %>','/Kiennghi/Ajax_Themkiennghi_giamsat/','search_place')" data-original-title="Thêm kiến nghị" rel="tooltip" href="javascript:void(0)" class="add btn_f blue" ><i class="icon-plus-sign"></i></a>    
            </div>--%>
	    </div>        
        <div id="search_place"></div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Danh sách kiến nghị đã chọn</h3>
				    </div>
				    <div class="box-content nopadding">                    
                         <table class="table table-condensed table-bordered nomargin table-striped">
                            <tr>
                                <th width="3%" nowrap>STT</th>
                                <th nowrap>Nội dung kiến nghị</th>
                                <th nowrap>Trả lời của cơ quan có thẩm quyền</th>
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
  <script type="text/javascript">
      <%=ViewData["themkiennghi"]%>
      function TimKienNghi() {
        ShowPopUp('' + $("#form_chon").serialize() + '', '/Kiennghi/Ajax_Themkiennghi_giamsat_search');

    }
  </script>
</asp:Content>
