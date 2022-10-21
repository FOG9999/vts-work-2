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
								<i class="icon-print"></i> In báo cáo đơn đã chuyển xử lý, chưa có văn bản trả lời
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content">
                            <form method="post" name="form_" id="form_" class="form-horizontal form-column">
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="span6">
                                            <div class="control-group">
                                                <label for="textfield" class="control-label">Chọn năm<i class="f-red">*</i>:</label>
                                                <div class="controls">
                                                    <div class="chonngay-search input-block-level">
                                                        <select onchange="setDay()" class="chosen-select year-pick" name="iYear" id="iYear">
                                                             <%=ViewData["opt-yearlist"] %>
                                                        </select>
                                                        <%--
                                                        <input type="text" autocomplete="off" placeholder="Ngày bắt đầu" class="span6 datepick" name="dBatDau"/>
                                                    --%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label for="textfield" class="control-label">Chọn tháng<i class="f-red">*</i>:</label>
                                                <div class="controls">
                                                    <div class="chonngay-search input-block-level">
                                                        <select onchange="setDay()" class="chosen-select month-pick" name="iMonth" id="iMonth">
                                                             <%=ViewData["opt-monthlist"] %>
                                                        </select>                                                    
                                                        </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="span6">
                                            <div class="control-group">
                                                <label for="textfield" class="control-label">Từ ngày:</label>
                                                <div class="controls">
                                                    <div class="chonngay-search input-block-level">
                                                        <input type="text" autocomplete="off" placeholder="Ngày bắt đầu" class="span6 pick-date datepick" id="dBatDau" name="dBatDau"/>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label for="textfield" class="control-label">Đến ngày:</label>
                                                <div class="controls">
                                                    <div class="chonngay-search input-block-level">
                                                        <input type="text" autocomplete="off" placeholder="Ngày kết thúc" class="span6 pick-date datepick" id="dKetThuc" name="dKetThuc"/>
                                                    </div>
                                                </div>
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
                                <div class="row-fluid">
                                    <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Loại đơn:</label>
                                        <div class="controls">
                                            <div class="input-block-level banndannguyen-popup" id="ip_loaidon">
                                                <select class="chosen-select" id="iLoaiDon" name="iLoaiDon">
                                                    <option value="0">- - - Chọn tất cả</option>
                                                    <%=ViewData["opt-loaidon"] %>
                                                </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                         <div class="control-group ">
                                                 <label for="textfield" class="control-label ">Tên báo cáo:</label>
                                                 <div class="controls">
                                                     <div class="input-block-level banndannguyen-popup">
                                                         <select class="chosen-select" name="iTenBaoCao" id="iTenBaoCao" onchange="changeTenBaoCao(this.value)">
                                                             <%=ViewData["opt-tenbaocao"] %>
                                                         </select>
                                                         </div>
                                                 </div>
                                             </div>
                                         </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Đối tượng:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select id="iDoiTuong" name="iDoiTuong" class="chosen-select input-block-level">                                    
                                                        <%=ViewData["opt-doituong"] %>
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
           $("#iLoaiDon").chosen();
           $("#iTenBaoCao").chosen();
           $("#iDoiTuong").chosen();
    });
    function print(ext) {
        if ($("#iTenBaoCao").val() == 1 && $("#iMonth").val() == 0 || $("#iYear").val() == 0) {
            alert("Danh sách chuyển đơn theo tháng yêu cầu nhập đầy đủ năm và tháng!");
            return;
        }
        if (($("#iTenBaoCao").val() == 5 || $("#iTenBaoCao").val() == 2 ) && ($("#dBatDau").val() == '' || $("#dKetThuc").val() == '')) {
            alert("Danh sách chuyển đơn yêu cầu nhập ngày bắt đầu và ngày kết thúc!");
            return;
        }
        var pramt = $("#form_").serialize();
        if ($("#iTenBaoCao").val() == 1) window.open("/Kntc/BaoCaoMoiTonghop_ChuaTraLoi/?ext=" + ext + "&" + pramt, "_blank");
        if ($("#iTenBaoCao").val() == 2) window.open("/Kntc/BaoCaoDanhSachChuyenDonTheoKhoangTg_3C/?ext=" + ext + "&" + "iDoiTuong=" + $("#iDoiTuong").val() + "&dBatDau=" + $("#dBatDau").val() + "&dKetThuc=" + $("#dKetThuc").val() + "&iLoaiDon=" + $("#iLoaiDon").val(), "_blank");
        if ($("#iTenBaoCao").val() == 3) window.open("/Kntc/BaoCaoDanhSachChuyenDonTheoLinhVuc_3D/?ext=" + ext + "&" + pramt, "_blank");
        if ($("#iTenBaoCao").val() == 5) window.open("/Kntc/BaoCaoDanhSachCongVanDonDoc_3F/?ext=" + ext + "&" + "iDoiTuong=" + $("#iDoiTuong").val() + "&dBatDau=" + $("#dBatDau").val() + "&dKetThuc=" + $("#dKetThuc").val(), "_blank");
        return false;
    }
    function setDay() {
        console.log($("#iYear").val())
        console.log($("#iMonth").val())
        if ($("#iMonth").val() == 0 || $("#iYear").val() == 0) {
            $("#dBatDau").prop('readonly', false);
            $("#dKetThuc").prop('readonly', false);
            $( "#dBatDau" ).datepicker('add');
            $( "#dKetThuc" ).datepicker('add');
            return;
        }
        $("#dBatDau").prop('readonly', true);
        $("#dKetThuc").prop('readonly', true);
        $( "#dBatDau" ).removeClass('datepicker');
        $( "#dKetThuc" ).removeClass('datepicker');
        $( "#dBatDau" ).datepicker('remove');
        $( "#dKetThuc" ).datepicker('remove');
        $('#dBatDau').val($.date(new Date($("#iYear").val(), $("#iMonth").val() - 1, 1)));
        $('#dKetThuc').val($.date(new Date($("#iYear").val(), $("#iMonth").val(), 0).toString("MM/dd/yyyy")));
        
    }
    
    $.date = function(dateObject) {
        var d = new Date(dateObject);
        var day = d.getDate();
        var month = d.getMonth() + 1;
        var year = d.getFullYear();
        if (day < 10) {
            day = "0" + day;
        }
        if (month < 10) {
            month = "0" + month;
        }
        var date = day + "/" + month + "/" + year;
    
        return date;
    };

    function changeTenBaoCao(val) {
        
        if (val == 5) {
            $("#ip_loaidon").html('<select class="chosen-select" id="iLoaiDon" name="iLoaiDon"><option value = "0" > - - - Chọn tất cả</option></select> ');
            $("#iLoaiDon").chosen();
        } else {
            if (val == 3) {
                $.post("/Kntc/Ajax_Set_NamHienTai", function (data) {
                    console.log(data);
                    $("#iYear").html('<select onchange="setDay()" class="chosen-select year-pick" name="iYear" id="iYear">'+data+'</select> ');
                });
            }
            else {
                $.post("/Kntc/Ajax_Change_LoaiDonTheoLoaiBaoCao", function (data) {
                            // console.log(data);
                    $("#ip_loaidon").html('<select class="chosen-select" id="iLoaiDon" name="iLoaiDon"><option value = "0" > - - - Chọn tất cả</option>'+data+'</select> ');
                    $("#iLoaiDon").chosen();
                });
            }
        }
    }
        
</script>
<style>

</style>
