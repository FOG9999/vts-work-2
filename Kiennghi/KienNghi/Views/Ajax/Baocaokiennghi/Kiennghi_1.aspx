<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color">
			<div class="box-title">
				<h3>
					<i class="icon-signal"> </i> Kết quả báo cáo, thống kê
				</h3>
            </div>
            <div class="box-content popup_info nopadding" style="overflow-y:auto; height:500px">
                    <table class="table table-bordered table-condensed">
                        <tr>
                            <td colspan="7" class="tcenter b">
                                <p>Phụ lục 1 </p>
                                <p>Bảng tổng hợp kết quả giải quyết, trả lời  kiến nghị của cử tri </p>
                                <p>Tại <%=ViewData["kyhop"] %> của các bộ, ngành </p>
                            </td>
                        </tr>
                        <tr>
                            <th class="tcenter" width="5%" rowspan="2">STT</th>
                            <th class="tcenter" rowspan="2">Tên cơ quan, đơn vị </th>
                            <th class="tcenter" width="15%" rowspan="2">Tổng số kiến nghị</th>
                            <th class="tcenter" width="15%" rowspan="2">Tổng số KN đã trả lời</th>
                            <th class="tcenter" colspan="3">Kết quả giải quyết</th>
                        </tr>
                        <tr>
                            <th class="tcenter" width="15%">Đã giải quyết xong</th>
                            <th class="tcenter" width="15%">Đang giải quyết</th>
                            <th class="tcenter" width="15%">Giải trình, thông tin</th>
                        </tr>
                        <%=ViewData["list"] %>
                    </table>                                                    
            </div>                            
        </div>
    </div>
</div>

   