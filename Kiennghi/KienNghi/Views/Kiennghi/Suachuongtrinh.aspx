<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Cập nhật chương trình tiếp xúc cử tri
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Knct") %>
    <div id="main">
        <div class="container-fluid">
            <div class="breadcrumbs">
                <ul>
                    <li>
                        <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i>Trang chủ</a>
                        <i class="icon-angle-right"></i>
                    </li>
                    <li>
                        <span>Kiến nghị cử tri <i class="icon-angle-right"></i></span>
                    </li>
                    <li>
                        <a href="#">Cập nhật kế hoạch tiếp xúc cử tri</a>

                    </li>
                </ul>
                <div class="close-bread">
                    <a href="#"><i class="icon-remove"></i></a>
                </div>
            </div>
            <% KN_CHUONGTRINH kn = (KN_CHUONGTRINH)ViewData["kn"];
                ChuongtrinhCuTri ct = (ChuongtrinhCuTri)ViewData["detail"]; %>
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Cập nhật kế hoạch tiếp xúc cử tri</h3>
                        </div>
                        <div class="box-content">
                            <form method="post" name="_form" id="_form" onsubmit="return CheckForm();" enctype="multipart/form-data" class="form-horizontal form-column">
                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label">Đối tượng <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <select class="chosen-select" name="iDoiTuong" id="iDoiTuong" onchange="ChangeDoiTuong()">
                                                <%=ViewData["opt-doituong"]%>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label">Khóa <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <select class="chosen-select" name="iKhoaHop" id="iKhoaHop">
                                                <option value='0'>Chọn khóa họp</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label">Kì họp <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <select class="chosen-select" name="iKyHop" id="iKyHop">
                                                <option value='0'>Chọn kỳ họp</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group span6">
                                        <label class="control-label">Hình thức <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <%=ViewData["check-hinhthuc"] %>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label">Ngày bắt đầu/ Kết thúc</label>
                                        <div class="controls">
                                            <input type="text" class="span6 datepick" value="<%=ct.batdau %>" name="dBatDau" id="dBatDau" />
                                            <input type="text" class="span6 datepick" value="<%=ct.ketthuc %>" name="dKetThuc" id="dKetThuc" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label">Kế hoạch số <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <input type="text" value="<%=Server.HtmlEncode(kn.CKEHOACH) %>" class="input-block-level" id="cKeHoach" name="cKeHoach" />
                                        </div>
                                    </div>
                                    <div class="control-group span6">
                                        <label class="control-label">Đơn vị lập <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <select class="chosen-select" name="iDonVi" id="iDonVi" onchange="ChangeDoanLapKeHoach()">
                                                <%=ViewData["opt-donvi"] %>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label">Nội dung tiếp xúc</label>
                                        <div class="controls">
                                            <textarea name="cNoiDung" class="input-block-level"><%=Server.HtmlEncode(kn.CNOIDUNG) %> </textarea>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label">Địa chỉ tiếp xúc</label>
                                        <div class="controls">
                                            <input type="text" value="<%=Server.HtmlEncode(kn.CDIACHI) %>" class="input-block-level" name="cDiaChiTiepXuc" />
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label">Ngày ban hành</label>
                                        <div class="controls">
                                            <input type="text" class="span6 datepick" value="<%=ct.ngaybanhanh %>" name="dNgaybanhanh" id="dNgaybanhanh" autocomplete ="off"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" id="lichtiep">
                                    <div class="control-group">
                                        <label class="control-label">Lịch tiếp của tổ đại biểu</label>
                                        <div class="controls nopadding">
                                            <table class="table table-bordered table-condensed" style="border-right: 1px solid #ddd; border-top: 1px solid #ddd;">
                                                <tr class="theader">
                                                    <th width="20%" class="tcenter">Tổ Đại biểu</th>
                                                    <th width='15%' class='tcenter'>Địa phương (Quận/Huyện)</th>
                                                    <th width='15%' class='tcenter'>Địa phương (Xã/Phường/Thị trấn)</th>
                                                    <th width="15%" class="tcenter">Ngày tiếp</th>
                                                    <th class="tcenter">Địa chỉ</th>
                                                    <th width="5%"></th>
                                                </tr>
                                                <tr class="tfooter">
                                                    <td colspan="4" id="lastrow"></td>
                                                    <td colspan="5" class="tright"><span title='' onclick="PlusCP(currentNum)" id="bt-add" rel='tooltip' data-original-title='Thêm mới lịch' class="btn btn-primary"><i class="icon-plus-sign"></i></span></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label">
                                            File kèm theo</br>
                                            <em class="f-grey">Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</em>
                                        </label>
                                        <div class="controls">
                                            <%=ViewData["file"] %>
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
                                    <div class="control-group">
                                        <label class="control-label"></label>
                                        <div class="controls">
                                            <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                            <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
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
        console.log(1);
        var listKhoaHopObject = JSON.parse(`<%=ViewData["listKhoaHop"]%>`);
        var listKyHopObject = JSON.parse(`<%=ViewData["listKyHop"]%>`);
        var iKyHopHienTai = <%=ViewData["iKyHopHienTai"]%>;
        var currentNum = <%=ViewData["num"]%>;
        UpdateKhoaHopByLoai(<%=kn.IDOITUONG%>);
        ShowOptKiHop(<%=kn.IKHOA%>);
        function UpdateKhoaHopByLoai(iLoai) {
            if (listKhoaHopObject.length > 0) {
                var listKhoaHopObjectByDoiTuong = listKhoaHopObject.filter(function (khoa) {
                    return khoa.ILOAI == iLoai;
                });

                if (listKhoaHopObjectByDoiTuong && listKhoaHopObjectByDoiTuong.length > 0) {
                    var optKhoaHopHtml = `<option value='0'>Chọn khóa họp</option>`;
                    listKhoaHopObjectByDoiTuong.forEach(function (khoaHop) {
                        var selected = "";
                        if (khoaHop.IKHOA == <%=kn.IKHOA%>) {
                            selected = "selected";
                        }
                        optKhoaHopHtml += ("<option " + selected + " value='" + khoaHop.IKHOA + "'>" + khoaHop.CTEN + "</option>")
                    });
                    $("#iKhoaHop").html(optKhoaHopHtml);
                    $("#iKhoaHop").trigger("liszt:updated")
                }
            }
        }

        $("#iDoiTuong").on('change', function (data) {
            UpdateKhoaHopByLoai(data.currentTarget.value)
        });

        $("#iKhoaHop").on('change', function (data) {
            ShowOptKiHop(data.currentTarget.value)
        });

        var optListDaiBieuHtml = "";
        var optListHuyenHtml = "";
        LoadLichTiepDaiBieu(<%=kn.IDONVI%>, <%=kn.IDOITUONG%>);

        function ShowOptKiHop(idKhoaHop) {
            console.log("ShowOptKiHop");
            var listKyHop = [];
            if (idKhoaHop != 0) {
                listKyHop = listKyHopObject.filter(x => x.IKHOA == idKhoaHop);
            }
            else {
                var objectKyHop = listKyHopObject.find(x => x.IKYHOP == <%=kn.IKYHOP%>);
                $("#iKhoaHop").val(objectKyHop.IKHOA);
                ShowOptKiHop(objectKyHop.IKHOA);
            }
            if (listKyHop.length > 0) {
                listKyHop = listKyHop.sort((a, b) => (a.CTEN > b.CTEN) ? 1 : ((b.CTEN > a.CTEN) ? -1 : 0));
                var optKyHopHtml = '<option value="0">Chọn kỳ họp</option>';
                listKyHop.forEach(function (kyhop) {
                    if (kyhop.IKYHOP == <%=kn.IKYHOP%>) {
                        optKyHopHtml += ("<option " + " value='" + kyhop.IKYHOP + "' selected>" + kyhop.CTEN + "</option>");
                    }
                    else {
                        optKyHopHtml += ("<option " + " value='" + kyhop.IKYHOP + "'>" + kyhop.CTEN + "</option>");
                    }
                });
                $("#iKyHop").html(optKyHopHtml);
                $("#iKyHop").trigger("liszt:updated");
            }
        }

        ShowTimKiem_Conf("id=<%=kn.ICHUONGTRINH %>" + "&iDoiTuong=<%=kn.IDOITUONG %>", '/Kiennghi/Ajax_Load_lichtiep_edit/', 'lichtiep');
        function PlusCP(num) {

            if ($("#iDonVi").val() > 0) {
                var num_plus = num + 1;
                var data = '<tr id="db_' + num_plus + '" class="db">' +
                    '     <td>' +
                    '        <select class="input-block-level"  name="iDaiBieu"><option value="0">Chọn tổ đại biểu</option>' + optListDaiBieuHtml + '</select>' +
                    '     </td><td>' +
                    '         <select class="input-block-level" onchange="ChangeHuyenXa(this.value,' + num_plus + ')" name="iDiaPhuong"><option value="0">Chọn quận/huyện</option>' + optListHuyenHtml + '</select>' +
                    '    </td><td>' +
                    '         <select class="input-block-level" id ="iDiaPhuong_02_' + num_plus + '"name="iDiaPhuong_02"><option value="0">Chọn xã/phường/thị trấn</option></select>' +
                    '    </td><td>' +
                    '       <input type="text" class="datepick input-block-level" name="dNgayTiep" />' +
                    '    </td><td>' +
                    '        <input type="text" class="input-block-level" name="cDiaChi" />' +
                    '    </td><td>' +
                    '        <span title="Xóa" onclick="$(\'#db_' + num_plus + '\').remove()" class="btn btn-danger"><i class="icon-remove"></i></span>' +
                    '    </td></tr>';
                $("table tr.tfooter").before(data);
                $("#bt-add").attr("onclick", "PlusCP(" + num_plus + ")");
                //$("#db_" + num_plus + " select[name='iDaiBieu']").chosen();
                //$("#db_" + num_plus + " select[name='iDiaPhuong']").chosen();
                $('.datepick').datepicker();
                $("#iDaiBieu").chosen();
                $("#iDiaPhuong").chosen();
                $("#iDiaPhuong_02").chosen();
            }
            else {
                alert("Vui lòng chọn Đơn vị lập");
                $("#iDonVi").focus();
            }
        }
        function ChangeDoanLapKeHoach() {
            LoadLichTiepDaiBieu($("#iDonVi").val(), $("#iDoiTuong").val());
        }
        function ChangeDoiTuong() {
            LoadLichTiepDaiBieu($("#iDonVi").val(), $("#iDoiTuong").val());
        }

        function LoadLichTiepDaiBieu(iDonVi, iDoiTuong) {
            $(".db").remove();
            $("#bt-add").attr("onclick", "PlusCP(0)");
            if (iDonVi && iDonVi > 0) {
                $("#lastrow").show().html("<p class='tcenter'><img src='/Images/ajax-loader.gif' /></p>");
                $.post("/Kiennghi/Ajax_Load_DaiBieu_HDND", "iDoiTuong= " + iDoiTuong + "&iDonVi=" + iDonVi, function (data) {
                    optListDaiBieuHtml = "";
                    optListHuyenHtml = "";
                    if (data && data.listDaiBieu && data.listDaiBieu.length > 0) {
                        data.listDaiBieu.forEach(function (daibieu) {
                            optListDaiBieuHtml += ("<option " + " value='" + daibieu.IPHONGBAN + "'>" + daibieu.CTEN + "</option>");
                        });
                    }
                    if (data && data.listHuyen && data.listHuyen.length > 0) {
                        data.listHuyen.forEach(function (huyen) {
                            optListHuyenHtml += ("<option " + " value='" + huyen.IDIAPHUONG + "'>" + huyen.CTEN + "</option>");
                        });
                    }
                    $("#lastrow").hide();
                });
            }
        }

        function CheckForm() {
            if ($("#iKyHop").val() == 0) {
                alert("Vui lòng chọn kỳ họp!"); $("#iKyHop").focus(); return false;
            }
            if ($("#dKetThuc").val() != "" && $("#dBatDau").val() != "") {
                if (!Validate_DateVN("dBatDau")) {
                    return false;
                }
                if (!CompareDate("dBatDau", "dKetThuc")) {
                    return false;
                }
                if (!Validate_DateVN("dKetThuc")) {
                    return false;
                }
            }
            if ($("#cKeHoach").val() == "") {
                alert("Vui lòng nhập kế hoạch số!"); $("#cKeHoach").focus(); return false;
            }
            if ($("#iDonVi").val() == 0) {
                alert("Vui lòng chọn đoàn lập kế hoạch"); return false;
            }
            ShowPageLoading();
        }
        function HuyDaiBieu(id) {
            $("#daibieu_" + id).remove();
            var daibieu = "," + $("#daibieu_chon").val();
            daibieu = str_replace(daibieu, "," + id + ",", ",");
            $("#daibieu_chon").val(daibieu);
        }
        function HuyDiaPhuong(id) {
            $("#diaphuong_" + id).remove();
            var diaphuong = "," + $("#diaphuong_chon").val();
            diaphuong = str_replace(diaphuong, "," + id + ",", ",");
            $("#diaphuong_chon").val(diaphuong);
        }

        function ChangeHuyenXa(val, num_plus) {
            $.post("/Home/Ajax_Change_Opt_huyenxa", 'id=' + val, function (data) {
                $("#iDiaPhuong_02_" + num_plus).html(data);
                $("#iDiaPhuong_02_" + num_plus).trigger("liszt:updated");
                $("#iDiaPhuong_02_" + num_plus).chosen();
            });
        }

    </script>

</asp:Content>
