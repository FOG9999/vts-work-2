    <%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kiến nghị cử tri mới cập nhật
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
 <%: Html.Partial("../Shared/_Left_Knct") %>
    <%--<link href="../../css/ext/ext-theme-neptune-all4.css" rel="stylesheet" />
    <link href="../../css/ext/ext-theme-neptune-all_02.css" rel="stylesheet" />
    <script src="../../js/ext/shared/ext4.js"></script>
    <script src="../../js/ext/shared/ext-all.js"></script>
    <script src="../../js/ext/kiennghi/moicapnhat1.js"></script>--%>

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
				    <span>Kiến nghị mới cập nhật</span>
			    </li>
		    </ul>
		    
	    </div>  
            <div class="function_chung">
                <a <%=ViewData["bt-xuat-bao-cao"] %> onclick="ShowPopUp('', '/Kiennghi/Ajax_export_moicapnhat_popup/')"  data-original-title="Xuất báo cáo" rel="tooltip" href="javascript:void(0)" class="add btn_f blue" ><i class="icon-print"></i></a>
                <a <%=ViewData["bt-gop"] %> onclick="Gop_KienNghi()"  data-original-title="Tổng hợp kiến nghị" rel="tooltip" href="javascript:void(0)" class="add btn_f blue" style="display:none;"><i class="icon-signin"></i></a>
                <a <%=ViewData["bt-add-tonghop"] %> onclick="CheckKienNghiChon()"  data-original-title="Tạo tổng hợp và chuyển xử lý" rel="tooltip" href="javascript:void(0)"  class="add btn_f blue" ><i class="icon-list-alt"></i></a>
                <a <%=ViewData["bt-add"] %> onclick="ShowPageLoading()"  data-original-title="Thêm mới kiến nghị" rel="tooltip" href="/Kiennghi/Themmoi/" class="add btn_f blue" ><i class="icon-plus-sign"></i></a>
                <form class="search" id="form_search" method="get" onsubmit="return TimKiem();">                    
                    <input type="text" name="q" id="q" value="" placeholder="Nội dung, từ khóa">
                    <button type="button" title="Tìm kiếm" onclick="TimKiem()" class="btn_f blue"><i class="icon-search"></i></button>
                    <button type="button" title="Tìm kiếm" onclick="ShowTimKiem('/Kiennghi/Ajax_search_moicapnhat/','search_place')" style="" id="showbut" class="btn_f blue"><i class="icon-zoom-in"></i></button>
                    <button type="button" title="Tìm kiếm nâng cao" onclick="HideTimKiem('search_place')" class="btn_f blue" id="hidebut" style="display: none"><i class="icon-zoom-out"></i></button>
                </form>
               
            </div>      <div id="search_place"></div> 
                  <%--<div id="grid"></div>--%>
        <div class="row-fluid">
            <div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-list"></i> 
                            Kiến nghị mới cập nhật                            
					    </h3>
                        <div class="pull-right box-title-header">
                            <form id="form_header">
                                     <%--<select class="chosen-select" name="iKyHop" id="iKyHop" onchange="ChangeDonVi_KyHop();">
                                    <%=ViewData["opt-khoa-kyhop"] %>
                                    </select>--%>                                
                                 <select class="chosen-select" name="iDoan" id="iDoan" onchange="ChangeDonVi_KyHop();">
                                        <%=ViewData["opt-coquan"] %>
                                 </select>
                                <select class="chosen-select" name="iDonViXuLy_Parent" id="iDonViXuLy_Parent" onchange="DoiThamQuyenDonVi(this.value);">
                                    <%=ViewData["opt-thamquyen"] %>
                                 </select>
                                <select class="chosen-select" name="iDonViXuLy" id="iDonViXuLy" onchange="ChangeDonVi_KyHop();">
                                    <%=ViewData["opt-thamquyen-xuly"] %>
                                 </select>                               
                            </form>
                        </div>
				    </div>
				    <div class="box-content nopadding">              
                        <table class="table table-bordered table-condensed <%--table-striped--%>">
                            <thead>
                                <tr >
                                    <th width="3%" class="tcenter">STT </th>
                                    <th width="3%" class="tcenter">Chọn </th>
                                    <th class="tcenter">Nội dung </th>  
                                    <th class="tcenter"  width="15%">Tiếp nhận </th>                                                 
                                    <th class="tcenter" width="10%" nowrap>Chức năng</th>                                           
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
        
        function CheckKienNghiChon() {
            $.post("/Kiennghi/Ajax_Checkkiennghichontonghop", "", function (ok) {
                if (ok == "1") {
                    location.href = "/Kiennghi/Chontonghop/";
                } else if (ok == "-1") {
                    $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                       '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                        '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Chọn kiến nghị trước khi thêm vào tổng hợp</h3>' +
                        ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
						'</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                         '<p>Vui lòng chọn kiến nghị trước khi thêm vào tổng hợp</p>' +
                         '<div class="form-actions nomagin tright">' +
                         ' <button type="submit" class="btn btn-warning btn-focus" id="btn-submit">Quay lại</button></div></form></div></div></div></div></div>' +
                          ' </div></div>');
                } else {
                    $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                       '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                        '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Tổng hợp kiến nghị có nhiều đơn vị thẩm quyền giải quyết</h3>' +
                        ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
						'</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                         '<p>Vui lòng chọn các kiến nghị cùng <strong>Thẩm quyền đơn vị giải quyết</strong> trước khi tạo tổng hợp kiến nghị</p>' +
                         '<div class="form-actions nomagin tright">' +
                         ' <button type="submit" class="btn btn-warning btn-focus" id="btn-submit">Quay lại</button></div></form></div></div></div></div></div>' +
                          ' </div></div>');
                }
            });
            return false;
        }
        function ThemVaoTongHop(id) {
            $.post("/Kiennghi/Ajax_Themkiennghivaotonghop", "id=" + id, function (ok) {
                //$("#q_data").html(ok);
            });
        }
        function DoiThamQuyenDonVi(val) {
            $.post("/Kiennghi/Ajax_Get_ThamQuyen_DonVi", '&val=' + val, function (data) {
                $("#TrungUong").show();
                $("#iDonViXuLy").html('<select name="iDonViXuLy" id="iDonViXuLy" class="chosen-select">' + data + '</select>');
                $("#iDonViXuLy").trigger("liszt:updated");
                $("#iDonViXuLy").chosen();
            });
        }
        function Gop_KienNghi() {
            $.post("/Kiennghi/Ajax_Check_Listkiennghi_gop", "", function (ok) {
                //alert(ok);
                if (ok == "1") {
                    ShowPopUp('', '/Kiennghi/Ajax_Add_Kiennghi_gop');
                } else if (ok == "0") {
                    alert("Vui lòng chọn nhiều hơn một kiến nghị để tổng hợp");
                } else if (ok == "2") {
                    $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
                       '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
                        '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Kiến nghị gộp có nhiều đơn vị thẩm quyền giải quyết</h3>' +
                        ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
						'</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                         '<p>Vui lòng chọn các kiến nghị cùng <strong>Thẩm quyền đơn vị giải quyết</strong> trước khi tổng hợp kiến nghị</p>' +
                         '<div class="form-actions nomagin tright">' +
                         ' <button type="submit" class="btn btn-warning btn-focus" id="btn-submit">Quay lại</button></div></form></div></div></div></div></div>' +
                          ' </div></div>');
                }
            });
        }
        function ChangeDonVi_KyHop() {
            var pramt = $("#form_header").serialize();
            //window.location = "/Kiennghi/Moicapnhat/?" + $("#form_").serialize();
            //alert(window.location);
            location.href = "/Kiennghi/Moicapnhat/?" + pramt;
            //location.href = "/Kiennghi/Moicapnhat/?iDoan=" + $("#iDonVi").val() + "&iKyHop=" + $("#iKyHop").val() + "";
        }
        function TimKiem() {
            //var pramt = $("#form_header").serialize();
            //location.href = "/Kiennghi/Moicapnhat/?q=" + $("#q").val();
            $("#ip_data").empty().html("");
            $('#loadData').show();
            $.ajax({
                type: "post",
                url: "<%=Url.Action("Moicapnhat", "Kiennghi")%>",
                data: { q: $("#q").val(), hidNormalSearch: 1 },
                success: function (res) {
                    if (res) {
                        $('#loadData').hide();
                        $("#ip_data").empty().html(res.data);
                    } else {
                        $('#loadData').hide();
                        alert("Lỗi tìm kiếm kiến nghị mới cập nhật!");
                    }
                }
            });
            return false;
        }
        
    </script>
</asp:Content>
