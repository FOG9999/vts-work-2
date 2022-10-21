<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
            <div class="box box-color box-bordered">
    			<div class="box-title">
    				<h3>
    					<i class="icon-search"> </i> Tìm kiếm Tập hợp kiến nghị
    				</h3>
                </div>
                <div class="box-content popup_info">
                    <form id="form_" method="post" onsubmit="return CheckFormSearch();" class="form-horizontal form-column">
                        <input value="<%= ViewData["data-linhvuc"]%>" type="hidden" id="data-linhvuc" />
                        <div class="row-fluid">
                            <div class="span6">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Mã kiến nghị:</label>
                                    <div class="controls">
                                        <div class="input-block-level">
                                             <%=ViewData["MaKienNghi"] %> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span6">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Chọn kỳ họp:</label>
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
                                    <label for="textfield" class="control-label"></label>
                                    <div class="controls">
                                        <%=ViewData["check-hinhthuc"] %>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span6">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Từ ngày:</label>
                                        <div class="controls">
                                            <div class="chonngay-search input-block-level">
                                                 <input <%=ViewData["TuNgay"] %> type="text" class=" datepick" autocomplete="off" name="dBatDau" id="dBatDau" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Đến ngày:</label>
                                        <div class="controls">
                                            <div class="chonngay-search input-block-level">
                                                <input <%=ViewData["DenNgay"] %> type="text" class="datepick" autocomplete="off" name="dKetThuc" id="dKetThuc" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="span6">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Nguồn kiến nghị:</label>
                                    <div class="controls">
                                        <div class="input-block-level nguonkiennghi">
                                            <select id="listNguonKienNghi" name="listNguonKienNghi" multiple="multiple">
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
                                    <label for="textfield" class="control-label">Đơn vị tiếp nhận:</label>
                                    <div class="controls">
                                        <div class="input-block-level">
                                            <select id="iDonViTiepNhan" name="iDonViTiepNhan">
                                                <%=ViewData["opt-donvitiepnhan"] %>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="span6">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Thẩm quyền giải quyết:</label>
                                    <div class="controls">
                                        <div class="input-block-level donvixuly">
                                            <select onchange="ChangeLinhVucByDonVi(this.value)" name="iDonViXuLy" class="chosen-select input-block-level">
                                                <%=ViewData["opt-donvixuly"] %></select>
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
                                            <select id="listLinhVuc" name="listLinhVuc" multiple="multiple">
                                                <%--<option value="-1">- - - Chọn tất cả</option>--%>
                                                <%--<option value="0">Nhiều lĩnh vực liên quan</option>--%>
                                                <%=ViewData["opt-linhvuc"] %>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="span6">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Chọn Huyện/Thị xã/Thành phố:</label>
                                    <div class="controls">
                                        <div class="huyen_thixa_thanhpho input-block-level">
                                            <select id="listHuyen_Xa_ThanhPho" multiple="multiple" name="listHuyen_Xa_ThanhPho">
                                                <%=ViewData["huyen_thixa_thanhpho"] %>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span12">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Nội dung kiến nghị:</label>
                                    <div class="controls">
                                        <div class="input-block-level">
                                             <input <%=ViewData["NoiDung"]%> type="text" class="input-block-level" name="cNoiDungKN" id="cNoiDungKN" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--<div class="row-fluid">
                            <div class="span12">
                                <div class="control-group">
                                    <label for="textfield" class="control-label  ">Ghi chú:</label>
                                    <div class="controls">
                                        <div class="input-block-level">
                                            <input type="text" class="input-block-level" name="cNoiDung" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </div>--%>
                        <div class="row-fluid">
                            <div class="span12">
                                <div class="control-group tright">
                                    <input type="hidden" id="hidAdvancedSearch" name="hidAdvancedSearch" value="1"/>
                                    <button type="submit" class="btn btn-success"> Tra cứu</button>
                                    <%--<button type="button" class="btn btn-success" onclick="ExportRpt('xls');"><i class="icon-print"></i> In Excel</button>
                                    <button type="button" class="btn btn-success" onclick="ExportRpt('pdf');"><i class="icon-file"></i> In PDF</button>--%>
                                </div>
                            </div>
                        </div>
                    </form>
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
        $("#iDonViTiepNhan").chosen();
        $('#listHuyen_Xa_ThanhPho').multiselect({
            enableCollapsibleOptGroups: true,
            includeSelectAllOption: true,
            selectAllText: 'Chọn toàn bộ',
            allSelectedText: 'Đã chọn toàn bộ',
            nonSelectedText: 'Chọn Huyện/Thị xã/Thành phố',
            nSelectedText: 'Huyện/Thị xã/Thành phố đã chọn'
        });
        $("#listNguonKienNghi").multiselect({
           enableCollapsibleOptGroups: true,
           includeSelectAllOption: true,
           selectAllText: 'Chọn tất cả',
           allSelectedText: 'Đã chọn toàn bộ',
           nonSelectedText: 'Chọn nguồn kiến nghị',
           nSelectedText: 'nguồn kiến nghị đã chọn'
        });
        $("#listLinhVuc").multiselect({
            enableCollapsibleOptGroups: true,
            includeSelectAllOption: true,
            selectAllText: 'Chọn tất cả',
            allSelectedText: 'Đã chọn toàn bộ',
            nonSelectedText: 'Chọn lĩnh vực',
            nSelectedText: 'lĩnh vực đã chọn'
        });
    });
    
    function ChangeLinhVucByDonVi(val) {
        <%--$.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
            $("#div_linhvuc").html('<select id="listLinhVuc" name="listLinhVuc" multiple="multiple">' + data + '</select>');
            $("#listLinhVuc").multiselect({
                enableCollapsibleOptGroups: true,
               includeSelectAllOption: true,
                selectAllText: 'Chọn tất cả',
                allSelectedText: 'Đã chọn toàn bộ',
                nonSelectedText: 'Chọn lĩnh vực',
                nSelectedText: 'lĩnh vực đã chọn'
            });
        });--%>
    }
    function CheckFormSearch() {
        var pramt = $("#form_").serialize();
        
        //location.href = "/Kiennghi/Tonghop_Huyen/?" + pramt;
        $("#ip_data").empty().html("");
        $('#loadData').show();
        $.ajax({
            type: "post",
            url: "<%=Url.Action("Tonghop_Huyen", "Kiennghi")%>",
                data: pramt,
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm tập hợp kiến nghị thuộc thẩm quyền Huyện!");
                    }
                }
            });
        return false;
        
    }
</script>
