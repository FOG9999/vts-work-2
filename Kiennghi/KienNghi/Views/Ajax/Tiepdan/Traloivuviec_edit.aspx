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
								<i class="icon-reorder"></i> Nội dung trả lời
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                           <% TD_VUVIEC t = (TD_VUVIEC)ViewData["thongtinvuviec"]; %>
                       
                        <div class="box-content popup_info">
                            <form method="post" name="_form" onsubmit="return CapNhat()" action="/Tiepdan/Ajax_Traloi_Update" id="_form" enctype="multipart/form-data" class="form-horizontal" >
                                
                                 <div class="control-group">
							        <label for="textfield" class="control-label "><%= ViewData["ten"] %></label>
							        <div class="controls">
                                        <select name="iDonVi" id="iDonVi" class="input-block-level">
                                            <option value="0">--- Chọn cơ quan</option>  
                                            <%=ViewData["opt-coquan"] %>                                          
                                        </select>
							        </div>
						        </div>  
                                <%= ViewData["KinhGui"] %>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Số công văn</label>
							        <div class="controls">
                                        <input type="text" class="input-medium"  name="cSoVanBan" id="cSoVanBan"  value="<%=ViewData["Socongvan"] %>"/>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Ngày ban hành </label>
							        <div class="controls">
                                        <input type="text" class="input-medium datepick"  name="dNgayBanHanh" id="dNgayBanHanh" value="<%=ViewData["NgayBanHanh"] %>" autocomplete ="off"  />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Vụ việc đôn đốc</label>
							        <div class="controls">
                                        <select name="iDonDoc" id="iDonDoc" class="input-medium">
                                            <option value="0"  >- - - Chưa xác định</option>           
                                            <option value="1" <%if (t.IDONDOC == 1) { Response.Write("selected"); } %>>- - - Vụ việc thường </option>        
                                            <option value="2"  <%if (t.IDONDOC == 2) { Response.Write("selected"); } %>> - - - Vụ việc đôn đốc cụ thể</option>                                            
                                        </select>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Người ký</label>
							        <div class="controls">
                                        <input type="text" class="input-medium" name="cNguoiKy" value="<%=ViewData["NguoiXyly"] %>" />
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
                                        <textarea class="input-block-level" name="cNoiDung"><%=ViewData["NoiDung"] %></textarea>
							        </div>
						        </div>
                                  <%= ViewData["NoiNhan"] %>
                                <div class="control-group">
							        <label for="textfield" class="control-label">File đính kèm</br>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</em></label>
							        <div class="controls">
                                        <%= ViewData["XoaFile"] %>
                                        <div class="input-group file-group" >
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
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                    <input type="hidden" name="id" id="id" value="<%=ViewData["id"] %>" />
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
        if ($("#iDonVi").val() == 0) { alert("Vui lòng chọn cơ quan nhận"); focus("#iDonVi"); return false; }
      
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