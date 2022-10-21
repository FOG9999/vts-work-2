<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color">
			<div class="box-title">
				<h3 >
					<i class="icon-reorder"></i> Danh sách kiến nghị đã gộp
				</h3>
            </div>
                        
            <div class="box-content nopadding">                
                <table class="table table-condensed table-bordered nomargin table-striped">
                    <tr>
                        <th width="5%" class="tcenter">STT</th>
                        <th class="tcenter">Nội dung</th>
                        <th width="15%" class="tcenter">Tiếp nhận</th>
                        <th width="5%" class="tcenter">Xem</th>
                    </tr>
                    <%=ViewData["list"] %>
                </table>                
            </div>                            
        </div>
    </div>
</div>