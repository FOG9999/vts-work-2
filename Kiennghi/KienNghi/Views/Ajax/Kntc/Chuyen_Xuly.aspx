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
                                <i class="icon-reorder"></i>Chuyển xử lý đơn
                            </h3>
                            <ul class="tabs">
                                <li class="active">
                                    <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
                                </li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <%if (Convert.ToInt32(ViewData["type"]) == 0)
                              {%>
                            <div class="form-actions nomagin">
                                <div class="control-group">
                                    <div class="controls tcenter">
                                        <%=ViewData["caption"] %>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="controls tcenter">
                                        <a href="/Kntc/Sua/?id=<%=ViewData["id"] %>" class="btn btn-primary">Cập nhật</a>
                                        <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
                                    </div>
                                </div>

                            </div>
                            <% }
                              else
                              { %>
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">

                                <div class="control-group">
                                    <label for="textfield" class="control-label">Chọn chuyên viên xử lý <span class=" f-red">*</span></label>
                                    <div class="controls">
                                        <select name="iUser_GiaoXuLy" id="iUser_GiaoXuLy" class="input-block-level chosen-select">
                                            <option value="0">--- Chọn chuyên viên</option>
                                            <%=ViewData["opt-chuyenvien"] %>
                                        </select>
                                    </div>
                                </div>
                                <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                    <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
                                </div>
                            </form>
                            <%} %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $("#iUser_GiaoXuLy").chosen();
    function CapNhat() {
        if ($("#iUser_GiaoXuLy").val() == 0) {
            alert("Vui lòng nhập chọn chuyên viên giao xử lý!"); return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        var frm = $("#_form");
        var data = frm.serializeArray();

        $.ajax({
            type: "GET",
            headers: getHeaderToken(),
            contentType: "application/json; charset=utf-8",
            url: "<%=ResolveUrl("~")%>Kntc/Ajax_Chuyen_Xuly_insert",
            data: data,
            success: function (ok) {
                //$("#id").html(ok); HidePopup()
                if (ok == 1) {
                    location.reload();
                } else {
                    alert(ok);
                }
            }
        });
        return false;
    }
</script>
