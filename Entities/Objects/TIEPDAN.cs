using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Objects
{

    public class TIEPDAN_PHULUC
    {
        public decimal THOIGIAN { get; set; }
        public decimal TONGSO { get; set; }
        public decimal TIEPCONGDANTHEOLINHVUC { get; set; }
        public decimal TIEPCONGDANCUACANHANDBQH { get; set; }
        public decimal SOVUVIEC { get; set; }
        public decimal KHIEUNAI { get; set; }
        public decimal TOCAO { get; set; }
        public decimal KHIENNGHIPHANANH { get; set; }
        public decimal HANHCHINH { get; set; }
        public decimal TUPHAP { get; set; }
        public decimal DOANDONGNGUOI { get; set; }
        public decimal HUONGDANBANGVANBAN { get; set; }
        public decimal HUONGDANGIAITHICHTRUCTIEP { get; set; }
        public decimal CHUYENDENCOQUANCOTHAMQUYEN { get; set; }
    }
    public class TIEPDAN_SOLIEUTINH
    {
        public string DIAPHUONG { get; set; }
        public decimal SOBUOITCD { get; set; }
        public decimal LUOTNGUOI { get; set; }
        public decimal SOVUVIEC { get; set; }
        public decimal DOANDONGNGUOI { get; set; }
        public decimal TONGDONNHAN { get; set; }
        public decimal KHIEUNAI { get; set; }
        public decimal TOCAO { get; set; }
        public decimal KIENNGHIPHANANH { get; set; }
        public decimal DONTRUNG { get; set; }
        public decimal DATDAI { get; set; }
        public decimal CHINHSACHXH { get; set; }
        public decimal VIPHAMPLTHAMNHUNG { get; set; }
        public decimal QUANLIKINHTEXH { get; set; }
        public decimal KHAC { get; set; }
        public decimal TUPHAP { get; set; }
        public decimal HANHCHINH { get; set; }
        public decimal LINHVUCKHAC { get; set; }

        public decimal DANGNGHIENCUU { get; set; }
        public decimal SODONLUUTHEODOI { get; set; }
        public decimal SOVUVIECDACHUYEN { get; set; }
        public decimal SOVUVUDATRALOI { get; set; }
        public decimal HUONGDANTRALOI { get; set; }
        public decimal TYLE { get; set; }
        public decimal DONDOCVUVIECCUTHE { get; set; }
        public decimal CHUYENDE { get; set; }
        public decimal LONGGHEP { get; set; }
        public decimal VUVIECCUTHE { get; set; }
        public decimal IDDIAPHUONG { get; set; }



    }
    public class TIEPCONGDAN
    {
        public DateTime NGAYNHAN { get; set; }
        public decimal DIAPHUONG_0 { get; set; }
        public decimal DIAPHUONG_1 { get; set; }
        public string DIACHI { get; set; }
        public string NOIDUNGVUVIEC { get; set; }
        public string TENNGUOITIEP { get; set; }
        public string TENNGUOIGUI { get; set; }
        public decimal TRANGTHAIXULY { get; set; }

        public decimal IDVUVIEC { get; set; }
        public decimal DONVI { get; set; }
        public decimal DINHKY { get; set; }
        public decimal VUVIECTRUNG { get; set; }
        public decimal GIAMSAT { get; set; }
        public string YKIENGIAMSAT { get; set; }

    }




    public class TIEPCONGDAN_TRACUU
    {
        public DateTime NGAYNHAN { get; set; }
        public string TENTINH { get; set; }
        public string TENHUYEN { get; set; }
        public string DIACHINGUOIGUI { get; set; }
        public string NOIDUNGVUVIEC { get; set; }
        public string TENNGUOITIEP { get; set; }
        public string TENNGUOIGUI { get; set; }
        public string TINHCHAT { get; set; }
        public string LOAIDON { get; set; }
        public string NHOMNOIDUNG { get; set; }
        public decimal TRANGTHAIXULY { get; set; }
        public decimal IDVUVIEC { get; set; }
        public decimal IDONVI { get; set; }
        public string CTENDONVI { get; set; }
        public decimal DINHKY { get; set; }
        public decimal VUVIECTRUNG { get; set; }
        public decimal GIAMSAT { get; set; }
        public string YKIENGIAMSAT { get; set; }

    }
    public class TIEPCONGDAN_DANHMUC
    {
        public DateTime NGAYNHAN { get; set; }
        public string TENTINH { get; set; }
        public string TENHUYEN { get; set; }
        public string DIACHINGUOIGUI { get; set; }
        public string NOIDUNGVUVIEC { get; set; }
        public string TENNGUOITIEP { get; set; }
        public string TENNGUOIGUI { get; set; }
        public string TINHCHAT { get; set; }
        public string LOAIDON { get; set; }
        public string NHOMNOIDUNG { get; set; }
        public string TENNHOMLINHVUC { get; set; }
        public decimal TRANGTHAIXULY { get; set; }
        public decimal IDVUVIEC { get; set; }
        public decimal IDONVI { get; set; }
        public decimal USER_ID { get; set; }
        public string CTENDONVI { get; set; }
        public decimal DINHKY { get; set; }
        public decimal VUVIECTRUNG { get; set; }
        public decimal GIAMSAT { get; set; }
        public string YKIENGIAMSAT { get; set; }
        public decimal TIEPDOTXUAT { get; set; }
        public decimal LANHDAOTIEP { get; set; }
        public string NOIDUNGCHIDAO { get; set; }
        public decimal TOTAL { get; set; }
    

    }
    public class TIEPCONGDAN_TRACUU_VUVIEC
    {
        public DateTime NGAYNHAN { get; set; }
        public string TENTINH { get; set; }
        public string TENHUYEN { get; set; }
        public string DIACHINGUOIGUI { get; set; }
        public string NOIDUNGVUVIEC { get; set; }
        public string TENNGUOITIEP { get; set; }
        public string TENNGUOIGUI { get; set; }
        public string TINHCHAT { get; set; }
        public string LOAIDON { get; set; }
        public string NHOMNOIDUNG { get; set; }
        public string TENLINHVUC { get; set; }
        public decimal TRANGTHAIXULY { get; set; }
        public decimal IDVUVIEC { get; set; }
        public decimal IDONVI { get; set; }
        public decimal USER_ID { get; set; }
        public string CTENDONVI { get; set; }
        public decimal DINHKY { get; set; }
        public decimal VUVIECTRUNG { get; set; }
        public decimal GIAMSAT { get; set; }
        public string YKIENGIAMSAT { get; set; }
        public decimal TONGSOVUVIEC { get; set; }
        public decimal TIEPDOTXUAT { get; set; }
        public decimal LANHDAOTIEP { get; set; }
        public string NOIDUNGCHIDAO { get; set; }
        public decimal TOTAL { get; set; }
        public decimal DELVUVIEC { get; set; }

    }
    public class TIEPCONGDAN_LICHTIEPDINHKY
    {
        public decimal IDDINHKY { get; set; }
        public decimal IDDONVI { get; set; }
        public DateTime NGAYTIEP { get; set; }
        public decimal IDUSER { get; set; }
        public string DIADIEMTIEP { get; set; }
        public string TENTINH { get; set; }
        public string TENHUYEN { get; set; }
        public decimal TOTAL { get; set; }
        public decimal SOVUVIEC { get; set; }
        public decimal SOLUOTNGUOI { get; set; }
        public decimal SODDOANDONGNGUOI { get; set; }

    }


}
