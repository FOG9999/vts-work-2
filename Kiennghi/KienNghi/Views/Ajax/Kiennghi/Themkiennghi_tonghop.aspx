<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color box-bordered">
						<div class="box-title">
							<h3 >
								<i class="icon-search"> </i> Tìm kiếm kiến nghị thêm vào Tập hợp
							</h3>
                            
                        </div>
                        <div class="box-content">
                            <form  class="form-horizontal form-column" id="form_chon" onsubmit="return TimKienNghi()">
                                <div class="row-fluid">
                                    <%--<div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Kỳ họp:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select class="input-block-level" onchange="ChangeKeHoach()" id="iKyHop_" name="iKyHop_">
                                                        <%=ViewData["opt-kyhop"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <% if (ViewData["dbqh"].ToString() != "1")
                                             { %>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Đơn vị tiếp nhận</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iDonViTiepNhan_" id="iDonViTiepNhan_" onchange="ChangeKeHoach()" class="input-block-level">
                                                    <%=ViewData["opt-donvitonghop"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <% } %>
                                </div>
                                <div class="row-fluid">
                                    <%--<div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Kế hoạch:</label>
                                            <div class="controls">
                                                <div class="input-block-level" id="div_chuongtrinh">
                                                    <select name="iChuongTrinh_" id="iChuongTrinh_" class="input-block-level">
                                                        <option value="-1" selected>- - - Chọn tất cả</option>
                                                        <option value="0">Không nằm trong chương trình, kế hoạch tiếp xúc cử tri</option>
                                                        <%=ViewData["opt-kehoach"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Thẩm quyền giải quyết:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iThamQuyenDonVi_" onchange="ChangeLinhVucByDonVI(this.value)"  class="input-block-level">
                                                        
                                                        <%=ViewData["opt-donvithamquyen"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Lĩnh vực:</label>
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
                                            <label for="textfield" class="control-label">Nội dung, từ khóa:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <input type="text" name="cNoiDung_" class="input-block-level" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group tright">
                                            <input type="hidden" name="id" id="id" value="<%=ViewData["id"] %>" />
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
    $("select").chosen();
    function ChangeKeHoach() {
        $.post("/Kiennghi/Ajax_Change_Kyhop_kehoach", 'iKyHop=' + $("#iKyHop_").val() + '&iDonViTiepNhan=' + $("#iDonViTiepNhan_").val(), function (data) {
            //alert(data);
            $("#div_chuongtrinh").html('<select name="iChuongTrinh_" id="iChuongTrinh_" class="chosen-select"><option value="-1" selected>- - - Chọn tất cả</option><option value="0">Không nằm trong chương trình, kế hoạch tiếp xúc cử tri</option>' + data + '</select>');
            //$('#iChuongTrinh').trigger('chosen:updated');
            $("#iChuongTrinh_").chosen();
        });
    }
    function ChangeLinhVucByDonVI(val) {
        $.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
            //alert(data);
            $("#div_linhvuc").html('<select name="iLinhVuc_" id="iLinhVuc_" class="chosen-select"><option value="-1" selected>- - - Chọn tất cả</option>' + data + '</select>');
            //$('#iChuongTrinh').trigger('chosen:updated');
            $("#iLinhVuc_").chosen();
        });
    }
</script>