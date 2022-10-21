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
								<i class="icon-reorder"> Chọn địa phương</i> 
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info" >
                            <form id="form_" onsubmit="return CheckForm2()">                                 
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
    function ChonTinhThanh(val) {
        $.post("/Kiennghi/Ajax_Chon_tinh_huyen", 'tinh=' + val + '&iChuongTrinh=' + $("#iChuongTrinh").val(), function (data) {
            $("#huyen").html(data);
        });        
    }
    function CheckForm2() {
        var huyen = "";
        $('input[name="huyen"]:checked').each(function () {
            huyen += this.value + ",";
        });
        //alert(huyen);
        if (huyen == "") {
            HidePopup();
        } else {
            $.post("/Kiennghi/Ajax_View_chuongtrinh_diaphuong", 'huyen=' + huyen, function (data) {
                //alert(data);
                $("#diaphuong").html(data);
                HidePopup();
            });
        }
        return false;
    }
</script>
