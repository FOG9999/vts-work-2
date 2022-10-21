<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tra cứu kiến nghị cử tri
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <%: Html.Partial("../Shared/_Left_Knct") %>
<div id="main">
    <div class="container-fluid">
        <div class="breadcrumbs">
		    <ul>
			    <li>
				    <a href="<%=ResolveUrl("~") %>"><i class="icon-home"></i> Trang chủ</a>
                    <i class="icon-angle-right"></i>
			    </li>
                <li>
                    <span>Kiến nghị cử tri <i class="icon-angle-right"></i></span>                    
                </li>
                <li>
				    <span>Tra cứu kiến nghị cử tri</span>
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>        
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-search"></i> Tra cứu kiến nghị cử tri</h3>
                    </div>
				    <div class="box-content nopadding">                     
                        <form id="form_" method="get" class="form-horizontal form-column" onsubmit="return CheckForm();">     
                            <div class="row-fluid">
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Mã kiến nghị:</label>
                                        <div class="controls">
                                            <div class="banndannguyen-search input-block-level">
                                                <input type="text" class="input-block-level" name="cMaKienNghi" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                           <label for="textfield" class="control-label">Kỳ họp</label>
                                           <div class="controls ">
                                               <select class="chosen-select" onchange="ChangeKyHop(this.value)" name="iKyHop">
                                                   <%=ViewData["kyhop"] %>
                                               </select>
                                           </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label"></label>
                                            <div class="controls">
                                                <%=ViewData["check-hinhthuc"] %>
                                            </div>
                                        </div>
                                    </div>
                                </div>  
                                <div class="row-fluid">
                                    <div class="span6">
                                         <div class="span6">
                                            <div class="control-group">
                                                <label for="textfield" class="control-label">Ngày nhận</label>
                                                <div class="controls">
                                                    <div class="input-block-level">
                                                        <input type="text" placeholder="Từ ngày" value="<%=ViewData["dNgayNhan_from"] %>" class="span6 datepick" name="dNgayNhan_from" id="dNgayNhan_from" />
                                                        <input type="text" placeholder="Đến ngày"  value="<%=ViewData["dNgayNhan_to"] %>" class="span6 datepick" name="dNgayNhan_to" id="dNgayNhan_to" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label for="textfield" class="control-label">Ngày trả lời</label>
                                                <div class="controls">
                                                    <div class="input-block-level">
                                                        <input type="text" placeholder="Từ ngày" value="<%=ViewData["dNgayTraLoi_from"] %>" class="span6 datepick" name="dNgayTraLoi_from" id="dNgayTraLoi_from" />
                                                        <input type="text" placeholder="Đến ngày"  value="<%=ViewData["dNgayTraLoi_to"] %>" class="span6 datepick" name="dNgayTraLoi_to" id="dNgayTraLoi_to" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Nguồn kiến nghị:</label>
                                            <div class="controls">
                                                <div class=" input-block-level select-form-width-full">
                                                    <select id="listNguonKienNghi" name="listNguonKienNghi" multiple="multiple">
                                                        <%=ViewData["opt-nguonkiennghi"] %>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                <div class="row-fluid">
                                    
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Đơn vị tiếp nhận:</label>
                                            <div class="controls">
                                                <div class="select-form-width-full input-block-level">
                                                    <select id="iDonViTiepNhan" name="iDonViTiepNhan">
                                                        <%=ViewData["opt-doan"] %>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Lĩnh vực:</label>
                                            <div class="controls">
                                                <div class="linhvuc input-block-level" id="div_linhvuc">
                                                   <select id="iLinhVuc" name="iLinhVuc" class="chosen-select">
                                                       <option value="-1">- - - Chọn tất cả</option>
                                                       <%--<option value="0">Nhiều lĩnh vực liên quan</option>--%>
                                                       <%=ViewData["opt-linhvuc"] %>
                                                       </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                <div class="row-fluid">
                                <%--<div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label  ">Chọn Huyện/Thị xã/Thành phố:</label>
                                        <div class="controls">
                                            <div class="input-block-level">
                                            <select id="listHuyen_Xa_ThanhPho" multiple="multiple" name="listHuyen_Xa_ThanhPho">
                                                <%=ViewData["huyen_thixa_thanhpho"] %>
                                            </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="span6">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Thẩm quyền giải quyết:</label>
                                                <div class="controls">
                                                    <%--<%=ViewData["radio-thamquyen"] %>--%>
                                                    <div class="select-form-width-full input-block-level">
                                                        <select onchange="ChangeLinhVucByDonVI(this.value)" name="iThamQuyenDonVi" class="chosen-select">
                                                            <option value="0">Chọn tất cả</option>
                                                            <%=ViewData["opt-thamquyen"] %>
                                                        </select>
                                                    </div>
                                                </div>
                                        </div>
                                    </div>
                                
                                <%--<div class="control-group">
                                        <label for="textfield" class="control-label  ">Thẩm quyền giải quyết:</label>
                                        <div class="controls">
                                            <div class="input-block-level">
                                                <select onchange="ChangeLinhVucByDonVI(this.value)" name="iDonViXuLy" class="chosen-select input-block-level">                                    
                                                    <option value="0">- - - Chọn tất cả</option>
                                                    <%=ViewData["opt-donvixuly"] %></select>
                                            </div>
                                        </div>
                                    </div>--%>
                            </div>  
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Nội dung kiến nghị:</label>
                                            <div class="controls">
                                                <div class="banndannguyen-search input-block-level">
                                                    <input type="text" class="input-block-level" name="cNoiDungKN" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                <%--<div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Tên báo cáo:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <select name="iTenBaoCao" class="chosen-select input-block-level">                                    
                                                        <%=ViewData["opt-tenbaocao"] %></select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>
                                <%--<div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Ghi chú:</label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <input type="text" class="input-block-level" name="cNoiDung" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="control-group tright">
                                            <input type="hidden" value="1" name="hidechecksearch" />
                                            <input type="hidden" id="hidAdvancedSearch" name="hidAdvancedSearch" value="1"/>
                                            <button type="submit" class="btn btn-success"> Tra cứu</button>
                                            <%--<button type="button" onclick="print('xls')" class="btn btn-success"><i class="icon-print"></i> In Excel </button>
                                            <button type="button" onclick="print('pdf')" class="btn btn-success"><i class="icon-file"></i> In PDF </button>--%>
                                        </div>                            
                                    </div>
                                </div>  
                        </form>                   
				    </div>
			    </div>
		    </div>
	    </div>
		<div class="row-fluid">
            <div class="span12">
        		<div class="box box-color box-bordered">
        			<div class="box-title">
        				<h3><i class="icon-list"></i> 
                            Kiến nghị cử tri                           
        				</h3>
                        <%--<div class="pull-right box-title-header">
                            <form id="form_header">
                                     <%--<select class="chosen-select" name="iKyHop" id="iKyHop" onchange="ChangeDonVi_KyHop();">
                                    <%=ViewData["opt-khoa-kyhop"] %>
                                    </select>--%>                                
                                  <%--<select class="chosen-select" name="iDoan" id="iDoan" onchange="ChangeDonVi_KyHop();">
                                        <%=ViewData["opt-coquan"] %>
                                 </select>
                                <select class="chosen-select" name="iDonViXuLy_Parent" id="iDonViXuLy_Parent" onchange="DoiThamQuyenDonVi(this.value);">
                                    <%=ViewData["opt-thamquyen"] %>
                                 </select>
                                <select class="chosen-select" name="iDonViXuLy" id="iDonViXuLy" onchange="ChangeDonVi_KyHop();">
                                    <%=ViewData["opt-thamquyen-xuly"] %>
                                 </select>                               
                            </form>
                        </div>--%>
        			</div>
        			<div class="box-content nopadding">              
                        <table class="table table-bordered table-condensed <%--table-striped--%>">
                            <thead>
                                <tr >
                                    <th width="3%" class="tcenter">STT </th>
                                    <th class="tcenter">Nội dung </th>  
                                    <th class="tcenter"  width="15%">Tiếp nhận </th>                                                 
                                    <th class="tcenter"  width="25%">Thẩm quyền > Lĩnh vực </th>                                                 
                                    <th class="tcenter" width="5%" nowrap>Chức năng</th>                                           
                                </tr>
                            </thead>
                            <tbody id="ip_data">
                                <%=ViewData["list"] %>
                                <%=ViewData["phantrang"] %> 
                            </tbody>
                        </table>     
                        <div style="display: none;" id="loadData" class="tcenter"><img src='/Images/ajax-loader.gif' /></div>    
        			</div>
        		</div>
        	</div>
        </div>
    </div>  
</div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#listKyHop').multiselect({
                enableCollapsibleOptGroups: true,
                includeSelectAllOption: true,
                selectAllText: 'Chọn toàn bộ',
                allSelectedText: 'Đã chọn toàn bộ',
                nonSelectedText: 'Chọn kỳ họp',
                nSelectedText: 'kỳ họp đã chọn'
            });
            $("#iDonViTiepNhan").chosen();
            $('#listHuyen_Xa_ThanhPho').multiselect({
                enableCollapsibleOptGroups: true,
                includeSelectAllOption: true,
                selectAllText: 'Chọn toàn bộ',
                allSelectedText: 'Đã chọn toàn bộ',
                nonSelectedText: 'Chọn Huyện/Thị xã/Thành phố',
                nSelectedText: 'Huyện/Thị xã/Thành phố đã chọn'
            });
            $("#listNguonKienNghi").multiselect({
               enableCollapsibleOptGroups: true,
               includeSelectAllOption: true,
               selectAllText: 'Chọn tất cả',
               allSelectedText: 'Đã chọn toàn bộ',
               nonSelectedText: 'Chọn nguồn kiến nghị',
               nSelectedText: 'nguồn kiến nghị đã chọn'
            });
        });
        function ExportExcel() {
            var pramt = $("#form_").serialize();
            window.location = "/Kiennghi/Download_Tracuu/?" + pramt;
        }
        function ChangeLinhVucByDonVI(val) {
            <%--$.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
                //alert(data);
                $("#div_linhvuc").html('<select name="iLinhVuc" id="iLinhVuc" class="chosen-select"><option value="-1">- - - Chọn tất cả</option>' + data + '</select>');
                //$('#iChuongTrinh').trigger('chosen:updated');
                $("#iLinhVuc").chosen();
            });--%>
        }
        function CheckForm() {          
            <%--if ($("#dNgayNhan_from").val() != "") {
                if (!Validate_DateVN("dNgayNhan_from")) {
                    return false;
                }
            }
            if ($("#dNgayNhan_to").val() != "") {
                if (!Validate_DateVN("dNgayNhan_to")) {
                    return false;
                }
            }
            if ($("#dNgayNhan_from").val() != "" && $("#dNgayNhan_to").val() != "") {
                if (!CompareDate("dNgayNhan_from", "dNgayNhan_from")) {
                    return false;
                }
                
            }
            if ($("#dNgayTongHop_from").val() != "") {
                if (!Validate_DateVN("dNgayTongHop_from")) {
                    return false;
                }
            }
            if ($("#dNgayTongHop_to").val() != "") {
                if (!Validate_DateVN("dNgayTongHop_to")) {
                    return false;
                }
            }
            if ($("#dNgayTongHop_from").val() != "" && $("#dNgayTongHop_to").val() != "") {
                if (!CompareDate("dNgayTongHop_from", "dNgayTongHop_to")) {
                    return false;
                }
            }
            if ($("#dNgayTraLoi_from").val() != "") {
                if (!Validate_DateVN("dNgayTraLoi_from")) {
                    return false;
                }
            }
            if ($("#dNgayTraLoi_to").val() != "") {
                if (!Validate_DateVN("dNgayTraLoi_to")) {
                    return false;
                }
            }
            if ($("#dNgayTraLoi_from").val() != "" && $("#dNgayTraLoi_to").val() != "") {
                if (!CompareDate("dNgayTraLoi_from", "dNgayTraLoi_to")) {
                    return false;
                }
            }--%>
            //$("#ketqua_tracuu").show().html("<p class='tcenter'><img src='/Images/ajax-loader.gif'/></p>");
            //    $.ajax({
            //    type: "post",
            //    url: "/Kiennghi/Ajax_Tracuu_result",
            //        data: $("#form_").serialize(),
            //        success: function (ok) {
            //            $("#ketqua_tracuu").html(ok);
            //        }
            //});
            //var pramt = $("#form_").serialize();
            //window.location = "/Kiennghi/Tracuu/?" + pramt;
            //return false;

            var pramt = $("#form_").serialize();
            //location.href = "/Kiennghi/Tonghop_TW/?q=" + $("#q").val()+"&" + pramt;

            $("#ip_data").empty().html("");
            $('#loadData').show();
            $.ajax({
                type: "post",
                url: "<%=Url.Action("Tracuu", "Kiennghi")%>",
                data: pramt,
               success: function (res) {
                   if (res) {
                       $('#loadData').hide();
                       $("#ip_data").empty().html(res.data);
                   } else {
                       $('#loadData').hide();
                       alert("Lỗi tìm kiếm tập hợp kiến nghị!");
                   }
               }
           });
            return false;  }
       
    </script>
</asp:Content>
