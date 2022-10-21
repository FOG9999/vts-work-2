<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
 Lịch sử xử lý
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
				    <span>Lịch sử xử lý</span>                   
			    </li>
		    </ul>
		    
	    </div>        
        <% KN_TONGHOP th = (KN_TONGHOP)ViewData["tonghop"];
        TongHop_Kiennghi d = (TongHop_Kiennghi)ViewData["detail"]; %>
        <div class="breadcrumbs" style="padding:5px 15px">
            <p><strong>Nội dung Tập hợp:</strong> <%=Server.HtmlEncode(th.CNOIDUNG).Replace("\n", "<br /><br />") %> <%=ViewData["file"] %></p>
            <p><strong>Đoàn Tập hợp:</strong> <%=d.donvi_tonghop%>; 
                <strong>Thẩm quyền:</strong> <%=d.donvi_thamquyen%>;
                <strong>Lĩnh vực:</strong> <%=d.linhvuc%>
            </p>
        </div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Lịch sử xử lý</h3>                        
				    </div>
				    <div class="box-content nopadding">                                            
                           
                        <table class="table table-condensed table-bordered table-striped">                                    
                            <tr>
                                <th class="tcenter" width="15%">Thời gian</th>
                                <th>Người xử lý</th>
                                <th>Nội dung xử lý</th>
                            </tr>
                            <%=ViewData["list"] %>
                            </table>                                                                                                                                         
						                         
				    </div>
			    </div>
		    </div>
	    </div>
        <div style="margin-top:20px" class="tcenter">
            <a onclick="ShowPageLoading()" href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>
        </div>
    </div>  
        </div>
    

</asp:Content>
