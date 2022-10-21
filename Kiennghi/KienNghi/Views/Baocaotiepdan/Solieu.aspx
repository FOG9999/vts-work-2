<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Báo cáo tiếp dân số liệu

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
                        <span>Báo cáo<i class="icon-angle-right"></i></span>
                    </li>
                    <li>
                        <span>Tiếp công dân số liệu và kết quả</span>
                    </li>
                </ul>
                
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3>  <i class="icon-search"></i> Báo cáo tiếp công dân số liệu và kết quả</h3>
                          
                        </div>
                        <div class="box-content" style="text-align: left;">

                            <form method="post" name="form_export" id="form_export" onsubmit="return CheckForm();" enctype="multipart/form-data" class="form-horizontal form-column">
                                
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
                                                   <select name="iLoaidon" id="iLoaidon" class="input-block-level chosen-select" onchange="LoadLinhVucByLoaiDon(this.value)"><option value="0"> - - - Chọn tất cả </option><%= ViewData["opt-loaidon"] %></select>
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
                                                 <select name="iLinhVuc" id="iLinhVuc" class="input-block-level chosen-select"><option value="0"> - - - Chọn tất cả </option><%= ViewData["opt-linhvuc"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Nội dung đơn</label>
                                            <div class="controls">
                                                <div class="input-block-level" style="float:right"  id="LoadNoiDung">
                                                    <select name="iNoiDung" id="iNoiDung" class="input-block-level chosen-select"><option value="0"> - - - Chọn tất cả </option></select>  
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid"  ">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Đơn vị tiếp nhận vụ việc</label>
                                            <div class="controls">
                                                <div class="input-block-level" >
                                                       <select name="iDonvi" id="iDonvi" class="input-block-level chosen-select"> <%=ViewData["opt-coquantiepdan"] %></select>
                                                 
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6 ">
                                        <div class="control-group">
                                         
                                            <div class="controls">
                                                <div class="input-block-level" style="float:right">
                                                    <a  class="btn btn-primary" onclick="Xem()"><i class="icon-search"></i> Xem báo cáo</a>
                                            <span onclick="TaiExel()" class="btn btn-primary "><i class="icon-cloud-download"></i> Tải Excel</span>
                                            
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
            
            <div id="search_place"></div>
        </div>
    </div>
    <script>
        function Xem() {
            $("#search_place").show().html("<div class='tcenter'><img src='/Images/ajax-loader.gif'/></div>");
            {
                $.post("/Baocaotiepdan/Ajax_Tiepcongdan_solieu", $("#form_export").serialize(), function (ok) {
                    $("#search_place").html(ok);
                });
                return false;
            }
        }
        function TaiExel() {
            window.location = "/Baocaotiepdan/Download_Baocaotiepcongdansolieutinh/?dTuNgay=" + $("#dTuNgay").val() + "&dDenNgay=" + $("#dDenngay").val() + "&iDonvi=" + $("#iDonvi").val() + "&iLoaidon=" + $("#iLoaidon").val() + "&iLinhVuc=" + $("#iLinhVuc").val()+ "&iNoiDung=" + $("#iNoiDung").val();
        }
        function LoadLinhVucByLoaiDon(val) {
            $.post("<%=ResolveUrl("~")%>Baocaotiepdan/Ajax_LoadLinhVucByLoaiDon", "iLoaiDon=" + val,
                           function (data) {
                               $("#ip_linhvuc").html(data);
                               $("#iLinhVuc").chosen();
                           });
        }
        function LoadLinhVuc() {

            if ($("#iLinhVuc").val() != 0) {

                $.post("<%=ResolveUrl("~")%>Baocaotiepdan/Ajax_LoadLinhVucNoiDung", "iLinhVuc=" + $("#iLinhVuc").val(), function (data) {
                    $("#LoadNoiDung").html(data);

                    $("#iTinhChat").chosen();
                    $("#iNoiDung").chosen();


                });
            }
        }
    </script>
</asp:Content>
