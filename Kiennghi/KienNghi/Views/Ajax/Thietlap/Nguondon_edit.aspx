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
                                <i class="icon-reorder"></i>Cập nhật nguồn đơn
                            </h3>
                            <ul class="tabs">
                                <li class="active">
                                    <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
                                </li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <% KNTC_NGUONDON linhvuc = (KNTC_NGUONDON)ViewData["Nguondon"]; %>
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                <div class="control-group">
                                    <div style="float: left">
                                        <label for="textfield" class="control-label">Mã nguồn đơn</label>
                                        <div class="controls">
                                            <input type="text" name="cCode" id="cCode" class="input-medium" value="<%=Server.HtmlEncode(linhvuc.CCODE) %>" />
                                        </div>
                                    </div>
                                    <div style="float: left">
                                        <label for="textfield" class="control-label">Loại nguồn đơn<i class="f-red">*</i></label>
                                        <div class="controls">
                                            <div style ="margin-left: 20px; float:left">
                                                <span class="">
                                                    <label>
                                                        <input class="nomargin" onclick ="onChangeNguonDon(0)" type="radio" name="iLoai" value="0" <%= linhvuc.ILOAI == 0 ? "checked" : ""%> />
                                                        Quốc hội</label>
                                                </span>

                                            </div>
                                            <div style ="margin-left: 20px; float:left">
                                                <span class="" style="margin-left: 10px">
                                                    <label>
                                                        <input class="nomargin" type="radio" onclick ="onChangeNguonDon(1)" name="iLoai" value="1" <%= linhvuc.ILOAI == 1 ? "checked" : ""%> />
                                                        Hội Đồng Nhân Dân</label>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label ">Tên nguồn đơn <i class="f-red">*</i></label>
                                    <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(linhvuc.CTEN) %>" name="cTen" id="cTen" class="input-block-level" onchange="kiemtrungten()" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Thuộc nguồn đơn</label>
                                    <div class="controls" id ="iNguonDon">
                                        <select name="iParent" id="iParent" class="input-block-level">
                                            <option value='0'>- - - Gốc</option>
                                            <%=ViewData["opt_nguondon"] %>
                                        </select>
                                    </div>
                                </div>

                                <input type="hidden" name="id" value="<%=ViewData["id"] %>" id="id" />

                                <input type="hidden" name="idnguondon" value="<%=ViewData["idnguondon"] %>" id="idnguondon" />
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
    function onChangeNguonDon(idparent) {
        var id = $("#idnguondon").val();
        $.post("/Thietlap/Ajax_LoadOption_NguonDon", { idparent: idparent, id: id, type : 1 }, function (data) {
            $("#iNguonDon").html(data);

        });
    }

    function CapNhat() {
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập nguồn đơn"); $("#cTen").focus();
            return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Nguondon_update", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Cập nhật thành công");
            } else {
                alert("Mã nguồn đơn đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });

        return false;
    }

</script>
