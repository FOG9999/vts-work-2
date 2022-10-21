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
using Spire.Xls;
using System.Data;
using Entities.Objects;
using Newtonsoft.Json;
using Utilities.Constants;
using Utilities.Enums;
using Microsoft.Ajax.Utilities;

namespace KienNghi.Controllers
{
    public class ThietlapController : BaseController
    {
        //
        // GET: /Thietlap/
        BaseBusineess base_buss = new BaseBusineess();
        KiennghiBusineess _kiennghi = new KiennghiBusineess();
        Thietlap tl = new Thietlap();
        Base _base = new Base();
        Vanban vb = new Vanban();
        Funtions func = new Funtions();
        Log log = new Log();
        Kiennghi_cl kn = new Kiennghi_cl();
        Thietlaplist thutuc = new Thietlaplist(); 
        Dictionary<string, object> _condition;
        Base base_appcode = new Base();
        /// <summary>
        /// Danh mục khóa
        /// </summary>
        ThietlapBusineess _thietlap = new ThietlapBusineess();
        public string Get_Option_DonViThamQuyen(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            return "<option value='0'>Chọn đơn vị thẩm quyền</option>" + kn.OptionCoQuanXuLy(coquan, 0, 0, iDonVi, 0);
        }
        public string Get_Option_DonVi(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            return "<option value='0'>Chọn đơn vị thẩm quyền</option>" + tl.OptionCoQuan_Update(coquan, 0, 0, iDonVi, 0);
        }
        public string Get_Option_LinhVucCha(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            return "<option value='0'>Chọn lĩnh vực cha</option>" + tl.OptionCoQuan_Update(coquan, 0, 0, iDonVi, 0);
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
        public string Get_Option_LoaiDoiTuong(int iLoai)
        {
            string str = "";
            UserInfor u_info = GetUserInfor();
            if(iLoai == -1)
            {
                if (u_info.user_login.ITYPE == 4)
                {
                    str = "<option value='0' selected>" + StringEnum.GetStringValue(Loai_DoiTuong.DBQH) + "</option><option value='1'>" + StringEnum.GetStringValue(Loai_DoiTuong.HDND) + "</option>";
                }
                else
                    str = "<option value='0'>" + StringEnum.GetStringValue(Loai_DoiTuong.DBQH) + "</option><option selected value='1'>" + StringEnum.GetStringValue(Loai_DoiTuong.HDND) + "</option>";
            }
            else
            {
                if(iLoai == 0)
                    str = "<option value='0' selected>" + StringEnum.GetStringValue(Loai_DoiTuong.DBQH) + "</option><option value='1'>" + StringEnum.GetStringValue(Loai_DoiTuong.HDND) + "</option>";
                else
                    str = "<option value='0'>" + StringEnum.GetStringValue(Loai_DoiTuong.DBQH) + "</option><option selected value='1'>" + StringEnum.GetStringValue(Loai_DoiTuong.HDND) + "</option>";
            }
            return str;
        }
        public ActionResult Khoa()
        {

            try
            {
                //....
                if (!CheckAuthToken_Api()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(21, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                string url_key = _base.Set_Url_keycookie();
                // Mặc định
                int iLoai = 1;
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                    iLoai = 0;
                
                if (Request["iDoiTuong"] != null)
                {
                    iLoai = Convert.ToInt32(Request["iDoiTuong"]);
                    
                }
                List<QUOCHOI_KHOA> khoa = _thietlap.Get_List_Quochoi_Khoa().Where(x => x.IDELETE == 0).ToList();
                ViewData["list"] = tl.List_Khoa(khoa, 0, url_key);
                ViewData["opt-khoa"] = tl.OptionKhoa_Search(url_key);
                ViewData["opt-doituong"] = Get_Option_LoaiDoiTuong(iLoai);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách Khóa");
                return View("../Home/Error_Exception");
            }

        }
         public string Get_Option_Nam(int year = 0)
        {
            string str = "";
            int Ynow = DateTime.Now.Year;
            string select = "";
            for (int i = Ynow- 20; i<= Ynow + 20; i++)
            {
                if (i == year)
                    select = "selected ";
                str += "<option " + select + "value='" + i + "'>Năm " + i + "</option>";
                select = "";
            }
            return str;
        }
        public ActionResult Ajax_Khoa_add()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(21, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_themkhoa");
                ViewData["opt-nam"] = Get_Option_Nam();
                return PartialView("../Ajax/Thietlap/Khoa_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm mới khóa họp");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Khoa_insert(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(21, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_List_Quochoi_Khoa().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim()).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_themkhoa"))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    QUOCHOI_KHOA q = new QUOCHOI_KHOA();
                    q.ILOAI = Decimal.Parse(fc["iLoai"]);
                    string ten = func.RemoveTagInput(fc["cTen"]);
                    q.CTEN = ten;
                    int batDau = Convert.ToInt32(fc["dBatDau"]);
                    int ketThuc = Convert.ToInt32(fc["dKetThuc"]);
                    DateTime yBatDau = new DateTime(batDau, 1, 1);
                    DateTime yKetThuc = new DateTime(ketThuc, 1, 1);
                    q.DBATDAU = yBatDau;
                    q.DKETTHUC = yKetThuc;
                    q.IMACDINH = 0;
                    q.IHIENTHI = 1;
                    q.IVITRI = 1;
                    q.CCODE = fc["cCode"];
                    q.IDELETE = 0;
                    _thietlap.Insert_QuocHoi_Khoa(q);
                    int iUserLogin = u_info.tk_action.iUser;
                    Tracking(iUserLogin, "Thêm khóa: " + q.CTEN);
                    Response.Write(1);
                    
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm mới khóa họp");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Khoa_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(21, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_suakhoa", id);
                QUOCHOI_KHOA editing = _thietlap.Get_Quochoi_Khoa(id);
                ViewData["id"] = fc["id"];
                //ViewData["khoa"] = _thietlap.Get_Quochoi_Khoa(id);
                ViewData["khoa"] = editing;
                DateTime temp = (DateTime)editing.DBATDAU;
                int yBatDau = 0;
                if(editing.DBATDAU != null)
                    yBatDau = temp.Year;
                temp = (DateTime)editing.DKETTHUC;
                int yKetThuc = 0;
                if (editing.DKETTHUC != null)
                    yKetThuc = temp.Year;
                ViewData["opt-nam-batdau"] = Get_Option_Nam(yBatDau);
                ViewData["opt-nam-ketthuc"] = Get_Option_Nam(yKetThuc);
                return PartialView("../Ajax/Thietlap/Khoa_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách khóa");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Khoa_update(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(21, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iKhoa = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_List_Quochoi_Khoa().Where(v => v.CCODE != null  &&  v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IKHOA != iKhoa).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suakhoa", iKhoa))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    QUOCHOI_KHOA q = _thietlap.Get_Quochoi_Khoa(iKhoa);
                    q.CTEN = func.RemoveTagInput(fc["cTen"]);
                    q.ILOAI = Decimal.Parse(fc["iLoai"]);
                    int batDau = Convert.ToInt32(fc["dBatDau"]);
                    int ketThuc = Convert.ToInt32(fc["dKetThuc"]);
                    DateTime yBatDau = new DateTime(batDau, 1, 1);
                    DateTime yKetThuc = new DateTime(ketThuc, 1, 1);
                    q.DBATDAU = yBatDau;
                    q.DKETTHUC = yKetThuc;
                    q.CCODE = fc["cCode"];
                    _thietlap.Update_QuocHoi_Khoa(q);
                    int iUserLogin = u_info.tk_action.iUser;
                    Tracking(iUserLogin, "Cập nhật lại khóa: " + q.CTEN);
                    Response.Write(1);
                  
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách khóa  ");
                //return null;
                throw;
            }

        }
        [ValidateInput(false)]
        public ActionResult Ajax_Khoa_status(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(21, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                QUOCHOI_KHOA q = _thietlap.Get_Quochoi_Khoa(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(id_user(), "Bỏ chọn áp dụng khóa: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(id_user(), "Chọn áp dụng khóa: " + q.CTEN);
                }
                _thietlap.Update_QuocHoi_Khoa(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa trạng thái danh sách khóa");
                //return null;
                throw;
            }

        }
        [ValidateInput(false)]
        public ActionResult Ajax_Khoa_macdinh(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(21, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUserLogin = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                QUOCHOI_KHOA q = _thietlap.Get_Quochoi_Khoa(id);
                //Lấy loại đối tượng của khoá hiện tại
                int iLoai = (int)q.ILOAI;
                if (q.IMACDINH != 1)
                {
                    //Tìm khoá mặc định cũ và set IMACDINH về 0
                    Dictionary<string, object> paramMacDinh = new Dictionary<string, object>();
                    paramMacDinh.Add("IMACDINH", 1);
                    paramMacDinh.Add("ILOAI", iLoai);
                    if(_thietlap.Get_List_Quochoi_Khoa(paramMacDinh).Count > 0)
                    {
                        var macDinhCu = _thietlap.Get_List_Quochoi_Khoa(paramMacDinh).First();
                        macDinhCu.IMACDINH = 0;
                        paramMacDinh = new Dictionary<string, object>();
                        paramMacDinh.Add("IMACDINH", 1);
                        paramMacDinh.Add("IKHOA", macDinhCu.IKHOA);
                        // Set kỳ họp mặc định của khoá mặc định cũ có IMACDINH = 0
                        var kyHopMacDinhCu = _thietlap.Get_List_Quochoi_Kyhop(paramMacDinh);
                        if(kyHopMacDinhCu.Count > 0)
                        {
                            var temp = kyHopMacDinhCu.First();
                            temp.IMACDINH = 0;
                            _thietlap.Update_QuocHoi_Kyhop(temp);
                        }
                        _thietlap.Update_QuocHoi_Khoa(macDinhCu);
                    }
                    

                    // Set IMACDINH của khoá mặc định mới
                    q.IMACDINH = 1;
                    _thietlap.Update_QuocHoi_Khoa(q);

                }
                Tracking(iUserLogin, "Chọn khóa " + q.CTEN + " làm khóa hiện tại");
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Mặc định trạng thái danh sách khóa");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Khoa_search(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                //....
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(21, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr, Request.Cookies["url_key"].Value));
                }
                List<QUOCHOI_KHOA> khoa = _thietlap.Get_List_Quochoi_Khoa();
                Response.Write(tl.List_Khoa(khoa, id, Request.Cookies["url_key"].Value));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách khóa");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Khoa_del(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(21, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUserLogin = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var thongtinkyhopcokhoa = _thietlap.Get_List_Quochoi_Kyhop().Where(x => x.IKHOA == id && x.IDELETE == 0).ToList();
                if (thongtinkyhopcokhoa.Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    var khoa = _thietlap.Get_Quochoi_Khoa(id);
                    khoa.IDELETE = 1;
                    khoa.IHIENTHI = 0;
                    _thietlap.Update_QuocHoi_Khoa(khoa);
                    Tracking(iUserLogin, "Xóa khóa " + khoa.CTEN + "");
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa danh sách khóa");
                //return null;
                throw;
            }

        }
        /// <summary>
        /// Danh mục kỳ họp
        /// </summary>
        /// <returns></returns>
        public ActionResult Kyhop()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                // Mặc định
                int iLoai = 1;
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                    iLoai = 0;

                if (Request["iDoiTuong"] != null)
                {
                    iLoai = Convert.ToInt32(Request["iDoiTuong"]);

                }
                TaikhoanAtion act = GetUserInfor().tk_action;
                if (!base_buss.Action_(42, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                string url_key = func.Get_Url_keycookie();
                ViewData["opt-doituong"] = Get_Option_LoaiDoiTuong(iLoai);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("ILOAI", iLoai);
                List<QUOCHOI_KHOA> khoa = _thietlap.Get_List_Quochoi_Khoa(dict);
                List<QUOCHOI_KYHOP> kyhop = _thietlap.Get_List_Quochoi_Kyhop();
                ViewData["list"] = tl.List_Kyhop_Khoa(khoa, kyhop, 0, url_key);
                ViewData["opt-kyhop"] = tl.OptionKyhop_Search(url_key);
                return View();
            }
            catch (Exception e)
            {

              
                log.Log_Error(e, "danh sách kỳ họp");
                return View("../Home/Error_Exception");
            }

        }
        public string Get_Option_Year()
        {
            int year_now = DateTime.Now.Year;
            string str = "";
            for (int i = year_now + 5; i > 2000; i--)
            {
                string select = ""; if (i == year_now) { select = "selected"; }
                str += "<option " + select + " value='" + i + "'>" + i + "</option>";
            }
            return str;
        }
        public ActionResult Ajax_Kyhop_add(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                SetTokenAction("thietlap_themkyhop");
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(42, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                //int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["opt-khoa"] = JsonConvert.SerializeObject(tl.Option_Khoa_QuocHoi());
                return PartialView("../Ajax/Thietlap/Kyhop_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách kỳ họp");
                //return null;
                throw;
            }


        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Kyhop_insert(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(42, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_List_Quochoi_Kyhop().Where(v => v.CCODE != null  && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IKHOA == Convert.ToInt32(fc["iKhoa"]) && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_themkyhop"))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    QUOCHOI_KYHOP q = new QUOCHOI_KYHOP();
                    q.CTEN = func.RemoveTagInput(fc["cTen"]);
                    q.IKHOA = Convert.ToInt32(fc["iKhoa"]);
                    QUOCHOI_KHOA k = _thietlap.Get_Quochoi_Khoa((int)q.IKHOA);
                    if (fc["dBatDau"] != "")
                    {
                        q.DBATDAU = Convert.ToDateTime(func.ConvertDateToSql(fc["dBatDau"]));
                        if (q.DBATDAU < k.DBATDAU || q.DBATDAU > k.DKETTHUC)
                        {
                            Response.Write(3);
                            return null;
                        }
                    }
                    if (fc["dKetThuc"] != "")
                    {
                        q.DKETTHUC = Convert.ToDateTime(func.ConvertDateToSql(fc["dKetThuc"]));
                        if (q.DKETTHUC < k.DBATDAU || q.DKETTHUC > k.DKETTHUC)
                        {
                            Response.Write(3);
                            return null;
                        }
                    }
                    q.IMACDINH = 0;
                    q.IHIENTHI = 1;
                    q.IDELETE = 0;
                    q.CCODE = fc["cCode"];
                    q.IVITRI = 1;
                    _thietlap.Insert_QuocHoi_Kyhop(q);
                    int iUserLogin = id_user();
                    Tracking(iUserLogin, "Thêm kỳ họp: " + q.CTEN + " khóa " + tl.GetName_KhoaHop_ByKyHop((int)q.IKYHOP));
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách kỳ họp");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Kyhop_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(42, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suakyhop", id);
                QUOCHOI_KYHOP kyhop = _thietlap.Get_Quochoi_Kyhop(id);
                if(kyhop != null)
                {
                    QUOCHOI_KHOA khoa = _thietlap.Get_Quochoi_Khoa(Decimal.ToInt32(kyhop.IKHOA ?? 0));
                    if(khoa != null)
                    {
                        ViewData["iLoaiKhoa"] = Decimal.ToInt32(khoa.ILOAI);
                    } 
                }
                ViewData["id"] = fc["id"];
                ViewData["kyhop"] = kyhop;
                ViewData["opt-khoa"] = JsonConvert.SerializeObject(tl.Option_Khoa_QuocHoi());
                return PartialView("../Ajax/Thietlap/Kyhop_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa  danh sách kỳ họp  ");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Kyhop_del(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(42, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                QUOCHOI_KYHOP k = _thietlap.Get_Quochoi_Kyhop(id);
                int iUserLogin = u_info.tk_action.iUser;
                k.IHIENTHI = 0;
                k.IDELETE = 1;
                Tracking(iUserLogin, "Xóa kỳ họp: " + tl.GetName_KyHop_KhoaHop(id).Replace("</br>", " - "));
                _thietlap.Update_QuocHoi_Kyhop(k);
                //db.Database.ExecuteSqlCommand("delete from quochoi_kyhop where iKyHop=" + id);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa  danh sách kỳ họp  ");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Kyhop_update(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                //....
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(42, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iKyHop = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_List_Quochoi_Kyhop().Where(v => v.CCODE != null  && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IKHOA == Convert.ToInt32(fc["iKhoa"]) && v.IKYHOP != iKyHop && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    //if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }

                    if (!CheckTokenAction("thietlap_suakyhop", iKyHop))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    QUOCHOI_KYHOP q = _thietlap.Get_Quochoi_Kyhop(iKyHop);
                    q.CTEN = func.RemoveTagInput(fc["cTen"]);
                    QUOCHOI_KHOA k = _thietlap.Get_Quochoi_Khoa((int)q.IKHOA);
                    if (fc["dBatDau"] != "")
                    {
                        q.DBATDAU = Convert.ToDateTime(func.ConvertDateToSql(fc["dBatDau"]));
                        if (q.DBATDAU < k.DBATDAU || q.DBATDAU > k.DKETTHUC)
                        {
                            Response.Write(3);
                            return null;
                        }
                    }
                    if (fc["dKetThuc"] != "")
                    {
                        q.DKETTHUC = Convert.ToDateTime(func.ConvertDateToSql(fc["dKetThuc"]));
                        if (q.DKETTHUC < k.DBATDAU || q.DKETTHUC > k.DKETTHUC)
                        {
                            Response.Write(3);
                            return null;
                        }
                    }
                    q.IKHOA = Convert.ToInt32(fc["iKhoa"]);
                    q.CCODE = fc["cCode"];
                    _thietlap.Update_QuocHoi_Kyhop(q);
                    int iUserLogin = u_info.tk_action.iUser;
                    Tracking(iUserLogin, "Cập nhật lại kỳ họp: " + q.CTEN + " khóa " + tl.GetName_KhoaHop_ByKyHop((int)q.IKYHOP));
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa  danh sách kỳ họp ");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Kyhop_macdinh(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(42, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUserLogin = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                QUOCHOI_KYHOP q = _thietlap.Get_Quochoi_Kyhop(id);
                if (q.IMACDINH != 1)
                {
                    // Tìm kỳ họp mặc định cũ cùng khoá với mặc định mới
                    Dictionary<string, object> paramMacDinh = new Dictionary<string, object>();
                    paramMacDinh.Add("IMACDINH", 1);
                    paramMacDinh.Add("IKHOA", q.IKHOA);
                    var qhk = _thietlap.Get_List_Quochoi_Kyhop(paramMacDinh);
                    if (qhk.Count > 0)
                    {
                        var macDinhCu = qhk.FirstOrDefault();
                        macDinhCu.IMACDINH = 0;
                        _thietlap.Update_QuocHoi_Kyhop(macDinhCu);
                    }
                    q.IMACDINH = 1;
                    _thietlap.Update_QuocHoi_Kyhop(q);

                }

                Tracking(iUserLogin, "Chọn kỳ họp: " + q.CTEN + " khóa " + tl.GetName_KhoaHop_ByKyHop((int)q.IKYHOP) + " làm kỳ họp hiện tại");
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Mặc định  danh sách kỳ họp  ");
                //return null;
                throw;
            }



        }

        public ActionResult Ajax_Kyhop_status(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(42, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iKyHop = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                QUOCHOI_KYHOP q = _thietlap.Get_Quochoi_Kyhop(iKyHop);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(id_user(), "Bỏ chọn áp dụng kỳ họp: " + tl.GetName_KyHop_KhoaHop(iKyHop).Replace("</br>", " - "));
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(id_user(), "Chọn áp dụng kỳ họp: " + tl.GetName_KyHop_KhoaHop(iKyHop).Replace("</br>", " - "));
                }
                _thietlap.Update_QuocHoi_Kyhop(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Mặc định  danh sách kỳ họp  ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Kyhop_search(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                List<QUOCHOI_KHOA> Khoa = _thietlap.Get_List_Quochoi_Khoa();
                List<QUOCHOI_KYHOP> Kyhop = _thietlap.Get_List_Quochoi_Kyhop();
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                Response.Write(tl.List_Kyhop_Khoa_search(Khoa,Kyhop, id, Request.Cookies["url_key"].Value));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách kỳ họp  ");
                //return null;
                throw;
            }

        }
        /* Quản lý cơ quan quốc hội*/
        public ActionResult Ajax_Coquan_list()
        {

            try
            {
                if (!CheckAuthToken()) { return null; }
                Response.Write(tl.Ajax_List_CoQuan());
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách cơ quan  ");
                //return null;
                throw;
            }
        }
        public ActionResult Coquan(int page = 1)
        {
            try
            {
                //....
                if (!CheckAuthToken_Api()) { Response.Redirect("/Home/Login"); return null; }
                
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(22, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                string url_key = func.Get_Url_keycookie();
               List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan().Where(x => x.IDELETE == 0).ToList(); 
              //  List<DIAPHUONG> diaphuong = _thietlap.Get_Diaphuong();
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                string tukhoa = "";
                if (Request["q"] != null)
                {
                    tukhoa = Request["q"];
                }
                var thongtincoquan = thutuc.GET_THIETLAPCOQUAN_PHANTRANG("PKG_THIETLAP_HETHONG.PKG_COQUAN_PHANTRANG", tukhoa, page, post_per_page).ToList();
                if (thongtincoquan != null && thongtincoquan.Count() > 0)
                {
                    ViewData["list"] = tl.COQUANTHIETLAP(thongtincoquan);
                    ViewData["phantrang"] = "<tr><td colspan='7'>" + base_appcode.PhanTrang((int)thongtincoquan.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='7'  class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }
                ViewData["opt-coquan"] = tl.Option_Coquan(coquan, 0, 0, url_key);
                return View();
            }
            catch (Exception e)
            {
             //   log.Log_Error(e, "danh sách cơ quan  ", null, error_level = 1);
                log.Log_Error(e, "danh sách cơ quan ");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Coquan_use(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(22, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                QUOCHOI_COQUAN tk = _thietlap.GetBy_Quochoi_CoquanID(id);
                if (tk.IUSE == 1)
                {
                    tk.IUSE = 0;
                    Tracking(id_user(), "Bỏ chọn đơn vị '" + tk.CTEN + "' tham gia phần mềm ");
                }
                else
                {
                    tk.IUSE = 1;
                    Tracking(id_user(), "Chọn đơn vị '" + tk.CTEN + "' tham gia phần mềm ");
                }
                _thietlap.Update_QuocHoi_Coquan(tk);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách cơ quan");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Coquan_group(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                QUOCHOI_COQUAN tk = _thietlap.GetBy_Quochoi_CoquanID(id);
                if (tk.IGROUP == 1)
                {
                    tk.IGROUP = 0; Tracking(id_user(), "Bỏ chọn đơn vị '" + tk.CTEN + "' làm nhóm cơ quan ");
                }
                else
                {
                    tk.IGROUP = 1; Tracking(id_user(), "Chọn đơn vị '" + tk.CTEN + "' làm nhóm cơ quan ");
                }
                _thietlap.Update_QuocHoi_Coquan(tk);
                Response.Write(1);

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách cơ quan  ");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Coquan_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(22, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suacoquan", id);
                QUOCHOI_COQUAN donvi = _thietlap.GetBy_Quochoi_CoquanID(id);
                ViewData["id"] = fc["id"];
                ViewData["coquan"] = donvi;
                ViewData["opt-donvi"] = Get_Option_DonVi((int)donvi.IPARENT);
                ViewData["opt-tinh"] = tl.Option_TinhThanh_ByID_Parent(0, (int)donvi.IDIAPHUONG);
                return PartialView("../Ajax/Thietlap/Coquan_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách cơ quan  ");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Coquan_update(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(22, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));

                string cCode = func.RemoveTagInput(fc["cCode"]).Trim().ToUpper();
                string cTen = func.RemoveTagInput(fc["cTen"]).Trim().ToUpper();


                var list = _thietlap.Get_List_CoQuan_SQL(cCode, id).ToList();
                var list2 = _thietlap.Get_List_Quochoi_Coquan().Where(v => v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.ICOQUAN != id && v.IDELETE == 0).ToList();
                if (list.Count() > 0)
                {
                    Response.Write(2);

                }
                else if (list2.Count() > 0)
                {
                    Response.Write(3);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suacoquan", id))
                    {
                        Response.Redirect("/Home/Error");
                    }
                    QUOCHOI_COQUAN donvi = _thietlap.GetBy_Quochoi_CoquanID(id);
                    donvi.IPARENT = Convert.ToInt32(fc["iParent"]);
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.IDIAPHUONG = Convert.ToInt32(fc["iDiaPhuong"]);
                    _thietlap.Update_QuocHoi_Coquan(donvi);
                    int iUserLogin = id_user();
                    Tracking(iUserLogin, "Cập nhật lại đơn vị: " + donvi.CTEN);
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách cơ quan ");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Coquan_del(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(22, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                QUOCHOI_COQUAN coquan = _thietlap.GetBy_Quochoi_CoquanID(id);
                coquan.IHIENTHI = 0;
                coquan.IDELETE = 1;
                _thietlap.Update_QuocHoi_Coquan(coquan);
                int iUserLogin = id_user();
                Tracking(iUserLogin, "Xóa cơ quan: " + coquan.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa danh sách cơ quan  ");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_Coquan_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(22, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                QUOCHOI_COQUAN donvi = _thietlap.GetBy_Quochoi_CoquanID(id);
                donvi.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_QuocHoi_Coquan(donvi);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái danh sách cơ quan  ");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Coquan_add()
        {
            try
            {

                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(22, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_themcoquan");
                ViewData["opt-donvi"] = Get_Option_DonVi(0);
                ViewData["opt-tinh"] = tl.Option_TinhThanh_ByID_Parent(0, 0);
                return PartialView("../Ajax/Thietlap/Coquan_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách cơ quan  ");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Coquan_insert(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(22, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string cCode = func.RemoveTagInput(fc["cCode"]).Trim().ToUpper();
                string cTen = func.RemoveTagInput(fc["cTen"]).Trim();
                var list = _thietlap.Get_List_CoQuan_SQL_CheckInsert(cCode.ToUpper().Trim()).ToList();
                if (list.Count() > 0)
                {
                    Response.Write(2);
                }
                else if (_thietlap.Get_List_Quochoi_Coquan().ToList().Where(v => v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0).Count() > 0)
                {
                    Response.Write(3);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_themcoquan"))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }

                    QUOCHOI_COQUAN donvi = new QUOCHOI_COQUAN();
                    int iParent = Convert.ToInt32(fc["iParent"]);
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IPARENT", iParent);
                    int iViTri = _thietlap.GetBy_List_Quochoi_Coquan(_condition).Count() + 1;
                    donvi.IPARENT = iParent;
                    donvi.IVITRI = iViTri;
                    donvi.IDIAPHUONG = Convert.ToInt32(fc["iDiaPhuong"]);
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.IMACDINH = 0;
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.IGROUP = 0;
                    donvi.IUSE = 1;
                    donvi.IHIENTHI = 1;
                    donvi.IDELETE = 0;
                    _thietlap.Insert_Quochoi_Coquan(donvi);
                    Tracking(id_user(), "Thêm mới đơn vị: " + donvi.CTEN);
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách cơ quan  ");
                //return null;
                throw;
            }

        }
        /* Quản lý phòng ban nội bộ*/
        public ActionResult Phongban()
        {
            try
            {
                //....
                if (!CheckAuthToken_Api()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                if (!base_buss.Action_(23, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                string url_cookie = func.Get_Url_keycookie();
                List<USER_PHONGBAN> phongban = _thietlap.Get_List_Phongban();
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                ViewData["list"] = tl.List_DonVi_PhongBan(phongban, coquan, iUser, url_cookie);
                ViewData["OptionPhongBan"] = tl.Option_Phongban(phongban);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Danh sách phòng ban");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Phongban_add()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(23, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                SetTokenAction("thietlap_themphongban");
              //  List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan().Where(x => x.IDELETE == 0 && x.IPARENT ==0).ToList();
                ViewData["opt_donvi"] = tl.OptionCoQuan_TreeList();
                ViewData["opt_phongbancha"] = tl.Option_PhongBanChaCon();
      
                return PartialView("../Ajax/Thietlap/Phongban_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách phòng ban");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Phongban_insert(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(23, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string cTen = func.RemoveTagInput(fc["cTen"]);
                int imadonvi = Convert.ToInt32(fc["iDonVi"]); 
                if (_thietlap.Get_List_Phongban().Where(v => v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0 && v.IDONVI == imadonvi).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {

                    if (!CheckTokenAction("thietlap_themphongban"))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    int iUser = u_info.tk_action.iUser;
                    USER_PHONGBAN phong = new USER_PHONGBAN();
                    int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                    phong.IDONVI = iDonVi;
                    phong.CTEN = func.RemoveTagInput(fc["cTen"]);
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IDONVI", iDonVi);
                    int iViTri = _thietlap.GetBy_List_Phongban(_condition).ToList().Count() + 1;
                    phong.IVITRI = iViTri;
                    phong.IHIENTHI = 1;
                    phong.IDELETE = 0;
                    decimal iPhongBanCha = Convert.ToDecimal(fc["iPhongBanCha"]);
                    phong.IPARENT = iPhongBanCha;
                    _thietlap.Insert_Phongban(phong);
                    int iUserLogin = id_user();
                    Tracking(id_user(), "Thêm mới phòng: " + phong.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách phòng ban");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Phongban_order(FormCollection fc)
        {
            try
            {

                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(23, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                USER_PHONGBAN phongban = _thietlap.GetBy_PhongbanID(id);
                int value = Convert.ToInt32(fc["value"]);
                phongban.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Phongban(phongban);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm vị trí danh sách phòng ban");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Phongban_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(23, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suaphongban", id);
                USER_PHONGBAN phong = _thietlap.GetBy_PhongbanID(id);
                ViewData["id"] = fc["id"];
                ViewData["phong"] = phong;
                ViewData["opt_donvi"] = tl.OptionCoQuan_TreeList((int)phong.IDONVI);
                ViewData["opt_phongbancha"] = tl.Option_PhongBanChaCon((int)phong.IPARENT);
                return PartialView("../Ajax/Thietlap/Phongban_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách phòng ban");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Phongban_update(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(23, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id_phong = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cTen"]);
                int imadonvi = Convert.ToInt32(fc["iDonVi"]);
                if (_thietlap.Get_List_Phongban().Where(v => v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IPHONGBAN != id_phong && v.IDELETE == 0 && v.IDONVI == imadonvi).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    //
                    if (!CheckTokenAction("thietlap_suaphongban", id_phong))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    USER_PHONGBAN phong = _thietlap.GetBy_PhongbanID(id_phong);
                    phong.IDONVI = Convert.ToInt32(fc["iDonVi"]);
                    phong.CTEN = func.RemoveTagInput(fc["cTen"]);
                    phong.IPARENT = Convert.ToDecimal(fc["iPhongBanCha"]); ;
                    _thietlap.Update_Phongban(phong);
                    int iUserLogin = id_user();
                    Tracking(id_user(), "Cập nhật lại phòng: " + phong.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách phòng ban");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Phongban_del(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(23, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                USER_PHONGBAN phong = _thietlap.GetBy_PhongbanID(id);
                int iUserLogin = u_info.tk_action.iUser;
                phong.IDELETE = 1;
                phong.IHIENTHI = 0;
                Tracking(iUserLogin, "Hủy phòng: " + phong.CTEN);
                _thietlap.Update_Phongban(phong);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa danh sách phòng ban");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Phongban_status(FormCollection fc)
        {
            try
            {

                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(23, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUserLogin = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                USER_PHONGBAN q = _thietlap.GetBy_PhongbanID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUserLogin, "Bỏ chọn áp dụng phòng ban: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUserLogin, "Chọn áp dụng phòng ban: " + q.CTEN);
                }
                _thietlap.Update_Phongban(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái danh sách phòng ban");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Phongban_search(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                //....
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                List<USER_PHONGBAN> phongban = _thietlap.Get_List_Phongban();
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(23, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUserLogin = u_info.tk_action.iUser;
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                if (id != 0)
                {
                    Response.Write(tl.List_DonVi_PhongBan_Search(coquan, phongban, id, iUserLogin, Request.Cookies["url_key"].Value));
                }
                else
                {
                    Response.Write(tl.List_DonVi_PhongBan_Search(coquan, phongban, 0 , iUserLogin, Request.Cookies["url_key"].Value));
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách phòng ban");
                //return null;
                throw;
            }

        }
        //nhóm tài khoản
        public ActionResult Nhomtaikhoan()
        {
            if (!CheckAuthToken_Api()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                //....
                int iUser = id_user();
                TaikhoanAtion act = GetUserInfor().tk_action;
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(24, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                if (!base_buss.Action_(24, act))
                {
                    ViewData["bt_add"] = " hide ";
                }
                List<USER_GROUP> usergroup = _thietlap.Get_Usergroup();
                List<ACTION> action = _thietlap.Get_Action();
                List<USER_GROUP_ACTION> userGroupAction = _thietlap.Get_user_Group_Action();
                ViewData["url_cookie"] = func.Get_Url_keycookie();
                ViewData["list"] = tl.List_NhomQuyenOptimize(usergroup, action, userGroupAction, this.ControllerContext);
                ViewData["Option-taikhoan"] = tl.Option_NhomQuyen(usergroup, iUser);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Danh sách Nhóm quyền");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Nhomtaikhoan_add()
        {
            try
            {

                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(24, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_themnhomtaikhoan");
                //ViewData["list_chucnang"] = _base.List_CheckBox_Nhomtaikhoan();
                return PartialView("../Ajax/Thietlap/Nhomquyen_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách nhóm quyền");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Nhomtaikhoan_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(24, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suanhomtaikhoan", id);
                //ViewData["list_chucnang"] = _base.List_CheckBox_Nhomtaikhoan(id);
                ViewData["id"] = fc["id"];
                ViewData["nhom"] = _thietlap.GetBy_UsergroupID(id);
                return PartialView("../Ajax/Thietlap/Nhomquyen_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách nhóm quyền");
                //return null;
                throw;
            }

        }
        public ActionResult Nhomtaikhoan_cog()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(24, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_themnhomtaikhoan_cog", id);
                ViewData["list"] = tl.List_CheckBox_NhomQuyen(id);
                ViewData["nhom"] = _thietlap.GetBy_UsergroupID(id);
                ViewData["id"] = Request["id"];
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "  danh sách nhóm quyền");
                //return null;
                throw;
            }


        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Nhomtaikhoan_cog(FormCollection fc)
        {

            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                bool resule = false;
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("thietlap_themnhomtaikhoan_cog", id))
                {
                    Response.Redirect("/Home/Error");
                    return null;
                }
                string action = fc["action"];

                /*------- tìm tất cả người dùng thuộc group -------*/
                List<USERS> listUser = _thietlap.Get_Taikhoan().Where(n => n.CARRGROUP != null && n.CARRGROUP != "" && n.CARRGROUP.Split(',').Contains(id.ToString())).ToList();

                foreach (var user in listUser)
                {

                    if (user != null)
                    {
                        /*------- tìm tất cả quyền thuộc tất cả nhóm của người dùng -------*/
                        string[] listGroup = user.CARRGROUP.Split(',');
                        _condition = new Dictionary<string, object>
                        {
                            { "IDELETE", 0 }
                        };
                        List<USER_GROUP_ACTION> listAction = _thietlap.Get_user_Group_Action(_condition).Where(n => listGroup.Contains(n.IGROUP.ToString())).ToList();

                        /*------- duyệt từng quyền t -------*/
                        if (listAction != null && listAction.Count() > 0)
                        {
                            foreach (var t in listAction)
                            {
                                /*------- tìm tất cả nhóm có quyền t -------*/
                                _condition = new Dictionary<string, object>
                                {
                                    { "IACTION", t.IACTION },
                                    { "IDELETE", 0 }
                                };
                                var listGroupHasAction = _thietlap.Get_user_Group_Action(_condition).Select(n => n.IGROUP.ToString()).ToList();

                                /*------- nếu có nhiều hơn 2 nhóm có quyền t thì giữ không thì xóa -------*/
                                int count = 0;
                                for (int i = 0; i < listGroup.Count() && count < 2; i++)
                                {
                                    var item = listGroup[i];
                                    if (item != null)
                                    {
                                        if (listGroupHasAction.Contains(item))
                                        {
                                            count++;
                                        }
                                    }
                                }

                                _condition = new Dictionary<string, object>
                                {
                                    { "IUSER", user.IUSER },
                                    { "IACTION", t.IACTION }
                                };
                                if (count < 2)
                                {
                                    _thietlap.Delete_User_Action_Multi(_condition);
                                }
                            }
                        }
                    }
                }

                // xóa toàn bộ quyền của nhóm
                List<USER_GROUP_ACTION> listUserGroup = new List<USER_GROUP_ACTION>();
                resule = _thietlap.updateUserGroupFunctionList(id, listUserGroup);

                if(action!=null)
                {
                    if (action != "")
                    {

                        foreach (var t in action.Split(','))
                        {

                            if (t != "")
                            {

                                foreach (var user in listUser)
                                {

                                    if (user != null)
                                    {
                                        /*------- thêm quyền trong số những quyền mà nhóm được chọn -------*/
                                        USER_ACTION userObj = new USER_ACTION
                                        {
                                            IACTION = Convert.ToInt32(t),
                                            IUSER = user.IUSER
                                        };
                                        _condition = new Dictionary<string, object>
                                        {
                                            { "IUSER", user.IUSER },
                                            { "IACTION", Convert.ToInt32(t) }
                                        };
                                        USER_ACTION userAction = _thietlap.GetBy_List_User_Action(_condition).FirstOrDefault();

                                        if (userAction == null)
                                        {
                                            _thietlap.Insert_User_Action(userObj);
                                        }
                                    }
                                }

                                USER_GROUP_ACTION UserGroupObj = new USER_GROUP_ACTION();
                                UserGroupObj.IGROUP = id;
                                UserGroupObj.IACTION = Convert.ToInt32(t);
                                UserGroupObj.IDELETE = 0;
                                _thietlap.Insert_Usergroupaction(UserGroupObj);

                            }

                        }

                    }
                }
               
                Tracking(id_user(), "Thiết lập chức năng cho tài khoản: " + _thietlap.GetBy_UsergroupID((int)id).CTEN);

                Response.Redirect(Request.Cookies["url_return"].Value);

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " danh sách nhóm quyền");
                //return null;
                throw;
            }


        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Nhomtaikhoan_insert(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(24, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string cTen = func.RemoveTagInput(fc["cTen"]);
                if (_thietlap.Get_Usergroup().Where(v => v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_themnhomtaikhoan"))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    int iUser = id_user();
                    USER_GROUP g = new USER_GROUP();
                    g.CTEN = func.RemoveTagInput(fc["cTen"]);
                    g.CMOTA = func.RemoveTagInput(fc["cMoTa"]);
                    _thietlap.Insert_Usergroup(g);
                    Tracking(iUser, "Thêm nhóm tài khoản: " + g.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách nhóm quyền");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Nhomtaikhoan_update(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(24, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int iGroup = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cTen"]);
                if (_thietlap.Get_Usergroup().Where(v => v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IGROUP != iGroup && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    //
                    if (!CheckTokenAction("thietlap_suanhomtaikhoan", iGroup))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    USER_GROUP g = _thietlap.GetBy_UsergroupID(iGroup);
                    g.CTEN = func.RemoveTagInput(fc["cTen"]);
                    g.CMOTA = func.RemoveTagInput(fc["cMoTa"]);
                    _thietlap.Update_Usergroup(g);
                    Tracking(iUser, "Cập nhật lại nhóm tài khoản: " + g.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách nhóm quyền");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_List_action_choice(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken_Api()) { return null; }
                string arr = fc["arr"];
                UserInfor u_info = GetUserInfor();
                Response.Write(tl.List_CheckBox_action_choice(arr, u_info.tk_action));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "  danh sách  quyền chọn");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Nhomtaikhoan_search(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken_Api()) { return null; }
                //....
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(24, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32((id_encr));
                }
                List<USER_GROUP> usergroup = _thietlap.Get_Usergroup();
                Response.Write(tl.List_NhomQuyen_search(usergroup, (int)u_info.tk_action.iUser, Request.Cookies["url_key"].Value, id));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " tìm kiếm  danh sách nhóm  quyền ");
                //return null;
                throw;
            }

        }
        // Taif khoan
        public ActionResult Taikhoan(int page = 1)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                
                if (!base_buss.Action_(25,u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int iDonVi = 0;
                if (Request["iDonVi"] != null)
                {
                    iDonVi = Convert.ToInt32(Request["iDonVi"]);
                }
                if (!base_buss.Action_(25, act))
                {
                    ViewData["bt_add"] = " hide ";
                }
                if (!_base.IS_BanDanNguyen(iUser))
                {
                    ViewData["disable_donvi"] = "  style='display:none' ";
                }
                string tukhoa = "";
                if (Request["q"] != null)
                {
                    tukhoa = Request["q"];
                }
                //List<USERS> user = _thietlap.Get_Taikhoan();
                //List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                //List<USER_PHONGBAN> phongban = _thietlap.Get_List_Phongban();
                //List<USER_GROUP> group = _thietlap.Get_All_Usergroup();
                 int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                 var donvi = thutuc.GetNguoiDung_OPT("PKG_THIETLAP_HETHONG.PRC_NGUOIDUNG_OPT", tukhoa, page, post_per_page).ToList();
                if(donvi != null && donvi.Count() > 0 )
                {
                    ViewData["taikhoan"] = tl.TAIKHOAN_LIST(donvi);
                    ViewData["phantrang"] = "<tr><td colspan='6'>" + base_appcode.PhanTrang((int)donvi.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    ViewData["taikhoan"] = "<tr><td colspan='6'  class='alert tcenter alert-info' >Không có kết quả tìm kiếm nào</td></tr>";
                }
             //   ViewData["opt-donvi"] = tl.OptionCoQuan_TaiKhoan(0, 0, iDonVi);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "  danh sách tài khoản");
                return View("../Home/Error_Exception");
            }
        }
        [ValidateInput(false)]
        public ActionResult Ajax_Taikhoan_CheckUsername(FormCollection fc)
        {
            try
            {

                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                string username = fc["username"];
                if (fc["id_user"].ToString() == "0")
                {
                    _condition = new Dictionary<string, object>();
                    _condition.Add("CUSERNAME", username);
                    if (_thietlap.GetBy_List_Taikhoan(_condition).Count() > 0)
                    { Response.Write(1); }
                    else { Response.Write(0); }
                }
                else
                {
                    int id_user = Convert.ToInt32(HashUtil.Decode_ID(fc["id_user"], Request.Cookies["url_key"].Value));


                    var list = _thietlap.Get_List_Taikhoan_Sql_checkuser(username, id_user).ToList();

                    if (list.Count() > 0)
                    { Response.Write(1); }
                    else { Response.Write(0); }
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " kiểm tra danh sách tài khoản");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Taikhoan_info(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
             
                List<USER_GROUP> group = _thietlap.Get_All_Usergroup();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["taikhoan"] = _thietlap.GetBy_TaikhoanID(id);
                ViewData["detail"] = tl.Taikhoan_Detail(id);
                ViewData["group"] = tl.Row_Taikhoan_list_nhomquyen(group, _thietlap.GetBy_TaikhoanID(id).CARRGROUP);
                ViewData["action"] = tl.List_ChucNang_TaiKhoan_View(id);
                return PartialView("../Ajax/Thietlap/Taikhoan_info");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thông tin danh sách tài khoản");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Taikhoan_lichsu(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["list"] = tl.Lichsu_Taikhoan(id);
                return PartialView("../Ajax/Thietlap/Taikhoan_lichsu");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thông tin lịch sử danh sách tài khoản");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Taikhoan_add(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_themtaikhoan");
                int iUser = u_info.tk_action.iUser;
                int iDonVi = IDDonVi_User(iUser);
                ViewData["opt-type"] = tl.Option_UserType();
                //ViewData["opt_phongban"] = tl.Option_PhongBan_ByDonVi(0, 0);
                //ViewData["chucvu"] = tl.Option_ChucVu_TheoPhongBan();
                //ViewData["opt_donvi"] = tl.OptionCoQuan_TK(0, 0, 0, 0);
                ViewData["opt_donvi"] = tl.OptionCoQuan_TreeList();
                //List<USER_PHONGBAN> phongban = _thietlap.Get_List_Phongban();
                if (fc["type"] != null)
                {
                    ViewData["opt_donvi"] = tl.OptionCoQuan_TreeList();
                }
                ViewData["opt_phongban"] = Get_Option_ThamQuyen_DiaPhuong_Parent_VP_DBQH_HDND(0);
                ViewData["opt_loaitaikhoan"] = tl.Option_Taikhoan_Type(0,u_info.tk_action);
                return PartialView("../Ajax/Thietlap/Taikhoan_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách tài khoản");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Taikhoan_edit(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suataikhoan", id);
                int iUser = u_info.tk_action.iUser;
                USERS tk = _thietlap.GetBy_TaikhoanID(id);
                ViewData["id"] = fc["id"];
                ViewData["taikhoan"] = tk;
                ViewData["defaultDisplayPassword"] = ThietLapConstants.DEFAULT_DISPLAY_PASSWORD;
                ViewData["chucvu"] = tl.Option_ChucVu_TheoPhongBan((int)tk.IPHONGBAN, (int)tk.ICHUCVU);
                ViewData["opt-type"] = tl.Option_UserType((int)tk.ITYPE);
                int iDonVi = Convert.ToInt32(tk.IDONVI);
                List<USER_PHONGBAN> phongban = _thietlap.Get_List_Phongban();
                if (!_base.IS_BanDanNguyen(iUser))
                {
                    ViewData["disable_donvi"] = " disabled ";
                }
                ViewData["opt_donvi"] = tl.OptionCoQuan_TreeList(iDonVi);
                ViewData["opt_phongban"] = Get_Option_ThamQuyen_DiaPhuong_Parent_VP_DBQH_HDND((int)tk.IPHONGBAN);
                //ViewData["opt_phongban"] = tl.Option_PhongBan_ByDonVi(Convert.ToInt32(tk.IDONVI), Convert.ToInt32(tk.IPHONGBAN));
                //ViewData["opt_type"] = _base.Option_Type_TaiKhoan((int)tk.iType, iUser);
                ViewData["opt_loaitaikhoan"] = tl.Option_Taikhoan_Type(Convert.ToInt32(tk.ITYPE), u_info.tk_action);
                return PartialView("../Ajax/Thietlap/Taikhoan_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa sách tài khoản");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Taikhoan_insert(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken_Api()) { return null; }
                //....
                UserInfor u_info = GetUserInfor();
                string cUsername = func.RemoveTagInput(fc["cUsername"]);
                _condition = new Dictionary<string, object>();
                _condition.Add("CUSERNAME", cUsername);
                if (_thietlap.GetBy_List_Taikhoan(_condition).ToList().Count() > 0)
                {
                    Response.Write("Tên đăng nhập đã tồn tại!");
                    return null;
                }

                string pass = func.RemoveTagInput(fc["cPassword"].ToString()).Trim();
                if (!_base.Check_Sercurity_Pass(pass))
                {
                    Response.Write("Mật khẩu phải có 8 ký tự trở lên, có ký tự viết hoa, chữ số và ký tự đặc biệt!");
                    return null;
                }

                string cSalt = Guid.NewGuid().ToString();
                pass = HashUtil.Encrypt(pass + cSalt, Convert.ToBoolean(HashUtil.HashType.SHA512));


                if (!CheckTokenAction("thietlap_themtaikhoan"))
                {
                    Response.Redirect("/Home/Error");
                    return null;
                }
                int iUser = u_info.tk_action.iUser;
                int iDonVi = IDDonVi_User(iUser);
                if (_base.IS_BanDanNguyen(iUser))
                {
                    iDonVi = Convert.ToInt32(fc["iDonVi"]);
                }

                USERS user = new USERS();
                user.ICHUCVU = Convert.ToInt32(fc["iChucVu"].ToString());
                user.CEMAIL = func.RemoveTagInput(fc["cEmail"]);
                user.CTEN = func.RemoveTagInput(fc["cTen"]);
                user.CPASSWORD = pass;
                user.CUSERNAME = func.RemoveTagInput(fc["cUsername"]);
                user.IPHONGBAN = Convert.ToInt32(fc["iPhongBan"]);
                user.CSDT = func.RemoveTagInput(fc["cSDT"]);
                user.ISTATUS = Convert.ToInt32(fc["iStatus"]);
                user.ITYPE = Convert.ToInt32(fc["iType"]);
                user.IDONVI = iDonVi;
                user.CSALT = cSalt;
                _thietlap.Insert_Taikhoan(user);
                TaiKhoan tk = tl.Taikhoan_Detail((int)user.IUSER);
                Tracking(iUser, "Thêm mới tài khoản: <strong>" + func.RemoveTagInput(fc["cTen"]) + "</strong> thuộc đơn vị <strong>" + tk.donvi + "</strong>");
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm sách tài khoản");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Taikhoan_update(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int iUser_Update = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));

                string cUsername = func.RemoveTagInput(fc["cUsername"]);
                _condition = new Dictionary<string, object>();
                _condition.Add("CUSERNAME", cUsername);
                //check tk đã tồn tại
                var list = _thietlap.GetAll_TaiKhoanByDic(_condition).Where(x => x.IUSER != iUser_Update).ToList();
                if (list.Count() > 0)
                {
                    Response.Write("Tên đăng nhập đã tồn tại!");
                    return null;
                }
                //check bảo mật mật khẩu
                if (func.RemoveTagInput(fc["cPassword"]).Trim() != "" && ThietLapConstants.DEFAULT_DISPLAY_PASSWORD != func.RemoveTagInput(fc["cPassword"]).Trim())
                {
                    string pass = func.RemoveTagInput(fc["cPassword"].ToString()).Trim();
                    if (!_base.Check_Sercurity_Pass(pass))
                    {
                        Response.Write("Mật khẩu phải có 8 ký tự trở lên, có ký tự viết hoa, chữ số và ký tự đặc biệt!");
                        return null;
                    }
                }
                if (!CheckTokenAction("thietlap_suataikhoan", iUser_Update))
                {
                    Response.Redirect("/Home/Error");
                    return null;
                }
                USERS user = _thietlap.GetBy_TaikhoanID(iUser_Update);
                user.ICHUCVU = Convert.ToInt32(fc["iChucVu"].ToString());
                user.CEMAIL = func.RemoveTagInput(fc["cEmail"]);
                user.CTEN = func.RemoveTagInput(fc["cTen"]);
                if (func.RemoveTagInput(fc["cPassword"]).Trim() != "" && ThietLapConstants.DEFAULT_DISPLAY_PASSWORD != func.RemoveTagInput(fc["cPassword"]).Trim())
                {
                    string pass = fc["cPassword"].ToString().Trim();
                    pass = HashUtil.Encrypt(pass + user.CSALT, Convert.ToBoolean(HashUtil.HashType.SHA512));
                    user.CPASSWORD = pass;
                    user.DLASTCHANGEPASS = DateTime.Now;
                }
                user.CUSERNAME = func.RemoveTagInput(fc["cUsername"]);
                user.IPHONGBAN = Convert.ToInt32(fc["iPhongBan"]);
                user.CSDT = fc["cSDT"];
                user.ISTATUS = Convert.ToInt32(fc["iStatus"]);
                user.ITYPE = Convert.ToInt32(fc["iType"]);
                user.IDONVI = Convert.ToInt32(fc["iDonVi"]);
                _thietlap.Update_Taikhoan(user);
                TaiKhoan tk = tl.Taikhoan_Detail((int)user.IUSER);
                Tracking(iUser, "Cập nhật tài khoản: <strong>" + func.RemoveTagInput(fc["cTen"]) + "</strong> thuộc đơn vị <strong>" + tk.donvi + "</strong>");
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa sách tài khoản");
                //return null;
                throw;
            }
        }
        [ValidateInput(false)]
        public ActionResult Ajax_Taikhoan_status(FormCollection fc)
        {
            try
            {

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                TaiKhoan k = tl.Taikhoan_Detail(id);

                USERS tk = _thietlap.GetBy_TaikhoanID(id);
                if (tk.ISTATUS == 1)
                {
                    tk.ISTATUS = 0;
                    Tracking(iUser, "Bỏ kích hoạt tài khoản: <strong>" + k.ten + "</strong> thuộc đơn vị <strong>" + k.donvi + "</strong>");

                }
                else
                {
                    tk.ISTATUS = 1;
                    Tracking(iUser, "Kích hoạt tài khoản: <strong>" + k.ten + "</strong> thuộc đơn vị <strong>" + k.donvi + "</strong>");
                }
                _thietlap.Update_Taikhoan(tk);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa trạng thái sách tài khoản");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Taikhoan_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(id_encr);
                }
                List<USERS> user = _thietlap.Get_Taikhoan();
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                List<USER_PHONGBAN> phongban = _thietlap.Get_List_Phongban();
                List<USER_GROUP> group = _thietlap.Get_All_Usergroup();
                Response.Write(tl.List_DonVi_TaiKhoan(phongban, coquan, user, group, id, GetUserInfor().tk_action.iUser));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm sách tài khoản");
                //return null;
                throw;
            }
        }
        public ActionResult Taikhoan_phanquyen()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                string id_re = Request["id"];
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(id_re, Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_taikhoanphanquyen", id);
                int iUser = u_info.tk_action.iUser;
                if (!_base.IS_BanDanNguyen(iUser))
                {
                    if (IDDonVi_User(iUser) != IDDonVi_User(id))
                    {
                        Response.Redirect("/Home/Error");
                    }
                }
                ViewData["taikhoan"] = _thietlap.Get_User(id);
                ViewData["id"] = Request["id"];
                List<USER_GROUP> us_group = _thietlap.Get_Usergroup();
                List<ACTION> us_action = _thietlap.GetBy_List_Action();
                ViewData["list_group"] = tl.List_CheckBox_Group(us_group, id, u_info.tk_action);
                ViewData["list_action"] = tl.List_CheckBox_Taikhoan_action(us_action, id, u_info.tk_action);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Phân quyền sách tài khoản");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Taikhoan_phanquyen(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUserLogin = u_info.tk_action.iUser;
                int iUser = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //
                if (!CheckTokenAction("thietlap_taikhoanphanquyen", iUser))
                {
                    Response.Redirect("/Home/Error");
                    return null;
                }

                // db.Database.ExecuteSqlCommand("delete from user_action where iUser=" + iUser);
                USERS u = _thietlap.Get_User(iUser);
                string group = fc["group"];
                if (group != "") { group = "," + group + ","; }
                u.CARRGROUP = group;
                _thietlap.Update_Taikhoan(u);
                _condition = new Dictionary<string, object>();
                _condition.Add("IUSER", iUser);
                List<USER_ACTION> ua = _thietlap.GetBy_List_User_Action(_condition);
                foreach (var i in ua)
                {
                    USER_ACTION uac = _thietlap.Get_User_Action((int)i.ID);
                    _thietlap.Delete_User_Action(uac);
                }

                if (fc["action"] != null)
                {
                    string arr = fc["action"];
                    if (arr != "")
                    {
                        foreach (var x in arr.Split(','))
                        {
                            if (x != "")
                            {
                                USER_ACTION a = new USER_ACTION();
                                a.IUSER = iUser;
                                a.IACTION = Convert.ToInt32(x);
                                _thietlap.Insert_User_Action(a);
                            }
                        }
                    }
                }


                Tracking(iUserLogin, "Phân quyền tài khoản: " + u.CTEN + "thuộc đơn vị " + _base.Get_TenDonVi((int)u.IUSER));
                Response.Redirect(Request.Cookies["url_return"].Value);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Phân quyền sách tài khoản");
                //return null;
                throw;
            }
        }
        public string Get_Option_KyHop(int iKyHop = 0)
        {
            List<QUOCHOI_KHOA> khoa = _kiennghi.GetAll_KhoaHop().OrderBy(x => x.DBATDAU).ToList();
            List<QUOCHOI_KYHOP> kyhop = _kiennghi.GetAll_KyHop().OrderBy(x => x.DBATDAU).ToList();
            return "<option value='0'>Chọn kỳ họp</option>" + kn.Option_Khoa_KyHop(khoa, kyhop, iKyHop);
        }
        public string Get_Option_Khoa(int iKyHop = 0)
        {
            List<QUOCHOI_KHOA> khoa = _kiennghi.GetAll_KhoaHop().OrderBy(x => x.DBATDAU).ToList();
            return "<option value='0'>Chọn khoá</option>" + kn.Option_Khoa(khoa, iKyHop);
        }
        public string Get_Option_Khoa_By_Loai(int iLoai, int iKhoa = 0)
        {
            List<QUOCHOI_KHOA> khoa = _kiennghi.GetAll_KhoaHop().OrderBy(x => x.DBATDAU).ToList();
            string str = "";
            if (iLoai != -1)
                khoa = khoa.Where(x=>x.ILOAI == iLoai).OrderBy(x => x.DBATDAU).ToList();
            foreach (var k in khoa)
            {
                str += "<option ";
                if (iKhoa == k.IKHOA)
                    str += "selected ";
                str += "value='" + k.IKHOA + "'>" + k.CTEN + "</option>";
            }
            return "<option value='0'>Chọn khoá</option>" +str;
        }

        public string Get_Option_Loai_Dai_bieu(int iLoai)
        {
            string str = "";
            for (int i = -1; i < 2; i++)
            {
                str += "<option ";

                if (i == iLoai)
                    str += "selected ";
                str += "value = '" + i + "' > ";
                if(i == 0)
                    str += "Đại biểu Quốc hội </option> ";
                else if (i == 1)
                    str += "Đại biểu HDND </option> ";
                else
                    str += "Chọn loại đại biểu </option> ";
            }
            return str;
        }
        // Đại biểu quốc hội
        public ActionResult Daibieu(int page = 1)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();

                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
              
                if (!base_buss.Action_(41, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                string donvi = "";
                if (Request["q"] != null)
                {
                    donvi = func.RemoveTagInput(Request["q"]);
                }
                int iKyHop = 0;
                int iDaibieu = 0;
                int iLoaiDaiBieu = -1;
                if (Request["iLoai"] != null)
                {
                    iLoaiDaiBieu = Convert.ToInt32(Request["iLoai"]);
                }
                if (Request["iKyHop"] != null)
                {
                    iKyHop = Convert.ToInt32(Request["iKyHop"]);
                }
                if (Request["q"] != null)
                {
                    string id_encr = Request["q"];
                    if (id_encr != "0")
                    {
                        iDaibieu = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                    }
                }
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                List<THIETLAP_DAIBIEU_PHANTRANG> daibieu = thutuc.Get_Daibieu_Phantrang("PKG_THIETLAP_HETHONG.PRC_DAIBIEU_PHANTRANG", iDaibieu, iKyHop, iLoaiDaiBieu, page, post_per_page).ToList();
                List<DIAPHUONG> diaphuong = _thietlap.Get_Diaphuong();
                if (daibieu != null && diaphuong != null && daibieu.Count() > 0)
                {
                    if (Request["q"] != null)
                    {
                        ViewData["list"] = tl.List_Doandaibieu_search(diaphuong, daibieu, iUser, "", iDaibieu);
                        ViewData["phantrang"] = "<tr><td colspan='10'>" + base_appcode.PhanTrang((int)daibieu.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    } else
                    {
                        ViewData["list"] = tl.List_Doandaibieu(diaphuong, daibieu, iUser, "", donvi, iKyHop);
                        ViewData["phantrang"] = "<tr><td colspan='10'>" + base_appcode.PhanTrang((int)daibieu.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    }
                }
                ViewData["Option_daibieu"] = tl.Option_Doandaibieu(iUser);
                ViewData["Option_LoaiDaiBieu"] = Get_Option_Loai_Dai_bieu(iLoaiDaiBieu);
                //  int iKyHop = ID_KyHop_HienTai();
                ViewData["khoa"] = Get_Option_Khoa_By_Loai(iLoaiDaiBieu, iKyHop);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Danh sách đại biểu");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Daibieu_add()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(41, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                //ViewData["listDiaPhuong"] = JsonConvert.SerializeObject(tl.Option_DiaPhuong_ByID_Parent(0));
                ViewData["listDiaPhuong"] = "<option selected value ='" + ID_DiaPhuong_HienTai + "'>" + _thietlap.GetBy_DiaphuongID(ID_DiaPhuong_HienTai).CTEN + "</option>";
                //ViewData["opt-huyen"] = tl.Option_TinhThanh_ByID_Parent(ID_DiaPhuong_HienTai, 0); // Cac Quan, huyen thuoc dia phuong du an hien tai (Thanh Hoa)
                ViewData["opt-huyen"] = tl.Option_To_Dai_Bieu();
                var _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IHIENTHI", 1);
                _dCondition.Add("IDELETE", 0);
                ViewData["opt-khoa"] = JsonConvert.SerializeObject(_thietlap.Get_List_Quochoi_Khoa(_dCondition));
                ViewData["opt-kyhop"] = JsonConvert.SerializeObject(_thietlap.Get_List_Quochoi_Kyhop(_dCondition));
                ViewData["khoa"] = JsonConvert.ToString(Get_Option_Khoa());
                SetTokenAction("daibieu_add", 0);
                return PartialView("../Ajax/Thietlap/Daibieu_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách đại biểu");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Daibieu_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(41, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                DAIBIEU d = _thietlap.GetBy_DaibieuID(id);
                //ViewData["listDiaPhuong"] = JsonConvert.SerializeObject(tl.Option_DiaPhuong_ByID_Parent(0));
                ViewData["listDiaPhuong"] = "<option selected value ='" + ID_DiaPhuong_HienTai + "'>" + _thietlap.GetBy_DiaphuongID(ID_DiaPhuong_HienTai).CTEN + "</option>";
                var _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IHIENTHI", 1);
                _dCondition.Add("IDELETE", 0);
                ViewData["opt-khoa"] = JsonConvert.SerializeObject(_thietlap.Get_List_Quochoi_Khoa(_dCondition));
                ViewData["opt-kyhop"] = JsonConvert.SerializeObject(_thietlap.Get_List_Quochoi_Kyhop(_dCondition));
                //ViewData["opt-huyen"] = tl.Option_TinhThanh_ByID_Parent(ID_DiaPhuong_HienTai, (int)d.IDIAPHUONG);
                ViewData["opt-huyen"] = tl.Option_To_Dai_Bieu((int)d.IDIAPHUONG);
                var _daibieuKyHopCondition = new Dictionary<string, object>();
                _daibieuKyHopCondition.Add("ID_DAIBIEU", d.IDAIBIEU);
                var listDaiBieuKyHop = _thietlap.GetBy_List_DaiBieu_KyHop(_daibieuKyHopCondition);
                ViewData["listDaiBieuKyHop"] = JsonConvert.SerializeObject(listDaiBieuKyHop.Select(x => x.ID_KYHOP));
                ViewData["daibieu"] = d;
                ViewData["id"] = fc["id"];
                if(d.DNGAYSINH != null)
                {
                    ViewData["Ngaysinh"] = func.ConvertDateVN(d.DNGAYSINH.ToString());
                }
               
                ViewData["List"] = tl.List_CheckBox_KyHop(id);
                SetTokenAction("daibieu_edit", id);
                return PartialView("../Ajax/Thietlap/Daibieu_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách đại biểu");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Daibieu_insert(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Daibieu().Where(v => v.CCODE != null && v.CCODE == cTen && v.IDIAPHUONG == Convert.ToInt32(Request["iDiaPhuong0"]) && v.IDELETE == 0 && (v.CEMAIL.Trim() == func.RemoveTagInput(Request["cEmail"]).Trim() || v.CSDT.Trim() == func.RemoveTagInput(Request["cSDT"]).Trim())).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("daibieu_add")) { Response.Redirect("/Home/Error/"); return null; }
                    DAIBIEU d = new DAIBIEU();
                    d.CTEN = func.RemoveTagInput(Request["cTen"]);
                    d.CEMAIL = func.RemoveTagInput(Request["cEmail"]);
                    d.CSDT = func.RemoveTagInput(Request["cSDT"]);
                    d.ILOAIDAIBIEU = Convert.ToInt32(fc["iLoaiDaiBieu"]);
                    //DBQH thi chi co o cap tinh, HDND thi se co dia phuong o cap thap hon
                    if (d.ILOAIDAIBIEU == 1)
                        d.IDIAPHUONG = Convert.ToInt32(Request["iDiaPhuong1"]);
                    else
                        d.IDIAPHUONG = Convert.ToInt32(Request["iDiaPhuong0"]);
                    d.ITRUONGDOAN = 0;
                    d.ITOTRUONG = 0;
                    d.IHIENTHI = 1;
                    d.IDELETE = 0;
                    d.IGIOITINH = Convert.ToInt32(fc["iGioiTinh"]);
                    d.DNGAYSINH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgaySinh"]));
                    d.CDOANDB = fc["cDoanDB"];
                    d.CNOILAMVIEC = fc["cNoiLamViec"];
                    d.CCOQUAN = fc["cCoQuan"];
                    d.CCHUCVUDAYDU = fc["cChucVuDayDu"];
                    d.CCODE = fc["cCode"];
                    d.CDONVIBAUCUSO = fc["cdonvibaucuso"];
                    d.IVITRI = 1;
                    if (Request["iTruongDoan"] != null) { d.ITRUONGDOAN = 1; }
                    if (Request["iToTruong"] != null) { d.ITOTRUONG = 1; }
                    _thietlap.Insert_Daibieu(d);
                    string tendiaphuong = _thietlap.GetBy_DiaphuongID((int)d.IDIAPHUONG).CTEN;
                    Tracking(id_user(), "Thêm đại biểu <strong>" + d.CTEN + "</strong> - " + tendiaphuong);
                    UserInfor u_info = GetUserInfor();
                    int iUser = u_info.tk_action.iUser;
                    int id = (int)d.IDAIBIEU;
                    _condition = new Dictionary<string, object>();
                    _condition.Add("ID_DAIBIEU", id);
                    List<DAIBIEU_KYHOP> ua = _thietlap.GetBy_List_DaiBieu_KyHop(_condition);
                    foreach (var i in ua)
                    {
                        DAIBIEU_KYHOP uac = _thietlap.GetBy_DaiBieu_KyHopID((int)i.ID);
                        _thietlap.Delete_DaiBieu_KyHop(uac);
                    }

                    if (fc["action"] != null)
                    {
                        string arr = fc["action"];
                        if (arr != "")
                        {
                            foreach (var x in arr.Split(','))
                            {
                                if (x != "")
                                {
                                    DAIBIEU_KYHOP a = new DAIBIEU_KYHOP();
                                    a.ID_DAIBIEU = d.IDAIBIEU;
                                    a.ID_KYHOP = Convert.ToInt32(x);
                                    _thietlap.Insert_DaiBieu_KyHop(a);
                                }
                            }
                        }
                    }
                    Tracking(iUser, "Chọn kỳ họp cho đại biểu " + _thietlap.GetBy_DaibieuID(id).CTEN + "");
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách đại biểu");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Daibieu_update(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                DAIBIEU dbieu = _thietlap.GetBy_DaibieuID(id);
                string cTen = func.RemoveTagInput(fc["cTen"]);
                if (_thietlap.Get_Daibieu().Where(v => v.CCODE != null && v.CCODE == cTen && v.IDIAPHUONG == Convert.ToInt32(Request["iDiaPhuong0"]) && v.IDELETE == 0 && v.IDAIBIEU != id && (v.CEMAIL.Trim() == func.RemoveTagInput(Request["cEmail"]).Trim() || v.CSDT.Trim() == func.RemoveTagInput(Request["cSDT"]).Trim())).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("daibieu_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                    DAIBIEU d = _thietlap.GetBy_DaibieuID(id);
                    d.IGIOITINH = Convert.ToInt32(fc["iGioiTinh"]);
                    d.DNGAYSINH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgaySinh"]));
                    d.CDOANDB = fc["cDoanDB"];
                    d.CNOILAMVIEC = fc["cNoiLamViec"];
                    d.CCOQUAN = fc["cCoQuan"];
                    d.CCHUCVUDAYDU = fc["cChucVuDayDu"];
                    d.CTEN = func.RemoveTagInput(Request["cTen"]);
                    d.CEMAIL = func.RemoveTagInput(Request["cEmail"]);
                    d.CSDT = func.RemoveTagInput(Request["cSDT"]);
                    d.ILOAIDAIBIEU = Convert.ToInt32(fc["iLoaiDaiBieu"]);
                    if (d.ILOAIDAIBIEU == 1)
                        d.IDIAPHUONG = Convert.ToInt32(Request["iDiaPhuong1"]);
                    else
                        d.IDIAPHUONG = Convert.ToInt32(Request["iDiaPhuong0"]);
                    d.ITRUONGDOAN = 0;
                    d.ITOTRUONG = 0;
                    d.CCODE = fc["cCode"];
                    d.CDONVIBAUCUSO = fc["cdonvibaucuso"];
                    if (Request["iTruongDoan"] != null) { d.ITRUONGDOAN = 1; }
                    if (Request["iToTruong"] != null) { d.ITOTRUONG = 1; }
                    _thietlap.Update_Daibieu(d);
                    string tendiaphuong = _thietlap.GetBy_DiaphuongID((int)d.IDIAPHUONG).CTEN;
                    Tracking(id_user(), "Cập nhật lại đại biểu <strong>" + d.CTEN + "</strong> - " + tendiaphuong);
                    UserInfor u_info = GetUserInfor();
                    int iUser = u_info.tk_action.iUser;
                   // int id = Convert.ToInt32(fc["iddaibieu"]);
                    _condition = new Dictionary<string, object>();
                    _condition.Add("ID_DAIBIEU", id);
                    List<DAIBIEU_KYHOP> ua = _thietlap.GetBy_List_DaiBieu_KyHop(_condition);
                    foreach (var i in ua)
                    {
                        DAIBIEU_KYHOP uac = _thietlap.GetBy_DaiBieu_KyHopID((int)i.ID);
                        _thietlap.Delete_DaiBieu_KyHop(uac);
                    }

                    if (fc["action"] != null)
                    {
                        string arr = fc["action"];
                        if (arr != "")
                        {
                            foreach (var x in arr.Split(','))
                            {
                                if (x != "")
                                {
                                    DAIBIEU_KYHOP a = new DAIBIEU_KYHOP();
                                    a.ID_DAIBIEU = id;
                                    a.ID_KYHOP = Convert.ToInt32(x);
                                    _thietlap.Insert_DaiBieu_KyHop(a);
                                }
                            }
                        }
                    }
                    Tracking(iUser, "Chọn kỳ họp cho đại biểu " + _thietlap.GetBy_DaibieuID(id).CTEN + "");
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách đại biểu");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Daibieu_status(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                DAIBIEU tk = _thietlap.GetBy_DaibieuID(id);
                if (tk.IHIENTHI == 1)
                {
                    tk.IHIENTHI = 0;
                    Tracking(id_user(), "Bỏ hiển thị đại biểu: <strong>" + tk.CTEN + "</strong> thuộc địa phương <strong>" +
                      _thietlap.GetBy_DiaphuongID((int)tk.IDIAPHUONG).CTEN + "</strong>");

                }
                else
                {
                    tk.IHIENTHI = 1;
                    Tracking(id_user(), "Chọn hiển thị đại biểu: <strong>" + tk.CTEN + "</strong> thuộc địa phương <strong>" +
                        _thietlap.GetBy_DiaphuongID((int)tk.IDIAPHUONG).CTEN + "</strong>");
                }
                _thietlap.Update_Daibieu(tk);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa trạng thái danh sách đại biểu");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Daibieu_del(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(41, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                DAIBIEU dbieu = _thietlap.GetBy_DaibieuID(id);
                dbieu.IDELETE = 1;
                dbieu.IHIENTHI = 0;
                _thietlap.Update_Daibieu(dbieu);
                Tracking(id_user(), "Xóa đại biểu  - " + dbieu.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa trạng thái danh sách đại biểu");
                //return null;
                throw;
            }
        }
        /*public ActionResult Ajax_Daibieu_search(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(41, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                List<DIAPHUONG> diaphuong = _thietlap.Get_Diaphuong();
                List<DAIBIEU> daibieu = _thietlap.Get_Daibieu();
                Response.Write(tl.List_Doandaibieu_search(diaphuong, daibieu, id_user(), Request.Cookies["url_key"].Value, id));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "tìm kiếm danh sách đại biểu");
                //return null;
                throw;
            }

        }*/
        // Linhx vực
        public ActionResult Linhvuc()
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc().OrderBy(x => x.CTEN).ToList();
                ViewData["list"] = tl.List_LinhVuc(linhvuc, 0, 0, id_user(), Request.Cookies["url_key"].Value);
                ViewData["option_linhvuc"] = tl.OptionLinhVuc_ThietLap(linhvuc, 0, 0, id_user(), Request.Cookies["url_key"].Value);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, " danh sách lĩnh vực");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Linhvuc_add()
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_themlinhvuc");
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc().OrderBy(x => x.CTEN).ToList();
                List<KNTC_LOAIDON> loaidon = _thietlap.Get_Loaidon();
                List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                ViewData["opt-linhvuc"] = tl.Option_LinhVucParent(linhvuc);
                ViewData["opt-loaidon"] = tl.Option_LoaiDon(loaidon, 0);
                ViewData["opt-noidung"] = tl.Option_NoiDungDon(noidungdon, 0);
                ViewData["list_group"] = tl.List_CheckBox_CoQuan();
                ViewData["opt_donvi"] = vb.Option_LinhVuc_Coquan(coquan, 0, 0, Request.Cookies["url_key"].Value, 0);
                return PartialView("../Ajax/Thietlap/Linhvuc_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách lĩnh vực");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_LoadNoiDungDon(int id)
        {
            try
            {   
                 if (!CheckAuthToken()) { return null; }    
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                
              
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc().OrderBy(x => x.CTEN).ToList();
                List<KNTC_LOAIDON> loaidon = _thietlap.Get_Loaidon();
                ViewData["opt-linhvuc"] = tl.Option_LinhVucParent(linhvuc);
                ViewData["opt-loaidon"] = tl.Option_LoaiDon(loaidon, 0);
                if(id != 0)
                {
                    Response.Write("<select name='iLoaidon' id='iLoaidon' class='input-block-level'>"+ tl.Option_LoaiDon(loaidon, 0)+"</selected>");
                }
                else
                {
                    Response.Write("<select name='iLoaidon' id='iLoaidon' class='input-block-level'><option value='0'> - - - Chưa xác định</option></selected>");

                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách lĩnh vực");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Linhvuc_insert(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                var listAllLinhVuc = _thietlap.Get_Linhvuc();
                int iUser = u_info.tk_action.iUser;
                string cCode = func.RemoveTagInput(fc["cCode"]);
                var list = _thietlap.Get_List_LinhVuc_SQL_CheckInsert(cCode.ToUpper().Trim()).ToList();


                string cTen = func.RemoveTagInput(fc["cTen"]);
                //Kiểm tra trùng tên
                if (listAllLinhVuc.Where(v => v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.ILOAIDON == Convert.ToInt32(fc["iLoaidon"]) && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                    return null;
                }

                if (list.Count() > 0)
                {
                    Response.Write(3);
                    return null;
                }
                else
                {
                    if (!CheckTokenAction("thietlap_themlinhvuc"))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    LINHVUC donvi = new LINHVUC();
                    int iParent = Convert.ToInt32(fc["iParent"]);
                    donvi.IHIENTHI = 1;
                    donvi.IDELETE = 0;
                    donvi.IPARENT = iParent;
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.INHOM = Convert.ToInt32(fc["iNhom"]);
                    donvi.ILOAIDON = Convert.ToInt32(fc["iLoaidon"]);
                    _thietlap.Insert_Linhvuc(donvi);
                    Tracking(iUser, "Thêm mới lĩnh vực: " + donvi.CTEN);
                    int idonvi = Convert.ToInt32(fc["iDonvi"]);
                    if (idonvi != -1)
                    {
                        DONVI_LINHVUC a = new DONVI_LINHVUC();
                        a.IDONVI = idonvi;
                        a.ILINHVUC = (int)donvi.ILINHVUC;
                        a.IDELETE = 0;
                        _thietlap.Insert_DonVi_LinhVuc(a);
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
                                    DONVI_LINHVUC a = new DONVI_LINHVUC();
                                    a.IDONVI = Convert.ToInt32(x);
                                    a.ILINHVUC = (int)donvi.ILINHVUC;
                                    a.IDELETE = 0;
                                    _thietlap.Insert_DonVi_LinhVuc(a);
                                }
                            }
                        }

                    }
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách lĩnh vực");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Linhvuc_status(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                LINHVUC q = _thietlap.GetBy_LinhvucID(id);
                if ((int)q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUser, "Bỏ chọn áp dụng lĩnh vực: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUser, "Chọn áp dụng lĩnh vực: " + q.CTEN);
                }
                _thietlap.Update_Linhvuc(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm trạng thái danh sách lĩnh vực");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Linhvuc_edit(FormCollection fc)
        {
           
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_sualinhvuc", id);
                LINHVUC donvi = _thietlap.GetBy_LinhvucID(id);
                ViewData["id"] = fc["id"];
                ViewData["linhvuc"] = donvi;
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc().OrderBy(x => x.CTEN).ToList();
                List<KNTC_LOAIDON> loaidon = _thietlap.Get_Loaidon();
                ViewData["opt-linhvuc"] = tl.Option_LinhVucParent_edit(linhvuc, (int)donvi.ILINHVUC,(int)donvi.IPARENT);
                ViewData["opt-loaidon"] = tl.Option_LoaiDon(loaidon,(int)donvi.ILOAIDON);
                return PartialView("../Ajax/Thietlap/Linhvuc_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách lĩnh vực");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Linhvuc_del(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var thongtinnhomnoidungdon = _thietlap.Get_Noidungdon().Where(x => x.ILINHVUC == id).ToList();
                if (thongtinnhomnoidungdon.Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    LINHVUC donvi = _thietlap.GetBy_LinhvucID(id);
                    donvi.IDELETE = 1;
                    donvi.IHIENTHI = 0;
                    donvi.IPARENT = Convert.ToInt32(fc["iParent"]);
                    _thietlap.Update_Linhvuc(donvi);
                    Tracking(id_user(), "Xóa lĩnh vực: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa danh sách lĩnh vực");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Linhvuc_update(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //
                var listAllLinhVuc = _thietlap.Get_Linhvuc();
                string cCode = func.RemoveTagInput(fc["cCode"]);
                string cTen = func.RemoveTagInput(fc["cTen"]);
                var list = _thietlap.Get_List_LinhVuc_SQL_CheckUpdate(cCode.ToUpper().Trim(), id).ToList();
                if (listAllLinhVuc.Where(v => v.CTEN != null && v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.ILINHVUC != id && v.ILOAIDON == Convert.ToInt32(fc["iLoaidon"]) && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else if (list.Count() > 0)
                {
                    Response.Write(3);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_sualinhvuc", id))
                    {
                        Response.Redirect("/Home/Error");
                        return null;
                    }
                    LINHVUC donvi = _thietlap.GetBy_LinhvucID(id);
                    donvi.IPARENT = Convert.ToInt32(fc["iParent"]);
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.INHOM = Convert.ToInt32(fc["iNhom"]);
                    donvi.ILOAIDON = Convert.ToInt32(fc["iLoaidon"]);
                    _thietlap.Update_Linhvuc(donvi);
                    int iUserLogin = u_info.tk_action.iUser;
                    Tracking(iUserLogin, "Cập nhật lại lĩnh vực: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách lĩnh vực");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Linhvuc_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                List<KNTC_NOIDUNGDON> noidung = _thietlap.Get_Noidungdon().ToList();
             
                //
                 Response.Write(tl.List_LinhVuc_search(linhvuc,0,0, id_user(),"",id));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách lĩnh vực");
                //return null;
                throw;
            }
        }
        //Loại đơn
        public ActionResult Loaidon()
        {
            try
            {

                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                
                if (!base_buss.Action_(27, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                List<KNTC_LOAIDON> loaidon = _thietlap.Get_Loaidon();
                ViewData["list"] = tl.List_LoaiDon(loaidon, iUser);

                ViewData["Option_loaidon"] = tl.Option_LoaiDon_();
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách loại đơn");
                return View("../Home/Error_Exception");
            }
        }

        public ActionResult Ajax_Loaidon_add()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(27, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_themloaidon");
                //ViewData["opt-linhvuc"] = _base.OptionLinhVuc_ThietLap();
                return PartialView("../Ajax/Thietlap/Loaidon_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách loại đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Loaidon_status(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(27, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
               
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_LOAIDON q = _thietlap.GetBy_LoaidonID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUser, "Bỏ chọn áp dụng loại đơn: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUser, "Chọn áp dụng loại đơn: " + q.CTEN);
                }
                _thietlap.Update_Loaidon(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái danh sách loại đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Loaidon_edit(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(27, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_sualoaidon", id);
                KNTC_LOAIDON donvi = _thietlap.GetBy_LoaidonID(id);
                ViewData["Loaidon"] = donvi;
                ViewData["id"] = fc["id"];
                Tracking(id_user(), "Cập nhật lại loại đơn: " + donvi.CTEN);
                return PartialView("../Ajax/Thietlap/Loaidon_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách loại đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Loaidon_del(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(27, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_LOAIDON donvi = _thietlap.GetBy_LoaidonID(id);
                donvi.IDELETE = 1;
                donvi.IHIENTHI = 0;
                _thietlap.Update_Loaidon(donvi);
                Tracking(id_user(), "Xóa loại đơn: " + donvi.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa danh sách loại đơn");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Loaidon_insert(FormCollection fc)
        {

            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(27, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                KNTC_LOAIDON donvi = new KNTC_LOAIDON();
                string cTen = func.RemoveTagInput(fc["cCode"]);
                //
                if (_thietlap.Get_Loaidon().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_themloaidon"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.IHIENTHI = 1;
                    donvi.IDELETE = 0;
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.IVITRI = 1;
                    _thietlap.Insert_Loaidon(donvi);
                    Tracking(iUser, "Thêm mới loại đơn: " + donvi.CTEN);
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách loại đơn");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Loaidon_update(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(27, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cCode"]);
                //
                if (_thietlap.Get_Loaidon().Where(v =>v.CCODE != null  && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.ILOAIDON != id && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    //
                    if (!CheckTokenAction("thietlap_sualoaidon", id))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    KNTC_LOAIDON donvi = _thietlap.GetBy_LoaidonID(id);
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    _thietlap.Update_Loaidon(donvi);
                    // int iUserLogin = id_user();
                    Tracking(iUser, "Cập nhật lại loại đơn: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách loại đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Loaidon_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(27, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                List<KNTC_LOAIDON> loaidon = _thietlap.Get_Loaidon();
                Response.Write(tl.List_LoaiDon_search(loaidon, id_user(), id, Request.Cookies["url_key"].Value));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách loại đơn");
                //return null;
                throw;
            }
        }
        // Noij dung don

        public ActionResult Noidungdon()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;

                if (!base_buss.Action_(28, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
                ViewData["list"] = tl.List_Nhom_Noidungdon(linhvuc, noidungdon, iUser, Request.Cookies["url_key"].Value);

                ViewData["Option_noidungdon"] = tl.Option_Nhom_Noidungdon(linhvuc, noidungdon, iUser, Request.Cookies["url_key"].Value);

                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách nội dung loại đơn");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_Noidungdon_status(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(28, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_NOIDUNGDON q = _thietlap.GetBy_NoidungdonID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUser, "Bỏ chọn áp dụng nội dung  đơn: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUser, "Chọn áp dụng nội dung đơn: " + q.CTEN);
                }
                _thietlap.Update_Noidungdon(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Trạng thái danh sách nội dung loại đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Noidungdon_add()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(28, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                SetTokenAction("thietlap_themnoidungdon");
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();

                ViewData["opt-Option_linhvuc"] = tl.Option_LinhVuc_ChonNoiDung_Themmoi_NoiDung(linhvuc);
                //ViewData["opt-linhvuc"] = _base.OptionLinhVuc_ThietLap();
                return PartialView("../Ajax/Thietlap/Noidungdon_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách nội dung loại đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Noidungdon_edit(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(28, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suanoidungdon", id);
                KNTC_NOIDUNGDON donvi = _thietlap.GetBy_NoidungdonID(id);
                ViewData["Noidungdon"] = donvi;
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();

                ViewData["Option_linhvuc"] = tl.Option_LinhVuc_ChonNoiDung(linhvuc, 0, 0, (int)donvi.ILINHVUC, (int)donvi.ILINHVUC);
                ViewData["id"] = fc["id"];


                return PartialView("../Ajax/Thietlap/Noidungdon_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách nội dung loại đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Noidungdon_del(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(28, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var thongtintinhchat = _thietlap.Get_Tinhchat().Where(x => x.INHOMNOIDUNG == id).ToList();
                if (thongtintinhchat.Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    KNTC_NOIDUNGDON donvi = _thietlap.GetBy_NoidungdonID(id);
                    donvi.IHIENTHI = 0;
                    donvi.IDELETE = 1;
                    _thietlap.Update_Noidungdon(donvi);
                    Tracking(u_info.tk_action.iUser, "Xóa nội dung đơn: " + donvi.CTEN);
                    Response.Write(1);

                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Xóa danh sách nội dung loại đơn");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Noidungdon_insert(FormCollection fc)
        {
            //
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                string cTen = func.RemoveTagInput(fc["cCode"]);
                //
                if (_thietlap.Get_Noidungdon().Where(v => v.CCODE !=null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {

                    if (!CheckTokenAction("thietlap_themnoidungdon"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    KNTC_NOIDUNGDON donvi = new KNTC_NOIDUNGDON();
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.IHIENTHI = 1;
                    donvi.IDELETE = 0;
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.ILINHVUC = Convert.ToDecimal(fc["iNhomLinhVuc"]);
                    _thietlap.Insert_Noidungdon(donvi);
                    Tracking(u_info.tk_action.iUser, "Thêm mới nội dung đơn: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách nội dung loại đơn");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Noidungdon_update(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUserLogin = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cCode"]);
                //
                if (_thietlap.Get_Noidungdon().Where(v => v.CCODE != null && v.CCODE == cTen && v.INOIDUNG != id && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suanoidungdon", id))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    KNTC_NOIDUNGDON donvi = _thietlap.GetBy_NoidungdonID(id);
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.ILINHVUC = Convert.ToDecimal(fc["iNhomLinhVuc"]);
                    _thietlap.Update_Noidungdon(donvi);
                    Tracking(iUserLogin, "Cập nhật lại nội dung đơn: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách nội dung loại đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Noidungdon_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(28, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32((id_encr));
                }
                List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
                List<LINHVUC> Linhvuc = _thietlap.Get_Linhvuc();
                Response.Write(tl.List_Noidungdon_search(Linhvuc, noidungdon, id_user(), id, Request.Cookies["url_key"].Value));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách nội dung loại đơn");
                //return null;
                throw;
            }
        }

        // tinh chat don
        public ActionResult Tinhchatdon()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                if (!base_buss.Action_(29, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
                List<KNTC_TINHCHAT> tinhchat = _thietlap.Get_Tinhchat();
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                ViewData["list"] = tl.List_Nhom_TinhChatDon(linhvuc, noidungdon, tinhchat, iUser, Request.Cookies["url_key"].Value);
                ViewData["Option_tinhchat"] = tl.Option_TinhChatDon_(iUser);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách tính chất đơn");
                return View("../Home/Error_Exception");
            }
        }

        public ActionResult Ajax_Tinhchatdon_status(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(29, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_TINHCHAT q = _thietlap.GetBy_TinhchatID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUser, "Bỏ chọn áp dụng tính chất  đơn: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUser, "Chọn áp dụng tính chất đơn: " + q.CTEN);
                }
                _thietlap.Update_Tinhchat(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái danh sách tính chất đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Tinhchatdon_add()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(29, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_themtinhchat");
                List<KNTC_NOIDUNGDON> nguondon = _thietlap.Get_Noidungdon();
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                ViewData["nhomdon"] = tl.Option_Nhom_Noidungdon(linhvuc, nguondon, id_user(), Request.Cookies["url_key"].Value);
              


                ViewData["opt-Option_linhvuc"] = tl.Option_LinhVuc_ChonNoiDung(linhvuc);
                return PartialView("../Ajax/Thietlap/Tinhchatdon_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách tính chất đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Tinhchatdon_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(29, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suatinhchat", id);
                KNTC_TINHCHAT donvi = _thietlap.GetBy_TinhchatID(id);
                ViewData["Tinhchatdon"] = donvi;
                List<LINHVUC> linhvuc = _thietlap.Get_Linhvuc();
                List<KNTC_NOIDUNGDON> ND = _thietlap.Get_Noidungdon();
                ViewData["opt-nhomdon"] = tl.Option_Nhom_Noidungdon(linhvuc, ND, id_user(), Request.Cookies["url_key"].Value, (int)donvi.INHOMNOIDUNG);
               
                ViewData["opt-Option_linhvuc"] = tl.Option_LinhVuc_ChonNoiDung(linhvuc);
                ViewData["id"] = fc["id"];
                return PartialView("../Ajax/Thietlap/Tinhchatdon_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách tính chất đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Tinhchatdon_del(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(29, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
               
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_TINHCHAT donvi = _thietlap.GetBy_TinhchatID(id);
                donvi.IHIENTHI = 0;
                donvi.IDELETE = 1;
                _thietlap.Update_Tinhchat(donvi);
                Tracking(iUser, "Xóa tính chất đơn: " + donvi.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa danh sách tính chất đơn");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Tinhchatdon_insert(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                string cTen = func.RemoveTagInput(fc["cCode"]);
                //
                if (_thietlap.Get_Tinhchat().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    //
                    if (!CheckTokenAction("thietlap_themtinhchat"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }


                    KNTC_TINHCHAT donvi = new KNTC_TINHCHAT();


                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.IHIENTHI = 1;
                    donvi.IDELETE = 0;
                    donvi.IVITRI = 1;
                    donvi.CCODE = fc["cCode"];
                    donvi.INHOMNOIDUNG = Convert.ToDecimal(fc["iNhomnoidung"]);
                    _thietlap.Insert_Tinhchat(donvi);
                    Tracking(iUser, "Thêm mới tính chất đơn: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách tính chất đơn");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Tinhchatdon_update(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                string cTen = func.RemoveTagInput(fc["cTen"]);


                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));

                if (_thietlap.Get_Tinhchat().Where(v => v.CCODE  != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() &&  v.ITINHCHAT != id && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    //
                    if (!CheckTokenAction("thietlap_suatinhchat", id))
                    {
                        // Response.Redirect("/Home/Error");
                        return null;
                    }

                    KNTC_TINHCHAT donvi = _thietlap.GetBy_TinhchatID(id);
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.INHOMNOIDUNG = Convert.ToDecimal(fc["iNhomnoidung"]);
                    donvi.CCODE = fc["cCode"];
                    _thietlap.Update_Tinhchat(donvi);
                    UserInfor u_info = GetUserInfor();
                    int iUser = u_info.tk_action.iUser;
                    Tracking(iUser, "Cập nhật lại tính chất đơn: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách tình chất đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Tinhchatdon_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(29, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                List<KNTC_NOIDUNGDON> noidungdon = _thietlap.Get_Noidungdon();
                List<KNTC_TINHCHAT> tinhchat = _thietlap.Get_Tinhchat();
                Response.Write(tl.List_Tinhchatdon_search(id_user(), id, Request.Cookies["url_key"].Value));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách tình chất đơn");
                //return null;
                throw;
            }
        }
        // Nguooif don
        public ActionResult Nguondon()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }

                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                if (!base_buss.Action_(30, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                TaikhoanAtion act = GetUserInfor().tk_action;
                List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                ViewData["list"] = tl.List_Nguondon_parent(nguondon, 0, 0, iUser, Request.Cookies["url_key"].Value);
                ViewData["Option_nguondon"] = tl.Option_Nguondon_(iUser);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách nguồn đơn");
                //return null;
                throw;
            }
        }

        public ActionResult Nguonkiennghi()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }

                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                if (!base_buss.Action_(30, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                TaikhoanAtion act = GetUserInfor().tk_action;
                List<KN_NGUONDON> nguondon = _thietlap.Get_Nguonkiennghi().OrderBy(x => x.IVITRI).ThenBy(x => x.CCODE).ToList();
                ViewData["list"] = tl.List_Nguonkiennghi_parent(nguondon, 0, 0, iUser, Request.Cookies["url_key"].Value);
                ViewData["Option_nguondon"] = tl.Option_Nguonkiennghi_(iUser);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách nguồn đơn");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_Nguondon_status(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(30, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
              
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_NGUONDON q = _thietlap.GetBy_NguondonID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUser, "Bỏ chọn áp dụng nguồn  đơn: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUser, "Chọn áp dụng nguồn đơn: " + q.CTEN);
                }
                _thietlap.Update_Nguondon(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái danh sách nguồn đơn");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Nguondon_add()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(30, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                SetTokenAction("thietlap_themnguondon");
                //ViewData["opt-linhvuc"] = _base.OptionLinhVuc_ThietLap();
                List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                ViewData["opt_nguondon"] = tl.Option_Nguondon_iparent(nguondon);
                return PartialView("../Ajax/Thietlap/Nguondon_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách nguồn đơn");
                //return null;
                throw;
            }
        }


        public ActionResult Ajax_Nguonkiennghi_add()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(30, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                SetTokenAction("thietlap_themnguonkiennghi");
                //ViewData["opt-linhvuc"] = _base.OptionLinhVuc_ThietLap();
                List<KN_NGUONDON> nguondon = _thietlap.Get_Nguonkiennghi();
                ViewData["opt_nguondon"] = tl.Option_Nguonkiennghi_iparent(nguondon);
                return PartialView("../Ajax/Thietlap/Nguonkiennghi_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách nguồn kiến nghị");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_Nguondon_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(30, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suanguondon", id);
                KNTC_NGUONDON donvi = _thietlap.GetBy_NguondonID(id);
                List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                ViewData["Nguondon"] = donvi;
                ViewData["idnguondon"] = donvi.INGUONDON;
                ViewData["opt_nguondon"] = tl.Option_NguonDon_ByID_Parent((int)donvi.ILOAI, (int)donvi.INGUONDON, 1);
                //ViewData["opt_nguondon"] = tl.Option_NguonDon_ByID_Parent((int)donvi.IPARENT, 0);
                ViewData["id"] = fc["id"];
                return PartialView("../Ajax/Thietlap/Nguondon_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách nguồn đơn");
                //return null;
                throw;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_LoadOption_NguonDon(int idparent, int id, int type)
        {
            string str = "";
            str = "<select name='iParent' id='iParent' class='input-block-level'><option value='0'>- - - Gốc</option>" +
                       tl.Option_NguonDon_ByID_Parent(idparent, id, type) + "</select>";

            Response.Write(str);
            return null;
        }

        public ActionResult Ajax_Nguonkiennghi_edit(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(30, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suanguonkiennghi", id);
                KN_NGUONDON donvi = _thietlap.GetBy_NguonkiennghiID(id);
                List<KN_NGUONDON> nguondon = _thietlap.Get_Nguonkiennghi();
                ViewData["Nguondon"] = donvi;
                ViewData["opt_nguondon"] = tl.Option_Nguonkiennghi_iparent(nguondon, (int)donvi.IPARENT);
                ViewData["id"] = fc["id"];
                return PartialView("../Ajax/Thietlap/Nguonkiennghi_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách nguồn kiến nghị");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Nguondon_del(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KNTC_NGUONDON donvi = _thietlap.GetBy_NguondonID(id);
                donvi.IDELETE = 1;
                donvi.IHIENTHI = 0;
                _thietlap.Update_Nguondon(donvi);

                Tracking(iUser, "Xóa nguồn đơn: " + donvi.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa danh sách nguồn đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Nguonkiennghi_del(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_NGUONDON donvi = _thietlap.GetBy_NguonkiennghiID(id);
                donvi.IDELETE = 1;
                donvi.IHIENTHI = 0;
                _thietlap.Update_Nguonkiennghi(donvi);

                Tracking(iUser, "Xóa nguồn kiến nghị: " + donvi.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Xóa danh sách nguồn kiến nghị");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Nguondon_insert(FormCollection fc)
        {
            //
            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                string cTen = func.RemoveTagInput(fc["cCode"]).ToUpper().Trim();
                if (_thietlap.Get_Nguondon().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_themnguondon"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    KNTC_NGUONDON donvi = new KNTC_NGUONDON();
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.IHIENTHI = 1;
                    donvi.IDELETE = 0;
                    donvi.IPARENT = Convert.ToInt32(fc["iParent"]);
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.IVITRI = 1;
                    donvi.ILOAI = Convert.ToInt32(fc["iLoai"]);
                    _thietlap.Insert_Nguondon(donvi);
                    Tracking(iUser, "Thêm mới nguồn đơn: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách nguồn đơn");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Nguonkiennghi_insert(FormCollection fc)
        {
            //
            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                string cTen = func.RemoveTagInput(fc["cCode"]).ToUpper().Trim();
                if (_thietlap.Get_Nguonkiennghi().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_themnguonkiennghi"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    KN_NGUONDON donvi = new KN_NGUONDON();
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.IHIENTHI = 1;
                    donvi.IDELETE = 0;
                    donvi.IPARENT = Convert.ToInt32(fc["iParent"]);
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.IVITRI = 1;
                    _thietlap.Insert_Nguonkiennghi(donvi);
                    Tracking(iUser, "Thêm mới nguồn kiến nghị: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách nguồn kiến nghị");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Nguondon_update(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Nguondon().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim()
                    && v.INGUONDON != id && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suanguondon", id))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    KNTC_NGUONDON donvi = _thietlap.GetBy_NguondonID(id);
                    if (donvi.INGUONDON > 0)
                    {
                        donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                        donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                        donvi.IPARENT = Convert.ToDecimal(fc["iParent"]);
                        donvi.ILOAI = Convert.ToDecimal(fc["iLoai"]);
                        _thietlap.Update_Nguondon(donvi);

                        Tracking(iUser, "Cập nhật lại nguồn đơn: " + donvi.CTEN);
                        Response.Write(1);
                    }
                    else
                    {
                        Response.Write(-1);
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách nguồn đơn");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Nguonkiennghi_update(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Nguonkiennghi().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim()
                    && v.INGUONDON != id && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suanguonkiennghi", id))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    KN_NGUONDON donvi = _thietlap.GetBy_NguonkiennghiID(id);
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.IPARENT = Convert.ToInt32(fc["iParent"]);
                    _thietlap.Update_Nguonkiennghi(donvi);

                    Tracking(iUser, "Cập nhật lại nguồn kiến nghị: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách nguồn kiến nghị");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Nguondon_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(30, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                string id_encr = fc["id"];
                int id = 0;

                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon();
                Response.Write(tl.List_Nguondon_parent_Search(nguondon, 0, 0, iUser, Request.Cookies["url_key"].Value, id));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách nguồn đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Nguonkiennghi_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(30, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                string id_encr = fc["id"];
                int id = 0;

                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                List<KN_NGUONDON> nguondon = _thietlap.Get_Nguonkiennghi();
                Response.Write(tl.List_Nguonkiennghi_parent_Search(nguondon, 0, 0, iUser, Request.Cookies["url_key"].Value, id));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách nguồn kiến nghị");
                //return null;
                throw;
            }
        }

        //Dia phuong
        public ActionResult Diaphuong(int page = 1)
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(31, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
               
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                string tukhoa = "";
                if (Request["ip_noidung"] != null)
                {
                    tukhoa = Request["ip_noidung"];
                }
                var thongtindiaphuong = thutuc.THIETLAPDIAPHUONG_PHANTRANG("PRC_DIAPHUONG_PHANTRANG", tukhoa, page, post_per_page).ToList();
                var thongtin = thutuc.THIETLAPDIAPHUONG("PRC_DIAPHUONG").ToList();
                if (thongtindiaphuong != null && thongtindiaphuong.Count() > 0)
                {
                    ViewData["list"] = tl.DIAPHUONGTHIETLAP(thongtindiaphuong);
                    ViewData["phantrang"] = "<tr><td colspan='5'>" + base_appcode.PhanTrang((int)thongtindiaphuong.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='5'  class='alert tcenter alert-info'>Không có kết quả tìm kiếm nào</td></tr>";
                }
               // ViewData["list"] = tl.List_Diaphuong(diaphuong, 0, 0, iUser);
                ViewData["Option_DiaPhuong"] = tl.Option_Diaphuong_PhanTrang(thongtin);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách địa phương");
                return View("../Home/Error_Exception");
            }
        }
       
        public ActionResult Ajax_Diaphuong_status(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(31, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
              
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                DIAPHUONG q = _thietlap.GetBy_DiaphuongID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUser, "Bỏ chọn áp dụng địa phương: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUser, "Chọn áp dụng địa phương: " + q.CTEN);
                }
                _thietlap.Update_Diaphuong(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Trạng thái danh sách địa phương");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Diaphuong_add()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(31, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_themdiaphuong");
                ViewData["opt-type"] = tl.Option_Type_Diaphuong();
                List<DIAPHUONG> diaphuong = _thietlap.Get_Diaphuong();
                ViewData["opt-linhvuc"] = tl.OptionDiaphuong_ThietLap(diaphuong);
                return PartialView("../Ajax/Thietlap/Diaphuong_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm  danh sách địa phương");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Diaphuong_edit(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(31, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suadiaphuong", id);
                //


                DIAPHUONG donvi = _thietlap.GetBy_DiaphuongID(id);
                ViewData["id"] = fc["id"];
                ViewData["linhvuc"] = donvi;
                ViewData["opt-type"] = tl.Option_Type_Diaphuong(donvi.CTYPE);
                List<DIAPHUONG> diaphuong = _thietlap.Get_Diaphuong();
                ViewData["opt-linhvuc"] = tl.OptionDiaphuong_ThietLap(diaphuong, 0, 0, (int)donvi.IPARENT, id);
                return PartialView("../Ajax/Thietlap/Diaphuong_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa  danh sách địa phương");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Diaphuong_del(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(31, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                DIAPHUONG donvi = _thietlap.GetBy_DiaphuongID(id);
                donvi.IHIENTHI = 0;
                donvi.IDELETE = 1;
                _thietlap.Update_Diaphuong(donvi);
                int iUserLogin = iUser;
                Tracking(iUserLogin, "Xóa địa phương: " + donvi.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Xóa  danh sách địa phương");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Diaphuong_insert(FormCollection fc)
        {
            //
            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Diaphuong().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0 ).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    string cCode = func.RemoveTagInput(fc["cCode"]);
                    if (cCode != "")
                    {
                        _condition = new Dictionary<string, object>();
                        _condition.Add("CCODE", cCode);
                        if (_thietlap.GetBy_List_Diaphuong(_condition).Count() > 0)
                        {
                            Response.Write(2);
                        }
                    }
                    if (!CheckTokenAction("thietlap_themdiaphuong"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }



                    DIAPHUONG donvi = new DIAPHUONG();
                    int iParent = Convert.ToInt32(fc["iParent"]);

                    donvi.IPARENT = iParent;
                    donvi.IHIENTHI = 1;
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.IDELETE = 0;
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.CTYPE = func.RemoveTagInput(fc["cType"]);
                    _thietlap.Insert_Diaphuong(donvi);
                    Tracking(iUser, "Thêm mới địa phương: " + donvi.CTEN);
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm  danh sách địa phương");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Diaphuong_update(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cTen"]);
                string cCode = func.RemoveTagInput(fc["cCode"]);
                var thongtinmadiaphuong = _thietlap.Get_List_Diaphuong_Sql(cCode, id).ToList();
                if (_thietlap.Get_Diaphuong().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0  && v.IDIAPHUONG != id ).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else if (thongtinmadiaphuong.Count() > 0)
                {
                    Response.Write(3);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suadiaphuong", id))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }
                    DIAPHUONG donvi = _thietlap.GetBy_DiaphuongID(id);
                    donvi.IPARENT = Convert.ToInt32(fc["iParent"]);
                    donvi.CCODE = func.RemoveTagInput(fc["cCode"]);
                    donvi.CTEN = func.RemoveTagInput(fc["cTen"]);
                    donvi.CTYPE = func.RemoveTagInput(fc["cType"]);
                    _thietlap.Update_Diaphuong(donvi);
                    Tracking(iUser, "Cập nhật lại địa phương: " + donvi.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa  danh sách địa phương");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Diaphuong_search(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(31, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                List<DIAPHUONG> diaphuong = _thietlap.Get_Diaphuong();
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                Response.Write(tl.List_Diaphuong_search(diaphuong, 0, 0, id_user(), Request.Cookies["url_key"].Value, id));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Tìm kiếm  danh sách địa phương");
                //return null;
                throw;
            }

        }

        public ActionResult Nghenghiep()
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                //....

                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(34, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;

                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                List<NGHENGHIEP> nghenghiep = _thietlap.Get_Nghenghiep();
                ViewData["list"] = tl.List_Nghenghiep(nghenghiep, iUser);
                ViewData["Option_nghenghiep"] = tl.Option_Nghenghiep_(iUser);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách nghề nghiệp");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult Ajax_Nghenghiep_add()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(34, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                SetTokenAction("thietlap_themnghenghiep");
                return PartialView("../Ajax/Thietlap/Nghenghiep_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách nghề nghiệp");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Nghenghiep_insert(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(34, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Nghenghiep().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim()  ).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {

                    //
                    if (!CheckTokenAction("thietlap_themnghenghiep"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    NGHENGHIEP nghe = new NGHENGHIEP();

                    nghe.CTEN = func.RemoveTagInput(fc["cTen"]);
                    nghe.IHIENTHI = 1;
                    nghe.IDELETE = 0;
                    nghe.CCODE = fc["cCode"];
                    nghe.IVITRI = 1;
                    _thietlap.Insert_Nghenghiep(nghe);
                    Tracking(id_user(), "Thêm mới nghề nghiệp: " + nghe.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách nghề nghiệp");
                //return null;
                throw;
            }


        }
        public ActionResult Ajax_Nghenghiep_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(34, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suanghenghiep", id);
                NGHENGHIEP donvi = _thietlap.GetBy_NghenghiepID(id);
                ViewData["nghe"] = donvi;
                ViewData["id"] = fc["id"];
                return PartialView("../Ajax/Thietlap/Nghenghiep_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách nghề nghiệp");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Nghenghiep_del(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(34, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
              
                NGHENGHIEP donvi = _thietlap.GetBy_NghenghiepID(id);
                donvi.IHIENTHI = 0;
                donvi.IDELETE = 1;
                _thietlap.Update_Nghenghiep(donvi);
                int iUserLogin = u_info.tk_action.iUser;
                Tracking(iUserLogin, "xóa nghề nghiệp: " + donvi.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Xóa danh sách nghề nghiệp");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Nghenghiep_update(FormCollection fc)
        {

            try
            {
                if (!CheckAuthToken()) { return null; }
                //....
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(34, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));           //
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Nghenghiep().Where(v => v.CCODE != null &&  v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.INGHENGHIEP != id && v.IDELETE == 0  ).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suanghenghiep", id))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }
                    NGHENGHIEP nghe = _thietlap.GetBy_NghenghiepID(id);
                    nghe.CTEN = func.RemoveTagInput(fc["cTen"]);
                     nghe.CCODE = fc["cCode"];
                    _thietlap.Update_Nghenghiep(nghe);
                    int iUserLogin = u_info.tk_action.iUser;
                    Tracking(iUserLogin, "Cập nhật lại nghề nghiệp: " + nghe.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách nghề nghiệp");
                //return null;
                throw;
            }

        }
        // áp dụng nghề 
        public ActionResult Ajax_Nghenghiep_status(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(34, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                NGHENGHIEP q = _thietlap.GetBy_NghenghiepID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(u_info.tk_action.iUser, "Bỏ chọn áp dụng nghề nghiệp: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(u_info.tk_action.iUser, "Chọn áp dụng nghề nghiệp: " + q.CTEN);
                }
                _thietlap.Update_Nghenghiep(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Trạng thái danh sách nghề nghiệp");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Nghenghiep_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(34, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                List<NGHENGHIEP> nghenghiep = _thietlap.Get_Nghenghiep();
                Response.Write(tl.List_Nghenghiep_search(nghenghiep, id_user(), id, Request.Cookies["url_key"].Value));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Trạng thái danh sách nghề nghiệp");
                //return null;
                throw;
            }
        }
        // Quốc tịch
        public ActionResult Quoctich()
        {
            try
            {
                //....

                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(35, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
              
                TaikhoanAtion act = GetUserInfor().tk_action;
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                List<QUOCTICH> quoctich = _thietlap.Get_Quoctich();
                ViewData["list"] = tl.List_Quoctich(quoctich, iUser);
                ViewData["Option_quoctich"] = tl.Option_Quoctich_(iUser);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, " danh sách quốc tịch");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Quoctich_status(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(35, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                QUOCTICH q = _thietlap.GetBy_QuoctichID(id);
              
                int iUser = u_info.tk_action.iUser;
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUser, "Bỏ chọn áp dụng quốc tịch: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUser, "Chọn áp dụng Quốc tịch: " + q.CTEN);
                }
                _thietlap.Update_Quoctich(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái danh sách quốc tịch");
                //return null;
                throw;
            }
        }
        // add quốc tịch
        public ActionResult Ajax_Quoctich_add()
        {
            try
            {
                //....

                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(35, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_themquoctich");
                return PartialView("../Ajax/Thietlap/Quoctich_add");
            }
            catch (Exception e)
            {
                //Handle Exception;
                return View("Error");
            }
        }
        public ActionResult Ajax_Quoctich_del(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(35, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                QUOCTICH donvi = _thietlap.GetBy_QuoctichID(id);
                donvi.IHIENTHI = 0;
                donvi.IDELETE = 1;
                _thietlap.Update_Quoctich(donvi);
                int iUserLogin = u_info.tk_action.iUser;
                Tracking(iUserLogin, "Xóa Quốc tịch: " + donvi.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách quốc tịch");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Quoctich_insert(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(35, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Quoctich().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_themquoctich"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }


                    QUOCTICH quoctich = new QUOCTICH();
                    quoctich.CTEN = func.RemoveTagInput(fc["cTen"]);
                    quoctich.CCODE = fc["cCode"];
                    quoctich.IVITRI = 1;
                    quoctich.IHIENTHI = 1;
                    quoctich.IDELETE = 0;
                    _thietlap.Insert_Quoctich(quoctich);
                    Tracking(iUser, "Thêm mới quốc tịch: " + quoctich.CTEN);
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách quốc tịch");
                //return null;
                throw;
            }
        }
        // edit quốc tịch
        public ActionResult Ajax_Quoctich_edit(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(35, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suaquoctich", id);
                QUOCTICH quoctich = _thietlap.GetBy_QuoctichID(id);
                ViewData["quoctich"] = quoctich;
                ViewData["id"] = fc["id"];
                return PartialView("../Ajax/Thietlap/Quoctich_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Thêm danh sách quốc tịch");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Quoctich_update(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(35, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Quoctich().Where(v => v.CCODE != null && v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IQUOCTICH != id && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suaquoctich", id))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    QUOCTICH quoctich = _thietlap.GetBy_QuoctichID(id);
                    quoctich.CTEN = func.RemoveTagInput(fc["cTen"]);
                    quoctich.CCODE = fc["cCode"];
                    _thietlap.Update_Quoctich(quoctich);
                    Tracking(iUser, "Cập nhật lại quốc tịch: " + quoctich.CTEN);
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách quốc tịch");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Quoctich_search(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(35, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                List<QUOCTICH> quoctich = _thietlap.Get_Quoctich();
                Response.Write(tl.List_Quoctich_search(quoctich, id_user(), id, Request.Cookies["url_key"].Value));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách quốc tịch");
                //return null;
                throw;
            }
        }
        // Phần dân tộc
        public ActionResult Dantoc()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                
                if (!base_buss.Action_(36, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                List<DANTOC> dantoc = _thietlap.Get_Dantoc();
                ViewData["list"] = tl.List_Dantoc(dantoc, iUser);
                ViewData["Option_dantoc"] = tl.Option_Dantoc_(iUser);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách dân tộc");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Dantoc_status(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(36, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
               
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                DANTOC q = _thietlap.GetBy_DantocID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUser, "Bỏ chọn áp dụng Dân tộc: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUser, "Chọn áp dụng Dân tộc: " + q.CTEN);
                }
                _thietlap.Update_Dantoc(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái danh sách dân tộc");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Dantoc_del(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(36, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
      
                DANTOC donvi = _thietlap.GetBy_DantocID(id);
                donvi.IHIENTHI = 0;
                donvi.IDELETE = 1;
                _thietlap.Update_Dantoc(donvi);
                int iUserLogin = u_info.tk_action.iUser;
                Tracking(iUserLogin, "Xóa dân tộc: " + donvi.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Xóa danh sách dân tộc");
                //return null;
                throw;
            }
        }
        // add quốc tịch
        public ActionResult Ajax_Dantoc_add()
        {
            try
            {
                //....
                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(36, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_dantoc");
                return PartialView("../Ajax/Thietlap/Dantoc_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách dân tộc");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Dantoc_insert(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(36, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                string cTen = func.RemoveTagInput(fc["cCode"]).ToUpper().Trim();
                if (_thietlap.Get_Dantoc().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_dantoc"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }
                    DANTOC dantoc = new DANTOC();
                    dantoc.CTEN = func.RemoveTagInput(fc["cTen"]);
                    dantoc.IHIENTHI = 1;
                    dantoc.IDELETE = 0;
                    dantoc.CCODE = fc["cCode"];
                    dantoc.IVITRI = 1;
                    _thietlap.Insert_Dantoc(dantoc);
                    Tracking(iUser, "Thêm mới dân tộc: " + dantoc.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách dân tộc");
                //return null;
                throw;
            }
        }
        // edit quốc tịch
        public ActionResult Ajax_Dantoc_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(36, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suadantoc", id);
                DANTOC dantoc = _thietlap.GetBy_DantocID(id);
                ViewData["dantoc"] = dantoc;
                ViewData["id"] = fc["id"];
                return PartialView("../Ajax/Thietlap/Dantoc_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách dân tộc");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Dantoc_update(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Dantoc().Where(v => v.CCODE != null  && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDANTOC != id).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suadantoc", id))
                    {
                        // Response.Redirect("/Home/Error");
                        return null;
                    }

                    DANTOC dantoc = _thietlap.GetBy_DantocID(id);
                    dantoc.CTEN = func.RemoveTagInput(fc["cTen"]);
                    dantoc.CCODE = fc["cCode"];
                    _thietlap.Update_Dantoc(dantoc);
                    Tracking(iUser, "Cập nhật lại dân tộc: " + dantoc.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách dân tộc");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Dantoc_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(36, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                List<DANTOC> dantoc = _thietlap.Get_Dantoc();
                Response.Write(tl.List_Dantoc_search(dantoc, id_user(), id, Request.Cookies["url_key"].Value));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Tìm kiếm danh sách dân tộc");
                //return null;
                throw;
            }
        }

        public ActionResult Loaivanban()
        {
            try
            {

                //....
                if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(37, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;

                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                List<VB_LOAI> loaivb = _thietlap.Get_Loaivanban();
                ViewData["list"] = tl.List_Vanban(loaivb, iUser);
                ViewData["Option_vanban"] = tl.Option_Vanban(iUser);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "  danh sách loại văn bản");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Vanban_add()
        {
            try
            {
                //....

                if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(37, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_themvanban");
                return PartialView("../Ajax/Thietlap/Vanban_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách loại văn bản");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Vanban_insert(FormCollection fc)
        {
            try
            {
                //....

                //
                if (!CheckAuthToken()) { return null; }
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Loaivanban().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_themvanban"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }


                    VB_LOAI vbloai = new VB_LOAI();

                    vbloai.CTEN = func.RemoveTagInput(fc["cTen"]);
                    vbloai.IHIENTHI = 1;
                    vbloai.IDELETE = 0;
                    vbloai.CCODE = func.RemoveTagInput(fc["cCode"]);
                    vbloai.IVITRI = 1;
                    _thietlap.Insert_Loaivanban(vbloai);
                    Tracking(id_user(), "Thêm mới văn bản loại: " + vbloai.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách loại văn bản");
                //return null;
                throw;
            }
        }
        // edit 
        public ActionResult Ajax_Vanban_edit(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(37, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suavanban", id);
                VB_LOAI vbloai = _thietlap.GetBy_LoaivanbanID(id);
                ViewData["vb_loai"] = vbloai;
                ViewData["id"] = fc["id"];
                return PartialView("../Ajax/Thietlap/Vanban_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách loại văn bản");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Vanban_del(FormCollection fc)
        {
            try
            {
                //....

                if (!CheckAuthToken_Api()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                VB_LOAI donvi = _thietlap.GetBy_LoaivanbanID(id);
                donvi.IHIENTHI = 0;
                donvi.IDELETE = 1;
                _thietlap.Update_Loaivanban(donvi);
                int iUserLogin = u_info.tk_action.iUser;
                Tracking(iUserLogin, "Xóa loại văn bản: " + donvi.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Xóa danh sách loại văn bản");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Vanban_update(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();

                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //
               
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_Loaivanban().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.ILOAI != id && v.IDELETE == 0).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suavanban", id))
                    {
                        // Response.Redirect("/Home/Error");
                        return null;
                    }

                    VB_LOAI vbloai = _thietlap.GetBy_LoaivanbanID(id);
                    vbloai.CTEN = func.RemoveTagInput(fc["cTen"]);
                    vbloai.CCODE = func.RemoveTagInput(fc["cCode"]);
                    _thietlap.Update_Loaivanban(vbloai);

                    Tracking(u_info.tk_action.iUser, "Cập nhật lại văn bản loại: " + vbloai.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách loại văn bản");
                //return null;
                throw;
            }

        }
        // áp dụng 
        [ValidateInput(false)]
        public ActionResult Ajax_Vanban_status(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(37, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                VB_LOAI q = _thietlap.GetBy_LoaivanbanID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(u_info.tk_action.iUser, "Bỏ chọn áp dụng văn bản loại: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(u_info.tk_action.iUser, "Chọn áp dụng văn bản loại: " + q.CTEN);
                }
                _thietlap.Update_Loaivanban(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Trạng thái danh sách loại văn bản");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Vanban_search(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(37, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(HashUtil.Decode_ID(id_encr));
                }
                Response.Write(tl.List_Vanban_search(id_user(), id, Request.Cookies["url_key"].Value));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Tìm kiếm danh sách loại văn bản");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Taikhoan_Timkiem(FormCollection fc)
        {
            try
            {
                if (!CheckAuthToken()) { return null; }
                //....
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int iDonVi = 0;
                List<USERS> user = _thietlap.Get_Taikhoan();
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                List<USER_PHONGBAN> phongban = _thietlap.Get_List_Phongban();
                List<USER_GROUP> group = _thietlap.Get_All_Usergroup();
                string str = "";
                string ctentimkiem = func.RemoveTagInput(fc["ip_noidung"].ToUpper().Trim());
            
                if(ctentimkiem == "")
                {
                    var donvi = thutuc.GetNguoiDung_OPT("PKG_THIETLAP_HETHONG.PRC_NGUOIDUNG_OPT", "", 1, 20).ToList();
                    if (donvi != null && donvi.Count() > 0)
                    {
                        str= tl.TAIKHOAN_LIST(donvi) +"<tr><td colspan='6'>" + base_appcode.PhanTrang((int)donvi.FirstOrDefault().TOTAL, 20, 1, RemovePageFromUrl()) + Option_Post_Per_Page(20) + "</td></tr>";
                      
                    }
                }
                else
                {
                    var thongtintimkiem = _thietlap.TimKiemTaiKhoan(ctentimkiem).ToList();
                    int dem = 1;
                    foreach (var x in thongtintimkiem)
                    {

                        str += tl.List_DonVi_TaiKhoan_search(phongban, coquan, user, group, iDonVi, iUser, (int)x.IUSER);
                        //  str += tiepdan.Tiepdan_dinhky_search((int)x.IDINHKY, id_user(), dem);
                        dem++;
                    }
                   
                }
                string table = "<table class='table table-bordered table-condensed'>" +
                            "<thead> <tr> " +
                            "<th nowrap width='15%'>Tên đăng nhập/ người dùng</th>" +
                            " <th nowrap>Phòng ban (chức vụ)</th>" +
                            "<th nowrap>Thông tin tài khoản</th>" +
                            " <th nowrap class='width='35%'>Nhóm tài khoản</th>" +
                                "  <th nowrap class='tcenter' width='5%'>Kích hoạt</th>" +
                            " <th nowrap class='tcenter' width='5%'>Chức năng</th></tr>" +
                                "</thead>" +
                                "<tbody >" +
                                "" + str + "" +
                                "</tbody>" +
                           " </table>";
                Response.Write(table);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Tìm kiếm danh sách tài khoản");
                //return null;
                throw;
            }
        }

        public ActionResult Taikhoan_Lichsu(string id)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id_user = Convert.ToInt32(HashUtil.Decode_ID(id, Request.Cookies["url_key"].Value));
                ViewData["list"] = tl.TK_Lichsu(id_user);
                var taikhoan = _thietlap.Get_User(id_user);
                ViewData["tendonvi"] = _thietlap.GetBy_Quochoi_CoquanID(id_user).CTEN;
                ViewData["tennguoidung"] = taikhoan.CTEN;
                ViewData["user"] = taikhoan.CUSERNAME;
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Tìm kiếm lịch sử sử dụng");
                return View("../Home/Error_Exception");
            }
           
        }
        public ActionResult Timkiemlichsu(int page= 1)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                ViewData["opt-coquantiepdan"] =  tl.OptionCoQuan_TreeList();
                string dTuNgay = "";
                string dDenNgay =  "";
                if (Request["dTuNgay"] != null && Request["dTuNgay"].ToString() != "")
                {
                    ViewData["TuNgay"] = (Request["dTuNgay"]);
                   
                    dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dTuNgay"]));

                }
                else
                {
                    ViewData["TuNgay"] =DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");
                    dTuNgay = func.ConvertDateToSql(DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy"));
                }
                if (Request["dDenNgay"] != null && Request["dDenNgay"].ToString() != "")
                {
                    ViewData["DenNgay"] = (Request["dDenNgay"]);
                    dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(Request["dDenNgay"]));

                }
                else
                {
                    ViewData["DenNgay"] = DateTime.Now.ToString("dd/MM/yyyy");
                    dDenNgay = func.ConvertDateToSql(DateTime.Now.ToString("dd/MM/yyyy"));
                }
                string tennguoidung="";
                if (Request["ten"] != null && Request["ten"].ToString() != "")
                {
                    tennguoidung = Request["ten"];
                }
                string noidung="";
                if (Request["tukhoa"] != null && Request["tukhoa"].ToString() != "")
                {
                    noidung = Request["tukhoa"];
                }
                int imadonvi = -1;
                if (Request["iDonVi"] != null && Request["iDonVi"].ToString() != "")
                {
                    imadonvi = Convert.ToInt32(Request["iDonVi"]);
                }
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    post_per_page = Convert.ToInt32(Request["post_per_page"]);
                }
                var track = thutuc.LICHSU_PHANTRANG("PKG_THIETLAP_HETHONG.PKG_LICHSU", dTuNgay, dDenNgay, noidung,imadonvi,tennguoidung, page, post_per_page).OrderByDescending(x=>x.THOIGIAN).ToList();
                if (track != null && track.Count() > 0)
                {
                    ViewData["list"] = tl.PhanTrangTracking(track);
                    ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)track.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) + Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    ViewData["list"] = "<tr><td colspan='4'  class='alert tcenter alert-info' >Không có kết quả tìm kiếm nào</td></tr>";
                }
              
               
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Tìm kiếm lịch sử sử dụng");
                return View("../Home/Error_Exception");
            }
           
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Tracuu_Tracking(FormCollection fc)
        {

            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                ViewData["opt-coquantiepdan"] = tl.OptionCoQuan_TreeList();
              
                string data = ""; 
                //   string key = fc["tukhoa"];
                string dTuNgay = "";
                string dDenNgay = "";
                if (fc["dTuNgay"] != null || fc["dTuNgay"].ToString() != "")
                {
                    dTuNgay = func.ConvertDateToSql(func.RemoveTagInput(fc["dTuNgay"]));

                }
                if (fc["dDenNgay"] != null || fc["dDenNgay"].ToString() != "")
                {
                    dDenNgay = func.ConvertDateToSql(func.RemoveTagInput(fc["dDenNgay"]));

                }
                int idonvi = Convert.ToInt32(func.RemoveTagInput(fc["iDonVi"]));
                string ten = func.RemoveTagInput(fc["ten"]);
                string key = func.RemoveTagInput(fc["tukhoa"]);
                string str = "";
                var tracking = thutuc.GET_LISTLICHSUTIMKIEM("PKG_THIETLAP_HETHONG.PKG_TIMKIEMLICHSU", key,dTuNgay, dDenNgay).ToList();
              
                var ds_taikhoan = thutuc.GET_LISTLICHSUTIMKIEMUSER("PKG_THIETLAP_HETHONG.PKG_TIMKIEMTHEODONVI", ten, idonvi).ToList();
             
                str = tl.TimKiemTracking(tracking, ds_taikhoan);
               
                    Response.Write(str);
                
               
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Tìm kiếm lịch sử sử dụng");
                throw;
            }
           


        }

        //start import danh mục cơ quan
        public ActionResult Ajax_Coquan_import()
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                SetTokenAction("Import_add");
                return PartialView("../Ajax/Thietlap/Coquan_import");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " import cơ quan vào hệ thống");
                return View("../Home/Error_Exception");
            }
           
        }
        [HttpPost]
        public ActionResult Coquan_Import(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { return null; }
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
                InsertCoquan_AfterImport(file_name);
                Response.Redirect(Request.Cookies["url_return"].Value);
            }
            catch(Exception e)
            {
                log.Log_Error(e, " Lỗi thêm cơ quan ");
                //return null;
                throw;
            }
            return null;
        }
        public string UploadFile(HttpPostedFileBase file)
        {
            Random random = new Random(); int rand = random.Next(0, 99999);

            string file_name = "";
            string dir_path_upload = AppConfig.dir_path_upload;
            string path_upload = "/Thietlap/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
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
        public Boolean InsertCoquan_AfterImport(string file_path)
        {
            bool result = true;
            try
            {
                UserInfor u_info = GetUserInfor();
                string path = Server.MapPath(file_path);
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(path);
                Worksheet sheet = workbook.Worksheets[0];
                DataTable db = sheet.ExportDataTable();
                //string str = "";
                if (db.Rows.Count > 0)
                {
                    int iParent = 0;
                    //str += "<table>";
                    for (int i = 0; i < db.Rows.Count; i++)
                    {
                        //str += "<tr>";
                        //str += "<td>" + db.Rows[i][0].ToString() + "</td>";
                        //str += "<td>" + db.Rows[i][1].ToString() + "</td>";
                        //str += "<td>" + db.Rows[i][2].ToString() + "</td>";
                        
                        //str += "</tr>";
                        if (db.Rows[i][1] != null && db.Rows[i][1].ToString() != "")
                        {
                            iParent = 0;
                            QUOCHOI_COQUAN q = new QUOCHOI_COQUAN();
                            q.IPARENT = 0;
                            q.IHIENTHI = 0;
                            q.IDELETE = 0;
                            q.IDIAPHUONG = 0; q.IGROUP = 0; q.IMACDINH = 0; q.IUSE = 0; q.IVITRI = 1;
                            q.CTEN = func.RemoveTagInput(db.Rows[i][1].ToString());
                            q.CCODE = func.RemoveTagInput(db.Rows[i][0].ToString());
                            _thietlap.Insert_Quochoi_Coquan(q);
                            iParent = (int)q.ICOQUAN;
                        }
                        else
                        {
                            if (db.Rows[i][2] != null && db.Rows[i][2].ToString() != "")
                            {
                                QUOCHOI_COQUAN q = new QUOCHOI_COQUAN();
                                q.IPARENT = iParent;
                                q.IHIENTHI = 0;
                                q.IDELETE = 0;
                                q.IDIAPHUONG = 0; q.IGROUP = 0; q.IMACDINH = 0; q.IUSE = 0; q.IVITRI = 1;
                                q.CTEN = func.RemoveTagInput(db.Rows[i][2].ToString());
                                q.CCODE = func.RemoveTagInput(db.Rows[i][0].ToString());
                                _thietlap.Insert_Quochoi_Coquan(q);
                            }

                        }

                    }
                    
                }
                //Response.Write(str);
            }
            catch
            {
                result = false;
            }

            return result;
        }


        //end import danh mục cơ quan




        // bổ sung chức vụ 
        public ActionResult Chucvu()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                List<USER_CHUCVU> chucvu = _thietlap.Get_List_User_Chucvu();
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                ViewData["list"] = tl.List_Chucvu(chucvu, iUser);
                ViewData["Option_chucvu"] = tl.Option_Chucvu(chucvu,iUser);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách chức vụ");
                return View("../Home/Error_Exception");
            }
        }
      
        public ActionResult Ajax_Chucvu_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

               
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                var chucvu = _thietlap.Get_Chucvu(id);
                chucvu.IDELETE = 1;
                _thietlap.Update_Userchucvu(chucvu);
                int iUserLogin = u_info.tk_action.iUser;
                Tracking(iUserLogin, "Xóa chức vụ: " + chucvu.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Xóa danh sách chức vụ");
                //return null;
                throw;
            }
        }
        // add quốc tịch
        public ActionResult Ajax_Chucvu_add()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
             
                SetTokenAction("thietlap_chucvu");
                ViewData["opt_phongban"] = tl.Option_DonVi_PhongBanTree();
                return PartialView("../Ajax/Thietlap/Chucvu_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách chức vụ");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Chucvu_insert(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
               
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                
                string cTen = func.RemoveTagInput(fc["cCode"]).ToUpper().Trim();
                if (_thietlap.Get_List_User_Chucvu().Where(v => v.CCODE != null && v.IDELETE == 0 && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim()).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_chucvu"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }
                    USER_CHUCVU chucvu = new USER_CHUCVU();
                    chucvu.CTEN = func.RemoveTagInput(fc["cTen"]).Trim();
                    chucvu.IDELETE = 0;
                    chucvu.IVITRI = 1;
                    chucvu.CCODE = fc["cCode"];
                    chucvu.IPHONGBAN = Convert.ToDecimal(fc["iPhongBan"]);
                    _thietlap.Insert_chucvu(chucvu);
                    Tracking(iUser, "Thêm mới chức vụ: " + chucvu.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách chức vụ");
                //return null;
                throw;
            }
        }
        // edit quốc tịch
        public ActionResult Ajax_Chucvu_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
            

                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suachucvu", id);
                USER_CHUCVU chucvu = _thietlap.Get_Chucvu(id);
                ViewData["chucvu"] = chucvu;
                ViewData["id"] = fc["id"];
                ViewData["opt_phongban"] = tl.Option_DonVi_PhongBanTree(chucvu.IPHONGBAN);
                return PartialView("../Ajax/Thietlap/Chucvu_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách chức vụ");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Chucvu_update(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
               
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cCode"]);
                if (_thietlap.Get_List_User_Chucvu().Where(v => v.CCODE != null && v.ICHUCVU != id && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim()).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suachucvu", id))
                    {
                        // Response.Redirect("/Home/Error");
                        return null;
                    }

                    USER_CHUCVU chucvu = _thietlap.Get_Chucvu(id);
                    chucvu.CTEN = func.RemoveTagInput(fc["cTen"]).Trim();
                    chucvu.CCODE = fc["cCode"];
                    chucvu.IPHONGBAN = Convert.ToDecimal(fc["iPhongBan"]);
                    _thietlap.Update_Userchucvu(chucvu);
                    Tracking(iUser, "Cập nhật lại chức vụ: " + chucvu.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách chức vụ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Chucvu_search(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(25, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(id_encr);
                }
                List<USER_CHUCVU> chucvu = _thietlap.Get_List_User_Chucvu();
                Response.Write(tl.List_Chucvu_search(chucvu, id_user(), Request.Cookies["url_key"].Value,id));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Tìm kiếm danh sách chức vụ");
                //return null;
                throw;
            }
        }

        //   lĩnh vực cơ quan
        public ActionResult Linhvuc_Coquan()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                List<LINHVUC_COQUAN> linhvuc_coquan = _thietlap.Get_Linhvuc_Coquan_Sorted();
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                ViewData["Option_linhvuc_coquan"] = tl.Option_linhvuccoquan(linhvuc_coquan, coquan, iUser);
             //   func.SetCookies("url_return", Request.Url.AbsoluteUri);
                ViewData["list"] = tl.List_linhvuccoquan(linhvuc_coquan, coquan, iUser);
              
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách lĩnh vực cơ quan");
                return View("../Home/Error_Exception");
            }
        }

        public ActionResult Ajax_Linhvuc_Coquan_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
              
                var linhvuccoquan = _thietlap.GetBy_Linhvuc_CoquanID(id);
                var listChild = _thietlap.GetById_Linhvuc_Child(id);
                //Xoa het ca cac linh vuc con
                foreach(var item in listChild)
                {
                    item.IDELETE = 1;
                    item.IHIENTHI = 0;
                    _thietlap.Update_Linhvuc_Coquan(item);
                }
                linhvuccoquan.IDELETE = 1;
                linhvuccoquan.IHIENTHI = 0;
                _thietlap.Update_Linhvuc_Coquan(linhvuccoquan);
                int iUserLogin = u_info.tk_action.iUser;
                Tracking(iUserLogin, "Xóa lĩnh vực cơ quan: " + linhvuccoquan.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Xóa danh sách lĩnh vực cơ quan");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_Linhvuc_Coquan_add()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
                UserInfor u_info = GetUserInfor();
                List<LINHVUC_COQUAN> linhvuc_coquan = _thietlap.Get_Linhvuc_Coquan_Sorted();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_linhvucoquan");
                //ViewData["opt-donvi"] = tl.OptionCoQuan_phongban();
                ViewData["opt-donvi"] = tl.OptionCoQuan_TreeList();
                ViewData["opt-linhvuccha"] = tl.OptionLinhVucCha_TreeList();
                ViewData["listLinhVucCoQuan"] = JsonConvert.SerializeObject(linhvuc_coquan);
                return PartialView("../Ajax/Thietlap/Linhvuc_Coquan_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách lĩnh vực cơ quan");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Linhvuc_Coquan_insert(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                string cTen = func.RemoveTagInput(fc["cCode"]).ToUpper().Trim();
                decimal icoquan = Convert.ToDecimal(fc["icoquan"]);
                if (_thietlap.Get_Linhvuc_Coquan().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0 && v.ICOQUAN != icoquan).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_linhvucoquan"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }
                    LINHVUC_COQUAN linhvuccoquan = new LINHVUC_COQUAN();
                    linhvuccoquan.CTEN = func.RemoveTagInput(fc["cTen"]).Trim();
                    linhvuccoquan.IDELETE = 0;
                    linhvuccoquan.IHIENTHI = 1;
                    linhvuccoquan.CCODE = func.RemoveTagInput(fc["cCode"]).Trim();
                    linhvuccoquan.ICOQUAN = Convert.ToDecimal(fc["icoquan"]);
                    linhvuccoquan.IPARENT = Convert.ToDecimal(fc["iparent"]);
                    _thietlap.Insert_Linhvuc_Coquan(linhvuccoquan);
                    Tracking(iUser, "Thêm mới lĩnh vực cơ quan: " + linhvuccoquan.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách chức vụ");
                //return null;
                throw;
            }
        }
    
        public ActionResult Ajax_Linhvuc_Coquan_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_sualinhvuccoquan", id);
                LINHVUC_COQUAN linhvuccoquan = _thietlap.GetBy_Linhvuc_CoquanID(id);
                List<LINHVUC_COQUAN> listLinhvuc_coquan = _thietlap.Get_Linhvuc_Coquan_Sorted().Where(x => x.IDELETE == 0).ToList();
                ViewData["linhvuccoquan"] = linhvuccoquan;
                ViewData["listLinhVucCoQuan"] = JsonConvert.SerializeObject(listLinhvuc_coquan);
                ViewData["ten"] = Server.HtmlDecode(linhvuccoquan.CTEN);
                ViewData["code"] = Server.HtmlDecode(linhvuccoquan.CCODE);
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                ViewData["opt-donvi"] = tl.OptionCoQuan_TreeList(Decimal.ToInt32(linhvuccoquan.ICOQUAN));
                // Lựa chọn mặc định là lĩnh vực cha hiện tại
                string select =  "";
                String optLinhVucCha = "";
                optLinhVucCha += "<option value ='0'> - - - Gốc</option>";
                if (linhvuccoquan.IPARENT != 0)
                {
                    LINHVUC_COQUAN linhVucCha = _thietlap.GetBy_Linhvuc_CoquanID((int)linhvuccoquan.IPARENT);
                    select = "selected";
                    optLinhVucCha += "<option " + select + " value ='" + linhVucCha.ILINHVUC + "'>" + HttpUtility.HtmlEncode(linhVucCha.CTEN) + "</option>";
                }
                List<LINHVUC_COQUAN> childList = _thietlap.GetById_Linhvuc_Child((int)linhvuccoquan.ILINHVUC);
                var listIdChildList = childList.Select(x => x.ILINHVUC);
                foreach (var x in listLinhvuc_coquan)
                {
                    if(x.ICOQUAN == linhvuccoquan.ICOQUAN && x.ILINHVUC != linhvuccoquan.ILINHVUC && x.ILINHVUC != linhvuccoquan.IPARENT && !listIdChildList.Contains(x.ILINHVUC))
                        optLinhVucCha += "<option value ='" + x.ILINHVUC + "'>" + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
                }
                ViewData["opt-linhvuccha"] = optLinhVucCha;

                ViewData["id"] = fc["id"];
                return PartialView("../Ajax/Thietlap/Linhvuc_Coquan_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách lĩnh vực cơ quan");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_LoadKyHopTheoDaiBieu(int iLoaiDaiBieu)
        {

            if (!CheckAuthToken()) { return null; }
            try
            {
                    string str = Get_Option_Khoa_By_Loai(iLoaiDaiBieu);

                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Load Ky Hop ");
                //Handle Exception;
                return null;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Linhvuc_Coquan_update(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cCode"]).ToUpper().Trim();
                decimal icoquan = Convert.ToDecimal(fc["icoquan"]);
                if (_thietlap.Get_Linhvuc_Coquan().Where(v => v.CCODE != null && v.CCODE.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0 && v.ILINHVUC != id && v.ICOQUAN != icoquan).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_sualinhvuccoquan",id))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    var linhvuccoquan = _thietlap.GetBy_Linhvuc_CoquanID(id);
                    linhvuccoquan.CTEN = func.RemoveTagInput(fc["cTen"]).Trim();
                    linhvuccoquan.ICOQUAN = Convert.ToDecimal(fc["icoquan"]);
                    linhvuccoquan.CCODE = func.RemoveTagInput(fc["cCode"]).Trim();
                    linhvuccoquan.IPARENT = Convert.ToDecimal(fc["iparent"]);
                    _thietlap.Update_Linhvuc_Coquan(linhvuccoquan);
                    Tracking(iUser, "Sửa lĩnh vực cơ quan: " + linhvuccoquan.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa danh sách chức vụ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Linhvuc_Coquan_search(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(26, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(id_encr);
                }
               
                int iUser = u_info.tk_action.iUser;
                List<LINHVUC_COQUAN> linhvuc_coquan = _thietlap.Get_Linhvuc_Coquan();
                List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                //ViewData["list"] = tl.List_linhvuccoquan(linhvuc_coquan, coquan, iUser);
                Response.Write(tl.List_linhvuccoquan_search(linhvuc_coquan, coquan, iUser, id));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách lĩnh vực chức vụ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Linhvuc_Coquan_status(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                //....

               
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                LINHVUC_COQUAN q = _thietlap.GetBy_Linhvuc_CoquanID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUser, "Bỏ chọn áp dụng lĩnh vực cơ quan: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUser, "Chọn áp dụng lĩnh vực cơ quan: " + q.CTEN);
                }
                _thietlap.Update_Linhvuc_Coquan(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái danh sách lĩnh vực cơ quan");
                //return null;
                throw;
            }
        }
        // Bổ sug 
        public ActionResult Ajax_Loaidon_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                KNTC_LOAIDON loaidon = _thietlap.GetBy_LoaidonID(id);
                loaidon.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Loaidon(loaidon);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí loại đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Nguondon_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                KNTC_NGUONDON loaidon = _thietlap.GetBy_NguondonID(id);
                loaidon.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Nguondon(loaidon);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí loại đơn");
                //return null;
                throw;
            }

        }
        //   tra loi phan loai 
        public ActionResult Traloiphanloai()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Login"); return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(52, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                TaikhoanAtion act = GetUserInfor().tk_action;
                List<KN_TRALOI_PHANLOAI> Traloiphanloai = _thietlap.Get_TraLoi_PhanLoai();
                ViewData["list"] = tl.List_Traloiphanloai(Traloiphanloai, 0,0, iUser);
                ViewData["opt-donvi"] = tl.Option_Traloiphanloai(Traloiphanloai, 0,0, iUser);
                return View();
            }
            catch (Exception e)
            {
                log.Log_Error(e, "danh sách kiên nghị trả lời phân loại");
                return View("../Home/Error_Exception");
            }
        }

        public ActionResult Ajax_Traloi_Phanloai_del(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                //....


                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(52, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                var traloiphanloai = _thietlap.GetBy_TraLoi_PhanLoaiID(id);
                traloiphanloai.IDELETE = 1;
                traloiphanloai.IHIENTHI = 0;
                _thietlap.Update_TraLoi_PhanLoai(traloiphanloai);
                int iUserLogin = u_info.tk_action.iUser;
                Tracking(iUserLogin, "Xóa trả lời phân loại: " + traloiphanloai.CTEN);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Xóa trả lời phân loại");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_Traloi_Phanloai_add()
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                //....
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(52, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("thietlap_traloiphanloai");
                List<KN_TRALOI_PHANLOAI> traloi = _thietlap.Get_TraLoi_PhanLoai();
                
                int iUser = u_info.tk_action.iUser;
                ViewData["opt-donvi"] = tl.Option_Traloiphanloai_parent(traloi, 0,0, iUser);
             
                return PartialView("../Ajax/Thietlap/Traloi_Phanloai_add");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm danh sách kiện nghị trả lời phân loại");
                //return null;
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Traloi_Phanloai_insert(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                string cTen = func.RemoveTagInput(fc["cTen"]).ToUpper().Trim();
                decimal iparent = Convert.ToDecimal(fc["iparent"]);
                string cCode = func.RemoveTagInput(fc["cCode"]).Trim();
                if (_thietlap.Get_TraLoi_PhanLoai().Where(v => v.CCODE != null &&  v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0 && v.CCODE != cCode).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_traloiphanloai"))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }
                    KN_TRALOI_PHANLOAI traloiphanloai = new KN_TRALOI_PHANLOAI();
                    traloiphanloai.CTEN = func.RemoveTagInput(fc["cTen"]).Trim();
                    traloiphanloai.IDELETE = 0;
                    traloiphanloai.IHIENTHI = 1;
                    traloiphanloai.CCODE = func.RemoveTagInput(fc["cCode"]).Trim();
                    traloiphanloai.IPARENT = Convert.ToDecimal(fc["iparent"]);
                    _thietlap.Insert_TraLoi_PhanLoai(traloiphanloai);
                    Tracking(iUser, "Thêm mới kiến nghị trả lời phân loại: " + traloiphanloai.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Thêm mới kiến nghị trả lời phân loại");
                //return null;
                throw;
            }
        }

        public ActionResult Ajax_Traloi_Phanloai_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(52, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("thietlap_suatraloiphanloai", id);
                KN_TRALOI_PHANLOAI traloiphanloai = _thietlap.GetBy_TraLoi_PhanLoaiID(id);
           
                int iUser = u_info.tk_action.iUser;
                ViewData["traloiphanloai"] = traloiphanloai;
                ViewData["ten"] = Server.HtmlDecode(traloiphanloai.CTEN);
                if (traloiphanloai.CCODE != null)
                {
                    ViewData["code"] = Server.HtmlDecode(traloiphanloai.CCODE);
                } else
                {
                    ViewData["code"] = "";
                }
                List<KN_TRALOI_PHANLOAI> Traloiphanloai = _thietlap.Get_TraLoi_PhanLoai();
                ViewData["opt-donvi"] = tl.Option_Traloiphanloai_parent_edit(Traloiphanloai, 0, 0, iUser, (int)traloiphanloai.IPHANLOAI);
                ViewData["id"] = fc["id"];
                return PartialView("../Ajax/Thietlap/Traloi_Phanloai_edit");
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa danh sách kiện nghị trả lời phân loại");
                //return null;
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Traloi_Phanloai_update(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....

                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(52, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                string cTen = func.RemoveTagInput(fc["cTen"]).ToUpper().Trim();
                decimal iparent = Convert.ToDecimal(fc["iparent"]);
                string cCode = func.RemoveTagInput(fc["cCode"]).Trim();
                if (_thietlap.Get_TraLoi_PhanLoai().Where(v => v.CCODE != null  && v.CTEN.ToUpper().Trim() == cTen.ToUpper().Trim() && v.IDELETE == 0 && v.CCODE != cCode && v.IPHANLOAI != id).ToList().Count() > 0)
                {
                    Response.Write(2);
                }
                else
                {
                    if (!CheckTokenAction("thietlap_suatraloiphanloai", id))
                    {
                        //Response.Redirect("/Home/Error");
                        return null;
                    }

                    var traloiphanloai = _thietlap.GetBy_TraLoi_PhanLoaiID(id);
                    traloiphanloai.CTEN = func.RemoveTagInput(fc["cTen"]).Trim();
                    traloiphanloai.IPARENT = Convert.ToDecimal(fc["iparent"]);
                    traloiphanloai.CCODE = func.RemoveTagInput(fc["cCode"]).Trim();
                    _thietlap.Update_TraLoi_PhanLoai(traloiphanloai);
                    Tracking(iUser, "Sửa kiến nghị trả lời phân loại: " + traloiphanloai.CTEN);
                    Response.Write(1);
                }
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Sửa kiến nghị trả lời phân loại");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Traloi_Phanloai_search(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //....
                UserInfor u_info = GetUserInfor();
                if (!base_buss.Action_(52, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                string id_encr = fc["id"];
                int id = 0;
                if (id_encr != "0")
                {
                    id = Convert.ToInt32(id_encr);
                }
              
                int iUser = u_info.tk_action.iUser;
                List<KN_TRALOI_PHANLOAI> Traloiphanloai = _thietlap.Get_TraLoi_PhanLoai();
                Response.Write(tl.List_Traloiphanloai_search(Traloiphanloai, 0,0, iUser,"", id));
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Tìm kiếm danh sách kiện nghị trả lời phân loại");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Traloi_Phanloai_status(FormCollection fc)
        {
            if (!CheckAuthToken_Api()) { return null; }
            try
            {
                //....


                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_TRALOI_PHANLOAI q = _thietlap.GetBy_TraLoi_PhanLoaiID(id);
                if (q.IHIENTHI == 1)
                {
                    q.IHIENTHI = 0;
                    Tracking(iUser, "Bỏ chọn áp dụng trả lời phân loại: " + q.CTEN);
                }
                else
                {
                    q.IHIENTHI = 1;
                    Tracking(iUser, "Chọn áp dụng trả lời phân loại: " + q.CTEN);
                }
                _thietlap.Update_TraLoi_PhanLoai(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái danh sách trả lời phân loại");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Traloi_Phanloai_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                KN_TRALOI_PHANLOAI q = _thietlap.GetBy_TraLoi_PhanLoaiID(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_TraLoi_PhanLoai(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí trả lời phân loại");
                //return null;
                throw;
            }

        }
        public ActionResult Ajax_Noidungdon_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                KNTC_NOIDUNGDON q = _thietlap.GetBy_NoidungdonID(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Noidungdon(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí nội dung đơn");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Kyhop_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                QUOCHOI_KYHOP q = _thietlap.Get_Quochoi_Kyhop(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_QuocHoi_Kyhop(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí kỳ họp");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Khoa_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                QUOCHOI_KHOA q = _thietlap.Get_Quochoi_Khoa(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_QuocHoi_Khoa(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí khóa họp");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Chucvu_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                USER_CHUCVU q = _thietlap.Get_Chucvu(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Userchucvu(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí chức vụ");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Daibieu_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

               DAIBIEU q = _thietlap.GetBy_DaibieuID(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Daibieu(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí đại biểu");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Tinhchatdon_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                KNTC_TINHCHAT q = _thietlap.GetBy_TinhchatID(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Tinhchat(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí tính chất");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Nghenghiep_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                NGHENGHIEP q = _thietlap.GetBy_NghenghiepID(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Nghenghiep(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí nghề nghiệp");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Quoctich_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                QUOCTICH q = _thietlap.GetBy_QuoctichID(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Quoctich(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí quốc tịch");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Linhvuc_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                LINHVUC q = _thietlap.GetBy_LinhvucID(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Linhvuc(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí lĩnh vực");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Dantoc_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                DANTOC q = _thietlap.GetBy_DantocID(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Dantoc(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí dân tộc");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Vanban_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                VB_LOAI q = _thietlap.GetBy_LoaivanbanID(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Loaivanban(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí văn bản");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Change_phongban_donvi(int id)
        {
            if (!CheckAuthToken_Api()) { return null; }
            Response.Write(tl.Option_PhongBan_ByDonVi_TK(id, 0));
            return null;
        }

        public ActionResult Ajax_Change_Chucvu (int id)
        {
            if (!CheckAuthToken_Api()) { return null; }
            Response.Write(tl.Option_ChucVu_TheoPhongBan(id));
            return null;
        }
        // Bổ sung ngày 04
        public ActionResult Ajax_Sosanh2ngaytimkiem(FormCollection fc)
        {
            DateTime denngay = Convert.ToDateTime(func.ConvertDateToSql(fc["dDenNgay"].ToString()));
            DateTime tungay = Convert.ToDateTime(func.ConvertDateToSql(fc["dTuNgay"].ToString()));
           
            if ( Convert.ToInt32((denngay - tungay).TotalDays) > 7)
            {
                Response.Write(denngay.AddDays(-7).ToString("dd/MM/yyyy"));

            }
            else
            {
                Response.Write(1);
            }
            return null;
        }
        // Bổ sung ngày 06 03 2018
        public ActionResult Ajax_Linhvuc_Coquan_order(FormCollection fc)
        {
            try
            {
                //....
                if (!CheckAuthToken()) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int value = Convert.ToInt32(fc["value"]);

                LINHVUC_COQUAN q = _thietlap.GetBy_Linhvuc_CoquanID(id);
                q.IVITRI = Convert.ToInt32(value);
                _thietlap.Update_Linhvuc_Coquan(q);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, "Trạng thái vị trí lĩnh vực cơ quan");
                //return null;
                throw;
            }
        }
        // Bổ sung 
        [ValidateInput(false)]
        public ActionResult Ajax_Nhomtaikhoan_status(FormCollection fc)
        {
            try
            {

                if (!CheckAuthToken()) { return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));


                USER_GROUP tk = _thietlap.GetBy_UsergroupID(id);
                if (tk.IDELETE == 1)
                {
                    tk.IDELETE = 0;
                    Tracking(iUser, "Bỏ kích hoạt tài khoản: <strong>" + tk.CTEN + "</strong>");

                }
                else
                {
                    tk.IDELETE = 1;
                    Tracking(iUser, "Kích hoạt tài khoản: <strong>" + tk.CTEN + "</strong> ");
                }
                _thietlap.Update_Usergroup(tk);
                Response.Write(1);
                return null;
            }
            catch (Exception e)
            {
                log.Log_Error(e, " Sửa trạng thái sách tài khoản");
                //return null;
                throw;
            }
        }
        // Bổ sung 08/04 
        public ActionResult Ajax_Linhvuc_add_dv(FormCollection fc)
        {
            int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
            ViewData["cTen"] = _thietlap.GetBy_LinhvucID(id).CTEN;
            ViewData["List"] = tl.List_CheckBox_CoQuan(id);
            ViewData["idlinhvuc"] = id;
            return PartialView("../Ajax/Thietlap/LinhVuc_DonVi_add");
        }
        public ActionResult Ajax_DonVi_LinhVuc_insert(FormCollection fc)
        {
            UserInfor u_info = GetUserInfor();
            int iUser = u_info.tk_action.iUser;
            int id = Convert.ToInt32(fc["idlinhvuc"]);
            _condition = new Dictionary<string, object>();
            _condition.Add("ILINHVUC", id);
            List<DONVI_LINHVUC> ua = _thietlap.GetBy_List_DonVi_LinhVuc(_condition);
            foreach (var i in ua)
            {
                DONVI_LINHVUC uac = _thietlap.GetBy_DonVi_LinhVucID((int)i.ID);
                _thietlap.Delete_DonVi_LinhVuc(uac);
            }

            if (fc["action"] != null)
            {
                string arr = fc["action"];
                if (arr != "")
                {
                    foreach (var x in arr.Split(','))
                    {
                        if (x != "")
                        {
                            DONVI_LINHVUC a = new DONVI_LINHVUC();
                            a.ILINHVUC = id;
                            a.IDONVI = Convert.ToInt32(x);
                            _thietlap.Insert_DonVi_LinhVuc(a);
                        }
                    }
                }
            }
            Tracking(iUser, "Chọn đơn vị cho lĩnh vực " + _thietlap.GetBy_LinhvucID(id).CTEN+ "");
            Response.Write(1);
            return null;

        }
        public ActionResult Ajax_Daubieu_add_Kyhop(FormCollection fc)
        {
            int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
            ViewData["cTen"] = _thietlap.GetBy_DaibieuID(id).CTEN;
            ViewData["List"] = tl.List_CheckBox_KyHop(id);
            ViewData["iddaibieu"] = id;
            return PartialView("../Ajax/Thietlap/DaiBieu_KyHop_add");
        }
        public ActionResult Ajax_DaiBieu_KyHop_insert(FormCollection fc)
        {
            UserInfor u_info = GetUserInfor();
            int iUser = u_info.tk_action.iUser;
            int id = Convert.ToInt32(fc["iddaibieu"]);
            _condition = new Dictionary<string, object>();
            _condition.Add("ID_DAIBIEU", id);
            List<DAIBIEU_KYHOP> ua = _thietlap.GetBy_List_DaiBieu_KyHop(_condition);
            foreach (var i in ua)
            {
                DAIBIEU_KYHOP uac = _thietlap.GetBy_DaiBieu_KyHopID((int)i.ID);
                _thietlap.Delete_DaiBieu_KyHop(uac);
            }

            if (fc["action"] != null)
            {
                string arr = fc["action"];
                if (arr != "")
                {
                    foreach (var x in arr.Split(','))
                    {
                        if (x != "")
                        {
                            DAIBIEU_KYHOP a = new DAIBIEU_KYHOP();
                            a.ID_DAIBIEU = id;
                            a.ID_KYHOP = Convert.ToInt32(x);
                            _thietlap.Insert_DaiBieu_KyHop(a);
                        }
                    }
                }
            }
            Tracking(iUser, "Chọn kỳ họp cho đại biểu " + _thietlap.GetBy_DaibieuID(id).CTEN + "");
            Response.Write(1);
            return null;

        }

        // Bổ sung 04 - 11
        public ActionResult Ajax_Load_Opt_donvi()
        {
            if (!CheckAuthToken_Api()) { return null; }
            Response.Write(tl.OptionCoQuan_TreeList());
            return null;
        }
       
    }
}
