<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Quản lý địa phương
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
				     <span>    Thiết lập <i class="icon-angle-right"></i>  </span>
			    </li>
                <li>
                      <span> Danh sách địa phương </span>
				  
			    </li>
		    </ul>
		    
	    </div>
        <div class="function_chung">
                <a onclick="ShowPopUp('','/Thietlap/Ajax_Diaphuong_add')" data-original-title="Thêm mới" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get">

                    <input type="text" name="ip_noidung" id="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" value="" placeholder="Tìm kiếm địa phương ...">
                    <button type="submit" class="btn_f blue"><i class="icon-search"></i></button>
                </form>
            </div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-map-marker"></i> Danh sách địa phương</h3>
                        
				    </div>
				    <div class="box-content nopadding">
                            
					    <table class="table table-bordered table-striped">
                            <thead>
                                <tr>
                                     
                                    <th nowrap class="tcenter" width="5%">Mã địa phương</th>                                 
                                    <th nowrap>Tên địa phương</th>     
                                    <th nowrap class="tcenter" width="10%">Cấp đơn vị hành chính</th>  
                                    <th class="tcenter"  width="5%" nowrap>Áp dụng</th>                                                           
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
<script type="text/javascript">  

         function TimKiem() {
             var tentimkiem = $("#ip_noidung").val();
             window.location = "/Thietlap/Coquan/?q=" + tentimkiem;
             return false;

    }
</script>
</asp:Content>
