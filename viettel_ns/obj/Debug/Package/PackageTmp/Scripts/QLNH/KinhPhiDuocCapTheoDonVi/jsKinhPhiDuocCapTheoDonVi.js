var CONFIRM = 0;
var GUID_EMPTY = '00000000-0000-0000-0000-000000000000';
var ERROR = 1;
var TBL_NCCQCT = "tblKinhPhiDuocCapTheoDonViChiTiet";
var arrChiQuy = [];
var data = [];
var arrDonvi = [];
var arrBQuanly = [];

$(document).ready(function ($) {
   
});

function FilterInComboBox(params, data) {
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === '') {
        return data;
    }

    // Do not display the item if there is no 'text' property
    if (typeof data.text === 'undefined') {
        return null;
    }

    if (data.text.toUpperCase().indexOf(params.term.toUpperCase()) > -1) {
        var modifiedData = $.extend({}, data, true);
        //modifiedData.text += ' (matched)';
        return modifiedData;
    }

    return null;
}

function ResetChangePage() {
    var iDonvi = $("#iDonvi").val();
    var dTuNgay = null;
    var dDenNgay = null;
    GetListData(dTuNgay, dDenNgay, iDonvi);
}

function GetListData(dTuNgay, dDenNgay, iDonvi) {
    $.ajax({
        type: "POST",
        dataType: "html",
        async: false,
        url: "/QLNH/KinhPhiDuocCapTheoDonVi/KinhPhiDuocCapTheoDonViSearch",
        data: { dTuNgay, dDenNgay, iDonvi},
        success: function (data) {
            $("#lstDataView").html(data);
            $("#txtTuNgay").val(dTuNgay);
            $("#txtDenNgay").val(dDenNgay);
            $("#iDonvi").val(iDonvi);
        }
    });
}


function ChangePage() {
    var dTuNgay = $("#txtTuNgay").val();
    var dDenNgay = $("#txtDenNgay").val();
    var iDonvi = $("#iDonvi").val();
    GetListData(dTuNgay, dDenNgay, iDonvi);

}




