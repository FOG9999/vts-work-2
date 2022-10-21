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
								<i class="icon-reorder"></i> THông tin tiếp công dân định kỳ
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">  
                                <table class="table">
                                    <tr>
                                        <th colspan="2">Thông tin tiếp dân</th>
                                    </tr>
                                    <tr>
                                        <td width="20%" class="b">Ngày tiếp</td>
                                        <td>26/10/2017</td>
                                    </tr>
                                    <tr>
                                        <td class="b">Số lượt người</td>
                                        <td>17</td>
                                    </tr>
                                    <tr>
                                        <td class="b">Đoàn đông người</td>
                                        <td>4</td>
                                    </tr>
                                    <tr>
                                        <td class="b">Số vụ việc</td>
                                        <td>12</td>
                                    </tr>                                    
                                    <tr>
                                        <th colspan="2">Phân loại vụ việc</th>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            Khiếu nại: <strong>4</strong></br>
                                             Tố cáo: <strong>0</strong></br>
                                             Phản ánh, kiến nghị liên quan đến khiếu nại, tố cáo: <strong>3</strong></br>
                                             Đơn có nhiều nội dung khác: <strong>5</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="tcenter">
                                            
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

