<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tập hợp đã có trả lời
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
                        <span>Tập hợp đã có trả lời</span>
                    </li>
                </ul>

            </div>
            <div class="function_chung">
                <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">
                    <button type="button" title="Tìm kiếm" onclick="ShowPopUp('','/Kiennghi/Ajax_export_dacotraloi/')" class="btn_f blue"><i class="icon-print"></i></button>
                    <select class="chosen-select" name="iCanhBao" id="iCanhBao" style="width: 300px; margin-right: 5px">
                        <option value="" disabled selected>Chọn tình trạng</option>
                        <option value="0">Trong hạn</option>
                        <option value="1">Sắp đến hạn</option>
                        <option value="2">Quá hạn</option>
                    </select>
                     <input type="text" name="q" id="q" value="" placeholder="Nội dung, từ khóa">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kiennghi/Ajax_search_dacotraloi/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm nâng cao" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>

                </form>
            </div>
            <div id="search_place"></div>
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Tập hợp đã có trả lời
                            
                            </h3>
                            <div class="pull-right box-title-header">
                                <form id="form_header">
                                    <% if (ViewData["is_bdn"].ToString() == "1")
                                        { %>
                                    <select class="chosen-select" name="iDonViXuLy_Parent" id="iDonViXuLy_Parent" onchange="ChangeDonVi_KyHop();">
                                        <%=ViewData["opt-thamquyen"] %>
                                    </select>
                                    <select class="chosen-select" name="iDonViXuLy" id="iDonViXuLy" onchange="ChangeDonVi_KyHop();">
                                        <%=ViewData["opt-thamquyen-xuly"] %>
                                    </select>
                                    <% } %>
                                </form>
                            </div>
                        </div>
                        <div class="box-content nopadding">

                            <table class="table table-bordered table-condensed">
                                <thead>
                                    <tr>
                                        <th width="3%" class="tcenter">STT </th>
                                        <th class="" width="30%">Ghi chú / Nội dung kiến nghị</th>
                                        <th class="tcenter" nowrap>Tình trạng</th>
                                        <th class="tcenter" nowrap>Trả lời kiến nghị</th>
                                        <th width="10%" class="tcenter" nowrap>Chức năng</th>
                                    </tr>
                                </thead>
                                <tbody id="ip_data">
                                    <%=ViewData["list"] %>
                                    <%=ViewData["phantrang"] %> 
                                </tbody>
                            </table>     
                            <div style="display: none;" id="loadData" class="tcenter"><img src='/Images/ajax-loader.gif' /></div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ChangeDonVi_KyHop() {
            var pramt = $("#form_header").serialize();
            //window.location = "/Kiennghi/Moicapnhat/?" + $("#form_").serialize();
            //alert(window.location);
            location.href = "/Kiennghi/Dacotraloi/?" + pramt;
            //location.href = "/Kiennghi/Moicapnhat/?iDoan=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "";
        }

        function TimKiem() {
            
            var pramt = $("#form_header").serialize();
            //location.href = "/Kiennghi/Dacotraloi/?q=" + $("#q").val() + "&" + pramt + "&iCanhBao=" + $("#iCanhBao").val();
            $("#ip_data").empty().html("");
            $('#loadData').show();
            $.ajax({
                type: "post",
                url: "<%=Url.Action("Dacotraloi", "Kiennghi")%>",
                data: { q: $("#q").val(), iCanhBao: $("#iCanhBao").val(), hidNormalSearch: 1 },
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm tập hợp đã có trả lời!");
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
