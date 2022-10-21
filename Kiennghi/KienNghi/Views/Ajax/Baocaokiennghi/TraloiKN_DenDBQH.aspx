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
                        <%--<tr>
                            <td colspan="7" class="tcenter b">
                                <p>Tổng hợp trả lời KNTC của Bộ Ngành </p>
                                <p>BẢNG THEO DÕI TRẢ LỜI Ý KIẾN, KIẾN NGHỊ CỬ TRI CỦA BỘ, NGÀNH <%=ViewData["kyhop"] %></p>
                            </td>
                        </tr>--%>
                        <%=ViewData["list"] %>
                    </table>                                                    
            </div>                            
        </div>
    </div>
</div>

   