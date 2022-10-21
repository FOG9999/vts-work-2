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
								<i class="icon-reorder"></i> Cập nhật lại mẫu kiến nghị import
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% KN_IMPORT im = (KN_IMPORT)ViewData["import"]; %>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat()" enctype="multipart/form-data" action="/Kiennghi/Import_update" >                                
                                 <div class="control-group">
							        <label for="textfield" class="control-label ">Kỳ họp <i class="f-red">*</i></label>
							        <div class="controls">
                                        <select name="iKyHop" id="iKyHop" class="input-block-level">
                                            <%=ViewData["opt-kyhop"] %>
                                        </select>
							        </div>
						        </div>  
                                 <div class="control-group">
							        <label for="textfield" class="control-label"></label>
							        <div class="controls">
                                         <%=ViewData["check-hinhthuc"] %>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Ghi chú <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" class="input-block-level" value="<%=im.CGHICHU %>" name="CGHICHU" id="CGHICHU" />
							        </div>
						        </div>  
                               
                                <div class="control-group">
							        <label for="textfield" class="control-label ">File đính kèm <i class="f-red">*</i></br>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng xls, xlsx</em></label>
							        <div class="controls">                                        
                                        <div class="input-group file-group upload">
                                            <span class="input-group-btn">
                                                <span class="btn btn-success btn-file">
                                                    Duyệt file
                                                    <input onchange="CheckFileTypeUpload('file_upload','file_name');" 
                                                        name="file_upload" id="file_upload" type="file">
                                                </span>
                                            </span>
                                            <input class="input-xlarge" disabled id="file_name" type="text">
                                            <span class="btn btn-danger" onclick="$('#file_upload,#file_name').val('');" title="Hủy"><i class="icon-trash"></i></span>
                                        </div>                                      
							        </div>
						        </div>                                                                                                                    
						        <div class="form-actions nomagin">
                                    <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                    <p id="load" style="display:none;">Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>
                                    <p id="button_submit">
                                        <button type="submit" class="btn btn-success">Cập nhật</button>                                    
                                         <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
                                    </p>
                                    
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
        if ($("#iKyHop").val() == 0) { alert("Vui lòng chọn kỳ họp!"); return false; }
        if ($("#CGHICHU").val() == "") { alert("Vui lòng nhập ghi chú!"); $("#CGHICHU").focus(); return false; }
        if ($("#file_upload").val() == "") { alert("Vui lòng chọn file import!"); return false; }
        $("#load,#button_submit").toggle();
        //return false;
        //$(".form-actions").html("");
        //return false;
        //if ($("#cNoiDung").val() == "") { alert("Vui lòng nhập nội dung giám sát!"); return false; }
        

    }
</script>
