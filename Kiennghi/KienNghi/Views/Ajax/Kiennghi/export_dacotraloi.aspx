<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color">
			            <div class="box-title">
				            <h3>
					            <i class="icon-print"> </i> In báo cáo tập hợp kiến nghị đã có trả lời
				            </h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content">
                            <form id="form_" method="post" onsubmit="return CheckFormSearch();" class="form-horizontal form-column">  
                                <input value="<%= ViewData["data-linhvuc"]%>" type="hidden" id="data-linhvuc" />

                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Chọn kỳ họp</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full">
                                                    <select id="listKyHop" multiple="multiple" name="listKyHop">
                                                    <%=ViewData["opt-kyhop"] %>
                                                </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label"></label>
                                            <div class="controls">
                                                <%=ViewData["check-kyhop"] %>
                                            </div>
                                        </div>
                                    </div>
                                </div>  
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Nguồn kiến nghị:</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full">
                                                <select id="listHuyen_Xa_ThanhPho" multiple="multiple" name="listHuyen_Xa_ThanhPho">
                                                    <%=ViewData["huyen_thixa_thanhpho"] %>
                                                </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Lĩnh vực:</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full" id="div_linhvuc">
                                                    <select id="iLinhVuc" name="iLinhVuc" multiple="multiple">
                                                    <%=ViewData["opt-linhvuc"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                    
                                 <div class="row-fluid">
                                     <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Loại báo cáo:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iLoaiBaoCao" id="iLoaiBaoCao" class="chosen-select input-block-level">                                    
                                                        <%=ViewData["opt-loaibaocao"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Tên báo cáo:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iTenBaoCao" id="iTenBaoCao" class="chosen-select input-block-level">                                    
                                                        <%=ViewData["opt-tenbaocao"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div> 
                    
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group tright">
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
        $('#listHuyen_Xa_ThanhPho').multiselect({
            enableCollapsibleOptGroups: true,
            includeSelectAllOption: true,
            selectAllText: 'Chọn toàn bộ',
            allSelectedText: 'Đã chọn toàn bộ',
            nonSelectedText: 'Chọn Huyện/Thị xã/Thành phố',
            nSelectedText: 'Huyện/Thị xã/Thành phố đã chọn'
        });
        $('#iLinhVuc').multiselect({
            //enableCollapsibleOptGroups: true,
            includeSelectAllOption: true,
            selectAllText: 'Chọn tất cả',
            allSelectedText: 'Đã chọn tất cả',
            nonSelectedText: 'Chọn lĩnh vực',
            nSelectedText: 'Lĩnh vực đã chọn'
        });
        $('#iLoaiBaoCao').chosen();
        $('#iTenBaoCao').chosen();
    });
   
    function print(ext) {
        var lstLinhVuc = $('#iLinhVuc').val();
        if (lstLinhVuc == "" || lstLinhVuc == null) {
            alert("Vui lòng chọn lĩnh vực ");
            return;
        }
        var pramt = $("#form_").serialize();
        window.open("/Kiennghi/BaoCao_TapHop_Dacotraloi/?ext=" + ext + "&" + pramt, "_blank");
        return false;
    }
</script>