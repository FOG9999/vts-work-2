<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="">

            <div class="box-content nopadding">

                <form id="form_" method="post" onsubmit="return CheckForm();">

                    <table class="table table-bordered form4">

                        <tr>
                            <td>Từ khóa</td>
                            <td colspan="3">
                                <input type="text" class="input-block-level" name="tukhoa" id="tukhoa" /></td>
                        </tr>
                        <tr>
                            <td>Ngày ban hành</td>
                            <td>
                                <input type="text" name="dTuNgay" id="dTuNgay" class="input-medium datepick" onchange="CompareDate('dTuNgay','dDenNgay')" placeholder="Từ ngày" />
                                &nbsp;&nbsp;&nbsp;
                                        <input type="text" name="dDenNgay" id="dDenNgay" class="input-medium datepick" onchange="CompareDate('dTuNgay','dDenNgay')" placeholder="Đến ngày" />
                            </td>
                            <td nowrap>Loại văn bản</td>
                            <td>
                                <select name="iLoai" class="input-block-level chosen-select">
                                    <option value="-1">- - - Chọn tất cả</option>

                                    <%=ViewData["opt-loai"] %>
                                </select>
                            </td>
                        </tr>



                        <tr>
                            <td nowrap>Đơn vị ban hành</td>
                            <td>
                                <select name="iDonvi" class="input-block-level chosen-select">

                                    <option value="-1">- - - Chọn tất cả</option>

                                    <%=ViewData["otp-donvi"] %>
                                </select>
                            </td>

                            <td nowrap>Lĩnh vực</td>
                            <td>
                                <select name="iLinhVuc" class="input-block-level chosen-select">
                                    <option value="-1">- - - Chọn tất cả</option>

                                    <%=ViewData["opt-linhvuc"] %>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Trạng thái văn bản</td>
                            <td>
                                <select name="iTrangthai" id="iTrangthai" class="input-block-level chosen-select">

                                    <%=ViewData["trangthai"] %>
                                </select>
                            </td>

                           <td class="">Kỳ họp</td>
                                         <td>  <select name="iKyhop" id="iKyhop" class="input-block-level chosen-select" >
                                                <option value="0">- - -  Chọn tất cả</option>
                                                <%=ViewData["opt-kyhop"] %>
                                            </select></td>
                        </tr>

                        <tr>
                            <td colspan="4" class="tcenter">
                                <button type="submit" class="btn btn-success" style="float: right">Tra cứu</button>

                            </td>
                        </tr>
                    </table>

                </form>
            </div>
        </div>
    </div>
</div>

<script>
    function CheckForm() {
        $("#ip_data").show().html("<tr><td colspan=4 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
        $.post("/Vanban/Ajax_Tracuu_result", $("#form_").serialize(), function (ok) {
            $("#ip_data").html(ok);
        });
        return false;
    }
</script>
