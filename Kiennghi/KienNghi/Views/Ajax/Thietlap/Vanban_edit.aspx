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
							<h3 >
								<i class="icon-reorder"></i> Cập nhật loại văn bản 
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <% VB_LOAI vbloai = (VB_LOAI)ViewData["vb_loai"]; %>
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Mã dân tộc</label>
                                    <div class="controls">
                                        <input type="text"  value="<%=Server.HtmlEncode(vbloai.CCODE) %>" name="cCode" id="cCode" class="input-medium"/>
                                    </div>
                                </div>
                                
                                 <div class="control-group">
							        <label for="textfield" class="control-label ">Tên loại văn bản <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(vbloai.CTEN) %>"  name="cTen" id="cTen" class="input-block-level"   onchange="kiemtrungten()" />
							        </div>
						        </div>
                                
                                 <input type="hidden" name="id" value="<%=ViewData["id"] %>" id="id" />                                                                                    
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
    function CapNhat() {
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên loại văn bản "); $("#cTen").focus();
            return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Vanban_update", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Cập nhật thành công");
            } else {
                alert("Mã Văn bản đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;
      
    }
   
</script>
