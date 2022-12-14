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
								<i class="icon-reorder"> Xác nhận kiến nghị trùng</i> 
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% kn_kiennghi k_ = (kn_kiennghi)ViewData["kn"];
                            kn_kiennghi kn = (kn_kiennghi)ViewData["kn_trung"];
                           Kiennghi k = (Kiennghi)ViewData["detail"];%>
                        <div class="box-content popup_info nopadding">
                            <table class="table table-condensed table-bordered form4"> 
                                   
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
                                        <td  class="b">Kế hoạch</td>
                                        <td >
                                            <%=Server.HtmlEncode(k.kehoach) %>
                                        </td>
                                        <td class="b">Đoàn tiếp nhận</td>
                                        <td>
                                            <%=Server.HtmlEncode(k.donvi_tiepnhan) %>
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
                                        <td  class="b" nowrap>Nội dung kiến nghị</td>
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
                                                <%  Response.Write("<p id='file_" + kn.iKienNghi + "'><a href='" + kn.cFile + "' class='btn btn-success'><i class='icon-download-alt'></i> File đính kèm </a></p>");%>
                                            </td>
                                    </tr>
                                    <% } %>
                                    <tr>
                                        <td class="b">Tình trạng
                                        </td>
                                        <td colspan="3">
                                            <%=Server.HtmlEncode(k.tinhtrang) %>
                                        </td>                                      
                                      
                                    </tr>          
                                                   
                                    
                                    </table>
                            <p class="tcenter">                                    
                                <a href="javascript:void(0)" onclick="XacNhanTrung('<%=ViewData["id"] %>');"  class="btn btn-primary">Xác nhận trùng, đóng kiến nghị</a>
                                <a href="#" onclick="HidePopup();" class="btn btn-warning">Quay lại</a>
                            </p>
                            
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>
<script type="text/javascript">
    function XacNhanTrung(id) {
        $.post('/Kiennghi/Ajax_Kiennghi_trung_dong_insert', 'id='+id, function (data) {
            if(data==1){
                AlertAction('Cập nhật thành công!');
                $(".btn-dong").hide();
                HidePopup();
            }
        });
    }
</script>