<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Xem chi tiết
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <%: Html.Partial("../Shared/_Left_Tiepdan") %>
    <div id="main" class="">
         <a href="#" class="show_menu_trai">Menu trái</a>
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                <li>
				    <a href="#">Xem chi tiết vụ việc tiếp dân</a>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>
        <% TD_VUVIEC k = (TD_VUVIEC)ViewData["vuviec"]; %>
        <% tiepdan_cl d= (tiepdan_cl)ViewData["detail"]; %>
        <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Xem chi tiết vụ việc tiếp dân</h3>
                        </div>
                        <div class="box-content" style="text-align: left;">                     
                        <form  class="form-horizontal form-column" id="form_" name="form_"  onsubmit="return CheckForm()" enctype="multipart/form-data" method="post">  
					        <div class="row-fluid">
                                <div class=""></div>
                                <p class="title_form">Thông tin vụ việc</p>
                            </div>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Loại hình tiếp </label>
                                        <div class="controls">
                                        <p><%= ViewData["loaitiep"] %></p>     
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Lịch tiếp </label>
                                        <div class="controls">
                                             <p><%= ViewData["Ngaylapdon"] %></p>  
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Tên công dân đến </label>
                                        <div class="controls">
                                        <p>  <%= Server.HtmlEncode(k.CNGUOIGUI_TEN)   %> </p>   
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label ">Người tiếp </label>
                                        <div class="controls">
                                           <p><%= Server.HtmlEncode(k.CNGUOITIEP)   %></p>   
                                        </div>           
                                    </div>
                                </div>
                            </div><!-- end row-fluid-->
                           
                               
                            <div class="row-fluid">
                                <div class="control-group">
                                    <div class="span12">
                                         <label for="textfield" class="control-label ">Đoàn đông người </label>
                                            <div class="controls">
                                             <p>
                                                      <% if ((int)k.IDOANDONGNGUOI == 1)
                                                        { %>                   
                              
                                                      <%=k.ISONGUOI %>
                                                       
                                                          <% } %>
                                                      <% else
                                                        { %>                   
                              
                                                         0
                                                       
                                                          <% } %>

                                              </p>
                                                 
                                            </div>
                                        
                                    </div>
                                </div>
                            </div><!-- end row-fluid-->
                            <div class="row-fluid" >
                                <div class="control-group">
                                      <div class="span12">
                                    <label for="textfield" class="control-label ">Địa chỉ người nộp </label>
                                    <div class="controls">
                                   <p> <%= Server.HtmlEncode(ViewData["thongtindiachi"].ToString()) %></p>  
                                    </div>
                                          </div>
                                </div>
                            </div><!-- end row-fluid-->
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Quốc tịch</label>
                                        <div class="controls">
                                            <div class="input-block-level">
                                                <p>  <%=ViewData["Quoctich"] %>       </p>    
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Dân tộc</label>
                                        <div class="controls">
                                            <div class="input-block-level">
                                             <p>    <%=ViewData["Dantoc"] %>          </p>        

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div><!-- end row-->
                            <div class="row-fluid" >
                                <div class="span12">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Tóm tắt nội dung vụ việc </label>
                                    <div class="controls">
                                    <p>  <%= Server.HtmlEncode(k.CNOIDUNG) %></p>  
                                  
                                            
                                    </div>
                                </div>
                                </div>
                            </div>
                            <div class="row-fluid" >
                                <div class="span12">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">File đính kèm </label>
                                    <div class="controls">
                                      
                                  <%= ViewData["File"] %>
                                            
                                    </div>
                                </div>
                                </div>
                            </div><!-- end row-->
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Loại vụ việc </label>
                                    <div class="controls">
                                   <p> <%=ViewData["Loaidon"]  %>          </p>     
                                    </div>
                                </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Lĩnh vực </label>
                                    <div class="controls">
                                   <p>  <%=ViewData["Linhvuc"] %>             </p>       
                                    </div>
                                </div>
                                </div>
                                </div>
                             <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Nhóm nội dung </label>
                                    <div class="controls" id="LoadNoiDung">
                                        <p>     <%= ViewData["Nhomnoidung"]  %>            </p>
                                  
                                    </div>
                                </div>
                                </div>
                                 <div class="span6">
                                     <div class="control-group">
                                    <label for="textfield" class="control-label">Tính chất vụ việc</label>
                                    <div class="controls" id="LoadTinhChat">
                                        <p>   <%= ViewData["tinhchat"] %>          </p>
                                               
                                    </div>
                                </div>
                                 </div>
                            </div>
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                    <label for="textfield" class="control-label ">Số lượng đơn trùng</label>
                                    <div class="controls" id="">
                                        <p>     <%= ViewData["soluongtrung"]  %>            </p>
                                  
                                    </div>
                                </div>
                                </div>
                                 <div class="span6">
                                     
                                 </div>
                            </div>
                               <%--<div class="row-fluid" style="padding:0px;">
                            <table class="table table-bordered table-condensed" style="width:100% !important;<%=ViewData["hienthi"]%>">
                                         <tbody><tr><th style="width:3%;text-align:center"> STT </th>
                                               <th style="width:15%;text-align:center"> Ngày ban hành </th>
                                             <th> Nội dung trả lời </th>
                                             <th style="width:15%;text-align:center"> Chức năng </th>
                                             </tr>
                                             <%=ViewData["traloi"] %>
                                         </tbody>

                                                    </table>
                                   </div>--%>
                            
                            <!-- end row-->
                            
                            <div class="row-fluid" style="border-bottom:1px solid #fff">
                                <div class="form-actions" >
                                 
                                    <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning" onclick="ShowPageLoading()">Quay lại</a>
                                </div>
                            </div>
                        </form>  
				    </div>
                    </div>
                </div>
            </div>


        
    </div>
</div>
    
</asp:Content>
