<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Import kiến nghị
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
				    <span>Import kiến nghị</span>
			    </li>
		    </ul>
		    
	    </div>       
                  <div class="function_chung">
                <a data-original-title="Mẫu kiến nghị import kèm theo" rel="tooltip" href="/Kiennghi/Download_Mau_Import/" class="add btn_f blue"><i class="icon-list-alt"></i></a>
                <a <%=ViewData["bt-add"] %> onclick="ShowTimKiem('/Kiennghi/Ajax_Import_add','import_place')" data-original-title="Import kiến nghị" rel="tooltip" href="javascript:void(0)" class="add btn_f blue" ><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">                    
                    <input type="text" name="q" id="q" value="" placeholder="Kế hoạch, nội dung">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button> 
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kiennghi/Ajax_search_giamsat/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm nâng cao" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
                    
                </form>
            </div> <div id="search_place"></div><div id="import_place"></div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-save"></i> Danh sách import kiến nghị</h3>
                        <div class="pull-right box-title-header">
                             <form id="form_header">
                                 <select class="chosen-select" name="iKyHop" id="iKyHop" onchange="ChangeDonVi_KyHop();">
                                        <%=ViewData["opt-kyhop"] %>
                                  </select>               
                            </form>
                            </div>
				    </div>
				    <div class="box-content nopadding">                    
                        <table class="table table-bordered table-condensed table-striped">
                            <thead>
                                <tr >
                                    <th width="3%" class="tcenter">STT </th> 
                                    <th nowrap width="15%" class="tcenter">Ngày Import</th>                                       
                                    <th nowrap class="">Ghi chú</th>
                                    <th class="tcenter" nowrap>Số kiến nghị</th>   
                                    <th class="tcenter" width="5%" nowrap>Chức năng</th>                                     
                                </tr>
                            </thead>
                            <tbody id="q_data">
                                <%=ViewData["list"] %>
                            </tbody>
                        </table> 					                              
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
   <script type="text/javascript">
       function ChangeDonVi_KyHop() {
           var pramt = $("#form_header").serialize();
           location.href = "/Kiennghi/Import/?" + pramt;
       }       
    </script>
</asp:Content>
