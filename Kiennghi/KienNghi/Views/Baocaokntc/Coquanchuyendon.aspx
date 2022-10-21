<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Báo cáo theo cơ quan chuyển đơn
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
                        <span>Theo cơ quan chuyển đơn</span>
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
                                <i class="icon-search"></i>Thống kế đơn theo nguồn đơn
                            </h3>

                        </div>
                        <div class="box-content popup_info nopadding">
                            <form method="post" name="form_export" id="form_export" enctype="multipart/form-data" class="form-horizontal form-column">

                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Thời gian </label>
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
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Loại đơn</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iLoaiDon" id="iLoaiDon" class="input-block-level chosen-select" onchange="LoadLinhVucByLoaiDon(this.value)">
                                                        <option value="0">- - - Chọn tất cả </option>
                                                        <%= ViewData["opt-loaidon"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Lĩnh vực</label>
                                            <div class="controls">
                                                <div class="input-block-level" id="ip_linhvuc">
                                                    <select name="iLinhVuc" id="iLinhVuc" class="input-block-level chosen-select" onchange="LoadLinhVuc()">
                                                        <option value="0">- - - Chọn tất cả </option>
                                                        <%= ViewData["opt-linhvuc"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Nội dung đơn</label>
                                            <div class="controls">
                                                <div class="input-block-level" style="float: right" id="LoadNoiDung">
                                                    <select name="iNoiDung" id="iNoiDung" class="input-block-level chosen-select" onchange="LoadOpTinhChat()">
                                                        <option value="0">- - - Chọn tất cả </option>
                                                        <%= ViewData["opt-noidung"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Tính chất đơn</label>
                                            <div class="controls">
                                                <div class="input-block-level" id="LoadTinhChat">
                                                    <select name="iTinhChat" id="iTinhChat" class="input-block-level chosen-select">
                                                        <option value="0">- - - Chọn tất cả </option>
                                                        <%= ViewData["opt-tinhchat"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Đơn vị xử lý, giải quyết </label>
                                            <div class="controls">
                                                <div class="input-block-level" style="float: right">
                                                    <select name="iDonVi" id="iDonVi" class="input-block-level chosen-select">
                                                    
                                                        <%= ViewData["opt-donvi"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Nguồn đơn</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select class="input-block-level chosen-select" name="iNguonDon">
                                                        <option value="0">- - - Chọn tất cả</option>
                                                        <%=ViewData["opt-nguondon"] %>
                                                    </select>
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
                <div id="data"></div>
            </div>

        </div>
    </div>
    <script>
        function Xem() {
            $("#data").show().html("<div class='tcenter'><img src='/Images/ajax-loader.gif'/></div>");
            $.post("/Baocaokntc/Ajax_Xembaocao_Coquanchuyendon", $("#form_export").serialize(), function (ok) {
                $("#data").html(ok);
            });
            return false;

        }
        function TaiExel() {
            window.location = "/Baocaokntc/Coquanchuyendon_Exl/?tungay=" + $("#dTuNgay").val() + "&denngay=" + $("#dDenngay").val() + "&iLoaidon=" + $("#iLoaiDon").val() + "&iLinhVuc=" + $("#iLinhVuc").val() + "&iNoiDung=" + $("#iNoiDung").val() + "&iTinhChat=" + $("#iTinhChat").val();
        }
        function LoadLinhVucByLoaiDon(val) {
            $.post("<%=ResolveUrl("~")%>Baocaokntc/Ajax_LoadLinhVucByLoaiDon", "iLoaiDon=" + val,
                     function (data) {
                         $("#ip_linhvuc").html(data);
                         $("#iLinhVuc").chosen();
                         $("#LoadNoiDung").html("<select style='width:100%' name='iNoiDung' id='iNoiDung' class='input-medium chosen-select'><option value='0'>Chọn tất cả</option></select>");
                         $("#LoadTinhChat").html("<select style='width:100%' name='iTinhChat' id='iTinhChat' class='input-medium chosen-select'><option value='0'>Chọn tất cả</option></select>");
                         $("#iTinhChat").chosen();
                         $("#iNoiDung").chosen();
                     });
        }
        function LoadLinhVuc() {

            if ($("#iLinhVuc").val() != 0) {

                $.post("<%=ResolveUrl("~")%>Baocaokntc/Ajax_LoadLinhVucNoiDung", "iLinhVuc=" + $("#iLinhVuc").val(), function (data) {
                         $("#LoadNoiDung").html(data);
                         $("#LoadTinhChat").html("<select style='width:100%' name='iTinhChat' id='iTinhChat' class='input-medium chosen-select'><option value='0'>Chọn tất cả</option></select>");
                         $("#iTinhChat").chosen();
                         $("#iNoiDung").chosen();


                     });
                 }
             }
             function LoadOpTinhChat() {
                 if ($("#iNoiDung").val() != 0) {
                     $.post("<%=ResolveUrl("~")%>Baocaokntc/Ajax_LoadTinhChatNoiDung",
                     "iNoiDung=" + $("#iNoiDung").val(),
                     function (data) {
                         $("#LoadTinhChat").html(data);
                         $("#iTinhChat").chosen();
                     }
                 );
                 }

             }
    </script>
</asp:Content>
