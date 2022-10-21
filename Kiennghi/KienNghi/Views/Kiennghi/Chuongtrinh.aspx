<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kế hoạch tiếp xúc cử tri
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
				   <span>Kế hoạch tiếp xúc cử tri</span> 
			    </li>
		    </ul>
		    
	    </div>   
        <div class="function_chung">
            <a onclick="ShowPageLoading()" data-original-title="Thêm mới" <%=ViewData["btn-add"] %> rel="tooltip" class="add btn_f blue" href="/Kiennghi/Themmoichuongtrinh"><i class="icon-plus-sign"></i></a>
            <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">                    
                <input type="text" name="q" id="q" value="" placeholder="Kế hoạch, nội dung ...">
                <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kiennghi/Ajax_search_chuongtrinh/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                <button type="button" title="Tìm kiếm nâng cao" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
            </form>
        </div>     
        <div id="search_place"></div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-list"></i> 
                            Kế hoạch tiếp xúc cử tri                                
					    </h3>
                        <div class="pull-right box-title-header">  
                            <form id="form_header">
                                <% if (ViewData["is_dbqh"].ToString() == "0")
                                    { %>
                                <select class="chosen-select" name="iDoan" id="iDoan" onchange="ChangeDonVi_KyHop();">
                                    <%=ViewData["opt-coquan"] %>
                                </select>
                                <% } %>
                            </form>
                        </div>
				    </div>
				    <div class="box-content nopadding">            
                        <table class="table table-bordered table-condensed  table-striped">
                            <thead>
                                <tr >
                                    <th width="3%" style="text-align:center">STT </th>                                    
                                    <th >Kế hoạch / Nội dung tiếp xúc</th>
                                    <th class="tcenter" >Thời gian </th>                                       
                                    <th style="text-align:center;width:5%" nowrap> Chức năng</th>
                                </tr>
                            </thead>
                            <tbody id="ip_data">
                                <%=ViewData["list"] %>
                                <%=ViewData["phantrang"] %> 
                            </tbody>
                        </table>     
                        <div style="display: none;" id="loadData" class="tcenter"><img src='/Images/ajax-loader.gif' /></div>
                                
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
</div>
    <script type="text/javascript">
        
        function ChangeDonVi_KyHop() {
            var pramt = $("#form_header").serialize();
            location.href = "/Kiennghi/Chuongtrinh/?" + pramt;
        }
        function TimKiem() {
            //if ($("#q").val() == "") {
            //    alert("Vui lòng nhập kế hoạch, nội dung cần tìm!"); $("#q").focus(); return false;
            //}
            var pramt = $("#form_header").serialize();
            //location.href = "/Kiennghi/Chuongtrinh/?q=" + $("#q").val() + "&" + + pramt;
            $("#ip_data").empty().html("");
            $('#loadData').show();
            $.ajax({
                type: "post",
                url: "<%=Url.Action("Chuongtrinh", "Kiennghi")%>",
                data: { q: $("#q").val(), hidNormalSearch: 1 },
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm chương trình tiếp xúc cử tri!");
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
