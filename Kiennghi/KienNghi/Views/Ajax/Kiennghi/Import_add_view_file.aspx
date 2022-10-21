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
                                <th nowrap class="tcenter">Theo kế hoạch số</th> 
                                <th nowrap class="tcenter">Nội dung kiến nghị</th> 
                                <th nowrap class="tcenter">Nguồn kiến nghị</th> 
                                <th nowrap class="tcenter">Thẩm quyền giải quyết</th> 
                                <th nowrap class="tcenter">Lĩnh vực</th> 
                                <th nowrap class="tcenter">Ghi chú</th> 
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
