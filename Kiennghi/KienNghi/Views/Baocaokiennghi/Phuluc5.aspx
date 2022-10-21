<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Danh mục kiến nghị tồn đọng qua nhiều kỳ họp đang trong quá trình giải quyết 
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
                        <span>Danh mục kiến nghị tồn đọng qua nhiều kỳ họp đang trong quá trình giải quyết </span>
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
                                <i class="icon-search"></i>Danh mục kiến nghị tồn đọng qua nhiều kỳ họp đang trong quá trình giải quyết 
                            </h3>

                        </div>
                        <div class="box-content">
                            <form class="form-horizontal form-column" id="form_export" onsubmit="return Tracuu()">
                                <div class="row-fluid">

                                    <div class="control-group span6">
                                        <label class="control-label">Thời gian giải quyết dứt điểm</label>
                                        <div class="controls">
                                            <select name="type" id="type" onchange="ChangeThoiGianGiaiQuyet(this.value)" class="input-small">
                                                <option value="nam">Theo năm</option>
                                                <option value="quy">Theo quý</option>
                                                <option value="thang">Theo tháng</option>
                                            </select>
                                            <span id="quy" class="type_thoigian input-small" style="display: none">
                                                <select name="quy_giaiquyet" id="quy_giaiquyet" class="input-small"><%=ViewData["opt-quy"] %></select>
                                            </span>
                                            <span id="thang" class="type_thoigian input-small" style="display: none">
                                                <select name="thang_giaiquyet" id="thang_giaiquyet" class="input-small"><%=ViewData["opt-thang"] %></select>
                                            </span>
                                            <span id="nam" class="type_thoigian input-small ">
                                                <select name="nam_giaiquyet" id="nam_giaiquyet" class="input-small"><%=ViewData["opt-nam"] %></select>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="control-group span6">
                                        <label class="control-label">Đơn vị tiếp nhận</label>
                                        <div class="controls">
                                            <select class="chosen-select" id="iDonVi" name="iDonVi">
                                                <%=ViewData["opt-coquan"] %>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label">Lĩnh vực</label>
                                        <div class="controls">
                                            <select class="chosen-select" name="iLinhVuc" id="iLinhVuc">
                                                <option value="-1">Chọn tất cả</option>
                                                <%=ViewData["opt-linhvuc"] %>
                                            </select>
                                        </div>
                                    </div>

                                </div>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12 tright">
                                        <div class="control-group">

                                            <div class="controls">
                                                <div class="input-block-level" style="float: right">
                                                    <button type="submit" class="btn btn-primary"><i class="icon-search"></i>Xem báo cáo</button>
                                                    <span onclick="Download('excel')" class="btn btn-primary "><i class="icon-cloud-download"></i>Tải Excel</span>
                                                    <span onclick="Download('word')" class="btn btn-primary hide"><i class="icon-cloud-download"></i>Tải Word</span>
                                                    <span onclick="Download('pdf')" class="btn btn-primary hide"><i class="icon-cloud-download"></i>Tải PDF</span>
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
            <div id="search_place" class="nomargin"></div>
        </div>
    </div>
    <script type="text/javascript">
        function ChangeThoiGianGiaiQuyet(val) {
            $(".type_thoigian").hide();
            $("#" + val).show();
            $("#nam").show();
        }
        function Download(type) {
            if ($("#iKyHop").val() == 0) {
                alert("Vui lòng chọn kỳ họp!"); $("#iKyHop").focus(); return false;
            }
            window.location = "/Baocaokiennghi/Kiennghi_phuluc5_excel/?type=" +
                $("#type").val() + "&quy_giaiquyet=" + $("#quy_giaiquyet").val() + "&nam_giaiquyet=" + $("#nam_giaiquyet").val() +
                "&thang_giaiquyet=" + $("#thang_giaiquyet").val() + "&iDonVi=" + $("#iDonVi").val() + "&iLinhVuc=" + $("#iLinhVuc").val();
        }
        function Tracuu() {
            if ($("#iKyHop").val() == 0) { alert("Vui lòng chọn kỳ họp!"); $("#iKyHop").focus(); return false; }
            $("#search_place").html("<p class='tcenter'><img src='/Images/ajax-loader.gif'/></p>");
            $.post("/Baocaokiennghi/Ajax_Kiennghi_phuluc5", $("#form_export").serialize(), function (ok) {
                //alert(ok);
                $("#search_place").html(ok);
            });
            return false;
        }
    </script>
</asp:Content>
