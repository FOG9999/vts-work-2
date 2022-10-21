<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kiểm trùng kiến nghị
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
				    <span>Kiểm trùng kiến nghị</span>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>        
        <% KN_KIENNGHI kn = (KN_KIENNGHI)ViewData["kiennghi"]; %>    
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Kiểm trùng kiến nghị - Phạm vi: Cùng kỳ họp</h3>
				    </div>
				    <div class="box-content nopadding">              
                        <table class="table table-bordered table-condensed table-striped">
                        <thead>
                            <tr >
                                <th width="3%" class="tcenter">STT </th>
                                <th width="3%" class="tcenter" nowrap>Chọn trùng</th>
                                <th class="tcenter">Nội dung </th>
                                <th width="3%" class="tcenter" nowrap>Tiếp nhận</th>                                           
                            </tr>
                        </thead>
                        <%=ViewData["kyhop"] %> 
                        </table> 
					                  
				    </div>
			    </div>
		    </div>
	    </div>
        <div class="row-fluid" style="margin-top:20px">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Kiểm trùng kiến nghị - Phạm vi: Các kỳ họp trước</h3>
				    </div>
				    <div class="box-content nopadding">              
                        <table class="table table-bordered table-condensed table-striped">
                        <thead>
                            <tr >
                                <th width="3%" class="tcenter">STT </th>
                                <th width="3%" class="tcenter" nowrap>Chọn trùng</th>                                
                                <th class="tcenter">Nội dung </th>
                                <th width="3%" class="tcenter" nowrap>Tiếp nhận</th>  
                                <th class="tcenter" width="15%">Kỳ họp - Khóa họp </th>                                    
                            </tr>
                        </thead>
                            <%=ViewData["kytruoc"] %> 
                        </table>             
				    </div>
			    </div>
		    </div>
	    </div>
        <div style="margin-top:20px" class="tcenter">
            <form method="post" action="/Kiennghi/Kiemtrung/" onsubmit="return CheckForm();">
                <%--<input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                <span id="btn-dong" <% if((int)kn.IKIENNGHI_TRUNG==0){ Response.Write("style='display:none'"); }%>>
                    <button type="submit" class="btn btn-primary">Lưu theo dõi kiến nghị trùng</button>
                </span>--%>
                <a onclick="ShowPageLoading()" href="/Kiennghi/Moicapnhat/" class="btn btn-warning">Quay lại</a>
                <a onclick="Luutheodoi()" class="btn btn-warning">Lưu theo dõi</a>
            </form>
        </div>
    </div>  
        </div>
    <div id="hddid"></div>
    <script type="text/javascript">
        function CheckForm() {
            ShowPageLoading();
        }
        function ChonKienNghiTrung(id_trung, post, url) {
            $.post(url, post, function (data) {
                //alert(data);
                $(".chontrung").removeClass("trans_func").addClass("f-grey");
                if (data == 1) {//Chọn
                    $("#btn_" + id_trung).addClass("trans_func").removeClass("f-grey");
                    $("#btn-dong").show();
                } else {
                    $("#btn-dong").hide();
                }
                AlertAction("Cập nhật thành công!");
            });
        }

        function Luutheodoi() {
            $.post("/Kiennghi/Ajax_KienNghi_theodoi", function (data) {
                if (data == 0) {//Chọn
                    alert("Chưa chọn đơn trùng");
                } else {
                    location.href = "/Kiennghi/Theodoi_luu/"
                }
                
            });
           
        }
    </script>
</asp:Content>
