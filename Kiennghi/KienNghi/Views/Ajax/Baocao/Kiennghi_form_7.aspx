<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<tr>
    <td>Chọn năm</td>
    <td>
        <select id="iYear" name="iYear" class="chosen-select">
            <option value="2017">2017</option>
            <option value="2016">2016</option>
            <option value="2015">2015</option>
            <option value="2014">2014</option>
            <option value="2013">2013</option>
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