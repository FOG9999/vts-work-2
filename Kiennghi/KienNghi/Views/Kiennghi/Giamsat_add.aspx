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
								<i class="icon-reorder"></i> Cập nhật giám sát kết quả trả lời kiến nghị
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat()" enctype="multipart/form-data" action="/Kiennghi/Giamsat_insert" >                                
                                 <div class="control-group">
							        <label for="textfield" class="control-label">Ngày bắt đầu  <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" class="input-medium datepick" name="dNgayBatDau" id="dNgayBatDau" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Đơn vị giám sát <i class="f-red">*</i></label>
							        <div class="controls">
                                        <select name="iDonVi" class="input-block-level">
                                            <%=ViewData["opt-coquan"] %>
                                        </select>
							        </div>
						        </div>  
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Kế hoạch giám sát <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" class="input-block-level" name="cTen" id="cTen" />
							        </div>
						        </div>  
                                <div class="control-group">
							        <label for="textfield" class="control-label">Nội dung</label>
							        <div class="controls">
                                        <textarea class="input-block-level" name="cNoiDung"></textarea>
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
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-success">Lưu kế hoạch giám sát</button>
                                    
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
    $("#cTen").focus();
    function CapNhat() {
        if ($("#dNgayBatDau").val() == "") { alert("Vui lòng nhập ngày bắt đầu kế hoạch giám sát!"); $("#cTen").focus(); return false; }
        if (!Validate_DateVN("dNgayBatDau")) {
            return false;
        }
        if ($("#cTen").val() == "") { alert("Vui lòng nhập tên kế hoạch giám sát!"); $("#cTen").focus(); return false; }

        //if ($("#cNoiDung").val() == "") { alert("Vui lòng nhập nội dung giám sát!"); return false; }
        

    }
</script>
