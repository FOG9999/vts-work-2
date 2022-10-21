<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Đơn đã nhận chờ xử lý
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Kntc") %>
    <div id="main" class="">
        <div class="container-fluid">
            <div class="breadcrumbs">
                <ul>
                    <li>
                        <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i>Trang chủ</a>
                        <i class="icon-angle-right"></i>
                    </li>
                    <li>
                        <span>Khiếu nại tố cáo <i class="icon-angle-right"></i></span>

                    </li>
                    <li>
                        <span>Đơn đã nhận chờ xử lý, giải quyết</span>
                    </li>
                </ul>

            </div>
            <div class="function_chung">
                <form class="search" id="form_search" method="get" onsubmit="return false;">
                    <%-- <%=ViewData["select-donvi"] %>--%>
                    <select id="iKyHop" name="iKyHop" class="chosen-select">
                            <option value="0">Chọn khóa họp</option>
                        <%=ViewData["opt-kyhop"] %>
                    </select>
                    <input type="text" name="ip_noidung" id="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" value="<%=ViewData["ip_noidung"] %>" placeholder="Nội dung đơn, người gửi đơn...">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>

                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kntc/Ajax_Danhanxuly_formsearch/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>

                </form>
            </div>
            <div id="search_place"></div>
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-time"></i>Danh sách đơn đã nhận chờ xử lý, giải quyết</h3>
                            <div class="pull-right box-title-header">
                                <%-- <select class="chosen-select" name="iTrangThai" id="iTrangThai" onchange="ChangeDonViTrangThai();">
                                    <%=ViewData["select-trangthai"] %>
                                </select>
                                --%>
                                <%--  <select class="chosen-select" name="iDonVi" id="iDonVi" onchange="ChangeDonViTrangThai();">
                                <option value="0">--- Chọn tất cả</option>
                                <%=ViewData["select-donvi"] %>
                            </select>--%>
                            </div>
                        </div>
                        <div class="box-content nopadding">

                            <table class="table table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th class="tcenter" width="5%">STT</th>
                                        <th width="25%">Người gửi / địa chỉ</th>
                                        <th width="60%">Nội dung đơn</th>
                                        <th class="tcenter" width="10%">Chức năng</th>
                                    </tr>
                                </thead>
                                <tbody id="ip_data">
                                    <%=ViewData["list"] %>
                                    <%=ViewData["phantrang"] %>
                                </tbody>
                            </table>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(window).on("pageshow", function (event) {
            HidePageLoading();
        });
        function TimKiem() {
            location.href = "/Kntc/Danhanxuly/?ikhoa=" + $("#iKyHop").val() +"&ip_noidung=" + $("#ip_noidung").val();
            return false;
        }
    </script>
</asp:Content>
