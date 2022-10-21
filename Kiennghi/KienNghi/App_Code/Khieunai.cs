using DataAccess.Busineess;
using Entities.Models;
using Entities.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Utilities;
using Utilities.Enums;

namespace KienNghi.App_Code
{
    public class Khieunai : Base
    {
        private Funtions func = new Funtions();
        private Base _base = new Base();
        private BaseBusineess _base_bussiness = new BaseBusineess();
        private KntcBusineess kntc = new KntcBusineess();
        private ThietlapBusineess _thietlap = new ThietlapBusineess();
        private Thietlap tl = new Thietlap();

        private KntcReport kntcrpt = new KntcReport();
        //public string File_View(int id, string type)
        //{
        //    FileuploadRepository _file = new FileuploadRepository();
        //    string str = "";
        //    Dictionary<string, object> _dic = new Dictionary<string, object>();
        //    _dic.Add("ID", id);
        //    _dic.Add("CTYPE", type);
        //    var file = _file.GetAll(_dic).ToList();
        //    if (file.Count() > 0)
        //    {
        //        str += "";
        //        foreach (var f in file)
        //        {
        //            string[] f_ = f.CFILE.Split('/');
        //            //lấy địa chỉ file upload cùng thư mục code
        //            //string file_path = f.CFILE;
        //            //lấy địa chỉ file upload ngoài thư mục code
        //            string file_path = f.CFILE;
        //            string dir_path_download = HashUtil.dir_path_download;
        //            if (dir_path_download != "") { file_path = dir_path_download + file_path; }
        //            str += " <a href='" + file_path + "' class=''><i class='icon-download-alt'></i> </a>";
        //        }
        //        str += "";
        //    }

        //    return str;
        //}
        public string EncodeOutput(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }
        public string OptionCoQuanXuLy(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
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
                int coquan_child = coquan.Where(x => x.IPARENT == (int)donvi.ICOQUAN && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).Count();
                if (coquan_child > 0 && donvi.IGROUP == 1)
                {
                    str += "<optgroup label='" + EncodeOutput(donvi.CTEN) + "'>";
                    str += OptionCoQuanXuLy(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + EncodeOutput(donvi.CTEN) + "</option>";
                    if (coquan_child > 0 && donvi.IGROUP == 0)
                    {
                        str += OptionCoQuanXuLy(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    }
                }

            }
            return str;
        }
        public string File_View(int id, string type)
        {
            string str = "";
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("ID", id);
            _dic.Add("CTYPE", type);
            string url_cookie = func.Get_Url_keycookie();
            var file = kntc.List_File(_dic).ToList();
            if (file.Count() > 0)
            {
                str += "";
                foreach (var f in file)
                {
                    string id_encr = HashUtil.Encode_ID(f.ID_FILE.ToString(), url_cookie);
                    str += " <a href='/Home/DownLoad/" + id_encr + "' class=''><i class='icon-download-alt'></i> </a>";
                }
                str += "";
            }

            return str;
        }

        public string TraCuuDon(List<DONTRACUU> don, UserInfor u, string url_cookie)
        {
            string str = "";
            var list = don;
            foreach (var d in list)
            {
                //int sodontrung = Convert.ToInt32(d.ISOLUONGTRUNG + d.SODONTRUNG);
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string trangthaixuly = GetTinhTrangDon(d.TINHTRANG);
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTINHTHANH) != null) diachi += HttpUtility.HtmlEncode(d.CTINHTHANH) + " .";
                string noidungdon = "";
                if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null)
                {
                    if (HttpUtility.HtmlEncode(d.CNOIDUNG).Trim() != "")
                    {
                        noidungdon = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                    }
                }
                string chitiet = "<a class='f-bule' href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
                string del = "<a href=\"javascript:void(0)\" data-original-title='Xóa' onclick=\"DeletePage_Confirm_TraCuu('" + id_encr + "','id=" + id_encr + "','/Kntc/Ajax_Kntc_back/','Bạn muốn khôi phục đơn khiếu nại này','khôi phục')\" rel='tooltip' title='Khôi phục' class='trans_func'><i class=' icon-signout'></i></a> ";
                string ghichu = "<p><span class='f-red'> Đơn đang tạm xóa </span></p>";
                if (d.IDELETE == 0) { del = " <a href=\"javascript:void(0)\" data-original-title='Xóa' onclick=\"DeletePage_Confirm_TraCuu('" + id_encr + "','id=" + id_encr + "','/Kntc/Ajax_Kntc_del/','Bạn muốn xóa đơn khiếu nại này','xóa')\" rel='tooltip' title='' class='trans_func'><i class='icon-trash'></i></a> "; ghichu = ""; }
                //string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                if ((int)d.IUSER != u.tk_action.iUser) { del = ""; }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p><strong>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) +
                   "</strong></p>" + diachi + "</td><td class='tcenter' nowrap>" +
             func.ConvertDateVN(d.DNGAYNHAN.ToString()) +
                   // bỏ khong hien thi so luong don trung trong màn hình tìm kiem
                   //"</td><td><p><a class='f-bule' href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'>" + noidungdon + "</a></p><p>Trạng thái: " + trangthaixuly + ghichu + "</p></td><td><p>Số lượng đơn trùng: " + sodontrung + "</p><p>Ghi chú: " + d.CGHICHU + "</p></td><td class='tcenter'>" + del + chitiet + "</td></tr>";
                   "</td><td><p><a class='f-bule' href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'>" + noidungdon + "</a></p><p>Trạng thái: " + trangthaixuly + ghichu + "</p></td><td><p>Ghi chú: " + d.CGHICHU + "</p></td><td class='tcenter'>" + del + chitiet + "</td></tr>";
            }

            return str;
        }

        public string GetTinhTrangDon(decimal tt)
        {
            string tinhtrang = "";
            List<TrangThaiDon> trangthai = Enum.GetValues(typeof(TrangThaiDon)).Cast<TrangThaiDon>().ToList();
            foreach (var a in trangthai)
            {
                if (tt == (decimal)a)
                {
                    tinhtrang = StringEnum.GetStringValue(a);
                    return tinhtrang;
                };
            }
            return tinhtrang;
        }

        public string GetTinhTrangDonDaXuLy(decimal tt)
        {
            string tinhtrang = "";
            List<TrangThaiDonDaXuLy> trangthai = Enum.GetValues(typeof(TrangThaiDonDaXuLy)).Cast<TrangThaiDonDaXuLy>().ToList();
            foreach (var a in trangthai)
            {
                if (tt == (decimal)a)
                {
                    tinhtrang = StringEnum.GetStringValue(a);
                    return tinhtrang;
                };
            }
            return tinhtrang;
        }

        public string Option_Khoa_KyHop(List<QUOCHOI_KHOA> khoa, int ikyhop )
        {
            string str = "";
            khoa = khoa.Where(x => x.IHIENTHI == 1).OrderBy(x => x.DBATDAU).ToList();

            foreach (var k in khoa )
            {
                string select = "";
                // if (k.IKHOA == ikyhop || k.IMACDINH == 1)
                // {
                //     select = "selected";
                // }
                str += "<option " + select + " value='" + k.IKHOA + "'>" + EncodeOutput(k.CTEN) + "</option>";
            }
            return str;
        }
        public string KNTC_TraCuuDonThu(List<KNTC_DON> don)
        {
            string str = "";
            don = don.OrderByDescending(x => x.DNGAYNHAN).ToList();
            int count = 1;
            string url_cookie = func.Get_Url_keycookie();
            if (don.Count() > 0)
            {
                str += "<tr><td colspan='5' class='alert tcenter alert-info'>Có " + don.Count() + " kết quả tìm kiếm</td></tr>";
                foreach (var d in don)
                {
                    string diachi = DiaChi_Don((int)d.IDIAPHUONG_0, (int)d.IDIAPHUONG_1, HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI));
                    // new id
                    string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                    KNTC kn = KNTC_Detail((int)d.IDON, id_encr);
                    // end
                    string donvi = "Chưa xác định";
                    if (d.IDONVITHULY != 0)
                    {
                        donvi = kntc.GetByID_CoQuan((int)d.IDONVITHULY).CTEN;
                    }
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'>" + donvi
                           + "</td><td><p>" + HttpUtility.HtmlEncode(d.CNOIDUNG) + "</p>" +
                            "</td><td>" + kn.tinhtrang + "</td><td class='tcenter'>" + kn.bt_info + kn.bt_lichsu + "</td></tr>";

                    count++;
                }
            }
            else
            {
                str += "<tr><td colspan='5' class='tcenter alert alert-danger'>Không tìm thấy kết quả nào!</td></tr";
            }
            return str;
        }

        public string KNTC_Don_Giamsat(List<KNTC_GIAMSAT> giamsat, UserInfor u, string url_cookie)
        {
            string str = "";
            var list = giamsat.ToList();
            int count = 1;
            //string url_cookie = func.Get_Url_keycookie();
            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    // new id
                    string id_encr = HashUtil.Encode_ID(d.IGIAMSAT.ToString(), url_cookie);
                    // end
                    string edit = " <a href=\"javascript:void(0)\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Giamsat_edit/')\" data-original-title='Sửa' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void(0)\" data-original-title='Xóa' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kntc/Ajax_Giamsat_del/','Bạn muốn xóa kế hoạch giám sát này')\" rel='tooltip' title='' class='trans_func'><i class='icon-trash'></i></a> ";
                    string chon = "<input type='checkbox' name='don' value='" + d.IDON + "' />";
                    string file = File_View((int)d.IGIAMSAT, "kntc_giamsat");

                    if (d.IUSER != u.user_login.IUSER)
                    {
                        edit = ""; del = "";
                    }
                    if (!u.tk_action.is_lanhdao)
                    {
                        edit = ""; del = "";
                    }
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'>Ban Dân Nguyện</td><td class='tcenter'>" + d.CKEHOACH +
                         "</td><td>" + d.CCHUYENDE + "</td><td>" +
                        HttpUtility.HtmlEncode(d.CNOIDUNG) + ". " + file + NguoiCapNhat((int)d.IUSER, (DateTime)d.DDATE) +
                        "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";

                    count++;
                }
            }
            return str;
        }

        public string NguoiCapNhat(int iUser, DateTime time)
        {
            string str = "";
            string tennguoinhap = "";
            USERS u = kntc.GetUser(iUser);
            if (u != null)
            {
                tennguoinhap = u.CTEN;
            }
            str += "<p class='tright f-grey'><em>Cập nhật bởi " + tennguoinhap + " ngày " + func.ConvertDateVN(time.ToString()) + "</em></p>";
            return str;
        }

        public string Don_Moicapnhat(List<KNTCDON_MOICAPNHAT> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            //var don = db.kntc_don.Where(x => x.iTinhTrangXuLy == 0).OrderByDescending(x => x.dNgayNhan).ToList();
            //var don = _iKNTC_Don.GetList("select * from KNTC_DON").Where(x=>x.ITINHTRANGXULY==0).OrderByDescending(x=>x.IDON).ToList();
            int count = 1;
            KNTCDON_MOICAPNHAT kd = don.FirstOrDefault();
            int total = 0;
            if (kd != null) total = (int)kd.TOTAL;
            if (don.Count() > 0)
            {
                foreach (var d in don)
                {
                    string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                    string edit = " <a href=\"/Kntc/Sua/?id=" + id_encr + "\"  onclick=\"ShowPageLoading()\" data-original-title='Sửa đơn' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kntc/Ajax_Don_del','Bạn muốn xóa đơn này')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string chon = "<input type='checkbox' name='don' value='" + id_encr + "' />";
                    string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuyen_Xuly')\" href='javascript:void()' data-original-title='Chuyển xử lý đơn' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
                    string kiemtrung = " <a  onclick=\"ShowPageLoading()\" href='/Kntc/Kiemtrung/?id=" + id_encr + "' data-original-title='Kiểm trùng đơn' rel='tooltip' title='' class='trans_func'><i class='icon-search'></i></a>";
                    if (!_base_bussiness.ActionMulty_("10,11,12,44", act)) { kiemtrung = ""; }
                    //string diachi = DiaChi_Don((int)d.IDIAPHUONG_0, (int)d.IDIAPHUONG_1, HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI));
                    if (!_base_bussiness.ActionMulty_("11,44", act)) { chuyen_xuly = ""; }
                    if (!_base_bussiness.ActionMulty_("45,44", act)) { edit = ""; del = ""; }
                    string dontrung = "";

                    string noidung = Capnhatnoidung(id_encr);

                    string noidungdon = "";
                    string type = "";
                    if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null)
                    {
                        if (HttpUtility.HtmlEncode(d.CNOIDUNG).Trim() != "")
                        {
                            noidungdon = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                            noidung = "";
                        }
                    }
                    TD_VUVIEC vuviec = kntc.Get_DonKNTCByIDTCD((int)d.IDON);
                    if (vuviec != null)
                    {
                        type = "<span class='f-red'>[ Từ tiếp công dân ]</span>";
                    }
                    string diachi = "";
                    if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                    if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                    if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                    string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                    string giamsat = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Giamsat/?id=" + id_encr + "' data-original-title='Kế hoạch giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-zoom-in'></i></a> ";
                    string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                    string ngaynhan = ""; if (d.DNGAYNHAN != null) ngaynhan = func.ConvertDateVN(d.DNGAYNHAN.ToString());
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p><strong>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) +
                        "</strong></p>" + diachi + "</td><td class='tcenter' nowrap>" +
                    ngaynhan +
                        "</td><td><p>" + noidungdon + "</p>" + noidung + " " + dontrung + type + "</td><td class='tcenter' nowrap>" + info + lichsu + kiemtrung + chuyen_xuly + edit + del + "</td></tr>";

                    count++;
                }
            }
            return str;
        }

        private string Xemdontrung(string id_encr)
        {
            string str = "<a href='/Kntc/Don_info/?id=" + id_encr + "' class='f-bule'>[ Xem đơn trùng ]</a>";
            return str;
        }

        private string Capnhatnoidung(string id_encr)
        {
            string str = "<a href='/Kntc/Sua/?id=" + id_encr + "' class='f-blue'>[ Cập nhật nội dung ]</a>";
            return str;
        }

        public KNTC KNTC_Detail(int id, string id_encr)
        {
            KNTC kn = null;
            UserInfor u = GetUserInfor();

            KNTC_DON don = null;
            if (id == 0)
            {
                don = (KNTC_DON)HttpContext.Current.Session["KNTCDonSession"];
            }
            else
            {
                don = kntc.GetDON(id);
            }
            if (don != null)
            {
                kn = new KNTC();
                kn.nguoinop = don.CNGUOIGUI_TEN;
                if (Convert.ToInt32(don.INGUOIGUI_QUOCTICH) != 0)
                {
                    var k = kntc.Get_QuocTich((int)don.INGUOIGUI_QUOCTICH);
                    if (k != null)
                    {
                        kn.quoctich = HttpUtility.HtmlEncode(k.CTEN);
                    }
                }
                else
                {
                    kn.quoctich = "Chưa xác định";
                }
                if (Convert.ToInt32(don.INGUOIGUI_DANTOC) != 0)
                {
                    DANTOC dantoc = kntc.Get_DanToc(Convert.ToInt32(don.INGUOIGUI_DANTOC));
                    if (dantoc != null)
                    {
                        kn.dantoc = HttpUtility.HtmlEncode(dantoc.CTEN);
                    }
                }
                else
                {
                    kn.dantoc = "Chưa xác định";
                }
                if (Convert.ToInt32(don.ILINHVUC) != 0)
                {
                    var k = kntc.Get_LinhVuc((int)don.ILINHVUC);
                    if (k != null)
                    {
                        kn.linhvuc = HttpUtility.HtmlEncode(k.CTEN);
                    }
                }
                else
                {
                    kn.linhvuc = "Chưa xác định";
                }
                if (Convert.ToInt32(don.ITINHCHAT) != 0)
                {
                    var k = kntc.Get_TinhChat((int)don.ITINHCHAT);
                    if (k != null)
                    {
                        kn.tinhchat = HttpUtility.HtmlEncode(k.CTEN);
                    }
                }
                else
                {
                    kn.tinhchat = "Chưa xác định";
                }
                if (Convert.ToInt32(don.ILOAIDON) != 0)
                {
                    var k = kntc.Get_LoaiDon((int)don.ILOAIDON);
                    if (k != null)
                    {
                        kn.loaidon = HttpUtility.HtmlEncode(k.CTEN);
                    }
                }
                else
                {
                    kn.loaidon = "Chưa xác định";
                }
                if (Convert.ToInt32(don.INOIDUNG) != 0)
                {
                    var k = kntc.Get_NoiDungDon((int)don.INOIDUNG);
                    if (k != null)
                    {
                        kn.loai_noidung = HttpUtility.HtmlEncode(k.CTEN);
                    }
                }
                else
                {
                    kn.loai_noidung = "Chưa xác định";
                }
                if (Convert.ToInt32(don.INGUONDON) != 0)
                {
                    if (don.INGUONDON != 0)
                    {
                        var k = kntc.Get_NguonDon((int)don.INGUONDON);
                        kn.nguondon = k != null ? HttpUtility.HtmlEncode(k.CTEN) : string.Empty;
                    }
                }
                else
                {
                    kn.nguondon = "Chưa xác định";
                }
                string tu = "";
                TD_VUVIEC vuviec = kntc.Get_DonKNTCByIDTCD(id);
                if (vuviec != null)
                {
                    kn.nguon = "Đơn có vụ việc trùng";
                }
                string diachi = "";
                if (HttpUtility.HtmlEncode(don.CNGUOIGUI_DIACHI) != null)
                {
                    diachi = HttpUtility.HtmlEncode(don.CNGUOIGUI_DIACHI) + ", ";
                }
                if (Convert.ToInt32(don.IDIAPHUONG_2) != 0)
                {
                    var k = kntc.Get_DiaPhuong((int)don.IDIAPHUONG_2);
                    var xa = "";
                    if (k != null)
                    {
                        xa = HttpUtility.HtmlEncode(k.CTEN);
                    }
                    diachi += xa + ", ";
                }
                if (Convert.ToInt32(don.IDIAPHUONG_1) != 0)
                {
                    var k = kntc.Get_DiaPhuong((int)don.IDIAPHUONG_1);
                    if (k != null)
                    {
                        kn.huyen = HttpUtility.HtmlEncode(k.CTEN);
                    }
                    diachi += kn.huyen + ", ";
                }
                if (Convert.ToInt32(don.IDIAPHUONG_0) != 0)
                {
                    if ((int)don.IDIAPHUONG_0 != -1)
                    {
                        var k = kntc.Get_DiaPhuong((int)don.IDIAPHUONG_0);
                        if (k != null)
                        {
                            kn.tinh = HttpUtility.HtmlEncode(k.CTEN);
                        }

                        diachi += kn.tinh + ".";
                    }
                }
                else
                {
                    kn.tinh = "";
                }
                kn.diachi_nguoinop = diachi;
                kn.bt_info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                kn.bt_giamsat = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Giamsat/?id=" + id_encr + "' data-original-title='Kế hoạch giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-zoom-in'></i></a> ";
                kn.bt_lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                if (don.CLUUTHEODOI_LYDO != null)
                    kn.lydoluudon = don.CLUUTHEODOI_LYDO;
                if (don.CHITIETLYDO_LUUTHEODOI != null)
                    kn.lydochitiet = don.CHITIETLYDO_LUUTHEODOI;

                if (Convert.ToInt32(don.IDONVITHULY) != 0)
                {
                    QUOCHOI_COQUAN cq = kntc.GetDonVi((int)don.IDONVITHULY);
                    if (cq != null)
                    {
                        kn.donvi_thuly = cq.CTEN;
                    }
                }
                if (Convert.ToInt32(don.IDONVITIEPNHAN) != 0)
                {
                    QUOCHOI_COQUAN cq = kntc.GetDonVi((int)don.IDONVITIEPNHAN);
                    if (cq != null)
                    {
                        kn.donvi_tiepnhan = cq.CTEN;
                    }
                }

                if (don.ITINHTRANGXULY.HasValue)
                {
                    if (don.ITINHTRANGXULY == (decimal)TrangThaiDon.MoiCapNhat)
                    {
                        kn.tinhtrang = StringEnum.GetStringValue(TrangThaiDon.MoiCapNhat);
                    }
                    if (don.ITINHTRANGXULY == (decimal)TrangThaiDon.DaChuyenXuLy)
                    {
                        kn.tinhtrang = StringEnum.GetStringValue(TrangThaiDon.DaChuyenXuLy);
                    }
                    if (don.ITINHTRANGXULY == (decimal)TrangThaiDon.DaPhanLoai)
                    {
                        if (don.ITHAMQUYEN == (decimal)ThamQuyenXuLy.ThuocThamQuyen)
                        {
                            if (don.IDUDIEUKIEN == (decimal)DieuKienXuLy.DuDieuKien)
                            {
                                kn.tinhtrang = StringEnum.GetStringValue(ThamQuyenXuLy.ThuocThamQuyen) + "," + StringEnum.GetStringValue(DieuKienXuLy.DuDieuKien);
                            }
                            if (don.IDUDIEUKIEN == (decimal)DieuKienXuLy.ChuaXacDinh)
                            {
                                kn.tinhtrang = StringEnum.GetStringValue(ThamQuyenXuLy.ThuocThamQuyen);
                            }
                            if (don.IDUDIEUKIEN == (decimal)DieuKienXuLy.KhongDuDieuKien)
                            {
                                kn.tinhtrang = StringEnum.GetStringValue(ThamQuyenXuLy.ThuocThamQuyen) + "," + StringEnum.GetStringValue(DieuKienXuLy.KhongDuDieuKien);
                            }
                        }
                        else
                        {
                            kn.tinhtrang = StringEnum.GetStringValue(ThamQuyenXuLy.KhongThuocThamQuyen);
                        }
                    }
                    if (don.ITINHTRANGXULY == (decimal)TrangThaiDon.ChoXuLy)
                    {
                        kn.tinhtrang = StringEnum.GetStringValue(TrangThaiDon.ChoXuLy);
                    }
                    if (don.ITINHTRANGXULY == (decimal)TrangThaiDon.DangXuLy)
                    {
                        kn.tinhtrang = StringEnum.GetStringValue(TrangThaiDon.DangXuLy);
                    }
                    if (don.ITINHTRANGXULY == (decimal)TrangThaiDon.KhongXuLy)
                    {
                        kn.tinhtrang = StringEnum.GetStringValue(TrangThaiDon.KhongXuLy);
                    }
                    if (don.ITINHTRANGXULY == (decimal)TrangThaiDon.HoanThanh)
                    {
                        kn.tinhtrang = StringEnum.GetStringValue(TrangThaiDon.HoanThanh);
                    }
                }


                if (don.IDOKHAN == (decimal)DoKhan.Thuong)
                {
                    kn.dokhan = StringEnum.GetStringValue(DoKhan.Thuong);
                }
                if (don.IDOKHAN == (decimal)DoKhan.Khan)
                {
                    kn.dokhan = StringEnum.GetStringValue(DoKhan.Khan);
                }
                if (don.IDOKHAN == (decimal)DoKhan.ThuongKhan)
                {
                    kn.dokhan = StringEnum.GetStringValue(DoKhan.ThuongKhan);
                }
                if (don.IDOKHAN == (decimal)DoKhan.HoaToc)
                {
                    kn.dokhan = StringEnum.GetStringValue(DoKhan.HoaToc);
                }
                if (don.IDOMAT == (decimal)DoMat.Thuong)
                {
                    kn.domat = StringEnum.GetStringValue(DoMat.Thuong);
                }
                if (don.IDOMAT == (decimal)DoMat.Mat)
                {
                    kn.domat = StringEnum.GetStringValue(DoMat.Mat);
                }
                if (don.IDOMAT == (decimal)DoMat.ToiMat)
                {
                    kn.domat = StringEnum.GetStringValue(DoMat.ToiMat);
                }
                if (don.IDOMAT == (decimal)DoMat.TuyetMat)
                {
                    kn.domat = StringEnum.GetStringValue(DoMat.TuyetMat);
                }

                kn.ketquadanhgia = "Chưa xác định";
                if (don.IDANHGIA.HasValue)
                {
                    if (don.IDANHGIA != 0)
                    {
                        kn.ketquadanhgia = GetNameDanhgia((decimal)don.IDANHGIA);
                    }
                }
            }
            return kn;
        }

        public string FileHoSoDon(int id)
        {
            string str = "";
            str += " <a href='/kntc/HoSoDon/?id=" + id + "' class='f-green'><i class='icon-download-alt'></i></a>";
            return str;
        }

        public string DiaChi_Don(int diaphuong0, int diaphuong1, string diachi)
        {
            string str = diachi + ", ";
            if (diachi == "")
            {
                str = "";
            }
            if (diaphuong1 != 0)
            {
                DIAPHUONG d1 = kntc.Get_DiaPhuong(diaphuong1);
                DIAPHUONG d0 = kntc.Get_DiaPhuong(diaphuong0);
                if (d1 != null || d0 != null)
                {
                    str += d1.CTYPE + " " + HttpUtility.HtmlEncode(d1.CTEN) + ", " + HttpUtility.HtmlEncode(d0.CTYPE) + " " + HttpUtility.HtmlEncode(d0.CTEN);
                }
            }
            else
            {
                DIAPHUONG d0 = kntc.Get_DiaPhuong(diaphuong0);
                if (d0 != null)
                {
                    str += HttpUtility.HtmlEncode(d0.CTYPE) + " " + HttpUtility.HtmlEncode(d0.CTEN);
                }
            }
            return str;
        }

        public string Option_DanToc(decimal id_chucvu = 1)
        {
            string str = "";
            var chucvu = kntc.List_DanToc().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.CTEN).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.IDANTOC == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.IDANTOC + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_QuocTich(decimal id_chucvu = 233)
        {
            string str = "";
            var chucvu = kntc.List_QuocTich().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(t => t.CTEN).ToList();
            //var chucvu = _quoctich.GetAll().OrderByDescending(t => t.CTEN).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.IQUOCTICH == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.IQUOCTICH + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_NguonDon(int id_chucvu = 0)
        {
            string str = "";
            var chucvu = kntc.List_NguonDon().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.INGUONDON == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.INGUONDON + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_NguoiNhap(int iDonvi = 0, int id_user = 0)
        {
            string str = "";
            var chucvu = kntc.List_NguoiNhap(iDonvi).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.IUSER == id_user) { select = " selected "; }
                str += "<option " + select + " value='" + p.IUSER + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_LinhVuc(int id_chucvu = 0)
        {
            string str = "";
            var chucvu0 = kntc.List_LinhVuc().Where(x => x.IPARENT == 0 && x.IHIENTHI == 1 && x.IDELETE == 0).OrderByDescending(t => t.CTEN).ToList();
            foreach (var p in chucvu0)
            {
                string select0 = ""; if (p.ILINHVUC == id_chucvu) { select0 = " selected "; }
                var chucvu1 = kntc.List_LinhVuc().Where(x => x.IPARENT == p.ILINHVUC && x.IHIENTHI == 1).OrderByDescending(t => t.CTEN).ToList();
                if (chucvu1.Count() == 0)
                {
                    str += "<option " + select0 + " value='" + p.ILINHVUC + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
                }
                else
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(p.CTEN) + "'>";
                    foreach (var p1 in chucvu1)
                    {
                        string select1 = ""; if (p1.ILINHVUC == id_chucvu) { select1 = " selected "; }
                        str += "<option " + select1 + " value='" + p1.ILINHVUC + "'>" + HttpUtility.HtmlEncode(p1.CTEN) + "</option>";
                    }
                    str += "</optgroup>";
                }
            }
            return str;
        }

        public string Option_DieuKienXuLy(decimal id = -1)
        {
            string str = "<option value=\"-1\""+ (id == -1 ? " selected " : " ") +">Chưa xác định</option>" +
                         "<option value=\"1\"" + (id == 1 ? " selected " : " ") + ">Đủ điều kiện xử lý</option>" +
                         "<option value=\"0\"" + (id == 0 ? " selected " : " ") + ">Không đủ điều kiện xử lý</option>";
            return str;
        }

        public string Option_HinhThucXuLy(decimal id = -1)
        {
            string str = "<option value=\"-1\"" + (id == -1 ? " selected " : " ") + ">Đang nghiên cứu</option>" +
                         "<option value=\"1\"" + (id == 1 ? " selected " : " ") + ">Chuyển đơn</option>" +
                         "<option value=\"2\"" + (id == 2 ? " selected " : " ") + ">Hướng dẫn giải thích, trả lời</option>" +
                         "<option value=\"0\"" + (id == 0 ? " selected " : " ") + ">Không chuyển</option>";
            return str;
        }

        public string Option_LoaiBaoCao(decimal id = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<option value=\"0\"" + (id == 0 ? " selected " : "") + ">Chọn tất cả</option>");
            sb.AppendLine("<option value=\"1\"" + (id == 1 ? " selected " : "") + ">HĐND Tỉnh</option>");
            sb.AppendLine("<option value=\"2\"" + (id == 2 ? " selected " : "") + ">Đoàn đại biểu QH Tỉnh</option>");
            return sb.ToString();
        }

        public string Option_TenBaoCao(decimal id = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<option value=\"1\"" + (id == 1 ? " selected " : "") + ">Tổng hợp kết quả tiếp nhận đơn</option>");
            return sb.ToString();
        }

        public string Option_Ky(int kySelected = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<option value=\"0\"" + (kySelected == 0 ? " selected " : "") + ">Năm</option>");
            sb.AppendLine("<option value=\"1\"" + (kySelected == 1 ? " selected " : "") + ">Quý</option>");
            sb.AppendLine("<option value=\"2\"" + (kySelected == 2 ? " selected " : "") + ">Tháng</option>");
            sb.AppendLine("<option value=\"3\"" + (kySelected == 3 ? " selected " : "") + ">Khác</option>");
            return sb.ToString();
        }

        public string Option_Year_List(int countPrev = 50, int countNext = 1, int selectedValue = 0, bool hasDefault = true, int defaultValue = 0, string defaultText = "--Tất cả--")
        {
            string str = "";
            var currentYear = DateTime.Now.Year;
            var startYear = currentYear - countPrev;
            var endYear = currentYear + countNext;
            if (hasDefault) str += "<option value='" + defaultValue + "' " + (defaultValue == selectedValue ? "selected" : "") + ">" + defaultText + "</option>";

            for (var year = endYear; year >= startYear; year--)
            {
                str = str + "<option value ='" + year + "' " + (year == selectedValue ? "selected" : "") + ">" + year + "</option>";
            }

            return str;
        }

        public string Option_Month_List(bool hasDefault = true, int defaultValue = 0, string defaultText = "--Tất cả--")
        {
            string str = "";
            const int numberOfMonth = 12;
            if (hasDefault) str += "<option value='" + defaultValue + "' selected>" + defaultText + "</option>";
            for (var month = 1; month <= numberOfMonth; month++)
            {
                str = str + "<option value ='" + month + "'>" + month + "</option>";
            }

            return str;
        }

        public string Option_Quy_List(bool hasDefault = true, int defaultValue = 0, string defaultText = "--Tất cả--")
        {
            string str = "";
            if (hasDefault) str += "<option value='" + defaultValue + "' selected>" + defaultText + "</option>";
            for (int i = 1; i <= 4; i++)
            {
                str += "<option value='" + i + "'>Quý " + NumberExtension.ToRoman(i) + "</option>";
            }
            return str;
        }

        public string Option_TinhThanh_ByID_Parent(List<DIAPHUONG> lstDiaphuong, decimal parent = 0, decimal id = 0)
        {
            string str = "";
            var chucvu = lstDiaphuong.Where(x => x.IPARENT == parent).OrderBy(t => t.CTEN).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.IDIAPHUONG == id) { select = " selected "; }
                str += "<option " + select + " value='" + p.IDIAPHUONG + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_ChuyenVien_ChuyenXuLy_KNTC(List<USERS> lstUser, List<USER_ACTION> lstUserAction, decimal id)
        {
            string str = "";
            var chucvu = lstUser.Where(x => x.ITYPE != -1).OrderBy(t => t.ICHUCVU).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.IUSER == id) { select = " selected "; }
                string tenchucvu = "";
                if (p.ICHUCVU != 0)
                {
                    var user_cv = kntc.Get_ChucVu((int)p.ICHUCVU);
                    tenchucvu = user_cv == null ? "" : HttpUtility.HtmlEncode(user_cv.CTEN) + ". ";
                }

                if (lstUserAction.Where(x => x.IUSER == (int)p.IUSER).Count() > 0)
                {
                    str += "<option " + select + " value='" + p.IUSER + "'>" + tenchucvu + HttpUtility.HtmlEncode(p.CTEN) + " </option>";
                }
            }
            return str;
        }

        public string Option_ChuyenVien_ChuyenXuLy(int iDonVi = 4, int id_user_choice = 0, string type = "")
        {
            string str = "";
            var chucvu = kntc.List_User().Where(x => x.IDONVI == iDonVi && x.ISTATUS == 1).OrderByDescending(t => t.ICHUCVU).ToList();
            List<KNTC_DON> don = kntc.List_Don().ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.IUSER == id_user_choice) { select = " selected "; }
                string tenchucvu = "";
                if (p.ICHUCVU != 0)
                {
                    tenchucvu = HttpUtility.HtmlEncode(kntc.Get_ChucVu((int)p.ICHUCVU).CTEN);
                }
                int count_don_KNTC = 0;
                var don_count = don.Where(x => x.IUSER_DUOCGIAOXULY == (int)p.IUSER).ToList();
                if (type == "choxuly") { count_don_KNTC = don_count.Where(x => x.ITINHTRANGXULY == 1).Count(); }
                if (type == "thuocthamquyen") { count_don_KNTC = don_count.Where(x => x.ITINHTRANGXULY == 2 && x.ITHAMQUYEN == 1 && x.IDUDIEUKIEN == -1).Count(); }
                if (type == "khongthuocthamquyen") { count_don_KNTC = don_count.Where(x => x.ITINHTRANGXULY == 2 && x.ITHAMQUYEN == 0).Count(); }
                if (type == "dudieukien") { count_don_KNTC = don_count.Where(x => x.ITINHTRANGXULY == 2 && x.ITHAMQUYEN == 1 && x.IDUDIEUKIEN == 1).Count(); }
                if (type == "khongdudieukien") { count_don_KNTC = don_count.Where(x => x.ITINHTRANGXULY == 2 && x.ITHAMQUYEN == 1 && x.IDUDIEUKIEN == 0).Count(); }
                if (count_don_KNTC > 0)
                {
                    str += "<option " + select + " value='" + p.IUSER + "'>" + tenchucvu + ". " + HttpUtility.HtmlEncode(p.CTEN) + " (" + count_don_KNTC + ")</option>";
                }
            }
            return str;
        }

        public string KNTC_Don_Choxuly(List<KNTCDON_MOICAPNHAT> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            var list = don.ToList();
            int count = 1;
            foreach (var d in list)
            {
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string edit = " <a href=\"/Kntc/Sua/?id=" + id_encr + "\"  onclick=\"ShowPageLoading()\" data-original-title='Sửa đơn' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string chon = "<input type='checkbox' name='don' value='" + id_encr + "' />";
                string phanloai = " <a  onclick=\"ShowPageLoading()\" href=\"/Kntc/Phanloai?id=" + id_encr + "&type=1 "+ "\" href='javascript:void()' data-original-title='Phân loại đơn' rel='tooltip' title='' class='trans_func'><i class='icon-list-ol'></i></a>";
                //string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuyen_Xuly')\" href='javascript:void()' data-original-title='Chuyển xử lý đơn' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
                string del = " <a href=\"javascript:void()\" title='Xóa' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kntc/Ajax_Kntc_del','Bạn có muốn xóa khiếu nại này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string chuyen_xuly = "";
                string kiemtrung = " <a  onclick=\"ShowPageLoading()\" href='/Kntc/Kiemtrung/?id=" + id_encr + "' data-original-title='Kiểm trùng đơn' rel='tooltip' title='' class='trans_func'><i class='icon-search'></i></a>";
                if (!_base_bussiness.ActionMulty_("11,44", act)) { chuyen_xuly = ""; }
                if (!_base_bussiness.ActionMulty_("12,44", act)) { phanloai = ""; }
                if (!_base_bussiness.ActionMulty_("45,44", act)) { edit = ""; }
                // if (!_base_bussiness.IsAdmin_(act)) { phanloai = ""; }
                string dontrung = "";

                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string giamsat = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Giamsat/?id=" + id_encr + "' data-original-title='Kế hoạch giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-zoom-in'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";

                string noidung = "<span class='f-red'>[ Nội dung chưa được cập nhật ]</span>";
                if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null)
                {
                    if (HttpUtility.HtmlEncode(d.CNOIDUNG).Trim() != "")
                    {
                        noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                    }
                }
                string ngaynhan = ""; if (d.DNGAYNHAN != null) ngaynhan = func.ConvertDateVN(d.DNGAYNHAN.ToString());
                string type = "";
                TD_VUVIEC vuviec = kntc.Get_DonKNTCByIDTCD((int)d.IDON);
                if (vuviec != null) { type = "<span class='f-red'>[ Từ tiếp công dân ]</span>"; }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p><strong>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) +
                    "</strong></p>" + diachi + "</td><td class='tcenter' nowrap>" + ngaynhan + "</td><td><p>" +
                   noidung + "</p>" + dontrung + type + "</td><td class='tcenter' nowrap>" + edit + info + lichsu + kiemtrung + chuyen_xuly + phanloai + del + "</td></tr>";

                count++;
            }

            return str;
        }

        public string List_DonTrung(List<KNTC_DON> list, KNTC_DON don_check, string url_cookie)
        {
            string str = "";

            if (list.Count() == 0)
            {
                return "<div class='alert alert-success tcenter b nomargin'><i class='icon-ok-sign'></i> Không tìm thấy đơn trùng</div>";
            }
            str += "<table class='table table-bordered table-condensed nomargin'><tr><th width='3%' class='tcenter'>STT</th><th width='10%' nowrap class='tcenter'>Chọn</th><th nowrap>Nội dung đơn</th><th nowrap>Ngày nhập / Đơn vị nhập</th><th nowrap>Người gửi / Địa chỉ người gửi</th><th nowrap>Tình trạng đơn</th><th class='tcenter' nowrap>Xem đơn</th></tr>";
            int count = 1;
            string id_doncheck_encr = HashUtil.Encode_ID(don_check.IDON.ToString(), url_cookie);
            foreach (var d in list)
            {
                //string check = "";
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string chon = "<a type id='btn_" + id_encr + "' onclick=\"ChonDonTrung('" + id_encr + "')\" data-original-title='Chọn đơn trùng' rel='tooltip' title='' href='javascript:void(0)'   class='chontrung f-grey'><i class='icon-ok-sign'></i></a>";

                //string chon_trung = "<input type='radio' " + check + " name='dontrung' id='dontrung' value='" + d.IDON + "'/>";
                string tinhtrangdon = GetTinhTrangDon((decimal)d.ITINHTRANGXULY);
                KNTC k = KNTC_Detail((int)d.IDON, id_encr);
                if (k != null)
                {
                    str += "<tr><td class='tcenter b'>" + count + "</td><td class='tcenter'>" + chon + "</td><td>" + HttpUtility.HtmlEncode(d.CNOIDUNG) +
                   "</td><td><p><strong>" + func.ConvertDateVN(d.DNGAYNHAN.ToString()) + "</strong></p>" + k.donvi_tiepnhan + "</td><td><p><strong>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</strong></p>" + DiaChi_Don((int)d.IDIAPHUONG_0, (int)d.IDIAPHUONG_1, HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI)) +
                   "</td><td>" + tinhtrangdon + "</td><td class='tcenter'>" + k.bt_info + "</td></tr>";
                    count++;
                }
            }
            str += "</table>";
            return str;
        }

        public string List_LichSuXuLy(int idon)
        {
            string str = "";
            str += "<table class='table table-bordered table-condensed nomargin'><tr><th width='3%' class='tcenter'>STT</th><th nowrap class='tcenter'>Ngày chuyển</th><th class='tcenter' nowrap>Đơn vị gửi</th><th nowrap class='tcenter'>Đơn vị nhận</th><th nowrap class='tcenter'>Ghi chú</th></tr>";
            int stt = 0;
            var list = kntc.List_LichSuDon().Where(v => v.IDON == idon).OrderByDescending(v => v.DNGAYCHUYEN).ToList();
            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    stt++;
                    string ngaychuyen = "";
                    string ngaybanhanh = "";
                    if (d.DNGAYCHUYEN != null) { ngaychuyen = func.ConvertDateVN(d.DNGAYCHUYEN.ToString()); }
                    string donviNhan = ""; string donviGui = "";
                    QUOCHOI_COQUAN cqnhan = kntc.GetDonVi((int)d.IDONVITIEPNHAN); if (cqnhan != null) donviNhan = cqnhan.CTEN;
                    QUOCHOI_COQUAN cqgui = kntc.GetDonVi((int)d.IDONVIGUI); if (cqgui != null) donviGui = cqgui.CTEN;
                    KNTC_VANBAN vb = kntc.GetVB_ByID((int)d.IVANBAN);
                    string vanban = "";
                    if (vb != null)
                    {
                        if (vb.DNGAYBANHANH != null) { ngaybanhanh = func.ConvertDateVN(vb.DNGAYBANHANH.ToString()); }
                    }
                    if (d.ICHUYENXULY == (int)LoaiLichSu.Luanchuyen) vanban = "Luân chuyển xử lý"; else vanban = "Chuyển đến đơn vị thẩm quyền xử lý";
                    str += "<tr><td class='tcenter'>" + stt + "</td><td class='tcenter'>" + ngaychuyen + "</td><td class=''>" + donviGui + "</td><td class=''>" + donviNhan + "</td><td class=''>" + vanban + "</td></tr>";
                }
            }
            else
            {
                str += "<tr><td colspan=5 class='tcenter f-red'>Không có lịch sử luân chuyển</td></tr>";
            }
            str += "</table>";
            return str;
        }

        public string List_DonTrungDetail(List<KNTC_DON> list, string url_cookie)
        {
            string str = "";
            str += "<table class='table table-bordered table-condensed nomargin'><tr><th width='3%' class='tcenter'>STT</th><th nowrap  class='tcenter'>Ngày nhập</th><th nowrap>Nội dung đơn</th><th nowrap>Người gửi / Địa chỉ người gửi</th><th nowrap>Tình trạng đơn</th><th class='tcenter' nowrap>Xem đơn</th></tr>";
            int count = 1;
            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                    string tinhtrangdon = GetTinhTrangDon((decimal)d.ITINHTRANGXULY);
                    KNTC k = KNTC_Detail((int)d.IDON, id_encr);
                    if (k != null)
                    {
                        str += "<tr><td class='tcenter b'>" + count + "</td><td class='tcenter'>" + func.ConvertDateVN(d.DNGAYNHAN.ToString()) + "</td><td>" + HttpUtility.HtmlEncode(d.CNOIDUNG) +
                       "</td><td><p><strong>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</strong></p>" + DiaChi_Don((int)d.IDIAPHUONG_0, (int)d.IDIAPHUONG_1, HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI)) +
                       "</td><td>" + tinhtrangdon + "</td><td class='tcenter'>" + k.bt_info + "</td></tr>";
                        count++;
                    }
                }
            }
            else
            {
                str += "<tr><td colspan=5 class='tcenter f-red'>Không có đơn trùng</td></tr>";
            }
            str += "</table>";
            return str;
        }

        public string KNTC_Don_Khongdudieukien(List<KNTCDON_MOICAPNHAT> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";

            var list = don.ToList();
            foreach (var d in list)
            {
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string edit = " <a href=\"/Kntc/Sua/?id=" + id_encr + "\"  onclick=\"ShowPageLoading()\" data-original-title='Sửa đơn' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string phanloai = " <a  onclick=\"ShowPageLoading()\" href=\"/Kntc/Phanloai/?id=" + id_encr + "\" href='javascript:void()' data-original-title='Phân loại đơn' rel='tooltip' title='' class='trans_func'><i class='icon-list-ol'></i></a>";
                string dontrung = "";
                string del = " <a href=\"javascript:void()\" title='Xóa' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kntc/Ajax_Kntc_del','Bạn có muốn xóa khiếu nại này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string kiemtrung = " <a href='/Kntc/Kiemtrung/?id=" + id_encr + "' onclick=\"ShowPageLoading()\" data-original-title='Kiểm trùng đơn' rel='tooltip' title='' class='trans_func'><i class='icon-search'></i></a>";
                if (_base_bussiness.ActionMulty_("10,11,12,44", act)) { kiemtrung = ""; }
                string luudon = "<a href='#' class='trans_func' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Luutheodoi')\" data-original-title='Lưu đơn, theo dõi' rel='tooltip' title=''><i class='icon-save'></i></a>";
                if (!_base_bussiness.ActionMulty_("44,12", act)) { phanloai = ""; }
                if (!_base_bussiness.ActionMulty_("44,13", act)) { luudon = ""; }
                if (!_base_bussiness.ActionMulty_("45,44", act)) { edit = ""; }
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string giamsat = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Giamsat/?id=" + id_encr + "' data-original-title='Kế hoạch giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-zoom-in'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                string ngaynhan = ""; if (d.DNGAYNHAN != null) ngaynhan = func.ConvertDateVN(d.DNGAYNHAN.ToString());
                str += "<tr><td class='tcenter b'>" + d.STT + "</td><td><p><strong>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) +
                    "</strong></p>" + diachi + "</td><td class='tcenter' nowrap>" +
                   ngaynhan + "</td><td>" +
                    "<p>" + noidung + "</p>" + dontrung +
                    "</td><td class='tcenter' nowrap>" + luudon +
                    "</td><td class='tcenter' nowrap>" + edit + info + lichsu + kiemtrung + phanloai + del + "</td></tr>";
            }
            return str;
        }

        public string KNTC_Don_Thuocthamquyen(List<KNTCDON_MOICAPNHAT> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";

            var list = don.ToList();
            foreach (var d in list)
            {
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string edit = " <a href=\"/Kntc/Sua/?id=" + id_encr + "\"  onclick=\"ShowPageLoading()\" data-original-title='Sửa đơn' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string phanloai = " <a onclick=\"ShowPageLoading()\" href=\"/Kntc/Phanloai?id=" + id_encr + "\" href='javascript:void()' data-original-title='Phân loại đơn' rel='tooltip' title='' class='trans_func'><i class='icon-list-ol'></i></a>";
                if (!_base_bussiness.ActionMulty_("12,44", act)) { phanloai = ""; }
                if (!_base_bussiness.ActionMulty_("45,44", act)) { edit = ""; }
                string dontrung = "";
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";

                string kiemtrung = " <a  onclick=\"ShowPageLoading()\" href='/Kntc/Kiemtrung/?id=" + id_encr + "' data-original-title='Kiểm trùng đơn' rel='tooltip' title='' class='trans_func'><i class='icon-search'></i></a>";
                if (_base_bussiness.ActionMulty_("10,11,12,44", act)) { kiemtrung = ""; }
                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                string ngaynhan = ""; if (d.DNGAYNHAN != null) ngaynhan = func.ConvertDateVN(d.DNGAYNHAN.ToString());

                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p><strong>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) +
                    "</strong></p>" + diachi + "</td><td class='tcenter' nowrap>" + ngaynhan + "</td><td>" +
                    "<p>" + noidung + "</p>" + dontrung +
                    "</td><td class='tcenter' nowrap>" + edit + info + lichsu + kiemtrung + phanloai + "</td></tr>";
            }
            return str;
        }

        public string KNTC_Don_Khongthuocthamquyen(List<KNTCDON_MOICAPNHAT> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            var list = don.ToList();
            foreach (var d in list)
            {
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string edit = " <a href=\"/Kntc/Sua/?id=" + id_encr + "\"  onclick=\"ShowPageLoading()\" data-original-title='Sửa đơn' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string phanloai = " <a  onclick=\"ShowPageLoading()\" href=\"/Kntc/Phanloai?id=" + id_encr + "\" href='javascript:void()' data-original-title='Phân loại đơn' rel='tooltip' title='' class='trans_func'><i class='icon-list-ol'></i></a>";
                string chuyen = " <a href=\"javascript:void()\" data-original-title='Luân chuyển xử lý' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Luanchuyendonthu')\" class='trans_func'><i class='icon-signout'></i></a> ";
                if (!_base_bussiness.ActionMulty_("12,44", act)) { phanloai = ""; chuyen = ""; }
                if (!_base_bussiness.ActionMulty_("45,44", act)) { edit = ""; }
                string dontrung = "";
                string filemau = DDL_ChonDonKhongThuocThamQuyen((int)d.IDON);
                string kiemtrung = " <a  onclick=\"ShowPageLoading()\" href='/Kntc/Kiemtrung/?id=" + id_encr + "' data-original-title='Kiểm trùng đơn' rel='tooltip' title='' class='trans_func'><i class='icon-search'></i></a>";
                if (_base_bussiness.ActionMulty_("10,11,12,44", act)) { kiemtrung = ""; }
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                string ngaynhan = ""; if (d.DNGAYNHAN != null) ngaynhan = func.ConvertDateVN(d.DNGAYNHAN.ToString());
                str += "<tr><td class='tcenter b'>" + d.STT + "</td><td><p><strong>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) +
                     "</strong></p>" + diachi + "</td><td class='tcenter' nowrap>" +
                    ngaynhan + "</td><td><p>" + noidung + "</p>" + dontrung +
                     "</td><td class='tcenter' nowrap>" + chuyen +
                     "</td><td  class='tcenter' >" + filemau + "</td><td class='tcenter' nowrap>" + edit + info + lichsu + kiemtrung + phanloai + "</td></tr>";
            }
            return str;
        }

        public string DDL_ChonDonKhongThuocThamQuyen(int id)
        {
            string str = "";
            string ketqua = "Chọn mẫu đơn";
            //string id_encr = HashUtil.Encode_ID(iDon.ToString());

            str = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-primary dropdown-toggle'>" +
                ketqua + " <span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                "<li><a href='/Kntc/GiayBaoTin1/" + id + "' > Giấy báo tin 01</a></li>" +
                "<li><a href='/Kntc/GiayBaoTin2/" + id + "' > Giấy báo tin 02</a></li>" +
                "<li><a href='/Kntc/DeXuatXuLyDonKhieuNai/" + id + "' > Mẫu đề xuất đơn khiếu nại</a></li>" +
                 "<li><a href='/Kntc/HuongDanGuiDon/" + id + "'>Hướng dẫn, giải thích trả lời</a></li>" +
                "</ul></div>";
            return str;
        }

        public string KNTC_Don_Dudieukien(List<KNTCDON_MOICAPNHAT> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";

            var list = don.ToList();
            foreach (var d in list)
            {
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string edit = " <a href=\"/Kntc/Sua/?id=" + id_encr + "\"  onclick=\"ShowPageLoading()\" data-original-title='Sửa đơn' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string phanloai = " <a  onclick=\"ShowPageLoading()\" href=\"/Kntc/Phanloai/?id=" + id_encr + "\" href='javascript:void()' data-original-title='Phân loại đơn' rel='tooltip' title='' class='trans_func'><i class='icon-list-ol'></i></a>";
                string dontrung = "";
                string del = " <a href=\"javascript:void()\" title='Xóa' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kntc/Ajax_Kntc_del','Bạn có muốn xóa khiếu nại này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string kiemtrung = " <a  onclick=\"ShowPageLoading()\" href='/Kntc/Kiemtrung/?id=" + id_encr + "' data-original-title='Kiểm trùng đơn' rel='tooltip' title='' class='trans_func'><i class='icon-search'></i></a>";
                if (_base_bussiness.ActionMulty_("10,11,12,44", act)) { kiemtrung = ""; }
                if (!_base_bussiness.ActionMulty_("12,44", act)) { phanloai = ""; }
                if (!_base_bussiness.ActionMulty_("45,44", act)) { edit = ""; }
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                string ngaynhan = ""; if (d.DNGAYNHAN != null) ngaynhan = func.ConvertDateVN(d.DNGAYNHAN.ToString());

                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p><strong>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) +
                    "</strong></p>" + diachi + "</td><td class='tcenter' nowrap>" + ngaynhan + "</td><td>" +
                    "<p>" + noidung + "</p>" + dontrung +
                    "</td><td class='tcenter' nowrap>" + DDL_KetQuaXuLy((int)d.IDON, act.iUser, id_encr) +
                    "</td><td class='tcenter' nowrap>" + edit + info + lichsu + kiemtrung + phanloai + del + "</td></tr>";
            }
            return str;
        }

        public string DDL_KetQuaXuLy(int iDon, int iUser, string id_encr)
        {
            string str = "";
            KNTC_DON don = kntc.GetDON(iDon);
            string ketqua = "Chọn hình thức";
            //string id_encr = HashUtil.Encode_ID(iDon.ToString());
            if (!_base.Action(13, iUser))
            {
                return "<p class='tcenter'>Đang nghiên cứu</p>";
            }
            string dangnghiencuu = "<li><a href='#' onclick=\"UpdateTrangthai('id=" + id_encr + "','/Kntc/Ajax_Update_ketquaxuly')\">Đang nghiên cứu</a></li>";
            if (don != null)
            {
                if (don.IDUDIEUKIEN_KETQUA == -1)
                {
                    ketqua = "Đang nghiên cứu"; dangnghiencuu = "";
                }
            }

            str = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-primary dropdown-toggle'>" +
                ketqua + " <span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" + dangnghiencuu +
                "<li><a href='#' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuyenxuly_noibo')\">Chuyển đơn</a></li>" +
                "<li><a href='#' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Huongdan_traloi')\">Hướng dẫn, giải thích trả lời</a></li>" +
                "<li><a href='#' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Luutheodoi')\">Không chuyển</a></li>" +
                "</ul></div>";
            return str;
        }

        public string DDL_ChonDonXuLy(int id)
        {
            string str = "";
            string ketqua = "<i class='icon-file-alt'></i>";
            //string id_encr = HashUtil.Encode_ID(id.ToString());

            str = "<div class='btn-group'><a  href=\"javascript:void()\" data-toggle='dropdown' class='trans_func '>" +
                ketqua + "</a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                "<li><a href='/Kntc/PheuChuyenDonCoBaoCao/" + id + "' >Đơn có báo cáo</a></li>" +
                "<li><a href='/Kntc/PheuChuyenDonKhongBaoCao/" + id + "' >Đơn không báo cáo</a></li>" +

                "</ul></div>";
            return str;
        }

        public string KNTC_Don_Danhanxuly(List<KNTCDON_MOICAPNHAT> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            UserInfor u = GetUserInfor();
            var list = don.OrderByDescending(x => x.DNGAYNHAN).ToList();
            foreach (var d in list)
            {
                // new id
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                // end
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                string luan_chuyen = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Luanchuyendonthu')\" href='javascript:void()' data-original-title='Luân chuyển xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-refresh'></i></a>";
                string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuyendon_khongthuocthamquyen')\" href='javascript:void()' data-original-title='Chuyển đơn vị thẩm quyền xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
                string tiepnhanxuly = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Văn bản thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
                //string tiepnhanxuly = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Tiếp nhận xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-inbox'></i></a>";
                if (!_base_bussiness.ActionMulty_("13,44", act)) { luan_chuyen = ""; }
                if (!_base_bussiness.ActionMulty_("45,44", act)) { chuyen_xuly = ""; }
                if (!_base_bussiness.ActionMulty_("14,44", act)) { tiepnhanxuly = ""; }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p class='b'>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</p>" + diachi + "</td><td><p>" + noidung + "</p>" +
                        "</td><td nowrap class='tcenter'>" +
                       luan_chuyen + chuyen_xuly + tiepnhanxuly + info + lichsu + "</td></tr>";
            }
            return str;
        }

        public string KNTC_Don_Luanchuyenxuly(List<KNTCDON> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            UserInfor u = GetUserInfor();
            var list = don.OrderBy(x => x.STT).ToList();

            foreach (var d in list)
            {
                // new id
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                // end
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                //string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuyenxuly_donvi')\" href='javascript:void()' data-original-title='Xử lý đơn' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
                //string dondoc = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanbandondoc')\" data-original-title='Văn bản đôn đốc thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-screenshot'></i></a>";
                string donvithuly = "<p>Đơn vị thụ lý: <em>" + d.CTEN + "</em></p>";
                string tranthaidon = "<p>Trạng thái đơn: <em>" + GetTinhTrangDon((decimal)d.ITINHTRANGXULY) + "</em></p>";
                //string donvitiepnhan = "<p>Đơn vị tiếp nhận: <em>" + d.DONVITIEPNHAN + " </em></p>";
                //string vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Văn bản thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
                string ghichu = "";
                string xulylai = "";
                string cten = "";
                if (d.ITINHTRANGXULY == (int)TrangThaiDon.HoanThanh)
                {
                    if (d.CTEN != null)
                        cten = d.CTEN;
                    ghichu = "<p class=''><em>" + cten + " đã hoàn thành xử lý đơn</em></p>";
                    // dondoc = ""; // Nếu như đơn đã hoành thành thì không có chức năng đôn đốc đơn
                    //xulylai = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuathoadang')\" data-original-title='Xử lý lại đơn chưa thỏa đáng' rel='tooltip' title='' class='trans_func'><i class='icon-retweet'></i></a>";
                    //vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_lienquan')\" data-original-title='Văn bản liên quan' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
                }
                else if (d.ITINHTRANGXULY == (int)TrangThaiDon.DangXuLy)
                {
                    if (d.CTEN != null)
                        cten = d.CTEN;
                    ghichu = "<p class=''><em>" + cten + " đang xử lý đơn</em></p>";
                    //vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Văn bản thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
                }
                else if (d.ITINHTRANGXULY == (int)TrangThaiDon.ChoXuLy)
                {
                    if (d.CTEN != null)
                        cten = d.CTEN;
                    ghichu = "<p class=''><em>Đã chuyển cho " + cten.ToLower() + " xử lý đơn</em</p>";
                    //vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Văn bản thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
                }
                else if (d.ITINHTRANGXULY == (int)TrangThaiDon.KhongXuLy)
                {
                    if (d.CTEN != null)
                        cten = d.CTEN;
                    //vanban_add = "";
                    //dondoc = "";
                    ghichu = "<p class=''><em>Đơn đã được " + cten + " lưu theo dõi, không xử lý đơn </em</p>";
                }
                // if (!_base_bussiness.ActionMulty_("14,44", act)) { vanban_add = ""; }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p class='b'>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</p>" + diachi + "</td><td><p>" + noidung + "</p>" + ghichu + "" +
                        "</td><td nowrap class='tcenter'>" +
                      info + lichsu + "</td></tr>";
            }

            return str;
        }

        public string KNTC_Don_Chuyenxuly(List<KNTCDON> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            var list = don.OrderBy(x => x.STT).ToList();

            foreach (var d in list)
            {
                // new id
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                // end
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                //string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuyenxuly_donvi')\" href='javascript:void()' data-original-title='Xử lý đơn' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
                string dondoc = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanbandondoc')\" data-original-title='Văn bản đôn đốc thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-screenshot'></i></a>";
                string donvithuly = "<p>Đơn vị thụ lý: <em>" + d.CTEN + "</em></p>";
                string hanphanhoi = "";
                if (d.ICANHBAO != null)
                {
                    if (d.ICANHBAO == 2)
                        hanphanhoi += "<i class='f-red'>Quá hạn</i>";
                    if (d.ICANHBAO == 1)
                        hanphanhoi += "Sắp đến hạn";
                    if (d.ICANHBAO == 0)
                        hanphanhoi += "Trong hạn";
                }
                hanphanhoi += "</em></p>";
                //string donvitiepnhan = "<p>Đơn vị tiếp nhận: <em>" + d.DONVITIEPNHAN + " </em></p>";
                string vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Trả lời đơn chuyển' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
                string ghichu = "";
                string xulylai = "";
                string giamsat = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Giamsat/?id=" + id_encr + "' data-original-title='Kế hoạch giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-zoom-in'></i></a> ";
                string trangthai = GetTinhTrangDonDaXuLy((decimal)d.ITINHTRANGXULY);
                if (d.ITINHTRANGXULY == (int)TrangThaiDon.HoanThanh)
                {
                    ghichu = "<p class=''><em>" + d.CTEN + " đã hoàn thành xử lý đơn</em></p>";
                    dondoc = ""; // Nếu như đơn đã hoành thành thì không có chức năng đôn đốc đơn
                    xulylai = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuathoadang')\" data-original-title='Xử lý lại đơn chưa thỏa đáng' rel='tooltip' title='' class='trans_func'><i class='icon-retweet'></i></a>";
                    vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_lienquan')\" data-original-title='Văn bản liên quan' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
                }
                else if (d.ITINHTRANGXULY == (int)TrangThaiDon.DangXuLy)
                {
                    ghichu = "<p class=''><em>" + d.CTEN + " đang xử lý đơn</em></p>";
                    giamsat = "";
                    //vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Văn bản thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
                }
                else if (d.ITINHTRANGXULY == (int)TrangThaiDon.ChoXuLy)
                {
                    ghichu = "<p class=''><em>Đã chuyển cho " + d.CTEN + " xử lý đơn</em</p>";
                    giamsat = "";
                    //vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Văn bản thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
                }
                else if (d.ITINHTRANGXULY == (int)TrangThaiDon.KhongXuLy)
                {
                    vanban_add = "";
                    dondoc = "";
                    giamsat = "";
                    xulylai = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuathoadang')\" data-original-title='Xử lý lại đơn chưa thỏa đáng' rel='tooltip' title='' class='trans_func'><i class='icon-retweet'></i></a>";
                    ghichu = "<p class=''><em>Đơn đã được " + d.CTEN + " lưu theo dõi, không xử lý đơn </em</p>";
                }
                //if (!_base_bussiness.ActionMulty_("14,44", act)) { vanban_add = ""; }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p class='b'>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</p>" + diachi + "</td><td><p>" + noidung + "</p>" + ghichu + "" +
                        "</td><td>" + trangthai + "</td><td>" + hanphanhoi + "</td><td nowrap class='tcenter'>" +
                     xulylai + giamsat + vanban_add + dondoc + info + lichsu + "</td></tr>";
            }

            return str;
        }

        public string DDL_ChonDonChuyenXuLy(int id)
        {
            string str = "";
            string ketqua = "Chọn mẫu đơn";
            //string id_encr = HashUtil.Encode_ID(iDon.ToString());

            str = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-primary dropdown-toggle'>" +
                ketqua + " <span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                "<li><a href='/Kntc/TraDonKhieuNai/" + id + "' >Trả đơn khiếu nại</a></li>" +
                "<li><a href='/Kntc/KhongThuLyGiaiQuyet/" + id + "' >Không thụ lý giải quyết</a></li>" +
                "</ul></div>";
            return str;
        }

        public string dontrunglap_(int iDonTrung, string id_encr)
        {
            string str = "";
            if (iDonTrung != 0)
            {
                str = "<a href='javascript:void(0)' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_info')\" class='label label-warning'><i class='icon-warning-sign'></i> Đơn trùng lắp</a>";
            }
            return str;
        }

        public string loaivanban(string loai)
        {
            string _loai = "";
            if (loai == "ketqua")
            {
                _loai = " Văn bản liên quan đến quá trình giải quyết";
            }
            if (loai == "giaothuchien")
            {
                _loai = " Văn bản đôn đốc, giao đơn vị thực hiện";
            }
            if (loai == "bangiao")
            {
                _loai = " Văn bản bàn giao đơn vị khác thực hiện xử lý";
            }
            if (loai == "hoanthanh")
            {
                _loai = "Văn bản quyết định giải quyết";
            }
            if (loai == "dinhchi")
            {
                _loai = " Văn bản đình chỉ xử lý số";
            }
            if (loai == "khongthuly")
            {
                _loai = " Văn bản không thụ lý đơn";
            }
            if (loai == "huongdan_traloi")
            {
                _loai = " Văn bản hướng dẫn trả lời";
            }
            if (loai == "vanbanluanchuyendonthu")
            {
                _loai = " Văn bản luân chuyển đơn thư đến đơn vị phụ trách";
            }
            if (loai == "chuyenxuly_noibo")//chuyendon_khongthuocthamquyen
            {
                _loai = " Văn bản chuyển đơn vị thẩm quyền xử lý";
            }
            if (loai == "chuyendon_khongthuocthamquyen")//chuyendon_khongthuocthamquyen
            {
                _loai = " Văn bản chuyển đơn vị thẩm quyền xử lý";
            }
            if (loai == "vanbandondocthuchien") //chuyenxulylaidon
            {
                _loai = " Văn bản đôn đốc thực hiện";
            }
            if (loai == "chuyenxulylaidon") //chuyenxulylaidon
            {
                _loai = " Văn bản chuyển xử lý lại đơn";
            }
            return _loai;
        }

        public string Vanban_Moicapnhat(int idon)
        {
            string vanban_moicapnhat = "";
            Dictionary<string, object> _don = new Dictionary<string, object>();
            _don.Add("IDON", idon);
            List<KNTC_VANBAN> vanban = kntc.List_VanBan().Where(v => v.IDON == idon).ToList();
            if (vanban.Count() > 0)
            {
                KNTC_VANBAN v = vanban.OrderByDescending(x => x.DDATE).FirstOrDefault();
                if (v != null)
                {
                    string coquan_banhanh = kntc.GetDonVi((int)v.ICOQUANBANHANH).CTEN;
                    string ngaybanhanh = "";
                    string loaivanban = "";
                    if (v.DNGAYBANHANH != null)
                    {
                        ngaybanhanh = func.ConvertDateVN(v.DNGAYBANHANH.ToString());
                    }
                    if (v.CLOAI == "ketqua")
                    {
                        loaivanban = " Văn bản liên quan đến quá trình giải quyết:";
                    }
                    if (v.CLOAI == "giaothuchien")
                    {
                        loaivanban = " Văn bản đôn đốc, giao đơn vị thực hiện số: ";
                    }
                    if (v.CLOAI == "bangiao")
                    {
                        loaivanban = " Văn bản bàn giao đơn vị khác thực hiện xử lý: ";
                    }
                    if (v.CLOAI == "hoanthanh")
                    {
                        loaivanban = "Quyết định giải quyết:";
                    }
                    if (v.CLOAI == "dinhchi")
                    {
                        loaivanban = " Văn bản đình chỉ xử lý số: ";
                    }
                    if (v.CLOAI == "khongthuly")
                    {
                        loaivanban = " Văn bản không thụ lý đơn:";
                    }
                    if (v.CLOAI == "huongdan_traloi")
                    {
                        loaivanban = " Văn bản hướng dẫn trả lời:";
                    }
                    if (v.CLOAI == "vanbanluanchuyendonthu")
                    {
                        loaivanban = " Văn bản luân chuyển đơn thư đến đơn vị phụ trách:";
                    }
                    if (v.CLOAI == "chuyenxuly_noibo")
                    {
                        loaivanban = " Văn bản chuyển đơn vị thẩm quyền xử lý:";
                    }
                    if (v.CLOAI == "vanbandondocthuchien")
                    {
                        loaivanban = " Văn bản đôn đốc thực hiện:";
                    }
                    vanban_moicapnhat = "<p>" + loaivanban + " Số <strong>" + v.CSOVANBAN + "</strong> của " + coquan_banhanh +
                        " ban hành ngày " + ngaybanhanh + ". " + File_View((int)v.IVANBAN, "kntc_vanban");
                    vanban_moicapnhat += "</p>";
                }
            }
            return vanban_moicapnhat;
        }

        public string KNTC_Don_Dangxuly(List<KNTCDON_MOICAPNHAT> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            UserInfor u = GetUserInfor();
            var list = don.OrderByDescending(x => x.DNGAYNHAN).ToList();

            foreach (var d in list)
            {
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string vanban_moicapnhat = Vanban_Moicapnhat((int)d.IDON);
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Văn bản thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
                if (!_base_bussiness.ActionMulty_("14,44", act)) { vanban_add = ""; }
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);

                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p class='b'>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</p>" + diachi + "</td><td><p>" + noidung + "</p>" +
                        "</td><td>" + vanban_moicapnhat + "</td><td class='tcenter' nowrap>" + vanban_add + info + lichsu + "</td></tr>";
            }

            return str;
        }

        public string KNTC_Don_Daxuly(List<KNTCDON_MOICAPNHAT> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            var list = don.ToList();
            foreach (var d in list)
            {
                // new id
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                string giamsat = ""; //" <a onclick=\"ShowPageLoading()\" href='/Kntc/Giamsat/?id=" + id_encr + "' data-original-title='Kế hoạch giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-zoom-in'></i></a> ";
                string vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_lienquan')\" data-original-title='Văn bản liên quan' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";

                string vanban_moicapnhat = Vanban_Moicapnhat((int)d.IDON);
                string ketqua = "";
                KNTC_DON _don = kntc.GetDON((int)d.IDON);
                if (_don != null)
                {
                    if (_don.IDANHGIA != null)
                    {
                        if (_don.IDANHGIA != 0)
                        {
                            ketqua = "Kết quả đánh giá: <em class='b'>" + GetNameDanhgia((int)_don.IDANHGIA) + "</em>";
                        }
                    }
                }

                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p class='b'>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</p>" + diachi + "</td><td class=''>" +
                       noidung + "</td><td><p>" + vanban_moicapnhat + "</p><p>" + ketqua + "</p>" +
                        "</td><td class='tcenter' nowrap>" + vanban_add + giamsat + info + lichsu + "</td></tr>";
            }
            return str;
        }

        //public string KNTC_Don_Dachuyenxuly(List<KNTCDON> don, TaikhoanAtion act, int page, int iDonvi, int iTrangthai)
        //{
        //    string str = "";
        //    var list = don.ToList();
        //    if (page != 0)
        //    {
        //        list = don.OrderByDescending(x => x.DNGAYNHAN).ToPagedList(page, pageSize).ToList();
        //    }
        //    UserInfor u = GetUserInfor();
        //    int total = don.OrderByDescending(x => x.DNGAYNHAN).Count();
        //    if (iTrangthai == 0)//Đang chờ xử lý
        //    {
        //        int count = 1;
        //        string url_cookie = func.Get_Url_keycookie();
        //        if (total > 0)
        //        {
        //            foreach (var d in list)
        //            {
        //                // new id
        //                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
        //                // end
        //                string diachi = "";
        //                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
        //                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
        //                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
        //                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
        //                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
        //                //string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuyenxuly_donvi')\" href='javascript:void()' data-original-title='Xử lý đơn' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
        //                string chuyen_xuly = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Văn bản thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
        //                string donvithuly = "<p>Đơn vị thụ lý: <em>" + d.CTEN + "</em></p>";
        //                string donvitiepnhan = "<p>Đơn vị tiếp nhận: <em>" + d.DONVITIEPNHAN + " </em></p>";
        //                string vanbanchuathoadang_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuathoadang')\" data-original-title='Xử lý lại đơn chưa thỏa đáng' rel='tooltip' title='' class='trans_func'><i class='icon-retweet'></i></a>";
        //                if (!u.tk_action.is_lanhdao)
        //                {
        //                    if (d.IDONVITHULY != (decimal)u.user_login.IDONVI)
        //                    {
        //                        chuyen_xuly = "";
        //                        donvitiepnhan = "";
        //                    }
        //                    else
        //                    {
        //                        donvithuly = "";
        //                    }
        //                }
        //                else
        //                {
        //                    if (d.IDONVITIEPNHAN == (decimal)u.user_login.IDONVI)
        //                    {
        //                        donvitiepnhan = "";
        //                    }
        //                    else
        //                    {
        //                        donvithuly = "";
        //                    }
        //                }
        //                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td><p class='b'>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</p>" + diachi + "</td><td><p>" + noidung + "</p>" + donvithuly + donvitiepnhan + "" +
        //                        "</td><td nowrap class='tcenter'>" +
        //                        vanbanchuathoadang_add + chuyen_xuly + info + lichsu + "</td></tr>";

        //                count++;
        //            }
        //            if (page != 0)
        //            {
        //                str += "<tr><td colspan='6'>" + PhanTrang(total, pageSize, page, "" +
        //                   "/Kntc/Dachuyenxuly/?&trangthai=" + iTrangthai + "") + "</td></tr>";
        //            }

        //        }
        //    }
        //    else if (iTrangthai == 1)//Đang xử lý
        //    {
        //        int count = 1;
        //        string url_cookie = func.Get_Url_keycookie();
        //        if (total > 0)
        //        {
        //            foreach (var d in list)
        //            {
        //                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
        //                string vanban_moicapnhat = Vanban_Moicapnhat((int)d.IDON);
        //                string diachi = "";
        //                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
        //                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
        //                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
        //                string vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_add')\" data-original-title='Văn bản thực hiện' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
        //                if (!_base_bussiness.ActionMulty_("14,44", act)) { vanban_add = ""; }
        //                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
        //                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);

        //                string donvithuly = "<p>Đơn vị thụ lý: <em>" + d.CTEN + "</em></p>";
        //                string donvitiepnhan = "<p>Đơn vị tiếp nhận: <em>" + d.DONVITIEPNHAN + " </em></p>";
        //                if (!u.tk_action.is_lanhdao)
        //                {
        //                    if (d.IDONVITHULY != (decimal)u.user_login.IDONVI)
        //                    {
        //                        vanban_add = "";
        //                        donvitiepnhan = "";
        //                    }
        //                    else
        //                    {
        //                        donvithuly = "";
        //                    }
        //                }
        //                else
        //                {
        //                    if (d.IDONVITIEPNHAN == (decimal)u.user_login.IDONVI)
        //                    {
        //                        donvitiepnhan = "";
        //                    }
        //                    else
        //                    {
        //                        donvithuly = "";
        //                    }
        //                }
        //                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td><p class='b'>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</p>" + diachi + "</td><td><p>" + noidung + "</p>" +
        //                        "" + donvithuly + donvitiepnhan + "</td><td>" + vanban_moicapnhat + "</td><td class='tcenter' nowrap>" + vanban_add + info + lichsu + "</td></tr>";

        //                count++;
        //            }
        //            if (page != 0)
        //            {
        //                str += "<tr><td colspan='6'>" + PhanTrang(total, pageSize, page, "" +
        //                 "/Kntc/Dachuyenxuly/?&trangthai=" + iTrangthai + "") + "</td></tr>";

        //            }

        //        }
        //    }
        //    else// Đã xử lý
        //    {
        //        int count = 1;
        //        string url_cookie = func.Get_Url_keycookie();
        //        if (total > 0)
        //        {
        //            foreach (var d in list)
        //            {
        //                // new id
        //                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
        //                string diachi = "";
        //                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
        //                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
        //                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
        //                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
        //                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
        //                string giamsat = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Giamsat/?id=" + id_encr + "' data-original-title='Kế hoạch giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-zoom-in'></i></a> ";
        //                string vanban_add = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Vanban_lienquan')\" data-original-title='Văn bản liên quan' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a>";
        //                //string danhgia = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_DanhGia')\" data-original-title='Đánh giá đơn' rel='tooltip' title='' class='trans_func'><i class='icon-ok-circle'></i></a>";
        //                string vanban_moicapnhat = Vanban_Moicapnhat((int)d.IDON);
        //                string ketqua = "";
        //                if (d.IDANHGIA != null)
        //                {
        //                    if (d.IDANHGIA != 0)
        //                    {
        //                        ketqua = "Kết quả đánh giá: <em class='b'>" + GetNameDanhgia((int)d.IDANHGIA) + "</em>";
        //                    }
        //                }

        //                string donvithuly = "<p>Đơn vị thụ lý: <em>" + d.CTEN + "</em></p>";
        //                string donvitiepnhan = "<p>Đơn vị tiếp nhận: <em>" + d.DONVITIEPNHAN + " </em></p>";
        //                if (!u.tk_action.is_lanhdao)
        //                {
        //                    if (d.IDONVITHULY != (decimal)u.user_login.IDONVI)
        //                    {
        //                        giamsat = "";
        //                        vanban_add = "";
        //                        donvitiepnhan = "";
        //                    }
        //                    else
        //                    {
        //                        donvithuly = "";
        //                    }
        //                }
        //                else
        //                {
        //                    if (d.IDONVITIEPNHAN == (decimal)u.user_login.IDONVI)
        //                    {
        //                        donvitiepnhan = "";
        //                    }
        //                    else
        //                    {
        //                        donvithuly = "";
        //                    }
        //                }
        //                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td><p class='b'>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</p>" + diachi + "</td><td class=''>" +
        //                       noidung + "" + donvithuly + donvitiepnhan + "</td><td><p>" + vanban_moicapnhat + "</p><p>" + ketqua + "</p>" +
        //                        "</td><td class='tcenter' nowrap>" + vanban_add + giamsat + info + lichsu + "</td></tr>";

        //                count++;
        //            }
        //            if (page != 0)
        //            {
        //                str += "<tr><td colspan='6'>" + PhanTrang(total, pageSize, page, "" +
        //                   "/Kntc/Dachuyenxuly/?&trangthai=" + iTrangthai + "") + "</td></tr>";
        //            }

        //        }
        //    }
        //    return str;
        //}
        public string KNTC_Don_Khongxuly(List<KNTCDON_MOICAPNHAT> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            UserInfor u = GetUserInfor();
            var list = don.ToList();
            foreach (var d in list)
            {
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Don_lichsu')\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a> ";
                string noidung = ""; if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null) noidung = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                string edit = " <a href=\"/Kntc/Sua/?id=" + id_encr + "\"  onclick=\"ShowPageLoading()\" data-original-title='Sửa đơn' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kntc/Ajax_Don_del','Bạn muốn xóa đơn này')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string diachi = "";
                if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                if (HttpUtility.HtmlEncode(d.CTENTINH) != null) diachi += HttpUtility.HtmlEncode(d.CTENTINH) + " .";
                string trangthai = "";
                string xulylai = "";
                KNTC_DON _don = kntc.GetDON((int)d.IDON);
                if (_don != null)
                {
                    if (_don.IDANHGIA != null)
                    {
                        if (_don.ITINHTRANGXULY == (int)TrangThaiDon.KhongXuLy)
                        {
                            xulylai = "<a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Chuathoadang')\" data-original-title='Xử lý lại đơn chưa thỏa đáng' rel='tooltip' title='' class='trans_func'><i class='icon-retweet'></i></a>";
                        }
                        if (_don.ILUUTHEODOI == Convert.ToDecimal(LyDoLuuTheoDoi.DonTrung)) trangthai = StringEnum.GetStringValue(LyDoLuuTheoDoi.DonTrung);
                        else if (_don.ILUUTHEODOI == Convert.ToDecimal(LyDoLuuTheoDoi.KhongThuLy)) trangthai = StringEnum.GetStringValue(LyDoLuuTheoDoi.KhongThuLy);
                        else if (_don.ILUUTHEODOI == Convert.ToDecimal(LyDoLuuTheoDoi.KhongXuLy)) trangthai = StringEnum.GetStringValue(LyDoLuuTheoDoi.KhongXuLy);
                        else if (_don.ILUUTHEODOI == Convert.ToDecimal(LyDoLuuTheoDoi.DaTraLoi)) trangthai = StringEnum.GetStringValue(LyDoLuuTheoDoi.DaTraLoi);
                        else trangthai = StringEnum.GetStringValue(LyDoLuuTheoDoi.KhongThuLy);
                    }
                }
                if (!_base_bussiness.ActionMulty_("45,44", act)) { edit = ""; del = ""; }
                if (_don.IDONVITIEPNHAN != u.user_login.IDONVI) { edit = ""; del = ""; }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p class='b'>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) + "</p>" + diachi + "</td><td class=''>" +
                        noidung + "</td><td class='tcenter'>" + trangthai + "</td><td>" + lydo_khongxuly((int)d.IDON) + "</td><td class='tcenter' nowrap>" + edit + xulylai + info + lichsu + del + "</td></tr>";
            }
            return str;
        }

        public string lydo_khongxuly(int id)
        {
            KNTC_DON don = kntc.GetDON(id);
            string str = "";
            if (don.ILUUTHEODOI == Convert.ToDecimal(LyDoLuuTheoDoi.DonTrung))
            {
                str += don.CHITIETLYDO_LUUTHEODOI;
            }
            else if (don.ILUUTHEODOI == Convert.ToDecimal(LyDoLuuTheoDoi.KhongXuLy))
            {
                str += don.CHITIETLYDO_LUUTHEODOI;
            }
            else
            {
                List<KNTC_VANBAN> vb = kntc.List_VanBan().Where(v => v.IDON == (int)don.IDON && (v.CLOAI == "khongthuly" || v.CLOAI == "dinhchi")).ToList();
                if (vb.Count() > 0)
                {
                    KNTC_VANBAN kntc_vb = vb.OrderByDescending(x => x.IVANBAN).FirstOrDefault();
                    string donvi_banhanh = kntc.GetDonVi((int)kntc_vb.ICOQUANBANHANH).CTEN;
                    string file = File_View((int)kntc_vb.IVANBAN, "kntc_vanban");
                    if (kntc_vb.CLOAI == "khongthuly")
                    {
                        str += "Đơn không được thụ lý: Văn bản số <strong>" + kntc_vb.CSOVANBAN + "</strong> ban hành ngày " + func.ConvertDateVN(kntc_vb.DNGAYBANHANH.ToString()) +
                            " của " + donvi_banhanh + ". " + file;
                    }
                    if (kntc_vb.CLOAI == "dinhchi")
                    {
                        str += "Đơn bị đình chỉ xử lý: Văn bản số <strong>" + kntc_vb.CSOVANBAN + "</strong> ban hành ngày " + func.ConvertDateVN(kntc_vb.DNGAYBANHANH.ToString()) +
                            " của " + donvi_banhanh + ". " + file;
                    }
                }
            }
            return str;
        }

        public string LichSu_Don(decimal id_don)
        {
            string str = "";
            List<TRACKING> lichsu = kntc.List_TracKing().Where(v => v.IDON == id_don).OrderByDescending(v => v.DDATE).ToList();
            foreach (var l in lichsu)
            {
                //USERS user_giao = _user.GetByID();
                TaiKhoan tk = tl.Taikhoan_Detail((int)l.IUSER);
                string nguoigiao = "<strong>" + tk.ten + "</strong> (" +
                    tk.chucvu + ")";
                str += "<tr><td class='tcenter' nowrap>" + String.Format("{0:hh:mm dd/MM/yyyy}", Convert.ToDateTime(l.DDATE)) +
                    "</td><td class='tcenter' nowrap>" + nguoigiao +
                    "</td><td>" + l.CACTION + "</td></tr>";
            }
            return str;
        }

        public string Option_DonVi_XuLyDon(int id_thuly, int id_tiepnhan, int iTinhTrang = 0)
        {
            //string sql = "select distinct IDONVITHULY from KNTC_DON where ITINHTRANGXULY=4 order by IDONVITHULY";
            UserInfor u = GetUserInfor();
            var donvi = kntc.Get_IDDonvi_xulydon(iTinhTrang, id_tiepnhan);
            string str = "";

            //_don.Add("IDONVITHULY", id);
            foreach (var d in donvi)
            {
                int xl = (int)d.IDONVITHULY;
                QUOCHOI_COQUAN coquan = kntc.GetDonVi(xl);
                if (coquan != null)
                {
                    string select = "";
                    if (d.IDONVITHULY == id_thuly) select = "selected";
                    str += "<option " + select + " value='" + coquan.ICOQUAN + "'>" + coquan.CTEN + " (" + d.IDON + ")</option>";
                }
            }
            return str;
        }

        public string Option_DonVi_TiepNhanDon(int id_thuly, int id_tiepnhan, int iTinhTrang = 0)
        {
            //string sql = "select distinct IDONVITHULY from KNTC_DON where ITINHTRANGXULY=4 order by IDONVITHULY";
            UserInfor u = GetUserInfor();
            var donvi = kntc.Get_IDDonvi_Tiepnhan(iTinhTrang, id_tiepnhan);
            string str = "";

            //_don.Add("IDONVITHULY", id);
            foreach (var d in donvi)
            {
                int xl = (int)d.IDONVITIEPNHAN;
                QUOCHOI_COQUAN coquan = kntc.GetDonVi(xl);
                if (coquan != null)
                {
                    if (d.IDONVITIEPNHAN != id_tiepnhan)
                    {
                        string select = "";
                        if (d.IDONVITIEPNHAN == id_thuly) select = "selected";
                        str += "<option " + select + " value='" + coquan.ICOQUAN + "'>" + coquan.CTEN + " (" + d.IDON + ")</option>";
                    }
                }
            }
            return str;
        }

        public string Option_TrangThaiChuyenXuLy(int iTrangThai)
        {
            string[] trangthai = new string[] { "Chờ xử lý", "Đang xử lý", "Đã xử lý", "Không xử lý, lưu theo dõi" };
            string str = "";
            int count = 0;
            foreach (var a in trangthai)
            {
                string select = ""; if (count == iTrangThai) { select = " selected "; }
                str += "<option " + select + " value=" + count + ">" + a + "</option>";
                count++;
            }
            return str;
        }

        public string Option_LuuTheoDoi(int id_chucvu = 0)
        {
            string str = "";

            List<KNTC_LUUTHEODOI> chucvu = kntc.List_LuuTheoDoi().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.CTEN).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.ID == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.ID + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_LoaiDon(decimal id_chucvu = 0)
        {
            string str = "";

            List<KNTC_LOAIDON> chucvu = kntc.List_LoaiDon().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.ILOAIDON == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.ILOAIDON + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_Nguondon(List<KNTC_NGUONDON> nguondon, decimal id_chucvu = 0)
        {
            string str = "";
            var chucvu = nguondon.Where(x => x.IHIENTHI == 1 && x.IPARENT == 0 && x.IDELETE == 0).OrderBy(v => v.IVITRI).ToList();
            foreach (var p in chucvu)
            {
                if (nguondon.Where(v => v.IPARENT == p.INGUONDON).Count() > 0)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(p.CTEN) + "'>";
                    var list = nguondon.Where(x => x.IHIENTHI == 1 && x.IPARENT == p.INGUONDON && x.IDELETE == 0).OrderBy(v => v.IVITRI).ToList();
                    foreach (var d in list)
                    {
                        string select = ""; if (d.INGUONDON == id_chucvu) { select = " selected "; }
                        str += "<option " + select + " value='" + d.INGUONDON + "'>" + d.CTEN + "</option>";
                    }
                }
                else
                {
                    string select = ""; if (p.INGUONDON == id_chucvu) { select = " selected "; }
                    str += "<option " + select + " value='" + p.INGUONDON + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
                }
            }
            return str;
        }

        public string Option_NoiDungDon(int id_chucvu = 0, int ilinhvuc = 0)
        {
            string str = "";
            List<KNTC_NOIDUNGDON> chucvu = kntc.List_NoiDungDon().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderByDescending(x => x.IVITRI).ToList();
            if (ilinhvuc != 0)
            {
                chucvu = chucvu.Where(v => v.ILINHVUC == ilinhvuc).ToList();
            }
            foreach (var p in chucvu)
            {
                string select = ""; if (p.INOIDUNG == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.INOIDUNG + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_TinhChatDon(int id_chucvu = 0, int inoidung = 0)
        {
            string str = "";

            List<KNTC_TINHCHAT> chucvu = kntc.List_TinhChat().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderByDescending(x => x.IVITRI).ToList();
            if (inoidung != 0)
            {
                chucvu = chucvu.Where(v => v.INHOMNOIDUNG == inoidung).ToList();
            }
            foreach (var p in chucvu)
            {
                string select = ""; if (p.ITINHCHAT == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.ITINHCHAT + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string OptionCoQuan(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            var list = coquan.Where(x => x.IPARENT == id_parent && x.ICOQUAN != id_donvi_choice).OrderByDescending(x => x.IVITRI).ToList();
            foreach (var donvi in list)
            {

                if (coquan.Where(x => x.IPARENT == (int)donvi.ICOQUAN && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).Count() > 0 && donvi.IGROUP == 1)
                {
                    str += "<optgroup label='" + donvi.CTEN + "'>";
                    str += OptionCoQuan(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else if (coquan.Where(x => x.IPARENT == (int)donvi.ICOQUAN && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).Count() > 0 && donvi.IGROUP == 0)
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + donvi.CTEN + "</option>";
                    str += OptionCoQuan(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + donvi.CTEN + "</option>";
                }
            }
            return str;
        }

        public string OptionCoQuanByIparent(int id_parent = 0, int iDonvi = 0)
        {
            string str = "";
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("IPARENT", id_parent);
            condition.Add("IHIENTHI", 1);
            condition.Add("IDELETE", 0);
            var list = kntc.List_DonVi(condition).OrderByDescending(x => x.IVITRI).ToList();
            foreach (var d in list)
            {
                string select = "";
                if (d.ICOQUAN == iDonvi) { select = " selected "; }
                str += "<option " + select + " value='" + d.ICOQUAN + "'>" + d.CTEN + "</option>";
            }

            return str;
        }

        public string OptionTrangThai(int itrangthaidon = 0)
        {
            string str = "";
            List<TrangThaiDon> trangthai = Enum.GetValues(typeof(TrangThaiDon)).Cast<TrangThaiDon>().ToList();
            foreach (var a in trangthai)
            {
                string select = "";
                if (itrangthaidon == (decimal)a) { select = " selected "; }
                str += "<option " + select + " value='" + (decimal)a + "'>" + StringEnum.GetStringValue(a) + "</option>";
            }
            return str;
        }

        public string OptionTrangThaiDonXuLy(int itrangthaidon = 0)
        {
            string str = "";
            int[] map = new[] { 3, 4, 6, 5 };
            List<TrangThaiDonDaChuyen> trangthai = Enum.GetValues(typeof(TrangThaiDonDaChuyen)).Cast<TrangThaiDonDaChuyen>().ToList();
            str += "<option  value='0'>Xem tất cả</option>";
            foreach (var a in trangthai.OrderBy(v => v.ToString()))
            {
                string select = "";
                if (itrangthaidon == (decimal)a) { select = " selected "; }
                str += "<option " + select + " value='" + (decimal)a + "'>" + StringEnum.GetStringValue(a) + "</option>";
            }
            return str;
        }

        public string OptionTrangThaiDonDaTraLoi(int itrangthaidon = 0)
        {
            string str = "";
            int[] map = new[] { 3, 4, 6, 5 };
            List<TrangThaiDonDaTraLoi> trangthai = Enum.GetValues(typeof(TrangThaiDonDaTraLoi)).Cast<TrangThaiDonDaTraLoi>().ToList();
            str += "<option  value='0'>Xem tất cả</option>";
            foreach (var a in trangthai.OrderBy(v => v.ToString()))
            {
                string select = "";
                if (itrangthaidon == (decimal)a) { select = " selected "; }
                str += "<option " + select + " value='" + (decimal)a + "'>" + StringEnum.GetStringValue(a) + "</option>";
            }
            return str;
        }

        public string OptionTrangThaiDonChuaTraLoi(int itrangthaidon = 0)
        {
            string str = "";
            int[] map = new[] { 3, 4, 6, 5 };
            List<TrangThaiDonChuaTraLoi> trangthai = Enum.GetValues(typeof(TrangThaiDonChuaTraLoi)).Cast<TrangThaiDonChuaTraLoi>().ToList();
            str += "<option  value='0'>Xem tất cả</option>";
            foreach (var a in trangthai.OrderBy(v => v.ToString()))
            {
                string select = "";
                if (itrangthaidon == (decimal)a) { select = " selected "; }
                str += "<option " + select + " value='" + (decimal)a + "'>" + StringEnum.GetStringValue(a) + "</option>";
            }
            return str;
        }

        public string Row_Vanban_lienquan(int iDon)
        {
            Dictionary<string, object> _don = new Dictionary<string, object>();
            _don.Add("IDON", iDon);
            string str = "";
            str += "<table class='table table-bordered table-condensed nomargin'><tr><th width='3%' class='tcenter'>STT</th><th class='tcenter' nowrap>Loại văn bản</th><th nowrap class='tcenter'>Số văn bản</th><th nowrap class='tcenter'>Ngày ban hành</th><th nowrap class='tcenter'>Đơn vị ban hành</th><th nowrap class='tcenter'>Người ban hành / Chức vụ</th><th class='tcenter' nowrap>File</th><th class='tcenter' nowrap>Chức năng</th></tr>";
            var vanban = kntc.List_VanBan().Where(v => v.IDON == iDon).ToList();
            int stt = 0;
            foreach (var v in vanban.OrderByDescending(x => x.IVANBAN))
            {
                string ngaybanhanh = "";
                if (v.DNGAYBANHANH != null) { ngaybanhanh = func.ConvertDateVN(v.DNGAYBANHANH.ToString()); }
                QUOCHOI_COQUAN cq = kntc.GetDonVi((int)v.ICOQUANBANHANH);
                if (cq != null)
                {
                    stt++;
                    string nguoigui = "";
                    if (v.CCHUCVU != null) nguoigui = v.CCHUCVU + " - ";
                    if (v.CNGUOIKY != null) nguoigui += v.CNGUOIKY;
                    string loai = loaivanban(v.CLOAI);
                    str += "<tr><td class='tcenter'>" + stt + "</td><td>" + loai + "</td><td class='tcenter'>" + v.CSOVANBAN + "</td><td class='tcenter'>" + ngaybanhanh + "</td><td class=''>" + kntc.GetDonVi((int)v.ICOQUANBANHANH).CTEN + "</td><td class=''>" + nguoigui + "</td><td class='tcenter'>" + File_View((int)v.IVANBAN, "kntc_vanban") + "</td>";
                    // str += "<p><strong>- Văn bản số " + v.CSOVANBAN + " của " + kntc.GetDonVi((int)v.ICOQUANBANHANH).CTEN +
                    //" ban hành ngày " + ngaybanhanh + "</strong> " + File_View((int)v.IVANBAN, "kntc_vanban") + "</br>" + v.CNOIDUNG + "</p>";
                    str += "<td class='tcenter'>";
                    string popup = "";
                    if (v.CLOAI == "vanbandondocthuchien") popup = " onclick=\"ShowPopUp('', '/Kntc/Ajax_popup_in_congvandondoc?idVanban=" + v.IVANBAN.ToString() + "')\" ";
                    if (v.CLOAI.Contains("chuyenxuly")) popup = " onclick=\"ShowPopUp('', '/Kntc/Ajax_popup_in_phieuchuyendon?idVanban=" + v.IVANBAN.ToString() + "')\" ";
                    str += "<a href='javascript:void(0)'" + popup + "rel='tooltip' title='In văn bản' class='trans_func'><i class='icon-print'></i><a>";
                    str += "</td></tr>";
                }
            }
            str += "</table>";
            return str;
        }

        //public string File_View(int id, string type)
        //{
        //    string str = "";
        //    var file = kntc.List_File().Where(v => v.ID == id && v.CTYPE == type).ToList();
        //    foreach (var f in file)
        //    {
        //        str += " <a href='" + f.CFILE + "' class='f-green'><i class='icon-download-alt'></i></a>";
        //    }
        //    return str;
        //}
        public string File_Edit(int id, string type, string url_cookie)
        {
            string str = "";
            var file = kntc.List_File().Where(v => v.ID == id && v.CTYPE == type).ToList();
            foreach (var f in file)
            {
                string id_encr = HashUtil.Encode_ID(f.ID_FILE.ToString(), url_cookie);
                string[] f_ = f.CFILE.Split('/');
                str += "<p id='file_" + id_encr + "'><a href='/Home/DownLoad/" + id_encr + "'><i class='icon-download-alt'></i></> <a href='javascript:void(0)' onclick=\"DeleteFile('" + id_encr +
                        "', '/Kntc/Ajax_Delele_file')\" class='f-orangered' title='Hủy'><i class='icon-remove'></i><a></p>";
            }
            return str;
        }

        public string Option_Nhom_LinhVuc(List<LINHVUC> linhvuc, int iD, int iLoaidon)
        {
            string str = "";
            var list = linhvuc.Where(x => x.IPARENT == 0 && x.IDELETE == 0 && x.IHIENTHI == 1).OrderByDescending(v => v.IVITRI).ToList();
            foreach (var d in list)
            {
                var list2 = linhvuc.Where(x => x.IPARENT == d.ILINHVUC && x.IDELETE == 0 && x.IHIENTHI == 1 && x.ILOAIDON == iLoaidon).OrderByDescending(v => v.IVITRI).ToList();
                if (iLoaidon == 0)
                {
                    list2 = linhvuc.Where(x => x.IPARENT == d.ILINHVUC && x.IDELETE == 0 && x.IHIENTHI == 1).OrderByDescending(v => v.IVITRI).ToList();
                }
                if (linhvuc.Count() > 0)
                {
                    str += "<optgroup label='" + d.CTEN + "'>";

                    foreach (var p in list2)
                    {
                        string select = ""; if (p.ILINHVUC == iD && p.ILOAIDON == iLoaidon) { select = " selected "; }
                        str += "<option " + select + " value='" + p.ILINHVUC + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
                    }
                }
            }

            return str;
        }

        public string OptionLinhVuc(List<LINHVUC> linhvuc, int iD)
        {
            string str = "";
            string space_level = "- - - ";

            var linhvuc1 = linhvuc.Where(x => x.IDELETE == 0).OrderByDescending(x => x.CTEN).ToList();
            foreach (var t in linhvuc1)
            {
                string select = ""; if (t.ILINHVUC == iD) { select = " selected "; }
                str += "<option " + select + " value=" + t.ILINHVUC + ">" + space_level + t.CTEN + "</option>";
            }
            return str;
        }

        public string Option_NoiDungDon_ThuocLinhVuc(decimal id_chucvu = 0, decimal iLinhvuc = 0)
        {
            string str = "";
            List<KNTC_NOIDUNGDON> chucvu = _thietlap.Get_Noidungdon().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            if (iLinhvuc != 0)
            {
                chucvu = _thietlap.Get_Noidungdon().Where(x => x.IHIENTHI == 1 && x.ILINHVUC == iLinhvuc && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            }
            foreach (var p in chucvu)
            {
                string select = ""; if (p.INOIDUNG == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.INOIDUNG + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_TinhChatDon_ThuocNguonDon(decimal id_chucvu = 0, decimal iNoidungdon = 0)
        {
            string str = "";

            List<KNTC_TINHCHAT> chucvu = _thietlap.Get_Tinhchat().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            if (iNoidungdon != 0)
            {
                chucvu = _thietlap.Get_Tinhchat().Where(x => x.IHIENTHI == 1 && x.INHOMNOIDUNG == iNoidungdon && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            }
            foreach (var p in chucvu)
            {
                string select = ""; if (p.ITINHCHAT == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.ITINHCHAT + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_LinhVuc_LoaiDon(decimal id_chucvu = 0, decimal iLoaiDon = 0)
        {
            string str = "";
            List<LINHVUC> chucvu = _thietlap.Get_Linhvuc().Where(v => v.IPARENT == 0 && v.IDELETE == 0 && v.IHIENTHI == 1).ToList();
            List<LINHVUC> chucvuCon;
            foreach (var d in chucvu)
            {
                chucvuCon = chucvu.Where(x => x.IPARENT == d.ILINHVUC).OrderBy(x => x.IVITRI).ToList();
                if (iLoaiDon != 0)
                {
                    chucvuCon = chucvuCon.Where(x => x.ILOAIDON == iLoaiDon).OrderBy(x => x.IVITRI).ToList();
                }
                if (chucvuCon.Count() > 0)
                {
                    str += "<optgroup label='" + d.CTEN + "'>";

                    foreach (var p in chucvuCon)
                    {
                        string select = ""; if (p.ILINHVUC == id_chucvu) { select = " selected "; }
                        str += "<option " + select + " value='" + p.ILINHVUC + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
                    }
                }
                else
                {
                    if (!(iLoaiDon != 0 && d.ILOAIDON != iLoaiDon))
                    {
                        string select = ""; if (d.ILINHVUC == id_chucvu) { select = " selected "; }
                        str += "<option " + select + " value='" + d.ILINHVUC + "'>" + HttpUtility.HtmlEncode(d.CTEN) + "</option>";
                    }
                }
            }

            return str;
        }

        public string Option_Domat(decimal iDoMat)
        {
            string str = "";
            List<DoMat> trangthai = Enum.GetValues(typeof(DoMat)).Cast<DoMat>().ToList();
            foreach (var a in trangthai)
            {
                string select = "";
                if (iDoMat == (decimal)a) { select = " selected "; }
                str += "<option " + select + " value='" + (decimal)a + "'>" + StringEnum.GetStringValue(a) + "</option>";
            }
            return str;
        }

        public string Option_Dokhan(decimal iDoMat)
        {
            string str = "";
            List<DoKhan> trangthai = Enum.GetValues(typeof(DoKhan)).Cast<DoKhan>().ToList();
            foreach (var a in trangthai)
            {
                string select = "";
                if (iDoMat == (decimal)a) { select = " selected "; }
                str += "<option " + select + " value='" + (decimal)a + "'>" + StringEnum.GetStringValue(a) + "</option>";
            }
            return str;
        }

        public string Option_Danhgia(decimal iDanhGia)
        {
            string str = "";
            List<KetQuaDanhGia> trangthai = Enum.GetValues(typeof(KetQuaDanhGia)).Cast<KetQuaDanhGia>().ToList();
            foreach (var a in trangthai)
            {
                string select = "";
                if (iDanhGia == (decimal)a) { select = " selected "; }
                str += "<option " + select + " value='" + (decimal)a + "'>" + StringEnum.GetStringValue(a) + "</option>";
            }
            return str;
        }

        public string Option_HinhThuc(decimal iHinhThuc)
        {
            string str = "";
            List<HienThiDon> trangthai = Enum.GetValues(typeof(HienThiDon)).Cast<HienThiDon>().ToList();
            foreach (var a in trangthai)
            {
                string select = "";
                if (iHinhThuc == (decimal)a) { select = " selected "; }
                str += "<option " + select + " value='" + (decimal)a + "'>" + StringEnum.GetStringValue(a) + "</option>";
            }
            return str;
        }

        public string GetNameDanhgia(decimal tt)
        {
            string tinhtrang = "";
            List<KetQuaDanhGia> trangthai = Enum.GetValues(typeof(KetQuaDanhGia)).Cast<KetQuaDanhGia>().ToList();
            foreach (var a in trangthai)
            {
                if (tt == (decimal)a)
                {
                    tinhtrang = StringEnum.GetStringValue(a);
                    return tinhtrang;
                };
            }
            return tinhtrang;
        }

        // Phần báo cáo
        public string Thongkeloaikhieuto(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";
            var list = kntcrpt.getReportBaoBaoThongKeLoaiKhieuTo("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_LOAIKHIEUTO", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);

            decimal Total = 0;
            decimal TotalTyle = 0;
            if (list != null && list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    Total += d.SOLUONG;
                    TotalTyle += d.TYLE;
                    str += "<tr><td class='tcenter'>" + d.STT + "</td><td>" + d.CTEN + "</td><td class='tright'>" + d.SOLUONG + "</td><td  class='tright'>" + d.TYLE + "</td></tr>";
                }
                str += "<tr><td class='b' colspan='2'>TỔNG CỘNG: </td><td class='tright'>" + Total + "</td><td class='tright'>" + TotalTyle + "</td></tr>";
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }
        public string Get_TenKhoaHop_By_IDKyHop(int iKhoa)
        {
            string str = "";
            QUOCHOI_KHOA khoa = kntc.Get_Khoa_QuocHoi(iKhoa);
            if (khoa != null)
            {
                str = khoa.CTEN;
            }

            return str;
        }

        public string Baocaodonthuhangtuan(DateTime tungay, DateTime denngay, int iNam)
        {
            string str = "";
            var list = kntcrpt.getReportBaocaodonthuhangtuan("PKG_KNTC_BAOCAO.PRO_BAOCAO_DONTHUHANGTUAN", tungay, denngay, iNam);
            list = list.ToList();
            var list1 = list.Where(x => x.ITINHTRANGXULY == 6).ToList();
            var list2 = list.Where(x => x.ITINHTRANGXULY == 3 && x.IDOITUONGGUI == 0).ToList();
            var list3 = list.Where(x => x.ITINHTRANGXULY == 3 && x.IDOITUONGGUI == 1).ToList();
            string nam = denngay.ToString("yyyy");
            string thang = denngay.ToString("MM");
            str += "<table class='table table-condensed table-bordered'>" +
                "<tr><td colspan='5' class='tcenter b'>VĂN PHÒNG ĐOÀN ĐBQH VÀ<br/>HĐND TỈNH THANH HÓA</td><td colspan='5' class='tcenter b'>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</td></tr>" +
                        "<tr><td colspan='5' class='tcenter b'><u>Số:...../BC-VP</u></td><td class='tcenter b'><u>Độc lập - Tự do - Hạnh phúc</u></td></tr>" +
                        "<tr><td colspan= '12' class='tright'><i>Thanh Hóa, ngày...tháng...năm...</i></td></tr>" +
                        "<tr><td colspan = '12' class='tcenter b'>BÁO CÁO</td></tr>" +
                        "<tr><td colspan = '12' class='tcenter b'>Kết quả tiếp công dân, tiếp nhận, xử lý đơn khiếu nại, tố cáo từ ngày "+ tungay.ToString("dd/MM/yyyy") + " đến ngày  " + denngay.ToString("dd/MM/yyyy") + " của Đoàn đại biểu Quốc hội và " +
                        "<br/>Thường trực HĐND Tỉnh Thanh Hóa</td></tr>" +
                        "<tr><td colspan = '12' class='tleft b'>1. Tình hình tiếp công dân tại trụ sở tiếp công dân tỉnh</td></tr>" +
                        "<tr><td colspan = '12' class='tleft b'><i>1.1. Đoàn đại biểu Quốc hội tỉnh</i></td></tr>" +
                        "<tr><td colspan = '12' class='tleft b'><i>1.2. Thường trực Hội đồng nhân dân tỉnh</i></tr>" +
                        "<tr><td colspan = '12' class='tleft b'>2. Về kết quả tiếp nhận, phân loại, xử lý đơn, theo dõi việc giải quyết đơn khiếu nại, tố cáo, phản ánh, kiến nghị của công dân</td></tr>" +
                        "<tr><td colspan = '12' class='tleft b'><i>2.1. Đoàn đại biểu Quốc hội tỉnh</i></td></tr>" +
                        "<tr><td colspan = '12' class='tleft b'><i>2.2. Thường trực Hội đồng nhân dân tỉnh</i></td></tr>" +
                        "<tr><td colspan = '12' class='tleft b'>3. Các vụ việc nổi cộm, phức tạp</td></tr>" +
                        "<tr><td colspan='5' class='tcenter b'>VĂN PHÒNG ĐOÀN ĐBQH VÀ<br/>HĐND TỈNH THANH HÓA</td><td colspan='5' class='tcenter b'>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM<br/>" +
                        "<u>Độc lập - Tự do - Hạnh phúc</u></td></tr>" +
                        "<tr><td colspan = '12' class='tcenter b'>DANH SÁCH CÁC ĐƠN VỊ ĐÃ TRẢ LỜI ĐƠN CHUYỂN CỦA ĐOÀN ĐBQH TỈNH VÀ THƯỜNG TRỰC HĐND TỈNH</td></tr>" +
                        "<tr><td colspan = '12' class='tcenter b'><i>(Kèm theo Báo cáo số:....../BC-VP ngày....tháng" + thang + "năm " + nam + " của Văn phòng Đoàn ĐBQH và HĐND tỉnh Thanh Hóa)</i></td></tr>" +
                        "<tr>" +
                        "<td class='tcenter b'>TT</td> <td class='tcenter b'>Tên đơn vị</td> " +
                        "<td class='tcenter b'>Số ký hiệu văn bản</td><td class='tcenter b'>Ngày, tháng, năm văn bản</td><td class='tcenter b'>Nội dung trả lời đơn</td>" +
                        "<td class='tcenter b'>Số CV, ngày tháng năm Đoàn ĐBQH chuyển đơn</td>" +
                        "<td class='tcenter b'>Ghi chú</td></tr>";
            decimal count1 = 0;
            if (list1 != null)
            {
                foreach (var d in list1)
                {
                    count1++;
                    str += "<tr><td class='tcenter'>" + count1 + "</td><td>" + d.CTENCOQUAN2 + "</td><td class='tleft'>" + d.CSOVANBAN + "" +
                        "</td><td  class='tleft'>" + func.ConvertDateVN(d.DNGAYBANHANH.ToString()) + "</td><td  class='tleft'>" + d.CNOIDUNGVB + "</td>" +
                        "<td  class='tleft'>" + d.CSOVANBAN + " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString()) + " của " + d.CTENCOQUAN2 + "</td><td  class='tleft'>" + d.GHICHU_XULY + "</td> " +
                        "</tr>";
                }
            }

            str += "<tr><td colspan= '12' class='tcenter b'>DANH SÁCH CHUYỂN ĐƠN THÁNG "+ thang +" NĂM "+ nam +" CỦA ĐOÀN ĐBQH TỈNH<tr/></td><tr>" +
                        "<td class='tcenter b'>TT</td> <td class='tcenter b'>Ngày ban hành</td> <td class='tcenter b'>Số ký hiệu văn bản</td> " +
                        "<td class='tcenter b'>Công dân gửi đơn</td><td class='tcenter b'>Trích yếu</td><td class='tcenter b'>Nơi nhận VB chuyển của Đoàn ĐBQH tỉnh</td>" +
                        "<td class='tcenter b'>Ghi chú</td></tr>";

            decimal count2 = 0;
            if (list2 != null)
            {
                foreach (var d in list2)
                {
                    count2++;
                    str += "<tr><td class='tcenter'>" + count2 + "</td><td>" + func.ConvertDateVN(d.DNGAYBANHANH.ToString()) + "</td><td class='tleft'>" + d.CSOVANBAN + "" +
                        "</td><td  class='tleft'>" + d.CNGUOIGUI_TEN + ", "+ d.CNGUOIGUI_DIACHI + "</td><td  class='tleft'>" + d.CNOIDUNG + "</td>" +
                        "<td  class='tleft'>" + d.CTENCOQUAN3 + "</td><td  class='tleft'>" + d.CGHICHU + "</td> " +
                        "</tr>";
                }
            }

            str += "<tr><td colspan= '12' class='tcenter b'>DANH SÁCH CHUYỂN ĐƠN THÁNG " + thang + " NĂM " + nam + " CỦA THƯỜNG TRỰC HĐND TỈNH<tr/></td><tr>" +
                        "<td class='tcenter b'>TT</td> <td class='tcenter b'>Ngày ban hành</td> <td class='tcenter b'>Số ký hiệu văn bản</td> " +
                        "<td class='tcenter b'>Công dân gửi đơn</td><td class='tcenter b'>Trích yếu</td><td class='tcenter b'>Nơi nhận VB chuyển của Đoàn ĐBQH tỉnh</td>" +
                        "<td class='tcenter b'>Ghi chú</td></tr>";

            decimal count3 = 0;
            if (list3 != null)
            {
                foreach (var d in list3)
                {
                    count3++;
                    str += "<tr><td class='tcenter'>" + count3 + "</td><td>" + func.ConvertDateVN(d.DNGAYBANHANH.ToString()) + "</td><td class='tleft'>" + d.CSOVANBAN + "" +
                        "</td><td  class='tleft'>" + d.CNGUOIGUI_TEN + ", " + d.CNGUOIGUI_DIACHI + "</td><td  class='tleft'>" + d.CNOIDUNG + "</td>" +
                        "<td  class='tleft'>" + d.CTENCOQUAN3 + "</td><td  class='tleft'>" + d.CGHICHU + "</td> " +
                        "</tr>";
                }
            }

            return str;
        }

        public string Theodoigiaiquyetdon(DateTime tungay, DateTime denngay, int iKyHop, int iLoai)
        {
            string str = "";
            string TenKhoa = Get_TenKhoaHop_By_IDKyHop(iKyHop);
            var list = kntcrpt.getReportTheodoigiaiquyetdon("PKG_KNTC_BAOCAO.PRO_BAOCAO_THEODOIGQD", tungay, denngay, iKyHop, iLoai);
            list = list.ToList();
            str += "<table class='table table-condensed table-bordered'><tr><td colspan='12' class='tcenter b'>" +
                "TỔNG HỢP CÔNG VĂN CHUYỂN ĐƠN GỬI ĐẾN ĐOÀN ĐBQH TỈNH " + TenKhoa.ToUpper().Replace("KHóA", "KHÓA") + "</td></tr>" +
                        "<tr><td colspan='12' class='tcenter i'>(Từ ngày " + tungay.ToString("dd/MM/yyyy") + " đến ngày " + denngay.ToString("dd/MM/yyyy") + " )</td></tr>" +
                        "<tr>" +
                        "<td class='tcenter b'>TT</td> <td class='tcenter b'>Ngày ban hành</td> " +
                        "<td class='tcenter b'>Số ký hiệu văn bản</td><td class='tcenter b'>Nơi nhận văn bản</td><td class='tcenter b'>Công dân gửi đơn</td>" +
                        "<td class='tcenter b'>Trích yếu</td><td class='tcenter b'>Đã trả lời</td>" +
                        "<td class='tcenter b'>Ghi chú</td></tr>";
            decimal count = 0;
            if (list != null)
            {
                foreach (var d in list)
                {
                    count++;
                    str += "<tr><td class='tcenter'>" + count + "</td><td>" + func.ConvertDateVN(d.DNGAYBANHANH.ToString()) + "</td><td class='tleft'>" + d.CSOVANBAN + "" +
                        "</td><td  class='tleft'>" + d.CTENCOQUAN1 + "</td><td  class='tleft'>" + d.CNGUOIGUI_TEN + ", " + d.CNGUOIGUI_DIACHI + "</td>" +
                        "<td  class='tleft'>" + d.CNOIDUNG + "</td><td  class='tleft'>" + d.CSOVANBAN + " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString()) + " " + d.CTENCOQUAN2 + "</td><td  class='tleft'>" + d.GHICHU_XULY + "</td> " +
                        "</tr>";
                }
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Congvanchuyendon(DateTime tungay, DateTime denngay, int iloaidon, int iLoai)
        {
            string str = "";
            var list = kntcrpt.getReportBaoBaoCongvanchuyendon("PKG_KNTC_BAOCAO.PRO_BAOCAO_CVCHUYENDON", tungay, denngay, iloaidon, iLoai);
            int count_congvanchuyendon = 1;
            int count_congvandondoc = 1;
            int count_congvantraloi = 1;
            int count_donluu = 1;
            if (list != null)
            {
                var list_congvanchuyendon_chuacotraloi = list.Where(x => x.ICOQUANBANHANH == 4 && x.ITINHTRANGXULY == 3 && x.CLOAI == "chuyenxuly_noibo").ToList();
                var list_congvanchuyendon_dacotraloi = list.Where(x => x.ICOQUANBANHANH == 4 && x.ITINHTRANGXULY == 6).ToList();
                var list_congvandondoc = list.Where(x => x.ITINHTRANGXULY == 3).ToList();
                var list_congvandondoc_1 = list_congvandondoc.Where(x => x.CLOAI != "vanbandondocthuchien").ToList();
                var list_congvantraloi = list.Where(x => x.ITINHTRANGXULY == 6 && x.CLOAI == "huongdan_traloi").ToList();
                var list_donluu = list.Where(x => x.ITINHTRANGXULY == 5).ToList();
                str += "<table class='table table-condensed table-bordered'><tr><td colspan='12' class='tcenter b'>CÔNG VĂN CHUYỂN ĐƠN CỦA VĂN PHÒNG ĐOÀN ĐBQH VÀ HĐND TỈNH</td></tr>" +
                        "<tr>" +
                        "<td class='tcenter b' rowspan='2'>TT</td> <td class='tcenter b' rowspan='2'>Họ và tên, địa chỉ của công dân</td> " +
                        "<td class='tcenter b' rowspan='2'>Nội dung đơn</td><td class='tcenter b' colspan='3'>Phân loại đơn</td><td class='tcenter b' colspan='4'>Lĩnh vực</td>" +
                        "<td class='tcenter b' rowspan='2'>Số công văn, ngày tháng chuyển đơn đến cơ quan có thẩm quyền</td><td class='tcenter b' rowspan='2'>VB trả lời</td>" +
                        "<td class='tcenter b' rowspan='2'>Tổng số</td></tr>" +
                        "<tr><td class='tcenter b'>KN</td><td class='tcenter b'>TC</td><td class='tcenter b'>PAKN</td><td class='tcenter b'>Đất đai</td>" +
                        "<td class='tcenter b'>Chính sách XH</td><td class='tcenter b'>Tư pháp</td><td class='tcenter b'>Khác</td></tr>";
                foreach (var d in list_congvanchuyendon_dacotraloi)
                {
                    string KN = "";
                    string TC = "";
                    string PAKN = "";
                    string DatDai = "";
                    string ChinhSacXH = "";
                    string TuPhap = "";
                    string Khac = "";
                    int TongSo = 0;
                    var vanBanTraLoi = list.Where(x => x.ICOQUANBANHANH != 4 && x.IDON == d.IDON && x.CLOAI == "hoanthanh" && x.ITINHTRANGXULY == 6).ToList();
                    if (vanBanTraLoi.Count == 0)
                        continue;
                    if (d.ILOAIDON == 1)
                    {
                        KN = "1";
                        TongSo++;
                    }
                    else if (d.ILOAIDON == 2)
                    {
                        TC = "1";
                        TongSo++;
                    }
                    else if (d.ILOAIDON == 3)
                    {
                        PAKN = "1";
                        TongSo++;
                    }

                    if (d.ILINHVUC == 35) DatDai = "1";
                    else if (d.ILINHVUC == 4) ChinhSacXH = "1";
                    else if (d.ILINHVUC == 5) TuPhap = "1";
                    else if (d.ILINHVUC == 6) Khac = "1";
                    string tempTraLoi = "";
                    foreach (var item in vanBanTraLoi)
                    {
                        tempTraLoi += "Số " + item.CSOVANBAN;
                        if (item.DNGAYBANHANH != null)
                            tempTraLoi += " ngày " + func.ConvertDateVN(item.DNGAYBANHANH.ToString());
                        tempTraLoi += " từ " + item.CTEN;
                        if (item == vanBanTraLoi.Last())
                            tempTraLoi += ".";
                        else
                            tempTraLoi += ", ";
                    }
                    str += "<tr><td class='tcenter b'>" + count_congvanchuyendon + "</td><td class='tleft'>" + d.CNGUOIGUI_TEN + "</td><td class='tleft'>" + d.CNOIDUNG + "</td>" +
                        "<td  class='tcenter'>" + KN + "</td><td  class='tcenter'>" + TC + "</td><td  class='tcenter'>" + PAKN + "</td>" +
                        "<td  class='tcenter'>" + DatDai + "</td><td  class='tcenter'>" + ChinhSacXH + "</td><td  class='tcenter'>" + TuPhap + "</td><td  class='tcenter'>" + Khac + "</td>" +
                        "<td  class='tleft'>Số " + d.CSOVANBAN;
                    if (d.DNGAYBANHANH != null)
                        str += " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString());
                    str+= " chuyển " + d.CTEN + " </td>" +
                        "<td  class='tleft'>" + tempTraLoi + " </td>" +
                        "<td  class='tcenter'>" + TongSo + "</td>" +
                        "</tr>";
                    count_congvanchuyendon++;
                }
                
                str += "</table>";
                str += "<table style='margin-top:30px' class='table table-condensed table-bordered'>" +
                    "<tr><td colspan='12' class='tcenter b'>CÔNG VĂN ĐÔN ĐỐC CỦA VĂN PHÒNG HĐND TỈNH</td></tr>" +
                        "<tr>" +
                        "<td class='tcenter b' rowspan='2'>TT</td> <td class='tcenter b' rowspan='2'>Họ và tên, địa chỉ của công dân</td> " +
                        "<td class='tcenter b' rowspan='2'>Nội dung đơn</td><td class='tcenter b' colspan='3'>Phân loại đơn</td><td class='tcenter b' colspan='4'>Lĩnh vực</td>" +
                        "<td class='tcenter b' rowspan='2'>Số công văn đôn đốc, ngày tháng chuyển đơn đến cơ quan có thẩm quyền</td><td class='tcenter b' rowspan='2'>VB trả lời</td>" +
                        "<td class='tcenter b' rowspan='2'>Tổng số</td></tr>" +
                        "<tr><td class='tcenter b'>KN</td><td class='tcenter b'>TC</td><td class='tcenter b'>PAKN</td><td class='tcenter b'>Đất đai</td>" +
                        "<td class='tcenter b'>Chính sách XH</td><td class='tcenter b'>Tư pháp</td><td class='tcenter b'>Khác</td></tr>";
                foreach (var d in list_congvandondoc_1)
                {
                    string KN = "";
                    string TC = "";
                    string PAKN = "";
                    string DatDai = "";
                    string ChinhSacXH = "";
                    string TuPhap = "";
                    string Khac = "";
                    int TongSo = 0;
                    var vanBanTraLoi = list.Where(x => x.ITINHTRANGXULY == 3 && x.IDON == d.IDON && x.CLOAI == "vanbandondocthuchien").ToList();
                    if (vanBanTraLoi.Count == 0)// Khong phai la van ban don doc
                        continue;
                    if (d.ILOAIDON == 1)
                    {
                        KN = "1";
                        TongSo++;
                    }
                    else if (d.ILOAIDON == 2)
                    {
                        TC = "1";
                        TongSo++;
                    }
                    else if (d.ILOAIDON == 3)
                    {
                        PAKN = "1";
                        TongSo++;
                    }

                    if (d.ILINHVUC == 35) DatDai = "1";
                    else if (d.ILINHVUC == 4) ChinhSacXH = "1";
                    else if (d.ILINHVUC == 5) TuPhap = "1";
                    else if (d.ILINHVUC == 6) Khac = "1";
                    string tempTraLoi = "";
                    foreach (var item in vanBanTraLoi)
                    {
                        tempTraLoi += "Số " + item.CSOVANBAN;
                        if (item.DNGAYBANHANH != null)
                            tempTraLoi += " ngày " + func.ConvertDateVN(item.DNGAYBANHANH.ToString());
                        tempTraLoi += " từ " + item.CTEN;
                        if (item == vanBanTraLoi.Last())
                            tempTraLoi += ".";
                        else
                            tempTraLoi += ", ";
                    }
                    str += "<tr><td class='tcenter b'>" + count_congvandondoc + "</td><td class='tleft'>" + d.CNGUOIGUI_TEN + "</td><td class='tleft'>" + d.CNOIDUNG + "</td>" +
                        "<td  class='tcenter'>" + KN + "</td><td  class='tcenter'>" + TC + "</td><td  class='tcenter'>" + PAKN + "</td>" +
                        "<td  class='tcenter'>" + DatDai + "</td><td  class='tcenter'>" + ChinhSacXH + "</td><td  class='tcenter'>" + TuPhap + "</td><td  class='tcenter'>" + Khac + "</td>" +
                        "<td  class='tleft'>Số " + d.CSOVANBAN;
                    if (d.DNGAYBANHANH != null)
                        str += " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString());
                    str += " chuyển " + d.CTEN + " </td>" +
                        "<td  class='tleft'>" + tempTraLoi + " </td>" +
                        "<td  class='tcenter'>" + TongSo + "</td>" +
                        "</tr>";
                    count_congvandondoc++;
                }
                str += "</table>";
                str += "<table style='margin-top:30px' class='table table-condensed table-bordered'>" +
                    "<tr><td colspan='12' class='tcenter b'>CÔNG VĂN TRẢ LỜI, HƯỚNG DẪN CÔNG DÂN</td></tr>" +
                        "<tr>" +
                        "<td class='tcenter b' rowspan='2'>TT</td> <td class='tcenter b' rowspan='2'>Họ và tên, địa chỉ của công dân</td> " +
                        "<td class='tcenter b' rowspan='2'>Nội dung đơn</td><td class='tcenter b' colspan='3'>Phân loại đơn</td><td class='tcenter b' colspan='4'>Lĩnh vực</td>" +
                        "<td class='tcenter b' rowspan='2'>Số công văn, ngày tháng chuyển đơn đến cơ quan có thẩm quyền</td><td class='tcenter b' rowspan='2'>Ghi chú</td>" +
                        "<tr><td class='tcenter b'>KN</td><td class='tcenter b'>TC</td><td class='tcenter b'>PAKN</td><td class='tcenter b'>Đất đai</td>" +
                        "<td class='tcenter b'>Chính sách XH</td><td class='tcenter b'>Tư pháp</td><td class='tcenter b'>Khác</td></tr>";

                foreach (var d in list_congvantraloi)
                {
                    string KN = "";
                    string TC = "";
                    string PAKN = "";
                    string DatDai = "";
                    string ChinhSacXH = "";
                    string TuPhap = "";
                    string Khac = "";
                    int TongSo = 0;

                    if (d.ILOAIDON == 1)
                    {
                        KN = "1";
                        TongSo++;
                    }
                    else if (d.ILOAIDON == 2)
                    {
                        TC = "1";
                        TongSo++;
                    }
                    else if (d.ILOAIDON == 3)
                    {
                        PAKN = "1";
                        TongSo++;
                    }

                    if (d.ILINHVUC == 35) DatDai = "1";
                    else if (d.ILINHVUC == 4) ChinhSacXH = "1";
                    else if (d.ILINHVUC == 5) TuPhap = "1";
                    else if (d.ILINHVUC == 6) Khac = "1";

                    str += "<tr><td class='tcenter b'>" + count_congvantraloi + "</td><td class='tleft'>" + d.CNGUOIGUI_TEN + "</td><td class='tleft'>" + d.CNOIDUNG + "</td>" +
                        "<td  class='tcenter'>" + KN + "</td><td  class='tcenter'>" + TC + "</td><td  class='tcenter'>" + PAKN + "</td>" +
                        "<td  class='tcenter'>" + DatDai + "</td><td  class='tcenter'>" + ChinhSacXH + "</td><td  class='tcenter'>" + TuPhap + "</td><td  class='tcenter'>" + Khac + "</td>" +
                        "<td  class='tleft'>Số " + d.CSOVANBAN + " ngày " + func.ConvertDateVN(d.DNGAYBANHANH.ToString()) + " chuyển " + d.CTEN + "</td>" +
                        "<td  class='tleft'>" + d.GHICHU_XULY + "</td>" +
                        "</tr>";
                    count_congvantraloi++;
                }
                str += "</table>";
                str += "<table style='margin-top:30px' class='table table-condensed table-bordered'>" +
                    "<tr><td colspan='12' class='tcenter b'>ĐƠN LƯU</td></tr>" +
                        "<tr>" +
                        "<td class='tcenter b'>TT</td> <td class='tcenter b'>Họ tên công dân</td> " +
                        "<td class='tcenter b'>Ngày tháng nhận đơn</td><td class='tcenter b'>Nội dung đơn</td><td class='tcenter b'>Số đơn gửi</td>" +
                        "<td class='tcenter b'>Kiếu nại</td><td class='tcenter b'>Tố cáo</td><td class='tcenter b'>Phản ánh, KN</td>" +
                        "<td class='tcenter b'>Đất đai</td><td class='tcenter b'>Chính sách XH</td><td class='tcenter b'>Tư pháp</td>" +
                        "<td class='tcenter b'>Lĩnh vực khác</td><td class='tcenter b'>Ghi chú</td>"
                        ;
                foreach (var d in list_donluu)
                {
                    string KN = "";
                    string TC = "";
                    string PAKN = "";
                    string DatDai = "";
                    string ChinhSacXH = "";
                    string TuPhap = "";
                    string Khac = "";
                    int TongSo = 0;

                    if (d.ILOAIDON == 1)
                    {
                        KN = "1";
                        TongSo++;
                    }
                    else if (d.ILOAIDON == 2)
                    {
                        TC = "1";
                        TongSo++;
                    }
                    else if (d.ILOAIDON == 3)
                    {
                        PAKN = "1";
                        TongSo++;
                    }

                    if (d.ILINHVUC == 35)
                    {
                        DatDai = "1";
                        TongSo++;
                    }
                    else if (d.ILINHVUC == 4)
                    {
                        ChinhSacXH = "1";
                        TongSo++;
                    }
                    else if (d.ILINHVUC == 5)
                    {
                        TuPhap = "1";
                        TongSo++;
                    }
                    else if (d.ILINHVUC == 6)
                    {
                        Khac = "1";
                        TongSo++;
                    }

                    str += "<tr><td class='tcenter b'>" + count_donluu + "</td><td class='tleft'>" + d.CNGUOIGUI_TEN + "</td><td class='tleft'>" + func.ConvertDateVN(d.DNGAYNHAN.ToString()) + "</td>" +
                        "<td class='tleft'>" + d.CNOIDUNG + "</td>" +
                        "<td  class='tcenter'>" + TongSo + "</td><td  class='tcenter'>" + KN + "</td><td  class='tcenter'>" + TC + "</td><td  class='tcenter'>" + PAKN + "</td>" +
                        "<td  class='tcenter'>" + DatDai + "</td><td  class='tcenter'>" + ChinhSacXH + "</td><td  class='tcenter'>" + TuPhap + "</td><td  class='tcenter'>" + Khac + "</td>" +
                        "<td  class='tleft'>" + d.CLUUTHEODOI_LYDO + "</td>" +
                        "</tr>";
                    count_donluu++;
                }
                str += "</table>";

            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Thongkecoquanthamquyen(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";

            var list = kntcrpt.getReportBaoBaoThongKeCoquanthamquyen("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_TQGQ", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);
            int stt = 0;

            decimal Total = 0;
            decimal TotalTyle = 0;
            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    stt++;
                    Total += d.SOLUONG;
                    TotalTyle += d.TYLE;
                    str += "<tr><td class='tcenter'>" + d.STT + "</td><td>" + d.CTEN + "</td><td class='tright'>" + d.SOLUONG + "</td><td  class='tright'>" + Math.Round(d.TYLE, 2) + "</td></tr>";
                }
                str += "<tr><td class='b' colspan='2'>TỔNG CỘNG: </td><td class='tright'>" + Total + "</td><td class='tright'>" + Math.Round(TotalTyle, 2) + "</td></tr>";
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Thongkenoiguidon(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";

            var list = kntcrpt.getReportBaoBaoThongKeNoiGuiDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_NOIGUIDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);
            int stt = 0;

            decimal Total = 0;
            decimal TotalTyle = 0;
            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    stt++;
                    Total += d.SOLUONG;
                    TotalTyle += d.TYLE;
                    str += "<tr><td class='tcenter'>" + d.STT + "</td><td>" + d.CTEN + "</td><td class='tright'>" + d.SOLUONG + "</td><td  class='tright'>" + Math.Round(d.TYLE, 2) + "</td></tr>";
                }
                str += "<tr><td class='b' colspan='2'>TỔNG CỘNG: </td><td class='tright'>" + Total + "</td><td class='tright'>" + Math.Round(TotalTyle, 2) + "</td></tr>";
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Thongkenguoinhapdon(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";

            var list = kntcrpt.getReportBaoBaoThongKeNguoiNhapDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_NGUOINHAPDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);
            int stt = 0;

            decimal Total = 0;
            decimal TotalTyle = 0;
            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    stt++;
                    Total += d.SOLUONG;
                    TotalTyle += d.TYLE;
                    str += "<tr><td class='tcenter'>" + d.STT + "</td><td>" + d.CTEN + "</td><td class='tright'>" + d.SOLUONG + "</td><td  class='tright'>" + Math.Round(d.TYLE, 2) + "</td></tr>";
                }
                str += "<tr><td class='b' colspan='2'>TỔNG CỘNG: </td><td class='tright'>" + Total + "</td><td class='tright'>" + Math.Round(TotalTyle, 2) + "</td></tr>";
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Thongkenguoixuly(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";

            var list = kntcrpt.getReportBaoBaoThongKeNguoiXuLy("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_NGUOIXULY", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);
            int stt = 0;
            decimal Total = 0;
            decimal TotalTyle = 0;
            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    stt++;
                    Total += d.SOLUONG;
                    TotalTyle += d.TYLE;
                    str += "<tr><td class='tcenter'>" + d.STT + "</td><td>" + d.CTEN + "</td><td class='tright'>" + d.SOLUONG + "</td><td  class='tright'>" + Math.Round(d.TYLE, 2) + "</td></tr>";
                }
                str += "<tr><td class='b' colspan='2'>TỔNG CỘNG: </td><td class='tright'>" + Total + "</td><td class='tright'>" + Math.Round(TotalTyle, 2) + "</td></tr>";
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Thongkecoquanchuyendon(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";

            var list = kntcrpt.getReportBaoBaoThongKecoquanchuyendon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_COQUANCHUYENDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);
            int stt = 0;

            decimal Total = 0;
            decimal TotalTyle = 0;
            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    stt++;
                    Total += d.SOLUONG;
                    TotalTyle += d.TYLE;
                    str += "<tr><td class='tcenter'>" + d.STT + "</td><td>" + d.CTEN + "</td><td class='tright'>" + d.SOLUONG + "</td><td  class='tright'>" + d.TYLE + "</td></tr>";
                }
                str += "<tr><td class='b' colspan='2'>TỔNG CỘNG: </td><td class='tright'>" + Total + "</td><td class='tright'>" + Math.Round(TotalTyle, 2) + "</td></tr>";
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Thongketrungdon(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";

            var list = kntcrpt.getReportBaoBaoThongKeTrungDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_TRUNGDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);
            int stt = 0;

            decimal Total = 0;
            decimal TotalLan = 0;
            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    stt++;
                    Total += d.SODONTRUNG;
                    TotalLan += d.SOLANTRUNG;
                    str += "<tr><td class='tcenter'>" + stt + "</td><td>" + d.CTENDIADANH + "</td><td class='tright'>" + d.SODONTRUNG + "</td><td class=''>" + HttpContext.Current.Server.HtmlDecode(d.CNOIDUNGDON) + "</td><td  class='tright'>" + d.SOLANTRUNG + "</td></tr>";
                }
                str += "<tr><td class='b' colspan='2'>TỔNG CỘNG: </td><td class='tright'>" + Total + "</td><td class='tright'></td><td class='tright'>" + TotalLan + "</td></tr>";
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Thongketongsodon(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";
            var list = kntcrpt.getReportBaoBaoThongKeTongSoDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_TONGSODON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);
            decimal Total = 0;
            decimal TotalTrung = 0;
            decimal TotalTyle = 0;
            if (list != null)
            {
                if (list != null && list.Count() > 0)
                {
                    foreach (var d in list)
                    {
                        Total += d.SODONDUDIEUKIEN;
                        TotalTrung += d.SODONTRUG;

                        str += "<tr><td class='tcenter'>" + d.STT + "</td><td>" + d.CLOAIKHIEUTO + "</td><td class='tright'>" + d.SODONDUDIEUKIEN + "</td><td class='tright'>" + d.SODONTRUG + "</td><td  class='tright'>" + d.TYLE + "</td></tr>";
                    }
                    TotalTyle = (TotalTrung / Total) * 100;
                    str += "<tr><td class='b' colspan='2'>TỔNG CỘNG: </td><td class='tright'>" + Total + "</td><td class='tright'>" + TotalTrung + "</td><td class='tright'>" + Math.Round(TotalTyle, 2) + "</td></tr>";
                }
                else
                {
                    str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
                }
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Thongkechitietdon(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";

            var list = kntcrpt.getReportBaoBaoThongKeChiTietDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_CHIITETDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);

            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    int songuoi = (int)d.SONGUOI;
                    if (d.SONGUOI == 0) { songuoi = 1; }
                    str += "<tr><td class='tcenter'>" + d.STT + "</td><td class='tcenter'>" + func.ConvertDateVN(d.NGAYVAOSO.ToShortDateString()) + "</td><td >" + d.HOVATEN + "</td><td >" + d.DIACHI + "</td><td  class='tright'>" + songuoi + "</td><td  class='tright'>" + d.SOLAN + "</td><td >" + d.NOIDUNG + "</td><td >" + d.LOAIDON + "</td><td >" + d.CHUYENDEN + "</td></tr>";
                }
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Thongkediaban1(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";

            var list = kntcrpt.getReportBaoBaoThongKeDiaBan1("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_DIABAN1", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);

            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    str += "<tr><td class='tcenter'>" + d.STT + "</td><td>" + d.CTEN + "</td><td >" + d.CDIABAN + "</td><td  class='tright'>" + d.SOLUONG + "</td></tr>";
                }
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        public string Thongkediaban2(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan)
        {
            string str = "";

            var list = kntcrpt.getReportBaoBaoThongKeDiaBan2("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_DIABAN2", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);

            if (list != null && list.Count() > 0)
            {
                foreach (var d in list)
                {
                    str += "<tr><td class='tcenter'>" + d.STT + "</td><td>" + d.CTEN + "</td><td class='tright'>" + d.SOLUONG + "</td></tr>";
                }
            }
            else
            {
                str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }
            return str;
        }

        //public string Thongkesolieudon(string tungay, string denngay)
        //{
        //    string str = "";
        //    str += TieuDeBaoCao();
        //    var list = kntcrpt.getReportBaoBaoThongKeSoLieuDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_SOLIEUDON", tungay, denngay);
        //    str += "<tbody id=\"data\">";
        //    if (list != null && list.Count() > 0)
        //    {
        //        foreach (var d in list)
        //        {
        //            str += "<tr><td>" + d.CHUYENVIEN + "</td><td>" + d.TONGNHANDON + "</td><td class='tright'>" + d.HANHCHINH + "</td><td  class='tright'>" + d.TUPHAP + "</td><td class='tright'>" + d.KHAC + "</td><td  class='tright'>" + d.DDKTONGNHANDON + "</td><td  class='tright'>" + d.DDKTONGXULY + "</td><td  class='tright'>" + d.DDKSODON + "</td><td  class='tright'>" + d.DDKCHUYEN + "</td><td  class='tright'>" + d.DDKKHONGCHUYEN + "</td><td  class='tright'>" + d.DDKDANGNGHIENCUU + "</td><td  class='tright'>" + d.DDKDLOAISOBO + "</td><td  class='tright'>" + d.CHUAXULY + "</td><td  class='tright'>" + d.TONG + "</td><td  class='tright'>" + d.QHSODON + "</td><td  class='tright'>" + d.QHCHUYEN + "</td><td  class='tright'>" + d.QHDANGNGHIENCUU + "</td><td  class='tright'>" + d.QHKHONGCHUYEN + "</td><td  class='tright'>" + d.TBSODON + "</td><td  class='tright'>" + d.TBCHUYEN + "</td><td  class='tright'>" + d.TBDANGNGHIENCUU + "</td><td  class='tright'>" + d.TBKHONGCHUYEN + "</td><td  class='tright'>" + d.KSODON + "</td><td  class='tright'>" + d.KCHUYEN + "</td><td  class='tright'>" + d.KDANGNGHIENCUU + "</td><td  class='tright'>" + d.KKHONGCHUYEN + "</td><td  class='tright'>" + d.TONGXULY + "</td><td  class='tright'>" + d.TONGCHUYEN + "</td><td  class='tright'>" + d.TONGDANGNGHIENCUU + "</td><td  class='tright'>" + d.TONGLUU + "</td></tr>";
        //        }
        //        //str += "<tr><td class='b' colspan='2'>TỔNG CỘNG: </td><td class='tright'>" + Total + "</td><td class='tright'>" + Math.Round(TotalTyle, 2) + "</td></tr>";
        //    }
        //    else
        //    {
        //        str += "<tr><td colspan='4' class='tcenter'>Chưa có dữ liệu</td></tr>";
        //    }
        //    str += "</tbody>";
        //    return str;

        //}
        //public string KNTC_LIST_DON()
        //{
        //    var don = kntcrpt.getReportDonKhieuTo("PKG_KNTC_BAOCAO.PRO_LIST_ALL_DON");
        //    var linhvuc = kntcrpt.getReportNhomLinhVuc("PKG_KNTC_BAOCAO.PRO_LIST_NHOMLINHVUC");
        //    var nguondon = kntcrpt.getReportNguonDon("PKG_KNTC_BAOCAO.PRO_LIST_NGUONDON");
        //    string str = "";
        //    //List<decimal> list_id_linhvuc = new List<decimal>();
        //    //List<decimal> list_id_parent_nguondon = new List<decimal>();
        //    List<decimal> list_id_chuyenvien = new List<decimal>();
        //    foreach (var l in don)
        //    {
        //        //if (!list_id_linhvuc.Contains(l.ID_PARENT_LINHVUC)) { list_id_linhvuc.Add(l.ID_PARENT_LINHVUC); }
        //        //if (!list_id_parent_nguondon.Contains((decimal)l.ID_PARENT_NGUONDON)) { list_id_parent_nguondon.Add((decimal)l.ID_PARENT_NGUONDON); }
        //        if (!list_id_chuyenvien.Contains((decimal)l.IUSER_DUOCGIAOXULY)) { list_id_chuyenvien.Add((decimal)l.IUSER_DUOCGIAOXULY); }
        //    }

        //    str += "<tr><th>Tên chuyên viên</th>";
        //    foreach (var l in linhvuc)
        //    {
        //        str += "<th colspan = " + linhvuc.Count() + ">" + l.CTEN + "</th>";
        //    }
        //    foreach (var n in nguondon)
        //    {
        //        int nhomnguondon = (int)kntc.Get_NguonDon(Convert.ToInt32(n.INGUONDON)).IPARENT;
        //        string tennhom = kntc.Get_NguonDon(nhomnguondon).CTEN;
        //        str += "<th>" + tennhom + "</th>";
        //    }

        //    str += "</tr>";
        //    foreach (var c in list_id_chuyenvien)
        //    {
        //        USERS u = kntc.Get_User(Convert.ToInt32(c));
        //        str += "<tr><td>" + u.CTEN + "</td>";
        //        str += "<td>" + don.Where(x => x.IUSER_DUOCGIAOXULY == c).Count() + "</td>";
        //        foreach (var l in linhvuc)
        //        {
        //            str += "<td>" + don.Where(x => x.ILINHVUC == l.ILINHVUC && x.IUSER_DUOCGIAOXULY == c).Count() + "</td>";
        //        }
        //        str += "</tr>";
        //    }
        //    return str;

        //}
        public string TieuDeBaoCao(string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc, int idonvi, int inguondon, int itiepnhan) // Get tiêu đề báo cáo
        {
            var linhvuc = kntcrpt.getReportNhomLinhVuc("PKG_KNTC_BAOCAO.PRO_LIST_NHOMLINHVUC");
            if (linhvuc == null)
            {
                linhvuc = new List<LINHVUC>();
            }
            var nguondon = kntcrpt.getReportNguonDon("PKG_KNTC_BAOCAO.PRO_LIST_NGUONDON");
            if (nguondon == null)
            {
                nguondon = new List<KNTC_NGUONDON>();
            }
            var don = kntcrpt.getReportDonKhieuTo("PKG_KNTC_BAOCAO.PRO_LIST_ALL_DON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc, idonvi, inguondon, itiepnhan);
            if (don == null)
            {
                don = new List<DONKHIEUTO>();
            }
            List<decimal> list_id_chuyenvien = new List<decimal>();
            foreach (var l in don)
            {
                if (!list_id_chuyenvien.Contains((decimal)l.IUSER)) { if (l.IUSER != 0) { list_id_chuyenvien.Add((decimal)l.IUSER); } }
            }

            string str = "<thead><tr><th style=\"text-align: center\" rowspan=\"5\">Chuyên viên</th>"
            + " <th style=\"text-align: center\" rowspan=\"5\">Tổng đơn nhận</th>"
            + " <th style=\"text-align: center\" colspan=\"" + linhvuc.Count + "\">Lĩnh vực</th>";
            foreach (var n in nguondon.Where(v => v.IPARENT == 0))
            {
                if (nguondon.Where(v => v.IPARENT == n.INGUONDON).Count() > 0)
                {
                    int count = 1 + nguondon.Where(v => v.IPARENT == n.INGUONDON).Count() * 4;
                    str += "<th style=\"text-align: center\" colspan=\"" + count + "\">" + n.CTEN.ToUpper() + "</th>";
                }
                else
                {
                    str += "<th style=\"text-align: center\" colspan=\"8\">" + n.CTEN.ToUpper() + "</th>";
                }
            }
            str += " <th style=\"text-align: center\" rowspan=\"5\">Tổng xử lý</th>"
            + " <th style=\"text-align: center\" rowspan=\"5\">Tổng chuyển</th>"
            + " <th style=\"text-align: center\" rowspan=\"5\">Tổng đang nghiên cứu</th>"
            + " <th style=\"text-align: center\" rowspan=\"5\">Tổng lưu</th>"
            + " </tr><tr>";
            foreach (var l in linhvuc)
            {
                str += "<th style=\"text-align: center\" rowspan=\"4\">" + l.CTEN + "</th>";
            }
            foreach (var n in nguondon.Where(v => v.IPARENT == 0))
            {
                if (nguondon.Where(v => v.IPARENT == n.INGUONDON).Count() <= 0)
                {
                    str += "<th style=\"text-align: center\" rowspan=\"4\">Tổng đơn nhận</th>"
                                         + " <th style=\"text-align: center\" colspan=\"6\">Đã xử lý</th>"
                                          + "<th style=\"text-align: center\" rowspan=\"4\">Chưa xử lý</th>";
                }
                else
                {
                    str += "<th style=\"text-align: center\" rowspan=\"4\">Tổng</th>";
                    foreach (var d in nguondon.Where(v => v.IPARENT == n.INGUONDON))
                    {
                        str += "<th style=\"text-align: center\" colspan=\"4\">" + d.CTEN + "</th>";
                    }
                }
            }
            str += "</tr><tr>";
            foreach (var n in nguondon.Where(v => v.IPARENT == 0))
            {
                if (nguondon.Where(v => v.IPARENT == n.INGUONDON).Count() <= 0)
                {
                    str += "<th style=\"text-align: center\" rowspan=\"3\">Tổng xử lý</th>"
                                           + " <th style=\"text-align: center\" colspan=\"4\">Đủ ĐK</th>"
                                           + " <th style=\"text-align: center\" rowspan=\"3\">Loại sơ bộ</th>";
                }
                else
                {
                    foreach (var d in nguondon.Where(v => v.IPARENT == n.INGUONDON))
                    {
                        str += " <th style=\"text-align: center\" rowspan=\"3\">Số đơn</th>"
                                            + " <th style=\"text-align: center\" rowspan=\"3\">Chuyển</th>"
                                            + " <th style=\"text-align: center\" rowspan=\"3\">Đang nghiên cứu</th>"
                                            + " <th style=\"text-align: center\" rowspan=\"3\">Không chuyển</th>";
                    }
                }
            }
            str += "</tr><tr>";
            foreach (var n in nguondon.Where(v => v.IPARENT == 0))
            {
                if (nguondon.Where(v => v.IPARENT == n.INGUONDON).Count() <= 0)
                {
                    str += "<th style=\"text-align: center\" rowspan=\"2\">Số đơn</th>"
                                           + " <th style=\"text-align: center\" rowspan=\"2\">Chuyển</th>"
                                           + " <th style=\"text-align: center\" rowspan=\"2\">Không chuyển</th>"
                                           + " <th style=\"text-align: center\" rowspan=\"2\">Đang nghiên cứu</th>";
                }
            }
            str += "</tr><tr><tr></tr></thead>";
            str += "<tbody id=\"data\">";
            foreach (var c in list_id_chuyenvien)
            {
                USERS u = kntc.Get_User(Convert.ToInt32(c));
                str += "<tr><td>" + u.CTEN + "</td>";
                str += "<td class='tright'>" + don.Where(x => x.IUSER == c).Count() + "</td>";
                foreach (var l in linhvuc)
                {

                    str += "<td class='tright'>" + don.Where(x => x.ID_PARENT_LINHVUC == l.ILINHVUC && x.IUSER == c).Count() + "</td>";
                }
                foreach (var n in nguondon.Where(v => v.IPARENT == 0))
                {
                    if (nguondon.Where(v => v.IPARENT == n.INGUONDON).Count() <= 0)
                    {
                        str += "<td class='tright'>" + don.Where(x => x.IUSER == c && x.ID_PARENT_NGUONDON == n.INGUONDON).Count() + "</td>"
                                               + " <td class='tright' >" + don.Where(x => x.IUSER == c && x.ID_PARENT_NGUONDON == n.INGUONDON && x.ITINHTRANGXULY == 6).Count() + "</td>"
                                               + " <td class='tright'>" + don.Where(x => x.IUSER == c && x.ID_PARENT_NGUONDON == n.INGUONDON && x.ITINHTRANGXULY == 6 && x.IDUDIEUKIEN == 1).Count() + "</td>"
                                               + " <td class='tright' >" + don.Where(x => x.IUSER == c && x.ID_PARENT_NGUONDON == n.INGUONDON && x.IDUDIEUKIEN == 1 && x.IDUDIEUKIEN_KETQUA == 1).Count() + "</td>"
                                               + " <td class='tright' >" + don.Where(x => x.IUSER == c && x.ID_PARENT_NGUONDON == n.INGUONDON && x.IDUDIEUKIEN == 1 && x.IDUDIEUKIEN_KETQUA == 0).Count() + "</td>"
                                               + " <td class='tright' >" + don.Where(x => x.IUSER == c && x.ID_PARENT_NGUONDON == n.INGUONDON && x.IDUDIEUKIEN == 1 && x.IDUDIEUKIEN_KETQUA == 2).Count() + "</td>"
                                               + " <td class='tright'>" + don.Where(x => x.IUSER == c && x.ID_PARENT_NGUONDON == n.INGUONDON && x.ITINHTRANGXULY == 2 && x.IDUDIEUKIEN != 1).Count() + "</td>"
                                               + " <td class='tright' >" + don.Where(x => x.IUSER == c && x.ID_PARENT_NGUONDON == n.INGUONDON && x.ITINHTRANGXULY == 2).Count() + "</td>";
                    }
                    else
                    {
                        str += "<td class='tright'>" + don.Where(x => x.IUSER == c && x.ID_PARENT_NGUONDON == n.INGUONDON).Count() + "</td>";
                        foreach (var d in nguondon.Where(v => v.IPARENT == n.INGUONDON))
                        {
                            str += " <td class='tright'>" + don.Where(x => x.IUSER == c && x.INGUONDON == d.INGUONDON).Count() + "</td>"
                                                + " <td class='tright'>" + don.Where(x => x.IUSER == c && x.INGUONDON == d.INGUONDON && x.IDUDIEUKIEN == 1 && x.IDUDIEUKIEN_KETQUA == 1).Count() + "</td>"
                                                + " <td class='tright'>" + don.Where(x => x.IUSER == c && x.INGUONDON == d.INGUONDON && x.IDUDIEUKIEN == 1 && x.IDUDIEUKIEN_KETQUA == 0).Count() + "</td>"
                                                + " <td class='tright'>" + don.Where(x => x.IUSER == c && x.INGUONDON == d.INGUONDON && x.IDUDIEUKIEN == 1 && x.IDUDIEUKIEN_KETQUA == 2).Count() + "</td>";
                        }
                    }
                }
                str += "<td class='tright'>" + don.Where(x => x.IUSER == c && x.ITINHTRANGXULY == 6).Count() + "</td>";
                str += "<td class='tright'>" + don.Where(x => x.IUSER == c && x.IDUDIEUKIEN == 1 && x.IDUDIEUKIEN_KETQUA == 1).Count() + "</td>";
                str += "<td class='tright'>" + don.Where(x => x.IUSER == c && x.IDUDIEUKIEN == 1 && x.IDUDIEUKIEN_KETQUA == 0).Count() + "</td>";
                str += "<td class='tright'>" + don.Where(x => x.IUSER == c && x.IDUDIEUKIEN == 1 && x.IDUDIEUKIEN_KETQUA == 2).Count() + "</td>";
                str += "</tr>";
            }
            str += "</tbody>";
            return str;
        }

        //public string SoLieuDonByLinhVuc(int iLinhVuc, int iUser, string tungay, string denngay, int iloaidon, int itinhchat, int inoidung, int ilinhvuc)
        //{
        //    string str = "";
        //    var list = kntcrpt.getReportBaoBaoThongKeSoLieuDon("PKG_KNTC_BAOCAO.PRO_BAOCAO_TK_SOLIEUDON", tungay, denngay, iloaidon, itinhchat, inoidung, ilinhvuc);

        //    return str;
        //}
        public string Dontamxoa(List<DONTRACUU> don, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            var list = don;
            foreach (var d in list)
            {
                KNTC_DON objdon = kntc.GetDON((int)d.IDON);
                if (objdon != null)
                {
                    Dictionary<string, object> _condition = new Dictionary<string, object>();
                    _condition.Add("IDONTRUNG", d.IDON);
                    int sodontrung = (int)d.ISOLUONGTRUNG + kntc.List_Don(_condition).ToList().Count;
                    string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                    string trangthaixuly = GetTinhTrangDon(d.TINHTRANG);
                    string diachi = "";
                    if (HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) != null) diachi += HttpUtility.HtmlEncode(d.CNGUOIGUI_DIACHI) + " ,";
                    if (HttpUtility.HtmlEncode(d.CTENHUYEN) != null) diachi += HttpUtility.HtmlEncode(d.CTENHUYEN) + " ,";
                    if (HttpUtility.HtmlEncode(d.CTINHTHANH) != null) diachi += HttpUtility.HtmlEncode(d.CTINHTHANH) + " .";
                    string noidungdon = "";
                    if (HttpUtility.HtmlEncode(d.CNOIDUNG) != null)
                    {
                        if (HttpUtility.HtmlEncode(d.CNOIDUNG).Trim() != "")
                        {
                            noidungdon = func.TomTatNoiDung(HttpUtility.HtmlEncode(d.CNOIDUNG), id_encr);
                        }
                    }
                    string khoiphuc = "<a href=\"javascript:void(0)\" data-original-title='Xóa' onclick=\"DeletePage_Confirm_TraCuu('" + id_encr + "','id=" + id_encr + "','/Kntc/Ajax_Kntc_back/','Bạn muốn khôi phục đơn khiếu nại này','khôi phục')\" rel='tooltip' title='Khôi phục' class='trans_func'><i class=' icon-signout'></i></a> ";
                    string del = "<a href=\"javascript:void(0)\" data-original-title='Xóa' onclick=\"DeletePage_Confirm_TraCuu('" + id_encr + "','id=" + id_encr + "','/Kntc/Ajax_Don_del/','Bạn muốn xóa đơn khiếu nại này','xóa')\" rel='tooltip' title='' class='trans_func'><i class='icon-trash'></i></a> ";
                    string chitiet = "<a class='f-bule' href='/Kntc/Don_info/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + d.STT + "</td><td><p><strong>" + HttpUtility.HtmlEncode(d.CNGUOIGUI_TEN) +
                       "</strong></p>" + diachi + "</td><td>" + noidungdon + "<p>Trạng thái: " + trangthaixuly + "</p></td><td class='tcenter'>" + del + khoiphuc + chitiet + "</td></tr>";
                }
            }
            return str;
        }

        /* Ham tao string hien thi HTML de xem truoc bao cao thang
         */
        public string Xemtruoc_baocaothang(string tungay, string denngay)
        {
            string str = "";
            DateTime temp = Convert.ToDateTime(tungay);
            string nam = temp.Year.ToString();
            string thang = temp.Month.ToString();
            var ListBaoCaoThang = kntcrpt.getReportBaoCaoThang("PKG_KNTC_BAOCAO.PRO_BAOCAO_THANG", tungay, denngay);
            List<BAOCAOTHANG> List1 = ListBaoCaoThang.Where(x => x.IDOITUONGGUI == 0).ToList();
            List<BAOCAOTHANG> List2 = ListBaoCaoThang.Where(x => x.IDOITUONGGUI == 1).ToList();
            // Phan Header
            str += "<table class='table table-condensed table-bordered'>" +
                        "<tr>" +
                        "<td class='tcenter b'>VĂN PHÒNG ĐOÀN ĐBQH VÀ HĐND TỈNH THANH HÓA</td>" +
                        "<td class='tcenter b'>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tcenter i'>Số:...../BC-VP</td>" +
                        "<td class='tcenter b'>Độc lập - Tự do - Hạnh phúc</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tcenter i'></td>" +
                        "<td class='tright '>Thanh Hóa, ngày...tháng...năm 2022</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tcenter b' colspan = '2'>BÁO CÁO</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tcenter b' colspan = '2'>" + "Kết quả tiếp công dân, tiếp nhận, xử lý đơn khiếu nại, tố cáo của công dân tháng "
                        + thang + " năm " + nam + " của Đoàn đại biểu Quốc hội và Thường trực HĐND Tỉnh Thanh Hóa" +
                        "</td>" +
                        "<tr>" +
                        "<td class='tleft b' colspan = '2'>1. Tình hình tiếp công dân tại trụ sở tiếp công dân tỉnh</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tleft b' colspan = '2'>1.1. Đoàn đại biểu Quốc hội tỉnh</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tleft b' colspan = '2'>1.2. Thường trực Hội đồng nhân dân tỉnh </td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tleft b' colspan = '2'>2. Về kết quả tiếp nhận, phân loại, xử lý đơn, theo dõi việc giải quyết đơn khiếu nại, tố cáo, phản ánh, kiến nghị của công dân</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tleft b' colspan = '2'>2.1. Đoàn đại biểu Quốc hội tỉnh</td>" +
                        "</tr>" +
                        "</tr></table>";
            // Ket thuc Header
            // Bang 2.1
            str += "<table class='table table-condensed table-bordered'>" +
                        "<tr>" +
                        "<td class='tcenter b' rowspan='2'>STT</td> " +
                        "<td class='tcenter b' colspan='3'>Số lượng đơn tiếp nhận</td>" +
                        "<td class='tcenter b' rowspan = '2'>Số đơn đã chuyển</td>" +
                        "<td class='tcenter b' rowspan = '2'>Số đơn lưu</td>" +
                        "<td class='tcenter b' rowspan = '2'>Số đơn được giải quyết</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tcenter b'>Qua bưu điện </td>" +
                        "<td class='tcenter b'>Qua trụ sở tiếp dân</td>" +
                        "<td class='tcenter b'>Khác</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tcenter'></td>" +
                        "<td class='tcenter'>" + List1.Where(x => x.INGUONDON == 9).Count() + "</td>" +
                        "<td class='tcenter'>" + List1.Where(x => x.INGUONDON == 25).Count() + "</td>" +
                        "<td class='tcenter'>" + List1.Where(x => x.INGUONDON == 7).Count() + "</td>" +
                        "<td class='tcenter'>" + List1.Where(x => x.ITINHTRANGXULY == 3).Count() + "</td>" +
                        "<td class='tcenter'>" + List1.Where(x => x.ITINHTRANGXULY == 5).Count() + "</td>" +
                        "<td class='tcenter'>" + List1.Where(x => x.ITINHTRANGXULY == 6).Count() + "</td>" +
                        "</tr>";
            str += "</table>";
            // Bang 2.2
            str += "<table class='table table-condensed table-bordered'>" +
                        "<tr class = 'tleft b'><td class='tleft b' colspan = '6'>2.2. Thường trực Hội đồng nhân dân tỉnh" +
                        "</td></tr>" +
                        "<tr>" +
                        "<td class='tcenter b' rowspan='2' style ='width:5%'>STT</td> " +
                        "<td class='tcenter b' colspan='3'>Số lượng đơn tiếp nhận</td>" +
                        "<td class='tcenter b' rowspan = '2'>Số đơn đã chuyển</td>" +
                        "<td class='tcenter b' rowspan = '2'>Số đơn đã hướng dẫn</td>" +
                        "<td class='tcenter b' rowspan = '2'>Số đơn lưu</td>" +
                        "<td class='tcenter b' rowspan = '2'>Số đơn được giải quyết</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tcenter b'>Qua bưu điện </td>" +
                        "<td class='tcenter b'>Qua trụ sở tiếp dân</td>" +
                        "<td class='tcenter b'>Khác</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td class='tcenter'></td>" +
                        "<td class='tcenter'>" + List2.Where(x => x.INGUONDON == 9).Count() + "</td>" +
                        "<td class='tcenter'>" + List2.Where(x => x.INGUONDON == 25).Count() + "</td>" +
                        "<td class='tcenter'>" + List2.Where(x => x.INGUONDON == 7).Count() + "</td>" +
                        "<td class='tcenter'>" + List2.Where(x => x.ITINHTRANGXULY == 3).Count() + "</td>" +
                        "<td class='tcenter'></td>" +
                        "<td class='tcenter'>" + List2.Where(x => x.ITINHTRANGXULY == 5).Count() + "</td>" +
                        "<td class='tcenter'>" + List2.Where(x => x.ITINHTRANGXULY == 6).Count() + "</td>" +
                        "</tr>";
            str += "</table>";
            //Bang 3
            List<BAOCAOTHANG> list3 = ListBaoCaoThang.Where(x => x.ITINHTRANGXULY == 6).ToList();
            List<BAOCAOTHANG> list3_1 = list3.Where(x => x.ICOQUANBANHANH != 4).ToList();
            str += "<table class='table table-condensed table-bordered'>" +
                        "<tr class = 'tleft b'><td class='tleft b' colspan = '7'>3. Các vụ việc nổi cộm, phức tạp" +
                        "</td></tr>" +
                        "<tr class = 'tcenter b'><td class='tcenter b' colspan = '12'>DANH SÁCH CÁC ĐƠN VỊ ĐÃ TRẢ LỜI ĐƠN CHUYỂN CỦA ĐOÀN ĐBQH TỈNH VÀ THƯỜNG TRỰC HĐND TỈNH" +
                        "</td></tr>" +
                        "<tr>" +
                        "<td class='tcenter b' style ='width:5%'>TT</td> " +
                        "<td class='tcenter b'>Tên đơn vị</td> " +
                        "<td class='tcenter b'>Số ký hiệu văn bản</td> " +
                        "<td class='tcenter b'>Ngày, tháng, năm văn bản</td> " +
                        "<td class='tcenter b'>Nội dung trả lời đơn</td> " +
                        "<td class='tcenter b'>Số CV, ngày tháng năm Đoàn ĐBQH chuyển đơn</td> " +
                        "<td class='tcenter b'>Ghi chú </td> " +
                        "</tr>";
            int stt = 1;
            foreach (var item in list3_1)
            {
                var tempItem = list3.FirstOrDefault(x => x.ICOQUANBANHANH == 4 && x.IDON_VANBAN == item.IDON_VANBAN);
                if (tempItem == null)
                    continue;
                String ngaybanhanh = "";
                if (tempItem.DNGAYBANHANH != null)
                    ngaybanhanh = String.Format("{0:dd/MM/yyyy}", tempItem.DNGAYBANHANH);
                str += "<tr>" +
                    "<td class='tcenter' style ='width:5%'>" + stt + "</td>" +
                    "<td class='tcenter'>" + item.COQUANBANHANH +
                    "<td class='tcenter'>" + item.CSOVANBAN + "</td>" +
                    "<td class='tcenter'>" + String.Format("{0:dd/MM/yyyy}", item.DNGAYBANHANH) + "</td>" +
                    "<td class='tcenter'>" + item.NOIDUNGVANBAN + "</td>" +
                    "<td class='tcenter'>" + "Số " + tempItem.CSOVANBAN + " ngày " + ngaybanhanh + " của " + tempItem.COQUANBANHANH + "</td>" +
                    "<td class='tcenter'>" + item.GHICHUVANBAN + "</td>" +
                    "</tr>";
                stt++;
            }
            str += "</table>";
            // Bang 4
            List<BAOCAOTHANG> list4 = ListBaoCaoThang.Where(x => x.ITINHTRANGXULY == 3 && x.IDOITUONGGUI == 0 && x.ICOQUANBANHANH == 4).ToList();
            str += "</table>";
            str += "<table class='table table-condensed table-bordered'>" +
                        "<tr class = 'tleft b'><td class='tcenter b' colspan = '7'>DANH SÁCH CHUYỂN ĐƠN THÁNG " + thang +" NĂM " + nam + " CỦA ĐOÀN ĐBQH TỈNH" +
                        "</td></tr>" +
                        "<tr>" +
                        "<td class='tcenter b' style ='width:5%'>TT</td> " +
                        "<td class='tcenter b'>Tên đơn vị</td> " +
                        "<td class='tcenter b'>Số ký hiệu văn bản</td> " +
                        "<td class='tcenter b'>Công dân gửi đơn</td> " +
                        "<td class='tcenter b'>Trích yếu</td> " +
                        "<td class='tcenter b'>Nơi nhận VB chuyển của Đoàn ĐBQH tỉnh</td> " +
                        "<td class='tcenter b'>Ghi chú </td> " +
                        "</tr>";
            stt = 1;
            foreach (var item in list4)
            {

                String ngaybanhanh = "";
                if (item.DNGAYBANHANH != null)
                    ngaybanhanh = String.Format("{0:dd/MM/yyyy}", item.DNGAYBANHANH);
                str += "<tr>" +
                    "<td class='tcenter' style ='width:5%'>" + stt + "</td>" +
                    "<td class='tcenter'>" + ngaybanhanh +
                    "<td class='tcenter'>" + item.CSOVANBAN + "</td>" +
                    "<td class='tcenter'>" + item.CNGUOIGUI_TEN + ", " + item.DIACHI + "." + "</td>" +
                    "<td class='tcenter'>" + item.NOIDUNGDON + "</td>" +
                    "<td class='tcenter'>" + item.CDONVIXULY + "</td>" +
                    "<td class='tcenter'>" + item.GHICHUDON + "</td>" +
                    "</tr>";
                stt++;
            }
            str += "</table>";
            // Bang 5
            List<BAOCAOTHANG> list5 = ListBaoCaoThang.Where(x => x.ITINHTRANGXULY == 3 && x.IDOITUONGGUI == 1 && x.ICOQUANBANHANH == 4).ToList();
            str += "</table>";
            str += "<table class='table table-condensed table-bordered'>" +
                        "<tr class = 'tleft b'><td class='tcenter b' colspan = '7'>DANH SÁCH CHUYỂN ĐƠN THÁNG " + thang + " NĂM " + nam + " CỦA THƯỜNG TRỰC HĐND TỈNH" +
                        "</td></tr>" +
                        "<tr>" +
                        "<td class='tcenter b' style ='width:5%'>TT</td> " +
                        "<td class='tcenter b'>Tên đơn vị</td> " +
                        "<td class='tcenter b'>Số ký hiệu văn bản</td> " +
                        "<td class='tcenter b'>Công dân gửi đơn</td> " +
                        "<td class='tcenter b'>Trích yếu</td> " +
                        "<td class='tcenter b'>Nơi nhận VB chuyển của Đoàn ĐBQH tỉnh</td> " +
                        "<td class='tcenter b'>Ghi chú </td> " +
                        "</tr>";
            stt = 1;
            foreach (var item in list5)
            {

                String ngaybanhanh = "";
                if (item.DNGAYBANHANH != null)
                    ngaybanhanh = String.Format("{0:dd/MM/yyyy}", item.DNGAYBANHANH);
                str += "<tr>" +
                    "<td class='tcenter' style ='width:5%'>" + stt + "</td>" +
                    "<td class='tcenter'>" + ngaybanhanh +
                    "<td class='tcenter'>" + item.CSOVANBAN + "</td>" +
                    "<td class='tcenter'>" + item.CNGUOIGUI_TEN + ", " + item.DIACHI + "." + "</td>" +
                    "<td class='tcenter'>" + item.NOIDUNGDON + "</td>" +
                    "<td class='tcenter'>" + item.CDONVIXULY + "</td>" +
                    "<td class='tcenter'>" + item.GHICHUDON + "</td>" +
                    "</tr>";
                stt++;
            }
            str += "</table>";
            return str;

        }

        public string List_Import_Kntc_Don(List<PRC_KNTC_IMPORT_LISTDON> import, string url_key)
        {
            string str = "";
            int count = 1;

            foreach (var d in import)
            {
                string id_encr = HashUtil.Encode_ID(d.IDON.ToString(), url_key);
                string detail = "<a onclick=\"ShowPageLoading()\" href='/Kntc/Don_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
                string del = " <a href=\"javascript:void()\" title='Xóa đơn đã import' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kntc/Ajax_Don_del_import','Bạn có muốn xóa đơn này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                str += "<tr ><td width = '3%' class='tcenter'>" + count + "</td><td class='tcenter' >" + d.CNGUOIGUI_TEN + "</td><td class='tcenter' >" + d.TEN_DIAPHUONG1 + "</td><td class='tcenter' >" + d.TEN_DIAPHUONG2 + "</td><td class='tcenter' >" + d.CNGUOIGUI_DIACHI + "</td><td>" + d.CNOIDUNG + "</td> <td class='tcenter' >" + d.TEN_LOAIDON + " </td>  <td width = '10%' class='tcenter' >"+ detail+ del+ "</td> </tr>"; ;
                count++;

            }
            return str;
        }
    }
    
}