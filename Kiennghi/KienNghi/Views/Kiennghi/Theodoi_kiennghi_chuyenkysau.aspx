<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kiến nghị tồn qua nhiều kỳ họp
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
				    <span> Kiến nghị tồn qua nhiều kỳ họp</span>
			    </li>
		    </ul>
		    
	    </div>     
                  <div class="function_chung">
                <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">                    
                    <input type="text" name="q" id="q" value="" placeholder="Nội dung, từ khóa">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kiennghi/Ajax_search_theodoi_kiennghi_chuyenkysau/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm nâng cao" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
                </form>
            </div>   
        <div id="search_place"></div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-list"></i> 
                            Kiến nghị tồn qua nhiều kỳ họp
					    </h3>
                        <div class="pull-right box-title-header">
                             <form id="form_header">
                                 <select class="chosen-select" name="iKyHop" id="iKyHop" onchange="ChangeDonVi_KyHop();">
                                        <%=ViewData["opt-khoa-kyhop"] %>
                                        </select>
                                
                                
                                 <select class="chosen-select" name="iDoan" id="iDoan" onchange="ChangeDonVi_KyHop();">
                                        <% if (ViewData["dbqh"].ToString() == "0") { %><option value="0">Chọn tất cả</option> <% } %>
                                        <%=ViewData["opt-coquan"] %>
                                 </select>
                               
                                <select class="chosen-select" name="iDonViXuLy_Parent" id="iDonViXuLy_Parent" onchange="ChangeDonVi_KyHop();">
                                    <%=ViewData["opt-thamquyen"] %>
                                 </select>
                                <select class="chosen-select" name="iDonViXuLy" id="iDonViXuLy" onchange="ChangeDonVi_KyHop();">
                                    <%=ViewData["opt-thamquyen-xuly"] %>
                                 </select>                               
                            </form>
                            </div>
				    </div>
				    <div class="box-content nopadding">              
                            <table class="table table-bordered table-condensed table-striped">
                                <thead>
                                    <tr>
                                        <th width="3%" class="tcenter">STT </th>
                                        <th class="tcenter">Nội dung </th>      
                                        <th class="tcenter" width="40%" >Kết quả xử lý</th>                                                
                                        <th class="tcenter" width="10%" nowrap>Chức năng</th>                                           
                                    </tr>
                                </thead>
                                <tbody id="q_data"><%=ViewData["list"] %></tbody>
                                <%=ViewData["phantrang"] %> 
                           </table> 
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
    <script type="text/javascript">
        function ChangeDonVi_KyHop() {
            var pramt = $("#form_header").serialize();
            //window.location = "/Kiennghi/Moicapnhat/?" + $("#form_").serialize();
            //alert(window.location);
            location.href = "/Kiennghi/Theodoi_kiennghi_chuyenkysau/?" + pramt;
            //location.href = "/Kiennghi/Moicapnhat/?iDoan=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "";
        }
        function TimKiem() {
            var pramt = $("#form_header").serialize();
            location.href = "/Kiennghi/Theodoi_kiennghi_chuyenkysau/?q=" + $("#q").val() + "&" + pramt;
            return false;
        }
    </script>
</asp:Content>
