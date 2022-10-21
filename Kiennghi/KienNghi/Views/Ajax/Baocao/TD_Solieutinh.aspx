<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color box-bordered">
            <div class="box-title">
                <h3>
                    <i class="icon-reorder"></i>Số liệu báo cáo tiếp công dân số liệu và kết quả    
                </h3>
                
            </div>

            <div class=" box-content popup_info nopadding">
                <div style="overflow:auto">
                    <table class="table table-bordered table-condensed" >
                                    <thead>
                                       <%-- <tr>
                                            <th rowspan="4" style="text-align:center;width:3%">STT
                                            </th>
                                            <th rowspan="4" style="text-align:center;width:5%">
                                                Địa phương
                                            </th>
                                            <th colspan="4" rowspan="2" style="text-align:center;width:12%">TIẾP CÔNG DÂN
                                            </th>
                                            <th colspan="13"  style="text-align:center;width:50%">TIẾP NHẬN ĐƠN THƯ
                                            </th>
                                             <th colspan="7"  style="text-align:center;width:25%">KẾT QUẢ XỬ LÝ
                                            </th>
                                             <th colspan="3" rowspan="2"  style="text-align:center;">GIÁM SÁT
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="3" style="text-align:center">Tổng số nhận
                                            </th>
                                            <th rowspan="3" style="text-align:center">Khiếu nại
                                            </th>
                                            <th rowspan="3" style="text-align:center">Tố cáo
                                            </th>
                                            <th rowspan="3" style="text-align:center">Kiến nghị, phản ánh
                                            </th>
                                            <th rowspan="3" style="text-align:center">Đơn trùng
                                            </th>
                                             <th  style="text-align:center" colspan="5">
                                                 Phân loại theo nội dung
                                            </th>
                                                <th  style="text-align:center" colspan="3">
                                                    Phân loại theo lĩnh vực
                                            </th>
                                            <th rowspan="3" style="text-align:center">Đang nghiên cứu
                                            </th>
                                            <th rowspan="3" style="text-align:center">Số đơn lưu theo dõi
                                            </th>
                                            <th rowspan="3" style="text-align:center">Số vụ việc đã chuyển
                                            </th>
                                            <th rowspan="3" style="text-align:center">Số vụ việc đã được thông tin trả lời
                                            </th>
                                             <th rowspan="3" style="text-align:center">Hướng dẫn, giải thích trả lời
                                            </th>
                                            <th rowspan="3" style="text-align:center">Tỉ lệ đã xử lý/đơn nhận
                                            </th>
                                              <th rowspan="3" style="text-align:center">Đơn đôn đốc vụ việc cụ thể
                                            </th>
                                            
                                        </tr>
                                           <tr>
                                            <th rowspan="2" style="text-align:center">Số buổi TCD </th>
                                            <th rowspan="2" style="text-align:center">Lượt người </th>
                                            <th rowspan="2" style="text-align:center">Số vụ viêc</th>
                                            <th rowspan="2" style="text-align:center">Đoàn đông người </th>
                                          
                                            <th rowspan="2" style="text-align:center" >Đất đai</th>
                                            <th rowspan="2" style="text-align:center">Chính sách XH </th>
                                                <th rowspan="2" style="text-align:center" >Vi phạm pháp luật, tham nhũng</th>
                                            <th rowspan="2" style="text-align:center">Quản lý kinh tế, XH</th>
                                               <th rowspan="2" style="text-align:center" >Khác</th>
                                            <th rowspan="2" style="text-align:center">Tư pháp </th>
                                                <th rowspan="2" style="text-align:center" >Hành chính</th>
                                            <th rowspan="2" style="text-align:center">Khác</th>
                                               <th rowspan="2" style="text-align:center">Chuyên đề</th>
                                                <th rowspan="2" style="text-align:center" >Lồng ghép</th>
                                            <th rowspan="2" style="text-align:center">Vụ việc cụ thể</th>
                                        </tr>
                                        <tr>
                                             
                                        </tr>--%>
                                        <%= ViewData["Baocaotieude"]  %>
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