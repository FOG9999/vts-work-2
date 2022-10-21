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
								<i class="icon-reorder"> Kế hoạch tiếp xúc cử tri</i> 
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info" style="padding:0px !important">

                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                            <div class="scroll" style="height:500px">
                                <table class="table table-condensed table-bordered"> 
                                    <tr>
                                        <td style="width:50%;text-align:center" colspan="2"><input type="radio" /> Trước kì họp </td>
                                        <td style="width:50%;text-align:center" colspan="2"><input type="radio" /> Sau kì họp </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%">Kì họp <i class="f-red">*</i></td>
                                        <td style="width:35%"><input type="text" class="form-control" style="width:100%" /></td>
                                        <td style="width:15%">Thời gian bắt đầu <i class="f-red">*</i></td>
                                        <td style="width:35%"><input type="text" class="form-control"  value="<%=DateTime.Now.ToString("dd/MM/yyy") %>"  style="width:100%"/></td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%">Kế hoạch</td>
                                        <td style="width:35%"><input type="text" class="form-control"  style="width:100%" /></td>
                                        <td style="width:15%">Thời gian kết thúc <i class="f-red">*</i></td>
                                        <td style="width:35%"><input type="text" class="form-control"  value="<%=DateTime.Now.ToString("dd/MM/yyy")   %>" style="width:100%"/></td>
                                    </tr>
                                     <tr>
                                        <td>Nội dung <i class="f-red">*</i></td>
                                         <td colspan="3"> <textarea rows="2" class="form-control" style="width:100% !important"> </textarea></td>
                                    </tr>
                                    <tr>
                                        <td>Địa phương tiếp xúc <i class="f-red">*</i></td>
                                        <td><input type="text" class="form-control" style="width:100%" /></td>
                                        <td colspan="2">  <a class="btn btn-success" style="float:left"><i class="icon-search"></i></a></td>
                                      
                                    </tr>
                                    <tr>
                                        <td colspan="4"> 
                                             <table class="table table-condensed table-bordered">
                                                 <thead>
                                                     <th style="text-align:center">STT</th>
                                                     <th style="width:80%">Địa phương</th>
                                                     <th style="width:10%;text-align:center" >Xóa</th>
                                                 </thead>
                                                 <tbody>
                                                     <tr>
                                                        <td  style="text-align:center">1</td>
                                                        <td>Thành phố Hòa Bình - Tỉnh Hòa Bình</td>
                                                        <td style="text-align:center">  <a class="btn btn-danger" style="text-align:center"><i class="icon-trash"></i></a></td>
                                                    </tr>
                                                     <tr>
                                                        <td  style="text-align:center">2</td>
                                                        <td>Huyện Cao Phong - Tỉnh Hòa Bình</td>
                                                        <td style="text-align:center">  <a class="btn btn-danger" style="text-align:center"><i class="icon-trash"></i></a></td>
                                                    </tr>
                                                 </tbody>
                                             </table>

                                        </td>
                                        
                                      
                                    </tr>
                                      <tr>
                                        <td>Đại biểu quốc hội <i class="f-red">*</i></td>
                                        <td><input type="text" class="form-control" style="width:100%" /></td>
                                        <td colspan="2">  <a class="btn btn-success" style="float:left"><i class="icon-search"></i></a></td>
                                      
                                    </tr>
                                    <tr>
                                        <td colspan="4"> 
                                             <table class="table table-condensed table-bordered">
                                                 <thead>
                                                     <th style="text-align:center">STT</th>
                                                     <th style="width:80%">Đại biểu quốc hội</th>
                                                     <th style="width:10%;text-align:center" >Xóa</th>
                                                 </thead>
                                                 <tbody>
                                                     <tr>
                                                        <td  style="text-align:center">1</td>
                                                        <td>Nguyễn Thanh Hải</td>
                                                        <td style="text-align:center">  <a class="btn btn-danger" style="text-align:center"><i class="icon-trash"></i></a></td>
                                                    </tr>
                                                     <tr>
                                                        <td  style="text-align:center">2</td>
                                                        <td>Nguyễn Mạnh Hùng</td>
                                                        <td style="text-align:center">  <a class="btn btn-danger" style="text-align:center"><i class="icon-trash"></i></a></td>
                                                    </tr>
                                                 </tbody>
                                             </table>

                                        </td>
                                        
                                      
                                    </tr>
                                    <tr><td>Chọn file</td>
                                        <td colspan="3"><input type="file" /></td>
                                    </tr>
                                    
                                    
                                    
                                    </table>
                                </div>                                                                                                             
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                    <input type="hidden" name="id" value="" />
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
