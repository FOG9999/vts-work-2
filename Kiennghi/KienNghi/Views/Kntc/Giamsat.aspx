<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Kế hoạch giám sát
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Kntc") %>
<div id="main" class="">
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                
                <li>
				    <span>Khiếu nại tố cáo   <i class="icon-angle-right"></i></span>
			    </li>
                <li>
				     <span>Kế hoạch giám sát</span>
			    </li>
		    </ul>
		  
	    </div>  
          <div class="function_chung">
                <%if(Convert.ToInt32(ViewData["bandannguyen"]) ==1){ %>
                <a <%=ViewData["bt_add"] %> onclick="ShowPopUp('id=<%=ViewData["id"] %>','/Kntc/Ajax_Giamsat_add')" data-original-title="Thêm mới" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
           <%} %>
                 </div>      
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-time"></i>Kế hoạch giám sát</h3>
				    </div>
				    <div class="box-content nopadding">                     
                        
					        <table class="table table-bordered table-striped nomargin">
                                <thead>
                                    <tr>   
                                        <th nowrap class="tcenter" width="3%">STT</th>
                                        <th nowrap class="tcenter" width="12%">Đoàn giám sát</th>
                                        <th nowrap class="tcenter" width="12%">Kế hoạch</th>
                                        <th nowrap class="tcenter">Chuyên đề</th>
                                        <th nowrap">Ý kiến đoàn</th>   
                                        <th nowrap class="tcenter" width="5%">Chức năng</th>                                        
                                    </tr>
                                </thead>
                                <tbody id="ip_data">                                                       
                                    <%=ViewData["list"] %>
                                </tbody>                            
                            </table>      
                                
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
</asp:Content>
