using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Busineess;
using Entities.Models;
using System.Web.Security;
using KienNghi.App_Code;
using Utilities;
using System.Text.RegularExpressions;
using Spire.Xls;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using Entities.Objects;
using System.Net;
namespace KienNghi.Controllers
{
    public class HomeController : BaseController
    {
        //private Context db = new Context();
        Funtions func = new Funtions();
        Thietlap tl = new Thietlap();
       // KiennghiBusineess _kiennghi = new KiennghiBusineess(); ở đây có cái lấy ni,,mà chạy alf bị lỗi nên e ẩn
        BaseBusineess base_busineess = new BaseBusineess();
        HomeBusineess _home = new HomeBusineess();
        KntcBusineess kn = new KntcBusineess();
        ThietlapBusineess _tl = new ThietlapBusineess();
        Base _base_code = new Base();
        Log log = new Log();
        public ActionResult Ajax_Error_ajax_submit(FormCollection fc)
        {
            ViewData["error"] = fc["error"];
            return PartialView("../Ajax/Error_ajax_submit");
        }
        public ActionResult Index()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                _base_code.Set_Url_keycookie();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex);
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Change_Opt_tinhthanh(int id)
        {
            //Response.Write(form.Option_TinhThanh_ByID_Parent(id, 0));
            string str = "";    
            if (id == -1)
            {
                str = "<select name='iDiaPhuong_1' id='iDiaPhuong_01' disabled class='chosen-select'><option value='0'>Chọn huyện/thành phố/thị xã</option></select>";
            }
            else
            {
                str = "<select name='iDiaPhuong_1' id='iDiaPhuong_01' class='chosen-select'><option value='0'>Chọn huyện/thành phố/thị xã</option>" +
                    tl.Option_TinhThanh_ByID_Parent(id, 0) + "</select>";
            }
            Response.Write(str);
            return null;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Change_Opt_huyenxa(int id)
        {
            //Response.Write(form.Option_TinhThanh_ByID_Parent(id, 0));
            string str = "";
            if (id == 0)
            {
                str = "<select name='iDiaPhuong_2' id='iDiaPhuong_02' class='select' class='chosen-select'><option value='0'>Chọn xã/phường/thị trấn</option></select>";
            }
            else
            {
                
                str = "<select name='iDiaPhuong_2' id='iDiaPhuong_02' class='select' class='chosen-select'><option value='0'>Chọn xã/phường/thị trấn</option>" +
                       tl.Option_TinhThanh_ByID_Parent(id, 0) + "</select>";
            }    
           
            Response.Write(str);
            return null;
        }


        public ActionResult Ajax_Taikhoan_update(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }//controller page
            try
            {
                UserInfor u_info = GetUserInfor();
                string password = Server.HtmlEncode(fc["cPassword"].Trim());
                if (password != "")
                {
                    //password = fc["cPassword"].Trim();
                    if (!_base_code.Check_Sercurity_Pass(password))
                    {
                        Response.Write("Mật khẩu phải có 8 ký tự trở lên, có ký tự viết hoa, chữ số và ký tự đặc biệt!");
                        return null;
                    }
                    //if (password.Length < 8)
                    //{
                    //    Response.Write("Mật khẩu quá ngắn, vui lòng nhập mật khẩu có ít nhất 8 ký tự");
                    //    return null;
                    //}
                    //else
                    //{
                    //    if (!Regex.IsMatch(password, @"[\d]", RegexOptions.ECMAScript)
                    //        || !Regex.IsMatch(password, @"[a-z]", RegexOptions.ECMAScript)
                    //        || !Regex.IsMatch(password, @"[A-Z]", RegexOptions.ECMAScript)
                    //        || !Regex.IsMatch(password, @"[~`!@#$%\^\&\*\(\)\-_\+=\[\{\]\}\|\\;:'\""<\,>\.\?\/£]", RegexOptions.ECMAScript))
                    //    {
                    //        Response.Write("Mật khẩu phải có 8 ký tự trở lên, có ký tự viết hoa, chữ số và ký tự đặc biệt!");
                    //        return null;
                    //    }
                    //}
                }
                USERS u = u_info.user_login;
                u.CEMAIL = Server.HtmlEncode(fc["cEmail"]);
                u.CTEN = Server.HtmlEncode(fc["cTen"]);
                if (password != "")
                {
                    password = HashUtil.Encrypt(password + u.CSALT, Convert.ToBoolean(HashUtil.HashType.SHA512));
                    if (password == u.CPASSWORD)
                    {
                        Response.Write("Vui lòng nhập mật khẩu mới khác mật khẩu cũ!");
                        return null;
                    }
                    u.CPASSWORD = password;
                    u.DLASTCHANGEPASS = DateTime.Now;
                }
                u.IPHONGBAN = Convert.ToInt32(fc["iPhongBan"]);
                u.ICHUCVU = Convert.ToInt32(fc["iChucVu"]);
                u.CSDT = Server.HtmlEncode(fc["cSDT"]);
                u.IUSER = u.IUSER;

                _home.UpdateUser(u);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Update tài khoản");
                throw;
            }

        }
        public ActionResult Taikhoan()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor(); if (u_info == null) { return null; }
                SetTokenAction("taikhoan_update");
                ViewData["taikhoan"] = u_info.user_login;
                ViewData["detail"] = tl.Taikhoan_Detail(u_info.tk_action.iUser);
                ViewData["opt-chucvu"] = tl.Option_ChucVu((int)u_info.user_login.ICHUCVU);
                ViewData["opt-phongban"] = tl.Option_PhongBan_ByDonVi((int)u_info.user_login.IDONVI, (int)u_info.user_login.IPHONGBAN);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thông tin tài khoản");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Delele_file(FormCollection fc)
        {
            int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
            //db.Database.ExecuteSqlCommand("delete from file_upload where ID_File=" + id); 
            // -- > Thảy đổi xóa bằng lớp DAO
            Response.Write(1);
            return null;
        }
        public ActionResult Error()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                ViewData["err"] = "<h3><i class='icon-warning-sign'></i></h3><p> Bạn chưa được cấp quyền để xử lý dữ liệu này!</p><p> Vui lòng liên hệ với Quản trị hệ thống để được thông tin thêm.</p>";
                string type = "";
                if (Request["type"] != null)
                {
                    type = Request["type"].ToString();
                    if (type == "size")
                    {
                        ViewData["err"] = "<h3><i class='icon-warning-sign'></i> Lỗi file đính kèm!</h3><p> Vui lòng chỉ chọn những file có dung lượng < 10M</p>";
                    }
                    if (type == "type")
                    {
                        ViewData["err"] = "<h3><i class='icon-warning-sign'></i> Lỗi file đính kèm!</h3><p> Vui lòng chỉ chọn những file định dạng doc, docx, pdf, jpg, rar, zip</p>";
                    }
                    if (type == "page")
                    {
                        ViewData["err"] = "<h3><i class='icon-warning-sign'></i> Lỗi không tìm thấy trang!</h3>";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Trang lỗi");
                return View("../Home/Error_Exception");
            }

        }
        public Captcha Create_CaptChaCode()
        {
            Captcha capt = new Captcha();
            string captcha_ = Guid.NewGuid().ToString().Substring(0, 6);
            capt.captchaImage = func.GenerateBase64CaptchaImage(400, 100, captcha_);
            capt.captcha = captcha_;
            capt.captcha_encrypt = HashUtil.Encode_ID(captcha_, AppConfig.key);
            return capt;
        }
        public JsonResult Ajax_Change_captcha()
        {
            try
            {
                Captcha c = Create_CaptChaCode();
                return Json(new {
                    captchaImage = c.captchaImage,
                    captcha_encrypt = c.captcha_encrypt
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tạo mới mã captcha");
                throw;
            }
        }

        public ActionResult Login()
        {
            //if (Request.Cookies["ASP.NET_SessionId"] != null)
            //{
            //    func.RemoveCookies("ASP.NET_SessionId");
            //}
            //HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "") { Expires = DateTime.Now.AddYears(-1) };
            //Response.Cookies.Add(cookie2);
            //Session.Abandon();
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            ////Remove_Loginfail();
            //int login_fail = Write_LoginFail();
            //ViewData["login_fail"] = "";
            //if (login_fail >= 5)
            //{
            //    ViewData["login_fail"] = "fail";
            //}
            //return View();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                func.RemoveCookies("ASP.NET_SessionId");
            }
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "") { Expires = DateTime.Now.AddYears(-1) };
            Response.Cookies.Add(cookie2);
            int login_fail = get_LoginFail();
            //Remove_Loginfail();
            ViewData["login_fail"] = "";
            if (login_fail >= 5)
            {
                Captcha c = Create_CaptChaCode();
                ViewData["login_fail"] = "fail";
                ViewData["captchaImage"] = c.captchaImage;
                ViewData["captcha_hidden"] = c.captcha_encrypt;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            try
            {
                string err = "";
                string cUser = Server.HtmlEncode(fc["cUser"]);
                string cPass = Server.HtmlEncode(fc["cPass"].ToString().Trim());
                //string log = fc["log"];
                int login_fail = get_LoginFail();
                if (login_fail >= 5)
                {
                    if (fc["codecapcha"] == null || fc["codecapcha"].ToString().Trim() == "")
                    {
                        Session.Abandon();
                        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                        ViewData["err"] = "<div class='alert alert-danger tcenter'>Vui lòng nhập mã captcha!</div>";
                        log.Log_Login("LOGIN", cUser, 0);
                        ViewData["login_fail"] = "fail";
                        Captcha c = Create_CaptChaCode();
                        ViewData["captcha_hidden"] = c.captcha_encrypt;
                        ViewData["captchaImage"] = c.captchaImage;
                        ViewData["user"] = cUser;
                        ViewData["pass"] = cPass;
                        //Session.Abandon();
                        //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                        return View();
                    }
                    else
                    {
                        string codecapcha = fc["codecapcha"];
                        string captcha_hidden = fc["codecapcha_hidden"];
                        if (captcha_hidden != HashUtil.Encode_ID(codecapcha, AppConfig.key))
                        {
                            Session.Abandon();
                            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                            ViewData["err"] = "<div class='alert alert-danger tcenter'>Mã bảo mật không đúng!</div>";
                            log.Log_Login("LOGIN", cUser, 0);
                            ViewData["login_fail"] = "fail";
                            Captcha c = Create_CaptChaCode();
                            ViewData["captcha_hidden"] = c.captcha_encrypt;
                            ViewData["captchaImage"] = c.captchaImage;
                            ViewData["user"] = cUser;
                            ViewData["pass"] = cPass;
                            return View();
                        }
                    }
                }
                string ip = Request.ServerVariables["REMOTE_ADDR"];
                USERS u = _home.GetByUseName(cUser);
                if (u != null)
                {
                    string pass = HashUtil.Encrypt(cPass + u.CSALT, Convert.ToBoolean(HashUtil.HashType.SHA512));
                    if (u.CPASSWORD == pass)
                    {
                        string encrypt_session = HashUtil.Encrypt(u.IUSER + "|" + u.CSALT, Convert.ToBoolean(HashUtil.HashType.SHA512));
                        func.SetCookies("user_id", encrypt_session);
                        string guid = Guid.NewGuid().ToString();
                        u.CAUTHTOKEN = guid;
                        _home.UpdateUser(u);
                        //System.Web.HttpContext.Current.Session.Add("AuthToken", guid);
                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                        Remove_Loginfail();
                        Tracking((int)u.IUSER, "Đăng nhập hệ thống");
                        SetUserInfor_After_Login(u);
                        log.Log_Login("LOGIN", u.CUSERNAME, 1);
                        func.Set_Url_keycookie();
                        //Response.Redirect("/");
                        Response.Redirect("/Kiennghi/Chuongtrinh/");
                        return null;

                    }
                    else
                    {
                        Write_LoginFail();
                        Session.Abandon();
                        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                        login_fail = get_LoginFail();
                        err = "Sai tên đăng nhập hoặc mật khẩu";
                        ViewData["login_fail"] = "";
                        if (login_fail >= 5)
                        {
                            Captcha c = Create_CaptChaCode();
                            ViewData["login_fail"] = "fail";
                            ViewData["captcha_hidden"] = c.captcha_encrypt;
                            ViewData["captchaImage"] = c.captchaImage;
                        }
                        ViewData["err"] = "<div class='alert alert-danger tcenter'>" + err + "</div>";
                        ViewData["user"] = cUser;
                        ViewData["pass"] = cPass;
                        log.Log_Login("LOGIN", u.CUSERNAME, 0);
                        return View();
                    }
                }
                else
                {
                    Write_LoginFail();
                    Session.Abandon();
                    Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                    login_fail = get_LoginFail();
                    err = "Sai tên đăng nhập hoặc mật khẩu";
                    ViewData["login_fail"] = "";
                    if (login_fail >= 5)
                    {
                        Captcha c = Create_CaptChaCode();
                        ViewData["login_fail"] = "fail";
                        ViewData["captcha_hidden"] = c.captcha_encrypt;
                        ViewData["captchaImage"] = c.captchaImage;
                    }
                    ViewData["err"] = "<div class='alert alert-danger tcenter'>" + err + "</div>";
                    ViewData["user"] = cUser;
                    ViewData["pass"] = cPass;
                    log.Log_Login("LOGIN", cUser, 0);
                    return View();
                }
                //return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Đăng nhập hệ thống");
                return View();
                // throw;
            }

        }
        public int get_LoginFail()
        {
            string ip = Request.ServerVariables["REMOTE_ADDR"];
            return _home.Get_LoginFail(ip);
        }
        public Boolean Write_LoginFail()
        {
            string ip = Request.ServerVariables["REMOTE_ADDR"];
            return _home.Write_LoginFail(ip);
        }
        public Boolean Remove_Loginfail()
        {
            bool result = true;
            try
            {
                string ip = Request.ServerVariables["REMOTE_ADDR"];
                DateTime now = DateTime.Now.AddMinutes(-15);
                result = _home.Remove_Loginfail(ip, now);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Hủy đăng nhập lỗi");
                result = false;
            }
            return result;
        }
        public ActionResult Logout()
        {
            try
            {
                UserInfor u_info = GetUserInfor();
                if (u_info != null && u_info.user_login != null)
                {
                    log.Log_Login("LOGOUT", u_info.user_login.CUSERNAME, 1);
                }
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                Session["SESSION_TOKEN"] = null;

                System.Web.HttpContext.Current.Session.Clear();
                foreach (System.Collections.DictionaryEntry entry in System.Web.HttpContext.Current.Cache)
                {
                    System.Web.HttpContext.Current.Cache.Remove((string)entry.Key);
                }
                FormsAuthentication.SignOut();
                Session.Abandon();
                HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "")
                {
                    Expires = DateTime.Now.AddYears(-1)
                };
                Response.Cookies.Add(cookie1);
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    func.RemoveCookies("ASP.NET_SessionId");
                }
                HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "") { Expires = DateTime.Now.AddYears(-1) };
                Response.Cookies.Add(cookie2);

                func.RemoveCookies("user_id");
                func.RemoveCookies("url_return");
                func.RemoveCookies("url_key");
                func.RemoveCookies("AuthToken");
                func.RemoveCookies("token_action");

                Response.Redirect("/Home/Login");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Đăng xuất");
                Response.Redirect("/Home/Login");
                return null;
            }

        }

        public ActionResult Ajax_Load_UserName()
        {
            UserInfor info = GetUserInfor();
            if (info != null)
            {
                Response.Write(info.user_login.CTEN + " - " + info.tk_action.tenphongban);
            }
            return null;
        }
        public ActionResult Ajax_Load_DonVi_DangNhap()
        {
            UserInfor info = GetUserInfor();
            if (info != null)
            {
                Response.Write(Server.HtmlDecode(info.tk_action.tendonvi).ToUpper());
            }
            return null;
        }
        public string Header_MenuTop(string uri)
        {
            string str = "";
            UserInfor info = GetUserInfor();
            if (info == null) { return ""; }
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            ///kiến nghị
            if (base_busineess.ActionMulty_(action_kiennghi, act))
            {
                string active = ""; if (uri.IndexOf("/Kiennghi/") != -1) { active = "active"; }
                str += "<li class='" + active + "'>" + "<a href='#' data-toggle='dropdown' class='dropdown-toggle menu0'><span> Kiến nghị cử tri </span><span class='caret'></span></a>" +
                    "<ul class='dropdown-menu'>";
                if (info.tk_action.is_lanhdao)
                {
                    str += MenuTop_Kiennghi_Lanhdao(uri);
                }
                if (info.tk_action.is_chuyenvien)
                {
                    str += MenuTop_Kiennghi_Chuyenvien(uri);
                }
                if (info.tk_action.is_dbqh)
                {
                    str += MenuTop_Kiennghi_DBQH(uri);
                }
                str += "</ul></li>";


            }
            //kntc
            if (base_busineess.ActionMulty_(action_kntc, act))
            {
                string active = ""; if (uri.IndexOf("/Kntc/") != -1) { active = "active"; }
                str += "<li class='" + active + "'>" +
                "<a href='#' data-toggle='dropdown' class='dropdown-toggle menu0'><span> Khiếu nại tố cáo </span><span class='caret'></span></a>" +
                "<ul class='dropdown-menu'>";
                if (info.tk_action.is_dbqh)
                {
                    str += MenuTop(0, act, "Thêm mới đơn", "/Kntc/Tiepnhan/");
                    // str += MenuTop(0, act, "Đơn mới cập nhật", "/Kntc/Moicapnhat/");
                    str += MenuTop(0, act, "Đơn chờ xử lý, phân loại", "/Kntc/Choxuly/");
                    str += MenuTop(0, act, "Đủ điều kiện xử lý", " /Kntc/Dudieukien/");
                    str += MenuTop(0, act, "Không đủ điều kiện xử lý", "/Kntc/Khongdudieukien/");
                    // str += MenuTop(0, act, "Đơn không thuộc lĩnh vực phụ trách", "/Kntc/Khongthuocthamquyen/");
                    str += MenuTop(0, act, "Đơn chưa có văn bản trả lời ", "/Kntc/Chuatraloi/");
                    str += MenuTop(0, act, "Đơn đã có văn bản trả lời ", "/Kntc/Datraloi/");
                    str += MenuTop(0, act, "Đơn đã hướng dẫn, trả lời", "/Kntc/Dahuongdan/");
                    str += MenuTop(0, act, "Đơn không xử lý, lưu theo dõi", "/Kntc/Khongxuly/");
                    str += MenuTop(0, act, "Tra cứu", "/Kntc/Tracuu/");
                    str += MenuTop(0, act, "Đơn đang tạm xóa", "/Kntc/Tamxoa/");
                    str += MenuTop(0, act, "Import Đơn", "/Kntc/Import/");
                }
                else
                {
                    if (base_busineess.ActionMulty_("10,44", act))
                    {
                        str += MenuTop(0, act, "Thêm mới đơn", "/Kntc/Tiepnhan/");
                    }
                    if (base_busineess.ActionMulty_("10,11,44", act))
                    {
                        // str += MenuTop(0, act, "Đơn mới cập nhật", "/Kntc/Moicapnhat/");
                    }
                    if (base_busineess.ActionMulty_("12,11,44", act))
                    {
                        str += MenuTop(0, act, "Đơn chờ xử lý, phân loại", "/Kntc/Choxuly/");
                    }
                    if (base_busineess.ActionMulty_("12,11,44", act))
                    {
                        str += MenuTop(0, act, "Đủ điều kiện xử lý", " /Kntc/Dudieukien/");
                        str += MenuTop(0, act, "Không đủ điều kiện xử lý", "/Kntc/Khongdudieukien/");
                        // str += MenuTop(0, act, "Đơn không thuộc lĩnh vực phụ trách", "/Kntc/Khongthuocthamquyen/");
                    }
                    if (base_busineess.ActionMulty_("13,14,15,44,45", act))
                    {
                        //str += MenuTop(0, act, "Đơn thư đến ", "/Kntc/Danhanxuly/");
                    }
                    if (base_busineess.ActionMulty_("13,44", act))
                    {
                        //str += MenuTop(0, act, "Đơn thư đã luân chuyển ", "/Kntc/Daluanchuyen/");
                    }
                    if (base_busineess.ActionMulty_("45,44", act))
                    {
                        str += MenuTop(0, act, "Đơn chưa có văn bản trả lời ", "/Kntc/Chuatraloi/");
                        str += MenuTop(0, act, "Đơn đã có văn bản trả lời ", "/Kntc/Datraloi/");
                        //str += MenuTop(0, act, "Đơn đã chưa có văn bản trả lời ", "/Kntc/Chuatraloi/");
                    }

                    str += MenuTop(0, act, "Đơn đã hướng dẫn, trả lời", "/Kntc/Dahuongdan/");
                    str += MenuTop(0, act, "Đơn không xử lý, lưu theo dõi", "/Kntc/Khongxuly/");

                    if (base_busineess.ActionMulty_("14,15,44", act))
                    {

                        // str += MenuTop(0, act, "Đơn đang xử lý, giải quyết", "/Kntc/Dangxuly/");
                        // str += MenuTop(0, act, "Đơn thuộc thầm quyền, xử lý giải quyết", "/Kntc/Daxuly/");
                        // str += MenuTop(0, act, "Đơn không xử lý, giải quyết", "/Kntc/Khongxuly/");
                    }
                    if (base_busineess.ActionMulty_(action_kntc, act))
                    {
                        str += MenuTop(0, act, "Tra cứu", "/Kntc/Tracuu/");
                        str += MenuTop(0, act, "Đơn đang tạm xóa", "/Kntc/Tamxoa/");
                        str += MenuTop(0, act, "Import Đơn", "/Kntc/Import/");
                    }
                }
                

                str += "</ul></li>";
            }
            //tiếp dân
            if (base_busineess.ActionMulty_(action_tiepdan, act))
            {
                string active = ""; if (uri.IndexOf("/Tiepdan/") != -1) { active = "active"; }
                str += "<li class='" + active + "'>" +
                "<a href='#' data-toggle='dropdown' class='dropdown-toggle menu0'><span> Tiếp công dân </span><span class='caret'></span></a>" +
                "<ul class='dropdown-menu'>";
                str += MenuTop(43, act, "Thêm mới vụ việc tiếp dân", "/Tiepdan/Themmoi/");
                if (base_busineess.ActionMulty_("17", act))
                {
                    str += MenuTop(0, act, "Tiếp định kỳ", "/Tiepdan/Dinhky/");
                }
                if (base_busineess.Action_(53, act))
                {
                    str += MenuTop(0, act, "Tiếp đột xuất", "/Tiepdan/Dotxuat/");
                }
                str += MenuTop(19, act, "Tiếp thường xuyên", "/Tiepdan/Thuongxuyen/");
                str += MenuTop(0, act, "Vụ việc đang tạm xóa", "/Tiepdan/Tamxoa/");
                str += MenuTop(0, act, "Tra cứu", "/Tiepdan/Thuongxuyen_tracuu");

                str += "</ul></li>";
            }

            // văn bản
            if (base_busineess.ActionMulty_("39,40", act))
            {
                string active_baocao = ""; if (uri.IndexOf("/Vanban/") != -1) { active_baocao = "active"; }
                str += "<li class='" + active_baocao + "'>" +
                    "<a href='#' data-toggle='dropdown' class='dropdown-toggle menu0'><span> Văn bản công bố </span><span class='caret'></span></a>" +
                    "<ul class='dropdown-menu'>";
                str += MenuTop(0, act, "Thêm mới văn bản", "/Vanban/Themmoi/");
                str += MenuTop(0, act, "Văn bản đang soạn thảo", "/Vanban/Moicapnhat/");
                str += MenuTop(0, act, "Văn bản đã ban hành", "/Vanban/Duyet/");
                str += MenuTop(0, act, "Văn bản hết hiệu lực", "/Vanban/Quahan/");
                str += MenuTop(0, act, "Tra cứu văn bản", "/Vanban/Tracuu/");
                str += "</ul></li>";
            }

            // báo cáo
            if (base_busineess.ActionMulty_("33,47,48", act))
            {
                string active_baocao = ""; if (uri.IndexOf("/Baocao/") != -1) { active_baocao = "active"; }
                str += "<li class='" + active_baocao + "'>" +
                    "<a href='#' data-toggle='dropdown' class='dropdown-toggle menu0'><span> Báo cáo </span><span class='caret'></span></a>" +
                    "<ul class='dropdown-menu'>";
                if (base_busineess.Action_(33, act))
                {
                    str += MenuTop(0, act, "Kiến nghị cử tri", "/Baocaokiennghi/Phuluc1/");
                }
                if (base_busineess.Action_(47, act))
                {
                    str += MenuTop(0, act, "Khiếu nại tố cáo", "/Baocaokntc/Loaikhieuto/");
                }
                if (base_busineess.Action_(48, act))
                {
                    str += MenuTop(0, act, "Tiếp công dân", "/Baocaotiepdan/Phuluc/");
                }
                str += "</ul></li>";
            }

            //thiets lập hệ thống
            if (base_busineess.ActionMulty_(action_thietlap, act))
            {
                string active = ""; if (uri.IndexOf("/Thietlap/") != -1) { active = "active"; }
                str += "<li class='" + active + "'>" +
                "<a href='#' data-toggle='dropdown' class='dropdown-toggle menu0'><span> Thiết lập hệ thống</span><span class='caret'></span></a>" +
                "<ul class='dropdown-menu'>" +
                MenuTop(21, act, "Khóa", "/Thietlap/Khoa/") +
                MenuTop(42, act, "Kỳ họp", "/Thietlap/Kyhop/") +
                MenuTop(22, act, "Đơn vị hành chính", "/Thietlap/Coquan/") +
                MenuTop(23, act, "HĐND Tỉnh và Đoàn ĐBQH Tỉnh", "/Thietlap/Phongban/") +
                MenuTop(25, act, "Chức vụ", "/Thietlap/Chucvu/") +
                MenuTop(24, act, "Nhóm người dùng", "/Thietlap/Nhomtaikhoan/") +
                MenuTop(25, act, "Người dùng", "/Thietlap/Taikhoan/") +
                MenuTop(25, act, "Lịch sử người dùng", "/Thietlap/Timkiemlichsu/") +
                MenuTop(41, act, "Đại biểu", "/Thietlap/Daibieu/") +
                MenuTop(26, act, "Khiếu nại lĩnh vực", "/Thietlap/Linhvuc/") +
                MenuTop(26, act, "Kiến nghị lĩnh vực ", "/Thietlap/Linhvuc_Coquan/") +
                MenuTop(52, act, "Trả lời phân loại", "/Thietlap/Traloiphanloai/") +
                MenuTop(27, act, "Loại đơn", "/Thietlap/Loaidon") +
                MenuTop(28, act, "Nội dung đơn", "/Thietlap/Noidungdon/") +
                MenuTop(29, act, "Tính chất vụ việc", "/Thietlap/tinhchatdon/") +
                MenuTop(30, act, "Nguồn đơn", "/Thietlap/Nguondon/") +
                MenuTop(32, act, "Nguồn kiến nghị", "/Thietlap/Nguonkiennghi/") +
                MenuTop(31, act, "Địa phương", "/Thietlap/Diaphuong/") +
                MenuTop(34, act, "Nghề nghiệp", "/Thietlap/Nghenghiep/") +
                MenuTop(35, act, "Quốc tịch", "/Thietlap/Quoctich/") +
                MenuTop(36, act, "Dân tộc", "/Thietlap/Dantoc/") +
                MenuTop(37, act, "Loại văn bản", "/Thietlap/Loaivanban/") +

                "</ul></li>";
            }
            return str;
        }

        public ActionResult Ajax_Left_Menu_Phanloai(FormCollection fc)
        {

            string uri = fc["uri"];
            UserInfor info = GetUserInfor();
            if (info == null) { return null; }
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string phanloai = MenuLeft(27, act, "Loại đơn", "/Thietlap/Loaidon/", "icon-tags", uri) +

                            MenuLeft(28, act, "Nội dung đơn", "/Thietlap/Noidungdon/", "icon-align-left", uri) +
                            MenuLeft(29, act, "Tính chất vụ việc", "/Thietlap/Tinhchatdon/", "icon-list-ul", uri) +
                            MenuLeft(30, act, "Nguồn đơn", "/Thietlap/Nguondon/", "icon-exchange", uri) +
                            MenuLeft(32, act, "Nguồn kiến nghị", "/Thietlap/Nguonkiennghi/", "icon-exchange", uri) +
                            MenuLeft(31, act, "Địa phương", "/Thietlap/Diaphuong/", "icon-map-marker", uri) +
                            MenuLeft(34, act, "Nghề nghiệp", "/Thietlap/Nghenghiep/", "icon-briefcase", uri) +
                            MenuLeft(35, act, "Quốc tịch", "/Thietlap/Quoctich/", "icon-globe", uri) +
                            MenuLeft(36, act, "Dân tộc", "/Thietlap/Dantoc/", "icon-group", uri) +
                            MenuLeft(37, act, "Loại văn bản", "/Thietlap/Loaivanban/", "icon-file", uri);

            Response.Write(phanloai);
            return null;
        }

        public string MenuTop_Kiennghi_DBQH(string uri)
        {
            UserInfor info = GetUserInfor();
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string str = MenuTop(2, act, "Chương trình tiếp xúc cử tri", "/Kiennghi/Chuongtrinh/");
            str += MenuTop(3, act, "Thêm kiến mới nghị cử tri", "/Kiennghi/Themmoi/");
            if (base_busineess.ActionMulty_("3,4,5", act))
            {
                str += MenuTop(0, act, "Danh sách kiến nghị cử tri mới cập nhật", "/Kiennghi/Moicapnhat/");
            }
            //str += MenuLeft(0, act, "Tập hợp kiến nghị đoàn ĐBQH", "#", "icon-th-list", uri);
            if (base_busineess.ActionMulty_("5,6", act))
            {
                str += MenuTop(0, act, "Tập hợp chờ xử lý", "/Kiennghi/Tonghop/");
                str += MenuTop(0, act, "Tập hợp đã chuyển Ban Dân nguyện", "/Kiennghi/Tonghop_chuyendiaphuong/");
                str += MenuTop(0, act, "Tập hợp đã chuyển Ủy ban nhân dân tỉnh", "/Kiennghi/Tonghop_chuyendannguyen/");
            }

            if (base_busineess.ActionMulty_("7,8", act))
            {
                str += MenuTop(0, act, "Tập hợp Uỷ ban nhân dân tỉnh chuyển chờ xử lý", "/Kiennghi/Tonghop_bandannguyen_chuyen/");
                str += MenuTop(0, act, "Tập hợp chưa có trả lời", "/Kiennghi/Chuatraloi/");
                str += MenuTop(0, act, "Tập hợp đã có trả lời", "/Kiennghi/Dacotraloi/");
                //str += MenuTop(0, act, "Tập hợp chưa xử lý", "/Kiennghi/Chuaxuly/");
                //str += MenuTop(0, act, "Tập hợp đang giải quyết", "/Kiennghi/Dangxuly/");
                //str += MenuTop(0, act, "Tập hợp đã giải quyết", "/Kiennghi/Traloi/");
                //str += MenuTop(0, act, "Tập hợp giải trình, cung cấp thông tin", "/Kiennghi/Giaitrinh/");
                //str += MenuLeft(0, "Kiến nghị đã có trả lời", "/Kiennghi/Hoanthanh/", "icon-paste", uri);
            }

            //str += MenuTop(0, act, "Kiến nghị thuộc thẩm quyền địa phương giải quyết", "/Kiennghi/Theodoi_kiennghi_diaphuong/");
            //str += MenuTop(0, act, " Kiến nghị chuyển Ban Dân nguyện", "/Kiennghi/Theodoi_kiennghi_dannguyen/");
            str += MenuTop(0, act, " Kiến nghị trùng, lưu theo dõi", "/Kiennghi/Theodoi_luu/");
            str += MenuTop(0, act, "Tra cứu", "/Kiennghi/Tracuu/");
            str += MenuTop(0, act, "Kiến nghị đang tạm xóa", "/Kiennghi/Tamxoa/");
            str += MenuTop(0, act, "Import kiến nghị", "/Kiennghi/Import/");
            return str;
        }
        public string MenuTop_Kiennghi_Chuyenvien(string uri)
        {
            UserInfor info = GetUserInfor();
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string str = MenuTop(2, act, "Kế hoạch tiếp xúc cử tri", "/Kiennghi/Chuongtrinh/");
            str += MenuTop(3, act, "Thêm mới kiến nghị cử tri", "/Kiennghi/Themmoi/");
            if (base_busineess.ActionMulty_("3,4,5", act))
            {
                str += MenuTop(0, act, "Danh sách kiến nghị cử tri mới cập nhật", "/Kiennghi/Moicapnhat/");
            }
            //str += MenuLeft(3, act, "Thêm kiến nghị cử tri", "/Kiennghi/Themmoi/", "icon-plus-sign", uri);
            if (base_busineess.ActionMulty_("3,4,5", act))
            {
                //str += MenuTop(0, act, "Tập hợp kiến nghị", "/Kiennghi/Tonghop_diaphuongchuyen/");
                str += MenuTop(0, act, "Tập hợp kiến nghị thuộc thẩm quyền TW", "/Kiennghi/Tonghop_TW/");
                str += MenuTop(0, act, "Tập hợp kiến nghị thuộc thẩm quyền Tỉnh", "/Kiennghi/Tonghop_Tinh/");
                str += MenuTop(0, act, "Tập hợp kiến nghị thuộc thẩm quyền Huyện", "/Kiennghi/Tonghop_Huyen/");
            }
            //str += MenuTop(0, act, "Tập hợp kiến nghị đã chuyển xử lý", "/Kiennghi/Tonghop_bandannguyen_dachuyen/");
            str += MenuTop(0, act, "Tập hợp chưa có trả lời", "/Kiennghi/Chuatraloi/");
            //str += MenuTop(0, act, "Tập hợp  đang giải quyết", "/Kiennghi/Dangxuly/");
            str += MenuTop(0, act, "Tập hợp  đã có trả lời", "/Kiennghi/Traloi/");

            //str += MenuTop(0, act, "Theo dõi Tập hợp kiến nghị thuộc thẩm quyền địa phương giải quyết", "/Kiennghi/Theodoi_diaphuong/");
            //str += MenuTop(0, act, "Theo dõi Tập hợp kiến nghị đã chuyển Cơ quan Trung ương giải quyết", "/Kiennghi/Theodoi_chuyengiaiquyet/");
            //str += MenuTop(0, act, "Tập hợp kiến nghị đã chuyển giải quyết", "/Kiennghi/Theodoi_chuyengiaiquyet/");
            str += MenuTop(0, act, "Kiến nghị trùng, lưu theo dõi", "/Kiennghi/Theodoi_luu/");
            //str += MenuTop(0, act, "Kiến nghị tồn qua nhiều kỳ họp", "/Kiennghi/Theodoi_kiennghi_chuyenkysau/");
            //str += MenuTop(0, act, "Kiến nghị đã trả lời", "/Kiennghi/Theodoi_kiennghi_tralai/");
            str += MenuTop(0, act, "Kế hoạch giám sát", "/Kiennghi/Giamsat/");
            str += MenuTop(46, act, "Tra cứu", "/Kiennghi/Tracuu/");
            str += MenuTop(0, act, "Kiến nghị đang tạm xóa", "/Kiennghi/Tamxoa/");
            str += MenuTop(0, act, "Import kiến nghị", "/Kiennghi/Import/");

            return str;
        }
        public string MenuTop_Kiennghi_Lanhdao(string uri)
        {
            UserInfor info = GetUserInfor();
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string str = MenuTop(2, act, "Kế hoạch tiếp xúc cử tri", "/Kiennghi/Chuongtrinh/");
            str += MenuTop(3, act, "Thêm mới kiến nghị cử tri", "/Kiennghi/Themmoi/");
            if (base_busineess.ActionMulty_("3,4,5", act))
            {
                str += MenuTop(0, act, "Danh sách kiến nghị cử tri mới cập nhật", "/Kiennghi/Moicapnhat/");
            }
            //str += MenuLeft(3, act, "Thêm kiến nghị cử tri", "/Kiennghi/Themmoi/", "icon-plus-sign", uri);
            if (base_busineess.ActionMulty_("3,4,5", act))
            {
                //str += MenuTop(0, act, "Tập hợp kiến nghị", "/Kiennghi/Tonghop_diaphuongchuyen/");
                str += MenuTop(0, act, "Tập hợp kiến nghị thuộc thẩm quyền TW", "/Kiennghi/Tonghop_TW/");
                str += MenuTop(0, act, "Tập hợp kiến nghị thuộc thẩm quyền Tỉnh", "/Kiennghi/Tonghop_Tinh/");
                str += MenuTop(0, act, "Tập hợp kiến nghị thuộc thẩm quyền Huyện", "/Kiennghi/Tonghop_Huyen/");
            }
            //str += MenuTop(0, act, "Tập hợp kiến nghị đã chuyển xử lý", "/Kiennghi/Tonghop_bandannguyen_dachuyen/");
            str += MenuTop(0, act, "Tập hợp chưa có trả lời", "/Kiennghi/Chuatraloi/");
            str += MenuTop(0, act, "Tập hợp đã có trả lời", "/Kiennghi/Dacotraloi/");
            //str += MenuTop(0, act, "Tập hợp chưa xử lý", "/Kiennghi/Chuaxuly/");
            //str += MenuTop(0, act, "Tập hợp đang giải quyết", "/Kiennghi/Dangxuly/");
            //str += MenuTop(0, act, "Tập hợp đã giải quyết", "/Kiennghi/Traloi/");
            //str += MenuTop(0, act, "Tập hợp giải trình, cung cấp thông tin", "/Kiennghi/Giaitrinh/");


            //str += MenuTop(0, act, "Theo dõi Tập hợp kiến nghị thuộc thẩm quyền địa phương giải quyết", "/Kiennghi/Theodoi_diaphuong/");
            //str += MenuTop(0, act, "Theo dõi Tập hợp kiến nghị đã chuyển Cơ quan Trung ương giải quyết", "/Kiennghi/Theodoi_chuyengiaiquyet/");
            //str += MenuTop(0, act, "Tập hợp kiến nghị đã chuyển giải quyết", "/Kiennghi/Theodoi_chuyengiaiquyet/");
            str += MenuTop(0, act, "Kiến nghị trùng, lưu theo dõi", "/Kiennghi/Theodoi_luu/");
            //str += MenuTop(0, act, "Kiến nghị tồn qua nhiều kỳ họp", "/Kiennghi/Theodoi_kiennghi_chuyenkysau/");
            //str += MenuTop(0, act, "Kiến nghị đã trả lời", "/Kiennghi/Theodoi_kiennghi_tralai/");
            str += MenuTop(0, act, "Kế hoạch giám sát", "/Kiennghi/Giamsat/");
            str += MenuTop(46, act, "Tra cứu", "/Kiennghi/Tracuu/");
            str += MenuTop(0, act, "Kiến nghị đang tạm xóa", "/Kiennghi/Tamxoa/");
            str += MenuTop(0, act, "Import kiến nghị", "/Kiennghi/Import/");

            return str;
        }
        public string Left_Menu_Kiennghi_DBQH(string uri)
        {
            UserInfor info = GetUserInfor();
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string str = MenuLeft(2, act, "Chương trình tiếp xúc cử tri", "/Kiennghi/Chuongtrinh/", "icon-list", uri);
            str += MenuLeft(3, act, "Thêm mới kiến nghị cử tri", "/Kiennghi/Themmoi/", "icon-plus-sign", uri, "Thêm mới kiến nghị cử tri");
            if (base_busineess.ActionMulty_("3,4,5", act))
            {
                str += MenuLeft(0, act, "Danh sách kiến nghị cử tri mới cập nhật", "/Kiennghi/Moicapnhat/", "icon-list", uri, "Danh sách kiến nghị cử tri mới cập nhật");
            }

            if (base_busineess.ActionMulty_("5,6", act))
            {
                str += "<li title='' ><a class=''  href='#'><i class='icon-list-alt'></i> <span>Tập hợp kiến nghị</span></a><ul class='subnav_1'>";
                str += MenuLeft(0, act, "Chờ xử lý", "/Kiennghi/Tonghop/", "icon-tags", uri);
                str += MenuLeft(0, act, "Đã chuyển Ban Dân nguyện", "/Kiennghi/Tonghop_chuyendiaphuong/", "icon-signin", uri);
                str += MenuLeft(0, act, "Đã chuyển Ủy ban nhân dân tỉnh", "/Kiennghi/Tonghop_chuyendannguyen/", "icon-signin", uri);
                str += "</ul></li>";
            }

            if (base_busineess.ActionMulty_("7,8", act))
            {
                str += "<li title='' ><a class=''  href='#'><i class='icon-list-alt'></i> <span>Tập hợp kiến nghị thuộc thẩm quyền xử lý</span></a><ul class='subnav_1'>";
                str += MenuLeft(0, act, "Tập hợp Uỷ ban nhân dân tỉnh chuyển chờ xử lý", "/Kiennghi/Tonghop_bandannguyen_chuyen/", "icon-signin", uri, "Tập hợp kiến nghị Uỷ ban nhân dân tỉnh chuyển xử lý");
                str += MenuLeft(0, act, "Tập hợp chưa có trả lời", "/Kiennghi/Chuatraloi/", "icon-reorder", uri, "Tập hợp chưa có trả lời");
                str += "<li title ='Tập hợp đã có trả lời'><a href='/Kiennghi/Dacotraloi'><i class='icon-signout'></i> <span>Tập hợp đã có trả lời</span></a><ul class='subnav_1'>";
                str += MenuLeft(0, act, "Tập hợp chưa giải quyết", "/Kiennghi/Chuaxuly/", "icon-reorder", uri, "Tập hợp chưa giải quyết");
                str += MenuLeft(0, act, "Tập hợp đang giải quyết", "/Kiennghi/Dangxuly/", "icon-reorder", uri, "Tập hợp đang giải quyết");
                str += MenuLeft(0, act, "Tập hợp đã giải quyết", "/Kiennghi/Traloi/", "icon-reorder", uri, "Tập hợp đã giải quyết");
                str += MenuLeft(0, act, "Tập hợp giải trình, cung cấp thông tin", "/Kiennghi/Giaitrinh/", "icon-reorder", uri, "Tập hợp giải trình, cung cấp thông tin");
                str += "</ul></li>";
                str += "</ul></li>";
                //str += MenuLeft(0, "Kiến nghị đã có trả lời", "/Kiennghi/Hoanthanh/", "icon-paste", uri);
            }
            //str += MenuLeft(0, act, "Theo dõi kết quả xử lý kiến nghị", "#", "icon-signal", uri);
            str += "<li><a href='#'><i class='icon-signal'></i> <span>Theo dõi kết quả xử lý kiến nghị (Xử lý tại địa phương)</span></a><ul class='subnav_1'>";
            //str += MenuLeft(0, act, " Kiến nghị thuộc thẩm quyền địa phương giải quyết", "/Kiennghi/Theodoi_kiennghi_diaphuong/", "icon-sitemap", uri, "Kiến nghị thuộc thẩm quyền địa phương giải quyết");
            //str += MenuLeft(0, act, " Kiến nghị chuyển Ban Dân nguyện", "/Kiennghi/Theodoi_kiennghi_dannguyen/", "icon-hospital", uri, "Kiến nghị chuyển Ban Dân nguyện");
            str += MenuLeft(0, act, " Kiến nghị trùng, lưu theo dõi", "/Kiennghi/Theodoi_luu/", "icon-copy", uri, "Kiến nghị trùng, lưu theo dõi");
            //str += MenuLeft(0, act, " Kiến nghị tồn qua nhiều kỳ họp", "/Kiennghi/Theodoi_kiennghi_chuyenkysau/", "icon-table", uri, "Kiến nghị tồn qua nhiều kỳ họp");
            //str += MenuLeft(0, act, " Kiến nghị đã trả lời", "/Kiennghi/Theodoi_kiennghi_tralai/", "icon-circle-arrow-left", uri, "Kiến nghị đã trả lời");
            str += "</ul></li>";
            //str += MenuTop(0, act, "Kiến nghị trả lại", "/Kiennghi/Theodoi_kiennghi_tralai/");
            str += MenuLeft(0, act, "Tra cứu", "/Kiennghi/Tracuu/", "icon-search", uri);
            str += MenuLeft(0, act, "Kiến nghị đang tạm xóa", "/Kiennghi/Tamxoa/", "icon-trash", uri);
            str += MenuLeft(0, act, "Import kiến nghị", "/Kiennghi/Import/", "icon-save", uri);
            return str;
        }
        public string Left_Menu_Kiennghi_Lanhdao(string uri)
        {
            UserInfor info = GetUserInfor();
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string str = MenuLeft(2, act, "Kế hoạch tiếp xúc cử tri", "/Kiennghi/Chuongtrinh/", "icon-list", uri);
            //str += MenuLeft(3, act, "Thêm kiến nghị cử tri", "/Kiennghi/Themmoi/", "icon-plus-sign", uri);
            if (base_busineess.ActionMulty_("3,4,5", act))
            {
                str += MenuLeft(3, act, "Thêm mới kiến nghị cử tri", "/Kiennghi/Themmoi/", "icon-plus-sign", uri, "Thêm mới kiến nghị cử tri");
                str += MenuLeft(0, act, "Danh sách kiến nghị cử tri mới cập nhật", "/Kiennghi/Moicapnhat/", "icon-list", uri, "Danh sách kiến nghị cử tri mới cập nhật");
            }
            if (base_busineess.ActionMulty_("3,4,5", act))
            {
                str += "<li title ='Tập hợp kiến nghị'><a href='#'><i class='icon-signout'></i> <span>Tập hợp kiến nghị</span></a><ul class='subnav_1'>";
                //str += MenuLeft(0, act, "Tập hợp kiến nghị", "/Kiennghi/Tonghop_diaphuongchuyen/", "icon-signout", uri, "Tập hợp kiến nghị");
                str += MenuLeft(0, act, "Tập hợp kiến nghị thuộc thẩm quyền TW", "/Kiennghi/Tonghop_TW/", "icon-list-alt", uri, "Tập hợp kiến nghị thuộc thẩm quyền TW");
                str += MenuLeft(0, act, "Tập hợp kiến nghị thuộc thẩm quyền Tỉnh", "/Kiennghi/Tonghop_Tinh/", "icon-list-alt", uri, "Tập hợp kiến nghị thuộc thẩm quyền Tỉnh");
                str += MenuLeft(0, act, "Tập hợp kiến nghị thuộc thẩm quyền Huyện", "/Kiennghi/Tonghop_Huyen/", "icon-list-alt", uri, "Tập hợp kiến nghị thuộc thẩm quyền Huyện");
                str += "</ul></li>";
            }
            //str += MenuLeft(0, act, "Tập hợp kiến nghị đã chuyển xử lý", "/Kiennghi/Tonghop_bandannguyen_dachuyen/", "icon-reorder", uri, "Tập hợp kiến nghị đã chuyển xử lý");
            str += "<li title ='Tập hợp kiến nghị đã chuyển cơ quan thẩm quyền'><a href='#'><i class='icon-signout'></i> <span>Tập hợp kiến nghị đã chuyển cơ quan thẩm quyền</span></a><ul class='subnav_1'>";
            str += MenuLeft(0, act, "Tập hợp chưa có trả lời", "/Kiennghi/Chuatraloi/", "icon-reorder", uri, "Tập hợp chưa có trả lời");
            str += "<li title ='Tập hợp đã có trả lời'><a href='/Kiennghi/Dacotraloi'><i class='icon-signout'></i> <span>Tập hợp đã có trả lời</span></a><ul class='subnav_1'>";
            str += MenuLeft(0, act, "Tập hợp chưa giải quyết", "/Kiennghi/Chuaxuly/", "icon-reorder", uri, "Tập hợp chưa giải quyết");
            str += MenuLeft(0, act, "Tập hợp đang giải quyết", "/Kiennghi/Dangxuly/", "icon-reorder", uri, "Tập hợp đang giải quyết");
            str += MenuLeft(0, act, "Tập hợp đã giải quyết", "/Kiennghi/Traloi/", "icon-reorder", uri, "Tập hợp đã giải quyết");
            str += MenuLeft(0, act, "Tập hợp giải trình, cung cấp thông tin", "/Kiennghi/Giaitrinh/", "icon-reorder", uri, "Tập hợp giải trình, cung cấp thông tin");
            str += "</ul></li>";
            str += "</ul></li>";
            str += "<li title ='Theo dõi kết quả xử lý kiến nghị'><a href='#'><i class='icon-signal'></i> <span>Theo dõi kết quả xử lý kiến nghị</span></a><ul class='subnav_1'>";
            //str += MenuLeft(0, act, " Tập hợp kiến nghị thuộc thẩm quyền địa phương giải quyết", "/Kiennghi/Theodoi_diaphuong/", "icon-sitemap", uri, "Tập hợp kiến nghị thuộc thẩm quyền địa phương giải quyết");
            //str += MenuLeft(0, act, " Tập hợp kiến nghị đã chuyển Cơ quan Trung ương giải quyết", "/Kiennghi/Theodoi_chuyengiaiquyet/", "icon-signin", uri, "Tập hợp kiến nghị đã chuyển Cơ quan Trung ương giải quyết");
            //str += MenuLeft(0, act, " Tập hợp kiến nghị đã chuyển giải quyết", "/Kiennghi/Theodoi_chuyengiaiquyet/", "icon-signin", uri, "Tập hợp kiến nghị đã chuyển giải quyết");
            str += MenuLeft(0, act, " Kiến nghị trùng, lưu theo dõi", "/Kiennghi/Theodoi_luu/", "icon-copy", uri, "Kiến nghị trùng, lưu theo dõi");
            //str += MenuLeft(0, act, " Kiến nghị tồn qua nhiều kỳ họp", "/Kiennghi/Theodoi_kiennghi_chuyenkysau/", "icon-table", uri, "Kiến nghị tồn qua nhiều kỳ họp");
            //str += MenuLeft(0, act, " Kiến nghị đã trả lời", "/Kiennghi/Theodoi_kiennghi_tralai/", "icon-circle-arrow-left", uri, "Kiến nghị đã trả lời");
            str += "</ul></li>";
            str += MenuLeft(46, act, "Kế hoạch giám sát", "/Kiennghi/Giamsat/", "icon-group", uri);
            str += MenuLeft(0, act, "Tra cứu", "/Kiennghi/Tracuu/", "icon-search", uri);
            str += MenuLeft(0, act, "Kiến nghị đang tạm xóa", "/Kiennghi/Tamxoa/", "icon-trash", uri);
            str += MenuLeft(0, act, "Import kiến nghị", "/Kiennghi/Import/", "icon-save", uri);
            return str;
        }
        public string Left_Menu_Kiennghi_Chuyenvien(string uri)
        {
            UserInfor info = GetUserInfor();
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string str = MenuLeft(2, act, "Kế hoạch tiếp xúc cử tri", "/Kiennghi/Chuongtrinh/", "icon-list", uri);
            //str += MenuLeft(3, act, "Thêm kiến nghị cử tri", "/Kiennghi/Themmoi/", "icon-plus-sign", uri);
            if (base_busineess.ActionMulty_("3,4,5", act))
            {
                str += MenuLeft(3, act, "Thêm mới kiến nghị cử tri", "/Kiennghi/Themmoi/", "icon-plus-sign", uri, "Thêm mới kiến nghị cử tri");
                str += MenuLeft(0, act, "Danh sách kiến nghị cử tri mới cập nhật", "/Kiennghi/Moicapnhat/", "icon-list", uri, "Danh sách kiến nghị cử tri mới cập nhật");
            }
            if (base_busineess.ActionMulty_("3,4,5", act))
            {
                str += "<li title ='Tập hợp kiến nghị'><a href='#'><i class='icon-signout'></i> <span>Tập hợp kiến nghị</span></a><ul class='subnav_1'>";
                //str += MenuLeft(0, act, "Tập hợp kiến nghị", "/Kiennghi/Tonghop_diaphuongchuyen/", "icon-signout", uri, "Tập hợp kiến nghị");
                str += MenuLeft(0, act, "Tập hợp kiến nghị thuộc thẩm quyền TW", "/Kiennghi/Tonghop_TW/", "icon-list-alt", uri, "Tập hợp kiến nghị thuộc thẩm quyền TW");
                str += MenuLeft(0, act, "Tập hợp kiến nghị thuộc thẩm quyền Tỉnh", "/Kiennghi/Tonghop_Tinh/", "icon-list-alt", uri, "Tập hợp kiến nghị thuộc thẩm quyền Tỉnh");
                str += MenuLeft(0, act, "Tập hợp kiến nghị thuộc thẩm quyền Huyện", "/Kiennghi/Tonghop_Huyen/", "icon-list-alt", uri, "Tập hợp kiến nghị thuộc thẩm quyền Huyện");
                str += "</ul></li>";
            }
            //str += MenuLeft(0, act, "Tập hợp kiến nghị đã chuyển xử lý", "/Kiennghi/Tonghop_bandannguyen_dachuyen/", "icon-reorder", uri, "Tập hợp kiến nghị đã chuyển xử lý");
            str += "<li title ='Tập hợp kiến nghị đã chuyển cơ quan thẩm quyền'><a href='#'><i class='icon-signout'></i> <span>Tập hợp kiến nghị đã chuyển cơ quan thẩm quyền</span></a><ul class='subnav_1'>";
            str += MenuLeft(0, act, "Tập hợp chưa có trả lời", "/Kiennghi/Chuatraloi/", "icon-reorder", uri, "Tập hợp chưa có trả lời");
            str += "<li title ='Tập hợp đã có trả lời'><a href='/Kiennghi/Dacotraloi'><i class='icon-signout'></i> <span>Tập hợp đã có trả lời</span></a><ul class='subnav_1'>";
            str += MenuLeft(0, act, "Tập hợp chưa giải quyết", "/Kiennghi/Chuaxuly/", "icon-reorder", uri, "Tập hợp chưa giải quyết");
            str += MenuLeft(0, act, "Tập hợp đang giải quyết", "/Kiennghi/Dangxuly/", "icon-reorder", uri, "Tập hợp đang giải quyết");
            str += MenuLeft(0, act, "Tập hợp đã giải quyết", "/Kiennghi/Traloi/", "icon-reorder", uri, "Tập hợp đã giải quyết");
            str += MenuLeft(0, act, "Tập hợp giải trình, cung cấp thông tin", "/Kiennghi/Giaitrinh/", "icon-reorder", uri, "Tập hợp giải trình, cung cấp thông tin");
            str += "</ul></li>";
            str += "</ul></li>";
            str += "<li title ='Theo dõi kết quả xử lý kiến nghị'><a href='#'><i class='icon-signal'></i> <span>Theo dõi kết quả xử lý kiến nghị</span></a><ul class='subnav_1'>";
            //str += MenuLeft(0, act, " Tập hợp kiến nghị thuộc thẩm quyền địa phương giải quyết", "/Kiennghi/Theodoi_diaphuong/", "icon-sitemap", uri, "Tập hợp kiến nghị thuộc thẩm quyền địa phương giải quyết");
            //str += MenuLeft(0, act, " Tập hợp kiến nghị đã chuyển Cơ quan Trung ương giải quyết", "/Kiennghi/Theodoi_chuyengiaiquyet/", "icon-signin", uri, "Tập hợp kiến nghị đã chuyển Cơ quan Trung ương giải quyết");
            //str += MenuLeft(0, act, " Tập hợp kiến nghị đã chuyển giải quyết", "/Kiennghi/Theodoi_chuyengiaiquyet/", "icon-signin", uri, "Tập hợp kiến nghị đã chuyển giải quyết");
            str += MenuLeft(0, act, " Kiến nghị trùng, lưu theo dõi", "/Kiennghi/Theodoi_luu/", "icon-copy", uri, "Kiến nghị trùng, lưu theo dõi");
            //str += MenuLeft(0, act, " Kiến nghị tồn qua nhiều kỳ họp", "/Kiennghi/Theodoi_kiennghi_chuyenkysau/", "icon-table", uri, "Kiến nghị tồn qua nhiều kỳ họp");
            //str += MenuLeft(0, act, " Kiến nghị đã trả lời", "/Kiennghi/Theodoi_kiennghi_tralai/", "icon-circle-arrow-left", uri, "Kiến nghị đã trả lời");
            str += "</ul></li>";
            str += MenuLeft(46, act, "Kế hoạch giám sát", "/Kiennghi/Giamsat/", "icon-group", uri);
            str += MenuLeft(0, act, "Tra cứu", "/Kiennghi/Tracuu/", "icon-search", uri);
            str += MenuLeft(0, act, "Kiến nghị đang tạm xóa", "/Kiennghi/Tamxoa/", "icon-trash", uri);
            str += MenuLeft(0, act, "Import kiến nghị", "/Kiennghi/Import/", "icon-save", uri);
            return str;
        }

        public ActionResult Ajax_Left_Menu_Kiennghi(FormCollection fc)
        {
            UserInfor info = GetUserInfor();
            if (info == null) { return null; }
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string uri = fc["uri"];
            if (act.is_lanhdao)
            {
                Response.Write(Left_Menu_Kiennghi_Lanhdao(uri));
            }
            if (act.is_chuyenvien)
            {
                Response.Write(Left_Menu_Kiennghi_Chuyenvien(uri));
            }
            if (act.is_dbqh)
            {
                Response.Write(Left_Menu_Kiennghi_DBQH(uri));
            }
            return null;
        }
        
        public ActionResult Ajax_Left_Menu_Kntc(FormCollection fc)
        {

            string uri = fc["uri"];
            UserInfor info = GetUserInfor();
            if (info == null) { return null; }
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string str = "";
            if (act.is_dbqh)
            {
                str += MenuLeft(0, act, "Thêm mới đơn", "/Kntc/Tiepnhan/", "icon-plus-sign", uri);
                // str += MenuLeft(0, act, "Đơn mới cập nhật", "/Kntc/Moicapnhat/", "icon-file-alt", uri);
                str += MenuLeft(0, act, "Đơn chờ xử lý, phân loại", "/Kntc/Choxuly/", "icon-time", uri);
                str += "<li><a href='#'><i class='icon-external-link'></i> <span>Xử lý đơn thư</span></a><ul class='subnav_1'>";
                str += MenuLeft(0, act, "Đủ điều kiện xử lý", "/Kntc/Dudieukien/", "icon-ok", uri);
                str += MenuLeft(0, act, "Không đủ điều kiện xử lý", "/Kntc/Khongdudieukien/", "icon-remove", uri);
                str += "</ul></li>";
                // str += MenuLeft(0, act, "Đơn không thuộc lĩnh vực phụ trách", "/Kntc/Khongthuocthamquyen/", "icon-warning-sign", uri);
                str += "<li><a href='#'><i class='icon-external-link'></i> <span>Đơn đã chuyển cơ quan thẩm quyền</span></a><ul class='subnav_1'>";
                str += MenuLeft(0, act, "Đơn chưa có văn bản trả lời ", "/Kntc/Chuatraloi/", " icon-minus-sign", uri);
                str += MenuLeft(0, act, "Đơn đã có văn bản trả lời ", "/Kntc/Datraloi/", " icon-ok-circle", uri);
                str += "</ul></li>";
                str += MenuLeft(0, act, "Đơn đã hướng dẫn, trả lời", "/Kntc/Dahuongdan/", "icon-ok", uri);
                str += MenuLeft(0, act, "Đơn không xử lý, lưu theo dõi", "/Kntc/Khongxuly/", "icon-bell", uri);
                str += MenuLeft(0, act, "Tra cứu", "/Kntc/Tracuu/", "icon-search", uri);
                str += MenuLeft(0, act, "Đơn đang tạm xóa", "/Kntc/Tamxoa/", "icon-trash", uri);
                str += MenuLeft(0, act, "Import Đơn", "/Kntc/Import/", "icon-save", uri);
            }
            else
            {
                if (base_busineess.ActionMulty_("10,44", act))
                {
                    str += MenuLeft(0, act, "Thêm mới đơn", "/Kntc/Tiepnhan/", "icon-plus-sign", uri);
                }
                if (base_busineess.ActionMulty_("10,11,44", act))
                {
                    // str += MenuLeft(0, act, "Đơn mới cập nhật", "/Kntc/Moicapnhat/", "icon-file-alt", uri);
                }
                if (base_busineess.ActionMulty_("12,11,44", act))
                {
                    str += MenuLeft(0, act, "Đơn chờ xử lý, phân loại", "/Kntc/Choxuly/", "icon-time", uri);
                }

                if (base_busineess.ActionMulty_("12,11,44", act))
                {
                    str += "<li><a href='#'><i class='icon-external-link'></i> <span>Xử lý đơn thư</span></a><ul class='subnav_1'>";
                    str += MenuLeft(0, act, "Đủ điều kiện xử lý", "/Kntc/Dudieukien/", "icon-ok", uri);
                    str += MenuLeft(0, act, "Không đủ điều kiện xử lý", "/Kntc/Khongdudieukien/", "icon-remove", uri);
                    str += "</ul></li>";
                    // str += MenuLeft(0, act, "Đơn không thuộc lĩnh vực phụ trách", "/Kntc/Khongthuocthamquyen/", "icon-warning-sign", uri);
                }
                if (base_busineess.ActionMulty_("13,14,15,44,45", act))
                {
                    //str += MenuLeft(0, act, "Đơn thư đến ", "/Kntc/Danhanxuly/", " icon-share-alt", uri);
                }
                if (base_busineess.ActionMulty_("13,44", act))
                {
                    //str += MenuLeft(0, act, "Đơn thư đã luân chuyển ", "/Kntc/Daluanchuyen/", "icon-reply", uri);
                }
                if (base_busineess.ActionMulty_("45,44", act))
                {
                    str += "<li><a href='#'><i class='icon-external-link'></i> <span>Đơn đã chuyển cơ quan thẩm quyền</span></a><ul class='subnav_1'>";
                    str += MenuLeft(0, act, "Đơn chưa có văn bản trả lời ", "/Kntc/Chuatraloi/", " icon-minus-sign", uri);
                    str += MenuLeft(0, act, "Đơn đã có văn bản trả lời ", "/Kntc/Datraloi/", " icon-ok-circle", uri);
                    str += "</ul></li>";
                }

                str += MenuLeft(0, act, "Đơn đã hướng dẫn, trả lời", "/Kntc/Dahuongdan/", "icon-ok", uri);

                if (base_busineess.ActionMulty_("14,15,44", act))
                {
                    //str += "<li><a href='#'><i class='icon-signal'></i> <span>Đơn đã có văn bản trả lời</span></a><ul class='subnav_1'></ul></li>";
                    // str += "<li><a href='#'><i class='icon-check'></i> <span>Đơn thuộc thẩm quyền xử lý, giải quyết</span></a><ul class='subnav_1'>";
                    //str += MenuLeft(0, act, "Đơn đang xử lý, giải quyết", "/Kntc/Dangxuly/", "icon-spinner", uri);
                    //str += MenuLeft(0, act, "Đơn đã xử lý, giải quyết", "/Kntc/Daxuly/", "icon-ok", uri);
                    //str += "</ul></li>";
                    str += MenuLeft(0, act, "Đơn không xử lý, lưu theo dõi", "/Kntc/Khongxuly/", "icon-bell", uri);
                }
                if (base_busineess.ActionMulty_(action_kntc, act))
                {
                    str += MenuLeft(0, act, "Tra cứu", "/Kntc/Tracuu/", "icon-search", uri);
                    str += MenuLeft(0, act, "Đơn đang tạm xóa", "/Kntc/Tamxoa/", "icon-trash", uri);
                    str += MenuLeft(0, act, "Import Đơn", "/Kntc/Import/", "icon-save", uri);
                }
            }
            
            Response.Write(str);
            return null;
        }
        public ActionResult Ajax_Left_Menu_Baocao(FormCollection fc)
        {

            string uri = fc["uri"];
            UserInfor info = GetUserInfor();
            if (info == null) { return null; }
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string str = "";
            if (base_busineess.Action_(33, act))
            {
                if (uri.IndexOf("/Baocaokiennghi/") != -1)
                {
                    str += MenuLeft(0, act, "Bảng tổng hợp kết quả giải quyết, trả lời kiến nghị của cử tri", "/Baocaokiennghi/Phuluc1/", "icon-table", uri);
                    str += MenuLeft(0, act, "Văn bản pháp luật đã ban hành có nội dung liên quan tới việc tiếp thu, giải quyết", "/Baocaokiennghi/Phuluc2/", "icon-table", uri);
                    str += MenuLeft(0, act, "Văn bản cử tri kiến nghị sửa đổi (đang được xem xét để sửa đổi, bổ sung)", "/Baocaokiennghi/Phuluc3/", "icon-table", uri);
                    str += MenuLeft(0, act, "Kết quả giải quyết đối với kiến nghị tồn đọng qua nhiều kỳ họp", "/Baocaokiennghi/Phuluc4/", "icon-table", uri);
                    str += MenuLeft(0, act, "Danh mục kiến nghị tồn đọng qua nhiều kỳ họp đang trong quá trình giải quyết", "/Baocaokiennghi/Phuluc5/", "icon-table", uri);
                    str += MenuLeft(0, act, "Danh mục kiến nghị cử tri gửi tới đang trong quá trình giải quyết", "/Baocaokiennghi/Phuluc5b/", "icon-table", uri);
                    str += MenuLeft(0, act, "Chuyên đề giám sát", "/Baocaokiennghi/Phuluc6/", "icon-table", uri);
                    str += MenuLeft(0, act, "Bản thống kê thời gian nhận được báo cáo tổng hợp ý kiến, kiến nghị của cử tri", "/Baocaokiennghi/Phuluc9/", "icon-table", uri);
                    str += MenuLeft(0, act, "Mẫu phân loại sơ bộ", "/Baocaokiennghi/Phanloai/", "icon-table", uri);
                    str += MenuLeft(0, act, "Bản tập hợp trả lời kiến nghị cử tri", "/Baocaokiennghi/Traloi/", "icon-table", uri);
                    str += MenuLeft(0, act, "Tổng hợp trả lời KNCT của Bộ Ngành", "/Baocaokiennghi/TraloiKNTC/", "icon-table", uri);
                    str += MenuLeft(0, act, "Báo cáo kết quả hoạt động của tổ đại biểu", "/Baocaokiennghi/Ketqua/", "icon-table", uri);
                    str += MenuLeft(0, act, "Tổng hợp kiến nghị cử tri gửi đến các đoàn đại biểu", "/Baocaokiennghi/TraloiKN_DenDBQH/", "icon-table", uri);
                }
            }
            if (base_busineess.Action_(47, act))
            {
                if (uri.IndexOf("/Baocaokntc/") != -1)
                {
                    str += MenuLeft(0, act, "Thống kê loại khiếu tố", "/Baocaokntc/Loaikhieuto/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Thống kê cơ quan thẩm quyền", "/Baocaokntc/Coquanthamquyen/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Thống kê nơi gửi đơn", "/Baocaokntc/Noiguidon/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Thống kê người nhập đơn", "/Baocaokntc/Nguoinhapdon/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Thống kê người xử lý đơn", "/Baocaokntc/Nguoixuly/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Thống kế đơn theo nguồn đơn", "/Baocaokntc/Coquanchuyendon/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Thống kê trùng đơn", "/Baocaokntc/Trungdon/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Thống kê tổng số đơn", "/Baocaokntc/Tongsodon/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Thống kê chi tiết đơn", "/Baocaokntc/Chitietdon/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Thống kê theo địa bàn", "/Baocaokntc/Theodiaban/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Thống kê số liệu đơn", "/Baocaokntc/Solieudon/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Công văn chuyển đơn", "/Baocaokntc/Congvanchuyendon/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Tổng hợp theo dõi giải quyết đơn", "/Baocaokntc/Theodoigiaiquyetdon/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Báo cáo đơn thư hàng tuần", "/Baocaokntc/Baocaodonthuhangtuan/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Báo cáo đơn thư hàng tháng", "/Baocaokntc/Baocaothang/", "icon-bar-chart", uri);
                }
            }
            if (base_busineess.Action_(48, act))
            {
                if (uri.IndexOf("/Baocaotiepdan/") != -1)
                {
                    str += MenuLeft(0, act, "Phụ lục tiếp công dân", "/Baocaotiepdan/Phuluc/", "icon-bar-chart", uri);
                    str += MenuLeft(0, act, "Số liệu tiếp công dân", "/Baocaotiepdan/Solieu/", "icon-bar-chart", uri);
                }
            }
            Response.Write(str);
            return null;
        }
        public ActionResult Ajax_Left_Menu_Tiepdan(FormCollection fc)
        {

            string uri = fc["uri"];
            string str = "";
            UserInfor info = GetUserInfor();
            if (info == null) { return null; }
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            str += MenuLeft(43, act, "Thêm mới vụ việc tiếp dân", "/Tiepdan/Themmoi/", "icon-plus-sign", uri);
            if (base_busineess.ActionMulty_("17", act))
            {
                str += MenuLeft(0, act, "Tiếp định kỳ", "/Tiepdan/Dinhky/", "icon-file-alt", uri);
                //str += MenuLeft(0, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i class='icon-search'></i> Tra cứu", "/Tiepdan/Dinhky_tracuu/", "", uri);

            }
            if (base_busineess.Action_(53, act))
            {
                str += MenuLeft(53, act, "Tiếp đột xuất", "/Tiepdan/Dotxuat/", "icon-file-alt", uri);
            }
            if (base_busineess.Action_(19, act))
            {
                str += MenuLeft(19, act, "Tiếp thường xuyên", "/Tiepdan/Thuongxuyen/", "icon-file-alt", uri);
                str += MenuLeft(0, act, "Tra cứu", "/Tiepdan/Thuongxuyen_tracuu/", "icon-search", uri);
                str += MenuLeft(0, act, "Vụ việc đang tạm xóa", "/Tiepdan/Tamxoa/", "icon-trash", uri);
            }

            Response.Write(str);
            return null;
        }
        public ActionResult Ajax_Left_Menu_Vanban(FormCollection fc)
        {

            string uri = fc["uri"];
            UserInfor info = GetUserInfor();
            if (info == null) { return null; }
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            string str = MenuLeft(39, act, "Thêm mới văn bản", "/Vanban/Themmoi/", "icon-plus-sign", uri);
            if (base_busineess.ActionMulty_("39,40", act))
            {
                str += MenuLeft(0, act, "Văn bản đang soạn thảo", "/Vanban/Moicapnhat/", "icon-file-alt", uri);
                str += MenuLeft(0, act, "Văn bản đã ban hành", "/Vanban/Duyet/", "icon-ok-sign", uri);
                str += MenuLeft(0, act, "Văn bản hết hiệu lực", "/Vanban/Quahan/", "icon-warning-sign", uri);
            }
            str += MenuLeft(0, act, "Tra cứu văn bản", "/Vanban/Tracuu/", "icon-search", uri);
            Response.Write(str);
            return null;
        }
        public ActionResult Ajax_Left_Menu_Hethong(FormCollection fc)
        {

            string hethong = "";
            string uri = fc["uri"];
            UserInfor info = GetUserInfor();
            if (info == null) { return null; }
            int iuser = (int)info.user_login.IUSER;
            TaikhoanAtion act = info.tk_action;
            hethong = MenuLeft(21, act, "Khóa", "/Thietlap/Khoa/", "icon-th-large", uri) +
                        MenuLeft(42, act, "Kỳ họp", "/Thietlap/Kyhop/", "icon-table", uri) +
                        MenuLeft(22, act, "Đơn vị hành chính", "/Thietlap/Coquan/", "icon-h-sign", uri) +
                        MenuLeft(23, act, "HĐND Tỉnh và Đoàn ĐBQH Tỉnh", "/Thietlap/Phongban/", "icon-sitemap", uri) +
                        MenuLeft(25, act, "Chức vụ", "/Thietlap/Chucvu/", "icon-sitemap", uri) +
                        MenuLeft(24, act, "Nhóm người dùng", "/Thietlap/Nhomtaikhoan/", "icon-th", uri) +
                        MenuLeft(25, act, "Người dùng", "/Thietlap/Taikhoan/", "icon-user", uri) +
                        MenuLeft(25, act, "Lịch sử người dùng", "/Thietlap/Timkiemlichsu/", "icon-user", uri) +
                        MenuLeft(41, act, "Đại biểu", "/Thietlap/Daibieu/", "icon-user-md", uri) +
                        MenuLeft(26, act, "Khiếu nại lĩnh vực", "/Thietlap/Linhvuc/", "icon-tag", uri) +
                        MenuLeft(26, act, "Kiến nghị lĩnh vực", "/Thietlap/Linhvuc_Coquan/", "icon-tag", uri) +
                        MenuLeft(52, act, "Trả lời phân loại", "/Thietlap/Traloiphanloai/", "icon-tag", uri) +
                        MenuLeft(27, act, "Loại đơn", "/Thietlap/Loaidon/", "icon-tags", uri) +
                        MenuLeft(28, act, "Nội dung đơn", "/Thietlap/Noidungdon/", "icon-align-left", uri) +
                        MenuLeft(29, act, "Tính chất vụ việc", "/Thietlap/Tinhchatdon/", "icon-list-ul", uri) +
                        MenuLeft(30, act, "Nguồn đơn", "/Thietlap/Nguondon/", "icon-exchange", uri) +
                        MenuLeft(32, act, "Nguồn kiến nghị", "/Thietlap/Nguonkiennghi/", "icon-exchange", uri) +
                        MenuLeft(31, act, "Địa phương", "/Thietlap/Diaphuong/", "icon-map-marker", uri) +
                        MenuLeft(34, act, "Nghề nghiệp", "/Thietlap/Nghenghiep/", "icon-briefcase", uri) +
                        MenuLeft(35, act, "Quốc tịch", "/Thietlap/Quoctich/", "icon-globe", uri) +
                        MenuLeft(36, act, "Dân tộc", "/Thietlap/Dantoc/", "icon-group", uri) +
                        MenuLeft(37, act, "Loại văn bản", "/Thietlap/Loaivanban/", "icon-file", uri);
            Response.Write(hethong);
            return null;
        }

        public ActionResult Ajax_Load_MenuTop(FormCollection fc)
        {
            UserInfor info = GetUserInfor();
            if (info == null) { return null; }
            string uri = fc["uri"];
            string trangchu = "", trangchu_active = "";
            if (uri == "/") { trangchu_active = "active"; }
            trangchu = "<li class='" + trangchu_active + "'><a href='/'>Trang chủ</a></li>";
            //string str = trangchu;
            //string str = trangchu + Header_MenuTop(uri);
            string str = Header_MenuTop(uri);
            Response.Write(str);
            return null;
        }
        public string MenuTop(int id, TaikhoanAtion act, string name, string url)
        {
            string str = "";
            str = "<li  onclick=\"ShowPageLoading()\"><a href='" + url + "'></i> " + name + "</a></li>";
            if (id == 0) { return str; }
            if (!base_busineess.Action_(id, act)) { return ""; }
            return str;
        }
        public string MenuLeft(int id, TaikhoanAtion act, string name, string url, string icon, string uri, string title = "")
        {
            string str = "";

            string active = "";
            if (url == uri) { active = "active"; }
            str = "<li title='" + name + "' onclick=\"ShowPageLoading()\"><a class='" + active + "'  href='" + url + "'><i class='" + icon + "'></i> <span>" + name + "</span></a></li>";
            if (id == 0) { return str; }
            if (!base_busineess.Action_(id, act)) { return ""; }
            return str;
        }
        public ActionResult Delete_success(string url)
        {
            Response.Redirect(url);
            return null;
        }
        public ActionResult DownLoad(string id)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            int id_sc = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
            FILE_UPLOAD f = kn.GET_FileBYID(id_sc);
            string[] f_ = f.CFILE.Split('/');
            WebClient client = new WebClient();
            string file_path = f.CFILE;
            int count = f_.Length - 1;
            string fileName = f_[count].ToString();
            string dir_path_download = AppConfig.dir_path_download;

            byte[] fileBytes;
            if (dir_path_download != "")
            {
                file_path = dir_path_download + file_path;
            }
            else
            {
                file_path = Server.MapPath("~/" + f.CFILE + "");
            }
            fileBytes = client.DownloadData(file_path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }

        //public ActionResult ConvertRole()
        //{
        //    var lst_user = _tl.Get_Taikhoan();
        //    Dictionary<string, object> con = new Dictionary<string, object>();
        //    con.Add("IPARENT", 20);
        //    var lst_usercoquan = _tl.GetBy_List_Quochoi_Coquan(con);
        //    var lst = from u in lst_user
        //              join uc in lst_usercoquan
        //              on u.IDONVI equals uc.ICOQUAN
        //              where u.IUSER != 86
        //              select u;
        //    var lst_u = lst.ToList();
        //    foreach (var u in lst_u)
        //    {
        //        con = new Dictionary<string, object>();
        //        con.Add("IUSER", u.IUSER);
        //        bool kq = _tl.Delete_User_Action_Multi(con);
        //        con = new Dictionary<string, object>();
        //        con.Add("IUSER", 86);
        //        var lst_useraction = _tl.GetBy_List_User_Action(con);
        //        foreach (var x in lst_useraction)
        //        {
        //            USER_ACTION ua = new USER_ACTION();
        //            ua.IACTION = x.IACTION;
        //            ua.IUSER = u.IUSER;
        //            _tl.Insert_User_Action(ua);
        //        }

        //    }

        //    return null;
        //}

    }

}
