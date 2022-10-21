<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Ý kiến giám sát kiến nghị
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
				    <span>Danh sách ý kiến giám sát kiến nghị</span>
			    </li>
		    </ul>
		    
	    </div>  
            
                  <% KN_KIENNGHI kn = (KN_KIENNGHI)ViewData["kiennghi"];
                            KN_CL detail = (KN_CL)ViewData["detail"];
                            %>                   
        <div class="breadcrumbs" style="padding:5px 15px">
            <p><strong>Nội dung: </strong><%=Server.HtmlEncode(kn.CNOIDUNG) %></p>
            <p><strong>Đoàn tiếp nhận: </strong><%=detail.donvi_tiepnhan %>; <strong>Đơn vị xử lý: </strong><%=detail.donvi_thamquyen %>; </p>
            <%=ViewData["traloi"] %>
        </div>      
        
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Danh sách ý kiến giám sát kiến nghị</h3>
                        <ul class="tabs"><li class="active">
                            <a <%=ViewData["btn-add"] %> onclick="ShowPopUp('id=<%=ViewData["id"] %>','/Kiennghi/Ajax_Giamsat_ykien_add/')" data-original-title="Thêm ý kiến" rel="tooltip" href="javascript:void(0)" class="add btn_f blue" ><i class="icon-plus-sign"></i></a>
                                         </li></ul>
				    </div>
				    <div class="box-content nopadding">                    
                         <table class="table table-condensed table-bordered nomargin table-striped">
                            <tr>
                                <th width="3%" nowrap>STT</th>
                                <th nowrap>Ngày làm việc</th>
                                <th nowrap>Nội dung</th>
                                <th width="3%" nowrap>Chức năng</th>
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
