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
                            <h3>
                                <i class="icon-reorder"></i>Cập nhật đơn vị
                            </h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% QUOCHOI_COQUAN donvi = (QUOCHOI_COQUAN)ViewData["coquan"]; %>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Mã Đơn vị</label>
                                    <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(donvi.CCODE) %>" name="cCode" id="cCode" class="input-block-level" onchange="kiemtrungma()" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label ">Tên Đơn vị <i class="f-red">*</i> </label>
                                    <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(donvi.CTEN) %>" name="cTen" id="cTen" onchange="kiemtrungten()" class="input-block-level" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label ">Thuộc cấp cơ quan <i class="f-red">*</i></label>
                                    <div class="controls">
                                        <select name="iParent" id="iParent" class="input-block-level">
                                            <option value='0'>- - - Gốc</option>
                                            <%=ViewData["opt-donvi"] %>
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Địa phương</label>
                                    <div class="controls">
                                        <select class="input-block-level" name="iDiaPhuong" id="iDiaPhuong">
                                            <option value="0">--- Chọn địa phương tương ứng</option>
                                            <%=ViewData["opt-tinh"] %>
                                        </select>
                                    </div>
                                </div>
                                  <input type="hidden" name="id" id="id" value="<%=ViewData["id"] %>" />
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
    $("#iParent").chosen();
    $("#iDiaPhuong").chosen();
    function CapNhat() {
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên đơn vị"); $("#cTen").focus(); return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Coquan_update", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Cập nhật thành công")
            }
            else if (ok == 2) {
                alert("Mã cơ quan đã tồn tại, Vui lòng nhập mã khác!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
            else {
                alert("Tên cơ quan đã tồn tại,Vui lòng nhập tên khác!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;

    }
 
</script>
