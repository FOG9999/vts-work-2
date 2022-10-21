<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Objects" %>
<%@ Import Namespace="KienNghi.Models" %>
<div id="screen"></div>
<div id="popup" class="popup">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"></i> Chi tiết vụ việc tiếp dân thường xuyên
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>                        
                        <div class="box-content popup_info nopadding">
                            <% tiepdan_thuongxuyen t = (tiepdan_thuongxuyen)ViewData["tiepdan"];
                                ThuongXuyen tx = (ThuongXuyen)ViewData["tx"]; %>
                            <table class="table table-condensed table-bordered form4">
                                    <tr>
                                        <th colspan="4">Thông tin vụ việc tiếp nhập</th>
                                    </tr>
                                    <tr>
                                        <td class="f-">Ngày tiếp </td>
                                        <td>
                                            <%=tx.ngaytiep %>
                                        </td>
                                        <td class="">Địa điểm</td>
                                        <td>
                                            <%=t.cDiaDiem %>
                                        </td>
                                    </tr>  
                                    <tr>
                                        <td class="f" >Cơ quan tiếp </td>
                                        <td><%=tx.coquan_tiep %></td>
                                        <td class="f" >Cán bộ tiếp </td>
                                        <td><%=t.cNguoiTiep %></td>
                                        
                                    </tr>                              
                                    <tr>
                                        <td class="f-" width="15%">Người gửi </td>
                                        <td width="35%">
                                            <%=t.cNguoiGui_Ten %>
                                        </td>
                                        <td class="f-" width="15%">Địa chỉ người gửi </td>
                                        <td width="35%">
                                            <%=t.cNguoiGui_DiaChi %>
                                        </td>
                                    </tr> 
                                    <% if (t.iDoan == 1) {%>
                                    <tr>
                                        <td></td>
                                        <td colspan="3">
                                            Đoàn đông người: <strong><%=t.iDoan_Nguoi %></strong> người
                                        </td>
                                    </tr>     
                                    <% } %>                          
                                    <tr>
                                        <td class="f-">Tóm tắt nội dung vụ việc </td>
                                        <td colspan="3" >
                                            <%=t.cNoiDung %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">Đơn kèm theo</td>
                                        <td colspan="3" >
                                            <%=ViewData["file_tiepdan"] %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th colspan="4">Phân loại vụ việc</th>
                                    </tr>
                                    <tr>
                                        <td class="">Loại vụ việc</td>
                                        <td><%=tx.loaivuviec %>                                        
                                        </td>
                                        <td class="">Lĩnh vực</td>
                                        <td><%=tx.linhvuc %>                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nhóm nội dung</td>
                                        <td><%=tx.loai_noidung %>                                        
                                        </td>
                                        <td>Tính chất vụ việc</td>
                                        <td><%=tx.tinhchat %>                                        
                                        </td>
                                    </tr> 
                                    <tr>
                                        <th colspan="4">Kết quả trả lời, giải quyết</th>
                                    </tr>
                                    <% tiepdan_thuongxuyen_ketqua k = (tiepdan_thuongxuyen_ketqua)ViewData["ketqua"]; %>
                                    <tr>
                                        <td class="">Người trả lời, giải quyết</td>
                                        <td colspan="3" >
                                            <%=k.cKetQua_NguoiTraLoi %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">Kết quả trả lời, giải quyết</td>
                                        <td colspan="3" >
                                            <%=k.cKetQua %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">Văn bản trả lời</td>
                                        <td colspan="3" >
                                            <%=ViewData["file_traloi"] %>
                                            
                                        </td>
                                    </tr>
                                
                                </table>
                            
                            
                                <p class="tcenter">
                                <span onclick="HidePopup();" class="btn btn-warning">Quay lại</span></p>
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>

