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
                                    <div class="banndannguyen-search input-block-level">
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
                                    <div class="select-form-width-full input-block-level">
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
                                        <div class="input-block-level chonngay-search">
                                            <input <%=ViewData["TuNgay"] %> type="text" class=" datepick" autocomplete="off" name="dBatDau" id="dBatDau" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div class="span6">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Đến ngày:</label>
                                    <div class="controls">
                                        <div class="input-block-level chonngay-search">
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
                                    <div class="nguonkiennghi input-block-level">
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
                                    <div class="select-form-width-full input-block-level">
                                        <select id="iDonViTiepNhan" name="iDonViTiepNhan">
                                            <%=ViewData["opt-donvitiepnhan"] %>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Lĩnh vực:</label>
                                <div class="controls">
                                    <div class="linhvuc input-block-level" id="div_linhvuc">
                                       <select id="listLinhVuc" name="listLinhVuc" multiple="multiple">
                                           <%--<option value="-1">- - - Chọn tất cả</option>--%>
                                           <%--<option value="0">Nhiều lĩnh vực liên quan</option>--%>
                                           <%=ViewData["opt-linhvuc"] %>
                                           </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                    <div class="row-fluid">
                    <%--<div class="span6">
                        <div class="control-group">
                            <label for="textfield" class="control-label  ">Chọn Huyện/Thị xã/Thành phố:</label>
                            <div class="controls">
                                <div class="input-block-level">
                                <select id="listHuyen_Xa_ThanhPho" multiple="multiple" name="listHuyen_Xa_ThanhPho">
                                    <%=ViewData["huyen_thixa_thanhpho"] %>
                                </select>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                    <div class="span6">
                        <div class="control-group">
                            <label for="textfield" class="control-label">Thẩm quyền giải quyết<i class="f-red">*</i>:</label>
                                    <div class="controls">
                                        <%--<%=ViewData["radio-thamquyen"] %>--%>
                                        <div class="select-form-width-full input-block-level">
                                            <select onchange="ChangeLinhVucByDonVI(this.value)" id="iDonViXuLy" name="iDonViXuLy" class="chosen-select">
                                                <%=ViewData["opt-donvithamquyen"] %>
                                            </select>
                                        </div>
                                    </div>
                            </div>
                        </div>
                    
                    <%--<div class="control-group">
                            <label for="textfield" class="control-label  ">Thẩm quyền giải quyết:</label>
                            <div class="controls">
                                <div class="input-block-level">
                                    <select onchange="ChangeLinhVucByDonVI(this.value)" name="iDonViXuLy" class="chosen-select input-block-level">                                    
                                        <option value="0">- - - Chọn tất cả</option>
                                        <%=ViewData["opt-donvixuly"] %></select>
                                </div>
                            </div>
                        </div>--%>
                </div>  
                    <div class="row-fluid">
                        <div class="span12">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Nội dung kiến nghị:</label>
                                <div class="controls">
                                    <div class="banndannguyen-search input-block-level">
                                        <input <%=ViewData["NoiDung"]%> type="text" class="input-block-level" name="cNoiDungKN" id="cNoiDungKN" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    <%--<div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Tên báo cáo:</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                        <select name="iTenBaoCao" class="chosen-select input-block-level">                                    
                                            <%=ViewData["opt-tenbaocao"] %></select>
                                    </div>
                                </div>
                            </div>
                        </div>--%>
                    <%--<div class="row-fluid">
                        <div class="span12">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Ghi chú:</label>
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
                                <input type="hidden" value="1" name="hidechecksearch" />
                                <input type="hidden" id="hidAdvancedSearch" name="hidAdvancedSearch" value="1"/>
                                <button type="submit" class="btn btn-success"> Tra cứu</button>
                                <%--<button type="button" onclick="print('xls')" class="btn btn-success"><i class="icon-print"></i> In Excel </button>
                                <button type="button" onclick="print('pdf')" class="btn btn-success"><i class="icon-file"></i> In PDF </button>--%>
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
        <%--$('#listHuyen_Xa_ThanhPho').multiselect({
            enableCollapsibleOptGroups: true,
            includeSelectAllOption: true,
            selectAllText: 'Chọn toàn bộ',
            allSelectedText: 'Đã chọn toàn bộ',
            nonSelectedText: 'Chọn Huyện/Thị xã/Thành phố',
            nSelectedText: 'Huyện/Thị xã/Thành phố đã chọn'
        });--%>
        $('#listLinhVuc').multiselect({
            enableCollapsibleOptGroups: true,
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
    function ChangeLinhVucByDonVI(val) {
        <%--$.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
            $("#div_linhvuc").html('<select id="iLinhVuc" name="iLinhVuc" multiple="multiple">' + data + '</select>');
            $("#iLinhVuc").multiselect({
               enableCollapsibleOptGroups: true,
               includeSelectAllOption: true,
               selectAllText: 'Chọn tất cả',
               allSelectedText: 'Đã chọn toàn bộ',
               nonSelectedText: 'Chọn lĩnh vực',
               nSelectedText: 'lĩnh vực đã chọn'
            });
        });--%>
    }
    function CheckThamQuyen() {
        var check = $("input[type='radio'][name='iThamQuyen']:checked").val();
        if (check == 1) {
            $("#TrungUong").show();
            //$("#DiaPhuong").hide();
        } else {
            $("#TrungUong").hide();
            //$("#DiaPhuong").show();
        }
    }
    function DoiThamQuyenDonVi(val) {
        $.post("/Kiennghi/Ajax_Get_ThamQuyen_DonVi", '&val=' + val, function (data) {
            $("#TrungUong").show();
            $("#iDonViXuLy").html('<select name="iDonViXuLy" id="iDonViXuLy" class="chosen-select">' + data + '</select>');
            $("#iDonViXuLy").trigger("liszt:updated");
            $("#iDonViXuLy").chosen();
        });
    }

    function CheckFormSearch() {
        var pramt = $("#form_").serialize();
        //window.location = "/Kiennghi/Tonghop_bandannguyen/?" + $("#form_").serialize();
        //alert(window.location);
        //location.href = "/Kiennghi/Tonghop_TW/?" + pramt;
        $("#ip_data").empty().html("");
        $('#loadData').show();
        $.ajax({
            type: "post",
            url: "<%=Url.Action("Tonghop_TW", "Kiennghi")%>",
                data: pramt,
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm tập hợp kiến nghị thuộc thẩm quyền TW!");
                    }
                }
            });
        return false;
        
    }
    
</script>
