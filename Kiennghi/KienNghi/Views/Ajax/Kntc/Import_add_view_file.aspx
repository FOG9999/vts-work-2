<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div >
    <div class="span12">
        <div class="box box-color box-bordered">
			
                <% bool isError = (bool)ViewData["error"];
                    if (!isError){ %>
                        <table class="table table-bordered table-condensed table-striped">
                            <thead>
                                <tr >
                                    <th width="5%" class="tcenter">Trạng thái</th> 
                                    <th width="5%" class="tcenter">STT</th> 
                                    <th nowrap class="tcenter">Họ và tên công dân<i class="f-red">*</i></th> 
                                    <th nowrap class="tcenter">Quận/Huyện<i class="f-red">*</i></th> 
                                    <th nowrap class="tcenter">Xã/Phường/Thị trấn<i class="f-red">*</i></th> 
                                    <th nowrap class="tcenter">Địa chỉ cụ thể<i class="f-red">*</i></th> 
                                    <th nowrap class="tcenter">Số CMND</th> 
                                    <th nowrap class="tcenter">Nội dung đơn<i class="f-red">*</i></th> 
                                    <th nowrap class="tcenter">Ngày nhận đơn<i class="f-red">*</i> (dd/MM/yyyy)</th> 
                                    <th nowrap class="tcenter">Loại đơn<i class="f-red">*</i></th> 
                                    <th width="5%" class="tcenter">Lỗi</th>                                     
                                </tr>
                            </thead>
                            <tbody id="q_data">
                                <%=ViewData["list"] %>
                            </tbody>
                        </table> 
                <%} %>
            
         </div>
    </div>
</div>
      
<script type="text/javascript">
    if ('<%=ViewData["error"]%>' == 'True') {
        alert("Nội dung file không hợp lệ")
    }
    function showAlert(val) {
        alert("Dữ liệu"+ val +" không hợp lệ")
    }
</script>
