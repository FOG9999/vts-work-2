<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="span12">
		<div class="box box-color box-bordered">
			<div class="box-title">
				<h3><i class="icon-time"></i>Kết quả tra cứu</h3>
                <ul class="tabs">
                    <li class="active">
                        <%=ViewData["btn-add"]  %>
                    </li>
                </ul>
			</div>
			<div class="box-content nopadding">                     
                <form id="form_" onsubmit="return false;">  
					<table class="table table-bordered table-condensed nomargin">
                        <thead>
                            <tr>   
                                <th nowrap class="tcenter" width="3%">STT</th>
                                <th nowrap  width="10%" class="tcenter" >Ngày nhận</th>
                                <th nowrap  width="17%">Người gửi / Địa chỉ</th>                                        
                                <th nowrap  width="25%">Nội dung / Người tiếp</th>  
                                <th nowrap  width="35%">Hình thức xử lý / Thông tin vụ việc</th>     
                                <th nowrap  width="10%" >Kết quả trả lời</th>                                  
                            </tr>
                        </thead>
                        <tbody>                                                       
                            <%=ViewData["list"] %>
                        </tbody>                            
                    </table>      
                </form>                   
			</div>
		</div>
	</div>