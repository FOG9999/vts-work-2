using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Models;
using System.IO;
using DataAccess.Busineess;
using Utilities;
using KienNghi.App_Code;
using Entities.Objects;
using ClosedXML.Excel;
using Utilities.Enums;
namespace KienNghi.Controllers
{
    public class TiepdanController : BaseController
    {
        //
        // GET: /Tiepdan/
        private const string vuviectiepdanSession = "vuviectiepdanSession";
        private const string vuvieckiemtrungSession = "vuvieckiemtrungSession";
        Tiepdan tiepdan = new Tiepdan();
        Khieunai kn = new Khieunai();
        Thietlap tl = new Thietlap();
        Log log = new Log();
        KntcBusineess _kntc = new KntcBusineess();
        BaseBusineess ba_se = new BaseBusineess();
        Funtions func = new Funtions();
        TiepdanBusineess _tiepdan = new TiepdanBusineess();
        ThietlapBusineess _thietlap = new ThietlapBusineess();
        Dictionary<string, object> _condition;
        VanbanBusineess _vanban = new VanbanBusineess();
        TiepdanReport tiepdanreport = new TiepdanReport();
        Base base_appcode = new Base();
        public string RemovePostPerPageFromUrl()
        {
            //string url = Request.RawUrl;
            string url = RemovePageFromUrl();
            string[] url_split = url.Split('&');
            if (url_split.Length > 1)
            {
                url = "";
                if (url_split.Length == 2)
                {
                    url = url_split[0];
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
            return url;
        }
        public string Option_Post_Per_Page(int post_per_page)
        {
            string url = RemovePostPerPageFromUrl();
            string str = "<div class='post_per_page'>Hiển thị <select onchange=\"location.href='" + url + "&post_per_page='+this.value\" class='input-small' name='post_per_page'>";
            List<int> list_post = new List<int>();
            list_post.Add(pageSize); list_post.Add(50); list_post.Add(100); list_post.Add(200);
            foreach (var p in list_post)
            {
                string select = ""; if (p == post_per_page) { select = "selected"; }
                str += "<option " + select + " value='" + p + "'>" + p + " dòng</option>";
            }
            str += "</select> mỗi trang</div>";
            return str;
        }
        public TD_VUVIEC get_Request_Paramt_VuViec()
        {
            TD_VUVIEC vuviec = new TD_VUVIEC();

            //dTuNgay=&dDenNgay=&iDonVi=4&cNguoiGui_Ten=&cNguoiGui_DiaChi=&
            //cNoiDung=&iLoai=-1&iLinhVuc=-1&iNoiDung=-1&iTinhChat=-1&iHinhThuc=-1&iLoaiVuViec=0
            if (Request["iLoai"] != null)
            {
                vuviec.ILOAIDON = Convert.ToInt32(Request["iLoai"]);
            }
            else { vuviec.ILOAIDON = -1; }
            //
            if (Request["iLinhVuc"] != null)
            {
                vuviec.ILINHVUC = Convert.ToInt32(Request["iLinhVuc"]);
            }
            else { vuviec.ILINHVUC = -1; }
            //
            //
            if (Request["iNoiDung"] != null)
            {
                vuviec.INOIDUNG = Convert.ToInt32(Request["iNoiDung"]);
            }
            else { vuviec.INOIDUNG = -1; }
            //
            if (Request["iTinhChat"] != null)
            {
                vuviec.ITINHCHAT = Convert.ToInt32(Request["iTinhChat"]);
            }
            else { vuviec.ITINHCHAT = -1; }
            //
            if (Request["iHinhThuc"] != null)
            {
                vuviec.ITINHTRANGXULY = Convert.ToInt32(Request["iHinhThuc"]);
            }
            else { vuviec.ITINHTRANGXULY = -1; }
            //
            if (Request["iDonVi"] != null)
            {
                vuviec.IDONVI = Convert.ToInt32(Request["iDonVi"]);
            }
            else { vuviec.IDONVI = -1; }
            //
            //
            if (Request["cNoiDung"] != null)
            {
                vuviec.CNOIDUNG = Request["cNoiDung"];
            }
            else { vuviec.CNOIDUNG = ""; }
            if (Request["cNguoiGui_Ten"] != null)
            {
                vuviec.CNGUOIGUI_TEN = Request["cNguoiGui_Ten"];
            }
            else { vuviec.CNGUOIGUI_TEN = ""; }
            if (Request["cNguoiGui_DiaChi"] != null)
            {
                vuviec.CNGUOIGUI_DIACHI = Request["cNguoiGui_DiaChi"];
            }
            else { vuviec.CNGUOIGUI_DIACHI = ""; }
            if (Request["cNguoiGui_DiaChi"] != null)
            {
                vuviec.CNGUOIGUI_DIACHI = Request["cNguoiGui_DiaChi"];
            }
            else { vuviec.CNGUOIGUI_DIACHI = ""; }


            return vuviec;
        }
        public string UploadFile(HttpPostedFileBase file)
        {
            Random random = new Random(); int rand = random.Next(0, 99999);

            string file_name = "";
            string dir_path_upload = AppConfig.dir_path_upload;
            string path_upload = "/Tiepdan/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
            string full_path = "";
            if (dir_path_upload == "")
            {
                full_path = "/upload/" + path_upload;
                bool IsExists = Directory.Exists(Server.MapPath(path_upload));
                if (!IsExists)
                {
                    Directory.CreateDirectory(Server.MapPath(path_upload));
                }
                file_name = path_upload + DateTime.Now.ToString("Hmmss") + rand + "_" + func.ConvertVn(file.FileName);
                file.SaveAs(Server.MapPath(file_name));
                return file_name;
            }
            else
            {
                full_path = dir_path_upload + path_upload;
                bool IsExists = Directory.Exists(full_path);
                if (!IsExists)
                {
                    Directory.CreateDirectory(full_path);
                }
                if (file != null && file.ContentLength > 0)
                {
                    file_name = DateTime.Now.ToString("Hmmss") + rand + "_" + func.ConvertVn(file.FileName);
                    file.SaveAs(full_path + file_name);
                }
                return path_upload + file_name;
            }
        }
        public ActionResult Dinhky(int page = 1)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                int iuser = -1;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iuser = u_info.tk_action.iUser;
                }
                if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                string tukhoa = "";
                if (Request["q"] != null)
                {
                    tukhoa = Request["q"];
                }
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                var thongtinlichtiep = tiepdanreport.GetListLichTiepDinhKy("PKG_TD_VUVIEC.PRC_LICHTIEPDINHKY", tukhoa, page, post_per_page, iDonViTiepNhan, iuser);
                //ViewData["list"] = tiepdan.Tiepdan_dinhky(u_info.tk_action.iUser, (int)u_info.user_login.IDONVI,kiemtra);
                if (thongtinlichtiep != null && thongtinlichtiep.Count() > 0)
                {
                    ViewData["list"] = tiepdan.LIST_LICHTIEPDINHKY(thongtinlichtiep, iDonViTiepNhan, iuser);
                    ViewData["phantrang"] = "<tr><td colspan='5'>" + base_appcode.PhanTrang((int)thongtinlichtiep.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='5'  class='alert tcenter alert-info' >Không có kết quả tìm kiếm</td></tr>";
                }
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách định kỳ");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Dinhky_tamthoi(int page = 1)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                int iuser = -1;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iuser = u_info.tk_action.iUser;
                }
                if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                string tukhoa = "";
                if (Request["q"] != null)
                {
                    tukhoa = Request["q"];
                }
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                var thongtinlichtiep = tiepdanreport.GetListLichTiepDinhKy("PKG_TD_VUVIEC.PRC_LICHTIEPDINHKY", tukhoa, page, post_per_page, iDonViTiepNhan, iuser);
                //ViewData["list"] = tiepdan.Tiepdan_dinhky(u_info.tk_action.iUser, (int)u_info.user_login.IDONVI,kiemtra);
                if (thongtinlichtiep != null && thongtinlichtiep.Count() > 0)
                {
                    ViewData["list"] = tiepdan.LIST_LICHTIEPDINHKY(thongtinlichtiep, iDonViTiepNhan, iuser);
                    ViewData["phantrang"] = "<tr><td colspan='5'>" + base_appcode.PhanTrang((int)thongtinlichtiep.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='5'  class='alert tcenter alert-info' >Không có kết quả tìm kiếm</td></tr>";
                }
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách định kỳ");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_TDVuViecDK_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(id_encr);
                }
                // UserInfor u_info = GetUserInfor();
                int kiemtra = 0;
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    kiemtra = 1;
                }
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                Response.Write(tiepdan.Tiepdan_dinhky_search(u_info.tk_action.iUser, Request.Cookies["url_key"].Value, (int)id, 0, kiemtra));


                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Vụ viêc tra cứu");
                throw;
            }
        }
        public ActionResult Thuongxuyen(int page = 1)
        {

            //....
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                UserInfor u_info = GetUserInfor();
                int iUser_KQ = u_info.tk_action.iUser;
                int iUser = -1;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("19", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser = -1;

                }
                string tukhoa = "";
                if (Request["q"] != null)
                {
                    tukhoa = Request["q"];
                }
                TD_VUVIEC Thongtinvuviec = get_Request_Paramt_VuViec();
                List<TD_VUVIEC_XULY> Thongtinxulyvuviec = _tiepdan.Get_list_TDVuviecxuly();
                string dTuNgay = DateTime.MinValue.ToString("dd-MMM-yyyy");
                string dDenNgay = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                if (Request["dTuNgay"] != null && Request["dTuNgay"] != "")
                {

                    dTuNgay = func.ConvertDateToSql(Request["dTuNgay"].ToString());
                }
                if (Request["dDenNgay"] != null && Request["dDenNgay"] != "")
                {
                    dDenNgay = func.ConvertDateToSql(Request["dDenNgay"].ToString());
                }
                var td = tiepdanreport.GETLISTTIEPCONGDAN("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIECDANHSACH", Thongtinvuviec, dTuNgay, dDenNgay, page, post_per_page, "" + tukhoa.Trim() + "", -1, -1, 0, iUser, iDonViTiepNhan).ToList();
                if (td.Count() > 0)
                {
                    if ((Request["dTuNgay"] != null && Request["dTuNgay"] != "") || Request["q"] != null)
                    {
                        ViewData["ketqua"] = "<tr><td colspan='8' class='alert tcenter alert-info'> Có " + td.FirstOrDefault().TOTAL + " kết quả tìm kiếm</td></tr>";
                    }
                    ViewData["list"] = tiepdan.TIEPDANVUVIECTHUONGXUYEN_PHANTRANG(td, Thongtinxulyvuviec, Request.Cookies["url_key"].Value);
                    if (td.Count() > 0)
                    {
                        ViewData["phantrang"] = "<tr><td colspan='8'>" + base_appcode.PhanTrang((int)td.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    }
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='8' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách thường xuyên ");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tamxoa(int page = 1)
        {

            //....
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                UserInfor u_info = GetUserInfor();
                int iUser_KQ = u_info.tk_action.iUser;
                int iUser = -1;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("19", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser = -1;

                }
                string tukhoa = "";
                if (Request["q"] != null)
                {
                    tukhoa = Request["q"];
                }
                TD_VUVIEC Thongtinvuviec = get_Request_Paramt_VuViec();
                List<TD_VUVIEC_XULY> Thongtinxulyvuviec = _tiepdan.Get_list_TDVuviecxuly();
                string dTuNgay = DateTime.MinValue.ToString("dd-MMM-yyyy");
                string dDenNgay = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                if (Request["dTuNgay"] != null && Request["dTuNgay"] != "")
                {

                    dTuNgay = func.ConvertDateToSql(Request["dTuNgay"].ToString());
                }
                if (Request["dDenNgay"] != null && Request["dDenNgay"] != "")
                {
                    dDenNgay = func.ConvertDateToSql(Request["dDenNgay"].ToString());
                }
                var td = tiepdanreport.GETLISTTIEPCONGDAN("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIECTAMTHOI", Thongtinvuviec, dTuNgay, dDenNgay, page, post_per_page, "" + tukhoa.Trim() + "", -1, -1, 0, iUser, iDonViTiepNhan).ToList();
                if (td.Count() > 0)
                {
                    if ((Request["dTuNgay"] != null && Request["dTuNgay"] != "") || Request["q"] != null)
                    {
                        ViewData["ketqua"] = "<tr><td colspan='8' class='alert tcenter alert-info'> Có " + td.FirstOrDefault().TOTAL + " kết quả tìm kiếm</td></tr>";
                    }
                    ViewData["list"] = tiepdan.TIEPDANVUVIECTHUONGXUYEN_PHANTRANG_XOATAMTHOI(td, Thongtinxulyvuviec, Request.Cookies["url_key"].Value);
                    if (td.Count() > 0)
                    {
                        ViewData["phantrang"] = "<tr><td colspan='8'>" + base_appcode.PhanTrang((int)td.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    }
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='8' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách thường xuyên ");
                return View("../Home/Error_Exception");
            }

        }
        public string RemovePageFromUrl()
        {
            string url = Request.RawUrl;
            string[] url_split = url.Split('&');
            if (url_split.Length > 1)
            {
                url = "";
                if (url_split.Length == 2)
                {
                    url = url_split[0];
                }
                else
                {
                    int count = 0;
                    foreach (var u in url_split)
                    {
                        if (count < url_split.Length)
                        {
                            if (u.IndexOf("page=") == -1)
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
            return url;
        }
        public ActionResult Thuongxuyen_add()
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                if (!ba_se.Action_(19, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iDonVi = tl.IDDonVi_User(u_info.tk_action.iUser);
                SetTokenAction("tiepdan_thuongxuyen_add");
                if (tl.IS_BanDanNguyen(u_info.tk_action.iUser))
                {
                    ViewData["opt-coquantiepdan"] = tiepdan.OptionCoQuan(0, 0, iDonVi, ID_Coquan_bo_nganh);
                }
                else
                {
                    ViewData["opt-coquantiepdan"] = "<option value='" + iDonVi + "'>" + _tiepdan.Get_Quochoi_Coquan(iDonVi).CTEN + "</option>";
                }
                List<KNTC_LOAIDON> loaidon = _tiepdan.HienThiDanhSachLoaiDon();
                List<KNTC_NOIDUNGDON> noidung = _thietlap.Get_Noidungdon();
                List<KNTC_TINHCHAT> tinhchat = _thietlap.Get_Tinhchat();
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                ViewData["opt-loaidon"] = tl.Option_LoaiDon(loaidon);
                ViewData["opt-noidung"] = tl.Option_NoiDungDon(noidung);
                ViewData["opt-tinhchat"] = tl.Option_TinhChatDon(tinhchat);
                ViewData["opt-linhvuc"] = tl.Option_LinhVuc(linhvuc);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách thường xuyên ");
                //return null;
                throw;
            }
        }

        public string Get_Option_DonViTiepNhan(int iDonVi = 0)
        {

            List<QUOCHOI_COQUAN> coquan;
            UserInfor u_info = GetUserInfor();
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            donvi.Add("IPARENT", ID_Coquan_doandaibieu);
            coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
            string opt_bandannguyen = "";
            if (u_info.tk_action.is_lanhdao)
            {
                // opt_bandannguyen = "<option value='" + u_info.user_login.IDONVI + "'>Chọn đơn vị tiếp nhận</option>";
                if (u_info.user_login.IDONVI == iDonVi)
                {
                    opt_bandannguyen += "<option selected value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
                }
                else
                {
                    opt_bandannguyen += "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
                }
            }
            return opt_bandannguyen + tiepdan.OptionCoQuanXuLy(coquan, ID_Coquan_doandaibieu, 0, iDonVi, 0);
        }
        public ActionResult Ajax_TDVuViec_search(int id)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                List<TD_VUVIEC> vuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDONVI == id).ToList();
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    vuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDINHKY == 0 && x.IUSER == iUser && x.IDONVI == id).ToList();
                    //  thongtintimkiem = _tiepdan.Get_search_noidungvuviec(ctentimkiem).Where(x => x.IDINHKY == 0 && x.IUSER == iuser && x.IDONVI == idonvi).ToList();
                }

                List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                string str = tiepdan.Thuongxuyen_vuviec(vuviec, Xuly, u_info.tk_action.iUser, Request.Cookies["url_key"].Value, (int)id, 1);
                Response.Write(str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Vu việc tra cứu ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_TDDOTXUAT_search(int id)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();

                List<TD_VUVIEC> vuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDONVI == id).ToList();
                //if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                //{
                ////    vuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDINHKY == 0 && x.IUSER == iUser && x.IDONVI == id).ToList();
                //    //  thongtintimkiem = _tiepdan.Get_search_noidungvuviec(ctentimkiem).Where(x => x.IDINHKY == 0 && x.IUSER == iuser && x.IDONVI == idonvi).ToList();
                //}

                int iUser_KQ = u_info.tk_action.iUser;
                int iUser = 0;
                int iDonViTiepNhan = id;
                if (!ba_se.ActionMulty_Redirect_("53", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                    //iDonViTiepNhan = 0;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser = 0;
                    iDonViTiepNhan = 0;
                }
                //   List<TD_VUVIEC> vuviec = _tiepdan.Get_TDVuviec();


                List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                string str = tiepdan.TIEPDANVUVIECDOTXUAT(Xuly, u_info.tk_action.iUser, id, 1, Request.Cookies["url_key"].Value, iUser_KQ);
                Response.Write(str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Vu việc tra cứu ");
                //return null;
                throw;
            }
        }
        public string Opt_LoaiVuViec(int iloai = 0)
        {
            string str = "";
            string sel1 = "", sel2 = "", sel3 = "";
            if (iloai == 1)
            {
                sel1 = "selected";
            }
            else if (iloai == 0)
            {
                sel2 = "selected";
            }
            else
            {
                sel3 = "selected";
            }
            str = "<option value='1' " + sel1 + ">Tiếp nhận trong tiếp công dân Định kỳ</option>" +
                      "<option value='0' " + sel2 + ">Tiếp nhận trong tiếp công dân Thường xuyên</option>" +
                      "<option value='2'  " + sel3 + ">Tiếp nhận trong tiếp công dân Đột xuất</option>";
            return str;
        }
        public ActionResult Thuongxuyen_tracuu(int page = 1)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                if (!ba_se.ActionMulty_Redirect_("17,18,19,51", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                int iUser = u_info.tk_action.iUser;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;


                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                // ViewData["opt-donvi"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, iDonViTiepNhan);

                Dictionary<string, object> donvi = new Dictionary<string, object>();
                if (u_info.tk_action.is_lanhdao)
                {
                    donvi.Add("IPARENT", ID_Coquan_doandaibieu);
                    coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
                    //  ViewData["opt-coquantiepdan"] = "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>" +
                    //    tiepdan.OptionCoQuanXuLy(coquan, ID_Coquan_doandaibieu, 0, iDonViTiepNhan, 0);
                    ViewData["opt-donvi"] = " <option value='-1'>- - - Chọn tất cả</option>" + Get_Option_DonViTiepNhan(iDonViTiepNhan);
                }
                else
                {
                    donvi.Add("ICOQUAN", iDonViTiepNhan);
                    coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
                    ViewData["opt-donvi"] = "<option value='" + iDonViTiepNhan + "'>" + u_info.tk_action.tendonvi + "</option>";
                }
                int tracuudoan = Convert.ToInt32(Request["iDoan_Tracuu"]);
                int idonvitracuu = Convert.ToInt32(Request["iDonVi"]);
                string ctennguoiguitracuu = Request["cNguoiGui_Ten"];
                string cdiachinguoigui = Request["cNguoiGui_DiaChi"];
                string cnoidungtracuu = Request["cNoiDung"];
                int iloaidontracuu = Convert.ToInt32(Request["iLoai"]);
                int ilinhvuctracuu = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidungtracuu = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchattracuu = Convert.ToInt32(Request["iTinhChat"]);
                int itinhtrangxuly = Convert.ToInt32(Request["iHinhThuc"]);
                int ikiemtratrung = Convert.ToInt32(Request["iVuViecTrung"]);
                int itaikhoan = Convert.ToInt32(Request["iTaiKhoan"]);
                ViewData["opt-taikhoan"] = tiepdan.Option_TaiKhoanDonVi(iDonViTiepNhan, itaikhoan);
                // List<KNTC_LOAIDON> loaidon = _tiepdan.HienThiDanhSachLoaiDon();
                // List<KNTC_NOIDUNGDON> noidung = _thietlap.Get_Noidungdon();
                //List<KNTC_TINHCHAT> tinhchat = _thietlap.Get_Tinhchat();
                //List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();

                ViewData["opt-loaidon"] = kn.Option_LoaiDon(iloaidontracuu);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(inoidungtracuu);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(itinhchattracuu);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(ilinhvuctracuu);
                ViewData["opt-hinhthuc"] = tiepdan.optin_hinhthuc();
                ViewData["cNguoiGui"] = ctennguoiguitracuu;
                ViewData["cDiaChi"] = cdiachinguoigui;
                ViewData["cNoiDung"] = cnoidungtracuu;
                int iUser2 = 0;
                int iDonViTiepNhan2 = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser2 = u_info.tk_action.iUser;
                    //iDonViTiepNhan = 0;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser2 = 0;
                    iDonViTiepNhan2 = 0;
                }
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }

                int loaivuviec = Convert.ToInt32(Request["iLoaiVuViec"]);
                ViewData["Opt_VuViec"] = Opt_LoaiVuViec(loaivuviec);
                string dTuNgay = DateTime.MinValue.ToString("dd-MMM-yyyy");
                string dDenNgay = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                if (Request["dTuNgay"] != null && Request["dTuNgay"].ToString() != "")
                {
                    dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));
                    ViewData["TuNgay"] = Request["dTuNgay"];

                }
                if (Request["dDenNgay"] != null && Request["dDenNgay"].ToString() != "")
                {
                    dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));
                    ViewData["DenNgay"] = Request["dDenNgay"];
                }


                if (itaikhoan != -1)
                {
                    iUser2 = itaikhoan;
                }
                List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                TD_VUVIEC VUVIEC = new TD_VUVIEC();
                if (loaivuviec == 2)
                {

                    VUVIEC.ITIEPDOTXUAT = 1;
                    VUVIEC.IDINHKY = 0;
                }
                else
                {
                    VUVIEC.ITIEPDOTXUAT = 0;
                    VUVIEC.IDINHKY = loaivuviec;
                }

                VUVIEC.IDOANDONGNGUOI = tracuudoan;
                VUVIEC.IDONVI = idonvitracuu;
                VUVIEC.CNGUOIGUI_DIACHI = cdiachinguoigui;
                VUVIEC.CNGUOIGUI_TEN = ctennguoiguitracuu;
                VUVIEC.CNOIDUNG = cnoidungtracuu;
                VUVIEC.ILOAIDON = iloaidontracuu;
                VUVIEC.ILINHVUC = ilinhvuctracuu;
                VUVIEC.INOIDUNG = inoidungtracuu;
                VUVIEC.ITINHCHAT = itinhchattracuu;
                VUVIEC.ITINHTRANGXULY = itinhtrangxuly;
                VUVIEC.IVUVIECTRUNG = ikiemtratrung;
                VUVIEC.IUSER = iUser2;
                var thongtinvuviec = tiepdanreport.ListTraCuu("PKG_TD_VUVIEC.PRC_TIEPDAN_TRACUUVUVIEC", VUVIEC, dTuNgay, dDenNgay, page, post_per_page);
                if (thongtinvuviec != null && thongtinvuviec.Count() > 0)
                {
                    ViewData["list"] = tiepdan.TIEPDANTRACUU(thongtinvuviec, Xuly, dTuNgay, dDenNgay);
                    ViewData["phantrang"] = "<tr><td colspan='7'>" + base_appcode.PhanTrang((int)thongtinvuviec.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='7' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Vu việc tra cứu ");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Dinhky_add()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = _kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,0, 0);
                SetTokenAction("tiepdan_dinhky_add");
                return PartialView("../Ajax/Tiepdan/Dinhky_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm mới lịch");
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Dinhky_insert()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                var fc = this.Request;
                string dNgay = func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayTiep"])).Trim();
                string cdiadiem = func.RemoveTagInput(fc["cDiaDiem"]).ToUpper().Trim().ToUpper();
                // var kiemtra = _tiepdan.Get_List_TiepDanDinhKy().Where(x => x.CDIADIEM.ToUpper() == cdiadiem && x.DNGAYTIEP == dNgay).ToList();
                var kiemtra = _tiepdan.Check_trung(dNgay, cdiadiem).ToList();
                if (kiemtra.Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {

                    if (!CheckTokenAction("tiepdan_dinhky_add")) { Response.Redirect("/Home/Error/"); return null; }
                    TIEPDAN_DINHKY t = new TIEPDAN_DINHKY();

                    t.DNGAYTIEP = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayTiep"])));
                    t.IDOAN = Convert.ToInt32(fc["iDiaPhuong_0"]);
                    if (Convert.ToInt32(fc["iDiaPhuong_0"]) == -1)
                    {
                        t.ILUOT = 0;
                    }
                    else
                    {
                        t.ILUOT = Convert.ToInt32(fc["iDiaPhuong_1"]);
                    }
                    t.IVUVIEC = u_info.user_login.IDONVI;
                    t.IUSER = u_info.tk_action.iUser;
                    t.DDATE = DateTime.Now;
                    t.CDIADIEM = func.RemoveTagInput(fc["cDiaDiem"]);
                    _tiepdan.Insert_TiepDanDinhKy(t);
                    _tiepdan.Tracking_dinhky(id_user(), (int)t.IDINHKY, "Thêm mới tiếp dân định kỳ ngày: " + fc["dNgayTiep"]);
                    //Phanloai_VuViec(t.iDinhKy, fc);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                //Handle Exception;
                log.Log_Error(e, "Thêm mới lịch");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Dinhky_update()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                var fc = this.Request;
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string dNgay = func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayTiep"]));
                string cdiadiem = func.RemoveTagInput(fc["cDiaDiem"]);
                var kiemtra = _tiepdan.Check_trung(dNgay, cdiadiem).Where(x => x.IDINHKY != id).ToList();
                if (kiemtra.Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("tiepdan_dinhky_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                    TIEPDAN_DINHKY t = _tiepdan.HienThiThongTinTiepDanDinhKy(id);
                    t.DNGAYTIEP = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayTiep"])));
                    t.CDIADIEM = fc["cDiaDiem"];
                    t.IDOAN = Convert.ToInt32(fc["iDiaPhuong_0"]);
                    if (Convert.ToInt32(fc["iDiaPhuong_0"]) == -1)
                    {
                        t.ILUOT = 0;
                    }
                    else
                    {
                        t.ILUOT = Convert.ToInt32(fc["iDiaPhuong_1"]);
                    }
                    _tiepdan.UpdateThongTinTiepDanDinhKy(t);
                    _tiepdan.Tracking_dinhky(id_user(), (int)t.IDINHKY, "Cập nhật lại tiếp dân định kỳ");
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "chỉnh sửa lịch tiếp");
                //return null;
                throw;
            }
        }
        public ActionResult Dinhky_vuviec(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                int iUser_KQ = u_info.tk_action.iUser;
                int iUser = -1;
                ViewData["id_dinhky"] = Request["id"];
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser = -1;

                }
                string tukhoa = "";
                if (Request["q"] != null)
                {
                    tukhoa = Request["q"];
                }
                TD_VUVIEC Thongtinvuviec = get_Request_Paramt_VuViec();
                List<TD_VUVIEC_XULY> Thongtinxulyvuviec = _tiepdan.Get_list_TDVuviecxuly();
                string dTuNgay = DateTime.MinValue.ToString("dd-MMM-yyyy");
                string dDenNgay = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                if (Request["dTuNgay"] != null && Request["dTuNgay"] != "")
                {

                    dTuNgay = func.ConvertDateToSql(Request["dTuNgay"].ToString());
                }
                if (Request["dDenNgay"] != null && Request["dDenNgay"] != "")
                {
                    dDenNgay = func.ConvertDateToSql(Request["dDenNgay"].ToString());
                }
                var td = tiepdanreport.GETLISTTIEPCONGDAN("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIECDANHSACH", Thongtinvuviec, dTuNgay, dDenNgay, page, post_per_page, "" + tukhoa.Trim() + "", id, -1, id, iUser, iDonViTiepNhan).ToList();
                if (td.Count() > 0)
                {
                    if ((Request["dTuNgay"] != null && Request["dTuNgay"] != "") || Request["q"] != null)
                    {
                        ViewData["ketqua"] = "<tr><td colspan='8' class='alert tcenter alert-info'> Có " + td.FirstOrDefault().TOTAL + " kết quả tìm kiếm</td></tr>";
                    }
                    ViewData["list"] = tiepdan.TIEPDANVUVIECDINHKY_PHANTRANG(td, Thongtinxulyvuviec, Request.Cookies["url_key"].Value);
                    ViewData["phantrang"] = "<tr><td colspan='8'>" + base_appcode.PhanTrang((int)td.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }

                else
                {
                    ViewData["list"] = "<tr><td colspan='8' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách vụ việc dịnh kỳ");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Dinhky_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {

                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = fc["id"];
                SetTokenAction("tiepdan_dinhky_edit", id);
                ViewData["dinhky"] = _tiepdan.HienThiThongTinTiepDanDinhKy(id);
                var thongtin = _tiepdan.HienThiThongTinTiepDanDinhKy(id);
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = _kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,0, (int)thongtin.IDOAN);
                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,(int)thongtin.IDOAN, (int)thongtin.ILUOT);
                return PartialView("../Ajax/Tiepdan/Dinhky_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "chỉnh sửa vụ việc dịnh kỳ");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Dinhky_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                _condition = new Dictionary<string, object>();
                _condition.Add("IDINHKY", id);
                TIEPDAN_DINHKY tddk = _tiepdan.HienThiThongTinTiepDanDinhKy(id);
                _condition = new Dictionary<string, object>();
                _condition.Add("IDINHKY", id);
                var vuviec = _tiepdan.Get_TDVuviec(_condition).ToList();
                foreach (var x in vuviec)
                {
                    _tiepdan.delete_vuviec((int)x.IVUVIEC);
                }
                _tiepdan.DeleteThongTinTiepDanDinhKy(tddk);
                _tiepdan.Tracking_TD_dinhky(id_user(), (int)tddk.IDINHKY, Request.Cookies["url_key"].Value);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa vụ việc dịnh kỳ");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Thuongxuyen_add(FormCollection fc)
        {
            try
            {                //....

                if (!CheckTokenAction("tiepdan_thuongxuyen_add")) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                for (int i = 1; i < 4; i++)
                {
                    HttpPostedFileBase file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        CheckFile_Upload(file);
                    }
                    HttpPostedFileBase file_kequa = Request.Files["ketqua_upload" + i];
                    if (file_kequa != null && file_kequa.ContentLength > 0)
                    {
                        CheckFile_Upload(file_kequa);
                    }
                }
                TIEPDAN_THUONGXUYEN t = new TIEPDAN_THUONGXUYEN();
                t.IUSER = iUser; t.DDATE = DateTime.Now;
                t.DNGAYTIEP = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTiep"]));
                t.ICOQUANTIEPDAN = Convert.ToInt32(fc["iCoQuanTiepDan"]);
                t.CDIADIEM = func.RemoveTagInput(fc["cDiaDiem"]);
                t.CNGUOITIEP = func.RemoveTagInput(fc["cNguoiTiep"]);
                t.CNGUOIGUI_TEN = func.RemoveTagInput(fc["cNguoiGui_Ten"]);
                t.CNGUOIGUI_DIACHI = func.RemoveTagInput(fc["cNguoiGui_DiaChi"]);
                if (fc["iDoan"] != null)
                {
                    t.IDOAN = 1;
                    t.IDOAN_NGUOI = Convert.ToInt32(fc["iDoan_Nguoi"]);
                }
                else
                {
                    t.IDOAN = 0; t.IDOAN_NGUOI = 0;
                }
                t.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                t.ILOAI = Convert.ToInt32(fc["iLoai"]);
                t.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                t.INOIDUNG = Convert.ToInt32(fc["iNoiDung"]);
                t.ITINHCHAT = Convert.ToInt32(fc["iTinhChat"]);
                t.ITHUONGXUYEN_TRUNG = 0;
                t.ITHUONGXUYEN = 0;

                _tiepdan.Insert_TiepDanThuongXuyen(t);

                TIEPDAN_THUONGXUYEN_KETQUA k = new TIEPDAN_THUONGXUYEN_KETQUA();
                k.ITHUONGXUYEN = t.ITHUONGXUYEN;
                k.CKETQUA_NGUOITRALOI = func.RemoveTagInput(fc["cKetQua_NguoiTraLoi"]);
                k.CKETQUA = func.RemoveTagInput(fc["cKetQua"]);
                _tiepdan.Insert_TiepDanThuongXuyenLoaivuviec(k);
                for (int i = 1; i < 4; i++)
                {
                    HttpPostedFileBase file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "thuongxuyen";
                        f.CFILE = UploadFile(file);
                        f.ID = t.ITHUONGXUYEN;
                        _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                    }
                    HttpPostedFileBase file_kequa = Request.Files["ketqua_upload" + i];
                    if (file_kequa != null && file_kequa.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "thuongxuyen_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = k.IKETQUA;
                        _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                    }
                }

                //_tiepdan.Tracking_thuongxuyen(iUser, (int)t.ITHUONGXUYEN, "Thêm vụ việc tiếp dân thường xuyên");
                Response.Redirect(Request.Cookies["url_return"].Value);
                return null;
            }
            catch (Exception e)
            {
                //Handle Exception;
                log.Log_Error(e, "Thêm vụ việc thường xuyên");
                //return null;
                throw;
            }
        }

        // Bổ sung 05/12
        public ActionResult Themmoi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            UserInfor u_info = GetUserInfor();
            try
            {
                //....

                if (!ba_se.ActionMulty_Redirect_("17,18,19,51", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                if (Request["id"] != null && Request["id"] != "")
                {

                    if (Request["module"] != null && Request["module"] != "" && Request["module"] == "edit")
                    {
                        int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                        var vuviec = _tiepdan.Get_TDVuviecID((int)id);
                        var vuviec_trung = vuviec;
                        if (vuviec.IVUVIECTRUNG != 0)
                        {
                            vuviec_trung = _tiepdan.Get_TDVuviecID((int)vuviec.IVUVIECTRUNG);
                        }
                        //  ViewData["opt-nguondon"] = kn.Option_NguonDon(id_user());
                        ViewData["opt-quoctich"] = kn.Option_QuocTich((int)vuviec_trung.INGUOIGUI_QUOCTICH);
                        ViewData["opt-dantoc"] = kn.Option_DanToc((int)vuviec_trung.INGUOIGUI_DANTOC);
                        //   ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent((int)vuviec.IDIAPHUONG_0, (int)vuviec.IDIAPHUONG_1);
                        ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)vuviec_trung.ILOAIDON);


                        ViewData["Thongtinnoidung"] = vuviec_trung.CNOIDUNG;
                        ViewData["opt-tinhchat"] = tiepdan.Option_TinhChatDon_ThuocNguonDon((int)vuviec_trung.ITINHCHAT, (int)vuviec_trung.INOIDUNG);
                        ViewData["opt-noidung"] = tiepdan.Option_NoiDungDon_ThuocLinhVuc((int)vuviec_trung.INOIDUNG, (int)vuviec_trung.ILINHVUC);
                        List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                        List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
                        ViewData["morong"] = 1;
                        ViewData["style"] = "display:block";

                        ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon((int)vuviec_trung.ILINHVUC, (int)vuviec_trung.ILOAIDON);
                        // ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC();
                        _condition = new Dictionary<string, object>();
                        _condition.Add("IHIENTHI", 1);
                        _condition.Add("IDELETE", 0);
                        List<DIAPHUONG> lstDiaphuong = _kntc.List_DiaPhuong(_condition);
                        ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,0, (int)vuviec_trung.IDIAPHUONG_0);
                        ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,(int)vuviec_trung.IDIAPHUONG_0, (int)vuviec_trung.IDIAPHUONG_1);
                        ViewData["sonha"] = vuviec_trung.CNGUOIGUI_DIACHI;
                        ViewData["nguoitiep"] = vuviec.CNGUOITIEP;
                        ViewData["NgayLichTiep"] = func.ConvertDateVN(vuviec_trung.DNGAYNHAN.ToString());
                        ViewData["Lanhdao"] = tl.Option_LanhDao(0, (int)u_info.user_login.IDONVI);
                        ViewData["tencongdanden"] = vuviec_trung.CNGUOIGUI_TEN;
                        ViewData["lichtiepdinhky"] = "";
                        ViewData["lichtiepthuongxuyen"] = "selected";
                        ViewData["lichtiepdotxuat"] = "";
                        ViewData["opt_tiepdinhky"] = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick' value='" + func.ConvertDateVN(vuviec.DNGAYNHAN.ToString()) + "' />";
                        if (vuviec.IDINHKY != null && vuviec.IDINHKY != 0)
                        {
                            ViewData["lichtiepdinhky"] = "selected";
                            ViewData["lichtiepthuongxuyen"] = "";
                            ViewData["opt_tiepdinhky"] = "  <select class='input-block-level' name='iTiepDinhKy' id='iTiepDinhKy' >" +
                                                             " <option value='0'>- - - Chọn lịch tiếp </option>" +
                                                             "" + tiepdan.OptionTiepdan((int)vuviec.IDINHKY, (int)u_info.user_login.IDONVI) + "" +
                                                          "</select>";
                            ViewData["ngaytiep"] = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick'  />";
                            ViewData["TenTieuDe"] = "Ngày tiếp <i style='color:red'>*</i>";

                        }
                        if (vuviec.ITIEPDOTXUAT == 1)
                        {
                            ViewData["lichtiepdinhky"] = "";
                            ViewData["lichtiepthuongxuyen"] = "";
                            ViewData["lichtiepdotxuat"] = "selected";
                        }


                        // str = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick' />";


                        var thongtinkiemtrung = _tiepdan.Get_TDVuviecID(id);
                        ViewData["XoaFile"] = read_fileVB((int)id);
                        ViewData["VuViecID"] = Request["id"];
                        // _tiepdan.Delete_TDVuviec(vuviec);
                        return View();
                    }
                    else if (Request["module"] != null && Request["module"] != "" && Request["module"] == "add")
                    {
                        TD_VUVIEC vuviecss = (TD_VUVIEC)Session[vuviectiepdanSession];
                        _condition = new Dictionary<string, object>();
                        _condition.Add("IHIENTHI", 1);
                        _condition.Add("IDELETE", 0);
                        List<DIAPHUONG> lstDiaphuong = _kntc.List_DiaPhuong(_condition);
                        ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,0, (int)vuviecss.IDIAPHUONG_0);
                        ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,(int)vuviecss.IDIAPHUONG_0, (int)vuviecss.IDIAPHUONG_1);
                        ViewData["sonha"] = vuviecss.CNGUOIGUI_DIACHI;
                        ViewData["nguoitiep"] = vuviecss.CNGUOITIEP;
                        ViewData["NgayLichTiep"] = func.ConvertDateVN(vuviecss.DNGAYNHAN.ToString());
                        ViewData["Lanhdao"] = tl.Option_LanhDao((int)vuviecss.ILANHDAOTIEP, (int)u_info.user_login.IDONVI);
                        ViewData["tencongdanden"] = vuviecss.CNGUOIGUI_TEN;
                        ViewData["lichtiepdinhky"] = "";
                        ViewData["lichtiepthuongxuyen"] = "selected";
                        ViewData["lichtiepdotxuat"] = "";
                        ViewData["opt_tiepdinhky"] = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick' value='" + func.ConvertDateVN(vuviecss.DNGAYNHAN.ToString()) + "' />";
                        if (vuviecss.IDINHKY != null && vuviecss.IDINHKY != 0)
                        {
                            ViewData["lichtiepdinhky"] = "selected";
                            ViewData["lichtiepthuongxuyen"] = "";
                            ViewData["opt_tiepdinhky"] = "  <select class='input-block-level' name='iTiepDinhKy' id='iTiepDinhKy' >" +
                                                             " <option value='0'>- - - Chọn lịch tiếp </option>" +
                                                             "" + tiepdan.OptionTiepdan((int)vuviecss.IDINHKY, (int)u_info.user_login.IDONVI) + "" +
                                                          "</select>";
                            ViewData["ngaytiep"] = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick' value='" + func.ConvertDateVN(vuviecss.DNGAYNHAN.ToString()) + "'  />";
                            ViewData["TenTieuDe"] = "Ngày tiếp <i style='color:red'>*</i>";

                        }
                        if (vuviecss.ITIEPDOTXUAT == 1)
                        {
                            ViewData["lichtiepdinhky"] = "";
                            ViewData["lichtiepthuongxuyen"] = "";
                            ViewData["lichtiepdotxuat"] = "selected";
                        }

                        ViewData["opt-quoctich"] = kn.Option_QuocTich((int)vuviecss.INGUOIGUI_QUOCTICH);
                        ViewData["opt-dantoc"] = kn.Option_DanToc((int)vuviecss.INGUOIGUI_DANTOC);
                        ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)vuviecss.ILOAIDON);


                        ViewData["Thongtinnoidung"] = vuviecss.CNOIDUNG;
                        ViewData["opt-tinhchat"] = tiepdan.Option_TinhChatDon_ThuocNguonDon((int)vuviecss.ITINHCHAT, (int)vuviecss.INOIDUNG);
                        ViewData["opt-noidung"] = tiepdan.Option_NoiDungDon_ThuocLinhVuc((int)vuviecss.INOIDUNG, (int)vuviecss.ILINHVUC);
                        List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                        List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
                        ViewData["morong"] = 1;
                        ViewData["style"] = "display:block";
                        ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon((int)vuviecss.ILINHVUC, (int)vuviecss.ILOAIDON);
                        ViewData["VuViecID"] = "";
                        return View();
                    }
                    else
                    {
                        TD_VUVIEC vuviec = (TD_VUVIEC)Session[vuviectiepdanSession];
                        //   var vuviec = _tiepdan.Get_TDVuviecID((int)id);
                        var vuviec_trung = vuviec;
                        if (vuviec.IVUVIECTRUNG != 0)
                        {
                            vuviec_trung = _tiepdan.Get_TDVuviecID((int)vuviec.IVUVIECTRUNG);
                        }
                        //  ViewData["opt-nguondon"] = kn.Option_NguonDon(id_user());
                        ViewData["opt-quoctich"] = kn.Option_QuocTich((int)vuviec_trung.INGUOIGUI_QUOCTICH);
                        ViewData["opt-dantoc"] = kn.Option_DanToc((int)vuviec_trung.INGUOIGUI_DANTOC);
                        //   ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent((int)vuviec.IDIAPHUONG_0, (int)vuviec.IDIAPHUONG_1);
                        ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)vuviec_trung.ILOAIDON);


                        ViewData["Thongtinnoidung"] = vuviec_trung.CNOIDUNG;
                        ViewData["opt-tinhchat"] = tiepdan.Option_TinhChatDon_ThuocNguonDon((int)vuviec_trung.ITINHCHAT, (int)vuviec_trung.INOIDUNG);
                        ViewData["opt-noidung"] = tiepdan.Option_NoiDungDon_ThuocLinhVuc((int)vuviec_trung.INOIDUNG, (int)vuviec_trung.ILINHVUC);
                        List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                        List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
                        ViewData["morong"] = 1;
                        ViewData["style"] = "display:block";

                        ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon((int)vuviec_trung.ILINHVUC, (int)vuviec_trung.ILOAIDON);
                        // ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC();
                        _condition = new Dictionary<string, object>();
                        _condition.Add("IHIENTHI", 1);
                        _condition.Add("IDELETE", 0);
                        List<DIAPHUONG> lstDiaphuong = _kntc.List_DiaPhuong(_condition);
                        ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,0, (int)vuviec_trung.IDIAPHUONG_0);
                        ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,(int)vuviec_trung.IDIAPHUONG_0, (int)vuviec_trung.IDIAPHUONG_1);
                        ViewData["sonha"] = vuviec_trung.CNGUOIGUI_DIACHI;
                        ViewData["nguoitiep"] = vuviec.CNGUOITIEP;
                        ViewData["NgayLichTiep"] = func.ConvertDateVN(vuviec_trung.DNGAYNHAN.ToString());
                        ViewData["Lanhdao"] = tl.Option_LanhDao((int)vuviec_trung.ILANHDAOTIEP, (int)u_info.user_login.IDONVI);
                        ViewData["tencongdanden"] = vuviec_trung.CNGUOIGUI_TEN;
                        ViewData["lichtiepdinhky"] = "";
                        ViewData["lichtiepthuongxuyen"] = "selected";
                        ViewData["lichtiepdotxuat"] = "";
                        ViewData["opt_tiepdinhky"] = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick' value='" + func.ConvertDateVN(vuviec.DNGAYNHAN.ToString()) + "' />";
                        if (vuviec.IDINHKY != null && vuviec.IDINHKY != 0)
                        {
                            ViewData["lichtiepdinhky"] = "selected";
                            ViewData["lichtiepthuongxuyen"] = "";
                            ViewData["opt_tiepdinhky"] = "  <select class='input-block-level' name='iTiepDinhKy' id='iTiepDinhKy' >" +
                                                             " <option value='0'>- - - Chọn lịch tiếp </option>" +
                                                             "" + tiepdan.OptionTiepdan((int)vuviec.IDINHKY, (int)u_info.user_login.IDONVI) + "" +
                                                          "</select>";
                            ViewData["ngaytiep"] = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick' value='" + func.ConvertDateVN(vuviec.DNGAYNHAN.ToString()) + "'  />";
                            ViewData["TenTieuDe"] = "Ngày tiếp <i style='color:red'>*</i>";

                        }
                        if (vuviec.ITIEPDOTXUAT == 1)
                        {
                            ViewData["lichtiepdinhky"] = "";
                            ViewData["lichtiepthuongxuyen"] = "";
                            ViewData["lichtiepdotxuat"] = "selected";
                        }


                        // str = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick' />";
                        ViewData["VuViecID"] = "";
                        // _tiepdan.Delete_TDVuviec(vuviec);
                    }
                    return View();

                }
                else
                {
                    if (Request["sel"] != null && Request["sel"] != "")
                    {
                        if (Request["sel"] == "thuongxuyen")
                        {
                            ViewData["lichtiepdinhky"] = "";
                            ViewData["lichtiepdotxuat"] = "";
                            ViewData["ngaytiep"] = "";
                            ViewData["lichtiepthuongxuyen"] = "selected";
                            ViewData["opt_tiepdinhky"] = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick'  />";
                        }
                        else if (Request["sel"] == "dotxuat")
                        {
                            ViewData["lichtiepdinhky"] = "";
                            ViewData["lichtiepdotxuat"] = "selected";
                            ViewData["lichtiepthuongxuyen"] = "";
                            ViewData["ngaytiep"] = "";
                            ViewData["opt_tiepdinhky"] = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick'  />";
                        }
                        else
                        {
                            ViewData["lichtiepdotxuat"] = "";
                            ViewData["lichtiepdinhky"] = "selected";
                            ViewData["lichtiepthuongxuyen"] = "";
                            ViewData["opt_tiepdinhky"] = "<select class='input-block-level chzn-done' name='iTiepDinhKy' id='iTiepDinhKy' onchange='LoadThongTinLich()'><option value='0'>- - - Chọn lịch tiếp </option> +" + tiepdan.OptionTiepdan(0, (int)u_info.user_login.IDONVI) + "</select>";
                            ViewData["ngaytiep"] = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick'  />";
                            ViewData["TenTieuDe"] = "Ngày tiếp <i style='color:red'>*</i>";
                        }
                    }
                    //func.RemoveCookies("__RequestVerificationToken");
                    List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                    List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
                    // func.SetCookies("url_return", Request.Url.AbsoluteUri);

                    ViewData["style"] = "display:none";
                    if (!ba_se.ActionMulty_Redirect_("43,17,18,19", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                    int iUser = u_info.tk_action.iUser;
                    SetTokenAction("tiepdan_themmoi");
                    ViewData["Lanhdao"] = tl.Option_LanhDao(0, (int)u_info.user_login.IDONVI);
                    ViewData["opt-nguondon"] = kn.Option_NguonDon(id_user());
                    ViewData["opt-quoctich"] = kn.Option_QuocTich(233);
                    ViewData["opt-dantoc"] = kn.Option_DanToc(1);
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IHIENTHI", 1);
                    _condition.Add("IDELETE", 0);
                    List<DIAPHUONG> lstDiaphuong = _kntc.List_DiaPhuong(_condition);
                    ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,0, 0);
                    ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                    ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                    ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                    ViewData["opt-linhvuc"] = tiepdan.Option_LinhVuc();
                    // 
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IDONVI", 0);
                    _condition.Add("ISTATUS", 1);
                    List<USERS> lstUser = _kntc.List_User(_condition);
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IACTION", 12);
                    List<USER_ACTION> lstUserAction = _kntc.List_UserAction(_condition);
                    ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, 0);
                    // ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(0,0);
                    ViewData["nguoitiep"] = tl.Taikhoan_Detail(iUser).ten;
                    ViewData["morong"] = 0;
                    return View();
                }


            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm mới vụ việc ");
                return View("../Home/Error_Exception");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoi(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                //....
                if (fc["vuvieckiemtra"] != "")
                {
                    int id = Convert.ToInt32(HashUtil.Decode_ID(fc["vuvieckiemtra"], Request.Cookies["url_key"].Value));
                    for (int i = 1; i < 4; i++)
                    {
                        file = Request.Files["file_upload" + i];
                        if (file != null && file.ContentLength > 0)
                        {
                            if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/"); return null; }
                        }
                    }
                    UserInfor u_info = GetUserInfor();
                    int iUser = u_info.tk_action.iUser;
                    TD_VUVIEC don = _tiepdan.Get_TDVuviecID(id);


                    //don.INGUONDON = Convert.ToInt32(fc["iNguonDon"]);
                    don.CNGUOIGUI_TEN = func.RemoveTagInput(fc["cNguoiGui_Ten"]).Trim();
                    if (fc["iDoanDongNguoi"] != null)
                    {
                        don.IDOANDONGNGUOI = 1;
                        don.ISONGUOI = Convert.ToInt32(fc["iSoNguoi"]);

                    }
                    else
                    {
                        don.IDOANDONGNGUOI = 0;
                        don.ISONGUOI = 0;
                    }
                    if (Convert.ToInt32(fc["iLoaiTiep"]) == 0)
                    {
                        don.IDINHKY = Convert.ToInt32(fc["iTiepDinhKy"]);
                        don.ITIEPDOTXUAT = 0;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));

                    }
                    else if (Convert.ToInt32(fc["iLoaiTiep"]) == 2)
                    {
                        don.IDINHKY = 0;
                        don.ITIEPDOTXUAT = 1;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                    }
                    else
                    {
                        don.IDINHKY = 0;
                        don.ITIEPDOTXUAT = 0;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                    }
                    don.IVUVIECTRUNG = 0;
                    don.IDIAPHUONG_0 = Convert.ToInt32(fc["iDiaPhuong_0"]);
                    don.IDIAPHUONG_1 = Convert.ToInt32(fc["iDiaPhuong_1"]);
                    don.CNGUOITIEP = func.RemoveTagInput(fc["cNguoiTiep_Ten"]).Trim();
                    don.CNGUOIGUI_DIACHI = func.RemoveTagInput(fc["cNguoiGui_DiaChi"]).Trim();
                    don.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]).Trim();
                    don.IUSER = iUser;
                    don.INGUONDON = 0;
                    don.IDELETE = 0;
                    don.DDATE = DateTime.Now;
                    don.CNOIDUNGCHIDAO = func.RemoveTagInput(fc["cNoiDungChiDao"]).Trim();
                    don.ILANHDAOTIEP = Convert.ToInt32(fc["iLanhDaoTiep"]);
                    don.ILOAIDON = Convert.ToInt32(fc["iLoaiDon"]);
                    don.INOIDUNG = Convert.ToInt32(fc["iNoiDung"]);
                    don.ITINHCHAT = Convert.ToInt32(fc["iTinhChat"]);
                    don.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                    don.INGUOIGUI_DANTOC = Convert.ToInt32(fc["iNguoiGui_DanToc"]);
                    don.INGUOIGUI_QUOCTICH = Convert.ToInt32(fc["iNguoiGui_QuocTich"]);
                    _tiepdan.Update_TDVuviec(don);
                    //  kntc.TiepNhan_Don(don);
                    int ivuviec = (int)don.IVUVIEC;
                    for (int i = 1; i < 4; i++)
                    {
                        file = Request.Files["file_upload" + i];
                        if (file != null && file.ContentLength > 0)
                        {
                            FILE_UPLOAD f = new FILE_UPLOAD();
                            f.CTYPE = "tiepdan_vuviec";
                            f.CFILE = UploadFile(file);
                            f.ID = (int)don.IVUVIEC;
                            _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                        }

                    }
                    List<TD_VUVIEC> v = _tiepdan.get_vuviec_kiemtrung(ivuviec);
                    //if (v != null && v.Count() > 0)
                    //{
                    Response.Redirect("/Tiepdan/Kiemtrung/?id=" + HashUtil.Encode_ID(ivuviec.ToString(), Request.Cookies["url_key"].Value) + "#success");
                    //}
                    //else
                    //{

                    //    Response.Redirect("/Tiepdan/Themmoi/?sel=thuongxuyen#success");
                    //}

                }
                else
                {
                    if (!CheckTokenAction("tiepdan_themmoi"))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    for (int i = 1; i < 4; i++)
                    {
                        file = Request.Files["file_upload" + i];
                        if (file != null && file.ContentLength > 0)
                        {
                            if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/"); return null; }
                        }
                    }
                    UserInfor u_info = GetUserInfor();
                    int iUser = u_info.tk_action.iUser;
                    TD_VUVIEC don = new TD_VUVIEC();
                    don.INGUONDON = Convert.ToInt32(fc["iNguonDon"]);
                    don.CNGUOIGUI_TEN = func.RemoveTagInput(fc["cNguoiGui_Ten"]).Trim();
                    don.ISOLUONGTRUNG = 0;
                    if (fc["iDoanDongNguoi"] != null)
                    {
                        don.IDOANDONGNGUOI = 1;
                        don.ISONGUOI = Convert.ToInt32(fc["iSoNguoi"]);
                    }
                    else
                    {
                        don.IDOANDONGNGUOI = 0;
                        don.ISONGUOI = 0;
                    }
                    if (Convert.ToInt32(fc["iLoaiTiep"]) == 0)
                    {
                        don.IDINHKY = Convert.ToInt32(fc["iTiepDinhKy"]);
                        don.ITIEPDOTXUAT = 0;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));

                    }
                    else if (Convert.ToInt32(fc["iLoaiTiep"]) == 2)
                    {
                        don.IDINHKY = 0;
                        don.ITIEPDOTXUAT = 1;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                    }
                    else
                    {
                        don.IDINHKY = 0;
                        don.ITIEPDOTXUAT = 0;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                    }
                    don.IDIAPHUONG_0 = Convert.ToInt32(fc["iDiaPhuong_0"]);
                    if (Convert.ToInt32(fc["iDiaPhuong_0"]) == -1)
                    {
                        don.IDIAPHUONG_1 = 0;
                    }
                    else
                    {
                        don.IDIAPHUONG_1 = Convert.ToInt32(fc["iDiaPhuong_1"]);
                    }
                    don.CNGUOITIEP = func.RemoveTagInput(fc["cNguoiTiep_Ten"]).Trim();
                    don.CNGUOIGUI_DIACHI = func.RemoveTagInput(fc["cNguoiGui_DiaChi"]).Trim();
                    don.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                    don.IUSER = iUser;
                    don.DDATE = DateTime.Now;
                    don.ILOAIDON = Convert.ToInt32(fc["iLoaiDon"]);
                    don.INOIDUNG = Convert.ToInt32(fc["iNoiDung"]);
                    don.ITINHCHAT = Convert.ToInt32(fc["iTinhChat"]);
                    don.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                    don.ITINHTRANGXULY = 0;
                    don.IVUVIECTRUNG = 0;
                    don.IDONVITIEPNHAN = 0;
                    don.IGIAMSAT = 0;
                    don.IDONDOC = 0;
                    don.IDELETE = 0;
                    don.CNOIDUNGCHIDAO = func.RemoveTagInput(fc["cNoiDungChiDao"]).Trim();
                    don.ILANHDAOTIEP = Convert.ToInt32(fc["iLanhDaoTiep"]);
                    don.CYKIENGIAMSAT = "";
                    don.IDONVI = (int)u_info.user_login.IDONVI;
                    don.INGUOIGUI_DANTOC = Convert.ToInt32(fc["iNguoiGui_DanToc"]);
                    don.INGUOIGUI_QUOCTICH = Convert.ToInt32(fc["iNguoiGui_QuocTich"]);
                    _tiepdan.Insert_TDVuviec(don);

                    int ivuviec = (int)don.IVUVIEC;
                    for (int i = 1; i < 4; i++)
                    {
                        file = Request.Files["file_upload" + i];
                        if (file != null && file.ContentLength > 0)
                        {
                            FILE_UPLOAD f = new FILE_UPLOAD();
                            f.CTYPE = "tiepdan_vuviec";
                            f.CFILE = UploadFile(file);
                            f.ID = (int)don.IVUVIEC;
                            _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                        }

                    }
                    func.SetCookies("url_return", Request.Url.AbsoluteUri);
                    _tiepdan.Tracking_Tiepdan(iUser, ivuviec, "Thêm mới vụ việc");
                    List<TD_VUVIEC> v = _tiepdan.get_vuviec_kiemtrung(ivuviec);
                    //if (v != null && v.Count() > 0)
                    //{
                    Response.Redirect("/Tiepdan/Kiemtrung/?id=" + HashUtil.Encode_ID(ivuviec.ToString(), Request.Cookies["url_key"].Value) + "#success");
                    //}
                    //else
                    //{

                    //    Response.Redirect("/Tiepdan/Themmoi/?sel=thuongxuyen#success");
                    //}
                }
                return null;
            }
            catch (Exception e)
            {
                //Handle Exception;
                log.Log_Error(e, "Thêm mới vụ việc ");
                return View("../Home/Error_Exception");
            }

        }


        public ActionResult Kiemtrung(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { return null; }
            UserInfor u_info = GetUserInfor();
            if (!ba_se.ActionMulty_Redirect_("17,18,19,51", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            func.SetCookies("url_return", Request.Url.AbsoluteUri);
            try
            {
                //....
                if (Request["id"] == null)
                {
                    TD_VUVIEC vuviec = (TD_VUVIEC)Session[vuviectiepdanSession];
                    ViewData["vuviec"] = vuviec;
                    if (vuviec.IDINHKY == 0 || vuviec.IDINHKY == null)
                    {
                        ViewData["Ngaytiep"] = "";
                    }
                    else
                    {
                        //ViewData["Ngaytiep"] = func.ConvertDateVN(_tiepdan.HienThiThongTinTiepDanDinhKy((int)vuviec.IDINHKY).DNGAYTIEP.ToString());
                    }
                    string thongtintinh = "";
                    var kiemtratinh = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)vuviec.IDIAPHUONG_0).ToList();
                    if (kiemtratinh.Count() > 0)
                    {
                        thongtintinh = _thietlap.GetBy_DiaphuongID((int)vuviec.IDIAPHUONG_0).CTEN;
                    }
                    string thongtinhuyen = "";
                    var kiemtrahuyen = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)vuviec.IDIAPHUONG_1).ToList();
                    if (kiemtrahuyen.Count() > 0)
                    {
                        thongtinhuyen = _thietlap.GetBy_DiaphuongID((int)vuviec.IDIAPHUONG_1).CTEN + " , ";
                    }
                    string diachinggui = "";
                    if (vuviec.CNGUOIGUI_DIACHI != "" && vuviec.CNGUOIGUI_DIACHI != null)
                    {
                        diachinggui = "" + vuviec.CNGUOIGUI_DIACHI + " , ";
                    }
                    ViewData["thongtindiachi"] = diachinggui + "" + thongtinhuyen + "" + thongtintinh + "";
                    if (vuviec.INGUOIGUI_QUOCTICH == 0)
                    {
                        ViewData["Quoctich"] = "Chưa xác định";
                    }
                    else
                    {
                        ViewData["Quoctich"] = _thietlap.GetBy_QuoctichID((int)vuviec.INGUOIGUI_QUOCTICH).CTEN;
                    }
                    if (vuviec.INGUOIGUI_DANTOC == 0)
                    {
                        ViewData["Dantoc"] = "Chưa xác định";
                    }
                    else
                    {
                        ViewData["Dantoc"] = _thietlap.GetBy_DantocID((int)vuviec.INGUOIGUI_DANTOC).CTEN;
                    }
                    if (vuviec.ILOAIDON == 0)
                    {
                        ViewData["Loaidon"] = "Chưa xác định";
                    }
                    else
                    {
                        ViewData["Loaidon"] = _thietlap.GetBy_LoaidonID((int)vuviec.ILOAIDON).CTEN;
                    }
                    if (vuviec.ILINHVUC == 0)
                    {
                        ViewData["Linhvuc"] = "Chưa xác định";
                    }
                    else
                    {
                        ViewData["Linhvuc"] = _thietlap.GetBy_LinhvucID((int)vuviec.ILINHVUC).CTEN;
                    }
                    if (vuviec.INOIDUNG == 0)
                    {
                        ViewData["Nhomnoidung"] = "Chưa xác định";
                    }
                    else
                    {
                        ViewData["Nhomnoidung"] = _thietlap.GetBy_NoidungdonID((int)vuviec.INOIDUNG).CTEN;
                    }
                    if (vuviec.ITINHCHAT == 0)
                    {
                        ViewData["tinhchat"] = "Chưa xác định";
                    }
                    else
                    {
                        ViewData["tinhchat"] = _thietlap.GetBy_NoidungdonID((int)vuviec.ITINHCHAT).CTEN;
                    }
                    if (vuviec.DNGAYNHAN.ToString() != null)
                    {
                        ViewData["Ngaylapdon"] = func.ConvertDateVN(vuviec.DNGAYNHAN.ToString());
                    }
                    string load = "";
                    //ViewData["File"] = tiepdan.File_View((int)vuviec.IVUVIEC, "tiepdan_vuviec");
                    var v = _tiepdan.get_vuviec_kiemtrungnhanh((int)vuviec.IDIAPHUONG_0, vuviec.CNOIDUNG, vuviec.CNGUOIGUI_TEN).Where(x => x.IVUVIECTRUNG == 0).ToList();
                    // List<TD_VUVIEC> v = _tiepdan.get_vuviec_kiemtrung(id).Where(x => x.IVUVIECTRUNG == 0).ToList(); 
                    if (vuviec.IDINHKY != 0 && vuviec.ITIEPDOTXUAT == 0)
                    {
                        ViewData["loaitiep"] = "Tiếp định kỳ";
                        load = "/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(vuviec.IDINHKY.ToString(), Request.Cookies["url_key"].Value);
                        v = _tiepdan.get_vuviec_kiemtrungnhanh((int)vuviec.IDIAPHUONG_0, vuviec.CNOIDUNG, vuviec.CNGUOIGUI_TEN).Where(x => x.IVUVIECTRUNG == 0 && x.IDINHKY != 0 && x.ITIEPDOTXUAT == 0).ToList();
                    }
                    else if (vuviec.IDINHKY == 0 && vuviec.ITIEPDOTXUAT != 0)
                    {
                        ViewData["loaitiep"] = "Tiếp đột xuất";
                        load = "/Tiepdan/Dotxuat/";
                        v = _tiepdan.get_vuviec_kiemtrungnhanh((int)vuviec.IDIAPHUONG_0, vuviec.CNOIDUNG, vuviec.CNGUOIGUI_TEN).Where(x => x.IVUVIECTRUNG == 0 && x.IDINHKY == 0 && x.ITIEPDOTXUAT != 0).ToList();
                    }
                    else
                    {
                        ViewData["loaitiep"] = "Tiếp thường xuyên";
                        load = "/Tiepdan/Thuongxuyen/";
                        v = _tiepdan.get_vuviec_kiemtrungnhanh((int)vuviec.IDIAPHUONG_0, vuviec.CNOIDUNG, vuviec.CNGUOIGUI_TEN).Where(x => x.IVUVIECTRUNG == 0 && x.IDINHKY == 0 && x.ITIEPDOTXUAT == 0).ToList();
                    }
                    if (vuviec.CNOIDUNG == null)
                    {
                        ViewData["Nhaptiep"] = "  <a href='/Tiepdan/Themmoi?id=capnhatnoidung' class='btn btn-warning' onclick='ShowPageLoading()'>Cập nhật nội dung</a>";
                        ViewData["Chucnang"] = "";
                    }
                    else
                    {
                        ViewData["Nhaptiep"] = "  <a href='/Tiepdan/Themmoi?id=capnhatnoidung' class='btn btn-warning' onclick='ShowPageLoading()'>Cập nhật nội dung</a>";

                    }
                    List<KNTC_DON> don_kntc = _tiepdan.get_vuviec_kiemtrung_kntc(vuviec.CNGUOIGUI_TEN, (int)vuviec.IDIAPHUONG_0, vuviec.CNOIDUNG);
                    string str = "";
                    if (don_kntc != null && don_kntc.Count() != 0)
                    {
                        if (vuviec.IKNTC_DON != null)
                        {
                            str = tiepdan.List_DonTrung_KTNC2(don_kntc, (int)vuviec.IKNTC_DON, (int)vuviec.IVUVIEC, Request.Cookies["url_key"].Value);
                        }
                        else
                        {
                            str = tiepdan.List_DonTrung_KTNC2(don_kntc, 0, (int)vuviec.IVUVIEC, Request.Cookies["url_key"].Value);
                        }

                    }
                    ViewData["KiemtrungKntc"] = str;
                    ViewData["load"] = load;
                    string kiemtrung = tiepdan.Kiemtrungvuviec2(v, Request.Cookies["url_key"].Value);
                    ViewData["Kiemtrung"] = kiemtrung;
                }
                else
                {

                    int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                    ViewData["id"] = HashUtil.Encode_ID(id.ToString());
                    var thongtin = tiepdan.Vuviec_Detail(id, Request["id"]);
                    var vuviec = _tiepdan.Get_TDVuviecID(id);
                    ViewData["detail"] = tiepdan.Vuviec_Detail(id, Request["id"]);
                    ViewData["vuviec"] = vuviec;
                    if (vuviec.IDINHKY == 0 || vuviec.IDINHKY == null)
                    {
                        ViewData["Ngaytiep"] = "";
                    }
                    else
                    {
                        //ViewData["Ngaytiep"] = func.ConvertDateVN(_tiepdan.HienThiThongTinTiepDanDinhKy((int)vuviec.IDINHKY).DNGAYTIEP.ToString());
                    }
                    string thongtintinh = "";
                    var kiemtratinh = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)vuviec.IDIAPHUONG_0).ToList();
                    if (kiemtratinh.Count() > 0)
                    {
                        thongtintinh = _thietlap.GetBy_DiaphuongID((int)vuviec.IDIAPHUONG_0).CTEN;
                    }
                    string thongtinhuyen = "";
                    var kiemtrahuyen = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)vuviec.IDIAPHUONG_1).ToList();
                    if (kiemtrahuyen.Count() > 0)
                    {
                        thongtinhuyen = _thietlap.GetBy_DiaphuongID((int)vuviec.IDIAPHUONG_1).CTEN + " , ";
                    }
                    string diachinggui = "";
                    if (vuviec.CNGUOIGUI_DIACHI != "" && vuviec.CNGUOIGUI_DIACHI != null)
                    {
                        diachinggui = "" + vuviec.CNGUOIGUI_DIACHI + " , ";
                    }
                    ViewData["thongtindiachi"] = diachinggui + "" + thongtinhuyen + "" + thongtintinh + "";
                    ViewData["Quoctich"] = thongtin.quoctich;
                    ViewData["Dantoc"] = thongtin.dantoc;
                    ViewData["Loaidon"] = thongtin.loaidon;
                    ViewData["Linhvuc"] = thongtin.linhvuc;
                    ViewData["Nhomnoidung"] = thongtin.loai_noidung;
                    ViewData["tinhchat"] = thongtin.tinhchat;

                    if (vuviec.DNGAYNHAN.ToString() != null)
                    {
                        ViewData["Ngaylapdon"] = func.ConvertDateVN(vuviec.DNGAYNHAN.ToString());
                    }
                    ViewData["File"] = tiepdan.File_View((int)vuviec.IVUVIEC, "tiepdan_vuviec");
                    var v = _tiepdan.get_vuviec_kiemtrung(id).Where(x => x.IVUVIECTRUNG == 0).ToList();
                    string load = "";
                    if (v != null && v.Count() > 0)
                    {
                        if (vuviec.IDINHKY != 0 && vuviec.ITIEPDOTXUAT == 0)
                        {
                            ViewData["loaitiep"] = "Tiếp định kỳ";
                            load = "/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(vuviec.IDINHKY.ToString(), Request.Cookies["url_key"].Value);
                            v = _tiepdan.get_vuviec_kiemtrung(id).Where(x => x.IVUVIECTRUNG == 0 && x.IDINHKY != 0 && x.ITIEPDOTXUAT == 0).ToList();
                        }
                        else if (vuviec.IDINHKY == 0 && vuviec.ITIEPDOTXUAT != 0)
                        {
                            ViewData["loaitiep"] = "Tiếp đột xuất";
                            load = "/Tiepdan/Dotxuat/";
                            v = _tiepdan.get_vuviec_kiemtrung(id).Where(x => x.IVUVIECTRUNG == 0 && x.IDINHKY == 0 && x.ITIEPDOTXUAT != 0).ToList();
                        }
                        else
                        {
                            ViewData["loaitiep"] = "Tiếp thường xuyên";
                            load = "/Tiepdan/Thuongxuyen/";
                            v = _tiepdan.get_vuviec_kiemtrung(id).Where(x => x.IVUVIECTRUNG == 0 && x.IDINHKY == 0 && x.ITIEPDOTXUAT == 0).ToList();
                        }
                    }

                    if (vuviec.CNOIDUNG == null)
                    {
                        ViewData["Nhaptiep"] = "  <a href='/Tiepdan/Themmoi/?id=" + Request["id"] + "' class='btn btn-warning' onclick='ShowPageLoading()'>Cập nhật nội dung</a>";
                        ViewData["Chucnang"] = "";
                    }
                    else
                    {
                        ViewData["Nhaptiep"] = "  <a href='/Tiepdan/Themmoi/?id=" + Request["id"] + "&module=edit' class='btn btn-warning' onclick='ShowPageLoading()'>Cập nhật nội dung</a>";
                        ViewData["Chucnang"] = "" +
                                "<div class='btn-group'  style='margin-left:30%;border-radius:4px !important'   ><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn tình trạng xử lý <span class='caret'></span></a><ul class='dropdown-menu dropdown-success'><li><a href='" + load + "' class='btn btn-success' style='float:left' > Đang nghiên cứu </a></li><li><a href='#' onclick=\"ShowPopUp('id=" + Request["id"] + "','/Tiepdan/Ajax_Huongdanxuly')\" >Hướng dẫn bằng văn bản </a></li><li><a href='/Tiepdan/Ajax_HuongDanTrucTiep_insert/" + Request["id"] + "' onclick='ShowPageLoading()'  >Giải thích hướng dẫn trực tiếp</a></li><li><a href='#' onclick=\"ShowPopUp('id=" + Request["id"] + "','/Tiepdan/Ajax_Chuyenxuly')\"   >Nhận đơn (chuyển đơn sang khiếu nại tố cáo) </a></li><li><a href='#'  onclick=\"ShowPopUp('id=" + Request["id"] + "','/Tiepdan/Ajax_Chuyenxuly_noibo')\"   >Chuyển xử lý (chuyển Cơ quan có thẩm quyển)</a></li></ul></div>" +
                                "";

                    }
                    List<KNTC_DON> don_kntc = _tiepdan.get_vuviec_kiemtrung_kntc(vuviec.CNGUOIGUI_TEN, (int)vuviec.IDIAPHUONG_0, vuviec.CNOIDUNG);
                    string str = "";
                    if (don_kntc != null && don_kntc.Count() != 0)
                    {
                        if (vuviec.IKNTC_DON != null)
                        {
                            str = tiepdan.List_DonTrung_KTNC(don_kntc, (int)vuviec.IKNTC_DON, (int)vuviec.IVUVIEC, Request.Cookies["url_key"].Value);
                        }
                        else
                        {
                            str = tiepdan.List_DonTrung_KTNC(don_kntc, 0, (int)vuviec.IVUVIEC, Request.Cookies["url_key"].Value);
                        }

                    }

                    if (str == "")
                    {
                        str = "<tr><td colspan='6' class='alert alert-success tcenter b nomargin'> <i class='icon-ok-sign'></i> Không tìm thấy đơn trùng  </td></tr>";
                    }

                    ViewData["KiemtrungKntc"] = str;
                    ViewData["load"] = load;
                    string kiemtrung = tiepdan.Kiemtrungvuviec(v, Request.Cookies["url_key"].Value, (int)vuviec.IVUVIEC, (int)vuviec.IVUVIECTRUNG);
                    ViewData["Kiemtrung"] = kiemtrung;

                }
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, " kiểm trùng vụ việc ");
                return View("../Home/Error_Exception");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiemtrung(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                //....
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["iVuViec"]));
                var vuviec = _tiepdan.Get_TDVuviecID(id);
                //   vuviec.IVUVIECTRUNG = Convert.ToInt32(HashUtil.Decode_ID(fc["VuViec_Trung"]));
                //  _tiepdan.Update_TDVuviec(vuviec);
                string load = "";
                if (vuviec.IDINHKY != 0 && vuviec.ITIEPDOTXUAT == 0)
                {
                    load = "/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(vuviec.IDINHKY.ToString(), Request.Cookies["url_key"].Value + "#success");
                }
                else if (vuviec.IDINHKY == 0 && vuviec.ITIEPDOTXUAT != 0)
                {
                    load = "/Tiepdan/Dotxuat/#success";
                }
                else
                {
                    load = "/Tiepdan/Thuongxuyen/#success";
                }

                _tiepdan.Tracking_Tiepdan(iUser, (int)vuviec.IVUVIEC, "Vụ việc trùng");
                Response.Redirect(load);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "kiểm trùng vụ việc ");
                return View("../Home/Error_Exception");
            }
        }

        public string read_fileVB(int id)
        {

            //
            string url_cookie = func.Get_Url_keycookie();
            _condition = new Dictionary<string, object>();
            _condition.Add("ID", id);
            _condition.Add("CTYPE", "tiepdan_vuviec");
            string str = "";
            var filevb = _tiepdan.LIST_FILE(_condition).ToList();
            foreach (var x in filevb)
            {
                string id_encr = HashUtil.Encode_ID(x.ID_FILE.ToString(), url_cookie);
                if (x.CFILE != "")
                {
                    string del = "<a href='javascript:void(0)' onclick=\"DeleteFile('" + id_encr + "','/Tiepdan/Ajax_Delete_VanBan_file')\" class='btn btn-danger'><i class='icon-remove'></i></a>";
                    str += "<p id='file_" + id_encr + "'><a href='/Home/DownLoad/" + id_encr + "' class='btn btn-success'><i class='icon-download-alt'></i> File đính kèm </a>" + del + "</p>";
                }
            }
            return str;
        }
        public string read_fileVB_xuly(int id)
        {

            //
            string url_cookie = func.Get_Url_keycookie();
            _condition = new Dictionary<string, object>();
            _condition.Add("ID", id);
            _condition.Add("CTYPE", "tiepdan_vuviec_xuly");
            string str = "";
            var filevb = _tiepdan.LIST_FILE(_condition).ToList();
            foreach (var x in filevb)
            {
                string id_encr = HashUtil.Encode_ID(x.ID_FILE.ToString(), url_cookie);
                if (x.CFILE != "")
                {
                    string del = "<a href='javascript:void(0)' onclick=\"DeleteFile('" + id_encr + "','/Tiepdan/Ajax_Delete_VanBan_file')\" class='btn btn-danger'><i class='icon-remove'></i></a>";
                    str += "<p id='file_" + id_encr + "'><a href='/Home/DownLoad/" + id_encr + "' class='btn btn-success'><i class='icon-download-alt'></i> File đính kèm </a>" + del + "</p>";
                }
            }
            return str;

        }
        public string read_fileVB_xuly_traloi(int id)
        {
            //
            string url_cookie = func.Get_Url_keycookie();
            _condition = new Dictionary<string, object>();
            _condition.Add("ID", id);
            _condition.Add("CTYPE", "tiepdan_vuviec_xuly_traloi");
            string str = "";
            var filevb = _tiepdan.LIST_FILE(_condition).ToList();
            foreach (var x in filevb)
            {
                string id_encr = HashUtil.Encode_ID(x.ID_FILE.ToString(), url_cookie);
                if (x.CFILE != "")
                {
                    string del = "<a href='javascript:void(0)' onclick=\"DeleteFile('" + id_encr + "','/Tiepdan/Ajax_Delete_VanBan_file')\" class='btn btn-danger'><i class='icon-remove'></i></a>";
                    str += "<p id='file_" + id_encr + "'><a href='/Home/DownLoad/" + id_encr + "' class='btn btn-success'><i class='icon-download-alt'></i> File đính kèm </a>" + del + "</p>";
                }
            }
            return str;


        }
        public ActionResult Sua()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                //....
                UserInfor u_info = GetUserInfor();
                if (!ba_se.ActionMulty_Redirect_("17,18,19,51", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("tiepdan_suatiepdan");
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                string id_encr = HashUtil.Encode_ID(id.ToString(), Request.Cookies["url_key"].Value);
                ViewData["id_vuviec"] = id_encr;

                var vuviec = _tiepdan.Get_TDVuviecID((int)id);
                //  ViewData["opt-nguondon"] = kn.Option_NguonDon(id_user());
                ViewData["Lanhdao"] = tl.Option_LanhDao((int)vuviec.ILANHDAOTIEP, (int)u_info.user_login.IDONVI);
                ViewData["opt-quoctich"] = kn.Option_QuocTich((int)vuviec.INGUOIGUI_QUOCTICH);
                ViewData["opt-dantoc"] = kn.Option_DanToc((int)vuviec.INGUOIGUI_DANTOC);
                //   ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent((int)vuviec.IDIAPHUONG_0, (int)vuviec.IDIAPHUONG_1);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)vuviec.ILOAIDON);
                ViewData["opt-tinhchat"] = tiepdan.Option_TinhChatDon_ThuocNguonDon((int)vuviec.ITINHCHAT, (int)vuviec.INOIDUNG);
                ViewData["opt-noidung"] = tiepdan.Option_NoiDungDon_ThuocLinhVuc((int)vuviec.INOIDUNG, (int)vuviec.ILINHVUC);
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
                ViewData["opt_tiepdinhky"] = tiepdan.OptionTiepdan((int)vuviec.IDINHKY, (int)u_info.user_login.IDONVI);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon((int)vuviec.ILINHVUC, (int)vuviec.ILOAIDON);
                _condition = new Dictionary<string, object>();
                _condition.Add("IDONVI", 0);
                _condition.Add("ISTATUS", 1);
                List<USERS> lstUser = _kntc.List_User(_condition);
                _condition = new Dictionary<string, object>();
                _condition.Add("IACTION", 12);
                List<USER_ACTION> lstUserAction = _kntc.List_UserAction(_condition);
                ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, 0);
                //ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(0, 0);
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = _kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,0, (int)vuviec.IDIAPHUONG_0);
                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong,(int)vuviec.IDIAPHUONG_0, (int)vuviec.IDIAPHUONG_1);
                var thongtinkiemtrung = _tiepdan.Get_TDVuviecID(id);
                ViewData["ID"] = thongtinkiemtrung;
                ViewData["XoaFile"] = read_fileVB((int)id);
                string load = "";

                if (vuviec.IDINHKY != 0)
                {
                    ViewData["ngaytiep"] = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick' value='" + func.ConvertDateVN(vuviec.DNGAYNHAN.ToString()) + "'  />";
                    ViewData["TenTieuDe"] = "Ngày tiếp <i style='color:red'>*</i>";
                    load = "/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(vuviec.IDINHKY.ToString(), Request.Cookies["url_key"].Value + "#success");
                }
                else
                {
                    load = "/Tiepdan/Thuongxuyen/#success";
                }
                ViewData["Load"] = load;
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Chỉnh sửa vụ việc ");
                return View("../Home/Error_Exception");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(FormCollection fc, HttpPostedFileBase file)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id_vuviec"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("tiepdan_suatiepdan"))
                {
                    Response.Redirect("/Home/Error");
                    return null;
                }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/"); return null; }
                    }
                }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                TD_VUVIEC don = _tiepdan.Get_TDVuviecID(id);

                don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                //don.INGUONDON = Convert.ToInt32(fc["iNguonDon"]);
                don.CNGUOIGUI_TEN = func.RemoveTagInput(fc["cNguoiGui_Ten"]).Trim();
                if (fc["iDoanDongNguoi"] != null)
                {
                    don.IDOANDONGNGUOI = 1;
                    don.ISONGUOI = Convert.ToInt32(fc["iSoNguoi"]);
                }
                else
                {
                    don.IDOANDONGNGUOI = 0;
                    don.ISONGUOI = 0;
                }
                if (Convert.ToInt32(fc["iLoaiTiep"]) == 0)
                {
                    don.IDINHKY = Convert.ToInt32(fc["iTiepDinhKy"]);
                    don.ITIEPDOTXUAT = 0;
                    don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));

                }
                else if (Convert.ToInt32(fc["iLoaiTiep"]) == 2)
                {
                    don.IDINHKY = 0;
                    don.ITIEPDOTXUAT = 1;
                    don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                }
                else
                {
                    don.IDINHKY = 0;
                    don.ITIEPDOTXUAT = 0;
                    don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                }
                don.IDIAPHUONG_0 = Convert.ToInt32(fc["iDiaPhuong_0"]);
                don.IDIAPHUONG_1 = Convert.ToInt32(fc["iDiaPhuong_1"]);
                don.CNGUOITIEP = func.RemoveTagInput(fc["cNguoiTiep_Ten"]).Trim();
                don.CNGUOIGUI_DIACHI = func.RemoveTagInput(fc["cNguoiGui_DiaChi"]).Trim();
                don.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]).Trim();
                don.IUSER = iUser;
                don.CNOIDUNGCHIDAO = func.RemoveTagInput(fc["cNoiDungChiDao"]).Trim();
                don.ILANHDAOTIEP = Convert.ToInt32(fc["iLanhDaoTiep"]);
                don.DDATE = DateTime.Now;
                don.ILOAIDON = Convert.ToInt32(fc["iLoaiDon"]);
                don.INOIDUNG = Convert.ToInt32(fc["iNoiDung"]);
                don.ITINHCHAT = Convert.ToInt32(fc["iTinhChat"]);
                don.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                don.INGUOIGUI_DANTOC = Convert.ToInt32(fc["iNguoiGui_DanToc"]);
                don.INGUOIGUI_QUOCTICH = Convert.ToInt32(fc["iNguoiGui_QuocTich"]);
                _tiepdan.Update_TDVuviec(don);
                //  kntc.TiepNhan_Don(don);
                int ivuviec = (int)don.IVUVIEC;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "tiepdan_vuviec";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)don.IVUVIEC;
                        _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                    }

                }
                _tiepdan.Tracking_Tiepdan(iUser, ivuviec, "Chỉnh sửa vụ việc");
                if (don.IDINHKY != 0 && don.ITIEPDOTXUAT == 0)
                {
                    Response.Redirect("/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(don.IDINHKY.ToString(), Request.Cookies["url_key"].Value) + "#success");
                }

                else if (don.IDINHKY != 0 && don.ITIEPDOTXUAT == 1)
                {
                    Response.Redirect("/Tiepdan/Dotxuat/#success");
                }
                else
                {
                    Response.Redirect("/Tiepdan/Thuongxuyen/#success");
                }
                //Response.Redirect("/Tiepdan/Kiemtrung/?id=" + HashUtil.Encode_ID(ivuviec.ToString(), Request.Cookies["url_key"].Value) + "");
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Chỉnh sửa vụ việc ");
                return View("../Home/Error_Exception");
            }

        }
        // bổ sung tối 08/12
        public ActionResult Ajax_UpdateNgay(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                string str = "";
                if (id != 0)
                {
                    str = func.ConvertDateVN(_tiepdan.HienThiThongTinTiepDanDinhKy((int)id).DNGAYTIEP.ToString());
                }

                Response.Write(str);
                return null;
            }
            catch (Exception e)
            {
                //Handle Exception;
                return null;
            }

        }
        public ActionResult Xemchitiet()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
                UserInfor u_info = GetUserInfor();
                if (!ba_se.ActionMulty_Redirect_("17,18,19,51", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = HashUtil.Encode_ID(id.ToString());

                var vuviec = _tiepdan.Get_TDVuviecID(id);
                ViewData["detail"] = tiepdan.Vuviec_Detail(id, Request["id"]);
                var thongtin = tiepdan.Vuviec_Detail(id, Request["id"]);
                ViewData["vuviec"] = vuviec;
                ViewData["Ngaytiep"] = func.ConvertDateVN(vuviec.DNGAYNHAN.ToString());
                if (vuviec.IDINHKY == 0 || vuviec.IDINHKY == null)
                {

                    ViewData["QuayLai"] = "/Tiepdan/Thuongxuyen";
                }
                else
                {
                    ViewData["QuayLai"] = "/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(vuviec.IDINHKY.ToString(), Request.Cookies["url_key"].Value) + "";

                }
                ViewData["Quoctich"] = thongtin.quoctich;
                string thongtintinh = "";
                var kiemtratinh = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)vuviec.IDIAPHUONG_0).ToList();
                if (kiemtratinh.Count() > 0)
                {
                    thongtintinh = _thietlap.GetBy_DiaphuongID((int)vuviec.IDIAPHUONG_0).CTEN;
                }
                string thongtinhuyen = "";
                var kiemtrahuyen = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)vuviec.IDIAPHUONG_1).ToList();
                if (kiemtrahuyen.Count() > 0)
                {
                    thongtinhuyen = _thietlap.GetBy_DiaphuongID((int)vuviec.IDIAPHUONG_1).CTEN + " , ";
                }
                string diachinggui = "";
                if (vuviec.CNGUOIGUI_DIACHI != "" && vuviec.CNGUOIGUI_DIACHI != null)
                {
                    diachinggui = "" + vuviec.CNGUOIGUI_DIACHI + " , ";
                }
                if (vuviec.IDINHKY != 0 && vuviec.ITIEPDOTXUAT == 0)
                {
                    ViewData["loaitiep"] = "Tiếp định kỳ";
                }
                else if (vuviec.IDINHKY == 0 && vuviec.ITIEPDOTXUAT != 0)
                {
                    ViewData["loaitiep"] = "Tiếp đột xuất";
                }
                else
                {
                    ViewData["loaitiep"] = "Tiếp thường xuyên";
                }
                ViewData["thongtindiachi"] = diachinggui + "" + thongtinhuyen + "" + thongtintinh + "";
                ViewData["Dantoc"] = thongtin.dantoc;
                ViewData["Loaidon"] = thongtin.loaidon;
                ViewData["Linhvuc"] = thongtin.linhvuc;
                ViewData["Nhomnoidung"] = thongtin.loai_noidung;
                ViewData["tinhchat"] = thongtin.tinhchat;
                ViewData["Ngaylapdon"] = func.ConvertDateVN(vuviec.DNGAYNHAN.ToString());
                ViewData["File"] = tiepdan.File_View((int)vuviec.IVUVIEC, "tiepdan_vuviec");
                ViewData["traloi"] = tiepdan.Vanban_traloi_vuviec(id, Request.Cookies["url_key"].Value);
                if (vuviec.ISOLUONGTRUNG == 0 || vuviec.ISOLUONGTRUNG == null)
                {
                    ViewData["soluongtrung"] = "0";
                }
                else
                {
                    ViewData["soluongtrung"] = vuviec.ISOLUONGTRUNG;
                }
                //ViewData["hienthi"] = "display:block";
                //if (tiepdan.Vanban_traloi_vuviec(id, Request.Cookies["url_key"].Value) == "")
                //{
                //    ViewData["hienthi"] = "display:none";
                //}

                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Chi tiết vụ việc ");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Tracuuvuviec(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                int id = Convert.ToInt32(fc["id"]);
                string str = "";
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;

                if (id == 1)
                {
                    List<TD_VUVIEC> vuviec = _tiepdan.Get_TDVuviec();

                    List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                    str = tiepdan.Thuongxuyen_vuviec(vuviec, Xuly, iUser, Request.Cookies["url_key"].Value);
                }
                else
                {
                    List<TD_VUVIEC> vuviec = _tiepdan.Get_TDVuviec();

                    str = tiepdan.Kydinh_vuviec_search(vuviec, iUser, Request.Cookies["url_key"].Value);
                }
                Response.Write(str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tra cứu vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Vuviec_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....


                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                _tiepdan.delete_vuviec(id);
                _tiepdan.Tracking_Tiepdan(id_user(), id, "Xóa vụ việc tiếp dân");
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Vuviec_deltamthoi(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....


                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var thongtinvuviec = _tiepdan.Get_TDVuviecID(id);
                thongtinvuviec.IDELETE = 1;
                _tiepdan.Update_TDVuviec(thongtinvuviec);
                _tiepdan.Tracking_Tiepdan(id_user(), id, "Xóa tạm thời vụ việc tiếp dân");
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Vuviec_KhoiPhuc(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....


                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var thongtinvuviec = _tiepdan.Get_TDVuviecID(id);
                thongtinvuviec.IDELETE = 0;
                _tiepdan.Update_TDVuviec(thongtinvuviec);
                _tiepdan.Tracking_Tiepdan(id_user(), id, " Khôi phục Xóa tạm thời vụ việc tiếp dân");
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Khôi phục  Xóa vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Luukiemtrung(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....


                int id_vuviec = Convert.ToInt32(HashUtil.Decode_ID(fc["iVuViec"]));
                int vuviectrung = Convert.ToInt32(HashUtil.Decode_ID(fc["VuViec_Trung"], Request.Cookies["url_key"].Value)); ;
                var kiemtrung = _tiepdan.Get_TDVuviecID(id_vuviec);
                kiemtrung.IVUVIECTRUNG = (int)vuviectrung;
                _tiepdan.Update_TDVuviec(kiemtrung);
                _tiepdan.Tracking_Tiepdan(id_user(), id_vuviec, "Lưu kiểm trùng");
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "lưu kiểm trùng vụ việc ");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Luukiemtrung_nhapsoluong()
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                int id_vuviec = Convert.ToInt32(HashUtil.Decode_ID(Request["id"]));
                ViewData["id"] = Request["id"];
                ViewData["idtrung"] = Request["idtrung"];
                return PartialView("../Ajax/Tiepdan/VuViec_Trung");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form theo dõi");
                return null;
            }
        }
        public ActionResult Ajax_Luukiemtrung_nhapsoluong2()
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                ViewData["idtrung"] = Request["idtrung"];
                return PartialView("../Ajax/Tiepdan/VuViec_Trung2");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form theo dõi");
                return null;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Vuviectrung_insert(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                TD_VUVIEC don = _tiepdan.Get_TDVuviecID(iDon);
                int iDonTrung = Convert.ToInt32(HashUtil.Decode_ID(fc["idtrung"], Request.Cookies["url_key"].Value));
                TD_VUVIEC dontrung = _tiepdan.Get_TDVuviecID(iDonTrung);
                don.ISOLUONGTRUNG = Convert.ToInt32(fc["iSoLuongTrung"]);
                don.CLYDOTRUNG = func.RemoveTagInput(fc["cLuuTheoDoi_LyDo"]);
                don.IVUVIECTRUNG = dontrung.IVUVIEC;
                _tiepdan.Update_TDVuviec(don);
                int iUser = id_user();
                _tiepdan.Tracking_Tiepdan(id_user(), iDon, "Lưu kiểm trùng nhập số lượng đơn trùng");
                string load = "";
                if (don.IDINHKY != 0 && don.ITIEPDOTXUAT == 0)
                {
                    load = "/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(don.IDINHKY.ToString(), Request.Cookies["url_key"].Value + "#success");
                }
                else if (don.IDINHKY == 0 && don.ITIEPDOTXUAT != 0)
                {
                    load = "/Tiepdan/Dotxuat/#success";
                }

                else
                {
                    load = "/Tiepdan/Thuongxuyen/#success";
                }
                Response.Redirect("" + load + "");
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Thêm mới lý do vụ việc trùng");
                return null;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Vuviectrung_insert2(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                TD_VUVIEC vuviecss = (TD_VUVIEC)Session[vuviectiepdanSession];


                UserInfor u_info = GetUserInfor();
                TD_VUVIEC don = new TD_VUVIEC();
                int iDonTrung = Convert.ToInt32(HashUtil.Decode_ID(fc["idtrung"], Request.Cookies["url_key"].Value));
                TD_VUVIEC dontrung = _tiepdan.Get_TDVuviecID(iDonTrung);
                don.IDINHKY = vuviecss.IDINHKY;
                don.ITIEPDOTXUAT = vuviecss.ITIEPDOTXUAT;
                don.ISONGUOI = vuviecss.ISONGUOI;
                don.DNGAYNHAN = vuviecss.DNGAYNHAN;
                don.IDIAPHUONG_0 = vuviecss.IDIAPHUONG_0;
                don.IDIAPHUONG_1 = vuviecss.IDIAPHUONG_1;
                don.CNGUOIGUI_DIACHI = vuviecss.CNGUOIGUI_DIACHI;
                don.ILANHDAOTIEP = vuviecss.ILANHDAOTIEP;
                don.CNGUOITIEP = vuviecss.CNGUOITIEP;
                don.CNGUOIGUI_TEN = vuviecss.CNGUOIGUI_TEN;
                don.ISOLUONGTRUNG = Convert.ToInt32(fc["iSoLuongTrung"]);
                don.CLYDOTRUNG = func.RemoveTagInput(fc["cLuuTheoDoi_LyDo"]);
                don.CNOIDUNG = dontrung.CNOIDUNG;
                don.DDATE = DateTime.Now;
                don.ILOAIDON = dontrung.ILOAIDON;
                don.INOIDUNG = dontrung.INOIDUNG;
                don.ITINHCHAT = dontrung.ITINHCHAT;
                don.ILINHVUC = dontrung.ILINHVUC;
                don.ITINHTRANGXULY = 0;
                don.IDONVITIEPNHAN = dontrung.IDONVITIEPNHAN;
                don.IGIAMSAT = 0;
                don.IDOANDONGNGUOI = 0;
                don.ITIEPDOTXUAT = vuviecss.ITIEPDOTXUAT;
                don.CNOIDUNGCHIDAO = dontrung.CNOIDUNGCHIDAO;
                don.IDONDOC = 0;
                don.IVUVIECTRUNG = dontrung.IVUVIEC;
                don.CYKIENGIAMSAT = "";
                don.IUSER = u_info.tk_action.iUser;
                don.IDONVI = (int)dontrung.IDONVI;
                don.INGUOIGUI_DANTOC = dontrung.INGUOIGUI_DANTOC;
                don.INGUOIGUI_QUOCTICH = dontrung.INGUOIGUI_QUOCTICH;
                _tiepdan.Insert_TDVuviec(don);
                int iUser = id_user();
                _tiepdan.Tracking_Tiepdan(id_user(), (int)don.IVUVIEC, "Lưu kiểm trùng nhập số lượng đơn trùng");
                string load = "";
                if (don.IDINHKY != 0 && don.ITIEPDOTXUAT == 0)
                {
                    load = "/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(don.IDINHKY.ToString(), Request.Cookies["url_key"].Value + "#success");
                }
                else if (don.IDINHKY == 0 && don.ITIEPDOTXUAT != 0)
                {
                    load = "/Tiepdan/Dotxuat/#success";
                }

                else
                {
                    load = "/Tiepdan/Thuongxuyen/#success";
                }
                Response.Redirect("" + load + "");
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Thêm mới lý do vụ việc trùng");
                return null;
            }
        }
        public ActionResult Ajax_Update_ketquaxuly(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....


                int iVuviec = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var thongtinvuviec = _tiepdan.Get_TDVuviecID(iVuviec);
                thongtinvuviec.ITINHTRANGXULY = 0;
                _tiepdan.Update_TDVuviec(thongtinvuviec);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "update kết quả vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Chuyenxuly(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                // ViewData["opt-donvi"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, iDonViTiepNhan);
                UserInfor u_info = GetUserInfor();
                // Dictionary<string, object> donvi = new Dictionary<string, object>();

                //donvi.Add("ICOQUAN", 4);
                //coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
                ViewData["opt-coquan"] = "<option value='4' selected> - - - Ban Dân Nguyện</option>";

                ViewData["id"] = fc["id"];
                // List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();

                //  ViewData["opt-coquan"] = tiepdan.OptionCoQuan_BaoCao(coquan);
                List<USER_CHUCVU> chucvu = _thietlap.Get_List_User_Chucvu();
                ViewData["opt-chucvu"] = tl.Option_Chucvu(chucvu, id_user());
                int iVuviec = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("tiepdan_chuyennoibo", iVuviec);
                if (_tiepdan.Get_TDVuviecID(iVuviec).CNOIDUNG == null || _tiepdan.Get_TDVuviecID(iVuviec).CNOIDUNG == "")
                {
                    return PartialView("../Ajax/Tiepdan/Capnhatnoidung");
                }
                else
                {
                    return PartialView("../Ajax/Tiepdan/Ketqua_Xuly");
                }


            }
            catch (Exception e)
            {
                //Handle Exception;
                log.Log_Error(e, "chuyển xử lý kết quả vụ việc ");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Chuyenxuly_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("tiepdan_chuyennoibo", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                file = Request.Files["file_upload1"];
                if (file != null && file.ContentLength > 0)
                {
                    CheckFile_Upload(file);
                }

                TD_VUVIEC_XULY v = new TD_VUVIEC_XULY();
                v.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                v.ICOQUANNHAN = Convert.ToInt32(fc["iChucVu"]);
                v.IUSER = u_info.tk_action.iUser;
                v.DNGAYLUU = DateTime.Now;
                if (fc["dNgayBanHanh"] == "")
                {
                    v.DNGAYXULY = DateTime.Now;
                }
                else
                {
                    v.DNGAYXULY = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayBanHanh"])));
                }
                v.CNGUOIXULY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.IVUVIEC = iDon;
                _tiepdan.Insert_TDVuviecxuly(v);
                int ivuviec = iDon;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "tiepdan_vuviec_xuly";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)v.IXULY;
                        _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                    }

                }

                //kntc.Vanban_insert(v);
                TD_VUVIEC don = _tiepdan.Get_TDVuviecID(iDon);
                don.ITINHTRANGXULY = 2;
                don.IDONDOC = Convert.ToInt32(fc["iDonDoc"]);
                don.IDONVI = Convert.ToInt32(fc["iDonVi"]);
                _tiepdan.Update_TDVuviec(don);

                KNTC_DON kntc = new KNTC_DON();
                kntc.CDIACHI_VUVIEC = "";
                kntc.CMADON = don.IVUVIEC.ToString();
                kntc.CNGUOIGUI_TEN = don.CNGUOIGUI_TEN;
                kntc.CNGUOIGUI_DIACHI = don.CNGUOIGUI_DIACHI;
                kntc.CNOIDUNG = don.CNOIDUNG;
                kntc.CNGUOIGUI_CMND = "";
                kntc.DDATE = DateTime.Now;
                kntc.DNGAYNHAN = don.DNGAYNHAN;
                kntc.IDIAPHUONG_0 = don.IDIAPHUONG_0;
                kntc.IDIAPHUONG_1 = don.IDIAPHUONG_1;
                kntc.IDIAPHUONG_2 = 0;
                kntc.IDOANDONGNGUOI = don.IDOANDONGNGUOI;
                if (don.IKNTC_DON != null && don.IKNTC_DON != 0)
                {
                    kntc.IDONTRUNG = 0;
                }
                else
                {
                    kntc.IDONTRUNG = don.IKNTC_DON;
                }
                kntc.IDONVITHULY = 0;
                kntc.IDUDIEUKIEN_KETQUA = 0;
                kntc.IDUDIEUKIEN = 0;
                kntc.ILINHVUC = (int)don.ILINHVUC;
                kntc.ILOAIDON = don.ILOAIDON;
                kntc.ILUUTHEODOI = 0;
                kntc.INGUOIGUI_DANTOC = don.INGUOIGUI_DANTOC;
                kntc.INGUOIGUI_QUOCTICH = don.INGUOIGUI_QUOCTICH;
                kntc.INGUONDON = 0;
                kntc.INOIDUNG = don.INOIDUNG;
                kntc.ISONGUOI = don.ISONGUOI;
                kntc.ITINHCHAT = don.ITINHCHAT;
                kntc.ITHULY = 0;
                kntc.IDOKHAN = 0;
                kntc.IDONVITIEPNHAN = (int)u_info.user_login.IDONVI;
                kntc.IDOMAT = 0;
                kntc.ITHAMQUYEN = 0;
                kntc.IUSER = u_info.tk_action.iUser;
                kntc.IDON = (int)u_info.user_login.IDONVI;
                kntc.IUSER_DUOCGIAOXULY = 0;
                kntc.IUSER_GIAOXULY = 0;
                kntc.ITINHTRANG_DONVIXULY = 0;
                kntc.ITINHTRANG_NOIBO = 0;
                kntc.ITINHTRANGXULY = 0;
                _tiepdan.InsertDonKNTC(kntc);
                _tiepdan.Tracking_Tiepdan((int)u_info.tk_action.iUser, (int)kntc.IDON, "Chuyển từ tiếp công dân sáng KNTC");
                int IDon_KNTC = (int)kntc.IDON;
                _condition = new Dictionary<string, object>();
                _condition.Add("ID", iDon);
                _condition.Add("CTYPE", "tiepdan_vuviec");
                var filevb = _tiepdan.LIST_FILE(_condition).ToList();
                foreach (var x in filevb)
                {
                    if (x.CFILE != "")
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_don";
                        f.CFILE = x.CFILE;
                        f.ID = (int)IDon_KNTC;
                        _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                    }
                }

                KNTC_VANBAN vb = new KNTC_VANBAN();
                vb.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                vb.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                vb.ICOQUANNHAN = 0;
                vb.IUSER = id_user();
                vb.DDATE = DateTime.Now;
                if (fc["dNgayBanHanh"] != "")
                {
                    vb.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                }
                else
                {
                    vb.DNGAYBANHANH = DateTime.Now;
                }

                vb.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                vb.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                vb.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                vb.IDON = IDon_KNTC;
                vb.CLOAI = fc["loai"];
                //vb.CCHUCVU = func.RemoveTagInput(fc["cChucVu"]);
                _kntc.Vanban_insert(vb);
                int iVanban_last = (int)vb.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanban_last;
                        _kntc.Upload_file(f);
                    }
                }
                TD_VUVIEC v_ = _tiepdan.Get_TDVuviecID((int)v.IVUVIEC);
                v_.IKNTC_DON = IDon_KNTC;
                _tiepdan.Update_TDVuviec(v_);
                //don.IDUDIEUKIEN_KETQUA = 1;
                //don.IDONVITHULY = 4;
                //don.ITHULY = 1;
                //kntc.Update_Don(don);
                int iUser = u_info.tk_action.iUser;
                _tiepdan.Tracking_Tiepdan(iUser, iDon, "Chuyển xử lý vụ việc");
                var thongtinvuviec = _tiepdan.Get_TDVuviecID((int)v.IVUVIEC);
                string load = "";
                if (thongtinvuviec.IDINHKY != 0 && don.ITIEPDOTXUAT == 0)
                {
                    Response.Redirect("/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(don.IDINHKY.ToString(), Request.Cookies["url_key"].Value) + "");
                }
                else if (don.ITIEPDOTXUAT != 0)
                {
                    Response.Redirect("/Tiepdan/Dotxuat/");
                }
                else
                {
                    Response.Redirect("/Tiepdan/Thuongxuyen/");
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "chuyển xử lý kết quả vụ việc");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Huongdanxuly(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....


                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = fc["id"];
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                ViewData["opt-coquan"] = tiepdan.OptionCoQuan_BaoCao(coquan);
                List<USER_CHUCVU> chucvu = _thietlap.Get_List_User_Chucvu();
                ViewData["opt-chucvu"] = tl.Option_Chucvu(chucvu, id_user());
                SetTokenAction("tiepdan_huongdan_traloi", iDon);
                if (_tiepdan.Get_TDVuviecID(iDon).CNOIDUNG == null || _tiepdan.Get_TDVuviecID(iDon).CNOIDUNG == "")
                {
                    return PartialView("../Ajax/Tiepdan/Capnhatnoidung");
                }
                else
                {
                    return PartialView("../Ajax/Tiepdan/Huongdan_traloi");
                }

            }
            catch (Exception e)
            {
                log.Log_Error(e, "hướng dẫn xử lý kết quả vụ việc ");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Huongdan_traloi_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();

                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("tiepdan_huongdan_traloi", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                file = Request.Files["file_upload1"];
                if (file != null && file.ContentLength > 0)
                {
                    CheckFile_Upload(file);
                }

                TD_VUVIEC_XULY v = new TD_VUVIEC_XULY();
                v.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                v.ICOQUANNHAN = Convert.ToInt32(fc["iChucVu"]);
                v.IUSER = u_info.tk_action.iUser;
                v.DNGAYLUU = DateTime.Now;
                if (fc["dNgayBanHanh"] == "")
                {
                    v.DNGAYXULY = DateTime.Now;
                }
                else
                {
                    v.DNGAYXULY = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayBanHanh"])));
                }
                v.CNGUOIXULY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.IVUVIEC = iDon;
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                _tiepdan.Insert_TDVuviecxuly(v);
                int ivuviec = (int)v.IVUVIEC;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "tiepdan_vuviec_xuly";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)v.IXULY;
                        _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                    }

                }

                //kntc.Vanban_insert(v);
                TD_VUVIEC don = _tiepdan.Get_TDVuviecID(iDon);
                don.ITINHTRANGXULY = 1;
                don.IDONDOC = Convert.ToInt32(fc["iDonDoc"]);
                _tiepdan.Update_TDVuviec(don);
                //don.IDUDIEUKIEN_KETQUA = 1;
                //don.IDONVITHULY = 4;
                //don.ITHULY = 1;
                //kntc.Update_Don(don);
                int iUser = u_info.tk_action.iUser;
                _tiepdan.Tracking_Tiepdan(iUser, iDon, "Hướng dẫn trã lời vụ việc");
                var thongtinvuviec = _tiepdan.Get_TDVuviecID((int)v.IVUVIEC);
                //  string load = "";
                if (thongtinvuviec.IDINHKY != 0 && don.ITIEPDOTXUAT == 0)
                {
                    Response.Redirect("/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(don.IDINHKY.ToString(), Request.Cookies["url_key"].Value) + "");
                }
                else if (don.ITIEPDOTXUAT != 0)
                {
                    Response.Redirect("/Tiepdan/Dotxuat/");
                }
                else
                {
                    Response.Redirect("/Tiepdan/Thuongxuyen/");
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "hướng dẫn xử lý kết quả vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Chuyenxuly_noibo(string id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....


                // new id
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                ViewData["opt-coquan"] = tiepdan.OptionCoQuan_BaoCao(coquan);
                List<USER_CHUCVU> chucvu = _thietlap.Get_List_User_Chucvu();
                ViewData["opt-chucvu"] = tl.Option_Chucvu(chucvu, id_user());
                SetTokenAction("tiepdan_chuyenxulynoibo", iDon);
                // end

                ViewData["id"] = id;
                if (_tiepdan.Get_TDVuviecID(iDon).CNOIDUNG == null || _tiepdan.Get_TDVuviecID(iDon).CNOIDUNG == "")
                {
                    return PartialView("../Ajax/Tiepdan/Capnhatnoidung");
                }
                else
                {
                    return PartialView("../Ajax/Tiepdan/Ketqua_XulyCQBH");
                }

            }
            catch (Exception e)
            {
                log.Log_Error(e, "chuyển xử lý nội bộ kết quả vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_HuongDanTrucTiep(string id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                SetTokenAction("tiepdan_huongdantructiep", iDon);
                ViewData["id"] = id;
                if (_tiepdan.Get_TDVuviecID(iDon).CNOIDUNG == null || _tiepdan.Get_TDVuviecID(iDon).CNOIDUNG == "")
                {
                    return PartialView("../Ajax/Tiepdan/Capnhatnoidung");
                }
                else
                {
                    return PartialView("../Ajax/Tiepdan/Huongdantructiep");
                }
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Hướng dẫn trực tiếp kết quả vụ việc ");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Chuyenxuly_noibo_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                //....


                UserInfor u_info = GetUserInfor();
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("tiepdan_chuyenxulynoibo", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                file = Request.Files["file_upload1"];
                if (file != null && file.ContentLength > 0)
                {
                    CheckFile_Upload(file);
                }

                TD_VUVIEC_XULY v = new TD_VUVIEC_XULY();
                v.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                v.ICOQUANNHAN = Convert.ToInt32(fc["iChucVu"]);

                v.IUSER = u_info.tk_action.iUser;
                v.DNGAYLUU = DateTime.Now;
                if (fc["dNgayBanHanh"] == "")
                {
                    v.DNGAYXULY = DateTime.Now;
                }
                else
                {
                    v.DNGAYXULY = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayBanHanh"])));
                }
                v.CNGUOIXULY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CKINHGUI = func.RemoveTagInput(fc["CKINHGUI"]);
                v.CNOINHAN = func.RemoveTagInput(fc["CNOINHAN"]);
                v.IVUVIEC = iDon;
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                _tiepdan.Insert_TDVuviecxuly(v);
                int ivuviec = (int)v.IVUVIEC;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "tiepdan_vuviec_xuly";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)v.IXULY;
                        _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                    }

                }


                //kntc.Vanban_insert(v);
                TD_VUVIEC don = _tiepdan.Get_TDVuviecID(iDon);
                don.ITINHTRANGXULY = 3;
                //  don.IDONVI = Convert.ToInt32(fc["iDonVi"]);
                don.IDONDOC = Convert.ToInt32(fc["iDonDoc"]);
                _tiepdan.Update_TDVuviec(don);
                int iUser = u_info.tk_action.iUser;
                _tiepdan.Tracking_Tiepdan(iUser, iDon, "Chuyển xử lý nội bộ");
                var thongtinvuviec = _tiepdan.Get_TDVuviecID((int)v.IVUVIEC);
                string load = "";
                if (thongtinvuviec.IDINHKY != 0 && don.ITIEPDOTXUAT == 0)
                {
                    Response.Redirect("/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(don.IDINHKY.ToString(), Request.Cookies["url_key"].Value) + "");
                }
                else if (don.ITIEPDOTXUAT != 0)
                {
                    Response.Redirect("/Tiepdan/Dotxuat/");
                }
                else
                {
                    Response.Redirect("/Tiepdan/Thuongxuyen/");
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "chuyển xử lý nội bộ kết quả vụ việc ");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_HuongDanTrucTiep_insert(string id)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(id.ToString(), Request.Cookies["url_key"].Value));
                TD_VUVIEC don = _tiepdan.Get_TDVuviecID(iDon);
                don.ITINHTRANGXULY = 4;
                _tiepdan.Update_TDVuviec(don);

                if (don.IDINHKY != 0 && don.ITIEPDOTXUAT == 0)
                {
                    Response.Redirect("/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(don.IDINHKY.ToString(), Request.Cookies["url_key"].Value) + "");
                }
                else if (don.ITIEPDOTXUAT != 0)
                {
                    Response.Redirect("/Tiepdan/Dotxuat/");
                }
                else
                {
                    Response.Redirect("/Tiepdan/Thuongxuyen/");
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Hướng dẫn trực tiếp");
                //return null;
                throw;
            }


        }
        public ActionResult Ajax_Checkkiemtratrung()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                int ivuviec = Convert.ToInt32(HashUtil.Decode_ID(Request["id"]));
                int kiemtra = 0;
                var thongtinvuviec = _tiepdan.Get_TDVuviecID(ivuviec);
                if (thongtinvuviec.IVUVIECTRUNG == null && thongtinvuviec.ITINHTRANGXULY == 0)
                {
                    kiemtra = 1;
                }
                Response.Write(kiemtra);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "kiểm tra trùng vụ việc ");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Delete_VanBan_file(string id)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                //....
                int iflie = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                var thongtinfile = _tiepdan.Get_ByIDFILE(iflie);
                _tiepdan.DELETE_FILE(thongtinfile);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "xóa file vụ việc ");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_Vuviec_result(FormCollection fc)// Tra cứu tổng
        {
            //....
            if (!CheckAuthToken()) { return null; }
            try
            {

                UserInfor u_info = GetUserInfor();
                if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                TD_VUVIEC vuviec = new TD_VUVIEC();
                int iUser = 0;
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                }

                int loaivuviec = Convert.ToInt32(fc["iLoaiVuViec"]);
                vuviec.IDINHKY = loaivuviec;
                string dTuNgay = fc["dTuNgay"].ToString();
                string dDenNgay = fc["dDenNgay"].ToString();
                if (fc["dTuNgay"].ToString() != "")
                {
                    dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));

                }
                if (fc["dDenNgay"].ToString() != "")
                {
                    dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));

                }
                if (fc["iDoan"] != null) { vuviec.IDOANDONGNGUOI = Convert.ToInt32(Request["iDoan"]); }
                else
                {
                    vuviec.IDOANDONGNGUOI = 0;
                }


                int id_don = Convert.ToInt32(Request["iDoan"]);
                if (Convert.ToInt32(fc["iDonVi"]) != -1) { vuviec.IDONVI = Convert.ToInt32(Request["iDonVi"]); }
                else { vuviec.IDONVI = -1; }
                if (fc["cNguoiGui_Ten"] != "" && fc["cNguoiGui_Ten"] != "<!-- " && fc["cNguoiGui_Ten"] != "<? ?>") { vuviec.CNGUOIGUI_TEN = func.RemoveTagInput(Request["cNguoiGui_Ten"]); }
                if (fc["cNguoiGui_DiaChi"] != "" && fc["cNguoiGui_DiaChi"] != "<!-- " && fc["cNguoiGui_DiaChi"] != "<? ?>") { vuviec.CNGUOIGUI_DIACHI = func.RemoveTagInput(Request["cNguoiGui_DiaChi"]); }
                if (fc["cNoiDung"] != "" && fc["cNoiDung"] != "<!-- " && fc["cNoiDung"] != "<? ?>") { vuviec.CNOIDUNG = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Convert.ToInt32(fc["iLoai"]) != -1) { vuviec.ILOAIDON = Convert.ToInt32(func.RemoveTagInput(Request["iLoai"])); }
                if (Convert.ToInt32(fc["iLinhVuc"]) != -1) { vuviec.ILINHVUC = Convert.ToInt32(func.RemoveTagInput(Request["iLinhVuc"])); }
                if (Convert.ToInt32(fc["iNoiDung"]) != -1) { vuviec.INOIDUNG = Convert.ToInt32(func.RemoveTagInput(Request["iNoiDung"])); }
                if (Convert.ToInt32(fc["iTinhChat"]) != -1) { vuviec.ITINHCHAT = Convert.ToInt32(func.RemoveTagInput(Request["iTinhChat"])); }
                if (Convert.ToInt32(fc["iHinhThuc"]) != -1) { vuviec.ITINHTRANGXULY = Convert.ToInt32(func.RemoveTagInput(Request["iHinhThuc"])); }
                if (fc["ikiemtrung"] != null) { vuviec.IVUVIECTRUNG = Convert.ToInt32(fc["ikiemtrung"]); }
                string d = fc["ikiemtrung"];
                ViewData["list"] = tiepdan.Dinhky_vuviec_search_dinhky(vuviec, dTuNgay, dDenNgay, iUser);
                return PartialView("../Ajax/Tiepdan/Vuviec_result");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tra cứu vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Dinhky_result_tracuu(FormCollection fc)
        {

            if (!CheckAuthToken()) { return null; }
            try
            {

                TD_VUVIEC vuviec = new TD_VUVIEC();
                UserInfor u_info = GetUserInfor();
                int idonvi = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                int loaivuviec = Convert.ToInt32(fc["iLoaiVuViec"]);
                vuviec.IDINHKY = loaivuviec;
                string dTuNgay = fc["dTuNgay"].ToString();
                string dDenNgay = fc["dDenNgay"].ToString();
                if (fc["dTuNgay"].ToString() != "")
                {
                    dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));

                }
                if (fc["dDenNgay"].ToString() != "")
                {
                    dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));

                }
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    vuviec.IUSER = iUser;
                }
                if (fc["iDoan"] != null) { vuviec.IDOANDONGNGUOI = Convert.ToInt32(Request["iDoan"]); }
                if (Convert.ToInt32(fc["iDonVi"]) != -1) { vuviec.IDONVI = Convert.ToInt32(Request["iDonVi"]); idonvi = Convert.ToInt32(Request["iDonVi"]); }
                else { vuviec.IDONVI = -1; }
                if (fc["cNguoiGui_Ten"] != "") { vuviec.CNGUOIGUI_TEN = func.RemoveTagInput(Request["cNguoiGui_Ten"]); }
                if (fc["cNguoiGui_DiaChi"] != "") { vuviec.CNGUOIGUI_DIACHI = func.RemoveTagInput(Request["cNguoiGui_DiaChi"]); }
                if (fc["cNoiDung"] != "") { vuviec.CNOIDUNG = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Convert.ToInt32(fc["iLoai"]) != -1) { vuviec.ILOAIDON = Convert.ToInt32(func.RemoveTagInput(Request["iLoai"])); }
                if (Convert.ToInt32(fc["iLinhVuc"]) != -1) { vuviec.ILINHVUC = Convert.ToInt32(func.RemoveTagInput(Request["iLinhVuc"])); }
                if (Convert.ToInt32(fc["iNoiDung"]) != -1) { vuviec.INOIDUNG = Convert.ToInt32(func.RemoveTagInput(Request["iNoiDung"])); }
                if (Convert.ToInt32(fc["iTinhChat"]) != -1) { vuviec.ITINHCHAT = Convert.ToInt32(func.RemoveTagInput(Request["iTinhChat"])); }
                if (Convert.ToInt32(fc["iHinhThuc"]) != -1) { vuviec.ITINHTRANGXULY = Convert.ToInt32(func.RemoveTagInput(Request["iHinhThuc"])); }
                if (fc["ikiemtrung"] != null) { vuviec.IVUVIECTRUNG = Convert.ToInt32(fc["ikiemtrung"]); }
                string d = fc["ikiemtrung"];


                List<TD_VUVIEC> thongtinvuviec = _tiepdan.Get_search_dinhky(vuviec, dTuNgay, dDenNgay);
                List<TD_VUVIEC_XULY> xuly = _tiepdan.Get_list_TDVuviecxuly();
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    thongtinvuviec = _tiepdan.Get_search_dinhky(vuviec, dTuNgay, dDenNgay).Where(x => x.IUSER == iUser).ToList();
                }
                if (thongtinvuviec.Count() != 0)
                {
                    ViewData["list"] = tiepdan.Dinhky_vuviec(thongtinvuviec, xuly, iUser, id, Request.Cookies["url_key"].Value, idonvi, 1);
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='8' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào!</td></tr>";
                }
                //ViewData["list"] = tiepdan.Dinhky_vuviec(thongtinvuviec, xuly, iUser, id, Request.Cookies["url_key"].Value, idonvi);
                return PartialView("../Ajax/Tiepdan/Dinhky_result");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tra cứu vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Thuongxuyen_result_tracuu(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                TD_VUVIEC vuviec = new TD_VUVIEC();
                int madonvi = (int)u_info.user_login.IDONVI;
                int loaivuviec = Convert.ToInt32(fc["iLoaiVuViec"]);
                vuviec.IDINHKY = 0;
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    vuviec.IUSER = iUser;
                }
                string dTuNgay = fc["dTuNgay"].ToString();
                string dDenNgay = fc["dDenNgay"].ToString();
                if (fc["dTuNgay"].ToString() != "")
                {
                    dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));

                }
                if (fc["dDenNgay"].ToString() != "")
                {
                    dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));

                }
                if (fc["iDoan"] != null) { vuviec.IDOANDONGNGUOI = Convert.ToInt32(Request["iDoan"]); }
                else
                {
                    vuviec.IDOANDONGNGUOI = 0;
                }
                int id_don = Convert.ToInt32(Request["iDoan"]);
                if (Convert.ToInt32(fc["iDonVi"]) != -1) { vuviec.IDONVI = Convert.ToInt32(Request["iDonVi"]); madonvi = Convert.ToInt32(Request["iDonVi"]); }
                else { vuviec.IDONVI = -1; }
                if (fc["cNguoiGui_Ten"] != "") { vuviec.CNGUOIGUI_TEN = func.RemoveTagInput(Request["cNguoiGui_Ten"]); }
                if (fc["cNguoiGui_DiaChi"] != "") { vuviec.CNGUOIGUI_DIACHI = func.RemoveTagInput(Request["cNguoiGui_DiaChi"]); }
                if (fc["cNoiDung"] != "" && fc["cNoiDung"] != "<!-- " && fc["cNoiDung"] != "<? ?>") { vuviec.CNOIDUNG = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Convert.ToInt32(fc["iLoai"]) != -1) { vuviec.ILOAIDON = Convert.ToInt32(func.RemoveTagInput(Request["iLoai"])); }
                if (Convert.ToInt32(fc["iLinhVuc"]) != -1) { vuviec.ILINHVUC = Convert.ToInt32(func.RemoveTagInput(Request["iLinhVuc"])); }
                if (Convert.ToInt32(fc["iNoiDung"]) != -1) { vuviec.INOIDUNG = Convert.ToInt32(func.RemoveTagInput(Request["iNoiDung"])); }
                if (Convert.ToInt32(fc["iTinhChat"]) != -1) { vuviec.ITINHCHAT = Convert.ToInt32(func.RemoveTagInput(Request["iTinhChat"])); }
                if (Convert.ToInt32(fc["iHinhThuc"]) != -1) { vuviec.ITINHTRANGXULY = Convert.ToInt32(func.RemoveTagInput(Request["iHinhThuc"])); }
                if (fc["ikiemtrung"] != null) { vuviec.IVUVIECTRUNG = Convert.ToInt32(fc["ikiemtrung"]); }
                string d = fc["ikiemtrung"];
                List<TD_VUVIEC> thongtinvuviec = _tiepdan.Get_search_dinhky(vuviec, dTuNgay, dDenNgay);
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    thongtinvuviec = _tiepdan.Get_search_dinhky(vuviec, dTuNgay, dDenNgay).Where(x => x.IUSER == iUser).ToList();
                }
                List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                if (thongtinvuviec.Count() != 0)
                {
                    ViewData["list"] = tiepdan.Thuongxuyen_vuviec(thongtinvuviec, Xuly, (int)u_info.tk_action.iUser, Request.Cookies["url_key"].Value, (int)madonvi, 1);
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='8' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào!</td></tr>";
                }

                return PartialView("../Ajax/Tiepdan/Thuongxuyen_result");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tra cứu vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Dotxuat_result_tracuu(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                if (!ba_se.ActionMulty_Redirect_("53", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                TD_VUVIEC vuviec = new TD_VUVIEC();
                int madonvi = (int)u_info.user_login.IDONVI;
                int loaivuviec = Convert.ToInt32(fc["iLoaiVuViec"]);
                vuviec.IDINHKY = 0;
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    vuviec.IUSER = iUser;
                }
                string dTuNgay = fc["dTuNgay"].ToString();
                string dDenNgay = fc["dDenNgay"].ToString();
                if (fc["dTuNgay"].ToString() != "")
                {
                    dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));

                }
                if (fc["dDenNgay"].ToString() != "")
                {
                    dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));

                }
                if (fc["iDoan"] != null) { vuviec.IDOANDONGNGUOI = Convert.ToInt32(Request["iDoan"]); }
                else
                {
                    vuviec.IDOANDONGNGUOI = 0;
                }
                int id_don = Convert.ToInt32(Request["iDoan"]);
                if (Convert.ToInt32(fc["iDonVi"]) != -1) { vuviec.IDONVI = Convert.ToInt32(Request["iDonVi"]); madonvi = Convert.ToInt32(Request["iDonVi"]); }
                else { vuviec.IDONVI = -1; }
                if (fc["cNguoiGui_Ten"] != "") { vuviec.CNGUOIGUI_TEN = func.RemoveTagInput(Request["cNguoiGui_Ten"]); }
                if (fc["cNguoiGui_DiaChi"] != "") { vuviec.CNGUOIGUI_DIACHI = func.RemoveTagInput(Request["cNguoiGui_DiaChi"]); }
                if (fc["cNoiDung"] != "" && fc["cNoiDung"] != "<!-- " && fc["cNoiDung"] != "<? ?>") { vuviec.CNOIDUNG = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Convert.ToInt32(fc["iLoai"]) != -1) { vuviec.ILOAIDON = Convert.ToInt32(func.RemoveTagInput(Request["iLoai"])); }
                if (Convert.ToInt32(fc["iLinhVuc"]) != -1) { vuviec.ILINHVUC = Convert.ToInt32(func.RemoveTagInput(Request["iLinhVuc"])); }
                if (Convert.ToInt32(fc["iNoiDung"]) != -1) { vuviec.INOIDUNG = Convert.ToInt32(func.RemoveTagInput(Request["iNoiDung"])); }
                if (Convert.ToInt32(fc["iTinhChat"]) != -1) { vuviec.ITINHCHAT = Convert.ToInt32(func.RemoveTagInput(Request["iTinhChat"])); }
                if (Convert.ToInt32(fc["iHinhThuc"]) != -1) { vuviec.ITINHTRANGXULY = Convert.ToInt32(func.RemoveTagInput(Request["iHinhThuc"])); }
                if (fc["ikiemtrung"] != null) { vuviec.IVUVIECTRUNG = Convert.ToInt32(fc["ikiemtrung"]); }
                string d = fc["ikiemtrung"];
                List<TD_VUVIEC> thongtinvuviec = _tiepdan.Get_search_dotxuat(vuviec, dTuNgay, dDenNgay);
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    thongtinvuviec = _tiepdan.Get_search_dotxuat(vuviec, dTuNgay, dDenNgay).Where(x => x.IUSER == iUser).ToList();
                }
                List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                if (thongtinvuviec.Count() != 0)
                {
                    ViewData["list"] = tiepdan.Thuongxuyen_vuviec(thongtinvuviec, Xuly, (int)u_info.tk_action.iUser, Request.Cookies["url_key"].Value, (int)madonvi, 1);
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='8' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào!</td></tr>";
                }

                return PartialView("../Ajax/Tiepdan/Thuongxuyen_result");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tra cứu vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Traloi_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....


                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var thongtinxuly = _tiepdan.Get_TDVuviecxulyID(id);
                _tiepdan.Delete_TDVuviec(thongtinxuly);
                _tiepdan.Tracking_Tiepdan(id_user(), id, "Xóa vụ việc tiếp dân");
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Xóa trả lời  vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Traloi_vuviec(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....


                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = fc["id"];
                var thongtintraloi = _tiepdan.Get_TDVuviecxulyID(id);
                SetTokenAction("tiepdan_traloi_edit", id);
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                ViewData["opt-coquan"] = "<option value='" + (int)thongtintraloi.ICOQUANBANHANH + "' selected> - - - " + _thietlap.Get_Quochoi_Coquan((int)thongtintraloi.ICOQUANBANHANH).CTEN + " </option>";
                ViewData["Socongvan"] = thongtintraloi.CSOVANBAN;
                ViewData["NgayBanHanh"] = func.ConvertDateVN(thongtintraloi.DNGAYXULY.ToString());
                ViewData["NoiDung"] = thongtintraloi.CNOIDUNG;
                ViewData["NguoiXyly"] = thongtintraloi.CNGUOIXULY;
                List<USER_CHUCVU> chucvu = _thietlap.Get_List_User_Chucvu();
                ViewData["opt-chucvu"] = tl.Option_Chucvu(chucvu, id_user(), (int)thongtintraloi.ICOQUANNHAN);
                ViewData["XoaFile"] = read_fileVB_xuly((int)id);
                var thongtinvuviec = _tiepdan.Get_TDVuviecID((int)thongtintraloi.IVUVIEC);
                if (thongtinvuviec.ITINHTRANGXULY == 2)
                {
                    ViewData["ten"] = "Cơ quan ban hành  <i class='f-red'>*</i>";
                }
                else
                {
                    ViewData["ten"] = "Cơ quan nhận <i class='f-red'>*</i>";
                }
                if (thongtinvuviec.ITINHTRANGXULY == 3)
                {
                    ViewData["KinhGui"] = "<div class='control-group'>" +
                    "<label for='textfield' class='control-label'>Kính gửi</label>" +
                                    "<div class='controls'>" + thongtintraloi.CKINHGUI + "</div></div>";
                    ViewData["NoiNhan"] = "<div class='control-group'>" +
                    "<label for='textfield' class='control-label'>Nơi nhận</label>" +
                                    "<div class='controls'>" + thongtintraloi.CNOINHAN + "</div></div>";
                }
                ViewData["thongtinvuviec"] = thongtinvuviec;
                return PartialView("../Ajax/Tiepdan/Traloivuviec_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " thêm trả lời  vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Traloi_Noidung(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....         
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = fc["id"];
                var thongtintraloi = _tiepdan.Get_TDVuviecxulyID(id);
                SetTokenAction("tiepdan_traloi_edit", id);
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                ViewData["opt-coquan"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, 0);
                ViewData["Socongvan"] = thongtintraloi.CSOVANBAN;
                ViewData["tencoquan"] = _thietlap.GetBy_Quochoi_CoquanID((int)thongtintraloi.ICOQUANBANHANH).CTEN;
                ViewData["NgayBanHanh"] = func.ConvertDateVN(thongtintraloi.DNGAYXULY.ToString());
                ViewData["NoiDung"] = thongtintraloi.CNOIDUNG;
                ViewData["NguoiXyly"] = thongtintraloi.CNGUOIXULY;
                ViewData["File"] = tiepdan.File_View((int)id, "tiepdan_vuviec_xuly");
                ViewData["XoaFile"] = read_fileVB_xuly((int)id);


                return PartialView("../Ajax/Tiepdan/Noidung");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "trả lời  vụ việc ");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Traloi_Update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....


                UserInfor u_info = GetUserInfor();
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("tiepdan_traloi_edit", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                file = Request.Files["file_upload1"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/"); return null; }
                }

                TD_VUVIEC_XULY v = _tiepdan.Get_TDVuviecxulyID(iDon);
                v.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                v.ICOQUANNHAN = Convert.ToInt32(fc["iChucVu"]);
                v.DNGAYXULY = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayBanHanh"])));
                v.CNGUOIXULY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                int ivuviec = (int)v.IVUVIEC;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "tiepdan_vuviec_xuly";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)v.IXULY;
                        _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                    }

                }
                _tiepdan.Update_TDVuviecxuly(v);
                int iUser = u_info.tk_action.iUser;
                _tiepdan.Tracking_Tiepdan(iUser, iDon, "sửa trả lời vụ việc");
                var thongtinvuviec = _tiepdan.Get_TDVuviecID((int)v.IVUVIEC);
                thongtinvuviec.IDONDOC = Convert.ToInt32(fc["iDonDoc"]);
                _tiepdan.Update_TDVuviec(thongtinvuviec);
                if (thongtinvuviec.IDINHKY != 0)
                {
                    Response.Redirect("/Tiepdan/Dinhky_vuviec/?id=" + HashUtil.Encode_ID(thongtinvuviec.IDINHKY.ToString(), Request.Cookies["url_key"].Value) + "");
                }
                else
                {
                    Response.Redirect("/Tiepdan/Thuongxuyen");
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Chỉnh sửa trả lời  vụ việc ");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_Dinhky_search(string id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;


                //List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                //ViewData["opt-donvi"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, iDonViTiepNhan);
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                // ViewData["opt-donvi"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, iDonViTiepNhan);

                Dictionary<string, object> donvi = new Dictionary<string, object>();

                donvi.Add("ICOQUAN", iDonViTiepNhan);
                coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
                ViewData["opt-donvi"] = "<option value='" + iDonViTiepNhan + "'>" + u_info.tk_action.tendonvi + "</option>";


                //List<KNTC_LOAIDON> loaidon = _tiepdan.HienThiDanhSachLoaiDon();
                // List<KNTC_NOIDUNGDON> noidung = _thietlap.Get_Noidungdon();
                // List<KNTC_TINHCHAT> tinhchat = _thietlap.Get_Tinhchat();
                //  List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                ViewData["id"] = id;

                return PartialView("../Ajax/Tiepdan/Dinhky_search");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm  vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Thuongxuyen_search()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;


                //List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                //ViewData["opt-donvi"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, iDonViTiepNhan);

                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                // ViewData["opt-donvi"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, iDonViTiepNhan);

                Dictionary<string, object> donvi = new Dictionary<string, object>();
                donvi.Add("ICOQUAN", iDonViTiepNhan);
                coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
                ViewData["opt-donvi"] = "<option value='" + iDonViTiepNhan + "'>" + u_info.tk_action.tendonvi + "</option>";


                //  List<KNTC_LOAIDON> loaidon = _tiepdan.HienThiDanhSachLoaiDon();
                //   List<KNTC_NOIDUNGDON> noidung = _thietlap.Get_Noidungdon();
                //  List<KNTC_TINHCHAT> tinhchat = _thietlap.Get_Tinhchat();
                //  List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                return PartialView("../Ajax/Tiepdan/Thuongxuyen_search");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm  vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Dotxuat_search()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;


                //List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                //ViewData["opt-donvi"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, iDonViTiepNhan);

                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                // ViewData["opt-donvi"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, iDonViTiepNhan);

                Dictionary<string, object> donvi = new Dictionary<string, object>();

                donvi.Add("ICOQUAN", iDonViTiepNhan);
                coquan = _thietlap.GetBy_List_Quochoi_Coquan(donvi);
                ViewData["opt-donvi"] = "<option value='" + iDonViTiepNhan + "'>" + u_info.tk_action.tendonvi + "</option>";


                //List<KNTC_LOAIDON> loaidon = _tiepdan.HienThiDanhSachLoaiDon();
                // List<KNTC_NOIDUNGDON> noidung = _thietlap.Get_Noidungdon();
                //  List<KNTC_TINHCHAT> tinhchat = _thietlap.Get_Tinhchat();
                //  List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                return PartialView("../Ajax/Tiepdan/Dotxuat_search");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm  vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_TimKiemDinhKy_result(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                Response.Write(tiepdan.Tiepdan_dinhky(id_user(), id));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm vụ việc");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_DiaDiemTiep()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                UserInfor u_info = GetUserInfor();
                int idonvi = (int)u_info.user_login.IDONVI;
                string str = "";
                string ctentimkiem = func.RemoveTagInput(Request["search"].ToUpper().Trim());
                var thongtintimkiem = _tiepdan.Get_search_tendinhky(ctentimkiem);
                int dem = 1;

                int kiemtra = 0;
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    kiemtra = 1;
                }
                foreach (var x in thongtintimkiem)
                {
                    str += tiepdan.Tiepdan_dinhky_search((int)u_info.tk_action.iUser, Request.Cookies["url_key"].Value, (int)x.IDINHKY, (int)idonvi, dem, kiemtra);
                    dem++;
                }
                Response.Write(str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm  địa điểm tiếp vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_TimKiemnoidungvuviec_result()
        {
            //....
            if (!CheckAuthToken()) { return null; }
            try
            {

                UserInfor u_info = GetUserInfor();
                string str = "";
                int idonvi = (int)u_info.user_login.IDONVI;
                int iuser = (int)u_info.user_login.IUSER;
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                string ctentimkiem = func.RemoveTagInput(Request["search"].ToUpper().Trim());
                var thongtintimkiem = _tiepdan.Get_search_noidungvuviec(ctentimkiem).Where(x => x.IDINHKY == id && x.IDONVI == idonvi).ToList();
                int dem = 1;
                List<TD_VUVIEC> vuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDINHKY == id && x.IDONVI == idonvi).ToList();
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    vuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDINHKY == id && x.IUSER == iuser && x.IDONVI == idonvi).ToList();
                    thongtintimkiem = _tiepdan.Get_search_noidungvuviec(ctentimkiem).Where(x => x.IDINHKY == id && x.IUSER == iuser && x.IDONVI == idonvi).ToList();
                }
                if (u_info.tk_action.is_admin)
                {
                    vuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDINHKY == id).ToList();
                    thongtintimkiem = _tiepdan.Get_search_noidungvuviec(ctentimkiem).Where(x => x.IDINHKY == id).ToList();
                }
                List<TD_VUVIEC_XULY> xuly = _tiepdan.Get_list_TDVuviecxuly();
                foreach (var x in thongtintimkiem)
                {
                    str += tiepdan.Dinhky_vuviec_search(vuviec, xuly, id_user(), id, Request.Cookies["url_key"].Value, dem, (int)x.IVUVIEC, (int)idonvi);
                    dem++;
                }
                string thongbao = "<tr><td colspan='8' class='alert tcenter alert-info'>Có " + (dem - 1) + " kết quả tìm kiếm</td></tr>";
                if (thongtintimkiem.Count() == 0)
                {
                    thongbao = "<tr><td colspan='8' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào</td></tr>";
                }
                Response.Write("" + thongbao + "" + str);

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm nội dung  vụ việc ");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_TimKiemnoidungthuonxuyen_result()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                string str = "";
                int idonvi = (int)u_info.user_login.IDONVI;
                int iuser = (int)u_info.user_login.IUSER;
                string ctentimkiem = func.RemoveTagInput(Request["search"].ToUpper().Trim());
                var thongtintimkiem = _tiepdan.Get_search_noidungvuviec(ctentimkiem).Where(x => x.IDINHKY == 0 && x.IDONVI == idonvi).ToList();
                List<TD_VUVIEC> vuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDINHKY == 0 && x.IDONVI == idonvi).ToList();
                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    vuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDINHKY == 0 && x.IUSER == iuser && x.IDONVI == idonvi).ToList();
                    thongtintimkiem = _tiepdan.Get_search_noidungvuviec(ctentimkiem).Where(x => x.IDINHKY == 0 && x.IUSER == iuser && x.IDONVI == idonvi).ToList();
                }
                if (u_info.tk_action.is_admin)
                {
                    vuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDINHKY == 0).ToList();
                    thongtintimkiem = _tiepdan.Get_search_noidungvuviec(ctentimkiem).Where(x => x.IDINHKY == 0).ToList();
                }
                List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                int dem = 1;
                foreach (var x in thongtintimkiem)
                {
                    str += tiepdan.Thuongxuyen_vuviec_search(vuviec, Xuly, iuser, Request.Cookies["url_key"].Value, dem, (int)x.IVUVIEC, idonvi);
                    dem++;
                }
                string thongbao = "<tr><td colspan='8' class='alert tcenter alert-info'>Có " + (dem - 1) + " kết quả tìm kiếm</td></tr>";
                if (thongtintimkiem.Count() == 0)
                {
                    thongbao = "<tr><td colspan='8' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào</td></tr>";
                }
                Response.Write("" + thongbao + "" + str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm nội dung vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Traloixuly()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                ViewData["id_vuviec"] = Request["id"];
                ViewData["list"] = tiepdan.Vanban_traloi_vuviec_chuyenxuly(id, Request.Cookies["url_key"].Value);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Trả lời xử lý vụ việc");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_TraLoi_ChuyenXuLyVuViec(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                SetTokenAction("tiepdan_themtraloi");
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                _condition = new Dictionary<string, object>();
                _condition.Add("IVUVIEC", id);
                int id_xuly = (int)_tiepdan.Get_TDVuviecxuly(_condition).FirstOrDefault().IXULY;
                string id_encr = HashUtil.Encode_ID(id_xuly.ToString(), Request.Cookies["url_key"].Value);
                var thongtintraloi = _tiepdan.Get_TDVuviecxulyID(id_xuly);
                SetTokenAction("tiepdan_traloi_edit", id);
                ViewData["opt-coquan"] = _thietlap.GetBy_Quochoi_CoquanID((int)thongtintraloi.ICOQUANBANHANH).CTEN;
                List<USER_CHUCVU> chucvu = _thietlap.Get_List_User_Chucvu();
                ViewData["opt-chucvu"] = tl.Option_Chucvu(chucvu, id_user());
                ViewData["id"] = id_encr;
                ViewData["iVuViec"] = id;
                ViewData["iDonVi"] = (int)thongtintraloi.ICOQUANBANHANH;
                return PartialView("../Ajax/Tiepdan/Traloichuyenxuly");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trả lời xử lý vụ việc");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_traloichuyenxuly_insert(FormCollection fc, HttpPostedFileBase file)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();

                if (!CheckTokenAction("tiepdan_themtraloi")) { Response.Redirect("/Home/Error/"); return null; }
                file = Request.Files["file_upload1"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/"); return null; }
                }

                TD_VUVIEC_XULY v = new TD_VUVIEC_XULY();
                v.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                v.ICOQUANNHAN = Convert.ToInt32(fc["iChucVu"]);
                v.IVUVIEC = Convert.ToInt32(fc["iVuViec"]);
                v.DNGAYXULY = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayBanHanh"])));
                v.CNGUOIXULY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.CLOAI = "traloichuyenxuly";
                _tiepdan.Insert_TDVuviecxuly(v);

                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "tiepdan_vuviec_xuly_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)v.IXULY;
                        _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                    }

                }

                //kntc.Vanban_insert(v);

                //don.IDUDIEUKIEN_KETQUA = 1;
                //don.IDONVITHULY = 4;
                //don.ITHULY = 1;
                //kntc.Update_Don(don);
                int iUser = u_info.tk_action.iUser;
                _tiepdan.Tracking_Tiepdan(iUser, (int)v.IVUVIEC, "trả lời chuyển xử lý vụ việc");
                var thongtinvuviec = _tiepdan.Get_TDVuviecID((int)v.IVUVIEC);
                string load = "";
                Response.Redirect("/Tiepdan/Traloixuly/?id=" + HashUtil.Encode_ID(thongtinvuviec.IVUVIEC.ToString(), Request.Cookies["url_key"].Value) + "");
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm trả lời xử lý vụ việc");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_TraLoi_ChuyenXuLyVuViec_edit(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = fc["id"];
                var thongtintraloi = _tiepdan.Get_TDVuviecxulyID(id);
                SetTokenAction("suatraloi_vuviecchuyenxuly", id);
                ViewData["opt-coquan"] = _thietlap.GetBy_Quochoi_CoquanID((int)thongtintraloi.ICOQUANBANHANH).CTEN;
                List<USER_CHUCVU> chucvu = _thietlap.Get_List_User_Chucvu();
                ViewData["opt-chucvu"] = tl.Option_Chucvu(chucvu, id_user(), (int)thongtintraloi.ICOQUANNHAN);
                // ViewData["Socongvan"] = thongtintraloi.CSOVANBAN;
                ViewData["NgayBanHanh"] = func.ConvertDateVN(thongtintraloi.DNGAYXULY.ToString());
                ViewData["NoiDung"] = thongtintraloi.CNOIDUNG;
                ViewData["XoaFile"] = read_fileVB_xuly_traloi((int)id);
                ViewData["SoCongVan"] = thongtintraloi.CSOVANBAN;
                ViewData["nguoiky"] = thongtintraloi.CNGUOIXULY;

                return PartialView("../Ajax/Tiepdan/Traloichuyenxuly_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa trả lời xử lý vụ việc ");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_traloichuyenxuly_Update(FormCollection fc, HttpPostedFileBase file)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("suatraloi_vuviecchuyenxuly", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                file = Request.Files["file_upload1"];
                if (file != null && file.ContentLength > 0)
                {
                    CheckFile_Upload(file);
                }

                TD_VUVIEC_XULY v = _tiepdan.Get_TDVuviecxulyID(iDon);
                v.DNGAYXULY = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayBanHanh"])));
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.CNGUOIXULY = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cNguoiKy"]);
                v.ICOQUANNHAN = Convert.ToInt32(fc["iChucVu"]);
                int ivuviec = (int)v.IVUVIEC;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "tiepdan_vuviec_xuly_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)v.IXULY;
                        _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                    }
                }

                _tiepdan.Update_TDVuviecxuly(v);
                TD_VUVIEC vuviecedit = _tiepdan.Get_TDVuviecID((int)v.IVUVIEC);

                int iUser = u_info.tk_action.iUser;
                _tiepdan.Tracking_Tiepdan(iUser, iDon, "sửa trả lời vụ việc");
                var thongtinvuviec = _tiepdan.Get_TDVuviecID((int)v.IVUVIEC);
                Response.Redirect("/Tiepdan/Traloixuly/?id=" + HashUtil.Encode_ID(thongtinvuviec.IVUVIEC.ToString(), Request.Cookies["url_key"].Value) + "");
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa trả lời xử lý vụ việc ");
                //return null;
                throw;
            }
        }



        // Bổ Sung ngày 15
        public string Ajax_LoadLinhVucNoiDung(int iLinhVuc)
        {

            string str = "<select name='iNoiDung' id='iNoiDung' class='input-medium chosen-select' style='width:100%' onchange='LoadOpTinhChat()'>" +
                                        "<option value='0'>- - - Chưa xác định</option>" +
                                        "" + tiepdan.Option_NoiDungDon_ThuocLinhVuc(id_user(), iLinhVuc) + "" +
                                        "</select>";
            return str;
        }
        public string Ajax_LoadLinhVucNoiDung_add(int iLinhVuc)
        {
            string str = "<select name='iNhomnoidung' id='iNhomnoidung' class='input-medium chosen-select' style='width:100%' onchange='LoadOpTinhChat()'>" +
                                        "<option value='0'>- - - Chưa xác định</option>" +
                                        "" + tiepdan.Option_NoiDungDon_ThuocLinhVuc(id_user(), iLinhVuc) + "" +
                                        "</select>";
            return str;
        }
        public string Ajax_LoadTinhChatNoiDung(int iNoiDung)
        {
            string str = "<select name='iTinhChat' id='iTinhChat' class='input-medium chosen-select' style='width:100%'>" +
                                        "<option value='0'>- - - Chưa xác định</option>" +
                                        "" + tiepdan.Option_TinhChatDon_ThuocNguonDon(id_user(), iNoiDung) + "" +
                                        "</select>";
            return str;
        }
        public string Ajax_LoadLinhVucThuocLoaiDon(int iLoaiDon)
        {
            List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
            string str = "<select name='iLinhVuc' id='iLinhVuc' class='input-medium chosen-select' style='width:100%' onchange='LoadLinhVuc()' >" +
                                       "<option value='0'>- - - Chưa xác định</option>" +
                                       "" + tiepdan.Option_LinhVucThuocLoaiDon(linhvuc, 0, 0, id_user(), iLoaiDon) + "" +
                                       "</select>";
            return str;
        }
        public ActionResult Ajax_Vuviec_update(FormCollection fc)
        {
            int id_check = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
            TD_VUVIEC don_bd = _tiepdan.Get_TDVuviecID(id_check);
            string cnoidung = fc["Noidung_vuviec"];
            try
            {
                //....

                int iUser = id_user();
                int id_trung = Convert.ToInt32(HashUtil.Decode_ID(fc["id_trung"], Request.Cookies["url_key"].Value));
                TD_VUVIEC don = _tiepdan.Get_TDVuviecID(id_check);
                if (Convert.ToInt32(don.CMADON) == id_trung)
                {

                    //  don.CNOIDUNG = cnoidung;

                    _tiepdan.Tracking_Tiepdan(iUser, id_check, "Bỏ chọn đơn trùng");
                    don.CMADON = "0";
                    _tiepdan.Update_TDVuviec(don);
                    Response.Write(0);
                }
                else
                {
                    TD_VUVIEC don_trung = _tiepdan.Get_TDVuviecID(id_trung);
                    _tiepdan.Tracking_Tiepdan(iUser, id_check, "chọn đơn trùng");
                    don.CMADON = id_trung.ToString();
                    _tiepdan.Update_TDVuviec(don);
                    Response.Write(fc["id_trung"]);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa trả lời xử lý vụ việc ");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Vuviec_update2(FormCollection fc)
        {

            try
            {
                TD_VUVIEC vuvieckiemtrung = (TD_VUVIEC)Session[vuviectiepdanSession];
                int iUser = id_user();
                int id_trung = Convert.ToInt32(HashUtil.Decode_ID(fc["id_trung"], Request.Cookies["url_key"].Value));

                if (Convert.ToInt32(vuvieckiemtrung.CMADON) == id_trung)
                {
                    vuvieckiemtrung.CMADON = "0";
                    Response.Write(0);
                }
                else
                {
                    vuvieckiemtrung.CMADON = id_trung.ToString();
                    Response.Write(fc["id_trung"]);

                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa trả lời xử lý vụ việc ");
                //return null;
                throw;
            }

        }
        public ActionResult Word_Phieuhuongdan(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "phieuhuongdan.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();
            var thongtin = _tiepdan.Get_TDVuviecID((int)id);
            ViewData["donvi"] = _thietlap.GetBy_Quochoi_CoquanID((int)u_info.user_login.IDONVI).CTEN;
            ViewData["ten"] = thongtin.CNGUOIGUI_TEN;
            ViewData["noidung"] = thongtin.CNOIDUNG;
            string thongtintinh = "";
            var kiemtratinh = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)thongtin.IDIAPHUONG_0).ToList();
            if (kiemtratinh.Count() > 0)
            {
                thongtintinh = _thietlap.GetBy_DiaphuongID((int)thongtin.IDIAPHUONG_0).CTEN;
            }
            string thongtinhuyen = "";
            var kiemtrahuyen = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)thongtin.IDIAPHUONG_1).ToList();
            if (kiemtrahuyen.Count() > 0)
            {
                thongtinhuyen = _thietlap.GetBy_DiaphuongID((int)thongtin.IDIAPHUONG_1).CTEN + " , ";
            }
            string diachinggui = "";
            if (thongtin.CNGUOIGUI_DIACHI != "" && thongtin.CNGUOIGUI_DIACHI != null)
            {
                diachinggui = "" + thongtin.CNGUOIGUI_DIACHI + " , ";
            }
            ViewData["diachi"] = diachinggui + "" + thongtinhuyen + "" + thongtintinh + "";
            //  ViewData["diachi"] = _thietlap.GetBy_DiaphuongID((int)thongtin.IDIAPHUONG_0).CTEN + "," + _thietlap.GetBy_DiaphuongID((int)thongtin.IDIAPHUONG_1).CTEN + "," + thongtin.CNGUOIGUI_DIACHI;
            return View();
        }
        public ActionResult Word_Nhandon(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "nhandon.doc"));
            Response.ContentType = "application/vnd.ms-word";
            UserInfor u_info = GetUserInfor();
            var thongtin = _tiepdan.Get_TDVuviecID((int)id);
            ViewData["donvi"] = _thietlap.GetBy_Quochoi_CoquanID((int)u_info.user_login.IDONVI).CTEN;
            ViewData["ten"] = thongtin.CNGUOIGUI_TEN;
            ViewData["noidung"] = thongtin.CNOIDUNG;

            string thongtintinh = "";
            var kiemtratinh = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)thongtin.IDIAPHUONG_0).ToList();
            if (kiemtratinh.Count() > 0)
            {
                thongtintinh = _thietlap.GetBy_DiaphuongID((int)thongtin.IDIAPHUONG_0).CTEN;
            }
            string thongtinhuyen = "";
            var kiemtrahuyen = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)thongtin.IDIAPHUONG_1).ToList();
            if (kiemtrahuyen.Count() > 0)
            {
                thongtinhuyen = _thietlap.GetBy_DiaphuongID((int)thongtin.IDIAPHUONG_1).CTEN + " , ";
            }
            string diachinggui = "";
            if (thongtin.CNGUOIGUI_DIACHI != "" && thongtin.CNGUOIGUI_DIACHI != null)
            {
                diachinggui = "" + thongtin.CNGUOIGUI_DIACHI + " , ";
            }
            ViewData["diachi"] = diachinggui + "" + thongtinhuyen + "" + thongtintinh + "";
            Response.Charset = "UTF-8";
            return View();
        }
        public ActionResult Word_Chuyenxuly(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "chuyenxuly.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();
            var thongtin = _tiepdan.Get_TDVuviecID((int)id);
            ViewData["donvi"] = _thietlap.GetBy_Quochoi_CoquanID((int)u_info.user_login.IDONVI).CTEN;
            ViewData["ten"] = thongtin.CNGUOIGUI_TEN;
            ViewData["noidung"] = thongtin.CNOIDUNG;
            string thongtintinh = "";
            var kiemtratinh = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)thongtin.IDIAPHUONG_0).ToList();
            if (kiemtratinh.Count() > 0)
            {
                thongtintinh = _thietlap.GetBy_DiaphuongID((int)thongtin.IDIAPHUONG_0).CTEN;
            }
            string thongtinhuyen = "";
            var kiemtrahuyen = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == (int)thongtin.IDIAPHUONG_1).ToList();
            if (kiemtrahuyen.Count() > 0)
            {
                thongtinhuyen = _thietlap.GetBy_DiaphuongID((int)thongtin.IDIAPHUONG_1).CTEN + " , ";
            }
            string diachinggui = "";
            if (thongtin.CNGUOIGUI_DIACHI != "" && thongtin.CNGUOIGUI_DIACHI != null)
            {
                diachinggui = "" + thongtin.CNGUOIGUI_DIACHI + " , ";
            }
            ViewData["diachi"] = diachinggui + "" + thongtinhuyen + "" + thongtintinh + "";
            //ViewData["diachi"] = _thietlap.GetBy_DiaphuongID((int)thongtin.IDIAPHUONG_0).CTEN + "," + _thietlap.GetBy_DiaphuongID((int)thongtin.IDIAPHUONG_1).CTEN + "," + thongtin.CNGUOIGUI_DIACHI;
            return View();
        }
        // bổ sung tối 21 



        public ActionResult Giamsat_vuviec(int id)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                ViewData["id"] = id;
                SetTokenAction("tiepdan_giamsat_add");
                return PartialView("../Ajax/Tiepdan/Tiepdan_giamsat");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " giám sát vụ việc ");
                //return null;
                throw;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Giamsat_insert(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(fc["id"]);
                var thongtinvuviec = _tiepdan.Get_TDVuviecID(id);
                thongtinvuviec.IGIAMSAT = Convert.ToInt32(func.RemoveTagInput(fc["iGiamsat"]));
                thongtinvuviec.CYKIENGIAMSAT = func.RemoveTagInput(fc["cNoiDung"]);
                _tiepdan.Update_TDVuviec(thongtinvuviec);
                Response.Write(1); return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm loại giám sát  ");
                //return null;
                throw;
            }


        }
        public ActionResult Giamsat_vuviec_edit(int id)
        {

            try
            {
                if (!CheckAuthToken()) { return null; }
                ViewData["id"] = id;
                SetTokenAction("tiepdan_giamsat_add");
                var thongtinvuvuviec = _tiepdan.Get_TDVuviecID(id);
                ViewData["thongtin"] = thongtinvuvuviec;
                return PartialView("../Ajax/Tiepdan/Tiepdan_giamsat_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa loại giám sát  ");
                //return null;
                throw;
            }
        }
        // bổ sung 
        public ActionResult Ajax_LoadLich(int id)
        {
            if (!CheckAuthToken()) { return null; }
            string str = "";
            try
            {
                UserInfor u_info = GetUserInfor();

                int iUser = u_info.tk_action.iUser;

                if (id == 0)
                {
                    str = "  <select class='input-block-level' name='iTiepDinhKy' id='iTiepDinhKy' onchange='LoadThongTinLich()' >" +
                                                       " <option value='0'>- - - Chọn lịch tiếp </option>" +
                                                       "" + tiepdan.OptionTiepdan(0, (int)u_info.user_login.IDONVI) + "" +
                                                    "</select>";
                }
                else
                {
                    str = "<input type='text' name='dNgayNhan' id='dNgayNhan' class='input-medium datepick' />";
                }
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa loại giám sát  ");
                //return null;
                throw;
            }
            Response.Write(str);
            return null;

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Kiemtrung()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                TD_VUVIEC don = new TD_VUVIEC();

                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                don.INGUONDON = Convert.ToInt32(Request["iNguonDon"]);
                don.CNGUOIGUI_TEN = func.RemoveTagInput(Request["cNguoiGui_Ten"]).Trim();
                don.ISOLUONGTRUNG = 0;
                if (Request["iDoanDongNguoi"] != null)
                {
                    don.IDOANDONGNGUOI = 1;
                    don.ISONGUOI = Convert.ToInt32(Request["iSoNguoi"]);
                }
                else
                {
                    don.IDOANDONGNGUOI = 0;
                    don.ISONGUOI = 0;
                }
                if (Convert.ToInt32(Request["iLoaiTiep"]) == 0)
                {
                    don.IDINHKY = Convert.ToInt32(Request["iTiepDinhKy"]);
                    don.ITIEPDOTXUAT = 0;
                    don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(Request["dNgayNhan"])));

                }
                else if (Convert.ToInt32(Request["iLoaiTiep"]) == 2)
                {
                    don.IDINHKY = 0;
                    don.ITIEPDOTXUAT = 1;
                    don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(Request["dNgayNhan"])));
                }
                else
                {
                    don.IDINHKY = 0;
                    don.ITIEPDOTXUAT = 0;
                    don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(Request["dNgayNhan"])));
                }
                don.IDIAPHUONG_0 = Convert.ToInt32(Request["iDiaPhuong_0"]);
                if (Convert.ToInt32(Request["iDiaPhuong_0"]) == -1)
                {
                    don.IDIAPHUONG_1 = 0;
                }
                else
                {
                    don.IDIAPHUONG_1 = Convert.ToInt32(Request["iDiaPhuong_1"]);
                }
                don.CNOIDUNGCHIDAO = func.RemoveTagInput(Request["cNoiDungChiDao"]).Trim();
                don.ILANHDAOTIEP = Convert.ToInt32(Request["iLanhDaoTiep"]);
                don.CNGUOITIEP = func.RemoveTagInput(Request["cNguoiTiep_Ten"]).Trim();
                don.CNGUOIGUI_DIACHI = func.RemoveTagInput(Request["cNguoiGui_DiaChi"]).Trim();
                don.CNOIDUNG = "";
                don.IUSER = iUser;
                don.DDATE = DateTime.Now;
                don.ILOAIDON = Convert.ToInt32(Request["iLoaiDon"]);
                don.INOIDUNG = Convert.ToInt32(Request["iNoiDung"]);
                don.ITINHCHAT = Convert.ToInt32(Request["iTinhChat"]);
                don.ILINHVUC = Convert.ToInt32(Request["iLinhVuc"]);
                don.ITINHTRANGXULY = 0;
                don.CMADON = "0";
                don.IVUVIECTRUNG = 0;
                don.IDONVITIEPNHAN = 0;
                don.IGIAMSAT = 0;
                don.IDONDOC = 0;
                don.CYKIENGIAMSAT = "";
                don.INGUONDON = 0;
                don.IDONVI = (int)u_info.user_login.IDONVI;
                don.INGUOIGUI_DANTOC = Convert.ToInt32(Request["iNguoiGui_DanToc"]);
                don.INGUOIGUI_QUOCTICH = Convert.ToInt32(Request["iNguoiGui_QuocTich"]);
                Session[vuviectiepdanSession] = don;
                var v = _tiepdan.get_vuviec_kiemtrungnhanh((int)don.IDIAPHUONG_0, don.CNOIDUNG, don.CNGUOIGUI_TEN).Where(x => x.IVUVIECTRUNG == 0).ToList();
                // List<TD_VUVIEC> v = _tiepdan.get_vuviec_kiemtrung(id).Where(x => x.IVUVIECTRUNG == 0).ToList(); 
                if (don.IDINHKY != 0 && don.ITIEPDOTXUAT == 0)
                {
                    v = _tiepdan.get_vuviec_kiemtrungnhanh((int)don.IDIAPHUONG_0, don.CNOIDUNG, don.CNGUOIGUI_TEN).Where(x => x.IVUVIECTRUNG == 0 && x.IDINHKY != 0 && x.ITIEPDOTXUAT == 0).ToList();
                }
                else if (don.IDINHKY == 0 && don.ITIEPDOTXUAT != 0)
                {
                    v = _tiepdan.get_vuviec_kiemtrungnhanh((int)don.IDIAPHUONG_0, don.CNOIDUNG, don.CNGUOIGUI_TEN).Where(x => x.IVUVIECTRUNG == 0 && x.IDINHKY == 0 && x.ITIEPDOTXUAT != 0).ToList();
                }
                else
                {
                    v = _tiepdan.get_vuviec_kiemtrungnhanh((int)don.IDIAPHUONG_0, don.CNOIDUNG, don.CNGUOIGUI_TEN).Where(x => x.IVUVIECTRUNG == 0 && x.IDINHKY == 0 && x.ITIEPDOTXUAT == 0).ToList();
                }
                if (v.Count() > 0)
                {
                    Response.Write(0);
                }
                else
                {
                    Response.Write(1);
                }


                return null;
            }
            catch (Exception e)
            {
                //Handle Exception;
                log.Log_Error(e, "Kiểm trùng vụ việc ");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Kiemtrungvuviec(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                //....
                if (fc["vuvieckiemtra"] != null && fc["vuvieckiemtra"] != "")
                {
                    for (int i = 1; i < 4; i++)
                    {
                        file = Request.Files["file_upload" + i];
                        if (file != null && file.ContentLength > 0)
                        {
                            if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/"); return null; }
                        }
                    }
                    int id_vuvieckiemtra = Convert.ToInt32(HashUtil.Decode_ID(fc["vuvieckiemtra"], Request.Cookies["url_key"].Value));
                    UserInfor u_info = GetUserInfor();
                    int iUser = u_info.tk_action.iUser;
                    TD_VUVIEC don = _tiepdan.Get_TDVuviecID(id_vuvieckiemtra);
                    don.INGUONDON = Convert.ToInt32(fc["iNguonDon"]);
                    don.CNGUOIGUI_TEN = func.RemoveTagInput(fc["cNguoiGui_Ten"]).Trim();
                    don.ISOLUONGTRUNG = 0;
                    if (fc["iDoanDongNguoi"] != null)
                    {
                        don.IDOANDONGNGUOI = 1;
                        don.ISONGUOI = Convert.ToInt32(fc["iSoNguoi"]);
                    }
                    else
                    {
                        don.IDOANDONGNGUOI = 0;
                        don.ISONGUOI = 0;
                    }
                    if (Convert.ToInt32(fc["iLoaiTiep"]) == 0)
                    {
                        don.IDINHKY = Convert.ToInt32(fc["iTiepDinhKy"]);
                        don.ITIEPDOTXUAT = 0;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));

                    }
                    else if (Convert.ToInt32(fc["iLoaiTiep"]) == 2)
                    {
                        don.IDINHKY = 0;
                        don.ITIEPDOTXUAT = 1;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                    }
                    else
                    {
                        don.IDINHKY = 0;
                        don.ITIEPDOTXUAT = 0;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                    }
                    don.IDIAPHUONG_0 = Convert.ToInt32(fc["iDiaPhuong_0"]);
                    if (Convert.ToInt32(fc["iDiaPhuong_0"]) == -1)
                    {
                        don.IDIAPHUONG_1 = 0;
                    }
                    else
                    {
                        don.IDIAPHUONG_1 = Convert.ToInt32(fc["iDiaPhuong_1"]);
                    }
                    don.CNOIDUNGCHIDAO = func.RemoveTagInput(fc["cNoiDungChiDao"]).Trim();
                    don.ILANHDAOTIEP = Convert.ToInt32(fc["iLanhDaoTiep"]);
                    don.CNGUOITIEP = func.RemoveTagInput(fc["cNguoiTiep_Ten"]).Trim();
                    don.CNGUOIGUI_DIACHI = func.RemoveTagInput(fc["cNguoiGui_DiaChi"]).Trim();
                    don.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                    don.IUSER = iUser;
                    don.DDATE = DateTime.Now;
                    don.ILOAIDON = Convert.ToInt32(fc["iLoaiDon"]);
                    don.INOIDUNG = Convert.ToInt32(fc["iNoiDung"]);
                    don.ITINHCHAT = Convert.ToInt32(fc["iTinhChat"]);
                    don.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                    don.ITINHTRANGXULY = 0;
                    don.IVUVIECTRUNG = 0;
                    don.IDONVITIEPNHAN = 0;
                    don.IGIAMSAT = 0;
                    don.IDONDOC = 0;
                    don.CYKIENGIAMSAT = "";
                    don.IDONVI = (int)u_info.user_login.IDONVI;
                    don.INGUOIGUI_DANTOC = Convert.ToInt32(fc["iNguoiGui_DanToc"]);
                    don.INGUOIGUI_QUOCTICH = Convert.ToInt32(fc["iNguoiGui_QuocTich"]);
                    _tiepdan.Update_TDVuviec(don);
                    int ivuviec = (int)don.IVUVIEC;
                    for (int i = 1; i < 4; i++)
                    {
                        file = Request.Files["file_upload" + i];
                        if (file != null && file.ContentLength > 0)
                        {
                            FILE_UPLOAD f = new FILE_UPLOAD();
                            f.CTYPE = "tiepdan_vuviec";
                            f.CFILE = UploadFile(file);
                            f.ID = (int)don.IVUVIEC;
                            _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                        }

                    }
                    Response.Write(fc["vuvieckiemtra"]);
                }
                else
                {
                    for (int i = 1; i < 4; i++)
                    {
                        file = Request.Files["file_upload" + i];
                        if (file != null && file.ContentLength > 0)
                        {
                            if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/"); return null; }
                        }
                    }
                    UserInfor u_info = GetUserInfor();
                    int iUser = u_info.tk_action.iUser;
                    TD_VUVIEC don = new TD_VUVIEC();
                    don.INGUONDON = Convert.ToInt32(fc["iNguonDon"]);
                    don.CNGUOIGUI_TEN = func.RemoveTagInput(fc["cNguoiGui_Ten"]).Trim();
                    don.ISOLUONGTRUNG = 0;
                    if (fc["iDoanDongNguoi"] != null)
                    {
                        don.IDOANDONGNGUOI = 1;
                        don.ISONGUOI = Convert.ToInt32(fc["iSoNguoi"]);
                    }
                    else
                    {
                        don.IDOANDONGNGUOI = 0;
                        don.ISONGUOI = 0;
                    }
                    if (Convert.ToInt32(fc["iLoaiTiep"]) == 0)
                    {
                        don.IDINHKY = Convert.ToInt32(fc["iTiepDinhKy"]);
                        don.ITIEPDOTXUAT = 0;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));

                    }
                    else if (Convert.ToInt32(fc["iLoaiTiep"]) == 2)
                    {
                        don.IDINHKY = 0;
                        don.ITIEPDOTXUAT = 1;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                    }
                    else
                    {
                        don.IDINHKY = 0;
                        don.ITIEPDOTXUAT = 0;
                        don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                    }
                    don.IDIAPHUONG_0 = Convert.ToInt32(fc["iDiaPhuong_0"]);
                    if (Convert.ToInt32(fc["iDiaPhuong_0"]) == -1)
                    {
                        don.IDIAPHUONG_1 = 0;
                    }
                    else
                    {
                        don.IDIAPHUONG_1 = Convert.ToInt32(fc["iDiaPhuong_1"]);
                    }
                    don.CNGUOITIEP = func.RemoveTagInput(fc["cNguoiTiep_Ten"]).Trim();
                    don.CNGUOIGUI_DIACHI = func.RemoveTagInput(fc["cNguoiGui_DiaChi"]).Trim();
                    don.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                    don.IUSER = iUser;
                    don.DDATE = DateTime.Now;
                    don.ILOAIDON = Convert.ToInt32(fc["iLoaiDon"]);
                    don.INOIDUNG = Convert.ToInt32(fc["iNoiDung"]);
                    don.ITINHCHAT = Convert.ToInt32(fc["iTinhChat"]);
                    don.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                    don.ITINHTRANGXULY = 0;
                    don.IVUVIECTRUNG = 0;
                    don.IDONVITIEPNHAN = 0;
                    don.IGIAMSAT = 0;
                    don.IDONDOC = 0;
                    don.CYKIENGIAMSAT = "";
                    don.IDONVI = (int)u_info.user_login.IDONVI;
                    don.INGUOIGUI_DANTOC = Convert.ToInt32(fc["iNguoiGui_DanToc"]);
                    don.INGUOIGUI_QUOCTICH = Convert.ToInt32(fc["iNguoiGui_QuocTich"]);
                    _tiepdan.Insert_TDVuviec(don);
                    int ivuviec = (int)don.IVUVIEC;
                    for (int i = 1; i < 4; i++)
                    {
                        file = Request.Files["file_upload" + i];
                        if (file != null && file.ContentLength > 0)
                        {
                            FILE_UPLOAD f = new FILE_UPLOAD();
                            f.CTYPE = "tiepdan_vuviec";
                            f.CFILE = UploadFile(file);
                            f.ID = (int)don.IVUVIEC;
                            _tiepdan.Insert_File_TiepDanThuongXuyenLoaivuviec(f);
                        }

                    }
                    _tiepdan.Tracking_Tiepdan(iUser, ivuviec, "Thêm mới vụ việc");
                    List<TD_VUVIEC> v = _tiepdan.get_vuviec_kiemtrung(ivuviec);
                    if (v != null && v.Count() > 0)
                    {
                        Response.Write(HashUtil.Encode_ID(ivuviec.ToString(), Request.Cookies["url_key"].Value));
                    }
                    else
                    {
                        _tiepdan.delete_vuviec(ivuviec);
                        Response.Write(1);
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                //Handle Exception;
                log.Log_Error(e, "Thêm mới vụ việc ");
                return View("../Home/Error_Exception");
            }
        }


        // Ajax



        // TRA CỨU BẰNG THỦ TỤC NEW 
        public ActionResult Ajax_TRACUUDINHKY()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                int iUser_KQ = u_info.tk_action.iUser;
                int iUser = 0;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                    //iDonViTiepNhan = 0;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser = 0;
                    iDonViTiepNhan = 0;
                }
                string str = "";
                string ctentimkiem = func.RemoveTagInput(Request["search"].ToUpper().Trim());
                List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                str = tiepdan.TIEPDANVUVIECTRACUU_DINHKY(Xuly, iUser, iDonViTiepNhan, Request.Cookies["url_key"].Value, iUser_KQ, ctentimkiem, id);
                Response.Write("" + str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm nội dung vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_TRACUUTHUONGXUYEN()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser_KQ = u_info.tk_action.iUser;
                int iUser = 0;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                    //iDonViTiepNhan = 0;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser = 0;
                    iDonViTiepNhan = 0;
                }
                string str = "";
                string ctentimkiem = func.RemoveTagInput(Request["search"].ToUpper().Trim());
                List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                if (ctentimkiem != "")
                {
                    str = tiepdan.TIEPDANVUVIECTRACUU_THUONGXUYEN(Xuly, iUser, iDonViTiepNhan, Request.Cookies["url_key"].Value, iUser_KQ, ctentimkiem);
                }
                else
                {
                    str = tiepdan.TIEPDANVUVIECTRACUU_THUONGXUYEN(Xuly, iUser, iDonViTiepNhan, Request.Cookies["url_key"].Value, iUser_KQ, "");
                }
                Response.Write("" + str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm nội dung vụ việc ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_TRACUUDOTXUAT()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser_KQ = u_info.tk_action.iUser;
                int iUser = 0;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("53", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                    //iDonViTiepNhan = 0;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser = 0;
                    iDonViTiepNhan = 0;
                }
                string str = "";
                string ctentimkiem = func.RemoveTagInput(Request["search"].ToUpper().Trim());
                List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                if (ctentimkiem != "")
                {
                    str = tiepdan.TIEPDANVUVIECTRACUU_DOTXUAT(Xuly, iUser, iDonViTiepNhan, Request.Cookies["url_key"].Value, iUser_KQ, ctentimkiem);
                }
                else
                {
                    str = tiepdan.TIEPDANVUVIECTRACUU_DOTXUAT(Xuly, iUser, iDonViTiepNhan, Request.Cookies["url_key"].Value, iUser_KQ, "");
                }
                Response.Write("" + str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm nội dung vụ việc ");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_TRACUUNANGCAO(int page = 1)// Tra cứu tổng
        {
            //....
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iUser_KQ = u_info.tk_action.iUser;
                int iUser = 0;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("17,18", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                    //iDonViTiepNhan = 0;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser = 0;
                    iDonViTiepNhan = 0;
                }
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }

                int loaivuviec = Convert.ToInt32(Request["iLoaiVuViec"]);
                string dTuNgay = DateTime.MinValue.ToString("dd-MMM-yyyy");
                string dDenNgay = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                if (Request["dTuNgay"].ToString() != "")
                {
                    dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));

                }
                if (Request["dDenNgay"].ToString() != "")
                {
                    dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));

                }
                int tracuudoan = Convert.ToInt32(Request["iDoan_Tracuu"]);
                int idonvitracuu = Convert.ToInt32(Request["iDonVi"]);
                string ctennguoiguitracuu = Request["cNguoiGui_Ten"];
                string cdiachinguoigui = Request["cNguoiGui_DiaChi"];
                string cnoidungtracuu = Request["cNoiDung"];
                int iloaidontracuu = Convert.ToInt32(Request["iLoai"]);
                int ilinhvuctracuu = Convert.ToInt32(Request["iLinhVuc"]);
                int inoidungtracuu = Convert.ToInt32(Request["iNoiDung"]);
                int itinhchattracuu = Convert.ToInt32(Request["iTinhChat"]);
                int itinhtrangxuly = Convert.ToInt32(Request["iHinhThuc"]);
                int ikiemtratrung = Convert.ToInt32(Request["iVuViecTrung"]);

                List<TD_VUVIEC_XULY> Xuly = _tiepdan.Get_list_TDVuviecxuly();
                TD_VUVIEC VUVIEC = new TD_VUVIEC();
                if (loaivuviec == 2)
                {

                    VUVIEC.ITIEPDOTXUAT = 1;
                    VUVIEC.IDINHKY = 0;
                }
                else
                {
                    VUVIEC.ITIEPDOTXUAT = 0;
                    VUVIEC.IDINHKY = loaivuviec;
                }

                VUVIEC.IDOANDONGNGUOI = tracuudoan;
                VUVIEC.IDONVI = idonvitracuu;
                VUVIEC.CNGUOIGUI_DIACHI = cdiachinguoigui;
                VUVIEC.CNGUOIGUI_TEN = ctennguoiguitracuu;
                VUVIEC.CNOIDUNG = cnoidungtracuu;
                VUVIEC.ILOAIDON = iloaidontracuu;
                VUVIEC.ILINHVUC = ilinhvuctracuu;
                VUVIEC.INOIDUNG = inoidungtracuu;
                VUVIEC.ITINHCHAT = itinhchattracuu;
                VUVIEC.ITINHTRANGXULY = itinhtrangxuly;
                VUVIEC.IVUVIECTRUNG = ikiemtratrung;
                VUVIEC.IUSER = iUser;
                var thongtinvuviec = tiepdanreport.ListTraCuu("PKG_TD_VUVIEC.PRC_TIEPDAN_TRACUUVUVIEC", VUVIEC, dTuNgay, dDenNgay, page, post_per_page);
                if (thongtinvuviec != null && thongtinvuviec.Count() > 0)
                {
                    ViewData["list"] = tiepdan.TIEPDANTRACUU(thongtinvuviec, Xuly, dTuNgay, dDenNgay);
                    ViewData["phantrang"] = "<tr><td colspan='6'>" + base_appcode.PhanTrang((int)thongtinvuviec.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='6' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }

                //  ViewData["list"] = tiepdan.TIEPDAN_TRACUUNANGCAO(VUVIEC, Xuly, iUser, iDonViTiepNhan, Request.Cookies["url_key"].Value, iUser_KQ, dTuNgay, dDenNgay);
                return PartialView("../Ajax/Tiepdan/Vuviec_result");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tra cứu vụ việc ");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_Thongtinketqua()
        {

            string str = "";
            str = "<select name='iketquatraloi' id='iketquatraloi' class='input-block-level chosen-select' >" +
                                                     "<option value='-1'> - - - Chọn tất cả</option>" +
                                                      " <option value='0'>- - - Có kết quả trả lời</option>" +
                                                      " <option value='1'>- - - Không có kết quả trả lời</option>" +
                                                    " </select>";
            Response.Write(str);
            return null;
        }
        public ActionResult Ajax_LoadLinhVucByLoaiDon_TraCuu(int iLoaiDon)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string str = "<select name='iLinhVuc' id='iLinhVuc' class='input-medium chosen-select' onchange=\"LoadLinhVuc()\" style='width:100%'>" +
                                            "<option value='-1'>- - - Chọn tất cả</option>" +
                                            "" + kn.Option_LinhVuc_LoaiDon(0, iLoaiDon) + "" + "</select>";

                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Load tính chất nội dung ");
                //Handle Exception;
                return null;
            }
        }
        public string Ajax_LoadLinhVucNoiDung_TraCuu(int iLinhVuc)
        {

            string str = "<select name='iNoiDung' id='iNoiDung' class='input-medium chosen-select' style='width:100%' onchange='LoadOpTinhChat()'>" +
                                        "<option value='-1'>- - - Chọn tất cả</option>" +
                                        "" + tiepdan.Option_NoiDungDon_ThuocLinhVuc(id_user(), iLinhVuc) + "" +
                                        "</select>";
            return str;
        }

        public string Ajax_LoadTinhChatNoiDung_TraCuu(int iNoiDung)
        {
            string str = "<select name='iTinhChat' id='iTinhChat' class='input-medium chosen-select' style='width:100%'>" +
                                        "<option value='-1'>- - - Chọn tất cả</option>" +
                                        "" + tiepdan.Option_TinhChatDon_ThuocNguonDon(id_user(), iNoiDung) + "" +
                                        "</select>";
            return str;
        }
        public ActionResult Dotxuat(int page = 1)
        {

            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                int iUser_KQ = u_info.tk_action.iUser;
                int iUser = -1;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("53", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser = -1;

                }
                string tukhoa = "";
                if (Request["q"] != null)
                {
                    tukhoa = Request["q"];
                }
                TD_VUVIEC Thongtinvuviec = get_Request_Paramt_VuViec();
                List<TD_VUVIEC_XULY> Thongtinxulyvuviec = _tiepdan.Get_list_TDVuviecxuly();
                string dTuNgay = DateTime.MinValue.ToString("dd-MMM-yyyy");
                string dDenNgay = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                if (Request["dTuNgay"] != null && Request["dTuNgay"] != "")
                {

                    dTuNgay = func.ConvertDateToSql(Request["dTuNgay"].ToString());
                }
                if (Request["dDenNgay"] != null && Request["dDenNgay"] != "")
                {
                    dDenNgay = func.ConvertDateToSql(Request["dDenNgay"].ToString());
                }
                var td = tiepdanreport.GETLISTTIEPCONGDAN("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIECDANHSACH", Thongtinvuviec, dTuNgay, dDenNgay, page, post_per_page, "" + tukhoa.Trim() + "", -1, -1, 0, iUser, iDonViTiepNhan, 1).ToList();
                if (td.Count() > 0)
                {
                    if ((Request["dTuNgay"] != null && Request["dTuNgay"] != "") || Request["q"] != null)
                    {
                        ViewData["ketqua"] = "<tr><td colspan='8' class='alert tcenter alert-info'> Có " + td.FirstOrDefault().TOTAL + " kết quả tìm kiếm</td></tr>";
                    }
                    ViewData["list"] = tiepdan.TIEPDANVUVIECDOTXUAT_PHANTRANG(td, Thongtinxulyvuviec, Request.Cookies["url_key"].Value);
                    if (td.Count() > 0)
                    {
                        ViewData["phantrang"] = "<tr><td colspan='8'>" + base_appcode.PhanTrang((int)td.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    }
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='8' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }


                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách đột xuất ");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Dotxuat_tamthoi(int page = 1)
        {

            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                int iUser_KQ = u_info.tk_action.iUser;
                int iUser = -1;
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                if (!ba_se.ActionMulty_Redirect_("53", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!ba_se.ActionMulty_Redirect_("51", u_info.tk_action))
                {
                    iUser = u_info.tk_action.iUser;
                }
                if (u_info.tk_action.is_admin)
                {
                    iUser = -1;

                }
                string tukhoa = "";
                if (Request["q"] != null)
                {
                    tukhoa = Request["q"];
                }
                TD_VUVIEC Thongtinvuviec = get_Request_Paramt_VuViec();
                List<TD_VUVIEC_XULY> Thongtinxulyvuviec = _tiepdan.Get_list_TDVuviecxuly();
                string dTuNgay = DateTime.MinValue.ToString("dd-MMM-yyyy");
                string dDenNgay = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                if (Request["dTuNgay"] != null && Request["dTuNgay"] != "")
                {

                    dTuNgay = func.ConvertDateToSql(Request["dTuNgay"].ToString());
                }
                if (Request["dDenNgay"] != null && Request["dDenNgay"] != "")
                {
                    dDenNgay = func.ConvertDateToSql(Request["dDenNgay"].ToString());
                }
                var td = tiepdanreport.GETLISTTIEPCONGDAN("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIECTAMTHOI", Thongtinvuviec, dTuNgay, dDenNgay, page, post_per_page, "" + tukhoa.Trim() + "", -1, -1, 0, iUser, iDonViTiepNhan, 1).ToList();
                if (td.Count() > 0)
                {
                    if ((Request["dTuNgay"] != null && Request["dTuNgay"] != "") || Request["q"] != null)
                    {
                        ViewData["ketqua"] = "<tr><td colspan='8' class='alert tcenter alert-info'> Có " + td.FirstOrDefault().TOTAL + " kết quả tìm kiếm</td></tr>";
                    }
                    ViewData["list"] = tiepdan.TIEPDANVUVIECDOTXUAT_PHANTRANG(td, Thongtinxulyvuviec, Request.Cookies["url_key"].Value);
                    if (td.Count() > 0)
                    {
                        ViewData["phantrang"] = "<tr><td colspan='8'>" + base_appcode.PhanTrang((int)td.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    }
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='8' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }


                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách đột xuất ");
                return View("../Home/Error_Exception");
            }

        }

    }
}
