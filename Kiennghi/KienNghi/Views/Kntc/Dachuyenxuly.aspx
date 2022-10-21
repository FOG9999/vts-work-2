<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Danh sách đơn đã chuyển xử lý, giải quyết

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <%: Html.Partial("../Shared/_Left_Kntc") %>
<div id="main" class="">
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                <li>
				    <span>Khiếu nại tố cáo <i class="icon-angle-right"></i></span>
                    
			    </li>
                <li>
                   <span> Đơn đã chuyển đến cơ quan có thẩm quyền xử lý, giải quyết</span>
			    </li>
		    </ul>
		  
	    </div>
          <div class="function_chung">
                <form class="search" id="form_search" method="get" onsubmit="return false;">
                   <%-- <%=ViewData["select-donvi"] %>--%>
                    <input type="text" name="ip_noidung" id="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" value="" placeholder="Nội dung đơn, người gửi đơn...">
                        <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                 
                        <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kntc/Ajax_Dachuyenxuly_formsearch/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                        <button type="button" title="Tìm kiếm" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>

                </form>
            </div>   
          <div id="search_place"></div>          
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-time"></i>Danh sách đơn đã chuyển đến cơ quan có thẩm quyền xử lý, giải quyết</h3>
                        <div class="pull-right box-title-header">                                     
                            <select class="chosen-select" name="iTrangThai" id="iTrangThai" onchange="ChangeDonViTrangThai();">
                                <%=ViewData["select-trangthai"] %>
                            </select>
                                      
                          <%--  <select class="chosen-select" name="iDonVi" id="iDonVi" onchange="ChangeDonViTrangThai();">
                                <option value="0">--- Chọn tất cả</option>
                                <%=ViewData["select-donvi"] %>
                            </select>--%>
                        </div>
				    </div>
				    <div class="box-content nopadding">                     
                         
					    <table class="table table-bordered table-striped">
                            <thead>
                               <tr>   
                                    <th  class="tcenter" style="width:3%">STT</th>
                                     <th ">Người gửi / địa chỉ</th>   
                                    <th ">Nội dung đơn</th>  
                                   
                                    <th style="width:5%">Chức năng</th>             
                                </tr>
                            </thead>
                            <tbody id="ip_data">                                                       
                                <%=ViewData["list"] %>
                            </tbody>                            
                        </table>      
                                 
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
      <script>
          function ChangeDonViTrangThai() {
              location.href = "/Kntc/Dachuyenxuly/?trangthai=" + $("#iTrangThai").val() + "";
          }
          function TimKiem() {
              $("#ip_data").show().html("<tr><td colspan=6 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
              $.post("/Kntc/Ajax_Dachuyenxuly_search", $("#form_search").serialize() + "&iDonVi=" + $("#iDonVi").val() + "&iTrangThai=" + $("#iTrangThai").val(), function (ok) {
                  $("#ip_data").html(ok);
              });
              return false;
          }
   </script></asp:Content>
