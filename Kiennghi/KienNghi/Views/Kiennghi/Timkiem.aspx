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
								<i class="icon-reorder"> Tìm kiếm kế hoạch tiếp xúc cử tri</i> 
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
                                        <td style="width:15%">Thời gian bắt đầu</td>
                                        <td style="width:35%"><input type="text" class="input-medium datepick"  value="<%=DateTime.Now.ToString("dd/MM/yyy") %>" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%">Kế hoạch</td>
                                        <td style="width:35%"><input type="text" class="input-medium datepick" /></td>
                                        <td style="width:15%">Thời gian kết thúc</td>
                                        <td style="width:35%"><input type="text" class="input-medium datepick"  value="<%=DateTime.Now.ToString("dd/MM/yyy")   %>"/></td>
                                    </tr>
                                    <tr>
                                        <td>Địa phương tiếp xúc</td>
                                        <td><select name="iDiaPhuong_0" onchange="ChangeTinhThanh('iDiaPhuong_1',this.value)" 
                                                id="iDiaPhuong_0" class="input-block-level"><option value="0">Tỉnh Hòa Bình</option></select></td>
                                                                               <td>Đoàn đại biểu quốc hội</td>
                                        <td><select name="iDiaPhuong_0" onchange="ChangeTinhThanh('iDiaPhuong_1',this.value)" 
                                                id="iDiaPhuong_0" class="input-block-level"><option value="0">Đoàn ĐBQH tỉnh Hòa Bình</option></select></td>
                                    </tr>
                                    </table>                                                                                                           
						        <div class="form-actions nomagin">
                                    <span onclick="HidePopup();" class="btn btn-primary">Tìm kiếm</span>
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
