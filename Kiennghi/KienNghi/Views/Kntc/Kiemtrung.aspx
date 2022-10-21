<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kiểm trùng đơn
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
                        <span>Kiểm trùng đơn</span>
                    </li>
                </ul>
                <div class="close-bread">
                    <a href="#"><i class="icon-remove"></i></a>
                </div>
            </div>
            <% KNTC_DON don = (KNTC_DON)ViewData["don"];
               KNTC d = (KNTC)ViewData["don_detail"];
            %>
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Kiểm trùng đơn</h3>
                        </div>
                        <div class="box-content nopadding">
                            <form method="post" id="form_" class="nomargin">

                                <table class="table table-bordered table-condensed">
                                    <tr>
                                        <th colspan="4">Thông tin đơn</th>
                                    </tr>
                                    <tr>
                                        <td class="f-" width="15%">Đối tượng gửi</td>
                                        <td width="35%">
                                            <%= don.IDOITUONGGUI == 0 ? "Quốc hội" : "Hội Đồng Nhân Dân"%>
                                        </td>
                                    </tr>
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
                                        <td colspan="1">
                                            <%=d.diachi_nguoinop %>
                                        </td>
                                        <td class =""> Số điện thoại người nộp</td>
                                         <td><%=Server.HtmlEncode(don.CNGUOIGUI_SDT) %></td>
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
                                        <th colspan="4">Đơn thư trùng / Có nội dung tương tự</th>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="nopadding">
                                            <%=ViewData["list_dontrung"] %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="tcenter">
                                            <%var idt = ""; %>
                                            <input type="hidden" value="<%=ViewData["idoncheck"] %>" id="iDontrung" name="iDontrung" />
                                            <input type="hidden" name="id" id="id" value="<%=ViewData["id"] %>" />
                                            <span id="btn-dong" <%= ViewData["dontrung"] %>></span>
                                            <span id="phanloai" <%= ViewData["style"] %>>
                                                <a onclick="PhanLoai()" class="btn btn-success">Phân loại </a>
                                            </span>

                                            <input id="capnhat" <%= ViewData["chuyenvien"] %> onclick="ShowPageLoading()" type="submit" class="btn btn-success" value="Cập nhật nội dung" />
                                            <a href="/Kntc/Themmoidon" class="btn btn-success">Thêm mới đơn</a>
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
    <script type="text/javascript">
        $(window).on("pageshow", function (event) {
            HidePageLoading();
        });
        function PhanLoai()
        {
            //alert($("#form_").serialize());
            //return false;
            ShowPageLoading();
            var data = $("#form_").serialize();
            location.href = "/Kntc/Phanloai/?" + data;
        }
        function KiemTrung() {
            $("#dontrung").show().html("<td colspan='4' class='tcenter'><img src='/Images/ajax-loader.gif' /></td>");
            $.post("/Kntc/Ajax_Check_Trungdon", 'id=' + $('#iDon').val(), function (data) {
                $("#dontrung").html(data);
            });
        }
        function ChangeThamQuyen(val) {
            if (val == 1) {
                $("#thuocthamquyen").show();
            } else {
                $("#thuocthamquyen").hide();
            }
        }
        function ChonDonTrung(val) {

            $(".chontrung").removeClass("trans_func").addClass("f-grey");
            var idoncheck2 = $("#iDontrung").val();
            if (val != idoncheck2) {

                $("#iDontrung").val(val);
                $("#btn_" + val).addClass("trans_func").removeClass("f-grey");
                $("#btn-dong").show();
                $("#btn-dong").html("<a  onclick=ShowPopUp('id=" + $("#id").val() + "&itrung=" + val + "','/Kntc/Ajax_Lydotrung') class=\"btn btn-primary\" href=\"javascript:void(0)\">Cập nhật trùng</a>");
                // alert(idt);
            }
            else {
                idt = "";
                $("#iDontrung").val("");
                $("#btn_" + val).removeClass("trans_func").addClass("f-grey");
                $("#btn-dong").hide();
            }
            //$.post(url, post, function (data) {
            //    //alert(data);
            //    $(".chontrung").removeClass("trans_func").addClass("f-grey");
            //    if (data == 1) {//Chọn
            //        $("#btn_" + id_trung).addClass("trans_func").removeClass("f-grey");
            //        $("#btn-dong").show();
            //    } else {
            //        $("#btn-dong").hide();
            //    }
            //    AlertAction("Cập nhật thành công!");

            //});
        }

    </script>
</asp:Content>
