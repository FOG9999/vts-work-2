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
								<i class="icon-reorder"></i> Cập nhật tài khoản
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">                            
                            <div class="scroll" style="height:450px">
                                <table class="table table-bordered">
                                    <tr>
                                        <th width="10%" nowrap>Thời gian</th>
                                        <th >Nội dung cập nhật</th>
                                    </tr>
                                    <%=ViewData["list"] %>                                    
                                </table>
                            </div>
                            <p class="tcenter"><span onclick="HidePopup();" class="btn btn-warning">Quay lại</span></p>
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>