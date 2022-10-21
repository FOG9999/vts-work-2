<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Thêm mới đơn thư
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Kntc") %>
<div id="main" class="">
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                <li>
				    <a href="#">Thêm mới đơn thư</a>
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
					    <h3><i class="icon-plus-sign"></i>Thêm mới đơn thư</h3>
				    </div>
				    <div class="box-content">
					    <form method="post" id="form_" class="form-horizontal nomargin" onsubmit="return CheckForm();" enctype="multipart/form-data">
                        
                              <div class="control-group">
								    <label for="textfield" class="control-label f-red">Ngày nhận đơn*</label>
								    <div class="controls">
									    <input type="text" name="dNgayNhan" autocomplete="off" id="dNgayNhan"  class="input-medium datepick" />
								    </div>
							    </div>
                              <div class="control-group">
								    <label for="textfield" class="control-label f-red">Người nộp đơn*</label>
								    <div class="controls">
                                        <p><input type="text"  name="cNguoiGui_Ten" id="cNguoiGui_Ten"  class="input-block-level" /></p>
									    
								        <input type="checkbox" name="iDoanDongNguoi" class="nomargin" onchange="$('#doan').toggle();" /> Đoàn đông người
                                        <span id="doan" style="margin-left:15px; display:none">
                                        <input type="text" class="input-medium" name="iSoNguoi" id="iSoNguoi" onchange="CheckNum('iSoNguoi')" value="0" placeholder="Số người" /></span>
                                    </div>
							 </div>                            
                             <div class="control-group">
								<label for="textfield" class="control-label f-red">Nguồn đơn*</label>
								<div class="controls">
									<select class="input-block-level chosen-select" name="iNguonDon"><%=ViewData["opt-nguondon"] %></select>
								</div>
							</div>                             
                              <div class="control-group">
								    <label for="textfield" class="control-label f-red">Địa chỉ người nộp *</label>
								    <div class="controls">
									        <div class="span6">
                                                <select name="iDiaPhuong_0" onchange="ChangeTinhThanh('iDiaPhuong_1',this.value)" id="iDiaPhuong_0" class="span6 chosen-select">
                                                <option value="0">Chọn tỉnh thành</option><%=ViewData["opt-tinh"] %></select>
									       </div>
                                           <div class="span6">
                                                <select name="iDiaPhuong_1" id="iDiaPhuong_1" class="span6 chosen-select">
                                               <option value="0">Chọn huyện/thành phố/thị xã</option></select>
									       </div>
                                        <input type="text" class="input-block-level" name="cNguoiGui_DiaChi" placeholder="Số nhà, đường..." />
								    </div>
							    </div>
                               <div class="control-group span6 nomargin-left">
								    <label for="textfield" class="control-label">Quốc tịch</label>
								    <div class="controls">
									    <select name="iNguoiGui_QuocTich" class="input-large chosen-select"><%=ViewData["opt-quoctich"] %></select>
								    </div>
							    </div>
                                <div class="control-group span6 ">
								    <label for="textfield" class="control-label">Dân tộc</label>
								    <div class="controls">
									    <select name="iNguoiGui_DanToc" class="input-large chosen-select">
                                            <option value="0">Chưa xác định</option>
                                            <%=ViewData["opt-dantoc"] %>
                                        </select>
								    </div>
							    </div>                         
                              <div class="control-group">
								    <label for="textfield" class="control-label ">Số CMND; ngày cấp; nơi cấp</label>
								    <div class="controls">
									    <input type="text" name="cNguoiGui_CMND" class="input-block-level" />
								    </div>
							    </div>
                               <div class="control-group">
								    <label for="textfield" class="control-label ">Địa chỉ xảy ra vụ việc</label>
								    <div class="controls">
									    <input type="text" name="cDiaChi_VuViec" class="input-block-level" />
								    </div>
							    </div>
                               
                                <div class="control-group">
								    <label for="textfield" class="control-label f-red">Tóm tắt nội dung đơn*</label>
								    <div class="controls">
									    <textarea class="input-block-level" name="cNoiDung" id="cNoiDung"></textarea>
								    </div>
							    </div>
                                <div class="control-group span6 nomargin-left">
								    <label for="textfield" class="control-label">Lĩnh vực</label>
								    <div class="controls">
									    <select name="iLinhVuc" class="input-large chosen-select">
                                            <option value="0">Chưa xác định</option>
                                            <%=ViewData["opt-linhvuc"] %>
                                        </select>
								    </div>
							    </div>
                                <div class="control-group span6 ">
								    <label for="textfield" class="control-label">Loại đơn</label>
								    <div class="controls">
									    <select name="iLoaiDon" class="input-large chosen-select">
                                            <option value="0">Chưa xác định</option>
                                            <%=ViewData["opt-loaidon"] %>
                                        </select>
								    </div>
							    </div>
                                <div class="control-group span6 nomargin-left">
								    <label for="textfield" class="control-label">Nhóm nội dung</label>
								    <div class="controls">
									    <select name="iNoiDung" class="input-large chosen-select">
                                            <option value="0">Chưa xác định</option>
                                            <%=ViewData["opt-noidung"] %>
                                        </select>
								    </div>
							    </div>
                                <div class="control-group span6 ">
								    <label for="textfield" class="control-label">Tính chất vụ việc</label>
								    <div class="controls">
									    <select name="iTinhChat" class="input-large chosen-select">
                                            <option value="0">Chưa xác định</option>
                                            <%=ViewData["opt-tinhchat"] %>
                                        </select>
								    </div>
							    </div>
                            <!--
                                <div class="control-group span6 nomargin-left"">
								    <label for="textfield" class="control-label">Thẩm quyền xử lý</label>
								    <div class="controls">
									    <select name="iThuocThamQuyen" id="iThuocThamQuyen" onchange="ChangeThamQuyen(this.value)" class="input-block-level">
                                            <option value="-1">--- Vui lòng chọn</option>
                                            <option value="1">Đơn thuộc thẩm quyền xử lý</option>
                                            <option value="0">Đơn không thuộc thẩm quyền xử lý</option>
                                        </select>
								    </div>
							    </div>
                                <div class="control-group  span6" style="" id="thuocthamquyen">
								    <label for="textfield" class="control-label">Điều kiện xử lý</label>
								    <div class="controls">
									    <select name="iDuDieuKien" class="chosen-select">
                                            <option value="-1">Chưa xác định</option>
                                            <option value="1">Đủ điều kiện xử lý</option>
                                            <option value="0" >Không đủ điều kiện xử lý</option>
                                        </select>
								    </div>
							    </div>
                                -->
                                <div class="control-group">
								    <label for="textfield" class="control-label">Chuyển xử lý đơn</label>
								    <div class="controls">
									    <select name="iChuyenVien" class="input-large chosen-select">
                                            <option value="0">- - - Chọn chuyên viên xử lý</option>
                                            <%=ViewData["opt-chuyenvien-xuly"] %>
                                        </select>
								    </div>
							    </div>
                                <div class="control-group">
								    <label for="textfield" class="control-label ">File đính kèm</br>
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
                            <div id="submit-don">
                                <div class="control-group">
								    <label for="textfield" class="control-label "></label>
								    <div class="controls">
									    <input type="submit" value="Kiểm trùng đơn" class="btn btn-success" />                                       
                                        <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>
								    </div>
							    </div>
                            </div>
                                
                            
					    </form>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script type="text/javascript">
        function ChangeThamQuyen(val) {
            if (val == 1) {
                $("#thuocthamquyen").show();
            } else {
                $("#thuocthamquyen").hide();
            }
        }
        function CheckForm() {
            //alert($("[name=__RequestVerificationToken]").val());
            //$("[name=__RequestVerificationToken]").val("i4sQEtJtGVTUV85XK4DP69lOUjckIK4mJhlEUv-9nRH8mjN0m9_AWeKx8_h7PGmF5UfDtRIFCD7aPUk21nuj2k8Or1xmp793oid0Oc-R4kuPvjGc6krs3JOX5To4q04kOBBeeKdbPaXVt_xzGSGA9q3LSC-dU7s4HBEsa5RkAaA1");
            if ($("#dNgayNhan").val() == "") {
                alert("Vui lòng chọn ngày nhận đơn"); $("#dNgayNhan").focus();
                return false;
            }
            if (!Validate_DateVN("dNgayNhan")) {
                return false;
            }
            if ($("#cNguoiGui_Ten").val() == "") {
                alert("Vui lòng nhập người nộp đơn"); $("#cNguoiGui_Ten").focus();
                return false;
            }
            if ($("#iDiaPhuong_0").val() == 0) {
                alert("Vui lòng chọn địa chỉ tỉnh/thành người gửi đơn");
                return false;
            }
            if ($("#cNoiDung").val() == "") {
                alert("Vui lòng nhập tóm tắt nội dung đơn"); $("#cNoiDung").focus();
                return false;
            }
        }
    </script>
</asp:Content>
