<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tổng hợp theo dõi giải quyết đơn
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
                        <span>Tổng hợp theo dõi giải quyết đơn</span>
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
                                <i class="icon-search"></i>Tổng hợp theo dõi giải quyết đơn
                            </h3>

                        </div>
                        <div class="box-content" style="text-align: left;">

                            <form method="post" name="form_export" id="form_export" enctype="multipart/form-data" class="form-horizontal form-column">

                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label">Đơn gửi đến</label>
                                        <div class="controls">
                                            <select id="iLoai" name="iLoai" class="chosen-select" onchange="ChangeKhoaTheoLoai(this.value)">
                                                <option value="0">Quốc hội</option>
                                                <option value="1">Hội đồng nhân dân</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Khóa họp <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level" id="iKyHopTheoLoai">
                                                    <select class="input-block-level chosen-select" id="iKyHop" name="iKyHop">
                                                        <%=ViewData["opt-kyhop"] %>
                                                    </select>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Từ ngày </label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <span class="span6 nopadding">
                                                        <input type="text" value="<%=DateTime.Now.ToString("dd/MM/yyyy") %>" name="dTuNgay" autocomplete="off" id="dTuNgay" class="datepick input-block-level" /></span>
                                                    <span class="span6 nopadding">
                                                        <input type="text" value="<%=DateTime.Now.ToString("dd/MM/yyyy") %>" name="dDenngay" autocomplete="off" id="dDenngay" class="datepick input-block-level" /></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12 tright">
                                        <div class="control-group">

                                            <div class="controls">
                                                <div class="input-block-level" style="float: right">
                                                    <a class="btn btn-primary" onclick="Xem()"><i class="icon-search"></i>Xem báo cáo</a>
                                                    <span onclick="TaiExel()" class="btn btn-primary "><i class="icon-cloud-download"></i>Tải Excel</span>

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
            <div id="data"></div>

        </div>
    </div>
    <script>
        function Xem() {
            $("#data").show().html("<div class='tcenter'><img src='/Images/ajax-loader.gif'/></div>");
            $.post("/Baocaokntc/Ajax_Xembaocao_Theodoigiaiquyetdon", $("#form_export").serialize(), function (ok) {
                $("#data").html(ok);
            });
            return false;

        }

        function ChangeKhoaTheoLoai(val) {
            if (val != 2) {
                $.post("/Kntc/Ajax_Change_KyHopTheoLoai_option", 'id=' + val, function (data) {
                    $("#iKyHopTheoLoai").show().html(data);
                    $("#iKyHop").chosen();
                });
            } else {
                $("#iKyHopTheoLoai").html("");
            }
        }

        function TaiExel() {

            window.location = "/Baocaokntc/Theodoigiaiquyetdon_Exl/?tungay=" + $("#dTuNgay").val() + "&denngay=" + $("#dDenngay").val() + "&iKyHop=" + $("#iKyHop").val() + "&iLoai=" + $("#iLoai").val();
        }
    </script>
</asp:Content>
