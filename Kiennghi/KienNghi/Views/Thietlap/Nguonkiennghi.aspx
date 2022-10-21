<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Quản lý danh mục nguồn kiến nghị
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial("../Shared/_Left_Thietlap") %>
<div id="main" class="">
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
				    <span>Danh sách nguồn kiến nghị</span>
			    </li>
		    </ul>
		    
	    </div>
        <div class="function_chung">
                <a onclick="ShowPopUp('','/Thietlap/Ajax_Nguonkiennghi_add')" data-original-title="Thêm mới" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get">

                    <select name="select" id="select" class='chosen-select form-control' onchange="ChangeChosenSelect('select','/Thietlap/Ajax_Nguonkiennghi_search','div_change')">
                        <option selected value="0">Từ khóa tìm kiếm</option>
                            <%=ViewData["Option_nguondon"]  %>												
					</select>
                    <button type="submit" class="btn_f blue"><i class="icon-search"></i></button>
                </form>
            </div>
        
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-exchange"></i>Danh sách danh mục nguồn kiến nghị</h3>
                        
				    </div>
				    <div class="box-content nopadding">
					    <table class="table table-bordered table-striped ">
                            <thead>
                                <tr>                          
                           
                                       <th nowrap class="tcenter" width="5%">Mã nguồn kiến nghị</th> 
                                    <th nowrap>Tên nguồn kiến nghị</th>    

                                       <th nowrap class="tcenter" width="5%">Vị trí</th>   
                                    <th class="tcenter"  width="5%" nowrap>Áp dụng</th>                                                          
                                    <th nowrap class="tcenter" width="5%">Chức năng</th>
                                </tr>
                            </thead>
                            <tbody id="div_change">                          
                               <%=ViewData["list"] %>
                            </tbody>
                        </table>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</div>
</asp:Content>
