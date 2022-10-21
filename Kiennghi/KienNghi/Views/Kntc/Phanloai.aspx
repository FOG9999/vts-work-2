<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Phân loại đơn thư
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Kntc") %>
    <div id="main" class="">
        <a href="#" class="show_menu_trai">Menu trái</a>
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
                        <span>Phân loại đơn thư</span>
                    </li>
                </ul>
                <div class="close-bread">
                    <a href="#"><i class="icon-remove"></i></a>
                </div>
            </div>
            <% KNTC_DON don = (KNTC_DON)ViewData["don"];
                KNTC d = (KNTC)ViewData["don_detail"];%>
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Phân loại đơn thư</h3>
                            <input type="hidden" name="formchuyentiep" value="<%=ViewData["type"] %>" id="formchuyentiep" class="input-block-level" />
                        </div>
                        <div class="box-content" style="text-align: left;">
                            <form method="post" id="form_" class="form-horizontal form-column" enctype="multipart/form-data">
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Ngày nhận đơn </label>
                                            <div class="controls">
                                                <!-- Ngày nhận đơn-->
                                                <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(don.DNGAYNHAN)) %>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Nguồn đơn </label>
                                            <div class="controls">
                                                <!-- Nguồn đơn-->
                                                <%=d.nguondon %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Người nộp đơn </label>
                                            <div class="controls">
                                                <p><%=Server.HtmlEncode(don.CNGUOIGUI_TEN) %>  </p>
                                                <% if (Convert.ToInt16(don.IDOANDONGNGUOI) == 1)
                                                    {%>     
                                        Đoàn đông người (<%=don.ISONGUOI%> người)
                                         <% } %>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Số CMND </label>
                                            <div class="controls">
                                                <%=Server.HtmlEncode(don.CNGUOIGUI_CMND) %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Địa chỉ người nộp đơn </label>
                                            <div class="controls">
                                                <%=d.diachi_nguoinop %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Quốc tịch</label>
                                            <div class="controls">
                                                <%=d.quoctich %>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Dân tộc</label>
                                            <div class="controls">
                                                <%=d.dantoc %>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Tóm tắt nội dung</label>
                                            <div class="controls">
                                                <%=Server.HtmlEncode(don.CNOIDUNG) %>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">File đính kèm</label>
                                            <div class="controls">
                                                <%=ViewData["file"] %>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Loại đơn <span class="f-red">*</span></label>
                                            <div class="controls">
                                                <select name="iLoaiDon" id="iLoaiDon" class="chosen-select" onchange="LoadLinhVucByLoaiDon(this.value)">
                                                    <option value="0">--- Chọn loại đơn</option>
                                                    <%=ViewData["opt-loaidon"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Lĩnh vực <span class="f-red">*</span></label>
                                            <div class="controls" id="ip_linhvuc">
                                                <select name="iLinhVuc" id="iLinhVuc" class="input-block-level chosen-select" onchange="LoadLinhVuc()">
                                                    <option value="-1">--- Chọn lĩnh vực</option>
                                                    <option value="0">Chưa xác định</option>
                                                    <%=ViewData["opt-linhvuc"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Nội dung đơn</label>
                                            <div class="controls" id="LoadNoiDung">
                                                <select name="iNoiDung" id="iNoiDung" class="input-block-level chosen-select" onchange="LoadOpTinhChat()">
                                                    <option value="-1">--- Chọn nội dung</option>
                                                    <option value="0">Chưa xác định</option>
                                                    <%=ViewData["opt-noidung"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Tính chất vụ việc</label>
                                            <div class="controls" id="LoadTinhChat">
                                                <select name="iTinhChat" id="iTinhChat" class="input-block-level chosen-select">
                                                    <option value="-1">--- Chọn tính chất</option>
                                                    <option value="0">Chưa xác định</option>
                                                    <%=ViewData["opt-tinhchat"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Thẩm quyền xử lý <span class="f-red">*</span></label>
                                            <div class="controls" id="">
                                                <%--<select name="iThuocThamQuyen" id="iThuocThamQuyen" onchange="ChangeThamQuyen(this.value)" class="input-block-level chosen-select">--%>
                                                    <%--<option value="-1">--- Vui lòng chọn</option>--%>
                                                    <%--<option value="1" <% if (Convert.ToInt16(don.ITHAMQUYEN) == 1) { Response.Write("selected"); } %>>Đơn thuộc lĩnh vực phụ trách</option>--%>
                                                <%--</select>--%>
                                                <input type="hidden" name="iThuocThamQuyen" value="1" id="iThuocThamQuyen" class="input-block-level" />
                                                <input type="text" name="iThuocThamQuyen_text" value="Đơn thuộc lĩnh vực phụ trách" id="iThuocThamQuyen_text" disabled class="input-block-level" autofocus />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6" id="thuocthamquyen">
                                    <%--<div class="span6" id="thuocthamquyen" style="<% if (Convert.ToInt16(don.ITHAMQUYEN) !=1) { Response.Write("display:none"); } %>">--%>
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Điều kiện xử lý <span class="f-red">*</span></label>
                                            <div class="controls" id="ip_dieukien">
                                                <select name="iDuDieuKien" id="iDuDieuKien" class="input-block-level chosen-select" onchange="HinhThucXuLy(this.value)">
                                                    <option value="-1" <% if (Convert.ToInt32(don.IDUDIEUKIEN) == -1) { Response.Write("selected"); } %>>Chưa xác định</option>
                                                    <option value="1" <% if (Convert.ToInt32(don.IDUDIEUKIEN) == 1) { Response.Write("selected"); } %>>Đủ điều kiện xử lý</option>
                                                    <option value="0" <% if (Convert.ToInt32(don.IDUDIEUKIEN) == 0) { Response.Write("selected"); } %>>Không đủ điều kiện xử lý</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row-fluid" id="hinhthuc">
                                    <div class="span6" id="iSoluongnguoi">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Theo số lượng người</label>
                                            <div class="controls">
                                                <select name="id_Soluongnguoi" id="id_Soluongnguoi" class="input-block-level chosen-select">
                                                    <option value="0" <% if (Convert.ToInt32(don.IPLSONGUOI) == 0) { Response.Write("selected"); } %>>Đơn có họ tên chữ ký của một người</option>
                                                    <option value="1" <% if (Convert.ToInt32(don.IPLSONGUOI) == 1) { Response.Write("selected"); } %>>Đơn có họ tên chữ ký của 02 người trở lên</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6" id="hinhthucxuly">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Hình thức xử lý <span class="f-red">*</span></label>
                                            <div class="controls" id="ip_hinhthucxuly">
                                                <select name="iHinhThuc" id="iHinhThuc" class="input-block-level chosen-select">
                                                    <% if (Convert.ToInt32(don.IDUDIEUKIEN) == 1)
                                                       {%>
                                                        <option <% if (Convert.ToInt32(don.IDUDIEUKIEN_KETQUA) == -1) { Response.Write("selected"); } %> value="-1">Đang nghiên cứu</option>
                                                        <option <% if (Convert.ToInt32(don.IDUDIEUKIEN_KETQUA) == 1) { Response.Write("selected"); } %> value="1">Chuyển đơn</option>
                                                        <option <% if (Convert.ToInt32(don.IDUDIEUKIEN_KETQUA) == 2) { Response.Write("selected"); } %> value="2">Hướng dẫn giải thích, trả lời</option>
                                                        <option <% if (Convert.ToInt32(don.IDUDIEUKIEN_KETQUA) == 0) { Response.Write("selected"); } %>value="0">Không chuyển</option>
                                                        <%}
                                                       else if (Convert.ToInt32(don.IDUDIEUKIEN) == 0)
                                                       { %>
                                                        <option <% if (Convert.ToInt32(don.IDUDIEUKIEN_KETQUA) == -1) { Response.Write("selected"); } %> value="-1">Đang nghiên cứu</option>
                                                        <option <% if (Convert.ToInt32(don.IDUDIEUKIEN_KETQUA) == 1) { Response.Write("selected"); } %> value="1">Lưu theo dõi</option>
                                                        <%} 
                                                    %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    
                                </div>
                                <div class="row-fluid" style="margin-top: 1%" id ="mota">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="iMoTa" class="control-label ">Mô tả</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea class="input-block-level" name="iMoTa" id="iMoTa"><%=don.CHITIETLYDO_LUUTHEODOI %></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <div class="controls" id="">
                                                <input type="hidden" name="iDontrung" id="iDontrung" value="<%=ViewData["iddontrung"] %>" />
                                                <input type="hidden" name="id" id="id" value="<%=ViewData["id_encrypt"] %>" />
                                                <a onclick="CheckForm()" class="btn btn-success">Lưu và xử lý</a>
                                                <%--<a href="<%=Request.Cookies["url_return"].Value %>" onclick="ShowPageLoading()" class="btn btn-warning">Quay lại</a>--%>
                                                <a href="#" onclick="window.history.back();" class="btn btn-warning">Quay lại</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            $(window).on("pageshow", function (event) {
                HidePageLoading();
            });
            window.onload = () => {
                if ($("#iMoTa").text() == "") {
                    $("#mota").hide();
                }
                if ($('#id').val()) {
                    HinhThucXuLy(parseInt($('#iDuDieuKien').val()), true)
                } else {
                    ChangeThamQuyen(1);
                }
            }

            function KiemTrung() {
                $("#dontrung").show().html("<td colspan='4' class='tcenter'><img src='/Images/ajax-loader.gif' /></td>");
                $.post("/Kntc/Ajax_Check_Trungdon", 'id=' + $('#id').val(), function (data) {
                    $("#dontrung").html(data);
                });
            }
            function CheckForm() {
                if ($("#iLoaiDon").val() == 0) {
                    alert("Vui lòng chọn loại đơn");
                    $("#iLoaiDon").focus();
                    return false;
                }
                if ($("#iMoTa").val().length >= 250) {
                    alert("Vui lòng nhập không nhập quá 250 ký tự cho mô tả");
                    $("#iMoTa").focus();
                    return false;
                }

                if ($("#iLinhVuc").val() == -1) {
                    $("#iLinhVuc").focus()
                    alert("Vui lòng chọn lĩnh vực"); return false;
                }
                if ($("#iThuocThamQuyen").val() == -1) {
                    alert("Vui lòng chọn thẩm quyền xử lý"); return false;
                }

                ShowPageLoading();
                $.post("/Kntc/Phanloai", $("#form_").serialize(), function (ok) {

                    if (ok == 1) {
                        var iformparent = $("#formchuyentiep").val();
                        if ($("#iThuocThamQuyen").val() == 1) {
                            if ($("#iDuDieuKien").val() == 1) {
                                if ($("#iHinhThuc").val() == 1) {
                                    ShowPopUp('id=' + $("#id").val(), '/Kntc/Ajax_Chuyenxuly_noibo');
                                    return false;
                                }
                                else if ($("#iHinhThuc").val() == 2) {
                                    ShowPopUp('id=' + $("#id").val(), '/Kntc/Ajax_Huongdan_traloi');
                                    return false;
                                }
                                else
                                    if (iformparent == "1") {
                                        location.href = "/Kntc/Dudieukien/#success";
                                        return false;
                                    }
                                    else {
                                        if ($("#iHinhThuc").val() == 0) {
                                            ShowPopUp('id=' + $("#id").val(), '/Kntc/Ajax_Luutheodoi');
                                            return false;
                                        }
                                        else {

                                            location.href = "/Kntc/Dudieukien/#success";
                                            return false;
                                        }
                                    }
                                   
                            }
                            else if ($("#iDuDieuKien").val() == -1) {
                                location.href = "/Kntc/Thuocthamquyen/#success";
                                return false;
                            }
                            else {
                                if (iformparent == "1") {
                                    location.href = "/Kntc/Khongdudieukien/#success";
                                    return false;
                                }
                                else {
                                    if ($("#iHinhThuc").val() == 1) {
                                        ShowPopUp('id=' + $("#id").val(), '/Kntc/Ajax_Luutheodoi');
                                        return false;
                                    }
                                    else {
                                        location.href = "/Kntc/Khongdudieukien/#success";
                                        return false;
                                    }
                                }
                               

                            }
                        }
                        else {
                            if ($("#iHinhThuc").val() == 1) {
                                ShowPopUp('id=' + $("#id").val(), '/Kntc/Ajax_Luanchuyendonthu');
                                return false;
                            }
                            else {

                                location.href = "/Kntc/Khongthuocthamquyen/#success";
                                return false;
                            }
                        }
                    }
                });
                return false;

            }
            function ChangeThamQuyen(val) {
                if (val == 1) {
                    $("#thuocthamquyen").show();
                    $("#hinhthucxuly").hide();
                    $("#iDuDieuKien").chosen();
                }
                else if (val == 0) {
                    // $("#thuocthamquyen").hide();
                    $("#hinhthuc").html('<div class="span6" id="hinhthucxuly"><div class="control-group"><label for="textfield" class="control-label ">Hình thức xử lý <span class="f-red">*</span></label><div class="controls" id="ip_hinhthucxuly"></div></div></div>');
                    $("#iDuDieuKien").chosen();
                    $("#ip_hinhthucxuly").html(
                        "<select name=\"iHinhThuc\" id=\"iHinhThuc\" class=\"input-block-level\"><option value=\"0\">Đang nghiên cứu</option><option value=\"1\">Chuyển đơn thư</option></select>"
                    ); $("#iHinhThuc").chosen();
                }
                else {

                    // $("#thuocthamquyen").hide();
                    $("#hinhthucxuly").hide();
                }
            }
            function HinhThucXuLy(val, isEditing) {
                // $("#hinhthuc").html('<div class="span6" id="hinhthucxuly"><div class="control-group"><label for="textfield" class="control-label ">Hình thức xử lý <span class="f-red">*</span></label><div class="controls" id="ip_hinhthucxuly"></div></div></div>');
                $("#hinhthucxuly").show();
                if (val == 1) {
                    var html;
                    if (isEditing) {
                        var currVal = parseInt($('#iHinhThuc').val());
                        html = "<select name=\"iHinhThuc\" id=\"iHinhThuc\" class=\"input-block-level\">" +
                            `<option value=\"-1\" ${currVal == -1 ? 'selected': ''}>Đang nghiên cứu</option>` +
                            `<option value=\"1\" ${currVal == 1 ? 'selected' : ''}>Chuyển đơn</option>` +
                            `<option value=\"2\" ${currVal == 2 ? 'selected' : ''}>Hướng dẫn giải thích, trả lời</option>` +
                            `<option value=\"0\" ${currVal == 0 ? 'selected' : ''}>Không chuyển</option>` +
                            "</select>";
                    }
                    else html = "<select name=\"iHinhThuc\" id=\"iHinhThuc\" class=\"input-block-level\">" +
                        `<option value=\"-1\">Đang nghiên cứu</option>` +
                        `<option value=\"1\">Chuyển đơn</option>` +
                        `<option value=\"2\">Hướng dẫn giải thích, trả lời</option>` +
                        `<option value=\"0\">Không chuyển</option>` +
                        "</select>";
                    $("#ip_hinhthucxuly").html(html);
                    $("#iHinhThuc").chosen();
                } else if (val == 0) {                    
                    $("#ip_hinhthucxuly").html("<select name=\"iHinhThuc\" id=\"iHinhThuc\" class=\"input-block-level\"  onchange =\"ThemMoTa()\" disabled><option value=\"1\" selected>Lưu theo dõi</option><option value=\"-1\">Đang nghiên cứu</option></select>");
                    $("#iHinhThuc").chosen();
                    ThemMoTa();
                }
                else {
                    $("#hinhthucxuly").hide();
                }
            }
            function ThemMoTa() {
                if ($("#iHinhThuc").val() == 1) {
                    $("#mota").show();
                }         
            }
            function LoadLinhVucByLoaiDon(val) {
                $.post("<%=ResolveUrl("~")%>Kntc/Ajax_LoadLinhVucByLoaiDon", "iLoaiDon=" + val,
                    function (data) {
                        $("#ip_linhvuc").html(data);
                        $("#iLinhVuc").chosen();
                    });
            }
            function LoadLinhVuc() {

                if ($("#iLinhVuc").val() != 0) {

                    $.post("<%=ResolveUrl("~")%>Kntc/Ajax_LoadLinhVucNoiDung", "iLinhVuc=" + $("#iLinhVuc").val(), function (data) {
                        $("#LoadNoiDung").html(data);
                        $("#LoadTinhChat").html("<select style='width:100%' name='iTinhChat' id='iTinhChat' class='input-medium chosen-select'><option value='-1'> Chọn tính chất</option><option value='0'> Chưa xác định</option></select>");
                        $("#iTinhChat").chosen();
                        $("#iNoiDung").chosen();

                    });
                }
            }
            function LoadOpTinhChat() {
                if ($("#iNoiDung").val() != 0) {
                    $.post("<%=ResolveUrl("~")%>Kntc/Ajax_LoadTinhChatNoiDung",
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
