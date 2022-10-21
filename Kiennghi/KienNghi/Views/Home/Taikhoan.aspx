<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Quản lý thông tin tài khoản
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="main" class="nomargin">
        <div class="container-fluid">
            <div class="breadcrumbs">
                <ul>
                    <li>
                        <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i>Trang chủ</a>
                        <i class="icon-angle-right"></i>
                    </li>
                    <li>
                        <a href="#">Quản lý thông tin tài khoản</a>
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
                            <h3><i class="icon-user"></i>Quản lý thông tin tài khoản</h3>
                        </div>
                        <div class="box-content">
                            <% USERS tk = (USERS)ViewData["taikhoan"];
                               TaiKhoan detail = (TaiKhoan)ViewData["detail"];
                            %>
                            <div class="alert alert-danger">
                                <i class="icon-info-sign"></i>Vui lòng liên hệ với quản trị hệ thống nếu cần thay đổi thông tin về phòng ban, đơn vị công tác!
                            </div>
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">

                                <div class="control-group">
                                    <label for="textfield" class="control-label f-red">Tên đăng nhập</label>
                                    <div class="controls">
                                        <input type="text" value="<%=tk.CUSERNAME %>" onchange="CheckUsername();" disabled name="cUsername" id="cUsername" class="input-xlarge" />
                                        <span id="alert_user"></span>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label f-red">Mật khẩu mới</label>
                                    <div class="controls">
                                        <p>
                                            <input type="password" autocomplete="off" name="cPassword" id="cPassword" class="input-xlarge" />
                                            <span id="result"></span>
                                        </p>
                                        <em class="help-block f-red">Mật khẩu phải có 8 ký tự trở lên, có ký tự viết hoa, chữ số và ký tự đặc biệt!</em>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label f-red">Tên người dùng</label>
                                    <div class="controls">
                                        <input type="text" value="<%=tk.CTEN %>" name="cTen" id="cTen" class="input-xlarge" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label f-">Thuộc đơn vị</label>
                                    <div class="controls f-red b"><%=detail.donvi %></div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label f-">Thuộc phòng ban</label>
                                    <div class="controls">
                                        <select name="iPhongBan" id="iPhongBan" class="input-large">
                                            <%=ViewData["opt-phongban"] %>
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label f-red">Chức vụ</label>
                                    <div class="controls">
                                        <select name="iChucVu" id="iChucVu" class="input-large">
                                            <%=ViewData["opt-chucvu"] %>
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label f-">Email</label>
                                    <div class="controls">
                                        <input type="text" value="<%=tk.CEMAIL %>" name="cEmail" id="cEmail" class="input-xxlarge" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Số điện thoại</label>
                                    <div class="controls">
                                        <input type="text" value="<%=tk.CSDT %>" name="cSDT" id="cSDT" class="input-xxlarge" />
                                    </div>
                                </div>

                                <div class="form-actions nomagin">
                                    <div id="alert" class="alert alert-danger" style="display: none"></div>
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                    <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>
                                </div>

                            </form>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        <%--function CheckUsername() {
            if ($("#cUsername").val() != "") {
                $.post("<%=ResolveUrl("~")%>Cauhinh/Ajax_Taikhoan_CheckUsername",
                    "id_user=" + $("#iUser").val() + "&username=" + $("#cUsername").val(),
                    function (data) {
                        $("#check_user").val(data);
                        if (data == 1) {
                            $("#alert_user").addClass("btn btn-danger").html("<i class='icon-warning-sign'></i> Tài khoản này đã được đăng ký!");
                        } else {
                            $("#alert_user").addClass("btn btn-success").html("<i class='icon-ok'></i>")
                        }
                    }
                );
            }
        }--%>

        function ChangePass() {
            $('#result').html(checkStrength($('#cPassword').val()));
        }
        function checkStrength(password) {
            var strength = 0;
            if (password.length < 8) {
                $('#result').removeClass()
                $('#result').addClass('btn btn-danger')
                return '<i class="icon-warning-sign"></i> Mật khẩu quá ngắn';
            }
            if (password.length >= 8) strength += 1;
            if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1;
            if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1;
            if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1;
            $("#streng_pass").val(strength);
            if (strength < 2) {
                $('#result').removeClass()
                $('#result').addClass('btn btn-warning')
                return '<i class="icon-warning-sign"></i> Bảo mật yếu';
            } else if (strength == 2) {
                $('#result').removeClass()
                $('#result').addClass('btn btn-primary')
                return 'Bảo mật tốt';
            } else {
                $('#result').removeClass()
                $('#result').addClass('btn btn-success')
                return '<i class="icon-ok"></i> Bảo mật mạnh mẽ';
            }

        }
        function CapNhat() {

            if ($("#cTen").val() == "") {
                alert("Vui lòng nhập tên người dùng!"); $("#cTen").focus(); return false;
            }
            if ($("#cEmail").val() == "") {
                //alert("Vui lòng nhập email!"); $("#cEmail").focus(); return false;
            } else {
                if (!emailRegExp.test($("#cEmail").val())) {
                    alert("Email không hợp lệ!"); $("#cEmail").focus(); return false;
                }
            }
            if ($("#cPassword").val() != "") {
                if ($("#streng_pass").val() < 2) {
                    alert("Vui lòng chọn mật khẩu khó hơn để đảm bảo tính an toàn cho tài khoản của bạn!");
                    $("#cPassword").focus(); return false;
                }
            }
            //$("#q_data").html("<tr><td colspan=5 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            $.post("/Home/Ajax_Taikhoan_update", $("#_form").serialize(), function (ok) {
                if (ok == 1) {
                    AlertAction("Cập nhật thành công");
                } else {
                    //$("#alert").show().html(ok);
                    alert(ok);
                }
            });
            return false;
        }
    </script>
</asp:Content>
