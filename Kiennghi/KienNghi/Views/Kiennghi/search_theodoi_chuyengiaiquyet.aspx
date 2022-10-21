<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color  box-bordered">
			<div class="box-title">
				<h3>
					<i class="icon-search"> </i> Tìm kiếm tổng hợp kiến nghị
				</h3>
                <%--<ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>--%>
            </div>
            <div class="box-content">
                <form id="form_" method="post" onsubmit="return CheckFormSearch();" class="form-horizontal form-column">     
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Chọn kỳ họp:</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                        <select class="chosen-select" name="iKyHop" id="iKyHop">
                                        <%=ViewData["kyhop"] %>
                                    </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label"></label>
                                <div class="controls">
                                    <%=ViewData["check-hienthi"] %>
                                </div>
                            </div>
                        </div>
                    </div>  
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Thẩm quyền giải quyết:</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                        <select onchange="ChangeLinhVucByDonVI(this.value)" name="iDonViXuLy" class="chosen-select input-block-level">                                    
                                            <option value="0">- - - Chọn tất cả</option>
                                            <%=ViewData["opt-donvixuly"] %></select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Lĩnh vực:</label>
                                <div class="controls">
                                    <div class="input-block-level" id="div_linhvuc">
                                        <select name="iLinhVuc" class="chosen-select input-block-level">
                                        <option value="-1">- - - Chọn tất cả</option>
                                        <option value="0">Nhiều lĩnh vực liên quan</option>
                                        <%=ViewData["opt-linhvuc"] %></select>
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
                                        <input type="text" class="input-block-level" name="cNoiDung" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                    </div>
                    <div class="row-fluid">
                        <div class="span12">
                            <div class="control-group tright">
                                <button type="submit" class="btn btn-success"> Tra cứu</button>
                            </div>                            
                        </div>
                    </div>                                    
                </form>
                
            </div>                            
        </div>
    </div>
</div>
<script type="text/javascript">
    //function ChangeDoan(val) {
    //    $.post("/Kiennghi/Ajax_search_chuongtrinh_change_doan", "id=" + val, function (ok) {
    //        alert(ok);
    //        $("#iDiaPhuong").html(ok);
    //    });
    //    $.post("/Kiennghi/Ajax_search_chuongtrinh_change_daiphuong", "id=" + val, function (ok) {
    //        $("#iDaiBieu").html(ok);
    //    });
    //}
    function ChangeLinhVucByDonVI(val) {
        $.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
            $("#div_linhvuc").html('<select name="iLinhVuc" id="iLinhVuc" class="chosen-select"><option value="-1">- - - Chọn tất cả</option>' + data + '</select>');
            $("#iLinhVuc").chosen();
        });
    }
    function CheckFormSearch() {
        var pramt = $("#form_").serialize();
        location.href = "/Kiennghi/Theodoi_chuyengiaiquyet/?"+pramt;
        //$("#q_data").html("<tr><td colspan=5 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
        //$.post("/Kiennghi/Ajax_search_theoidoi_chuyengiaiquyet_result", $("#form_").serialize(), function (ok) {
        //    $("#q_data").html(ok);
        //});
        return false;
    }
</script>
   