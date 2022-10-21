<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Thêm mới đơn thư
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
                        <span>Thêm mới đơn thư</span>
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
                            <h3><i class="icon-tags"></i>Thêm mới đơn</h3>
                        </div>
                        <div class="box-content" style="text-align: left;">
                            <form class="form-horizontal form-column" id="form_" name="form_" onsubmit="return CheckForm()" enctype="multipart/form-data" method="post">
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Đơn gửi đến<i class="f-red">*</i></label>
                                            <div class="controls">
                                                <%=ViewData["opt-don-guiden"] %>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                            <div class="control-group">
                                            <label for="textfield" class="control-label ">Khóa<i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level" id="iKyHopTheoLoai">
                                                    <select class="input-block-level chosen-select" id="iKyHop" name="iKyHop">
                                                        <%=ViewData["opt-kyhop"] %>
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
                                                <input type="text" value="<%=ViewData["dNgayNhan"]  %>" name="dNgayNhan" autocomplete="off" id="dNgayNhan" class="input-medium datepick" />

                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Nguồn đơn <i class="f-red">*</i></label>
                                            <div class="controls" id="ip_nguondon">
                                                <!-- Nguồn đơn-->
                                                <select class="input-block-level chosen-select" id="iNguonDon" name="iNguonDon">
                                                    <option value="0">--- Chưa xác định</option>
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
                                                <input type="text" name="cNguoiGui_Ten" value="<%=ViewData["cNguoiGui_Ten"]  %>" id="cNguoiGui_Ten" class="input-block-level" autofocus />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Số CMND  </label>
                                            <div class="controls">
                                                <!-- Nhập dữ liệu-->
                                                <input type="text" autocomplete="off" value="<%=ViewData["cNguoiGui_CMND"] %>" name="cNguoiGui_CMND" class="input-block-level" />

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- end row-fluid-->
                                <div class="row-fluid">
                                    <div class="control-group">
                                        <div class="span6">
                                            <div class="controls">
                                                <label class="checkbox">
                                                    <input type="checkbox" name="iDoanDongNguoi" id="iDoanDongNguoi" onchange="$('#doan').toggle();" />
                                                    Đoàn đông người
                                                     
                                                    <span id="doan" style="margin-left: 15px; display: none">
                                                        <input type="text" class="input-medium" name="iSoNguoi" id="iSoNguoi" onchange="CheckNum('iSoNguoi')" value="0" placeholder="Số người" /></span>
                                                </label>

                                            </div>

                                        </div>
                                        <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Số điện thoại  </label>
                                            <div class="controls">
                                                <!-- Nhập dữ liệu-->
                                                <input type="text" autocomplete="off" value="<%=ViewData["cNguoiGui_SDT"] %>" name="cNguoiGui_SDT" class="input-block-level" />

                                            </div>
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
                                                    <select name="iDiaPhuong_1" id="iDiaPhuong_01"class="chosen-select"  onchange="ChangeHuyenXa('iDiaPhuong_2',this.value)">
                                                        <option value="0">Chọn huyện/thành phố/thị xã</option>
                                                        <%=ViewData["opt-huyen"] %>
                                                    </select>
                                                </p>
                                                <p class="span4" id="iDiaPhuong_2">
                                                    <select name="iDiaPhuong_2" id="iDiaPhuong_02"  class="chosen-select ">
                                                        <option value="0">Chọn xã/phường/thị trấn</option>
                                                      <%-- <%=ViewData["opt-huyen"] %>--%>
                                                    </select>
                                                </p>
                                                <p class="clear">
                                                    <input autocomplete="off" value="<%=ViewData["cNguoiGui_DiaChi"] %>" type="text" class="span6" name="cNguoiGui_DiaChi" id="cNguoiGui_DiaChi" placeholder="Số nhà, đường..." />
                                                    <%--<input type="submit" value="Kiểm trùng" class="btn btn-success" />--%>
                                                    <a href="#" onclick="KiemTrungNhanh()" <%=ViewData["nutkiemtrung"] %> class="btn btn-success">Kiểm trùng</a>
                                                    <span class="f-red" id="noti"></span>
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
                                                        <%=ViewData["opt-dantoc"] %> <i class="f-red">*</i>
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
                                                    <textarea class="input-block-level" name="cNoiDung" id="cNoiDung"><%=ViewData["cNoiDung"] %></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%-- Không có luồng phần chuyên viên xử lý --%>
                                <%--<div class="row-fluid" <%=ViewData["chuyenvien"] %>>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Chuyển xử lý</label>
                                        <div class="controls">
                                            <select name="iGiaoXuLy" class="input-block-level chosen-select">
                                                <option value="0">- - - Chọn chuyên viên xử lý</option>
                                                <%=ViewData["opt-chuyenvien-xuly"] %>
                                            </select>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Ghi chú </label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea class="input-block-level" name="cGhiChu" id="cGhiChu"><%=ViewData["cGhiChu"] %></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- end row-->
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span10">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">
                                                File đính kèm
                                       
                                            <small>Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</small></label>
                                            <div class="controls">
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
                                    <div class="span2">
                                        <div class="control-group" style="float: right">
                                            <a <%=ViewData["dong"] %> id="open" onclick="Open()" href="javascript:void(0)">Mở rộng <i id="icon_morong" class="icon-arrow-down"></i></a>
                                            <a <%=ViewData["mo"] %> id="close" onclick="Close()" href="javascript:void(0)">Mở rộng <i id="icon_thuhep" class=" icon-arrow-up"></i></a>
                                        </div>
                                    </div>
                                    <script>
                                        function Open() {
                                            document.getElementById('phanloai').style = "display: block";
                                            document.getElementById('close').style = "display: block";
                                            document.getElementById('open').style = "display: none";
                                        }
                                        function Close() {
                                            document.getElementById('phanloai').style = "display: none";
                                            document.getElementById('close').style = "display: none";
                                            document.getElementById('open').style = "display: block";
                                        }
                                    </script>

                                </div>
                                <div <%=ViewData["morong"] %> id="phanloai">
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
                                                <label for="textfield" class="control-label">Độ khẩn </label>
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
                                                <label for="textfield" class="control-label ">Loại đơn </label>
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
                                                    <select name="iNoiDung" onchange="LoadOpTinhChat()" id="iNoiDung" class="input-medium chosen-select">
                                                        <option value="0">- - - Chưa xác định</option>
                                                        <%=ViewData["opt-noidung"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label for="textfield" class="control-label">Tính chất vụ việc </label>
                                                <div class="controls" id="LoadTinhChat">
                                                    <select name="iTinhChat" class="input-medium chosen-select">
                                                        <option value="0">- - - Chưa xác định</option>
                                                        <%= ViewData["opt-tinhchat"] %>
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
                                                   <%-- <select name="iThuocThamQuyen" id="iThuocThamQuyen" onchange="ChangeThamQuyen(this.value)" class="input-block-level chosen-select">
                                                        <option value="-1">--- Vui lòng chọn</option>
                                                        <option value="1">Đơn thuộc lĩnh vực phụ trách</option>
                                                        <option value="0">Đơn không thuộc lĩnh vực phụ trách</option>
                                                    </select>--%>
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
                                    <div class="row-fluid" >
                                        <div class="span6" id="iSoluongnguoi">
                                            <div class="control-group">
                                                <label for="textfield" class="control-label ">Theo số lượng người</label>
                                                <div class="controls">
                                                    <select name="id_Soluongnguoi" id="id_Soluongnguoi" class="input-block-level chosen-select">
                                                       <option value="0" selected>Đơn có họ tên chữ ký của một người</option>
                                                       <option value="1">Đơn có họ tên chữ ký của 02 người trở lên</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6" id="hinhthuc">
                                            <div id="hinhthucxuly1" >
                                                <div class="control-group">
                                                    <label for="textfield" class="control-label ">Hình thức xử lý <span class="f-red">*</span></label>
                                                    <div class="controls">
                                                        <select name="iHinhThuc" class="input-block-level chosen-select">
                                                            <%= ViewData["opt-hinhthucxuly"] %>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="hinhthucxuly2" >
                                                <div class="control-group">
                                                    <label for="textfield" class="control-label ">Hình thức xử lý <span class="f-red">*</span></label>
                                                    <div class="controls">
                                                        <select name="iHinhThuc" class="input-block-level chosen-select" onchange="ThemMoTa()" disabled>
                                                            <option value="1" selected>Lưu theo dõi</option>
                                                            <option value="-1">Đang nghiên cứu</option>
                                                        </select>
                                                    </div>
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
                                                    <textarea class="input-block-level" name="iMoTa" id="iMoTa"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                </div>
                                <div class="row-fluid" style="border-bottom: 1px solid #fff">
                                    <div class="form-actions">
                                        <input type="submit" value="Lưu đơn & kiểm trùng" class="btn btn-success" />
                                        <input type="hidden" value="<%=ViewData["id"] %>" id="id" name="id" />
                                        <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>
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
        $(window).on("pageshow", function (event) {
            HidePageLoading();
        });
        $("#iDiaPhuong_1").change(function () {
            var id = $("#iDiaPhuong_01").val();
            $.post("/Home/Ajax_Change_Opt_huyenxa", 'id=' + id, function (data) {
                str = data;
                $("#iDiaPhuong_2").html(data);
                $("#iDiaPhuong_02").chosen();
            });
        })
        $('#hinhthuc').hide();
        function KiemTrungNhanh() {
            if ($("#cNguoiGui_Ten").val() == "") {
                alert("Vui lòng người nộp đơn");
                $("#cNguoiGui_Ten").focus();
                return false;
            }
            if ($("#iDiaPhuong_0").val() == 0) {
                alert("Vui lòng chọn tỉnh thành");
                $("#iDiaPhuong_0").focus();
                return false;
            }
            $.post("<%=ResolveUrl("~")%>Kntc/KiemTrungNhanh", $("#form_").serialize(),
                function (data) {
                    if (data == 0) {
                        $("#noti").html("Không có đơn trùng");
                    }
                    else {
                        location.href = "/Kntc/Kiemtrung";
                    }

                }
            );
        }

        function ChangeThamQuyen(val) {
            if (val == 1) {
                $("#thuocthamquyen").show();
            } else {
                $("#thuocthamquyen").hide();
            }
        }
        function CheckForm() {
            if ($("#iNguonDon").val() == 0) {
                alert("Vui lòng chọn nguồn đơn");
                $("#iNguonDon").focus();
                return false;
            }

            if (parseInt($("#iSoNguoi").val().trim()) > 1000000) {
                alert("Vui lòng nhập không quá 1.000.000 người cho đoàn đông người");
                $("#iSoNguoi").focus();
                return false;
            }

            var ngayNhan = $("#dNgayNhan").val();
            if (ngayNhan == "") {
                alert("Vui lòng chọn ngày nhận đơn");
                $("#dNgayNhan").focus();
                return false;
            } else {

                if (! /^\d\d\/\d\d\/\d\d\d\d$/.test(ngayNhan)) {
                    alert("Vui lòng nhập ngày nhận đơn hợp lệ");
                    $("#dNgayNhan").focus();
                    return false;
                }
                const parts = ngayNhan.split('/').map((p) => parseInt(p, 10));
                
                const d = new Date(parts[2], parts[1], parts[0]);
                console.log(parts);
                console.log(d.getMonth());
                console.log(d.getDate());
                console.log(d.getFullYear());
                if (d.getMonth() === parts[0] && d.getDate() === parts[1] && d.getFullYear() === parts[2]) {
                    alert("Vui lòng nhập ngày nhận đơn hợp lệ");
                    $("#dNgayNhan").focus();
                    return false;
                }

            }

            if ($("#cNguoiGui_Ten").val().trim() == "") {
                $("#cNguoiGui_Ten input[type=text]").focus();
                alert("Vui lòng nhập người nộp đơn");
                return false;
            }
            //if (document.getElementById('iDoanDongNguoi').checked == true) {
            //    if ($("#iSoNguoi").val() == "") {
            //        alert("Vui lòng nhập số người!");
            //        $("#iSoNguoi").focus();
            //        return false;
            //    }
            //}
            if ($("#iDiaPhuong_0").val() == 0) {
                alert("Vui lòng chọn địa chỉ tỉnh/thành người gửi đơn");
                return false;
            }
            if ($("#iDoanDongNguoi").val() == undefined) {
                if ($("#iSoNguoi").val() == 0) {
                    alert("Vui lòng nhập số người");
                    $("#iSoNguoi").focus();
                    return false;
                }
            }
            if ($("#iDiaPhuong_01").val() == 0) {
                alert("Vui lòng chọn huyện/thành phố/thị xã người gửi đơn"); $("#iDiaPhuong_01").focus();
                return false;
            }
            if ($("#cNoiDung").val().trim() == "") {
                alert("Vui lòng nhập nội dung đơn"); $("#cNoiDung").focus();
                return false;
            }
            if ($("#cNguoiGui_DiaChi").val().trim() == "") {
                alert("Vui lòng nhập địa chỉ người nộp"); $("#cNguoiGui_DiaChi").focus();
                return false;
            }

            ShowPageLoading();
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
        function ShowTrung() {
            if ($("#cNguoiGui_Ten").val() == "") {

                $("#cNguoiGui_Ten input[type=text]").focus();
                alert("Vui lòng nhập người nộp đơn");


                return false;
            }
            if ($("#iDiaPhuong_0").val() == 0) {
                alert("Vui lòng chọn địa chỉ tỉnh/thành người gửi đơn");
                return false;
            }
            HidePopup();
            $("body").prepend('<div id="screen"><div id="loader"><div id="load1" class="spin"></div><div id="load2" class="spin"></div>' +
                '<div id="load3" class="spin"></div><div id="load4" class="spin"></div><div id="load5" class="spin"></div></div></div>');
            $.post("/Kntc/Ajax_HienThiDonTrung", $("#form_").serialize(), function (ok) {
                HidePopup();
                $("body").prepend(ok);
                $('.datepick').datepicker();
            });
        }
        function ChangeKhoaTheoLoai(val) {
            if (val != 2) {
                $.post("/Kntc/Ajax_Change_KyHopTheoLoai_option", 'id=' + val, function (data) {
                    console.log(data.strNguonDon);
                    $("#iKyHopTheoLoai").show().html(data.split(",")[1]);
                    $("#ip_nguondon").html(data.split(",")[0]);
                    $("#iKyHop").chosen();
                    $("#iNguonDon").chosen();
                });
            } else {
                $("#iKyHopTheoLoai").html("");
            }
        }

        function HinhThucXuLy(val) {
            var dieukienxuly = $('#iDuDieuKien').val();
            $('#hinhthuc').show();
            if (dieukienxuly == 1) {
                $('#hinhthucxuly1').hide();
                $('#hinhthucxuly2').hide();
            } else if (dieukienxuly == 0) {
                $('#hinhthucxuly1').hide();
                $('#hinhthucxuly2').hide();
            } else {
                $('#hinhthuc').hide();
            }
            
        }
    </script>
</asp:Content>
