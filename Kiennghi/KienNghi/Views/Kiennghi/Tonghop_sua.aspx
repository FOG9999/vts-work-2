<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
 Cập nhật Tập hợp kiến nghị cử tri
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
				    <span>Cập nhật Tập hợp kiến nghị cử tri</span>                    
			    </li>                
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div> 
                     <% KN_TONGHOP th = (KN_TONGHOP)ViewData["tonghop"]; %>
         <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Cập nhật Tập hợp kiến nghị cử tri</h3>

                        </div>
                        <div class="box-content" style="text-align: left;">

                            <form method="post" name="_form" id="_form"  onsubmit="return CapNhat();" enctype="multipart/form-data" class="form-horizontal form-column">
                                <div class="row-fluid">
                                    <%--<div class="span6">                                        
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Kỳ họp <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select class="chosen-select"  id="iKyHop" name="iKyHop">
                                                    <%=ViewData["opt-kyhop"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>                                       
                                    </div>--%>
                                    <div class="span6">
                                        <div class="control-group nomargin nopadding">
                                            <label for="textfield" class="control-label">Hình thức <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <%=ViewData["check-hinhthuc"] %>
                                            </div>
                                        </div>
                                    </div>
                                    <% if (ViewData["dbqh"].ToString() == "1"){ %>
                                    <div class="span6">
                                        <div class="control-group nomargin nopadding">
                                            <label for="textfield" class="control-label">Chuyển đơn vị xử lý</label>
                                            <div class="controls">
                                                <%=ViewData["donvithamquyen"] %>
                                            </div>
                                        </div>
                                    </div>
                                    <% } %>
                                </div>  
                                <%--<div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Nơi Tập hợp <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select <%=ViewData["disable_donvitonghop"] %>  name="iDonViTongHop" id="iDonViTongHop" class="chosen-select">
                                                <%=ViewData["opt-donvitonghop"] %>
                                            </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <% if (ViewData["dbqh"].ToString() == "1"){%>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Thẩm quyền giải quyết</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                   <select name="iThamQuyenDonVi" id="iThamQuyenDonVi" onchange="ChangeLinhVucByDonVI(this.value)" class="chosen-select">                                            
                                                    <%=ViewData["opt-donvithamquyen"] %>
                                            </select>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <% } %>
                                </div>                         
                                
                                    
                                    <% if (ViewData["dbqh"].ToString() == "0"){ %>
                                    <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Thẩm quyền giải quyết <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                   <select name="iThamQuyenDonVi" id="iThamQuyenDonVi" onchange="ChangeLinhVucByDonVI(this.value)" class="chosen-select">                                            
                                                    <%=ViewData["opt-donvithamquyen"] %>
                                            </select>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Lĩnh vực </label>
                                            <div class="controls">
                                                <div class="input-block-level" id="div_linhvuc">
                                                    <select name="iLinhVuc" id="iLinhVuc" class="chosen-select">                                                 
                                                     <%=ViewData["opt-linhvuc"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                    <% }else {%>
                                        <input type="hidden" name="iLinhVuc" value="0" />
                                    <% } %>
                                
                                --%>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Ghi chú <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea  name="cNoiDung"  rows="15" id="cNoiDung" class="input-block-level"><%=Server.HtmlEncode(th.CNOIDUNG) %></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Từ khóa tìm kiếm (tags)  <em>Cách nhau bởi dấu <strong class="f-red">";"</strong></em></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea  name="cTuKhoa" class="input-block-level"> <%=Server.HtmlEncode(th.CTUKHOA) %> </textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">
                                            File đính kèm
                                       
                                            <small>Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</small></label>
                                        <div class="controls">
                                            <%=ViewData["file"] %>
                                            <% for (int i = 1; i < 4; i++)
                                            {
                                                    string style_none = ""; if (i > 1) { style_none = "display:none; margin-top:5px;"; }
                                                    string change = "";
                                                    if (i < 3)
                                                    {
                                                        int j = i + 1;
                                                        change = "$('.upload"+j+"').show()";
                                                    }
                                                    %>
                                                <div class="input-group file-group upload<%=i %>" style="<%=style_none%>">
                                                    <span class="input-group-btn">
                                                        <span class="btn btn-success btn-file">
                                                            Duyệt file
                                                            <input onchange="CheckFileTypeUpload('file_upload<%=i %>','file_name<%=i %>');<%=change %>" 
                                                                name="file_upload<%=i %>" id="file_upload<%=i %>" type="file">
                                                        </span>
                                                    </span>
                                                    <input class="input-xlarge" disabled id="file_name<%=i %>" type="text">
                                                    <span class="btn btn-danger" onclick="$('#file_upload<%=i %>,#file_name<%=i %>').val('');" title="Hủy"><i class="icon-trash"></i></span>
                                                </div>
                                            <% } %> 
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="row-fluid" style="border-bottom: 1px solid #fff">
                                    <div class="form-actions">
                                                <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                        <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                        <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning" onclick="ShowPageLoading()">Quay lại</a>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>         
                  
                  
                  
                  
                         
        
    </div>  
        </div>
    
    <script type="text/javascript">
        function CapNhat() {
            //if ($("#iKyHop").val() == 0) {
            //    alert("Vui lòng chọn kỳ họp!"); $("#iKyHop").focus(); return false;
            //}
            //if ($("#iDonViTongHop").val() == 0) {
            //    alert("Vui lòng chọn đơn vị Tập hợp!"); $("#iDonViTongHop").focus(); return false;
            //}
            //if ($("#iThamQuyenDonVi").val() == -1) {
            //    alert("Vui lòng chọn đơn vị có thẩm quyền giải quyết!"); $("#iThamQuyenDonVi").focus(); return false;
            //}
            if ($("#cNoiDung").val() == "") {
                alert("Vui lòng điền ghi chú Tập hợp!"); $("#cNoiDung").focus(); return false;
            }
            ShowPageLoading();
            //if ($("#iThamQuyenDonVi").val() == 0) { alert("Vui lòng chọn đơn vị có thẩm quyền xử lý!"); return false;}
        }
        function ChangeLinhVucByDonVI(val) {
            $.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
                //alert(data);
                $("#div_linhvuc").html('<select name="iLinhVuc" id="iLinhVuc" class="chosen-select">' + data + '</select>');
                //$('#iChuongTrinh').trigger('chosen:updated');
                $("#iLinhVuc").chosen();
            });
        }
    </script>
</asp:Content>
