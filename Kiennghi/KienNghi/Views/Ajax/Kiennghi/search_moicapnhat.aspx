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
                                <div class="control-group control-tungay-denngay">
                                <label for="textfield" class="control-label ">Từ ngày:</label>
                                <div class="controls">
                                <div class="input-block-level">
                                    <input <%=ViewData["TuNgay"] %> type="text" class=" datepick" autocomplete="off" name="iTuNgay" id="iTuNgay" />
                                </div>
                                </div>
                            </div>
                            </div>
                            <div class ="span6">
                                <div class="control-group control-tungay-denngay">
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
                                        <%--<option value="0">- - - Chọn tất cả</option>--%>
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
                                        <%--<option value="-1">- - - Chọn tất cả</option>
                                        <option value="0">Nhiều lĩnh vực liên quan</option>--%>
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
                                <%--<button type="button" class="btn btn-" data-original-title="In Excel" onclick="XuatBaoCao('xlsx')"  rel="tooltip"> <i class="icon-print"></i>Xuất Excel</button>
                                <button type="button" class="btn btn-" data-original-title="In PDF" onclick="XuatBaoCao('pdf')"  rel="tooltip"><i class="icon-file"></i> Xuất PDF</button>--%>
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
        $.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi_ChonNhieu", 'id=' + val, function (data) {
            $("#div_linhvuc").html('<select name="iLinhVuc" id="listLinhVuc"  multiple="multiple">' + data + '</select>');
            /*$("#iLinhVuc").chosen();*/
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
        //window.location = "/Kiennghi/Moicapnhat/?" + $("#form_").serialize();
        //alert(window.location);

        var patt = /[^0-9a-zA-Z]/;
        if (!$('#iMaKienNghi').val().match(patt) || $('#iMaKienNghi').val() == "") {
            //location.href = "/Kiennghi/Moicapnhat/?" + pramt;
            $("#ip_data").empty().html("");
            $('#loadData').show();
            $.ajax({
                type: "post",
                url: "<%=Url.Action("Moicapnhat", "Kiennghi")%>",
                data: pramt,
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm kiến nghị mới cập nhật!");
                    }
                }
            });
        } else {
            alert("Mã kiến nghị không hợp lệ!");
        }
        return false;
        /*
        $("#q_data").html("<tr><td colspan=5 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
        $.post("/Kiennghi/Ajax_search_moicapnhat_result", $("#form_").serialize(), function (ok) {
            $("#q_data").html(ok);
        });
        return false;
        */
    }
    /**function XuatBaoCao(ext) {
        debugger
        var tenbaocao = $('#iTenBaoCao').val();
        var makiennghi = $('#iMaKienNghi').val();
        var dtungay = $('#iTuNgay').val();
        var ddenngay = $('#iDenNgay').val();
        var lstKyHop = $('#listKyHop').val();
        var lstNguonKN = $('#listNguonKN').val();
        var lstLinhVuc = $('#listLinhVuc').val();
        var slstKyHop = lstKyHop != undefined ? lstKyHop.join(',') : "";
        var slstNguonKN = lstNguonKN != undefined ? lstNguonKN.join(',') : "";
        var slstLinhVuc = lstLinhVuc != undefined ? lstLinhVuc.join(',') : "";
        var iThamQuyenDonVi = $('#iDonViXuLy').val();
        var iDonViTiepNhan = $('#iDoan').val();

        var loaibaocao = $('#iLoaiBaoCao').val();
        var url = "";
        if (tenbaocao == 1) {
            url = "/Kiennghi/Baocao_DanhMucCuTri1A?lstKyHop=" + slstKyHop + "&lstNguonKN=" + slstNguonKN + "&lstLinhVuc=" + slstLinhVuc + "&loaibaocao=" + loaibaocao + "&ext=" + ext;
            
        }
        else if (tenbaocao == 2) {
            url = "/Kiennghi/BaoCao_TongHopYKienCuTri1B?lstKyHop=" + slstKyHop + "&lstNguonKN=" + slstNguonKN + "&lstLinhVuc=" + slstLinhVuc + "&loaibaocao=" + loaibaocao + "&ext=" + ext;
        }
        else if (tenbaocao == 4) {
            url = "/Kiennghi/BaoCao_TongHopYKienCuTri1B2?lstKyHop=" + slstKyHop + "&lstNguonKN=" + slstNguonKN + "&lstLinhVuc=" + slstLinhVuc + "&loaibaocao=" + loaibaocao + "&ext=" + ext;
        }
        window.open(url, '_blank');

    }**/
</script>
