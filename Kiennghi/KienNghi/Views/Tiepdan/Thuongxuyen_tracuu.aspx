<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tra cứu vụ việc tiếp nhận qua tiếp công dân
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Tiepdan") %>
    <div id="main">
         <a href="#" class="show_menu_trai">Menu trái</a>
        <div class="container-fluid">
            <div class="breadcrumbs">
                <ul>
                    <li>
                        <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i>Trang chủ</a>
                        <i class="icon-angle-right"></i>
                    </li>
                      <li>
				    <span> Tiếp công dân   <i class="icon-angle-right"></i>  </span>
			    </li>
                    <li>
                        <span>Tra cứu vụ việc tiếp nhận qua tiếp công dân</span>
                    </li>
                </ul>

            </div>
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-search"></i>Tra cứu vụ việc tiếp nhận qua tiếp công dân</h3>
                        </div>
                        <div class="box-content nopadding">



                            <form method="get" name="_form" id="_form" onsubmit="return CheckForm();">
                                <table class="table table-bordered form4">
                                    <tbody>
                                        <tr>
                                            <td class="" nowrap="">Vụ việc tiếp nhận <i class="f-red">*</i></td>
                                            <td colspan="3">
                                                <select class="input-block-level chosen-select" onchange="ChangeLoaiTiepDan(this.value)" id="iLoaiVuViec" name="iLoaiVuViec">
                                                  <%=ViewData["Opt_VuViec"] %>
                                                </select>
                                            </td>
                                        </tr>
                                    </tbody>
                                    <tbody id="">
                                        <tr>
                                            <td>Ngày tiếp</td>
                                            <td nowrap="">
                                                <input type="text" placeholder="từ ngày" class="input-medium datepick" name="dTuNgay" id="dTuNgay" onchange="CompareDate('dTuNgay','dDenNgay')" value="<%= ViewData["TuNgay"]  %>">
                                                <input type="text" placeholder="đến ngày" class="input-medium datepick" name="dDenNgay" id="dDenNgay" onchange="CompareDate('dTuNgay','dDenNgay')" value="<%= ViewData["DenNgay"]  %>">
                                            </td>
                                            <td>Đoàn đông người</td>
                                            <td>
                                                 <select name="iDoan_Tracuu" id="iDoan_Tracuu" class="input-block-level chosen-select ">
                                                     
                                                     <option value="-1" > - - - Chọn tất cả</option>
                                                      <option value="1" > - - - Đoàn đông người</option>
                                                      <option value="0" > - - - Không phải đoàn đông người</option>

                                                 </select>
                                    
                                               
                                        </tr>
                                        <tr>
                                            <td>Cơ quan tiếp</td>
                                            <td>
                                                <select name="iDonVi" id="iDonVi" class="input-block-level chosen-select ">
                                                    
                                                    <%= ViewData["opt-donvi"] %>
                                                </select></td>
                                            <td>Vụ việc trùng</td>
                                            <td> <select name="iVuViecTrung" id="iVuViecTrung" class="input-block-level chosen-select ">
                                                     <option value="-1"> - - - Chọn tất cả</option>
                                                      <option value="1"> - - - Có vụ việc trùng</option>
                                                      <option value="0"> - - - Không có vụ việc trùng</option>

                                                 </select></td>
                                        </tr>
                                        <tr>
                                            <td>Tên công dân đến</td>
                                            <td>
                                                <input type="text" class="input-block-level" name="cNguoiGui_Ten" id="cNguoiGui_Ten" value="<%= ViewData["cNguoiGui"]  %>"></td>
                                            <td>Địa chỉ công dân</td>
                                            <td>
                                                <input type="text" class="input-block-level" name="cNguoiGui_DiaChi" id="cNguoiGui_DiaChi" value="<%= ViewData["cDiaChi"]  %>"></td>
                                        </tr>
                                        <tr>
                                            <td>Tóm tắt nội dung vụ việc</td>
                                            <td colspan="3">
                                                <input type="text" class="input-block-level" name="cNoiDung" id="cNoiDung" value="<%= ViewData["cNoiDung"] %>" ></td>
                                        </tr>
                                        <tr>
                                            <td class="">Loại vụ việc</td>
                                            <td>
                                                <select name="iLoai" id="iLoai" class="input-block-level chosen-select" onchange="LoadLinhVucByLoaiDon(this.value)">
                                                    <option value="-1">- - - Chọn tất cả</option>

                                                    <%= ViewData["opt-loaidon"] %>
                                                </select>
                                            </td>
                                            <td class="">Lĩnh vực</td>
                                            <td id="ip_linhvuc">
                                                <select name="iLinhVuc" id="iLinhVuc" class="input-block-level chosen-select" onchange="LoadLinhVuc()">
                                                    <option value="-1">- - - Chọn tất cả</option>
                                                    <%=  ViewData["opt-linhvuc"] %>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Nhóm nội dung</td>
                                            <td id="LoadNoiDung">
                                                <select name="iNoiDung" id="iNoiDung"  class="input-block-level chosen-select" onclick="LoadOpTinhChat()">
                                                    <option value="-1">- - - Chọn tất cả</option>
                                                    <%= ViewData["opt-noidung"] %>
                                                </select>
                                            </td>
                                            <td>Tính chất vụ việc</td>
                                            <td id="LoadTinhChat">
                                                <select name="iTinhChat"  id="iTinhChat" class="input-block-level chosen-select" >
                                                    <option value="-1">- - - Chọn tất cả</option>
                                                    <%= ViewData["opt-tinhchat"] %>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Hình thức xử lý</td>
                                            <td>
                                                <select name="iHinhThuc" id="iHinhThuc" class="input-block-level chosen-select" onclick="Open()">
                                                    <%=ViewData["opt-hinhthuc"] %>
                                                   
                                                </select>
                                            </td>
                                            <td>Người nhập</td>
                                            <td >
                                                <select name="iTaiKhoan" id="iTaiKhoan" class="input-block-level chosen-select" >
                                                    <option value="-1">- - - Chọn tất cả</option>
                                                    <%=ViewData["opt-taikhoan"] %>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="tright">
                                              
                                                <button type="submit" class="btn btn-success"  > <i class="icon-search"></i>  Tra cứu</button>
                                                 <a onclick="Taiexcel()" class="btn btn-success"  > <i class="icon-cloud-download"></i> Tải Excel</a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </form>


                        </div>
                    </div>
                </div>
            </div>
            <div class="row-fluid" style="margin-top: 20px;" id="ketqua_tracuu">
                <div class="span12">
		<div class="box box-color box-bordered">
			<div class="box-title">
				<h3><i class="icon-time"></i>Kết quả tra cứu</h3>
                <ul class="tabs">
                    <li class="active">
                        <%=ViewData["btn-add"]  %>
                    </li>
                </ul>
			</div>
			<div class="box-content nopadding">                     
                <form id="form_" onsubmit="return false;">  
					<table class="table table-bordered table-condensed nomargin">
                        <thead>
                            <tr>   
                                <th nowrap class="tcenter" width="3%">STT</th>
                                <th nowrap  width="10%" class="tcenter" >Ngày nhận</th>
                                <th nowrap  width="17%">Người gửi / Địa chỉ</th>                                        
                                <th nowrap  width="20%">Nội dung / Người tiếp</th>  
                                <th nowrap  width="35%">Hình thức xử lý / Thông tin vụ việc</th>     
                                <th nowrap  width="10%" >Kết quả trả lời</th>  
                                 <th nowrap  width="5%" >Chức năng</th>                                            
                            </tr>
                        </thead>
                        <tbody>                                                       
                            <%=ViewData["list"] %>
                        </tbody>        
                          <%= ViewData["phantrang"] %>                              
                    </table>      
                </form>                   
			</div>
		</div>
	</div>
                
              
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function DeletePage_Confirm_TraCuu(id, post, url, str_confirm) { //Xóa nhanh
            var alert_confirm = str_confirm;
            if (alert_confirm == "") { alert_confirm = "Bạn có muốn xóa hay không?"; }
            $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                               '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                                '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Xác nhận xóa</h3>' +
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
        ChangeLoaiTiepDan("Dinhky");
        function ChangeLoaiTiepDan(val) {
            $.post("/Tiepdan/Ajax_" + val + "_tracuu", "", function (data) {
                $("#loaitiepnhan").html(data);
            });
        }
        function CheckForm() {
          
            var tentimkiem = $("#_form").serialize();
            window.location = "/Tiepdan/Thuongxuyen_tracuu/?" + tentimkiem
            //$("#ketqua_tracuu").show().html("<p class='tcenter'><img src='/Images/ajax-loader.gif'/></p>");
            //var loaitiepdan = $("#iLoaiVuViec").val();
            //$.post("/Tiepdan/Ajax_TRACUUNANGCAO", $("#_form").serialize(), function (ok) {
            //    $("#ketqua_tracuu").html(ok);
            //});
            return false;

        }
        function Taiexcel() {
            window.location = "/Baocaotiepdan/Download_exceltracuu/?dTuNgay=" + $("#dTuNgay").val() + "&dDenNgay=" + $("#dDenNgay").val() + "&iDonVi=" + $("#iDonVi").val() + "&iLoaiVuViec=" + $("#iLoaiVuViec").val() + "&iDoan_Tracuu=" + $("#iDoan_Tracuu").val()
            + "&cNguoiGui_Ten=" + $("#cNguoiGui_Ten").val() + "&cNguoiGui_DiaChi=" + $("#cNguoiGui_DiaChi").val() + "&cNoiDung=" + $("#cNoiDung").val()
             + "&iLoai=" + $("#iLoai").val() + "&iLinhVuc=" + $("#iLinhVuc").val() + "&iNoiDung=" + $("#iNoiDung").val()
             + "&iTinhChat=" + $("#iTinhChat").val() + "&iHinhThuc=" + $("#iHinhThuc").val() + "&iVuViecTrung=" + $("#iVuViecTrung").val() + "&iTaiKhoan=" + $("#iTaiKhoan").val();
          
        }
        function Open() {
            $.post("/Tiepdan/Ajax_Thongtinketqua", $("#_form").serialize(), function (ok) {
                $("#phanloai").html(ok);
                $("#title").html("Kết quả trả lời");
                $("#iketquatraloi").chosen();
            });
            return false;
            
        }
        function LoadLinhVucByLoaiDon(val) {
            $.post("<%=ResolveUrl("~")%>Tiepdan/Ajax_LoadLinhVucByLoaiDon_TraCuu", "iLoaiDon=" + val,
        function (data) {
            $("#ip_linhvuc").html(data);
            $("#iLinhVuc").chosen();
        });
        }
        function LoadLinhVuc() {
            if ($("#iLinhVuc").val() != -1) {
                $.post("<%=ResolveUrl("~")%>Tiepdan/Ajax_LoadLinhVucNoiDung_TraCuu",
                "iLinhVuc=" + $("#iLinhVuc").val(),
                function (data) {
                    $("#LoadNoiDung").html(data);
                    $("#iTinhChat").chosen();
                    $("#iNoiDung").chosen();
                }
            );
            }
        }
        function LoadOpTinhChat() {
            if ($("#iNoiDung").val() != -1) {
                $.post("<%=ResolveUrl("~")%>Tiepdan/Ajax_LoadTinhChatNoiDung_TraCuu",
                 "iNoiDung=" + $("#iNoiDung").val(),
                 function (data) {
                     $("#LoadTinhChat").html(data);
                     $("#iTinhChat").chosen();
                 }
             );
            }
        }
    </script>
</asp:Content>
