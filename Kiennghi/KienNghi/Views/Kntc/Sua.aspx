<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Sửa đơn thư
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
                        <span>Chỉnh sửa đơn thư</span>
                    </li>
                </ul>
                <div class="close-bread">
                    <a href="#"><i class="icon-remove"></i></a>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <% KNTC_DON don = (KNTC_DON)ViewData["don"];%>
                    <% UserInfor user = (UserInfor)ViewData["user"];%>
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Chỉnh sửa đơn thư</h3>
                        </div>
                        <div class="box-content" style="text-align: left;">
                            <form class="form-horizontal form-column" id="form_" name="form_" onsubmit="return CheckForm()" enctype="multipart/form-data" method="post">
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Đối tượng gửi<i class="f-red">*</i></label>
                                            <div class="controls <%=user.tk_action.is_lanhdao && user.user_login.IUSER == don.IUSER ? "" : "disabled"%>">
                                                <span class="span4">
                                                    <label>
                                                        <input class="nomargin" type="radio" <%=user.tk_action.is_lanhdao && user.user_login.IUSER == don.IUSER ? "" : "disabled"%> name="iDoiTuongGui" onchange="ChangeKhoaTheoLoai(this.value)" value="0" <%= don.IDOITUONGGUI == 0 ? "checked" : ""%> />
                                                        Quốc hội</label>
                                                </span>
                                                <span class="span4">
                                                    <label>
                                                        <input class="nomargin" type="radio" <%=user.tk_action.is_lanhdao && user.user_login.IUSER == don.IUSER ? "" : "disabled"%> name="iDoiTuongGui" onchange="ChangeKhoaTheoLoai(this.value)" value="1" <%= don.IDOITUONGGUI == 1 ? "checked" : ""%> />
                                                        Hội Đồng Nhân Dân</label>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Khóa <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level" id="iKyHopTheoLoai">
                                                    <select class="input-block-level chosen-select" id="iKhoa" name="iKhoa">
                                                        <option value="0">Chọn khoá</option>
                                                        <%=ViewData["opt-khoa"] %>
                                                    </select>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Ngày nhận đơn <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <!-- Ngày nhận đơn-->
                                                <input type="text" value="<%=Convert.ToDateTime(don.DNGAYNHAN).ToString("dd/MM/yyyy")%>" name="dNgayNhan" autocomplete="off" id="dNgayNhan" class="input-medium datepick" />

                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Nguồn đơn <i class="f-red">*</i></label>
                                            <div class="controls" id="ip_nguondon">
                                                <!-- Nguồn đơn-->

                                                <select class="input-block-level chosen-select" name="iNguonDon">
                                                    <option value="0">- - - Chưa xác định</option>
                                                    <%=ViewData["opt-nguondon"] %>
                                                </select>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Người viết đơn <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <input type="text" name="cNguoiGui_Ten" id="cNguoiGui_Ten" value="<%=Server.HtmlEncode(don.CNGUOIGUI_TEN) %>" class="input-block-level" autofocus />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Số CMND  </label>
                                            <div class="controls">
                                                <!-- Nhập dữ liệu-->
                                                <input type="text" autocomplete="off" name="cNguoiGui_CMND" value="<%=Server.HtmlEncode(don.CNGUOIGUI_CMND) %>" class="input-block-level" />

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- end row-fluid-->
                                <div class="row-fluid">
                                    <div class="control-group">
                                        <div class="span12">
                                            <div class="controls">
                                                <label class="checkbox">
                                                    <input type="checkbox" <%if (don.IDOANDONGNGUOI == 1) { Response.Write("checked"); } %> name="iDoanDongNguoi" id="iDoanDongNguoi" onchange="$('#doan').toggle();" />
                                                    Đoàn đông người
                                                    <% string a = "display: none"; %>
                                                    <%if (don.IDOANDONGNGUOI == 1) { a = "display:  display:inline-block"; } %>
                                                    <span id="doan" style="margin-left: 15px; <%=a%>">
                                                        <input type="text" class="input-medium" name="iSoNguoi" id="iSoNguoi" value="<%=don.ISONGUOI %>" onchange="CheckNum('iSoNguoi')" value="0" placeholder="Số người" /></span>
                                                </label>

                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <!-- end row-fluid-->
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="control-group">
                                        <div class="span12">
                                            <label for="textfield" class="control-label ">Địa chỉ người viết đơn <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <p class="span4">
                                                    <select name="iDiaPhuong_0" onchange="ChangeTinhThanh('iDiaPhuong_1',this.value)" id="iDiaPhuong_0" class="chosen-select">
                                                        <option value="0">Chọn tỉnh thành</option>
                                                        <%=ViewData["opt-tinh"] %>
                                                        <option value="-1">Khác</option>
                                                    </select>
                                                </p>
                                                <p class="span4" id="iDiaPhuong_1">
                                                    <select <% if ((int)don.IDIAPHUONG_0 == -1) { Response.Write("disabled"); } %> name="iDiaPhuong_1" id="iDiaPhuong_01" class="chosen-select">
                                                        <option value="0">Chọn huyện/thành phố/thị xã</option>
                                                        <%=ViewData["opt-huyen"] %>
                                                    </select>
                                                </p>

                                                <p class="span4" id="iDiaPhuong_2">
                                                    <select name="iDiaPhuong_2" id="iDiaPhuong_02" <% if ((int)don.IDIAPHUONG_2 == -1) { Response.Write("disabled"); } %> class="chosen-select ">
                                                        <option value="0">Chọn xã/phường/thị trấn</option>
                                                        <%=ViewData["opt-xa"] %>
                                                    </select>
                                                </p>
                                                <p class="clear">
                                                    <input type="text" class="span6" id="cNguoiGui_DiaChi" name="cNguoiGui_DiaChi" value="<%=Server.HtmlEncode(don.CNGUOIGUI_DIACHI) %>" placeholder="Số nhà, đường..." />
                                                </p>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- end row-fluid-->
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Quốc tịch</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iNguoiGui_QuocTich" class="input-medium chosen-select"><%=ViewData["opt-quoctich"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Dân tộc</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iNguoiGui_DanToc" class="input-medium chosen-select">
                                                        <%=ViewData["opt-dantoc"] %>
                                                    </select>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- end row-->
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Tóm tắt nội dung đơn <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea class="input-block-level" name="cNoiDung" id="cNoiDung"> <%if (don.CNOIDUNG != null) { if (don.CNOIDUNG.Trim() != "") Response.Write(Server.HtmlEncode(don.CNOIDUNG.Trim())); }%> </textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Ghi chú </label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea class="input-block-level" name="cGhiChu" id="cGhiChu"><%=don.CGHICHU %></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- end row-->
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Độ mật </label>
                                            <div class="controls">
                                                <select name="iDoMat" id="iDoMat" class="input-medium chosen-select">
                                                    <%= ViewData["opt-domat"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Độ khẩn</label>
                                            <div class="controls">
                                                <select name="iDoKhan" id="iDoKhan" class="input-medium chosen-select">
                                                    <%= ViewData["opt-dokhan"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- end row-->
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Loại đơn</label>
                                            <div class="controls">
                                                <select name="iLoaiDon" id="iLoaiDon" class="input-medium chosen-select" onchange="LoadLinhVucByLoaiDon(this.value)">
                                                    <option value="0">- - - Chưa xác định</option>
                                                    <%=ViewData["opt-loaidon"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Lĩnh vực </label>
                                            <div class="controls" id="ip_linhvuc">
                                                <select name="iLinhVuc" id="iLinhVuc" class="chosen-select" onchange="LoadLinhVuc()" style="width: 100%">
                                                    <option value="0">- - - Chưa xác định</option>
                                                    <%=ViewData["opt-linhvuc"] %>
                                                </select>

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Nhóm nội dung </label>
                                            <div class="controls" id="LoadNoiDung">
                                                <select name="iNoiDung" id="iNoiDung" class="input-medium chosen-select">
                                                    <option value="0">- - - Chưa xác định</option>
                                                    <%=ViewData["opt-noidung"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Tính chất vụ việc</label>
                                            <div class="controls" id="LoadTinhChat">
                                                <select name="iTinhChat" class="input-medium chosen-select">
                                                    <option value="0">- - - Chưa xác định</option>
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
                                                <%--<select name="iThuocThamQuyen" id="iThuocThamQuyen" onchange="ChangeThamQuyen(this.value)" class="input-block-level chosen-select" disabled>--%>
                                                <%--<option value="-1">--- Vui lòng chọn</option>--%>
                                                <%--<option value="1">Đơn thuộc lĩnh vực phụ trách</option>--%>
                                                <%--<option value="0">Đơn không thuộc lĩnh vực phụ trách</option>--%>
                                                <%--</select>--%>
                                                <input type="hidden" name="iThuocThamQuyen" value="1" id="iThuocThamQuyen" class="input-block-level" />
                                                <input type="text" name="iThuocThamQuyen_text" value="Đơn thuộc lĩnh vực phụ trách" id="iThuocThamQuyen_text" disabled class="input-block-level" autofocus />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6" id="thuocthamquyen">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Điều kiện xử lý <span class="f-red">*</span></label>
                                            <div class="controls" id="ip_dieukien">
                                                <select name="iDuDieuKien" id="iDuDieuKien" class="input-block-level chosen-select" onchange="HinhThucXuLy(this.value)">
                                                    <%= ViewData["opt-dieukienxuly"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6" id="iSoluongnguoi">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Theo số lượng người <span class="f-red">*</span></label>
                                            <div class="controls">
                                                <select name="id_Soluongnguoi" id="id_Soluongnguoi" class="input-block-level chosen-select">    
                                                    <option value="0"  <%if (don.IPLSONGUOI == 0) { Response.Write("selected"); } %>>Đơn có họ tên chữ ký của một người</option>
                                                    <option value="1"  <%if (don.IPLSONGUOI == 1) { Response.Write("selected"); } %> >Đơn có họ tên chữ ký của 02 người trở lên</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6"  id="hinhthuc">
                                        <div  id="hinhthucxuly1">
                                            <div class="control-group">
                                                <label for="textfield" class="control-label ">Hình thức xử lý <span class="f-red">*</span></label>
                                                <div class="controls" >
                                                    <select name="iHinhThuc"  class="input-block-level chosen-select">
                                                        <%= ViewData["opt-hinhthucxuly"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div  id="hinhthucxuly2" style="margin-left: 0">
                                            <div class="control-group">
                                                <label for="textfield" class="control-label ">Hình thức xử lý <span class="f-red">*</span></label>
                                                <div class="controls" >
                                                    <select name="iHinhThuc"  class="input-block-level chosen-select" onchange="ThemMoTa()" disabled>
                                                        <option value="1" selected>Lưu theo dõi</option>
                                                        <option value="-1">Đang nghiên cứu</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="row-fluid" style="margin-top: 1%" id="mota">
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

                                <!-- end row-->
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">
                                            File đính kèm
                                       
                                            <small>Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</small></label>
                                        <div class="controls">
                                            <%=ViewData["file"] %>
                                            <% for (int i = 1; i < 4; i++)
                                                {
                                                    string style_none = ""; if (i > 1) { style_none = "display:none; margin-top:5px;"; }
                                                    string change = "";
                                                    if (i < 3)
                                                    {
                                                        int j = i + 1;
                                                        change = "$('.upload" + j + "').show()";
                                                    }
                                            %>
                                            <div class="input-group file-group upload<%=i %>" style="<%=style_none%>">
                                                <span class="input-group-btn">
                                                    <span class="btn btn-success btn-file">Duyệt file
                                                        <input onchange="CheckFileTypeUpload('file_upload<%=i %>','file_name<%=i %>');<%=change %>"
                                                            name="file_upload<%=i %>" id="file_upload<%=i %>" type="file">
                                                    </span>
                                                </span>
                                                <input class="input-xlarge" disabled id="file_name<%=i %>" type="text">
                                                <span class="btn btn-danger" onclick="$('#file_upload<%=i %>,#file_name<%=i %>').val('');" title="Hủy"><i class="icon-trash"></i></span>
                                            </div>
                                            <% } %>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" style="border-bottom: 1px solid #fff">
                                    <div class="form-actions">
                                        <input type="hidden" id="iDon" name="iDon" value="<%= ViewData["id_encrypt"] %>" />
                                        <input type="submit" value="Lưu đơn" class="btn btn-success" />
                                        <%--<a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>--%>
                                        <a href="#" onclick="window.history.back();" class="btn btn-warning">Quay lại</a>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $("#iDiaPhuong_1").change(function () {
            var id = $("#iDiaPhuong_01").val();
            $.post("/Home/Ajax_Change_Opt_huyenxa", 'id=' + id, function (data) {
                str = data;
                $("#iDiaPhuong_2").html(data);
                $("#iDiaPhuong_02").chosen();
            });
        })
        var dieuKienXuLy = $('#iDuDieuKien').val();
        HinhThucXuLy(parseInt(dieuKienXuLy));
        function ChangeThamQuyen(val) {
            if (val == 1) {
                $("#thuocthamquyen").show();
            } else {
                $("#thuocthamquyen").hide();
            }
        }
        //$("[name=__RequestVerificationToken]").val("i4sQEtJtGVTUV85XK4DP69lOUjckIK4mJhlEUv-9nRH8mjN0m9_AWeKx8_h7PGmF5UfDtRIFCD7aPUk21nuj2k8Or1xmp793oid0Oc-R4kuPvjGc6krs3JOX5To4q04kOBBeeKdbPaXVt_xzGSGA9q3LSC-dU7s4HBEsa5RkAaA1");
        function CheckForm() {

            if ($("#dNgayNhan").val() == "") {
                alert("Vui lòng chọn ngày nhận đơn"); $("#dNgayNhan").focus();
                return false;
            }
            if (!Validate_DateVN("dNgayNhan")) {
                return false;
            }
            if ($("#cNguoiGui_Ten").val() == "") {
                alert("Vui lòng nhập người nộp đơn"); $("#cNguoiGui_Ten").focus();
                return false;
            }
            //if (document.getElementById('iDoanDongNguoi').checked == true) {
            //    if ($("#iSoNguoi").val() < 2) {
            //        alert("Vui lòng nhập Đoàn đông người phải có số người từ 2 trở lên!");
            //        $("#iSoNguoi").focus();
            //        return false;
            //    }
            //}
            if ($("#iDiaPhuong_0").val() == 0) {
                alert("Vui lòng chọn địa chỉ tỉnh/thành người gửi đơn"); $("#iDiaPhuong_0").focus();
                return false;
            }
            if ($("#cNguoiGui_DiaChi").val() == "") {
                alert("Vui lòng nhập địa chỉ người nộp"); $("#cNguoiGui_DiaChi").focus();
                return false;
            }
            if ($("#iDiaPhuong_01").val() == 0) {
                alert("Vui lòng chọn huyện/thành phố/thị xã người gửi đơn"); $("#iDiaPhuong_01").focus();
                return false;
            }
            if ($("#cNoiDung").val() == "") {
                alert("Vui lòng nhập tóm tắt nội dung đơn"); $("#cNoiDung").focus();
                return false;
            }

        }

        function ChangeKhoaTheoLoai(val) {
            if (val != 2) {
                $.post("/Kntc/Ajax_Change_KyHopTheoLoai_option", 'id=' + val, function (data) {
                    console.log(data.strNguonDon);
                    $("#iKyHopTheoLoai").show().html(data.split(",")[1]);
                    $("#ip_nguondon").html(data.split(",")[0]);
                    $("#iKyHop").chosen();
                });
            } else {
                $("#iKyHopTheoLoai").html("");
            }
        }

        function LoadLinhVuc() {
            if ($("#iLinhVuc").val() != 0) {

                $.post("<%=ResolveUrl("~")%>Kntc/Ajax_LoadLinhVucNoiDung", "iLinhVuc=" + $("#iLinhVuc").val(),
                    function (data) {
                        $("#LoadNoiDung").html(data);

                        $("#LoadTinhChat").html("<select style='width:100%' name='iTinhChat' id='iTinhChat' class='input-medium chosen-select'><option value='0'> - - - Chưa xác định</option></select>");
                        $("#iNoiDung").chosen();
                        $("#iTinhChat").chosen();
                    }
                );
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
        function LoadLinhVucByLoaiDon(val) {
            $.post("<%=ResolveUrl("~")%>Kntc/Ajax_LoadLinhVucByLoaiDon", "iLoaiDon=" + val,
                function (data) {
                    $("#ip_linhvuc").html(data);
                    $("#iLinhVuc").chosen();
                });
        }

        function HinhThucXuLy(val) {
            var dieukienxuly = $('#iDuDieuKien').val();
            $('#hinhthuc').show();
            if (dieukienxuly == 1) {
                $('#hinhthucxuly1').show();
                $('#hinhthucxuly2').hide();
            } else if (dieukienxuly == 0) {
                $('#hinhthucxuly1').hide();
                $('#hinhthucxuly2').show();
            } else {
                $('#hinhthuc').hide();
            }
        }
    </script>
</asp:Content>
