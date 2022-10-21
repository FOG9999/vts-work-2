<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3>
                                <i class="icon-reorder"></i>Báo cáo thống kê chi tiết đơn
                            </h3>

                        </div>
                      
                        <div class=" box-content popup_info nopadding">
                            <div>
                                <table class="table table-condensed table-bordered">
                                    <thead>
                                        <tr>
                                            <th width="5%">STT</th>
                                            <th width="10%">Ngày vào sổ</th>
                                            <th width="15%">Họ và tên</th>
                                            <th width="15%">Địa chỉ</th>
                                            <th width="7%">Số người</th>
                                            <th width="5%">Số lần</th>
                                            <th width="15%">Nội dung</th>
                                            <th width="10%">Loại đơn</th>
                                            <th width="15%">Chuyển đến</th>
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