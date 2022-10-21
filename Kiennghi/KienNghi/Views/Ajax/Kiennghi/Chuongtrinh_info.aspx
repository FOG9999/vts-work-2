<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"> Thông tin chi tiết chương trình tiếp xúc cử tri</i> 
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% KN_CHUONGTRINH chuongtrinh = (KN_CHUONGTRINH)ViewData["chuongtrinh"];%>
                        <div class="box-content popup_info nopadding">
                            <div>
                            <table class="table table-condensed table-bordered form4"> 
                                    
                                    <tr>
                                        <td class="b">Kì họp</td>
                                        <td ><%=ViewData["kyhop"] %></td>

                                        <td colspan="2" class="b"><% if (chuongtrinh.ITRUOCKYHOP == 1)
                                                                       {
                                                                           Response.Write("Trước kỳ họp");
                                                                       }
                                                                       else { Response.Write("Sau kỳ họp"); } %></td>
                                        
                                    </tr>
                                    <tr>
                                        <td class="b">Ngày bắt đầu</td>
                                        <td><%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(chuongtrinh.DBATDAU)) %></td>
                                        <td class="b">Ngày kết thúc</td>
                                        <td><%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(chuongtrinh.DKETTHUC)) %></td>
                                    </tr>
                                    <tr>
                                        <td class="b">Kế hoạch số</td>
                                        <td>
                                            <%=Server.HtmlEncode(chuongtrinh.CKEHOACH) %>
                                        </td>
                                        <td class="b">Cơ quan/Đoàn lập kế hoạch</td>
                                        <td><%=ViewData["donvi"] %></td>
                                    </tr>
                                     <tr>
                                         <td class="b">Nội dung  tiếp xúc</td>
                                         <td colspan="3"> 
                                             <%=Server.HtmlEncode(chuongtrinh.CNOIDUNG) %></td>
                                    </tr>
                                <% if (chuongtrinh.CFILE != "") { %>
                                    <tr>
                                        <td class="b">File kèm theo</td>
                                        <td colspan="3"><%
                                                                //string del = "<a href='javascript:void(0)' onclick=\"DeleteFile(" + kn.iChuongTrinh + ", '/Kiennghi/Ajax_Delete_chuongtrinh_file')\" class='btn btn-danger'><i class='icon-remove'></i></a>";
                                                                Response.Write("<p><a href='" + chuongtrinh.CFILE + "' class='btn btn-success'><i class='icon-download-alt'></i> File đính kèm </a></p>");
                                                             %>
                                        </td>
                                    </tr>
                                <% } %>
                                    <tr>
                                        <td class="b">Địa phương tiếp xúc</td>     
                                        <td colspan="3"><%=ViewData["diaphuong"] %> </td>                          
                                    </tr>
                                    <tr>
                                        <td class="b">Đại biểu quốc hội tiếp xúc</td> 
                                        <td colspan="3"><%=ViewData["daibieu"] %> </td>                                 
                                    </tr>
                                    <tr>
                                        <td class="b">Địa chỉ tiếp xúc</td> 
                                        <td colspan="3"><%=Server.HtmlEncode(chuongtrinh.CDIACHI) %> </td>                                 
                                    </tr>
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