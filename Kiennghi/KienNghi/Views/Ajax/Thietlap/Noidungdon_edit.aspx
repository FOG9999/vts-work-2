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
								<i class="icon-reorder"></i> Cập nhật nội dung đơn
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <% KNTC_NOIDUNGDON linhvuc = (KNTC_NOIDUNGDON)ViewData["Noidungdon"]; %>
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                              <div class="control-group">
							        <label for="textfield" class="control-label">Mã nội dung</label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(linhvuc.CCODE) %>" name="cCode" id="cCode" class="input-medium"  />
							        </div>
						        </div>
                                 <div class="control-group">
							        <label for="textfield" class="control-label ">Tên nội dung đơn <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(linhvuc.CTEN )%>"  name="cTen" id="cTen" class="input-block-level" onchange="kiemtrungten()" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Nhóm lĩnh vực <i class="f-red">*</i></label>
							        <div class="controls">
                                     
                                       
                                         <select name="iNhomLinhVuc" id="iNhomLinhVuc" class="input-block-level">
                                             <option value='0'>- - - Chưa xác định</option>
                                            <%=  ViewData["Option_linhvuc"]%>
                                        </select>
							      
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
    $("#iNhomLinhVuc").chosen();
    function CapNhat() {
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập nội dung đơn"); $("#cTen").focus();
            return false;
        }
        if ($("#iNhomLinhVuc").val() == 0) {
            alert("Vui lòng nhóm lĩnh vực"); $("#iNhomLinhVuc").focus();
            return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Noidungdon_update", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Cập nhật thành công");
            } else {
                alert("Mã Nội dung đơn đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
     
        return false;
    }
   
</script>
