<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tra cứu tiếp công dân định kỳ
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <%: Html.Partial("../Shared/_Left_Tiepdan") %>
<div id="main">
     <a href="#" class="show_menu_trai">Menu trái</a>
              <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                   <li>
				    <span> Tiếp công dân   <i class="icon-angle-right"></i>  </span>
			    </li>
                <li>
				    <span>Tra cứu tiếp công dân định kỳ</span>
			    </li>
		    </ul>
		    <div class="function_chung">
                <a href="/Tiepdan/Themmoi"  data-original-title="Thêm mới" rel="tooltip"  class="add btn_f blue"><i class="icon-plus"></i></a>
                <form class="search" id="form_search" method="get">

                       <input type="text" autocomplete="off" name="timkiem" id="timkiem" placeholder="">
                    <button type="submit" class="btn_f blue"><i class="icon-search"></i></button>
                </form>
            </div>
	    </div>        
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-search"></i> Tra cứu tiếp công dân định kỳ</h3>
                    </div>
				    <div class="box-content nopadding">                     
                        <form id="form_" onsubmit="return CheckForm();">     
                             
                            <table class="table table-bordered form4" >
                                <tr>
                                    <td>Ngày tiếp</td>
                                    <td nowrap><input type="text" placeholder="từ ngày" class="input-medium datepick" name="dTuNgay" />
                                                <input type="text" placeholder="đến ngày" class="input-medium datepick" name="dDenNgay" />
                                    </td>                                
                                    <td>Đoàn đông người</td>
                                    <td><input type="checkbox" name="iDoan" /></td>
                                </tr>                                    
                                <tr>
                                    <td>Phân loại vụ việc</td>
                                    <td colspan="3">
                                        <div class="actions"><%=ViewData["phanloai"] %></div>
                                        </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="tcenter">
                                        <input type="hidden" name="phanloai" />
                                        <button type="submit" class="btn btn-success"> Tra cứu</button>        
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
            var phanloai = "";
            $('input[name="loai"]:checked').each(function () {
                phanloai += this.value + ",";
            });
            $("input[name='phanloai']").val(phanloai);
            $.ajax({
                type: "GET",
                headers: getHeaderToken(),
                contentType: "application/json; charset=utf-8",
                url: "<%=ResolveUrl("~")%>Tiepdan/Ajax_Dinhky_result",
                data: data,
                success: function (ok) {
                    $("#ketqua_tracuu").html(ok);
                }
            });
            return false;
        }
       
    </script>
    
</asp:Content>
