<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Quản lý cơ quan, đơn vị
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
                    <span>  Thiết lập  <i class="icon-angle-right"></i> </span>
				  
			    </li>
                <li>
                       <span>
                          Đơn vị hành chính
                           </span>
				  
			    </li>
		    </ul>
		        
	    </div>
        <div class="function_chung">
            <a onclick="ShowPopUp('','/Thietlap/Ajax_Coquan_import')" data-original-title="Import danh mục cơ quan" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-cloud-upload"></i></a>
            <a onclick="ShowPopUp('','/Thietlap/Ajax_Coquan_add')" data-original-title="Thêm mới cơ quan" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
            <form class="search" id="form_search" method="post" onsubmit="return false;" >
                <input type="text" name="ip_noidung" id="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" value="" placeholder="Tìm kiếm cơ quan ...">
                <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
            </form>
        </div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-h-sign"></i>Danh sách đơn vị hành chính</h3>
				    </div>
				    <div class="box-content nopadding">
					    <table class="table table-bordered  table-striped">
                            <thead>
                                <tr>
                                       
                                    <th nowrap class="tcenter" width="5%">STT</th>                                 
                                    <th nowrap class="tcenter" width="5%">Mã cơ quan</th>                                 
                                    <th nowrap>Tên cơ quan</th>
                                    <th nowrap>Địa phương tương ứng</th>
                                    <th nowrap class="tcenter" width="5%">Vị trí</th> 
                                    <th nowrap class="tcenter" width="5%">Tham gia phần mềm</th> 
                                    <th nowrap class="tcenter" width="5%">Nhóm cơ quan</th>                                                                  
                                    <th nowrap class="tcenter" width="5%">Chức năng</th>
                                </tr>
                            </thead>
                            <tbody id="div_change">                          
                               <%=ViewData["list"] %>
                               
                            </tbody>
                             <%=ViewData["phantrang"] %>
                        </table>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script src="/js/bootstrap-typeahead.js"></script>
    <script type="text/javascript">  
    //GetContentTimKiem("/Thietlap/Ajax_Coquan_list", "timkiem", "");

    function TimKiemCoQuan() {
        if ($("#select").val() != 0) {
            $.post("<%=ResolveUrl("~")%>Thietlap/Ajax_TimKiemCoQuan",
                "id=" + $("#select").val(),
                function (data) {
                    $("#refresh").html(data);
                }
            );
        }
        }

    function TimKiem() {
        var tentimkiem = $("#ip_noidung").val();
        window.location = "/Thietlap/Coquan/?q=" + tentimkiem;
        return false;

    }
    
    </script>
</asp:Content>
