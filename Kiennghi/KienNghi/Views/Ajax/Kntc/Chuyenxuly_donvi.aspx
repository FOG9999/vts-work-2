<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Chọn tình trạng xử lý đơn
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup_Re();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" action="/Kntc/Ajax_Chuyenxuly_donvi_insert" id="_form" onsubmit="return CheckForm();" class="form-horizontal">
                                <div class="control-group">
							        <label for="textfield" class="control-label f-red">Hình thức xử lý*</label>
							        <div class="controls">
                                        <select name="iHinhThuc" id="iHinhThuc" class="chosen-select" onchange="ChangeHinhThuc(this.value)">
                                            <option value="0">Chọn hình thức</option>
                                            <option value="1">Đang xử lý, giải quyết</option>
                                            <option value="2">Không xử lý</option>
                                        </select>
							        </div>
						        </div>
                                <div class="control-group" id="lydo2" style="display:none;">
							        <label for="textfield" class="control-label">Lý do<span class=" f-red">*</span></label>
							        <div class="controls">
                                        <select name="iTheoDoi" id="iTheoDoi"><%=ViewData["opt-luutheodoi"] %></select>
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
<script type="text/javascript">
    $("#iHinhThuc").chosen();
    function HidePopup_Re() {
        location.reload();
    }
    function CheckForm() {
        if ($("#iHinhThuc").val() == 0) {
            alert("Vui lòng chọn hình thức xử lý!");
            return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
    }
   
    function ChangeHinhThuc(val) {
        if (val == 2) {
            $("#lydo2").show();
        } else {
            $("#lydo2").hide();
        }
    }
</script>
