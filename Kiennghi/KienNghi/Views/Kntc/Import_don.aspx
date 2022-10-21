<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Danh sách đơn đã Import
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
				    <span>Danh sách đơn đã Import</span>
			    </li>
		    </ul>
		    
	    </div>       
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered ">
				    <div class="box-title">
					    <h3><i class="icon-save"></i> Danh sách đơn đã Import</h3>
				    </div>
				    <div class="box-content nopadding" style="overflow: auto; width: auto; height: 500px;">                    
                        <table class="table table-bordered table-condensed table-striped">
                            <thead>
                                <tr >
                                    <th width="3%" class="tcenter">STT </th> 
                                    <th class="tcenter" nowrap>Họ và tên công dân</th>
                                    <th class="tcenter" nowrap>Quận/ huyện</th>
                                    <th class="tcenter" nowrap>Xã/Phường/Thị trấn</th>
                                    <th class="tcenter" nowrap>Địa chỉ cụ thể</th>
                                    <th nowrap class="tcenter">Nội dung đơn</th>   
                                    <th nowrap class="tcenter">Loại đơn</th>   
                                    <th width="10%" class="tcenter">Chức năng</th>   
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

       
   </script>
</asp:Content>

