<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Bảng tổng hợp kết quả giải quyết, trả lời kiến nghị của cử tri
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
                        <span>Tổng hợp trả lời KNCT của Bộ Ngành</span>
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
                                <i class="icon-search"></i> Tổng hợp trả lời KNCT của Bộ Ngành
                            </h3>

                        </div>
                        <div class="box-content" >
                            <form class="form-horizontal form-column" id="form_export" onsubmit="return Tracuu()">
                                <div class="row-fluid">                                                              
                                    <div class="control-group span6">
                                        <label class="control-label">Loại <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <select id="iLoai" name="iLoai" class="chosen-select" onchange="ChangeKhoaTheoLoai(this.value)">
                                                <option value="0">Quốc hội</option>
                                                <option value="1">Hội đồng nhân dân</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group span6">
                                        <label class="control-label">Chọn kỳ họp <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <div class="input-block-level" id="iKyHopTheoLoai">
                                            <select id="iKyHop" name="iKyHop" class="chosen-select">
                                                <%=ViewData["opt-kyhop"] %>
                                            </select>
                                            </div>
                                        </div>
                                    </div>  
                                </div>
                                <div class="row-fluid">                                                              
                                    <div class="control-group span6">
                                        <label class="control-label">Hình thức <span class="f-red">*</span></label>
                                        <div id="iHinhThuc" name="iHinhThuc" class="controls">
                                            <%=ViewData["check-hinhthuc"] %>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">                                                              
                                    <div class="control-group span6">
                                    </div>
                                    <div class="control-group span6 tright">                                        
                                        <div class="controls">
                                            <button type="submit" class="btn btn-primary"><i class="icon-search"></i> Xem báo cáo</button>
                                            <span onclick="Download('excel')" class="btn btn-primary "><i class="icon-cloud-download"></i> Tải Excel</span>
                                            <span onclick="Download('word')" class="btn btn-primary hide"><i class="icon-cloud-download"></i> Tải Word</span>
                                            <span onclick="Download('pdf')" class="btn btn-primary hide"><i class="icon-cloud-download"></i> Tải PDF</span>
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
            window.location = "/Baocaokiennghi/Kiennghi_TraloiKNTC_excel/?iKyHop=" +
                $("#iKyHop").val() + "&iLoai=" + $("#iLoai").val() + "&iHinhThuc=" + $('input[name="iTruocKyHop"]:checked').val();
        }
        function Tracuu() {
            if ($("#iKyHop").val() == 0) { alert("Vui lòng chọn kỳ họp!"); $("#iKyHop").focus(); return false; }
            $("#search_place").html("<p class='tcenter'><img src='/Images/ajax-loader.gif'/></p>");
            $.post("/Baocaokiennghi/Ajax_Kiennghi_TraloiKNTC", $("#form_export").serialize(), function (ok) {
                //alert(ok);
                $("#search_place").html(ok);
            });
            return false;
        }

        function ChangeKhoaTheoLoai(val) {
            if (val != 2) {
                $.post("/Baocaokiennghi/Ajax_Change_KyHopTheoLoai_option", 'id=' + val, function (data) {
                    $("#iKyHopTheoLoai").show().html(data);
                    $("#iKyHop").chosen();
                });
            } else {
                $("#iKyHopTheoLoai").html("");
            }
        }
    </script>
</asp:Content>
