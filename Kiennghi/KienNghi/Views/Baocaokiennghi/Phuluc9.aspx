<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Bản thống kê thời gian nhận được báo cáo tổng hợp ý kiến, kiến nghị của cử tri
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
                        <span>Bản thống kê thời gian nhận được báo cáo tổng hợp ý kiến, kiến nghị của cử tri </span>
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
                                <i class="icon-search"></i>Bản thống kê thời gian nhận được báo cáo tổng hợp ý kiến, kiến nghị của cử tri
                            </h3>
                        </div>
                        <div class="box-content popup_info nopadding">
                            <form class="form-horizontal form-column" id="form_export" onsubmit="return Tracuu()">
                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label">Chọn kỳ họp <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <select id="iKyHop" name="iKyHop" class="chosen-select">
                                                <%=ViewData["opt-kyhop"] %>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group span6">
                                        <label class="control-label">Hình thức</label>
                                        <div class="controls">
                                            <%=ViewData["check-hinhthuc"] %>
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
        function Download(type) {
            if ($("#iKyHop").val() == 0) {
                alert("Vui lòng chọn kỳ họp!"); $("#iKyHop").focus(); return false;
            }
            window.location = "/Baocaokiennghi/Kiennghi_phuluc9_excel/?iKyHop=" +
                $("#iKyHop").val() + "&iTruocKyHop=" + $("#iTruocKyHop").val();
        }
        function Tracuu() {
            if ($("#iKyHop").val() == 0) { alert("Vui lòng chọn kỳ họp!"); $("#iKyHop").focus(); return false; }
            $("#search_place").html("<p class='tcenter'><img src='/Images/ajax-loader.gif'/></p>");
            $.post("/Baocaokiennghi/Ajax_Kiennghi_phuluc9", $("#form_export").serialize(), function (ok) {
                //alert(ok);
                $("#search_place").html(ok);
            });
            return false;
        }
    </script>
</asp:Content>
