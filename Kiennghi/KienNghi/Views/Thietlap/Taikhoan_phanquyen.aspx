<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Phân quyền chức năng tài khoản
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
				    <span> Phân quyền chức năng tài khoản</span>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>
        <% USERS u = (USERS)ViewData["taikhoan"];%>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Phân quyền chức năng tài khoản "<%=u.CTEN %>"</h3>
                        
				    </div>
				    <div class="box-content nopadding">
                        <form method="post">
                            
                        <table class="table table-bordered table-striped">
                            <tr>
                                <td width="20%" class="b">Chọn nhóm tài khoản</td>
                                <td><%=ViewData["list_group"] %></td>
                            </tr>
                            <tr>
                                <td width="20%" class="b">Chọn chức năng</td>
                                <td>
                                    <div class="actions" id="action">
                                        <%=ViewData["list_action"] %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                    <input type="submit" value="Cập nhật" class="btn btn-success" />
                                    <a href="<%=ResolveUrl("~") %>Thietlap/Taikhoan" class="btn btn-warning">Quay lại</a>
                                </td>
                            </tr>
                        </table>
                            </form>
					    
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script type="text/javascript">
        $(".nhom").click(function () {
            var act = $(".nhom:checked").map(function () {
                return $(this).val();
            }).toArray();
            ShowPageLoading();
            $.post("<%=ResolveUrl("~") %>Thietlap/Ajax_List_action_choice", "arr=" + act, function (data) {
                $("#action").html(data);
                HidePageLoading()
            })
        });
    </script>
</asp:Content>
