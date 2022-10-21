<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Thêm mới lịch tiếp công dân định kỳ
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">  
                                
                               <table class="table form4 table-condensed">
                                  
                                <tr>
                                        <th colspan="2">Thông tin lịch tiếp dân</th>
                                    </tr>
                                    <tr>
                                        <td class="" style="width:5% !important">Ngày tiếp <i class="f-red">*</i></td>
                                        <td ><input type="text" name="dNgayTiep" id="dNgayTiep" class="input-medium datepick " /></td>
                                    
                                      
                                    </tr>
                                   <tr>
                                    <td  >Địa chỉ tiếp <i class="f-red">*</i></td>
                                    <td >
                                        <p class="span6"><select name="iDiaPhuong_0" onchange="ChangeTinhThanh_DinhKy('iDiaPhuong_1',this.value)" id="iDiaPhuong_0" class="chosen-select">
                                            <option value="0">Chọn tỉnh thành</option><%=ViewData["opt-tinh"] %>
                                            <option value="-1">Khác</option>
                                           </select></p>
                                        <p class="span6" id="iDiaPhuong_1"><select name="iDiaPhuong_1"  id="iDiaPhuong_01"  ><option value="0">Chọn huyện/thành phố/thị xã</option></select></p>
                                        <p class="clear"><input autocomplete="off" type="text" class="span6" name="cDiaDiem"  id="cDiaDiem" placeholder="Số nhà, đường..." /></p>
                                        
                                    </td>
                                </tr>
                        

                                    <tr>
                                        <td colspan="2" class="tcenter">
                                            <input type="hidden" id="check_user" name="check_user" value="0" />
                                            <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                            <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
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
        if ($("#iDiaPhuong_0").val() == "") {
            alert("Vui lòng chọn tỉnh"); $("#iDiaPhuong_0").focus(); return false;
        }
        $.post("/Tiepdan/Ajax_Dinhky_insert", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
            } else {
                alert("Lịch tiếp đã tồn tại, Vui lòng kiểm tra lại!");
            }
        });
        
        return false;
    }

    function CheckDiaDiem() {
        if ($("#cUsername").val() != "") {
            $.post("<%=ResolveUrl("~")%>Tiepdan/Ajax_Dinhky_kiemtrung",
                "dNgayTiep=" + $("#dNgayTiep").val() + "&cDiaDiem=" + $("#cDiaDiem").val(),
                function (data) {
                    $("#check_user").val(data);
                    if (data == 1) {
                        alert("Lịch tiếp định kỳ này đã tồn tại. Vui lòng nhập mới");
                        return false;
                    } 
                }
            );
        }
    }
    function ChangeTinhThanh_DinhKy(id_change, val) {
        $.post("/Home/Ajax_Change_Opt_tinhthanh", 'id=' + val, function (data) {
            $("#" + id_change).html(data);
            $("#iDiaPhuong_01").chosen();
        });

    }
</script>
