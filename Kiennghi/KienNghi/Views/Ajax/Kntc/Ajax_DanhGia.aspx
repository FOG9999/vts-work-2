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
                                <i class="icon-reorder"></i>Kết quả đánh giá đơn đã xử lý
                            </h3>
                            <ul class="tabs">
                                <li class="active">
                                    <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
                                </li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="__form" action="/Kntc/Ajax_Danhgia_insert" id="_form" onsubmit="return CheckFormL();" class="form-horizontal">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Kết quả đánh giá<span class=" f-red">*</span></label>
                                    <div class="controls">
                                        <select id="idanhgia" name="idanhgia" class="input-block-level">
                                            <option value="0">Chọn kết quả đánh giá</option>
                                            <%=ViewData["danhgia"] %>
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Ghi chú</label>
                                    <div class="controls">
                                        <textarea class="input-block-level" id="ghichu" name="ghichu"></textarea>
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
    function CheckFormL() {
        if ($("#iSoLuongTrung").val() == "") {
            alert("Vui lòng nhập số lượng trùng!"); return false;
        }
        if ($("#cLuuTheoDoi_LyDo").val() == "") {
            alert("Vui lòng nhập lý do!"); return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
    }
</script>
