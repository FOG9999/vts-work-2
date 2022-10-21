<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="span12">
	<div class="box box-color box-bordered">
		<div class="box-title">
			<h3><i class="icon-search"></i> Kết quả tra cứu</h3>                        
		</div>
		<div class="box-content nopadding">                     
                         
				<table class="table table-bordered table-condensed table-striped">
                <thead>
                    <tr >
                        <th width="3%" class="tcenter">STT </th>                  
                        <th class="tcenter">Nội dung </th>                                           
                        <th width="15%" class="tcenter" nowrap>Tiếp nhận</th>    
                        <th width="25%" class="tcenter" nowrap>Thẩm quyền > Lĩnh vực</th>      
                        <th nowrap class="tcenter" width="5%"  nowrap>Chức năng</th>                              
                    </tr>
                </thead>
                    <%=ViewData["list"] %>
                </table>     
                                           
		</div>
	</div>
</div>