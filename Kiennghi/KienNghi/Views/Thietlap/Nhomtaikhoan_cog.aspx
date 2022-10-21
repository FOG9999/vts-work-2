<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Thiết lập chức năng nhóm tài khoản
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Thietlap") %>
<div id="main" class="">
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
				    <span>Danh sách chức năng nhóm tài khoản</span>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>
        <% USER_GROUP nhom = (USER_GROUP)ViewData["nhom"]; %>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Thiết lập chức năng nhóm tài khoản <%=nhom.CTEN %></h3>
                        
				    </div>
				    <div class="box-content">
                        <form method="post"  class="nomargin">
                            
                            <div class="actions">
                                <%=ViewData["list"] %>
                            </div>
                            <br />
                            <p class="tcenter clear">
                                
                                <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                <input type="submit" value="Cập nhật" class="btn btn-success" />
                                <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>
                            </p>
                        </form>
					    
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script>
        function Checkboxchucnang(val)
        {
            document.getElementById("action" + val).checked = !document.getElementById("action" + val).checked;
            //var checkbox = $("#action" + val + "").attr("checked"); alert(checkbox);
            //if (checkbox == 'checked') {
            //    $("#action" + val + "").removeAttr("checked", "checked");
            //}
            //if (checkbox == 'undefined')
            //    {
            //    $("#action" + val + "").attr("checked", "checked");
            //}
                    }
    </script>
</asp:Content>

