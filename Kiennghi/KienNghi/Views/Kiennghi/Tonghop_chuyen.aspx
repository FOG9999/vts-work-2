<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tập hợp kiến nghị đã chuyển xử lý
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
				    <span>Tập hợp kiến nghị đã chuyển xử lý</span>
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
					    <h3><i class="icon-tags"></i> Tập hợp kiến nghị đã chuyển xử lý</h3>
                        <ul class="tabs">
                            <li class="active">
                                <a href="javascript:void(0)" onclick="ShowPopUp('url=Tonghop_chuyen','/Kiennghi/Ajax_Tonghop_search')"><i class="icon-search"></i> Tìm kiếm</a>
                            </li>
                        </ul>
				    </div>
				    <div class="box-content nopadding">                     
                        
                            <table class="table table-bordered table-condensed table-striped">
                                <thead>
                                    <tr >
                                        <th width="3%" class="tcenter">STT </th>                    
                                        <th class="tcenter">Nội dung Tập hợp</th>
                                        <th class="tcenter" nowrap>Thẩm quyền xử lý </th>     
                                        <th width="3%" class="tcenter" nowrap>Trả lời kiến nghị </th>                                          
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
