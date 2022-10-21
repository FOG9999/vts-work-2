<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color box-bordered">
			<div class="box-title">
				<h3>
					<i class="icon-search"> </i> Tìm kiếm chương trình tiếp xúc cử tri
				</h3>
               <%-- <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>--%>
            </div>
            <div class="box-content">
                <form id="form_" method="post" onsubmit="return CheckFormSearch();" class="form-horizontal form-column">     
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Chọn kỳ họp</label>
                                <div class="controls">
                                    <div class="input-block-level">
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
                                    <%=ViewData["check-hinhthuc"] %>
                                </div>
                            </div>
                        </div>
                    </div>  
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label  ">Bắt đầu từ</label>
                                <div class="controls">
                                    <div class="input-block-level">
                                        <input type="text" autocomplete="off" placeholder="ngày bắt đầu" class="span6 datepick" name="dBatDau"/>
                                        <input type="text" autocomplete="off" placeholder="ngày kết thúc" class="span6 datepick" name="dKetThuc"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <% if (ViewData["is_dbqh"].ToString() == "0")
                            { %>
                        <div class="span6">
                            <div class="control-group">
                                <label for="textfield" class="control-label">Đoàn lập kế hoạch:</label>
                                <div class="controls">
                                    <div class="input-block-level" id="div_linhvuc">
                                        <select name="iDoan" id="iDoan" onchange="ChangeDoan(this.value)" class="chosen-select input-block-level"><%=ViewData["opt-donvi"] %></select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <% } %>
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
    //function ChangeDoan(val) {
    //    $.post("/Kiennghi/Ajax_search_chuongtrinh_change_doan", "id=" + val, function (ok) {
    //        alert(ok);
    //        $("#iDiaPhuong").html(ok);
    //    });
    //    $.post("/Kiennghi/Ajax_search_chuongtrinh_change_daiphuong", "id=" + val, function (ok) {
    //        $("#iDaiBieu").html(ok);
    //    });
    //}
    $(document).ready(function () {
        $('#listKyHop').multiselect({
            enableCollapsibleOptGroups: true,
            includeSelectAllOption: true,
            selectAllText: 'Chọn toàn bộ',
            allSelectedText: 'Đã chọn toàn bộ',
            nonSelectedText: 'Chọn kỳ họp',
            nSelectedText: 'kỳ họp đã chọn'
        });
    });
    function CheckFormSearch() {
        //$("#q_data").html("<tr><td colspan=5 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
        //$.post("/Kiennghi/Ajax_search_chuongtrinh_result", $("#form_").serialize(), function (ok) {
        //    $("#q_data").html(ok);
        //});
        //return false;
        var pramt = $("#form_").serialize();
        //window.location = "/Kiennghi/Chuongtrinh/?" + $("#form_").serialize();
        //location.href = "/Kiennghi/Chuongtrinh/?" + pramt;
        $("#ip_data").empty().html("");
        $('#loadData').show();
        $.ajax({
            type: "post",
            url: "<%=Url.Action("Chuongtrinh", "Kiennghi")%>",
                data: pramt,
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm chương trình tiếp xúc cử tri!");
                    }
                }
            });
        return false;
    }
</script>
   