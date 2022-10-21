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
                                <i class="icon-reorder"></i>Lưu đơn, theo dõi
                            </h3>
                                <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup_Re();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" onsubmit="return CheckForm()" action="/Kntc/Ajax_Luutheodoi_insert" id="_form" enctype="multipart/form-data" class="form-horizontal">

                                <div class="control-group">
                                    <label for="textfield" class="control-label">Chọn lý do<span class=" f-red">*</span></label>
                                    <div class="controls">
                                        <select class="chosen-select" style="width:400px" name="iTheoDoi" id="iTheoDoi"><%=ViewData["opt-luutheodoi"] %></select>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Mô tả chi tiết lý do</label>
                                    <div class="controls">
                                        <textarea maxlength="500" style="width:400px; height:200px" name ="cLuuTheoDoi_LyDo" id="cLuuTheoDoi_LyDo"></textarea>
                                    </div>
                                </div>
                                <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>

                                    <span onclick="HidePopup_Re();" class="btn btn-warning">Quay lại</span>
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
    function HidePopup_Re() {
        location.reload();
    }
    function CheckForm() {
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
    }

</script>
