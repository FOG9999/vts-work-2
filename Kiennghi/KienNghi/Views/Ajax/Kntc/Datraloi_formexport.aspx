<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color">
                        <div class="box-title">
                            <h3><i class="icon-print"> </i> In báo cáo danh sách khiếu nại tố cáo đã có văn bản trả lời</h3>
                            <ul class="tabs">
                                <li class="active">
	                                <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
                                </li>
                            </ul>
                        </div>
                        <div class="box-content">
                            <form id="form_" method="post" class="form-horizontal form-column">
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Tên báo cáo:</label>
                                            <div class="controls">
                                                <div class="input-block-level" >
                                                    <select name="iTenBaoCao" id="iTenBaoCao" class="chosen-select input-block-level" onchange="ChangeTenBaoCao(this.value);">
                                                        <option value="1" selected>BÁO CÁO 4A: DANH SÁCH CÁC ĐƠN VỊ ĐÃ TRẢ LỜI ĐƠN CHUYỂN</option>
                                                        <option value="2">BÁO CÁO 4B: BÁO CÁO TỔNG HỢP KẾT QUẢ XỬ LÝ (01-TT02)</option>
                                                        <option value="3">BÁO CÁO 4C: BÁO CÁO TỔNG HỢP KẾT QUẢ XỬ LÝ (02-TT02)</option>
                                                        <option value="4">BÁO CÁO 4D: BÁO CÁO TỔNG HỢP KẾT QUẢ XỬ LÝ (03-TT02)</option>
                                                        <option value="5">BÁO CÁO 4E: KẾT QUẢ TIẾP NHẬN, XỬ LÝ ĐƠN THƯ VÀ GIÁM SÁT VIỆC GIẢI QUYẾT KHIẾU NẠI TỐ CÁO TRONG THÁNG</option>
                                                        <option value="6">BÁO CÁO 4F: KẾT QUẢ TIẾP NHẬN, XỬ LÝ ĐƠN THƯ VÀ GIÁM SÁT VIỆC GIẢI QUYẾT KHIẾU NẠI TỐ CÁO TRONG THÁNG (CHI TIẾT)</option>
                                                        <option value="7">BÁO CÁO 4G: KẾT QUẢ TIẾP NHẬN, XỬ LÝ ĐƠN THƯ KNTC (Theo tỷ lệ)</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                   
                                   <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Loại báo cáo :</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full">
                                                    <select name="iLoaiBaoCao" id="iLoaiBaoCao" class="chosen-select input-block-level">
                                                        <%=ViewData["loaibaocao"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid div-khoa-coquantl">
                                    
                                     <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Chọn khóa</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full">
                                                   <select id="ikhoa" name="ikhoa" class="chosen-select">
                                                        <option value="0">Chọn khóa họp</option>
                                                        <%=ViewData["opt-khoa"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                     <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Cơ quan trả lời:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select onchange="ChangeLinhVucByDonVI(this.value)" id="iCoQuanTraLoi" name="iCoQuanTraLoi" class="chosen-select input-block-level">
                                                        <option value="0">- - - Chọn tất cả</option>
                                                        <%=ViewData["opt-coquantraloi"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                   
                                    <div class="span6 div-ky" style="display: none;">
                                        <div class="control-group">
                                            <label for="iKy" class="control-label">Kỳ:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select onchange="ChangeKy(this.value)" id="iKy" name="iKy" class="chosen-select input-block-level"><%=ViewData["opt-ky"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6 div-year" style="display: none;">
                                        <div class="control-group">
                                            <label for="iYear" class="control-label">Năm<i class="f-red">*</i>:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select id="iYear" name="iYear" onchange="ChangeYear(this.value)" class="chosen-select input-block-level"><%=ViewData["opt-year"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class ="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label " id="khoangthoigian">Ngày công văn trả lời:</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full" style="display:flex;align-content:space-between;">
                                                    <input type="text" class="datepick" autocomplete="off" name="iTuNgay" id="iTuNgay" placeholder="Từ ngày..." style="width:50%;margin-right:5px;"/>
                                                    <input type="text" class="datepick" autocomplete="off" name="iDenNgay" id="iDenNgay" placeholder="Đến ngày..." style="width:50%"/>
                                                    <input type="hidden" id="iTuNgayKyTruoc"/>
                                                    <input type="hidden" id="iDenNgayKyTruoc"/>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class ="span6 div-container" style="display: none;">
                                       <div class="span12 div-month" style="display: none;">
                                            <div class="control-group">
                                                <label for="iMonth" class="control-label">Tháng:</label>
                                                <div class="controls">
                                                    <div class="input-block-level">
                                                        <select id="iMonth" name="iMonth" onchange="ChangeMonth(this.value)" class="chosen-select input-block-level"><%=ViewData["opt-month"] %></select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span12 div-quy" style="display: none;">
                                            <div class="control-group">
                                                <label for="iQuy" class="control-label">Quý:</label>
                                                <div class="controls">
                                                    <div class="input-block-level">
                                                        <select id="iQuy" name="iQuy" onchange="ChangeQuy(this.value)" class="chosen-select input-block-level"><%=ViewData["opt-quy"] %></select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                </div>
                                
                                <div class="row-fluid">
                                    <div class="span6 div-diaphuong" style="display: none;">
                                        <div class="control-group">
                                            <label for="iDiaPhuong" class="control-label">Địa phương:</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full">
                                                    <select id="listDiaPhuong" multiple="multiple" name="listDiaPhuong">
                                                        <%=ViewData["opt-diaphuong"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group tright">
                                            <button type="button" class="btn btn-" data-original-title="In Excel" onclick="XuatBaoCao('xls')"  rel="tooltip"> <i class="icon-print"></i>Xuất Excel</button>
                                            <button type="button" class="btn btn-" data-original-title="In PDF" onclick="XuatBaoCao('pdf')"  rel="tooltip"><i class="icon-file"></i> Xuất PDF</button>
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
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#ikhoa').chosen();
        $('#iLoaiBaoCao').chosen();
        $('#iTenBaoCao').chosen();
        $('#iCoQuanTraLoi').chosen();
        $("#iKy").chosen();
        $("#iYear").chosen();
        $("#iMonth").chosen();
        $("#iQuy").chosen();
        $('#listDiaPhuong').multiselect({
            enableCollapsibleOptGroups: true,
            includeSelectAllOption: true,
            selectAllText: 'Chọn toàn bộ',
            allSelectedText: 'Đã chọn toàn bộ',
            nonSelectedText: 'Chọn Huyện/Thị xã/Thành phố',
            nSelectedText: 'Huyện/Thị xã/Thành phố đã chọn'
        });
    });

    function ChangeTenBaoCao(value) {
        if (value != "1") {
            $(".div-ky").css("display", "block");
            ChangeKy($("#iKy").val());
            $("#khoangthoigian").html("Ngày nhận");
            $(".div-khoa-coquantl").css("display", "none");
            if (value == "6" || value == "7") {
                ChangeKy(value);
            }
        }
        else {
            $(".div-container,.div-ky,.div-year,.div-month,.div-quy, .div-diaphuong").css("display", "none");
            $("#iKy,#iYear").val(0).prop("disabled", false).trigger("liszt:updated");
            $("#iMonth,#iQuy").val(0).trigger("liszt:updated");
            $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
            $("#iTuNgay,#iDenNgay").prop("disabled", false);
            $("#iTuNgayKyTruoc,#iDenNgayKyTruoc").val("");
            $("#khoangthoigian").html("Ngày công văn trả lời");
            $(".div-khoa-coquantl").css("display", "block");
        }
    }

    function ChangeKy(value) {
        switch (value) {
            case "0":
                $(".div-container,.div-year").css("display", "block");
                $(".div-month,.div-quy").css("display", "none");
                $("#iYear").val(0).prop("disabled", false).trigger("liszt:updated");
                $("#iMonth,#iQuy").val(0).trigger("liszt:updated");
                $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
                $("#iTuNgay,#iDenNgay").prop("disabled", false);
                $("#iTuNgayKyTruoc,#iDenNgayKyTruoc").val("");
                break;
            case "1":
                $(".div-container,.div-year").css("display", "block");
                $(".div-month").css("display", "none");
                $(".div-quy").css("display", "block");
                $("#iYear").val(0).prop("disabled", false).trigger("liszt:updated");
                $("#iMonth,#iQuy").val(0).trigger("liszt:updated");
                $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
                $("#iTuNgay,#iDenNgay").prop("disabled", false);
                $("#iTuNgayKyTruoc,#iDenNgayKyTruoc").val("");
                break;
            case "2":
                $(".div-container,.div-year").css("display", "block");
                $(".div-month").css("display", "block");
                $(".div-quy").css("display", "none");
                $("#iYear").val(0).prop("disabled", false).trigger("liszt:updated");
                $("#iMonth,#iQuy").val(0).trigger("liszt:updated");
                $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
                $("#iTuNgay,#iDenNgay").prop("disabled", false);
                $("#iTuNgayKyTruoc,#iDenNgayKyTruoc").val("");
                break;
            case "3":
                $(".div-container,.div-year").css("display", "block");
                $(".div-month,.div-quy").css("display", "none");
                $("#iYear").val(0).prop("disabled", true).trigger("liszt:updated");
                $("#iMonth,#iQuy").val(0).trigger("liszt:updated");
                $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
                $("#iTuNgay,#iDenNgay").prop("disabled", false);
                $("#iTuNgayKyTruoc,#iDenNgayKyTruoc").val("");
                break;
            case "6":
                $(".div-container,.div-year").css("display", "block");
                $(".div-month,.div-quy").css("display", "none");
                $("#iYear").val(0).prop("disabled", false).trigger("liszt:updated");
                $("#iMonth,#iQuy").val(0).trigger("liszt:updated");
                $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
                $("#iTuNgayKyTruoc,#iDenNgayKyTruoc").val("");
                $(".div-diaphuong").css("display", "block");
                break;
            case "7":
                $(".div-container,.div-year").css("display", "block");
                $(".div-month,.div-quy").css("display", "none");
                $("#iYear").val(0).prop("disabled", false).trigger("liszt:updated");
                $("#iMonth,#iQuy").val(0).trigger("liszt:updated");
                $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
                $("#iTuNgayKyTruoc,#iDenNgayKyTruoc").val("");
                $(".div-diaphuong").css("display", "block");
                break;
            default:
                $(".div-container,.div-ky,.div-year,.div-month,.div-quy").css("display", "none");
                $("#iYear").val(0).prop("disabled", false).trigger("liszt:updated");
                $("#iMonth,#iQuy,#iKy").val(0).trigger("liszt:updated");
                $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
                $("#iTuNgay,#iDenNgay").prop("disabled", false);
                $("#iTuNgayKyTruoc,#iDenNgayKyTruoc").val("");
                break;
        }
    }

    function ChangeYear(value) {
        if (value == "0") {
            $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
            $("#iTuNgay,#iDenNgay").prop("disabled", false);
            $("#iTuNgayKyTruoc,#iDenNgayKyTruoc").val("");
            return;
        }
        switch ($("#iKy").val()) {
            case "0":
                $("#iTuNgay,#iDenNgay").prop("readonly", true);
                $("#iTuNgay,#iDenNgay").prop("disabled", true);
                $('#iTuNgay').val($.date(new Date(value, 0, 1)));
                $('#iDenNgay').val($.date(new Date(value, 12, 0)));
                $('#iTuNgayKyTruoc').val($.date(new Date(value - 1, 0, 1)));
                $('#iDenNgayKyTruoc').val($.date(new Date(value - 1, 12, 0)));
                break;
            case "1":
                var arrFirstMonth = [1, 4, 7, 10];
                var arrLastMonth = [3, 6, 9, 12];

                $("#iTuNgay,#iDenNgay").prop("readonly", true);
                $("#iTuNgay,#iDenNgay").prop("disabled", true);
                $('#iTuNgay').val($.date(new Date(value, $("#iQuy").val() != "0" ? (arrFirstMonth[parseInt($("#iQuy").val()) - 1] - 1) : 0, 1)));
                $('#iDenNgay').val($.date(new Date(value, $("#iQuy").val() != "0" ? arrLastMonth[parseInt($("#iQuy").val()) - 1] : 12, 0)));
                $('#iTuNgayKyTruoc').val($.date(new Date((($("#iQuy").val() == "0" || $("#iQuy").val() == "1") ? (value - 1) : value),
                    $("#iQuy").val() != "0" ? ($("#iQuy").val() == "1" ? 9 : (arrFirstMonth[parseInt($("#iQuy").val()) - 2] - 1)) : 0, 1)));
                $('#iDenNgayKyTruoc').val($.date(new Date((($("#iQuy").val() == "0" || $("#iQuy").val() == "1") ? (value - 1) : value),
                    $("#iQuy").val() != "0" ? ($("#iQuy").val() == "1" ? 12 : arrLastMonth[parseInt($("#iQuy").val()) - 2]) : 12, 0)));
                break;
            case "2":
                $("#iTuNgay,#iDenNgay").prop("disabled", true);
                $("#iTuNgay,#iDenNgay").prop("readonly", true);
                $('#iTuNgay').val($.date(new Date(value, $("#iMonth").val() != "0" ? (parseInt($("#iMonth").val()) - 1) : 0, 1)));
                $('#iDenNgay').val($.date(new Date(value, $("#iMonth").val() != "0" ? parseInt($("#iMonth").val()) : 12, 0)));
                $('#iTuNgayKyTruoc').val($.date(new Date((($("#iMonth").val() == "0" || $("#iMonth").val() == "1") ? (value - 1) : value),
                    $("#iMonth").val() != "0" ? ($("#iMonth").val() == "1" ? 11 : (parseInt($("#iMonth").val()) - 2)) : 0, 1)));
                $('#iDenNgayKyTruoc').val($.date(new Date((($("#iMonth").val() == "0" || $("#iMonth").val() == "1") ? (value - 1) : value),
                    $("#iMonth").val() != "0" ? ($("#iMonth").val() == "1" ? 12 : (parseInt($("#iMonth").val()) - 1)) : 12, 0)));
                break;
            default:
                $("#iTuNgay,#iDenNgay").prop("readonly", false);
                $("#iTuNgay,#iDenNgay").prop("disabled", false);
                break;
        }
    }

    function ChangeMonth(value) {
        var year = parseInt($("#iYear").val());
        if (year == 0) {
            $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
            $("#iTuNgay,#iDenNgay").prop("disabled", false);
            return;
        }

        $("#iTuNgay,#iDenNgay").prop("readonly", true);
        $("#iTuNgay,#iDenNgay").prop("disabled", true);
        $('#iTuNgay').val($.date(new Date(year, value != "0" ? (parseInt(value) - 1) : 0, 1)));
        $('#iDenNgay').val($.date(new Date(year, value != "0" ? parseInt(value) : 12, 0)));
        $('#iTuNgayKyTruoc').val($.date(new Date(((value == "0" || value == "1") ? (year - 1) : year),
            value != "0" ? (value == "1" ? 11 : (parseInt(value) - 2)) : 0, 1)));
        $('#iDenNgayKyTruoc').val($.date(new Date(((value == "0" || value == "1") ? (year - 1) : year),
            value != "0" ? (value == "1" ? 12 : (parseInt(value) - 1)) : 12, 0)));
    }

    function ChangeQuy(value) {
        var year = parseInt($("#iYear").val());
        if (year == 0) {
            $("#iTuNgay,#iDenNgay").val("").prop("readonly", false);
            $("#iTuNgay,#iDenNgay").prop("disabled", false);
            return;
        }

        var arrFirstMonth = [1, 4, 7, 10];
        var arrLastMonth = [3, 6, 9, 12];

        $("#iTuNgay,#iDenNgay").prop("readonly", true);
        $("#iTuNgay,#iDenNgay").prop("disabled", true);
        $('#iTuNgay').val($.date(new Date(year, value != "0" ? (arrFirstMonth[parseInt(value) - 1] - 1) : 0, 1)));
        $('#iDenNgay').val($.date(new Date(year, value != "0" ? arrLastMonth[parseInt(value) - 1] : 12, 0)));
        $('#iTuNgayKyTruoc').val($.date(new Date(((value == "0" || value == "1") ? (year - 1) : year),
            value != "0" ? (value == "1" ? 9 : (arrFirstMonth[parseInt(value) - 2] - 1)) : 0, 1)));
        $('#iDenNgayKyTruoc').val($.date(new Date(((value == "0" || value == "1") ? (year - 1) : year),
            value != "0" ? (value == "1" ? 12 : arrLastMonth[parseInt(value) - 2]) : 12, 0)));
    }

    $.date = function (dateObject) {
        var d = new Date(dateObject);
        var day = d.getDate();
        var month = d.getMonth() + 1;
        var year = d.getFullYear();
        if (day < 10) {
            day = "0" + day;
        }
        if (month < 10) {
            month = "0" + month;
        }
        var date = day + "/" + month + "/" + year;

        return date;
    };

    function XuatBaoCao(ext) {
        var tenbaocao = $('#iTenBaoCao').val();
        var tuNgay = $('#iTuNgay').val();
        var denNgay = $('#iDenNgay').val();
        if (tenbaocao != "1") {
            switch ($("#iKy").val()) {
                case "0":
                case "1":
                case "2":
                    if ($("#iYear").val() == "0") {
                        alert("Vui lòng chọn năm!");
                        return;
                    }
                    break;
                default:
                    if ($.trim($('#iTuNgay').val()) == "" || $.trim($('#iDenNgay').val()) == "") {
                        alert("Vui lòng chọn ngày nhận!");
                        return;
                    }
                    if (($.trim($('#iTuNgay').val()) != "" && !Validate_DateVN("iTuNgay"))
                        || ($.trim($('#iDenNgay').val()) != "" && !Validate_DateVN("iDenNgay"))) {
                        return;
                    }
                    if ($.trim($('#iTuNgay').val()) != "" && $.trim($('#iDenNgay').val()) != "") {
                        if (!CompareDate("iTuNgay", "iDenNgay")) {
                            return;
                        }
                    }
                    break;
            }
        }
        var khoa = $('#ikhoa').val();
        var loaibaocao = $('#iLoaiBaoCao').val();
        var coQuanTraLoi = $('#iCoQuanTraLoi').val();

        var url = "/Kntc/Baocao_DonDaTraLoi?ikhoa=" + khoa + "&iTenbaocao=" + tenbaocao + "&iLoaiBaoCao="
            + loaibaocao + "&iCoQuanTraLoi=" + coQuanTraLoi + "&iTuNgay=" + tuNgay + "&iDenNgay=" + denNgay + "&ext=" + ext;
        if (tenbaocao != "1") {
            var text = "";
            if ($("#iKy").val() == 0) {
                text += "Năm " + $("#iYear").val();
            }
            if ($("#iKy").val() == 1) {
                if ($("#iQuy").val() == 1) text += "Quý I ";
                if ($("#iQuy").val() == 2) text += "Quý II ";
                if ($("#iQuy").val() == 3) text += "Quý III ";
                if ($("#iQuy").val() == 4) text += "Quý IV ";
                text += "Năm " + $("#iYear").val();
            }
            if ($("#iKy").val() == 2) {
                if ($("#iMonth").val() != 0 )text += "Tháng " + $("#iMonth").val() + " ";
                text += "Năm " + $("#iYear").val();
            }
            if ($("#iKy").val() == 3) {
                text += "Từ ngày " + tuNgay + " Đến ngày " + denNgay;
            }
            console.log(text);
            url += "&iTuNgayKyTruoc=" + $("#iTuNgayKyTruoc").val() + "&iDenNgayKyTruoc=" + $("#iDenNgayKyTruoc").val() + "&text=" + text;
        }
        if (tenbaocao == "6" || tenbaocao == "7") {
            url += "&listDiaPhuong=" + $("#listDiaPhuong").val();
        }

        window.open(url, '_blank');
    }
</script>
