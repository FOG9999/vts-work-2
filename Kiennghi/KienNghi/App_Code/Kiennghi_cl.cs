using Entities.Models;
using Entities.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Enums;
using DataAccess.Dao;
using DataAccess.Busineess;
using System.Data;
using System.Web;
using DocumentFormat.OpenXml.Math;

namespace KienNghi.App_Code
{
    public class Kiennghi_cl
    {
        Funtions func = new Funtions();
        KienNghiReport kn_report = new KienNghiReport();
        Base _base = new Base();
        //KN_KiennghiRepository _kiennghi = new KN_KiennghiRepository();
        //KN_ChuongtrinhRepository _kiennghi_chuongtrinh = new KN_ChuongtrinhRepository();
        //Quochoi_CoquanRepository _quochoi_coquan = new Quochoi_CoquanRepository();
        //KN_Chuongtrinh_diaphuongRepository _chuongtrinh_diaphuong = new KN_Chuongtrinh_diaphuongRepository();
        //DiaPhuongRepository _diaphuong = new DiaPhuongRepository();
        //KN_Chuongtrinh_DaibieuRepository _chuongtrinh_daibieu = new KN_Chuongtrinh_DaibieuRepository();
        //UsserRepository _user = new UsserRepository();
        //LinhvucRepository _linhvuc = new LinhvucRepository();
        //Quochoi_KyhopRepository _quochoi_kyhop = new Quochoi_KyhopRepository();
        //Quochoi_KhoaRepository _quochoi_khoa = new Quochoi_KhoaRepository();
        //KN_ChuongtrinhRepository _kn_chuongtrinh = new KN_ChuongtrinhRepository();
        //KN_Kiennghi_TraloiRepository _kn_traloi = new KN_Kiennghi_TraloiRepository();
        //KN_giamsatRepository _kn_giamsat = new KN_giamsatRepository();
        //KN_Giamsat_PhanloaiRepository _kn_giamsat_phanloai = new KN_Giamsat_PhanloaiRepository();
        //KN_Giamsat_DanhgiaRepository _kn_giamsat_danhgia = new KN_Giamsat_DanhgiaRepository();
        //KN_TonghopRepository _kn_tonghop = new KN_TonghopRepository();
        //KN_VanbanRepository _kn_vanban = new KN_VanbanRepository();
        BaseBusineess base_bussiness = new BaseBusineess();
        KiennghiBusineess _kn = new KiennghiBusineess();
        KntcBusineess _kntc = new KntcBusineess();
        ThietlapBusineess _tl = new ThietlapBusineess();
        Thietlap tl = new Thietlap();
        Dictionary<string, object> _condition;
        public string OptionDaiBieu_By_ID_COQUAN(int id_coquan,int id_daibieu)
        {
            string str = "";
            QUOCHOI_COQUAN coquan = _kn.HienThiThongTinCoQuan(id_coquan);
            if (coquan != null)
            {
                int id_diaphuong = (int)coquan.IDIAPHUONG;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("IDIAPHUONG", id_diaphuong);
                dic.Add("IDELETE", 0); dic.Add("IHIENTHI", 1);
                var daibieu = _kn.GetAll_Daibieu(dic);
                if (daibieu.Count() > 0)
                {
                    foreach(var d in daibieu)
                    {
                        string select = ""; if (d.IDAIBIEU == id_daibieu) { select = "selected"; }
                        str += "<option "+ select + " value='"+d.IDAIBIEU+"'>"+d.CTEN+"</option>";
                    }
                }
            }
            return str;
        }
        public string Option_ToDaiBieu(int id_phongban = 0, int iParent = -1)
        {
            string str = "";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if(iParent != -1)
                dic.Add("IPARENT", iParent);
            dic.Add("IDELETE", 0);
            var listToDaiBieu = _tl.GetBy_List_Phongban(dic);
            foreach (var toDaiBieu in listToDaiBieu)
            {
                string select = ""; if (toDaiBieu.IPHONGBAN == id_phongban) { select = " selected "; }
                str += "<option " + select + " value='" + toDaiBieu.IPHONGBAN + "'>" + toDaiBieu.CTEN + "</option>";
            }
            return str;
        }
        public string Option_Nguonkiennghi(List<KN_NGUONDON> nguondon, decimal id_chucvu = 0)
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

        public string Option_Nguonkiennghi_ChonNhieu(List<KN_NGUONDON> nguondon, string lstKN ="")
        {
            string str = "";
            string[] arrLstKN = lstKN.Split(',');
            var chucvu = nguondon.Where(x => x.IHIENTHI == 1 && x.IPARENT == 0 && x.IDELETE == 0).OrderBy(v => v.IVITRI).ToList();
            //str += "<optgroup label ='Chọn tất cả'>" + "</optgroup>";
            foreach (var p in chucvu)
            {
                string select = " ";
                if(lstKN == "all")
                {
                    select = " selected ";
                }
                else
                {
                    Boolean isCheck = Array.Exists(arrLstKN, element => element == p.INGUONDON.ToString());
                    if (isCheck)
                    {
                        select = " selected ";
                    }
                }
                
                str += "<option " + select  + " value='" + p.INGUONDON + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;

        }

        public string Option_Linhvucconquan_Chonnhieu(List<LINHVUC_COQUAN> linhvuc, string lstNV)
        {
            string str = "";
            string[] arrLstLV = lstNV.Split(',');
            foreach (var t in linhvuc)
            {
                string select = "";
                if (lstNV == "all")
                {
                    select = "selected ";
                }
                else
                {
                    Boolean isCheck = Array.Exists(arrLstLV, element => element == t.ILINHVUC.ToString());
                    if (isCheck)
                    {
                        select = " selected ";
                    }
                }

                if (t.IPARENT == 0)
                {
                    //str += "<optgroup label ='" + t.CCODE + "-" + EncodeOutput(t.CTEN) + "'>" + "</optgroup>";
                    str += "<option " + select + " value='" + t.ILINHVUC + "' class='bold'>" + t.CCODE + "-" + EncodeOutput(t.CTEN) + "</option>";
                }
                else
                {
                    var linhVucCha = _kn.GetBy_Linhvuc_CoquanID((int)(t.IPARENT));
                    while (linhVucCha.IPARENT != 0)
                    {
                        linhVucCha = _kn.GetBy_Linhvuc_CoquanID((int)(linhVucCha.IPARENT));
                    }
                    str += "<option " + select + " value='" + t.ILINHVUC + "'>" + t.CCODE + "-" + EncodeOutput(t.CTEN) + "</option>";
                }
            }
            return str;
        }

        public string Option_NguonKienNghi(int id_chucvu = 0)
        {
            string str = "";
            var chucvu = _kntc.List_NguonKienNghi().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.INGUONDON == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.INGUONDON + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

        public string OptionDiaPhuong_By_ID_COQUAN(int id_coquan, int id_huyen)
        {
            string str = "";
            QUOCHOI_COQUAN coquan = _kn.HienThiThongTinCoQuan(id_coquan);
            if (coquan != null)
            {
                int id_diaphuong = (int)coquan.IDIAPHUONG;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("IPARENT", id_diaphuong);
                dic.Add("IDELETE", 0); dic.Add("IHIENTHI", 1);

                var diaphuong = _kn.GetAll_DiaPhuong(dic);
                if (diaphuong.Count() > 0)
                {
                    foreach (var d in diaphuong)
                    {
                        
                        string select = ""; if (d.IDIAPHUONG == id_huyen) { select = "selected"; }
                        str += "<option " + select + " value='" + d.IDIAPHUONG + "'>"+d.CTYPE+" " + d.CTEN + "</option>";
                    }
                }
            }
            return str;
        }

        public string OptionDiaPhuong_By_Parent(int iParent, int iDiaPhuong = 0)
        {
            string str = "";
   
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("IPARENT", iParent);
                dic.Add("IDELETE", 0); dic.Add("IHIENTHI", 1);

                var diaphuong = _kn.GetAll_DiaPhuong(dic);
                if (diaphuong.Count() > 0)
                {
                    foreach (var d in diaphuong)
                    {
                        string select = ""; if (d.IDIAPHUONG == iDiaPhuong) { select = "selected"; }
                        str += "<option " + select + " value='" + d.IDIAPHUONG + "'>" + d.CTYPE + " " + d.CTEN + "</option>";
                    }
                }
            return str;
        }
        public string ChuongTring_ChiTiet(List<PRC_CHUONGTRINH_CHITIET> chitiet,string url_key)
        {
            string str = "";
            decimal id_daibieu = 0;
            int count = 1;
            foreach(var c in chitiet)
            {
                string id_encrt = HashUtil.Encode_ID(c.ID.ToString(), url_key);
                string edit = "<a onclick=\"ShowPopUp('id=" + id_encrt + "','/Kiennghi/Ajax_Chuongtrinh_chitiet_edit')\"  title='Cập nhật' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = "<a href=\"javascript: void()\" title=\"Hủy văn bản\" rel=\"tooltip\" onclick=\"DeletePage_Confirm('" + id_encrt + "', 'id=" + id_encrt + "', '/Kiennghi/Ajax_Chuongtrinh_chitiet_del', 'Bạn có muốn xóa Lịch tiếp của tổ đại biểu này?')\" class=\"trans_func\" data-original-title=\"Xóa lịch tiếp\"><i class=\"icon-trash\"></i></a> ";
                string ngaytiep = "";
                if (c.NGAYTIEP != DateTime.MinValue)
                {
                    ngaytiep = func.ConvertDateVN(c.NGAYTIEP.ToString());
                }
                str += "<tr><td class='tcenter'>" + count + "</td><td>"+EncodeOutput(c.TENDAIBIEU)+ "</td><td class='tcenter'>"+ 
                    EncodeOutput(c.TENDIAPHUONG)+"</td><td class='tcenter'>"+ ngaytiep+"</td><td class='tcenter'>"+ EncodeOutput(c.DIACHITIEP) + "</td><td class='tcenter' nowrap>" + edit+del + "</td> </tr>";
                count++;
            }
            return str;
        }
        public string ChuongTring_View(List<PRC_CHUONGTRINH_CHITIET> chitiet)
        {
            string str = "";
            int count = 1;
            foreach (var c in chitiet)
            {
                string ngaytiep = "";
                if (c.NGAYTIEP != DateTime.MinValue)
                {
                    ngaytiep = func.ConvertDateVN(c.NGAYTIEP.ToString());
                }
                str += "<tr><td class='tcenter'>" + count + "</td><td>" + EncodeOutput(c.TENDAIBIEU) + "</td><td class='tcenter'>" +
                    EncodeOutput(c.TENDIAPHUONG) + "</td><td class='tcenter'>" + EncodeOutput(c.TENDIAPHUONG2) + "</td><td class='tcenter'>" + ngaytiep + "</td><td class='tcenter'>" + EncodeOutput(c.DIACHITIEP) + "</td></tr>";
                count++;
            }
            return str;
        }
        public string List_Tonghop_Duthao(int id_tonghop, TaikhoanAtion act,string url_key)
        {
            string str = "";
            Dictionary<string, object> dic_duthao = new Dictionary<string, object>();
            dic_duthao.Add("ITONGHOP", id_tonghop);
            dic_duthao.Add("CLOAI", "duthao");
            Dictionary<string, object> dic_duthao_ketqua = new Dictionary<string, object>();
            dic_duthao_ketqua.Add("ITONGHOP", id_tonghop);
            dic_duthao_ketqua.Add("CLOAI", "duthao_ketqua");
            var duthao = _kn.Get_AllVanban(dic_duthao).ToList();
            var duthao_ketqua = _kn.Get_AllVanban(dic_duthao_ketqua).ToList();
            string id_tonghop_encr = HashUtil.Encode_ID(id_tonghop.ToString(), url_key);
            if (duthao.Count() > 0)
            {
                KN_VANBAN vanban = duthao.FirstOrDefault();
                string file = File_View((int)vanban.IVANBAN, "kn_vanban");
                string id_vanban_encr = HashUtil.Encode_ID(vanban.IVANBAN.ToString(), url_key);
                string edit = "<a onclick=\"ShowPopUp('id=" + id_vanban_encr + "','/Kiennghi/Ajax_Duthao_edit')\"  title='Cập nhật' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = "<a href=\"javascript: void()\" title=\"Hủy văn bản\" rel=\"tooltip\" onclick=\"DeletePage_Confirm('"+ id_vanban_encr + "', 'id="+ id_vanban_encr + "', '/Kiennghi/Ajax_Vanban_del', 'Bạn có muốn xóa văn bản này?')\" class=\"trans_func\" data-original-title=\"Xóa văn bản\"><i class=\"icon-trash\"></i></a> ";
                str += "<tr><td>Văn bản số <strong>"+EncodeOutput(vanban.CSOVANBAN)+"</strong> ban hành ngày "+func.ConvertDateVN(vanban.DNGAYBANHANH.ToString())+
                    ". "+ file + "</td><td class='tcenter' nowrap>"+EncodeOutput(vanban.CNGUOIKY)+"</td><td class='tcenter'>"+edit+del+"</td></tr>";
            }
            else
            {
                string add="<a onclick=\"ShowPopUp('id="+ id_tonghop_encr + "&type=duthao','/Kiennghi/Ajax_Duthao_add')\"  title='Cập nhật' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a> ";
                str += "<tr><td colspan='2'>Văn bản dự thảo Tập hợp gửi cơ quan có thẩm quyền xử lý</td><td class='tcenter'>"+ add + "</td></tr>";
            }
            if (duthao_ketqua.Count() > 0)
            {
                KN_VANBAN vanban = duthao_ketqua.FirstOrDefault();
                string file = File_View((int)vanban.IVANBAN, "kn_vanban");
                string id_vanban_encr = HashUtil.Encode_ID(vanban.IVANBAN.ToString(), url_key);
                string edit = "<a onclick=\"ShowPopUp('id=" + id_vanban_encr + "','/Kiennghi/Ajax_Duthao_edit')\"  title='Cập nhật' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = "<a href=\"javascript: void()\" title=\"Hủy văn bản\" rel=\"tooltip\" onclick=\"DeletePage_Confirm('" + id_vanban_encr + "', 'id=" + id_vanban_encr + "', '/Kiennghi/Ajax_Vanban_del', 'Bạn có muốn xóa văn bản này?')\" class=\"trans_func\" data-original-title=\"Xóa văn bản\"><i class=\"icon-trash\"></i></a> ";
                str += "<tr><td>Văn bản số <strong>" + EncodeOutput(vanban.CSOVANBAN) + "</strong> ban hành ngày " + func.ConvertDateVN(vanban.DNGAYBANHANH.ToString()) +
                    ". "+ file + "</td><td class='tcenter' nowrap>" + EncodeOutput(vanban.CNGUOIKY) + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            else
            {
                string add = "<a onclick=\"ShowPopUp('id=" + id_tonghop_encr + "&type=duthao_ketqua','/Kiennghi/Ajax_Duthao_add')\"  title='Cập nhật' rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a> ";
                str += "<tr><td colspan='2'>Văn bản trả lời dự thảo Tập hợp của cơ quan có thẩm quyền xử lý</td><td class='tcenter'>" + add + "</td></tr>";
            }
            return str;
        }
        public string EncodeOutput(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }
        public string SubString_KienNghi_Import(string noidung)
        {
            //if (noidung.Length >= 2000)
            //{
            //    noidung = noidung.Substring(0, 1996) + " ...";
            //}
            return noidung;
        }
        public string Get_TenKhoaHop_By_IDKyHop(int iKyHop)
        {
            string str = "";
            QUOCHOI_KYHOP kyhop = _kn.Get_KyHop_QuocHoi(iKyHop);
            if (kyhop != null)
            {
                QUOCHOI_KHOA khoa = _kn.Get_Khoa_QuocHoi((int)kyhop.IKHOA);
                if (khoa != null)
                {
                    str = EncodeOutput(khoa.CTEN);
                }
            }

            return str;
        }
        //HaiPN them vao do tieng Viet encode bi xu ly loi
        public string Get_TenKhoaHop_By_IDKyHop_KhongEncode(int iKyHop)
        {
            string str = "";
            QUOCHOI_KYHOP kyhop = _kn.Get_KyHop_QuocHoi(iKyHop);
            if (kyhop != null)
            {
                QUOCHOI_KHOA khoa = _kn.Get_Khoa_QuocHoi((int)kyhop.IKHOA);
                if (khoa != null)
                {
                    str = khoa.CTEN;
                }
            }

            return str;
        }
        public string Get_TenDiaPhuong(int iDiaPhuong)
        {
            string str = "";
            if (iDiaPhuong == 0) { return str; }
            DIAPHUONG dp = _kn.HienThiThongTinDiaPhuong(iDiaPhuong);
            if (dp != null) { return EncodeOutput(dp.CTEN); }
            return str;
        }
        public string Get_TenKyHop(int iKyHop)
        {
            string str = "";
            QUOCHOI_KYHOP kyhop = _kn.Get_KyHop_QuocHoi(iKyHop);
            if (kyhop != null)
            {
                str = kyhop.CTEN;
            }
            str = EncodeOutput(str);
            return str;
        }
        public string List_KienNghiTrung(List<PRC_KIENNGHI_TRUNG> all_kiennghi, KN_KIENNGHI kn, int type, string url_cookie)
        {
            //KN_KiennghiRepository _kiennghi = new KN_KiennghiRepository();
            string str = "";
            if (type == 1)
            {
                all_kiennghi = all_kiennghi.Where(x => x.ID_KYHOP_TIEPNHAN == (int)kn.IKYHOP).ToList();
            }
            else
            {
                all_kiennghi = all_kiennghi.Where(x => x.ID_KYHOP_TIEPNHAN != (int)kn.IKYHOP).ToList();
            }
            int count = 1;
            if (all_kiennghi.Count() == 0 && type == 1)
            {
                return "<tr><td colspan='4' class='alert alert-success tcenter'>Không tìm thấy kiến nghị cùng kỳ họp nào có nội dung tương tự!</td></tr>";
            }
            if (all_kiennghi.Count() == 0 && type == 0)
            {
                return "<tr><td colspan='5' class='alert alert-success tcenter'>Không tìm thấy kiến nghị nào ở các kỳ họp khóa trước có nội dung tương tự!</td></tr>";
            }
            //string url_cookie = func.Get_Url_keycookie();
            string id_encr_kiennghi = HashUtil.Encode_ID(kn.IKIENNGHI.ToString(), url_cookie);
            foreach (var k in all_kiennghi)
            {
                string id_encr_trung = HashUtil.Encode_ID(k.ID_KIENNGHI.ToString(), url_cookie);
                //KN_CL k_ = KienNghi_Detail((int)k.ID_KIENNGHI, url_cookie);
                string kyhop = "";
                if (type == 0)
                {
                    kyhop = "<td class='tcenter'>" + EncodeOutput(k.TEN_KYHOP_TIEPNHAN) + "</br>" + EncodeOutput(k.TEN_KHOAHOP) + "</td>";
                }
                string chon = "<a id='btn_" + id_encr_trung + "' title='Chọn kiến nghị trùng' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonKienNghiTrung('" +
                    id_encr_trung + "','id_trung=" + id_encr_trung + "&id=" + id_encr_kiennghi + "','/Kiennghi/Ajax_KienNghi_trung_update')\" class='chontrung f-grey'><i class='icon-ok-sign'></i></a>";
                if (kn.IKIENNGHI_TRUNG == k.ID_KIENNGHI)
                {
                    chon = "<a id='btn_" + id_encr_trung + "' title='Bỏ chọn trùng' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonKienNghiTrung('" +
                    id_encr_trung + "','id_trung=" + id_encr_trung + "&id=" + id_encr_kiennghi + "','/Kiennghi/Ajax_KienNghi_trung_update')\" class='trans_func chontrung'><i class='icon-ok-sign'></i></a>";
                }
                string thamquyendonvi = "<strong>Thẩm quyền giải quyết:</strong> "+ EncodeOutput(k.TEN_DONVITHAMQUYEN);
                str += "<tr><td class='tcenter b'>" + count + "</td><td class='tcenter'>" + chon + "</td><td><p>" + EncodeOutput(k.NOIDUNG_KIENNGHI) + "</p>" + thamquyendonvi+
                        "</td><td class='tcenter'>" + EncodeOutput(k.TEN_DONVITIEPNHAN) + "</td>" + kyhop + "</tr>";
                count++;
            }
            return str;
        }
        public string Option_All_Tinh_Huyen(List<DIAPHUONG> diaphuong, int id_parent = 0, int level = 0, int id_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            var list = diaphuong.Where(x => x.IPARENT == id_parent && x.IHIENTHI == 1).OrderBy(x => x.CTEN).ToList();
            foreach (var l in list)
            {
                string select = ""; if (l.IDIAPHUONG == id_choice) { select = " selected "; }
                str += "<option " + select + " value='" + l.IDIAPHUONG + "'>" + space_level + EncodeOutput(l.CTEN) + "</option>";
                str += Option_All_Tinh_Huyen(diaphuong, (int)l.IDIAPHUONG, level + 1, id_choice);
            }
            return str;
        }
        public string Option_DaiBieu_ByID_DiaPhuong(List<DAIBIEU> daibieu, int id)
        {
            string str = "";
            foreach (var d in daibieu)
            {
                string select = ""; if (d.IDAIBIEU == id) { select = "selected"; }
                string truongdoan = ""; if (d.ITRUONGDOAN == 1) { truongdoan = " (Trưởng đoàn)"; }
                str += "<option " + select + " value='" + d.IDAIBIEU + "'>" + EncodeOutput(d.CTEN) + truongdoan + "</option>";
            }
            return str;
        }
        public string File_View(int id, string type)
        {
            FileuploadRepository _file = new FileuploadRepository();
            string str = "";
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("ID", id);
            _dic.Add("CTYPE", type);
            var file = _file.GetAll(_dic).ToList();
            if (file.Count() > 0)
            {
                str += "";
                foreach (var f in file)
                {
                    string[] f_ = f.CFILE.Split('/');
                    string id_encrt = HashUtil.Encode_ID(f.ID_FILE.ToString(), "");
                    //lấy địa chỉ file upload cùng thư mục code
                    //string file_path = f.CFILE;
                    //lấy địa chỉ file upload ngoài thư mục code
                    string file_path = "/Home/Download/"+ id_encrt;
                    //string dir_path_download = HashUtil.dir_path_download;
                    //if (dir_path_download != "") { file_path = dir_path_download + file_path; }
                    str += " <a href='" + file_path + "' class=''><i class='icon-download-alt'></i> </a>";
                }
                str += "";
            }

            return str;
        }
        public string Option_ChuongTrinh_ByKyHop(List<KN_CHUONGTRINH> chuongtrinhList, int selectedChuongTrinhId = 0)
        {
            string str = "";
            foreach (var k in chuongtrinhList)
            {
                string select = ""; if (k.ICHUONGTRINH == selectedChuongTrinhId) { select = " selected "; }
                str += "<option " + select + " value='" + k.ICHUONGTRINH + "'>" + EncodeOutput(k.CKEHOACH) + "</option>";
            }
            return str;
        }
        public string Option_ChuongTrinh_ByKyHop_And_UserType(List<KN_CHUONGTRINH> chuongtrinhList, UserInfor u, int selectedChuongTrinhId = 0) 
        {
            string str = "";
            chuongtrinhList = chuongtrinhList.OrderByDescending(x => x.DDATE).ToList();
            if (!u.tk_action.is_lanhdao)
            {
                if (u.user_login.ITYPE == (int)UserType.ChuyenVienDBQH)
                    chuongtrinhList = chuongtrinhList.Where(x => x.IDOITUONG == 0).ToList();
                else
                    chuongtrinhList = chuongtrinhList.Where(x => x.IDOITUONG == 1).ToList();
            }
            
            foreach (var k in chuongtrinhList)
            {
                string select = ""; if (k.ICHUONGTRINH == selectedChuongTrinhId) { select = " selected "; }
                str += "<option " + select + " value='" + k.ICHUONGTRINH + "'>" + EncodeOutput(k.CKEHOACH) + "</option>";
            }
            return str;
        }
        public string File_Edit(int id, string type, string url_cookie)
        {
            FileuploadRepository _file = new FileuploadRepository();
            string str = "";
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("ID", id);
            _dic.Add("CTYPE", type);
            var file = _file.GetAll(_dic).ToList();
            foreach (var f in file)
            {
                string id_encr = HashUtil.Encode_ID(f.ID_FILE.ToString(), url_cookie);
                string[] f_ = f.CFILE.Split('/');
                string id_encrt = HashUtil.Encode_ID(f.ID_FILE.ToString(), "");
                str += "<p id='file_" + id_encr + "'><a href='/Home/Download/"+ id_encrt+"'>" + f_[f_.Length - 1] +
                        "<a> <a href='javascript:void(0)' onclick=\"DeleteFile('" + id_encr +
                        "', '/Kiennghi/Ajax_Delele_file')\" class='f-orangered' title='Hủy'><i class='icon-remove'></i><a></p>";
            }
            return str;
        }
        public string Option_Khoa_KyHop(List<QUOCHOI_KHOA> khoa, List<QUOCHOI_KYHOP> kyhop, int ikyhop)
        {
            string str = "";
            khoa = khoa.Where(x => x.IHIENTHI == 1).OrderBy(x => x.DBATDAU).ToList();
            foreach (var k in khoa)
            {
                var kyhop1 = kyhop.Where(x => x.IKHOA == (int)k.IKHOA && x.IHIENTHI == 1).OrderBy(x => x.DBATDAU).ToList();
                if (kyhop1.Count() > 0)
                {
                    str += "<optgroup label='" + k.CTEN + "'>";
                    foreach (var t in kyhop1)
                    {
                        string select = ""; if (t.IKYHOP == ikyhop) { select = "selected"; }
                        str += "<option " + select + " value='" + t.IKYHOP + "'>" + EncodeOutput(t.CTEN) + "</option>";
                    }
                    str += "</optgroup>";
                }
            }
            return str;
        }

        public string Option_KyHop_TheoKhoa(List<QUOCHOI_KYHOP> kyhop, int ikyhop, int iKyHop_TheoKhoa)
        {
            string str = "";
            int count = 0;
            var kyhop1 = kyhop.Where(x => x.IKHOA == (int)iKyHop_TheoKhoa && x.IHIENTHI == 1).OrderBy(x => x.DBATDAU).ToList();
            if (kyhop1.Count() > 0)
            {
                foreach (var t in kyhop1)
                {
                    count += 1;
                    str += "<input style='margin-bottom:5px' type='checkbox' id='" + t.IKYHOP + "' name='ckboxKyhop' value='" + t.IKYHOP + "'> " +
                    "<label style='float: unset !important; display: inline-block !important; width:16rem' for='" + t.IKYHOP + "'> " + EncodeOutput(t.CTEN) + "</label>";
                    if (count % 2 == 0)
                    {
                        str += "<br />";
                    }
                }
                
            }
            return str;
        }

        
        public string Option_Khoa(List<QUOCHOI_KHOA> khoa, int ikyhop)
        {
            string str = "";
            khoa = khoa.Where(x => x.IHIENTHI == 1).OrderBy(x => x.DBATDAU).ToList(); ;
            foreach (var k in khoa)
            {
                str += "<option ";
                if (k.IKHOA == ikyhop)
                    str += "selected ";
                str += "value='" + k.IKHOA + "'>" + EncodeOutput(k.CTEN) + "</option>";
            }
            return str;
        }
        
        public string Get_Ten_TruocKyHop(int iTruocKyHop)
        {

            string str = "";
            var hinhthuc = new List<TruocKyHop>
            {
                new TruocKyHop { ten = "Trước kỳ họp", value = 1, class_span = "span5" },
                new TruocKyHop { ten = "Sau kỳ họp", value = 0, class_span = "span4" },
                new TruocKyHop { ten = "Khác", value = 2, class_span = "span3" }
            };
            if(hinhthuc.Where(x => x.value == iTruocKyHop).Count() > 0)
            {
                return hinhthuc.Where(x => x.value == iTruocKyHop).FirstOrDefault().ten;
            }

            return str;
        }
        public ChuongtrinhCuTri Chuongtring_detail(int id, string id_encr)
        {
            KN_CHUONGTRINH ct = _kn.Get_ChuongTrinh_ByID(id);
            ChuongtrinhCuTri c = new ChuongtrinhCuTri();
            
            if (ct.DNGAYBANHANH != null)
                c.ngaybanhanh = func.ConvertDateVN(ct.DNGAYBANHANH.ToString());
            if (ct.DBATDAU != null)
                c.batdau = func.ConvertDateVN(ct.DBATDAU.ToString());
            if (ct.DKETTHUC != null)
                c.ketthuc = func.ConvertDateVN(ct.DKETTHUC.ToString());
            if (_kn.Get_KyHop_QuocHoi((int)ct.IKYHOP) != null)
            {
                QUOCHOI_KYHOP k = _kn.Get_KyHop_QuocHoi((int)ct.IKYHOP);
                c.kyhop = EncodeOutput(k.CTEN);
                if (_kn.Get_Khoa_QuocHoi((int)k.IKHOA) != null)
                {
                    c.khoahop = EncodeOutput(_kn.Get_Khoa_QuocHoi((int)k.IKHOA).CTEN);
                }

            }
            c.truockyhop = Get_Ten_TruocKyHop((int)ct.ITRUOCKYHOP);
            c.file_view = File_View(id, "kn_chuongtrinh");
            //c.daibieu_view = ChuongTrinh_DaiBieu(id);
            //c.diaphuong_view = ChuongTrinh_DiaPhuong(id);
            var coquan = _kn.HienThiThongTinCoQuan((int)ct.IDONVI);
            if (coquan != null)
            {
                c.doandaibieu = EncodeOutput(coquan.CTEN);
            }
            c.bt_info = " <a href='/Kiennghi/Chuongtrinh_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
            string edit = " <a href=\"/Kiennghi/Suachuongtrinh?id=" + id_encr + "\" title='Sửa chương trình' class='trans_func'><i class='icon-pencil'></i></a> ";
            string del = " <a href=\"javascript:void()\" title='Xóa'  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Chuongtrinh_del','Bạn có muốn xóa chương trình này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
            c.bt_edit = edit;
            c.bt_del = del;
            return c;
        }
        public string OptionCoQuanXuLy_WithParent(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
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
                string select = "";
                if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + EncodeOutput(donvi.CTEN) + "</option>";
                if (coquan.Where(x => x.IPARENT == (int)donvi.ICOQUAN && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).Count() > 0)
                {
                    str += OptionCoQuanXuLy_WithParent(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                }
            }
            return str;
        }
        public string OptionCoQuanXuLy(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            string str = "";
            string space_level = "";
            str += "<option value='0'> Chọn đơn vị tiếp nhận</option>";
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
        public string Option_CoquanDiaPhuong(List<QUOCHOI_COQUAN> listCoQuan , int iDonVi, int iType)
        {
            listCoQuan = listCoQuan.OrderBy(x => x.ICOQUAN).ToList();
            string str = "";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (iType == (int)ThamQuyen_DiaPhuong.Huyen)
            {
                var listParent = _kn.GetAll_CoQuanByParam(dic).Where(x=> x.ICOQUAN == (int)Parent_Huyen.UyBanMatTran || x.ICOQUAN == (int)Parent_Huyen.DonViHanhChinh || x.ICOQUAN == (int)Parent_Huyen.HuyenUy);
                foreach (var huyen in listParent)
                {
                    str += "<optgroup label='" + huyen.CTEN +"'>";
                    foreach (var t in listCoQuan)
                    {
                        if(t.IPARENT == huyen.ICOQUAN)
                        {
                            str += "<option ";
                            if (t.ICOQUAN == iDonVi)
                                str += "selected ";
                            str += "value='" + t.ICOQUAN + "'>" + t.CTEN + "</option>";
                        }
                       
                    }
                    str += "</optgroup>";
                    
                }
                return str;
            }
            else
            {
                if (iType == (int)ThamQuyen_DiaPhuong.Trunguong)
                {
                    dic.Add("ICOQUAN", (int)Parent_Tinh_TW.TW);
                    var trungUongParent = _kn.GetAll_CoQuanByParam(dic).FirstOrDefault();
                    str += "<optgroup label='" + trungUongParent.CTEN + "'>";
                }
                else
                {
                    dic.Add("ICOQUAN", (int)Parent_Tinh_TW.TINH);
                    var tinhParent = _kn.GetAll_CoQuanByParam(dic).FirstOrDefault();
                    str += "<optgroup label='" + tinhParent.CTEN + "'>";
                }
                foreach (var t in listCoQuan)
                {

                    str += "<option ";
                    if (t.ICOQUAN == iDonVi)
                        str += "selected ";
                    str += "value='" + t.ICOQUAN + "'>" + t.CTEN + "</option>";
                }
                str += "</optgroup>";
                return str;
            }
           
        }
        //public string List_ChuongTrinh(List<KN_CHUONGTRINH> chuongtrinh, List<QUOCHOI_COQUAN> coquan, TaikhoanAtion act)
        //{
        //    string str = "";
        //    if (chuongtrinh.Count() == 0)
        //    {
        //        return "<tr><td colspan='6' class='alert-danger tcenter'>Không tìm thấy kết quả nào!</td></tr>";
        //    }
        //    string url_cookie = func.Get_Url_keycookie();
        //    coquan = coquan.OrderBy(x => x.CTEN).ToList();
        //    foreach (var cq in coquan)
        //    {
        //        var chuongtrinh_coquan = chuongtrinh.Where(x => x.IDONVI == (int)cq.ICOQUAN).OrderBy(x => x.DBATDAU).ToList();
        //        if (chuongtrinh_coquan.Count() > 0)
        //        {
        //            str += "<tr ><th colspan='6'>" + cq.CTEN + " (" + chuongtrinh_coquan.Count + ")</th></tr>";
        //            int count = 1;
        //            foreach (var ch in chuongtrinh_coquan)
        //            {

        //                string id_encr = HashUtil.Encode_ID(ch.ICHUONGTRINH.ToString(), url_cookie);
        //                ChuongtrinhCuTri ct = Chuongtring_detail((int)ch.ICHUONGTRINH, id_encr);
        //                _condition = new Dictionary<string, object>();
        //                _condition.Add("ICHUONGTRINH", ch.ICHUONGTRINH);
        //                if (_kn.HienThiDanhSachKienNghi(_condition).Count() > 0) { ct.bt_del = ""; }
        //                if ((int)ch.IUSER != act.iUser)
        //                {
        //                    ct.bt_del = ""; ct.bt_edit = "";
        //                }
        //                str += "<tr id=\"tr_" + id_encr + "\"><td class='tcenter b'>" + count + "</td><td><p><strong>" + ch.CKEHOACH +
        //                    "</strong></p><p>" + ch.CNOIDUNG + "</p></td><td class='tcenter'>" +
        //                    func.ConvertDateVN(ch.DBATDAU.ToString()) + "</br><strong>" + func.ConvertDateVN(ch.DKETTHUC.ToString()) + "</strong>" +
        //                    "</td><td>" + ct.diaphuong_view + "</td><td>" + ct.daibieu_view +
        //                    "</td><td class='tcenter' nowrap>" + ct.bt_info + ct.bt_edit + ct.bt_del + "</td></tr>";

        //                count++;
        //            }
        //        }

        //    }

        //    return str;
        //}
        public string List_ChuongTrinh_DiaPhuong_View(List<PRC_CHUONGTRINH_TXCT> chuongtrinh)
        {
            string str = "";
            List<decimal> list_id_daibieu = new List<decimal>();
            foreach (var c in chuongtrinh)
            {
                if (!list_id_daibieu.Contains(c.ID_DIAPHUONG1) && c.ID_DIAPHUONG1 != 0)
                {
                    list_id_daibieu.Add(c.ID_DIAPHUONG1);
                    str += "- " + EncodeOutput(c.TENDIAPHUONG1) + "</br>";
                }
            }
            return str;
        }
        public string List_ChuongTrinh_DaiBieu_View(List<PRC_CHUONGTRINH_TXCT> chuongtrinh)
        {
            string str = "";
            List<decimal> tendaibieu = new List<decimal>();
            foreach (var c in chuongtrinh)
            {
                if (!tendaibieu.Contains(c.ID_DAIBIEU) && c.ID_DAIBIEU != 0)
                {
                    tendaibieu.Add(c.ID_DAIBIEU);
                    str += "- " + EncodeOutput(c.TENDAIBIEU) + "</br>";
                }
            }
            return str;
        }
        public string List_ChuongTrinh(List<PRC_CHUONGTRINH_TXCT> chuongtrinh, TaikhoanAtion act,string type_search="")
        {
            string str = "";
            if (chuongtrinh.Count() == 0)
            {
                return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy kết quả nào!</td></tr>";
            }else
            {
                if (type_search == "search") {
                    //List<decimal> list_id_chuongtrinh = new List<decimal>();
                    //foreach (var c in chuongtrinh)
                    //{
                    //    if (!list_id_chuongtrinh.Contains(c.ICHUONGTRINH)) { list_id_chuongtrinh.Add(c.ICHUONGTRINH); }
                    //}
                    str = "<tr><td colspan='4' class='alert-info tcenter'>Có " + chuongtrinh.Count() + " kết quả tìm kiếm.</td></tr>";
                }
            }
            string url_cookie = func.Get_Url_keycookie();
            decimal id_donvi = 0;
            int count = 1;
            foreach(var c in chuongtrinh)
            {
                if (c.ID_DIAPHUONGTIEPXUC != id_donvi)
                {
                    str += "<tr ><th colspan='4'>" + EncodeOutput(c.TENDIAPHUONG) + "</th></tr>";
                    count = 1;
                }
                string id_encr = HashUtil.Encode_ID(c.ICHUONGTRINH.ToString(), url_cookie);
                string info = " <a href='/Kiennghi/Chuongtrinh_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
                string chitiet = " <a href='/Kiennghi/Chuongtrinh_chitiet/?id=" + id_encr + "' title='Lịch tiếp đại biểu'  class='trans_func'><i class='icon-calendar'></i></a>";
                string edit = " <a href=\"/Kiennghi/Suachuongtrinh?id=" + id_encr + "\" title='Sửa chương trình' class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" title='Xóa'  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Chuongtrinh_del','Bạn có muốn xóa chương trình này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                if ((int)c.ID_USER_CAPNHAT != act.iUser && !base_bussiness.Action_(50, act))
                {
                    edit = ""; del = "";chitiet = "";
                }
                //string diaphuong1 = "";string daibieu = "";
                //if (c.TENDIAPHUONG1 != null) { diaphuong1 = "- "+c.TENDIAPHUONG1.Replace(", ", "</br>- "); }
                //if (c.TENDAIBIEU != null) { daibieu = "- " + c.TENDAIBIEU.Replace(", ", "</br>- "); }
                str += "<tr><td class='tcenter b'>" + count + "</td><td><p><strong>" + EncodeOutput(c.CKEHOACH) +
                    "</strong></p><p>" + EncodeOutput(c.CNOIDUNG) + "</p></td><td class='tcenter'>";
                if (c.DBATDAU != null)
                    str += func.ConvertDateVN(c.DBATDAU.ToString());
                str += "</br><strong>";
                if (c.DKETTHUC != null)
                    str += func.ConvertDateVN(c.DKETTHUC.ToString());
                str+= "</strong>" +
                    "</td><td class='tcenter' nowrap>" + info + edit + del + "</td></tr>";
                count++;
                id_donvi = c.ID_DIAPHUONGTIEPXUC;
            }
            //List<decimal> list_id_donvi = new List<decimal>();
            //var chuongtrinh1 = chuongtrinh;
            //foreach (var c in chuongtrinh1)
            //{
            //    if (!list_id_donvi.Contains(c.ID_DIAPHUONGTIEPXUC)) {
            //        list_id_donvi.Add(c.ID_DIAPHUONGTIEPXUC);
            //        var chuongtrinh_diaphuong = chuongtrinh1.Where(x => x.ID_DIAPHUONGTIEPXUC == c.ID_DIAPHUONGTIEPXUC).OrderBy(x => x.DBATDAU).ToList();
            //        str += "<tr ><th colspan='6'>" + EncodeOutput(chuongtrinh_diaphuong.FirstOrDefault().TENDIAPHUONG) +"</th></tr>";
            //        List<decimal> list_id_ct = new List<decimal>();
            //        int count = 1;
            //        foreach (var ch in chuongtrinh_diaphuong)
            //        {
            //            if (!list_id_ct.Contains(ch.ICHUONGTRINH))
            //            {
            //                list_id_ct.Add(ch.ICHUONGTRINH);
            //                string id_encr = HashUtil.Encode_ID(ch.ICHUONGTRINH.ToString(), url_cookie);
            //                string info = " <a href='/Kiennghi/Chuongtrinh_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
            //                string edit = " <a href=\"/Kiennghi/Suachuongtrinh?id=" + id_encr + "\" title='Sửa chương trình' class='trans_func'><i class='icon-pencil'></i></a> ";
            //                string del = " <a href=\"javascript:void()\" title='Xóa'  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Chuongtrinh_del','Bạn có muốn xóa chương trình này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
            //                if ((int)ch.ID_USER_CAPNHAT != act.iUser && !base_bussiness.Action_(50,act))
            //                {
            //                    edit = ""; del = "";
            //                }
            //                str += "<tr><td class='tcenter b'>" + count + "</td><td><p><strong>" + EncodeOutput(ch.CKEHOACH) +
            //                    "</strong></p><p>" + EncodeOutput(ch.CNOIDUNG) + "</p></td><td class='tcenter'>" +
            //                    func.ConvertDateVN(ch.DBATDAU.ToString()) + "</br><strong>" + func.ConvertDateVN(ch.DKETTHUC.ToString()) + "</strong>" +
            //                    "</td><td>" + List_ChuongTrinh_DiaPhuong_View(chuongtrinh_diaphuong.Where(x => x.ICHUONGTRINH == ch.ICHUONGTRINH).ToList()) +
            //                    "</td><td>" + List_ChuongTrinh_DaiBieu_View(chuongtrinh_diaphuong.Where(x => x.ICHUONGTRINH == ch.ICHUONGTRINH).ToList()) +
            //                    "</td><td class='tcenter' nowrap>" + info + edit + del + "</td></tr>";
            //                count++;
            //            }
            //        }
            //    }
            //}
            //foreach(var  d in list_id_donvi)
            //{
            //    List<decimal> list_id_chuongtrinh = new List<decimal>();
            //    var chuongtrinh_diaphuong = chuongtrinh.Where(x => x.ID_DIAPHUONGTIEPXUC == d).OrderBy(x => x.DBATDAU).ToList();
            //    foreach(var c in chuongtrinh_diaphuong)
            //    {
            //        if (!list_id_chuongtrinh.Contains(c.ICHUONGTRINH)) { list_id_chuongtrinh.Add(c.ICHUONGTRINH); }                    
            //    }
            //    str += "<tr ><th colspan='6'>" + chuongtrinh.Where(x => x.ID_DIAPHUONGTIEPXUC == d).FirstOrDefault().TENDIAPHUONG + 
            //        " (" + list_id_chuongtrinh.Count() + ")</th></tr>";

            //    int count = 1;
            //    foreach (var t in list_id_chuongtrinh)
            //    {
            //        PRC_CHUONGTRINH_TXCT ch = chuongtrinh_diaphuong.Where(x => x.ICHUONGTRINH == t).FirstOrDefault();
            //        string id_encr = HashUtil.Encode_ID(ch.ICHUONGTRINH.ToString(), url_cookie);
            //        string info = " <a href='/Kiennghi/Chuongtrinh_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
            //        string edit = " <a href=\"/Kiennghi/Suachuongtrinh?id=" + id_encr + "\" title='Sửa chương trình' class='trans_func'><i class='icon-pencil'></i></a> ";
            //        string del = " <a href=\"javascript:void()\" title='Xóa'  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Chuongtrinh_del','Bạn có muốn xóa chương trình này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
            //        if ((int)ch.ID_USER_CAPNHAT != act.iUser && !act.is_admin)
            //        {
            //            edit = ""; del = "";
            //        }
            //        str += "<tr id=\"tr_" + id_encr + "\"><td class='tcenter b'>" + count + "</td><td><p><strong>" + ch.CKEHOACH +
            //            "</strong></p><p>" + ch.CNOIDUNG + "</p></td><td class='tcenter'>" +
            //            func.ConvertDateVN(ch.DBATDAU.ToString()) + "</br><strong>" + func.ConvertDateVN(ch.DKETTHUC.ToString()) + "</strong>" +
            //            "</td><td>" + List_ChuongTrinh_DiaPhuong_View(chuongtrinh_diaphuong.Where(x => x.ICHUONGTRINH == t && x.TENDIAPHUONG != null).ToList()) +
            //            "</td><td>" + List_ChuongTrinh_DaiBieu_View(chuongtrinh_diaphuong.Where(x => x.ICHUONGTRINH == t && x.TENDAIBIEU != null).ToList()) +
            //            "</td><td class='tcenter' nowrap>" + info + edit + del + "</td></tr>";
            //        count++;
            //    }
            //}
            return str;
        }

        internal string OptionDaiBieuHDND(int iDiaPhuong, int iDoiTuong, int iDaiBieuSelected = 0)
        {
            StringBuilder str = new StringBuilder("<option value='0'> Chọn đại biểu</ option >");
            Dictionary<string, object> daibieuParam = new Dictionary<string, object>();
            daibieuParam.Add("IDIAPHUONG", iDiaPhuong);
            daibieuParam.Add("ILOAIDAIBIEU", iDoiTuong);
            daibieuParam.Add("IDELETE", 0);
            daibieuParam.Add("IHIENTHI", 1);
            var listDaiBieu = _kn.GetAll_Daibieu(daibieuParam);
            if (listDaiBieu != null && listDaiBieu.Count() > 0)
            {
                foreach(var daibieu in listDaiBieu)
                {
                    if(daibieu.IDAIBIEU == iDaiBieuSelected)
                    {
                        str.Append("<option value='" + daibieu.IDAIBIEU + "' selected>" + daibieu.CTEN + "</option>");
                    }
                    else
                    {
                        str.Append("<option value='" + daibieu.IDAIBIEU + "'>" + daibieu.CTEN + "</option>");
                    }
                }
            }
            return str.ToString();
        }

        public string List_Import_Taphop(List<PRC_TAPHOP_IMPORT> inport)
        {
            string str = "";
            decimal id_tonghop = 0; int count = 1;
            foreach (var d in inport)
            {
                if (id_tonghop != d.ID_TONGHOP)
                {
                    str += "<tr><th colspan='3'>" + d.NOIDUNG_TONGHOP + "</th></tr>";
                }
                str += "<tr><td class='tcenter'>" + count + "</td><td colspan=''>" + EncodeOutput(d.NOIDUNG_KIENNGHI) + "</td><td class='tcenter'>" + d.TENDONVI_THAMQUYEN + "</td></tr>";
                count++;
                id_tonghop = (decimal)d.ID_TONGHOP;
            }
            return str;
        }
        public string List_Import_Kiennghi(List<PRC_KIENNGHI_IMPORT_LISTKN> import,string url_key)
        {
            string str = "";
            decimal id_donvi_tiepnhan = 0;int count = 1;

            foreach(var d in import)
            {
                string id_encr = HashUtil.Encode_ID(d.IKIENNGHI.ToString(), url_key);
                string detail = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
                string del = "<a href=\"javascript:void()\" title='Xóa' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kiennghi/Ajax_Kiennghi_del_import','Bạn có muốn xóa kiến nghị này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                str += "<tr><td class='tcenter'>" + count + "</td><td class='tcenter' colspan=''>" + d.CMAKIENNGHI + "</td><td colspan=''>" + d.CNOIDUNG + "</td><td class='tcenter' colspan=''>" + d.TENDONVI_TIEPNHAN + "</td><td class='tcenter' colspan=''>" + d.TEN_LINHVUC + "</td><td class='tcenter'>" + d.TENDONVI_THAMQUYEN + "</td><td class='tcenter'>"+ detail+ del + "</td></tr>";
                count++;
                
            }
            return str;
        }
        public string List_Import(List<KN_IMPORT> import,string url_key)
        {
            string str = "";
            int count = 1;
            foreach (var im in import)
            {
                string truockyhop = Get_Ten_TruocKyHop((int)im.ITRUOCKYHOP);
                //if ( == 0)
                //{
                //    truockyhop = "Sau kỳ họp";
                //}
                string id_encr = HashUtil.Encode_ID(im.ID.ToString(), url_key);
                string chitiet = " <a href='/Kiennghi/Import_kiennghi/?id=" + id_encr + "' title='Danh sách kiến nghị đã import'  class='trans_func' data-original-title=\"Danh sách kiến nghị đã import\"><i class='icon-list-ol'></i></a>";
                string delete = "<a href =\"javascript: void()\" title=\"Xóa danh sách kiến nghị đã import\" rel=\"tooltip\" onclick=\"DeletePage_Confirm('" + id_encr + "', 'id=" + id_encr + "', '/Kiennghi/Ajax_Import_kiennghi_del', 'Bạn có muốn xóa danh sách kiến nghị đã import này?')\" class=\"trans_func\" data-original-title=\"Xóa danh sách kiến nghị đã import\"><i class=\"icon-trash\"></i></a> ";
                string taphop = " <a href='/Kiennghi/Import_taphop/?id=" + id_encr + "' title='Danh sách tập hợp kiến nghị đã tạo'  class='trans_func'><i class='icon-list'></i></a>";
                string taotonghop = "";
                if (im.ITINHTRANG == 1)
                {
                    taotonghop = "<i class='icon-ok-sign f-green' title='Đã tạo tập hợp chuyển xử lý'></i>";

                }else { taphop = ""; }
                str += "<tr><td class='tcenter b'>" + count + "</td><td class='tcenter'>" + func.ConvertDateVN(im.DDATE.ToString()) +
                            "</td><td><p>" + EncodeOutput(im.CGHICHU) + "</p><p><strong>" + truockyhop + 
                            "</strong></p></td><td class='tcenter b'>" + im.ISOKIENNGHI + 
                            "</td><td class='tcenter b'>" + chitiet + delete + taphop + "</td></tr>";

                count++;
            }

            return str;
        }
        public string ChuongTrinh_DiaPhuong(int ichuongtrinh)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("ICHUONGTRINH", ichuongtrinh);
            var daibieu = _kn.GetAll_ChuongTrinh_DiaPhuong(_condition).ToList();
            var diaphuong = _kn.GetAll_DiaPhuong();
            foreach (var d in daibieu)
            {
                //string tendiaphuong0 = _diaphuong.GetByID((int)d.IDIAPHUONG0).CTEN;
                var dp = diaphuong.Where(x => x.IDIAPHUONG == (int)d.IDIAPHUONG1);
                if (dp.Count() > 0)
                {
                    string tendiaphuong1 = diaphuong.Where(x => x.IDIAPHUONG == (int)d.IDIAPHUONG1).FirstOrDefault().CTEN;
                    str += "- " + EncodeOutput(tendiaphuong1) + "</br>";
                }
                
            }
            return str;
        }
        public string ChuongTrinh_DaiBieu(int ichuongtrinh)
        {
            string str = "";
            DaibieuReposity _daibieu = new DaibieuReposity();
            _condition = new Dictionary<string, object>();
            _condition.Add("ICHUONGTRINH", ichuongtrinh);
            var daibieu = _kn.GetAll_ChuongTrinh_DaiBieu(_condition).ToList();
            foreach (var d in daibieu)
            {
                DAIBIEU u = _daibieu.GetByID((int)d.IUSER_DAIBIEU);
                if (u != null)
                {
                    string truongdoan = ""; if (u.ITRUONGDOAN == 1) { truongdoan = " (Trưởng đoàn)"; }
                    str += "- " + EncodeOutput(u.CTEN) + " " + truongdoan + "</br>";
                }               
            }
            return str;
        }
        public string KN_Lichsu(int id_kiennghi)
        {
            TrackingRepository track = new TrackingRepository();
            var all_user = _kn.GetAll_Users();
            string str = "";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IKIENNGHI", id_kiennghi);
            var lichsu = track.GetAll(dic).OrderBy(x => x.DDATE).ToList();
            foreach (var t in lichsu)
            {
                int iUser = (int)t.IUSER;
                if (all_user.Where(x => x.IUSER == iUser).Count() > 0)
                {
                    TaiKhoan tk = tl.Taikhoan_Detail(iUser);
                    string phongban_donvi = "";
                    if (tk.phongban != null) { phongban_donvi = tk.phongban + ", "; }
                    if (tk.donvi != null) { phongban_donvi += tk.donvi + "."; }
                    str += "<tr><td class='tcenter'>" + String.Format("{0:hh:mm dd/MM/yyyy}", (DateTime)t.DDATE) +
                        "</td><td><strong>" + EncodeOutput(tk.ten) + "</strong></br>" + EncodeOutput(phongban_donvi) + 
                        "</td><td>" + EncodeOutput(t.CACTION) + "</td></tr>";
                }
            }
            return str;
        }
        public string KN_Tonghop_Lichsu(int id_tonghop)
        {
            TrackingRepository track = new TrackingRepository();
            var all_user = _kn.GetAll_Users();
            string str = "";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ITONGHOP", id_tonghop);
            dic.Add("IKIENNGHI", 0);
            var lichsu = track.GetAll(dic).OrderBy(x => x.DDATE).ToList();
            foreach (var t in lichsu)
            {
                int iUser = (int)t.IUSER;
                if (all_user.Where(x => x.IUSER == iUser).Count() > 0)
                {
                    TaiKhoan tk = tl.Taikhoan_Detail(iUser);
                    string phongban_donvi = "";
                    if (tk.phongban != null) { phongban_donvi = tk.phongban + ", "; }
                    if (tk.donvi != null) { phongban_donvi += tk.donvi + "."; }
                    str += "<tr><td class='tcenter'>" + String.Format("{0:hh:mm dd/MM/yyyy}", (DateTime)t.DDATE) +
                        "</td><td><strong>" + EncodeOutput(tk.ten) + "</strong></br>" + EncodeOutput(phongban_donvi) + 
                        "</td><td>" + EncodeOutput(t.CACTION) + "</td></tr>";
                }
            }
            return str;
        }
        public string KN_Moicapnhat_Tracuu(List<PRC_KIENNGHI_MOICAPNHAT> kn,TaikhoanAtion act,List<ID_Session_KienNghi_ChonTongHop> list_id_kiennghi_tonghop)
        {
            string str = "";
            if (kn.Count() == 0)
            {
                return "<tr><td class='tcenter alert-danger' colspan='5'>Không tìm thấy kiến nghị nào!</td></tr>";
            }
            string url_cookie = func.Get_Url_keycookie();
            List<decimal> id_donvithamquyen = new List<decimal>();
            int count_kiennghi = 1;int count_linhvuc = 1;
            decimal id_linhvuc = -1;
            decimal id_thamquyen = 0; decimal id_kiennghi = 0;
            foreach (var j in kn)
            {
                if (j.ID_THAMQUYEN_DONVI != id_thamquyen)
                {
                    id_linhvuc = -1; count_linhvuc = 1; 
                      str += "<tr><td colspan='5' class='b'>" + EncodeOutput(j.TEN_THAMQUYEN_DONVI) + "</td></tr>";
                }
                if (j.ID_LINHVUC != id_linhvuc)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (j.ID_LINHVUC != 0) { tenlinhvuc = j.TEN_LINHVUC; }
                    str += "<tr><td colspan='5' class='b'>- - " + count_linhvuc + ". " + EncodeOutput(tenlinhvuc) + "</td></tr>";
                    count_linhvuc++;
                    id_linhvuc = j.ID_LINHVUC;
                }
                if (id_kiennghi != j.ID_KIENNGHI)
                {
                    str += KN_MoiCapNhat_Row(kn, j, act, url_cookie, count_kiennghi, list_id_kiennghi_tonghop);
                    count_kiennghi++;
                    id_kiennghi = j.ID_KIENNGHI;
                }
                id_thamquyen = j.ID_THAMQUYEN_DONVI;

            }
            return str;
        }
        public string KN_MoiCapNhat_Row(List<PRC_KIENNGHI_MOICAPNHAT> kn,PRC_KIENNGHI_MOICAPNHAT k,TaikhoanAtion act, string url_cookie, int count,List<ID_Session_KienNghi_ChonTongHop> list_id_kiennghi_tonghop)
        {
            string str = "";
            string check_tonghop = "";
            if (list_id_kiennghi_tonghop!=null && list_id_kiennghi_tonghop.Where(x=>x.IKIENNGHI==k.ID_KIENNGHI).Count()>0)
            {
                check_tonghop = " checked ";
            }
            string chon_tonghop_kiennghi = "<input type='checkbox' " + /*check_tonghop + */" onclick=\"ThemVaoTongHop(" + k.ID_KIENNGHI + ")\" />";
            string id_encr = HashUtil.Encode_ID(k.ID_KIENNGHI.ToString(), url_cookie);
            // end
            string edit = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Sua/?id=" + id_encr + "\" title='Sửa kiến nghị'  class='trans_func'><i class='icon-pencil'></i></a> ";
            string kiemtrung = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiemtrung/?id=" + id_encr + "\" title='Kiểm trùng'  class='trans_func'><i class='icon-search'></i></a> ";

            //string del = " <a href=\"javascript:void()\" title='Xóa' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr +
            //                "','/Kiennghi/Ajax_Kiennghi_del')\" class='trans_func'><i class='icon-trash'></i></a> ";
            string del = " <a href=\"javascript:void()\" title='Xóa' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kiennghi/Ajax_Xoatam','Bạn có muốn xóa kiến nghị này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
            string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
            string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' class='trans_func'><i class='icon-time'></i></a>";
            string tra_kiennghi = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra')\" href=\"javascript:void(0)\" title='Trả lại kiến nghị' class='trans_func'><i class='icon-circle-arrow-left'></i></a> ";
            tra_kiennghi = "";
            //KN_CL kn = KienNghi_Detail((int)k.IKIENNGHI, id_encr);
            if (k.ID_USER_CAPNHAT != act.iUser && !act.is_admin && !base_bussiness.Action_(50, act))
            {
                del = ""; tra_kiennghi = "";
            }
            if (!base_bussiness.Action_(4, act)) { kiemtrung = ""; }
            string trung = "";
            if (k.ID_KIENNGHI_TRUNG != 0)
            {
                //chon_tonghop_kiennghi = ""; edit = "";
                string id_trung = HashUtil.Encode_ID(k.ID_KIENNGHI_TRUNG.ToString(), url_cookie);
                trung = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_trung + "' class='f-orangered'>[ <i class='icon-paste'></i> Kiến nghị trùng]</a>";
            }            
            string noidung_kn = func.TomTatNoiDung(EncodeOutput(k.NOIDUNG_KIENNGHI), id_encr);
            if (k.ID_GOP == -1)
            {
                noidung_kn = "<a class='gachchan' href='/Kiennghi/Kiennghi_gop_list/?id=" + id_encr+"' title='Nhấn vào để xem danh sách các kiến nghị đã gộp'>"+ EncodeOutput(k.NOIDUNG_KIENNGHI) + "</a>";
                trung = ""; kiemtrung = "";
            }
            string donvitiepnhan = k.TEN_DONVITIEPNHAN;
            if (k.ID_GOP == -1)
            {
                donvitiepnhan = k.TENDONVITIEPNHAN_GOP;
            }            
            str += "<tr><td class='tcenter'>"+count+"</td><td class='tcenter b'>" + chon_tonghop_kiennghi + "</td><td class='td-wordwrap'>" + noidung_kn + trung +
                "</td><td class='tcenter'>"+ EncodeOutput(donvitiepnhan) + "</td><td class='tcenter' nowrap>" + bt_lichsu + info + tra_kiennghi + kiemtrung + edit + del + "</td></tr>";
            
            return str;
        }
        public string KN_Tamxoa_Tracuu(List<PRC_KIENNGHI_TAMXOA> kn, TaikhoanAtion act, List<ID_Session_KienNghi_ChonTongHop> list_id_kiennghi_tonghop)
        {
            string str = "";
            if (kn.Count() == 0)
            {
                return "<tr><td class='tcenter alert-danger' colspan='5'>Không tìm thấy kiến nghị nào!</td></tr>";
            }
            string url_cookie = func.Get_Url_keycookie();
            List<decimal> id_donvithamquyen = new List<decimal>();
            int count_kiennghi = 1; int count_linhvuc = 1;
            decimal id_linhvuc = -1;
            decimal id_thamquyen = 0; decimal id_kiennghi = 0;
            foreach (var j in kn)
            {
                if (j.ID_THAMQUYEN_DONVI != id_thamquyen)
                {
                    id_linhvuc = -1; count_linhvuc = 1;
                    str += "<tr><td colspan='5' class='b'>" + EncodeOutput(j.TEN_THAMQUYEN_DONVI) + "</td></tr>";
                }
                if (j.ID_LINHVUC != id_linhvuc)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (j.ID_LINHVUC != 0) { tenlinhvuc = j.TEN_LINHVUC; }
                    str += "<tr><td colspan='5' class='b'>- - " + count_linhvuc + ". " + EncodeOutput(tenlinhvuc) + "</td></tr>";
                    count_linhvuc++;
                    id_linhvuc = j.ID_LINHVUC;
                }
                if (id_kiennghi != j.ID_KIENNGHI)
                {
                    str += KN_Tamxoa_Row(kn, j, act, url_cookie, count_kiennghi, list_id_kiennghi_tonghop);
                    count_kiennghi++;
                    id_kiennghi = j.ID_KIENNGHI;
                }
                id_thamquyen = j.ID_THAMQUYEN_DONVI;

            }
            return str;
        }
        public string KN_Tamxoa_Row(List<PRC_KIENNGHI_TAMXOA> kn, PRC_KIENNGHI_TAMXOA k, TaikhoanAtion act, string url_cookie, int count, List<ID_Session_KienNghi_ChonTongHop> list_id_kiennghi_tonghop)
        {
            string str = "";
            string check_tonghop = "";
            if (list_id_kiennghi_tonghop != null && list_id_kiennghi_tonghop.Where(x => x.IKIENNGHI == k.ID_KIENNGHI).Count() > 0)
            {
                check_tonghop = " checked ";
            }
            string chon_tonghop_kiennghi = "<input type='checkbox' " + /*check_tonghop + */" onclick=\"ThemVaoTongHop(" + k.ID_KIENNGHI + ")\" />";
            string id_encr = HashUtil.Encode_ID(k.ID_KIENNGHI.ToString(), url_cookie);
            // end
            string edit = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Sua/?id=" + id_encr + "\" title='Sửa kiến nghị'  class='trans_func'><i class='icon-pencil'></i></a> ";
            string kiemtrung = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiemtrung/?id=" + id_encr + "\" title='Kiểm trùng'  class='trans_func'><i class='icon-search'></i></a> ";

            //string del = " <a href=\"javascript:void()\" title='Xóa' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr +
            //                "','/Kiennghi/Ajax_Kiennghi_del')\" class='trans_func'><i class='icon-trash'></i></a> ";
            string phuchoi = " <a href=\"javascript:void()\" title='Phục hồi kiến nghị'  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Phuchoi','Bạn có muốn phục hồi lại kiến nghị này?')\" class='trans_func'><i class='icon-signout'></i></a> ";
            string del = " <a href=\"javascript:void()\" title='Xóa' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kiennghi/Ajax_Kiennghi_del','Bạn có muốn xóa kiến nghị này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
            string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
            string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' class='trans_func'><i class='icon-time'></i></a>";
            string tra_kiennghi = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra')\" href=\"javascript:void(0)\" title='Trả lại kiến nghị' class='trans_func'><i class='icon-circle-arrow-left'></i></a> ";
            tra_kiennghi = "";
            //KN_CL kn = KienNghi_Detail((int)k.IKIENNGHI, id_encr);
            if (k.ID_USER_CAPNHAT != act.iUser && !act.is_admin && !base_bussiness.Action_(50, act))
            {
                del = ""; tra_kiennghi = "";
            }
            if (!base_bussiness.Action_(4, act)) { kiemtrung = ""; }
            string trung = "";
            if (k.ID_KIENNGHI_TRUNG != 0)
            {
                //chon_tonghop_kiennghi = ""; edit = "";
                string id_trung = HashUtil.Encode_ID(k.ID_KIENNGHI_TRUNG.ToString(), url_cookie);
                trung = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_trung + "' class='f-orangered'>[ <i class='icon-paste'></i> Kiến nghị trùng]</a>";
            }
            string noidung_kn = func.TomTatNoiDung(EncodeOutput(k.NOIDUNG_KIENNGHI), id_encr);
            if (k.ID_GOP == -1)
            {
                noidung_kn = "<a class='gachchan' href='/Kiennghi/Kiennghi_gop_list/?id=" + id_encr + "' title='Nhấn vào để xem danh sách các kiến nghị đã gộp'>" + EncodeOutput(k.NOIDUNG_KIENNGHI) + "</a>";
                trung = ""; kiemtrung = "";
            }
            string donvitiepnhan = k.TEN_DONVITIEPNHAN;
            if (k.ID_GOP == -1)
            {
                donvitiepnhan = k.TENDONVITIEPNHAN_GOP;
            }
            str += "<tr><td class='tcenter'>" + count + "</td><td class='tcenter b'>" + chon_tonghop_kiennghi + "</td><td class='td-wordwrap'>" + noidung_kn + trung +
                "</td><td class='tcenter'>" + EncodeOutput(donvitiepnhan) + "</td><td class='tcenter' nowrap>" + bt_lichsu + info + tra_kiennghi + kiemtrung + edit + phuchoi + del + "</td></tr>";

            return str;
        }
        public string KN_Moicapnhat(List<KN_KIENNGHI> kiennghi, List<QUOCHOI_COQUAN> coquan, TaikhoanAtion act)
        {
            string str = "";
            if (kiennghi.Count() == 0)
            {
                return "<tr><td class='tcenter alert-danger' colspan='4'>Không tìm thấy kiến nghị nào!</td></tr>";
            }
            string url_cookie = func.Get_Url_keycookie();
            foreach (var c in coquan)
            {
                var kiennghi1 = kiennghi.Where(x => x.IDONVITIEPNHAN == (int)c.ICOQUAN).OrderBy(x => x.DDATE).ToList();
                if (kiennghi1.Count() > 0)
                {
                    str += "<tr><th colspan='4'>" + c.CTEN.ToUpper() + "</th></tr>";
                    int count = 1;
                    foreach (var k in kiennghi1)
                    {
                        // new id 
                        string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_cookie);
                        // end
                        string edit = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Sua/?id=" + id_encr + "\" title='Sửa kiến nghị'  class='trans_func'><i class='icon-pencil'></i></a> ";
                        string kiemtrung = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiemtrung/?id=" + id_encr + "\" title='Kiểm trùng'  class='trans_func'><i class='icon-search'></i></a> ";
                        
                        //string del = " <a href=\"javascript:void()\" title='Xóa' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr +
                        //                "','/Kiennghi/Ajax_Kiennghi_del')\" class='trans_func'><i class='icon-trash'></i></a> ";
                        string del = " <a href=\"javascript:void()\" title='Xóa' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                                        "','/Kiennghi/Ajax_Xoatam','Bạn có muốn xóa kiến nghị này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                        string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
                        string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' class='trans_func'><i class='icon-time'></i></a>";
                        string tra_kiennghi = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra')\" href=\"javascript:void(0)\" title='Trả lại kiến nghị' class='trans_func'><i class='icon-circle-arrow-left'></i></a> ";

                        //KN_CL kn = KienNghi_Detail((int)k.IKIENNGHI, id_encr);
                        if (k.IUSER != act.iUser && !act.is_admin)
                        {
                            edit = ""; del = ""; tra_kiennghi = "";
                        }
                        if (!base_bussiness.Action_(4, act)) { kiemtrung = ""; }
                        string trung = "";
                        if (k.IKIENNGHI_TRUNG != 0)
                        {
                            string id_trung = HashUtil.Encode_ID(k.IKIENNGHI_TRUNG.ToString(), url_cookie);
                            trung = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_trung + "' class='f-orangered'>[ <i class='icon-paste'></i> Kiến nghị trùng]</a>";
                        }
                        string noidung_kn = func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG),id_encr);
                        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>"+ noidung_kn + trung +
                            "</td><td class='tcenter' nowrap>" + bt_lichsu + info + tra_kiennghi + kiemtrung + edit + del + "</td></tr>";
                        count++;
                    }
                }

            }

            return str;
        }
        public KN_KIENNGHI_TRALOI Info_TraLoi_KienNghi(int id_kiennghi, decimal tinhtrang)
        {
            KN_KIENNGHI_TRALOI traloi = null;
            Dictionary<string, object> _traloi = new Dictionary<string, object>();
            _traloi.Add("IKIENNGHI", id_kiennghi);
            _traloi.Add("ITINHTRANG", tinhtrang);
            KN_Kiennghi_TraloiRepository t = new KN_Kiennghi_TraloiRepository();
            var ketqua = t.GetAll(_traloi);
            if (ketqua.Count() > 0)
            {
                traloi = ketqua.FirstOrDefault();
            }
            return traloi;
        }
        public string KN_Kiennghi_TongHop_ChuyenBDN(List<KN_KIENNGHI> kiennghi, TaikhoanAtion act, string url_cookie)
        {
            string str = "";
            if (kiennghi.Count() == 0)
            {
                return "<tr><td class='tcenter alert' colspan='5'>Không tìm thấy kiến nghị nào!</td></tr>";
            }
            int count = 1;
            foreach (var k in kiennghi)
            {
                // new id 
                string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_cookie);
                // end
                string kiemtrung = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiemtrung/?id=" + id_encr + "\" title='Kiểm trùng' rel='tooltip' title='' class='trans_func'><i class='icon-search'></i></a> ";
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                string tra_kiennghi = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra')\" href=\"javascript:void(0)\" title='Trả lại kiến nghị' rel='tooltip' title='' class='trans_func'><i class='icon-circle-arrow-left'></i></a> ";
                //KN_CL kn = KienNghi_Detail((int)k.IKIENNGHI, id_encr);
                if (k.IUSER != act.iUser && !act.is_admin)
                {
                    tra_kiennghi = "";
                }
                if (!base_bussiness.Action_(4, act)) { kiemtrung = ""; }
                string tinhtrang = "Chưa Tập hợp";
                if (k.ITONGHOP_BDN != 0) { tinhtrang = "Đã Tập hợp lại"; tra_kiennghi = ""; }
                if (k.ITINHTRANG == (decimal)TrangThaiKienNghi.KienNghiTrung_DongKienNghi && k.IKIENNGHI_TRUNG != 0)
                {
                    string id_trung = HashUtil.Encode_ID(k.IKIENNGHI_TRUNG.ToString(), url_cookie);
                    tinhtrang = "<strong>Kiến nghị trùng</strong></br><a href='/Kiennghi/Kiennghi_info/?id=" + id_trung + "' class='f-orangered'>[ <i class='icon-paste'></i> Kiến nghị trùng]</a>";
                    tra_kiennghi = "";
                }


                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td><p>" + func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG), id_encr) + "</p>" +
                    "</td><td>" + tinhtrang + "</td><td class='tcenter' nowrap>" + bt_lichsu + info + tra_kiennghi + kiemtrung + "</td></tr>";
                count++;
            }
            return str;
        }

        public string List_KienNghi_Chon(string kn_chon)
        {
            string str = ""; int count = 1;
            foreach (var k in kn_chon.Split(','))
            {

                if (k != "")
                {
                    int iKienNghi = Convert.ToInt32(k);
                    KN_KIENNGHI kiennghi = _kn.HienThiThongTinKienNghi(iKienNghi);
                    string del = " <a href=\"javascript:void()\" title='Hủy' rel='tooltip' title='' onclick=\"BoChonKienNghi('" + iKienNghi + "')\" class='trans_func'><i class='icon-remove'></i></a> ";
                    str += "<tr id='tr_" + iKienNghi + "'><td class='tcenter b'>" + count + "</td><td>" + EncodeOutput(kiennghi.CNOIDUNG) + "</td><td class='tcenter' nowrap>" + del + "</td></tr>";
                    count++;
                }
            }
            return str;
        }
        public string KN_Theodoi_luu(List<PRC_KIENNGHI_LIST> kiennghi)
        {
            if (kiennghi == null) return string.Empty;
            string str = "";
            if (kiennghi.Count() == 0)
            {
                return "<tr><td class='tcenter alert-danger' colspan='4'>Không tìm thấy kiến nghị nào!</td></tr>";
            }
            string url_cookie = func.Get_Url_keycookie();
            List<decimal> id_donvithamquyen = new List<decimal>();
            int count_kiennghi = 1;int count_linhvuc = 1;
            decimal id_donvi = 0;decimal id_linhvuc = -1;
            foreach(var j in kiennghi)
            {
                if (id_donvi != j.ID_THAMQUYEN_DONVI)
                {
                    str += "<tr><td colspan='4' class='b'>" + EncodeOutput(j.TEN_THAMQUYEN_DONVI.ToUpper()) + " </td></tr>";
                    id_linhvuc = -1; count_linhvuc = 1;
                }
                if (id_linhvuc != j.ID_LINHVUC)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (j.ID_LINHVUC != 0) { tenlinhvuc = j.TEN_LINHVUC; }
                    str += "<tr><td colspan='4' class='b'>- - " + count_linhvuc + ". " + EncodeOutput(tenlinhvuc) + " </td></tr>";
                    id_linhvuc = j.ID_LINHVUC;
                }
                string id_encr = HashUtil.Encode_ID(j.ID_KIENNGHI.ToString(), url_cookie);
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' class='trans_func'><i class='icon-time'></i></a>";
                string trung = "";
                if (j.ID_KIENNGHI_TRUNG != 0)
                {
                    string id_trung = HashUtil.Encode_ID(j.ID_KIENNGHI_TRUNG.ToString(), url_cookie);
                    trung = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_trung + "' class='f-orangered'>[ <i class='icon-paste'></i> Kiến nghị trùng]</a>";
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count_kiennghi + "</td><td><p>" + func.TomTatNoiDung(EncodeOutput(j.NOIDUNG_KIENNGHI), id_encr) +
                    "</p>" + trung + "</td><td class='tcenter'>" + EncodeOutput(j.TEN_DONVITIEPNHAN) + "</td><td class='tcenter' nowrap>" + bt_lichsu + info + "</td></tr>";
                count_kiennghi++;
                id_donvi = j.ID_THAMQUYEN_DONVI;
            }
            //foreach (var j in kiennghi)
            //{
            //    if (!id_donvithamquyen.Contains(j.ID_THAMQUYEN_DONVI))
            //    {
            //        var kiennghi2 = kiennghi.Where(x => x.ID_THAMQUYEN_DONVI == j.ID_THAMQUYEN_DONVI).ToList();
            //        str += "<tr><td colspan='4' class='b'>" + kiennghi2.FirstOrDefault().TEN_THAMQUYEN_DONVI.ToUpper() + " (" + kiennghi2.Count() + " kiến nghị)</td></tr>";
            //        List<decimal> id_linhvuc = new List<decimal>();
            //        int count_linhvuc = 1;
            //        foreach (var k in kiennghi2.OrderByDescending(x => x.ID_LINHVUC).ToList())
            //        {
            //            if (!id_linhvuc.Contains(k.ID_LINHVUC))
            //            {
            //                var kiennghi3 = kiennghi2.Where(x => x.ID_LINHVUC == k.ID_LINHVUC).ToList();
            //                string tenlinhvuc = "Lĩnh vực: Chưa xác định";
            //                if (k.ID_LINHVUC > 0)
            //                {
            //                    tenlinhvuc = kiennghi3.Where(x => x.ID_LINHVUC == k.ID_LINHVUC).FirstOrDefault().TEN_LINHVUC;
            //                }
            //                str += "<tr><td colspan='4' class='b'>- - " + count_linhvuc + ". " + EncodeOutput(tenlinhvuc) + " (" + kiennghi3.GroupBy(x => x.ID_KIENNGHI).Count() + " kiến nghị)</td></tr>";
            //                count_linhvuc++;
            //                //str += "<tbody class='table-striped'>";
            //                List<decimal> list_id_kiennghi = new List<decimal>();
            //                foreach (var l in kiennghi3)
            //                {
            //                    if (!list_id_kiennghi.Contains(l.ID_KIENNGHI))
            //                    {
            //                        string id_encr = HashUtil.Encode_ID(l.ID_KIENNGHI.ToString(), url_cookie);
            //                        string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
            //                        string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' class='trans_func'><i class='icon-time'></i></a>";
            //                        string trung = "";
            //                        if (l.ID_KIENNGHI_TRUNG != 0)
            //                        {
            //                            string id_trung = HashUtil.Encode_ID(l.ID_KIENNGHI_TRUNG.ToString(), url_cookie);
            //                            trung = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_trung + "' class='f-orangered'>[ <i class='icon-paste'></i> Kiến nghị trùng]</a>";
            //                        }
            //                        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count_kiennghi + "</td><td><p>" + func.TomTatNoiDung(EncodeOutput(l.NOIDUNG_KIENNGHI), id_encr) + 
            //                            "</p>" + trung + "</td><td class='tcenter'>"+ EncodeOutput(l.TEN_DONVITIEPNHAN)+"</td><td class='tcenter' nowrap>" + bt_lichsu + info + "</td></tr>";
            //                        list_id_kiennghi.Add(l.ID_KIENNGHI);
            //                        count_kiennghi++;
            //                    }
            //                }
            //                //str += "</tbody>";

            //                id_linhvuc.Add(k.ID_LINHVUC);
            //            }
            //        }
            //        id_donvithamquyen.Add(j.ID_THAMQUYEN_DONVI);
            //    }
            //}
            return str;
            //foreach (var q in coquan)
            //{
            //    var kiennghi1 = kiennghi.Where(x => x.IDONVITIEPNHAN == (int)q.ICOQUAN).ToList();
            //    int count = 1;
            //    if (kiennghi1.Count() > 0)
            //    {
            //        str += "<tr><th class='' colspan='3'>" + q.CTEN.ToUpper() + " (" + kiennghi1.Count() + ")</th></tr>";
            //        foreach (var k in kiennghi1)
            //        {
            //            string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_cookie);
            //            string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
            //            string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' class='trans_func'><i class='icon-time'></i></a>";
            //            string trung = "";
            //            if (k.IKIENNGHI_TRUNG != 0)
            //            {
            //                string id_trung = HashUtil.Encode_ID(k.IKIENNGHI_TRUNG.ToString(), url_cookie);
            //                trung = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_trung + "' class='f-orangered'>[ <i class='icon-paste'></i> Kiến nghị trùng]</a>";
            //            }
            //            str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td><p>" + func.TomTatNoiDung(k.CNOIDUNG, id_encr) + "</p>" + trung +
            //                "</td><td class='tcenter' nowrap>" + bt_lichsu + info + "</td></tr>";
            //            count++;
            //        }
            //    }
            //}

            //return str;
        }
        public string Content_KN_kiennghi_traloi(KN_KIENNGHI kiennghi)
        {
            string str = "";
            KN_Kiennghi_TraloiRepository _traloi = new KN_Kiennghi_TraloiRepository();
            KN_KIENNGHI_TRALOI traloi = null;
            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add("IKIENNGHI", kiennghi.IKIENNGHI);
            if (_traloi.GetAll(obj).Count() > 0)
            {
                traloi = _traloi.GetAll(obj).FirstOrDefault();
                string file = "";
                if (kiennghi.ID_KIENNGHI_PARENT > 0)
                {
                    Dictionary<string, object> obj_ = new Dictionary<string, object>();
                    obj_.Add("IKIENNGHI", kiennghi.ID_KIENNGHI_PARENT);
                    KN_KIENNGHI_TRALOI traloi_ = _traloi.GetAll(obj_).FirstOrDefault();
                    file = File_View((int)traloi_.ITRALOI, "kn_traloi");
                }
                else
                {
                    file = File_View((int)traloi.ITRALOI, "kn_traloi");
                }
                str = "<p>" + EncodeOutput(traloi.CTRALOI) + " " + File_View((int)traloi.ITRALOI, "kn_traloi") + "</p>";
            }
            return str;
        }
        public string Content_Traloi_Kiennghi_for_detail_by_traloi(int iKienNghi, KN_KIENNGHI_TRALOI traloi)
        {
            string str = "";
            //KN_Kiennghi_TraloiRepository _traloi = new KN_Kiennghi_TraloiRepository();
            //Dictionary<string, object> _dic = new Dictionary<string, object>();
            //_dic.Add("IKIENNGHI", iKienNghi);

            //var traloi_kiennghi = _kn.GetAll_TraLoi_KienNghi_ByParamt(_dic);
            //if (traloi_kiennghi.Count() == 0)
            //{
            //    return "Chưa có trả lời";
            //}
            //KN_KIENNGHI_TRALOI traloi = traloi_kiennghi.FirstOrDefault();
            string file = File_View((int)traloi.ITRALOI, "kn_traloi");
            string phanloai = "";
            List<KN_TRALOI_PHANLOAI> phanloai_traloi = _kn.GetAll_KN_TRALOI_PHANLOAI();
            phanloai_traloi = phanloai_traloi.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).ToList();
            if (phanloai_traloi.Count() > 0)
            {
                phanloai = "<p><strong>Phân loại:</strong> " + EncodeOutput(phanloai_traloi.FirstOrDefault().CTEN) + "</p>";
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.DaCoTraLoi)//1: Đã có trả lời,
            {

                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.DaCoTraLoi) + ":</strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;

            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.TraLaiKienNghi)//2:Trả lại kiến nghị
            {
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.TraLaiKienNghi) + ":</strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuyenGiaiQuyetKyHopSau)//3: Chuyển giải quyết kỳ họp sau
            {
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.ChuyenGiaiQuyetKyHopSau) + ":</strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.CoLoTrinhGiaiQuyet)//Có lộ trình giải quyết
            {
                string ngaydukien = "";
                if (traloi.DNGAY_DUKIEN != null)
                {
                    ngaydukien = "<p><strong>Ngày dự kiến giải quyết dứt điểm: </strong>" + func.ConvertDateVN(traloi.DNGAY_DUKIEN.ToString()) + "</p>";
                }
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.CoLoTrinhGiaiQuyet) + ": </strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + ngaydukien + "</p>" + phanloai;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoLoTrinhGiaiQuyet)//Chưa có lộ trình giải quyết
            {
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.ChuaCoLoTrinhGiaiQuyet) + ": </strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaTheGiaiQuyetNgay)//Chưa thể giải quyết ngay
            {
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.ChuaTheGiaiQuyetNgay) + ": </strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoNguonLucGiaiQuyet)//Chưa có nguồn lực giải quyết
            {
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.ChuaCoNguonLucGiaiQuyet) + ": </strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;
            }
            string danhgia = DanhGia_TraLoi_KienNghi(iKienNghi, (int)traloi.ITRALOI);
            if (danhgia != "")
            {
                str += "<p style='text-decoration: underline;'><strong>Đánh giá kết quả trả lời: </strong>" + DanhGia_TraLoi_KienNghi(iKienNghi, (int)traloi.ITRALOI);
            }
            return str;
        }
        //public string Content_Traloi_Kiennghi_for_detail(int iKienNghi)
        //{
        //    string str = "";
        //    Dictionary<string, object> _dic = new Dictionary<string, object>();
        //    _dic.Add("IKIENNGHI", iKienNghi);
        //    var traloi = _kn.GetAll_TraLoi_KienNghi_ByParamt(_dic);
        //    if (traloi!=null){
        //        int count = 1;
        //        foreach(var t in traloi)
        //        {
        //            str += Content_Traloi_Kiennghi_for_detail_by_traloi(iKienNghi, t);
        //            if(count>1 && count < traloi.Count())
        //            {
        //                str += "<div class='dr'></div>";
        //            }
        //        }
        //    }
        //    return str;
        //}
        public string Content_Traloi_Theodoi_kiennghi_chuyenkysau_first(PRC_KIENNGHI_CHUYENKYSAU l, 
            List<KN_KIENNGHI_TRALOI> traloi, List<KN_GIAMSAT> giamsat,string url_cookie, TaikhoanAtion act)
        {
            KN_KIENNGHI kiennghi = _kn.HienThiThongTinKienNghi((int)l.ID_KIENNGHI);
            string str = "";
            string id_encr = HashUtil.Encode_ID(kiennghi.IKIENNGHI.ToString(), url_cookie);
            traloi = traloi.Where(x => x.IKIENNGHI == (int)kiennghi.IKIENNGHI).OrderBy(x=>x.DDATE).ToList();
            if (traloi.Count() > 0)
            {
                KN_KIENNGHI_TRALOI traloi_kn = traloi.FirstOrDefault();
                str = Info_Traloi_GiamSat_kiennghi_Chuyenkysau(traloi_kn, giamsat, act);
            }
            return str;
        }
        public string Content_Traloi_kiennghi(PRC_LIST_KN_TRALOI_DANHGIA t)
        {
            string str = "";
            if (t.TRALOI_NOIDUNG != null)
            {
                int id_kiennghi = (int)t.ID_KIENNGHI;
                Dictionary<string, object> dic_ = new Dictionary<string, object>();
                dic_.Add("IKIENNGHI", id_kiennghi);
                var traloi_list = _kn.GetAll_TraLoi_KienNghi_ByParamt(dic_);
                string file = "";
                if (traloi_list.Count() > 0)
                {
                    KN_KIENNGHI_TRALOI traloi_ = traloi_list.FirstOrDefault();
                    file = File_View((int)traloi_.ITRALOI, "kn_traloi");
                }
                string trangthai = "Trả lời kiến nghị"; //if (t.TRALOI_TINHTRANG == 2) { trangthai = "Trả lại kiến nghị"; }
                string phanloai = "";
                if (t.PARENT_TRALOI_PHANLOAI != null)
                {
                    phanloai = "<strong>Phân loại:</strong> " + EncodeOutput(t.PARENT_TRALOI_PHANLOAI) + " > " + EncodeOutput(t.TRALOI_PHANLOAI)+"</br>";
                }
                else
                {
                    phanloai = "<strong>Phân loại:</strong> " + EncodeOutput(t.TRALOI_PHANLOAI)+ "</br>";
                }
                string ngaybanhanh = "";
                if (t.TRALOI_NGAYBANHANH_VANBAN != DateTime.MinValue)
                {
                    ngaybanhanh = " ngày " + func.ConvertDateVN(t.TRALOI_NGAYBANHANH_VANBAN.ToString());
                }
                string vanban = "<strong>Văn bản xử lý: </strong>Công văn số " + EncodeOutput(t.TRALOI_SOVANBAN)+ngaybanhanh+" "+ file + "</br>";
                string coquantraloi = "";
                if (t.COQUANTRALOI != null)
                {
                    coquantraloi="<strong>Cơ quan trả lời: </strong> " + EncodeOutput(t.COQUANTRALOI) + "</br>";
                }
                string traloi = "<strong>Kết quả xử lý: </strong>" + EncodeOutput(t.TRALOI_NOIDUNG);
                str = "<div class='alert alert-success kn_traloi'><p class='b'>" + trangthai + "</p>"+ 
                            phanloai+vanban+ coquantraloi + traloi+"</div>";
            }            
            return str;
        }
        public string Content_GiamSat_kiennghi(PRC_LIST_KN_TRALOI_DANHGIA t)
        {
            string str = "";
            if (t.GIAMSAT_PHANLOAI != null)
            {
                string phanloai = "";
                if (t.PARENT_GIAMSAT_PHANLOAI != null)
                {
                    phanloai = "<strong>Phân loại:</strong> " + EncodeOutput(t.PARENT_GIAMSAT_PHANLOAI) + " > " + EncodeOutput(t.GIAMSAT_PHANLOAI) + "</br>";
                }
                else
                {
                    phanloai = "<strong>Phân loại:</strong> " + EncodeOutput(t.TRALOI_PHANLOAI) + "</br>";
                }
                string denghi = "Đóng kiến nghị.";
                if (t.GIAMSAT_DONGKIENNGHI == 0)
                {
                    denghi = "Chuyển theo dõi kỳ họp sau.";
                }
                denghi = "<strong>Đề nghị:</strong> " + denghi+"</br>";
                string dungtiendo = "Đúng tiến độ.";
                if (t.GIAMSAT_DUNGTIENDO == 0)
                {
                    dungtiendo = "Chậm tiến độ.";
                }
                dungtiendo = "<strong>Tiến độ:</strong> " + dungtiendo;
                str = "<div class='alert alert-info kn_traloi'><p><strong>Kết quả đánh giá: </strong></p>" + phanloai + denghi + dungtiendo+"</div>";
            }
            return str;
        }
        public string Content_Traloi_kiennghi_Edit(PRC_LIST_TONGHOP_KIENNGHI t, TaikhoanAtion act,string url_key)
        {
            string str = "";
            if (t.ID_TRALOI != 0)
            {
                int id_kiennghi = (int)t.IKIENNGHI;
                Dictionary<string, object> dic_ = new Dictionary<string, object>();
                dic_.Add("IKIENNGHI", id_kiennghi);
                var traloi_list = _kn.GetAll_TraLoi_KienNghi_ByParamt(dic_);
                string file = "";
                if (traloi_list.Count() > 0)
                {
                    KN_KIENNGHI_TRALOI traloi_ = traloi_list.FirstOrDefault();
                    file = File_View((int)traloi_.ITRALOI, "kn_traloi");
                }
                string trangthai = "Trả lời kiến nghị"; //if (t.TRALOI_TINHTRANG == 2) { trangthai = "Trả lại kiến nghị"; }
                string phanloai = "";
                if (t.PARENT_TRALOI_PHANLOAI != null)
                {
                    phanloai = "<strong>Phân loại:</strong> " + EncodeOutput(t.PARENT_TRALOI_PHANLOAI) + " > " + EncodeOutput(t.TRALOI_PHANLOAI) + "</br>";
                }
                else
                {
                    phanloai = "<strong>Phân loại:</strong> " + EncodeOutput(t.TRALOI_PHANLOAI) + "</br>";
                }
                string ngaybanhanh = "";
                if (t.TRALOI_NGAYBANHANH_VANBAN != DateTime.MinValue)
                {
                    ngaybanhanh = " ngày " + func.ConvertDateVN(t.TRALOI_NGAYBANHANH_VANBAN.ToString());
                }
                string vanban = "<strong>Văn bản xử lý: </strong>Công văn số " + EncodeOutput(t.TRALOI_SOVANBAN) + ngaybanhanh + " " + file + "</br>";
                string coquantraloi = "";
                if (t.COQUANTRALOI != null)
                {
                    coquantraloi= "<strong>Cơ quan trả lời: </strong> "+ EncodeOutput(t.COQUANTRALOI)+"</br>";
                }
                string traloi = "<strong>Kết quả xử lý: </strong>" + EncodeOutput(t.TRALOI_NOIDUNG);
                if (act == null)
                {
                    str = "<div class='alert alert-success kn_traloi'><p class='b'>" + trangthai + "</p>" + phanloai + vanban + traloi + "</div>";
                    return str;
                }
                string id_encr_traloi = HashUtil.Encode_ID(t.ID_TRALOI.ToString(), url_key);
                string traloi_edit = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr_traloi + "','/Kiennghi/Ajax_Kiennghi_traloi_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string traloi_del = " <a onclick=\"DeletePage_Confirm_Traloi('" + id_encr_traloi + "','id=" + id_encr_traloi +
                            "','/Kiennghi/Ajax_Kiennghi_traloi_del','Bạn có muốn xóa trả lời kiến nghị này hay không?')\" href =\"javascript:void()\" title='Hủy' rel='tooltip' title=''><i class='icon-trash'></i></a>";
                if (t.ID_TRALOI_USER != act.iUser && !base_bussiness.Action_(50, act) && t.ID_GIAMSAT!=0) { traloi_edit = ""; traloi_del = ""; }
                if (t.ID_GIAMSAT != 0) { traloi_edit = ""; traloi_del = ""; }
                str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_edit + traloi_del + 
                    "</div><p class='b'>" + trangthai + "</p>" + phanloai + vanban+ coquantraloi + traloi + "</div>";
                if (act.is_lanhdao)
                {
                    if (t.ID_GIAMSAT == 0)
                    {
                        string giamsat_add = "<a href =\"javascript:void()\" title='Đánh giá trả lời' rel='tooltip' title='' onclick=\"ShowPopUp('id=" +
                        id_encr_traloi + "','/Kiennghi/Ajax_Kiennghi_danhgia')\" class='btn btn-primary'><i class='icon-edit'></i> Đánh giá trả lời</a> ";
                        if (!base_bussiness.ActionMulty_("8,50", act))
                        {
                            giamsat_add = "";
                        }
                        str += giamsat_add;
                    }
                    else
                    {
                        str += Content_GiamSat_kiennghi_Edit(t, act, url_key);
                    }
                }
            }
            return str;
        }

        public string Content_GiamSat_kiennghi_Edit(PRC_LIST_TONGHOP_KIENNGHI t, TaikhoanAtion act, string url_key)
        {
            string str = "";
            if (t.GIAMSAT_PHANLOAI != null)
            {
                string phanloai = "";
                if (t.PARENT_GIAMSAT_PHANLOAI != null)
                {
                    phanloai = "<strong>Phân loại:</strong> " + EncodeOutput(t.PARENT_GIAMSAT_PHANLOAI) + " > " + EncodeOutput(t.GIAMSAT_PHANLOAI) + "</br>";
                }
                else
                {
                    phanloai = "<strong>Phân loại:</strong> " + EncodeOutput(t.GIAMSAT_PHANLOAI) + "</br>";
                }
                string denghi = "Đóng kiến nghị.";
                if (t.GIAMSAT_DONGKIENNGHI == 0)
                {
                    denghi = "Chuyển theo dõi kỳ họp sau.";
                }
                denghi = "<strong>Đề nghị:</strong> " + denghi + "</br>";
                string dungtiendo = "Đúng tiến độ.";
                if (t.GIAMSAT_DUNGTIENDO == 0)
                {
                    dungtiendo = "Chậm tiến độ.";
                }
                dungtiendo = "<strong>Tiến độ:</strong> " + dungtiendo;
                string id_ecrt_danhgia = HashUtil.Encode_ID(t.ID_GIAMSAT.ToString(), url_key);
                string giamsat_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_ecrt_danhgia + "','/Kiennghi/Ajax_Kiennghi_danhgia_edit')\" class='trans_func'><i class='icon-edit'></i></a> ";
                string giamsat_del = " <a href =\"javascript:void()\" title='Hủy' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_ecrt_danhgia + "','id=" + id_ecrt_danhgia +
                            "','/Kiennghi/Ajax_Kiennghi_danhgia_del','Bạn có muốn xóa đánh giá kiến nghị này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                if (t.ID_GIAMSAT_USER != act.iUser && !base_bussiness.Action_(50, act) && !act.is_admin) { giamsat_add = ""; giamsat_del = ""; }
                str = "<div class='alert alert-info kn_traloi'><div class='alert-icon'>" + giamsat_add + giamsat_del + 
                    "</div><p><strong>Kết quả đánh giá: </strong></p>" + phanloai + denghi + dungtiendo + "</div>";
            }
            return str;
        }
        public string KN_Theodoi_kiennghi_chuyenkysau(List<PRC_LIST_KN_TRALOI_DANHGIA> kiennghi, 
            TaikhoanAtion act)
        {
            string str = "";
            if (kiennghi.Count() == 0)
            {
                return "<tr><td class='tcenter alert-danger' colspan='4'>Không tìm thấy kiến nghị nào!</td></tr>";
            }
            string url_cookie = func.Get_Url_keycookie();
            List<decimal> id_donvithamquyen = new List<decimal>();
            int count = 1;
            var kiennghi1 = kiennghi.Where(x => x.ID_GOP <= 0).ToList();
            decimal id_donvi = 0;decimal id_linhvuc = -1;int count_linhvuc = 1;
            foreach (var j in kiennghi1)
            {
                if (j.ID_THAMQUYENDONVI != id_donvi && !act.is_chuyenvien)
                {
                    str += "<tr><th colspan='4' class='b'>";
                    if (j.TEN_THAMQUYENDONVI != null)
                        str += EncodeOutput(j.TEN_THAMQUYENDONVI.ToUpper());
                    str += "</th></tr>";
                    count_linhvuc = 1; id_linhvuc = -1;
                }
                if (j.ID_LINHVUC_KIENNGHI != id_linhvuc)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (j.ID_LINHVUC_KIENNGHI != 0) { tenlinhvuc = j.TEN_LINHVUC_KIENNGHI; }
                    if (!act.is_chuyenvien)
                    {
                        str += "<tr><td colspan='4' class='b'>- - " + count_linhvuc + ". " + EncodeOutput(tenlinhvuc) + " </td></tr>";
                    }
                    else
                    {
                        str += "<tr><td colspan='4' class='b'>" + EncodeOutput(tenlinhvuc) + " </td></tr>";
                    }
                    count_linhvuc++;
                }
                string id_encr = HashUtil.Encode_ID(j.ID_KIENNGHI.ToString(), url_cookie);
                string traloi_chuyenkysau = Content_Traloi_kiennghi(j);
                string giamsat = "";
                if (act.is_lanhdao) { giamsat = Content_GiamSat_kiennghi(j); }
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                string noidung_kiennghi = "<p>" + func.TomTatNoiDung(EncodeOutput(j.NOIDUNG_KIENNGHI), id_encr) + "</p>";
                string donvitiepnhan = j.TEN_DONVITIEPNHAN;
                if (j.ID_KIENNGHI < 0)
                {
                    donvitiepnhan = j.TENDONVITIEPNHAN_GOP;
                }                
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + noidung_kiennghi + "</br>" +
                    "<strong>Tiếp nhận: " + EncodeOutput(donvitiepnhan) + "</strong></td><td>" +
                    traloi_chuyenkysau + giamsat + "</td><td class='tcenter' nowrap>" + bt_lichsu + info + "</td></tr>";
                count++;
                id_donvi = j.ID_THAMQUYENDONVI;
                id_linhvuc = j.ID_LINHVUC_KIENNGHI;
                //if (!id_donvithamquyen.Contains(j.ID_THAMQUYENDONVI))
                //{
                //    var kiennghi2 = kiennghi1.Where(x => x.ID_THAMQUYENDONVI == j.ID_THAMQUYENDONVI).ToList();
                //    if (!act.is_chuyenvien)
                //    {
                //        str += "<tr><th colspan='4' class='b'>" + EncodeOutput(kiennghi2.FirstOrDefault().TEN_THAMQUYENDONVI.ToUpper()) + "</th></tr>";
                //    }
                //    List<decimal> id_linhvuc = new List<decimal>();
                //    int count_linhvuc = 1;
                //    foreach (var k in kiennghi2.OrderByDescending(x => x.ID_LINHVUC_KIENNGHI).ToList())
                //    {
                //        if (!id_linhvuc.Contains(k.ID_LINHVUC_KIENNGHI))
                //        {
                //            var kiennghi3 = kiennghi2.Where(x => x.ID_LINHVUC_KIENNGHI == k.ID_LINHVUC_KIENNGHI).ToList();
                //            string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                //            if (k.ID_LINHVUC_KIENNGHI > 0)
                //            {
                //                tenlinhvuc = kiennghi3.FirstOrDefault().TEN_LINHVUC_KIENNGHI;
                //            }
                //            if (!act.is_chuyenvien)
                //            {
                //                str += "<tr><td colspan='4' class='b'>- - " + count_linhvuc + ". " + EncodeOutput(tenlinhvuc) + " </td></tr>";
                //            }
                //            else
                //            {
                //                str += "<tr><td colspan='4' class='b'>" + EncodeOutput(tenlinhvuc) + " </td></tr>";
                //            }

                //            count_linhvuc++;
                //            //str += "<tbody class='table-striped'>";
                //            List<decimal> list_id_kiennghi = new List<decimal>();

                //            foreach (var l in kiennghi3)
                //            {
                //                if (!list_id_kiennghi.Contains(l.ID_KIENNGHI))
                //                {
                //                    list_id_kiennghi.Add(l.ID_KIENNGHI);
                //                    string id_encr = HashUtil.Encode_ID(l.ID_KIENNGHI.ToString(), url_cookie);                                                                        
                //                    string traloi_chuyenkysau = Content_Traloi_kiennghi(l);
                //                    string giamsat = "";
                //                    if (act.is_lanhdao) { giamsat = Content_GiamSat_kiennghi(l); }
                //                    string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
                //                    string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                //                    string noidung_kiennghi = "<p>" + func.TomTatNoiDung(EncodeOutput(l.NOIDUNG_KIENNGHI), id_encr) + "</p>";
                //                    List<string> list_donvi_gop = new List<string>();
                //                    string donvitiepnhan = "";
                //                    string kiennghi_gop = "";
                //                    int count_donvi_gop = 0;
                //                    var kn_gop = kiennghi.Where(x => x.ID_KIENNGHI_CHILD_GOP == l.ID_KIENNGHI).ToList();
                //                    if (kn_gop.Count() > 0)
                //                    {
                //                        foreach (var d in kn_gop)
                //                        {
                //                            if (!list_donvi_gop.Contains(d.TENDONVITIEPNHAN_GOP))
                //                            {
                //                                if (count_donvi_gop > 0) { donvitiepnhan += ", "; }
                //                                donvitiepnhan += d.TENDONVITIEPNHAN_GOP;
                //                                list_donvi_gop.Add(d.TENDONVITIEPNHAN_GOP);
                //                                count_donvi_gop++;
                //                            }
                //                        }
                //                        kiennghi_gop = "<em class='f-orangered'>[Kiến nghị gộp]</em>";
                //                    }
                //                    else
                //                    {
                //                        donvitiepnhan = l.TEN_DONVITIEPNHAN;
                //                    }
                //                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + noidung_kiennghi + "</br>"+
                //                        "<strong>Tiếp nhận: "+ EncodeOutput(donvitiepnhan) + "</strong> "+ kiennghi_gop + "</td><td>" +
                //                        traloi_chuyenkysau + giamsat+"</td><td class='tcenter' nowrap>" + bt_lichsu + info + "</td></tr>";
                //                    count++;
                //                }
                //            }
                //            //str += "</tbody>";
                //            id_linhvuc.Add(k.ID_LINHVUC_KIENNGHI);
                //        }
                //    }
                //    id_donvithamquyen.Add(j.ID_THAMQUYENDONVI);
                //}
            }
            return str;

            //coquan = coquan.OrderBy(x => x.IVITRI).ToList();
            //string url_cookie = func.Get_Url_keycookie();
            //foreach (var q in coquan)
            //{
            //    var kiennghi1 = kiennghi.Where(x => x.ITHAMQUYENDONVI == (int)q.ICOQUAN).ToList();
            //    int count = 1;
            //    if (kiennghi1.Count() > 0)
            //    {
            //        str += "<tr><th class='' colspan='4'>" + q.CTEN.ToUpper() + " (" + kiennghi1.Count() + ")</th></tr>";
            //        foreach (var k in kiennghi1)
            //        {
            //            string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_cookie);
            //            string traloi_chuyenkysau = Content_Traloi_Theodoi_kiennghi_chuyenkysau_first(k, traloi, giamsat, url_cookie,act);
                        
            //            string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
            //            string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
            //            string giamsat_add_ = " ";
            //            if (traloi.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).Count() > 1)
            //            {   KN_KIENNGHI_TRALOI kn_traloi = traloi.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).OrderByDescending(x=>x.DDATE).FirstOrDefault();
            //                giamsat_add_ = "<a href =\"javascript:void()\" title='Đánh giá trả lời' rel='tooltip' title='' onclick=\"ShowPopUp('id=" +
            //                HashUtil.Encode_ID(kn_traloi.ITRALOI.ToString(), url_cookie) + "','/Kiennghi/Ajax_Kiennghi_danhgia')\" class='trans_func'><i class='icon-edit'></i></a>";
            //                if (giamsat.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI && x.ITRALOI == (int)kn_traloi.ITRALOI).Count() > 0)
            //                {
            //                    giamsat_add_ = "";
            //                }
            //            }
            //            else {
            //                giamsat_add_ = "";
            //            }
            //            string doan_tiepnhan = str_donvi_tiepnhan((int)k.IDONVITIEPNHAN, coquan);
            //            if (!base_bussiness.Action_(8, act)) { giamsat_add_ = ""; }
            //            string noidung_kiennghi = "<p>" + func.TomTatNoiDung(k.CNOIDUNG, id_encr) + "</p>" + doan_tiepnhan;

            //            str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + noidung_kiennghi + "</td><td>" +
            //                traloi_chuyenkysau + "</td><td class='tcenter' nowrap>" + giamsat_add_ + bt_lichsu + info + "</td></tr>";
            //            count++;
            //        }
            //    }
            //}
            //return str;
        }
        public string KN_Delete(List<PRC_KIENNGHI_DELETE> kiennghi)
        {
            string str = "";
            if (kiennghi.Count() == 0)
            {
                return "<tr><td class='tcenter alert-danger' colspan='5'>Không tìm thấy kiến nghị nào đã tạm xóa!</td></tr>";
            }
            string url_cookie = func.Get_Url_keycookie();
            int count_kiennghi = 1;
            foreach (var l in kiennghi)
            {
                string id_encr = HashUtil.Encode_ID(l.ID_KIENNGHI.ToString(), url_cookie);
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                string noidung_kiennghi = "<p>" + func.TomTatNoiDung(EncodeOutput(l.NOIDUNG_KIENNGHI), id_encr) + "</p>";
               
                string ten_donvi_gop = l.TEN_DONVI_TIEPNHAN;
                
                string phuchoi = " <a href=\"javascript:void()\" title='Phục hồi kiến nghị'  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Phuchoi','Bạn có muốn phục hồi lại kiến nghị này?')\" class='trans_func'><i class='icon-refresh'></i></a> ";
                string del = " <a href=\"javascript:void()\" title='Xóa'  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_xoa','Bạn có muốn xóa hẳn kiến nghị này?')\" class='trans_func'><i class='icon-trash'></i></a> ";

                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count_kiennghi + "</td><td>" + noidung_kiennghi + "</td><td class='tcenter'>" +
                    EncodeOutput(ten_donvi_gop) + "</td><td>" + EncodeOutput(l.TEN_DONVI_THAMQUYEN) + "</td><td class='tcenter' nowrap>" + bt_lichsu + info + phuchoi+ del + "</td></tr>";
                count_kiennghi++;
            }
            return str;
        }
        public string KN_Tracuu(List<PRC_KIENNGHI_LIST_TRACUU> kiennghi,UserInfor u)
        {
            string str = "";
            if (kiennghi.Count() == 0)
            {
                return "<tr><td class='tcenter alert-danger' colspan='5'>Không tìm thấy kiến nghị nào!</td></tr>";
            }
            string url_cookie = func.Get_Url_keycookie();
            int count_kiennghi = 1;
            foreach(var l in kiennghi)
            {
                string id_encr = HashUtil.Encode_ID(l.ID_KIENNGHI.ToString(), url_cookie);
                string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                string noidung_kiennghi = "<p>" + func.TomTatNoiDung(EncodeOutput(l.NOIDUNG_KIENNGHI), id_encr) + "</p>";
                string linhvuc = "";
                if (l.ID_LINHVUC != 0) { linhvuc = " > " + l.TEN_LINHVUC; }

                string ten_donvi_gop = l.TEN_DONVITIEPNHAN;
                if (l.ID_GOP == -1)
                {
                    ten_donvi_gop = l.TENDONVITIEPNHAN_GOP;
                }
                string str_xoa = "";
                string del = " <a href=\"javascript:void()\" title='Xóa'  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Xoatam','Bạn có muốn tạm xóa kiến nghị này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                if (l.IDELETE == 1)
                {
                    str_xoa = "</br><span class='f-orangered'>[Kiến nghị đã tạm xóa]</span>";
                    del = " <a href=\"javascript:void()\" title='Phục hồi kiến nghị'  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Phuchoi','Bạn có muốn phục hồi lại kiến nghị này?')\" class='trans_func'><i class='icon-refresh'></i></a> ";
                }
                if (!u.tk_action.is_admin && !_base.Action(50,(int)u.user_login.IUSER) && (int)u.user_login.IUSER != l.IUSER){
                    str_xoa = "";
                    del = "";
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count_kiennghi + "</td><td>" + noidung_kiennghi + str_xoa+"</td><td class='tcenter'>" +
                    EncodeOutput(ten_donvi_gop) + "</td><td>" + EncodeOutput(l.TEN_THAMQUYEN_DONVI) + EncodeOutput(linhvuc) + "</td><td class='tcenter' nowrap>" + bt_lichsu + info + del+ "</td></tr>";
                count_kiennghi++;
            }
            //var kiennghi3 = kiennghi.Where(x => x.ID_GOP <= 0).OrderBy(x=>x.ID_GOP).ToList();
            //if (iTinhtrang == 4)//đang xử lý
            //{
            //    kiennghi3=kiennghi3.Where(x=>x.TRALOI_PHANLOAI==null).toli
            //}
            //if (iTinhtrang == 5)//đã trả lời
            //{

            //}
            //foreach (var l in kiennghi3)
            //{
            //    if (!list_id_kiennghi.Contains(l.ID_KIENNGHI))
            //    {
            //        list_id_kiennghi.Add(l.ID_KIENNGHI);
            //        string id_encr = HashUtil.Encode_ID(l.ID_KIENNGHI.ToString(), url_cookie);
            //        string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
            //        string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
            //        string noidung_kiennghi = "<p>" + func.TomTatNoiDung(EncodeOutput(l.NOIDUNG_KIENNGHI), id_encr) + "</p>";
            //        string linhvuc = "";
            //        if (l.ID_LINHVUC != 0) { linhvuc = " > " + l.TEN_LINHVUC; }

            //        string ten_donvi_gop = l.TEN_DONVITIEPNHAN;
            //        if (l.ID_GOP == -1)
            //        {
            //            ten_donvi_gop = l.TENDONVITIEPNHAN_GOP;
            //        }

            //        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count_kiennghi + "</td><td>" + noidung_kiennghi + "</td><td class='tcenter'>" +
            //            EncodeOutput(ten_donvi_gop) + "</td><td>"+ EncodeOutput(l.TEN_THAMQUYEN_DONVI)+ EncodeOutput(linhvuc) +"</td><td class='tcenter' nowrap>" + bt_lichsu + info + "</td></tr>";
            //        count_kiennghi++;
            //    }
            //}
            //foreach (var j in kiennghi)
            //{
            //    if (!id_donvithamquyen.Contains(j.ID_THAMQUYENDONVI) && j.ID_THAMQUYENDONVI != 0)
            //    {
            //        var kiennghi2 = kiennghi.Where(x => x.ID_THAMQUYENDONVI == j.ID_THAMQUYENDONVI).ToList();
            //        str += "<tr><td colspan='4' class='b'>" + kiennghi2.FirstOrDefault().TEN_THAMQUYEN_DONVI.ToUpper() + "</td></tr>";
            //        List<decimal> id_linhvuc = new List<decimal>();
            //        int count_linhvuc = 1;
            //        foreach (var k in kiennghi2.OrderByDescending(x => x.ID_LINHVUC).ToList())
            //        {
            //            if (!id_linhvuc.Contains(k.ID_LINHVUC))
            //            {
            //                var kiennghi3 = kiennghi2.Where(x => x.ID_LINHVUC == k.ID_LINHVUC).ToList();
            //                string tenlinhvuc = "Lĩnh vực: Chưa xác định";
            //                if (k.ID_LINHVUC > 0)
            //                {
            //                    tenlinhvuc = kiennghi3.Where(x => x.ID_LINHVUC == k.ID_LINHVUC).FirstOrDefault().TEN_LINHVUC;
            //                }
            //                str += "<tr><td colspan='4' class='b'>- - " + count_linhvuc + ". " + tenlinhvuc + " </td></tr>";
            //                count_linhvuc++;
            //                //str += "<tbody class='table-striped'>";
            //                List<decimal> list_id_kiennghi = new List<decimal>();
            //                int count = 1;
            //                foreach (var l in kiennghi3)
            //                {
            //                    if (!list_id_kiennghi.Contains(l.ID_KIENNGHI))
            //                    {
            //                        list_id_kiennghi.Add(l.ID_KIENNGHI);
            //                        string id_encr = HashUtil.Encode_ID(l.ID_KIENNGHI.ToString(), url_cookie);
            //                        string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
            //                        string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
            //                        string noidung_kiennghi = "<p>" + func.TomTatNoiDung(l.NOIDUNG_KIENNGHI, id_encr) + "</p>";
            //                        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + noidung_kiennghi + "</td><td>" +
            //                            l.TEN_DONVITIEPNHAN + "</td><td class='tcenter' nowrap>" + bt_lichsu + info + "</td></tr>";
            //                        count++;
            //                    }
            //                }
            //                //str += "</tbody>";

            //                id_linhvuc.Add(k.ID_LINHVUC);
            //            }
            //        }
            //        id_donvithamquyen.Add(j.ID_THAMQUYEN_DONVI);
            //    }
            //}
            return str;
        }
        public string KN_Theodoi_kiennghi_tralai(List<PRC_KIENNGHI_CHUYENKYSAU> kiennghi, List<KN_KIENNGHI_TRALOI> traloi, List<KN_GIAMSAT> giamsat, TaikhoanAtion act)
        {
            string str = "";
            if (kiennghi.Count() == 0)
            {
                return "<tr><td class='tcenter alert-danger' colspan='4'>Không tìm thấy kiến nghị nào!</td></tr>";
            }
            string url_cookie = func.Get_Url_keycookie();
            List<decimal> id_donvithamquyen = new List<decimal>();
            int count_kiennghi = 1;
            foreach (var j in kiennghi)
            {
                if (!id_donvithamquyen.Contains(j.ID_THAMQUYEN_DONVI))
                {
                    var kiennghi2 = kiennghi.Where(x => x.ID_THAMQUYEN_DONVI == j.ID_THAMQUYEN_DONVI).ToList();
                    str += "<tr><td colspan='4' class='b'>" + EncodeOutput(kiennghi2.FirstOrDefault().TEN_THAMQUYEN_DONVI.ToUpper()) + " (" + kiennghi2.Count() + " kiến nghị)</td></tr>";
                    List<decimal> id_linhvuc = new List<decimal>();
                    int count_linhvuc = 1;
                    foreach (var k in kiennghi2.OrderByDescending(x => x.ID_LINHVUC).ToList())
                    {
                        if (!id_linhvuc.Contains(k.ID_LINHVUC))
                        {
                            var kiennghi3 = kiennghi2.Where(x => x.ID_LINHVUC == k.ID_LINHVUC).ToList();
                            string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                            if (k.ID_LINHVUC > 0)
                            {
                                tenlinhvuc = kiennghi3.Where(x => x.ID_LINHVUC == k.ID_LINHVUC).FirstOrDefault().TEN_LINHVUC;
                            }
                            str += "<tr><td colspan='4' class='b'>- - " + count_linhvuc + ". " + EncodeOutput(tenlinhvuc) + " (" + kiennghi3.GroupBy(x => x.ID_KIENNGHI).Count() + " kiến nghị)</td></tr>";
                            count_linhvuc++;
                            //str += "<tbody class='table-striped'>";
                            List<decimal> list_id_kiennghi = new List<decimal>();
                            int count = 1;
                            foreach (var l in kiennghi3)
                            {
                                if (!list_id_kiennghi.Contains(l.ID_KIENNGHI))
                                {
                                    list_id_kiennghi.Add(l.ID_KIENNGHI);
                                    string id_encr = HashUtil.Encode_ID(l.ID_KIENNGHI.ToString(), url_cookie);

                                    string traloi_chuyenkysau = Content_Traloi_Theodoi_kiennghi_chuyenkysau_first(l, traloi, giamsat, url_cookie, act);
                                    string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
                                    string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                                    string noidung_kiennghi = "<p>" + func.TomTatNoiDung(EncodeOutput(l.NOIDUNG_KIENNGHI), id_encr) + "</p>";
                                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + noidung_kiennghi + "</td><td>" +
                                        traloi_chuyenkysau + "</td><td class='tcenter' nowrap>" + bt_lichsu + info + "</td></tr>";
                                    count++;
                                }
                            }
                            //str += "</tbody>";

                            id_linhvuc.Add(k.ID_LINHVUC);
                        }
                    }
                    id_donvithamquyen.Add(j.ID_THAMQUYEN_DONVI);
                }
            }
            return str;
            //string str = "";
            //if (kiennghi.Count() == 0)
            //{
            //    return "<tr><td class='tcenter alert-danger' colspan='4'>Không tìm thấy kiến nghị nào!</td></tr>";
            //}
            //coquan = coquan.OrderBy(x => x.IVITRI).ToList();
            //string url_cookie = func.Get_Url_keycookie();
            //foreach (var q in coquan)
            //{
            //    var kiennghi1 = kiennghi.Where(x => x.ITHAMQUYENDONVI == (int)q.ICOQUAN).ToList();
            //    int count = 1;
            //    if (kiennghi1.Count() > 0)
            //    {
            //        str += "<tr><th class='' colspan='4'>" + q.CTEN.ToUpper() + " (" + kiennghi1.Count() + ")</th></tr>";
            //        foreach (var k in kiennghi1)
            //        {
            //            string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_cookie);
            //            string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' class='trans_func'><i class='icon-info-sign'></i></a>";
            //            string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' class='trans_func'><i class='icon-time'></i></a>";
            //            //string traloi_add_ = Btn_DLL_Traloi_kiennghi_chuyenkysau(id_encr);
            //            string giamsat_add_ = " <a href =\"javascript:void()\" title='Đánh giá trả lời'  onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_danhgia')\" class='trans_func'><i class='icon-edit'></i></a> ";
            //            //if (remove == false) { traloi_add_ = ""; }
            //            string doan_tiepnhan = str_donvi_tiepnhan((int)k.IDONVITIEPNHAN, coquan);
            //            string noidung_kiennghi = "<p>" + func.TomTatNoiDung(k.CNOIDUNG, id_encr) + "</p>" + doan_tiepnhan;
            //            str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + noidung_kiennghi + "</td><td>" +
            //                Content_KN_kiennghi_traloi(k) + "</td><td class='tcenter' nowrap>" + bt_lichsu + info + "</td></tr>";
            //            count++;
            //        }
            //    }
            //}
            //return str;
        }
        public string DiaChiDayDu(string diachi,string huyen,string tinh)
        {
            
            string diachi_daydu = "";
            if(diachi!="" && diachi != null)
            {
                diachi_daydu = diachi;
            }
            if (huyen != null)
            {
                if (diachi_daydu != "")
                {
                    diachi_daydu += " ,";
                }
                diachi_daydu += huyen;
            }
            if (tinh != null)
            {
                if (diachi_daydu != "")
                {
                    diachi_daydu += " ,";
                }
                diachi_daydu += tinh;
            }
            //diachi_daydu = diachi_daydu;
            return diachi_daydu;
        }
        public KN_CL KienNghi_Detail(int id, string url_key)
        {
            KN_CL kn = new KN_CL();
            KN_KIENNGHI don = _kn.HienThiThongTinKienNghi(id);
            if (don.ILINHVUC != 0)
            {
                var linhvuc = _kn.Get_LinhVuc_CoQuan((int)don.ILINHVUC);
                if (linhvuc != null)
                {
                    kn.linhvuc = EncodeOutput(linhvuc.CTEN);
                } else
                {
                    kn.linhvuc = "Chưa xác định";
                }
            }
            else
            {
                kn.linhvuc = "Chưa xác định";
            }
            if (don.IKYHOP != 0)
            {
                QUOCHOI_KYHOP kyhop = _kn.Get_KyHop_QuocHoi((int)don.IKYHOP);
                if (kyhop != null)
                {
                    kn.kyhop = EncodeOutput(kyhop.CTEN);
                    QUOCHOI_KHOA khoa = _kn.Get_Khoa_QuocHoi((int)kyhop.IKHOA);
                    if (khoa != null)
                    {
                        kn.khoahop = EncodeOutput(khoa.CTEN);
                    }

                }

            }
            if (don.INGUONKIENNGHI != null && don.INGUONKIENNGHI != 0)
            {
                KN_NGUONDON nguonkiennghi = _kntc.Get_NguonKienNghi((int)don.INGUONKIENNGHI);
                kn.nguonkiennghi = EncodeOutput(nguonkiennghi.CTEN);

            }
            else
            {
                List<KIENNGHI_NGUONDONMap> lstnguondon = _kn.GetAll_KnNguonDon_Map(id);
                List<string> lstTen = new List<string>();
                foreach (var item in lstnguondon)
                {
                    lstTen.Add(item.CTEN);
                }
                string nguonkiennghi = String.Join(",", lstTen);
                kn.nguonkiennghi = EncodeOutput(nguonkiennghi);
            }    
            KN_CHUONGTRINH chuongtrinh = _kn.Get_ChuongTrinh_ByID((int)don.ICHUONGTRINH);
            if (chuongtrinh != null)
            {
                kn.kehoach = EncodeOutput(chuongtrinh.CKEHOACH);
            }
            else
            {
                kn.kehoach = "Không nằm trong chương trình, kế hoạch tiếp xúc cử tri";
            }
            QUOCHOI_COQUAN coquan = _kn.HienThiThongTinCoQuan((int)don.IDONVITIEPNHAN);
            if (coquan != null)
            {
                kn.donvi_tiepnhan = EncodeOutput(coquan.CTEN) ;
            }
            QUOCHOI_COQUAN coquan_thamquyen = _kn.HienThiThongTinCoQuan((int)don.ITHAMQUYENDONVI);
            if (coquan_thamquyen != null)
            {
                kn.donvi_thamquyen = EncodeOutput(coquan_thamquyen.CTEN);
            }
            else
            {
                kn.donvi_thamquyen = "Chưa xác định";
            }
            kn.truockyhop = Get_Ten_TruocKyHop((int)don.ITRUOCKYHOP);
            //if (don.ITRUOCKYHOP == 1) { kn.truockyhop = "Trước kỳ họp"; }
            string id_encr = HashUtil.Encode_ID(don.IKIENNGHI.ToString(), url_key);
            kn.file_view = File_View(id, "kn_kiennghi");
            kn.bt_info = " <a href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
            kn.bt_lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_lichsu')\" title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
            kn.bt_edit = " <a href=\"/Kiennghi/Sua/?id=" + id_encr + "\" title='Sửa kiến nghị' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
            kn.bt_del = " <a href=\"javascript:void()\" title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kiennghi/Ajax_Xoatam','Bạn có muốn xóa kiến nghị này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
            if (don.IDIAPHUONG0 != null)
            {
                kn.diachi_tinh = Get_TenDiaPhuong((int)don.IDIAPHUONG0);
            }else { kn.diachi_tinh = ""; }
            if (don.IDIAPHUONG1 != null)
            {
                kn.diachi_huyen = Get_TenDiaPhuong((int)don.IDIAPHUONG1);
            }
            else { kn.diachi_huyen = ""; }
            kn.diachi = EncodeOutput(don.CDIACHI);
            kn.diachi_daydu = DiaChiDayDu(kn.diachi, kn.diachi_huyen, kn.diachi_tinh);
            if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.Moicapnhat)
            {
                kn.tinhtrang = StringEnum.GetStringValue(TrangThaiKienNghi.Moicapnhat);
            }
            if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.Choxuly)
            {
                kn.tinhtrang = StringEnum.GetStringValue(TrangThaiKienNghi.Choxuly);
            }
            if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.ChuaTraLoiDangXuly)
            {
                kn.tinhtrang = "Đã chuyển đến " + kn.donvi_thamquyen;
            }
            if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet)
            {
                kn.tinhtrang = kn.donvi_thamquyen + " đang xử lý, giải quyết";
                //KN_KIENNGHI kn_traloi_dangia = new KN_KIENNGHI();
                //kn_traloi_dangia.IKIENNGHI = id;
                //var kn_traloi = _kn.PRC_LIST_KN_TRALOI_DANHGIA(kn_traloi_dangia);
                //_condition = new Dictionary<string, object>();
                //_condition.Add("IKIENNGHI", id);
                //if (_kn.GetAll_TraLoi_KienNghi_ByParamt(_condition).Count() > 0)
                //{
                //    kn.tinhtrang = "Đã có kết quả giải quyết từ " + kn.donvi_thamquyen;                    
                //    //kn.tr_traloi = "<tr><td>Kết quả giải quyết:</td><td colspan='3'>" + Content_Traloi_Kiennghi_for_detail(id) + "</td></tr>";
                //}
            }
            
            if (don.ID_GOP > 0)
            {
                string xem_kiennghi_gop = "";
                xem_kiennghi_gop = "<span class='f-orangered'>Kiến nghị đã được gộp</span> <a href='/Kiennghi/Kiennghi_info?id="+HashUtil.Encode_ID(don.ID_GOP.ToString(),url_key)+"'>[Xem chi tiết]</a>";
                kn.tr_traloi = "<tr><td>Kết quả giải quyết:</td><td colspan='3'>" + xem_kiennghi_gop + "</td></tr>";
                kn.tinhtrang = xem_kiennghi_gop;
            }
            //if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.KienNghiDaTraLoi_DongKienNghi)
            //{
            //    kn.tinhtrang = "Đã có kết quả trả lời từ " + kn.donvi_thamquyen;
            //}
            if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.KienNghiTrung_DongKienNghi)
            {
                string tinhtrang =StringEnum.GetStringValue(TrangThaiKienNghi.KienNghiTrung_DongKienNghi);
                string id_encr_trung = HashUtil.Encode_ID(don.IKIENNGHI_TRUNG.ToString(), url_key);
                tinhtrang += " <a href='/Kiennghi/Kiennghi_info/?id=" + id_encr_trung + "' class='f-orangered'>[Xem kiến nghị trùng]</a>";
                kn.tinhtrang = tinhtrang;
            }
            KN_KIENNGHI kn_pram = new KN_KIENNGHI();
            kn_pram.IKIENNGHI = id;
            var kn_traloi = _kn.PRC_LIST_KN_TRALOI_DANHGIA(kn_pram);
            if (kn_traloi.Count() > 0)
            {
                kn.tinhtrang = "Đã có trả lời";
            }

            return kn;
        }
        string Str_tinhtrang_kiennghi(KN_KIENNGHI don)
        {
            string tinhtrang = "";
            if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.Moicapnhat)
            {
                tinhtrang = StringEnum.GetStringValue(TrangThaiKienNghi.Moicapnhat);
            }
            if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.Choxuly)
            {
                tinhtrang = StringEnum.GetStringValue(TrangThaiKienNghi.Choxuly);
            }
            string donvithamquyen = "";
            if (don.ITHAMQUYENDONVI > 0) {
                QUOCHOI_COQUAN coquan = _kn.HienThiThongTinCoQuan((int)don.ITHAMQUYENDONVI);
                if (coquan != null)
                {
                    donvithamquyen = _kn.HienThiThongTinCoQuan((int)don.ITHAMQUYENDONVI).CTEN;
                }

            }
            if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.ChuaTraLoiDangXuly)
            {

                tinhtrang = "Ban Dân nguyện đã Tập hợp lại và chuyển đến " + EncodeOutput(donvithamquyen);
            }
            if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.KienNghiDangXemXetGiaiQuyet || don.ITINHTRANG == (decimal)TrangThaiKienNghi.KienNghiDaGiaiTrinh)
            {
                tinhtrang = "<strong>" + EncodeOutput(donvithamquyen) + "</strong> đang nghiên cứu, xử lý";
            }
            if (don.ITINHTRANG == (decimal)TrangThaiKienNghi.KienNghiTrung_DongKienNghi)
            {
                tinhtrang = StringEnum.GetStringValue(TrangThaiKienNghi.KienNghiTrung_DongKienNghi);
            }
            return tinhtrang;
        }
        public string Col_Traloi_kiennghi(int id_kiennghi)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IKIENNGHI", id_kiennghi);
            if (_kn.GetAll_TraLoi_KienNghi_ByParamt(_condition).Count() > 0)
            {
                string traloi = "";
                KN_KIENNGHI_TRALOI t = _kn.GetAll_TraLoi_KienNghi_ByParamt(_condition).FirstOrDefault();
                traloi = EncodeOutput(t.CTRALOI);
                str = traloi + ". " + File_View((int)t.ITRALOI, "kn_traloi");
            }
            return str;
        }
        //public string DanhGia_TraLoi_KienNghi_landau(int iKienNghi, KN_GIAMSAT g)
        //{
        //    string denghi = "Đóng kiến nghị";
        //    if (g.IDONGKIENNGHI == 0)
        //    {
        //        denghi = "Theo dõi ở kỳ họp sau";
        //    }
        //    string tiendo = "<strong class='f-green'>Đúng tiến độ</strong>";
        //    if (g.IDUNGTIENDO == 0)
        //    {
        //        tiendo = "<strong class='f-orangered'>Chậm tiến độ</strong>";
        //    }
        //    string str = "<p><strong>- Phân loại:</strong> " + _kn.Get_GiamSat_Phanloai((int)g.IPHANLOAI).CTEN +
        //                "</p><p><strong>- Đánh giá:</strong> " + _kn.Get_GiamSat_DanhGia((int)g.IDANHGIA).CTEN +
        //                "</p><p><strong>- Đề nghị:</strong> " + denghi + "</p> - <strong>Tiến độ:</strong> " + tiendo;
        //    return str;
        //}

        public string DanhGia_TraLoi_KienNghi(int iKienNghi,int iTraLoi)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IKIENNGHI", iKienNghi);
            _condition.Add("ITRALOI", iTraLoi);
            var giamsat = _kn.GetAll_Giamsat_TraLoi_byParam(_condition);
            if (giamsat.Count() > 0)
            {
                KN_GIAMSAT g = giamsat.FirstOrDefault();
                string denghi = "Đóng kiến nghị";
                if (g.IDONGKIENNGHI == 0)
                {
                    denghi = "Theo dõi ở kỳ họp sau";
                }
                string tiendo = "<strong class='f-green'>Đúng tiến độ</strong>";
                if (g.IDUNGTIENDO == 0)
                {
                    tiendo = "<strong class='f-orangered'>Chậm tiến độ</strong>";
                }
                str = "<p><strong>- Phân loại:</strong> " +EncodeOutput( _kn.Get_GiamSat_Phanloai((int)g.IPHANLOAI).CTEN) +
                            "</p><p><strong>- Đánh giá:</strong> " + EncodeOutput(_kn.Get_GiamSat_DanhGia((int)g.IDANHGIA).CTEN) +
                            "</p><p><strong>- Đề nghị:</strong> " + denghi + "</p> - <strong>Tiến độ:</strong> " + tiendo;
            }
            
            return str;
        }
        public string Row_KN_Tonghop_Doan_Choxuly_tonghop(List<PRC_TONGHOP_KIENNGHI> tonghop, PRC_TONGHOP_KIENNGHI t, TaikhoanAtion act,string url_cookie, int count)
        {
            string str = "";
            string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
            string edit = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_sua?id=" + id_encr + "\" title='Sửa' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
            string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
            string del = " <a href=\"javascript:void()\" title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Tonghop_del','Bạn có muốn xóa Tập hợp này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
            string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Chuyen_Xuly_tonghop')\" href='javascript:void()' title='Chuyển xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
            string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
            if (!base_bussiness.Action_(6, act) || t.SOKIENNGHI_TONGHOP == 0)
            {
                chuyen_xuly = "";
            }
            if (t.IUSER_CAPNHAT != act.iUser && !base_bussiness.Action_(50, act))
            {
                edit = ""; del = "";
            }
            string so_kn = " <a href=\"/Kiennghi/Tonghop_chonkiennghi/?id=" + id_encr + "\" title='Danh sách kiến nghị' rel='tooltip' title=''  class='trans_func'><i class='icon-list-ul'></i></a> ";
            string coquan_thamquyen = EncodeOutput(t.TEN_THAMQUYEN_DONVI_TONGHOP);
            string noidung = EncodeOutput(t.NOIDUNG_TONGHOP) + " (" + t.SOKIENNGHI_TONGHOP + " kiến nghị) ";
            str += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count + "</td><td colspan='2' class='b'>" + noidung +
            "</a></td><td class='tcenter b'>" +
            coquan_thamquyen + "</td><td class='tcenter' nowrap>" + bt_lichsu + chitiet + so_kn + chuyen_xuly + edit + del + "</td></tr>";
            if (t.SOKIENNGHI_TONGHOP > 0)
            {
                List<PRC_TONGHOP_KIENNGHI> kiennghi = tonghop.Where(x => x.ID_KIENNGHI_TONGHOP == t.ITONGHOP).ToList();
                //str += "<tbody id='list_" + t.ITONGHOP + "'>";
                int count1 = 1;
                List<decimal> list_id_kiennghi_gop = new List<decimal>();
                foreach (var k in kiennghi)
                {
                    if (!list_id_kiennghi_gop.Contains(k.IKIENNGHI))
                    {
                        list_id_kiennghi_gop.Add(k.ID_KIENNGHI_PARENT_GOP);
                        str += Row_KN_Tonghop_Doan_Choxuly_tonghop_kiennghi(kiennghi, k, act, url_cookie, count1);
                        count1++;
                    }                    
                }
                //str += "</tbody>";
            }
            return str;
        }
        public string Row_KN_Tonghop_Doan_Choxuly_tonghop_kiennghi(List<PRC_TONGHOP_KIENNGHI> tonghop, PRC_TONGHOP_KIENNGHI k, TaikhoanAtion act, string url_cookie, int count)
        {
            string str = "";
            string id_encr_kiennghi = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_cookie);
            string chitiet_kiennghi = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiennghi_info?id=" + id_encr_kiennghi + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
            string noidung_kiennghi = func.TomTatNoiDung(EncodeOutput(k.NOIDUNG_KIENNGHI), id_encr_kiennghi);
            string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr_kiennghi + "','/Kiennghi/Ajax_Chuyen_tonghop')\" href='javascript:void()' data-original-title='Chuyển qua Tập hợp khác' rel='tooltip' title='' class='trans_func'><i class='icon-signin'></i></a>";
            //string kiennghi_gop = "";
            string del = " <a href=\"javascript:void()\" data-original-title='Loại kiến nghị khỏi Tập hợp' rel='tooltip' title='' onclick=\"DeletePage_Confirm_KN_TONGHOP('" + k.IKIENNGHI + "','id=" + k.IKIENNGHI + 
            "','/Kiennghi/Ajax_Remove_kiennghi_by_tonghop','Bạn có muốn loại kiến nghị này khỏi Tập hợp không?')\" class='trans_func'><i class='icon-remove'></i></a> ";
            if (k.ID_GOP == -1) { noidung_kiennghi = "<a href='/Kiennghi/Kiennghi_gop_list/?id="+id_encr_kiennghi+"'>" + EncodeOutput(k.NOIDUNG_KIENNGHI) + "</a>"; }
            str += "<tr><td class='tcenter' width='3%'>" + count + "</td><td colspan='2'><em>" + noidung_kiennghi
                + "</em></td><td class='tcenter'>" + EncodeOutput(k.TEN_THAMQUYEN_DONVI_KIENNGHI) + "</td><td class='tcenter'>" + chitiet_kiennghi + chuyen_xuly+ del+"</td></tr>";
            return str;
        }
        public string KN_Tonghop_Doan_Choxuly(List<PRC_TONGHOP_KIENNGHI> tonghop, TaikhoanAtion act)
        {
            string str = "";
            if (tonghop.Count() == 0) { return "<tr><td colspan='5' class='alert tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
            int count = 1; int count1 = 1;
            string url_cookie = func.Get_Url_keycookie();
            decimal id_tonghop = 0;
            decimal id_kiennghi = 0;
            foreach(var t in tonghop)
            {
                string id_encr_th = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
                if (id_tonghop != t.ITONGHOP)
                {
                    string edit = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_sua?id=" + id_encr_th + "\" title='Sửa' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                    string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr_th + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                    string del = " <a href=\"javascript:void()\" title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr_th + "','id=" + id_encr_th + "','/Kiennghi/Ajax_Tonghop_del','Bạn có muốn xóa Tập hợp này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr_th + "','/Kiennghi/Ajax_Chuyen_Xuly_tonghop')\" href='javascript:void()' title='Chuyển xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
                    string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr_th + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                    if (!base_bussiness.Action_(6, act) || t.SOKIENNGHI_TONGHOP == 0)
                    {
                        chuyen_xuly = "";
                    }
                    if (t.IUSER_CAPNHAT != act.iUser && !base_bussiness.Action_(50, act))
                    {
                        edit = ""; del = "";
                    }
                    string so_kn = " <a href=\"/Kiennghi/Tonghop_chonkiennghi/?id=" + id_encr_th + "\" title='Danh sách kiến nghị' rel='tooltip' title=''  class='trans_func'><i class='icon-list-ul'></i></a> ";
                    string coquan_thamquyen = t.TEN_THAMQUYEN_DONVI_TONGHOP;
                    string taidanhsach = "<a href='/Kiennghi/Download_kiennghi_bytonghop/?id=" + id_encr_th + "' title='Tải danh sách kiến nghị' rel='tooltip' title=''  class='trans_func'><i class='icon-file-alt'></i></a>";
                    string noidung = EncodeOutput(t.NOIDUNG_TONGHOP) + " (" + t.SOKIENNGHI_TONGHOP + " kiến nghị) ";
                    string icon = "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='plus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\"><i class='icon-plus'></i></a>" +
                        "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='minus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\" style='display:none'><i class='icon-minus'></i></a>";
                    if (t.SOKIENNGHI_TONGHOP == 0)
                    {
                        taidanhsach = ""; icon = "<i class='icon-plus f-grey'></i>";
                    }

                    str += "<tr><td colspan='3' class=''>"+ icon+" " + noidung +
                    "</a></td><td class='tcenter b'>" + EncodeOutput(coquan_thamquyen) + "</td><td class='tcenter' nowrap>" + bt_lichsu + chitiet+ taidanhsach + so_kn + chuyen_xuly + edit + del + "</td></tr>";
                    //count++;
                }
                if (id_kiennghi != t.IKIENNGHI && t.IKIENNGHI!=0)
                {
                    id_kiennghi = t.IKIENNGHI;
                    //str += Row_KN_Tonghop_Doan_Choxuly_tonghop_kiennghi(tonghop, t, act, url_cookie, count1);
                    string id_encr_kiennghi = HashUtil.Encode_ID(t.IKIENNGHI.ToString(), url_cookie);
                    string chitiet_kiennghi = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiennghi_info?id=" + id_encr_kiennghi + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                    string noidung_kiennghi = func.TomTatNoiDung(EncodeOutput(t.NOIDUNG_KIENNGHI), id_encr_kiennghi);
                    string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr_kiennghi + "','/Kiennghi/Ajax_Chuyen_tonghop')\" href='javascript:void()' data-original-title='Chuyển qua Tập hợp khác' rel='tooltip' title='' class='trans_func'><i class='icon-signin'></i></a>";
                    //string kiennghi_gop = "";
                    string del = " <a href=\"javascript:void()\" data-original-title='Loại kiến nghị khỏi Tập hợp' rel='tooltip' title='' onclick=\"DeletePage_Confirm_KN_TONGHOP('" + t.IKIENNGHI + "','id=" + t.IKIENNGHI +
                    "','/Kiennghi/Ajax_Remove_kiennghi_by_tonghop','Bạn có muốn loại kiến nghị này khỏi Tập hợp không?')\" class='trans_func'><i class='icon-remove'></i></a> ";
                    if (t.ID_GOP == -1) { noidung_kiennghi = "<a href='/Kiennghi/Kiennghi_gop_list/?id=" + id_encr_kiennghi + "'>" + EncodeOutput(t.NOIDUNG_KIENNGHI) + "</a>"; }
                    str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td class='tcenter' width='3%'>" + count + "</td><td colspan='2'><em>" + noidung_kiennghi
                        + "</em></td><td class='tcenter'>" + EncodeOutput(t.TEN_THAMQUYEN_DONVI_KIENNGHI) + "</td><td class='tcenter'>" + chitiet_kiennghi + chuyen_xuly + del + "</td></tr>";
                    count++;
                }
                id_tonghop = t.ITONGHOP;
            }
            //List<decimal> list_id_tonghop = new List<decimal>();
            //tonghop = tonghop.OrderBy(x => x.ID_GOP).ToList();
            //foreach (var t in tonghop)
            //{
            //    if (!list_id_tonghop.Contains(t.ITONGHOP))
            //    {
            //        list_id_tonghop.Add(t.ITONGHOP);
            //        //str += Row_KN_Tonghop_Doan_Choxuly_tonghop(tonghop, t, act, url_cookie, count);
            //        string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
            //        string edit = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_sua?id=" + id_encr + "\" title='Sửa' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
            //        string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
            //        string del = " <a href=\"javascript:void()\" title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Tonghop_del','Bạn có muốn xóa Tập hợp này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
            //        string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Chuyen_Xuly_tonghop')\" href='javascript:void()' title='Chuyển xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
            //        string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
            //        if (!base_bussiness.Action_(6, act) || t.SOKIENNGHI_TONGHOP == 0)
            //        {
            //            chuyen_xuly = "";
            //        }
            //        if (t.IUSER_CAPNHAT != act.iUser && !base_bussiness.Action_(50, act))
            //        {
            //            edit = ""; del = "";
            //        }
            //        string so_kn = " <a href=\"/Kiennghi/Tonghop_chonkiennghi/?id=" + id_encr + "\" title='Danh sách kiến nghị' rel='tooltip' title=''  class='trans_func'><i class='icon-list-ul'></i></a> ";
            //        string coquan_thamquyen = t.TEN_THAMQUYEN_DONVI_TONGHOP;
            //        string noidung = t.NOIDUNG_TONGHOP + " (" + t.SOKIENNGHI_TONGHOP + " kiến nghị) ";
            //        str += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count + "</td><td colspan='2' class='b'>" + noidung +
            //        "</a></td><td class='tcenter b'>" +
            //        coquan_thamquyen + "</td><td class='tcenter' nowrap>" + bt_lichsu + chitiet + so_kn + chuyen_xuly + edit + del + "</td></tr>";
            //        if (t.SOKIENNGHI_TONGHOP > 0)
            //        {
            //            List<PRC_TONGHOP_KIENNGHI> kiennghi = tonghop.Where(x => x.ID_KIENNGHI_TONGHOP == t.ITONGHOP).ToList();
            //            List<decimal> list_id_kiennghi_gop = new List<decimal>();
            //            foreach (var k in kiennghi)
            //            {
            //                if (!list_id_kiennghi_gop.Contains(k.IKIENNGHI))
            //                {
            //                    list_id_kiennghi_gop.Add(k.ID_KIENNGHI_PARENT_GOP);
            //                    str += Row_KN_Tonghop_Doan_Choxuly_tonghop_kiennghi(kiennghi, k, act, url_cookie, count1);
            //                    count1++;
            //                }
            //            }
            //            //str += "</tbody>";
            //        }
            //        count++;
            //    }
                
            //}
            return str;
        }
        public string KN_Tonghop_ChuyenDanNguyen(List<PRC_LIST_TONGHOP_KIENNGHI> tonghop)
        {
            string str = "";
            if (tonghop.Count() == 0) { return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }

            string url_cookie = func.Get_Url_keycookie();
            decimal id_donvi = 0; decimal id_linhvuc = -1; decimal id_tonghop = 0;
            int count_lv = 1; int count_tonghop = 1; int count_kn = 1;
            foreach (var i in tonghop)
            {
                string id_encr_th = HashUtil.Encode_ID(i.ITONGHOP.ToString(), url_cookie);
                if (id_tonghop != i.ITONGHOP)
                {

                    string danhsach_kiennghi = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_traloi/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Danh sách kiến nghị'><i class='icon-list-ul'></i></a>";
                    string chitiet = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_info/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết Tập hợp'><i class='icon-info-sign'></i></a>";
                    string lichsu = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                    string icon = "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='plus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\"><i class='icon-plus'></i></a>" +
                        "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='minus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\" style='display:none'><i class='icon-minus'></i></a>";
                    if (i.SOKIENNGHI_TONGHOP == 0)
                    {
                        icon = "<i class='icon-plus f-grey'></i> ";
                    }
                    str += "<tr><td class='' colspan='3'>" + icon + " " + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_TONGHOP), id_encr_th) +
                        "</td><td class=\"tcenter\" nowrap>" + danhsach_kiennghi + chitiet + lichsu + "</td></tr>";
                    count_lv = 1; id_linhvuc = -1; count_tonghop++;
                }
                if (id_linhvuc != i.ID_LINHVUC_KIENNGHI)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (i.ID_LINHVUC_KIENNGHI != 0) { tenlinhvuc = "Lĩnh vực: " + EncodeOutput(i.TEN_LINHVUC_KIENNGHI); }
                    str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td colspan='4' class=''>- - - " + tenlinhvuc + "</td></tr>";
                    count_lv++;
                }

                string id_encr_kn = HashUtil.Encode_ID(i.IKIENNGHI.ToString(), url_cookie);
                string tiepnhan = i.TEN_DONVITIEPNHAN_KIENNGHI;
                if (i.ID_GOP < 0) { tiepnhan = i.TENDONVITIEPNHAN_GOP; }
                tiepnhan = "</br><em><strong>Tiếp nhận:</strong> " + tiepnhan + "</em>";
                string traloi_add_ = "";
                if (i.ID_TRALOI != 0)
                {
                    traloi_add_ = Content_Traloi_kiennghi_Edit(i, null, url_cookie);
                }
                string chitiet_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết kiến nghị'><i class='icon-info-sign'></i></a>";
                string lichsu_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td class='tcenter'>" + count_kn + "</td><td><em>" + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_KIENNGHI), id_encr_kn) + tiepnhan + "</em></td><td nowrap>" + traloi_add_ + "</td><td class='tcenter'>" + chitiet_kn + lichsu_kn + "</td></tr>";
                id_tonghop = i.ITONGHOP;
                id_linhvuc = i.ID_LINHVUC_KIENNGHI;
                id_donvi = i.ID_THAMQUYEN_DONVI_TONGHOP;
                count_kn++;
            }

            return str;
        }
        //public string KN_Tonghop_ChuyenXuLy(List<KN_TONGHOP> tonghop, List<QUOCHOI_COQUAN> coquan, TaikhoanAtion act)
        //{
        //    string str = "";

        //    if (tonghop.Count() == 0) { return "<tr><td colspan='3' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
        //    string url_cookie = func.Get_Url_keycookie();
        //    coquan = coquan.OrderBy(x => x.CTEN).ToList();
        //    foreach (var q in coquan)
        //    {
        //        var tonghop1 = tonghop.Where(x => x.IDONVITONGHOP == (int)q.ICOQUAN).ToList();
        //        if (tonghop1.Count() > 0)
        //        {
        //            str += "<tr><th colspan='3' class=''>" + EncodeOutput(q.CTEN.ToUpper()) + " (" + tonghop1.Count() + ")</th></tr>";
        //            int count = 1;
        //            foreach (var t in tonghop)
        //            {
        //                // new id 
        //                string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
        //                string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
        //                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + func.TomTatNoiDung(EncodeOutput(t.CNOIDUNG), id_encr) +
        //                "</td><td class='tcenter' nowrap>" + bt_lichsu + chitiet + "</td></tr>";
        //                count++;
        //            }
        //        }
        //    }

        //    return str;
        //}
        //public string KN_Tonghop_Doan_Chuyen_BDN(List<PRC_TONGHOP_KIENNGHI> tonghop,TaikhoanAtion act)
        //{
        //    string str = "";
        //    //var tonghop1 = tonghop.Where(x => x.ID_GOP <= 0).OrderByDescending(x => x.SOKIENNGHI_TONGHOP).ToList();
        //    if (tonghop == null) { return "<tr><td colspan='4' class='tcenter alert alert-danger'>Không tìm thấy Tập hợp kiến nghị nào!</td><tr>"; }

        //    List<decimal> donvitiepnhan_cap0 = new List<decimal>();
        //    int count_tonghop = 1; int count_kiennghi = 1;
        //    string url_cookie = func.Get_Url_keycookie();
        //    decimal id_donvi = 0;decimal id_tonghop = 0;decimal id_kiennghi = 0;
        //    List<decimal> donvitiepnhan_cap1 = new List<decimal>();
        //    decimal id_linhvuc = -1;
        //    decimal count_linhvuc = 1;
        //    foreach(var  d in tonghop)
        //    {
        //        //if (id_donvi != d.ID_THAMQUYEN_DONVI_TONGHOP)
        //        //{
        //        //    str += "<tr><th colspan='4'>" + EncodeOutput(d.TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper()) + "</th><tr>";
        //        //}
        //        string id_encr_th = HashUtil.Encode_ID(d.ITONGHOP.ToString(), url_cookie);
        //        if (id_tonghop != d.ITONGHOP)
        //        {
        //            string icon = "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' onclick=\"$('.tr_" + id_encr_th + "').toggle()\"><i class='icon-plus'></i></a>";

        //            string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr_th + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //            str += "<tr><td class='' colspan='2'>"+ icon+" " + EncodeOutput(d.NOIDUNG_TONGHOP) + 
        //                "</td><td class='tcenter'>" + EncodeOutput(d.TEN_DONVITONGHOP) + "</td><td class='tcenter'>" + chitiet + "</td></tr>";
        //            count_tonghop++;
        //            id_linhvuc = -1; count_linhvuc = 1;
        //        }
        //        if (id_kiennghi != d.IKIENNGHI && d.IKIENNGHI!=0 && d.SOKIENNGHI_TONGHOP>0)
        //        {
        //            string id_encr = HashUtil.Encode_ID(d.IKIENNGHI.ToString(), url_cookie);                    
        //            string noidung_kiennghi = func.TomTatNoiDung(EncodeOutput(d.NOIDUNG_KIENNGHI), id_encr);string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiennghi_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //            str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td class='tcenter'>" + count_kiennghi + "</td><td><em>" + 
        //                noidung_kiennghi + "</em></td><td class='tcenter'>" + EncodeOutput(d.TEN_THAMQUYEN_DONVI_KIENNGHI) + "</td><td class='tcenter'>" + chitiet + "</td></tr>";
        //            count_kiennghi++;
        //        }
        //        id_linhvuc = d.ID_LINHVUC_KIENNGHI;
        //        id_kiennghi = d.IKIENNGHI;
        //        id_tonghop = d.ITONGHOP;
        //        id_donvi = d.ID_THAMQUYEN_DONVI_TONGHOP;
        //    }
        //    //foreach (var d in tonghop1)
        //    //{
        //    //    if (!donvitiepnhan_cap1.Contains(d.ID_THAMQUYEN_DONVI_TONGHOP))
        //    //    {
        //    //        var tonghop3 = tonghop1.Where(x => x.ID_THAMQUYEN_DONVI_TONGHOP == d.ID_THAMQUYEN_DONVI_TONGHOP).ToList();
        //    //        str += "<tr><th colspan='5'>" + tonghop3.FirstOrDefault().TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper() + "</th><tr>";
        //    //        str += "<tbody class='table-striped'>";
        //    //        List<decimal> list_id_tonghop = new List<decimal>();
        //    //        foreach (var j in tonghop3)
        //    //        {
        //    //            if (!list_id_tonghop.Contains(j.ITONGHOP))
        //    //            {
        //    //                var tonghop4 = tonghop3.Where(x => x.ITONGHOP == j.ITONGHOP).OrderByDescending(x => x.SOKIENNGHI_TONGHOP).ToList();
        //    //                string id_encr = HashUtil.Encode_ID(j.ITONGHOP.ToString(), url_cookie);
        //    //                string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //    //                str += "<tr><td class='b tcenter'>" + count_tonghop + "</td><td class='b' colspan='2'>" + j.NOIDUNG_TONGHOP + "</td><td class='tcenter b'>" + j.TEN_DONVITONGHOP + "</td><td class='tcenter'>" + chitiet + "</td></tr>";
        //    //                List<decimal> list_id_kiennghi = new List<decimal>();
                            
        //    //                foreach (var k in tonghop4)
        //    //                {
        //    //                    if (!list_id_kiennghi.Contains(k.IKIENNGHI) && k.IKIENNGHI != 0)
        //    //                    {
        //    //                        id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_cookie);
        //    //                        string donvitiepnhan = k.TEN_DONVITIEPNHAN_KIENNGHI;
        //    //                        string noidung_kiennghi = func.TomTatNoiDung(k.NOIDUNG_KIENNGHI, id_encr);
        //    //                        if (k.ID_GOP == -1)
        //    //                        {
        //    //                            noidung_kiennghi = "<a href='/Kiennghi/Kiennghi_gop_list/?id=" + id_encr + "' class='gachchan'>" + k.NOIDUNG_KIENNGHI + "</a>";
        //    //                            List<string> list_donvi_gop = new List<string>();
        //    //                            donvitiepnhan = k.TEN_DONVITIEPNHAN_KIENNGHI;
        //    //                            int count_donvi_gop = 0;
        //    //                            foreach (var g in tonghop.Where(x => x.ID_GOP == k.IKIENNGHI).ToList())
        //    //                            {
        //    //                                if (!list_donvi_gop.Contains(d.TENDONVITIEPNHAN_GOP))
        //    //                                {
        //    //                                    if (count_donvi_gop > 0) { donvitiepnhan += ", "; }
        //    //                                    donvitiepnhan += d.TENDONVITIEPNHAN_GOP;
        //    //                                    list_donvi_gop.Add(d.TENDONVITIEPNHAN_GOP);
        //    //                                    count_donvi_gop++;
        //    //                                }
        //    //                            }
        //    //                        }
        //    //                        chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiennghi_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //    //                        str += "<tr><td class='' nowrap></td><td class='tcenter' width='3%'>" + count_kiennghi + "</td><td>" + noidung_kiennghi + "</td><td class='tcenter'>" + donvitiepnhan + "</td><td class='tcenter'>" + chitiet + "</td></tr>";
        //    //                        list_id_kiennghi.Add(k.IKIENNGHI); count_kiennghi++;
        //    //                    }
        //    //                }
        //    //                list_id_tonghop.Add(j.ITONGHOP); count_tonghop++;
        //    //            }
        //    //        }
        //    //        str += "</tbody>";
        //    //        donvitiepnhan_cap1.Add(d.ID_THAMQUYEN_DONVI_TONGHOP);
        //    //    }
        //    //}
        //    /*
        //    if (tonghop.Count() == 0) { return "<tr><td colspan='4' class='alert tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
        //    string url_cookie = func.Get_Url_keycookie();
        //    List<decimal> id_donvithamquyen = new List<decimal>();
        //    foreach (var l in tonghop)
        //    {
        //        if (!id_donvithamquyen.Contains(l.ID_THAMQUYEN_DONVI)) { id_donvithamquyen.Add(l.ID_THAMQUYEN_DONVI); }
        //    }
        //    foreach (var i in id_donvithamquyen)
        //    {
        //        var tonghop_donvi = tonghop.Where(x => x.ID_THAMQUYEN_DONVI == i).ToList();
        //        str += "<tr><td colspan='4' class='b'>"+tonghop_donvi.Where(x=>x.ID_THAMQUYEN_DONVI==i).FirstOrDefault().TEN_THAMQUYEN_DONVI+" ("+tonghop_donvi.Count()+")</td></tr>";
        //        List<decimal> id_linhvuc = new List<decimal>();
        //        foreach(var l1 in tonghop_donvi)
        //        {
        //            if (!id_linhvuc.Contains(l1.ID_LINHVUC)) { id_linhvuc.Add(l1.ID_LINHVUC); }
        //        }
        //        id_linhvuc.Reverse();
        //        foreach (var l2 in id_linhvuc)
        //        {
        //            var tonghop_linhvuc = tonghop_donvi.Where(x => x.ID_LINHVUC == l2).ToList();
        //            string tenlinhvuc = "Chưa xác định";
        //            if (l2 != 0)
        //            {
        //                tenlinhvuc = tonghop_linhvuc.FirstOrDefault().TEN_LINHVUC;
        //            }
        //            str += "<tr><td colspan='4' class='b'>- - - " + tenlinhvuc + " (" + tonghop_linhvuc.Count() + ")</td></tr>";
        //            int count = 1;
        //            foreach (var t in tonghop_linhvuc)
        //            {
        //                string id_encr = HashUtil.Encode_ID(t.ID_TONGHOP.ToString(), url_cookie);
        //                decimal sokn_tonghop = t.SOKIENNGHI;
        //                string ds_kiennghi = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_kiennghi_doan?id=" + id_encr + "\" title='Xem kiến nghị' rel='tooltip' title='' class='b'>" + sokn_tonghop + "</a> ";
        //                string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
        //                string noidung_tonghop = func.TomTatNoiDung(t.NOIDUNG_TONGHOP, id_encr);
        //                string donvi_tonghop = "";
        //                if (act.is_lanhdao) { donvi_tonghop = "</br><strong>Đơn vị Tập hợp:</strong> " + t.TEN_DONVITONGHOP; }
        //                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + t.NOIDUNG_TONGHOP + donvi_tonghop +
        //                "</td><td class='tcenter'>" + ds_kiennghi + "</td><td class='tcenter' nowrap>" + bt_lichsu + chitiet + "</td></tr>";
        //                count++;
        //            }
        //        }
        //    }
        //    */
        //    //coquan = coquan.OrderBy(x => x.CTEN).ToList();
        //    //foreach (var q in coquan)
        //    //{
        //    //    var tonghop1 = tonghop.Where(x => x.IDONVITONGHOP == (int)q.ICOQUAN).ToList();
        //    //    if (tonghop1.Count() > 0)
        //    //    {
        //    //        str += "<tr><th colspan='5' class=''>" + q.CTEN.ToUpper() + " (" + tonghop1.Count() + ")</th></tr>";
        //    //        int count = 1;
        //    //        foreach (var t in tonghop)
        //    //        {
        //    //            // new id 
        //    //            string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);

        //    //            int sokn_tonghop = kiennghi.Where(x => x.ITONGHOP == (t.ITONGHOP)).Count();
        //    //            string ds_kiennghi = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_kiennghi_doan?id=" + id_encr + "\" title='Xem kiến nghị' rel='tooltip' title='' class='b'>" + sokn_tonghop + "</a> ";
        //    //            string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //    //            string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
        //    //            str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter' nowrap>" + t.CMATONGHOP + "</td><td>" + t.CNOIDUNG +
        //    //            "</td><td class='tcenter'>" + ds_kiennghi + "</td><td class='tcenter' nowrap>" + bt_lichsu + chitiet + "</td></tr>";
        //    //            count++;
        //    //        }
        //    //    }
        //    //}

        //    return str;
        //}
        public string KN_Theodoi_tonghop_chuyendiaphuong(List<PRC_LIST_TONGHOP_KIENNGHI> tonghop)
        {
            string str = "";
            if (tonghop.Count() == 0) { return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
            string url_cookie = func.Get_Url_keycookie();
            List<decimal> list_id_coquan = new List<decimal>();
            int count = 1;
            decimal id_donvi = 0;decimal id_linhvuc = -1;
            int count_lv = 1;
            foreach (var c in tonghop)
            {
                if (id_donvi != c.ID_THAMQUYEN_DONVI_TONGHOP)
                {
                    str += "<tr><th colspan='4' >" + EncodeOutput(c.TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper()) + "</th></tr>";
                    id_linhvuc = -1; count_lv++;
                }
                if (id_linhvuc != c.ID_LINHVUC_TONGHOP)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (c.ID_LINHVUC_TONGHOP != 0)
                    {
                        tenlinhvuc = c.TEN_LINHVUC_TONGHOP;
                    }
                    str += "<tr><th colspan='4' >- - " + count_lv + ". " + EncodeOutput(tenlinhvuc) + "</th></tr>";
                    id_linhvuc = c.ID_LINHVUC_TONGHOP;
                }
                string id_encr = HashUtil.Encode_ID(c.ITONGHOP.ToString(), url_cookie);
                string chitiet = " <a onclick=\"ShowPageLoading()\"  href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                string kn = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_traloi?id=" + id_encr + "' title='Danh sách kiến nghị' rel='tooltip' title='' class='trans_func'><i class='icon-list-ul'></i></a>";
                string tinhtrang = TinhTrangTraLoi_TongHop(c.ITINHTRANG, c.SOKIENNGHI_CHUATRALOI);
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + func.TomTatNoiDung(EncodeOutput(c.NOIDUNG_TONGHOP), id_encr) +
                "</td><td class='tcenter'>" + tinhtrang + "</td><td class='tcenter' nowrap>" + kn + bt_lichsu + chitiet + "</td></tr>";
                count++;
                id_donvi = c.ID_THAMQUYEN_DONVI_TONGHOP;
                //if (!list_id_coquan.Contains(c.ID_THAMQUYEN_DONVI_TONGHOP)) {
                //    list_id_coquan.Add(c.ID_THAMQUYEN_DONVI_TONGHOP);
                //    var tonghop1 = tonghop.Where(x => x.ID_THAMQUYEN_DONVI_TONGHOP == c.ID_THAMQUYEN_DONVI_TONGHOP).OrderByDescending(x=>x.ID_LINHVUC_TONGHOP).ToList();
                //    str += "<tr><th colspan='4' >" + EncodeOutput(c.TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper()) + "</th></tr>";

                //    List<decimal> list_id_linhvuc = new List<decimal>();
                //    int count_lv = 1;
                //    foreach (var t in tonghop1)
                //    {
                //        if (!list_id_linhvuc.Contains(t.ID_LINHVUC_TONGHOP))
                //        {
                //            list_id_linhvuc.Add(t.ID_LINHVUC_TONGHOP);
                //            var tonghop2 = tonghop1.Where(x => x.ID_LINHVUC_TONGHOP == t.ID_LINHVUC_TONGHOP).ToList();
                //            string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                //            if (t.ID_LINHVUC_TONGHOP != 0) { tenlinhvuc = tonghop2.FirstOrDefault().TEN_LINHVUC_TONGHOP; }
                //            str += "<tr><td class='b' colspan='4'>- - "+count_lv+". " + EncodeOutput(tenlinhvuc) + "</th></tr>";
                //            count_lv++;

                //            foreach (var k in tonghop2)
                //            {
                //                string id_encr = HashUtil.Encode_ID(k.ITONGHOP.ToString(), url_cookie);
                //                string chitiet = " <a onclick=\"ShowPageLoading()\"  href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                //                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                //                string kn = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_traloi?id=" + id_encr + "' title='Danh sách kiến nghị' rel='tooltip' title='' class='trans_func'><i class='icon-list-ul'></i></a>";
                //                string tinhtrang = TinhTrangTraLoi_TongHop(k.ITINHTRANG,k.SOKIENNGHI_CHUATRALOI);
                //                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + func.TomTatNoiDung(EncodeOutput(k.NOIDUNG_TONGHOP), id_encr) +
                //                "</td><td class='tcenter'>" + tinhtrang + "</td><td class='tcenter' nowrap>" + kn + bt_lichsu + chitiet + "</td></tr>";
                //                count++;
                //            }                          
                //        }

                //    }
                //}

            }
            return str;
        }
        public string TinhTrangTraLoi_TongHop(decimal tinhtrang,decimal sokiennghi_chuatraloi)
        {
            string str = "Đang chờ chuyển xử lý";
            if (tinhtrang > (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen)
            {
                if (sokiennghi_chuatraloi == 0)
                {
                    str = "Đã xử lý, hoàn thành"; 
                }
                else
                {
                    str = "Đang xử lý";
                }
            }
            return str;
        }
        //public string KN_Theodoi_tonghop_chuyen_giaiquyet(List<KN_TONGHOP> tonghop, List<QUOCHOI_COQUAN> coquan, TaikhoanAtion act)
        //{
        //    string str = "";
        //    Quochoi_CoquanRepository _coquan = new Quochoi_CoquanRepository();
        //    if (tonghop.Count() == 0) { return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
        //    int count = 1;
        //    string url_cookie = func.Get_Url_keycookie();
        //    coquan = coquan.OrderBy(x => x.CTEN).ToList();
        //    foreach (var q in coquan)
        //    {
        //        var tonghop1 = tonghop.Where(x => x.ITHAMQUYENDONVI == (int)q.ICOQUAN).ToList();
        //        if (tonghop1.Count() > 0)
        //        {
        //            str += "<tr><th colspan='4' class=''>" + EncodeOutput(q.CTEN.ToUpper()) + " (" + tonghop1.Count() + ")</th></tr>";
        //            foreach (var t in tonghop1)
        //            {
        //                string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
        //                string tinhtrang = Str_tonghop_tinhtrang(t);
        //                string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
        //                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
        //                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + func.TomTatNoiDung(EncodeOutput(t.CNOIDUNG), id_encr) +
        //                "</td><td>" + tinhtrang + "</td><td class='tcenter' nowrap>" + bt_lichsu + chitiet + "</td></tr>";
        //                count++;
        //            }
        //        }

        //    }

        //    return str;
        //}
        public string KN_Tonghop_Bandannguyen(List<PRC_TONGHOP_KIENNGHI> tonghop, UserInfor act, int loaiTaphop)
        {

            string str = "";
            //var tonghop1 = tonghop.Where(x => x.ID_GOP <= 0).ToList();
            if (tonghop == null) { return "<tr><td colspan='5' class='tcenter alert alert-danger'>Không tìm thấy Tập hợp kiến nghị nào!</td><tr>"; }
            
            List<decimal> donvitiepnhan_cap0 = new List<decimal>();
            int count_tonghop = 1; int count_lv = 1;
            string url_cookie = func.Get_Url_keycookie();
            decimal id_donvi = 0; decimal id_tonghop = 0; decimal id_linhvuc = -1;int count_kn = 1;
            decimal id_kiennghi = 0;
            foreach(var d in tonghop)
            {
                              
                if (id_donvi != d.ID_THAMQUYEN_DONVI_TONGHOP)
                {
                    id_linhvuc = -1; count_lv = 1;id_tonghop = 0;
                    if(d.TEN_THAMQUYEN_DONVI_TONGHOP != null)
                        str += "<tr><th colspan='4'>" + EncodeOutput(d.TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper()) + " </th><tr>";
                }
                string id_encr_th = HashUtil.Encode_ID(d.ITONGHOP.ToString(), url_cookie);
                if (id_tonghop != d.ITONGHOP)
                {
                    
                    // string duthao = " <a href=\"/Kiennghi/Duthao/?id=" + id_encr_th + "\" title='Dự thảo Tập hợp gửi đơn vị thẩm quyền' rel='tooltip' title='' class='trans_func'><i class='icon-file-alt'></i></a> ";
                    string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr_th + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                    string edit = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_sua?id=" + id_encr_th + "\" title='Sửa' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr_th + "','id=" + id_encr_th + "','/Kiennghi/Ajax_Tonghop_del','Bạn có muốn xóa Tập hợp này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string chuyen_congvan = " <a onclick=\"ShowPopUp('id=" + id_encr_th + "&" + "loaiTaphop=" + loaiTaphop + "','/Kiennghi/Ajax_Chuyen_Donvi_xuly')\" href='javascript:void()' title='Chuyển công văn' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
                    string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr_th + "','/Kiennghi/Ajax_Chuyen_Xuly_Chuyenvien')\" href='javascript:void()' title='Chuyển xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-forward'></i></a>";
                    string tralai = "";
                    KN_CHUYENXULY cxl = _kn.GetChuyenXuLy_ByITONGHOP((int)d.ITONGHOP);
                    if (cxl != null)
                    {
                        string id_encr_cxl = HashUtil.Encode_ID(cxl.IKN_CHUYENXULY.ToString(), url_cookie);
                        tralai = " <a onclick=\"ShowPopUp('id=" + id_encr_cxl + "','/Kiennghi/Ajax_Chuyen_Xuly_Tralai')\" href='javascript:void()' title='Trả lại' rel='tooltip' title='' class='trans_func'><i class='icon-arrow-left'></i></a>";
                    }
                    string so_kn = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_chonkiennghi/?id=" + id_encr_th + "\" title='Danh sách kiến nghị' rel='tooltip' title=''  class='trans_func'><i class='icon-list-ul'></i></a> ";
                    string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr_th + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                    if (!base_bussiness.Action_(6, act.tk_action) && !base_bussiness.Action_(50, act.tk_action)) { chuyen_congvan = ""; }
                    if (d.IUSER_CAPNHAT != act.user_login.IUSER && !base_bussiness.Action_(50, act.tk_action))
                    {
                        edit = ""; del = "";
                    }
                    if (!base_bussiness.Action_(50, act.tk_action)) { bt_lichsu = ""; }
                    string noidung = EncodeOutput(d.NOIDUNG_TONGHOP).Replace("\r\n", "</br>");
                    string icon = "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='plus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\"><i class='icon-plus'></i></a>" +
                        "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='minus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\" style='display:none'><i class='icon-minus'></i></a>";
                    if (d.SOKIENNGHI_TONGHOP == 0) { icon = "<i class='icon-plus f-grey'></i> "; }
                    str += "<tr><td colspan='3' class=''>"+ icon + " " + noidung + " </td><td class='tcenter' nowrap>" + chitiet + bt_lichsu  + chuyen_congvan + /*chuyen_xuly + tralai*/ so_kn + edit + del + "</td></tr>";
                    count_tonghop++;
                    id_linhvuc = -1;
                }                
                if (d.IKIENNGHI != 0 && id_kiennghi!=d.IKIENNGHI)
                {
                    if (id_linhvuc != d.ID_LINHVUC_KIENNGHI)
                    {
                        string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                        if (d.ID_LINHVUC_KIENNGHI != 0)
                        {
                            tenlinhvuc = "Lĩnh vực: " + EncodeOutput(d.TEN_LINHVUC_KIENNGHI);
                        }
                        str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td class='' colspan='4'>- - - " + tenlinhvuc + "</td><tr>";
                        count_lv++;
                    }
                    string tiepnhan = EncodeOutput(d.TEN_DONVITIEPNHAN_KIENNGHI);
                    if (d.ID_GOP < 0)
                    {
                        tiepnhan = EncodeOutput(d.TENDONVITIEPNHAN_GOP);
                    }
                    string id_encr = HashUtil.Encode_ID(d.IKIENNGHI.ToString(), url_cookie);
                    string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                    string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiennghi_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                    string edit = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Sua?id=" + id_encr + "\" title='Sửa' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
  
                    string del = " <a href=\"javascript:void()\" data-original-title='Loại kiến nghị khỏi Tập hợp' rel='tooltip' title='' onclick=\"DeletePage_Confirm_KN_TONGHOP('" + d.IKIENNGHI + "','id=" + d.IKIENNGHI + "','/Kiennghi/Ajax_Remove_kiennghi_by_tonghop','Bạn có muốn loại kiến nghị này khỏi Tập hợp không?')\" class='trans_func'><i class='icon-remove'></i></a> ";
                    string chuyen_xuly = " <a onclick=\"ShowPopUp('id_th=" + id_encr_th + "&" + "id=" + id_encr + "&" + "loaiTaphop=" + loaiTaphop + "','/Kiennghi/Ajax_Chuyen_tonghop')\" href='javascript:void()' data-original-title='Chuyển qua Tập hợp khác' rel='tooltip' title='' class='trans_func'><i class='icon-signin'></i></a>";
                    string noidung_chuyen = ""; string class_chuyen = "";
                    if (d.THAMQUYEN_CHUYEN != null)
                    {
                        noidung_chuyen = "</br><em class='f-red'>[" + d.THAMQUYEN_CHUYEN+"]</em>";
                        class_chuyen = "f-orangered";
                    }
                    str += "<tr class='tr_" + id_encr_th + " "+ class_chuyen + "' style='display:none'><td class='tcenter'>" + count_kn + "</td><td><em>" + func.TomTatNoiDung(EncodeOutput(d.NOIDUNG_KIENNGHI), d.IKIENNGHI.ToString()) + "</em>"+ noidung_chuyen + "</td><td class='tcenter'>"+ tiepnhan + 
                            "</td><td class='tcenter'>"+ chitiet+bt_lichsu+edit+ chuyen_xuly+del+ "</td></tr>";
                    count_kn++;
                }
                id_kiennghi = d.IKIENNGHI;
                id_linhvuc = d.ID_LINHVUC_KIENNGHI;
                id_tonghop = d.ITONGHOP;
                id_donvi = d.ID_THAMQUYEN_DONVI_TONGHOP;
            }
            
            return str;
        }
        //public string VietTat_CoQuan(string tendonvi)
        //{
        //    string str = "";
        //    int count = 0;
        //    tendonvi = tendonvi.ToUpper().Replace("và", " ");
        //    string[] str_ = tendonvi.Trim().Split(' ');
        //    foreach (var x in str_)
        //    {
        //        str += x[0].ToString();
        //        count++;
        //    }
        //    return str;
        //}
        public string DBQH_Tonghop_ChuyenDiaPhuongXuLy(List<PRC_LIST_TONGHOP_KIENNGHI> tonghop3)
        {
            string str = "";   
            

            if (tonghop3.Count() == 0) { return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
            string url_cookie = func.Get_Url_keycookie();
            decimal id_donvi = 0; decimal id_linhvuc = -1; decimal id_tonghop = 0;
            int count_lv = 1; int count_tonghop = 1; int count_kn = 1;
            foreach (var i in tonghop3)
            {
                string id_encr_th = HashUtil.Encode_ID(i.ITONGHOP.ToString(), url_cookie);
                if (id_tonghop != i.ITONGHOP)
                {

                    string danhsach_kiennghi = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_traloi/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Danh sách kiến nghị'><i class='icon-list-ul'></i></a>";
                    string chitiet = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_info/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết Tập hợp'><i class='icon-info-sign'></i></a>";
                    string lichsu = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                    string icon = "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='plus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\"><i class='icon-plus'></i></a>" +
                        "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='minus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\" style='display:none'><i class='icon-minus'></i></a>";
                    if (i.SOKIENNGHI_TONGHOP == 0)
                    {
                        icon = "<i class='icon-plus f-grey'></i> ";
                    }
                    str += "<tr><td class='' colspan='3'>" + icon + " " + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_TONGHOP), id_encr_th) +
                        "</td><td class=\"tcenter\" nowrap>" + danhsach_kiennghi + chitiet + lichsu + "</td></tr>";
                    count_lv = 1; id_linhvuc = -1; count_tonghop++;
                }
                if (id_linhvuc != i.ID_LINHVUC_KIENNGHI)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (i.ID_LINHVUC_KIENNGHI != 0) { tenlinhvuc = "Lĩnh vực: " + EncodeOutput(i.TEN_LINHVUC_KIENNGHI); }
                    str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td colspan='4' class=''>- - - " + tenlinhvuc + "</td></tr>";
                    count_lv++;
                }

                string id_encr_kn = HashUtil.Encode_ID(i.IKIENNGHI.ToString(), url_cookie);
                string tiepnhan = i.TEN_DONVITIEPNHAN_KIENNGHI;
                if (i.ID_GOP < 0) { tiepnhan = i.TENDONVITIEPNHAN_GOP; }
                tiepnhan = "</br><em><strong>Tiếp nhận:</strong> " + tiepnhan + "</em>";
                string traloi_add_ =  ""; 
                if (i.ID_TRALOI != 0)
                {
                    traloi_add_ = Content_Traloi_kiennghi_Edit(i, null, url_cookie);
                }
                string chitiet_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết kiến nghị'><i class='icon-info-sign'></i></a>";
                string lichsu_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td class='tcenter'>" + count_kn + "</td><td><em>" + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_KIENNGHI), id_encr_kn) + tiepnhan + "</em></td><td nowrap>" + traloi_add_ + "</td><td class='tcenter'>" + chitiet_kn + lichsu_kn + "</td></tr>";
                id_tonghop = i.ITONGHOP;
                id_linhvuc = i.ID_LINHVUC_KIENNGHI;
                id_donvi = i.ID_THAMQUYEN_DONVI_TONGHOP;
                count_kn++;
            }

            return str;
        }
        public string KN_Tonghop_Chuyen_BanDanNguyen(List<PRC_LIST_TONGHOP_KIENNGHI> tonghop3)
        {
            string str = "";
            if (tonghop3.Count() == 0) { return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
            string url_cookie = func.Get_Url_keycookie();
            decimal id_donvi = 0; decimal id_linhvuc = -1; decimal id_tonghop = 0;
            int count_lv = 1; int count_tonghop = 1; int count_kn = 1;
            foreach (var i in tonghop3)
            {
                string id_encr_th = HashUtil.Encode_ID(i.ITONGHOP.ToString(), url_cookie);
                if (id_tonghop != i.ITONGHOP)
                {

                    string danhsach_kiennghi = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_traloi/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Danh sách kiến nghị'><i class='icon-list-ul'></i></a>";
                    string chitiet = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_info/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết Tập hợp'><i class='icon-info-sign'></i></a>";
                    string lichsu = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                    string icon = "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='plus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\"><i class='icon-plus'></i></a>" +
                        "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='minus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\" style='display:none'><i class='icon-minus'></i></a>";
                    if (i.SOKIENNGHI_TONGHOP == 0)
                    {
                        icon = "<i class='icon-plus f-grey'></i> ";
                    }
                    str += "<tr><td class='' colspan='2'>" + icon + " " + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_TONGHOP), id_encr_th) +
                        "</td><td class='b tcenter'>"+EncodeOutput(i.TEN_DONVITONGHOP)+"</td><td class=\"tcenter\" nowrap>" + chitiet + "</td></tr>";
                    count_lv = 1; id_linhvuc = -1; count_tonghop++;
                }
                if (id_linhvuc != i.ID_LINHVUC_KIENNGHI)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (i.ID_LINHVUC_KIENNGHI != 0) { tenlinhvuc = "Lĩnh vực: " + EncodeOutput(i.TEN_LINHVUC_KIENNGHI); }
                    str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td colspan='4' class=''>- - - " + tenlinhvuc + "</td></tr>";
                    count_lv++;
                }
                string id_encr_kn = HashUtil.Encode_ID(i.IKIENNGHI.ToString(), url_cookie);
                
                string traloi_add_ = "<p class='tcenter'>" + EncodeOutput(i.TEN_DONVI_THAMQUYENKIENNGHI) + "</p>";
                if (i.ID_TRALOI != 0)
                {
                    traloi_add_ += Content_Traloi_kiennghi_Edit(i, null, url_cookie);
                }
                string chitiet_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết kiến nghị'><i class='icon-info-sign'></i></a>";
                string lichsu_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td class='tcenter'>" + count_kn + "</td><td><em>" + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_KIENNGHI), id_encr_kn) + 
                    "</em></td><td nowrap>" + traloi_add_ + "</td><td class='tcenter'>" + chitiet_kn  + "</td></tr>";
                id_tonghop = i.ITONGHOP;
                id_linhvuc = i.ID_LINHVUC_KIENNGHI;
                id_donvi = i.ID_THAMQUYEN_DONVI_TONGHOP;
                count_kn++;
            }

            return str;
        }
        public string KN_Tonghop_Bandannguyen_dachuyen(List<PRC_LIST_TONGHOP_KIENNGHI> tonghop, TaikhoanAtion act)
        {
            string str = "";
            //tonghop = tonghop.Where(x => x.ITINHTRANG > 0).ToList();
            if (tonghop.Count() == 0) { return "<tr><td colspan='3' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }

            string url_cookie = func.Get_Url_keycookie();
            List<decimal> list_id_coquan = new List<decimal>();
            int count = 1;int count_lv = 1;
            decimal id_donvi = 0; decimal id_linhvuc = -1; decimal id_kiennghi = 0;
            foreach (var q in tonghop)
            {
                if (id_donvi != q.ID_THAMQUYEN_DONVI_TONGHOP)
                {
                    id_linhvuc = -1; count_lv = 1;                    
                    str += "<tr><th colspan='3' class=''>" + EncodeOutput(q.TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper()) + "</th></tr>";
                }
                if (id_linhvuc != q.ID_LINHVUC_TONGHOP)
                {
                    string tenlinhvuc = q.TEN_LINHVUC_TONGHOP; if (q.ID_LINHVUC_TONGHOP == 0) { tenlinhvuc = "Lĩnh vực: Chưa xác định"; }
                    id_linhvuc = q.ID_LINHVUC_TONGHOP;
                    str += "<tr><td colspan='3' class='b'>- - " + count_lv + ". " + EncodeOutput(tenlinhvuc) + "</td></tr>";
                    count_lv++;
                }
                if (id_kiennghi!=q.ITONGHOP && q.SOKIENNGHI_TONGHOP>0 && q.ITONGHOP!=0){
                    string id_encr = HashUtil.Encode_ID(q.ITONGHOP.ToString(), url_cookie);
                    string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                    string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                    //string coquan_thamquyen = "";
                    //if (t.ITHAMQUYENDONVI != 0) { coquan_thamquyen = coquan.Where(x=>x.ICOQUAN==(int)t.ITHAMQUYENDONVI).FirstOrDefault().CTEN; }
                    str += "<tr ><td class='tcenter'>" + count + "</td><td>" + func.TomTatNoiDung(EncodeOutput(q.NOIDUNG_TONGHOP), id_encr) +
                    "</td><td class='tcenter' nowrap>" + chitiet + bt_lichsu + "</td></tr>";
                    count++;
                }

                id_donvi = q.ID_THAMQUYEN_DONVI_TONGHOP;
                id_kiennghi = q.ITONGHOP;
                //if (!list_id_coquan.Contains(q.ID_THAMQUYEN_DONVI_TONGHOP)) {
                //    list_id_coquan.Add(q.ID_THAMQUYEN_DONVI_TONGHOP);
                //    var tonghop1 = tonghop.Where(x => x.ID_THAMQUYEN_DONVI_TONGHOP ==q.ID_THAMQUYEN_DONVI_TONGHOP).ToList();
                //    str += "<tr><th colspan='3' class=''>" + q.TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper() + "</th></tr>";
                //    List<decimal> list_id_linhvuc = new List<decimal>();
                //    int count_linhvuc = 1;
                //    foreach (var l in tonghop1)
                //    {                        
                //        if (!list_id_linhvuc.Contains(l.ID_LINHVUC_TONGHOP))
                //        {
                //            string tenlinhvuc = l.TEN_LINHVUC_TONGHOP;
                //            var tonghop3 = tonghop1.Where(x => x.ID_LINHVUC_TONGHOP == l.ID_LINHVUC_TONGHOP).ToList();
                //            if (l.ID_LINHVUC_TONGHOP == 0) { tenlinhvuc = "Lĩnh vực: Chưa xác định"; }
                //            str += "<tr><td colspan='3' class='b'>- - " + count_linhvuc + ". " + tenlinhvuc + "</td></tr>";
                //            count_linhvuc++;

                //            foreach (var t in tonghop3)
                //            {
                //                string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
                //                string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                //                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                //                //string coquan_thamquyen = "";
                //                //if (t.ITHAMQUYENDONVI != 0) { coquan_thamquyen = coquan.Where(x=>x.ICOQUAN==(int)t.ITHAMQUYENDONVI).FirstOrDefault().CTEN; }
                //                str += "<tr ><td class='tcenter'>" + count + "</td><td>" + func.TomTatNoiDung(t.NOIDUNG_TONGHOP, id_encr) +
                //                "</td><td class='tcenter' nowrap>" + chitiet + bt_lichsu + "</td></tr>";
                //                count++;
                //            }
                //            list_id_linhvuc.Add(l.ID_LINHVUC_TONGHOP);
                //        }
                //    }
                //}                
            }

            return str;
        }
        public Boolean Update_TrangThai_Xuly_Kiennghi_diaphuong(int iTonghop)
        {
            bool result = true;
            KN_TONGHOP th = _kn.Get_Tonghop(iTonghop);
            if (th != null)
            {
                th.ITINHTRANG = (decimal)TrangThai_TongHop.DangXuLy;
                result=_kn.UpdateTongHop(th);
            }
            return result;
        }
        public string KN_Tonghop_Dangxuly_DonViThamQuyen_row(PRC_LIST_TONGHOP_KIENNGHI t, TaikhoanAtion act,string url_cookie,int count)
        {
            string str = "";
            string thongbao_quahan = "";
            if (t.SOKIENNGHI_TONGHOP > t.SOKIENNGHI_DATRALOI)
            {
                DateTime firstdate = Convert.ToDateTime("0001-01-01");
                if (t.NGAYDUKIENHOANTHANH.Date == firstdate.Date)
                {
                    t.NGAYDUKIENHOANTHANH = NgayKetThuc_DuKien((DateTime)t.NGAYBANHANH_VANBAN);
                }
                if (t.NGAYDUKIENHOANTHANH != null)
                {
                    if (t.NGAYDUKIENHOANTHANH < DateTime.Now.Date && t.ITINHTRANG == 4)
                    {
                        int songayquahan = Convert.ToInt32((DateTime.Now.Date - t.NGAYDUKIENHOANTHANH).TotalDays);
                        thongbao_quahan = "</br><span class='f-orangered'>[Quá hạn trả lời Tập hợp kiến nghị: " + songayquahan + " ngày]</span>";
                    }
                }
            }
            string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
            string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
            string so_kn = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiennghi_traloi/?id=" + id_encr + "\" title='Danh sách kiến nghị' rel='tooltip' title=''  class='trans_func'><i class='icon-list-ul'></i></a> ";
            string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
            if (!base_bussiness.Action_(50, act)) { bt_lichsu = ""; }
            str += "<tr><td class='tcenter b'>" + count + "</td><td>" + func.TomTatNoiDung(EncodeOutput(t.NOIDUNG_TONGHOP), id_encr) + thongbao_quahan +
            "</td><td class='tcenter' nowrap>" + so_kn + chitiet + bt_lichsu + "</td></tr>";
            return str;
        }
        //public string KN_Tonghop_Dangxuly_DonViThamQuyen(List<PRC_LIST_TONGHOP_KIENNGHI> tonghop, TaikhoanAtion act,string url_cookie,string type,int count_kn)
        //{
        //    //string str = "<tr><th colspan='3' class=''>" + tonghop.FirstOrDefault().TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper() + " (" + tonghop.Count() + ")</th></tr>";
        //    string str = "";
        //    //int count = 1;
        //    List<decimal> list_id_linhvuc = new List<decimal>();
        //    int count_lv = 1;
        //    tonghop = tonghop.OrderByDescending(x => x.ID_LINHVUC_TONGHOP).ToList();
        //    foreach (var l in tonghop)
        //    {
        //        if (!list_id_linhvuc.Contains(l.ID_LINHVUC_TONGHOP))
        //        {                    
        //            string tenlinhvuc = l.TEN_LINHVUC_TONGHOP;
        //            if (l.ID_LINHVUC_TONGHOP == 0) { tenlinhvuc = "Lĩnh vực: Chưa xác định"; }
        //            var tonghop1 = tonghop.Where(x => x.ID_LINHVUC_TONGHOP == l.ID_LINHVUC_TONGHOP).ToList();
        //            if (act.is_lanhdao)
        //            {
        //                str += "<tr><td class='b' colspan='3'>-- " + count_lv + ". " + tenlinhvuc + "</td></tr>";
        //            }else
        //            {
        //                str += "<tr><td class='b' colspan='3'>" + tenlinhvuc + "</td></tr>";
        //            }
                    
        //            count_lv++;
        //            //str += "<tbody>";
        //            foreach (var t in tonghop1)
        //            {
        //                if (type == "dangxuly")
        //                {
        //                    if (t.SOKIENNGHI_CHUATRALOI > 0)
        //                    {
        //                        str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(t, act, url_cookie, count_kn);
        //                    }
        //                }else
        //                {
        //                    if (t.SOKIENNGHI_DATRALOI >0)
        //                    {
        //                        str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(t, act, url_cookie, count_kn);
        //                    }
        //                }
        //                count_kn++;
        //            }
        //            //str += "</tbody>";
        //            list_id_linhvuc.Add(l.ID_LINHVUC_TONGHOP);
        //        }
        //    }
            
        //    return str;
        //}
        public string KN_Tonghop_Dangxuly(List<PRC_LIST_TONGHOP_KIENNGHI> tonghop, TaikhoanAtion act,string type="")
        {
            string str = "";
            if (tonghop.Count() == 0) { return "<tr><td colspan='3' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
            int count_kn = 1;
            string url_cookie = func.Get_Url_keycookie();
            decimal id_donvi = 0;decimal id_linhvuc = -1;decimal id_tonghop = 0;
            int count_lv = 1;
            if (!act.is_lanhdao)
            {
                foreach (var l in tonghop)
                {
                    if (id_linhvuc != l.ID_LINHVUC_TONGHOP)
                    {
                        string tenlinhvuc = l.TEN_LINHVUC_TONGHOP;
                        if (l.ID_LINHVUC_TONGHOP == 0) { tenlinhvuc = "Lĩnh vực: Chưa xác định"; }
                        str += "<tr><td class='b' colspan='3'>" + EncodeOutput(tenlinhvuc) + "</td></tr>";
                        id_linhvuc = l.ID_LINHVUC_TONGHOP;
                    }
                    if (id_tonghop != l.ITONGHOP && l.ITONGHOP!=0 && l.SOKIENNGHI_TONGHOP>0)
                    {
                        if (type == "dangxuly")
                        {
                            if (l.SOKIENNGHI_CHUATRALOI > 0)
                            {
                                str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(l, act, url_cookie, count_kn);
                            }
                        }
                        else
                        {
                            if (l.SOKIENNGHI_DATRALOI > 0)
                            {
                                str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(l, act, url_cookie, count_kn);
                            }
                        }
                        count_kn++;
                    }
                    id_tonghop = l.ITONGHOP;
                }
                

                //List<decimal> list_id_linhvuc = new List<decimal>();               
                //tonghop = tonghop.OrderByDescending(x => x.ID_LINHVUC_TONGHOP).ToList();
                //foreach (var l in tonghop)
                //{
                //    if (!list_id_linhvuc.Contains(l.ID_LINHVUC_TONGHOP))
                //    {
                //        string tenlinhvuc = l.TEN_LINHVUC_TONGHOP;
                //        if (l.ID_LINHVUC_TONGHOP == 0) { tenlinhvuc = "Lĩnh vực: Chưa xác định"; }
                //        var tonghop1 = tonghop.Where(x => x.ID_LINHVUC_TONGHOP == l.ID_LINHVUC_TONGHOP).ToList();
                //        if (act.is_lanhdao)
                //        {
                //            str += "<tr><td class='b' colspan='3'>-- " + count_lv + ". " + tenlinhvuc + "</td></tr>";
                //        }
                //        else
                //        {
                //            str += "<tr><td class='b' colspan='3'>" + tenlinhvuc + "</td></tr>";
                //        }

                //        count_lv++;
                //        //str += "<tbody>";
                //        foreach (var t in tonghop1)
                //        {
                //            if (type == "dangxuly")
                //            {
                //                if (t.SOKIENNGHI_CHUATRALOI > 0)
                //                {
                //                    str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(t, act, url_cookie, count_kn);
                //                }
                //            }
                //            else
                //            {
                //                if (t.SOKIENNGHI_DATRALOI > 0)
                //                {
                //                    str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(t, act, url_cookie, count_kn);
                //                }
                //            }
                //            count_kn++;
                //        }
                //        //str += "</tbody>";
                //        list_id_linhvuc.Add(l.ID_LINHVUC_TONGHOP);
                //    }
                //}

                return str;
            }
            str = "";
            List<decimal> list_id_donvi_thamquyen = new List<decimal>();            
            foreach (var i in tonghop)
            {
                if (id_donvi != i.ID_THAMQUYEN_DONVI_TONGHOP)
                {
                    str += "<tr><th colspan='3' class='b'>" + EncodeOutput(i.TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper()) + "</th></tr>";
                    count_lv = 1;id_linhvuc = -1;
                }
                if (id_linhvuc != i.ID_LINHVUC_TONGHOP)
                {
                    string tenlinhvuc = i.TEN_LINHVUC_TONGHOP;
                    if (i.ID_LINHVUC_TONGHOP == 0) { tenlinhvuc = "Lĩnh vực: Chưa xác định"; }
                    str += "<tr><td class='b' colspan='3'>-- " + count_lv + ". " + EncodeOutput(tenlinhvuc) + "</td></tr>";
                    count_lv ++;
                    id_linhvuc = i.ID_LINHVUC_TONGHOP;
                }
                if (id_tonghop != i.ITONGHOP && i.ITONGHOP != 0 && i.SOKIENNGHI_TONGHOP > 0)
                {
                    //if (type == "dangxuly")
                    //{
                    //    if (i.SOKIENNGHI_CHUATRALOI > 0)
                    //    {
                    //        str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(i, act, url_cookie, count_kn);
                    //    }
                    //    str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(i, act, url_cookie, count_kn);
                    //}
                    //else
                    //{
                    //    if (i.SOKIENNGHI_DATRALOI > 0)
                    //    {
                    //        str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(i, act, url_cookie, count_kn);
                    //    }
                    //}
                    str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(i, act, url_cookie, count_kn);
                    count_kn++;
                }
                //id_tonghop = i.ITONGHOP;
                id_donvi = i.ID_THAMQUYEN_DONVI_TONGHOP;
                //if (!list_id_donvi_thamquyen.Contains(i.ID_THAMQUYEN_DONVI_TONGHOP))
                //{
                //    var tonghop2 = tonghop.Where(x => x.ID_THAMQUYEN_DONVI_TONGHOP == i.ID_THAMQUYEN_DONVI_TONGHOP).ToList();
                //    str += "<tr><th colspan='3' class='b'>" + tonghop2.FirstOrDefault().TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper() + "</th></tr>";
                //    //str += KN_Tonghop_Dangxuly_DonViThamQuyen(tonghop2, act, url_cookie,type, count_kn);
                //    list_id_donvi_thamquyen.Add(i.ID_THAMQUYEN_DONVI_TONGHOP);
                //    List<decimal> list_id_linhvuc = new List<decimal>();

                //    tonghop2 = tonghop2.OrderByDescending(x => x.ID_LINHVUC_TONGHOP).ToList();
                //    foreach (var l in tonghop2)
                //    {
                //        if (!list_id_linhvuc.Contains(l.ID_LINHVUC_TONGHOP))
                //        {
                //            string tenlinhvuc = l.TEN_LINHVUC_TONGHOP;
                //            if (l.ID_LINHVUC_TONGHOP == 0) { tenlinhvuc = "Lĩnh vực: Chưa xác định"; }
                //            var tonghop1 = tonghop2.Where(x => x.ID_LINHVUC_TONGHOP == l.ID_LINHVUC_TONGHOP).ToList();
                //            if (act.is_lanhdao)
                //            {
                //                str += "<tr><td class='b' colspan='3'>-- " + count_lv + ". " + tenlinhvuc + "</td></tr>";
                //            }
                //            else
                //            {
                //                str += "<tr><td class='b' colspan='3'>" + tenlinhvuc + "</td></tr>";
                //            }

                //            count_lv++;
                //            //str += "<tbody>";
                //            foreach (var t in tonghop1)
                //            {
                //                if (type == "dangxuly")
                //                {
                //                    if (t.SOKIENNGHI_CHUATRALOI > 0)
                //                    {
                //                        str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(t, act, url_cookie, count_kn);
                //                    }
                //                }
                //                else
                //                {
                //                    if (t.SOKIENNGHI_DATRALOI > 0)
                //                    {
                //                        str += KN_Tonghop_Dangxuly_DonViThamQuyen_row(t, act, url_cookie, count_kn);
                //                    }
                //                }
                //                count_kn++;
                //            }
                //            //str += "</tbody>";
                //            list_id_linhvuc.Add(l.ID_LINHVUC_TONGHOP);
                //        }
                //    }
                //}
            }
            return str;
        }
        public string KN_Tonghop_ChuaTraLoi(List<PRC_LIST_TONGHOP_KIENNGHI> tonghop, TaikhoanAtion act)
        {
            string str = "";
            if (tonghop.Count() == 0) { return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
            
            string url_cookie = func.Get_Url_keycookie();
            decimal id_donvi = 0; decimal id_linhvuc = -1; decimal id_tonghop = 0;
            int count_lv = 1;int count_tonghop = 1; int count_kn = 1;            
            foreach (var i in tonghop)
            {
                if (id_donvi != i.ID_THAMQUYEN_DONVI_TONGHOP && act.is_lanhdao)
                {
                    str += "<tr><th colspan='5' class='b'>" + EncodeOutput(i.TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper()) + "</th></tr>";
                    count_lv = 1; id_linhvuc = -1;
                }
                string id_encr_th = HashUtil.Encode_ID(i.ITONGHOP.ToString(), url_cookie);
                if (id_tonghop != i.ITONGHOP)
                {
                    string edit_cv = "<a onclick=\"ShowPopUp('id=" + id_encr_th + "','/Kiennghi/Ajax_Sua_congvan')\" href='javascript:void()' title='Sửa công văn' rel='tooltip' class='trans_func'><i class='icon-edit'></i></a>";
                    //string id_encr = HashUtil.Encode_ID(i.ITONGHOP.ToString(), url_cookie);
                    string danhsach_kiennghi="<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_traloi/?id="+ id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Danh sách kiến nghị'><i class='icon-list-ul'></i></a>";
                    string chitiet = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_info/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết Tập hợp'><i class='icon-info-sign'></i></a>";
                    string lichsu = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                    string space = "";
                    string icon = "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='plus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\"><i class='icon-plus'></i></a>" +
                        "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='minus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\" style='display:none'><i class='icon-minus'></i></a>";
                    if (i.SOKIENNGHI_TONGHOP == 0) { icon = "<i class='icon-plus f-grey'></i> "; }
                    str += "<tr><td class='' colspan='4'>"+ icon + " " + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_TONGHOP), id_encr_th) +
                        "</td><td class=\"tcenter\" nowrap>"+danhsach_kiennghi+chitiet+edit_cv+lichsu+"</td></tr>";
                    count_lv = 1; id_linhvuc = -1; count_tonghop++;
                }
                if (id_linhvuc != i.ID_LINHVUC_KIENNGHI)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (i.ID_LINHVUC_KIENNGHI != 0) { tenlinhvuc = "Lĩnh vực: "+EncodeOutput(i.TEN_LINHVUC_KIENNGHI); }
                    str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td colspan='4' class=''>- - - " + tenlinhvuc + "</td></tr>";
                    count_lv++;
                }

                string id_encr_kn = HashUtil.Encode_ID(i.IKIENNGHI.ToString(), url_cookie);
                string tiepnhan = EncodeOutput(i.TEN_DONVITIEPNHAN_KIENNGHI);
                if (i.ID_GOP < 0) { tiepnhan = EncodeOutput(i.TENDONVITIEPNHAN_GOP); }
                tiepnhan = "</br><em><strong>Tiếp nhận:</strong> " + tiepnhan+ "</em>";
                string traloi_add_ = Btn_DLL_Traloi_kiennghi(id_encr_kn);
                if (!base_bussiness.ActionMulty_("7,50", act)) { traloi_add_ = ""; }
                string chitiet_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết kiến nghị'><i class='icon-info-sign'></i></a>";
                string lichsu_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                string edit = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Sua?id=" + id_encr_kn + "\" title='Sửa kiến nghị' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td class='tcenter'>" + count_kn + "</td><td><em>" + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_KIENNGHI), id_encr_kn) + tiepnhan + "</em></td><td nowrap>"
                    ;
                if (i.ICANHBAO != null)
                {
                    if (i.ICANHBAO == 2)
                        str += "<i class='f-red'>Quá hạn</i>";
                    if (i.ICANHBAO == 1)
                        str += "Sắp đến hạn";
                    if (i.ICANHBAO == 0)
                        str += "Trong hạn";
                }
                str += "</em></td><td nowrap>" + traloi_add_ + "</td><td class='tcenter'>" + chitiet_kn + lichsu_kn + edit + "</td></tr>";
                id_tonghop = i.ITONGHOP;
                id_linhvuc = i.ID_LINHVUC_KIENNGHI;
                id_donvi = i.ID_THAMQUYEN_DONVI_TONGHOP;
                count_kn++;
            }
            return str;
        }
        public string KN_Tonghop_DangTraLoi(List<PRC_LIST_TONGHOP_KIENNGHI> tonghop, TaikhoanAtion act)
        {
            string str = "";
            if (tonghop.Count() == 0) { return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }

            string url_cookie = func.Get_Url_keycookie();
            decimal id_donvi = 0; decimal id_linhvuc = -1; decimal id_tonghop = 0;
            int count_lv = 1; int count_tonghop = 1; int count_kn = 1;
            foreach (var i in tonghop)
            {
                if (id_donvi != i.ID_THAMQUYEN_DONVI_TONGHOP && act.is_lanhdao)
                {
                    str += "<tr><th colspan='5' class='b'>" + EncodeOutput(i.TEN_THAMQUYEN_DONVI_TONGHOP.ToUpper()) + "</th></tr>";
                    count_lv = 1; id_linhvuc = -1;
                }
                string id_encr_th = HashUtil.Encode_ID(i.ITONGHOP.ToString(), url_cookie);
                if (id_tonghop != i.ITONGHOP)
                {
                    string edit_cv = "<a onclick=\"ShowPopUp('id=" + id_encr_th + "','/Kiennghi/Ajax_Sua_congvan')\" href='javascript:void()' title='Sửa công văn' rel='tooltip' class='trans_func'><i class='icon-edit'></i></a>";
                    string danhsach_kiennghi = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_traloi/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Danh sách kiến nghị'><i class='icon-list-ul'></i></a>";
                    string chitiet = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_info/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết Tập hợp'><i class='icon-info-sign'></i></a>";
                    string lichsu = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                    string space = "";
                    string icon = "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='plus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\"><i class='icon-plus'></i></a>" +
                        "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='minus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\" style='display:none'><i class='icon-minus'></i></a>";
                    if (i.SOKIENNGHI_TONGHOP == 0) { icon = "<i class='icon-plus f-grey'></i> "; }
                    str += "<tr><td class='' colspan='4'>" + icon + " " + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_TONGHOP), id_encr_th) +
                        "</td><td class=\"tcenter\" nowrap>" + danhsach_kiennghi + chitiet +edit_cv + lichsu + "</td></tr>";
                    count_lv = 1; id_linhvuc = -1; count_tonghop++;
                }
                if (id_linhvuc != i.ID_LINHVUC_KIENNGHI)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (i.ID_LINHVUC_KIENNGHI != 0) { tenlinhvuc = "Lĩnh vực: "+EncodeOutput(i.TEN_LINHVUC_KIENNGHI); }
                    str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td colspan='4' class=''>- - - " + tenlinhvuc + "</td></tr>";
                    count_lv++;
                }

                string id_encr_kn = HashUtil.Encode_ID(i.IKIENNGHI.ToString(), url_cookie);
                string tiepnhan = i.TEN_DONVITIEPNHAN_KIENNGHI;
                if (i.ID_GOP < 0) { tiepnhan = i.TENDONVITIEPNHAN_GOP; }
                tiepnhan = "</br><em><strong>Tiếp nhận:</strong> " + tiepnhan+ "</em>";
                string traloi_add_ = Btn_DLL_Traloi_kiennghi(id_encr_kn);
                if (!base_bussiness.ActionMulty_("7,50", act)) { traloi_add_ = ""; }
                if (i.ID_TRALOI != 0)
                {
                    traloi_add_ = Content_Traloi_kiennghi_Edit(i, act, url_cookie);
                }
                string edit = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Sua?id=" + id_encr_kn + "\" title='Sửa kiến nghị' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string chitiet_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết kiến nghị'><i class='icon-info-sign'></i></a>";
                string lichsu_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td class='tcenter'>" + count_kn + "</td><td style='word-wrap: break-word'><em>" + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_KIENNGHI), id_encr_kn) + tiepnhan + "</em></td><td>";
                if (i.ICANHBAO != null)
                {
                    if (i.ICANHBAO == 2)
                        str += "<i class='f-red'>Quá hạn</i>";
                    if (i.ICANHBAO == 1)
                        str += "Sắp đến hạn";
                    if (i.ICANHBAO == 0)
                        str += "Trong hạn";
                }
                str += "</em></td><td style='word-wrap: break-word'>" + traloi_add_ + "</td><td class='tcenter' style='word-wrap: break-word'>" + chitiet_kn + lichsu_kn + edit + "</td></tr>";
                id_tonghop = i.ITONGHOP;
                id_linhvuc = i.ID_LINHVUC_KIENNGHI;
                id_donvi = i.ID_THAMQUYEN_DONVI_TONGHOP;
                count_kn++;
            }
            return str;
        }
        public string KN_Tonghop_Datraloi(List<KN_TONGHOP> tonghop, List<QUOCHOI_COQUAN> coquan, List<KN_VANBAN> vanban, TaikhoanAtion act)
        {
            string str = "";
            Quochoi_CoquanRepository _coquan = new Quochoi_CoquanRepository();
            if (tonghop.Count() == 0) { return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
            //int count = 1;
            string url_cookie = func.Get_Url_keycookie();
            coquan = coquan.OrderBy(x => x.CTEN).ToList();
            foreach (var c in coquan)
            {
                var tonghop1 = tonghop.Where(x => x.ITHAMQUYENDONVI == (int)c.ICOQUAN).ToList();
                if (tonghop1.Count() > 0)
                {
                    str += "<tr><th colspan='4' class=''>" + EncodeOutput(c.CTEN.ToUpper()) + " (" + tonghop1.Count() + ")</th></tr>";
                    int count = 1;
                    foreach (var t in tonghop1)
                    {
                        // new id 
                        string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
                        string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                        string traloi_edit = "";
                        string so_kn = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiennghi_traloi/?id=" + id_encr + "\" title='Danh sách kiến nghị' rel='tooltip' title=''  class='trans_func'><i class='icon-list-ul'></i></a> ";
                        string traloi = "";
                        string bt_del = " ";
                        string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
                        if (vanban.Where(x => x.ITONGHOP == (int)t.ITONGHOP).Count() > 0)
                        {
                            KN_VANBAN vb = vanban.Where(x => x.ITONGHOP == (int)t.ITONGHOP).FirstOrDefault();
                            string id_encr_vanban = HashUtil.Encode_ID(vb.IVANBAN.ToString(), url_cookie);
                            traloi = "<p>Công văn số <strong>" + vb.CSOVANBAN + "</strong> ban hành ngày " +
                                func.ConvertDateVN(vb.DNGAYBANHANH.ToString()) + ". " + File_View((int)t.ITONGHOP, "tonghop_traloi") + "</p>";
                            traloi_edit = " <a onclick=\"ShowPopUp('id=" + id_encr_vanban + "','/Kiennghi/Ajax_Tonghop_traloi_edit')\" href='javascript:void()' title='Sửa trả lời' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a>";
                            bt_del = "<a href='javascript:void(0)' onclick=\"Delete_Refresh('" + id_encr_vanban + "','/Kiennghi/Ajax_Tonghop_traloi_del')\" title='Xóa trả lời' rel='tooltip' title='' class='trans_func'><i class='icon-trash'></i></a>";
                            if (vb.IUSER != act.iUser) { traloi_edit = ""; bt_del = ""; }
                        }

                        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + func.TomTatNoiDung(EncodeOutput(t.CNOIDUNG), id_encr) +
                        "</td><td>" + traloi + "</td><td class='tcenter' nowrap>" + so_kn + traloi_edit + bt_del + bt_lichsu + chitiet + "</td></tr>";
                        count++;
                    }
                }
            }
            return str;
        }
        public string KN_Tonghop_bandannguyen_chuyen(List<PRC_LIST_TONGHOP_KIENNGHI> tonghop, TaikhoanAtion act)
        {
            //string str = "";
            //string url_cookie = func.Get_Url_keycookie();
            //List<decimal> list_id_linhvuc = new List<decimal>();
            //tonghop = tonghop.OrderByDescending(x => x.ID_LINHVUC_TONGHOP).ToList();
            //int count = 1;
            //foreach (var l in tonghop)
            //{
            //    if (!list_id_linhvuc.Contains(l.ID_LINHVUC_TONGHOP))
            //    {
            //        list_id_linhvuc.Add(l.ID_LINHVUC_TONGHOP);
            //        var tonghop1 = tonghop.Where(x => x.ID_LINHVUC_TONGHOP == l.ID_LINHVUC_TONGHOP).ToList();
            //        string tenlinhvuc = "Lĩnh vực: Chưa xác định";
            //        if (l.ID_LINHVUC_TONGHOP != 0) { tenlinhvuc = tonghop1.FirstOrDefault().TEN_LINHVUC_TONGHOP; }
            //        str += "<tr><th colspan='3' class=''>" + EncodeOutput(tenlinhvuc) + "</th></tr>";

            //        List<decimal> list_id_tonghop = new List<decimal>();
            //        foreach (var t in tonghop1)
            //        {
            //            if (!list_id_tonghop.Contains(t.ITONGHOP))
            //            {
            //                list_id_tonghop.Add(t.ITONGHOP);
            //                string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
            //                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "'title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
            //                string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
            //                string chuyen = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Chuyen_tonghop_xuly/?id=" + id_encr + "\" title='Chuyển xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-signin'></i></a> ";
            //                if (!base_bussiness.Action_(50, act)) { bt_lichsu = ""; }
            //                if (!base_bussiness.ActionMulty_("7,50", act)) { chuyen = ""; }
            //                str += "<tr><td class='tcenter'>" + count + "</td><td>" + func.TomTatNoiDung(EncodeOutput(t.NOIDUNG_TONGHOP), id_encr) +
            //                "</td><td class='tcenter' nowrap>" + chitiet + bt_lichsu + chuyen + "</td></tr>";
            //                count++;
            //            }
            //            // new id                         
            //        }

            //    }
            //}         
            //return str;
            string str = "";
            if (tonghop.Count() == 0) { return "<tr><td colspan='3' class='alert-danger tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }

            string url_cookie = func.Get_Url_keycookie();
            decimal id_donvi = 0; decimal id_linhvuc = -1; decimal id_tonghop = 0;
            int count_lv = 1; int count_tonghop = 1; int count_kn = 1;
            foreach (var i in tonghop)
            {
                string id_encr_th = HashUtil.Encode_ID(i.ITONGHOP.ToString(), url_cookie);
                
                if (id_tonghop != i.ITONGHOP)
                {
                    
                    string chuyen = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Chuyen_tonghop_xuly/?id=" + id_encr_th + "\" title='Chuyển xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-signin'></i></a> ";
                    if (!base_bussiness.ActionMulty_("7,50", act)) { chuyen = ""; }
                    string danhsach_kiennghi = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_traloi/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Danh sách kiến nghị'><i class='icon-list-ul'></i></a>";
                    string chitiet = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_info/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết Tập hợp'><i class='icon-info-sign'></i></a>";
                    string lichsu = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Tonghop_lichsu/?id=" + id_encr_th + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                    string space = "";
                    string icon = "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='plus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\"><i class='icon-plus'></i></a>" +
                        "<a href='javascript:void(0)' title='Xem danh sách kiến nghị' rel='tooltip' title='' class='trans_func' id='minus_" + id_encr_th + "' onclick=\"$('.tr_" + id_encr_th + ",#minus_" + id_encr_th + ",#plus_" + id_encr_th + "').toggle();\" style='display:none'><i class='icon-minus'></i></a>";
                    if (i.SOKIENNGHI_TONGHOP == 0) { icon = "<i class='icon-plus f-grey'></i> "; }
                    str += "<tr><td colspan='2'>"+ icon+" " + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_TONGHOP), id_encr_th) +
                        "</td><td class=\"tcenter\" nowrap>" + chuyen + chitiet + lichsu + "</td></tr>";
                    count_lv = 1; id_linhvuc = -1; count_tonghop++;
                }
                if (id_linhvuc != i.ID_LINHVUC_KIENNGHI)
                {
                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                    if (i.ID_LINHVUC_KIENNGHI != 0) { tenlinhvuc = "Lĩnh vực: " + EncodeOutput(i.TEN_LINHVUC_KIENNGHI); }
                    str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td colspan='3' class=''>- - - " + tenlinhvuc + "</td></tr>";
                    count_lv++;
                }

                string id_encr_kn = HashUtil.Encode_ID(i.IKIENNGHI.ToString(), url_cookie);
                string tiepnhan = EncodeOutput(i.TEN_DONVITIEPNHAN_KIENNGHI);
                if (i.ID_GOP < 0) { tiepnhan = EncodeOutput(i.TENDONVITIEPNHAN_GOP); }
                tiepnhan = "</br><em><strong>Tiếp nhận:</strong> " + tiepnhan + "</em>";
                
                string chitiet_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Chi tiết kiến nghị'><i class='icon-info-sign'></i></a>";
                string lichsu_kn = "<a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu/?id=" + id_encr_kn + "' title='' rel='tooltip' class='trans_func' data-original-title='Lịch sử xử lý'><i class='icon-time'></i></a>";
                str += "<tr class='tr_" + id_encr_th + "' style='display:none'><td class='tcenter'>" + count_kn + "</td><td><em>" + func.TomTatNoiDung(EncodeOutput(i.NOIDUNG_KIENNGHI), id_encr_kn) + tiepnhan + "</em></td><td class='tcenter'>" + chitiet_kn + lichsu_kn + "</td></tr>";
                id_tonghop = i.ITONGHOP;
                id_linhvuc = i.ID_LINHVUC_KIENNGHI;
                id_donvi = i.ID_THAMQUYEN_DONVI_TONGHOP;
                count_kn++;
            }
            return str;
        }

        public string KN_Tonghop_Chuyen_BDN(List<KN_TONGHOP> tonghop, TaikhoanAtion act)
        {
            string str = "";
            Quochoi_CoquanRepository _coquan = new Quochoi_CoquanRepository();
            if (tonghop.Count() == 0) { return "<tr><td colspan='3' class='alert tcenter'>Không tìm thấy Tập hợp kiến nghị nào</td></tr>"; }
            int count = 1;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var t in tonghop)
            {
                // new id 
                string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
                string bt_lichsu = " <a href='/Kiennghi/Tonghop_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";

                //TongHop_Kiennghi th = Tonghop_Detail((int)t.ITONGHOP, id_encr);
                // end
                string chitiet = " <a onclick=\"ShowPageLoading()\"  href=\"/Kiennghi/Tonghop_info?id=" + id_encr + "\" title='Xem chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";
                string coquan_thamquyen = "";
                if (t.ITHAMQUYENDONVI != 0) { coquan_thamquyen = _coquan.GetByID((int)t.ITHAMQUYENDONVI).CTEN; }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td>" + func.TomTatNoiDung(EncodeOutput(t.CNOIDUNG), id_encr) +
                "</td><td class='tcenter b'>" +
                EncodeOutput(coquan_thamquyen) + "</td><td class='tcenter' nowrap>" + chitiet + bt_lichsu + "</td></tr>";
                count++;
            }
            return str;
        }
        public TongHop_Kiennghi Tonghop_Detail(int id, string id_encr)
        {
            // new id 
            //string id_encr = HashUtil.Encode_ID(id.ToString());
            // end
            TongHop_Kiennghi kn = new TongHop_Kiennghi();
            KN_TONGHOP don = _kn.Get_Tonghop(id);
            kn.truockkyhop = Get_Ten_TruocKyHop((int)don.ITRUOCKYHOP);
            //if (don.ITRUOCKYHOP == 0)
            //{
            //    kn.truockkyhop = "Sau kỳ họp";
            //}
            LINHVUC_COQUAN linhvuc = _kn.Get_LinhVuc_CoQuan((int)don.ILINHVUC);
            if (linhvuc != null)
            {
                kn.linhvuc = EncodeOutput(linhvuc.CTEN);
            }
            else
            {
                kn.linhvuc = "Nhiều lĩnh vực liên quan";
            }
            QUOCHOI_KYHOP kyhop = _kn.Get_KyHop_QuocHoi((int)don.IKYHOP);
            if (kyhop != null)
            {
                kn.kyhop = EncodeOutput(kyhop.CTEN);
                if (_kn.Get_Khoa_QuocHoi((int)kyhop.IKHOA) != null)
                {
                    kn.khoahop = EncodeOutput(_kn.Get_Khoa_QuocHoi((int)kyhop.IKHOA).CTEN);
                }
                
            }
            QUOCHOI_COQUAN coquan_tonghop = _kn.HienThiThongTinCoQuan((int)don.IDONVITONGHOP);
            if (coquan_tonghop != null)
            {
                kn.donvi_tonghop = EncodeOutput(coquan_tonghop.CTEN);
            }
            QUOCHOI_COQUAN coquan_thamquyen = _kn.HienThiThongTinCoQuan((int)don.ITHAMQUYENDONVI);
            if (coquan_thamquyen != null)
            {
                kn.donvi_thamquyen = EncodeOutput(coquan_thamquyen.CTEN);
            }
            else
            {
                kn.donvi_thamquyen = "Chưa xác định";
            }
            kn.bt_info = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Tonghop_info')\" title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i> Chi tiết</a>";
            kn.bt_lichsu = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Tonghop_lichsu')\" title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i> Lịch sử xử lý</a>";
            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.ChoXuLy)
            {
                kn.tinhtrang = StringEnum.GetStringValue(TrangThai_TongHop.ChoXuLy);
            }
            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.DaChuyenBanDanNguyen)
            {
                kn.tinhtrang = StringEnum.GetStringValue(TrangThai_TongHop.DaChuyenBanDanNguyen);
            }
            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen)
            {
                kn.tinhtrang = "Ban Dân nguyện đã chuyển đến " + kn.donvi_thamquyen;
            }
            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.DangXuLy)
            {
                kn.tinhtrang = kn.donvi_thamquyen + " đang xử nghiên cứu, xử lý";
            }
            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.DaTraLoi)
            {
                kn.tinhtrang = "Đã có kết quả trả lời từ " + kn.donvi_thamquyen;
                string traloi = "";
                _condition = new Dictionary<string, object>();
                _condition.Add("ITONGHOP", id);
                _condition.Add("CLOAI", "tonghop_traloi");
                KN_VANBAN t = _kn.GetAll_VanbanByParam(_condition).FirstOrDefault();
                traloi = "Văn bản số <strong>" + EncodeOutput(t.CSOVANBAN) + "</strong> ban hành ngày " + func.ConvertDateVN(t.DNGAYBANHANH.ToString()) +
                    " Vv " + EncodeOutput(t.CNOIDUNG) + ". " + File_View((int)t.IVANBAN, "traloi_tonghop");
                kn.tr_traloi = "<div class='row-fluid'><div class='control-group'><label class='control-label b'>Kết quả trả lời</label><div class='controls'>" + traloi + "</div></div></div>";
            }

            //if (don.ITINHTRANG == 3)
            //{
            //    kn.tinhtrang = "Đã hoàn thành";
            //    if (don.ITINHTRANG == 4) { kn.tinhtrang = "Tiếp tục theo dõi"; }
            //    string traloi = "";
            //    _condition = new Dictionary<string, object>();
            //    _condition.Add("ITONGHOP", id);
            //    _condition.Add("CLOAI", "traloi_tonghop");
            //    KN_VANBAN t = _kn_vanban.GetAll(_condition).FirstOrDefault();
            //    traloi = "Văn bản trả lời số <strong>" + t.CSOVANBAN + "</strong> ban hành ngày " + func.ConvertDateVN(t.DNGAYBANHANH.ToString()) +
            //        " Vv " + t.CNOIDUNG;
            //    if (t.CFILE != "")
            //    {
            //        traloi += " <a href='" + t.CFILE + "'><i class='icon-download-alt'></i></a>";
            //    }
            //    kn.tr_traloi = "<tr><td class='b'>Trả lời</td><td colspan='3'>" + traloi + "</td></tr>";
            //}
            return kn;
        }
        public string Str_tonghop_tinhtrang(KN_TONGHOP don)
        {
            string str = "";
            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.ChoXuLy)
            {
                str = StringEnum.GetStringValue(TrangThai_TongHop.ChoXuLy);
            }

            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen)
            {
                str = StringEnum.GetStringValue(TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen);
            }
            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.DangXuLy)
            {
                str = StringEnum.GetStringValue(TrangThai_TongHop.DangXuLy);
            }
            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.DaTraLoi)
            {
                str = "<p>"+ StringEnum.GetStringValue(TrangThai_TongHop.DaTraLoi) +"</p> ";
                string traloi = "";
                _condition = new Dictionary<string, object>();
                _condition.Add("ITONGHOP", (int)don.ITONGHOP);
                _condition.Add("CLOAI", "tonghop_traloi");
                KN_VANBAN t = _kn.GetAll_VanbanByParam(_condition).FirstOrDefault();
                traloi = "Văn bản số <strong>" + EncodeOutput(t.CSOVANBAN) + "</strong> ban hành ngày " + func.ConvertDateVN(t.DNGAYBANHANH.ToString()) +
                    " Vv " + EncodeOutput(t.CNOIDUNG) + ". " + File_View((int)t.IVANBAN, "traloi_tonghop");
                str += traloi;
            }
            return str;
        }
        public string Row_tonghop_tinhtrang(PRC_LIST_TONGHOP_KIENNGHI don)
        {
            string str = "";
            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.ChoXuLy)
            {
                str = StringEnum.GetStringValue(TrangThai_TongHop.ChoXuLy);
            }

            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen)
            {
                str = StringEnum.GetStringValue(TrangThai_TongHop.DaChuyenDenDonViCoThamQuyen);
            }
            if (don.ITINHTRANG == (decimal)TrangThai_TongHop.DangXuLy)
            {
                str = StringEnum.GetStringValue(TrangThai_TongHop.DangXuLy);
            }
            if (don.SOKIENNGHI_CHUATRALOI == 0)
            {
                str= StringEnum.GetStringValue(TrangThai_TongHop.DaTraLoi);
            }
            return str;
        }
        public string CheckBox_Tinh_Huyen_TiepXuc(int iDonVi, string diaphuongchon)
        {
            string str = "";
            Quochoi_CoquanRepository _quochoi_coquan = new Quochoi_CoquanRepository();
            DiaPhuongRepository _diaphuong = new DiaPhuongRepository();

            QUOCHOI_COQUAN coquan = _quochoi_coquan.GetByID(iDonVi);
            Dictionary<string, object> _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IPARENT", (int)coquan.IDIAPHUONG);
            _dCondition.Add("IHIENTHI", 1);
            var huyen = _diaphuong.GetAll(_dCondition).OrderBy(x => x.CTEN).ToList();
            foreach (var h in huyen)
            {
                string check = "";
                if (diaphuongchon != "")
                {
                    diaphuongchon = "," + diaphuongchon + ",";
                    if (diaphuongchon.IndexOf("," + h.IDIAPHUONG + ",") != -1)
                    {
                        check = " checked ";
                    }
                }
                str += "<p style='width:50%; float:left;'><input " + check + " type='checkbox' name='huyen' value='" + h.IDIAPHUONG + "' class='nomargin'/> " + EncodeOutput(h.CTYPE) + " " + EncodeOutput(h.CTEN) + "</p>";
            }
            return str;
        }
        public string CheckBox_DaiBieu_TiepXuc(string daibieuchon, int iDonVi)
        {
           QUOCHOI_COQUAN coquan = _kn.HienThiThongTinCoQuan(iDonVi);
            Dictionary<string, object> _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IDIAPHUONG", (int)coquan.IDIAPHUONG);
            _dCondition.Add("IDELETE", 0); _dCondition.Add("IHIENTHI", 1);
            List<DAIBIEU> daibieu = _kn.GetAll_Daibieu(_dCondition);
            string str = "";
            foreach (var d in daibieu)
            {
                string check = "", truongdoan = "";
                if (d.ITRUONGDOAN == 1) { truongdoan = " (Trưởng đoàn)"; }
                if (daibieuchon != "")
                {
                    daibieuchon = "," + daibieuchon + ",";
                    if (daibieuchon.IndexOf("," + d.IDAIBIEU + ",") != -1)
                    {
                        check = " checked ";
                    }
                }
                str += "<p style='width:50%; float:left;'><input " + check + " type='checkbox' name='daibieu' value='" + d.IDAIBIEU +
                    "' class='nomargin'/> " + EncodeOutput(d.CTEN) + " " + truongdoan + "</p>";
            }
            return str;
        }
        //public string KN_Tonghop_chuyen(int iUser, string sql)
        //{
        //    string str = "";
        //    var tonghop = _kn_tonghop.GetList(sql).OrderByDescending(x => x.ITONGHOP).ToList();
        //    int count = 1;
        //    string url_cookie = func.Get_Url_keycookie();
        //    foreach (var t in tonghop)
        //    {
        //        // new id 
        //        string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
        //        // end
        //        TongHop_Kiennghi th = Tonghop_Detail((int)t.ITONGHOP, id_encr);

        //        string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Chuyen_Xuly_tonghop')\" href='javascript:void()' title='Chuyển xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
        //        string sql1 = "select * from KN_KIENNGHI where ITONGHOP =" + t.ITONGHOP + " AND ITINHTRANG >= 3";

        //        int kiennghi_traloi = _kiennghi.GetList(sql1).Count();
        //        _condition = new Dictionary<string, object>();
        //        _condition.Add("ITONGHOP", t.ITONGHOP);
        //        int sokiennghi = _kiennghi.GetAll(_condition).Count();
        //        string class_traloi = " btn-primary";
        //        if (kiennghi_traloi == sokiennghi) { class_traloi = " btn-success"; }
        //        string so_kn = " <a title='Xem trả lời kiến nghị' rel='tooltip' title='' href=\"/Kiennghi/Tonghop_traloi_kiennghi?id=" + id_encr + "\"  class='btn " + class_traloi + "'>" + kiennghi_traloi + " / " + sokiennghi + "</a> ";

        //        if (t.ILINHVUC == 0) { th.linhvuc = "<span class='f-orangered'>" + th.linhvuc + "</span>"; }
        //        if (t.ICHUONGTRINH == 0) { th.kehoach = "<span class='f-orangered'>" + th.kehoach + "</span>"; }
        //        if (t.ITHAMQUYENDONVI == 0) { th.donvi_thamquyen = "<span class='f-orangered'>" + th.donvi_thamquyen + "</span>"; }
        //        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter' nowrap></td><td>" + t.CNOIDUNG +
        //        "<p><strong>Lĩnh vực:</strong> " + th.linhvuc + "; <strong>Kế hoạch:</strong> " + th.kehoach + "</p>" + th.bt_info + th.bt_lichsu +
        //        "</td><td class='tcenter b'>" +
        //        th.donvi_thamquyen + "</td><td class='tcenter'>" + so_kn + "</td></tr>";

        //        count++;
        //    }
        //    return str;
        //}
        //public string KN_Dangxuly(int iUser, string sql)
        //{
        //    string str = "";
        //    var tonghop = _kn_tonghop.GetList(sql).OrderByDescending(x => x.ITONGHOP).ToList();
        //    int count = 1;
        //    string url_cookie = func.Get_Url_keycookie();
        //    foreach (var t in tonghop)
        //    {

        //        // new id 
        //        string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
        //        // end
        //        TongHop_Kiennghi th = Tonghop_Detail((int)t.ITONGHOP, id_encr);
        //        string chuyen_xuly = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Chuyen_Xuly_tonghop')\" href='javascript:void()' title='Chuyển xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-signout'></i></a>";
        //        string traloi = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Tonghop_traloi')\" href='javascript:void()' title='Trả lời' rel='tooltip' title='' class='trans_func'><i class='icon-file-alt'></i></a>";
        //        if (!_base.Action(7, iUser))
        //        {
        //            traloi = "";
        //        }
        //        else
        //        {
        //            if (_base.IDDonVi_User(iUser) != t.ITHAMQUYENDONVI && !_base.IsAdmin(iUser))
        //            {
        //                traloi = "";
        //            }
        //        }
        //        string sql1 = "select * from KN_KIENNGHI where ITONGHOP =" + t.ITONGHOP + " AND ITINHTRANG >= 3";

        //        int kiennghi_traloi = _kiennghi.GetList(sql1).Count();
        //        _condition = new Dictionary<string, object>();
        //        _condition.Add("ITONGHOP", t.ITONGHOP);
        //        int sokiennghi = _kiennghi.GetAll(_condition).Count();
        //        string class_traloi = " btn-primary";
        //        if (kiennghi_traloi == sokiennghi) { class_traloi = " btn-success"; }
        //        string so_kn = " <a title='Xem và trả lời kiến nghị' rel='tooltip' title='' href=\"/Kiennghi/Tonghop_kiennghi?id=" + id_encr + "\"  class='btn " + class_traloi + "'>" + kiennghi_traloi + " / " + sokiennghi + "</a> ";
        //        if (t.ILINHVUC == 0) { th.linhvuc = "<span class='f-orangered'>" + th.linhvuc + "</span>"; }
        //        if (t.ICHUONGTRINH == 0) { th.kehoach = "<span class='f-orangered'>" + th.kehoach + "</span>"; }
        //        if (t.ITHAMQUYENDONVI == 0) { th.donvi_thamquyen = "<span class='f-orangered'>" + th.donvi_thamquyen + "</span>"; }
        //        if (t.ITHAMQUYENDONVI != _base.ID_Ban_DanNguyen)
        //        {
        //            chuyen_xuly = "";
        //        }
        //        else
        //        {
        //            if (!_base.Action(6, iUser)) { chuyen_xuly = ""; }
        //        }
        //        _condition = new Dictionary<string, object>();
        //        _condition.Add("ITONGHOP", t.ITONGHOP);
        //        _condition.Add("CLOAI", "chuyentonghop");
        //        KN_VANBAN vb = _kn_vanban.GetAll(_condition).FirstOrDefault();
        //        string file_vanban = ""; if (vb.CFILE != "") { file_vanban = " <a href='" + vb.CFILE + "'><i class='icon-download-alt'></i></a>"; }
        //        string vbchuyen = "(Văn bản chuyển xử lý số <strong>" + vb.CSOVANBAN + "</strong> ban hành ngày " + func.ConvertDateVN(vb.DNGAYBANHANH.ToString()) + " " + file_vanban + ")";
        //        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter' nowrap></td><td>" + t.CNOIDUNG +
        //        "<p><strong>Tập hợp:</strong> " + th.donvi_tonghop + ";<strong>Lĩnh vực:</strong> " + th.linhvuc + "; <strong>Kế hoạch:</strong> " + th.kehoach + "</p>" + th.bt_info + th.bt_lichsu +
        //        "</td><td class='tcenter'><p class='b'>" + th.donvi_thamquyen + "</p>" + vbchuyen + "</td><td class='tcenter'>" +
        //        so_kn + "</td><td class='tcenter'>" + chuyen_xuly + traloi + "</td></tr>";

        //        count++;
        //    }
        //    return str;
        //}
        //public string KN_Traloi(int iUser, string sql)
        //{
        //    string str = "";
        //    var tonghop = _kn_tonghop.GetList(sql).OrderByDescending(x => x.ITONGHOP).ToList();
        //    int count = 1;
        //    string url_cookie = func.Get_Url_keycookie();
        //    foreach (var t in tonghop)
        //    {


        //        // new id 
        //        string id_encr = HashUtil.Encode_ID(t.ITONGHOP.ToString(), url_cookie);
        //        // end
        //        TongHop_Kiennghi th = Tonghop_Detail((int)t.ITONGHOP, id_encr);
        //        string sql1 = "select * from KN_KIENNGHI where ITONGHOP =" + t.ITONGHOP + " AND ITINHTRANG >= 3";

        //        int kiennghi_traloi = _kiennghi.GetList(sql1).Count();
        //        _condition = new Dictionary<string, object>();
        //        _condition.Add("ITONGHOP", t.ITONGHOP);
        //        int sokiennghi = _kiennghi.GetAll(_condition).Count();
        //        string class_traloi = " btn-primary";
        //        if (kiennghi_traloi == sokiennghi) { class_traloi = " btn-success"; }
        //        string so_kn = " <a title='Xem và trả lời kiến nghị' rel='tooltip' title='' href=\"/Kiennghi/Tonghop_traloi_kiennghi?id=" + id_encr + "\"  class='btn " + class_traloi + "'>" + kiennghi_traloi + " / " + sokiennghi + "</a> ";
        //        if (t.ILINHVUC == 0) { th.linhvuc = "<span class='f-orangered'>" + th.linhvuc + "</span>"; }
        //        if (t.ICHUONGTRINH == 0) { th.kehoach = "<span class='f-orangered'>" + th.kehoach + "</span>"; }
        //        if (t.ITHAMQUYENDONVI == 0) { th.donvi_thamquyen = "<span class='f-orangered'>" + th.donvi_thamquyen + "</span>"; }
        //        _condition = new Dictionary<string, object>();
        //        _condition.Add("ITONGHOP", t.ITONGHOP);
        //        _condition.Add("CLOAI", "traloi_tonghop");
        //        KN_VANBAN vb = _kn_vanban.GetAll(_condition).FirstOrDefault();

        //        string giamsat = " <a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Tonghop_giamsat_add')\" href='javascript:void()' title='Cập nhật kết quả giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-file-alt'></i></a>";
        //        string traloi = " <a onclick=\"ShowPopUp('id=" + HashUtil.Encode_ID(vb.IVANBAN.ToString(), url_cookie) + "','/Kiennghi/Ajax_Tonghop_traloi_edit')\" href='javascript:void()' title='Sửa trả lời' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a>";
        //        string traloi_del = " <a href =\"javascript:void()\" title='Hủy trả lời' rel='tooltip' title='' onclick=\"Delete_Refresh('" + id_encr + "','/Kiennghi/Ajax_Tonghop_traloi_del')\" class='trans_func'><i class='icon-trash'></i></a> ";
        //        string tinhtrang_traloi = "Hoàn thành";
        //        if (t.ITINHTRANG == 4) { tinhtrang_traloi = "Tiếp tục theo dõi"; }
        //        string noidung_traloi = "<p class='b'>" + tinhtrang_traloi + "</p>" + vb.CNOIDUNG;
        //        if (vb.CFILE != "") { noidung_traloi += " <a href='" + vb.CFILE + "'><i class='icon-download-alt'></i><a/>"; }
        //        _condition = new Dictionary<string, object>();
        //        _condition.Add("ITONGHOP", t.ITONGHOP);
        //        _condition.Add("CLOAI", "chuyentonghop");
        //        KN_VANBAN vb_chuyen = _kn_vanban.GetAll(_condition).FirstOrDefault();
        //        string file_vanban = ""; if (vb_chuyen.CFILE != "") { file_vanban = " <a href='" + vb_chuyen.CFILE + "'><i class='icon-download-alt'></i></a>"; }
        //        string vbchuyen = "(Văn bản chuyển xử lý số <strong>" + vb_chuyen.CSOVANBAN + "</strong> ban hành ngày " + func.ConvertDateVN(vb_chuyen.DNGAYBANHANH.ToString()) + " " + file_vanban + ")";
        //        if (!_base.Action(7, iUser))
        //        {
        //            traloi = ""; traloi_del = "";
        //        }
        //        else
        //        {
        //            if (_base.IDDonVi_User(iUser) != t.ITHAMQUYENDONVI && !_base.IsAdmin(iUser))
        //            {
        //                traloi = ""; traloi_del = "";
        //            }
        //        }
        //        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter' nowrap></td><td>" + t.CNOIDUNG +
        //        "<p>" + _base.GetName_KyHop_KhoaHop((int)t.IKYHOP).Replace("</br>", "-") + "; <strong>Kế hoạch:</strong> " + th.kehoach + "; <strong>Lĩnh vực:</strong> " + th.linhvuc + "</p>" + th.bt_info + th.bt_lichsu +
        //        "</td><td class='tcenter'><p class='b'>" + th.donvi_thamquyen + "</p>" + vbchuyen + "</td><td><p>" + noidung_traloi + "</p><p class='tcenter'>" + traloi + traloi_del + "</p></td><td class='tcenter'>" + so_kn + "</td></tr>";

        //        count++;
        //    }
        //    return str;
        //}
        //public string KN_Dong(List<KN_KIENNGHI> kiennghi)
        //{
        //    string str = "";
        //    //var kiennghi = _kiennghi.GetList(sql).OrderByDescending(x => x.DDATE).ToList();

        //    int count = 1;
        //    string url_cookie = func.Get_Url_keycookie();
        //    foreach (var k in kiennghi)
        //    {
        //        //string thamquyen_tiepnhan = "";
        //        string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_cookie);
        //        string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
        //        string bt_lichsu = " <a href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
        //        string tinhtrang = Str_tinhtrang_kiennghi(k);
        //        //KN_CL kn = KienNghi_Detail((int)k.IKIENNGHI, id_encr);
        //        str += "<tr><td class='tcenter b'>" + count + "</td><td><p>" + func.TomTatNoiDung(k.CNOIDUNG, id_encr) + "</p>" +
        //            "</td><td class='tcenter b'>" + tinhtrang + "</td><td class='tcenter'>" + info + bt_lichsu + "</td></tr>";
        //        count++;
        //    }
        //    return str;
        //}
        public string List_Giamsat(List<PRC_DOANGIAMSAT> doan, TaikhoanAtion act)
        {
            string str = "";
            if (doan.Count() == 0)
            {
                return "<tr><td class='tcenter alert-danger' colspan='5'>Không tìm thấy kế hoạch giám sát nào!</td></tr>";
            }
            string url_cookie = func.Get_Url_keycookie();
            decimal id_donvi = 0;int count = 1;
            foreach(var c in doan)
            {
                if (id_donvi != c.ID_DONVI)
                {
                    str += "<tr><th class='' colspan='5'>" + EncodeOutput(c.TEN_DONVI.ToUpper()) + " (" + doan.Where(x=>x.ID_DONVI==c.ID_DONVI).Count() + ")</th></tr>";
                    count = 1;
                }
                string id_encr = HashUtil.Encode_ID(c.IDOAN.ToString(), url_cookie);
                string edit = " <a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Giamsat_edit')\" title='Sửa giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                                "','/Kiennghi/Ajax_Giamsat_del','Bạn có muốn xóa kế hoạch giám sát này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                if (c.ID_USER_CAPNHAT != act.iUser && !act.is_admin) { edit = ""; del = ""; }                
                if (c.SOKIENNGHI > 0) { del = ""; }
                string kiennghi = "<a href=\"/Kiennghi/Giamsat_kiennghi/?id=" + id_encr + "\"  title='Danh sách kiến nghị' rel='tooltip' title='' class='trans_func'><i class='icon-list-ul'></i></a>";
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter'>" + func.ConvertDateVN(c.NGAYBATDAU.ToString()) + "</td><td>" + EncodeOutput(c.TEN_KEHOACH) +
                    "</td><td>" + EncodeOutput(c.NOIDUNG_GIAMSAT) + File_View((int)c.IDOAN, "kn_doangiamsat") + "</td><td class='tcenter' nowrap>" + kiennghi + edit + del + "</td></tr>";
                count++;
                id_donvi = c.ID_DONVI;
            }
            //foreach (var c in coquan)
            //{
            //    int count = 1;
            //    var doan1 = doan.Where(x => x.IDONVI == (int)c.ICOQUAN).OrderBy(x => x.CTEN).ToList();
            //    if (doan1.Count() > 0)
            //    {
            //        str += "<tr><th class='' colspan='5'>" + c.CTEN.ToUpper() + " (" + doan1.Count() + ")</th></tr>";
            //        foreach (var k in doan1)
            //        {
            //            string id_encr = HashUtil.Encode_ID(k.IDOAN.ToString(), url_cookie);
            //            string edit = " <a href=\"javascript:void()\" onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Giamsat_edit')\" title='Sửa giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
            //            string del = " <a href=\"javascript:void()\" title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
            //                            "','/Kiennghi/Ajax_Giamsat_del','Bạn có muốn xóa kế hoạch giám sát này?')\" class='trans_func'><i class='icon-trash'></i></a> ";
            //            if (k.IUSER != act.iUser && !act.is_admin) { edit = ""; del = ""; }
            //            Dictionary<string, object> k_ = new Dictionary<string, object>();
            //            k_.Add("IDOANGIAMSAT", (int)k.IDOAN);
            //            if (_doan_kiennghi.GetAll(k_).Count() > 0) { del = ""; }
            //            string kiennghi = "<a href=\"/Kiennghi/Giamsat_kiennghi/?id=" + id_encr + "\"  title='Danh sách kiến nghị' rel='tooltip' title='' class='trans_func'><i class='icon-list-ul'></i></a>";
            //            str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter'>" + func.ConvertDateVN(k.DNGAYBATDAU.ToString()) + "</td><td>" + EncodeOutput(k.CTEN) +
            //                "</td><td>" + EncodeOutput(k.CNOIDUNG) + File_View((int)k.IDOAN, "kn_doangiamsat") + "</td><td class='tcenter' nowrap>" + kiennghi + edit + del + "</td></tr>";
            //            count++;
            //        }
            //    }
            //}

            return str;
        }
        public string List_KienNghi_ByID_doangiamsat(List<KN_KIENNGHI> kiennghi, List<KN_DOANGIAMSAT_KIENNGHI> doan_kiennghi, TaikhoanAtion act, bool remove, string url_key)
        {
            string str = "";
            if (kiennghi.Count() == 0)
            {
                return "<tr><td colspan='4' class='alert tcenter'>Chưa có kiến nghị nào được thêm vào</td></tr>";
            }
            int count = 1;
            foreach (var k in doan_kiennghi)
            {
                if (kiennghi.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).Count() > 0) {
                    KN_KIENNGHI _k = kiennghi.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).FirstOrDefault();
                    string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_key);
                    string del = " <a href =\"javascript:void()\" title='Hủy kiến nghị' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Kiennghi/Ajax_Remove_kiennghi_by_doan','Bạn có muốn đưa kiến nghị này ra khỏi chương trình giám sát?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    if (remove == false) { del = ""; }
                    string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiennghi_info?id=" + id_encr + "\" title='Chi tiết kiến nghị' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a> ";

                    string ykien = " <a href=\"/Kiennghi/Giamsat_ykien?id=" + id_encr + "\" title='Ý kiến giám sát' rel='tooltip' title='' class='trans_func'><i class='icon-edit'></i></a> ";
                    str += "<tr id='tr_" + k.IKIENNGHI + "'><td class='tcenter b'>" + count + "</td><td>" + func.TomTatNoiDung(EncodeOutput(_k.CNOIDUNG), id_encr) +
                        "</td><td>" + Col_Traloi_kiennghi((int)k.IKIENNGHI) + "</td><td class='tcenter'>" + del + chitiet + ykien + "</td></tr>";
                    count++;
                }

            }
            return str;
        }
        public string List_Ykien_ByID_kiennghi(List<KN_DOANGIAMSAT_YKIEN> ykien, TaikhoanAtion act, bool remove, string url_key)
        {
            string str = "";
            if (ykien.Count() == 0)
            {
                return "<tr><td colspan='4' class='alert tcenter'>Chưa có ý kiến nào!</td></tr>";
            }
            int count = 1;
            foreach (var k in ykien)
            {
                string id_encr = HashUtil.Encode_ID(k.IYKIEN.ToString(), url_key);
                string del = " <a href =\"javascript:void()\" title='Hủy' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kiennghi/Ajax_Remove_ykien','Bạn có muốn xóa ý kiến này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string edit = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Giamsat_ykien_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                if (remove == false) { del = ""; edit = ""; }
                str += "<tr><td class='tcenter b'>" + count + "</td><td>" + func.ConvertDateVN(k.DNGAYLAMVIEC.ToString()) +
                    "</td><td><p><strong>" + EncodeOutput(k.CTEN) + "</strong></p>" + EncodeOutput(k.CNOIDUNG) + " " + File_View((int)k.IYKIEN, "giamsat_ykien") + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                count++;
            }
            return str;
        }

        public string List_KienNghi_KiemTrungNoiDung(List<KN_KIENNGHI> kiennghi, List<QUOCHOI_COQUAN> coquan, int id_kiennghi_parent)
        {
            string str = "";
            int count = 1;
            //Where(x=>x.IKIENNGHI!=id && (x.ID_KIENNGHI_PARENT==0 || x.ID_KIENNGHI_PARENT == id))
            var kiennghi1 = kiennghi.Where(x => x.IKIENNGHI != id_kiennghi_parent && (x.ID_KIENNGHI_PARENT == 0 || x.ID_KIENNGHI_PARENT == id_kiennghi_parent)).OrderByDescending(x => x.ID_KIENNGHI_PARENT).ToList();
            foreach (var k in kiennghi1)
            {
                if (kiennghi.Where(x => x.ID_KIENNGHI_PARENT == k.IKIENNGHI).Count() == 0)
                {
                    string check = ""; if (k.ID_KIENNGHI_PARENT == id_kiennghi_parent) { check = " checked "; }
                    string input = "<input " + check + " onclick=\"UpdateStatus('id_kiennghi_parent=" + id_kiennghi_parent + "&id_kiennghi=" + k.IKIENNGHI + "', '/Kiennghi/Ajax_Chonkiennghi_cungnoidung')\" type='checkbox' name='kn_chon' value='" + k.IKIENNGHI + "'/>";
                    
                    string doantonghop = "<strong>Đoàn Tập hợp: </strong><em>" + EncodeOutput(coquan.Where(x => x.ICOQUAN == (int)k.IDONVITIEPNHAN).FirstOrDefault().CTEN) + "</em>";
                    str += "<tr><td class='tcenter b'>" + count + "</td><td><p>" + EncodeOutput(k.CNOIDUNG) +
                        "</p>" + doantonghop + "</td><td class='tcenter'>" + input + "</td></tr>";
                    count++;
                }                
            }
            return str;
        }
        public string List_KienNghi_ByID_tonghop(List<PRC_KIENNGHI_BYTONGHOP> kiennghi,  TaikhoanAtion act, bool remove, string url_key)
        {
            
            if (kiennghi.Count() == 0)
            {
                return "<tr><td colspan='3' class='alert tcenter'>Chưa có kiến nghị nào được thêm vào</td></tr>";
            }
            string tiepnhan = "";
            string chon_gop = "";
            if (act.is_lanhdao)
            {
                chon_gop = "<th width='5%' class='tcenter' nowrap>Chọn</th>";
                tiepnhan = "<th width='15%' class='tcenter' nowrap>Tiếp nhận</th>";
            }
            string str = "<tr><th width='5%' nowrap>STT</th>"+chon_gop+"<th nowrap>Nội dung kiến nghị</th>" + tiepnhan + "<th width='10%' class='tcenter' nowrap>Chức năng</th></tr>";

            int count = 1;
            List<decimal> list_id_kiennghi = new List<decimal>();
            var kiennghi_tonghoop = kiennghi.Where(x => x.ID_GOP <= 0).ToList();
            foreach (var k in kiennghi_tonghoop)
            {
                if (!list_id_kiennghi.Contains(k.IKIENNGHI))
                {
                    list_id_kiennghi.Add(k.IKIENNGHI);
                    string chon_gop_kiennghi = "<input type='checkbox' value='" + k.IKIENNGHI + "' name='kn_themtonghop' />";
                    string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_key);
                    string del = " <a href =\"javascript:void()\" title='Loại kiến nghị khỏi Tập hợp' rel='tooltip' title='' onclick=\"DeletePage_Confirm_KN_TONGHOP('" + k.IKIENNGHI + "','id=" + k.IKIENNGHI +
                            "','/Kiennghi/Ajax_Remove_kiennghi_by_tonghop','Bạn có muốn loại kiến nghị này khỏi Tập hợp không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a>";
                    if (remove == false) { del = ""; }
                    string noidung_kn = func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG), id_encr);
                    if (k.ID_GOP == -1)
                    {
                        chon_gop_kiennghi = "";
                        noidung_kn = "<a class='gachchan' href='/Kiennghi/Kiennghi_gop_list/?id=" + id_encr + "' title='Nhấn vào để xem danh sách các kiến nghị đã gộp'>" + EncodeOutput(k.CNOIDUNG) + "</a>";
                    }
                    string doan_tiepnhan = "";
                    string chon_gop_kn = "";
                    if (act.is_lanhdao)
                    {
                        doan_tiepnhan = "<td class='tcenter'>" + EncodeOutput(k.TENDONVITIEPNHAN) + "</td>";
                        chon_gop_kn = "<td class='tcenter'>" + chon_gop_kiennghi + "</td>";
                        if (k.ID_GOP == -1)
                        {
                            doan_tiepnhan = "<td class='tcenter'>" + EncodeOutput(k.TENDONVITIEPNHAN_GOP) + "</td>";
                        }
                    }
                    string diachi = DiaChiDayDu(k.CDIACHI, k.TEN_HUYEN, k.TEN_TINH);
                    if (diachi != "") { diachi = "<strong>Địa chỉ</strong>: <em>" + diachi + "</em>"; }
                    str += "<tr id='tr_" + k.IKIENNGHI + "'><td class='tcenter b'>" + count + "</td>"+chon_gop_kn+"<td>" + noidung_kn + "</br>" + diachi + "</td>" + doan_tiepnhan + "<td class='tcenter'>" + del + info + "</td></tr>";
                    count++;
                }
                
            }
            return str;
        }
        //public string Row_KienNghi_ByID_tonghop(KN_KIENNGHI k, List<QUOCHOI_COQUAN> coquan, TaikhoanAtion act, bool remove, string url_key, int count = 1, bool sup = false)
        //{
        //    string str = "";
        //    string del = " <a href =\"javascript:void()\" title='Loại kiến nghị khỏi Tập hợp' rel='tooltip' title='' onclick=\"DeletePage_Confirm_KN_TONGHOP('" + k.IKIENNGHI + "','id=" + k.IKIENNGHI +
        //                    "','/Kiennghi/Ajax_Remove_kiennghi_by_tonghop','Bạn có muốn loại kiến nghị này khỏi Tập hợp không?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
        //    string doantonghop = str_donvi_tiepnhan((int)k.IDONVITIEPNHAN, coquan);
        //    string chontrung = " <a onclick=\"ShowPageLoading()\" href =\"/Kiennghi/Tonghop_chontrung/?id=" + 
        //            HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_key) + 
        //            "\" title='Gộp kiến nghị cùng nội dung' rel='tooltip' title='' class='trans_func'><i class='icon-copy'></i></a> ";
        //    if (k.ID_KIENNGHI_PARENT > 0 || k.ITONGHOP_BDN == 0) { chontrung = ""; }
        //    string padding = "";
        //    if (k.ID_KIENNGHI_PARENT > 0) { padding = " style='padding-left:40px;font-style: italic;' "; }
        //    if (remove == false) { del = ""; chontrung = ""; }
        //    string noidung_doan = "<div><p>" + func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG), k.IKIENNGHI.ToString()) + "</p>" + doantonghop + "</div>";
        //    if (sup)
        //    {
        //        del = "";
        //        noidung_doan = "<p class='b'>" + k.CNOIDUNG_TRUNG + "</p><div style='padding-left:40px;font-style: italic;'><p>" + func.TomTatNoiDung(k.CNOIDUNG, k.IKIENNGHI.ToString()) + "</p>" + doantonghop + "</div>";
        //    }
        //    str += "<tr id='tr_" + k.IKIENNGHI + "'><td class='tcenter b'>" + count + "</td><td " +
        //            padding + ">" + noidung_doan + "</td><td class='tcenter'>" + chontrung + del + "</td></tr>";
        //    return str;
        //}
        public string List_KienNghi_Chon_Gop(List<KN_KIENNGHI> kiennghi_gop, List<QUOCHOI_COQUAN> coquan, TaikhoanAtion act, bool remove, string url_key)
        {
            
            if (kiennghi_gop.Count() == 0)
            {
                return "<tr><td colspan='4' class='alert tcenter'>Chưa có kiến nghị nào được thêm vào</td></tr>";
            }
            string tiepnhan = "<th width='15%' class='tcenter' nowrap>Tiếp nhận</th>";
            
            string str = "<tr><th width='5%' nowrap>STT</th><th nowrap>Nội dung kiến nghị</th>" + tiepnhan + "<th width='10%' class='tcenter' nowrap>Chức năng</th></tr>";

            int count = 1;
            foreach (var k in kiennghi_gop)
            {
                string del = " <a href =\"javascript:void()\" title='Loại kiến nghị khỏi kiến nghị gộp' rel='tooltip' title='' onclick=\"Delete_Refresh('" + k.IKIENNGHI + "','/Kiennghi/Ajax_Remove_from_kiennghi_gop')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string doantonghop = "";
                if (coquan.Where(x => x.ICOQUAN == k.IDONVITIEPNHAN).Count() > 0)
                {
                    doantonghop = coquan.Where(x => x.ICOQUAN == k.IDONVITIEPNHAN).FirstOrDefault().CTEN;
                }
                string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_key);
                string info=" <a href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
                if (remove == false) { del = ""; }
                string noidung_doan = "<p>" + func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG), id_encr) + "</p>";
                str += "<tr id='tr_" + k.IKIENNGHI + "'><td class='tcenter b'>" + count + "</td><td>" + noidung_doan + "</td><td class='tcenter'>" + EncodeOutput(doantonghop) + "</td><td class='tcenter'>" + info + del + "</td></tr>";
                count++;

            }
            return str;
        }
        public string List_KienNghi_Chon_Gop_View(List<KN_KIENNGHI> kiennghi_gop, List<QUOCHOI_COQUAN> coquan, string url_key)
        {
            string str = "";
            int count = 1;
            foreach (var k in kiennghi_gop)
            {
                string doantonghop = "";
                if (coquan.Where(x => x.ICOQUAN == k.IDONVITIEPNHAN).Count() > 0)
                {
                    doantonghop = "<td class='tcenter'>" + EncodeOutput(coquan.Where(x => x.ICOQUAN == k.IDONVITIEPNHAN).FirstOrDefault().CTEN) + "</td>";
                }
                else { doantonghop = "<td class='tcenter'></td>"; }
                string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_key);
                string info = " <a href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";                
                string noidung_doan = "<p>" + func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG), id_encr) + "</p>";
                str += "<tr id='tr_" + k.IKIENNGHI + "'><td class='tcenter b'>" + count + "</td><td>" + noidung_doan + "</td>" + doantonghop + "<td class='tcenter'>" + info + "</td></tr>";
                count++;

            }
            return str;
        }
        public string Info_Kiennghi_traloi_danhgia(KN_GIAMSAT kn_giamsat)
        {
            if (kn_giamsat == null || kn_giamsat.IPHANLOAI==null) { return ""; }
            //KN_GIAMSAT kn_giamsat = giamsat.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).FirstOrDefault();
            //KN_Giamsat_PhanloaiRepository _phanloai = new KN_Giamsat_PhanloaiRepository();
            //KN_Giamsat_DanhgiaRepository _danhgia = new KN_Giamsat_DanhgiaRepository();
            string str = "<p class='b'>Kết quả đánh giá:</p>";
            List<KN_TRALOI_PHANLOAI> phanloai_danhgia = _kn.GetAll_KN_TRALOI_PHANLOAI();
            var phanloai_traloi1 = phanloai_danhgia.Where(x => x.IPHANLOAI == (int)kn_giamsat.IPHANLOAI).ToList();
            string noidung_phanloai = "";
            if (phanloai_traloi1.Count() > 0)
            {
                KN_TRALOI_PHANLOAI phanloaitraloi = phanloai_traloi1.FirstOrDefault();
                noidung_phanloai = EncodeOutput(phanloaitraloi.CTEN);
                if (phanloaitraloi.IPARENT != 0 && phanloai_danhgia.Where(x => x.IPHANLOAI == (int)phanloaitraloi.IPARENT).Count() > 0)
                {
                    KN_TRALOI_PHANLOAI phanloaitraloi_parent = phanloai_danhgia.Where(x => x.IPHANLOAI == (int)phanloaitraloi.IPARENT).FirstOrDefault();
                    noidung_phanloai = EncodeOutput(phanloaitraloi_parent.CTEN) + " > " + noidung_phanloai;
                }
                noidung_phanloai = "<p><strong>Phân loại:</strong> " + noidung_phanloai + "</p>";
            }
            //string phanloai = "<strong>Phân loại:</strong> " + _phanloai.GetByID((int)kn_giamsat.IPHANLOAI).CTEN + "</br>";
            //string danhgia = "<strong>Đánh giá:</strong> " + _danhgia.GetByID((int)kn_giamsat.IDANHGIA).CTEN + "</br>";
            string tiendo = "<strong>Tiến độ:</strong> Đúng tiến độ</br> ";
            if (kn_giamsat.IDUNGTIENDO == 0) { tiendo = "<strong>Tiến độ:</strong> Chậm tiến độ</br> "; }
            string denghi = "<strong>Đề nghị:</strong> Đóng kiến nghị</br> ";
            if (kn_giamsat.IDONGKIENNGHI == 0) { denghi = "<strong>Đề nghị:</strong> Theo dõi ở kỳ họp sau "; }
            return str + noidung_phanloai+ denghi + tiendo;
        }
        public string NguoiCapNhat(int iUser, DateTime time)
        {
            //UsserRepository _user = new UsserRepository();
            string str = "";
            USERS u = _kn.HienThiThongTinTaikhoan(iUser);
            if (u != null)
            {
                str += "<p class='tright f-grey'><em>Cập nhật bởi " + EncodeOutput(u.CTEN) + " ngày " + func.ConvertDateVN(time.ToString()) + "</em></p>";
            }
            return str;
        }
        public string Info_Traloi_GiamSat_kiennghi(KN_KIENNGHI_TRALOI traloi, TaikhoanAtion act, string url_key)
        {
            string str = "";
            string id_encr = HashUtil.Encode_ID(traloi.ITRALOI.ToString(), url_key);
            string file = File_View((int)traloi.ITRALOI, "kn_traloi");
            //var all_phanloai = _kn.GetAll_KN_TRALOI_PHANLOAI();
            
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("ITRALOI", (int)traloi.ITRALOI);
            KN_GIAMSAT g = null;

            KN_giamsatRepository _giamsat = new KN_giamsatRepository();
            List<KN_GIAMSAT> list_giamsat = _giamsat.GetAll(_dic);
            if (list_giamsat.Count() > 0)
            {
                g = list_giamsat.FirstOrDefault();
            }
            
            
            string giamsat = "";
            if (g != null && act.is_lanhdao && g.IPHANLOAI!=null) {
                string id_ecrt_danhgia = HashUtil.Encode_ID(g.IGIAMSAT.ToString(), url_key);
                string giamsat_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_ecrt_danhgia + "','/Kiennghi/Ajax_Kiennghi_danhgia_edit')\" class='trans_func'><i class='icon-edit'></i></a> ";
                string giamsat_del = " <a href =\"javascript:void()\" title='Hủy' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_ecrt_danhgia + "','id=" + id_ecrt_danhgia +
                            "','/Kiennghi/Ajax_Kiennghi_danhgia_del','Bạn có muốn xóa đánh giá kiến nghị này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                if (g.IUSER != act.iUser && !act.is_admin) { giamsat_add = ""; giamsat_del = ""; }
                giamsat = "<div class='alert alert-info kn_traloi'><div class='alert-icon'>" + giamsat_add + giamsat_del + "</div>" + Info_Kiennghi_traloi_danhgia(g) + "</div>";
            }
            string congvan = "";
            if (traloi.CSOVANBAN != null)
            {
                congvan = "Công văn số <strong>" + EncodeOutput(traloi.CSOVANBAN) + "</strong> ngày " + func.ConvertDateVN(traloi.DNGAYBANHANH.ToString());
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.DaCoTraLoi)//1: Đã có trả lời,
            {
                
                List<KN_TRALOI_PHANLOAI> phanloai_traloi = _kn.GetAll_KN_TRALOI_PHANLOAI();
                var phanloai_traloi1 = phanloai_traloi.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).ToList();
                string phanloai = "";
                if (phanloai_traloi1.Count() > 0)
                {

                    KN_TRALOI_PHANLOAI phanloaitraloi = phanloai_traloi1.FirstOrDefault();
                    string noidung_phanloai = EncodeOutput(phanloaitraloi.CTEN);
                    if (phanloaitraloi.IPARENT != 0 && phanloai_traloi.Where(x => x.IPHANLOAI == (int)phanloaitraloi.IPARENT).Count() > 0)
                    {
                        KN_TRALOI_PHANLOAI phanloaitraloi_parent = phanloai_traloi.Where(x => x.IPHANLOAI == (int)phanloaitraloi.IPARENT).FirstOrDefault();
                        noidung_phanloai = EncodeOutput(phanloaitraloi_parent.CTEN) + " > " + noidung_phanloai;
                    }
                    phanloai = "<p><strong>Phân loại:</strong> " + noidung_phanloai + "</p>";
                }
                string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_traloi_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string traloi_del = " <a onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kiennghi/Ajax_Kiennghi_traloi_del','Bạn có muốn xóa trả lời kiến nghị này hay không?')\" href =\"javascript:void()\" title='Hủy' rel='tooltip' title=''><i class='icon-trash'></i></a>";

                if (g != null) {
                    traloi_add = ""; traloi_del = "";
                }else
                {
                    if(traloi.IUSER != act.iUser && !base_bussiness.Action_(50, act)) { traloi_add = ""; traloi_del = ""; }
                }
                string lotrinh = "";
                if (traloi.IPHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH && traloi.DNGAY_DUKIEN!=null) {
                    lotrinh = "</br><strong>Dự kiến giải quyết dứt điểm:</strong> " + func.ConvertDateVN(traloi.DNGAY_DUKIEN.ToString());
                }
                str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add + traloi_del +
                        "</div><p><strong class='gachchan'>Trả lời kiến nghị:</strong></br>" + phanloai + "</p><strong>Văn bản xử lý:</strong> " + congvan + "</br><strong>Kết quả xử lý:</strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + lotrinh+ " </div>" + giamsat;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.TraLaiKienNghi)//2:Trả lại kiến nghị
            {
                List<KN_TRALOI_PHANLOAI> phanloai_traloi = _kn.GetAll_KN_TRALOI_PHANLOAI();
                var phanloai_traloi1 = phanloai_traloi.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).ToList();
                string phanloai = "";
                if (phanloai_traloi1.Count() > 0)
                {
                    
                    KN_TRALOI_PHANLOAI phanloaitraloi = phanloai_traloi1.FirstOrDefault();
                    string noidung_phanloai = EncodeOutput(phanloaitraloi.CTEN);
                    if (phanloaitraloi.IPARENT != 0 && phanloai_traloi.Where(x => x.IPHANLOAI == (int)phanloaitraloi.IPARENT).Count()>0) {
                        KN_TRALOI_PHANLOAI phanloaitraloi_parent = phanloai_traloi.Where(x => x.IPHANLOAI == (int)phanloaitraloi.IPARENT).FirstOrDefault();
                        noidung_phanloai = EncodeOutput(phanloaitraloi_parent.CTEN) + " > " + EncodeOutput(noidung_phanloai);
                    }
                    phanloai = "<strong>Phân loại:</strong> " + noidung_phanloai + "</br>";
                }
                string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_traloi_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string traloi_del = " <a onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                            "','/Kiennghi/Ajax_Kiennghi_traloi_del','Bạn có muốn xóa trả lời kiến nghị này hay không?')\" href =\"javascript:void()\" title='Hủy' rel='tooltip' title=''><i class='icon-trash'></i></a>";
                if (g != null)
                {
                    traloi_add = ""; traloi_del = "";
                }
                else
                {
                    if (traloi.IUSER != act.iUser && !base_bussiness.Action_(50, act)) { traloi_add = ""; traloi_del = ""; }
                }
                if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
                string lotrinh = "";
                if (traloi.IPHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH && traloi.DNGAY_DUKIEN != null)
                {
                    lotrinh = "</br><strong>Dự kiến giải quyết dứt điểm:</strong> " + func.ConvertDateVN(traloi.DNGAY_DUKIEN.ToString());
                }
                str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add + traloi_del +
                        "</div><p><strong class='gachchan'>Trả lại kiến nghị:</strong></br>" + phanloai + "</p><strong>Văn bản xử lý:</strong> " + congvan + "</br><strong>Kết quả xử lý:</strong> " + traloi.CTRALOI + " " + file + lotrinh + " </div>" + giamsat;
                
            }
            /*
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuyenGiaiQuyetKyHopSau)//3: Chuyển giải quyết kỳ họp sau
            {
                //string traloi_del = " <a onclick=\"Delete_Refresh('" + id_encr + "', '/Kiennghi/Ajax_Kiennghi_tra_chuyenkysau_del')\" href =\"javascript:void()\" title='Hủy' rel='tooltip' title=''><i class='icon-trash'></i></a>";
                string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chuyenkysau_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
                str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add +
                    "</div><p class='b'>Chuyển theo dõi kỳ họp sau</p><strong>Lý do: </strong>" + traloi.CTRALOI + " " + file + "</div>" + giamsat;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.CoLoTrinhGiaiQuyet)//Có lộ trình giải quyết
            {
                //string traloi_del = " <a onclick=\"Delete_Refresh('" + id_encr + "', '/Kiennghi/Ajax_Kiennghi_tra_chuyenkysau_del')\" href =\"javascript:void()\" title='Hủy' rel='tooltip' title=''><i class='icon-trash'></i></a>";
                string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_lotrinh_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
                string ngaydukien = "";
                if (traloi.DNGAY_DUKIEN != null) { ngaydukien = "<p><strong>Ngày dự kiến giải quyết dứt điểm: </strong> " + func.ConvertDateVN(traloi.DNGAY_DUKIEN.ToString()) + "</p>"; }
                str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add +
                    "</div><p><strong>Có lộ trình giải quyết:</strong>" + traloi.CTRALOI + " " + file + "<p>" +
                        ngaydukien + "</div>" + giamsat;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoLoTrinhGiaiQuyet)//Chưa có lộ trình giải quyết
            {
                string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chualotrinh_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
                str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add + "</div><p class='b'>Chưa có lộ trình giải quyết</p><strong>Lý do: </strong>" + traloi.CTRALOI + " " + file + "</div>" + giamsat;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaTheGiaiQuyetNgay)//Chưa thể giải quyết ngay
            {
                string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chuagiaiquyet_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
                str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add +
                    "</div><p class='b'>Chưa thể giải quyết ngay:</p><strong>Lý do: </strong>" + traloi.CTRALOI + " " + file + "</div>" + giamsat;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoNguonLucGiaiQuyet)//Chưa có nguồn lực giải quyết
            {
                string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_nguonluc_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
                str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add +
                    "</div><p class='b'>Chưa có nguồn lực giải quyết:</p><strong>Lý do: </strong>" + traloi.CTRALOI + " " + file + "</div>" + giamsat;
            }
            */
            return str;
        }
        public string Info_Traloi_GiamSat_kiennghi_Chuyenkysau(KN_KIENNGHI_TRALOI traloi, List<KN_GIAMSAT> list_giamsat, TaikhoanAtion act)
        {
            string str = "";
            string file = File_View((int)traloi.ITRALOI, "kn_traloi");
            //var all_phanloai = _kn.GetAll_KN_TRALOI_PHANLOAI();
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("ITRALOI", (int)traloi.ITRALOI);
            KN_GIAMSAT g = null;
            KN_giamsatRepository _giamsat = new KN_giamsatRepository();
            if (list_giamsat.Count() > 0)
            {
                g = list_giamsat.FirstOrDefault();
            }


            string giamsat = "";
            if (g != null && act.is_lanhdao)
            {
                giamsat = "<div class='alert alert-info kn_traloi'>" + Info_Kiennghi_traloi_danhgia(g) + "</div>";
            }
            string congvan = "";
            if (traloi.CSOVANBAN != null)
            {
                congvan = "Công văn số <strong>" + EncodeOutput(traloi.CSOVANBAN) + "</strong> ngày " + func.ConvertDateVN(traloi.DNGAYBANHANH.ToString());
            }
            string tinhtrang_traloi = "Trả lại kiến nghị";
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.DaCoTraLoi)//1: Đã có trả lời,
            {
                tinhtrang_traloi = "Trả lời kiến nghị";
            }
            List<KN_TRALOI_PHANLOAI> phanloai_traloi = _kn.GetAll_KN_TRALOI_PHANLOAI();
            var phanloai_traloi1 = phanloai_traloi.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).ToList();
            string phanloai = "";
            if (phanloai_traloi1.Count() > 0)
            {

                KN_TRALOI_PHANLOAI phanloaitraloi = phanloai_traloi1.FirstOrDefault();
                string noidung_phanloai = EncodeOutput(phanloaitraloi.CTEN);
                if (phanloaitraloi.IPARENT != 0 && phanloai_traloi.Where(x => x.IPHANLOAI == (int)phanloaitraloi.IPARENT).Count() > 0)
                {
                    KN_TRALOI_PHANLOAI phanloaitraloi_parent = phanloai_traloi.Where(x => x.IPHANLOAI == (int)phanloaitraloi.IPARENT).FirstOrDefault();
                    noidung_phanloai = EncodeOutput(phanloaitraloi_parent.CTEN) + " > " + noidung_phanloai;
                }
                phanloai = "<p><strong>Phân loại:</strong> " + noidung_phanloai + "</p>";
            }
                
            string lotrinh = "";
            if (traloi.IPHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH && traloi.DNGAY_DUKIEN != null)
            {
                lotrinh = "</br><strong>Dự kiến giải quyết dứt điểm:</strong> " + func.ConvertDateVN(traloi.DNGAY_DUKIEN.ToString());
            }
            str = "<div class='alert alert-success kn_traloi'><p><strong class='gachchan'>"+ tinhtrang_traloi + ":</strong></br>" + phanloai + "</p><strong>Văn bản xử lý:</strong> " + congvan + "</br><strong>Kết quả xử lý:</strong> " + traloi.CTRALOI + " " + file + lotrinh + " </div>" + giamsat;
            return str;
        }
        //public string Info_Traloi_GiamSat_kiennghi_chuyenkysau(KN_KIENNGHI_TRALOI traloi, TaikhoanAtion act, string url_key)
        //{
        //    string str = "";
        //    string id_encr = HashUtil.Encode_ID(traloi.ITRALOI.ToString(), url_key);
        //    string file = File_View((int)traloi.ITRALOI, "kn_traloi");
        //    KN_giamsatRepository _giamsat = new KN_giamsatRepository();
        //    Dictionary<string, object> _dic = new Dictionary<string, object>();
        //    _dic.Add("ITRALOI", (int)traloi.ITRALOI);
        //    List<KN_GIAMSAT> list_giamsat = _giamsat.GetAll(_dic);
        //    KN_GIAMSAT g = null;
        //    if (list_giamsat.Count() > 0)
        //    {
        //        g = list_giamsat.FirstOrDefault();
        //    }
        //    string giamsat = "";
        //    if (g != null)
        //    {
        //        string id_ecrt_danhgia = HashUtil.Encode_ID(g.IGIAMSAT.ToString(), url_key);
        //        string giamsat_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_ecrt_danhgia + "','/Kiennghi/Ajax_Kiennghi_danhgia_chuyenkysau_edit')\" class='trans_func'><i class='icon-edit'></i></a> ";
        //        string giamsat_del = " <a href =\"javascript:void()\" title='Hủy' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_ecrt_danhgia + "','id=" + id_ecrt_danhgia +
        //                    "','/Kiennghi/Ajax_Kiennghi_danhgia_chuyenkysau_del','Bạn có muốn xóa đánh giá kiến nghị này hay không?')\" class='trans_func'><i class='icon-trash'></i></a> ";
        //        if (g.IUSER != act.iUser && !act.is_admin) { giamsat_add = ""; giamsat_del = ""; }
        //        giamsat = "<div class='alert alert-info kn_traloi'><div class='alert-icon'>" +
        //        giamsat_add + giamsat_del + "</div>" + Info_Kiennghi_traloi_danhgia(g) + "</div>";
        //    }
        //    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.DaCoTraLoi)//1: Đã có trả lời,
        //    {
        //        List<KN_TRALOI_PHANLOAI> phanloai_traloi = _kn.GetAll_KN_TRALOI_PHANLOAI();
        //        phanloai_traloi = phanloai_traloi.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).ToList();
        //        string phanloai = "";
        //        if (phanloai_traloi.Count() > 0)
        //        {
        //            phanloai = "<p><strong>Phân loại:</strong> " + phanloai_traloi.FirstOrDefault().CTEN + "</p>";
        //        }
        //        string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_traloi_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
        //        string traloi_del = " <a onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
        //                    "','/Kiennghi/Ajax_Kiennghi_traloi_del','Bạn có muốn xóa trả lời kiến nghị này hay không?')\"  href =\"javascript:void()\" title='Hủy' rel='tooltip' title=''><i class='icon-trash'></i></a>";

        //        if (traloi.IUSER != act.iUser || g != null)
        //        {
        //            traloi_add = ""; traloi_del = "";

        //        }
        //        str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add + traloi_del +
        //                "</div><p class='b'>Trả lời kiến nghị:</p>" + traloi.CTRALOI + " " + file + " " + phanloai + "</div>" + giamsat;
        //    }
        //    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.TraLaiKienNghi)//2:Trả lại kiến nghị
        //    {
        //        string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
        //        //string traloi_del = " <a onclick=\"Delete_Refresh('" + id_encr + "', '/Kiennghi/Ajax_Kiennghi_tra_del')\" href =\"javascript:void()\" title='Hủy' rel='tooltip' title=''><i class='icon-trash'></i></a>";
        //        if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
        //        str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add +
        //            "</div><p class='b'>Trả lại kiến nghị</p><strong>Lý do: </strong>" + traloi.CTRALOI + " " + file + "</div>" + giamsat;
        //    }
        //    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuyenGiaiQuyetKyHopSau)//3: Chuyển giải quyết kỳ họp sau
        //    {
        //        //string traloi_del = " <a onclick=\"Delete_Refresh('" + id_encr + "', '/Kiennghi/Ajax_Kiennghi_tra_chuyenkysau_del')\" href =\"javascript:void()\" title='Hủy' rel='tooltip' title=''><i class='icon-trash'></i></a>";
        //        string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chuyenkysau_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
        //        if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
        //        str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add +
        //            "</div><p class='b'>Chuyển theo dõi kỳ họp sau</p><strong>Lý do: </strong>" + traloi.CTRALOI + " " + file + "</div>" + giamsat;
        //    }
        //    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.CoLoTrinhGiaiQuyet)//Có lộ trình giải quyết
        //    {
        //        //string traloi_del = " <a onclick=\"Delete_Refresh('" + id_encr + "', '/Kiennghi/Ajax_Kiennghi_tra_chuyenkysau_del')\" href =\"javascript:void()\" title='Hủy' rel='tooltip' title=''><i class='icon-trash'></i></a>";
        //        string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_lotrinh_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
        //        if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
        //        string ngaydukien = "";
        //        if (traloi.DNGAY_DUKIEN != null) { ngaydukien = "<p><strong>Ngày dự kiến giải quyết dứt điểm: </strong> " + func.ConvertDateVN(traloi.DNGAY_DUKIEN.ToString()) + "</p>"; }
        //        str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add +
        //            "</div><p><strong>Có lộ trình giải quyết:</strong>" + traloi.CTRALOI + " " + file + "<p>" +
        //                ngaydukien + "</div>" + giamsat;
        //    }
        //    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoLoTrinhGiaiQuyet)//Chưa có lộ trình giải quyết
        //    {
        //        string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chualotrinh_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
        //        if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
        //        str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add + "</div><p class='b'>Chưa có lộ trình giải quyết</p><strong>Lý do: </strong>" + traloi.CTRALOI + " " + file + "</div>" + giamsat;
        //    }
        //    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaTheGiaiQuyetNgay)//Chưa thể giải quyết ngay
        //    {
        //        string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chuagiaiquyet_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
        //        if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
        //        str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add +
        //            "</div><p class='b'>Chưa thể giải quyết ngay:</p><strong>Lý do: </strong>" + traloi.CTRALOI + " " + file + "</div>" + giamsat;
        //    }
        //    if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoNguonLucGiaiQuyet)//Chưa có nguồn lực giải quyết
        //    {
        //        string traloi_add = " <a href =\"javascript:void()\" title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_nguonluc_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
        //        if (traloi.IUSER != act.iUser || g != null) { traloi_add = ""; }
        //        str = "<div class='alert alert-success kn_traloi'><div class='alert-icon'>" + traloi_add +
        //            "</div><p class='b'>Chưa có nguồn lực giải quyết:</p><strong>Lý do: </strong>" + traloi.CTRALOI + " " + file + "</div>" + giamsat;
        //    }
        //    return str;
        //}
        public string Detail_Traloi_Kiennghi(int iKienNghi)
        {
            string str = "";
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("IKIENNGHI", iKienNghi);
            var traloi_kiennghi = _kn.GetAll_TraLoi_KienNghi_ByParamt(_dic);
            if (traloi_kiennghi.Count() > 0)
            {
                KN_KIENNGHI_TRALOI traloi = traloi_kiennghi.FirstOrDefault();
                int iPhanloai=(int)traloi.IPHANLOAI;
                List<KN_TRALOI_PHANLOAI> phanloai_traloi = _kn.GetAll_KN_TRALOI_PHANLOAI();
                var phanloai_traloi1 = phanloai_traloi.Where(x => x.IPHANLOAI == iPhanloai).ToList();
                string phanloai = "";
                if (phanloai_traloi1.Count() > 0)
                {

                    KN_TRALOI_PHANLOAI phanloaitraloi = phanloai_traloi1.FirstOrDefault();
                    string noidung_phanloai = EncodeOutput(phanloaitraloi.CTEN);
                    if (phanloaitraloi.IPARENT != 0 && phanloai_traloi.Where(x => x.IPHANLOAI == (int)phanloaitraloi.IPARENT).Count() > 0)
                    {
                        KN_TRALOI_PHANLOAI phanloaitraloi_parent = phanloai_traloi.Where(x => x.IPHANLOAI == (int)phanloaitraloi.IPARENT).FirstOrDefault();
                        noidung_phanloai = EncodeOutput(phanloaitraloi_parent.CTEN) + " > " + noidung_phanloai;
                    }
                    phanloai = "<strong>Phân loại:</strong> " + noidung_phanloai + "</br>";
                }
                string congvan = "";
                if (traloi.CSOVANBAN != null)
                {
                    congvan = "<strong>Văn bản xử lý:</strong> Công văn số <strong>" + EncodeOutput(traloi.CSOVANBAN) + "</strong> ngày " + func.ConvertDateVN(traloi.DNGAYBANHANH.ToString())+"</br>";
                }
                string tinhtrangtraloi = "Trả lời kiến nghị";
                if (traloi.ITINHTRANG == 2) { tinhtrangtraloi = "Trả lại kiến nghị"; }
                string file = File_View((int)traloi.ITRALOI, "kn_traloi");
                str = "<p class='gachchan b'>"+ tinhtrangtraloi +"</p>"+ phanloai + congvan+"<strong>Kết quả xử lý:</strong> "+ EncodeOutput(traloi.CTRALOI)+". "+ file;
            }
            return str;
        }
        public string Content_Traloi(int iKienNghi)
        {
            string str = "";
            //KN_Kiennghi_TraloiRepository _traloi = new KN_Kiennghi_TraloiRepository();
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("IKIENNGHI", iKienNghi);

            var traloi_kiennghi = _kn.GetAll_TraLoi_KienNghi_ByParamt(_dic);
            if (traloi_kiennghi.Count() == 0) {
                return "Chưa có trả lời";
            }
            //KN_KIENNGHI_TRALOI traloi = traloi_kiennghi.FirstOrDefault();

           
            str = Detail_Traloi_Kiennghi(iKienNghi);
            //string phanloai = "";
            //List<KN_TRALOI_PHANLOAI> phanloai_traloi = _kn.GetAll_KN_TRALOI_PHANLOAI();
            //phanloai_traloi = phanloai_traloi.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).ToList();
            //if (phanloai_traloi.Count() > 0)
            //{
            //    phanloai = "<p><strong>Phân loại:</strong> " + phanloai_traloi.FirstOrDefault().CTEN + "</p>";
            //}
            //if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.DaCoTraLoi)//1: Đã có trả lời,
            //{                
            //    str = "<p><strong>"+ StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.DaCoTraLoi) +":</strong> " + traloi.CTRALOI + " " + file + "</p>"+ phanloai;

            //}
            //if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.TraLaiKienNghi)//2:Trả lại kiến nghị
            //{
            //    str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.TraLaiKienNghi) + ":</strong> " + traloi.CTRALOI + " " + file + "</p>" + phanloai;
            //}

            //string danhgia = DanhGia_TraLoi_KienNghi(iKienNghi, (int)traloi.ITRALOI);
            //if (danhgia != "")
            //{
            //    str += "<p style='text-decoration: underline;'><strong>Đánh giá kết quả trả lời: </strong>" + DanhGia_TraLoi_KienNghi(iKienNghi, (int)traloi.ITRALOI);
            //}            
            return str;
        }
        public string Content_Traloi_LanDau(int iKienNghi, KN_KIENNGHI_TRALOI traloi)
        {
            string str = "";
            string file = File_View((int)traloi.ITRALOI, "kn_traloi");
            string phanloai = "";
            List<KN_TRALOI_PHANLOAI> phanloai_traloi = _kn.GetAll_KN_TRALOI_PHANLOAI();
            phanloai_traloi = phanloai_traloi.Where(x => x.IPHANLOAI == (int)traloi.IPHANLOAI).ToList();
            if (phanloai_traloi.Count() > 0)
            {
                phanloai = "<p><strong>Phân loại:</strong> " + EncodeOutput(phanloai_traloi.FirstOrDefault().CTEN) + "</p>";
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.DaCoTraLoi)//1: Đã có trả lời,
            {

                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.DaCoTraLoi) + ":</strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;

            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.TraLaiKienNghi)//2:Trả lại kiến nghị
            {
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.TraLaiKienNghi) + ":</strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuyenGiaiQuyetKyHopSau)//3: Chuyển giải quyết kỳ họp sau
            {
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.ChuyenGiaiQuyetKyHopSau) + ":</strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.CoLoTrinhGiaiQuyet)//Có lộ trình giải quyết
            {
                string ngaydukien = "";
                if (traloi.DNGAY_DUKIEN != null)
                {
                    ngaydukien = "<p><strong>Ngày dự kiến giải quyết dứt điểm: </strong>" + func.ConvertDateVN(traloi.DNGAY_DUKIEN.ToString()) + "</p>";
                }
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.CoLoTrinhGiaiQuyet) + ": </strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + ngaydukien + "</p>" + phanloai;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoLoTrinhGiaiQuyet)//Chưa có lộ trình giải quyết
            {
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.ChuaCoLoTrinhGiaiQuyet) + ": </strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaTheGiaiQuyetNgay)//Chưa thể giải quyết ngay
            {
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.ChuaTheGiaiQuyetNgay) + ": </strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;
            }
            if (traloi.ITINHTRANG == (decimal)TrangThai_TraLoiKienNghi.ChuaCoNguonLucGiaiQuyet)//Chưa có nguồn lực giải quyết
            {
                str = "<p><strong>" + StringEnum.GetStringValue(TrangThai_TraLoiKienNghi.ChuaCoNguonLucGiaiQuyet) + ": </strong> " + EncodeOutput(traloi.CTRALOI) + " " + file + "</p>" + phanloai;
            }
            return str;
        }
        public string Btn_DLL_Traloi_kiennghi(string id_encr)
        {
            string str = "";
            str = "<a onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_traloi')\" class='btn btn-primary' href='javascript:void(0)'><i class='icon-plus-sign'></i> Trả lời kiến nghị</a>";
            //str = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn dropdown-toggle'><i class='icon-plus-sign'></i> Chọn kết quả xử lý <span class='caret'></span></a>" +
            //    "<ul class='dropdown-menu dropdown-primary'>" +
            //    "<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_traloi')\"><a href='#'>Trả lời kiến nghị</a></li>" +
            //    "<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra')\"><a href='#'>Trả lại kiến nghị</a></li>" +
            //    //"<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chuyenkysau')\"><a href='#'>Chuyển theo dõi kỳ họp sau</a></li>" +
            //    //"<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_lotrinh')\"><a href='#'>Có lộ trình giải quyết</a></li>" +
            //    //"<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chualotrinh')\"><a href='#'>Chưa có lộ trình giải quyết</a></li>" +
            //    //"<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chuagiaiquyet')\"><a href='#'>Chưa thể giải quyết ngay</a></li>" +
            //    //"<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_nguonluc')\"><a href='#'>Chưa có nguồn lực giải quyết</a></li>" +
            //    "</ul></div>";
            return str;
        }
        //public string Btn_DLL_Traloi_kiennghi_chuyenkysau(string id_encr)
        //{
        //    string str = "";
        //    str = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn dropdown-toggle'><i class='icon-plus-sign'></i> Chọn kết quả xử lý <span class='caret'></span></a>" +
        //        "<ul class='dropdown-menu dropdown-primary'>" +
        //        "<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_traloi')\"><a href='#'>Trả lời kiến nghị</a></li>" +
        //        "<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra')\"><a href='#'>Trả lại kiến nghị</a></li>" +
        //        //"<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chuyenkysau')\"><a href='#'>Chuyển theo dõi kỳ họp sau</a></li>" +
        //        "<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_lotrinh')\"><a href='#'>Có lộ trình giải quyết</a></li>" +
        //        "<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chualotrinh')\"><a href='#'>Chưa có lộ trình giải quyết</a></li>" +
        //        "<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_chuagiaiquyet')\"><a href='#'>Chưa thể giải quyết ngay</a></li>" +
        //        "<li onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_tra_nguonluc')\"><a href='#'>Chưa có nguồn lực giải quyết</a></li>" +
        //        "</ul></div>";
        //    return str;
        //}
        public string List_KienNghi_ByID_tonghop_chuyenxuly(List<KN_KIENNGHI> kiennghi, List<QUOCHOI_COQUAN> coquan, List<KN_KIENNGHI_TRALOI> traloi,
                                            List<KN_GIAMSAT> giamsat, TaikhoanAtion act, string url_key, bool remove)
        {
            string str = "";
            kiennghi = kiennghi.OrderByDescending(x => x.IPARENT).ToList();
            if (kiennghi.Count() == 0)
            {
                return "<tr><td colspan='4' class='alert tcenter'>Chưa có kiến nghị nào được thêm vào</td></tr>";
            }
            int count = 1;
            List<decimal> list_id_kiennghi_parent = new List<decimal>();
            foreach (var k in kiennghi)
            {
                if (!list_id_kiennghi_parent.Contains((decimal)k.ID_KIENNGHI_PARENT)) {
                    list_id_kiennghi_parent.Add((decimal)k.ID_KIENNGHI_PARENT);
                }
            }
            list_id_kiennghi_parent.Reverse();
            foreach (var l in list_id_kiennghi_parent)
            {
                if (l > 0)
                {
                    //lấy nhóm KN cùng nội dung
                    KN_KIENNGHI k = kiennghi.Where(x => x.IKIENNGHI == l).FirstOrDefault();
                    str += Row_KienNghi_ByID_tonghop_chuyenxuly(kiennghi, k, coquan, traloi, giamsat, act, count, url_key, remove, true);
                    count++;
                } else
                {
                    // Lấy nhóm KN riêng lẻ
                    var kiennghi_child = kiennghi.Where(x => x.ID_KIENNGHI_PARENT == l ).ToList();
                    foreach (var k1 in kiennghi_child)
                    {
                        if (kiennghi.Where(x => x.ID_KIENNGHI_PARENT == k1.IKIENNGHI).Count() == 0)
                        {
                            str += Row_KienNghi_ByID_tonghop_chuyenxuly(kiennghi, k1, coquan, traloi, giamsat, act, count, url_key, remove, false);
                            count++;
                        }
                       
                    }
                }

            }
            //foreach (var k in kiennghi)
            //{
            //    string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_key);
            //    string giamsat_add_ = " <a href =\"javascript:void()\" title='Đánh giá trả lời' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Kiennghi/Ajax_Kiennghi_danhgia')\" class='trans_func'><i class='icon-edit'></i></a> ";
            //    string noidung_traloi = "";
            //    string noidung_giamsat = "";
            //    string traloi_add_ = Btn_DLL_Traloi_kiennghi(id_encr);
            //    if (!base_bussiness.Action_(7, act)) { traloi_add_ = ""; }
            //    if (traloi.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).Count() > 0)
            //    {
            //        KN_KIENNGHI_TRALOI kn_traloi = traloi.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).FirstOrDefault();
            //        noidung_traloi = Info_Traloi_GiamSat_kiennghi(kn_traloi, act, url_key);                    
            //        if (giamsat.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).Count() > 0)
            //        {
            //            giamsat_add_ = "";
            //        }
            //        traloi_add_ = "";
            //    }
            //    else { giamsat_add_ = ""; }
            //    if (remove == false) { traloi_add_ = ""; }
            //    string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
            //    string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
            //    str += "<tr id='tr_" + k.IKIENNGHI + "'><td class='tcenter b'>" + count + "</td><td class='tcenter'>" + k.CMAKIENNGHI + "</td><td><p>" + k.CNOIDUNG +
            //        "</p></td><td>"+ traloi_add_ + noidung_traloi + noidung_giamsat+"</td><td nowrap class='tcenter'>" + giamsat_add_ + info+ bt_lichsu + "</td></tr>";
            //    count++;
            //}
            return str;
        }
        public string Row_KienNghi_ByID_tonghop_chuyenxuly(List<KN_KIENNGHI> kiennghi, KN_KIENNGHI k, List<QUOCHOI_COQUAN> coquan,
               List<KN_KIENNGHI_TRALOI> traloi, List<KN_GIAMSAT> giamsat,
               TaikhoanAtion act, int count, string url_key, bool remove, bool sup)
        {
            string str = "";
            string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_key);
            string giamsat_add_ = "  ";
            string noidung_traloi = "";
            string noidung_giamsat = "";
            string traloi_add_ = Btn_DLL_Traloi_kiennghi(id_encr);
            if (!base_bussiness.ActionMulty_("7,50", act) ) { traloi_add_ = ""; }
            if (traloi.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).Count() > 0)
            {
                KN_KIENNGHI_TRALOI kn_traloi = traloi.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).FirstOrDefault();
                giamsat_add_ = "<a href =\"javascript:void()\" title='Đánh giá trả lời' rel='tooltip' title='' onclick=\"ShowPopUp('id=" +
                    HashUtil.Encode_ID(kn_traloi.ITRALOI.ToString(), url_key) + "','/Kiennghi/Ajax_Kiennghi_danhgia')\" class='trans_func'><i class='icon-edit'></i></a> ";
                noidung_traloi = Info_Traloi_GiamSat_kiennghi(kn_traloi, act, url_key);
                if (giamsat.Where(x => x.IKIENNGHI == (int)k.IKIENNGHI).Count() > 0)
                {
                    giamsat_add_ = "";
                }
                traloi_add_ = "";
            }
            else { giamsat_add_ = ""; }
            if (remove == false) { traloi_add_ = ""; }
            string doan_tiepnhan = str_donvi_tiepnhan((int)k.IDONVITIEPNHAN, coquan);
            if (k.ID_KIENNGHI_PARENT > 0) {

                giamsat_add_ = "";
                traloi_add_ = "";
            }
            if (!base_bussiness.ActionMulty_("8,50",act))
            {
                giamsat_add_ = "";
            }
            
            string noidung_kiennghi = func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG), id_encr) +  doan_tiepnhan;
            //if (k.IPARENT == 1) {
            //    noidung_kiennghi = "<div class='b'><p class='f-orangered'>[Kiến nghị Tập hợp các nội dung]</p>" +
            //    func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG), id_encr) + "</div>" + doan_tiepnhan;
            //}
            if (sup)
            {
                //noidung_kiennghi = "<p class='b'>" + k.CNOIDUNG_TRUNG + "</p><div style='padding-left:20px;font-style: italic;'>" + noidung_kiennghi + "</div>";
                noidung_kiennghi = "<div class='b'>" + EncodeOutput(k.CNOIDUNG_TRUNG) + "</div>";
                string noidung_child = "";
                var kiennghi_child = kiennghi.Where(x => x.ID_KIENNGHI_PARENT == (int)k.IKIENNGHI).ToList();
                kiennghi_child.Add(k);
                int count1 = 1;
                foreach (var child in kiennghi_child)
                {
                    if (count1 > 1) { noidung_child += "<div class='p_dr'></div>"; }
                    noidung_child += "<p><strong>" + count1 + ". Kiến nghị:</strong><em>" + func.TomTatNoiDung(EncodeOutput(child.CNOIDUNG), child.IKIENNGHI.ToString()) + "</em></br>" + str_donvi_tiepnhan((int)child.IDONVITIEPNHAN, coquan) + "</p>";
                    count1++;
                }
                noidung_kiennghi += "<p><a href='javascript:void(0)' onclick=\"$('#noidung_" + k.IKIENNGHI + "').toggle(500);\"><i class='icon-list-ol'></i> Xem các kiến nghị cùng nội dung (" + kiennghi_child.Count() + ")</a></p>" +
                                    "<div style='display:none' id='noidung_" + k.IKIENNGHI + "'>" + noidung_child + "</div>";
            }
            string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
            string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý' rel='tooltip' title='' class='trans_func'><i class='icon-time'></i></a>";
            str += "<tr id='tr_" + k.IKIENNGHI + "'><td class='tcenter b'>" + count + "</td><td >" + noidung_kiennghi + "</td><td >" + traloi_add_ + noidung_traloi + noidung_giamsat + "</td><td nowrap class='tcenter'>" + giamsat_add_ + info + bt_lichsu + "</td></tr>";
            return str;
        }
        public string str_donvi_tiepnhan(int IDONVITIEPNHAN, List<QUOCHOI_COQUAN> coquan)
        {
            string doan_tiepnhan = "";
            coquan = coquan.Where(x => x.ICOQUAN == IDONVITIEPNHAN).ToList();
            if (coquan.Count() > 0) { doan_tiepnhan = "<p><strong>Đơn vị tiếp nhận: </strong>" +
                    EncodeOutput(coquan.FirstOrDefault().CTEN) + "<p>"; }
            return doan_tiepnhan;
        }
        //public string List_KienNghi_ByID_tonghop_view(List<KN_KIENNGHI> kiennghi, string url_key)
        //{
        //    string str = "";
        //    kiennghi= kiennghi.OrderByDescending(x=>x.IPARENT).ToList();
        //    if (kiennghi.Count() == 0)
        //    {
        //        return "<tr><td colspan='4' class='alert tcenter'>Chưa có kiến nghị nào được thêm vào</td></tr>";
        //    }
        //    int count = 1;
        //    foreach (var k in kiennghi)
        //    {
        //        string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_key);
        //        string noidung = func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG), id_encr);
        //        //string doan_tiepnhan = str_donvi_tiepnhan((int)k.IDONVITIEPNHAN, coquan);
        //        string bold = "";
        //        if (k.IPARENT ==1) {
        //            noidung = "<div class='b'><p class='f-orangered'>[Kiến nghị Tập hợp các nội dung]</p>" + noidung+"</div>";
        //        }
        //        string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
        //        str += "<tr id='tr_" + k.IKIENNGHI + "'><td class='tcenter b'>" + count + "</td><td class='"+bold+"'>" + noidung + "</td><td>" + Str_tinhtrang_kiennghi(k) + "</td><td class='tcenter'>" + info + "</td></tr>";
        //        count++;
        //    }
        //    return str;
        //}
        public string List_KienNghiByTonghop_download(List<PRC_KIENNGHI_BYTONGHOP> kiennghi)
        {
            string str = "";
            if (kiennghi.Count() == 0)
            {
                return "<tr><td colspan='4' class='alert tcenter'>Chưa có kiến nghị nào được thêm vào</td></tr>";
            }
            int count = 1;
            foreach(var k in kiennghi)
            {
                string tiepnhan = k.TENDONVITIEPNHAN;
                if (k.ID_GOP < 0) { tiepnhan = k.TENDONVITIEPNHAN_GOP; }
                str += "<tr><td class='tcenter'>"+count+"</td><td>"+k.CNOIDUNG+"</td><td class='tcenter'>"+ tiepnhan + "</td><td>"+k.CDIACHI+"</td></tr>";
                    count++;
            }
            return str;
        }
        public string List_KienNghi_ByID_tonghop_view_new(KN_TONGHOP tonghop,List<PRC_KIENNGHI_BYTONGHOP> kiennghi, string url_key)
        {
            string str = "";
            if (kiennghi.Count() == 0)
            {
                return "<tr><td colspan='4' class='alert tcenter'>Chưa có kiến nghị nào được thêm vào</td></tr>";
            }
            int count = 1;
            var kiennghi1 = kiennghi;
            if (tonghop.IDONVITONGHOP == (decimal)ID_Capcoquan.Bandannguyen)
            {
                kiennghi1 = kiennghi.Where(x => x.ID_GOP <= 0).ToList();
            }            
            List<decimal> list_id_kn = new List<decimal>();
            foreach (var k in kiennghi1)
            {
                if (!list_id_kn.Contains(k.IKIENNGHI))
                {
                    list_id_kn.Add(k.IKIENNGHI);
                    string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_key);
                    string noidung = func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG), id_encr);
                    string ten_donvi_gop = k.TENDONVITIEPNHAN;
                    if (k.ID_GOP == -1)
                    {
                        ten_donvi_gop = k.TENDONVITIEPNHAN_GOP;
                    }
                    //string doan_tiepnhan = str_donvi_tiepnhan((int)k.IDONVITIEPNHAN, coquan);
                    string bold = "";
                    string nguondon = k.CNGUONDON;
                    string info = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_info/?id=" + id_encr + "' title='Chi tiết' rel='tooltip' title='' class='trans_func'><i class='icon-info-sign'></i></a>";
                    str += "<tr id='tr_" + k.IKIENNGHI + "'><td class='tcenter b'>" + count + "</td><td class='" + bold + "'>" + noidung +
                        "</td><td class='tcenter'>" + EncodeOutput(ten_donvi_gop) + "</td><td>" + nguondon + "</td><td class='tcenter'>" + info + "</td></tr>";
                    count++;
                }
                
            }
            return str;
        }
        public string KN_ListKienNghi_ThemVaoKienNghiGop(List<PRC_KIENNGHI_MOICAPNHAT> kiennghi, int id_kiennghi_gop, TaikhoanAtion act)
        {
            
            if (kiennghi.Count() == 0)
            {
                return "<tr><td class='alert-danger tcenter'>Không tìm thấy kiến nghị nào</td></tr>";
            }
            string tiepnhan = "";
            if (act.is_lanhdao)
            {
                tiepnhan = "<th width='15%' class='tcenter' nowrap>Tiếp nhận</th>";
            }
            string str = "<tr><th width='3%' nowrap>STT</th><th nowrap>Nội dung kiến nghị</th>"+ tiepnhan + "<th width='10%' class='tcenter' nowrap>Chọn</th></tr>";
            int count = 1;
            foreach (var k in kiennghi)
            {
                string doantonghop = "";
                if (act.is_lanhdao)
                {
                    doantonghop = "<td class='tcenter'>" + EncodeOutput(k.TEN_DONVITIEPNHAN) + "</strong>";
                }
                string input = "<input onclick=\"UpdateStatus('id_kiennghi_gop=" + id_kiennghi_gop + "&id_kiennghi=" + k.ID_KIENNGHI + "', '/Kiennghi/Ajax_Chonkiennghi_gop')\" type='checkbox' name='kn_chon'/>";
                str += "<tr><td class='tcenter b'>" + count + "</td><td>" + EncodeOutput(k.NOIDUNG_KIENNGHI) +
                    "</td>" + doantonghop + "<td class='tcenter'>" + input + "</td></tr>";
                count++;
            }
            return str;
        }
        public string KN_ListKienNghiThemVaoTongHop(List<PRC_KIENNGHI_MOICAPNHAT> kiennghi, int id_tonghop, TaikhoanAtion act)
        {
            if (kiennghi.Count() == 0)
            {
                return "<tr><td class='alert-danger tcenter'>Không tìm thấy kiến nghị nào</td></tr>";
            }
            string tiepnhan = "";
            if (act.is_lanhdao)
            {
                tiepnhan = "<th width='15%' class='tcenter' nowrap>Tiếp nhận</th>";
            }
            string str = "<tr><th width='3%' nowrap>STT</th><th nowrap>Nội dung kiến nghị</th>" + tiepnhan + "<th width='10%' class='tcenter' nowrap>Chọn</th></tr>";
            int count = 1;
            List<decimal> list_id_kn = new List<decimal>();
            var kiennghi1 = kiennghi.Where(x => x.ID_GOP <= 0).ToList();
            foreach (var k in kiennghi1)
            {
                if (!list_id_kn.Contains(k.ID_KIENNGHI))
                {
                    list_id_kn.Add(k.ID_KIENNGHI);
                    string doantonghop = "";
                    string donvi_tiepnhan = k.TEN_DONVITIEPNHAN;
                    if (k.ID_GOP < 0)
                    {
                        donvi_tiepnhan = k.TENDONVITIEPNHAN_GOP;
                        //List<string> donvi_nhan = new List<string>();
                        //var kiennghi_gop = kiennghi.Where(x => x.ID_GOP == k.ID_KIENNGHI).ToList();
                        //int count_gop = 0;
                        //foreach (var d in kiennghi_gop)
                        //{
                        //    if (!donvi_nhan.Contains(d.TENDONVITIEPNHAN_GOP) && d.TENDONVITIEPNHAN_GOP!=null)
                        //    {
                        //        if (count_gop > 0) { donvi_tiepnhan += ", "; }
                        //        donvi_tiepnhan += d.TENDONVITIEPNHAN_GOP; count_gop++;
                        //    }
                        //}
                    }
                    //if (donvi_tiepnhan == "") { donvi_tiepnhan = k.TEN_DONVITIEPNHAN; }
                    if (act.is_lanhdao)
                    {
                        doantonghop = "<td class='tcenter'>" + EncodeOutput(donvi_tiepnhan) + "</strong></td>";
                    }
                    string input = "<input onclick=\"UpdateStatus('id_tonghop=" + id_tonghop + "&id_kiennghi=" + k.ID_KIENNGHI + "', '/Kiennghi/Ajax_Chonkiennghi_tonghop')\" type='checkbox' name='kn_chon'/>";
                    str += "<tr><td class='tcenter b'>" + count + "</td><td>" + EncodeOutput(k.NOIDUNG_KIENNGHI) +
                        "</td>" + doantonghop + "<td class='tcenter'>" + input + "</td></tr>";
                    count++;
                }
                
            }
            return str;
        }
        public string KN_ListKienNghiThemVaoGiamSat(List<PRC_LIST_KN_TRALOI_DANHGIA> kiennghi, List<KN_DOANGIAMSAT_KIENNGHI> doan_kiennghi,int id_giamsat, TaikhoanAtion act)
        {
            string str = "";
            //KN_Doangiamsat_kiennghiRepository _doan_kiennghi = new KN_Doangiamsat_kiennghiRepository();
            if (kiennghi.Count() == 0)
            {
                return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy kiến nghị nào</td></tr>";
            }
            int count = 1;
            kiennghi = kiennghi.Where(x => x.ID_GOP <= 0).ToList();
            List<decimal> list_id_kiennghi = new List<decimal>();
            foreach (var k in kiennghi)
            {
                //string doantonghop = "<p>Đoàn Tập hợp: <strong>" + coquan.Where(x => x.ICOQUAN == (int)k.IDONVITIEPNHAN).FirstOrDefault().CTEN + "</strong></p>";
                //string thamquyen = "<p>Thẩm quyền xử lý: <strong>" + coquan.Where(x => x.ICOQUAN == (int)k.ITHAMQUYENDONVI).FirstOrDefault().CTEN + "</strong></p>";
                if (!list_id_kiennghi.Contains(k.ID_KIENNGHI))
                {
                    list_id_kiennghi.Add(k.ID_KIENNGHI);
                    if (doan_kiennghi.Where(x => x.IKIENNGHI == (int)k.ID_KIENNGHI).Count() == 0)
                    {
                        string input = "<input onclick=\"UpdateStatus('id_giamsat=" + id_giamsat + "&id_kiennghi=" + k.ID_KIENNGHI + "', '/Kiennghi/Ajax_Chonkiennghi_giamsat')\" type='checkbox' name='kn_chon' value='" + k.ID_KIENNGHI + "'/>";
                        str += "<tr><td class='tcenter b'>" + count + "</td><td><p>" + func.TomTatNoiDung(EncodeOutput(k.NOIDUNG_KIENNGHI), k.ID_KIENNGHI.ToString()) +
                            "</p></td><td>" + Content_Traloi_kiennghi(k) + "</td><td class='tcenter'>" + input + "</td></tr>";
                        count++;
                    }
                }
                

            }
            return str;
        }
        //public string Option_LinhVuc_CoQuan(List<LINHVUC_COQUAN> linhvuc, int id_choice = 0)
        //{
        //    string str = "";
        //    foreach (var t in linhvuc)
        //    {
        //        string select = ""; if (t.ILINHVUC == id_choice) { select = " selected "; }
        //        str += "<option " + select + " value='" + t.ILINHVUC + "'>" + EncodeOutput(t.CTEN) + "</option>";
        //    }
        //    return str;
        //}

        // Bo sung Ma Linh Vuc vao phan hien thi
        public string Option_LinhVuc_CoQuan(List<LINHVUC_COQUAN> linhvuc, int id_choice = 0)
        {
            string str = "";
            foreach (var t in linhvuc)
            {
                string select = ""; if (t.ILINHVUC == id_choice) { select = " selected "; }
                str += "<option " + select + " value='" + t.ILINHVUC + "'>";
                if (t.IPARENT != 0)
                {
                    str += "- - - ";
                    var linhVucCha = _kn.GetBy_Linhvuc_CoquanID((int)(t.IPARENT));
                    while (linhVucCha.IPARENT != 0)
                    {
                        str += "- - - ";
                        linhVucCha = _kn.GetBy_Linhvuc_CoquanID((int)(linhVucCha.IPARENT));
                    }
                }
                str +=    t.CCODE + "-" + EncodeOutput(t.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_LinhVuc_CoQuan_Optgroup(List<LINHVUC_COQUAN> linhvuc, int id_choice = 0)
        {
            string str = "";
            string select = "";
            int count = 0;
            var linhvucChaList = linhvuc.Where(x => x.IPARENT == 0).ToList();
            foreach (var t in linhvucChaList)
            {
                var linhvucCon = linhvuc.Where(x => x.IPARENT == t.ILINHVUC).ToList();
                if (linhvucCon != null && linhvucCon.Count > 0)
                {
                    str += "<optgroup label='" + t.CCODE + "-" + EncodeOutput(t.CTEN) + "' value='" + t.ILINHVUC + "'>";
                    foreach(var c in linhvucCon)
                    {
                        str += Recursive_Option_LinhVuc_CoQuan_Optgroup(linhvuc, c, id_choice, ref count);
                    }
                    str += "</optgroup>";
                }
                else
                {
                    if (t.ILINHVUC == id_choice) { select = " selected "; }
                    str += "<option " + select + " value='" + t.ILINHVUC + "'>";
                    str += t.CCODE + "-" + EncodeOutput(t.CTEN) + "</option>";
                }
            }
            return str;
        }

        public string Recursive_Option_LinhVuc_CoQuan_Optgroup(List<LINHVUC_COQUAN> linhVucLst, LINHVUC_COQUAN linhVuc, int id_choice, ref int count)
        {
            string str = "";
            string select = "";
            var linhvucConList = linhVucLst.Where(x => x.IPARENT == linhVuc.ILINHVUC).ToList();
            if (linhvucConList != null && linhvucConList.Count > 0)
            {
                count++;
                str += "<optgroup label='" + Enumerable.Repeat(" ", count + 1) + EncodeOutput(linhVuc.CTEN) + "' value='" + linhVuc.ILINHVUC + "'>";
                foreach (var item in linhvucConList)
                {
                    str += Recursive_Option_LinhVuc_CoQuan_Optgroup(linhVucLst, item, id_choice, ref count);
                }
                str += "</optgroup>";
            }
            else
            {
                if (linhVuc.ILINHVUC == id_choice) { select = " selected "; }
                str += "<option " + select + " value='" + linhVuc.ILINHVUC + "'>" + EncodeOutput(linhVuc.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_KN_Traloi_Phanloai(List<KN_TRALOI_PHANLOAI> phanloai, int id_parent = 0, int level = 0, int id_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var phanloai1 = phanloai.Where(x => x.IPARENT == id_parent).OrderBy(x => x.IVITRI).ToList();
            foreach (var t in phanloai1)
            {
                var phanloai2 = phanloai.Where(x => x.IPARENT == (int)t.IPHANLOAI);
                string select = ""; if (t.IPHANLOAI == id_choice) { select = "selected"; }
                string disabled = "";
                if (phanloai2.Count() > 0)
                {
                    disabled = " disabled ";
                }
                str += "<option " + disabled + " " + select + " value=" + t.IPHANLOAI + ">" + space_level + EncodeOutput(t.CTEN) + "</option>";
                if (phanloai2.Count() > 0)
                {
                    str += Option_KN_Traloi_Phanloai(phanloai, (int)t.IPHANLOAI, level + 1, id_choice);
                }
            }
            return str;
        }
        public string Option_Coquan_LinhVuc(List<QUOCHOI_COQUAN> coquan, List<LINHVUC_COQUAN> linhvuc, int id_parent = 0, int level = 0)
        {
            string str = "";
            string space = "";
            for(int i = 0; i < level; i++)
            {
                space += "- - - ";
            }
            var coquan0 = coquan.Where(x => x.IPARENT == id_parent && x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
            foreach (var c in coquan0)
            {
                var coquan1 = coquan.Where(x => x.IPARENT == (int)c.ICOQUAN && x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
                if (coquan1.Count == 0)
                {
                    
                    var linhvuc1 = linhvuc.Where(x => x.ICOQUAN == (int)c.ICOQUAN).ToList();
                    if (linhvuc1.Count() > 0)
                    {
                        str += "<optgroup label='" + EncodeOutput(c.CTEN.ToUpper()) + "'>";
                        //str += "<option value=''>" + space + c.CTEN.ToUpper() + "</option>";
                        foreach (var l in linhvuc1)
                        {
                            str += "<option value='" + l.ILINHVUC + "'>" + EncodeOutput(l.CTEN) + "</option>";
                        }
                        str += "</optgroup>";
                    }
                    
                }else
                {
                    str += "<optgroup label='" + EncodeOutput(c.CTEN.ToUpper()) + "'>";
                    str += Option_Coquan_LinhVuc(coquan, linhvuc, (int)c.ICOQUAN, level + 1);
                    str += "</optgroup>";
                }
            }
            return str;
        }
        public string Option_LinhVuc(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0,int id_choice=0)
        {
            string str = "";
            string[] nhom = { "Hành chính", "Tư pháp", "Khác" };
            int i = 1;
            foreach(var n in nhom)
            {
                var linhvuc_nhom = linhvuc.Where(x => x.INHOM == i).ToList();
                if (linhvuc_nhom.Count() > 0)
                {
                    str += "<optgroup label='" + n + "'>" + Option_LinhVuc_By_Nhom(linhvuc_nhom,id_parent,level,id_choice) + "</optgroup>";
                }                
                i++;
            }
            //for (int i = 0; i < level; i++) { space_level += "- - - "; }
            //var linhvuc1 = linhvuc.Where(x => x.IPARENT == id_parent).OrderBy(x => x.CTEN).ToList();
            //foreach (var t in linhvuc1)
            //{
            //    string select = ""; if (t.ILINHVUC == id_choice) { select = "selected"; }
            //    str += "<option "+ select + " value=" + t.ILINHVUC + ">" + space_level + t.CTEN + "</option>";
            //    str += Option_LinhVuc(linhvuc, (int)t.ILINHVUC, level + 1,id_choice);
            //}
            return str;
        }
        public string Option_LinhVuc_By_Nhom(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int id_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var linhvuc1 = linhvuc.Where(x => x.IPARENT == id_parent).OrderBy(x => x.CTEN).ToList();
            foreach (var t in linhvuc1)
            {
                string select = ""; if (t.ILINHVUC == id_choice) { select = "selected"; }
                str += "<option " + select + " value=" + t.ILINHVUC + ">" + space_level + EncodeOutput(t.CTEN) + "</option>";
                str += Option_LinhVuc_By_Nhom(linhvuc, (int)t.ILINHVUC, level + 1, id_choice);
            }
            return str;
        }
        public string Option_GiamSat_PhanLoai(List<KN_GIAMSAT_PHANLOAI> phanloai, int id_choice)
        {
            string str = "";
            foreach(var g in phanloai)
            {
                string select = ""; if (g.IPHANLOAI == id_choice) { select = "selected"; }
                str += "<option "+select+" value='"+g.IPHANLOAI+"'>"+ EncodeOutput(g.CTEN)+"</option>";
            }
            return str;
        }
        public string Option_GiamSat_Danhgia(List<KN_GIAMSAT_DANHGIA> phanloai, int id_choice)
        {
            string str = "";
            foreach (var g in phanloai)
            {
                string select = ""; if (g.IDANHGIA == id_choice) { select = "selected"; }
                str += "<option " + select + " value='" + g.IDANHGIA + "'>" + EncodeOutput(g.CTEN) + "</option>";
            }
            return str;
        }
        public string KN_Tonghop_Kiennghi_Diaphuong(List<KN_KIENNGHI> kiennghi, TaikhoanAtion act)
        {
            string str = "";
            Quochoi_CoquanRepository _coquan = new Quochoi_CoquanRepository();
            if (kiennghi.Count() == 0) { return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy kiến nghị nào</td></tr>"; }
            
            string url_cookie = func.Get_Url_keycookie();
            int count1 = 1;
            foreach (var k in kiennghi)
            {
                string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_cookie);
                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý'  class='trans_func'><i class='icon-time'></i></a>";
                string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiennghi_info?id=" + id_encr + "\" title='Xem chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a> ";
                string tinhtrang = Str_tinhtrang_kiennghi(k);
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count1 + "</td><td>" + func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG), id_encr) + "</td><td>" + tinhtrang + "</td><td class='tcenter' nowrap>" + chitiet + bt_lichsu + "</td></tr>";
                count1++;
            }
            return str;
        }
        public string KN_Tonghop_Kiennghi_ChuyenDanNguyen(List<KN_KIENNGHI> kiennghi, TaikhoanAtion act)
        {
            string str = "";
            Quochoi_CoquanRepository _coquan = new Quochoi_CoquanRepository();
            if (kiennghi.Count() == 0) { return "<tr><td colspan='4' class='alert-danger tcenter'>Không tìm thấy kiến nghị nào</td></tr>"; }

            string url_cookie = func.Get_Url_keycookie();
            int count1 = 1;
            foreach (var k in kiennghi)
            {
                string id_encr = HashUtil.Encode_ID(k.IKIENNGHI.ToString(), url_cookie);
                string bt_lichsu = " <a onclick=\"ShowPageLoading()\" href='/Kiennghi/Kiennghi_lichsu?id=" + id_encr + "' title='Lịch sử xử lý'  class='trans_func'><i class='icon-time'></i></a>";
                string chitiet = " <a onclick=\"ShowPageLoading()\" href=\"/Kiennghi/Kiennghi_info?id=" + id_encr + "\" title='Xem chi tiết'  class='trans_func'><i class='icon-info-sign'></i></a> ";
                string tinhtrang = Str_tinhtrang_kiennghi(k);
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count1 + "</td><td>" + func.TomTatNoiDung(EncodeOutput(k.CNOIDUNG),id_encr) + "</td><td>" + tinhtrang + "</td><td class='tcenter' nowrap>" + chitiet + bt_lichsu + "</td></tr>";
                count1++;
            }
            return str;
        }
        //public string Baocao_Kiennghi_6(List<QUOCHOI_COQUAN> coquan, int iKyHop)
        //{
        //    //phụ lục 6
        //    string str = "";

        //    return str;
        //}
        //public string Baocao_Kiennghi_1(List<QUOCHOI_COQUAN> coquan,int iKyHop)
        //{
        //    //phụ lục 1
        //    string str = "";
        //    var coquan1 = coquan.Where(x => x.IPARENT == 0 && x.IHIENTHI==1).OrderBy(x => x.IVITRI).ToList();
        //    int count = 1;
        //    string tong_baocao = "Cộng ";
        //    int tong_coquan = 0; int tong_coquan_datraloi = 0;
        //    int tong_coquan_dagiaiquyet = 0; int tong_coquan_danggiaiquyet = 0;
        //    int tong_coquan_giaitrinh = 0;
        //    foreach (var c in coquan1)
        //    {
        //        string num_roman = Convert_to_RomanNumber(count);
        //        if (count > 1) { tong_baocao += " + ";  }
        //        tong_baocao += num_roman;
        //        var coquan2= coquan.Where(x => x.IPARENT == (int)c.ICOQUAN && x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
        //        str += "<tr><td text-align='center' class='tcenter' colspan='7'><strong>" + num_roman + " . " + c.CTEN+"</strongg></td></tr>";
        //        int count1 = 1;
        //        int tong_coquan1 = 0; int tong_coquan1_datraloi = 0;
        //        int tong_coquan1_dagiaiquyet = 0; int tong_coquan1_danggiaiquyet = 0;
        //        int tong_coquan1_giaitrinh = 0; 
        //        foreach (var c1 in coquan2)
        //        {
        //            int tong = _kn.Count_KienNghi_BoNganh(iKyHop, (int)c1.ICOQUAN, -1);
        //            if (tong > 0)
        //            {
        //                tong_coquan1 += tong;
        //                int tong_datraloi = _kn.Count_KienNghi_BoNganh_DaXuLy(iKyHop, (int)c1.ICOQUAN);
        //                tong_coquan1_datraloi += tong_datraloi;
        //                int tong_dagiaiquyet = _kn.Count_KienNghi_BoNganh_KetQua(iKyHop, (int)c1.ICOQUAN, 4);
        //                tong_coquan1_dagiaiquyet += tong_dagiaiquyet;
        //                int tong_danggiaiquyet = _kn.Count_KienNghi_BoNganh_KetQua(iKyHop, (int)c1.ICOQUAN, 3);
        //                tong_coquan1_danggiaiquyet += tong_danggiaiquyet;
        //                int tong_giaitrinh = tong_datraloi - tong_dagiaiquyet - tong_danggiaiquyet;
        //                str += "<tr><td class='tcenter' text-align='center'>" + count1 + "</td><td>" + c1.CTEN +
        //                    "</td><td class='tcenter' text-align='center'>" + tong.ToString("#,##0").Replace(",", ".") + "</td>" +
        //                    "</td><td class='tcenter' text-align='center'>" + tong_datraloi.ToString("#,##0").Replace(",", ".") + "</td>" +
        //                    "</td><td class='tcenter' text-align='center'>" + tong_dagiaiquyet.ToString("#,##0").Replace(",", ".") + "</td>" +
        //                    "</td><td class='tcenter' text-align='center'>" + tong_danggiaiquyet.ToString("#,##0").Replace(",", ".") + "</td>" +
        //                    "</td><td class='tcenter' text-align='center'>" + tong_giaitrinh.ToString("#,##0").Replace(",", ".") + "</td>" +
        //                    "</tr>";
        //                count1++;
        //            }                    
        //        }
        //        str += "<tr><td text-align='center' class='tcenter' colspan='2'><strong>Cộng " + num_roman + " </strong></td>"+
        //            "</td><td class='tcenter' text-align='center'><strong>"+ tong_coquan1 .ToString("#,##0").Replace(",", ".") + "</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tong_coquan1_datraloi.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tong_coquan1_dagiaiquyet.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tong_coquan1_danggiaiquyet.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tong_coquan1_giaitrinh.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
        //            "</tr>";
        //        double tyle_coquan1_datraloi = Math.Round(Convert.ToDouble(tong_coquan1_datraloi) * 100 / Convert.ToDouble(tong_coquan1), 2);
        //        double tyle_coquan1_dagiaiquyet = Math.Round(Convert.ToDouble(tong_coquan1_dagiaiquyet) * 100 / Convert.ToDouble(tong_coquan1), 2);
        //        double tyle_coquan1_danggiaiquyet = Math.Round(Convert.ToDouble(tong_coquan1_danggiaiquyet) * 100 / Convert.ToDouble(tong_coquan1), 2);
        //        double tyle_coquan1_giaitrinh = Math.Round(Convert.ToDouble(tong_coquan1_giaitrinh) * 100 / Convert.ToDouble(tong_coquan1), 2);
        //        str += "<tr><td text-align='center' class='tcenter' colspan='2'><strong>Tỷ lệ "+ num_roman + "</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>100%</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tyle_coquan1_datraloi.ToString("#,##0").Replace(",", ".") + " %</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tyle_coquan1_dagiaiquyet.ToString("#,##0").Replace(",", ".") + " %</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tyle_coquan1_danggiaiquyet.ToString("#,##0").Replace(",", ".") + " %</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tyle_coquan1_giaitrinh.ToString("#,##0").Replace(",", ".") + " %</strong></td>" +
        //            "</tr>";
        //        count++;
        //        tong_coquan += tong_coquan1;
        //        tong_coquan_datraloi += tong_coquan1_datraloi;
        //        tong_coquan_dagiaiquyet += tong_coquan1_dagiaiquyet;
        //        tong_coquan_danggiaiquyet += tong_coquan1_danggiaiquyet;
        //        tong_coquan_giaitrinh += tong_coquan1_giaitrinh;
        //    }
        //    str += "<tr><td text-align='center' class='tcenter' colspan='2'><strong>"+ tong_baocao + " </strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tong_coquan.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tong_coquan_datraloi.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tong_coquan_dagiaiquyet.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tong_coquan_danggiaiquyet.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
        //            "</td><td class='tcenter' text-align='center'><strong>" + tong_coquan_giaitrinh.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
        //            "</tr>";
        //    double tyle_coquan_datraloi = Math.Round(Convert.ToDouble(tong_coquan_datraloi) * 100 / Convert.ToDouble(tong_coquan), 2);
        //    double tyle_coquan_dagiaiquyet = Math.Round(Convert.ToDouble(tong_coquan_dagiaiquyet) * 100 / Convert.ToDouble(tong_coquan), 2);
        //    double tyle_coquan_danggiaiquyet = Math.Round(Convert.ToDouble(tong_coquan_danggiaiquyet) * 100 / Convert.ToDouble(tong_coquan), 2);
        //    double tyle_coquan_giaitrinh = Math.Round(Convert.ToDouble(tong_coquan_giaitrinh) * 100 / Convert.ToDouble(tong_coquan), 2);
        //    str += "<tr><td text-align='center' class='tcenter' colspan='2'><strong>Tỷ lệ</strong></td>" +
        //        "</td><td class='tcenter' text-align='center'><strong>100%</strong></td>" +
        //        "</td><td class='tcenter' text-align='center'><strong>" + tyle_coquan_datraloi.ToString("#,##0").Replace(",", ".") + " %</strong></td>" +
        //        "</td><td class='tcenter' text-align='center'><strong>" + tyle_coquan_dagiaiquyet.ToString("#,##0").Replace(",", ".") + " %</strong></td>" +
        //        "</td><td class='tcenter' text-align='center'><strong>" + tyle_coquan_danggiaiquyet.ToString("#,##0").Replace(",", ".") + " %</strong></td>" +
        //        "</td><td class='tcenter' text-align='center'><strong>" + tyle_coquan_giaitrinh.ToString("#,##0").Replace(",", ".") + " %</strong></td>" +
        //        "</tr>";
        //    str = "<tr><td text-align='center' class='tcenter' colspan='7'><strong>Phụ lục 1</strong></td></tr>" +
        //        "<tr><td text-align='center' class='tcenter' colspan='7'><strong>Bảng Tập hợp kết quả giải quyết, trả lời "+ tong_coquan.ToString("#,##0").Replace(",", ".") + " kiến nghị của cử tri</strong></td></tr>" +
        //        "<tr><td text-align='center' class='tcenter' colspan='7'><strong>Tại kỳ họp "+Get_TenKyHop(iKyHop)+", "+Get_TenKhoaHop_By_IDKyHop(iKyHop)+" của các bộ, ngành</strong></td></tr>" +
        //        "<tr><td rowspan='2' text-align='center' class='tcenter'>STT<strong></strong></td>" +
        //        "<td rowspan='2' text-align='center' class='tcenter'><strong>Tên cơ quan, đơn vị</strong></td>" +
        //        "<td rowspan='2' text-align='center' class='tcenter'><strong>Tổng số kiến nghị</strong></td>" +
        //        "<td rowspan='2' text-align='center' class='tcenter'><strong>Tổng số KN đã trả lời</strong></td>" +
        //        "<td class='tcenter' text-align='center' colspan='3'><strong>Kết quả giải quyết</strong></td>" +
        //        "</tr><tr><dt class='tcenter'><strong>Đã giải quyết xong</strong></td>" +
        //        "<td text-align='center' class='tcenter'><strong>Đang giải quyết</strong></td>" +
        //        "<td text-align='center' class='tcenter'><strong>Giải trình, thông tin</strong></td></tr>"+
        //        str;

        //    return str;
        //}
        
        public string OptionThamQuyenGiaiQuyet(int id_parent)
        {
            string str = "<option value='0'>Chọn Thẩm quyền giải quyết</option>";
            if(id_parent == (int)ThamQuyen_DiaPhuong.Trunguong)
            {
                str += "<option selected value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "' " + ">" + "Trung Ương" + "</option>";
                str += "<option value='" + (int)ThamQuyen_DiaPhuong.Tinh + "' " + ">" + "Tỉnh" + "</option>";
                str += "<option value='" + (int)ThamQuyen_DiaPhuong.Huyen + "' " + ">" + "Huyện" + "</option>";
            }
            else if (id_parent == (int)ThamQuyen_DiaPhuong.Tinh)
            {
                str += "<option value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "' " + ">" + "Trung Ương" + "</option>";
                str += "<option value='" + (int)ThamQuyen_DiaPhuong.Tinh + "' " + ">" + "Tỉnh" + "</option>";
                str += "<option value='" + (int)ThamQuyen_DiaPhuong.Huyen + "' " + ">" + "Huyện" + "</option>";
            }
            else if (id_parent == (int)ThamQuyen_DiaPhuong.Huyen)
            {
                str += "<option value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "' " + ">" + "Trung Ương" + "</option>";
                str += "<option value='" + (int)ThamQuyen_DiaPhuong.Tinh + "' " + ">" + "Tỉnh" + "</option>";
                str += "<option selected value='" + (int)ThamQuyen_DiaPhuong.Huyen + "' " + ">" + "Huyện" + "</option>";
            }
            else
            {
                str += "<option value='" + (int)ThamQuyen_DiaPhuong.Trunguong + "' " + ">" + "Trung Ương" + "</option>";
                str += "<option value='" + (int)ThamQuyen_DiaPhuong.Tinh + "' " + ">" + "Tỉnh" + "</option>";
                str += "<option value='" + (int)ThamQuyen_DiaPhuong.Huyen + "' " + ">" + "Huyện" + "</option>";
            }

            return str;
        }
        public string OptionThamQuyenDonVi_Parent(List<QUOCHOI_COQUAN> coquan,int id_parent)
        {
            string str = "<option value='0'>Chọn cơ quan</option>";
            if (id_parent != 0) { str = "<option value='0'>Chọn tất cả</option>"; }
            foreach(var c in coquan)
            {
                string select = ""; if (c.ICOQUAN == id_parent) { select = "selected"; }
                str += "<option value='"+c.ICOQUAN+"' "+select+">"+ EncodeOutput(c.CTEN)+"</option>";
            }
            return str;
        }
        public string OptionPhanLoaiTraLoi_Parent(List<KN_TRALOI_PHANLOAI> phanloai, int id_parent)
        {
            string str = "";
            foreach (var c in phanloai)
            {
                string select = ""; if (c.IPHANLOAI == id_parent) { select = "selected"; }
                str += "<option value='" + c.IPHANLOAI + "' " + select + ">" + EncodeOutput(c.CTEN) + "</option>";
            }
            return str;
        }

        public string OptionPhanLoaiTraLoi(List<KN_TRALOI_PHANLOAI> phanloai, int id_parent)
        {
            string str = "";
            foreach (var c in phanloai)
            {
                string select = ""; if (c.IPHANLOAI == id_parent) { select = "selected"; }
                str += "<option value='" + c.ITINHTRANG + "' " + select + ">" + EncodeOutput(c.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_CoQuan(List<QUOCHOI_COQUAN> coquan, int thamquyendonvi)
        {
            string str = "";
            Dictionary<string, object> donvi = new Dictionary<string, object>();
            str += "<option selected value = '";
            if (thamquyendonvi == AppConfig.ID_BAN_DAN_NGUYEN_NEW)
            {

                donvi.Add("ICOQUAN", AppConfig.ID_BAN_DAN_NGUYEN_NEW);
                var coquanmacdinh = _kn.GetAll_CoQuanByParam(donvi).FirstOrDefault();
                str +=  coquanmacdinh.ICOQUAN + "'>" + coquanmacdinh.CTEN;
            }
            else
            {
                donvi.Add("ICOQUAN", AppConfig.ID_UY_BAN_NHAN_DAN);
                var coquanmacdinh = _kn.GetAll_CoQuanByParam(donvi).FirstOrDefault();
                str +=  coquanmacdinh.ICOQUAN + "'>" + coquanmacdinh.CTEN;
            }
                
            foreach (var t in coquan)
            {
                str += "<option value='" + t.ICOQUAN + "'>" + "---" + EncodeOutput(t.CTEN) + "</option>";
            }
            return str;
        }

        public string OptionKyHop_Parent(List<KN_TRALOI_PHANLOAI> phanloai, int id_parent)
        {
            string str = "";
            foreach (var c in phanloai)
            {
                string select = ""; if (c.IPHANLOAI == id_parent) { select = "selected"; }
                str += "<option value='" + c.IPHANLOAI + "' " + select + ">" + EncodeOutput(c.CTEN) + "</option>";
            }
            return str;
        }

        public string OptionThamQuyenDonVi_Parent_Child(List<QUOCHOI_COQUAN> coquan, int id_parent)
        {
            string str = "<option value='0'>Chọn đơn vị thẩm quyền xử lý</option>";
            if (coquan == null) { return str; }
            if (id_parent != 0) { str = "<option value='0'>Chọn tất cả</option>"; }
            
            foreach (var c in coquan)
            {
                string select = ""; if (c.ICOQUAN == id_parent) { select = "selected"; }
                str += "<option value='" + c.ICOQUAN + "' " + select + ">" + EncodeOutput(c.CTEN) + "</option>";
            }
            return str;
        }
        public string Convert_to_RomanNumber(int num)
        {
            switch (num)
            {
                case 1: return "I";
                case 2: return "II";
                case 3: return "III";
                case 4: return "IV";
                case 5: return "V";
                case 6: return "VI";
                case 7: return "VII";
                case 8: return "VIII";
                case 9: return "IX";
                case 10: return "X";
                default: return "";
            }
        }
        
        public string BAOCAO_TK_PHULUC_TRALOI(int iKyHop, int iDonVi, int iLinhVuc)
        {
            string str = "";
            //var list_donvi = _kn.GetAll_CoQuanByParam(null);
            
            var list = kn_report.getReportBaoCaoThongKeTraLoi("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_TRALOI", iKyHop, iDonVi, iLinhVuc);
            
            str += "<h4 class='tcenter' text-align='center'>BẢN TẬP HỢP TRẢ LỜI KIẾN NGHỊ CỬ TRI " +
                        HttpContext.Current.Server.HtmlDecode(Get_TenKyHop(iKyHop)).ToUpper() + ", " +
                        HttpContext.Current.Server.HtmlDecode(Get_TenKhoaHop_By_IDKyHop(iKyHop)).ToUpper() + "</h4>";
            if (list==null)
            {
                return str;
            }
            var list1 = list.Where(x => x.ID_GOP <= 0).ToList();
            List<decimal> list_id_donvi0 = new List<decimal>();
            int count_donvi0 = 1;
            foreach(var l0 in list)
            {
                if (!list_id_donvi0.Contains(l0.ID_THAMQUYENDONVI_PARENT))
                {
                    list_id_donvi0.Add(l0.ID_THAMQUYENDONVI_PARENT);
                    var list_donvi0 = list1.Where(x => x.ID_THAMQUYENDONVI_PARENT == l0.ID_THAMQUYENDONVI_PARENT).OrderBy(x=>x.ID_GOP).ToList();
                    str += "<p class='tcenter' text-align='center' style='text-decoration: underline;'><strong>Phần " +
                    Convert_to_RomanNumber(count_donvi0) + ". " + list_donvi0.FirstOrDefault().TEN_THAMQUYENDONVI_PARENT + "</strong></p>";
                    List<decimal> list_id_donvi = new List<decimal>();
                    int count_donvi = 1;
                    foreach (var l in list_donvi0)
                    {
                        if (!list_id_donvi.Contains(l.ID_THAMQUYENDONVI))
                        {
                            list_id_donvi.Add(l.ID_THAMQUYENDONVI);
                            var list_donvi = list_donvi0.Where(x => x.ID_THAMQUYENDONVI == l.ID_THAMQUYENDONVI).OrderBy(x => x.ID_GOP).ToList();
                            str += "<p class='tcenter' text-align='center'><strong>" + count_donvi + ". " +
                            list_donvi.FirstOrDefault().TEN_THAMQUYENDONVI + "</strong></p><p>&nbsp;</p>";
                            str += BAOCAO_TK_PHULUC_TRALOI_KienNghi_TheoDonVi(list_donvi, list);
                            count_donvi++;
                        }
                    }
                    count_donvi0 ++;
                }
            }

            //danh mục đơn vị cấp 0
            //List<decimal> list_id_donvi_nhom = new List<decimal>();
            //foreach (var l in list) { if (l.ID_THAMQUYENDONVI_PARENT != 0 && l.ID_THAMQUYENDONVI_PARENT != 20 && !list_id_donvi_nhom.Contains(l.ID_THAMQUYENDONVI_PARENT)) { list_id_donvi_nhom.Add(l.ID_THAMQUYENDONVI_PARENT); } }
            //if (list_id_donvi_nhom==null)
            //{
            //    return str;
            //}
            //var list_vanban_tonghop = list.Where(x => x.TONGHOP_SOVANBAN != null).ToList();
            //int count0 = 1;
            //foreach(var d0 in list_id_donvi_nhom)
            //{
            //    var list_donvi_xuly = list.Where(x => x.ID_THAMQUYENDONVI_PARENT == d0).ToList();
            //    if (list_donvi_xuly.Count() > 0)
            //    { 
            //        //danh mục đơn vị cấp 1
            //        List<decimal> list_id_donvi_cap1 = new List<decimal>();
            //        foreach (var l in list_donvi_xuly) { if (!list_id_donvi_cap1.Contains(l.ID_DONVI_THAMQUYEN)) { list_id_donvi_cap1.Add(l.ID_DONVI_THAMQUYEN); } }
            //        if (list_id_donvi_cap1.Count() > 0)
            //        {
            //            //var list_kiennghi_donvi0 = list_donvi_xuly.Where(x => x.ID_PARENT_DONVI == d0
            //            //    && x.NOIDUNG_KIENNGHI != null && x.ID_TONGHOP != 0 && x.ID_GIAMSAT_KIENNGHI != 0).ToList();
            //            //if (list_kiennghi_donvi0.Count() > 0)
            //            //{
            //                str += "<p class='tcenter' text-align='center' style='text-decoration: underline;'><strong>Phần " +
            //                Convert_to_RomanNumber(count0) + ". " + HttpContext.Current.Server.HtmlDecode(list_donvi_xuly.Where(x => x.ID_THAMQUYENDONVI_PARENT == d0).FirstOrDefault().TEN_THAMQUYENDONVI_PARENT) + "</strong></p>";
            //                count0++;
            //                int count1 = 1;
            //                foreach (var d1 in list_id_donvi_cap1)
            //                {
            //                    //kiến nghị đơn vị thẩm quyền xử lý
            //                    var list_kiennghi_donvi = list.Where(x => x.ID_DONVI_THAMQUYEN == d1
            //                    && x.NOIDUNG_KIENNGHI != null && x.ID_TONGHOP != 0 && x.ID_GIAMSAT_KIENNGHI != 0).ToList();
            //                    if (list_kiennghi_donvi.Count() > 0)
            //                    {
            //                        str += "<p class='tcenter' text-align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>" + count1 + ". " +
            //                        HttpContext.Current.Server.HtmlDecode(list_kiennghi_donvi.Where(x => x.ID_DONVI_THAMQUYEN == d1).FirstOrDefault().TEN_THAMQUYENDONVI) + "</strong></p><p>&nbsp;</p>";
            //                        str += BAOCAO_TK_PHULUC_TRALOI_KienNghi_TheoDonVi(list_kiennghi_donvi, list_vanban_tonghop);
            //                        count1++;
            //                    }
            //                }
            //            //}
            //            //tên đơn vị cấp 0                        
            //        }
            //    }
            //}            
            return str;
        }
        public string BAOCAO_TK_PHULUC_TRALOI_KienNghi_TheoDonVi(List<KIENNGHITRALOI> list, List<KIENNGHITRALOI> list_all)
        {
            string str = "";
            int count = 1;
            list = list.OrderBy(x => x.ID_GOP).ToList();
            List<decimal> list_id_kiennghi = new List<decimal>();
            foreach (var l in list) {
                if (!list_id_kiennghi.Contains(l.ID_KIENNGHI))
                {
                    list_id_kiennghi.Add(l.ID_KIENNGHI);
                    string donvitiepnhan = "";
                    if (l.ID_GOP == -1)
                    {
                        var kn_gop = list_all.Where(x => x.ID_KIENNGHI_CHILD_GOP == l.ID_KIENNGHI).ToList();
                        List<string> list_donvi_gop = new List<string>(); int count_donvi_gop = 0;

                        foreach (var g in kn_gop)
                        {
                            if (!list_donvi_gop.Contains(g.TENDONVITIEPNHAN_GOP))
                            {
                                if (count_donvi_gop > 0) { donvitiepnhan += ", "; }
                                donvitiepnhan += g.TENDONVITIEPNHAN_GOP;
                                list_donvi_gop.Add(g.TENDONVITIEPNHAN_GOP);
                                count_donvi_gop++;
                            }
                        }

                    }
                    if (donvitiepnhan == "") { donvitiepnhan = l.TEN_DONVITIEPNHAN; }
                    string diaphuong_kiennghi = count + ". Cử tri " + donvitiepnhan + " kiến nghị";
                    string congvan = "";
                    if (l.TRALOI_SOVANBAN != null)
                    {
                        string ngaybanhanh = "";
                        if (l.TRALOI_NGAYBANHANH_VANBAN != DateTime.MinValue)
                        {
                            ngaybanhanh = ", ngày " + func.ConvertDateVN(l.TRALOI_NGAYBANHANH_VANBAN.ToString());
                        }
                        congvan = "(Tại Công văn số " + l.TRALOI_SOVANBAN + ngaybanhanh + ")";
                    }
                    str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong><em>" + diaphuong_kiennghi + ": </em></strong><em>" + l.NOIDUNG_KIENNGHI + "</em></p>"; ;
                    str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Trả lời :</strong> " + congvan + "</p>";
                    str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + l.TRALOI_NOIDUNG + "</p>";
                    count++;
                }
                
            }
            
            //foreach(var k in list_id_kiennghi)
            //{
            //    //string noidung = "";
            //    string diaphuong_kiennghi = count+ ". Cử tri ";
            //    if (k != 0)
            //    {
            //        var kiennghi_cungnoidung = list.Where(x => x.ID_KIENNGHI == k).ToList();
            //        KIENNGHITRALOI kn = kiennghi_cungnoidung.Where(x => x.ID_KIENNGHI == k).FirstOrDefault();
            //        var tonghop_traloi = list_tonghop.Where(x => x.ID_TONGHOP == kn.ID_KIENNGHI_TONGHOP_BDN);
            //        if (tonghop_traloi.Count() > 0 && kn.ID_GIAMSAT_KIENNGHI==kn.ID_KIENNGHI)
            //        {
            //            KIENNGHITRALOI tonghop = tonghop_traloi.FirstOrDefault();
            //            str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong><em>" + count + ". " + TenCuTri_DiaPhuong(kiennghi_cungnoidung) + "</strong></em></p>";
            //            str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<em>" + HttpContext.Current.Server.HtmlDecode(kn.NOIDUNG_KIENNGHI_CUNGNOIDUNG) + "</em></p>";
            //            str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Trả lời :</strong> (Tại Công văn số " + HttpContext.Current.Server.HtmlDecode(tonghop.TONGHOP_SOVANBAN) + " ngày "+
            //                       func.ConvertDateVN(tonghop.TONGHOP_NGAYBANHANHVANBAN.ToString())+") </p>";
            //            str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + HttpContext.Current.Server.HtmlDecode(kn.TRALOI) + "</p>";
            //            count++;
            //        }
                    
            //    }
            //    else
            //    {
            //        var kiennghi_le = list.Where(x => x.ID_KIENNGHI_PARENT == k && x.NOIDUNG_KIENNGHI!=null).ToList();
            //        List<decimal> list_id_kiennghi_le = new List<decimal>();
            //        foreach (var kn in kiennghi_le)
            //        {
            //            if (list.Where(x => x.ID_KIENNGHI_PARENT == kn.ID_KIENNGHI).Count() == 0)
            //            {//chỉ lấy những kiến nghị ko có chung
            //                var tonghop_traloi = list_tonghop.Where(x => x.ID_TONGHOP == kn.ID_KIENNGHI_TONGHOP_BDN);
            //                //if (tonghop_traloi.Count() > 0 && kn.ID_GIAMSAT_KIENNGHI == kn.ID_KIENNGHI)
            //                if (tonghop_traloi.Count() > 0 && kn.ID_GIAMSAT_KIENNGHI == kn.ID_KIENNGHI && !list_id_kiennghi_le.Contains(kn.ID_KIENNGHI))
            //                {
            //                    list_id_kiennghi_le.Add(kn.ID_KIENNGHI);
            //                }
            //            }
            //        }
                            
            //        foreach (var id in list_id_kiennghi_le)
            //        {
            //            KIENNGHITRALOI kn = kiennghi_le.Where(x => x.ID_KIENNGHI == id).FirstOrDefault();
            //            KIENNGHITRALOI tonghop = list_tonghop.Where(x => x.ID_TONGHOP == kn.ID_KIENNGHI_TONGHOP_BDN).FirstOrDefault();
            //            str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong><em>" + count + ". " + " Cử tri " + HttpContext.Current.Server.HtmlDecode(kn.LOAI_DIAPHUONG) + " " +
            //                    HttpContext.Current.Server.HtmlDecode(kn.TEN_DIAPHUONG) + " kiến nghị:" + "</strong></em></p>"; ;
            //            str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<em>" + HttpContext.Current.Server.HtmlDecode(kn.NOIDUNG_KIENNGHI) + "</em></p>";
            //            str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Trả lời :</strong> (Tại Công văn số " + HttpContext.Current.Server.HtmlDecode(tonghop.TONGHOP_SOVANBAN) + " ngày " +
            //                func.ConvertDateVN(tonghop.TONGHOP_NGAYBANHANHVANBAN.ToString()) + ") </p>";
            //            str += "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + HttpContext.Current.Server.HtmlDecode(kn.TRALOI) + "</p>";
            //            count++;
            //        }
                        
            //    }
                
            //}
            return str;
        }
        /*
        public string TenCuTri_DiaPhuong(List<KIENNGHITRALOI> kiennghi)
        {
            string str = "";
            if (kiennghi.Count() > 0)
            {
                if (kiennghi.Count() == 1)
                {
                    var kn = kiennghi.FirstOrDefault();
                    str += "Cử tri "+HttpContext.Current.Server.HtmlDecode(kn.LOAI_DIAPHUONG)+" "+ 
                            HttpContext.Current.Server.HtmlDecode(kn.TEN_DIAPHUONG) + " kiến nghị:";
                }else
                {
                    str += "Cử tri ";
                    kiennghi = kiennghi.Where(x=>x.TEN_DIAPHUONG!=null).OrderBy(x => x.ID_KIENNGHI_PARENT).ToList();
                    List<string> list_tendiaphuong = new List<string>();
                    foreach(var t in kiennghi)
                    {
                        if (!list_tendiaphuong.Contains(t.TEN_DIAPHUONG)) { list_tendiaphuong.Add(t.TEN_DIAPHUONG); }
                    }
                    int count = 1;
                    foreach (var kn in list_tendiaphuong)
                    {
                        //if (count == 1)
                        //{
                        //    str += HttpContext.Current.Server.HtmlDecode(kn);
                        //}
                        str += " " + HttpContext.Current.Server.HtmlDecode(kn);
                        if(count < list_tendiaphuong.Count())
                        {
                            str += ",";
                        }
                        count++;
                    }
                    str += " kiến nghị:";
                }
            }
            return str;
        }
        */
        //public string BAOCAO_TK_PHULUC_TRALOI(int iKyHop,int iDonVi,int iLinhVuc)
        //{
        //    string str = "";
        //    var list_donvi = _kn.GetAll_CoQuanByParam(null);
        //    var list = kn_report.getReportBaoCaoThongKeTraLoi("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_TRALOI", iKyHop, iDonVi,iLinhVuc);
        //    str += "<h4 class='tcenter' text-align='center'>BẢN TẬP HỢP TRẢ LỜI KIẾN NGHỊ CỬ TRI "+
        //                HttpContext.Current.Server.HtmlDecode(Get_TenKyHop(iKyHop).ToUpper()) +", "+ 
        //                HttpContext.Current.Server.HtmlDecode(Get_TenKhoaHop_By_IDKyHop(iKyHop).ToUpper())+ "</h4>";
        //    List<decimal> list_id_donvi_nhom = new List<decimal>();
        //    foreach (var l in list) { if (!list_id_donvi_nhom.Contains(l.ID_PARENT_DONVI)  && l.ID_PARENT_DONVI!=20) { list_id_donvi_nhom.Add(l.ID_PARENT_DONVI); } }
        //    int count0 = 1;
        //    foreach (var l in list_id_donvi_nhom)// cấp cơ quan
        //    {
        //        var list_parent = list.Where(x => x.ID_PARENT_DONVI == l && x.NOIDUNG_TONGHOP != null && x.TONGHOP_SOVANBAN != null
        //                            && x.TONGHOP_NGAYBANHANHVANBAN != null).ToList();
        //        if (list_parent.Count() > 0)
        //        {
        //            str += "<p class='tcenter' text-align='center' style='text-decoration: underline;'><strong>" +
        //            HttpContext.Current.Server.HtmlDecode(list_donvi.Where(x => x.ICOQUAN == (int)l).FirstOrDefault().CTEN) + "</strong></p>";
        //            List<decimal> list_id_donvi_xuly = new List<decimal>();
        //            foreach (var t in list)
        //            {
        //                if (!list_id_donvi_xuly.Contains(t.ID_DONVI_THAMQUYEN) && t.ID_PARENT_DONVI == l)
        //                {
        //                    list_id_donvi_xuly.Add(t.ID_DONVI_THAMQUYEN);
        //                }
        //            }

        //            foreach (var l1 in list_id_donvi_xuly)// cơ quan xử lý
        //            {
        //                var tonghop_by_donvi = list_parent.Where(x => x.ID_DONVI_THAMQUYEN == l1).ToList();
        //                if (tonghop_by_donvi.Count() > 0)
        //                {
        //                    str += "<p class='tcenter' text-align='center'><strong>" +
        //                        HttpContext.Current.Server.HtmlDecode(list_donvi.Where(x => x.ICOQUAN == (int)l1).FirstOrDefault().CTEN) + "</strong></p>";
        //                    //lấy Tập hợp theo đơn vị

        //                    //group id Tập hợp
        //                    List<decimal> list_id_tonghop = new List<decimal>();
        //                    foreach (var t in tonghop_by_donvi)
        //                    {
        //                        if (!list_id_tonghop.Contains(t.ID_TONGHOP)
        //                            && t.NOIDUNG_TONGHOP != null && t.TONGHOP_SOVANBAN != null
        //                            && t.TONGHOP_NGAYBANHANHVANBAN != null
        //                            )
        //                        {
        //                            list_id_tonghop.Add(t.ID_TONGHOP);
        //                        }
        //                    }
        //                    //lấy tên các Tập hợp
        //                    int count_tonghop = 1;
        //                    foreach (var t in list_id_tonghop)
        //                    {
        //                        KIENNGHITRALOI tonghop = tonghop_by_donvi.Where(x => x.ID_TONGHOP == t
        //                        && x.NOIDUNG_TONGHOP != null && x.TONGHOP_SOVANBAN != null
        //                            && x.TONGHOP_NGAYBANHANHVANBAN != null).FirstOrDefault();
        //                        //list id -dia phuowngtieeps nhận
        //                        var kn_by_tonghop = list.Where(x => x.ID_TONGHOP == t).ToList();
        //                        List<decimal> list_id_donvi_tiepnhan = new List<decimal>();
        //                        foreach (var v in kn_by_tonghop) { if (!list_id_donvi_tiepnhan.Contains(v.ID_DONVI_TIEPNHAN)) { list_id_donvi_tiepnhan.Add(v.ID_DONVI_TIEPNHAN); } }
        //                        str += "<p><em><strong>" + count_tonghop + ". Cử tri tỉnh " +
        //                            Get_List_Name_DonViTiepNhan(list_id_donvi_tiepnhan, list_donvi) +
        //                            " kiến nghị:</strong></em> " + HttpContext.Current.Server.HtmlDecode(tonghop.NOIDUNG_TONGHOP) + "</p>";
        //                        str += "<p><strong>Trả lời:</strong> (Tại " + HttpContext.Current.Server.HtmlDecode(tonghop.TONGHOP_SOVANBAN)
        //                                + " ngày " + func.ConvertDateVN(tonghop.TONGHOP_NGAYBANHANHVANBAN.ToString()) + ")</p>";
        //                        int count_kienngi = 1;

        //                        List<decimal> list_id_kiennghi = new List<decimal>();
        //                        foreach (var kn in kn_by_tonghop)
        //                        {
        //                            if (!list_id_kiennghi.Contains(kn.ID_KIENNGHI)) { list_id_kiennghi.Add(kn.ID_KIENNGHI); }
        //                        }
        //                        foreach (var kn in list_id_kiennghi)
        //                        {
        //                            KIENNGHITRALOI kiennghi = kn_by_tonghop.Where(x => x.ID_KIENNGHI == kn).FirstOrDefault();
        //                            str += "<p><em>" + count_tonghop + "." + count_kienngi + ". <strong>Kiến nghị:</strong> " + HttpContext.Current.Server.HtmlDecode(kiennghi.NOIDUNG_KIENNGHI) + "</em></p>";
        //                            str += "<p style='padding-left:10px;'><strong>Trả lời:</strong> " + HttpContext.Current.Server.HtmlDecode(kiennghi.TRALOI) + "</p>";
        //                            count_kienngi++;
        //                        }
        //                        count_tonghop++;
        //                    }
        //                }
        //            }


        //            //var list_kn_donvi_xuly = list.Where(x => x.ID_DONVI_THAMQUYEN == l1).ToList();
        //            //List<decimal> list_id_donvi_tiepnhan = new List<decimal>();
        //            //foreach (var t in list_kn_donvi_xuly) { if (!list_id_donvi_tiepnhan.Contains(t.ID_DONVI_TIEPNHAN)) { list_id_donvi_tiepnhan.Add(t.ID_DONVI_TIEPNHAN); } }

        //        }
        //    }
        //    return str;
        //}
        /*

        public string BAOCAO_TK_PHULUC_PHANLOAI_By_IDPhanLoai(List<KIENNGHIPHANLOAI> list)
        {
            string str = "";
            str += "<p>&nbsp;</p><table cellpadding='0' cellspacing='0' class='baocao'><tr><td class='tcenter' text-align='center' width='10%'><strong>STT</strong></td>" +
                "<td class='tcenter' text-align='center'><strong>Nội dung kiến nghị</strong></td>" +
                "<td class='tcenter' text-align='center' width='15%'><strong>Địa phương</strong></td></tr>";
            int count0 = 1;
            List<decimal> list_id_cungnoidung = new List<decimal>();
            foreach(var l in list) {
                if (l.NOIDUNG_KIENNGHI!=null && !list_id_cungnoidung.Contains(l.ID_KIENNGHI_PARENT))
                {
                    list_id_cungnoidung.Add(l.ID_KIENNGHI_PARENT);
                } 
            }
            foreach(var l in list_id_cungnoidung)
            {
                if (l > 0)
                {
                    KIENNGHIPHANLOAI k = list.Where(x => x.ID_KIENNGHI == l).FirstOrDefault();
                    string noidung = HttpContext.Current.Server.HtmlDecode(k.NOIDUNG_KIENNGHI_CUNGNOIDUNG);
                    string diaphuong = "";
                    if (k.TEN_DIAPHUONG != null)
                    {
                        diaphuong = HttpContext.Current.Server.HtmlDecode(k.TEN_DIAPHUONG);
                    }
                    var kiennghi_cungnoidung = list.Where(x => x.ID_KIENNGHI_PARENT == l && x.TEN_DIAPHUONG!=null && x.NOIDUNG_KIENNGHI !=null).ToList();
                    foreach(var n in kiennghi_cungnoidung)
                    {
                        if (n.TEN_DIAPHUONG != k.TEN_DIAPHUONG)
                        {
                            diaphuong += ", " + HttpContext.Current.Server.HtmlDecode(n.TEN_DIAPHUONG);
                        }
                    }
                    str += "<tr><td class='tcenter' text-align='center'>" + count0 + "</td><td>" +
                            noidung + "</td><td class='tcenter' text-align='center'><em>" + diaphuong + "</em></td></tr>";
                    count0++;
                }else
                {
                    var kiennghi_le = list.Where(x => x.ID_KIENNGHI_PARENT == l && x.NOIDUNG_KIENNGHI != null).ToList();
                    List<decimal> list_id_phanloai = new List<decimal>();
                    foreach (var n in kiennghi_le)
                    {
                        if (list.Where(x => x.ID_KIENNGHI_PARENT == n.ID_KIENNGHI).Count() == 0
                            && !list_id_phanloai.Contains(n.ID_KIENNGHI))
                        {
                            list_id_phanloai.Add(n.ID_KIENNGHI);
                        }
                    }
                    foreach(var id in list_id_phanloai)
                    {
                        KIENNGHIPHANLOAI n = kiennghi_le.Where(x => x.ID_KIENNGHI == id).FirstOrDefault();
                        string noidung = HttpContext.Current.Server.HtmlDecode(n.NOIDUNG_KIENNGHI);
                        string diaphuong = "";
                        if (n.TEN_DIAPHUONG != null)
                        {
                            diaphuong = HttpContext.Current.Server.HtmlDecode(n.TEN_DIAPHUONG);
                        }
                        str += "<tr><td class='tcenter' text-align='center'>" + count0 + "</td><td>" +
                        noidung + "</td><td class='tcenter' text-align='center'><em>" + diaphuong + "</em></td></tr>";
                        count0++;
                    }
                }
            }
            //var list_kiennghi_parent = list.Where(x => x.ID_KIENNGHI_PARENT == 0 && x.NOIDUNG_KIENNGHI!=null).ToList();
            //foreach (var k in list_kiennghi_parent)
            //{
            //    string diaphuong = HttpContext.Current.Server.HtmlDecode(k.TEN_DIAPHUONG);
            //    string noidung = HttpContext.Current.Server.HtmlDecode(k.NOIDUNG_KIENNGHI);
            //    if (list.Where(x => x.ID_KIENNGHI_PARENT == k.ID_KIENNGHI).Count() > 0)
            //    {
            //        noidung = HttpContext.Current.Server.HtmlDecode(k.NOIDUNG_KIENNGHI_CUNGNOIDUNG);
            //        var list_kiennghi_child = list.Where(x => x.ID_KIENNGHI_PARENT == k.ID_KIENNGHI).ToList();
            //        foreach(var d in list_kiennghi_child)
            //        {
            //            if (d.TEN_DIAPHUONG != k.TEN_DIAPHUONG)
            //            {
            //                diaphuong += ", " + HttpContext.Current.Server.HtmlDecode(d.TEN_DIAPHUONG);
            //            }                        
            //        }
            //    }
            //    str += "<tr><td class='tcenter' text-align='center'>" + count0 + "</td><td>" +
            //                noidung +"</td><td class='tcenter' text-align='center'><em>" + diaphuong + "</em></td></tr>";
            //    count0++;
            //}
            str += "</table><p>&nbsp;</p>";
            return str;
        }
        */
        public string BAOCAO_TK_PHULUC_PHANLOAI_Phanloai_Table(List<KIENNGHIPHANLOAI> list, List<KIENNGHIPHANLOAI> list_all)
        {
            string str= "";
            str += "<table cellpadding='0' cellspacing='0' class='baocao'><tr><td class='tcenter' text-align='center' width='10%'><strong>STT</strong></td>" +
                "<td class='tcenter' text-align='center'><strong>Nội dung kiến nghị</strong></td>" +
                "<td class='tcenter' text-align='center' width='15%'><strong>Địa phương</strong></td></tr>";
            int count0 = 1;
            List<decimal> list_id_kiennghi = new List<decimal>();
            list = list.OrderBy(x => x.ID_GOP).ToList();
            foreach (var l in list)
            {
                if (!list_id_kiennghi.Contains(l.ID_KIENNGHI))
                {
                    list_id_kiennghi.Add(l.ID_KIENNGHI);
                    string donvitiepnhan = "";
                    int count_donvi_gop = 0;
                    var kn_gop = list_all.Where(x => x.ID_KIENNGHI_PARENT_GOP == l.ID_KIENNGHI).ToList();
                    if (kn_gop.Count() > 0)
                    {
                        List<string> list_donvi_gop = new List<string>();
                        foreach (var g in kn_gop)
                        {
                            if (!list_donvi_gop.Contains(g.TENDONVITIEPNHAN_GOP))
                            {
                                if (count_donvi_gop > 0) { donvitiepnhan += ", "; }
                                donvitiepnhan += g.TENDONVITIEPNHAN_GOP;
                                list_donvi_gop.Add(g.TENDONVITIEPNHAN_GOP);
                                count_donvi_gop++;
                            }
                        }
                    }
                    if (donvitiepnhan == "")
                    {
                        donvitiepnhan = l.TEN_DONVITIEPNHAN;
                    }
                    str += "<tr><td class='tcenter' text-align='center'>" + count0 + "</td><td>" +
                            l.NOIDUNG_KIENNGHI + "</td><td class='tcenter' text-align='center'><em>" + donvitiepnhan + "</em></td></tr>";
                    count0++;
                }
            }
            str += "</table><p>&nbsp;</p>";
            return str;
        }

        public string BAOCAO_TK_TRALOI_KNTC(int iKyHop, int iLoai, int iHinhThuc)
        {
            string str = "";
            var list = kn_report.getReportBaoCaoThongKeTraLoiKNTC("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_TRALOIKNTC", iLoai, iKyHop, iHinhThuc);
            list = list.OrderBy(x => x.IKYHOP).ToList();
            string loai = "";
            if (iLoai == 0) { loai = " QUỐC HỘI "; } else { loai = " HỘI ĐỒNG NHÂN DÂN "; }
            string tenkyhop = "<br>BẢNG THEO DÕI TRẢ LỜI Ý KIẾN, KIẾN NGHỊ CỬ TRI CỦA BỘ, NGÀNH ";
            if (iHinhThuc == 1) { tenkyhop += "TRƯỚC "; } else { tenkyhop += "SAU "; }
            string tenhoa = Get_TenKhoaHop_By_IDKyHop(iKyHop).ToUpper().Replace("KHóA", "KHÓA");
            tenkyhop += Get_TenKyHop(iKyHop).ToUpper() + loai + tenhoa + "<br/>";
            int count = 1;
            str += "<tr><td colspan='12' class='tcenter b'>" + tenkyhop + "</td></tr>";
            str += "<tr><td colspan='2' class='tcenter b' width='5%'>STT</td><td colspan='2' class='tcenter b' width='15%'>Cử tri</td>" +
                 "<td colspan='2' class='tcenter b'>Câu hỏi</td><td colspan='2' class='tcenter b' width='15%'>Bộ/ngành trả lời</td>" +
                 "<td colspan='2' class='tcenter b' width='15%'>Văn bản trả lời</td>" +
                 "<td colspan='2' class='tcenter b'>Nội dung trả lời</td></tr>";
            foreach (var kn in list)
            {
                if (count == 1)
                {
                    str += "<tr>";
                }
                str += "<td colspan='2' class='tcenter b' width='5%' text-align='center'>" + count + "</td>" +
                        "<td colspan='2' class='tcenter b' width='10%'>" + kn.CUTRI + "</td>" +
                        "<td colspan='2' class='tcenter b' width='40%'>" + kn.CNOIDUNG + "</td>" +
                        "<td colspan='2' class='tcenter b' width='10%'>" + kn.COQUANTHAMQUYEN + "</td>" +
                        "<td colspan='2' class='tcenter b' width='10%'> Văn bản số "+ kn.CSOVANBAN + " ngày "+ func.ConvertDateVN(kn.DNGAYBANHANH.ToString()) +"</td>" +
                        "<td colspan='2' class='tcenter b' width='40%'>" + kn.CTRALOI + "</td>";
                if (count < list.Count())
                {//chưa cuối
                    str += "</tr><tr>";
                }
                else
                {
                    
                    str += "</tr>";
                }
                count++;
            }
            return str;
        }

        public string BAOCAO_TK_TraloiKN_DenDBQH(string iKyHop, int iLoai, int iKhoa)
        {
            string str = "";
            var list = kn_report.getReportBaoCaoThongKeTraLoiKN_DENDBQH("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_TRALOIKN_DENDBQH", iLoai, iKyHop, iKhoa);
            var listIDKhoa = list.OrderBy(x => x.IKYHOP).ToList();
            List<int> list_id_donvi = new List<int>();
            string loai = "";
            string tenKhoa = "";
            QUOCHOI_KHOA khoa = _kn.Get_Khoa_QuocHoi((int)iKhoa);
            if (khoa != null)
            {
                tenKhoa = EncodeOutput(khoa.CTEN);
            }
            string tenhoa = tenKhoa.ToUpper().Replace("KHóA", "KHÓA") + "<br/>";

            if (iLoai == 0) { loai = " QUỐC HỘI "; } else { loai = " HỘI ĐỒNG NHÂN DÂN "; }
            string tenkyhop = "<br>TỔNG HỢP KIẾN NGHỊ CỬ TRI GỬI ĐẾN ĐOÀN ĐẠI BIỂU"+ loai + "TỈNH ";
            tenkyhop += tenhoa ;
            int count = 1;
            str += "<tr><td colspan='6' class='tcenter b'> VĂN PHÒNG ĐOÀN ĐBQH VÀ<br>HĐND TỈNH THANH HÓA </td>" +
                "<td colspan='6' class='tcenter b'> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM </td></tr>" +
                "<tr><td colspan='6' class='tcenter b'> Phòng Tổng hợp, Thông tin, Dân nguyện </td>" +
                "<td colspan='6' class='tcenter b'> <u>Độc lập – Tự do – Hạnh phúc </u></td></tr>" +
                "<tr><td colspan='12' class='tcenter b'>" + tenkyhop + "</td></tr>";
            str += "<tr><td colspan='2' class='tcenter b' width='5%'>STT</td><td colspan='2' class='tcenter b'>NỘI DUNG KIẾN NGHỊ</td>" +
                 "<td colspan='2' class='tcenter b'>VĂN BẢN TRẢ LỜI</td><td colspan='2' class='tcenter b'>GHI CHÚ</td></tr>";

            decimal GroupIDKhoa = 0;
            foreach (var kn_IDKhoa in listIDKhoa)
            {
                if (GroupIDKhoa != kn_IDKhoa.IKYHOP)
                {
                    str += "<tr><td class='tcenter' text-align='center' colspan='7'><strong>" + kn_IDKhoa.CTEN + "</strong></td></tr>";
                    var listIDKyHop = list.Where(x => x.IKYHOP == kn_IDKhoa.IKYHOP).ToList();
                    foreach (var kn in listIDKyHop)
                    {
                        if (count == 1)
                        {
                            str += "<tr>";
                        }
                        str += "<td colspan='2' class='tcenter b' width='5%' text-align='center'>" + count + "</td>" +
                                "<td colspan='2' class='tcenter b' width='35%'>" + kn.CNOIDUNG + "</td>" +
                                "<td colspan='2' class='tcenter b' width='35%'>Văn bản số " + kn.CSOVANBAN + " ngày " + func.ConvertDateVN(kn.DNGAYBANHANH.ToString()) + " của " + kn.CCOQUANTRALOI + "</td>" +
                                "<td colspan='2' class='tcenter b' width='25%'>" + kn.GHICHU_KQ + "</td>";

                        if (count < list_id_donvi.Count())
                        {//chưa cuối
                            str += "</tr><tr>";
                        }
                        else
                        {

                            str += "</tr>";
                        }
                        count++;
                    }
                }
                GroupIDKhoa = kn_IDKhoa.IKYHOP;

            }

            
            return str;
        }

        public string BAOCAO_TK_PHULUC_PHANLOAI(int iKyHop,int iDonVi,int iLinhVuc)
        {
            string str = "";
            string tendonvi = "";
            QUOCHOI_COQUAN donvi = _kn.HienThiThongTinCoQuan(iDonVi);
            if (donvi != null) { tendonvi = donvi.CTEN; }
            var list = kn_report.getReportBaoCaoThongKePhanLoai("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHANLOAI", iKyHop, iDonVi,iLinhVuc);
            var list1 = list.Where(x => x.ID_GOP <= 0).ToList();
            List<decimal> list_id_parent_phanloai = new List<decimal>();
            int count_phanloai0 = 1;
            string tieude_baocao = "KIẾN NGHỊ CỦA CỬ TRI TẠI " + Get_TenKyHop(iKyHop) +
                ", " + Get_TenKhoaHop_By_IDKyHop(iKyHop) + " THUỘC LĨNH VỰC " + tendonvi;
            str += "<h4 class='tcenter' text-align='center'>" + tieude_baocao.ToUpper() + "</h4>";
            foreach (var lp in list1)
            {
                if (!list_id_parent_phanloai.Contains(lp.ID_PARENT_TRALOI_PHANLOAI))
                {
                    list_id_parent_phanloai.Add(lp.ID_PARENT_TRALOI_PHANLOAI);
                    string tenphanloai_parent = lp.TEN_TRALOI_PHANLOAI;
                    if (lp.ID_PARENT_TRALOI_PHANLOAI != 0) { tenphanloai_parent = lp.TEN_PHANLOAI_PARENT; }
                    str += "<p class='tcenter' text-align='center'><strong>BẢNG " + count_phanloai0 + ": " + tenphanloai_parent + " </strong></p>";
                    var list_phanloai0 = list1.Where(x => x.ID_PARENT_TRALOI_PHANLOAI == lp.ID_PARENT_TRALOI_PHANLOAI).ToList();
                    if (lp.ID_PARENT_TRALOI_PHANLOAI == 0)
                    {//gốc có kiến nghị
                        str += BAOCAO_TK_PHULUC_PHANLOAI_Phanloai_Table(list_phanloai0, list);
                    }
                    else
                    {
                        List<decimal> list_id_phanloai = new List<decimal>();
                        int countphanloai1 = 1;
                        foreach (var l1 in list_phanloai0)
                        {
                            if (!list_id_phanloai.Contains(l1.ID_TRALOI_PHANLOAI))
                            {
                                list_id_phanloai.Add(l1.ID_TRALOI_PHANLOAI);
                                var list_phanloai1 = list_phanloai0.Where(x => x.ID_TRALOI_PHANLOAI == l1.ID_TRALOI_PHANLOAI).ToList();
                                str += "<p class='tcenter' text-align='center'><strong>" + countphanloai1 + ". " + list_phanloai1.FirstOrDefault().TEN_TRALOI_PHANLOAI + " </strong></p>";
                                str += BAOCAO_TK_PHULUC_PHANLOAI_Phanloai_Table(list_phanloai1, list);
                                countphanloai1++;
                            }
                        }
                    }
                    count_phanloai0++;
                }
            }
            
            //List<decimal> list_id_phanloai_parent = new List<decimal>();
            //foreach(var l in list) {
            //    if (l.ID_PARENT_PHANLOAI == 0 && !list_id_phanloai_parent.Contains(l.ID_PHANLOAI)) {
            //        list_id_phanloai_parent.Add(l.ID_PHANLOAI);
            //    }
            //}
            //string tieude_baocao = "KIẾN NGHỊ CỦA CỬ TRI TẠI " + HttpContext.Current.Server.HtmlDecode(Get_TenKyHop(iKyHop)) + 
            //    ", " + HttpContext.Current.Server.HtmlDecode(Get_TenKhoaHop_By_IDKyHop(iKyHop)) + " THUỘC LĨNH VỰC "+HttpContext.Current.Server.HtmlDecode(tendonvi.CTEN);
            //str += "<h4 class='tcenter' text-align='center'>"+ tieude_baocao.ToUpper() + "</h4>";
            //int count = 1;
            //foreach(var i in list_id_phanloai_parent)
            //{
            //    str += "<p class='tcenter' text-align='center'><strong>BẢNG "+ count + ": "+list.Where(x=>x.ID_PHANLOAI==i).FirstOrDefault().TEN_PHANLOAI +" </strong></p>";
            //    List<decimal> list_id_phanloai_child = new List<decimal>();
            //    foreach (var l in list) {
            //        if (l.ID_PARENT_PHANLOAI == i && !list_id_phanloai_child.Contains(l.ID_PHANLOAI))
            //        {
            //            list_id_phanloai_child.Add(l.ID_PHANLOAI);
            //        }
            //    }
            //    if (list_id_phanloai_child.Count() > 0)
            //    {
            //        int count1 = 1;
            //        foreach (var i1 in list_id_phanloai_child)
            //        {
            //            str += "<p class='tcenter' text-align='center'><strong>" + count1 + ". " + list.Where(x => x.ID_PHANLOAI == i1).FirstOrDefault().TEN_PHANLOAI + " </strong></p>";
            //            var list_child = list.Where(x => x.ID_PHANLOAI == i1).ToList();
            //            if (list_child.Count() > 0)
            //            {
            //                str += BAOCAO_TK_PHULUC_PHANLOAI_By_IDPhanLoai(list_child);
            //            }
            //            count1++;
            //        }
            //    }else
            //    {
            //        var list_child = list.Where(x => x.ID_PHANLOAI == i).ToList();
            //        if (list_child.Count() > 0)
            //        {
            //            str += BAOCAO_TK_PHULUC_PHANLOAI_By_IDPhanLoai(list_child);
            //        }
            //    }
            //    count++;
            //}
            //var list_giaiquyet = list.Where(x => x.PHANLOAI == 4).ToList();
            //var list_giaitrinhthongtin = list.Where(x => x.PHANLOAI != 4).ToList();
            //int count0 = 1;
            //str += "<table cellpadding='0' cellspacing='0' class='baocao'><tr><td class='tcenter' text-align='center' width='10%'><strong>STT</strong></td>" +
            //    "<td class='tcenter' text-align='center'><strong>Nội dung kiến nghị</strong></td>" +
            //    "<td class='tcenter' text-align='center' width='15%'><strong>Địa phương</strong></td></tr>";
            //foreach (var k in list_giaitrinhthongtin)
            //{
            //    str += "<tr><td class='tcenter' text-align='center'>" + count0 + "</td><td>" + 
            //        HttpContext.Current.Server.HtmlDecode(k.NOIDUNG_KIENNGHI) +
            //            "</td><td class='tcenter' text-align='center'>" + 
            //            HttpContext.Current.Server.HtmlDecode(k.TEN_DIAPHUONG) + "</td></tr>";
            //    count0++;
            //}
            //str += "</table>";
            //str += "<p>";
            //str += "<p class='tcenter' text-align='center'><strong>BẢNG 2: Kiến nghị có thể giải quyết được ngay </strong></p>";
            //str += "<p>";
            //str += "<table cellpadding='0' cellspacing='0'  class='baocao'><tr><td class='tcenter' text-align='center' width='10%'><strong>STT</strong></td>" +
            //    "<td class='tcenter' text-align='center'><strong>Nội dung kiến nghị</strong></td>" +
            //    "<td class='tcenter' text-align='center' width='15%'><strong>Địa phương</strong></td></tr>";
            //count0 = 1;
            //foreach (var k in list_giaiquyet)
            //{
            //    str += "<tr><td class='tcenter' text-align='center'>" + count0 + "</td><td>" +
            //        HttpContext.Current.Server.HtmlDecode(k.NOIDUNG_KIENNGHI) +
            //            "</td><td class='tcenter' text-align='center'>" +
            //            HttpContext.Current.Server.HtmlDecode(k.TEN_DIAPHUONG) + "</td></tr>";
            //    count0++;
            //}
            //str += "</table>";

            return str;
        }
        public string BAOCAO_TK_PHULUC2 (int iKyHop,int iDonVi,int iLinhVuc)
        {
            string str = "";
            var list_donvi = _kn.GetAll_CoQuanByParam(null);
            var list = kn_report.getReportBaoCaoThongKePhuLuc2("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC2", iKyHop, iDonVi,iLinhVuc);
            List<decimal> list_id_donvi = new List<decimal>();
            list = list.OrderBy(x => x.VITRI).ToList();
            foreach (var l in list) { if (!list_id_donvi.Contains(l.ID_PARENT)) { list_id_donvi.Add(l.ID_PARENT); } }
            int count0 = 1;
            foreach (var d in list_id_donvi)
            {
                var kn_donvi = list.Where(x => x.ID_PARENT == d).ToList();
                str += "<tr><td colspan='4'><strong>" + count0 + ". " + list_donvi.Where(x => x.ICOQUAN == (int)d).FirstOrDefault().CTEN + "</strong></td></tr>";
                int count = 1;
                foreach (var k in kn_donvi)
                {
                    str += "<tr><td class='tcenter' text-align='center'>" + count + "</td><td>" + k.SOHIEUVANBAN +
                            "</td><td class='tcenter' text-align='center'>" + func.ConvertDateVN(k.NGAY.ToString()) + "</td><td>" + k.TRICHYEU + "</td></tr>";
                    count++;
                }
                count0++;
            }

            return str;
        }
        public string BAOCAO_TK_PHULUC9(int IKYHOP, int iTruocKyHop)
        {
            string str = "";
            var list = kn_report.getReportBaoCaoThongKePhuLuc7("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC7", IKYHOP, iTruocKyHop);
            list = list.OrderBy(x => x.ID_DONVI).ToList();
            List<int> list_id_donvi = new List<int>();
            foreach (var l in list)
            {
                if (!list_id_donvi.Contains((int)l.ID_DONVI)) { list_id_donvi.Add((int)l.ID_DONVI); }
            }
            string tenkyhop = "Bản thống kê thời gian nhận được báo cáo Tập hợp ý kiến, kiến nghị của cử tri ";
            if (iTruocKyHop == 1) { tenkyhop += "trước "; }else { tenkyhop += "sau "; }
            tenkyhop += Get_TenKyHop(IKYHOP) + ", " + Get_TenKhoaHop_By_IDKyHop(IKYHOP);
            int count = 1;
            str += "<tr><td colspan='6' class='tcenter b'>"+tenkyhop+"</td></tr>";
            str += "<tr><td class='tcenter b' width='5%'>STT</td><td class='tcenter b'>Tỉnh/ Thành phố</td>" +
                 "<td class='tcenter b'>Ngày nhận BC</td><td class='tcenter b'  width='5%'>STT</td>" +
                 "<td class='tcenter b'>Tỉnh/ Thành phố</td><td class='tcenter b'>Ngày nhận BC</td></tr>";
            foreach(var d in list_id_donvi) {
                if(count==1)
                {
                    str += "<tr>";
                }
                KIENNGHIPHULUC7 kn = list.Where(x => x.ID_DONVI == d).FirstOrDefault();
                string ngay = String.Format("{0:dd/MM}", Convert.ToDateTime(kn.NGAYNHAN));
                str += "<td class='tcenter' text-align='center'>" + count + "</td>" +
                        "<td >" + kn.TEN_TINHTHANH+ "</td>" +
                        "<td class='tcenter' text-align='center'>" + ngay + "</td>";
                if (count< list_id_donvi.Count())
                {//chưa cuối
                    if (count > 1)
                    {
                        if (count % 2 == 1) {
                            str += "</tr><tr>";
                        }                      
                    }
                }
                else {
                    if (count % 2 == 1)
                    {
                        str += "<td></td><td></td><td></td>";
                    }
                    str += "</tr>";
                }
                count++;
            }
            return str;
        }
        public string BAOCAO_TK_PHULUC6(string tungay,string denngay,int iDonVi,string thoigian_giamsat)
        {
            var list = kn_report.getReportBaoCaoThongKePhuLuc6("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC6", 
                        func.ConvertDateToSql(tungay), func.ConvertDateToSql(denngay), iDonVi);
            list = list.OrderBy(x => x.ID_DONVI).ToList();
            List<int> list_id_donvi = new List<int>();
            foreach (var l in list)
            {
                if (!list_id_donvi.Contains((int)l.ID_DONVI)) { list_id_donvi.Add((int)l.ID_DONVI); }
            }
            string str = "<tr><td class='tcenter b' text-align='center' colspan='3'>"+list.Count()+"  chuyên đề giám sát "+thoigian_giamsat+"</td></tr>";
            str += "<tr><td class='tcenter b'>STT</td><td class='tcenter b'>Cơ quan giám sát</td><td class='tcenter b'>Nội dung giám sát</td></tr>";
            int count = 1;
            foreach (var d in list_id_donvi)
            {
                var list_donvi = list.Where(x => x.ID_DONVI == d).ToList();
                if (list_donvi.Count() == 1)
                {
                    str += "<tr><td class='tcenter' text-align='center'>" + count + "</td>" +
                        "<td>" + list_donvi[0].TEN_DONVI + "</td>" +
                        "<td>" + list_donvi[0].KEHOACH + "</br>" + list_donvi[0].NOIDUNG + "</tr>";
                    count++;
                }
                else
                {
                    int count1 = 1;
                    foreach(var l in list_donvi)
                    {
                        if (count1 == 1)
                        {
                            str += "<tr><td class='tcenter' text-align='center'>" + count + "</td>" +
                                "<td rowspan='"+ list_donvi.Count()+ "'>" + l.TEN_DONVI + "</td>" +
                                "<td>" + l.KEHOACH + "</br>" + l.NOIDUNG + "</tr>";
                            count++;
                        }
                        else
                        {
                            str += "<tr><td class='tcenter' text-align='center'>" + count + "</td>" +
                                "<td>" + l.KEHOACH + "</br>" + l.NOIDUNG + "</tr>";
                            count++;
                        }
                        count1++;
                    }
                }
            }
              
            return str;
        }
        public string BAOCAO_TK_PHULUC4(int iDonVi,int iLinhVuc)
        {
            var list = kn_report.getReportBaoCaoThongKePhuLuc4("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC4", iDonVi,iLinhVuc);
            string str = "";
            int count = 1;
            list = list.Where(x => x.TONGKIENNGHI_DAGIAIQUYET>0 || x.TONGKIENNGHI_DANGGIAIQUYET>0 && x.ID_CAPCOQUAN==(decimal)ID_Capcoquan.Bobannganh).ToList();
            int tong_dagiaiquyet = (int)list.Sum(x => x.TONGKIENNGHI_DAGIAIQUYET);
            int tong_danggiaiquyet= (int)list.Sum(x => x.TONGKIENNGHI_DANGGIAIQUYET);
            int tong = tong_dagiaiquyet+ tong_danggiaiquyet;
            str += "<tr><td colspan='5' class='tcenter b'>Kết quả giải quyết đối với "+ tong+ " kiến nghị tồn đọng qua nhiều kỳ họp</td></tr>";
            str += "<tr><td colspan='5' class='tcenter b'>(" + tong_dagiaiquyet + " đã giải quyết xong; " +
                                            tong_danggiaiquyet + " kiến nghị chưa giải quyết)</td></tr>";
            str+= "<tr><td class='tcenter b'>STT</td><td class='tcenter b'>Tên bộ, ngành</td><td class='tcenter b'>Tổng số kiến nghị</td>"+
                    "<td class='tcenter b'>Kiến nghị đã giải quyết xong</td><td class='tcenter b'>Kiến nghị đang giải quyết</td></tr>";
            foreach (var l in list)
            {
                decimal tongkiennghi = l.TONGKIENNGHI_DAGIAIQUYET + l.TONGKIENNGHI_DANGGIAIQUYET;
                if (tongkiennghi > 0)
                {
                    str += "<tr><td class='tcenter' text-align='center'>" + count + "</td>" +
                    "<td>" + l.TEN_DONVI + "</td>" +
                    "<td class='tcenter' text-align='center'>" + tongkiennghi + "</td>" +
                    "<td class='tcenter' text-align='center'>" + l.TONGKIENNGHI_DAGIAIQUYET + "</td>" +
                    "<td class='tcenter' text-align='center'>" + l.TONGKIENNGHI_DANGGIAIQUYET + "</td>" +
                    "</tr>";
                    count++;
                }                
            }
            str += "<tr><td></td><td class='tcenter' text-align='center'><strong>Tổng</strong></td>"+
                "<td class='tcenter' text-align='center'><strong>"+ tong + "</strong></td>" +
                "<td class='tcenter' text-align='center'><strong>"+ tong_dagiaiquyet + "</strong></td>" +
                "<td class='tcenter' text-align='center'><strong>"+ tong_danggiaiquyet + "</strong></td>" +
                "</tr>";
            int tyle = 100;
            double tyle_dagiaiquyet = 0;
            double tyle_danggiaiquyet = 0;
            if (tong > 0)
            {
                tyle_dagiaiquyet = Math.Round(Convert.ToDouble(tong_dagiaiquyet) * 100 / Convert.ToDouble(tong), 2);
                tyle_danggiaiquyet = 100 - tyle_dagiaiquyet;
            }
            
            str += "<tr><td></td><td class='tcenter' text-align='center'><strong>Tỷ lệ</strong></td>" +
                "<td class='tcenter' text-align='center'><strong>" + tyle + " %</strong></td>" +
                "<td class='tcenter' text-align='center'><strong>" + tyle_dagiaiquyet + " %</strong></td>" +
                "<td class='tcenter' text-align='center'><strong>" + tyle_danggiaiquyet + " %</strong></td>" +
                "</tr>";
            return str;
        }
        public string BAOCAO_TK_PHULUC3(int iKyHop,int iDonVi,int iLinhVuc)
        {
            var list = kn_report.getReportBaoCaoThongKePhuLuc3("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC3", iKyHop, iDonVi,iLinhVuc);
            List<decimal> list_id_phanloai = new List<decimal>();
            int count = 0;
            string str = "";
            foreach(var l in list)
            {
                if (!list_id_phanloai.Contains(l.ID_PHANLOAI))
                {
                    list_id_phanloai.Add(l.ID_PHANLOAI);count++;
                    var list1 = list.Where(x => x.ID_PHANLOAI == l.ID_PHANLOAI).ToList();
                    string col_lotrinh = "";
                    if (l.ID_PHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                    {
                        col_lotrinh = "<td class='tcenter' text-align='center' width='20%'><strong>Lộ trình giải quyết</strong></td>";
                    }
                    str += "<tr><td class='tcenter b' text-align='center' colspan='3'><strong>Bảng "+count+": "+l.TENPHANLOAI+" (" + list1.Count() + ")</strong></td></tr>";
                    str += "<tr><td class='tcenter' text-align='center'><strong>STT</strong></td>" +
                            "<td class='tcenter' text-align='center'><strong>Nội dung</strong></td>" +col_lotrinh+"</tr>";
                    int count1 = 1;
                    foreach (var k in list1)
                    {
                        string col_lotrinh1 = "";
                        if (l.ID_PHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                        {
                            DateTime firstdate = Convert.ToDateTime("0001-01-01");
                            if (k.NGAYDUKIEN != firstdate)
                            {
                                col_lotrinh1 = "<td text-align='center' width='20%'>"+func.ConvertDateVN(k.NGAYDUKIEN.ToString())+"</td>";
                            }else
                            {
                                col_lotrinh1 = "<td></td>";
                            }                            
                        }                        
                        str += "<tr><td class='tcenter' text-align='center'>" + count1 + "</td>" +
                                "<td>" + k.NOIDUNG_KIENNGHI + "</td>"+ col_lotrinh1 + "</tr>";
                        count1++;
                    }
                }
            }

            //string str = "";
            //var list_colotrinh = list.Where(x => x.TINHTRANG_TRALOI == 4).ToList();
            //var list_kolotrinh = list.Where(x => x.TINHTRANG_TRALOI == 5).ToList();
            //List<int> id_donvi_colotrinh = new List<int>();
            //foreach( var l in list_colotrinh)
            //{
            //    if (!id_donvi_colotrinh.Contains((int)l.ID_DONVI)) { id_donvi_colotrinh.Add((int)l.ID_DONVI); }
            //}
            //List<int> id_donvi_kolotrinh = new List<int>();
            //foreach (var l in list_kolotrinh)
            //{
            //    if (!id_donvi_kolotrinh.Contains((int)l.ID_DONVI)) { id_donvi_kolotrinh.Add((int)l.ID_DONVI); }
            //}
            //str += "<tr><td class='tcenter b' text-align='center' colspan='3'><strong>Bảng 1: Các văn bản có lộ trình giải quyết ("+ list_colotrinh .Count()+ ")</strong></td></tr>";
            //str += "<tr><td class='tcenter' text-align='center'><strong>STT</strong></td>" +
            //        "<td class='tcenter' text-align='center'><strong>Nội dung</strong></td>" +
            //        "<td class='tcenter' text-align='center' width='20%'><strong>Lộ trình giải quyết</strong></td>" +
            //        "</tr>";
            //if (id_donvi_colotrinh != null)
            //{
            //    int count = 1;
            //    foreach(var d in id_donvi_colotrinh)
            //    {
            //        str += "<tr><td colspan='3'><strong>"+count+". "+list_colotrinh.Where(x=>x.ID_DONVI==d).FirstOrDefault().TEN_DONVI+"</strong></td></tr>";
            //        var list_colotrinh_donvi = list_colotrinh.Where(x => x.ID_DONVI == d).ToList();
            //        int count1 = 1;
            //        foreach (var l in list_colotrinh_donvi)
            //        {
            //            str += "<tr><td class='tcenter' text-align='center'>"+count1+"</td>"+
            //                    "<td>"+HttpContext.Current.Server.HtmlDecode(l.NOIDUNG_KIENNGHI) +
            //                    "</td><td>" + HttpContext.Current.Server.HtmlDecode(l.LOTRINH) + "</td></tr>";
            //            count1++;
            //        }
            //    }

            //}
            //str += "<tr><td class='tcenter b' text-align='center' colspan='3'><strong>Bảng 2: Các văn bản chưa lộ trình giải quyết (" + list_kolotrinh.Count() + ")</strong></td></tr>";

            //str += "<tr><td class='tcenter' text-align='center'><strong>STT</strong></td>" +
            //        "<td class='tcenter' text-align='center' colspan='2'><strong>Nội dung</strong></td>" +

            //        "</tr>";
            //if (id_donvi_kolotrinh != null)
            //{
            //    int count = 1;
            //    foreach (var d in id_donvi_kolotrinh)
            //    {
            //        str += "<tr><td colspan='3'><strong>" + count + ". " + list_kolotrinh.Where(x => x.ID_DONVI == d).FirstOrDefault().TEN_DONVI + "</strong></td></tr>";
            //        var list_kolotrinh_donvi = list_kolotrinh.Where(x => x.ID_DONVI == d).ToList();
            //        int count1 = 1;
            //        foreach (var l in list_kolotrinh_donvi)
            //        {
            //            str += "<tr><td class='tcenter' text-align='center'>" + count1 + "</td>" +
            //                    "<td colspan='2'>" + l.NOIDUNG_KIENNGHI +
            //                    "</td></tr>";
            //            count1++;
            //        }
            //    }

            //}
            return str;
        }
        public string BAOCAO_TK_PHULUC5B(int iKyHop,int iDonVi,int iLinhVuc)
        {
            string str = "";
            var list = kn_report.getReportBaoCaoThongKePhuLuc5B("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC5B", iKyHop, iDonVi, iLinhVuc);
            var list1 = list.Where(x => x.ID_GOP <= 0).ToList();
            
            
            str += "<tr><td class='tcenter b' text-align='center' colspan='3'>Danh mục " + list1.GroupBy(x=>x.ID_KIENNGHI).Count() + " kiến nghị cử tri gửi tới " + Get_TenKyHop(iKyHop) +
                            ", " + Get_TenKhoaHop_By_IDKyHop(iKyHop) + " đang trong quá trình giải quyết</td></tr>";
            List<decimal> list_id_phanloai = new List<decimal>();
            int count_phanloai = 1;
            
            foreach (var p in list1)
            {
                if (!list_id_phanloai.Contains(p.ID_PHANLOAI_TRALOI))
                {
                    list_id_phanloai.Add(p.ID_PHANLOAI_TRALOI);
                    int colspan = 1;
                    var list2 = list1.Where(x => x.ID_PHANLOAI_TRALOI == p.ID_PHANLOAI_TRALOI).ToList();
                    string col_lotrinh = "<td class='tcenter' text-align='center' width='15%'><strong>Lộ trình giải quyết</strong></td>";
                    if (p.ID_PHANLOAI_TRALOI != (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                    {
                        col_lotrinh = ""; colspan = 2;
                    }
                    str += "<tr><td class='tcenter b' text-align='center' colspan='3'>Bảng "+count_phanloai+": "+list2.FirstOrDefault().TEN_PHANLOAI_TRALOI+" (" + list2.GroupBy(x => x.ID_KIENNGHI).Count() + ")</td></tr>";
                    str += "<tr><td class='tcenter' text-align='center'><strong>STT</strong></td>" +
                        "<td class='tcenter' colspan='"+ colspan + "' text-align='center'><strong>Nội dung kiến nghị</strong></td>" + col_lotrinh+
                        "</tr>";
                    count_phanloai++;
                    List<decimal> list_id_donvi = new List<decimal>();
                    int count_donvi = 1;
                    foreach (var l in list2)
                    {
                        if (!list_id_donvi.Contains(l.ID_THAMQUYENCOQUAN))
                        {
                            var list_donvi = list2.Where(x => x.ID_THAMQUYENCOQUAN == l.ID_THAMQUYENCOQUAN).OrderByDescending(x=>x.ID_LINHVUC).ToList();
                            list_id_donvi.Add(l.ID_THAMQUYENCOQUAN);
                            str += "<tr><td colspan='3'><strong>" + count_donvi + ". " + list_donvi.FirstOrDefault().TEN_THAMQUYENCOQUAN + " (" + list_donvi.GroupBy(x => x.ID_KIENNGHI).Count() + ")</strong></td></tr>";
                            List<decimal> list_id_linhvuc = new List<decimal>();
                            int count_linhvuc = 1;
                            foreach (var m in list_donvi)
                            {
                                if (!list_id_linhvuc.Contains(m.ID_LINHVUC))
                                {
                                    list_id_linhvuc.Add(m.ID_LINHVUC);                                    
                                    var list_linhvuc = list_donvi.Where(x => x.ID_LINHVUC == m.ID_LINHVUC).ToList();
                                    string tenlinhvuc = "Lĩnh vực: Chưa xác định";
                                    if (m.ID_LINHVUC != 0) { tenlinhvuc = list_linhvuc.FirstOrDefault().TEN_LINHVUC; }
                                    str += "<tr><td colspan='3'><strong>" + count_donvi + "."+ count_linhvuc+". " + tenlinhvuc + " (" + list_linhvuc.GroupBy(x=>x.ID_KIENNGHI).Count() + ")</strong></td></tr>";
                                    int count2 = 1;
                                    List<decimal> list_id_kiennghi = new List<decimal>();
                                    foreach (var k in list_linhvuc)
                                    {
                                        if (!list_id_kiennghi.Contains(k.ID_KIENNGHI))
                                        {
                                            list_id_kiennghi.Add(k.ID_KIENNGHI);
                                            string donvitiepnhan = "";
                                            int count_donvi_gop = 0;
                                            var kn_gop = list.Where(x => x.ID_KIENNGHI_PARENT_GOP == l.ID_KIENNGHI).ToList();
                                            if (kn_gop.Count() > 0)
                                            {
                                                List<string> list_donvi_gop = new List<string>();
                                                foreach (var g in kn_gop)
                                                {
                                                    if (!list_donvi_gop.Contains(g.TENDONVITIEPNHAN_GOP))
                                                    {
                                                        if (count_donvi_gop > 0) { donvitiepnhan += ", "; }
                                                        donvitiepnhan += g.TENDONVITIEPNHAN_GOP;
                                                        list_donvi_gop.Add(g.TENDONVITIEPNHAN_GOP);
                                                        count_donvi_gop++;
                                                    }
                                                }
                                            }
                                            if (donvitiepnhan == "")
                                            {
                                                donvitiepnhan = l.TEN_DONVITIEPNHAN;
                                            }
                                            donvitiepnhan = " (Cử tri " + donvitiepnhan + ")";

                                            string ngaygiaiquyet = "";
                                            if (k.NGAY_DUKIENGIAIQUYET != Convert.ToDateTime("0001-01-01"))
                                            {
                                                ngaygiaiquyet = func.ConvertDateVN(k.NGAY_DUKIENGIAIQUYET.ToString());
                                            }
                                            string col = "<td class='tcenter'>" + ngaygiaiquyet + "</td>";
                                            if (p.ID_PHANLOAI_TRALOI != (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH)
                                            {
                                                col = "";
                                            }
                                            str += "<tr><td class='tcenter' text-align='center'>" + count2 + "</td><td colspan='"+ colspan + "'>" + k.NOIDUNG_KIENNGHI + donvitiepnhan + "</td>"+col+"</tr>";
                                            count2++;
                                        }                                        
                                    }
                                    count_linhvuc++;
                                }
                            }
                            count_donvi++;
                        }
                    }
                }
            }

            
            
            /*
            var list_colotrinh = list.Where(x=>x.TINHTRANG_TRALOI==4).ToList();
            var list_kolotrinh = list.Where(x => x.TINHTRANG_TRALOI == 5).ToList();
            List<int> list_id_donvi = new List<int>();
            foreach (var l in list)
            {
                if (!list_id_donvi.Contains((int)l.ID_DONVI)) { list_id_donvi.Add((int)l.ID_DONVI); }
            }
            int count = 1;
            str += "<tr><td class='tcenter b' text-align='center' colspan='3'>Danh mục "+ list.Count() + " kiến nghị cử tri gửi tới "+Get_TenKyHop(iKyHop)+
                            ", "+ Get_TenKhoaHop_By_IDKyHop(iKyHop) + " đang trong quá trình giải quyết</td></tr>";
            str += "<tr><td class='tcenter b' text-align='center' colspan='3'>Bảng 1: Các văn bản có lộ trình giải quyết (" + list_colotrinh.Count() + ")</td></tr>";
            str += "<tr><td class='tcenter' text-align='center'><strong>STT</strong></td>"+
                "<td class='tcenter' text-align='center'><strong>Nội dung kiến nghị</strong></td>"+
                "<td class='tcenter' text-align='center' width='15%'><strong>Lộ trình giải quyết</strong></td></tr>";
            foreach (var l in list_id_donvi)//id_donvi
            {
                var list_kn_donvi = list_colotrinh.Where(x => x.ID_DONVI == l).ToList();
                if (list_kn_donvi.Count() > 0)
                {
                    str += "<tr><td colspan='3'><strong>" + count + ". " + list_kn_donvi.Where(x => x.ID_DONVI == l).FirstOrDefault().TEN_DONVI + " (" + list_kn_donvi.Count() + ")</strong></td></tr>";
                    foreach (var l1 in list_kn_donvi)//list_kiến nghị đơn vị
                    {
                        List<int> list_id_linhvuc = new List<int>();
                        if (!list_id_linhvuc.Contains((int)l1.ID_LINHVUC)) { list_id_linhvuc.Add((int)l1.ID_LINHVUC); }
                        int count1 = 1;
                        foreach (var lv in list_id_linhvuc)
                        {
                            var list_kn_linhvuc = list_kn_donvi.Where(x => x.ID_LINHVUC == lv).ToList();
                            if (list_kn_linhvuc.Count() > 0)
                            {
                                str += "<tr><td colspan='3'><strong>" + count + "." + count1 + " " + list_kn_linhvuc.Where(x => x.ID_LINHVUC == lv).FirstOrDefault().TENLINNHVUC + " (" + list_kn_linhvuc.Count() + ")</strong></td></tr>";
                                int count2 = 1;
                                foreach (var k in list_kn_linhvuc)
                                {
                                    str += "<tr><td class='tcenter' text-align='center'>" + count2 + "</td><td>" + k.NOIDUNG_KIENNGHI + "</td><td>" + k.TRALOI_KIENNGHI + "</td></tr>";
                                    count2++;
                                }
                                count1++;
                            }                            
                        }
                    }
                    count++;
                }                
            }
            count = 1;
            str += "<tr><td class='tcenter b' text-align='center' colspan='3'>Bảng 2: Các văn bản chưa có lộ trình giải quyết (" + list_colotrinh.Count() + ")</td></tr>";
            str += "<tr><td class='tcenter' text-align='center'><strong>STT</strong></td>" +
                    "<td class='tcenter' text-align='center' colspan='2'><strong>Nội dung kiến nghị</strong></td></tr>";
            foreach (var l in list_id_donvi)//id_donvi
            {
                var list_kn_donvi = list_kolotrinh.Where(x => x.ID_DONVI == l).ToList();
                if (list_kn_donvi.Count() > 0)
                {
                    str += "<tr><td colspan='3'><strong>" + count + ". " + list_kn_donvi.Where(x => x.ID_DONVI == l).FirstOrDefault().TEN_DONVI + " (" + list_kn_donvi.Count() + ")</strong></td></tr>";
                    foreach (var l1 in list_kn_donvi)//list_kiến nghị đơn vị
                    {
                        List<int> list_id_linhvuc = new List<int>();
                        if (!list_id_linhvuc.Contains((int)l1.ID_LINHVUC)) { list_id_linhvuc.Add((int)l1.ID_LINHVUC); }
                        int count1 = 1;
                        foreach (var lv in list_id_linhvuc)
                        {
                            var list_kn_linhvuc = list_kn_donvi.Where(x => x.ID_LINHVUC == lv).ToList();
                            if (list_kn_linhvuc.Count() > 0)
                            {
                                str += "<tr><td colspan='3'><strong>" + count + "." + count1 + " " + list_kn_linhvuc.Where(x => x.ID_LINHVUC == lv).FirstOrDefault().TENLINNHVUC + " (" + list_kn_linhvuc.Count() + ")</strong></td></tr>";
                                int count2 = 1;
                                foreach (var k in list_kn_linhvuc)
                                {
                                    str += "<tr><td class='tcenter' text-align='center'>" + count2 + "</td><td colspan='2'>" + k.NOIDUNG_KIENNGHI + "</td></tr>";
                                    count2++;
                                }
                                count1++;
                            }                            
                        }
                    }
                    count++;
                }                
            }
            */
            return str;
        }
        public string BAOCAO_TK_PHULUC5(string tungay,string denngay,int idonvi,int ilinhvuc,string thoidiem_giaiquyet="")
        {
            string str = "";
            var list = kn_report.getReportBaoCaoThongKePhuLuc5("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC5", func.ConvertDateToSql(tungay), func.ConvertDateToSql(denngay),idonvi,ilinhvuc);
            var list1 = list.Where(x => x.ID_GOP<=0 && x.ID_PHANLOAI == (decimal)PhanLoai_TraLoiKienNghi.DANGGIAIQUYET_COLOTRINH).ToList();
            List<decimal> list_id_coquan = new List<decimal>();
            str += "<tr><td class='tcenter b' text-align='center' colspan='3'><strong>Bảng 1: " + list1.Count() + " kiến nghị có lộ trình giải quyết dứt điểm đến " + thoidiem_giaiquyet + "</strong></td></tr>";
            str += "<tr><td class='tcenter' text-align='center'><strong>STT</strong></td>" +
                    "<td class='tcenter' text-align='center'><strong>Nội dung kiến nghị/địa phương kiến nghị</strong></td>" +
                    "<td class='tcenter' text-align='center'><strong>Thời điểm kiến nghị</strong></td>" +
                    "</tr>";
            int count_donvi = 0;
            foreach (var d in list1)
            {
                if (!list_id_coquan.Contains(d.ID_THAMQUYENCOQUAN))
                {
                    list_id_coquan.Add(d.ID_THAMQUYENCOQUAN); count_donvi++;
                     var list2 = list1.Where(x => x.ID_THAMQUYENCOQUAN == d.ID_THAMQUYENCOQUAN).ToList();
                    str += "<tr><td colspan='3'><strong>" + count_donvi + ". " + list2.Where(x => x.ID_THAMQUYENCOQUAN == d.ID_THAMQUYENCOQUAN).FirstOrDefault().TEN_THAMQUYENCOQUAN + " (" + list2.Count() + ")</strong></td></tr>";
                    int count1 = 1;
                    List<decimal> list_id_kiennghi = new List<decimal>();
                    foreach (var l in list2)
                    {
                        if (!list_id_kiennghi.Contains(l.ID_KIENNGHI))
                        {
                            list_id_kiennghi.Add(l.ID_KIENNGHI);
                            string donvitiepnhan = "";
                            int count_donvi_gop = 0;
                            var kn_gop = list.Where(x => x.ID_KIENNGHI_PARENT_GOP == l.ID_KIENNGHI).ToList();
                            if (kn_gop.Count() > 0)
                            {
                                List<string> list_donvi_gop = new List<string>();
                                foreach (var g in kn_gop)
                                {
                                    if (!list_donvi_gop.Contains(g.TENDONVITIEPNHAN_GOP))
                                    {
                                        if (count_donvi_gop > 0) { donvitiepnhan += ", "; }
                                        donvitiepnhan += g.TENDONVITIEPNHAN_GOP;
                                        list_donvi_gop.Add(g.TENDONVITIEPNHAN_GOP);
                                        count_donvi_gop++;
                                    }
                                }
                            }
                            if (donvitiepnhan == "")
                            {
                                donvitiepnhan = l.TEN_DONVITIEPNHAN;
                            }
                            donvitiepnhan = " (Cử tri " + donvitiepnhan + ")";
                            str += "<tr><td class='tcenter' text-align='center'>" + count1 + "</td>" +
                                    "<td>" + l.NOIDUNG_KIENNGHI + donvitiepnhan +
                                    "</td><td class='tcenter'>" + l.TEN_KYHOP + ", " + l.TEN_KHOAHOP + "</td></tr>";
                            count1++;
                        }
                        
                    }
                }
            }
            
            //var list_colotrinh = list.Where(x => x.TINHTRANG_TRALOI == 4).ToList();
            //var list_chuagiaiquyet = list.Where(x => x.TINHTRANG_TRALOI == 6).ToList();
            //var list_nguonluc = list.Where(x => x.TINHTRANG_TRALOI == 7).ToList();
            //List<int> id_donvi_colotrinh = new List<int>();
            //foreach (var l in list_colotrinh)
            //{
            //    if (!id_donvi_colotrinh.Contains((int)l.ID_DONVI)) { id_donvi_colotrinh.Add((int)l.ID_DONVI); }
            //}
            //List<int> id_donvi_chuagiaiquyet = new List<int>();
            //foreach (var l in list_chuagiaiquyet)
            //{
            //    if (!id_donvi_chuagiaiquyet.Contains((int)l.ID_DONVI)) { id_donvi_chuagiaiquyet.Add((int)l.ID_DONVI); }
            //}
            //List<int> id_nguonluc = new List<int>();
            //foreach (var l in list_nguonluc)
            //{
            //    if (!id_nguonluc.Contains((int)l.ID_DONVI)) { id_nguonluc.Add((int)l.ID_DONVI); }
            //}
            //str += "<tr><td class='tcenter b' text-align='center' colspan='3'><strong>Bảng 1: " + list_colotrinh.Count() + " kiến nghị có lộ trình giải quyết dứt điểm đến "+ thoidiem_giaiquyet + "</strong></td></tr>";
            //str += "<tr><td class='tcenter' text-align='center'><strong>STT</strong></td>" +
            //        "<td class='tcenter' text-align='center'><strong>Nội dung kiến nghị/địa phương kiến nghị</strong></td>" +
            //        "<td class='tcenter' text-align='center'><strong>Thời điểm kiến nghị</strong></td>" +
            //        "</tr>";
            //if (id_donvi_colotrinh != null)
            //{
            //    int count = 1;
            //    foreach (var d in id_donvi_colotrinh)
            //    {
            //        var list_colotrinh_donvi = list_colotrinh.Where(x => x.ID_DONVI == d).ToList();
            //        str += "<tr><td colspan='3'><strong>" + count + ". " + list_colotrinh.Where(x => x.ID_DONVI == d).FirstOrDefault().TEN_DONVI + " ("+ list_colotrinh_donvi.Count() + ")</strong></td></tr>";
                    
            //        int count1 = 1;
            //        foreach (var l in list_colotrinh_donvi)
            //        {
            //            str += "<tr><td class='tcenter' text-align='center'>" + count1 + "</td>" +
            //                    "<td>" + l.NOIDUNG_KIENNGHI +
            //                    "</td><td>" + l.TEN_KYHOP + ", "+l.TEN_KHOAHOP+"</td></tr>";
            //            count1++;
            //        }
            //    }

            //}

            //str += "<tr><td class='tcenter b' text-align='center' colspan='3'><strong>Bảng 2: " + list_chuagiaiquyet.Count() + " kiến nghị chưa thể giải quyết ngay</strong></td></tr>";
            //str += "<tr><td class='tcenter' text-align='center'><strong>STT</strong></td>" +
            //        "<td class='tcenter' text-align='center'><strong>Nội dung kiến nghị/địa phương kiến nghị</strong></td>" +
            //        "<td class='tcenter' text-align='center'><strong>Thời điểm kiến nghị</strong></td>" +
            //        "</tr>";
            //if (id_donvi_chuagiaiquyet != null)
            //{
            //    int count = 1;
            //    foreach (var d in id_donvi_chuagiaiquyet)
            //    {
            //        var list_colotrinh_donvi = list_chuagiaiquyet.Where(x => x.ID_DONVI == d).ToList();
            //        str += "<tr><td colspan='3'><strong>" + count + ". " + list_chuagiaiquyet.Where(x => x.ID_DONVI == d).FirstOrDefault().TEN_DONVI + " ("+ list_colotrinh_donvi.Count() + ")</strong></td></tr>";
            //        int count1 = 1;
            //        foreach (var l in list_colotrinh_donvi)
            //        {
            //            str += "<tr><td class='tcenter' text-align='center'>" + count1 + "</td>" +
            //                    "<td>" + l.NOIDUNG_KIENNGHI +
            //                    "</td><td>" + l.TEN_KYHOP + ", " + l.TEN_KHOAHOP + "</td></tr>";
            //            count1++;
            //        }
            //    }

            //}

            //str += "<tr><td class='tcenter b' text-align='center' colspan='3'><strong>Bảng 3: " + list_nguonluc.Count() + " kiến nghị chưa có nguồn lực để giải quyết</strong></td></tr>";
            //str += "<tr><td class='tcenter' text-align='center'><strong>STT</strong></td>" +
            //        "<td class='tcenter' text-align='center'><strong>Nội dung kiến nghị/địa phương kiến nghị</strong></td>" +
            //        "<td class='tcenter' text-align='center'><strong>Thời điểm kiến nghị</strong></td>" +
            //        "</tr>";
            //if (id_nguonluc != null)
            //{
            //    int count = 1;
            //    foreach (var d in id_nguonluc)
            //    {
            //        var list_colotrinh_donvi = list_nguonluc.Where(x => x.ID_DONVI == d).ToList();
            //        str += "<tr><td colspan='3'><strong>" + count + ". " + list_nguonluc.Where(x => x.ID_DONVI == d).FirstOrDefault().TEN_DONVI + " ("+ list_colotrinh_donvi.Count() + ")</strong></td></tr>";
            //        int count1 = 1;
            //        foreach (var l in list_colotrinh_donvi)
            //        {
            //            str += "<tr><td class='tcenter' text-align='center'>" + count1 + "</td>" +
            //                    "<td>" + l.NOIDUNG_KIENNGHI +
            //                    "</td><td>" + l.TEN_KYHOP + ", " + l.TEN_KHOAHOP + "</td></tr>";
            //            count1++;
            //        }
            //    }

            //}
            return str;
        }
        public string BAOCAO_TK_PHULUC1(int iKyHop,int iDonVi,int iLinhVuc)
        {
            var list = kn_report.getReportBaoCaoThongKePhuLuc1("PKG_KIENNGHI_BAOCAO.PRO_BAOCAO_TK_PHULUC1", iKyHop, iDonVi,iLinhVuc);
            string str = "";
            Random RandomNumber = new Random();

            var donvi_level0 = list.Where(X => X.ID_PARENT == 0).OrderBy(x => x.TEN_DONVI).ToList();
            int count0 = 1;
            string tong_baocao = "Cộng ";
            int TONGKIENNGHI_ = 0; int TONGKIENNGHI_TRALOI_ = 0; int TONGKIENNGHI_DAGIAIQUYET_ = 0;
            int TONGKIENNGHI_DANGGIAIQUYET_ = 0; int TONGKIENNGHI_GIAITRINH_ = 0;
            foreach (var d0 in donvi_level0)
            {
                
                string roman_num = Convert_to_RomanNumber(count0);
                if (count0 > 1) { tong_baocao += " + "; }
                tong_baocao += roman_num;
                str += "<tr><td class='tcenter' text-align='center' colspan='7'><strong>"+roman_num+". "+d0.TEN_DONVI+"</strong></td></tr>";
                var donvi1 = list.Where(X => X.ID_PARENT == d0.ID_DONVI).OrderBy(x => x.TEN_DONVI).ToList();
                int count1 = 1;
                int TONGKIENNGHI = 0;int TONGKIENNGHI_TRALOI = 0;int TONGKIENNGHI_DAGIAIQUYET = 0;
                int TONGKIENNGHI_DANGGIAIQUYET = 0;int TONGKIENNGHI_GIAITRINH = 0;
                foreach (var l in donvi1)
                {
                    str += "<tr><td class='tcenter' text-align='center'>" + count1 + "</td><td>" + l.TEN_DONVI +
                        "</td><td class='tcenter' text-align='center'>" + l.TONGKIENNGHI.ToString("#,##0").Replace(",", ".") + "</td>" +
                        "</td><td class='tcenter' text-align='center'>" + l.TONGKIENNGHI_TRALOI.ToString("#,##0").Replace(",", ".") + "</td>" +
                        "</td><td class='tcenter' text-align='center'>" + l.TONGKIENNGHI_DAGIAIQUYET.ToString("#,##0").Replace(",", ".") + "</td>" +
                        "</td><td class='tcenter' text-align='center'>" + l.TONGKIENNGHI_DANGGIAIQUYET.ToString("#,##0").Replace(",", ".") + "</td>" +
                        "</td><td class='tcenter' text-align='center'>" + l.TONGKIENNGHI_GIAITRINH.ToString("#,##0").Replace(",", ".") + "</td>" +
                        "</tr>";
                    TONGKIENNGHI += (int)l.TONGKIENNGHI;
                    TONGKIENNGHI_TRALOI += (int)l.TONGKIENNGHI_TRALOI;
                    TONGKIENNGHI_DAGIAIQUYET += (int)l.TONGKIENNGHI_DAGIAIQUYET;
                    TONGKIENNGHI_DANGGIAIQUYET += (int)l.TONGKIENNGHI_DANGGIAIQUYET;
                    TONGKIENNGHI_GIAITRINH += (int)l.TONGKIENNGHI_GIAITRINH;
                    count1++;
                }
                TONGKIENNGHI_ += TONGKIENNGHI;
                TONGKIENNGHI_TRALOI_ += TONGKIENNGHI_TRALOI;
                TONGKIENNGHI_DAGIAIQUYET_ += TONGKIENNGHI_DAGIAIQUYET;
                TONGKIENNGHI_DANGGIAIQUYET_ += TONGKIENNGHI_DANGGIAIQUYET;
                TONGKIENNGHI_GIAITRINH_ += TONGKIENNGHI_GIAITRINH;
                str += "<tr><td text-align='center' class='tcenter' colspan='2'><strong>Cộng " + roman_num + " </strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_TRALOI.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_DAGIAIQUYET.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_DANGGIAIQUYET.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_GIAITRINH.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
                    "</tr>";
                int TONGKIENNGHI_tyle = 100; double TONGKIENNGHI_TRALOI_tyle = 0; double TONGKIENNGHI_DAGIAIQUYET_tyle = 0;
                double TONGKIENNGHI_DANGGIAIQUYET_tyle = 0; double TONGKIENNGHI_GIAITRINH_tyle = 0;
                if (TONGKIENNGHI > 0)
                {
                    TONGKIENNGHI_TRALOI_tyle = Math.Round(Convert.ToDouble(TONGKIENNGHI_TRALOI) * 100 / Convert.ToDouble(TONGKIENNGHI), 2);
                    TONGKIENNGHI_DAGIAIQUYET_tyle = Math.Round(Convert.ToDouble(TONGKIENNGHI_DAGIAIQUYET) * 100 / Convert.ToDouble(TONGKIENNGHI), 2);
                    TONGKIENNGHI_DANGGIAIQUYET_tyle = Math.Round(Convert.ToDouble(TONGKIENNGHI_DANGGIAIQUYET) * 100 / Convert.ToDouble(TONGKIENNGHI), 2);
                    TONGKIENNGHI_GIAITRINH_tyle = Math.Round(Convert.ToDouble(TONGKIENNGHI_GIAITRINH) * 100 / Convert.ToDouble(TONGKIENNGHI), 2);
                }
                str += "<tr><td text-align='center' class='tcenter' colspan='2'><strong>Tỷ lệ </strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>"+ TONGKIENNGHI_tyle + " %</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_TRALOI_tyle + " %</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_DAGIAIQUYET_tyle + " %</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_DANGGIAIQUYET_tyle + " %</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_GIAITRINH_tyle + " %</strong></td>" +
                    "</tr>";

                count0++;
            }
            str += "<tr><td text-align='center' class='tcenter' colspan='2'><strong>" + tong_baocao + " </strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_TRALOI_.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_DAGIAIQUYET_.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_DANGGIAIQUYET_.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_GIAITRINH_.ToString("#,##0").Replace(",", ".") + "</strong></td>" +
                    "</tr>";
            int TONGKIENNGHI_tyle_ = 100; double TONGKIENNGHI_TRALOI_tyle_ = 0; double TONGKIENNGHI_DAGIAIQUYET_tyle_ = 0;
            double TONGKIENNGHI_DANGGIAIQUYET_tyle_ = 0; double TONGKIENNGHI_GIAITRINH_tyle_ = 0;
            if (TONGKIENNGHI_ > 0)
            {
                TONGKIENNGHI_TRALOI_tyle_ = Math.Round(Convert.ToDouble(TONGKIENNGHI_TRALOI_) * 100 / Convert.ToDouble(TONGKIENNGHI_), 2);
                TONGKIENNGHI_DAGIAIQUYET_tyle_ = Math.Round(Convert.ToDouble(TONGKIENNGHI_DAGIAIQUYET_) * 100 / Convert.ToDouble(TONGKIENNGHI_), 2);
                TONGKIENNGHI_DANGGIAIQUYET_tyle_ = Math.Round(Convert.ToDouble(TONGKIENNGHI_DANGGIAIQUYET_) * 100 / Convert.ToDouble(TONGKIENNGHI_), 2);
                TONGKIENNGHI_GIAITRINH_tyle_ = Math.Round(Convert.ToDouble(TONGKIENNGHI_GIAITRINH_) * 100 / Convert.ToDouble(TONGKIENNGHI_), 2);
            }
            str += "<tr><td text-align='center' class='tcenter' colspan='2'><strong>Tỷ lệ </strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>"+ TONGKIENNGHI_tyle_ + " %</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_TRALOI_tyle_ + " %</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_DAGIAIQUYET_tyle_ + " %</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_DANGGIAIQUYET_tyle_ + " %</strong></td>" +
                    "</td><td class='tcenter' text-align='center'><strong>" + TONGKIENNGHI_GIAITRINH_tyle_ + " %</strong></td>" +
                    "</tr>";
            return str;
        }
        public DateTime NgayKetThuc_DuKien(DateTime ngaybatdau)
        {
            int songaylamviec = 60;
            DateTime ngayketthuc = ngaybatdau.AddDays(songaylamviec);
            int count_t7cn = Count_Thu7CN_TuNgayDenNgay(ngaybatdau, ngayketthuc);
            int tong_ngay = count_t7cn + songaylamviec;
            ngayketthuc = ngaybatdau.AddDays(tong_ngay);
            return ngayketthuc;
        }
        public int Count_Thu7CN_TuNgayDenNgay(DateTime tungay, DateTime denngay)
        {
            int count = 0;
            DateTime ngaybatdau = Convert.ToDateTime(tungay).Date;
            DateTime First = ngaybatdau; // ngày bắt đầu làm việc
            DateTime Last = Convert.ToDateTime(denngay).Date; // ngày kết thúc làm việc
            DateTime FirstSunday = First.AddDays(7 - (double)First.DayOfWeek); // ngày chủ nhật đầu tiên
            DateTime LastSunday = Last.AddDays(-(double)Last.DayOfWeek); // ngày chủ nhật cuối cùng
            count = LastSunday.Subtract(FirstSunday).Days / 7 + 1;
            count = count * 2;
            return count;
        }
        //public XLWorkbook Excel_BAOCAO_TK_PHULUC1(DataTable dt, int iKyHop)
        //{
        //    DataTable excel = new DataTable("excel");
        //    //using (XLWorkbook wb = new XLWorkbook())
        //    //{
        //    XLWorkbook ws = new XLWorkbook();
        //        ws.Worksheets.Add("Phụ lục 1");
        //        // Merge a row
        //        ws.Cell("A2").Value = "Phụ lục 1";
        //        ws.Range("A2:G3").Row(1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //        ws.Cell("A3").Value = "Bảng Tập hợp kết quả giải quyết, trả lời  2.458 kiến nghị của cử tri";
        //        ws.Range("A3:G4").Row(1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //        ws.Cell("A4").Value = "Tại kỳ họp thứ 3, Quốc hội khóa XIVcủa các bộ, ngành";
        //        ws.Range("A4:G5").Row(1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //        ws.Cell("A7").Value = "STT";
        //        ws.Range("A7:B8").Column(1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //        ws.Cell("B7").Value = "Tên cơ quan, đơn vị";
        //        ws.Range("B7:C8").Column(1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //        ws.Cell("C7").Value = "Tổng số kiến nghị";
        //        ws.Range("C7:D8").Column(1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //        ws.Cell("D7").Value = "Tổng số KN đã trả lời";
        //        ws.Range("D7:E8").Column(1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //        ws.Cell("E7").Value = "Kết quả giải quyết";
        //        ws.Range("E7:G8").Column(1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        ws.Cell("E8").Value = "Đã giải quyết xong";
        //        ws.Cell("E9").Value = "Đang quyết xong";
        //        ws.Cell("E10").Value = "Giải trình thông tin";

        //        ws.Cell("A9").Value = "I. Chính phủ, các bộ, cơ quan ngang bộ";
        //        ws.Range("A9:G10").Row(1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //        //excel.Columns.Add("STT", typeof(int));
        //        //excel.Columns.Add("Tên cơ quan, đơn vị ", typeof(string));
        //        //excel.Columns.Add("Tổng số kiến nghị", typeof(int));
        //        //excel.Columns.Add("Tổng số KN đã trả lời", typeof(int));
        //        //excel.Columns.Add("Kết quả giải quyết", typeof(int));
        //        //excel.Columns.Add("Đã giải quyết xong", typeof(int));
        //        //excel.Columns.Add("Đang giải quyết", typeof(int));
        //        //excel.Columns.Add("Giải trình, thông tin", typeof(int));

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            excel.Rows.Add(dt.Rows[i][0], dt.Rows[i][1], dt.Rows[i][2], dt.Rows[i][3], dt.Rows[i][4], dt.Rows[i][5], dt.Rows[i][6]);
        //        }
        //        ws.Worksheets.Add(excel);
        //   // }
        //    return ws;
        //}

        public string Option_LinhvucconquanHuyen_Chonnhieu(List<LINHVUC_COQUAN> linhvuc, string lstNV = "")
        {
            string str = "";
            string[] arrLstLV = lstNV.Split(',');
            var lstParent = linhvuc.Where(x => x.IHIENTHI == 1 && x.IPARENT == 0 && x.IDELETE == 0).OrderBy(v => v.IVITRI).ToList();
            foreach (var p in lstParent)
            {
                str += "<option" + " value='" + p.ILINHVUC + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
    }
}
