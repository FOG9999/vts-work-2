<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    :. Lỗi
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="main" class="nomargin">
    <div class="container-fluid">
        
        
        <div class="row-fluid">
            <span class="span3"></span>
            <div class="span6">
			    <div class="box ">
				    
				    <div class="box-content nopadding">
					    <div class="alert alert-error">
                            <h3><i class='icon-warning-sign'></i> Đã có lỗi xảy ra trong quá trình xử lý dữ liệu!</h3>
                             <p>Chúng tôi đã ghi nhận và khắc phục trong thời gian sớm nhất.</p>         
                            <a href="javascript:void()" onclick="history.back()" class="btn">Quay lại</a>
					    </div>
				    </div>
			    </div>
		    </div>
            <span class="span3"></span>
	    </div>
    </div>
</div>
</asp:Content>
