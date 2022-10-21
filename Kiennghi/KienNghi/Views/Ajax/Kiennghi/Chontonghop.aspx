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
								<i class="icon-reorder"> Chọn kiến nghị tổng hợp</i> 
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info" style="padding:0px !important">

                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                          
                                <table class="table table-condensed table-bordered"> 
                                    
                                    <tr>
                                        <td style="width:15%">Kì họp</td>
                                        <td style="width:35%"><select class="input-block-level">
                                            <option>Kỳ họp thứ 3 - Quốc Hội khóa XIV</option>
                                            <option>Kỳ họp thứ 4 - Quốc Hội khóa XIV</option>
                                                              </select></td>
                                        <td style="width:15%">Đoàn đại biểu quốc hội</td>
                                        <td style="width:35%"><select name="iDiaPhuong_0" onchange="ChangeTinhThanh('iDiaPhuong_1',this.value)" 
                                                id="iDiaPhuong_0" class="input-block-level"><option value="0">Đoàn ĐBQH tỉnh Hòa Bình</option></select></td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%">Mã kiến nghị</td>
                                        <td style="width:35%"><input type="text" class="input-medium datepick" /></td>
                                        <td style="width:15%"></td>
                                        <td style="width:35%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="nopadding">
                                            <table class="table table-condensed table-bordered table-colored-header"> 
                                                <thead>
                                                    <tr >
                                                        <th width="3%" class="tcenter">STT </th>
                                                        <th width="3%" class="tcenter">Chọn</th>
                                                        <th width="10%" nowrap class="tcenter">Mã Kiến nghị </th>
                                                        <th class="tcenter">Nội dung </th>
                                                        <th class="tcenter" nowrap>Đoàn ĐBQH </th>                                          
                                                    </tr>
                                                </thead>
                                                <tr>
                                                    <td class="tcenter">1</td>
                                                    <td class="tcenter"><input type="checkbox" /></td>
                                                    <td class="b f-red tcenter" nowrap>KN_XIV_001</td>
                                                    <td>Đề nghị Quốc hội tăng cường giám sát việc thực hiện quy hoạch tại các khu đô thị lớn, tránh việc điều chỉnh, phá vỡ quy hoạch làm ảnh hưởng đến mật độ dân cư, cơ sở hạ tầng và các công trình phúc lợi.</td>
                                                    <td class="tcenter b">Đoàn ĐBQH Hòa Bình</td>
                                                    
                                                </tr>    
                                                <tr>
                                                    <td class="tcenter">1</td>
                                                    <td class="tcenter"><input type="checkbox" /></td>
                                                    <td class="b f-red tcenter" nowrap>KN_XIV_002</td>
                                                    <td>Thời gian vừa qua, Ủy ban thường vụ Quốc hội gửi các dự án luật về các Đoàn đại biểu Quốc hội các tỉnh, thành phố để lấy ý kiến đóng góp ở địa phương. Tuy nhiên, thời gian yêu cầu gửi </td>
                                                    <td class="tcenter b">Đoàn ĐBQH Hòa Bình</td>                                                   
                                                </tr> 
                                            </table>
                                        </td>
                                    </tr>
                                    </table>                                                                                                           
						        <div class="form-actions nomagin">
                                    <span onclick="HidePopup();" class="btn btn-primary">Chọn</span>
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
