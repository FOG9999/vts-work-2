<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color">
                        <div class="box-title">
                            <h3>
                                <i class="icon-reorder"></i>Chuyển xử lý 
                            </h3>
                            <ul class="tabs">
                                <li class="active">
                                    <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup_Re();" data-toggle="tab"><i class="icon-remove"></i></a>
                                </li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" onsubmit="return CapNhat()" action="/Kntc/Ajax_Chuyenxuly_noibo_insert" id="_form" enctype="multipart/form-data" class="form-horizontal">

                                <%-- <div class="control-group">
                                    <label for="textfield" class="control-label">Cơ quan nhận<span class=" f-red">*</span></label>
                                    <div class="controls">
                                        <select name="iDonVi" id="iDonVi" class="input-block-level chosen-select">
                                            <option value="0">--- Chọn cơ quan</option>
                                            <%=ViewData["opt-coquan"] %>
                                        </select>
                                    </div>
                                </div>--%>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Cơ quan nhận<span class=" f-red">*</span></label>
                                    <div class="controls">
                                        <%=ViewData["radio-thamquyen"] %>
                                        <div class="input-block-level chuyenxuly-donvi" id="TrungUong">
                                            <select name="iDonVi" id ="iDonVi" onchange="ChangeLinhVucByDonVI(this.value)" class="chosen-select">
                                                <%=ViewData["opt-donvithamquyen"] %>
                                            </select>
                                        </div>
<%--                                        <div class="input-block-level" id="DiaPhuong" style="display: none">
                                            <% if (ViewData["is_dbqh"].ToString() == "1")
                                                { %>
                                            <select name="iDonVi_DiaPhuong" onchange="ChangeLinhVucByDonVI(this.value)" class="chosen-select">
                                                <%=ViewData["opt-donvithamquyen-diaphuong"] %>
                                            </select>
                                            <% }
                                                else
                                                { %>
                                            <div>
                                                <div class="span6">
                                                    <select onchange="ChangeDiaPhuongParent(this.value)" name="iDonVi_DiaPhuong_Parent" id="iThamQuyenDonVi_DiaPhuong_Parent" class="chosen-select">
                                                        <%=ViewData["opt-donvithamquyen-diaphuong-parent"] %>
                                                    </select>
                                                </div>
                                                <div class="span6" id="diaphuong_child">
                                                    <select name="iDonVi_DiaPhuong" id="iThamQuyenDonVi_DiaPhuong" class="chosen-select">
                                                        <option value="0">Chọn đơn vị</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <% } %>
                                        </div>--%>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Số công văn</label>
                                    <div class="controls">
                                        <input type="text" class="input-medium" name="cSoVanBan" id="cSoVanBan" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label ">Ngày ban hành</label>
                                    <div class="controls">
                                        <input type="text" class="input-medium datepick" name="dNgayBanHanh" id="dNgayBanHanh" autocomplete ="off"/>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Ngày yêu cầu phản hồi <i class="f-red">*</i></label>
                                    <div class="controls">
                                        <input type="text" class="input-medium datepick" id="dNgayPhanHoi" name="dNgayPhanHoi" autocomplete ="off"/>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Người ký/Chức vụ</label>
                                    <div class="controls">
                                        <input type="text" class="span6" name="cNguoiKy" />
                                        <input type="text" class="span6" name="cChucVu" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Cơ quan chuyển đến</label>
                                    <div class="controls">
                                        <textarea class="input-block-level" style="height: 50px;" name="cCoQuanChuyenDen"></textarea>
                                        <%--<input type="text" class="input-block-level" rows="5" cols="40" style="height: 50px;"  />--%>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Nơi nhận</label>
                                    <div class="controls">
                                        <textarea class="input-block-level" style="height: 50px;" name="cNoiNhan"></textarea>
                                        <%--<input type="text" class="input-block-level" rows="5" cols="40" style="height: 50px;" name="cNoiNhan" />--%>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label for="textfield" class="control-label">
                                        File đính kèm</br>
                                        <em class="f-grey">Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</em></label>
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
                                <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                    <span onclick="HidePopup_Re();" class="btn btn-warning">Quay lại</span>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script>

    function CheckThamQuyen() {
        var check = $("input[type='radio'][name='iThamQuyen']:checked").val();
        if (check == 1) {
            $("#TrungUong").show(); $("#DiaPhuong").hide();
        } else {
            $("#TrungUong").hide(); $("#DiaPhuong").show();
        }
    }
    $(".chosen-select").chosen();
    function HidePopup_Re() {
        location.reload();
    }
    function CapNhat() {

        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        if ($("#dNgayPhanHoi").val() == "")
        {
            alert("Vui lòng điền ngày phản hồi yêu cầu");
            $("#dNgayPhanHoi").focus();
            return false;
        }
    }
    function DoiThamQuyenDonVi(val) {
        debugger;
        $.post("/Kiennghi/Ajax_Get_ThamQuyen_DonVi", '&val=' + val, function (data) {
            $("#TrungUong").show();
            $("#iDonVi").html('<select name="iDonVi" id="iDonVi" class="chosen-select">' + data + '</select>');
            $("#iDonVi").trigger("liszt:updated");
            $("#iDonVi").chosen();
        });
    }
    function ChangeDiaPhuongParent(val) {
        //alert(val);
        if (val != 0) {
            //$.ajax({
            //    url: "/Kiennghi/Ajax_Get_Coquan_diaphuong_child",
            //    type: 'POST',
            //    dataType: 'json',
            //    data: { "id": val },
            //    success: function (result) {
            //        //var param = result.param;
            //        var list_coquan = result.list_coquan;
            //        //alert(list_coquan);
            //        $("#diaphuong_child").html('<select name="iThamQuyenDonVi_DiaPhuong" id="iThamQuyenDonVi_DiaPhuong">' +
            //            Option_DonVi(list_coquan, val,0) + '</select>');
            //        //$('#iChuongTrinh').trigger('chosen:updated');
            //        $("#iThamQuyenDonVi_DiaPhuong").chosen();
            //    },
            //});

            $.post("/Kiennghi/Ajax_Get_Coquan_diaphuong_child_", 'id=' + val, function (data) {
                //alert(data);
                $("#diaphuong_child").html('<select name="iDonVi_DiaPhuong" id="iThamQuyenDonVi_DiaPhuong"><option value="0">Chọn đơn vị</option>' + data + '</select>');
                //$('#iChuongTrinh').trigger('chosen:updated');
                $("#iThamQuyenDonVi_DiaPhuong").chosen();
            });
        } else {
            $("#diaphuong_child").html('<select name="iDonVi_DiaPhuong" id="iThamQuyenDonVi_DiaPhuong"><option value="0">Chọn đơn vị</option></select>');
            //$('#iChuongTrinh').trigger('chosen:updated');
            $("#iThamQuyenDonVi_DiaPhuong").chosen();
        }
    }
</script>
