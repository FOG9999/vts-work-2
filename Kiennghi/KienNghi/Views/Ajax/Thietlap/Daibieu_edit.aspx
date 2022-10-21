<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Entities.Models" %>
<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color">
                        <div class="box-title">
                            <h3>
                                <i class="icon-reorder"></i>Cập nhật đại biểu quốc hội
                            </h3>
                            <ul class="tabs">
                                <li class="active">
                                    <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
                                </li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <% DAIBIEU d = (DAIBIEU)ViewData["daibieu"]; %>
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                <div class="action" style="height: 450px; overflow: auto;">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Loại Đại Biểu <i class="f-red">*</i></label>
                                        <div class="controls">
                                            <select class="input-block-level" name="iLoaiDaiBieu" id="iLoaiDaiBieu" onchange="ChangeLoaiDaiBieu(this.value)">
                                                <option value="0" <%= d.ILOAIDAIBIEU == 0 ? "selected" : ""%> >Đại Biểu Quốc Hội</option>
                                                <option value="1" <%= d.ILOAIDAIBIEU == 1 ? "selected" : ""%> >Đại Biểu Hội Đồng Nhân Dân</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Tên địa phương <i class="f-red">*</i></label>
                                        <div class="controls">
                                            <select class="input-block-level" name="iDiaPhuong0" id="iDiaPhuong0">
                                                <%= ViewData["listDiaPhuong"]%>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group" id="ctrlDiaPhuong1">
                                        <label for="textfield" class="control-label ">Tổ đại biểu <i class="f-red">*</i></label>
                                        <div class="controls">
                                            <select class="input-block-level" name="iDiaPhuong1" id="iDiaPhuong1">
                                                <option value="0">- - -  Chọn Tổ đại biểu</option>
                                                <%=ViewData["opt-huyen"] %>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Đơn vị bầu cử số</label>
                                        <div class="controls">
                                            <input type="text" <%= d.ILOAIDAIBIEU == 0 ? "disabled" : ""%> value="<%=d.CDONVIBAUCUSO %>" name="cdonvibaucuso" id="cdonvibaucuso" class="input-medium" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Mã đại biểu</label>
                                        <div class="controls">
                                            <input type="text" value="<%=d.CCODE %>" name="cCode" id="cCode" class="input-medium" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">
                                            Tên đại biểu <i class="f-red">*</i>
                                        </label>
                                        <div class="controls">
                                            <input type="text" value="<%=Server.HtmlEncode(d.CTEN) %>" name="cTen" id="cTen" class="input-block-level" onchange="kiemtrungten()" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Tổ trưởng</label>
                                        <div class="controls">
                                            <input type="checkbox" name="iToTruong" <% if (d.ITOTRUONG == 1) { Response.Write("checked"); } %>/>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Ngày sinh <i class="f-red">*</i></label>
                                        <div class="controls">
                                            <input type="text" value="<%=ViewData["Ngaysinh"] %>" name="dNgaySinh" id="dNgaySinh" class="input-medium datepick" />

                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Giới tính <i class="f-red">*</i></label>

                                        <div class="controls">
                                            <input type="radio" name="iGioiTinh" value="0" <%if (d.IGIOITINH == 0) { Response.Write("checked"); } %>>
                                            Nam 
                                            <br />
                                            <input type="radio" name="iGioiTinh" value="1" <%if (d.IGIOITINH == 1) { Response.Write("checked"); } %>>
                                            Nữ
                                        </div>

                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label f-">Email <i class="f-red">*</i></label>
                                        <div class="controls">
                                            <input type="text" value="<%=Server.HtmlEncode(d.CEMAIL) %>" name="cEmail" id="cEmail" class="input-block-level" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Số điện thoại <i class="f-red">*</i></label>
                                        <div class="controls">
                                            <input type="text" value="<%=Server.HtmlEncode(d.CSDT) %>" name="cSDT" id="cSDT" class="input-block-level" onchange="CheckNum('cSDT')" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Trưởng đoàn</label>
                                        <div class="controls">
                                            <input type="checkbox" <% if (d.ITRUONGDOAN == 1) { Response.Write("checked"); } %> name="iTruongDoan" />
                                        </div>
                                    </div>
                                    <div class="control-group" style="display: none">
                                        <label for="textfield" class="control-label">Đoàn đại biểu  </label>
                                        <div class="controls">
                                            <input type="text" value="<%=d.CDOANDB %>" name="cDoanDB" id="cDoanDB" class="input-block-level" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Nơi làm việc  </label>
                                        <div class="controls">
                                            <input type="text" value="<%=d.CNOILAMVIEC %>" name="cNoiLamViec" id="cNoiLamViec" class="input-block-level" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Cơ quan  </label>
                                        <div class="controls">
                                            <input type="text" value="<%=d.CCOQUAN %>" name="cCoQuan" id="cCoQuan" class="input-block-level" onchange="CheckNum('cSDT')" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Chức vụ vụ đầy đủ</label>
                                        <div class="controls">
                                            <textarea rows="2" class="input-block-level" id="cChucVuDayDu" name="cChucVuDayDu"><%=d.CCHUCVUDAYDU %></textarea>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Khóa</label>
                                        <div class="controls">
                                            <div class="actions" id="action" style="height: 150px; overflow: auto;">
                                                <%= ViewData["List"] %>
                                            </div>
                                        </div>
                                    </div>
                                    <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                </div>
                                <div class="form-actions nomagin">

                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
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
    $("#iLoaiDaiBieu").chosen();
    $("#ctrlDiaPhuong1").hide();
    $("#iDiaPhuong1").chosen();
    $("#iDiaPhuong0").chosen();
    
    var listAllKhoa = JSON.parse(`<%= ViewData["opt-khoa"]%>`);
    var listAllKyHop = JSON.parse(`<%= ViewData["opt-kyhop"]%>`);
    var listDaiBieuKyHop = JSON.parse(`<%= ViewData["listDaiBieuKyHop"]%>`);
    <%--var listTinh = JSON.parse(`<%= ViewData["listDiaPhuong"]%>`);
    if (listTinh.length > 0) {
        optTinhHtml = `<option value="0">- - -  Chọn địa phương</option>`;
        listTinh.forEach(function (tinh) {
            if (tinh.IDIAPHUONG == <%=d.IDIAPHUONG %>) {
                optTinhHtml += ("<option " + " value='" + tinh.IDIAPHUONG + "' selected>" + tinh.CTEN + "</option>");
            }
            else {
                optTinhHtml += ("<option " + " value='" + tinh.IDIAPHUONG + "'>" + tinh.CTEN + "</option>");
            }
        });
        $("#iDiaPhuong0").html(optTinhHtml);
        $("#iDiaPhuong0").chosen();
    }--%>

    function ChangeLoaiDaiBieu(data) {
        UpdateKhoaKyHopByLoai(data);
        if (data === '1') {
            $('#cdonvibaucuso').removeAttr('disabled');
            $("#ctrlDiaPhuong1").show();
        }
        else {
            $('#cdonvibaucuso').attr('disabled', 'disabled');
            $("#ctrlDiaPhuong1").hide();
        }
    }
    updateHuyen();
    function updateHuyen() {
        if ($("#iLoaiDaiBieu").val() == 1)
            $("#ctrlDiaPhuong1").show();
        else
            $("#ctrlDiaPhuong1").hide();
    }
    UpdateKhoaKyHopByLoai($("#iLoaiDaiBieu").val());
    function UpdateKhoaKyHopByLoai(iLoai) {
        var OptListKhoaKyHopHtml = "";
        var listKhoa = listAllKhoa.filter(function (khoa) {
            return khoa.ILOAI == iLoai;
        });
        if (listKhoa && listKhoa.length > 0) {
            //OptListKhoaKyHopHtml += "<ul class='list-chucnang'>"
            listKhoa.forEach(function (khoa) {
                debugger;
                var check = "";
                if (listDaiBieuKyHop.indexOf(khoa.IKHOA) > -1) {
                    console.log(khoa);
                    check = "checked";
                }
                OptListKhoaKyHopHtml += "<ul class='list-chucnang'>"
                var dBatDau = khoa.DBATDAU;
                dBatDauStr = new Date(dBatDau).getFullYear().toString();
                var dKetThuc = khoa.DKETTHUC;
                dKetThucStr = new Date(dKetThuc).getFullYear().toString();
                OptListKhoaKyHopHtml += "<li><input type='checkbox' " + check + " name='action' value='" + khoa.IKHOA + "' id='action" + khoa.IKHOA + "' /> <a href=\"javascript:void()\" data-original-title='Chọn khoá họp' onclick=\"Checkboxchucnang(" + khoa.IKHOA + ")\" rel='tooltip' title=''  style='color:black'>" + khoa.CTEN + " (" + dBatDauStr + "-" + dKetThucStr + ")" + "</a ></li > ";
                OptListKhoaKyHopHtml += "</ul>"
                //OptListKhoaKyHopHtml += "<p>" + khoa.CTEN + "</p>"
                //var listKyHopByKhoa = listAllKyHop.filter(function (kyhop) {
                //    return kyhop.IKHOA == khoa.IKHOA;
                //});
                //if (listKyHopByKhoa && listKyHopByKhoa.length > 0) {
                //    OptListKhoaKyHopHtml += "<ul class='list-chucnang'>"
                //    listKyHopByKhoa.forEach(function (kyhop) {
                //        var check = "";
                //        if (listDaiBieuKyHop.indexOf(kyhop.IKYHOP) > -1) {
                //            console.log(kyhop);
                //            check = "checked";
                //        }
                //        OptListKhoaKyHopHtml += "<li><input type='checkbox' " + check + " name='action' value='" + kyhop.IKYHOP + "' id='action" + kyhop.IKYHOP + "' /> <a href=\"javascript:void()\" data-original-title='Chọn kỳ họp' onclick=\"Checkboxchucnang(" + kyhop.IKYHOP + ")\" rel='tooltip' title=''  style='color:black'>" + kyhop.CTEN + "</a></li>";
                //    })
                //    OptListKhoaKyHopHtml += "</ul>"
                //}
            });
        }
        $('#action').html(OptListKhoaKyHopHtml);
    }
    // Khong chon duoc nhieu Khoa 1 luc
    $('input[type="checkbox"]').on('change', function () {
        $('input[name="' + this.name + '"]').not(this).prop('checked', false);
    });

    function CapNhat() {
        debugger;
        if ($("#iDiaPhuong0").val() == 0) {
            alert("Vui lòng chọn địa phương!"); $("#iDiaPhuong0").focus(); return false;
        }

        if ($("#iLoaiDaiBieu").val() == 1 && $("#iDiaPhuong1").val() == 0) {
            alert("Vui lòng chọn tổ đại biểu!");
            $("#iDiaPhuong1").focus();
            return false;
        }
            

        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên !"); $("#cTen").focus(); return false;
        }
        if ($("#dNgaySinh").val() == "") {
            alert("Vui lòng chon ngày sinh !"); $("#dNgaySinh").focus(); return false;
        }

        if ($("#cEmail").val() == "") {
            alert("Vui lòng nhập email!"); $("#cEmail").focus(); return false;
        } else {
            if (!emailRegExp.test($("#cEmail").val())) {
                alert("Email không hợp lệ!"); $("#cEmail").focus(); return false;
            }
        }
        if ($("#cSDT").val() == "") {
            alert("Vui lòng nhập số điện thoại"); $("#cSDT").focus(); return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Daibieu_update", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Cập nhật thành công")
            } else {
                alert("Tên đại điểu đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;

    }

</script>

