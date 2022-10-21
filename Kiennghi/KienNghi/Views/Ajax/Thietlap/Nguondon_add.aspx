<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color">
                        <div class="box-title">
                            <h3>
                                <i class="icon-reorder"></i>Thêm mới nguồn đơn
                            </h3>
                            <ul class="tabs">
                                <li class="active">
                                    <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
                                </li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                <div class="control-group">
                                    <div style="float: left">
                                        <label for="textfield" class="control-label">Mã nguồn đơn</label>
                                        <div class="controls">
                                            <input type="text" name="cCode" id="cCode" class="input-medium" value="" />
                                        </div>
                                    </div>

                                    <div style="float: left">
                                        <label for="textfield" class="control-label">Loại nguồn đơn<i class="f-red">*</i></label>
                                        <div class="controls">
                                            <div style="margin-left: 20px; float: left">
                                                <span class="">
                                                    <label>
                                                        <input class="nomargin" onclick="onChangeNguonDon(0)" type="radio" name="iLoai" value="0"  />
                                                        Quốc hội</label>
                                                </span>

                                            </div>
                                            <div style="margin-left: 20px; float: left">
                                                <span class="" style="margin-left: 10px">
                                                    <label>
                                                        <input class="nomargin" type="radio" onclick="onChangeNguonDon(1)" name="iLoai" value="1" />
                                                        Hội Đồng Nhân Dân</label>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label ">Tên nguồn đơn <i class="f-red">*</i></label>
                                    <div class="controls">
                                        <input type="text" value="" name="cTen" id="cTen" class="input-block-level" onchange="kiemtrungten()" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Thuộc nguồn đơn</label>
                                    <div class="controls" id ="iNguonDon">
                                        <select name="iParent" id="iParent" class="input-block-level">
                                            <option value='0'>- - - Gốc</option>
                                        </select>
                                    </div>
                                </div>
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
        $.post("/Thietlap/Ajax_LoadOption_NguonDon", { idparent: idparent, id: 0, type: 0 }, function (data) {
            $("#iNguonDon").html(data);

        });
    }
    function CapNhat() {
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập nguồn đơn"); $("#cTen").focus();
            return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Nguondon_insert", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
            } else {
                alert("Mã Nguồn đơn đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;
    }

</script>
