<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Thiết lập kỳ họp
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
				  <span>  Thiết lập   <i class="icon-angle-right"></i> </span>
			    </li>
                <li>
                    <span>  Danh sách kỳ họp</span>
				   
			    </li>
		    </ul>
		    
	    </div> 
                  <div class="function_chung">
                <a onclick="ShowPopUp('','/Thietlap/Ajax_Kyhop_add')" data-original-title="Thêm mới kỳ họp" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get">

                    <select name="select" id="select" class='chosen-select form-control' onchange="ChangeChosenSelect('select','/Thietlap/Ajax_Kyhop_search','div_change')">
                        <option selected value="0">Từ khóa tìm kiếm</option>
                            <%=ViewData["opt-kyhop"] %>												
					</select>
                    <button type="submit" class="btn_f blue"><i class="icon-search"></i></button>
                </form>
            </div>       
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-table"></i> Danh sách kỳ họp</h3>
                        <div class="pull-right box-title-header">
                            <form id="form_header">
                                <select class="chosen-select" name="iDoiTuong" id="iDoiTuong" onchange="ChangeDoiTuong()" title ="Chọn đối tượng">
                                    <%=ViewData["opt-doituong"]%>
                                </select>
                            </form>
                          </div>
				    </div>
                    

				    <div class="box-content nopadding">                     
                       
                            <table class="table table-bordered table-condensed table-striped">
                                <thead>
                                    <tr >
                                        <th width="3%" class="tcenter">STT </th> 
                                        <th class="tcenter" nowrap style="width:5%">Mã kỳ họp</th>  
                                        <th nowrap class="tcenter">Tên Khóa / Kỳ họp</th>                                        
                                        <th class="tcenter">Bắt đầu </th>
                                        <th class="tcenter">Kết thúc </th>
                                        <th class="tcenter" width="10%" class="tcenter" nowrap>Chọn mặc định </br>kỳ họp hiện tại </th>   
                                        <th class="tcenter" width="4%" nowrap>Vị trí</th>                                                 
                                        <th class="tcenter" nowrap>Áp dụng</th>     
                                        <th class="tcenter" nowrap>Chức năng</th>  
                                                                             
                                    </tr>
                                </thead>
                                <tbody id="div_change">
                                    <%=ViewData["list"].ToString() %>
                                </tbody>
                         </table> 
					                           
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
   <script type="text/javascript">
       function ChonKyHop(id, post, url) {
           $.post(url, post, function (data) {
               $(".chontrung").removeClass("trans_func").addClass("f-grey");
               if (data == 1) {//Chọn
                   $("#btn_" + id).addClass("trans_func").removeClass("f-grey");
               } 
               AlertAction("Chọn mặc định khóa họp thành công!");
           });
       }
       function ChangeDoiTuong() {
           var pramt = $("#form_header").serialize();
           //window.location = "/Kiennghi/Moicapnhat/?" + $("#form_").serialize();
           //alert(window.location);
           location.href = "/Thietlap/Kyhop/?" + pramt;
           //location.href = "/Kiennghi/Moicapnhat/?iDoan=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "";
       }
   </script>
</asp:Content>
