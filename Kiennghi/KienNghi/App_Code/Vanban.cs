using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Models;
using Entities.Objects;
using DataAccess.Dao;
using Utilities;
using DataAccess.Busineess;
using PagedList;
namespace KienNghi.App_Code
{
    public class Vanban : Base
    {

        VanbanBusineess _vanban = new  VanbanBusineess();
        ThietlapBusineess _thietlap = new ThietlapBusineess();
        TiepdanBusineess _tiepdan = new TiepdanBusineess();
        Dictionary<string, object> _condition;
        Funtions func = new Funtions();
        public string File_View(int id)
        {
            /// FileuploadRepository _file = new FileuploadRepository();
            string str = "";
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("IVANBAN", id);
            string url_cookie = func.Get_Url_keycookie();
            var file = _vanban.GetBy_List_Vanban_file(_dic).ToList();
            if (file.Count() > 0)
            {
                str += "";
                foreach (var f in file)
                {
                    string id_encr = HashUtil.Encode_ID(f.IFILE.ToString(), url_cookie);
                    str += "<p> <a href='/VanBan/DownLoad/" + id_encr + "' class='trans_func'><i class='icon-download-alt'></i> </a></p>";
                }
                str += "";
            }

            return str;
        }
        public string List_Vanban(int iUser,int page)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IHIENTHI", 0);
            var vanban = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATECREATE).ToPagedList(page,pageSize).ToList();
            int total = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATECREATE).ToList().Count();
            int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.IVANBAN.ToString(), url_cookie);
                // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title=''   class='trans_func'  onclick=\"ShowPageLoading()\"><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa','Bạn có chắc xóa Quốc tịch " + HttpUtility.HtmlEncode(x.CTIEUDE) + "  khỏi danh sách?')\"   class='trans_func'><i class='icon-trash'></i></a> ";
                string duyet = " <a href=\"javascript:void()\" data-original-title='Duyệt ban hành văn bản này' rel='tooltip' title='' onclick=\"UpdateTrangthai_Duyet_VanBan('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan','Bạn có muốn duyệt văn bản " + HttpUtility.HtmlEncode(x.CTIEUDE) + "?')\"  class='trans_func' ><i class='icon-ok'></i></a> ";
                //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                // del = "";
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", x.IVANBAN);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                string file = "";
                if(list.Count() != null && list.Count() > 0)
                {
                    file = File_View((int)x.IVANBAN);
                }
                //foreach (var d in list) 
                //{
                //    if(d.CURL !=null)
                //    {
                //        file += "<p><a href='" + d.CURL + "'  class='trans_func'><i class='icon-download-alt'></i> </a></p>";
                //    }
                //}
                if (!IsAdmin(iUser) && !Action(39, iUser))
                {
                    edit = ""; del = "";
                }
                if (!Action(40, iUser) && !IsAdmin(iUser))
                {
                    duyet = "";
                }
                count++;
                string loaivanban = "Chưa xác định";
                if(x.ILOAI != null && _vanban.Get_ListVB().Where(v=>v.ILOAI != null && v.ILOAI == x.ILOAI).Count() > 0)
                {
                    loaivanban = _vanban.GetBy_Loaivanban((int)x.ILOAI).CTEN;
                }
                string linhvuc = "Chưa xác định";
                if(x.ILINHVUC != null && _thietlap.Get_Linhvuc().Where(v=>v.ILINHVUC != null && v.ILINHVUC == x.ILINHVUC).Count() > 0 )
                {
                    linhvuc = _thietlap.GetBy_Linhvuc_CoquanID((int)x.ILINHVUC).CTEN;
                }
                str += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                    "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + HttpUtility.HtmlEncode(x.CTIEUDE) +
                    "</strong></span><span > Vv " + HttpUtility.HtmlEncode(x.CTRICHYEU) +
                    "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                    func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + Row_vanban_list_donvi((int)x.IVANBAN) +
                    "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + duyet + edit + del + "</p></td></tr>";
            }
            str += "<tr><td colspan='4'>" + PhanTrang(total, pageSize, page, "" +
           "/Vanban/Moicapnhat") + "</td></tr>";
            return str;
        }
        public string Row_vanban_list_donvi(int ivanban)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IVANBAN", ivanban);
            var list = _vanban.GetBy_List_Vanban_donvi(_condition).ToList();

            if (list.Count() > 0)
            {
                foreach (var x in list)
                {
                    int iGroup = Convert.ToInt32(x.IDONVI);
                    var thongtincoquan = _thietlap.Get_List_Quochoi_Coquan().Where(v => v.ICOQUAN == x.IDONVI).ToList();
                    if(thongtincoquan.Count() > 0 )
                    {
                        str += _thietlap.GetBy_Quochoi_CoquanID((int)x.IDONVI).CTEN + ",";
                    }
                }
            }
            int lenth = str.Length;
            if (lenth > 0)
            {
                str = str.Substring(0, lenth - 1);
            }

            return str;
        }
        public string Option_Loai(int id_loai = 0)
        {
            string str = "";
            var chucvu = _vanban.Get_Loaivanban().ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.ILOAI == id_loai) { select = " selected "; }
                str += "<option " + select + " value='" + p.ILOAI + "'>" + p.CTEN + "</option>";
            }
            return str;
        }
        public string Option_Donvi(int id)
        {

            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IPARENT", 0);
            var action0 = _thietlap.GetBy_List_Quochoi_Coquan(_condition).OrderBy(x => x.IVITRI).ToList();
            foreach (var a0 in action0)
            {
                str += " <optgroup label='" + a0.CTEN.ToString() + "'>";
                _condition = new Dictionary<string, object>();
                _condition.Add("IPARENT", a0.ICOQUAN);
                var action1 = _thietlap.GetBy_List_Quochoi_Coquan(_condition).OrderBy(x => x.IVITRI).ToList();
                foreach (var a1 in action1)
                {
                    string select = ""; if (a1.ICOQUAN == id) { select = " selected "; }
                    str += "<option " + select + " value=" + a1.ICOQUAN + ">" + a1.CTEN + "</option>";
                }
                str += " </optgroup>";
            }

            return str;
        }
        public string Option_LinhVuc_CoQuan(List<LINHVUC_COQUAN> linhvuc, int id_choice = 0)
        {
            string str = "";
            foreach (var t in linhvuc)
            {
                string select = ""; if (t.ILINHVUC == id_choice) { select = " selected "; }
                str += "<option " + select + " value='" + t.ILINHVUC + "'>" + t.CTEN + "</option>";
            }
            return str;
        }
        public string List_CheckBox_Donvi(int iVanban)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IPARENT", 0);
            var action0 = _thietlap.GetBy_List_Quochoi_Coquan(_condition).OrderBy(x => x.IVITRI).ToList();
            foreach (var a0 in action0)
            {
                str += "<p class='b'>" + a0.CTEN + ":</p><ul class='list-chucnang'>";
                _condition = new Dictionary<string, object>();
                _condition.Add("IPARENT", a0.ICOQUAN);
                var action1 = _thietlap.GetBy_List_Quochoi_Coquan(_condition).OrderBy(x => x.IVITRI).ToList();
                foreach (var a1 in action1)
                {
                    string check = "";
                    string disabled = "";
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IDONVI", a1.ICOQUAN);
                    _condition.Add("IVANBAN", iVanban);
                    if (_vanban.GetBy_List_Vanban_donvi(_condition).ToList().Count() > 0)
                    {
                        check = "checked";
                    }

                    str += "<li><input " + check + disabled + " type='checkbox' name='action' value='" + a1.ICOQUAN + "' onclick='UpdateoptLinhvuc()' /> " + a1.CTEN + "</li>";
                }
                str += "</ul>";
            }

            return str;

        }
        public string List_CheckBox_Donvi2(int iVanban, List<QUOCHOI_COQUAN> coquan,List<VB_DONVI_VANBAN> vanbandonvi)
        {
            string str = "";
            var action0 = coquan.Where(x=>x.IPARENT == 0).Where(x=>x.IDELETE ==0).OrderBy(x => x.IVITRI).ToList();
            foreach (var a0 in action0)
            {
                str += "<p class='b'>" + a0.CTEN + ":</p><ul class='list-chucnang'>";
                _condition = new Dictionary<string, object>();
                _condition.Add("IPARENT", a0.ICOQUAN);
                var action1 = _thietlap.GetBy_List_Quochoi_Coquan(_condition).Where(x=>x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                foreach (var a1 in action1)
                {
                    string check = "";
                    string disabled = "";
                    if (vanbandonvi.Where(x => x.IDONVI == a1.ICOQUAN && x.IVANBAN == iVanban ).ToList().Count() > 0)
                    {
                        check = "checked";
                    }

                    str += "<li><input " + check + disabled + " type='checkbox' name='action' id='action'  value='" + a1.ICOQUAN + "' onchange='UpdateoptLinhvuc()' /> " + a1.CTEN + "</li>";
                }
                str += "</ul>";
            }

            return str;

        }
        public string List_Vanban_Daduyet(int iUser,int page)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IHIENTHI", 1);
            var vanban = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATECREATE).ToPagedList(page,pageSize).ToList();
            int total = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATECREATE).ToList().Count();
            int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.IVANBAN.ToString(), url_cookie);
                // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title='' class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string chuyenvethemmoi = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản mới cập nhật' rel='tooltip' title='' onclick=\"UpdateTrangthai_ChuyenMoi('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_Moi','Bạn có muốn chuyển văn bản về mới cập nhật')\" class='trans_func' ><i class='icon-signout'></i></a> ";
                string chuyenvehethan = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản hết hạn' rel='tooltip' title='' onclick=\"UpdateTrangthai_HetHieuLuc('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_QuaHan','Bạn có muốn chuyển văn bản về quá hạn')\" class='trans_func' ><i class='icon-time'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Hủy' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa')\"  class='trans_func'><i class='icon-trash'></i></a> ";

                //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                // del = "";
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", x.IVANBAN);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                string file = "";
                if (list.Count() != null && list.Count() > 0)
                {
                    file = File_View((int)x.IVANBAN);
                }

                if (!IsAdmin(iUser) && x.IUSERDUYET != iUser)
                {
                    edit = ""; del = "";
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
                str += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                    "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + HttpUtility.HtmlEncode(x.CTIEUDE) +
                    "</strong></span><span > Vv " + HttpUtility.HtmlEncode(x.CTRICHYEU) +
                    "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                    func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + Row_vanban_list_donvi((int)x.IVANBAN) +
                    "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + chuyenvethemmoi + chuyenvehethan + "</p></td></tr>";
            }
            str += "<tr><td colspan='4'>" + PhanTrang(total, pageSize, page, "" +
           "/Vanban/Duyet") + "</td></tr>";
            return str;
        }
        public string List_Vanban_search(int iUser, int id, int dem, string url_cookie,int hienthi)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IHIENTHI", 1);
            _condition.Add("IVANBAN", id);
            var vanban = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATE).ToList();
            int count = 0;
            if (vanban.Count() == 0)
            {
                str = "<tr><td class='alert tcenter alert-danger' colspan=4> Không tìm thấy kết quả nào </td></tr>";
            }
            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.IVANBAN.ToString(), url_cookie);
                // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title='' class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string chuyenvethemmoi = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản mới cập nhật' rel='tooltip' title='' onclick=\"UpdateTrangthai_ChuyenMoi('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_Moi','Bạn có muốn chuyển văn bản về mới cập nhật')\" class='trans_func' ><i class='icon-signout'></i></a> ";
                string chuyenvehethan = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản hết hạn' rel='tooltip' title='' onclick=\"UpdateTrangthai_HetHieuLuc('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_QuaHan','Bạn có muốn chuyển văn bản về quá hạn')\" class='trans_func' ><i class='icon-time'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Hủy' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa')\"  class='trans_func'><i class='icon-trash'></i></a> ";

                //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                // del = "";
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", x.IVANBAN);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                string file = "";
                if (list.Count() != null && list.Count() > 0)
                {
                    file = File_View((int)x.IVANBAN);
                }

                if (!IsAdmin(iUser) && x.IUSERDUYET != iUser)
                {
                    edit = ""; del = "";
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
                str +="<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + dem +
                    "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + HttpUtility.HtmlEncode(x.CTIEUDE) +
                    "</strong></span><span > Vv " + HttpUtility.HtmlEncode(x.CTRICHYEU) +
                    "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                    func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + Row_vanban_list_donvi((int)x.IVANBAN) +
                    "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + chuyenvethemmoi + chuyenvehethan + "</p></td></tr>";
            }
            return str;
        }
        public string List_Vanban_xoanthao_search(int iUser, int id, int dem, string url_cookie, int hienthi)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IHIENTHI", 0);
            _condition.Add("IVANBAN", id);
            var vanban = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATECREATE).ToList();
            int count = 0;
            if (vanban.Count() == 0)
            {
                str = "<tr><td class='alert tcenter alert-danger' colspan=4> Không tìm thấy kết quả nào </td></tr>";
            }
            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.IVANBAN.ToString(), url_cookie);
                // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title=''   class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa','Bạn có muốn xóa văn bản "+HttpUtility.HtmlEncode(x.CTIEUDE)+" khỏi danh sách?')\"   class='trans_func'><i class='icon-trash'></i></a> ";
                string duyet = " <a href=\"javascript:void()\" data-original-title='Duyệt' rel='tooltip' title='' onclick=\"UpdateTrangthai_Duyet_VanBan('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan','Bạn có muốn duyệt văn bản " + HttpUtility.HtmlEncode(x.CTIEUDE) + "?')\"  class='trans_func' ><i class='icon-ok'></i></a> ";
                //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                // del = "";
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", x.IVANBAN);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                string file = "";
                if (list.Count() != null && list.Count() > 0)
                {
                    file = File_View((int)x.IVANBAN);
                }
                if (!Action(39, iUser) && !IsAdmin(iUser))
                {
                    edit = ""; del = "";
                }
                if (!Action(40, iUser) && !IsAdmin(iUser))
                {
                    duyet = "";
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
                str +="<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + dem +
                    "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + HttpUtility.HtmlEncode(x.CTIEUDE) +
                    "</strong></span><span > Vv " + HttpUtility.HtmlEncode(x.CTRICHYEU) +
                    "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                    func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + Row_vanban_list_donvi((int)x.IVANBAN) +
                    "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + duyet + edit + del + "</p></td></tr>";
            }
            return str;
        }
        public string List_Vanban_QuaHan_search(int iUser, int id, int dem, string url_cookie, int hienthi)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IHIENTHI", hienthi);
            _condition.Add("IVANBAN", id);
            var vanban = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATE).ToList();
            int count = 0;
            if(vanban.Count() == 0)
            {
                str = "<tr><td class='alert tcenter alert-danger' colspan=4> Không tìm thấy kết quả nào </td></tr>";
            }
            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.IVANBAN.ToString(), url_cookie);
                // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title='' class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string chuyenvethemmoi = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản mới cập nhật' rel='tooltip' title='' onclick=\"UpdateTrangthai_ChuyenMoi('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_Moi','Bạn có muốn chuyển văn bản về mới cập nhật')\" class='trans_func' ><i class='icon-signout'></i></a> ";
                string chuyenvehethan = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản hết hạn' rel='tooltip' title='' onclick=\"UpdateTrangthai('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_QuaHan','Bạn có muốn chuyển văn bản về quá hạn')\" class='trans_func' ><i class='icon-time'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Hủy' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa')\"  class='trans_func'><i class='icon-trash'></i></a> ";

                //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                // del = "";
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", x.IVANBAN);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                string file = "";
                if (list.Count() != null && list.Count() > 0)
                {
                    file = File_View((int)x.IVANBAN);
                }

                if (!IsAdmin(iUser) && x.IUSERDUYET != iUser)
                {
                    edit = ""; del = "";
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
                str += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + dem +
                    "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + HttpUtility.HtmlEncode(x.CTIEUDE) +
                    "</strong></span><span > Vv " + HttpUtility.HtmlEncode(x.CTRICHYEU) +
                    "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                    func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + Row_vanban_list_donvi((int)x.IVANBAN) +
                    "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td></td></tr>";
            }
            return str;
        }
        public string List_Vanban_Quahan(int iUser,int page)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IHIENTHI", -1);
            var vanban = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATECREATE).ToPagedList(page, pageSize).ToList();
            int total = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATECREATE).ToList().Count();
            int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.IVANBAN.ToString(), url_cookie);
                // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title='' class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string chuyenvethemmoi = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản mới cập nhật' rel='tooltip' title='' onclick=\"UpdateTrangthai_ChuyenMoi('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_Moi','Bạn có muốn chuyển văn bản về mới cập nhật')\" class='trans_func' ><i class='icon-signout'></i></a> ";
                string chuyenvehethan = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản hết hạn' rel='tooltip' title='' onclick=\"UpdateTrangthai('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_QuaHan','Bạn có muốn chuyển văn bản về quá hạn')\" class='trans_func' ><i class='icon-time'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Hủy' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa')\"  class='trans_func'><i class='icon-trash'></i></a> ";

                //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                // del = "";
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", x.IVANBAN);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                string file = "";
                if (list.Count() != null && list.Count() > 0)
                {
                    file = File_View((int)x.IVANBAN);
                }

                if (!IsAdmin(iUser) && x.IUSERDUYET != iUser)
                {
                    edit = ""; del = "";
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
                str += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                    "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + HttpUtility.HtmlEncode(x.CTIEUDE) +
                    "</strong></span><span > Vv " + HttpUtility.HtmlEncode(x.CTRICHYEU) +
                    "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                    func.ConvertDateVN(x.DDATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + Row_vanban_list_donvi((int)x.IVANBAN) +
                    "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td></td></tr>";
            }
            str += "<tr><td colspan='4'>" + PhanTrang(total, pageSize, page, "" +
          "/Vanban/Quahan") + "</td></tr>";
            return str;
        }
        public string Option_LinhVuc_Coquan(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, string url_cookie = "", int id = 0)
        {
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var donvi = coquan.Where(x => x.IPARENT == id_parent && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            string sel = "";
            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {
                    if (t.ICOQUAN == id)
                    {
                        sel = "selected";
                    }
                    if (t.IPARENT == 0)
                    {
                        str += "<optgroup  label='" + t.CTEN + "' ></optgroup>";
                    }
                    else
                    {
                        str += "<option " + sel + " value='" + t.ICOQUAN + "'>" + space_level + t.CTEN + "</option>";
                    }
               


                    str += Option_LinhVuc_Coquan(coquan, (int)t.ICOQUAN, level + 1, url_cookie, id);
                    sel = "";
                }
            }
            return str;
        }


        public string LIST_MOICAPNHAT(List<PHANTRANG_VANBAN> vanban, int iUser = 0)
        {
            string str = "";
            
           // int total = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATECREATE).ToList().Count();
            int count = 0;
            decimal kiemtra = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.IDVANBAN.ToString(), url_cookie);
                // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title=''   class='trans_func'  onclick=\"ShowPageLoading()\"><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa','Bạn có chắc xóa Quốc tịch " + HttpUtility.HtmlEncode(x.TIEUDE) + "  khỏi danh sách?')\"   class='trans_func'><i class='icon-trash'></i></a> ";
                string duyet = " <a href=\"javascript:void()\" data-original-title='Duyệt ban hành văn bản này' rel='tooltip' title='' onclick=\"UpdateTrangthai_Duyet_VanBan('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan','Bạn có muốn duyệt văn bản " + HttpUtility.HtmlEncode(x.TIEUDE) + "?')\"  class='trans_func' ><i class='icon-ok'></i></a> ";
                string chuyenvethemmoi = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản mới cập nhật' rel='tooltip' title='' onclick=\"UpdateTrangthai_ChuyenMoi('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_Moi','Bạn có muốn chuyển văn bản về mới cập nhật')\" class='trans_func' ><i class='icon-signout'></i></a> ";
                string chuyenvehethan = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản hết hạn' rel='tooltip' title='' onclick=\"UpdateTrangthai_HetHieuLuc('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_QuaHan','Bạn có muốn chuyển văn bản về quá hạn')\" class='trans_func' ><i class='icon-time'></i></a> ";
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", x.IDVANBAN);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                string file = "";
                if (list.Count() != null && list.Count() > 0)
                {
                    file = File_View((int)x.IDVANBAN);
                }
               
                if (!IsAdmin(iUser) && !Action(39, iUser))
                {
                    edit = ""; del = "";
                }
                if (!Action(40, iUser) && !IsAdmin(iUser))
                {
                    duyet = "";
                }
               
                string loaivanban = "Chưa xác định";
                if (x.TENLOAIVB != "")
                {
                    loaivanban = x.TENLOAIVB;
                }
                string linhvuc = "Chưa xác định";
                if (x.TENLINHVUC != "")
                {
                    linhvuc = x.TENLINHVUC;
                }
                if (kiemtra != x.IDVANBAN)
                {
                    count++;
                    kiemtra = x.IDVANBAN;
                    str += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                                       "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + HttpUtility.HtmlEncode(x.TIEUDE) +
                                       "</strong></span><span > Vv " + HttpUtility.HtmlEncode(x.TRICHYEU) +
                                       "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                                       func.ConvertDateVN(x.DATECREATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + Row_vanban_list_donvi((int)x.IDVANBAN) +
                                       "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + duyet + edit + del + "</p></td></tr>";
                }
               
            }
            return str;
        }
        public string LIST_VANBANDADUYET(List<PHANTRANG_VANBAN> vanban, int iUser = 0)
        {
            string str = "";
            int count = 0;
            decimal kiemtra = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.IDVANBAN.ToString(), url_cookie);
                // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
              //  string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title=''   class='trans_func'  onclick=\"ShowPageLoading()\"><i class='icon-pencil'></i></a> ";
               // string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa','Bạn có chắc xóa Quốc tịch " + HttpUtility.HtmlEncode(x.TIEUDE) + "  khỏi danh sách?')\"   class='trans_func'><i class='icon-trash'></i></a> ";
               // string duyet = " <a href=\"javascript:void()\" data-original-title='Duyệt ban hành văn bản này' rel='tooltip' title='' onclick=\"UpdateTrangthai_Duyet_VanBan('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan','Bạn có muốn duyệt văn bản " + HttpUtility.HtmlEncode(x.TIEUDE) + "?')\"  class='trans_func' ><i class='icon-ok'></i></a> ";
                string chuyenvethemmoi = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản mới cập nhật' rel='tooltip' title='' onclick=\"UpdateTrangthai_ChuyenMoi('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_Moi','Bạn có muốn chuyển văn bản về mới cập nhật')\" class='trans_func' ><i class='icon-signout'></i></a> ";
                string chuyenvehethan = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản hết hạn' rel='tooltip' title='' onclick=\"UpdateTrangthai_HetHieuLuc('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_QuaHan','Bạn có muốn chuyển văn bản về quá hạn')\" class='trans_func' ><i class='icon-time'></i></a> ";
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", x.IDVANBAN);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                string file = "";
                if (list.Count() != null && list.Count() > 0)
                {
                    file = File_View((int)x.IDVANBAN);
                }
                string loaivanban = "Chưa xác định";
                if (x.TENLOAIVB != "")
                {
                    loaivanban = x.TENLOAIVB;
                }
                string linhvuc = "Chưa xác định";
                if (x.TENLINHVUC != "")
                {
                    linhvuc = x.TENLINHVUC;
                }
                if(kiemtra != x.IDVANBAN)
                {
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                  "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + HttpUtility.HtmlEncode(x.TIEUDE) +
                  "</strong></span><span > Vv " + HttpUtility.HtmlEncode(x.TRICHYEU) +
                  "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                  func.ConvertDateVN(x.DATECREATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + Row_vanban_list_donvi((int)x.IDVANBAN) +
                  "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + chuyenvethemmoi + chuyenvehethan + "</p></td></tr>";
                }
              
            }
           
            return str;
        }
        public string LIST_QUAHAN(List<PHANTRANG_VANBAN> vanban, int iUser = 0)
        {
            string str = "";
            int count = 0;
            decimal kiemtra = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.IDVANBAN.ToString(), url_cookie);
                // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
              //  string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title=''   class='trans_func'  onclick=\"ShowPageLoading()\"><i class='icon-pencil'></i></a> ";
               // string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa','Bạn có chắc xóa Quốc tịch " + HttpUtility.HtmlEncode(x.TIEUDE) + "  khỏi danh sách?')\"   class='trans_func'><i class='icon-trash'></i></a> ";
               // string duyet = " <a href=\"javascript:void()\" data-original-title='Duyệt ban hành văn bản này' rel='tooltip' title='' onclick=\"UpdateTrangthai_Duyet_VanBan('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan','Bạn có muốn duyệt văn bản " + HttpUtility.HtmlEncode(x.TIEUDE) + "?')\"  class='trans_func' ><i class='icon-ok'></i></a> ";
                string chuyenvethemmoi = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản mới cập nhật' rel='tooltip' title='' onclick=\"UpdateTrangthai_ChuyenMoi('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_Moi','Bạn có muốn chuyển văn bản về mới cập nhật')\" class='trans_func' ><i class='icon-signout'></i></a> ";
                string chuyenvehethan = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản hết hạn' rel='tooltip' title='' onclick=\"UpdateTrangthai_HetHieuLuc('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_QuaHan','Bạn có muốn chuyển văn bản về quá hạn')\" class='trans_func' ><i class='icon-time'></i></a> ";
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", x.IDVANBAN);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                string file = "";
                if (list.Count() != null && list.Count() > 0)
                {
                    file = File_View((int)x.IDVANBAN);
                }
                string loaivanban = "Chưa xác định";
                if (x.TENLOAIVB != "")
                {
                    loaivanban = x.TENLOAIVB;
                }
                string linhvuc = "Chưa xác định";
                if (x.TENLINHVUC != "")
                {
                    linhvuc = x.TENLINHVUC;
                }
                if (kiemtra != x.IDVANBAN)
                {
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                        "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + HttpUtility.HtmlEncode(x.TIEUDE) +
                        "</strong></span><span > Vv " + HttpUtility.HtmlEncode(x.TRICHYEU) +
                        "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                        func.ConvertDateVN(x.DATECREATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + Row_vanban_list_donvi((int)x.IDVANBAN) +
                        "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td></td></tr>";
                }
            }
           
            return str;
        }
        public string LIST_TRACUU(List<PHANTRANG_VANBAN> vanban, int iUser = 0)
        {
            string str = "";

            // int total = _vanban.GetBy_List_Vanban(_condition).OrderByDescending(x => x.DDATECREATE).ToList().Count();
            int count = 0;
            decimal kiemtra = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.IDVANBAN.ToString(), url_cookie);
                // string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.i)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                string edit = " <a href=\"/Vanban/Chinhsua?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title=''   class='trans_func'  onclick=\"ShowPageLoading()\"><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Vanban/Xoa','Bạn có chắc xóa Quốc tịch " + HttpUtility.HtmlEncode(x.TIEUDE) + "  khỏi danh sách?')\"   class='trans_func'><i class='icon-trash'></i></a> ";
                string duyet = " <a href=\"javascript:void()\" data-original-title='Duyệt ban hành văn bản này' rel='tooltip' title='' onclick=\"UpdateTrangthai_Duyet_VanBan('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan','Bạn có muốn duyệt văn bản " + HttpUtility.HtmlEncode(x.TIEUDE) + "?')\"  class='trans_func' ><i class='icon-ok'></i></a> ";
                string chuyenvethemmoi = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản mới cập nhật' rel='tooltip' title='' onclick=\"UpdateTrangthai_ChuyenMoi('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_Moi','Bạn có muốn chuyển văn bản về mới cập nhật')\" class='trans_func' ><i class='icon-signout'></i></a> ";
                string chuyenvehethan = " <a href=\"javascript:void()\" data-original-title='Chuyển về văn bản hết hạn' rel='tooltip' title='' onclick=\"UpdateTrangthai_HetHieuLuc('" + id_encr + "','id=" + id_encr + "','/Vanban/DuyetVanBan_QuaHan','Bạn có muốn chuyển văn bản về quá hạn')\" class='trans_func' ><i class='icon-time'></i></a> ";
                _condition = new Dictionary<string, object>();
                _condition.Add("IVANBAN", x.IDVANBAN);
                var list = _vanban.GetBy_List_Vanban_file(_condition).ToList();
                string file = "";
                if (list.Count() != null && list.Count() > 0)
                {
                    file = File_View((int)x.IDVANBAN);
                }

                if (!IsAdmin(iUser) && !Action(39, iUser))
                {
                    edit = ""; del = "";
                }
                if (!Action(40, iUser) && !IsAdmin(iUser))
                {
                    duyet = "";
                }
                if(x.HIENTHI == 0)
                {
                    chuyenvehethan = "";
                    chuyenvethemmoi = "";
                }
                else if(x.HIENTHI == 1)
                {
                    edit = "";
                    del = "";
                    duyet = "";
                }
                else
                {
                    chuyenvehethan = "";
                    chuyenvethemmoi = "";
                    edit = "";
                    del = "";
                    duyet = "";
                }
                string loaivanban = "Chưa xác định";
                if (x.TENLOAIVB != "")
                {
                    loaivanban = x.TENLOAIVB;
                }
                string linhvuc = "Chưa xác định";
                if (x.TENLINHVUC != "")
                {
                    linhvuc = x.TENLINHVUC;
                }
                if (kiemtra != x.IDVANBAN)
                {
                    count++;
                    kiemtra = x.IDVANBAN;
                    str += "<tr id='tr_" + id_encr + "'><td class='b tcenter'>" + count +
                                       "</td><td class=''><span class=\"name\">" + loaivanban + " số <strong> " + HttpUtility.HtmlEncode(x.TIEUDE) +
                                       "</strong></span><span > Vv " + HttpUtility.HtmlEncode(x.TRICHYEU) +
                                       "</span><br/><span><strong>Ngày ban hành: </strong><span class='f-orangered'>" +
                                       func.ConvertDateVN(x.DATECREATE.ToString()) + "</span>; <strong> Đơn vị ban hành: </strong><span class='f-green'>" + Row_vanban_list_donvi((int)x.IDVANBAN) +
                                       "</span>; <strong> Lĩnh vực: </strong><span class='f-green'>" + linhvuc + "</span></p></td><td class='tcenter'>" + file + "</td><td class='tcenter'><p>" + duyet + edit + del +chuyenvethemmoi +chuyenvehethan+"</p></td></tr>";
                }

            }
            return str;
        }
    }
}
