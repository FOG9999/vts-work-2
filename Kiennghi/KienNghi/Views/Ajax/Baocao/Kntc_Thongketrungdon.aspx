<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3>
                                <i class="icon-reorder"></i>Báo cáo thống kê trùng đơn
                            </h3>

                        </div>

                        <div class=" box-content popup_info nopadding">
                            <div>
                                <table class="table table-condensed table-bordered">
                                    <thead>
                                        <tr>
                                            <th width="10%">STT</th>
                                            <th width="50%">Tên địa danh</th>
                                            <th width="10%">Số đơn trùng</th>
                                            <th width="20%">Nội dung đơn</th>
                                            <th width="10%">Số lần trùng</th>
                                        </tr>
                                    </thead>
                                    <tbody id="data">
                                        <%=ViewData["data"] %>
                                    </tbody>

                                </table>

                            </div>

                        </div>
                    </div>
                </div>
            </div>
