<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Quản lý danh sách đại biểu quốc hội
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
                    <span>   Thiết lập   <i class="icon-angle-right"></i> </span>
				   
                   
			    </li>
                <li>
				     <span> Danh sách đại biểu quốc hội</span>
			    </li>
		    </ul>
		    
	    </div>
        <div class="function_chung">
                <a onclick="ShowPopUp('','/Thietlap/Ajax_Daibieu_add')" data-original-title="Thêm mới đại biểu" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="post" onsubmit="return false;">

                    <select name="select" id="select" class='chosen-select form-control'>
                        <option selected value="0">Từ khóa tìm kiếm</option>
                            <%=ViewData["Option_daibieu"] %>												
					</select>
                    <button type="submit" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                </form>
            </div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-user-md"></i>Danh sách đại biểu</h3>
                        <div class="pull-right box-title-header">
                            <form id="form_header">
                                <div class ="row-fluid">
                                    <select class="chosen-select" name="iLoai" id="iLoai" onchange="ChangeDonVi_KyHop()">
                                       <%= ViewData["Option_LoaiDaiBieu"]%>
                                 </select>   
                                 <select class="chosen-select" name="iKyHop" id="iKyHop" onchange ="ChangeDonVi_KyHop()">
                                        <%=ViewData["khoa"] %>
                                        </select>
                                </div>
                                
                            </form>
                        </div>
				    </div>
                    
				    <div class="box-content nopadding">
					    <table class="table table-bordered table-condensed table-striped ">
                            <thead>
                                <tr>   
                                    <th nowrap="true" class="tcenter" width="5%">STT</th>
                                    <th class="tcenter" style="width:7%" >Mã đại biểu</th>              
                                    <th  nowrap="true" >Thông tin đại biểu</th>                                 
                                      <th  nowrap="true" width="15%">Cơ quan/Nơi làm việc</th>   
                                      <th  nowrap="true" width="20%">Chức vụ đầy đủ</th>  
                                    <th   width="10%"  class="tcenter">Loại đại biểu</th> 
                                     <th  nowrap="true" width="10%"  class="tcenter">Khóa</th>   
                                    <th  nowrap="true" class="tcenter" width="4%">Vị trí</th>              
                                    <th  nowrap="true" class="tcenter" width="5%">Áp dụng</th>                                                            
                                    <th  nowrap="true" class="tcenter" width="8%">Chức năng</th>
                                </tr>
                            </thead>
                            <tbody id="div_change">                          
                                <%=ViewData["list"] %>
                            </tbody>
                            <%= ViewData["phantrang"] %>
                        </table>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
     <script>

         function ChangeKyHop(val) {
             debugger;
             $.post("<%=ResolveUrl("~")%>Thietlap/Ajax_LoadKyHopTheoDaiBieu", "iLoaiDaiBieu=" + val,
                 function (data) {
                     $("#iKyHop option").remove();
                     $("#iKyHop").html(data);
                     $('#iKyHop').trigger('liszt:updated');
                 });
         }
         function ChangeDonVi_KyHop() {
             var pramt = $("#form_header").serialize();
             //window.location = "/Kiennghi/Moicapnhat/?" + $("#form_").serialize();
             //alert(window.location);
             location.href = "/Thietlap/Daibieu/?" + pramt;
             //location.href = "/Kiennghi/Moicapnhat/?iDoan=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "";
         }

         function TimKiem() {
             var daibieu = $("#select").val();
             window.location = "/Thietlap/Daibieu/?q=" + daibieu;
             return false;

         }
     </script>
</asp:Content>
