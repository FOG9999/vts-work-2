<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Kiểm tra vụ việc trùng
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Tiepdan") %>
 
<div id="main" class="">
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                <li>
				    <a href="#">Kiểm tra vụ việc trùng</a>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>        
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-list"></i>Danh sách vụ việc có thông tin tương tự</h3>
				    </div>
				    <div class="box-content nopadding">                     
                       
					        <table class="table table-bordered table-condensed nomargin">
                                <thead>
                                    <tr>   
                                        <th nowrap class="tcenter" width="3%">STT</th>    
                                        <th nowrap class="tcenter">Ngày tiếp</br>Người tiếp (Cơ quan)</th>
                                        <th nowrap class="tcenter" >Nội dung đơn/Người nộp(địa chỉ)</th>     
                                        <th nowrap" nowrap>Kết quả trả lời</th>  
                                        <th nowrap class="tcenter" width="5%">Chọn</th>                                        
                                    </tr>
                                </thead>
                                <tbody>                                                       
                                    <%=ViewData["list"] %>
                                </tbody>                            
                            </table>      
                                          
				    </div>
			    </div>
		    </div>
	    </div>
        <div style="margin-top:20px" class="tcenter">
          
            <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>
        </div>
    </div>
</div>
    <script type="text/javascript">
        function ChonVuViecTrung(id_trung, post, url) {
            $.post(url, post, function (data) {
                //alert(data);
                $(".chontrung").removeClass("btn-success");
                if (data == 1) {//Chọn
                    $("#btn_" + id_trung).addClass("btn-success");                    
                } 
                AlertAction("Cập nhật thành công!");
            });
        }
    </script>
</asp:Content>
