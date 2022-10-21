namespace Utilities.Enums
{
    /// <summary>
    /// Enum Log type
    /// </summary>
    /// Description: Biểu diễn các log
    public enum enLogType
    {
        [StringValue("Log thông thường")]
        NomalLog = 0,
        [StringValue("Log theo người dùng")]
        UserLog = 1,
        [StringValue("Log hệ thống")]
        SystemLog = 2,
        [StringValue("Log tấn công")]
        AttackLog = 3,
    }

    /// <summary>
    /// Các hành động của Log.
    /// </summary>
   
    public enum enActionType
    {
        [StringValue("Xem thông tin")]
        View = 0,

        [StringValue("Cập nhật thông tin")]
        Update = 1,

        [StringValue("Xóa thông tin")]
        Delete = 2,

        [StringValue("Xuất dữ liệu ra file")]
        ExportExcel = 3,

        [StringValue("Nhập dữ liệu từ file")]
        ImporttExcel = 4,

        [StringValue("Thêm mới")]
        Insert = 5,    
		
        [StringValue("Gửi duyệt")]
        SendApproved = 6,

        [StringValue("Phê duyệt")]
        Approve = 7,

        [StringValue("Hủy duyệt")]
        ApproveCancel = 8,
        [StringValue("Gửi SMS")]
        SendSMS = 9,
        [StringValue("Phân quyền")]
        Permission = 10,

        [StringValue("Đăng nhập")]
        Login = 11,

        [StringValue("Hành động khác")]
        Other = 100,
    }
}
