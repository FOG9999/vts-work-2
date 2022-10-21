<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Trả lời kiến nghị
							</h3>
                            
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" onsubmit="return CheckForm();" name="_form" id="_form" class="form-horizontal form-column" enctype="multipart/form-data" action="/Kiennghi/Kiennghi_traloi_insert" >
                                <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label">Đơn vị trả lời <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <%--<select id="iDonViTraLoi" name="iDonViTraLoi" class="chosen-select">
                                                <%=ViewData["opt-donvitraloi"] %>
                                            </select>--%>
                                            <%=ViewData["radio-thamquyen"] %>
                                            <div class="input-block-level">
                                                <select id="iThamQuyenDonVi" name="iDonViTraLoi" class="chosen-select">
                                                    <%=ViewData["opt-donvithamquyen"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>  
                                </div>
                                <div class="row-fluid">
                                   <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Phân loại kết quả trả lời <i class="f-red">*</i></label>
							                <div class="controls">
                                                <div>
                                                    <select class="input-block-level chosen-select" name="iPhanLoai" id="iPhanLoai" onchange="ChangePhanloai(this.value)">
                                                        <option value="0">Vui lòng chọn</option>
                                                        <%=ViewData["opt-phanloai"] %>
                                                    </select>
                                                </div> 
                                                <div id="phanloai_child" style="margin-top:10px"></div>                                           
							                </div>
						                </div>
                                   </div>
                                    <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Số công văn <i class="f-red">*</i></label>
							                <div class="controls">
                                                <input type="text" name="cSoVanBan" id="cSoVanBan" class="input-medium" />
							                </div>
						                </div>
                                   </div>
                                </div>
                                
                                <div class="row-fluid">
                                    <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Ngày ban hành <i class="f-red">*</i></label>
							                <div class="controls">
                                                <input type="text" name="dNgayBanHanh" id="dNgayBanHanh" class="input-medium datepick" autocomplete ="off"/>
							                </div>
						                </div>
                                   </div>
                                    <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Người ký </label>
							                <div class="controls">
                                                <input type="text" name="cNguoiKy" id="cNguoiKy" class="input-block-level" />
							                </div>
						                </div>
                                   </div>
                                </div>
                                <% if (ViewData["thamquyen_diaphuong"].ToString() == "1"){ %>
                                <div class="row-fluid">
                                    <div class="span12">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Đơn vị trả lời </label>
							                <div class="controls">
                                                <input placeholder="Sở; ban; ngành... trực thuộc địa phương" type="text" name=" cCoQuanTraLoi" id="cCoQuanTraLoi" class="input-block-level" />
							                </div>
						                </div>
                                   </div>
                                </div>
                                <% } %>
                                <div class="row-fluid">    
                                    <div class="control-group">
	                                    <label for="textfield" class="control-label ">Nội dung trả lời <i class="f-red">*</i></label>
	                                    <div class="controls">
                                            <textarea class="input-block-level" rows="4" id="cNoiDung" name="cNoiDung"></textarea>                                        
	                                    </div>
                                    </div>    
                                </div>
                                <div class="row-fluid">    
                                    <div class="control-group">
	                                    <label for="textfield" class="control-label ">Ghi chú </label>
	                                    <div class="controls">
                                            <textarea class="input-block-level" rows="4" id="cGhiChu" name="cGhiChu"></textarea>                                        
	                                    </div>
                                    </div>    
                                </div>
                                <div id="div_lotrinh" style="display:none">
                                    <div class="row-fluid">    
                                        <div class="control-group">
		                                    <label for="textfield" class="control-label">Ngày dự kiến hoàn thành </label>
		                                    <div class="controls">
                                                <input type="text" name="DNGAY_DUKIEN" id="DNGAY_DUKIEN" class="datepick input-medium" value="" />
		                                    </div>
	                                    </div>   
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
						        <div class="form-actions">
                                    <button type="submit" class="btn btn-success">Lưu</button>
                                    <input type="hidden" name="iTinhTrang" value="1" />
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
    $('#iDonViTraLoi').chosen();
    $('#iPhanLoai').chosen();
    $('#iPhanLoai1').chosen();
    $("#iThamQuyenDonVi").chosen();
    function ChangePhanloai(val) {
        debugger;
        if (val != 0) {
            $.post("/Kiennghi/Ajax_Change_Phanloai_traloi_option", 'id=' + val, function (data) {
                $("#phanloai_child").show().html(data);
                $("#iPhanLoai1").trigger("liszt:updated");
                $('#iPhanLoai1').chosen();
            });
        } else {
            $("#phanloai_child").html(""); $("#div_lotrinh").hide(); $("#DNGAY_DUKIEN").val();
        }              
    }
    function DoiThamQuyenDonVi(val) {
        $.post("/Kiennghi/Ajax_Get_ThamQuyen_DonVi", '&val=' + val, function (data) {
            $("#iThamQuyenDonVi").html('<select name="iDonViTraLoi" id="iThamQuyenDonVi" class="chosen-select">' + data + '</select>');
            $("#iThamQuyenDonVi").trigger("liszt:updated");
            $("#iThamQuyenDonVi").chosen();
        });
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
        if ($("#cNoiDung").val().length >= 4000) { alert("Vui lòng nhập nội dung trả lời ngắn hơn 4000 ký tự!"); $("#cNoiDung").focus(); return false; }
        if ($("#cGhiChu").val().length >= 500){ alert("Vui lòng nhập ghi chú ngắn hơn 500 ký tự!"); $("#cGhiChu").focus(); return false; } 
    }
</script>