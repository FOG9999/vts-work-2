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
							<h3><i class="icon-reorder"></i> Cập nhật đánh giá kết quả trả lời</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% KN_GIAMSAT g = (KN_GIAMSAT)ViewData["giamsat"]; %>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" enctype="multipart/form-data" class="form-horizontal" onsubmit="return CapNhat();">                               
                                
                                <div class="control-group">
							        <label for="textfield" class="control-label">Nội dung kiến nghị</label>
							        <div class="controls"><%=ViewData["noidung_kiennghi"] %></div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Nội dung trả lời</label>
							        <div class="controls f-green"><%=ViewData["traloi"] %></div>
						        </div>                                
                                <div class="control-group">
							        <label for="textfield" class="control-label">Phân loại kết quả trả lời</label>
							        <div class="controls">
                                        <select name="iPhanLoai" class="input-block-level"><%=ViewData["opt-phanloai"] %></select>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Đánh giá kết quả trả lời</label>
							        <div class="controls">
                                        <select name="iDanhGia" class="input-block-level"><%=ViewData["opt-danhgia"] %></select>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Đề nghị</label>
							        <div class="controls">
                                        <select name="iDongKienNghi" class="input-block-level">
                                            <option value="1">Đóng kiến nghị</option>
                                            <option <% if (g.IDONGKIENNGHI == 0) { Response.Write("selected"); } %> value="0">Theo dõi ở kỳ họp sau</option>
                                            
                                        </select>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Tiến độ</label>
							        <div class="controls">
                                        <p><input type="radio" <% if (g.IDUNGTIENDO == 1) { Response.Write("checked"); } %>  name="iDungTienDo" id="iDungTienDo" value="1" /> Đúng tiến độ</p>
                                        <p><input type="radio" <% if (g.IDUNGTIENDO == 0) { Response.Write("checked"); } %> name="iDungTienDo" id="iDungTienDo" value="0" /> Chậm tiến độ</p>
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
    
    function CapNhat() {
        var frm = $("#_form");
        var data = frm.serializeArray();

        $.ajax({
            type: "GET",
            headers: getHeaderToken(),
            contentType: "application/json; charset=utf-8",
            url: "<%=ResolveUrl("~")%>Kiennghi/Kiennghi_giamsat_update",
            data: data,
            success: function (ok) {
                if (ok == 1) {
                    location.reload();
                } else {
                    alert(ok);
                }
            }
        });
        return false;
    }
</script>
