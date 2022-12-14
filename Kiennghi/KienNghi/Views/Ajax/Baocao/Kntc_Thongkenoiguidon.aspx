<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
        <div class="span12">
            <div class="box box-color box-bordered">
                <div class="box-title">
                    <h3>
                        <i class="icon-reorder"></i>Báo cáo thống kê nơi gửi đơn
                    </h3>
                
                </div>

                <div class=" box-content popup_info nopadding">
                    <div>
                        <table class="table table-condensed table-bordered">
                            <thead>
                                <tr>
                                <th width="10%">STT</th>
                                <th width="70%">Nơi gửi đơn</th>
                                <th width="10%">Số lượng</th>
                                <th width="10%">Tỷ lệ %</th>
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
