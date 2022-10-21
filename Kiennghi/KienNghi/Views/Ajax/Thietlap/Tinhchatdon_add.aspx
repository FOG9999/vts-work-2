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
								<i class="icon-reorder"></i> Thêm mới tính chất đơn
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
                                    <label for="textfield" class="control-label">Mã tính chất đơn</label>
                                    <div class="controls">
                                        <input type="text"   name="cCode" id="cCode" class="input-medium" autofocus/>
                                    </div>
                                </div>
                                 <div class="control-group">
							        <label for="textfield" class="control-label ">Tên tính chất đơn <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value=""  name="cTen" id="cTen" class="input-block-level" onchange="kiemtrungten()" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label ">Nhóm lĩnh vực</label>
							        <div class="controls">
                                     
                                       
                                         <select name="iLinhVuc" id="iLinhVuc" class="input-block-level" onchange="LoadLinhVuc()">
                                             <option value='0'>- - - Chưa xác định</option>
                                            <%=   ViewData["opt-Option_linhvuc"]%>
                                        </select>
							      
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label " >Nhóm nội dung <i class="f-red">*</i></label>
							        <div class="controls" id="LoadNoiDung">
                                        <select name="iNhomnoidung" id="iNhomnoidung" class="input-block-level">
                                            <option value='0'>- - - Chưa xác định</option>
                                            <%=  ViewData["nhomdon"] %>
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
    $("#iNhomnoidung").chosen();
    $("#iLinhVuc").chosen();
    function CapNhat() {
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tính chất đơn"); $("#cTen").focus();
            return false;
        } if ($("#iNhomnoidung").val() == 0) {
            alert("Vui lòng chọn nhóm nội dung"); $("#iNhomnoidung").focus();
            return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Tinhchatdon_insert", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Thêm mới thành công");
            } else {
                alert(" Mã Tính chất đơn đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;
      
    }
    function LoadLinhVuc() {
        if ($("#iLinhVuc").val() != 0) {
            $.post("<%=ResolveUrl("~")%>Tiepdan/Ajax_LoadLinhVucNoiDung_add",
                "iLinhVuc=" + $("#iLinhVuc").val(),
                function (data) {
                    $("#LoadNoiDung").html(data);
                   
                }
            );
        }
    }
    
</script>
