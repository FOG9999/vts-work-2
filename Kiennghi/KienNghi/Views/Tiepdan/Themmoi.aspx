<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Thêm mới tiếp dân
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Tiepdan") %>
    <div id="main" class="">
        <a href="#" class="show_menu_trai">Menu trái</a>
        <div class="container-fluid">
            <div class="breadcrumbs">
                <ul>
                    <li>
                        <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i>Trang chủ</a>
                        <i class="icon-angle-right"></i>
                    </li>
                        <li>
                        <a href="#"> Tiếp công dân </a>
                        <i class="icon-angle-right"></i>
                    </li>
                    <li>
                        <a href="#">Thêm mới vụ việc tiếp dân</a>
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
                            <h3><i class="icon-tags"></i>Thêm mới vụ việc tiếp dân</h3>
                        </div>
                        <div class="box-content" style="text-align: left;">                     
                        <form  class="form-horizontal form-column" id="form_" name="form_"  onsubmit="return CheckForm()" enctype="multipart/form-data" method="post">  
					        
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Loại hình tiếp <i class="f-red">*</i></label>
                                        <div class="controls">
                                              <select name="iLoaiTiep" id="iLoaiTiep" onchange="LoadLich(this.value)" class="input-block-level chosen-select"  >
                                                  <option value="-1" >- - - Chưa xác định  </option>
                                                    <option value="1" <%=  ViewData["lichtiepthuongxuyen"] %>>- - - Tiếp thường xuyên  </option>
                                                    <option value="0" <%=  ViewData["lichtiepdinhky"] %>>- - - Tiếp định kỳ </option>
                                                   <option value="2" <%=  ViewData["lichtiepdotxuat"] %>>- - - Tiếp đột xuất  </option>
                                                    <option></option>
                                                </select>

                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Lịch tiếp <i class="f-red">*</i></label>
                                        <div class="controls">
                                              <select class="" style="display: none" name="iNguonDon"><%=ViewData["opt-nguondon"] %></select>
                                            <div class="input-block-level" id="Loadlichtiep">
                                  
                                               <%=ViewData["opt_tiepdinhky"]  %>
                                              

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
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Tên công dân đến <i class="f-red">*</i></label>
                                        <div class="controls">
                                              <input type="text" name="cNguoiGui_Ten" id="cNguoiGui_Ten" value="<%= ViewData["tencongdanden"] %>" class="input-block-level" autofocus  />
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Người tiếp <i class="f-red">*</i></label>
                                        <div class="controls">
                                              <input type="text" name="cNguoiTiep_Ten" value="<%=ViewData["nguoitiep"] %>" id="cNguoiTiep_Ten" class="input-block-level" />
                                        </div>           
                                    </div>
                                </div>
                            </div><!-- end row-fluid-->
                            <div class="row-fluid">
                                <div class="control-group">
                                    <div class="span12">
                                            <div class="controls">
                                                <label class="checkbox">
                                                     <input type="checkbox" name="iDoanDongNguoi" id="iDoanDongNguoi"  onchange="$('#doan').toggle();" /> Đoàn đông người
                                                      <span id="doan" style="margin-left: 15px; display: none">
                                            <input type="text" class="input-medium" name="iSoNguoi" id="iSoNguoi" onchange="CheckNum('iSoNguoi')" value="0" placeholder="Số người" /></span>
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
                                                <p class="span6">
                                                    <select name="iDiaPhuong_0" onchange="ChangeTinhThanh('iDiaPhuong_1',this.value)" id="iDiaPhuong_0" class="chosen-select">
                                                        <option value="0">Chọn tỉnh thành</option>
                                                        <%=ViewData["opt-tinh"] %>
                                                        <option value="-1">Khác</option>
                                                    </select>
                                                </p>
                                                <p class="span6" id="iDiaPhuong_1">
                                                    <select name="iDiaPhuong_1" id="iDiaPhuong_01" class="chosen-select ">
                                                        <option value="0">Chọn huyện/thành phố/thị xã</option>
                                                        <%=ViewData["opt-huyen"] %>
                                                    </select>
                                                </p>
                                                <p class="clear">
                                                    <input autocomplete="off" type="text" class="span6" name="cNguoiGui_DiaChi" id="cNguoiGui_DiaChi" placeholder="Số nhà, đường..." value="<%= ViewData["sonha"] %>" />
                                                    <a onclick="KiemTrung()" class="btn btn-success" />  Kiểm trùng </a>
                                                    <span id="thongbaokiemtrung"></span>
                                                </p>

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
                                                  <select name="iNguoiGui_QuocTich" class="input-medium chosen-select"><%=ViewData["opt-quoctich"] %></select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Dân tộc</label>
                                        <div class="controls">
                                            <div class="input-block-level">
                                                 <select name="iNguoiGui_DanToc" class="input-medium chosen-select">
                                                <%=ViewData["opt-dantoc"] %> <i class="f-red">*</i>
                                            </select> 

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div><!-- end row-->
                            <div class="row-fluid" >
                                <div class="span12">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Tóm tắt nội dung vụ việc <i class="f-red">*</i></label>
                                    <div class="controls">
                                         <div class="input-block-level">
                                       <textarea class="input-block-level" name="cNoiDung" id="cNoiDung"><%= ViewData["Thongtinnoidung"] %></textarea>
                                             </div>
                                    </div>
                                </div>
                                </div>
                            </div>
                            <div class="row-fluid" >
                                <div class="span12">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Nội dung chỉ đạo</label>
                                    <div class="controls">
                                         <div class="input-block-level">
                                       <textarea class="input-block-level" name="cNoiDungChiDao" id="cNoiDungChiDao"></textarea>
                                             </div>
                                    </div>
                                </div>
                                </div>
                            </div>
                            <div class="row-fluid" >
                                 <div class="span10">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">File đính kèm
                                        <small>Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</small></label>
                                    <div class="controls">
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
                                                <input class="input-xlarge" disabled id="file_name<%=i %>" type="text" >
                                                <span class="btn btn-danger" onclick="$('#file_upload<%=i %>,#file_name<%=i %>').val('');" title="Hủy"><i class="icon-trash"></i></span>
                                            </div>
                                            <% } %>
                                    </div>
                                     
                                </div>
                                     </div>
                                <div class="span2">
                                        <div class="control-group" style="float: right">
                                            <a style="display: block" id="open" onclick="Open()" href="javascript:void(0)">Mở rộng <i id="icon_morong" class="icon-arrow-down"></i></a>
                                            <input  type="hidden" value="<%= ViewData["morong"] %>" id="giatrikiemtra" name="giatrikiemtra"/>
                                            <a style="display: none" id="close" onclick="Close()" href="javascript:void(0)">Mở rộng <i id="icon_thuhep" class=" icon-arrow-up"></i></a>
                                        </div>
                                    </div>
                            </div><!-- end row-->
                            <div id="phanloai" style= <%=ViewData["style"] %>>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Loại đơn <i class="f-red">*</i></label>
                                    <div class="controls">
                                       <select name="iLoaiDon" id="iLoaiDon" class="input-medium chosen-select" onchange="LoadLinhVucByLoaiDon(this.value)" >
                                                <option value="0">- - - Chưa xác định</option>
                                                <%=ViewData["opt-loaidon"] %>
                                            </select>
                                    </div>
                                </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Lĩnh vực <i class="f-red">*</i></label>
                                    <div class="controls" id="ip_linhvuc" >
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
                                    <div class="controls" id="LoadNoiDung" >
                                       <select name="iNoiDung" id="iNoiDung" class="input-medium chosen-select" onclick="LoadOpTinhChat()">
                                                <option value="0">- - - Chưa xác định</option>
                                           <%= ViewData["opt-noidung"] %>
                                           </select>
                                    </div>
                                </div>
                                </div>
                                 <div class="span6">
                                     <div class="control-group">
                                    <label for="textfield" class="control-label">Tính chất vụ việc</label>
                                    <div class="controls" id="LoadTinhChat">
                                         <select name="iTinhChat" class="input-medium chosen-select">
                                                <option value="0">- - - Chưa xác định</option>
                                             <%=  ViewData["opt-tinhchat"] %>
                                            </select> 
                                    </div>
                                </div>
                                 </div>
                            </div>
                            </div>
                            <!-- end row-->
                            <input type="hidden"  id="vuvieckiemtra" name="vuvieckiemtra" value="<%=ViewData["VuViecID"] %>"/>
                            <div class="row-fluid" style="border-bottom:1px solid #fff">
                                <div class="form-actions" >
                                    <input type="submit" value="Lưu vụ việc & kiểm trùng" class="btn btn-success" />
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
        $("#iTiepDinhKy").chosen();
        function lammoi() { location.reload() }
        function CheckForm() {
           
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
            if ($("#iDiaPhuong_0").val() != 0) {
                if ($("[name=iDiaPhuong_1]").val() == 0) {
                    alert("Vui lòng chọn Chọn huyện/thành phố/thị xã");
                    return false;
                }
            }
            if ($("#cNguoiGui_DiaChi").val() == "") {
                alert("Vui lòng nhập địa chỉ người nộp đơn"); $("#cNguoiGui_DiaChi").focus();
                return false;
            }
            if ($("#cNoiDung").val().trim().length < 1) { alert("Vui lòng nhập nội dung"); $("#cNoiDung").focus(); return false; }
            if ($("#giatrikiemtra").val() == 0)
            {
                alert("Vui lòng nhấp vào phần mở rộng để nhập đầy đủ thông tin"); $("#iLinhVuc").focus();
                return false;
            }
            if ($("#iLoaiDon").val() == 0) {
                alert("Vui lòng chọn loại đơn vụ việc"); $("#iLoaiDon").focus();
                return false;
            }
            if ($("#iLinhVuc").val() == 0) {
                alert("Vui lòng chọn loại lĩnh vực"); $("#iLinhVuc").focus();
                return false;
            }
           
           
            ShowPageLoading();
        }
        //function Submit_Confirm() { //Xóa nhanh
        //    if ($("#iLoaiTiep").val() == -1) {

        //        alert("Vui lòng chọn loại hình tiếp");
        //        return false;

        //    }
        //    if ($("#iLoaiTiep").val() == 0) {
        //        if ($("#iTiepDinhKy").val() == 0) {
        //            alert("Vui lòng chọn lịch tiếp");
        //            return false;
        //        }
        //    }
        //    if ($("#iLoaiTiep").val() != 0) {
        //        if ($("#dNgayNhan").val() == "") {
        //            alert("Vui lòng chọn ngày tiếp"); $("#dNgayNhan").focus();
        //            return false;
        //        }
        //    }
        //    if ($("#cNguoiGui_Ten").val() == "") {
        //        alert("Vui lòng nhập tên công dân đến"); $("#cNguoiGui_Ten").focus();
        //        return false;
        //    }

        //    if ($('#iDoanDongNguoi:checkbox:checked').length > 0) {
        //        if ($("#iSoNguoi").val() <= 1) {
        //            alert("vui lòng nhập số người lớn hơn"); $("#iSoNguoi").focus();
        //            return false;
        //        }
        //    }
        //    if ($("#iDiaPhuong_0").val() == 0) {
        //        alert("Vui lòng chọn địa chỉ tỉnh/thành người gửi đơn");
        //        return false;
        //    }
        //    if ($("#iDiaPhuong_0").val() != 0) {
        //        if ($("[name=iDiaPhuong_1]").val() == 0) {
        //            alert("Vui lòng chọn Chọn huyện/thành phố/thị xã");
        //            return false;
        //        }
        //    }
        //    if ($("#cNguoiGui_DiaChi").val() == "") {
        //        alert("Vui lòng nhập địa chỉ người nộp đơn"); $("#cNguoiGui_DiaChi").focus();
        //        return false;
        //    }
        //    var alert_confirm = "";
        //    if (alert_confirm == "") { alert_confirm = "Bạn có muốn lưu hay không?"; }
        //    $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
        //                       '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
        //                        '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Xác nhận lưu vụ việc</h3>' +
        //                        ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
        //                        '</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
        //                         '<p>' + alert_confirm + '</p>' +
        //                         '<div class="form-actions nomagin tright"><button type="button" onclick="KiemTrung()" class="btn btn-primary">Đồng ý</button>' +
        //                         ' <button type="submit" class="btn btn-warning btn-focus" id="btn-submit">Hủy bỏ</button></div></form></div></div></div></div></div>' +
        //                          ' </div></div>');
        //    $("#btn-submit").focus();
        //    //ShowPopUp(post + "&url=" + url + "&str_confirm=" + alert_confirm, "/Home/Ajax_Confirm_delete");

        //    //if (confirm(alert_confirm)) {
        //    //    $.post(sitename + url, post, function (data) {
        //    //        if (data == 1) {
        //    //            location.reload();
        //    //        } else {
        //    //            alert(data);
        //    //        }
        //    //    });
        //    //}
        //}
        function KiemTrung()
        {
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
            $.post("/Tiepdan/Ajax_Kiemtrung", $("#form_").serialize(), function (ok) {
                // alert(ok);
                if (ok != 1)
                {
                    window.location = "/Tiepdan/Kiemtrung/#success";
                    AlertAction("Cập nhật thành công")
                }
                else
                {
                    $("#thongbaokiemtrung").html("<i style='color:red'>Không có vụ việc trùng</i>");
                }

            });
            return false;
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
        function LoadThongTinLich() {
           
            $("#tieudetheolich").html("Ngày tiếp <i style='color:red'>*</i>");
            $("#ngaytieptheolich").html("<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick'  />");
            $("#dNgayNhan").datepicker();
        }
        function LoadLich(val) {
           
                $.post("<%=ResolveUrl("~")%>Tiepdan/Ajax_LoadLich/"+val+"",
                 function (data) {
                     $("#Loadlichtiep").html(data);
                     $("#iTiepDinhKy").chosen();
                     $("#dNgayNhan").datepicker();
                     $("#tieudetheolich").html("");
                     $("#ngaytieptheolich").html("");
                 }
             );
           
        }
      
    </script>
     <script>
         function Open() {
             document.getElementById('phanloai').style = "display: block";
             document.getElementById('close').style = "display: block";
             document.getElementById('open').style = "display: none";
             $("#giatrikiemtra").val(1);
         }
         function Close() {
             document.getElementById('phanloai').style = "display: none";
             document.getElementById('close').style = "display: none";
             document.getElementById('open').style = "display: block";
             $("#giatrikiemtra").val(0);
         }
                                    </script>
</asp:Content>
