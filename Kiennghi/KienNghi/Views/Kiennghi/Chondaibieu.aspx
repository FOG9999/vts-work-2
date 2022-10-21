<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"> </i> Chọn đại biểu
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form id="form_" onsubmit="return CheckForm1()"> 
                                
                                    <table class="table table-condensed table-bordered"> 
                                        <%=ViewData["opt-tinh"] %>    
                                    </table> 
					              
                                <p class="tcenter">
                                    <button type="submit" class="btn btn-success"> Lưu vào chương trình</button>                                      
                                            <a href="#" onclick="HidePopup();" class="btn btn-warning">Quay lại</a>
                                </p>
                        </form>
                            
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>
<script type="text/javascript">
    
    function CheckForm1() {
        var daibieu = "";
        $('input[name="daibieu"]:checked').each(function () {
            daibieu += this.value + ",";
        });
        //alert(huyen);
        $.post("/Kiennghi/Ajax_View_chuongtrinh_daibieu", 'daibieu=' + daibieu, function (data) {
            //alert(data);
            $("#daibieu").html(data);
            HidePopup();
        });
        return false;
    }
</script>
