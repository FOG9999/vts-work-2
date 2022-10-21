<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid" >
                <div class="span12">
                    <div class="box box-color">
                        <div class="box box-color ">
                            <div class="box-title">
				                <h3>
					                <i class="icon-print"> </i> In báo cáo đơn chờ xử lý
				                </h3>
                                <ul class="tabs">
						            <li class="active">
							            <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
						            </li>
                                </ul>
                            </div>

                            <div class="box-content popup_info"  style="padding: 0px !important">
                                <form id="form_" method="post" >
                                    <table class="table table-bordered form4">
                                        <tr>
                                            <td class="">Ngày nhận đơn</td>
                                            <td>
                                                <input type="text" name="dTuNgay" placeholder="Từ ngày" class="input-medium datepick" style="width: 50%"/>
                                                <input type="text" name="dDenNgay" placeholder="Đến ngày" class="input-medium datepick" style="width: 49%"/>
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td>Loại báo cáo</td>
                                            <td>
                                                <select id="iLoaiBaoCao" name="iLoaiBaoCao" class="chosen-select">
                                                    <%=ViewData["opt-loaibaocao"] %>
                                                </select>
                                            </td>
                                            <td>Tên báo cáo</td>
                                            <td>
                                                <select id="iTenBaoCao" name="iTenBaoCao" class="chosen-select">
                                                    <%=ViewData["opt-tenbaocao"] %>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td colspan="3" class="tright">
                                                <input type="hidden" id="hidAdvancedSearch" name="hidAdvancedSearch" value="1"/>
                                                
                                                <button type="button" class="btn btn-success" onclick="ExportRpt('xls');"><i class="icon-print"></i> In Excel</button>
                                                <button type="button" class="btn btn-success" onclick="ExportRpt('pdf');"><i class="icon-file"></i> In PDF</button>
                                            </td>
                                        </tr>
                                    </table>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script>
   
    $("#iTenBaoCao").chosen();
    $("#iLoaiBaoCao").chosen();
    

    function ExportRpt(ext) {
        var loaibaocao = $("#iLoaiBaoCao").val();
        var tenbaocao = $("#iTenBaoCao").val();
        var dTuNgay = $("input[name='dTuNgay']").val();
        var dDenNgay = $("input[name='dDenNgay']").val();

        var url = "/Kntc/BaoCaoDonChoXuLiPhanLoai?ext=" + ext + "&loaibaocao=" + loaibaocao
            + "&tenbaocao=" + tenbaocao + "&dTuNgay=" + dTuNgay + "&dDenNgay=" + dDenNgay;
        setTimeout(function () { window.open(url, '_blank'); }, 1000);
    }
</script>
