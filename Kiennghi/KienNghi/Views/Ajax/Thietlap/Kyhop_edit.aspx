<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Cập nhật khóa
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% QUOCHOI_KYHOP k = (QUOCHOI_KYHOP)ViewData["kyhop"]; %>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Loại Khóa<i class="f-red">*</i></label>
                                    <div class="controls">
                                        <span class="span4">
                                            <label><input class="nomargin" type="radio"  name="iLoai" value="0" onclick="ChangeLoaiKhoaHop()" <%=(int)ViewData["iLoaiKhoa"] == 0 ? "checked" : ""%>/>
                                            Quốc hội</label>
                                        </span>
                                        <span class="span4">
                                            <label><input class="nomargin" type="radio"  name="iLoai" value="1" onclick="ChangeLoaiKhoaHop()" <%=(int)ViewData["iLoaiKhoa"] == 1 ? "checked" : ""%>/>
                                            Hội Đồng Nhân Dân</label>
                                        </span>
                                    </div>
                                </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Thuộc khóa <i class="f-red">*</i> </label>
							        <div class="controls">
                                        <select name="iKhoa" id="iKhoa_ThietLap"><%=ViewData["opt-khoa"] %></select>
							        </div>
						        </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Mã kỳ họp</label>
                                    <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(k.CCODE) %>" name="cCode" id="cCode" class="input-medium"/>
                                    </div>
                                </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Tên kỳ họp <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(k.CTEN) %>" name="cTen" id="cTen"  class="input-block-level" onchange="kiemtrungten()" />
							        </div>
						        </div>
                                
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Ngày bắt đầu</label>
							        <div class="controls">
                                        <input type="text" <%if (k.DBATDAU != null) {%>value="<%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(k.DBATDAU.ToString())) %>"<%} %>  name="dBatDau" id="dBatDau" class="input-medium datepick" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Ngày kết thúc</label>
							        <div class="controls">
                                        <input type="text" <%if (k.DKETTHUC != null) {%>value="<%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(k.DKETTHUC.ToString())) %>"<%} %>  name="dKetThuc" id="dKetThuc" class="input-medium datepick" />
							        </div>
						        </div>   
                                   <input type="hidden" name="id" value="<%=ViewData["id"] %>" />                                                                              
						        <div class="form-actions nomagin">
                                 
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
    debugger;
    var listAllKhoa = JSON.parse(`<%= ViewData["opt-khoa"]%>`);
    ChangeLoaiKhoaHop();

    function ChangeLoaiKhoaHop() {
        var listKhoa = listAllKhoa.filter(x => x.ILOAI == $("input[name='iLoai']:checked").val());
        if (listKhoa) {
            optKhoaHtml = `<option value="0">- - -  Chọn Khóa Họp</option>`;
            listKhoa.forEach(function (khoa) {
                if (khoa.IKHOA == <%=k.IKHOA%>) {
                    optKhoaHtml += ("<option " + " value='" + khoa.IKHOA + "' selected>" + khoa.CTEN + "</option>");
                }
                else {
                    optKhoaHtml += ("<option " + " value='" + khoa.IKHOA + "'>" + khoa.CTEN + "</option>");
                }
            });
            $("#iKhoa_ThietLap").html(optKhoaHtml);
            $("#iKhoa_ThietLap").chosen();
            $("#iKhoa_ThietLap").trigger("liszt:updated");
        }
    }

    function CapNhat() {
        if ($("#iKhoa_ThietLap").val() == "0") {
            alert("Vui lòng nhập chọn khoá họp"); $("#iKhoa").focus();
            return false;
        }
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên kì họp"); $("#cTen").focus();
            return false;
        }
        if ($("#cCode").val() == "") {
            alert("Vui lòng nhập mã kì họp"); $("#cCode").focus();
            return false;
        }
        if ($("#dBatDau").val() != "" && !Validate_DateVN("dBatDau")) {
            return false;
        }
        if ($("#dKetThuc").val() != "" && !Validate_DateVN("dKetThuc")) {
            return false;
        }
        if ($("#dBatDau").val() != "" && $("#dKetThuc").val() != "" && !CompareDate("dBatDau", "dKetThuc")) {
            return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Kyhop_update", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Cập nhật thành công")
            } if (ok == 2) {
                alert("Mã Kỳ họp đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            } if (ok == 3) {
                alert("Thời gian diễn ra kỳ họp phải nằm trong thời gian khóa");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;
    }
</script>
<style>
    #iKhoa_ThietLap_chzn{
       width: 150px !important;
    }
</style>