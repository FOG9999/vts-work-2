<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kiểm trùng nội dung kiến nghị trong tổng hợp
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
				    <span>Kiểm trùng nội dung kiến nghị trong tổng hợp</span>
			    </li>
		    </ul>
		   
	    </div>
        <% KN_KIENNGHI th = (KN_KIENNGHI)ViewData["kiennghi"];
            
            KN_CL d = (KN_CL)ViewData["detail"]; %>
        <div class="breadcrumbs" style="padding:5px 15px">
            <p><strong>Nội dung:</strong> <%=Server.HtmlEncode(th.CNOIDUNG) %> <%=ViewData["file"] %></p>
            <p><strong>Đơn vị tiếp nhận:</strong> <%=Server.HtmlEncode(d.donvi_tiepnhan)%>; 
                <strong>Thẩm quyền:</strong> <%=Server.HtmlEncode(d.donvi_thamquyen)%>;
                <strong>Lĩnh vực:</strong> <%=Server.HtmlEncode(d.linhvuc)%>
            </p>
        </div>
        
        <div class="row-fluid" style="margin-bottom:20px">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-pencil"></i> Cập nhật nội dung chung của nhóm kiến nghị cùng nội dung</h3>
				    </div>
				    <div class="box-content">        
                         <form id="_form" method="post" class="form-horizontal" onsubmit="return CheckForm();">
                            <div class="control-group">
					            <label for="textfield" class="control-label">Nội dung chung</label>
					            <div class="controls">
                                    <textarea name="CNOIDUNG_TRUNG" id="CNOIDUNG_TRUNG" class="input-block-level"><%=Server.HtmlEncode(th.CNOIDUNG_TRUNG) %></textarea>
					            </div>
				            </div>
                            <div class="control-group nomargin">
					            <label for="textfield" class="control-label"></label>
					            <div class="controls">
                                    <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                    <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning">Quay lại</a>
					            </div>
				            </div>
                        </form>                       
				    </div>
			    </div>
		    </div>
	    </div>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i> Chọn kiến nghị có nội dung tương tự trong tổng hợp</h3>
				    </div>
				    <div class="box-content nopadding">        
                         <table class="table table-condensed table-bordered table-striped">
                             <thead>
                                 <tr>
                                    <th width="3%" nowrap>STT</th>
                                    <th nowrap>Nội dung kiến nghị</th>
                                    <th width="3%" nowrap>Chọn</th>
                                </tr>
                             </thead>
                            <tbody>
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
    function CheckForm() {
        if ($("#CNOIDUNG_TRUNG").val() == "") {
            alert("Vui lòng nhập nội dung chung của kiến nghị trùng"); $("#CNOIDUNG_TRUNG").focus();
            return false;
        }
    }
   
</script>
</asp:Content>
