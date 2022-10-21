<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color box-bordered">
            <div class="box-title">
                <h3>
                    <i class="icon-reorder"></i>Số liệu tiếp công dân phụ lục
                </h3>
                
            </div>

            <div class=" box-content popup_info nopadding">
                <div style="overflow:auto">
                    <table class="table table-bordered table-condensed" >
                                    <thead>
                                       <%-- <tr>
                                            <th rowspan="4" style="text-align:center">Thời gian
                                            </th>
                                            <th colspan="10" style="text-align:center">TÌNH HÌNH TIẾP CÔNG DÂN
                                            </th>
                                            <th colspan="3" style="text-align:center">KẾT QUẢ TIẾP CÔNG DÂN
                                            </th>
                                        </tr>
                                        <tr>
                                            <th colspan="3" style="text-align:center">Tổng số lượt tiếp
                                            </th>
                                            <th colspan="7" style="text-align:center">Phân loại qua việc tiếp công dân
                                            </th>
                                            <th rowspan="3" style="text-align:center">Hướng dẫn bằng văn bản
                                            </th>
                                            <th rowspan="3" style="text-align:center">Hướng dẫn, giải thích trực tiếp
                                            </th>
                                            <th rowspan="3" style="text-align:center">Chuyển đơn đến cơ quan có thẩm quyền
                                            </th>
                                        </tr>
                                           <tr>
                                            <th rowspan="2" style="text-align:center">Tổng số </th>
                                            <th rowspan="2" style="text-align:center">Tiếp công dân theo lĩnh vực phụ trách </th>
                                            <th rowspan="2" style="text-align:center">Tiếp công dân của cá nhân đoàn ĐBQH </th>
                                            <th rowspan="2" style="text-align:center">Số vụ việc </th>
                                            <th colspan="3" style="text-align:center" >Theo loại đơn </th>
                                            <th colspan="2" style="text-align:center" >Theo lĩnh vực</th>
                                            <th rowspan="2" style="text-align:center">Đoàn đông người </th>
                                        </tr>
                                        <tr>
                                             <th  style="text-align:center">Khiếu nại </th>
                                            <th  style="text-align:center">Tố cáo</th>
                                             <th style="text-align:center" >Kiến nghị phản ánh</th>
                                             <th  style="text-align:center">Hành chính</th>
                                             <th  style="text-align:center">Tư pháp</th>
                                        </tr>--%>
                                        <%=ViewData["tieude"] %>
                                    </thead>
                                    <tbody>
                                        <%=ViewData["data"] %>
                                    </tbody>
                                </table>
                </div>
               
            </div>
        </div>
    </div>
</div>