using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Objects
{
    public class PRC_LIST_USER_CAPNHAT
    {
        public string USER_CAPNHAT { get; set; }
        public Nullable<decimal> USER_ID { get; set; }
    }
    public class PRC_KIENNGHI_DELETE
    {
        public string NOIDUNG_KIENNGHI { get; set; }
        public string TEN_DONVI_THAMQUYEN { get; set; }
        public string TEN_DONVI_TIEPNHAN { get; set; }
        public Nullable<decimal> ID_KIENNGHI { get; set; }
    }
    public class PRC_KIENNGHI_IMPORT{
        public decimal ID { get; set; }
        public Nullable<decimal> IDONVITIEPNHAN { get; set; }
        public Nullable<System.DateTime> DNGAYBANHANH { get; set; }
        public string CNOIDUNG { get; set; }
        public string CCONGVAN { get; set; }
        public string CSOCONGVAN { get; set; }
        public Nullable<decimal> IDONVITHAMQUYEN { get; set; }
        public Nullable<decimal> ILINHVUC { get; set; }
        public Nullable<decimal> ID_KIENNGHI { get; set; }
        public Nullable<decimal> ID_TONGHOP_BDN { get; set; }
        public Nullable<decimal> ID_IMPORT { get; set; }
        public Nullable<decimal> IKYHOP { get; set; }
        public Nullable<decimal> ITRUOCKYHOP { get; set; }
        public Nullable<decimal> ICHUONGTRINH { get; set; }
        public string TENDONVI_TIEPNHAN { get; set; }
        public string TENDONVI_THAMQUYEN { get; set; }
        public string TEN_KYHOP { get; set; }
        public string TEN_KHOAHOP { get; set; }
    }
    public class PRC_TAPHOP_IMPORT
    {
        public string NOIDUNG_KIENNGHI { get; set; }
        public string TENDONVI_THAMQUYEN { get; set; }
        public string NOIDUNG_TONGHOP { get; set; }
        public Nullable<decimal> ID_TONGHOP { get; set; }
    }

    public class PRC_CHUONGTRINH_TXCT_EXPORT {
        public string MA_COQUAN { get; set; }
        public string TEN_COQUAN { get; set; }
        public string KEHOACH_SO { get; set; }
        public string NOIDUNG { get; set; }
        public string KYHOP { get; set; }
        public decimal TRUOCKYHOP { get; set; }
        public decimal ID_CHUONGTRINH { get; set; }
        public DateTime NGAYBATDAU { get; set; }
        public DateTime NGAYKETTHUC { get; set; }

    }
    public class PRC_COQUAN_LINHVUC
    {
        public decimal ID_COQUAN { get; set; }
        public decimal ID_PARENT_COQUAN { get; set; }
        public decimal ID_LINHVUC { get; set; }
        public string TENCOQUAN { get; set; }
        public string TEN_PARENT_COQUAN { get; set; }
        public string MA_COQUAN { get; set; }
        public string TENLINHVUC { get; set; }
        public string MA_LINHVUC { get; set; }
    }
    public class PRC_DOANGIAMSAT
    {
        public decimal TOTAL { get; set; }
        public decimal IDOAN { get; set; }
        public decimal ID_USER_CAPNHAT { get; set; }
        public decimal ID_DONVI { get; set; }
        public decimal SOKIENNGHI { get; set; }
        public string TEN_KEHOACH { get; set; }
        public string NOIDUNG_GIAMSAT { get; set; }
        public DateTime NGAYBATDAU { get; set; }
        public string TEN_DONVI { get; set; }
    }
    public class PRC_CHUONGTRINH_CHITIET
    {
        public decimal ID { get; set; }
        public string TENDAIBIEU { get; set; }
        public decimal ID_TODAIBIEU { get; set; }
        public decimal ID_DIAPHUONG { get; set; }
        public decimal ID_DIAPHUONG2 { get; set; }
        public string DIACHITIEP { get; set; }
        public DateTime NGAYTIEP { get; set; }
        public string TENDIAPHUONG { get; set; }
        public string TENDIAPHUONG2 { get; set; }
    }
    public class PRC_LIST_KN_TRALOI_DANHGIA
    {
        public decimal TOTAL { get; set; }
        public decimal ID_THAMQUYENDONVI { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public string TEN_THAMQUYENDONVI { get; set; }
        public decimal ID_KIENNGHI { get; set; }
        public decimal ID_DONVITIEPNHAN { get; set; }
        public decimal ID_GOP { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public decimal ID_THAMQUYENDONVI_PARENT { get; set; }
        public string TEN_THAMQUYENDONVI_PARENT { get; set; }
        public string TRALOI_SOVANBAN { get; set; }
        public string TRALOI_NOIDUNG { get; set; }
        public DateTime TRALOI_NGAYBANHANH_VANBAN { get; set; }
        public decimal TRALOI_TINHTRANG { get; set; }
        public string TRALOI_PHANLOAI { get; set; }
        public decimal ID_PARENT_TRALOI_PHANLOAI { get; set; }
        public string GIAMSAT_PHANLOAI { get; set; }
        public decimal ID_PARENT_GIAMSAT_PHANLOAI { get; set; }
        public decimal GIAMSAT_DONGKIENNGHI { get; set; }
        public decimal GIAMSAT_DUNGTIENDO { get; set; }
        public decimal ID_KIENNGHI_CHILD_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
        public string PARENT_TRALOI_PHANLOAI { get; set; }
        public string PARENT_GIAMSAT_PHANLOAI { get; set; }
        public decimal ID_LINHVUC_KIENNGHI { get; set; }
        public string TEN_LINHVUC_KIENNGHI { get; set; }
        public string COQUANTRALOI { get; set; }
    }
    public class PRC_KIENNGHI_TRUNG
    {
        public decimal ID_KIENNGHI { get; set; }
        public decimal ID_KYHOP_TIEPNHAN { get; set; }
        public decimal ID_KHOAHOP { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public string TEN_KYHOP_TIEPNHAN { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public decimal ID_DONVITIEPNHAN { get; set; }
        public string TEN_DONVITHAMQUYEN { get; set; }
        public decimal ID_THAMQUYENDONVI { get; set; }
        public string TEN_KHOAHOP { get; set; }
    }
    public class TruocKyHop
    {
        public string ten { get; set; }
        public int value { get; set; }
        public string class_span { get; set; }
    }
    public class PRC_TONGHOP_KIENNGHI
    {
        public decimal TOTAL { get; set; }
        public decimal ITONGHOP { get; set; }
        public decimal ITINHTRANG { get; set; }
        public decimal IUSER_CAPNHAT { get; set; }
        public string TEN_DONVITONGHOP { get; set; }
        public decimal ID_DONVITONGHOP { get; set; }
        public decimal ID_THAMQUYEN_DONVI_TONGHOP { get; set; }
        public string TEN_THAMQUYEN_DONVI_TONGHOP { get; set; }
        public string TEN_LINHVUC_TONGHOP { get; set; }
        public decimal ID_PARENT_THAMQUYEN_DONVI { get; set; }
        public string NOIDUNG_TONGHOP { get; set; }
        public decimal SOKIENNGHI_TONGHOP { get; set; }
        public decimal IKIENNGHI { get; set; }
        public decimal ID_GOP { get; set; }
        public decimal ID_LINHVUC_TONGHOP { get; set; }
        public decimal ID_KIENNGHI_TONGHOP { get; set; }
        public decimal ID_KIENNGHI_TONGHOP_BDN { get; set; }
        public decimal ID_DONVITIEPNHAN { get; set; }
        public string TEN_DONVITIEPNHAN_KIENNGHI { get; set; }
        public decimal ID_THAMQUYEN_DONVI_KIENNGHI { get; set; }
        public string TEN_THAMQUYEN_DONVI_KIENNGHI { get; set; }
        public string TEN_LINHVUC_KIENNGHI { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public decimal IKIENNGHI_GOP { get; set; }
        public decimal ID_KIENNGHI_PARENT_GOP { get; set; }
        public decimal ID_LINHVUC_KIENNGHI { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
        public string TEN_PARENT_THAMQUYEN_DONVI { get; set; }
        public decimal ICOQUAN_PARENT_THAMQUYEN_DONVI { get; set; }
        public DateTime NGAY_TONGHOP { get; set; }
        public DateTime NGAY_CHUYENTONGHOP { get; set; }
        public string THAMQUYEN_CHUYEN { get; set; }
    }
    public class PRC_LIST_TONGHOP_KIENNGHI
    {
        public decimal TOTAL { get; set; }
        public decimal ITONGHOP { get; set; }
        public decimal ITINHTRANG { get; set; }
        public decimal IUSER_CAPNHAT { get; set; }
        public string TEN_DONVITONGHOP { get; set; }
        public decimal ID_DONVITONGHOP { get; set; }
        public decimal ID_THAMQUYEN_DONVI_TONGHOP { get; set; }
        public string TEN_THAMQUYEN_DONVI_TONGHOP { get; set; }
        public string TEN_LINHVUC_TONGHOP { get; set; }
        public decimal ID_PARENT_THAMQUYEN_DONVI { get; set; }
        public string NOIDUNG_TONGHOP { get; set; }
        public decimal SOKIENNGHI_TONGHOP { get; set; }
        public decimal SOKIENNGHI_DATRALOI { get; set; }
        public DateTime NGAYDUKIENHOANTHANH { get; set; }
        public string TEN_PARENT_THAMQUYEN_DONVI { get; set; }
        public decimal ICOQUAN_PARENT { get; set; }
        public DateTime NGAYBANHANH_VANBAN { get; set; }
        public decimal ID_LINHVUC_TONGHOP { get; set; }
        public decimal SOKIENNGHI_CHUATRALOI { get; set; }
        public decimal IKIENNGHI { get; set; }
        public string TEN_DONVI_THAMQUYENKIENNGHI { get; set; }
        public decimal ID_GOP { get; set; }
        public decimal ID_LINHVUC_KIENNGHI { get; set; }
        public string TEN_LINHVUC_KIENNGHI { get; set; }
        public string TEN_DONVITIEPNHAN_KIENNGHI { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public string TRALOI_NOIDUNG { get; set; }
        public string TRALOI_SOVANBAN { get; set; }
        public DateTime TRALOI_NGAYBANHANH_VANBAN { get; set; }
        public string TRALOI_PHANLOAI { get; set; }
        public decimal ID_PARENT_TRALOI_PHANLOAI { get; set; }
        public string PARENT_TRALOI_PHANLOAI { get; set; }
        public string GIAMSAT_PHANLOAI { get; set; }
        public decimal ID_PARENT_GIAMSAT_PHANLOAI { get; set; }
        public string PARENT_GIAMSAT_PHANLOAI { get; set; }
        public decimal GIAMSAT_DONGKIENNGHI { get; set; }
        public decimal GIAMSAT_DUNGTIENDO { get; set; }
        public decimal ID_TRALOI { get; set; }
        public decimal ID_GIAMSAT { get; set; }
        public decimal ID_TRALOI_USER { get; set; }
        public decimal ID_GIAMSAT_USER { get; set; }
        public string COQUANTRALOI { get; set; }
        public DateTime INGAYQUYDINH { get; set; }
        public decimal ICANHBAO { get; set; }
        public decimal IKYHOP { get; set; }
    }
    public class PRC_KIENNGHI_MOICAPNHAT
    {
        //PRC_KIENNGHI_MOICAPNHAT
        /*
         p_IUSER in NUMBER, /* DEFAULT: 0
        p_ILINHVUC IN NUMBER, /* DEFAULT: -1
        p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0
        p_IKYHOP in NUMBER, /* DEFAULT: 0
        p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1
        p_ITHAMQUYENDONVI_PARENT in number :0
            p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0

        p_CNOIDUNG IN NVARCHAR2 /* DEFAULT: 
             */
        public decimal TOTAL { get; set; }
        public decimal ID_GOP { get; set; }
        public decimal ID_DONVI_CAPNHAT { get; set; }
        public decimal SOLUONG_KIENNGHI_GOP { get; set; }
        public decimal ID_KIENNGHI_TRUNG { get; set; }
        public decimal ID_USER_CAPNHAT { get; set; }
        public decimal ID_KIENNGHI { get; set; }
        public decimal ID_KIENNGHI_TONGHOP { get; set; }
        public decimal ID_KIENNGHI_TONGHOP_BDN { get; set; }
        public decimal ID_DONVITIEPNHAN { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public decimal ID_THAMQUYEN_DONVI { get; set; }
        public string TEN_THAMQUYEN_DONVI { get; set; }
        public decimal ID_LINHVUC { get; set; }
        public string TEN_LINHVUC { get; set; }
        public decimal ITINHTRANG { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public string ID_ENCR { get; set; }
        public decimal ID_PARENT_THAMQUYEN_DONVI { get; set; }
        public string TEN_PARENT_THAMQUYEN_DONVI { get; set; }
        public decimal ID_KIENNGHI_PARENT_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
    }
    public class PRC_KIENNGHI_LIST
    {
        //PRC_KIENNGHI_MOICAPNHAT
        /*
         p_ITINHTRANG IN NUMBER, /* DEFAULT: -1
        p_ILINHVUC IN NUMBER, /* DEFAULT: -1
        p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0
        p_IKYHOP in NUMBER, /* DEFAULT: 0
        p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1
        p_ITHAMQUYENDONVI_PARENT in number :0
        p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0
        p_CNOIDUNG IN NVARCHAR2 /* DEFAULT: 
             */
        public decimal TOTAL { get; set; }
        public decimal ID_GOP { get; set; }
        public decimal ID_DONVI_CAPNHAT { get; set; }
        public decimal SOLUONG_KIENNGHI_GOP { get; set; }
        public decimal ID_KIENNGHI_TRUNG { get; set; }
        public decimal ID_USER_CAPNHAT { get; set; }
        public decimal ID_KIENNGHI { get; set; }
        public decimal ID_DONVITIEPNHAN { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public decimal ID_THAMQUYEN_DONVI { get; set; }
        public string TEN_THAMQUYEN_DONVI { get; set; }
        public decimal ID_LINHVUC { get; set; }
        public string TEN_LINHVUC { get; set; }
        public decimal ITINHTRANG { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public string ID_ENCR { get; set; }
        public decimal ID_PARENT_THAMQUYEN_DONVI { get; set; }
        public string TEN_PARENT_THAMQUYEN_DONVI { get; set; }
        public decimal ID_KIENNGHI_PARENT_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
    }
    public class PRC_KIENNGHI_LIST_TRACUU
    {

        public decimal TOTAL { get; set; }
        public decimal DATRALOI { get; set; }
        public decimal GIAMSAT_DONGKIENNGHI { get; set; }
        public DateTime NGAYCAPNHAT { get; set; }
        public DateTime NGAYTONGHOP { get; set; }
        public DateTime TRALOI_NGAYBANHANH { get; set; }
        public string TRALOI_SOVANBAN { get; set; }
        public DateTime NGAY_CHUYEN { get; set; }
        public decimal ID_GOP { get; set; }
        ////public decimal ID_DONVI_CAPNHAT { get; set; }
        ////public decimal SOLUONG_KIENNGHI_GOP { get; set; }
        public decimal ID_KIENNGHI_TONGHOP { get; set; }
        public decimal ID_KIENNGHI_TONGHOP_BDN { get; set; }
        public decimal ID_KIENNGHI_TRUNG { get; set; }
        public decimal ID_USER_CAPNHAT { get; set; }
        public decimal ID_KIENNGHI { get; set; }
        public decimal ID_DONVITIEPNHAN { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public decimal ID_THAMQUYEN_DONVI { get; set; }
        public string TEN_THAMQUYEN_DONVI { get; set; }
        public decimal ID_LINHVUC { get; set; }
        public string TEN_LINHVUC { get; set; }
        public decimal ITINHTRANG { get; set; }
        public decimal IDELETE { get; set; }

        public decimal IUSER { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        //public string ID_ENCR { get; set; }
        //public decimal ID_PARENT_THAMQUYEN_DONVI { get; set; }
        public string TEN_PARENT_THAMQUYEN_DONVI { get; set; }
        //public decimal ID_KIENNGHI_PARENT_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
    }
    public class PRC_KIENNGHI_CHUYENKYSAU
    {
        //PRC_KIENNGHI_MOICAPNHAT
        /*
         p_ITINHTRANG IN NUMBER, /* DEFAULT: -1
        p_ILINHVUC IN NUMBER, /* DEFAULT: -1
        p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0
        p_IKYHOP in NUMBER, /* DEFAULT: 0
        p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1
        p_ITHAMQUYENDONVI_PARENT in number :0
        p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0
        p_CNOIDUNG IN NVARCHAR2 /* DEFAULT: 
             */

        public decimal ID_GOP { get; set; }
        public decimal ID_DONVI_CAPNHAT { get; set; }
        public decimal SOLUONG_KIENNGHI_GOP { get; set; }
        public decimal ID_KIENNGHI_TRUNG { get; set; }
        public decimal ID_USER_CAPNHAT { get; set; }
        public decimal ID_KIENNGHI { get; set; }
        public decimal ID_DONVITIEPNHAN { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public decimal ID_THAMQUYEN_DONVI { get; set; }
        public string TEN_THAMQUYEN_DONVI { get; set; }
        public decimal ID_LINHVUC { get; set; }
        public string TEN_LINHVUC { get; set; }
        public decimal ITINHTRANG { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public string ID_ENCR { get; set; }
        public decimal ID_PARENT_THAMQUYEN_DONVI { get; set; }
        public string TEN_PARENT_THAMQUYEN_DONVI { get; set; }
        public decimal ID_KIENNGHI_PARENT_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
        public string NOIDUNG_TRALOI { get; set; }
        public string SOVANBAN_TRALOI { get; set; }
        public DateTime NGAYBANHANH_TRALOI { get; set; }
        public string PHANLOAI_TRALOI { get; set; }
        public string PHANLOAI_DANHGIA { get; set; }
    }
    public class PRC_CHUONGTRINH_TXCT
    {
        //PRC_CHUONGTRINH_TIEPXUC_CUTRI
        /* đầu vào: kỳ họp
        p_IKYHOP in NUMBER, /* DEFAULT: 0
        p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1
        p_IDOAN_LAPKEHOACH in NUMBER,/* DEFAULT: 0  
        p_CNOIDUNG IN NVARCHAR2 /* DEFAULT: */
        public decimal ICHUONGTRINH { get; set; }
        public string CKEHOACH { get; set; }
        public string CDIACHI { get; set; }
        public string CNOIDUNG { get; set; }
        public Nullable<DateTime> DBATDAU { get; set; }
        public Nullable<DateTime> DKETTHUC { get; set; }
        public string TENDIAPHUONG { get; set; }
        public string TENDIAPHUONG1 { get; set; }
        public string TENDAIBIEU { get; set; }
        public decimal ID_DIAPHUONGTIEPXUC { get; set; }
        public decimal ID_USER_CAPNHAT { get; set; }
        public decimal ID_DIAPHUONG1 { get; set; }
        public decimal ID_DAIBIEU { get; set; }
        public decimal TOTAL { get; set; }
    }
    public class PRC_KIENNGHI_BYTONGHOP
    {
        /*
         PRC_LIST_KIENNGHI_BY_TONGHOP_KIENNGHI
         p_ITONGHOP in NUMBER,p_ITONGHOP_BDN in NUMBER
             */
        public decimal ID_GOP { get; set; }
        public decimal IKIENNGHI { get; set; }
        public string CNOIDUNG { get; set; }
        public string CDIACHI { get; set; }
        public string CNGUONDON { get; set; }
        public decimal ITINHTRANG { get; set; }
        public string TENDONVITIEPNHAN { get; set; }
        public string TEN_TINH { get; set; }
        public string TEN_HUYEN { get; set; }
        public decimal IKIENNGHI_GOP { get; set; }
        public decimal ID_KIENNGHI_CHILD_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }

    }

    public class PRC_KIENNGHI_TAMXOA
    {
        public decimal TOTAL { get; set; }
        public decimal ID_GOP { get; set; }
        public decimal ID_DONVI_CAPNHAT { get; set; }
        public decimal SOLUONG_KIENNGHI_GOP { get; set; }
        public decimal ID_KIENNGHI_TRUNG { get; set; }
        public decimal ID_USER_CAPNHAT { get; set; }
        public decimal ID_KIENNGHI { get; set; }
        public decimal ID_KIENNGHI_TONGHOP { get; set; }
        public decimal ID_KIENNGHI_TONGHOP_BDN { get; set; }
        public decimal ID_DONVITIEPNHAN { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public decimal ID_THAMQUYEN_DONVI { get; set; }
        public string TEN_THAMQUYEN_DONVI { get; set; }
        public decimal ID_LINHVUC { get; set; }
        public string TEN_LINHVUC { get; set; }
        public decimal ITINHTRANG { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public string ID_ENCR { get; set; }
        public decimal ID_PARENT_THAMQUYEN_DONVI { get; set; }
        public string TEN_PARENT_THAMQUYEN_DONVI { get; set; }
        public decimal ID_KIENNGHI_PARENT_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
    }

    public class PRC_KIENNGHI_IMPORT_LISTKN
    {
        public decimal ID { get; set; }
        public Nullable<decimal> IDONVITIEPNHAN { get; set; }
        public Nullable<System.DateTime> DNGAYBANHANH { get; set; }
        public string CNOIDUNG { get; set; }
        public string CCONGVAN { get; set; }
        public string CSOCONGVAN { get; set; }
        public Nullable<decimal> IDONVITHAMQUYEN { get; set; }
        public Nullable<decimal> ILINHVUC { get; set; }
        public Nullable<decimal> IKIENNGHI { get; set; }
        public Nullable<decimal> ID_TONGHOP_BDN { get; set; }
        public Nullable<decimal> ID_IMPORT { get; set; }
        public Nullable<decimal> IKYHOP { get; set; }
        public Nullable<decimal> ITRUOCKYHOP { get; set; }
        public Nullable<decimal> ICHUONGTRINH { get; set; }
        public string TENDONVI_TIEPNHAN { get; set; }
        public string TENDONVI_THAMQUYEN { get; set; }
        public string TEN_KYHOP { get; set; }
        public string TEN_KHOAHOP { get; set; }
        public string CMAKIENNGHI { get; set; }
        public string TEN_LINHVUC { get; set; }
    }
}
