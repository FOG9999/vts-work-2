<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Cập nhật tiếp công dân định kỳ
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% TIEPDAN_DINHKY t = (TIEPDAN_DINHKY)ViewData["dinhky"]; %>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">  
                                <table class="table form4 table-condensed">
                                  
                                <tr>
                                        <th colspan="2">Thông tin tiếp dân <i class="f-red">*</i></th>
                                    </tr>
                                    <tr>
                                        <td  style="width:5% !important">Ngày tiếp <i class="f-red">*</i></td>
                                        <td ><input type="text" name="dNgayTiep" id="dNgayTiep" class="input-medium datepick "  value="<%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(t.DNGAYTIEP)) %>"/></td>
                                    
                                      
                                    </tr>
                                    <tr>
                                    <td  >Địa chỉ tiếp <i class="f-red">*</i></td>
                                    <td >
                                        <p class="span6"><select name="iDiaPhuong_0" onchange="ChangeTinhThanh_DinhKy('iDiaPhuong_1',this.value)" id="iDiaPhuong_0" class="chosen-select">
                                            <option value="0">Chọn tỉnh thành</option><%=ViewData["opt-tinh"] %>
                                            <option value="-1">Khác</option>
                                           </select></p>
                                        <p class="span6" id="iDiaPhuong_1"><select name="iDiaPhuong_1" id="iDiaPhuong_01"  class="chosen-select "><option value="0">Chọn huyện/thành phố/thị xã</option> <%= ViewData["opt-huyen"] %></select></p>
                                        <p class="clear"><input autocomplete="off" type="text" class="span6" name="cDiaDiem"  id="cDiaDiem" placeholder="Số nhà, đường..." value="<%=t.CDIADIEM %>" /></p>
                                        
                                    </td>
                                </tr>
                        

                                    <tr>
                                        <tr>
                                        <td colspan="2" class="tcenter">
                                            <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                            <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                            <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
                                        </td>
                                    </tr>
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
<script type="text/javascript">
    $("#iDiaPhuong_0").chosen();
    $("#iDiaPhuong_01").chosen();
    function CapNhat() {
        // alert($("#cDiaDiem").val());
        if ($("#dNgayTiep").val() == "") { alert("Vui lòng chọn ngày tiếp dân!"); $("#dNgayTiep").focus(); return false; }
        if (!Validate_DateVN("dNgayTiep")) {
            return false;
        }
        if ($("#cDiaDiem").val() == "") {
            alert("Vui lòng nhập địa điểm tiếp dân"); $("#cDiaDiem").focus(); return false;
        }
        $.post("/Tiepdan/Ajax_Dinhky_update", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
            } else {
                alert("Lịch tiếp đã tồn tại, Vui lòng kiểm tra lại!");
            }
        });

        return false;
    }
    function ChangeTinhThanh_DinhKy(id_change, val) {
        $.post("/Home/Ajax_Change_Opt_tinhthanh", 'id=' + val, function (data) {
            $("#" + id_change).html(data);
            $("#iDiaPhuong_01").chosen();
        });

    }
</script>
