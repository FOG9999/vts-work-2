<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Thêm mới phòng ban
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat(); ">
                                
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Tên phòng ban <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value=""  name="cTen" id="cTen" class="input-block-level"  onchange="kiemtrungten()" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label " >Phòng ban cha</label>
							        <div class="controls">
                                        <select name="iPhongBanCha" id="iPhongBanCha" class="input-block-level chosen-select" onchange="ChangePhongbanCha(this, this.value);">
                                            <option value='0'>- - - Chọn phòng ban</option>
                                            <%=ViewData["opt_phongbancha"] %>
                                        </select>
							        </div>
						        </div> 
                                <div class="control-group">
							        <label for="textfield" class="control-label " >Thuộc đơn vị <i class="f-red">*</i></label>
							        <div class="controls">
                                        <select name="iDonVi" id="iDonVi" class="input-block-level chosen-select">
                                            <option value='0'>- - - Chọn đơn vị</option>
                                            <%=ViewData["opt_donvi"] %>
                                        </select>
							        </div>
						        </div> 
                                                                                                                
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
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
    $("#iDonVi").chosen();
    $("#iPhongBanCha").chosen();
    function CapNhat() {
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập phòng ban"); $("#cTen").focus();
            return false;
        }
        if ($("#iDonVi").val() == "0") {
            alert("Vui lòng chọn đơn vị"); $("#iDonVi").focus();
            return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        var disabled = $("#iDonVi").prop('disabled');
        $("#iDonVi").prop('disabled', false);

        $.post("/Thietlap/Ajax_Phongban_insert", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Thêm mới thành công");
            } else {
                alert("Phòng ban đơn đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
                $("#iDonVi").prop('disabled', disabled);
            }
        });
        
        return false;
    }
    function ChangePhongbanCha(element, value) {
        if (!value || value == 0) {
            $("#iDonVi").val(0).prop('disabled', false).trigger("liszt:updated");
        } else {
            var iDonvi = $(element).find("option:selected").data("idonvi");
            $("#iDonVi").val(iDonvi).prop('disabled', true).trigger("liszt:updated");
            $("#iDonVi").prop('disabled', false);
        }
    }
</script>
<style>
    .chzn-disabled {
        opacity: 1 !important;
    }
</style>
