<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
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
					    <form method="post" id="form_" class="nomargin" onsubmit="return CheckForm();" enctype="multipart/form-data">
                          
                            <table class="table table-bordered table-condensed">
                                <tbody><tr>
                                    <th colspan="6">Thông tin vụ việc</th>
                                </tr> 
                                    <tr>
                                    <td class="f-red" width="15%" >Loại hình tiếp*</td>
                                    <td width="35%">
                                       <%=d.loaihinhtiep %>
                                    </td>
                                    <td class="f-red" width="15%">Lịch tiếp*</td>
                                    <td colspan="3">
                                      <%=ViewData["Ngaytiep"] %>
                                    </td>
                                </tr>                               
                                <tr>
                                    <td class="f-red" width="15%" >Ngày nhận đơn*</td>
                                    <td width="35%">
                                       <%= ViewData["Ngaylapdon"]  %>
                                    </td>
                                    <td class="f-red" width="15%"></td>
                                    <td colspan="3"></td>
                                </tr>        
                                <tr>
                                    <td>Tên người nộp</td>
                                </tr>     
                                <% if ((int)k.IDOANDONGNGUOI == 1)
                                    { %>                   
                                <tr>
                                    <td></td>
                                    <td colspan="5">
                                        <input readonly="" type="checkbox" name="iDoanDongNguoi" class="nomargin" onchange="$('#doan').toggle();"> Đoàn đông người
                                        <span id="doan" style="margin-left:15px; display:none"><input readonly="" type="text" class="input-medium" name="iSoNguoi" value="<%=k.ISONGUOI %>" placeholder="Số người"></span>
                                    </td>
                                </tr>
                                    <% } %>
                                <tr>
                                    <td class="f-red">Địa chỉ người nộp*</td>
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
                                    <td class="f-red">Tóm tắt nội dung đơn*</td>
                                    <td colspan="5">
                                        <%=k.CNOIDUNG %>
                                    </td>
                                </tr>
                                    <tr>
                                    <td class="f">Địa điểm tiếp</td>
                                    <td colspan="5">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="">File đính kèm</td>
                                    <td colspan="5">
                                        <p id="file_24"><a href="<%= ViewData["File"] %>" class="btn btn-success"><i class="icon-download-alt"></i> File đính kèm </a></p>
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="6">Phân loại đơn</th>
                                </tr>
                                <tr>
                                    <td class="f-red">Loại đơn</td>
                                    <td>    
                                        <%=ViewData["Loaidon"]  %>                                    
                                    </td>
                                    <td class="f-red">Lĩnh vực</td>
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
                                    <th colspan="6">Vụ việc trùng / Có nội dung tương tự</th>
                                </tr>
                                   <tr>
                                        
                                        <th style="width:15%;text-align:center">Chọn vụ việc</th>
                                        <th style="width:15%">Nội dung đơn</th>
                                        <th  colspan="2">Người gửi/Địa chỉ người gửi</th>
                                        <th style="width:15%">Tình trạng đơn</th>
                                        <th style="width:15%;text-align:center">Xem đơn</th>
                                    </tr>
                               <%= ViewData["Kiemtrung"] %></table>
                            </div>
                            <br />
                            <p  style="text-align:center">
                                <input type="hidden" name="iVuViec" id="iVuViec" value="<%= ViewData["id"] %>">
                                        <input type="submit" value="Lưu" class="btn btn-success">                                       
                                        <a href="/" class="btn btn-warning">Quay lại</a>
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
            //alert($("[name=__RequestVerificationToken]").val());
            //$("[name=__RequestVerificationToken]").val("i4sQEtJtGVTUV85XK4DP69lOUjckIK4mJhlEUv-9nRH8mjN0m9_AWeKx8_h7PGmF5UfDtRIFCD7aPUk21nuj2k8Or1xmp793oid0Oc-R4kuPvjGc6krs3JOX5To4q04kOBBeeKdbPaXVt_xzGSGA9q3LSC-dU7s4HBEsa5RkAaA1");
            if ($("[name=VuViec_Trung]").val() == "") {
                alert("Vui lòng chọn vụ việc trùng"); 
                return false;
            }
           
        }
    </script>
</asp:Content>

