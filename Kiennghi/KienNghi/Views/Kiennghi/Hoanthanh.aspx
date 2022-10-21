<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kiến nghị đã có trả lời
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
				    <span>Kiến nghị đã có trả lời</span>
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
					    <h3><i class="icon-tags"></i> Lọc kết quả</h3>
				    </div>
				    <div class="box-content nopadding">                     
                        <form id="form_" onsubmit="return CheckForm();">     
                             
                            <table class="table table-bordered form4" >
                                <tr>
                                    <td nowrap>Tình trạng</td>
                                    <td>
                                    <select name="iTinhTrang" class="input-block-level">
                                        <option value="-1">- - - Chọn tất cả</option>
                                        <option value="4">Đã có trả lời, đóng kiến nghị</option>
                                        <option value="6">Chuyển theo dõi ở kỳ họp sau</option>
                                        <option value="5">Kiến nghị trùng, đóng kiến nghị</option>
                                        </select>
                                    </td>
                                    
                                    <td>Chọn kỳ họp</td>
                                    <td><p>
                                        <select class="input-block-level" onchange="ChangeKyHop(this.value)" name="iKyHop">
                                            <option value="-1">Chọn tất cả</option>
                                            <%=ViewData["kyhop"] %>
                                        </select>
                                        </p>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="4" onclick="$('#nangcao').toggle();" > <i class="icon-circle-arrow-right"></i> Tra cứu nâng cao</th>
                                </tr>
                                <tbody id="nangcao" style="display:none">
                                     <tr>
                                        <td>Kế hoạch</td>
                                        <td><select name="iChuongTrinh" id="iChuongTrinh" class="input-block-level">
                                                <option value="-1"> - - - Chọn tất cả</option>
                                                <option value="0">Không nằm trong chương trình, kế hoạch tiếp xúc cử tri</option>
                                                <%=ViewData["kehoach"] %>
                                            </select></td>                                
                                        <td nowrap>Đoàn tiếp nhận</td>
                                        <td>
                                        <select name="iDonViTiepNhan" id="iDonViTiepNhan" class="input-block-level">
                                                <%=ViewData["opt-doan"] %></select>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td nowrap>Thẩm quyền xử lý</td>
                                        <td>
                                        <select name="iThamQuyenDonVi" class="input-block-level">

                                            <option value="-1">- - - Chọn tất cả</option>
                                                <option value="0">Chưa xác định</option>
                                                <%=ViewData["thamquyen"] %>
                                            </select>
                                        </td>                                
                                        <td nowrap>Lĩnh vực</td>
                                        <td>
                                        <select name="iLinhVuc" class="input-block-level">
                                            <option value="-1">- - - Chọn tất cả</option>
                                                    <option value="0">Chưa xác định</option>
                                                <%=ViewData["opt-linhvuc"] %>
                                            </select>
                                        </td>
                                    </tr>
                                </tbody>
                                <tr>
                                    <td colspan="4" class="tcenter">
                                        <button type="submit" class="btn btn-success"> Tra cứu</button>                                      
                                <a href="#" onclick="HidePopup();" class="btn btn-warning">Quay lại</a>
                                    </td>
                                </tr>
                            </table>                            
                            
                        </form>
                                         
				    </div>
			    </div>
		    </div>
	    </div>
        <div class="row-fluid" style="margin-top:20px;display:none;" id="ketqua_tracuu">
            
	    </div>
    </div>  
        </div>
    <script type="text/javascript">        
        function CheckForm() {
            $("#ketqua_tracuu").show().html("<p class='tcenter'><img src='/Images/ajax-loader.gif'/></p>");
            var frm = $("#form_");
            var data = frm.serializeArray();

            $.ajax({
                type: "GET",
                headers: getHeaderToken(),
                contentType: "application/json; charset=utf-8",
                url: "<%=ResolveUrl("~")%>Kiennghi/Ajax_Hoanthanh_result",
                data: data,
                success: function (ok) {
                    $("#ketqua_tracuu").html(ok);
                }
            });
            return false;
        }       
    </script>
</asp:Content>
