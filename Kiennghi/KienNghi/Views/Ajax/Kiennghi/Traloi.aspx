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
								<i class="icon-reorder"></i> TRả lời kiến nghị
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                
                                 
                                <div class="control-group">
							        <label for="textfield" class="control-label">Nội dung kiến nghị</label>
							        <div class="controls">
                                        <textarea class="input-block-level" name="cNoiDung"></textarea>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">Nội dung trả lời</label>
							        <div class="controls">
                                        <textarea class="input-block-level" name="cNoiDung"></textarea>
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label">File đính kèm</label>
							        <div class="controls">
                                        <div class="input-group file-group" style="margin-bottom:10px">
                                            <span class="input-group-btn">
                                                <span class="btn btn-success btn-file">
                                                    Duyệt file
                                                    <input onchange="CheckFileTypeUpload('file_upload1','file_name1');" name="file_upload1" id="file_upload1" type="file">
                                                </span>
                                            </span>
                                            <input class="input-xlarge" disabled id="file_name1" type="text">
                                            <span class="btn btn-danger" onclick="$('#file_upload1,#file_name1').val('');" title="Hủy"><i class="icon-trash"></i></span>
                                        </div>
							        </div>
						        </div>                                                                                                                    
						        <div class="form-actions nomagin">
                                    <span onclick="HidePopup();" class="btn btn-success">Cập nhật</span>
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

