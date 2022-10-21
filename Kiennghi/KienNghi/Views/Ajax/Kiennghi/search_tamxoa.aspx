<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color box-bordered">
			<div class="box-title">
				<h3>
					<i class="icon-search"> </i> Tìm kiếm kiến nghị
				</h3>
            </div>
            <div class="box-content">
                <form id="form_" method="post" onsubmit="return CheckFormSearch();" class="form-horizontal form-column">  
                    <input value="<%= ViewData["data-linhvuc"]%>" type="hidden" id="data-linhvuc" />
                     <%--Bổ sung tìm kiếm mã kiến nghị--%>
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
                                <label for="textfield" class="control-label ">Chọn kỳ họp</label>
                                <div class="controls">
                                    <div class="input-block-level">
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
                                <label for="textfield" class="control-label  ">Nguồn kiến nghị:</label>
                                <div class="controls">
                                <div class="input-block-level  select-form-right">
                                    <select name="listNguonKN" id="listNguonKN" multiple="multiple">
                                        <%=ViewData["opt-nguonkiennghi"] %>
                                    </select>
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row-fluid">
                        
                    </div>
                    
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Đơn vị tiếp nhận:</label>
                                <div class="controls">
                                <div class="input-block-level">
                                    <select name="iDoan" id="iDoan"  class="chosen-select input-block-level">
                                        <%=ViewData["opt-doan"] %>

                                    </select>
                                </div>
                                </div>
                            </div>
                        </div>
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Lĩnh vực:</label>
                                <div class="controls">
                                    <div class="input-block-level select-form-right" id="div_linhvuc">
                                        <select name="listLinhVuc" id="listLinhVuc"  multiple="multiple" >
                                        <%=ViewData["opt-linhvuc"] %></select>
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
                                        <select onchange="ChangeLinhVucByDonVI(this.value)" id="iDonViXuLy" name="iDonViXuLy" class="chosen-select input-block-level">                                    
                                            <option value="0">- - - Chọn tất cả</option>
                                            <%=ViewData["opt-donvixuly"] %></select>
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
                                        <input <%=ViewData["NoiDung"]%> type="text" class="input-block-level" name="cNoiDung" id="iNoiDung" />
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
            nSelectedText: 'kỳ họp đã chọn'
        });
        $('#listLinhVuc').multiselect({
            enableCollapsibleOptGroups: true,
            includeSelectAllOption: true,
            selectAllText: 'Chọn tất cả',
            allSelectedText: 'Đã chọn tất cả',
            nonSelectedText: 'Chọn lĩnh vực',
            nSelectedText: 'Lĩnh vực đã chọn'
        });
    });
    function ChangeLinhVucByDonVI(val) {
        $.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi_ChonNhieu", 'id=' + val, function (data) {
            $("#div_linhvuc").html('<select name="iLinhVuc" id="listLinhVuc"  multiple="multiple">' + data + '</select>');
            $('#listLinhVuc').multiselect({
                enableCollapsibleOptGroups: true,
                includeSelectAllOption: true,
                selectAllText: 'Chọn toàn bộ',
                allSelectedText: 'Đã chọn toàn bộ',
                nonSelectedText: 'Chọn lĩnh vực',
                nSelectedText: 'Lĩnh vực đã chọn'
            });
        });
    }
    function CheckFormSearch() {
        var pramt=$("#form_").serialize();

        var patt = /[^0-9a-zA-Z]/;
        if (!$('#iMaKienNghi').val().match(patt) || $('#iMaKienNghi').val() == "") {
            //location.href = "/Kiennghi/Tamxoa/?" + pramt;
            $("#ip_data").empty().html("");
            $('#loadData').show();
            $.ajax({
                type: "post",
                url: "<%=Url.Action("Tamxoa", "Kiennghi")%>",
                data: pramt,
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm kiến nghị tạm xóa!");
                    }
                }
            });
        } else {
            alert("Mã kiến nghị không hợp lệ!");
        }
        return false;
    }
</script>
