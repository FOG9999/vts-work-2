<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Đơn không đủ điều kiện xử lý
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Kntc") %>
<div id="main" class="">
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                 <li>
				    <span>Khiếu nại tố cáo   <i class="icon-angle-right"></i></span>
			    </li>
                <li>
                   <span>Đơn không đủ điều kiện xử lý</span>
			    </li>
		    </ul>
		    
	    </div> 
        <div class="function_chung">
                <form class="search" id="form_search" method="get" onsubmit="return false;">
                    <%--<%=ViewData["select-chuyenvien"] %>--%>
                    <select id="iKyHop" name="iKyHop" class="chosen-select">
                            <option value="0">Chọn khóa họp</option>
                        <%=ViewData["opt-kyhop"] %>
                    </select>
                    <input type="text" name="ip_noidung" id="ip_noidung" onkeypress="if(event.keyCode==13){TimKiem()}" value="<%=ViewData["ip_noidung"] %>" placeholder="Nội dung đơn, người gửi đơn...">
                        <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                    
                             <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kntc/Ajax_Khongdudieukien_formsearch/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                        <button type="button" title="Tìm kiếm" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>

                </form>
            </div>       
         <div id="search_place"></div>  
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-time"></i>Danh sách Đơn không đủ điều kiện xử lý</h3>
                        
				    </div>
				    <div class="box-content nopadding">                     
                        
					        <table class="table table-bordered table-striped">
                                <thead>
                                    <tr>   
                                        <th nowrap class="tcenter" width="3%">STT</th>
                                        <th nowrap width="20%" class="tcenter">Người nộp/Địa chỉ người nộp</th> 
                                        <th nowrap class="tcenter" width="5%">Ngày nhận</th>
                                        <th nowrap">Nội dung đơn</th>   
                                        <th nowrap class="tcenter" width="5%">Lưu đơn</th>
                                        <th nowrap class="tcenter" width="5%">Chức năng</th>                                        
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
    <script>
        $(window).on("pageshow", function (event) {
            HidePageLoading();
        });
        function TimKiem() {
            //location.href = "/Kntc/Khongdudieukien/?ikhoa=" + $("#iKyHop").val() + "&ip_noidung=" + $("#ip_noidung").val();
            $("#ip_data").empty().html("");
            $('#loadData').show();
            $.ajax({
                type: "post",
                url: "<%=Url.Action("Khongdudieukien", "Kntc")%>",
                  data: { ikhoa: $("#iKyHop").val(), ip_noidung: $("#ip_noidung").val(), hidNormalSearch: 1 },
                  success: function (res) {
                      if (res) {
                          $('#loadData').hide();
                          $("#ip_data").empty().html(res.data);
                      } else {
                          $('#loadData').hide();
                          alert("Lỗi tìm kiếm đơn không đủ điều kiện xử lý!");
                      }
                  }
              });
            return false;
        }
    </script>
</asp:Content>
