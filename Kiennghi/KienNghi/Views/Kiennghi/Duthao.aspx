<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
 Chi tiết tổng hợp kiến nghị cử tri
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
				    <span>Chi tiết tổng hợp kiến nghị cử tri</span>                    
			    </li>
                
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>       
        <% KN_TONGHOP th = (KN_TONGHOP)ViewData["tonghop"];
        TongHop_Kiennghi d = (TongHop_Kiennghi)ViewData["detail"]; %>
        <div class="breadcrumbs" style="padding:5px 15px">
            <p><strong>Nội dung tổng hợp:</strong> <%=Server.HtmlEncode(th.CNOIDUNG).Replace("\r\n", "<br /><br />") %> <%=ViewData["file"] %></p>
            <p><strong>Đoàn tổng hợp:</strong> <%=d.donvi_tonghop%>; 
                <strong>Thẩm quyền:</strong> <%=d.donvi_thamquyen%>;
                <strong>Lĩnh vực:</strong> <%=d.linhvuc%>
            </p>
        </div> 
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Văn bản dự thảo tổng hợp gửi cơ quan có thẩm quyền xử lý</h3>                        
				    </div>
				    <div class="box-content nopadding">                     
                        <table class="table table-bordered table-condensed table-striped">
                            <tr>
                                <th>Văn bản</th>
                                <th width="25%" class="tcenter" nowrap>Người ký</th>
                                <th width="5%" class="tcenter" nowrap>Chức năng</th>
                            </tr>
                            <%=ViewData["list-vanban"] %>
                        </table>                           
				    </div>
			    </div>
		    </div>
	    </div>
        <div style="margin-top:20px" class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Danh sách kiến nghị đã chọn tổng hợp</h3>
                        <ul class="tabs" style="margin-top: 10px;">
                            <li class="active">
                                <a href="/Kiennghi/Download_kiennghi_bytonghop/?id=<%=Request["id"] %>"><i class="icon-download-alt"></i> Tải danh sách</a>
                            </li>
                        </ul>
				    </div>
				    <div class="box-content nopadding">                    
                         <table class="table table-condensed table-bordered nomargin table-striped">
                            <tr>
                                <th width="3%" nowrap>STT</th>
                                <th nowrap>Nội dung kiến nghị</th>
                                <th nowrap width="15%" class="tcenter">Tiếp nhận</th>
                                <th nowrap width="25%" class="tcenter">Địa chỉ</th>
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
