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
								<i class="icon-reorder"></i> Cập nhật trả lời tổng hợp kiến nghị
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% KN_VANBAN v = (KN_VANBAN)ViewData["vanban"]; %>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" enctype="multipart/form-data" action="/Kiennghi/Tonghop_traloi_update" onsubmit="return CapNhat()">                                  
                                
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Số công văn <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(v.CSOVANBAN) %>" class="input-medium" id="cSoVanBan" name="cSoVanBan" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Ngày ban hành <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="<%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(v.DNGAYBANHANH)) %>" class="input-medium datepick" id="dNgayBanHanh" name="dNgayBanHanh" autocomplete ="off"/>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Người ký</label>
							        <div class="controls">
                                        <input type="text" class="input-medium" value="<%=Server.HtmlEncode(v.CNGUOIKY) %>" name="cNguoiKy" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Nội dung tóm tắt</label>
							        <div class="controls">
                                        <textarea class="input-block-level" name="cNoiDung"><%=Server.HtmlEncode(v.CNOIDUNG %></textarea>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">File đính kèm</br>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</em></label>
							            <div class="controls">
                                            <%=ViewData["file"] %>
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
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-success">Cập nhật</button>
                                    <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
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
        if ($("#cSoVanBan").val() == "") {
            alert("Vui lòng nhập số công văn"); $("#cSoVanBan").focus();
            return false;
        }
        if ($("#dNgayBanHanh").val() == "") {
            alert("Vui lòng chọn ngày ban hành"); $("#dNgayBanHanh").focus();
            return false;
        }
        if (!Validate_DateVN("dNgayBanHanh")) {
            return false;
        }
    }
</script>

