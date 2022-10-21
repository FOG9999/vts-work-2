<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Chi tiết thông tin đơn
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
                        <span>Chi tiết thông tin đơn</span>
                    </li>
                </ul>
                <div class="close-bread">
                    <a href="#"><i class="icon-remove"></i></a>
                </div>
            </div>
            <% KNTC_DON don = (KNTC_DON)ViewData["don"];
               KNTC d = (KNTC)ViewData["kn"];
            %>
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Thông tin đơn</h3>
                        </div>
                        <div class="box-content nopadding">
                            <form method="post" id="form_" class="nomargin" action="/Kntc/Kiemtrung_update/">
                                <table class="table table-bordered table-condensed">
                                    <%if (d.nguon != null)
                                      { %>
                                    <tr>
                                        <td colspan="4" class="tcenter f-red"><%=d.nguon %></td>
                                    </tr>

                                    <%} %>
                                    <tr>
                                        <td class="f-" width="15%">Ngày nhận đơn</td>
                                        <td width="35%">
                                            <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(don.DNGAYNHAN)) %>
                                        </td>
                                        <td class="" width="15%">Nguồn đơn</td>
                                        <td>
                                            <%=d.nguondon %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">Người nộp đơn</td>
                                        <td>
                                            <p><%=Server.HtmlEncode(don.CNGUOIGUI_TEN) %>  </p>
                                            <% if (Convert.ToInt16(don.IDOANDONGNGUOI) == 1)
                                               {%>
                                        Đoàn đông người (<%=don.ISONGUOI%> người)
                                         <% } %>
                                        </td>
                                        <td>Số CMND; ngày cấp; nơi cấp</td>
                                        <td><%=Server.HtmlEncode(don.CNGUOIGUI_CMND) %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="f-">Địa chỉ người nộp</td>
                                        <td colspan="3">
                                            <%=d.diachi_nguoinop %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Quốc tịch</td>
                                        <td><%=d.quoctich %>
                                        </td>
                                        <td>Dân tộc</td>
                                        <td><%=d.dantoc %>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="f-">Tóm tắt nội dung đơn</td>
                                        <td colspan="3">
                                            <%=Server.HtmlEncode(don.CNOIDUNG) %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="f-">Ghi chú</td>
                                        <td colspan="3">
                                            <%=Server.HtmlEncode(don.CGHICHU) %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">File đính kèm</td>
                                        <td colspan="3"><%=ViewData["file"] %></td>
                                    </tr>

                                    <tr>
                                        <td class="f-">Loại đơn</td>
                                        <td><%=d.loaidon %>
                                        </td>
                                        <td class="f-">Lĩnh vực</td>
                                        <td><%=d.linhvuc %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nhóm nội dung</td>
                                        <td><%=d.loai_noidung %>
                                        </td>
                                        <td>Tính chất vụ việc</td>
                                        <td><%=d.tinhchat %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Lý do lưu đơn</td>
                                        <td colspan="3"><%=d.lydoluudon %></td>
                                    </tr>
                                    <tr>
                                        <td>Lý do chi tiết</td>
                                        <td colspan="3"><%=d.lydochitiet %></td>
                                    </tr>
                                    <tr>
                                        <td>Tình trạng đơn</td>
                                        <td colspan="3"><%=d.tinhtrang %></td>
                                    </tr>
                                    <tr>
                                        <td>Đơn vị thụ lý</td>
                                        <td><%=d.donvi_thuly %></td>
                                        <td>Đơn vị tiếp nhận</td>
                                        <td><%=d.donvi_tiepnhan %></td>
                                    </tr>
                                    <tr>
                                        <td>Độ mật</td>
                                        <td><%=d.domat %>
                                        </td>
                                        <td>Độ khẩn</td>
                                        <td><%=d.dokhan %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">Kết quả đánh giá</td>
                                        <td colspan="3"><%=d.ketquadanhgia %></td>
                                    </tr>
                                    <tr>
                                        <td class="">Số đơn trùng</td>
                                        <td><%= ViewData["sodontrung"]%></td>
                                        <td class="">Hồ sơ đơn</td>
                                        <td><%=ViewData["hosodon"] %></td>

                                    </tr>
                                    <tr>
                                        <td colspan="4"><strong>Danh sách văn bản thực hiện</strong>
                                            <br />
                                            <%=ViewData["vanban_lienquan"] %>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><strong>Lịch sử chuyển đơn thư</strong>
                                            <br />
                                            <%=ViewData["lichsuluanchuyen"] %>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"><strong>Danh sách đơn trùng</strong>
                                            <br />
                                            <%=ViewData["list_dontrung"] %>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="tcenter">
                                            <%--<a href="<%=Request.Cookies["url_return"].Value %>" onclick="ShowPageLoading()" class="btn btn-warning">Quay lại</a>--%>
                                            <a href="#" onclick="window.history.back();" class="btn btn-warning">Quay lại</a>
                                        </td>
                                    </tr>
                                </table>
                            </form>
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
    </script>
</asp:Content>
