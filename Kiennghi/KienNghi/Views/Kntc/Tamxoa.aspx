<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Danh sách đơn khiếu nại tố cáo đang tạm xóa
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
				    <span>Khiếu nại tố cáo   <i class="icon-angle-right"></i></span>
			    </li>
                <li>
                   <span>Đơn đang tạm xóa</span>
			    </li>
		    </ul>
		    
	    </div> 
        <div class="function_chung">
                <form class="search" id="form_search" method="get" onsubmit="return false;">
                    <%--<%=ViewData["select-donvi"] %>--%>
                    <select id="iKyHop" name="iKyHop" class="chosen-select">
                            <option value="0">Chọn khóa họp</option>
                        <%=ViewData["opt-kyhop"] %>
                    </select>
                    <input type="text" name="ip_noidung" id="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" value="<%=ViewData["ip_noidung"] %>" placeholder="Nội dung đơn, người gửi đơn...">
                        <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                 
                        <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kntc/Ajax_Tamxoa_formsearch/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                        <button type="button" title="Tìm kiếm" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>

                </form>
            </div>     
         <div id="search_place"></div>    
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-time"></i>Danh sách đơn đang tạm xóa</h3>
                        
				    </div>
				    <div class="box-content nopadding">                     
                        
					        <table class="table table-bordered table-striped">
                                <thead>
                                    <tr>   
                                        <th  class="tcenter" style="width:5%">STT</th>
                                       <th style="width: 25%">Người nộp/Địa chỉ người nộp</th>
                                        <th style="width: 35%" ">Nội dung đơn</th>   
                                        <th style="width: 10%"  class="tcenter">Chức năng</th>    
                                    </tr>
                                </thead>
                                <tbody id="ip_data">
                                <%=ViewData["list"] %>
                                <%=ViewData["phantrang"] %> 
                            </tbody>
                        </table>     
                        <div style="display: none;" id="loadData" class="tcenter"><img src='/Images/ajax-loader.gif' /></div>     
                                 
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script>
        $(window).on("pageshow", function (event) {
            HidePageLoading();
        });
        function TimKiem() {

            //location.href = "/Kntc/Tamxoa/?ikhoa=" + $("#iKyHop").val() + "&ip_noidung=" + $("#ip_noidung").val();
            $("#ip_data").empty().html("");
            $('#loadData').show();
            $.ajax({
                type: "post",
                url: "<%=Url.Action("Tamxoa", "Kntc")%>",
                  data: { ikhoa: $("#iKyHop").val(), ip_noidung: $("#ip_noidung").val(), hidNormalSearch: 1 },
                  success: function (res) {
                      if (res) {
                          $('#loadData').hide();
                          $("#ip_data").empty().html(res.data);
                      } else {
                          $('#loadData').hide();
                          alert("Lỗi tìm kiếm đơn đang tạm xóa!");
                      }
                  }
              });
            return false;

        }
        function DeletePage_Confirm_TraCuu(id, post, url, str_confirm, type) { //Xóa nhanh
            var alert_confirm = str_confirm;
            if (alert_confirm == "") { alert_confirm = "Bạn có muốn xóa hay không?"; }
            $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                               '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                                '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Xác nhận ' + type + '</h3>' +
                                ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
                                '</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                                 '<p>' + alert_confirm + '</p>' +
                                 '<div class="form-actions nomagin tright"><button type="button" onclick="ConfirmDelete_TraCuu(\'' + post + '\',\'' + url + '\')" class="btn btn-primary">Đồng ý</button>' +
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
        function ConfirmDelete_TraCuu(post, url) {
            //ShowPopUp("error=Đã có lỗi xảy ra trong quá trình xóa dữ liệu, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
            $.post(sitename + url, post, function (data) {
                if (data == 1) {
                    location.reload();
                } else {
                    //alert(data);
                    ShowPopUp("error=Đã có lỗi xảy ra trong quá trình xóa dữ liệu, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
                }
            });

        }
    </script>
</asp:Content>
