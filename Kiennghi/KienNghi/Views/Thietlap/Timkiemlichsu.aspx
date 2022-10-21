<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Tìm kiếm lịch sử
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <%: Html.Partial("../Shared/_Left_Thietlap") %>
    <!-- Daterangepicker -->
	
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
				    <span> Thiết lập   <i class="icon-angle-right"></i>  </span>
                   
			    </li>
                <li>
				    <span>Tìm kiếm lịch sử xử lý</span>                   
			    </li>
		    </ul>
	    </div>   
            
                  <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered table-striped">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Lịch sử người dùng</h3>

                        </div>
                        <div class="box-content nopadding">
                            
                           <form id="_form" name="_form" onsubmit="return CheckForm();" method="post">
                                
                                <table class="table table-bordered form4">
                                    
                                    <tr>
                                        <td>Tên người dùng</td>
                                        <td colspan="3">
                                            <input type="text" class="input-block-level" name="ten" id="ten" /></td>
                                    </tr>
                                    <tr>
                                        <td>Thời gian</td>
                                        <td>
                                            <input type="text" name="dTuNgay" id="dTuNgay" class="input-medium datepick"  onchange="CompareDate2('dTuNgay','dDenNgay')" value="<%=  ViewData["TuNgay"] %>" placeholder="Từ ngày" />
                                            &nbsp;&nbsp;&nbsp;
                                        <input type="text" name="dDenNgay" id="dDenNgay" class="input-medium datepick" onchange="CompareDate2('dTuNgay','dDenNgay')"  value="<%=  ViewData["DenNgay"]%>" placeholder="Đến ngày" />
                                        </td>
                                        <td nowrap>Đơn vị</td>
                                        <td>
                                            <select name="iDonVi" id="iDonVi" class="input-block-level chosen-select">
                                                <option value="-1">- - - Chọn tất cả</option>
                                                
                                                <%= ViewData["opt-coquantiepdan"] %>
                                            </select>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>Nội dung hoạt động</td>
                                        <td colspan="3">
                                            <input type="text" class="input-block-level" name="tukhoa" id="tukhoa" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="tcenter">
                                            <button type="submit" class="btn btn-success" style="float:right">Tra cứu</button>
                                         
                                        </td>
                                    </tr>
                                </table>

                            </form>
                        </div>
                    </div>
                </div>
            </div>  
                  <br />    
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Danh sách hoạt động xử lý</h3>                        
				    </div>
				    <div class="box-content nopadding">                                            
                        <table class="table table-condensed table-bordered">                                    
                            <tr>
                                <th class="tcenter" style="width:5%">STT</th>
                                <th class="tcenter">Thời gian</th>
                                <th>Username/Người xử lý</th>
                                <th>Nội dung xử lý</th>
                            </tr>
                            <tbody id="ketqua_tracuu">
                                <%=ViewData["list"] %>
                            </tbody>
                           <%= ViewData["phantrang"]  %>
                            </table>                                                                                                                                                              
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
    <script>
        function CompareDate2(from, to) {


            if ($("#" + from).val() != "" && $("#" + to).val() != "") {
                //var date_from = $("#" + from).val();
                //var date_to = $("#" + to).val();
                var date_from = moment($("#" + from).val(), 'DD/MM/YYYY');
                var date_to = moment($("#" + to).val(), 'DD/MM/YYYY');
                //alert(date_from + " - " + date_to);
                if (date_to >= date_from) {

                    $.post("/Thietlap/Ajax_Sosanh2ngaytimkiem", $("#_form").serialize(), function (ok) {
                        // alert(ok);
                        if (ok == 1) {
                            return true;
                        }
                        else {
                            alert("Vui lòng chọn ngày kết thúc không lớn hơn ngày bắt đầu 7 ngày !");
                            $("#dTuNgay").val(ok);
                            return false;
                        }

                    });

                }
                else {
                    alert("Vui lòng chọn ngày kết thúc lớn hơn ngày bắt đầu !");
                    $("#" + to).focus();
                    $("#" + to).val("");
                    return false;
                }


            }
        }
        function CheckForm() {
            if ($("#dTuNgay").val() != "" && $("#dDenNgay").val() != "") {
              
                //$("#ketqua_tracuu").show().html("<tr><td colspan=4 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
                //$.post("/Thietlap/Ajax_Tracuu_Tracking", $("#form_").serialize(), function (ok) {
                //    $("#ketqua_tracuu").html(ok);
                //});
                //return false;
                var tentimkiem = $("#_form").serialize();
                window.location = "/Thietlap/Timkiemlichsu/?" + tentimkiem
                return false;
            }
            else
            {
                alert("Vui lòng chọn ngày kết thúc và ngày bắt đầu !");
                return false;
            }
        }
     
</script>

</asp:Content>