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
								<%--<i class="icon-reorder"></i> Chuyển xử lý Tập hợp kiến nghị--%>
                                <i class="icon-reorder"></i>Chuyển công văn tập hợp kiến nghị
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat()" enctype="multipart/form-data" action="/Kiennghi/Chuyen_donvi_xuly_insert" >                                
                                 
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Cơ quan nhận </label>
							        <div class="controls">
                                        <%=ViewData["radio-thamquyen"] %>
                                        <div class="input-block-level" id="TrungUong">
                                            <select name="iThamQuyenDonVi" id ="iThamQuyenDonVi" class="chosen-select">                                            
                                                <%=ViewData["opt-donvithamquyen"] %>
                                            </select>
                                        </div>
                                        <%--<select name="iDonVi" id="iDonVi" class="input-block-level">
                                            <option value="0">--- Chọn cơ quan</option>  
                                            <%=ViewData["opt_donvi"] %>                                          
                                        </select>--%>
							        </div>
						        </div>  
                                <div class="control-group">
							        <label for="textfield" class="control-label">Số công văn </label>
							        <div class="controls">
                                        <input type="text" class="input-medium" id="cSoVanBan" name="cSoVanBan" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Ngày ban hành</label>
							        <div class="controls">
                                        <input type="text" autocomplete="off" class="input-medium datepick" id="dNgayBanHanh" name="dNgayBanHanh" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label "> Ngày yêu cầu phản hồi <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" autocomplete="off" class="input-medium datepick" id="dNgayPhanHoi" name="dNgayPhanHoi" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Người ký</label>
							        <div class="controls">
                                        <input type="text" class="input-medium" name="cNguoiKy" />
							        </div>
						        </div>
                                <%--<div class="control-group">
							        <label for="textfield" class="control-label">Nội dung tóm tắt</label>
							        <div class="controls">
                                        <textarea class="input-block-level" name="cNoiDung"></textarea>
							        </div>
						        </div>--%>
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
                                    <button type="submit" class="btn btn-success">Chuyển xử lý</button>
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
        //if ($("#iDonVi").val() == 0) { alert("Vui lòng chọn cơ quan chuyển đến!"); return false; }
        //if ($("#cSoVanBan").val() == "") {
        //    alert("Vui lòng nhập số công văn"); $("#cSoVanBan").focus();
        //    return false;
        //}
        //if ($("#dNgayBanHanh").val() == "") {
        //    alert("Vui lòng chọn ngày ban hành"); $("#dNgayBanHanh").focus();
        //    return false;
        //}
        if ($("#dNgayPhanHoi").val() == "") {
            alert("Vui lòng chọn ngày phản hồi"); $("#dNgayPhanHoi").focus();
            return false;
        }
        if (!Validate_DateVN("dNgayPhanHoi")) {
            return false;
        }

    }
    function ChangeLinhVucByDonVI(val) {
        $.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
            //alert(data);
            $("#div_linhvuc").html('<select name="iLinhVuc" id="iLinhVuc" class="chosen-select">' + data + '</select>');
            //$('#iChuongTrinh').trigger('chosen:updated');
            $("#iLinhVuc").chosen();
        });
    }
    $("#iThamQuyenDonVi").chosen();
    function DoiThamQuyenDonVi(val) {
        $.post("/Kiennghi/Ajax_Get_ThamQuyen_DonVi", '&val=' + val, function (data) {
            $("#iThamQuyenDonVi").html('<select name="iDonViTraLoi" id="iThamQuyenDonVi" class="chosen-select">' + data + '</select>');
            $("#iThamQuyenDonVi").trigger("liszt:updated");
            $("#iThamQuyenDonVi").chosen();
        });
    }
</script>
