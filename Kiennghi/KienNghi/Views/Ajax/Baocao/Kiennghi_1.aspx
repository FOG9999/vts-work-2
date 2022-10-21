<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color">
			<div class="box-title">
				<h3>
					<i class="icon-signal"> </i> Kết quả báo cáo, thống kê
				</h3>
            </div>
            <div class="box-content popup_info" style="padding:0px !important">
                    <table class="table table-bordered table-condensed">
                        <%=ViewData["list"] %>
                    </table>                                                    
            </div>                            
        </div>
    </div>
</div>

   