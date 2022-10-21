<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tra cứu văn bản 
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
                        <span>Tra cứu văn bản</span>
                    </li>
                </ul>
                <div class="close-bread">
                    <a href="#"><i class="icon-remove"></i></a>
                </div>
            </div>

            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Tra cứu văn bản</h3>

                        </div>
                        <div class="box-content nopadding">
                            
                           <form id="form_" name="form_" method="post"  onsubmit="return CheckForm();">
                                
                                <table class="table table-bordered form4">
                                    <tr>
                                        <td>Từ khóa</td>
                                        <td colspan="3">
                                            <input type="text" class="input-block-level" name="tukhoa" id="tukhoa"  autofocus /></td>
                                    </tr>
                                    <tr>
                                        <td>Ngày ban hành</td>
                                        <td>
                                            <input type="text" name="dTuNgay" id="dTuNgay" class="input-medium datepick" placeholder="Từ ngày" onchange="CompareDate('dTuNgay','dDenNgay')" />
                                            &nbsp;&nbsp;&nbsp;
                                        <input type="text" name="dDenNgay" id="dDenNgay" class="input-medium datepick" placeholder="Đến ngày" onchange="CompareDate('dTuNgay','dDenNgay')" />
                                        </td>
                                        <td nowrap>Loại văn bản</td>
                                        <td>
                                            <select name="iLoai" class="input-block-level chosen-select">
                                                <option value="-1">- - - Chọn tất cả</option>
                                                
                                                <%=ViewData["opt-loai"] %>
                                            </select>
                                        </td>
                                    </tr>



                                    <tr>
                                        <td nowrap>Đơn vị ban hành</td>
                                        <td>
                                            <select name="iDonvi" class="input-block-level chosen-select">

                                                <option value="-1">- - - Chọn tất cả</option>
                                                
                                                <%=ViewData["otp-donvi"] %>
                                            </select>
                                        </td>

                                        <td nowrap>Lĩnh vực</td>
                                        <td>
                                            <select name="iLinhVuc" class="input-block-level chosen-select">
                                                <option value="-1">- - - Chọn tất cả</option>
                                                
                                                <%=ViewData["opt-linhvuc"] %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Trạng thái văn bản</td>
                                        <td>
                                            <select name="iTrangthai" id="iTrangthai" class="input-block-level chosen-select">

                                                <option value="2">- - - Chọn tất cả</option>
                                                 <option value="1">- - - Văn bản đã ban hành</option>
                                                 <option value="0">- - - Văn bản đang soạn thảo</option>
                                                 <option value="-1">- - - Văn bản hết hiệu lực</option>
                                            </select>
                                        </td>

                                          <td class="">Kỳ họp</td>
                                         <td>  <select name="iKyhop" id="iKyhop" class="input-block-level chosen-select" >
                                                <option value="0">- - -  Chọn tất cả</option>
                                                <%=ViewData["opt-kyhop"] %>
                                            </select></td>
                                    </tr>

                                    <tr>
                                        <td colspan="4" >
                                            <button type="submit" class="btn btn-success" style="float:right">Tra cứu</button>
                                          
                                        </td>
                                    </tr>
                                </table>

                            </form>
                        </div>
                    </div>
                </div>
            </div>
            <br />
           <div class="row-fluid" id="ip_data">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i>Danh sách văn bản tìm kiếm </h3>
                        
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
                            <tbody >                          
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
       

        function reload() { location.reload();}
       <%-- function CheckForm() {
            
            $("#ketqua_tracuu").show().html("<p class='tcenter'><img src='/Images/ajax-loader.gif'/></p>");
            var frm = $("#form_");
            var data = frm.serializeArray();

            $.ajax({
                type: "POSt",
                headers: getHeaderToken(),
                contentType: "application/json; charset=utf-8",
                url: "<%=ResolveUrl("~")%>Vanban/Ajax_Tracuu_result",
                data: data,
                success: function (ok) {
                    
                   
                    $("#ketqua_tracuu").html(ok);
                    
                }
            });
            return false;
        }--%>
    </script>
    <script>
        function CheckForm() {
            var tentimkiem = $("#form_").serialize();
                window.location = "/Vanban/Tracuu/?" + tentimkiem
           
            //$("#ip_data").show().html("<div class='tcenter'> <p ><img src='/Images/ajax-loader.gif'/></p></div>");
            //$.post("/Vanban/Ajax_Tracuu_result", $("#form_").serialize(), function (ok) {
            //    $("#ip_data").html(ok);
            //});
            return false;
        }
</script>
    <script>
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

        function UpdateTrangthai_ChuyenMoi(id, post, url, str_confirm) { //Xóa nhanh
            var alert_confirm = str_confirm;
            if (alert_confirm == "") { alert_confirm = "Bạn có muốn xóa hay không?"; }
            $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                               '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                                '<div class="box box-color"><div class="box-title"><h3><i class="icon-info"></i> Xác nhận chuyển văn bản công bố</h3>' +
                                ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
                                '</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                                 '<p>' + alert_confirm + '</p>' +
                                 '<div class="form-actions nomagin tright"><button type="button" onclick="ConfirmUpdateTrangthai_ChuyenMoi(\'' + post + '\',\'' + url + '\')" class="btn btn-primary">Đồng ý</button>' +
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
        function ConfirmUpdateTrangthai_ChuyenMoi(post, url) {
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
        function UpdateTrangthai_HetHieuLuc(id, post, url, str_confirm) { //Xóa nhanh
            var alert_confirm = str_confirm;
            if (alert_confirm == "") { alert_confirm = "Bạn có muốn xóa hay không?"; }
            $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                               '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                                '<div class="box box-color"><div class="box-title"><h3><i class="icon-info"></i> Xác nhận chuyển văn bản công bố</h3>' +
                                ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
                                '</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                                 '<p>' + alert_confirm + '</p>' +
                                 '<div class="form-actions nomagin tright"><button type="button" onclick="ConfirmUpdateTrangthai_HetHieuLuc(\'' + post + '\',\'' + url + '\')" class="btn btn-primary">Đồng ý</button>' +
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
        function ConfirmUpdateTrangthai_HetHieuLuc(post, url) {
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
