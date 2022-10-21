<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="KienNghi.Models" %>
<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"> Thông tin chi tiết tổng hợp kiến nghị</i> 
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% kn_tonghop kn = (kn_tonghop)ViewData["tonghop"];
                           TongHop_Kiennghi k = (TongHop_Kiennghi)ViewData["detail"];%>
                        <div class="box-content popup_info nopadding">
                            <div class="scroll" style="height:500px">
                            <table class="table table-condensed table-bordered form4 nomargin"> 
                                   
                                    <tr>
                                        <td  class="b">Kì họp</td>
                                        <td>
                                            <%=Server.HtmlEncode(k.khoahop) %> - <strong><%=Server.HtmlEncode(k.kyhop) %></strong>

                                        </td>
                                        <td></td>
                                        <td >
                                        <% if (kn.iTruocKyHop == 1) { Response.Write("Trước kỳ họp"); } else { Response.Write("Sau kỳ họp"); } %>    
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="b">Nơi tổng hợp</td>
                                        <td>
                                            <%=Server.HtmlEncode(k.donvi_tonghop) %>
                                        </td>
                                        <td  class="b">Kế hoạch</td>
                                        <td >
                                            <%=Server.HtmlEncode(k.kehoach) %>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td  class="b">Thẩm quyền xử lý</td>
                                        <td >
                                            <%=Server.HtmlEncode(k.donvi_thamquyen) %>
                                        </td>
                                        <td   class="b">Lĩnh vực</td>
                                        <td >
                                            <%=Server.HtmlEncode(k.linhvuc) %>
                                        </td>
                                    </tr>
                                    
                                     <tr>
                                        <td  class="b" nowrap>Nội dung tổng hợp</td>
                                         <td colspan="3"> <%=Server.HtmlEncode(kn.cNoiDung) %></td>
                                    </tr>
                                    <tr>
                                        <td class="b">Từ khóa tìm kiếm
                                        </td>
                                        <td colspan="3">
                                            <%=Server.HtmlEncode(kn.cTuKhoa) %>
                                        </td>                                      
                                      
                                    </tr>                                  
                                    
                                    <% if (kn.cFile != "") {%>
                                        <tr><td>File đính kèm</td>
                                        <td colspan="3">      
                                                <%  Response.Write("<p><a href='" + kn.cFile + "' class='btn btn-success'><i class='icon-download-alt'></i> File đính kèm </a></p>");%>
                                            </td>
                                    </tr>
                                    <% } %>
                                     <tr>
                                        <td class="b">Tình trạng
                                        </td>
                                        <td colspan="3">
                                            <%=k.tinhtrang %>
                                        </td>                                     
                                    </tr>      
                                    <% if (kn.iTinhTrang >= 3)
                                        { %>
                                    <%=k.tr_traloi %>
                                    <% } %>                    
                                    </table>
                                    
                                    <table class="table table-condensed table-bordered"> 
                                    <thead>
                                        <tr >
                                            <th width="3%" class="tcenter">STT </th>
                                            <th width="10%" nowrap class="tcenter">Mã Kiến nghị </th>
                                            <th class="tcenter">Nội dung </th>                                      
                                        </tr>
                                    </thead>
                                    <tbody id="list_kiennghi">
                                        <input type="hidden" id="kiennghi" name="kiennghi" value="<%=ViewData["kiennghi"] %>" />
                                        <%=ViewData["list"] %>
                                    </tbody>
                                </table>
                                </div>
                            <p class="tcenter">                                    
                                        <a href="#" onclick="HidePopup();" class="btn btn-warning">Quay lại</a>
                            </p>
                            
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>