<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color box-bordered">
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
                    <input value="<%= ViewData["data-linhvuc"]%>" type="hidden" id="data-linhvuc" />
                    <div class="row-fluid">
                       <div class="span6">
                           <div class="control-group">
                               <label for="textfield" class="control-label">Mã kiến nghị:</label>
                               <div class="controls">
                                   <div class="banndannguyen-search input-block-level">
                                       <input type="text" class="input-block-level" name="cMaKienNghi" />
                                       </div>
                                   </div>
                               </div>
                           </div>
                       </div>
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
                                            <input type="text" autocomplete="off" placeholder="Ngày bắt đầu" class="span6 datepick" name="dBatDau"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div class="span6">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Đến ngày:</label>
                                    <div class="controls">
                                        <div class="input-block-level chonngay-search">
                                            <input type="text" autocomplete="off" placeholder="Ngày kết thúc" class="span6 datepick" name="dKetThuc"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    </div>
                    <% if (ViewData["dbqh"].ToString() == "0")
                        { %>
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Đơn vị tiếp nhận:</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                        <select name="iDoan"  class="chosen-select input-block-level">
                                            
                                            <%=ViewData["opt-doan"] %></select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Nguồn kiến nghị:</label>
                                <div class="controls">
                                    <div class="select-form-width-full input-block-level">
                                        <select id="listNguonKienNghi" name="listNguonKienNghi" multiple="multiple">
                                            <%=ViewData["opt-nguonkiennghi"] %>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    
                    <% } %>
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Thẩm quyền giải quyết:</label>
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
                                    <div class="select-form-width-full input-block-level" id="div_linhvuc">
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
        $("#listNguonKienNghi").multiselect({
           enableCollapsibleOptGroups: true,
           includeSelectAllOption: true,
           selectAllText: 'Chọn tất cả',
           allSelectedText: 'Đã chọn toàn bộ',
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
    });
    function ChangeLinhVucByDonVI(val) {
        <%--$.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
            $("#div_linhvuc").html('<select name="listLinhVuc" id="listLinhVuc" class="chosen-select"><option value="-1">- - - Chọn tất cả</option>' + data + '</select>');
            $("#listLinhVuc").chosen();
        });--%>
    }
    function CheckFormSearch() {
        var pramt = $("#form_").serialize();
        //location.href = "/Kiennghi/Theodoi_luu/?" + pramt;
        $("#ip_data").empty().html("");
        $('#loadData').show();
        $.ajax({
            type: "post",
            url: "<%=Url.Action("Theodoi_luu", "Kiennghi")%>",
                data: pramt,
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm kiến nghị trùng, lưu theo dõi!");
                    }
                }
            });
        return false;
    }
</script>
   