<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Chuyển kiến nghị sang Tập hợp khác
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" onsubmit="return CheckForm();" name="_form" id="_form" class="form-horizontal" enctype="multipart/form-data" action="/Kiennghi/Kiennghi_chuyen_insert" >
                                <%--<% if (ViewData["dbqh"].ToString() == "0")
                                    { %>
                                <div class="row-fluid" style="border-bottom:none">
                                   <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Chọn đơn vị thẩm quyền <i class="f-red">*</i></label>
							                <div class="controls">
                                                <div>
                                                    <select class="input-block-level chosen-select" name="iDonVi" id="iDonVi" onchange="ChangeDonViThamQuyen(this.value)">
                                                        <%=ViewData["opt-thamquyen"] %>
                                                    </select>
                                                </div>                                            
							                </div>
						                </div>
                                   </div>
                                </div>                                
                                <% } %>--%>
                                <div class="row-fluid" style="border-bottom:none">
                                    <div class="span12">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Chọn Tập hợp kiến nghị cần chuyển <i class="f-red">*</i></label>
							                <div class="controls" id="place_tonghop">
                                                <select class="input-block-level chosen-select" name="iTongHop" id="iTongHop" >
                                                    <option value="0">Vui lòng chọn</option>
                                                    <%=ViewData["opt-tonghop"] %>
                                                </select>
							                </div>
						                </div>
                                   </div>
                                    
                                </div>
                                                                                                                                             
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-success">Cập nhật</button>
                                    <input type="hidden" name="id" id="id" value="<%=ViewData["id"] %>" />
                                    <input type="hidden" name="loaicq" id="loaicq" value="<%=ViewData["loaicq"] %>" />
                                    <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
						        </div>                     
                             </form>
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>

<script type="text/javascript">   
    $("#iTongHop,#iDonVi").chosen();
    function ChangeDonViThamQuyen(val) {
        if (val != 0) {
            $.post("/Kiennghi/Ajax_Change_donvi_tonghop_option", 'id=' + val + '&id_kiennghi=' + $("#id").val() + '&loaicq=' + $("#loaicq").val(), function (data) {
                //$("#iTongHop").html('<option value="0">Vui lòng chọn</option>' + data);
                $("#place_tonghop").html('<select name="iTongHop" id="iTongHop" class="chosen-select"><option value="0">- - - Vui lòng chọn</option>' + data + '</select>');
                $("#iTongHop").chosen();
            });
        } else {
            $("#place_tonghop").html('<select name="iTongHop" id="iTongHop" class="chosen-select"><option value="0">- - - Vui lòng chọn</option></select>');
            $("#iTongHop").chosen();
        }
        
        //$("#iTongHop").chosen();
    }
    function CheckForm() {
        if ($("#iDonVi").val() == 0) {
            alert("VUi lòng chọn đơn vị thẩm quyền!"); $("#iDonVi").focus(); return false;
        }
        if ($("#iTongHop").val() == 0) {
            alert("VUi lòng chọn Tập hợp cần chuyển!"); $("#iTongHop").focus(); return false;
        }
    }
</script>