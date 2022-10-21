<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid" id="iTiepDinhKy">
    <div class="span12">
        <div class="box box-color box-bordered">
            <div class="box-title">
					    <h3><i class="icon-search"></i> Tìm kiếm vụ việc tiếp công dân</h3>
                        
				    </div>
            <div class="box-content nopadding">
                <form id="_form" name="_form" onsubmit="return CheckForm();">
                    <table class="table table-bordered form4">
                        
                        <tbody id="">
                            <tr>
                                            <td>Ngày tiếp</td>
                                            <td nowrap="">
                                                <input type="text" placeholder="từ ngày" class="input-medium datepick" name="dTuNgay" id="dTuNgay" onchange="CompareDate('dTuNgay','dDenNgay')">
                                                <input type="text" placeholder="đến ngày" class="input-medium datepick" name="dDenNgay" id="dDenNgay" onchange="CompareDate('dTuNgay','dDenNgay')">
                                            </td>
                                            <td>Đoàn đông người</td>
                                            <td>
                                                <input type="checkbox"  name="iDoan" id="iDoan" value="1"> </td>
                                        </tr>
                                        <tr>
                                            <td>Cơ quan tiếp</td>
                                            <td>
                                                <select name="iDonVi" id="iDonVi" class="input-block-level chosen-select">
                                                  
                                                    <%= ViewData["opt-donvi"] %>
                                                </select></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>Tên công dân đến</td>
                                            <td>
                                                <input type="text" class="input-block-level" name="cNguoiGui_Ten"></td>
                                            <td>Địa chỉ công dân</td>
                                            <td>
                                                <input type="text" class="input-block-level" name="cNguoiGui_DiaChi"></td>
                                        </tr>
                                        <tr>
                                            <td>Tóm tắt nội dung vụ việc</td>
                                            <td colspan="3">
                                                <input type="text" class="input-block-level" name="cNoiDung"></td>
                                        </tr>
                                        <tr>
                                            <td class="">Loại vụ việc</td>
                                            <td>
                                                <select name="iLoai" id="iLoai" class="input-block-level chosen-select">
                                                    <option value="-1">- - - Chọn tất cả</option>

                                                    <%= ViewData["opt-loaidon"] %>
                                                </select>
                                            </td>
                                            <td class="">Lĩnh vực</td>
                                            <td>
                                                <select name="iLinhVuc" id="iLinhVuc" class="input-block-level chosen-select">
                                                    <option value="-1">- - - Chọn tất cả</option>
                                                    <%=  ViewData["opt-linhvuc"] %>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Nhóm nội dung</td>
                                            <td>
                                                <select name="iNoiDung" id="iNoiDung" class="input-block-level chosen-select">
                                                    <option value="-1">- - - Chọn tất cả</option>
                                                    <%= ViewData["opt-noidung"] %>
                                                </select>
                                            </td>
                                            <td>Tính chất vụ việc</td>
                                            <td>
                                                <select name="iTinhChat"  id="iTinhChat" class="input-block-level chosen-select">
                                                    <option value="-1">- - - Chọn tất cả</option>
                                                    <%= ViewData["opt-tinhchat"] %>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Hình thức xử lý</td>
                                            <td>
                                                <select name="iHinhThuc"  id="iHinhThuc" class="input-block-level chosen-select">
                                                    <option value="-1">- - - Chọn tất cả</option>
                                                    <option value="0">Đang nghiên cứu</option>
                                                    <option value="1">Hướng dẫn xử lý</option>
                                                    <option value="2">Nhận đơn</option>
                                                    <option value="3">Chuyển xử lý</option>
                                                   
                                                </select>
                                            </td>
                                            <td>Kiểm trùng</td>
                                            <td>
                                                <input type="checkbox" id="ikiemtrung" name="ikiemtrung" value="0" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="tright">
                                                 <input type="hidden" id="iLoaiVuViec" name="iLoaiVuViec" value="-1" />
                                                <button type="submit" class="btn btn-success">Tra cứu</button>
                                            </td>
                                        </tr>
                        </tbody>
                    </table>
                </form>
            </div>
        </div>
    </div>
</div>
<br />
<script type="text/javascript">
    $("#iHinhThuc").chosen();
    $("#iTinhChat").chosen();
    $("#iLinhVuc").chosen();
    $("#iNoiDung").chosen();
    $("#iDonVi").chosen();
    $("#iLoai").chosen();

    ChangeLoaiTiepDan("Dinhky");
    function ChangeLoaiTiepDan(val) {
        $.post("/Tiepdan/Ajax_" + val + "_tracuu", "", function (data) {
            $("#loaitiepnhan").html(data);
        });
    }
    function CheckForm() {
        var tentimkiem = $("#_form").serialize();
        window.location = "/Tiepdan/Thuongxuyen/?" + tentimkiem
        //$("#ketqua_tracuu").show().html("<tr><td colspan='8' class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
        //var loaitiepdan = $("#iLoaiVuViec").val();
        //$.post("/Tiepdan/Ajax_Thuongxuyen_result_tracuu", $("#_form").serialize(), function (ok) {
        //    $("#ketqua_tracuu").html(ok);
        //});
        return false;
    }

</script>
