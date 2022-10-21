<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color box-bordered">
			<div class="box-title">
				<h3 >
					<i class="icon-reorder"></i> Thực hiện import dữ liệu kiến nghị 
				</h3>
                <%-- <ul class="tabs">
					<li class="active">
						<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
					</li>
                </ul> --%>
            </div>
            <div class="box-content ">
                <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat()" enctype="multipart/form-data"  >                                
                    <%--<div class="control-group">
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
					</div>--%>
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
                                        <input onchange="onChangeFileImport('file_upload','file_name');"
                                            name="file_upload" id="file_upload" type="file" accept=".xlsx, .xls">
                                    </span>
                                </span>
                                <input class="input-xlarge" disabled id="file_name" type="text">
                                <span onclick="CheckFileImpport();" class="btn btn-warning">Kiểm tra</span>
                                <span class="btn btn-danger" onclick="Huy()" title="Hủy">Hủy</span>
                            </div>                                      
						</div>
					</div>   
                    <div id="div_dataUpload"></div>
                    <p id="load" style="display:none;">Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>
					<div class="form-actions tright">
                        
                        <p id="button_submit">
                            <button type="submit" class="btn btn-success" id="bt-submit-file" disabled>Lưu dữ liệu</button>                                    
                        <span onclick="HideTimKiem('import_place');" class="btn btn-warning">Đóng</span>
                        </p>
                                    
					</div>                     
                    </form>
            </div>                            
        </div>
    </div>
</div>
      
<script type="text/javascript">
    function Huy() {
        $('#file_upload,#file_name').val('');
        $("#CGHICHU").val('');
        $("#div_dataUpload").html("");
        $("#bt-submit-file").prop('disabled', true)
    }
    function CapNhat() {
        //if ($("#iKyHop").val() == 0) { alert("Vui lòng chọn kỳ họp!"); return false; }
        if ($("#CGHICHU").val() == "") { alert("Vui lòng nhập ghi chú!"); $("#CGHICHU").focus(); return false; }
        //if ($("#file_upload").val() == "") { alert("Vui lòng chọn file import!"); return false; }
        $("#load,#button_submit").toggle();
        
        $.post("/Kiennghi/InsertFile_KienNghi_Import", 'ghichu=' + $("#CGHICHU").val(), function (data) {
            if (data) alert("Lưu dữ liệu thành công!")
            else alert("Lưu dữ liệu thất bại!")
        })
        
    }

    function CheckFileImpport() {
        
        if ($("#file_upload").val() == "") { alert("Vui lòng chọn file import!"); return false; }
       
        $("#div_dataUpload").html("<p>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");

        var formdata = new FormData(); //FormData object
        var fileInput = document.getElementById('file_upload');
        for (i = 0; i < fileInput.files.length; i++) {
            formdata.append(fileInput.files[i].name, fileInput.files[i]);
        }
       
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Kiennghi/Check_import_insert');
        xhr.send(formdata);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                $("#div_dataUpload").html(xhr.responseText);
                $.post("/Kiennghi/Get_status_check_file_import", function (data) {
                    if (data == "true") $("#bt-submit-file").prop('disabled', false);
                    else $("#bt-submit-file").prop('disabled', true)
                })
            }
            else {
                $("#div_dataUpload").html("")
            }
        }
        return false;
    }

    function onChangeFileImport(file_upload, file_name) {
        CheckFileTypeUpload(file_upload, file_name);
        $("#bt-submit-file").prop('disabled', true);
    }
</script>
