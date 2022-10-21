<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="control-group">
    <label class="control-label">Lịch tiếp của tổ đại biểu</label>
    <div class="controls nopadding">
        <table class="table table-bordered table-condensed" style="border-right:1px solid #ddd;border-top:1px solid #ddd;">
            <tr class="db">
                <th width="20%" nowrap class="tcenter">Đại biểu</th>
                <th width="25%" class="tcenter">Địa phương (Quận; Huyện)</th>
                <th width="15%" class="tcenter">Ngày tiếp</th>
                <th class="tcenter">Địa chỉ</th>
                <th width="5%"></th>
            </tr>
            <%--<tr id="db_1" class="db">
                <td class="tcenter">
                    <select class="input-block-level" name="iDaiBieu"><option value="0">Chọn đại biểu</option><%=ViewData["opt-daibieu"] %></select>
                </td>
                <td class="tcenter">
                    <select class="input-block-level" name="iDiaPhuong"><option value="0">Chọn quận/huyện</option><%=ViewData["opt-diaphuong"] %></select>
                </td>
                <td class="tcenter">
                    <input type="text" class="datepick input-block-level" name="dNgayTiep" />
                </td>
                <td class="tcenter">
                    <input type="text" class="input-block-level" name="cDiaChi" />
                </td>
                <td class="tcenter">
                                                        
                </td>
            </tr>--%>
            <tr>
                <td colspan="5" class="tright"><span title='' onclick="PlusCP(0)" id="bt-add" rel='tooltip' data-original-title='Thêm mới lịch' class="btn btn-primary"><i class="icon-plus-sign"></i></span></td>
            </tr>
        </table>

    </div>
</div>