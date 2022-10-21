<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Danh sách kiến nghị cử tri
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
				    <span>Danh sách kiến nghị cử tri</span> 
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
					    <h3><i class="icon-tags"></i> Danh sách Kiến nghị cử tri</h3>
				    </div>
				    <div class="box-content nopadding">              
                            <table class="table table-bordered table-condensed">
                                <thead>
                                    <tr >
                                        <th width="3%" class="tcenter">STT </th>
                                        <th width="10%" nowrap class="tcenter">Mã Kiến nghị </th>
                                        <th class="tcenter" width="35%">Nội dung </th>                                               
                                        <th class="tcenter" width="30%">Trả lời</th>         
                                        <th class="tcenter">Đánh giá kết quả trả lời</th>                                           
                                    </tr>
                                </thead>
                                <%=ViewData["list"] %> 
                                </table> 
					                  
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
    
</asp:Content>
