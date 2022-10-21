<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<%@ Import Namespace="Utilities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Import đơn
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <%: Html.Partial("../Shared/_Left_Kntc") %>
<div id="main">
              <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                <li>
                    <span>Khiếu nại tố cáo <i class="icon-angle-right"></i></span>                    
                </li>
                <li>
				    <span>Import đơn</span>
			    </li>
		    </ul>
		    
	    </div>       
                  <div class="function_chung">
                <a data-original-title="Mẫu đơn import kèm theo" rel="tooltip" href="/Kntc/Download_Mau_Import/" class="add btn_f blue"><i class="icon-list-alt"></i></a>
                <a <%=ViewData["bt-add"] %> onclick="ShowTimKiem('/Kntc/Ajax_Import_add','import_place')" data-original-title="Import đơn" rel="tooltip" href="javascript:void(0)" class="add btn_f blue" ><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">                    
                    <%--<input type="text" name="q" id="q" value="" placeholder="Kế hoạch, nội dung">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button> 
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kntc/Ajax_search_giamsat/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm nâng cao" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
                    --%>
                </form>
            </div>  <div id="search_place"></div> <div id="import_place"></div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-save"></i> Import đơn</h3>
<%--                        <div class="pull-right box-title-header">
                             <form id="form_header">
                                 <select class="chosen-select" name="iKyHop" id="iKyHop" onchange="ChangeDonVi_KyHop();">
                                        <%=ViewData["opt-kyhop"] %>
                                  </select>               
                            </form>
                            </div>--%>
				    </div>
				    <div class="box-content nopadding">                    
                        <table class="table table-bordered table-condensed table-striped">
                            <thead>
                                <tr >
                                    <th width="3%" class="tcenter">STT </th> 
                                    <th nowrap width="15%" class="tcenter">Ngày Import</th>                                       
                                    <th nowrap class="">Ghi chú</th>
                                    <th class="tcenter" nowrap>Số đơn</th>   
                                    <th class="tcenter" nowrap>Kết quả Import</th>   
                                    <th class="tcenter" width="5%" nowrap>Chức năng</th>                                     
                                </tr>
                            </thead>
                            <tbody id="q_data">
                                <% List<KNTC_DON_IMPORT> listDonImport = (List<KNTC_DON_IMPORT>)ViewData["list"];
                                    var stt = 0;
                                    foreach(var donImport in listDonImport)
                                    {
                                        stt++;
                                    %>
                                     <tr >
                                        <td class="tcenter b" ><%=stt %></td> 
                                        <td class="tcenter" ><%=donImport.DDATE.HasValue ? donImport.DDATE.Value.ToString("dd/MM/yyyy") : ""%></td>                                       
                                        <td><%=HttpUtility.HtmlEncode(donImport.CGHICHU)%></td>
                                        <td class="tcenter b" ><%=donImport.ISODON%></td>   
                                        <td class="tcenter b" ><%=donImport.ITINHTRANG.HasValue && donImport.ITINHTRANG.Value == 1 ? "Thành công" : "Thất bại"%></td>   
                                        <td class="tcenter b"> <a class='<%=donImport.ITINHTRANG.HasValue && donImport.ITINHTRANG.Value == 1 ? "" : "hide"%>' href="/Kntc/Import_don/?id=<%=HashUtil.Encode_ID(donImport.ID.ToString())%>" title="Danh sách đơn đã import" class="trans_func"><i class="icon-list-ol"></i></a>
                                            <a href="javascript:void()" onclick="DeletePage_Confirm('<%=HashUtil.Encode_ID(donImport.ID.ToString())%>','id=<%=HashUtil.Encode_ID(donImport.ID.ToString())%>','/Kntc/Ajax_Import_don_del','Bạn có muốn xóa đơn đã import này hay không?')" title="Xóa đơn đã import" class="trans_func"><i class="icon-trash"></i></a>
                                        </td>                                 
                                    </tr>
                                    <%}
                                %>
                            </tbody>
                        </table>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
   <script type="text/javascript">
       var importResult = '<%=ViewData["importResult"]%>'
       console.log(importResult);
       if (importResult == "success") {
           AlertAction('Import đơn thành công!');
       }
       else if (importResult == "fail") {
           AlertAction('Import đơn thất bại!');
       }

       function ChangeDonVi_KyHop() {
           var pramt = $("#form_header").serialize();
           location.href = "/Kntc/Import/?" + pramt;
       }       
   </script>
</asp:Content>
