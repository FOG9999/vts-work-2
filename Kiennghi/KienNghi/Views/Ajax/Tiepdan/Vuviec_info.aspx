<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Thêm mới vụ việc trong tiếp dân định kỳ
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>                        
                        <div class="box-content popup_info nopadding">
                            <table class="table table-condensed table-bordered">
                                    <tr>
                                        <th colspan="4">Thông tin vụ việc</th>
                                    </tr>                                
                                    <tr>
                                        <td class="b" width="15%">Người gửi</td>
                                        <td width="35%">
                                            Nguyễn Văn V
                                        </td>
                                        <td class="b" width="15%">Địa chỉ người gửi</td>
                                        <td width="35%">
                                            Thành Phố Huế, Tỉnh Thừa Thiên Huế
                                        </td>
                                    </tr>                                                               
                                    <tr>
                                        <td class="b">Tóm tắt nội dung đơn</td>
                                        <td colspan="3" class="b" >
                                            Khiếu nại về việc cấp Giấy chứng nhận quyền sử dụng đất cho Công ty TNHH NN MTV Lâm nghiệp ABC chồng lấn lên đất của Ông
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="b">Đơn kèm theo</td>
                                        <td colspan="3" ><a href="" class="btn btn-success"><i class="icon-download-alt"></i></a></td>
                                    </tr>
                                    <tr>
                                        <th colspan="4">Phân loại đơn & Kết quả giải quyết</th>
                                    </tr>
                                    <tr>
                                        <td class="b">Loại đơn</td>
                                        <td>Khiếu nại                                        
                                        </td>
                                        <td class="b">Lĩnh vực</td>
                                        <td>Đất đai                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="b">Nhóm nội dung</td>
                                        <td>Đất đai                             
                                        </td>
                                        <td class="b">Tính chất vụ việc</td>
                                        <td>        Bồi thường                                
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td class="b">Kết quả trả lơi, giải quyết</td>
                                        <td colspan="3" >
                                            Lưu theo dõi, chuyển đơn vị có thẩm quyền trả lời bằng văn bản
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="b">Văn bản kèm theo</td>
                                       <td colspan="3" ><a href="" class="btn btn-success"><i class="icon-download-alt"></i></a></td>
                                    </tr>
                                </table>
                            
                            
                                <p class="tcenter">
                                <span class="btn btn-primary" onclick="HidePopup();">Cập nhật</span>
                                <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span></p>
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>

