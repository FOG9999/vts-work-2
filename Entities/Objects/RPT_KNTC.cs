using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Objects
{
    public class RPT_KNTC_DONCHOXULIPHANLOAI
    {
        public decimal IDON { get; set; }
        public decimal STT { get; set; }
        public decimal IDOITUONGGUI { get; set; }
        public string CNGUOIGUI_TEN { get; set; }
        public string CNGUOIGUI_DIACHI { get; set; }
        public string CDONKHIEUNAI { get; set; }
        public string CDONTOCAO { get; set; }
        public string CDONKIENNGHIPHANANH { get; set; }
        public string CDONNOIDUNGKHAC { get; set; }
        public string CCHEDOCHINHSACH { get; set; }
        public string CDATDAINHACUA { get; set; }
        public string CCONGCHUCCONGVU { get; set; }
        public string CKHAC { get; set; }
        public string CTUPHAP { get; set; }
        public string CDANGDOANTHE { get; set; }
        public string CTHAMNHUNG { get; set; }
        public string CLINHVUCKHAC { get; set; }
        public string CDONDUDIEUKIENXULI { get; set; }
        public string CDONTRUNG { get; set; }
        public string CDONKHONGDUDIEUKIEN { get; set; }
        public string ROWTITLE { get; set; }
    }

    public class PRC_KNTC_IMPORT_LISTDON
    {
        public decimal IDON { get; set; }
        public decimal IDOITUONGGUI { get; set; }
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
        public string CNGUOIGUI_SDT { get; set; }
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
        public decimal IKHOA { get; set; }
        public decimal IDUDIEUKIEN_KETQUA { get; set; }
        public Nullable<decimal> ILINHVUC { get; set; }
        public decimal ILUUTHEODOI { get; set; }
        public string CLUUTHEODOI_LYDO { get; set; }
        public string CHITIETLYDO_LUUTHEODOI { get; set; }
        public decimal ISOLUONGTRUNG { get; set; }
        public decimal IDOMAT { get; set; }
        public decimal IDOKHAN { get; set; }
        public string CGHICHU { get; set; }
        public Nullable<decimal> IDONVITIEPNHAN { get; set; }
        public Nullable<decimal> IDANHGIA { get; set; }
        public string CGHICHUDANHGIA { get; set; }
        public Nullable<decimal> IDELETE { get; set; }
        public Nullable<decimal> IIDIMPORT { get; set; }
        public Nullable<decimal> ICANHBAO { get; set; }
        public Nullable<System.DateTime> INGAYQUYDINH { get; set; }
        public Nullable<decimal> ICHITIETLYDOLUUDON { get; set; }
        public Nullable<decimal> IPLSONGUOI { get; set; }
        public string TEN_DIAPHUONG1 { get; set; }
        public string TEN_DIAPHUONG2 { get; set; }
        public string TEN_LOAIDON { get; set; }
    }

    public class RPT_KNTC_DONVIDATRALOI_4A
    {
        public decimal STT { get; set; }
        public decimal IDON { get; set; }
        public string CTEN { get; set; }
        public string CNOIDUNG { get; set; }
        public string INOIDUNG { get; set; }
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
        public string HOTEN_DIACHI { get; set; }
        public string SOCONGVANCHUYEN { get; set; }
        public string SOCONGVANTRALOI { get; set; }
        public string COQUANCHUYENDEN { get; set; }
        public DateTime? NGAYBANHANHTRALOI { get; set; }
        public string NGAYBANHANH { get; set; }
        public string COQUANTRALOI { get; set; }
        public string NOIDUNGTRALOI { get; set; }
        public string VANBANGHICHU { get; set; }
        public string TT { get; set; }
        public string LOAIVANBAN { get; set; }


    }

    public class RPT_KNTC_CONGVANDONDOC_3F
    {
        public decimal STT { get; set; }
        public decimal IDON { get; set; }
        public string CTEN { get; set; }
        public string CNOIDUNG { get; set; }
        public string INOIDUNG { get; set; }
        public DateTime DNGAYNHAN { get; set; }
        public string CNGUOIGUI_DIACHI { get; set; }
        public string CTENTINH { get; set; }
        public string CTENHUYEN { get; set; }
        public decimal IDONVITHULY { get; set; }
        public decimal ITHAMQUYEN { get; set; }
        public decimal IDUDIEUKIEN { get; set; }
        public decimal ITINHTRANGXULY { get; set; }
        public decimal ICANHBAO { get; set; }
        public DateTime? INGAYQUYDINH { get; set; }
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
        public string HOTEN_DIACHI { get; set; }
        public string TT { get; set; }
        public string LOAIDON_KN { get; set; }
        public string LOAIDON_TC { get; set; }
        public string LOAIDON_PAKN { get; set; }
        public string LOAIDON_DCNNDN { get; set; }
        public string LINHVUC_HC { get; set; }
        public string LINHVUC_TP { get; set; }
        public string LINHVUC_DTT { get; set; }
        public string LINHVUC_TN { get; set; }
        public string LINHVUC_KHAC { get; set; }
        public string SOVANBAN { get; set; }
        public DateTime? NGAYBANHANHTRALOI { get; set; }
        public string NGAYBANHANHTRALOISTRING { get; set; }
        public DateTime? NGAYBAOCAO { get; set; }
    }

    public class RPT_KNTC_DONDATRALOI_4B
    {
        public string STENDONVI { get; set; }
        public decimal COUNTDONNHIEUNGUOIDUNGTENKYTRUOC { get; set; }
        public decimal COUNTDONMOTNGUOIDUNGTENKYTRUOC { get; set; }
        public decimal COUNTDONKHACKYTRUOC { get; set; }
        public decimal COUNTDONNHIEUNGUOIDUNGTENTRONGKY { get; set; }
        public decimal COUNTDONMOTNGUOIDUNGTENTRONGKY { get; set; }
        public decimal COUNTDONKHACTRONGKY { get; set; }
        public decimal COUNTSODONDAXULY { get; set; }
        public decimal COUNTDONKHIEUNAI { get; set; }
        public decimal COUNTDONTOCAO { get; set; }
        public decimal COUNTDONKIENNGHI { get; set; }
        public decimal COUNTDONNHIEUNOIDUNGKHAC { get; set; }
    }
    
    public class RPT_KNTC_DONCHITIET_4F
    {
        public string DIAPHUONG1 { get; set; }
        public string DIAPHUONG2  { get; set; }
        public string CNGUOIGUITEN { get; set; }
        public string ILOAIDON { get; set; }
        public string LOAIDON_KN { get; set; }
        public string LOAIDON_TC { get; set; }
        public string LOAIDON_KNA { get; set; }
        public string LOAIDON_KHAC { get; set; }
        public string LINHVUC_HC { get; set; }
        public string LINHVUC_TP { get; set; }
        public string LINHVUC_DTT { get; set; }
        public string LINHVUC_TN { get; set; }
        public string LINHVUC_KHAC { get; set; }
        public decimal ILINHVUC { get; set; }
        public string DUDIEUKIEN { get; set; }
        public string DONTRUNG { get; set; }
        public string KODUDIEUKIEN { get; set; }
        public string CHUYEN_GIAIQUYET { get; set; }
        public string DON_DOC_CQTQ { get; set; }
        public string TRA_LOI_HD { get; set; }
        public string DANG_NGHIEN_CUU { get; set; }
        public string LUU_THEO_DOI { get; set; }
        public string NHAN_TRA_LOI { get; set; }
        public string KO_LUU_THEO_DOI { get; set; }
        public decimal ITINHTRANGXULY { get; set; }
        public decimal ISFIRST { get; set; }
        public string STENDONVI { get; set; }
    }
    
    public class RPT_KNTC_DONCHITIET_4G
    {
        public string DIAPHUONG1 { get; set; }
        public decimal COUNT_TONG_TIEP_NHAN { get; set; }
        public decimal COUNT_TONG_DON_TRA_LOI { get; set; }
        public decimal TYLE1 { get; set; }
        public decimal COUNT_DON_KHIEU_NAI { get; set; }
        public decimal COUNT_DON_TO_CAO { get; set; }
        public decimal COUNT_DON_KN { get; set; }
        public decimal COUNT_ND_KHAC_NHAU { get; set; }
        public decimal COUNT_HANH_CHINH { get; set; }
        public decimal COUNT_TUPHAP { get; set; }
        public decimal COUNT_DANG { get; set; }
        public decimal COUNT_THAM_NHUNG { get; set; }
        public decimal COUNT_LINH_VUC_KHAC { get; set; }
        public decimal COUNT_DON_TRUNG { get; set; }
        public decimal COUNT_DONDUDK{ get; set; }
        public decimal COUNT_DONKODUDK{ get; set; }
        public decimal COUNT_CHUYEN_GIAI_QUYET { get; set; }
        public decimal COUNT_DON_DOC_CQTQ { get; set; }
        public decimal COUNT_TRA_LOI_HD { get; set; }
        public decimal COUNT_DANG_NGHIEN_CUU { get; set; }
        public decimal COUNT_LUU_THEO_DOI { get; set; }
        public decimal COUNT_NHAN_TRA_LOI { get; set; }
        public decimal COUNT_TONG_DON_LUU { get; set; }
        public decimal TYLE2 { get; set; }
        public decimal ISFIRST { get; set; }
        public string STENDONVI { get; set; }
    }
    
    public class RPT_KNTC_DONDATRALOI_4C
    {
        public string STENDONVI { get; set; }
        public decimal COUNTDONNHIEUNGUOIDUNGTENKYTRUOC { get; set; }
        public decimal COUNTDONMOTNGUOIDUNGTENKYTRUOC { get; set; }
        public decimal COUNTDONNHIEUNGUOIDUNGTENTRONGKY { get; set; }
        public decimal COUNTDONMOTNGUOIDUNGTENTRONGKY { get; set; }
        public decimal COUNTSODONDAXULYTRUOCKY { get; set; }
        public decimal COUNTSODONDAXULYTRONGKY { get; set; }
        public decimal COUNTCHEDOCHINHSACH { get; set; }
        public decimal COUNTDATDAINHACUA { get; set; }
        public decimal COUNTCONGCHUCCONGVU { get; set; }
        public decimal COUNTKHAC { get; set; }
        public decimal COUNTLINHVUCTUPHAP { get; set; }
        public decimal COUNTLINHVUCDANGDOANTHE { get; set; }
        public decimal COUNTTHAMNHUNG { get; set; }
        public decimal COUNTLINHVUCKHAC { get; set; }
    }

    public class RPT_KNTC_DONDATRALOI_4E
    {
        public string TT { get; set; }
        public string DIAPHUONG { get; set; }
        public decimal TONGSODON { get; set; }
        #region I. Phân loại đơn
        #region 1. Theo loại đơn
        // 1.1. Đơn khiếu nại
        public decimal ND_KN_SODON { get; set; }
        public string ND_KN_TYLE { get; set; }

        // 1.2. Đơn tố cáo
        public decimal ND_TC_SODON { get; set; }
        public string ND_TC_TYLE { get; set; }
        
        // 1.3. Đơn kiến nghị, phản ánh
        public decimal ND_PA_SODON { get; set; }
        public string ND_PA_TYLE { get; set; }
        
        // 1.4. Đơn có nhiều nội dung khác nhau
        public decimal ND_KHAC_SODON { get; set; }
        public string ND_KHAC_TYLE { get; set; }
        #endregion
        #region 2. Theo lĩnh vực
        // 2.1. Hành chính
        public decimal LV_HC_SODON { get; set; }
        public string LV_HC_TYLE { get; set; }

        // 2.2. Tư pháp
        public decimal LV_TP_SODON { get; set; }
        public string LV_TP_TYLE { get; set; }

        // 2.3. Đảng, đoàn thể
        public decimal LV_DT_SODON { get; set; }
        public string LV_DT_TYLE { get; set; }

        // 2.4. Tham nhũng
        public decimal LV_TN_SODON { get; set; }
        public string LV_TN_TYLE { get; set; }

        // 2.5. Lĩnh vực khác
        public decimal LV_KHAC_SODON { get; set; }
        public string LV_KHAC_TYLE { get; set; }
        #endregion
        #region 3. Theo điều kiện xử lý
        // 3.1. Đơn đủ điều kiện xử lý
        public decimal DK_D_SODON { get; set; }
        public string DK_D_TYLE { get; set; }

        #region 3.2. Đơn không đủ điều kiện xử lý
        // 3.2.1. Tổng số
        public decimal DK_KD_TONG_SODON { get; set; }
        public string DK_KD_TONG_TYLE { get; set; }

        // 3.2.2. Đơn trùng
        public decimal DK_KD_TRUNG_SODON { get; set; }
        public string DK_KD_TRUNG_TYLE { get; set; }
        #endregion
        #endregion
        #endregion
        #region II. Kết quả xử lý đơn đủ điền kiện
        // 1. Chuyển cơ quan có thẩm quyền giải quyết
        public decimal CHUYEN_SODON { get; set; }
        public string CHUYEN_TYLE { get; set; }

        // 2. Đôn đốc cơ quan có thẩm quyền
        public decimal DDOC_SODON { get; set; }
        public string DDOC_TYLE { get; set; }

        // 3. Trả lời, hướng dẫn công dân
        public decimal TLOI_SODON { get; set; }
        public string TLOI_TYLE { get; set; }

        // 4. Đang nghiên cứu
        public decimal NCUU_SODON { get; set; }
        public string NCUU_TYLE { get; set; }

        // 5. Lưu theo dõi
        public decimal TDOI_SODON { get; set; }
        public string TDOI_TYLE { get; set; }

        // 6. Số vụ việc nhận được trả lời
        public decimal REP_SODON { get; set; }
        public string REP_TYLE { get; set; }
        #endregion
        // III. Tổng đơn lưu
        public decimal TONGLUU_SODON { get; set; }
        public string TONGLUU_TYLE { get; set; }

        public bool ISTITLE { get; set; }
    }
}
