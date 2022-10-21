<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kế hoạch giám sát kết quả trả lời kiến nghị
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
				    <span>Kế hoạch giám sát kết quả trả lời kiến nghị</span>
			    </li>
		    </ul>
		    
	    </div>       
                  <div class="function_chung">
                <a <%=ViewData["bt-add"] %> onclick="ShowPopUp('','/Kiennghi/Ajax_Giamsat_add')" data-original-title="Thêm kế hoạch giám sát" rel="tooltip" href="javascript:void(0)" class="add btn_f blue" ><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">                    
                    <input type="text" name="q" id="q" value="" placeholder="Kế hoạch, nội dung">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button> 
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kiennghi/Ajax_search_giamsat/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm nâng cao" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
                    
                </form>
            </div>
                   <div id="search_place"></div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Kế hoạch giám sát kết quả trả lời kiến nghị</h3>
				    </div>
				    <div class="box-content nopadding">                    
                        <table class="table table-bordered table-condensed table-striped">
                            <thead>
                                <tr >
                                    <th width="3%" class="tcenter">STT </th>   
                                    <th nowrap class="" width ="8%">Ngày bắt đầu </th>                                     
                                    <th nowrap class="" width ="8%">Kế hoạch </th>
                                    <th class="">Nội dung</th> 
                                    <th width="3%" class="tcenter" nowrap>Chức năng</th>                                          
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
   <script type="text/javascript">
        
       function TimKiem() {
           //location.href = "/Kiennghi/Giamsat/?q=" + $("#q").val();
           $("#ip_data").empty().html("");
           $('#loadData').show();
           $.ajax({
               type: "post",
               url: "<%=Url.Action("Giamsat", "Kiennghi")%>",
                data: { q: $("#q").val(), hidNormalSearch: 1 },
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm kế hoạch giám sát kết quả trả lời kiến nghị!");
                    }
                }
            });
            return false;
        }
   </script>
</asp:Content>
