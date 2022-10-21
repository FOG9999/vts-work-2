using System;

namespace Utilities.ReportForm.Entity
{
    /// <summary>
    /// Lưu các thông tin phần Header report
    /// </summary>
    /// Other: Tony
    /// Description:
    [Serializable]
    public class FHeader
    {
        public string CompanyName = "BỆNH VIỆN BẠCH MAI";
        public string CompanySub = "Phòng tổng hợp";
        public string Province = "Hà Nội";
        public string SubTime = "Hà Nội, Ngày ... Tháng ... Năm ....";
        public string Title = "THỐNG KÊ.........................................";
        public string NationalTitle = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
        public string NationalSlogan = "Độc lập - Tự do - Hạnh phúc";
        public string DateFromTo = "(Từ ngày ...../...../....... Đến ngày ...../...../.......)";
        public string SignalSub = "(Ký, ghi rõ họ tên)";
        public string SignalLeft = "Người tạo";
        public string SignalRight = "Thủ trưởng";
    }
}