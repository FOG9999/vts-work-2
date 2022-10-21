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
								<i class="icon-reorder"></i> Hướng dẫn, giải thích trả lời đơn
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup_Re();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" onsubmit="return CheckForm()" action="/Kntc/Ajax_Huongdan_traloi_insert" id="_form" enctype="multipart/form-data" class="form-horizontal">
                                
                                 <div class="control-group">
							        <label for="textfield" class="control-label">Cơ quan ban hành<span class=" f-red">*</span></label>
							        <div class="controls chuyenxuly-donvi">
                                        <select name="iDonVi" id="iDonVi" class="input-block-level chosen-select">
                                            <option value="429">Văn phòng Đoàn đại biểu Quốc hội và HĐND Tỉnh</option>                                            
                                        </select>
							        </div>
						        </div>  
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Số công văn</label>
							        <div class="controls">
                                        <input type="text" class="input-medium"  name="cSoVanBan" id="cSoVanBan" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Ngày ban hành</label>
							        <div class="controls">
                                        <input type="text" class="input-medium datepick" id="dNgayBanHanh"  name="dNgayBanHanh" autocomplete ="off"/>
							        </div>
						        </div>
                                  <div class="control-group">
                                    <label for="textfield" class="control-label">Người ký/Chức vụ</label>
                                    <div class="controls">
                                        <input type="text" class="span6" name="cNguoiKy" />
                                         <input type="text" class="span6" name="cChucVu" />
                                    </div>
                                </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Ghi chú</label>
							        <div class="controls">
                                        <textarea class="input-block-level" rows="4" id="cGhiChu" name="cGhiChu"></textarea>       
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">File đính kèm</br>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</em></label>
							        <div class="controls">
                                        <% for (int i = 1; i < 4; i++)
                                            {
                                                string style_none = ""; if (i > 1) { style_none = "display:none; margin-top:5px;"; }
                                                string change = "";
                                                if (i < 3)
                                                {
                                                    int j = i + 1;
                                                    change = "$('.upload"+j+"').show()";
                                                }
                                                %>
                                            <div class="input-group file-group upload<%=i %>" style="<%=style_none%>">
                                                <span class="input-group-btn">
                                                    <span class="btn btn-success btn-file">
                                                        Duyệt file
                                                        <input onchange="CheckFileTypeUpload('file_upload<%=i %>','file_name<%=i %>');<%=change %>" 
                                                            name="file_upload<%=i %>" id="file_upload<%=i %>" type="file">
                                                    </span>
                                                </span>
                                                <input class="input-xlarge" disabled id="file_name<%=i %>" type="text">
                                                <span class="btn btn-danger" onclick="$('#file_upload<%=i %>,#file_name<%=i %>').val('');" title="Hủy"><i class="icon-trash"></i></span>
                                            </div>
                                        <% } %>                                       
							        </div>
						        </div> 
                                   <input type="hidden" name="id" value="<%=ViewData["id"] %>" />                                                                                                               
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                 
                                    <span onclick="HidePopup_Re();" class="btn btn-warning">Quay lại</span>
						        </div>                     
                             </form>
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>
<script>
    $("#iDonVi").chosen();
    function HidePopup_Re() {
        location.reload();
    }
    function  CheckForm()
    {
       // if ($("#iDonVi").val() == 0) { alert("Vui lòng chọn cơ quan nhận"); $("#iDonVi").focus(); return false; }
       
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
       
    }
</script>

