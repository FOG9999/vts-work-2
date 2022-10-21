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
								<i class="icon-reorder"></i> Kết quả tìm kiếm
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        
                        <div class="box-content popup_info nopadding">
                            <div class="scroll" style="height:500px">
                                    <table class="table table-condensed table-bordered nomargin table-striped">
                                        <tr>
                                            <th width="3%" nowrap>STT</th>                                            
                                            <th nowrap>Nội dung kiến nghị</th>
                                            <th nowrap>Kết quả trả lời</th>
                                            <th width="3%" nowrap>Chọn</th>
                                        </tr>
                                        <%=ViewData["list"] %>
                                    </table>
                               
                            </div>
                            <p class="tcenter"><span onclick="location.reload();" class="btn btn-warning">Quay lại</span></p>
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>



