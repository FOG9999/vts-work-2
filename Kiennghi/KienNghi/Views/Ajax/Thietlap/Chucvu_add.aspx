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
								<i class="icon-reorder"></i> Thêm mới chức vụ
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
                                    <label for="textfield" class="control-label">Mã chức vụ</label>
                                   
                                    <div class="controls">
                                        <input type="text"  name="cCode" id="cCode" class="input-medium"/>
                                    </div>
                                </div>
                                 <div class="control-group">
							        <label for="textfield" class="control-label ">Tên chức vụ <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value=""  name="cTen" id="cTen" class="input-block-level" autofocus />
							        </div>
						        </div>
                                
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Phòng ban <i class="f-red">*</i></label>
							        <div class="controls">
                                        <select name="iPhongBan" id="iPhongBan">
                                            <option value='0'>- - - Chọn phòng ban</option>         
                                            <%=ViewData["opt_phongban"] %>                                     
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
    $("#iPhongBan").chosen();

    function CapNhat() {

        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên chức vụ "); $("#cTen").focus();
            return false;
        }

        if ($("#iPhongBan").val() == 0) {
            alert("Vui lòng chọn phòng ban!"); $("#iPhongBan").focus();
            return false;
        }

        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Chucvu_insert", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Thêm mới thành công")
            } else {
                alert("Mã chức vụ đã tồn tại.");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;
    }
    
</script>