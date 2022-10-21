namespace Utilities.Enums

{
    public enum ID_Capcoquan : int
    {
        [StringValue(@"Ban Dân Nguyện")]
        Bandannguyen = 4,
        [StringValue(@"Quốc hội cơ quan trực thuộc Quốc hội")]
        Coquanquochoi = 1,
        [StringValue(@"Các Bộ, cơ quan ngang Bộ")]
        Bobannganh = 11,
        [StringValue(@"Các đoàn đại biểu, HĐND  các Tỉnh / Thành phố")]
        DoanDBQH = 20,
    }

    public enum ThamQuyen_DiaPhuong : int
    {
        [StringValue(@"TW")]
        Trunguong = 1,
        [StringValue(@"TINH")]
        Tinh = 2,
        [StringValue(@"HUYEN")]
        Huyen = 3,
    }

    public enum Parent_Huyen : int
    {
        [StringValue(@"Huyện ủy")]
        HuyenUy = 178,
        [StringValue(@"Ủy ban mặt trận TQ các Huyện/TP/TX")]
        UyBanMatTran = 211,
        [StringValue(@"Các đơn vị hành chính trên địa bàn huyện")]
        DonViHanhChinh = 239,
    }

    public enum Parent_Tinh_TW : int
    {
        [StringValue(@"Cơ quan Trung ương")]
        TW = 1,
        [StringValue(@"Các đơn vị thuộc tỉnh thành")]
        TINH = 135,
    }

    public enum TrangThaiKienNghi : int
    {
        // 0: Mới cập nhật
        // 1: Đã chuyển địa phương xử lý
        // 2: Đã chuyển tổng hợp đến BDN
        // 3: BDN đã chuyển cơ quan thẩm quyền
        // 4: đang xử lý
        // 5: tổng hợp đã có trả lời
        // 6: kiến nghị đã có trả lời, đóng kiến nghị
        // 7: kiến nghị trùng, đóng kiến nghị
        // 8: Chưa xử lý
        // 9: đã trả lại kiến nghị 
        [StringValue(@"Tất cả")]
        All = -1,

        [StringValue(@"Mới cập nhật")]
        Moicapnhat = 0,

        [StringValue(@"Kiến nghị chờ xử lý")]
        Choxuly = 1,

        //[StringValue(@"Đã tổng hợp chuyển Ban Dân nguyện xử lý")]
        //DaChuyenTongHopDenBDN = 2,

        [StringValue(@"Kiến nghị chưa trả lời")]
        ChuaTraLoiDangXuly = 3,

        [StringValue(@"Kiến nghị đang xem xét giải quyết / đang xử lý")]
        KienNghiDangXemXetGiaiQuyet = 4,

        [StringValue(@"Kiến nghị đã giải trình, cung cấp thông tin")]
        KienNghiDaGiaiTrinh = 5,

        [StringValue(@"Kiến nghị đã giải quyết xong")]
        KienNghiDaTraLoi = 6,

        [StringValue(@"Kiến nghị trùng")]
        KienNghiTrung_DongKienNghi = 7,

        [StringValue(@"Chưa xử lý")]
        ChuaXuLy = 8,

        //[StringValue(@"Đã trả lại kiến nghị")]
        //DaTraLaiKienNghi = 9,

        [StringValue(@"Tạm xóa kiến nghị")]
        TamxoaKiennghi = 10
    }
    public enum TrangThai_TraLoiKienNghi : int
    {
        //-1: Tất cả
        // 1: Đã có trả lời, 
        // 2: Trả lại kiến nghị
        // 3: Chuyển giải quyết kỳ họp sau
        // 4: Có lộ trình giải quyết
        // 5: Chưa có lộ trình giải quyết
        // 6: Chưa thể giải quyết ngay
        // 7: Chưa có nguồn lực giải quyết
        // 19: Chưa giải quyết

        
        [StringValue(@"Đã có trả lời")]
        DaCoTraLoi = 1,
        [StringValue(@"Trả lại kiến nghị")]
        TraLaiKienNghi = 2,
        [StringValue(@"Chuyển giải quyết kỳ họp sau")]
        ChuyenGiaiQuyetKyHopSau = 3,
        [StringValue(@"Có lộ trình giải quyết")]
        CoLoTrinhGiaiQuyet = 4,
        [StringValue(@"Chưa có lộ trình giải quyết")]
        ChuaCoLoTrinhGiaiQuyet = 5,
        [StringValue(@"Chưa thể giải quyết ngay")]
        ChuaTheGiaiQuyetNgay = 6,
        [StringValue(@"Chưa có nguồn lực giải quyết")]
        ChuaCoNguonLucGiaiQuyet = 7,
        [StringValue(@"Đang xem xét giải quyết")]
        Xemxetgiaiquyet = 7,
        [StringValue(@"Giải trình, cung cấp thông tin")]
        Giaitrinh = 1,
        [StringValue(@"Đã giải quyết xong")]
        Dagiaiquetxong = 2,
        [StringValue(@"Chưa giải quyết")]
        Chuagiaiquyet = 19


    }
    public enum PhanLoai_TraLoiKienNghi : int
    {
        [StringValue(@"Giải trình, cung cấp thông tin ")]
        GIAITRINH_CUNGCAPTHONGTIN = 1,
        [StringValue(@"Đã giải quyết xong: Ban hành hoặc sửa đổi, bổ sung VBQPPL")]
        DAGIAIQUYET_BANHANHVANBAN = 3,
        [StringValue(@"Đã giải quyết xong: Thanh tra kiểm tra")]
        DAGIAIQUYET_THANHTRA_KIEMTRA = 4,
        [StringValue(@"Đã giải quyết xong: Tổ chức thực hiện")]
        DAGIAIQUYET_TOCHUC_THUCHIEN = 5,
        [StringValue(@"Đang xem xét giải quyết: Có lộ trình giải quyết")]
        DANGGIAIQUYET_COLOTRINH = 7,
        [StringValue(@"Đang xem xét giải quyết: Đã trình chính phủ ban hành văn bản")]
        DANGGIAIQUYET_TRINHCHINHPHU = 8,
        [StringValue(@"Đang xem xét giải quyết: Không lộ trình giải quyết")]
        DANGGIAIQUYET_KHONGLOTRINH = 9,
        [StringValue(@"Đang xem xét giải quyết: Chưa có nguồn lực")]
        DANGGIAIQUYET_CHUACONGUONLUC = 10,
        [StringValue(@"Đang xem xét giải quyết: Chưa thể giải quyết được ngay")]
        DANGGIAIQUYET_CHUAGIAIQUYETDUOCNGAY = 11,
        [StringValue(@"Đang xem xét giải quyết: Liên quan văn bản cấp trên")]
        DANGGIAIQUYET_VANBANCAPTREN = 12,
        [StringValue(@"Đang xem xét giải quyết: Lý do khác")]
        DANGGIAIQUYET_LYDOKHAC = 13
    }
    public enum TrangThai_TongHop : int
    {
        // 0:Chờ xử lý
        // 1:đã chuyển địa phương xử lý
        // 2:Đã chuyển ban dân nguyện
        // 3:đã chuyển đến đơn vị có thẩm quyền
        // 4: Đang xử lý
        // 5: đã trả lời, hoàn thành

        [StringValue(@"Chờ xử lý")]
        ChoXuLy = 0,
        [StringValue(@"Đã chuyển địa phương xử lý")]
        DaChuyenDiaPhuongXuLy = 1,
        [StringValue(@"Đã chuyển Ban Dân Nguyện")]
        DaChuyenBanDanNguyen = 2,
        [StringValue(@"Đã chuyển đến đơn vị có thẩm quyền")]
        DaChuyenDenDonViCoThamQuyen = 3,
        [StringValue(@"Đang xử lý")]
        DangXuLy = 4,
        [StringValue(@"Đã trả lời, hoàn thành")]
        DaTraLoi = 5
    }

    public enum Loai_DoiTuong : int
    {

        [StringValue(@"Đoàn ĐBQH Tỉnh")]
        DBQH = 0,
        [StringValue(@"HĐND Tỉnh")]
        HDND = 1,
        
    }

}
