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
								<i class="icon-print"></i> In báo cáo kiến nghị có thẩm quyền trung ương
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content">
                            <form method="post" name="form_" id="form_" class="form-horizontal form-column">
                                <input value="<%= ViewData["data-linhvuc"]%>" type="hidden" id="data-linhvuc" />
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Chọn kỳ họp:</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full">
                                                    <select id="listKyHop" multiple="multiple" name="listKyHop">
                                                        <%=ViewData["kyhop"] %>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    <%--<div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label"></label>
                                            <div class="controls">
                                                <%=ViewData["check-hinhthuc"] %>
                                                </div>
                                            </div>
                                        </div> --%>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Nguồn kiến nghị:</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full">
                                                    <select id="listNguonKienNghi" name="listNguonKienNghi" multiple="multiple"><%=ViewData["opt-nguonkiennghi"] %>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Lĩnh vực:</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full" id="div_linhvuc">
                                                    <select id="listLinhVuc" name="listLinhVuc"multiple="multiple">
                                <%--                                        <option value="-1">- - - Chọn tất cả</option>
                                                                        <option value="0">Nhiều lĩnh vực liên quan</option>--%>
                                                        <%=ViewData["opt-linhvuc"] %></select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    <div class="span6">
                                         <div class="control-group ">
                                                 <label for="textfield" class="control-label ">Tên báo cáo:</label>
                                                 <div class="controls">
                                                     <div  class="input-block-level banndannguyen-popup">
                                                         <select class="chosen-select" name="iTenBaoCao" id="iTenBaoCao">
                                                             <%=ViewData["opt-tenbaocao"] %>
                                                             </select>
                                                         </div>
                                                 </div>
                                             </div>
                                         </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group tright">
                                            <input type="hidden" value="1" name="hidechecksearch" />
                                            <button type="button" onclick="print('xls')" class="btn btn-success"><i class="icon-print"></i> In Excel </button>
                                            <button type="button" onclick="print('pdf')" class="btn btn-success"><i class="icon-file"></i> In PDF </button>
                                            </div>                            
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
    $(document).ready(function () {
            $('#listKyHop').multiselect({
                enableCollapsibleOptGroups: true,
                includeSelectAllOption: true,
                selectAllText: 'Chọn toàn bộ',
                allSelectedText: 'Đã chọn toàn bộ',
                nonSelectedText: 'Chọn kỳ họp',
                nSelectedText: 'kỳ họp đã chọn'
            });
            $('#listLinhVuc').multiselect({
                //enableCollapsibleOptGroups: true,
                includeSelectAllOption: true,
                selectAllText: 'Chọn tất cả',
                allSelectedText: 'Đã chọn tất cả',
                nonSelectedText: 'Chọn lĩnh vực',
                nSelectedText: 'Lĩnh vực đã chọn'
            });
            $("#listNguonKienNghi").multiselect({
                enableCollapsibleOptGroups: true,
                includeSelectAllOption: true,
                selectAllText: 'Chọn tất cả',
                allSelectedText: 'Đã chọn toàn bộ',
                nonSelectedText: 'Chọn nguồn kiến nghị',
                nSelectedText: 'nguồn kiến nghị đã chọn'
            });
            $("#iDonViTiepNhan").chosen();
    });

    function print(ext) {
        var lstLinhVuc = $('#listLinhVuc').val();
        if (lstLinhVuc == "" || lstLinhVuc == null) {
            alert("Vui lòng chọn lĩnh vực ");
            return;
        }
            var pramt = $("#form_").serialize();
            window.open("/Kiennghi/BaoCaoMoiTonghop_bandannguyen/?ext=" + ext + "&" + pramt,"_blank");
            return false;
        }
</script>
<style>

</style>

