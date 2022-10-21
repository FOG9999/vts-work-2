<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<tr>
    <td>Chọn kỳ họp</td>
    <td>
        <select id="iKyHop" name="iKyHop" class="chosen-select">
            <%=ViewData["opt-kyhop"] %>
        </select>
    </td>
    <td></td>
    <td></td>
</tr>
<tr>
    <td></td>
    <td colspan="3">
        <button type="submit" class="btn btn-primary">Xem báo cáo</button>
        <span onclick="Download('word')" class="btn btn-primary"><i class="icon-cloud-download"></i> Tải Word</span>
        <span onclick="Download('excel')" class="btn btn-primary"><i class="icon-cloud-download"></i> Tải Excel</span>
        <span onclick="Download('pdf')" class="btn btn-primary"><i class="icon-cloud-download"></i> Tải PDF</span>
    </td>
</tr>