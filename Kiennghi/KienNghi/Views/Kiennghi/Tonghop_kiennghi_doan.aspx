<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Danh sách kiến nghị 
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
				    <span>Danh sách kiến nghị </span>
			    </li>
		    </ul>
		    
	    </div>     
        <% KN_TONGHOP th = (KN_TONGHOP)ViewData["tonghop"];
        TongHop_Kiennghi d = (TongHop_Kiennghi)ViewData["detail"]; %>
        <div class="breadcrumbs" style="padding:5px 15px">
            <p><strong>Nội dung Tập hợp:</strong> <%=th.CNOIDUNG %> <%=ViewData["file"] %></p>
            <p><strong>Đoàn Tập hợp:</strong> <%=d.donvi_tonghop%>; 
                <strong>Thẩm quyền:</strong> <%=d.donvi_thamquyen%>;
                <strong>Lĩnh vực:</strong> <%=d.linhvuc%>
            </p>
        </div>    
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-list"></i> 
                            Danh sách kiến nghị 
                            
					    </h3>
                        
				    </div>
				    <div class="box-content nopadding">              
                            <table class="table table-bordered table-condensed table-striped">
                                <thead>
                                    <tr >
                                        <th width="3%" class="tcenter">STT </th>
                                        <th class="tcenter">Nội dung </th>  
                                        <th class="tcenter" width="35%">Tình trạng </th>                                                
                                        <th class="tcenter" width="10%" nowrap>Chức năng</th>                                           
                                    </tr>
                                </thead>
                                <tbody id="q_data"><%=ViewData["list"] %></tbody> 
                                </table> 
					                  
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
    <script type="text/javascript">
        function ChangeDonVi_KyHop() {
            location.href = "/Kiennghi/Moicapnhat/?iDonVi=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "";
        }
        function TimKiem() {
            if ($("#q").val() == "") {
                alert("Vui lòng nhập nội dung, từ khóa!"); $("#q").focus(); return false;
            }
            $("#q_data").html("<tr><td colspan=4 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
            $.post("/Kiennghi/Ajax_search_moicapnhat_result", $("#form_search").serialize(), function (ok) {
                $("#q_data").html(ok);
            });
            return false;
        }
    </script>
</asp:Content>
