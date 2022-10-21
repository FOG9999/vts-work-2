<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Quản lý nhóm tài khoản
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
				    <span>Danh sách nhóm tài khoản</span>
			    </li>
		    </ul>
		    
	    </div>
       <div class="function_chung">
                <a onclick="ShowPopUp('','/Thietlap/Ajax_Nhomtaikhoan_add')" data-original-title="Thêm mới nhóm tài khoản" rel="tooltip" href="#" class="add btn_f blue"><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get">

                    <select name="select" id="select" class='chosen-select form-control' onchange="ChangeChosenSelect('select','/Thietlap/Ajax_Nhomtaikhoan_search','div_change')">
                        <option selected value="0">Từ khóa tìm kiếm</option>
                          <%=  ViewData["Option-taikhoan"]  %>											
					</select>
                    <button type="submit" class="btn_f blue"><i class="icon-search"></i></button>
                </form>
            </div> 
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-th"></i> Danh sách các nhóm tài khoản</h3>
                        
				    </div>
				    <div class="box-content nopadding">
					    <table class="table table-bordered table-condensed table-striped">
                            <thead>
                                <tr>   
                                    <th nowrap>Tên nhóm tài khoản</th>   
                                    <th nowrap>Mô tả</th>
                                    <th nowrap width="45%">Các chức năng</th>  
                                    <th nowrap class="tcenter" width="5%">Kích hoạt</th>                                                                                                     
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

