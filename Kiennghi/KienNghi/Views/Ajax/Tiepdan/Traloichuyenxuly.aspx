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
								<i class="icon-reorder"></i> Trả lời vụ việc chuyển xử lý
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" action="/Tiepdan/Ajax_traloichuyenxuly_insert" id="_form" enctype="multipart/form-data" class="form-horizontal" onsubmit="return CapNhat()">
                                
                                 <div class="control-group">
							        <label for="textfield" class="control-label f">Cơ quan nhận <i class="f-red">*</i></label>
							        <div class="controls">
                                        <%= ViewData["opt-coquan"] %>
							        </div>
						        </div>  
                                   <div class="control-group">
							        <label for="textfield" class="control-label ">Số công văn <i class="f-red">*</i> </label>
							        <div class="controls">
                                        <input type="text" class="input-medium"  name="cSoVanBan" id="cSoVanBan" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Ngày trả lời <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" class="input-medium datepick"  name="dNgayBanHanh" id="dNgayBanHanh" />
							        </div>
						        </div>
                                  <div class="control-group">
							        <label for="textfield" class="control-label">Người trả lời</label>
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
							        <label for="textfield" class="control-label ">Nội dung tóm tắt <i class="f-red">*</i></label>
							        <div class="controls">
                                        <textarea class="input-block-level" name="cNoiDung" id="cNoiDung"></textarea>
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
                                  <input type="hidden" name="iVuViec" value="<%=ViewData["iVuViec"] %>" />
                                      <input type="hidden" name="iDonVi" value="<%=ViewData["iDonVi"] %>" />
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
    $("#iChucVu").chosen();
    function CapNhat() {
       
        
        if ($("#dNgayBanHanh").val() == "") { alert("Vui lòng chọn ngày"); focus("#dNgayBanHanh"); return false; }
        if ($("#cSoVanBan").val() == "") { alert("Vui lòng nhập số công văn"); focus("#cSoVanBan"); return false; }
        if ($("#cNoiDung").val().trim().length < 1) { alert("Vui lòng nhập nội dung trả lời"); $("#cNoiDung").focus(); return false; }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
    }
</script>
<style>
  
    #iChucVu_chzn{
        width:150px !important;
    }
   
    

</style>


