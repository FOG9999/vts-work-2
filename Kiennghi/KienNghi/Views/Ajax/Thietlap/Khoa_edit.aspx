<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Cập nhật khóa
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% QUOCHOI_KHOA k = (QUOCHOI_KHOA)ViewData["khoa"]; %>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                <div class="control-group">
                                    <label for="textfield" class="control-label">Loại <i class="f-red">*</i></label>
                                    <div class="controls">
                                        <span class="span4">
                                            <label><input class="nomargin" type="radio"  name="iLoai" value="0" <%=k.ILOAI == 0 ? "checked" : ""%>/>
                                            Quốc hội</label>
                                        </span>
                                        <span class="span4">
                                            <label><input class="nomargin" type="radio"  name="iLoai" value="1" <%=k.ILOAI == 1 ? "checked" : ""%>/>
                                            Hội Đồng Nhân Dân</label>
                                        </span>
                                    </div>
                                </div>
                               <div class="control-group">
                                    <label for="textfield" class="control-label">Mã khóa <i class="f-red">*</i></label>
                                    <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(k.CCODE) %>" name="cCode" id="cCode" class="input-medium"/>
                                    </div>
                                </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Tên khóa <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(k.CTEN) %>" name="cTen" id="cTen"  onchange="kiemtrungten()" class="input-block-level" />
							        </div>
						        </div>
                                
                                <div class="control-group">
							        <label for="textfield" class="control-label">Năm bắt đầu <i class="f-red">*</i></label>
							        <div class="controls">
                                        <select id="dBatDau" name="dBatDau" class="input-medium chosen-select">
                                                <option value="0">Chọn năm</option>
                                                <%=ViewData["opt-nam-batdau"] %>
                                            </select>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Năm kết thúc <i class="f-red">*</i></label>
							        <div class="controls">
                                        <select id="dKetThuc" name="dKetThuc" class="input-medium chosen-select">
                                                <option value="0">Chọn năm</option>
                                                <%=ViewData["opt-nam-ketthuc"] %>
                                            </select>
							        </div>
						        </div>   
                                  <input type="hidden" name="id" value="<%=ViewData["id"] %>" />                                                                              
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
    $("#dBatDau").chosen();
    $("#dKetThuc").chosen();
    function CapNhat() {
        if ($("#cCode").val() == "") {
            alert("Vui lòng nhập mã khóa"); $("#cCode").focus();
            return false;
        }
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên khóa"); $("#cTen").focus();
            return false;
        }
        if ($("#dBatDau").val() == "") {
            alert("Vui lòng chọn năm bắt đầu"); $("#dBatDau").focus();
            return false;
        }
        if ($("#dKetThuc").val() == "") {
            alert("Vui lòng chọn năm kết thúc"); $("#dKetThuc").focus();
            return false;
        }
        if ($("#dBatDau").val() > $("#dKetThuc").val()) {
            alert("Vui lòng chọn năm kết thúc lớn hơn năm bắt đầu !"); $("#dKetThuc").focus();
            return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Khoa_update", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Cập nhật thành công");
            } else {
                alert("Mã Khóa này đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;
      
    }
   
</script>
