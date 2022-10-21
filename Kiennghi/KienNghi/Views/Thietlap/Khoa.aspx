<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Thiết lập khóa
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
				       <span> Thiết lập   <i class="icon-angle-right"></i> </span>
			    </li>
                <li>
                     <span> Danh sách khóa</span>
				
			    </li>
		    </ul>
		    
	    </div>  
                  <div class="function_chung">
                <a <%=ViewData["bt_add"] %> onclick="ShowPopUp('','/Thietlap/Ajax_Khoa_add')" data-original-title="Thêm mới khóa" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get">

                    <select name="select" id="select" class='chosen-select form-control' onchange="ChangeChosenSelect('select','/Thietlap/Ajax_Khoa_search','div_change')">
                        <option selected value="0">Từ khóa tìm kiếm</option>
                            <%=ViewData["opt-khoa"] %>												
					</select>
                    <button type="submit" class="btn_f blue"><i class="icon-search"></i></button>
                </form>
            </div>      
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
                            <h3><i class="icon-th-large"></i> Danh sách khóa</h3>
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
                                            <th class="tcenter" nowrap width="5%">Mã khóa</th>          
                                        <th nowrap class="tcenter">Tên khóa </th>
                                        <th class="tcenter">Bắt đầu </th>
                                        <th class="tcenter">Kết thúc </th>
                                        <th class="tcenter" width="10%" class="tcenter" nowrap>Chọn mặc định khóa hiện tại </th>    
                                          <th class="tcenter" nowrap width="4%">Vị trí</th>                                                
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
       function ChonKhoaHop(id, post, url) {
           $.post(url, post, function (data) {
               $(".chontrung").removeClass("trans_func").addClass("f-grey");
               if (data == 1) {//Chọn
                   $("#btn_" + id).addClass("trans_func").removeClass("f-grey");
               } 
               AlertAction("Chọn mặc định khóa họp thành công!");
           });
       }
     
       function DeletePage_Khoa(id, post, url, str_confirm) { //Xóa nhanh
           var alert_confirm = str_confirm;
           if (alert_confirm == "") { alert_confirm = "Bạn có muốn xóa hay không?"; }
           $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                              '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                               '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Xác nhận xóa</h3>' +
                               ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
                               '</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                                '<p>' + alert_confirm + '</p>' +
                                '<div class="form-actions nomagin tright"><button type="button" onclick="ConfirmDelete_Khoa(\'' + post + '\',\'' + url + '\')" class="btn btn-primary">Đồng ý</button>' +
                                ' <button type="submit" class="btn btn-warning btn-focus" id="btn-submit">Hủy bỏ</button></div></form></div></div></div></div></div>' +
                                 ' </div></div>');
           $("#btn-submit").focus();
           //ShowPopUp(post + "&url=" + url + "&str_confirm=" + alert_confirm, "/Home/Ajax_Confirm_delete");

           //if (confirm(alert_confirm)) {
           //    $.post(sitename + url, post, function (data) {
           //        if (data == 1) {
           //            location.reload();
           //        } else {
           //            alert(data);
           //        }
           //    });
           //}
       }
       function ConfirmDelete_Khoa(post, url) {
           //ShowPopUp("error=Đã có lỗi xảy ra trong quá trình xóa dữ liệu, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
           $.post(sitename + url, post, function (data) {
               if (data == 1) {
                   window.location.href += "#delete";
                   //location.href = url;
                   location.reload();
               } else {
                   alert("Bạn không thể xóa Khóa này  vì đã có các kỳ họp bên trong.");
                
               }
           });

       }
       function ChangeDoiTuong() {
           var pramt = $("#form_header").serialize();
           //window.location = "/Kiennghi/Moicapnhat/?" + $("#form_").serialize();
           //alert(window.location);
           location.href = "/Thietlap/Khoa/?" + pramt;
           //location.href = "/Kiennghi/Moicapnhat/?iDoan=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "";
       }
   </script>
</asp:Content>
