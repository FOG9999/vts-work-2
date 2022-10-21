<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kiểm trùng vụ việc tiếp dân
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
				    <a href="#">Kiểm trùng vụ việc tiếp dân</a>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>
        <% TD_VUVIEC k = (TD_VUVIEC)ViewData["vuviec"]; %>
        <% tiepdan_cl d= (tiepdan_cl)ViewData["detail"]; %>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i>Kiểm trùng vụ việc tiếp dân</h3>
				    </div>
				    <div class="box-content nopadding">
					    <form method="post" id="form_" name="form_" class="nomargin" onsubmit="return CheckForm();" enctype="multipart/form-data">
                          
                            <table class="table table-bordered table-condensed">
                                <tbody><tr>
                                    <th colspan="6">Thông tin vụ việc</th>
                                </tr> 
                                    <tr>
                                    <td class="f" width="15%" >Loại hình tiếp</td>
                                    <td width="35%">
                                       <%=d.loaihinhtiep %>
                                    </td>
                                    <td class="f" width="15%">Lịch tiếp</td>
                                    <td colspan="3">
                                      <%=ViewData["Ngaylapdon"]  %>
                                    </td>
                                </tr>                               
                                        
                                <tr>
                                    <td>Tên công dân dến</td>
                                    <td><%=k.CNGUOIGUI_TEN   %></td>
                                     <td>Người tiếp</td>
                                    <td><%=k.CNGUOITIEP   %></td>
                                </tr>     
                                <% if ((int)k.IDOANDONGNGUOI == 1)
                                    { %>                   
                                <tr>
                                    <td></td>
                                    <td colspan="5">
                                       Đoàn đông người : <%=k.ISONGUOI %>
                                        <span id="doan" style="margin-left:15px; display:none"><input readonly="" type="text" class="input-medium" name="iSoNguoi" value="<%=k.ISONGUOI %>" placeholder="Số người"></span>
                                    </td>
                                </tr>
                                    <% } %>
                                <tr>
                                    <td class="f">Địa chỉ công dân đến</td>
                                    <td colspan="5">
                                      <%=k.CNGUOIGUI_DIACHI %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Quốc tịch</td>

                                    <td>      
                                        <%=ViewData["Quoctich"] %>                                  
                                    </td>
                                    <td>Dân tộc</td>
                                    <td colspan="2">     
                                          <%=ViewData["Dantoc"] %>                                       
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="f-">Tóm tắt nội dung vụ việc</td>
                                    <td colspan="5">
                                        <%=k.CNOIDUNG %>
                                    </td>
                                </tr>
                                    
                                <tr>
                                    <td class="">File đính kèm</td>
                                    <td colspan="5">
                                       <%= ViewData["File"] %>
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="6">Phân loại đơn</th>
                                </tr>
                                <tr>
                                    <td class="f-">Loại đơn</td>
                                    <td>    
                                        <%=ViewData["Loaidon"]  %>                                    
                                    </td>
                                    <td class="f">Lĩnh vực</td>
                                    <td colspan="3">    
                                        <%=ViewData["Linhvuc"] %>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>Nhóm nội dung</td>
                                    <td>  
                                        <%= ViewData["Nhomnoidung"]  %>                                      
                                    </td>
                                    <td>Tính chất vụ việc</td>
                                    <td colspan="3">  
                                        <%= ViewData["tinhchat"] %>                                      
                                    </td>
                                </tr>
                                </tbody>
                                </table>
                            <div style="overflow:auto; max-height:350px">
                                <table class="table table-bordered table-condensed">
                                    <tr>
                                    <th colspan="7">Vụ việc trùng / Có nội dung tương tự</th>
                                </tr>
                                   <tr>
                                        <th style="width:5%;text-align:center">STT</th>
                                        <th style="width:15%;text-align:center">Chọn vụ việc</th>
                                        <th style="width:15%">Nội dung đơn</th>
                                        <th  colspan="2">Người gửi/Địa chỉ người gửi</th>
                                        <th style="width:15%">Tình trạng đơn</th>
                                        <th style="width:15%;text-align:center">Xem đơn</th>
                                    </tr>
                                        <%= ViewData["Kiemtrung"] %>
                                </table>
                            </div>
                            <br />
                            <p  style="text-align:center">
                                <input type="hidden" name="iVuViec" id="iVuViec" value="<%= ViewData["id"] %>">
                                 
                                <%=ViewData["Chucnang"] %>    
                                <span id="btn-dong" <% if ((int)k.IVUVIECTRUNG == 0 ) { Response.Write("style='display:none'"); }%>>
                                            <input class="btn btn-primary"  value="Lưu theo dõi vụ việc trùng" type="submit" />
                                        </span>
                                <a href="<%=  ViewData["load"] %>" class="btn btn-warning">Quay lại</a>
                                                                 
                                        
                            </p>
					    </form>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
    <script type="text/javascript">
        function CheckForm() {
            
        }
        function CheckTrung() {
            $.ajax({
                type: "post",
                url: "<%=ResolveUrl("~")%>Tiepdan/Ajax_Luukiemtrung",
                data: $("#form_").serialize(),
                success: function (ok) {
                    if (ok == 1) {
                        
                        AlertAction("Cập nhật thành công.");
                    }
                   
                }
            });
        }
        function CheckKiemTrung() {
          
        }
        function ChonDonTrung(id_trung, post, url) {
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
     
    </script>
</asp:Content>

