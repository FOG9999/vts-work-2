<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color">
			<div class="box-title">
				<h3>
					<i class="icon-signal"> </i> Kết quả thống kê
				</h3>
            </div>
            <div class="box-content popup_info nopadding" style="overflow-y:auto; height:500px">
                    <table class="table table-bordered table-condensed">
                        <tr>
                            <td colspan="5" class="tcenter b">
                                <p>Phụ lục 2 </p>
                                <p>Văn bản pháp luật đã ban hành có nội dung liên quan tới việc tiếp thu, giải quyết</p>
                                <p>kiến nghị của cử tri gửi tới <%=ViewData["kyhop"] %></p>
                               
                            </td>
                        </tr>
                        <tr>
                            <th class="tcenter b" width="5%">STT</th>
                            <th class="tcenter b">Số hiệu văn bản</th>
                            <th class="tcenter b">Ngày ban hành</th>
                            <th class="tcenter b">trích yếu</th>
                        </tr>
                        <%=ViewData["list"] %>
                    </table>                                                    
            </div>                            
        </div>
    </div>
</div>

   