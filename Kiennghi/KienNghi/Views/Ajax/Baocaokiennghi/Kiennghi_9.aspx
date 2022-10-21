<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color">
			<div class="box-title">
				<h3>
					<i class="icon-signal"> </i> Kết quả thống kê thời gian nhận báo cáo tổng hợp ý kiến, kiến nghị của cử tri
				</h3>
            </div>
            <div class="box-content popup_info nopadding" style="overflow-y:auto; height:500px">
                    <table class="table table-bordered table-condensed">
                        <tr>
                            <td colspan="6" class="tcenter b">
                                <p>Phụ lục 9 </p>
                            </td>
                        </tr>
                        
                        <%=ViewData["list"] %>
                    </table>                                                    
            </div>                            
        </div>
    </div>
</div>

   