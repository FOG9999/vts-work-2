<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Quản lý tài khoản
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
				    <span>Danh sách tài khoản</span>
			    </li>
		    </ul>
		        
	    </div>
        <div class="function_chung">
                    <a onclick="ShowPopUp('','/Thietlap/Ajax_Taikhoan_add')" data-original-title="Thêm mới" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                    <form class="search" id="form_search" method="post" onsubmit="return false;" >
                 <input type="text" name="ip_noidung" id="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" value="" placeholder="Tìm kiếm tài khoản ...">
                        <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                </form>
                </div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-user"></i>Danh sách tài khoản</h3>
                        
				    </div>
				    <div class="box-content nopadding"  id="div_change">
					    <table class="table table-bordered table-condensed table-striped ">
                            <thead>
                                <tr>   
                                    <th nowrap width="15%">Tên đăng nhập/ người dùng</th>                                 
                                    <th nowrap>Phòng ban (chức vụ)</th>
                                    <th nowrap>Thông tin tài khoản</th>                                            
                                    <th nowrap class="" width="35%">Nhóm tài khoản</th>
                                    <th nowrap class="tcenter" width="5%">Kích hoạt</th>                                                            
                                    <th nowrap class="tcenter" width="5%">Chức năng</th>
                                </tr>
                            </thead>
                            <tbody  >                          
                                <%= ViewData["taikhoan"] %>
                            </tbody>
                            <%= ViewData["phantrang"] %>
                        </table>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script type="text/javascript">
        function TimKiem() {
            //$("#div_change").show().html("<tr><td colspan=6  class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            //$.post("/Thietlap/Ajax_Taikhoan_Timkiem", $("#form_search").serialize(), function (ok) {
            //    $("#div_change").html(ok);
            //});
            var tentimkiem  = $("#ip_noidung").val();
            window.location = "/Thietlap/Taikhoan/?q=" + tentimkiem;
            return false;
    
        }

        <%-- function TimKiem() {
            $("#ketqua_tracuu").show().html("<tr><td colspan='4'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            var frm = $("#fromsearch");
            var data = frm.serializeArray();
            var tentimkiem = $("#tentimkiem").val();
            $.ajax({
                type: "post",
                headers: getHeaderToken(),
                contentType: "application/json; charset=utf-8",
                url: "<%=ResolveUrl("~")%>Vanban/Ajax_VanbanDuyet_Timkiem",
                data: data,
                success: function (ok) {
                    $("#ketqua_tracuu").html(ok);
                }
            });
            return false;
        }--%>

    </script>
</asp:Content>
