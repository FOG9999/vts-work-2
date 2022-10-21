<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Entities.Models" %>
<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Cập nhật khiếu nại lĩnh vực
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info">
                            <% LINHVUC linhvuc = (LINHVUC)ViewData["linhvuc"]; %>
                            <form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="return CapNhat();">
                                
                                <div class="control-group">
							        <label for="textfield" class="control-label">Mã lĩnh vực</label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(linhvuc.CCODE) %>" name="cCode" id="cCode" class="input-medium" onchange="kiemtrungma()" />
							        </div>
						        </div>
                                 <div class="control-group">
							        <label for="textfield" class="control-label">Tên lĩnh vực <i class="f-red">*</i></label>
							        <div class="controls">
                                        <input type="text" value="<%=Server.HtmlEncode(linhvuc.CTEN) %>"  name="cTen" id="cTen" class="input-block-level"   onchange="kiemtrungten()" />
							        </div>
						        </div>
                                <div class="control-group">
							        <label for="textfield" class="control-label" >Thuộc lĩnh vực</label>
							        <div class="controls">
                                        <select name="iParent" id="iParent" class="input-block-level" onchange="Loadloaidon(this.value)">
                                            <option value='0'>- - - Gốc</option>
                                            <%=ViewData["opt-linhvuc"] %>
                                        </select>
							        </div>
						        </div>
                                   <div class="control-group">
                                    <label for="textfield" class="control-label">Nhóm loại đơn</label>
                                    <div class="controls" id="loadloaidon">
                                        <select name="iLoaidon" id="iLoaidon" class="input-block-level">
                                            <option value='0'>- - - Chưa xác định</option>
                                           <%= ViewData["opt-loaidon"] %>
                                        </select>
                                    </div>
                                </div>
                               <div class="control-group" style="display:none">
                                    <label for="textfield" class="control-label">Nhóm lĩnh vực</label>
                                    <div class="controls">
                                        <select name="iNhom" id="iNhom" class="input-block-level">
                                            <option value="1" <% if (linhvuc.INHOM == 1) { Response.Write("selected"); } %>) >- - - Hành chính</option>
                                            <option value="2" <% if (linhvuc.INHOM == 2) { Response.Write("selected"); } %>) >- - - Tư pháp</option>
                                              <option value="3" <% if (linhvuc.INHOM == 3) { Response.Write("selected"); } %>) >- - - Khác</option>
                                           
                                        </select>
                                    </div>
                                </div>
                                 
                                <input type="hidden" name="id" value="<%=ViewData["id"] %>" id="id" />                                                                                    
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
<script type="text/javascript">
    $("#iLoaidon").chosen();
    $("#iParent").chosen();
   
    function CapNhat() {
        //$("[name=__RequestVerificationToken]").val("i4sQEtJtGVTUV85XK4DP69lOUjckIK4mJhlEUv-9nRH8mjN0m9_AWeKx8_h7PGmF5UfDtRIFCD7aPUk21nuj2k8Or1xmp793oid0Oc-R4kuPvjGc6krs3JOX5To4q04kOBBeeKdbPaXVt_xzGSGA9q3LSC-dU7s4HBEsa5RkAaA1");
        if ($("#cTen").val() == "") {
            alert("Vui lòng nhập tên lĩnh vực"); $("#cTen").focus(); return false;
        }
        $(".form-actions").html("<p class=''>Đang xử lý, vui lòng đợi! &nbsp;&nbsp;&nbsp;<img src='/Images/ajax-loader.gif' /></p>");
        $.post("/Thietlap/Ajax_Linhvuc_update", $("#_form").serialize(), function (ok) {
            if (ok == 1) {
                location.reload();
                AlertAction("Cập nhật thành công")
            } else if (ok == 2) {
                alert("Tên lĩnh vực đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
            else {
                alert("Mã lĩnh vực đã tồn tại, Vui lòng kiểm tra lại!");
                $(".form-actions").html("<button type='submit' class='btn btn-primary' id='btn-submit'>Cập nhật</button> <span onclick='HidePopup();' class='btn btn-warning'>Quay lại</span>");
            }
        });
        return false;
       
    }
    function Loadloaidon(val) {

        $.post("<%=ResolveUrl("~")%>Thietlap/Ajax_LoadNoiDungDon/" + val + "",

           function (data) {
               $("#loadloaidon").html(data);
               $("#iLoaidon").chosen();
           }
       );

    }
</script>
