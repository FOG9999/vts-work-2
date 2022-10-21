<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Danh sách tập hợp kiến nghị đã Import
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
				    <span>Danh sách tập hợp kiến nghị đã Import</span>
			    </li>
		    </ul>
		    
	    </div>       
                 
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered ">
				    <div class="box-title">
					    <h3><i class="icon-save"></i> Danh sách tập hợp kiến nghị đã Import</h3>
				    </div>
				    <div class="box-content nopadding" style="overflow: auto; width: auto; height: 500px;">                    
                        <table class="table table-bordered table-condensed table-striped">
                            <thead>
                                <tr >
                                    <th width="3%" class="tcenter">STT </th> 
                                    <th nowrap class="tcenter">Nội dung tập hợp / kiến nghị</th>   
                                    <th class="tcenter" width="15%" nowrap>Thẩm quyền</th>                                     
                                </tr>
                            </thead>
                            <tbody id="q_data">
                                <%=ViewData["list"] %>
                            </tbody>
                        </table> 					                              
				    </div>
                    <div style="margin-top:20px">
                        <a onclick="ShowPageLoading()" href="/Kiennghi/Import_kiennghi/?id=<%=Request["id"] %>" class="btn btn-warning">Quay lại</a>
                    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
</asp:Content>
