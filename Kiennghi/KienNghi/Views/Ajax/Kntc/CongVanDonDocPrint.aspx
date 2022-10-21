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
				            <h3>
					            <i class="icon-print"> </i> In báo cáo công văn đôn đốc
				            </h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content">
                            <form id="form_" method="post" class="form-horizontal form-column">  
                                 <% 
                                     KNTC_VANBAN vanban = (KNTC_VANBAN)ViewData["vanbantocao"];
                                     QUOCHOI_COQUAN donvibanhanh = (QUOCHOI_COQUAN)ViewData["donvibanhanh"];
                                     QUOCHOI_COQUAN donvinhan = (QUOCHOI_COQUAN)ViewData["donvinhan"];
                                %>
                                <table class="table table-bordered table-condensed">
                                    
                                    <% if (vanban != null)
                                        { %>
                                        <tr>
                                            <th class="f-" width="15%">Số công văn</th>
                                            <td width="35%">
                                                <%= vanban.CSOVANBAN %>
                                            </td>
                                            <th class="" width="15%">Ngày công văn</th>
                                            <td>
                                                 <%= String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime( vanban.DNGAYBANHANH)) %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="f-" width="15%">Đơn vị ban hành</th>
                                            <%if (donvibanhanh != null)
                                                { %>
                                                <td width="35%">
                                                     <%= donvibanhanh.CTEN %>
                                                </td>
                                            <%  }
                                                else
                                                { %>
                                                 <td width="35%">
                                                </td>
                                            <%  } %>
                                            <th class="" width="15%">Đơn vị nhận</th>
                                            <%if (donvinhan != null)
                                                { %>
                                                <td width="35%">
                                                     <%= donvinhan.CTEN %>
                                                </td>
                                            <%  }
                                                else
                                                { %>
                                                 <td width="35%">
                                                </td>
                                            <%  } %>
                                            
                                        </tr>
                                    
                                </table>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group tright">                        
                                            <button type="button" class="btn btn-" data-original-title="In Excel" onclick="XuatBaoCao('xls',<%=vanban.IVANBAN%> )"  rel="tooltip"> <i class="icon-print"></i>Xuất Excel</button>
                                            <button type="button" class="btn btn-" data-original-title="In PDF" onclick="XuatBaoCao('pdf',<%=vanban.IVANBAN%>)"  rel="tooltip"><i class="icon-file"></i> Xuất PDF</button>
                                        </div>                            
                                    </div>
                                </div>     
                                <%} %>
                            </form>
                
                        </div>                            
                    </div>
                </div>
            </div>                           
        </div>
    </div>
</div>
<script type="text/javascript">
    function XuatBaoCao(ext, id) {
        
        console.log(ext)
        console.log(id)
        url = "/Kntc/Baocao_CongVanDonDoc?iVanBan=" + id + "&ext=" + ext;
        window.open(url, '_blank');

    }
</script>
