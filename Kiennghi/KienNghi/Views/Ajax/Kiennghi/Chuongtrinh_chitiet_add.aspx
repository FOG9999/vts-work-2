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
								<i class="icon-reorder"></i> Cập nhật lịch tiếp đại biểu
							</h3>
                            
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" onsubmit="return CheckForm();" name="_form" id="_form" class="form-horizontal form-column" enctype="multipart/form-data" action="/Kiennghi/Chuongtrinh_chitiet_insert" >
                                <div class="row-fluid">
                                   <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Đại biểu <i class="f-red">*</i></label>
							                <div class="controls">
                                                <div>
                                                    <select class="input-block-level" name="iDaiBieu" id="iDaiBieu">
                                                        <option value="0">Vui lòng chọn</option>
                                                        <%=ViewData["opt-daibieu"] %>
                                                    </select>
                                                </div>                                            
							                </div>
						                </div>
                                   </div>
                                    <div class="span6">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Địa phương <i class="f-red">*</i></label>
							                <div class="controls">
                                                <div>
                                                    <select class="input-block-level" name="iDiaPhuong" id="iDiaPhuong">
                                                        <option value="0">Vui lòng chọn</option>
                                                        <%=ViewData["opt-diaphuong"] %>
                                                    </select>
                                                </div>                                            
							                </div>
						                </div>
                                   </div>
                                </div>                                
                                <div class="row-fluid">
                                    <div class="span12">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Ngày tiếp </label>
							                <div class="controls">
                                                <input type="text" name="dNgayTiep" id="dNgayTiep" class="input-medium datepick" />
							                </div>
						                </div>
                                   </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                       <div class="control-group">
							                <label for="textfield" class="control-label">Địa chỉ tiếp </label>
							                <div class="controls">
                                                <input type="text" name="cDiaChi" id="cDiaChi" class="input-block-level" />
							                </div>
						                </div>
                                   </div>
                                </div>                                                                                                             
						        <div class="form-actions">
                                    <button type="submit" class="btn btn-success">Cập nhật</button>
                                    <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
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
    function CheckForm() {
        if ($("#iDaiBieu").val() == 0) { alert("Vui lòng chọn đại biểu tiếp!"); $("#iDaiBieu").focus(); return false; }
        if ($("#iDiaPhuong").val() == 0) { alert("Vui lòng chọn địa phương!"); $("#iDiaPhuong").focus(); return false; }
    }
</script>