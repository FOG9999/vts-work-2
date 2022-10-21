using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Objects
{
    public class KIENNGHIPHULUC1
    {
        // đầu vào: kỳ họp
        public decimal STT { get; set; }
        public string TEN_DONVI { get; set; }
        public decimal ID_PARENT { get; set; }
        public decimal ID_DONVI { get; set; }
        public decimal TONGKIENNGHI { get; set; }
        public decimal TONGKIENNGHI_TRALOI { get; set; }
        public decimal TONGKIENNGHI_DAGIAIQUYET { get; set; }
        public decimal TONGKIENNGHI_DANGGIAIQUYET { get; set; }
        public decimal TONGKIENNGHI_GIAITRINH { get; set; }   
    }
    public class KIENNGHIPHULUC2
    {
        // đầu vào: kỳ họp
        public decimal ID_PARENT { get; set; }
        public decimal VITRI { get; set; }
        public string SOHIEUVANBAN { get; set; }
        public DateTime NGAY { get; set; }
        public string TRICHYEU { get; set; }
        
    }
    public class KIENNGHIPHULUC3
    {
        // đầu vào: kỳ họp
        public decimal STT { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public string LOTRINH { get; set; }
        public decimal TINHTRANG_TRALOI { get; set; }
        public decimal ID_DONVI { get; set; }
        public string TEN_DONVI { get; set; }
        public string  TENPHANLOAI { get; set; }
        public decimal ID_PHANLOAI { get; set; }
        public DateTime NGAYDUKIEN { get; set; }
    }
    public class KIENNGHIPHULUC4
    {
        // đầu vào: ko có
        public decimal STT { get; set; }
        public decimal ID_CAPCOQUAN { get; set;}
        public string TEN_DONVI { get; set; }
        public string TEN_CAPCOQUAN { get; set; }
        public decimal TONGKIENNGHI { get; set; }
        public decimal TONGKIENNGHI_DAGIAIQUYET { get; set; }
        public decimal TONGKIENNGHI_DANGGIAIQUYET { get; set; }
        
    }
    public class KIENNGHIPHULUC5
    {// đầu vào: kỳ họp
        public decimal ID_GOP { get; set; }
        public decimal ID_THAMQUYENCOQUAN { get; set; }
        public decimal ID_KIENNGHI { get; set; }
        public string TEN_THAMQUYENCOQUAN { get; set; }
        public string TEN_KYHOP { get; set; }
        public string TEN_KHOAHOP { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public decimal ID_PHANLOAI { get; set; }
        public string TEN_PHANLOAI { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public decimal ID_KIENNGHI_PARENT_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
    }
    public class KIENNGHIPHULUC5B
    {// đầu vào: không có
        public decimal ID_LINHVUC { get; set; }
        public string TEN_LINHVUC { get; set; }
        public decimal ID_GOP { get; set; }
        public decimal ID_THAMQUYENCOQUAN { get; set; }
        public decimal ID_KIENNGHI { get; set; }
        public string TEN_THAMQUYENCOQUAN { get; set; }
        public string TEN_KYHOP { get; set; }
        public string TEN_KHOAHOP { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public decimal ID_PHANLOAI_TRALOI { get; set; }
        public string TEN_PHANLOAI_TRALOI { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public decimal ID_KIENNGHI_PARENT_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
        public DateTime NGAY_DUKIENGIAIQUYET { get; set; }
    }
    public class KIENNGHIPHULUC6
    {// đầu vào: năm (int)
        public string KEHOACH { get; set; }
        public decimal ID_DONVI { get; set; }
        public string TEN_DONVI { get; set; }
        public string NOIDUNG { get; set; }
    }
    public class KIENNGHIPHULUC7
    {// đầu vào: kỳ họp
        
        public string TEN_TINHTHANH { get; set; }
        public DateTime NGAYNHAN { get; set; }
        public decimal ID_DONVI { get; set; }
    }

    public class KIENNGHITRALOIKNTC
    {// đầu vào: kỳ họp

        public string CNOIDUNG { get; set; }
        public decimal ITHAMQUYENDONVI { get; set; }
        public string CSOVANBAN { get; set; }
        public DateTime DNGAYBANHANH { get; set; }
        public string CTRALOI { get; set; }
        public string CUTRI { get; set; }
        public string COQUANTHAMQUYEN { get; set; }
        public decimal IKYHOP { get; set; }
    }

    public class KIENNGHITRALOIKNDBQH
    {// đầu vào: kỳ họp

        public string CNOIDUNG { get; set; }
        public decimal IKYHOP { get; set; }
        public decimal ITHAMQUYENDONVI { get; set; }
        public string CSOVANBAN { get; set; }
        public DateTime DNGAYBANHANH { get; set; }
        public string GHICHU_KQ { get; set; }
        public string CCOQUANTRALOI { get; set; }
        public string CTEN { get; set; }
        public decimal IKHOA { get; set; }
    }

    public class KIENNGHIPHANLOAI
    {// đầu vào: không có
        public decimal ID_TRALOI_PHANLOAI { get; set; }
        public string TEN_TRALOI_PHANLOAI { get; set; }
        public decimal ID_PARENT_TRALOI_PHANLOAI { get; set; }
        public decimal ID_KIENNGHI { get; set; }
        public string NOIDUNG_KIENNGHI { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public decimal ID_KIENNGHI_PARENT_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }
        public string TEN_PHANLOAI_PARENT { get; set; }
        public decimal ID_GOP { get; set; }


    }
    public class KIENNGHITRALOI {
        // đầu vào: kỳ họp
        public decimal ID_THAMQUYENDONVI { get; set; }
        public string TEN_DONVITIEPNHAN { get; set; }
        public string TEN_THAMQUYENDONVI { get; set; }
        public decimal ID_KIENNGHI { get; set; }
        public decimal ID_GOP { get; set;}
        public string NOIDUNG_KIENNGHI { get; set; }
        public decimal ID_THAMQUYENDONVI_PARENT { get; set; }
        public string TEN_THAMQUYENDONVI_PARENT { get; set; }
        public string TRALOI_SOVANBAN { get; set; }
        public DateTime TRALOI_NGAYBANHANH_VANBAN { get; set; }
        public string TRALOI_NOIDUNG { get; set; }
        public decimal ID_KIENNGHI_CHILD_GOP { get; set; }
        public string TENDONVITIEPNHAN_GOP { get; set; }        

    }
    public class KIENNGHI_TRACUU_PHANTRANG
    {
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
    }

    /*  HaiPN16
     *  class phục vụ báo cáo kết quả
     */
    public class KIENNGHI_KETQUA
    {
        public string TEN_DIA_PHUONG { get; set; }
        public string CNOIDUNG { get; set; }
        public string TEN_LINH_VUC { get; set; }

    }

    public class KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG
    {
        public string CSTT { get; set; }
        public decimal ILINHVUC { get; set; }
        public string CTENLINHVUC { get; set; }
        public decimal ITONGSOKIENNGHI { get; set; }
        public decimal DTILE { get; set; }
        public List<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_HEADER> LIST_HEADER { get; set; }
        public List<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA> LIST_DATA { get; set; }
    } 

    public class KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_HEADER
    {
        public decimal IDIAPHUONG { get; set; }
        public string CTENDIAPHUONG { get; set; }
        public string SOKIENNGHI_NAME { get; set; } = "Số KNCT";
        public string TILE_NAME { get; set; } = "Tỉ lệ %";
    }

    public class KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA
    {
        public decimal ISOKIENNGHI { get; set; }
        public decimal DTILE { get; set; }
    }

    public class KIENNGHI_REPORT_TONGHOPHUYEN
    {
        public decimal DEPTH { get; set; }
        public string TENNGUONKIENNGHI { get; set; }
        public decimal INGUONKIENNGHI { get; set; }
        public string CMAKIENNGHI { get; set; }
        public string CNOIDUNG { get; set; }
        public string ICOCHE { get; set; }
        public string IKINHTE { get; set; }
        public string IVANHOA { get; set; }
        public string ITUPHAP { get; set; }
        public string ANQP { get; set; }
        public string CTENLINHVUC { get; set; }
        public string CGHICHU { get; set; }
        public string FIRSTTITLE { get; set; }
        public decimal IDOITUONGGUI { get; set; }
    }
    public class KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG
    {
        public string CTENDIAPHUONG { get; set; }
        public decimal INGUONKIENNGHI { get; set; }
        public string CMAKIENNGHI { get; set; }
        public string CNOIDUNG { get; set; }
        public decimal ILINHVUC { get; set; }
        public decimal IDOITUONGGUI { get; set; }
        public string CTENLINHVUC { get; set; }
        public string LVCOCHE_CHINHSACH { get; set; }
        public string LVTUPHAP { get; set; }
        public string LVANQP { get; set; }
        public string LVVHXH { get; set; }
        public string LVKINHTE_NGANSACH { get; set; }
        public string CGHICHU { get; set; }
        public string DEPTH { get; set; }
        public string FIRSTTITLE { get; set; }
        public string CCOQUANTRALOI { get; set; }
        public string CTRALOI { get; set; }
        public string CTENDIAPHUONGCUTRI { get; set; }
        public string CSOVANBAN { get; set; }
        
    }

    public class KIENNGHI_REPORT_TONGHOP_TINH
    {
        public decimal IKIENNGHI { get; set; }
        public decimal STT { get; set; }
        public decimal IDOITUONGGUI { get; set; }
        public string CTENDIAPHUONG { get; set; }
        public string CSOKIENNGHI { get; set; }
        public decimal ILINHVUC { get; set; }
        public string CNOIDUNG { get; set; }
        public string CCOCHECHINHSACH { get; set; }
        public string CKINHTENGANSACH { get; set; }
        public string CVANHOAXAHOI { get; set; }
        public string CTUPHAP { get; set; }
        public string CANQP { get; set; }
        public string CGHICHU { get; set; }
        public string ROWTITLE { get; set; }
    }
}
