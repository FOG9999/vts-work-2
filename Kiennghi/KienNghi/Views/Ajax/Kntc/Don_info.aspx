<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Thông tin đơn
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <% KNTC_DON don = (KNTC_DON)ViewData["don"];
                           KNTC kn = (KNTC)ViewData["kn"]; %>
                        <div class="box-content popup_info nopadding">
                            <div class="scroll" style="height:500px">
                                <table class="table table-condensed table-bordered">
                                    <tr>
                                        <th colspan="4">Thông tin đơn</th>
                                    </tr>                                
                                    <tr>
                                        <td class="b" width="15%">Ngày nhận đơn<span class=" f-red">*</span></td>
                                        <td width="35%">
                                            <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(don.DNGAYNHAN)) %>
                                        </td>
                                        <td class="b" width="15%">Nguồn đơn*</td>
                                        <td>
                                            <%=kn.nguondon %>
                                        </td>
                                    </tr>                                
                                    <tr>
                                        <td class="b">Người nộp đơn<span class=" f-red">*</span></td>
                                        <td><%=don.CNGUOIGUI_TEN %>                                        
                                        </td>
                                        <td class="b">Số CMND; ngày cấp; nơi cấp</td>
                                        <td><%=don.CNGUOIGUI_CMND %></td>
                                    </tr>
                                    <% if(don.IDOANDONGNGUOI==1){ %>
                                    <tr>
                                        <td></td>
                                        <td colspan="3">
                                            <strong>Đoàn đông người</strong>; Số người <%=don.ISONGUOI %>
                                        </td>
                                    </tr>
                                    <% } %>
                                    <tr>
                                        <td class="b">Địa chỉ người nộp</td>
                                        <td colspan="3">
                                            <%=don.CNGUOIGUI_DIACHI %>
                                            <% if (don.IDIAPHUONG_1 != 0) { Response.Write(kn.huyen + ", "); } %>
                                            <% Response.Write(kn.tinh); %>
                                       
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="b">Quốc tịch</td>
                                        <td><%=kn.quoctich %>                                        
                                        </td>
                                        <td class="b">Dân tộc</td>
                                        <td><%=kn.dantoc %>                                    
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="b">Địa chỉ xảy ra vụ việc</td>
                                        <td colspan="3" >
                                            <%=don.CDIACHI_VUVIEC %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="b">Tóm tắt nội dung đơn</td>
                                        <td colspan="3" >
                                            <%=don.CNOIDUNG %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="b">File đính kèm</td>
                                        <td colspan="3" >
                                            <%=ViewData["file"]%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th colspan="4">Phân loại đơn</th>
                                    </tr>
                                    <tr>
                                        <td class="b">Loại đơn</td>
                                        <td><%=kn.loaidon %>                                        
                                        </td>
                                        <td class="b">Lĩnh vực</td>
                                        <td><%=kn.linhvuc %>                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="b">Nhóm nội dung</td>
                                        <td><%=kn.loai_noidung %>                                        
                                        </td>
                                        <td class="b">Tính chất vụ việc</td>
                                        <td><%=kn.tinhchat %>                                        
                                        </td>
                                    </tr>  
                                    <tr>
                                        <td class="b">Tình trạng đơn</td>
                                        <td colspan="3"><%=kn.tinhtrang %>   
                                    </tr>
                                    <tr>
                                        <td class="b">Văn bản liên quan</td>
                                        <td colspan="3"><%=ViewData["vanban_lienquan"] %></td>
                                    </tr>
                                    <!--
                                    <tr>
                                        <th colspan="4">Văn bản kết quả xử lý</th>
                                    </tr>     
                                    <tr>
                                        <td colspan="4" class="nopadding">
                                            <table class="table table-bordered nomargin table-colored-header">
                                                <thead>
                                                    <tr>
                                                        <th width="10%" nowrap>Ngày ban hành</th>
                                                        <th width="10%" nowrap>Số văn bản</th>
                                                        <th width="15%" nowrap>Người ký</th>
                                                        <th nowrap>Tóm tắt nội dung</th>
                                                        <th width="10%" nowrap>File đính kèm</th>
                                                        <th width="5%" nowrap>Sửa</th>
                                                    </tr>
                                                </thead>
                                                <tbody><%=ViewData["vanban"] %></tbody>
                                            
                                            </table>
                                        </td>
                                    </tr>
                                    <%=ViewData["giamsat"] %>
                                    -->
                                </table>
                            </div>
                            
                            <p class="tcenter">
                                <span class="btn btn-primary"><i class="icon-download-alt"></i> Tải về</span>
                                <span onclick="HidePopup();"  class="btn btn-warning">Quay lại</span></p>
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>

