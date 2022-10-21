<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<tr id="db_<%=ViewData["num"] %>" class="db">
    <td class="tcenter">
        <select class="input-block-level" name="iDiaPhuong" onchange="ChangeDiaPhuongHDND(this.value, <%=ViewData["num"] %>)">
            <option value="0">Chọn quận/huyện</option>
            <%=ViewData["opt-diaphuong"] %>
        </select>
    </td>
    <td class="tcenter">
        <select class="input-block-level" name="iDaiBieu" id="iDaiBieuRowList">
            <option value="0">Chọn đại biểu</option>
            <%=ViewData["opt-daibieu"] %>
        </select>
    </td>
    <td class="tcenter">
        <input type="text" class="datepick input-block-level" name="dNgayTiep" />

    </td>
    <td class="tcenter">
        <input type="text" class="input-block-level" name="cDiaChi" /></td>
    <td class="tcenter">
        <span title="Xóa" onclick="$('#db_<%=ViewData["num"] %>').remove()" class="btn btn-danger">
            <i class="icon-remove"></i></span>
    </td>
</tr>
