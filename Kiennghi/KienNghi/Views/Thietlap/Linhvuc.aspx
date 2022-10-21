<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Quản lý danh mục lĩnh vực
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
				  <span> Thiết lập  <i class="icon-angle-right"></i>  </span>
			    </li>
                <li>
                     <span> Danh mục khiếu nại lĩnh vực </span>
				  
			    </li>
		    </ul>
		    
	    </div>
        <div class="function_chung">
                <a onclick="ShowPopUp('','/Thietlap/Ajax_Linhvuc_add')" data-original-title="Thêm mới lĩnh vực" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get">

                    <select name="select" id="select" class='chosen-select form-control' onchange="ChangeChosenSelect('select','/Thietlap/Ajax_Linhvuc_search','div_change')">
                        <option selected value="0">Từ khóa tìm kiếm</option>
                             <%=ViewData["option_linhvuc"] %>										
					</select>
                    <button type="submit" class="btn_f blue"><i class="icon-search"></i></button>
                </form>
            </div>
        
        <div class="row-fluid">
            <div class="span12">
                <form method="post">
                    
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tag"></i>Danh sách danh mục khiếu nại lĩnh vực</h3>
                        
				    </div>
				    <div class="box-content nopadding">
                          
                     
					    <table class="table table-bordered table-striped">
                            <thead>
                                <tr>
                                     
                                    <th nowrap class="tcenter" width="5%">Mã lĩnh vực</th>                                 
                                    <th nowrap>Tên lĩnh vực</th>    
                                     <th class="tcenter"  width="4%" nowrap>Vị trí</th>                       
                                    <th class="tcenter"  width="5%" nowrap>Áp dụng</th>                                               
                                    <th nowrap class="tcenter" width="5%">Chức năng</th>
                                </tr>
                            </thead>
                            <tbody id="div_change">                          
                               <%=ViewData["list"] %>
                            </tbody>
                        </table>
                        
				    </div>
			    </div>
                    </form>
		    </div>
	    </div>
    </div>
</div>
     <script type="text/javascript">  
         function TimKiemLV() {
             if ($("#select").val() != 0) {
                 
            $.post("<%=ResolveUrl("~")%>Thietlap/Ajax_TimKiemLV",
                "id=" + $("#select").val(),
                function (data) {
                    $("#refresh").html(data);
                }
            );
        }
    }
         
         function DeletePage_LinhVuc(id, post, url, str_confirm) { //Xóa nhanh
             var alert_confirm = str_confirm;
             if (alert_confirm == "") { alert_confirm = "Bạn có muốn xóa hay không?"; }
             $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                                '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                                 '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Xác nhận xóa</h3>' +
                                 ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
                                 '</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                                  '<p>' + alert_confirm + '</p>' +
                                  '<div class="form-actions nomagin tright"><button type="button" onclick="ConfirmDelete_LinhVuc(\'' + post + '\',\'' + url + '\')" class="btn btn-primary">Đồng ý</button>' +
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
         function ConfirmDelete_LinhVuc(post, url) {
             //ShowPopUp("error=Đã có lỗi xảy ra trong quá trình xóa dữ liệu, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
             $.post(sitename + url, post, function (data) {
                 if (data == 1) {
                     window.location.href += "#delete";
                     //location.href = url;
                     location.reload();
                 } else {
                     alert("bạn không thể xóa lĩnh vực này đã có các nội dung đơn bên trong.");
                    
                 }
             });

         }
    </script>
</asp:Content>
