using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Utilities.Enums
{
    public enum TrangThaiXuLy
    {
        [StringValue("Đang nghiên cứu")]
        DangNghienCuu = 0,

        [StringValue("Hướng dẫn bằng văn bản")]
        HuongDanXuLy = 1,

        [StringValue("Nhận đơn chuyển sang KNTC")]
        NhanDon = 2,

        [StringValue("Chuyển xử lý cơ quan thẩm quyền")]
        ChuyenXuLy = 3,
        [StringValue("Giải thích hướng dẫn trực tiếp")]
        HuongDanTrucTiep = 4,
    }
    public enum TrangThaiChuyenXuLy
    {
        [StringValue("traloichuyenxuly")]
        TraLoiChuyenXuLy = 0,
    }

}
