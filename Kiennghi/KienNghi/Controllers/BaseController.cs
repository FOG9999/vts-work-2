using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Entities.Models;
using DataAccess.Busineess;
using Utilities;
using System.Web;
using Entities.Objects;
using KienNghi.Models;

namespace KienNghi.Controllers
{
    public class BaseController : Controller
    {
        private Funtions func = new Funtions();
        private BaseBusineess _base = new BaseBusineess();
        public KiennghiBusineess _kiennghi = new KiennghiBusineess();
        public ThietlapBusineess _thietlap = new ThietlapBusineess();
        Log log = new Log();
        private Dictionary<string, object> _condition;
        public int Max_FileSize = 10485760; //1M
        public string[] file_type = { "doc", "docx", "pdf", "jpg", "jpeg", "xls", "xlsx" };
        public int pageSize = 20;
        public int ID_Coquan_doandaibieu = 135;
        public int ID_Coquan_bo_nganh = 11;
        public int ID_Coquan_quochoi = 1;
        public int ID_Ban_DanNguyen = 4;
        public int ID_UY_BAN_NHAN_DAN = AppConfig.ID_UY_BAN_NHAN_DAN; //429
        public int ID_PhongBan_VP_DBQH_HDND = 30;
        public int ID_DDBQH_THA = 127;
        public int ID_HDND_Tinh = 137;
        public int ID_Coquan_HoiDong_DiaPhuong = AppConfig.ID_BAN_DAN_NGUYEN_NEW_PARENT;  //251;
        public int ID_Ban_DanNguyen_New = AppConfig.ID_BAN_DAN_NGUYEN_NEW; // 4
        public int ID_DiaPhuong_HienTai = AppConfig.IDIAPHUONG;// Tinh Thanh Hoa, 317
        public int ID_KyHop_HienTai()
        {
            return _base.Get_ID_KyHop_HienTai();
        }
        public int ID_KhoaHop_HienTai()
        {
            return _base.Get_ID_KhoaHop_HienTai();
        }
        public string action_thietlap = "21,22,23,24,25,26,27,28,29,30,31";
        public string action_baocao = "33";
        public string action_kiennghi = "2,3,4,5,6,7,8";
        public string action_kntc = "10,11,12,13,14,15,44";
        public string action_tiepdan = "17,18,19";
        private enum ImageFileExtension
        {
            none = 0,
            jpg = 1,
            jpeg = 2
        }
        public enum FileType
        {
            Image = 1,
            PDF = 2,
            DOC = 3,
            DOCX = 4
        }


        #region ui
        protected virtual JsonResult ToCheckboxList(ChecklistModel vm)
        {
            var result = this.RenderRazorViewToString("_CheckboxList", vm);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        protected virtual JsonResult ToDropdownList(ChecklistModel vm)
        {
            var result = vm.List
                .Select(x => string.Format("<option value='{0}'>{1}</option>", x.Value, x.Text));

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        protected virtual JsonResult ToDropdownList(SelectList list)
        {
            var result = list
                .Select(x => string.Format("<option value='{0}'>{1}</option>", x.Value, x.Text));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public int id_user()
        {
            //string session_user = Request.Cookies["user_id"].Value;
            //string session_user_decrypt = HashUtil.Decrypt(session_user, Convert.ToBoolean(HashUtil.HashType.SHA512));
            //int iUserLogin = Convert.ToInt32(session_user_decrypt.Split('|')[0]);

            return GetUserInfor().tk_action.iUser;
        }
        public Boolean CheckLogin()
        {
            //if (Request.Cookies["user_id"] == null || Request.Cookies["AuthToken"] == null || Session["userInfo"]==null)
            if (Request.Cookies["user_id"] == null || Request.Cookies["AuthToken"] == null || Session["userInfo"] == null)
            {
                //string user_id = "";string AuthToken = "";string userInfo = "";
                //if(Request.Cookies["user_id"] == null) { user_id = "null"; }
                //if (Session["AuthToken"] == null) { AuthToken = "null"; }
                //if (Session["userInfo"] == null) { userInfo = "null"; }
                //log.LogInfo("Username:Unknow", "CheckLogin()", "user_id:"+ user_id+"*"+ "AuthToken:" + AuthToken + "*"+ "userInfo:" + userInfo);
                //Response.Redirect("/Home/Logout/");
                return false;
            }
            else
            {
                UserInfor infor = (UserInfor)Session["userInfo"];
                if (infor == null) {
                    //Response.Redirect("/Home/Logout/");
                    //log.LogInfo("Username:Unknow", "CheckLogin()", "UserInfor()");
                    return false;
                }else
                {
                    func.SetCookies("user_id", Request.Cookies["user_id"].Value);
                    return true;
                }                
            }
        }
        public string RemovePageFromUrl(bool queryString = true, string actionName = "", string controllerName = "")
        {
            string url = Request.RawUrl;
            if (queryString)
            {
                string[] url_split = url.Split('&');
                if (url_split.Length > 1)
                {
                    url = "";
                    if (url_split.Length == 2)
                    {
                        url = url_split[0];
                        if (Request.RawUrl.IndexOf("&page=") == -1)
                        {
                            url += "&" + url_split[1];
                        }
                    }
                    else
                    {
                        int count = 0;
                        foreach (var u in url_split)
                        {
                            if (u.IndexOf("post_per_page") != -1)
                            {
                                url += "&" + u;
                                count++;
                            }
                            else
                            {
                                if (u.IndexOf("page=") == -1 && u != "")
                                {
                                    if (count > 0)
                                    {
                                        url += "&";
                                    }
                                    url += u;
                                    count++;
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (url.IndexOf("?") == -1)
                    {
                        url += "?";
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                url = Url.Action(actionName, controllerName);
            }
            return url;
        }
        public string RemovePostPerPageFromUrl(bool queryString = true, string actionName = "", string controllerName = "")
        {
            //string url = Request.RawUrl;
            string url = RemovePageFromUrl(queryString, actionName, controllerName);
            if (queryString)
            {
                string[] url_split = url.Split('&');
                if (url_split.Length > 1)
                {
                    url = "";
                    if (url_split.Length == 2)
                    {
                        url = url_split[0];
                        if (Request.RawUrl.IndexOf("&post_per_page=") == -1)
                        {
                            url += "&" + url_split[1];
                        }
                    }
                    else
                    {
                        int count = 0;
                        foreach (var u in url_split)
                        {
                            if (count < url_split.Length)
                            {
                                //if (u.IndexOf("post_per_page=") == -1 || u.IndexOf("page=") == -1)
                                if (u.IndexOf("post_per_page=") == -1)
                                {
                                    if (count > 0)
                                    {
                                        url += "&";
                                    }
                                    url += u;
                                    count++;
                                }
                            }
                        }
                    }
                    url = url.Replace("&&", "&");
                }
                else
                {
                    url += "?";
                }
                url = url.Replace("??", "?");
            }
            return url;
        }
        public Boolean CheckAuthToken()
        {
            bool check = true;
            if (!CheckLogin())
            {
                //Response.Redirect("/Home/Logout/");
                check = false;
            }else
            {
                UserInfor user_info = GetUserInfor();
                USERS u = user_info.user_login;
                string auth_user = u.CAUTHTOKEN;
                string auth_cookie = Request.Cookies["AuthToken"].Value;
                if (!auth_cookie.Equals(auth_user))
                {
                    //string auth_cookie = Session["AuthToken"].ToString();
                    //string auth_user = u.CAUTHTOKEN;
                    //Response.Redirect("/Home/Logout/");
                    //log.LogInfo("Username:Unknow", "CheckAuthToken()", "auth_cookie:"+ auth_cookie+ "*CAUTHTOKEN:"+ auth_user);
                    check = false;
                }
                else
                {
                    string guid = Guid.NewGuid().ToString();
                    u.CAUTHTOKEN = guid;
                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                    //System.Web.HttpContext.Current.Session.Add("AuthToken", guid);
                    //log.LogInfo("Username:Unknow", "SetAuthToken", "AuthToken:" + guid);
                    SetUserInfor(u,user_info.tk_action, user_info.Token_form);
                    
                }
            }            
            return check;
        }
        public Boolean CheckAuthToken_Api()
        {
            return CheckAuthToken();
            //UserInfor user_info = GetUserInfor();
            //USERS u = user_info.user_login;
            //string auth_user = u.CAUTHTOKEN;
            //if (Session["AuthToken"] == null) { return false; }
            //if (!Session["AuthToken"].ToString().Equals(auth_user))
            //{
            //    return false;
            //}
            //else
            //{
            //    string guid = Guid.NewGuid().ToString();
            //    u.CAUTHTOKEN = guid;
            //    System.Web.HttpContext.Current.Session.Add("AuthToken", guid);
            //    SetUserInfor(u, user_info.tk_action, user_info.Token_form);
            //    //Response.Cookies.Add(new HttpCookie("AuthToken", guid));

            //    return true;
            //}
        }
        public Boolean CheckTokenAction(string controller, int id_action = 0)
        {
            UserInfor user_info = GetUserInfor();
            List<RequestTokenForm> token = user_info.Token_form;
            if (token == null)
            {
                return false;
            }
            else
            {
                USERS u = user_info.user_login;
                string token_act = HashUtil.Encode_Action((int)u.IUSER, controller, id_action);
                RequestTokenForm token_requets = new RequestTokenForm();
                token_requets.token = token_act;
                token_requets.iUser = (int)u.IUSER;
                token_requets.controller = controller;
                if (token.Where(x => x.controller == controller && x.iUser == (int)u.IUSER && x.token == token_act).Count() >0)
                {
                    RequestTokenForm token_remove = token.Where(x => x.controller == controller && x.iUser == (int)u.IUSER && x.token == token_act).First();
                    token.Remove(token_remove);
                    SetUserInfor(u, user_info.tk_action, token);
                    return true;
                }else
                {
                    return false;
                }
               
            }
        }
        public Boolean SetTokenAction(string controller, int id_action = 0)
        {
            UserInfor user_info = GetUserInfor();
            List<RequestTokenForm> token = user_info.Token_form;
            USERS u = user_info.user_login;
            string token_act = HashUtil.Encode_Action((int)u.IUSER, controller, id_action);
            RequestTokenForm token_action = new RequestTokenForm();
            token_action.controller = controller;
            token_action.token = token_act;
            token_action.iUser = (int)u.IUSER;
            if (token == null)
            {
                token = new List<RequestTokenForm>();
                token.Add(token_action);
            }
            else
            {
                if (token.Where(x => x.controller == controller && x.iUser == (int)u.IUSER && x.token == token_act).Count() == 0)
                {
                    token.Add(token_action);
                }
            }
            return SetUserInfor(u, user_info.tk_action, token);

        }
        public Boolean RemoveTokenAction(string controller, int id_action)
        {
            try
            {
                UserInfor user_info = GetUserInfor();
                List<RequestTokenForm> token = user_info.Token_form;
                USERS u = user_info.user_login;
                string token_act = HashUtil.Encode_Action((int)u.IUSER, controller, id_action);
                RequestTokenForm token_action = new RequestTokenForm();
                token_action.controller = controller;
                token_action.token = token_act;
                token_action.iUser = (int)u.IUSER;
                if (token.IndexOf(token_action) == -1)
                {
                    token.Remove(token_action);
                }
                return true;
            }
            catch
            {
                return false;
            }
            
           
        }
        
        public Boolean Tracking(int iUser, string action)
        {
            try
            {
                TRACKING t = new TRACKING();
                t.IUSER = iUser;
                t.IVANBAN = 0;
                t.IKIENNGHI = 0; t.ITONGHOP = 0; t.IDON = 0; t.ITIEPDAN_THUONGXUYEN = 0;
                t.DDATE = DateTime.Now;
                t.CACTION = action;
                _base.InsertTracking(t);
                return true;
            }
            catch
            {
                return false;
            }            
        }
       
        public Boolean CheckFile_Type(string type)
        {
            string[] type_mime = { "application/msword",//doc
                                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",//docx
                                    "image/png",//png
                                    "image/jpeg",//jpeg
                                    "image/jpg",//jpg
                                    "application/pdf",//pdf
                                    "application/vnd.ms-excel",//xls
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"//xlsx
                                    };
            if (type_mime.Contains(type.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isValidImageFile(byte[] bytFile, String FileContentType)
        {
            bool isvalid = false;

            byte[] chkBytejpg = { 255, 216, 255, 224 };

            ImageFileExtension imgfileExtn = ImageFileExtension.none;

            if (FileContentType.Contains("jpg") | FileContentType.Contains("jpeg"))
            {
                imgfileExtn = ImageFileExtension.jpg;
            }


            if (imgfileExtn == ImageFileExtension.jpg || imgfileExtn == ImageFileExtension.jpeg)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkBytejpg[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }
            return isvalid;
        }
        public Boolean CheckFile_Upload(HttpPostedFileBase upload)
        {
            bool check = true;
            int iFileSize = upload.ContentLength;
            if (iFileSize > Max_FileSize)
            {
                //func.Redirect_Current("/Home/Error/?type=size");
                check = false;
            }
            string fileName = upload.FileName; // getting File Name
            string fileContentType = upload.ContentType; // getting ContentType
            byte[] tempFileBytes = new byte[upload.ContentLength]; // getting filebytes
            var data = upload.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(upload.ContentLength));
            string file_type = upload.ContentType;
            if (!CheckFile_Type(file_type))
            {
                //func.Redirect_Current("/Home/Error/?type=type");
                check = false;
            }
            //else
            //{
            //    if (file_type == "image/jpeg" || file_type == "image/jpg")
            //    {
            //        var result = isValidImageFile(tempFileBytes, fileContentType); // Validate 
            //        check = result;
            //        //if (result == false)
            //        //{
            //        //    //func.Redirect_Current("/Home/Error/?type=type");
            //        //    check = false;
            //        //}
            //    }
            //    //if (file_type == "application/pdf")
            //    //{
            //    //    var result = isValidPDFFile(tempFileBytes, fileContentType); // Validate 
            //    //    check = result;
            //    //    //if (result == false)
            //    //    //{
            //    //    //    //func.Redirect_Current("/Home/Error/?type=type");
            //    //    //}
            //    //}
            //}
            return check;

        }
        private enum PDFFileExtension
        {
            none = 0,
            PDF = 1
        }
        public static bool isValidPDFFile(byte[] bytFile, String FileContentType)
        {
            byte[] chkBytepdf = { 37, 80, 68, 70 };
            bool isvalid = false;

            PDFFileExtension pdffileExtn = PDFFileExtension.none;
            if (FileContentType.Contains("pdf"))
            {
                pdffileExtn = PDFFileExtension.PDF;
            }

            if (pdffileExtn == PDFFileExtension.PDF)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (Int32 i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkBytepdf[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }

            return isvalid;
        }
        //public Boolean RemoveTokenAction(int iUser, string controller, int id_action, string token)
        //{
        //    string token_encript = HashUtil.Encode_Action(iUser, controller, id_action);
        //    if (token.IndexOf("*") != -1)//nhiều token cùng thời điểm
        //    {
        //        string[] token_ = token.Split('*');
        //        foreach (var to in token_)
        //        {
        //            if (to.Equals(token_encript))
        //            {
        //                //db.Database.ExecuteSqlCommand("delete from token where token_action='" + to + "'");
        //                _condition = new Dictionary<string, object>();
        //                _condition.Add("TOKENACTION", to);
        //                var l_token = _base.GetAllToken(_condition).ToList();
        //                foreach (var d in l_token)
        //                {

        //                    TOKEN tokenID = _base.GetByIDToken((int)d.ID);
        //                    _base.DeleteToken(tokenID);
        //                }
        //                token = token.Replace(to, "");
        //            }
        //        }
        //        if (token == "*")//hết các phiên
        //        {
        //            func.RemoveCookies("token_action");
        //        }
        //        else//còn phiên
        //        {
        //            if (token.Substring(0, 1) == "*")// xóa token phía trước
        //            {
        //                token = token.Remove(0, 1);
        //            }
        //            else
        //            {
        //                token = token.Remove(token.Length - 1);
        //            }
        //            func.SetCookies("token_action", token);
        //        }
        //    }
        //    else
        //    {


        //        _condition = new Dictionary<string, object>();
        //        _condition.Add("TOKENACTION", token);
        //        var l_token = _base.GetAllToken(_condition).ToList();
        //        foreach (var d in l_token)
        //        {

        //            TOKEN tokenID = _base.GetByIDToken((int)d.ID);
        //            _base.DeleteToken(tokenID);
        //        }
        //        func.RemoveCookies("token_action");
        //    }
        //    string now = DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd hh:mm:ss");

        //    var token2 = _base.GetListToken("select * from token where dTime<='" + now + "'").ToList();
        //    foreach (var d in token2)
        //    {

        //        TOKEN tokenID = _base.GetByIDToken((int)d.ID);
        //        _base.DeleteToken(tokenID);
        //    }
        //}
        public string Checkbox_Checked(int status)
        {
            if (status == 0) { return ""; } else { return " checked "; }
        }
        //public Boolean ActionMulty(string action, int iUser)
        //{
        //    if (IsAdmin(iUser))
        //    {
        //        return true;
        //    }
        //    else
        //    {

        //        if (_base.GetList_UserAction("select * from user_action where iUser=" + iUser + " and iAction IN (" + action + ")").Count() > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}
        //public Boolean IsAdmin(int iUser)
        //{
        //    if (IS_Root(iUser)) { return true; }
        //    if ((int)_base.GetByIDUser(iUser).ITYPE == 1)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public Boolean IS_Root(int iUser)
        //{
        //    if ((int)_base.GetByIDUser(iUser).ITYPE == -1)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}     
        public int IDDonVi_User(int iUser)
        {
            return (int)_base.GetByIDUser(iUser).IDONVI;
        }
        public string Get_TenDonVi(int iUser)
        {
            int iDonVi = IDDonVi_User(iUser);
            return _base.GetByIDQuochoiCoquan(iDonVi).CTEN;
        }
        //public void Tracking_kiennghi(int iUser, int iKienNghi, string action)
        //{
        //    TRACKING t = new TRACKING();
        //    t.IUSER = iUser;
        //    t.IDON = 0;
        //    t.IVANBAN = 0;
        //    t.IKIENNGHI = iKienNghi; t.ITONGHOP = 0;
        //    t.DDATE = DateTime.Now;
        //    t.CACTION = action;
        //    _base.InsertTracking(t);
        //}
        //public void Check_Exist_ID_KienNghi(int id)
        //{
        //    _condition = new Dictionary<string, object>();
        //    _condition.Add("IKIENNGHI", id);
        //    if (_base.HienThiDanhSachKienNghi(_condition).Count() == 0)
        //    {
        //        func.Redirect_Current("/Home/Error/");
        //    }

        //}
        //public void Tracking_thuongxuyen(int iUser, int tiepdan, string action)
        //{
        //    TRACKING t = new TRACKING();
        //    t.IUSER = iUser; t.IVANBAN = 0;
        //    t.ITIEPDAN_THUONGXUYEN = tiepdan;
        //    t.IKIENNGHI = 0; t.ITONGHOP = 0; t.IDON = 0; t.ITIEPDAN_DINHKY = 0;
        //    t.DDATE = DateTime.Now;
        //    t.CACTION = action;
       
        //    _base.InsertTracking(t);
        //}

        public Boolean SetUserInfor(USERS u,TaikhoanAtion tk_action, List<RequestTokenForm> token=null)
        {
            try
            {
                UserInfor info = new UserInfor();
                info.user_login = u;
                info.tk_action = tk_action;
                info.Token_form = token;
                System.Web.HttpContext.Current.Session.Add("userInfo", info);
                //log.LogInfo("Username:Unknow", "SetAuthToken", "user_login.AuthToken:" + info.user_login.CAUTHTOKEN);
                return true;
            }
            catch
            {
                return false;
            }           
        }
        public Boolean SetUserInfor_After_Login(USERS u, List<RequestTokenForm> token = null)
        {
            try
            {
                UserInfor info = new UserInfor();
                info.user_login = u;
                info.tk_action = _base.tk_action((int)u.IUSER);
                info.Token_form = token;
                System.Web.HttpContext.Current.Session.Add("userInfo", info);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public UserInfor GetUserInfor()
        {
            UserInfor info = new UserInfor();
            if (System.Web.HttpContext.Current.Session["userInfo"] != null)
            {
                info = (UserInfor)Session["userInfo"];                
            }
            return info;
        }

        public string Get_Option_ThamQuyen_TrungUong(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            donvi.Add("IDELETE", 0); donvi.Add("IHIENTHI", 1);
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            return "<option value='0'>Chọn đơn vị thẩm quyền</option>" + OptionCoQuanXuLy_TrungUong(coquan, 0, 0, iDonVi, 0);
        }
        public string Get_Option_ThamQuyen_DiaPhuong(int iDonVi = 0)
        {
            UserInfor u_info = GetUserInfor();
            QUOCHOI_COQUAN coquan_diaphuong = _kiennghi.HienThiThongTinCoQuan((int)u_info.user_login.IDONVI);

            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            donvi.Add("IDELETE", 0);
            donvi.Add("IHIENTHI", 1);
            donvi.Add("IDIAPHUONG", coquan_diaphuong.IDIAPHUONG);
            donvi.Add("IPARENT", ID_Coquan_HoiDong_DiaPhuong);
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);

            if (coquan.Count > 0)
            {
                QUOCHOI_COQUAN coquan_hdnd = coquan.FirstOrDefault();
                //List<QUOCHOI_COQUAN> coquan_hdnd_diaphuong=new List<QUOCHOI_COQUAN>();
                //coquan_hdnd_diaphuong.Add(coquan_hdnd);
                Dictionary<string, object> donvi_diaphuong = new Dictionary<string, object>();
                donvi_diaphuong.Add("IDELETE", 0);
                donvi_diaphuong.Add("IHIENTHI", 1);
                donvi_diaphuong.Add("IPARENT", coquan_hdnd.ICOQUAN);
                var list_coquan_diaphuong = _kiennghi.GetAll_CoQuanByParam(donvi_diaphuong);
                list_coquan_diaphuong.Add(coquan_hdnd);
                //coquan = coquan.Where(x => x.IPARENT != ID_Coquan_quochoi).ToList();
                return "<option value='0'>Chọn đơn vị thẩm quyền</option>" + OptionCoQuanXuLy_DiaPhuong(list_coquan_diaphuong, ID_Coquan_HoiDong_DiaPhuong, 0, iDonVi, 0);

            }
            else
            {
                return "<option value='0'>Chọn đơn vị thẩm quyền</option>";
            }

        }
        public string Get_Option_ThamQuyen_DiaPhuong_Parent_VP_DBQH_HDND(int iPhongBan = 0)
        {
            UserInfor u_info = GetUserInfor();
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            donvi.Add("IDELETE", 0); donvi.Add("IHIENTHI", 1); donvi.Add("IDONVI", ID_UY_BAN_NHAN_DAN);
            var listPhongBan = _thietlap.GetBy_List_Phongban(donvi);
            var VP_DB_QH = _thietlap.GetBy_Quochoi_CoquanID(ID_UY_BAN_NHAN_DAN);
            string str = "";
            if (listPhongBan.Count > 0)
            {
                str = "<optgroup label='" + VP_DB_QH.CTEN + "'";
                str += "<option value='0' onclick ='DoiChuyenVien(this.value')>Chọn đơn vị thẩm quyền</option>";

                foreach (var item in listPhongBan)
                {
                    str += "<option ";
                    if (item.IPHONGBAN == iPhongBan)
                        str += "selected ";
                    str += "value='" + item.IPHONGBAN + "'onclick ='DoiChuyenVien(this.value)'>" + item.CTEN + "</option>";
                }
                str += "</optgroup>";
                return str;
            }

               

            else
            {
                return "<option value='0' onclick ='DoiChuyenVien(this.value)'>Chọn đơn vị thẩm quyền</option>" ;
            }

        }
        public string Get_Option_ThamQuyen_DiaPhuong_Parent(int iDonVi = 0)
        {
            UserInfor u_info = GetUserInfor();
            QUOCHOI_COQUAN coquan_diaphuong = _kiennghi.HienThiThongTinCoQuan((int)u_info.user_login.IDONVI);

            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            donvi.Add("IDELETE", 0); donvi.Add("IHIENTHI", 1); donvi.Add("IPARENT", ID_Coquan_HoiDong_DiaPhuong);
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            if (coquan.Count > 0)
            {
                //coquan = coquan.ToList();
                return "<option value='0'>Chọn đơn vị thẩm quyền</option>" + OptionCoQuanXuLy_DiaPhuong(coquan, ID_Coquan_HoiDong_DiaPhuong, 0, iDonVi, 0);

            }
            else
            {
                return "<option value='0'>Chọn đơn vị thẩm quyền</option>";
            }

        }
        public string OptionCoQuanXuLy_TrungUong(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            var list = coquan.Where(x => x.IPARENT == id_parent && x.ICOQUAN != 20 && x.ICOQUAN != 251 && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
            foreach (var donvi in list)
            {
                if (coquan.Where(x => x.IPARENT == (int)donvi.ICOQUAN && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).Count() > 0)
                {
                    str += "<optgroup label='" + Server.HtmlEncode(donvi.CTEN) + "'>";
                    str += OptionCoQuanXuLy_TrungUong(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + Server.HtmlEncode(donvi.CTEN) + "</option>";
                }

            }
            return str;
        }
        public string OptionCoQuanXuLy_DiaPhuong(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            var list = coquan.Where(x => x.IPARENT == id_parent && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
            foreach (var donvi in list)
            {
                if (coquan.Where(x => x.IPARENT == (int)donvi.ICOQUAN && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).Count() > 0)
                {
                    str += "<optgroup label='" + Server.HtmlEncode(donvi.CTEN) + "'>";
                    str += OptionCoQuanXuLy_DiaPhuong(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + Server.HtmlEncode(donvi.CTEN) + "</option>";
                }

            }
            return str;
        }
    }
}
