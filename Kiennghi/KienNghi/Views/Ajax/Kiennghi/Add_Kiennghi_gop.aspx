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
								<i class="icon-reorder"></i> Gộp kiến nghị cùng nội dung
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal form-column" onsubmit="return CapNhat()" enctype="multipart/form-data" action="/Kiennghi/Kiennghi_gop" >                                
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
							                <label for="textfield" class="control-label">Thẩm quyền giải quyết  <i class="f-red">*</i></label>
							                <div class="controls">
                                                <select name="iThamQuyenDonVi" onchange="ChangeLinhVucByDonVi1(this.value)" class="chosen-select">                                            
                                                    <%=ViewData["opt-donvithamquyen"] %>
                                                </select>
							                </div>
						                </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
							                <label for="textfield" class="control-label">Lĩnh vực</label>
							                <div class="controls">
                                                <div class="input-block-level" id="div_linhvuc1">
                                                    <select name="iLinhVuc" id="iLinhVuc1" class="chosen-select">                                                 
                                                        <%=ViewData["opt-linhvuc"] %>
                                                    </select>
                                                </div>
							                </div>
						                </div>
                                    </div>
                                </div>                                 
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Nội dung kiến nghị <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea  name="cNoiDung" id="cNoiDung" class="input-block-level"><%=ViewData["noidung"] %></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>     
                                <div class="row-fluid" style="border-bottom: 1px solid #fff">                                                                                                               
						            <div class="form-actions">
                                        <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                        <button type="submit" class="btn btn-success">Cập nhật</button>                                    
                                        <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
						            </div>  
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
    $("#cNoiDung").focus();
    function CapNhat() {
        if ($("#iThamQuyenDonVi").val() == 0) { alert("Vui lòng chọn đơn vị có thẩm quyền giải quyết!"); $("#iThamQuyenDonVi").focus(); return false; }
        if ($("#cNoiDung").val() == "") { alert("Vui lòng nhập nội dung kiến nghị gộp!"); $("#cNoiDung").focus(); return false; }
    }
    function ChangeLinhVucByDonVi1(val) {
        $.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
            //alert(data);
            $("#div_linhvuc1").html('<select name="iLinhVuc" id="iLinhVuc1" class="chosen-select">' + data + '</select>');
            //$('#iChuongTrinh').trigger('chosen:updated');
            $("#iLinhVuc1").chosen();
        });
    }
</script>
