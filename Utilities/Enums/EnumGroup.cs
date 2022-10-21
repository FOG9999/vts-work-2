using System;

namespace Utilities.Enums
{
    public enum Sex
    {
        [StringValue("Nam")]
        Nam = 1,
        [StringValue("Nữ")]
        Nu = 0,
    }
    public enum PhanLoaiThamSo
    {
        [StringValue("Tham số theo BHXH")]
        BHXH = 1,
        [StringValue("Tham số theo người dùng")]
        NguoiDung = 0,
    }
    public enum LoaiTinNhan
    {
        [StringValue("Bệnh nhân")]
        BenhNhan = 0,
        [StringValue("Cơ sở y tế")]
        CSYT = 1,
       
    }
  
    public enum DataType
    {
        [StringValue(@"Single line of text")]
        Type1 = 1,
        [StringValue(@"Multiple lines of text")]
        Type2 = 2,
        [StringValue(@"Number (1, 1.0, 100)")]
        Type3 = 3,
        [StringValue(@"Date and Time")]
        Type4 = 4,
        [StringValue(@"Lookup")]
        Type5 = 5,
        [StringValue(@"Yes/No (check box)")]
        Type6 = 6,
    }

    public enum Step
    {
        [StringValue("Chờ active")]
        Active = 1,
        [StringValue("Chờ thay đổi mật khâu")]
        ChangePass = 2,
        [StringValue("Chờ đăng ký CA")]
        RegistCa = 3,
        [StringValue("Đã có thể sử dụng")]
        Allow = 0,
    }
    //Nhóm phục vụ báo cáo 01/BV
    public enum ReportCode
    {
        [StringValue("Khám bệnh")]
        KhamBenh = 7,
        [StringValue("Ngày điều trị ngoại trú ")]
        NgayDTNGoaiTru = 8,
        [StringValue("Xét nghiệm")]
        XetNghiem = 1,
        [StringValue("Chẩn đoán hình ảnh")]
        ChanDoanHinhAnh = 2,
        [StringValue("Thăm dò chức năng")]
        ThamDoChucNang = 13,
        [StringValue("Thủ thuật, phẫu thuật ")]
        ThuThuatPhauThuat = 5,
        [StringValue("Dịch vụ kỹ thuật cao chi phí lớn")]
        DVKTCPCao = 9,
        [StringValue("Máu và chế phẩm máu")]
        MauVaChePhamMau = 4,
        [StringValue("Thuốc, dịch truyền - Trong danh mục BHYT")]
        ThuocDTBHYT = 2,
        [StringValue("Thuốc điều trị ung thư, chống thải ghép ngoài danh mục")]
        ThuocK = 10,
        [StringValue("Vật tư y tế - Trong danh mục BHYT  nhóm 1")]
        VTYTNhom1 = 6,
        [StringValue("Vật tư y tế - Trong danh mục BHYT  nhóm 2")]
        VTYTNhom2 = 11,
        [StringValue("Vận chuyển")]
        VanChuyen = 11,
    }
    //Điều kiện
    public enum NotIdentified
    {
        [StringValue("Chưa xác định - Tất cả")]
        Nam = -1,
        
    }

    //PageSize
    public enum Globals
    {
        [StringValue("Số lượng bản ghi trên trang")]
        PageSize = 20

    }


    public enum LoaiNguoiDung
    {
        [StringValue("Người dùng bệnh viện")]
        BenhVien = 1,
        [StringValue("Người dùng BHXH")]
        BHXH = 2
    }
    public enum CapDoNguoiDung
    {
        [StringValue("Cấp TW")]
        TW = 0,
        [StringValue("Tỉnh thành phố")]
        Tinh = 1,
        [StringValue("Quận/huyện")]
        QuanHuyen = 2,
        [StringValue("Xã/phường")]
        XaPhuong = 3,
    }
    public enum IsAdmin
    {
        [StringValue("Người dùng là Admin")]
        IsAdmin = 1,
        [StringValue("Người dùng không phải Admin")]
        NotAdmin = 0
    }

    public enum NhomChiPhiBHYT
    {
        [StringValue("Khám bệnh")]
        KhamBenh = 13,

        [StringValue("Giường điều trị ngoại trú")]
        GiuongNgoaiTru = 14,

        [StringValue("Giường điều trị nội trú")]
        GiuongNoiTru = 15,

        [StringValue("Xét nghiệm")]
        XetNghiem = 1,

        [StringValue("Chẩn đoán hình ảnh")]
        ChanDoanHinhAnh = 2,

        [StringValue("Thăm dò chức năng")]
        ThamDoChucNang = 3,

        [StringValue("Thủ thuật, phẫu thuật")]
        PhauThuatThuThuat = 8,

        [StringValue("Dịch vụ kỹ thuật cao chi phí lớn")]
        DVKTTheoTyLe = 9,

        [StringValue("Máu và chế phẩm máu")]
        MauVaChePhamMau = 7,

        [StringValue("Thuốc, dịch truyền trong danh mục BHYT")]
        ThuocTrongDanhMuc = 4,

        [StringValue("Thuốc, dịch truyền ngoài danh mục BHYT")]
        ThuocNgoaiDanhMuc = 6,

        [StringValue("Thuốc điều trị ung thư, chống thải ghép ngoài danh mục")]
        ThuocUngThu = 5,

        [StringValue("VTYT trong danh mục BHYT")]
        VTYTTrongDanhMuc = 10,

        [StringValue("VTYT ngoài danh mục BHYT")]
        VTYTTyLe = 11,

        [StringValue("Vận chuyển")]
        VanChuyen = 12,
    }

    public enum UserType
    {
        [StringValue("Root Admin")]
        RootAdmin = -1,
        [StringValue("Chưa phân quyền")]
        ChuaPhanQuyen = 0,
        [StringValue("Quản trị hệ thống")]
        Admin = 1,
        [StringValue("Lãnh đạo đơn vị")]
        LanhDao = 2,
        [StringValue("Chuyên viên Hội đồng Nhân dân")]
        ChuyenVienHDND = 3,
        [StringValue("Chuyên viên Hội đồng Nhân dân Đại biểu Quốc hội")]
        ChuyenVienDBQH = 4,
    }
}
