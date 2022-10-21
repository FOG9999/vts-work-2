<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="span12">
    <div class="box box-color box-bordered">
        <div class="box-title">
            <h3><i class="icon-search"></i>Kết quả tra cứu</h3>
        </div>
        <div class="box-content nopadding">

            <table class="table table-bordered table-condensed">
                <thead>
                    <tr>
                        <th style="width: 3%">STT</th>
                        <th style="width: 25%">Người nộp/Địa chỉ người nộp</th>
                        <th style="width: 5%">Ngày nhận</th>
                        <th style="width: 40%">Nội dung đơn</th>
                        <th style="width: 20%">Ghi chú</th>
                    </tr>
                </thead>
                <tbody>
                    <%=ViewData["list"] %>
                </tbody>
            </table>

        </div>
    </div>
</div>
