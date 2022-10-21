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
                                <i class="icon-reorder"></i> Thêm mới khiếu nại lĩnh vực
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
                                    <label for="textfield" class="control-label">Mã lĩnh vực</label>
                                    <div class="controls">
                                        <input type="text" value="" name="cCode" id="cCode" class="input-medium" onchange="kiemtrungma()" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label ">Tên lĩnh vực <i class="f-red">*</i></label>
                                    <div class="controls">
                                        <input type="text" value="" name="cTen" id="cTen" class="input-block-level" onchange="kiemtrungten()" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Thuộc lĩnh vực</label>
                                    <div class="controls">
                                        <select name="iParent" id="iParent" class="input-block-level">
                                            <option value='0'>- - - Gốc</option>
                                            <%=ViewData["opt-linhvuc"] %>
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Nhóm loại đơn</label>
                                    <div class="controls" id="loadloaidon">
                                        <select name="iLoaidon" id="iLoaidon" class="input-block-level">
                                            <option value='0'>- - - Chưa xác định</option>
                                            <%=ViewData["opt-loaidon"] %>
                                        
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group" style="display:none">
                                    <label for="textfield" class="control-label">Nhóm lĩnh vực</label>
                                    <div class="controls">
                                        <select name="iNhom" id="iNhom" class="input-block-level">
                                            <option value='1'>- - - Hành chính</option>
                                            <option value='2'>- - - Tư pháp</option>
                                              <option value='3'>- - - Khác</option>
                                           
                                        </select>
                                    </div>
                                </div>
                              <div class="control-group">
                                            <label for="textfield" class="control-label  ">Đơn vị <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iDonvi" id="iDonvi" class="input-block-level chosen-select" onchange="KiemTra(this.value)">
                                                        <option value="0">- - -  Chọn đơn vị ban hành</option>
                                                        <option value="-1" class="b"><span class="b">Nhiều đơn vị ban hành</span></option>
                                                        <%=ViewData["opt_donvi"] %>
                                                    </select>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group" id="hienthi" style="display:none;">
                                            <label for="textfield" class="control-label  "></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <div class="actions" id="action" style="height: 300px; overflow: auto">
                                                        <%=ViewData["list_group"] %>
                                                    </div>
                                                </div>
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
    $("#iLoaidon").chosen();
    $("#iParent").chosen();
    $("#iDonvi").chosen();
    function KiemTra(val) {
        if (val == -1) {
            document.getElementById("hienthi").style.display = "block";
        }
        else {
            document.getElementById("hienthi").style.display = "none";
        }
    }
    function CapNhat() {
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên lĩnh vực"); $("#cTen").focus(); return false;
        }
        if ($("#iDonvi").val() == 0) { alert("Vui lòng chọn đơn vị "); $("#iDonvi").focus(); return false; }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Linhvuc_insert", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Thêm mới thành công")
            } else if (ok == 2) {
                alert("Tên Lĩnh vực đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
                ;
            }
            else {
                alert("Mã lĩnh vực đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;

    }
    function Loadloaidon(val) {
    
            $.post("<%=ResolveUrl("~")%>Thietlap/Ajax_LoadNoiDungDon/"+val+"",
              
                function (data) {
                    $("#loadloaidon").html(data);
                    $("#iLoaidon").chosen();
                }
            );
        
    }
</script>
