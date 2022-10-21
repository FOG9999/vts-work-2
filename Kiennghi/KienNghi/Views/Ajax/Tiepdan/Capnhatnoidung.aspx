<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color">
                        <div class="box-title">
                            <h3>
                                <i class="icon-reorder"></i> Xử lý vụ việc
                            </h3>
                            <ul class="tabs">
                                <li class="active">
                                    <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
                                </li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <div class="form-actions nomagin">
                                <div class="control-group">
                                    <div class="controls tcenter">
                                        <p>Vụ việc chưa có nội dung vui lòng cập nhật.</p>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="controls tcenter">
                                        <a href="/Tiepdan/Sua/?id=<%=ViewData["id"] %>" class="btn btn-primary">Cập nhật</a>
                                        <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>