<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tập hợp kiến nghị Ban Dân nguyện đã chuyển xử lý
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
				    <span>Tập hợp kiến nghị Ban Dân nguyện đã chuyển xử lý</span>
			    </li>
		    </ul>
		    
	    </div> 
                  <div class="function_chung">
                <a data-original-title="Tìm kiếm" rel="tooltip" style="display:none" class="add btn_f blue" href="javascript:void(0)" onclick="ShowTimKiem('/Kiennghi/Ajax_Kiennghi_moicapnhat_search/','search_place')"><i class="icon-search"></i></a>
                <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">
                    
                    <input type="text" name="q" id="q" value="" placeholder="Nội dung, từ khóa">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kiennghi/Ajax_search_tonghop_bandannguyen_dachuyen/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm nâng cao" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
                        
                </form>
            </div>       <div id="search_place"></div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Tập hợp kiến nghị Ban Dân nguyện đã chuyển xử lý
                            
					    </h3>
                        <div class="pull-right box-title-header">
                         <form id="form_header">
                             <select class="chosen-select" name="iKyHop" id="iKyHop" onchange="ChangeDonVi_KyHop();">
                                <%=ViewData["opt-khoa-kyhop"] %>
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
                            <table class="table table-bordered table-condensed">
                                <thead>
                                    <tr >
                                        <th width="3%" class="tcenter">STT </th>                  
                                        <th class="">Nội dung Tập hợp</th>
                                        <th width="3%" class="tcenter" nowrap>Chức năng</th>                                          
                                    </tr>
                                </thead>
                                <tbody id="q_data"><%=ViewData["list"] %></tbody>  <%=ViewData["phantrang"] %>
                                
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
           location.href = "/Kiennghi/Tonghop_bandannguyen_dachuyen/?" + pramt;
           //location.href = "/Kiennghi/Moicapnhat/?iDoan=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "";
       }
        function TimKiem() {
            //if ($("#q").val() == "") {
            //    alert("Vui lòng  nhập nội dung, từ khóa!"); $("#q").focus(); return false;
            //}
            var pramt = $("#form_header").serialize();
            location.href = "/Kiennghi/Tonghop_bandannguyen_dachuyen/?q=" + $("#q").val() + "&" + pramt;
            //$("#q_data").html("<tr><td colspan=5 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            //$.post("/Kiennghi/Ajax_search_tonghop_bandannguyen_dachuyen_result", "q=" + $("#q").val() + "&iKyHop=" + $("#iKyHop").val(), function (ok) {
            //    $("#q_data").html(ok);
            //});
            return false;
        }
    </script>
</asp:Content>
