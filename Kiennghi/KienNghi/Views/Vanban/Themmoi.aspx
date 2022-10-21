<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Thêm mới văn bản
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Vanban") %>
    <div id="main" class="">
        <a href="#" class="show_menu_trai">Menu trái</a>
        <div class="container-fluid">
            <div class="breadcrumbs">
                <ul>
                    <li>
                        <a href="<%=ResolveUrl("~") %>">Trang chủ</a>
                        <i class="icon-angle-right"></i>
                    </li>
                        <li>
				    <span> Văn bản công bố   <i class="icon-angle-right"></i>  </span>
			    </li>
                    <li>
                        <span>Thêm mới văn bản</span>
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
                            <h3><i class="icon-tags"></i>Thêm mới văn bản</h3>

                        </div>
                        <div class="box-content" style="text-align: left;">

                            <form method="post" name="_form" id="_form" onsubmit="return CheckForm();" enctype="multipart/form-data" class="form-horizontal form-column">
                                
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Số văn bản <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <input type="text" name="cTieude" id="cTieude" class="input-block-level" autofocus />

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Ngày ban hành <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <input type="text" name="dDate" id="dDate" class="input-block-level datepick" autocomplete ="off"/>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Trích yếu <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea name="cTrichyeu" id="cTrichyeu" class="input-block-level"> </textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                   <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Đơn vị ban hành <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iDonvi" id="iDonvi" class="input-block-level chosen-select" onchange="ChangeLinhVucByDonVI(this.value)">
                                                        <option value="0">- - -  Chọn đơn vị ban hành</option>
                                                        <option value="-1" class="b"><span class="b">Nhiều đơn vị ban hành</span></option>
                                                        <%=ViewData["opt_donvi"] %>
                                                    </select>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Lĩnh vực <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level" id="div_linhvuc">
                                                    <select id="iLinhvuc" name="iLinhvuc" class="input-block-level chosen-select">
                                                        <option value="0">- - -  Chọn lĩnh vực</option>
                                                        <%=ViewData["opt-linhvuc"] %>
                                                    </select>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Loại văn bản <i class="f-red">*</i> </label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select id="iLoai" name="iLoai" class="input-block-level chosen-select">
                                                        <option value="0">- - -  Chọn loại văn bản</option>
                                                        <%=ViewData["opt-loai"] %>
                                                    </select>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Kỳ họp <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iKyhop" id="iKyhop" class="input-block-level chosen-select">
                                                        <option value="0">- - -  Chọn kỳ họp</option>
                                                        <%=ViewData["opt-kyhop"] %>
                                                    </select>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" id="hienthi" style="margin-top: 1%;display: none"  >
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  "></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <div class="actions" id="action" style="height: 300px; overflow: auto">
                                                        <%=ViewData["list_group"] %>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" >
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">
                                            File đính kèm
                                       
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
                                                <input class="input-xlarge" disabled id="file_name<%=i %>" type="text">
                                                <span class="btn btn-danger" onclick="$('#file_upload<%=i %>,#file_name<%=i %>').val('');" title="Hủy"><i class="icon-trash"></i></span>
                                            </div>
                                            <% } %>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" style="border-bottom: 1px solid #fff">
                                    <div class="form-actions">
                                        <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
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
    <script>
        function ChangeLinhVucByDonVI(val) {
            KiemTra(val);
            if (val != -1)
            {
                $.post("/Vanban/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
                    //alert(data);
                    $("#div_linhvuc").html('<select name="iLinhVuc" id="iLinhVuc" class="chosen-select">' + data + '</select>');
                    //$('#iChuongTrinh').trigger('chosen:updated');
                    $("#iLinhVuc").chosen();
                });
            }
        }
       
        function CheckForm() {
            if ($("#cTieude").val() == "") { alert("Vui lòng nhập số văn bản"); $("#cTieude").focus(); return false; }
            if ($("#dDate").val() == "") { alert("Vui lòng nhập ngày ban hành"); $("#dDate").focus(); return false; }
            if ($("#cTrichyeu").val().trim().length < 1) { alert("Vui lòng nhập trích yếu"); $("#cTrichyeu").focus(); return false; }
            if ($("#iDonvi").val() == 0) { alert("Vui lòng chọn đơn vị ban hành"); $("#iDonvi").focus(); return false; }
            if ($("#iLinhVuc").val() == 0) { alert("Vui lòng chọn lĩnh vực"); $("#iLinhvuc").focus(); return false; }
            if ($("#iLoai").val() == 0) { alert("Vui lòng chọn loại văn bản"); $("#iLoai").focus(); return false; }
            ShowPageLoading();
        }
        function KiemTra(val) {
            if (val == -1) {
                document.getElementById("hienthi").style.display = "block";
            }
            else {
                document.getElementById("hienthi").style.display = "none";
            }
        }
    </script>
</asp:Content>
