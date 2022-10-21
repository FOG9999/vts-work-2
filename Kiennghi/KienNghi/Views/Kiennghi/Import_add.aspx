<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>



<div class="row-fluid">
    <div class="span12">
        <div class="box box-color">
			<div class="box-title">
				<h3 >
					<i class="icon-reorder"></i> Cập nhật thông tin mẫu kiến nghị import
				</h3>
                <%-- <ul class="tabs">
					<li class="active">
						<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
					</li>
                </ul>--%>
            </div>
            <div class="box-content popup_info">
                <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat()" enctype="multipart/form-data" action="/Kiennghi/Import_insert" >                                
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
                                <span class="span6"><input type="radio" name="iTruocKyHop" checked id="iTruocKyHop" value="1" /> Trước kì họp</span>
                                <span class="span6"><input type="radio" name="iTruocKyHop" id="iTruocKyHop" value="0" /> Sau kì họp</span>
						</div>
					</div>
                    <div class="control-group">
						<label for="textfield" class="control-label ">Ghi chú <i class="f-red">*</i></label>
						<div class="controls">
                            <input type="text" class="input-block-level" name="CGHICHU" id="CGHICHU" />
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
                        <button type="submit" class="btn btn-success">Cập nhật</button>
                                    
                        <span onclick="HideTimKiem('search_place');" class="btn btn-warning">Quay lại</span>
					</div>                     
                    </form>
            </div>                            
        </div>
    </div>
</div>
       
<script type="text/javascript">
    function CapNhat() {
        if ($("#iKyHop").val() == 0) { alert("Vui lòng chọn kỳ họp!"); return false; }
        if ($("#CGHICHU").val() == "") { alert("Vui lòng nhập ghi chú!"); $("#CGHICHU").focus(); return false; }
        if ($("#file_upload").val() == "") { alert("Vui lòng chọn file import!"); return false; }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        //return false;
        //if ($("#cNoiDung").val() == "") { alert("Vui lòng nhập nội dung giám sát!"); return false; }
        

    }
</script>
