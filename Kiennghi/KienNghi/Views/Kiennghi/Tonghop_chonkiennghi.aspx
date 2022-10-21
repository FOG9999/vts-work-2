<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Chọn kiến nghị Tập hợp
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
				    <span>Chọn kiến nghị Tập hợp</span>
			    </li>
		    </ul>
		    <%--<div class="function_chung" <%=ViewData["btn-add"] %> >    
                <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">                    
                    <input type="text" name="q" id="q" value="" placeholder="Nội dung, từ khóa">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem_Conf('id=<%=ViewData["id"] %>','/Kiennghi/Ajax_Themkiennghi_tonghop/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    
                </form>
            </div>--%>
	    </div>
        <% KN_TONGHOP th = (KN_TONGHOP)ViewData["tonghop"];
        TongHop_Kiennghi d = (TongHop_Kiennghi)ViewData["detail"]; %>
        <div class="breadcrumbs" style="padding:5px 15px">
            <p><strong>Ghi chú:</strong> <%=Server.HtmlEncode(th.CNOIDUNG).Replace("\r\n", "<br />") %> <%--<%=ViewData["file"] %>--%></p>
            <p><strong>Đoàn Tập hợp:</strong> <%=d.donvi_tonghop%>; 
                <strong>Thẩm quyền:</strong> <%=d.donvi_thamquyen%>;
                <%--<strong>Lĩnh vực:</strong> <%=d.linhvuc%>--%>
            </p>
        </div>
        <div id="search_place" class="nomargin"></div>        
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Danh sách kiến nghị đã chọn Tập hợp</h3>
                        <ul class="tabs" style="margin-top: 10px;">
                            <%--<li class="active"><%=ViewData["btn-gop"] %></li>--%>
                        </ul>
				    </div>
				    <div class="box-content nopadding">                    
                         <table class="table table-condensed table-bordered table-striped">
                            <%=ViewData["list"] %>
                        </table>       
                        <p class="tcenter" style="margin-top:10px">
                            <a onclick="ShowPageLoading()" href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>
                            <%=ViewData["chuyen_choxuly"] %>
                            <%--<%=ViewData["chuyen_xuly"] %> --%>                           
                        </p>                    
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
  <script type="text/javascript">
      ShowTimKiem_Conf('id=<%=ViewData["id"] %>', '/Kiennghi/Ajax_Themkiennghi_tonghop/', 'search_place');
      function TimKiem() {
          if ($("#q").val() == "") {
              alert("Vui lòng nhập nội dung, từ khóa!"); $("#q").focus(); return false;
          }
          ShowPopUp('q=' + $("#q").val() + '&id=<%=ViewData["id"]%>', '/Kiennghi/Ajax_Themkiennghi_tonghop_search');
          return false;
      }
      function Gop_KienNghi() {
          var choices = "";
          var choices1 = [];
          $("input[name='kn_themtonghop']:checked").each(function () {
              choices += $(this).attr('value')+",";
              choices1.push($(this).attr('value'));
          });
          if (choices1.length < 2) {
              alert("Vui lòng chọn nhiều hơn một kiến nghị để Tập hợp")
              return false;
          } else {
              ShowPopUp('id=' + choices, '/Kiennghi/Ajax_Add_Kiennghi_gop');
          }          
          //alert(choices);
          return false;
      }
      function TimKienNghi() {
        ShowPopUp('' + $("#form_chon").serialize() + '', '/Kiennghi/Ajax_Themkiennghi_tonghop_search');
        return false;
        <%--$("#ketqua").show().html("<p class='tcenter'><img src='/Images/ajax-loader.gif' /></p>");
        $.ajax({
            type: "post",
            url: "<%=ResolveUrl("~")%>Kiennghi/Ajax_Themkiennghi_tonghop_search",
            data: $("#form_chon").serialize(),
            success: function (ok) {
                $("#ketqua").html(ok);
            }
        });
        
        return false;--%>
    }
  </script>
</asp:Content>
