<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>

<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Cập nhật trả lời kiến nghị
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form onsubmit="return CheckForm();" method="post" name="_form" id="_form" class="form-horizontal" enctype="multipart/form-data" action="/Kiennghi/Kiennghi_traloi_update" >
                                <% KN_KIENNGHI_TRALOI t = (KN_KIENNGHI_TRALOI)ViewData["traloi"]; %>
                                <div class="row-fluid">
                                   <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Phân loại kết quả trả lời <i class="f-red">*</i></label>
							                <div class="controls">
                                                <div>
                                                    <select class="input-block-level" name="iPhanLoai" id="iPhanLoai" onchange="ChangePhanloai(this.value)">
                                                        <%=ViewData["opt-phanloai"] %>
                                                    </select>
                                                </div> 
                                                <div id="phanloai_child" style="margin-top:10px"><%=ViewData["select-phanloai"] %></div>
							                </div>
						                </div>
                                   </div>
                                    <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Số công văn <i class="f-red">*</i></label>
							                <div class="controls">
                                                <input type="text" value="<%=Server.HtmlEncode(t.CSOVANBAN) %>" name="cSoVanBan" id="cSoVanBan" class="input-medium" />
							                </div>
						                </div>
                                   </div>
                                </div>
                                
                                <div class="row-fluid">
                                    <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Ngày ban hành <i class="f-red">*</i></label>
							                <div class="controls">
                                                
                                                <input type="text" value="<% if (t.DNGAYBANHANH != null) { Response.Write(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(t.DNGAYBANHANH))); } %>" name="dNgayBanHanh" id="dNgayBanHanh" class="input-medium datepick" autocomplete ="off" />
							                </div>
						                </div>
                                   </div>
                                    <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Người ký </label>
							                <div class="controls">
                                                <input type="text" value="<%=Server.HtmlEncode(t.CNGUOIKY) %>" name="cNguoiKy" id="cNguoiKy" class="input-block-level" />
							                </div>
						                </div>
                                   </div>
                                </div>
                                <div class="row-fluid">
                                        <div class="control-group">
							                <label for="textfield" class="control-label">Kết quả xử lý <i class="f-red">*</i></label>
							                <div class="controls">
                                                <textarea class="input-block-level" rows="4" id="cNoiDung" name="cNoiDung"><%=Server.HtmlEncode(t.CTRALOI) %></textarea>
							                </div>
						                </div>
                                    </div>   
                               <div id="div_lotrinh" <%=ViewData["colotrinh"] %>>
                                    <div class="row-fluid">    
                                        <div class="control-group">
		                                    <label for="textfield" class="control-label">Ngày dự kiến hoàn thành </label>
		                                    <div class="controls">
                                                <input type="text" name="DNGAY_DUKIEN" id="DNGAY_DUKIEN" class="datepick input-medium" 
                                                    value="<% if (t.DNGAY_DUKIEN != null) { Response.Write(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(t.DNGAY_DUKIEN))); } %>" />
		                                    </div>
	                                    </div>   
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
    $("#cSoVanBan").focus();
    function ChangePhanloai(val) {
        if (val != 0) {
            $.post("/Kiennghi/Ajax_Change_Phanloai_traloi_option", 'id=' + val, function (data) {
                $("#phanloai_child").show().html(data);
            });
        } else {
            $("#phanloai_child").html(""); 
        }
        $("#div_lotrinh").hide(); $("#DNGAY_DUKIEN").val();
    }
    function ChangePhanLoai1(val, val1) {
        if (val == val1) {
            $("#div_lotrinh").show();
        } else {
            $("#div_lotrinh").hide(); $("#DNGAY_DUKIEN").val();
        }
    }
    function CheckForm() {
        if ($("#iPhanLoai").val() == 0) { alert("Vui lòng chọn phân loại kết quả trả lời!"); $("#iPhanLoai").focus(); return false; }
        if ($("#iPhanLoai1").length) {
            if ($("#iPhanLoai1").val() == 0) {
                alert("Vui lòng chọn phân loại kết quả trả lời tiếp theo!"); $("#iPhanLoai1").focus(); return false;
            }
        }
        if ($("#cSoVanBan").val() == "") { alert("Vui lòng nhập số công văn!"); $("#cSoVanBan").focus(); return false; }
        if ($("#dNgayBanHanh").val() == "") { alert("Vui lòng chọn ngày ban hành văn bản!"); $("#dNgayBanHanh").focus(); return false; }
        if (!Validate_DateVN("dNgayBanHanh")) {
            return false;
        }
        if ($("#cNoiDung").val() == "") { alert("Vui lòng nhập nội dung trả lời kiến nghị!"); $("#cNoiDung").focus(); return false; }
    }
</script>
