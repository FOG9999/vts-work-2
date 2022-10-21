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
								<i class="icon-reorder"> TÌm kiếm kiến nghị cử tri đã đóng</i> 
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info" style="padding:0px !important">
                            <form id="form_" method="get" action="/Kiennghi/Hoanthanh/">     
                                <table class="table table-bordered">
                                    <tr>
                                        <td width="20%">Chọn kỳ họp</td>
                                        <td><p>
                                            <select class="input-block-level" onchange="ChangeKyHop(this.value)" name="iKyHop">
                                                <option value="-1">Chọn tất cả</option>
                                                <%=ViewData["kyhop"] %>
                                            </select>
                                            </p>
                                            <span class="span5"><input type="radio" name="iTruocKyHop" id="iTruocKyHop" value="1" /> Trước kì họp</span>
                                            <span class="span5"><input type="radio" name="iTruocKyHop" id="iTruocKyHop" value="0" /> Sau kì họp</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Kế hoạch</td>
                                        <td><select name="iChuongTrinh" id="iChuongTrinh" class="input-block-level">
                                                <option value="-1"> - - - Chọn tất cả</option>
                                                <option value="0">Không nằm trong chương trình, kế hoạch tiếp xúc cử tri</option>
                                                <%=ViewData["kehoach"] %>
                                            </select></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Đoàn tiếp nhận</td>
                                        <td>
                                        <select name="iDonViTiepNhan" id="iDonViTiepNhan" class="input-block-level">
                                                <%=ViewData["opt-doan"] %></select>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td nowrap>Thẩm quyền xử lý</td>
                                        <td>
                                        <select name="iThamQuyenDonVi" class="input-block-level">

                                            <option value="-1">- - - Chọn tất cả</option>
                                                <option value="0">Chưa xác định</option>
                                                <%=ViewData["thamquyen"] %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Lĩnh vực</td>
                                        <td>
                                        <select name="iLinhVuc" class="input-block-level">
                                            <option value="-1">- - - Chọn tất cả</option>
                                                 <option value="0">Chưa xác định</option>
                                                <%=ViewData["opt-linhvuc"] %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Tình trạng</td>
                                        <td>
                                        <select name="iTinhTrang" class="input-block-level">

                                             <option value="-1">- - - Chọn tất cả</option>
                                            <option value="4">Đã trả lời, đóng kiến nghị</option>
                                           <option value="5">Kiến nghị trùng, đóng kiến nghị</option>
                                            <option value="6">Chuyển theo dõi ở kỳ họp sau</option>
                                            </select>
                                        </td>
                                    </tr>
                                </table>                            
                                <p class="tcenter">
                                    <button type="submit" class="btn btn-success"> Tra cứu</button>                                      
                                    <a href="#" onclick="HidePopup();" class="btn btn-warning">Quay lại</a>
                                </p>
                            </form>
                            
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>
<script type="text/javascript">
    function ChangeKyHop(id) {
        $.post("/Kiennghi/Ajax_Change_Kyhop_kehoach", 'id=' + id, function (data) {

            $("#iChuongTrinh").html('<option value="-1">- - - Chọn tất cả</option>' + data);

        });
    }
</script>