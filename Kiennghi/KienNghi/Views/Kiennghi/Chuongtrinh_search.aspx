<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div class="row-fluid">
    <div class="span12">
        <div class="box box-color">
			<div class="box-title">
				<h3>
					<i class="icon-search"> </i> Tìm kiếm chương trình tiếp xúc cử tri
				</h3>
                <ul class="tabs">
					<li class="active">
						<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
					</li>
                </ul>
            </div>
            <div class="box-content popup_info" style="padding:0px !important">
                <form id="form_" method="post" action="/Kiennghi/Chuongtrinh/" onsubmit="return CheckForm();">     
                    <table class="table table-bordered form4">
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
                            <td>Từ ngày; đến ngày </td>
                            <td><input type="text" placeholder="ngày bắt đầu" class="input-medium datepick" name="dBatDau"/>
                                <input type="text" placeholder="ngày kết thúc" class="input-medium datepick" name="dKetThuc"/>
                            </td>
                            <td>Đoàn lập kế hoạch:</td>
                            <td>
                                <select name="iDonVi" id="iDonVi" class="chosen-select"><%=ViewData["opt-doan"] %></select>
                            </td>
                        </tr>
                        <tr>
                            <td>Kế hoạch, nội dung</td>
                            <td colspan="3"><input type="text" class="input-block-level" name="cNoiDung" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Địa phương</td>
                            <td>
                                <select class="chosen-select"  name="iDiaPhuong">
                                    <%=ViewData["opt-diaphuong"] %>
                                </select>
                            </td>
                            <td>Đại biểu</td>
                            <td>
                                <select class="chosen-select"  name="iDiaPhuong">
                                    <%=ViewData["opt-daibieu"] %>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="tright">
                                <button type="submit" class="btn btn-success"> Tra cứu</button>                                      
                                
                            </td>
                        </tr>
                    </table>                            
                                
                </form>
                            
            </div>                            
        </div>
    </div>
</div>
   