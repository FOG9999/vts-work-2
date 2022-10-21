<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Đơn mới cập nhật
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
                        <span>Khiếu nại tố cáo   <i class="icon-angle-right"></i></span>
                    </li>
                    <li>
                        <span>Đơn mới cập nhật</span>
                    </li>
                </ul>

            </div>
            <div class="function_chung">
                <a href="/Kntc/Tiepnhan/" title="Thêm mới đơn" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="post" onsubmit="return false;">

                    <%--<button type="button" title="Chuyển xử lý" onclick="ChuyenXuLy()" class="btn_f blue"><i class="icon-signout"></i></button>--%>
                    <select id="iKyHop" name="iKyHop" class="chosen-select">
                            <option value="0">Chọn khóa họp</option>
                        <%=ViewData["opt-kyhop"] %>
                    </select>
                    <input type="text" name="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" id="ip_noidung" value="<%=ViewData["ip_noidung"] %>" placeholder="Nội dung đơn, người gửi đơn...">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kntc/Ajax_Moicapnhat_formsearch/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>

                </form>
            </div>
            <div id="search_place"></div>
            <div class="row-fluid" id="">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-time"></i>Danh sách đơn mới cập nhật</h3>

                        </div>
                        <div class="box-content nopadding">

                            <table class="table table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th style="width: 5%">STT</th>
                                        <th style="width: 25%">Người nộp/Địa chỉ người nộp</th>
                                        <th style="width: 5%">Ngày nhận</th>
                                        <th style="width: 55%">Nội dung đơn</th>
                                        <th style="width: 10%">Chức năng</th>
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
    <script type="text/javascript">
        $(window).on("pageshow", function (event) {
            HidePageLoading();
        });
        function ChuyenXuLy() {
            var val = "";
            $('input[name="don"]:checked').each(function () {
                val += this.value + ",";
            });

            if (val == "") {
                alert("Vui lòng chọn đơn cần chuyển lưu trữ!");
            } else {
                //ShowPopUp("val=" + val, "/LuuTru/Ajax_Pop_ChuyenLuuTru_HoSos");
                ShowPopUp('id=' + val + '', "/Kntc/Ajax_Chuyen_Xuly");

            }
        }
        function TimKiem() {
           
            location.href = "/Kntc/Moicapnhat/?ikhoa=" + $("#iKyHop").val() +"&ip_noidung=" + $("#ip_noidung").val();
            return false;

        }
    </script>
</asp:Content>
