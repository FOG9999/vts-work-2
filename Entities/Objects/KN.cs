using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.Models;
namespace Entities.Objects
{
    public class UserInfor
    {
        public USERS user_login;
        public TaikhoanAtion tk_action;
        public List<RequestTokenForm> Token_form;        
        
    }
    public class Captcha
    {
        public string captcha { get; set; }
        public string captchaImage { get; set; }
        public string captcha_encrypt { get; set; }
    }
    public class RequestTokenForm
    {
        public int iUser { get; set; }
        public string controller { get; set; }
        public string token { get; set; }
    }    
    public class TokenAction
    {
        public string controller { get; set; }
        public string token { get; set; }
        public int iUser { get; set; }
        public string key_url { get; set; }
        public int id_action { get; set; }
        public string date { get; set; }
    }
    public class TaiKhoan
    {
        public string ten { get; set; }
        public string phongban { get; set; }
        public string donvi { get; set; }
        public string chucvu { get; set; }
    }
    public class ID_Action
    {
        public int iAction { get; set; }
    }
    public class ID_ChuongTrinh
    {
        public int ICHUONGTRINH { get; set; }
    }
    public class ID_Parent
    {
        public int iParent { get; set; }
    }
    public class ID_Donvithuly
    {
        public int iDonViThuLy { get; set; }
    }
    public class ID_DinhKy
    {
        public int iDinhKy { get; set; }
    }
    public class TongHop_Kiennghi
    {

        public string linhvuc { get; set; }
        public string truockkyhop { get; set; }
        public string donvi_tonghop { get; set; }
        public string donvi_thamquyen { get; set; }
        public string kyhop { get; set; }
        public string khoahop { get; set; }
        public string kehoach { get; set; }
        public string bt_lichsu { get; set; }
        public string bt_info { get; set; }
        public string tr_traloi { get; set; }
        public string tinhtrang { get; set; }
        // 0:Chờ xử lý

        // 1:đã chuyển địa phương xử lý
        // 2:Đã chuyển ban dân nguyện

        // 3:đã chuyển đến đơn vị có thẩm quyền
        // 4: Đang xử lý
        // 5: đã trả lời, hoàn thành
        // 6: 
        // 4:Tiếp tục theo dõi

        /*
         Văn bản
         tonghop_chuyenxuly : chuyển tổng hợp kiến nghị cho đơn vị xử lý
         tonghop_chuyendonvi_xuly: chuyển đến đơn vị có thẩm quyền xử lý
         tonghop_traloi: trả lời tổng hợp kiến nghị
         */

    }
    public class KN_CL
    {

        public string linhvuc { get; set; }
        public string donvi_thamquyen { get; set; }
        public string donvi_tiepnhan { get; set; }
        public string kyhop { get; set; }
        public string khoahop { get; set; }
        public string kehoach { get; set; }
        public string bt_lichsu { get; set; }
        public string bt_info { get; set; }
        public string bt_edit { get; set; }
        public string bt_del { get; set; }
        public string bt_kiemtrung { get; set; }
        public string tinhtrang { get; set; }
        public string tr_traloi { get; set; }
        public string tr_giamsat { get; set; }
        public string tr_tonghop { get; set; }
        public string truockyhop { get; set; }
        public string file_view { get; set; }
        public string diachi_daydu { get; set; }
        public string diachi { get; set; }
        public string diachi_tinh { get; set; }
        public string diachi_huyen { get; set; }

        public string nguonkiennghi { get; set; }
        // 0: Mới cập nhật
        // 1: Đã chuyển địa phương xử lý
        // 2: Đã chuyển tổng hợp đến BDN
        // 3: BDN đã chuyển cơ quan thẩm quyền
        // 4: đang xử lý
        // 5: tổng hợp đã có trả lời
        // 6: kiến nghị đã có trả lời, đóng kiến nghị
        // 7: kiến nghị trùng, đóng kiến nghị
        // 8: theo dõi ở kỳ họp sau
        // 9: đã trả lại kiến nghị 

        /* kết quả trả lời kiến nghị KN_KIENNGHI_TRALOI */
        // 1: Đã có trả lời, 
        // 2: Trả lại kiến nghị
        // 3: Chuyển giải quyết kỳ họp sau
        // 4: Có lộ trình giải quyết
        // 5: Chưa có lộ trình giải quyết
        // 6: Chưa thể giải quyết ngay
        // 7: Chưa có nguồn lực giải quyết

    }
    public class KNTC
    {
        //tinh trang đơn: iTinhTrangXuLy
        //0: mới cập nhật; 1: đã chuyển xử lý; 2: Đã phân loại;3:chuyển xử lý; 4: Đang xử lý; 6:Hoàn thành; 5: ko xử lý, lưu theo dõi
        
        //Điều kiện xử lý: iDuDieuKien (nội bộ khi phân loại, thuộc thẩm quyền)
        //-1:chưa xđinh;0 ko đủ;1: đủ
        //Kết quả xử lý: iDuDieuKien_KetQua (nội bộ khi phân loại đơn đủ điều kiện, thẩm quyền)
        //-1:chưa xđinh;0 lưu đơn, theo dõi;1: chuyển xử lý;2: đang nghiên cứu;3:hướng dẫn, giải thích, trả lời
        public string nguoinop { get; set; }
        public string loaidon { get; set; }
        public string quoctich { get; set; }
        public string dantoc { get; set; }
        public string tinhchat { get; set; }
        public string linhvuc { get; set; }
        public string loai_noidung { get; set; }
        public string nguondon { get; set; }
        public string tinh { get; set; }
        public string huyen { get; set; }
        public string bt_lichsu { get; set; }
        public string bt_info { get; set; }
        public string bt_giamsat { get; set; }
        public string lydoluudon { get; set; }
        public string lydochitiet { get; set; }
        public string tinhtrang { get; set; }
        public string donvi_thuly { get; set; }
        public string donvi_tiepnhan { get; set; }
        public string diachi_nguoinop { get; set; }
        public string nguon { get; set; }
        public string domat { get; set; }
        public string dokhan { get; set; }
        public decimal sodontrung { get; set; }
        public string ketquadanhgia { get; set; }
    }
    public class ThuongXuyen
    {
        //tinh trang đơn: iTinhTrangXuLy
        //0: mới cập nhật; 1: đã chuyển xử lý; 2: Đã phân loại; 3: Đang xử lý; 4:Hoàn thành; 5: ko xử lý, lưu theo dõi
        //Điều kiện xử lý: iDuDieuKien (nội bộ khi phân loại, thuộc thẩm quyền)
        //-1:chưa xđinh;0 ko đủ;1: đủ
        //Kết quả xử lý: iDuDieuKien_KetQua (nội bộ khi phân loại đơn đủ điều kiện, thẩm quyền)
        //-1:chưa xđinh;0 lưu đơn, theo dõi;1: chuyển xử lý;2: đang nghiên cứu;3:hướng dẫn, giải thích, trả lời
        public string loaivuviec { get; set; }
        public string tinhchat { get; set; }
        public string linhvuc { get; set; }
        public string loai_noidung { get; set; }
        public string bt_info { get; set; }
        public string bt_trung { get; set; }
        public string coquan_tiep { get; set; }
        public string ngaytiep { get; set; }
    }
    public class DinhKy_VuViec
    {
        public string loaivuviec { get; set; }
        public string tinhchat { get; set; }
        public string linhvuc { get; set; }
        public string loai_noidung { get; set; }
        public string doan_dongnguoi { get; set; }
        public string traloi { get; set; }
        public string bt_info { get; set; }
    }
    public class ChuongtrinhCuTri
    {
        public string batdau { get; set; }
        public string ketthuc { get; set; }
        public string kyhop { get; set; }
        public string khoahop { get; set; }
        public string doandaibieu { get; set; }
        public string diaphuong_view { get; set; }
        public string daibieu_view { get; set; }
        public string file_view { get; set; }
        public string file_edit { get; set; }
        public string truockyhop { get; set; }
        public string bt_info { get; set; }
        public string bt_edit { get; set; }
        public string bt_del { get; set; }
        public string ngaybanhanh { get; set; }

    }
    // danh muc thêm ngày 04/12
    public class TaikhoanAtion
    {
        public bool is_admin { get; set; }
        public bool is_root { get; set; }
        public bool is_lanhdao { get; set; }
        public bool is_dbqh { get; set; }
        public bool is_chuyenvien { get; set; }
        public string tendonvi { get; set; }
        public string tenphongban { get; set; }
        public int iUser { get; set; }
        public string list_action { get; set; }
        public int is_groupquochoi { get; set; }
    }
    public class ID_Session_KienNghi_ChonTongHop { 
        public decimal IKIENNGHI { get; set; }
        public decimal ILINHVUC { get; set; }
        public decimal ITHAMQUYENDONVI { get; set; }
    }
}
