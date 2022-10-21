<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="KienNghi.Models" %>
<%@ Import Namespace="Entities.Objects" %>

<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Xem chi tiết vụ việc trong tiếp dân định kỳ
							</h3>
                            
                        </div>                        
                        <div class="box-content popup_info nopadding">
                            <% tiepdan_dinhky_vuviec v = (tiepdan_dinhky_vuviec)ViewData["vuviec"]; %>
                            <% DinhKy_VuViec d = (DinhKy_VuViec)ViewData["detail"]; %>
                                <table class="table table-condensed table-bordered form4">
                                     <tr>
                                        <td class="b" width="15%">Người tiếp</td>
                                        <td width="35%"><%=v.cNguoiTiep %></td>
                                        <td class="b" width="15%">Đoàn đông người</td>
                                        <td width="35%">
                                            <% if (v.iDoan == 1)
                                                {%>Đoàn đông người (<%=v.iDoan_Nguoi %> người)<% }
                                           else { } %>
                                        </td>
                                    </tr>                  
                                    <tr>
                                        <td class="b" width="15%">Người gửi</td>
                                        <td width="35%">
                                            <%=v.cNguoiGui_Ten %>
                                        </td>
                                        <td class="b" width="15%">Địa chỉ người gửi</td>
                                        <td width="35%">
                                            <%=v.cNguoiGui_DiaChi %>
                                        </td>
                                    </tr> 
                                                                
                                    <tr>
                                        <td class="b">Tóm tắt nội dung vụ việc</td>
                                        <td colspan="3" >
                                            <%=v.cNoiDung %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="b" colspan="2">Đơn kèm theo
                                        </td>
                                        <td colspan="2" >
                                            <%=ViewData["file_vuviec"] %>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td class="b">Phân loại vụ việc</td>
                                        <td colspan="3">
                                               <p><%=d.linhvuc %></p>       
                                            <p><%=d.loai_noidung %></p>      
                                            <p><%=d.tinhchat %></p>      
                                            <p><%=d.loaivuviec %></p>                                
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td class="b">Kết quả trả lời, giải quyết</td>
                                        <td colspan="3" >
                                            <%=d.traloi %>
                                        </td>
                                    </tr>
                                    
                                
                                </table>
                                <p class="tcenter">
                                  
                                <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span></p>
				
                            
                            
                                
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>
