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
					            <i class="icon-print"> </i> In báo cáo danh sách kiến nghị mới tiếp nhận
				            </h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content">
                            <form id="form_" method="post" class="form-horizontal form-column">  
                                 <input value="<%= ViewData["data-linhvuc"]%>" type="hidden" id="data-linhvuc" />
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Chọn kỳ họp</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full">
                                               <select id="listKyHop" multiple="multiple" name="listKyHop">
                                                    <%=ViewData["kyhop"] %>
                                                </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Nguồn kiến nghị:</label>
                                            <div class="controls">
                                            <div class="input-block-level select-form-width-full">
                                                <select name="listNguonKN" id="listNguonKN" multiple="multiple">
                                                    <%=ViewData["opt-nguonkiennghi"] %>
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
                                                    <select name="listLinhVuc" id="listLinhVuc"  multiple="multiple">
                                                    <%=ViewData["opt-linhvuc"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                     </div>
                                     <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Loại báo cáo :</label>
                                            <div class="controls">
                                                <div class="input-block-level select-form-width-full">
                                                    <select " name="iLoaiBaoCao" id="iLoaiBaoCao" class="chosen-select input-block-level">   
                                                        <%=ViewData["loaibaocao"] %></select>

                                                </div>
                                            </div>
                                        </div>
                                       </div>
                                   </div>
                                
                                <div class="row-fluid">
                                    
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label select-form-width-full">Tên báo cáo:</label>
                                            <div class="controls">
                                                <div class="input-block-level" >
                                                    <select name="iTenBaoCao" id="iTenBaoCao" class="chosen-select input-block-level">
                                                        <option value="1">1A - Phụ lục I: Danh mục kiến nghị của cử tri gửi đến kỳ họp</option>
                                                        <option value="2">1B - Tổng hợp kiến nghị của cử tri gửi đến kỳ họp (Lĩnh vực dòng ngang)</option>
                                                        <option value="3">1B1 - Tổng hợp kiến nghị của cử tri gửi đến kỳ họp (Theo tỷ lệ từng lĩnh vực)</option>
                                                        <option value="4">1B2 - Tổng hợp kiến nghị của cử tri gửi đến kỳ họp (Theo tỷ lệ từng lĩnh vực, đơn vị)</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>  
                               
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group tright">                        
                                            <button type="button" class="btn btn-" data-original-title="In Excel" onclick="XuatBaoCao('xlsx')"  rel="tooltip"> <i class="icon-print"></i>Xuất Excel</button>
                                            <button type="button" class="btn btn-" data-original-title="In PDF" onclick="XuatBaoCao('pdf')"  rel="tooltip"><i class="icon-file"></i> Xuất PDF</button>
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
            selectAllText: 'Chọn tất cả',
            allSelectedText: 'Đã chọn tất cả',
            nonSelectedText: 'Chọn kỳ họp',
            nSelectedText: 'kỳ họp đã chọn'
        });
        $('#listNguonKN').multiselect({
            enableCollapsibleOptGroups: true,
            includeSelectAllOption: true,
            selectAllText: 'Chọn tất cả',
            allSelectedText: 'Đã chọn tất cả',
            nonSelectedText: 'Chọn nguồn kiến nghị',
            nSelectedText: 'nguồn kiến nghị đã chọn'
        });
        $('#listLinhVuc').multiselect({
            enableCollapsibleOptGroups: true,
            includeSelectAllOption: true,
            selectAllText: 'Chọn tất cả',
            allSelectedText: 'Đã chọn tất cả',
            nonSelectedText: 'Chọn lĩnh vực',
            nSelectedText: 'Lĩnh vực đã chọn'
        });
        $('#iLoaiBaoCao').chosen();
        $('#iTenBaoCao').chosen();
    });
   
    function XuatBaoCao(ext) {
        
        var tenbaocao = $('#iTenBaoCao').val();
        var lstKyHop = $('#listKyHop').val();
        var lstNguonKN = $('#listNguonKN').val();
        var lstLinhVuc = $('#listLinhVuc').val();
        if (lstLinhVuc == "" || lstLinhVuc == null) {
            alert("Vui lòng chọn lĩnh vực ");
            return;
        }
        var slstKyHop = lstKyHop != undefined ? lstKyHop.join(',') : "";
        var slstNguonKN = lstNguonKN != undefined ? lstNguonKN.join(',') : "";
        var slstLinhVuc = lstLinhVuc != undefined ? lstLinhVuc.join(',') : "";

        var loaibaocao = $('#iLoaiBaoCao').val();
        var url = "";
        if (tenbaocao == 1) {
            url = "/Kiennghi/Baocao_DanhMucCuTri1A?lstKyHop=" + slstKyHop + "&lstNguonKN=" + slstNguonKN + "&lstLinhVuc=" + slstLinhVuc + "&loaibaocao=" + loaibaocao + "&ext=" + ext;

        }
        else if (tenbaocao == 2) {
            url = "/Kiennghi/BaoCao_TongHopYKienCuTri1B?lstKyHop=" + slstKyHop + "&lstNguonKN=" + slstNguonKN + "&lstLinhVuc=" + slstLinhVuc + "&loaibaocao=" + loaibaocao + "&ext=" + ext;
        }
        else if (tenbaocao == 3) {
            url = "/Kiennghi/BaoCao_TongHopYKienCuTri1B1?lstKyHop=" + slstKyHop + "&lstNguonKN=" + slstNguonKN + "&lstLinhVuc=" + slstLinhVuc + "&loaibaocao=" + loaibaocao + "&ext=" + ext;
        }
        else if (tenbaocao == 4) {
            url = "/Kiennghi/BaoCao_TongHopYKienCuTri1B2?lstKyHop=" + slstKyHop + "&lstNguonKN=" + slstNguonKN + "&lstLinhVuc=" + slstLinhVuc + "&loaibaocao=" + loaibaocao + "&ext=" + ext;
        }
        window.open(url, '_blank');

    }
</script>
