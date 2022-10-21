<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Văn bản đang soạn thảo
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <%: Html.Partial("../Shared/_Left_Vanban") %>
<div id="main" class="">
     <a href="#" class="show_menu_trai">Menu trái</a>
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>">Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                   <li>
				    <span> Văn bản công bố   <i class="icon-angle-right"></i>  </span>
			    </li>
                <li>
				    <span>Văn bản đang soạn thảo</span>
			    </li>
		    </ul>
		    
	    </div>
        <div class="function_chung">

                <a href="/Vanban/Themmoi" data-original-title="Thêm mới" rel="tooltip" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                  <form class="search" id="form_search" method="get" onsubmit="return false;">
             
                 <input type="text" name="ip_noidung" id="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" value="" placeholder="Nội dung văn bản ...">
                        <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>  
                     <button type="button" title="Tìm kiếm"onclick="ShowTimKiem_Conf('type= 0','/Vanban/Ajax_Vanban_search/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                        <button type="button" title="Tìm kiếm" onclick="HideTimKiem('search_place')" class="btn_f blue"  id="hidebut" style="display:none" ><i class="icon-zoom-out"></i></button>
                </form>
            </div>
         <div id="search_place"></div> 
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i>Danh sách văn bản đang soạn thảo</h3>
				    </div>
				    <div class="box-content nopadding">
					    <table class="table table-bordered table-striped">
                            <thead>
                                <tr>                          
                                     <th nowrap width="3%">STT</th>   
                                    <th nowrap width="67%" >Thông tin văn bản</th>   
                                    <th class="tcenter"  width="5%" nowrap>File</th>                                                    
                                    <th nowrap class="tcenter" width="15%">Chức năng</th>
                                </tr>
                            </thead>
                            <tbody id="ip_data">                          
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
        function TimKiem() {
            //$("#ip_data").show().html("<tr><td colspan=4  class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            //$.post("/Vanban/Ajax_Vanbanxoanthao_Timkiem", $("#form_search").serialize(), function (ok) {
            //    $("#ip_data").html(ok);
            //});
            var tentimkiem = $("#ip_noidung").val();
            window.location = "/Vanban/Moicapnhat/?q=" + tentimkiem;
            return false;
        }
    </script>
    <script type="text/javascript">
       
        //function UpdateTrangthai_Duyet_VanBan(post, url) {
        //    //alert(post+url);
        //    if (confirm("Bạn có thật sự muốn duyệt văn bản này?")) {
        //        $.post(sitename + url, post, function (data) {
        //            if (data == 1) {
        //                location.reload();
        //            } else {
        //                alert(data);
        //            }
        //        });
        //    }
        //}
        function UpdateTrangthai_Duyet_VanBan(id, post, url, str_confirm) { //Xóa nhanh
            var alert_confirm = str_confirm;
            if (alert_confirm == "") { alert_confirm = "Bạn có muốn xóa hay không?"; }
            $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                               '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                                '<div class="box box-color"><div class="box-title"><h3><i class="icon-info"></i> Xác nhận duyệt văn bản công bố</h3>' +
                                ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
                                '</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                                 '<p>' + alert_confirm + '</p>' +
                                 '<div class="form-actions nomagin tright"><button type="button" onclick="ConfirmUpdateTrangthai_Duyet_VanBan(\'' + post + '\',\'' + url + '\')" class="btn btn-primary">Đồng ý</button>' +
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
        function ConfirmUpdateTrangthai_Duyet_VanBan(post, url) {
            //ShowPopUp("error=Đã có lỗi xảy ra trong quá trình xóa dữ liệu, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
            $.post(sitename + url, post, function (data) {
                if (data == 1) {
                    window.location.href += "#success";
                    //location.href = url;
                    location.reload();
                } else {
                    //alert(data);
                    ShowPopUp("error=Đã có lỗi xảy ra trong quá trình thực hiện.", "/Home/Ajax_Error_ajax_submit/");
                }
            });

        }

    </script>
</asp:Content>

