<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color">
                        <div class="box-title">
                            <h3>
                                <i class="icon-reorder"></i>Danh sách đơn trùng
                            </h3>
                            <ul class="tabs">
                                <li class="active">
                                    <a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
                                </li>
                            </ul>
                        </div>
                        <div class="box-content popup_info scroll">
                            <div class="scroll" style="height: 400px">
                                <%=ViewData["dontrung"] %>
                            </div>


                        </div>
                        <div class="form-actions nomagin tcenter">
                            <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
