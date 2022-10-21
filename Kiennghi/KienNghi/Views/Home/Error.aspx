<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Lỗi quyền truy cập
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent"  runat="server">

<div id="main" class="nomargin">
    <div class="container-fluid">
        
        
        <div class="row-fluid">
            <span class="span3"></span>
            <div class="span6">
			    <div class="box ">
				    
				    <div class="box-content nopadding">
					    <div class="alert alert-error bgerror">
                            <%=ViewData["err"] %>
                            <a href="/" class="btn btnerror">Quay lại trang chủ <i class="fa fa-frown-o"></i></a>
					    </div>
				    </div>
			    </div>
		    </div>
            <span class="span3"></span>
	    </div>
    </div>
</div>
</asp:Content>
