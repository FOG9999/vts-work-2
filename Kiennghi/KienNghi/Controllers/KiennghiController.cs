using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;
using KienNghi.App_Code;
using DataAccess.Busineess;
using Entities.Models;
using System.IO;
using Spire.Xls;
using System.Data;
using Entities.Objects;
using Utilities.Enums;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using KienNghi.Flexcel;
using System.Globalization;

namespace KienNghi.Controllers
{
    public class KiennghiController : FlexcelReportController
    {
        //
        // GET: /Kiennghi/
        Funtions func = new Funtions();
        //Base _base = new Base();
        KiennghiBusineess _kiennghi = new KiennghiBusineess();
        KienNghiReport _kienNghiReport = new KienNghiReport();
        ThietlapBusineess _thietlap = new ThietlapBusineess();
        Dictionary<string, object> _condition;
        Kiennghi_cl kn = new Kiennghi_cl();
        Thietlap tl = new Thietlap();
        BaseBusineess base_business = new BaseBusineess();
        KntcBusineess kntc = new KntcBusineess();
        Khieunai khieunai = new Khieunai();
        Log log = new Log();
        Base base_appcode = new Base();
        public int ID_PARENT_TODAIBIEU = 46;
        public int ThamQuyenDiaPhuong;
        //phúc 
        //Star update lại ITRALOI trong KN_GIAMSAT

        public ActionResult Tamxoa()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                string url = Request.RawUrl;
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    string[] arr_post_per_page = Request["post_per_page"].Split(',');
                    if (arr_post_per_page.Length > 1)
                    {
                        post_per_page = Convert.ToInt32(arr_post_per_page[0]);
                    }
                    else
                    {
                        post_per_page = Convert.ToInt32(Request["post_per_page"]);
                    }

                }
                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("3,4,5", u_info.tk_action)) { Response.Redirect("/Home/Error/"); }
                if (!base_business.ActionMulty_("5,50", u_info.tk_action))//chọn Tập hợp
                {
                    ViewData["bt-add-tonghop"] = " style='display:none'";
                }
                if (!base_business.ActionMulty_("3,50", u_info.tk_action))//thêm mới kiến nghị
                {
                    ViewData["bt-add"] = " style='display:none'";
                    ViewData["bt-gop"] = " style='display:none'";
                }
                if (!u_info.tk_action.is_lanhdao) { ViewData["bt-gop"] = " style='display:none'"; }
                int iDonViTiepNhan = 0;
                int iDonViXuLy = 0;
                int iNguonKienNghi = 0;
                int iDonViXuLy_Parent = 0;
                int iDiaPhuong_0 = 0;
                int iDiaPhuong_1 = 0;

                int iKyHop = ID_KyHop_HienTai();
                KN_KIENNGHI kn_pram = get_Request_Paramt_KienNghi();
                if (Request["iDoan"] != null)
                {
                    iDonViTiepNhan = Convert.ToInt32(Request["iDoan"]);
                }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (Request["q"] != null) { kn_pram.CNOIDUNG = func.RemoveTagInput(Request["q"]); }
                if (Request["cNoiDung"] != null) { kn_pram.CNOIDUNG = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                if (Request["iNguonKienNghi"] != null) { iNguonKienNghi = Convert.ToInt32(Request["iNguonKienNghi"]); }
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDiaPhuong_0"] != null) { iDiaPhuong_0 = Convert.ToInt32(Request["iDiaPhuong_0"]); }
                if (Request["iDiaPhuong_1"] != null) { iDiaPhuong_1 = Convert.ToInt32(Request["iDiaPhuong_1"]); }

                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenGiaiQuyet(iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    ViewData["opt-thamquyen-xuly"] = Get_Option_DonViThamQuyen_ByCType(iDonViXuLy_Parent, iDonViXuLy);
                }
                else
                {
                    //coquan = null;
                    ViewData["opt-thamquyen-xuly"] = Get_Option_DonViThamQuyen_ByCType();
                }
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                kn_pram.ITHAMQUYENDONVI = iDonViXuLy;
                kn_pram.IDONVITIEPNHAN = iDonViTiepNhan;
                kn_pram.ITINHTRANG = (decimal)TrangThaiKienNghi.TamxoaKiennghi;
                kn_pram.IUSER = iUser;
                kn_pram.IDIAPHUONG0 = iDiaPhuong_0;
                kn_pram.IDIAPHUONG1 = iDiaPhuong_1;
                //kn_pram.INGUONKIENNGHI = iNguonKienNghi;
                //Mặc định sẽ hiển thị hết kiến nghị trong mọi kỳ họp
                kn_pram.IKYHOP = 0;
                string listKyHop = "0";
                ViewData["dbqh"] = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    kn_pram.IKYHOP = -1;
                    listKyHop = Request["listKyHop"];
                }
                List<PRC_KIENNGHI_TAMXOA> list_kn;
                /*Bổ sung thêm các trường tìm kiếm */
                string imakiennghi = "";
                if (Request["iMaKienNghi"] != "" && Request["iMaKienNghi"] != null)
                {
                    imakiennghi = Request["iMaKienNghi"];
                }
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["iTuNgay"] != null && Request["iTuNgay"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["iTuNgay"]));
                }
                if (Request["iDenNgay"] != null && Request["iDenNgay"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["iDenNgay"]));
                }
                string lstNguonKN = "";
                if (Request["listNguonKN"] != null)
                {
                    lstNguonKN = Request["listNguonKN"];
                }
                string lstLinhVuc = "";
                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                string cnoidung = Request["cNoiDung"];

                int iTruocKyHop = -1;
                if (Request["listLinhVuc"] != null)
                {
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);
                }

                list_kn = _kiennghi.PRC_KIENNGHI_TAMXOA(kn_pram, listKyHop, imakiennghi, dtungay, ddenngay, lstNguonKN, lstLinhVuc, cnoidung, 0, page, post_per_page, iTruocKyHop);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (list_kn != null && list_kn.Count() > 0)
                {
                    if (!u_info.tk_action.is_lanhdao)
                    {
                        list_kn.Where(x => x.ITINHTRANG == 0).ToList();
                    }
                    List<ID_Session_KienNghi_ChonTongHop> list_id_kiennghi_tonghop = Get_IDKienNghiChonTongHop();
                    htmlList = kn.KN_Tamxoa_Tracuu(list_kn, u_info.tk_action, list_id_kiennghi_tonghop);
                }
                else
                {
                    htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }
                if (list_kn != null && list_kn.Count() > 0)
                {
                    htmlPhanTrang = "<tr><td colspan='5'>" + base_appcode.PhanTrang((int)list_kn.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }

                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }

                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-coquan"] = Get_Option_DonViTiepNhan(iDonViTiepNhan);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tạm xóa kiến nghị");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Xoatam(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI k = _kiennghi.HienThiThongTinKienNghi(id);
                k.IDELETE = 1;
                _kiennghi.UpdateThongTinKienNghi(k);
                if(k.ID_IMPORT != null && k.ID_IMPORT != 0)
                {
                    //update kn_import
                    KN_IMPORT ip = _kiennghi.Get_Import((int)k.ID_IMPORT);
                    ip.ISOKIENNGHI--;
                    _kiennghi.Update_Import(ip);
                }
                Response.Write(1);
                _kiennghi.Tracking_KN((int)u.user_login.IUSER, id, "Xóa tạm kiến nghị");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tạm xóa kiến nghị");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_Phuchoi(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI k = _kiennghi.HienThiThongTinKienNghi(id);
                k.IDELETE = 0;
                _kiennghi.UpdateThongTinKienNghi(k);
                if (k.ID_IMPORT != null && k.ID_IMPORT != 0)
                {
                    //update kn_import
                    KN_IMPORT ip = _kiennghi.Get_Import((int)k.ID_IMPORT);
                    ip.ISOKIENNGHI++;
                    _kiennghi.Update_Import(ip);
                }
                Response.Write(1);
                _kiennghi.Tracking_KN((int)u.user_login.IUSER, id, "Xóa kiến nghị");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Phục hồi kiến nghị");
                //return null;
                throw;
            }
        }
        public Boolean Set_IDKienNghiChonTongHop(ID_Session_KienNghi_ChonTongHop id)
        {
            bool result = true;
            try
            {
                List<ID_Session_KienNghi_ChonTongHop> list_id = Get_IDKienNghiChonTongHop();
                if (list_id != null)
                {
                    if (list_id.Where(x => x.IKIENNGHI == id.IKIENNGHI).Count() == 0)
                    {
                        list_id.Add(id);
                    }
                    else
                    {
                        ID_Session_KienNghi_ChonTongHop id_session = list_id.Where(x => x.IKIENNGHI == id.IKIENNGHI).First();
                        list_id.Remove(id_session);
                    }
                    //if (!list_id.Contains(id))
                    //{
                    //    list_id.Add(id);
                    //}else
                    //{
                    //    list_id.Remove(id);
                    //}
                }
                else
                {
                    list_id.Add(id);
                }
                System.Web.HttpContext.Current.Session.Add("id_kiennghi", list_id);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public Boolean RemoveAll_IDKienNghiChonTongHop()
        {
            bool result = true;
            try
            {
                List<ID_Session_KienNghi_ChonTongHop> list_id = null;
                System.Web.HttpContext.Current.Session.Add("id_kiennghi", list_id);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public List<ID_Session_KienNghi_ChonTongHop> Get_IDKienNghiChonTongHop()
        {
            List<ID_Session_KienNghi_ChonTongHop> list_id = new List<ID_Session_KienNghi_ChonTongHop>();
            if (System.Web.HttpContext.Current.Session["id_kiennghi"] != null)
            {
                list_id = (List<ID_Session_KienNghi_ChonTongHop>)Session["id_kiennghi"];
            }
            return list_id;
        }
        public ActionResult Update_ITRALOI_KN_GIAMSAT()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                var giamsat = _kiennghi.GetAll_Giamsat_TraLoi();
                var traloi = _kiennghi.GetAll_TraLoi_KienNghi();
                foreach (var g in giamsat)
                {
                    int iKienNghi = (int)g.IKIENNGHI;
                    var traloi_ = traloi.Where(x => x.IKIENNGHI == iKienNghi).ToList();
                    if (traloi_.Count() > 0)
                    {
                        KN_GIAMSAT gi = g;
                        gi.ITRALOI = (int)traloi_.FirstOrDefault().ITRALOI;
                        _kiennghi.Update_Giamsat_traloi(gi);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "");
            }
            return null;
        }
        //End update lại ITRALOI trong KN_GIAMSAT
        public ActionResult Ajax_Chuongtrinh_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //xóa đại biểu
                List<KN_CHUONGTRINH_DAIBIEU> ct = _kiennghi.Get_DaiBieu_ByChuongTrinh(id);
                foreach (var d in ct) { _kiennghi.Delete_DaiBieu_ByChuongTrinh(d); }
                // xóa địa phương
                List<KN_CHUONGTRINH_DIAPHUONG> dp = _kiennghi.Get_Diaphuong_ByChuongTrinh(id);
                foreach (var d in dp) { _kiennghi.Delete_Diaphuong_ByChuongTrinh(d); }
                //
                KN_CHUONGTRINH chuongtrinh = _kiennghi.Get_ChuongTrinh_ByID(id);
                int iUser = (int)GetUserInfor().user_login.IUSER;
                Tracking(iUser, "Xóa chương trình tiếp xúc cử tri, kế hoạch số: " + chuongtrinh.CKEHOACH);
                _kiennghi.Delete_ChuongTrinh_ByID(chuongtrinh);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa chương trình tiếp xúc cử tri");
                //return null;
                throw;
            }
        }
        public ActionResult Ajax_search_chuongtrinh()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iKyHop = ID_KyHop_HienTai();
                int iDonVi = (int)u_info.user_login.IDONVI;
                ViewData["is_dbqh"] = 1;
                if (u_info.tk_action.is_lanhdao)
                {
                    iDonVi = 0; ViewData["is_dbqh"] = 0; ViewData["opt-donvi"] = Get_Option_DoanDaiBieu(iDonVi);
                }
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                return PartialView("../Ajax/Kiennghi/search_chuongtrinh");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiếm chương trình tiếp xúc cử tri");
                throw;
            }
            //return null;
        }
        public ActionResult Ajax_search_chuongtrinh_change_doan(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(fc["id"]);
                string str = "<option value='0'>- - - Chọn tất cả</option>";
                if (id != 0)
                {
                    List<DIAPHUONG> diaphuong = _kiennghi.GetAll_DiaPhuong().Where(x => x.IHIENTHI == 1).ToList();
                    QUOCHOI_COQUAN tk_coquan = _kiennghi.HienThiThongTinCoQuan(id);
                    if (tk_coquan.IDIAPHUONG != 0)
                    {
                        diaphuong = diaphuong.Where(x => x.IPARENT == (int)tk_coquan.IDIAPHUONG).OrderBy(x => x.CTEN).ToList();
                        str += kn.Option_All_Tinh_Huyen(diaphuong, (int)tk_coquan.IDIAPHUONG);
                    }
                }
                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex);
                throw;
            }

        }
        public ActionResult Ajax_search_chuongtrinh_change_diaphuong(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(fc["id"]);
                string str = "<option value='0'>- - - Chọn tất cả</option>";
                if (id != 0)
                {
                    Dictionary<string, object> _dic_daibieu = new Dictionary<string, object>();
                    var daibieu = _kiennghi.GetAll_Daibieu(_dic_daibieu).Where(x => x.IHIENTHI == 1).OrderByDescending(x => x.ITRUONGDOAN).ToList();
                    str += "<option value='0'>- - - Chọn tất cả</option>" + kn.Option_DaiBieu_ByID_DiaPhuong(daibieu, 0);
                }
                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex);
                throw;
            }

        }
        public ActionResult Ajax_Chuyen_tonghop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int id_th = Convert.ToInt32(HashUtil.Decode_ID(fc["id_th"], Request.Cookies["url_key"].Value));
                KN_TONGHOP knTonghop = _kiennghi.Get_Tonghop(id_th);
                int iKyHop = ID_KyHop_HienTai();
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                SetTokenAction("Chuyen_tonghop", id);
                ViewData["id"] = Request["id"];
                ViewData["id_th"] = fc["id_th"];
                ViewData["dbqh"] = 1;
                Console.WriteLine(fc);
                int iUser = (int)u_info.user_login.IUSER;
                int iThamQuyenDonVi = (int)knTonghop.ITHAMQUYENDONVI;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                    ViewData["opt-thamquyen"] = Get_Option_DonViThamQuyen_ByCType(Int32.Parse(fc["loaiTaphop"]), (int)knTonghop.ITHAMQUYENDONVI);
                    ViewData["dbqh"] = 0;
                }
                string LOAICQ = "";
                if (Int32.Parse(fc["loaiTaphop"]) == (int)ThamQuyen_DiaPhuong.Trunguong)
                    LOAICQ = typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value;
                if (Int32.Parse(fc["loaiTaphop"]) == (int)ThamQuyen_DiaPhuong.Tinh)
                    LOAICQ = typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value;
                if (Int32.Parse(fc["loaiTaphop"]) == (int)ThamQuyen_DiaPhuong.Huyen)
                    LOAICQ = typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value;
                
                dic.Add("ITINHTRANG", (int)TrangThai_TongHop.ChoXuLy);
                dic.Add("IDONVITONGHOP", 0);
                dic.Add("ITHAMQUYENDONVI", iThamQuyenDonVi);
                dic.Add("IKYHOP", 0);
                dic.Add("IUSER", iUser);
                dic.Add("LOAICQ", LOAICQ);
                var tonghop = _kiennghi.GetAll_TongHopByType(dic);
                string str = "";
                if (tonghop.Count() > 0) 
                {
                    foreach (var t in tonghop)
                    {
                        if (t.ITONGHOP == id_th)
                        {
                            str += "<option selected value='" + t.ITONGHOP + "'>" + t.CNOIDUNG + "</option>";
                        }
                        else
                        {
                            str += "<option value='" + t.ITONGHOP + "'>" + t.CNOIDUNG + "</option>";
                        }
                    }
                }
                ViewData["loaicq"] = LOAICQ;
                ViewData["opt-tonghop"] = str;
                return PartialView("../Ajax/Kiennghi/Chuyen_tonghop");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "CHuyển kiến nghị sang Tập hợp khác");
                throw;
            }
        }
        [HttpPost]
        public ActionResult Kiennghi_chuyen_insert(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Chuyen_tonghop", id)) { Response.Redirect("/Home/Error/"); return null; }
                int iTongHop = Convert.ToInt32(fc["iTongHop"]);
                KN_TONGHOP th = _kiennghi.Get_Tonghop(iTongHop);
                if (th == null)
                {
                    Response.Redirect("/Home/Error/"); return null;
                }

                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                if (u_info.tk_action.is_lanhdao)// Lưu lại lịch sử chuyển kiến nghị sang thẩm quyền khác;
                {
                    int iTongHop_Cu = (int)kiennghi.ITONGHOP_BDN;
                    int iTongHop_Moi = iTongHop;
                    KN_TONGHOP th_cu = _kiennghi.Get_Tonghop(iTongHop_Cu);
                    if (th_cu != null)
                    {
                        if (th_cu.ITHAMQUYENDONVI != th.ITHAMQUYENDONVI)// khác thẩm quyền đơn vị
                        {
                            QUOCHOI_COQUAN coquan_moi = _kiennghi.HienThiThongTinCoQuan((int)th.ITHAMQUYENDONVI);
                            QUOCHOI_COQUAN coquan_cu = _kiennghi.HienThiThongTinCoQuan((int)th_cu.ITHAMQUYENDONVI);
                            if (coquan_cu != null && coquan_moi != null)
                            {
                                _kiennghi.Tracking_KN_TongHop((int)u_info.user_login.IUSER, id, iTongHop,
                                    "Chuyển từ tập hợp thuộc thẩm quyền \"" + coquan_cu.CTEN + "\" sang thẩm quyền \"" + coquan_moi.CTEN + "\"");
                            }
                        }
                    }
                }
                else
                {
                    _kiennghi.Tracking_KN((int)u_info.user_login.IUSER, id, "Chuyển kiến nghị sang Tập hợp khác!");
                }
                kiennghi.ITONGHOP = iTongHop;
                _kiennghi.UpdateThongTinKienNghi(kiennghi);
                Response.Redirect(Request.Cookies["url_return"].Value);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "CHuyển kiến nghị sang Tập hợp khác");
                throw;
            }
        }
        public ActionResult Ajax_Change_donvi_tonghop_option(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(fc["id"]);
                int id_kiennghi = Convert.ToInt32(HashUtil.Decode_ID(fc["id_kiennghi"], Request.Cookies["url_key"].Value));
                string loaicq = Convert.ToString(fc["loaicq"]);
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id_kiennghi);
                int iKyHop = ID_KyHop_HienTai();
                if (kiennghi != null) { iKyHop = (int)kiennghi.IKYHOP; }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                UserInfor u_info = GetUserInfor();
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                dic.Add("IUSER", iUser);
                dic.Add("ITHAMQUYENDONVI", id);
                dic.Add("ITINHTRANG", (int)TrangThai_TongHop.ChoXuLy);
                dic.Add("IDONVITONGHOP", 0);
                dic.Add("IKYHOP", 0);
                dic.Add("LOAICQ", loaicq);
                var tonghop = _kiennghi.GetAll_TongHopByType(dic);
                string str = "";
                if (tonghop.Count() > 0)
                {
                    foreach (var t in tonghop)
                    {
                        str += "<option value='" + t.ITONGHOP + "'>" + t.CNOIDUNG + "</option>";
                    }
                }
                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Hiển thị Tập hợp kiến nghị của đơn vị được chọn");
                throw;
            }
        }
        public ActionResult Duthao()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = new UserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                ViewData["tonghop"] = tonghop;
                TongHop_Kiennghi detail = kn.Tonghop_Detail(id, "");
                ViewData["detail"] = detail;
                ViewData["file"] = kn.File_View(id, "kn_tonghop");
                List<PRC_KIENNGHI_BYTONGHOP> kiennghi;
                kiennghi = _kiennghi.PRC_LIST_KIENNGHI_BY_TONGHOP_KIENNGHI(0, id);
                ViewData["list"] = kn.List_KienNghi_ByID_tonghop_view_new(tonghop, kiennghi, Request.Cookies["url_key"].Value);
                ViewData["list-vanban"] = kn.List_Tonghop_Duthao(id, u_info.tk_action, Request.Cookies["url_key"].Value);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Dự thảo Tập hợp gửi đơn vị thẩm quyền xử lý");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Duthao_add(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = id;

                SetTokenAction("duthao_add", id);
                string type = fc["type"];
                ViewData["type"] = type;
                if (type == "duthao")
                {
                    ViewData["title"] = "Cập nhật dự thảo Tập hợp gửi cơ quan có thẩm quyền xử lý";
                }
                else
                {
                    ViewData["title"] = "Cập nhật trả lời dự thảo Tập hợp của cơ quan có thẩm quyền xử lý";
                }
                return PartialView("../Ajax/Kiennghi/Duthao_add");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Cập nhật dự thảo Tập hợp kiến nghị");
                throw;
            }
        }

        public ActionResult Ajax_Sua_congvan(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var tonghop = _kiennghi.Get_Tonghop(id);
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ITONGHOP", id);
                var kiennghi = _kiennghi.HienThiDanhSachKienNghi(param).First();
                param.Add("CLOAI", "tonghop_chuyendonvi_xuly"); //tonghop_chuyendonvi_xuly là CLOAI của văn bản chuyển công văn
                var vanban = _kiennghi.GetAll_VanbanByParam(param).First();
                param = new Dictionary<string, object>();
                param.Add("ICOQUAN", vanban.ICOQUANNHAN);
                var coquanchon = _kiennghi.GetAll_CoQuanByParam(param).First();
                int iType = 0;
                ViewData["socongvan"] = vanban.CSOVANBAN;
                ViewData["ngaybanhanh"] = vanban.DNGAYBANHANH;
                ViewData["ngayphanhoi"] = kiennghi.INGAYQUYDINH;
                ViewData["vanban"] = vanban;
                ViewData["nguoiky"] = vanban.CNGUOIKY;
                ViewData["kiennghi"] = kiennghi;
                ViewData["file"] = kn.File_Edit((int)vanban.IVANBAN, "kn_vanban", Request.Cookies["url_key"].Value);
                ViewData["id_vanban"] = vanban.IVANBAN;
                ViewData["radio-thamquyen"] = "";
                ViewData["opt-donvithamquyen"] = "";
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                {
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong, (int)coquanchon.ICOQUAN);
                    ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Trunguong);
                }

                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                {
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Tinh, (int)coquanchon.ICOQUAN);
                    ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Tinh);
                }

                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                {
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Huyen, (int)coquanchon.ICOQUAN);
                    ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Huyen);
                }

                return PartialView("../Ajax/Kiennghi/Sua_congvan");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Cập nhật dự thảo Tập hợp kiến nghị");
                throw;
            }
        }

        public ActionResult Ajax_Sua_congvan_update(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                var vanban = _kiennghi.Get_Vanban(Convert.ToInt32(fc["id_vanban"]));
                vanban.ICOQUANNHAN = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                vanban.CSOVANBAN = fc["cSoVanBan"];
                vanban.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                vanban.DNGAYBAOCAO = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayPhanHoi"]));
                vanban.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                _kiennghi.Update_Vanban(vanban);
                //Update các kiến nghị trong tổng hợp của văn bản chuyển công văn
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ITONGHOP", (int)vanban.ITONGHOP);
                var listKienNghi = _kiennghi.HienThiDanhSachKienNghi(param);
                foreach (var kienNghi in listKienNghi)
                {
                    kienNghi.INGAYQUYDINH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayPhanHoi"]));
                    _kiennghi.UpdateThongTinKienNghi(kienNghi);
                }
                Response.Redirect(Request.Cookies["url_return"].Value);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Cập nhật dự thảo Tập hợp kiến nghị");
                throw;
            }
        }
        public ActionResult Ajax_Duthao_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_VANBAN vanban = _kiennghi.Get_Vanban(id);
                ViewData["file"] = kn.File_Edit(id, "kn_vanban", Request.Cookies["url_key"].Value);
                ViewData["id"] = id;
                ViewData["vanban"] = vanban;
                SetTokenAction("duthao_edit", id);

                return PartialView("../Ajax/Kiennghi/Duthao_edit");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Cập nhật dự thảo Tập hợp kiến nghị");
                throw;
            }
        }
        public ActionResult Ajax_Vanban_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                KN_VANBAN v = _kiennghi.Get_Vanban(id);
                int iTonghop = (int)v.ITONGHOP;
                _kiennghi.Delete_vanban(v);
                _kiennghi.Tracking_Tonghop((int)u_info.user_login.IUSER, iTonghop, "Xóa văn bản dự thảo Tập hợp");
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xóa văn bản dự thảo Tập hợp ");
                throw;
            }
        }
        public ActionResult Ajax_Import_kiennghi_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                KN_IMPORT ip = _kiennghi.Get_Import(id);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ID_IMPORT", id);


                //if (iKyHop != 0) { dic.Add("iKyHop", iKyHop); }
                List<KN_KIENNGHI> kn = _kiennghi.HienThiDanhSachKienNghi(dic);

                foreach(var k in kn)
                {
                    _kiennghi.DeleteThongTinKienNghi(k);
                };
                _kiennghi.Delete_Import(ip);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa import kiến nghị ");
                throw;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Duthao_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(fc["id"]);
                if (!CheckTokenAction("duthao_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }

                UserInfor u_info = GetUserInfor();
                KN_VANBAN t = _kiennghi.Get_Vanban(id);
                t.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                t.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                t.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);

                _kiennghi.Update_Vanban(t);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = t.IVANBAN;
                        kntc.Upload_file(f);
                    }
                }
                _kiennghi.Tracking_Tonghop((int)u_info.user_login.IUSER, id, "Cập nhật văn bản dự thảo Tập hợp");
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Cập nhật văn bản dự thảo ");
                return View("../Home/Error_Exception");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Duthao_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(fc["id"]);
                if (!CheckTokenAction("duthao_add", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                string type = fc["type"];
                UserInfor u_info = GetUserInfor();
                KN_VANBAN t = new KN_VANBAN();
                t.IKIENNGHI = 0;
                t.ITONGHOP = id;
                t.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                t.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                t.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                t.ICOQUANBANHANH = 0;
                t.CLOAI = type;
                t.DDATE = DateTime.Now;
                t.DNGAYDUKIENHOANTHANH = DateTime.Now;
                t.ICOQUANBANHANH = ID_Ban_DanNguyen;
                t.ICOQUANNHAN = 0;
                t.IUSER = u_info.user_login.IUSER;
                _kiennghi.Insert_Vanban(t);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = t.IVANBAN;
                        kntc.Upload_file(f);
                    }
                }
                if (type == "duthao")
                {
                    _kiennghi.Tracking_Tonghop((int)u_info.user_login.IUSER, id, "Cập nhật văn bản dự thảo Tập hợp gửi cơ quan có thẩm quyền xử lý");
                }
                else
                {
                    _kiennghi.Tracking_Tonghop((int)u_info.user_login.IUSER, id, "Cập nhật văn bản trả lời dự thảo Tập hợp của cơ quan có thẩm quyền xử lý");
                }
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Cập nhật văn bản dự thảo ");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Themmoichuongtrinh()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                TaikhoanAtion act = u_info.tk_action;
                if (!base_business.Action_(2, act)) { Response.Redirect("/Home/Error/"); return null; }
                SetTokenAction("kn_themmoichuongtrinh", 0);
                ViewData["dbqh"] = 0;
                ViewData["opt-doituong"] = Get_Option_LoaiDoiTuong();
                Dictionary<string, object> _dic = new Dictionary<string, object>();
                _dic.Add("IPARENT", ID_Coquan_doandaibieu);
                var coquan = _kiennghi.GetAll_CoQuanByParam(_dic);
                ViewData["opt-donvi"] = Get_Option_DonViLap_PhanQuyen();
                ViewData["iKyHopHienTai"] = ID_KyHop_HienTai();
                ViewData["listKhoaHop"] = JsonConvert.SerializeObject(_kiennghi.GetAll_KhoaHop().OrderBy(x => x.DBATDAU).ToList());
                ViewData["listKyHop"] = JsonConvert.SerializeObject(_kiennghi.GetAll_KyHop().OrderBy(x => x.DBATDAU).ToList());
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                return View();

            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form thêm mới chương trình tiếp xúc cử tri");
                return View("../Home/Error_Exception");
            }

        }
        public string Get_Option_DonVi_GuiDen()
        {
            string str = "";
            //var linhvuc = _kiennghi.GetAll_LinhVuc_CoQuan_By_IDCoQuan();
            //var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            UserInfor u_info = GetUserInfor();
            if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
            {
                str += "<span class='span4'><label><input class='nomargin' type='radio' disabled  name='iDoiTuongGui' value='0'/>Quốc hội</label> </span>";
                str += "<span class='span4'><label><input class='nomargin' type='radio' disabled name='iDoiTuongGui' value='1' checked/>Hội Đồng Nhân Dân</label> </span>";
            }
            else if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
            {
                str += "<span class='span4'><label><input class='nomargin' type='radio' disabled name='iDoiTuongGui' value='0' checked/>Quốc hội</label> </span>";
                str += "<span class='span4'><label><input class='nomargin' type='radio' disabled name='iDoiTuongGui' value='1'/>Hội Đồng Nhân Dân</label> </span>";
            }
            else
            {
                str += "<span class='span4'><label><input class='nomargin' type='radio' onclick='ChangeDoiTuongGui()' name='iDoiTuongGui' value='0' checked/>Quốc hội</label> </span>";
                str += "<span class='span4'><label><input class='nomargin' type='radio' onclick='ChangeDoiTuongGui()' name='iDoiTuongGui' value='1'/>Hội Đồng Nhân Dân</label> </span>";
            }
            return str;
        }
        public ActionResult Ajax_Load_lichtiep_row(int id, int num, int iDoiTuong)
        {
            try
            {
                if (iDoiTuong == 0)
                {
                    ViewData["opt-daibieu"] = kn.OptionDaiBieu_By_ID_COQUAN(id, 0);
                }
                else
                {
                    ViewData["opt-diaphuong"] = kn.OptionDiaPhuong_By_ID_COQUAN(id, 0);
                }
                ViewData["num"] = num;
                ViewData["iDoiTuong"] = iDoiTuong;
                return PartialView("../Ajax/Kiennghi/Load_lichtiep_row");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "");
                return null;
            }
        }

        public JsonResult Ajax_Load_DaiBieu_HDND(int iDoiTuong, int iDonvi)
        {
            try
            {
                QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(iDonvi);
                if (coquan == null) return null;

                int id_diaphuong = (int)coquan.IDIAPHUONG;
                //Dictionary<string, object> daibieuParam = new Dictionary<string, object>();
                //daibieuParam.Add("IDIAPHUONG", ID_DiaPhuong_HienTai);
                //daibieuParam.Add("ILOAIDAIBIEU", iDoiTuong);
                //daibieuParam.Add("IDELETE", 0);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("IPARENT", ID_PARENT_TODAIBIEU);
                dic.Add("IDELETE", 0);
                var listToDaiBieu = _thietlap.GetBy_List_Phongban(dic);

                Dictionary<string, object> huyenParam = new Dictionary<string, object>();
                huyenParam.Add("IPARENT", ID_DiaPhuong_HienTai);
                huyenParam.Add("IDELETE", 0);
                huyenParam.Add("IHIENTHI", 1);
                var listHuyen = _kiennghi.GetAll_DiaPhuong(huyenParam);

                var result = new
                {
                    listDaiBieu = listToDaiBieu,
                    listHuyen = listHuyen
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "");
                return null;
            }
        }
        public ActionResult Ajax_Load_lichtiep(int id, int iDoiTuong)
        {
            try
            {
                return PartialView("../Ajax/Kiennghi/Load_lichtiep");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "");
                return null;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoichuongtrinh(FormCollection fc, HttpPostedFileBase file,
            string[] iDaiBieu, string[] iDiaPhuong, string[] iDiaPhuong_02, string[] dNgayTiep, string[] cDiaChi)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            if (!CheckTokenAction("kn_themmoichuongtrinh", 0))
            {
                Response.Redirect("/Home/Error/");
                return null;
            }
            try
            {
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }

                    }
                }

                int iUser = id_user();
                KN_CHUONGTRINH kn = new KN_CHUONGTRINH();
                kn.IKHOA = Convert.ToInt32(fc["iKhoaHop"]);
                kn.IKYHOP = Convert.ToInt32(fc["iKyHop"]);
                kn.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]);
                func.SetCookies("truockyhop", fc["iTruocKyHop"]);
                func.SetCookies("truockyhop", fc["iTruocKyHop"]);
                kn.IUSER = iUser;
                kn.DDATE = DateTime.Now;
                kn.IDONVI = Convert.ToInt32(fc["iDonVi"]);
                if (fc["dBatDau"] != "")
                    kn.DBATDAU = Convert.ToDateTime(func.ConvertDateToSql(fc["dBatDau"]));
                if (fc["dKetThuc"] != "")
                    kn.DKETTHUC = Convert.ToDateTime(func.ConvertDateToSql(fc["dKetThuc"]));
                kn.IDOITUONG = Convert.ToInt32(fc["iDoituong"]);
                if (fc["dNgaybanhanh"] != "")
                    kn.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgaybanhanh"]));
                kn.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                kn.CDIACHI = func.RemoveTagInput(fc["cDiaChiTiepXuc"]);
                kn.CKEHOACH = func.RemoveTagInput(fc["cKeHoach"]);
                _kiennghi.InsertChuongTrinhKienNghi(kn);
                int ICHUONGTRINH = (int)kn.ICHUONGTRINH;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_chuongtrinh";
                        f.CFILE = UploadFile(file);
                        f.ID = ICHUONGTRINH;
                        kntc.Upload_file(f);
                    }

                }

                var iDiaPhuongDBQH = 0;
                if (Convert.ToInt32(fc["iDoituong"]) == 0)
                {
                    QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(Convert.ToInt32(fc["iDonVi"]));
                    if (coquan != null)
                    {
                        iDiaPhuongDBQH = coquan.IDIAPHUONG.HasValue ? (int)coquan.IDIAPHUONG : 0;
                    }
                }
                if (fc["iDaiBieu"] != null)
                {
                    for (int i = 0; i < iDaiBieu.Count(); i++)
                    {
                        if (Convert.ToInt32(iDaiBieu[i]) != 0)
                        {
                            KN_CHUONGTRINH_CHITIET c = new KN_CHUONGTRINH_CHITIET();
                            c.ICHUONGTRINH = ICHUONGTRINH;
                            c.ITODAIBIEU = Convert.ToInt32(iDaiBieu[i]);
                            c.IDIAPHUONG = Convert.ToInt32(iDiaPhuong[i]);
                            c.IDIAPHUONG2 = Convert.ToInt32(iDiaPhuong_02[i]);
                            c.CDIACHI = cDiaChi[i];
                            if (dNgayTiep[i].ToString() != "")
                            {
                                c.DNGAYTIEP = Convert.ToDateTime(func.ConvertDateToSql(dNgayTiep[i].ToString()));
                            }
                            _kiennghi.InsertCHUONGTRINH_CHITIET(c);
                        }
                    }
                }
                //string daibieu = "";
                //if (fc["daibieu_chon"] != null) {
                //    daibieu = fc["daibieu_chon"]; }
                //string diaphuong = "";
                //if (fc["diaphuong_chon"] != null) {
                //    diaphuong = fc["diaphuong_chon"]; }
                //if (daibieu != "") { Insert_DaiBieu_ChuongTrinh(ICHUONGTRINH, daibieu); }
                //if (diaphuong != "") { Insert_DiaPhuong_ChuongTrinh(ICHUONGTRINH, diaphuong); }
                string id_chuongtrinh = HashUtil.Encode_ID(kn.ICHUONGTRINH.ToString(), Request.Cookies["url_key"].Value);
                Tracking(iUser, "Thêm chương trình tiếp xúc cử tri, kế hoạch số: " + kn.CKEHOACH);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thêm mới chương trình tiếp xúc cử tri");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Chuongtrinh_chitiet_insert(FormCollection fc)
        {
            try
            {
                if (!CheckTokenAction("Chuongtrinh_chitiet_add", 0)) { Response.Redirect("/Home/Error/"); return null; }
                KN_CHUONGTRINH_CHITIET c = new KN_CHUONGTRINH_CHITIET();
                c.ICHUONGTRINH = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                c.ITODAIBIEU = Convert.ToInt32(fc["iDaiBieu"]);
                c.IDIAPHUONG = Convert.ToInt32(fc["iDiaPhuong"]);
                c.CDIACHI = fc["cDiaChi"];
                if (fc["dNgayTiep"].ToString() != "")
                {
                    c.DNGAYTIEP = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTiep"].ToString()));
                }
                _kiennghi.InsertCHUONGTRINH_CHITIET(c);
                Response.Redirect("/Kiennghi/Chuongtrinh_chitiet/?id=" + fc["id"]);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách chi tiết chương trình tiếp xúc cử tri");
                return View("../Home/Error_Exception");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Chuongtrinh_chitiet_update(FormCollection fc)
        {
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Chuongtrinh_chitiet_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                KN_CHUONGTRINH_CHITIET c = _kiennghi.GetByID_ChuongTrinhChiTiet(id);
                c.ITODAIBIEU = Convert.ToInt32(fc["iDaiBieu"]);
                c.IDIAPHUONG = Convert.ToInt32(fc["iDiaPhuong"]);
                c.CDIACHI = fc["cDiaChi"];
                if (fc["dNgayTiep"].ToString() != "")
                {
                    c.DNGAYTIEP = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTiep"].ToString()));
                }
                _kiennghi.UpdateCHUONGTRINH_CHITIET(c);

                Response.Redirect("/Kiennghi/Chuongtrinh_chitiet/?id=" + HashUtil.Encode_ID(c.ICHUONGTRINH.ToString(), Request.Cookies["url_key"].Value));
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách chi tiết chương trình tiếp xúc cử tri");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Chuongtrinh_chitiet_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_CHUONGTRINH_CHITIET c = _kiennghi.GetByID_ChuongTrinhChiTiet(id);
                _kiennghi.DeleteCHUONGTRINH_CHITIET(c);

                Response.Write(1);
                return null;

            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xóa lịch tiếp");
                throw;
            }

        }
        public ActionResult Ajax_Chuongtrinh_chitiet_add(FormCollection fc)
        {
            try
            {
                int iChuongTrinh = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_CHUONGTRINH chuongtrinh = _kiennghi.HienThiThongTinChuongTrinh(iChuongTrinh);
                ViewData["opt-daibieu"] = kn.OptionDaiBieu_By_ID_COQUAN((int)chuongtrinh.IDONVI, 0);
                ViewData["opt-diaphuong"] = kn.OptionDiaPhuong_By_ID_COQUAN((int)chuongtrinh.IDONVI, 0);
                ViewData["id"] = fc["id"];
                SetTokenAction("Chuongtrinh_chitiet_add", 0);
                return PartialView("../Ajax/Kiennghi/Chuongtrinh_chitiet_add");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form Thêm lịch tiếp đại biểu");
                throw;
            }

        }
        public ActionResult Ajax_Load_lichtiep_edit(int id, int iDoiTuong)
        {
            try
            {
                KN_CHUONGTRINH ct = _kiennghi.HienThiThongTinChuongTrinh(id);
                int idonvi = (int)ct.IDONVI;
                QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(idonvi);
                if (coquan == null) return null;
                var lichtiep = _kiennghi.PRC_CHUONGTRINH_CHITIET(id);
                int num = 0;
                StringBuilder str = new StringBuilder();
                if (lichtiep.Count() > 0)
                {
                    foreach (var l in lichtiep)
                    {
                        num++;
                        string xoa = "<span title='Xóa' onclick=\"$('#db_" + num + "').remove()\" class='btn btn-danger'><i class='icon-remove'></i></span>";
                        string ngaytiep = "";
                        if (l.NGAYTIEP != DateTime.MinValue) { ngaytiep = func.ConvertDateVN(l.NGAYTIEP.ToString()); }
                        str.Append("<tr id='db_" + num + "' class='db'>" +
                            "<td >" +
                            "<select class='input-block-level' name='iDaiBieu'>" +
                            kn.Option_ToDaiBieu((int)l.ID_TODAIBIEU, ID_PARENT_TODAIBIEU) +
                            "</select></td>" +
                            "<td ><select class='input-block-level'   name='iDiaPhuong' onchange='ChangeHuyenXa(this.value," + num + ")' >" +
                            "<option value='0'>Chọn địa phương</option>" +
                            kn.OptionDiaPhuong_By_ID_COQUAN(idonvi, (int)l.ID_DIAPHUONG)
                            + "</select></td>" +
                             "<td ><select class='input-block-level' id='iDiaPhuong_02_" + num + "' name='iDiaPhuong_02' >" +
                            "<option value='0'>Chọn địa phương</option>" +
                            kn.OptionDiaPhuong_By_Parent((int)l.ID_DIAPHUONG, (int)l.ID_DIAPHUONG2)
                            + "</select></td>" +
                            "<td ><input type='text' value='" + ngaytiep + "' class='datepick input-block-level' name='dNgayTiep' /></td>" +
                            "<td ><input type='text' value='" + Server.HtmlEncode(l.DIACHITIEP) + "' class='input-block-level' name='cDiaChi' /></td>" +
                            "<td >" + xoa + "</td></tr>");
                    }
                }
                ViewData["lichtiep"] = str;
                ViewData["iDoiTuong"] = iDoiTuong;
                ViewData["num"] = num;
                return PartialView("../Ajax/Kiennghi/Load_lichtiep_edit");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "");
                return null;
            }
        }
        public ActionResult Ajax_Chuongtrinh_chitiet_edit(FormCollection fc)
        {
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_CHUONGTRINH_CHITIET chitiet = _kiennghi.GetByID_ChuongTrinhChiTiet(id);
                int iChuongTrinh = Convert.ToInt32(chitiet.ICHUONGTRINH);
                KN_CHUONGTRINH chuongtrinh = _kiennghi.HienThiThongTinChuongTrinh(iChuongTrinh);
                ViewData["opt-daibieu"] = kn.OptionDaiBieu_By_ID_COQUAN((int)chuongtrinh.IDONVI, (int)chitiet.ITODAIBIEU);
                ViewData["opt-diaphuong"] = kn.OptionDiaPhuong_By_ID_COQUAN((int)chuongtrinh.IDONVI, (int)chitiet.IDIAPHUONG);
                ViewData["id"] = fc["id"];
                ViewData["chitiet"] = chitiet;
                SetTokenAction("Chuongtrinh_chitiet_edit", id);
                return PartialView("../Ajax/Kiennghi/Chuongtrinh_chitiet_edit");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form Thêm lịch tiếp đại biểu");
                throw;
            }

        }
        public ActionResult Chuongtrinh_chitiet()
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int iChuongTrinh = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                ChuongtrinhCuTri ct = kn.Chuongtring_detail(iChuongTrinh, Request.Cookies["url_key"].Value);
                KN_CHUONGTRINH chuongtrinh = _kiennghi.HienThiThongTinChuongTrinh(iChuongTrinh);
                var chitiet = _kiennghi.PRC_CHUONGTRINH_CHITIET(iChuongTrinh);
                //ViewData["btn-add"] = "<a href='javascript:void(0)' onclick=\"ShowPopUp('id=" + Request["id"] + "','/Kiennghi/Ajax_Chuongtrinh_chitiet_add')\"><i class==></i> Thêm mới</a>";
                ViewData["kn"] = chuongtrinh;
                ViewData["detail"] = ct;
                ViewData["list"] = kn.ChuongTring_ChiTiet(chitiet, Request.Cookies["url_key"].Value);
                ViewData["btn-add"] = "<a href='javascript:void(0)' onclick=\"ShowPopUp('id=" + Request["id"] + "','/Kiennghi/Ajax_Chuongtrinh_chitiet_add')\"><i class='icon-plus'></i> Thêm mới</a>";
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách chi tiết chương trình tiếp xúc cử tri");
                return View("../Home/Error_Exception");
            }

        }
        public string Get_CheckBox_TruocKyHop(int iTruocKyHop = -1)
        {

            string str = "";
            if (iTruocKyHop == -1)//chưa chọn, mới cập nhật
            {
                if (Request.Cookies["truockyhop"] != null)//chưa có cookie trước đó
                {
                    iTruocKyHop = Convert.ToInt32(Request.Cookies["truockyhop"].Value);
                }
                else// set mặc định là trước kỳ họp
                {
                    iTruocKyHop = 1;
                }
            }
            var hinhthuc = new List<TruocKyHop>
            {
                new TruocKyHop { ten = "Trước kỳ họp", value = 1, class_span = "span5" },
                new TruocKyHop { ten = "Sau kỳ họp", value = 0, class_span = "span4" },
                new TruocKyHop { ten = "Khác", value = 2, class_span = "span3" }
            };
            foreach (var h in hinhthuc)
            {
                string check = "";
                if (h.value == iTruocKyHop) { check = " checked "; }
                str += "<span class='" + h.class_span + "'><input class='nomargin' type='radio' id='iTruocKyHop' name='iTruocKyHop' " + check + " id='h_" + h.class_span + "' value='" + h.value + "' /> " +
                     "<label for='h_" + h.class_span + "'>" + h.ten + "</label> </span>";
            }
            return str;
        }
        public string Get_CheckBox_TruocSau_KyHop(int iTruocKyHop = -1)
        {

            string str = "";
            if (iTruocKyHop == -1)//chưa chọn, mới cập nhật
            {
                if (Request.Cookies["truockyhop"] != null)//chưa có cookie trước đó
                {
                    iTruocKyHop = Convert.ToInt32(Request.Cookies["truockyhop"].Value);
                }
                else// set mặc định là trước kỳ họp
                {
                    iTruocKyHop = 1;
                }
            }
            var hinhthuc = new List<TruocKyHop>
            {
                new TruocKyHop { ten = "Trước kỳ họp", value = 1, class_span = "span5" },
                new TruocKyHop { ten = "Sau kỳ họp", value = 0, class_span = "span4" },
            };
            foreach (var h in hinhthuc)
            {
                string check = "";
                if (h.value == iTruocKyHop) { check = " checked "; }
                str += "<span class='" + h.class_span + "'><input class='nomargin' type='radio' id='iTruocKyHop' name='iTruocKyHop' " + check + " id='h_" + h.class_span + "' value='" + h.value + "' /> " +
                     "<label for='h_" + h.class_span + "'>" + h.ten + "</label> </span>";
            }
            return str;
        }
        public ActionResult Chuongtrinh()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                if (!base_business.Action_(2, u_info.tk_action))
                {
                    Response.Redirect("/Home/Error/");
                }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                int iKyHop = ID_KyHop_HienTai(); if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                int iUser = (int)u_info.user_login.IUSER;
                int iDonVi = (int)u_info.user_login.IDONVI;

                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                if (Request["iDoan"] != null)
                {
                    iDonVi = Convert.ToInt32(Request["iDoan"]);
                }
                ViewData["is_dbqh"] = 1;
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_dbqh"] = 0;
                    ViewData["opt-coquan"] = Get_Option_DoanDaiBieu(iDonVi);
                }
                KN_CHUONGTRINH kn_ct = new KN_CHUONGTRINH();
                kn_ct.IUSER = iUser;
                kn_ct.IKYHOP = iKyHop;
                string listKyHop = "0";
                ViewData["dbqh"] = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                if (Request["q"] != null) { kn_ct.CNOIDUNG = func.RemoveTagInput(Request["q"]); }
                if (Request["cNoiDung"] != null) { kn_ct.CNOIDUNG = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iTruocKyHop"] != null) { kn_ct.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]); }
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                List<PRC_CHUONGTRINH_TXCT> chuongtrinh = _kiennghi.PRC_CHUONGTRINH_TIEPXUC_CUTRI(kn_ct, listKyHop, page, post_per_page);
                if (chuongtrinh.Count() > 0)
                {
                    if (Request["dBatDau"] != null && Request["dBatDau"].ToString().Trim() != "")
                    {
                        DateTime batdau = Convert.ToDateTime(func.ConvertDateToSql(Request["dBatDau"]));
                        chuongtrinh = chuongtrinh.Where(x => x.DBATDAU >= batdau).ToList();
                    }
                    if (Request["dKetThuc"] != null && Request["dKetThuc"].ToString().Trim() != "")
                    {
                        DateTime ketthuc = Convert.ToDateTime(func.ConvertDateToSql(Request["dKetThuc"]));
                        chuongtrinh = chuongtrinh.Where(x => x.DBATDAU <= ketthuc).ToList();
                    }
                }
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if(chuongtrinh.Count() > 0)
                {
                    htmlList = kn.List_ChuongTrinh(chuongtrinh, u_info.tk_action);
                    htmlPhanTrang = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)chuongtrinh.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
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

                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách chương trình tiếp xúc cử tri");
                return View("../Home/Error_Exception");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        //public ActionResult Ajax_search_chuongtrinh_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        UserInfor u_info = GetUserInfor();
        //        KN_CHUONGTRINH chuongtrinh = new KN_CHUONGTRINH();
        //        if (fc["q"] != null)
        //        {
        //            chuongtrinh.CNOIDUNG = func.RemoveTagInput(fc["q"]);                    
        //        }
        //        if (fc["cNoiDung"] != null)
        //        {
        //            chuongtrinh.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
        //        }
        //        if (fc["iKyHop"] != null) { chuongtrinh.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        if (fc["iTruocKyHop"] != null) { chuongtrinh.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        if (fc["iDoan"] != null) {
        //            chuongtrinh.IDONVI = Convert.ToInt32(fc["iDoan"]);
        //        }
        //        if (!u_info.tk_action.is_lanhdao)
        //        {
        //            chuongtrinh.IDONVI = (int)u_info.user_login.IDONVI;
        //        }                
        //        var ct = _kiennghi.PRC_CHUONGTRINH_TIEPXUC_CUTRI(chuongtrinh,1,9999);
        //        if (ct.Count() > 0)
        //        {
        //            if (fc["dBatDau"] != null && fc["dBatDau"].ToString().Trim() != "") {
        //                DateTime batdau= Convert.ToDateTime(func.ConvertDateToSql(fc["dBatDau"]));
        //                ct = ct.Where(x => x.DBATDAU >= batdau).ToList();
        //            }
        //            if (fc["dKetThuc"] != null && fc["dKetThuc"].ToString().Trim() != "") {
        //                DateTime ketthuc = Convert.ToDateTime(func.ConvertDateToSql(fc["dKetThuc"]));
        //                ct = ct.Where(x => x.DBATDAU <= ketthuc).ToList();
        //            }
        //        }    
        //        Response.Write(kn.List_ChuongTrinh(ct, u_info.tk_action,"search"));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {

        //        log.Log_Error(ex, "Tìm kiếm chương trình tiếp xúc cử tri");
        //        throw;
        //    }

        //}
        /*
        public ActionResult Chuongtrinh()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u_info = GetUserInfor();
            func.SetCookies("url_return", Request.Url.AbsoluteUri);
            int iKyHop = ID_KyHop_HienTai();
            int iUser = (int)u_info.user_login.IUSER;
            int iDonVi = (int)u_info.user_login.IDONVI;
            if (u_info.tk_action.is_lanhdao) { iDonVi = 0; }
            if (Request["iDonVi"] != null)
            {
                iDonVi = Convert.ToInt32(Request["iDonVi"]);
            }
            if (Request["iKyHop"] != null)
            {
                iKyHop = Convert.ToInt32(Request["iKyHop"]);
                ViewData["title"] = "Kết quả tìm kiếm";
            }
            else
            {
                ViewData["title"] = "Chương trình tiếp xúc cử tri: " + _base.GetName_KyHop_KhoaHop(iKyHop).Replace("</br>", " - ");
            }

            string dBatDau = ""; if (Request["dBatDau"] != null) { dBatDau = Request["dBatDau"]; }
            string dKetThuc = ""; if (Request["dKetThuc"] != null) { dKetThuc = Request["dKetThuc"]; }
            string cKeHoach = ""; if (Request["cKeHoach"] != null) { cKeHoach = func.RemoveTagInput(Request["cKeHoach"]); }
            int iDiaPhuong_0 = 0; if (Request["iDiaPhuong_0"] != null) { iDiaPhuong_0 = Convert.ToInt32(Request["iDiaPhuong_0"]); }
            int iDiaPhuong_1 = 0; if (Request["iDiaPhuong_1"] != null) { iDiaPhuong_1 = Convert.ToInt32(Request["iDiaPhuong_1"]); }
            int iDoan = 0; if (Request["iDoan"] != null) { iDoan = Convert.ToInt32(Request["iDoan"]); }
            int iTruocKyHop = -1; if (Request["iTruocKyHop"] != null) { iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]); }
            string cNoiDung = ""; if (Request["cNoiDung"] != null) { cNoiDung = func.RemoveTagInput(Request["cNoiDung"]); }
            string daibieu = ""; if (Request["daibieu"] != null) { daibieu = func.RemoveTagInput(Request["daibieu"]); }
            string sql = "";
            if (iDiaPhuong_1 == 0 && iDoan == 0 && iDiaPhuong_0 == 0)
            {

                sql = "select distinct ICHUONGTRINH from KN_CHUONGTRINH where IKYHOP=" + iKyHop;
            }
            else
            {
                sql = "select distinct KN_CHUONGTRINH.ICHUONGTRINH from KN_CHUONGTRINH";
                if (iDiaPhuong_0 != 0)
                {

                    sql += " inner join KN_CHUONGTRINH_DIAPHUONG on KN_CHUONGTRINH.ICHUONGTRINH=KN_CHUONGTRINH_DIAPHUONG.ICHUONGTRINH " +
                        " and KN_CHUONGTRINH_DIAPHUONG.IDIAPHUONG0=" + iDiaPhuong_0;
                    if (iDiaPhuong_1 != 0)
                    {
                        sql += " and KN_CHUONGTRINH_DIAPHUONG.IDIAPHUONG1=" + iDiaPhuong_1;
                    }
                }
                if (iDoan != 0)
                {
                    sql += " inner join KN_CHUONGTRINH_DAIBIEU on KN_CHUONGTRINH.ICHUONGTRINH=KN_CHUONGTRINH_DAIBIEU.ICHUONGTRINH " +
                        " and KN_CHUONGTRINH_DAIBIEU.IUSER_COQUAN=" + iDoan;
                    if (daibieu != "")
                    {
                        sql += " and KN_CHUONGTRINH_DAIBIEU.IUSER_COQUAN IN(" + daibieu + "-1)";
                    }
                }
                sql += " and KN_CHUONGTRINH.IKYHOP=" + iKyHop;
            }
            if (iDonVi != 0) { sql += " and KN_CHUONGTRINH.IDONVI=" + iDonVi + ""; }
            if (dBatDau != "") { sql += " and KN_CHUONGTRINH.DBATDAU>='" + func.ConvertDateToSql(dBatDau) + "'"; }
            if (dKetThuc != "") { sql += " and KN_CHUONGTRINH.DKETTHUC<='" + func.ConvertDateToSql(dKetThuc) + "'"; }
            if (cKeHoach != "") { sql += " and KN_CHUONGTRINH.CKEHOACH like N'%" + cKeHoach + "%'"; }
            if (cNoiDung != "") { sql += " and KN_CHUONGTRINH.CNOIDUNG like N'%" + cNoiDung + "%'"; }
            if (iTruocKyHop != -1) { sql += " and KN_CHUONGTRINH.ITRUOCKYHOP=" + iTruocKyHop; }
            ViewData["chuongtrinh"] = kn.ChuongTrinh(u_info.tk_action, sql);

            return View();
        }
        */
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
                str = "<div class='input-block-level'><span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_TrungUong'  name='iThamQuyen' value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "'>" +
                    " <label for='iThamQuyen_TrungUong'>" + "Trung Ương" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Tinh' name='iThamQuyen'  checked value='" + (int)ThamQuyen_DiaPhuong.Tinh + "'> " +
                     "<label for='iThamQuyen_Tinh'>" + "Tỉnh" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Huyen' name='iThamQuyen'  value='" + (int)ThamQuyen_DiaPhuong.Huyen + "'>" +
                     " <label for='iThamQuyen_Huyen'>" + "Huyện" + "</label></span>" +
                     "</div>";
            }
            else
            {
                str = "<div class='input-block-level'><span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_TrungUong' name='iThamQuyen' value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "'>" +
                    " <label for='iThamQuyen_TrungUong'>" + "Trung Ương" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Tinh' name='iThamQuyen' value='" + (int)ThamQuyen_DiaPhuong.Tinh + "'> " +
                     "<label for='iThamQuyen_Tinh'>" + "Tỉnh" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Huyen' name='iThamQuyen' checked value='" + (int)ThamQuyen_DiaPhuong.Huyen + "'>" +
                     " <label for='iThamQuyen_Huyen'>" + "Huyện" + "</label></span>" +
                     "</div>";
            }


            return str;
        }
        
        public string GetRadioButton_ThamQuyen_KienNghi_ChuyenDonVi(int id_thamquyen)
        {
            string str = "";
            if (id_thamquyen == 0 || id_thamquyen == (int)ThamQuyen_DiaPhuong.Trunguong)
            {
                str = "<div class='input-block-level'><span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_TrungUong' name='iThamQuyen' checked value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "'>" +
                    " <label for='iThamQuyen_TrungUong'>" + "Trung Ương" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Tinh' name='iThamQuyen' disabled value='" + (int)ThamQuyen_DiaPhuong.Tinh + "'> " +
                     "<label for='iThamQuyen_Tinh'>" + "Tỉnh" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Huyen' name='iThamQuyen'disabled value='" + (int)ThamQuyen_DiaPhuong.Huyen + "'>" +
                     " <label for='iThamQuyen_Huyen'>" + "Huyện" + "</label></span>" +
                     "</div>";
            }
            else if (id_thamquyen == (int)ThamQuyen_DiaPhuong.Tinh)
            {
                str = "<div class='input-block-level'><span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_TrungUong' disabled name='iThamQuyen' value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "'>" +
                    " <label for='iThamQuyen_TrungUong'>" + "Trung Ương" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Tinh' name='iThamQuyen'  checked value='" + (int)ThamQuyen_DiaPhuong.Tinh + "'> " +
                     "<label for='iThamQuyen_Tinh'>" + "Tỉnh" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Huyen' name='iThamQuyen' disabled  value='" + (int)ThamQuyen_DiaPhuong.Huyen + "'>" +
                     " <label for='iThamQuyen_Huyen'>" + "Huyện" + "</label></span>" +
                     "</div>";
            }
            else
            {
                str = "<div class='input-block-level'><span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_TrungUong' name='iThamQuyen' disabled value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "'>" +
                    " <label for='iThamQuyen_TrungUong'>" + "Trung Ương" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Tinh' name='iThamQuyen' disabled value='" + (int)ThamQuyen_DiaPhuong.Tinh + "'> " +
                     "<label for='iThamQuyen_Tinh'>" + "Tỉnh" + "</label></span>" +
                     "<span class='span4'><input class='nomargin' onclick=\"DoiThamQuyenDonVi(this.value)\" type='radio' id='iThamQuyen_Huyen' name='iThamQuyen' checked value='" + (int)ThamQuyen_DiaPhuong.Huyen + "'>" +
                     " <label for='iThamQuyen_Huyen'>" + "Huyện" + "</label></span>" +
                     "</div>";
            }


            return str;
        }
        public JsonResult Ajax_Get_Coquan_diaphuong_child(int id)
        {
            QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(id);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IHIENTHI", 1);
            dic.Add("IDELETE", 0);
            dic.Add("IPARENT", id);
            var list_coquan = _kiennghi.HienThiDanhSachCoQuan(dic);

            return Json(new { list_coquan }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Ajax_Get_Coquan_diaphuong_child_(int id)
        {
            //QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(id);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IHIENTHI", 1);
            dic.Add("IDELETE", 0);
            //dic.Add("IDIAPHUONG", coquan.IDIAPHUONG);
            dic.Add("IPARENT", id);
            var list_coquan = _kiennghi.HienThiDanhSachCoQuan(dic);
            string option = kn.OptionCoQuanXuLy(list_coquan, id, 0, 0, 0);
            Response.Write(option);
            return null;
        }
        public JsonResult Ajax_Get_ThamQuyen_DonVi(int val)
        {
            string str = Get_Option_DonViThamQuyen_ByCType(val, 0);
            Response.Write(str);
            return null;
        }
        public ActionResult Themmoi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            UserInfor u_info = GetUserInfor();
            if (!base_business.Action_(3, u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                SetTokenAction("kn_themkiennghi", 0);
                int iKyHop = ID_KyHop_HienTai();
                //ViewData["opt-kyhop"] = _base.Option_QuocHoi_Khoa_KyHop();          

                int iDonVi = ID_Coquan_doandaibieu;
                ViewData["opt-donvi-guiden"] = Get_Option_DonVi_GuiDen();
                ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(0);
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_dbqh"] = "0";
                    ViewData["opt-donvithamquyen-diaphuong-parent"] = Get_Option_ThamQuyen_DiaPhuong_Parent(0);
                    //ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen(ID_Ban_DanNguyen);
                    ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan(ID_Ban_DanNguyen);

                }
                else
                {
                    ViewData["is_dbqh"] = "1";
                    ViewData["opt-donvithamquyen-diaphuong"] = Get_Option_ThamQuyen_DiaPhuong(0);
                    ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();

                }
                //var chuongtrinh = _kiennghi.GetAll_ChuongTrinh(iDonVi, iKyHop);
                //Logic cũ là sẽ hiển thị chương trình theo kỳ họp chọn trên màn, logic mới là hiển thị chương trình trước rồi điều chỉnh các lựa chọn theo chương trình
                var chuongtrinh = _kiennghi.GetAll_ChuongTrinh(0, 0);

                if (chuongtrinh.Count > 0)
                {
                    ViewData["opt-chuongtrinhTXCT"] = kn.Option_ChuongTrinh_ByKyHop_And_UserType(chuongtrinh.OrderByDescending(x => x.DDATE).ToList(), u_info);
                }

                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                //Lựa chọn mặc định cho thẩm quyền là Trung Ương
                ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong);

                //ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi(nguonkiennghi, 0);
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi);

                ViewData["opt-doandaibieu"] = Get_Option_DonViLap();
                ViewData["opt-linhvuc"] = GetAll_Option_LinhVucSorted();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                ViewData["opt-tinh"] = Get_Option_DiaPhuong0(AppConfig.IDIAPHUONG);
                ViewData["opt-huyen"] = Get_Option_DiaPhuong1(AppConfig.IDIAPHUONG, 0);
                ViewData["so-kiennghi"] = Get_Auto_SoKienNghi(0);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form thêm mới kiến nghị cử tri");
                return View("../Home/Error_Exception");
            }

        }

        public string List_CheckBox_KyHop(int iDaibieu = 0, string List_Check_KyHop = "")
        {

            UserInfor u_info = GetUserInfor();
            string str = "";
            var listKhoa = _thietlap.Get_List_Quochoi_Khoa().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                listKhoa = listKhoa.Where(x => x.ILOAI == 1).ToList();
            if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                listKhoa = listKhoa.Where(x => x.ILOAI == 0).ToList();
            List<QUOCHOI_KYHOP> listQH_Kyhop = _thietlap.Get_List_Quochoi_Kyhop();
            var strArray = List_Check_KyHop.Split(',');
            foreach (var khoa in listKhoa)
            {
                var listKyhop = listQH_Kyhop.Where(v => v.IKHOA == (int)khoa.IKHOA && khoa.IDELETE == 0).OrderBy(v => v.IVITRI).ToList();
                if (listKyhop != null && listKyhop.Count() > 0)
                {
                    str += "<optgroup label ='" + khoa.CTEN + "'>" + "</optgroup>";
                    foreach (var kyhop in listKyhop)
                    {
                        string selected = "";
                        if (strArray.Contains(kyhop.IKYHOP.ToString()))
                            selected = "selected ";
                        str += "<option " + selected + "value ='" + kyhop.IKYHOP + "'>" + kyhop.CTEN + "</option>";

                    }
                }

            }
            return str;
        }

        public string Get_Auto_SoKienNghi(int? iDoiTuongGui = null)
        {
            string makiennghi = "";
            string DateNow = DateTime.Now.ToString("yyyy") + "";
            if (iDoiTuongGui.HasValue)
            {
                if (iDoiTuongGui.Value == 1)
                {
                    makiennghi += "HDND" + DateNow;
                }
                else if (iDoiTuongGui.Value == 0)
                {
                    makiennghi += "QH" + DateNow;
                }
            }
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
          
            var firstkn = _kiennghi.HienThiDanhSachKienNghi(dictionary).Where(x=> x.CMAKIENNGHI != "" && x.CMAKIENNGHI != null && x.CMAKIENNGHI.Contains(makiennghi)).OrderByDescending(x => x.DDATE).FirstOrDefault();
            string makn = "";
            if (firstkn != null) makn = firstkn.CMAKIENNGHI.Remove(0, makiennghi.Length);
            if(makn.Length != 4)
            {
                makn = "0000";
            } else
            {
                makn = (makn.ToInt32OrDefault() + 1).ToString("D4");
            }

            return makiennghi+ makn;
        }

        [HttpPost]
        public JsonResult Ajax_GetOpt_KyHopKhoaHop(int iKyHop = 0, int? iDoiTuongGui = null)
        {
            var optListKyHopKhoaHop = Get_Option_KyHop(iKyHop, iDoiTuongGui);
            return Json(optListKyHopKhoaHop, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ajax_GetOpt_SoKienNghi(int? iDoiTuongGui = null)
        {
            try
            {
                var SoKienNghi = Get_Auto_SoKienNghi(iDoiTuongGui);
                var optSoKienNghi = "<input type='text' id='cSoKiennghi' name='cSoKiennghi' value='" + SoKienNghi + "' class='input-block-level' readonly='true' />";
                Response.Write(optSoKienNghi);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "So kien nghi.");
                return null;
            }
            return null;
        }

        public ActionResult Ajax_Change_Tinhthanh(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                Response.Write(Get_Option_DiaPhuong1(id));
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form thêm mới kiến nghị cử tri");
                return null;
            }
        }
        public string Get_Option_DiaPhuong0(int idiaphuong = 0)
        {
            string str = "<option  value='0'>Chọn tỉnh thành</option>";
            var diaphuong = _kiennghi.GetAll_DiaPhuong().Where(x => x.IPARENT == 0 && x.IHIENTHI == 1).OrderBy(x => x.CTEN).ToList();
            // diaphuong = diaphuong;
            if (idiaphuong == 0)//thêm mới -> load địa phương gắn vs đơn vị của tài khoản
            {
                UserInfor u = GetUserInfor();
                int iDonVi_User = (int)u.user_login.IDONVI;
                QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(iDonVi_User);
                if (coquan.IDIAPHUONG != null && coquan.IDIAPHUONG != 0)
                {
                    idiaphuong = (int)coquan.IDIAPHUONG;
                }
            }
            foreach (var d in diaphuong)
            {
                string select = ""; if (d.IDIAPHUONG == idiaphuong) { select = " selected "; }
                str += "<option " + select + " value='" + d.IDIAPHUONG + "'>" + Server.HtmlEncode(d.CTEN) + "</option>";
            }
            return str;
        }
        public string Get_Option_DiaPhuong1(int iparent = 0, int idiaphuong = 0)
        {
            string str = "<option  value='0'>Chọn huyện/thành phố/thị xã</option>";
            if (iparent == 0)//thêm mới -> load địa phương gắn vs đơn vị của tài khoản
            {
                UserInfor u = GetUserInfor();
                int iDonVi_User = (int)u.user_login.IDONVI;
                QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(iDonVi_User);
                if (coquan.IDIAPHUONG != null && coquan.IDIAPHUONG != 0)
                {
                    iparent = (int)coquan.IDIAPHUONG;
                }
            }
            if (iparent == 0) { return str; }
            var diaphuong = _kiennghi.GetAll_DiaPhuong().Where(x => x.IHIENTHI == 1 && x.IPARENT == iparent).OrderBy(x => x.CTEN).ToList();
            foreach (var d in diaphuong)
            {
                string select = ""; if (d.IDIAPHUONG == idiaphuong) { select = " selected "; }
                str += "<option " + select + " value='" + d.IDIAPHUONG + "'>" + d.CTYPE + " " + Server.HtmlEncode(d.CTEN) + "</option>";
            }
            return str;
        }
        public string CreateMaKienNghi(int iDonViTiepNhan, int iKyHop)
        {
            try
            {
                string str = "";
                //QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(iDonViTiepNhan);
                //if (coquan.CCODE == null) {
                //    coquan.CCODE = kn.VietTat_CoQuan(coquan.CTEN);
                //    _kiennghi.UpdateCoQuan(coquan);
                //}
                //Dictionary<string, object> dic = new Dictionary<string, object>();
                //dic.Add("IKYHOP", iKyHop); dic.Add("IDONVITIEPNHAN", iDonViTiepNhan);
                //var kiennghi = _kiennghi.HienThiDanhSachKienNghi(dic);
                //int iSoKienNghi = kiennghi.Count() + 1;
                //str = coquan.CCODE + "_" + "_MAKYHOP_" + iSoKienNghi;
                return str;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tạo mã kiến nghị");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoi(FormCollection fc, HttpPostedFileBase file)
        {
            //if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            //if (!CheckTokenAction("kn_themkiennghi"))
            //{
            //    Response.Redirect("/Home/Error/");
            //    return null;
            //}
            UserInfor u_info = GetUserInfor();
            try
            {
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }

                int iUser = u_info.tk_action.iUser;
                int ThamQuyenDonVi = 0;
                ThamQuyenDonVi = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                string tukhoa = func.RemoveTagInput(fc["cTuKhoa"]);
                int IDONVITIEPNHAN = Convert.ToInt32(fc["iDonViTiepNhan"]);
                int iKyHop = Convert.ToInt32(fc["iKyHop"]);
                string ma_kiennghi = CreateMaKienNghi(IDONVITIEPNHAN, iKyHop);
                KN_KIENNGHI kn = new KN_KIENNGHI();
                kn.IDOITUONGGUI = Convert.ToInt32(fc["iDoiTuongGui"]);
                if (u_info.user_login.ITYPE == (decimal)UserType.ChuyenVienHDND)
                {
                    kn.IDOITUONGGUI = (decimal)Loai_DoiTuong.HDND;
                }
                if (u_info.user_login.ITYPE == (decimal)UserType.ChuyenVienDBQH)
                {
                    kn.IDOITUONGGUI = (decimal)Loai_DoiTuong.DBQH;
                }
                kn.IKYHOP = iKyHop;
                kn.IDIAPHUONG0 = Convert.ToInt32(fc["iDiaPhuong0"]);
                kn.IDIAPHUONG1 = Convert.ToInt32(fc["iDiaPhuong1"]);
                kn.CDIACHI = func.RemoveTagInput(fc["cDiaChi"]);
                //kn.INGUONKIENNGHI = Convert.ToInt32(fc["iNguonKienNghi"]);
                string lstNguonKN = "";
                if (Request["lstNguonKN"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    lstNguonKN = Request["lstNguonKN"];
                }

                kn.ID_IMPORT = 0;
                kn.ID_GOP = 0;
                kn.IDELETE = 0;
                kn.CMAKIENNGHI = func.RemoveTagInput(fc["cSoKiennghi"]);
                kn.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                kn.CTUKHOA = tukhoa;
                kn.DDATE = DateTime.Now;
                kn.ICHUONGTRINH = Convert.ToInt32(fc["iChuongTrinhTXCT"]);
                kn.IDONVITIEPNHAN = IDONVITIEPNHAN;
                kn.IKIEMTRATRUNG = 0;
                kn.IKIENNGHI_TRUNG = 0;
                kn.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                kn.CGHICHU = func.RemoveTagInput(fc["cGhiChu"]);
                kn.ITHAMQUYENDONVI = ThamQuyenDonVi;
                kn.ITINHTRANG = (decimal)TrangThaiKienNghi.Moicapnhat;
                kn.ITONGHOP = 0;
                kn.IPARENT = 0;
                kn.ITONGHOP_BDN = 0;
                kn.ID_KIENNGHI_PARENT = 0;

                kn.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]);
                func.SetCookies("truockyhop", fc["iTruocKyHop"]);
                kn.IUSER = iUser;
                _kiennghi.InsertKienNghi(kn);
                int iKienNghi = (int)kn.IKIENNGHI;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_kiennghi";
                        f.CFILE = UploadFile(file);
                        f.ID = iKienNghi;
                        kntc.Upload_file(f);
                    }

                }
                string[] arrLSKN = lstNguonKN.Split(',');
                for (int i = 0; i < arrLSKN.Count(); i++)
                {
                    KIENNGHI_NGUONDON kn_nd = new KIENNGHI_NGUONDON();
                    kn_nd.IKIENNGHI = iKienNghi;
                    kn_nd.INGUONDON = Convert.ToDecimal(arrLSKN[i]);
                    _kiennghi.InsertKienNghi_NguonDon(kn_nd);
                }
                _kiennghi.Tracking_KN(iUser, iKienNghi, "Thêm mới kiến nghị");
                _kiennghi.Update_TuKhoa(tukhoa);
                //Tracking_kiennghi(iUser, (int)kn.IKIENNGHI, "Thêm mới kiến nghị");
                string id_ecr = HashUtil.Encode_ID(iKienNghi.ToString(), Request.Cookies["url_key"].Value);
                Response.Redirect("/Kiennghi/Kiemtrung/?id=" + id_ecr + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Insert kiến nghị cử tri");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
            if (!CheckTokenAction("kn_suakiennghi", id))
            {
                Response.Redirect("/Home/Error/"); return null;
            }
            try
            {
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                var user = GetUserInfor();
                int iUser = user.tk_action.iUser;
                //int ThamQuyenDonVi = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                //if (GetUserInfor().tk_action.is_dbqh)// tk đoàn ĐBQH
                //{
                //    if (Convert.ToInt32(fc["iThamQuyen"]) == 2)
                //    {
                //        ThamQuyenDonVi = Convert.ToInt32(fc["iThamQuyenDonVi_DiaPhuong"]);
                //    }
                //}
                int ThamQuyenDonVi = 0;
                //if (GetUserInfor().tk_action.is_dbqh)// tk đoàn ĐBQH
                //{
                //    if (Convert.ToInt32(fc["iThamQuyen"]) == 2)
                //    {
                //        ThamQuyenDonVi = Convert.ToInt32(fc["iThamQuyenDonVi_DiaPhuong"]);
                //    }
                //}
                //if (Convert.ToInt32(fc["iThamQuyen"]) == 2)
                //{
                //    ThamQuyenDonVi = ID_Ban_DanNguyen_New;
                //}
                //else
                //{
                ThamQuyenDonVi = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                //}
                KN_KIENNGHI kn = _kiennghi.HienThiThongTinKienNghi(id);
                kn.IDOITUONGGUI = Convert.ToInt32(fc["iDoiTuongGui"]);
                if (user.user_login.ITYPE == (decimal)UserType.ChuyenVienHDND)
                {
                    kn.IDOITUONGGUI = (decimal)Loai_DoiTuong.HDND;
                }
                if (user.user_login.ITYPE == (decimal)UserType.ChuyenVienDBQH)
                {
                    kn.IDOITUONGGUI = (decimal)Loai_DoiTuong.DBQH;
                }
                kn.IKYHOP = Convert.ToInt32(fc["iKyHop"]);
                //string noidung_decode = Server.HtmlDecode(fc["cNoiDung"]);
                kn.CNOIDUNG = func.RemoveTagInput(Server.HtmlDecode(fc["cNoiDung"]));
                kn.CTUKHOA = func.RemoveTagInput(fc["cTuKhoa"]);
                //kn.DDATE = DateTime.Now;
                kn.IDIAPHUONG0 = Convert.ToInt32(fc["iDiaPhuong0"]);
                kn.IDIAPHUONG1 = Convert.ToInt32(fc["iDiaPhuong1"]);
                kn.CMAKIENNGHI = func.RemoveTagInput(fc["cSoKiennghi"]);
                kn.CDIACHI = func.RemoveTagInput(fc["cDiaChi"]);
                kn.ICHUONGTRINH = Convert.ToInt32(fc["iChuongTrinhTXCT"]);
                kn.IDONVITIEPNHAN = Convert.ToInt32(fc["iDonViTiepNhan"]);
                kn.INGUONKIENNGHI = Convert.ToInt32(fc["iNguonKienNghi"]);
                kn.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                kn.ITHAMQUYENDONVI = ThamQuyenDonVi;
                kn.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]);
                kn.CGHICHU = func.RemoveTagInput(fc["cGhiChu"]);
                //kn.IUSER = iUser;
                _kiennghi.UpdateThongTinKienNghi(kn);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_kiennghi";
                        f.CFILE = UploadFile(file);
                        f.ID = id;
                        kntc.Upload_file(f);
                    }

                }

                string lstNguonKN = "";
                if (Request["lstNguonKN"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    lstNguonKN = Request["lstNguonKN"];
                }

                _kiennghi.DeleteKNNguonDon(id);

                string[] arrLSKN = lstNguonKN.Split(',');
                for (int i = 0; i < arrLSKN.Count(); i++)
                {
                    KIENNGHI_NGUONDON kn_nd = new KIENNGHI_NGUONDON();
                    kn_nd.IKIENNGHI = id;
                    kn_nd.INGUONDON = Convert.ToDecimal(arrLSKN[i]);
                    _kiennghi.InsertKienNghi_NguonDon(kn_nd);
                }

                _kiennghi.Update_TuKhoa(kn.CTUKHOA);
                _kiennghi.Tracking_KN(iUser, id, "Sửa kiến nghị");
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Sửa kiến nghị");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult Ajax_KienNghi_trung_update(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iUser = GetUserInfor().tk_action.iUser;
                int id_check = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int id_trung = Convert.ToInt32(HashUtil.Decode_ID(fc["id_trung"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI don = _kiennghi.HienThiThongTinKienNghi(id_check);
                if (don.IKIENNGHI_TRUNG == id_trung)
                {
                    don.IKIENNGHI_TRUNG = 0;
                    don.IKIEMTRATRUNG = 0;
                    kntc.Tracking_KNTC(iUser, id_check, "Bỏ chọn kiến nghị trùng");
                    _kiennghi.UpdateThongTinKienNghi(don);
                    Response.Write(0);
                }
                else
                {
                    don.IKIENNGHI_TRUNG = id_trung;
                    don.IKIEMTRATRUNG = 1;
                    kntc.Tracking_KNTC(iUser, id_check, "Chọn kiến nghị trùng");
                    _kiennghi.UpdateThongTinKienNghi(don);
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Chọn/Bỏ chọn kiến nghị trùng");
                throw;
            }

        }

        public ActionResult Ajax_KienNghi_theodoi(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iUser = GetUserInfor().tk_action.iUser;

                int id_check = Convert.ToInt32(Session[iUser + "Sessesion_KiemTrung"].ToString());
                KN_KIENNGHI don = _kiennghi.HienThiThongTinKienNghi(id_check);
                if (don.IKIEMTRATRUNG == 0)
                {
                    Response.Write(0);
                }
                else if (don.IKIEMTRATRUNG == 1)
                {
                    don.ITINHTRANG = 7;
                    _kiennghi.UpdateThongTinKienNghi(don);
                    Response.Write(1);
                }

                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Chọn/Bỏ chọn kiến nghị trùng");
                throw;
            }

        }
        public ActionResult Kiemtrung()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int iUser = GetUserInfor().tk_action.iUser;
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("4,3", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                Session[iUser + "Sessesion_KiemTrung"] = id;
                //Check_Exist_ID_KienNghi(id);
                ViewData["id"] = Request["id"];
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["kiennghi"] = kiennghi;
                //if (kiennghi.CNOIDUNG.IndexOf('.') != -1)
                //{
                //    string[] noidung_kiemtra_split = kiennghi.CNOIDUNG.Split('.');
                //    for (int i = 0; i < 5; i++)
                //    {
                //        int j = i + 1;
                //        string name_pram = "p_CNOIDUNG" + j;
                //        string val = "";
                //        if (i< noidung_kiemtra_split.Length)
                //        {
                //            val = noidung_kiemtra_split[i].Trim();
                //        }                        
                //        Response.Write("</br>"+i+"."+val);
                //    }
                //}
                var kn_trung = _kiennghi.PRC_KIENNGHI_TRUNG(kiennghi.CNOIDUNG);
                kn_trung = kn_trung.Where(x => x.ID_KIENNGHI != id).ToList();
                //var kn_cungkyhop = kn_trung.Where(x => x.ID_KYHOP_TIEPNHAN == kiennghi.IKYHOP).ToList();
                //var kn_khackyhop = kn_trung.Where(x => x.ID_KYHOP_TIEPNHAN != kiennghi.IKYHOP).ToList();
                //danh sách kiến nghị trùng
                //Dictionary<string, object> _dic = new Dictionary<string, object>();
                //_dic.Add("IDONVITIEPNHAN", (int)u_info.user_login.IDONVI);
                //List<KN_KIENNGHI> all_kiennghi = _kiennghi.GetAll_KienNghi_Trung_Doan(kiennghi).Where(x => x.IKIENNGHI != id).ToList();
                ViewData["kyhop"] = kn.List_KienNghiTrung(kn_trung, kiennghi, 1, Request.Cookies["url_key"].Value);
                ViewData["kytruoc"] = kn.List_KienNghiTrung(kn_trung, kiennghi, 0, Request.Cookies["url_key"].Value);

                string btn_luu = "", trangthai_kiemtra = "";
                if (kiennghi.IKIENNGHI_TRUNG == 0)
                {
                    trangthai_kiemtra = " style='display:none' ";
                }
                btn_luu = " <a href='javascript:void();' " + trangthai_kiemtra + " class='btn btn-primary btn-dong' onclick=\"ShowPopUp('id=" +
                    Request["id"].ToString() + "','/Kiennghi/Ajax_Kiennghi_trung_dong')\">Lưu theo dõi, đóng kiến nghị</a> ";
                ViewData["btn_luu"] = btn_luu;
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách kiến nghị trùng để kiểm trùng");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        public ActionResult Kiemtrung(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiTrung_DongKienNghi;
                _kiennghi.UpdateThongTinKienNghi(kiennghi);
                UserInfor u_info = GetUserInfor();
                _kiennghi.Tracking_KN((int)u_info.tk_action.iUser, id, "Xác nhận kiến nghị trùng, lưu theo dõi.");
                Response.Redirect("/Kiennghi/Moicapnhat/#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xác nhận kiến nghị trùng, lưu theo dõi");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult Ajax_Change_KyHop_ChuongTrinh(int iChuongTrinh)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                var chuongTrinh = _kiennghi.Get_ChuongTrinh_ByID(iChuongTrinh);
                var kyHop = _kiennghi.Get_KyHop_QuocHoi((int)chuongTrinh.IKYHOP);
                string str = "<option value ='" + kyHop.IKYHOP + "'>" + kyHop.CTEN + "</option>";
                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex);
                throw;
            }

        }


        public ActionResult Ajax_Change_DonVi_ChuongTrinh(int iChuongTrinh)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                var chuongTrinh = _kiennghi.Get_ChuongTrinh_ByID(iChuongTrinh);
                string str = "";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ICOQUAN", chuongTrinh.IDONVI);
                var coQuan = _kiennghi.GetAll_CoQuanByParam(param).First();
                if (coQuan != null)
                    str = "<option value ='" + coQuan.ICOQUAN + "'>" + coQuan.CTEN + "</option>";
                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex);
                throw;
            }

        }
        public ActionResult Ajax_Change_HinhThuc_ChuongTrinh(int iChuongTrinh)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                var chuongTrinh = _kiennghi.Get_ChuongTrinh_ByID(iChuongTrinh);
                string str = "";
                int iTruocKyHop = (int)chuongTrinh.ITRUOCKYHOP;
                if (iTruocKyHop == -1)//chưa chọn, mới cập nhật
                {
                    if (Request.Cookies["truockyhop"] != null)//chưa có cookie trước đó
                    {
                        iTruocKyHop = Convert.ToInt32(Request.Cookies["truockyhop"].Value);
                    }
                    else// set mặc định là trước kỳ họp
                    {
                        iTruocKyHop = 1;
                    }
                }
                var hinhthuc = new List<TruocKyHop>
                {
                    new TruocKyHop { ten = "Trước kỳ họp", value = 1, class_span = "span5" },
                    new TruocKyHop { ten = "Sau kỳ họp", value = 0, class_span = "span4" },
                    new TruocKyHop { ten = "Khác", value = 2, class_span = "span3" }
                };
                foreach (var h in hinhthuc)
                {
                    string check = "disabled";
                    if (h.value == iTruocKyHop) { check = " checked "; }
                    str += "<span class='" + h.class_span + "'><input class='nomargin' type='radio' id='iTruocKyHop' name='iTruocKyHop' " + check + " id='h_" + h.class_span + "' value='" + h.value + "' /> " +
                         "<label for='h_" + h.class_span + "'>" + h.ten + "</label> </span>";
                }
                Response.Write(str);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex);
                throw;
            }

        }

        public ActionResult Ajax_Chondiaphuong(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string diaphuong_chon = fc["diaphuong_chon"];
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                ViewData["opt-tinh"] = kn.CheckBox_Tinh_Huyen_TiepXuc(iDonVi, diaphuong_chon);
                return PartialView("../Ajax/Kiennghi/Chondiaphuong");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách địa phương chọn lưu vào chương trình tiếp xúc cử tri");
                throw;
            }
            //if (!CheckAuthToken_Api()) { return null; }

        }
        public ActionResult Ajax_Chondaibieu(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string daibieu_chon = fc["daibieu_chon"];
                int iDonVi = Convert.ToInt32(fc["iDonVi"]);
                ViewData["opt-tinh"] = kn.CheckBox_DaiBieu_TiepXuc(daibieu_chon, iDonVi);
                return PartialView("../Ajax/Kiennghi/Chondaibieu");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách đại biểu chọn lưu vào chương trình tiếp xúc cử tri");
                throw;
            }
            //if (!CheckAuthToken_Api()) { return null; }

        }
        public ActionResult Ajax_View_chuongtrinh_diaphuong(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            //if (!CheckAuthToken_Api()) { return null; }
            try
            {
                string huyen = fc["huyen"];
                if (huyen == "")
                {
                    Response.Write("");
                }
                else
                {
                    string str = "<table class='table table-condensed table-bordered tableborder'>" +
                        "<input type='hidden' name='diaphuong_chon' id='diaphuong_chon' value='" + huyen + "'/>";
                    int count = 1;
                    foreach (var h in huyen.Split(','))
                    {
                        if (h != "")
                        {
                            int id_huyen = Convert.ToInt32(h);
                            DIAPHUONG dp1 = _kiennghi.HienThiThongTinDiaPhuong(id_huyen);
                            DIAPHUONG dp0 = _kiennghi.HienThiThongTinDiaPhuong((int)dp1.IPARENT);
                            string del = " <a href=\"javascript:void(0)\" onclick=\"HuyDiaPhuong(" + h + ")\" data-original-title='Hủy' rel='tooltip' title='' class='btn btn-danger'><i class='icon-trash'></i></a> ";
                            str += "<tr id='diaphuong_" + h + "'><td width='10%' class='b tcenter'>" + count + "</td>" +
                                "<td class=''><strong>" + Server.HtmlEncode(dp1.CTYPE) + " " + Server.HtmlEncode(dp1.CTEN) + "</strong> - " +
                                Server.HtmlEncode(dp0.CTYPE) + " " + Server.HtmlEncode(dp0.CTEN) + "</td>" +
                                "<td width='10%' class='b tcenter'>" + del + "</td>" +
                                "</tr>";
                        }
                        count++;
                    }
                    str += "</table>";
                    Response.Write(str);
                }

                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Lấy danh sách đại phương theo chương trình");
                throw;
            }


        }
        public ActionResult Ajax_View_chuongtrinh_daibieu(FormCollection fc)
        {
            //if (!CheckAuthToken_Api()) { return null; }
            if (!CheckAuthToken()) { return null; }
            try
            {
                string daibieu = fc["daibieu"];
                if (daibieu == "")
                {
                    Response.Write(""); return null;
                }
                else
                {
                    string str = "<table class='table table-condensed table-bordered tableborder'>" +
                        "<input type='hidden' name='daibieu_chon' id='daibieu_chon' value='" + daibieu + "'/>";
                    int count = 1;
                    foreach (var h in daibieu.Split(','))
                    {
                        if (h != "")
                        {
                            int id_user = Convert.ToInt32(h);
                            //USERS u = _kiennghi.HienThiThongTinTaikhoan(id_user);
                            DAIBIEU d = _kiennghi.Get_DaiBieu(id_user);
                            string truongdoan = ""; if (d.ITRUONGDOAN == 1) { truongdoan = " (Trưởng đoàn)"; }
                            string del = " <a href=\"javascript:void(0)\" onclick=\"HuyDaiBieu(" + h + ")\" data-original-title='Hủy' rel='tooltip' title='' class='btn btn-danger'><i class='icon-trash'></i></a> ";
                            str += "<tr id='daibieu_" + h + "'><td width='10%' class='b tcenter'>" + count + "</td>" +
                                "<td class=''><strong>" + Server.HtmlEncode(d.CTEN) + "</strong> " + truongdoan + "</td>" +
                                "<td width='10%' class='b tcenter'>" + del + "</td>" + "</tr>";
                        }
                        count++;
                    }
                    str += "</table>";
                    Response.Write(str);
                    return null;
                }
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Lấy danh sách đại biểu theo chương trình tiếp xúc cử tri");
                throw;
            }


        }

        public Boolean Insert_DaiBieu_ChuongTrinh(int id, string daibieu)
        {
            //if (!CheckAuthToken()) { return null; }
            // xóa đại biểu trong chương trình
            try
            {
                List<KN_CHUONGTRINH_DAIBIEU> ct = _kiennghi.Get_DaiBieu_ByChuongTrinh(id);
                Dictionary<string, object> _condition = new Dictionary<string, object>();
                foreach (var d in ct) { _kiennghi.Delete_DaiBieu_ByChuongTrinh(d); }
                if (daibieu != "")
                {
                    foreach (var d in daibieu.Split(','))
                    {
                        if (d != "")
                        {
                            int idaibieu = Convert.ToInt32(d);
                            KN_CHUONGTRINH_DAIBIEU ctdb = new KN_CHUONGTRINH_DAIBIEU();
                            ctdb.ICHUONGTRINH = id;
                            ctdb.IUSER_DAIBIEU = idaibieu;
                            ctdb.IUSER_COQUAN = 0;
                            _kiennghi.InsertChuongTrinhDaiBieu(ctdb);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Thêm đại biểu vào chương trình tiếp xúc cử tri");
                throw;
            }

        }
        public Boolean Insert_DiaPhuong_ChuongTrinh(int id, string diaphuong)

        {
            try
            {
                //db.Database.ExecuteSqlCommand("delete from kn_chuongtrinh_diaphuong where iChuongTrinh=" + id);
                List<KN_CHUONGTRINH_DIAPHUONG> ct = _kiennghi.Get_Diaphuong_ByChuongTrinh(id);
                Dictionary<string, object> _condition = new Dictionary<string, object>();
                foreach (var d in ct) { _kiennghi.Delete_Diaphuong_ByChuongTrinh(d); }
                if (diaphuong != "")
                {
                    foreach (var d in diaphuong.Split(','))
                    {
                        if (d != "")
                        {
                            int iHuyen = Convert.ToInt32(d);
                            //db.Database.ExecuteSqlCommand("insert into kn_chuongtrinh_diaphuong(iChuongTrinh,iDiaPhuong0,iDiaPhuong1) values(" + id +
                            //    "," + db.diaphuongs.Single(x => x.iDiaPhuong.Equals(iHuyen)).iParent + "," + iHuyen + ")");

                            KN_CHUONGTRINH_DIAPHUONG ctdb = new KN_CHUONGTRINH_DIAPHUONG();
                            ctdb.ICHUONGTRINH = id;
                            ctdb.IDIAPHUONG0 = (int)_kiennghi.HienThiThongTinDiaPhuong(iHuyen).IPARENT;
                            ctdb.IDIAPHUONG1 = iHuyen;
                            _kiennghi.InsertChuongTrinhDiaPhuong(ctdb);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Thêm địa phương chọn lưu vào chương trình tiếp xúc cử tri");
                throw;
            }

        }
        public ActionResult Chuongtrinh_info(string id)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int iChuongTrinh = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                ChuongtrinhCuTri ct = kn.Chuongtring_detail(iChuongTrinh, Request.Cookies["url_key"].Value);
                KN_CHUONGTRINH chuongtrinh = _kiennghi.HienThiThongTinChuongTrinh(iChuongTrinh);
                ViewData["kn"] = chuongtrinh;
                ViewData["detail"] = ct;
                var chitiet = _kiennghi.PRC_CHUONGTRINH_CHITIET(iChuongTrinh);
                if (chitiet.Count() > 0)
                {
                    string list = "<table class='table table-bordered table-condensed table-striped' style='border-top:1px solid #ddd;border-right:1px solid #ddd'><tr><th class='tcenter' width='5%'>STT</th>" +
                        "<th class='tcenter'>Tổ đại biểu</th><th class='tcenter'>Quận/huyện</th><th class='tcenter'>Xã/thị trấn</th><th class='tcenter'>Ngày tiếp</th><th class='tcenter'>Địa chỉ</th></tr>" + kn.ChuongTring_View(chitiet) + "</table>";
                    ViewData["list"] = list;
                }
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thông tin chi tiết chương trình tiếp xúc cử tri");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Sua()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                var user = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("kn_suakiennghi", id);
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                UserInfor u_info = GetUserInfor();
                List<LINHVUC> linhvuc = _kiennghi.GetAll_LinhVuc();
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan((int)kiennghi.ITHAMQUYENDONVI, (int)kiennghi.ILINHVUC);
                ViewData["kn"] = kiennghi;
                ViewData["user"] = user;
                ViewData["file"] = kn.File_Edit(id, "kn_kiennghi", Request.Cookies["url_key"].Value);
                ViewData["id"] = Request["id"];
                ViewData["opt-kyhop"] = Get_Option_KyHop((int)kiennghi.IKYHOP);
                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();

                List<KIENNGHI_NGUONDON> lst_nguondon = _kiennghi.GetAll_KnNguonDon_ByID(id);
                List<string> lst = new List<string>();
                foreach (var item in lst_nguondon)
                {
                    lst.Add(item.INGUONDON.ToString());
                }
                string sKNNguonDon = String.Join(",", lst);

                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, sKNNguonDon);

                Dictionary<string, object> donvi = new Dictionary<string, object>();
                List<QUOCHOI_COQUAN> coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
                int iDonVi = (int)u_info.user_login.IDONVI;
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ICOQUAN", kiennghi.ITHAMQUYENDONVI);
                var coquanchon = _kiennghi.GetAll_CoQuanByParam(param).FirstOrDefault();
                ViewData["opt-doandaibieu"] = Get_Option_DonViLap();
                //List<KN_CHUONGTRINH> chuongtrinh = _kiennghi.GetAll_ChuongTrinh(iDonVi, (int)kiennghi.IKYHOP);
                QUOCHOI_COQUAN coquan_thamquyen = coquan.Where(x => x.ICOQUAN == (int)kiennghi.ITHAMQUYENDONVI).FirstOrDefault();
                ViewData["is_dbqh"] = "0";
                ViewData["thamquyen_diaphuong"] = "0";
                ViewData["radio-thamquyen"] = "";

                ViewData["diaphuong"] = kiennghi.ITHAMQUYENDONVI == ID_Ban_DanNguyen_New ? "1" : "0";
                if (kiennghi.ID_GOP == -1)
                {

                    ViewData["opt-donvithamquyen"] = "<option value='" + kiennghi.ITHAMQUYENDONVI + "'>" + coquan_thamquyen.CTEN + "</option>";
                }
                else
                {
                    //if (u_info.tk_action.is_lanhdao)
                    //{

                    //    ViewData["opt-donvithamquyen"] = kn.OptionCoQuanXuLy(coquan, 0, 0, (int)kiennghi.ITHAMQUYENDONVI, 0);
                    //}
                    //else
                    //{
                    //    ViewData["is_dbqh"] = "1";
                    //    ViewData["opt-donvithamquyen"] = kn.OptionCoQuanXuLy(coquan, 0, 0, (int)kiennghi.ITHAMQUYENDONVI, 0);
                    //    ViewData["opt-donvithamquyen-diaphuong"] = Get_Option_ThamQuyen_DiaPhuong((int)kiennghi.ITHAMQUYENDONVI);
                    //    if (coquan_thamquyen.IDIAPHUONG == 0)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        ViewData["thamquyen_diaphuong"] = "1";
                    //    }
                    //}
                    ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)kiennghi.ITHAMQUYENDONVI);
                    if (u_info.tk_action.is_lanhdao)

                    {
                        ViewData["is_dbqh"] = "0";
                        ViewData["opt-donvithamquyen"] = "";
                        if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                        {
                            ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong, (int)kiennghi.ITHAMQUYENDONVI);
                            ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Trunguong);
                        }

                        if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                        {
                            ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Tinh, (int)kiennghi.ITHAMQUYENDONVI);
                            ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Tinh);
                        }

                        if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                        {
                            ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Huyen, (int)kiennghi.ITHAMQUYENDONVI);
                            ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Huyen);
                        }

                        QUOCHOI_COQUAN thamquyen = _kiennghi.HienThiThongTinCoQuan((int)kiennghi.ITHAMQUYENDONVI);
                        if (thamquyen.IDIAPHUONG == 0)//trung ương
                        {
                            ViewData["opt-donvithamquyen-diaphuong-parent"] = Get_Option_ThamQuyen_DiaPhuong_Parent(0);
                            ViewData["opt-donvithamquyen-diaphuong"] = "<option value='0'> Chọn đơn vị</option>";
                        }
                        else
                        {
                            ViewData["diaphuong"] = 1;
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("IHIENTHI", 1);
                            dic.Add("IDELETE", 0);
                            dic.Add("IDIAPHUONG", thamquyen.IDIAPHUONG);
                            dic.Add("IPARENT", ID_Coquan_HoiDong_DiaPhuong);
                            var list_thamquyen = _kiennghi.HienThiDanhSachCoQuan(dic);
                            ViewData["opt-donvithamquyen"] = "";
                            if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                                ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong, (int)kiennghi.ITHAMQUYENDONVI);
                            if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                                ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Tinh, (int)kiennghi.ITHAMQUYENDONVI);
                            if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                                ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Huyen, (int)kiennghi.ITHAMQUYENDONVI);
                            if (list_thamquyen.Count == 0)
                            {
                                ViewData["opt-donvithamquyen-diaphuong"] = "<option value='0'> Chọn đơn vị</option>";
                                ViewData["opt-donvithamquyen-diaphuong-parent"] = Get_Option_ThamQuyen_DiaPhuong_Parent(0);
                            }
                            else
                            {
                                ViewData["opt-donvithamquyen-diaphuong-parent"] = Get_Option_ThamQuyen_DiaPhuong_Parent((int)list_thamquyen.FirstOrDefault().ICOQUAN);
                                Dictionary<string, object> dic1 = new Dictionary<string, object>();
                                dic1.Add("IHIENTHI", 1);
                                dic1.Add("IDELETE", 0);
                                //dic1.Add("IDIAPHUONG", thamquyen.IDIAPHUONG);
                                dic1.Add("IPARENT", (int)list_thamquyen.FirstOrDefault().ICOQUAN);
                                var list_thamquyen_child = _kiennghi.HienThiDanhSachCoQuan(dic1);
                                ViewData["opt-donvithamquyen-diaphuong"] = "<option value='0'> Chọn đơn vị</option>" +
                                    kn.OptionCoQuanXuLy(list_thamquyen_child, (int)list_thamquyen.FirstOrDefault().ICOQUAN, 0, (int)kiennghi.ITHAMQUYENDONVI, 0);
                            }

                        }
                    }
                    else
                    {
                        ViewData["is_dbqh"] = "1";
                        if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                        {
                            ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Trunguong);
                        }

                        if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                        {
                            ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Tinh);
                        }

                        if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                        {
                            ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Huyen);
                        }
                        ViewData["opt-donvithamquyen"] = Get_Option_ThamQuyen_TrungUong((int)kiennghi.ITHAMQUYENDONVI);
                        ViewData["opt-donvithamquyen-diaphuong"] = Get_Option_ThamQuyen_DiaPhuong((int)kiennghi.ITHAMQUYENDONVI);
                        QUOCHOI_COQUAN thamquyen = _kiennghi.HienThiThongTinCoQuan((int)kiennghi.ITHAMQUYENDONVI);
                        if (thamquyen.IDIAPHUONG != 0)//trung ương
                        {
                            ViewData["diaphuong"] = 1;
                        }

                    }
                }

                var chuongtrinh = _kiennghi.GetAll_ChuongTrinh(0, 0);
                if (chuongtrinh.Count > 0)
                {
                    ViewData["opt-chuongtrinhTXCT"] = kn.Option_ChuongTrinh_ByKyHop(chuongtrinh, (int)kiennghi.ICHUONGTRINH);
                }
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop((int)kiennghi.ITRUOCKYHOP);
                ViewData["opt-tinh"] = Get_Option_DiaPhuong0((int)kiennghi.IDIAPHUONG0);
                ViewData["opt-huyen"] = Get_Option_DiaPhuong1((int)kiennghi.IDIAPHUONG0, (int)kiennghi.IDIAPHUONG1);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form sửa kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Suachuongtrinh()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iUser = u_info.tk_action.iUser;
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                ChuongtrinhCuTri ct = kn.Chuongtring_detail(id, Request.Cookies["url_key"].Value);

                KN_CHUONGTRINH chuongtrinh = _kiennghi.HienThiThongTinChuongTrinh(id);
                if (chuongtrinh.IUSER != iUser && u_info.tk_action.is_admin == false)
                {
                    Response.Redirect("/Home/Error/");
                }
                int num = _kiennghi.PRC_CHUONGTRINH_CHITIET(id).Count();
                ViewData["num"] = num;
                SetTokenAction("kn_suachuongtrinh", id);
                ViewData["kn"] = chuongtrinh;
                ViewData["detail"] = ct;
                ViewData["id"] = Request["id"];
                //ViewData["daibieu"] = View_chuongtrinh_daibieu(id);
                //ViewData["diaphuong"] = View_chuongtrinh_diaphuong(id);
                ViewData["iKyHopHienTai"] = ID_KyHop_HienTai();
                ViewData["listKhoaHop"] = JsonConvert.SerializeObject(_kiennghi.GetAll_KhoaHop().OrderBy(x => x.DBATDAU).ToList());
                ViewData["listKyHop"] = JsonConvert.SerializeObject(_kiennghi.GetAll_KyHop().OrderBy(x => x.DBATDAU).ToList());
                ViewData["file"] = kn.File_Edit(id, "kn_chuongtrinh", Request.Cookies["url_key"].Value);
                ViewData["opt-doituong"] = Get_Option_LoaiDoiTuong((int)chuongtrinh.IDOITUONG);
                ViewData["opt-donvi"] = Get_Option_DonViLap_PhanQuyen((int)chuongtrinh.IDONVI);
                //if (u_info.tk_action.is_lanhdao == false)
                //{
                //    ViewData["opt-donvi"] = "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
                //}
                //else
                //{
                //    Dictionary<string, object> _dic = new Dictionary<string, object>();
                //    _dic.Add("IPARENT", ID_Coquan_doandaibieu);
                //    var coquan = _kiennghi.GetAll_CoQuanByParam(_dic);
                //    ViewData["opt-donvi"] = "<option value='0'>- - - Chọn đoàn đại biểu</option>" +
                //                             kn.OptionCoQuanXuLy(coquan, ID_Coquan_doandaibieu, 0, (int)chuongtrinh.IDONVI, 0);
                //}
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop((int)chuongtrinh.ITRUOCKYHOP);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form sửa chương trình");
                return View("../Home/Error_Exception");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suachuongtrinh(FormCollection fc, HttpPostedFileBase file, string[] iDaiBieu, string[] iDiaPhuong, string[] iDiaPhuong_02, string[] dNgayTiep, string[] cDiaChi)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int iUser = id_user();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("kn_suachuongtrinh", id))
                {
                    Response.Redirect("/Home/Error/");
                    return null;
                }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }

                }
                KN_CHUONGTRINH kn = _kiennghi.HienThiThongTinChuongTrinh(id);
                kn.IDONVI = Convert.ToInt32(fc["iDonVi"]);
                kn.IKHOA = Convert.ToInt32(fc["iKhoaHop"]);
                kn.IKYHOP = Convert.ToInt32(fc["iKyHop"]);
                kn.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]);
                if (fc["dBatDau"] != "")
                    kn.DBATDAU = Convert.ToDateTime(func.ConvertDateToSql(fc["dBatDau"]));
                if (fc["dKetThuc"] != "")
                    kn.DKETTHUC = Convert.ToDateTime(func.ConvertDateToSql(fc["dKetThuc"]));
                kn.IDOITUONG = Convert.ToInt32(fc["iDoituong"]);
                if (fc["dNgaybanhanh"] != "")
                    kn.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgaybanhanh"]));
                kn.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                kn.CKEHOACH = func.RemoveTagInput(fc["cKeHoach"]);
                kn.CDIACHI = func.RemoveTagInput(fc["cDiaChiTiepXuc"]);
                _kiennghi.UpdateChuongTrinhKienNghi(kn);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_chuongtrinh";
                        f.CFILE = UploadFile(file);
                        f.ID = id;
                        kntc.Upload_file(f);
                    }

                }
                _kiennghi.Delete_All_ChuongTrinh_ChiTiet(id);
                var iDiaPhuongDBQH = 0;
                if (Convert.ToInt32(fc["iDoituong"]) == 0)
                {
                    QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(Convert.ToInt32(fc["iDonVi"]));
                    if (coquan != null)
                    {
                        iDiaPhuongDBQH = coquan.IDIAPHUONG.HasValue ? (int)coquan.IDIAPHUONG : 0;
                    }
                }
                if (fc["iDaiBieu"] != null)
                {
                    for (int i = 0; i < iDaiBieu.Count(); i++)
                    {
                        if (Convert.ToInt32(iDaiBieu[i]) != 0)
                        {
                            KN_CHUONGTRINH_CHITIET c = new KN_CHUONGTRINH_CHITIET();
                            c.ICHUONGTRINH = id;
                            c.ITODAIBIEU = Convert.ToInt32(iDaiBieu[i]);
                            c.IDIAPHUONG = Convert.ToInt32(iDiaPhuong[i]);
                            c.IDIAPHUONG2 = Convert.ToInt32(iDiaPhuong_02[i]);
                            c.CDIACHI = cDiaChi[i];
                            if (dNgayTiep[i].ToString() != "")
                            {
                                c.DNGAYTIEP = Convert.ToDateTime(func.ConvertDateToSql(dNgayTiep[i].ToString()));
                            }
                            _kiennghi.InsertCHUONGTRINH_CHITIET(c);
                        }
                    }
                }

                Tracking(iUser, "Sửa chương trình tiếp xúc cử tri, kế hoạch: " + kn.CKEHOACH);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Update chương trình tiếp xúc cử tri");
                return View("../Home/Error_Exception");
            }

        }
        public string View_chuongtrinh_daibieu(int id_chuongtrinh)
        {
            string str = "<table class='table table-condensed table-bordered tableborder'>";
            int count = 1;
            _condition = new Dictionary<string, object>();
            _condition.Add("ICHUONGTRINH", id_chuongtrinh);
            var daibieu = _kiennghi.Get_DaiBieu_ByChuongTrinh(id_chuongtrinh);
            //var daibieu = db.kn_chuongtrinh_daibieu.Where(x => x.iChuongTrinh == id_chuongtrinh).ToList();
            string arr_daibieu = "";
            foreach (var h in daibieu)
            {
                int id_user = Convert.ToInt32(h.IUSER_DAIBIEU);
                DAIBIEU d = _kiennghi.Get_DaiBieu(id_user);
                string truongdoan = ""; if (d.ITRUONGDOAN == 1) { truongdoan = " (Trưởng đoàn)"; }
                string del = " <a href=\"javascript:void(0)\" onclick=\"HuyDaiBieu(" + id_user + ")\" data-original-title='Hủy' rel='tooltip' title='' class='btn btn-danger'><i class='icon-trash'></i></a> ";
                str += "<tr id='daibieu_" + h.IUSER_DAIBIEU + "'><td width='10%' class='b tcenter'>" + count + "</td>" +
                    "<td class=''><strong>" + Server.HtmlEncode(d.CTEN) +
                    "</strong> " + truongdoan + "</td>" +
                    "<td width='10%' class='b tcenter'>" + del + "</td>" +
                    "</tr>";
                count++;
                arr_daibieu += id_user + ",";
            }
            str += "<input type='hidden' name='daibieu_chon' id='daibieu_chon' value='" + arr_daibieu + "'/></table>";
            return str;
        }
        public string View_chuongtrinh_diaphuong(int id_chuongtrinh)
        {
            string str = "<table class='table table-condensed table-bordered tableborder'>";
            int count = 1;
            var diaphuong = _kiennghi.Get_Diaphuong_ByChuongTrinh(id_chuongtrinh).ToList();
            string arr_daibieu = "";
            var diaphuong_all = _kiennghi.GetAll_DiaPhuong();
            foreach (var h in diaphuong)
            {
                string del = " <a href=\"javascript:void(0)\" onclick=\"HuyDiaPhuong(" + h.IDIAPHUONG1 + ")\" data-original-title='Hủy' rel='tooltip' title='' class='btn btn-danger'><i class='icon-trash'></i></a> ";
                DIAPHUONG huyen = diaphuong_all.Where(x => x.IDIAPHUONG == (int)h.IDIAPHUONG1).FirstOrDefault();
                str += "<tr id='diaphuong_" + h.IDIAPHUONG1 + "'><td width='10%' class='b tcenter'>" + count + "</td>" +
                    "<td class=''>" + Server.HtmlEncode(huyen.CTYPE) + " " + Server.HtmlEncode(huyen.CTEN) + "</td>" +
                    "<td width='10%' class='b tcenter'>" + del + "</td>" +
                    "</tr>";
                count++;
                arr_daibieu += h.IDIAPHUONG1 + ",";
            }
            str += "<input type='hidden' name='diaphuong_chon' id='diaphuong_chon' value='" + arr_daibieu + "'/></table>";
            return str;
        }
        public ActionResult Ajax_Chuongtrinh_search()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                List<QUOCHOI_COQUAN> coquan;
                Dictionary<string, object> donvi = new Dictionary<string, object>();
                UserInfor u_info = GetUserInfor();
                int iDonVi = (int)u_info.user_login.IDONVI;
                if (u_info.tk_action.is_lanhdao)
                {
                    if (Request["iDonVi"] != null)
                    {
                        iDonVi = Convert.ToInt32(Request["iDonVi"]);
                        donvi.Add("ICOQUAN", iDonVi);
                    }
                    else
                    {
                        iDonVi = 0;
                    }
                    coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
                    ViewData["opt-doan"] = "<option value='0'>Tất cả</option>" +
                                            kn.OptionCoQuanXuLy(coquan, ID_Coquan_doandaibieu, 0, iDonVi, 0);
                }
                else
                {
                    donvi.Add("ICOQUAN", iDonVi);
                    coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
                    ViewData["opt-doan"] = "<option value='" + iDonVi + "'>" + Server.HtmlEncode(u_info.tk_action.tendonvi) + "</option>";

                }
                List<DIAPHUONG> diaphuong = _kiennghi.GetAll_DiaPhuong();
                ViewData["opt-diaphuong"] = kn.Option_All_Tinh_Huyen(diaphuong);
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                return PartialView("../Ajax/Kiennghi/Chuongtrinh_search");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm kiếm chương trình tiếp xúc cử tri");
                throw;
            }

        }
        public ActionResult Ajax_Themkiennghi_gop(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                int iKyHop = (int)kiennghi.IKYHOP;
                ViewData["id"] = Request["id"];
                //Dictionary<string, object> donvi = new Dictionary<string, object>();
                //int iDonVi = (int)tonghop.IDONVITONGHOP;
                //List<KN_CHUONGTRINH> chuongtrinh = _kiennghi.GetAll_ChuongTrinh(iDonVi, iKyHop);
                if (u_info.tk_action.is_dbqh)
                {
                    ViewData["dbqh"] = 1;
                }
                else
                {
                    ViewData["dbqh"] = 0;
                }
                ViewData["opt-donvitonghop"] = Get_Option_DonViTiepNhan();
                //ViewData["opt-kehoach"] = kn.Option_KeHoach_ByKyHop(chuongtrinh, 0);
                //List<LINHVUC> linhvuc = _kiennghi.GetAll_LinhVuc();
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);

                QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan((int)kiennghi.ITHAMQUYENDONVI);
                ViewData["opt-donvithamquyen"] = "<option value='" + kiennghi.ITHAMQUYENDONVI + "'>" + coquan.CTEN + "</option>";

                //ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen();
                return PartialView("../Ajax/Kiennghi/Themkiennghi_gop");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm kiếm kiến nghị gộp");
                throw;
            }
        }
        public ActionResult Ajax_Themkiennghi_tonghop(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_TONGHOP tonghop = _kiennghi.Get_TongHop_KienNghi(id);
                int iKyHop = (int)tonghop.IKYHOP;
                ViewData["id"] = Request["id"];
                Dictionary<string, object> donvi = new Dictionary<string, object>();
                int iDonVi = (int)tonghop.IDONVITONGHOP;
                List<KN_CHUONGTRINH> chuongtrinh = _kiennghi.GetAll_ChuongTrinh(iDonVi, iKyHop);
                if (u_info.tk_action.is_dbqh)
                {
                    ViewData["dbqh"] = 1;
                    ViewData["opt-donvitonghop"] = "<option value='" + iDonVi + "'>" + _kiennghi.HienThiThongTinCoQuan(iDonVi).CTEN + "</option>";
                }
                else
                {
                    ViewData["opt-donvitonghop"] = Get_Option_DonViTiepNhan();
                    ViewData["dbqh"] = 0;
                }

                ViewData["opt-kehoach"] = kn.Option_ChuongTrinh_ByKyHop(chuongtrinh, 0);
                //List<LINHVUC> linhvuc = _kiennghi.GetAll_LinhVuc();
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                //ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                if (u_info.tk_action.is_lanhdao)
                {
                    QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan((int)tonghop.ITHAMQUYENDONVI);

                    ViewData["opt-donvithamquyen"] = "<option value='" + tonghop.ITHAMQUYENDONVI + "'>" + coquan.CTEN + "</option>";
                }
                else
                {
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen();
                }

                return PartialView("../Ajax/Kiennghi/Themkiennghi_tonghop");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm kiếm kiến nghị lưu vào Tập hợp kiến nghị");
                throw;
            }

        }
        public KN_KIENNGHI get_Request_Paramt_KienNghi()
        {
            KN_KIENNGHI kn = new KN_KIENNGHI();
            if (Request["iKyHop"] != null)
            {
                kn.IKYHOP = Convert.ToInt32(Request["iKyHop"]);
            }
            else { kn.IKYHOP = ID_KyHop_HienTai(); }

            if (Request["iDoan"] != null)
            {
                kn.IDONVITIEPNHAN = Convert.ToInt32(Request["iDoan"]);
            }
            else { kn.IDONVITIEPNHAN = 0; }

            if (Request["iDonViXuLy"] != null)
            {
                kn.ITHAMQUYENDONVI = Convert.ToInt32(Request["iDonViXuLy"]);
            }
            else { kn.ITHAMQUYENDONVI = 0; }

            if (Request["iLinhVuc"] != null)
            {
                kn.ILINHVUC = Convert.ToInt32(Request["iLinhVuc"]);
            }
            else { kn.ILINHVUC = -1; }

            if (Request["cNoiDung"] != null || Request["q"] != null)
            {
                if (Request["cNoiDung"] != null)
                {
                    kn.CNOIDUNG = Request["cNoiDung"];
                }
                if (Request["q"] != null)
                {
                    kn.CNOIDUNG = Request["q"];
                }
            }
            else { kn.CNOIDUNG = ""; }
            return kn;
        }
        public KN_TONGHOP get_Request_Paramt_TongHop_KienNghi()
        {
            KN_TONGHOP kn = new KN_TONGHOP();
            if (Request["iKyHop"] != null)
            {
                kn.IKYHOP = Convert.ToInt32(Request["iKyHop"]);
            }
            else { kn.IKYHOP = ID_KyHop_HienTai(); }

            if (Request["iDoan"] != null)
            {
                kn.IDONVITONGHOP = Convert.ToInt32(Request["iDoan"]);
            }
            else { kn.IDONVITONGHOP = 0; }

            if (Request["iDonViXuLy"] != null)
            {
                kn.ITHAMQUYENDONVI = Convert.ToInt32(Request["iDonViXuLy"]);
            }
            else { kn.ITHAMQUYENDONVI = 0; }

            if (Request["iLinhVuc"] != null)
            {
                kn.ILINHVUC = Convert.ToInt32(Request["iLinhVuc"]);
            }
            else { kn.ILINHVUC = -1; }

            if (Request["cNoiDung"] != null || Request["q"] != null)
            {
                if (Request["cNoiDung"] != null)
                {
                    kn.CNOIDUNG = Request["cNoiDung"];
                }
                if (Request["q"] != null)
                {
                    kn.CNOIDUNG = Request["q"];
                }
            }
            else { kn.CNOIDUNG = ""; }
            return kn;
        }
        public KN_TONGHOP get_Request_Paramt_TongHop_KienNghi_BanDanNguyen()
        {
            KN_TONGHOP kn = new KN_TONGHOP();
            if (Request["iKyHop"] != null)
            {
                kn.IKYHOP = Convert.ToInt32(Request["iKyHop"]);
            }
            else { kn.IKYHOP = ID_KyHop_HienTai(); }

            if (Request["iDoan"] != null)
            {
                kn.IDONVITONGHOP = Convert.ToInt32(Request["iDoan"]);
            }
            else { kn.IDONVITONGHOP = 0; }

            if (Request["iDonViXuLy"] != null)
            {
                kn.ITHAMQUYENDONVI = Convert.ToInt32(Request["iDonViXuLy"]);
            }
            else { kn.ITHAMQUYENDONVI = 0; }

            if (Request["cNoiDung"] != null || Request["q"] != null)
            {
                if (Request["cNoiDung"] != null)
                {
                    kn.CNOIDUNG = Request["cNoiDung"];
                }
                if (Request["q"] != null)
                {
                    kn.CNOIDUNG = Request["q"];
                }
            }
            else { kn.CNOIDUNG = ""; }
            return kn;
        }
        public ActionResult Ajax_Themkiennghivaotonghop(int id)
        {
            KN_KIENNGHI kn = _kiennghi.HienThiThongTinKienNghi(id);
            if (kn != null)
            {
                ID_Session_KienNghi_ChonTongHop id_session = new ID_Session_KienNghi_ChonTongHop();
                id_session.IKIENNGHI = (decimal)id;
                id_session.ILINHVUC = (decimal)kn.ILINHVUC;
                id_session.ITHAMQUYENDONVI = (decimal)kn.ITHAMQUYENDONVI;
                Set_IDKienNghiChonTongHop(id_session);
            }
            return null;
        }

        //public ActionResult Moicapnhat_Json()
        //{
        //    if (!CheckAuthToken()) {  return null; }
        //    try
        //    {
        //        UserInfor u_info = GetUserInfor();

        //        int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
        //        if (u_info.tk_action.is_lanhdao)
        //        {
        //            iDonViTiepNhan = 0;
        //        }
        //        int iKyHop = ID_KyHop_HienTai();
        //        KN_KIENNGHI kn_pram = new KN_KIENNGHI();
        //        if (Request["iDoan"] != null)
        //        {
        //            iDonViTiepNhan = Convert.ToInt32(Request["iDoan"]);
        //            kn_pram.IDONVITIEPNHAN = iDonViTiepNhan;
        //        }
        //        if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
        //        kn_pram.IKYHOP = iKyHop;
        //        kn_pram.ITINHTRANG = (decimal)TrangThaiKienNghi.Moicapnhat;
        //        var list_kn = _kiennghi.List_KienNghi_Tracuu(kn_pram);
        //        string url_cookie = func.Get_Url_keycookie();
        //        foreach (var n in list_kn)
        //        {
        //            string id_encr = HashUtil.Encode_ID(n.ID_KIENNGHI.ToString(), url_cookie);
        //            n.ID_ENCR = id_encr;
        //            if (n.ID_LINHVUC == 0) { n.TEN_LINHVUC = "Lĩnh vực khác"; }
        //        }
        //        return Json(new { totalCount = list_kn.Count, Response = list_kn }, JsonRequestBehavior.AllowGet);
        //        //return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Danh sách kiến nghị mới cập nhật");
        //        return null;
        //    }
        //}
        public ActionResult Moicapnhat()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                System.Web.HttpContext.Current.Session["id_kiennghi"] = null;
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                string url = Request.RawUrl;
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null)
                {
                    string[] arr_post_per_page = Request["post_per_page"].Split(',');
                    if (arr_post_per_page.Length > 1)
                    {
                        post_per_page = Convert.ToInt32(arr_post_per_page[0]);
                    }
                    else
                    {
                        post_per_page = Convert.ToInt32(Request["post_per_page"]);
                    }

                }
                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("3,4,5", u_info.tk_action)) { Response.Redirect("/Home/Error/"); }
                if (!base_business.ActionMulty_("5,50", u_info.tk_action))//chọn Tập hợp
                {
                    ViewData["bt-add-tonghop"] = " style='display:none'";
                }
                if (!base_business.ActionMulty_("3,50", u_info.tk_action))//thêm mới kiến nghị
                {
                    ViewData["bt-add"] = " style='display:none'";
                    ViewData["bt-gop"] = " style='display:none'";
                }
                if (!u_info.tk_action.is_lanhdao) { ViewData["bt-gop"] = " style='display:none'"; }
                int iDonViTiepNhan = 0;
                int iDonViXuLy = 0;
                int iNguonKienNghi = 0;
                int iDonViXuLy_Parent = 0;
                int iDiaPhuong_0 = 0;
                int iDiaPhuong_1 = 0;

                int iKyHop = ID_KyHop_HienTai();
                KN_KIENNGHI kn_pram = get_Request_Paramt_KienNghi();
                if (Request["iDoan"] != null)
                {
                    iDonViTiepNhan = Convert.ToInt32(Request["iDoan"]);
                } 
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (Request["q"] != null) { kn_pram.CNOIDUNG = func.RemoveTagInput(Request["q"]); }
                if (Request["cNoiDung"] != null) { kn_pram.CNOIDUNG = func.RemoveTagInput(Request["cNoiDung"]); }
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                if (Request["iNguonKienNghi"] != null) { iNguonKienNghi = Convert.ToInt32(Request["iNguonKienNghi"]); }
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDiaPhuong_0"] != null) { iDiaPhuong_0 = Convert.ToInt32(Request["iDiaPhuong_0"]); }
                if (Request["iDiaPhuong_1"] != null) { iDiaPhuong_1 = Convert.ToInt32(Request["iDiaPhuong_1"]); }

                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenGiaiQuyet(iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    ViewData["opt-thamquyen-xuly"] = Get_Option_DonViThamQuyen_ByCType(iDonViXuLy_Parent, iDonViXuLy);
                }
                else
                {
                    //coquan = null;
                    ViewData["opt-thamquyen-xuly"] = Get_Option_DonViThamQuyen_ByCType();
                }
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                kn_pram.ITHAMQUYENDONVI = iDonViXuLy;
                kn_pram.IDONVITIEPNHAN = iDonViTiepNhan;
                kn_pram.ITINHTRANG = (decimal)TrangThaiKienNghi.Moicapnhat;
                kn_pram.IUSER = iUser;
                kn_pram.IDIAPHUONG0 = iDiaPhuong_0;
                kn_pram.IDIAPHUONG1 = iDiaPhuong_1;
                //kn_pram.INGUONKIENNGHI = iNguonKienNghi;
                //Mặc định sẽ hiển thị hết kiến nghị trong mọi kỳ họp
                kn_pram.IKYHOP = 0;
                string listKyHop = "0";
                ViewData["dbqh"] = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    kn_pram.IKYHOP = -1;
                    listKyHop = Request["listKyHop"];
                }
                List<PRC_KIENNGHI_MOICAPNHAT> list_kn;
                /*Bổ sung thêm các trường tìm kiếm */
                string imakiennghi = "";
                if (Request["iMaKienNghi"] != "" && Request["iMaKienNghi"] != null)
                {
                    imakiennghi = Request["iMaKienNghi"];
                }
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["iTuNgay"] != null && Request["iTuNgay"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["iTuNgay"]));
                }
                if (Request["iDenNgay"] != null && Request["iDenNgay"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["iDenNgay"]));
                }
                string lstNguonKN = "";
                if (Request["listNguonKN"] != null)
                {
                    lstNguonKN = Request["listNguonKN"];
                }
                string lstLinhVuc = "";
                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                string cnoidung = Request["cNoiDung"];
                if (Request["q"] != null)
                {
                    cnoidung = Request["q"];
                }

                int iTruocKyHop = -1;
                if (Request["iTruocKyHop"] != null)
                {
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);
                }

                list_kn = _kiennghi.PRC_KIENNGHI_MOICAPNHAT_PHANTRANG(kn_pram, listKyHop, imakiennghi, dtungay, ddenngay, lstNguonKN, lstLinhVuc, cnoidung, 0, page, post_per_page, iTruocKyHop);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;
                
                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;
                if (list_kn != null && list_kn.Count() > 0)
                {
                    if (!u_info.tk_action.is_lanhdao)
                    {
                        list_kn.Where(x => x.ITINHTRANG == 0).ToList();
                    }
                    List<ID_Session_KienNghi_ChonTongHop> list_id_kiennghi_tonghop = Get_IDKienNghiChonTongHop();
                    htmlList = kn.KN_Moicapnhat_Tracuu(list_kn, u_info.tk_action, list_id_kiennghi_tonghop);
                }
                else
                {
                    htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }
                if (list_kn != null && list_kn.Count() > 0)
                {
                    htmlPhanTrang = "<tr><td colspan='5'>" + base_appcode.PhanTrang((int)list_kn.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-coquan"] = Get_Option_DonViTiepNhan(iDonViTiepNhan);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách kiến nghị mới cập nhật");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult BaoCao_TongHopYKienCuTri1B2(string lstKyHop, string lstNguonKN, string lstLinhVuc, int loaibaocao, string ext = "pdf")
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

                string fileName = "";
                var templatePath = "";
                if (loaibaocao == 1)
                {
                    fileName = string.Format("{0}.{1}", "Baocao1B2(TongHop)_TongHopYKienCuTri", ext);
                    templatePath = ReportConstants.BaoCaoDanhMucKienNghiTemplate;
                }
                if (loaibaocao == 2)
                {
                    fileName = string.Format("{0}.{1}", "Baocao1B2(HĐND)_TongHopYKienCuTri", ext);
                    templatePath = ReportConstants.BaoCaoDanhMucKienNghiTemplate;
                }
                if (loaibaocao == 3)
                {
                    fileName = string.Format("{0}.{1}", "Baocao1B2(QH)_TongHopYKienCuTri", ext);
                    templatePath = ReportConstants.BaoCaoDanhMucKienNghiTemplate;
                }
                templatePath = Server.MapPath(ReportConstants.BaoCaoDanhMucKienNghiTemplate);
                ExcelFile xls = ExportReportMoiCapNhat(lstKyHop, lstNguonKN, lstLinhVuc, loaibaocao, templatePath);
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách kiến nghị mới cập nhật");
                return View("../Home/Error_Exception");
            }
        }

        public ExcelFile ExportReportMoiCapNhat(string lstKyHop, string lstNguonKN, string lstLinhVuc, int loaibaocao, string templatePath)
        {
            var listKienNghiReport = new List<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG>();
            listKienNghiReport.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG()
            {
                CSTT = "I",
                CTENLINHVUC = "Cơ chế chính sách",
                ITONGSOKIENNGHI = 100,
                DTILE = 100,
                LIST_DATA = new List<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA>()
            });
            listKienNghiReport.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG()
            {
                CSTT = "1",
                CTENLINHVUC = "Lĩnh vực a",
                ITONGSOKIENNGHI = 20,
                DTILE = 20,
                LIST_DATA = new List<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA>()
            });
            listKienNghiReport.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG()
            {
                CSTT = "2",
                CTENLINHVUC = "Lĩnh vực b",
                ITONGSOKIENNGHI = 80,
                DTILE = 80,
                LIST_DATA = new List<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA>()
            });

            listKienNghiReport.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG()
            {
                CSTT = "II",
                CTENLINHVUC = "Kinh tế Ngân sách",
                ITONGSOKIENNGHI = 70,
                DTILE = 70,
                LIST_DATA = new List<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA>()
            });
            listKienNghiReport.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG()
            {
                CSTT = "1",
                CTENLINHVUC = "Lĩnh vực 1",
                ITONGSOKIENNGHI = 40,
                DTILE = 40,
                LIST_DATA = new List<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA>()
            });
            listKienNghiReport.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG()
            {
                CSTT = "2",
                CTENLINHVUC = "Lĩnh vực 2",
                ITONGSOKIENNGHI = 30,
                DTILE = 30,
                LIST_DATA = new List<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA>()
            });

            Random rd = new Random();
            foreach (var item in listKienNghiReport)
            {
                item.LIST_DATA.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA() { DTILE = 10 + rd.Next(1, 10), ISOKIENNGHI = 15 + rd.Next(1, 10)});
                item.LIST_DATA.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA() { DTILE = 10 + rd.Next(1, 10), ISOKIENNGHI = 15 + rd.Next(1, 10)});
                item.LIST_DATA.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA() { DTILE = 10 + rd.Next(1, 10), ISOKIENNGHI = 15 + rd.Next(1, 10)});
                item.LIST_DATA.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_DATA() { DTILE = 10 + rd.Next(1, 10), ISOKIENNGHI = 15 + rd.Next(1, 10)});
            }

            var listKienNghiReportHeader = new List<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_HEADER>();
            listKienNghiReportHeader.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_HEADER { CTENDIAPHUONG = "Thành phố TH" });
            listKienNghiReportHeader.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_HEADER { CTENDIAPHUONG = "Thành phố Sầm Sơn" });
            listKienNghiReportHeader.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_HEADER { CTENDIAPHUONG = "Thành phố N" });
            listKienNghiReportHeader.Add(new KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_HEADER { CTENDIAPHUONG = "Thành phố Z" });

            XlsFile Result = new XlsFile(true);
            Result.Open(templatePath);
            FlexCelReport fr = new FlexCelReport();

            // set value header
            //var _columnCountBC1 = 3;
            //var columnStart = 0;
            //for (int i = 0; i < _columnCountBC1; i++)
            //{
            //    if (columnStart + i < listKienNghiReportHeader.Count)
            //    {
            //        fr.SetValue(string.Format("C{0}", i*2), listKienNghiReportHeader[i].CTENDIAPHUONG);
            //        fr.SetValue(string.Format("C{0}", i*2 + 1), listKienNghiReportHeader[i].CTENDIAPHUONG);
            //        fr.SetValue(string.Format("D{0}", i*2), listKienNghiReportHeader[i].SOKIENNGHI_NAME);
            //        fr.SetValue(string.Format("D{0}", i*2 + 1), listKienNghiReportHeader[i].TILE_NAME);
            //    }
            //    else
            //    {
            //        fr.SetValue(string.Format("C{0}", i*2), "");
            //        fr.SetValue(string.Format("C{0}", i*2 + 1), "");
            //        fr.SetValue(string.Format("D{0}", i*2), "");
            //        fr.SetValue(string.Format("D{0}", i*2 + 1), "");
            //    }
            //}

            fr.AddTable<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG>("List", listKienNghiReport);
            fr.AddTable<KIENNGHI_REPORT_TILE_DIAPHUONG_COTDONG_HEADER>("ListHeader", listKienNghiReportHeader);
            fr.UseForm(this).Run(Result);

            //do merge cell
            Result.MergeH(7, 5, 2*listKienNghiReportHeader.Count);

            return Result;

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

        public ActionResult Ajax_LoadOption_Thamquyenxuly(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                var coquan = _kiennghi.HienThiDanhSachCoQuan().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0 && x.IPARENT == id).ToList();
                Response.Write(kn.OptionThamQuyenDonVi_Parent_Child(coquan, id));
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Load option đơn vị thẩm quyền xử lý by id_parent");
                return null;
            }
        }
        public ActionResult Theodoi_luu()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                int iDonViXuLy = 0; int iDonViXuLy_Parent = 0;
                int iDonViTiepNhan = 0;
                int iUser = Convert.ToInt32(u_info.user_login.IUSER);
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                    iDonViTiepNhan = 0;
                }
                //base_business.ActionMulty_Redirect_("3,4,5", u_info.tk_action);
                int iKyHop = ID_KyHop_HienTai();
                KN_KIENNGHI kn_pram = get_Request_Paramt_KienNghi();
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (Request["q"] != null) { kn_pram.CNOIDUNG = func.RemoveTagInput(Request["q"]); }
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDoan"] != null) { iDonViTiepNhan = Convert.ToInt32(Request["iDoan"]); }
                string lstLinhVuc = "";
                if (Request["listLinhVuc"] != null) { lstLinhVuc = Request["listLinhVuc"]; }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();

                //if (!u_info.tk_action.is_lanhdao)
                //{
                //iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                //}
                kn_pram.IUSER = iUser;
                kn_pram.ITHAMQUYENDONVI = iDonViXuLy;
                kn_pram.IDONVITIEPNHAN = iDonViTiepNhan;
                kn_pram.IKYHOP = iKyHop;
                int iTruocKyHop = -1;
                if (Request["iTruocKyHop"] != null)
                {
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"].ToString());
                }
                kn_pram.ITRUOCKYHOP = iTruocKyHop;
                //if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
                //if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
                //if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
                //kn_pram.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiTrung_DongKienNghi;
                string listKyHop = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                string cmakiennghi = "";
                if (Request["cMaKienNghi"] != "" && Request["cMaKienNghi"] != null)
                {
                    cmakiennghi = Request["cMaKienNghi"];
                }
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["dBatDau"] != null && Request["dBatDau"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["dBatDau"]));
                }
                if (Request["dKetThuc"] != null && Request["dKetThuc"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["dKetThuc"]));
                }
                string lstNguonKN = "";
                if (Request["listNguonKienNghi"] != null)
                {
                    lstNguonKN = Request["listNguonKienNghi"];
                }
                var list_kn = _kiennghi.PRC_KIENNGHI_LIST_TRUNG(kn_pram, lstLinhVuc, listKyHop, cmakiennghi, lstNguonKN, dtungay, ddenngay, iDonViXuLy_Parent, page, post_per_page);
                //list_kn = list_kn.Where(x => x.ID_KIENNGHI_TRUNG != 0).OrderByDescending(x=>x.ID_LINHVUC).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenGiaiQuyet(iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    ViewData["opt-thamquyen-xuly"] = Get_Option_DonViThamQuyen_ByCType(iDonViXuLy_Parent, iDonViXuLy);
                }
                else
                {
                    //coquan = null;
                    ViewData["opt-thamquyen-xuly"] = Get_Option_DonViThamQuyen_ByCType();
                }
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["dbqh"] = "0";
                }
                else
                {
                    ViewData["dbqh"] = "1";
                }

                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-coquan"] = Get_Option_DonViTiepNhan(iDonViTiepNhan);

                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (list_kn.Count() > 0)
                {
                    htmlList = kn.KN_Theodoi_luu(list_kn);
                    htmlPhanTrang = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)list_kn.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";

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

                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách kiến nghị trùng, lưu theo dõi");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Kiennghi_xoa(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                Dictionary<string, object> dic_kn = new Dictionary<string, object>();
                dic_kn.Add("IKIENNGHI", id);
                List<KN_GIAMSAT> g = _kiennghi.GetAll_Giamsat_TraLoi_byParam(dic_kn);
                List<KN_KIENNGHI_TRALOI> t = _kiennghi.GetAll_TraLoi_KienNghi_ByParamt(dic_kn);
                if (g.Count() > 0)// xóa giám sát
                {
                    KN_GIAMSAT giamsat = g.FirstOrDefault();
                    _kiennghi.DeleteGiamSat_KienNghi(giamsat);
                }
                if (t.Count() > 0)//xóa trả lời
                {
                    KN_KIENNGHI_TRALOI giamsat = t.FirstOrDefault();
                    _kiennghi.DeleteTraLoi_KienNghi(giamsat);
                }
                KN_KIENNGHI kn = _kiennghi.HienThiThongTinKienNghi(id);
                if (kn.ID_GOP == -1)
                {//update lại trạng thái các kiến nghị đã gộp
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("ID_GOP", id);
                    var kn_gop = _kiennghi.HienThiDanhSachKienNghi(dic);
                    if (kn_gop.Count() > 0)
                    {
                        foreach (var k in kn_gop)
                        {
                            KN_KIENNGHI k1 = k;
                            k1.ID_GOP = 0;
                            _kiennghi.UpdateThongTinKienNghi(k1);
                        }
                    }
                }
                _kiennghi.DeleteThongTinKienNghi(kn);

                Response.Write(1);
                return null;

            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xóa kiến nghị");
                throw;
            }
        }
        public ActionResult Ajax_Kiennghi_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI kn = _kiennghi.HienThiThongTinKienNghi(id);
                if (kn.ID_GOP == -1)
                {//update lại trạng thái các kiến nghị đã gộp
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("ID_GOP", id);
                    var kn_gop = _kiennghi.HienThiDanhSachKienNghi(dic);
                    if (kn_gop.Count() > 0)
                    {
                        foreach (var k in kn_gop)
                        {
                            KN_KIENNGHI k1 = k;
                            k1.ID_GOP = 0;
                            _kiennghi.UpdateThongTinKienNghi(k1);
                        }
                    }
                }
                kn.IDELETE = 1;
                _kiennghi.DeleteThongTinKienNghi(kn);

                Response.Write(1);
                return null;

            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xóa kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_del_import(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI kn = _kiennghi.HienThiThongTinKienNghi(id);
                if (kn.ID_GOP == -1)
                {//update lại trạng thái các kiến nghị đã gộp
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("ID_GOP", id);
                    var kn_gop = _kiennghi.HienThiDanhSachKienNghi(dic);
                    if (kn_gop.Count() > 0)
                    {
                        foreach (var k in kn_gop)
                        {
                            KN_KIENNGHI k1 = k;
                            k1.ID_GOP = 0;
                            _kiennghi.UpdateThongTinKienNghi(k1);
                        }
                    }
                }
                if (kn.ID_IMPORT != null && kn.ID_IMPORT != 0)
                {
                    //update kn_import
                    KN_IMPORT ip = _kiennghi.Get_Import((int)kn.ID_IMPORT);
                    ip.ISOKIENNGHI--;
                    _kiennghi.Update_Import(ip);
                }
                kn.IDELETE = 1;
                _kiennghi.DeleteThongTinKienNghi(kn);

                Response.Write(1);
                return null;

            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xóa kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Remove_kiennghi_by_doan(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                Dictionary<string, object> dic_kn = new Dictionary<string, object>();
                dic_kn.Add("IKIENNGHI", id);
                var doan_kiennghi = _kiennghi.GetAll_DoanGiamSat_Kiengnhi(dic_kn);
                foreach (var d in doan_kiennghi)
                {
                    _kiennghi.DeleteDoan_kiennghi(d);
                }
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa kiến nghị trong kế hoạch giám sát");
                throw;
            }

        }
        public ActionResult Giamsat_ykien()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                int id_kiennghi = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id_kiennghi);
                ViewData["kiennghi"] = kiennghi;
                ViewData["detail"] = kn.KienNghi_Detail(id_kiennghi, Request.Cookies["url_key"].Value);
                KN_KIENNGHI kn_pram = new KN_KIENNGHI(); kn_pram.IKIENNGHI = id_kiennghi;
                var kn_traloi = _kiennghi.PRC_LIST_KN_TRALOI_DANHGIA(kn_pram);
                ViewData["bdn"] = 0;
                if (kn_traloi.Count() > 0)
                {
                    PRC_LIST_KN_TRALOI_DANHGIA traloi_danhgia = kn_traloi.FirstOrDefault();
                    ViewData["traloi"] = kn.Content_Traloi_kiennghi(traloi_danhgia);
                    if (u_info.tk_action.is_lanhdao)
                    {
                        ViewData["bdn"] = 1;
                        ViewData["giamsat"] = kn.Content_GiamSat_kiennghi(traloi_danhgia);
                    }
                }


                Dictionary<string, object> _kn = new Dictionary<string, object>();
                _kn.Add("IKIENNGHI", id_kiennghi);
                KN_DOANGIAMSAT_KIENNGHI doan_kiennghi = _kiennghi.GetAll_DoanGiamSat_Kiengnhi(_kn).FirstOrDefault();
                KN_DOANGIAMSAT doan = _kiennghi.Get_DoanGiamSat((int)doan_kiennghi.IDOANGIAMSAT);
                bool remove = true;
                if (doan.IUSER != u_info.tk_action.iUser && !u_info.tk_action.is_admin)
                {
                    ViewData["btn-add"] = " style='display:none' ";
                    remove = false;
                }
                var ykien = _kiennghi.GetAll_DoanGiamSat_Ykien(_kn);
                ViewData["id"] = Request["id"];
                ViewData["list"] = kn.List_Ykien_ByID_kiennghi(ykien, u_info.tk_action, remove, Request.Cookies["url_key"].Value);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách ý kiến của kiến nghị nằm trong kế hoạch giám sát");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Giamsat_ykien_add(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                SetTokenAction("Giamsat_ykien_add");
                ViewData["id"] = fc["id"];
                return PartialView("../Ajax/Kiennghi/Giamsat_ykien_add");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form thêm ý kiến của kiến nghị nằm trong kế hoạch giám sát");
                throw;
            }

        }
        public ActionResult Ajax_Giamsat_ykien_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = fc["id"];
                SetTokenAction("Giamsat_ykien_add", id);
                KN_DOANGIAMSAT_YKIEN ykien = _kiennghi.Get_DoanGiamSat_Ykien(id);
                ViewData["ykien"] = ykien;
                ViewData["file"] = kn.File_Edit(id, "giamsat_ykien", Request.Cookies["url_key"].Value);
                return PartialView("../Ajax/Kiennghi/Giamsat_ykien_edit");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form sửa ý kiến của kiến nghị nằm trong kế hoạch giám sát");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Giamsat_ykien_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Giamsat_ykien_add", id)) { Response.Redirect("/Home/Error/"); return null; }

                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }

                }
                UserInfor u_info = GetUserInfor();

                KN_DOANGIAMSAT_YKIEN y = _kiennghi.Get_DoanGiamSat_Ykien(id);

                y.DNGAYLAMVIEC = Convert.ToDateTime(func.ConvertDateToSql(fc["DNGAYLAMVIEC"]));
                y.CTEN = func.RemoveTagInput(fc["cTen"]);
                y.CNOIDUNG = func.RemoveTagInput(fc["CNOIDUNG"]);
                _kiennghi.UpdateDoan_ykien(y);

                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "giamsat_ykien";
                        f.CFILE = UploadFile(file);
                        f.ID = id;
                        kntc.Upload_file(f);
                    }
                }
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Update ý kiến của kiến nghị nằm trong kế hoạch giám sát");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Giamsat_ykien_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                if (!CheckTokenAction("Giamsat_ykien_add")) { Response.Redirect("/Home/Error/"); return null; }
                int id_kiennghi = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }

                }
                UserInfor u_info = GetUserInfor();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("IKIENNGHI", id_kiennghi);
                KN_DOANGIAMSAT_KIENNGHI doan_kiennghi = _kiennghi.GetAll_DoanGiamSat_Kiengnhi(dic).FirstOrDefault();
                KN_DOANGIAMSAT_YKIEN y = new KN_DOANGIAMSAT_YKIEN();
                y.IUSER = u_info.tk_action.iUser;
                y.DDATE = DateTime.Now;
                y.DNGAYLAMVIEC = Convert.ToDateTime(func.ConvertDateToSql(fc["DNGAYLAMVIEC"]));
                y.CTEN = func.RemoveTagInput(fc["cTen"]);
                y.CNOIDUNG = func.RemoveTagInput(fc["CNOIDUNG"]);
                y.IKIENNGHI = id_kiennghi;
                y.IDOANGIAMSAT = (int)doan_kiennghi.IDOANGIAMSAT;
                _kiennghi.InsertDoan_ykien(y);
                int id_ykien = (int)y.IYKIEN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "giamsat_ykien";
                        f.CFILE = UploadFile(file);
                        f.ID = id_ykien;
                        kntc.Upload_file(f);
                    }
                }
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Insert ý kiến của kiến nghị nằm trong kế hoạch giám sát");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Giamsat_kiennghi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                KN_DOANGIAMSAT doan = _kiennghi.Get_DoanGiamSat(id);
                bool remove = true;
                ViewData["themkiennghi"] = "ShowTimKiem_Conf('id=" + Request["id"] + "','/Kiennghi/Ajax_Themkiennghi_giamsat/','search_place')";
                if (doan.IUSER != u_info.tk_action.iUser && !u_info.tk_action.is_admin)
                {
                    //ViewData["btn-add"] = " style='display:none' ";
                    ViewData["themkiennghi"] = "";
                    remove = false;
                }
                //ViewData["id"] = Request["id"];
                Dictionary<string, object> dic_kn = new Dictionary<string, object>();
                //dic_kn.Add("ITINHTRANG", (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet);
                var kiennghi = _kiennghi.HienThiDanhSachKienNghi(dic_kn);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("IDOANGIAMSAT", id);
                var doan_kiennghi = _kiennghi.GetAll_DoanGiamSat_Kiengnhi(dic);
                ViewData["list"] = kn.List_KienNghi_ByID_doangiamsat(kiennghi, doan_kiennghi, u_info.tk_action, remove, Request.Cookies["url_key"].Value);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách kiến nghị nằm trong kế hoạch giám sát");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Giamsat_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_DOANGIAMSAT doan = _kiennghi.Get_DoanGiamSat(id);

                _kiennghi.Tracking(GetUserInfor().tk_action.iUser, "Xóa kế hoạch giám sát: " + doan.CTEN);
                _kiennghi.Delete_DoanGiamSat(doan);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xóa kế hoạch giám sát");
                throw;
            }

        }
        public ActionResult Ajax_Chonkiennghi_cungnoidung(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }

            try
            {
                UserInfor u_info = GetUserInfor();
                int id_kiennghi = Convert.ToInt32(fc["id_kiennghi"]);
                int id_kiennghi_parent = Convert.ToInt32(fc["id_kiennghi_parent"]);
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id_kiennghi);
                if (kiennghi.ID_KIENNGHI_PARENT == 0)
                {
                    kiennghi.ID_KIENNGHI_PARENT = id_kiennghi_parent;
                }
                else
                {
                    kiennghi.ID_KIENNGHI_PARENT = 0;
                }
                _kiennghi.UpdateThongTinKienNghi(kiennghi);

                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Kiểm trùng nội dung trong Tập hợp");
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Tonghop_chontrung(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Tonghop_chontrung", id)) { Response.Redirect("/Home/Error/"); return null; }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                kiennghi.CNOIDUNG_TRUNG = func.RemoveTagInput(fc["CNOIDUNG_TRUNG"]);
                _kiennghi.UpdateThongTinKienNghi(kiennghi);
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, id, "Cập nhật nội dung chung cho các kiến nghị cùng nội dung");
                Response.Redirect("/Kiennghi/Tonghop_chontrung/?id=" + fc["id"] + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Cập nhật nội dung trùng trong Tập hợp");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Tonghop_chontrung()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                if (kiennghi.CNOIDUNG_TRUNG == null)
                {
                    kiennghi.CNOIDUNG_TRUNG = kiennghi.CNOIDUNG;
                    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                }
                SetTokenAction("Tonghop_chontrung", id);
                ViewData["id"] = Request["id"];
                ViewData["kiennghi"] = kiennghi;
                ViewData["detail"] = kn.KienNghi_Detail(id, Request.Cookies["url_key"].Value);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ITONGHOP_BDN", kiennghi.ITONGHOP_BDN);
                //dic.Add("ID_KIENNGHI_PARENT", 0);
                var list = _kiennghi.HienThiDanhSachKienNghi(dic);
                Dictionary<string, object> dic_coquan = new Dictionary<string, object>();
                List<QUOCHOI_COQUAN> coquan = _kiennghi.GetAll_CoQuanByParam(dic_coquan).ToList();
                ViewData["list"] = kn.List_KienNghi_KiemTrungNoiDung(list, coquan, id);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Kiểm trùng nội dung trong Tập hợp");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Tonghop_info()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                UserInfor u_info = new UserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                ViewData["tonghop"] = tonghop;
                TongHop_Kiennghi detail = kn.Tonghop_Detail(id, "");
                ViewData["detail"] = detail;
                ViewData["file"] = kn.File_View(id, "kn_tonghop");
                List<PRC_KIENNGHI_BYTONGHOP> kiennghi;

                if (tonghop.IDONVITONGHOP == ID_Ban_DanNguyen)
                {
                    kiennghi = _kiennghi.PRC_LIST_KIENNGHI_BY_TONGHOP_KIENNGHI(0, id);
                }
                else
                {
                    kiennghi = _kiennghi.PRC_LIST_KIENNGHI_BY_TONGHOP_KIENNGHI(id, 0);
                }
                ViewData["list"] = kn.List_KienNghi_ByID_tonghop_view_new(tonghop, kiennghi, Request.Cookies["url_key"].Value);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thông tin chi tiết Tập hợp kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tonghop_chonkiennghi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                bool remove = true;
                ViewData["btn-gop"] = "<a href='javascript:void(0)' onclick=\"Gop_KienNghi()\" title='Gộp kiến nghị'><i class='icon-signin'></i></a>";
                if (tonghop.IUSER != u_info.tk_action.iUser && !u_info.tk_action.is_admin)
                {
                    ViewData["btn-add"] = " style='display:none' ";
                    ViewData["btn-gop"] = "";
                    remove = false;
                }
                ViewData["id"] = Request["id"];
                ViewData["tonghop"] = tonghop;
                TongHop_Kiennghi detail = kn.Tonghop_Detail(id, "");
                ViewData["detail"] = detail;
                ViewData["file"] = kn.File_View(id, "kn_tonghop");
                List<PRC_KIENNGHI_BYTONGHOP> kiennghi;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if (u_info.tk_action.is_dbqh)
                {
                    kiennghi = _kiennghi.PRC_LIST_KIENNGHI_BY_TONGHOP_KIENNGHI(id, 0); ViewData["btn-gop"] = "";
                }
                else
                {
                    kiennghi = _kiennghi.PRC_LIST_KIENNGHI_BY_TONGHOP_KIENNGHI(id, 0);
                }
                var kienNghiChon = _kiennghi.HienThiThongTinKienNghi((int)kiennghi.FirstOrDefault().IKIENNGHI);
                dic.Add("ICOQUAN", kienNghiChon.ITHAMQUYENDONVI);
                QUOCHOI_COQUAN coquanchon = _kiennghi.GetAll_CoQuanByParam(dic).FirstOrDefault();
                string chuyen_choxuly = "<a href='/Kiennghi/Tonghop_TW/'  class='btn btn-success'>Chuyển chờ xử lý</a> ";
                if (coquanchon.CTYPE == StringEnum.GetStringValue(ThamQuyen_DiaPhuong.Tinh))
                    chuyen_choxuly = "<a href='/Kiennghi/Tonghop_Tinh/'  class='btn btn-success'>Chuyển chờ xử lý</a> ";
                if (coquanchon.CTYPE == StringEnum.GetStringValue(ThamQuyen_DiaPhuong.Huyen))
                    chuyen_choxuly = "<a href='/Kiennghi/Tonghop_Huyen/'  class='btn btn-success'>Chuyển chờ xử lý</a> ";
                if (kiennghi == null) { ViewData["btn-gop"] = ""; }
                ViewData["list"] = kn.List_KienNghi_ByID_tonghop(kiennghi, u_info.tk_action, remove, Request.Cookies["url_key"].Value);
                string chuyen_xuly = "<a onclick=\"ShowPopUp('id=" + Request["id"] + "','/Kiennghi/Ajax_Chuyen_Donvi_xuly')\" href='javascript:void()'  class='btn btn-success'>Chuyển đơn vị thẩm quyền xử lý</a>";

                //if (u_info.tk_action.is_dbqh)
                //{
                //    chuyen_xuly = "<a onclick=\"ShowPopUp('id=" + Request["id"] + "','/Kiennghi/Ajax_Chuyen_Xuly_tonghop')\" href='javascript:void()'  class='btn btn-success'>Chuyển đơn vị thẩm quyền xử lý</a>";
                //    chuyen_choxuly = "<a href='/Kiennghi/Tonghop/'  class='btn btn-success'>Chuyển chờ xử lý</a>";
                //}
                if (!base_business.Action_(6, u_info.tk_action)) { chuyen_xuly = ""; }
                if (kiennghi.Count() == 0)
                {
                    chuyen_xuly = "";
                }
                ViewData["chuyen_xuly"] = chuyen_xuly;
                ViewData["chuyen_choxuly"] = chuyen_choxuly;
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Chọn kiến nghị đưa vào Tập hợp kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Remove_ykien(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_DOANGIAMSAT_YKIEN ykien = _kiennghi.Get_DoanGiamSat_Ykien(id);
                int iKienNghi = (int)ykien.IKIENNGHI;
                _kiennghi.DeleteDoan_ykien(ykien);
                _kiennghi.Tracking_KN(GetUserInfor().tk_action.iUser, iKienNghi, "Hủy ý kiến giám sát");
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xóa ý kiến giám sát kiến nghị trong kế hoạch giám sát");
                throw;
            }

        }
        public ActionResult Ajax_Remove_from_kiennghi_gop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(fc["id"]);
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                kiennghi.ID_GOP = 0;
                _kiennghi.UpdateThongTinKienNghi(kiennghi);
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, id, "Bỏ chọn kiến nghị trong kiến nghị gộp");
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Bỏ chọn kiến nghị trong kiến nghị gộp");
                throw;
            }

        }

        public ActionResult Ajax_Remove_kiennghi_by_tonghop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(fc["id"]);
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                if (u_info.tk_action.is_dbqh)
                {
                    kiennghi.ITONGHOP = 0;
                }
                if (u_info.tk_action.is_lanhdao)
                {
                    kiennghi.ITONGHOP_BDN = 0;
                }

                kiennghi.ITINHTRANG = 0;
                kiennghi.ID_KIENNGHI_PARENT = 0;
                _kiennghi.UpdateThongTinKienNghi(kiennghi);
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, id, "Bỏ chọn Tập hợp kiến nghị");
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Bỏ chọn kiến nghị trong Tập hợp kiến nghị");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Chontonghop(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                if (!CheckTokenAction("kn_chontonghop"))
                {
                    Response.Redirect("/Home/Error/"); return null;
                }
                //for (int i = 1; i < 4; i++)
                //{
                //    file = Request.Files["file_upload" + i];
                //    if (file != null && file.ContentLength > 0)
                //    {
                //        if (!CheckFile_Upload(file))
                //        {
                //            Response.Redirect("/Home/Error/?type=type"); return null;
                //        }
                //    }

                //}
                UserInfor u_info = GetUserInfor();
                KN_TONGHOP t = new KN_TONGHOP();
                t.ICHUONGTRINH = 0;
                //t.IDONVITONGHOP = Convert.ToInt32(fc["iDonViTongHop"]);
                t.IDONVITONGHOP = u_info.user_login.IDONVI;
                //t.ITHAMQUYENDONVI = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                t.ITHAMQUYENDONVI = Get_IDThamQuyenDonVi_From_ID_Session_KienNghi_ChonTongHop();
                //t.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                t.ILINHVUC = 0;
                t.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                //t.CTUKHOA = func.RemoveTagInput(fc["cTuKhoa"]);
                t.CTUKHOA = "";
                //t.IKYHOP = Convert.ToInt32(fc["iKyHop"]);
                t.IKYHOP = Get_IDKyHop_From_ID_Session_KienNghi_ChonTongHop();
                t.IUSER = u_info.tk_action.iUser;
                t.DDATE = DateTime.Now;
                t.ITINHTRANG = (decimal)TrangThai_TongHop.ChoXuLy;
                t.CMATONGHOP = "";
                t.ID_IMPORT = 0;
                //if (fc["iTruocKyHop"] != null)
                //{
                //    t.ITRUOCKYHOP = 1;
                //}
                //else
                //{
                //    t.ITRUOCKYHOP = 0;
                //}
                t.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]);
                _kiennghi.InsertTongHop(t);
                int id = (int)t.ITONGHOP;
                //for (int i = 1; i < 4; i++)
                //{
                //    file = Request.Files["file_upload" + i];
                //    if (file != null && file.ContentLength > 0)
                //    {
                //        FILE_UPLOAD f = new FILE_UPLOAD();
                //        f.CTYPE = "kn_tonghop";
                //        f.CFILE = UploadFile(file);
                //        f.ID = id;
                //        kntc.Upload_file(f);
                //    }

                //}
                _kiennghi.Tracking_Tonghop(u_info.tk_action.iUser, id, "Thêm Tập hợp kiến nghị");
                List<ID_Session_KienNghi_ChonTongHop> list_session_id_kiennghi = Get_IDKienNghiChonTongHop();
                if (list_session_id_kiennghi != null)
                {
                    foreach (var l in list_session_id_kiennghi)
                    {
                        KN_KIENNGHI kn = _kiennghi.HienThiThongTinKienNghi((int)l.IKIENNGHI);
                        kn.ITINHTRANG = (decimal)TrangThaiKienNghi.Choxuly;
                        kn.ITONGHOP = id;
                        _kiennghi.UpdateThongTinKienNghi(kn);
                    }
                    RemoveAll_IDKienNghiChonTongHop();
                }
                string id_enct = HashUtil.Encode_ID(id.ToString(), Request.Cookies["url_key"].Value);
                Response.Redirect("/Kiennghi/Tonghop_chonkiennghi/?id=" + id_enct + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Insert Tập hợp kiến nghị");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Removessionkiennghi()
        {
            RemoveAll_IDKienNghiChonTongHop();
            return null;
        }
        public int Get_IDLinhVuc_From_ID_Session_KienNghi_ChonTongHop()
        {
            int id = 0;
            List<ID_Session_KienNghi_ChonTongHop> list = Get_IDKienNghiChonTongHop();
            if (list != null)
            {
                List<decimal> list_id_linhvuc = new List<decimal>();
                foreach (var l in list)
                {
                    if (!list_id_linhvuc.Contains(l.ILINHVUC)) { list_id_linhvuc.Add(l.ILINHVUC); }
                }
                if (list_id_linhvuc.Count() == 1)
                {
                    id = (int)list_id_linhvuc[0];
                }
            }
            return id;
        }
        public int Get_IDThamQuyenDonVi_From_ID_Session_KienNghi_ChonTongHop()
        {
            int id = 0;
            List<ID_Session_KienNghi_ChonTongHop> list = Get_IDKienNghiChonTongHop();
            if (list != null)
            {
                List<decimal> list_id_linhvuc = new List<decimal>();
                foreach (var l in list)
                {
                    if (!list_id_linhvuc.Contains(l.ITHAMQUYENDONVI)) { list_id_linhvuc.Add(l.ITHAMQUYENDONVI); }
                }
                if (list_id_linhvuc.Count() == 1)
                {
                    id = (int)list_id_linhvuc[0];
                }
            }
            return id;
        }
        public int Get_IDKyHop_From_ID_Session_KienNghi_ChonTongHop()
        {
            int id = ID_KyHop_HienTai();
            List<ID_Session_KienNghi_ChonTongHop> list = Get_IDKienNghiChonTongHop();
            if (list.Count() > 0)
            {
                decimal id_kiennghi = list.FirstOrDefault().IKIENNGHI;
                KN_KIENNGHI kn = _kiennghi.HienThiThongTinKienNghi((int)id_kiennghi);
                id = (int)kn.IKYHOP;
            }
            return id;
        }
        public string Get_NoiDung_From_ID_Session_KienNghi_ChonTongHop()
        {
            List<ID_Session_KienNghi_ChonTongHop> list = Get_IDKienNghiChonTongHop();
            string str = "";
            if (list != null)
            {
                foreach (var l in list)
                {
                    KN_KIENNGHI kn = _kiennghi.HienThiThongTinKienNghi((int)l.IKIENNGHI);
                    str += Server.HtmlEncode(kn.CNOIDUNG) + "\n";
                }
            }
            return str;
        }
        public int CheckListSessionKienNghiChon()
        {
            List<ID_Session_KienNghi_ChonTongHop> list = Get_IDKienNghiChonTongHop();
            if (list.Count() > 0)
            {
                List<decimal> list_id_thamquyen = new List<decimal>();
                foreach (var l in list)
                {
                    KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)l.IKIENNGHI);
                    if (kiennghi != null)
                    {
                        if (!list_id_thamquyen.Contains((decimal)kiennghi.ITHAMQUYENDONVI))
                        {
                            list_id_thamquyen.Add((decimal)kiennghi.ITHAMQUYENDONVI);
                        }
                    }
                }
                return list_id_thamquyen.Count();
            }
            else
            {
                return 0;
            }
        }
        public ActionResult Ajax_Checkkiennghichontonghop()
        {
            if (!CheckAuthToken()) { return null; }
            int count_donvi = CheckListSessionKienNghiChon();
            if (count_donvi > 1)
            {
                Response.Write(0);
            }
            else
            {
                if (count_donvi == 0)//chưa chọn kiến nghị nào
                {
                    Response.Write(-1);
                }
                else
                {
                    Response.Write(1);
                }
            }

            return null;
        }
        public ActionResult Ajax_Check_Listkiennghi_gop()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                List<ID_Session_KienNghi_ChonTongHop> list = Get_IDKienNghiChonTongHop();
                if (list.Count() > 1)// có trên 2 kiến nghị
                {
                    int count_thamquyen = CheckListSessionKienNghiChon();
                    if (count_thamquyen > 1)
                    {
                        Response.Write(2);
                    }
                    else
                    {
                        Response.Write(1);
                    }
                }
                else
                {
                    Response.Write(0);
                }
            }
            catch
            {
                Response.Write(0);
            }
            return null;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_gop(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            if (!CheckTokenAction("gop_kiennghi"))
            {
                Response.Redirect("/Home/Error/");
                return null;
            }
            try
            {
                int id_kiennghi_first;
                string id = fc["id"];
                List<int> list_id_kiennghi = new List<int>();
                if (id != "")
                {
                    id_kiennghi_first = Convert.ToInt32(id.Split(',')[0]);
                    foreach (var l in id.Split(','))
                    {
                        if (l != "")
                        {
                            list_id_kiennghi.Add(Convert.ToInt32(l));
                            //id_kiennghi_first = Convert.ToInt32(l);
                        }
                    }

                }
                else
                {
                    List<ID_Session_KienNghi_ChonTongHop> list = Get_IDKienNghiChonTongHop();
                    if (list == null)
                    {
                        Response.Redirect("/Home/Error/");
                        return null;
                    }
                    id_kiennghi_first = (int)list.FirstOrDefault().IKIENNGHI;
                    foreach (var l in list)
                    {
                        list_id_kiennghi.Add((int)l.IKIENNGHI);
                    }
                }
                KN_KIENNGHI kn_session = _kiennghi.HienThiThongTinKienNghi(id_kiennghi_first);
                int iUser = GetUserInfor().tk_action.iUser;
                int IDONVITIEPNHAN = (int)GetUserInfor().user_login.IDONVI;
                int iKyHop = (int)kn_session.IKYHOP;
                KN_KIENNGHI kn = new KN_KIENNGHI();
                kn.IKYHOP = iKyHop;
                kn.IDIAPHUONG0 = 0;
                kn.IDIAPHUONG1 = 0;
                kn.CDIACHI = "";
                kn.ID_GOP = -1;
                kn.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                kn.ID_IMPORT = 0;
                kn.IDELETE = 0;
                kn.CTUKHOA = "";
                kn.DDATE = DateTime.Now;
                kn.ICHUONGTRINH = 0;
                kn.IDONVITIEPNHAN = IDONVITIEPNHAN;
                kn.IKIEMTRATRUNG = 0;
                kn.IKIENNGHI_TRUNG = 0;
                kn.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                kn.ITHAMQUYENDONVI = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                kn.ITINHTRANG = (decimal)TrangThaiKienNghi.Moicapnhat;
                kn.ITONGHOP = 0;
                if (id != "")//gộp từ Tập hợp
                {
                    //KN_KIENNGHI kn_ss = _kiennghi.HienThiThongTinKienNghi(id_kiennghi_first);
                    kn.ITONGHOP_BDN = kn_session.ITONGHOP_BDN;
                }
                else
                {
                    kn.ITONGHOP_BDN = 0;
                }
                kn.IPARENT = 0;
                //kn.ITONGHOP_BDN = 0;
                kn.ID_KIENNGHI_PARENT = 0;
                kn.ITRUOCKYHOP = kn_session.ITRUOCKYHOP;
                kn.IUSER = iUser;
                _kiennghi.InsertKienNghi(kn);
                int iKienNghi = (int)kn.IKIENNGHI;

                _kiennghi.Tracking_KN(iUser, iKienNghi, "Thêm mới kiến nghị gộp");
                string id_ecr = HashUtil.Encode_ID(iKienNghi.ToString(), Request.Cookies["url_key"].Value);
                foreach (var k in list_id_kiennghi)
                {
                    int id_kiennghi = k;
                    KN_KIENNGHI kn_ss = _kiennghi.HienThiThongTinKienNghi(id_kiennghi);
                    if (kn_ss.ID_GOP == 0)
                    {
                        kn_ss.ID_GOP = iKienNghi;
                        kn_ss.ILINHVUC = kn.ILINHVUC;
                        kn_ss.ITHAMQUYENDONVI = kn.ITHAMQUYENDONVI;
                        _kiennghi.UpdateThongTinKienNghi(kn_ss);
                        _kiennghi.Tracking_KN(iUser, id_kiennghi, "Chọn thêm vào kiến nghị gộp");
                    }

                }
                RemoveAll_IDKienNghiChonTongHop();
                Response.Redirect(Request.Cookies["url_return"].Value);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Insert kiến nghị gộp");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Add_Kiennghi_gop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                SetTokenAction("gop_kiennghi");
                UserInfor u_info = GetUserInfor();
                if (fc["id"] != null)
                {
                    string id = fc["id"];
                    ViewData["id"] = id;
                    int ID_Kiennghi = 0;
                    string noidung_gop = "";
                    int iDonVi = 0;
                    int iLinhVuc = 0;
                    foreach (var i in id.Split(','))
                    {
                        if (i != "" && _kiennghi.HienThiThongTinKienNghi(Convert.ToInt32(i)) != null)
                        {
                            ID_Kiennghi = Convert.ToInt32(i);
                            KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(ID_Kiennghi);
                            if (kiennghi != null)
                            {
                                iDonVi = (int)kiennghi.ITHAMQUYENDONVI;
                                iLinhVuc = (int)kiennghi.ILINHVUC;
                                noidung_gop += kiennghi.CNOIDUNG + "\n";
                            }
                        }
                    }
                    QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(iDonVi);
                    ViewData["opt-donvithamquyen"] = "<option value='" + iDonVi + "'>" + Server.HtmlEncode(coquan.CTEN) + "</option>";
                    ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan(iDonVi, iLinhVuc);
                }
                else
                {
                    List<ID_Session_KienNghi_ChonTongHop> list = Get_IDKienNghiChonTongHop();
                    ViewData["id"] = "";
                    int iDonVi = Convert.ToInt32(list.FirstOrDefault().ITHAMQUYENDONVI);
                    int iLinhVuc = 0;
                    if (list.Where(x => x.ILINHVUC != 0).Count() > 0)
                    {
                        iLinhVuc = (int)list.Where(x => x.ILINHVUC != 0).FirstOrDefault().ILINHVUC;
                    }
                    KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)list.FirstOrDefault().IKIENNGHI);
                    ViewData["noidung"] = kiennghi.CNOIDUNG;
                    QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(iDonVi);
                    ViewData["opt-donvithamquyen"] = "<option value='" + iDonVi + "'>" + Server.HtmlEncode(coquan.CTEN) + "</option>";
                    ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan(iDonVi, iLinhVuc);
                }
                return PartialView("../Ajax/Kiennghi/Add_Kiennghi_gop");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form gộp kiến nghị");
                throw;
            }

        }
        public ActionResult Chontonghop()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                    SetTokenAction("kn_chontonghop");
                UserInfor u_info = GetUserInfor();
                if (!base_business.Action_(5, u_info.tk_action))
                {
                    Response.Redirect("/Home/Error/");
                }
                if (Get_IDKienNghiChonTongHop().Count() == 0 && u_info.tk_action.is_lanhdao)
                {//chưa chọn kiến nghị thêm vào Tập hợp (BDN)
                    Response.Redirect("/Home/Error/");
                }
                int iKyHop = ID_KyHop_HienTai();
                Dictionary<string, object> donvi = new Dictionary<string, object>();
                //List<QUOCHOI_COQUAN> coquan = _kiennghi.GetAll_CoQuanByParam(donvi).Where(x => x.IHIENTHI == 1).OrderBy(x => x.CTEN).ToList();
                int iDonVi = (int)u_info.user_login.IDONVI;
                //ViewData["opt-donvitonghop"] = "<option value='" + iDonVi + "'>" + u_info.tk_action.tendonvi + "</option>";
                int iThamQuyenDonVi = ID_Ban_DanNguyen;
                int iLinhVuc = 0;
                if (Get_IDThamQuyenDonVi_From_ID_Session_KienNghi_ChonTongHop() != 0) { iThamQuyenDonVi = Get_IDThamQuyenDonVi_From_ID_Session_KienNghi_ChonTongHop(); }
                if (Get_IDLinhVuc_From_ID_Session_KienNghi_ChonTongHop() != 0) { iLinhVuc = Get_IDLinhVuc_From_ID_Session_KienNghi_ChonTongHop(); }
                //List<LINHVUC> linhvuc = _kiennghi.GetAll_LinhVuc();               
                //ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);                
                ViewData["opt-donvitonghop"] = "<option value='" + iDonVi + "'>" + Server.HtmlEncode(u_info.tk_action.tendonvi) + "</option>";
                //ViewData["noidung_tonghop"] = Get_NoiDung_From_ID_Session_KienNghi_ChonTongHop();
                ViewData["noidung_tonghop"] = "";
                //List<KN_CHUONGTRINH> chuongtrinh = _kiennghi.GetAll_ChuongTrinh(iDonVi, iKyHop);
                //ViewData["opt-kehoach"] = kn.Option_KeHoach_ByKyHop(chuongtrinh, 0);
                if (u_info.tk_action.is_dbqh)
                {
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen(ID_Ban_DanNguyen);
                }
                else
                {
                    List<ID_Session_KienNghi_ChonTongHop> list = Get_IDKienNghiChonTongHop();
                    if (list.Count() == 0)
                    {
                        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen(ID_Ban_DanNguyen);
                    }
                    else
                    {
                        iThamQuyenDonVi = Get_IDThamQuyenDonVi_From_ID_Session_KienNghi_ChonTongHop();
                        QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(iThamQuyenDonVi);
                        ViewData["opt-donvithamquyen"] = "<option value='" + iThamQuyenDonVi + "'>" + Server.HtmlEncode(coquan.CTEN) + "</option>";
                    }
                }

                /*
                List<ID_Session_KienNghi_ChonTongHop> list = Get_IDKienNghiChonTongHop();
                if (list.Count() == 0)
                {
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen(ID_Ban_DanNguyen);
                }else
                {
                    iThamQuyenDonVi = Get_IDThamQuyenDonVi_From_ID_Session_KienNghi_ChonTongHop();
                    QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan(iThamQuyenDonVi);
                    ViewData["opt-donvithamquyen"] = "<option value='"+iThamQuyenDonVi+"'>"+coquan.CTEN+"</option>";
                }
                */
                if (u_info.tk_action.is_lanhdao)
                {
                    //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan(iThamQuyenDonVi, iLinhVuc);
                    ViewData["dbqh"] = 0;
                }
                else//đbqh
                {
                    ViewData["dbqh"] = 1;
                    ViewData["donvithamquyen"] = GetRadioButton_ThamQuyenTongHop(0);
                }
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form thêm Tập hợp kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public string GetRadioButton_ThamQuyenTongHop(int id_thamquyen)
        {
            string str = "";
            UserInfor u_info = GetUserInfor();
            if (id_thamquyen == ID_Ban_DanNguyen || id_thamquyen == 0)
            {
                str = "<span class='span12 nopadding'><input class='nomargin' type='radio' checked id='donvi_" + ID_Ban_DanNguyen + "' name='iThamQuyenDonVi' value='" + ID_Ban_DanNguyen + "' /> " +
                     "<label for='donvi_" + ID_Ban_DanNguyen + "'> Uỷ ban Nhân dân Tỉnh</label> </span>" +
                     "<span class='span12 nopadding nomargin'><input class='nomargin' type='radio' id='donvi_" + u_info.user_login.IDONVI + "' name='iThamQuyenDonVi' value='" + u_info.user_login.IDONVI + "' /> " +
                     "<label for='donvi_" + u_info.user_login.IDONVI + "'>Ban Dân Nguyện</label> </span>";
            }
            else
            {
                str = "<span class='span12 nopadding'><input class='nomargin' type='radio' id='donvi_" + ID_Ban_DanNguyen + "' name='iThamQuyenDonVi' value='" + ID_Ban_DanNguyen + "' /> " +
                     "<label for='donvi_" + ID_Ban_DanNguyen + "'> Uỷ ban Nhân dân Tỉnh</label> </span>" +
                     "<span class='span12 nopadding nomargin'><input class='nomargin' type='radio' id='donvi_" + u_info.user_login.IDONVI + "' name='iThamQuyenDonVi' checked value='" + u_info.user_login.IDONVI + "' /> " +
                     "<label for='donvi_" + u_info.user_login.IDONVI + "'>Ban Dân Nguyện</label> </span>";
            }
            //if (u_info.user_login.IDONVI == id_thamquyen)
            //{

            //}
            //else
            //{

            //}
            return str;
        }
        public string Get_OptionDonViThamQuyen_TongHop(int iDonVi)
        {
            UserInfor u_info = GetUserInfor();
            string str = "";
            if (u_info.tk_action.is_dbqh)
            {
                string select_diaphuong = "";
                if (iDonVi == u_info.user_login.IDONVI) { select_diaphuong = " selected "; }
                str = "<option value='" + ID_Ban_DanNguyen + "'>" +
                                        _kiennghi.HienThiThongTinCoQuan(ID_Ban_DanNguyen).CTEN + "</option>" +
                                        "<option " + select_diaphuong + " value='" + u_info.user_login.IDONVI + "'>" +
                                        Server.HtmlEncode(u_info.tk_action.tendonvi) + "</option>";
            }
            else
            {
                str = Get_Option_DonViThamQuyen(iDonVi);
            }
            return str;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Themkiennghi_tonghop_search_key(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                if (u_info == null) { return null; }
                string cNoiDung_ = func.RemoveTagInput(fc["q"]);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (u_info.tk_action.is_dbqh)
                {
                    dic.Add("ITINHTRANG", (decimal)TrangThaiKienNghi.Moicapnhat);
                    dic.Add("ITONGHOP", 0);
                }
                else
                {
                    dic.Add("ITONGHOP_BDN", 0);
                }
                List<KN_KIENNGHI> kiennghi = _kiennghi.HienThiDanhSachKienNghi(dic);
                if (u_info.tk_action.is_lanhdao)
                {
                    kiennghi = kiennghi.Where(x => x.ITINHTRANG == (decimal)TrangThaiKienNghi.Moicapnhat ||
                                x.ITINHTRANG == (decimal)TrangThaiKienNghi.Choxuly).OrderBy(x => x.CNOIDUNG).ToList();
                }
                if (cNoiDung_ != "") { kiennghi = kiennghi.Where(x => x.CNOIDUNG.Contains(cNoiDung_)).ToList(); }
                Dictionary<string, object> dic_coquan = new Dictionary<string, object>();
                //dic_coquan.Add("IPARENT", ID_Coquan_doandaibieu);
                List<QUOCHOI_COQUAN> coquan = _kiennghi.GetAll_CoQuanByParam(dic_coquan).Where(x => x.IHIENTHI == 1).ToList();
                if (kiennghi.Count() > 0)
                {
                    Response.Write("<tr><td colspan='4' class='alert-info tcenter'>Có " + kiennghi.Count() + " kết quả tìm kiếm.</td></tr>");
                }
                //ViewData["list"] = kn.KN_ListKienNghiThemVaoTongHop(kiennghi, id, coquan, u_info.tk_action);
                return PartialView("../Ajax/Kiennghi/Themkiennghi_tonghop_search");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiếm kiến nghị thêm vào Tập hợp kiến nghị");
                throw;
            }


        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Themkiennghi_gop_search(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                if (u_info == null) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI k_ = new KN_KIENNGHI();
                int iKyHop_ = ID_KyHop_HienTai();
                if (fc["iKyHop_"] != null) { iKyHop_ = Convert.ToInt32(fc["iKyHop_"]); }
                k_.IKYHOP = iKyHop_;
                if (fc["q"] != null) { k_.CNOIDUNG = func.RemoveTagInput(fc["q"]); }
                if (fc["cNoiDung_"] != null) { k_.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung_"]); }
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_dbqh)
                {
                    k_.IDONVITIEPNHAN = (int)u_info.user_login.IDONVI;
                }
                else
                {
                    if (fc["iDonViTiepNhan_"] != null && Convert.ToInt32(fc["iDonViTiepNhan_"]) != 0)
                    {
                        k_.IDONVITIEPNHAN = Convert.ToInt32(fc["iDonViTiepNhan_"]);
                    }
                    if (base_business.Action_(50, u_info.tk_action))
                    {
                        iUser = 0;
                    }
                }
                k_.IUSER = iUser;
                if (fc["iThamQuyenDonVi_"] != null && Convert.ToInt32(fc["iThamQuyenDonVi_"]) != -1)
                {
                    k_.ITHAMQUYENDONVI = Convert.ToInt32(fc["iThamQuyenDonVi_"]);
                }
                if (fc["iLinhVuc_"] != null && Convert.ToInt32(fc["iLinhVuc_"]) != -1)
                {
                    k_.ILINHVUC = Convert.ToInt32(fc["iLinhVuc_"]);
                }
                List<PRC_KIENNGHI_MOICAPNHAT> kiennghi = new List<PRC_KIENNGHI_MOICAPNHAT>();
                if (u_info.tk_action.is_lanhdao)
                {
                    kiennghi = _kiennghi.PRC_KIENNGHI_MOICAPNHAT(k_, 0, 1, 9999);
                }
                else
                {
                    kiennghi = _kiennghi.PRC_KIENNGHI_MOICAPNHAT_DIAPHUONG(k_, 0, 1, 9999);
                }
                kiennghi = kiennghi.Where(x => x.ID_GOP == 0 && x.ID_KIENNGHI_TRUNG == 0).ToList();
                string ketqua = "";
                if (kiennghi.Count() > 0)
                {
                    //ketqua = "<tr><td colspan='4' class='alert-info tcenter'>Có " + kiennghi.Count() + " kết quả tìm kiếm.</td></tr>";
                }
                ViewData["list"] = ketqua + kn.KN_ListKienNghi_ThemVaoKienNghiGop(kiennghi, id, u_info.tk_action);
                return PartialView("../Ajax/Kiennghi/Themkiennghi_tonghop_search");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm kiếm kiến nghị thêm vào Tập hợp kiến nghị");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Themkiennghi_tonghop_search(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                if (u_info == null) { return null; }
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI k_ = new KN_KIENNGHI();
                int iKyHop_ = ID_KyHop_HienTai();
                if (fc["iKyHop_"] != null) { iKyHop_ = Convert.ToInt32(fc["iKyHop_"]); }
                k_.IKYHOP = iKyHop_;
                if (fc["q"] != null) { k_.CNOIDUNG = func.RemoveTagInput(fc["q"]); }
                if (fc["cNoiDung_"] != null) { k_.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung_"]); }
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_dbqh)
                {
                    k_.IDONVITIEPNHAN = (int)u_info.user_login.IDONVI;

                }
                else
                {
                    if (fc["iDonViTiepNhan_"] != null && Convert.ToInt32(fc["iDonViTiepNhan_"]) != 0)
                    {
                        k_.IDONVITIEPNHAN = Convert.ToInt32(fc["iDonViTiepNhan_"]);
                    }
                    if (base_business.Action_(50, u_info.tk_action))
                    {
                        iUser = 0;
                    }
                }
                k_.IUSER = iUser;
                if (fc["iThamQuyenDonVi_"] != null && Convert.ToInt32(fc["iThamQuyenDonVi_"]) != 0)
                {
                    k_.ITHAMQUYENDONVI = Convert.ToInt32(fc["iThamQuyenDonVi_"]);
                }
                if (fc["iLinhVuc_"] != null && Convert.ToInt32(fc["iLinhVuc_"]) != -1)
                {
                    k_.ILINHVUC = Convert.ToInt32(fc["iLinhVuc_"]);
                }
                List<PRC_KIENNGHI_MOICAPNHAT> kiennghi = new List<PRC_KIENNGHI_MOICAPNHAT>();
                if (u_info.tk_action.is_lanhdao)
                {
                    kiennghi = _kiennghi.PRC_KIENNGHI_MOICAPNHAT(k_, 0, 1, 9999);
                }
                else
                {
                    kiennghi = _kiennghi.PRC_KIENNGHI_MOICAPNHAT_DIAPHUONG(k_, 0, 1, 9999);
                }
                //kiennghi = kiennghi.Where(x => x.ID_GOP == 0 || x.ID_GOP==-1).ToList();
                //kiennghi = kiennghi.Where(x => x.ID_KIENNGHI_TRUNG == 0).ToList();
                string ketqua = "";
                if (kiennghi.Count() > 0)
                {
                    //ketqua="<tr><td colspan='4' class='alert-info tcenter'>Có " + kiennghi.Count() + " kết quả tìm kiếm.</td></tr>";
                }
                ViewData["list"] = ketqua + kn.KN_ListKienNghiThemVaoTongHop(kiennghi, id, u_info.tk_action);
                return PartialView("../Ajax/Kiennghi/Themkiennghi_tonghop_search");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm kiếm kiến nghị thêm vào Tập hợp kiến nghị");
                throw;
            }
        }
        public ActionResult Ajax_Themkiennghi_giamsat_search(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                int iKyHop_ = Convert.ToInt32(fc["iKyHop_"]);
                int iDonViTiepNhan_ = Convert.ToInt32(fc["iDonViTiepNhan_"]);
                int iThamQuyenDonVi_ = Convert.ToInt32(fc["iThamQuyenDonVi_"]);
                int iLinhVuc_ = Convert.ToInt32(fc["iLinhVuc_"]);
                string cNoiDung_ = func.RemoveTagInput(fc["cNoiDung_"]);
                KN_KIENNGHI kn_pram = new KN_KIENNGHI();
                kn_pram.IKYHOP = iKyHop_;
                kn_pram.IDONVITIEPNHAN = iDonViTiepNhan_;
                kn_pram.ITHAMQUYENDONVI = iThamQuyenDonVi_;
                kn_pram.ILINHVUC = iLinhVuc_;
                kn_pram.CNOIDUNG = cNoiDung_;
                var kiennghi = _kiennghi.PRC_LIST_KN_TRALOI_DANHGIA(kn_pram);
                //Dictionary<string, object> dic = new Dictionary<string, object>();
                //if (iKyHop_ != 0) { dic.Add("IKYHOP", iKyHop_); }
                //if (iDonViTiepNhan_ != 0) { dic.Add("IDONVITIEPNHAN", iDonViTiepNhan_); }
                //if (iThamQuyenDonVi_ != 0) { dic.Add("ITHAMQUYENDONVI", iThamQuyenDonVi_); }
                //if (iLinhVuc_ != -1) { dic.Add("ILINHVUC", iLinhVuc_); }
                //dic.Add("ITINHTRANG", (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi);
                //List<KN_KIENNGHI> kiennghi = _kiennghi.HienThiDanhSachKienNghi(dic);
                //if (cNoiDung_ != "") { kiennghi = kiennghi.Where(x => x.CNOIDUNG.Contains(cNoiDung_)).ToList(); }
                Dictionary<string, object> dic_coquan = new Dictionary<string, object>();
                //dic_coquan.Add("IPARENT", ID_Coquan_doandaibieu);
                List<QUOCHOI_COQUAN> coquan = _kiennghi.GetAll_CoQuanByParam(dic_coquan).Where(x => x.IHIENTHI == 1).ToList();

                var doan_kiennghi = _kiennghi.GetAll_DoanGiamSat_Kiengnhi(dic_coquan);


                ViewData["list"] = kn.KN_ListKienNghiThemVaoGiamSat(kiennghi, doan_kiennghi, id, u_info.tk_action);
                return PartialView("../Ajax/Kiennghi/Themkiennghi_giamsat_search");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form kiếm kiến nghị thêm vào kế hoạch giám sát");
                throw;
            }


        }
        public ActionResult Ajax_Chonkiennghi_gop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id_kiennghi = Convert.ToInt32(fc["id_kiennghi"]);
                int id_kiennghi_gop = Convert.ToInt32(fc["id_kiennghi_gop"]);
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id_kiennghi);
                if (kiennghi.ID_GOP == 0)
                {
                    kiennghi.ID_GOP = id_kiennghi_gop;
                    _kiennghi.Tracking_KN(u_info.tk_action.iUser, id_kiennghi, "Thêm vào kiến nghị gộp");
                }
                else
                {
                    kiennghi.ID_GOP = 0;
                    _kiennghi.Tracking_KN(u_info.tk_action.iUser, id_kiennghi, "Hủy khỏi kiến nghị");
                }
                _kiennghi.UpdateThongTinKienNghi(kiennghi);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Chọn kiến nghị gộp");
                throw;
            }

        }
        public ActionResult Ajax_Chonkiennghi_tonghop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id_kiennghi = Convert.ToInt32(fc["id_kiennghi"]);
                int id_tonghop = Convert.ToInt32(fc["id_tonghop"]);
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id_kiennghi);

                List<ID_Session_KienNghi_ChonTongHop> list_id = Get_IDKienNghiChonTongHop();
                if (list_id != null)
                {
                    if (list_id.Where(x => x.IKIENNGHI == id_kiennghi).Count() > 0)
                    {
                        ID_Session_KienNghi_ChonTongHop id_session = new ID_Session_KienNghi_ChonTongHop();
                        id_session.IKIENNGHI = (decimal)id_kiennghi;
                        id_session.ILINHVUC = (decimal)kiennghi.ILINHVUC;
                        id_session.ITHAMQUYENDONVI = (decimal)kiennghi.ITHAMQUYENDONVI;
                        Set_IDKienNghiChonTongHop(id_session);
                    }
                }


                if (u_info.tk_action.is_dbqh)
                {
                    if (kiennghi.ITONGHOP == 0)
                    {
                        kiennghi.ITONGHOP = id_tonghop;
                        _kiennghi.Tracking_KN(u_info.tk_action.iUser, id_kiennghi, "Thêm vào Tập hợp kiến nghị");
                    }
                    else
                    {
                        kiennghi.ITONGHOP = 0;
                        _kiennghi.Tracking_KN(u_info.tk_action.iUser, id_kiennghi, "Hủy Tập hợp kiến nghị");
                    }
                }
                if (u_info.tk_action.is_lanhdao)
                {
                    if (kiennghi.ITONGHOP_BDN == 0)
                    {
                        //List<ID_Session_KienNghi_ChonTongHop> list_id = Get_IDKienNghiChonTongHop();

                        //ID_Session_KienNghi_ChonTongHop id_session = list_id.Where(x => x.IKIENNGHI == id.IKIENNGHI).First();
                        //list_id.Remove(id_session);

                        kiennghi.ITONGHOP_BDN = id_tonghop;
                        _kiennghi.Tracking_KN(u_info.tk_action.iUser, id_kiennghi, "Thêm vào Tập hợp kiến nghị của Ban Dân nguyện");
                    }
                    else
                    {
                        kiennghi.ITONGHOP_BDN = 0;
                        _kiennghi.Tracking_KN(u_info.tk_action.iUser, id_kiennghi, "Hủy Tập hợp kiến nghị của Ban Dân nguyện");
                    }
                }
                _kiennghi.UpdateThongTinKienNghi(kiennghi);

                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Chọn kiến nghị Tập hợp");
                throw;
            }

        }
        public ActionResult Ajax_Chonkiennghi_giamsat(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id_kiennghi = Convert.ToInt32(fc["id_kiennghi"]);
                int id_giamsat = Convert.ToInt32(fc["id_giamsat"]);
                UserInfor u_info = GetUserInfor();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("IKIENNGHI", id_kiennghi);
                dic.Add("IDOANGIAMSAT", id_giamsat);
                if (_kiennghi.GetAll_DoanGiamSat_Kiengnhi(dic).Count() == 0)
                {
                    KN_DOANGIAMSAT_KIENNGHI doan_kiennghi = new KN_DOANGIAMSAT_KIENNGHI();
                    doan_kiennghi.IKIENNGHI = id_kiennghi;
                    doan_kiennghi.IDOANGIAMSAT = id_giamsat;
                    _kiennghi.InsertDoan_kiennghi(doan_kiennghi);
                    _kiennghi.Tracking_KN(u_info.tk_action.iUser, id_kiennghi, "Thêm vào giám sát");
                }
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Chọn kiến nghị thêm vào kế hoạch giám sát");
                throw;
            }

        }
        public ActionResult Ajax_Themkiennghi_giamsat(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_DOANGIAMSAT giamsat = _kiennghi.Get_DoanGiamSat(id);
                //int iKyHop = (int)giamsat.IKYHOP;
                ViewData["id"] = Request["id"];
                Dictionary<string, object> donvi = new Dictionary<string, object>();
                List<QUOCHOI_COQUAN> coquan = _kiennghi.GetAll_CoQuanByParam(donvi).Where(x => x.IHIENTHI == 1).OrderBy(x => x.CTEN).ToList();
                var doan = coquan.Where(x => x.IPARENT == ID_Coquan_doandaibieu).ToList();
                ViewData["opt-donvitiepnhan"] = "<option value='0'>- - - Chọn tất cả</option>" + kn.OptionCoQuanXuLy(doan, ID_Coquan_doandaibieu, 0, 0, 0);
                ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen();
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                ViewData["opt-kyhop"] = Get_Option_KyHop(ID_KyHop_HienTai());

                return PartialView("../Ajax/Kiennghi/Themkiennghi_giamsat");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form Tìm kiếm kiến nghị thêm vào kế hoạch giám sát");
                throw;
            }

        }
        public ActionResult Ajax_View_kiennghi_chontonghop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                string kn_chon = fc["kn_chon"];
                if (kn_chon != "")
                {
                    Response.Write(kn.List_KienNghi_Chon(kn_chon));
                }
                else
                {
                    Response.Write("");
                }
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xem kiến nghị đã chọn Tập hợp");
                throw;
            }

        }
        public ActionResult Ajax_search_giamsat()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(0);
                ViewData["opt-donvi"] = "<option value='0'>Chọn đơn vị </option>" + Get_Option_Coquanxuly();
                ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong);
                return PartialView("../Ajax/Kiennghi/search_giamsat");

            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form Tìm kiếm kế hoạch giám sát");
                throw;
            }

        }
        public ActionResult Giamsat()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                if (!base_business.Action_(46, u_info.tk_action))
                {
                    Response.Redirect("/Home/Error/"); return null;
                }
                string noidung = "";
                int id_donvi = 0;
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                if (Request["cNoiDung"] != null) { noidung = Request["cNoiDung"]; }
                if (Request["q"] != null) { noidung = Request["q"]; }
                if (Request["iThamQuyenDonVi"] != null) { id_donvi = Convert.ToInt32(Request["iThamQuyenDonVi"]); }
                var iUser = u_info.tk_action.iUser;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                var doan = _kiennghi.PRC_DOANGIAMSAT(noidung, id_donvi, iUser, page, post_per_page);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;
                
                if (doan.Count() > 0)
                {
                    htmlPhanTrang = "<tr><td colspan='6'>" + base_appcode.PhanTrang((int)doan.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    
                }
                htmlList = kn.List_Giamsat(doan, u_info.tk_action);
                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách kế hoạch giám sát");
                return View("../Home/Error_Exception");
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_giamsat_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_DOANGIAMSAT giamsat = new KN_DOANGIAMSAT();
        //        int iDonVi = 0;
        //        if (fc["iDonVi"] != null) { iDonVi = Convert.ToInt32(fc["iDonVi"]); }
        //        if (fc["cNoiDung"] != null) { giamsat.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        giamsat.IDONVI = iDonVi;
        //        if (fc["q"] != null) { giamsat.CNOIDUNG = func.RemoveTagInput(fc["q"]); }
        //        var doan = _kiennghi.GetAll_Search_Doangiamsat(giamsat);
        //        UserInfor u_info = GetUserInfor();
        //        var coquan = _kiennghi.HienThiDanhSachCoQuan();
        //        if (doan.Count() > 0)
        //        {
        //            Response.Write("<tr><td colspan='5' class='alert-info tcenter'>Có " + doan.Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        Response.Write(kn.List_Giamsat(doan, coquan, u_info.tk_action));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Kết quả tìm kiếm kế hoạch giám sát");
        //        throw;
        //    }

        //}
        public ActionResult Ajax_Giamsat_add()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                SetTokenAction("giamsat_add");
                UserInfor u_info = GetUserInfor();

                List<QUOCHOI_COQUAN> coquan;
                Dictionary<string, object> donvi = new Dictionary<string, object>();
                coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(0);
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong);
                    ViewData["opt-coquan"] = kn.OptionCoQuanXuLy(coquan, 0, 0, ID_Ban_DanNguyen, 0);
                }
                else
                {
                    ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(0);
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong);
                    ViewData["opt-coquan"] = "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
                }
                return PartialView("../Ajax/Kiennghi/Giamsat_add");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form thêm mới kế hoạch giám sát");
                throw;
            }

        }
        public ActionResult Ajax_Giamsat_edit(FormCollection fc)
        {

            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                SetTokenAction("giamsat_edit", id);
                KN_DOANGIAMSAT doan = _kiennghi.Get_DoanGiamSat(id);
                ViewData["id"] = fc["id"];

                ViewData["doan"] = doan;
                ViewData["file"] = kn.File_Edit(id, "kn_doangiamsat", Request.Cookies["url_key"].Value);
                Dictionary<string, object> donvi = new Dictionary<string, object>();
                donvi.Add("ICOQUAN", doan.IDONVI);
                var coquanchon = _kiennghi.GetAll_CoQuanByParam(donvi).FirstOrDefault();
                if (u_info.tk_action.is_lanhdao)
                {
                    if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    {
                        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong, (int)coquanchon.ICOQUAN);
                        ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Trunguong);
                    }

                    if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    {
                        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Tinh, (int)coquanchon.ICOQUAN);
                        ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Tinh);
                    }

                    if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    {
                        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Huyen, (int)coquanchon.ICOQUAN);
                        ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Huyen);
                    }
                }
                else
                {
                    if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    {
                        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong, (int)coquanchon.ICOQUAN);
                        ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Trunguong);
                    }

                    if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    {
                        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Tinh, (int)coquanchon.ICOQUAN);
                        ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Tinh);
                    }

                    if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    {
                        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Huyen, (int)coquanchon.ICOQUAN);
                        ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Huyen);
                    }
                }
                return PartialView("../Ajax/Kiennghi/Giamsat_edit");

            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form sửa kế hoạch giám sát");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Giamsat_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("giamsat_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_DOANGIAMSAT t = _kiennghi.Get_DoanGiamSat(id);
                t.DNGAYBATDAU = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBatDau"]));
                t.CTEN = func.RemoveTagInput(fc["cTen"]);
                t.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                t.IDONVI = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                _kiennghi.Update_DoanGiamsat(t);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_doangiamsat";
                        f.CFILE = UploadFile(file);
                        f.ID = id;
                        kntc.Upload_file(f);
                    }
                }
                Tracking(u_info.tk_action.iUser, "Sửa kế hoạch giám sát: " + t.CTEN);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Update kế hoạch giám sát");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Giamsat_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                if (!CheckTokenAction("giamsat_add")) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_DOANGIAMSAT t = new KN_DOANGIAMSAT();
                t.IKYHOP = 0;
                t.DNGAYBATDAU = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBatDau"]));
                t.IDONVI = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                t.CTEN = func.RemoveTagInput(fc["cTen"]);
                t.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                t.IUSER = u_info.tk_action.iUser;
                t.DDATE = DateTime.Now;
                _kiennghi.Insert_DoanGiamsat(t);
                int iDoan = (int)t.IDOAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_doangiamsat";
                        f.CFILE = UploadFile(file);
                        f.ID = iDoan;
                        kntc.Upload_file(f);
                    }
                }
                Tracking(u_info.tk_action.iUser, "Thêm mới kế hoạch giám sát: " + t.CTEN);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Insert kế hoạch giám sát");
                return View("../Home/Error_Exception");
            }
            //int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));

        }
        public ActionResult Tonghop()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                UserInfor u_info = GetUserInfor();
                base_business.ActionMulty_Redirect_("5,6", u_info.tk_action);
                if (!base_business.Action_(5, u_info.tk_action))//chọn Tập hợp
                {
                    ViewData["bt-add"] = " style='display:none'";
                }
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                int iDonViXuLy_Parent = 0; if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                int iDonViXuLy = 0; if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                int iKyHop = ID_KyHop_HienTai(); if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (iDonViXuLy != 0 && coquan.Where(x => x.ICOQUAN == iDonViXuLy).Count() == 0)
                    {
                        iDonViXuLy = 0;
                    }
                }
                else
                {
                    coquan = null;
                }
                ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, iDonViXuLy);
                KN_TONGHOP tonghop_pr = get_Request_Paramt_TongHop_KienNghi();
                tonghop_pr.IKYHOP = iKyHop;
                tonghop_pr.IDONVITONGHOP = iDonViTiepNhan;
                tonghop_pr.ITINHTRANG = (decimal)TrangThai_TongHop.ChoXuLy;
                tonghop_pr.ITHAMQUYENDONVI = iDonViXuLy;
                List<PRC_TONGHOP_KIENNGHI> tonghop = _kiennghi.List_PRC_TONGHOP_KIENNGHI(tonghop_pr, iDonViXuLy_Parent, page, post_per_page);
                if (tonghop.Count() > 0)
                {
                    ViewData["phantrang"] = "<tr><td colspan='5'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                ViewData["list"] = kn.KN_Tonghop_Doan_Choxuly(tonghop, u_info.tk_action);
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);

                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Chuyen_tonghop_xuly()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                if (!base_business.ActionMulty_("7,50", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                //if (tonghop.IDONVITONGHOP != ID_Ban_DanNguyen 
                //    || tonghop.ITHAMQUYENDONVI != (int)u_info.user_login.IDONVI 
                //    || tonghop.ITINHTRANG != (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen)
                //{ Response.Redirect("/Home/Error/"); return null; }
                tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;
                _kiennghi.UpdateTongHop(tonghop);
                _kiennghi.Tracking_Tonghop(u_info.tk_action.iUser, id, "Chuyển xử lý Tập hợp của Ban Dân nguyện chuyển đến");

                Dictionary<string, object> condition = new Dictionary<string, object>();
                condition.Add("ITONGHOP_BDN", id);
                var kiennghi_update = _kiennghi.HienThiDanhSachKienNghi(condition);
                foreach (var k in kiennghi_update)
                {
                    k.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet;
                    _kiennghi.UpdateThongTinKienNghi(k);
                }

                //_kiennghi.KN_Update_All(condition, "ITONGHOP_BDN", id);
                Response.Redirect("/Kiennghi/Chuatraloi/#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Chuyển xử lý Tập hợp đến đơn vị thẩm quyền");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tonghop_bandannguyen_chuyen()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("7,50", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                int iKyHop = ID_KyHop_HienTai();
                //if (Request["iDonVi"] != null) { ITHAMQUYENDONVI = Convert.ToInt32(Request["iDonVi"]); }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                KN_TONGHOP kn_tonghop = new KN_TONGHOP();
                kn_tonghop.IKYHOP = iKyHop;
                kn_tonghop.ITHAMQUYENDONVI = ITHAMQUYENDONVI;
                //kn_tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen;
                //kn_tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen;
                if (Request["q"] != null) { kn_tonghop.CNOIDUNG = Request["q"]; }
                if (Request["cNoiDung"] != null) { kn_tonghop.CNOIDUNG = Request["cNoiDung"]; }
                if (Request["iTruocKyHop"] != null) { kn_tonghop.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]); }
                var tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(kn_tonghop, 0, 3, 3, -1, -1, -1, -1, page, post_per_page);
                //tonghop = tonghop.Where(x => x.ITINHTRANG == ).ToList();
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["list"] = kn.KN_Tonghop_bandannguyen_chuyen(tonghop, u_info.tk_action);
                if (tonghop.Count() > 0)
                {
                    ViewData["phantrang"] = "<tr><td colspan='3'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }

                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị Ban Dân nguyện đã chuyển");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Chuatraloi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("7,8,50", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                if (u_info.tk_action.is_lanhdao)
                {
                    ITHAMQUYENDONVI = 0;

                }
                int iDonViXuLy_Parent = 0;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDonViXuLy"] != null)
                {
                    ITHAMQUYENDONVI = Convert.ToInt32(Request["iDonViXuLy"]);

                }
                else
                {
                    if (u_info.tk_action.is_dbqh)//đbqh đăng nhập
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (!u_info.tk_action.is_lanhdao)
                {
                    if (Request["iDonViXuLy"] == null && !u_info.tk_action.is_dbqh)
                    {
                        ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                    }

                }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (ITHAMQUYENDONVI != 0 && coquan.Where(x => x.ICOQUAN == ITHAMQUYENDONVI).Count() == 0)
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                else
                {
                    coquan = null;
                }
                ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, ITHAMQUYENDONVI);
                var iUser = u_info.tk_action.iUser;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                KN_TONGHOP th = get_Request_Paramt_TongHop_KienNghi();
                th.IKYHOP = iKyHop;
                th.ITHAMQUYENDONVI = ITHAMQUYENDONVI;
                th.IUSER = iUser;
                if (u_info.tk_action.is_dbqh && th.IDONVITONGHOP == 0)//đbqh đăng nhập
                {
                    th.IDONVITONGHOP = u_info.user_login.IDONVI;
                }
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;               
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;
                
                string listKyHop = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                th.ITINHTRANG = (decimal)TrangThaiKienNghi.ChuaTraLoiDangXuly;
                int donvitiepnhan = 0;
                if (Request["iDoan"] != null)
                {
                    donvitiepnhan = Convert.ToInt32(Request["iDoan"]);
                }
                String makiennghi = Request["iMaKienNghi"];
                string listNguonKienNghi = "0";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listNguonKienNghi = Request["listHuyen_Xa_ThanhPho"];
                }

                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["iTuNgay"] != null && Request["iTuNgay"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["iTuNgay"]));
                }
                if (Request["iDenNgay"] != null && Request["iDenNgay"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["iDenNgay"]));
                }
                if (Request["iTruocKyHop"] != null)
                {
                    th.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]);
                }
                //var tonghop = _kiennghi.PRC_LIST_TONGHOP_CHUATRALOI( th,0,"", listKyHop, "0", iDonViXuLy_Parent, null, null, page, post_per_page);
                var tonghop = _kiennghi.PRC_LIST_TONGHOP_CHUATRALOI(th, donvitiepnhan, makiennghi, listKyHop, listNguonKienNghi, iDonViXuLy_Parent, dtungay, ddenngay, page, post_per_page);

                
                // option co value 3 la khi khong chon lua chon nao
                if (Request["iCanhBao"] != null && Request["iCanhBao"] != "null" && Request["iCanhBao"] != "")
                {
                    int canhbao = (int)Convert.ToInt64(Request["iCanhBao"]);
                    tonghop = tonghop.Where(x => x.ICANHBAO == canhbao).ToList();
                }
                //tonghop = tonghop.Where(x=> x.SOKIENNGHI_CHUATRALOI>0).ToList();
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (tonghop.Count() > 0)
                {
                    htmlPhanTrang = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlList = kn.KN_Tonghop_ChuaTraLoi(tonghop, u_info.tk_action);
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

                ViewData["is_bdn"] = 0;
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                }
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị đang xử lý");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Dangxuly()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("7,8", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                if (u_info.tk_action.is_lanhdao)
                {
                    ITHAMQUYENDONVI = 0;

                }
                int iDonViXuLy_Parent = 0;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDonViXuLy"] != null)
                {
                    ITHAMQUYENDONVI = Convert.ToInt32(Request["iDonViXuLy"]);
                }
                else
                {
                    if (u_info.tk_action.is_dbqh)//đbqh đăng nhập
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (!u_info.tk_action.is_lanhdao)
                {
                    if (Request["iDonViXuLy"] == null && !u_info.tk_action.is_dbqh)
                    {
                        ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                    }
                }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (ITHAMQUYENDONVI != 0 && coquan.Where(x => x.ICOQUAN == ITHAMQUYENDONVI).Count() == 0)
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                else
                {
                    coquan = null;
                }
                ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, ITHAMQUYENDONVI);
                var iUser = u_info.tk_action.iUser;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                KN_TONGHOP th = get_Request_Paramt_TongHop_KienNghi();
                th.IUSER = iUser;
                th.IKYHOP = iKyHop;
                th.ITHAMQUYENDONVI = ITHAMQUYENDONVI;
                if (u_info.tk_action.is_dbqh && th.IDONVITONGHOP == 0)//đbqh đăng nhập
                {
                    th.IDONVITONGHOP = u_info.user_login.IDONVI;
                }
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;               
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;

                //Haibd6: Đồng bộ chung kết quả Kiến nghị Đang xử lý và Chưa trả lời
                //var tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(th, iDonViXuLy_Parent,4,4,0,-1,0,-1, page, post_per_page);
                string listKyHop = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                th.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet;
                int donvitiepnhan = 0;
                if (Request["iDoan"] != null)
                {
                    donvitiepnhan = Convert.ToInt32(Request["iDoan"]);
                }
                String makiennghi = Request["iMaKienNghi"];
                string listNguonKienNghi = "0";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listNguonKienNghi = Request["listHuyen_Xa_ThanhPho"];
                }
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["iTuNgay"] != null && Request["iTuNgay"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["iTuNgay"]));
                }
                if (Request["iDenNgay"] != null && Request["iDenNgay"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["iDenNgay"]));
                }
                if (Request["iTruocKyHop"] != null)
                {
                    th.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]);
                }
                var tonghop = _kiennghi.PRC_LIST_TONGHOP_CHUATRALOI(th, donvitiepnhan, makiennghi, listKyHop, listNguonKienNghi, iDonViXuLy_Parent, dtungay, ddenngay, page, post_per_page);
                if (Request["iCanhBao"] != null && Request["iCanhBao"] != "null" && Request["iCanhBao"] != "")
                {
                    int canhbao = (int)Convert.ToInt64(Request["iCanhBao"]);
                    tonghop = tonghop.Where(x => x.ICANHBAO == canhbao).ToList();
                }
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;
                //tonghop = tonghop.Where(x=> x.SOKIENNGHI_CHUATRALOI>0).ToList();

                if (tonghop.Count() > 0)
                {
                    htmlPhanTrang = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlList = kn.KN_Tonghop_DangTraLoi(tonghop, u_info.tk_action);
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

                ViewData["is_bdn"] = 0;
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                }
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị đang xử lý");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Chuaxuly()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("7,8", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                if (u_info.tk_action.is_lanhdao)
                {
                    ITHAMQUYENDONVI = 0;

                }
                int iDonViXuLy_Parent = 0;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDonViXuLy"] != null)
                {
                    ITHAMQUYENDONVI = Convert.ToInt32(Request["iDonViXuLy"]);
                }
                else
                {
                    if (u_info.tk_action.is_dbqh)//đbqh đăng nhập
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (!u_info.tk_action.is_lanhdao)
                {
                    if (Request["iDonViXuLy"] == null && !u_info.tk_action.is_dbqh)
                    {
                        ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                    }
                }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (ITHAMQUYENDONVI != 0 && coquan.Where(x => x.ICOQUAN == ITHAMQUYENDONVI).Count() == 0)
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                else
                {
                    coquan = null;
                }
                ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, ITHAMQUYENDONVI);
                var iUser = u_info.tk_action.iUser;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                KN_TONGHOP th = get_Request_Paramt_TongHop_KienNghi();
                th.IUSER = iUser;
                th.IKYHOP = iKyHop;
                th.ITHAMQUYENDONVI = ITHAMQUYENDONVI;
                if (u_info.tk_action.is_dbqh && th.IDONVITONGHOP == 0)//đbqh đăng nhập
                {
                    th.IDONVITONGHOP = u_info.user_login.IDONVI;
                }
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;               
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;

                //Haibd6: Đồng bộ chung kết quả Kiến nghị Đang xử lý và Chưa trả lời
                //var tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(th, iDonViXuLy_Parent,4,4,0,-1,0,-1, page, post_per_page);
                string listKyHop = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                int donvitiepnhan = 0;
                if (Request["iDoan"] != null)
                {
                    donvitiepnhan = Convert.ToInt32(Request["iDoan"]);
                }
                String makiennghi = Request["iMaKienNghi"];
                string listNguonKienNghi = "0";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listNguonKienNghi = Request["listHuyen_Xa_ThanhPho"];
                }
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["iTuNgay"] != null && Request["iTuNgay"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["iTuNgay"]));
                }
                if (Request["iDenNgay"] != null && Request["iDenNgay"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["iDenNgay"]));
                }
                if (Request["iTruocKyHop"] != null)
                {
                    th.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]);
                }
                th.ITINHTRANG = (decimal)TrangThaiKienNghi.ChuaXuLy;
                var tonghop = _kiennghi.PRC_LIST_TONGHOP_CHUATRALOI(th, donvitiepnhan, makiennghi, listKyHop, listNguonKienNghi, iDonViXuLy_Parent, dtungay, ddenngay, page, post_per_page);
                
                if (Request["iCanhBao"] != null && Request["iCanhBao"] != "null" && Request["iCanhBao"] != "")
                {
                    int canhbao = (int)Convert.ToInt64(Request["iCanhBao"]);
                    tonghop = tonghop.Where(x => x.ICANHBAO == canhbao).ToList();
                }
                //tonghop = tonghop.Where(x=> x.SOKIENNGHI_CHUATRALOI>0).ToList();
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (tonghop.Count() > 0)
                {
                    htmlPhanTrang = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlList = kn.KN_Tonghop_DangTraLoi(tonghop, u_info.tk_action);
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

                ViewData["is_bdn"] = 0;
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                }
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị đang xử lý");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Dacotraloi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("7,8", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                if (u_info.tk_action.is_lanhdao)
                {
                    ITHAMQUYENDONVI = 0;

                }
                int iDonViXuLy_Parent = 0;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDonViXuLy"] != null)
                {
                    ITHAMQUYENDONVI = Convert.ToInt32(Request["iDonViXuLy"]);
                }
                else
                {
                    if (u_info.tk_action.is_dbqh)//đbqh đăng nhập
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (!u_info.tk_action.is_lanhdao)
                {
                    if (Request["iDonViXuLy"] == null && !u_info.tk_action.is_dbqh)
                    {
                        ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                    }
                }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (ITHAMQUYENDONVI != 0 && coquan.Where(x => x.ICOQUAN == ITHAMQUYENDONVI).Count() == 0)
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                else
                {
                    coquan = null;
                }
                ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, ITHAMQUYENDONVI);
                var iUser = u_info.tk_action.iUser;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                KN_TONGHOP th = get_Request_Paramt_TongHop_KienNghi();
                th.IUSER = iUser;
                th.IKYHOP = iKyHop;
                th.ITHAMQUYENDONVI = ITHAMQUYENDONVI;
                
                if (u_info.tk_action.is_dbqh && th.IDONVITONGHOP == 0)//đbqh đăng nhập
                {
                    th.IDONVITONGHOP = u_info.user_login.IDONVI;
                }
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;               
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;

                //Haibd6: Đồng bộ chung kết quả Kiến nghị Đang xử lý và Chưa trả lời
                //var tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(th, iDonViXuLy_Parent,4,4,0,-1,0,-1, page, post_per_page);
                string listKyHop = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                th.ITINHTRANG = -1;
                if (Request["iTrangThai"] != null)
                {
                    th.ITINHTRANG = Convert.ToInt32(Request["iTrangThai"]);
                }
                int donvitiepnhan = 0;
                if (Request["iDoan"] != null)
                {
                    donvitiepnhan = Convert.ToInt32(Request["iDoan"]);
                }
                String makiennghi = Request["iMaKienNghi"];
                string listNguonKienNghi = "0";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listNguonKienNghi = Request["listHuyen_Xa_ThanhPho"];
                }
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["iTuNgay"] != null && Request["iTuNgay"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["iTuNgay"]));
                }
                if (Request["iDenNgay"] != null && Request["iDenNgay"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["iDenNgay"]));
                }
                if (Request["iTruocKyHop"] != null)
                {
                    th.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]);
                }
                var tonghop = _kiennghi.PRC_LIST_TONGHOP_CHUATRALOI( th, donvitiepnhan, makiennghi, listKyHop, listNguonKienNghi, iDonViXuLy_Parent, dtungay, ddenngay, page, post_per_page);
                
                if (Request["iCanhBao"] != null && Request["iCanhBao"] != "null" && Request["iCanhBao"] != "")
                {
                    int canhbao = (int)Convert.ToInt64(Request["iCanhBao"]);
                    tonghop = tonghop.Where(x => x.ICANHBAO == canhbao).ToList();
                }
                //tonghop = tonghop.Where(x=> x.SOKIENNGHI_CHUATRALOI>0).ToList();
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (tonghop.Count() > 0)
                {
                    htmlPhanTrang = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlList = kn.KN_Tonghop_DangTraLoi(tonghop, u_info.tk_action);
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

                ViewData["is_bdn"] = 0;
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                }

                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị đang xử lý");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Giaitrinh()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("7,8", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                if (u_info.tk_action.is_lanhdao)
                {
                    ITHAMQUYENDONVI = 0;

                }
                int iDonViXuLy_Parent = 0;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDonViXuLy"] != null)
                {
                    ITHAMQUYENDONVI = Convert.ToInt32(Request["iDonViXuLy"]);
                }
                else
                {
                    if (u_info.tk_action.is_dbqh)//đbqh đăng nhập
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (!u_info.tk_action.is_lanhdao)
                {
                    if (Request["iDonViXuLy"] == null && !u_info.tk_action.is_dbqh)
                    {
                        ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                    }
                }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (ITHAMQUYENDONVI != 0 && coquan.Where(x => x.ICOQUAN == ITHAMQUYENDONVI).Count() == 0)
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                else
                {
                    coquan = null;
                }
                ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, ITHAMQUYENDONVI);
                var iUser = u_info.tk_action.iUser;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                KN_TONGHOP th = get_Request_Paramt_TongHop_KienNghi();
                th.IUSER = iUser;
                th.IKYHOP = iKyHop;
                th.ITHAMQUYENDONVI = ITHAMQUYENDONVI;
                if (u_info.tk_action.is_dbqh && th.IDONVITONGHOP == 0)//đbqh đăng nhập
                {
                    th.IDONVITONGHOP = u_info.user_login.IDONVI;
                }
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;               
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;

                //Haibd6: Đồng bộ chung kết quả Kiến nghị Đang xử lý và Chưa trả lời
                //var tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(th, iDonViXuLy_Parent,4,4,0,-1,0,-1, page, post_per_page);
                string listKyHop = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                th.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaGiaiTrinh;
                int donvitiepnhan = 0;
                if (Request["iDoan"] != null)
                {
                    donvitiepnhan = Convert.ToInt32(Request["iDoan"]);
                }
                String makiennghi = Request["iMaKienNghi"]; string listNguonKienNghi = "0";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listNguonKienNghi = Request["listHuyen_Xa_ThanhPho"];
                }
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["iTuNgay"] != null && Request["iTuNgay"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["iTuNgay"]));
                }
                if (Request["iDenNgay"] != null && Request["iDenNgay"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["iDenNgay"]));
                }
                if (Request["iTruocKyHop"] != null)
                {
                    th.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]);
                }
                var tonghop = _kiennghi.PRC_LIST_TONGHOP_CHUATRALOI(th, donvitiepnhan, makiennghi, listKyHop, listNguonKienNghi, iDonViXuLy_Parent, dtungay, ddenngay, page, post_per_page);
                
                if (Request["iCanhBao"] != null && Request["iCanhBao"] != "null" && Request["iCanhBao"] != "")
                {
                    int canhbao = (int)Convert.ToInt64(Request["iCanhBao"]);
                    tonghop = tonghop.Where(x => x.ICANHBAO == canhbao).ToList();
                }
                //tonghop = tonghop.Where(x=> x.SOKIENNGHI_CHUATRALOI>0).ToList();
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (tonghop.Count() > 0)
                {
                    htmlPhanTrang = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlList = kn.KN_Tonghop_DangTraLoi(tonghop, u_info.tk_action);
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
                ViewData["is_bdn"] = 0;
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                }
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị đang xử lý");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Traloi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("7,8", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                if (u_info.tk_action.is_lanhdao)
                {
                    ITHAMQUYENDONVI = 0;

                }
                int iDonViXuLy_Parent = 0;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDonViXuLy"] != null)
                {
                    ITHAMQUYENDONVI = Convert.ToInt32(Request["iDonViXuLy"]);
                }
                else
                {
                    if (u_info.tk_action.is_dbqh)//đbqh đăng nhập
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    if (ITHAMQUYENDONVI != 0 && coquan.Where(x => x.ICOQUAN == ITHAMQUYENDONVI).Count() == 0)
                    {
                        ITHAMQUYENDONVI = 0;
                    }
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, ITHAMQUYENDONVI);
                }
                else
                {
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(null, ITHAMQUYENDONVI);
                }
                ViewData["is_bdn"] = 1;
                if (!u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 0;
                    if (Request["iDonViXuLy"] == null && !u_info.tk_action.is_dbqh)
                    {
                        ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                    }
                }
                var iUser = u_info.tk_action.iUser;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                KN_TONGHOP th = get_Request_Paramt_TongHop_KienNghi();
                th.IUSER = iUser;
                th.IKYHOP = iKyHop;
                th.ITHAMQUYENDONVI = ITHAMQUYENDONVI;
                if (u_info.tk_action.is_dbqh && th.IDONVITONGHOP == 0)//đbqh đăng nhập
                {
                    th.IDONVITONGHOP = u_info.user_login.IDONVI;
                }
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;               
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                //th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;
                string listKyHop = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                th.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi;

                int donvitiepnhan = 0;
                if (Request["iDoan"] != null)
                {
                    donvitiepnhan = Convert.ToInt32(Request["iDoan"]);
                }
                String makiennghi = Request["iMaKienNghi"]; string listNguonKienNghi = "0";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listNguonKienNghi = Request["listHuyen_Xa_ThanhPho"];
                }
                DateTime? dtungay = null;
                DateTime? ddenngay = null;
                if (Request["iTuNgay"] != null && Request["iTuNgay"] != "")
                {
                    dtungay = Convert.ToDateTime(func.ConvertDateToSql(Request["iTuNgay"]));
                }
                if (Request["iDenNgay"] != null && Request["iDenNgay"] != "")
                {
                    ddenngay = Convert.ToDateTime(func.ConvertDateToSql(Request["iDenNgay"]));
                }
                if (Request["iTruocKyHop"] != null)
                {
                    th.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]);
                }
                var tonghop = _kiennghi.PRC_LIST_TONGHOP_CHUATRALOI(th, donvitiepnhan, makiennghi, listKyHop, listNguonKienNghi, iDonViXuLy_Parent, dtungay, ddenngay, page, post_per_page);
                if (Request["iCanhBao"] != null && Request["iCanhBao"] != "null" && Request["iCanhBao"] != "")
                {
                    int canhbao = (int)Convert.ToInt64(Request["iCanhBao"]);
                    tonghop = tonghop.Where(x => x.ICANHBAO == canhbao).ToList();
                }
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (tonghop.Count() > 0)
                {
                    htmlPhanTrang = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                    htmlList = kn.KN_Tonghop_DangTraLoi(tonghop, u_info.tk_action);
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

                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị đã trả lời");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Theodoi_chuyengiaiquyet()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                int iDonViXuLy = 0;
                int iDonViXuLy_Parent = 0;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);

                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    if (iDonViXuLy != 0 && coquan.Where(x => x.ICOQUAN == iDonViXuLy).Count() == 0)
                    {
                        iDonViXuLy = 0;
                    }
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, iDonViXuLy);
                }
                else
                {
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(null, iDonViXuLy);
                }

                KN_TONGHOP tonghop = get_Request_Paramt_TongHop_KienNghi();
                tonghop.IDONVITONGHOP = (decimal)ID_Capcoquan.Bandannguyen;
                tonghop.IKYHOP = iKyHop;
                tonghop.ITHAMQUYENDONVI = iDonViXuLy;

                var list_tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(tonghop, iDonViXuLy_Parent, 3, 4, -1, -1, -1, -1, page, post_per_page);
                //list_tonghop = list_tonghop.Where(x => x.ITINHTRANG >= (int)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen).ToList();
                ViewData["list"] = kn.KN_Theodoi_tonghop_chuyendiaphuong(list_tonghop);
                if (list_tonghop.Count() > 0)
                {
                    ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)list_tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Theo dõi Tập hợp kiến nghị đã chuyển giải quyết");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Theodoi_dannguyen()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                //if (!u_info.tk_action.is_lanhdao) { Response.Redirect("/Home/Error/"); }

                int iDonViTiepNhan = 0;
                if (u_info.tk_action.is_dbqh)
                {
                    iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                }
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iDonVi"] != null) { iDonViTiepNhan = Convert.ToInt32(Request["iDonVi"]); }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                Dictionary<string, object> _kn = new Dictionary<string, object>();
                if (iDonViTiepNhan != 0) { _kn.Add("IDONVITONGHOP", iDonViTiepNhan); }
                if (iKyHop != 0)
                {
                    _kn.Add("IKYHOP", iKyHop);
                }
                _kn.Add("ITINHTRANG", (decimal)TrangThai_TongHop.DaChuyenBanDanNguyen);//chuyển ban dân nguyện
                List<KN_TONGHOP> tonghop = _kiennghi.GetAll_TongHopByParam(_kn);
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);

                List<QUOCHOI_COQUAN> coquan;
                Dictionary<string, object> donvi = new Dictionary<string, object>();

                if (u_info.tk_action.is_lanhdao)
                {
                    donvi.Add("IPARENT", ID_Coquan_doandaibieu);
                    coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
                    ViewData["opt-coquan"] = "<option value='0'>Chọn đoàn ĐBQH</option>" +
                                            kn.OptionCoQuanXuLy(coquan, ID_Coquan_doandaibieu, 0, iDonViTiepNhan, 0);
                }
                else
                {
                    donvi.Add("ICOQUAN", iDonViTiepNhan);
                    coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
                    ViewData["opt-coquan"] = "<option value='" + iDonViTiepNhan + "'>" + u_info.tk_action.tendonvi + "</option>";
                }
                //ViewData["list"] = kn.KN_Theodoi_tonghop_chuyendiaphuong(tonghop, coquan, u_info.tk_action);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tập hợp kiến nghị đã chuyển Ban Dân nguyện");
                return View("../Home/Error_Exception");
            }

        }
        //public ActionResult Theodoi_diaphuong()
        //{
        //    // BDN
        //    if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

        //    try
        //    {
        //        func.SetCookies("url_return", Request.Url.AbsoluteUri);
        //        UserInfor u_info = GetUserInfor();
        //        base_business.ActionMulty_Redirect_("5,6", u_info.tk_action);

        //        int iDonViXuLy = 0;
        //        int iKyHop = ID_KyHop_HienTai();
        //        if (Request["iDonVi"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonVi"]); }
        //        if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }

        //        ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);               
        //        ViewData["opt-coquan"] = Get_Option_DoanDaiBieu(iDonViXuLy);
        //        KN_TONGHOP tonghop = new KN_TONGHOP();
        //        tonghop.IKYHOP = iKyHop;
        //        tonghop.ITHAMQUYENDONVI = iDonViXuLy;
        //        var list_tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(tonghop, 20);
        //        //list_tonghop = list_tonghop.Where(x => x.ITINHTRANG >= (int)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen).ToList();
        //        ViewData["list"] = kn.KN_Theodoi_tonghop_chuyendiaphuong(list_tonghop);
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Theo dõi Tập hợp kiến nghị thuộc thẩm quyền địa phương giải quyết");
        //        return View("../Home/Error_Exception");
        //    }

        //}
        public ActionResult Theodoi_kiennghi_diaphuong()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                int iKyHop = ID_KyHop_HienTai();

                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                Dictionary<string, object> _kn = new Dictionary<string, object>();
                if (iKyHop != 0) { _kn.Add("IKYHOP", iKyHop); }
                _kn.Add("ITHAMQUYENDONVI", (int)u_info.user_login.IDONVI);
                List<KN_KIENNGHI> kiennghi = _kiennghi.HienThiDanhSachKienNghi(_kn).ToList();
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["list"] = kn.KN_Tonghop_Kiennghi_Diaphuong(kiennghi, u_info.tk_action);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Theo dõi kiến nghị đã chuyển giải quyết");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Theodoi_kiennghi_dannguyen()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            //if (!CheckAuthToken()) { return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                int iKyHop = ID_KyHop_HienTai();

                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                Dictionary<string, object> _kn = new Dictionary<string, object>();
                _kn.Add("IDONVITIEPNHAN", (int)u_info.user_login.IDONVI);
                if (iKyHop != 0) { _kn.Add("IKYHOP", iKyHop); }
                List<KN_KIENNGHI> kiennghi = _kiennghi.HienThiDanhSachKienNghi(_kn).Where(x => x.ITHAMQUYENDONVI != (int)u_info.user_login.IDONVI).ToList();

                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["list"] = kn.KN_Tonghop_Kiennghi_ChuyenDanNguyen(kiennghi, u_info.tk_action);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách kiến nghị đã chuyển Ban Dân nguyện giải quyết");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tonghop_chuyendiaphuong()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("5,6", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!base_business.Action_(5, u_info.tk_action))//chọn Tập hợp
                {
                    ViewData["bt-add"] = " style='display:none'";
                }
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                int iDonViXuLy = (int)u_info.user_login.IDONVI;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                KN_TONGHOP kn_pramt = new KN_TONGHOP();
                //kn_pramt.ITHAMQUYENDONVI = 0;
                kn_pramt.IDONVITONGHOP = iDonViTiepNhan;
                kn_pramt.IKYHOP = iKyHop;
                if (Request["q"] != null) { kn_pramt.CNOIDUNG = Request["q"]; }
                if (Request["cNoiDung"] != null) { kn_pramt.CNOIDUNG = Request["cNoiDung"]; }
                if (Request["iTruocKyHop"] != null) { kn_pramt.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]); }
                var tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(kn_pramt, 0, 2, 4, -1, -1, -1, -1, page, post_per_page);

                ViewData["list"] = kn.DBQH_Tonghop_ChuyenDiaPhuongXuLy(tonghop);
                if (tonghop.Count() > 0)
                {
                    ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";

                }
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị đã chuyển địa phương giải quyết");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tonghop_chuyendannguyen()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("5,6", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!base_business.Action_(5, u_info.tk_action))//chọn Tập hợp
                {
                    ViewData["bt-add"] = " style='display:none'";
                }
                int iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                int iKyHop = ID_KyHop_HienTai(); if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }

                KN_TONGHOP kn_tonghop = new KN_TONGHOP();
                kn_tonghop.IDONVITONGHOP = iDonViTiepNhan;
                kn_tonghop.IKYHOP = iKyHop;
                if (Request["q"] != null) { kn_tonghop.CNOIDUNG = Request["q"]; }
                if (Request["cNoiDung"] != null) { kn_tonghop.CNOIDUNG = Request["cNoiDung"]; }
                if (Request["iTruocKyHop"] != null) { kn_tonghop.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]); }
                var list_tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(kn_tonghop, 0, 2, 2, -1, -1, -1, -1, page, post_per_page);
                //list_tonghop = list_tonghop.Where(x => x.ITINHTRANG == (int)TrangThai_TongHop.DaChuyenBanDanNguyen).ToList();
                ViewData["list"] = kn.KN_Tonghop_ChuyenDanNguyen(list_tonghop);
                if (list_tonghop.Count() > 0)
                {
                    ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)list_tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị đã chuyển Ban Dân nguyện");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult Tonghop_lichsu()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                ViewData["tonghop"] = tonghop;
                ViewData["detail"] = kn.Tonghop_Detail(id, Request.Cookies["url_key"].Value);
                ViewData["list"] = kn.KN_Tonghop_Lichsu(id);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Lịch sử xử lý Tập hợp kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tonghop_sua()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = Request["id"];
                SetTokenAction("kn_tonghop_edit", id);
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                ////ViewData["file"] = kn.File_Edit(id, "kn_tonghop", Request.Cookies["url_key"].Value);
                ViewData["tonghop"] = tonghop;
                if (tonghop.IUSER != u_info.tk_action.iUser && !u_info.tk_action.is_admin)
                {
                    Response.Redirect("/Home/Error/");
                }
                int iKyHop = (int)tonghop.IKYHOP;
                Dictionary<string, object> donvi = new Dictionary<string, object>();
                //List<QUOCHOI_COQUAN> coquan = _kiennghi.GetAll_CoQuanByParam(donvi).Where(x => x.IHIENTHI == 1).OrderBy(x => x.CTEN).ToList();
                int iDonVi = (int)tonghop.IDONVITONGHOP;
                List<KN_CHUONGTRINH> chuongtrinh = _kiennghi.GetAll_ChuongTrinh(iDonVi, iKyHop);
                donvi.Add("ICOQUAN", iDonVi);
                ViewData["dbqh"] = 0;
                if (u_info.tk_action.is_dbqh)
                {
                    ViewData["dbqh"] = 1;
                    ViewData["donvithamquyen"] = GetRadioButton_ThamQuyenTongHop((int)tonghop.ITHAMQUYENDONVI);
                }
                ////ViewData["opt-donvitonghop"] = "<option value='" + iDonVi + "'>" + Server.HtmlEncode(_kiennghi.HienThiThongTinCoQuan(iDonVi).CTEN) + "</option>";

                ////ViewData["opt-kyhop"] = Get_Option_KyHop(iKyHop);
                ////if (u_info.tk_action.is_lanhdao)
                ////{
                ////    ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan((int)tonghop.ITHAMQUYENDONVI, (int)tonghop.ILINHVUC);
                ////    ViewData["dbqh"] = 0;
                ////}
                ////else//đbqh
                ////{
                ////    ViewData["dbqh"] = 1;
                ////}
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop((int)tonghop.ITRUOCKYHOP);
                //ViewData["opt-donvithamquyen"] = kn.OptionCoQuanXuLy(coquan, 0, 0, (int)tonghop.ITHAMQUYENDONVI, 0);
                ////if (u_info.tk_action.is_lanhdao)
                ////{

                ////    Dictionary<string, object> kn_tonghop = new Dictionary<string, object>();
                ////    kn_tonghop.Add("ITONGHOP_BDN", id);
                ////    if (_kiennghi.HienThiDanhSachKienNghi(kn_tonghop).Count()>0)
                ////    {
                ////        QUOCHOI_COQUAN coquan = _kiennghi.HienThiThongTinCoQuan((int)tonghop.ITHAMQUYENDONVI);
                ////        ViewData["opt-donvithamquyen"] = "<option value='" + (int)tonghop.ITHAMQUYENDONVI + "'>" + Server.HtmlEncode(coquan.CTEN) + "</option>";
                ////    }
                ////    else
                ////    {
                ////        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen((int)tonghop.ITHAMQUYENDONVI);
                ////    }
                ////}
                ////else
                ////{
                ////    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen((int)tonghop.ITHAMQUYENDONVI);
                ////}


                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form sửa Tập hợp kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Tonghop_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                UserInfor u_info = GetUserInfor();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ITONGHOP", id);
                List<KN_KIENNGHI> kiennghi = _kiennghi.HienThiDanhSachKienNghi(dic);
                foreach (var k in kiennghi)
                {
                    KN_KIENNGHI k1 = k; k1.ITONGHOP = 0;
                    _kiennghi.UpdateThongTinKienNghi(k1);
                    _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)k1.IKIENNGHI, "Hủy Tập hợp kiến nghị");
                }
                List<TRACKING> tracking = _kiennghi.HienThiDanhSachTracKing(dic);
                foreach (var k in tracking)
                {
                    _kiennghi.Delete_KienNghi_Tracking(k);
                }
                _kiennghi.DelteTongHop(tonghop);

                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xóa Tập hợp kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Chuyen_Donvi_xuly(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                Dictionary<string, object> _kn = new Dictionary<string, object>();
                _kn.Add("ICOQUAN", tonghop.ITHAMQUYENDONVI);
                var coquanchon = _kiennghi.GetAll_CoQuanByParam(_kn).FirstOrDefault();
                _kn.Add("IHIENTHI", 1);
                //var coquan = _kiennghi.GetAll_CoQuanByParam(_kn);
                int iDonViTongHop = (int)tonghop.IDONVITONGHOP;
                ViewData["id"] = fc["id"];
                SetTokenAction("Chuyen_Donvi_xuly", id);
                //ViewData["opt_donvi"] = kn.OptionCoQuanXuLy(coquan);
                string ten_donvi_xuly = "";
                QUOCHOI_COQUAN donvi_xuly = _kiennghi.HienThiThongTinCoQuan((int)tonghop.ITHAMQUYENDONVI);
                if (donvi_xuly != null)
                {
                    ten_donvi_xuly = donvi_xuly.CTEN;
                }
                ViewData["radio-thamquyen"] = "";

                ViewData["diaphuong"] = tonghop.ITHAMQUYENDONVI == ID_Ban_DanNguyen_New ? "1" : "0";
                ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi_ChuyenDonVi(Int32.Parse(fc["loaiTaphop"]));
                ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType(Int32.Parse(fc["loaiTaphop"]), (int)tonghop.ITHAMQUYENDONVI);
                if (u_info.tk_action.is_lanhdao)

                {
                    ViewData["is_dbqh"] = "0";
                    ViewData["opt-donvithamquyen"] = "";
                    if (donvi_xuly.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    {
                        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong, (int)tonghop.ITHAMQUYENDONVI);
                        ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi_ChuyenDonVi((int)ThamQuyen_DiaPhuong.Trunguong);
                    }

                    if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    {
                        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Tinh, (int)tonghop.ITHAMQUYENDONVI);
                        ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi_ChuyenDonVi((int)ThamQuyen_DiaPhuong.Tinh);
                    }

                    if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                    {
                        ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Huyen, (int)tonghop.ITHAMQUYENDONVI);
                        ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi_ChuyenDonVi((int)ThamQuyen_DiaPhuong.Huyen);
                    }
                }
                //ViewData["ten_donvi_xuly"] = ten_donvi_xuly;
                return PartialView("../Ajax/Kiennghi/Chuyen_Donvi_xuly");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form chuyển đơn vị xử lý Tập hợp kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Chuyen_Xuly_Tralai(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var chuyenxuly = _kiennghi.GetChuyenXuLy_ById(id);
                var nguoinhan = _thietlap.Get_User((int)chuyenxuly.INGUOINHAN);
                ViewData["id"] = fc["id"];
                //SetTokenAction("kn_Chuyen_Xuly_tonghop", id);
                //ViewData["opt_donvi"] = "<option value='" + ID_Ban_DanNguyen + "'>" +
                //                        _kiennghi.HienThiThongTinCoQuan(ID_Ban_DanNguyen).CTEN + "</option>" +
                //                        "<option value='" + iDonViTongHop + "'>" +
                //                        _kiennghi.HienThiThongTinCoQuan(iDonViTongHop).CTEN + "</option>";
                ViewData["opt_donvi"] = Get_Option_ThamQuyen_DiaPhuong_Parent_VP_DBQH_HDND((int)nguoinhan.IPHONGBAN);
                ViewData["opt-chuyenvien"] = Option_Chuyen_Vien((int)nguoinhan.IPHONGBAN, (int)chuyenxuly.INGUOINHAN);
                ViewData["opt-cNoiDung"] = chuyenxuly.CNOIDUNG;
                SetTokenAction("Ajax_Chuyen_Xuly_Chuyenvien", id);
                return PartialView("../Ajax/Kiennghi/Chuyen_Xuly_Tralai");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form chuyển đơn vị xử lý Tập hợp kiến nghị");
                throw;
            }

        }
        public ActionResult Chuyen_Xuly_Chuyenvien_update(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                var chuyenxuly = _kiennghi.GetChuyenXuLy_ById(id);
                if (!CheckTokenAction("Ajax_Chuyen_Xuly_Chuyenvien", id)) { Response.Redirect("/Home/Error/"); return null; }
                DateTime dDate = DateTime.Now;
                UserInfor u_info = GetUserInfor();
                chuyenxuly.INGUOICHUYEN = u_info.user_login.IUSER;
                chuyenxuly.INGUOINHAN = Convert.ToInt32(fc["iNguoiNhan"]);
                chuyenxuly.IDONVI = Convert.ToInt32(fc["iDonVi"]);
                chuyenxuly.CNOIDUNG = fc["cNoiDung"];
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ITONGHOP", chuyenxuly.ITONGHOP);
                var listKienNghi = _kiennghi.HienThiDanhSachKienNghi(dic);
                foreach (var item in listKienNghi)
                {
                    item.IUSER = chuyenxuly.INGUOINHAN;
                    _kiennghi.UpdateThongTinKienNghi(item);
                }
                _kiennghi.Update_Chuyenxuly(chuyenxuly);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Insert chuyển xử lý");
                return View("../Home/Error_Exception");
            }
        }
        public JsonResult Ajax_Update_ListToDaiBieu()
        {
            string str = "";
            str += "<option value='0'>Chọn tổ đại biểu</option>";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IPARENT", ID_PARENT_TODAIBIEU);
            var listToDaiBIeu = _thietlap.GetBy_List_Phongban(dic);
            foreach (var item in listToDaiBIeu)
            {
                str += "<option value='" + item.IPHONGBAN + "'>" + item.CTEN + "</option>";
            }
            Response.Write(str);
            return null;
        }

        public string Option_Chuyen_Vien(int iPhongBan = 0, int iUser = 0)
        {
            string str = "";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IPHONGBAN", iPhongBan);
            List<USERS> userList = kntc.List_User(dic);
            foreach (var item in userList)
            {
                str += "<option ";
                if (iUser == item.IUSER)
                    str += "selected ";
                str += "value='" + item.IUSER + "'>" + item.CTEN + "</option>";
            }
            return str;
        }
        public JsonResult Ajax_Update_ListChuyenVien(int val)
        {
            string str = "";
            str += "<option value='0'>Chọn chuyên viên</option>";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IPHONGBAN", val);
            List<USERS> userList = kntc.List_User(dic);
            foreach (var item in userList)
            {
                str += "<option value='" + item.IUSER + "'>" + item.CTEN + "</option>";
            }
            Response.Write(str);
            return null;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Chuyen_Xuly_Chuyenvien_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Ajax_Chuyen_Xuly_Chuyenvien", id)) { Response.Redirect("/Home/Error/"); return null; }
                DateTime dDate = DateTime.Now;
                KN_CHUYENXULY chuyenxuly = new KN_CHUYENXULY();
                UserInfor u_info = GetUserInfor();
                chuyenxuly.INGUOICHUYEN = u_info.user_login.IUSER;
                chuyenxuly.INGUOINHAN = Convert.ToInt32(fc["iNguoiNhan"]);
                chuyenxuly.IDONVI = Convert.ToInt32(fc["iDonVi"]);
                chuyenxuly.CNOIDUNG = fc["cNoiDung"];
                chuyenxuly.ITONGHOP = id;
                _kiennghi.Insert_Chuyenxuly(chuyenxuly);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ITONGHOP", chuyenxuly.ITONGHOP);
                var listKienNghi = _kiennghi.HienThiDanhSachKienNghi(dic);
                foreach (var item in listKienNghi)
                {
                    item.IUSER = chuyenxuly.INGUOINHAN;
                    _kiennghi.UpdateThongTinKienNghi(item);
                }
                _kiennghi.Update_Chuyenxuly(chuyenxuly);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Insert chuyển xử lý");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Chuyen_Xuly_Chuyenvien(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                ViewData["id"] = fc["id"];
                //SetTokenAction("kn_Chuyen_Xuly_tonghop", id);
                //ViewData["opt_donvi"] = "<option value='" + ID_Ban_DanNguyen + "'>" +
                //                        _kiennghi.HienThiThongTinCoQuan(ID_Ban_DanNguyen).CTEN + "</option>" +
                //                        "<option value='" + iDonViTongHop + "'>" +
                //                        _kiennghi.HienThiThongTinCoQuan(iDonViTongHop).CTEN + "</option>";
                string ten_donvi_xuly = Server.HtmlEncode(u_info.tk_action.tendonvi);
                ViewData["opt_donvi"] = Get_Option_ThamQuyen_DiaPhuong_Parent_VP_DBQH_HDND(0);
                ViewData["opt-chuyenvien"] = Option_Chuyen_Vien(0, 0);
                ViewData["ten_donvi_xuly"] = ten_donvi_xuly;
                SetTokenAction("Ajax_Chuyen_Xuly_Chuyenvien", id);
                return PartialView("../Ajax/Kiennghi/Chuyen_Xuly_Chuyenvien");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form chuyển đơn vị xử lý Tập hợp kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Chuyen_Xuly_tonghop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor(); ;
                ViewData["id"] = fc["id"];
                SetTokenAction("kn_Chuyen_Xuly_tonghop", id);
                //ViewData["opt_donvi"] = "<option value='" + ID_Ban_DanNguyen + "'>" +
                //                        _kiennghi.HienThiThongTinCoQuan(ID_Ban_DanNguyen).CTEN + "</option>" +
                //                        "<option value='" + iDonViTongHop + "'>" +
                //                        _kiennghi.HienThiThongTinCoQuan(iDonViTongHop).CTEN + "</option>";
                string ten_donvi_xuly = Server.HtmlEncode(u_info.tk_action.tendonvi);
                ViewData["opt_donvi"] = Get_Option_ThamQuyen_DiaPhuong(0);
                ViewData["ten_donvi_xuly"] = ten_donvi_xuly;
                return PartialView("../Ajax/Kiennghi/Chuyen_Xuly_tonghop");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form chuyển đơn vị xử lý Tập hợp kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_traloi_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI traloi = _kiennghi.Get_TraLoi_KienNghi(id);
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(Convert.ToInt32(traloi.IKIENNGHI));
                ViewData["id"] = fc["id"];
                ViewData["traloi"] = traloi;
                var phanloai0 = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
                KN_TRALOI_PHANLOAI traloi_phanloai;
                if (traloi.IPHANLOAI != (int)PhanLoai_TraLoiKienNghi.GIAITRINH_CUNGCAPTHONGTIN)
                {
                    traloi_phanloai = phanloai0.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI && x.IPARENT != 0).FirstOrDefault();
                }
                else
                {
                    traloi_phanloai = phanloai0.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).FirstOrDefault();
                }

                if (traloi_phanloai != null)
                {
                    ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == 0).ToList(), (int)traloi_phanloai.IPARENT);
                    if (traloi.IPHANLOAI != (int)PhanLoai_TraLoiKienNghi.GIAITRINH_CUNGCAPTHONGTIN)
                    {
                        var traloi_phanloai_parent = phanloai0.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).ToList();
                        if (traloi_phanloai_parent.Count() > 0)
                        {
                            int IPhanLoai_Parent = (int)traloi_phanloai_parent.FirstOrDefault().IPARENT;
                            ViewData["select-phanloai"] = "<select onchange=\"ChangePhanLoai1(this.value," + (int)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH + ")\" class='input-block-level' name='iPhanLoai1' id='iPhanLoai1'>" +
                            kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == IPhanLoai_Parent).ToList(), (int)traloi.IPHANLOAI) + "</select>";
                        }

                    }
                }
                else
                {
                    ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == 0).ToList(), (int)traloi.IPHANLOAI);
                }

                ViewData["opt-donvitraloi"] = Get_Option_CoQuan(Convert.ToInt32(kiennghi.ITHAMQUYENDONVI));

                ViewData["file"] = kn.File_Edit(id, "kn_traloi", Request.Cookies["url_key"].Value);
                ViewData["colotrinh"] = "style='display:none'";
                if (traloi.IPHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                {
                    ViewData["colotrinh"] = "";
                }
                ViewData["thamquyen_diaphuong"] = 0;
                if (u_info.tk_action.is_dbqh)
                {
                    ViewData["thamquyen_diaphuong"] = 1;
                }
                SetTokenAction("Kiennghi_traloi_edit", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_traloi_edit");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form sửa trả lời kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_tra_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI_TRALOI traloi = _kiennghi.Get_TraLoi_KienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["traloi"] = traloi;
                var phanloai0 = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
                KN_TRALOI_PHANLOAI traloi_phanloai;
                if (traloi.IPHANLOAI != (int)PhanLoai_TraLoiKienNghi.GIAITRINH_CUNGCAPTHONGTIN)
                {
                    traloi_phanloai = phanloai0.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI && x.IPARENT != 0).FirstOrDefault();
                }
                else
                {
                    traloi_phanloai = phanloai0.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).FirstOrDefault();
                }

                if (traloi_phanloai != null)
                {
                    ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == 0).ToList(), (int)traloi_phanloai.IPARENT);
                    if (traloi.IPHANLOAI != (int)PhanLoai_TraLoiKienNghi.GIAITRINH_CUNGCAPTHONGTIN)
                    {
                        var traloi_phanloai_parent = phanloai0.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).ToList();
                        if (traloi_phanloai_parent.Count() > 0)
                        {
                            int IPhanLoai_Parent = (int)traloi_phanloai_parent.FirstOrDefault().IPARENT;
                            ViewData["select-phanloai"] = "<select onchange=\"ChangePhanLoai1(this.value," + (int)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH + ")\" id='iPhanLoai1' class='input-block-level' name='iPhanLoai1'>" +
                            kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == IPhanLoai_Parent).ToList(), (int)traloi.IPHANLOAI) + "</select>";
                        }

                    }
                }
                else
                {
                    ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == 0).ToList(), 0);
                }
                ViewData["colotrinh"] = "style='display:none'";
                if (traloi.IPHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                {
                    ViewData["colotrinh"] = "";
                }
                ViewData["file"] = kn.File_Edit(id, "kn_traloi", Request.Cookies["url_key"].Value);
                SetTokenAction("Kiennghi_traloi_edit", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_edit");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form sửa kiến nghị trả lại");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = _kiennghi.Get_TraLoi_KienNghi(id);
                int iPhanLoai = Convert.ToInt32(fc["iPhanLoai"]);
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                t.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                t.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                if (iPhanLoai != (int)PhanLoai_TraLoiKienNghi.GIAITRINH_CUNGCAPTHONGTIN && fc["iPhanLoai1"] != null)
                {
                    iPhanLoai = Convert.ToInt32(fc["iPhanLoai1"]);
                    if (iPhanLoai == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                    {

                    }
                }
                if (fc["DNGAY_DUKIEN"] != null)
                {
                    if (fc["DNGAY_DUKIEN"].ToString() != "")
                    {
                        t.DNGAY_DUKIEN = Convert.ToDateTime(func.ConvertDateToSql(fc["DNGAY_DUKIEN"]));
                    }
                }
                t.IPHANLOAI = iPhanLoai;
                //t.CLOTRINH = func.RemoveTagInput(fc["cLoTrinh"]);
                _kiennghi.UpdateTraLoi_KienNghi(t);
                List<FILE_UPLOAD> file_upload = new List<FILE_UPLOAD>();
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                        file_upload.Add(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)t.IKIENNGHI);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    Update_Traloi_all_kiennghi_cungnoidung(t, file_upload);
                //}


                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Sửa lý do trả lại kiến nghị");
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Update lý do trả lại kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Kiennghi_tra_chuyenkysau(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["opt-phanloai"] = Get_Option_KN_Traloi_Phanloai();
                SetTokenAction("Kiennghi_tra_chuyenkysau", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_chuyenkysau");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form chuyển kiến nghị theo dõi ở kỳ sau");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_tra_chuyenkysau_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI_TRALOI traloi = _kiennghi.Get_TraLoi_KienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["traloi"] = traloi;
                ViewData["opt-phanloai"] = Get_Option_KN_Traloi_Phanloai((int)traloi.IPHANLOAI);
                ViewData["file"] = kn.File_Edit(id, "kn_traloi", Request.Cookies["url_key"].Value);
                SetTokenAction("Kiennghi_tra_chuyenkysau_edit", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_chuyenkysau_edit");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form sửa chuyển kiến nghị theo dõi ở kỳ sau");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_chuyenkysau_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_chuyenkysau_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = _kiennghi.Get_TraLoi_KienNghi(id);
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                _kiennghi.UpdateTraLoi_KienNghi(t);
                List<FILE_UPLOAD> file_upload = new List<FILE_UPLOAD>();
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                        file_upload.Add(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)t.IKIENNGHI);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    Update_Traloi_all_kiennghi_cungnoidung(t, file_upload);
                //}
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Sửa lý do chuyển theo dõi kỳ sau");
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "update lý do chuyển kiến nghị theo dõi ở kỳ sau");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Kiennghi_tra_lotrinh(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["opt-phanloai"] = Get_Option_KN_Traloi_Phanloai();
                SetTokenAction("Kiennghi_tra_lotrinh", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_lotrinh");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form cập nhật lộ trình giải quyết kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_tra_lotrinh_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI_TRALOI traloi = _kiennghi.Get_TraLoi_KienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["traloi"] = traloi;
                ViewData["opt-phanloai"] = Get_Option_KN_Traloi_Phanloai((int)traloi.IPHANLOAI);
                ViewData["file"] = kn.File_Edit(id, "kn_traloi", Request.Cookies["url_key"].Value);
                SetTokenAction("Kiennghi_tra_lotrinh_edit", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_lotrinh_edit");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form sửa lộ trình giải quyết kiến nghị");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_lotrinh_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_lotrinh_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = _kiennghi.Get_TraLoi_KienNghi(id);
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                if (fc["DNGAY_DUKIEN"].ToString() != "")
                {
                    t.DNGAY_DUKIEN = Convert.ToDateTime(func.ConvertDateToSql(fc["DNGAY_DUKIEN"]));
                }
                else
                {
                    Nullable<DateTime> date = null;
                    t.DNGAY_DUKIEN = date;
                }
                _kiennghi.UpdateTraLoi_KienNghi(t);
                List<FILE_UPLOAD> file_upload = new List<FILE_UPLOAD>();
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                        file_upload.Add(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)t.IKIENNGHI);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    Update_Traloi_all_kiennghi_cungnoidung(t, file_upload);
                //}
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Sửa lộ trình giải quyết");
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Update lộ trình giải quyết kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Kiennghi_tra_nguonluc_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI_TRALOI traloi = _kiennghi.Get_TraLoi_KienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["traloi"] = traloi;
                ViewData["opt-phanloai"] = Get_Option_KN_Traloi_Phanloai((int)traloi.IPHANLOAI);
                ViewData["file"] = kn.File_Edit(id, "kn_traloi", Request.Cookies["url_key"].Value);
                SetTokenAction("Kiennghi_tra_nguonluc_edit", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_nguonluc_edit");

            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form sửa chưa có nguồn lực giải quyết kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_tra_chuagiaiquyet_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI_TRALOI traloi = _kiennghi.Get_TraLoi_KienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["traloi"] = traloi;
                ViewData["opt-phanloai"] = Get_Option_KN_Traloi_Phanloai((int)traloi.IPHANLOAI);
                ViewData["file"] = kn.File_Edit(id, "kn_traloi", Request.Cookies["url_key"].Value);
                SetTokenAction("Kiennghi_tra_chuagiaiquyet_edit", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_chuagiaiquyet_edit");

            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form sửa lý do chưa giải quyết kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_tra_nguonluc(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["opt-phanloai"] = Get_Option_KN_Traloi_Phanloai();
                SetTokenAction("Kiennghi_tra_nguonluc", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_nguonluc");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form cập nhật lý do chưa có nguồn lực giải quyết kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_tra_chuagiaiquyet(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["opt-phanloai"] = Get_Option_KN_Traloi_Phanloai();
                SetTokenAction("Kiennghi_tra_chuagiaiquyet", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_chuagiaiquyet");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form cập nhật lý do chưa giải quyết kiến nghị");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_tra_chualotrinh(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["opt-phanloai"] = Get_Option_KN_Traloi_Phanloai();
                SetTokenAction("Kiennghi_tra_chualotrinh", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_chualotrinh");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form cập nhật lý do chưa có lộ trình giải quyết");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_tra_chualotrinh_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI_TRALOI traloi = _kiennghi.Get_TraLoi_KienNghi(id);
                ViewData["id"] = fc["id"];
                ViewData["traloi"] = traloi;
                ViewData["opt-phanloai"] = Get_Option_KN_Traloi_Phanloai((int)traloi.IPHANLOAI);
                ViewData["file"] = kn.File_Edit(id, "kn_traloi", Request.Cookies["url_key"].Value);
                SetTokenAction("Kiennghi_tra_chualotrinh_edit", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra_chualotrinh_edit");

            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form sửa lý do chưa có lộ trình giải quyết");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_nguonluc_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_nguonluc", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = new KN_KIENNGHI_TRALOI();
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.IUSER = u_info.tk_action.iUser; t.DDATE = DateTime.Now;
                t.IKIENNGHI = id;
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                t.ITINHTRANG = (decimal)TrangThai_TraLoiKienNghi.ChuaCoNguonLucGiaiQuyet;
                _kiennghi.InsertTraLoi_KienNghi(t);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi;//đã trả lời
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, id, "Trả lời kiến nghị");
                //    //cập nhật cho các kiến nghị cùng nội dung
                //    Traloi_all_kiennghi_cungnoidung(kiennghi, t);
                //}


                //Traloi_all_kiennghi_cungnoidung(kiennghi, t);
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                //kiennghi.ITINHTRANG = 9;
                //_kiennghi.UpdateThongTinKienNghi(kiennghi);
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Cập nhật kết quả trả lời:</br> Chưa có nguồn lực giải quyết: " + t.CTRALOI);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Insert lý do chưa có nguồn lực giải quyết");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_chuagiaiquyet_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_chuagiaiquyet", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = new KN_KIENNGHI_TRALOI();
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.IUSER = u_info.tk_action.iUser; t.DDATE = DateTime.Now;
                t.IKIENNGHI = id;
                t.ITINHTRANG = (decimal)TrangThai_TraLoiKienNghi.ChuaTheGiaiQuyetNgay;
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                _kiennghi.InsertTraLoi_KienNghi(t);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi;//đã trả lời
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, id, "Trả lời kiến nghị");
                //    //cập nhật cho các kiến nghị cùng nội dung
                //    Traloi_all_kiennghi_cungnoidung(kiennghi, t);
                //}
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                //kiennghi.ITINHTRANG = 9;
                //_kiennghi.UpdateThongTinKienNghi(kiennghi);
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Cập nhật kết quả trả lời:</br> Chưa thể giải quyết ngay: " + t.CTRALOI);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Insert cập nhật lý do chưa thể giải quyết ngay kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_nguonluc_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_nguonluc_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = _kiennghi.Get_TraLoi_KienNghi(id);
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                _kiennghi.UpdateTraLoi_KienNghi(t);
                List<FILE_UPLOAD> file_upload = new List<FILE_UPLOAD>();
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                        file_upload.Add(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)t.IKIENNGHI);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    Update_Traloi_all_kiennghi_cungnoidung(t, file_upload);
                //}
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Sửa nội dung kết quả chưa có nguồn lực giải quyết");
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Update lý do chưa có nguồn lực giải quyết");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_chuagiaiquyet_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_chuagiaiquyet_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = _kiennghi.Get_TraLoi_KienNghi(id);
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                _kiennghi.UpdateTraLoi_KienNghi(t);
                List<FILE_UPLOAD> file_upload = new List<FILE_UPLOAD>();
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                        file_upload.Add(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)t.IKIENNGHI);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    Update_Traloi_all_kiennghi_cungnoidung(t, file_upload);
                //}
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Sửa lý do chưa thể giải quyết ngay");
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Update lý do chưa giải quyết ngay kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_chualotrinh_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_chualotrinh_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = _kiennghi.Get_TraLoi_KienNghi(id);
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                _kiennghi.UpdateTraLoi_KienNghi(t);
                List<FILE_UPLOAD> file_upload = new List<FILE_UPLOAD>();
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                        file_upload.Add(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)t.IKIENNGHI);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    Update_Traloi_all_kiennghi_cungnoidung(t, file_upload);
                //}
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Sửa lý do chưa có lộ trình giải quyết");
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Update lý do chưa có lộ trình giải quyết");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Kiennghi_tra(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["id"] = fc["id"];
                var phanloai0 = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
                ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0, 0);
                SetTokenAction("Kiennghi_traloi", id);
                return PartialView("../Ajax/Kiennghi/Kiennghi_tra");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form trả lại kiến nghị");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_chualotrinh_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_chualotrinh", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = new KN_KIENNGHI_TRALOI();
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.IUSER = u_info.tk_action.iUser; t.DDATE = DateTime.Now;
                t.IKIENNGHI = id;
                t.ITINHTRANG = (decimal)TrangThai_TraLoiKienNghi.ChuaCoLoTrinhGiaiQuyet;
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                _kiennghi.InsertTraLoi_KienNghi(t);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi;//đã trả lời
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, id, "Trả lời kiến nghị");
                //    //cập nhật cho các kiến nghị cùng nội dung
                //    Traloi_all_kiennghi_cungnoidung(kiennghi, t);
                //}
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Cập nhật kết quả trả lời:</br> Chưa có lộ trình giải quyết: " + t.CTRALOI);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Insert kết quả trả lời: Chưa có lộ trình giải quyết ");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_lotrinh_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_lotrinh", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = new KN_KIENNGHI_TRALOI();
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                t.IUSER = u_info.tk_action.iUser;
                if (fc["DNGAY_DUKIEN"].ToString() != "")
                {
                    t.DNGAY_DUKIEN = Convert.ToDateTime(func.ConvertDateToSql(fc["DNGAY_DUKIEN"]));
                }
                t.DDATE = DateTime.Now;
                t.IKIENNGHI = id;
                t.ITINHTRANG = (decimal)TrangThai_TraLoiKienNghi.CoLoTrinhGiaiQuyet;
                _kiennghi.InsertTraLoi_KienNghi(t);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                    }
                }

                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi;//đã trả lời
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, id, "Trả lời kiến nghị");
                //    //cập nhật cho các kiến nghị cùng nội dung
                //    Traloi_all_kiennghi_cungnoidung(kiennghi, t);
                //}
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Cập nhật kết quả trả lời:</br> Có lộ trình giải quyết: " + t.CTRALOI);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Update kết quả trả lời: Có lộ trình giải quyết ");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_chuyenkysau_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra_chuyenkysau", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = new KN_KIENNGHI_TRALOI();
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.IUSER = u_info.tk_action.iUser; t.DDATE = DateTime.Now;
                t.IKIENNGHI = id;
                t.ITINHTRANG = (decimal)TrangThai_TraLoiKienNghi.ChuyenGiaiQuyetKyHopSau;
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                _kiennghi.InsertTraLoi_KienNghi(t);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi;//đã trả lời
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, id, "Trả lời kiến nghị");
                //    //cập nhật cho các kiến nghị cùng nội dung
                //    Traloi_all_kiennghi_cungnoidung(kiennghi, t);
                //}
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                //kiennghi.ITINHTRANG = 9;
                //_kiennghi.UpdateThongTinKienNghi(kiennghi);
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Cập nhật kết quả trả lời:</br> Chuyển theo dõi kỳ họp sau với lý do: " + t.CTRALOI);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Insert kết quả trả lời: Chuyển theo dõi kỳ họp sau ");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_tra_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_tra", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = new KN_KIENNGHI_TRALOI();
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.IUSER = u_info.tk_action.iUser; t.DDATE = DateTime.Now;
                t.IKIENNGHI = id;
                t.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                t.ITINHTRANG = (decimal)TrangThai_TraLoiKienNghi.TraLaiKienNghi;
                _kiennghi.InsertTraLoi_KienNghi(t);
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                    }
                }
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.DaTraLaiKienNghi;//đã trả lại
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, id, "Trả lời kiến nghị");
                //    //cập nhật cho các kiến nghị cùng nội dung
                //    Traloi_all_kiennghi_cungnoidung(kiennghi, t);
                //}

                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Cập nhật kết quả trả lời:</br>Trả lại kiến nghị với lý do: " + t.CTRALOI);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Insert kết quả trả lời: Trả lại kiến nghị ");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Kiennghi_traloi(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["id"] = fc["id"];
                var phanloai0 = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();

                ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0, 0);
                //ViewData["opt-donvitraloi"] = Get_Option_CoQuan(Convert.ToInt32(kiennghi.ITHAMQUYENDONVI));

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ICOQUAN", kiennghi.ITHAMQUYENDONVI);
                var coquanchon = _kiennghi.GetAll_CoQuanByParam(param).FirstOrDefault();
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Trunguong)).GetCustomAttribute<StringValueAttribute>(false).Value)
                {
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong, (int)kiennghi.ITHAMQUYENDONVI);
                    ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Trunguong);
                }
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Tinh)).GetCustomAttribute<StringValueAttribute>(false).Value)
                {
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Tinh, (int)kiennghi.ITHAMQUYENDONVI);
                    ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Tinh);
                }
                if (coquanchon.CTYPE == typeof(ThamQuyen_DiaPhuong).GetField(nameof(ThamQuyen_DiaPhuong.Huyen)).GetCustomAttribute<StringValueAttribute>(false).Value)
                {
                    ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Huyen, (int)kiennghi.ITHAMQUYENDONVI);
                    ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi((int)ThamQuyen_DiaPhuong.Huyen);
                }

                SetTokenAction("Kiennghi_traloi", id);
                ViewData["thamquyen_diaphuong"] = 0;
                if (u_info.tk_action.is_dbqh)
                {
                    ViewData["thamquyen_diaphuong"] = 1;
                }
                return PartialView("../Ajax/Kiennghi/Kiennghi_traloi");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form trả lời kiến nghị");
                throw;
            }

        }

        public string Get_Option_CoQuan(int thamquyendonvi)
        {
            List<QUOCHOI_COQUAN> coquan;
            //if (thamquyendonvi == AppConfig.ID_BAN_DAN_NGUYEN_NEW)
            //{
            //    coquan = _kiennghi.GetAll_KN_COQUAN_THUOCTHAMQUYEN().Where(x => x.IPARENT == 2962).ToList();
            //}
            //else
            //{
            //    coquan = _kiennghi.GetAll_KN_COQUAN_THUOCTHAMQUYEN().Where(x => x.IPARENT == 1 || x.IPARENT == 11 || x.IPARENT == 20 || x.IPARENT == 226
            //                                                                    || x.IPARENT == 228 || x.IPARENT == 252).ToList();
            //}
            coquan = _kiennghi.GetAll_KN_COQUAN_THUOCTHAMQUYEN();
            return kn.Option_CoQuan(coquan, thamquyendonvi);
        }

        public ActionResult Ajax_Kiennghi_danhgia(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["id"] = fc["id"];
                //var phanloai = _kiennghi.GetAll_GiamSat_PhanLoai();
                //var danhgia = _kiennghi.GetAll_GiamSat_DanhGia();
                var phanloai0 = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
                SetTokenAction("Kiennghi_danhgia", id);
                Dictionary<string, object> dic_ = new Dictionary<string, object>();
                dic_.Add("ITRALOI", id);
                List<KN_KIENNGHI_TRALOI> traloi_list = _kiennghi.GetAll_TraLoi_KienNghi_ByParamt(dic_);
                if (traloi_list.Count() > 0)
                {
                    KN_KIENNGHI_TRALOI traloi = traloi_list.FirstOrDefault();
                    KN_TRALOI_PHANLOAI traloi_phanloai = phanloai0.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).FirstOrDefault();
                    if (traloi_phanloai != null)
                    {
                        if (traloi.IPHANLOAI == (int)PhanLoai_TraLoiKienNghi.GIAITRINH_CUNGCAPTHONGTIN)
                        {
                            ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == 0).ToList(), (int)traloi.IPHANLOAI);
                            if (phanloai0.Where(x => x.IPARENT == (int)traloi.IPHANLOAI).Count() > 0)
                            {
                                ViewData["select-phanloai"] = "<select class='input-block-level' name='iPhanLoai1'>" +
                                kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == (int)traloi.IPHANLOAI).ToList(), 0) + "</select>";
                            }

                        }
                        else
                        {
                            ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == 0).ToList(), (int)traloi_phanloai.IPARENT);
                            var traloi_phanloai_parent = phanloai0.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).ToList();
                            if (traloi_phanloai_parent.Count() > 0)
                            {
                                int IPhanLoai_Parent = (int)traloi_phanloai_parent.FirstOrDefault().IPARENT;
                                ViewData["select-phanloai"] = "<select class='input-block-level' name='iPhanLoai1'>" +
                                kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == IPhanLoai_Parent).ToList(), (int)traloi.IPHANLOAI) + "</select>";
                            }
                        }
                    }
                    else
                    {
                        ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == 0).ToList(), 0);
                    }
                }
                else
                {
                    ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == 0).ToList(), 0);
                }

                //ViewData["opt-phanloai"] = kn.Option_GiamSat_PhanLoai(phanloai, 0);
                //ViewData["opt-danhgia"] = kn.Option_GiamSat_Danhgia(danhgia, 0);
                return PartialView("../Ajax/Kiennghi/Kiennghi_danhgia");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form đánh giá trả lời kiến nghị ");
                throw;
            }

        }
        public ActionResult Ajax_Kiennghi_danhgia_chuyenkysau_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string id_encr = fc["id"];
                int id = Convert.ToInt32(HashUtil.Decode_ID(id_encr, Request.Cookies["url_key"].Value));
                ViewData["id"] = id_encr;
                KN_GIAMSAT giamsat = _kiennghi.Get_Giamsat_TraLoi(id);
                ViewData["giamsat"] = giamsat;
                var phanloai = _kiennghi.GetAll_GiamSat_PhanLoai();
                var danhgia = _kiennghi.GetAll_GiamSat_DanhGia();
                SetTokenAction("Kiennghi_danhgia_edit", id);
                ViewData["opt-phanloai"] = kn.Option_GiamSat_PhanLoai(phanloai, (int)giamsat.IPHANLOAI);
                ViewData["opt-danhgia"] = kn.Option_GiamSat_Danhgia(danhgia, (int)giamsat.IDANHGIA);
                return PartialView("../Ajax/Kiennghi/Kiennghi_danhgia_chuyenkysau_edit");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form sửa đánh giá trả lời kiến nghị ");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_danhgia_chuyenkysau_update(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_danhgia_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                UserInfor u_info = GetUserInfor();
                KN_GIAMSAT giamsat = _kiennghi.Get_Giamsat_TraLoi(id);
                giamsat.IDONGKIENNGHI = Convert.ToInt32(fc["iDongKienNghi"]);
                giamsat.IDANHGIA = Convert.ToInt32(fc["iDanhGia"]);
                giamsat.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai"]);
                giamsat.IDUNGTIENDO = Convert.ToInt32(fc["iDungTienDo"]);
                _kiennghi.Update_Giamsat_traloi(giamsat);
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)giamsat.IKIENNGHI, "Sửa đánh giá trả lời kiến nghị");
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)giamsat.IKIENNGHI);



                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Update đánh giá trả lời kiến nghị ");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Kiennghi_danhgia_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string id_encr = fc["id"];
                int id = Convert.ToInt32(HashUtil.Decode_ID(id_encr, Request.Cookies["url_key"].Value));
                ViewData["id"] = id_encr;
                KN_GIAMSAT giamsat = _kiennghi.Get_Giamsat_TraLoi(id);
                ViewData["giamsat"] = giamsat;
                var phanloai0 = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
                KN_TRALOI_PHANLOAI traloi_phanloai = phanloai0.Where(x => x.IPHANLOAI == (int)giamsat.IPHANLOAI).FirstOrDefault();

                if (traloi_phanloai != null)
                {
                    ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == 0).ToList(), (int)traloi_phanloai.IPARENT);
                    var traloi_phanloai_parent = phanloai0.Where(x => x.IPHANLOAI == (int)giamsat.IPHANLOAI).ToList();
                    if (traloi_phanloai_parent.Count() > 0)
                    {
                        int IPhanLoai_Parent = (int)traloi_phanloai_parent.FirstOrDefault().IPARENT;
                        ViewData["select-phanloai"] = "<select class='input-block-level' name='iPhanLoai1'>" +
                        kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == IPhanLoai_Parent).ToList(), (int)giamsat.IPHANLOAI) + "</select>";
                    }
                }
                else
                {
                    ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi_Parent(phanloai0.Where(x => x.IPARENT == 0).ToList(), 0);
                }
                SetTokenAction("Kiennghi_danhgia_edit", id);
                //ViewData["opt-phanloai"] = kn.Option_GiamSat_PhanLoai(phanloai, (int)giamsat.IPHANLOAI);
                //ViewData["opt-danhgia"] = kn.Option_GiamSat_Danhgia(danhgia, (int)giamsat.IDANHGIA);
                return PartialView("../Ajax/Kiennghi/Kiennghi_danhgia_edit");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form sửa đánh giá trả lời kiến nghị ");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_danhgia_insert(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {

                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_danhgia", id)) { Response.Redirect("/Home/Error/"); return null; }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI traloi = _kiennghi.Get_TraLoi_KienNghi(id);
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)traloi.IKIENNGHI);
                KN_GIAMSAT giamsat = new KN_GIAMSAT();
                //giamsat.IDANHGIA = Convert.ToInt32(fc["iDanhGia"]);
                giamsat.IDONGKIENNGHI = Convert.ToInt32(fc["iDongKienNghi"]);
                giamsat.IDUNGTIENDO = Convert.ToInt32(fc["iDungTienDo"]);
                giamsat.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai1"]);
                giamsat.IKIENNGHI = (int)traloi.IKIENNGHI;
                giamsat.ITRALOI = id;
                giamsat.IUSER = u_info.tk_action.iUser;
                giamsat.DDATE = DateTime.Now;
                _kiennghi.Insert_Giamsat_traloi(giamsat);

                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    if (giamsat.IDONGKIENNGHI == 0)// chuyển theo dõi kỳ sau
                //    {                    
                //        kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.TheoDoiKyHopSau;
                //        _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //        _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)traloi.IKIENNGHI, "Đánh giá trả lời kiến nghị: Chuyển theo dõi kỳ họp sau.");
                //    }
                //    else
                //    {
                //        _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)traloi.IKIENNGHI, "Đánh giá trả lời kiến nghị: Đóng kiến nghị");
                //    }
                //    //Danhgia_all_kiennghi_cungnoidung(kiennghi, giamsat);
                //}

                //cập nhật đánh giá cho những kiến  nghị cùng nội dung


                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Insert đánh giá trả lời kiến nghị ");
                return View("../Home/Error_Exception");
            }

        }
        public Boolean Update_Danhgia_all_kiennghi_cungnoidung(KN_KIENNGHI kiennghi, KN_GIAMSAT giamsat)
        {
            bool result = true;
            try
            {

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ID_KIENNGHI_PARENT", kiennghi.IKIENNGHI);
                var kiennghi_trung = _kiennghi.HienThiDanhSachKienNghi(dic);
                if (kiennghi_trung.Count() > 0)
                {
                    UserInfor u_info = GetUserInfor();
                    foreach (var k in kiennghi_trung)
                    {
                        KN_KIENNGHI kn = k;
                        Dictionary<string, object> dic_ = new Dictionary<string, object>();
                        dic_.Add("IKIENNGHI", (int)k.IKIENNGHI);
                        var all_giamsat = _kiennghi.GetAll_Giamsat_TraLoi_byParam(dic_);
                        if (all_giamsat != null)
                        {
                            KN_GIAMSAT g = all_giamsat.FirstOrDefault();
                            g.IDANHGIA = Convert.ToInt32(giamsat.IDANHGIA);
                            g.IDONGKIENNGHI = Convert.ToInt32(giamsat.IDONGKIENNGHI);
                            g.IDUNGTIENDO = Convert.ToInt32(giamsat.IDUNGTIENDO);
                            g.IPHANLOAI = Convert.ToInt32(giamsat.IPHANLOAI);
                            _kiennghi.Update_Giamsat_traloi(g);
                            //if (giamsat.IDONGKIENNGHI == 0)// chuyển theo dõi kỳ sau
                            //{

                            //    kn.ITINHTRANG = (decimal)TrangThaiKienNghi.TheoDoiKyHopSau;
                            //    _kiennghi.UpdateThongTinKienNghi(kn);
                            //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)kn.IKIENNGHI, "Đánh giá trả lời kiến nghị: Chuyển theo dõi kỳ họp sau.");
                            //}
                            //else
                            //{
                            //    if (kn.ITINHTRANG == (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                            //    {
                            //        kn.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi;
                            //        _kiennghi.UpdateThongTinKienNghi(kn);
                            //    }
                            //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)k.IKIENNGHI, "Đánh giá trả lời kiến nghị: Đóng kiến nghị");
                            //}
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Cập nhật đánh giá các kiến nghị cùng nội dung");
                result = false;
            }
            return result;
        }
        public Boolean Danhgia_all_kiennghi_cungnoidung(KN_KIENNGHI kiennghi, KN_GIAMSAT giamsat)
        {
            bool result = true;
            try
            {

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ID_KIENNGHI_PARENT", kiennghi.IKIENNGHI);
                var kiennghi_trung = _kiennghi.HienThiDanhSachKienNghi(dic);
                if (kiennghi_trung.Count() > 0)
                {
                    UserInfor u_info = GetUserInfor();
                    foreach (var k in kiennghi_trung)
                    {

                        KN_GIAMSAT g = new KN_GIAMSAT();
                        g.IDANHGIA = Convert.ToInt32(giamsat.IDANHGIA);
                        g.IDONGKIENNGHI = Convert.ToInt32(giamsat.IDONGKIENNGHI);
                        g.IDUNGTIENDO = Convert.ToInt32(giamsat.IDUNGTIENDO);
                        g.IPHANLOAI = Convert.ToInt32(giamsat.IPHANLOAI);
                        g.IKIENNGHI = (int)k.IKIENNGHI;
                        g.IUSER = u_info.tk_action.iUser;
                        g.DDATE = DateTime.Now;
                        _kiennghi.Insert_Giamsat_traloi(g);
                        //if (giamsat.IDONGKIENNGHI == 0)// chuyển theo dõi kỳ sau
                        //{
                        //    KN_KIENNGHI kn = k;
                        //    kn.ITINHTRANG = (decimal)TrangThaiKienNghi.TheoDoiKyHopSau;
                        //    _kiennghi.UpdateThongTinKienNghi(kn);
                        //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)kn.IKIENNGHI, "Đánh giá trả lời kiến nghị: Chuyển theo dõi kỳ họp sau.");
                        //}
                        //else
                        //{
                        //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)k.IKIENNGHI, "Đánh giá trả lời kiến nghị: Đóng kiến nghị");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Đánh giá các kiến nghị cùng nội dung");
                result = false;
            }
            return result;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_danhgia_update(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_danhgia_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                UserInfor u_info = GetUserInfor();
                KN_GIAMSAT giamsat = _kiennghi.Get_Giamsat_TraLoi(id);

                giamsat.IDONGKIENNGHI = Convert.ToInt32(fc["iDongKienNghi"]);
                //giamsat.IDANHGIA = Convert.ToInt32(fc["iDanhGia"]);
                giamsat.IPHANLOAI = Convert.ToInt32(fc["iPhanLoai1"]);
                giamsat.IDUNGTIENDO = Convert.ToInt32(fc["iDungTienDo"]);

                _kiennghi.Update_Giamsat_traloi(giamsat);

                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)giamsat.IKIENNGHI);

                //if (giamsat.IDONGKIENNGHI == 0)// chuyển theo dõi kỳ sau
                //{   
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.TheoDoiKyHopSau;
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)giamsat.IKIENNGHI, "Sửa đánh giá trả lời kiến nghị: Chuyển theo dõi kỳ họp sau.");
                //}
                //else
                //{
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi; //đóng kiến nghị
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)giamsat.IKIENNGHI, "Sửa đánh giá trả lời kiến nghị: Đóng kiến nghị");
                //}
                //Update_Danhgia_all_kiennghi_cungnoidung(kiennghi, giamsat);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Update đánh giá trả lời kiến nghị ");
                return View("../Home/Error_Exception");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_traloi_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {

                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_traloi_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = _kiennghi.Get_TraLoi_KienNghi(id);
                int iPhanLoai = Convert.ToInt32(fc["iPhanLoai"]);
                if (u_info.tk_action.is_dbqh && Request["cCoQuanTraLoi"] != null)//kiến nghị thuộc thẩm quyền địa phương
                {
                    t.CCOQUANTRALOI = func.RemoveTagInput(fc["cCoQuanTraLoi"]);
                }
                t.GHICHU_KQ = func.RemoveTagInput(fc["cGhiChu"]);
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                t.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                t.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                if (iPhanLoai != (int)PhanLoai_TraLoiKienNghi.GIAITRINH_CUNGCAPTHONGTIN && fc["iPhanLoai1"] != null)
                {
                    iPhanLoai = Convert.ToInt32(fc["iPhanLoai1"]);
                }
                if (fc["DNGAY_DUKIEN"] != null)
                {
                    if (fc["DNGAY_DUKIEN"].ToString() != "")
                    {
                        t.DNGAY_DUKIEN = Convert.ToDateTime(func.ConvertDateToSql(fc["DNGAY_DUKIEN"]));
                    }
                }
                t.IPHANLOAI = iPhanLoai;
                t.ICOQUANTRALOI = Convert.ToInt32(fc["iDonViTraLoi"]);
                //t.CLOTRINH = func.RemoveTagInput(fc["cLoTrinh"]);
                _kiennghi.UpdateTraLoi_KienNghi(t);
                List<FILE_UPLOAD> file_upload = new List<FILE_UPLOAD>();
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = (int)t.ITRALOI;
                        kntc.Upload_file(f);
                        file_upload.Add(f);
                    }
                }
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi((int)t.IKIENNGHI);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    Update_Traloi_all_kiennghi_cungnoidung(t, file_upload);
                //}
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Sửa trả lời kiến nghị");

                KN_TRALOI_PHANLOAI TlPhanLoai = _kiennghi.Get_KN_TRALOI_PHANLOAI(Convert.ToInt32(fc["iPhanLoai"]));
                if (TlPhanLoai != null)
                {
                    int idKn = Convert.ToInt32(t.IKIENNGHI);
                    KN_KIENNGHI dataKN = _kiennghi.HienThiThongTinKienNghi(idKn);
                    dataKN.ITINHTRANG = TlPhanLoai.ITINHTRANG;
                    _kiennghi.UpdateThongTinKienNghi(dataKN);

                }

                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Update trả lời kiến nghị ");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Kiennghi_traloi_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI_TRALOI traloi = _kiennghi.Get_TraLoi_KienNghi(id);
                int iKienNghi = (int)traloi.IKIENNGHI;
                _kiennghi.DeleteTraLoi_KienNghi(traloi);
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(iKienNghi);
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet;
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //    _kiennghi.Tracking_KN(GetUserInfor().tk_action.iUser, id, "Xóa trả lời kiến nghị");
                //    Delete_Traloi_Kiennghi_cungnoidung(kiennghi);
                //}else
                //{
                //    _kiennghi.Tracking_KN(GetUserInfor().tk_action.iUser, id, "Xóa trả lời kiến nghị");
                //}

                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Xóa trả lời kiến nghị ");
                throw;
            }

        }

        public Boolean Delete_Traloi_Kiennghi_cungnoidung(KN_KIENNGHI kn)
        {
            bool result = true;
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ID_KIENNGHI_PARENT", kn.IKIENNGHI);
                var kiennghi = _kiennghi.HienThiDanhSachKienNghi(dic);
                if (kiennghi.Count() > 0)
                {
                    _kiennghi.Update_All_Tinhtrang_Kiennghi_CungNoiDung((int)kn.ITINHTRANG, (int)kn.IKIENNGHI);
                    foreach (var k in kiennghi)
                    {
                        _kiennghi.Delete_Traloi_byIDKIENNGHI((int)k.IKIENNGHI);
                        _kiennghi.Tracking_KN(GetUserInfor().tk_action.iUser, (int)k.IKIENNGHI, "Xóa trả lời kiến nghị");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Hủy trả lời kiến nghị");
                result = false;
            }
            return result;
        }
        public ActionResult Ajax_Kiennghi_danhgia_chuyenkysau_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_GIAMSAT traloi = _kiennghi.Get_Giamsat_TraLoi(id);
                int iKienNghi = (int)traloi.IKIENNGHI;
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(iKienNghi);

                _kiennghi.DeleteGiamSat_KienNghi(traloi);
                _kiennghi.Tracking_KN(GetUserInfor().tk_action.iUser, iKienNghi, "Xóa đánh giá trả lời kiến nghị");
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa đánh giá trả lời kiến nghị ");
                throw;
            }


        }
        public ActionResult Ajax_Kiennghi_danhgia_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_GIAMSAT traloi = _kiennghi.Get_Giamsat_TraLoi(id);
                int iKienNghi = (int)traloi.IKIENNGHI;
                //KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(iKienNghi);
                //if (kiennghi.ITINHTRANG == (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)//chuyển kỳ sau
                //{
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi;//đã trả lời
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //}
                _kiennghi.DeleteGiamSat_KienNghi(traloi);
                _kiennghi.Tracking_KN(GetUserInfor().tk_action.iUser, iKienNghi, "Xóa đánh giá trả lời kiến nghị");
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa đánh giá trả lời kiến nghị ");
                throw;
            }


        }
        public Boolean Delete_All_DanhGia_Traloi_kiennghi_cungnoidung(KN_KIENNGHI kiennghi)
        {
            bool result = true;
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ID_KIENNGHI_PARENT", kiennghi.IKIENNGHI);
                var kiennghi_trung = _kiennghi.HienThiDanhSachKienNghi(dic);

                if (kiennghi_trung.Count() > 0)
                {
                    UserInfor u_info = GetUserInfor();
                    foreach (var k in kiennghi_trung)
                    {
                        Dictionary<string, object> dic_ = new Dictionary<string, object>();
                        dic_.Add("IKIENNGHI", k.IKIENNGHI);
                        var giamsat = _kiennghi.GetAll_Giamsat_TraLoi_byParam(dic_);
                        if (giamsat.Count() > 0)
                        {
                            //xóa giám sát kiến nghị
                            KN_GIAMSAT g = giamsat.FirstOrDefault();
                            _kiennghi.DeleteGiamSat_KienNghi(g);
                        }
                        //KN_KIENNGHI kn = k;
                        //if (kn.ITINHTRANG == (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)//chuyển kỳ sau
                        //{
                        //    kn.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi;//đã trả lời
                        //    _kiennghi.UpdateThongTinKienNghi(kn);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa đánh  giá kiến nghị cùng nội dung");
                result = false;
            }
            return result;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kiennghi_traloi_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Kiennghi_traloi", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }
                }
                int iTinhTrang = Convert.ToInt32(fc["iTinhTrang"]);
                int iPhanLoai = Convert.ToInt32(fc["iPhanLoai"]);
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI_TRALOI t = new KN_KIENNGHI_TRALOI();
                t.IKIENNGHI = id;
                t.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                t.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                t.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                t.CTRALOI = func.RemoveTagInput(fc["cNoiDung"]);
                t.GHICHU_KQ = func.RemoveTagInput(fc["cGhiChu"]);
                if (u_info.tk_action.is_dbqh && Request["cCoQuanTraLoi"] != null)//kiến nghị thuộc thẩm quyền địa phương
                {
                    t.CCOQUANTRALOI = func.RemoveTagInput(fc["cCoQuanTraLoi"]);
                }
                if (iPhanLoai != (int)PhanLoai_TraLoiKienNghi.GIAITRINH_CUNGCAPTHONGTIN && fc["iPhanLoai1"] != null)
                {
                    iPhanLoai = Convert.ToInt32(fc["iPhanLoai1"]);

                }
                decimal iTinhTrangKienNghi = 0;
                if (iPhanLoai == Convert.ToDecimal(TrangThai_TraLoiKienNghi.Xemxetgiaiquyet))
                {
                    iTinhTrangKienNghi = 4;
                }
                else if (iPhanLoai == Convert.ToDecimal(TrangThai_TraLoiKienNghi.Dagiaiquetxong))
                {
                    iTinhTrangKienNghi = 5;
                }
                else if (iPhanLoai == Convert.ToDecimal(TrangThai_TraLoiKienNghi.Giaitrinh))
                {
                    iTinhTrangKienNghi = 6;
                }
                else if (iPhanLoai == Convert.ToDecimal(TrangThai_TraLoiKienNghi.Chuagiaiquyet))
                {
                    iTinhTrangKienNghi = 8;
                }
                if (fc["DNGAY_DUKIEN"] != null)
                {
                    if (fc["DNGAY_DUKIEN"].ToString() != "")
                    {
                        t.DNGAY_DUKIEN = Convert.ToDateTime(func.ConvertDateToSql(fc["DNGAY_DUKIEN"]));
                    }
                }
                t.ICOQUANTRALOI = Convert.ToInt32(fc["iDonViTraLoi"]);
                //t.CLOTRINH = func.RemoveTagInput(fc["cLoTrinh"]);
                t.IUSER = u_info.tk_action.iUser;
                t.IPHANLOAI = iPhanLoai;
                t.DDATE = DateTime.Now;
                t.ITINHTRANG = iTinhTrang;//1:trả lời; 2: trả lại
                _kiennghi.InsertTraLoi_KienNghi(t);

                KN_TRALOI_PHANLOAI TlPhanLoai = _kiennghi.Get_KN_TRALOI_PHANLOAI(Convert.ToInt32(fc["iPhanLoai"]));
                if (TlPhanLoai != null)
                {
                    int idKn = Convert.ToInt32(t.IKIENNGHI);
                    KN_KIENNGHI dataKN = _kiennghi.HienThiThongTinKienNghi(idKn);
                    dataKN.ITINHTRANG = TlPhanLoai.ITINHTRANG;
                    _kiennghi.UpdateThongTinKienNghi(dataKN);

                }


                int itraloi = (int)t.ITRALOI;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_traloi";
                        f.CFILE = UploadFile(file);
                        f.ID = itraloi;
                        kntc.Upload_file(f);
                    }
                }
                //if (kiennghi.ITINHTRANG != (decimal)TrangThaiKienNghi.TheoDoiKyHopSau)
                //{
                //    kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi;
                //    _kiennghi.UpdateThongTinKienNghi(kiennghi);
                //    _kiennghi.Tracking_KN(u_info.tk_action.iUser, id, "Trả lời kiến nghị");
                //    //cập nhật cho các kiến nghị cùng nội dung
                //    Traloi_all_kiennghi_cungnoidung(kiennghi, t);
                //}                
                _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)t.IKIENNGHI, "Cập nhật kết quả trả lời: " + t.CTRALOI);
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Insert trả lời kiến nghị ");
                return View("../Home/Error_Exception");
            }
        }
        public Boolean UpdateTinhTrang_TraLoiTongHop_KienNghi(int iTonghop)
        {
            bool result = true;
            //
            return result;
        }
        public string Get_Option_KN_Traloi_Phanloai(int id = 0)
        {
            string str = "";
            var phanloai = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
            var phanloai0 = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var p in phanloai0)
            {
                var phanloai1 = phanloai.Where(x => x.IPARENT == p.IPHANLOAI).ToList();
                if (phanloai1.Count() > 0)
                {
                    str += "<optgroup label='" + p.CTEN + "'>";
                    foreach (var p1 in phanloai1)
                    {
                        string select = ""; if (p1.IPHANLOAI == id) { select = " selected "; }
                        str += "<option " + select + " value='" + p1.IPHANLOAI + "'>" + p1.CTEN + "</option>";
                    }
                    str += "</optgroup>";
                }
                else
                {
                    string select = ""; if (p.IPHANLOAI == id) { select = " selected "; }
                    str += "<option " + select + " value='" + p.IPHANLOAI + "'>" + p.CTEN + "</option>";
                }
            }
            ///str = kn.Option_KN_Traloi_Phanloai(phanloai,0,0,id);
            return str;
        }

        public Boolean Traloi_all_kiennghi_cungnoidung(KN_KIENNGHI kiennghi, KN_KIENNGHI_TRALOI traloi)
        {
            bool result = true;
            try
            {

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ID_KIENNGHI_PARENT", kiennghi.IKIENNGHI);
                var kiennghi_trung = _kiennghi.HienThiDanhSachKienNghi(dic);
                var file = _kiennghi.GetAll_FileUpload((int)traloi.ITRALOI, "kn_traloi");
                if (kiennghi_trung.Count() > 0)
                {
                    string tracking_content = "";
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.DaCoTraLoi)
                    {
                        tracking_content += "Cập nhật kết quả trả lời: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.TraLaiKienNghi)
                    {
                        tracking_content += "Cập nhật lý do trả lại kiến nghị: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuyenGiaiQuyetKyHopSau)
                    {
                        tracking_content += "Cập nhật lý do chuyển giải quyết kỳ họp sau: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.CoLoTrinhGiaiQuyet)
                    {
                        tracking_content += "Cập nhật lộ trình giải quyết: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoLoTrinhGiaiQuyet)
                    {
                        tracking_content += "Cập nhật lý do chưa có lộ trình giải quyết: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaTheGiaiQuyetNgay)
                    {
                        tracking_content += "Cập nhật lý do chưa thể giải quyết ngay: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoNguonLucGiaiQuyet)
                    {
                        tracking_content += "Cập nhật lý do chưa có nguồn lực giải quyết: " + traloi.CTRALOI;
                    }
                    _kiennghi.Update_All_Tinhtrang_Kiennghi_CungNoiDung((int)kiennghi.ITINHTRANG, (int)kiennghi.IKIENNGHI);
                    foreach (var k in kiennghi_trung)
                    {
                        KN_KIENNGHI_TRALOI t = new KN_KIENNGHI_TRALOI();
                        t.IKIENNGHI = (int)k.IKIENNGHI;
                        t.ITINHTRANG = (int)traloi.ITINHTRANG;
                        t.IUSER = (int)traloi.IUSER;
                        t.DDATE = traloi.DDATE;
                        t.IPHANLOAI = (int)traloi.IPHANLOAI;
                        if (traloi.DNGAY_DUKIEN != null)
                        {
                            t.DNGAY_DUKIEN = traloi.DNGAY_DUKIEN;
                        }
                        t.CTRALOI = traloi.CTRALOI;
                        _kiennghi.InsertTraLoi_KienNghi(t);

                        //insert file_upload cho mỗi trả  lời cùng nội dung
                        //if (file.Count() > 0)
                        //{
                        //    foreach(var f in file)
                        //    {
                        //        FILE_UPLOAD f_ = new FILE_UPLOAD();
                        //        f_.CTYPE = "kn_traloi";
                        //        f_.CFILE = f.CFILE;
                        //        f_.ID = (int)t.ITRALOI;
                        //        kntc.Upload_file(f_);
                        //    }
                        //}
                        _kiennghi.Tracking_KN((int)traloi.IUSER, (int)k.IKIENNGHI, tracking_content);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public Boolean Update_Traloi_all_kiennghi_cungnoidung(KN_KIENNGHI_TRALOI traloi, List<FILE_UPLOAD> file_upload)
        {
            bool result = true;
            try
            {

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ID_KIENNGHI_PARENT", traloi.IKIENNGHI);
                var kiennghi_trung = _kiennghi.HienThiDanhSachKienNghi(dic);
                ///var file = _kiennghi.GetAll_FileUpload((int)traloi.ITRALOI, "kn_traloi");
                if (kiennghi_trung.Count() > 0)
                {
                    string tracking_content = "";
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.DaCoTraLoi)
                    {
                        tracking_content += "Cập nhật lại kết quả trả lời: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.TraLaiKienNghi)
                    {
                        tracking_content += "Cập nhật lại lý do trả lại kiến nghị: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuyenGiaiQuyetKyHopSau)
                    {
                        tracking_content += "Cập nhật lại lý do chuyển giải quyết kỳ họp sau: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.CoLoTrinhGiaiQuyet)
                    {
                        tracking_content += "Cập nhật lại lộ trình giải quyết: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoLoTrinhGiaiQuyet)
                    {
                        tracking_content += "Cập nhật lại lý do chưa có lộ trình giải quyết: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaTheGiaiQuyetNgay)
                    {
                        tracking_content += "Cập nhật lại lý do chưa thể giải quyết ngay: " + traloi.CTRALOI;
                    }
                    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoNguonLucGiaiQuyet)
                    {
                        tracking_content += "Cập nhật lý do chưa có nguồn lực giải quyết: " + traloi.CTRALOI;
                    }
                    foreach (var k in kiennghi_trung)
                    {
                        Dictionary<string, object> _dic = new Dictionary<string, object>();
                        _dic.Add("IKIENNGHI", (int)k.IKIENNGHI);
                        var traloi_ = _kiennghi.GetAll_TraLoi_KienNghi_ByParamt(_dic);
                        if (traloi_.Count() > 0)
                        {
                            KN_KIENNGHI_TRALOI t = traloi_.FirstOrDefault();
                            if (traloi.DNGAY_DUKIEN != null)
                            {
                                t.DNGAY_DUKIEN = traloi.DNGAY_DUKIEN;
                            }
                            t.CTRALOI = traloi.CTRALOI;
                            t.IPHANLOAI = (int)traloi.IPHANLOAI;
                            _kiennghi.UpdateTraLoi_KienNghi(t);
                            //Insert file_upload cho mỗi kiến nghị cùng nội dung
                            //if (file_upload.Count() > 0)
                            //{
                            //    foreach (var f in file_upload)
                            //    {
                            //        FILE_UPLOAD f_ = new FILE_UPLOAD();
                            //        f_.CTYPE = "kn_traloi";
                            //        f_.CFILE = f.CFILE;
                            //        f_.ID = (int)t.ITRALOI;
                            //        kntc.Upload_file(f_);
                            //    }
                            //}
                            _kiennghi.Tracking_KN((int)traloi.IUSER, (int)k.IKIENNGHI, tracking_content);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public ActionResult Ajax_Delele_file(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string id_decrypt = HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value);
                int id = Convert.ToInt32(id_decrypt);
                FILE_UPLOAD file = _kiennghi.Get_FILE_UPLOAD(id);
                if (file != null)
                {
                    //if (file.CTYPE == "kn_traloi")
                    //{
                    //    int iTraLoi = (int)file.ID;
                    //    KN_KIENNGHI_TRALOI traloi = _kiennghi.Get_TraLoi_KienNghi(iTraLoi);
                    //    if (traloi != null)
                    //    {

                    //        //kiểm tra kiến nghị cùng nội dung
                    //        int iKienNghi = (int)traloi.IKIENNGHI;
                    //        Dictionary<string, object> dic = new Dictionary<string, object>();
                    //        dic.Add("ID_KIENNGHI_PARENT", iKienNghi);
                    //        var kiennghi = _kiennghi.HienThiDanhSachKienNghi(dic);
                    //        if (kiennghi.Count() > 0)
                    //        {
                    //            string file_name = file.CFILE;
                    //            foreach (var k in kiennghi)
                    //            {


                    //                //xóa file upload kn_traloi
                    //                //_kiennghi.Delete_Traloi_byIDKIENNGHI((int)k.IKIENNGHI);
                    //            }
                    //        }
                    //    }                        
                    //}
                    kntc.Delete_File_upload(id);
                }
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                //Handle Exception;
                log.Log_Error(ex, "Xóa file");
                return null;
            }
        }
        public ActionResult Ajax_Tonghop_traloi_del(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_VANBAN vanban = _kiennghi.Get_Vanban(id);

                int iTongHop = (int)vanban.ITONGHOP;
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(iTongHop);
                tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;
                _kiennghi.UpdateTongHop(tonghop);
                _kiennghi.Delete_vanban(vanban);//hủy văn bản trả lời
                _kiennghi.Tracking_Tonghop(u_info.tk_action.iUser, iTongHop, "Hủy trả lời Tập hợp kiến nghị");
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xóa trả lời Tập hợp");
                throw;
            }
        }
        public ActionResult Ajax_Tonghop_traloi_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                //Dictionary<string, object> _vanban = new Dictionary<string, object>();
                //_vanban.Add("IVANBAN", id);_vanban.Add("CLOAI", "tonghop_traloi");
                KN_VANBAN vanban = _kiennghi.Get_Vanban(id);
                ViewData["id"] = fc["id"];
                ViewData["vanban"] = vanban;
                ViewData["file"] = kn.File_Edit(id, "kn_vanban", Request.Cookies["url_key"].Value);
                SetTokenAction("Tonghop_traloi", id);
                return PartialView("../Ajax/Kiennghi/Tonghop_traloi_edit");

            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form sửa trả lời Tập hợp");
                throw;
            }

        }
        public ActionResult Ajax_Tonghop_traloi(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                ViewData["id"] = fc["id"];
                SetTokenAction("Tonghop_traloi", id);
                return PartialView("../Ajax/Kiennghi/Tonghop_traloi");

            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form trả lời Tập hợp ");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Tonghop_traloi_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Tonghop_traloi", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }

                }
                UserInfor u_info = GetUserInfor();
                KN_VANBAN v = _kiennghi.Get_Vanban(id);
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);

                v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);

                _kiennghi.Update_Vanban(v);

                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = id;
                        kntc.Upload_file(f);
                    }
                }
                _kiennghi.Tracking_Tonghop(u_info.tk_action.iUser, id, "Sửa trả lời Tập hợp kiến nghị");
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Update trả lời Tập hợp");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Tonghop_traloi_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Tonghop_traloi", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }

                }
                UserInfor u_info = GetUserInfor();
                KN_VANBAN v = new KN_VANBAN();
                v.ITONGHOP = id;
                v.IKIENNGHI = 0;
                v.ICOQUANBANHANH = (int)u_info.user_login.IDONVI;
                v.ICOQUANNHAN = 0;
                v.CSOVANBAN = func.RemoveTagInput(fc["cSoVanBan"]);
                v.IUSER = u_info.tk_action.iUser;
                v.DDATE = DateTime.Now;
                v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                v.CLOAI = "tonghop_traloi";
                _kiennghi.Insert_Vanban(v);
                int iVanBan = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanBan;
                        kntc.Upload_file(f);
                    }
                }
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                //if ((int)tonghop.ITINHTRANG == (decimal)TrangThai_TongHop.DangXuLy)//Tập hợp ban dân nguyện chuyển đến
                //{
                //    Dictionary<string, object> condition = new Dictionary<string, object>();
                //    condition.Add("ITONGHOP_BDN", id);
                //    var kiennghi_update = _kiennghi.HienThiDanhSachKienNghi(condition);
                //    foreach (var k in kiennghi_update)
                //    {
                //        k.ITINHTRANG = (decimal)TrangThaiKienNghi.TongHopDaCoTraLoi;
                //        _kiennghi.UpdateThongTinKienNghi(k);
                //    }
                //    //condition.Add("ITINHTRANG", 5);
                //    //_kiennghi.KN_Update_All(condition, "ITONGHOP_BDN", id);

                //}
                //else//Tập hợp chuyển địa phương
                //{
                //    Dictionary<string, object> condition = new Dictionary<string, object>();
                //    condition.Add("ITONGHOP", id);
                //    var kiennghi_update = _kiennghi.HienThiDanhSachKienNghi(condition);
                //    foreach (var k in kiennghi_update)
                //    {
                //        if (k.ITINHTRANG == (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet)
                //        {
                //            k.ITINHTRANG = (decimal)TrangThaiKienNghi.TongHopDaCoTraLoi;
                //            _kiennghi.UpdateThongTinKienNghi(k);
                //        }

                //    }
                //}
                tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DaTraLoi;
                //tonghop.ITHAMQUYENDONVI = (int)v.ICOQUANNHAN;
                _kiennghi.UpdateTongHop(tonghop);
                _kiennghi.Tracking_Tonghop(u_info.tk_action.iUser, id, "Trả lời Tập hợp kiến nghị");
                Response.Redirect("/Kiennghi/Traloi/#success");

                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Insert trả lời Tập hợp kiến nghị ");
                return View("../Home/Error_Exception");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Chuyen_donvi_xuly_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Chuyen_Donvi_xuly", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }

                }
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                UserInfor u_info = GetUserInfor();
                DateTime ngayquydinh = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayPhanHoi"]));
                KN_VANBAN v = new KN_VANBAN();
                if (fc["dNgayBanHanh"] != "")
                {
                    v.DNGAYDUKIENHOANTHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                    v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                }
                v.ITONGHOP = id;
                v.IKIENNGHI = 0;
                v.ICOQUANBANHANH = (int)u_info.user_login.IDONVI;
                v.ICOQUANNHAN = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                v.DNGAYBAOCAO = ngayquydinh;
                v.CSOVANBAN = fc["cSoVanBan"];
                v.IUSER = u_info.tk_action.iUser;
                v.DDATE = DateTime.Now;
                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CNOIDUNG = "";
                v.CLOAI = "tonghop_chuyendonvi_xuly";
                _kiennghi.Insert_Vanban(v);
                int iVanBan = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanBan;
                        kntc.Upload_file(f);
                    }
                }
                tonghop.ITHAMQUYENDONVI = (int)v.ICOQUANNHAN;
                if (tonghop.IDONVITONGHOP == ID_Ban_DanNguyen && (int)v.ICOQUANNHAN == ID_Ban_DanNguyen)
                {
                    tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;//ban dân nguyện Tập hợp và chuyển đến BDN
                }
                else
                {
                    //chuyenr đơn vị khác
                    tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen;
                }
                _kiennghi.UpdateTongHop(tonghop);
                _kiennghi.Tracking_Tonghop(u_info.tk_action.iUser, id, "Chuyển Tập hợp đến cơ quan có thẩm quyền xử lý");
                Dictionary<string, object> condition = new Dictionary<string, object>();
                condition.Add("ITONGHOP", id);
                var kiennghi_update = _kiennghi.HienThiDanhSachKienNghi(condition);
                foreach (var k in kiennghi_update)
                {
                    k.ITINHTRANG = (decimal)TrangThaiKienNghi.ChuaTraLoiDangXuly;
                    k.INGAYQUYDINH = ngayquydinh;
                    DateTime now = DateTime.Now;
                    double khacbiet = (ngayquydinh - now).TotalDays;
                    if (khacbiet < 0)
                        k.ICANHBAO = 2;
                    if (khacbiet >= 0 && khacbiet <= 5)
                        k.ICANHBAO = 1;
                    if (khacbiet > 5)
                        k.ICANHBAO = 0;
                    //k.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonVi"]);
                    _kiennghi.UpdateThongTinKienNghi(k);
                }
                //TaoKienNghi_Parent(tonghop);
                Response.Redirect("/Kiennghi/Chuatraloi/");

                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Chuyển Tập hợp đến cơ quan thẩm quyền ");
                return View("../Home/Error_Exception");
            }

        }
        public Boolean TaoKienNghi_Parent(KN_TONGHOP tonghop)
        {
            bool result = true;
            try
            {
                Dictionary<string, object> _dic = new Dictionary<string, object>();
                _dic.Add("ITONGHOP_BDN", tonghop.ITONGHOP);
                _dic.Add("ID_KIENNGHI_PARENT", 0);
                var kiennghi = _kiennghi.HienThiDanhSachKienNghi(_dic);
                string noidung = "";
                foreach (var k in kiennghi)
                {
                    noidung += k.CNOIDUNG + " \n ";
                }
                KN_KIENNGHI kn = new KN_KIENNGHI();
                kn.IKYHOP = tonghop.IKYHOP;
                kn.CMAKIENNGHI = "";
                kn.CNOIDUNG = noidung;
                kn.CTUKHOA = "";
                kn.DDATE = DateTime.Now;
                kn.ICHUONGTRINH = 0;
                kn.IDONVITIEPNHAN = tonghop.IDONVITONGHOP;
                kn.IKIEMTRATRUNG = 0;
                kn.IKIENNGHI_TRUNG = 0;
                kn.ILINHVUC = tonghop.ILINHVUC;
                kn.ITHAMQUYENDONVI = tonghop.ITHAMQUYENDONVI;
                kn.ITINHTRANG = tonghop.ITINHTRANG;
                kn.ITONGHOP = 0;
                kn.IPARENT = 1;
                kn.ID_GOP = 0;
                kn.IDIAPHUONG0 = 0;
                kn.IDIAPHUONG1 = 0;
                kn.ITONGHOP_BDN = tonghop.ITONGHOP;
                kn.ID_KIENNGHI_PARENT = 0;
                kn.ITRUOCKYHOP = tonghop.ITRUOCKYHOP;
                kn.IUSER = GetUserInfor().tk_action.iUser;
                _kiennghi.InsertKienNghi(kn);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Không tạo được kiến nghị cha");
                result = false;
                throw;

            }
            return result;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Chuyen_Xuly_tonghop_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("kn_Chuyen_Xuly_tonghop", id)) { Response.Redirect("/Home/Error/"); return null; }
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!CheckFile_Upload(file))
                        {
                            Response.Redirect("/Home/Error/?type=type"); return null;
                        }
                    }

                }
                UserInfor u_info = GetUserInfor();
                DateTime ngaybanhanh = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                DateTime ngayhoanthanh_dukien = kn.NgayKetThuc_DuKien(ngaybanhanh);
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                KN_VANBAN v = new KN_VANBAN();
                v.ITONGHOP = id;
                v.IKIENNGHI = 0;
                v.ICOQUANBANHANH = (int)u_info.user_login.IDONVI;
                v.ICOQUANNHAN = Convert.ToInt32(fc["iDonVi"]);
                v.IUSER = u_info.tk_action.iUser;
                v.DDATE = DateTime.Now;
                v.DNGAYBANHANH = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayBanHanh"]));
                v.CNGUOIKY = func.RemoveTagInput(fc["cNguoiKy"]);
                v.CNOIDUNG = "";
                v.DNGAYDUKIENHOANTHANH = ngayhoanthanh_dukien;
                v.CLOAI = "tonghop_chuyenxuly";
                _kiennghi.Insert_Vanban(v);
                int iVanBan = (int)v.IVANBAN;
                for (int i = 1; i < 4; i++)
                {
                    file = Request.Files["file_upload" + i];
                    if (file != null && file.ContentLength > 0)
                    {
                        FILE_UPLOAD f = new FILE_UPLOAD();
                        f.CTYPE = "kn_vanban";
                        f.CFILE = UploadFile(file);
                        f.ID = iVanBan;
                        kntc.Upload_file(f);
                    }
                }
                //KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                if (tonghop.ITHAMQUYENDONVI != u_info.user_login.IDONVI)
                {// chuyển cho BDN
                    tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DaChuyenBanDanNguyen;
                    _kiennghi.UpdateTongHop(tonghop);
                    Dictionary<string, object> condition = new Dictionary<string, object>();
                    condition.Add("ITONGHOP", id);
                    var kiennghi_update = _kiennghi.HienThiDanhSachKienNghi(condition);
                    foreach (var k in kiennghi_update)
                    {
                        k.ITINHTRANG = (decimal)TrangThaiKienNghi.Choxuly;
                        //k.ITHAMQUYENDONVI = ID_Ban_DanNguyen;
                        _kiennghi.UpdateThongTinKienNghi(k);
                    }

                    //_kiennghi.Tracking_Tonghop(u_info.tk_action.iUser, id, "Chuyển Tập hợp đến Ban Dân nguyện");
                    //_kiennghi.UpdateTongHop(tonghop);
                    //Dictionary<string, object> condition = new Dictionary<string, object>();
                    //condition.Add("ITINHTRANG", 2);
                    //_kiennghi.KN_Update_All(condition,"ITONGHOP", id);
                    Response.Redirect("/Kiennghi/Tonghop_chuyendannguyen/");
                }
                else
                {//chuyển địa phương
                    tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;
                    tonghop.ITHAMQUYENDONVI = ID_Ban_DanNguyen_New;
                    _kiennghi.UpdateTongHop(tonghop);
                    Dictionary<string, object> condition = new Dictionary<string, object>();
                    condition.Add("ITONGHOP", id);
                    var kiennghi_update = _kiennghi.HienThiDanhSachKienNghi(condition);
                    foreach (var k in kiennghi_update)
                    {
                        k.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet;
                        k.ITHAMQUYENDONVI = ID_Ban_DanNguyen_New;
                        _kiennghi.UpdateThongTinKienNghi(k);
                    }

                    //Dictionary<string, object> condition = new Dictionary<string, object>();
                    //condition.Add("ITINHTRANG", 1);
                    //_kiennghi.Tracking_Tonghop(u_info.tk_action.iUser, id, "Chuyển địa phương xử lý");
                    //_kiennghi.UpdateTongHop(tonghop);
                    //_kiennghi.KN_Update_All(condition, "ITONGHOP", id);
                    Response.Redirect("/Kiennghi/Tonghop_chuyendiaphuong/#success");
                }
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Chuyển Tập hợp đến địa phương");
                return View("../Home/Error_Exception");
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Tonghop_sua(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("kn_tonghop_edit", id))
                {
                    Response.Redirect("/Home/Error/"); return null;
                }
                //for (int i = 1; i < 4; i++)
                //{
                //    file = Request.Files["file_upload" + i];
                //    if (file != null && file.ContentLength > 0)
                //    {
                //        if (!CheckFile_Upload(file))
                //        {
                //            Response.Redirect("/Home/Error/?type=type"); return null;
                //        }
                //    }

                //}
                UserInfor u_info = GetUserInfor();
                KN_TONGHOP t = _kiennghi.Get_Tonghop(id);
                if (fc["iThamQuyenDonVi"] != null)
                {
                    t.ITHAMQUYENDONVI = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                }
                //t.ITHAMQUYENDONVI = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                //t.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]);
                t.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]);
                //t.CTUKHOA = func.RemoveTagInput(fc["cTuKhoa"]);
                //t.IKYHOP = Convert.ToInt32(fc["iKyHop"]);
                t.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]);
                //if (fc["iTruocKyHop"] != null)
                //{
                //    t.ITRUOCKYHOP = 1;
                //}
                //else
                //{
                //    t.ITRUOCKYHOP = 0;
                //}
                _kiennghi.UpdateTongHop(t);

                //for (int i = 1; i < 4; i++)
                //{
                //    file = Request.Files["file_upload" + i];
                //    if (file != null && file.ContentLength > 0)
                //    {
                //        FILE_UPLOAD f = new FILE_UPLOAD();
                //        f.CTYPE = "kn_tonghop";
                //        f.CFILE = UploadFile(file);
                //        f.ID = id;
                //        kntc.Upload_file(f);
                //    }

                //}
                _kiennghi.Tracking_Tonghop(u_info.tk_action.iUser, id, "Sửa Tập hợp kiến nghị");
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Update Tập hợp kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Ajax_Change_Phanloai_traloi(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                if (id == 0) { return null; }
                string label_traloi = "Nội dung trả lời";
                if (id == (int)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_KHONGLOTRINH)
                {
                    label_traloi = "Lý do không có lộ trình giải quyết";
                }
                if (id == (int)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_CHUACONGUONLUC)
                {
                    label_traloi = "Lý do chưa có nguồn lực giải quyết";
                }
                if (id == (int)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_CHUAGIAIQUYETDUOCNGAY)
                {
                    label_traloi = "Lý do chưa thể giải quyết được ngay";
                }
                ViewData["label_traloi"] = label_traloi;
                ViewData["id"] = id;
                return PartialView("../Ajax/Kiennghi/Change_Phanloai_traloi");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax thay đổi Phân loại kết quả trả lời.");
                return null;
            }

        }
        public ActionResult Ajax_Change_Phanloai_traloi_option(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                if (id != 0)
                {
                    var phanloai0 = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1 && x.IPARENT == id).OrderBy(x => x.IVITRI).ToList();
                    if (phanloai0.Count() > 0)
                    {
                        Response.Write("<select class='input-block-level' id='iPhanLoai1' onchange=\"ChangePhanLoai1(this.value," + (int)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH + ")\" name='iPhanLoai1'><option value='0'>Vui lòng chọn</option>" + kn.OptionPhanLoaiTraLoi_Parent(phanloai0, 0) + "</select>");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax thay đổi Phân loại kết quả trả lời.");
                return null;
            }

        }
        public ActionResult Ajax_Change_Phanloai_danhgia_option(int id)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                if (id != 0)
                {
                    var phanloai0 = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1 && x.IPARENT == id).OrderBy(x => x.IVITRI).ToList();
                    if (phanloai0.Count() > 0)
                    {
                        Response.Write("<select class='input-block-level' name='iPhanLoai1'>" + kn.OptionPhanLoaiTraLoi_Parent(phanloai0, 0) + "</select>");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Ajax thay đổi Phân loại kết quả trả lời.");
                return null;
            }

        }
        public ActionResult Kiennghi_traloi()
        {
            //  BDN ĐBQH CQTW
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                ViewData["tonghop"] = tonghop;
                ViewData["noidung"] = func.TomTatNoiDung(tonghop.CNOIDUNG, "tonghop");
                ViewData["detail"] = kn.Tonghop_Detail(id, Request.Cookies["url_key"].Value);
                ViewData["file"] = kn.File_View(id, "kn_tonghop");
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if (tonghop.IDONVITONGHOP == ID_Ban_DanNguyen)
                {
                    dic.Add("ITONGHOP_BDN", id);
                }
                else
                {
                    dic.Add("ITONGHOP", id);
                }
                var kiennghi = _kiennghi.HienThiDanhSachKienNghi(dic);
                var traloi = _kiennghi.GetAll_TraLoi_KienNghi();
                var giamsat = _kiennghi.GetAll_Giamsat_TraLoi();
                var coquan = _kiennghi.GetAll_CoQuanByParam(null);
                ViewData["list"] = kn.List_KienNghi_ByID_tonghop_chuyenxuly(kiennghi, coquan, traloi, giamsat, u_info.tk_action, Request.Cookies["url_key"].Value, true);
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Danh sách  kiến nghị  kèm trả lời");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tonghop_bandannguyen_dachuyen()
        {
            //  BDN
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }
                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("5,6", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }
                int iDonViXuLy_Parent = 0;
                int iDonViXuLy = 0;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (iDonViXuLy != 0 && coquan.Where(x => x.ICOQUAN == iDonViXuLy).Count() == 0)
                    {
                        iDonViXuLy = 0;
                    }
                }
                else
                {
                    coquan = null;
                }
                ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, iDonViXuLy);
                KN_TONGHOP kn_tonghop = get_Request_Paramt_TongHop_KienNghi();
                kn_tonghop.IDONVITONGHOP = u_info.user_login.IDONVI;
                kn_tonghop.IKYHOP = iKyHop;
                kn_tonghop.ITHAMQUYENDONVI = iDonViXuLy;
                var tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(kn_tonghop, iDonViXuLy_Parent, 3, 4, -1, -1, -1, -1, page, post_per_page);
                //tonghop = tonghop.Where(x => x.ITINHTRANG >= (int)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen).ToList();
                ViewData["list"] = kn.KN_Tonghop_Bandannguyen_dachuyen(tonghop, u_info.tk_action);
                if (tonghop.Count() > 0)
                {
                    ViewData["phantrang"] = "<tr><td colspan='3'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                return View();
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tập hợp kiến nghị Ban Dân nguyện đã chuyển xử lý");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tonghop_TW()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            ThamQuyenDiaPhuong = (int)ThamQuyen_DiaPhuong.Trunguong;
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("5,6", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!base_business.Action_(5, u_info.tk_action))//chọn Tập hợp
                {
                    ViewData["bt-add"] = " style='display:none'";
                }
                int IDONVITONGHOP = 0;
                int iDonViXuLy_Parent = 0;
                int iDonViXuLy = 0;
                int iKyHop = ID_KyHop_HienTai();
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (iDonViXuLy != 0 && coquan.Where(x => x.ICOQUAN == iDonViXuLy).Count() == 0)
                    {
                        iDonViXuLy = 0;
                    }
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, iDonViXuLy);
                }
                else
                {
                    //coquan = null;
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(null, iDonViXuLy);
                }
                string maKienNghi = "";
                if (Request["cMaKienNghi"] != null)
                {
                    maKienNghi = Request["cMaKienNghi"].ToString();
                }
                int iTruocKyHop = -1;
                if (Request["iTruocKyHop"] != null)
                {
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"].ToString());
                }
                DateTime dTuNgay;
                string tungay;
                if (!DateTime.TryParseExact(Request["dBatDau"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dTuNgay))
                {
                    tungay = "";
                }
                else
                {
                    tungay = dTuNgay.ToString("dd-MMM-yyyy");
                }
                DateTime dDenNgay;
                string denngay;
                if (!DateTime.TryParseExact(Request["dKetThuc"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dDenNgay))
                {
                    denngay = "";
                }
                else
                {
                    denngay = dDenNgay.ToString("dd-MMM-yyyy");
                }
                string listKyHop = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                string listLinhVuc = "";
                if (Request["listLinhVuc"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listLinhVuc = Request["listLinhVuc"];
                }
                int iDonViTiepNhan = 0;
                if (Request["iDonViTiepNhan"] != null)
                {
                    iDonViTiepNhan = Convert.ToInt32(Request["iDonViTiepNhan"]);
                }
                string cNoiDungKienNghi = "";
                
                if (Request["cNoiDungKN"] != null)
                {
                    cNoiDungKienNghi = func.RemoveTagInput(Request["cNoiDungKN"].ToString());
                }
                if (Request["q"] != null && Request["q"]!="") { cNoiDungKienNghi = func.RemoveTagInput(Request["q"]); }
                string listHuyen_Xa_ThanhPho = "";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo thành phố
                    listHuyen_Xa_ThanhPho = Request["listHuyen_Xa_ThanhPho"];
                }
                string listNguonKienNghi = "";
                if (Request["listNguonKienNghi"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listNguonKienNghi = Request["listNguonKienNghi"];
                }
                KN_TONGHOP kn_tonghop = get_Request_Paramt_TongHop_KienNghi_BanDanNguyen();
                kn_tonghop.IDONVITONGHOP = 0;
                kn_tonghop.ITRUOCKYHOP = iTruocKyHop;
                //kn_tonghop.IDONVITONGHOP = u_info.user_login.IUSER;
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                // var tonghop = _kiennghi.List_PRC_TONGHOP_KIENNGHI_BDN(kn_tonghop, listKyHop, listHuyen_Xa_ThanhPho, listLinhVuc, iUser, iDonViXuLy_Parent,page,post_per_page);
                var tonghop = _kiennghi.List_PRC_TONGHOP_KIENNGHI_BDN(kn_tonghop, maKienNghi, listKyHop, tungay, denngay, listNguonKienNghi, iDonViTiepNhan, listLinhVuc,
                      cNoiDungKienNghi, listHuyen_Xa_ThanhPho, iUser, iDonViXuLy_Parent, page, post_per_page);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (tonghop.Count() > 0)
                {
                    htmlList = kn.KN_Tonghop_Bandannguyen(tonghop, u_info, (int)ThamQuyen_DiaPhuong.Trunguong);
                    htmlPhanTrang = "<tr><td colspan='5'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";

                }
                else
                {
                    htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }

                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }

                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;

                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult BaoCaoMoiTonghop_bandannguyen(string ext = "pdf")
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

                int iDonViXuLy = 0;
                if (Request["iDonViXuLy"] != null)
                {
                    iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"].ToString());
                }
                string listLinhVuc = "";
                if (Request["listLinhVuc"] != null)
                {
                    listLinhVuc = Request["listLinhVuc"];
                }
            
                int iTenBaoCao = 0;
                if (Request["iTenBaoCao"] != null)
                {
                    iTenBaoCao = Convert.ToInt32(Request["iTenBaoCao"].ToString());
                }
                string listNguonKienNghi = "";
                if (Request["listNguonKienNghi"] != null)
                {
                    listNguonKienNghi = Request["listNguonKienNghi"];
                }
                var templatePath = "";
                if (iTenBaoCao == 4)
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KN_QH_Baocao_ThuocBNTW);
                }
                else if (iTenBaoCao == 3)
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KN_HDND_Baocao_ThuocBNTW);
                }
                else
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KN_QH_HDND_Baocao_ThuocBNTW);
                }
                var iDoiTuong = 0;
                string listKyHop = "";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                string listHuyen_Xa_ThanhPho = "";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo thành phố
                    listHuyen_Xa_ThanhPho = Request["listHuyen_Xa_ThanhPho"];
                }
                string fileName = "";
                if (iDoiTuong == 0)
                {
                    fileName = string.Format("{0}.{1}", "BaoCao_MoiCapNhat_QH", ext);
                }
                else if (iDoiTuong == 1)
                {
                    fileName = string.Format("{0}.{1}", "BaoCao_MoiCapNhat_HDND", ext);
                }
                UserInfor u_info = GetUserInfor();
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> listKienNghiReport = _kienNghiReport.getReportKienNghiTongHop_BanDanNguyen(iDonViXuLy, listKyHop, listNguonKienNghi, listLinhVuc, iTenBaoCao, listHuyen_Xa_ThanhPho, iUser);
                // List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> listKienNghiReport = _kienNghiReport.getReportKienNghiTongHop_BanDanNguyen(iDonViXuLy, listLinhVuc, iTenBaoCao, listKyHop, listHuyen_Xa_ThanhPho);
                ExcelFile xls = ExportReportTonghop_bandannguyen(iTenBaoCao, templatePath, listKienNghiReport);
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }
        }

        public ExcelFile ExportReportTonghop_bandannguyen(int iTenBaoCao, string templatePath, List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> listKienNghiReport)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(templatePath);
            TXlsNamedRange Range;

            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 2, 32, "='Print_Titles'!$8:$9");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
            Result.SetNamedRange(Range);

            FlexCelReport fr = new FlexCelReport();
            if (iTenBaoCao == 2 && listKienNghiReport.Count > 0)
            {

                decimal check = 2;
                foreach (var item in listKienNghiReport)
                {
                    if (item.IDOITUONGGUI != check)
                    {
                        if (item.IDOITUONGGUI == 0)
                        {
                            item.FIRSTTITLE = "I. ĐOÀN ĐẠI BIỂU QUỐC HỘI";
                        }
                        if (item.IDOITUONGGUI == 1)
                        {
                            item.FIRSTTITLE = "II. THƯỜNG TRỰC HỘI ĐỒNG NHÂN DÂN TỈNH";
                        }
                        check = item.IDOITUONGGUI;
                    }
                }
            }
            fr.AddTable<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG>("List", listKienNghiReport);
            fr.SetValue(new
            {

            });
            fr.UseForm(this).Run(Result);

            return Result;
        }

        public ActionResult BaoCao_TapHop_Dacotraloi(string ext = "pdf")
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

                int iTruocKyHop = 0;
                if (Request["iTruocKyHop"] != null)
                {
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"].ToString());
                }
                string listLinhVuc = "";
                if (Request["iLinhVuc"] != null)
                {
                    listLinhVuc = Request["iLinhVuc"];
                }
                int iTenBaoCao = 0;
                if (Request["iTenBaoCao"] != null)
                {
                    iTenBaoCao = Convert.ToInt32(Request["iTenBaoCao"].ToString());
                }
                int iLoaiBaoCao = 0;
                if (Request["iLoaiBaoCao"] != null)
                {
                    iLoaiBaoCao = Convert.ToInt32(Request["iLoaiBaoCao"].ToString());
                }
                var templatePath = "";
                string fileName = "";
                if (iTenBaoCao == 1)
                {
                    if (iLoaiBaoCao == 0)
                    {
                        templatePath = Server.MapPath(ReportConstants.rpt_KN_QH_Baocao_TapHop_DTL);
                        fileName = string.Format("{0}.{1}", "BaoCao_TongHop_DaTraLoi_QH", ext);
                    }
                    else if (iLoaiBaoCao == 1)
                    {
                        templatePath = Server.MapPath(ReportConstants.rpt_KN_HDND_Baocao_TapHop_DTL);
                        fileName = string.Format("{0}.{1}", "BaoCao_TongHop_DaTraLoi_HDND", ext);
                    }
                    else
                    {
                        templatePath = Server.MapPath(ReportConstants.rpt_KN_QH_HDND_Baocao_TapHop_DTL);
                        fileName = string.Format("{0}.{1}", "BaoCao_TongHop_DaTraLoi_QH_HDND", ext);
                    }
                }else if (iTenBaoCao == 2)
                {
                    if (iLoaiBaoCao == 0)
                    {
                        templatePath = Server.MapPath(ReportConstants.rpt_KN_QH_Baocao_TapHop_DTL_2B);
                        fileName = string.Format("{0}.{1}", "BaoCao_TongHop_DaTraLoi_QH", ext);
                    }
                    else if (iLoaiBaoCao == 1)
                    {
                        templatePath = Server.MapPath(ReportConstants.rpt_KN_HDND_Baocao_TapHop_DTL_2B);
                        fileName = string.Format("{0}.{1}", "BaoCao_TongHop_DaTraLoi_HDND", ext);
                    }
                    else
                    {
                        templatePath = Server.MapPath(ReportConstants.rpt_KN_QH_HDND_Baocao_TapHop_DTL_2B);
                        fileName = string.Format("{0}.{1}", "BaoCao_TongHop_DaTraLoi_QH_HDND", ext);
                    }
                }
                else if (iTenBaoCao == 3)
                {
                    if (iLoaiBaoCao == 0)
                    {
                        templatePath = Server.MapPath(ReportConstants.rpt_KN_QH_Baocao_TapHop_DTL_2B);
                        fileName = string.Format("{0}.{1}", "BaoCao_TongHop_DaTraLoi_QH", ext);
                    }
                    else if (iLoaiBaoCao == 1)
                    {
                        templatePath = Server.MapPath(ReportConstants.rpt_KN_HDND_Baocao_TapHop_DTL_2B);
                        fileName = string.Format("{0}.{1}", "BaoCao_TongHop_DaTraLoi_HDND", ext);
                    }
                    else
                    {
                        templatePath = Server.MapPath(ReportConstants.rpt_KN_QH_HDND_Baocao_TapHop_DTL_2B);
                        fileName = string.Format("{0}.{1}", "BaoCao_TongHop_DaTraLoi_QH_HDND", ext);
                    }
                }

                string listKyHop = "";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                string listHuyen_Xa_ThanhPho = "";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo thành phố
                    listHuyen_Xa_ThanhPho = Request["listHuyen_Xa_ThanhPho"];
                }
                UserInfor u_info = GetUserInfor();
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }

                List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> listKienNghiReport = _kienNghiReport.getReportKienNghiTapHop_Dacotraloi(iTruocKyHop , iLoaiBaoCao, listLinhVuc, listKyHop, listHuyen_Xa_ThanhPho, iUser);
                ExcelFile xls = ExportReportTapHop_Dacotraloi(iLoaiBaoCao, templatePath, listKienNghiReport);
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }
        }

        public ExcelFile ExportReportTapHop_Dacotraloi(int iLoaiBaoCao, string templatePath, List<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG> listKienNghiReport)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(templatePath);
            TXlsNamedRange Range;

            FlexCelReport fr = new FlexCelReport();
            if (iLoaiBaoCao == -1 && listKienNghiReport.Count > 0)
            {

                decimal check = 2;
                foreach (var item in listKienNghiReport)
                {
                    if (item.IDOITUONGGUI != check)
                    {
                        if (item.IDOITUONGGUI == 0)
                        {
                            item.FIRSTTITLE = "I. ĐOÀN ĐẠI BIỂU QUỐC HỘI";
                        }
                        if (item.IDOITUONGGUI == 1)
                        {
                            item.FIRSTTITLE = "II. THƯỜNG TRỰC HỘI ĐỒNG NHÂN DÂN TỈNH";
                        }
                        check = item.IDOITUONGGUI;
                    }
                }
            }
            fr.AddTable<KIENNGHI_REPORT_KNCT_THUOC_BONGANHTRUNGUONG>("List", listKienNghiReport);
            fr.SetValue(new
            {

            });
            fr.UseForm(this).Run(Result);

            return Result;
        }

        public ActionResult Tonghop_Huyen()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            ThamQuyenDiaPhuong = (int)ThamQuyen_DiaPhuong.Huyen;
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("5,6", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!base_business.Action_(5, u_info.tk_action))//chọn Tập hợp
                {
                    ViewData["bt-add"] = " style='display:none'";
                }
                int IDONVITONGHOP = 0;
                int iDonViXuLy_Parent = 0;
                int iDonViXuLy = 0;
                int iKyHop = ID_KyHop_HienTai();
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (iDonViXuLy != 0 && coquan.Where(x => x.ICOQUAN == iDonViXuLy).Count() == 0)
                    {
                        iDonViXuLy = 0;
                    }
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, iDonViXuLy);
                }
                else
                {
                    //coquan = null;
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(null, iDonViXuLy);
                }
                string listKyHop = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                string listNguonKienNghi = "0";
                if (Request["listNguonKienNghi"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo nguồn kiến nghị
                    listNguonKienNghi = Request["listNguonKienNghi"];
                }
                string listLinhVuc = "";
                if (Request["listLinhVuc"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo lĩnh vực
                    listLinhVuc = Request["listLinhVuc"].ToString();
                }
                string maKienNghi = "";
                if (Request["cMaKienNghi"] != null)
                {
                    maKienNghi = Request["cMaKienNghi"].ToString();
                }
                DateTime dTuNgay;
                string tungay;
                if (!DateTime.TryParseExact(Request["dBatDau"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dTuNgay))
                {
                    tungay = "";
                }
                else
                {
                    tungay = dTuNgay.ToString("dd-MMM-yyyy");
                }

                DateTime dDenNgay;
                string denngay;
                if (!DateTime.TryParseExact(Request["dKetThuc"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dDenNgay))
                {
                    denngay = "";
                }
                else
                {
                    denngay = dDenNgay.ToString("dd-MMM-yyyy");
                }
                
                string listHuyen_Xa_ThanhPho = "";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo thành phố
                    listHuyen_Xa_ThanhPho = Request["listHuyen_Xa_ThanhPho"];
                }
                int iDonViTiepNhan = 0;
                string cNoiDungKienNghi = "";
                if (Request["iDonViTiepNhan"] != null) { iDonViTiepNhan = Convert.ToInt32(Request["iDonViTiepNhan"]); }
                if (Request["q"] != null) { cNoiDungKienNghi = func.RemoveTagInput(Request["q"].ToString()); }
                if (Request["cNoiDungKN"] != null) { cNoiDungKienNghi = func.RemoveTagInput(Request["cNoiDungKN"].ToString()); }
                KN_TONGHOP kn_tonghop = get_Request_Paramt_TongHop_KienNghi();
                if (Request["iTruocKyHop"] != null) { kn_tonghop.ITRUOCKYHOP = Convert.ToInt32(Request["iTruocKyHop"]); }
                kn_tonghop.IDONVITONGHOP = 0;
                kn_tonghop.CNOIDUNG = cNoiDungKienNghi;
                //kn_tonghop.IDONVITONGHOP = u_info.user_login.IUSER;
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                var tonghop = _kiennghi.List_PRC_TONGHOP_KIENNGHI_HUYEN(kn_tonghop, listKyHop, maKienNghi, tungay, denngay, listLinhVuc,
                    listNguonKienNghi, iDonViTiepNhan, cNoiDungKienNghi, iUser, listHuyen_Xa_ThanhPho, iDonViXuLy_Parent, page, post_per_page);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (tonghop.Count() > 0)
                {
                    htmlList = kn.KN_Tonghop_Bandannguyen(tonghop, u_info, (int)ThamQuyen_DiaPhuong.Huyen);
                    htmlPhanTrang = "<tr><td colspan='5'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";

                }
                else
                {
                    htmlList = "<tr><td colspan='5' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }
                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }

                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tonghop_Tinh()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            ThamQuyenDiaPhuong = (int)ThamQuyen_DiaPhuong.Tinh;
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("5,6", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!base_business.Action_(5, u_info.tk_action))//chọn Tập hợp
                {
                    ViewData["bt-add"] = " style='display:none'";
                }
                int IDONVITONGHOP = 0;
                int iDonViXuLy_Parent = 0;
                int iDonViXuLy = 0;
                int iKyHop = ID_KyHop_HienTai();
                int iUser = (int)u_info.user_login.IUSER;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser = 0;
                }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (iDonViXuLy != 0 && coquan.Where(x => x.ICOQUAN == iDonViXuLy).Count() == 0)
                    {
                        iDonViXuLy = 0;
                    }
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, iDonViXuLy);
                }
                else
                {
                    //coquan = null;
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(null, iDonViXuLy);
                }
                string listKyHop = "0";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                string listNguonKienNghi = "0";
                if (Request["listNguonKienNghi"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo nguồn kiến nghị
                    listNguonKienNghi = Request["listNguonKienNghi"];
                }
                string listLinhVuc = "";
                if (Request["listLinhVuc"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo lĩnh vực
                    listLinhVuc = Request["listLinhVuc"].ToString();
                }
                string maKienNghi = "";
                if (Request["cMaKienNghi"] != null)
                {
                    maKienNghi = Request["cMaKienNghi"].ToString();
                }
                DateTime dTuNgay;
                string tungay;
                if (!DateTime.TryParseExact(Request["dBatDau"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dTuNgay))
                {
                    tungay = "";
                }
                else
                {
                    tungay = dTuNgay.ToString("dd-MMM-yyyy");
                }

                DateTime dDenNgay;
                string denngay;
                if (!DateTime.TryParseExact(Request["dKetThuc"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dDenNgay))
                {
                    denngay = "";
                }
                else
                {
                    denngay = dDenNgay.ToString("dd-MMM-yyyy");
                }
                
                string listHuyen_Xa_ThanhPho = "";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo thành phố
                    listHuyen_Xa_ThanhPho = Request["listHuyen_Xa_ThanhPho"];
                }
                int iDonViTiepNhan = 0;
                string cNoiDungKienNghi = "";
                if (Request["iDonViTiepNhan"] != null) { iDonViTiepNhan = Convert.ToInt32(Request["iDonViTiepNhan"]); }
                if (Request["cNoiDungKN"] != null) { cNoiDungKienNghi = func.RemoveTagInput(Request["cNoiDungKN"]); }
                if (Request["q"] != null && Request["q"]!="") { cNoiDungKienNghi = func.RemoveTagInput(Request["q"]); }
                KN_TONGHOP kn_tonghop = get_Request_Paramt_TongHop_KienNghi();
                int iTruocKyHop = -1;
                if (Request["iTruocKyHop"] != null)
                { 
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);
                }
                kn_tonghop.ITRUOCKYHOP = iTruocKyHop;
                kn_tonghop.IDONVITONGHOP = IDONVITONGHOP;
                kn_tonghop.ITINHTRANG = 0;
                kn_tonghop.CNOIDUNG = cNoiDungKienNghi;
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                var tonghop = _kiennghi.List_PRC_KIENNGHI_TONGHOP_HDNDTINH(kn_tonghop, listKyHop, maKienNghi, tungay, denngay, listLinhVuc,
                        listNguonKienNghi, iDonViTiepNhan, iUser, listHuyen_Xa_ThanhPho, iDonViXuLy_Parent, page, post_per_page);
                bool isNormalSearch = false;
                bool isAdvancedSearch = false;
                if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                string htmlList = string.Empty;
                string htmlPhanTrang = string.Empty;

                if (tonghop.Count() > 0)
                {
                    htmlList = kn.KN_Tonghop_Bandannguyen(tonghop, u_info, (int)ThamQuyen_DiaPhuong.Tinh);
                    htmlPhanTrang = "<tr><td colspan='5'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                else
                {
                    htmlList = "<tr><td colspan='5' class='alert tcenter alert-danger'>Không có bản ghi nào</td></tr>";
                    if (isAdvancedSearch || isNormalSearch) htmlList = "<tr><td colspan='6' class='alert tcenter alert-danger'>Không có bản ghi nào phù hợp</td></tr>";
                }

                ViewData["list"] = htmlList;
                ViewData["phantrang"] = htmlPhanTrang;
                if ((Request["isLoadAjax"] != null && Request["isLoadAjax"].ToString() == "1") || isAdvancedSearch || isNormalSearch)
                {
                    return Json(new { data = htmlList + htmlPhanTrang }, JsonRequestBehavior.AllowGet);
                }

                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult BaoCaoTongHop_HoiDongNhanDan(string ext = "xls")
        {
           
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
                string fileName;
                string path;
                string listKyHop = "";
                if (Request["listKyHop"] != null)
                {
                    listKyHop = Request["listKyHop"];
                }
                string listLinhVuc = "";
                if (Request["listLinhVuc"] != null)
                {
                    listLinhVuc = Request["listLinhVuc"];
                }
                string listHuyen_Xa_ThanhPho = "";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo lĩnh vực
                    listHuyen_Xa_ThanhPho = Request["listHuyen_Xa_ThanhPho"].ToString();
                }
                int iTenBaoCao = 0;
                if (Request["iTenBaoCao"] != null)
                {
                    iTenBaoCao = Convert.ToInt32(Request["iTenBaoCao"].ToString());
                }
                if (iTenBaoCao == 0)
                {
                    fileName = string.Format("{0}.{1}", "HĐND: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp tỉnh", ext);
                    path = Server.MapPath(ReportConstants.rpt_KN_HDND_BaoCao_KienNghiThamQuyenCapTinh);
                }
                else if (iTenBaoCao == 1)
                {
                    fileName = string.Format("{0}.{1}", "QH: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp tỉnh", ext);
                    path = Server.MapPath(ReportConstants.rpt_KN_QH_BaoCao_KienNghiThamQuyenCapTinh);
                }
                else
                {
                    fileName = string.Format("{0}.{1}", "Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp tỉnh", ext);
                    path = Server.MapPath(ReportConstants.rpt_KN_TongHop_BaoCao_KienNghiThamQuyenCapTinh);
                }
                ExcelFile xls = ExportReportKienNghiCapTinh(iTenBaoCao, path, listKyHop, listLinhVuc, listHuyen_Xa_ThanhPho);
                return Print(xls, ext, fileName);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tập hợp kiến nghị từ Đại biểu HĐND");
                return View("../Home/Error_Exception");
            }
        }

        private ExcelFile ExportReportKienNghiCapTinh(int type, string templatePath, string listKyHop, string listLinhVuc, string listHuyenXaTP)
        {
            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }
            List<KIENNGHI_REPORT_TONGHOP_TINH> listKienNghiReport = _kienNghiReport.GetReportKienNghiHoiDongNhanDan("RPT_KN_HDND_TAPHOPKNTHAMQUYENTINH", type, listKyHop, listLinhVuc, listHuyenXaTP, iUser);

            XlsFile Result = new XlsFile(true);
            Result.Open(templatePath);
            FlexCelReport fr = new FlexCelReport();

            if (type != 0 && type != 1)
            {
                var indexes = listKienNghiReport.GroupBy(x => x.IDOITUONGGUI).Select(g => g.OrderBy(s => s.STT)).Select(g => g.FirstOrDefault().IKIENNGHI).ToList();
                foreach (var item in listKienNghiReport)
                {
                    if (indexes != null && indexes.Contains(item.IKIENNGHI))
                    {
                        if (item.IDOITUONGGUI == 1)
                        {
                            item.ROWTITLE = "I. ĐOÀN ĐẠI BIỂU QUỐC HỘI TỈNH";
                        }
                        else if (item.IDOITUONGGUI == 0)
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

            fr.AddTable<KIENNGHI_REPORT_TONGHOP_TINH>("List", listKienNghiReport);
            fr.UseForm(this).Run(Result);
            return Result;
        }

        public ActionResult Ajax_search_dangxuly()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                ViewData["is_bdn"] = 0;
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();
                ViewData["opt-doan"] = Get_Option_DonViTiepNhan(u_info);
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                    //ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                    //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                }
                ViewData["opt-donvixuly"] = Get_Option_CoQuan_TheoNhom(0);
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                return PartialView("../Ajax/Kiennghi/search_dangxuly");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp đang xử lý");
                throw;
            }

        }
        public ActionResult Ajax_search_chuaxuly()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                ViewData["is_bdn"] = 0;
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();
                ViewData["opt-doan"] = Get_Option_DonViTiepNhan(u_info);
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                    //ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                    //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                }
                ViewData["opt-donvixuly"] = Get_Option_CoQuan_TheoNhom(0);
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                return PartialView("../Ajax/Kiennghi/search_chuaxuly");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp đang xử lý");
                throw;
            }

        }
        public ActionResult Ajax_search_dacotraloi()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();
                ViewData["is_bdn"] = 0;
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                    //ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                }
                ViewData["opt-donvixuly"] = Get_Option_CoQuan_TheoNhom(0);
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();

                ViewData["opt-doan"] = Get_Option_DonViTiepNhan(u_info);

                var phanloai0 = _kiennghi.GetAll_KN_TRALOI_PHANLOAI().Where(x => x.IHIENTHI == 1 && x.IPARENT == 0).OrderByDescending(x => x.IPHANLOAI).ToList();
                ViewData["opt-phanloai"] = kn.OptionPhanLoaiTraLoi(phanloai0, -1);

                return PartialView("../Ajax/Kiennghi/search_dacotraloi");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp đang xử lý");
                throw;
            }

        }

        public ActionResult Ajax_export_dacotraloi()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-kyhop"] = Get_CheckBox_TruocSau_KyHop();
                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                ViewData["huyen_thixa_thanhpho"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, "all");
                //ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho(); 
                
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu("all");
                
                string loaibaocao = "";
                if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='-1' selected>Tất cả<option>";
                    loaibaocao = loaibaocao + "<option value='1'> HĐND Tỉnh<option>";
                    loaibaocao = loaibaocao + "<option value='0'> Đoàn ĐBQH Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='1' selected> HĐND Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                {
                    ViewData["is_dbqh"] = 1;
                    loaibaocao = loaibaocao + "<option value='0' selected> Đoàn ĐBQH Tỉnh<option>";
                }
                ViewData["opt-loaibaocao"] = loaibaocao;

                string strTenbaocao = "";
                strTenbaocao += "<option value='1'>Danh mục kiến nghị của cử tri đã được UBND Tỉnh, Bộ ngành TW trả lời </option>";
                strTenbaocao += "<option value='2'>Danh mục kiến nghị đã được Ủy ban nhân dân tỉnh và Sở ngành trả lời </option>";
                strTenbaocao += "<option value='3'>Tổng số kiến nghị đã được trả lời(Theo tỷ lệ)</option>";
                ViewData["opt-tenbaocao"] = strTenbaocao;

                return PartialView("../Ajax/Kiennghi/Export_Dacotraloi");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp đang xử lý");
                throw;
            }

        }
        public ActionResult Ajax_export_chuaxuly()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-kyhop"] = Get_CheckBox_TruocSau_KyHop();
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();

                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu("");

                string loaibaocao = "";
                if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='1' selected>Tất cả<option>";
                    loaibaocao = loaibaocao + "<option value='2'> HĐND Tỉnh<option>";
                    loaibaocao = loaibaocao + "<option value='3'> Đoàn ĐBQH Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='2' selected> HĐND Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                {
                    ViewData["is_dbqh"] = 1;
                    loaibaocao = loaibaocao + "<option value='3' selected> Đoàn ĐBQH Tỉnh<option>";
                }
                ViewData["opt-loaibaocao"] = loaibaocao;

                string strTenbaocao = "";
                strTenbaocao += "<option value='1'>Danh mục kiến nghị của cử tri đã được UBND Tỉnh, Bộ ngành TW trả lời </option>";
                strTenbaocao += "<option value='2'>Danh mục kiến nghị đã được Ủy ban nhân dân tỉnh và Sở ngành trả lời </option>";
                strTenbaocao += "<option value='3'>Tổng số kiến nghị đã được trả lời(Theo tỷ lệ)</option>";
                ViewData["opt-tenbaocao"] = strTenbaocao;

                return PartialView("../Ajax/Kiennghi/Export_Chuaxuly");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp đang xử lý");
                throw;
            }

        }
        public ActionResult Ajax_export_dangxuly()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-kyhop"] = Get_CheckBox_TruocSau_KyHop();
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();

                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu("");

                string loaibaocao = "";
                if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='1' selected>Tất cả<option>";
                    loaibaocao = loaibaocao + "<option value='2'> HĐND Tỉnh<option>";
                    loaibaocao = loaibaocao + "<option value='3'> Đoàn ĐBQH Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='2' selected> HĐND Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                {
                    ViewData["is_dbqh"] = 1;
                    loaibaocao = loaibaocao + "<option value='3' selected> Đoàn ĐBQH Tỉnh<option>";
                }
                ViewData["opt-loaibaocao"] = loaibaocao;

                string strTenbaocao = "";
                strTenbaocao += "<option value='1'>Danh mục kiến nghị của cử tri đã được UBND Tỉnh, Bộ ngành TW trả lời </option>";
                strTenbaocao += "<option value='2'>Danh mục kiến nghị đã được Ủy ban nhân dân tỉnh và Sở ngành trả lời </option>";
                strTenbaocao += "<option value='3'>Tổng số kiến nghị đã được trả lời(Theo tỷ lệ)</option>";
                ViewData["opt-tenbaocao"] = strTenbaocao;

                return PartialView("../Ajax/Kiennghi/Export_Dangxuly");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp đang xử lý");
                throw;
            }

        }
        public ActionResult Ajax_export_giaitrinh()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-kyhop"] = Get_CheckBox_TruocSau_KyHop();
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();

                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu("");

                string loaibaocao = "";
                if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='1' selected>Tất cả<option>";
                    loaibaocao = loaibaocao + "<option value='2'> HĐND Tỉnh<option>";
                    loaibaocao = loaibaocao + "<option value='3'> Đoàn ĐBQH Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='2' selected> HĐND Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                {
                    ViewData["is_dbqh"] = 1;
                    loaibaocao = loaibaocao + "<option value='3' selected> Đoàn ĐBQH Tỉnh<option>";
                }
                ViewData["opt-loaibaocao"] = loaibaocao;

                string strTenbaocao = "";
                strTenbaocao += "<option value='1'>Danh mục kiến nghị của cử tri đã được UBND Tỉnh, Bộ ngành TW trả lời </option>";
                strTenbaocao += "<option value='2'>Danh mục kiến nghị đã được Ủy ban nhân dân tỉnh và Sở ngành trả lời </option>";
                strTenbaocao += "<option value='3'>Tổng số kiến nghị đã được trả lời(Theo tỷ lệ)</option>";
                ViewData["opt-tenbaocao"] = strTenbaocao;

                return PartialView("../Ajax/Kiennghi/Export_Giaitrinh");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp đang xử lý");
                throw;
            }

        }
        public ActionResult Ajax_export_traloi()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-kyhop"] = Get_CheckBox_TruocSau_KyHop();
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();

                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu("");

                string loaibaocao = "";
                if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='1' selected>Tất cả<option>";
                    loaibaocao = loaibaocao + "<option value='2'> HĐND Tỉnh<option>";
                    loaibaocao = loaibaocao + "<option value='3'> Đoàn ĐBQH Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='2' selected> HĐND Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                {
                    ViewData["is_dbqh"] = 1;
                    loaibaocao = loaibaocao + "<option value='3' selected> Đoàn ĐBQH Tỉnh<option>";
                }
                ViewData["opt-loaibaocao"] = loaibaocao;

                string strTenbaocao = "";
                strTenbaocao += "<option value='1'>Danh mục kiến nghị của cử tri đã được UBND Tỉnh, Bộ ngành TW trả lời </option>";
                strTenbaocao += "<option value='2'>Danh mục kiến nghị đã được Ủy ban nhân dân tỉnh và Sở ngành trả lời </option>";
                strTenbaocao += "<option value='3'>Tổng số kiến nghị đã được trả lời(Theo tỷ lệ)</option>";
                ViewData["opt-tenbaocao"] = strTenbaocao;

                return PartialView("../Ajax/Kiennghi/Export_Traloi");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp đang xử lý");
                throw;
            }

        }
        public ActionResult Ajax_search_giaitrinh()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                ViewData["is_bdn"] = 0;
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();
                ViewData["opt-doan"] = Get_Option_DonViTiepNhan(u_info);
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                    //ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                    //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                }
                ViewData["opt-donvixuly"] = Get_Option_CoQuan_TheoNhom(0);
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                return PartialView("../Ajax/Kiennghi/search_giaitrinh");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp đang xử lý");
                throw;
            }

        }
        public ActionResult Ajax_search_chuatraloi()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();
                ViewData["is_bdn"] = 0;
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                    //ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                }
                ViewData["opt-donvixuly"] = Get_Option_CoQuan_TheoNhom(0);
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();

                ViewData["opt-doan"] = Get_Option_DonViTiepNhan(u_info);

                return PartialView("../Ajax/Kiennghi/search_chuatraloi");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp chưa có trả lời");
                throw;
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_dangxuly_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_TONGHOP tonghop = new KN_TONGHOP();
        //        UserInfor u_info = GetUserInfor();
        //        int ITHAMQUYENDONVI = 0;
        //        //tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
        //        if (fc["q"] != null && fc["q"]!="") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]); }
        //        if (fc["iKyHop"] != null){tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]);}
        //        if (fc["cNoiDung"] != null && fc["cNoiDung"]!="") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }                
        //        if (fc["iDonViXuLy"] != null) {
        //            tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]);
        //            ITHAMQUYENDONVI= Convert.ToInt32(fc["iDonViXuLy"]);
        //        }
        //        if (fc["iLinhVuc"] != null) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }

        //        if (!u_info.tk_action.is_lanhdao)
        //        {
        //           tonghop.ITHAMQUYENDONVI = u_info.user_login.IDONVI;
        //        }
        //        tonghop.ITINHTRANG = (int)TrangThai_TongHop.DangXuLy;
        //        var th = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(tonghop);
        //        //th = th.Where(x => x.ITINHTRANG >= (int)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen).ToList();
        //        th = th.Where(x => x.SOKIENNGHI_CHUATRALOI > 0).ToList();
        //        if (th.Count() > 0)
        //        {
        //            //Response.Write("<tr><td colspan='3' class='alert-info tcenter'>Có " + th.Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        Response.Write(kn.KN_Tonghop_Dangxuly(th, GetUserInfor().tk_action));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Kết quả tìm Tập hợp đang xử lý");
        //        throw;
        //    }

        //}
        public ActionResult Ajax_search_theodoi_diaphuong()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                int iKyHop = ID_KyHop_HienTai();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-donvixuly"] = Get_Option_DoanDaiBieu();
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                //linh vực            
                //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_theodoi_diaphuong");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Tìm Tập hợp kiến nghị thuộc thẩm quyền địa phương");
                throw;
            }
        }
        public ActionResult Ajax_search_theodoi_chuyengiaiquyet()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                //ViewData["opt-doan"] = Get_Option_DonViTiepNhan();
                //linh vực            
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                ViewData["check-hienthi"] = Get_CheckBox_TruocKyHop();
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_theodoi_chuyengiaiquyet");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form tìm Tập hợp kiến nghị đã chuyển Cơ quan Trung ương giải quyết");
                throw;
            }

        }
        public ActionResult Ajax_search_bandannguyen_chuyen()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                //ViewData["opt-donvixuly"] = "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
                //ViewData["opt-doan"] = Get_Option_DonViTiepNhan();
                //linh vực            
                //ViewData["opt-linhvuc"] = Get_Option_Linhvuc();
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_bandannguyen_chuyen");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form tìm Tập hợp kiến nghị Ban Dân nguyện đã chuyển xử lý");
                throw;
            }

        }
        public ActionResult Ajax_search_moicapnhat()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //func.SetCookies("url_return", Request.Url.AbsoluteUri);
                
                string url = Request.Url.AbsoluteUri;
                
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                string listKyHop = "";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                ViewData["kyhop"] = List_CheckBox_KyHop(iKyHop, listKyHop);
                //ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                //ViewData["opt-nguonkiennghi"] = kn.Option_NguonKienNghi();

                string lstNguonKN = "all";
                if (Request["iTruocKyHop"] != null) lstNguonKN = "";
                if (Request["listNguonKN"] != null)
                {
                    lstNguonKN = Request["listNguonKN"];
                }

                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, lstNguonKN);

                int iDonViXuLy = 0;
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }

                ViewData["opt-donvixuly"] = Get_Option_CoQuan_TheoNhom(iDonViXuLy);

                ViewData["is_dbqh"] = 1;
                string loaibaocao = "";
                if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='1'>Tất cả<option>";
                    loaibaocao = loaibaocao + "<option value='2'> HĐND Tỉnh<option>";
                    loaibaocao = loaibaocao + "<option value='3'> Đoàn ĐBQH Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='2'> HĐND Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                {
                    ViewData["is_dbqh"] = 1;
                    loaibaocao = loaibaocao + "<option value='3'> Đoàn ĐBQH Tỉnh<option>";
                }
                ViewData["loaibaocao"] = loaibaocao;
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = khieunai.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                //if (u_info.tk_action.is_lanhdao)
                //{
                //    ViewData["is_dbqh"] = 0;
                //    ViewData["opt-doan"] = Get_Option_DonViTiepNhan();
                //}
                int iDonViTiepNhan = 0;
                if (Request["iDoan"] != null)
                {
                    iDonViTiepNhan = Convert.ToInt32(Request["iDoan"]);
                }
                ViewData["opt-doan"] = Get_Option_DonViTiepNhan(u_info, iDonViTiepNhan);
                //linh vực            
                //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                string lstLinhVuc = "all";
                if (Request["iTruocKyHop"] != null) lstLinhVuc = "";

                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu(lstLinhVuc);
                

                int iTruocKyHop = 1;
                if (Request["iTruocKyHop"] != null)
                {
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);
                }

                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop(iTruocKyHop);
                //end lĩnh vực

                string imakiennghi = "";
                if (Request["iMaKienNghi"] != "" && Request["iMaKienNghi"] != null)
                {
                    imakiennghi = Request["iMaKienNghi"];
                }
                ViewData["MaKienNghi"] = "<input type='text' class='input-block-level' id='iMaKienNghi' name='iMaKienNghi' value='" + imakiennghi + "' />";

                // thoi gian
                string dtungay = "";
                string ddenngay = "";
                if (Request["iTuNgay"] != null && Request["iTuNgay"] != "")
                {
                    dtungay = Request["iTuNgay"];
                }
                ViewData["TuNgay"] = " value='" + dtungay + "' ";
                if (Request["iDenNgay"] != null && Request["iDenNgay"] != "")
                {
                    ddenngay = Request["iDenNgay"];
                }
                ViewData["DenNgay"] = " value='" + ddenngay + "' ";

                string cnoidung = Request["cNoiDung"];
                ViewData["NoiDung"] = " value='" + cnoidung + "' ";

                return PartialView("../Ajax/Kiennghi/search_moicapnhat");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm kiến mới cập nhật");
                throw;
            }

        }

        public ActionResult Ajax_search_tamxoa()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                string url = Request.Url.AbsoluteUri;
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                string listKyHop = "";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                ViewData["kyhop"] = List_CheckBox_KyHop(iKyHop, listKyHop);
                //ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                //ViewData["opt-nguonkiennghi"] = kn.Option_NguonKienNghi();

                string lstNguonKN = "all";
                if (Request["iTruocKyHop"] != null) lstNguonKN = "";
                if (Request["listNguonKN"] != null)
                {
                    lstNguonKN = Request["listNguonKN"];
                }

                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, lstNguonKN);

                int iDonViXuLy = 0;
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }

                ViewData["opt-donvixuly"] = Get_Option_CoQuan_TheoNhom(iDonViXuLy);

                ViewData["is_dbqh"] = 1;
                string loaibaocao = "";
                if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='1'>Tất cả<option>";
                    loaibaocao = loaibaocao + "<option value='2'> HĐND Tỉnh<option>";
                    loaibaocao = loaibaocao + "<option value='3'> Đoàn ĐBQH Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                {
                    ViewData["is_dbqh"] = 0;
                    loaibaocao = loaibaocao + "<option value='2'> HĐND Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                {
                    ViewData["is_dbqh"] = 1;
                    loaibaocao = loaibaocao + "<option value='3'> Đoàn ĐBQH Tỉnh<option>";
                }
                ViewData["loaibaocao"] = loaibaocao;
                _condition = new Dictionary<string, object>();
                _condition.Add("IHIENTHI", 1);
                _condition.Add("IDELETE", 0);
                List<DIAPHUONG> lstDiaphuong = kntc.List_DiaPhuong(_condition);
                ViewData["opt-tinh"] = khieunai.Option_TinhThanh_ByID_Parent(lstDiaphuong, 0, 0);
                //if (u_info.tk_action.is_lanhdao)
                //{
                //    ViewData["is_dbqh"] = 0;
                //    ViewData["opt-doan"] = Get_Option_DonViTiepNhan();
                //}
                int iDonViTiepNhan = 0;
                if (Request["iDoan"] != null)
                {
                    iDonViTiepNhan = Convert.ToInt32(Request["iDoan"]);
                }
                ViewData["opt-doan"] = Get_Option_DonViTiepNhan(u_info, iDonViTiepNhan);
                //linh vực            
                //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                string lstLinhVuc = "all";
                if (Request["iTruocKyHop"] != null) lstLinhVuc = "";

                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu(lstLinhVuc);

                int iTruocKyHop = 1;
                if (Request["iTruocKyHop"] != null)
                {
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);
                }

                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop(iTruocKyHop);
                //end lĩnh vực

                string imakiennghi = "";
                if (Request["iMaKienNghi"] != "" && Request["iMaKienNghi"] != null)
                {
                    imakiennghi = Request["iMaKienNghi"];
                }
                ViewData["MaKienNghi"] = "<input type='text' class='input-block-level' id='iMaKienNghi' name='iMaKienNghi' value='" + imakiennghi + "' />";

                // thoi gian
                string dtungay = "";
                string ddenngay = "";
                if (Request["iTuNgay"] != null && Request["iTuNgay"] != "")
                {
                    dtungay = Request["iTuNgay"];
                }
                ViewData["TuNgay"] = " value='" + dtungay + "' ";
                if (Request["iDenNgay"] != null && Request["iDenNgay"] != "")
                {
                    ddenngay = Request["iDenNgay"];
                }
                ViewData["DenNgay"] = " value='" + ddenngay + "' ";

                string cnoidung = Request["cNoiDung"];
                ViewData["NoiDung"] = " value='" + cnoidung + "' ";

                return PartialView("../Ajax/Kiennghi/search_tamxoa");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm kiến mới cập nhật");
                throw;
            }

        }


        public ActionResult Ajax_export_moicapnhat_popup()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["kyhop"] = List_CheckBox_KyHop(iKyHop);
                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, "all");

                string loaibaocao = "";
                if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
                {
                    loaibaocao = loaibaocao + "<option selected value='1'>Tất cả<option>";
                    loaibaocao = loaibaocao + "<option value='2'> HĐND Tỉnh<option>";
                    loaibaocao = loaibaocao + "<option value='3'> Đoàn ĐBQH Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                {
                    loaibaocao = loaibaocao + "<option selected value='2'> HĐND Tỉnh<option>";
                }
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                {
                    loaibaocao = loaibaocao + "<option selected value='3'> Đoàn ĐBQH Tỉnh<option>";
                }
                ViewData["loaibaocao"] = loaibaocao;
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu("all");
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_moicapnhat_popup");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm kiến mới cập nhật");
                throw;
            }

        }

        public ActionResult Ajax_search_tonghop()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_tonghop");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm Tập hợp kiến nghị tại địa phương");
                throw;
            }

        }
        public ActionResult Ajax_search_tonghop_chuyendiaphuong()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                return PartialView("../Ajax/Kiennghi/search_tonghop_chuyendiaphuong");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm Tập hợp kiến nghị đã chuyển chuyển địa phương giải quyết");
                throw;
            }

        }
        public ActionResult Ajax_search_tonghop_chuyendannguyen()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                //ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                //ViewData["opt-doan"] = "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
                //linh vực            
                //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_tonghop_chuyendannguyen");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form tìm Tập hợp kiến nghị đã chuyển Ban dân nguyện Tập hợp");
                throw;
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_tonghop_chuyendannguyen_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_TONGHOP kn_tonghop = new KN_TONGHOP();

        //        kn_tonghop.IDONVITONGHOP = GetUserInfor().user_login.IDONVI;

        //        if (fc["q"] != null && fc["q"]!="") {
        //            kn_tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
        //        }
        //        if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { kn_tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iTruocKyHop"] != null) { kn_tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        if (fc["iKyHop"] != null) { kn_tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        var list_tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(kn_tonghop, 0);
        //        list_tonghop = list_tonghop.Where(x => x.ITINHTRANG == (int)TrangThai_TongHop.DaChuyenBanDanNguyen).ToList();
        //        if (list_tonghop.Count() > 0)
        //        {
        //            Response.Write("<tr><td colspan='4' class='alert-info tcenter'>Có " + list_tonghop.Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        Response.Write(kn.KN_Tonghop_ChuyenDanNguyen(list_tonghop));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {

        //        log.Log_Error(ex, "Tìm Tập hợp kiến nghị đã chuyển Ban Dân nguyện");
        //        throw;
        //    }

        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_tonghop_chuyendiaphuong_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_TONGHOP tonghop = new KN_TONGHOP();

        //        tonghop.IDONVITONGHOP = GetUserInfor().user_login.IDONVI;
        //        tonghop.ITHAMQUYENDONVI = GetUserInfor().user_login.IDONVI;
        //        if (fc["q"] != null && fc["q"]!="") {
        //            tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
        //        }
        //        if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        var th = _kiennghi.Search_Tonghop(tonghop).ToList();
        //        th = th.Where(x => x.ITINHTRANG >= (int)TrangThai_TongHop.DangXuLy).ToList();

        //        if (th.Count() > 0)
        //        {
        //            Response.Write("<tr><td colspan='5' class='alert-info tcenter'>Có " + th.Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        Response.Write(kn.DBQH_Tonghop_ChuyenDiaPhuongXuLy(th));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {

        //        log.Log_Error(ex, "Tìm Tập hợp kiến nghị đã chuyển địa phương giải quyết");
        //        throw;
        //    }

        //}
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_search_moicapnhat_result(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                KN_KIENNGHI tonghop = new KN_KIENNGHI();
                tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.ChoXuLy;
                if (fc["q"] != null && fc["q"] != "")
                {
                    tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
                    //tonghop.IKYHOP = ID_KyHop_HienTai();
                }
                if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
                if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
                UserInfor u_info = GetUserInfor();
                if (u_info.tk_action.is_dbqh)
                {
                    tonghop.IDONVITIEPNHAN = (int)u_info.user_login.IDONVI;
                }
                else
                {
                    if (fc["iDoan"] != null) { tonghop.IDONVITIEPNHAN = Convert.ToInt32(fc["iDoan"]); }
                }
                if (fc["iDonViXuLy"] != null) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
                if (fc["iLinhVuc"] != null) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
                if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
                List<PRC_KIENNGHI_MOICAPNHAT> list_kn;
                if (u_info.tk_action.is_lanhdao)
                {
                    list_kn = _kiennghi.PRC_KIENNGHI_MOICAPNHAT(tonghop, 0);
                }
                else
                {
                    list_kn = _kiennghi.PRC_KIENNGHI_MOICAPNHAT(tonghop, 0);
                }
                if (!u_info.tk_action.is_lanhdao)
                {
                    list_kn.Where(x => x.ITINHTRANG == 0).ToList();
                }
                List<ID_Session_KienNghi_ChonTongHop> list_id_kiennghi_tonghop = Get_IDKienNghiChonTongHop();
                if (list_kn.Count() > 0)
                {
                    Response.Write("<tr><td colspan='4' class='alert-info tcenter'>Có " + list_kn.GroupBy(x => x.ID_KIENNGHI).Count() + " kết quả tìm kiếm.</td></tr>");
                }
                Response.Write(kn.KN_Moicapnhat_Tracuu(list_kn, u_info.tk_action, list_id_kiennghi_tonghop));
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiến nghị mới cập nhật");
                throw;
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_bandannguyen_chuyen_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_TONGHOP tonghop = new KN_TONGHOP();
        //        tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
        //        //tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen;
        //        if (fc["q"] != null) { tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]); }
        //        if (fc["cNoiDung"] != null) { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        //if (fc["iDonViXuLy"] != null) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
        //        tonghop.ITHAMQUYENDONVI = (int)GetUserInfor().user_login.IDONVI;
        //        //if (fc["iLinhVuc"] != null && Convert.ToInt32(fc["iLinhVuc"]) != -1) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
        //        var tonghop_list = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(tonghop);
        //        tonghop_list = tonghop_list.Where(x => x.ITINHTRANG == (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen).ToList();

        //        //th = th.Where(x => x.ITINHTRANG >= (int)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen).ToList();
        //        List<QUOCHOI_COQUAN> coquan;
        //        Dictionary<string, object> donvi = new Dictionary<string, object>();
        //        coquan = _kiennghi.GetAll_CoQuanByParam(donvi).Where(x => x.IPARENT != ID_Coquan_doandaibieu).ToList();
        //        if (tonghop_list.Count() > 0)
        //        {
        //            Response.Write("<tr><td colspan='4' class='alert-info tcenter'>Có " + tonghop_list.Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        Response.Write(kn.KN_Tonghop_bandannguyen_chuyen(tonghop_list, GetUserInfor().tk_action));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Tìm Tập hợp kiến nghị đã chuyển Cơ quan Trung ương giải quyết");
        //        throw;
        //    }

        //}
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_search_tonghop_result(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                KN_TONGHOP tonghop = new KN_TONGHOP();
                UserInfor u_info = GetUserInfor();
                tonghop.IDONVITONGHOP = (int)u_info.user_login.IDONVI;
                tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.ChoXuLy;
                if (fc["q"] != null && fc["q"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]); }
                if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
                if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
                if (fc["iDonViXuLy"] != null && Convert.ToInt32(fc["iDonViXuLy"]) != -1) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
                if (fc["iLinhVuc"] != null && Convert.ToInt32(fc["iLinhVuc"]) != -1) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
                if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }


                List<PRC_TONGHOP_KIENNGHI> list_tonghop = _kiennghi.List_PRC_TONGHOP_KIENNGHI(tonghop, 0);

                //if (list_tonghop.Count() > 0)
                //{
                //    Response.Write("<tr><td colspan='5' class='alert-info tcenter'>Có " + list_tonghop.GroupBy(x=>x.ITONGHOP).Count() + " kết quả tìm kiếm.</td></tr>");
                //}
                Response.Write(kn.KN_Tonghop_Doan_Choxuly(list_tonghop, u_info.tk_action));
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm Tập hợp kiến nghị tại địa phương");
                throw;
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_theodoi_chuyengiaiquyet_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_TONGHOP tonghop = new KN_TONGHOP();

        //        tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
        //        //tonghop.ITINHTRANG = 2;
        //        if (fc["q"] != null && fc["q"]!="") {
        //            tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
        //        }
        //        if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        if (fc["iDonViXuLy"] != null && Convert.ToInt32(fc["iDonViXuLy"]) != 0) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
        //        if (fc["iLinhVuc"] != null && Convert.ToInt32(fc["iLinhVuc"]) != -1) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
        //        var list_tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(tonghop, 0);
        //        if (list_tonghop.Count() > 0)
        //        {
        //            Response.Write("<tr><td colspan='4' class='alert-info tcenter'>Có " + list_tonghop.Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        Response.Write(kn.KN_Theodoi_tonghop_chuyendiaphuong(list_tonghop));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Form tìm Tập hợp kiến nghị đã chuyển giải quyết");
        //        throw;
        //    }

        //}
        public ActionResult Ajax_search_theodoi_dannguyen()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                ViewData["opt-doan"] = Get_Option_DonViTiepNhan();
                //linh vực            
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_theodoi_dannguyen");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form tìm Tập hợp kiến nghị đã chuyển Ban Dân nguyện");
                throw;
            }

        }
        public ActionResult Ajax_search_theodoi_kiennghi_tralai_khongxuly()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                return PartialView("../Ajax/Kiennghi/search_theodoi_kiennghi_tralai_khongxuly");

            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form tìm kiến nghị đã trả lại");
                throw;
            }

        }
        /*
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_search_theodoi_kiennghi_tralai_khongxuly_result(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                KN_KIENNGHI tonghop = new KN_KIENNGHI();

                //tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
                UserInfor u_info = GetUserInfor();
                if (fc["q"] != null) { tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
                    tonghop.IKYHOP = ID_KyHop_HienTai();
                }
                if (fc["cNoiDung"] != null) { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
                if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
                if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
                tonghop.ITHAMQUYENDONVI = u_info.user_login.IDONVI;

                var traloi = _kiennghi.GetAll_TraLoi_KienNghi();
                var giamsat = _kiennghi.GetAll_Giamsat_TraLoi();
                //ViewData["list"] = kn.KN_Theodoi_kiennghi_chuyenkysau(list_kn, traloi, giamsat, u_info.tk_action);
                var list_kn = _kiennghi.PRC_KIENNGHI_TRALAI(tonghop, 0);

                Response.Write(kn.KN_Theodoi_kiennghi_tralai(list_kn, traloi,giamsat, GetUserInfor().tk_action));
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiến nghị đã trả lại");
                throw;
            }

        }
        */
        public ActionResult Ajax_search_theodoi_kiennghi_tralai()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iKyHop = ID_KyHop_HienTai();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                ViewData["dbqh"] = 1;
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["dbqh"] = 0;
                    ViewData["opt-doan"] = Get_Option_DonViTiepNhan();
                }
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                ViewData["opt-hinhthuc"] = Get_CheckBox_TruocKyHop();
                return PartialView("../Ajax/Kiennghi/search_theodoi_kiennghi_tralai");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form tìm kiến nghị trả lại");
                throw;
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_theodoi_kiennghi_tralai_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_KIENNGHI tonghop = new KN_KIENNGHI();
        //        //tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;

        //        if (fc["q"] != null && fc["q"]!="") {
        //            tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
        //        }
        //        UserInfor u_info = GetUserInfor();
        //        if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        if (fc["iDoan"] != null) { tonghop.IDONVITIEPNHAN = Convert.ToInt32(fc["iDoan"]); }
        //        if (fc["iDonViXuLy"] != null) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
        //        if (fc["iLinhVuc"] != null) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
        //        if (u_info.tk_action.is_chuyenvien)
        //        {
        //            tonghop.ITHAMQUYENDONVI = u_info.user_login.IDONVI;
        //        }
        //        //tonghop.ITINHTRANG = (decimal)TrangThaiKienNghi.DaTraLaiKienNghi;

        //        var th = _kiennghi.PRC_LIST_KN_TRALOI_DANHGIA(tonghop, 0);

        //        if(tonghop.IDONVITIEPNHAN==0 && tonghop.ITHAMQUYENDONVI==0 && u_info.tk_action.is_dbqh)
        //        {
        //            decimal id_donvi_user = (decimal)u_info.user_login.IDONVI;
        //            th = th.Where(x => x.ID_DONVITIEPNHAN == id_donvi_user || x.ID_THAMQUYENDONVI == id_donvi_user).ToList();
        //        }
        //        //List<QUOCHOI_COQUAN> coquan;
        //        //Dictionary<string, object> donvi = new Dictionary<string, object>();
        //        ////donvi.Add("IPARENT", ID_Coquan_doandaibieu);
        //        //coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
        //        //if (th.Count() > 0)
        //        //{
        //        //    Response.Write("<tr><td colspan='5' class='alert-info tcenter'>Có " + th.Count() + " kết quả tìm kiếm.</td></tr>");
        //        //}
        //       /// var traloi = _kiennghi.GetAll_TraLoi_KienNghi();
        //        //var giamsat = _kiennghi.GetAll_Giamsat_TraLoi();
        //        Response.Write(kn.KN_Theodoi_kiennghi_chuyenkysau(th, u_info.tk_action));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {

        //        log.Log_Error(ex, "Tìm kiến nghị đã trả lại");
        //        throw;
        //    }

        //}
        public ActionResult Ajax_search_theodoi_kiennghi_chuyenkysau()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iKyHop = ID_KyHop_HienTai();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                ViewData["dbqh"] = 0;
                if (!u_info.tk_action.is_lanhdao)
                {
                    ViewData["dbqh"] = 1;
                    //ViewData["opt-donvixuly"] = "<option value='"+ u_info.tk_action.iUser + "'>" + u_info.tk_action.tendonvi + "</option>";
                }
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                ViewData["opt-doan"] = Get_Option_DonViTiepNhan();
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                return PartialView("../Ajax/Kiennghi/search_theodoi_kiennghi_chuyenkysau");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form tìm kiến nghị tồn qua nhiều kỳ họp");
                throw;
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_theodoi_kiennghi_chuyenkysau_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_KIENNGHI tonghop = new KN_KIENNGHI();

        //        //tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
        //        UserInfor u_info = GetUserInfor();
        //        if (fc["q"] != null && fc["q"]!="") {
        //            tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
        //            //tonghop.IKYHOP = ID_KyHop_HienTai();
        //        }

        //        if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        if (fc["iDoan"] != null) { tonghop.IDONVITIEPNHAN = Convert.ToInt32(fc["iDoan"]); }
        //        if (fc["iDonViXuLy"] != null) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
        //        if (fc["iLinhVuc"] != null) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
        //        if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        if (!u_info.tk_action.is_lanhdao)
        //        {
        //            tonghop.IDONVITIEPNHAN = u_info.user_login.IDONVI;
        //        }

        //        var th = _kiennghi.PRC_LIST_KN_TRALOI_DANHGIA(tonghop,0);
        //        th = th.Where(x => x.GIAMSAT_DONGKIENNGHI == 0 && x.GIAMSAT_PHANLOAI!=null).OrderByDescending(x => x.ID_LINHVUC_KIENNGHI).ToList();
        //        Response.Write(kn.KN_Theodoi_kiennghi_chuyenkysau(th, u_info.tk_action));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Tìm kiến nghị theo dõi chuyển kỳ sau");
        //        throw;
        //    }

        //}
        public ActionResult Ajax_search_theodoi_luu()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["kyhop"] = List_CheckBox_KyHop();
                int iDonViXuLy = 0;
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                ViewData["opt-donvixuly"] = Get_Option_CoQuan_TheoNhom(iDonViXuLy);
                // ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                UserInfor u_info = GetUserInfor();
                ViewData["dbqh"] = 1;
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["opt-doan"] = Get_Option_DonViTiepNhan(); 
                    ViewData["dbqh"] = 0;
                }
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                
                string lstNguonKN = "all";
                string lstLinhVuc = "all";
                if (Request["iTruocKyHop"] != null) {
                    lstLinhVuc = "";
                    lstNguonKN = "";
                }
                    
                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                if (Request["listNguonKN"] != null)
                {
                    lstNguonKN = Request["listNguonKN"];
                }
                //linh vực            
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu(lstLinhVuc);
                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, lstNguonKN);

                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_theodoi_luu");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form tìm kiến nghị trùng, lưu theo dõi");
                throw;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_search_theodoi_luu_result(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI tonghop = new KN_KIENNGHI();
                if (fc["q"] != null && fc["q"] != "")
                {
                    tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
                    //tonghop.IKYHOP = ID_KyHop_HienTai();
                }
                if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
                if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
                if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
                if (fc["iDoan"] != null) { tonghop.IDONVITIEPNHAN = Convert.ToInt32(fc["iDoan"]); }
                if (fc["iDonViXuLy"] != null) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
                if (fc["iLinhVuc"] != null) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
                tonghop.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiTrung_DongKienNghi;
                if (u_info.tk_action.is_dbqh)
                {
                    tonghop.IDONVITIEPNHAN = u_info.user_login.IDONVI;
                }
                var th = _kiennghi.PRC_KIENNGHI_LIST(tonghop, 0);
                th = th.Where(x => x.ID_KIENNGHI_TRUNG != 0).ToList();
                if (th.Count() > 0)
                {
                    Response.Write("<tr><td colspan='4' class='alert-info tcenter'>Có " + th.Count() + " kết quả tìm kiếm.</td></tr>");
                    Response.Write(kn.KN_Theodoi_luu(th));
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiến nghị trùng, lưu theo dõi");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_search_theodoi_dannguyen_result(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                KN_TONGHOP tonghop = new KN_TONGHOP();

                tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
                tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DaChuyenBanDanNguyen;
                if (fc["q"] != null && fc["q"] != "")
                {
                    tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
                    //tonghop.IKYHOP = ID_KyHop_HienTai();
                }
                if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
                if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
                if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
                if (fc["iDoan"] != null && Convert.ToInt32(fc["iDoan"]) != 0) { tonghop.IDONVITONGHOP = Convert.ToInt32(fc["iDoan"]); }
                if (fc["iDonViXuLy"] != null && Convert.ToInt32(fc["iDonViXuLy"]) != -1) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
                if (fc["iLinhVuc"] != null && Convert.ToInt32(fc["iLinhVuc"]) != -1) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
                var th = _kiennghi.Search_Tonghop(tonghop);

                List<QUOCHOI_COQUAN> coquan;
                Dictionary<string, object> donvi = new Dictionary<string, object>();
                coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
                if (th.Count() > 0)
                {
                    Response.Write("<tr><td colspan='4' class='alert-info tcenter'>Có " + th.Count() + " kết quả tìm kiếm.</td></tr>");
                }
                //Response.Write(kn.KN_Theodoi_tonghop_chuyendiaphuong(th, coquan, GetUserInfor().tk_action));
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm Tập hợp kiến nghị đã chuyển Ban Dân nguyện");
                throw;
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_theodoi_diaphuong_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_TONGHOP tonghop = new KN_TONGHOP();
        //        //tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
        //        tonghop.IKYHOP = ID_KyHop_HienTai();
        //        if (fc["q"] != null) { tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);}
        //        if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        if (fc["cNoiDung"] != null) { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        if (fc["iDonViXuLy"] != null && Convert.ToInt32(fc["iDonViXuLy"]) != -1) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }


        //        var list_tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(tonghop, 20);
        //        list_tonghop = list_tonghop.Where(x => x.ITINHTRANG >= (int)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen).ToList();

        //        if (list_tonghop.Count() > 0)
        //        {
        //            Response.Write("<tr><td colspan='5' class='alert-info tcenter'>Có " + list_tonghop.Count() + " kết quả tìm kiếm.</td></tr>");
        //            Response.Write(kn.KN_Theodoi_tonghop_chuyendiaphuong(list_tonghop));
        //        }search_Traloi(
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Form tìm Tập hợp kiến nghị thuộc địa phương");
        //        throw;
        //    }

        //}
        public ActionResult Ajax_search_traloi()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["opt-kyhop"] = List_CheckBox_KyHop();
                UserInfor u_info = GetUserInfor();
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                ViewData["is_bdn"] = 0;
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();
                ViewData["opt-doan"] = Get_Option_DonViTiepNhan(u_info);
                if (u_info.tk_action.is_lanhdao)
                {
                    ViewData["is_bdn"] = 1;
                    //ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                    //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                }
                ViewData["opt-donvixuly"] = Get_Option_CoQuan_TheoNhom(0); ;
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_traloi");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form tìm Tập hợp kiến nghị đã trả lời");
                throw;
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_traloi_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_TONGHOP tonghop = new KN_TONGHOP(); UserInfor u_info = GetUserInfor();
        //        //tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
        //        if (fc["q"] != null && fc["q"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]); }
        //        if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        if (fc["iDonViXuLy"] != null) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
        //        if (fc["iLinhVuc"] != null) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
        //        if (!u_info.tk_action.is_lanhdao)
        //        {
        //            tonghop.ITHAMQUYENDONVI = u_info.user_login.IDONVI;
        //        }
        //        tonghop.ITINHTRANG = (int)TrangThai_TongHop.DangXuLy;
        //        var th = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(tonghop);
        //        th = th.Where(x => x.SOKIENNGHI_CHUATRALOI>0).ToList();
        //        if (th.Count() > 0)
        //        {
        //            //Response.Write("<tr><td colspan='3' class='alert-info tcenter'>Có " + th.Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        Response.Write(kn.KN_Tonghop_Dangxuly(th, GetUserInfor().tk_action));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Tìm Tập hợp kiến nghị đã trả lời");
        //        throw;
        //    }

        //}
        public ActionResult Ajax_search_theodoi_kiennghi_dannguyen()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                return PartialView("../Ajax/Kiennghi/search_theodoi_kiennghi_dannguyen");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form tìm Tập hợp kiến nghị đã chuyển Ban Dân nguyện");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_search_theodoi_kiennghi_dannguyen_result(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI k = new KN_KIENNGHI();
                k.ITINHTRANG = -1;
                k.IKYHOP = 0;
                k.ILINHVUC = -1;
                k.ITHAMQUYENDONVI = -1;
                k.ITRUOCKYHOP = -1;
                k.IDONVITIEPNHAN = (int)u_info.user_login.IDONVI;
                if (fc["iKyHop"] != null) { k.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
                if (fc["iTruocKyHop"] != null) { k.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
                if (fc["iLinhVuc"] != null && Convert.ToInt32(fc["iLinhVuc"]) != -1) { k.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
                if (fc["iDonViXuLy"] != null && Convert.ToInt32(fc["iDonViXuLy"]) != -1) { k.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
                if (fc["q"] != null && fc["q"] != "") { k.CNOIDUNG = func.RemoveTagInput(fc["q"]); }
                if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { k.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
                var kiennghi = _kiennghi.GetAll_KienNghi_Tracuu(k).Where(x => x.ITHAMQUYENDONVI != (int)u_info.user_login.IDONVI).ToList();
                if (kiennghi.Count() > 0)
                {
                    Response.Write("<tr><td colspan='5' class='alert-info tcenter'>Có " + kiennghi.Count() + " kết quả tìm kiếm.</td></tr>");
                }
                Response.Write(kn.KN_Tonghop_Kiennghi_ChuyenDanNguyen(kiennghi, u_info.tk_action));
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm Tập hợp kiến nghị đã chuyển Ban Dân nguyện");
                throw;
            }

        }
        public ActionResult Ajax_search_theodoi_kiennghi_diaphuong()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //if (!CheckAuthToken()) { return null; }
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-donvixuly"] = "<option value='" + u_info.user_login.IDONVI + "'>" + u_info.tk_action.tendonvi + "</option>";
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                return PartialView("../Ajax/Kiennghi/search_theodoi_kiennghi_diaphuong");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form Tìm Tập hợp kiến nghị thuộc địa phương");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_search_theodoi_kiennghi_diaphuong_result(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {

                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI k = new KN_KIENNGHI();
                k.ITINHTRANG = -1;
                k.IKYHOP = 0;
                k.ILINHVUC = -1;
                k.ITHAMQUYENDONVI = (int)u_info.user_login.IDONVI;
                k.ITRUOCKYHOP = -1;
                k.IDONVITIEPNHAN = 0;
                if (fc["iKyHop"] != null)
                {
                    k.IKYHOP = Convert.ToInt32(fc["iKyHop"]);
                    //k.IKYHOP = ID_KyHop_HienTai();
                }
                if (fc["iTruocKyHop"] != null) { k.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
                if (fc["iLinhVuc"] != null && Convert.ToInt32(fc["iLinhVuc"]) != -1) { k.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
                if (fc["q"] != null && fc["q"] != "") { k.CNOIDUNG = func.RemoveTagInput(fc["q"]); }
                if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { k.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
                var kiennghi = _kiennghi.GetAll_KienNghi_Tracuu(k);
                if (kiennghi.Count() > 0)
                {
                    Response.Write("<tr><td colspan='5' class='alert-info tcenter'>Có " + kiennghi.Count() + " kết quả tìm kiếm.</td></tr>");
                }
                Response.Write(kn.KN_Tonghop_Kiennghi_Diaphuong(kiennghi, u_info.tk_action));
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm Tập hợp kiến nghị thuộc địa phương");
                throw;
            }

        }
        public ActionResult Ajax_search_tonghop_bandannguyen()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iKyHop = ID_KyHop_HienTai();
                //ma kien nghi
                string imakiennghi = "";
                if (Request["cMaKienNghi"] != "" && Request["cMaKienNghi"] != null)
                {
                    imakiennghi = Request["cMaKienNghi"];
                }
                ViewData["MaKienNghi"] = "<input type='text' class='input-block-level' id='cMaKienNghi' name='cMaKienNghi' value='" + imakiennghi + "' />";
                // ki hop
                string listKyHop = "";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                ViewData["kyhop"] = List_CheckBox_KyHop(iKyHop, listKyHop);
                // thoi gian
                string dtungay = "";
                string ddenngay = "";
                if (Request["dBatDau"] != null && Request["dBatDau"] != "")
                {
                    dtungay = Request["dBatDau"];
                }
                ViewData["TuNgay"] = " value='" + dtungay + "' ";
                if (Request["dKetThuc"] != null && Request["dKetThuc"] != "")
                {
                    ddenngay = Request["dKetThuc"];
                }
                ViewData["DenNgay"] = " value='" + ddenngay + "' ";

                // ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();
                ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(0);

                //don vi tham quyen
                int iDonViXuLy = 0;
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }

                ViewData["opt-donvithamquyen"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Trunguong, iDonViXuLy);
                // ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();

                //truoc ki hop
                int iTruocKyHop = 1;
                if (Request["iTruocKyHop"] != null)
                {
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);
                }

                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop(iTruocKyHop);
                //nguoinkiennghi
                string lstNguonKN = "all";
                if (Request["iTruocKyHop"] != null) lstNguonKN = "";
                if (Request["listNguonKienNghi"] != null)
                {
                    lstNguonKN = Request["listNguonKienNghi"];
                }
                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, lstNguonKN);
                //linh vuc
                string lstLinhVuc = "all";
                if (Request["iTruocKyHop"] != null) lstLinhVuc = "";
                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu(lstLinhVuc);

                //donvitiepnhan
                int iDonViTiepNhan = 0;
                if (Request["iDonViTiepNhan"] != null)
                {
                    iDonViTiepNhan = Convert.ToInt32(Request["iDonViTiepNhan"]);
                }
                ViewData["opt-donvitiepnhan"] = Get_Option_DonViTiepNhan(u_info, iDonViTiepNhan);

                ViewData["opt-tenbaocao"] = Get_Option_BaoCao_By_ID_USERS();
                //noi dung 
                string cnoidung = Request["cNoiDungKN"];
                ViewData["NoiDung"] = " value='" + cnoidung + "' ";

                //end lĩnh vực

                return PartialView("../Ajax/Kiennghi/search_tonghop_bandannguyen");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form Tìm Tập hợp kiến nghị từ Đại biểu HĐND");
                throw;
            }

        }

        public ActionResult Ajax_search_tonghop_tinh()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iKyHop = ID_KyHop_HienTai();
                //noi dung 
                string cnoidung = Request["cNoiDungKN"];
                ViewData["NoiDung"] = " value='" + cnoidung + "' ";
                //ma kien nghi
                string imakiennghi = "";
                if (Request["cMaKienNghi"] != "" && Request["cMaKienNghi"] != null)
                {
                    imakiennghi = Request["cMaKienNghi"];
                }
                ViewData["MaKienNghi"] = "<input type='text' class='input-block-level' id='cMaKienNghi' name='cMaKienNghi' value='" + imakiennghi + "' />";
                // ki hop
                string listKyHop = "";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                ViewData["kyhop"] = List_CheckBox_KyHop(iKyHop, listKyHop);
                // thoi gian
                string dtungay = "";
                string ddenngay = "";
                if (Request["dBatDau"] != null && Request["dBatDau"] != "")
                {
                    dtungay = Request["dBatDau"];
                }
                ViewData["TuNgay"] = " value='" + dtungay + "' ";
                if (Request["dKetThuc"] != null && Request["dKetThuc"] != "")
                {
                    ddenngay = Request["dKetThuc"];
                }
                ViewData["DenNgay"] = " value='" + ddenngay + "' ";
                //don vi tham quyen
                int iDonViXuLy = 0;
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }

                    ViewData["opt-donvixuly"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Tinh,iDonViXuLy);  
                //truockihop
                int iTruocKyHop = 1;
                if (Request["iTruocKyHop"] != null)
                {
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);
                }

                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop(iTruocKyHop);
                //huyen hti xa thanh pho
                string huyenthixathanhpho = "";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    huyenthixathanhpho = Request["listHuyen_Xa_ThanhPho"];
                }
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho(lstKN : huyenthixathanhpho);
                //nguon kien nghi
                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                string lstNguonKN = "all";
                if (Request["iTruocKyHop"] != null) lstNguonKN = "";
                if (Request["listNguonKienNghi"] != null)
                {
                    lstNguonKN = Request["listNguonKienNghi"];
                }
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, lstNguonKN);
                //linh vuc
                string lstLinhVuc = "all";
                if (Request["iTruocKyHop"] != null) lstLinhVuc = "";
                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu(lstLinhVuc);
                //donvitiepnhan
                int iDonViTiepNhan = 0;
                if (Request["iDonViTiepNhan"] != null)
                {
                    iDonViTiepNhan = Convert.ToInt32(Request["iDonViTiepNhan"]);
                }
                ViewData["opt-donvitiepnhan"] = Get_Option_DonViTiepNhan(u_info, iDonViTiepNhan);

                ViewData["opt-tenbaocao"] = Get_Option_TenBaoCao_Tonghop_Tinh(u_info);
                return PartialView("../Ajax/Kiennghi/search_tonghop_tinh");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form Tìm Tập hợp kiến nghị từ Đại biểu HĐND");
                throw;
            }

        }

        private string Get_Option_TenBaoCao_Tonghop_Tinh(UserInfor u_info, int iTenBC = 0)
        {
            StringBuilder sb = new StringBuilder();
            if ((int)u_info.user_login.ITYPE == (int)UserType.Admin || (int)u_info.user_login.ITYPE == (int)UserType.LanhDao)
            {
                sb.AppendLine("<option value='0' " + (iTenBC == 0 ? "selected" : "") + ">PHỤ LỤC - HĐND: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp Tỉnh</option>");
                sb.AppendLine("<option value='1' " + (iTenBC == 1 ? "selected" : "") + ">PHỤ LỤC - QH: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp Tỉnh</option>");
                sb.AppendLine("<option value='2' " + (iTenBC == 2 ? "selected" : "") + ">PHỤ LỤC: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp Tỉnh</option>");
            }
            else if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
            {
                sb.AppendLine("<option value='0' " + (iTenBC == 0 ? "selected" : "") + ">PHỤ LỤC - HĐND: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp Tỉnh</option>");
            }
            else if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
            {
                sb.AppendLine("<option value='1' " + (iTenBC == 1 ? "selected" : "") + ">PHỤ LỤC - QH: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp Tỉnh</option>");
            }
            return sb.ToString();
        }

        public ActionResult Ajax_search_tonghop_Huyen()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int iKyHop = ID_KyHop_HienTai();
                //noi dung 
                string cnoidung = Request["cNoiDungKN"];
                ViewData["NoiDung"] = " value='" + cnoidung + "' ";
                //ma kien nghi
                string imakiennghi = "";
                if (Request["cMaKienNghi"] != "" && Request["cMaKienNghi"] != null)
                {
                    imakiennghi = Request["cMaKienNghi"];
                }
                ViewData["MaKienNghi"] = "<input type='text' class='input-block-level' id='cMaKienNghi' name='cMaKienNghi' value='" + imakiennghi + "' />";
                // ki hop
                string listKyHop = "";
                if (Request["listKyHop"] != null)
                {
                    //Trong trường hợp có tìm kiếm theo kỳ họp
                    listKyHop = Request["listKyHop"];
                }
                ViewData["kyhop"] = List_CheckBox_KyHop(iKyHop, listKyHop);
                // thoi gian
                string dtungay = "";
                string ddenngay = "";
                if (Request["dBatDau"] != null && Request["dBatDau"] != "")
                {
                    dtungay = Request["dBatDau"];
                }
                ViewData["TuNgay"] = " value='" + dtungay + "' ";
                if (Request["dKetThuc"] != null && Request["dKetThuc"] != "")
                {
                    ddenngay = Request["dKetThuc"];
                }
                ViewData["DenNgay"] = " value='" + ddenngay + "' ";
                //don vi tham quyen
                int iDonViXuLy = 0;
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }

                ViewData["opt-donvixuly"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Huyen, iDonViXuLy);
                //truockihop
                int iTruocKyHop = 1;
                if (Request["iTruocKyHop"] != null)
                {
                    iTruocKyHop = Convert.ToInt32(Request["iTruocKyHop"]);  
                }

                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop(iTruocKyHop);
                //huyen thi xa thanh pho
                string huyenthixathanhpho = "";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    huyenthixathanhpho = Request["listHuyen_Xa_ThanhPho"];
                }
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho(lstKN: huyenthixathanhpho);
                // nguon kien nghi
                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                string lstNguonKN = "all";
                if (Request["iTruocKyHop"] != null) lstNguonKN = "";
                if (Request["listNguonKienNghi"] != null)
                {
                    lstNguonKN = Request["listNguonKienNghi"];
                }
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, lstNguonKN);
                //linh vuc
                string lstLinhVuc = "all";
                if (Request["iTruocKyHop"] != null) lstLinhVuc = "";
                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu(lstLinhVuc);
                //donvitiepnhan
                int iDonViTiepNhan = 0;
                if (Request["iDonViTiepNhan"] != null)
                {
                    iDonViTiepNhan = Convert.ToInt32(Request["iDonViTiepNhan"]);
                }
                ViewData["opt-donvitiepnhan"] = Get_Option_DonViTiepNhan(u_info, iDonViTiepNhan);
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_tonghop_huyen");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form Tìm Tập hợp kiến nghị từ Đại biểu HĐND");
                throw;
            }
        }
        public ActionResult Ajax_search_tonghop_bandannguyen_dachuyen()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                //linh vực            
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_tonghop_bandannguyen_dachuyen");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form Tìm Tập hợp kiến nghị Ban Dân nguyện đã chuyển");
                throw;
            }

        }
        public ActionResult Ajax_search_tonghop_bandannguyen_chuyen()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                //ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                ViewData["check-kyhop"] = Get_CheckBox_TruocKyHop();
                //linh vực            
                //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_tonghop_bandannguyen_chuyen");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form Tìm Tập hợp kiến nghị Ủy ban nhân dân tỉnh chuyển đến");
                throw;
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_tonghop_bandannguyen_chuyen_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_TONGHOP tonghop = new KN_TONGHOP();
        //        tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
        //        if (fc["q"] != null && fc["q"] != "")
        //        {
        //            tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
        //            //tonghop.IKYHOP = ID_KyHop_HienTai();
        //        }
        //        if (fc["cNoiDung"] != null && fc["cNoiDung"]!="") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        //if (fc["iDoan"] != null && Convert.ToInt32(fc["iDoan"])!=0) { tonghop.IDONVITONGHOP = Convert.ToInt32(fc["iDoan"]); }
        //        //if (fc["iDonViXuLy"] != null && Convert.ToInt32(fc["iDonViXuLy"]) != -1) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
        //        //if (fc["iLinhVuc"] != null && Convert.ToInt32(fc["iLinhVuc"]) != -1) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
        //        tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen;
        //        var th = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(tonghop);
        //        if (th.Count() > 0)
        //        {
        //            Response.Write("<tr><td colspan='3' class='alert-info tcenter'>Có " + th.Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        Response.Write(kn.KN_Tonghop_bandannguyen_chuyen(th, GetUserInfor().tk_action));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Form Tìm Tập hợp Ban Dân nguyện đã chuyển");
        //        throw;
        //    }

        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_tonghop_bandannguyen_dachuyen_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        UserInfor u_info = GetUserInfor();
        //        KN_TONGHOP tonghop = new KN_TONGHOP();
        //        tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
        //        if (fc["q"] != null && fc["q"]!="") {
        //            tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
        //            //tonghop.IKYHOP = ID_KyHop_HienTai();
        //        }
        //        if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        //if (fc["iDoan"] != null && Convert.ToInt32(fc["iDoan"])!=0) { tonghop.IDONVITONGHOP = Convert.ToInt32(fc["iDoan"]); }
        //        if (fc["iDonViXuLy"] != null && Convert.ToInt32(fc["iDonViXuLy"]) != -1) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
        //        if (fc["iLinhVuc"] != null && Convert.ToInt32(fc["iLinhVuc"]) != -1) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }
        //        var th = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(tonghop, 0);
        //        //var th = _kiennghi.Search_Tonghop(tonghop).Where(x => x.ITINHTRANG > (decimal)TrangThai_TongHop.DaChuyenBanDanNguyen).ToList();

        //        if (th.Count() > 0)
        //        {
        //            Response.Write("<tr><td colspan='3' class='alert-info tcenter'>Có " + th.GroupBy(x=>x.ITONGHOP).Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        Response.Write(kn.KN_Tonghop_Bandannguyen_dachuyen(th, u_info.tk_action));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Form Tìm Tập hợp Ban Dân nguyện đã chuyển");
        //        throw;
        //    }

        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_tonghop_bandannguyen_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_TONGHOP tonghop = new KN_TONGHOP();
        //        UserInfor u_info = GetUserInfor();
        //        tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.ChoXuLy;//chuyển ban dân nguyện
        //        tonghop.IDONVITONGHOP = ID_Ban_DanNguyen;
        //        if (fc["q"] != null && fc["q"]!="")
        //        {
        //            tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
        //            //tonghop.IKYHOP = ID_KyHop_HienTai();
        //        }
        //        if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        if (fc["iDoan"] != null) { tonghop.IDONVITONGHOP = Convert.ToInt32(fc["iDoan"]); }
        //        if (fc["iDonViXuLy"] != null) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
        //        if (fc["iLinhVuc"] != null) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }

        //        var th = _kiennghi.List_PRC_TONGHOP_KIENNGHI(tonghop);
        //        Response.Write(kn.KN_Tonghop_Bandannguyen(th, u_info));


        //        if (th.Count() > 0)
        //        {
        //            Response.Write("<tr><td colspan='5' class='alert-info tcenter'>Có " + th.Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        //Response.Write(kn.KN_Tonghop_Bandannguyen(th, GetUserInfor().tk_action));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Form Tìm Tập hợp kiến nghị chuyển Ban Dân nguyện");
        //        throw;
        //    }

        //}
        public ActionResult Ajax_search_tonghop_diaphuongchuyen()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();
                ViewData["opt-doan"] = Get_Option_DonViTiepNhan();
                //linh vực            
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                //end lĩnh vực
                return PartialView("../Ajax/Kiennghi/search_tonghop_diaphuongchuyen");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form Tìm Tập hợp kiến nghị chuyển địa phương giải quyết");
                throw;
            }

        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Ajax_search_tonghop_diaphuongchuyen_result(FormCollection fc)
        //{
        //    if (!CheckAuthToken()) { return null; }
        //    try
        //    {
        //        KN_TONGHOP tonghop = new KN_TONGHOP();
        //        tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DaChuyenBanDanNguyen;//chuyển ban dân nguyện
        //        if (fc["q"] != null && fc["q"]!="") {
        //            tonghop.CNOIDUNG = func.RemoveTagInput(fc["q"]);
        //            //tonghop.IKYHOP = ID_KyHop_HienTai();
        //        }
        //        if (fc["cNoiDung"] != null && fc["cNoiDung"] != "") { tonghop.CNOIDUNG = func.RemoveTagInput(fc["cNoiDung"]); }
        //        if (fc["iKyHop"] != null) { tonghop.IKYHOP = Convert.ToInt32(fc["iKyHop"]); }
        //        if (fc["iTruocKyHop"] != null) { tonghop.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]); }
        //        if (fc["iDoan"] != null && Convert.ToInt32(fc["iDoan"]) != 0) { tonghop.IDONVITONGHOP = Convert.ToInt32(fc["iDoan"]); }
        //        if (fc["iDonViXuLy"] != null && Convert.ToInt32(fc["iDonViXuLy"]) != -1) { tonghop.ITHAMQUYENDONVI = Convert.ToInt32(fc["iDonViXuLy"]); }
        //        if (fc["iLinhVuc"] != null && Convert.ToInt32(fc["iLinhVuc"]) != -1) { tonghop.ILINHVUC = Convert.ToInt32(fc["iLinhVuc"]); }

        //        var th = _kiennghi.Search_Tonghop(tonghop);

        //        List<QUOCHOI_COQUAN> coquan;
        //        Dictionary<string, object> donvi = new Dictionary<string, object>();
        //        donvi.Add("IPARENT", ID_Coquan_doandaibieu);
        //        coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
        //        if (th.Count() > 0)
        //        {
        //            Response.Write("<tr><td colspan='5' class='alert-info tcenter'>Có " + th.Count() + " kết quả tìm kiếm.</td></tr>");
        //        }
        //        Response.Write(kn.KN_Tonghop_ChuyenXuLy(th, coquan, GetUserInfor().tk_action));
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Log_Error(ex, "Form Tìm Tập hợp địa phương chuyển");
        //        throw;
        //    }

        //}
        public string Get_Option_KyHop(int iKyHop = 0, int? iDoiTuongGui = null)
        {
            List<QUOCHOI_KHOA> khoa = _kiennghi.GetAll_KhoaHop();
            if (iDoiTuongGui.HasValue)
            {
                khoa = khoa.Where(x => x.ILOAI == iDoiTuongGui.Value).ToList();
            }
            khoa = khoa.OrderBy(x => x.DBATDAU).ToList();

            var listKhoa = khoa.Select(x => x.IKHOA);
            List<QUOCHOI_KYHOP> kyhop = _kiennghi.GetAll_KyHop().Where(x => x.IKHOA.HasValue && listKhoa.Contains(x.IKHOA.Value)).OrderBy(x => x.DBATDAU).ToList();
            return "<option value='0'>Chọn kỳ họp</option>" + kn.Option_Khoa_KyHop(khoa, kyhop, iKyHop);
        }
        public string Get_Option_LoaiDoiTuong(int val = -1)
        {
            string str = "";
            UserInfor u_info = GetUserInfor();
            if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
            {
                str = "<option selected value='0'>" + StringEnum.GetStringValue(Loai_DoiTuong.DBQH) + "</option>";
            }
            else if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
            {
                str = "<option selected value='1'>" + StringEnum.GetStringValue(Loai_DoiTuong.HDND) + "</option>";
            }
            else if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
            {
                if (val == 0)
                {
                    str = "<option selected value='0'>" + StringEnum.GetStringValue(Loai_DoiTuong.DBQH) + "</option><option value='1'>" + StringEnum.GetStringValue(Loai_DoiTuong.HDND) + "</option>";
                }
                else
                {
                    str = "<option value='0'>" + StringEnum.GetStringValue(Loai_DoiTuong.DBQH) + "</option><option selected value='1'>" + StringEnum.GetStringValue(Loai_DoiTuong.HDND) + "</option>";
                }
            }

            return str;

        }
        public string Get_ChuongTrinh(int iKyHop = 0, int iChuongTrinh = 0)
        {
            List<QUOCHOI_KYHOP> listKyHop = new List<QUOCHOI_KYHOP>();
            if (iKyHop == 0)
            {
                listKyHop = _kiennghi.GetAll_KyHop();
            }
            else
            {
                listKyHop.Add(_kiennghi.Get_KyHop_QuocHoi(iKyHop));
            }
            List<KN_CHUONGTRINH> listAllChuongTrinh = _kiennghi.GetAll_ChuongTrinh(0, iKyHop);
            StringBuilder str = new StringBuilder("<option value='0'>Chọn chương trình</option>");
            foreach (var kyhop in listKyHop)
            {
                var listChuongTrinh = listAllChuongTrinh.Where(x => x.IKYHOP == kyhop.IKYHOP);
                if (listChuongTrinh != null && listChuongTrinh.Count() > 0)
                {
                    str.Append("<optgroup label='" + kyhop.CTEN + "'>");
                    foreach (var chuongtrinh in listChuongTrinh)
                    {
                        if (chuongtrinh.ICHUONGTRINH == iChuongTrinh)
                        {
                            str.Append("<option value='" + chuongtrinh.ICHUONGTRINH + "' selected>" + chuongtrinh.CKEHOACH + "</option>");
                        }
                        else
                        {
                            str.Append("<option value='" + chuongtrinh.ICHUONGTRINH + "'>" + chuongtrinh.CKEHOACH + "</option>");
                        }
                    }
                    str.Append("</optgroup>");
                }
            }
            return str.ToString();
        }

        public string Get_Option_DoanDaiBieu(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            UserInfor u_info = GetUserInfor();
            string opt = "";
            if (u_info.tk_action.is_lanhdao)
            {
                //donvi.Add("IPARENT", ID_Coquan_doandaibieu);
                coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
                opt = "<option value='0'>Chọn Đoàn ĐBQH</option>" + kn.OptionCoQuanXuLy(coquan, ID_Coquan_doandaibieu, 0, iDonVi, 0);
            }
            else
            {
                opt = "<option value='" + u_info.user_login.IDONVI + "'>" + Server.HtmlEncode(u_info.tk_action.tendonvi) + "</option>";
            }

            return opt;
        }
        public string Get_Option_DonViXuLyKienNghi(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            UserInfor u_info = GetUserInfor();

            string opt = "";
            if (u_info.tk_action.is_lanhdao)
            {
                Dictionary<string, object> donvi = new Dictionary<string, object>();
                coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
                opt = "<option value='0'>Chọn đơn vị xử lý</option>" + kn.OptionCoQuanXuLy(coquan, 0, 0, iDonVi, 0);
            }
            else
            {
                opt = "<option value='" + u_info.user_login.IDONVI + "'>" + Server.HtmlEncode(u_info.tk_action.tendonvi) + "</option>";
            }
            return opt;
        }
        public string Get_Option_DonViThamQuyen(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            return "<option value='0'>Chọn đơn vị thẩm quyền</option>" + kn.OptionCoQuanXuLy(coquan, 0, 0, iDonVi, 0);
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
            {
                return "<option value='0'>Chọn đơn vị thẩm quyền</option>" + kn.Option_CoquanDiaPhuong(coquan, iDonVi, iType);
            }
            else
            {
                return "<option value='0'>Chọn đơn vị thẩm quyền</option>" + kn.Option_CoquanDiaPhuong(coquan, iDonVi, iType);
            }
        }
        public string Get_Option_DonViThamQuyen_WithParent(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            return "<option value='0'>Chọn đơn vị thẩm quyền</option>" +
                    kn.OptionCoQuanXuLy_WithParent(coquan, 0, 0, iDonVi, 0);
        }
        public string Get_Option_DonViTiepNhanKienNghi_ForCQTW(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            UserInfor u_info = GetUserInfor();
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            donvi.Add("IPARENT", ID_Coquan_doandaibieu);
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            string opt = "<option value='0'>Chọn đơn vị tiếp nhận</option>";
            if (iDonVi == ID_Ban_DanNguyen)
            {
                opt += "<option selected value='" + ID_Ban_DanNguyen + "'>Ban Dân Nguyện</option>";
            }
            else
            {
                opt += "<option value='" + ID_Ban_DanNguyen + "'>Ban Dân Nguyện</option>";
            }
            opt += kn.OptionCoQuanXuLy(coquan, ID_Coquan_doandaibieu, 0, iDonVi, 0);

            return opt;
        }
        public string get_Option_TaiKhoan_CapNhat(int id_user, List<PRC_LIST_USER_CAPNHAT> user)
        {
            string str = "<option value='0'>- - - Chọn tất cả</option>";
            foreach (var u in user)
            {
                string select = ""; if (u.USER_ID == id_user) { select = "selected"; }
                str += "<option " + select + " value='" + u.USER_ID + "'>" + u.USER_CAPNHAT + "</option>";
            }
            return str;
        }
        public string Get_Option_DonViTiepNhan(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            UserInfor u_info = GetUserInfor();
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            donvi.Add("IPARENT", ID_Coquan_doandaibieu);
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            return kn.OptionCoQuanXuLy(coquan, ID_Coquan_doandaibieu, 0, iDonVi, 0);
        }
        // Hàm cũ (Get_Option_DonViTiepNhan) ở trên đang lấy theo IPARENT, khác với logic hiện tại
        public string Get_Option_DonViTiepNhan_By_ID(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            UserInfor u_info = GetUserInfor();
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            return kn.OptionCoQuanXuLy(coquan, 0, 0, iDonVi, 0);
        }

        //Cố định 2 lựa chọn Đoàn ĐBQH và HĐND Tỉnh Thanh Hoá. Lựa chọn mặc định phụ thuộc vào quyền của người dùng
        public string Get_Option_DonViLap(int iDonVi = 0)
        {
            UserInfor u_info = GetUserInfor();
            string str = "";
            var HDND_Tinh = _thietlap.Get_Quochoi_Coquan(ID_HDND_Tinh);
            var DDBQH_THA = _thietlap.Get_Quochoi_Coquan(ID_DDBQH_THA);
            str += "<option value='" + HDND_Tinh.ICOQUAN + "'>" + HDND_Tinh.CTEN + "</option>";
            str += "<option ";
            if (iDonVi == 0)
            {
                if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                    str += "selected";
            }

            else
            {
                if (iDonVi == ID_DDBQH_THA)
                    str += "selected";
            }

            str += " value='" + DDBQH_THA.ICOQUAN + "'>" + DDBQH_THA.CTEN + "</option>";

            return str;
        }

        public string Get_Option_DonViLap_PhanQuyen(int iDonVi = 0)
        {
            UserInfor u_info = GetUserInfor();
            string str = "";
            var HDND_Tinh = _thietlap.Get_Quochoi_Coquan(ID_HDND_Tinh);
            var DDBQH_THA = _thietlap.Get_Quochoi_Coquan(ID_DDBQH_THA);
            if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
            {
                str += "<option value='" + DDBQH_THA.ICOQUAN + "'>" + DDBQH_THA.CTEN + "</option>";
            }
            else if (u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
            {
                str += "<option value='" + HDND_Tinh.ICOQUAN + "'>" + HDND_Tinh.CTEN + "</option>";
            }
            else if (u_info.user_login.ITYPE == (int)UserType.LanhDao || u_info.user_login.ITYPE == (int)UserType.Admin)
            {
                if (iDonVi == ID_DDBQH_THA)
                {
                    str += "<option value='" + HDND_Tinh.ICOQUAN + "'>" + HDND_Tinh.CTEN + "</option>";
                    str += "<option selected value='" + DDBQH_THA.ICOQUAN + "'>" + DDBQH_THA.CTEN + "</option>";
                }
                else
                {
                    str += "<option selected value='" + HDND_Tinh.ICOQUAN + "'>" + HDND_Tinh.CTEN + "</option>";
                    str += "<option value='" + DDBQH_THA.ICOQUAN + "'>" + DDBQH_THA.CTEN + "</option>";
                }




            }
            return str;
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
                str = str + "<option "+ select+ " value ='" + item.ICOQUAN + "'>" + "- - -" + item.CTEN + "</option>";
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

        public string Get_Option_Coquanxuly(int iDonVi = 0)
        {
            List<QUOCHOI_COQUAN> coquan;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
            return kn.OptionCoQuanXuLy(coquan, 0, 0, iDonVi, 0);
        }

        public string Get_Option_DonViTiepNhan(UserInfor u_info, int iQuocHoiCoQuan = 0)
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> param = new Dictionary<string, object>();
            List<QUOCHOI_COQUAN> coquan = _kiennghi.HienThiDanhSachCoQuan(param);
            QUOCHOI_COQUAN coquanHDND = coquan.Where(x => x.ICOQUAN == AppConfig.IQUOCHOI_COQUAN_HDNDTINH).FirstOrDefault();
            QUOCHOI_COQUAN coquanDBQH = coquan.Where(x => x.ICOQUAN == AppConfig.IQUOCHOI_COQUAN_DOANDBQH).FirstOrDefault();
            if ((int)u_info.user_login.ITYPE == (int)UserType.Admin || (int)u_info.user_login.ITYPE == (int)UserType.LanhDao)
            {
                sb.AppendLine("<option value='0' " + (iQuocHoiCoQuan == 0 ? "selected" : "") + ">Tất cả<option>");
                if (coquanHDND != null)
                {
                    sb.AppendLine("<option value='" + coquanHDND.ICOQUAN + "' " + (iQuocHoiCoQuan == coquanHDND.ICOQUAN ? "selected" : "") + ">" + HttpUtility.HtmlEncode(coquanHDND.CTEN) + "<option>");
                }
                if (coquanDBQH != null)
                {
                    sb.AppendLine("<option value='" + coquanDBQH.ICOQUAN + "' " + (iQuocHoiCoQuan == coquanDBQH.ICOQUAN ? "selected" : "") + ">" + HttpUtility.HtmlEncode(coquanDBQH.CTEN) + "<option>");
                }
            }
            else if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
            {
                if (coquanHDND != null)
                {
                    sb.AppendLine("<option value='" + coquanHDND.ICOQUAN + "' " + "selected" + ">" + HttpUtility.HtmlEncode(coquanHDND.CTEN) + "<option>");
                }
            }
            else if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
            {
                if (coquanDBQH != null)
                {
                    sb.AppendLine("<option value='" + coquanDBQH.ICOQUAN + "' " + "selected" + ">" + HttpUtility.HtmlEncode(coquanDBQH.CTEN) + "<option>");
                }
            }
            return sb.ToString();
        }

        public string Get_Option_LinhVuc_By_ID_CoQuan(int id_coquan = 0, int id_linhvuc = 0)
        {
            string str = "<option value='0'>Chưa xác định</option>";
            //var linhvuc = _kiennghi.GetAll_LinhVuc_CoQuan_By_IDCoQuan();
            var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            return str + kn.Option_LinhVuc_CoQuan(linhvuc, id_linhvuc);
        }
        public string Get_Option_BaoCao_By_ID_USERS()
        {
            string str = "";
            //var linhvuc = _kiennghi.GetAll_LinhVuc_CoQuan_By_IDCoQuan();
            //var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            UserInfor u_info = GetUserInfor();
            if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
            {
                str += "<option value='3'>PHỤ LỤC II - HĐND: Danh mục kiến nghị của cử tri thuộc thẩm quyền Bộ, Ngành, Trung ương </option>";
            }
            else if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
            {
                str += "<option value='4'>PHỤ LỤC II - QH:  Danh mục kiến nghị của cử tri thuộc thẩm quyền Bộ, Ngành, Trung ương </option>";
            }
            else
            {
                str += "<option value='2'>PHỤ LỤC II :  Danh mục kiến nghị của cử tri thuộc thẩm quyền Bộ, Ngành, Trung ương </option>";
                str += "<option value='3'>PHỤ LỤC II - HĐND: Danh mục kiến nghị của cử tri thuộc thẩm quyền Bộ, Ngành, Trung ương </option>";
                str += "<option value='4'>PHỤ LỤC II - QH:  Danh mục kiến nghị của cử tri thuộc thẩm quyền Bộ, Ngành, Trung ương </option>";
            }
            return str;
        }
        public string Get_Option_LinhVuc_ChonNhieu(string lstLV = "")
        {
            var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            ViewData["data-linhvuc"] = string.Join(",", linhvuc.Select(x => x.IPARENT.ToString()).ToArray());
            return kn.Option_Linhvucconquan_Chonnhieu(linhvuc, lstLV);
        }

        public string Get_Option_LinhVuc_CoQuan_Optgroup(int id_linhvuc = 0)
        {
            string str = "<option value='0'>Chưa xác định</option>";
            var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            return str + kn.Option_LinhVuc_CoQuan_Optgroup(linhvuc, id_linhvuc);
        }

        public string GetAll_Option_LinhVucSorted(int id_coquan = 0, int id_linhvuc = 0)
        {
            string str = "<option value='0'>Chưa xác định</option>";
            UserInfor u_info = GetUserInfor();
            int iUser = u_info.tk_action.iUser;
            List<LINHVUC_COQUAN> linhvuc_coquan = _thietlap.Get_Linhvuc_Coquan_Sorted();
            List<QUOCHOI_COQUAN> coquan = _thietlap.Get_List_Quochoi_Coquan();
            str += tl.Option_linhvuccoquan(linhvuc_coquan, coquan, iUser);
            return str;
        }
        public string Get_Option_Linhvuc(int iLinhVuc = 0)
        {
            List<LINHVUC> linhvuc = _kiennghi.GetAll_LinhVuc();
            return kn.Option_LinhVuc(linhvuc, 0, 0, iLinhVuc);
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

        public ActionResult Ajax_Change_LinhVuc_By_ID_DonVi_ChonNhieu(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id_donvi = Convert.ToInt32(fc["id"]);
                Response.Write(Get_Option_LinhVuc_ChonNhieu());
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Lấy Option lĩnh vực của Đơn vị tương ứng");
                throw;
            }
        }

        public ActionResult Tonghop_kiennghi_doan(FormCollection fc)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {

                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                ViewData["tonghop"] = tonghop;
                ViewData["detail"] = kn.Tonghop_Detail(id, Request.Cookies["url_key"].Value);
                ViewData["file"] = kn.File_View(id, "kn_kiennghi");
                Dictionary<string, object> kiennghi_den = new Dictionary<string, object>();
                kiennghi_den.Add("ITONGHOP", id);
                var kiennghi = _kiennghi.HienThiDanhSachKienNghi(kiennghi_den);
                ViewData["list"] = kn.KN_Kiennghi_TongHop_ChuyenBDN(kiennghi, u_info.tk_action, Request.Cookies["url_key"].Value);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách kiến nghị thuộc Tập hợp kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tonghop_diaphuongchuyen()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
            
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                if (!base_business.ActionMulty_Redirect_("5,6", u_info.tk_action)) { Response.Redirect("/Home/Error/"); return null; }

                if (!base_business.Action_(5, u_info.tk_action))//chọn Tập hợp
                {
                    ViewData["bt-add"] = " style='display:none'";
                }
                int iDonViXuLy_Parent = 0; if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                int iDonViXuLy = 0; if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }

                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    //đơn vị thẩm quyền ko phải là con của iDonViXuLy_Parent
                    if (iDonViXuLy != 0 && coquan.Where(x => x.ICOQUAN == iDonViXuLy).Count() == 0)
                    {
                        iDonViXuLy = 0;
                    }
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, iDonViXuLy);
                }
                else
                {
                    //coquan = null;
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(null, iDonViXuLy);
                }

                int iDonViTiepNhan = 0;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iDoan"] != null) { iDonViTiepNhan = Convert.ToInt32(Request["iDoan"]); }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                KN_TONGHOP kn_tonghop = new KN_TONGHOP();
                kn_tonghop.IKYHOP = iKyHop;
                kn_tonghop.IDONVITONGHOP = iDonViTiepNhan; ;
                kn_tonghop.ITINHTRANG = (decimal)TrangThai_TongHop.DaChuyenBanDanNguyen;
                kn_tonghop.ITHAMQUYENDONVI = iDonViXuLy;
                //var tonghop = _kiennghi.List_PRC_TONGHOP_KIENNGHI(kn_tonghop, iDonViXuLy_Parent,page,post_per_page);
                var tonghop = _kiennghi.PRC_LIST_TONGHOP_KIENNGHI(kn_tonghop, iDonViXuLy_Parent, 2, 2, -1, -1, -1, -1, page, post_per_page);
                if (tonghop.Count() > 0)
                {
                    ViewData["list"] = kn.KN_Tonghop_Chuyen_BanDanNguyen(tonghop);
                    ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)tonghop.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }

                //ViewData["list"] = kn.KN_Tonghop_Doan_Chuyen_BDN(tonghop, u_info.tk_action);           
                ViewData["opt-coquan"] = Get_Option_DoanDaiBieu(iDonViTiepNhan);


                return View();

            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách Tập hợp kiến nghị địa phương chuyển");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Tracuu()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                UserInfor u_info = GetUserInfor();

                // tìm kiếm
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int iKyHop = ID_KyHop_HienTai();
                int iThamQuyenDonVi = 0;
                int iDonViTiepNhan = 0;
                int iLinhVuc = -1;
                int iTinhTrang = -1;
                int iUser_Capnhat = u_info.tk_action.iUser;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser_Capnhat = 0;
                }

                if (true)
                {
                    int post_per_page = pageSize;
                    if (Request["post_per_page"] != null)
                    {
                        post_per_page = Convert.ToInt32(Request["post_per_page"]);
                    }

                    int page = 1;
                    if (Request["page"] != null)
                    {
                        page = Convert.ToInt32(Request["page"]);
                    }

                    if (Request["iTinhTrang"] != null)
                    {
                        iTinhTrang = Convert.ToInt32(Request["iTinhTrang"]);
                    }

                    KN_KIENNGHI kiennghi = new KN_KIENNGHI();
                    var fc = this.Request;
                    string cNoiDung = func.RemoveTagInput(fc["cNoiDung"]);
                    //ViewData["cNoiDung"] = kn.EncodeOutput(fc["cNoiDung"]);
                    kiennghi.CNOIDUNG = cNoiDung;
                    int iTruocKyHop = -1;
                    if (fc["iTruocKyHop"] != null)
                    {
                        iTruocKyHop = Convert.ToInt32(fc["iTruocKyHop"]);
                    }

                    kiennghi.ITRUOCKYHOP = iTruocKyHop;
                    if (fc["iKyHop"] != null)
                    {
                        iKyHop = Convert.ToInt32(fc["iKyHop"]);
                    }

                    kiennghi.IKYHOP = iKyHop;
                    if (fc["iDonViTiepNhan"] != null)
                    {
                        iDonViTiepNhan = Convert.ToInt32(fc["iDonViTiepNhan"]);
                    }

                    kiennghi.IDONVITIEPNHAN = iDonViTiepNhan;
                    if (fc["iThamQuyenDonVi"] != null)
                    {
                        iThamQuyenDonVi = Convert.ToInt32(fc["iThamQuyenDonVi"]);
                    }

                    kiennghi.ITHAMQUYENDONVI = iThamQuyenDonVi;
                    if (fc["iLinhVuc"] != null)
                    {
                        iLinhVuc = Convert.ToInt32(fc["iLinhVuc"]);
                    }

                    kiennghi.ILINHVUC = iLinhVuc;
                    //if (fc["iUser_Capnhat"] != null) { iUser_Capnhat = Convert.ToInt32(fc["iUser_Capnhat"]); }
                    kiennghi.IUSER = iUser_Capnhat;
                    kiennghi.ITINHTRANG = iTinhTrang;
                    DateTime dNgayNhan_from = DateTime.MinValue;
                    if (fc["dNgayNhan_from"] != null && fc["dNgayNhan_from"] != "")
                    {
                        //ViewData["dNgayNhan_from"] = fc["dNgayNhan_from"];
                        dNgayNhan_from = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayNhan_from"]));
                    }

                    DateTime dNgayNhan_to = DateTime.MaxValue;
                    if (fc["dNgayNhan_to"] != null && fc["dNgayNhan_to"] != "")
                    {
                        //ViewData["dNgayNhan_to"] = fc["dNgayNhan_to"];
                        dNgayNhan_to = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayNhan_to"]));
                    }

                    DateTime dNgayTongHop_from = DateTime.MinValue;
                    if (fc["dNgayTongHop_from"] != null && fc["dNgayTongHop_from"] != "")
                    {
                        //ViewData["dNgayTongHop_from"] = fc["dNgayTongHop_from"];
                        dNgayTongHop_from = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTongHop_from"]));
                    }

                    DateTime dNgayTongHop_to = DateTime.MaxValue;
                    if (fc["dNgayTongHop_to"] != null && fc["dNgayTongHop_to"] != "")
                    {
                        //ViewData["dNgayTongHop_to"] = fc["dNgayTongHop_to"];
                        dNgayTongHop_to = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTongHop_to"]));
                    }

                    DateTime? dNgayTraLoi_from = null;
                    if (fc["dNgayTraLoi_from"] != null && fc["dNgayTraLoi_from"] != "")
                    {
                        //ViewData["dNgayTraLoi_from"] = fc["dNgayTraLoi_from"];
                        dNgayTraLoi_from = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTraLoi_from"]));
                    }

                    DateTime? dNgayTraLoi_to = null;
                    if (fc["dNgayTraLoi_to"] != null && fc["dNgayTraLoi_to"] != "")
                    {
                        //ViewData["dNgayTraLoi_to"] = fc["dNgayTraLoi_to"];
                        dNgayTraLoi_to = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTraLoi_to"]));
                    }

                    int tinhtrang_from = Convert.ToInt32(TrangThaiKienNghi.Moicapnhat);
                    int tinhtrang_to = Convert.ToInt32(TrangThaiKienNghi.TamxoaKiennghi);
                    int ketqua_giamsat = -1;

                    int datraloi = -1; //tất cả; 0: chưa trả lời; 1 đã trả lời
                    string maKienNghi = "";
                    if (Request["cMaKienNghi"] != null)
                    {
                        maKienNghi = Request["cMaKienNghi"].ToString();
                    }

                    string listNguonKienNghi = "";
                    if (Request["listNguonKienNghi"] != null)
                    {
                        //Trong trường hợp có tìm kiếm theo kỳ họp
                        listNguonKienNghi = Request["listNguonKienNghi"];
                    }

                    string cNoiDungKienNghi = "";
                    if (Request["cNoiDungKN"] != null)
                    {
                        cNoiDungKienNghi = Request["cNoiDungKN"].ToString();
                    }

                    //if (iTinhTrang == 1)//chuyển BDN
                    //{
                    //    tinhtrang_to = 4;
                    //}
                    //if (iTinhTrang == 2)//chuyển địa phương; cơ quan thẩm quyền giải quyết
                    //{
                    //    tinhtrang_from = 3; tinhtrang_to = 4;
                    //}
                    //if (iTinhTrang == 3)//đang xử lý
                    //{
                    //    tinhtrang_from = 4; tinhtrang_to = 4;
                    //    datraloi = 0;
                    //}
                    //if (iTinhTrang == 4)//đã có trả lời
                    //{
                    //    tinhtrang_from = 4; tinhtrang_to = 4;
                    //    datraloi = 1;
                    //}
                    //if (iTinhTrang == 5)//kiến nghị trùng
                    //{
                    //    tinhtrang_from = 7; tinhtrang_to = 7;
                    //}
                    //if (iTinhTrang == 6)//tồn kỳ họp
                    //{
                    //    tinhtrang_from = 4; tinhtrang_to = 8;
                    //    datraloi = 1; ketqua_giamsat = 0;
                    //}
                    //if (iTinhTrang == 5)//tra cứu trùng
                    //{
                    //    list = _kiennghi.PRC_TRACUU_TRUNG(kiennghi, 0, dNgayNhan_from, dNgayNhan_to,page, post_per_page);
                    //}
                    //else//tra cứu
                    //{
                    List<PRC_KIENNGHI_LIST_TRACUU> list = _kiennghi.PRC_KIENNGHI_LIST_TRACUU_CAPNHAT(kiennghi, 0,
                        dNgayNhan_from,
                        dNgayNhan_to, dNgayTongHop_from, dNgayTongHop_to, dNgayTraLoi_from, dNgayTraLoi_to,
                        tinhtrang_from, tinhtrang_to, ketqua_giamsat, datraloi, maKienNghi, listNguonKienNghi,
                        cNoiDungKienNghi, page, post_per_page);
                    //}
                    bool isNormalSearch = false;
                    bool isAdvancedSearch = false;
                    if (Request["hidAdvancedSearch"] != null && Request["hidAdvancedSearch"].ToString() == "1") isAdvancedSearch = true;
                    if (Request["hidNormalSearch"] != null && Request["hidNormalSearch"].ToString() == "1") isNormalSearch = true;

                    string htmlList = string.Empty;
                    string htmlPhanTrang = string.Empty;
                    if (list.Count() > 0)
                    {
                        htmlPhanTrang = "<tr><td colspan='5'>" + base_appcode.PhanTrang((int)list.FirstOrDefault().TOTAL,
                                        post_per_page, page, RemovePageFromUrl()) +
                                    Option_Post_Per_Page(post_per_page) + "</td></tr>";
                        htmlList = kn.KN_Tracuu(list, u_info);
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

                }
                // load dữ liệu tìm kiếm 
                
                ViewData["kyhop"] = List_CheckBox_KyHop();
                // ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho();
                ViewData["radio-thamquyen"] = GetRadioButton_ThamQuyen_KienNghi(0);
                // ViewData["opt-donvixuly"] = Get_Option_Coquanxuly();

                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                string lstNguonKN = "all";
                if (Request["iTruocKyHop"] != null) lstNguonKN = "";
                if (Request["listNguonKN"] != null)
                {
                    lstNguonKN = Request["listNguonKN"];
                }
                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, lstNguonKN);
                string lstLinhVuc = "";
                if (Request["iTruocKyHop"] != null) lstLinhVuc = "";
                if (Request["iLinhVuc"] != null)
                {
                    lstLinhVuc = Request["iLinhVuc"];
                }

                //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu(lstLinhVuc);
                ViewData["opt-tenbaocao"] = Get_Option_BaoCao_By_ID_USERS();

                ViewData["kyhop"] = Get_Option_KyHop(iKyHop);
                
                ViewData["opt-thamquyen"] = Get_Option_CoQuan_TheoNhom(0);
                //ViewData["opt-doan"] = Get_Option_DonViTiepNhan(iDonViTiepNhan);
                if (Request["iThamQuyenDonVi"] != null) { iDonViTiepNhan = Convert.ToInt32(Request["iThamQuyenDonVi"]); }
                ViewData["opt-doan"] = Get_Option_DonViTiepNhan(u_info, iDonViTiepNhan);
                //if (iThamQuyenDonVi != -1)
                //{
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan(iThamQuyenDonVi, iLinhVuc);
                //}
                //int iDonVi = (int)u_info.user_login.IDONVI;
                //ViewData["opt-nguoicapnhat"] = get_Option_TaiKhoan_CapNhat(iUser_Capnhat, _kiennghi.PRC_LIST_USER_CAPNHAT((int)u_info.user_login.IDONVI));
                //ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                //if (u_info.tk_action.is_lanhdao)
                //{
                //    ViewData["opt-thamquyen"] = Get_Option_Coquanxuly(iThamQuyenDonVi);
                //    ViewData["bdn"] = 1;
                //    ViewData["opt-doan"] =  Get_Option_DonViTiepNhan(iDonViTiepNhan);
                //    if (iThamQuyenDonVi != -1)
                //    {
                //        ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan(iThamQuyenDonVi, iLinhVuc);
                //    }
                //    ViewData["opt-nguoicapnhat"] = get_Option_TaiKhoan_CapNhat(iUser_Capnhat, _kiennghi.PRC_LIST_USER_CAPNHAT((int)u_info.user_login.IDONVI));
                //}
                //else if(u_info.tk_action.is_dbqh)
                //{
                //    ViewData["opt-thamquyen"] = Get_Option_Coquanxuly(iThamQuyenDonVi);
                //    ViewData["dbqh"] = 1;
                //    ViewData["opt-doan"] = "<option value='" + iDonVi + "'>" + u_info.tk_action.tendonvi + "</option>";
                //    if (iThamQuyenDonVi != -1)
                //    {
                //        ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan(iThamQuyenDonVi, iLinhVuc);
                //    }
                //    ViewData["opt-nguoicapnhat"] = get_Option_TaiKhoan_CapNhat(iUser_Capnhat, _kiennghi.PRC_LIST_USER_CAPNHAT((int)u_info.user_login.IDONVI));
                //}
                //else if (u_info.tk_action.is_chuyenvien)
                //{
                //    iThamQuyenDonVi = (int)u_info.user_login.IDONVI;
                //    ViewData["opt-doan"] = Get_Option_DonViTiepNhanKienNghi_ForCQTW(iDonViTiepNhan);
                //    ViewData["opt-thamquyen"] = "<option value='" + iDonVi + "'>" + u_info.tk_action.tendonvi + "</option>";
                //    ViewData["cqtw"] = 1;
                //    ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan(iThamQuyenDonVi, iLinhVuc);
                //}                
                ViewData["opt-tracuu"] = Option_Tracuu(iTinhTrang); 
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tra cứu");
                return View("../Home/Error_Exception");
            }
        }
        // end phúc
        public ActionResult Ajax_Load_Panel_Kiennghi_Dagop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
                Dictionary<string, object> condition = new Dictionary<string, object>();
                condition.Add("ID_GOP", id);
                var kn_gop = _kiennghi.HienThiDanhSachKienNghi(condition);
                if (kn_gop == null) { return null; }
                List<QUOCHOI_COQUAN> coquan = _kiennghi.GetAll_CoQuanByParam(null).ToList();
                ViewData["list"] = kn.List_KienNghi_Chon_Gop_View(kn_gop, coquan, Request.Cookies["url_key"].Value);
                return PartialView("../Ajax/Kiennghi/Load_Panel_Kiennghi_Dagop");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách các kiến nghị đã gộp");
                return null;
            }
        }
        public ActionResult Kiennghi_gop_list()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = Request["id"];
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["kiennghi"] = kiennghi;
                ViewData["detail"] = kn.KienNghi_Detail(id, Request.Cookies["url_key"].Value);
                Dictionary<string, object> condition = new Dictionary<string, object>();
                condition.Add("ID_GOP", kiennghi.IKIENNGHI);
                var kn_gop = _kiennghi.HienThiDanhSachKienNghi(condition);
                bool remove = true;
                if (u_info.tk_action.is_lanhdao)
                {
                    if (kiennghi.ITONGHOP_BDN != 0)//đã đưa vào tổng h
                    {
                        if (kiennghi.ITINHTRANG >= (int)TrangThaiKienNghi.ChuaTraLoiDangXuly)
                        {//đã chuyển
                            remove = false;
                        }
                        else
                        {
                            if (kiennghi.IUSER != u_info.user_login.IUSER && !base_business.Action_(50, u_info.tk_action))
                            {
                                remove = false;
                            }
                        }
                    }
                    else
                    {
                        if (kiennghi.IUSER != u_info.user_login.IUSER && !base_business.Action_(50, u_info.tk_action))
                        {
                            remove = false;
                        }
                    }
                }
                else
                {
                    if (kiennghi.ITONGHOP != 0)
                    {
                        remove = false;//kiến nghị đã được đưa vào Tập hợp
                    }
                    else
                    {
                        if (u_info.tk_action.is_dbqh)
                        {
                            if (kiennghi.IUSER != u_info.user_login.IUSER) { remove = false; }//ko phải người nhập
                        }
                        else { remove = false; }
                    }
                }
                if (remove == false)
                {
                    ViewData["themkiennghi"] = 0;
                }
                else
                {
                    ViewData["themkiennghi"] = 1;
                }
                if (kiennghi.IUSER != u_info.user_login.IUSER && !base_business.Action_(50, u_info.tk_action))
                {
                    ViewData["add"] = " style='display:none' "; remove = false;
                }
                if (kiennghi.ITONGHOP != 0 || kiennghi.ITONGHOP_BDN != 0)
                {//đã đưa vào Tập hợp => ko cho xóa
                    ViewData["add"] = " style='display:none' "; remove = false;
                }
                Dictionary<string, object> dic_coquan = new Dictionary<string, object>();
                //dic_coquan.Add("IPARENT", ID_Coquan_doandaibieu);
                List<QUOCHOI_COQUAN> coquan = _kiennghi.GetAll_CoQuanByParam(dic_coquan).ToList();
                ViewData["list"] = kn.List_KienNghi_Chon_Gop(kn_gop, coquan, u_info.tk_action, remove, Request.Cookies["url_key"].Value);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thông tin chi tiết kiến nghị");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Kiennghi_info()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                ViewData["id"] = Request["id"];
                UserInfor u_info = GetUserInfor();

                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["kiennghi"] = kiennghi;

                ViewData["detail"] = kn.KienNghi_Detail(id, Request.Cookies["url_key"].Value);
                ViewData["capnhat"] = kn.NguoiCapNhat((int)kiennghi.IUSER, (DateTime)kiennghi.DDATE);
                ViewData["file"] = kn.File_View(id, "kn_kiennghi");
                KN_KIENNGHI kn_pram = new KN_KIENNGHI(); kn_pram.IKIENNGHI = id;
                var kn_traloi = _kiennghi.PRC_LIST_KN_TRALOI_DANHGIA(kn_pram);
                ViewData["bdn"] = 0;
                int count_traloi = 0;
                if (kn_traloi.Count() > 0)
                {
                    count_traloi = 1;
                    PRC_LIST_KN_TRALOI_DANHGIA traloi_danhgia = kn_traloi.FirstOrDefault();
                    ViewData["traloi"] = kn.Content_Traloi_kiennghi(traloi_danhgia);
                    if (u_info.tk_action.is_lanhdao)
                    {
                        ViewData["bdn"] = 1;

                        var knGiamSat = _kiennghi.GetAll_Giamsat_TraLoiByKienNghiID(id);
                        if (knGiamSat != null && knGiamSat.Count > 0)
                        {
                            ViewData["giamsat"] = kn.Content_GiamSat_kiennghi(traloi_danhgia);
                        }
                    }
                }
                ViewData["count_traloi"] = count_traloi;
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thông tin chi tiết kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        public ActionResult Kiennghi_lichsu()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                KN_KIENNGHI kiennghi = _kiennghi.HienThiThongTinKienNghi(id);
                ViewData["kiennghi"] = kiennghi;
                ViewData["detail"] = kn.KienNghi_Detail(id, Request.Cookies["url_key"].Value);
                ViewData["list"] = kn.KN_Lichsu(id);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Lịch sử xử lý kiến nghị");
                return View("../Home/Error_Exception");
            }

        }
        /*
        public ActionResult Theodoi_kiennghi_tralai_khongxuly()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                UserInfor u_info = GetUserInfor();
                int iDonViTiepNhan = 0;
                int iKyHop = ID_KyHop_HienTai();
                if (Request["iDonVi"] != null) { iDonViTiepNhan = Convert.ToInt32(Request["iDonVi"]); }
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                KN_KIENNGHI kn_pram = new KN_KIENNGHI();
                kn_pram.ITHAMQUYENDONVI = u_info.user_login.IDONVI ;
                kn_pram.IDONVITIEPNHAN = iDonViTiepNhan;
                //kn_pram.ITINHTRANG = (decimal)TrangThaiKienNghi.KienNghiTrung_DongKienNghi;
                var list_kn = _kiennghi.PRC_KIENNGHI_TRALAI(kn_pram, 0);

                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                //ViewData["list"] = kn.KN_Theodoi_luu(list_kn);
                var traloi = _kiennghi.GetAll_TraLoi_KienNghi();
                var giamsat = _kiennghi.GetAll_Giamsat_TraLoi();
                //ViewData["list"] = kn.KN_Theodoi_kiennghi_chuyenkysau(list_kn, traloi, giamsat, u_info.tk_action);

                ViewData["list"] = kn.KN_Theodoi_kiennghi_tralai(list_kn, traloi, giamsat, u_info.tk_action);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách kiến nghị trả lại");
                return View("../Home/Error_Exception");
            }

        }

        */
        public ActionResult Theodoi_kiennghi_tralai()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                int iDonViXuLy = 0;
                int iDonViXuLy_Parent = 0;
                int iDonViTiepNhan = 0;
                if (u_info.tk_action.is_lanhdao) { iDonViTiepNhan = 0; }
                //base_business.ActionMulty_Redirect_("3,4,5", u_info.tk_action);
                int iKyHop = ID_KyHop_HienTai();
                KN_KIENNGHI kn_pram = get_Request_Paramt_KienNghi();
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (Request["q"] != null) { kn_pram.CNOIDUNG = func.RemoveTagInput(Request["q"]); }
                if (Request["iDonViXuLy"] != null)
                {
                    iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]);
                }
                else
                {

                }
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDoan"] != null) { iDonViTiepNhan = Convert.ToInt32(Request["iDoan"]); }

                ViewData["dbqh"] = 0;
                ViewData["cqtw"] = 0;
                if (u_info.tk_action.is_dbqh)//ĐBQH
                {
                    ViewData["dbqh"] = 1;

                    kn_pram.ITHAMQUYENDONVI = iDonViXuLy;

                    kn_pram.IDONVITIEPNHAN = u_info.user_login.IDONVI;
                    //if (iDonViTiepNhan == 0 && iDonViXuLy == 0)
                    //{
                    //    kn_pram.ITHAMQUYENDONVI = u_info.user_login.IDONVI;
                    //}
                    //else
                    //{
                    //    if (iDonViTiepNhan == 0)
                    //    {
                    //        kn_pram.ITHAMQUYENDONVI = u_info.user_login.IDONVI;
                    //        kn_pram.IDONVITIEPNHAN = 0;
                    //    }
                    //    else
                    //    {
                    //        kn_pram.ITHAMQUYENDONVI = iDonViXuLy;
                    //        kn_pram.IDONVITIEPNHAN = u_info.user_login.IDONVI;
                    //    }
                    //}                    
                    ViewData["opt-coquan"] = Get_Option_DonViTiepNhan(iDonViTiepNhan);
                }
                else if (u_info.tk_action.is_lanhdao)//bdn
                {
                    ViewData["opt-coquan"] = Get_Option_DonViTiepNhan(iDonViTiepNhan);
                    kn_pram.ITHAMQUYENDONVI = iDonViXuLy;
                    kn_pram.IDONVITIEPNHAN = iDonViTiepNhan;
                }
                else if (u_info.tk_action.is_chuyenvien)//cqtw
                {
                    ViewData["cqtw"] = 1;
                    ViewData["opt-coquan"] = Get_Option_DonViTiepNhanKienNghi_ForCQTW(iDonViTiepNhan);
                    kn_pram.ITHAMQUYENDONVI = u_info.user_login.IDONVI;
                    kn_pram.IDONVITIEPNHAN = iDonViTiepNhan;
                }
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    if (iDonViXuLy != 0 && coquan.Where(x => x.ICOQUAN == iDonViXuLy).Count() == 0)
                    {
                        iDonViXuLy = 0;
                    }
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, iDonViXuLy);
                }
                else
                {
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(null, iDonViXuLy);
                }

                var list_kn = _kiennghi.PRC_LIST_KN_TRALOI_DANHGIA(kn_pram, iDonViXuLy_Parent, -1, page, post_per_page);
                //if (u_info.tk_action.is_dbqh && iDonViTiepNhan == 0 && iDonViXuLy == 0)
                //{
                //    int iDonVi_User = (int)u_info.user_login.IDONVI;
                //    //list_kn = list_kn.Where(x => x.ID_DONVITIEPNHAN == iDonVi_User || x.ID_THAMQUYENDONVI == iDonVi_User).ToList();
                //}

                ViewData["list"] = kn.KN_Theodoi_kiennghi_chuyenkysau(list_kn, u_info.tk_action);
                if (list_kn.Count() > 0)
                {
                    ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)list_kn.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";
                }
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách kiến nghị trả lại");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult Theodoi_kiennghi_chuyenkysau()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                int post_per_page = pageSize; if (Request["post_per_page"] != null) { post_per_page = Convert.ToInt32(Request["post_per_page"]); }

                UserInfor u_info = GetUserInfor();
                int iDonViXuLy = 0; int iDonViXuLy_Parent = 0;
                int iDonViTiepNhan = 0;
                //if (u_info.tk_action.is_lanhdao) { iDonViTiepNhan = 0; }
                //base_business.ActionMulty_Redirect_("3,4,5", u_info.tk_action);
                int iKyHop = ID_KyHop_HienTai();
                KN_KIENNGHI kn_pram = get_Request_Paramt_KienNghi();
                if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                if (Request["q"] != null) { kn_pram.CNOIDUNG = func.RemoveTagInput(Request["q"]); }
                if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
                if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
                if (Request["iDoan"] != null) { iDonViTiepNhan = Convert.ToInt32(Request["iDoan"]); }
                ViewData["dbqh"] = 0;
                ViewData["cqtw"] = 0;
                if (u_info.tk_action.is_dbqh)//ĐBQH
                {
                    ViewData["dbqh"] = 1;
                    iDonViTiepNhan = (int)u_info.user_login.IDONVI;
                    kn_pram.ITHAMQUYENDONVI = iDonViXuLy;
                    kn_pram.IDONVITIEPNHAN = iDonViTiepNhan;
                    //if (iDonViTiepNhan == 0 && iDonViXuLy == 0)
                    //{

                    //}
                    //else
                    //{
                    //    if (iDonViTiepNhan == 0)
                    //    {
                    //        kn_pram.ITHAMQUYENDONVI = u_info.user_login.IDONVI;
                    //        kn_pram.IDONVITIEPNHAN = 0;
                    //    }
                    //    else
                    //    {
                    //        kn_pram.ITHAMQUYENDONVI = iDonViXuLy;
                    //        kn_pram.IDONVITIEPNHAN = u_info.user_login.IDONVI;
                    //    }
                    //}
                    ViewData["opt-coquan"] = Get_Option_DonViTiepNhan(iDonViTiepNhan);
                }
                else if (u_info.tk_action.is_lanhdao)//bdn
                {
                    ViewData["opt-coquan"] = Get_Option_DonViTiepNhan(iDonViTiepNhan);
                    kn_pram.ITHAMQUYENDONVI = iDonViXuLy;
                    kn_pram.IDONVITIEPNHAN = iDonViTiepNhan;
                }
                else if (u_info.tk_action.is_chuyenvien)//cqtw
                {
                    ViewData["cqtw"] = 1;
                    ViewData["opt-coquan"] = Get_Option_DonViTiepNhanKienNghi_ForCQTW(iDonViTiepNhan);
                    kn_pram.ITHAMQUYENDONVI = u_info.user_login.IDONVI;
                    kn_pram.IDONVITIEPNHAN = iDonViTiepNhan;
                }

                Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                if (iDonViXuLy_Parent != 0)
                {
                    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                    if (iDonViXuLy != 0 && coquan.Where(x => x.ICOQUAN == iDonViXuLy).Count() == 0)
                    {
                        iDonViXuLy = 0;
                    }
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, iDonViXuLy);
                }
                else
                {
                    ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(null, iDonViXuLy);
                }
                if (u_info.tk_action.is_dbqh && iDonViTiepNhan == 0 && iDonViXuLy == 0)
                {
                    int iDonVi_User = (int)u_info.user_login.IDONVI;
                    kn_pram.ITHAMQUYENDONVI = iDonVi_User;
                    //list_kn = list_kn.Where(x => x.ID_DONVITIEPNHAN == iDonVi_User || x.ID_THAMQUYENDONVI == iDonVi_User).ToList();
                }

                var list_kn = _kiennghi.PRC_LIST_KN_TRALOI_DANHGIA(kn_pram, iDonViXuLy_Parent, 0, page, post_per_page);
                //list_kn = list_kn.Where(x => x.GIAMSAT_DONGKIENNGHI == 0 && x.GIAMSAT_PHANLOAI != null).OrderByDescending(x => x.ID_LINHVUC_KIENNGHI).ToList();

                //Dictionary<string, object> donvi_pr = new Dictionary<string, object>();
                //donvi_pr.Add("IHIENTHI", 1); donvi_pr.Add("IDELETE", 0);
                //var coquan = _kiennghi.GetAll_CoQuanByParam(donvi_pr).ToList();
                //ViewData["opt-thamquyen"] = kn.OptionThamQuyenDonVi_Parent(coquan.Where(x => x.IPARENT == 0).ToList(), iDonViXuLy_Parent);
                //if (iDonViXuLy_Parent != 0)
                //{
                //    coquan = coquan.Where(x => x.IPARENT == iDonViXuLy_Parent).ToList();
                //}
                //else
                //{
                //    coquan = null;
                //}
                //ViewData["opt-thamquyen-xuly"] = kn.OptionThamQuyenDonVi_Parent_Child(coquan, iDonViXuLy);
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);
                ViewData["list"] = kn.KN_Theodoi_kiennghi_chuyenkysau(list_kn, u_info.tk_action);
                if (list_kn.Count() > 0)
                {
                    ViewData["phantrang"] = "<tr><td colspan='4'>" + base_appcode.PhanTrang((int)list_kn.FirstOrDefault().TOTAL, post_per_page, page, RemovePageFromUrl()) +
                        Option_Post_Per_Page(post_per_page) + "</td></tr>";

                }
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách kiến nghị chuyển kỳ sau");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult Ajax_Tonghop_search(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["url"] = fc["url"];

                Dictionary<string, object> donvi = new Dictionary<string, object>();
                List<QUOCHOI_COQUAN> coquan = _kiennghi.GetAll_CoQuanByParam(donvi);
                ViewData["opt-khoa-kyhop"] = Get_Option_KyHop(iKyHop);

                //ViewData["kehoach"] = _base.Option_KeHoach_ByKyHop(_base.IDDonVi_User(id_user()), iKyHop, 0);
                ViewData["thamquyen"] = "<option value='-1'>- - - Chọn tất cả</option>" +
                                        "<option value='0'> Chưa xác định</option> " +
                                        kn.OptionCoQuanXuLy(coquan);
                //ViewData["opt-th"] = form.Option_TinhThanh_ByID_Parent(0, 0);
                ViewData["opt-doan"] = "<option value='0'>- - - Chọn tất cả</option>" +
                                        kn.OptionCoQuanXuLy(coquan.Where(x => x.IPARENT == ID_Coquan_quochoi).ToList(), 0, 0, 0, 0) +
                                        kn.OptionCoQuanXuLy(coquan.Where(x => x.IPARENT == ID_Coquan_doandaibieu).ToList(), 0, 0, 0, 0);
                //List<LINHVUC> linhvuc = _kiennghi.GetAll_LinhVuc();
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                if (fc["url"] == "Tonghop")//Tập hợp
                {
                    if (!u_info.tk_action.is_lanhdao)
                    {
                        int iDonVi = (int)u_info.user_login.IDONVI;
                        ViewData["opt-doan"] = "<option value='" + iDonVi + "'>" +
                            _kiennghi.HienThiThongTinCoQuan(iDonVi).CTEN + "</option>";
                    }
                }
                else//đang xử lý, đã trả lời
                {
                    if (!u_info.tk_action.is_lanhdao)
                    {
                        int iDonVi = (int)u_info.user_login.IDONVI;
                        ViewData["thamquyen"] = "<option value='" + iDonVi + "'>" + _kiennghi.HienThiThongTinCoQuan(iDonVi).CTEN + "</option>";
                    }
                }


                return PartialView("../Ajax/Kiennghi/Tonghop_search");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form tìm Tập hợp kiến nghị");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ajax_Tracuu_result(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                //if (!CheckAuthToken_Api()) { return null; }
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI kiennghi = new KN_KIENNGHI();
                string cNoiDung = func.RemoveTagInput(fc["cNoiDung"]);
                kiennghi.CNOIDUNG = cNoiDung;
                int iTruocKyHop = -1; if (fc["iTruocKyHop"] != null) { iTruocKyHop = Convert.ToInt32(fc["iTruocKyHop"]); }
                kiennghi.ITRUOCKYHOP = iTruocKyHop;
                int iKyHop = 0; if (fc["iKyHop"] != null) { iKyHop = Convert.ToInt32(fc["iKyHop"]); }
                kiennghi.IKYHOP = iKyHop;
                int iDonViTiepNhan = 0; if (fc["iDonViTiepNhan"] != null) { iDonViTiepNhan = Convert.ToInt32(fc["iDonViTiepNhan"]); }
                kiennghi.IDONVITIEPNHAN = iDonViTiepNhan;
                int iThamQuyenDonVi = -1; if (fc["iThamQuyenDonVi"] != null) { iThamQuyenDonVi = Convert.ToInt32(fc["iThamQuyenDonVi"]); }
                kiennghi.ITHAMQUYENDONVI = iThamQuyenDonVi;
                int iLinhVuc = -1; if (fc["iLinhVuc"] != null) { iLinhVuc = Convert.ToInt32(fc["iLinhVuc"]); }
                kiennghi.ILINHVUC = iLinhVuc;
                int iUser_Capnhat = u_info.tk_action.iUser;
                if (u_info.tk_action.is_lanhdao)
                {
                    iUser_Capnhat = 0;
                }
                kiennghi.IUSER = iUser_Capnhat;
                //kiennghi.ITINHTRANG = iTinhTrang;
                DateTime dNgayNhan_from = DateTime.MinValue; if (fc["dNgayNhan_from"] != null && fc["dNgayNhan_from"] != "") { dNgayNhan_from = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayNhan_from"])); }
                DateTime dNgayNhan_to = DateTime.MaxValue; if (fc["dNgayNhan_to"] != null && fc["dNgayNhan_to"] != "") { dNgayNhan_to = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayNhan_to"])); }
                DateTime dNgayTongHop_from = DateTime.MinValue; if (fc["dNgayTongHop_from"] != null && fc["dNgayTongHop_from"] != "") { dNgayTongHop_from = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTongHop_from"])); }
                DateTime dNgayTongHop_to = DateTime.MaxValue; if (fc["dNgayTongHop_to"] != null && fc["dNgayTongHop_to"] != "") { dNgayTongHop_to = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTongHop_to"])); }
                DateTime dNgayTraLoi_from = DateTime.MinValue; if (fc["dNgayTraLoi_from"] != null && fc["dNgayTraLoi_from"] != "") { dNgayTraLoi_from = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTraLoi_from"])); }
                DateTime dNgayTraLoi_to = DateTime.MaxValue; if (fc["dNgayTraLoi_to"] != null && fc["dNgayTraLoi_to"] != "") { dNgayTraLoi_to = Convert.ToDateTime(func.ConvertDateToSql(fc["dNgayTraLoi_to"])); }

                int iTinhTrang = -1; if (fc["iTinhTrang"] != null) { iTinhTrang = Convert.ToInt32(fc["iTinhTrang"]); }
                int tinhtrang_from = Convert.ToInt32(TrangThaiKienNghi.Moicapnhat);
                int tinhtrang_to = Convert.ToInt32(TrangThaiKienNghi.TamxoaKiennghi);
                int ketqua_giamsat = -1;

                var listIdTinhTrang = EnumHelper<TrangThaiKienNghi>.GetListHashCode();
                string list_id_tinhtrang = String.Join(",", listIdTinhTrang.Select(x => x.Id));
                if (u_info.tk_action.is_chuyenvien)
                {
                    tinhtrang_from = 3; tinhtrang_to = 4;
                    list_id_tinhtrang = "" + (decimal)TrangThaiKienNghi.ChuaTraLoiDangXuly + "," +
                                           (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet + "";
                }
                string list_giamsat = "-1,0,1";//tất cả; 0:chuyển kỳ sau; 1: đóng kiến nghị
                int datraloi = -1;//tất cả; 0: chưa trả lời; 1 đã trả lời
                if (iTinhTrang == 1)//chuyển BDN
                {
                    tinhtrang_to = 4;
                    //list = list.Where(x => x.ITINHTRANG >= (decimal)TrangThaiKienNghi.DaChuyenTongHopDenBDN).ToList();
                    //list_id_tinhtrang = "(2,3,4)";
                }
                if (iTinhTrang == 2)//chuyển địa phương; cơ quan thẩm quyền giải quyết
                {
                    list_id_tinhtrang = (decimal)TrangThaiKienNghi.ChuaTraLoiDangXuly + "," +
                                            (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet + "";
                    tinhtrang_from = 3; tinhtrang_to = 4;
                    //list = list.Where(x => x.ITINHTRANG >= (decimal)TrangThaiKienNghi.ChuaTraLoiDangXuly).ToList();
                }
                if (iTinhTrang == 3)//đang xử lý
                {
                    tinhtrang_from = 4; tinhtrang_to = 4;
                    list_id_tinhtrang = "" + (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet + "";
                    datraloi = 0;
                    //list = list.Where(x => x.ITINHTRANG == (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet && x.TRALOI_SOVANBAN == null).ToList();
                }
                if (iTinhTrang == 4)//đã có trả lời
                {
                    tinhtrang_from = 4; tinhtrang_to = 4;
                    list_id_tinhtrang = "" + (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet + "";
                    datraloi = 1;
                    //list = list.Where(x => x.TRALOI_SOVANBAN != null).ToList();
                }
                if (iTinhTrang == 5)//kiến nghị trùng
                {
                    tinhtrang_from = 7; tinhtrang_to = 7;
                    list_id_tinhtrang = "" + (decimal)TrangThaiKienNghi.KienNghiTrung_DongKienNghi + "";
                    //list = list.Where(x => x.ITINHTRANG == (decimal)TrangThaiKienNghi.KienNghiTrung_DongKienNghi).ToList();
                }
                if (iTinhTrang == 6)//tồn kỳ họp
                {
                    tinhtrang_from = 4; tinhtrang_to = 4;
                    list_id_tinhtrang = "" + (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet + "";
                    datraloi = 1; ketqua_giamsat = 0;
                    //list = list.Where(x => x.GIAMSAT_DONGKIENNGHI == 0).ToList();
                    list_giamsat = "0";
                }
                List<PRC_KIENNGHI_LIST_TRACUU> list = _kiennghi.PRC_KIENNGHI_LIST_TRACUU(kiennghi, 0, dNgayNhan_from,
                dNgayNhan_to, dNgayTongHop_from, dNgayTongHop_to, dNgayTraLoi_from, dNgayTraLoi_to,
                tinhtrang_from, tinhtrang_to, ketqua_giamsat, datraloi);
                //ViewData["list"] = kn.KN_Tracuu(list);
                return PartialView("../Ajax/Kiennghi/Tracuu_result");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Kết quả tra cứu");
                return View("../Home/Error_Exception");
            }

        }

        public string Option_Tracuu(int iTinhTrang = 0)
        {
            StringBuilder str = new StringBuilder();
            var listTrangThaiKienNghi = EnumHelper<TrangThaiKienNghi>.GetListHashCode();
            foreach (var trangThai in listTrangThaiKienNghi)
            {
                str.Append("<option value=" + trangThai.Id + ">" + trangThai.Text + "</option>");
            }
            return str.ToString();
        }
        public string UploadFile(HttpPostedFileBase file)
        {
            /* ------- start upload ngoài thư mục code */
            string file_name = "";
            string dir_path_upload = AppConfig.dir_path_upload;
            string path_upload = "/kiennghi/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
            string full_path = "";
            Random random = new Random(); int rand = random.Next(0, 99999);
            if (dir_path_upload == "")
            {
                full_path = "/upload" + path_upload;
                bool IsExists = Directory.Exists(Server.MapPath(full_path));
                if (!IsExists)
                {
                    Directory.CreateDirectory(Server.MapPath(full_path));
                }
                file_name = full_path + DateTime.Now.ToString("Hmmssff") + rand + "_" + func.ConvertVn(file.FileName);
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
                    file_name = DateTime.Now.ToString("Hmmssff") + rand + "_" + func.ConvertVn(file.FileName);
                    file.SaveAs(full_path + file_name);
                }
                return path_upload + file_name;
            }
            //Stream fs = file.InputStream;


            /* upload binary*/

            /* end upload binary*/

            /* ------- end upload ngoài thư mục code */

            /* ------- start upload trong thư mục code */
            //string file_name = "";
            //string path_upload = "/upload/kiennghi/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
            //bool IsExists = Directory.Exists(Server.MapPath(path_upload));
            //if (!IsExists)
            //{
            //    Directory.CreateDirectory(Server.MapPath(path_upload));
            //}
            //if (file != null && file.ContentLength > 0)
            //{
            //    file_name = path_upload + DateTime.Now.ToString("Hmmss") + "_" + func.ConvertVn(file.FileName);
            //    file.SaveAs(Server.MapPath(file_name));
            //}
            //return file_name;
            /* ------- end upload trong thư mục code */
        }
        //public byte[] UploadFileBinary(HttpPostedFileBase file)
        //{
        //    /* ------- start upload ngoài thư mục code */
        //    Stream fs = file.InputStream;
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] bytes = br.ReadBytes((Int32)fs.Length);
        //    return bytes;
        //}
        public ActionResult Ajax_Import_tao_tonghop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id_import = Convert.ToInt32(HashUtil.Decode_ID(fc["id"].ToString(), Request.Cookies["url_key"].Value));
                KN_IMPORT import = _kiennghi.Get_Import(id_import);

                var kiennghi = _kiennghi.PRC_KIENNGHI_IMPORT(id_import);
                int id_donvitiepnhan = 0;
                int id_tonghop = 0;
                foreach (var k in kiennghi)
                {
                    if (id_donvitiepnhan != k.IDONVITIEPNHAN)
                    {
                        KN_TONGHOP t = TaoTapHop_byKienNghi(k, import);
                        id_tonghop = (int)t.ITONGHOP;
                    }
                    InsertKienNghi_By_KienNghiImport(k, id_import, id_tonghop);
                    id_donvitiepnhan = (int)k.IDONVITIEPNHAN;
                }
                import.ITINHTRANG = 1;//đã tạo tập hợp
                _kiennghi.Update_Import(import);
                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tạo tập hợp kiến nghị từ danh sách import");
                return View("../Home/Error_Exception");
            }

        }
        public Boolean InsertKienNghi_By_KienNghiImport(PRC_KIENNGHI_IMPORT import, int id_import, int id_tonghop)
        {
            bool result = true;
            try
            {
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI kiennghi = new KN_KIENNGHI();
                kiennghi.CDIACHI = "";
                kiennghi.CMAKIENNGHI = "";
                kiennghi.CNOIDUNG = import.CNOIDUNG;
                kiennghi.CNOIDUNG_TRUNG = "";
                kiennghi.CTUKHOA = "";
                kiennghi.DDATE = DateTime.Now;
                kiennghi.ICHUONGTRINH = import.ICHUONGTRINH;
                kiennghi.IDIAPHUONG0 = 0;
                kiennghi.IDIAPHUONG1 = 0;
                kiennghi.IDONVITIEPNHAN = import.IDONVITIEPNHAN;
                kiennghi.ID_GOP = 0;
                kiennghi.ID_KIENNGHI_PARENT = 0;
                kiennghi.IKIEMTRATRUNG = 0;
                kiennghi.IKIENNGHI_TRUNG = 0;
                kiennghi.IKYHOP = import.IKYHOP;
                kiennghi.ILINHVUC = import.ILINHVUC;
                kiennghi.IPARENT = 0;
                kiennghi.ITHAMQUYENDONVI = import.IDONVITHAMQUYEN;
                kiennghi.ITINHTRANG = (int)TrangThaiKienNghi.Moicapnhat;
                kiennghi.ITONGHOP = id_tonghop;
                kiennghi.ITONGHOP_BDN = 0;
                kiennghi.ID_IMPORT = id_import;
                kiennghi.IDELETE = 0;
                kiennghi.ITRUOCKYHOP = import.ITRUOCKYHOP;
                kiennghi.IUSER = (int)GetUserInfor().user_login.IUSER;
                _kiennghi.InsertKienNghi(kiennghi);
            }
            catch (Exception ex)
            {
                result = false;
                log.Log_Error(ex, "Thêm kiến nghị từ kiến nghị import");
                throw;
            }

            return result;
        }
        public KN_TONGHOP TaoTapHop_byKienNghi(PRC_KIENNGHI_IMPORT kn, KN_IMPORT import)
        {
            // tạo tổng hợp
            KN_TONGHOP t = new KN_TONGHOP();
            t.IDONVITONGHOP = kn.IDONVITIEPNHAN;
            t.ICHUONGTRINH = 0;
            t.IKYHOP = kn.IKYHOP;
            t.ILINHVUC = 0;
            t.ID_IMPORT = import.ID;
            t.ITHAMQUYENDONVI = ID_Ban_DanNguyen;
            t.ITINHTRANG = 2;//đã chuyển BDN
            t.ITRUOCKYHOP = kn.ITRUOCKYHOP;
            t.IUSER = (int)GetUserInfor().user_login.IUSER;
            string congvan = " ngày " + func.ConvertDateVN(import.DDATE.ToString());
            if (kn.CSOCONGVAN != null && kn.CCONGVAN != null && kn.DNGAYBANHANH != null)
            {
                congvan = " theo " + kn.CCONGVAN + " số " + kn.CSOCONGVAN + " ngày " + func.ConvertDateVN(kn.DNGAYBANHANH.ToString()) + ".";
            }
            t.CNOIDUNG = "Tập hợp kiến nghị của đoàn " + kn.TENDONVI_TIEPNHAN + " gửi đến " + kn.TEN_KYHOP +
                    " " + kn.TEN_KHOAHOP + congvan;
            t.DDATE = DateTime.Now;
            _kiennghi.InsertTongHop(t);
            //Tạo công văn liên quan
            KN_VANBAN v = new KN_VANBAN();
            v.ITONGHOP = t.ITONGHOP;
            v.IKIENNGHI = 0;
            v.ICOQUANBANHANH = kn.IDONVITIEPNHAN;
            v.ICOQUANNHAN = ID_Ban_DanNguyen;
            v.IUSER = (int)GetUserInfor().user_login.IUSER;
            v.DDATE = DateTime.Now;
            if (kn.DNGAYBANHANH != null)
            {
                v.DNGAYBANHANH = kn.DNGAYBANHANH;
            }
            else
            {
                v.DNGAYBANHANH = import.DDATE;
            }
            v.CNGUOIKY = "";
            v.CNOIDUNG = "";
            v.DNGAYDUKIENHOANTHANH = DateTime.Now;
            v.CLOAI = "tonghop_chuyenxuly";
            _kiennghi.Insert_Vanban(v);

            return t;
        }
        public ActionResult Ajax_Import_huy_tonghop(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id_import = Convert.ToInt32(HashUtil.Decode_ID(fc["id"].ToString(), Request.Cookies["url_key"].Value));
                KN_IMPORT import = _kiennghi.Get_Import(id_import);
                // hủy tổng hợp & văn bản liên quan
                // hủy kiến nghị
                // Hủy trả lời
                // Hủy đánh giá
                if (_kiennghi.Huy_Kiennghi_taphop_import(id_import))
                {
                    import.ITINHTRANG = 0;//đã tạo tập hợp
                    _kiennghi.Update_Import(import);
                }

                Response.Write(1);
                return null;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Hủy tập hợp kiến nghị từ danh sách import");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Import_taphop()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id_import = Convert.ToInt32(HashUtil.Decode_ID(Request["id"].ToString(), Request.Cookies["url_key"].Value));
                KN_IMPORT import = _kiennghi.Get_Import(id_import);
                var kiennghi = _kiennghi.PRC_TAPHOP_IMPORT(id_import);
                ViewData["list"] = kn.List_Import_Taphop(kiennghi);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách tập hợp đã import");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Import_kiennghi()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                int id_import = Convert.ToInt32(HashUtil.Decode_ID(Request["id"].ToString(), Request.Cookies["url_key"].Value));
                KN_IMPORT import = _kiennghi.Get_Import(id_import);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ID_IMPORT", id_import);
                List<PRC_KIENNGHI_IMPORT_LISTKN> kiennghi = _kiennghi.PRC_KIENNGHI_IMPORT_LISTKN(id_import);
                
                //var kiennghi = _kiennghi.PRC_KIENNGHI_IMPORT(id_import);
                ViewData["list"] = kn.List_Import_Kiennghi(kiennghi, Request.Cookies["url_key"].Value);
                /**string tao_taphop = " <a data-original-title='Tạo tập hợp tương ứng của Đoàn ĐBQH' onclick=\"Tao_TapHop('" + Request["id"].ToString() + "')\" rel='tooltip' href='#' class='add btn_f blue'>" +
                                    "<i class='icon-plus-sign'></i></a>";
                string xem_taphop = " <a data-original-title='Xem tập hợp, kiến nghị đã tạo' rel='tooltip' href='/Kiennghi/Import_taphop?id=" + Request["id"].ToString() + "' class='add btn_f blue'>" +
                                    "<i class='icon-list-alt'></i></a>";
                string xoa_taphop = " <a data-original-title='Hủy tập hợp, kiến nghị đã tạo' onclick=\"Huy_TapHop('" + Request["id"].ToString() + "')\" rel='tooltip' href='#' class='add btn_f blue'>" +
                                    "<i class='icon-remove'></i></a>";
                string import_lai = " <a data-original-title='Import lại danh sách kiến nghị' onclick=\"ShowPopUp('id=" + Request["id"].ToString() + "','/Kiennghi/Ajax_Import_edit/')\" rel='tooltip' href='#' class='add btn_f blue'>" +
                                    "<i class='icon-refresh'></i></a>";
                if (import.ITINHTRANG == 0)//chưa tạo tập hợp
                {
                    xem_taphop = ""; xoa_taphop = "";
                }
                if (import.ITINHTRANG == 1)//Đã tự động tạo tập hợp
                {
                    import_lai = ""; tao_taphop = "";
                }
                ViewData["tao_taphop"] = tao_taphop;
                ViewData["xem_taphop"] = xem_taphop;
                ViewData["xoa_taphop"] = xoa_taphop;
                ViewData["import_lai"] = import_lai;**/
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách import kiến nghị");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Import()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int page = 1; if (Request["page"] != null) { page = Convert.ToInt32(Request["page"]); }
                func.SetCookies("url_return", Request.Url.AbsoluteUri);
                //int iKyHop = ID_KyHop_HienTai();
                //if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
                UserInfor u_info = GetUserInfor();
                int iUser = (int)u_info.user_login.IUSER;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if (!u_info.tk_action.is_lanhdao)
                {
                    dic.Add("IUSER", iUser);
                }
                
                //if (iKyHop != 0) { dic.Add("iKyHop", iKyHop); }
                var import = _kiennghi.GetAll_Import(dic);
                //var import = _kiennghi.GetAll_Import_Paging(dic, page, 2);
                
                /**if (!u_info.tk_action.is_lanhdao)
                {
                    import = import.Where(x => x.IUSER == iUser).ToList();
                }**/
                ViewData["opt-kyhop"] = Get_Option_KyHop();
                ViewData["list"] = kn.List_Import(import, Request.Cookies["url_key"].Value);
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách import kiến nghị");
                return View("../Home/Error_Exception");
            }
        }
        public ActionResult Ajax_Import_edit(FormCollection fc)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"].ToString(), Request.Cookies["url_key"].Value));
                KN_IMPORT import = _kiennghi.Get_Import(id);
                SetTokenAction("Import_edit", id);
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = Get_Option_KyHop((int)import.IKYHOP);
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop((int)import.ITRUOCKYHOP);
                ViewData["id"] = fc["id"].ToString();
                ViewData["import"] = import;
                return PartialView("../Ajax/Kiennghi/Import_edit");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form import kiến nghị");
                throw;
            }
        }
        public ActionResult Ajax_Import_add()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                SetTokenAction("Import_add");
                UserInfor u_info = GetUserInfor();
                ViewData["opt-kyhop"] = Get_Option_KyHop(ID_KyHop_HienTai());
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                return PartialView("../Ajax/Kiennghi/Import_add");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Form import kiến nghị");
                throw;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Import_update(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"].ToString(), Request.Cookies["url_key"].Value));
                if (!CheckTokenAction("Import_edit", id)) { Response.Redirect("/Home/Error/"); return null; }
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
                else
                {
                    Response.Redirect("/Home/Error/?type=type"); return null;
                }

                UserInfor u_info = GetUserInfor();
                KN_IMPORT t = _kiennghi.Get_Import(id);
                t.IKYHOP = Convert.ToInt32(fc["iKyHop"]);
                t.CGHICHU = func.RemoveTagInput(fc["CGHICHU"]);

                t.ITRUOCKYHOP = 1;
                if (fc["iTruocKyHop"] != null)
                {
                    t.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]);
                }
                //t.DDATE = DateTime.Now;
                t.ISOKIENNGHI = 0;
                t.CFILE = file_name;
                t.ITINHTRANG = 0;
                _kiennghi.Update_Import(t);

                if (t != null)
                {
                    if (_kiennghi.Delete_Kiennghi_ByID_import(id))
                    {
                        Tracking(u_info.tk_action.iUser, "Import lại kiến nghị: " + t.CGHICHU);
                        InsertKienNghi_Import(t);
                    }
                    //InsertKienNghi_After_Import(t);
                }
                Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Import kiến nghị");
                return View("../Home/Error_Exception");
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
                KN_IMPORT t = new KN_IMPORT();
                t.IKYHOP = Convert.ToInt32(fc["iKyHop"]);
                t.CGHICHU = func.RemoveTagInput(fc["CGHICHU"]);
                t.IUSER = u_info.tk_action.iUser;
                t.ITRUOCKYHOP = 1;
                if (fc["iTruocKyHop"] != null)
                {
                    t.ITRUOCKYHOP = Convert.ToInt32(fc["iTruocKyHop"]);
                }
                t.DDATE = DateTime.Now;
                t.ISOKIENNGHI = 0;
                t.CFILE = file_name;
                t.ITINHTRANG = 0;
                //t = _kiennghi.Insert_Import(t);

                //if (t != null)
                //{
               //     Tracking(u_info.tk_action.iUser, "Import kiến nghị: " + t.CGHICHU);
                //    string checkFileError = CheckFile_KienNghi_Import(t);
                //    if(checkFileError == "") InsertFile_KienNghi_Import(t);
                //    //InsertKienNghi_After_Import(t);
                //}

                //Response.Redirect(Request.Cookies["url_return"].Value + "#success");
                return null;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Import kiến nghị");
                return View("../Home/Error_Exception");
            }
            //int id = Convert.ToInt32(HashUtil.Decode_ID(fc["id"], Request.Cookies["url_key"].Value));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Check_import_insert(FormCollection fc, HttpPostedFileBase file)
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                func.SetCookies("file_check", "false");
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
                KN_IMPORT t = new KN_IMPORT();
                t.CGHICHU = func.RemoveTagInput(fc["CGHICHU"]);
                t.IUSER = u_info.tk_action.iUser;
                t.ITRUOCKYHOP = 1;
                t.CFILE = file_name;
                t.ITINHTRANG = 0;
                string result = "";
                string str = "";
                if (t != null)
                {
                    Tracking(u_info.tk_action.iUser, "Import kiến nghị: " + t.CGHICHU);
                    
                    var chuongtrinh = _kiennghi.GetAll_ChuongTrinh(0, 0);
                    var nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                    Dictionary<string, object> donvi = new Dictionary<string, object>();
                    var coquanthamquyen = _kiennghi.GetAll_CoQuanByParam(donvi).ToList();
                    var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
                    string dir_path_download = AppConfig.dir_path_download;
                    string file_path = "";
                    if (dir_path_download != "")
                    {
                        file_path = dir_path_download + t.CFILE;
                    }
                    else
                    {
                        file_path = Server.MapPath("~/" + t.CFILE + "");
                    }

                    Workbook workbook = new Workbook();
                    workbook.LoadFromFile(file_path);
                    Worksheet sheet = workbook.Worksheets[0];
                    DataTable db = sheet.ExportDataTable();

                    bool check = true;
                    if (db.Columns.Count < 7)
                    {
                        ViewData["error"] = true;
                        filecheck = false;
                        return PartialView("../Ajax/kntc/Import_add_view_file");
                    }
                    if (db.Rows.Count > 0)
                    {
                        filecheck = true;
                        int id_import = (int)t.ID;

                        for (int i = 0; i < db.Rows.Count; i++)
                        {
                            result = "";
                            DataRow dr = db.Rows[i];
                            check = true;
                            //check theo ke hoach so
                            if (dr[1].ToString() != "")
                            {
                                if (chuongtrinh.Where(x => x.CKEHOACH == dr[1].ToString()).Count() == 0)
                                {
                                    check = false;
                                    result = result + " kế hoạch số,";
                                }
                            }
                            else
                            {
                                check = false;
                                result = result + " kế hoạch số,";
                            }

                            //check noi dung
                            if (dr[2].ToString() == "")
                            {
                                check = false; 
                                result = result + " nội dung,";
                            }
                            //check nguon kien nghi

                            if (dr[3].ToString() != "")
                            {
                                var ngknarr = dr[3].ToString().Split(',');
                                if (ngknarr.Length > 0)
                                {
                                    for (int j = 0; j < ngknarr.Length; j++)
                                    {
                                        if (nguonkiennghi.Where(x => x.INGUONDON.ToString() == ngknarr[j]).Count() == 0)
                                        {
                                            check = false;
                                            result = result + " nguồn kiến nghị,";
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    check = false; 
                                    result = result + " nguồn kiến nghị,";
                                }
                            }
                            else
                            {
                                check = false;
                            }

                            //check tham quyen giai quyet
                            if (dr[4].ToString() != "")
                            {
                                if (coquanthamquyen.Where(x => x.ICOQUAN.ToString() == dr[4].ToString()).Count() == 0)
                                {
                                    check = false;
                                    result = result + " thẩm quyền giải quyết,";
                                }
                            }
                            else
                            {
                                check = false;
                                result = result + " thẩm quyền giải quyết,";
                            }
                            //check linh vuc
                            if (dr[5].ToString() != "")
                            {
                                if (linhvuc.Where(x => x.MALINHVUC == dr[5].ToString()).Count() == 0)
                                {
                                    check = false;
                                    result = result + " lĩnh vực,";
                                }
                            }
                            else
                            {
                                check = false;
                                result = result + " lĩnh vực,";
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
                            str = str + "<td class='tcenter' >" + dr[1] + "</td>";
                            str = str + "<td >" + dr[2] + "</td>";
                            str = str + "<td class='tcenter' > " + dr[3] + "</td>";
                            str = str + "<td class='tcenter'>" + dr[4] + "</td>";
                            str = str + "<td class='tcenter' >" + dr[5] + "</td>";
                            str = str + "<td class='tcenter' > " + dr[6] + "</td>";
                            if (check == false)
                            {
                                str = str + "<td width = '5%' class='tcenter' onclick='showAlert(" +"`"+result.Remove(result.Length-1,1)+"`"+ ")' ><i class='icon-eye-open'></i></td></tr>";
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
                if(filecheck) func.SetCookies("file_check", "true");
                else func.SetCookies("file_check", "false");
                return PartialView("../Ajax/Kiennghi/Import_add_view_file");
            }
            catch (Exception ex)
            {

                log.Log_Error(ex, "Form import kiến nghị");
                func.SetCookies("file_check", "false");
                throw;
            }
            
        }

        public string Get_status_check_file_import()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                return Request.Cookies["file_check"].Value;
            }
            catch (Exception ex)
            {

                log.Log_Error(ex);
                throw;
            }

        }

        public string CheckFile_KienNghi_Import(KN_IMPORT import)
        {
            string result = "";
            try
            {
                var chuongtrinh = _kiennghi.GetAll_ChuongTrinh(0, 0);
                var nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                Dictionary<string, object> donvi = new Dictionary<string, object>();
                var coquanthamquyen = _kiennghi.GetAll_CoQuanByParam(donvi).ToList();
                var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
                string dir_path_download = AppConfig.dir_path_download;
                string file_path = "";
                if (dir_path_download != "")
                {
                    file_path = dir_path_download + import.CFILE;
                }
                else
                {
                    file_path = Server.MapPath("~/" + import.CFILE + "");
                }
                
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(file_path);
                Worksheet sheet = workbook.Worksheets[0];
                DataTable db = sheet.ExportDataTable();
                
                bool check = true;
                if (db.Rows.Count > 0)
                {
                    int id_import = (int)import.ID;

                    for (int i = 0; i < db.Rows.Count; i++)
                    {
                        DataRow dr = db.Rows[i];
                        check = true;
                        //check theo ke hoach so
                        if (dr[1].ToString() != "" && check)
                        {
                            if (chuongtrinh.Where(x => x.CKEHOACH == dr[1].ToString()).Count() == 0)
                            {
                                check = false;
                            }
                        }
                        else
                        {
                            check = false;
                        }

                        //check noi dung
                        if (dr[2].ToString() == "" && check)
                        {
                            check = false;
                        }
                        //check nguon kien nghi

                        if (dr[3].ToString() != "" && check)
                        {
                            var ngknarr = dr[3].ToString().Split(',');
                            if(ngknarr.Length > 0)
                            {
                                for(int t = 0; t < ngknarr.Length; t++)
                                {
                                    if (nguonkiennghi.Where(x => x.INGUONDON.ToString() == ngknarr[t]).Count() == 0)
                                    {
                                        check = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                check = false;
                            }
                        }
                        else
                        {
                            check = false;
                        }

                        //check tham quyen giai quyet
                        if (dr[4].ToString() != "" && check)
                        {
                            if (coquanthamquyen.Where(x => x.ICOQUAN.ToString() == dr[4].ToString()).Count() == 0)
                            {
                                check = false;
                            }
                        }
                        else
                        {
                            check = false;
                        }
                        //check linh vuc
                        if (dr[5].ToString() != "" && check)
                        {
                            if (linhvuc.Where(x => x.MALINHVUC == dr[5].ToString()).Count() == 0)
                            {
                                check = false;
                            }
                        }
                        else
                        {
                            check = false;
                        }
                        int r = i + 2;
                        if(check == false)
                        {
                            if(result == "") result = result + r;
                            else result = result + ',' + r;
                        }
                         

                    }
                }
                
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thêm kiến nghị từ danh sách import");
                throw;
            }

            return result;
        }
        public Boolean InsertFile_KienNghi_Import(string ghichu)
        {
            bool result = true;
            try
            {
                var file_name = Request.Cookies["file_name"].Value;
                UserInfor u_info = GetUserInfor();
                var chuongtrinh = _kiennghi.GetAll_ChuongTrinh(0, 0);
                var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
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
                KN_IMPORT import = new KN_IMPORT();
                import.IKYHOP = null;
                import.CGHICHU = ghichu;
                import.IUSER = u_info.tk_action.iUser;
                import.ITRUOCKYHOP = 1;
                import.DDATE = DateTime.Now;
                import.ISOKIENNGHI = 0;
                import.CFILE = Request.Cookies["file_name"].Value;
                import.ITINHTRANG = 0;
                import.ISOKIENNGHI = db.Rows.Count;
                import = _kiennghi.Insert_Import(import);

                if (db.Rows.Count > 0)
                {

                    for (int i = 0; i < db.Rows.Count; i++)
                    {
                        DataRow dr = db.Rows[i];

                        ////////////
                        int iUser = GetUserInfor().tk_action.iUser;
                        int ThamQuyenDonVi = 0;
                        ThamQuyenDonVi = Convert.ToInt32(dr[4]);
                        KN_KIENNGHI kn = new KN_KIENNGHI();
                        kn.IDOITUONGGUI = 1;
                        if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
                        {
                            kn.IDOITUONGGUI = 1;
                        }
                        if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                        {
                            kn.IDOITUONGGUI = 0;
                        }
                        
                        string lstNguonKN = dr[3].ToString();
                        kn.ID_IMPORT = import.ID;
                        kn.ID_GOP = 0;
                        kn.IDELETE = 0;
                        kn.CMAKIENNGHI = Get_Auto_SoKienNghi(0);
                        kn.CNOIDUNG = dr[2].ToString();
                        kn.CTUKHOA = "";
                        kn.DDATE = DateTime.Now;
                        var ct = chuongtrinh.Where(x => x.CKEHOACH == dr[1].ToString()).FirstOrDefault();
                        kn.ICHUONGTRINH = ct.ICHUONGTRINH;
                        kn.IKYHOP = ct.IKYHOP;
                        kn.IDONVITIEPNHAN = ct.IDONVI;
                        kn.IKIEMTRATRUNG = 0;
                        kn.IKIENNGHI_TRUNG = 0;
                        var lv = linhvuc.Where(x => x.MALINHVUC == dr[5].ToString()).FirstOrDefault();
                        kn.ILINHVUC = lv.ILINHVUC;
                        kn.CGHICHU = dr[6].ToString();
                        kn.ITHAMQUYENDONVI = ThamQuyenDonVi;
                        kn.ITINHTRANG = (decimal)TrangThaiKienNghi.Moicapnhat;
                        kn.ITONGHOP = 0;
                        kn.IPARENT = 0;
                        kn.ITONGHOP_BDN = 0;
                        kn.ID_KIENNGHI_PARENT = 0;

                        kn.ITRUOCKYHOP = 1;
                        func.SetCookies("truockyhop", "0");
                        kn.IUSER = iUser;
                        _kiennghi.InsertKienNghi(kn);
                        int iKienNghi = (int)kn.IKIENNGHI;
                        
                        string[] arrLSKN = lstNguonKN.Split(',');
                        for (int t = 0; t < arrLSKN.Count(); t++)
                        {
                            KIENNGHI_NGUONDON kn_nd = new KIENNGHI_NGUONDON();
                            kn_nd.IKIENNGHI = iKienNghi;
                            kn_nd.INGUONDON = Convert.ToDecimal(arrLSKN[t]);
                            _kiennghi.InsertKienNghi_NguonDon(kn_nd);
                        }
                        _kiennghi.Tracking_KN(iUser, iKienNghi, "Thêm mới kiến nghị");
                        

                    }
                }
                
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thêm kiến nghị từ danh sách import");
                throw;
            }
            return result;
        }
        public Boolean InsertKienNghi_Import(KN_IMPORT import)
        {
            bool result = true;
            try
            {
                UserInfor u_info = GetUserInfor();
                var coquan = _kiennghi.List_PRC_COQUAN_LINHVUC();
                //var chuongtrinh = _kiennghi.List_PRC_CHUONGTRINH_TXCT_EXPORT(ID_KyHop_HienTai());
                string dir_path_download = AppConfig.dir_path_download;
                string file_path = "";
                if (dir_path_download != "")
                {
                    file_path = dir_path_download + import.CFILE;
                }
                else
                {
                    file_path = Server.MapPath("~/" + import.CFILE + "");
                }
                //string path = Server.MapPath(import.CFILE);
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(file_path);
                Worksheet sheet = workbook.Worksheets[0];
                DataTable db = sheet.ExportDataTable();
                int count = 0;
                if (db.Rows.Count > 0)
                {
                    int id_import = (int)import.ID;

                    for (int i = 0; i < db.Rows.Count; i++)
                    {
                        int id_donvitiepnhan = ID_Ban_DanNguyen;
                        int id_donvithamquyen = ID_Ban_DanNguyen;
                        int id_linhvuc = 0; int id_chuongtrinh = 0;

                        DataRow dr = db.Rows[i];
                        if (dr[1].ToString() != "")
                        {
                            KN_KIENNGHI_IMPORT k = new KN_KIENNGHI_IMPORT();
                            k.ID_IMPORT = id_import;
                            k.ID_KIENNGHI = 0;
                            k.ID_TONGHOP_BDN = 0;
                            k.ITRUOCKYHOP = (int)import.ITRUOCKYHOP;
                            k.IKYHOP = (int)import.IKYHOP;
                            k.CNOIDUNG = dr[2].ToString();

                            PRC_COQUAN_LINHVUC p_thamquyen = new PRC_COQUAN_LINHVUC();
                            PRC_COQUAN_LINHVUC p_linhvuc = new PRC_COQUAN_LINHVUC();
                            if (dr[3].ToString() != "")//đơn vị tiếp nhận
                            {
                                if (coquan.Where(x => x.MA_COQUAN != null && (x.TENCOQUAN.ToUpper().Contains(dr[3].ToString().ToUpper()) || x.MA_COQUAN.ToUpper().Contains(dr[3].ToString().ToUpper()))).Count() > 0)
                                {
                                    id_donvitiepnhan = (int)coquan.Where(x => x.MA_COQUAN != null && (x.TENCOQUAN.ToUpper().Contains(dr[3].ToString().ToUpper()) || x.MA_COQUAN.ToUpper().Contains(dr[3].ToString().ToUpper()))).FirstOrDefault().ID_COQUAN;
                                }
                            }
                            if (dr[4].ToString() != "")//đơn vị thẩm quyền
                            {
                                if (coquan.Where(x => x.MA_COQUAN != null && (x.TENCOQUAN.ToUpper().Contains(dr[4].ToString().ToUpper()) || x.MA_COQUAN.ToUpper().Contains(dr[4].ToString().ToUpper()))).Count() > 0)
                                {
                                    p_thamquyen = coquan.Where(x => x.MA_COQUAN != null && (x.TENCOQUAN.ToUpper().Contains(dr[4].ToString().ToUpper()) || x.MA_COQUAN.ToUpper().Contains(dr[4].ToString().ToUpper()))).FirstOrDefault();
                                    id_donvithamquyen = (int)p_thamquyen.ID_COQUAN;
                                }
                            }
                            if (dr[5].ToString() != "")//lĩnh vực
                            {
                                if (coquan.Where(x => x.MA_LINHVUC != null && x.TENLINHVUC != null && (x.TENLINHVUC.ToUpper().Contains(dr[5].ToString().ToUpper()) || x.MA_LINHVUC.ToUpper().Contains(dr[5].ToString().ToUpper()))).Count() > 0)
                                {
                                    p_linhvuc = coquan.Where(x => x.MA_LINHVUC != null && x.TENLINHVUC != null && (x.TENLINHVUC.ToUpper().Contains(dr[5].ToString().ToUpper()) || x.MA_LINHVUC.ToUpper().Contains(dr[5].ToString().ToUpper()))).FirstOrDefault();
                                    id_linhvuc = (int)p_linhvuc.ID_LINHVUC;
                                }
                            }
                            if (p_thamquyen != null && p_linhvuc != null)
                            {
                                if (p_thamquyen.ID_COQUAN != p_linhvuc.ID_COQUAN)//row lĩnh vực != row cơ quan
                                {
                                    id_linhvuc = 0;
                                }
                            }
                            else
                            {
                                if (p_linhvuc != null)//lấy id_thẩm quyền theo row lĩnh vực
                                {
                                    id_donvithamquyen = (int)p_linhvuc.ID_COQUAN;
                                }
                            }
                            //if (dr[5].ToString() != "")//chương trình TXCT
                            //{
                            //    if (chuongtrinh.Where(x => x.KEHOACH_SO != null && x.KEHOACH_SO.ToUpper().Contains(dr[5].ToString().ToUpper())).Count() > 0)
                            //    {
                            //        id_chuongtrinh = (int)chuongtrinh.Where(x => x.KEHOACH_SO.ToUpper().Contains(dr[5].ToString().ToUpper())).FirstOrDefault().ID_CHUONGTRINH;
                            //    }
                            //}
                            k.IDONVITHAMQUYEN = id_donvithamquyen;
                            k.ILINHVUC = id_linhvuc;
                            k.ICHUONGTRINH = id_chuongtrinh;
                            k.IDONVITIEPNHAN = id_donvitiepnhan;
                            k.CCONGVAN = dr[1].ToString();
                            k.CSOCONGVAN = dr[1].ToString();
                            string ngaybanhanh = dr[9].ToString().Trim();
                            if (ngaybanhanh != "")
                            {
                                k.DNGAYBANHANH = Utils.ToDateTime(ngaybanhanh);
                            }
                            _kiennghi.KN_KIENNGHI_IMPORT_insert(k);
                            count++;

                        }

                    }
                }
                import.ISOKIENNGHI = count;
                _kiennghi.Update_Import(import);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thêm kiến nghị từ danh sách import");
                throw;
            }
            Response.Redirect(Request.Cookies["url_return"].Value + "#success");
            return result;
        }

        public Boolean InsertKienNghi_After_Import(KN_IMPORT import)
        {
            bool result = true;
            try
            {
                UserInfor u_info = GetUserInfor();
                string path = Server.MapPath(import.CFILE);
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(path);
                Worksheet sheet = workbook.Worksheets[0];
                DataTable db = sheet.ExportDataTable();
                Dictionary<string, object> _donvi = new Dictionary<string, object>();
                var coquan = _kiennghi.GetAll_CoQuanByParam(_donvi);
                //var chuongtrinh = _kiennghi.GetAll_ChuongTrinh(0,(int)import.IKYHOP);
                var coquan_code = coquan.Where(x => x.CCODE != null).ToList();
                var linhvuc = _kiennghi.GetAll_LinhVuc_CoQuan_By_IDCoQuan().Where(x => x.CCODE != null).ToList();
                var kyhop = _kiennghi.GetAll_KyHop();
                int count = 1;
                if (db.Rows.Count > 0)
                {
                    for (int i = 0; i < db.Rows.Count; i++)
                    {
                        KN_KIENNGHI kiennghi = new KN_KIENNGHI();

                        string donvi_tiepnhan = func.RemoveTagInput(db.Rows[i][2].ToString()).Trim();
                        string kiennghi_linhvuc = func.RemoveTagInput(db.Rows[i][4].ToString()).Trim();
                        string kiennghi_thamquyen = func.RemoveTagInput(db.Rows[i][3].ToString()).Trim();
                        string ten_chuongtrinh = func.RemoveTagInput(db.Rows[i][5].ToString()).Trim();
                        //string tenkyhop = func.RemoveTagInput(db.Rows[i][7].ToString()).Trim();
                        //string truockyhop = func.RemoveTagInput(db.Rows[i][8].ToString()).Trim();
                        int iDonViTiepNhan = ID_Ban_DanNguyen;
                        int id_chuongtrinh = 0;
                        if (donvi_tiepnhan != null && coquan.Where(x => x.CTEN.ToUpper().Contains(donvi_tiepnhan.ToUpper())).Count() > 0)
                        {
                            iDonViTiepNhan = (int)coquan.Where(x => x.CTEN.ToUpper().Contains(donvi_tiepnhan.ToUpper())).FirstOrDefault().ICOQUAN;
                            //if (ten_chuongtrinh != null && chuongtrinh.Where(x => x.IDONVI==iDonViTiepNhan && x.CKEHOACH.ToUpper().Contains(ten_chuongtrinh.ToUpper())).Count() > 0)
                            //{
                            //    id_chuongtrinh = (int)chuongtrinh.Where(x => x.IDONVI == iDonViTiepNhan && x.CKEHOACH.ToUpper().Contains(ten_chuongtrinh.ToUpper())).Count();
                            //}
                        }
                        int iThamQuyen = ID_Ban_DanNguyen; int iLinhVuc = 0;
                        if (kiennghi_thamquyen != null && coquan_code.Where(x => x.CCODE.ToUpper().Contains(kiennghi_thamquyen.ToUpper())).Count() > 0)
                        {
                            iThamQuyen = (int)coquan_code.Where(x => x.CCODE.ToUpper().Contains(kiennghi_thamquyen.ToUpper())).FirstOrDefault().ICOQUAN;
                            if (kiennghi_linhvuc != null && linhvuc.Where(x => x.CCODE.ToUpper().Contains(kiennghi_linhvuc.ToUpper())).Count() > 0)
                            {
                                LINHVUC_COQUAN lv = linhvuc.Where(x => x.CCODE.ToUpper().Contains(kiennghi_linhvuc.ToUpper())).FirstOrDefault();
                                if (lv.ICOQUAN == iThamQuyen)
                                {
                                    iLinhVuc = (int)lv.ILINHVUC;
                                }
                            }
                        }


                        int iKyHop = (int)import.IKYHOP;
                        //if (tenkyhop != null && coquan.Where(x => x.CTEN.ToUpper().Contains(tenkyhop.ToUpper())).Count() > 0)
                        //{
                        //    iKyHop = (int)kyhop.Where(x => x.CTEN.ToUpper().Contains(tenkyhop.ToUpper())).FirstOrDefault().IKYHOP;
                        //}                        
                        string ma_kiennghi = "";
                        kiennghi.CMAKIENNGHI = ma_kiennghi;
                        kiennghi.CNOIDUNG = kn.SubString_KienNghi_Import(func.RemoveTagInput(db.Rows[i][1].ToString()));
                        kiennghi.IDONVITIEPNHAN = iDonViTiepNhan;
                        kiennghi.IUSER = u_info.tk_action.iUser;
                        kiennghi.ITRUOCKYHOP = 1;
                        kiennghi.ITONGHOP_BDN = 0;
                        kiennghi.ITONGHOP = 0;
                        kiennghi.ITINHTRANG = (decimal)TrangThaiKienNghi.Moicapnhat;
                        kiennghi.ITHAMQUYENDONVI = iThamQuyen;
                        kiennghi.ILINHVUC = iLinhVuc;
                        kiennghi.ITRUOCKYHOP = (int)import.ITRUOCKYHOP;
                        kiennghi.IKYHOP = iKyHop;
                        kiennghi.IKIENNGHI_TRUNG = 0;
                        kiennghi.IKIEMTRATRUNG = 0;
                        kiennghi.ICHUONGTRINH = id_chuongtrinh;
                        kiennghi.ID_KIENNGHI_PARENT = 0;
                        kiennghi.DDATE = DateTime.Now;
                        kiennghi.CTUKHOA = "";
                        kiennghi.IDIAPHUONG0 = 0;
                        kiennghi.IPARENT = 0;
                        kiennghi.IDIAPHUONG1 = 0;
                        kiennghi.CDIACHI = "";
                        kiennghi.ID_GOP = 0;
                        _kiennghi.InsertKienNghi(kiennghi);

                        _kiennghi.Tracking_KN(u_info.tk_action.iUser, (int)kiennghi.IKIENNGHI, "Thêm kiến nghị từ import kiến nghị");
                        count++;
                    }
                }
                import.ISOKIENNGHI = count;
                _kiennghi.Update_Import(import);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Thêm kiến nghị từ danh sách import");
                throw;
            }

            return result;
        }
        //public ActionResult ReadExcel()
        //{
        //    string path = Server.MapPath("/Images/kiennghi_example.xls");
        //    Workbook workbook = new Workbook();
        //    workbook.LoadFromFile(path);
        //    Worksheet sheet = workbook.Worksheets[0];
        //    DataTable db = sheet.ExportDataTable();
        //    int count_row = db.Rows.Count;
        //    string sql_query = "";
        //    List<KN_IMPORT_TMP> tmp = new List<KN_IMPORT_TMP>();
        //    if (count_row > 0)
        //    {
        //        for (int i = 0; i < count_row; i++)
        //        {
        //            if (db.Rows[i][0].ToString() != "")
        //            {
        //                KN_IMPORT_TMP t = new KN_IMPORT_TMP();
        //                t.NOIDUNG = db.Rows[i][1].ToString();
        //                t.DIAPHUONG = db.Rows[i][2].ToString();
        //                t.DONVI_THAMQUYEN = db.Rows[i][3].ToString();
        //                t.LINHVUC = db.Rows[i][4].ToString();
        //                t.KEHOACH = db.Rows[i][5].ToString();
        //                t.KYHOP = db.Rows[i][6].ToString();
        //                t.HINHTHUC = db.Rows[i][7].ToString();
        //                t.CONGVAN = db.Rows[i][8].ToString();
        //                t.SOCONGVAN = db.Rows[i][9].ToString();
        //                DateTime ngaybanhanh = DateTime.Now;
        //                if (db.Rows[i][10].ToString() != "")
        //                {
        //                    ngaybanhanh = Convert.ToDateTime(db.Rows[i][10].ToString());
        //                }
        //                t.NGAYBANHANH = ngaybanhanh;
        //                t.ID_IMPORT = 0;
        //                tmp.Add(t);
        //                //sql_query += "INSERT INTO KN_IMPORT_TMP(NOIDUNG,DIAPHUONG,DONVI_THAMQUYEN,LINHVUC,KEHOACH,KYHOP,HINHTHUC,CONGVAN,SOCONGVAN,NGAYBANHANH,ID_IMPORT) " +
        //                //    "VALUES('" + db.Rows[i][1].ToString() + "','" + 
        //                //    db.Rows[i][2].ToString() + "','" + db.Rows[i][3].ToString() + "','" + db.Rows[i][4].ToString() + "','" + 
        //                //    db.Rows[i][5].ToString() + "','" + db.Rows[i][6].ToString() + "','" + db.Rows[i][7].ToString() + "','" + 
        //                //    db.Rows[i][8].ToString() + "','" + db.Rows[i][9].ToString() + "','" + db.Rows[i][10].ToString() + "','" + db.Rows[i][11].ToString() + "');";

        //            }
        //        }
        //    }
        //    Response.Write(tmp.Count());
        //    Response.Write(_kiennghi.Get_MultyQuery(tmp));
        //    //_kiennghi.ExcuteQuery(sql_query);
        //    //Response.Write(sql_query);
        //    return null;
        //}
        public IXLWorksheet Sheet_DanhMucKiennghi(XLWorkbook w_b)
        {

            var wb = w_b.Worksheets.Add("DanhSachKienNghi");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 13;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(1).Width = 8;
            wb.Column(2).Width = 24;
            wb.Column(3).Width = 60;
            wb.Column(4).Width = 30;
            wb.Column(5).Width = 30;
            wb.Column(6).Width = 30;
            wb.Column(7).Width = 30;
            wb.Row(1).Height = 40;
            //wb.Column(8).Width = 20;
            //wb.Column(9).Width = 20;
            //wb.Column(10).Width = 20;
            //wb.Column(11).Width = 20;
            wb.Cell(1, 1).Value = "STT";
            wb.Range(1, 1, 1, 1).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188))
                                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 2).Value = "Theo kế hoạch số";
            wb.Range(1, 2, 1, 2).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).
                                                    Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 3).Value = "Nội dung kiến nghị";
            wb.Range(1, 3, 1, 3).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 4).Value = "Nguồn kiến nghị";
            wb.Range(1, 4, 1, 4).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 5).Value = "Thẩm quyền giải quyết";
            wb.Range(1, 5, 1, 5).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188))
                                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
           
            wb.Cell(1, 6).Value = "Lĩnh vực";
            wb.Range(1, 6, 1, 6).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 7).Value = "Ghi chú";
            wb.Range(1, 7, 1, 7).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            /**wb.Cell(1, 8).Value = "Công văn";
            wb.Range(1, 8, 1, 8).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).
                                                    Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 9).Value = "Số";
            wb.Range(1, 9, 1, 9).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 10).Value = "Ngày \n (ngày/tháng/năm)";
            wb.Range(1, 10, 1, 10).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;**/

            /**int row = 2;
            string path = Server.MapPath("/Images/kiennghi_example.xls");
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(path);
            Worksheet sheet = workbook.Worksheets[0];
            DataTable db = sheet.ExportDataTable();
            int count_row = db.Rows.Count;
            if (count_row > 0)
            {
                for (int i = 0; i < count_row; i++)
                {
                    if (db.Rows[i][0].ToString() != "")
                    {
                        wb.Cell(row, 1).Value = db.Rows[i][0].ToString();
                        wb.Range(row, 1, row, 1).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(row, 2).Value = db.Rows[i][1].ToString();
                        wb.Range(row, 2, row, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).
                                                    Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetWrapText(true);
                        wb.Cell(row, 3).Value = db.Rows[i][2].ToString();
                        wb.Range(row, 3, row, 3).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(row, 4).Value = db.Rows[i][3].ToString();
                        wb.Range(row, 4, row, 4).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(row, 5).Value = db.Rows[i][4].ToString();
                        wb.Range(row, 5, row, 5).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(row, 6).Value = db.Rows[i][5].ToString();
                        wb.Range(row, 6, row, 6).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(row, 7).Value = db.Rows[i][6].ToString();
                        wb.Range(row, 7, row, 7).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(row, 8).Value = db.Rows[i][7].ToString();
                        wb.Range(row, 8, row, 8).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(row, 9).Value = db.Rows[i][8].ToString();
                        wb.Range(row, 9, row, 9).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(row, 10).Value = db.Rows[i][9].ToString();
                        wb.Range(row, 10, row, 10).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        //wb.Cell(row, 11).Value = db.Rows[i][10].ToString();
                        //wb.Range(row, 11, row, 11).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                        row++;
                    }

                }
            }**/
            return wb;
        }
        public IXLWorksheet Sheet_DanhMucNguonKienNghi(XLWorkbook w_b)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IHIENTHI", 1); dic.Add("IPARENT", 0); dic.Add("IDELETE", 0);
            var tinhthanh = _thietlap.Get_Nguonkiennghi();
            var wb = w_b.Worksheets.Add("DanhMucTinhThanh");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 11;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(1).Width = 5;
            wb.Column(2).Width = 40;
            wb.Row(1).Height = 30;
            wb.Cell(1, 1).Value = "Mã";
            wb.Range(1, 1, 1, 1).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188))
                                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 2).Value = "Tên nguồn kiến nghị";
            wb.Range(1, 2, 1, 2).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).
                                                    Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            
            int row = 2; 
            foreach (var c in tinhthanh)
            {
                wb.Cell(row, 1).Value = c.CCODE;
                wb.Range(row, 1, row, 1).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(row, 2).Value = c.CTEN;
                wb.Range(row, 2, row, 2).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                
                row++; 
            }
            return wb;
        }
        public IXLWorksheet Sheet_DanhMucThamQuyen(XLWorkbook w_b)
        {
            var wb = w_b.Worksheets.Add("DanhMucThamQuyen");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 11;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(1).Width = 8;
            wb.Column(2).Width = 50;
            wb.Column(3).Width = 15;
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
            wb.Cell(1, 3).Value = "Nhóm";
            wb.Range(1, 3, 1, 3).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            int row = 2; int stt = 1;
            decimal id_coquan = 0;
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            var coquan = _kiennghi.GetAll_CoQuanByParam(donvi).OrderByDescending(c=>c.CTYPE).ToList();
            
            foreach (var c in coquan)
            {
                if(id_coquan != c.ICOQUAN)
                {
                    wb.Cell(row, 1).Value = c.ICOQUAN;
                    wb.Range(row, 1, row, 1).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    wb.Cell(row, 2).Value = c.CTEN;
                    wb.Range(row, 2, row, 2).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 3).Value = c.CTYPE;
                    wb.Range(row, 3, row, 3).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    row++; stt++;
                }
                id_coquan = c.ICOQUAN;
            }
            return wb;
        }
        public IXLWorksheet Sheet_DanhMucLinhVuc(XLWorkbook w_b)
        {
            var wb = w_b.Worksheets.Add("DanhMucLinhVuc");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 11;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(1).Width = 10;
            wb.Column(2).Width = 20;
            wb.Column(3).Width = 60;
            
            wb.Row(1).Height = 30;
            wb.Cell(1, 1).Value = "STT";
            wb.Range(1, 1, 1, 1).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188))
                                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 2).Value = "Mã lĩnh vực";
            wb.Range(1, 2, 1, 2).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).
                                                    Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 3).Value = "Tên lĩnh vực";
            wb.Range(1, 3, 1, 3).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
           

            int row = 2; int stt = 0; int sttsub = 0;
            var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            foreach (var c in linhvuc)
            {
                wb.Cell(row, 1).SetDataType(XLDataType.Text);
                wb.Cell(row, 1).SetValue(c.CCODE);
                wb.Range(row, 1, row, 1).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(row, 2).Value = c.MALINHVUC;
                wb.Range(row, 2, row, 2).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wb.Cell(row, 3).Value = c.CTEN;
                var ccode = c.CCODE.Split('.');
                if(ccode.Length == 1 ) wb.Range(row, 3, row, 3).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
                else if(ccode.Length >= 3 && ccode[2] != "") wb.Range(row, 3, row, 3).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Italic = true;
                else if (ccode.Length >= 2 && ccode[1] != "") wb.Range(row, 3, row, 3).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                row++;
            }
            return wb;
        }
        public IXLWorksheet Sheet_KeHoachCTTXCT(XLWorkbook w_b)
        {
            var chuongtrinh = _kiennghi.List_PRC_CHUONGTRINH_TXCT_EXPORT(ID_KyHop_HienTai());
            var wb = w_b.Worksheets.Add("KeHoachCTTXCT");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 11;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Column(1).Width = 5;
            wb.Column(2).Width = 15;
            wb.Column(3).Width = 20;
            wb.Column(4).Width = 20;
            wb.Column(5).Width = 40;
            wb.Column(6).Width = 20;
            wb.Column(7).Width = 20;
            wb.Column(8).Width = 15;
            wb.Column(9).Width = 15;
            //wb.Column(8).Style.NumberFormat.Format = "mm/dd/yyyy";
            //wb.Column(9).Style.NumberFormat.Format = "mm/dd/yyyy";
            wb.Cell(1, 1).Value = "STT";
            wb.Range(1, 1, 1, 1).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232))
                                            .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 2).Value = "Mã Tỉnh";
            wb.Range(1, 2, 1, 2).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).
                                                    Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 3).Value = "Tên Tỉnh";
            wb.Range(1, 3, 1, 3).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 4).Value = "Kế hoạch số";
            wb.Range(1, 4, 1, 4).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).
                                                    Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 5).Value = "Nội dung";
            wb.Range(1, 5, 1, 5).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 6).Value = "Kỳ họp";
            wb.Range(1, 6, 1, 6).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            wb.Cell(1, 7).Value = "Trước kỳ họp";
            wb.Range(1, 7, 1, 7).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 8).Value = "Từ ngày";
            wb.Range(1, 8, 1, 8).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;
            wb.Cell(1, 9).Value = "Đến ngày";
            wb.Range(1, 9, 1, 9).Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(232, 232, 232)).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.Bold = true;

            int row = 2; int stt = 1;
            var hinhthuc = new List<TruocKyHop>
            {
                new TruocKyHop { ten = "Trước kỳ họp", value = 1, class_span = "span5" },
                new TruocKyHop { ten = "Sau kỳ họp", value = 0, class_span = "span4" },
                new TruocKyHop { ten = "Khác", value = 2, class_span = "span3" }
            };

            foreach (var c in chuongtrinh)
            {
                wb.Cell(row, 1).Value = stt;
                wb.Range(row, 1, row, 1).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(row, 2).Value = c.MA_COQUAN;
                wb.Range(row, 2, row, 2).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(row, 3).Value = c.TEN_COQUAN;
                wb.Range(row, 3, row, 3).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                if (c.KEHOACH_SO != null)
                {
                    int id_truockyhop = (int)c.TRUOCKYHOP;
                    string truockyhop = hinhthuc.Where(x => x.value == id_truockyhop).FirstOrDefault().ten;
                    wb.Cell(row, 4).Value = c.KEHOACH_SO;
                    wb.Range(row, 4, row, 4).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 5).Value = c.NOIDUNG;
                    wb.Range(row, 5, row, 5).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 6).Value = c.KYHOP;
                    wb.Range(row, 6, row, 6).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 7).Value = truockyhop;
                    wb.Range(row, 7, row, 7).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 8).Value = func.ConvertDateVN(c.NGAYBATDAU.ToString());
                    wb.Range(row, 8, row, 8).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 9).Value = func.ConvertDateVN(c.NGAYKETTHUC.ToString());
                    wb.Range(row, 9, row, 9).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                }
                else
                {
                    wb.Cell(row, 4).Value = "";
                    wb.Range(row, 4, row, 4).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 5).Value = "";
                    wb.Range(row, 5, row, 5).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 6).Value = "";
                    wb.Range(row, 6, row, 6).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 7).Value = "";
                    wb.Range(row, 7, row, 7).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 8).Value = "";
                    wb.Range(row, 8, row, 8).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    wb.Cell(row, 9).Value = "";
                    wb.Range(row, 9, row, 9).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                }

                row++; stt++;
            }
            return wb;
        }

        public ActionResult Download_Mau_Import()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                //var coquan = _kiennghi.List_PRC_COQUAN_LINHVUC();
                XLWorkbook w_b = new XLWorkbook();
                Sheet_DanhMucKiennghi(w_b);
                Sheet_DanhMucLinhVuc(w_b);
                Sheet_DanhMucThamQuyen(w_b);
                Sheet_DanhMucNguonKienNghi(w_b);
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=kiennghi_example.xlsx");
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
                log.Log_Error(ex, "Download mẫu import kiến nghị");
            }
            return null;
        }
        public ActionResult Download_Baocaokiennghi()
        {
            if (!CheckAuthToken()) { return null; }
            KN_TONGHOP baocaotonghopkn = new KN_TONGHOP();
            // Tham số
            int iKyHop = ID_KyHop_HienTai();
            int iDoan = 0; int iDonViXuLy_Parent = 0; int iDonViXuLy = 0;
            if (Request["iDonViXuLy_Parent"] != null) { iDonViXuLy_Parent = Convert.ToInt32(Request["iDonViXuLy_Parent"]); }
            if (Request["iDonViXuLy"] != null) { iDonViXuLy = Convert.ToInt32(Request["iDonViXuLy"]); }
            if (Request["iDoan"] != null) { iDoan = Convert.ToInt32(Request["iDoan"]); }
            if (Request["iKyHop"] != null) { iKyHop = Convert.ToInt32(Request["iKyHop"]); }
            //
            baocaotonghopkn.IKYHOP = iKyHop;
            baocaotonghopkn.ITHAMQUYENDONVI = iDonViXuLy;
            baocaotonghopkn.IDONVITONGHOP = iDoan;
            baocaotonghopkn.ITINHTRANG = (int)TrangThai_TongHop.DaChuyenBanDanNguyen;
            var list = _kiennghi.List_PRC_TONGHOP_KIENNGHI(baocaotonghopkn, iDonViXuLy_Parent, 1, Int32.MaxValue);
            list = list.Where(x => x.ID_GOP <= 0).ToList();
            XLWorkbook w_b = new XLWorkbook();

            var wb = w_b.Worksheets.Add("Số liệu tỉnh");
            wb.PageSetup.FitToPages(1, 1);
            wb.Style.Font.FontSize = 13;
            wb.Column(1).Width = 5;
            wb.Style.Font.FontName = "Times New Roman";
            wb.Cell(1, 1).Value = "Tập hợp KIẾN NGHỊ CÁC ĐOÀN CHUYỂN ĐẾN";
            wb.Range(1, 1, 1, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(2, 1).Value = "";
            wb.Range(2, 1, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            wb.Cell(3, 1).Value = Server.HtmlDecode(kn.Get_TenKyHop((int)baocaotonghopkn.IKYHOP)) + " - " + Server.HtmlDecode(kn.Get_TenKhoaHop_By_IDKyHop((int)baocaotonghopkn.IKYHOP));
            wb.Range(3, 1, 3, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Font.Bold = true;
            string thamquyen_giaiquyet = ""; string thamquyen_giaiquyet_parent = "";
            if (iDonViXuLy != 0)
            {
                thamquyen_giaiquyet = _kiennghi.HienThiThongTinCoQuan(iDonViXuLy).CTEN;
            }
            if (iDonViXuLy_Parent != 0)
            {
                thamquyen_giaiquyet_parent = _kiennghi.HienThiThongTinCoQuan(iDonViXuLy_Parent).CTEN;
            }
            if (thamquyen_giaiquyet_parent != "" && thamquyen_giaiquyet != "")
            {
                thamquyen_giaiquyet = thamquyen_giaiquyet_parent + " - " + thamquyen_giaiquyet_parent;
            }
            else
            {
                if (thamquyen_giaiquyet_parent != "") { thamquyen_giaiquyet = thamquyen_giaiquyet_parent; }
            }
            if (thamquyen_giaiquyet != "")
            {
                wb.Cell(4, 1).Value = " Thẩm quyền giải quyết: " + Server.HtmlDecode(thamquyen_giaiquyet);
                wb.Range(4, 1, 4, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;
            }


            // TIÊU ĐỀ
            wb.Column(1).Width = 5;
            wb.Column(2).Width = 45;
            wb.Column(3).Width = 20;
            wb.Column(4).Width = 20;
            wb.Column(5).Width = 20;
            wb.Column(6).Width = 30;
            wb.Cell(5, 1).Value = "STT";
            wb.Range(5, 1, 5, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 2).Value = "Nội dung";
            wb.Range(5, 2, 5, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 3).Value = "Tỉnh/Thành phố";
            wb.Range(5, 3, 5, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 4).Value = "Lĩnh vực";
            wb.Range(5, 4, 5, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 5).Value = "Ngày Tập hợp";
            wb.Range(5, 5, 5, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                               .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            wb.Cell(5, 6).Value = "Đơn vị thẩm quyền";
            wb.Range(5, 6, 5, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            // END TIÊU ĐỀ

            // GIÁ TRỊ CÁC HÀNG
            int i = 6;
            int stt = 1;
            if (list != null && list.Count() > 0)
            {
                List<decimal> list_id_tonghop = new List<decimal>();
                foreach (var x in list)
                {
                    if (!list_id_tonghop.Contains(x.ITONGHOP))
                    {
                        list_id_tonghop.Add(x.ITONGHOP);
                        wb.Cell(i, 1).Value = stt;
                        wb.Range(i, 1, i, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(i, 2).Value = Server.HtmlDecode(x.NOIDUNG_TONGHOP);
                        wb.Range(i, 2, i, 2).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(i, 3).Value = Server.HtmlDecode(x.TEN_DONVITONGHOP);
                        wb.Range(i, 3, i, 3).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                        wb.Cell(i, 4).Value = Server.HtmlDecode(x.TEN_LINHVUC_TONGHOP);
                        wb.Range(i, 4, i, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                   .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(i, 5).Value = func.ConvertDateVN(x.NGAY_TONGHOP.ToString());
                        wb.Range(i, 5, i, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        wb.Cell(i, 6).Value = Server.HtmlDecode(x.TEN_THAMQUYEN_DONVI_TONGHOP);
                        wb.Range(i, 6, i, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                   .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        i++;
                        stt++;
                    }

                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=tonghopkiennghi.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                w_b.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null;
        }
        public ActionResult Download_Tracuu()
        {
            if (!CheckAuthToken()) { return null; }
            try
            {
                UserInfor u_info = GetUserInfor();
                KN_KIENNGHI baocaotonghopkn = new KN_KIENNGHI();
                baocaotonghopkn.ITHAMQUYENDONVI = Convert.ToInt32(Request["iThamQuyenDonVi"]);
                baocaotonghopkn.IDONVITIEPNHAN = Convert.ToInt32(Request["iDonViTiepNhan"]);
                baocaotonghopkn.IKYHOP = Convert.ToInt32(Request["iKyHop"]);
                baocaotonghopkn.ILINHVUC = Convert.ToInt32(Request["iLinhVuc"]);
                baocaotonghopkn.CNOIDUNG = func.RemoveTagInput(Request["cNoiDung"]);
                baocaotonghopkn.IUSER = Convert.ToInt32(Request["iUser_Capnhat"]);
                DateTime dNgayNhan_from = DateTime.MinValue; if (Request["dNgayNhan_from"] != null && Request["dNgayNhan_from"] != "") { dNgayNhan_from = Convert.ToDateTime(func.ConvertDateToSql(Request["dNgayNhan_from"])); }
                DateTime dNgayNhan_to = DateTime.MaxValue; if (Request["dNgayNhan_to"] != null && Request["dNgayNhan_to"] != "") { dNgayNhan_to = Convert.ToDateTime(func.ConvertDateToSql(Request["dNgayNhan_to"])); }
                DateTime dNgayTongHop_from = DateTime.MinValue; if (Request["dNgayTongHop_from"] != null && Request["dNgayTongHop_from"] != "") { dNgayTongHop_from = Convert.ToDateTime(func.ConvertDateToSql(Request["dNgayTongHop_from"])); }
                DateTime dNgayTongHop_to = DateTime.MaxValue; if (Request["dNgayTongHop_to"] != null && Request["dNgayTongHop_to"] != "") { dNgayTongHop_to = Convert.ToDateTime(func.ConvertDateToSql(Request["dNgayTongHop_to"])); }
                DateTime dNgayTraLoi_from = DateTime.MinValue; if (Request["dNgayTraLoi_from"] != null && Request["dNgayTraLoi_from"] != "") { dNgayTraLoi_from = Convert.ToDateTime(func.ConvertDateToSql(Request["dNgayTraLoi_from"])); }
                DateTime dNgayTraLoi_to = DateTime.MaxValue; if (Request["dNgayTraLoi_to"] != null && Request["dNgayTraLoi_to"] != "") { dNgayTraLoi_to = Convert.ToDateTime(func.ConvertDateToSql(Request["dNgayTraLoi_to"])); }
                int iTinhTrang = -1; if (Request["iTinhTrang"] != null) { iTinhTrang = Convert.ToInt32(Request["iTinhTrang"]); }
                int tinhtrang_from = 2;
                int tinhtrang_to = 7;
                int ketqua_giamsat = -1;
                if (u_info.tk_action.is_chuyenvien)
                {
                    tinhtrang_from = 3; tinhtrang_to = 4;
                }
                int datraloi = -1;//tất cả; 0: chưa trả lời; 1 đã trả lời
                if (iTinhTrang == 1)//chuyển BDN
                {
                    tinhtrang_to = 4;
                }
                if (iTinhTrang == 2)//chuyển địa phương; cơ quan thẩm quyền giải quyết
                {
                    tinhtrang_from = 3; tinhtrang_to = 4;
                }
                if (iTinhTrang == 3)//đang xử lý
                {
                    tinhtrang_from = 4; tinhtrang_to = 4;
                    datraloi = 0;
                }
                if (iTinhTrang == 4)//đã có trả lời
                {
                    tinhtrang_from = 4; tinhtrang_to = 4;
                    datraloi = 1;
                }
                if (iTinhTrang == 5)//kiến nghị trùng
                {
                    tinhtrang_from = 7; tinhtrang_to = 7;
                }
                if (iTinhTrang == 6)//tồn kỳ họp
                {
                    tinhtrang_from = 4; tinhtrang_to = 4;
                    datraloi = 1; ketqua_giamsat = 0;
                }
                List<PRC_KIENNGHI_LIST_TRACUU> list = new List<PRC_KIENNGHI_LIST_TRACUU>();
                if (iTinhTrang == 5)//tra cứu trùng
                {
                    list = _kiennghi.PRC_TRACUU_TRUNG(baocaotonghopkn, 0, dNgayNhan_from, dNgayNhan_to, 1, Int32.MaxValue);
                }
                else//tra cứu
                {
                    list = _kiennghi.PRC_KIENNGHI_LIST_TRACUU(baocaotonghopkn, 0, dNgayNhan_from,
                    dNgayNhan_to, dNgayTongHop_from, dNgayTongHop_to, dNgayTraLoi_from, dNgayTraLoi_to,
                    tinhtrang_from, tinhtrang_to, ketqua_giamsat, datraloi, 1, Int32.MaxValue);
                }
                XLWorkbook w_b = new XLWorkbook();
                var wb = w_b.Worksheets.Add("Báo cáo tra cứu kiến nghị");
                // HÀNG TIÊU ĐỀ
                wb.PageSetup.FitToPages(1, 1);
                wb.Style.Font.FontName = "Times New Roman";
                wb.Style.Font.FontSize = 13;
                wb.Column(1).Width = 5;

                wb.Cell(1, 1).Value = "TRA CỨU KIẾN NGHỊ";
                wb.Range(1, 1, 1, 9).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.Bold = true;

                // END HÀNG TIÊU ĐỀ
                // TIÊU ĐỀ
                wb.Column(1).Width = 5;
                wb.Column(2).Width = 35;
                wb.Column(3).Width = 20;
                wb.Column(4).Width = 20;
                wb.Column(5).Width = 25;
                wb.Column(6).Width = 15;
                wb.Column(7).Width = 15;
                wb.Column(8).Width = 15;
                wb.Column(9).Width = 15;

                wb.Cell(2, 1).Value = "STT";
                wb.Range(2, 1, 2, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                   .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(2, 2).Value = "Nội dung";
                wb.Range(2, 2, 2, 2).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                   .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(2, 3).Value = "Địa phương";
                wb.Range(2, 3, 2, 3).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(2, 4).Value = "Trạng thái xử lý";
                wb.Range(2, 4, 2, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                   .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(2, 5).Value = "Đơn vị thẩm quyền";
                wb.Range(2, 5, 2, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(2, 6).Value = "Ngày tiếp nhận";
                wb.Range(2, 6, 2, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(2, 7).Value = "Ngày Tập hợp";
                wb.Range(2, 7, 2, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(2, 8).Value = "Ngày chuyển";
                wb.Range(2, 8, 2, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                wb.Cell(2, 9).Value = "Ngày trả lời";
                wb.Range(2, 9, 2, 9).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                  .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                // END TIÊU ĐỀ
                // HÀNG GIÁ TRỊ TRA CỨU
                int i = 3;
                int stt = 1;
                List<decimal> list_id_kiennghi = new List<decimal>();
                var list1 = list.Where(x => x.ID_GOP <= 0).ToList();
                foreach (var x in list1)
                {
                    if (!list_id_kiennghi.Contains(x.ID_KIENNGHI))
                    {
                        list_id_kiennghi.Add(x.ID_KIENNGHI);
                        wb.Cell(i, 1).Value = stt;
                        wb.Range(i, 1, i, 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin); ;
                        wb.Cell(i, 2).Value = Server.HtmlDecode(x.NOIDUNG_KIENNGHI);
                        wb.Range(i, 2, i, 2).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        string ten_donvi_gop = x.TEN_DONVITIEPNHAN;
                        if (x.ID_GOP == -1)
                        {
                            ten_donvi_gop = x.TENDONVITIEPNHAN_GOP;
                            //doan_tiepnhan = "<td class='tcenter'>" + ten_donvi_gop + "</td>";
                        }
                        wb.Cell(i, 3).Value = ten_donvi_gop;
                        wb.Range(i, 3, i, 3).Merge().Style.Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        string tinhtrang = "";
                        if (x.TRALOI_NGAYBANHANH != DateTime.MinValue)
                        {
                            tinhtrang = "Đã có trả lời";
                        }
                        else
                        {
                            tinhtrang = "Đang xử lý";
                        }
                        //if(x.t)
                        wb.Cell(i, 4).Value = tinhtrang;
                        wb.Range(i, 4, i, 4).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                        wb.Cell(i, 5).Value = Server.HtmlDecode(x.TEN_THAMQUYEN_DONVI);
                        wb.Range(i, 5, i, 5).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        string ngaycapnhat = "";
                        if (x.NGAYCAPNHAT != DateTime.MinValue)
                        {
                            ngaycapnhat = func.ConvertDateVN(x.NGAYCAPNHAT.ToString());
                        }
                        wb.Cell(i, 6).Value = ngaycapnhat;
                        wb.Range(i, 6, i, 6).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        string ngaytonghop = "";
                        if (x.NGAYTONGHOP != DateTime.MinValue)
                        {
                            ngaytonghop = func.ConvertDateVN(x.NGAYTONGHOP.ToString());
                        }
                        wb.Cell(i, 7).Value = ngaytonghop;
                        wb.Range(i, 7, i, 7).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        string ngaychuyen = "";
                        if (x.NGAY_CHUYEN != DateTime.MinValue)
                        {
                            ngaychuyen = func.ConvertDateVN(x.NGAY_CHUYEN.ToString());
                        }
                        wb.Cell(i, 8).Value = ngaychuyen;
                        wb.Range(i, 8, i, 8).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        string ngaytraloi = "";
                        if (x.TRALOI_NGAYBANHANH != DateTime.MinValue)
                        {
                            ngaytraloi = func.ConvertDateVN(x.TRALOI_NGAYBANHANH.ToString());
                        }
                        wb.Cell(i, 9).Value = ngaytraloi;
                        wb.Range(i, 9, i, 9).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        i++;
                        stt++;
                    }
                }
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=baocaotracuu.xlsx");
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
                log.Log_Error(ex, "Download nội dung tra cứu kiến nghị");
            }
            return null;
        }
        public ActionResult Download_kiennghi_bytonghop()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int id = Convert.ToInt32(HashUtil.Decode_ID(Request["id"], Request.Cookies["url_key"].Value));
                KN_TONGHOP tonghop = _kiennghi.Get_Tonghop(id);
                List<PRC_KIENNGHI_BYTONGHOP> kiennghi;
                if (tonghop.IDONVITONGHOP == ID_Ban_DanNguyen)//tổng hợp của BDN
                {
                    kiennghi = _kiennghi.PRC_LIST_KIENNGHI_BY_TONGHOP_KIENNGHI(0, id);
                }
                else//tổng hợp của đoàn
                {
                    kiennghi = _kiennghi.PRC_LIST_KIENNGHI_BY_TONGHOP_KIENNGHI(id, 0);
                }
                ViewData["list"] = kn.List_KienNghiByTonghop_download(kiennghi);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/force-download;";
                Response.AddHeader("Content-Disposition", "attachment; filename=kiennghi.doc");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "";
                return View();
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách import kiến nghị");
                return View("../Home/Error_Exception");
            }

        }

        public ActionResult Baocao_DanhMucCuTri1A(string lstKyHop, string lstNguonKN, string lstLinhVuc, int loaibaocao, string ext = "pdf")
        {
            string fileName = "";
            var templatePath = "";
            if (loaibaocao == 1)
            {
                fileName = string.Format("{0}.{1}", "Baocao1A(TongHop)_DanhMucKienNghiCuTri", ext);
                templatePath = ReportConstants.KienNghi_TongHop_BaoCao1A_DanhMucCuTri;
            }
            if (loaibaocao == 2)
            {
                fileName = string.Format("{0}.{1}", "Baocao1A(HĐND)-DanhMucKienNghiCuTri", ext);
                templatePath = ReportConstants.KienNghi_HĐND_BaoCao1A_DanhMucCuTri;
            }
            if (loaibaocao == 3)
            {
                fileName = string.Format("{0}.{1}", "Baocao1A(QH)-DanhMucKienNghiCuTri", ext);
                templatePath = ReportConstants.KienNghi_QH_BaoCao1A_DanhMucCuTri;
            }
            ExcelFile xls = ExportReportDanhMucCuTri1A(lstKyHop,lstNguonKN,lstLinhVuc, loaibaocao, templatePath);
            return Print(xls, ext, fileName);
        }

        public ExcelFile ExportReportDanhMucCuTri1A(string lstKyHop, string lstNguonKN, string lstLinhVuc, int loaibaocao, string templatePath)
        {

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(templatePath));
            FlexCelReport fr = new FlexCelReport();
            List<KIENNGHICUTRI_1A> dt = new List<KIENNGHICUTRI_1A>();
            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }
            if (loaibaocao == 1)
            {
                //Đoàn đại biểu quốc hội
                List<KIENNGHICUTRI_1A> dt_QH = new List<KIENNGHICUTRI_1A>();
                dt_QH = _kienNghiReport.getReportKienNghiCuTriBaoCao1A(lstKyHop, lstNguonKN, lstLinhVuc, 2, iUser);
                List<KIENNGHICUTRI_1A> dt_HĐND = new List<KIENNGHICUTRI_1A>();
                dt_HĐND = _kienNghiReport.getReportKienNghiCuTriBaoCao1A(lstKyHop, lstNguonKN, lstLinhVuc, 3, iUser);
                fr.AddTable<KIENNGHICUTRI_1A>("listQH", dt_QH);
                fr.AddTable<KIENNGHICUTRI_1A>("listHDND", dt_HĐND);
            }
            else
            {
                dt = _kienNghiReport.getReportKienNghiCuTriBaoCao1A(lstKyHop, lstNguonKN, lstLinhVuc, loaibaocao, iUser);
            }

            fr.AddTable<KIENNGHICUTRI_1A>("dt", dt);
            fr.SetValue(new
            {
                To = 1
            });
            fr.UseForm(this).Run(Result);
            return Result;

        }
        public ActionResult BaoCao_TongHopYKienCuTri1B(string lstKyHop, string lstNguonKN, string lstLinhVuc, int loaibaocao, string ext = "pdf")
        {
            string fileName = "";
            var templatePath = "";
            if (loaibaocao == 1)
            {
                fileName = string.Format("{0}.{1}", "Baocao1B(TongHop)_TongHopYKienCuTri", ext);
                templatePath = ReportConstants.rpt_KN_TongHop_BaoCao1BTongHopYKienCuTri;
            }    
            if (loaibaocao == 2)
            {
                fileName = string.Format("{0}.{1}", "Baocao1B(HĐND)_TongHopYKienCuTri", ext);
                templatePath = ReportConstants.rpt_KN_HĐND_BaoCao1BTongHopYKienCuTri;
            }
            if (loaibaocao == 3)
            {
                fileName = string.Format("{0}.{1}", "Baocao1B(QH)_TongHopYKienCuTri", ext);
                templatePath = ReportConstants.rpt_KN_QH_BaoCao1BTongHopYKienCuTri;
            }
            ExcelFile xls = ExportReportTongHopYKienCuTri1B(lstKyHop, lstNguonKN, lstLinhVuc, loaibaocao, templatePath);
            return Print(xls, ext, fileName);
        }

        public ExcelFile ExportReportTongHopYKienCuTri1B(string lstKyHop, string lstNguonKN, string lstLinhVuc, int loaibaocao, string templatePath)
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
            List<KIENNGHICUTRI_1A> dt = new List<KIENNGHICUTRI_1A>();
            List<KIENNGHICUTRI_1A> lst_kn = new List<KIENNGHICUTRI_1A>();
            string skyhop = "KỲ HỌP...";
            string skhoa = "KHÓA...";
            var lstQHKyHop = _thietlap.Get_List_Quochoi_Kyhop().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            List<LINHVUC_COQUAN> lstlinhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            string[] arrKyHop = lstKyHop.Split(',');
            if (arrKyHop.Length == 1 && arrKyHop[0] != "")
            {
                QUOCHOI_KYHOP kyhop = lstQHKyHop.Where(x => x.IKYHOP == Convert.ToInt32(arrKyHop[0])).FirstOrDefault();
                if (kyhop != null)
                {
                    skyhop = kyhop.CTEN.ToUpper();
                    QUOCHOI_KHOA khoa = _thietlap.Get_List_Quochoi_Khoa().Where(x => x.IDELETE == 0 && x.IKHOA == kyhop.IKHOA).FirstOrDefault();
                    skhoa = khoa.CTEN.ToUpper();
                }
            }
            if (loaibaocao == 1)
            {
                //QH - 
                dt.Add(new KIENNGHICUTRI_1A { TT = "A. ĐOÀN ĐẠI BIỂU QUỐC HỘI", DIAPHUONG = "", SOKIENNGHI = "", NOIDUNGKIENNGHI = "", GHICHU = "", ISBOLD = 1, ISMERGE = true, ISTITLE = 1 });
                var lst_kn_hdnd = _kienNghiReport.getReportTongHopYKienCuTri1B(lstKyHop, lstNguonKN, lstLinhVuc, 1, iUser);
                getLinhVucCha(dt, lst_kn_hdnd, lstlinhvuc);
                //HDND
                dt.Add(new KIENNGHICUTRI_1A { TT = "B. THƯỜNG TRỰC HĐND TỈNH", DIAPHUONG = "", SOKIENNGHI = "", NOIDUNGKIENNGHI = "", GHICHU = "", ISBOLD = 1, ISMERGE = true, ISTITLE = 1 });
                var lst_kn_qh = _kienNghiReport.getReportTongHopYKienCuTri1B(lstKyHop, lstNguonKN, lstLinhVuc, 0, iUser);
                getLinhVucCha(dt, lst_kn_qh, lstlinhvuc);

            }
            if (loaibaocao == 2)
            {
                var lst_kn_hdnd = _kienNghiReport.getReportTongHopYKienCuTri1B(lstKyHop, lstNguonKN, lstLinhVuc, 1, iUser);
                getLinhVucCha(dt, lst_kn_hdnd, lstlinhvuc);
            }
            if (loaibaocao == 3)
            {
                var lst_kn_qh = _kienNghiReport.getReportTongHopYKienCuTri1B(lstKyHop, lstNguonKN, lstLinhVuc, 0, iUser);
                getLinhVucCha(dt, lst_kn_qh, lstlinhvuc);
            }


            fr.AddTable<KIENNGHICUTRI_1A>("dt", dt);
            fr.SetValue(new
            {
                To = 1,
                skyhop = skyhop,
                skhoa = skhoa

            });
            fr.UseForm(this).Run(Result);
            //Thực hiện mercel
            int rowstart = 9;
            int colstart = 3;
            int collst = 11;
            int index = 0;
            foreach (var item in dt)
            {
                if (item.ISMERGE)
                {
                    if (item.ISTITLE == 1)
                    {
                        Result.MergeCells(rowstart + index, 2, rowstart + index, collst);
                    }
                    else
                    {
                        Result.MergeCells(rowstart + index, colstart, rowstart + index, collst);
                    }
                }
                index++;
            }

            return Result;
        }

        public void getLinhVucCha(List<KIENNGHICUTRI_1A> lstData, List<KIENNGHICUTRI_1A> lstKN, List<LINHVUC_COQUAN> lstlinhvuc)
        {
            var lstlinhvuc_parent = lstlinhvuc.Where(x => x.IPARENT == 0).ToList();
            foreach (var item in lstlinhvuc_parent)
            {
                var lstlinhvuc_child = lstlinhvuc.Where(x => x.IPARENT == item.ILINHVUC).ToList();

                lstData.Add(new KIENNGHICUTRI_1A { TT = NumberExtension.ToRoman(Convert.ToInt16(item.ILINHVUC)), DIAPHUONG = item.CTEN.ToUpper(), SOKIENNGHI = "", NOIDUNGKIENNGHI = "", GHICHU = "", ISBOLD = 1, ISMERGE = true });
                getLinhVuc_KienNghi(lstData, Convert.ToInt16(item.ILINHVUC), lstKN, lstlinhvuc_child);
            }
        }

        public void getLinhVuc_KienNghi(List<KIENNGHICUTRI_1A> lstData, int iLinhVuc, List<KIENNGHICUTRI_1A> lstKN, List<LINHVUC_COQUAN> lstlinhvuc_child, string stt_parent = "")
        {
            var lst_kn = lstKN.Where(x => x.ILINHVUC == iLinhVuc).ToList();
            foreach (var kn in lst_kn)
            {
                lstData.Add(new KIENNGHICUTRI_1A { TT = "", DIAPHUONG = "-" + kn.DIAPHUONG, SOKIENNGHI = kn.SOKIENNGHI, NOIDUNGKIENNGHI = kn.NOIDUNGKIENNGHI, GHICHU = kn.GHICHU, ISBOLD = 0, ISMERGE = false });
            }
            if (lstlinhvuc_child.Count > 0)
            {
                int index = 1;
                int stt_cnt = 1;
                foreach (var ls in lstlinhvuc_child)
                {
                    string stt = stt_parent == "" ? (stt_parent + index.ToString()) : (stt_parent + "." + index.ToString());
                    int isbold = stt_parent == "" ? 1 : 0;
                    var lstChild_2 = lstlinhvuc_child.Where(x => x.IPARENT == ls.ILINHVUC).ToList();
                    var lst_kn_clild = lstKN.Where(x => x.ILINHVUC == ls.ILINHVUC).ToList();
                    if(lst_kn_clild.Count > 0) {
                        lstData.Add(new KIENNGHICUTRI_1A { TT = stt_cnt.ToString(), DIAPHUONG = ls.CTEN, SOKIENNGHI = "", NOIDUNGKIENNGHI = "", GHICHU = "", ISBOLD = isbold, ISMERGE = true });
                        stt_cnt++;
                        }
                    foreach (var kn in lst_kn_clild)
                    {
                        lstData.Add(new KIENNGHICUTRI_1A { TT = "", DIAPHUONG = "-" + kn.DIAPHUONG, SOKIENNGHI = kn.SOKIENNGHI, NOIDUNGKIENNGHI = kn.NOIDUNGKIENNGHI, GHICHU = kn.GHICHU, ISBOLD = 0, ISMERGE = false });
                    }
                    if (lstChild_2 != null)
                    {
                        foreach (var lst_child in lstChild_2)
                        {
                            getLinhVuc_KienNghi(lstData, Convert.ToInt32(lst_child.ILINHVUC), lstKN, lstlinhvuc_child, stt);
                        }

                    }

                    index++;
                }
            }
        }

        public ActionResult BaoCao_TongHopYKienCuTri1B1(string lstKyHop, string lstNguonKN, string lstLinhVuc, int loaibaocao, string ext = "pdf")
        {
            string fileName = "";
            var templatePath = "";
            if (loaibaocao == 1)
            {
                fileName = string.Format("{0}.{1}", "Baocao1B1(TongHop)_TongHopYKienCuTri", ext);
                templatePath = ReportConstants.rpt_KN_TongHop_BaoCao1B1TongHopYKienCuTri;
            }
            if (loaibaocao == 2)
            {
                fileName = string.Format("{0}.{1}", "Baocao1B1(HĐND)_TongHopYKienCuTri", ext);
                templatePath = ReportConstants.rpt_KN_HĐND_BaoCao1B1TongHopYKienCuTri;
            }
            if (loaibaocao == 3)
            {
                fileName = string.Format("{0}.{1}", "Baocao1B1(QH)_TongHopYKienCuTri", ext);
                templatePath = ReportConstants.rpt_KN_QH_BaoCao1B1TongHopYKienCuTri;
            }
            ExcelFile xls = ExportReportTongHopYKienCuTri1B1(lstKyHop, lstNguonKN, lstLinhVuc, loaibaocao, templatePath);
            return Print(xls, ext, fileName);
        }

        public ExcelFile ExportReportTongHopYKienCuTri1B1(string lstKyHop, string lstNguonKN, string lstLinhVuc, int loaibaocao, string templatePath)
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

            List<KIENNGHICUTRI_1B1> dt = new List<KIENNGHICUTRI_1B1>();
            List<KIENNGHICUTRI_1B1> lst_kn = new List<KIENNGHICUTRI_1B1>();
            string skyhop = "KỲ HỌP...";
            string skhoa = "KHÓA...";
            var lstQHKyHop = _thietlap.Get_List_Quochoi_Kyhop().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            List<LINHVUC_COQUAN> lstlinhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            string[] arrKyHop = lstKyHop.Split(',');
            if (arrKyHop.Length == 1 && arrKyHop[0] != "")
            {
                QUOCHOI_KYHOP kyhop = lstQHKyHop.Where(x => x.IKYHOP == Convert.ToInt32(arrKyHop[0])).FirstOrDefault();
                if (kyhop != null)
                {
                    skyhop = kyhop.CTEN.ToUpper();
                    QUOCHOI_KHOA khoa = _thietlap.Get_List_Quochoi_Khoa().Where(x => x.IDELETE == 0 && x.IKHOA == kyhop.IKHOA).FirstOrDefault();
                    skhoa = khoa.CTEN.ToUpper();
                }
            }
            if (loaibaocao == 1)
            {
                //QH - 
                dt.Add(new KIENNGHICUTRI_1B1 { TT = "A. ĐOÀN ĐẠI BIỂU QUỐC HỘI", DIAPHUONG = "", TONGSOKIENNGHI = 0, TYLEKIENNGHI = "", GHICHU = "", ISBOLD = 1, ISMERGE = true, ISTITLE = 1 });
                var lst_kn_hdnd = _kienNghiReport.getReportTongHopYKienCuTri1B1(lstKyHop, lstNguonKN, lstLinhVuc, 0, iUser);
                getLinhVucCha(dt, lst_kn_hdnd, lstlinhvuc);
                //HDND
                dt.Add(new KIENNGHICUTRI_1B1 { TT = "B. THƯỜNG TRỰC HĐND TỈNH", DIAPHUONG = "", TONGSOKIENNGHI = 0, TYLEKIENNGHI = "", GHICHU = "", ISBOLD = 1, ISMERGE = true, ISTITLE = 1 });
                var lst_kn_qh = _kienNghiReport.getReportTongHopYKienCuTri1B1(lstKyHop, lstNguonKN, lstLinhVuc, 1, iUser);
                getLinhVucCha(dt, lst_kn_qh, lstlinhvuc);

            }
            if (loaibaocao == 2)
            {
                var lst_kn_hdnd = _kienNghiReport.getReportTongHopYKienCuTri1B1(lstKyHop, lstNguonKN, lstLinhVuc, 1, iUser);
                getLinhVucCha(dt, lst_kn_hdnd, lstlinhvuc);
            }
            if (loaibaocao == 3)
            {
                var lst_kn_qh = _kienNghiReport.getReportTongHopYKienCuTri1B1(lstKyHop, lstNguonKN, lstLinhVuc, 0, iUser);
                getLinhVucCha(dt, lst_kn_qh, lstlinhvuc);
            }


            fr.AddTable<KIENNGHICUTRI_1B1>("dt", dt);
            fr.SetValue(new
            {
                To = 1,
                skyhop = skyhop,
                skhoa = skhoa

            });
            fr.UseForm(this).Run(Result);
            //Thực hiện mercel
            int rowstart = 9;
            int colstart = 3;
            int collst = 11;
            int index = 0;
            foreach (var item in dt)
            {
                if (item.ISMERGE)
                {
                    if (item.ISTITLE == 1)
                    {
                        Result.MergeCells(rowstart + index, 2, rowstart + index, collst);
                    }
                    else
                    {
                        Result.MergeCells(rowstart + index, colstart, rowstart + index, collst);
                    }
                }
                index++;
            }

            return Result;
        }

        public void getLinhVucCha(List<KIENNGHICUTRI_1B1> lstData, List<KIENNGHICUTRI_1B1> lstKN, List<LINHVUC_COQUAN> lstlinhvuc)
        {
            var lstlinhvuc_parent = lstlinhvuc.Where(x => x.IPARENT == 0).ToList();
            foreach (var item in lstlinhvuc_parent)
            {
                var lstlinhvuc_child = lstlinhvuc.Where(x => x.IPARENT == item.ILINHVUC).ToList();

                lstData.Add(new KIENNGHICUTRI_1B1 { TT = NumberExtension.ToRoman(Convert.ToInt16(item.ILINHVUC)), DIAPHUONG = item.CTEN.ToUpper(), TONGSOKIENNGHI = 0, TYLEKIENNGHI = "", GHICHU = "", ISBOLD = 1, ISMERGE = true });
                getLinhVuc_KienNghi(lstData, Convert.ToInt16(item.ILINHVUC), lstKN, lstlinhvuc_child);
            }
        }

        public void getLinhVuc_KienNghi(List<KIENNGHICUTRI_1B1> lstData, int iLinhVuc, List<KIENNGHICUTRI_1B1> lstKN, List<LINHVUC_COQUAN> lstlinhvuc_child, string stt_parent = "")
        {
            var lst_kn = lstKN.Where(x => x.ILINHVUC == iLinhVuc).ToList();
            var lst_diaphuong = _thietlap.Get_Nguonkiennghi();
            var lstDataNew = new List<KIENNGHICUTRI_1B1>();
            int sumAll = 0;
            foreach (var diaphuong in lst_diaphuong)
            {
                int tongKN = lst_kn.Where(x => x.DIAPHUONG.Equals(diaphuong.CTEN)).Count();
                if (tongKN > 0)
                {
                    lstDataNew.Add(new KIENNGHICUTRI_1B1 { TT = "", DIAPHUONG = "-" + diaphuong.CTEN, TONGSOKIENNGHI = tongKN, TYLEKIENNGHI = "%", GHICHU = "", ISBOLD = 0, ISMERGE = false });
                    sumAll += tongKN;
                }
            }
            foreach (var data in lstDataNew)
            {
                data.TYLEKIENNGHI = string.Format("{0:0.00}%", 100.0 * data.TONGSOKIENNGHI / sumAll);
            }
            lstData.AddRange(lstDataNew);
            if (lstlinhvuc_child.Count > 0)
            {
                int index = 1;
                int stt_cnt = 1;
                foreach (var ls in lstlinhvuc_child)
                {
                    string stt = stt_parent == "" ? (stt_parent + index.ToString()) : (stt_parent + "." + index.ToString());
                    int isbold = stt_parent == "" ? 1 : 0;
                    var lstChild_2 = lstlinhvuc_child.Where(x => x.IPARENT == ls.ILINHVUC).ToList();
                    var lst_kn_clild = lstKN.Where(x => x.ILINHVUC == ls.ILINHVUC).ToList();
                    if (lst_kn_clild.Count > 0)
                    {
                        lstData.Add(new KIENNGHICUTRI_1B1 { TT = stt_cnt.ToString(), DIAPHUONG = ls.CTEN, TONGSOKIENNGHI = 0, TYLEKIENNGHI = "", GHICHU = "", ISBOLD = isbold, ISMERGE = true });
                        stt_cnt++;
                    }
                    lstDataNew = new List<KIENNGHICUTRI_1B1>();
                    sumAll = 0;
                    foreach (var diaphuong in lst_diaphuong)
                    {
                        int tongKN = lst_kn_clild.Where(x => x.DIAPHUONG.Equals(diaphuong.CTEN)).Count();
                        if (tongKN > 0)
                        {
                            lstDataNew.Add(new KIENNGHICUTRI_1B1 { TT = "", DIAPHUONG = "-" + diaphuong.CTEN, TONGSOKIENNGHI = tongKN, TYLEKIENNGHI = "%", GHICHU = "", ISBOLD = 0, ISMERGE = false });
                            sumAll += tongKN;
                        }
                    }
                    foreach (var data in lstDataNew)
                    {
                        data.TYLEKIENNGHI = string.Format("{0:0.00}%", 100.0 * data.TONGSOKIENNGHI / sumAll);
                    }
                    lstData.AddRange(lstDataNew);
                    if (lstChild_2 != null)
                    {
                        foreach (var lst_child in lstChild_2)
                        {
                            getLinhVuc_KienNghi(lstData, Convert.ToInt32(lst_child.ILINHVUC), lstKN, lstlinhvuc_child, stt);
                        }

                    }

                    index++;
                }
            }
        }

        public ActionResult BaoCaoTongHopHuyen(string ext = "pdf")
        {
            try
            {
                if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
                
                string listKyHop = "";
                if (Request["listKyHop"] != "")
                {
                    listKyHop = Request["listKyHop"];
                }
                string listLinhVuc = "";
                if (Request["listLinhVuc"] != "")
                {
                    listLinhVuc = Request.QueryString["listLinhVuc"];
                }
                int iThamQuyenDonVi = 0;
                if (Request["iThamQuyenDonVi"] != null)
                {
                    iThamQuyenDonVi = Convert.ToInt32(Request["iThamQuyenDonVi"]);
                }
                string listHuyen_Xa_ThanhPho = "";
                if (Request["listHuyen_Xa_ThanhPho"] != null)
                {
                    listHuyen_Xa_ThanhPho = Request["listHuyen_Xa_ThanhPho"];
                }
                int iTenBaoCao = 0;
                if (Request["iTenBaoCao"] != null)
                {
                    iTenBaoCao = Convert.ToInt32(Request["iTenBaoCao"].ToString());
                }
                // if (Request.QueryString["iNguonKienNghi"] != "")
                // {
                //     iNguonKienNghi = Request.QueryString["iNguonKienNghi"];
                // }
                // DateTime dTuNgay = new DateTime();
                // if (Request.QueryString["dTuNgay"] != "")
                // {
                //     dTuNgay = DateTime.Parse(Request.QueryString["dTuNgay"].ToString());
                // }
                // DateTime dNgayDen = DateTime.MaxValue;
                // if (Request.QueryString["dNgayDen"] != "")
                // {
                //     dNgayDen = DateTime.Parse(Request.QueryString["dNgayDen"].ToString());
                // }
                string fileName = "";
                fileName = string.Format("{0}.{1}", "BaoCao_TongHop_Huyen", ext);
                var templatePath = "";
                int keydata = 0;
                if (iTenBaoCao == 2)
                {
                    keydata = 2;
                    templatePath = Server.MapPath(ReportConstants.rpt_KN_BaoCao_BaoCaoTongHopHuyenTemplate);
                }
                if (iTenBaoCao == 3)
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KN_HDND_BaoCaoTongHopHuyenTemplate);
                    keydata = 1;
                }
                if (iTenBaoCao == 4)
                {
                    templatePath = Server.MapPath(ReportConstants.rpt_KN_QH_BaoCaoTongHopHuyenTemplate);
                    keydata = 0;
                }

                ExcelFile xls = ExportReportTongHopHuyen(listKyHop, iThamQuyenDonVi, listHuyen_Xa_ThanhPho, listLinhVuc, keydata, iTenBaoCao, templatePath);
                return Print(xls, ext, fileName);

            }
            catch (Exception e)
            {
                log.Log_Error(e);
                return View("../Home/Error_Exception");
            }
        }

        public ExcelFile ExportReportTongHopHuyen(string listKyHop, int iThamQuyenDonVi, string listHuyen_Xa_ThanhPho, string listLinhVuc, int keydata, int iTenBaoCao, string templatePath)
        {
            UserInfor u_info = GetUserInfor();
            int iUser = (int)u_info.user_login.IUSER;
            if (u_info.tk_action.is_lanhdao)
            {
                iUser = 0;
            }
            List<KIENNGHI_REPORT_TONGHOPHUYEN> listKienNghiReport = _kienNghiReport.getReportTongHopHuyen(listKyHop, iThamQuyenDonVi, listHuyen_Xa_ThanhPho, listLinhVuc, iTenBaoCao, keydata, iUser);

            decimal check = 2;
            foreach (var item in listKienNghiReport)
            {
                if (item.IDOITUONGGUI != check)
                {
                    if (item.IDOITUONGGUI == 0)
                    {
                        item.FIRSTTITLE = "I. ĐOÀN ĐẠI BIỂU QUỐC HỘI";
                    }
                    if (item.IDOITUONGGUI == 1)
                    {
                        item.FIRSTTITLE = "II. THƯỜNG TRỰC HỘI ĐỒNG NHÂN DÂN HUYỆN";
                    }
                    check = item.IDOITUONGGUI;
                }
            }
            XlsFile Result = new XlsFile(true);
            Result.Open(templatePath);
            FlexCelReport fr = new FlexCelReport();

            fr.AddTable<KIENNGHI_REPORT_TONGHOPHUYEN>("List", listKienNghiReport);
            fr.UseForm(this).Run(Result);
            return Result;

        }
        public string List_CheckBox_Huyen_ThiXa_ThanhPho(int iDaibieu = 0, string lstKN = "")
        {

            UserInfor u_info = GetUserInfor();
            string str = "";
            string[] arrLstKN = lstKN.Split(',');
            var listHuyen_ThiXa_ThanhPho = _thietlap.Get_Nguonkiennghi().ToList();
            foreach (var item in listHuyen_ThiXa_ThanhPho)
            {
               
                string select = " ";
                if (lstKN == "all")
                {
                    select = " selected ";
                }
                else
                {
                    Boolean isCheck = Array.Exists(arrLstKN, element => element == item.INGUONDON.ToString());
                    if (isCheck)
                    {
                        select = " selected ";
                    }
                }

                str += "<option " + select + " value='" + item.INGUONDON + "'>" + item.CTEN + "</option>";

            }
            return str;
        }
        public string Get_Option_BaoCaoHuyen_By_ID_USERS()
        {
            string str = "";
            //var linhvuc = _kiennghi.GetAll_LinhVuc_CoQuan_By_IDCoQuan();
            //var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            UserInfor u_info = GetUserInfor();
            if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienHDND)
            {
                str += "<option value='3'> PHỤ LỤC IV HĐND: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp Huyện </option>";
            }
            else if ((int)u_info.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
            {
                str += "<option value='4'> PHỤ LỤC IV QH: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp Huyện </option>";
            }
            else
            {
                str += "<option value='2'>PHỤ LỤC IV : Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp Huyện </option>";
                str += "<option value='3'>PHỤ LỤC IV HĐND: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp Huyện </option>";
                str += "<option value='4'>PHỤ LỤC IV QH: Danh mục kiến nghị của cử tri thuộc thẩm quyền cấp Huyện </option>";
            }
            return str;
        }
        public string Get_Option_LinhVucHuyen_ChonNhieu(string lstLV = "")
        {
            var linhvuc = _thietlap.Get_Linhvuc_Coquan_Sorted();
            return kn.Option_LinhvucconquanHuyen_Chonnhieu(linhvuc, lstLV);
        }
        
        public ActionResult Ajax_xuat_tonghop_bandannguyen()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }

            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["listKhoaHop"] = JsonConvert.SerializeObject(_kiennghi.GetAll_KhoaHop().OrderBy(x => x.DBATDAU).ToList());
                ViewData["kyhop"] = List_CheckBox_KyHop();
                string lstNguonKN = "all";
                if (Request["iTruocKyHop"] != null) lstNguonKN = "";
                if (Request["listNguonKN"] != null)
                {
                    lstNguonKN = Request["listNguonKN"];
                }
                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, lstNguonKN);
                string lstLinhVuc = "all";
                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu(lstLinhVuc);
                //ViewData["opt-linhvuc"] = Get_Option_LinhVuc_By_ID_CoQuan();
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                ViewData["opt-tenbaocao"] = Get_Option_BaoCao_By_ID_USERS();
                return PartialView("../Ajax/Kiennghi/xuat_tonghop_bandannguyen");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xuất chương trình tiếp xúc cử tri");
                throw;
            }
        }
        
        public ActionResult Ajax_xuat_tonghop_tinh()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["kyhop"] = List_CheckBox_KyHop();
                string lstNguonKN = "all";
                if (Request["iTruocKyHop"] != null) lstNguonKN = "";
                if (Request["listNguonKN"] != null)
                {
                    lstNguonKN = Request["listNguonKN"];
                }
                List<KN_NGUONDON> nguonkiennghi = _thietlap.Get_Nguonkiennghi();
                ViewData["opt-nguonkiennghi"] = kn.Option_Nguonkiennghi_ChonNhieu(nguonkiennghi, lstNguonKN);
                string lstLinhVuc = "all";
                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu(lstLinhVuc);
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho(0, "all");
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                ViewData["opt-tenbaocao"] = Get_Option_TenBaoCao_Tonghop_Tinh(u_info);
                return PartialView("../Ajax/Kiennghi/xuat_tonghop_tinh");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xuất chương trình tiếp xúc cử tri");
                throw;
            }
        }
        
        public ActionResult Ajax_xuat_tonghop_huyen()
        {
            if (!CheckAuthToken()) { Response.Redirect("/Home/Logout/"); return null; }
            try
            {
                int iKyHop = ID_KyHop_HienTai();
                UserInfor u_info = GetUserInfor();
                ViewData["kyhop"] = List_CheckBox_KyHop();
                ViewData["opt-donvixuly"] = Get_Option_DonViThamQuyen_ByCType((int)ThamQuyen_DiaPhuong.Huyen);
                string lstLinhVuc = "all";
                if (Request["listLinhVuc"] != null)
                {
                    lstLinhVuc = Request["listLinhVuc"];
                }
                ViewData["opt-linhvuc"] = Get_Option_LinhVuc_ChonNhieu(lstLinhVuc);
                ViewData["huyen_thixa_thanhpho"] = List_CheckBox_Huyen_ThiXa_ThanhPho(0, "all");
                ViewData["check-hinhthuc"] = Get_CheckBox_TruocKyHop();
                ViewData["opt-tenbaocao"] = Get_Option_BaoCao_By_ID_USERS();
                return PartialView("../Ajax/Kiennghi/xuat_tonghop_huyen");
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Xuất chương trình tiếp xúc cử tri");
                throw;
            }
        }
    }
}
