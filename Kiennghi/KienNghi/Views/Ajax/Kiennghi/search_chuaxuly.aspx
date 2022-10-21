<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color box-bordered">
			<div class="box-title">
				<h3>
					<i class="icon-search"> </i> Tìm kiếm Tập hợp kiến nghị chưa xử lý
				</h3>
                
            </div>
            <div class="box-content">
                <form id="form_" method="post" onsubmit="return CheckFormSearch();" class="form-horizontal form-column">  

                    <%--Mã kiến nghị --%>
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="control-group">
                                 <label for="textfield" class="control-label">Mã kiến nghị:</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                         <input type='text' class='input-block-level' id='iMaKienNghi' name='iMaKienNghi' />
                                        
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

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
                    <%--Bổ sung tìm kiếm từ ngày đến ngày--%>
                    <div class="row-fluid">
                        <div class="span6">
                            <div class ="span6">
                                <div class="control-group">
                                <label for="textfield" class="control-label ">Từ ngày:</label>
                                <div class="controls">
                                <div class="input-block-level">
                                    <input <%=ViewData["TuNgay"] %> type="text" class=" datepick" autocomplete="off" name="iTuNgay" id="iTuNgay" />
                                </div>
                                </div>
                            </div>
                            </div>
                            <div class ="span6">
                                <div class="control-group control-group-denNgay">
                                <label for="textfield" class="control-label"> Đến ngày:</label>
                                <div class="controls">
                                <div class="input-block-level">
                                    <input <%=ViewData["DenNgay"] %> type="text" class="datepick" autocomplete="off" name="iDenNgay" id="iDenNgay" />
                                </div>
                                </div>
                            </div>
                            </div>
                             
                        </div>
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Nguồn kiến nghị:</label>
                                <div class="controls">
                                    <div class="input-block-level select-form-right">
                                    <select id="listHuyen_Xa_ThanhPho" multiple="multiple" name="listHuyen_Xa_ThanhPho">
                                        <%=ViewData["huyen_thixa_thanhpho"] %>
                                    </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid">
                        
                        
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Đơn vị tiếp nhận:</label>
                                <div class="controls">
                                <div class="input-block-level">
                                    <select name="iDoan" id="iDoan"  class="chosen-select input-block-level">
                                        <%--<option value="0">- - - Chọn tất cả</option>--%>
                                        <%=ViewData["opt-doan"] %>

                                    </select>
                                </div>
                                </div>
                            </div>
                        </div>
                    
                        
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Thẩm quyền xử lý:</label>
                                <div class="controls">
                                    <div class="input-block-level  select-form-right">
                                        <select name="iDonViXuLy" class="chosen-select input-block-level">                                    
                                        <%--<select onchange="ChangeLinhVucByDonVI(this.value)" name="iDonViXuLy" class="chosen-select input-block-level">--%>                                    
                                            <option value="0">- - - Chọn tất cả</option>
                                            <%=ViewData["opt-donvixuly"] %>
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
                                <label for="textfield" class="control-label">Nội dung, từ khóa:</label>
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
                                <input type="hidden" id="hidAdvancedSearch" name="hidAdvancedSearch" value="1"/>
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
    });
    function ChangeLinhVucByDonVI(val) {
        //alert(val);
        $.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
            //alert(data);
            $("#div_linhvuc").html('<select name="iLinhVuc" id="iLinhVuc" class="chosen-select"><option value="-1">- - - Chọn tất cả</option>' + data + '</select>');
            $("#iLinhVuc").chosen();
        });
    }
    function CheckFormSearch() {
        var pramt = $("#form_").serialize();
        //location.href = "/Kiennghi/Chuaxuly/?q=" + $("#q").val() + "&" + pramt;
        $("#ip_data").empty().html("");
        $('#loadData').show();
        $.ajax({
            type: "post",
            url: "<%=Url.Action("Chuaxuly", "Kiennghi")%>",
                data: pramt,
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm tập hợp chưa xử lý!");
                    }
                }
            });
        return false;
    }
</script>
   