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
								<i class="icon-reorder"></i> Thêm mới kiến nghị lĩnh vực
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                
                                 <div class="control-group">
                                    <label for="textfield" class="control-label">Mã lĩnh vực</label>
                                    <div class="controls">
                                        <input type="text" value="" name="cCode" id="cCode" class="input-medium" />
                                    </div>
                                </div>
                                 <div class="control-group">
							        <label for="textfield" class="control-label ">Tên loại lĩnh vực<i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value=""  name="cTen" id="cTen" class="input-block-level" />
							        </div>
						        </div>
                                   <div class="control-group">
							        <label for="textfield" class="control-label " >Thuộc cấp cơ quan <i class="f-red">*</i></label>
							        <div class="controls">
                                        <select name="icoquan" id="icoquan" class="input-block-level">
                                            <option value='0'>- - - Chọn cơ quan</option>
                                            <%=ViewData["opt-donvi"] %>
                                        </select>
							        </div>
                                   <div class="control-group">
							        <label for="textfield" class="control-label " >Lĩnh vực cha </label>
							        <div class="controls">
                                        <select name="iparent" id="iparent" class="input-block-level" >
                                            <option value='0'>- - - Gốc</option>
                                        </select>
							        </div>
						        </div> 
						        </div>
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                    <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
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
    var listLinhVucCoQuan = JSON.parse(`<%= ViewData["listLinhVucCoQuan"]%>`);
    $("#icoquan").chosen();
    function CapNhat() {
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên lĩnh vực "); $("#cTen").focus();
            return false;
        }
        if ($("#icoquan").val() == 0) {
            alert("Vui chọn cơ quan "); $("#icoquan").focus();
            return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Linhvuc_Coquan_insert", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Thêm mới thành công")
            } else {
                alert("Tên lĩnh vực đã tồn tại.");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;
    }
    $("#icoquan").change(function () {
        $("#iparent option[value!=0]").remove();
        $("#iparent").trigger("liszt:updated");
        var coQuanDuocChon = $("#icoquan").val();
        if (coQuanDuocChon != 0) {
            $("#iparent").prop("disabled", false);
            var str = "";
            var filteredList = [];
            for (linhVucCoQuan of listLinhVucCoQuan) {
                if (linhVucCoQuan.ICOQUAN == coQuanDuocChon) {
                    $('#iparent').append($('<option>',
                        {
                            value: linhVucCoQuan.ILINHVUC,
                            text: linhVucCoQuan.CCODE + ' - - ' +linhVucCoQuan.CTEN
                        }));
                    $("#icoquan").chosen();
                    $("#icoquan").trigger("liszt:updated");
                }
            }
            $("#iparent").chosen();
            $("#iparent").trigger("liszt:updated");
        }           
    });
    
</script>