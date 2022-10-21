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
								<i class="icon-reorder"> </i> Cập nhật văn bản
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <% KN_VANBAN vanban = (KN_VANBAN)ViewData["vanban"]; %>
                            <form method="post" id="form_" action="/Kiennghi/Duthao_update/" enctype="multipart/form-data" class="form-horizontal" onsubmit="return CheckForm()"> 
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Số văn bản <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="<%=vanban.CSOVANBAN %>" class="input-medium" name="cSoVanBan" id="cSoVanBan" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Ngày ban hành <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" class="input-medium datepick" value="<%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(vanban.DNGAYBANHANH)) %>" name="dNgayBanHanh" id="dNgayBanHanh" autocomplete ="off"/>
							        </div>
						        </div>  
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Người ký</label>
							        <div class="controls">
                                        <input type="text" class="input-medium" value="<%=vanban.CNGUOIKY%>" name="cNguoiKy" id="cNguoiKy" />
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
                                <div class="form-actions">
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
    
    function CheckForm() {
        if ($("#cSoVanBan").val() == "") {
            alert("Vui lòng điền số văn bản!"); $("#cSoVanBan").focus(); return false;
        }
        if ($("#dNgayBanHanh").val() == "") {
            alert("Vui lòng chọn ngày ban hành văn bản!"); $("#dNgayBanHanh").focus(); return false;
        }
        if (!Validate_DateVN("dNgayBanHanh")) {
            return false;
        }
        //alert(huyen);
        //$.post("/Kiennghi/Ajax_View_chuongtrinh_daibieu", 'daibieu=' + daibieu, function (data) {
        //    //alert(data);
        //    $("#daibieu").html(data);
        //    HidePopup();
        //});
        //return false;
    }
</script>
