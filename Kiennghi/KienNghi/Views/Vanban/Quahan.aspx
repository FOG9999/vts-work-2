<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Văn bản hết hiệu lực
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <%: Html.Partial("../Shared/_Left_Vanban") %>
<div id="main" class="">
     <a href="#" class="show_menu_trai">Menu trái</a>
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>">Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                   <li>
				    <span> Văn bản công bố   <i class="icon-angle-right"></i>  </span>
			    </li>
                <li>
				    <span>Văn bản hết hiệu lực</span>
			    </li>
		    </ul>
		    
	    </div>
        <div class="function_chung">

               
                <form class="search" id="form_search" method="post" onsubmit="return false;" >
                       
            
                 <input type="text" name="ip_noidung" id="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" value="" placeholder="Nội dung văn bản ...">
                        <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                      
                     <button type="button" title="Tìm kiếm" onclick="ShowTimKiem_Conf('type= -1','/Vanban/Ajax_Vanban_search/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                        <button type="button" title="Tìm kiếm" onclick="HideTimKiem('search_place')" class="btn_f blue"  id="hidebut" style="display:none" ><i class="icon-zoom-out"></i></button>
                </form>
              
            </div>
         <div id="search_place"></div> 
        <div class="row-fluid" >
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i>Danh sách văn bản hết hiệu lực</h3>
                        
				    </div>
				    <div class="box-content nopadding">
					    <table class="table table-bordered table-striped">
                            <thead>
                                <tr>                          
                                     <th nowrap width="3%">STT</th>   
                                    <th nowrap width="67%" >Thông tin văn bản</th>   
                                         
                                    <th class="tcenter"  width="15%" nowrap>File</th>                                                    
                                   <th></th>
                                </tr>
                            </thead>
                            <tbody id="ip_data">                          
                               <%=ViewData["list"] %>
                            </tbody>
                              <%= ViewData["phantrang"] %>
                        </table>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    
    <script type="text/javascript">
        function TimKiem() {
            //$("#ip_data").show().html("<tr><td colspan=4  class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            //$.post("/Vanban/Ajax_Vanbanhethan_Timkiem", $("#form_search").serialize(), function (ok) {
            //    $("#ip_data").html(ok);
            //});
            //return false;
            var tentimkiem = $("#ip_noidung").val();
            window.location = "/Vanban/Quahan/?q=" + tentimkiem;
            return false;
        }

        <%-- function TimKiem() {
            $("#ketqua_tracuu").show().html("<tr><td colspan='4'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            var frm = $("#fromsearch");
            var data = frm.serializeArray();
            var tentimkiem = $("#tentimkiem").val();
            $.ajax({
                type: "post",
                headers: getHeaderToken(),
                contentType: "application/json; charset=utf-8",
                url: "<%=ResolveUrl("~")%>Vanban/Ajax_VanbanDuyet_Timkiem",
                data: data,
                success: function (ok) {
                    $("#ketqua_tracuu").html(ok);
                }
            });
            return false;
        }--%>

    </script>
</asp:Content>
