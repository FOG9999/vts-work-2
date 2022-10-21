<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color box-bordered">
			<div class="box-title">
				<h3>
					<i class="icon-search"> </i> Tìm kiếm kế hoạch giám sát
				</h3>
                <%--<ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>--%>
            </div>
            <div class="box-content popup_info">
                <form id="form_" method="post" onsubmit="return CheckFormSearch();" class="form-horizontal form-column">     
                    <div class="row-fluid">
                        <div class="span12">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Chọn đơn vị giám sát:</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                        <select class="chosen-select" name="iDonVi" id="iDonVi">
                                        <%=ViewData["opt-donvi"] %>
                                    </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>                      
                    <div class="row-fluid">
                        <div class="span12">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Kế hoạch, nội dung:</label>
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
    function CheckFormSearch() {
        //location.href = "/Kiennghi/Giamsat/?" + $("#form_").serialize();
        //$("#q_data").html("<tr><td colspan=4 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
        //$.post("/Kiennghi/Ajax_search_giamsat_result", $("#form_").serialize(), function (ok) {
        //    $("#q_data").html(ok);
        //});
        return false;
    }
</script>
   