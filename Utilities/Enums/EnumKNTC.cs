using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Enums
{
    public enum TrangThaiDon
    {
        [StringValue("Đơn mới cập nhật")]
        MoiCapNhat = 0,

        [StringValue("Đơn đã chuyển xử lý, chờ phân loại")]
        DaChuyenXuLy = 1,

        [StringValue("Đơn đã phân loại")]
        DaPhanLoai = 2,

        [StringValue("Đơn chờ xử lý, giải quyết")]
        ChoXuLy = 3,

        [StringValue("Đơn đang xử lý")]
        DangXuLy = 4,

        [StringValue("Đơn không xử lý, lưu theo dõi")]
        KhongXuLy = 5,

        [StringValue("Đã xử lý, giải quyết")]
        HoanThanh = 6,
        
        [StringValue("Đơn đã hướng dẫn, trả lời")]
        DaHuongDan = 7,

        [StringValue("Đơn chưa giải quyết")]
        ChuaGiaiQuyet = 8,

        [StringValue("Đơn đã hướng dẫn")]
        DaHuongDanTraLoi = 9,
    }
    public enum TrangThaiDonDaXuLy
    {
        [StringValue("Đơn đã chuyển")]
        DaChuyen = 3,
        
        [StringValue("Đơn đã giải quyết")]
        HoanThanh = 6,

        [StringValue("Đơn đang giải quyết")]
        KetQua = 7,

        [StringValue("Đơn chưa giải quyết")]
        ChuaGiaiQuyet = 8,

        [StringValue("Đơn đã hướng dẫn trả lời")]
        DaHuongDanTraLoi = 9,
    }
    public enum DieuKienXuLy
    {
        [StringValue("Chưa xác định")]
        ChuaXacDinh = -1,

        [StringValue("Không đủ điều kiện")]
        KhongDuDieuKien = 0,

        [StringValue("Đủ điều kiện")]
        DuDieuKien = 1,
    }
    public enum ThamQuyenXuLy
    {
        [StringValue("Chưa xác định")]
        ChuaXacDinh = -1,

        [StringValue("Không thuộc thẩm quyền")]
        KhongThuocThamQuyen = 0,

        [StringValue("Thuộc thẩm quyền")]
        ThuocThamQuyen = 1,
    }
    public enum DoMat
    {
        [StringValue("Thường")]
        Thuong = 1,

        [StringValue("Mật")]
        Mat = 2,

        [StringValue("Tối mật")]
        ToiMat = 3,

        [StringValue("Tuyệt mật")]
        TuyetMat = 4,
    }
    public enum DoKhan
    {
        [StringValue("Thường")]
        Thuong = 1,

        [StringValue("Khẩn")]
        Khan = 2,

        [StringValue("Thượng khẩn")]
        ThuongKhan = 3,

        [StringValue("Hỏa tốc")]
        HoaToc = 4,
    }
    public enum LyDoLuuTheoDoi
    {
        [StringValue("Không chuyển")]
        KhongXuLy = 1,

        [StringValue("Không thụ lý hoặc bị đình chỉ")]
        KhongThuLy = 2,

        [StringValue("Đơn trùng")]
        DonTrung = 3,

        [StringValue("Đơn đã có văn bản trả lời")]
        DaTraLoi = 4,
    }
    public enum KetQuaDanhGia
    {
        [StringValue("Đúng")]
        Dung = 1,

        [StringValue("Sai bác đơn")]
        Sai = 2,

        [StringValue("1 phần")]
        Phan = 3,
    }
    public enum HienThiDon
    {
        [StringValue("Tất cả đơn")]
        All = 0,

        [StringValue("Đơn đã tiếp nhận ")]
        TiepNhan = 1,

        [StringValue("Đơn chuyển tới")]
        ThuLy = 2,
    }
    public enum LoaiLichSu
    {
        [StringValue("Chuyển xử lý")]
        ChuyenXuLy = 1,

        [StringValue("Luân chuyển đơn")]
        Luanchuyen = 2,
    }
    public enum TrangThaiDonDaTraLoi
    {
        [StringValue("Đơn đang xử lý")]
        DangXuLy = 4,
        [StringValue(" Đơn đã xử lý, giải quyết")]
        HoanThanh = 6,
    }
    public enum TrangThaiDonChuaTraLoi
    {
        [StringValue("Đơn đã chuyển, chờ xử lý")]
        ChoXuLy = 3,
        [StringValue("Đơn không xử lý, lưu theo dõi")]
        KhongXuLy = 5,
    }
    public enum TrangThaiDonDaChuyen
    {
        [StringValue("Đơn đang xử lý")]
        DangXuLy = 4,
        [StringValue("Đơn đã chuyển, chờ xử lý")]
        ChoXuLy = 3,
        [StringValue("Đơn không xử lý, lưu theo dõi")]
        KhongXuLy = 5,      
        [StringValue(" Đơn đã xử lý, giải quyết")]
        HoanThanh = 6,
    }

    public enum Loai_DoiTuongKNTC : int
    {

        [StringValue(@"Đoàn ĐBQH Tỉnh")]
        DBQH = 0,
        [StringValue(@"HĐND Tỉnh")]
        HDND = 1,

    }
}
