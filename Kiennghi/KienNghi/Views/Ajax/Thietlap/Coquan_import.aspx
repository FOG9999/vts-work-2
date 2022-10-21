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
								<i class="icon-reorder"></i> Import danh mục cơ quan
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat()" enctype="multipart/form-data" action="/Thietlap/Coquan_Import" >                                
                                <div class="control-group">
							        <label for="textfield" class="control-label ">File đính kèm <i class="f-red">*</i> </br>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng xls, xlsx</em></label>
							        <div class="controls">                                        
                                        <div class="input-group file-group upload">
                                            <span class="input-group-btn">
                                                <span class="btn btn-success btn-file">
                                                    Duyệt file
                                                    <input onchange="CheckFileValid();" 
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
    function CheckFileValid() {
        var filename = ($("#file_upload"))[0].files[0].name;
        var fileSize = ($("#file_upload"))[0].files[0].size;
        var size = Math.round((fileSize / 1024 / 1024));
        var extension = filename.substr(filename.lastIndexOf("."));
        var allowedExtensionsRegx = /(\.xls|\.xlsx)$/i;
        if (!allowedExtensionsRegx.test(extension)) {
            $("#file_upload").val("");
            alert("Định dạng file không hợp lệ");
            return false;
        } else if (size > 10) {
            $("#file_upload").val("");
            alert("Kích thước file quá lớn");
            return false;
        } else {
            $("#file_name").val($("#file_upload").val());
        }
    }
    function CapNhat() {
        if ($("#file_upload").val() == "") { alert("Vui lòng chọn file import!"); return false; }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        //return false;
        //if ($("#cNoiDung").val() == "") { alert("Vui lòng nhập nội dung giám sát!"); return false; }
        AlertAction("import dữ liệu thành công")

    }
</script>
