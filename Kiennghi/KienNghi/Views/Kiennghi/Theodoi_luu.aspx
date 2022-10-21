<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kiến nghị trùng, lưu theo dõi
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
				    <span>Kiến nghị trùng, lưu theo dõi</span>
			    </li>
		    </ul>
		    
	    </div> 
                  <div class="function_chung">
                <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">
                    
                    <input type="text" name="q" id="q" value="" placeholder="Nội dung, từ khóa">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kiennghi/Ajax_search_theodoi_luu/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm nâng cao" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
                        
                </form>
            </div>      <div id="search_place"></div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-list"></i> 
                            Kiến nghị trùng, lưu theo dõi
                           
					    </h3>
                        <div class="pull-right box-title-header">
                             <form id="form_header">
                               
                                
                                <%--<% if (ViewData["dbqh"].ToString() == "0") { %>
                                 <select class="chosen-select" name="iDoan" id="iDoan" onchange="ChangeDonVi_KyHop();">
                                        <%=ViewData["opt-coquan"] %>
                                 </select>
                                <% } %>--%>
                                <select class="chosen-select" name="iDonViXuLy_Parent" id="iDonViXuLy_Parent" onchange="DoiThamQuyenDonVi(this.value);">
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
                                    <tr >
                                        <th width="3%" class="tcenter">STT </th>
                                        <th class="tcenter">Nội dung </th>     
                                        <th width="15%" class="tcenter">Đơn vị thẩm quyền </th>                                            
                                        <th class="tcenter" width="10%" nowrap>Chức năng</th>                                           
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
            location.href = "/Kiennghi/Theodoi_luu/?" + pramt;
            //location.href = "/Kiennghi/Moicapnhat/?iDoan=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "";
        }
        function TimKiem() {
            var pramt = $("#form_header").serialize();
            //location.href = "/Kiennghi/Theodoi_luu/?q=" + $("#q").val() + "&" + pramt;
            $("#ip_data").empty().html("");
            $('#loadData').show();
            $.ajax({
                type: "post",
                url: "<%=Url.Action("Theodoi_luu", "Kiennghi")%>",
                data: { q: $("#q").val(), hidNormalSearch: 1 },
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm kiến nghị trùng, lưu theo dõi!");
                    }
                }
            });
            return false;
        }

        function DoiThamQuyenDonVi(val) {
            $.post("/Kiennghi/Ajax_Get_ThamQuyen_DonVi", '&val=' + val, function (data) {
                $("#TrungUong").show();
                $("#iDonViXuLy").html('<select name="iDonViXuLy" id="iDonViXuLy" class="chosen-select">' + data + '</select>');
                $("#iDonViXuLy").trigger("liszt:updated");
                $("#iDonViXuLy").chosen();
            });
        }
    </script>
</asp:Content>
