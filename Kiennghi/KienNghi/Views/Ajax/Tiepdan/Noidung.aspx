<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Nội dung xử lý
							</h3>
                            
                        </div>
                            <div class="box-content popup_info nopadding">

                           

                                <table class="table table-bordered table-condensed">
                                <tbody>
                                    <%=ViewData["list"] %>
                                    <tr>
                                    <td class="f" width="15%" >Tên cơ quan</td>
                                    <td colspan="4">
                                       <%=ViewData["tencoquan"] %>
                                    </td>
                                    
                                </tr>                               
                                <tr>
                                    <td class="f" width="15%" >Số công văn</td>
                                    <td width="35%" colspan="">
                                      <%=ViewData["Socongvan"] %>
                                    </td>
                                    <td class="f" width="15%" >Ngày ban hành</td>
                                    <td width="35%" colspan="2">
                                       <%=ViewData["NgayBanHanh"] %>
                                    </td>
                                   
                                </tr>        
                                <tr>
                                    <td class="f-">Nội dung vụ việc</td>
                                    <td colspan="5">
                                        <%=ViewData["NoiDung"] %>
                                    </td>
                                </tr>
                                    
                                <tr>
                                    <td class="">File đính kèm</td>
                                    <td colspan="5">
                                       <%= ViewData["File"] %>
                                    </td>
                                </tr>
                                
                                
                                
                                </tbody>
                                </table>                                                                                                             
						                             
                  
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>
