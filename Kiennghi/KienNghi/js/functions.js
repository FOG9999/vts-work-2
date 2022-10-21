// JavaScript Document
var sitename = "";
var emailRegExp = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.([a-z]){2,4})$/;
var file_type = "doc,docx,pdf,jpg,png,xls,xlsx";

function openPopup(url) {
    opener = window.open(url, 'openPopup', 'toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,resizable=0,dependent=no,width=0,height=0');

}
function UpdateNgayKetThuc(val, id) {
    val = val.split("/");
    year = parseInt(val[2]) + 20;
    $("#" + id).val(val[0] + "/" + val[1] + "/" + year);
}
function DownLoadDocument(post, url) {
    $.post(sitename+url, post, function () { });
}
function ChangeTinhThanh(id_change, val) {
    $.post("/Home/Ajax_Change_Opt_tinhthanh", 'id=' + val, function (data) {
        $("#" + id_change).html(data);
        $("#iDiaPhuong_01").chosen();

        var iddiaphuong2 = $("#iDiaPhuong_2");
        if (iddiaphuong2.length > 0) {
            var str = "<select name='iDiaPhuong_2' id='iDiaPhuong_02' class='chosen-select'><option value='0'>Chọn xã/phường/thị trấn</option></select>";
            $("#iDiaPhuong_2").html(str);
            $("#iDiaPhuong_02").chosen();
        }
    });
    
}
function ChangeTinhThanh1(id_change, val) {
    $.post("/Home/Ajax_Change_Opt_tinhthanh", 'id=' + val, function (data) {
        $("#" + id_change).html(data);
     
    });

}
function ChangeHuyenXa(id_change, val) {
    $.post("/Home/Ajax_Change_Opt_huyenxa", 'id=' + val, function (data) {
        $("#" + id_change).html(data);
        $("#iDiaPhuong_02").chosen();
    });

}

function Get_Menu(id,uri, url) {
    $.post(url, "uri="+uri, function (data) {
        //alert(data);
        $("#" + id).html(data);
    });
}
function CheckFileExcel(idfile, id_namefile) {
    var file_excel = "xls,xlsx";
    file = $("#" + idfile).val();
    filetype_upload = file.split(".");
    typefile = filetype_upload[filetype_upload.length - 1];
    var filetype_check = file_excel.split(",");
    check = 0;
    for (i = 0; i < filetype_check.length; i++) {
        if (typefile == filetype_check[i]) { check = check + 1; break; }
    }
    if (check == 0) {
        alert("Định dạng file đính kèm không hợp lệ! Vui lòng chỉ chọn các định dạng: " + file_excel);
        $("#" + idfile).val("");
        $("#" + id_namefile).val("");

    } else {
        $("#" + id_namefile).val(file);
    }
}
function CheckFileTypeUpload(idfile,id_namefile) {
    file = $("#" + idfile).val();
    filetype_upload = file.split(".");
    typefile = filetype_upload[filetype_upload.length - 1];
    var filetype_check = file_type.split(",");
    check = 0;
    for (i = 0; i < filetype_check.length; i++) {
        if (typefile == filetype_check[i]) { check = check + 1; break; }
    }
    file_type_text = "doc, docx, pdf, jpg, png, xls, xlsx";
    if (check == 0) {
        alert("Định dạng file đính kèm không hợp lệ! Vui lòng chỉ chọn các định dạng: " + file_type_text);
        $("#" + idfile).val("");
        $("#" + id_namefile).val("");
        
    } else {
        $("#" + id_namefile).val(file);
    }
}
function CheckFileType1(idfile) {
    file = $("#" + idfile).val();
    filetype_upload = file.split(".");
    typefile = filetype_upload[filetype_upload.length - 1];
    var filetype_check = file_type.split(",");
    check = 0;
    for (i = 0; i < filetype_check.length; i++) {
        if (typefile == filetype_check[i]) { check = check + 1; break; }
    }
    file_type_text = "doc, docx, pdf, jpg, png, xls, xlsx";
    if (check == 0) {        
        alert("Định dạng file đính kèm không hợp lệ! Vui lòng chỉ chọn các định dạng: " + file_type_text);
        $("#" + idfile).val("");
        return false;        
    } else { 
        return true;
    }
}
function CheckFileType(idfile) {
    file = $("#" + idfile).val();
    filetype_upload = file.split(".");
    typefile = filetype_upload[filetype_upload.length - 1];
    var filetype_check = file_type.split(",");
    check = 0;
    for (i = 0; i < filetype_check.length; i++) {
        if (typefile == filetype_check[i]) { check = check + 1; break; }
    }
    if (check == 0) {
        if (!confirm("Định dạng file đính kèm không hợp lệ! Bạn vẫn muốn lưu mà không dùng file đính kèm?")) {
            return false;
        } else {
            //$(".yourBtn").html("");
            $("#" + idfile).val("");
            return true;
        }
    } else { 
        return true
    }
}

function AlertAction(stralert) {
    var div="<div id='titthongbao' class='alert alert-danger'><i class='icon-ok-sign'></i> "+stralert+"</div>";
    $("body").prepend(div);
    $("#titthongbao").fadeOut(5000);
}

function TopPage(height){
    $('html, body').animate({ scrollTop: height }, 'slow');
}
function ShowTimKiem(url, id) {
    if ($('#' + id) && $('#' + id)[0].innerHTML == "") {
        $("#" + id).show().html("<p class='tcenter'><img src='/Images/ajax-loader.gif' /></p>");
        $.post(sitename + url, "", function (data) {
            $("#" + id).html(data);
            $('.datepick').datepicker();
            $(".chosen-select").chosen();
        });
    }
    else {
        $("#" + id).show();
    }
    $('#showbut').hide();
    $('#hidebut').show();
}
function HideTimKiem( id) {
    $("#" + id).hide();
    $('.datepick').datepicker();
    $('#showbut').show();
    $('#hidebut').hide();
}
function ShowPopUp(post, url) {    
    HidePopup();    
    $("body").prepend('<div id="screen"><div id="loader"><div id="load1" class="spin"></div><div id="load2" class="spin"></div>'+
                        '<div id="load3" class="spin"></div><div id="load4" class="spin"></div><div id="load5" class="spin"></div></div></div>');
    $.post(sitename + url, post, function (data) {
        HidePopup();
        //alert(data);
        //alert(data);
        $("body").prepend(data);
        //alert(1);
        $('.datepick').datepicker();
        //$('input[type=text],textarea').focus();
        $("#screen").attr("onClick", "HidePopup()");
        //$("select").chosen();
    });    
}
function ShowPopUpSecond(post, url) {    

    $("body").prepend('<div id="screen2"><div id="loader"><div id="load1" class="spin"></div><div id="load2" class="spin"></div>' +
        '<div id="load3" class="spin"></div><div id="load4" class="spin"></div><div id="load5" class="spin"></div></div></div>');
    $.post(sitename + url, post, function (data) {
        //alert(data);
        HidePopupSecond();
        $("body").prepend(data);
        $('.datepick').datepicker();
    });
}
function HidePopup(){
    $("#screen").hide().remove();
    $("#popup").hide().remove();
}
function HidePopupSecond() {
    $("#screen2").hide().remove();
    $("#popup2").hide().remove();
}
function UpdateTrangthai(post, url) {
    //alert(post+url);
    $.post(sitename + url, post, function (data) {
        if (data == 1) {
            location.reload();
        } else {
            alert(data);
        }
    });
}
function UpdateStatus(post, url) {
    //alert(post+url);
    $.post(sitename + url, post, function (data) {
        if (data == 1) {
            AlertAction("Cập nhật thành công!");
        } else {
            alert(data);
        }
    });
}
function UpdateOrder(post, value, url) {
    if (isNaN(value)) {
        alert("Vui lòng chỉ nhập số!");
        return false;
    } else {
        //alert(post);
        $.post(sitename + url, post + "&value=" + value, function (data) {
            if (data == 1) {
                AlertAction("Cập nhật thành công!")
            } else { alert(data); }
        });
        
    }
}
function Delete_Refresh(id, url) {
    if (confirm("Bạn có muốn xóa hay không?")) {
        $.post(sitename + url, "id=" + id, function (data) {
            if (data == 1) {
                location.reload();
            } else {
                alert(data);
            }
        });
    }
}
function DeleteFile(id, url) {
    if (confirm("Bạn có muốn xóa hay không?")) {
        $.post(sitename + url, "id=" + id, function (data) {
            if (data == 1) {
                $("#file_" + id).hide().remove();
                AlertAction("Xóa thành công!")
            } else {
                alert(data);
            }
        });
    }
}
function DeletePage_Confirm(id, post, url, str_confirm) { //Xóa nhanh
    var alert_confirm = str_confirm;
    if (alert_confirm == "") { alert_confirm = "Bạn có muốn xóa hay không?"; }
    $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">'+
                       '<div class="container-fluid"><div class="row-fluid"><div class="span12">'+
                        '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Xác nhận xóa</h3>'+
                        ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>'+
						'</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
                         '<p>' + alert_confirm + '</p>' +
                         '<div class="form-actions nomagin tright"><button type="button" onclick="ConfirmDelete(\'' + post + '\',\'' + url + '\')" class="btn btn-primary">Đồng ý</button>' +
                         ' <button type="submit" class="btn btn-warning btn-focus" id="btn-submit">Hủy bỏ</button></div></form></div></div></div></div></div>' +
                          ' </div></div>');
    $("#btn-submit").focus();
    //ShowPopUp(post + "&url=" + url + "&str_confirm=" + alert_confirm, "/Home/Ajax_Confirm_delete");
    
    //if (confirm(alert_confirm)) {
    //    $.post(sitename + url, post, function (data) {
    //        if (data == 1) {
    //            location.reload();
    //        } else {
    //            alert(data);
    //        }
    //    });
    //}
}

function DeletePage_Confirm_Traloi(id, post, url, str_confirm) { //Xóa nhanh
    var alert_confirm = str_confirm;
    if (alert_confirm == "") { alert_confirm = "Bạn có muốn xóa hay không?"; }
    $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">' +
        '<div class="container-fluid"><div class="row-fluid"><div class="span12">' +
        '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Xác nhận xóa</h3>' +
        ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>' +
        '</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
        '<p>' + alert_confirm + '</p>' +
        '<div class="form-actions nomagin tright"><button type="button" onclick="ConfirmDelete_Traloi(\'' + post + '\',\'' + url + '\')" class="btn btn-primary">Đồng ý</button>' +
        ' <button type="submit" class="btn btn-warning btn-focus" id="btn-submit">Hủy bỏ</button></div></form></div></div></div></div></div>' +
        ' </div></div>');
    $("#btn-submit").focus();
}

function DeletePage_Confirm_KN_TONGHOP(id, post, url, str_confirm) { //Xóa nhanh
    var alert_confirm = str_confirm;
    if (alert_confirm == "") { alert_confirm = "Bạn có muốn loại kiến nghị hay không?"; }
    $("body").prepend('<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">'+
        '<div class="container-fluid"><div class="row-fluid"><div class="span12">'+
            '<div class="box box-color"><div class="box-title"><h3><i class="icon-warning-sign"></i> Xác nhận loại kiến nghị</h3>'+
        ' <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>'+
        '</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" class="form-horizontal" onsubmit="HidePopup();">' +
        '<p>' + alert_confirm + '</p>' +
        '<div class="form-actions nomagin tright"><button type="button" onclick="ConfirmDelete(\'' + post + '\',\'' + url + '\')" class="btn btn-primary">Đồng ý</button>' +
        ' <button type="submit" class="btn btn-warning btn-focus" id="btn-submit">Hủy bỏ</button></div></form></div></div></div></div></div>' +
        ' </div></div>');
    $("#btn-submit").focus();
    //ShowPopUp(post + "&url=" + url + "&str_confirm=" + alert_confirm, "/Home/Ajax_Confirm_delete");

    //if (confirm(alert_confirm)) {
    //    $.post(sitename + url, post, function (data) {
    //        if (data == 1) {
    //            location.reload();
    //        } else {
    //            alert(data);
    //        }
    //    });
    //}
}
function ConfirmDelete(post, url) {
    //ShowPopUp("error=Đã có lỗi xảy ra trong quá trình xóa dữ liệu, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
    $.post(sitename + url, post, function (data) {
        if (data == 1) {
            //var url_refresh=window.location.href +"#delete";
            //location.href = url;
            //location.href = '/Home/Delete_success/?url=' + url_refresh;
            location.reload();
            AlertAction("Xóa tạm kỳ họp thành công");
        } else {
            //alert(data);
            ShowPopUp("error=Đã có lỗi xảy ra trong quá trình xóa dữ liệu, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
        }
    });
    
}
function ConfirmDelete_Traloi(post, url) {
    //ShowPopUp("error=Đã có lỗi xảy ra trong quá trình xóa dữ liệu, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
    $.post(sitename + url, post, function (data) {
        if (data == 1) {
            //var url_refresh=window.location.href +"#delete";
            //location.href = url;
            //location.href = '/Home/Delete_success/?url=' + url_refresh;
            location.reload();
            AlertAction("Xóa nội dung trả lời kiến nghị thành công");
        } else {
            //alert(data);
            ShowPopUp("error=Đã có lỗi xảy ra trong quá trình xóa dữ liệu, chúng tôi đã ghi nhận lỗi và sớm khắc phục chức năng này.", "/Home/Ajax_Error_ajax_submit/");
        }
    });

}
function DeletePage(id,post, url) { //Xóa nhanh
    if (confirm("Bạn có muốn xóa hay không?")) {
        $.post(sitename + url, post, function (data) {
            if (data == 1) {
                location.reload();
            } else {
                alert(data);
            }
        });
    }
}
function DeletePlace(id, post, url) { //Xóa nhanh
    if (confirm("Bạn có muốn xóa hay không?")) {
        $.post(sitename + url, post, function (data) {
            if (data == 1) {
                $("#" + id).hide().remove();
                AlertAction("Xóa thành công!")
            } else {
                alert(data);
            }
        });
    }
}
function str_split(string, split_length) {
    if (split_length === null) {
        split_length = 1;
    }
    if (string === null || split_length < 1) {
        return false;
    }
    string += '';
    var chunks = [],
      pos = 0,
      len = string.length;
    while (pos < len) {
        chunks.push(string.slice(pos, pos += split_length));
    }
    return chunks;
}
function str_replace(string, search, replace) {
    return string.split(search).join(replace);
}
function strrev(string) {
    string = string + '';
    var grapheme_extend = /(.)([\uDC00-\uDFFF\u0300-\u036F\u0483-\u0489\u0591-\u05BD\u05BF\u05C1\u05C2\u05C4\u05C5\u05C7\u0610-\u061A\u064B-\u065E\u0670\u06D6-\u06DC\u06DE-\u06E4\u06E7\u06E8\u06EA-\u06ED\u0711\u0730-\u074A\u07A6-\u07B0\u07EB-\u07F3\u0901-\u0903\u093C\u093E-\u094D\u0951-\u0954\u0962\u0963\u0981-\u0983\u09BC\u09BE-\u09C4\u09C7\u09C8\u09CB-\u09CD\u09D7\u09E2\u09E3\u0A01-\u0A03\u0A3C\u0A3E-\u0A42\u0A47\u0A48\u0A4B-\u0A4D\u0A51\u0A70\u0A71\u0A75\u0A81-\u0A83\u0ABC\u0ABE-\u0AC5\u0AC7-\u0AC9\u0ACB-\u0ACD\u0AE2\u0AE3\u0B01-\u0B03\u0B3C\u0B3E-\u0B44\u0B47\u0B48\u0B4B-\u0B4D\u0B56\u0B57\u0B62\u0B63\u0B82\u0BBE-\u0BC2\u0BC6-\u0BC8\u0BCA-\u0BCD\u0BD7\u0C01-\u0C03\u0C3E-\u0C44\u0C46-\u0C48\u0C4A-\u0C4D\u0C55\u0C56\u0C62\u0C63\u0C82\u0C83\u0CBC\u0CBE-\u0CC4\u0CC6-\u0CC8\u0CCA-\u0CCD\u0CD5\u0CD6\u0CE2\u0CE3\u0D02\u0D03\u0D3E-\u0D44\u0D46-\u0D48\u0D4A-\u0D4D\u0D57\u0D62\u0D63\u0D82\u0D83\u0DCA\u0DCF-\u0DD4\u0DD6\u0DD8-\u0DDF\u0DF2\u0DF3\u0E31\u0E34-\u0E3A\u0E47-\u0E4E\u0EB1\u0EB4-\u0EB9\u0EBB\u0EBC\u0EC8-\u0ECD\u0F18\u0F19\u0F35\u0F37\u0F39\u0F3E\u0F3F\u0F71-\u0F84\u0F86\u0F87\u0F90-\u0F97\u0F99-\u0FBC\u0FC6\u102B-\u103E\u1056-\u1059\u105E-\u1060\u1062-\u1064\u1067-\u106D\u1071-\u1074\u1082-\u108D\u108F\u135F\u1712-\u1714\u1732-\u1734\u1752\u1753\u1772\u1773\u17B6-\u17D3\u17DD\u180B-\u180D\u18A9\u1920-\u192B\u1930-\u193B\u19B0-\u19C0\u19C8\u19C9\u1A17-\u1A1B\u1B00-\u1B04\u1B34-\u1B44\u1B6B-\u1B73\u1B80-\u1B82\u1BA1-\u1BAA\u1C24-\u1C37\u1DC0-\u1DE6\u1DFE\u1DFF\u20D0-\u20F0\u2DE0-\u2DFF\u302A-\u302F\u3099\u309A\uA66F-\uA672\uA67C\uA67D\uA802\uA806\uA80B\uA823-\uA827\uA880\uA881\uA8B4-\uA8C4\uA926-\uA92D\uA947-\uA953\uAA29-\uAA36\uAA43\uAA4C\uAA4D\uFB1E\uFE00-\uFE0F\uFE20-\uFE26]+)/g;
    string = string.replace(grapheme_extend, '$2$1'); // Temporarily reverse
    return string.split('').reverse().join('');
}
function ConvertMoney(value) {    // chuyển số 00000000 sang định dạng tiền 00.000.000
    str_rev = strrev(value); strplit = str_split(str_rev, 3); count = strplit.length; var res = '';
    for (i = 0; i < count; i++) { res += strplit[i] + ','; }
    res = res.substring(0, res.length - 1); res = strrev(res);
    return res
}
function ChangeMoney(id) {//chuyển tiền 00.000.000 sang định dạng 00000000
    //value=trim(value);
    value = $("#" + id).val();
    value = str_replace(value, ",", "");
    value = str_replace(value, ".", "");
    $("#" + id).val(ConvertMoney(value));
}
function CheckNum(id) {    
    var val = str_replace($("#" + id).val(), ".", "");
    if (val == "") { $("#" + id).val(0); }
    val = str_replace($("#" + id).val(), ",", "");
    if (parseInt(val) < 0) { alert("Vui lòng chỉ nhập số dương!"); $("#" + id).focus(); $("#" + id).val(0); }
    if (isNaN(val)) {
        alert("Vui lòng chỉ nhập số!");
        $("#" + id).focus();
        $("#" + id).val(0);
    } else {
        //$("#" + id).val(val);
    }
}
function PlusFileUpload(class_next,name, num) {
    var num = parseInt(num) + 1;
    if (num > 3) {
        return "";
    } else {
        var upload = '<div class="input-group file-group ' + class_next + '" style=" margin-top:5px">' +
                   '<span class="input-group-btn"><span class="btn btn-success btn-file">'+
                   ' Duyệt file <input onchange="CheckFileTypeUpload(\'' + name + '_upload' + num + '\',\'' + name + '_name' + num + '\');PlusFileUpload(\'' + class_next + '\',\'' + name + '\', ' + num + ')" ' +
                   'name="' + name + '_upload' + num + '" id="' + name + '_upload' + num + '" type="file"></span></span>' +
                   '     <input class="input-xlarge" disabled id="' + name + '_name' + num + '" type="text">' +
                    '    <span class="btn btn-danger" onclick="$(\'#' + name + '_upload' + num + ',#' + name + '_name' + num + '\').val(\'\');" title="Hủy"><i class="icon-trash"></i></span>' +
                    '</div>'
        alert(upload);
        $("." + class_next ).last().after(upload);
    }
}
function getHeaderToken() {
    /*
    var token = $('input[name="__RequestVerificationToken"]').val();

    var headers = {};

    headers['__RequestVerificationToken'] = token;
    */
    //return headers;

}
function CreateNew_Token() {
   
    /*
    $.ajax({
        type: "GET",
        headers: getHeaderToken(),
        contentType: "application/json; charset=utf-8",
        url: "/Home/Ajax_Create_Token",
        success: function (data) {
            $("#token_id").html(data);
        }
    });
    */
}
function Validate_DateVN(id) {
    var date = $("#" + id).val();
    if (moment(date, 'DD/MM/YYYY', true).isValid()) {
        //alert("ok");
        return true;
    } else {
        alert("Vui lòng điền đúng định dạng ngày/tháng/năm !");
        $("#" + id).focus();
        $("#" + id).val("");
        return false;
    }
}
function CompareDate(from, to) {
    
    if ($("#" + from).val() != "" && $("#" + to).val() != "") {
        //var date_from = $("#" + from).val();
        //var date_to = $("#" + to).val();
        var date_from = moment($("#" + from).val(), 'DD/MM/YYYY');
        var date_to = moment($("#" + to).val(), 'DD/MM/YYYY');
        //alert(date_from + " - " + date_to);
        if (date_to>=date_from) {
            return true;
        } else {
            alert("Vui lòng chọn ngày kết thúc lớn hơn ngày bắt đầu !");
            $("#" + to).focus();
            $("#" + to).val("");
            return false;
        }
    }
}
function GetContentTimKiem(url, id, post) {
    $.post(url, post, function (data) {
        var typehead = data.split("|");
        $('#'+id).typeahead({
            source: typehead
        });
    });
}
function ChangeChosenSelect(id_select, url, id_change) {
    val = $("#" + id_select).val();
    $.post(url, 'id=' + val+'', function (data) {
        //alert(data);
        $("#" + id_change).html(data);
    });
}
function ShowTimKiem_Conf(post,url, id) {

    $("#" + id).show().html("<p class='tcenter'><img src='/Images/ajax-loader.gif' /></p>");
    $.post(sitename + url, post, function (data) {
        $("#" + id).html(data);
        $('.datepick').datepicker();
        document.getElementById('showbut').style.display = "none";
        document.getElementById('hidebut').style.display = "block";
        
    });
}
function ShowPageLoading() {
    $("body").prepend('<div id="screen"><div id="loader"><div id="load1" class="spin"></div><div id="load2" class="spin"></div>' +
        '<div id="load3" class="spin"></div><div id="load4" class="spin"></div><div id="load5" class="spin"></div></div></div>');
}

function HidePageLoading() {
    if ($("body #screen").length == 1) {
        $("body #screen").remove();
    }
}

function ChangePage(page, post_page, link, searchFormId, containerId, method, async) {
    if (!method) method = "POST";
    if (async == null || async == undefined) async = true;
    var formData = "";
    if (searchFormId) formData = $("#" + searchFormId).serialize() + "&";
    $.ajax({
        type: method,
        url: link,
        async: async,
        data: formData + "page=" + page + "&post_per_page=" + post_page + "&isLoadAjax=1",
        success: function (res) {
            if (res) {
                $("#" + containerId).empty().html(res.data);
            } else {
                alert("Lỗi phân trang");
            }
        }
    });
}