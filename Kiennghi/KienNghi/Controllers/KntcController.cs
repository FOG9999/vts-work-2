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
using Spire.Xls;
using System.Data;
using System.Text;
using System.Reflection;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using System.Globalization;
using KienNghi.Flexcel;
using System.Web.UI.WebControls;

namespace KienNghi.Controllers
{
    public class KntcController : FlexcelReportController
    {
        //
        // GET: /Kntc/
        private const string KNTCDonSession = "KNTCDonSession";
        Thietlap tl = new Thietlap();
        private Funtions func = new Funtions();
        private KntcBusineess kntc = new KntcBusineess();
        private KiennghiBusineess knBussiness = new KiennghiBusineess();
        private Khieunai kn = new Khieunai();
        //ThietlapBusineess _thietlap = new ThietlapBusineess();
        Tiepdan _tiepdan = new Tiepdan();
        Log log = new Log();
        Base base_appcode = new Base();
        BaseBusineess _base = new BaseBusineess();
        Dictionary<string, object> _condition = new Dictionary<string, object>();
        Kiennghi_cl _kn = new Kiennghi_cl();
        KntcReport _kntcReport = new KntcReport();

        public string UploadFile(HttpPostedFileBase file)
        {
            string file_name = "";
            string dir_path_upload = AppConfig.dir_path_upload;
            string path_upload = "/Kntc/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
            string full_path = "";
            Random random = new Random(); int rand = random.Next(0, 99999);
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
        public string Get_Option_DonViThamQuyen(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            return "<option value='0'>Chọn đơn vị thẩm quyền</option>" + kn.OptionCoQuanXuLy(coquan, 0, 0, iDonVi, 0);
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

        public string Option_Post_Per_Page_Ajax(int post_per_page, string searchFormId, string containerId,
            bool queryString = true, string actionName = "", string controllerName = "")
        {
            string url = RemovePostPerPageFromUrl(queryString, actionName, controllerName);
            string str = "<div class='post_per_page'>Hiển thị <select onchange=\"ChangePage(1,parseInt(this.value),'" + url + "','" + searchFormId + "','" + containerId + "')\" class='input-small' name='post_per_page'>";
            List<int> list_post = new List<int>
            {
                pageSize,
                50,
                100,
                200
            };
            foreach (var p in list_post)
            {
                string select = ""; if (p == post_per_page) { select = "selected"; }
                str += "<option " + select + " value='" + p + "'>" + p + " dòng</option>";
            }
            str += "</select> mỗi trang</div>";
            return str;
        }

        public ActionResult Giamsat()
        {

            try
            {
                int iUser = id_user();
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = _base.tk_action(iUser);
                _base.ActionMulty_Redirect_(action_kntc, act);
                // new id
                string id = Request["id"];
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                // end
                ViewData["bandannguyen"] = 1;
                if (!u.tk_action.is_lanhdao)
                {
                    ViewData["bandannguyen"] = 0;
                }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                ViewData["id"] = id;
                _condition = new Dictionary<string, object>();
                _condition.Add("IDON", iDon);
                List<KNTC_GIAMSAT> giamsat = kntc.List_Giamsat(_condition);
                ViewData["list"] = kn.KNTC_Don_Giamsat(giamsat, u, Request.Cookies["url_key"].Value);
                if (!_base.Action_(15, act))
                {
                    ViewData["btn-add"] = " style='display:none' ";
                }
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách giám sát", null, 1);
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Giamsat_add(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {

                ViewData["id"] = fc["id"];
                SetTokenAction("kntc_giamsat_add");
                return PartialView("../Ajax/Kntc/Giamsat_add");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Hiểm thị form thêm mới giám sát", null, 1);
                return null;
            }
        }
        public ActionResult Ajax_Delete_giamsat_file(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                //....

                int iGiamSat = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_GIAMSAT g = kntc.Get_Giamsat(iGiamSat);
                g.CFILE = "";
                kntc.Giamsat_update(g);
                Response.Write(1);

            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa file văn bản giám sát", null, 1);
                return null;
            }
            return null;

        }
        public ActionResult Ajax_Giamsat_del(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                int iGiamSat = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_GIAMSAT g = kntc.Get_Giamsat(iGiamSat);
                if (g != null)
                {
                    kntc.Giamsat_delete(g);
                }

                Response.Write(1);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa giám sát", null, 1);
                return null;

            }
            return null;
        }
        public ActionResult Ajax_Kntc_del(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_DON g = kntc.GetDON(iDon);
                if (g != null)
                {
                    if(g.IIDIMPORT != null && g.IIDIMPORT != 0 ){
                        KNTC_DON_IMPORT ip = kntc.GetKntcDonImport((int)g.IIDIMPORT);
                        ip.ISODON--;
                        kntc.UpdateKntcDonImport(ip);
                    }
                    g.IDELETE = 1;
                    kntc.Update_Don(g);
                }

                Response.Write(1);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa giám sát", null, 1);
                return null;

            }
            return null;
        }
        public ActionResult Ajax_Kntc_back(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_DON g = kntc.GetDON(iDon);
                if (g != null)
                {
                    if (g.IIDIMPORT != null && g.IIDIMPORT != 0)
                    {
                        KNTC_DON_IMPORT ip = kntc.GetKntcDonImport((int)g.IIDIMPORT);
                        ip.ISODON++;
                        kntc.UpdateKntcDonImport(ip);
                    }
                    g.IDELETE = 0;
                    kntc.Update_Don(g);
                }

                Response.Write(1);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa giám sát", null, 1);
                return null;

            }
            return null;
        }
        public ActionResult Ajax_Giamsat_edit(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                int iGiamSat = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("kntc_giamsat_edit", iGiamSat);
                KNTC_GIAMSAT g = kntc.Get_Giamsat(iGiamSat);
                if (g != null)
                {
                    ViewData["g"] = g;
                    ViewData["id"] = fc["id"];
                    ViewData["file"] = kn.File_Edit(iGiamSat, "kntc_giamsat", Request.Cookies["url_key"].Value);
                }
                return PartialView("../Ajax/Kntc/Giamsat_edit");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Sửa giám sát", null, 1);
                return null;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Giamsat_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                string id = fc["id"];
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iGiamSat = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("kntc_giamsat_edit", iGiamSat)) { Response.Redirect("/Home/Error/"); return null; }
                file = Request.Files["file_upload1"];
                if (file != null && file.ContentLength > 0)
                {
                    CheckFile_Upload(file);
                }
                // new id

                KNTC_GIAMSAT v = kntc.Get_Giamsat(iGiamSat);
                if (v != null)
                {
                    v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                    v.CKEHOACH = func.RemoveTagInput(fc["cKeHoach"]);
                    v.CCHUYENDE = func.RemoveTagInput(fc["cChuyenDe"]);
                    kntc.Giamsat_update(v);
                    for (int i = 1; i < 4; i++)
                    {
                        file = Request.Files["file_upload" + i];
                        if (file != null && file.ContentLength > 0)
                        {
                            FILE_UPLOAD f = new FILE_UPLOAD();
                            f.CTYPE = "kntc_giamsat";
                            f.CFILE = UploadFile(file);
                            f.ID = iGiamSat;
                            kntc.Upload_file(f);
                        }

                    }
                    int iUser = id_user();
                    kntc.Tracking_KNTC(iUser, (int)v.IDON, "Sửa Kế hoạch giám sát: " + v.CKEHOACH);
                    string id_encr = HashUtil.Encode_ID(v.IDON.ToString(), func.Get_Url_keycookie());
                    Response.Redirect("/Kntc/Giamsat?id=" + id_encr + "#success");
                }

                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Chỉnh sửa giám sát", null, 1);
                return null;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Giamsat_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {

                if (!CheckTokenAction("kntc_giamsat_add")) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        CheckFile_Upload(file);
                    }

                }
                string id = fc["id"];
                // new id
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                // end
                KNTC_DON don = kntc.GetDON(iDon);
                KNTC_GIAMSAT v = new KNTC_GIAMSAT();
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.IDONVI = 4;
                v.CKEHOACH = func.RemoveTagInput(fc["cKeHoach"]);
                v.IUSER = id_user();
                v.DDATE = DateTime.Now;
                v.CCHUYENDE = func.RemoveTagInput(fc["cChuyenDe"]);
                v.IDON = iDon;
                v.CFILE = "";
                kntc.Giamsat_insert(v);
                int last_id = (int)v.IGIAMSAT;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_giamsat";
                        f.CFILE = UploadFile(file);
                        f.ID = last_id;
                        kntc.Upload_file(f);
                    }

                }
                int iUser = id_user();
                kntc.Tracking_KNTC(iUser, iDon, "Thêm Kế hoạch giám sát: " + v.CKEHOACH);
                string id_encr = HashUtil.Encode_ID(v.IDON.ToString(), Request.Cookies["url_key"].Value);
                Response.Redirect("/Kntc/Giamsat?id=" + id_encr + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thêm mới giám sát");
                return null;
            }
        }
        public ActionResult Ajax_Update_ketquaxuly(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {

                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_DON don = kntc.GetDON(iDon);
                don.IDUDIEUKIEN_KETQUA = -1;
                kntc.Update_Don(don);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Cập nhật kết quả xử lý");
                return null;
            }


        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Khongthuly_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                int iUser = id_user();
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("kntc_khongthuly", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        CheckFile_Upload(file);
                    }

                }
                KNTC_VANBAN v = new KNTC_VANBAN();
                v.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                v.ICOQUANNHAN = 0;
                v.IUSER = iUser;
                v.DDATE = DateTime.Now;
                v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayBanHanh"])));
                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.IDON = iDon;
                v.CLOAI = "khongthuly";
                v.CCHUCVU = func.RemoveTagInput(fc["cChucVu"]);
                kntc.Vanban_insert(v);
                int iVanban = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanban;
                        kntc.Upload_file(f);
                    }

                }
                KNTC_DON don = kntc.GetDON(iDon);
                don.ITINHTRANGXULY = 5;
                kntc.Update_Don(don);


                kntc.Tracking_KNTC(iUser, iDon, "Thông báo trả lời đơn thư không được thụ lý");

                if (Request.Cookies["link_action"].Value == "Tiepnhan")
                {
                    Response.Redirect("/Kntc/Tiepnhan/" + "#success");
                }
                else
                {
                    if (don.ITHAMQUYEN == 1)
                    {
                        if (don.IDUDIEUKIEN == 1)
                        {
                            Response.Redirect("/Kntc/Dudieukien/");
                        }
                        else if (don.IDUDIEUKIEN == -1)
                        {
                            Response.Redirect("/Kntc/Thuocthamquyen/");
                        }
                        else
                        {
                            Response.Redirect("/Kntc/Khongdudieukien/");
                        }
                    }
                    else
                    {
                        Response.Redirect("/Kntc/Khongthuocthamquyen/");
                    }

                }


                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Trả lời đơn thư không được thụ lý");
                return null;
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Chuyenxuly_noibo_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                string id = fc["id"];
                UserInfor u = GetUserInfor();
                int iDonVi = (int)u.user_login.IDONVI;
                int iUser = u.tk_action.iUser;
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_DON don = kntc.GetDON(iDon);
                if (!CheckTokenAction("kntc_chuyennoibo", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        CheckFile_Upload(file);
                    }

                }
                int idonvi = 0;
                //if (Convert.ToInt32(fc["iThamQuyen"]) == 2)
                //{
                    //idonvi = Convert.ToInt32(fc["iDonVi_DiaPhuong"]);
                //}
                //else
                //{
                    idonvi = Convert.ToInt32(fc["iDonVi"]);
                //}
                KNTC_VANBAN v = new KNTC_VANBAN();
                v.ICOQUANBANHANH = u.user_login.IDONVI;
                v.ICOQUANNHAN = idonvi;
                v.IUSER = id_user();
                v.DDATE = DateTime.Now;
                if (fc["dNgayBanHanh"] != null)
                {
                    if (fc["dNgayBanHanh"] != "")
                    {
                        v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayBanHanh"])));
                    }
                }

                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CCOQUANCHUYENDEN = fc["cCoQuanChuyenDen"];
                v.CNOINHAN = fc["cNoiNhan"];
                v.IDON = iDon;
                v.CLOAI = "chuyenxuly_noibo";
                v.CCHUCVU = func.RemoveTagInput(fc["cChucVu"]);
                kntc.Vanban_insert(v);
                int iVanban = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanban;
                        kntc.Upload_file(f);
                    }
                }

                //if (idonvi != iDonVi)
                //{
                    KNTC_DON_LICHSU lichsu = new KNTC_DON_LICHSU();
                    lichsu.IDON = iDon;
                    lichsu.IDONVIGUI = iDonVi;
                    lichsu.IDONVITIEPNHAN = idonvi;
                    lichsu.ITRANGTHAI = (int)don.ITINHTRANGXULY;
                    lichsu.IUSER = iUser;
                    lichsu.ICHUYENXULY = (int)LoaiLichSu.ChuyenXuLy;
                    lichsu.IDONVIXULY = don.IDONVITHULY;
                    lichsu.DNGAYCHUYEN = DateTime.Now;
                    lichsu.IVANBAN = iVanban;
                    kntc.InsertLichSuDon(lichsu);
                //}

                don.ITINHTRANGXULY = 3;
                don.IDUDIEUKIEN_KETQUA = 1;
                don.IDONVITHULY = idonvi;
                don.ITHULY = 1;
                var ngayquydinh = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayPhanHoi"]));
                DateTime now = DateTime.Now;
                if (fc["dNgayPhanHoi"] != null)
                    don.INGAYQUYDINH = ngayquydinh;
                double khacbiet = (ngayquydinh - now).TotalDays;
                if (khacbiet < 0)
                    don.ICANHBAO = 2;
                if (khacbiet >= 0 && khacbiet <= 5)
                    don.ICANHBAO = 1;
                if (khacbiet > 5)
                    don.ICANHBAO = 0;
                kntc.Update_Don(don);
                kntc.Tracking_KNTC(iUser, iDon, "Chuyển đơn đến cơ quan có thẩm quyền xử lý");
                if (Request.Cookies["link_action"].Value == "Tiepnhan")
                {
                    Response.Redirect("/Kntc/Tiepnhan/" + "#success");
                }
                else
                {
                    Response.Redirect("/Kntc/Chuatraloi/#success");

                }

                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Chuyển xử lý nội bộ");
                return null;
            }
        }
        public ActionResult Ajax_Don_lichsu(string id)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                // new id
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                decimal iDon = Convert.ToDecimal(id_decrypt);
                // end
                ViewData["list"] = kn.LichSu_Don(iDon);
                return PartialView("../Ajax/Kntc/Don_lichsu");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Load lỉnh sử xử lý đơn");
                return null;
            }
        }
        public ActionResult Ajax_Vanban_add(string id)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                ViewData["id"] = id;
                // new id
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                KNTC_DON don = kntc.GetDON(iDon);
                Dictionary<string, object> condition = new Dictionary<string, object>();
                condition.Add("ICOQUAN", don.IDONVITHULY);
                condition.Add("IHIENTHI", 1);
                condition.Add("IDELETE", 0);
                //List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
                SetTokenAction("kntc_vanban_add", iDon);

                int iType = 0;
                var coquanchon = _kiennghi.GetAll_CoQuanByParam(condition).FirstOrDefault();
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Trunguong;
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Tinh;
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Huyen;
                ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(iType);
                ViewData["donvithuly"] = Get_Option_DonViThamQuyen_ByCType(iType, (int)coquanchon.ICOQUAN);
                //if (don != null)
                //{
                //    ViewData["donvithuly"] = tl.OptionCoQuan_TreeList((int)don.IDONVITHULY);
                //}
                //else
                //{
                    ////ViewData["donvithuly"] = kn.OptionCoQuan(coquan, 0, 0, 0, 0);
                //    ViewData["donvithuly"] = tl.OptionCoQuan_TreeList(0);
                //}
                //ViewData["donvixuly"] = kn.OptionCoQuan(coquan, 0, 0, 0, 0);
                ViewData["danhgia"] = kn.Option_Danhgia(0);
                // end
                return PartialView("../Ajax/Kntc/Vanban_add");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Load from thêm mới văn bản");
                return null;
            }
        }
        public ActionResult Ajax_Vanban_lienquan(string id)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                ViewData["id"] = id;
                // new id
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                KNTC_DON don = kntc.GetDON(iDon);
                Dictionary<string, object> condition = new Dictionary<string, object>();
                if (don != null && don.IDONVITHULY != 0)
                {
                    condition.Add("ICOQUAN", don.IDONVITHULY);
                }
                condition.Add("IHIENTHI", 1);
                condition.Add("IDELETE", 0);
                //List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
                SetTokenAction("kntc_vanban_add", iDon);

                int iType = 0;
                var coquanchon = _kiennghi.GetAll_CoQuanByParam(condition).FirstOrDefault();
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Trunguong;
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Tinh;
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Huyen;
                ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(iType);
                ViewData["donvithuly"] = Get_Option_DonViThamQuyen_ByCType(iType, (int)coquanchon.ICOQUAN);
                //if (don != null)
                //{
                //    if (don.IDONVITHULY != 0)
                //    {
                //        ViewData["donvithuly"] = kn.OptionCoQuan(coquan, 0, 0, (int)don.IDONVITHULY, 0);
                //    }
                //}
                //else
                //{
                //    ViewData["donvithuly"] = kn.OptionCoQuan(coquan, 0, 0, 0, 0);
                //}

                // end

                return PartialView("../Ajax/Kntc/Vanban_lienquan");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị văn bản liên quan");
                return null;
            }
        }
        //public string GetRadioButton_ThamQuyen_KienNghi(int id_thamquyen)
        //{
        //    string str = "";
        //    if (id_thamquyen == 0)
        //    {
        //        str = "<div class='input-block-level'><span class='span6'><input class='nomargin' onclick=\"CheckThamQuyen()\" type='radio' id='iThamQuyen_TrungUong' name='iThamQuyen' checked value='1'> <label for='iThamQuyen_TrungUong'>Trung Ương</label></span>" +
        //             "<span class='span6'><input class='nomargin' onclick=\"CheckThamQuyen()\" type='radio' id='iThamQuyen_DiaPhuong' name='iThamQuyen' value='2'> <label for='iThamQuyen_DiaPhuong'>Địa phương</label></span></div>";
        //    }
        //    else
        //    {
        //        QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(id_thamquyen);
        //        if (coquan.IDIAPHUONG == 0)//trung ương
        //        {
        //            str = "<div class='input-block-level'><span class='span6'><input class='nomargin' onclick=\"CheckThamQuyen()\" type='radio' id='iThamQuyen_TrungUong' name='iThamQuyen' checked value='1'> <label for='iThamQuyen_TrungUong'>Trung Ương</label></span>" +
        //             "<span class='span6'><input class='nomargin' onclick=\"CheckThamQuyen()\" type='radio' id='iThamQuyen_DiaPhuong' name='iThamQuyen' value='2'> <label for='iThamQuyen_DiaPhuong'>Địa phương</label></span></div>";
        //        }
        //        else
        //        {
        //            str = "<div class='input-block-level'><span class='span6'><input class='nomargin' onclick=\"CheckThamQuyen()\" type='radio' id='iThamQuyen_TrungUong' name='iThamQuyen'  value='1'> <label for='iThamQuyen_TrungUong'>Trung Ương</label></span>" +
        //             "<span class='span6'><input class='nomargin' onclick=\"CheckThamQuyen()\" type='radio' id='iThamQuyen_DiaPhuong' name='iThamQuyen' value='2' checked> <label for='iThamQuyen_DiaPhuong'>Địa phương</label></span></div>";
        //        }
        //    }


        //    return str;
        //}
        public string GetRadioButton_ThamQuyen_KienNghi(int id_thamquyen)
        {
            string str = "";
            if (id_thamquyen == 0 || id_thamquyen == (int)ThamQuyen_DiaPhuong.Trunguong)
            {
                str = "<div class='input-block-level'><span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_TrungUong' name='iThamQuyen' checked value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "'>" +
                    " <label for='iThamQuyen_TrungUong'>" + "Trung Ương" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Tinh' name='iThamQuyen' value='" + (int)ThamQuyen_DiaPhuong.Tinh + "'> " +
                     "<label for='iThamQuyen_Tinh'>" + "Tỉnh" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Huyen' name='iThamQuyen' value='" + (int)ThamQuyen_DiaPhuong.Huyen + "'>" +
                     " <label for='iThamQuyen_Huyen'>" + "Huyện" + "</label></span>" +
                     "</div>";
            }
            else if (id_thamquyen == (int)ThamQuyen_DiaPhuong.Tinh)
            {
                str = "<div class='input-block-level'><span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_TrungUong' name='iThamQuyen' value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "'>" +
                    " <label for='iThamQuyen_TrungUong'>" + "Trung Ương" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Tinh' name='iThamQuyen' checked value='" + (int)ThamQuyen_DiaPhuong.Tinh + "'> " +
                     "<label for='iThamQuyen_Tinh'>" + "Tỉnh" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Huyen' name='iThamQuyen' value='" + (int)ThamQuyen_DiaPhuong.Huyen + "'>" +
                     " <label for='iThamQuyen_Huyen'>" + "Huyện" + "</label></span>" +
                     "</div>";
            }
            else
            {
                str = "<div class='input-block-level'><span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_TrungUong' name='iThamQuyen' value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "'>" +
                    " <label for='iThamQuyen_TrungUong'>" + "Trung Ương" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Tinh' name='iThamQuyen'  value='" + (int)ThamQuyen_DiaPhuong.Tinh + "'> " +
                     "<label for='iThamQuyen_Tinh'>" + "Tỉnh" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Huyen' name='iThamQuyen' checked value='" + (int)ThamQuyen_DiaPhuong.Huyen + "'>" +
                     " <label for='iThamQuyen_Huyen'>" + "Huyện" + "</label></span>" +
                     "</div>";
            }


            return str;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Vanban_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                file = Request.Files["file_upload1"];
                string id = fc["id"];
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                if (!CheckTokenAction("kntc_vanban_add", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        CheckFile_Upload(file);
                    }
                }
                KNTC_DON don = kntc.GetDON(iDon);
                DateTime ngayBanHanh = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                KNTC_VANBAN v = new KNTC_VANBAN();
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                v.ICOQUANNHAN = 0;
                v.IUSER = id_user();
                v.DDATE = DateTime.Now;
                v.DNGAYBANHANH = ngayBanHanh;
                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.GHICHU_XULY = func.RemoveTagInput(fc["cGhiChu"]);
                v.IDON = iDon;
                v.CLOAI = fc["loai"];
                v.CCHUCVU = func.RemoveTagInput(fc["cChucVu"]);
                kntc.Vanban_insert(v);
                int iVanban_last = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanban_last;
                        kntc.Upload_file(f);
                    }
                }
                int iUser = id_user();
                DateTime ngayquydinh = Convert.ToDateTime(don.INGAYQUYDINH);
                double khacbiet = (ngayquydinh - ngayBanHanh).TotalDays;
                decimal iCanhBao = 0;
                if (khacbiet < 0 || don.ICANHBAO == 2) iCanhBao = 2;
                if (v.CLOAI == "ketqua")
                {
                    kntc.Tracking_KNTC(iUser, (int)don.IDON, "Thêm Văn bản liên quan đến quá trình giải quyết số " + v.CSOVANBAN);
                    don.ITINHTRANGXULY = 7;
                    don.ICANHBAO = iCanhBao;
                    kntc.Update_Don(don);
                    Response.Redirect("Datraloi/");
                }

                //if (v.CLOAI == "giaothuchien")
                //{
                //    kntc.Tracking_KNTC(iUser, (int)don.IDON, "Thêm Văn bản đôn đốc, giao đơn vị thực hiện số " + v.CSOVANBAN);
                //    don.ITINHTRANGXULY = 4;
                //    kntc.Update_Don(don);
                //    Response.Redirect("Datraloi/");
                //}
                if (v.CLOAI == "hoanthanh")
                {
                    kntc.Tracking_KNTC(iUser, (int)don.IDON, "Thêm Quyết định giải quyết (hoàn thành xử lý) số " + v.CSOVANBAN);
                    don.ITINHTRANGXULY = 6;
                    don.ILUUTHEODOI = Convert.ToDecimal(LyDoLuuTheoDoi.DaTraLoi);
                    don.IDANHGIA = Convert.ToDecimal(fc["idanhgia"]);
                    don.ICANHBAO = iCanhBao;
                    kntc.Update_Don(don);
                    Response.Redirect("Datraloi/");
                }

                if (v.CLOAI == "chuagiaiquyet")
                {
                    kntc.Tracking_KNTC(iUser, (int)don.IDON, "Thêm Văn bản liên quan chưa giải quyết số " + v.CSOVANBAN);
                    don.ITINHTRANGXULY = 8;
                    don.ICANHBAO = iCanhBao;
                    kntc.Update_Don(don);
                    Response.Redirect("Datraloi/");
                }

                if (v.CLOAI == "dahuongdan")
                {
                    kntc.Tracking_KNTC(iUser, (int)don.IDON, "Thêm Văn bản đã hướng dẫn, trả lời số " + v.CSOVANBAN);
                    don.ITINHTRANGXULY = 9;
                    don.ICANHBAO = iCanhBao;
                    kntc.Update_Don(don);
                    Response.Redirect("Datraloi/");
                }
                //if (v.CLOAI == "dinhchi")
                //{
                //    kntc.Tracking_KNTC(iUser, (int)don.IDON, "Thêm Văn bản đình chỉ xử lý (lưu đơn, theo dõi) số " + v.CSOVANBAN);
                //    don.ITINHTRANGXULY = 5;
                //    don.ILUUTHEODOI = Convert.ToDecimal(LyDoLuuTheoDoi.KhongThuLy);
                //    don.CLUUTHEODOI_LYDO = "Đơn bị đình chỉ xử lý";
                //    kntc.Update_Don(don);
                //    Response.Redirect("Khongxuly/");
                //}
                //if (v.CLOAI == "khongthuly")
                //{
                //    kntc.Tracking_KNTC(iUser, (int)don.IDON, "Thêm Văn bản không thụ lý đơn (lưu đơn, theo dõi) số " + v.CSOVANBAN);
                //    don.ITINHTRANGXULY = 5;
                //    don.ILUUTHEODOI = Convert.ToDecimal(LyDoLuuTheoDoi.KhongThuLy);
                //    don.CLUUTHEODOI_LYDO = "Đơn không được thụ lý";
                //    kntc.Update_Don(don);
                //    Response.Redirect("Khongxuly/");
                //}
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Thêm mới văn bản liên quan");
                return null;
            }
        }
        public string Get_Option_DonViThamQuyen_ByCType(int iType = 0, int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IHIENTHI", 1);
            dic.Add("IDELETE", 0);
            string Ctype = "";
            if (iType == (int)ThamQuyen_DiaPhuong.Trunguong)
                Ctype = typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value;
            if (iType == (int)ThamQuyen_DiaPhuong.Tinh)
                Ctype = typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value;
            if (iType == (int)ThamQuyen_DiaPhuong.Huyen)
                Ctype = typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value;
            dic.Add("CTYPE", Ctype);
            coquan = _kiennghi.GetAll_CoQuanByParam(dic);
            if (iDonVi != 0)
                return "<option value='0'>Chọn đơn vị thẩm quyền</option>" + _kn.Option_CoquanDiaPhuong(coquan, iDonVi, iType);
            else
                return "<option selected value='0'>Chọn đơn vị thẩm quyền</option>" + _kn.Option_CoquanDiaPhuong(coquan, iDonVi, iType);
        }
        public ActionResult Ajax_Chuyenxuly_noibo(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                ViewData["id"] = fc["id"];
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var don = kntc.GetDON(iDon);
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ICOQUAN", don.IDONVITHULY);
                SetTokenAction("kntc_chuyennoibo", iDon);
                int iType = 0;
                var coquanchon = _kiennghi.GetAll_CoQuanByParam(param).FirstOrDefault();
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Trunguong;
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Tinh;
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Huyen;
                ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(iType);
                ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType(iType, (int)coquanchon.ICOQUAN);
                //if (u_info.tk_action.is_dbqh)
                //{
                //    ViewData["is_dbqh"] = "1";
                //    ViewData["opt-donvithamquyen"] = Get_Option_ThamQuyen_TrungUong(ID_Ban_DanNguyen);
                //    ViewData["opt-donvithamquyen-diaphuong"] = Get_Option_ThamQuyen_DiaPhuong(0);
                //}
                //else
                //{
                //    ViewData["is_dbqh"] = "0";
                //    //ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen(ID_Ban_DanNguyen);
                //    //Dictionary<string, object> condition = new Dictionary<string, object>();
                //    //condition.Add("IHIENTHI", 1);
                //    //condition.Add("IDELETE", 0);
                //    //List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
                //    //ViewData["opt-donvithamquyen"] = kn.OptionCoQuan(coquan, 0, 0, 0, 0);

                //    ViewData["opt-donvithamquyen"] = Get_Option_ThamQuyen_TrungUong(ID_Ban_DanNguyen);
                //    ViewData["opt-donvithamquyen-diaphuong-parent"] = Get_Option_ThamQuyen_DiaPhuong_Parent(0);
                //}


                return PartialView("../Ajax/Kntc/Chuyenxuly_noibo");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form chuyển xử lý nội bộ");
                return null;
            }
        }
        public ActionResult Ajax_Khongthuly(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                ViewData["id"] = fc["id"];
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("kntc_khongthuly", iDon);
                return PartialView("../Ajax/Kntc/Khongthuly");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form không thụ lý");
                return null;
            }
        }

        public ActionResult Ajax_Chuyendon_khongthuocthamquyen(string id)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                // new id
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                SetTokenAction("kntc_chuyenkhongthuocthamquyen", iDon);
                // end

                Dictionary<string, object> condition = new Dictionary<string, object>();
                condition.Add("IHIENTHI", 1);
                condition.Add("IDELETE", 0);
                List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
                ViewData["opt-coquan"] = kn.OptionCoQuan(coquan, 0, 0, 0, 0);
                ViewData["id"] = id;
                return PartialView("../Ajax/Kntc/Chuyendon_khongthuocthamquyen");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form chuyển đơn không thuộc thẩm quyền");
                return null;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Chuyendon_khongthuocthamquyen_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor u = GetUserInfor();
                string id = fc["id"];
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int donvithuly = Convert.ToInt32(fc["iDonVi"]);
                int iDonVi = (int)u.user_login.IDONVI;
                int iDon = Convert.ToInt32(id_decrypt);
                if (!CheckTokenAction("kntc_chuyenkhongthuocthamquyen", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        CheckFile_Upload(file);
                    }

                }
                KNTC_VANBAN v = new KNTC_VANBAN();
                v.CNOIDUNG = fc["cNoiDung"];
                v.ICOQUANBANHANH = 4;
                v.ICOQUANNHAN = Convert.ToInt32(fc["iDonVi"]);
                v.IUSER = id_user();
                v.DDATE = DateTime.Now;
                if (fc["dNgayBanHanh"] != "")
                {
                    v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                }
                v.CNGUOIKY = fc["cNguoiKy"];
                v.CSOVANBAN = fc["cSoVanBan"];
                v.CCOQUANCHUYENDEN = fc["cCoQuanChuyenDen"];
                v.CNOINHAN = fc["cNoiNhan"];
                v.IDON = iDon;
                v.CLOAI = "chuyendon_khongthuocthamquyen";
                v.CCHUCVU = func.RemoveTagInput(fc["cChucVu"]);

                kntc.Vanban_insert(v);
                int iVanban_last = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanban_last;
                        kntc.Upload_file(f);
                    }
                }
                int iDonViCHuyen = Convert.ToInt32(fc["iDonVi"]);
                KNTC_DON don = kntc.GetDON(iDon);
                don.ITINHTRANGXULY = 3;
                don.IDONVITHULY = Convert.ToInt32(fc["iDonVi"]);
                kntc.Update_Don(don);
                int iUser = id_user();
                // Lưu lịch sử đơn khi chuyển cho đơn vị khác xử lý
                if (donvithuly != iDonVi)
                {
                    KNTC_DON_LICHSU lichsu = new KNTC_DON_LICHSU();
                    lichsu.IDON = iDon;
                    lichsu.IDONVIGUI = iDonVi;
                    lichsu.IDONVITIEPNHAN = donvithuly;
                    lichsu.ITRANGTHAI = (int)don.ITINHTRANGXULY;
                    lichsu.IUSER = iUser;
                    lichsu.DNGAYCHUYEN = DateTime.Now;
                    lichsu.ICHUYENXULY = (int)LoaiLichSu.ChuyenXuLy;
                    lichsu.IDONVIXULY = don.IDONVITHULY;
                    lichsu.IVANBAN = iVanban_last;
                    kntc.InsertLichSuDon(lichsu);
                }
                kntc.Tracking_KNTC(iUser, iDon, "Chuyển đơn đến đơn vị: " + kntc.GetDonVi(iDonViCHuyen).CTEN);
                //Response.Redirect(Request.Cookies["url_return"].Value);
                if (Request.Cookies["link_action"].Value == "Tiepnhan")
                {
                    Response.Redirect("/Kntc/Tiepnhan/" + "#success");
                }
                else
                {
                    Response.Redirect(Request.Cookies["url_return"].Value);

                }
                //Response.Redirect("/Kntc/Chuyenxuly/#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Chuyển xử lý");
                return null;
            }
        }
        public ActionResult Ajax_Chuyenxuly_donvi(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                ViewData["id"] = fc["id"];
                ViewData["opt-luutheodoi"] = kn.Option_LuuTheoDoi();
                return PartialView("../Ajax/Kntc/Chuyenxuly_donvi");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form chuyển xử lý");
                return null;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Chuyenxuly_donvi_insert(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int iUser = id_user();
                KNTC_DON don = kntc.GetDON(iDon);

                int iHinhThuc = Convert.ToInt32(fc["iHinhThuc"]);
                if (iHinhThuc == 1)
                {
                    don.ITINHTRANGXULY = 4;
                    kntc.Update_Don(don);
                    kntc.Tracking_KNTC(iUser, iDon, "Chuyển xử lý, giải quyết đơn");
                    // Response.Redirect("/Kntc/Chuyenxuly/#success");

                }
                else
                {
                    int iLyDo = Convert.ToInt32(fc["iTheoDoi"]);
                    KNTC_LUUTHEODOI theodoi = kntc.Get_LuuTheoDoi(iLyDo);
                    don.CLUUTHEODOI_LYDO = theodoi.CTEN;
                    don.ITINHTRANGXULY = 5;
                    don.ILUUTHEODOI = Convert.ToDecimal(LyDoLuuTheoDoi.KhongXuLy);
                    don.IDONVITHULY = tl.IDDonVi_User(iUser);
                    kntc.Update_Don(don);
                    kntc.Tracking_KNTC(iUser, iDon, "Không xử lý, giải quyết với lý do: " + theodoi.CTEN);
                }
                // Response.Redirect(Request.Cookies["url_return"].Value);

                if (Request.Cookies["link_action"].Value == "Tiepnhan")
                {
                    Response.Redirect("/Kntc/Tiepnhan/" + "#success");
                }
                else
                {
                    Response.Redirect("/Kntc/Chuyenxuly/#success");

                }
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Chuyển xử lý");
                return null;
            }
        }
        public ActionResult Ajax_Luutheodoi(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                ViewData["id"] = fc["id"];
                ViewData["idontrung"] = fc["idontrung"];
                ViewData["opt-luutheodoi"] = kn.Option_LuuTheoDoi();
                return PartialView("../Ajax/Kntc/Luutheodoi");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị lưu theo dõi");
                return null;
            }
        }
        public ActionResult Ajax_Vanbandondoc(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor ui = GetUserInfor();
                ViewData["id"] = fc["id"];
                string id_decrypt = HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                SetTokenAction("kntc_vanbandondoc_add", iDon);
                ViewData["donvi"] = "<option value=" + ui.user_login.IDONVI + ">" + ui.tk_action.tendonvi + "</option>";
                return PartialView("../Ajax/Kntc/Vanbandondoc_add");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Văn bản đôn đốc");
                return null;
            }
        }
        public ActionResult Ajax_Luanchuyendonthu(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor ui = GetUserInfor();
                ViewData["id"] = fc["id"];
                string id = fc["id"];
                UserInfor u_info = GetUserInfor();
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                int iDonvi = (int)ui.user_login.IDONVI;
                int iDonViGroup = ui.tk_action.is_groupquochoi;
                SetTokenAction("kntc_luanchuyendonthu_add", iDon);
                ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(0);
                if (u_info.tk_action.is_dbqh)
                {
                    ViewData["is_dbqh"] = "1";
                    //ViewData["opt-donvithamquyen"] = Get_Option_ThamQuyen_TrungUong(ID_Ban_DanNguyen);
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong, ID_Ban_DanNguyen);
                    ViewData["opt-donvithamquyen-diaphuong"] = Get_Option_ThamQuyen_DiaPhuong(0);
                    ViewData["donvi"] = "<option value=" + u_info.user_login.IDONVI + ">" + u_info.tk_action.tendonvi + "</option>";
                }
                else
                {
                    ViewData["is_dbqh"] = "0";
                    //ViewData["opt-donvithamquyen"] = Get_Option_ThamQuyen_TrungUong(ID_Ban_DanNguyen);
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong, ID_Ban_DanNguyen);
                    ViewData["opt-donvithamquyen-diaphuong-parent"] = Get_Option_ThamQuyen_DiaPhuong_Parent(0);

                    Dictionary<string, object> condition = new Dictionary<string, object>();
                    condition.Add("IHIENTHI", 1);
                    condition.Add("IDELETE", 0);
                    List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
                    ViewData["donvi"] = kn.OptionCoQuan(coquan, 0, 0, iDonvi, 0);
                }
                return PartialView("../Ajax/Kntc/Vanbanluanchuyen_add");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Văn bản đôn đốc");
                return null;
            }
        }
        public ActionResult Ajax_Chuathoadang(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor ui = GetUserInfor();
                ViewData["id"] = fc["id"];
                string id = fc["id"];
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                int iDonvi = (int)ui.user_login.IDONVI;
                int iDonViGroup = ui.tk_action.is_groupquochoi;
                int iType = 0;
                int iUser = ui.tk_action.iUser;
                Dictionary<string, object> param = new Dictionary<string, object>();
                var don = kntc.GetDON(iDon);
                param.Add("ICOQUAN", don.IDONVITHULY);
                var coquanchon = _kiennghi.GetAll_CoQuanByParam(param).FirstOrDefault();
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Trunguong;
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Tinh;
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    iType = (int)ThamQuyen_DiaPhuong.Huyen;
                ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(iType);
                ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType(iType, (int)coquanchon.ICOQUAN);
                SetTokenAction("kntc_vanbanchuathoadang_add", iDon);
                Dictionary<string, object> condition = new Dictionary<string, object>();
                condition.Add("IHIENTHI", 1);
                condition.Add("IDELETE", 0);
                List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
                ViewData["donvitiepnhan"] = kn.OptionCoQuan(coquan, 0, 0, 0, 0);
                //ViewData["donvi"] = kn.OptionCoQuan(0, 0, iDonvi, 0);
                //ViewData["donvi"] = "<option value=" + iDonvi + ">" + ui.tk_action.tendonvi + "</option>";
                ViewData["idonvi"] = ui.user_login.IDONVI;
                ViewData["sdonvi"] = ui.tk_action.tendonvi;
                return PartialView("../Ajax/Kntc/Vanbanxulylaidon_add");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Văn bản chưa thỏa đáng");
                return null;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Chuathoadang_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor u = GetUserInfor();
                file = Request.Files["file_upload1"];
                string id = fc["id"];
                int donvithuly = Convert.ToInt32(fc["iDonViTiepNhan"]);
                int iDonVi = (int)u.user_login.IDONVI;
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                if (!CheckTokenAction("kntc_vanbanchuathoadang_add", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        CheckFile_Upload(file);
                    }
                }
                KNTC_DON don = kntc.GetDON(iDon);
                KNTC_VANBAN v = new KNTC_VANBAN();
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                v.ICOQUANNHAN = 0;
                v.IUSER = id_user();
                v.DDATE = DateTime.Now;
                if (fc["dNgayBanHanh"] == "")
                {
                    v.DNGAYBANHANH = null;
                }
                else
                {
                    v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                }

                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.IDON = iDon;
                v.CLOAI = "chuyenxulylaidon";
                v.CCHUCVU = func.RemoveTagInput(fc["cChucVu"]);
                kntc.Vanban_insert(v);
                int iVanban_last = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanban_last;
                        kntc.Upload_file(f);
                    }
                }
                int iUser = id_user();
                // Lưu lịch sử đơn khi chuyển cho đơn vị khác xử lý
                if (donvithuly != iDonVi)
                {
                    KNTC_DON_LICHSU lichsu = new KNTC_DON_LICHSU();
                    lichsu.IDON = iDon;
                    lichsu.IDONVIGUI = iDonVi;
                    lichsu.IDONVITIEPNHAN = donvithuly;
                    lichsu.ITRANGTHAI = (int)don.ITINHTRANGXULY;
                    lichsu.IUSER = iUser;
                    lichsu.DNGAYCHUYEN = DateTime.Now;
                    int iloai = Convert.ToInt32(fc["iLoai"]);
                    if (iloai == 1) lichsu.ICHUYENXULY = (int)LoaiLichSu.Luanchuyen;
                    else lichsu.ICHUYENXULY = (int)LoaiLichSu.ChuyenXuLy;
                    lichsu.IDONVIXULY = don.IDONVITHULY;
                    lichsu.IVANBAN = iVanban_last;
                    kntc.InsertLichSuDon(lichsu);
                }
                kntc.Tracking_KNTC(iUser, (int)don.IDON, "Chuyển đến " + kntc.GetDonVi(donvithuly).CTEN + " để xử lý lại đơn chưa thỏa đáng, văn bản số: " + v.CSOVANBAN);
                don.ITINHTRANGXULY = (decimal)TrangThaiDon.ChoXuLy;
                don.IDONVITHULY = donvithuly;
                don.IDANHGIA = 0;
                kntc.Update_Don(don);
                Response.Redirect(Request.Cookies["url_return"].Value);
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Thêm mới văn bản chuyển xử lý đơn thư xử lý chwua thỏa đáng");
                return null;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Vanbanluanchuyendonthu_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor u = GetUserInfor();
                file = Request.Files["file_upload1"];
                string id = fc["id"];
                int iDonVi = (int)u.user_login.IDONVI;
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                if (!CheckTokenAction("kntc_luanchuyendonthu_add", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        CheckFile_Upload(file);
                    }
                }
                int idonvi = 0;
                if (Convert.ToInt32(fc["iThamQuyen"]) == 2)
                {
                    idonvi = Convert.ToInt32(fc["iDonVi_DiaPhuong"]);
                }
                else
                {
                    if(fc["iDonVi"] != null)
                        idonvi = Convert.ToInt32(fc["iDonVi"]);
                }
                KNTC_DON don = kntc.GetDON(iDon);
                KNTC_VANBAN v = new KNTC_VANBAN();
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.ICOQUANBANHANH = idonvi;
                v.ICOQUANNHAN = 0;
                v.IUSER = id_user();
                v.DDATE = DateTime.Now;
                if (fc["dNgayBanHanh"] == "")
                {
                    v.DNGAYBANHANH = null;
                }
                else
                {
                    v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                }

                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.IDON = iDon;
                v.CLOAI = "vanbanluanchuyendonthu";
                v.CCHUCVU = func.RemoveTagInput(fc["cChucVu"]);
                kntc.Vanban_insert(v);
                int iVanban_last = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanban_last;
                        kntc.Upload_file(f);
                    }
                }
                int iUser = id_user();
                // Lưu lịch sử đơn khi chuyển cho đơn vị khác xử lý
                if (idonvi != iDonVi)
                {
                    KNTC_DON_LICHSU lichsu = new KNTC_DON_LICHSU();
                    lichsu.IDON = iDon;
                    lichsu.IDONVIGUI = iDonVi;
                    lichsu.IDONVITIEPNHAN = idonvi;
                    lichsu.ITRANGTHAI = (int)don.ITINHTRANGXULY;
                    lichsu.IUSER = iUser;
                    lichsu.DNGAYCHUYEN = DateTime.Now;
                    lichsu.ICHUYENXULY = (int)LoaiLichSu.Luanchuyen;
                    lichsu.IDONVIXULY = (int)don.IDONVITHULY;
                    lichsu.IVANBAN = iVanban_last;
                    kntc.InsertLichSuDon(lichsu);
                }
                string tenCoquan = "";
                QUOCHOI_COQUAN cq = kntc.GetDonVi(idonvi);
                if (cq != null) tenCoquan = cq.CTEN;
                kntc.Tracking_KNTC(iUser, (int)don.IDON, "Luân chuyển đơn thư đến " + tenCoquan + " theo văn bản số: " + v.CSOVANBAN);
                don.ITINHTRANGXULY = (int)TrangThaiDon.ChoXuLy;

                don.IDONVITHULY = idonvi;
                kntc.Update_Don(don);
                if (Request.Cookies["link_action"].Value == "Tiepnhan")
                {
                    Response.Redirect("/Kntc/Tiepnhan/" + "#success");
                }
                else
                {
                    Response.Redirect(Request.Cookies["url_return"].Value);

                }
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Thêm mới văn bản luân chuyển đơn thư");
                return null;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Vanbandondoc_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                file = Request.Files["file_upload1"];
                string id = fc["id"];
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                if (!CheckTokenAction("kntc_vanbandondoc_add", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        CheckFile_Upload(file);
                    }
                }
                KNTC_DON don = kntc.GetDON(iDon);
                KNTC_VANBAN v = new KNTC_VANBAN();
                KNTC_VANBAN dongui = kntc.List_VanBan().Where(x => x.IDON == iDon && (x.CLOAI == "chuyenxulylaidon" || x.CLOAI == "chuyenxuly_noibo") && x.DDATE <= DateTime.Now).OrderByDescending(x => x.DNGAYBANHANH).FirstOrDefault();
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                v.ICOQUANNHAN = dongui.ICOQUANNHAN;
                v.IUSER = id_user();
                v.DDATE = DateTime.Now;
                if (fc["dNgayBanHanh"] == "")
                {
                    v.DNGAYBANHANH = null;
                }
                else
                {
                    v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                }
                if (fc["dThoiGianBaoCao"] == "")
                {
                    v.THOIGIANBC = null;
                }
                else
                {
                    v.THOIGIANBC = Convert.ToDateTime(func.ConvertDateToSql(fc["dThoiGianBaoCao"]));
                }

                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.IDON = iDon;
                v.CLOAI = "vanbandondocthuchien";
                v.CCHUCVU = func.RemoveTagInput(fc["cChucVu"]);
                kntc.Vanban_insert(v);
                int iVanban_last = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanban_last;
                        kntc.Upload_file(f);
                    }
                }
                int iUser = id_user();
                kntc.Tracking_KNTC(iUser, (int)don.IDON, "Thêm Văn bản đôn đốc thực hiện quyết định giải quyết số " + v.CSOVANBAN);
                kntc.Update_Don(don);
                Response.Redirect(Request.Cookies["url_return"].Value);
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Thêm mới văn bản đôn đốc thực hiện");
                return null;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Lydotrung_insert(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor ui = GetUserInfor();
                string id = fc["id"];
                string idtrung = fc["idontrung"];
                int idontrung = Convert.ToInt32(HashUtil.Decode_ID(idtrung, Request.Cookies["url_key"].Value));


                if (id != "")
                {
                    int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                    KNTC_DON don = kntc.GetDON(iDon);
                    don.ILUUTHEODOI = Convert.ToDecimal(LyDoLuuTheoDoi.DonTrung);
                    don.CLUUTHEODOI_LYDO = func.RemoveTagInput(fc["cLuuTheoDoi_LyDo"]);
                    don.ISOLUONGTRUNG = Convert.ToInt32(fc["iSoLuongTrung"]);
                    don.ITINHTRANGXULY = 5; don.IDONVITHULY = 4;
                    don.IDONVITIEPNHAN = ui.user_login.IDONVI;
                    KNTC_DON dontrung = kntc.GetDON(idontrung);
                    if (dontrung != null)
                    {
                        KNTC_DON dongoc = kntc.GetDON((int)dontrung.IDONTRUNG);
                        if (dongoc != null)
                        {
                            don.IDONTRUNG = dongoc.IDON;
                            dongoc.IDONTRUNG = -1;
                            kntc.Update_Don(dongoc);
                        }
                        else
                        {
                            don.IDONTRUNG = idontrung;
                            dontrung.IDONTRUNG = -1;
                            kntc.Update_Don(dontrung);
                        }

                    }
                    else { don.IDONTRUNG = idontrung; }
                    kntc.Update_Don(don);
                    //KNTC_DON dontrung = kntc.GetDON((int)don.IDONTRUNG);
                    //if (dontrung != null)
                    //{
                    //    don.ILOAIDON = dontrung.ILOAIDON;
                    //    don.ILINHVUC = dontrung.ILINHVUC;
                    //    don.INOIDUNG = dontrung.INOIDUNG;
                    //    don.ITINHCHAT = dontrung.ITINHCHAT;
                    //    don.IDIAPHUONG_0 = dontrung.IDIAPHUONG_0;
                    //    don.IDIAPHUONG_1 = dontrung.IDIAPHUONG_1;
                    //    don.IDIAPHUONG_2 = dontrung.IDIAPHUONG_2;
                    //    don.IDOANDONGNGUOI = dontrung.IDOANDONGNGUOI;
                    //    don.IDOKHAN = dontrung.IDOKHAN;
                    //    don.IDOMAT = dontrung.IDOMAT;
                    //    don.CNOIDUNG = dontrung.CNOIDUNG;
                    //    don.CNOIDUNG = dontrung.CNOIDUNG;
                    //    kntc.Update_Don(don);
                    //}
                    int iUser = id_user();
                    kntc.Tracking_KNTC(iUser, iDon, "Chọn trùng đơn, lưu theo dõi");
                    Response.Redirect("/Kntc/Tiepnhan/" + "#success");
                }
                else
                {
                    KNTC_DON donmoi = (KNTC_DON)Session[KNTCDonSession];
                    KNTC_DON don = new KNTC_DON();

                    don.CNGUOIGUI_TEN = donmoi.CNGUOIGUI_TEN;
                    don.ITHAMQUYEN = -1;
                    don.ITHULY = 0;
                    don.IDUDIEUKIEN = -1;
                    don.IDUDIEUKIEN_KETQUA = 0;
                    don.ITINHTRANG_DONVIXULY = 0;
                    don.ITINHTRANG_NOIBO = 0;
                    don.IUSER_GIAOXULY = 0;
                    don.IUSER_DUOCGIAOXULY = 0;
                    int iDonTrung = Convert.ToInt32(HashUtil.Decode_ID(idtrung, Request.Cookies["url_key"].Value));
                    KNTC_DON dontrung1 = kntc.GetDON(iDonTrung);
                    decimal dontrunggoc = 0;
                    if (dontrung1 != null)
                    {
                        KNTC_DON dongoc = kntc.GetDON((int)dontrung1.IDONTRUNG);
                        if (dongoc != null)
                        {
                            don.IDONTRUNG = dongoc.IDON;
                            dontrunggoc = dongoc.IDON;
                            dongoc.IDONTRUNG = -1;
                            kntc.Update_Don(dongoc);
                        }
                        else
                        {
                            don.IDONTRUNG = idontrung;
                            dontrunggoc = idontrung;
                            dontrung1.IDONTRUNG = -1;
                            kntc.Update_Don(dontrung1);
                        }

                    }
                    else { don.IDONTRUNG = idontrung; dontrunggoc = idontrung; }
                    KNTC_DON dontrung = kntc.GetDON(idontrung);
                    don.DNGAYNHAN = dontrung.DNGAYNHAN;
                    don.INGUONDON = dontrung.INGUONDON;
                    don.CNGUOIGUI_CMND = dontrung.CNGUOIGUI_CMND;
                    don.IDIAPHUONG_1 = dontrung.IDIAPHUONG_1;
                    don.IDOANDONGNGUOI = 1;
                    don.ISONGUOI = dontrung.ISONGUOI;
                    don.IDIAPHUONG_2 = 0;
                    don.IUSER = ui.user_login.IUSER;
                    don.DDATE = DateTime.Now;
                    don.IDONVITHULY = ui.user_login.IDONVI;
                    don.ITHAMQUYEN = -1;
                    don.ITHULY = 0;
                    don.IDUDIEUKIEN = -1;
                    don.IDUDIEUKIEN_KETQUA = 0;
                    don.ITINHTRANG_DONVIXULY = 0;
                    don.ITINHTRANG_NOIBO = 0;
                    don.IUSER_GIAOXULY = 0;
                    don.IUSER_DUOCGIAOXULY = 0;
                    don.ILUUTHEODOI = 0;
                    don.INGUOIGUI_DANTOC = dontrung.INGUOIGUI_DANTOC;
                    don.INGUOIGUI_QUOCTICH = dontrung.INGUOIGUI_QUOCTICH;
                    don.ILOAIDON = dontrung.ILOAIDON;
                    don.ILINHVUC = dontrung.ILINHVUC;
                    don.INOIDUNG = dontrung.INOIDUNG;
                    don.ITINHCHAT = dontrung.ITINHCHAT;
                    don.IDIAPHUONG_0 = dontrung.IDIAPHUONG_0;
                    don.IDIAPHUONG_1 = dontrung.IDIAPHUONG_1;
                    don.IDIAPHUONG_2 = dontrung.IDIAPHUONG_2;
                    don.IDOANDONGNGUOI = dontrung.IDOANDONGNGUOI;
                    don.IDOKHAN = dontrung.IDOKHAN;
                    don.IDOMAT = dontrung.IDOMAT;
                    don.CNOIDUNG = dontrung.CNOIDUNG;
                    don.ILUUTHEODOI = Convert.ToDecimal(LyDoLuuTheoDoi.DonTrung);
                    don.CLUUTHEODOI_LYDO = func.RemoveTagInput(fc["cLuuTheoDoi_LyDo"]);
                    don.ISOLUONGTRUNG = Convert.ToInt32(fc["iSoLuongTrung"]);
                    don.ITINHTRANGXULY = 5;
                    don.IDONTRUNG = idontrung;
                    don.IDONVITIEPNHAN = ui.user_login.IDONVI;
                    kntc.TiepNhan_Don(don);

                    int iUser = id_user();
                    kntc.Tracking_KNTC(iUser, (int)don.IDON, "Chọn trùng đơn, lưu theo dõi");
                    Session[KNTCDonSession] = null;
                    Response.Redirect("/Kntc/Tiepnhan/" + "#success");
                }
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Thêm mới ly do trùng");
                return null;
            }
        }
        public ActionResult Ajax_Lydotrung()
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                ViewData["id"] = Request["id"];
                ViewData["itrung"] = Request["itrung"];
                return PartialView("../Ajax/Kntc/Lydotrung");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form theo dõi");
                return null;
            }
        }
        public ActionResult Ajax_DanhGia()
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                ViewData["danhgia"] = kn.Option_Danhgia(0);
                string id = Request["id"];
                ViewData["id"] = id;
                return PartialView("../Ajax/Kntc/Ajax_DanhGia");
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
        public ActionResult Ajax_Danhgia_insert(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor ui = GetUserInfor();
                string id = fc["id"];
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_DON don = kntc.GetDON(iDon);
                don.IDANHGIA = Convert.ToDecimal(fc["idanhgia"]);
                don.CGHICHUDANHGIA = fc["ghichu"];
                kntc.Update_Don(don);
                kntc.Tracking_KNTC((int)ui.user_login.IUSER, (int)don.IDON, "Đánh giá đơn đã xử lý");
                Response.Redirect("/Kntc/Daxuly/" + "#success");
                return null;
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
        public ActionResult Ajax_Luutheodoi_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {

                int iUser = id_user();
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string lydo = fc["cLuuTheoDoi_LyDo"];
                KNTC_DON don = kntc.GetDON(iDon);
                don.ILUUTHEODOI = Convert.ToDecimal(LyDoLuuTheoDoi.KhongXuLy);
                int iLyDo = Convert.ToInt32(fc["iTheoDoi"]);
                KNTC_LUUTHEODOI theodoi = kntc.Get_LuuTheoDoi(iLyDo);
                if (theodoi != null)
                {
                    don.CLUUTHEODOI_LYDO = theodoi.CTEN;
                    don.ICHITIETLYDOLUUDON = theodoi.ID;
                }
                don.CHITIETLYDO_LUUTHEODOI = lydo;
                don.ITINHTRANGXULY = 5;
                don.IDUDIEUKIEN = 0;
                don.IDUDIEUKIEN_KETQUA = 1;
                don.IDONVITHULY = tl.IDDonVi_User(iUser);
                don.ITHULY = 1;
                
                kntc.Update_Don(don);
                kntc.Tracking_KNTC(iUser, iDon, "Cập nhật lưu đơn, theo dõi");

                if (Request.Cookies["link_action"].Value == "Tiepnhan")
                {
                    Response.Redirect("/Kntc/Khongxuly/" + "#success");
                }
                else
                {
                    Response.Redirect("/Kntc/Khongxuly/#success");

                }
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Cập nhật lưu đơn, không xử lý");
                return null;
            }
        }
        public ActionResult Ajax_Huongdan_traloi(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = fc["id"];
                SetTokenAction("kntc_huongdan_traloi", iDon);
                return PartialView("../Ajax/Kntc/Huongdan_traloi");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form cập nhật hướng dẫn trả lời");
                return null;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Huongdan_traloi_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                file = Request.Files["file_upload1"];
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("kntc_huongdan_traloi", iDon)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        CheckFile_Upload(file);
                    }

                }

                KNTC_VANBAN v = new KNTC_VANBAN();
                v.ICOQUANBANHANH = Convert.ToInt32(fc["iDonVi"]);
                v.ICOQUANNHAN = Convert.ToInt32(fc["iDonVi"]);
                v.IUSER = id_user();
                v.DDATE = DateTime.Now;
                if (fc["dNgayBanHanh"] != "")
                {
                    v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                }
                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.CCHUCVU = func.RemoveTagInput(fc["cChucVu"]);
                v.GHICHU_XULY = func.RemoveTagInput(fc["cGhiChu"]);
                string subPath = "/upload/kntc/";
                bool IsExists = Directory.Exists(Server.MapPath(subPath));
                if (!IsExists)
                {
                    Directory.CreateDirectory(Server.MapPath(subPath));
                }
                file = Request.Files["file_upload1"];
                int file_length = file.ContentLength;
                if (file != null && file_length > 0)
                {
                    string file_path = subPath + DateTime.Now.ToString("Hmmss") + "_" + func.ConvertVn(file.FileName);
                    file.SaveAs(Server.MapPath(file_path));
                    v.CFILE = file_path;
                }
                else
                {
                    v.CFILE = "";
                }
                v.IDON = iDon;
                v.CLOAI = "huongdan_traloi";

                kntc.Vanban_insert(v);
                int iVanban = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kntc_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanban;
                        kntc.Upload_file(f);
                    }
                }
                KNTC_DON don = kntc.GetDON(iDon);
                don.ITINHTRANGXULY = 10;
                don.IDUDIEUKIEN_KETQUA = 3;
                don.IDONVITHULY = ID_UY_BAN_NHAN_DAN;
                don.ITHULY = 1;
                kntc.Update_Don(don);
                int iUser = id_user();
                kntc.Tracking_KNTC(iUser, iDon, "Hướng dẫn, giải thích trả lời đơn");

                if (Request.Cookies["link_action"].Value == "Tiepnhan")
                {
                    Response.Redirect("/Kntc/Tiepnhan/" + "#success");
                }
                else
                {
                    //Response.Redirect("/Kntc/Daxuly/#success");
                    Response.Redirect("/Kntc/Dahuongdan/#success");

                }

                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Thêm mới hướng dẫn, giải thích trả lời đơn");
                return null;
            }
        }
        public ActionResult Ajax_Moicapnhat_search(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("44,10,11,12,13", act)) { return null; }
                string key = Request["ip_noidung"].ToString();
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                var don = kntc.GetListAllDon(key, tungay, denngay).Where(v => v.ITINHTRANGXULY == 0 && v.IDONVITIEPNHAN == u.user_login.IDONVI).ToList();
                if (!_base.ActionMulty_("44", act))
                {
                    ViewData["btn-add"] = " style='display:none' ";
                    don = don.Where(v => v.IUSER == u.user_login.IUSER).ToList();

                }
                string caution = "";
                if (don.Count() > 0)
                {

                    caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + don.Count() + " kết quả tìm kiếm</td></tr>";
                }
                else
                {
                    caution = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào</td></tr>";
                }
                // ViewData["list"] = caution + kn.Don_Moicapnhat(don, act, 0);

                return PartialView("../Ajax/Kntc/Moicapnhat_search");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn mới cập nhật");
                return null;
            }
        }
        public ActionResult Ajax_Moicapnhat_searchtotal(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {

                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("44,10,11,12,13", act)) { return null; }
                string key = fc["cNoiDung"].ToString();
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                int tinhthanh = Convert.ToInt32(fc["iDiaPhuong_0"]);
                int nguondon = Convert.ToInt32(fc["iNguonDon"]);
                int quoctich = Convert.ToInt32(fc["iNguoiGui_QuocTich"]);
                int dantoc = Convert.ToInt32(fc["iNguoiGui_DanToc"]);
                int doandongnguoi = 0;
                if (Request["iDoanDongNguoi"] != null)
                {
                    doandongnguoi = 1;
                }
                int loaidon = Convert.ToInt32(fc["iLoaiDon"]);
                int linhvuc = Convert.ToInt32(fc["iLinhVuc"]);
                int noidung = Convert.ToInt32(fc["iNoiDung"]);
                int tinhchat = Convert.ToInt32(fc["iTinhChat"]);
                if (fc["dTuNgay"] != null && fc["dTuNgay"].ToString() != "")
                {
                    tungay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));
                }
                if (fc["dDenNgay"] != null && fc["dDenNgay"].ToString() != "")
                {
                    denngay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));
                }
                var don = kntc.GetListAllDon(key, tungay, denngay, doandongnguoi, nguondon, tinhthanh, quoctich, dantoc, loaidon, linhvuc, noidung, tinhchat).Where(v => v.ITINHTRANGXULY == 0 && v.IDONVITIEPNHAN == u.user_login.IDONVI).ToList();

                if (!_base.ActionMulty_("44", act))
                {
                    ViewData["btn-add"] = " style='display:none' ";
                    don = don.Where(v => v.IUSER == u.user_login.IUSER).ToList();

                }
                string caution = "";
                if (don.Count() > 0)
                {

                    caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + don.Count() + " kết quả tìm kiếm</td></tr>";
                }
                else
                {
                    caution = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào</td></tr>";
                }
                // ViewData["list"] = caution + kn.Don_Moicapnhat(don, act, 0);

                return PartialView("../Ajax/Kntc/Moicapnhat_search");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Tìm kiếm danh sách đơn mới cập nhật");
                return null;
            }
        }
        public ActionResult Ajax_Moicapnhat_formsearch()
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {

                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                return PartialView("../Ajax/Kntc/Moicapnhat_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm đơn mới cập nhật");
                return null;
            }
        }
        public KNTC_DON get_Request_Paramt_KNTC()
        {
            KNTC_DON kn = new KNTC_DON();

            if (Request["iDoanDongNguoi"] != null)
            {
                kn.IDOANDONGNGUOI = 1;
            }
            else { kn.IDOANDONGNGUOI = 0; }

            if (Request["iNguonDon"] != null)
            {
                kn.INGUONDON = Convert.ToInt32(Request["iNguonDon"]);
            }
            else { kn.INGUONDON = 0; }

            if (Request["iDiaPhuong_0"] != null)
            {
                kn.IDIAPHUONG_0 = Convert.ToInt32(Request["iDiaPhuong_0"]);
            }
            else { kn.IDIAPHUONG_0 = 0; }

            if (Request["iDiaPhuong_1"] != null)
            {
                kn.IDIAPHUONG_1 = Convert.ToInt32(Request["iDiaPhuong_1"]);
            }
            else { kn.IDIAPHUONG_1 = 0; }

            if (Request["iDiaPhuong_2"] != null)
            {
                kn.IDIAPHUONG_2 = Convert.ToInt32(Request["iDiaPhuong_2"]);
            }
            else { kn.IDIAPHUONG_2 = 0; }

            if (Request["iNguoiGui_QuocTich"] != null)
            {
                kn.INGUOIGUI_QUOCTICH = Convert.ToInt32(Request["iNguoiGui_QuocTich"]);
            }
            else { kn.INGUOIGUI_QUOCTICH = 0; }

            if (Request["iNguoiGui_DanToc"] != null)
            {
                kn.INGUOIGUI_DANTOC = Convert.ToInt32(Request["iNguoiGui_DanToc"]);
            }
            else { kn.INGUOIGUI_DANTOC = 0; }

            if (Request["iLoaiDon"] != null)
            {
                kn.ILOAIDON = Convert.ToInt32(Request["iLoaiDon"]);
            }
            else { kn.ILOAIDON = 0; }

            if (Request["iLinhVuc"] != null)
            {
                kn.ILINHVUC = Convert.ToInt32(Request["iLinhVuc"]);
            }
            else { kn.ILINHVUC = 0; }

            if (Request["iNoiDung"] != null)
            {
                kn.INOIDUNG = Convert.ToInt32(Request["iNoiDung"]);
            }
            else { kn.INOIDUNG = 0; }

            if (Request["iTinhChat"] != null)
            {
                kn.ITINHCHAT = Convert.ToInt32(Request["iTinhChat"]);
            }
            else { kn.ITINHCHAT = 0; }
            if (Request["iNguoiNhap"] != null)
            {
                kn.IUSER = Convert.ToInt32(Request["iNguoiNhap"]);
            }
            else { kn.IUSER = 0; }
            if (Request["iDieuKienXuLy"] != null)
            {
                kn.IDUDIEUKIEN = Convert.ToInt32(Request["iDieuKienXuLy"]);
            }
            else { kn.IDUDIEUKIEN = -2; }
            return kn;
        }
        public ActionResult Moicapnhat(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                string url_cookie = func.Get_Url_keycookie();
                if (!_base.ActionMulty_Redirect_("44,10,11,12,13", act)) { Response.Redirect("/Home/Error/"); return null; }
                Dictionary<string, object> condition = new Dictionary<string, object>();
                //ActionMulty_Redirect("44,10,11,12,13", iUser);
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Moicapnhat");
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                //var don = kntc.GetListAllDon("",  tungay, denngay).Where(v => v.ITINHTRANGXULY == 0 && v.IDONVITIEPNHAN == u.user_login.IDONVI).ToList();
                string key = null;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null) 
                { 
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"])); 
                }
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITIEPNHAN", u.user_login.IDONVI);
                condition.Add("P_ITINHTRANGXULY", (int)TrangThaiDon.MoiCapNhat);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);
                if (!_base.ActionMulty_("44", act))
                {
                    ViewData["btn-add"] = " style='display:none' ";
                    condition.Add("P_IUSER", u.user_login.IUSER);
                }
                else
                {
                    condition.Add("P_IUSER", 0);
                }
                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                var don = kntc.ListDonMoiCapNhat(condition);
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    ViewData["list"] = caution + kn.Don_Moicapnhat(don, act, url_cookie);
                    ViewData["phantrang"] = "<tr><td colspan='5'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else ViewData["list"] = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";

                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn thư mới cập nhất");
                return View("../Home/Error_Exception");
            }
        }

        public ActionResult Tiepnhan(string id)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                int iUser = id_user();
                UserInfor u = GetUserInfor();
                TaikhoanAtion tk = _base.tk_action(iUser);
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("44,10", act)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Tiepnhan");
                SetTokenAction("kntc_themmoidon");
                ViewData["mo"] = "style='display:none'";
                ViewData["dong"] = "style='display:inline-block'";
                ViewData["nutkiemtrung"] = "style='display:inline-block'";
                ViewData["morong"] = "style='display:none;width: 100%;'";
                ViewData["opt-don-guiden"] = Get_Option_Don_GuiDen();
                KNTC_DON don = (KNTC_DON)Session[KNTCDonSession];
                if (don == null)
                {
                    if (id == null)
                    {
                        List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon().ToList();
                        if (u.user_login.ITYPE == (int)UserType.ChuyenVienHDND) nguondon = nguondon.Where(n => n.ILOAI == 1).ToList();
                        else nguondon = nguondon.Where(n => n.ILOAI == 0).ToList();
                        List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                        ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
                        int iKyHop = ID_KyHop_HienTai();
                        ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                        ViewData["opt-quoctich"] = kn.Option_QuocTich(233);
                        ViewData["opt-dantoc"] = kn.Option_DanToc(1);
                        _condition = new Dictionary<string, object>();
                        _condition.Add("IHIENTHI", 1);
                        _condition.Add("IDELETE", 0);
                        List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                        ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, AppConfig.IDIAPHUONG);
                        ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                        ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                        ViewData["opt-noidung"] = "";
                        ViewData["opt-tinhchat"] = "";
                        //ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, 0);
                        ViewData["opt-linhvuc"] = "";
                        ViewData["opt-domat"] = kn.Option_Domat(1);
                        ViewData["opt-dokhan"] = kn.Option_Dokhan(1);
                        ViewData["opt-chuyenvien-xuly"] = "";
                        ViewData["chuyenvien"] = "style='display:none;margin-top: 1%;'";
                        ViewData["opt-dieukienxuly"] = kn.Option_DieuKienXuLy();
                        ViewData["opt-hinhthucxuly"] = kn.Option_HinhThucXuLy();
                        if (_base.ActionMulty_("11,44", act))
                        {
                            ViewData["chuyenvien"] = "style='display:inline-block;margin-top: 1%;'";
                            _condition = new Dictionary<string, object>();
                            _condition.Add("IDONVI", (decimal)u.user_login.IDONVI);
                            _condition.Add("ISTATUS", 1);
                            List<USERS> lstUser = kntc.List_User(_condition);
                            _condition = new Dictionary<string, object>();
                            _condition.Add("IACTION", 12);
                            List<USER_ACTION> lstUserAction = kntc.List_UserAction(_condition);
                            ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, iUser);
                        }
                        ViewData["dNgayNhan"] = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    else
                    {

                        int iDon = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                        // ViewData["nutkiemtrung"] = "style='display:none'";
                        ViewData["id"] = id;
                        don = kntc.GetDON(iDon);
                        if (don != null)
                        {
                            ViewData["don"] = don;
                            string nguoinhan = ""; string cmnd = ""; string ngaynhan = ""; string sdt = "";
                            if (don.IDONTRUNG == 0)
                            {
                                if (don.CNGUOIGUI_TEN != null)
                                {
                                    nguoinhan = Server.HtmlEncode(don.CNGUOIGUI_TEN);
                                }
                                if (don.CNGUOIGUI_CMND != null)
                                {
                                    cmnd = Server.HtmlEncode(don.CNGUOIGUI_CMND);
                                }
                                if (don.DNGAYNHAN != null)
                                {
                                    ngaynhan = Convert.ToDateTime(don.DNGAYNHAN).ToString("dd/MM/yyyy");
                                }
                                if (don.CNGUOIGUI_SDT != null)
                                {
                                    sdt = Server.HtmlEncode(don.CNGUOIGUI_SDT);
                                }

                                ViewData["cNguoiGui_Ten"] = nguoinhan;
                                ViewData["cNguoiGui_SDT"] = sdt;
                                ViewData["dNgayNhan"] = ngaynhan;
                                ViewData["cNguoiGui_DiaChi"] = don.CNGUOIGUI_DIACHI;
                                ViewData["cNoiDung"] = don.CNOIDUNG;
                                List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                                ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, (int)don.INGUONDON);
                                int iKyHop = ID_KyHop_HienTai();
                                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                                ViewData["opt-quoctich"] = kn.Option_QuocTich((int)don.INGUOIGUI_QUOCTICH);
                                ViewData["opt-dantoc"] = kn.Option_DanToc((int)don.INGUOIGUI_DANTOC);
                                _condition = new Dictionary<string, object>();
                                _condition.Add("IHIENTHI", 1);
                                _condition.Add("IDELETE", 0);
                                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, AppConfig.IDIAPHUONG);
                                if (don.IDIAPHUONG_1 != null)
                                {
                                    ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, (int)don.IDIAPHUONG_1);

                                }
                                else
                                {
                                    ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                                }
                                ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)don.ILOAIDON);
                                ViewData["opt-noidung"] = kn.Option_NoiDungDon_ThuocLinhVuc((int)don.INOIDUNG, (int)don.ILINHVUC);
                                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon_ThuocNguonDon((int)don.ITINHCHAT, (int)don.INOIDUNG);
                                ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon((int)don.ILINHVUC, (int)don.ILOAIDON);
                                ViewData["opt-dieukienxuly"] = kn.Option_DieuKienXuLy(don.IDUDIEUKIEN);
                                ViewData["opt-hinhthucxuly"] = kn.Option_HinhThucXuLy(don.IDUDIEUKIEN_KETQUA);

                                ViewData["opt-chuyenvien-xuly"] = "";
                                ViewData["chuyenvien"] = "style='display:none;margin-top: 1%;'";
                                if (_base.ActionMulty_("11,44", act))
                                {
                                    ViewData["chuyenvien"] = "style='display:inline-block;margin-top: 1%;'";
                                    _condition = new Dictionary<string, object>();
                                    _condition.Add("IDONVI", (decimal)u.user_login.IDONVI);
                                    _condition.Add("ISTATUS", 1);
                                    List<USERS> lstUser = kntc.List_User(_condition);
                                    _condition = new Dictionary<string, object>();
                                    _condition.Add("IACTION", 12);
                                    List<USER_ACTION> lstUserAction = kntc.List_UserAction(_condition);
                                    ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, iUser);
                                    //ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC((decimal)u.user_login.IDONVI, iUser);
                                }
                                ViewData["opt-domat"] = kn.Option_Domat((int)don.IDOMAT);
                                ViewData["opt-dokhan"] = kn.Option_Dokhan((int)don.IDOKHAN);
                                ViewData["dong"] = "style='display:inline-block;'";
                                ViewData["mo"] = "style='display:none'";
                                if (don.ILOAIDON != 0 || don.ILINHVUC != 0 || don.INOIDUNG != 0 || don.ITINHCHAT != 0)
                                {
                                    ViewData["mo"] = "style='display:inline-block'";
                                    ViewData["dong"] = "style='display:none'";
                                    ViewData["morong"] = "style='display:inline-block;width: 100%;'";
                                }
                                else
                                {
                                    ViewData["morong"] = "style='display:none;width: 100%;'";
                                }
                            }
                            else
                            {
                                don = kntc.GetDON((int)don.IDONTRUNG);
                                if (don.CNGUOIGUI_TEN != null)
                                {
                                    nguoinhan = Server.HtmlEncode(don.CNGUOIGUI_TEN);
                                }
                                if (don.CNGUOIGUI_CMND != null)
                                {
                                    cmnd = Server.HtmlEncode(don.CNGUOIGUI_CMND);
                                } 
                                if (don.CNGUOIGUI_SDT != null)
                                {
                                    sdt = Server.HtmlEncode(don.CNGUOIGUI_SDT);
                                }

                                ViewData["cNguoiGui_Ten"] = nguoinhan;
                                ViewData["cNguoiGui_CMND"] = cmnd;
                                ViewData["cNguoiGui_SDT"] = sdt;
                                ViewData["dNgayNhan"] = DateTime.Now.ToString("dd/MM/yyyy");
                                ViewData["cNguoiGui_DiaChi"] = don.CNGUOIGUI_DIACHI;
                                ViewData["cNoiDung"] = don.CNOIDUNG;
                                List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                                ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, (int)don.INGUONDON);
                                int iKyHop = ID_KyHop_HienTai();
                                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                                ViewData["opt-quoctich"] = kn.Option_QuocTich((int)don.INGUOIGUI_QUOCTICH);
                                ViewData["opt-dantoc"] = kn.Option_DanToc((int)don.INGUOIGUI_DANTOC);
                                _condition = new Dictionary<string, object>();
                                _condition.Add("IHIENTHI", 1);
                                _condition.Add("IDELETE", 0);
                                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, AppConfig.IDIAPHUONG);
                                if (don.IDIAPHUONG_1 != null)
                                {
                                    ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, (int)don.IDIAPHUONG_0, (int)don.IDIAPHUONG_1);

                                }
                                else
                                {
                                    ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, (int)don.IDIAPHUONG_0, 0);
                                }
                                ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)don.ILOAIDON);
                                ViewData["opt-noidung"] = kn.Option_NoiDungDon_ThuocLinhVuc((int)don.INOIDUNG, (int)don.ILINHVUC);
                                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon_ThuocNguonDon((int)don.ITINHCHAT, (int)don.INOIDUNG);
                                ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon((int)don.ILINHVUC, (int)don.ILOAIDON);

                                ViewData["opt-chuyenvien-xuly"] = "";
                                ViewData["chuyenvien"] = "style='display:none;margin-top: 1%;'";
                                ViewData["chuyenvien"] = "style='display:none;margin-top: 1%;'";
                                if (_base.ActionMulty_("11,44", act))
                                {
                                    ViewData["chuyenvien"] = "style='display:inline-block;margin-top: 1%;'";
                                    //ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC((decimal)u.user_login.IDONVI, iUser);
                                    _condition = new Dictionary<string, object>();
                                    _condition.Add("IDONVI", (decimal)u.user_login.IDONVI);
                                    _condition.Add("ISTATUS", 1);
                                    List<USERS> lstUser = kntc.List_User(_condition);
                                    _condition = new Dictionary<string, object>();
                                    _condition.Add("IACTION", 12);
                                    List<USER_ACTION> lstUserAction = kntc.List_UserAction(_condition);
                                    ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, iUser);
                                }
                                ViewData["opt-domat"] = kn.Option_Domat((int)don.IDOMAT);
                                ViewData["opt-dokhan"] = kn.Option_Dokhan((int)don.IDOKHAN);
                                ViewData["dong"] = "style='display:inline-block;'";
                                ViewData["mo"] = "style='display:none'";
                                if (don.ILOAIDON != 0 || don.ILINHVUC != 0 || don.INOIDUNG != 0 || don.ITINHCHAT != 0)
                                {
                                    ViewData["mo"] = "style='display:inline-block'";
                                    ViewData["dong"] = "style='display:none'";
                                    ViewData["morong"] = "style='display:inline-block;width: 100%;'";
                                }
                                else
                                {
                                    ViewData["morong"] = "style='display:none;width: 100%;'";
                                }
                            }

                        }
                    }

                }
                else
                {
                    if (id == null)
                    {
                        ViewData["don"] = don;
                        string nguoinhan = ""; string cmnd = ""; string ngaynhan = "";string sdt = "";
                        if (don.IDONTRUNG == 0)
                        {
                            if (Server.HtmlEncode(don.CNGUOIGUI_TEN) != null)
                            {
                                nguoinhan = Server.HtmlEncode(don.CNGUOIGUI_TEN);
                            }
                            if (Server.HtmlEncode(don.CNGUOIGUI_CMND) != null)
                            {
                                cmnd = Server.HtmlEncode(don.CNGUOIGUI_CMND);
                            }
                            if (don.DNGAYNHAN != null)
                            {
                                ngaynhan = Convert.ToDateTime(don.DNGAYNHAN).ToString("dd/MM/yyyy");
                            } 
                            if (don.CNGUOIGUI_SDT != null)
                            {
                                sdt = Server.HtmlEncode(don.CNGUOIGUI_SDT);
                            }

                            ViewData["cNguoiGui_Ten"] = nguoinhan;
                            ViewData["cNguoiGui_CMND"] = cmnd;
                            ViewData["cNguoiGui_SDT"] = sdt;
                            ViewData["dNgayNhan"] = ngaynhan;
                            ViewData["cNguoiGui_DiaChi"] = Server.HtmlEncode(don.CNGUOIGUI_DIACHI);
                            ViewData["cNoiDung"] = Server.HtmlEncode(don.CNOIDUNG);
                            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, (int)don.INGUONDON);
                            int iKyHop = ID_KyHop_HienTai();
                            ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                            ViewData["opt-quoctich"] = kn.Option_QuocTich((int)don.INGUOIGUI_QUOCTICH);
                            ViewData["opt-dantoc"] = kn.Option_DanToc((int)don.INGUOIGUI_DANTOC);
                            _condition = new Dictionary<string, object>();
                            _condition.Add("IHIENTHI", 1);
                            _condition.Add("IDELETE", 0);
                            List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                            ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, AppConfig.IDIAPHUONG);
                            if (don.IDIAPHUONG_1 != null)
                            {
                                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, (int)don.IDIAPHUONG_1);

                            }
                            else
                            {
                                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                            }
                            ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)don.ILOAIDON);
                            ViewData["opt-noidung"] = kn.Option_NoiDungDon_ThuocLinhVuc((int)don.INOIDUNG, (int)don.ILINHVUC);
                            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon_ThuocNguonDon((int)don.ITINHCHAT, (int)don.INOIDUNG);
                            ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon((int)don.ILINHVUC, (int)don.ILOAIDON);

                            ViewData["opt-chuyenvien-xuly"] = "";
                            ViewData["chuyenvien"] = "style='display:none;margin-top: 1%;'";
                            if (_base.ActionMulty_("11,44", act))
                            {
                                ViewData["chuyenvien"] = "style='display:inline-block;margin-top: 1%;'";
                                _condition = new Dictionary<string, object>();
                                _condition.Add("IDONVI", (decimal)u.user_login.IDONVI);
                                _condition.Add("ISTATUS", 1);
                                List<USERS> lstUser = kntc.List_User(_condition);
                                _condition = new Dictionary<string, object>();
                                _condition.Add("IACTION", 12);
                                List<USER_ACTION> lstUserAction = kntc.List_UserAction(_condition);
                                ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, iUser);
                                //ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC((decimal)u.user_login.IDONVI, iUser);
                            }
                            ViewData["opt-domat"] = kn.Option_Domat((decimal)DoMat.Thuong);
                            ViewData["opt-dokhan"] = kn.Option_Dokhan((decimal)DoKhan.Thuong);
                            if (don.IDOMAT != null)
                            {
                                ViewData["opt-domat"] = kn.Option_Domat(don.IDOMAT);
                            }
                            if (don.IDOKHAN != null)
                            {
                                ViewData["opt-dokhan"] = kn.Option_Dokhan(don.IDOKHAN);
                            }
                            ViewData["dong"] = "style='display:inline-block;'";
                            ViewData["mo"] = "style='display:none'";
                            if (don.ILOAIDON != 0 || don.ILINHVUC != 0 || don.INOIDUNG != 0 || don.ITINHCHAT != 0)
                            {
                                ViewData["mo"] = "style='display:inline-block'";
                                ViewData["dong"] = "style='display:none'";
                                ViewData["morong"] = "style='display:inline-block;width: 100%;'";
                            }
                            else
                            {
                                ViewData["morong"] = "style='display:none;width: 100%;'";
                            }
                        }
                        else
                        {
                            don = kntc.GetDON((int)don.IDONTRUNG);
                            if (Server.HtmlEncode(don.CNGUOIGUI_TEN) != null)
                            {
                                nguoinhan = Server.HtmlEncode(don.CNGUOIGUI_TEN);
                            }
                            if (Server.HtmlEncode(don.CNGUOIGUI_CMND) != null)
                            {
                                cmnd = Server.HtmlEncode(don.CNGUOIGUI_CMND);
                            }
                            if (don.CNGUOIGUI_SDT != null)
                            {
                                sdt = Server.HtmlEncode(don.CNGUOIGUI_SDT);
                            }

                            ViewData["cNguoiGui_Ten"] = nguoinhan;
                            ViewData["cNguoiGui_CMND"] = cmnd;
                            ViewData["cNguoiGui_SDT"] = sdt;
                            ViewData["dNgayNhan"] = DateTime.Now.ToString("dd/MM/yyyy");
                            ViewData["cNguoiGui_DiaChi"] = Server.HtmlEncode(don.CNGUOIGUI_DIACHI);
                            ViewData["cNoiDung"] = Server.HtmlEncode(don.CNOIDUNG);
                            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                            ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, (int)don.INGUONDON);
                            int iKyHop = ID_KyHop_HienTai();
                            ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                            ViewData["opt-quoctich"] = kn.Option_QuocTich((int)don.INGUOIGUI_QUOCTICH);
                            ViewData["opt-dantoc"] = kn.Option_DanToc((int)don.INGUOIGUI_DANTOC);
                            _condition = new Dictionary<string, object>();
                            _condition.Add("IHIENTHI", 1);
                            _condition.Add("IDELETE", 0);
                            List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                            ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, AppConfig.IDIAPHUONG);
                            if (don.IDIAPHUONG_1 != null)
                            {
                                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, (int)don.IDIAPHUONG_1);

                            }
                            else
                            {
                                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                            }
                            ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)don.ILOAIDON);
                            ViewData["opt-noidung"] = kn.Option_NoiDungDon_ThuocLinhVuc((int)don.INOIDUNG, (int)don.ILINHVUC);
                            ViewData["opt-tinhchat"] = kn.Option_TinhChatDon_ThuocNguonDon((int)don.ITINHCHAT, (int)don.INOIDUNG);
                            ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon((int)don.ILINHVUC, (int)don.ILOAIDON);
                            ViewData["opt-chuyenvien-xuly"] = "";
                            ViewData["chuyenvien"] = "style='display:none;margin-top: 1%;'";
                            if (_base.ActionMulty_("11,44", act))
                            {
                                ViewData["chuyenvien"] = "style='display:inline-block;margin-top: 1%;'";
                                _condition = new Dictionary<string, object>();
                                _condition.Add("IDONVI", 4);
                                _condition.Add("ISTATUS", 1);
                                List<USERS> lstUser = kntc.List_User(_condition);
                                _condition = new Dictionary<string, object>();
                                _condition.Add("IACTION", 12);
                                List<USER_ACTION> lstUserAction = kntc.List_UserAction(_condition);
                                ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, iUser);
                                // ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(4, (int)don.IUSER_DUOCGIAOXULY);
                            }
                            ViewData["opt-domat"] = kn.Option_Domat((decimal)DoMat.Thuong);
                            ViewData["opt-dokhan"] = kn.Option_Dokhan((decimal)DoKhan.Thuong);
                            if (don.IDOMAT != null)
                            {
                                ViewData["opt-domat"] = kn.Option_Domat(don.IDOMAT);
                            }
                            if (don.IDOKHAN != null)
                            {
                                ViewData["opt-dokhan"] = kn.Option_Dokhan(don.IDOKHAN);
                            }
                            ViewData["dong"] = "style='display:inline-block;'";
                            ViewData["mo"] = "style='display:none'";
                            if (don.ILOAIDON != 0 || don.ILINHVUC != 0 || don.INOIDUNG != 0 || don.ITINHCHAT != 0)
                            {
                                ViewData["mo"] = "style='display:inline-block'";
                                ViewData["dong"] = "style='display:none'";
                                ViewData["morong"] = "style='display:inline-block;width: 100%;'";
                            }
                            else
                            {
                                ViewData["morong"] = "style='display:none;width: 100%;'";
                            }
                        }
                    }
                    else
                    {
                        int iDon = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                        // ViewData["nutkiemtrung"] = "style='display:none'";
                        ViewData["id"] = id;
                        don = kntc.GetDON(iDon);
                        if (don != null)
                        {
                            ViewData["don"] = don;
                            string nguoinhan = ""; string cmnd = ""; string ngaynhan = "";string sdt = "";
                            if (don.IDONTRUNG == 0)
                            {
                                if (don.CNGUOIGUI_TEN != null)
                                {
                                    nguoinhan = Server.HtmlEncode(don.CNGUOIGUI_TEN);
                                }
                                if (don.CNGUOIGUI_CMND != null)
                                {
                                    cmnd = Server.HtmlEncode(don.CNGUOIGUI_CMND);
                                }
                                if (don.DNGAYNHAN != null)
                                {
                                    ngaynhan = Convert.ToDateTime(don.DNGAYNHAN).ToString("dd/MM/yyyy");
                                }
                                if (don.CNGUOIGUI_SDT != null)
                                {
                                    sdt = Server.HtmlEncode(don.CNGUOIGUI_SDT);
                                }

                                ViewData["cNguoiGui_Ten"] = nguoinhan;
                                ViewData["cNguoiGui_CMND"] = cmnd;
                                ViewData["cNguoiGui_SDT"] = sdt;
                                ViewData["dNgayNhan"] = ngaynhan;
                                ViewData["cNguoiGui_DiaChi"] = don.CNGUOIGUI_DIACHI;
                                ViewData["cNoiDung"] = don.CNOIDUNG;
                                List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                                ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, (int)don.INGUONDON);
                                int iKyHop = ID_KyHop_HienTai();
                                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                                ViewData["opt-quoctich"] = kn.Option_QuocTich((int)don.INGUOIGUI_QUOCTICH);
                                ViewData["opt-dantoc"] = kn.Option_DanToc((int)don.INGUOIGUI_DANTOC);
                                _condition = new Dictionary<string, object>();
                                _condition.Add("IHIENTHI", 1);
                                _condition.Add("IDELETE", 0);
                                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, AppConfig.IDIAPHUONG);
                                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                                if (don.IDIAPHUONG_1 != null)
                                {
                                    ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, (int)don.IDIAPHUONG_0, (int)don.IDIAPHUONG_1);

                                }
                                else
                                {
                                    ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, (int)don.IDIAPHUONG_0, 0);
                                }
                                ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)don.ILOAIDON);
                                ViewData["opt-noidung"] = kn.Option_NoiDungDon_ThuocLinhVuc((int)don.INOIDUNG, (int)don.ILINHVUC);
                                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon_ThuocNguonDon((int)don.ITINHCHAT, (int)don.INOIDUNG);
                                ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon((int)don.ILINHVUC, (int)don.ILOAIDON);

                                ViewData["opt-chuyenvien-xuly"] = "";
                                ViewData["chuyenvien"] = "style='display:none;margin-top: 1%;'";
                                if (_base.ActionMulty_("11,44", act))
                                {
                                    ViewData["chuyenvien"] = "style='display:inline-block;margin-top: 1%;'";
                                    _condition = new Dictionary<string, object>();
                                    _condition.Add("IDONVI", (decimal)u.user_login.IDONVI);
                                    _condition.Add("ISTATUS", 1);
                                    List<USERS> lstUser = kntc.List_User(_condition);
                                    _condition = new Dictionary<string, object>();
                                    _condition.Add("IACTION", 12);
                                    List<USER_ACTION> lstUserAction = kntc.List_UserAction(_condition);
                                    ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, iUser);
                                    // ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC((decimal)u.user_login.IDONVI, iUser);
                                }
                                ViewData["opt-domat"] = kn.Option_Domat((int)don.IDOMAT);
                                ViewData["opt-dokhan"] = kn.Option_Dokhan((int)don.IDOKHAN);
                                ViewData["dong"] = "style='display:inline-block;'";
                                ViewData["mo"] = "style='display:none'";
                                if (don.ILOAIDON != 0 || don.ILINHVUC != 0 || don.INOIDUNG != 0 || don.ITINHCHAT != 0)
                                {
                                    ViewData["mo"] = "style='display:inline-block'";
                                    ViewData["dong"] = "style='display:none'";
                                    ViewData["morong"] = "style='display:inline-block;width: 100%;'";
                                }
                                else
                                {
                                    ViewData["morong"] = "style='display:none;width: 100%;'";
                                }
                            }
                            else
                            {
                                don = kntc.GetDON((int)don.IDONTRUNG);
                                if (don.CNGUOIGUI_TEN != null)
                                {
                                    nguoinhan = Server.HtmlEncode(don.CNGUOIGUI_TEN);
                                }
                                if (don.CNGUOIGUI_CMND != null)
                                {
                                    cmnd = Server.HtmlEncode(don.CNGUOIGUI_CMND);
                                }
                                if (don.CNGUOIGUI_SDT != null)
                                {
                                    sdt = Server.HtmlEncode(don.CNGUOIGUI_SDT);
                                }

                                ViewData["cNguoiGui_Ten"] = nguoinhan;
                                ViewData["cNguoiGui_CMND"] = cmnd;
                                ViewData["cNguoiGui_SDT"] = sdt;
                                ViewData["dNgayNhan"] = DateTime.Now.ToString("dd/MM/yyyy");
                                ViewData["cNguoiGui_DiaChi"] = don.CNGUOIGUI_DIACHI;
                                ViewData["cNoiDung"] = don.CNOIDUNG;
                                ViewData["cGhiChu"] = don.CGHICHU;
                                List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                                ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, (int)don.INGUONDON);
                                int iKyHop = ID_KyHop_HienTai();
                                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                                ViewData["opt-quoctich"] = kn.Option_QuocTich((int)don.INGUOIGUI_QUOCTICH);
                                ViewData["opt-dantoc"] = kn.Option_DanToc((int)don.INGUOIGUI_DANTOC);
                                _condition = new Dictionary<string, object>();
                                _condition.Add("IHIENTHI", 1);
                                _condition.Add("IDELETE", 0);
                                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, AppConfig.IDIAPHUONG);
                                if (don.IDIAPHUONG_1 != null)
                                {
                                    ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, (int)don.IDIAPHUONG_1);

                                }
                                else
                                {
                                    ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                                }
                                ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)don.ILOAIDON);
                                ViewData["opt-noidung"] = kn.Option_NoiDungDon_ThuocLinhVuc((int)don.INOIDUNG, (int)don.ILINHVUC);
                                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon_ThuocNguonDon((int)don.ITINHCHAT, (int)don.INOIDUNG);
                                ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon((int)don.ILINHVUC, (int)don.ILOAIDON);

                                ViewData["opt-chuyenvien-xuly"] = "";
                                ViewData["chuyenvien"] = "style='display:none;margin-top: 1%;'";
                                ViewData["chuyenvien"] = "style='display:none;margin-top: 1%;'";
                                if (_base.ActionMulty_("11,44", act))
                                {
                                    ViewData["chuyenvien"] = "style='display:inline-block;margin-top: 1%;'";
                                    _condition = new Dictionary<string, object>();
                                    _condition.Add("IDONVI", (decimal)u.user_login.IDONVI);
                                    _condition.Add("ISTATUS", 1);
                                    List<USERS> lstUser = kntc.List_User(_condition);
                                    _condition = new Dictionary<string, object>();
                                    _condition.Add("IACTION", 12);
                                    List<USER_ACTION> lstUserAction = kntc.List_UserAction(_condition);
                                    ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, iUser);
                                    //ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC((decimal)u.user_login.IDONVI, iUser);
                                }
                                ViewData["opt-domat"] = kn.Option_Domat((int)don.IDOMAT);
                                ViewData["opt-dokhan"] = kn.Option_Dokhan((int)don.IDOKHAN);
                                ViewData["dong"] = "style='display:inline-block;'";
                                ViewData["mo"] = "style='display:none'";
                                if (don.ILOAIDON != 0 || don.ILINHVUC != 0 || don.INOIDUNG != 0 || don.ITINHCHAT != 0)
                                {
                                    ViewData["mo"] = "style='display:inline-block'";
                                    ViewData["dong"] = "style='display:none'";
                                    ViewData["morong"] = "style='display:inline-block;width: 100%;'";
                                }
                                else
                                {
                                    ViewData["morong"] = "style='display:none;width: 100%;'";
                                }
                            }

                        }
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Tiếp nhận đơn thư");
                return View("../Home/Error_Exception");
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Tiepnhan(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string _Id = fc["id"];
                KNTC_DON don = new KNTC_DON();
                if (_Id != "")
                {
                    int ID = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                    don = kntc.GetDON(ID);
                }
                UserInfor ui = GetUserInfor();
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/?type=type"); return null; }
                    }

                }
                int iUser = ui.tk_action.iUser;
                if (!CheckTokenAction("kntc_themmoidon"))
                {
                    Response.Redirect("/Home/Error");
                    return null;
                }
                int iDon = 0;
                don.IPLSONGUOI = Convert.ToInt32(fc["id_Soluongnguoi"]);
                don.IDOITUONGGUI = fc["iDoiTuongGui"].ToInt32OrDefault();
                if (ui.user_login.ITYPE == (decimal)UserType.ChuyenVienHDND)
                {
                    don.IDOITUONGGUI = (decimal)Loai_DoiTuongKNTC.HDND;
                }
                if (ui.user_login.ITYPE == (decimal)UserType.ChuyenVienDBQH)
                {
                    don.IDOITUONGGUI = (decimal)Loai_DoiTuongKNTC.DBQH;
                }
                don.IDONVITIEPNHAN = ui.user_login.IDONVI;
                don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                don.INGUONDON = Convert.ToInt32(fc["iNguonDon"]);
                don.CNGUOIGUI_TEN = func.RemoveTagInput(fc["cNguoiGui_Ten"]);
                don.CNGUOIGUI_SDT = func.RemoveTagInput(fc["cNguoiGui_SDT"]);
                don.IKHOA = Convert.ToInt32(fc["iKyHop"]);
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
                don.CNGUOIGUI_CMND = func.RemoveTagInput(fc["cNguoiGui_CMND"]);
                don.IDIAPHUONG_0 = Convert.ToInt32(fc["iDiaPhuong_0"]);
                if (Convert.ToInt32(fc["iDiaPhuong_0"]) == -1)
                {
                    don.IDIAPHUONG_1 = 0;
                }
                else
                {
                    don.IDIAPHUONG_1 = Convert.ToInt32(fc["iDiaPhuong_1"]);
                }
                if (Convert.ToInt32(fc["iDiaPhuong_1"]) == -1)
                {
                    don.IDIAPHUONG_2 = 0;
                }
                else
                {
                    don.IDIAPHUONG_2 = Convert.ToInt32(fc["iDiaPhuong_2"]);
                }
                don.CNGUOIGUI_DIACHI = func.RemoveTagInput(fc["cNguoiGui_DiaChi"]);
                don.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                don.CGHICHU = func.RemoveTagInput(fc["cGhiChu"]);
                don.IUSER = iUser;
                don.DDATE = DateTime.Now;
                if (don.IDONTRUNG == null)
                {
                    don.IDONTRUNG = 0;
                }
                don.IDANHGIA = 0;
                don.IDONVITHULY = ui.user_login.IDONVI;
                don.ILOAIDON = Convert.ToInt32(fc["iLoaiDon"]);
                don.INOIDUNG = Convert.ToInt32(fc["iNoiDung"]);
                don.ITHAMQUYEN = Convert.ToInt32(fc["iThuocThamQuyen"]);
                don.ITHULY = 0;
                don.ITINHCHAT = Convert.ToInt32(fc["iTinhChat"]);
                don.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                don.IDUDIEUKIEN = Convert.ToInt32(fc["iDuDieuKien"]);
                don.IDUDIEUKIEN_KETQUA = Convert.ToInt32(fc["iHinhThuc"]);
                don.ITINHTRANG_DONVIXULY = 0;
                don.CHITIETLYDO_LUUTHEODOI = func.RemoveTagInput(fc["iMoTa"]);
                don.ITINHTRANG_NOIBO = 0;
                //Trường hợp điều kiện xử lý là khác chưa xác định, đơn thuộc tình trạng đã phân loại đơn
                if(Convert.ToInt32(fc["iDuDieuKien"]) == -1)
                {
                    don.ITINHTRANGXULY = Convert.ToDecimal(TrangThaiDon.DaChuyenXuLy);
                }
                else
                {                  
                    don.ITINHTRANGXULY = Convert.ToDecimal(TrangThaiDon.DaPhanLoai);
                }    
                
                don.IUSER_GIAOXULY = 0;
                don.IUSER_DUOCGIAOXULY = 0;
                don.IDELETE = 0;
                int iGiaoXuLy = Convert.ToInt32(fc["iGiaoXuLy"]);
                if (iGiaoXuLy != 0)
                {
                    //đã chuyển xử lý
                    don.IUSER_GIAOXULY = iUser;
                    don.IUSER_DUOCGIAOXULY = iGiaoXuLy;
                    // don.ITINHTRANGXULY = 1;
                }
                else
                {
                    // 190722 - chưa có luồng chuyên viên xử lý -> set default là user hiện tại
                    don.IUSER_GIAOXULY = iUser;
                    don.IUSER_DUOCGIAOXULY = iUser;
                }
                don.ILUUTHEODOI = 0;
                don.INGUOIGUI_DANTOC = Convert.ToInt32(fc["iNguoiGui_DanToc"]);
                don.INGUOIGUI_QUOCTICH = Convert.ToInt32(fc["iNguoiGui_QuocTich"]);
                don.IDOMAT = Convert.ToInt32(fc["iDoMat"]);
                don.IDOKHAN = Convert.ToInt32(fc["iDoKhan"]);
                if (_Id != "")
                {
                    kntc.Update_Don(don);
                    iDon = (int)don.IDON;
                    kntc.Tracking_KNTC(iUser, iDon, "Cập nhật đơn");
                }
                else
                {
                    kntc.TiepNhan_Don(don);
                    iDon = (int)don.IDON;
                    kntc.Tracking_KNTC(iUser, iDon, "Thêm mới đơn");
                }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = UploadFile(file);
                        if (fileName != "")
                        {
                            FILE_UPLOAD f = new FILE_UPLOAD();
                            f.CTYPE = "kntc_don";
                            f.CFILE = fileName;
                            f.ID = don.IDON;
                            kntc.Upload_file(f);
                        }

                    }
                }

                string url_key = tl.Set_Url_keycookie();
                string id_encr = HashUtil.Encode_ID(iDon.ToString(), url_key);
                Session[KNTCDonSession] = null;
                Response.Redirect("/Kntc/Kiemtrung/?id=" + id_encr + "#success");
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Thêm mới đơn thư");
                return null;
            }
        }
        public ActionResult Kiemtrung(string id)
        {

            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                TaikhoanAtion act = GetUserInfor().tk_action;

                if (id != null)
                {
                    int iDon = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                    if (!_base.ActionMulty_Redirect_("44,10", act)) { Response.Redirect("/Home/Error/"); return null; }
                    func.SetCookies("url_return", Request.Url.AbsoluteUri);
                    KNTC_DON don = kntc.GetDON(iDon);
                    if (don != null)
                    {
                        if (don.IDONTRUNG != 0)
                        {
                            ViewData["style"] = "style='display:none'";
                            ViewData["dontrung"] = "style='display:none'";
                        }
                    }

                    ViewData["don"] = don;
                    ViewData["don_detail"] = kn.KNTC_Detail(iDon, id);
                    string url_cookie = Request.Cookies["url_key"].Value;
                    ViewData["list_dontrung"] = kn.List_DonTrung(kntc.Get_List_DonTrung(don), don, url_cookie);
                    ViewData["dontrung"] = "style='display:none'";
                    ViewData["style"] = "style='display:inline-block'";
                    if (kntc.Get_List_DonTrung(don).Count() > 0)
                    {
                        if (don.IDONTRUNG != 0)
                        {
                            ViewData["dontrung"] = "style='display:inline-block'";
                        }
                    }
                    ViewData["chuyenvien"] = "style='display:inline-block'";
                    if (don.IUSER_DUOCGIAOXULY != 0)
                    {
                        ViewData["chuyenvien"] = "style='display:inline-block'";
                    }
                    else
                    {
                        ViewData["chuyenvien"] = "style='display:inline-block'";
                        ViewData["style"] = "style='display:none'";
                    }
                    if (don.CNOIDUNG == null || don.CNOIDUNG == "")
                    {
                        ViewData["style"] = "style='display:none'";
                        ViewData["chuyenvien"] = "style='display:inline-block'";
                    }
                    if (!_base.ActionMulty_("12,44", act))
                    {
                        ViewData["style"] = "style='display:none'";
                    }
                    ViewData["file"] = kn.File_View(iDon, "kntc_don");
                    ViewData["id"] = id;
                }
                else
                {
                    KNTC_DON don = (KNTC_DON)Session[KNTCDonSession];
                    if (don != null)
                    {
                        if (don.IDONTRUNG != 0)
                        {
                            ViewData["style"] = "style='display:none'";
                            ViewData["dontrung"] = "style='display:none'";
                        }
                    }

                    ViewData["don"] = don;
                    ViewData["don_detail"] = kn.KNTC_Detail((int)don.IDON, "");
                    string url_cookie = Request.Cookies["url_key"].Value;
                    ViewData["list_dontrung"] = kn.List_DonTrung(kntc.Get_List_DonTrung(don), don, url_cookie);
                    ViewData["dontrung"] = "style='display:none'";
                    ViewData["style"] = "style='display:inline-block'";
                    if (kntc.Get_List_DonTrung(don).Count() > 0)
                    {
                        if (don.IDONTRUNG != 0)
                        {
                            ViewData["dontrung"] = "style='display:inline-block'";
                        }
                    }
                    ViewData["chuyenvien"] = "style='display:inline-block'";
                    if (don.IUSER_DUOCGIAOXULY != 0)
                    {
                        ViewData["chuyenvien"] = "style='display:inline-block'";
                    }
                    else
                    {
                        ViewData["chuyenvien"] = "style='display:inline-block'";
                        ViewData["style"] = "style='display:none'";
                    }
                    if (don.CNOIDUNG == null || don.CNOIDUNG == "")
                    {
                        ViewData["style"] = "style='display:none'";
                        ViewData["chuyenvien"] = "style='display:inline-block'";
                    }
                    if (!_base.ActionMulty_("12,44", act))
                    {
                        ViewData["style"] = "style='display:none'";
                    }
                    ViewData["id"] = id;
                    ViewData["idoncheck"] = "";
                }
                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Kiểm trùng đơn");
                return null;
            }
        }
        public ActionResult KiemTrungNhanh()
        {

            UserInfor ui = GetUserInfor();
            if (!_base.ActionMulty_Redirect_("44,10", ui.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            KNTC_DON don = new KNTC_DON();


            string id = Request["id"];
            don.CNGUOIGUI_TEN = Request["cNguoiGui_Ten"];
            don.IDIAPHUONG_0 = Convert.ToInt32(Request["iDiaPhuong_0"]);
            don.IDONVITIEPNHAN = ui.user_login.IDONVI;
            int count = kntc.Get_List_DonTrung(don).ToList().Count();
            DateTime ngaynhan = DateTime.Now;
            string a = Request["dNgayNhan"];
            if (a != "")
            {
                ngaynhan = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(Request["dNgayNhan"])));
            }
            don.DNGAYNHAN = ngaynhan;
            don.INGUONDON = Convert.ToInt32(Request["iNguonDon"]);
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
            don.CNGUOIGUI_CMND = func.RemoveTagInput(Request["cNguoiGui_CMND"]);

            if (Convert.ToInt32(Request["iDiaPhuong_0"]) == -1)
            {
                don.IDIAPHUONG_1 = 0;
            }
            else
            {
                don.IDIAPHUONG_1 = Convert.ToInt32(Request["iDiaPhuong_1"]);
            }
            don.IDIAPHUONG_2 = 0;
            don.CNGUOIGUI_DIACHI = func.RemoveTagInput(Request["cNguoiGui_DiaChi"]);
            don.CNOIDUNG = func.RemoveTagInput(Request["cNoiDung"]);
            don.DDATE = DateTime.Now;
            don.IDONTRUNG = 0;

            don.IDONVITHULY = ui.user_login.IDONVI;
            don.ILOAIDON = Convert.ToInt32(Request["iLoaiDon"]);
            don.INOIDUNG = Convert.ToInt32(Request["iNoiDung"]);
            don.ITHAMQUYEN = -1;
            don.ITHULY = 0;
            don.ITINHCHAT = Convert.ToInt32(Request["iTinhChat"]);
            don.ILINHVUC = Convert.ToInt32(Request["iLinhVuc"]);
            don.IDUDIEUKIEN = -1;
            don.IDELETE = 0;
            don.IDUDIEUKIEN_KETQUA = 0;
            don.ITINHTRANG_DONVIXULY = 0;
            don.ITINHTRANG_NOIBO = 0;
            don.ITINHTRANGXULY = 0;
            don.IUSER_GIAOXULY = 0;
            don.IUSER_DUOCGIAOXULY = 0;
            don.ILUUTHEODOI = 0;
            don.INGUOIGUI_DANTOC = Convert.ToInt32(Request["iNguoiGui_DanToc"]);
            don.INGUOIGUI_QUOCTICH = Convert.ToInt32(Request["iNguoiGui_QuocTich"]);
            don.IDOMAT = Convert.ToInt32(Request["iDoMat"]);
            don.IDOKHAN = Convert.ToInt32(Request["iDoKhan"]);
            Session[KNTCDonSession] = don;
            Response.Write(count);
            return null;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiemtrung(FormCollection fc)
        {

            if (!CheckAuthToken()) { return null; }
            {
                TaikhoanAtion act = GetUserInfor().tk_action;
                if (!_base.ActionMulty_Redirect_("44,10", act)) { Response.Redirect("/Home/Error/"); return null; }
                if (fc["id"] != "")
                {
                    int idon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                    KNTC_DON don = kntc.GetDON(idon);
                    if (don != null)
                    {
                        if (don.ITINHTRANGXULY == 0 || don.ITINHTRANGXULY == 1)
                        {
                            Response.Redirect("/Kntc/Tiepnhan/?id=" + fc["id"]);
                        }
                        else
                        {
                            if (don.ITHAMQUYEN == 1)
                            {
                                if (don.IDUDIEUKIEN == 1)
                                    Response.Redirect("/Kntc/Dudieukien/" + "#success");
                                else
                                    if (don.IDUDIEUKIEN == -1)
                                        Response.Redirect("/Kntc/Thuocthamquyen/" + "#success");
                                    else
                                        Response.Redirect("/Kntc/Khongdudieukien/" + "#success");
                            }
                            else
                            {
                                Response.Redirect("/Kntc/Khongthuocthamquyen/" + "#success");
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("/Kntc/Tiepnhan");
                }

            }
            return null;
        }

        public ActionResult Themmoidon(FormCollection fc)
        {

            if (!CheckAuthToken()) { return null; }
            {
                TaikhoanAtion act = GetUserInfor().tk_action;
                if (!_base.ActionMulty_Redirect_("44,10", act)) { Response.Redirect("/Home/Error/"); return null; }
                Session[KNTCDonSession] = null;
                Response.Redirect("/Kntc/Tiepnhan");
            }
            return null;
        }
        public ActionResult Sua()
        {

            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("44,10", act)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encrypt = Request["id"];
                string id_decrypt = HashUtil.Decode_ID(id_encrypt, Request.Cookies["url_key"].Value);
                int id = Convert.ToInt32(id_decrypt);
                ViewData["file"] = kn.File_Edit(id, "kntc_don", Request.Cookies["url_key"].Value);
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Sua");
                SetTokenAction("kntc_suadon", id);
                KNTC_DON don = kntc.GetDON(id);
                if (don != null)
                {   
                    if(don.IKHOA != null)
                        ViewData["opt-khoa"] = Get_Option_KhoaHopByDVTN((int)don.IKHOA, Decimal.ToInt32(don.IDOITUONGGUI));
                    else
                        ViewData["opt-khoa"] = Get_Option_KhoaHopByDVTN(Decimal.ToInt32(don.IDOITUONGGUI));
                    ViewData["id_encrypt"] = id_encrypt;
                    ViewData["don"] = don;
                    ViewData["user"] = u;
                    List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon().Where(n => n.ILOAI == don.IDOITUONGGUI).ToList();
                    ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, Decimal.ToInt32(don.INGUONDON ?? 0));
                    ViewData["opt-quoctich"] = kn.Option_QuocTich(Decimal.ToInt32(don.INGUOIGUI_QUOCTICH ?? 0));
                    ViewData["opt-dantoc"] = kn.Option_DanToc(Decimal.ToInt32(don.INGUOIGUI_DANTOC ?? 0));
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IHIENTHI", 1);
                    _condition.Add("IDELETE", 0);
                    List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                    ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, Decimal.ToInt32(don.IDIAPHUONG_0 ?? 0));
                    ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, Decimal.ToInt32(don.IDIAPHUONG_0 ?? 0), Decimal.ToInt32(don.IDIAPHUONG_1 ?? 0));
                    ViewData["opt-xa"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, Decimal.ToInt32(don.IDIAPHUONG_1 ?? 0), Decimal.ToInt32(don.IDIAPHUONG_2 ?? 0));
                    ViewData["opt-loaidon"] = kn.Option_LoaiDon(Decimal.ToInt32(don.ILOAIDON ?? 0));
                    ViewData["opt-noidung"] = kn.Option_NoiDungDon_ThuocLinhVuc(Decimal.ToInt32(don.INOIDUNG ?? 0), Decimal.ToInt32(don.ILINHVUC ?? 0));
                    ViewData["opt-tinhchat"] = kn.Option_TinhChatDon_ThuocNguonDon(Decimal.ToInt32(don.ITINHCHAT ?? 0), Decimal.ToInt32(don.INOIDUNG ?? 0));
                    ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon(Decimal.ToInt32(don.ILINHVUC ?? 0), Decimal.ToInt32(don.ILOAIDON ?? 0));
                    ViewData["opt-chuyenvien-xuly"] = "";

                    ViewData["opt-dieukienxuly"] = kn.Option_DieuKienXuLy(don.IDUDIEUKIEN);
                    ViewData["opt-hinhthucxuly"] = kn.Option_HinhThucXuLy(don.IDUDIEUKIEN_KETQUA);
                    ViewData["chuyenvien"] = "style='display:inline-block;margin-top: 1%;'";
                    if (_base.ActionMulty_("11,44", act))
                    {
                        ViewData["chuyenvien"] = "style='display:inline-block;margin-top: 1%;'";
                        _condition = new Dictionary<string, object>();
                        _condition.Add("IDONVI", (decimal)u.user_login.IDONVI);
                        _condition.Add("ISTATUS", 1);
                        List<USERS> lstUser = kntc.List_User(_condition);
                        _condition = new Dictionary<string, object>();
                        _condition.Add("IACTION", 12);
                        List<USER_ACTION> lstUserAction = kntc.List_UserAction(_condition);
                        ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, u.user_login.IUSER);
                        //ViewData["opt-chuyenvien-xuly"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC((decimal)u.user_login.IDONVI, u.user_login.IUSER);
                    }
                    ViewData["opt-domat"] = kn.Option_Domat((decimal)DoMat.Thuong);
                    ViewData["opt-dokhan"] = kn.Option_Dokhan((decimal)DoKhan.Thuong);
                    if (don.IDOMAT != null)
                    {
                        ViewData["opt-domat"] = kn.Option_Domat(don.IDOMAT);
                    }
                    if (don.IDOKHAN != null)
                    {
                        ViewData["opt-dokhan"] = kn.Option_Dokhan(don.IDOKHAN);
                    }


                }
                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Sửa đơn ");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Don_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u = GetUserInfor();
                int UserID = Convert.ToInt32(u.tk_action.iUser);
                string id_decrypt = HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value);
                int id = Convert.ToInt32(id_decrypt);
                string diachi = "";
                string tennguoinop = "";
                KNTC_DON d = kntc.GetDON(id);
                if (d != null)
                {
                    tennguoinop = d.CNGUOIGUI_TEN;
                    if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += d.CNGUOIGUI_DIACHI + " ,";
                    if (d.IDIAPHUONG_2 != null)
                    {
                        DIAPHUONG dp = kntc.Get_DiaPhuong(Convert.ToInt32(d.IDIAPHUONG_2));
                        if (dp != null)
                        {
                            diachi += dp.CTEN + " ,";

                        }
                    }
                    if (d.IDIAPHUONG_1 != null)
                    {
                        DIAPHUONG dp = kntc.Get_DiaPhuong(Convert.ToInt32(d.IDIAPHUONG_1));
                        if (dp != null)
                        {
                            diachi += dp.CTEN + " ";

                        }
                    }
                }
                Dictionary<string, object> condition = new Dictionary<string, object>();
                condition.Add("IDONTRUNG", id);
                var dontrung = kntc.ListAll_Don(condition);
                if (dontrung.Count() > 0)
                {
                    foreach (var dt in dontrung)
                    {
                        KNTC_DON don_trung = kntc.GetDON((int)dt.IDON);
                        don_trung.IDONTRUNG = 0;
                        kntc.Update_Don(don_trung);
                    }
                }
                kntc.Tracking_KNTC(UserID, id, "Xóa đơn thư khiếu nại, tố cáo của ông/ bà: " + tennguoinop + " ( " + diachi + " ).");
                d.IDELETE = 1;
                kntc.Delete_Don(id);


                Response.Write(1);

                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Xóa đơn ");
                return null;
            }
        }
        public ActionResult Ajax_Don_del_import(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u = GetUserInfor();
                int UserID = Convert.ToInt32(u.tk_action.iUser);
                string id_decrypt = HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value);
                int id = Convert.ToInt32(id_decrypt);
                string diachi = "";
                string tennguoinop = "";
                KNTC_DON d = kntc.GetDON(id);
                if (d.IIDIMPORT != null && d.IIDIMPORT != 0)
                {
                    KNTC_DON_IMPORT ip = kntc.GetKntcDonImport((int)d.IIDIMPORT);
                    ip.ISODON--;
                    kntc.UpdateKntcDonImport(ip);
                }
                if (d != null)
                {
                    tennguoinop = d.CNGUOIGUI_TEN;
                    if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += d.CNGUOIGUI_DIACHI + " ,";
                    if (d.IDIAPHUONG_2 != null)
                    {
                        DIAPHUONG dp = kntc.Get_DiaPhuong(Convert.ToInt32(d.IDIAPHUONG_2));
                        if (dp != null)
                        {
                            diachi += dp.CTEN + " ,";

                        }
                    }
                    if (d.IDIAPHUONG_1 != null)
                    {
                        DIAPHUONG dp = kntc.Get_DiaPhuong(Convert.ToInt32(d.IDIAPHUONG_1));
                        if (dp != null)
                        {
                            diachi += dp.CTEN + " ";

                        }
                    }
                }
                Dictionary<string, object> condition = new Dictionary<string, object>();
                condition.Add("IDONTRUNG", id);
                var dontrung = kntc.ListAll_Don(condition);
                if (dontrung.Count() > 0)
                {
                    foreach (var dt in dontrung)
                    {
                        KNTC_DON don_trung = kntc.GetDON((int)dt.IDON);
                        don_trung.IDONTRUNG = 0;
                        kntc.Update_Don(don_trung);
                    }
                }
                kntc.Tracking_KNTC(UserID, id, "Xóa đơn thư khiếu nại, tố cáo của ông/ bà: " + tennguoinop + " ( " + diachi + " ).");
                d.IDELETE = 1;
                kntc.Delete_Don(id);


                Response.Write(1);

                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Xóa đơn ");
                return null;
            }
        }
        //public ActionResult Ajax_Don_trung_update(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        int iUser = id_user();
        //        int id_check = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
        //        int id_trung = Convert.ToInt32(HashUtil.Decode_ID(fc["id_trung"], Request.Cookies["url_key"].Value));
        //        KNTC_DON don = kntc.GetDON(id_check);
        //        //KNTC_DON dontrung = kntc.GetDON(id_trung);
        //        if (don != null)
        //        {
        //            if (don.IDONTRUNG == id_trung)
        //            {
        //                don.IDONTRUNG = 0;

        //                kntc.Tracking_KNTC(iUser, id_check, "Bỏ chọn đơn trùng");
        //                kntc.Update_Don(don);
        //                //kkntc.Update_Don(dontrung);
        //                Response.Write(0);
        //            }
        //            else
        //            {
        //                don.IDONTRUNG = id_trung;
        //                //don.ILOAIDON = dontrung.ILOAIDON;
        //                //don.ILINHVUC = dontrung.ILINHVUC;
        //                //don.INOIDUNG = dontrung.INOIDUNG;
        //                //don.ITINHCHAT = dontrung.ITINHCHAT;
        //                //don.IDIAPHUONG_0 = dontrung.IDIAPHUONG_0;
        //                //don.IDIAPHUONG_1 = dontrung.IDIAPHUONG_1;
        //                //don.IDIAPHUONG_2 = dontrung.IDIAPHUONG_2;
        //                //don.IDOANDONGNGUOI = dontrung.IDOANDONGNGUOI;
        //                //don.IDOKHAN = dontrung.IDOKHAN;
        //                //don.IDOMAT = dontrung.IDOMAT;
        //                //don.CNOIDUNG =  dontrung.CNOIDUNG;
        //                kntc.Tracking_KNTC(iUser, id_check, "Chọn đơn trùng");
        //                kntc.Update_Don(don);
        //                //kntc.Upkdate_Don(dontrung);
        //                Response.Write(1);
        //            }
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Handle Exception;
        //        log.Log_Error(ex, "Cập nhật đơn khiếu nại tố cáo trùng");
        //        return null;
        //    }
        //}

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                string id_decrypt = HashUtil.Decode_ID(fc["iDon"], Request.Cookies["url_key"].Value);
                int id = Convert.ToInt32(id_decrypt);
                if (!CheckTokenAction("kntc_suadon", id))
                {
                    Response.Redirect("/Home/Error");
                    return null;
                }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/?type=type"); return null; }

                    }

                }
                var user = GetUserInfor();
                int iUser = user.tk_action.iUser;
                KNTC_DON don = kntc.GetDON(id);
                if (don != null)
                {
                    don.IDOITUONGGUI = fc["iDoiTuongGui"].ToInt32OrDefault();
                    if (user.user_login.ITYPE == (decimal)UserType.ChuyenVienHDND)
                    {
                        don.IDOITUONGGUI = (decimal)Loai_DoiTuongKNTC.HDND;
                    }
                    if (user.user_login.ITYPE == (decimal)UserType.ChuyenVienDBQH)
                    {
                        don.IDOITUONGGUI = (decimal)Loai_DoiTuongKNTC.DBQH;
                    }
                    don.DNGAYNHAN = Convert.ToDateTime(func.ConvertDateToSql(func.RemoveTagInput(fc["dNgayNhan"])));
                    don.INGUONDON = Convert.ToInt32(fc["iNguonDon"]);
                    don.CNGUOIGUI_TEN = func.RemoveTagInput(fc["cNguoiGui_Ten"]);
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
                    don.CGHICHU = func.RemoveTagInput(fc["cGhiChu"]);
                    don.CNGUOIGUI_CMND = func.RemoveTagInput(fc["cNguoiGui_CMND"]);
                    don.IDIAPHUONG_0 = Convert.ToInt32(fc["iDiaPhuong_0"]);
                    don.IDIAPHUONG_1 = Convert.ToInt32(fc["iDiaPhuong_1"]);
                    don.IDIAPHUONG_2 = Convert.ToInt32(fc["iDiaPhuong_2"]);
                    don.CNGUOIGUI_DIACHI = func.RemoveTagInput(fc["cNguoiGui_DiaChi"]);
                    don.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                    don.ILOAIDON = Convert.ToInt32(fc["iLoaiDon"]);
                    don.INOIDUNG = Convert.ToInt32(fc["iNoiDung"]);
                    don.ITINHCHAT = Convert.ToInt32(fc["iTinhChat"]);
                    don.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                    don.IDUDIEUKIEN = Convert.ToInt32(fc["iDuDieuKien"]);
                    don.IDUDIEUKIEN_KETQUA = Convert.ToInt32(fc["iHinhThuc"]);
                    don.ITHAMQUYEN = Convert.ToInt32(fc["iThuocThamQuyen"]);
                    don.INGUOIGUI_DANTOC = Convert.ToInt32(fc["iNguoiGui_DanToc"]);
                    don.INGUOIGUI_QUOCTICH = Convert.ToInt32(fc["iNguoiGui_QuocTich"]);
                    don.CHITIETLYDO_LUUTHEODOI = func.RemoveTagInput(fc["iMoTa"]);
                    don.IDOMAT = Convert.ToInt32(fc["iDoMat"]);
                    don.IDOKHAN = Convert.ToInt32(fc["iDoKhan"]);
                    don.IPLSONGUOI = Convert.ToInt32(fc["id_Soluongnguoi"]);
                    if (Convert.ToInt32(fc["iKhoa"]) != 0)
                        don.IKHOA = Convert.ToInt32(fc["iKhoa"]);

                    //Bỏ luồng chuyên viên xử lý, set default là user hiện tại
                    //int iGiaoXuLy = 0;
                    //if (Convert.ToInt32(fc["iGiaoXuLy"]) != null)
                    //{
                    //    iGiaoXuLy = Convert.ToInt32(fc["iGiaoXuLy"]);
                    //}
                    //if (iGiaoXuLy != 0)
                    //{
                    //    //đã chuyển xử lý
                    //    don.IUSER_GIAOXULY = iUser;
                    //    don.IUSER_DUOCGIAOXULY = iGiaoXuLy;
                    //    don.ITINHTRANGXULY = 1;
                    //}
                    don.IUSER_GIAOXULY = iUser;
                    don.IUSER_DUOCGIAOXULY = iUser;
                    //Trường hợp điều kiện xử lý là khác chưa xác định, đơn thuộc tình trạng đã phân loại đơn
                    if (Convert.ToInt32(fc["iDuDieuKien"]) == -1)
                    {
                        don.ITINHTRANGXULY = Convert.ToDecimal(TrangThaiDon.DaChuyenXuLy);
                    }
                    else
                    {
                        don.ITINHTRANGXULY = Convert.ToDecimal(TrangThaiDon.DaPhanLoai);
                    }
                    kntc.Update_Don(don);
                    int iDon = (int)don.IDON;
                    for (int i = 1; i < 4; i++)
                    {
                        file = Request.Files["file_upload" + i];
                        if (file != null && file.ContentLength > 0)
                        {
                            FILE_UPLOAD f = new FILE_UPLOAD();
                            f.CTYPE = "kntc_don";
                            f.CFILE = UploadFile(file);
                            f.ID = don.IDON;
                            kntc.Upload_file(f);
                        }

                    }
                    kntc.Tracking_KNTC(iUser, iDon, "Sửa đơn");
                    if (don.ITINHTRANGXULY == (int)TrangThaiDon.DaChuyenXuLy)
                    {
                        Response.Redirect("/Kntc/Choxuly/" + "#success");
                    }
                    else if (don.ITINHTRANGXULY == (int)TrangThaiDon.MoiCapNhat)
                    {
                        Response.Redirect("/Kntc/Moicapnhat/" + "#success");
                    }
                    else if (don.ITINHTRANGXULY == (int)TrangThaiDon.KhongXuLy)
                    {
                        Response.Redirect("/Kntc/Khongxuly/" + "#success");
                    }
                    else if (don.ITINHTRANGXULY == (int)TrangThaiDon.ChoXuLy)
                    {
                        Response.Redirect("/Kntc/Danhanxuly/" + "#success");
                    }
                    else
                    {
                        if (don.ITHAMQUYEN == 1)
                        {
                            if (don.IDUDIEUKIEN == 1)
                                Response.Redirect("/Kntc/Dudieukien/" + "#success");
                            else
                                if (don.IDUDIEUKIEN == -1)
                                    Response.Redirect("/Kntc/Thuocthamquyen/" + "#success");
                                else
                                    Response.Redirect("/Kntc/Khongdudieukien/" + "#success");
                        }
                        else
                        {
                            Response.Redirect("/Kntc/Khongthuocthamquyen/" + "#success");
                        }
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Cập nhật đơn khiếu nại tố cáo");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Delele_file(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                string id_decrypt = HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value);
                int id = Convert.ToInt32(id_decrypt);
                kntc.Delete_File_upload(id);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Xóa file khiếu nại tố cáo");
                return null;
            }
        }
        public ActionResult Don_info(string id)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            UserInfor u_info = GetUserInfor();
            TaikhoanAtion act = u_info.tk_action;
            int idonvi = (int)u_info.user_login.IDONVI;
            if (!_base.ActionMulty_Redirect_(action_kntc, act)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                // new id
                string id_decrypt = HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);
                KNTC_DON d = kntc.GetDON(iDon);
                // end
                ViewData["don"] = d;
                ViewData["file"] = kn.File_View(iDon, "kntc_don");
                ViewData["kn"] = kn.KNTC_Detail(iDon, id_decrypt);
                ViewData["vanban_lienquan"] = kn.Row_Vanban_lienquan(iDon);
                string donvithuly = "Chưa xác định";
                if (d.IDONVITHULY != 0)
                {
                    donvithuly = kntc.GetDonVi((int)d.IDONVITHULY).CTEN;
                }
                if (d.IDONTRUNG != null)
                {
                    List<KNTC_DON> dontrung = kntc.Get_List_DonTrung(d).ToList();

                    //if (d.IDONTRUNG == -1)// Đây là đơn gốc
                    //{
                    //    dontrung = kntc._List_All_Don_Trung((int)d.IDON).Where(v => v.IDON != d.IDON && v.IDONVITIEPNHAN == u_info.user_login.IDONVI).ToList();
                    //}
                    //else
                    //{
                    //    dontrung = kntc._List_All_Don_Trung((int)d.IDONTRUNG).Where(v => v.IDON != d.IDON && v.IDONVITIEPNHAN == u_info.user_login.IDONVI).ToList();
                    //}
                    if (dontrung != null)
                    {
                        ViewData["list_dontrung"] = kn.List_DonTrung(dontrung, d, Request.Cookies["url_key"].Value);
                        ViewData["sodontrung"] = dontrung.Count();
                    }
                }
                ViewData["lichsuluanchuyen"] = kn.List_LichSuXuLy(iDon);
                ViewData["hosodon"] = kn.FileHoSoDon(iDon);
                ViewData["donvi"] = donvithuly;
                //ViewData["giamsat"] = view.KNTC_DonInfo_Giamsat(iDon);
                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Chi tiết đơn khiếu nại tố cáo");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Chuyen_Xuly(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                string iD = fc["id"];
                ViewData["id"] = fc["id"];
                int iDon = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u = GetUserInfor();
                KNTC_DON don = kntc.GetDON(iDon);
                if (don != null)
                {
                    if (Server.HtmlEncode(don.CNOIDUNG) != null)
                    {
                        if (Server.HtmlEncode(don.CNOIDUNG).Trim() == "" || don.INGUONDON == 0)
                        {
                            ViewData["caption"] = "<span class='f-red'>Đơn chưa đủ điều kiện để chuyển xử lý. Vui lòng cập nhật đơn</span>";
                            ViewData["type"] = 0;
                        }
                        else
                        {
                            SetTokenAction("kntc_chuyenxuly");
                            _condition = new Dictionary<string, object>();
                            _condition.Add("IDONVI", (decimal)u.user_login.IDONVI);
                            _condition.Add("ISTATUS", 1);
                            List<USERS> lstUser = kntc.List_User(_condition);
                            _condition = new Dictionary<string, object>();
                            _condition.Add("IACTION", 12);
                            List<USER_ACTION> lstUserAction = kntc.List_UserAction(_condition);
                            ViewData["opt-chuyenvien"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC(lstUser, lstUserAction, 0);
                            // ViewData["opt-chuyenvien"] = kn.Option_ChuyenVien_ChuyenXuLy_KNTC((decimal)u.user_login.IDONVI, 0);
                            ViewData["type"] = 1;

                        }

                    }
                    else
                    {
                        ViewData["caption"] = "<span class='f-red'>Đơn chưa đủ điều kiện để chuyển xử lý. Vui lòng cập nhật đơn</span>";
                        ViewData["type"] = 0;

                    }

                }
                return PartialView("../Ajax/Kntc/Chuyen_Xuly");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Chuyển xử lý đơn khiếu nại tố cáo");
                return null;
            }
        }
        [HttpGet]
        public ActionResult Ajax_Chuyen_Xuly_insert()
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {

                if (!CheckTokenAction("kntc_chuyenxuly"))
                {
                    Response.Redirect("/Home/Error/");
                    return null;
                }
                var fc = this.Request;
                string id = fc["id"];
                string[] id_ = id.Split(new char[] { ',' });
                int iUser_GiaoXuLy = Convert.ToInt32(fc["iUser_GiaoXuLy"]);
                int iUser = id_user();
                foreach (var x in id_)
                {
                    if (x.ToString() != "")
                    {
                        // new id 
                        string id_encrypt = x;
                        string id_decrypt = HashUtil.Decode_ID(id_encrypt, Request.Cookies["url_key"].Value);
                        int iDon = Convert.ToInt32(id_decrypt);
                        KNTC_DON don = kntc.GetDON(iDon);
                        if (don != null)
                        {
                            don.ITINHTRANGXULY = 1;
                            don.IUSER_GIAOXULY = iUser;
                            don.IUSER_DUOCGIAOXULY = iUser_GiaoXuLy;
                            kntc.Update_Don(don);

                            KNTC_DON_LICHSU lichsu = new KNTC_DON_LICHSU();
                            lichsu.IDON = iDon;
                            lichsu.IDONVIGUI = kntc.GetUser(iUser).IDONVI;
                            lichsu.IDONVITIEPNHAN = kntc.GetUser(iUser_GiaoXuLy).IDONVI;
                            lichsu.ITRANGTHAI = (int)don.ITINHTRANGXULY;
                            lichsu.IUSER = iUser;
                            lichsu.ICHUYENXULY = (int)LoaiLichSu.ChuyenXuLy;
                            lichsu.IDONVIXULY = don.IDONVITHULY;
                            lichsu.DNGAYCHUYEN = DateTime.Now;
                            kntc.InsertLichSuDon(lichsu);

                            string user_xuly = kntc.GetUser(iUser_GiaoXuLy).CTEN;
                            kntc.Tracking_KNTC(iUser, iDon, "Giao \"" + user_xuly + "\" xử lý");
                        }

                    }
                }
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Chuyển xử lý đơn khiếu nại tố cáo");
                return null;
            }
        }

        [ValidateInput(false)]
        public ActionResult Choxuly(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Choxuly");
                string url_cookie = func.Get_Url_keycookie();
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("44,10,11,12,13", act)) { Response.Redirect("/Home/Error/"); return null; }
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize;
                if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                DateTime dTuNgay;
                string tungay;
                if (!DateTime.TryParseExact(Request["dTuNgay"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dTuNgay))
                {
                    tungay = "";
                }
                else
                {
                    tungay = dTuNgay.ToString("dd-MMM-yyyy");
                }

                DateTime dDenNgay;
                string denngay;
                if (!DateTime.TryParseExact(Request["dDenNgay"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dDenNgay))
                {
                    denngay = "";
                }
                else
                {
                    denngay = dDenNgay.ToString("dd-MMM-yyyy");
                }
                //tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = Server.HtmlDecode(func.RemoveTagInput(Request["ip_noidung"])); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                var iUser = u.tk_action.iUser;
                if (u.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                condition.Add("P_USER", iUser);
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITIEPNHAN", u.user_login.IDONVI);
                condition.Add("P_ITINHTRANGXULY", (int)TrangThaiDon.DaChuyenXuLy);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);
                if (!_base.ActionMulty_("44", act))
                {
                    ViewData["btn-add"] = " style='display:none' ";
                    condition.Add("P_IUSER_DUOCGIAOXULY", u.user_login.IUSER);
                }
                else
                {
                    condition.Add("P_IUSER_DUOCGIAOXULY", 0);
                }
                KNTC_DON param = get_Request_Paramt_KNTC();
                
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_IQUANHUYEN", param.IDIAPHUONG_1);
                condition.Add("P_IXAPHUONG", param.IDIAPHUONG_2);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                condition.Add("P_IDIEUKIENXULY", param.IDUDIEUKIEN);
                var don = kntc.ListDonChoXuLy(condition);
                string htmlDonList = string.Empty;
                string htmlPhanTrang = string.Empty;
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                string searchFormId = string.Empty;
                string containerId = "ip_data";
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;
                if (isNormalSearch) searchFormId = "form_search";
                if (isAdvancedSearch) searchFormId = "form_";
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    //if (key == null) { caution = ""; }
                    htmlDonList = caution + kn.KNTC_Don_Choxuly(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='5'>" 
                        + base_appcode.PhanTrang_Ajax(total, post_per_page, page, RemovePageFromUrl(false, "Choxuly", "Kntc"), searchFormId, containerId)
                        + Option_Post_Per_Page_Ajax(post_per_page, searchFormId, containerId, false, "Choxuly", "Kntc") + "</td></tr>";
                    htmlPhanTrang = phantrang;
                }
                else
                {
                    htmlDonList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                }

                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlDonList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }
                ViewData["list"] = htmlDonList;
                ViewData["phantrang"] = htmlPhanTrang;
                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);
                ViewData["formchuyentiep"] = "Choxuly";
                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Chờ xử lý đơn khiếu nại tố cáo");
                return null;
            }
        }
        public ActionResult Ajax_Choxuly_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                //ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon(0, 0);
                ViewData["opt-dieukienxuly"] = kn.Option_DieuKienXuLy(-2);
                //ViewData["opt-loaibaocao"] = kn.Option_LoaiBaoCao();
                //ViewData["opt-tenbaocao"] = kn.Option_TenBaoCao(1);
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u_info.user_login.IDONVI, 0);
                if (u_info.user_login.ITYPE == 3)
                {
                    ViewData["opt-khoa"] = Get_Option_Khoa_TheoLoai(loai: 1);
                }
                else if (u_info.user_login.ITYPE == 4)
                {
                    ViewData["opt-khoa"] = Get_Option_Khoa_TheoLoai(loai: 0);
                }
                else
                {
                    ViewData["opt-khoa"] = Get_Option_KhoaHop();
                }
                return PartialView("../Ajax/Kntc/Choxuly_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm nâng cao đơn khiếu nại chờ xử lý");
                return null;
            }
        }

        public ActionResult Ajax_Choxuly_formexport()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                string loaibaocao = "";
                if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
                {
                    loaibaocao = loaibaocao + "<option selected value='0'>Chọn tất cả<option>";
                    loaibaocao = loaibaocao + "<option value='1'>HĐND Tỉnh<option>";
                    loaibaocao = loaibaocao + "<option value='2'>Đoàn đại biểu QH Tỉnh<option>";
                }
                else if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                {
                    loaibaocao = loaibaocao + "<option selected value='1'>HĐND Tỉnh<option>";
                }
                else if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                {
                    loaibaocao = loaibaocao + "<option selected value='2'>Đoàn đại biểu QH Tỉnh<option>";
                }
                ViewData["opt-loaibaocao"] = loaibaocao;
                ViewData["opt-tenbaocao"] = kn.Option_TenBaoCao(1);
                return PartialView("../Ajax/Kntc/Choxuly_formexport");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm nâng cao đơn khiếu nại chờ xử lý");
                return null;
            }
        }

        public ActionResult BaoCaoDonChoXuLiPhanLoai(string ext = "xls", int loaibaocao = 0, int tenbaocao = 0, string dTuNgay = "", string dDenNgay = "")
        {
            string fileName;
            string path;
            DateTime tungay;
            if (!DateTime.TryParseExact(dTuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tungay))
            {
                dTuNgay = "";
            }
            else
            {
                dTuNgay = tungay.ToString("dd-MMM-yyyy");
            }

            DateTime denngay;
            if (!DateTime.TryParseExact(dDenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out denngay))
            {
                dDenNgay = "";
            }
            else
            {
                dDenNgay = denngay.ToString("dd-MMM-yyyy");
            }

            if (loaibaocao == 2)
            {
                fileName = string.Format("{0}.{1}", "QH: Tổng hợp kết quả tiếp nhận đơn", ext);
                path = Server.MapPath(ReportConstants.rpt_KNTC_QH_BaoCao_TongHopKetQuaTiepNhanDon);
            }
            else if (loaibaocao == 1)
            {
                fileName = string.Format("{0}.{1}", "HĐND: Tổng hợp kết quả tiếp nhận đơn", ext);
                path = Server.MapPath(ReportConstants.rpt_KNTC_HDND_BaoCao_TongHopKetQuaTiepNhanDon);
            }
            else
            {
                fileName = string.Format("{0}.{1}", "Tổng hợp kết quả tiếp nhận đơn", ext);
                path = Server.MapPath(ReportConstants.rpt_KNTC_TongHop_BaoCao_TongHopKetQuaTiepNhanDon);
            }
            ExcelFile xls = ExportReportDonChoXuLiPhanLoai(path, loaibaocao, tenbaocao, dTuNgay, dDenNgay);
            return Print(xls, ext, fileName);
        }

        private ExcelFile ExportReportDonChoXuLiPhanLoai(string templatePath, int loaibaocao, int tenbaocao, string tungay, string denngay)
        {
            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }
            List<RPT_KNTC_DONCHOXULIPHANLOAI> donChoXuLiReportList = _kntcReport.GetReportKNTCDonChoXuLiPhanLoai("RPT_KNTC_DONCHOXULIPHANLOAI", loaibaocao, tungay, denngay, iUser);

            XlsFile Result = new XlsFile(true);
            Result.Open(templatePath);
            FlexCelReport fr = new FlexCelReport();

            if (loaibaocao != 1 && loaibaocao != 2)
            {
                var indexes = donChoXuLiReportList.GroupBy(x => x.IDOITUONGGUI).Select(g => g.OrderBy(s => s.STT)).Select(g => g.FirstOrDefault().IDON).ToList();
                foreach (var item in donChoXuLiReportList)
                {
                    if (indexes != null && indexes.Contains(item.IDON))
                    {
                        if (item.IDOITUONGGUI == 0)
                        {
                            item.ROWTITLE = "I. ĐOÀN ĐẠI BIỂU QUỐC HỘI TỈNH";
                        }
                        else if (item.IDOITUONGGUI == 1)
                        {
                            item.ROWTITLE = "II. THƯỜNG TRỰC HỘI ĐỒNG NHÂN DÂN TỈNH";
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            fr.AddTable<RPT_KNTC_DONCHOXULIPHANLOAI>("List", donChoXuLiReportList);
            fr.UseForm(this).Run(Result);
            return Result;
        }

        public ActionResult Ajax_Tamxoa_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                UserInfor u = GetUserInfor();
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Tamxoa_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm nâng cao đơn khiếu nại chờ xử lý");
                return null;
            }
        }
        public ActionResult Chuyenxuly(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Chuyenxuly");
                UserInfor u = GetUserInfor();
                if (!_base.ActionMulty_Redirect_("14,15,44", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                // func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int iDonVi = Convert.ToInt32(u.user_login.IDONVI);

                string key = null;
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                var lstDon = kntc.GetListAllDon(key, tungay, denngay);
                var don = lstDon.Where(v => v.ITINHTRANGXULY == 3 && (v.IDONVITHULY == iDonVi || v.IDONVITIEPNHAN == iDonVi)).ToList();
                int iDonViTiepNhan = 0;
                int iHienThi = 0;
                int iDonViThuLy = 0;
                ViewData["select-donvi"] = "";
                ViewData["select-hienthi"] = kn.Option_HinhThuc(0);
                if (Request["donvi"] != null)
                {
                    iDonViThuLy = Convert.ToInt32(Request["donvi"].ToString());
                    iDonViTiepNhan = Convert.ToInt32(Request["donvi"].ToString());
                }
                if (Request["hienthi"] != null)
                {
                    page = 0;
                    iHienThi = Convert.ToInt32(Request["hienthi"].ToString());
                    ViewData["select-hienthi"] = kn.Option_HinhThuc(iHienThi);
                    if (iHienThi == (int)HienThiDon.TiepNhan)
                    {
                        ViewData["select-donvi"] = kn.Option_DonVi_XuLyDon(iDonViThuLy, iDonVi, 3);// Lấy danh sách đơn vị thụ lý
                        don = lstDon.Where(v => v.ITINHTRANGXULY == 3 && v.IDONVITIEPNHAN == iDonVi).ToList();
                        if (iDonViThuLy != 0)
                        {
                            don = don.Where(v => v.IDONVITHULY == iDonViThuLy).ToList();
                        }

                    }
                    else if (iHienThi == (int)HienThiDon.ThuLy)
                    {
                        ViewData["select-donvi"] = kn.Option_DonVi_TiepNhanDon(iDonViThuLy, iDonVi, 3);// Lấy danh sách đơn vị tiếp nhận
                        don = lstDon.Where(v => v.ITINHTRANGXULY == 3 && v.IDONVITHULY == iDonVi && v.IDONVITIEPNHAN != iDonVi).ToList();
                        if (iDonViTiepNhan != 0)
                        {
                            don = don.Where(v => v.IDONVITIEPNHAN == iDonViTiepNhan).ToList();
                        }
                    }
                }


                //ViewData["list"] = kn.KNTC_Don_Chuyenxuly(don, u.tk_action, page, iDonVi);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại đã chuyển xử lý");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Chuyenxuly_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                UserInfor u = GetUserInfor();
                if (u.tk_action.is_lanhdao)
                {
                    ViewData["select-donvi"] = kn.Option_DonVi_XuLyDon((int)u.user_login.IDONVI, 3);
                }
                else
                {
                    ViewData["select-donvi"] = "<option value=" + u.user_login.IDONVI + ">" + u.tk_action.tendonvi + "</option>";
                }

                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Chuyenxuly_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị from tìm kiếm nâng câo đơn khiếu nại đã chuyển xử lý");
                return null;
            }
        }
        public ActionResult Datraloi(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }

            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Dachuyenxuly");
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("45,44", act)) { Response.Redirect("/Home/Error/"); return null; }
                string key = null;
                int trangthai = 0;
                int iKhoa = 0;
                ViewData["select-trangthai"] = kn.OptionTrangThaiDonDaTraLoi(0);
                DateTime dTuNgay;
                string tungay;
                if (!DateTime.TryParseExact(Request["dTuNgay"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dTuNgay))
                {
                    tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                }
                else
                {
                    tungay = dTuNgay.ToString("dd-MMM-yyyy");
                }

                DateTime dDenNgay;
                string denngay;
                if (!DateTime.TryParseExact(Request["dDenNgay"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dDenNgay))
                {
                    denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                }
                else
                {
                    denngay = dDenNgay.ToString("dd-MMM-yyyy");
                }
                int iDonVi = (int)u.user_login.IDONVI;
                int iluanchuyen = (int)LoaiLichSu.ChuyenXuLy;
                //var don = kntc.GetListLichSuDon(key, tungay, denngay, (int)u.user_login.IDONVI, iluanchuyen).Where(v => v.ITINHTRANGXULY == (int)TrangThaiDonDaTraLoi.DangXuLy || v.ITINHTRANGXULY == (int)TrangThaiDonDaTraLoi.HoanThanh).ToList();
                //if (Convert.ToInt32(Request["trangthai"]) != 0)
                //{
                //    trangthai = Convert.ToInt32(Request["trangthai"]);
                //    ViewData["select-trangthai"] = kn.OptionTrangThaiDonDaTraLoi(trangthai);
                //    don = don.Where(v => v.ITINHTRANGXULY == trangthai).ToList();
                //}
                //ViewData["list"] = kn.KNTC_Don_Chuyenxuly(don, u.tk_action, page, iDonVi);
                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                var iUser = u.tk_action.iUser;
                if (u.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                condition.Add("P_USER", iUser);
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVI", iDonVi);
                condition.Add("P_ICHUYENXULY", iluanchuyen);
                if (!act.is_lanhdao)
                    condition.Add("P_IUSER_GIAOXULY", act.iUser);
                else
                {
                    condition.Add("P_IUSER_GIAOXULY", 0);
                }
                if (Convert.ToInt32(Request["trangthai"]) != 0)
                {
                    trangthai = Convert.ToInt32(Request["trangthai"]);
                    ViewData["select-trangthai"] = kn.OptionTrangThaiDonDaTraLoi(Convert.ToInt32(trangthai));
                }
                condition.Add("P_ITINHTRANGXULY", trangthai);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);
                //if (!act.is_lanhdao)
                //    condition.Add("P_IUSER_GIAOXULY", act.iUser);
                //else
                //{
                //    condition.Add("P_IUSER_GIAOXULY", 0);
                //}
                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_IQUANHUYEN", param.IDIAPHUONG_1);
                condition.Add("P_IXAPHUONG", param.IDIAPHUONG_2);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonDaChuyenCoTraLoi(condition);
                if (Request["iCanhBao"] != null && Request["iCanhBao"] != "null" && Request["iCanhBao"] != "")
                {
                    int canhbao = (int)Convert.ToInt64(Request["iCanhBao"]);
                    don = don.Where(x => x.ICANHBAO == canhbao).ToList();
                }
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    htmlList = caution + kn.KNTC_Don_Chuyenxuly(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlPhanTrang = phantrang;
                }
                else
                {
                    htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }

                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(0);


                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại đã chuyển xử lý");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Datraloi_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                //ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                ViewData["select-trangthai"] = kn.OptionTrangThaiDonDaTraLoi(0);
                UserInfor u = GetUserInfor();
                if (u.tk_action.is_lanhdao)
                {
                    ViewData["select-donvi"] = kn.Option_DonVi_XuLyDon((int)u.user_login.IDONVI, 3);
                }
                else
                {
                    ViewData["select-donvi"] = "<option value=" + u.user_login.IDONVI + ">" + u.tk_action.tendonvi + "</option>";
                }
                return PartialView("../Ajax/Kntc/Datraloi_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị from tìm kiếm nâng câo đơn khiếu nại đã chuyển xử lý");
                return null;
            }
        }

        public ActionResult Ajax_Datraloi_formexport()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                
                UserInfor u_info = GetUserInfor();
                string loaibaocao = "";
                if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
                {
                    loaibaocao = loaibaocao + "<option selected value='0'>Tất cả<option>";
                    loaibaocao = loaibaocao + "<option value='2'>HĐND Tỉnh<option>";
                    loaibaocao = loaibaocao + "<option value='1'>Đoàn ĐBQH Tỉnh<option>";
                }
                else if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                {
                    loaibaocao = loaibaocao + "<option selected value='2'>HĐND Tỉnh<option>";
                }
                else if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                {
                    loaibaocao = loaibaocao + "<option selected value='1'>Đoàn ĐBQH Tỉnh<option>";
                }
                ViewData["opt-coquantraloi"] = Get_Option_CoQuan_TheoNhom(0);
                ViewData["loaibaocao"] = loaibaocao;
                ViewData["opt-ky"] = kn.Option_Ky();
                ViewData["opt-year"] = kn.Option_Year_List(countNext: 0, selectedValue: DateTime.Now.Year);
                ViewData["opt-month"] = kn.Option_Month_List();
                ViewData["opt-quy"] = kn.Option_Quy_List();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-diaphuong"] = List_CheckBox_Huyen_ThiXa_ThanhPho();
                ViewData["opt-doituong"] = Get_Option_DoiTuong_By_ID_USERS();

                return PartialView("../Ajax/Kntc/Datraloi_formexport");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị from tìm kiếm nâng câo đơn khiếu nại đã chuyển xử lý");
                return null;
            }
        }

        public string List_CheckBox_Huyen_ThiXa_ThanhPho(int iDaibieu = 0, Boolean selectedAll = true)
        {

            UserInfor u_info = GetUserInfor();
            string str = "";
            var listHuyen_ThiXa_ThanhPho = _thietlap.Get_Diaphuong().Where(x => x.IPARENT == 26).ToList();
            foreach (var item in listHuyen_ThiXa_ThanhPho)
            {
                if (selectedAll = true)
                {
                    str += "<option selected value ='" + item.IDIAPHUONG + "'>" + item.CTEN + "</option>";
                }
                else
                {
                    str += "<option value ='" + item.IDIAPHUONG + "'>" + item.CTEN + "</option>";
                }
            }
            return str;
        }
        public ActionResult Dahuongdan(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                //if (!_base.ActionMulty_Redirect_("44,14,15", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Dahuongdan");

                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                var iUser = u.tk_action.iUser;
                if (u.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                condition.Add("P_USER", iUser);
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITHULY", u.user_login.IDONVI);
                condition.Add("P_ITINHTRANGXULY", 10);

                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);

                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_IQUANHUYEN", param.IDIAPHUONG_1);
                condition.Add("P_IXAPHUONG", param.IDIAPHUONG_2);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonDaXuLyGiaiQuyet(condition);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    htmlList = caution + kn.KNTC_Don_Daxuly(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlPhanTrang = phantrang;
                }
                else
                {
                    htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }
                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }
                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại đã xử lý");
                return View("../Home/Error_Exception");
            }
        }

        public ActionResult Ajax_Dahuongdan_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                //ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                UserInfor u = GetUserInfor();
                if (u.tk_action.is_lanhdao)
                {
                    ViewData["select-donvi"] = kn.Option_DonVi_XuLyDon((int)u.user_login.IDONVI, 6);
                }
                else
                {
                    ViewData["select-donvi"] = "<option value=" + u.user_login.IDONVI + ">" + u.tk_action.tendonvi + "</option>";
                }

                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Dahuongdan_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiểm thị form tìm kiếm danh sách đơn khiếu nại đã xử lý");
                return null;
            }
        }

        public ActionResult Chuatraloi(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }

            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Chuatraloi");
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("45,44", act)) { Response.Redirect("/Home/Error/"); return null; }
                string key = null;
                int trangthai = 0;
                int iKhoa = 0;
                ViewData["select-trangthai"] = kn.OptionTrangThaiDonChuaTraLoi(0);
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                // denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                DateTime dTuNgay;
                string tungay;
                if (!DateTime.TryParseExact(Request["dTuNgay"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dTuNgay))
                {
                    tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                }
                else
                {
                    tungay = dTuNgay.ToString("dd-MMM-yyyy");
                }

                DateTime dDenNgay;
                string denngay;
                if (!DateTime.TryParseExact(Request["dDenNgay"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dDenNgay))
                {
                    denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                }
                else
                {
                    denngay = dDenNgay.ToString("dd-MMM-yyyy");
                }
                int iDonVi = (int)u.user_login.IDONVI;
                int iluanchuyen = (int)LoaiLichSu.ChuyenXuLy;
                //var don = kntc.GetListLichSuDon(key, tungay, denngay, (int)u.user_login.IDONVI, iluanchuyen).Where(v => v.ITINHTRANGXULY == (int)TrangThaiDonChuaTraLoi.ChoXuLy || v.ITINHTRANGXULY == (int)TrangThaiDonChuaTraLoi.KhongXuLy).ToList();
                //if (Convert.ToInt32(Request["trangthai"]) != 0)
                //{
                //    trangthai = Convert.ToInt32(Request["trangthai"]);
                //    ViewData["select-trangthai"] = kn.OptionTrangThaiDonChuaTraLoi(trangthai);
                //    don = don.Where(v => v.ITINHTRANGXULY == trangthai).ToList();
                //}
                //ViewData["list"] = kn.KNTC_Don_Chuyenxuly(don, u.tk_action, page, iDonVi);
                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                var iUser = u.tk_action.iUser;
                if (u.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                condition.Add("P_USER", iUser);
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVI", iDonVi);
                condition.Add("P_ICHUYENXULY", iluanchuyen);
                //condition.Add("P_ICHUYENXULY", 0);
                if (Convert.ToInt32(Request["trangthai"]) != 0)
                {
                    trangthai = Convert.ToInt32(Request["trangthai"]);
                    ViewData["select-trangthai"] = kn.OptionTrangThaiDonChuaTraLoi(Convert.ToInt32(trangthai));
                }
                if (!act.is_lanhdao)
                    condition.Add("P_IUSER_GIAOXULY", act.iUser);
                else
                {
                    condition.Add("P_IUSER_GIAOXULY", 0);
                }
                condition.Add("P_ITINHTRANGXULY", trangthai);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);

                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_IQUANHUYEN", param.IDIAPHUONG_1);
                condition.Add("P_IXAPHUONG", param.IDIAPHUONG_2);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                
                var don = kntc.ListDonDaChuyen(condition);
                if (Request["iCanhBao"] != null && Request["iCanhBao"] != "null" && Request["iCanhBao"] != "")
                {
                    int canhbao = (int)Convert.ToInt64(Request["iCanhBao"]);
                    don = don.Where(x => x.ICANHBAO == canhbao).ToList();
                }
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='7' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    htmlList = caution + kn.KNTC_Don_Chuyenxuly(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='7'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlPhanTrang = phantrang;
                }
                else
                {
                    htmlList = "<tr><td colspan='7' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }
                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }

                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = Get_Option_KyHop(0);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại đã chuyển xử lý");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Chuatraloi_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                //ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon(0, 0);
                UserInfor u = GetUserInfor();
                if (u.tk_action.is_lanhdao)
                {
                    ViewData["select-donvi"] = kn.Option_DonVi_XuLyDon((int)u.user_login.IDONVI, 3);
                }
                else
                {
                    ViewData["select-donvi"] = "<option value=" + u.user_login.IDONVI + ">" + u.tk_action.tendonvi + "</option>";
                }

                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Chuatraloi_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị from tìm kiếm nâng câo đơn khiếu nại đã chuyển xử lý");
                return null;
            }
        }
        public ActionResult Ajax_Dachuyenxuly_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                ViewData["select-trangthai"] = kn.Option_TrangThaiChuyenXuLy(0);
                ViewData["select-donvi"] = tl.OptionCoQuan_TreeList();
                UserInfor u = GetUserInfor();
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Dachuyenxuly_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị from tìm kiếm nâng câo đơn khiếu nại đã chuyển xử lý");
                return null;
            }
        }
        public ActionResult Dangxuly(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Dangxuly");
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_(action_kntc, u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                //int iDonVi = Convert.ToInt32(u.user_login.IDONVI);
                //string key = "";
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                //var don = kntc.GetListAllDon(key, tungay, denngay).Where(v => v.ITINHTRANGXULY == 4 && v.IDONVITHULY == iDonVi).ToList();

                //ViewData["list"] = kn.KNTC_Don_Dangxuly(don, act, page, iDonVi);

                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITHULY", u.user_login.IDONVI);
                condition.Add("P_ITINHTRANGXULY", (int)TrangThaiDon.DangXuLy);

                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);

                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonDaNhanXuLy(condition);
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    ViewData["list"] = caution + kn.KNTC_Don_Dangxuly(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    ViewData["phantrang"] = phantrang;
                }
                else ViewData["list"] = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";

                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Danh đơn khiếu nại đang xử lý");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Dangxuly_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                // ViewData["select-donvi"] = kn.Option_DonVi_XuLyDon(0, 4);
                UserInfor u = GetUserInfor();
                if (u.tk_action.is_lanhdao)
                {
                    ViewData["select-donvi"] = kn.Option_DonVi_XuLyDon((int)u.user_login.IDONVI, 4);
                }
                else
                {
                    ViewData["select-donvi"] = "<option value=" + u.user_login.IDONVI + ">" + u.tk_action.tendonvi + "</option>";
                }
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Dangxuly_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm đơn khiếu nại đang xử lý");
                return null;
            }
        }
        public ActionResult Phanloai()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);

                TaikhoanAtion act = GetUserInfor().tk_action;
                if (!_base.Action_(12, act)) { Response.Redirect("/Home/Error/"); return null; }
                //Action_Redirect(12, id_user());
                // new id
                if (Request["id"] == null) { Response.Redirect("/Home/Error/"); }
                string iddontrung_encrypt = Request["iDontrung"];
                if (iddontrung_encrypt != null)
                {
                    ViewData["iddontrung"] = iddontrung_encrypt;
                }
                string id_encrypt = Request["id"];
                string type = Request["type"]; // type = 1; màn chờ xử lý, type = 0 màn không đủ điều kiện
                ViewData["type"] = type;
                string id_decrypt = HashUtil.Decode_ID(id_encrypt, Request.Cookies["url_key"].Value);
                int id = Convert.ToInt32(id_decrypt);
                SetTokenAction("kntc_phanloai", id);
                // end
                KNTC_DON don = kntc.GetDON(id);
                if (don != null)
                {
                    ViewData["file"] = kn.File_View(id, "kntc_don");
                    ViewData["don"] = don;
                    List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                    ViewData["opt-nguondon"] = kn.Option_Nguondon(nguondon, 0);
                    ViewData["opt-quoctich"] = kn.Option_QuocTich((int)don.INGUOIGUI_QUOCTICH);
                    ViewData["opt-dantoc"] = kn.Option_DanToc((int)don.INGUOIGUI_DANTOC);
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IHIENTHI", 1);
                    _condition.Add("IDELETE", 0);
                    List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                    ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, (int)don.IDIAPHUONG_0);
                    ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, (int)don.IDIAPHUONG_0, (int)don.IDIAPHUONG_1);
                    ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)don.ILOAIDON);
                    ViewData["opt-noidung"] = kn.Option_NoiDungDon((int)don.INOIDUNG, (int)don.ILINHVUC);
                    ViewData["opt-tinhchat"] = kn.Option_TinhChatDon((int)don.ITINHCHAT, (int)don.INOIDUNG);
                    List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                    //ViewData["opt-linhvuc"] = tl.Option_Nhom_LinhVuc(linhvuc,0,0,(int)don.ILINHVUC,"");
                    ViewData["opt-linhvuc"] = kn.Option_Nhom_LinhVuc(linhvuc, (int)don.ILINHVUC, (int)don.ILOAIDON);
                    //ViewData["don-trung"] = kn.List_DonTrung(id);
                    ViewData["id_encrypt"] = id_encrypt;
                    ViewData["don_detail"] = kn.KNTC_Detail(id, id_encrypt);
                    ViewData["file"] = kn.File_View(id, "kntc_don");
                    ViewData["id"] = id;
                }

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị trang phân loại đơn khiếu nại đang xử lý");
                return null;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Phanloai(FormCollection fc)
        {

            if (!CheckAuthToken()) { return null; }
            try
            {
                // new id 

                string id_encrypt = fc["id"];
                string id_decrypt = HashUtil.Decode_ID(id_encrypt, Request.Cookies["url_key"].Value);
                int iDon = Convert.ToInt32(id_decrypt);



                //end
                if (!CheckTokenAction("kntc_phanloai", iDon))
                {
                    Response.Redirect("/Home/Error/");
                    return null;
                }
                KNTC_DON don = kntc.GetDON(iDon);
                if (don != null)
                {
                    don.IPLSONGUOI = Convert.ToInt32(fc["id_Soluongnguoi"]);
                    don.ILOAIDON = Convert.ToInt32(fc["iLoaiDon"]);
                    don.ITINHCHAT = Convert.ToInt32(fc["iTinhChat"]);
                    don.INOIDUNG = Convert.ToInt32(fc["iNoiDung"]);
                    don.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                    don.CHITIETLYDO_LUUTHEODOI = func.RemoveTagInput(fc["iMoTa"]);
                    int iThuocThamQuyen = Convert.ToInt32(fc["iThuocThamQuyen"]);
                    don.ITHAMQUYEN = iThuocThamQuyen;
                    //if (don.ITINHTRANGXULY == 1)
                    //{
                    //    don.ITINHTRANGXULY = 2;//đã phân loại
                    //    //don.IDONVITHULY = 
                    //}
                    string thamquyen = "";
                    if (iThuocThamQuyen == 0)
                    {
                        don.IDUDIEUKIEN = 0;
                        //don.iDuDieuKien_KetQua = 0;
                        thamquyen = " Không thuộc thẩm quyền";
                    }
                    else
                    {
                        don.IDUDIEUKIEN = Convert.ToInt32(fc["iDuDieuKien"]);
                        don.IDUDIEUKIEN_KETQUA = Convert.ToInt32(fc["iHinhThuc"]);
                        thamquyen = " Thuộc thẩm quyền";
                    }
                    if(Convert.ToInt32(fc["iDuDieuKien"]) == -1)
                    {
                        don.ITINHTRANGXULY = 1; // Chưa xử lý, chờ phân loại
                    }
                    else
                    {
                        don.ITINHTRANGXULY = 2;// Đã phân loại
                    }

                    if (fc["iDontrung"] != null && fc["iDontrung"] != "")
                    {
                        string idontrung_decrypt = HashUtil.Decode_ID(fc["iDontrung"], Request.Cookies["url_key"].Value);
                        int iDonTrung = Convert.ToInt32(idontrung_decrypt);
                        KNTC_DON dontrung = kntc.GetDON(iDonTrung);
                        if (dontrung != null)
                        {
                            KNTC_DON dongoc = kntc.GetDON((int)dontrung.IDONTRUNG);
                            if (dongoc != null)
                            {
                                don.IDONTRUNG = dongoc.IDON;
                                dongoc.IDONTRUNG = -1;
                                kntc.Update_Don(dongoc);
                            }
                            else
                            {
                                don.IDONTRUNG = iDonTrung;
                                dontrung.IDONTRUNG = -1;
                                kntc.Update_Don(dontrung);
                            }

                        }
                        else { don.IDONTRUNG = iDonTrung; }
                        thamquyen += "; Xác nhận đơn trùng lặp.";
                    }
                    kntc.Update_Don(don);
                    int iUser = id_user();
                    kntc.Tracking_KNTC(iUser, iDon, "Phân loại đơn: " + thamquyen);

                }

                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Phân loại đơn khiếu nại đang xử lý");
                return null;
            }
        }
        public ActionResult Thuocthamquyen(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Thuocthamquyen");
                string url_cookie = func.Get_Url_keycookie();
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("44,10,11,12,13", act)) { Response.Redirect("/Home/Error/"); return null; }
                //ActionMulty_Redirect("44,10,11,12,13", iUser);

                //int user = u.tk_action.iUser;
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                //var don = kntc.GetListAllDon("", tungay, denngay).Where(v => v.ITINHTRANGXULY == (int)TrangThaiDon.DaPhanLoai && v.ITHAMQUYEN == (int)ThamQuyenXuLy.ThuocThamQuyen && v.IDUDIEUKIEN == (int)DieuKienXuLy.ChuaXacDinh && v.IDONVITIEPNHAN == u.user_login.IDONVI).ToList();
                //if (!_base.ActionMulty_("44", act))
                //{
                //    don = don.Where(x => x.IUSER_DUOCGIAOXULY == user).ToList();
                //}
                //ViewData["list"] = kn.KNTC_Don_Thuocthamquyen(don, act, page);

                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                var iUser = u.tk_action.iUser;
                if (u.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                condition.Add("P_USER", iUser);
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITIEPNHAN", u.user_login.IDONVI);
                condition.Add("P_ITINHTRANGXULY", (int)TrangThaiDon.DaPhanLoai);
                condition.Add("P_ITHAMQUYEN", (int)ThamQuyenXuLy.ThuocThamQuyen);
                condition.Add("P_IDIEUKIEN", (int)(int)DieuKienXuLy.ChuaXacDinh);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);
                if (!_base.ActionMulty_("44", act))
                {
                    ViewData["btn-add"] = " style='display:none' ";
                    condition.Add("P_IUSER_DUOCGIAOXULY", u.user_login.IUSER);
                }
                else
                {
                    condition.Add("P_IUSER_DUOCGIAOXULY", 0);
                }
                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonDaPhanLoai(condition);
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    ViewData["list"] = caution + kn.KNTC_Don_Thuocthamquyen(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='5'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    ViewData["phantrang"] = phantrang;
                }
                else ViewData["list"] = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";

                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Danh sách đơn khiếu nại thuộc thẩm quyền");
                return null;
            }
        }
        public ActionResult Ajax_Thuocthamquyen_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                UserInfor u = GetUserInfor();
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Thuocthamquyen_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm đơn khiếu nại thuộc thẩm quyền");
                return null;
            }
        }
        public ActionResult Dudieukien(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Dudieukien");
                string url_cookie = func.Get_Url_keycookie();
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("44,11,12,13", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                // _base.ActionMulty_Redirect_("44,11,12,13", act);

                //int user = u.tk_action.iUser;
                //string key = "";
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));

                //var don = kntc.GetListAllDon(key, tungay, denngay).Where(v => v.ITINHTRANGXULY == 2 && v.ITHAMQUYEN == 1 && v.IDUDIEUKIEN == 1 && v.IDONVITIEPNHAN == u.user_login.IDONVI).ToList();
                //if (!_base.ActionMulty_("44", act))
                //{
                //    don = don.Where(x => x.IUSER_DUOCGIAOXULY == user).ToList();
                //}

                //ViewData["list"] = kn.KNTC_Don_Dudieukien(don, act, page);
                //return View();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                DateTime dTuNgay;
                string tungay;
                if (!DateTime.TryParseExact(Request["dTuNgay"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dTuNgay))
                {
                    tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                }
                else
                {
                    tungay = dTuNgay.ToString("dd-MMM-yyyy");
                }

                DateTime dDenNgay;
                string denngay;
                if (!DateTime.TryParseExact(Request["dDenNgay"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dDenNgay))
                {
                    denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                }
                else
                {
                    denngay = dDenNgay.ToString("dd-MMM-yyyy");
                }
                string key = null;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                var iUser = u.tk_action.iUser;
                if (u.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                condition.Add("P_USER", iUser);
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITIEPNHAN", u.user_login.IDONVI);
                condition.Add("P_ITINHTRANGXULY", (int)TrangThaiDon.DaPhanLoai);
                condition.Add("P_ITHAMQUYEN", (int)ThamQuyenXuLy.ThuocThamQuyen);
                condition.Add("P_IDIEUKIEN", (int)(int)DieuKienXuLy.DuDieuKien);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);
                if (!_base.ActionMulty_("44", act))
                {
                    ViewData["btn-add"] = " style='display:none' ";
                    condition.Add("P_IUSER_DUOCGIAOXULY", u.user_login.IUSER);
                }
                else
                {
                    condition.Add("P_IUSER_DUOCGIAOXULY", 0);
                }
                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_IQUANHUYEN", param.IDIAPHUONG_1);
                condition.Add("P_IXAPHUONG", param.IDIAPHUONG_2);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonDaPhanLoai(condition);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    htmlList = caution + kn.KNTC_Don_Dudieukien(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlPhanTrang = phantrang;
                }
                else
                {
                    htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }
                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }

                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại đủ điều kiện");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Dudieukien_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                //ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon(0, 0);
                //ViewData["opt-khoa"] = Get_Option_KhoaHop();
                if (u_info.user_login.ITYPE == 3)
                {
                    ViewData["opt-khoa"] = Get_Option_Khoa_TheoLoai(loai: 1);
                }
                else if (u_info.user_login.ITYPE == 4)
                {
                    ViewData["opt-khoa"] = Get_Option_Khoa_TheoLoai(loai: 0);
                }
                else
                {
                    ViewData["opt-khoa"] = Get_Option_KhoaHop();
                }
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u_info.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Dudieukien_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm đơn khiếu nại đủ điều kiện");
                return null;
            }
        }
        public ActionResult Khongdudieukien(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                string url_cookie = func.Get_Url_keycookie();
                if (!_base.ActionMulty_Redirect_("44,11,12,13", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Khongdudieukien");
                //int user = u.tk_action.iUser;
                //string key = "";
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));

                //var don = kntc.GetListAllDon(key, tungay, denngay).Where(v => v.ITINHTRANGXULY == 2 && v.ITHAMQUYEN == 1 && v.IDUDIEUKIEN == 0 && v.IDONVITIEPNHAN == u.user_login.IDONVI).ToList();
                //if (!_base.ActionMulty_("44", act))
                //{
                //    don = don.Where(x => x.IUSER_DUOCGIAOXULY == user).ToList();
                //}
                //ViewData["list"] = kn.KNTC_Don_Khongdudieukien(don, act, page);
                //return View();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                DateTime dTuNgay;
                string tungay;
                if (!DateTime.TryParseExact(Request["dTuNgay"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dTuNgay))
                {
                    tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                }
                else
                {
                    tungay = dTuNgay.ToString("dd-MMM-yyyy");
                }

                DateTime dDenNgay;
                string denngay;
                if (!DateTime.TryParseExact(Request["dDenNgay"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dDenNgay))
                {
                    denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                }
                else
                {
                    denngay = dDenNgay.ToString("dd-MMM-yyyy");
                }
                int iKhoa = 0;
                string key = null;

                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                var iUser = u.tk_action.iUser;
                if (u.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                condition.Add("P_USER", iUser);
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITIEPNHAN", u.user_login.IDONVI);
                condition.Add("P_ITINHTRANGXULY", (int)TrangThaiDon.DaPhanLoai);
                condition.Add("P_ITHAMQUYEN", (int)ThamQuyenXuLy.ThuocThamQuyen);
                condition.Add("P_IDIEUKIEN", (int)(int)DieuKienXuLy.KhongDuDieuKien);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);
                if (!_base.ActionMulty_("44", act))
                {
                    ViewData["btn-add"] = " style='display:none' ";
                    condition.Add("P_IUSER_DUOCGIAOXULY", u.user_login.IUSER);
                }
                else
                {
                    condition.Add("P_IUSER_DUOCGIAOXULY", 0);
                }
                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_IQUANHUYEN", param.IDIAPHUONG_1);
                condition.Add("P_IXAPHUONG", param.IDIAPHUONG_2);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonDaPhanLoai(condition);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    htmlList = caution + kn.KNTC_Don_Khongdudieukien(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlPhanTrang = phantrang;
                }
                else
                {
                    htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }
                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }
                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);
                ViewData["formchuyentiep"] = "Khongdudieukien";
                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại không đủ điều kiện");
                return View("../Home/Error_Exception");

            }
        }
        public ActionResult Ajax_Khongdudieukien_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();

                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                //ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc_LoaiDon(0, 0);
                UserInfor u = GetUserInfor();
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Khongdudieukien_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm đơn khiếu nại không đủ điều kiện");
                return null;
            }
        }
        public ActionResult Khongthuocthamquyen(int page = 1)
        {

            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("44,11,12,13", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Khongthuocthamquyen");
                //int user = u.tk_action.iUser;
                //string key = "";
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));

                //var don = kntc.GetListAllDon(key, tungay, denngay).Where(v => v.ITINHTRANGXULY == 2 && v.ITHAMQUYEN == 0 && v.IDONVITIEPNHAN == u.user_login.IDONVI).ToList();
                //if (!_base.ActionMulty_("44", act))
                //{
                //    don = don.Where(v => v.IUSER_DUOCGIAOXULY == user).ToList();
                //}
                //ViewData["list"] = kn.KNTC_Don_Khongthuocthamquyen(don, act, page);
                //return View();
                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITIEPNHAN", u.user_login.IDONVI);
                condition.Add("P_ITINHTRANGXULY", (int)TrangThaiDon.DaPhanLoai);
                condition.Add("P_ITHAMQUYEN", (int)ThamQuyenXuLy.KhongThuocThamQuyen);
                condition.Add("P_IDIEUKIEN", (int)(int)DieuKienXuLy.KhongDuDieuKien);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);
                if (!_base.ActionMulty_("44", act))
                {
                    ViewData["btn-add"] = " style='display:none' ";
                    condition.Add("P_IUSER_DUOCGIAOXULY", u.user_login.IUSER);
                }
                else
                {
                    condition.Add("P_IUSER_DUOCGIAOXULY", 0);
                }
                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonDaPhanLoai(condition);
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='7' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    ViewData["list"] = caution + kn.KNTC_Don_Khongthuocthamquyen(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='7'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    ViewData["phantrang"] = phantrang;
                }
                else ViewData["list"] = "<tr><td colspan='7' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";

                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại không đủ điều kiện");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Khongthuocthamquyen_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                UserInfor u = GetUserInfor();
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Khongthuocthamquyen_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm đơn khiếu nại không thuộc thẩm quyền");
                return null;
            }
        }
        public ActionResult Daxuly(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("44,14,15", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Daxuly");
                //int iDonVi = Convert.ToInt32(u.user_login.IDONVI);
                //string key = "";
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                //var don = kntc.GetListDonDaXuLy(key, tungay, denngay, iDonVi, (int)TrangThaiDon.HoanThanh).ToList();

                //ViewData["list"] = kn.KNTC_Don_Daxuly(don, act, page, iDonVi);
                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITHULY", u.user_login.IDONVI);
                condition.Add("P_ITINHTRANGXULY", (int)TrangThaiDon.HoanThanh);

                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);

                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonDaXuLyGiaiQuyet(condition);
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    ViewData["list"] = caution + kn.KNTC_Don_Daxuly(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    ViewData["phantrang"] = phantrang;
                }
                else ViewData["list"] = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";

                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại đã xử lý");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Daxuly_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                //ViewData["select-donvi"] = kn.Option_DonVi_XuLyDon(0, 6);
                UserInfor u = GetUserInfor();
                if (u.tk_action.is_lanhdao)
                {
                    ViewData["select-donvi"] = kn.Option_DonVi_XuLyDon((int)u.user_login.IDONVI, 6);
                }
                else
                {
                    ViewData["select-donvi"] = "<option value=" + u.user_login.IDONVI + ">" + u.tk_action.tendonvi + "</option>";
                }

                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Daxuly_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiểm thị form tìm kiếm danh sách đơn khiếu nại đã xử lý");
                return null;
            }
        }
        public ActionResult Khongxuly(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {

                TaikhoanAtion act = GetUserInfor().tk_action;
                if (!_base.ActionMulty_Redirect_("44,14,15", act)) { Response.Redirect("/Home/Error/"); return null; }
                UserInfor u = GetUserInfor();
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                //int iDonVi = (int)u.user_login.IDONVI;
                //string key = "";
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));

                //var don = kntc.GetListAllDon(key, tungay, denngay).Where(v => v.IDONVITHULY == iDonVi && v.ITINHTRANGXULY == Convert.ToDecimal(TrangThaiDon.KhongXuLy)).ToList();


                //ViewData["list"] = kn.KNTC_Don_Khongxuly(don, act, page);
                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                var iUser = u.tk_action.iUser;
                if (u.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                condition.Add("P_USER", iUser);
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                if (!act.is_lanhdao)
                    condition.Add("P_IUSER_GIAOXULY", act.iUser);
                else
                {
                    condition.Add("P_IUSER_GIAOXULY", 0);
                }
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);
                var don = kntc.ListDonLuuKhongXuly(condition);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (don != null && don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    htmlList = caution + kn.KNTC_Don_Khongxuly(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlPhanTrang = phantrang;
                }
                else
                {
                    htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }
                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }
                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại đã xử lý");
                return null;
            }
        }
        public ActionResult Ajax_Khongxuly_formsearch()
        {

            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                //ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-huyen"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, AppConfig.IDIAPHUONG, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                UserInfor u = GetUserInfor();
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Khongxuly_formsearch");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Hiển thị from tìm kiếm đơn khiếu nại không xử lý");
                //Handle Exception;
                return null;
            }
        }
        public ActionResult Tracuu(int page = 1)
        {

            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_(action_kntc, u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int iDonVi = Convert.ToInt32(u.user_login.IDONVI);

                // TRA CƯU
                /**if (Request["cNoiDung"] != null)
                {**/
                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int tinhtrang = -1;
                int idonvi = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); ViewData["cNoiDung"] = key; }
                if (Request["dTuNgay"] != null && Request["dTuNgay"] != "") { tungay = func.RemoveTagInput(func.ConvertDateToSql(Request["dTuNgay"])); ViewData["dTuNgay"] = Request["dTuNgay"]; }
                if (Request["dDenngay"] != null && Request["dDenngay"] != "") { denngay = func.RemoveTagInput(func.ConvertDateToSql(Request["dDenngay"])); ViewData["dDenngay"] = Request["dDenngay"]; }
                if (Request["iTinhTrangXuLy"] != null && Request["iTinhTrangXuLy"] != "") { tinhtrang = Convert.ToInt32(Request["iTinhTrangXuLy"]);  }
                Dictionary<string, object> con = new Dictionary<string, object>();
                con.Add("IHIENTHI", 1);
                con.Add("IDELETE", 0);
                List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(con);
                if (Request["iThamQuyenDonVi"] != null && Request["iThamQuyenDonVi"] != "") { idonvi = Convert.ToInt32(Request["iThamQuyenDonVi"]); ViewData["opt-donvi"] = "<option value='0'>--- Chọn tất cả</option>" + kn.OptionCoQuan(coquan, 0, 0, idonvi, 0); }
                condition.Add("P_CKEY", key);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITHULY", idonvi);
                condition.Add("P_ITINHTRANGDON", tinhtrang);
                if(act.is_lanhdao)
                    condition.Add("P_IUSER", 0);
                else
                    condition.Add("P_IUSER", act.iUser);
                condition.Add("P_IDONVI", iDonVi);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);
                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                    
                condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                   
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                    
                var don = kntc.ListDonTraCuu(condition);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

               
                if (don.Count() > 0)
                {
                    int total = 0;
                    DONTRACUU d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }

                    htmlList =  caution + kn.TraCuuDon(don, u, url_cookie);
                    string phantrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlPhanTrang = phantrang + "</tbody></table></div></div></div>";
                }
                else
                {
                    htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList =  "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }
                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }

                //view search
                ViewData["opt-nguondon"] = kn.Option_NguonDon((int)param.INGUONDON);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, (int)param.IDIAPHUONG_0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)param.ILOAIDON);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc((int)param.ILINHVUC);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon((int)param.INOIDUNG);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon((int)param.ITINHCHAT);
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap(iDonVi, (int)param.IUSER);
                ViewData["opt_trangthai"] = kn.OptionTrangThai(-1);
                /**}
                else
                {
                    ViewData["opt-nguondon"] = kn.Option_NguonDon();
                    ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap(iDonVi, 0);
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IHIENTHI", 1);
                    _condition.Add("IDELETE", 0);
                    List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                    ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, ID_DiaPhuong_HienTai);
                    ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                    ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                    ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                    ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                    ViewData["opt_trangthai"] = kn.OptionTrangThai(-1);
                    Dictionary<string, object> condition = new Dictionary<string, object>();
                    condition.Add("IHIENTHI", 1);
                    condition.Add("IDELETE", 0);
                    List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(condition);
                    ViewData["opt-donvi"] = "<option value='0'>--- Chọn tất cả</option>" + kn.OptionCoQuan(coquan, 0, 0, 0);
                }**/

                //ViewData["list"] = view.KNTC_Don_Tracuu();
                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Tìm kiếm nâng cao đơn khiếu nại ");
                return View("../Home/Error_Exception");
            }
        }
        [HttpGet]
        public ActionResult Dontrung_Exl()
        {
            UserInfor u = GetUserInfor();
            int iDonVi = Convert.ToInt32(u.user_login.IDONVI);
            if (!_base.ActionMulty_Redirect_(action_kntc, u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            string url_cookie = func.Get_Url_keycookie();
            Dictionary<string, object> condition = new Dictionary<string, object>();
            string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
            string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy"));
            string key = null;
            int tinhtrang = -1;
            int idonvi = 0;
            if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); }
            if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
            if (!String.IsNullOrEmpty(Request["dTuNgay"])) { tungay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"])); }
            if (!String.IsNullOrEmpty(Request["dDenngay"])) { denngay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenngay"])); }
            if (Request["iTinhTrangXuLy"] != "") { tinhtrang = Convert.ToInt32(Request["iTinhTrangXuLy"]); }
            Dictionary<string, object> con = new Dictionary<string, object>();
            con.Add("IHIENTHI", 1);
            con.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(con);
            if (Request["iThamQuyenDonVi"] != "") { idonvi = Convert.ToInt32(Request["iThamQuyenDonVi"]); }
            condition.Add("P_CKEY", key);
            condition.Add("P_DTUNGAY", tungay);
            condition.Add("P_DDENNGAY", denngay);
            condition.Add("P_IDONVITHULY", idonvi);
            condition.Add("P_ITINHTRANGDON", tinhtrang);
            condition.Add("P_IDONVI", iDonVi);
            KNTC_DON param = get_Request_Paramt_KNTC();
            condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
            condition.Add("P_INGUONDON", param.INGUONDON);
            //ViewData["opt-nguondon"] = kn.Option_NguonDon((int)param.INGUONDON);
            condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
            //ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(0, (int)param.IDIAPHUONG_0);
            condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
            condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
            condition.Add("P_ILOAIDON", param.ILOAIDON);
            //ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)param.ILOAIDON);
            condition.Add("P_ILINHVUC", param.ILINHVUC);
            //ViewData["opt-linhvuc"] = kn.Option_LinhVuc((int)param.ILINHVUC);
            condition.Add("P_INOIDUNG", param.INOIDUNG);
            //ViewData["opt-noidung"] = kn.Option_NoiDungDon((int)param.INOIDUNG);
            condition.Add("P_ITINHCHAT", param.ITINHCHAT);
            //ViewData["opt-tinhchat"] = kn.Option_TinhChatDon((int)param.ITINHCHAT);
            condition.Add("P_INGUOINHAN", param.IUSER);
            //ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap(iDonVi, (int)param.IUSER);
            var don = kntc.ListDonTrung(condition);
            XLWorkbook w_b = new XLWorkbook();
            var wb = w_b.Worksheets.Add("DanhSach");
            wb.Style.Font.FontSize = 13;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(3).Width = 50;
            wb.Column(4).Width = 50;
            wb.Column(5).Width = 25;
            wb.Column(6).Width = 25;
            wb.Column(7).Width = 50;
            wb.Column(8).Width = 25;
            wb.Column(9).Width = 25;
            wb.Column(10).Width = 25;
            wb.Column(11).Width = 25;
            wb.Column(12).Width = 25;
            wb.Column(13).Width = 25;
            wb.Column(14).Width = 25;
            wb.Column(15).Width = 25;
            wb.PageSetup.FitToPages(1, 1);
            wb.Cell(2, 2).Value = "Ban Dân Nguyện";
            wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                   .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            wb.Cell(2, 8).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            wb.Range(2, 8, 2, 16).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                .Font.Bold = true;

            wb.Cell(3, 8).Value = "Độc lập - Tự do - Hạnh phúc";
            wb.Range(3, 8, 3, 16).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetFontSize(11);
            wb.Cell(5, 5).Value = "DANH SÁCH ĐƠN KHIẾU NẠI TỐ CÁO";
            wb.Range(5, 5, 5, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            if (tungay != "")
            {
                wb.Cell(6, 5).Value = "Từ ngày " + Request["dTuNgay"] + " đến ngày " + Request["dDenNgay"] + "";
                wb.Range(6, 5, 6, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(11);
            }

            wb.Cell(8, 2).Value = "STT";
            wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 3).Value = "Tên người gửi ";
            wb.Cell(8, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 4).Value = "Địa chỉ";
            wb.Cell(8, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 5).Value = "Loại nội dung";
            wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

            wb.Cell(8, 6).Value = "Tính chất";
            wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 7).Value = "Nội dung";
            wb.Cell(8, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 8).Value = "Số lượng trùng";
            wb.Cell(8, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 9).Value = "Số đơn trùng";
            wb.Cell(8, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 10).Value = "Ghi chú";
            wb.Cell(8, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 11).Value = "Ngày tiếp nhận";
            wb.Cell(8, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 12).Value = "Nguồn đơn";
            wb.Cell(8, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 13).Value = "Loại đơn";
            wb.Cell(8, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 14).Value = "Lĩnh vực";
            wb.Cell(8, 14).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 15).Value = "Đơn vị thụ lý";
            wb.Cell(8, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 16).Value = "Trạng thái đơn";
            wb.Cell(8, 16).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            int j = 9;

            int stt = 1;
            foreach (var t in don)
            {

                wb.Cell(j, 2).Value = stt;
                wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 3).Value = Server.HtmlDecode(t.CNGUOIGUI_TEN);
                wb.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                string diachi = "";
                if ((t.CNGUOIGUI_DIACHI) != null) diachi += (t.CNGUOIGUI_DIACHI) + " ,";
                if ((t.CTENHUYEN) != null) diachi += (t.CTENHUYEN) + " ,";
                if ((t.CTINHTHANH) != null) diachi += (t.CTINHTHANH) + " .";

                wb.Cell(j, 4).Value = diachi;
                wb.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);


                wb.Cell(j, 5).Value = Server.HtmlDecode(t.CNOIDUNGDON);
                wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j, 6).Value = Server.HtmlDecode(t.CTINHCHAT); ;
                wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 7).Value = Server.HtmlDecode(t.CNOIDUNG);
                wb.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                //int sodontrung = (int)t.ISOLUONGTRUNG + kntc._List_All_Don_Trung((int)t.IDON).Count;
                int sodontrung = Convert.ToInt32(t.ISOLUONGTRUNG);
                wb.Cell(j, 8).Value = sodontrung;
                wb.Cell(j, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 9).Value = t.SODONTRUNG;
                wb.Cell(j, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j, 10).Value = t.CGHICHU;
                wb.Cell(j, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                string ngaynhan = ""; if (t.DNGAYNHAN != null) ngaynhan = func.ConvertDateVN(t.DNGAYNHAN.ToString());
                wb.Cell(j, 11).Value = ngaynhan;
                wb.Cell(j, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 12).Value = Server.HtmlDecode(t.CNGUONDON);
                wb.Cell(j, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j, 13).Value = Server.HtmlDecode(t.CLOAIDON);
                wb.Cell(j, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 14).Value = Server.HtmlDecode(t.CLINHVUC);
                wb.Cell(j, 14).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 15).Value = Server.HtmlDecode(t.CTEN);
                wb.Cell(j, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j, 16).Value = kn.GetTinhTrangDon(t.TINHTRANG);
                wb.Cell(j, 16).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);


                stt++;
                j++;
            }

            wb.Cell(j, 2).Value = "";
            wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(j, 3).Value = "TỔNG SỐ";
            wb.Range(j, 3, j, 15).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                .Alignment.SetWrapText(true).Font.SetFontSize(11);

            wb.Cell(j, 16).Value = don.Count();
            wb.Cell(j, 16).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                .Alignment.SetWrapText(true).Font.SetFontSize(11);

            wb.Cell(j + 2, 10).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
            wb.Range(j + 2, 4, j + 2, 16).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
            wb.Cell(j + 3, 10).Value = "NGƯỜI LẬP";
            wb.Range(j + 3, 10, j + 3, 16).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                .Font.Bold = true;
            wb.Rows().AdjustToContents();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=Danhsachdon_exl" + DateTime.Now.Date + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                w_b.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null;
        }
        [HttpGet]
        public ActionResult Search_Exl()
        {
            UserInfor u = GetUserInfor();
            int iDonVi = Convert.ToInt32(u.user_login.IDONVI);
            if (!_base.ActionMulty_Redirect_(action_kntc, u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            string url_cookie = func.Get_Url_keycookie();
            Dictionary<string, object> condition = new Dictionary<string, object>();
            string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
            string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
            string key = null;
            int tinhtrang = -1;
            int idonvi = 0;
            if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); }
            if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
            if (Request["dTuNgay"] != "") { tungay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"])); }
            if (Request["dDenngay"] != "") { denngay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenngay"])); }
            if (Request["iTinhTrangXuLy"] != "") { tinhtrang = Convert.ToInt32(Request["iTinhTrangXuLy"]); }
            Dictionary<string, object> con = new Dictionary<string, object>();
            con.Add("IHIENTHI", 1);
            con.Add("IDELETE", 0);
            List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(con);
            if (Request["iThamQuyenDonVi"] != "") { idonvi = Convert.ToInt32(Request["iThamQuyenDonVi"]); }
            condition.Add("P_CKEY", key);
            condition.Add("P_DTUNGAY", tungay);
            condition.Add("P_DDENNGAY", denngay);
            condition.Add("P_IDONVITHULY", idonvi);
            condition.Add("P_ITINHTRANGDON", tinhtrang);
            condition.Add("P_IDONVI", iDonVi);
            condition.Add("P_PAGE", 1);
            condition.Add("P_PAGE_SIZE", Int32.MaxValue);
            KNTC_DON param = get_Request_Paramt_KNTC();
            condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
            condition.Add("P_INGUONDON", param.INGUONDON);
            // ViewData["opt-nguondon"] = kn.Option_NguonDon((int)param.INGUONDON);
            condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
            // ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(0, (int)param.IDIAPHUONG_0);
            condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
            condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
            condition.Add("P_ILOAIDON", param.ILOAIDON);
            //  ViewData["opt-loaidon"] = kn.Option_LoaiDon((int)param.ILOAIDON);
            condition.Add("P_ILINHVUC", param.ILINHVUC);
            // ViewData["opt-linhvuc"] = kn.Option_LinhVuc((int)param.ILINHVUC);
            condition.Add("P_INOIDUNG", param.INOIDUNG);
            //ViewData["opt-noidung"] = kn.Option_NoiDungDon((int)param.INOIDUNG);
            condition.Add("P_ITINHCHAT", param.ITINHCHAT);
            //ViewData["opt-tinhchat"] = kn.Option_TinhChatDon((int)param.ITINHCHAT);
            condition.Add("P_INGUOINHAN", param.IUSER);
            // ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap(iDonVi, (int)param.IUSER);
            var don = kntc.ListDonTraCuu(condition);
            XLWorkbook w_b = new XLWorkbook();
            var wb = w_b.Worksheets.Add("DanhSach");
            wb.Style.Font.FontSize = 13;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(3).Width = 50;
            wb.Column(4).Width = 50;
            wb.Column(5).Width = 25;
            wb.Column(6).Width = 50;
            wb.Column(7).Width = 25;
            wb.Column(8).Width = 25;
            wb.Column(9).Width = 25;
            wb.Column(10).Width = 25;
            wb.Column(11).Width = 25;
            wb.Column(12).Width = 25;
            wb.Column(13).Width = 25;
            wb.Column(14).Width = 25;
            wb.PageSetup.FitToPages(1, 1);
            wb.Cell(2, 2).Value = "Ban Dân Nguyện";
            wb.Range(2, 2, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                   .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            wb.Cell(2, 8).Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            wb.Range(2, 8, 2, 15).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                .Font.Bold = true;

            wb.Cell(3, 8).Value = "Độc lập - Tự do - Hạnh phúc";
            wb.Range(3, 8, 3, 15).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetFontSize(11);
            wb.Cell(5, 5).Value = "DANH SÁCH ĐƠN KHIẾU NẠI TỐ CÁO";
            wb.Range(5, 5, 5, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            if (tungay != "")
            {
                wb.Cell(6, 5).Value = "Từ ngày " + Request["dTuNgay"] + " đến ngày " + Request["dDenNgay"] + "";
                wb.Range(6, 5, 6, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetFontSize(11);
            }

            wb.Cell(8, 2).Value = "STT";
            wb.Cell(8, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 3).Value = "Tên người gửi ";
            wb.Cell(8, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 4).Value = "Địa chỉ";
            wb.Cell(8, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 5).Value = "Ngày tiếp nhận ";
            wb.Cell(8, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

            wb.Cell(8, 6).Value = "Nội dung";
            wb.Cell(8, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 7).Value = "Nguồn đơn";
            wb.Cell(8, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 8).Value = "Loại đơn";
            wb.Cell(8, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 9).Value = "Lĩnh vực";
            wb.Cell(8, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 10).Value = "Nhóm nội dung";
            wb.Cell(8, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 11).Value = "Tính chất vụ việc";
            wb.Cell(8, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 12).Value = "Trạng thái đơn";
            wb.Cell(8, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 13).Value = "Ghi chú";
            wb.Cell(8, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(8, 14).Value = "Đơn vị thụ lý";
            wb.Cell(8, 14).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

            int j = 9;

            int stt = 1;
            foreach (var t in don)
            {

                wb.Cell(j, 2).Value = stt;
                wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 3).Value = Server.HtmlDecode(t.CNGUOIGUI_TEN);
                wb.Cell(j, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                string diachi = "";
                if ((t.CNGUOIGUI_DIACHI) != null) diachi += (t.CNGUOIGUI_DIACHI) + " ,";
                if ((t.CTENHUYEN) != null) diachi += (t.CTENHUYEN) + " ,";
                if ((t.CTINHTHANH) != null) diachi += (t.CTINHTHANH) + " .";

                wb.Cell(j, 4).Value = diachi;
                wb.Cell(j, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

                string ngaynhan = ""; if (t.DNGAYNHAN != null) ngaynhan = func.ConvertDateVN(t.DNGAYNHAN.ToString());
                wb.Cell(j, 5).Value = ngaynhan;
                wb.Cell(j, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j, 6).Value = Server.HtmlDecode(t.CNOIDUNG); ;
                wb.Cell(j, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 7).Value = Server.HtmlDecode(t.CNGUONDON);
                wb.Cell(j, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 8).Value = Server.HtmlDecode(t.CLOAIDON);
                wb.Cell(j, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 9).Value = Server.HtmlDecode(t.CLINHVUC);
                wb.Cell(j, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j, 10).Value = Server.HtmlDecode(t.CNOIDUNGDON);
                wb.Cell(j, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 11).Value = Server.HtmlDecode(t.CTINHCHAT);
                wb.Cell(j, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);

                wb.Cell(j, 12).Value = kn.GetTinhTrangDon(t.TINHTRANG);
                wb.Cell(j, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 13).Value = t.CGHICHU;
                wb.Cell(j, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);
                wb.Cell(j, 14).Value = Server.HtmlDecode(t.CTEN);
                wb.Cell(j, 14).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                              .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                              .Alignment.SetWrapText(true).Font.SetFontSize(11);


                stt++;
                j++;
            }

            wb.Cell(j, 2).Value = "";
            wb.Cell(j, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                .Alignment.SetWrapText(true).Font.SetFontSize(11);
            wb.Cell(j, 3).Value = "TỔNG SỐ";
            wb.Range(j, 3, j, 14).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                .Alignment.SetWrapText(true).Font.SetFontSize(11);

            wb.Cell(j, 15).Value = don.Count();
            wb.Cell(j, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.SetBold(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                .Alignment.SetWrapText(true).Font.SetFontSize(11);

            wb.Cell(j + 2, 10).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "";
            wb.Range(j + 2, 4, j + 2, 14).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11);
            wb.Cell(j + 3, 10).Value = "NGƯỜI LẬP";
            wb.Range(j + 3, 10, j + 3, 14).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetFontSize(11)
                                                .Font.Bold = true;
            wb.Rows().AdjustToContents();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=Danhsachdon_exl" + DateTime.Now.Date + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                w_b.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null;
        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Ajax_Tracuu_result()
        {
            UserInfor u = GetUserInfor();
            if (!_base.ActionMulty_Redirect_(action_kntc, u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            if (!CheckAuthToken()) { return null; }
            try
            {
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = Request["cNoiDung"].ToString();
                int doandongnguoi = 0;
                if (Request["iDoanDongNguoi"] != null)
                {
                    doandongnguoi = 1;
                }
                int nguondon = Convert.ToInt32(Request["iNguonDon"]);
                int tinhthanh = Convert.ToInt32(Request["iDiaPhuong_0"]);
                int quoctich = Convert.ToInt32(Request["iNguoiGui_QuocTich"]);
                int dantoc = Convert.ToInt32(Request["iNguoiGui_DanToc"]);
                int loaidon = Convert.ToInt32(Request["iLoaiDon"]);
                int linhvuc = Convert.ToInt32(Request["iLinhVuc"]);
                int noidung = Convert.ToInt32(Request["iNoiDung"]);
                int tinhchat = Convert.ToInt32(Request["iTinhChat"]);
                int tinhtrang = Convert.ToInt32(Request["iTinhTrangXuLy"]);
                int donvi = Convert.ToInt32(Request["iThamQuyenDonVi"]);
                if (Request["dTuNgay"] != null && Request["dTuNgay"].ToString() != "")
                {
                    tungay = func.ConvertDateToSql(Request["dTuNgay"]);
                }
                if (Request["dDenNgay"] != null && Request["dDenNgay"].ToString() != "")
                {
                    denngay = func.ConvertDateToSql(Request["dDenNgay"]);
                }

                // ViewData["list"] = kn.TraCuuDon(key, doandongnguoi, tungay, denngay, nguondon, tinhthanh, quoctich, dantoc, loaidon, linhvuc, donvi, noidung, tinhchat, tinhtrang, (int)u.user_login.IDONVI);

                return PartialView("../Ajax/Kntc/Tracuu_result");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiếm nâng cao đơn khiếu nại ");
                //Handle Exception;
                return null;
            }
        }
        public ActionResult Ajax_LoadLinhVucNoiDung(int iLinhVuc)
        {

            if (!CheckAuthToken()) { return null; }
            try
            {

                UserInfor u = GetUserInfor();
                if (!_base.ActionMulty_Redirect_(action_kntc, u.tk_action)) { return null; }
                string str = "<select name='iNoiDung' id='iNoiDung' class='input-medium chosen-select' style='width:100%' onchange='LoadOpTinhChat()'>" +
                                            "<option value='-1'>- - - Chọn nội dung</option><option value='0'>Chưa xác định</option>" +
                                            "" + kn.Option_NoiDungDon_ThuocLinhVuc(0, iLinhVuc) + "" +
                                            "</select>";
                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Load lĩnh vực nội dung ");
                return null;
            }
        }
        public ActionResult Ajax_LoadTinhChatNoiDung(int iNoiDung)
        {

            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u = GetUserInfor();
                if (!_base.ActionMulty_Redirect_(action_kntc, u.tk_action)) { return null; }
                string str = "<select name='iTinhChat' id='iTinhChat' class='input-medium chosen-select' style='width:100%'>" +
                                            "<option value='-1'>- - - Chọn tính chất</option><option value='0'>Chưa xác định</option>" +
                                            "" + kn.Option_TinhChatDon_ThuocNguonDon(0, iNoiDung) + "" + "</select>";
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
        public ActionResult Ajax_LoadLinhVucByLoaiDon(int iLoaiDon)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string str = "<select name='iLinhVuc' id='iLinhVuc' class='input-medium chosen-select' onchange=\"LoadLinhVuc()\" style='width:100%'>" +
                                            "<option value='0'>- - - Chưa xác định</option>" + kn.Option_LinhVuc_LoaiDon(0, iLoaiDon) + "" + "</select>";

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

        public ActionResult Ajax_LoadNguonDonByLoai(int iLoai)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon().Where(n => n.ILOAI == iLoai).ToList();
                
                string str = "<select class='input-block-level chosen-select' id='iNguonDon' name='iNguonDon'> <option value = '0' > ---Chưa xác định</option>"
                             + kn.Option_Nguondon(nguondon, 0) +"</select>";
                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Load nguồn đơn ");
                //Handle Exception;
                return null;
            }
        }
        [HttpPost]
        public ActionResult Ajax_HienThiDonTrung(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                KNTC_DON don = new KNTC_DON();
                don.CNGUOIGUI_TEN = func.RemoveTagInput(fc["cNguoiGui_Ten"]);
                don.IDIAPHUONG_0 = Convert.ToInt32(fc["iDiaPhuong_0"]);
                don.CNOIDUNG = "";
                var list = kntc.Get_List_DonTrung(don);

                int count = 1;
                if (list.Count() > 0)
                {
                    ViewData["dontrung"] += "<table class='table table-bordered table-condensed nomargin'><tr><th width='3%' class='tcenter'>STT</th><th nowrap>Nội dung đơn</th><th nowrap>Người gửi / Địa chỉ người gửi</th><th nowrap>Tình trạng đơn</th></tr>";

                    foreach (var d in list)
                    {
                        //string check = "";
                        string chon = "";
                        string id_encr = HashUtil.Encode_ID(d.IDON.ToString());
                        KNTC k = kn.KNTC_Detail((int)d.IDON, id_encr);
                        //string chon_trung = "<input type='radio' " + check + " name='dontrung' id='dontrung' value='" + d.IDON + "'/>";
                        string tinhtrangdon = k.tinhtrang;
                        ViewData["dontrung"] += "<tr><td class='tcenter b'>" + count + "</td><td>" + d.CNOIDUNG +
                            "</td><td><p><strong>" + d.CNGUOIGUI_TEN + "</strong></p>" + kn.DiaChi_Don((int)d.IDIAPHUONG_0, (int)d.IDIAPHUONG_1, d.CNGUOIGUI_DIACHI) + "</td><td>" + tinhtrangdon + "</td></tr>";
                        count++;
                    }
                    ViewData["dontrung"] += "</table>";

                }
                else
                {
                    ViewData["dontrung"] += " <div class='alert alert-success tcenter b nomargin'><i class='icon-ok-sign'></i> Không tìm thấy đơn trùng</div>";
                }

                return PartialView("../Ajax/Kntc/Kiemtrung");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Sửa giám sát", null, 1);
                return null;
            }

        }
        public ActionResult HuongDanGuiDon(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "phieuhuongdanguidon.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();
            ViewData["donvi"] = _thietlap.GetBy_Quochoi_CoquanID((int)u_info.user_login.IDONVI).CTEN;
            //string id = Request["id"];
            int iDon = Convert.ToInt32(id);
            KNTC_DON don = kntc.GetDON(iDon);
            ViewData["diachi"] = Server.HtmlEncode(don.CNGUOIGUI_DIACHI) + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_1).CTEN + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_0).CTEN;
            ViewData["Thongtindonkntc"] = don;

            return View();
        }
        public ActionResult PheuChuyenDonCoBaoCao(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Phieuchuyendoncobaocao.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();
            //  ViewData["donvi"] = _thietlap.GetBy_Quochoi_CoquanID((int)u_info.user_login.IDONVI).CTEN;
            KNTC_DON don = kntc.GetDON(id);
            ViewData["diachi"] = Server.HtmlEncode(don.CNGUOIGUI_DIACHI) + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_1).CTEN + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_0).CTEN;
            ViewData["Thongtindonkntc"] = don;
            return View();
        }
        public ActionResult PheuChuyenDonKhongBaoCao(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Phieuchuyendonkhongbaocao.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();
            KNTC_DON don = kntc.GetDON(id);
            ViewData["diachi"] = Server.HtmlEncode(don.CNGUOIGUI_DIACHI) + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_1).CTEN + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_0).CTEN;
            ViewData["Thongtindonkntc"] = don;
            return View();
        }
        public ActionResult GiayBaoTin1(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Giaybaotin1.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();
            KNTC_DON don = kntc.GetDON(id);
            ViewData["diachi"] = Server.HtmlEncode(don.CNGUOIGUI_DIACHI) + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_1).CTEN + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_0).CTEN;
            ViewData["Thongtindonkntc"] = don;
            return View();
        }
        public ActionResult GiayBaoTin2(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Giaybaotin2.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();
            KNTC_DON don = kntc.GetDON(id);
            ViewData["diachi"] = Server.HtmlEncode(don.CNGUOIGUI_DIACHI) + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_1).CTEN + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_0).CTEN;
            ViewData["Thongtindonkntc"] = don;
            return View();
        }
        public ActionResult HoSoDon(int id, string id_encr)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Hosodon.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();
            KNTC_DON don = kntc.GetDON(id);
            KNTC k_ = kn.KNTC_Detail(id, id_encr);
            ViewData["don"] = don;
            ViewData["d"] = k_;
            return View();
        }
        public ActionResult DeXuatXuLyDonKhieuNai(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Dexuatxulydonkhieunai.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();

            KNTC_DON don = kntc.GetDON(id);
            ViewData["diachi"] = Server.HtmlEncode(don.CNGUOIGUI_DIACHI) + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_1).CTEN + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_0).CTEN;
            ViewData["Thongtindonkntc"] = don;
            return View();
        }
        public ActionResult TraDonKhieuNai(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Tradonkhieunai.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();
            KNTC_DON don = kntc.GetDON(id);
            ViewData["diachi"] = Server.HtmlEncode(don.CNGUOIGUI_DIACHI) + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_1).CTEN + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_0).CTEN;
            ViewData["Thongtindonkntc"] = don;
            return View();
        }
        public ActionResult KhongThuLyGiaiQuyet(int id)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Khongthulygiaiquyet.doc"));
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "UTF-8";
            UserInfor u_info = GetUserInfor();
            KNTC_DON don = kntc.GetDON(id);
            ViewData["diachi"] = Server.HtmlEncode(don.CNGUOIGUI_DIACHI) + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_1).CTEN + "," + _thietlap.GetBy_DiaphuongID((int)don.IDIAPHUONG_0).CTEN;
            ViewData["Thongtindonkntc"] = don;
            return View();
        }
        public ActionResult Danhanxuly(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Danhanxuly");
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("13,14,15,44,45", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                // func.SetCookies("url_return", Request.Url.AbsoluteUri);
                //int iDonVi = Convert.ToInt32(u.user_login.IDONVI);

                //string key = "";
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                //var don = kntc.GetListAllDon(key, tungay, denngay).Where(v => v.ITINHTRANGXULY == 3 && v.IDONVITHULY == iDonVi).ToList();
                //ViewData["list"] = kn.KNTC_Don_Danhanxuly(don, u.tk_action, page, iDonVi);

                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITHULY", u.user_login.IDONVI);
                condition.Add("P_ITINHTRANGXULY", (int)TrangThaiDon.ChoXuLy);

                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);

                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonDaNhanXuLy(condition);
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON_MOICAPNHAT d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    ViewData["list"] = caution + kn.KNTC_Don_Danhanxuly(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    ViewData["phantrang"] = phantrang;
                }
                else ViewData["list"] = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";

                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại đã chuyển xử lý");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Danhanxuly_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                UserInfor u = GetUserInfor();
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Danhanxuly_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm đơn khiếu nại không thuộc thẩm quyền");
                return null;
            }
        }
        public ActionResult Daluanchuyen(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                func.SetCookies("link_action", "Daluanchuyen");
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_("13,44", u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                // func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int iDonVi = Convert.ToInt32(u.user_login.IDONVI);
                int iluanchuyen = (int)LoaiLichSu.Luanchuyen;
                int trangthai = 0;
                ViewData["select-trangthai"] = kn.OptionTrangThaiDonXuLy(trangthai);
                //string key = "";
                //string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                //string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                //var don = kntc.GetListLichSuDon(key, tungay, denngay, iDonVi, iluanchuyen).ToList();

                //ViewData["list"] = kn.KNTC_Don_Luanchuyenxuly(don, u.tk_action, page, iDonVi);
                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int iKhoa = 0;

                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVI", iDonVi);
                condition.Add("P_ICHUYENXULY", iluanchuyen);
                if (Convert.ToInt32(Request["trangthai"]) != 0)
                {
                    trangthai = Convert.ToInt32(Request["trangthai"]);
                    ViewData["select-trangthai"] = kn.OptionTrangThaiDonXuLy(trangthai);
                }
                condition.Add("P_ITINHTRANGXULY", trangthai);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);

                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonDaLuanChuyen(condition);
                if (don.Count() > 0)
                {
                    int total = 0;
                    KNTCDON d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='4' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }
                    ViewData["list"] = caution + kn.KNTC_Don_Luanchuyenxuly(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='4'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    ViewData["phantrang"] = phantrang;
                }
                else ViewData["list"] = "<tr><td colspan='4' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";

                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị danh sách đơn khiếu nại đã luân chuyển xử lý");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Daluanchuyenxuly_formsearch()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                ViewData["opt-khoa"] = Get_Option_KhoaHop();
                ViewData["opt-nguondon"] = kn.Option_NguonDon();
                ViewData["opt-quoctich"] = kn.Option_QuocTich();
                ViewData["opt-dantoc"] = kn.Option_DanToc();
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = kn.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-noidung"] = kn.Option_NoiDungDon(0);
                ViewData["opt-tinhchat"] = kn.Option_TinhChatDon(0);
                ViewData["opt-linhvuc"] = kn.Option_LinhVuc(0);
                UserInfor u = GetUserInfor();
                ViewData["opt-nguoinhap"] = kn.Option_NguoiNhap((int)u.user_login.IDONVI, 0);
                return PartialView("../Ajax/Kntc/Daluanchuyenxuly_formsearch");
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Hiển thị form tìm kiếm đơn khiếu nại không thuộc thẩm quyền");
                return null;
            }
        }
        public ActionResult Tamxoa(int page = 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login/"); return null; }
            try
            {
                UserInfor u = GetUserInfor();
                TaikhoanAtion act = u.tk_action;
                if (!_base.ActionMulty_Redirect_(action_kntc, u.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int iDonVi = Convert.ToInt32(u.user_login.IDONVI);
                string url_cookie = func.Get_Url_keycookie();
                Dictionary<string, object> condition = new Dictionary<string, object>();
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                string tungay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MinValue).ToString("dd/MM/yyyy"));
                string denngay = func.ConvertDateToSql(Convert.ToDateTime(DateTime.MaxValue).ToString("dd/MM/yyyy"));
                string key = null;
                int tinhtrang = -1;
                int idonvi = 0;
                int iKhoa = 0;
                if (Request["ip_noidung"] != null) { key = func.RemoveTagInput(Request["ip_noidung"]); ViewData["ip_noidung"] = key; }
                if (Request["cNoiDung"] != null) { key = func.RemoveTagInput(Request["cNoiDung"]); ViewData["cNoiDung"] = key; }
                if (Request["iKhoa"] != "0" && Request["iKhoa"] != null)
                {
                    iKhoa = Convert.ToInt32(func.RemoveTagInput(Request["iKhoa"]));
                }
                //if (Request["dTuNgay"] != "") { tungay = func.RemoveTagInput(func.ConvertDateToSql(Request["dTuNgay"])); ViewData["dTuNgay"] = Request["dTuNgay"]; }
                //if (Request["dDenngay"] != "") { denngay = func.RemoveTagInput(func.ConvertDateToSql(Request["dDenngay"])); ViewData["dDenngay"] = Request["dDenngay"]; }
                //if (Request["iTinhTrangXuLy"] != "") { tinhtrang = Convert.ToInt32(Request["iTinhTrangXuLy"]); ViewData["opt_trangthai"] = kn.OptionTrangThai(tinhtrang); }
                Dictionary<string, object> con = new Dictionary<string, object>();
                con.Add("IHIENTHI", 1);
                con.Add("IDELETE", 0);
                List<QUOCHOI_COQUAN> coquan = _thietlap.GetBy_List_Quochoi_Coquan(con);
                if (Request["iThamQuyenDonVi"] != "") { idonvi = Convert.ToInt32(Request["iThamQuyenDonVi"]); ViewData["opt-donvi"] = "<option value='0'>--- Chọn tất cả</option>" + kn.OptionCoQuan(coquan, 0, 0, idonvi, 0); }
                var iUser = u.tk_action.iUser;
                if (u.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                condition.Add("P_USER", iUser);
                condition.Add("P_CKEY", key);
                condition.Add("P_IKHOA", iKhoa);
                condition.Add("P_DTUNGAY", tungay);
                condition.Add("P_DDENNGAY", denngay);
                condition.Add("P_IDONVITHULY", idonvi);
                condition.Add("P_ITINHTRANGDON", tinhtrang);
                condition.Add("P_IDONVI", iDonVi);
                condition.Add("P_PAGE", page);
                condition.Add("P_PAGE_SIZE", post_per_page);
                KNTC_DON param = get_Request_Paramt_KNTC();
                condition.Add("P_IDOANDONGNGUOI", param.IDOANDONGNGUOI);
                condition.Add("P_INGUONDON", param.INGUONDON);
                condition.Add("P_ITINHTHANH", param.IDIAPHUONG_0);
                condition.Add("P_IQUOCTICH", param.INGUOIGUI_QUOCTICH);
                condition.Add("P_IDANTOC", param.INGUOIGUI_DANTOC);
                condition.Add("P_ILOAIDON", param.ILOAIDON);
                condition.Add("P_ILINHVUC", param.ILINHVUC);
                condition.Add("P_INOIDUNG", param.INOIDUNG);
                condition.Add("P_ITINHCHAT", param.ITINHCHAT);
                condition.Add("P_INGUOINHAN", param.IUSER);
                var don = kntc.ListDonTamXoa(condition);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (don != null && don.Count > 0)
                {
                    int total = 0;
                    DONTRACUU d = don.FirstOrDefault();
                    if (d != null) total = (int)d.TOTAL;
                    string caution = "<tr><td colspan='6' class='alert tcenter alert-info'>Có " + total + " kết quả</td></tr>";
                    if (key == null) { caution = ""; }

                    htmlList = caution + kn.Dontamxoa(don, act, url_cookie);
                    string phantrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang(total, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlPhanTrang = phantrang + "</tbody></table></div></div></div>";
                }
                else
                {
                    htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr></tbody></table></div></div></div>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }
                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }
                int iKyHop = iKhoa;
                ViewData["opt-kyhop"] = Get_Option_KhoaHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Tìm kiếm nâng cao đơn khiếu nại ");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_LoadLinhVuc(int iLoaiDon)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string str = "<select name='iLinhVuc' id='iLinhVuc' class='input-medium chosen-select' onchange=\"LoadNoiDungByLinhVuc(this.value)\" style='width:100%'>" +
                                            "<option value='0'>- - - Chọn tất cả</option>" +
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
        public ActionResult Ajax_LoadNoiDung(int iLinhVuc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string str = "<select name='iNoiDung' id='iNoiDung' class='input-medium chosen-select' onchange=\"LoadTinhChatByNoiDung(this.value)\" style='width:100%'>" +
                                            "<option value='0'>- - - Chọn tất cả</option>" +
                                            "" + kn.Option_NoiDungDon_ThuocLinhVuc(0, iLinhVuc) + "" + "</select>";

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
        public ActionResult Ajax_LoadTinhChat(int iNoiDung)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string str = "<select name='iTinhChat' id='iTinhChat' class='input-medium chosen-select' style='width:100%'>" +
                                            "<option value='0'>- - - Chọn tất cả</option>" +
                                            "" + kn.Option_TinhChatDon_ThuocNguonDon(0, iNoiDung) + "" + "</select>";

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

        public ActionResult Ajax_Change_KyHopTheoLoai_option(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon().Where(n => n.ILOAI == id).ToList();
                if (id != 2)
                {
                    string strKyHop = Get_Option_KyHop_TheoLoai(iKyHop, id);
                    string strNguonDon = "<select class='input-block-level chosen-select' id='iNguonDon' name='iNguonDon'> <option value = '0' > ---Chưa xác định</option>"
                                 + kn.Option_Nguondon(nguondon, 0) + "</select>";
                    Response.Write(strNguonDon + "," + strKyHop);
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax thay đổi Phân loại kết quả trả lời.");
                return null;
            }

        }
        public string Get_Option_KyHop(int iKyHop = 0)
        {
            UserInfor u_info = GetUserInfor();
            List<QUOCHOI_KHOA> khoa = knBussiness.GetAll_KhoaHop().OrderBy(x => x.DBATDAU).ToList();
            if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
            {
                khoa = knBussiness.GetAll_KhoaHop().Where(x => x.ILOAI == 1).OrderBy(x => x.DBATDAU).ToList();
            }
            else if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
            {
                khoa = knBussiness.GetAll_KhoaHop().Where(x => x.ILOAI == 0).OrderBy(x => x.DBATDAU).ToList();
            }
            return kn.Option_Khoa_KyHop(khoa, iKyHop);
        }
        public string Get_Option_KhoaHop(int iKyHop = 0)
        {
            List<QUOCHOI_KHOA> khoa = knBussiness.GetAll_KhoaHop().OrderBy(x => x.DBATDAU).ToList();
            return kn.Option_Khoa_KyHop(khoa, iKyHop);
        }
        public string Get_Option_KhoaHopByDVTN(int iKyHop = 0, int donvi = 0)
        {
            List<QUOCHOI_KHOA> khoa = knBussiness.GetAll_KhoaHop().Where(x => x.ILOAI == donvi).OrderBy(x => x.DBATDAU).ToList();
            return kn.Option_Khoa_KyHop(khoa, iKyHop);
        }
        public string Get_Option_KyHop_TheoLoai(int iKyHop = 0, int loai = 0)
        {
            List<QUOCHOI_KHOA> khoa = knBussiness.GetAll_KhoaHop().Where(x => x.ILOAI == loai).OrderBy(x => x.DBATDAU).ToList();
            return "<select name='iKyHop' id='iKyHop' class='input-medium chosen-select' style='width:100%'>" +
                                             kn.Option_Khoa_KyHop(khoa, iKyHop) + "" + "</select>";
        }
        public string Get_Option_Khoa_TheoLoai(int iKyHop = 0, int loai = 0)
        {
            List<QUOCHOI_KHOA> khoa = knBussiness.GetAll_KhoaHop().Where(x => x.ILOAI == loai).OrderBy(x => x.DBATDAU).ToList();
            return kn.Option_Khoa_KyHop(khoa, iKyHop);
        }

        public ActionResult Ajax_Import_don_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                KNTC_DON_IMPORT ip = kntc.GetKntcDonImport(id);

                //if (iKyHop != 0) { dic.Add("iKyHop", iKyHop); }
                List<KNTC_DON> kn = kntc.GetByIdImport(id);

                foreach (var k in kn)
                {
                    kntc.Delete_Don((int)k.IDON);
                };
                kntc.DeleteKntcDonImport(ip);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa import kiến nghị ");
                throw;
            }
        }

        public ActionResult Import()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsolutePath);
                var import = kntc.GetAll_Import();
                UserInfor u_info = GetUserInfor();
                int iUser = (int)u_info.user_login.IUSER;
                if (!u_info.tk_action.is_lanhdao)
                {
                    import = import.Where(x => x.IUSER == iUser).ToList();
                }
                string result = Request.Cookies["importResult"] != null ? Request.Cookies["importResult"].Value : "";
                if (!string.IsNullOrEmpty(result))
                {
                    if("success".Equals(result, StringComparison.OrdinalIgnoreCase))
                    {
                        ViewData["importResult"] = "success";
                    }
                    if ("fail".Equals(result, StringComparison.OrdinalIgnoreCase))
                    {
                        ViewData["importResult"] = "fail";
                    }
                    func.RemoveCookies("importResult");
                }
                ViewData["list"] = import;
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách import kiến nghị");
                return View("../Home/Error_Exception");
            }
        }

        public ActionResult Download_Mau_Import()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                /**var document = kntc.GetDonKntcTemplate();
                if (document == null) { return null; }
                var cd = new System.Net.Mime.ContentDisposition
                {
                    // for example foo.bak
                    FileName = document.FileName,

                    // always prompt the user for downloading, set to true if you want 
                    // the browser to try to show the file inline
                    Inline = false,
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(document.Data, document.ContentType);**/
                var condition = new Dictionary<string, object>();
                condition.Add("IHIENTHI", 1);
                condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(condition);

                XLWorkbook w_b = new XLWorkbook();
                Sheet_DanhSachKhieuNaiTocao(w_b);
                Sheet_DanhMucHuyen(lstDiaphuong, w_b);
                Sheet_DanhMucXaPhuong(lstDiaphuong, w_b);
                Sheet_DanhMucLoaiDon(w_b);
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=khieunaitocao_example.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    w_b.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Download mẫu import đơn");
            }
            return null;
        }

        public IXLWorksheet Sheet_DanhMucHuyen(List<DIAPHUONG> lstDiaphuong, XLWorkbook w_b)
        {
            var wb = w_b.Worksheets.Add("DanhMucHuyen");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 11;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(1).Width = 20;
            wb.Column(2).Width = 40;
            wb.Column(3).Width = 20;
            wb.Column(4).Width = 20;

            wb.Row(1).Height = 30;
            wb.Cell(1, 1).Value = "Mã";
            wb.Range(1, 1, 1, 1).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188))
                                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 2).Value = "Tên";
            wb.Range(1, 2, 1, 2).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).
                                                    Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 3).Value = "Loại";
            wb.Range(1, 3, 1, 3).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            
            wb.Cell(1, 4).Value = "Mã đơn vị cha";
            wb.Range(1, 4, 1, 4).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;


            int row = 2;
            lstDiaphuong = lstDiaphuong.Where(x => x.IPARENT == AppConfig.IDIAPHUONG).OrderBy(t => t.IDIAPHUONG).ToList();
            
            foreach (var c in lstDiaphuong)
            {
                
                wb.Cell(row, 1).Value = c.IDIAPHUONG;
                wb.Range(row, 1, row, 1).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                
                wb.Cell(row, 2).Value = c.CTEN;
                wb.Range(row, 2, row, 2).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                wb.Cell(row, 3).Value = c.CTYPE;
                wb.Range(row, 3, row, 3).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                wb.Cell(row, 4).Value = c.IPARENT;
                wb.Range(row, 4, row, 4).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                row++;
            }
            return wb;
        }

        public IXLWorksheet Sheet_DanhMucXaPhuong(List<DIAPHUONG> lstDiaphuong, XLWorkbook w_b)
        {
            var wb = w_b.Worksheets.Add("DanhMucXaPhuong");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 11;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(1).Width = 20;
            wb.Column(2).Width = 40;
            wb.Column(3).Width = 20;
            wb.Column(4).Width = 20;

            wb.Row(1).Height = 30;
            wb.Cell(1, 1).Value = "Mã";
            wb.Range(1, 1, 1, 1).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188))
                                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 2).Value = "Tên";
            wb.Range(1, 2, 1, 2).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).
                                                    Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 3).Value = "Loại";
            wb.Range(1, 3, 1, 3).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 4).Value = "Mã đơn vị cha";
            wb.Range(1, 4, 1, 4).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;


            int row = 2;
            lstDiaphuong = lstDiaphuong.Where(x => x.IPARENT != AppConfig.IDIAPHUONG && x.IPARENT != 0).OrderBy(t => t.IPARENT).ToList();

            foreach (var c in lstDiaphuong)
            {

                wb.Cell(row, 1).Value = c.IDIAPHUONG;
                wb.Range(row, 1, row, 1).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                wb.Cell(row, 2).Value = c.CTEN;
                wb.Range(row, 2, row, 2).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                wb.Cell(row, 3).Value = c.CTYPE;
                wb.Range(row, 3, row, 3).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                wb.Cell(row, 4).Value = c.IPARENT;
                wb.Range(row, 4, row, 4).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                row++;
            }
            return wb;
        }

        public IXLWorksheet Sheet_DanhMucLoaiDon(XLWorkbook w_b)
        {
            var wb = w_b.Worksheets.Add("DanhMucLoaiDon");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 11;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(1).Width = 10;
            wb.Column(2).Width = 40;

            wb.Row(1).Height = 30;
            wb.Cell(1, 1).Value = "Mã";
            wb.Range(1, 1, 1, 1).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188))
                                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 2).Value = "Tên loại đơn ";
            wb.Range(1, 2, 1, 2).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).
                                                    Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            

            int row = 2;
            var listLoaiDon = kntc.List_LoaiDon().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();

            foreach (var c in listLoaiDon)
            {

                wb.Cell(row, 1).Value = c.ILOAIDON;
                wb.Range(row, 1, row, 1).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                wb.Cell(row, 2).Value = c.CTEN;
                wb.Range(row, 2, row, 2).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                row++;
            }
            return wb;
        }

        public IXLWorksheet Sheet_DanhSachKhieuNaiTocao(XLWorkbook w_b)
        {

            var wb = w_b.Worksheets.Add("DanhSachKhieuNaiToCao");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 13;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(1).Width = 8;
            wb.Column(2).Width = 30;
            wb.Column(3).Width = 20;
            wb.Column(4).Width = 20;
            wb.Column(5).Width = 50;
            wb.Column(6).Width = 20;
            wb.Column(7).Width = 40;
            wb.Column(8).Width = 20;
            wb.Column(9).Width = 20;
            wb.Row(1).Height = 50;
           
            wb.Cell(1, 1).Value = "STT";
            wb.Range(1, 1, 1, 1).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188))
                                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 2).Value = "Họ và tên công dân*";
            wb.Range(1, 2, 1, 2).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).
                                                    Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 3).Value = "Quận/Huyện*";
            wb.Range(1, 3, 1, 3).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 4).Value = "Xã/Phường/Thị trấn *";
            wb.Range(1, 4, 1, 4).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 5).Value = "Địa chỉ cụ thể*";
            wb.Range(1, 5, 1, 5).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188))
                                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 6).Value = "Số CMND*";
            wb.Range(1, 6, 1, 6).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 7).Value = "Nội dung đơn*";
            wb.Range(1, 7, 1, 7).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 8).Value = "Ngày nhận đơn* (dd / MM / yyyy)";
            wb.Range(1, 8, 1, 8).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 9).Value = "Loại đơn*";
            wb.Range(1, 9, 1, 9).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            return wb;
        }

        public ActionResult Ajax_Import_add()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                SetTokenAction("Import_add");
                return PartialView("../Ajax/Kntc/Import_add");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form import kiến nghị");
                throw;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Import_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                if (!CheckTokenAction("Import_add")) { Response.Redirect("/Home/Error/"); return null; }
                file = Request.Files["file_upload"];
                string file_name = "";
                if (file != null && file.ContentLength > 0)
                {
                    if (!CheckFile_Upload(file))
                    {
                        Response.Redirect("/Home/Error/?type=type"); return null;
                    }
                    else
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            file_name = UploadFile(file);
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KNTC_DON_IMPORT donImport = new KNTC_DON_IMPORT();
                donImport.CGHICHU = func.RemoveTagInput(fc["CGHICHU"]);
                donImport.IUSER = u_info.tk_action.iUser;
                donImport.DDATE = DateTime.Now;
                donImport.CFILE = file_name;
                donImport = kntc.AddKntcDonImport(donImport);

                if (donImport != null)
                {
                    Tracking(u_info.tk_action.iUser, "Import đơn: " + donImport.CGHICHU);
                    var resultImport = ImportKntcDon(donImport, u_info);
                    if(resultImport != null) 
                    {
                        donImport.ISODON = resultImport.TotalRecord;
                        donImport.ITINHTRANG = resultImport.Status ? 1 : 0;
                        kntc.UpdateKntcDonImport(donImport);

                        if (resultImport.Status)
                        {
                            func.SetCookies("importResult", "success");
                            return Redirect("/Kntc/Import/");
                        }
                        else
                        {
                            log.Log_Error(new Exception(), String.Join(" ", resultImport.ListRecordFailIndex.Select(x => x.Key + "==>" + x.Value).ToArray()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Import kiến nghị");
            }
            func.SetCookies("importResult", "fail");
            return Redirect("/Kntc/Import/");
        }

        public Boolean InsertFile_Kntc_Import(string ghichu)
        {
            bool result = true;
            try
            {
                var file_name = Request.Cookies["file_name"].Value;
                UserInfor u_info = GetUserInfor();
                
                string dir_path_download = AppConfig.dir_path_download;
                if (dir_path_download != "")
                {
                    file_name = dir_path_download + file_name;
                }
                else
                {
                    file_name = Server.MapPath("~/" + file_name + "");
                }
                //string path = Server.MapPath(import.CFILE);
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(file_name);
                Worksheet sheet = workbook.Worksheets[0];
                DataTable db = sheet.ExportDataTable();

                KNTC_DON_IMPORT donImport = new KNTC_DON_IMPORT();
                donImport.CGHICHU = func.RemoveTagInput(ghichu);
                donImport.IUSER = u_info.tk_action.iUser;
                donImport.DDATE = DateTime.Now;
                donImport.CFILE = file_name;
                donImport.ISODON = db.Rows.Count;
                donImport = kntc.AddKntcDonImport(donImport);

                List<KNTC_DON> listKntcDonImport = new List<KNTC_DON>();

                if (db.Rows.Count > 0)
                {
                    string[] format = { "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yyyy", "d/M/yyyy h:mm:ss tt", "d/M/yyyy h:mm tt", "dd/MM/yyyy hh:mm:ss", "d/M/yyyy h:mm:ss", "d/M/yyyy hh:mm tt", "d/M/yyyy hh tt", "d/M/yyyy h:mm", "d/M/yyyy h:mm", "dd/MM/yyyy hh:mm", "dd/M/yyyy hh:mm" };

                    for (int i = 0; i < db.Rows.Count; i++)
                    {
                        DataRow dr = db.Rows[i];
                        KNTC_DON record = new KNTC_DON();
                        
                        record.CNGUOIGUI_TEN = dr[1].ToString();
                        record.IDIAPHUONG_0 = AppConfig.IDIAPHUONG;
                        record.IDIAPHUONG_1 = Convert.ToInt32(dr[2]);
                        record.IDIAPHUONG_2 = Convert.ToInt32(dr[3]);
                        record.CNGUOIGUI_DIACHI = dr[4].ToString();
                        record.CNGUOIGUI_CMND = dr[5].ToString();
                        record.CNOIDUNG = dr[6].ToString();
                        DateTime dateTime;
                        DateTime.TryParseExact(dr[7].ToString(), format, new CultureInfo("en-US"), DateTimeStyles.None, out dateTime);
                        record.DNGAYNHAN = DateTime.ParseExact(dateTime.ToString("MM/dd/yyyy hh:mm:ss tt"), "MM/dd/yyyy hh:mm:ss tt", null);
                        record.ILOAIDON = Convert.ToInt32(dr[8]);

                        record.IDONTRUNG = 0;
                        record.ITINHCHAT = 0;
                        record.INOIDUNG = 0;
                        record.ITHAMQUYEN = 0;
                        record.ITHULY = 0;
                        record.IDONVITHULY = u_info.user_login.IDONVI;
                        record.ITINHTRANGXULY = 1;
                        record.IDOANDONGNGUOI = 0;
                        record.CDIACHI_VUVIEC = null;
                        record.CFILE = null;
                        record.ITINHTRANG_NOIBO = 0;
                        record.DDATE = DateTime.Now;
                        record.IUSER = u_info.user_login.IUSER;
                        record.IUSER_GIAOXULY = 0;
                        record.CMADON = null;
                        record.INGUOIGUI_QUOCTICH = 233; //Id QuocTich VietNam
                        record.INGUOIGUI_DANTOC = 1;  //Id DanToc Kinh
                        record.IUSER_DUOCGIAOXULY = 0;
                        record.IDUDIEUKIEN = -1;
                        record.IDUDIEUKIEN_KETQUA = 0;
                        record.ILUUTHEODOI = 0;
                        record.CLUUTHEODOI_LYDO = null;
                        record.ISOLUONGTRUNG = 0;
                        record.IDOMAT = 1;
                        record.IDOKHAN = 1;
                        record.CGHICHU = null;
                        record.IDONVITIEPNHAN = u_info.user_login.IDONVI;
                        record.IDANHGIA = 0;
                        record.CGHICHUDANHGIA = null;
                        record.IDELETE = 0;
                        record.IIDIMPORT = donImport.ID;
                        listKntcDonImport.Add(record);

                        
                    }

                }
                if (kntc.InsertListKntcDon(listKntcDonImport))
                {
                    result = true;
                    donImport.ITINHTRANG = 1;
                    kntc.UpdateKntcDonImport(donImport);
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thêm kiến nghị từ danh sách import");
                throw;
            }
            return result;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Check_import_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("file_kntc_check", "false");
                bool filecheck = false;
                //if (!CheckTokenAction("Import_add")) { Response.Redirect("/Home/Error/"); return null; }
                file = Request.Files[0];
               
                string file_name = "";
                if (file != null && file.ContentLength > 0)
                {
                    if (!CheckFile_Upload(file))
                    {
                        Response.Redirect("/Home/Error/?type=type"); return null;
                    }
                    else
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            file_name = UploadFile(file);
                            func.SetCookies("file_name", file_name);
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KNTC_DON_IMPORT donImport = new KNTC_DON_IMPORT();
                donImport.CGHICHU = func.RemoveTagInput(fc["CGHICHU"]);
                donImport.IUSER = u_info.tk_action.iUser;
                donImport.DDATE = DateTime.Now;
                donImport.CFILE = file_name;

                string result = "";
                string str = "";
                if (donImport != null)
                {
                    Tracking(u_info.tk_action.iUser, "Import khiếu nại tố cáo: " + donImport.CGHICHU);
                    var condition = new Dictionary<string, object>();
                    condition.Add("IHIENTHI", 1);
                    condition.Add("IDELETE", 0);
                    List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(condition);
                    var listLoaiDon = kntc.List_LoaiDon().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();

                    string dir_path_download = AppConfig.dir_path_download;
                    string file_path = "";
                    if (dir_path_download != "")
                    {
                        file_path = dir_path_download + donImport.CFILE;
                    }
                    else
                    {
                        file_path = Server.MapPath("~/" + donImport.CFILE + "");
                    }

                    Workbook workbook = new Workbook();
                    workbook.LoadFromFile(file_path);
                    Worksheet sheet = workbook.Worksheets[0];
                    DataTable db = sheet.ExportDataTable();

                    bool check = true;
                    if (db.Columns.Count < 9)
                    {
                        ViewData["error"] = true;
                        filecheck = false;
                        return PartialView("../Ajax/kntc/Import_add_view_file");
                    }
                    if (db.Rows.Count > 0)
                    {
                        string[] format = { "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yyyy", "d/M/yyyy h:mm:ss tt", "d/M/yyyy h:mm tt", "dd/MM/yyyy hh:mm:ss", "d/M/yyyy h:mm:ss", "d/M/yyyy hh:mm tt", "d/M/yyyy hh tt", "d/M/yyyy h:mm", "d/M/yyyy h:mm", "dd/MM/yyyy hh:mm", "dd/M/yyyy hh:mm" };
                        filecheck = true;
                        int id_import = (int)donImport.ID;

                        for (int i = 0; i < db.Rows.Count; i++)
                        {
                            result = "";
                            DataRow dr = db.Rows[i];
                            check = true;

                            //check Họ và tên công dân
                            if (dr[1].ToString() == "")
                            {
                                check = false;
                                result = result + " họ và tên công dân,";
                            }

                            //check quận/huyện
                            if (dr[2].ToString() != "")
                            {
                                var huyen = lstDiaphuong.Where(x => x.IDIAPHUONG.ToString() == dr[2].ToString());
                                if (huyen.Count() == 0 || huyen.FirstOrDefault().IPARENT != AppConfig.IDIAPHUONG)
                                {
                                    check = false;
                                    result = result + " quận/huyện,";
                                }
                            }
                            else
                            {
                                check = false;
                                result = result + " quận/huyện,";
                            }

                            //check  xã/phường/thị trấn
                            if (dr[3].ToString() != "")
                            {
                                var huyen = lstDiaphuong.Where(x => x.IDIAPHUONG.ToString() == dr[3].ToString());
                                if (huyen.Count() == 0 || huyen.FirstOrDefault().IPARENT.ToString() != dr[2].ToString())
                                {
                                    check = false;
                                    result = result + " xã/phường/thị trấn,";
                                }
                            }
                            else
                            {
                                check = false;
                                result = result + " xã/phường/thị trấn,";
                            }

                            //check Địa chỉ cụ thể
                            if (dr[4].ToString() == "")
                            {
                                check = false;
                                result = result + " địa chỉ cụ thể,";
                            }

                            

                            //check noi dung
                            if (dr[6].ToString() == "")
                            {
                                check = false;
                                result = result + " nội dung đơn,";
                            }

                            //check Ngày nhận đơn
                            if (dr[7].ToString() != "")
                            {
                                
                                DateTime dateTime;
                                if (!DateTime.TryParseExact(dr[7].ToString(), format, new CultureInfo("en-US"), DateTimeStyles.None, out dateTime))
                                {
                                    check = false;
                                    result = result + " ngày nhận đơn,";
                                }
                            }
                            else
                            {
                                check = false;
                                result = result + " ngày nhận đơn,";
                            }
                            //check Loại đơn

                            if (dr[8].ToString() != "")
                            {
                                if (listLoaiDon.Where(x => x.ILOAIDON.ToString() == dr[8].ToString()).Count() == 0)
                                {
                                    check = false;
                                    result = result + " loại đơn,";
                                }
                            }
                            else
                            {
                                check = false;
                                result = result + " loại đơn,";
                            }

                            int r = i + 2;
                            if (check == false)
                            {
                                str = str + "<tr><td width = '5%' class='tcenter'><i class='icon-remove' style='color: red'></i></td>";
                            }
                            else
                            {
                                str = str + "<tr><td width = '5%' class='tcenter'><i class='icon-ok' style='color: green'></i></td>";
                            }

                            str = str + "<td width = '5%' class='tcenter' >" + dr[0] + "</td>";
                            str = str + "<td width = '5%' class='tcenter' >" + dr[1] + "</td>";
                            str = str + "<td class='tcenter' >" + dr[2] + "</td>";
                            str = str + "<td class='tcenter' > " + dr[3] + "</td>";
                            str = str + "<td class='tcenter'>" + dr[4] + "</td>";
                            str = str + "<td class='tcenter' >" + dr[5] + "</td>";
                            str = str + "<td width = '25%' > " + dr[6] + "</td>";
                            str = str + "<td class='tcenter' > " + dr[7] + "</td>";
                            str = str + "<td class='tcenter' > " + dr[8] + "</td>";
                            if (check == false)
                            {
                                str = str + "<td width = '5%' class='tcenter' onclick='showAlert(" + "`" + result.Remove(result.Length - 1, 1) + "`" + ")' ><i class='icon-eye-open'></i></td></tr>";
                                filecheck = false;
                            }
                            else
                            {
                                str = str + "<td width = '5%' class='tcenter'></td></tr>";
                            }
                        }
                    }
                }
                ViewData["list"] = str;
                ViewData["error"] = false;
                if (filecheck) func.SetCookies("file_kntc_check", "true");
                else func.SetCookies("file_kntc_check", "false");
                return PartialView("../Ajax/kntc/Import_add_view_file");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form import kiến nghị");
                func.SetCookies("file_kntc_check", "false");
                throw;
            }
            //int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
        }

        public string Get_status_check_file_import()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                return Request.Cookies["file_kntc_check"].Value;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex);
                throw;
            }

        }

        private ImportResult ImportKntcDon(KNTC_DON_IMPORT donImport, UserInfor userInfor)
        {
            try
            {
                string serverPathUpload = AppConfig.dir_path_upload;
                string filePath = serverPathUpload + donImport.CFILE;
                ImportResult importResult = new ImportResult();
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(filePath);
                Worksheet sheet = workbook.Worksheets[0];
                DataTable db = sheet.ExportDataTable();

                int rowFail = 0;
                List<KNTC_DON> listKntcDonImport = new List<KNTC_DON>();
                if (db.Rows.Count > 0)
                {
                    Dictionary<string, object> diaPhuongParam = new Dictionary<string, object>();
                    diaPhuongParam.Add("IDELETE", 0);
                    diaPhuongParam.Add("IHIENTHI", 1);
                    var listDiaPhuong = kntc.GetAll<DIAPHUONG>(diaPhuongParam);
                    var listTinh = listDiaPhuong.Where(x => x.IPARENT == 0);

                    Dictionary<string, object> nguonDonParam = new Dictionary<string, object>();
                    nguonDonParam.Add("IDELETE", 0);
                    nguonDonParam.Add("IHIENTHI", 1);
                    var listNguonDon = kntc.GetAll<KNTC_NGUONDON>(nguonDonParam);

                    Dictionary<string, object> loaiDonParam = new Dictionary<string, object>();
                    loaiDonParam.Add("IDELETE", 0);
                    loaiDonParam.Add("IHIENTHI", 1);
                    var listLoaiDon = kntc.GetAll<KNTC_LOAIDON>(loaiDonParam);

                    Dictionary<string, object> linhVucParam = new Dictionary<string, object>();
                    linhVucParam.Add("IDELETE", 0);
                    linhVucParam.Add("IHIENTHI", 1);
                    var listLinhVuc = kntc.GetAll<LINHVUC>(linhVucParam);

                    var rowHeader = db.Columns;
                    for (int i=0; i< db.Rows.Count; i++)
                    {
                        StringBuilder columnFails = new StringBuilder();
                        KNTC_DON record = new KNTC_DON();
                        DataRow dr = db.Rows[i];
                        if (string.IsNullOrEmpty(dr[1].ToString()))
                        {
                            columnFails.Append(rowHeader[1].ToString() + "|");
                        }
                        else
                        {
                            record.CNGUOIGUI_TEN = dr[1].ToString();
                        }

                        if (string.IsNullOrEmpty(dr[2].ToString()))
                        {
                            columnFails.Append(rowHeader[2].ToString() + "|");
                        }
                        else
                        {
                            var tinh = listTinh?.FirstOrDefault(x => dr[2].ToString().Equals(x.CCODE, StringComparison.OrdinalIgnoreCase));
                            if(tinh == null)
                            {
                                columnFails.Append(rowHeader[2].ToString() + "|");
                            }
                            else
                            {
                                record.IDIAPHUONG_0 = tinh.IDIAPHUONG;
                            }
                        }

                        if (string.IsNullOrEmpty(dr[3].ToString()))
                        {
                            columnFails.Append(rowHeader[3].ToString() + "|");
                        }
                        else
                        {
                            var huyen = listDiaPhuong?.Where(x => record.IDIAPHUONG_0 != null && x.IPARENT == record.IDIAPHUONG_0)?.FirstOrDefault(x => dr[3].ToString().Equals(x.IDIAPHUONG.ToString(), StringComparison.OrdinalIgnoreCase));
                            if (huyen == null)
                            {
                                columnFails.Append(rowHeader[3].ToString() + "|");
                            }
                            else
                            {
                                record.IDIAPHUONG_1 = huyen.IDIAPHUONG;
                            }
                        }

                        if (string.IsNullOrEmpty(dr[4].ToString()))
                        {
                            columnFails.Append(rowHeader[4].ToString() + "|");
                        }
                        else
                        {
                            record.CNGUOIGUI_DIACHI = dr[4].ToString();
                        }

                        if (string.IsNullOrEmpty(dr[5].ToString()))
                        {
                            columnFails.Append(rowHeader[5].ToString() + "|");
                        }
                        else
                        {
                            if (!dr[5].ToString().All(char.IsDigit))
                            {
                                columnFails.Append(rowHeader[5].ToString() + "|");
                            }
                            else
                            {
                                record.CNGUOIGUI_CMND = dr[5].ToString();
                            }
                        }

                        if (string.IsNullOrEmpty(dr[6].ToString()))
                        {
                            columnFails.Append(rowHeader[6].ToString() + "|");
                        }
                        else
                        {
                            record.CNOIDUNG = dr[6].ToString();
                        }

                        if (string.IsNullOrEmpty(dr[7].ToString()))
                        {
                            columnFails.Append(rowHeader[7].ToString() + "|");
                        }
                        else
                        {
                            DateTime dNgayNhanDon;
                            if (DateTime.TryParse(dr[7].ToString(), out dNgayNhanDon))
                            {
                                record.DNGAYNHAN = dNgayNhanDon;
                            }
                            else
                            {
                                columnFails.Append(rowHeader[7].ToString() + "|");
                            }
                            
                        }

                        if (string.IsNullOrEmpty(dr[8].ToString()))
                        {
                            columnFails.Append(rowHeader[8].ToString() + "|");
                        }
                        else
                        {
                            var nguonDon = listNguonDon.FirstOrDefault(x => dr[8].ToString().Equals(x.CCODE, StringComparison.OrdinalIgnoreCase));
                            if (nguonDon == null)
                            {
                                columnFails.Append(rowHeader[8].ToString() + "|");
                            }
                            else
                            {
                                record.INGUONDON = nguonDon.INGUONDON;
                            }
                        }

                        if (string.IsNullOrEmpty(dr[9].ToString()))
                        { 
                            var loaiDon = listLoaiDon.FirstOrDefault(x => dr[9].ToString().Equals(x.CCODE, StringComparison.OrdinalIgnoreCase));
                            if (loaiDon != null)
                            {
                                record.ILOAIDON = loaiDon.ILOAIDON;
                            }
                        }


                        if (!string.IsNullOrEmpty(dr[10].ToString()))
                        {
                            var linhvuc = listLinhVuc.FirstOrDefault(x => dr[10].ToString().Equals(x.CCODE, StringComparison.OrdinalIgnoreCase));
                            if (linhvuc != null) {
                                record.ILINHVUC = linhvuc.ILINHVUC;
                            }
                        }
                            

                        if (columnFails.Length == 0)
                        {
                            record.IDONTRUNG = 0;
                            record.ITINHCHAT = 0;
                            record.INOIDUNG = 0;
                            record.ITHAMQUYEN = 0;
                            record.ITHULY = 0;
                            record.IDONVITHULY = userInfor.user_login.IDONVI;
                            record.ITINHTRANGXULY = 0;
                            record.IDIAPHUONG_2 = 0;
                            record.IDOANDONGNGUOI = 0;
                            record.CDIACHI_VUVIEC = null;
                            record.CFILE = null;
                            record.ITINHTRANG_NOIBO = 0;
                            record.DDATE = DateTime.Now;
                            record.IUSER = userInfor.user_login.IUSER;
                            record.IUSER_GIAOXULY = 0;
                            record.CMADON = null;
                            record.INGUOIGUI_QUOCTICH = 233; //Id QuocTich VietNam
                            record.INGUOIGUI_DANTOC = 1;  //Id DanToc Kinh
                            record.IUSER_DUOCGIAOXULY = 0;
                            record.IDUDIEUKIEN = -1;
                            record.IDUDIEUKIEN_KETQUA = 0;
                            record.ILUUTHEODOI = 0;
                            record.CLUUTHEODOI_LYDO = null;
                            record.ISOLUONGTRUNG = 0;
                            record.IDOMAT = 1;
                            record.IDOKHAN = 1;
                            record.CGHICHU = null;
                            record.IDONVITIEPNHAN = userInfor.user_login.IDONVI;
                            record.IDANHGIA = 0;
                            record.CGHICHUDANHGIA = null;
                            record.IDELETE = 0;
                            record.IIDIMPORT = donImport.ID;

                            listKntcDonImport.Add(record);
                        }
                        else
                        {
                            importResult.ListRecordFailIndex.Add(i, columnFails.ToString());
                        }
                    }
                }

                if (importResult.ListRecordFailIndex.Count == 0 && listKntcDonImport.Count > 0)
                {
                    if (kntc.InsertListKntcDon(listKntcDonImport))
                    {
                        importResult.Status = true;
                        importResult.Message = "Import Success";
                    }
                    else
                    {
                        importResult.Status = false;
                        importResult.Message = "Insert to DB Fail";
                    }
                }
                else
                {
                    importResult.Status = false; 
                    importResult.Message = "Data Import Invalid";
                }
                importResult.TotalImportFail = rowFail;
                importResult.TotalRecord = db.Rows.Count;
                return importResult;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "KntcBussineess.ImportKntcDon Has Error");
                return null;
            }
        }
        public ActionResult Import_don()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id_import = Convert.ToInt32(HashUtil.Decode_ID(Request["id"].ToString()));
                if (id_import != null && id_import > 0)
                {
                    var listImportDon = kntc.PRC_KNTC_IMPORT_LISTDON(id_import);
                    if (listImportDon != null && listImportDon.Count > 0)
                    {
                        ViewData["list"] = kn.List_Import_Kntc_Don(listImportDon, Request.Cookies["url_key"].Value);
                    }
                }
                return View("../Kntc/Import_don");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách import kiến nghị");
                return View("../Home/Error_Exception");
            }
        }

        public string Get_Option_CoQuan_TheoNhom(int DonViXuLy = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            string str = "";
            //Trung ương
            str = str + "<option value ='' class ='group-result'> Trung ương </option> ";
            List<QUOCHOI_COQUAN> lstTrunguong;
            lstTrunguong = coquan.Where(x => x.CTYPE == "TW").ToList();
            string select = " ";
            foreach (var item in lstTrunguong)
            {
                if (item.ICOQUAN == DonViXuLy) select = " selected "; else select = "  ";
                str = str + "<option " + select + " value ='" + item.ICOQUAN + "'>" + "- - -" + item.CTEN + "</option>";
            }
            //Tỉnh
            str = str + "<option value ='' class ='group-result'> Tỉnh </option> ";
            List<QUOCHOI_COQUAN> lstTinh;
            lstTinh = coquan.Where(x => x.CTYPE == "TINH").ToList();
            foreach (var item in lstTinh)
            {
                if (item.ICOQUAN == DonViXuLy) select = " selected "; else select = "  ";
                str = str + "<option " + select + "  value ='" + item.ICOQUAN + "'>" + "- - -" + item.CTEN + "</option>";
            }
            //Huyện
            str = str + "<option value ='' class ='group-result'> Huyện </option> ";
            List<QUOCHOI_COQUAN> lstHuyen;
            lstHuyen = coquan.Where(x => x.CTYPE == "HUYEN").ToList();
            foreach (var item in lstHuyen)
            {
                if (item.ICOQUAN == DonViXuLy) select = " selected "; else select = "  ";
                str = str + "<option " + select + "  value ='" + item.ICOQUAN + "'>" + "- - -" + item.CTEN + "</option>";
            }

            return str;
        }
        public string Get_Option_DoiTuong_By_ID_USERS()
        {
            string str = "";
            //var linhvuc = _kiennghi.GetAll_LinhVuc_CoQuan_By_IDCoQuan();
            //var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            UserInfor u_info = GetUserInfor();
            if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
            {
                str += "<option value='1' selected>Hội đồng nhân dân tỉnh </option>";
            }
            else if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
            {
                str += "<option value='0' selected>Đoàn đại biểu Quốc hội tỉnh </option>";
            }
            else
            {
                str += "<option value='-1' selected>Tất cả </option>";
                str += "<option value='1'>Hội đồng nhân dân tỉnh </option>";
                str += "<option value='0'>Đoàn đại biểu Quốc hội tỉnh </option>";
            }
            return str;
        }
        
        public string Get_Option_BaoCao()
        {
            string str = "";
            //var linhvuc = _kiennghi.GetAll_LinhVuc_CoQuan_By_IDCoQuan();
            //var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            str += "<option value='1' selected>BÁO CÁO 3B: Danh sách chuyển đơn theo tháng</option>";
            str += "<option value='2'>BÁO CÁO 3C: Danh sách chuyển đơn theo khoảng thời gian đã chọn</option>";
            str += "<option value='3'>BÁO CÁO 3D: Danh sách chuyển đơn theo lĩnh vực</option>";
            str += "<option value='5'>BÁO CÁO 3F: Danh sách công văn đôn đốc</option>";
            return str;
        }

        public string Get_Year_List(Boolean currentYearEnable)
        {
            string str = "";
            const int numberOfYears = 51;
            var startYear = DateTime.Now.Year;
            var endYear = startYear + numberOfYears;
            if (currentYearEnable == false)
            {
                str += "<option value='0' selected>Hãy chọn năm</option>";
            }
            else
            {
                str += "<option value='0'>Hãy chọn năm</option>";
            }
            for (var year = startYear; year < endYear ; year++)
            {
                if (currentYearEnable == true)
                {
                    if (year == startYear)
                    {
                        str = str + "<option value ='" + year + "'selected>" + year + "</option>";
                    }
                    else
                    {
                        str = str + "<option value ='" + year + "'>" + year + "</option>";
                    }
                }
                else
                {
                    str = str + "<option value ='" + year + "'>" + year + "</option>";
                }
            }

            return str;
        }
        
        public string Get_Month_List()
        {
            string str = "";
            const int numberOfMonth = 12;
            str += "<option value='0' selected>Hãy chọn tháng</option>";
            for (var month = 1; month <= numberOfMonth ; month++)
            {
                str = str + "<option value ='" + month + "'>" + month + "</option>";
            }

            return str;
        }

        public string Get_Option_Don_GuiDen()
        {
            string str = "";
            UserInfor u_info = GetUserInfor();
            if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
            {
                str += "<span class='span4'><label><input class='nomargin' type='radio' disabled name='iDoiTuongGui' value='0'/>Đoàn ĐBQH Tỉnh</label> </span>";
                str += "<span class='span4'><label><input class='nomargin' type='radio' disabled name='iDoiTuongGui' value='1' checked/>Hội đồng Nhân dân Tỉnh</label> </span>";
            }
            else if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
            {
                str += "<span class='span4'><label><input class='nomargin' type='radio' disabled name='iDoiTuongGui' value='0' checked/>Đoàn ĐBQH Tỉnh</label> </span>";
                str += "<span class='span4'><label><input class='nomargin' type='radio' disabled name='iDoiTuongGui' value='1'/>Hội đồng Nhân dân Tỉnh</label> </span>";
            }
            else
            {
                str += "<span class='span4'><label><input class='nomargin' type='radio' onclick='ChangeKhoaTheoLoai(this.value)' name='iDoiTuongGui' value='0' checked/>Đoàn ĐBQH Tỉnh</label> </span>";
                str += "<span class='span4'><label><input class='nomargin' type='radio' onclick='ChangeKhoaTheoLoai(this.value)' name='iDoiTuongGui' value='1'/>Hội đồng Nhân dân Tỉnh</label> </span>";
            }
            return str;
        }
                
        public ActionResult Ajax_xuat_kntc_chuatraloi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                ViewData["opt-loaidon"] = kn.Option_LoaiDon(0);
                ViewData["opt-doituong"] = Get_Option_DoiTuong_By_ID_USERS();
                ViewData["opt-tenbaocao"] = Get_Option_BaoCao();
                ViewData["opt-yearlist"] = Get_Year_List(false);
                ViewData["opt-monthlist"] = Get_Month_List();
                
                return View("../Ajax/Kntc/xuat_kntc_chuatraloi");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách import kiến nghị");
                return View("../Home/Error_Exception");
            }
        }

        public ActionResult Ajax_Change_LoaiDonTheoLoaiBaoCao()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                var loaidon = kn.Option_LoaiDon(0);
                Response.Write(loaidon);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax thay đổi loai don theo ten bao cao.");
                return null;
            }

        }
        
        public ActionResult Ajax_Set_NamHienTai()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                var yearList = Get_Year_List(true);
                Response.Write(yearList);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax thay đổi loai don theo ten bao cao.");
                return null;
            }

        }

        public ActionResult BaoCaoMoiTonghop_ChuaTraLoi(string ext = "pdf")
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

                int iLoaiDon = 0;
                int iYear = 0;
                int iMonth = 0;
                int iNgayDen = 0;
                int iDoiTuong = -1;
                int iTenBaoCao = 0;
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["iYear"] != null && Request["iYear"] != "")
                {
                    iYear = Convert.ToInt32(Request["iYear"]);
                }
                if (Request["iMonth"] != null && Request["iMonth"] != "")
                {
                    iMonth = Convert.ToInt32(Request["iMonth"]);
                }
                if (Request["dBatDau"] != null && Request["dBatDau"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["dBatDau"]));
                }
                if (Request["dKetThuc"] != null && Request["dKetThuc"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["dKetThuc"]));
                }
                if (Request["iDoiTuong"] != null)
                {
                    iDoiTuong = Convert.ToInt32(Request["iDoiTuong"].ToString());
                }
                if (Request["iLoaiDon"] != null)
                {
                    iLoaiDon = Convert.ToInt32(Request["iLoaiDon"].ToString());
                }
                if (Request["iTenBaoCao"] != null)
                {
                    iTenBaoCao = Convert.ToInt32(Request["iTenBaoCao"].ToString());
                }
                var templatePath = "";
                string fileName = "";
                UserInfor u_info = GetUserInfor();
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                
                if (iDoiTuong == 0)
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KNTC_QH_Baocao_ChuaTraLoi);
                    fileName = string.Format("{0}.{1}", "BaoCao_ChuaTraLoi_QH", ext);
                }
                else if (iDoiTuong == 1)
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KNTC_HDND_Baocao_ChuaTraLoi);
                    fileName = string.Format("{0}.{1}", "BaoCao_ChuaTraLoi_HDND", ext);
                }
                else 
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KNTC_QH_HDND_Baocao_ChuaTraLoi);
                    fileName = string.Format("{0}.{1}", "BaoCao_ChuaTraLoi_QH_HDND", ext);
                }
                ExcelFile xls = ExportReportTonghop_ChuaTraLoi(dtungay, ddenngay, iYear, iMonth, iDoiTuong, iLoaiDon, iUser, templatePath);
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }
        }
        
        public ActionResult BaoCaoDanhSachChuyenDonTheoLinhVuc_3D(string ext = "pdf")
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

                int iLoaiDon = 0;
                int iYear = 0;
                int iMonth = 0;
                int iNgayDen = 0;
                int iDoiTuong = -1;
                int iTenBaoCao = 0;
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["iYear"] != null && Request["iYear"] != "")
                {
                    iYear = Convert.ToInt32(Request["iYear"]);
                }
                if (Request["iMonth"] != null && Request["iMonth"] != "")
                {
                    iMonth = Convert.ToInt32(Request["iMonth"]);
                }
                if (Request["dBatDau"] != null && Request["dBatDau"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["dBatDau"]));
                }
                if (Request["dKetThuc"] != null && Request["dKetThuc"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["dKetThuc"]));
                }
                if (Request["iDoiTuong"] != null)
                {
                    iDoiTuong = Convert.ToInt32(Request["iDoiTuong"].ToString());
                }
                if (Request["iLoaiDon"] != null)
                {
                    iLoaiDon = Convert.ToInt32(Request["iLoaiDon"].ToString());
                }
                if (Request["iTenBaoCao"] != null)
                {
                    iTenBaoCao = Convert.ToInt32(Request["iTenBaoCao"].ToString());
                }
                var templatePath = "";
                string fileName = "";
                UserInfor u_info = GetUserInfor();
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                
                if (iDoiTuong == 0)
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KNTC_QH_Baocao_ChuaTraLoi_3D);
                    fileName = string.Format("{0}.{1}", "BaoCao_ChuaTraLoi_QH", ext);
                }
                else if (iDoiTuong == 1)
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KNTC_HDND_Baocao_ChuaTraLoi_3D);
                    fileName = string.Format("{0}.{1}", "BaoCao_ChuaTraLoi_HDND", ext);
                }
                else 
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KNTC_TongHop_Baocao_ChuaTraLoi_3D);
                    fileName = string.Format("{0}.{1}", "BaoCao_ChuaTraLoi_QH_HDND", ext);
                }
                ExcelFile xls = ExportReportTonghop_ChuaTraLoi_3D(dtungay, ddenngay, iYear, iMonth, iDoiTuong, iLoaiDon, iUser, templatePath);
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }
        }

        public ExcelFile ExportReportTonghop_ChuaTraLoi(DateTime? dtungay, DateTime? ddenngay, int iYear, int iMonth, int iDoiTuong, int iLoaiDon, int iUser, string templatePath)
        {
            List<KNTC_REPORT_CHUATRALOI> listReport = new List<KNTC_REPORT_CHUATRALOI>();
            if (iDoiTuong == 0)
            {
                listReport = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi("RPT_KNTC_BAOCAO_CHUACOTRALOI_3B_3C", dtungay, ddenngay, iYear, iMonth, 0, iLoaiDon, iUser);
            }
            else if (iDoiTuong == 1)
            {
                listReport = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi("RPT_KNTC_BAOCAO_CHUACOTRALOI_3B_3C", dtungay, ddenngay, iYear, iMonth, 1, iLoaiDon, iUser);
            }
            else
            {
                var listdata1 = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi("RPT_KNTC_BAOCAO_CHUACOTRALOI_3B_3C", dtungay, ddenngay, iYear, iMonth, 1, iLoaiDon, iUser);
                if (listdata1.Count > 0)
                {
                    listReport.Add(new KNTC_REPORT_CHUATRALOI { STT = "A. ĐOÀN ĐẠI BIỂU QUỐC HỘI", DNGAYBANNHANH = null, CSOVANBAN = "", CNGUOIGUITEN = "", CNOIDUNG = "", CLOAIDON = "", ISMERGE = true, ISTITLE = 1, ISBOLD = 1 });
                    int count1 = 0;
                    foreach (var kntc_item in listdata1)
                    {
                        count1++;
                        listReport.Add(new KNTC_REPORT_CHUATRALOI { STT = count1.ToString(), DNGAYBANNHANH = kntc_item.DNGAYBANNHANH, CSOVANBAN = kntc_item.CSOVANBAN, CNGUOIGUITEN = kntc_item.CNGUOIGUITEN, CNOIDUNG = kntc_item.CNOIDUNG, CLOAIDON = kntc_item.CLOAIDON, ISBOLD = 0, ISMERGE = false });
                    }
                }

                var listdata2 = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi("RPT_KNTC_BAOCAO_CHUACOTRALOI_3B_3C", dtungay, ddenngay, iYear, iMonth, 0, iLoaiDon, iUser);
                if (listdata2.Count > 0)
                {
                    listReport.Add(new KNTC_REPORT_CHUATRALOI { STT = "B. THƯỜNG TRỰC HĐND TỈNH", DNGAYBANNHANH = null, CSOVANBAN = "", CNGUOIGUITEN = "", CNOIDUNG = "", CLOAIDON = "", ISMERGE = true, ISTITLE = 1, ISBOLD = 1 });
                    int count2 = 0;
                    foreach (var kntc_item in listdata2)
                    {
                        count2++;
                        listReport.Add(new KNTC_REPORT_CHUATRALOI { STT = count2.ToString(), DNGAYBANNHANH = kntc_item.DNGAYBANNHANH, CSOVANBAN = kntc_item.CSOVANBAN, CNGUOIGUITEN = kntc_item.CNGUOIGUITEN, CNOIDUNG = kntc_item.CNOIDUNG, CLOAIDON = kntc_item.CLOAIDON, ISBOLD = 0, ISMERGE = false });
                    }
                }
                    
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(templatePath);
            TXlsNamedRange Range;

            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 2, 32, "='Print_Titles'!$8:$9");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
            Result.SetNamedRange(Range);

            FlexCelReport fr = new FlexCelReport();
            /*if (iTenBaoCao == 1 && listReport.Count > 0)
            {

                decimal check = 1;
                foreach (var item in listReport)
                {
                    if (item.IDOITUONGGUI != check)
                    {
                        if (item.IDOITUONGGUI == 0)
                        {
                            item.FIRSTTITLE = "ĐOÀN ĐẠI BIỂU QUỐC HỘI";
                        }
                        if (item.IDOITUONGGUI == 1)
                        {
                            item.FIRSTTITLE = "HỘI ĐỒNG NHÂN DÂN TỈNH";
                        }
                        check = item.IDOITUONGGUI;
                    }
                }
            }*/
            fr.AddTable<KNTC_REPORT_CHUATRALOI>("List", listReport);
            var yearVal = "...";
            var monthVal = "...";
            if (iYear != 0)
            {
                yearVal = iYear.ToString();
            }

            if (iMonth != 0)
            {
                monthVal = iMonth.ToString();
            }
            fr.SetValue(new
            {
                year = yearVal,
                month = monthVal
            });
            fr.UseForm(this).Run(Result);
            //merge cell
            int rowStart = 9;
            int colStart = 1;
            int colLast = 7;
            int index = 0;
            
            foreach (var item in listReport)
            {
                if (item.ISMERGE)
                {
                    if (item.ISTITLE == 1)
                    {
                        Result.MergeCells(rowStart + index, 1, rowStart + index, colLast);
                    }
                }
                index++;
            }
            return Result;
        }
        
        public ExcelFile ExportReportTonghop_ChuaTraLoi_3D(DateTime? dtungay, DateTime? ddenngay, int iYear, int iMonth, int iDoiTuong, int iLoaiDon, int iUser, string templatePath)
        {
            List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> listFinal = new List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D>();
            List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> listReport = new List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D>();
            List<LINHVUC> lstLinhvuc = _thietlap.Get_Linhvuc();

            if (iDoiTuong == 0)
            {
                listReport = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi3D("RPT_KNTC_BAOCAO_CHUACOTRALOI_THEOLINHVUC_3D", dtungay, ddenngay, iYear, iMonth, 0, iLoaiDon, iUser);
                getKTNCTheoLinhVuc(listFinal, listReport, lstLinhvuc);
            }
            else if (iDoiTuong == 1)
            {
                listReport = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi3D("RPT_KNTC_BAOCAO_CHUACOTRALOI_THEOLINHVUC_3D", dtungay, ddenngay, iYear, iMonth, 1, iLoaiDon, iUser);
                getKTNCTheoLinhVuc(listFinal, listReport, lstLinhvuc);
            }
            else
            {
                List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> listdata1 = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi3D("RPT_KNTC_BAOCAO_CHUACOTRALOI_THEOLINHVUC_3D", dtungay, ddenngay, iYear, iMonth, 1, iLoaiDon, iUser);
                if (listdata1.Count > 0)
                {
                    listFinal.Add(new KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D { STT = "A. ĐOÀN ĐẠI BIỂU QUỐC HỘI", DNGAYBANNHANH = null, CSOVANBAN = "", CNGUOIGUITEN = "", CNOIDUNG = "", CLOAIDON = "", ISMERGE = true, ISTITLE = 1, ISBOLD = 1 });
                }
                int count1 = 0;
                foreach (var kntc_item in listdata1)
                {
                    count1++;
                    listReport.Add(new KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D { STT = count1.ToString(), DNGAYBANNHANH = kntc_item.DNGAYBANNHANH, CSOVANBAN = kntc_item.CSOVANBAN, CNGUOIGUITEN = kntc_item.CNGUOIGUITEN, CCOQUANNHAN = kntc_item.CCOQUANNHAN, CNOIDUNG = kntc_item.CNOIDUNG, CLOAIDON = kntc_item.CLOAIDON, ISBOLD = 0, ISMERGE = false });
                }
                getKTNCTheoLinhVuc(listFinal, listdata1, lstLinhvuc);

                List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> listdata2 = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi3D("RPT_KNTC_BAOCAO_CHUACOTRALOI_THEOLINHVUC_3D", dtungay, ddenngay, iYear, iMonth, 0, iLoaiDon, iUser);
                if (listdata2.Count > 0)
                {
                    listFinal.Add(new KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D { STT = "B. THƯỜNG TRỰC HĐND TỈNH", DNGAYBANNHANH = null, CSOVANBAN = "", CNGUOIGUITEN = "", CNOIDUNG = "", CLOAIDON = "", ISMERGE = true, ISTITLE = 1, ISBOLD = 1 });
                }

                int count2 = 0;
                foreach (var kntc_item in listdata2)
                {
                    count2++;
                    listReport.Add(new KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D { STT = count2.ToString(), DNGAYBANNHANH = kntc_item.DNGAYBANNHANH, CSOVANBAN = kntc_item.CSOVANBAN, CNGUOIGUITEN = kntc_item.CNGUOIGUITEN, CCOQUANNHAN = kntc_item.CCOQUANNHAN, CNOIDUNG = kntc_item.CNOIDUNG, CLOAIDON = kntc_item.CLOAIDON, ISBOLD = 0, ISMERGE = false });
                }
                getKTNCTheoLinhVuc(listFinal, listdata2, lstLinhvuc);


            }
            XlsFile Result = new XlsFile(true);
            Result.Open(templatePath);
            TXlsNamedRange Range;

            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 2, 32, "='Print_Titles'!$8:$9");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
            Result.SetNamedRange(Range);

            FlexCelReport fr = new FlexCelReport();
            /*if (iTenBaoCao == 1 && listReport.Count > 0)
            {

                decimal check = 1;
                foreach (var item in listReport)
                {
                    if (item.IDOITUONGGUI != check)
                    {
                        if (item.IDOITUONGGUI == 0)
                        {
                            item.FIRSTTITLE = "ĐOÀN ĐẠI BIỂU QUỐC HỘI";
                        }
                        if (item.IDOITUONGGUI == 1)
                        {
                            item.FIRSTTITLE = "HỘI ĐỒNG NHÂN DÂN TỈNH";
                        }
                        check = item.IDOITUONGGUI;
                    }
                }
            }*/
            fr.AddTable<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D>("List", listFinal);

            var yearVal = "...";
            var monthVal = "...";
            if (iYear != 0)
            {
                yearVal = iYear.ToString();
            }

            if (iMonth != 0)
            {
                monthVal = iMonth.ToString();
            }
            fr.SetValue(new
            {
                year = yearVal,
                month = monthVal
            });
            fr.UseForm(this).Run(Result);
            //merge cell
            int rowStart = 9;
            int colStart = 1;
            int colLast = 2;
            int index = 0;
            
            foreach (var item in listFinal)
            {
                if (item.ISMERGE)
                {
                    if (item.ISTITLE == 1)
                    {
                        Result.MergeCells(rowStart + index, colStart, rowStart + index, 7);
                    }
                    else
                    {
                        Result.MergeCells(rowStart + index, colStart, rowStart + index, colLast);
                    }
                }
                index++;
            }
            return Result;
        }
        
        public void getKTNCTheoLinhVuc(List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> lstData, List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> lstKNTC, List<LINHVUC> lstLinhvuc)
        {
            var lstLinhvuc_parent = lstLinhvuc.Where(x => x.IPARENT == 0).ToList();
            int stt = 0;
            foreach (var item in lstLinhvuc_parent)
            {
                var lstLinhvuc_child = lstLinhvuc.Where(x => x.IPARENT == item.ILINHVUC).ToList();
                List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> checkKNTCLinhVuc= lstKNTC.Where(x => x.ILINHVUC == item.ILINHVUC).ToList();
                
                if (checkKNTCLinhVuc.Count > 0 )
                {
                    stt++;
                    lstData.Add(new KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D { STT = NumberExtension.ToRoman(Convert.ToInt16(stt)) + ". "+ item.CTEN, ISBOLD = 1, ISMERGE = true });
                }
                getLinhVuc_KNTC(lstData, Convert.ToInt16(item.ILINHVUC), lstKNTC, lstLinhvuc_child);
            }
        }

        public void getLinhVuc_KNTC(List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> lstData, int iLinhVuc, List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> lstKNTC, List<LINHVUC> lstlinhvuc_child)
        { 
            List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> lst_kntc = lstKNTC.Where(x => x.ILINHVUC == iLinhVuc).ToList();
            int count = 0;
            foreach (var kntc_item in lst_kntc)
            {
                count++;
                lstData.Add(new KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D { STT = count.ToString(), DNGAYBANNHANH = kntc_item.DNGAYBANNHANH, CSOVANBAN = kntc_item.CSOVANBAN, CNGUOIGUITEN = kntc_item.CNGUOIGUITEN, CNOIDUNG = kntc_item.CNOIDUNG, CCOQUANNHAN = kntc_item.CCOQUANNHAN, CLOAIDON = kntc_item.CLOAIDON, ISBOLD = 0, ISMERGE = false });
            }
        }

        public ActionResult BaoCaoDanhSachChuyenDonTheoKhoangTg_3C(int iDoiTuong = -1, string dBatDau = "", string dKetThuc = "", int iLoaiDon = 0, string ext = "pdf")
        {
            try
            {

                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
                int iLoaiBaoCao = 0;

                DateTime? dtungay = null;
                DateTime? ddenngay = null;

                if (dBatDau != null && dBatDau != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(dBatDau));
                }
                if (dKetThuc != null && dKetThuc != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(dKetThuc));
                }

                iLoaiBaoCao = iDoiTuong;


                string fileName = "";
                var templatePath = "";
                if (iLoaiBaoCao == -1)
                {
                    fileName = string.Format("{0}.{1}", "Baocao3C(TongHop)_DanhSachChuyenDonTheoKhoangTg_3C", ext);
                    templatePath = ReportConstants.rpt_KNTC_TongHop_Baocao_ChuaTraLoi_3C;
                }
                if (iLoaiBaoCao == 0)
                {
                    fileName = string.Format("{0}.{1}", "Baocao3C(QH)_DanhSachChuyenDonTheoKhoangTg_3C", ext);
                    templatePath = ReportConstants.rpt_KNTC_QH_Baocao_ChuaTraLoi_3C;
                }
                if (iLoaiBaoCao == 1)
                {
                    fileName = string.Format("{0}.{1}", "Baocao3C(HDND)_DanhSachChuyenDonTheoKhoangTg_3C", ext);
                    templatePath = ReportConstants.rpt_KNTC_HDND_Baocao_ChuaTraLoi_3C;
                }
                ExcelFile xls = ExportReportDanhSachChuyenDonTheoKhoangTg_3C(iLoaiBaoCao, dtungay, ddenngay, iLoaiDon, templatePath);
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }
        }
        public ExcelFile ExportReportDanhSachChuyenDonTheoKhoangTg_3C(int iLoaiBaoCao, DateTime? dTuNgay, DateTime? dDenNgay, int iLoaiDon, string templatePath)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();
            List<KNTC_REPORT_CHUATRALOI> List = new List<KNTC_REPORT_CHUATRALOI>();
            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }

            if (iLoaiBaoCao == -1)
            {
                //Đoàn đại biểu quốc hội
                List<KNTC_REPORT_CHUATRALOI> dt_QH = new List<KNTC_REPORT_CHUATRALOI>();
                dt_QH = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi("RPT_KNTC_BAOCAO_CHUACOTRALOI_3B_3C", dTuNgay, dDenNgay, 0, 0, 0, iLoaiDon, iUser);
                List<KNTC_REPORT_CHUATRALOI> dt_HĐND = new List<KNTC_REPORT_CHUATRALOI>();
                dt_HĐND = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi("RPT_KNTC_BAOCAO_CHUACOTRALOI_3B_3C", dTuNgay, dDenNgay, 0, 0, 1, iLoaiDon, iUser);
                fr.AddTable<KNTC_REPORT_CHUATRALOI>("listQH", dt_QH);
                fr.AddTable<KNTC_REPORT_CHUATRALOI>("listHDND", dt_HĐND);
            }
            else
            {
                List = kntc.getReportBaoCaoMoiTongHop_ChuaTraLoi("RPT_KNTC_BAOCAO_CHUACOTRALOI_3B_3C", dTuNgay, dDenNgay, 0, 0, iLoaiBaoCao, iLoaiDon, iUser);
                fr.AddTable<KNTC_REPORT_CHUATRALOI>("List", List);
            }

            fr.SetValue(new
            {
                startdate = String.Format("{0:dd/MM/yyyy}", dTuNgay),
                enddate = String.Format("{0:dd/MM/yyyy}", dDenNgay)
            });
            fr.UseForm(this).Run(Result);
            return Result;

        }

        public ActionResult BaoCaoDanhSachCongVanDonDoc_3F (int iDoiTuong = -1, string dBatDau = "", string dKetThuc = "", string ext = "pdf")
        {
            try
            {

                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
                int iLoaiBaoCao = 0;
                
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                
                if (dBatDau != null && dBatDau != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(dBatDau));
                }
                if (dKetThuc != null && dKetThuc != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(dKetThuc));
                }
                
                iLoaiBaoCao = iDoiTuong;
                
                
                string fileName = "";
                var templatePath = "";
                if (iLoaiBaoCao == -1)
                {
                    fileName = string.Format("{0}.{1}", "Baocao3F(TongHop)_DanhSachCongVanDonDoc", ext);
                    templatePath = ReportConstants.rpt_KN_TongHop_Baocao_DanhSachCongVanDonDoc_3F;
                }
                if (iLoaiBaoCao == 0)
                {
                    fileName = string.Format("{0}.{1}", "Baocao3F(QH)_DanhSachCongVanDonDoc", ext);
                    templatePath = ReportConstants.rpt_KN_QH_Baocao_DanhSachCongVanDonDoc_3F;
                }
                if (iLoaiBaoCao == 1)
                {
                    fileName = string.Format("{0}.{1}", "Baocao3F(HDND)_DanhSachCongVanDonDoc", ext);
                    templatePath = ReportConstants.rpt_KN_HDND_Baocao_DanhSachCongVanDonDoc_3F;
                }
                ExcelFile xls = ExportReportDanhSachCongVanDonDoc_3F(iLoaiBaoCao, dtungay, ddenngay, templatePath);
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }
        }

        public ExcelFile ExportReportDanhSachCongVanDonDoc_3F( int iLoaiBaoCao, DateTime? dTuNgay, DateTime? dDenNgay, string templatePath)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();
            List<RPT_KNTC_CONGVANDONDOC_3F> List = new List<RPT_KNTC_CONGVANDONDOC_3F>();
            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }
            
            if (iLoaiBaoCao == -1)
            {
                //Đoàn đại biểu quốc hội
                List<RPT_KNTC_CONGVANDONDOC_3F> dt_QH = new List<RPT_KNTC_CONGVANDONDOC_3F>();
                dt_QH = _kntcReport.getReportDanhSachCongVanDonDoc_3F( 0, dTuNgay, dDenNgay, iUser);
                List<RPT_KNTC_CONGVANDONDOC_3F> dt_HĐND = new List<RPT_KNTC_CONGVANDONDOC_3F>();
                dt_HĐND = _kntcReport.getReportDanhSachCongVanDonDoc_3F( 1, dTuNgay, dDenNgay, iUser);
                fr.AddTable<RPT_KNTC_CONGVANDONDOC_3F>("listQH", dt_QH);
                fr.AddTable<RPT_KNTC_CONGVANDONDOC_3F>("listHDND", dt_HĐND);
            }
            else
            {
                List = _kntcReport.getReportDanhSachCongVanDonDoc_3F( iLoaiBaoCao, dTuNgay, dDenNgay, iUser);
                fr.AddTable<RPT_KNTC_CONGVANDONDOC_3F>("dt", List);
            }

            fr.SetValue(new
            {
                To = 1
            });
            fr.UseForm(this).Run(Result);
            return Result;

        }

        public ActionResult Baocao_DonDaTraLoi(int ikhoa = 0, int iTenbaocao = 1, int iLoaiBaoCao = 0, int iCoQuanTraLoi = 0,
            string iTuNgay = "", string iDenNgay = "", string iTuNgayKyTruoc = "", string iDenNgayKyTruoc = "", string listDiaPhuong = "",  string ext = "pdf", string text = "")
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
                string fileName = "";
                var templatePath = "";
                if (listDiaPhuong == "null")
                {
                    listDiaPhuong = "";
                }    

                ExcelFile xls = null;
                switch (iTenbaocao)
                {
                    case 1:
                        if (iLoaiBaoCao == 0)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4A(TongHop)_DanhSachCacDonViDaTraLoi", ext);
                            templatePath = ReportConstants.rpt_KN_TongHop_Baocao_DanhSachCacDonViDaTL_4A;
                        }
                        else if (iLoaiBaoCao == 1)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4A(QH)_DanhSachCacDonViDaTraLoi", ext);
                            templatePath = ReportConstants.rpt_KN_QH_Baocao_DanhSachCacDonViDaTL_4A;
                        }
                        else if (iLoaiBaoCao == 2)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4A(HDND)_DanhSachCacDonViDaTraLoi", ext);
                            templatePath = ReportConstants.rpt_KN_HDND_Baocao_DanhSachCacDonViDaTL_4A;
                        }
                        xls = ExportReportDanhSachCacDonViDaTL_4A(ikhoa, iLoaiBaoCao, iCoQuanTraLoi, iTuNgay, iDenNgay, templatePath);
                        break;
                    case 2:
                        if (iLoaiBaoCao == 0)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4B(TongHop)_TongHopKetQuaXuLyDon", ext);
                            templatePath = ReportConstants.rpt_KN_TongHop_Baocao_TongHopKetQuaXuLyDon_4B;
                        }
                        else if (iLoaiBaoCao == 1)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4B(QH)_TongHopKetQuaXuLyDon", ext);
                            templatePath = ReportConstants.rpt_KN_QH_Baocao_TongHopKetQuaXuLyDon_4B;
                        }
                        else if (iLoaiBaoCao == 2)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4B(HDND)_TongHopKetQuaXuLyDon", ext);
                            templatePath = ReportConstants.rpt_KN_HDND_Baocao_TongHopKetQuaXuLyDon_4B;
                        }
                        xls = ExportReportTongHopKetQuaXuLyDon_4B(iLoaiBaoCao, iTuNgay, iDenNgay, iTuNgayKyTruoc, iDenNgayKyTruoc, templatePath, text);
                        break;
                    case 6:
                        if (iLoaiBaoCao == 0)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4F(TongHop)_KetQuaTiepNhanXuLyDonChiTiet", ext);
                            templatePath = ReportConstants.rpt_KN_TongHop_Baocao_KetQuaTiepNhanXuLyDonChiTiet_4F;
                        }
                        else if (iLoaiBaoCao == 1)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4F(QH)_KetQuaTiepNhanXuLyDonChiTiet", ext);
                            templatePath = ReportConstants.rpt_KN_QH_Baocao_KetQuaTiepNhanXuLyDonChiTiet_4F;
                        }
                        else if (iLoaiBaoCao == 2)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4F(HDND)_KetQuaTiepNhanXuLyDonChiTiet", ext);
                            templatePath = ReportConstants.rpt_KN_HDND_Baocao_KetQuaTiepNhanXuLyDonChiTiet_4F;
                        }
                        xls = ExportReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4F(iTuNgay, iDenNgay, iTuNgayKyTruoc, iDenNgayKyTruoc, iLoaiBaoCao - 1, listDiaPhuong, templatePath, text);
                        break;
                    case 7:
                        if (iLoaiBaoCao == 0)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4G(TongHop)_KetQuaTiepNhanXuLyDonChiTiet", ext);
                            templatePath = ReportConstants.rpt_KN_TongHop_Baocao_KetQuaTiepNhanXuLyDonChiTiet_4G;
                        }
                        else if (iLoaiBaoCao == 1)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4G(QH)_KetQuaTiepNhanXuLyDonChiTiet", ext);
                            templatePath = ReportConstants.rpt_KN_QH_Baocao_KetQuaTiepNhanXuLyDonChiTiet_4G;
                        }
                        else if (iLoaiBaoCao == 2)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4G(HDND)_KetQuaTiepNhanXuLyDonChiTiet", ext);
                            templatePath = ReportConstants.rpt_KN_HDND_Baocao_KetQuaTiepNhanXuLyDonChiTiet_4G;
                        }
                        xls = ExportReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4G(iTuNgay, iDenNgay, iTuNgayKyTruoc, iDenNgayKyTruoc, iLoaiBaoCao - 1, listDiaPhuong, templatePath, text);
                        break;
                    case 3:
                        if (iLoaiBaoCao == 0)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4C(TongHop)_TongHopKetQuaXuLyDon", ext);
                            templatePath = ReportConstants.rpt_KN_TongHop_Baocao_TongHopKetQuaXuLyDon_4C;
                        }
                        else if (iLoaiBaoCao == 1)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4C(QH)_TongHopKetQuaXuLyDon", ext);
                            templatePath = ReportConstants.rpt_KN_QH_Baocao_TongHopKetQuaXuLyDon_4C;
                        }
                        else if (iLoaiBaoCao == 2)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4C(HDND)_TongHopKetQuaXuLyDon", ext);
                            templatePath = ReportConstants.rpt_KN_HDND_Baocao_TongHopKetQuaXuLyDon_4C;
                        }
                        xls = ExportReportTongHopKetQuaXuLyDon_4C(iLoaiBaoCao, iTuNgay, iDenNgay, iTuNgayKyTruoc, iDenNgayKyTruoc, templatePath, text);
                        break;

                    case 4:
                        if (iLoaiBaoCao == 0)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4D(TongHop)_TongHopKetQuaXuLyDon", ext);
                            templatePath = ReportConstants.rpt_KN_TongHop_Baocao_TongHopKetQuaXuLyDon_4D;
                        }
                        else if (iLoaiBaoCao == 1)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4D(QH)_TongHopKetQuaXuLyDon", ext);
                            templatePath = ReportConstants.rpt_KN_QH_Baocao_TongHopKetQuaXuLyDon_4D;
                        }
                        else if (iLoaiBaoCao == 2)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4D(HDND)_TongHopKetQuaXuLyDon", ext);
                            templatePath = ReportConstants.rpt_KN_HDND_Baocao_TongHopKetQuaXuLyDon_4D;
                        }
                        xls = ExportReportTongHopKetQuaXuLyDon_4D(iLoaiBaoCao, iTuNgay, iDenNgay, iTuNgayKyTruoc, iDenNgayKyTruoc, templatePath, text);
                        break;
                    case 5:
                        if (iLoaiBaoCao == 0)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4E(TongHop)_TiepNhanXuLyGiamSat", ext);
                            templatePath = ReportConstants.rpt_KN_TongHop_Baocao_TiepNhanXuLyGiamSat_4E;
                        }
                        else if (iLoaiBaoCao == 1)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4E(QH)_TiepNhanXuLyGiamSat", ext);
                            templatePath = ReportConstants.rpt_KN_QH_Baocao_TiepNhanXuLyGiamSat_4E;
                        }
                        else if (iLoaiBaoCao == 2)
                        {
                            fileName = string.Format("{0}.{1}", "Baocao4E(HDND)_TiepNhanXuLyGiamSat", ext);
                            templatePath = ReportConstants.rpt_KN_HDND_Baocao_TiepNhanXuLyGiamSat_4E;
                        }
                        xls = ExportReportTiepNhanXuLyGiamSat_4E(iLoaiBaoCao, iTuNgay, iDenNgay, templatePath, text);
                        break;
                    default:
                        xls = new XlsFile(true);
                        break;
                }
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }
        }

        public ExcelFile ExportReportDanhSachCacDonViDaTL_4A(int ikhoa, int iLoaiBaoCao, int iCoQuanTraLoi, string dTuNgay , string dDenNgay , string templatePath)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();
            List<RPT_KNTC_DONVIDATRALOI_4A> List = new List<RPT_KNTC_DONVIDATRALOI_4A>();
            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }
            if (dTuNgay == "") dTuNgay = null;
            if (dDenNgay == "") dDenNgay = null;
            DateTime? tuNgay = null;
            DateTime? denNgay = null;
            if (dTuNgay != null && dTuNgay != "") tuNgay = Convert.ToDateTime(func.ConvertDateToSql(dTuNgay));
            if (dDenNgay != null && dDenNgay != "") denNgay = Convert.ToDateTime(func.ConvertDateToSql(dDenNgay));
            if (iLoaiBaoCao == 0)
            {
                //Đoàn đại biểu quốc hội
                List<RPT_KNTC_DONVIDATRALOI_4A> dt_QH = new List<RPT_KNTC_DONVIDATRALOI_4A>();
                dt_QH = _kntcReport.getReportDanhSachCacDonViDaTL_4A(ikhoa, 1 , iCoQuanTraLoi, tuNgay, denNgay, iUser);
                List<RPT_KNTC_DONVIDATRALOI_4A> dt_HĐND = new List<RPT_KNTC_DONVIDATRALOI_4A>();
                dt_HĐND = _kntcReport.getReportDanhSachCacDonViDaTL_4A(ikhoa, 2, iCoQuanTraLoi, tuNgay, denNgay, iUser);
                fr.AddTable<RPT_KNTC_DONVIDATRALOI_4A>("listQH", dt_QH);
                fr.AddTable<RPT_KNTC_DONVIDATRALOI_4A>("listHDND", dt_HĐND);
            }
            else
            {
                List = _kntcReport.getReportDanhSachCacDonViDaTL_4A(ikhoa, iLoaiBaoCao, iCoQuanTraLoi, tuNgay, denNgay, iUser);
                fr.AddTable<RPT_KNTC_DONVIDATRALOI_4A>("dt", List);
            }

            fr.SetValue(new
            {
                To = 1
            });
            fr.UseForm(this).Run(Result);
            return Result;
        }

        public ExcelFile ExportReportTongHopKetQuaXuLyDon_4B(int iLoaiBaoCao, string dTuNgay, string dDenNgay, string dTuNgayKyTruoc, string dDenNgayKyTruoc, string templatePath, string text)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();

            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }
            DateTime? tuNgay = null;
            DateTime? denNgay = null;
            DateTime? tuNgayKyTruoc = null;
            DateTime? denNgayKyTruoc = null;
            if (!string.IsNullOrEmpty(dTuNgay))
            {
                tuNgay = Convert.ToDateTime(func.ConvertDateToSql(dTuNgay));
            }
            if (!string.IsNullOrEmpty(dDenNgay))
            {
                denNgay = Convert.ToDateTime(func.ConvertDateToSql(dDenNgay));
            }
            if (!string.IsNullOrEmpty(dTuNgayKyTruoc))
            {
                tuNgayKyTruoc = Convert.ToDateTime(func.ConvertDateToSql(dTuNgayKyTruoc));
            }
            if (!string.IsNullOrEmpty(dDenNgayKyTruoc))
            {
                denNgayKyTruoc = Convert.ToDateTime(func.ConvertDateToSql(dDenNgayKyTruoc));
            }

            if (iLoaiBaoCao == 0)
            {
                List<RPT_KNTC_DONDATRALOI_4B> dt_QH = _kntcReport.GetReportTongHopKetQuaXuLyDon_4B(1, tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iUser);
                
                List<RPT_KNTC_DONDATRALOI_4B> dt_HĐND = _kntcReport.GetReportTongHopKetQuaXuLyDon_4B(2, tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iUser);
                
                fr.AddTable<RPT_KNTC_DONDATRALOI_4B>("listQH", dt_QH);
                fr.AddTable<RPT_KNTC_DONDATRALOI_4B>("listHDND", dt_HĐND);
            }
            else
            {
                List<RPT_KNTC_DONDATRALOI_4B> dataList = _kntcReport.GetReportTongHopKetQuaXuLyDon_4B(iLoaiBaoCao, tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iUser);
                
                fr.AddTable<RPT_KNTC_DONDATRALOI_4B>("dt", dataList);
            }

            fr.SetValue(new
            {
                text = text,
            });
            fr.UseForm(this).Run(Result);
            return Result;

        }
        
        public ExcelFile ExportReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4F(string dTuNgay, string dDenNgay, string dTuNgayKyTruoc, string dDenNgayKyTruoc, int iDoiTuong, string listDiaPhuong,  string templatePath, string text)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();

            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }
            DateTime? tuNgay = null;
            DateTime? denNgay = null;
            DateTime? tuNgayKyTruoc = null;
            DateTime? denNgayKyTruoc = null;
            if (!string.IsNullOrEmpty(dTuNgay))
            {
                tuNgay = Convert.ToDateTime(func.ConvertDateToSql(dTuNgay));
                dTuNgay = tuNgay.HasValue ? tuNgay.Value.ToString("dd/MM/yyyy") : "……/……/………";
            }
            else
            {
                dTuNgay = "……/……/………";
            }
            if (!string.IsNullOrEmpty(dDenNgay))
            {
                denNgay = Convert.ToDateTime(func.ConvertDateToSql(dDenNgay));
                dDenNgay = denNgay.HasValue ? denNgay.Value.ToString("dd/MM/yyyy") : "……/……/………";
            }
            else
            {
                dDenNgay = "……/……/………";
            }
            if (!string.IsNullOrEmpty(dTuNgayKyTruoc))
            {
                tuNgayKyTruoc = Convert.ToDateTime(func.ConvertDateToSql(dTuNgayKyTruoc));
            }
            if (!string.IsNullOrEmpty(dDenNgayKyTruoc))
            {
                denNgayKyTruoc = Convert.ToDateTime(func.ConvertDateToSql(dDenNgayKyTruoc));
            }

            if (iDoiTuong == -1)
            {
                List<RPT_KNTC_DONCHITIET_4F> dt_QH = _kntcReport.GetReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4F(tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, 0, listDiaPhuong, iUser);
                foreach(RPT_KNTC_DONCHITIET_4F dt in dt_QH)
                {
                    dt.STENDONVI = "Đoàn ĐB Quốc hội tỉnh";
                }
                List<RPT_KNTC_DONCHITIET_4F> dt_HDND = _kntcReport.GetReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4F(tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, 1, listDiaPhuong, iUser);
                foreach (RPT_KNTC_DONCHITIET_4F dt in dt_HDND)
                {
                    dt.STENDONVI = "Hội đồng nhân dân tỉnh";
                }
                fr.AddTable<RPT_KNTC_DONCHITIET_4F>("listQH", dt_QH);
                fr.AddTable<RPT_KNTC_DONCHITIET_4F>("listHDND", dt_HDND);
            }
            else
            {
                List<RPT_KNTC_DONCHITIET_4F> dataList = _kntcReport.GetReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4F(tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iDoiTuong, listDiaPhuong, iUser);
                foreach (RPT_KNTC_DONCHITIET_4F dt in dataList)
                {
                    if (iDoiTuong == 0)
                    {
                        dt.STENDONVI = "Đoàn ĐB Quốc hội tỉnh";
                    }
                    else if (iDoiTuong == 1)
                    {
                        dt.STENDONVI = "Hội đồng nhân dân tỉnh";
                    }
                }
                fr.AddTable<RPT_KNTC_DONCHITIET_4F>("dt", dataList);
            }

            fr.SetValue(new
            {
                text = text.ToUpper()
            });
            fr.UseForm(this).Run(Result);
            return Result;

        }
        
        public ExcelFile ExportReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4G(string dTuNgay, string dDenNgay, string dTuNgayKyTruoc, string dDenNgayKyTruoc, int iDoiTuong, string listDiaPhuong,  string templatePath, string text)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();

            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }
            DateTime? tuNgay = null;
            DateTime? denNgay = null;
            DateTime? tuNgayKyTruoc = null;
            DateTime? denNgayKyTruoc = null;
            if (!string.IsNullOrEmpty(dTuNgay))
            {
                tuNgay = Convert.ToDateTime(func.ConvertDateToSql(dTuNgay));
                dTuNgay = tuNgay.HasValue ? tuNgay.Value.ToString("dd/MM/yyyy") : "……/……/………";
            }
            else
            {
                dTuNgay = "……/……/………";
            }
            if (!string.IsNullOrEmpty(dDenNgay))
            {
                denNgay = Convert.ToDateTime(func.ConvertDateToSql(dDenNgay));
                dDenNgay = denNgay.HasValue ? denNgay.Value.ToString("dd/MM/yyyy") : "……/……/………";
            }
            else
            {
                dDenNgay = "……/……/………";
            }
            if (!string.IsNullOrEmpty(dTuNgayKyTruoc))
            {
                tuNgayKyTruoc = Convert.ToDateTime(func.ConvertDateToSql(dTuNgayKyTruoc));
            }
            if (!string.IsNullOrEmpty(dDenNgayKyTruoc))
            {
                denNgayKyTruoc = Convert.ToDateTime(func.ConvertDateToSql(dDenNgayKyTruoc));
            }

            if (iDoiTuong == -1)
            {
                List<RPT_KNTC_DONCHITIET_4G> dt_QH = _kntcReport.GetReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4G(tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, 0, listDiaPhuong, iUser);
                foreach(RPT_KNTC_DONCHITIET_4G dt in dt_QH)
                {
                    dt.STENDONVI = "Đoàn ĐB Quốc hội tỉnh";
                }
                List<RPT_KNTC_DONCHITIET_4G> dt_HDND = _kntcReport.GetReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4G(tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, 1, listDiaPhuong, iUser);
                foreach (RPT_KNTC_DONCHITIET_4G dt in dt_HDND)
                {
                    dt.STENDONVI = "Hội đồng nhân dân tỉnh";
                }
                fr.AddTable<RPT_KNTC_DONCHITIET_4G>("listQH", dt_QH);
                fr.AddTable<RPT_KNTC_DONCHITIET_4G>("listHDND", dt_HDND);
            }
            else
            {
                List<RPT_KNTC_DONCHITIET_4G> dataList = _kntcReport.GetReportTongHopKetQuaTiepNhanXuLyDonChiTiet_4G(tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iDoiTuong, listDiaPhuong, iUser);
                foreach (RPT_KNTC_DONCHITIET_4G dt in dataList)
                {
                    if (iDoiTuong == 0)
                    {
                        dt.STENDONVI = "Đoàn ĐB Quốc hội tỉnh";
                    }
                    else if (iDoiTuong == 1)
                    {
                        dt.STENDONVI = "Hội đồng nhân dân tỉnh";
                    }
                }
                fr.AddTable<RPT_KNTC_DONCHITIET_4G>("dt", dataList);
            }

            fr.SetValue(new
            {
                text = text.ToUpper()
            });
            fr.UseForm(this).Run(Result);
            return Result;

        }

        public ExcelFile ExportReportTongHopKetQuaXuLyDon_4C(int iLoaiBaoCao, string dTuNgay, string dDenNgay, string dTuNgayKyTruoc, string dDenNgayKyTruoc, string templatePath, string text)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();

            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }

            DateTime? tuNgay = null;
            DateTime? denNgay = null;
            DateTime? tuNgayKyTruoc = null;
            DateTime? denNgayKyTruoc = null;
            if (!string.IsNullOrEmpty(dTuNgay))
            {
                tuNgay = Convert.ToDateTime(func.ConvertDateToSql(dTuNgay));
            }
            
            if (!string.IsNullOrEmpty(dDenNgay))
            {
                denNgay = Convert.ToDateTime(func.ConvertDateToSql(dDenNgay));
            }
            if (!string.IsNullOrEmpty(dTuNgayKyTruoc))
            {
                tuNgayKyTruoc = Convert.ToDateTime(func.ConvertDateToSql(dTuNgayKyTruoc));
            }
            if (!string.IsNullOrEmpty(dDenNgayKyTruoc))
            {
                denNgayKyTruoc = Convert.ToDateTime(func.ConvertDateToSql(dDenNgayKyTruoc));
            }

            if (iLoaiBaoCao == 0)
            {
                List<RPT_KNTC_DONDATRALOI_4C> dt_QH = _kntcReport.GetReportTongHopKetQuaXuLyDon_4C(1, tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iUser);
                
                List<RPT_KNTC_DONDATRALOI_4C> dt_HĐND = _kntcReport.GetReportTongHopKetQuaXuLyDon_4C(2, tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iUser);
                
                fr.AddTable<RPT_KNTC_DONDATRALOI_4C>("listQH", dt_QH);
                fr.AddTable<RPT_KNTC_DONDATRALOI_4C>("listHDND", dt_HĐND);
            }
            else
            {
                List<RPT_KNTC_DONDATRALOI_4C> dataList = _kntcReport.GetReportTongHopKetQuaXuLyDon_4C(iLoaiBaoCao, tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iUser);
                
                fr.AddTable<RPT_KNTC_DONDATRALOI_4C>("dt", dataList);
            }

            fr.SetValue(new
            {
                text = text,
            });
            fr.UseForm(this).Run(Result);
            return Result;

        }

        public ExcelFile ExportReportTongHopKetQuaXuLyDon_4D(int iLoaiBaoCao, string dTuNgay, string dDenNgay, string dTuNgayKyTruoc, string dDenNgayKyTruoc, string templatePath , string text)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();

            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }

            DateTime? tuNgay = null;
            DateTime? denNgay = null;
            DateTime? tuNgayKyTruoc = null;
            DateTime? denNgayKyTruoc = null;
            if (!string.IsNullOrEmpty(dTuNgay))
            {
                tuNgay = Convert.ToDateTime(func.ConvertDateToSql(dTuNgay));
            }
            else
            if (!string.IsNullOrEmpty(dDenNgay))
            {
                denNgay = Convert.ToDateTime(func.ConvertDateToSql(dDenNgay));
            }
            if (!string.IsNullOrEmpty(dTuNgayKyTruoc))
            {
                tuNgayKyTruoc = Convert.ToDateTime(func.ConvertDateToSql(dTuNgayKyTruoc));
            }
            if (!string.IsNullOrEmpty(dDenNgayKyTruoc))
            {
                denNgayKyTruoc = Convert.ToDateTime(func.ConvertDateToSql(dDenNgayKyTruoc));
            }

            if (iLoaiBaoCao == 0)
            {
                List<RPT_KNTC_DONDATRALOI_4C> dt_QH = _kntcReport.GetReportTongHopKetQuaXuLyDon_4D(1, tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iUser);

                List<RPT_KNTC_DONDATRALOI_4C> dt_HĐND = _kntcReport.GetReportTongHopKetQuaXuLyDon_4D(2, tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iUser);

                fr.AddTable<RPT_KNTC_DONDATRALOI_4C>("listQH", dt_QH);
                fr.AddTable<RPT_KNTC_DONDATRALOI_4C>("listHDND", dt_HĐND);
            }
            else
            {
                List<RPT_KNTC_DONDATRALOI_4C> dataList = _kntcReport.GetReportTongHopKetQuaXuLyDon_4D(iLoaiBaoCao, tuNgay, denNgay, tuNgayKyTruoc, denNgayKyTruoc, iUser);

                fr.AddTable<RPT_KNTC_DONDATRALOI_4C>("dt", dataList);
            }

            fr.SetValue(new
            {
                text = text,
            });
            fr.UseForm(this).Run(Result);
            return Result;

        }
        public void AddSoDon(RPT_KNTC_DONDATRALOI_4E cur, RPT_KNTC_DONDATRALOI_4E val)
        {
            cur.ND_KN_SODON += val.ND_KN_SODON;
            cur.ND_TC_SODON += val.ND_TC_SODON;
            cur.ND_PA_SODON += val.ND_PA_SODON;
            cur.ND_KHAC_SODON += val.ND_KHAC_SODON;
            cur.LV_HC_SODON += val.LV_HC_SODON;
            cur.LV_TP_SODON += val.LV_TP_SODON;
            cur.LV_DT_SODON += val.LV_DT_SODON;
            cur.LV_TN_SODON += val.LV_TN_SODON;
            cur.LV_KHAC_SODON += val.LV_KHAC_SODON;
            cur.DK_D_SODON += val.DK_D_SODON;
            cur.DK_KD_TONG_SODON += val.DK_KD_TONG_SODON;
            cur.DK_KD_TRUNG_SODON += val.DK_KD_TRUNG_SODON;
            cur.CHUYEN_SODON += val.CHUYEN_SODON;
            cur.DDOC_SODON += val.DDOC_SODON;
            cur.TLOI_SODON += val.TLOI_SODON;
            cur.NCUU_SODON += val.NCUU_SODON;
            cur.TDOI_SODON += val.TDOI_SODON;
            cur.REP_SODON += val.REP_SODON;
            cur.TONGLUU_SODON += val.TONGLUU_SODON;
        }

        private string CalcTyLe(int val, int sum)
        {
            if (sum == 0)
            {
                return "0";
            }
            return string.Format("{0:0.##}", 100.0 * val / sum);
        }

        public void CalcTyLe(RPT_KNTC_DONDATRALOI_4E cur, RPT_KNTC_DONDATRALOI_4E sum)
        {
            cur.ND_KN_TYLE = CalcTyLe((int)cur.ND_KN_SODON, (int)sum.ND_KN_SODON);
            cur.ND_TC_TYLE = CalcTyLe((int)cur.ND_TC_SODON, (int)sum.ND_TC_SODON);
            cur.ND_PA_TYLE = CalcTyLe((int)cur.ND_PA_SODON, (int)sum.ND_PA_SODON);
            cur.ND_KHAC_TYLE = CalcTyLe((int)cur.ND_KHAC_SODON, (int)sum.ND_KHAC_SODON);
            cur.LV_HC_TYLE = CalcTyLe((int)cur.LV_HC_SODON, (int)sum.LV_HC_SODON);
            cur.LV_TP_TYLE = CalcTyLe((int)cur.LV_TP_SODON, (int)sum.LV_TP_SODON);
            cur.LV_DT_TYLE = CalcTyLe((int)cur.LV_DT_SODON, (int)sum.LV_DT_SODON);
            cur.LV_TN_TYLE = CalcTyLe((int)cur.LV_TN_SODON, (int)sum.LV_TN_SODON);
            cur.LV_KHAC_TYLE = CalcTyLe((int)cur.LV_KHAC_SODON, (int)sum.LV_KHAC_SODON);
            cur.DK_D_TYLE = CalcTyLe((int)cur.DK_D_SODON, (int)sum.DK_D_SODON);
            cur.DK_KD_TONG_TYLE = CalcTyLe((int)cur.DK_KD_TONG_SODON, (int)sum.DK_KD_TONG_SODON);
            cur.DK_KD_TRUNG_TYLE = CalcTyLe((int)cur.DK_KD_TRUNG_SODON, (int)sum.DK_KD_TRUNG_SODON);
            cur.CHUYEN_TYLE = CalcTyLe((int)cur.CHUYEN_SODON, (int)sum.CHUYEN_SODON);
            cur.DDOC_TYLE = CalcTyLe((int)cur.DDOC_SODON, (int)sum.DDOC_SODON);
            cur.TLOI_TYLE = CalcTyLe((int)cur.TLOI_SODON, (int)sum.TLOI_SODON);
            cur.NCUU_TYLE = CalcTyLe((int)cur.NCUU_SODON, (int)sum.NCUU_SODON);
            cur.TDOI_TYLE = CalcTyLe((int)cur.TDOI_SODON, (int)sum.TDOI_SODON);
            cur.REP_TYLE = CalcTyLe((int)cur.REP_SODON, (int)sum.REP_SODON);
            cur.TONGLUU_TYLE = CalcTyLe((int)cur.TONGLUU_SODON, (int)sum.TONGLUU_SODON);
        }


        public ExcelFile ExportReportTiepNhanXuLyGiamSat_4E(int iLoaiBaoCao, string dTuNgay, string dDenNgay, string templatePath, string text)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();

            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }

            DateTime? tuNgay = null;
            DateTime? denNgay = null;

            string khoangthoigian = text;

            if (!string.IsNullOrEmpty(dTuNgay))
            {
                tuNgay = Convert.ToDateTime(func.ConvertDateToSql(dTuNgay));
                dTuNgay = tuNgay.HasValue ? tuNgay.Value.ToString("dd/MM/yyyy") : "……/……/………";
            }
            else
            {
                dTuNgay = "……/……/………";
            }
            if (!string.IsNullOrEmpty(dDenNgay))
            {
                denNgay = Convert.ToDateTime(func.ConvertDateToSql(dDenNgay));
                dDenNgay = denNgay.HasValue ? denNgay.Value.ToString("dd/MM/yyyy") : "……/……/………";
            }
            else
            {
                dDenNgay = "……/……/………";
            }

            List<RPT_KNTC_DONDATRALOI_4E> dt = new List<RPT_KNTC_DONDATRALOI_4E>();

            if (iLoaiBaoCao == 0)
            {
                List<RPT_KNTC_DONDATRALOI_4E> dt_QH = _kntcReport.GetReportTiepNhanXuLyGiamSat_4E(1, tuNgay, denNgay, iUser);
                var sum = new RPT_KNTC_DONDATRALOI_4E();
                foreach (var dp in dt_QH)
                {
                    AddSoDon(sum, dp);
                }
                foreach (var dp in dt_QH)
                {
                    CalcTyLe(dp, sum);
                }

                List<RPT_KNTC_DONDATRALOI_4E> dt_HĐND = _kntcReport.GetReportTiepNhanXuLyGiamSat_4E(2, tuNgay, denNgay, iUser);
                sum = new RPT_KNTC_DONDATRALOI_4E();
                foreach (var dp in dt_HĐND)
                {
                    AddSoDon(sum, dp);
                }
                foreach (var dp in dt_HĐND)
                {
                    CalcTyLe(dp, sum);
                }

                dt.Add(new RPT_KNTC_DONDATRALOI_4E { TT = "A. ĐOÀN ĐẠI BIỂU QUỐC HỘI", ISTITLE = true });
                dt.AddRange(dt_QH);
                dt.Add(new RPT_KNTC_DONDATRALOI_4E { TT = "B. THƯỜNG TRỰC HĐND TỈNH", ISTITLE = true });
                dt.AddRange(dt_HĐND);
            }
            else
            {
                List<RPT_KNTC_DONDATRALOI_4E> data = _kntcReport.GetReportTiepNhanXuLyGiamSat_4E(iLoaiBaoCao, tuNgay, denNgay, iUser);
                
                var sum = new RPT_KNTC_DONDATRALOI_4E();
                foreach (var dp in data)
                {
                    AddSoDon(sum, dp);
                }
                foreach (var dp in data)
                {
                    CalcTyLe(dp, sum);
                }
                dt.AddRange(data);
            }

            fr.AddTable<RPT_KNTC_DONDATRALOI_4E>("dt", dt);
            fr.SetValue(new
            {
                khoangthoigian = khoangthoigian.ToUpper()
            });

            fr.UseForm(this).Run(Result);

            int rowstart = 12;
            int colstart = 1;
            int collst = 41;
            int index = 0;
            TFlxFormat fmt = Result.GetFormat(Result.GetCellFormat(rowstart, colstart));
            fmt.HAlignment = THFlxAlignment.left;
            fmt.Font.Style = TFlxFontStyles.Bold;

            foreach (var item in dt)
            {
                if (item.ISTITLE)
                {
                    Result.MergeCells(rowstart + index, colstart, rowstart + index, collst);
                    Result.SetRowFormat(rowstart + index, Result.AddFormat(fmt));
                }
                index++;
            }

            return Result;

        }

        public ActionResult Ajax_popup_in_congvandondoc(string idVanban)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                KNTC_VANBAN vanbantocao = kntc.GetVB_ByID(Convert.ToInt32(idVanban));
                QUOCHOI_COQUAN donvibanhanh = kntc.GetDonVi(Convert.ToInt32(vanbantocao.ICOQUANBANHANH));
                QUOCHOI_COQUAN donvinhan = kntc.GetDonVi(Convert.ToInt32(vanbantocao.ICOQUANNHAN));
                ViewData["vanbantocao"] = vanbantocao;
                ViewData["donvibanhanh"] = donvibanhanh;
                ViewData["donvinhan"] = donvinhan;
                return PartialView("../Ajax/Kntc/CongVanDonDocPrint");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "In công văn đôn đốc", null, 1);
                return null;
            }
        }

        public ActionResult Baocao_CongVanDonDoc(string iVanBan = "", string ext = "pdf")
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
                var templatePath = "";
                KNTC_VANBAN vanbantocao = kntc.GetVB_ByID(Convert.ToInt32(iVanBan));
                KNTC_DON don = kntc.GetDON(Convert.ToInt32(vanbantocao.IDON));
                string fileName;
                if (don.IDOITUONGGUI == 0)
                {
                    fileName = string.Format("{0}.{1}", "Baocao3G(QH)_ChiTietCongVanDonDoc", ext);
                    templatePath = ReportConstants.rpt_KN_QH_Baocao_ChiTietCongVanDonDoc_3G;
                }
                else
                {
                    fileName = string.Format("{0}.{1}", "Baocao3G(HDND)_ChiTietCongVanDonDoc", ext);
                    templatePath = ReportConstants.rpt_KN_HDND_Baocao_ChiTietCongVanDonDoc_3G;
                }
                var diaphuong0 = kntc.Get_DiaPhuong(Convert.ToInt32(don.IDIAPHUONG_0)).CTEN;
                var diaphuong1 = kntc.Get_DiaPhuong(Convert.ToInt32(don.IDIAPHUONG_1)).CTEN;
                var diaphuong2 = kntc.Get_DiaPhuong(Convert.ToInt32(don.IDIAPHUONG_2)).CTEN;
                var coquanthuly = kntc.GetDonVi(Convert.ToInt32(don.IDONVITHULY)).CTEN;
                KNTC_VANBAN dongui = kntc.List_VanBan().Where(x => x.IDON == don.IDON && (x.CLOAI == "chuyenxulylaidon" || x.CLOAI == "chuyenxuly_noibo") && x.DDATE <= vanbantocao.DDATE).OrderByDescending(x => x.DNGAYBANHANH).FirstOrDefault();
                
                XlsFile xls = new XlsFile(true);
                xls.Open(Server.MapPath(templatePath));
                FlexCelReport fr = new FlexCelReport();
                
                fr.SetValue(new
                {
                    tendiachi = don.CNGUOIGUI_TEN.Trim()+", "+ don.CNGUOIGUI_DIACHI.Trim()+", " +diaphuong2+ ", "+diaphuong1+", "+diaphuong0 ,
                    coquanthuly = coquanthuly,
                    ngay =  Convert.ToDateTime(dongui.DNGAYBANHANH).ToString("dd"),
                    thang = Convert.ToDateTime(dongui.DNGAYBANHANH).ToString("MM"),
                    nam = Convert.ToDateTime(dongui.DNGAYBANHANH).ToString("yyyy"),
                    ngayquydinh = Convert.ToDateTime(don.INGAYQUYDINH).ToString("dd/MM/yyyy"),
                    congvanso = dongui.CSOVANBAN
                });;
                fr.UseForm(this).Run(xls);
                
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Báo cáo công văn đôn đốc");
                return View("../Home/Error_Exception");
            }
        }

        public ActionResult Ajax_popup_in_phieuchuyendon(string idVanban)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                KNTC_VANBAN vanbantocao = kntc.GetVB_ByID(Convert.ToInt32(idVanban));
                QUOCHOI_COQUAN donvibanhanh = kntc.GetDonVi(Convert.ToInt32(vanbantocao.ICOQUANBANHANH));
                QUOCHOI_COQUAN donvinhan = kntc.GetDonVi(Convert.ToInt32(vanbantocao.ICOQUANNHAN));
                ViewData["vanbantocao"] = vanbantocao;
                ViewData["donvibanhanh"] = donvibanhanh;
                ViewData["donvinhan"] = donvinhan;
                return PartialView("../Ajax/Kntc/PhieuChuyenDonPrint");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "In phiếu chuyển đơn", null, 1);
                return null;
            }
        }

        public ActionResult Baocao_PhieuChuyenDon(string iVanBan = "", string ext = "pdf")
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

                KNTC_VANBAN vanbantocao = kntc.GetVB_ByID(Convert.ToInt32(iVanBan));
                KNTC_DON don = kntc.GetDON(Convert.ToInt32(vanbantocao.IDON));
                decimal loaibaocao = don.IDOITUONGGUI;

                string fileName = "";
                var templatePath = "";
                if (loaibaocao == 1)
                {
                    fileName = string.Format("{0}.{1}", "Baocao3E(HDND)_PhieuChuyenDon", ext);
                    templatePath = ReportConstants.rpt_KN_HDND_Baocao_PhieuChuyenDon_3E;
                }
                if (loaibaocao == 0)
                {
                    fileName = string.Format("{0}.{1}", "Baocao3E(QH)_PhieuChuyenDon", ext);
                    templatePath = ReportConstants.rpt_KN_QH_Baocao_PhieuChuyenDon_3E;
                }
                ExcelFile xls = ExportReportPhieuChuyenDon3E(vanbantocao, don, loaibaocao, templatePath);
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Báo cáo phiếu chuyển đơn");
                return View("../Home/Error_Exception");
            }
        }

        public ExcelFile ExportReportPhieuChuyenDon3E(KNTC_VANBAN vanbantocao, KNTC_DON don, decimal loaibaocao, string templatePath)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();

            var diaphuong0 = kntc.Get_DiaPhuong(Convert.ToInt32(don.IDIAPHUONG_0)).CTEN;
            var diaphuong1 = kntc.Get_DiaPhuong(Convert.ToInt32(don.IDIAPHUONG_1)).CTEN;
            var diaphuong2 = kntc.Get_DiaPhuong(Convert.ToInt32(don.IDIAPHUONG_2)).CTEN;
            var coquanthuly = kntc.GetDonVi(Convert.ToInt32(don.IDONVITHULY)).CTEN;
            var tenkhoa = "KHÓA...";

            if (loaibaocao == 0)
            {
                QUOCHOI_KHOA khoa = _thietlap.Get_List_Quochoi_Khoa().Where(x => x.IDELETE == 0 && x.IKHOA == don.IKHOA).FirstOrDefault();
                tenkhoa = khoa.CTEN.ToUpper();
            }

            fr.SetValue(new
            {
                tenkhoa = tenkhoa,
                tennguoigui = don.CNGUOIGUI_TEN,
                tendiachi = don.CNGUOIGUI_DIACHI.Trim() + ", " + diaphuong2 + ", " + diaphuong1 + ", " + diaphuong0,
                coquanthuly = coquanthuly,
                noidungdon = don.CNOIDUNG
            });
            fr.UseForm(this).Run(Result);

            return Result;
        }
    }
}