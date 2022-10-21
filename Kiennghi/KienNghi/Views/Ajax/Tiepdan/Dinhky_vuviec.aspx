<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Vụ việc tiếp nhận qua tiếp dân định kỳ
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Tiepdan") %>
    <div id="main" class="">
        <div class="container-fluid">
            <div class="breadcrumbs">
                <ul>
                    <li>
                        <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i>Trang chủ</a>
                        <i class="icon-angle-right"></i>
                    </li>
                    <li>
                        <a href="#">Vụ việc tiếp nhận qua tiếp dân định kỳ</a>
                    </li>
                </ul>
                <div class="function_chung">

                    <a data-original-title="Thêm mới" rel="tooltip" href="/Tiepdan/Themmoi" class="add btn_f blue" onchange="$('#iTiepDinhKy').toggle()"><i class="icon-plus-sign"></i></a>
                    <form class="search" id="fromsearch" name="fromsearch" onsubmit="return CheckForm();">
                        <input type="hidden" id="id" name="id" value="<%=ViewData["id_dinhky"] %>" />
                        <input name="search" id="search" value="" onkeypress="if(event.keyCode==13){CheckForm()}" placeholder="Nội dung vụ việc" type="text">
                        <a onclick="CheckForm()" class="add btn_f blue" style="margin-top: 0px !important;"><i class="icon-search"></i></a>
                        <button type="button" title="Tìm kiếm" onclick="ShowTimKiem_Conf('id=<%=ViewData["id_dinhky"] %>','/Tiepdan/Ajax_Dinhky_search/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                        <button type="button" title="Tìm kiếm" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
                    </form>
                </div>

            </div>
      
            <div id="search_place"></div>
                
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-time"></i>Vụ việc tiếp nhận qua tiếp dân định kỳ</h3>
                            
                        </div>
                        <div class="box-content nopadding">
                            <form id="form_" onsubmit="return false;">
                                <table class="table table-bordered table-condensed nomargin">
                                    <thead>
                                        <tr>
                                            <th nowrap class="tcenter" width="3%">STT</th>
                                            <th nowrap style="width: 10%; text-align: center">Ngày tiếp</th>
                                            <th nowrap style="width: 35%">Nội dung vụ việc</th>
                                            <th nowrap style="width: 25%">Người gửi / Địa chỉ</th>
                                              <th nowrap style="width:15%">Hình thức giám sát</th>         
                                            <th nowrap style="text-align: center">Hình thức xử lý</th>
                                            <th nowrap class="tcenter" width="8%">Mẫu đơn</th>
                                            <th nowrap class="tcenter" width="8%">Chức năng</th>
                                        </tr>
                                    </thead>
                                     <tbody id="ketqua_tracuu">     
                                    <%= ViewData["ketqua"] %>                                                  
                                    <%=ViewData["list"] %>
                                </tbody> 
                                <%=ViewData["phantrang"] %>  
                                </table>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function CheckForm() {
            
            var tentimkiem = $("#search").val();
            alert(tentimkiem);
            window.location = "/Tiepdan/Dinhky_vuviec/?id=<%=ViewData["id_dinhky"] %>&q=" + tentimkiem;
            return false;
        }

    </script>
</asp:Content>
