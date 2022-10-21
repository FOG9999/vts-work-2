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
								<i class="icon-reorder"></i> Thêm mới cơ quan
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
							        <label for="textfield" class="control-label">Mã cơ quan</label>
							        <div class="controls">
                                        <input type="text" value="" name="cCode" id="cCode" class="input-block-level" onchange="kiemtrungma()"  />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Tên cơ quan <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="" name="cTen" id="cTen" class="input-block-level" onchange="kiemtrungten()" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label " >Thuộc cấp cơ quan <i class="f-red">*</i></label>
							        <div class="controls">
                                        <select name="iParent" id="iParent" class="input-block-level">
                                            <option value='0'>- - - Gốc</option>
                                            <%=ViewData["opt-donvi"] %>
                                        </select>
							        </div>
						        </div> 
                                
                                <div class="control-group">
							        <label for="textfield" class="control-label">Địa phương</label>
							        <div class="controls">
                                        <select class="input-block-level" name="iDiaPhuong" id="iDiaPhuong">
                                            <option value="0">--- Chọn địa phương tương ứng</option>
                                            <%=ViewData["opt-tinh"] %>
                                        </select>
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
    $("#iParent").chosen();
    $("#iDiaPhuong").chosen();
    function CapNhat() {
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên đơn vị"); $("#cTen").focus(); return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Coquan_insert", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Thêm mới thành công")
            }
            else if(ok==2)
            {
                alert("Mã cơ quan đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
            else {
                alert("Tên cơ quan đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;        
    }
   
</script>
