<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tra cứu đơn thư
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Kntc") %>
    <div id="main" class="">
        <div class="container-fluid">
            <div class="breadcrumbs">
                <ul>
                    <li>
                        <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i>Trang chủ</a>
                        <i class="icon-angle-right"></i>
                    </li>
                    <li>
                        <span>Khiếu nại tố cáo   <i class="icon-angle-right"></i></span>
                    </li>
                    <li>
                        <span>Tra cứu đơn thư</span>
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
                            <h3><i class="icon-tags"></i>Tra cứu đơn thư</h3>
                        </div>
                        <div class="box-content nopadding">
                            <form method="get" class="nomargin" id="form_" onsubmit="return CheckForm();">

                                <table class="table table-bordered table-condensed form4">
                                    <tr>
                                        <td class="">Từ khóa</td>
                                        <td colspan="3">
                                            <input type="text" name="cNoiDung" value="<%=ViewData["cNoiDung"] %>" placeholder="Nội dung đơn, người gửi đơn..." class="input-block-level" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">Thời gian nhận đơn</td>
                                        <td style="width: 15%">
                                            <div class="input-block-level">
                                                <span class="span6 nopadding">
                                                    <input type="text" value="<%=ViewData["dTuNgay"] %>" name="dTuNgay" autocomplete="off" id="dTuNgay" class="datepick input-block-level" /></span>
                                                <span class="span6 nopadding">
                                                    <input type="text" value="<%=ViewData["dDenngay"] %>" name="dDenngay" autocomplete="off" id="dDenngay" class="datepick input-block-level" /></span>
                                            </div>
                                        </td>
                                        <td style="width: 25%">Nguồn đơn</td>
                                        <td style="width: 30%">
                                            <select class="input-block-level chosen-select" name="iNguonDon">
                                                <option value="0">- - - Chọn tất cả</option>
                                                <%=ViewData["opt-nguondon"] %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">Tình thành</td>
                                        <td>
                                            <select name="iDiaPhuong_0" id="iDiaPhuong_0" class="input-block-level chosen-select">
                                                <option value="0">- - - Chọn tất cả</option>
                                                <%=ViewData["opt-tinh"] %>
                                            </select>
                                        </td>
                                        <td>Người nhập</td>
                                        <td>
                                            <select class="input-block-level chosen-select" name="iNguoiNhap">
                                                <option value="0">- - - Chọn tất cả</option>
                                                <%=ViewData["opt-nguoinhap"] %>
                                            </select>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">Loại đơn</td>
                                        <td>
                                            <select name="iLoaiDon" id="iLoaiDon" class="input-block-level chosen-select" onchange="LoadLinhVucByLoaiDon(this.value)">
                                                <option value="0">- - - Chọn tất cả</option>
                                                <%=ViewData["opt-loaidon"] %>
                                            </select>
                                        </td>
                                        <td class="">Lĩnh vực</td>
                                        <td id="ip_linhvuc">
                                            <select name="iLinhVuc" id="iLinhVuc" class="input-block-level chosen-select">
                                                <option value="0">- - - Chọn tất cả</option>
                                                <%=ViewData["opt-linhvuc"] %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nhóm nội dung</td>
                                        <td id="ip_noidung">
                                            <select name="iNoiDung" id="iNoiDung" class="input-block-level chosen-select">
                                                <option value="0">- - - Chọn tất cả</option>
                                                <%=ViewData["opt-noidung"] %>
                                            </select>
                                        </td>
                                        <td>Tính chất vụ việc</td>
                                        <td id="ip_tinhchat">
                                            <select name="iTinhChat" id="iTinhChat" class="input-block-level chosen-select">
                                                <option value="0" selected>- - - Chọn tất cả</option>
                                                <%=ViewData["opt-tinhchat"] %>
                                            </select>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Tình trạng đơn</td>
                                        <td>
                                            <select class="input-block-level chosen-select" name="iTinhTrangXuLy">
                                                <option value="-1" selected>- - - Chọn tất cả</option>
                                                <%=ViewData["opt_trangthai"] %>
                                            </select>

                                        </td>
                                        <td>Đơn vị thụ lý</td>
                                        <td>
                                            <select name="iThamQuyenDonVi" id="iThamQuyenDonVi" class="input-block-level chosen-select">
                                                <%=ViewData["opt-donvi"] %>
                                            </select></td>
                                    </tr>

                                    <tr>
                                        <td></td>
                                        <td colspan="3" class="tright">
                                            <input type="hidden" id="hidAdvancedSearch" name="hidAdvancedSearch" value="1"/>
                                            <input type="submit" value="Tra cứu" class="btn btn-success" />
                                            <%--<span onclick="TaiExel()" class="btn btn-primary "><i class="icon-cloud-download"></i>Tải Excel</span>
                                            <span onclick="TaiDonTrung()" class="btn btn-primary "><i class="icon-cloud-download"></i>Tải đơn trùng</span>--%>
                                        </td>
                                    </tr>
                                </table>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row-fluid" style="margin-top: 20px; display: block;" id="ketqua_tracuu">
                <div class="span12"><div class="box box-color box-bordered"> 
                    <div class="box-title">
                        <h3><i class="icon-search"></i>Kết quả tra cứu</h3></div>
                    <div class="box-content nopadding">
                        <table class="table table-bordered table-condensed">
                            <thead>
                                <tr>
                                    <th style="width: 3%">STT</th>
                                    <th style="width: 25%">Người nộp/Địa chỉ người nộp</th>
                                    <th style="width: 5%">Ngày nhận</th>
                                    <th style="width: 35%">Nội dung đơn</th>
                                    <th style="width: 20%">Ghi chú</th>
                                    <th>Chức năng</th></tr>
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
    <script type="text/javascript">
        $(window).on("pageshow", function (event) {
            HidePageLoading();
        });
        function LoadLinhVucByLoaiDon(val) {
            $.post("<%=ResolveUrl("~")%>Kntc/Ajax_LoadLinhVuc", "iLoaiDon=" + val,
                 function (data) {
                     $("#ip_linhvuc").html(data);
                     $("#ip_noidung").html("<select name='iNoiDung' id='iNoiDung' class='input-medium chosen-select' style='width:100%'><option value='0'>- - - Chọn tất cả</option></select>");
                     $("#ip_tinhchat").html("<select name='iTinhChat' id='iTinhChat' class='input-medium chosen-select' style='width:100%'><option value='0'>- - - Chọn tất cả</option></select>");
                     $("#iLinhVuc").chosen();
                     $("#iNoiDung").chosen();
                     $("#iTinhChat").chosen();
                 });
        }
        function LoadNoiDungByLinhVuc(val) {
            $.post("<%=ResolveUrl("~")%>Kntc/Ajax_LoadNoiDung", "iLinhVuc=" + val,
                 function (data) {
                     $("#ip_noidung").html(data);
                     $("#iNoiDung").chosen();
                 });
        }
        function LoadTinhChatByNoiDung(val) {
            $.post("<%=ResolveUrl("~")%>Kntc/Ajax_LoadTinhChat", "iNoiDung=" + val,
                   function (data) {
                       $("#ip_tinhchat").html(data);
                       $("#iTinhChat").chosen();
                   });
        }
        function TaiExel() {
            window.location = "/Kntc/Search_Exl/?" + $("#form_").serialize();
        }
        function TaiDonTrung() {
            window.location = "/Kntc/Dontrung_Exl/?" + $("#form_").serialize();
        }
        function CheckForm() {
            var pramt = $("#form_").serialize();
            // window.location = "/Kntc/Khongdudieukien/?" + $("#form_").serialize();
            //alert(window.location);
            //location.href = "/Kntc/Tracuu/?" + pramt;
            $("#ip_data").empty().html("");
            $('#loadData').show();
            $.ajax({
                type: "post",
                url: "<%=Url.Action("Tracuu", "Kntc")%>",
                data: pramt,
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm kiến nghị mới cập nhật!");
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
        function LoadLinhVucByLoaiDon(val) {
            $.post("<%=ResolveUrl("~")%>Baocaokntc/Ajax_LoadLinhVucByLoaiDon", "iLoaiDon=" + val,
                function (data) {
                    $("#ip_linhvuc").html(data);
                    $("#iLinhVuc").chosen();
                });
        }
    </script>
</asp:Content>
