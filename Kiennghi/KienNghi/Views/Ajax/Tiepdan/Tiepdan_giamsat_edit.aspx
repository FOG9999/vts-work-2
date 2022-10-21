<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Cập nhật giám sát vụ việc
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% TD_VUVIEC t = (TD_VUVIEC)ViewData["thongtin"]; %>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">  
                                <div class="control-group">
							        <label for="textfield" class="control-label">Hình thức giám sát <i class="f-red">*</i></label>
							        <div class="controls">
                                      <select name="iGiamsat" id="iGiamsat" class="chosen-select">
                                           <option value="0"> - - - Chọn mục giám sát</option>
                                            <option value="1" <% if (t.IGIAMSAT == 1) { Response.Write("selected"); } %>> - - - Chuyên đề</option>
                                              <option value="2" <% if (t.IGIAMSAT == 2) { Response.Write("selected"); } %>> - - - Lồng ghép</option>
                                              <option value="3" <% if (t.IGIAMSAT == 3) { Response.Write("selected"); } %>> - - - Chuyên đề</option>
                                           </select>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Nội dung giám sát <i class="f-red">*</i></label>
							        <div class="controls">
                                         <textarea class="input-block-level" name="cNoiDung" id="cNoiDung"><%=t.CYKIENGIAMSAT %></textarea>
							        </div>
						        </div>
                                   <input type="hidden"  id="id" name="id" value="<%= ViewData["id"] %>" />
                                <div class="form-actions nomagin">
                                       <input type="hidden" id="check_user" name="check_user" value="0" />
                                         
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
    $("#iGiamsat").chosen();
    function CapNhat() {
        if ($("#iGiamsat").val() == "") {
            alert("Vui lòng chuyên mục giám sát vụ việc"); $("#iGiamsat").focus(); return false;
        }
        if ($("#cNoiDung").val().trim().length < 1) { alert("Vui lòng nhập nội dung ý kiến"); $("#cNoiDung").focus(); return false; }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Tiepdan/Ajax_Giamsat_insert", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
            } else {
                
            }
        });
        return false;
    }

   
</script>
