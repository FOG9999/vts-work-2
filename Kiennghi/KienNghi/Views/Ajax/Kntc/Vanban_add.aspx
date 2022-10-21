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
                                <i class="icon-reorder"></i>Trả lời đơn chuyển
                            </h3>
                            <ul class="tabs">
                                <li class="active">
                                    <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
                                </li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" onsubmit="return CheckForm()" action="/Kntc/Ajax_Vanban_insert" id="_form" enctype="multipart/form-data" class="form-horizontal">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Tình trạng giải quyết<span class=" f-red">*</span></label>
                                    <div class="controls">
                                        <select name="loai" id="iloai" class="input-block-level chosen-select" onchange="DanhGia(this.value)">
                                            <%--<option value="loai" selected>Chọn loại văn bản</option>
                                            <option value="giaothuchien">Văn bản đôn đốc, giao thực hiện cho đơn vị xử lý</option>
                                            <option value="ketqua">Văn bản liên quan đến quá trình giải quyết</option>
                                            <option value="hoanthanh">Quyết định giải quyết (hoàn thành xử lý)</option>
                                            <option value="dinhchi">Văn bản đình chỉ xử lý (lưu đơn, theo dõi)</option>
                                            <option value="khongthuly">Văn bản không thụ lý đơn (lưu đơn, theo dõi)</option>--%>

                                            <option value="loai" selected>Chọn tình trạng giải quyết</option>
                                            <option value="chuagiaiquyet">Chưa giải quyết</option>
                                            <option value="ketqua">Đang giải quyết</option>
                                            <option value="hoanthanh">Đã giải quyết</option>
                                            <option value="dahuongdan">Đã hướng dẫn trả lời</option>
                                        </select>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label for="textfield" class="control-label">Cơ quan ban hành<span class=" f-red">*</span></label>
                                    <div class="controls">
                                        <%=ViewData["radio-thamquyen"] %>
                                        <select name="iDonVi" id="iDonVi" class="input-block-level">
                                            <option value="0">Chọn đơn vị</option>
                                            <%=ViewData["donvithuly"] %>
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Số công văn<span class=" f-red">*</span></label>
                                    <div class="controls">
                                        <input type="text" class="input-medium" name="cSoVanBan" id="cSoVanBan" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Ngày ban hành <span class=" f-red">*</span></label>
                                    <div class="controls">
                                        <input type="text" class="input-medium datepick" name="dNgayBanHanh" id="dNgayBanHanh" autocomplete ="off"/>
                                    </div>
                                </div>
                                <div class="control-group" id="danhgia" style="display: none">
                                    <label for="textfield" class="control-label">Kết quả đánh giá <span class=" f-red">*</span></label>
                                    <div class="controls">
                                        <select id="idanhgia" name="idanhgia" class="input-block-level">
                                            <option value="0">Chọn kết quả đánh giá</option>
                                            <%=ViewData["danhgia"] %>
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Người ký/Chức vụ</label>
                                    <div class="controls">
                                        <input type="text" class="span6" name="cNguoiKy" />
                                        <input type="text" class="span6" name="cChucVu" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Nội dung tóm tắt</label>
                                    <div class="controls">
                                        <textarea class="input-block-level" name="cNoiDung"></textarea>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Ghi chú</label>
                                    <div class="controls">
                                        <textarea class="input-block-level" name="cGhiChu"></textarea>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">
                                        File đính kèm<br/>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</em></label>
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

<script>
    $("#iloai").chosen();
    $("#iDonVi").chosen();
    function DanhGia(val) {
        if (val == "hoanthanh") {
            document.getElementById("danhgia").style.display = "block";
            $("#idanhgia").chosen();
        }
        else {
            document.getElementById("danhgia").style.display = "none";
        }
    }

    function DoiThamQuyenDonVi(val) {
        $.post("/Kiennghi/Ajax_Get_ThamQuyen_DonVi", '&val=' + val, function (data) {
            $("#iDonVi").html('<select name="iDonVi" id="iDonVi" class="chosen-select">' + data + '</select>');
            $("#iDonVi").trigger("liszt:updated");
            $("#iDonVi").chosen();
        });
    }

    function CheckForm() {

        if ($("#iloai").val() == "loai") {
            alert("Vui lòng chọn tình trạng giải quyết!"); $("#iloai").focus(); return false;
        }
        if ($("#iloai").val() == "bangiao") {
            if ($("#idonvixuly").val() == 0) {
                alert("Vui lòng chọn đơn vị xử lý đơn!"); $("#idonvixuly").focus(); return false;
            }
        }
        if ($("#iDonVi").val() == 0) {
            alert("Vui lòng chọn cơ quan ban hành!"); $("#iDonVi").focus(); return false;
        }
        if ($("#cSoVanBan").val() == "") {
            alert("Vui lòng nhập số văn bản!"); $("#cSoVanBan").focus(); return false;
        }
        if ($("#dNgayBanHanh").val() == "") {
            alert("Vui lòng nhập ngày ban hành!"); $("#dNgayBanHanh").focus(); return false;
        }
        if ($("#iloai").val() == "hoanthanh") {
            if ($("#idanhgia").val() == 0) {
                alert("Vui lòng chọn kết quả đánh giá!"); $("#idanhgia").focus(); return false;
            }
        }

        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
    }
</script>
