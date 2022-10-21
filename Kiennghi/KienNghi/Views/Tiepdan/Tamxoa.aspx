<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Tiếp công dân vụ việc xóa tạm thời
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Tiepdan") %>
 
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
				    <span> Tiếp công dân   <i class="icon-angle-right"></i>  </span>
			    </li>
                <li>
				    <span>Vụ việc tiếp dân đã xóa tạm thời</span>
			    </li>
		    </ul>
		    
	    </div>  
           <div id="search_place"></div>            
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-time"></i>Vụ việc tiếp nhận qua tiếp dân thường xuyên đang xóa tạm thời</h3>
                        <ul class="tabs">
                            
                            
                        </ul>
				    </div>
				    <div class="box-content nopadding">                     
                       
					        <form id="form_" onsubmit="return false;">  
					        <table class="table table-bordered table-condensed nomargin table-striped">
                                <thead>
                                    <tr>   
                                         <th nowrap class="tcenter" width="3%">STT</th>
                                         <th nowrap style="width:10%;text-align:center" >Ngày tiếp</th>
                                           <th nowrap style="width:30%">Người gửi / Địa chỉ</th>   
                                        <th nowrap  style="width:30%">Nội dung vụ việc / Người tiếp</th>
                                     
                                       
                                        <th nowrap class="tcenter" width="8%">Chức năng</th>                                         
                                    </tr>
                                </thead>
                                <tbody id="ketqua_tracuu">     
                                    <%= ViewData["ketqua"] %>                                                  
                                    <%=ViewData["list"] %>
                                </tbody> 
                                <%=ViewData["phantrang"] %>                           
                            </table>      
                        </form>      
                                          
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script type="text/javascript">
        function Return_Confirm(id, post, url, str_confirm) { //Xóa nhanh
            var alert_confirm = str_confirm;
            if (alert_confirm == "") { alert_confirm = "Bạn có muốn khôi phục vụ việc hay không?"; }
            $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                               '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                                '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Xác nhận khôi phục</h3>' +
                                ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
                                '</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                                 '<p>' + alert_confirm + '</p>' +
                                 '<div class="form-actions nomagin tright"><button type="button" onclick="Return_TraCuu(\'' + post + '\',\'' + url + '\')" class="btn btn-primary">Đồng ý</button>' +
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
        function Return_TraCuu(post, url) {
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
        function TimKiem(val) {
            $("#ketqua_tracuu").show().html("<tr><td colspan='8' class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            $.post("/Tiepdan/Ajax_TDVuViec_search/" + val + "", function (ok) {
                $("#ketqua_tracuu").html(ok);
            });
            return false;
        }


    </script>
</asp:Content>
