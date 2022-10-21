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
							<h3><i class="icon-reorder"></i> Cập nhật đánh giá kết quả trả lời</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% KN_GIAMSAT g = (KN_GIAMSAT)ViewData["giamsat"]; %>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" action="/Kiennghi/Kiennghi_danhgia_update/" id="_form" enctype="multipart/form-data" class="form-horizontal" onsubmit="return CheckForm();">                               
                                
                                <%--<div class="control-group hide">
							        <label for="textfield" class="control-label">Phân loại kết quả trả lời</label>
							        <div class="controls">
                                        <select name="iPhanLoai" class="input-block-level"><%=ViewData["opt-phanloai"] %></select>
							        </div>
						        </div>
                                <div class="control-group hide">
							        <label for="textfield" class="control-label">Đánh giá kết quả trả lời</label>
							        <div class="controls">
                                        <select name="iDanhGia" class="input-block-level"><%=ViewData["opt-danhgia"] %></select>
							        </div>
						        </div>--%>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Phân loại kết quả trả lời <i class="f-red">*</i></label>
							        <div class="controls">
                                        <div>
                                            <select class="input-block-level" name="iPhanLoai" id="iPhanLoai" onchange="ChangePhanloai(this.value)">
                                                <%=ViewData["opt-phanloai"] %>
                                            </select>
                                        </div> 
                                        <div id="phanloai_child" style="margin-top:10px"><%=ViewData["select-phanloai"] %></div>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Đề nghị</label>
							        <div class="controls">
                                        <select name="iDongKienNghi" class="input-block-level">
                                            <option value="1">Đóng kiến nghị</option>
                                            <option <% if ((int)g.IDONGKIENNGHI == 0) { Response.Write("selected"); } %> value="0">Theo dõi ở kỳ họp sau</option>
                                            
                                        </select>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Tiến độ</label>
							        <div class="controls">
                                        <p class="span4"><input type="radio" <% if ((int)g.IDUNGTIENDO == 1) { Response.Write("checked"); } %>  name="iDungTienDo" id="iDungTienDo" value="1" /> Đúng tiến độ</p>
                                        <p class="span4"><input type="radio" <% if ((int)g.IDUNGTIENDO == 0) { Response.Write("checked"); } %> name="iDungTienDo" id="iDungTienDo" value="0" /> Chậm tiến độ</p>
							        </div>
						        </div>                                                                                                                 
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-success">Cập nhật</button>
                                    <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
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
    function ChangePhanloai(val) {
        if (val != 0) {
            $.post("/Kiennghi/Ajax_Change_Phanloai_danhgia_option", 'id=' + val, function (data) {
                $("#phanloai_child").show().html(data);
            });
        } else {
            $("#phanloai_child").show().html("");
        }
    }
    function CheckForm() {
        if ($("#iPhanLoai").val() == 0) { alert("Vui lòng chọn phân loại kết quả trả lời!"); $("#iPhanLoai").focus(); return false; }
        if ($("#iPhanLoai1").length) {
            if ($("#iPhanLoai1").val() == 0) {
                alert("Vui lòng chọn phân loại kết quả trả lời tiếp theo!"); $("#iPhanLoai1").focus(); return false;
            }
        }
    }
</script>
