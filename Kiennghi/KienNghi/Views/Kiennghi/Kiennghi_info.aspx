<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
 Chi tiết kiến nghị cử tri
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
				    <span>Chi tiết kiến nghị</span>                   
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
					    <h3><i class="icon-tags"></i> Chi tiết kiến nghị</h3>
                        
				    </div>
				    <div class="box-content">  
                        <% KN_KIENNGHI kn = (KN_KIENNGHI)ViewData["kiennghi"];
                            KN_CL detail = (KN_CL)ViewData["detail"];
                            %>                   
                           <div class="form-horizontal  form-column">
                               <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label b">Kì họp</label>
                                        <div class="controls"><%=detail.kyhop %> - <%=detail.khoahop %> (<%=detail.truockyhop %>)</div>
                                    </div>                            
                                    <div class="control-group span6">
                                        <label class="control-label b"></label>
                                        <div class="controls"></div>
                                    </div>
                                </div>
                               <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label b">Đoàn tiếp nhận</label>
                                        <div class="controls"><%=detail.donvi_tiepnhan %></div>
                                    </div>                            
                                    <%--<div class="control-group span6">
                                        <label class="control-label b">Kế hoạch</label>
                                        <div class="controls"><%=detail.kehoach %></div>
                                    </div>--%>
                                   <div class="control-group span6">
                                        <label class="control-label b">Nguồn kiến nghị</label>
                                        <div class="controls"><%=detail.nguonkiennghi %></div>
                                    </div>    
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group span6">
                                        <label class="control-label b">Thẩm quyền giải quyết</label>
                                        <div class="controls"><%=detail.donvi_thamquyen %></div>
                                    </div>                            
                                    <div class="control-group span6">
                                        <label class="control-label b">Lĩnh vực</label>
                                        <div class="controls"><%=detail.linhvuc %></div>
                                    </div>
                                </div>
                               <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label b">Nội dung kiến nghị</label>
                                        <div class="controls"><%=Server.HtmlEncode(kn.CNOIDUNG).Replace("\n", "<br />") %></div>
                                    </div>     
                                </div>
                               <%--<div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label b">Địa chỉ</label>
                                        <div class="controls"><%=detail.diachi_daydu %></div>
                                    </div>     
                                </div>
                               <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label b">Từ khóa tìm kiếm (tags)</label>
                                        <div class="controls"><%=Server.HtmlEncode(kn.CTUKHOA) %></div>
                                    </div>     
                                </div>--%>
                               <% if (ViewData["file"].ToString() != "")
                                   { %>
                               <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label b">File đính kèm</label>
                                        <div class="controls"><%=ViewData["file"] %></div>
                                    </div>     
                                </div>
                               <% } %>
                               <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label b">Tình trạng xử lý</label>
                                        <div class="controls"><%=detail.tinhtrang %></div>
                                    </div>     
                                </div>
                               <% if (ViewData["count_traloi"].ToString() == "1")
                                   { %>
                               <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label b">Kết quả xử lý</label>
                                        <div class="controls"><%=ViewData["traloi"] %></div>
                                    </div>     
                                </div>
                               <% } %>
                               <% if (ViewData["bdn"].ToString() == "1")
                                   { %>
                               <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label b">Kết quả đánh giá</label>
                                        <div class="controls"><%=ViewData["giamsat"] %></div>
                                    </div>     
                                </div>
                               <% } %>
                               <%--<%=detail.tr_traloi%>
                               <%=detail.tr_giamsat %>--%>
                               <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label b"></label>
                                        <div class="controls"><a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a></div>
                                    </div>     
                                </div>
                               <div class="row-fluid">
                                    <div class="control-group">
                                        <label class="control-label b"></label>
                                        <div class="controls tright"><em><%=ViewData["capnhat"] %></em></div>
                                    </div>     
                                </div>
                           </div>
                                                                                                                                                                         
						                         
				    </div>
			    </div>
		    </div>
	    </div>
                  <div id="load_place"></div>
    </div>  
        </div>
    <script type="text/javascript">
        <% if (kn.ID_GOP == -1) { Response.Write("LoadKienNghiGop();"); }%>
        
        function LoadKienNghiGop() {
            
            $.post("/Kiennghi/Ajax_Load_Panel_Kiennghi_Dagop/", 'id=<%=ViewData["id"] %>', function (data) {
                $("#load_place").html(data);
            });
        }
        
    </script>
</asp:Content>
