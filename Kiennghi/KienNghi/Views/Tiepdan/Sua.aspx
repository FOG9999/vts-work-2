<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Chỉnh sửa tiếp dân
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Tiepdan") %>
    <div id="main" class="">
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
				    <span>Chỉnh sửa vụ việc tiếp dân</span>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>
         <% TD_VUVIEC k = (TD_VUVIEC)ViewData["ID"]; %>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i>Chỉnh sửa vụ việc tiếp dân</h3>
				    </div>
				      <div class="box-content" style="text-align: left;">     
                        <form  class="form-horizontal form-column" id="form_" name="form_"  onsubmit="return CheckForm()" enctype="multipart/form-data" method="post">  
					        
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Chọn loại hình tiếp <i class="f-red">*</i></label>
                                        <div class="controls">
                                              <select  name="iLoaiTiep"  id="iLoaiTiep" onchange="LoadLich(this.value)"  class="input-block-level chosen-select" >
                                                  <option value="-1"> - - - Chưa xác định</option>
                                            <option value="1" <% if (k.IDINHKY == 0) { Response.Write("selected"); } %>> - - - Tiếp thường xuyên  </option>
                                            <option value="0" <% if (k.IDINHKY != 0) { Response.Write("selected"); } %>> - - - Tiếp định kỳ </option>
                                                   <option value="2" <% if (k.ITIEPDOTXUAT != 0) { Response.Write("selected"); } %>> - - - Tiếp đột xuất </option>
                                           
                                       </select>

                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Lịch tiếp <i class="f-red">*</i></label>
                                        <div class="controls">
                                            <div class="input-block-level" id="Loadlichtiep">
                                           <% string dStlye = " display:block";
                                            string dStlye2 = " display:none";
                                            string dStyle3 = "chosen-select";
                                            if (k.IDINHKY == 0 )
                                            {
                                                dStyle3 = "";
;                                                   dStlye = " display:none";
                                                   dStlye2 = " display:block";
                                               }
                                            %>
                                       <select class="input-block-level <%=dStyle3 %> " name="iTiepDinhKy" id="iTiepDinhKy" style="<%=dStlye %>" onchange="LoadThongTinLich()" >

                                           <option value="0"> - - - Chọn lịch tiếp </option>
                                           <%= ViewData["opt_tiepdinhky"] %>

                                       </select>
                                         <input type="text" name="dNgayNhan"  id="dNgayNhan"  value="<%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(k.DNGAYNHAN.ToString())) %>" class="input-medium datepick"  style="<%=dStlye2 %>"/>
                                     

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Lãnh đạo tiếp</label>
                                        <div class="controls">
                                              <select name="iLanhDaoTiep" id="iLanhDaoTiep"  class="input-block-level chosen-select"  >
                                                   <option value="0">- - - Chọn lãnh đạo</option>
                                                  <%=ViewData["Lanhdao"] %>
                                                </select>

                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label " id="tieudetheolich"><%=ViewData["TenTieuDe"] %></label>
                                        <div class="controls" id="ngaytieptheolich">
                                              <%= ViewData["ngaytiep"] %>
                                            <script>  $("#dNgayNhan").datepicker();</script>
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Tên công dân đến <i class="f-red">*</i></label>
                                        <div class="controls">
                                           <input type="text"  name="cNguoiGui_Ten" id="cNguoiGui_Ten" value="<%=Server.HtmlEncode(k.CNGUOIGUI_TEN) %>"  class="input-block-level" />     
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Người tiếp <i class="f-red">*</i></label>
                                        <div class="controls">
                                             <input type="text"  name="cNguoiTiep_Ten" id="cNguoiTiep_Ten" value="<%=Server.HtmlEncode(k.CNGUOITIEP) %>"  class="input-block-level" />    
                                        </div>           
                                    </div>
                                </div>
                            </div><!-- end row-fluid-->
                            <div class="row-fluid">
                                <div class="control-group">
                                    <div class="span12">
                                            <div class="controls">
                                                <label class="checkbox">
                                                <% string dStlyec = " display:none";
                                            string check = "";
                                            if (k.ISONGUOI >= 2 )
                                               {
                                                   dStlyec = " display:block";
                                                   check = "checked";
                                                
                                                  
                                               }
                                            %>
                                        <input type="checkbox" name="iDoanDongNguoi" id="iDoanDongNguoi" <%=check %> class="" onchange="$('#doan').toggle();" /> Đoàn đông người
                                       
                                            <input type="text" class="input-medium" name="iSoNguoi" id="iSoNguoi" onchange="CheckNum('iSoNguoi')" value="<%=k.ISONGUOI %>" placeholder="Số người" />
                                                </label>
                                                 
                                            </div>
                                        
                                    </div>
                                </div>
                            </div><!-- end row-fluid-->
                            <div class="row-fluid" >
                                <div class="control-group">
                                      <div class="span12">
                                    <label for="textfield" class="control-label ">Địa chỉ người nộp <i class="f-red">*</i></label>
                                    <div class="controls">
                                        <div class="input-xlarge span4"><select name="iDiaPhuong_0" onchange="ChangeTinhThanh('iDiaPhuong_1',this.value)" id="iDiaPhuong_0" class="chosen-select">
                                            <option value="0">Chọn tỉnh thành</option>
                                            <%=ViewData["opt-tinh"] %>
                                            <option value="-1">Khác</option>
                                                         </select></div>
                                        <div class="input-xlarge span4"><div  id="iDiaPhuong_1">
                                                <select <% if ((int)k.IDIAPHUONG_0 == -1) { Response.Write("disabled"); } %> name="iDiaPhuong_1" class="chosen-select"><option value="0">Chọn huyện/thành phố/thị xã</option><%=ViewData["opt-huyen"] %></select>
                                            </div></div>
                                        <div class="span4">   <input type="text" class="input-block-level"  name="cNguoiGui_DiaChi" id="cNguoiGui_DiaChi" value="<%=Server.HtmlEncode(k.CNGUOIGUI_DIACHI) %>" placeholder="Số nhà, đường..." /></div>
                                    </div>
                                          </div>
                                </div>
                            </div><!-- end row-fluid-->
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Quốc tịch</label>
                                        <div class="controls">
                                            <div class="input-block-level">
                                                  <select name="iNguoiGui_QuocTich" class="input-block-level chosen-select"><%=ViewData["opt-quoctich"] %></select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Dân tộc</label>
                                        <div class="controls">
                                            <div class="input-block-level">
                                               <select name="iNguoiGui_DanToc" class="input-block-level chosen-select">
                                            <%=ViewData["opt-dantoc"] %>
                                        </select>    

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div><!-- end row-->
                            <div class="row-fluid">
                                <div class="span12">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Tóm tắt nội dung vụ việc <i class="f-red">*</i></label>
                                    <div class="controls">
                                         <div class="input-block-level">
                                       <textarea class="input-block-level" name="cNoiDung" id="cNoiDung"><%=Server.HtmlEncode(k.CNOIDUNG) %></textarea>
                                             </div>
                                    </div>
                                </div>
                                </div>
                            </div><!-- end row-->
                            <div class="row-fluid" >
                                <div class="span12">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Nội dung chỉ đạo</label>
                                    <div class="controls">
                                         <div class="input-block-level">
                                       <textarea class="input-block-level" name="cNoiDungChiDao" id="cNoiDungChiDao"><%=Server.HtmlEncode(k.CNOIDUNGCHIDAO) %></textarea>
                                             </div>
                                    </div>
                                </div>
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Loại vụ việc <i class="f-red">*</i></label>
                                    <div class="controls">
                                       <select name="iLoaiDon" id="iLoaiDon" class="input-medium chosen-select" onchange="LoadLinhVucByLoaiDon(this.value)"  >
                                                <option value="0">- - - Chưa xác định</option>
                                                <%=ViewData["opt-loaidon"] %>
                                            </select>
                                    </div>
                                </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Lĩnh vực <i class="f-red">*</i></label>
                                    <div class="controls" id="ip_linhvuc">
                                        <select name="iLinhVuc" id="iLinhVuc" class="chosen-select"  onchange="LoadLinhVuc()"  style="width: 100%">
                                                <option value="0">- - - Chưa xác định</option>
                                                <%=ViewData["opt-linhvuc"] %>
                                            </select> 

                                    </div>
                                </div>
                                </div>
                                </div>
                             <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Nhóm nội dung </label>
                                    <div class="controls" id="LoadNoiDung">
                                       <select name="iNoiDung" class="input-block-level chosen-select">
                                        <option value="0">- - - Chưa xác định</option>
                                        <%=ViewData["opt-noidung"] %></select>      
                                    </div>
                                </div>
                                </div>
                                 <div class="span6">
                                     <div class="control-group">
                                    <label for="textfield" class="control-label">Tính chất vụ việc</label>
                                    <div class="controls" id="LoadTinhChat">
                                         <select name="iTinhChat" class="input-block-level chosen-select">
                                        <option value="0">- - - Chưa xác định</option>
                                            <%=ViewData["opt-tinhchat"] %>
                                        </select>     
                                    </div>
                                </div>
                                 </div>
                            </div>
                            
                            <!-- end row-->
                            <div class="row-fluid">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">File đính kèm
                                        <small>Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</small></label>
                                    <div class="controls">
                                       <%=ViewData["XoaFile"] %>
                                            <% for (int i = 1; i < 4; i++)
                                               {
                                                   string style_none = ""; if (i > 1) { style_none = "display:none; margin-top:5px;"; }
                                                   string change = "";
                                                   if (i < 3)
                                                   {
                                                       int j = i + 1;
                                                       change = "$('.upload" + j + "').show()";
                                                   }
                                            %>
                                            <div class="input-group file-group upload<%=i %>" style="<%=style_none%>">
                                                <span class="input-group-btn">
                                                    <span class="btn btn-success btn-file">Duyệt file
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
                            </div>
                            <div class="row-fluid">
                                <div class="form-actions" >
                                         <input type="hidden" name="id_vuviec" id="id_vuviec" value="<%=ViewData["id_vuviec"] %>"  />
                                    <input type="submit" value="Cập nhật vụ việc" class="btn btn-success" />
                                    <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning" onclick="ShowPageLoading()">Quay lại</a>
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
      
        function CheckForm() {
            //alert($("[name=__RequestVerificationToken]").val());
            //$("[name=__RequestVerificationToken]").val("i4sQEtJtGVTUV85XK4DP69lOUjckIK4mJhlEUv-9nRH8mjN0m9_AWeKx8_h7PGmF5UfDtRIFCD7aPUk21nuj2k8Or1xmp793oid0Oc-R4kuPvjGc6krs3JOX5To4q04kOBBeeKdbPaXVt_xzGSGA9q3LSC-dU7s4HBEsa5RkAaA1");
            if ($("#iLoaiTiep").val() == -1) {
              
                    alert("Vui lòng chọn loại hình tiếp");
                    return false;
                
            }
            if ($("#iLoaiTiep").val() == 0) {
                if ($("#iTiepDinhKy").val() == 0) {
                    alert("Vui lòng chọn lịch tiếp");
                    return false;
                }
            }
           
                if ($("#dNgayNhan").val() == "") {
                    alert("Vui lòng chọn ngày tiếp"); $("#dNgayNhan").focus();
                    return false;
                }
          
            if ($("#cNguoiGui_Ten").val() == "") {
                alert("Vui lòng nhập tên công dân đến"); $("#cNguoiGui_Ten").focus();
                return false;
            }

            if ($('#iDoanDongNguoi:checkbox:checked').length > 0) {
                if ($("#iSoNguoi").val() <= 1) {
                    alert("vui lòng nhập số người lớn hơn"); $("#iSoNguoi").focus();
                    return false;
                }
            }
            if ($("#iDiaPhuong_0").val() == 0) {
                alert("Vui lòng chọn địa chỉ tỉnh/thành người gửi đơn");
                return false;
            }
            //if ($("#iDiaPhuong_0").val() != 0) {
            //    if ($("[name=iDiaPhuong_1]").val() == 0) {
            //        alert("Vui lòng chọn Chọn huyện/thành phố/thị xã");
            //        return false;
            //    }
            //}
            //if ($("#cNguoiGui_DiaChi").val() == "") {
            //    alert("Vui lòng nhập địa chỉ người nộp đơn"); $("#cNguoiGui_DiaChi").focus();
            //    return false;
            //}
            if ($("#iLoaiDon").val() == 0) {
                alert("Vui lòng chọn loại đơn vụ việc"); $("#iLoaiDon").focus();
                return false;
            }
            if ($("#iLinhVuc").val() == 0) {
                alert("Vui lòng chọn loại lĩnh vực"); $("#iLinhVuc").focus();
                return false;
            }
           
            if ($("#cNoiDung").val().trim().length < 1) { alert("Vui lòng nhập nội dung"); $("#cNoiDung").focus(); return false; }
            ShowPageLoading();
        }
        function Update_ngaytiep(val) {
            $.post("/Tiepdan/Ajax_UpdateNgay/" + val + "", "", function (data) {
                $("#dNgayNhan").val(data);
            });
        }

        function LoadLinhVuc() {
            if ($("#iLinhVuc").val() != 0) {
                $.post("<%=ResolveUrl("~")%>Tiepdan/Ajax_LoadLinhVucNoiDung",
                "iLinhVuc=" + $("#iLinhVuc").val(),
                function (data) {
                    $("#LoadNoiDung").html(data);
                    $("#LoadTinhChat").html("<select style='width:100%' name='iTinhChat' id='iTinhChat' class='input-medium chosen-select'><option  value='0'  > - - - Chưa xác định</option></select>");
                    $("#iTinhChat").chosen();
                    $("#iNoiDung").chosen();
                }
            );
            }
        }
        function LoadOpTinhChat() {
            if ($("#iNoiDung").val() != 0) {
                $.post("<%=ResolveUrl("~")%>Tiepdan/Ajax_LoadTinhChatNoiDung",
                     "iNoiDung=" + $("#iNoiDung").val(),
                     function (data) {
                         $("#LoadTinhChat").html(data);
                         $("#iTinhChat").chosen();
                     }
                 );
            }
        }
        function LoadLinhVucByLoaiDon(val) {
            $.post("<%=ResolveUrl("~")%>Kntc/Ajax_LoadLinhVucByLoaiDon", "iLoaiDon=" + val,
     function (data) {
         $("#ip_linhvuc").html(data);
         $("#iLinhVuc").chosen();
     });
        }
        function LoadLich(val) {

            $.post("<%=ResolveUrl("~")%>Tiepdan/Ajax_LoadLich/" + val + "",
              function (data) {
                  $("#Loadlichtiep").html(data);
                  $("#iTiepDinhKy").chosen();
                  $("#dNgayNhan").datepicker();
                  $("#tieudetheolich").html("");
                  $("#ngaytieptheolich").html("");

              }
          );

        }
  
        function LoadThongTinLich() {
           
            $("#tieudetheolich").html("Ngày tiếp <i style='color:red'>*</i>");
            $("#ngaytieptheolich").html("<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick'  />");
            $("#dNgayNhan").datepicker();

            
        }
    </script>
</asp:Content>
