<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Công văn chuyển đơn
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Baocao") %>

    <div id="main" class="">
        <div class="container-fluid ">
            <div class="breadcrumbs">
                <ul>
                    <li>
                        <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i>Trang chủ</a>
                        <i class="icon-angle-right"></i>
                    </li>
                    <li>
                        <span>Báo cáo <i class="icon-angle-right"></i></span>

                    </li>
                    <li>
                        <span>Báo cáo đơn thư hàng tháng</span>
                    </li>
                </ul>
                <div class="function_chung">
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3>
                                <i class="icon-search"></i>Báo cáo đơn thư hàng tháng
                            </h3>

                        </div>
                        <div class="box-content" style="text-align: left;">

                            <form method="post" name="form_export" id="form_export" enctype="multipart/form-data" class="form-horizontal form-column">
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Nhập năm</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <span class="span6 nopadding">
                                                        <select id="dNam" name="dNam" class="chosen-select" onchange ="CapNhatNgay()">
                                                            <option value="0">Chọn năm</option>
                                                            <%=ViewData["opt-nam"] %>
                                                        </select></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Nhập tháng </label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <span class="span6 nopadding">
                                                        <select id="dThang" name="dThang" class="chosen-select" onchange="CapNhatNgay()">
                                                            <option value="0">Chọn tháng</option>
                                                             <%= ViewData["opt-thang"]%>
                                                        </select>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" id="dongNgay">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="dTuNgay" class="control-label  ">Từ ngày<i class="f-red">*</i> </label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <span class="span6 nopadding">
                                                        <input type="text" name="dTuNgay" autocomplete="off" id="dTuNgay" class="datepick input-block-level" /></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="dDenNgay" class="control-label  ">Đến ngày<i class="f-red">*</i> </label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <span class="span6 nopadding">
                                                        <input type="text" name="dDenNgay" autocomplete="off" id="dDenNgay" class="datepick input-block-level" /></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12 tright">
                                        <div class="control-group">

                                            <div class="controls">
                                                <div class="input-block-level" style="float: right">
                                                    <a class="btn btn-primary" onclick="Xem()"><i class="icon-search"></i>Xem báo cáo</a>
                                                    <span onclick="TaiExel()" class="btn btn-primary "><i class="icon-cloud-download"></i>Tải Excel</span>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </form>
                        </div>

                    </div>
                </div>
            </div>
            <div id="data"></div>

        </div>
    </div>
    <script>
        // Ham de chuyen 1 so don vi tro thanh 1 string 2 chu so
        function padTo2Digits(num) {
            return num.toString().padStart(2, '0');
        }
        // Format 1 object Date thanh co dang dd/mm/yyyy
        function formatDate(date) {
            return [
                padTo2Digits(date.getDate()),
                padTo2Digits(date.getMonth() + 1),
                date.getFullYear(),
            ].join('/');
        }
        function CapNhatNgay() {
            var year = $("#dNam").val();
            var month = $("#dThang").val();
            // Hien thi ngay bat dau va ket thuc tuong ung voi thang va nam da nhap
            if (year != 0 && month != 0) {
                var firstDay = formatDate(new Date(year, month - 1, 1));
                var lastDay = formatDate(new Date(year, month, 0));
                $("#dDenNgay").val(lastDay);
                $("#dTuNgay").val(firstDay);
            }
            return true;
        }

        //function yearValidation(year) {
        //    var text = /^[0-9]+$/;
        //    if (year == "") {
        //        alert("Xin hãy nhập năm");
        //        return false
        //    }
        //    if (year != 0) {

        //        if ((year != "") && (!text.test(year))) {
        //            $("#dNam").val("");
        //            alert("Xin hãy nhập đúng định dạng năm");
        //            return false;
        //        }

        //        if (year.length != 4) {
        //            $("#dNam").val("");
        //            alert("Xin hãy nhập đúng năm");
        //            return false;
        //        }
        //        return true;
        //    }
        //}
        //function monthValidation(month) {
        //    var date_regex = /^(1[012]|[1-9])$/;
        //    if (!(date_regex.test(month))) {
        //        alert("Xin hãy nhập đúng tháng");
        //        $("#dThang").val("");
        //        return false;
        //    }
        //    return true;
        //}
        function Xem() {
            CapNhatNgay();
            if ($("#dDenNgay").val() == "" || $("#dTuNga").val == "") {
                alert("Xin hãy nhập đủ dữ liệu");
                return false;
            }
            $("#data").show().html("<div class='tcenter'><img src='/Images/ajax-loader.gif'/></div>");
            $.post("/Baocaokntc/Ajax_XemBaoCaoThang", $("#form_export").serialize(), function (ok) {
                $("#data").html(ok);
            });
        }
        function TaiExel() {
            if ($("#dDenNgay").val() == "" || $("#dTuNga").val == "") {
                alert("Xin hãy nhập đủ dữ liệu");
                return false;
            }
            window.location = "/Baocaokntc/Baocaothang_Exl/?tungay=" + $("#dTuNgay").val() + "&denngay=" + $("#dDenNgay").val();
        }
    </script>
</asp:Content>
