<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Tiếp công dân thường xuyên
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
				    <span>Vụ việc tiếp nhận qua tiếp dân thường xuyên</span>
			    </li>
		    </ul>
		    
	    </div>  
        <div class="function_chung">

                  <a  data-original-title="Thêm mới vụ việc thường xuyên" rel="tooltip" href="/Tiepdan/Themmoi/?sel=thuongxuyen" class="add btn_f blue" onchange="$('#iTiepDinhKy').toggle()"><i class="icon-plus-sign"></i></a>
                 <form class="search" id="fromsearch" name="fromsearch" onsubmit="return CheckForm();">

                        <input name="search" id="search" value="" onkeypress="if(event.keyCode==13){CheckForm()}" placeholder="Nội dung vụ việc" type="text">
                        <a onclick="CheckForm()" data-original-title="Tìm kiếm nội dung vụ việc thường xuyên" rel="tooltip" class="add btn_f blue" style="margin-top: 0px !important;"><i class="icon-search"></i></a>
                      
                        <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Tiepdan/Ajax_Thuongxuyen_search/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                        <button type="button" title="Tìm kiếm" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
                    </form>
            </div>
           <div id="search_place"></div>            
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-time"></i>Vụ việc tiếp nhận qua tiếp dân thường xuyên</h3>
                        <ul class="tabs">
                            
                            
                        </ul>
				    </div>
				    <div class="box-content nopadding">                     
                       
					        <form id="form_" onsubmit="return false;">  
					        <table class="table table-bordered table-condensed nomargin table-striped">
                                <thead>
                                    <tr>   
                                         <th nowrap class="tcenter" width="3%">STT</th>
                                         <th nowrap style="width:10%;text-align:center" >Ngày tiếp</th>
                                           <th nowrap style="width:30%">Người gửi / Địa chỉ</th>   
                                        <th nowrap  style="width:30%">Nội dung vụ việc / Người tiếp</th>
                                     
                                           <th nowrap style="width:15%">Hình thức giám sát</th>                                    
                                        <th nowrap style="text-align:center">Hình thức xử lý</th>  
                                         <th nowrap class="tcenter" width="8%">Mẫu phiếu</th>      
                                        <th nowrap class="tcenter" width="8%">Chức năng</th>                                         
                                    </tr>
                                </thead>
                                <tbody id="ketqua_tracuu">     
                                    <%= ViewData["ketqua"] %>                                                  
                                    <%=ViewData["list"] %>
                                </tbody> 
                                <%=ViewData["phantrang"] %>                           
                            </table>      
                        </form>      
                                          
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script type="text/javascript">

        function CheckForm() {
            <%-- $("#ketqua_tracuu").show().html("<tr><td colspan='8' class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            var frm = $("#fromsearch");
            var data = frm.serializeArray();
            var tentimkiem = $("#tentimkiem").val();
            $.ajax({
                type: "get",
                headers: getHeaderToken(),
                contentType: "application/json; charset=utf-8",
                url: "<%=ResolveUrl("~")%>Tiepdan/Ajax_TRACUUTHUONGXUYEN",
                data: data,
                success: function (ok) {
                    $("#ketqua_tracuu").html(ok);
                }
            });
            return false;--%>
            var tentimkiem = $("#search").val();
            window.location = "/Tiepdan/Thuongxuyen/?q=" + tentimkiem;
            return false;
        }

    </script>
    <script type="text/javascript">
      
        function TimKiem(val) {
            $("#ketqua_tracuu").show().html("<tr><td colspan='8' class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            $.post("/Tiepdan/Ajax_TDVuViec_search/" + val + "", function (ok) {
                $("#ketqua_tracuu").html(ok);
            });
            return false;
        }


    </script>
</asp:Content>
