<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="span12">
	<div class="box box-color box-bordered">
		<div class="box-title">
			<h3><i class="icon-search"></i> Kết quả tra cứu</h3>    
            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>                    
		</div>
		<div class="box-content nopadding">                     
                         
				<table class="table table-bordered table-condensed">
                <thead>
                    <tr >
                        <th width="3%" class="tcenter">STT </th>                                        
                        <th width="10%" nowrap class="tcenter">Mã kiến nghị </th>
                        <th class="tcenter">Nội dung </th>                                              
                        <th width="20%" class="tcenter" nowrap>Tình trạng</th>                                    
                    </tr>
                </thead>
                    <%=ViewData["list"] %>
                </table>     
                                           
		</div>
	</div>
</div>