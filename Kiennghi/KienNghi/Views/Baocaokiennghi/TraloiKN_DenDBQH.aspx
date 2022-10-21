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
                        <span>Tổng hợp kiến nghị cử tri gửi đến các đoàn đại biểu</span>
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
                                <i class="icon-search"></i> Tổng hợp kiến nghị cử tri gửi đến các đoàn đại biểu
                            </h3>

                        </div>
                        <div class="box-content" >
                            <form class="form-horizontal form-column" id="form_export" name="form_export" onsubmit="return Tracuu()">
                                <div class="row-fluid">                                                              
                                    <div class="control-group span6">
                                        <label class="control-label">Kiến nghị gửi tới <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <select id="iLoai" name="iLoai" class="chosen-select" onchange="ChangeKhoaTheoLoai(this.value)" >
                                                <option value="0">Đại biểu quốc hội</option>
                                                <option value="1">Đại biểu hội đồng nhân dân</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group span6">
                                        <label class="control-label">Khóa <span class="f-red">*</span></label>
                                        <div class="controls">
                                            <div class="input-block-level" id="iKhoaTheoLoai">
                                            <select id="iKhoa" name="iKhoa" class="input-block-level chosen-select" onchange="ChangeKhoa(this.value)">
                                                <%=ViewData["khoa"] %>
                                            </select>
                                            </div>
                                        </div>
                                    </div>  
                                </div>
                                <div class="row-fluid">                                                              
                                    <div class="control-group span6">
                                        <label class="control-label">Kỳ họp <span class="f-red">*</span></label>
                                        <div id="iKyHop" name="iKyHop" class="controls">
                                            <%=ViewData["kyhop-theokhoa"] %>
                                        </div>
                                        <div id="phanloai_child" style="margin-top:10px"></div>
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
            if ($("#iKhoa").val() == 0) {
                alert("Vui lòng chọn khóa họp!"); $("#iKhoa").focus(); return false;
            }
            var cbox = document.forms["form_export"]["ckboxKyhop"];
            var check = 0;
            var idcheck = "x";
            var idcheck1 = 1;
            for (var n = 0; n < cbox.length ; n++) {
                if (cbox[n].checked == true) {
                    check++;
                    idcheck += "" + cbox[n].value + "x";
                }
            }
            if (idcheck1 == 1) {
                if (cbox.checked == true) {
                    check++;
                    idcheck += "" + cbox.value + "x";
                }
            }
            if (check == 0 ) {
                alert("Vui lòng chọn kỳ họp!"); return false;
            }
            if ($("#iKhoa").val() == 0) {
                alert("Vui lòng chọn khóa!"); $("#iKhoa").focus(); return false;
            }
            window.location = "/Baocaokiennghi/Kiennghi_TraloiKN_DenDBQH_excel/?iKhoa=" + $("#iKhoa").val() + "&iLoai=" + $("#iLoai").val() + "&iKyHop=" + idcheck;
        }
        function Tracuu() {
            if ($("#iKhoa").val() == 0) {
                alert("Vui lòng chọn khóa họp!"); $("#iKhoa").focus(); return false;
            }
            var cbox = document.forms["form_export"]["ckboxKyhop"];
            var check = 0;
            var idcheck = "x";
            var idcheck1 = 1;
            for (var n = 0; n < cbox.length; n++) {
                idcheck1 = 0;
                if (cbox[n].checked == true) {
                    check++;
                    idcheck += "" + cbox[n].value + "x";
                }
            }
            if (idcheck1 == 1) {
                if(cbox.checked == true) {
                    check++;
                    idcheck += "" + cbox.value + "x";
                }
            }
            if (check == 0) {
                alert("Vui lòng chọn kỳ họp!"); return false;
            }
            if ($("#iKhoa").val() == 0) {
                alert("Vui lòng chọn khóa!"); $("#iKhoa").focus(); return false;
            }
            $("#search_place").html("<p class='tcenter'><img src='/Images/ajax-loader.gif'/></p>");
            $.post("/Baocaokiennghi/Ajax_Kiennghi_TraloiKN_DenDBQH/?iKyHop=" + idcheck +"", $("#form_export").serialize(), function (ok) {
                //alert(ok);
                $("#search_place").html(ok);
            });
            return false;
        }
        function ChangeKhoa(val) {
            if (val != 0) {
                $.post("/Baocaokiennghi/Ajax_Change_Khoa_option", 'id=' + val, function (data) {
                    $("#iKyHop").show().html(data);
                });
            } else {
                $("#iKyHop").html("");
            }
        }

        function ChangeKhoaTheoLoai(val) {
            if (val != 2) {
                $.post("/Baocaokiennghi/Ajax_Change_KhoaTheoLoai_option", 'id=' + val, function (data) {
                    $("#iKhoaTheoLoai").show().html(data);
                    $("#iKhoa").chosen();
                });
            } else {
                $("#iKhoaTheoLoai").html("");
            }
        }

        
    </script>
</asp:Content>
