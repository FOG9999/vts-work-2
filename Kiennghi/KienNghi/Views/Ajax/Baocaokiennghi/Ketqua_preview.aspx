<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color">
            <div class="box-title">
                <h3>
                    <i class="icon-signal"></i>Kết quả báo cáo    
                </h3>
            </div>
            <div class="box-content popup_info nopadding" style="overflow-y: auto; height: 500px">
                <table class="table table-bordered table-condensed">
                    <tr>
                        <td>HỘI ĐỒNG NHÂN DÂN </td>
                        <td>CỘNG HOÀ XÃ HỘI CHỦ NGHĨA VIỆT NAM	 </td>
                    </tr>
                    <tr>
                        <td>TỈNH THANH HÓA</td>
                        <td>Độc lập - Tự do - Hạnh phúc	</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>Thanh Hóa, ngày....tháng....năm...</td>
                    </tr>
                    <tr>
                        <td colspan="2">TỔNG HỢP CHI TIẾT Ý KIẾN, KIẾN NGHỊ CỦA CỬ TRI VÀ NHÂN DÂN TẠI
                            CÁC HUYỆN, THỊ XÃ, THÀNH PHỐ GỬI TỚI <%=ViewData["kyhop"] %>, HỘI ĐỒNG NHÂN DÂN TỈNH <%=ViewData["khoa"] %>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">(Kèm theo Công văn số        /......... ngày        tháng ... năm 20... của  <%=ViewData["loaidon"] %> tỉnh Thanh Hóa)
                        </td>
                    </tr>
                </table>
                <table class="table table-bordered table-condensed">
                    <tr>
                        <th width="15%">STT
                        </th>
                        <th width="15%">Đơn vị
                        </th>
                        <th width="50%">Nội dung
                        </th>
                        <th width="20%">Lĩnh vực
                        </th>
                        <%=ViewData["list"] %>
                    </tr>



                </table>
            </div>
        </div>
    </div>
</div>


