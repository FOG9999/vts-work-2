<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Danh sách kiến nghị đã Import
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <%: Html.Partial("../Shared/_Left_Knct") %>
<div id="main">
              <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                <li>
                    <span>Kiến nghị cử tri <i class="icon-angle-right"></i></span>                    
                </li>
                <li>
				    <span>Danh sách kiến nghị đã Import</span>
			    </li>
		    </ul>
		    
	    </div>       
                  <div class="function_chung">
                <%--<a data-original-title=" Tạo tập hợp tương ứng của Đoàn ĐBQH" rel="tooltip" href="/Kiennghi/Download_Mau_Import/" class="add btn_f blue">
                    <i class="icon-plus-sign"></i>
                </a>
                <a data-original-title=" Xem tập hợp kiến nghị đã tạo" rel="tooltip" href="/Kiennghi/Download_Mau_Import/" class="add btn_f blue">
                    <i class="icon-list-alt"></i>
                </a>
                 <a data-original-title="Hủy tập hợp kiến nghị đã tạo" rel="tooltip" href="/Kiennghi/Download_Mau_Import/" class="add btn_f blue">
                    <i class="icon-remove"></i>
                </a>
                <a onclick="ShowPopUp('','/Kiennghi/Ajax_Import_add')" data-original-title="Import lại danh sách kiến nghị" 
                    rel="tooltip" href="javascript:void(0)" class="add btn_f blue" ><i class="icon-refresh"></i></a>
                <a data-original-title=" Hủy danh sách kiến nghị" rel="tooltip" href="/Kiennghi/Download_Mau_Import/" class="add btn_f blue">
                    <i class="icon-remove"></i>
                </a>--%>
                      <%=ViewData["tao_taphop"] %><%=ViewData["xem_taphop"] %><%=ViewData["xoa_taphop"] %><%=ViewData["import_lai"] %>
            </div> <div id="search_place"></div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered ">
				    <div class="box-title">
					    <h3><i class="icon-save"></i> Danh sách kiến nghị đã Import</h3>
				    </div>
				    <div class="box-content nopadding" style="overflow: auto; width: auto; height: 500px;">                    
                        <table class="table table-bordered table-condensed table-striped">
                            <thead>
                                <tr >
                                    <th class="tcenter">STT </th> 
                                    <th nowrap class="tcenter">Mã kiến nghị</th>   
                                    <th nowrap class="tcenter">Nội dung kiến nghị</th>   
                                    <th nowrap class="tcenter">Đơn vị tiếp nhận</th>   
                                    <th nowrap class="tcenter">Lĩnh vực</th>   
                                    <th nowrap class="tcenter">Thẩm quyền</th>                                     
                                    <th  width="5%" class="tcenter">Chức năng</th>                                     
                                </tr>
                            </thead>
                            <tbody id="q_data">
                                <%=ViewData["list"] %>
                            </tbody>
                        </table> 					                              
				    </div>
			    </div>
		    </div>
	    </div>
        <div style="margin-top:20px" class="tcenter">
            <a onclick="ShowPageLoading()" href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>
        </div>
    </div>  
        </div>
   <script type="text/javascript">
       function Tao_TapHop(id) {
           ShowPageLoading();
           
           $.post("/Kiennghi/Ajax_Import_tao_tonghop", 'id=' + id, function (data) {
               if (data == 1) {
                   location.href = "/Kiennghi/Import_taphop?id=" + id + "#success";
               } else {
                   ShowPopUp("error=Đã có lỗi xảy ra trong quá trình tạo tập hợp kiến nghị, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
               }
           });
       }
       function Huy_TapHop(id) {
           ShowPageLoading();
           $.post("/Kiennghi/Ajax_Import_huy_tonghop", 'id=' + id, function (data) {
               if (data == 1) {
                   location.reload();
               } else {
                   ShowPopUp("error=Đã có lỗi xảy ra trong quá trình hủy tập hợp kiến nghị, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
               }
           });
       }
   </script>
</asp:Content>
