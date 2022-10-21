<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Lịch tiếp công dân định kỳ
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
				    <span> Lịch tiếp công dân định kỳ</span>
			    </li>
		    </ul>
		    
	    </div>  
        <div class="function_chung">
                <a onclick="ShowPopUp('','/Tiepdan/Ajax_Dinhky_add')" data-original-title="Thêm mới lịch tiếp định kỳ vụ việc" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_"  name="form_" onsubmit="return CheckForm();" >
                     <input name="search" id="search" value="" onkeypress="if(event.keyCode==13){CheckForm()}" placeholder="Địa điểm tiếp" type="text" >
                    <a onclick="CheckForm()" data-original-title="Tìm kiếm địa điểm lịch tiếp định kỳ vụ việc" rel="tooltip"  class="add btn_f blue" style="margin-top: 0px !important; "><i class="icon-search"></i></a>
                </form>
              
            </div>      
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-time"></i>Lịch tiếp công dân định kỳ</h3>
                        <ul class="tabs">
                            
                            
                        </ul>
				    </div>
				    <div class="box-content nopadding">                     
                        
					        <table class="table table-bordered table-condensed nomargin table-striped">
                                <thead>
                                    <tr>   
                                        <th nowrap class="tcenter" width="3%">STT</th>
                                        <th nowrap class="tcenter" width="10%">Ngày tiếp</th>
                                        <th  width="40%">Địa điểm tiếp</th>
                                        <th nowrap>Thống kê trong buổi tiếp</th>
                                       
                                      
                                        <th nowrap class="tcenter" width="10%">Chức năng</th>                                        
                                    </tr>
                                </thead>
                                <tbody id="ketqua_tracuu">                                                       
                                    <%=ViewData["list"] %>
                                </tbody>   
                                <%=  ViewData["phantrang"]  %>                     
                            </table>      
                                  
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script type="text/javascript">
       



       function CheckForm() {
           <%-- $("#ketqua_tracuu").show().html("<tr><td colspan='5' class='tcenter'><p ><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            var frm = $("#form_");
            var data = frm.serializeArray();
            var tentimkiem = $("#tentimkiem").val();
            $.ajax({
                type: "get",
                headers: getHeaderToken(),
                contentType: "application/json; charset=utf-8",
                url: "<%=ResolveUrl("~")%>Tiepdan/Ajax_DiaDiemTiep",
                data: data,
                success: function (ok) {
                    $("#ketqua_tracuu").html(ok);
                }
            });
            return false;--%>
           var tentimkiem = $("#search").val();
           window.location = "/Tiepdan/Dinhky/?q=" + tentimkiem;
           return false;
        }

    </script>
</asp:Content>

