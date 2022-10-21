<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Trả lời xử lý vụ việc
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <%: Html.Partial("../Shared/_Left_Tiepdan") %>
<div id="main" class="">
     <a href="#" class="show_menu_trai">Menu trái</a>
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                  <li>
				    <span> Tiếp công dân   <i class="icon-angle-right"></i>  </span>
			    </li>
                <li>
				    <span>Trả lời vụ việc chuyển xử lý</span>
			    </li>

		    </ul>
            
		    
	    </div> 
        <div class="function_chung">

                 <a onclick="ShowPopUp('id=<%= ViewData["id_vuviec"] %>','/Tiepdan/Ajax_TraLoi_ChuyenXuLyVuViec')" data-original-title="Thêm mới trả lời vụ việc chuyển xử lý" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
            </div>       
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-time"></i>Danh sách trả lời vụ việc chuyển xử lý</h3>
                        
				    </div>
				    <div class="box-content nopadding">                     
                        
					        <table class="table table-bordered table-condensed nomargin">
                                <thead>
                                    <tr>   
                                        <th nowrap class="tcenter" width="3%">STT</th>
                                        <th nowrap class="tcenter" width="10%">Ngày trả lời</th>
                                        <th  width="">Nội dung</th>
                                          <th class="tcenter" width="10%" >File trả lời</th>
                                        <th nowrap class="tcenter" width="10%">Chức năng</th>                                        
                                    </tr>
                                </thead>
                                <tbody id="ketqua_tracuu">                                                       
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
