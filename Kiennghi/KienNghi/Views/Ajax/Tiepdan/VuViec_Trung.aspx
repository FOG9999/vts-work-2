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
								<i class="icon-reorder"></i> Nhập lý do trùng vụ việc, lưu theo dõi
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="__form" action="/Tiepdan/Ajax_Vuviectrung_insert" id="_form" onsubmit="return CheckFormL();" class="form-horizontal">
                                <div class="control-group">
							        <label for="textfield" class="control-label">Số lượng trùng<span class=" f-red">*</span></label>
							        <div class="controls">
                                        <input type="text"  id="iSoLuongTrung"  name="iSoLuongTrung" onchange="CheckNum('iSoLuongTrung')"/>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Lý do trùng<span class=" f-red">*</span></label>
							        <div class="controls">
                                        <textarea class="input-block-level" id="cLuuTheoDoi_LyDo" name="cLuuTheoDoi_LyDo"></textarea>
							        </div>
						        </div>
                                
                                <input type="hidden" name="id" value="<%=ViewData["id"] %>" />   
                                <input type="hidden" name="idtrung" value="<%=ViewData["idtrung"] %>" />                                                                                                   
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