using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Entities.Models;
using KienNghi.App_Code;
using System.IO;
using System.Web.Helpers;
using DataAccess.Busineess;
using Utilities;
using Entities.Objects;
namespace KienNghi.Controllers
{
    public class VanbanController : BaseController
    {
        //
        // GET: /Vanban/
        Vanban vb = new Vanban();
        Thietlap tl = new Thietlap();
        Funtions func = new Funtions();
        Kiennghi_cl kn = new Kiennghi_cl();
        KiennghiBusineess _kiennghi = new KiennghiBusineess();
        KntcBusineess kn_ = new KntcBusineess();
        VanbanBusineess _vanban = new VanbanBusineess();
        ThietlapBusineess _thietlap = new ThietlapBusineess();
        BaseBusineess _base = new BaseBusineess();
        Base base_appcode = new Base();
        Thietlaplist thutuc = new Thietlaplist(); 
        Log log = new Log();
        Dictionary<string, object> _condition;
        int error_level = 0;
      //  public string path_upload = "/upload/vanban/" + DateTime.Now.Year + "/";
      
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
        public string UploadFile(HttpPostedFileBase file)
        {
            Random random = new Random(); int rand = random.Next(0, 99999);

            string file_name = "";
            string dir_path_upload = AppConfig.dir_path_upload;
            string path_upload = "/vanban/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
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
        public ActionResult DownLoad(string id)
        {
            int id_sc = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
            VB_FILE_VANBAN f = _vanban.GetBy_Vanban_fileID(id_sc);

            string[] f_ = f.CURL.Split('/');
            //lấy địa chỉ file upload cùng thư mục code
            //string file_path = f.CFILE;
            //lấy địa chỉ file upload ngoài thư mục code
            string file_path = Server.MapPath("~/" + f.CURL + ""); ;
            string dir_path_download = AppConfig.dir_path_download;
            if (dir_path_download != "") { file_path = dir_path_download + file_path; }

            string fileUrl = file_path;
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileUrl);
            string fileName = Path.GetFileName(fileUrl);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public string read_fileVB(int ivanban)
        {
            //
            string url_cookie = func.Get_Url_keycookie();
            _condition = new Dictionary<string, object>();
            _condition.Add("IVANBAN", ivanban);
            string str = "";
            var filevb = _vanban.GetBy_List_Vanban_file(_condition).ToList();
            foreach (var x in filevb)
            {
                string id_encr = HashUtil.Encode_ID(x.IFILE.ToString(), url_cookie);
                if (x.CURL != "")
                {
                    string del = "<a href='javascript:void(0)' onclick=\"DeleteFile('" + id_encr + "','/Vanban/Ajax_Delete_VanBan_file')\" class='btn btn-danger'><i class='icon-remove'></i></a>";
                    str += "<p id='file_" + id_encr + "'><a href='/Home/DownLoad/" + id_encr + "' class='btn btn-success'><i class='icon-download-alt'></i> File đính kèm </a>" + del + "</p>";
                }
            }
            return str;



            //
            //_condition = new Dictionary<string, object>();
            //_condition.Add("IVANBAN", ivanban);
            //string str = "";
            //var filevb = _vanban.GetBy_List_Vanban_file(_condition).ToList();
            //foreach (var x in filevb)
            //{
            //    if (x.CURL != null)
            //    {
            //        string del = "<a href='javascript:void(0)' onclick=\"DeleteFile(" + x.IFILE + ", '/Vanban/Ajax_Delete_VanBan_file')\" class='btn btn-danger'><i class='icon-remove'></i></a>";
            //        str += "<p id='file_" + x.IFILE + "'><a href='" + x.CURL + "'  class='btn btn-success' ><i class='icon-download-alt'></i> File đính kèm </a>" + del + "</p>";
            //    }
            //}
            //return str;
        }

        public TRACKING Tracking_vanban(int iUser, int iVanBan, string action)
        {
            TRACKING t = new TRACKING();
            t.IUSER = iUser;
            t.IDON = 0; t.IKIENNGHI = 0;
            t.IVANBAN = iVanBan; t.ITONGHOP = 0;
            t.DDATE = DateTime.Now;
            t.CACTION = action;
            return _base.InsertTracking(t);

        }
        public ActionResult Moicapnhat(int page =1 )
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                string tukhoa = "";
                if (Request["q"] != null)
                {
                    tukhoa = Request["q"];
                }
                UserInfor u_info = GetUserInfor();
                if (!_base.ActionMulty_Redirect_("39,40", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
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
                int imadonvi = -1;
                if(Request["iDonvi"] != null && Request["iDonvi"] != "")
                {
                    imadonvi = Convert.ToInt32(Request["iDonvi"]);
                }
                int iLinhVuc = -1;
                if (Request["iLinhVuc"] != null && Request["iLinhVuc"] != "")
                {
                    iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                }
                int iloaivb = -1;
                if (Request["iLoai"] != null && Request["iLoai"] != "")
                {
                    iloaivb = Convert.ToInt32(Request["iLoai"]);
                }
                int iKyhop = 0;
                if (Request["iKyhop"] != null && Request["iKyhop"] != "")
                {
                    iKyhop = Convert.ToInt32(Request["iKyhop"]);
                }
                int itrangthai = 0;
                if (Request["iTrangthai"] != null && Request["iTrangthai"] != "")
                {
                    itrangthai = Convert.ToInt32(Request["iTrangthai"]);
                }
                var thongtinvanban = thutuc.VANBAN("PKG_VANBAN.PRC_PHANTRANG_VANBAN", dTuNgay, dDenNgay, tukhoa, imadonvi, iLinhVuc, itrangthai, iloaivb, iKyhop, page, post_per_page);
                if (thongtinvanban != null && thongtinvanban.Count() > 0)
                {
                    ViewData["list"] = vb.LIST_MOICAPNHAT(thongtinvanban, u_info.tk_action.iUser);
                    ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)thongtinvanban.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='4' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }
               // ViewData["list"] = vb.List_Vanban(u_info.tk_action.iUser, page);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Chương trình mới cập nhật");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Change_LinhVuc_By_ID_DonVi(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id_donvi = Convert.ToInt32(fc["id"]);
                Response.Write(Get_Option_LinhVuc_By_ID_CoQuan(id_donvi, 0));
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Lấy Option lĩnh vực của Đơn vị tương ứng");
                throw;
            }
        }
      
       
        public string Get_Option_LinhVuc_By_ID_CoQuan(int id_coquan = 0, int id_linhvuc = 0)
        {
            string str = "<option value='0'> - - - Chưa xác định</option>";
            if (id_coquan == 0)
            {
                return str;
            }
            else
            {
                var linhvuc = _vanban.GetAll_LinhVuc_CoQuan_By_IDCoQuan(id_coquan);
                return str  + vb.Option_LinhVuc_CoQuan(linhvuc, id_linhvuc);
            }
        }
        public string Get_Option_KyHop(int iKyHop = 0)
        {
            List<QUOCHOI_KHOA> khoa = _kiennghi.GetAll_KhoaHop().OrderBy(x => x.DBATDAU).ToList();
            List<QUOCHOI_KYHOP> kyhop = _kiennghi.GetAll_KyHop().OrderBy(x => x.DBATDAU).ToList();

            return "" + kn.Option_Khoa_KyHop(khoa, kyhop, iKyHop);
        }
        // Danh muc thêm mới văn bản Hiếu làm
        public ActionResult Themmoi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                if (!_base.Action_(39, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("vb_themmoi");
                func.SetCookies("url_return", Request.Url.AbsoluteUri);

                int iUser = u_info.tk_action.iUser;
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                List<LINHVUC_COQUAN> linhvuc_coquan = _thietlap.Get_Linhvuc_Coquan();
                ViewData["opt-linhvuc"] = tl.Option_linhvuccoquan(linhvuc_coquan, coquan, iUser);
                ViewData["opt-loai"] = vb.Option_Loai(0);
             
                List<VB_DONVI_VANBAN> vanbandonvi = _vanban.Get_Vanban_donvi();
                ViewData["list_group"] = vb.List_CheckBox_Donvi2(0, coquan, vanbandonvi);
                ViewData["opt_donvi"] = vb.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, 0);

               
                ViewData["opt-kyhop"] = Get_Option_KyHop();
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thêm mới văn bản");
                return View("../Home/Error_Exception");
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoi(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
               
                UserInfor u_info = GetUserInfor();
                if (!CheckTokenAction("vb_themmoi")) { Response.Redirect("/Home/Login"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    HttpPostedFileBase file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/"); return null; }
                    }
                }
                int iUser = u_info.tk_action.iUser;
                VB_VANBAN vb = new VB_VANBAN();
                vb.CTIEUDE = func.RemoveTagInput(fc["cTieude"]).Trim();
                vb.CTRICHYEU = func.RemoveTagInput(fc["cTrichyeu"]).Trim();
                vb.DDATECREATE = DateTime.Now;
                vb.DDATE = Convert.ToDateTime(func.ConvertDateToSql(fc["dDate"]));
                vb.ILINHVUC = Convert.ToInt32(fc["iLinhvuc"]);
                vb.ILOAI = Convert.ToInt32(fc["iLoai"]);
                vb.IKYHOP = Convert.ToInt32(fc["iKyhop"]);
                vb.IHIENTHI = 0;
                vb.IUSER = id_user();
                vb.IUSERDUYET = 0;
                _vanban.Insert_Vanban(vb);
                int id = (int)vb.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    HttpPostedFileBase file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        VB_FILE_VANBAN fvb = new VB_FILE_VANBAN();

                        fvb.CURL = UploadFile(file);
                        fvb.IVANBAN = vb.IVANBAN;
                        _vanban.Insert_Vanban_file(fvb);

                    }
                }
                int idonvi = Convert.ToInt32(fc["iDonvi"]);
                if (idonvi != -1)
                {
                    VB_DONVI_VANBAN a = new VB_DONVI_VANBAN();
                    a.IVANBAN = id;
                    a.IDONVI = idonvi;
                    _vanban.Insert_Vanban_donvi(a);
                }
                else
                {
                    string group = fc["group"];
                    if (group != "") { group = "," + group + ","; }

                    string arr = fc["action"];
                    if (arr != null)
                    {
                        foreach (var x in arr.Split(','))
                        {
                            if (x != "")
                            {
                                VB_DONVI_VANBAN a = new VB_DONVI_VANBAN();
                                a.IVANBAN = id;
                                a.IDONVI = Convert.ToInt32(x);
                                _vanban.Insert_Vanban_donvi(a);
                            }
                        }
                    }

                }
                Tracking_vanban(id_user(), (int)vb.IVANBAN, "Thêm văn bản số " + vb.CTIEUDE);
                Response.Redirect("/Vanban/Moicapnhat/#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thêm mới văn bản");
                return View("../Home/Error_Exception");
            }
        }
        // End danh mục thêm mới văn bản
        // mục duyệt 
        public ActionResult DuyetVanBan(string id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
             
                int ID = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                VB_VANBAN vanban = _vanban.GetBy_VanbanID(ID);
                UserInfor u_info = GetUserInfor();
                vanban.IHIENTHI = 1;
                vanban.IUSERDUYET = u_info.tk_action.iUser;
                _vanban.Update_Vanban(vanban);
                Tracking_vanban(id_user(), ID, "Duyệt văn bản số: " + vanban.CTIEUDE);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Duyệt văn bản");
                throw;
            }
        }
        public ActionResult DuyetVanBan_Moi(string id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
            
                int ID = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                VB_VANBAN vanban = _vanban.GetBy_VanbanID(ID);
                vanban.IHIENTHI = 0;
                vanban.IUSERDUYET = id_user();
                _vanban.Update_Vanban(vanban);
                Tracking_vanban(id_user(), ID, "Chuyển về mới cập nhật văn bản số: " + vanban.CTIEUDE);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Duyệt văn bản");
                //return null;
                throw;
            }
        }
        public ActionResult DuyetVanBan_QuaHan(string id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
                
                int ID = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                VB_VANBAN vanban = _vanban.GetBy_VanbanID(ID);
                vanban.IHIENTHI = -1;
                vanban.IUSERDUYET = id_user();
                _vanban.Update_Vanban(vanban);
                Tracking_vanban(id_user(), ID, "Chuyển quá hạn văn bản số: " + vanban.CTIEUDE);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Duyệt văn bản quá hạn");
                //return null;
                throw;
            }
        }
        public ActionResult Duyet(int page= 1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                //....

                
                UserInfor u_info = GetUserInfor();
                if (!_base.ActionMulty_Redirect_("39,40", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
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
                int imadonvi = -1;
                if (Request["iDonvi"] != null && Request["iDonvi"] != "")
                {
                    imadonvi = Convert.ToInt32(Request["iDonvi"]);
                }
                int iLinhVuc = -1;
                if (Request["iLinhVuc"] != null && Request["iLinhVuc"] != "")
                {
                    iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                }
                int iloaivb = -1;
                if (Request["iLoai"] != null && Request["iLoai"] != "")
                {
                    iloaivb = Convert.ToInt32(Request["iLoai"]);
                }
                int iKyhop = 0;
                if (Request["iKyhop"] != null && Request["iKyhop"] != "")
                {
                    iKyhop = Convert.ToInt32(Request["iKyhop"]);
                }
                int itrangthai = 1;
                if (Request["iTrangthai"] != null && Request["iTrangthai"] != "")
                {
                    itrangthai = Convert.ToInt32(Request["iTrangthai"]);
                }
                var thongtinvanban = thutuc.VANBAN("PKG_VANBAN.PRC_PHANTRANG_VANBAN", dTuNgay, dDenNgay, tukhoa, imadonvi, iLinhVuc, itrangthai, iloaivb, iKyhop, page, post_per_page);
                if (thongtinvanban != null && thongtinvanban.Count() > 0)
                {
                    ViewData["list"] = vb.LIST_VANBANDADUYET(thongtinvanban, u_info.tk_action.iUser);
                    ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)thongtinvanban.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='4' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }
                // ViewData["list"] = vb.List_Vanban(u_info.tk_action.iUser, page);
                return View();
            
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Duyệt văn bản");
                //return null;
                throw;
            }
        }
        // duyệt văn bản
        // sửa xóa 
        public ActionResult ChinhSua(string id)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                //....
                UserInfor u_info = GetUserInfor();
                if (!_base.ActionMulty_Redirect_("39", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; };
                int ID = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                SetTokenAction("vb_sua", ID);
                ViewData["id"] = id;
                VB_VANBAN vanban = _vanban.GetBy_VanbanID(ID);
                ViewData["vanban"] = vanban;
                SetTokenAction("vb_sua", ID);
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                List<LINHVUC_COQUAN> linhvuc_coquan = _thietlap.Get_Linhvuc_Coquan();
                List<VB_DONVI_VANBAN> vanbandonvi = _vanban.Get_Vanban_donvi();
                ViewData["opt-linhvuc"] = tl.Option_linhvuccoquan(linhvuc_coquan, coquan, u_info.tk_action.iUser,(int)vanban.ILINHVUC);
                ViewData["opt-loai"] = vb.Option_Loai((int)vanban.ILOAI);
                ViewData["list_group"] = vb.List_CheckBox_Donvi2((int)vanban.IVANBAN, coquan, vanbandonvi);
                ViewData["cTieude"] = vanban.CTIEUDE;
                ViewData["cTrichyeu"] = vanban.CTRICHYEU;
                ViewData["dDate"] = func.ConvertDateVN(vanban.DDATE.ToString());
                // int idv = 0;
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", ID);
       
                if (_vanban.GetBy_List_Vanban_donvi(_condition).Count() > 1)
                {
                    ViewData["idonvi"] = 2;
                ViewData["opt_donvi"] = vb.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, 0);
                }
                else if (_vanban.GetBy_List_Vanban_donvi(_condition).Count() == 1)
                {
                    int idonvi = Convert.ToInt32(_vanban.GetBy_List_Vanban_donvi(_condition).SingleOrDefault().IDONVI);
                    ViewData["opt_donvi"] = vb.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, idonvi);
                    ViewData["idonvi"] = 1;
                }
                // lay ra danh sach don vi ban hanh van ban roi dua ra day thu
                ViewData["group"] = "";
                ViewData["XoaFile"] = read_fileVB((int)vanban.IVANBAN);
                ViewData["opt-kyhop"] = Get_Option_KyHop((int)vanban.IKYHOP);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Chỉnh sửa văn bản ");
                return View("../Home/Error_Exception");
            }
        }


        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Chinhsua(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
                
                int ID = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("vb_sua", ID)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file)) { Response.Redirect("/Home/Error/"); return null; }
                    }
                }
                int iUser = id_user();
                VB_VANBAN vb = _vanban.GetBy_VanbanID(ID);
                vb.CTIEUDE = func.RemoveTagInput(fc["cTieude"]);
                vb.CTRICHYEU = func.RemoveTagInput(fc["cTrichyeu"]);
                vb.DDATE = Convert.ToDateTime(func.ConvertDateToSql(fc["dDate"]));
                vb.ILINHVUC = Convert.ToInt32(fc["iLinhvuc"]);
                vb.ILOAI = Convert.ToInt32(fc["iLoai"]);
                vb.IKYHOP = Convert.ToInt32(fc["iKyhop"]);
                _vanban.Update_Vanban(vb);
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", ID);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();

                int id = (int)vb.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        VB_FILE_VANBAN fvb = new VB_FILE_VANBAN();
                        fvb.CURL = UploadFile(file);
                        fvb.IVANBAN = vb.IVANBAN;
                        _vanban.Insert_Vanban_file(fvb);

                    }
                }

                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", ID);
                var listvanbandonvi = _vanban.GetBy_List_Vanban_donvi(_condition).ToList();
                foreach (var x in listvanbandonvi)
                {
                    VB_DONVI_VANBAN xoadv = _vanban.GetBy_Vanban_donvi((int)x.ID);
                    _vanban.Delete_Vanban_donvi(xoadv);
                }
                string group = fc["group"];
                if (group != "") { group = "," + group + ","; }
                string arr = fc["action"];
                if (arr != null)
                {
                    foreach (var x in arr.Split(','))
                    {
                        if (x != "")
                        {
                            VB_DONVI_VANBAN a = new VB_DONVI_VANBAN();
                            a.IVANBAN = id;
                            a.IDONVI = Convert.ToInt32(x);
                            _vanban.Insert_Vanban_donvi(a);
                        }
                    }
                }
                Tracking_vanban(id_user(), ID, "Sửa văn bản số " + vb.CTIEUDE);
                Response.Redirect(Request.Cookies["url_return"].Value);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Chỉnh sửa văn bản ");
                return View("../Home/Error_Exception");
            }


        }
        public ActionResult Xoa(string id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
             
                int ID = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                VB_VANBAN vanban = _vanban.GetBy_VanbanID(ID);
                Tracking_vanban(id_user(), ID, "Xóa văn bản số " + vanban.CTIEUDE);
                _vanban.Delete_Vanban(vanban);
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", ID);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                foreach (var x in list)
                {
                    VB_FILE_VANBAN xoafile = _vanban.GetBy_Vanban_fileID((int)x.IFILE);
                    _vanban.Delete_Vanban_file(xoafile);
                }
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa văn bản ");
                //return null;
                throw;
            }

        }
        //end 
        public ActionResult Tracuu(int page = 1)
        {
            //....
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {


                UserInfor u_info = GetUserInfor();
                if (!_base.ActionMulty_Redirect_("38", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; };
                int iUser = u_info.tk_action.iUser;
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                List<LINHVUC_COQUAN> linhvuc_coquan = _thietlap.Get_Linhvuc_Coquan();
                ViewData["opt-linhvuc"] = tl.Option_linhvuccoquan(linhvuc_coquan, coquan, iUser);
                ViewData["opt-loai"] = vb.Option_Loai(0);
                ViewData["otp-donvi"] = tl.OptionCoQuan_TreeList();
                ViewData["opt-kyhop"] = Get_Option_KyHop();
                if (!_base.ActionMulty_Redirect_("39,40", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
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
                int imadonvi = -1;
                if (Request["iDonvi"] != null && Request["iDonvi"] != "")
                {
                    imadonvi = Convert.ToInt32(Request["iDonvi"]);
                }
                int iLinhVuc = -1;
                if (Request["iLinhVuc"] != null && Request["iLinhVuc"] != "")
                {
                    iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                }
                int iloaivb = -1;
                if (Request["iLoai"] != null && Request["iLoai"] != "")
                {
                    iloaivb = Convert.ToInt32(Request["iLoai"]);
                }
                int iKyhop = 0;
                if (Request["iKyhop"] != null && Request["iKyhop"] != "")
                {
                    iKyhop = Convert.ToInt32(Request["iKyhop"]);
                }
                int itrangthai = 2;
                if (Request["iTrangthai"] != null && Request["iTrangthai"] != "")
                {
                    itrangthai = Convert.ToInt32(Request["iTrangthai"]);
                }
                var thongtinvanban = thutuc.VANBAN("PKG_VANBAN.PRC_PHANTRANG_VANBAN", dTuNgay, dDenNgay, tukhoa, imadonvi, iLinhVuc, itrangthai, iloaivb, iKyhop, page, post_per_page);
                if (thongtinvanban != null && thongtinvanban.Count() > 0)
                {
                    if (Request["iTrangthai"] == null && Request["iKyhop"] == null && Request["iLoai"] == null && Request["iLinhVuc"] == null)
                    {
                        ViewData["phantrang"] = "";
                        ViewData["list"] = "<tr><td colspan='4' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                    }
                    else
                    {
                        ViewData["list"] = vb.LIST_TRACUU(thongtinvanban, u_info.tk_action.iUser);
                        ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)thongtinvanban.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    }
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='4' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tra cứu văn bản ");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult Ajax_Tracuu_result()
        {
            try
            {

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!_base.ActionMulty_Redirect_("39,40", u_info.tk_action)) { return null; }

                string data = "";
                string key = func.RemoveTagInput(Request["tukhoa"]);
                string dTuNgay = DateTime.MinValue.ToString("dd-MMM-yyyy");
                string dDenNgay = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                if (Request["dTuNgay"] != null && Request["dTuNgay"].ToString() != "")
                {
                    dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));

                }
                if (Request["dDenNgay"] != null && Request["dDenNgay"].ToString() != "")
                {
                    dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));

                }
                VB_VANBAN d = new VB_VANBAN();
                d.ILOAI = Convert.ToInt32(Request["iLoai"]);
                d.ILINHVUC = Convert.ToInt32(Request["iLinhVuc"]);
                d.IKYHOP = Convert.ToInt32(Request["iKyhop"]);
                d.CTRICHYEU = key;
                d.CTIEUDE = key;
                d.IHIENTHI = Convert.ToInt32(Request["iTrangthai"]);
                int iDonvi = Convert.ToInt32(Request["iDonvi"]);
                var list1 = _vanban.Get_List_Vanban_Sql(d, iDonvi, dTuNgay, dDenNgay).ToList();
                int count = 0;
                foreach (var x in list1)
                {
                    string id_encr = HashUtil.Encode_ID(x.IVANBAN.ToString(), Request.Cookies["url_key"].Value);
                    // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                    string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title='' class='trans_func' ><i class='icon-pencil'></i></a> ";
                    string chuyenvethemmoi = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản mới cập nhật' rel='tooltip' title='' onclick=\"UpdateTrangthai_ChuyenMoi('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_Moi','Bạn có muốn chuyển văn bản về mới cập nhật')\" class='trans_func' ><i class='icon-signout'></i></a> ";
                    string chuyenvehethan = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản hết hạn' rel='tooltip' title='' onclick=\"UpdateTrangthai_HetHieuLuc('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_QuaHan','Bạn có muốn chuyển văn bản về quá hạn')\" class='trans_func' ><i class='icon-time'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Hủy' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa','Bạn có muốn xóa văn bản " + Server.HtmlEncode(x.CTIEUDE) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                    //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                    string duyet = " <a href=\"javascript:void()\" data-original-title='Duyệt' rel='tooltip' title='' onclick=\"UpdateTrangthai_Duyet_VanBan('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan','Bạn có muốn duyệt văn bản " + Server.HtmlEncode(x.CTIEUDE) + "?')\"  class='trans_func' ><i class='icon-ok'></i></a> ";
                    // del = "";
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IVANBAN", x.IVANBAN);
                    var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                    string file = "";
                    if (list.Count() != null && list.Count() > 0)
                    {
                        file = vb.File_View((int)x.IVANBAN);
                    }
                    count++;
                    string loaivanban = "Chưa xác định";
                    if (x.ILOAI != null && _vanban.Get_ListVB().Where(v => v.ILOAI != null && v.ILOAI == x.ILOAI).Count() > 0)
                    {
                        loaivanban = _vanban.GetBy_Loaivanban((int)x.ILOAI).CTEN;
                    }
                    string linhvuc = "Chưa xác định";
                    if (x.ILINHVUC != null && _thietlap.Get_Linhvuc().Where(v => v.ILINHVUC != null && v.ILINHVUC == x.ILINHVUC).Count() > 0)
                    {
                        linhvuc = _thietlap.GetBy_Linhvuc_CoquanID((int)x.ILINHVUC).CTEN;
                    }
                    if (d.IHIENTHI == -1)
                    {
                        data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                       "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                       "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                       "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                       func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                       "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'></td></tr>";
                    }
                    else if (d.IHIENTHI == 1)
                    {
                        data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                      "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                      "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                      "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                      func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                      "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + chuyenvethemmoi + chuyenvehethan + "</p></td></tr>";
                    }
                    else if (d.IHIENTHI == 0)
                    {

                        data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                            "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                            "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                            "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                            func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                            "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + duyet + edit + del + "</p></td></tr>";
                    }
                    else
                    {
                        if (x.IHIENTHI == 1)
                        {
                            data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                      "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                      "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                      "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                      func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                      "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + chuyenvethemmoi + chuyenvehethan + "</p></td></tr>";
                        }
                        else if (x.IHIENTHI == 0)
                        {
                            data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                           "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                           "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                           "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                           func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                           "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + duyet + edit + del + "</p></td></tr>";
                        }
                        else if (x.IHIENTHI == -1)
                        {

                            data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                    "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                    "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                    "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                    func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                    "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'></td></tr>";
                        }
                        else
                        {
                            data += "";
                        }
                    }



                }
                if (data != "")
                {
                    ViewData["data"] = "<tr><td colspan='4' class='alert tcenter alert-info'>Có " + list1.Count()+ " kết quả tìm kiếm</td></tr>" + data;
                }
                else
                {
                    ViewData["data"] = "<tr><td colspan='4' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào!</td></tr>";
                }

                return PartialView("../Ajax/Vanban/Tracuu");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tra cứu văn bản ");
                throw;
            }
          
        }
        public ActionResult Ajax_Tracuu_result2()
        {
            try
            {

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!_base.ActionMulty_Redirect_("39,40", u_info.tk_action)) { return null; }

                string data = "";
                string key = func.RemoveTagInput(Request["tukhoa"]);
                string dTuNgay = DateTime.MinValue.ToString("dd-MMM-yyyy");
                string dDenNgay = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                if (Request["dTuNgay"] != null && Request["dTuNgay"].ToString() != "")
                {
                    dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));

                }
                if (Request["dDenNgay"] != null && Request["dDenNgay"].ToString() != "")
                {
                    dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));

                }
                VB_VANBAN d = new VB_VANBAN();
                d.ILOAI = Convert.ToInt32(Request["iLoai"]);
                d.ILINHVUC = Convert.ToInt32(Request["iLinhVuc"]);
                d.IKYHOP = Convert.ToInt32(Request["iKyhop"]);
                d.CTRICHYEU = key;
                d.CTIEUDE = key;
                d.IHIENTHI = Convert.ToInt32(Request["iTrangthai"]);
                int iDonvi = Convert.ToInt32(Request["iDonvi"]);
                var list1 = _vanban.Get_List_Vanban_Sql(d, iDonvi, dTuNgay, dDenNgay).ToList();
                int count = 0;
                foreach (var x in list1)
                {
                    string id_encr = HashUtil.Encode_ID(x.IVANBAN.ToString(), Request.Cookies["url_key"].Value);
                    // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                    string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title='' class='trans_func' ><i class='icon-pencil'></i></a> ";
                    string chuyenvethemmoi = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản mới cập nhật' rel='tooltip' title='' onclick=\"UpdateTrangthai_ChuyenMoi('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_Moi','Bạn có muốn chuyển văn bản về mới cập nhật')\" class='trans_func' ><i class='icon-signout'></i></a> ";
                    string chuyenvehethan = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản hết hạn' rel='tooltip' title='' onclick=\"UpdateTrangthai('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_QuaHan','Bạn có muốn chuyển văn bản về quá hạn')\" class='trans_func' ><i class='icon-time'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Hủy' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa','Bạn có chắc xóa Quốc tịch " + Server.HtmlEncode(x.CTIEUDE) + "  khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                    //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                    string duyet = " <a href=\"javascript:void()\" data-original-title='Duyệt ban hành văn bản này' rel='tooltip' title='' onclick=\"UpdateTrangthai_Duyet_VanBan('id=" + id_encr + "','/Vanban/DuyetVanBan')\"  class='trans_func' ><i class='icon-ok'></i></a> ";
                    // del = "";
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IVANBAN", x.IVANBAN);
                    var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                    string file = "";
                    if (list.Count() != null && list.Count() > 0)
                    {
                        file = vb.File_View((int)x.IVANBAN);
                    }

                    count++;
                    string loaivanban = "Chưa xác định";
                    if (x.ILOAI != null && _vanban.Get_ListVB().Where(v => v.ILOAI != null && v.ILOAI == x.ILOAI).Count() > 0)
                    {
                        loaivanban = _vanban.GetBy_Loaivanban((int)x.ILOAI).CTEN;
                    }
                    string linhvuc = "Chưa xác định";
                    if (x.ILINHVUC != null && _thietlap.Get_Linhvuc().Where(v => v.ILINHVUC != null && v.ILINHVUC == x.ILINHVUC).Count() > 0)
                    {
                        linhvuc = _thietlap.GetBy_Linhvuc_CoquanID((int)x.ILINHVUC).CTEN;
                    }
                    if (d.IHIENTHI == -1)
                    {
                        data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                       "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                       "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                       "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                       func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                       "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'></td></tr>";
                    }
                    else if (d.IHIENTHI == 1)
                    {
                        data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                      "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                      "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                      "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                      func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                      "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + chuyenvethemmoi + chuyenvehethan + "</p></td></tr>";
                    }
                    else if (d.IHIENTHI == 0)
                    {

                        data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                            "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                            "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                            "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                            func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                            "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + duyet + edit + del + "</p></td></tr>";
                    }
                    else
                    {
                        if (x.IHIENTHI == 1)
                        {
                            data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                      "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                      "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                      "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                      func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                      "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + chuyenvethemmoi + chuyenvehethan + "</p></td></tr>";
                        }
                        else if (x.IHIENTHI == 0)
                        {
                            data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                           "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                           "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                           "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                           func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                           "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + duyet + edit + del + "</p></td></tr>";
                        }
                        else if (x.IHIENTHI == -1)
                        {

                            data += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                    "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + Server.HtmlEncode(x.CTIEUDE) +
                    "</strong></span><span > Vv " + Server.HtmlEncode(x.CTRICHYEU) +
                    "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                    func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + vb.Row_vanban_list_donvi((int)x.IVANBAN) +
                    "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'></td></tr>";
                        }
                        else
                        {
                            data += "";
                        }
                    }



                }
                if (data != "")
                {
                    Response.Write(data);
                }
                else
                {
                    Response.Write("<tr><td colspan='4' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào!</td></tr>");
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tra cứu văn bản ");
                throw;
            }  
        }

        public string Ajax_Delete_VanBan_file(string id)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                int ifile = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                var thongtinfile = _vanban.GetBy_Vanban_fileID(ifile);
                _vanban.Delete_Vanban_file(thongtinfile);
                Response.Write(1);
                return null;

            }
            catch (Exception e)
            {
                log.Log_Error(e, "xóa file văn bản ");
                //return null;
                throw;
            }


        }
        // Bổ sung 
        public ActionResult Quahan(int page =1)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout"); return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                if (!_base.ActionMulty_Redirect_("39,40", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
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
                int imadonvi = -1;
                if (Request["iDonvi"] != null && Request["iDonvi"] != "")
                {
                    imadonvi = Convert.ToInt32(Request["iDonvi"]);
                }
                int iLinhVuc = -1;
                if (Request["iLinhVuc"] != null && Request["iLinhVuc"] != "")
                {
                    iLinhVuc = Convert.ToInt32(Request["iLinhVuc"]);
                }
                int iloaivb = -1;
                if (Request["iLoai"] != null && Request["iLoai"] != "")
                {
                    iloaivb = Convert.ToInt32(Request["iLoai"]);
                }
                int iKyhop = 0;
                if (Request["iKyhop"] != null && Request["iKyhop"] != "")
                {
                    iKyhop = Convert.ToInt32(Request["iKyhop"]);
                }
                int itrangthai = -1;
                if (Request["iTrangthai"] != null && Request["iTrangthai"] != "")
                {
                    itrangthai = Convert.ToInt32(Request["iTrangthai"]);
                }
                var thongtinvanban = thutuc.VANBAN("PKG_VANBAN.PRC_PHANTRANG_VANBAN", dTuNgay, dDenNgay, tukhoa, imadonvi, iLinhVuc, itrangthai, iloaivb, iKyhop, page, post_per_page);
                if (thongtinvanban != null && thongtinvanban.Count() > 0)
                {
                    ViewData["list"] = vb.LIST_QUAHAN(thongtinvanban, u_info.tk_action.iUser);
                    ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)thongtinvanban.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='4' class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }
                // ViewData["list"] = vb.List_Vanban(u_info.tk_action.iUser, page);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Delete file");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Vanban_search(float type)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
                UserInfor u_info = GetUserInfor();
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                List<LINHVUC_COQUAN> linhvuc_coquan = _thietlap.Get_Linhvuc_Coquan();
                ViewData["opt-linhvuc"] = tl.Option_linhvuccoquan(linhvuc_coquan, coquan, u_info.tk_action.iUser);
                ViewData["opt-loai"] = vb.Option_Loai(0);
                
                ViewData["otp-donvi"] = tl.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, 0);
                
                ViewData["opt-kyhop"] = Get_Option_KyHop();
                if (type == 0)
                {
                    ViewData["trangthai"] = "<option value='0'>- - - Văn bản đang soạn thảo</option>";
                }
                else if (type == 1)
                {
                    ViewData["trangthai"] = "<option value='1'>- - - Văn bản đã ban hành</option>";
                }
                else if (type == -1)
                {
                    ViewData["trangthai"] = "<option value='-1'>- - - Văn bản hết hiệu lực</option>";
                }
                else
                {
                    ViewData["trangthai"] = "<option value='0'>- - - Văn bản đang soạn thảo</option><option value='1'>- - - Văn bản đã ban hành</option><option value='-1'>- - - Văn bản hết hiệu lực</option>";
                }
                return PartialView("../Ajax/Vanban/Vanban_formsearch");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "văn bản tra cứu ");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_VanbanDuyet_Timkiem(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                string str = "";
                //int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                string ctentimkiem = func.RemoveTagInput(fc["ip_noidung"]).Trim();
                var thongtintimkiem = _vanban.GetList_SQLTimkiemten(ctentimkiem).Where(x => x.IHIENTHI == 1).ToList();
                int dem = 1;
                foreach (var x in thongtintimkiem)
                {
                    str += vb.List_Vanban_search(id_user(), (int)x.IVANBAN, dem, Request.Cookies["url_key"].Value, 1);
                    dem++;
                   
                }
                Response.Write("<tr><td colspan='4' class='alert tcenter alert-info'>Có " + (dem-1)+ " kết quả tìm kiếm</td></tr>" + str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "văn bản tra cứu ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Vanbanxoanthao_Timkiem(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                string str = "";
                //int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                string ctentimkiem = func.RemoveTagInput(fc["ip_noidung"]).Trim();
                var thongtintimkiem = _vanban.GetList_SQLTimkiemten(ctentimkiem).Where(x => x.IHIENTHI == 0).ToList();
                int dem = 1;
                foreach (var x in thongtintimkiem)
                {
                    str += vb.List_Vanban_xoanthao_search(id_user(), (int)x.IVANBAN, dem, Request.Cookies["url_key"].Value, 0);
                    dem++;
                }
                Response.Write("<tr><td colspan='4' class='alert tcenter alert-info'>Có " + (dem - 1) + " kết quả tìm kiếm</td></tr>" + str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "văn bản tra cứu ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Vanbanhethan_Timkiem(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                string str = "";
                //int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                string ctentimkiem = func.RemoveTagInput(fc["ip_noidung"]).Trim();
                var thongtintimkiem = _vanban.GetList_SQLTimkiemten(ctentimkiem).Where(x => x.IHIENTHI == -1).ToList();
                int dem = 1;
                foreach (var x in thongtintimkiem)
                {
                    str += vb.List_Vanban_QuaHan_search(id_user(), (int)x.IVANBAN, dem, Request.Cookies["url_key"].Value, -1);
                    dem++;
                }
                Response.Write("<tr><td colspan='4' class='alert tcenter alert-info'>Có " + (dem - 1) + " kết quả tìm kiếm</td></tr>" + str);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "văn bản tra cứu ");
                //return null;
                throw;
            }
        }



    }
}
