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
								<i class="icon-reorder"></i> Hướng dẫn, giải thích trả lời vụ việc
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" action="/Tiepdan/Ajax_Huongdan_traloi_insert" id="_form" enctype="multipart/form-data" class="form-horizontal" onsubmit="return CapNhat()">
                                
                                 <div class="control-group">
							        <label for="textfield" class="control-label ">Cơ quan ban hành  <i class="f-red">*</i></label>
							        <div class="controls">
                                        <select name="iDonVi" id="iDonVi" class="input-block-level chosen-select">
                                             <option value="0">- - - Chọn cơ quan</option> 
                                               <%= ViewData["opt-coquan"] %>                                                    
                                        </select>
							        </div>
						        </div>  
                                
                                <div class="control-group">
							        <label for="textfield" class="control-label">Số công văn  </label>
							        <div class="controls">
                                        <input type="text" class="input-medium"  name="cSoVanBan" id="cSoVanBan" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Ngày ban hành </label>
							        <div class="controls">
                                        <input type="text" class="input-medium datepick"  name="dNgayBanHanh" id="dNgayBanHanh" autocomplete ="off"/>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Vụ việc đôn đốc </label>
							        <div class="controls">
                                        <select name="iDonDoc" id="iDonDoc" class="input-medium">
                                            <option value="0" >- - - Chưa xác định</option>           
                                            <option value="1">- - - Vụ việc thường </option>        
                                            <option value="2"> - - - Vụ việc đôn đốc cụ thể</option>                                            
                                        </select>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Người ký</label>
							        <div class="controls">
                                        <input type="text" class="input-medium" name="cNguoiKy" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Chức vụ </label>
							        <div class="controls">
                                        <select name="iChucVu" id="iChucVu" class="input-medium">
                                            <option value="0" >- - - Chưa xác định</option>           
                                           <%= ViewData["opt-chucvu"] %>                                                    
                                        </select>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Nội dung tóm tắt</label>
							        <div class="controls">
                                        <textarea class="input-block-level" name="cNoiDung"></textarea>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">File đính kèm</br>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</em></label>
							        <div class="controls">
                                        <div class="input-group file-group" style="">
                                            <span class="input-group-btn">
                                                <span class="btn btn-success btn-file">
                                                    Duyệt file
                                                    <input onchange="CheckFileTypeUpload('file_upload1','file_name1');" name="file_upload1" id="file_upload1" type="file">
                                                </span>
                                            </span>
                                            <input class="input-xlarge" disabled id="file_name1" type="text">
                                            <span class="btn btn-danger" onclick="$('#file_upload1,#file_name1').val('');" title="Hủy"><i class="icon-trash"></i></span>
                                        </div>
                                       
							        </div>
						        </div>      
                                 <input type="hidden" name="id" value="<%=ViewData["id"] %>" />                                                                                                          
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
    $("#iDonVi").chosen();
    $("#iChucVu").chosen();
    $("#iDonDoc").chosen();
    function CapNhat() {
        if ($("#iDonVi").val() == 0) { alert("Vui lòng chọn cơ quan nhận"); $("#iDonVi").focus(); return false; }

        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
    }
</script>
<style>
  
    #iChucVu_chzn{
        width:150px !important;
    }
     #iDonDoc_chzn{
        width:150px !important;
    }
    

</style>

