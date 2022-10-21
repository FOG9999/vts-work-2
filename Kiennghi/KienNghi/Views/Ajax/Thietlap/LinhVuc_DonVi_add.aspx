<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Thêm đơn vị thuộc lĩnh vực: <%=ViewData["cTen"] %>
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                              <input name="idlinhvuc" id="idlinhvuc" value="<%=ViewData["idlinhvuc"] %>" type="hidden" />                         
                                    <div class="control-group">
							        <label for="textfield" class="control-label">Tên cơ quan</label>
							        <div class="controls">
                                          <div class="actions" id="action"  style="height: 400px; overflow: auto;">
                                               <%= ViewData["List"] %>
                                              </div>  
							        </div>
						        </div>                                                                     
						        <div class="form-actions nomagin">
                                    <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                    <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span>
						        </div>                     
                             </form>
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>
 <script>
     function Checkboxchucnang(val) {
         document.getElementById("action" + val).checked = !document.getElementById("action" + val).checked;
         //var checkbox = $("#action" + val + "").attr("checked"); alert(checkbox);
         //if (checkbox == 'checked') {
         //    $("#action" + val + "").removeAttr("checked", "checked");
         //}
         //if (checkbox == 'undefined')
         //    {
         //    $("#action" + val + "").attr("checked", "checked");
         //}
     }
    </script>
<script type="text/javascript">
    function CapNhat() {
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_DonVi_LinhVuc_insert", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Thêm mới thành công")
            } else {
             
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;
       
    }
   
</script>
