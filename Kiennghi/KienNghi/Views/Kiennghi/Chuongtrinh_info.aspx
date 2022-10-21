<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Chi tiết chương trình tiếp xúc cử tri
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Knct") %>
<div id="main">
              <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                <li>
                    <span>Kiến nghị cử tri <i class="icon-angle-right"></i></span>                    
                </li>
                <li>
				    <span>Chi tiết kế hoạch tiếp xúc cử tri</span>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>        
        <% KN_CHUONGTRINH kn = (KN_CHUONGTRINH)ViewData["kn"];
            ChuongtrinhCuTri ct = (ChuongtrinhCuTri)ViewData["detail"]; %>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Chi tiết kế hoạch tiếp xúc cử tri</h3>
                        
				    </div>
				    <div class="box-content">     
                        <div class="form-horizontal  form-column">
                            <div class="row-fluid">
                                <div class="control-group span6">
                                    <label class="control-label b">Kì họp</label>
                                    <div class="controls"><%=ct.kyhop %> - <%=ct.khoahop %> (<%=ct.truockyhop %>)</div>
                                </div>
                            
                                <div class="control-group span6">
                                    <label class="control-label b">Ngày bắt đầu/ kết thúc</label>
                                    <div class="controls"><%=ct.batdau %> - <%=ct.ketthuc %></div>
                                </div>
                            </div>
                            <div class="row-fluid">
                                <div class="control-group span6">
                                    <label class="control-label b">Kế hoạch số</label>
                                    <div class="controls"><%=Server.HtmlEncode(kn.CKEHOACH) %></div>
                                </div>
                            
                                <div class="control-group span6">
                                    <label class="control-label b">Đoàn lập kế hoạch</label>
                                    <div class="controls"><%=ct.doandaibieu %></div>
                                </div>
                            </div>
                           <%-- <div class="row-fluid">
                                <div class="control-group">
                                    <label class="control-label b">Nội dung tiếp xúc</label>
                                    <div class="controls"><%=Server.HtmlEncode(kn.CNOIDUNG) %></div>
                                </div>                                
                            </div>--%>
                            <%--<div class="row-fluid">
                                <div class="control-group span6">
                                    <label class="control-label b">Địa phương tiếp xúc cử tri</label>
                                    <div class="controls"><%=ct.diaphuong_view %></div>
                                </div>                                
                            
                                <div class="control-group span6">
                                    <label class="control-label b">Đại biểu tiếp xúc cử tri</label>
                                    <div class="controls"><%=ct.daibieu_view%></div>
                                </div>                                
                            </div>--%>
                            <%--<div class="row-fluid">
                                <div class="control-group">
                                    <label class="control-label b">Địa chỉ tiếp xúc cử tri</label>
                                    <div class="controls"><%=Server.HtmlEncode(kn.CDIACHI) %></div>
                                </div>                                
                            </div>--%>
                            <div class="row-fluid">
                                <div class="control-group">
                                    <label class="control-label b">Ngày ban hành</label>
                                    <div class="controls"><%=ct.ngaybanhanh %></div>
                                </div>                                
                            </div>
                            <div class="row-fluid">
                                <div class="control-group">
                                    <label class="control-label b">Lịch tiếp của tổ đại biểu</label>
                                    <div class="controls"><%=ViewData["list"] %></div>
                                </div>                                
                            </div>
                            <div class="row-fluid">
                                <div class="control-group">
                                    <label class="control-label b">File kèm theo</label>
                                    <div class="controls"><%=ct.file_view %></div>
                                </div>                                
                            </div>
                            <div class="row-fluid">
                                <div class="control-group">
                                    <label class="control-label b"></label>
                                    <div class="controls"><a onclick="ShowPageLoading()" href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a></div>
                                </div>                                
                            </div>
                        </div>                                                                                                                                                                      
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
</asp:Content>
