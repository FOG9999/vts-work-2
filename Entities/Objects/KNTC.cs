using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Objects
{
    public class KNTCDON
    {
        public decimal STT { get; set; }
        public decimal IDON { get; set; }
        public string CTEN { get; set; }
        public string CNOIDUNG { get; set; }
        public DateTime DNGAYNHAN { get; set; }
        public string CNGUOIGUI_DIACHI { get; set; }
        public string CTENTINH { get; set; }
        public string CTENHUYEN { get; set; }
        public decimal IDONVITHULY { get; set; }
        public decimal ITHAMQUYEN { get; set; }
        public decimal IDUDIEUKIEN { get; set; }
        public decimal ITINHTRANGXULY { get; set; }
        public decimal ICANHBAO { get; set; }
        public DateTime INGAYQUYDINH { get; set; }
        public decimal IUSER { get; set; }
        public decimal IUSER_DUOCGIAOXULY { get; set; }
        public decimal IDUDIEUKIEN_KETQUA { get; set; }
        public decimal IDONTRUNG { get; set; }
        public decimal ILUUTHEODOI { get; set; }
        public string CNGUOIGUI_TEN { get; set; }
        public decimal IDONVITIEPNHAN { get; set; }
        public string DONVITIEPNHAN { get; set; }
        public decimal IDANHGIA { get; set; }
        public string CGHICHUDANHGIA { get; set; }
        public decimal TOTAL { get; set; }
    }
    public class KNTCDON_MOICAPNHAT
    {
        public decimal STT { get; set; }
        public decimal IDON { get; set; }
        public string CNOIDUNG { get; set; }
        public DateTime DNGAYNHAN { get; set; }
        public string CNGUOIGUI_DIACHI { get; set; }
        public string CTENTINH { get; set; }
        public string CTENHUYEN { get; set; }
        public string CNGUOIGUI_TEN { get; set; }
        public decimal TOTAL { get; set; }
    }
    public class DONTRACUU
    {
        public decimal STT { get; set; }
        public decimal IDON { get; set; }
        public string CTEN { get; set; }
        public string CNOIDUNG { get; set; }
        public decimal TINHTRANG { get; set; }
        public string CNGUONDON { get; set; }
        public string CTINHTHANH { get; set; }
        public string CLOAIDON { get; set; }
        public string CLINHVUC { get; set; }
        public string CNOIDUNGDON { get; set; }
        public string CTINHCHAT { get; set; }
        public DateTime DNGAYNHAN { get; set; }
        public string CGHICHU { get; set; }
        public string CNGUOIGUI_TEN { get; set; }
        public decimal IDONVITIEPNHAN { get; set; }
        public decimal IDONVITHULY { get; set; }
        public string DONVITIEPNHAN { get; set; }
        public decimal IDANHGIA { get; set; }
        public string CTENHUYEN { get; set; }
        public string CNGUOIGUI_DIACHI { get; set; }
        public decimal ISOLUONGTRUNG { get; set; }
        public decimal SODONTRUNG { get; set; }
        public decimal IDELETE { get; set; }
        public decimal IUSER { get; set; }
        public decimal TOTAL { get; set; }
    }
    public class LOAIKHIEUTO
    {
        public decimal STT { get; set; }
        public string CTEN { get; set; }
        public decimal SOLUONG { get; set; }
        public decimal TYLE { get; set; }

    }

    public class CONGVANCHUYENDON
    {
        public string CNGUOIGUI_TEN { get; set; }
        public string CNOIDUNG { get; set; }
        public decimal ILOAIDON { get; set; }
        public decimal ILINHVUC { get; set; }
        public string CSOVANBAN { get; set; }
        public Nullable<DateTime> DNGAYBANHANH { get; set; }
        public Nullable<DateTime> DNGAYNHAN { get; set; }
        public string CTEN { get; set; }
        public decimal ITINHTRANGXULY { get; set; }
        public string CLUUTHEODOI_LYDO { get; set; }
        public string GHICHU_XULY { get; set; }
        public string CLOAI { get; set; }
        public decimal ICOQUANBANHANH { get; set; }
        public decimal ICOQUANNHAN { get; set; }
        public decimal IDON { get; set; }
        public decimal IVANBAN { get; set; }
    }

    public class Theodoigiaiquyetdon
    {
        public string CNGUOIGUI_DIACHI { get; set; }
        public string CNGUOIGUI_TEN { get; set; }
        public string CNOIDUNG { get; set; }
        public string CSOVANBAN { get; set; }
        public DateTime DNGAYBANHANH { get; set; }
        public DateTime DNGAYNHAN { get; set; }
        public string CTEN { get; set; }
        public string CTENCOQUAN1 { get; set; }
        public string CTENCOQUAN2 { get; set; }
        public string GHICHU_XULY { get; set; }
        public string CLOAI { get; set; }

    }

    public class BAOCAODONTHUHANGTUAN
    {
        public string CNGUOIGUI_TEN { get; set; }
        public string CNGUOIGUI_DIACHI { get; set; }
        public string CNOIDUNG { get; set; }
        public string CNOIDUNGVB { get; set; }
        public string CSOVANBAN { get; set; }
        public DateTime DNGAYBANHANH { get; set; }
        public DateTime DNGAYNHAN { get; set; }
        public string CTENCOQUAN1 { get; set; }
        public string CTENCOQUAN2 { get; set; }
        public string CTENCOQUAN3 { get; set; }
        public decimal ITINHTRANGXULY { get; set; }
        public decimal IDOITUONGGUI { get; set; }
        public string CLUUTHEODOI_LYDO { get; set; }
        public string CGHICHU { get; set; }
        public string GHICHU_XULY { get; set; }
        public string CLOAI { get; set; }
        public decimal IDON { get; set; }

    }
    public class NOIGUIDON
    {
        public decimal STT { get; set; }
        public string CTEN { get; set; }
        public decimal SOLUONG { get; set; }
        public decimal TYLE { get; set; }

    }
    public class KNTC_REPORT_CHUATRALOI
    {
        public string STT { get; set; }
        public decimal IDOITUONGGUI { get; set; }
        public Nullable<DateTime> DNGAYBANNHANH { get; set; }
        public string CSOVANBAN { get; set; }
        public string CNGUOIGUITEN { get; set; }
        public string CNOIDUNG { get; set; }
        public string CCOQUANNHAN { get; set; }
        public string CLOAIDON { get; set; }
        public string FIRSTTITLE { get; set; }
        public Boolean ISMERGE { get; set; }
        public int ISTITLE { get; set; }
        public int ISBOLD { get; set; }

    }
    public class KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D
    {
        public string STT { get; set; }
        public decimal IDOITUONGGUI { get; set; }
        public Nullable<DateTime> DNGAYBANNHANH { get; set; }
        public string CSOVANBAN { get; set; }
        public string CNGUOIGUITEN { get; set; }
        public string CNOIDUNG { get; set; }
        public string CCOQUANNHAN { get; set; }
        public string CLOAIDON { get; set; }
        public string FIRSTTITLE { get; set; }
        public int ISBOLD { get; set; }
        public decimal ILINHVUC { get; set; }
        public Boolean ISMERGE { get; set; }
        public int ISTITLE { get; set; }
        public Boolean FIRSTROW { get; set; }
    }
    public class THAMQUYENGIAIQUYET
    {
        public decimal STT { get; set; }
        public string CTEN { get; set; }
        public decimal SOLUONG { get; set; }
        public decimal TYLE { get; set; }

    }
    public class NGUOINHAPDON
    {
        public decimal STT { get; set; }
        public string CTEN { get; set; }
        public decimal SOLUONG { get; set; }
        public decimal TYLE { get; set; }

    }
    public class NGUOIXULY
    {
        public decimal STT { get; set; }
        public string CTEN { get; set; }
        public decimal SOLUONG { get; set; }
        public decimal TYLE { get; set; }

    }
    public class COQUANCHUYENDON
    {
        public decimal STT { get; set; }
        public string CTEN { get; set; }
        public decimal SOLUONG { get; set; }
        public decimal TYLE { get; set; }

    }
    public class TRUNGDON
    {
        public decimal STT { get; set; }
        public string CTENDIADANH { get; set; }
        public decimal SODONTRUNG { get; set; }
        public string CNOIDUNGDON { get; set; }
        public decimal SOLANTRUNG { get; set; }

    }
    public class TONGSODON
    {
        public decimal STT { get; set; }
        public string CLOAIKHIEUTO { get; set; }
        public decimal SODONDUDIEUKIEN { get; set; }
        public decimal SODONTRUG { get; set; }
        public decimal TYLE { get; set; }

    }
    public class CHIITETDON
    {
        public decimal STT { get; set; }
        public decimal SODON { get; set; }
        public DateTime NGAYVAOSO { get; set; }
        public string HOVATEN { get; set; }
        public string DIACHI { get; set; }
        public decimal SONGUOI { get; set; }
        public decimal SOLAN { get; set; }
        public string NOIDUNG { get; set; }
        public string LOAIDON { get; set; }
        public string CHUYENDEN { get; set; }

    }
    public class DIABAN
    {
        public decimal STT { get; set; }
        public string CTEN { get; set; }
        public string CDIABAN { get; set; }
        public decimal SOLUONG { get; set; }

    }
    public class DIABANTONG
    {
        public decimal STT { get; set; }
        public string CTEN { get; set; }
        public decimal SOLUONG { get; set; }

    }
    public class SOLIEUDON
    {
        public decimal STT { get; set; }
        public string CHUYENVIEN { get; set; }
        public decimal TONGNHANDON { get; set; }
        public decimal HANHCHINH { get; set; }
        public decimal TUPHAP { get; set; }
        public decimal KHAC { get; set; }
        public decimal DDKTONGNHANDON { get; set; }
        public decimal DDKTONGXULY { get; set; }
        public decimal DDKSODON { get; set; }
        public decimal DDKCHUYEN { get; set; }
        public decimal DDKKHONGCHUYEN { get; set; }
        public decimal DDKDANGNGHIENCUU { get; set; }
        public decimal DDKDLOAISOBO { get; set; }
        public decimal CHUAXULY { get; set; }
        public decimal TONG { get; set; }
        public decimal QHSODON { get; set; }
        public decimal QHCHUYEN { get; set; }
        public decimal QHDANGNGHIENCUU { get; set; }
        public decimal QHKHONGCHUYEN { get; set; }

        public decimal TBSODON { get; set; }
        public decimal TBCHUYEN { get; set; }
        public decimal TBDANGNGHIENCUU { get; set; }
        public decimal TBKHONGCHUYEN { get; set; }
        public decimal KSODON { get; set; }
        public decimal KCHUYEN { get; set; }
        public decimal KDANGNGHIENCUU { get; set; }
        public decimal KKHONGCHUYEN { get; set; }
        public decimal TONGXULY { get; set; }
        public decimal TONGCHUYEN { get; set; }
        public decimal TONGDANGNGHIENCUU { get; set; }
        public decimal TONGLUU { get; set; }

    }
    public partial class DONKHIEUTO
    {
        public decimal ID_PARENT_NGUONDON { get; set; }
        public decimal ID_PARENT_LINHVUC { get; set; }
        public string CTEN_NHOM_LINHVUC { get; set; }
        public string CTEN_NHOM_NGUONDON { get; set; }
        public Nullable<decimal> IDONTRUNG { get; set; }
        public Nullable<decimal> ILOAIDON { get; set; }
        public Nullable<decimal> ITINHCHAT { get; set; }
        public Nullable<decimal> INGUONDON { get; set; }
        public Nullable<decimal> INOIDUNG { get; set; }
        public Nullable<decimal> ITHAMQUYEN { get; set; }
        public Nullable<decimal> ITHULY { get; set; }
        public Nullable<decimal> IDONVITHULY { get; set; }
        public Nullable<decimal> ITINHTRANGXULY { get; set; }
        public Nullable<System.DateTime> DNGAYNHAN { get; set; }
        public Nullable<decimal> IDIAPHUONG_0 { get; set; }
        public Nullable<decimal> IDIAPHUONG_1 { get; set; }
        public Nullable<decimal> IDIAPHUONG_2 { get; set; }
        public string CNGUOIGUI_TEN { get; set; }
        public string CNGUOIGUI_DIACHI { get; set; }
        public string CNGUOIGUI_CMND { get; set; }
        public Nullable<decimal> IDOANDONGNGUOI { get; set; }
        public Nullable<decimal> ISONGUOI { get; set; }
        public string CDIACHI_VUVIEC { get; set; }
        public string CNOIDUNG { get; set; }
        public string CFILE { get; set; }
        public Nullable<decimal> ITINHTRANG_NOIBO { get; set; }
        public Nullable<decimal> ITINHTRANG_DONVIXULY { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public Nullable<decimal> IUSER { get; set; }
        public Nullable<decimal> IUSER_GIAOXULY { get; set; }
        public string CMADON { get; set; }
        public Nullable<decimal> INGUOIGUI_QUOCTICH { get; set; }
        public Nullable<decimal> INGUOIGUI_DANTOC { get; set; }
        public Nullable<decimal> IUSER_DUOCGIAOXULY { get; set; }
        public decimal IDUDIEUKIEN { get; set; }
        public decimal IDUDIEUKIEN_KETQUA { get; set; }
        public decimal ILINHVUC { get; set; }
        public decimal ILUUTHEODOI { get; set; }
        public string CLUUTHEODOI_LYDO { get; set; }
    }

    public class BAOCAOTHANG
    {
        public decimal IDON { get; set; }
        public decimal IDON_VANBAN { get; set; }
        public decimal INGUONDON { get; set; }
        public decimal IDOITUONGGUI { get; set; }
        public decimal ITINHTRANGXULY { get; set; }
        public string CSOVANBAN { get; set; }
        public Nullable<DateTime> DNGAYBANHANH { get; set; }
        public string COQUANBANHANH     { get; set; }
        public string NOIDUNGVANBAN { get; set; }
        public string GHICHUVANBAN { get; set; }
        public string CNGUOIGUI_TEN { get; set; }
        public string DIACHI { get; set; }
        public string NOIDUNGDON { get; set; }
        public string CDONVIXULY  { get; set; }
        public Nullable<decimal> IDONVITHULY {get; set; }
        public string GHICHUDON { get; set; }
        public Nullable<decimal> ICOQUANBANHANH { get; set; }
    }
    public class Document
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }

    public class ImportResult
    {
        public bool Status { get; set; }
        public int TotalRecord { get; set; }
        public int TotalImportFail { get; set; }
        public Dictionary<int, string> ListRecordFailIndex { get; set; } = new Dictionary<int, string>();
        public string Message { get; set; }
    }
}
