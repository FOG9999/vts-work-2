<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Lịch sử hoạt động
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <%: Html.Partial("../Shared/_Left_Thietlap") %>
<div id="main">
     <a href="#" class="show_menu_trai">Menu trái</a>
              <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                <li>
				    <span> Thiết lập   <i class="icon-angle-right"></i>  </span>
			    </li>
                <li>
				    <span>Hoạt động của các tài khoản</span>                   
			    </li>
		    </ul>
	    </div>   
         <div class="breadcrumbs" style="padding:5px 15px;">
		    <p><strong>Tên đăng nhập:</strong> <%= ViewData["user"] %></p>
            <p><strong>Tên người dùng:</strong> <%=ViewData["tennguoidung"] %>;
                <strong>Thuộc đơn vị:</strong> <%=ViewData["tendonvi"] %>;
            </p>
	    </div>     
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Danh sách hoạt động xử lý</h3>                        
				    </div>
				    <div class="box-content nopadding">                                            
                           
                        <table class="table table-condensed table-bordered">                                    
                            <tr>
                                <th class="tcenter">Thời gian</th>
                                <th>Người xử lý</th>
                                <th>Nội dung xử lý</th>
                            </tr>
                            <%=ViewData["list"] %>
                            </table>                                                                                                                                         
						                         
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>

</asp:Content>