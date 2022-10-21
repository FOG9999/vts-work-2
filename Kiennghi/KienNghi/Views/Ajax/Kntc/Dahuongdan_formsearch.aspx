<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div class="row-fluid" style="margin-bottom:20px">
    <div class="span12">
        <div class="box box-color">
           <div class="box box-color box-bordered">
                <div class="box-title">
                    <h3><i class="icon-search"></i>Tìm kiếm nâng cao đơn đã hướng dẫn, trả lời</h3>
                </div>
            <div class="box-content popup_info" style="padding: 0px !important">
                <form id="form_" method="post" onsubmit="return CheckForm();">
                        <table class="table table-bordered form4">
                            <tr>
                                <td class="">Từ khóa</td>
                                <td colspan="3">
                                    <input type="text" name="cNoiDung" placeholde="Nội dung đơn, người gửi đơn..." class="input-block-level" />
                                </td>
                            </tr>
                            <tr>
                                <td>Đoàn đông người</td>
                                <td><input type="checkbox" name="iDoanDongNguoi" class="nomargin" /></td>
                                <td>Người nhập</td>
                                <td>
                                    <select class="input-block-level chosen-select" name="iNguoiNhap">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-nguoinhap"] %>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td class="">Ngày nhận đơn</td>
                                <td>
                                    <input type="text" name="dTuNgay" placeholder="từ ngày" class="input-medium datepick" />
                                    <input type="text" name="dDenNgay" placeholder="đến ngày" class="input-medium datepick" />
                                </td>
                                <td>Nguồn đơn</td>
                                <td>
                                    <select class="input-block-level" name="iNguonDon" id="iNguonDon">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-nguondon"] %>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                               <%--  <td class="">Tình thành</td>
                                <td>
                                    <select name="iDiaPhuong_0" id="iDiaPhuong_0" class="input-block-level chosen-select">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-tinh"] %>
                                    </select>
                                </td>--%>
                                <td class="">Quận/Huyện/Thành phố</td>
                                <td>
                                    <select name="iDiaPhuong_1" id="iDiaPhuong_1" class="input-block-level chosen-select"  onchange="ChangeHuyenXaTheoHuyen(this.value)">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-huyen"] %>
                                    </select>
                                </td><td class="">Xã/Phường/Thị trấn</td>
                                <td id="iDiaPhuong_2">
                                    <select name="iDiaPhuong_2" id="iDiaPhuong_02"  class="chosen-select ">
                                        <option value="0">Chọn xã/phường/thị trấn</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>Quốc tịch</td>
                                <td>
                                    <select name="iNguoiGui_QuocTich" id="iNguoiGui_QuocTich" class="input-medium">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-quoctich"] %>
                                    </select>
                                </td>
                                <td>Dân tộc</td>
                                <td>
                                    <select name="iNguoiGui_DanToc" id="iNguoiGui_DanToc" class="input-medium">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-dantoc"] %>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td class="">Loại đơn</td>
                                <td>
                                    <select name="iLoaiDon" id="iLoaiDon" class="input-block-level" onchange="LoadLinhVucByLoaiDon(this.value)">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-loaidon"] %>
                                    </select>
                                </td>
                                <td class="">Lĩnh vực</td>
                                <td id="ip_linhvuc"> 
                                    <select name="iLinhVuc" id="iLinhVuc" class="input-block-level">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-linhvuc"] %>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>Nhóm nội dung</td>
                                <td id="LoadNoiDung">
                                    <select name="iNoiDung" id="iNoiDung" class="input-block-level">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-noidung"] %>
                                    </select>
                                </td>
                                <td>Tính chất vụ việc</td>
                                <td id="LoadTinhChat">
                                    <select name="iTinhChat" id="iTinhChat" class="input-block-level">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-tinhchat"] %>
                                    </select>
                                </td>
                            </tr>
                             <tr>
                                <td class="">Khoá họp</td>
                                <td>
                                    <select id="ikhoa" name="ikhoa" class="chosen-select">
                                        <option value="0">Chọn khóa họp</option>
                                        <%=ViewData["opt-khoa"] %>
                                    </select>
                                </td>
                                
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3" class="tright"><button type="submit" class="btn btn-primary">Tra cứu</button></td>
                            </tr>
                        </table>
                        <input type="hidden" id="hidAdvancedSearch" name="hidAdvancedSearch" value="1"/>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>
<script>
    $("#iTinhChat").chosen();
    $("#iNoiDung").chosen();
    $("#iNguonDon").chosen();
    $("#iLoaiDon").chosen();
    $("#iLinhVuc").chosen();
    $("#iNguoiGui_DanToc").chosen();
    $("#iNguoiGui_QuocTich").chosen();
    $("#iDiaPhuong_0").chosen();
    $("#idonvi").chosen();
    function CheckForm() {
        var pramt = $("#form_").serialize();
        //location.href = "/Kntc/Dahuongdan/?" + pramt;
        $("#ip_data").empty().html("");
        $('#loadData').show();
        $.ajax({
            type: "post",
            url: "<%=Url.Action("Dahuongdan", "Kntc")%>",
                data: pramt,
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm đơn đã hướng dẫn!");
                    }
                }
            });
        return false;
    }
    function LoadLinhVucByLoaiDon(val) {
        $.post("<%=ResolveUrl("~")%>Baocaokntc/Ajax_LoadLinhVucByLoaiDon", "iLoaiDon=" + val,
            function (data) {
                $("#ip_linhvuc").html(data);
                $("#iLinhVuc").chosen();
            });
    }
    function ChangeHuyenXaTheoHuyen(val) {
        $.post("/Home/Ajax_Change_Opt_huyenxa", 'id=' + val, function (data) {
            $("#iDiaPhuong_2").html(data);
            $("#iDiaPhuong_02").chosen();
        });

    }
</script>