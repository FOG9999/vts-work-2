<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
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
								<i class="icon-reorder"></i> Thông tin tài khoản
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% TaiKhoan detail = (TaiKhoan)ViewData["detail"];
                           USERS tk = (USERS)ViewData["taikhoan"];
                             %>
                        <div class="box-content popup_info">                            
                            <div class="scroll" style="height:450px">
                                <table class="table table-bordered table-condensed form4">
                                    <tr>
                                        <td class="b">Tên đăng nhập</td>
                                        <td><%=Server.HtmlEncode(tk.CUSERNAME) %> </td>
                                        <td class="b">Tên người dùng</td>
                                        <td><%=Server.HtmlEncode(tk.CTEN) %></td>
                                    </tr>
                                    <tr>
                                        <td class="b">Cơ quan/Đơn vị</td>
                                        <td><%=detail.donvi %></td>
                                        <td class="b">Phòng ban (Chức vụ)</td>
                                        <td><%=detail.phongban %> (<%=detail.chucvu %>)</td>
                                    </tr>
                                   <tr>
                                        <td class="b">Điện thoại</td>
                                        <td><%=Server.HtmlEncode(tk.CSDT)%></td>
                                        <td class="b">Email</td>
                                        <td><%=Server.HtmlEncode(tk.CEMAIL)%> </td>
                                    </tr>       
                                    <tr>
                                        <td class="b">Nhóm chức năng</td>
                                        <td colspan="3" class="b"><%=ViewData["group"] %></td>
                                    </tr>
                                    <tr>
                                        <td class="b">Chức năng</td>
                                        <td colspan="3" >
                                            <div class="actions"><%=ViewData["action"] %></div></td>
                                    </tr>                          
                                </table>
                            </div>
                            <p class="tcenter"><span onclick="HidePopup();" class="btn btn-warning">Quay lại</span></p>
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>