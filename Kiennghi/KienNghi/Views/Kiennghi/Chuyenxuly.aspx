<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kiến nghị cử tri đã chuyển xử lý
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
				    <a href="#">Kiến nghị cử tri đã chuyển xử lý</a>
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
					    <h3><i class="icon-tags"></i> Danh sách Kiến nghị cử tri đã chuyển xử lý</h3>
                        
				    </div>
				    <div class="box-content nopadding">                     
                        
                            <table class="table table-bordered table-condensed">
                                <thead>
                                    <tr >
                                        <th width="3%" class="tcenter">STT </th>
                                        
                                        <th width="10%" nowrap class="tcenter">Mã Tổng hợp </th>
                                        <th class="tcenter">Nội dung tổng hợp </th>
                                        <th class="tcenter" nowrap>Thẩm quyền xử lý </th>                                               
                                        <th class="tcenter" nowrap>Trang thái</th>   
                                        <th class="tcenter" nowrap>Trả lời</th>                                          
                                    </tr>
                                </thead>
                                <tr>
                                    <td class="tcenter">1</td>
                                    <td class="b f-red tcenter" nowrap>TH_XIV_001</br>
                                        <a href="javascript:void(0)" onclick="ShowPopUp('','/Kiennghi/Ajax_Lichsu')" data-original-title="Lịch sử xử lý" rel="tooltip" title="" class="btn btn-primary"><i class="icon-time"></i></a></td>
                                    <td>Tổng hợp các kiến nghị số 1</td>
                                    <td class="tcenter">Bộ Xây dựng</td>      
                                    <td class="tcenter" nowrap>                                        Dự thảo 
                                        
                                    </td>
                                    <td class="tcenter" nowrap>
                                        <a href="javascript:void(0)" onclick="ShowPopUp('','/Kiennghi/Ajax_Traloi')" data-original-title="" rel="tooltip" title="" class="btn btn-success"><i class="icon-file-alt"></i></a>  
                                        
                                    </td>
                                </tr>    
                                <tr>
                                    <td class="tcenter">1</td>
                                    <td class="b f-red tcenter" nowrap>TH_XIV_002</br>
                                        <a href="javascript:void(0)" onclick="ShowPopUp('','/Kiennghi/Ajax_Lichsu')" data-original-title="Lịch sử xử lý" rel="tooltip" title="" class="btn btn-primary"><i class="icon-time"></i></a></td>
                                    <td>Tổng hợp các kiến nghị số 2</td>
                                    <td class="tcenter">Ủy ban thường vụ Quốc Hội</td>      
                                    <td class="tcenter" nowrap>
                                        Dự thảo
                                    </td>
                                    <td class="tcenter" nowrap>
                                        <a href="javascript:void(0)" onclick="ShowPopUp('','/Kiennghi/Ajax_Traloi')" data-original-title="" rel="tooltip" title="" class="btn btn-success"><i class="icon-file-alt"></i></a>  
                                        
                                    </td>
                                </tr>  
                                </table> 
					              
                                      
				    </div>
			    </div>
		    </div>
	    </div>
    </div>  
        </div>
    
</asp:Content>
