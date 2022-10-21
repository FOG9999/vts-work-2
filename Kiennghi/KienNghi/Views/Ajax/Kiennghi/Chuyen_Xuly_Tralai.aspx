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
								<%--<i class="icon-reorder"></i> Chuyển xử lý Tập hợp kiến nghị--%>
                                <i class="icon-reorder"></i>Trả lại chuyển xử lý
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat()" enctype="multipart/form-data" action="/Kiennghi/Chuyen_Xuly_Chuyenvien_update" >                                
                                 
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Đơn vị nhận<i class="f-red">*</i></label>
							        <div class="controls">
                                        <select name="idonvi" id="idonvi" class="chosen-select input-block-level" onchange ="DoiChuyenVien(this.value)">
                                            <%=ViewData["opt_donvi"] %>                                          
                                        </select>
							        </div>
						        </div>  
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Chuyên viên<i class="f-red">*</i></label>
							        <div class="controls">
                                        <select name="iNguoiNhan" id="iNguoiNhan" class="chosen-select input-block-level">
                                            <%=ViewData["opt-chuyenvien"] %>                                          
                                        </select>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Lý do trả lại <i class="f-red">*</i></label>
							        <div class="controls">
                                        <textarea class="input-block-level" " name="cNoiDung" id="cNoiDung"  value="" />
							        </div>
						        </div>                                                                                                                  
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-success">Chuyển xử lý</button>
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
    $("#cNoiDung").val('<%=ViewData["opt-cNoiDung"] %>');
    $("#idonvi").chosen();
    $("#iNguoiNhan").chosen();
    function CapNhat() {
        if ($("#iNguoiNhan").val() == "0") {
            alert("Xin vui lòng chọn chuyên viên nhận xử lý");
            return false;
        }

    }
    function DoiChuyenVien(val) {
        debugger;
        $.post("/Kiennghi/Ajax_Update_ListChuyenVien", '&val=' + val, function (data) {
            $("#TrungUong").show();
            $("#iNguoiNhan").html('<select name="iNguoiNhan" id="iNguoiNhan" class="chosen-select">' + data + '</select>');
            $("#iNguoiNhan").trigger("liszt:updated");
            $("#iNguoiNhan").chosen();
        });
    }
</script>
