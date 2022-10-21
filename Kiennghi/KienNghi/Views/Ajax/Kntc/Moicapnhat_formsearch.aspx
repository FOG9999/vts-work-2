<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid" style="margin-bottom: 20px">
    <div class="span12">
        <div class="box box-color">
            <div class="box box-color box-bordered">
                <div class="box-title">
                    <h3><i class="icon-search"></i>Tìm kiếm nâng cao đơn mới cập nhật</h3>
                </div>
                <div class="box-content popup_info" style="padding: 0px !important">
                    <form id="form_" method="post" onsubmit="return CheckForm();">
                        <table class="table table-bordered form4">
                            <tr>
                                <td class="">Từ khóa</td>
                                <td colspan="3">
                                    <input type="text" placeholder="Nội dung đơn, người gửi đơn..." name="cNoiDung" class="input-block-level" />
                                </td>
                            </tr>
                            <tr>

                                <td>Đoàn đông người</td>
                                <td>
                                    <input type="checkbox" name="iDoanDongNguoi" class="nomargin" />
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
                                <td class="">Tình thành</td>
                                <td colspan="3">
                                    <select name="iDiaPhuong_0" id="iDiaPhuong_0" class="input-block-level chosen-select">
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-tinh"] %>
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
                                <td colspan="3" class="tright">
                                    <button type="submit" class="btn btn-primary">Tra cứu</button></td>
                            </tr>
                        </table>
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
    function CheckForm() {
        var pramt = $("#form_").serialize();
        //window.location = "/Kntc/Moicapnhat/?" + $("#form_").serialize();
        //alert(window.location);
        location.href = "/Kntc/Moicapnhat/?" + pramt;
        return false;
    }
</script>
