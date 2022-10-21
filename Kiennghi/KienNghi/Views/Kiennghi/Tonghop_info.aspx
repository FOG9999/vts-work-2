<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
 Chi tiết Tập hợp kiến nghị cử tri
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
				    <span>Chi tiết Tập hợp kiến nghị cử tri</span>                    
			    </li>
                
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>        
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Chi tiết Tập hợp kiến nghị cử tri</h3>                        
				    </div>
                    <% KN_TONGHOP th = (KN_TONGHOP)ViewData["tonghop"];
                        TongHop_Kiennghi d = (TongHop_Kiennghi)ViewData["detail"]; %>
				    <div class="box-content">                     
                        <div class="form-horizontal  form-column">
                               <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label b">Kì họp</label>
                                        <div class="controls"><%=d.kyhop %> -  <%=d.khoahop %> (<%=d.truockkyhop %>)</div>
                                    </div>                            
                                    <div class="control-group span6">
                                        <label class="control-label b">Đơn vị Tập hợp</label>
                                        <div class="controls"><%=d.donvi_tonghop %></div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label b">Thẩm quyền giải quyết</label>
                                        <div class="controls"><%=d.donvi_thamquyen %></div>
                                    </div>           
                                    <% if (th.IDONVITONGHOP != 4)
                                        { %>                 
                                    <div class="control-group span6">
                                        <label class="control-label b">Lĩnh vực</label>
                                        <div class="controls"><%=d.linhvuc %></div>
                                    </div>
                                    <% } %>
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label b">Ghi chú</label>
                                        <div class="controls"><%=Server.HtmlEncode(th.CNOIDUNG).Replace("\n", "<br />") %></div>
                                    </div>                            
                                    
                                </div>
                                <%--<div class="row-fluid">
                                    <div class="control-group ">
                                        <label class="control-label b">Từ khóa tìm kiếm (tags)</label>
                                        <div class="controls"><%=Server.HtmlEncode(th.CTUKHOA) %></div>
                                    </div>    
                                </div>--%>
                                <%--<% if (ViewData["file"].ToString() != "")
                                    { %>
                                    <div class="row-fluid">
                                        <div class="control-group ">
                                            <label class="control-label b">File đính kèm</label>
                                            <div class="controls"><%=ViewData["file"] %></div>
                                        </div>    
                                    </div>
                                <% } %>--%>
                                <div class="row-fluid">
                                    <div class="control-group ">
                                        <label class="control-label b">Tình trạng</label>
                                        <div class="controls"><%=d.tinhtrang %></div>
                                    </div>    
                                </div>
                                <%=d.tr_traloi %>
                                <div class="row-fluid">
                                    <div class="control-group ">
                                        <label class="control-label b"></label>
                                        <div class="controls"><a onclick="ShowPageLoading()" href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a></div>
                                    </div>    
                                </div>
                        </div>                   
                                              
				    </div>
			    </div>
		    </div>
	    </div>
        <div style="margin-top:20px" class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Danh sách kiến nghị đã chọn Tập hợp</h3>
				    </div>
				    <div class="box-content nopadding">                    
                         <table class="table table-condensed table-bordered nomargin table-striped">
                            <tr>
                                <th width="3%" nowrap>STT</th>
                                <th nowrap>Nội dung kiến nghị</th>
                                <th nowrap width="15%" class="tcenter">Tiếp nhận</th>
                                <th nowrap width="25%" class="tcenter">Nguồn đơn</th>
                                <th width="3%" nowrap>Xem</th>
                            </tr>
                            <%=ViewData["list"] %>
                        </table>                           
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
    
</asp:Content>
