<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Cập nhật vụ việc trong tiếp dân định kỳ
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>                        
                        <div class="box-content popup_info nopadding">
                            <% TIEPDAN_DINHKY_VUVIEC v = (TIEPDAN_DINHKY_VUVIEC)ViewData["vuviec"]; %>
                            <form method="post" id="form_" class="nomargin" action="/TiepDan/Vuviec_update" onsubmit="return CheckForm();" enctype="multipart/form-data">
                            
                                <table class="table table-condensed table-bordered form4">
                                     <tr>
                                        <td class="f-red" width="15%">Người tiếp *</td>
                                        <td width="35%">
                                            <input type="text" value="<%=v.CNGUOITIEP %>" name="cNguoiTiep" id="cNguoiTiep" class="input-block-level" />
                                        </td>
                                        <td class="" width="15%">Đoàn đông người</td>
                                        <td width="35%">
                                            <input type="checkbox" name="iDoan" <% if (v.IDOAN == 1) {%>checked<% } %> class="nomargin" onchange="$('#doan').toggle();" /> Đoàn đông người
                                            <span id="doan" style="margin-left:15px; <% if (v.iDoan == 0) {%>display:none<% } %>">
                                                <input type="text" class="input-medium" id="iDoan_Nguoi" onchange="CheckNum('iDoan_Nguoi')" name="iDoan_Nguoi"  value="<%=v.IDOAN_NGUOI %>" /></span>
                                        </td>
                                    </tr>                  
                                    <tr>
                                        <td class="f-red" width="15%">Người gửi *</td>
                                        <td width="35%">
                                            <input type="text" value="<%=v.CNGUOIGUI_TEN %>" name="cNguoiGui_Ten" id="cNguoiGui_Ten" class="input-block-level" />
                                        </td>
                                        <td class="f-red" width="15%">Địa chỉ người gửi *</td>
                                        <td width="35%">
                                            <input type="text" value="<%=v.CNGUOIGUI_DIACHI %>" name="cNguoiGui_DiaChi" id="cNguoiGui_DiaChi" class="input-block-level" />
                                        </td>
                                    </tr> 
                                                                
                                    <tr>
                                        <td class="f-red">Tóm tắt nội dung vụ việc *</td>
                                        <td colspan="3" >
                                            <textarea name="cNoiDung" id="cNoiDung" class="input-block-level"><%=v.CNOIDUNG %></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="" colspan="2">Đơn kèm theo</br>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</em></td>
                                        <td colspan="2" >
                                            <%=ViewData["file_vuviec"] %>
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
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td class="">Loại vụ việc</td>
                                        <td><select name="iLoai" id="iLoai" class="input-block-level"> 
                                            <option value="0">- - - Chưa xác định</option>                                          
                                            <%=ViewData["opt-loaidon"] %></select>                                        
                                        </td>
                                        <td class="">Lĩnh vực</td>
                                        <td><select name="iLinhVuc" id="iLinhVuc" class="input-block-level">     
                                                <option value="0">- - - Chưa xác định</option>                                               
                                                <%=ViewData["opt-linhvuc"] %>
                                            </select>                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nhóm nội dung</td>
                                        <td><select name="iNoiDung" class="input-block-level">
                                            <option value="0">- - - Chưa xác định</option>    
                                            <%=ViewData["opt-noidung"] %></select>                                        
                                        </td>
                                        <td>Tính chất vụ việc</td>
                                        <td><select name="iTinhChat" class="input-block-level">
                                            <option value="0">- - - Chưa xác định</option>    
                                                <%=ViewData["opt-tinhchat"] %>
                                            </select>                                        
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="">Kết quả trả lời, giải quyết</td>
                                        <td colspan="3" >
                                            <textarea name="cKetQua" class="input-block-level"><%=v.CTRALOI %></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="">Văn bản trả lời</br>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</em></td>
                                        <td colspan="2" >
                                            <%=ViewData["file_traloi"] %>
                                            <% for (int i = 1; i < 4; i++)
                                                {
                                                    string style_none = ""; if (i > 1) { style_none = "display:none; margin-top:5px;"; }
                                                    string change = "";
                                                    if (i < 3)
                                                    {
                                                        int j = i + 1;
                                                        change = "$('.ketqua"+j+"').show()";
                                                    }
                                                    %>
                                                    <div class="input-group file-group ketqua<%=i %>" style="<%=style_none%>">
                                                        <span class="input-group-btn">
                                                            <span class="btn btn-success btn-file">
                                                                Duyệt file
                                                                <input onchange="CheckFileTypeUpload('ketqua_upload<%=i %>','ketqua_name<%=i %>');<%=change %>" 
                                                                    name="ketqua_upload<%=i %>" id="ketqua_upload<%=i %>" type="file">
                                                            </span>
                                                        </span>
                                                        <input class="input-xlarge" disabled id="ketqua_name<%=i %>" type="text">
                                                        <span class="btn btn-danger" onclick="$('#ketqua_upload<%=i %>,#ketqua_name<%=i %>').val('');" title="Hủy"><i class="icon-trash"></i></span>
                                                    </div>
                                            <% } %>

                                        </td>
                                    </tr>
                                
                                </table>
                                <p class="tcenter">
                                    <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                <input type="submit" value="Cập nhật" class="btn btn-success" />  
                                <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span></p>
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
            if ($("#cNguoiTiep").val() == "") { alert("Vui lòng nhập người tiếp"); $("#cNguoiTiep").focus(); return false; }
            if ($("#cNguoiGui_Ten").val() == "") { alert("Vui lòng nhập tên người gửi"); $("#cNguoiGui_Ten").focus(); return false; }
            if ($("#cNguoiGui_DiaChi").val() == "") { alert("Vui lòng nhập địa chỉ người gửi"); $("#cNguoiGui_DiaChi").focus(); return false; }
            if ($("#cNoiDung").val() == "") { alert("Vui lòng nhập nội dung vụ việc"); $("#cNoiDung").focus(); return false; }
            
        }
    </script>