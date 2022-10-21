<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color">
			<div class="box-title">
				<h3>
					<i class="icon-search"> </i> Tìm kiếm  kiến nghị
				</h3>
                <%--<ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>--%>
            </div>
            <div class="box-content popup_info" style="padding:0px !important">
                <form id="form_" method="post" onsubmit="return CheckFormSearch();">     
                    <table class="table table-bordered form4 table-condensed">
                        <tr>
                            <td>Chọn kỳ họp</td>
                            <td>    <select class="chosen-select" name="iKyHop" id="iKyHop">
                                        <%=ViewData["kyhop"] %>
                                    </select>                                       
                            </td>
                            <td></td>
                            <td>
                                <span class="span5"><input type="radio" name="iTruocKyHop" id="iTruocKyHop" value="1" /> Trước kì họp</span>
                                <span class="span5"><input type="radio" name="iTruocKyHop" id="iTruocKyHop" value="0" /> Sau kì họp</span>
                            </td>
                        </tr>
                        <tr>
                             <td>Thẩm quyền xử lý:</td>
                            <td>
                                <select name="iDonViXuLy" class="chosen-select input-block-level">
                                    <option value="-1">- - - Chọn tất cả</option>
                                    <%=ViewData["opt-donvixuly"] %></select>
                            </td>
                            <td>Lĩnh vực:</td>
                            <td>
                                <select name="iLinhVuc" class="chosen-select input-block-level">
                                    <option value="-1">- - - Chọn tất cả</option>
                                    <option value="0">Nhiều lĩnh vực liên quan</option>
                                    <%=ViewData["opt-linhvuc"] %></select>
                            </td>
                        </tr>
                        <tr>
                            <td>Nội dung, từ khóa</td>
                            <td colspan="3"><input type="text" class="input-block-level" name="cNoiDung" /></td>
                        </tr>
                        <tr>
                            <td colspan="4" class="tright">
                                <button type="submit" class="btn btn-success"> Tra cứu</button>                                     
                            </td>
                        </tr>
                    </table>                                     
                </form>
            </div>                            
        </div>
    </div>
</div>
<script type="text/javascript">
   
    function CheckFormSearch() {
        $("#q_data").html("<tr><td colspan=5 class='tcenter'><p class='tcenter'><img src='/Images/ajax-loader.gif'/></p></td></tr>");
        $.post("/Kiennghi/Ajax_search_theodoi_kiennghi_dannguyen_result", $("#form_").serialize(), function (ok) {
            $("#q_data").html(ok);
        });
        return false;
    }
</script>
   