<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Entities.Models" %>
<%@ Import Namespace="Entities.Objects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  Cập nhật kiến nghị cử tri
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
				    <span>Cập nhật kiến nghị</span>
                   
			    </li>
		    </ul>
		    <div class="close-bread">
			    <a href="#"><i class="icon-remove"></i></a>
		    </div>
	    </div>       
                    <% KN_KIENNGHI kn = (KN_KIENNGHI)ViewData["kn"]; %> 
                    <% UserInfor user = (UserInfor)ViewData["user"];%>
                  <div class="row-fluid">
                <div class="span12">
                    <div class="box box-color box-bordered">
                        <div class="box-title">
                            <h3><i class="icon-tags"></i>Cập nhật kiến nghị</h3>

                        </div>
                        <div class="box-content" style="text-align: left;">

                            <form method="post" name="_form" id="_form" onsubmit="return CheckForm();" enctype="multipart/form-data" class="form-horizontal form-column">
                                <div class="row-fluid">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Kiến nghị gửi đến<i class="f-red">*</i></label>
                                        <div class="controls" <%=user.tk_action.is_lanhdao && user.user_login.IUSER == kn.IUSER ? "" : "disabled"%>">
                                            <span class="span4">
                                                <label><input class="nomargin" <%=user.tk_action.is_lanhdao && user.user_login.IUSER == kn.IUSER ? "" : "disabled"%> type="radio" onclick="ChangeDoiTuongGui()"  name="iDoiTuongGui" value="0" <%=kn.IDOITUONGGUI == 0 ? "checked" : ""%>/>
                                                Quốc hội</label>
                                            </span>
                                            <span class="span4">
                                                <label><input class="nomargin" <%=user.tk_action.is_lanhdao && user.user_login.IUSER == kn.IUSER ? "" : "disabled"%> type="radio" onclick="ChangeDoiTuongGui()" name="iDoiTuongGui" value="1" <%=kn.IDOITUONGGUI == 1 ? "checked" : ""%>/>
                                                Hội Đồng Nhân Dân</label>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">Số kiến nghị<i class="f-red">*</i></label>
                                        <div class="controls">
                                            <div class="input-block-level" id="cSoKiennghiChange">
                                            <input type="text"  id="cSoKiennghi" name="cSoKiennghi" value="<%=kn.CMAKIENNGHI%>" class='input-block-level' readonly="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Chương trình <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level" id="div_ChuongTrinh">
                                                    <select name="iChuongTrinhTXCT" id="iChuongTrinhTXCT" class="chosen-select" onchange="updateTheoChuongTrinh(this.value)">
                                                        <option value="0">Chọn chương trình</option>
                                                        <%=ViewData["opt-chuongtrinhTXCT"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Kỳ họp <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level" id="div_KyHop" >
                                                    <select readonly ="true" id="iKyHop" name="iKyHop">
                                                        <option value="0">Chọn kỳ họp</option>
                                                        <%=ViewData["opt-KyHop"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Hình thức <i class="f-red">*</i></label>
                                            <div class="controls" id ="div_HinhThuc">
                                                <%=ViewData["check-hinhthuc"] %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="row-fluid">
                                   <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Đơn vị tiếp nhận <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level" id ="div_DonVi">
                                                    <select name="iDonViTiepNhan" id="iDonViTiepNhan" class="chosen-select">     
                                                        <%=ViewData["opt-doandaibieu"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                    
                                </div>
                                
                                <div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label ">Nguồn kiến nghị <i class="f-red">*</i></label>
                                            <div class="controls">
                                                   <div class="input-block-level">
                                                   <select id="lstNguonKN" multiple="multiple" name="lstNguonKN">
                                                        <%=ViewData["opt-nguonkiennghi"]%>
                                                    </select>
                                             </div>
                                            </div>
                                          
                                        </div>
                                    </div>
                                
                                
                                
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Nội dung kiến nghị <i class="f-red">*</i></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea  name="cNoiDung" rows="10" id="cNoiDung" class="input-block-level"><%=Server.HtmlEncode(kn.CNOIDUNG) %> </textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    
                                        <div class="control-group">
                                            <label for="textfield" class="control-label">Thẩm quyền giải quyết<i class="f-red">*</i></label>
                                            <div class="controls">
                                                <%=ViewData["radio-thamquyen"] %>
                                                <div class="input-block-level" id="TrungUong">
                                                   <select name="iThamQuyenDonVi" id ="iThamQuyenDonVi" onchange="ChangeLinhVucByDonVI(this.value)" class="chosen-select">                                            
                                                        <%=ViewData["opt-donvithamquyen"] %>
                                                    </select>
                                                </div>
                                                <%--<div class="input-block-level" id="DiaPhuong" 
                                                    <% if (ViewData["diaphuong"].ToString() == "0") { %>
                                                    style="display:none"
                                                    <% } %>>
                                                    <% if (ViewData["is_dbqh"].ToString() == "1")
                                                        { %>
                                                   <select name="iThamQuyenDonVi_DiaPhuong" onchange="ChangeLinhVucByDonVI(this.value)" class="chosen-select">                                            
                                                        <%=ViewData["opt-donvithamquyen-diaphuong"] %>
                                                    </select>
                                                    <% }else { %>
                                                        <div >
                                                            <div class="span6">
                                                                    <select onchange="ChangeDiaPhuongParent(this.value)" name="iThamQuyenDonVi_DiaPhuong_Parent"  id="iThamQuyenDonVi_DiaPhuong_Parent" class="chosen-select">                                            
                                                                    <%=ViewData["opt-donvithamquyen-diaphuong-parent"] %>
                                                                    </select>
                                                            </div>
                                                            <div class="span6" id="diaphuong_child">
                                                                    <select name="iThamQuyenDonVi_DiaPhuong"  id="iThamQuyenDonVi_DiaPhuong" class="chosen-select">                                            
                                                                        <%=ViewData["opt-donvithamquyen-diaphuong"] %>
                                                                    </select>
                                                            </div>
                                                            </div>
                                                    <% } %>
                                                
                                            </div>--%>
                                        </div>
                                    </div>

                                    
                                </div>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Lĩnh vực </label>
                                            <div class="controls">
                                                <div class="input-block-level" id="div_linhvuc">
                                                    <select name="iLinhVuc" id="iLinhVuc" class="chosen-select">                                                 
                                                        <%=ViewData["opt-linhvuc"] %>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Ghi chú </label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea  name="cGhiChu" rows="10" id="cGhiChu" class="input-block-level"><%=Server.HtmlEncode(kn.CGHICHU) %> </textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="row-fluid">
                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Thẩm quyền giải quyết</label>
                                            <div class="controls">
                                                <div class="input-block-level">

                                                   <select name="iThamQuyenDonVi" onchange="ChangeLinhVucByDonVI(this.value)" class="chosen-select">                                            
                                                        <%=ViewData["opt-donvithamquyen"] %>
                                                    </select>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="span6">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Lĩnh vực </label>
                                            <div class="controls">
                                                <div class="input-block-level" id="div_linhvuc">
                                                    <select name="iLinhVuc" id="iLinhVuc" class="chosen-select">
                                                 
                                                <%=ViewData["opt-linhvuc"] %>
                                            </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="row-fluid" style="margin-top: 1%"  hidden ="hidden">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Địa chỉ</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <div class="span6">
                                                        <select name="iDiaPhuong0" onchange="ChangeTinhThanh(this.value)" id="iDiaPhuong0" class="chosen-select">                                                 
                                                            <%=ViewData["opt-tinh"] %>
                                                        </select>
                                                    </div>
                                                    <div class="span6" id="div_huyen">
                                                        <select name="iDiaPhuong1" id="iDiaPhuong1" class="chosen-select">                                                 
                                                            <%=ViewData["opt-huyen"] %>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="span12">
                                                    <input type="text" name="cDiaChi" value="<%=Server.HtmlEncode(kn.CDIACHI) %>" placeholder="Địa chỉ người gửi kiến nghị" class="input-block-level" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="row-fluid" style="margin-top: 1%">
                                    <div class="span12">
                                        <div class="control-group">
                                            <label for="textfield" class="control-label  ">Từ khóa tìm kiếm (tags)  <em>Cách nhau bởi dấu <strong class="f-red">";"</strong></em></label>
                                            <div class="controls">
                                                <div class="input-block-level">
                                                    <textarea  name="cTuKhoa" class="input-block-level"><%=Server.HtmlEncode(kn.CTUKHOA) %> </textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="row-fluid" style="margin-top: 1%">
                                    <div class="control-group">
                                        <label for="textfield" class="control-label">
                                            File đính kèm
                                       
                                            <small>Dung lượng tối đa 10Mb; định dạng doc, docx, pdf, jpg, png, xls, xlsx</small></label>
                                        <div class="controls">
                                             <%=ViewData["file"] %>
                                            <% for (int i = 1; i < 4; i++)
                                            {
                                                    string style_none = ""; if (i > 1) { style_none = "display:none; margin-top:5px;"; }
                                                    string change = "";
                                                    if (i < 3)
                                                    {
                                                        int j = i + 1;
                                                        change = "$('.upload"+j+"').show()";
                                                    }
                                                    %>
                                                <div class="input-group file-group upload<%=i %>" style="<%=style_none%>">
                                                    <span class="input-group-btn">
                                                        <span class="btn btn-success btn-file">
                                                            Duyệt file
                                                            <input onchange="CheckFileTypeUpload('file_upload<%=i %>','file_name<%=i %>');<%=change %>" 
                                                                name="file_upload<%=i %>" id="file_upload<%=i %>" type="file">
                                                        </span>
                                                    </span>
                                                    <input class="input-xlarge" disabled id="file_name<%=i %>" type="text">
                                                    <span class="btn btn-danger" onclick="$('#file_upload<%=i %>,#file_name<%=i %>').val('');" title="Hủy"><i class="icon-trash"></i></span>
                                                </div>
                                            <% } %> 
                                           
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid" style="border-bottom: 1px solid #fff">
                                    <div class="form-actions">
                                       <input type="hidden" name="id" value="<%=ViewData["id"] %>" />
                                        <button type="submit" class="btn btn-primary" id="btn-submit">Cập nhật</button>
                                        <a href="<%=Request.Cookies["url_return"].Value %>" class="btn btn-warning" onclick="ShowPageLoading()">Quay lại</a>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div> 
        
    </div>  
        </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#lstNguonKN').multiselect({
                enableCollapsibleOptGroups: true,
                includeSelectAllOption: false,
                selectAllText: 'Chọn toàn bộ',
                allSelectedText: 'Đã chọn toàn bộ',
                nonSelectedText: 'Chọn nguồn kiến nghị',
                nSelectedText: 'nguồn kiến nghị đã chọn'
            });
        });
        updateTheoChuongTrinh($("#iChuongTrinhTXCT").val());
        $("#iDonViTiepNhan").chosen();
        $("#TrungUong").show();
        function ChangeDoiTuongGui() {
            var iKyHop = $('#iKyHop').val();
            //if (!iKyHop) iKyHop = 0;
            //$.post("/Kiennghi/Ajax_GetOpt_KyHopKhoaHop", 'iKyHop=' + iKyHop + '&iDoiTuongGui=' + $("input[name='iDoiTuongGui']:checked").val(), function (data) {
            //    console.log(data);
            //    $("#iKyHop").html(data);
            //    if (data && data.length > 0) {
            //        $("#div_KyHop").html('<select class="chosen-select" onchange="ChangeKyHop()" id="iKyHop" name="iKyHop">' + data + '</select>');
            //        $("#iKyHop").chosen();
            //        $("#iKyHop").trigger("liszt:updated");
            //    }
            //    UpdateChuongTrinh();
            //});
            $.post("/Kiennghi/Ajax_GetOpt_SoKienNghi", 'iDoiTuongGui=' + $("input[name='iDoiTuongGui']:checked").val(), function (data) {
                console.log(data);
                $("#cSoKiennghi").html(data);
                if (data && data.length > 0) {
                    $("#cSoKiennghiChange").html(data);
                    $("#cSoKiennghi").trigger("liszt:updated");
                }
            });
        }

        function DoiThamQuyenDonVi(val) {
            debugger;
            $.post("/Kiennghi/Ajax_Get_ThamQuyen_DonVi", '&val=' + val, function (data) {
                $("#TrungUong").show();
                $("#iThamQuyenDonVi").html('<select name="iThamQuyenDonVi" id="iThamQuyenDonVi" class="chosen-select">' + data + '</select>');
                $("#iThamQuyenDonVi").trigger("liszt:updated");
                $("#iThamQuyenDonVi").chosen();
            });
        }

        function Option_DonVi(list, id_parent, level) {
            var str = "<option value='0'>Chọn đơn vị</option>";
            if (id_parent > 0) { str = ""; }

            var space_level = "";
            for (var l = 0; l < level; l++) {
                space_level += "- - - ";
            }
            var list_child = list.filter(obj => obj.IPARENT === id_parent);
            for (var i = 0; i < list_child.length; i++) {
                var t = list_child[i];
                var sel = "";
                str += "<option " + sel + " value='" + t["ICOQUAN"] + "'>" + space_level + t["CTEN"] + "</option>";
                var list_child1 = list.filter(obj => obj.IPARENT === t["ICOQUAN"]);
                if (list_child1.length > 0) {
                    var level_next = level + 1;
                    str += Option_DonVi(list, t["ICOQUAN"], level_next);
                }

            }
            return str;
        }
        function ChangeDiaPhuongParent(val) {
            if (val != 0) {
                $.post("/Kiennghi/Ajax_Get_Coquan_diaphuong_child_", 'id=' + val, function (data) {
                    //alert(data);
                    $("#diaphuong_child").html('<select name="iThamQuyenDonVi_DiaPhuong" id="iThamQuyenDonVi_DiaPhuong"><option value="0">Chọn đơn vị</option>' + data + '</select>');
                    //$('#iChuongTrinh').trigger('chosen:updated');
                    $("#iThamQuyenDonVi_DiaPhuong").chosen();
                });

                //$.post("/Kiennghi/Ajax_Get_Coquan_diaphuong_child", 'id=' + val, function (data) {
                //    //alert(data);
                //    $("#diaphuong_child").html('<select name="iThamQuyenDonVi_DiaPhuong" id="iThamQuyenDonVi_DiaPhuong"><option value="0">Chọn đơn vị</option>' + data + '</select>');
                //    //$('#iChuongTrinh').trigger('chosen:updated');
                //    $("#iThamQuyenDonVi_DiaPhuong").chosen();
                //});
            } else {
                $("#diaphuong_child").html('<select name="iThamQuyenDonVi_DiaPhuong" id="iThamQuyenDonVi_DiaPhuong"><option value="0">Chọn đơn vị</option>' + data + '</select>');
                //$('#iChuongTrinh').trigger('chosen:updated');
                $("#iThamQuyenDonVi_DiaPhuong").chosen();
            }
        }
        function CheckThamQuyen() {
            var check = $("input[type='radio'][name='iThamQuyen']:checked").val();
            if (check == 1) {
                $("#TrungUong").show();
                //$("#DiaPhuong").hide();
            } else {
                $("#TrungUong").hide();
                //$("#DiaPhuong").show();
            }
        }
        function CheckForm() {
            if ($("#iKyHop").val() == 0) {
                alert("Vui lòng chọn kỳ họp!");
                return false;
            }
            if ($("#iDonViTiepNhan").val() == 0) {
                alert("Vui lòng chọn đoàn ĐBQH tiếp nhận kiến nghị");
                return false;
            }
            if ($("#iChuongTrinhTXCT").val() == 0) {
                alert("Vui lòng chương trình!"); $("#iChuongTrinhTXCT").focus();
                return false;
            }
            if ($("#iNguonKienNghi").val() == 0) {
                alert("Vui lòng nguồn kiến nghị!"); $("#iNguonKienNghi").focus();
                return false;
            }
            if ($("#cNoiDung").val() == "") {
                alert("Vui lòng điền nội dung kiến nghị"); $("#cNoiDung").focus();
                return false;
            }
            if ($("#iThamQuyenDonVi").val() == 0) {
                alert("Vui lòng chọn đơn vị thẩm quyền!"); $("#iThamQuyenDonVi").focus();
                return false;
            }
            ShowPageLoading();
        }
        function ChangeKyHop() {
            UpdateChuongTrinh();
        }

        function ChangeDonViTiepNhan() {
            UpdateChuongTrinh();
        }
        function updateTheoChuongTrinh(val) {
            $.post("/Kiennghi/Ajax_Change_KyHop_ChuongTrinh", 'iChuongTrinh=' + val, function (data) {
                $("#div_KyHop").html('<select id="iKyHop"  name="iKyHop" readonly ="true">' + data + '</select>');

            }).done(function () {
                $.post("/Kiennghi/Ajax_Change_HinhThuc_ChuongTrinh", 'iChuongTrinh=' + val, function (data) {
                    $("#div_HinhThuc").html(data);

                })
            });
        }
        function UpdateChuongTrinh() {
            $.post("/Kiennghi/Ajax_Change_KyHop_ChuongTrinh", 'iKyHop=' + $('#iKyHop').val() + '&iDonViTiepNhan=' + $("#iDonViTiepNhan").val(), function (data) {
                console.log(data);
                if (data && data.length > 0) {
                    $("#div_ChuongTrinh").html('<select name="iChuongTrinhTXCT" id="iChuongTrinhTXCT" class="chosen-select"><option value="0">Chọn chương trình</option>' + data + '</select>');
                }
                else {
                    $("#div_ChuongTrinh").html('<select disabled name="iChuongTrinhTXCT" id="iChuongTrinhTXCT" class="chosen-select"><option value="0">Không có trong chương trình, kế hoạch tiếp xúc cử tri</option></select>');
                }
                $("#iChuongTrinhTXCT").chosen();
            });
        }
        function ChangeTinhThanh(val) {
            $.post("/Kiennghi/Ajax_Change_Tinhthanh", 'id=' + val, function (data) {
                //alert(data);
                $("#div_huyen").html('<select name="iDiaPhuong1" id="iDiaPhuong1" class="chosen-select">' + data + '</select>');
                //$('#iChuongTrinh').trigger('chosen:updated');
                $("#iDiaPhuong1").chosen();
            });
        }
        function ChangeLinhVucByDonVI(val) {
            $.post("/Kiennghi/Ajax_Change_LinhVuc_By_ID_DonVi", 'id=' + val, function (data) {
                //alert(data);
                $("#div_linhvuc").html('<select name="iLinhVuc" id="iLinhVuc" class="chosen-select">' + data + '</select>');
                //$('#iChuongTrinh').trigger('chosen:updated');
                $("#iLinhVuc").chosen();
            });
        }
    </script>

</asp:Content>
