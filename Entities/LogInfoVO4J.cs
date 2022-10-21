using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Entities 
{
    public class LogInfoVO4J
    {


        /** 1.Kiểu log (
         * 	start_action: bắt đầu giao dịch, 
         * 	end_action: kết thúc giao dịch, 
         * 	start_connect: bắt đầu kết nối tới thành phần/module khác, 
         * 	end_connect: kết thúc kết nối tới thành phần/module khác) 
         *  error : log lỗi
         *  info  : log thông tin
         *  login : log login
         *  logout: log logout
         * */
        private String logType;

        public String LogType
        {
            get { return logType; }
            set { logType = value; }
        }
        /** 2.Ma ứng dụng  */
        private String appCode;

        public String AppCode
        {
            get { return appCode; }
            set { appCode = value; }
        }
        /** 3.Mã module trên quản lý tài nguyên VD : VIT2_VA_140414_Sabeco*/
        private String serviceCode;

        public String ServiceCode
        {
            get { return serviceCode; }
            set { serviceCode = value; }
        }
        /** 4.Mã session tự tăng và là duy nhất cho mỗi giao dịch */
        private String sessionID;

        public String SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }
        /** 5.IP, Port node cha (node đầu tiên của hệ thống CNTT tiếp nhận giao dịch) */
        private String ipPortParentNode;

        public String IpPortParentNode
        {
            get { return ipPortParentNode; }
            set { ipPortParentNode = value; }
        }
        /** 6.IP, Port node hiện tại (node thực hiện ghi log) */
        private String ipPortCurrentNode;

        public String IpPortCurrentNode
        {
            get { return ipPortCurrentNode; }
            set { ipPortCurrentNode = value; }
        }
        /** 7.Nội dung yêu cầu cần xử lý */
        private String requestContent;

        public String RequestContent
        {
            get { return requestContent; }
            set { requestContent = value; }
        }
        /** 8.Kết quả phản hồi */
        private String responseContent;

        public String ResponseContent
        {
            get { return responseContent; }
            set { responseContent = value; }
        }
        /** 9.Thời gian bắt đầu xử lý (YYYY/MM/DD hh:mi:ss:ms) */
        private String startTime;

        public String StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private DateTime startTimeD;

        public DateTime StartTimeD
        {
            get { return startTimeD; }
            set { startTimeD = value; }
        }
        /** 10.Thời gian kết thúc xử lý (YYYY/MM/DD hh:mi:ss:ms) */
        private String endTime;

        public String EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        private DateTime endTimeD;

        public DateTime EndTimeD
        {
            get { return endTimeD; }
            set { endTimeD = value; }
        }
        /** 11.Thời gian xử lý của nghiệp vụ (đơn vị mili-second) */
        private String duration;

        public String Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        /** 12.Mã lỗi trả về của giao dịch (đây là mã được quy định của mỗi hệ thống) */
        private String errorCode;

        public String ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
        /** 13.Mô tả chi tiết lỗi */
        private String errorDescription;

        public String ErrorDescription
        {
            get { return errorDescription; }
            set { errorDescription = value; }
        }
        /** 14.Kết quả thực hiện của giao dịch (thành công/không thành công) */
        private String transactionStatus;

        public String TransactionStatus
        {
            get { return transactionStatus; }
            set { transactionStatus = value; }
        }
        /** 15.Thao tác nghiệp vụ (đăng ký dịch vụ, đăng nhập/đăng xuất hệ thống, mua gói cước …) */
        private String actionName;

        public String ActionName
        {
            get { return actionName; }
            set { actionName = value; }
        }
        /** 16.Dùng để trace người dùng, mặc định null nếu không có */
        private String userName;

        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        /** 17.Là account của dịch vụ */
        private String account;

        public String Account
        {
            get { return account; }
            set { account = value; }
        }
        /** 18.Địa chỉ IP tác động của máy trạm */
        private String ipClientAddress;

        public String IpClientAddress
        {
            get { return ipClientAddress; }
            set { ipClientAddress = value; }
        }
        /** 19.Đường dẫn */
        private String path;

        public String Path
        {
            get { return path; }
            set { path = value; }
        }
        /** 20.Chức năng tác động hoặc URL_request trong giao dịch DEBUG */
        private String functionCode;

        public String FunctionCode
        {
            get { return functionCode; }
            set { functionCode = value; }
        }

        private String urlRequest;

        public String UrlRequest
        {
            get { return urlRequest; }
            set { urlRequest = value; }
        }
        /** 21.Các tham số thực thực hiện trong giao dịch */
        private String paramList;

        public String ParamList
        {
            get { return paramList; }
            set { paramList = value; }
        }
        /** 22.Tên lớp (hàm) tác động trong giao dịch  OPTION*/
        private String classMethod;

        public String ClassMethod
        {
            get { return classMethod; }
            set { classMethod = value; }
        }
        /** 23.Description  OPTION*/
        private String description;

        public String Description
        {
            get { return description; }
            set { description = value; }
        }
        private String userID;   // DMS bổ sung thêm

        public String UserID
        {
            get { return userID; }
            set { userID = value; }
        }


    }
}
