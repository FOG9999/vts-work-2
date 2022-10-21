<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kiến nghị cử tri mới cập nhật
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
				    <span>Kiến nghị cử tri mới cập nhật</span>
			    </li>
		    </ul>
		    
	    </div>  
                  <div class="function_chung">
                <a <%=ViewData["bt-add-tonghop"] %> onclick="ShowPageLoading()"  data-original-title="Thêm mới tổng hợp" rel="tooltip" href="/Kiennghi/Chontonghop/" class="add btn_f blue" ><i class="icon-list-alt"></i></a>
                <a <%=ViewData["bt-add"] %> onclick="ShowPageLoading()"  data-original-title="Thêm mới kiến nghị" rel="tooltip" href="/Kiennghi/Themmoi/" class="add btn_f blue" ><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">                    
                    <input type="text" name="q" id="q" value="" placeholder="Nội dung, từ khóa">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kiennghi/Ajax_search_moicapnhat/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm nâng cao" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
                </form>
            </div>      <div id="search_place"></div> 
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-list"></i> 
                            Kiến nghị mới cập nhật
                            
					    </h3>
                        <div class="pull-right box-title-header">
                             <select class="chosen-select" name="iKyHop" id="iKyHop" onchange="ChangeDonVi_KyHop();">
                                    <%=ViewData["opt-khoa-kyhop"] %>
                                    </select>
                             <select class="chosen-select" name="iDonVi" id="iDonVi" onchange="ChangeDonVi_KyHop();">
                                    <%=ViewData["opt-coquan"] %>
                                       </select>
                            </div>
				    </div>
				    <div class="box-content nopadding">              
                        <table class="table table-bordered table-condensed table-striped">
                            <thead>
                                <tr >
                                    <th width="3%" class="tcenter">STT </th>
                                    <th width="3%" class="tcenter">Chọn </th>
                                    <th class="tcenter">Nội dung </th>                                                 
                                    <th class="tcenter" width="10%" nowrap>Chức năng</th>                                           
                                </tr>
                            </thead>
                            <tbody id="q_data"><%=ViewData["list"] %></tbody> 
                        </table>   
                        <%=ViewData["phantrang"] %>   
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
    <script type="text/javascript">
        function ThemVaoTongHop(id) {
            $.post("/Kiennghi/Ajax_Themkiennghivaotonghop", "id=" + id, function (ok) {
                //$("#q_data").html(ok);
            });
        }
        function ChangeDonVi_KyHop() {
            location.href = "/Kiennghi/Moicapnhat/?iDoan=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "";
        }
        function TimKiem() {
            location.href = "/Kiennghi/Moicapnhat/?iDoan=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "&q=" + $("#q").val();
            //if ($("#q").val() == "") {
            //    alert("Vui lòng nhập nội dung, từ khóa!"); $("#q").focus(); return false;
            //}
            //$("#q_data").html("<tr><td colspan=4 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            //$.post("/Kiennghi/Ajax_search_moicapnhat_result", "q=" + $("#q").val() + "&iKyHop=" + $("#iKyHop").val(), function (ok) {
            //    $("#q_data").html(ok);
            //});
            return false;
        }
    </script>
</asp:Content>
