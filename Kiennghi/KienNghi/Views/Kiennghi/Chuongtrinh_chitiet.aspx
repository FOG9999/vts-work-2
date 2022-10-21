<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Chi tiết lịch tiếp xúc cử tri
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
				    <span>Chi tiết lịch tiếp xúc cử tri</span>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>        
        <% KN_CHUONGTRINH kn = (KN_CHUONGTRINH)ViewData["kn"];
            ChuongtrinhCuTri ct = (ChuongtrinhCuTri)ViewData["detail"]; %>
        <div class="breadcrumbs" style="padding:5px 15px">
            
            <p><strong>Đoàn lập kế hoạch:</strong> <%=ct.doandaibieu%>; <strong>Kế hoạch số:</strong> <%=Server.HtmlEncode(kn.CKEHOACH).Replace("\r\n", "<br />") %> <%=ViewData["file"] %></p>
            <p><strong>Bắt đầu từ ngày:</strong> <%=ct.batdau%> <strong>đến ngày</strong> <%=ct.ketthuc %>;</p>
        </div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Lịch tiếp xúc cử tri của đoàn</h3>
                        <ul class="tabs" style="margin-top: 10px;">
                            <li class="active"><%=ViewData["btn-add"] %></li>
                        </ul>
				    </div>
				    <div class="box-content nopadding">                    
                         <table class="table table-condensed table-bordered table-striped">
                            <tr>
                                <th class="tcenter" width="5%">STT</th><th class="tcenter">Tên đại biểu</th><th class="tcenter">Địa phương (Quận/Huyện)</th><th class="tcenter">Ngày tiếp</th><th class="tcenter">Địa chỉ</th><th class="tcenter" width="5%" nowrap>Chức năng</th>
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
