<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Đơn chờ xử lý, giải quyết
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
                   <span> Đơn chờ xử lý, giải quyết</span>
			    </li>
		    </ul>
		  
	    </div>
          <div class="function_chung">
                <form class="search" id="form_search" method="get" onsubmit="return false;">
                   <%-- <%=ViewData["select-donvi"] %>--%>
                    <input type="text" name="ip_noidung" id="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" value="" placeholder="Nội dung đơn, người gửi đơn...">
                        <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                 
                        <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kntc/Ajax_Chuyenxuly_formsearch/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                        <button type="button" title="Tìm kiếm" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>

                </form>
            </div>   
          <div id="search_place"></div>          
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-time"></i>Danh sách đơn chờ xử lý, giải quyết</h3>
                       <div class="pull-right box-title-header">
                                <select class="chosen-select input-block-level" onchange="Sort($('#hienthi').val(),this.value)" id="donvi" name="donvi">
                                    <option value="0">Tất cả đơn vị</option>
                                    <%=ViewData["select-donvi"] %>
                                </select>
                            </div>
                            <div class="pull-right box-title-header">
                                <select class="chosen-select input-block-level" onchange="Sort(this.value,0)" id="hienthi" name="hienthi">
                                    <%=ViewData["select-hienthi"] %>
                                </select>
                            </div>
				    </div>
				    <div class="box-content nopadding">                     
                         
					    <table class="table table-bordered table-striped">
                            <thead>
                                <tr>   
                                    <th  class="tcenter" width="3%">STT</th>
                                     <th ">Người gửi / địa chỉ</th>   
                                    <th ">Nội dung đơn</th>  
                                   
                                     <%--<th  width="5%">Tải mẫu đơn</th>--%>     
                                    <th  width="5%">Chức năng</th>             
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
          function Sort(hienthi, donvi) {
              location.href = '/Kntc/Chuyenxuly/?hienthi=' + hienthi + '&donvi=' + donvi;
          }
          function TimKiem() {
              $("#ip_data").show().html("<tr><td colspan=6 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
              $.post("/Kntc/Ajax_Chuyenxuly_search", $("#form_search").serialize(), function (ok) {
                  $("#ip_data").html(ok);
              });
              return false;
          }
   </script>
</asp:Content>
