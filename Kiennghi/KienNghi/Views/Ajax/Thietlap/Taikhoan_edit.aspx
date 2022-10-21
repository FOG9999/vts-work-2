<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<style>
    .input-xxlarge{
        width: 270px !important;
    }
</style>
<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Cập nhật tài khoản
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <% USERS tk=(USERS)ViewData["taikhoan"]; %>
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Tên đăng nhập <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(tk.CUSERNAME) %>" name="cUsername" id="cUsername" class="input-xlarge" />
                                        <span id="alert_user"></span>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Mật khẩu</label>
							        <div class="controls">
                                        <p> <input type="password" value="<%=ViewData["defaultDisplayPassword"]%>" autocomplete="off" onkeyup="ChangePass();" name="cPassword" id="cPassword" class="input-xlarge" />
                                            <span id="result"></span> 
                                        </p>
                                        <em class="help-block f-red" style="margin-top:1%">Mật khẩu phải có 6 ký tự trở lên, có ký tự viết hoa, chữ số và ký tự đặc biệt (!,%,&,@,#,$,^,*,?,_,~)!</em>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Tên người dùng <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(tk.CTEN) %>" name="cTen" id="cTen" class="input-xxlarge" />
							        </div>
						        </div>
                                
                                   <div class="control-group">
                                    <div class="span6" style="width:30% !important"> <label for="textfield" class="control-label"  >Thuộc đơn vị <i class="f-red">*</i></label>
							        <div class="controls">
                                        <select <%=ViewData["disable_donvi"] %> name="iDonVi" id="iDonVi" onchange="ChangeDonVi(this.value)" class="input-xxlarge">
                                            <option value='0'>- - - Chọn đơn vị</option>
                                            <%=ViewData["opt_donvi"] %>
                                        </select>
							        </div></div>
                                        <div class="span6" style="width:50% !important;margin-left:10% !important">
                            <label for="textfield" class="control-label "  >Thuộc phòng ban</label>
							        <div class="controls" id="loadphong">
                                        <select name="iPhongBan" id="iPhongBan" onchange="updateChucVu(this.value)" class="input-xxlarge">
                                            <option value='0'>- - - Chọn phòng ban</option>         
                                            <%=ViewData["opt_phongban"] %>                                     
                                        </select>
							        </div>
                                        </div>
							       
						        </div> 
                               <div class="control-group ">
                                     <div class="span6" style="width:30% !important">  <label for="textfield" class="control-label f-" >Nhóm quyền</label>
							        <div class="controls">
                                        <select name="iType" id="iType" class="input-xxlarge">
                                            <option value="0">- - - Chọn nhóm quyền</option>
                                           <%=  ViewData["opt-type"] %>                                     
                                        </select>
							        </div> </div>
							       <div class="span6" style="width:50% !important;margin-left:10% !important"><label for="textfield" class="control-label " >Chức vụ</label>
							        <div class="controls">
                                        <select name="iChucVu" id="iChucVu" class="input-xxlarge">
                                            <option value="0">Chọn chức vụ</option>
                                            <%=ViewData["chucvu"] %>                                          
                                        </select>
							        </div>  </div>
						        </div>
                                <div class="control-group">
                                      <div class="span6" style="width:30% !important"> <label for="textfield" class="control-label f">Email</label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(tk.CEMAIL) %>" name="cEmail" id="cEmail" class="input-xxlarge" />
							        </div></div>
                                      <div class="span6" style="width:50% !important;margin-left:10% !important">   <label for="textfield" class="control-label">Số điện thoại</label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(tk.CSDT) %>" name="cSDT" id="cSDT" class="input-xxlarge" />
							        </div> </div>
							        
						        </div>
                                    
                                 <div class="control-group">
							        <label for="textfield" class="control-label" >Kích hoạt</label>
							        <div class="controls">
                                        <select name="iStatus" id="iStatus" class="input-medium">
                                            <option value='1'>Kích hoạt</option>
                                            <option <% if (tk.ISTATUS == 0) { Response.Write("selected"); } %> value='0'>Tạm khóa</option>
                                        </select>
							        </div>
						        </div>    
                                   <input type="hidden" name="id" id="id" value="<%=ViewData["id"] %>" />                                                                         
						        <div class="form-actions nomagin">
                                 
                                    <input type="hidden" name="streng_pass" id="streng_pass" value="0" />
                                    <input type="hidden" name="check_user" id="check_user" value="0" />
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                    <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
						        </div>                     
                             </form>
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>
<script type="text/javascript">
    $("#iDonVi").chosen();
    $("#iPhongBan").chosen();
    $("#iType").chosen();
    $("#iStatus").chosen();
  
    $("#iChucVu").chosen();
    function CheckUsername() {
        if ($("#cUsername").val() != "") {
            $.post("<%=ResolveUrl("~")%>Cauhinh/Ajax_Taikhoan_CheckUsername",
                "id_user=" + $("#id").val() + "&username=" + $("#cUsername").val(),
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
    }
 
    function ChangeDonVi(val) {

        if (val == 0) {
            $("#loadphong").html("<select name='iPhongBan' id='iPhongBan' class='input-xxlarge'><option value='0'>- - - Chọn phòng ban</option></select>");
            $("#iPhongBan").chosen();
        } else {
            $.post("<%=ResolveUrl("~")%>Thietlap/Ajax_Change_phongban_donvi", "id=" + val, function (data) {
                $("#loadphong").html("<select name='iPhongBan' id='iPhongBan' onchange='updateChucVu(this.value)' class='input-xxlarge'><option value='0'>- - - Chọn phòng ban</option>" + data + "</select>");
                $("#iPhongBan").chosen();
            });
        }

    }
    function ChangePass() {
        
            $('#result').html(checkStrength($('#cPassword').val()));
       
        
    }

    function updateChucVu(val) {
        debugger;
        if (val == 0) {
            $("#loadChucVu").html("<select name='iChucVu' id='iChucVu' class='input-xxlarge'><option value='0'>- - - Chọn chức vụ</option></select>");
            $("#iChucVu").trigger("liszt:updated");
            $("#iChucVu").chosen();
        } else {
            $.post("<%=ResolveUrl("~")%>Thietlap/Ajax_Change_Chucvu", "id=" + val, function (data) {
                $("#loadChucVu").html("<select name='iChucVu' id='iChucVu' class='input-xxlarge'><option value='0'>- - - Chọn chức vụ</option>" + data + "</select>");
                $("#iChucVu").trigger("liszt:updated");
                $("#iChucVu").chosen();
            });
        }
    }

    function checkStrength(password) {
        var strength = 0;
        if (password.length < 6) {
            $('#result').removeClass()
            $('#result').addClass('btn btn-danger')
            return '<i class="icon-warning-sign"></i> Mật khẩu quá ngắn';
        }

        $("#streng_pass").val(strength);
        if (!(password.match(/([a-zA-Z])/) && password.match(/([0-9])/))) {
            $('#result').removeClass()
            $('#result').addClass('btn btn-primary')
            return 'Cần chứa cả chữ hoa chữ thường và số';
        }
        else if (!password.match(/.*[!,%,&,@,#,$,^,*,?,_,~]/)) {
            $('#result').removeClass()
            $('#result').addClass('btn btn-primary')
            return 'Cần một kí tự đặc biệt ';
        }
        else if (password.length < 8) {
            $('#result').removeClass()
            $('#result').addClass('btn btn-warning')
            return '<i class="icon-warning-sign"></i> Bảo mật yếu';
        } else {
            $('#result').removeClass()
            $('#result').addClass('btn btn-success')
            strength = 2;
            $("#streng_pass").val(strength);
            return '<i class="icon-ok"></i> Bảo mật mạnh mẽ';
        }

    }
    function CapNhat() {
        if ($("#cUsername").val() == "") {
            alert("Vui lòng chọn tên đăng nhập!"); $("#cUsername").focus(); return false;
        }
        
        if ($("#check_user").val() == 1) {
            alert("Tên đăng nhập này đã tồn tại!"); $("#check_user").focus(); return false;
        }
        
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên người dùng!"); $("#cTen").focus(); return false;
        }
        if ($("#iDonVi").val() == 0) {
            alert("Vui lòng chọn đơn vị!"); $("#iDonVi").focus(); return false;
        }
        if ($("#iPhongBan").val() == 0) {
            //alert("Vui lòng chọn phòng ban!"); $("#iPhongBan").focus(); return false;
        }

        if ($("#cEmail").val() == "") {
            //alert("Vui lòng nhập email!"); $("#cEmail").focus(); return false;
        } else {
            if (!emailRegExp.test($("#cEmail").val())) {
                alert("Email không hợp lệ!"); $("#cEmail").focus(); return false;
            }
        }
        if ($("#cPassword").val() != "" && $("#cPassword").val() != "<%=ViewData["defaultDisplayPassword"]%>") {
            if ($("#streng_pass").val() < 2) {
                alert("Vui lòng chọn mật khẩu khó hơn để đảm bảo tính an toàn cho tài khoản của bạn!");
                $("#cPassword").focus(); return false;
            }
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Taikhoan_update", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
            } else {
                alert(ok);
                $(".form-actions").html("<input type='hidden' name='id' id='id' value='0' /><input type='hidden' name='streng_pass' id='streng_pass' value='0' /><input type='hidden' name='check_user' id='check_user' value='0' /><button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button>&nbsp;<span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
       
        return false;
    }
</script>

