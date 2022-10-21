<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="control-group">
    <label class="control-label">Lịch tiếp của tổ đại biểu</label>
    <div class="controls nopadding">
        <table class="table table-bordered table-condensed" style="border-right: 1px solid #ddd; border-top: 1px solid #ddd;">
            <tr class="theader">
                <th width="20%" class="tcenter">Tổ Đại biểu</th>
                <th width='15%' class='tcenter'>Địa phương (Quận/Huyện)</th>
                <th width='15%' class='tcenter'>Địa phương (Xã/Phường/Thị trấn)</th>
                <th width="15%" class="tcenter">Ngày tiếp</th>
                <th class="tcenter">Địa chỉ</th>
                <th width="5%"></th>
            </tr>
            <%=ViewData["lichtiep"] %>
            <tr class="tfooter">
                <td colspan="4" id="lastrow"></td>
                <td colspan="5" class="tright"><span title='' onclick="PlusCP('<%=ViewData["num"] %>')" id="bt-add" rel='tooltip' data-original-title='Thêm mới lịch' class="btn btn-primary"><i class="icon-plus-sign"></i></span></td>
            </tr>
        </table>
    </div>
</div>
