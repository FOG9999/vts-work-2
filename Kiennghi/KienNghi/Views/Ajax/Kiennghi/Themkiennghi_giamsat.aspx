<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color box-bordered">
			<div class="box-title">
				<h3 >
					<i class="icon-search"> </i> Tìm kiến nghị thêm vào kế hoạch giám sát
				</h3>
                <%--<ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>--%>
            </div>
            <div class="box-content">
                <form id="form_chon" onsubmit="return ChonKienNghi()" class="form-horizontal form-column">
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Chọn kỳ họp:</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                        <select class="input-block-level" onchange="ChangeKyHop_()" id="iKyHop_" name="iKyHop_">
                                            <option value="0">- - - Chọn tất cả</option>
                                            <%=ViewData["opt-kyhop"] %>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Đoàn tiếp nhận</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                       <select name="iDonViTiepNhan_" id="iDonViTiepNhan_" onchange="ChangeKyHop_()" class="input-block-level">
                                            <%=ViewData["opt-donvitiepnhan"] %></select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Thẩm quyền xử lý:</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                        <select onchange="ChangeLinhVucByDonVI(this.value)" name="iThamQuyenDonVi_" class="chosen-select input-block-level">                                    
                                            
                                            <%=ViewData["opt-donvithamquyen"] %></select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Lĩnh vực:</label>
                                <div class="controls">
                                    <div class="input-block-level" id="div_linhvuc">
                                        <select name="iLinhVuc_" class="input-block-level">
                                            <option value="-1" selected>- - - Chọn tất cả</option>
                                            <%=ViewData["opt-linhvuc"] %>
                                        </select>
                                    </div>
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Nội dung, từ khóa:</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                        <input type="text" class="input-block-level" name="cNoiDung_" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <div class="control-group tright">
                                    <input name="id" id="id" value="<%=ViewData["id"] %>" type="hidden">
                                    <a href="javascript:void(0)" onclick="TimKienNghi()" class="btn btn-success"><i class="icon-search"></i> Tìm kiến nghị</a> 
                            </div>                            
                        </div>
                    </div> 
                                                
                </form>                            
            </div>                            
        </div>
    </div>
</div>
<script type="text/javascript">
    function ChangeLinhVucByDonVI(val) {
        $.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
            $("#div_linhvuc").html('<select name="iLinhVuc" id="iLinhVuc" class="chosen-select"><option value="-1">- - - Chọn tất cả</option>' + data + '</select>');
            $("#iLinhVuc").chosen();
        });
    }
   
</script>
