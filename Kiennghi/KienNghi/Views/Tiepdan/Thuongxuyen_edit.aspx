<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="KienNghi.Models" %>
<%@ Import Namespace="KienNghi.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Cập nhật vụ việc tiếp công dân thường xuyên
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Tiepdan") %>
<div id="main" class="">
     <a href="#" class="show_menu_trai">Menu trái</a>
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                    <li>
				    <span> Tiếp công dân   <i class="icon-angle-right"></i>  </span>
			    </li>
                <li>
				    <span>Cập nhật vụ việc tiếp công dân thường xuyên</span>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>
        
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i>Cập nhật vụ việc tiếp công dân thường xuyên</h3>
				    </div>
				    <div class="box-content nopadding">
					    <form method="post" id="form_" class="nomargin" onsubmit="return CheckForm();" enctype="multipart/form-data">
                            
                            <% tiepdan_thuongxuyen t = (tiepdan_thuongxuyen)ViewData["tiepdan"]; %>
                            <table class="table table-condensed table-bordered form4">
                                    <tr>
                                        <th colspan="4">Thông tin vụ việc tiếp nhập</th>
                                    </tr>
                                    <tr>
                                        <td class="f-red">Ngày tiếp *</td>
                                        <td>
                                            <input type="text" name="dNgayTiep" value="<%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(t.dNgayTiep.ToString()))%>" id="dNgayTiep" class="input-medium datepick" />
                                        </td>
                                        <td class="">Địa điểm</td>
                                        <td>
                                            <input name="cDiaDiem" id="cDiaDiem" type="text" value="<%=t.cDiaDiem %>" class="input-block-level" />
                                        </td>
                                    </tr>  
                                    <tr>
                                        <td class="f-red" >Cơ quan tiếp *</td>
                                        <td><select class="input-block-level" name="iCoQuanTiepDan" id="iCoQuanTiepDan">
                                            
                                            <%=ViewData["opt-coquantiepdan"] %>
                                            </select></td>
                                        <td class="f-red" >Cán bộ tiếp *</td>
                                        <td><input type="text" name="cNguoiTiep" value="<%=t.cNguoiTiep %>" id="cNguoiTiep" class="input-block-level" /></td>
                                        
                                    </tr>                              
                                    <tr>
                                        <td class="f-red" width="15%">Người gửi *</td>
                                        <td width="35%">
                                            <input type="text" name="cNguoiGui_Ten" value="<%=t.cNguoiTiep %>" id="cNguoiGui_Ten" class="input-block-level" />
                                        </td>
                                        <td class="f-red" width="15%">Địa chỉ người gửi *</td>
                                        <td width="35%">
                                            <input type="text" name="cNguoiGui_DiaChi" value="<%=t.cNguoiGui_DiaChi %>" id="cNguoiGui_DiaChi" class="input-block-level" />
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td></td>
                                        <td colspan="3">
                                            <input type="checkbox" name="iDoan" <% if (t.iDoan == 1) {%>checked<% } %> class="nomargin" onchange="$('#doan').toggle();" /> Đoàn đông người
                                            <span id="doan" style="margin-left:15px; <% if (t.iDoan == 0) {%>display:none<% } %>">
                                                <input type="text" class="input-medium" id="iDoan_Nguoi" onchange="CheckNum('iDoan_Nguoi')" name="iDoan_Nguoi"  value="<%=t.iDoan_Nguoi %>" /></span>
                                        </td>
                                    </tr>                               
                                    <tr>
                                        <td class="f-red">Tóm tắt nội dung vụ việc *</td>
                                        <td colspan="3" >
                                            <textarea name="cNoiDung" id="cNoiDung" class="input-block-level"><%=t.cNoiDung %></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">Đơn kèm theo</br>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</em></td>
                                        <td colspan="3" >
                                            <%=ViewData["file_tiepdan"] %>
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
                                        <th colspan="4">Phân loại vụ việc</th>
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
                                        <th colspan="4">Kết quả trả lời, giải quyết</th>
                                    </tr>
                                <% tiepdan_thuongxuyen_ketqua k = (tiepdan_thuongxuyen_ketqua)ViewData["ketqua"]; %>
                                    <tr>
                                        <td class="">Người trả lời, giải quyết</td>
                                        <td colspan="3" >
                                            <input type="text" name="cKetQua_NguoiTraLoi" value="<%=k.cKetQua_NguoiTraLoi %>" class="input-block-level"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">Kết quả trả lời, giải quyết</td>
                                        <td colspan="3" >
                                            <textarea name="cKetQua" class="input-block-level"><%=k.cKetQua %></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">Văn bản trả lời</td>
                                        <td colspan="3" >
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
                                <tr>
                                    <td></td>
                                    <td colspan="3">
                                        <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                        
                                        <input type="submit" value="Cập nhật" class="btn btn-success" />                                       
                                        <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>
                                    </td>
                                </tr>
                                </table>
					    </form>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script type="text/javascript">
        function CheckForm() {
            if ($("#dNgayTiep").val() == "") { alert("Vui lòng chọn ngày tiếp"); $("#dNgayTiep").focus(); return false; }
            if (!Validate_DateVN("dNgayTiep")) {
                return false;
            }
            if ($("#iCoQuanTiepDan").val() == 0) { alert("Vui lòng chọn cơ quan tiếp"); $("#iCoQuanTiepDan").focus(); return false; }
            if ($("#cNguoiTiep").val() == "") { alert("Vui lòng nhập người tiếp"); $("#cNguoiTiep").focus(); return false; }
            if ($("#cNguoiGui_Ten").val() == "") { alert("Vui lòng nhập tên người gửi"); $("#cNguoiGui_Ten").focus(); return false; }
            if ($("#cNguoiGui_DiaChi").val() == "") { alert("Vui lòng nhập địa chỉ người gửi"); $("#cNguoiGui_DiaChi").focus(); return false; }
            if ($("#cNoiDung").val() == "") { alert("Vui lòng nhập nội dung vụ việc"); $("#cNoiDung").focus(); return false; }
            
        }
    </script>
</asp:Content>
