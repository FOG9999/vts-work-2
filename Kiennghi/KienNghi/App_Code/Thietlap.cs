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
using System.Text;
using KienNghi.ViewModel;
using KienNghi.Helper;
using System.Web.Mvc;
using System.Data;

namespace KienNghi.App_Code
{
    public class Thietlap : Base
    {

        ThietlapBusineess _thietlap = new ThietlapBusineess();
        Dictionary<string, object> _condition;
        Funtions func = new Funtions();
        Dictionary<string, object> _dCondition;
        Thietlaplist thietlaplist = new Thietlaplist();
        /// <summary>
        /// Danh mục khóa quốc hội
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int IDDonVi_User(int iUser)
        {
            return (int)_thietlap.Get_User(iUser).IDONVI;
        }
        public string GetName_KyHop_KhoaHop(int iKyHop)
        {
            string str = "";
            var thongtinkyhop = _thietlap.Get_List_Quochoi_Kyhop().Where(x => x.IKYHOP == iKyHop).ToList();
            if (thongtinkyhop.Count() > 0)
            {
                QUOCHOI_KYHOP kyhop = _thietlap.Get_Quochoi_Kyhop(iKyHop);
                var thongtinkhoa = _thietlap.Get_List_Quochoi_Khoa().Where(x => x.IKHOA == kyhop.IKHOA).ToList();
                if (thongtinkhoa.Count() > 0)
                {
                    QUOCHOI_KHOA khoa = _thietlap.Get_Quochoi_Khoa((int)kyhop.IKHOA);
                    str = "<strong>" + kyhop.CTEN + "</strong></br>" + khoa.CTEN;
                }
            }
            return str;
        }
        public string List_CheckBox_action_choice(string arr, TaikhoanAtion act)
        {
            //  User_ActionRepository _user_action = new User_ActionRepository();
            string str = "";

            _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IPARENT", 0);

            var action0 = _thietlap.GetBy_List_Action(_dCondition).OrderBy(x => x.IVITRI).ToList();
            foreach (var a0 in action0)
            {
                str += "<p class=''>" + a0.CTEN + ":</p><ul class='list-chucnang'>";
                _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IPARENT", a0.IACTION);
                var action1 = _thietlap.GetBy_List_Action(_dCondition).OrderBy(x => x.IVITRI).ToList();
                string action_user_login = act.list_action;
                foreach (var a1 in action1)
                {
                    string check = "";
                    string disabled = "";

                    if (arr != "" && _thietlap.Get_SQL_KIEMTRA(arr, (int)a1.IACTION).Count() > 0)
                    {
                        check = "checked ";
                    }
                    if (!act.is_admin)
                    {

                        if (action_user_login.IndexOf("|" + a1.IACTION + "|") == -1)
                        {
                            disabled = "disabled ";
                        }
                    }
                    if (arr == "")
                    {
                        disabled = "disabled ";
                    }
                    str += "<li><input " + check + disabled + " type='checkbox' name='action' value='" + a1.IACTION + "' /> " + a1.CTEN + "</li>";
                }
                str += "</ul>";
            }
            return str;
        }
        public string Option_ChucVu(int id_chucvu = 0)
        {
            string str = "";

            var chucvu = _thietlap.Get_List_User_Chucvu().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.ICHUCVU == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.ICHUCVU + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_ChucVu_TheoPhongBan(int iPhongBan = 0,int id_chucvu = 0)
        {
            string str = "";
            Dictionary<string, object> condition = new Dictionary<string, object>();
            if (iPhongBan != 0)
                condition.Add("IPHONGBAN", iPhongBan);
            var chucvu = _thietlap.Get_List_User_Chucvu_By_Conditions(condition).Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.ICHUCVU == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.ICHUCVU + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_Taikhoan_Type(int id_chucvu, TaikhoanAtion act)
        {
            string str = "";

            var chucvu = _thietlap.Get_Type().OrderBy(x => x.CNAME).ToList();
            if (act.is_admin)
            {
                foreach (var p in chucvu)
                {
                    string select = ""; if (p.ITYPE == id_chucvu) { select = " selected "; }
                    str += "<option " + select + " value='" + p.ITYPE + "'>" + p.CNAME + "</option>";
                }
            }
            else
            {

                var type_nguoidung = chucvu.Where(x => x.ITYPE == 2).FirstOrDefault();
                string select = ""; if (type_nguoidung.ITYPE == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + type_nguoidung.ITYPE + "'>" + type_nguoidung.CNAME + "</option>";
            }

            return str;
        }
        public string List_CheckBox_NhomQuyen(int iNhom = 0)
        {


            string str = "";
            _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IPARENT", 0);
            var action0 = _thietlap.GetBy_List_Action().Where(x => x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var a0 in action0)
            {
                str += "<p class=''>" + a0.CTEN + ":</p><ul class='list-chucnang'>";
                _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IPARENT", a0.IACTION);
                var action1 = _thietlap.GetBy_List_Action(_dCondition).OrderBy(x => x.IVITRI).ToList();
                foreach (var a1 in action1)
                {
                    string check = "";
                    _dCondition = new Dictionary<string, object>();
                    _dCondition.Add("IACTION", a1.IACTION);
                    _dCondition.Add("IGROUP", iNhom);
                    if (_thietlap.Get_user_Group_Action(_dCondition).ToList().Count() > 0)
                    {
                        check = "checked";
                    }
                    str += "<li><input " + check + " type='checkbox' name='action' value='" + a1.IACTION + "' id='action" + a1.IACTION + "' /> <a href=\"javascript:void()\" data-original-title='Chọn chức năng' onclick=\"Checkboxchucnang(" + a1.IACTION + ")\" rel='tooltip' title=''  style='color:black'>" + HttpUtility.HtmlEncode(a1.CTEN) + "</a></li>";
                }
                str += "</ul>";
            }
            return str;
        }
        public string Option_Vanban(int iUser)
        {
            string str = "";

            var vanban = _thietlap.Get_Loaivanban().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            int count = 0;

            foreach (var x in vanban)
            {
                string id_encr = HashUtil.Encode_ID(x.ILOAI.ToString());


                str += "<option value='" + id_encr + "' > - - - " + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
            }
            return str;
        }

        public string OptionKhoa_Search(string url_cookie)
        {
            string str = "";
            var khoa = _thietlap.Get_List_Quochoi_Khoa().Where(x => x.IDELETE == 0).OrderByDescending(x => x.DBATDAU).OrderBy(x => x.IVITRI).ToList();
            foreach (var k in khoa)
            {
                string id_encr = HashUtil.Encode_ID(k.IKHOA.ToString(), url_cookie);
                str += "<option value='" + id_encr + "'>" + HttpUtility.HtmlEncode(k.CTEN) + "</option>";
            }
            return str;

        }

        public string List_Khoa(List<QUOCHOI_KHOA> khoa, int iKhoa = 0, string url_cookie = "")
        {
            string str = "";
            khoa = khoa.Where(x => x.IDELETE == 0).OrderByDescending(x => x.DBATDAU).ToList();
            if (iKhoa != 0)
            {
                khoa = khoa.Where(x => x.IKHOA == iKhoa && x.IDELETE == 0).ToList();
            }
            int count = 1;

            foreach (var c in khoa)
            {
                string id_encr = HashUtil.Encode_ID(c.IKHOA.ToString(), url_cookie);
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Khoa_order')\" type=\"text\" value='" + c.IVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Khoa_edit')\" rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                //string add_kyhop = " <a href=\"javascript:void()\" data-original-title='Thêm kỳ họp' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Kyhop_add')\" rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Khoa('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Khoa_del','Bạn có chắc xóa khóa họp " + HttpUtility.HtmlEncode(c.CTEN) + " khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(c.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Khoa_status')\"/>";
                string chon = "<a id='btn_" + id_encr + "'  data-original-title='Chọn' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonKhoaHop('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Khoa_macdinh')\" class='chontrung f-grey'><i class='icon-ok-sign'></i></a>";
                if (c.IMACDINH == 1)
                {
                    chon = "<a id='btn_" + id_encr + "'  data-original-title='Chọn' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonKhoaHop('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Khoa_macdinh')\" class='trans_func chontrung'><i class='icon-ok-sign'></i></a>";
                }
                DateTime tempDate;
                int YBATDAU = 0;
                if (c.DBATDAU != null)
                {
                    tempDate = (DateTime)c.DBATDAU;
                    YBATDAU = tempDate.Year;
                }
                int YKETHUC = 0;
                if (c.DKETTHUC != null)
                {
                    tempDate = (DateTime)c.DKETTHUC;
                    YKETHUC = tempDate.Year;
                }
                str += "<tr><td class='tcenter '>" + count + "</td><td class='tcenter f-red'>" + HttpUtility.HtmlEncode(c.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(c.CTEN) +
                    "</td><td class='tcenter '>" + YBATDAU + "</td><td class='tcenter '>" + YKETHUC + "</td>" +
                    "<td class='tcenter'>" + chon + "</td><td class='tcenter '>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                count++;

            }
            return str;
        }
        //public string OptionLinhVuc_ThietLap(int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        //{
        //    LinhvucRepository _linhvuc = new LinhvucRepository();
        //    string str = "";
        //    string space_level = "";
        //    for (int i = 0; i < level; i++)
        //    {
        //        space_level += "- - - ";
        //    }
        //    _dCondition = new Dictionary<string, object>();
        //    _dCondition.Add("IPARENT", id_parent);

        //    var list = _linhvuc.GetAll(_dCondition).Where(x => x.ILINHVUC != id_donvi_choice).ToList();
        //    foreach (var donvi in list)
        //    {
        //        string select = "";
        //        if (donvi.ILINHVUC == id_donvi) { select = " selected "; }
        //        str += "<option " + select + " value='" + donvi.ILINHVUC + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
        //        _dCondition = new Dictionary<string, object>();
        //        _dCondition.Add("IPARENT", donvi.ILINHVUC);
        //        var kiemtra = _linhvuc.GetAll(_dCondition).ToList();
        //        if (kiemtra.Count() > 0)
        //        {
        //            str += OptionLinhVuc_ThietLap((int)donvi.ILINHVUC, level + 1, id_donvi, id_donvi_choice);
        //        }

        //    }
        //    return str;
        //}
        public string OptionCoQuan_TaiKhoan(int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {


            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            //string sql = " select * from QUOCHOI_COQUAN where IPARENT = " + id_parent + " and ICOQUAN <> " + id_donvi_choice + "";
            _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IPARENT", id_parent);
            var list = _thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Where(x => x.ICOQUAN != id_donvi_choice).OrderBy(x => x.IVITRI).ToList();

            foreach (var donvi in list)
            {
                //  string sql2 = " select * from USERS where IDONVI = " + donvi.ICOQUAN + " and ITYPE <> " + -1 + "";
                _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IDONVI", (int)donvi.ICOQUAN);
                int count_user = _thietlap.GetBy_List_Taikhoan(_dCondition).Where(x => x.ITYPE != -1).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList().Count();
                string select = "";
                if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                if (count_user > 0)
                {

                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + " (" + count_user + ")</option>";
                }
                else
                {
                    _dCondition = new Dictionary<string, object>();
                    _dCondition.Add("IPARENT", donvi.ICOQUAN);
                    if (_thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Count() > 0)
                    {
                        str += "<optgroup label='" + HttpUtility.HtmlEncode(donvi.CTEN) + "'>";
                        str += OptionCoQuan_TaiKhoan((int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                        str += "</optgroup>";
                    }
                }


            }
            return str;
        }
        public string Option_PhongBan_ByDonVi(int id_donvi = 0, int id_phongban = 0)
        {
            string str = "";
            _dCondition = new Dictionary<string, object>();
            
            var donvi = _thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            if (id_donvi != 0)
            {
                donvi = donvi.Where(x => x.ICOQUAN == id_donvi).ToList();
            }
            foreach (var p in donvi)
            {
                _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IDONVI", p.ICOQUAN);
                _dCondition.Add("IHIENTHI", 1);
                var phongban = _thietlap.GetBy_List_Phongban(_dCondition).Where(x => x.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
                if (phongban.Count() > 0)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(p.CTEN) + "'>";
                    foreach (var d in phongban)
                    {
                        string select = ""; if (d.IPHONGBAN == id_phongban) { select = " selected "; }
                        str += "<option " + select + " value='" + d.IPHONGBAN + "'>" + HttpUtility.HtmlEncode(d.CTEN) + "</option>";
                    }
                    str += "</optgroup>";
                }

            }
            return str;
        }

        public string Option_To_Dai_Bieu(int id_phongban = 0)
        {
            string str = "";
            List<USER_PHONGBAN> pbList = _thietlap.GetToDaiBieuList();

            foreach (var pb in pbList)
            {
                string select = ""; if (pb.IPHONGBAN == id_phongban) { select = " selected "; }
                str += "<option " + select + " value='" + pb.IPHONGBAN + "'>" + HttpUtility.HtmlEncode(pb.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_DonVi_PhongBanTree(decimal id_phongban = 0)
        {
            string str = "";
            _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IDELETE", 0);
            _dCondition.Add("IHIENTHI", 1);
            List<QUOCHOI_COQUAN> donViList = _thietlap.GetBy_List_Quochoi_Coquan(_dCondition).OrderBy(x => x.IVITRI).ToList();
            List<USER_PHONGBAN> phongBanList = _thietlap.Get_List_Phongban().Where(x => x.IDELETE == 0 && x.IHIENTHI == 1).OrderBy(v => v.IPARENT).ToList();
            List<USER_PHONGBAN> phongBanByDonVi = new List<USER_PHONGBAN>();
            List<decimal> iDList = new List<decimal>();
            int level = 0;
            foreach (var dv in donViList)
            {
                phongBanByDonVi = phongBanList.Where(x => x.IDONVI == dv.ICOQUAN).OrderBy(x => x.IPARENT).ToList();

                if (phongBanByDonVi.Count() > 0)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(dv.CTEN) + "'>";
                    foreach (var pb in phongBanByDonVi)
                    {
                        if (iDList.Contains(pb.IPHONGBAN)) continue;
                        string select = ""; if (pb.IPHONGBAN == id_phongban) { select = " selected "; }
                        str += "<option " + select + " value='" + pb.IPHONGBAN + "'>" + HttpUtility.HtmlEncode(pb.CTEN) + "</option>";
                        str += Recusive_Option_DonVi_PhongBanTree(ref iDList, phongBanList, pb.IPHONGBAN, id_phongban, level);
                    }
                    str += "</optgroup>";
                }
            }
            return str;
        }

        public string Recusive_Option_DonVi_PhongBanTree(ref List<decimal> iDList, List<USER_PHONGBAN> phongBanList, decimal id_phongbancha, decimal id_phongban_select, int level)
        {
            string str = "";
            var phongban = phongBanList.Where(x => x.IPARENT == id_phongbancha).ToList();
            if (phongban.Count() > 0)
            {
                level++;
                foreach (var pb in phongban)
                {
                    iDList.Add(pb.IPHONGBAN);
                    string select = ""; if (pb.IPHONGBAN == id_phongban_select) { select = " selected "; }
                    str += "<option " + select + " value='" + pb.IPHONGBAN + "' data-idonvi='" + pb.IDONVI + "'>" + string.Concat(Enumerable.Repeat("- ", level)) + HttpUtility.HtmlEncode(pb.CTEN) + "</option>";
                    str += Recusive_Option_DonVi_PhongBanTree(ref iDList, phongBanList, pb.IPHONGBAN, id_phongban_select, level);
                }
            }
            return str;
        }

        public string Option_PhongBanChaCon(int id_phongban = 0)
        {
            string str = "";
            List<USER_PHONGBAN> phongBanList = _thietlap.Get_List_Phongban().Where(x => x.IDELETE == 0 && x.IHIENTHI == 1).OrderBy(x => x.IPARENT).ToList();
            List<decimal> iDList = new List<decimal>();
            int level = 0;
            foreach (var pb in phongBanList)
            {
                if (iDList.Contains(pb.IPHONGBAN)) continue;
                string select = ""; if (pb.IPHONGBAN == id_phongban) { select = " selected "; }
                str += "<option " + select + " value='" + pb.IPHONGBAN + "' data-idonvi='" + pb.IDONVI + "'>" + HttpUtility.HtmlEncode(pb.CTEN) + "</option>";
                str += Recusive_Option_DonVi_PhongBanTree(ref iDList, phongBanList, pb.IPHONGBAN, id_phongban, level);
            }
            return str;
        }

        public string Option_PhongBan_ByDonVi_TK(int id_donvi = 0, int id_phongban = 0)
        {
            string str = "";
            //_dCondition = new Dictionary<string, object>();
            //_dCondition.Add("IGROUP", 0);
            //var donvi = _thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Where(x => x.IDELETE == 0).OrderBy(t => t.IVITRI).OrderBy(x => x.IVITRI).ToList();
            //if (id_donvi != 0)
            //{
            //    donvi = donvi.Where(x => x.ICOQUAN == id_donvi).ToList();
            //}
            //foreach (var p in donvi)
            //{
                _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IDONVI", id_donvi);
                _dCondition.Add("IHIENTHI", 1);
                var phongban = _thietlap.GetBy_List_Phongban(_dCondition).Where(x => x.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
                if (phongban.Count() > 0)
                {
                   // str += "<optgroup label='" + HttpUtility.HtmlEncode() + "'>";
                    foreach (var d in phongban)
                    {
                        string select = ""; if (d.IPHONGBAN == id_phongban) { select = " selected "; }
                        str += "<option " + select + " value='" + d.IPHONGBAN + "'>" + HttpUtility.HtmlEncode(d.CTEN) + "</option>";
                    }
                   // str += "</optgroup>";
                }

            //}
            return str;
        }
        public string OptionCoQuan(int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {

            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IPARENT", id_parent);

            var list = _thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Where(x => x.ICOQUAN != id_donvi_choice && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var donvi in list)
            {
                _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IPARENT", donvi.ICOQUAN);
                _dCondition.Add("IDELETE", 0);
                if (_thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Count() > 0)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(donvi.CTEN) + "'>";
                    str += OptionCoQuan((int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                }

            }
            return str;
        }
        public string OptionCoQuan_TK(int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            
                _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IPARENT", id_parent);

                var list = _thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Where(x => x.ICOQUAN != id_donvi_choice && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                foreach (var donvi in list)
                {
                    if (level < 2)
                    {
                        _dCondition = new Dictionary<string, object>();
                        _dCondition.Add("IPARENT", donvi.ICOQUAN);
                        _dCondition.Add("IDELETE", 0);
                        if (_thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Count() > 0)
                        {
                            str += "<optgroup label='" + HttpUtility.HtmlEncode(donvi.CTEN) + "'>";
                            str += OptionCoQuan_TK((int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                            str += "</optgroup>";
                        }
                        else
                        {
                            string select = "";
                            if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                            str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                        }
                    }

                }
            
            return str;
        }


        public string OptionCoQuan_TreeList(int id_donvi_choice = 0)
        {
            StringBuilder htmlOptions = new StringBuilder();
            var listCoQuans = _thietlap.GetQuocHoiCoQuanTreeList();
            var listParentIds = listCoQuans.Select(x => x.IPARENT.Value).Distinct();
            bool isFirstOptGroup = true;
            foreach(var donvi in listCoQuans)
            {
                if (donvi == null) continue;
                if (listParentIds.Contains(donvi.ICOQUAN))
                {
                    if (isFirstOptGroup)
                    {
                        isFirstOptGroup = false;
                    }
                    else
                    {
                        htmlOptions.Append("</optgroup>");
                    }
                    htmlOptions.Append("<optgroup label='" + HttpUtility.HtmlEncode(donvi.CTEN) + "'>");
                }
                else
                {
                    string select = "";
                    if (id_donvi_choice != 0 && donvi.ICOQUAN == id_donvi_choice) { select = " selected "; }
                    htmlOptions.Append("<option " + select + " value='" + donvi.ICOQUAN + "'>" + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>");
                }
            }
            return htmlOptions.ToString();
        }

        /* Trả về string chứa tất cả lĩnh vực đang có trong DB (đã chuẩn hoá để hiển thị dưới dang List trong HTML)
         */
        public string OptionLinhVucCha_TreeList(int id_donvi_choice = 0)
        {
            StringBuilder htmlOptions = new StringBuilder();
            var listLinhVucCoQuans = _thietlap.Get_Linhvuc_Coquan();
            var listParentIds = listLinhVucCoQuans.Select(x => x.IPARENT).Distinct();
            bool isFirstOptGroup = true;
            foreach (var linhVucCoQuan in listLinhVucCoQuans)
            {
                if (linhVucCoQuan == null) continue;
                if (listParentIds.Contains(linhVucCoQuan.ILINHVUC.Value))
                {
                    //if (isFirstOptGroup)
                    //{
                    //    isFirstOptGroup = false;
                    //}
                    //else
                    //{
                    //    htmlOptions.Append("</optgroup>");
                    //}
                    //htmlOptions.Append("<optgroup label='" + HttpUtility.HtmlEncode(linhVucCoQuan.CTEN) + "'>");
                }
                else
                {
                    string select = "";
                    if (id_donvi_choice != 0 && linhVucCoQuan.ILINHVUC == id_donvi_choice) {
                        select = " selected ";
                        var linhVucCha = _thietlap.GetBy_Linhvuc_CoquanID(id_donvi_choice);
                        if (linhVucCha.IPARENT != 0)
                            continue;
                        else
                            htmlOptions.Append("<option " + select + " value='" + linhVucCoQuan.ILINHVUC + "'>" + HttpUtility.HtmlEncode(linhVucCha.CTEN) + "</option>");
                    }
                    else
                        htmlOptions.Append("<option " + select + " value='" + linhVucCoQuan.ILINHVUC + "'>" + HttpUtility.HtmlEncode(linhVucCoQuan.CTEN) + "</option>");
                }
            }
            return htmlOptions.ToString();
        }
      
        public string Option_TinhThanh_ByID_Parent(int parent = 0, int id = 0)
        {
            string str = "";

            _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IPARENT", parent);
            _dCondition.Add("IHIENTHI", 1);
            List<DIAPHUONG> diaphuong = _thietlap.GetBy_List_Diaphuong(_dCondition).OrderBy(t => HttpUtility.HtmlEncode(t.CTEN)).ToList();
            foreach (var p in diaphuong)
            {
                string select = ""; if (p.IDIAPHUONG == id) { select = "selected "; }
                str += "<option " + select + " value='" + p.IDIAPHUONG + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
                select = "";
            }
            return str;
        }

        public List<DIAPHUONG> Option_DiaPhuong_ByID_Parent(int? parent = null)
        {
            _dCondition = new Dictionary<string, object>();
            if (parent != null)
            {
                _dCondition.Add("IPARENT", parent.Value);
            }
            _dCondition.Add("IHIENTHI", 1);
            _dCondition.Add("IDELETE", 0);
            List<DIAPHUONG> diaphuong = _thietlap.GetBy_List_Diaphuong(_dCondition).ToList();
            if (diaphuong == null)
            {
                return new List<DIAPHUONG>();
            }
            return diaphuong;
        }

        public string OptionCoQuan_ThietLap(int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IPARENT", id_parent);

            var list = _thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Where(x => x.ICOQUAN != id_donvi_choice && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var donvi in list)
            {
                string select = "";
                if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IPARENT", donvi.ICOQUAN);
                var kiemtra = _thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Where(t => t.IDELETE == 0).ToList().Count();
                if (kiemtra > 0)
                {
                    str += OptionCoQuan_ThietLap((int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                }
            }
            return str;
        }
        public string OptionDiaphuong_ThietLap(List<DIAPHUONG> diaphuong, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            //Quochoi_CoquanRepository _quochoi_coquan = new Quochoi_CoquanRepository();
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var list = diaphuong.Where(v => v.IPARENT == id_parent && v.IDIAPHUONG != id_donvi_choice && v.IDELETE == 0).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList();
            foreach (var donvi in list)
            {
                string select = "";
                if (donvi.IDIAPHUONG == id_donvi) { select = " selected "; }
                str += "<option " + select + " value='" + donvi.IDIAPHUONG + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                str += OptionDiaphuong_ThietLap(diaphuong.Where(x => x.IDELETE == 0).ToList(), (int)donvi.IDIAPHUONG, level + 1, id_donvi, id_donvi_choice);

            }
            return str;

        }
        public string List_Kyhop_Khoa_search(List<QUOCHOI_KHOA> khoahop, List<QUOCHOI_KYHOP> Kyhop, int iKyhop = 0, string url_cookie = "")
        {
            string str = "";
            var khoa = khoahop.Where(x => x.IDELETE == 0).OrderByDescending(x => x.DBATDAU).ToList();
            int count = 1;
            foreach (var c in khoa)
            {
                var kyhop = Kyhop.Where(v => v.IDELETE == 0 && v.IKHOA == c.IKHOA).OrderBy(x => x.DBATDAU).ToList();

                if (iKyhop != 0)
                {
                    kyhop = kyhop.Where(x => x.IKYHOP == iKyhop).ToList();
                }
                if (kyhop.Count() > 0 || iKyhop == 0)
                {
                    string id_encr = HashUtil.Encode_ID(c.IKHOA.ToString(), url_cookie);
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Khoa_edit')\" rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(c.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Khoa_status')\"/>";

                    string khoa_macdinh = "";
                    if (c.IMACDINH == 1)
                    {
                        khoa_macdinh = "<i class='icon-ok'></i>";
                    }
                    str += "<tr><td class='tcenter '></td><td></td><td class=''>" + HttpUtility.HtmlEncode(c.CTEN) +
                        "</td><td class='tcenter '>";
                    DateTime temp = (DateTime)c.DBATDAU;
                    string yBatDau = "";
                    if (c.DBATDAU != null)
                        yBatDau = temp.Year.ToString();
                    temp = (DateTime)c.DKETTHUC;
                    string yKetThuc = "";
                    if (c.DKETTHUC != null)
                        yKetThuc = temp.Year.ToString() ;
                    str+= yBatDau + "</td><td class='tcenter '>" + yKetThuc + "</td>" +
                        "<td class='tcenter'>" + khoa_macdinh + "</td><td class='tcenter'></td><td></td><td class='tcenter' nowrap></td></tr>";
                    count++;
                    int count1 = 1;
                    foreach (var k in kyhop)
                    {
                        id_encr = HashUtil.Encode_ID(k.IKYHOP.ToString(), url_cookie);
                        string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Kyhop_order')\" type=\"text\" value='" + k.IVITRI + "' class='input-block-level tcenter' />";
                        string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Kyhop_del','Bạn có muốn xóa kỳ họp " + HttpUtility.HtmlEncode(k.CTEN) + " khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                        string edit1 = " <a href=\"javascript:void()\" data-original-title='Sửa' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Kyhop_edit')\" rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                        string chon = "<a id='btn_" + id_encr + "'  data-original-title='Chọn' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonKyHop('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Kyhop_macdinh')\" class='f-grey chontrung'><i class='icon-ok-sign'></i></a>";
                        hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(k.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Kyhop_status')\"/>";
                        if (k.IMACDINH == 1)
                        {
                            chon = "<a id='btn_" + id_encr + "'  data-original-title='Chọn' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonKyHop('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Kyhop_macdinh')\" class='trans_func chontrung'><i class='icon-ok-sign'></i></a>";
                        }
                        if (c.IMACDINH == 0)
                        {
                            chon = "";
                        }
                        str += "<tr><td class='tcenter '>" + count1 + "</td><td class='tcenter  f-red '>" + HttpUtility.HtmlEncode(k.CCODE) + "</td><td class='' style='padding-left:25px'> - - - " + HttpUtility.HtmlEncode(k.CTEN) +
                        "</td><td class='tcenter'>";
                        if (k.DBATDAU != null)
                            str += func.ConvertDateVN(k.DBATDAU.ToString());
                        str += "</td><td class='tcenter'>";
                        if (k.DKETTHUC != null)
                            str += func.ConvertDateVN(k.DKETTHUC.ToString());
                        str+= "</td><td class='tcenter'>" + chon + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter' nowrap>" + edit1 + del + "</td></tr>";
                        count1++;
                    }
                }
            }
            return str;
        }
        public string List_DonVi_TaiKhoan_search(List<USER_PHONGBAN> phongban, List<QUOCHOI_COQUAN> coquan, List<USERS> user, List<USER_GROUP> group,
                                  int iDonVi, int iUser, int id)
        {
            //int iDonVi = Convert.ToInt32(db.users.Single(x => x.iUser.Equals(iUser)).iDonVi);
            string str = "";
            string url_cookie = func.Get_Url_keycookie();
            var donvi = coquan.OrderBy(t => t.IVITRI).ToList();
            if (iDonVi != 0)
            {
                donvi = donvi.Where(v => v.IPARENT == iDonVi).ToList();
            }
            if (donvi.Count() > 0)
            {
                foreach (var u in donvi)
                {
                    var tk = user.Where(v => v.IDONVI == (int)u.ICOQUAN && v.ITYPE != -1 && v.IUSER == id).OrderBy(p => p.ICHUCVU).ToList();
                    if (tk.Count > 0)
                    {
                        str += "<thead style='width:100%'><tr><th class='' colspan='6'>" + HttpUtility.HtmlEncode(u.CTEN) + "</th></tr></thead>";

                        foreach (var p in phongban.Where(v => v.IDONVI == (int)u.ICOQUAN).OrderBy(x => x.IVITRI))
                        {
                            var tk_phong = tk.Where(x => x.IPHONGBAN == (int)p.IPHONGBAN).OrderBy(x => x.ICHUCVU).ToList();
                            if (tk_phong.Count() > 0)
                            {
                                str += "<tr><th class='' colspan='6'> - - - " + HttpUtility.HtmlEncode(p.CTEN) + "</th></tr>";
                                foreach (var t in tk_phong)
                                {
                                    string id_encr = HashUtil.Encode_ID(t.IUSER.ToString(), url_cookie);
                                    str += Row_TaiKhoan(group, t, iUser, id_encr);
                                }
                            }
                        }
                        var tk0 = tk.Where(x => x.IPHONGBAN == 0).OrderBy(x => x.ICHUCVU).ToList();
                        if (tk0.Count() > 0)
                        {
                            str += "<tr><th class='' colspan='6'> - - - Phòng ban khác</th></tr>";
                            foreach (var t in tk0)
                            {
                                string id_encr = HashUtil.Encode_ID(t.IUSER.ToString(), url_cookie);
                                str += Row_TaiKhoan(group, t, iUser, id_encr);
                            }
                        }
                    }
                }
            }
            return str;
        }

        public string List_Kyhop_Khoa(List<QUOCHOI_KHOA> khoa, List<QUOCHOI_KYHOP> kyhop, int iKyhop = 0, string url_cookie = "")
        {
            string str = "";
            khoa = khoa.OrderByDescending(x => x.DBATDAU).Where(x => x.IDELETE == 0).ToList();
            int count = 1;
            //string url_cookie = func.Get_Url_keycookie();
            foreach (var c in khoa)
            {
                //Dictionary<string, object> _dQuochoi_kyhop = new Dictionary<string, object>();
                //_dQuochoi_kyhop.Add("IKHOA", c.IKHOA);
                var kyhop1 = kyhop.Where(v => v.IKHOA == (int)c.IKHOA && v.IDELETE == 0).OrderBy(x => x.DBATDAU).ToList();
                // var kyhop = db.quochoi_kyhop.Where(x => x.iKhoa == c.iKhoa).OrderBy(x => x.dBatDau).ToList();
                if (iKyhop != 0)
                {
                    kyhop1 = kyhop.Where(x => x.IKYHOP == iKyhop && x.IDELETE == 0).ToList();
                }
                if (kyhop.Count() > 0 || iKyhop == 0)
                {
                    string id_encr = HashUtil.Encode_ID(c.IKHOA.ToString(), url_cookie);
                    // string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Khoa_edit')\" rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                    //string add_kyhop = " <a href=\"javascript:void()\" data-original-title='Thêm kỳ họp' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Kyhop_add')\" rel='tooltip' title='' class='trans_func'><i class='icon-plus-sign'></i></a> ";
                    // string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Khoa_del')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string hienthi = "";
                    string khoa_macdinh = "";
                    if (c.IMACDINH == 1)
                    {
                        khoa_macdinh = "<i class='icon-ok'></i>";
                    }
                    DateTime temp = (DateTime)c.DBATDAU;
                    string yBatDau = "";
                    if (c.DBATDAU != null)
                        yBatDau = temp.Year.ToString();
                    temp = (DateTime)c.DKETTHUC;
                    string yKetThuc = "";
                    if (c.DKETTHUC != null)
                        yKetThuc = temp.Year.ToString();
                    str += "<tr><td class='tcenter '></td><td class='tcenter '></td><td class=''>" + HttpUtility.HtmlEncode(c.CTEN) +
                        "</td><td class='tcenter '>" + yBatDau + "</td><td class='tcenter '>" + yKetThuc + "</td>" +
                        "<td class='tcenter'>" + khoa_macdinh + "</td><td class='tcenter'></td><td class='tcenter'></td><td class='tcenter' nowrap></td></tr>";
                    count++;
                    int count1 = 1;
                    foreach (var k in kyhop1)
                    {
                        id_encr = HashUtil.Encode_ID(k.IKYHOP.ToString(), url_cookie);
                        string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Kyhop_del','Bạn có muốn xóa kỳ họp " + HttpUtility.HtmlEncode(k.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                        string edit1 = " <a href=\"javascript:void()\" data-original-title='Sửa' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Kyhop_edit')\" rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                        string chon = "<a id='btn_" + id_encr + "'  data-original-title='Chọn' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonKyHop('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Kyhop_macdinh')\" class='f-grey chontrung'><i class='icon-ok-sign'></i></a>";
                        string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Kyhop_order')\" type=\"text\" value='" + k.IVITRI + "' class='input-block-level tcenter' />";
                        hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(k.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Kyhop_status')\"/>";
                        if (k.IMACDINH == 1)
                        {
                            chon = "<a id='btn_" + id_encr + "'  data-original-title='Chọn' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonKyHop('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Kyhop_macdinh')\" class='trans_func chontrung'><i class='icon-ok-sign'></i></a>";
                        }
                        if (c.IMACDINH == 0)
                        {
                            chon = "";
                        }
                        str += "<tr><td class='tcenter '>" + count1 + "</td><td class='tcenter  f-red '>" + HttpUtility.HtmlEncode(k.CCODE) + "</td><td class='' style='padding-left:25px'> - - - " + HttpUtility.HtmlEncode(k.CTEN) +
                        "</td><td class='tcenter'>";
                        if (k.DBATDAU != null)
                            str += func.ConvertDateVN(k.DBATDAU.ToString());
                        str += "</td><td class='tcenter'>";
                        if (k.DKETTHUC != null)
                            str += func.ConvertDateVN(k.DKETTHUC.ToString()); 
                        str+= "</td>" + "<td class='tcenter'>" + chon + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter' nowrap>" + edit1 + del + "</td></tr>";
                        count1++;
                    }
                }
            }
            return str;
        }
        public string OptionKyhop_Search(string url_cookie)
        {
            string str = "";
            var khoa = _thietlap.Get_List_Quochoi_Khoa().OrderByDescending(d => d.DBATDAU);
            foreach (var k in khoa)
            {
                Dictionary<string, object> _dQuochoi_kyhop = new Dictionary<string, object>();
                _dQuochoi_kyhop.Add("IKHOA", k.IKHOA);
                var kyhop = _thietlap.Get_List_Quochoi_Kyhop(_dQuochoi_kyhop).Where(x => x.IDELETE == 0).OrderBy(d => d.DBATDAU).OrderBy(x => x.IVITRI).ToList();
                foreach (var t in kyhop)
                {
                    string id_encr = HashUtil.Encode_ID(t.IKYHOP.ToString());
                    str += "<option value='" + id_encr + "'>" + HttpUtility.HtmlEncode(t.CTEN) + " (" + HttpUtility.HtmlEncode(k.CTEN) + ")</option>";
                }

            }
            return str;

        }
        public string Ajax_List_CoQuan()
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IGROUP", 0);
            var coquan = _thietlap.GetBy_List_Quochoi_Coquan(_condition).ToList();
            int count = 0;
            foreach (var c in coquan)
            {
                if (count > 0) { str += "|"; }
                str += HttpUtility.HtmlEncode(c.CTEN);
                count++;
            }
            return str;
        }
        public string List_Coquan(List<QUOCHOI_COQUAN> coquan, List<DIAPHUONG> diaphuong, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var donvi = coquan.Where(x => x.IPARENT == id_parent && x.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();

            if (level == 0 && id_parent != 0)
            {//có truyền vào id đơn vị để tìm kiếm
                donvi = donvi.Where(x => x.ICOQUAN == id_parent && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            }
            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {
                    string id_encr = HashUtil.Encode_ID(t.ICOQUAN.ToString(), url_cookie);
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Coquan_order')\" type=\"text\" value='" + t.IVITRI + "' class='input-block-level tcenter' />";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Coquan_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Coquan_del','Bạn có chắc xóa cơ quan " + HttpUtility.HtmlEncode(t.CTEN) + " khỏi dánh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string group = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IGROUP)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_group')\"/>";
                    string use = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IUSE)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_use')\"/>";
                    string tendiaphuong = "";
                    var thongtindiaphuong = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == t.IDIAPHUONG).ToList();
                    if (t.IDIAPHUONG != 0 && thongtindiaphuong.Count() > 0)
                    {
                        tendiaphuong = diaphuong.Single(x => x.IDIAPHUONG == t.IDIAPHUONG).CTEN;
                    }
                    if (t.ICOQUAN == 4 && HttpUtility.HtmlEncode(t.CCODE) == "BDN")
                    {
                        edit = ""; del = "";
                    }
                    //  string diaphuong = ""; if (t.IDIAPHUONG != 0) { diaphuong = _thietlap.GetBy_DiaphuongID(((int)t.IDIAPHUONG)).CTEN; }
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td >" +
                        space_level + HttpUtility.HtmlEncode(t.CTEN) + "</td><td class='tcenter'>" + tendiaphuong + "</td><td class='tcenter'>" + order +
                        "</td><td class='tcenter'>" + use + "</td><td class='tcenter'>" + group + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                    if (coquan.Where(x => x.IPARENT == (int)t.ICOQUAN).Count() > 0)
                    {
                        str += List_Coquan(coquan, diaphuong, (int)t.ICOQUAN, level + 1, iUser, url_cookie);
                    }

                }
            }
            return str;
        }
        
        public string List_DonVi_PhongBan_Search(List<QUOCHOI_COQUAN> donvicoquan, List<USER_PHONGBAN> phongban, int id, int iUser_Login, string url_cookie)
        {
            string str = "";
            int iDonVi = IDDonVi_User(iUser_Login);
            _condition = new Dictionary<string, object>();
            var donvi = donvicoquan.Where(x => x.IDELETE == 0 && x.IGROUP == 0).OrderBy(t => t.IVITRI).ToList();

            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {
                    if (id == 0)
                    {

                        var phong = phongban.Where(v => v.IDELETE == 0 && v.IDONVI == (int)t.ICOQUAN).OrderBy(p => p.IVITRI).ToList();
                        if (phong.Count > 0)
                        {

                            str += "<tr><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td class='' colspan='4'>" + HttpUtility.HtmlEncode(t.CTEN) + "</td></tr>";
                            foreach (var p in phong)
                            {
                                string id_encr = HashUtil.Encode_ID(p.IPHONGBAN.ToString(), url_cookie);
                                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Phongban_order')\" type=\"text\" value='" + p.IVITRI + "' class='input-block-level tcenter' />";
                                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Phongban_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Phongban_del','Bạn có chắc xóa cơ quan " + HttpUtility.HtmlEncode(t.CTEN) + " khỏi danh sách')\" class='trans_func'><i class='icon-trash'></i></a> ";
                                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(p.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Phongban_status')\"/>";


                                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'></td><td>- - - " + HttpUtility.HtmlEncode(p.CTEN) +
                                    "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";

                            }
                        }
                    }
                    else
                    {

                        var phong = phongban.Where(v => v.IDELETE == 0 && v.IDONVI == (int)t.ICOQUAN && v.IPHONGBAN == id).OrderBy(p => p.IVITRI).ToList();
                        //   List<USER_PHONGBAN> phong = _thietlap.GetBy_List_Phongban(_condition).Where(v=>v.IDELETE == 0).OrderBy(p => p.IVITRI).ToList();
                        if (phong.Count > 0)
                        {

                            str += "<tr><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td class='' colspan='4'>" + HttpUtility.HtmlEncode(t.CTEN) + "</td></tr>";
                            foreach (var p in phong)
                            {
                                string id_encr = HashUtil.Encode_ID(p.IPHONGBAN.ToString(), url_cookie);
                                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Phongban_order')\" type=\"text\" value='" + p.IVITRI + "' class='input-block-level tcenter' />";
                                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Phongban_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Phongban_del','Bạn có chắc xóa cơ quan " + HttpUtility.HtmlEncode(t.CTEN) + " khỏi danh sách')\" class='trans_func'><i class='icon-trash'></i></a> ";
                                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(p.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Phongban_status')\"/>";


                                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'></td><td>- - - " + HttpUtility.HtmlEncode(p.CTEN) +
                                    "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";

                            }
                        }
                    }

                }
            }
            return str;
        }
        public string Option_Coquan(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, string url_cookie = "")
        {
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            List<QUOCHOI_COQUAN> donvi = coquan.Where(x => x.IPARENT == id_parent && x.IDELETE == 0).OrderBy(x => x.IVITRI).OrderBy(x => x.IVITRI).ToList();

            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {
                    string id_encr = HashUtil.Encode_ID(t.ICOQUAN.ToString(), url_cookie);
                    //string diaphuong = "";
                    //var thongtindiaphuong = _thietlap.Get_Diaphuong().Where(x => x.IDIAPHUONG == t.IDIAPHUONG).ToList();
                    //if (t.IDIAPHUONG != 0 && thongtindiaphuong.Count() > 0 )
                    //{ diaphuong = _thietlap.GetBy_DiaphuongID((int)t.IDIAPHUONG).CTEN; }
                    str += "<option value='" + id_encr + "'>" + space_level + " " + HttpUtility.HtmlEncode(t.CTEN) + "</option>";
                    str += Option_Coquan(coquan, (int)t.ICOQUAN, level + 1, url_cookie);
                }
            }
            return str;
        }
        public string Option_Coquan_phongban(List<QUOCHOI_COQUAN> coquan, int iddonvi = 0)
        {
            string str = "";
            if (coquan.Count() > 0)
            {
                string sel = "";
                foreach (var t in coquan)
                {
                    if (iddonvi == t.ICOQUAN)
                    {
                        sel = "selected";
                    }
                    str += "<option value='" + t.ICOQUAN + "' " + sel + "> " + HttpUtility.HtmlEncode(t.CTEN) + "</option>";
                }
            }
            return str;
        }

        public string List_DonVi_PhongBan(List<USER_PHONGBAN> phongban, List<QUOCHOI_COQUAN> donvicoquan, int iUser_Login, string url_cookie)
        {
            string str = "";
            int iDonVi = IDDonVi_User(iUser_Login);
            var donvi = donvicoquan.Where(x => x.IDELETE == 0 && x.IHIENTHI == 1).OrderBy(t => t.IVITRI).ToList();
            phongban = phongban.Where(x => x.IDELETE == 0 && x.IHIENTHI == 1).ToList();
            List<decimal> iDList = new List<decimal>();
            int level = 0;
            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {
                    List<USER_PHONGBAN> phong = phongban.Where(v => v.IDONVI == t.ICOQUAN).OrderBy(p => p.IPARENT).ToList();
                    if (phong.Count > 0)
                    {
                        str += "<tr><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td style='font-weight:bold;' colspan='4'>" + HttpUtility.HtmlEncode(t.CTEN) + "</td></tr>";
                        foreach (var p in phong)
                        {
                            if (iDList.Contains(p.IPHONGBAN)) continue;
                            string id_encr = HashUtil.Encode_ID(p.IPHONGBAN.ToString(), url_cookie);
                            string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Phongban_order')\" type=\"text\" value='" + p.IVITRI + "' class='input-block-level tcenter' />";
                            string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Phongban_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                            string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Phongban_del','Bạn có chắc xóa phòng ban " + HttpUtility.HtmlEncode(p.CTEN) + " khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                            string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(p.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Phongban_status')\"/>";


                            str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'></td><td>" + HttpUtility.HtmlEncode(p.CTEN) +
                                "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                            str += Recusive_List_DonVi_PhongBan(ref iDList, phongban, p.IPHONGBAN, level, url_cookie);
                        }
                    }
                }
            }
            return str;
        }

        public string Recusive_List_DonVi_PhongBan(ref List<decimal> iDList, List<USER_PHONGBAN> phongBanList, decimal id_phongbancha, int level, string url_cookie)
        {
            string str = "";
            var phongban = phongBanList.Where(x => x.IPARENT == id_phongbancha).ToList();
            if (phongban.Count() > 0)
            {
                level++;
                foreach (var pb in phongban)
                {
                    iDList.Add(pb.IPHONGBAN);
                    string id_encr = HashUtil.Encode_ID(pb.IPHONGBAN.ToString(), url_cookie);
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Phongban_order')\" type=\"text\" value='" + pb.IVITRI + "' class='input-block-level tcenter' />";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Phongban_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Phongban_del','Bạn có chắc xóa phòng ban " + HttpUtility.HtmlEncode(pb.CTEN) + " khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(pb.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Phongban_status')\"/>";


                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'></td><td>" + string.Concat(Enumerable.Repeat("- ", level)) + HttpUtility.HtmlEncode(pb.CTEN) +
                        "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                    str += Recusive_List_DonVi_PhongBan(ref iDList, phongBanList, pb.IPHONGBAN, level, url_cookie);
                }
            }
            return str;
        }

        public string Option_DonVi_PhongBan(int iUser_Login)
        {
            string str = "";
            int iDonVi = IDDonVi_User(iUser_Login);
            _condition = new Dictionary<string, object>();
            _condition.Add("IGROUP", 0);
            List<QUOCHOI_COQUAN> donvi = _thietlap.GetBy_List_Quochoi_Coquan(_condition).Where(x => x.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            //  string url_cookie = func.Get_Url_keycookie();
            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {
                    _condition = new Dictionary<string, object>();
                    _condition.Add("IDONVI", t.ICOQUAN);
                    List<USER_PHONGBAN> phong = _thietlap.GetBy_List_Phongban(_condition).Where(v => v.IDELETE == 0).OrderBy(p => p.IVITRI).OrderBy(x => x.IVITRI).ToList();
                    if (phong.Count > 0)
                    {

                        str += "<optgroup label='" + HttpUtility.HtmlEncode(t.CTEN) + "' >";
                        foreach (var p in phong)
                        {
                            string id_encr = HashUtil.Encode_ID(p.IPHONGBAN.ToString());

                            str += "<option value=" + id_encr + ">- - - " + HttpUtility.HtmlEncode(p.CTEN) +
                                "</option>";

                        }
                        str += "</optgroup>";

                    }
                }
            }
            return str;
        }
        public string Option_Phongban(List<USER_PHONGBAN> phong)
        {
            string str = "";
            if (phong.Count > 0)
            {
                foreach (var p in phong)
                {
                    string id_encr = HashUtil.Encode_ID(p.IPHONGBAN.ToString());
                    str += "<option value=" + id_encr + ">- - - " + HttpUtility.HtmlEncode(p.CTEN) +
                        "</option>";
                }
            }
            return str;
        }
        public string Option_Phongban_Update(List<USER_PHONGBAN> phong, int id = 0)
        {
            string str = "";
            if (phong.Count > 0)
            {
                foreach (var p in phong)
                {
                    string id_encr = HashUtil.Encode_ID(p.IPHONGBAN.ToString());
                    string sel = "";
                    if((int)p.IPHONGBAN == id)
                    {
                        sel = "selected";
                    }
                    str += "<option value=" + p.IPHONGBAN + " " + sel + " >- - - " + HttpUtility.HtmlEncode(p.CTEN) +
                        "</option>";
                }
            }
            return str;
        }
        // Nhóm quyền tài khoản 
        public string List_NhomQuyen(List<USER_GROUP> usergroup, int iUser)
        {
            string str = "";
            var nhom = usergroup.ToList();
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in nhom)
            {
                string id_encr = HashUtil.Encode_ID(x.IGROUP.ToString(), url_cookie);
                string edit = " <a href='javascript:void(0)' class='trans_func' title='Sửa thông tin nhóm' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nhomtaikhoan_edit')\"><i class='icon-pencil'></i></a>";
                string cauhinh = " <a href='/Thietlap/Nhomtaikhoan_cog/?id=" + id_encr + "' class='trans_func' title='Chức năng nhóm' ><i class='icon-cog'></i></a>";
                string trangthai = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IDELETE)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nhomtaikhoan_status')\"/>";
                str += "<tr><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td >" + HttpUtility.HtmlEncode(x.CMOTA) + "</td><td><div class='scroll'>" +
                    List_ChucNang_Nhom_View((int)x.IGROUP) +
                    "</div></td><td class='tcenter'>" + trangthai + "</td><td class='tcenter'>" + cauhinh + edit + "</td></tr>";
            }
            return str;
        }

        public string List_NhomQuyenOptimize(List<USER_GROUP> usergroupData, List<ACTION> actionListData, List<USER_GROUP_ACTION> userGroupActionData, ControllerContext controllerContext)
        {
            string str = "";
            string url_cookie = func.Get_Url_keycookie();
            foreach (var group in usergroupData)
            {
                UserGroupIndexViewModel userGroupIndexViewModel = new UserGroupIndexViewModel();
                userGroupIndexViewModel.CTEN = group.CTEN;
                userGroupIndexViewModel.CMOTA = group.CMOTA;
                userGroupIndexViewModel.IDELETE = group.IDELETE;
                userGroupIndexViewModel.IGROUP = group.IGROUP;
                List<USER_GROUP_ACTION> userGroupActionList = userGroupActionData.Where(x => x.IGROUP == group.IGROUP).ToList();
                if (userGroupActionList.Count > 0)
                {
                    var listActionViewModel = new List<ActionViewModel>();
                    foreach(var userGroupAction in userGroupActionList)
                    {
                        var action = actionListData.FirstOrDefault(x => userGroupAction.IACTION.HasValue && x.IACTION == userGroupAction.IACTION);
                        if(action != null)
                        {
                            listActionViewModel.Add( new ActionViewModel
                            {
                                IACTION = action.IACTION,
                                CTEN = action.CTEN,
                                IPARENT = action.IPARENT,
                                IVITRI = action.IVITRI
                            });
                        }
                    }
                    if(listActionViewModel.Count > 0)
                    {
                        userGroupIndexViewModel.ActionListGroup = new Dictionary<string, List<ActionViewModel>>();
                        var listActionGroupDic = listActionViewModel.GroupBy(x => x.IPARENT).ToDictionary(x => x.Key, x => x.ToList());
                        foreach(var actionGroup in listActionGroupDic)
                        {
                            var actionParent = actionListData.FirstOrDefault(x => x.IACTION == actionGroup.Key);
                            if(actionParent != null)
                            {
                                userGroupIndexViewModel.ActionListGroup.Add(actionParent.CTEN, actionGroup.Value);
                            }
                            else
                            {
                                userGroupIndexViewModel.ActionListGroup.Add(String.Empty, actionGroup.Value);
                            }
                        }
                    }
                }
                var htmlItemUserGroup = ViewRenderer.RenderPartialView("~/Views/Ajax/Thietlap/NhomTaiKhoanIndexRowPartial.cshtml", userGroupIndexViewModel, controllerContext);
                if (!string.IsNullOrEmpty(htmlItemUserGroup))
                {
                    str += htmlItemUserGroup;
                }
            }
            return str;
        }

        public string Option_NhomQuyen(List<USER_GROUP> usergroup, int iUser)
        {
            string str = "";
            var nhom = usergroup.ToList();
            foreach (var x in nhom)
            {
                str += "<option value='" + x.IGROUP + "'>" + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
            }
            return str;
        }
        public string List_NhomQuyen_search(List<USER_GROUP> usergroup, int iUser, string url_cookie, int id)
        {
            string str = "";
            var nhom = usergroup.ToList();
            if (id != 0)
            { nhom = usergroup.Where(x => x.IGROUP == id).ToList(); }
            foreach (var x in nhom)
            {
                string id_encr = HashUtil.Encode_ID(x.IGROUP.ToString(), url_cookie);
                string edit = " <a href='javascript:void(0)' class='trans_func' title='Sửa thông tin nhóm' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nhomtaikhoan_edit')\"><i class='icon-pencil'></i></a>";
                string cauhinh = " <a href='/Thietlap/Nhomtaikhoan_cog/?id=" + id_encr + "' class='trans_func' title='Chức năng nhóm' ><i class='icon-cog'></i></a>";
                string trangthai = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IDELETE)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nhomtaikhoan_status')\"/>";
                str += "<tr><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td >" + HttpUtility.HtmlEncode(x.CMOTA) + "</td><td><div class='scroll'>" +
                    List_ChucNang_Nhom_View((int)x.IGROUP) +
                    "</div></td><td class='tcenter'>" + trangthai + "</td><td class='tcenter'>" + cauhinh + edit + "</td></tr>";
            }
            return str;
        }

        // end 
        public string GetName_KhoaHop_ByKyHop(int iKyHop)
        {
            string str = "";
            var thongtinkyhop = _thietlap.Get_List_Quochoi_Kyhop().Where(x => x.IKYHOP == iKyHop).ToList();
            if (thongtinkyhop.Count() > 0)
            {
                QUOCHOI_KYHOP kyhop = _thietlap.Get_Quochoi_Kyhop(iKyHop);
                var thongtinkhoa = _thietlap.Get_List_Quochoi_Khoa().Where(x => x.IKHOA == kyhop.IKHOA).ToList();
                if (thongtinkhoa.Count() > 0)
                {
                    QUOCHOI_KHOA khoa = _thietlap.Get_Quochoi_Khoa((int)kyhop.IKHOA);
                    str = khoa.CTEN;
                }
            }
            return str;

        }
        public string List_ChucNang_Nhom_View(int id_nhom)
        {
            string str = "";

            // string sql = "select distinct ACTION.IPARENT from ACTION inner join USER_GROUP_ACTION on " + " ACTION.IACTION=USER_GROUP_ACTION.IACTION and USER_GROUP_ACTION.IGROUP=" + id_nhom;
            List<ACTION> action0 = _thietlap.Get_List_UsergroupSql(id_nhom, 0).ToList();
            foreach (var t in action0)
            {
                ACTION a = _thietlap.GetBy_ActionID((int)t.IPARENT);
                str += "<p class=''>" + a.CTEN + ":</p><ul class='list-chucnang'>";
                var action1 = _thietlap.Get_List_UsergroupSql(id_nhom, (int)a.IACTION).ToList();
                foreach (var a1 in action1)
                {

                    str += "<li>- " + a1.CTEN + "</li>";
                }
                str += "</ul>";
            }
            return str;
        }
        public List<QUOCHOI_KHOA> Option_Khoa_QuocHoi()
        {
            var _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IHIENTHI", 1);
            _dCondition.Add("IDELETE", 0);
            List<QUOCHOI_KHOA> listKhoa = _thietlap.Get_List_Quochoi_Khoa(_dCondition).ToList();
            return listKhoa;
        }
        //public string List_ChucNang_Nhom_View(int id_nhom)
        //{
        //    string str = "";
        //    List<ID_Parent> action0 = db.Database.SqlQuery<ID_Parent>("select distinct action.iParent from action inner join user_group_action on " +
        //        " action.iAction=user_group_action.iAction and user_group_action.iGroup=" + id_nhom).ToList();
        //    foreach (var t in action0)
        //    {
        //        action a = db.actions.Single(x => x.iAction.Equals(t.iParent));
        //        str += "<p class='b'>" + a.cTen + ":</p><ul class='list-chucnang'>";
        //        var action1 = db.actions.SqlQuery("select action.* from action inner join user_group_action on " +
        //       " action.iAction=user_group_action.iAction and action.iParent=" + t.iParent + " and user_group_action.iGroup=" + id_nhom).ToList();
        //        foreach (var a1 in action1)
        //        {
        //            str += "<li>- " + a1.cTen + "</li>";
        //        }
        //        str += "</ul>";
        //    }
        //    return str;
        //}
        public string List_ChucNang_TaiKhoan_View(int id_tk)
        {
            string str = "";
            List<ID_Parent> action0 = _thietlap.GetListID_Parent_SQL(id_tk).ToList();

            foreach (var t in action0)
            {
                if (t.iParent != 0)
                {
                    ACTION a = _thietlap.GetBy_ActionID(t.iParent);
                    str += "<p class=''>" + a.CTEN + ":</p><ul class='list-chucnang'>";
                }
                List<ID_Parent> action1 = _thietlap.GetListID_Parent_SQL_CheckAction((int)t.iParent, id_tk).ToList();
                foreach (var a1 in action1)
                {
                    ACTION action = _thietlap.GetBy_ActionID(a1.iParent);
                    str += "<li>- " + HttpUtility.HtmlEncode(action.CTEN) + "</li>";
                }
                str += "</ul>";
            }
            return str;
        }
        public string List_DonVi_TaiKhoan(List<USER_PHONGBAN> phongban, List<QUOCHOI_COQUAN> coquan, List<USERS> user, List<USER_GROUP> group,
                                int iDonVi, int iUser)
        {
            //int iDonVi = Convert.ToInt32(db.users.Single(x => x.iUser.Equals(iUser)).iDonVi);
            string str = "";
            string url_cookie = func.Get_Url_keycookie();
            var donvi = coquan.OrderBy(t => t.IVITRI).ToList();
            if (iDonVi != 0)
            {
                donvi = donvi.Where(v => v.IPARENT == iDonVi).ToList();
            }
            if (donvi.Count() > 0)
            {
                foreach (var u in donvi)
                {
                    var tk = user.Where(v => v.IDONVI == (int)u.ICOQUAN && v.ITYPE != -1).OrderBy(p => p.ICHUCVU).ToList();
                    if (tk.Count > 0)
                    {
                        str += "<thead><tr><th class='' colspan='6'>" + HttpUtility.HtmlEncode(u.CTEN) + "</th></tr></thead>";

                        foreach (var p in phongban.Where(v => v.IDONVI == (int)u.ICOQUAN).OrderBy(x => x.IVITRI))
                        {
                            var tk_phong = tk.Where(x => x.IPHONGBAN == (int)p.IPHONGBAN).OrderBy(x => x.ICHUCVU).ToList();
                            if (tk_phong.Count() > 0)
                            {
                                str += "<tr><th class='' colspan='6'> - - - " + HttpUtility.HtmlEncode(p.CTEN) + "</th></tr>";
                                foreach (var t in tk_phong)
                                {
                                    string id_encr = HashUtil.Encode_ID(t.IUSER.ToString(), url_cookie);
                                    str += Row_TaiKhoan(group, t, iUser, id_encr);
                                }
                            }
                        }
                        var tk0 = tk.Where(x => x.IPHONGBAN == 0).OrderBy(x => x.ICHUCVU).ToList();
                        if (tk0.Count() > 0)
                        {
                            str += "<tr><th class='' colspan='6'> - - - Phòng ban khác</th></tr>";
                            foreach (var t in tk0)
                            {
                                string id_encr = HashUtil.Encode_ID(t.IUSER.ToString(), url_cookie);
                                str += Row_TaiKhoan(group, t, iUser, id_encr);
                            }
                        }
                    }
                }
            }
            return str;
        }
        public string List_DonVi_TaiKhoan_search(List<USER_PHONGBAN> phongban, List<QUOCHOI_COQUAN> coquan, List<USERS> user, List<USER_GROUP> group, int iDonVi, int iUser, int iddonvi, string url_cookie)
        {
            //int iDonVi = Convert.ToInt32(db.users.Single(x => x.iUser.Equals(iUser)).iDonVi);
            string str = "";

            var donvi = coquan.OrderBy(t => t.IVITRI).ToList();
            if (iDonVi != 0)
            {
                donvi = donvi.Where(v => v.IPARENT == iDonVi).ToList();
            }
            if (donvi.Count() > 0)
            {
                foreach (var u in donvi)
                {
                    var tk = user.Where(v => v.IDONVI == (int)u.ICOQUAN && v.ITYPE != -1).OrderBy(p => p.ICHUCVU).ToList();
                    if (tk.Count > 0)
                    {
                        str += "<thead><tr><th class='' colspan='6'>" + HttpUtility.HtmlEncode(u.CTEN) + "</th></tr></thead>";

                        foreach (var p in phongban.Where(v => v.IDONVI == u.ICOQUAN).OrderBy(x => x.IVITRI))
                        {
                            var tk_phong = tk.Where(x => x.IPHONGBAN == (int)p.IPHONGBAN).OrderBy(x => x.ICHUCVU).ToList();
                            if (tk_phong.Count() > 0)
                            {
                                str += "<tr><th class='' colspan='6'> - - - " + HttpUtility.HtmlEncode(p.CTEN) + "</th></tr>";
                                foreach (var t in tk_phong)
                                {
                                    string id_encr = HashUtil.Encode_ID(t.IUSER.ToString(), url_cookie);
                                    str += Row_TaiKhoan(group, t, iUser, id_encr);
                                }
                            }
                        }
                        var tk0 = tk.Where(x => x.IPHONGBAN == 0).OrderBy(x => x.ICHUCVU).ToList();
                        if (tk0.Count() > 0)
                        {
                            str += "<tr><th class='' colspan='6'> - - - Phòng ban khác</th></tr>";
                            foreach (var t in tk0)
                            {
                                string id_encr = HashUtil.Encode_ID(t.IUSER.ToString(), url_cookie);
                                str += Row_TaiKhoan(group, t, iUser, id_encr);
                            }
                        }
                    }
                }
            }
            return str;
        }

        public TaiKhoan Taikhoan_Detail(int id)
        {
            TaiKhoan t = new TaiKhoan();
            USERS u = _thietlap.GetBy_TaikhoanID(id);
            if (u != null)
            {
                int iPhong = (int)u.IPHONGBAN;
                Dictionary<string, object> _dUserPhongban = new Dictionary<string, object>();
                _dUserPhongban.Add("IPHONGBAN", iPhong);
                var user_phongban = _thietlap.GetBy_List_Phongban(_dUserPhongban);
                if (user_phongban.Count() > 0)
                {
                    t.phongban = _thietlap.GetBy_PhongbanID(iPhong).CTEN;
                }
                int ichucvu = (int)u.ICHUCVU;
                List<USER_CHUCVU> user_chucvu = _thietlap.Get_List_User_Chucvu().Where(x => x.ICHUCVU == ichucvu).ToList();
                if (user_chucvu.Count() > 0)
                {
                    t.chucvu = _thietlap.Get_Chucvu(ichucvu).CTEN;
                }
                int iDonVi = (int)u.IDONVI;
                t.donvi = _thietlap.GetBy_Quochoi_CoquanID(iDonVi).CTEN;
                t.ten = u.CTEN;
            }
            return t;
        }
        public string Row_TaiKhoan(List<USER_GROUP> group, USERS t, int iUser_Login, string id_encr)
        {
            string str = "";

            if (t.ITYPE == 2)
            {
                //return "";
            }
            string trangthai = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.ISTATUS)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Taikhoan_status')\"/>";


            string chucvu = "";
            if (_thietlap.Get_Chucvu((int)t.ICHUCVU) != null)
            {
                chucvu = "( " + _thietlap.Get_Chucvu((int)t.ICHUCVU).CTEN + " )";
            }

            string ten = "<p><strong class='f-red'>" + t.CUSERNAME + "</strong></p><p>" + HttpUtility.HtmlEncode(t.CTEN) + "</p>";
            string thongtin = "<p><strong>Email:</strong> " + t.CEMAIL + "</p><p><strong>SĐT:</strong>: " + t.CSDT + "</p>";
            string cog = "<a href=\"/Thietlap/Taikhoan_phanquyen/?id=" + id_encr + "\" data-original-title='Phân quyền' rel='tooltip' title='' class='trans_func'><i class='icon-cog'></i></a> ";
            //cog = "";
            string chitiet = "<a href=\"javascript:void()\" data-original-title='Xem chi tiết' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Taikhoan_info')\" class='trans_func'><i class='icon-info-sign'></i></a> ";
            string edit = "<a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Taikhoan_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
            string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Taikhoan_del')\" class='trans_func'><i class='icon-trash'></i></a> ";
            string lichsu = " <a href='/Thietlap/Taikhoan_Lichsu/" + id_encr + "' data-original-title='Lịch sử xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-time'></i></a> ";
            //   string lichsu = " <a href=\"javascript:void()\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Taikhoan_lichsu')\" class='trans_func'><i class='icon-time'></i></a> ";
            del = "";
            str += "<tr id='tr_" + id_encr + "'><td nowrap>" + ten + "</td><td>" + chucvu + "</td><td>" + thongtin +
                "</td><td>" + Row_Taikhoan_list_nhomquyen(group, t.CARRGROUP) + "</td><td class='tcenter'>" + trangthai + "</td><td class='tcenter' nowrap>" + chitiet + lichsu + cog + edit + del + "</td></tr>";
            return str;
        }
        public string Row_TaiKhoan_PRC(List<THIETLAP_NGUOIDUNG> all, THIETLAP_NGUOIDUNG t, string id_encr)
        {
            string str = "";


            string trangthai = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.USER_STATUS)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Taikhoan_status')\"/>";
            string chucvu = "";
            if (t.USER_TENCHUCVU != null)
            {
                chucvu = t.USER_TENCHUCVU;
            }
            string ten = "<p><strong class='f-red'>" + t.USERNAME + "</strong></p><p>" + t.USER_TEN + "</p>";
            string thongtin = "<p><strong>Email:</strong> " + t.USER_EMAIL + "</p><p><strong>SĐT:</strong>: " + t.USER_SDT + "</p>";
            string cog = "<a href=\"/Thietlap/Taikhoan_phanquyen/?id=" + id_encr + "\" data-original-title='Phân quyền' rel='tooltip' title='' class='trans_func'><i class='icon-cog'></i></a> ";
            //cog = "";
            string chitiet = "<a href=\"javascript:void()\" data-original-title='Xem chi tiết' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Taikhoan_info')\" class='trans_func'><i class='icon-info-sign'></i></a> ";
            string edit = "<a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Taikhoan_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
            string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Taikhoan_del')\" class='trans_func'><i class='icon-trash'></i></a> ";
            string lichsu = " <a href='/Thietlap/Taikhoan_Lichsu/" + id_encr + "' data-original-title='Lịch sử xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-time'></i></a> ";
            //   string lichsu = " <a href=\"javascript:void()\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Taikhoan_lichsu')\" class='trans_func'><i class='icon-time'></i></a> ";
            del = "";
            str += "<tr id='tr_" + id_encr + "'><td nowrap>" + ten + "</td><td>" + chucvu + "</td><td>" + thongtin +
                "</td><td>" + Row_Taikhoan_list_nhomquyen_prc2(all, t) + "</td><td class='tcenter'>" + trangthai + "</td><td class='tcenter' nowrap>" + chitiet + lichsu + cog + edit + del + "</td></tr>";
            return str;
        }
        public string Row_TaiKhoan_Lanhdao(List<THIETLAP_NGUOIDUNG> all, THIETLAP_NGUOIDUNG t, string lanhdao = "")
        {
            string str = "";
            string chucvu = "";
            if (t.USER_TENCHUCVU != null)
            {
                chucvu = " - - - " + t.USER_TENCHUCVU;
            }
            string ten = t.USER_TEN;

            //   string lichsu = " <a href=\"javascript:void()\" data-original-title='Lịch sử xử lý' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Taikhoan_lichsu')\" class='trans_func'><i class='icon-time'></i></a> ";
            str += "<option value=" + t.ID_USER + " " + lanhdao + ">" + ten + " / " + t.USERNAME + " " + chucvu + "</option>";
            return str;
        }
        public string Row_Taikhoan_list_nhomquyen_prc(List<THIETLAP_NGUOIDUNG> all, THIETLAP_NGUOIDUNG t)
        {
            string str = "";

            if (t.USER_NHOM != null)
            {
                foreach (var x in "1,2,3,4,5,6,7,21,".Split(','))
                {
                    if (x != "")
                    {
                        int iGroup = Convert.ToInt32(x);
                        if (all.Where(g => g.IDNHOMQUYEN == iGroup).Count() > 0)
                        {
                            str += all.Where(g => g.IDNHOMQUYEN == iGroup).FirstOrDefault().TENNHOMQUYEN + "</br>";
                        }
                    }
                }
            }
            return str;
        }
        public string Row_Taikhoan_list_nhomquyen_prc2(List<THIETLAP_NGUOIDUNG> all, THIETLAP_NGUOIDUNG t)
        {
            string str = "";

            if (t.USER_NHOM != null)
            {
                string CARRGROUP = t.USER_NHOM;
                foreach (var x in CARRGROUP.Split(','))
                {
                    if (x != "")
                    {
                        int iGroup = Convert.ToInt32(x);
                        if (_thietlap.GetBy_UsergroupID(iGroup) != null)
                        {
                            str += _thietlap.GetBy_UsergroupID(iGroup).CTEN + "</br>";
                        }
                    }
                }
            }
            return str;
        }
        public string Row_Taikhoan_list_nhomquyen(List<USER_GROUP> group, string CARRGROUP)
        {
            string str = "";

            if (CARRGROUP != null)
            {
                foreach (var x in CARRGROUP.Split(','))
                {
                    if (x != "")
                    {
                        int iGroup = Convert.ToInt32(x);
                        if (group.Where(g => g.IGROUP == iGroup).Count() > 0)
                        {
                            str += group.Where(g => g.IGROUP == iGroup).FirstOrDefault().CTEN + "</br>";
                        }
                    }
                }
            }
            return str;
        }
        public string TenKyHop_DaiBieu(int idaibieu,  List<DAIBIEU_KYHOP> listDaiBieuKyHop)
        {
            if (listDaiBieuKyHop == null) return string.Empty;
            string str = "";
            var list = listDaiBieuKyHop.Where(x => x.ID_DAIBIEU == idaibieu).ToList();
            if (list != null && list.Count() > 0)
            {
                foreach (var x in list)
                {
                    var khoa = _thietlap.Get_Quochoi_Khoa((int)x.ID_KYHOP);
                    if(khoa != null)
                    {
                        str += "<p>" + khoa.CTEN + "</p>";
                    }
                }
            }
            return str;
        }
        public string List_Doandaibieu(List<DIAPHUONG> diaphuong, List<THIETLAP_DAIBIEU_PHANTRANG> daibieu, int iUser, string url_cookie = "", string key_search = "", int iKyhop = 0)
        {
            //int iDonVi = Convert.ToInt32(db.users.Single(x => x.iUser.Equals(iUser)).iDonVi);
            var listDaiBieuKyHop = _thietlap.Get_DaiBieu_KyHop();
            string str = "";
            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            var daibieuGroup = daibieu.GroupBy(x => x.IDIAPHUONG);
            if (daibieuGroup != null && daibieuGroup.Count() > 0)
            {
                int count = 1;
                foreach (var dbGroupItem in daibieuGroup)
                {
                    if (dbGroupItem.Count() > 0)
                    {
                        var dv = diaphuong.FirstOrDefault(x => x.IDIAPHUONG == dbGroupItem.Key.Value);
                        if (iKyhop == 0)
                        {
                            str += "<tr><th class='' colspan='10'>";
                            if (dv != null)
                                str += HttpUtility.HtmlEncode(dv.CTEN);
                            str += "</th></tr>";
                        }
                        int kiemtra = 0;
                        foreach (var t in dbGroupItem)
                        {
                            if (iKyhop != 0)
                            {
                                if (listDaiBieuKyHop.Where(p => p.ID_DAIBIEU == t.IDAIBIEU && p.ID_KYHOP == iKyhop).Count() > 0)
                                {
                                    if (kiemtra != t.IDIAPHUONG)
                                    {
                                        str += "<tr><th class='' colspan='10'>" + HttpUtility.HtmlEncode(dv.CTEN) + "</th></tr>";
                                        kiemtra = (int)t.IDIAPHUONG;
                                    }
                                    string id_encr = HashUtil.Encode_ID(t.IDAIBIEU.ToString(), url_cookie);
                                    string thongtin = ""; string truongdoan = "";
                                    if (t.CEMAIL != "") { thongtin += "<strong>Email: </strong>" + t.CEMAIL + "</br>"; }
                                    if (t.CSDT != "") { thongtin += "<strong>SĐT: </strong>" + t.CSDT + "</br>"; }
                                    if (t.ITRUONGDOAN == 1) { truongdoan = " (Trưởng đoàn) "; }
                                    string tenkyhop = TenKyHop_DaiBieu((int)t.IDAIBIEU, listDaiBieuKyHop);
                                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Daibieu_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Daibieu_status')\"/>";
                                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Daibieu_order')\" type=\"text\" value='" + t.IVITRI + "' class='input-block-level tcenter' />";
                                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Daibieu_del','Bạn có chắc xóa Đại biểu " + HttpUtility.HtmlEncode(t.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                                    string ngaysinh = "";
                                    string loaidaibieu = "";
                                    if (t.ILOAIDAIBIEU == 0)
                                        loaidaibieu = "Đại Biểu Quốc Hội";
                                    else
                                        loaidaibieu = "Đại Biểu Hội Đồng Nhân Dân";
                                    string gioitinh = "Nam";
                                    if (t.DNGAYSINH != null)
                                    {
                                        ngaysinh = func.ConvertDateVN(t.DNGAYSINH.ToString());
                                    }
                                    if (t.IGIOITINH != null)
                                    {
                                        if (t.IGIOITINH != 0)
                                        {
                                            gioitinh = "Nữ";
                                        }
                                    }
                                    string noilamviec = "";
                                    if (t.CNOILAMVIEC != null)
                                    {
                                        noilamviec = "/" + t.CNOILAMVIEC;
                                    }
                                    //    string add_kyhop = "<a href=\"javascript:void()\" data-original-title='Thêm  kỳ họp thuộc đại biểu' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Daubieu_add_Kyhop')\" class='trans_func'><i class='icon-list'></i></a>";
                                    str += "<tr><td class='tcenter '>" + count + "</td><td class='tcenter f-red '>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td><strong>" + HttpUtility.HtmlEncode(t.CTEN) + "</strong> " + truongdoan + "<p>" + gioitinh + "</p><p>" + ngaysinh + "</p></td>" +
                                       "<td class='tleft'>" + t.CCOQUAN + " " + noilamviec + "</td><td class='tleft'>" + t.CCHUCVUDAYDU + "</td>" + "</td><td class='tleft'>" + loaidaibieu + "</td>" +
                                        "<td class='tcenter'>" + tenkyhop + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                                    count++;
                                }
                            }
                            else
                            {
                                string tenkyhop = TenKyHop_DaiBieu((int)t.IDAIBIEU, listDaiBieuKyHop);
                                string id_encr = HashUtil.Encode_ID(t.IDAIBIEU.ToString(), url_cookie);
                                string thongtin = ""; string truongdoan = "";
                                if (t.CEMAIL != "") { thongtin += "<strong>Email: </strong>" + t.CEMAIL + "</br>"; }
                                if (t.CSDT != "") { thongtin += "<strong>SĐT: </strong>" + t.CSDT + "</br>"; }
                                if (t.ITRUONGDOAN == 1) { truongdoan = " (Trưởng đoàn) "; }
                                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Daibieu_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Daibieu_status')\"/>";
                                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Daibieu_order')\" type=\"text\" value='" + t.IVITRI + "' class='input-block-level tcenter' />";
                                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Daibieu_del','Bạn có chắc xóa Đại biểu " + HttpUtility.HtmlEncode(t.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                                string ngaysinh = "";
                                string gioitinh = "Nam";
                                string loaidaibieu = "";
                                if (t.ILOAIDAIBIEU == 0)
                                    loaidaibieu = "Đại Biểu Quốc Hội";
                                else
                                    loaidaibieu = "Đại Biểu Hội Đồng Nhân Dân";
                                if (t.DNGAYSINH != null)
                                {
                                    ngaysinh = func.ConvertDateVN(t.DNGAYSINH.ToString());
                                }
                                if (t.IGIOITINH != null)
                                {
                                    if (t.IGIOITINH != 0)
                                    {
                                        gioitinh = "Nữ";
                                    }
                                }
                                string noilamviec = "";
                                if (t.CNOILAMVIEC != null)
                                {
                                    noilamviec = "/" + t.CNOILAMVIEC;
                                }
                                //    string add_kyhop = "<a href=\"javascript:void()\" data-original-title='Thêm  kỳ họp thuộc đại biểu' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Daubieu_add_Kyhop')\" class='trans_func'><i class='icon-list'></i></a>";
                                str += "<tr><td class='tcenter '>" + count + "</td><td class='tcenter f-red '>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td><strong>" + HttpUtility.HtmlEncode(t.CTEN) + "</strong> " + truongdoan + "<p>" + gioitinh + "</p><p>" + ngaysinh + "</p></td>" +
                                       "<td class='tleft'>" + t.CCOQUAN + " " + noilamviec + "</td><td class='tleft'>" + t.CCHUCVUDAYDU + "</td>" + "</td><td class='tleft'>" + loaidaibieu + "</td>" +
                                        "<td class='tcenter'>" + tenkyhop + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                                count++;
                            }
                        }
                    }
                }

            }
            return str;
        }
        public string List_Doandaibieu_search(List<DIAPHUONG> diaphuong, List<THIETLAP_DAIBIEU_PHANTRANG> daibieu, int iUser, string url_cookie = "", int id = 0)
        {
            //int iDonVi = Convert.ToInt32(db.users.Single(x => x.iUser.Equals(iUser)).iDonVi);
            var listDaiBieuKyHop = _thietlap.Get_DaiBieu_KyHop();
            string str = "";
            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            if (id == 0)
            {
                var donvi = diaphuong.Where(x => x.IDELETE == 0).ToList().OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList();

                if (donvi.Count() > 0)
                {
                    foreach (var u in donvi)
                    {
                        var tk = daibieu.Where(v => v.IDIAPHUONG == u.IDIAPHUONG && v.IDELETE == 0).OrderByDescending(p => p.ITRUONGDOAN).OrderBy(x => x.IVITRI).ToList();
                        if (tk.Count > 0)
                        {
                            str += "<tr><th class='' colspan='10'>" + HttpUtility.HtmlEncode(u.CTEN) + "</th></tr>";
                            int count = 1;
                            foreach (var t in tk)
                            {
                                string tenkyhop = TenKyHop_DaiBieu((int)t.IDAIBIEU, listDaiBieuKyHop);
                                string id_encr = HashUtil.Encode_ID(t.IDAIBIEU.ToString(), url_cookie);
                                string thongtin = ""; string truongdoan = "";

                                if (t.CEMAIL != "") { thongtin += "<strong>Email: </strong>" + t.CEMAIL + "</br>"; }
                                if (t.CSDT != "") { thongtin += "<strong>SĐT: </strong>" + t.CSDT + "</br>"; }
                                if (t.ITRUONGDOAN == 1) { truongdoan = " (Trưởng đoàn) "; }
                                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Daibieu_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Daibieu_status')\"/>";
                                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Daibieu_order')\" type=\"text\" value='" + t.IVITRI + "' class='input-block-level tcenter' />";
                                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Daibieu_del','Bạn có chắc xóa tên Đại biểu " + HttpUtility.HtmlEncode(t.CTEN) + " khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                                string ngaysinh = "";
                                string gioitinh = "Nam";
                                string loaidaibieu = "";
                                if (t.ILOAIDAIBIEU == 0)
                                    loaidaibieu = "Đại Biểu Quốc Hội";
                                else
                                    loaidaibieu = "Đại Biểu Hội Đồng Nhân Dân";
                                if (t.DNGAYSINH != null)
                                {
                                    ngaysinh = func.ConvertDateVN(t.DNGAYSINH.ToString());
                                }
                                if (t.IGIOITINH != null)
                                {
                                    if (t.IGIOITINH != 0)
                                    {
                                        gioitinh = "Nữ";
                                    }
                                }
                                string noilamviec = "";
                                if (t.CNOILAMVIEC != null)
                                {
                                    noilamviec = "/" + t.CNOILAMVIEC;
                                }
                                //    string add_kyhop = "<a href=\"javascript:void()\" data-original-title='Thêm  kỳ họp thuộc đại biểu' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Daubieu_add_Kyhop')\" class='trans_func'><i class='icon-list'></i></a>";
                                str += "<tr><td class='tcenter '>" + count + "</td><td class='tcenter f-red '>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td><strong>" + HttpUtility.HtmlEncode(t.CTEN) + "</strong> " + truongdoan + "<p>" + gioitinh + "</p><p>" + ngaysinh + "</p></td>" +
                                      "<td class='tleft'>" + t.CCOQUAN + "  " + noilamviec + "</td><td class='tleft'>" + t.CCHUCVUDAYDU + "</td><td class='tcenter'>" + loaidaibieu + "</td>" +
                                       "<td class='tcenter'>" + tenkyhop + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                                count++;
                            }
                        }
                    }
                }
            }
            else
            {
                var donvi = diaphuong.Where(x => x.IDELETE == 0).ToList().OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList();

                if (donvi.Count() > 0)
                {
                    foreach (var u in donvi)
                    {
                        var tk = daibieu.Where(v => v.IDIAPHUONG == u.IDIAPHUONG && v.IDELETE == 0 && v.IDAIBIEU == id).OrderByDescending(p => p.ITRUONGDOAN).ToList();
                        if (tk.Count > 0)
                        {
                            str += "<tr><th class='' colspan='10'>" + HttpUtility.HtmlEncode(u.CTEN) + "</th></tr>";
                            int count = 1;
                            foreach (var t in tk)
                            {
                                string ngaysinh = "";
                                string gioitinh = "Nam";
                                if (t.DNGAYSINH != null)
                                {
                                    ngaysinh = func.ConvertDateVN(t.DNGAYSINH.ToString());
                                }
                                if (t.IGIOITINH != null)
                                {
                                    if (t.IGIOITINH != 0)
                                    {
                                        gioitinh = "Nữ";
                                    }
                                }
                                string loaidaibieu = "";
                                if (t.ILOAIDAIBIEU == 0)
                                    loaidaibieu = "Đại Biểu Quốc Hội";
                                else
                                    loaidaibieu = "Đại Biểu Hội Đồng Nhân Dân";
                                string tenkyhop = TenKyHop_DaiBieu((int)t.IDAIBIEU, listDaiBieuKyHop);
                                string id_encr = HashUtil.Encode_ID(t.IDAIBIEU.ToString(), url_cookie);
                                string thongtin = ""; string truongdoan = "";
                                if (t.CEMAIL != "") { thongtin += "<strong>Email: </strong>" + t.CEMAIL + "</br>"; }
                                if (t.CSDT != "") { thongtin += "<strong>SĐT: </strong>" + t.CSDT + "</br>"; }
                                if (t.ITRUONGDOAN == 1) { truongdoan = " (Trưởng đoàn) "; }
                                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Daibieu_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Daibieu_status')\"/>";
                                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Daibieu_order')\" type=\"text\" value='" + t.IVITRI + "' class='input-block-level tcenter' />";
                                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Daibieu_del','Bạn có chắc xóa tên Đại biểu " + HttpUtility.HtmlEncode(t.CTEN) + " khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                                string noilamviec = "";
                                if (t.CNOILAMVIEC != null)
                                {
                                    noilamviec = "/" + t.CNOILAMVIEC;
                                }
                                str += "<tr><td class='tcenter '>" + count + "</td><td class='tcenter f-red '>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td><strong>" + HttpUtility.HtmlEncode(t.CTEN) + "</strong> " + truongdoan + "<p>" + gioitinh + "</p><p>" + ngaysinh + "</p></td>" +
                                     "<td class='tleft'>" + t.CCOQUAN + "  " + noilamviec + "</td><td class='tleft'>" + t.CCHUCVUDAYDU + "</td><td class='tcenter'>" + loaidaibieu + "</td>" +
                                      "<td class='tcenter'>" + tenkyhop + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                                count++;
                            }
                        }
                    }
                }
            }

            return str;
        }
        public string Option_Doandaibieu(int iUser_Login)
        {
            string str = "";
            int iDonVi = IDDonVi_User(iUser_Login);
            List<DAIBIEU> donvi = _thietlap.Get_Daibieu().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            //  string url_cookie = func.Get_Url_keycookie();
            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {

                    string id_encr = HashUtil.Encode_ID(t.IDAIBIEU.ToString());
                    str += "<option value=" + id_encr + ">" + HttpUtility.HtmlEncode(t.CTEN) +
                        "</option>";
                }
            }
            return str;
        }
        public string Option_Doandaibieu(List<DIAPHUONG> diaphuong, List<DAIBIEU> daibieu, int iUser, string url_cookie = "", string key_search = "")
        {
            //int iDonVi = Convert.ToInt32(db.users.Single(x => x.iUser.Equals(iUser)).iDonVi);
            string str = "";
            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }

            var donvi = diaphuong.Where(x => x.IDELETE == 0).ToList().OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList();
            if (key_search != "")
            {
                donvi = donvi.Where(x => HttpUtility.HtmlEncode(x.CTEN).Contains(key_search)).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList();
            }
            if (donvi.Count() > 0)
            {
                foreach (var u in donvi)
                {
                    var tk = daibieu.Where(v => v.IDIAPHUONG == u.IDIAPHUONG && v.IDELETE == 0).OrderByDescending(p => p.ITRUONGDOAN).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList();
                    if (tk.Count > 0)
                    {
                        str += "<tr><th class='' colspan='5'>" + HttpUtility.HtmlEncode(u.CTEN) + "</th></tr>";
                        int count = 1;
                        foreach (var t in tk)
                        {
                            string id_encr = HashUtil.Encode_ID(t.IDAIBIEU.ToString(), url_cookie);
                            string thongtin = ""; string truongdoan = "";
                            if (t.CEMAIL != "") { thongtin += "<strong>Email: </strong>" + t.CEMAIL + "</br>"; }
                            if (t.CSDT != "") { thongtin += "<strong>SĐT: </strong>" + t.CSDT + "</br>"; }
                            if (t.ITRUONGDOAN == 1) { truongdoan = " (Trưởng đoàn) "; }
                            string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Daibieu_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                            string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Daibieu_status')\"/>";
                            string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Daibieu_del')\" class='trans_func'><i class='icon-trash'></i></a> ";
                            str += "<tr><td class='tcenter '>" + count + "</td><td><strong>" + HttpUtility.HtmlEncode(t.CTEN) + "</strong> " + truongdoan + "</td><td>" + thongtin +
                                "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                            count++;
                        }
                    }
                }
            }
            return str;
        }
        public string List_Nhom_LinhVuc(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";
            string[] arr = { "Hành chính", "Tư pháp", "Khác" };
            int count = 1;

            foreach (var a in arr)
            {
                var linhvuc_nhom = linhvuc.Where(x => x.INHOM == count).ToList();
                if (linhvuc_nhom.Count() > 0)
                {
                    str += "<tr><th colspan='4'>" + a.ToUpper() + "</th></tr>";
                    str += List_LinhVuc(linhvuc_nhom, 0, 0, iUser, url_cookie);
                }

                count++;
            }
            return str;
        }
        public string Option_Nhom_LinhVuc(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";
            string[] arr = { "Hành chính", "Tư pháp", "Khác" };
            int count = 1;
            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            foreach (var a in arr)
            {
                var linhvuc_nhom = linhvuc.Where(x => x.INHOM == count).OrderBy(x => x.IVITRI).ToList();
                if (linhvuc_nhom.Count() > 0)
                {

                    str += OptionLinhVuc_ThietLap_Search(linhvuc_nhom, 0, 0, iUser, url_cookie);

                }

                count++;
            }
            return str;
        }
        public string Option_Nhom_LinhVuc_Edit(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "", int id = 0)
        {
            string str = "";
            string[] arr = { "Hành chính", "Tư pháp", "Khác" };
            int count = 1;
            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            foreach (var a in arr)
            {
                var linhvuc_nhom = linhvuc.Where(x => x.INHOM == count).ToList();
                if (linhvuc_nhom.Count() > 0)
                {

                    str += OptionLinhVuc_ThietLap_Search_edit(linhvuc_nhom, 0, 0, iUser, url_cookie, id);

                }

                count++;
            }
            return str;
        }
        public string List_LinhVuc_search(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "", int id = 0)
        {
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            if (id == 0)
            {
                str = List_LinhVuc(linhvuc, 0, 0, iUser, url_cookie);
            }
            else
            {
                var donvi = linhvuc.Where(v => v.ILINHVUC == id).OrderBy(t => t.IVITRI).ToList();
                if (donvi.Count() > 0)
                {

                    foreach (var t in donvi)
                    {


                        string id_encr = HashUtil.Encode_ID(t.ILINHVUC.ToString(), url_cookie);
                        string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Linhvuc_status')\"/>";
                        string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                        string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_del','Bạn có chắc muốn xóa lĩnh vực " + HttpUtility.HtmlEncode(t.CTEN) + "')\" class='trans_func'><i class='icon-trash'></i></a> ";
                        string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Linhvuc_order')\" type=\"text\" value='" + t.IVITRI + "' class='input-block-level tcenter' />";
                        str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td class='" + bold_level + "'>" +
                             space_level + HttpUtility.HtmlEncode(t.CTEN) + "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                    }
                }
            }

            return str;
        }
        //public string List_LinhVuc(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        //{
        //    string str = "";
        //    string space_level = "", bold_level = "";
        //    if (level == 0) { bold_level = "  "; }
        //    for (int i = 0; i < level; i++) { space_level += "- - - "; }

        //        var linhvuc1 = linhvuc.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).OrderBy(t => t.ILOAIDON).ToList();
        //        foreach (var t in linhvuc1)
        //        {
        //            string id_encr = HashUtil.Encode_ID(t.ILINHVUC.ToString(), url_cookie);
        //            var thongtinlinhvuc = _thietlap.Get_Linhvuc().Where(v => v.IPARENT == 0 && v.IDELETE == 0).ToList();
        //            string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
        //            string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_LinhVuc('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_del')\" class='trans_func'><i class='icon-trash'></i></a> ";
        //            string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Linhvuc_status')\"/>";
        //            if (t.IPARENT == 0 )
        //            {
        //                if (thongtinlinhvuc.Count() != 0)
        //                {
        //                    edit = "";
        //                    del = "";
        //                }
        //                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" +HttpUtility.HtmlEncode(t.CCODE) + "</td><td class='" + bold_level + "'>" +
        //                space_level + HttpUtility.HtmlEncode(t.CTEN) + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
        //                var thongtinlinhvuc2 = _thietlap.Get_Linhvuc().Where(x => x.IPARENT == t.IPARENT && x.IDELETE == 0).ToList();
        //                int gan = 0;
        //                if (gan != t.ILOAIDON)
        //                {
        //                    str += "<tr><td colspan='4'>" + _thietlap.GetBy_LoaidonID((int)t.ILOAIDON).CTEN + "</td></tr>";
        //                    gan = (int)t.ILOAIDON;
        //                }

        //                str += List_LinhVuc(thongtinlinhvuc2, (int)t.IPARENT, level + 1, iUser, url_cookie);
        //            }



        //        }
        //    return str;
        //}
        public string List_LinhVuc(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";


            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = " b "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }

            var linhvuc1 = linhvuc.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            foreach (var t in linhvuc1)
            {
                var thongtinlinhvuc = _thietlap.Get_Linhvuc().Where(v => v.IPARENT == 0 && v.IDELETE == 0).ToList();
                string id_encr = HashUtil.Encode_ID(t.ILINHVUC.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Linhvuc_status')\"/>";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = "";
                List<decimal> linhVucNoDelete = new List<decimal>() { 45, 47, 49, 50, 51, 53 };
                if (!linhVucNoDelete.Contains(t.ILINHVUC))
                {
                    del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_del','Bạn có muốn xóa lĩnh vực?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                }
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Linhvuc_order')\" type=\"text\" value='" + t.IVITRI + "' class='input-block-level tcenter' />";
                string add_donvi = "<a href=\"javascript:void()\" data-original-title='Thêm đơn vị thuộc lĩnh vực' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_add_dv')\" class='trans_func'><i class='icon-list'></i></a>";
                if (t.IPARENT == 0)
                {
                    add_donvi = "";
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b f-red'>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td class='" + bold_level + "'>" +
                     space_level + HttpUtility.HtmlEncode(t.CTEN) + "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + add_donvi + edit + del + "</td></tr>";
                str += List_LinhVuc(linhvuc, (int)t.ILINHVUC, level + 1, iUser, url_cookie);
            }
            return str;
        }

        public string OptionLinhVuc_ThietLap(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var linhvuc1 = linhvuc.Where(x => x.IPARENT == id_parent && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var t in linhvuc1)
            {
                string id_encr = HashUtil.Encode_ID(t.ILINHVUC.ToString());
                str += "<option value=" + id_encr + ">" + space_level + HttpUtility.HtmlEncode(t.CTEN) + "</option>";
                str += OptionLinhVuc_ThietLap(linhvuc, (int)t.ILINHVUC, level + 1, iUser, url_cookie);
            }
            return str;
        }
        public string OptionLinhVuc_ThietLap_Search(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var linhvuc1 = linhvuc.Where(x => x.IPARENT == id_parent && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var t in linhvuc1)
            {
                string id_encr = HashUtil.Encode_ID(t.ILINHVUC.ToString());
                str += "<option value=" + t.ILINHVUC + "> - - - " + space_level + HttpUtility.HtmlEncode(t.CTEN) + "</option>";
                str += OptionLinhVuc_ThietLap_Search(linhvuc, (int)t.ILINHVUC, level + 1, iUser, url_cookie);
            }
            return str;
        }

        public string OptionLinhVuc_ThietLap_Search_edit(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "", int id = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var linhvuc1 = linhvuc.Where(x => x.IPARENT == id_parent && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var t in linhvuc1)
            {
                string sel = "";
                if (id == t.ILINHVUC)
                {
                    sel = "selected";
                }
                string id_encr = HashUtil.Encode_ID(t.ILINHVUC.ToString());
                str += "<option value=" + t.ILINHVUC + " " + sel + "> - - - " + space_level + HttpUtility.HtmlEncode(t.CTEN) + "</option>";
                str += OptionLinhVuc_ThietLap_Search_edit(linhvuc, (int)t.ILINHVUC, level + 1, iUser, url_cookie);
                sel = "";
            }
            return str;
        }
        public string List_LoaiDon(List<KNTC_LOAIDON> loaidon, int iUser)
        {
            string str = "";
            var diaban = loaidon.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in diaban)
            {
                string id_encr = HashUtil.Encode_ID(x.ILOAIDON.ToString(), url_cookie);
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Loaidon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Loaidon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Loaidon_del','Bạn có chắc xóa loại đơn " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                if (x.ILOAIDON == 1 || x.ILOAIDON == 2 || x.ILOAIDON == 3)
                {
                    edit = "";
                    del = "";
                    hienthi = "";
                }

                count++;
                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='b tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            return str;
        }

        public string List_Nhom_Noidungdon(List<LINHVUC> linhvuc, List<KNTC_NOIDUNGDON> noidungdon, int iUser = 0, string url_cookie = "")
        {

            string str = "";
            var thongtinlinhvucparent = linhvuc.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            var listLoaiDon = _thietlap.Get_Loaidon();
            int count2 = 1;
            foreach(var loaiDon in listLoaiDon)
            {
                str += "<tr><th colspan='6'>" + loaiDon.CTEN + "</th></tr>";
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("ILOAIDON", loaiDon.ILOAIDON);
                var listLinhVuc = _thietlap.GetBy_List_Linhvuc(dict);
                foreach(var linhVuc in listLinhVuc )
                {
                    str += "<tr><th colspan='6'>" + " - - - " + linhVuc.CTEN + "</th></tr>";
                    dict = new Dictionary<string, object>();
                    dict.Add("ILINHVUC", linhVuc.ILINHVUC);
                    var listNoiDungDon = _thietlap.GetBy_List_Noidungdon(dict);
                    int count = 0;
                    foreach (var noiDungDon in listNoiDungDon)
                    {
                        string id_encr = HashUtil.Encode_ID(noiDungDon.INOIDUNG.ToString(), url_cookie);
                        string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(noiDungDon.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Noidungdon_status')\"/>";
                        string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Noidungdon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                        string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Noidungdon_del','Bạn có muốn xóa nội dung?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                        string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Noidungdon_order')\" type=\"text\" value='" + noiDungDon.IVITRI + "' class='input-block-level tcenter' />";
                        count++;
                        str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(noiDungDon.CCODE) + "</td><td > - - -  " + HttpUtility.HtmlEncode(noiDungDon.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                    }
                }
            }
         




            return str;
        }
        public string Option_Nhom_Noidungdon(List<LINHVUC> linhvuc, List<KNTC_NOIDUNGDON> noidungdon, int iUser = 0, string url_cookie = "", int id_nhom = 0)
        {
            string str = "";
            var linhvuc_nhom = linhvuc.Where(x => x.IDELETE == 0).ToList();
            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            foreach (var a in linhvuc_nhom)
            {
                var nhomnoidung = noidungdon.Where(v => v.ILINHVUC == a.ILINHVUC && v.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();

                if (nhomnoidung.Count() > 0)
                {
                    str += "<optgroup label='" + a.CTEN + "' >";
                    var diaban = noidungdon.Where(x => x.ILINHVUC == a.ILINHVUC && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                    foreach (var x in diaban)
                    {
                        string sel = "";
                        if (id_nhom == x.INOIDUNG)
                        {
                            sel = "selected";
                        }
                        string id_encr = HashUtil.Encode_ID(x.INOIDUNG.ToString(), url_cookie);
                        str += "<option value='" + x.INOIDUNG + "' " + sel + ">  - - - " + HttpUtility.HtmlEncode(x.CTEN) + " </option>";
                        sel = "";
                    }
                    str += "</optgroup>";
                }
            }
            return str;
        }

        public string List_Noidungdon(List<KNTC_NOIDUNGDON> noidungdon, int iUser, string url_cookie)
        {
            string str = "";
            var diaban = noidungdon.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList(); int count = 0;

            foreach (var x in diaban)
            {
                string id_encr = HashUtil.Encode_ID(x.INOIDUNG.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Noidungdon_status')\"/>";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Noidungdon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm(" + id_encr + ",'id=" + id_encr + "','/Thietlap/Ajax_Noidungdon_del','Bạn có muốn xóa nội dung đơn " + HttpUtility.HtmlEncode(x.CTEN) + "')\" class='trans_func'><i class='icon-trash'></i></a> ";

                count++;
                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            return str;
        }

        public string List_Tinhchatdon(List<KNTC_TINHCHAT> tinhchat, int iUser)
        {
            string str = "";
            var diaban = tinhchat.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList(); int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in diaban)
            {
                string id_encr = HashUtil.Encode_ID(x.ITINHCHAT.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Tinhchatdon_status')\"/>";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Tinhchatdon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm(" + x.ITINHCHAT + ",'id=" + x.ITINHCHAT + "','/Thietlap/Ajax_Tinhchatdon_del','Bạn có muốn xóa tính chất đơn " + HttpUtility.HtmlEncode(x.CTEN) + "')\" class='trans_func'><i class='icon-trash'></i></a> ";

                //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                del = "";

                count++;
                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            return str;
        }
        public string List_Nhom_TinhChatDon(List<LINHVUC> linhvuc, List<KNTC_NOIDUNGDON> noidungdon, List<KNTC_TINHCHAT> tinhchat, int iUser = 0, string url_cookie = "")
        {
            string str = "";
            var thongtinlinhvucparent = linhvuc.Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var l in thongtinlinhvucparent)
            {
                var nhomlinhvuc = linhvuc.Where(x => x.IDELETE == 0 && x.IPARENT == l.ILINHVUC).ToList();
                if (nhomlinhvuc.Count() > 0)
                {
                    str += "<tr><th colspan='6'><strong> " + l.CTEN + "<strong></th></tr>";
                }
                foreach (var t in nhomlinhvuc)
                {
                    var noidungdon_nhom = noidungdon.Where(x => x.IDELETE == 0 && x.ILINHVUC == t.ILINHVUC).ToList();

                    if (noidungdon_nhom.Count() > 0)
                    {
                        str += "<tr><th colspan='6'>- - - " + HttpUtility.HtmlEncode(t.CTEN) + "</th></tr>";
                        foreach (var a in noidungdon_nhom)
                        {
                            var tinhchat_nhom = tinhchat.Where(v => v.INHOMNOIDUNG == a.INOIDUNG && v.IDELETE == 0).ToList();

                            if (tinhchat_nhom.Count() > 0)
                            {

                                str += "<tr><th colspan='6'> - - - - - -" + a.CTEN + "</th></tr>";
                                var diaban = tinhchat.Where(v => v.INHOMNOIDUNG == a.INOIDUNG && v.IDELETE == 0).OrderBy(x => x.IVITRI).ToList(); int count = 0;
                                foreach (var x in diaban)
                                {
                                    string id_encr = HashUtil.Encode_ID(x.ITINHCHAT.ToString(), url_cookie);
                                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Tinhchatdon_status')\"/>";
                                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Tinhchatdon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Tinhchatdon_del','Bạn có chắc xóa tính chất đơn " + HttpUtility.HtmlEncode(t.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Tinhchatdon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";

                                    count++;
                                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>- - - - - - - - -  " + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                                }
                            }
                        }

                    }
                }

            }



            return str;
        }

        public string Option_Nhom_TinhChatDon(List<KNTC_NOIDUNGDON> noidungdon, List<KNTC_TINHCHAT> tinhchat, int iUser = 0, string url_cookie = "")
        {
            string str = "";
            var noidungdon_nhom = noidungdon.Where(x => x.IDELETE == 0).ToList();
            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            foreach (var a in noidungdon_nhom)
            {
                var tinhchat_nhom = tinhchat.Where(v => v.INHOMNOIDUNG == a.INOIDUNG && v.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();

                if (tinhchat_nhom.Count() > 0)
                {
                    str += "<optgroup>" + a.CTEN + "</optgroup>";
                    var diaban = tinhchat.Where(v => v.INHOMNOIDUNG == a.INOIDUNG && v.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                    foreach (var x in diaban)
                    {
                        string id_encr = HashUtil.Encode_ID(x.ITINHCHAT.ToString(), url_cookie);

                        str += "<option value='" + id_encr + "'> - - -  " + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
                    }
                }
            }
            return str;
        }
        public string List_Nguondon(List<KNTC_NGUONDON> nguondon, int iUser)
        {
            string str = "";
            var diaban = nguondon.OrderBy(x => x.IVITRI).Where(x => x.IDELETE == 0).ToList();
            int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in diaban)
            {
                string id_encr = HashUtil.Encode_ID(x.INGUONDON.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nguondon_status')\"/>";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Nguondon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nguondon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Nguondon_del','Bạn có muốn xóa nguồn đơn " + HttpUtility.HtmlEncode(x.CTEN) + "')\" class='trans_func'><i class='icon-trash'></i></a> ";
                //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }


                count++;
                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            return str;
        }
        public string List_Diaphuong(List<DIAPHUONG> diaphuong, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var donvi = diaphuong.Where(x => x.IPARENT == id_parent && x.IDELETE == 0).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList();

            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {
                    string id_encr = HashUtil.Encode_ID(t.IDIAPHUONG.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Diaphuong_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_del','Bạn có chắc xóa địa phương " + HttpUtility.HtmlEncode(t.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";


                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td class='" + bold_level + "'>" +
                       space_level + HttpUtility.HtmlEncode(t.CTEN) + "</td><td class='tcenter' nowrap>" + t.CTYPE + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                    str += List_Diaphuong(diaphuong, (int)t.IDIAPHUONG, level + 1, iUser, url_cookie);
                }
            }
            return str;
        }
        public string Option_Diaphuong(List<DIAPHUONG> diaphuong, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }

            var donvi = diaphuong.Where(x => x.IPARENT == id_parent && x.IDELETE == 0).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList();
            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {

                    string id_encr = HashUtil.Encode_ID(t.IDIAPHUONG.ToString());
                    str += "<option value=" + id_encr + ">" +
                       space_level + HttpUtility.HtmlEncode(t.CTEN) + "<option>";
                    str += Option_Diaphuong(diaphuong.Where(x => x.IDELETE == 0).ToList(), (int)t.IDIAPHUONG, level + 1, iUser, url_cookie);
                }
            }
            return str;
        }

        public string Option_Diaphuong_PhanTrang(List<THIETLAP_DIAPHUONG_PHANTRANG> thongtin)
        {
            string str = "";
            string url_cookie = func.Get_Url_keycookie();
            decimal icoquan = 0;
            foreach (var x in thongtin)
            {
                string id_encr = HashUtil.Encode_ID(x.IDIAPHUONG_PARENT.ToString(), url_cookie);

                if (icoquan != x.IDIAPHUONG_PARENT)
                {
                    str += "<option value=" + id_encr + ">"
                     + HttpUtility.HtmlEncode(x.CTENDIAPHUONG_PARENT) + "<option>";
                    id_encr = HashUtil.Encode_ID(x.IDIAPHUONG.ToString(), url_cookie);
                    str += "<option value=" + id_encr + ">- - - "
                       + HttpUtility.HtmlEncode(x.CTENDIAPHUONG) + "<option>";
                    icoquan = x.IDIAPHUONG_PARENT;

                }
                else
                {
                    id_encr = HashUtil.Encode_ID(x.IDIAPHUONG.ToString(), url_cookie);
                    str += "<option value=" + id_encr + ">- - - "
                       + HttpUtility.HtmlEncode(x.CTENDIAPHUONG) + "<option>";
                }
            }
            return str;
        }
        public string List_Nghenghiep(List<NGHENGHIEP> nghenghiep, int iUser)
        {
            string str = "";

            int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in nghenghiep.Where(x => x.IDELETE == 0).OrderBy(v => v.IVITRI))
            {
                string id_encr = HashUtil.Encode_ID(x.INGHENGHIEP.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nghenghiep_status')\"/>";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nghenghiep_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Nghenghiep_del','Bạn có chắc xóa nghề nghiệp " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Nghenghiep_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";

                count++;
                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            return str;
        }
        public string List_Quoctich(List<QUOCTICH> quoctich, int iUser)
        {
            string str = "";
            int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var x in quoctich.Where(x => x.IDELETE == 0).OrderBy(v => v.IVITRI))
            {
                string id_encr = HashUtil.Encode_ID(x.IQUOCTICH.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Quoctich_status')\"/>";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Quoctich_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Quoctich_del','Bạn có chắc xóa quốc tịch " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Quoctich_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";

                count++;
                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            return str;
        }
        public string List_Dantoc(List<DANTOC> dantoc, int iUser)
        {
            string str = "";
            int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            var listdantoc = dantoc.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var x in listdantoc)
            {
                string id_encr = HashUtil.Encode_ID(x.IDANTOC.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Dantoc_status')\"/>";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Dantoc_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Dantoc_del','Bạn có chắc xóa Dân tộc " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Dantoc_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";

                count++;
                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            return str;
        }
        public string Lichsu_Taikhoan(int iUser)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IUSER", iUser);
            var lichsu = _thietlap.GetBy_List_Tracking(_condition).OrderByDescending(x => x.ID).ToList();

            foreach (var x in lichsu)
            {
                int iDon = (int)x.IDON;
                int iTongHop = (int)x.ITONGHOP;
                int iKienNghi = (int)x.IKIENNGHI;
                string don = "";
                if (iDon != 0)
                {
                    don = "</br><em class='f-grey'>Đơn KNTC: <a href='javascript:void()' onclick=\"ShowPopUp('id=" + iDon + "','/KNTC/Ajax_Don_info')\">Xem chi tiết</a></em>";
                }
                if (iKienNghi != 0)
                {
                    don = "</br><em class='f-grey'>Kiến nghị cử tri: <a href='javascript:void()' onclick=\"ShowPopUp('id=" + iKienNghi + "','/Kiennghi/Ajax_Kiennghi_info')\">Xem chi tiết</a></em>";
                }
                if (iTongHop != 0)
                {
                    don = "</br><em class='f-grey'>Tổng hợp KN cử tri: <a href='javascript:void()' onclick=\"ShowPopUp('id=" + iDon + "','/Kiennghi/Ajax_Tonghop_info')\">Xem chi tiết</a></em>";
                }
                str += "<tr><td class='tcenter f-red' nowrap>" + String.Format("{0:dd/MM/yyyy HH:mm}", x.DDATE) +
                    "</td><td class=''>" + x.CACTION + don + "</td></tr>";

            }
            return str;
        }

        public string List_LoaiDon_search(List<KNTC_LOAIDON> loaidon, int iUser, int id, string url_cookie)
        {
            string str = "";
            var diaban = loaidon.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            int count = 0;
            if (id == 0)
            {
                foreach (var x in diaban)
                {

                    string id_encr = HashUtil.Encode_ID(x.ILOAIDON.ToString(), url_cookie);
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Loaidon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Loaidon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Loaidon_del','Bạn có chắc xóa loại đơn " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    count++;
                    if (x.ILOAIDON == 1 || x.ILOAIDON == 2 || x.ILOAIDON == 3)
                    {
                        edit = "";
                        del = "";
                        hienthi = "";
                    }
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";

                }
            }
            foreach (var x in diaban)
            {
                if (x.ILOAIDON == id)
                {

                    string id_encr = HashUtil.Encode_ID(x.ILOAIDON.ToString(), url_cookie);
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Loaidon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Loaidon_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Loaidon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Loaidon_del','Bạn có chắc xóa loại đơn " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    if (x.ILOAIDON == 1 || x.ILOAIDON == 2 || x.ILOAIDON == 3)
                    {
                        edit = "";
                        del = "";
                        hienthi = "";
                    }
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }
            }
            return str;
        }
        public string List_Noidungdon_search(List<LINHVUC> linhvuc, List<KNTC_NOIDUNGDON> noidungdon, int iUser, int id, string url_cookie)
        {
            string str = "";
            int count = 0;
            if (id == 0)
            {
                var linhvuc_nhom = linhvuc.Where(x => x.IDELETE == 0).ToList();
                if (url_cookie == "")
                {
                    url_cookie = func.Get_Url_keycookie();
                }
                str = List_Nhom_Noidungdon(linhvuc, noidungdon, iUser, url_cookie);
            }
            else
            {
                var linhvuc_nhom = linhvuc.Where(x => x.IDELETE == 0).ToList();
                if (url_cookie == "")
                {
                    url_cookie = func.Get_Url_keycookie();
                }
                foreach (var a in linhvuc_nhom)
                {
                    var nhomnoidung = noidungdon.Where(v => v.ILINHVUC == a.ILINHVUC && v.IDELETE == 0 && v.INOIDUNG == id).ToList();

                    if (nhomnoidung.Count() > 0)
                    {
                        str += "<tr><th colspan='6'>" + a.CTEN + "</th></tr>";
                        var diaban = noidungdon.Where(x => x.ILINHVUC == a.ILINHVUC && x.IDELETE == 0 && x.INOIDUNG == id).OrderBy(x => x.IVITRI).ToList();
                        foreach (var x in diaban)
                        {
                            string id_encr = HashUtil.Encode_ID(x.INOIDUNG.ToString(), url_cookie);
                            string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Noidungdon_status')\"/>";
                            string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Noidungdon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                            string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm(" + id_encr + ",'id=" + id_encr + "','/Thietlap/Ajax_Noidungdon_del','Bạn có muốn xóa nội dung?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                            string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Noidungdon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                            count++;
                            str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red' >" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                        }
                    }
                }
            }

            return str;
        }

        public string List_Nhom_TinhChatDon_Search(List<KNTC_NOIDUNGDON> noidungdon, List<KNTC_TINHCHAT> tinhchat, int iUser = 0, string url_cookie = "", int id = 0)
        {
            string str = "";
            if (id == 0)
            {
                var noidungdon_nhom = noidungdon.Where(x => x.IDELETE == 0).ToList();
                if (url_cookie == "")
                {
                    url_cookie = func.Get_Url_keycookie();
                }
                foreach (var a in noidungdon_nhom)
                {
                    var tinhchat_nhom = tinhchat.Where(v => v.INHOMNOIDUNG == a.INOIDUNG && v.IDELETE == 0).ToList();

                    if (tinhchat_nhom.Count() > 0)
                    {
                        str += "<tr><th colspan='4'>" + a.CTEN + "</th></tr>";
                        var diaban = tinhchat.Where(v => v.INHOMNOIDUNG == a.INOIDUNG && v.IDELETE == 0).OrderBy(x => x.IVITRI).ToList(); int count = 0;
                        foreach (var x in diaban)
                        {
                            string id_encr = HashUtil.Encode_ID(x.ITINHCHAT.ToString(), url_cookie);
                            string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Tinhchatdon_status')\"/>";
                            string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Tinhchatdon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                            string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Tinhchatdon_del','Ban có muốn xóa tính chất đơn " + HttpUtility.HtmlEncode(x.CTEN) + " khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";


                            count++;
                            str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                        }
                    }
                }
            }
            else
            {
                var noidungdon_nhom = noidungdon.Where(x => x.IDELETE == 0).ToList();
                if (url_cookie == "")
                {
                    url_cookie = func.Get_Url_keycookie();
                }
                foreach (var a in noidungdon_nhom)
                {
                    var tinhchat_nhom = tinhchat.Where(v => v.INHOMNOIDUNG == a.INOIDUNG && v.IDELETE == 0).ToList();
                    if (tinhchat_nhom.Count() > 0)
                    {
                        str += "<tr><th colspan='4'>" + a.CTEN + "</th></tr>";
                        var diaban = tinhchat.Where(v => v.IDELETE == 0 && v.ITINHCHAT == id).OrderBy(x => x.IVITRI).ToList(); int count = 0;
                        foreach (var x in diaban)
                        {
                            if (id == x.ITINHCHAT)
                            {
                                string id_encr = HashUtil.Encode_ID(x.ITINHCHAT.ToString(), url_cookie);
                                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Tinhchatdon_status')\"/>";
                                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Tinhchatdon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Tinhchatdon_del','Bạn có muốn xóa tính chất đơn " + HttpUtility.HtmlEncode(x.CTEN) + " khỏi danh sách? ')\" class='trans_func'><i class='icon-trash'></i></a> ";
                                count++;
                                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=''" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                            }

                        }
                    }
                }
            }
            return str;
        }
        public string List_Tinhchatdon_search(int iUser, int id, string url_cookie)
        {
            string str = "";
            var tinhchat = _thietlap.Get_Tinhchat().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList(); int count = 0;
            if (id == 0)
            {
                str = List_Nhom_TinhChatDon(_thietlap.Get_Linhvuc(), _thietlap.Get_Noidungdon(), tinhchat, iUser, url_cookie);
            }
            foreach (var x in tinhchat)
            {
                if (x.ITINHCHAT == id)
                {
                    string id_encr = HashUtil.Encode_ID(x.ITINHCHAT.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Tinhchatdon_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Tinhchatdon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Tinhchatdon_del','Bạn có chắc xóa tính chất đơn " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    //if (db.tbl_duan.Where(t => t.iLinhVuc == x.iLinhVuc).Count() > 0) { del = ""; }
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Tinhchatdon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=''>" + order + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }

            }
            return str;
        }
        public string List_Nguondon_Search(List<KNTC_NGUONDON> nguondon, int iUser, int id, string url_cookie)
        {
            string str = "";
            var diaban = nguondon.OrderBy(x => x.IVITRI).ToList();
            int count = 0;
            if (id == 0)
            {
                foreach (var x in diaban)
                {

                    string id_encr = HashUtil.Encode_ID(x.INGUONDON.ToString(), url_cookie);
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Nguondon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nguondon_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nguondon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Nguondon_del','Bạn có muốn xóa " + HttpUtility.HtmlEncode(x.CTEN) + " khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";

                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }
            }
            foreach (var x in diaban)
            {
                if (id == x.INGUONDON)
                {

                    string id_encr = HashUtil.Encode_ID(x.INGUONDON.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nguondon_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nguondon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Nguondon_del','Bạn có muốn xóa " + HttpUtility.HtmlEncode(x.CTEN) + " khỏi danh sách? ')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Nguondon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class=' tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }
            }
            return str;
        }
        public string List_Diaphuong_search(List<DIAPHUONG> diaphuong, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "", int id = 0)
        {
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            _condition = new Dictionary<string, object>();
            _condition.Add("IDIAPHUONG", id);
            if (id == 0)
            {
                var thongtindiaphuong = thietlaplist.THIETLAPDIAPHUONG_PHANTRANG("PKG_THIETLAP_HETHONG.PKG_DIAPHUONG_PHANTRANG", "", 1, 20).ToList();
                //var thongtin = thutuc.THIETLAPDIAPHUONG("PKG_THIETLAP_HETHONG.PKG_DIAPHUONG").ToList();
                str = DIAPHUONGTHIETLAP(thongtindiaphuong);
                //str = List_Diaphuong(diaphuong, 0, 0, iUser, url_cookie);
            }
            else
            {
                var donvi = _thietlap.GetBy_List_Diaphuong(_condition).Where(x => x.IDELETE == 0).OrderBy(t => HttpUtility.HtmlEncode(t.CTEN)).ToList();
                //  var listdiaphuong = _thietlap.Get_Diaphuong().ToList();
                if (donvi.Count() > 0)
                {


                    foreach (var t in donvi)
                    {

                        string id_encr = HashUtil.Encode_ID(t.IDIAPHUONG.ToString(), url_cookie);
                        string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Diaphuong_status')\"/>";
                        string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                        string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_del','Bạn có chắc xóa địa phương " + HttpUtility.HtmlEncode(t.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";


                        str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td class='" + bold_level + "'>" +
                           space_level + HttpUtility.HtmlEncode(t.CTEN) + "</td><td class='tcenter' nowrap>" + t.CTYPE + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                        if (t.IPARENT == 0)
                        {
                            _condition = new Dictionary<string, object>();
                            _condition.Add("IPARENT", id);
                            var donvi2 = _thietlap.GetBy_List_Diaphuong(_condition).OrderBy(v => HttpUtility.HtmlEncode(v.CTEN)).ToList();
                            foreach (var v in donvi2)
                            {
                                id_encr = HashUtil.Encode_ID(v.IDIAPHUONG.ToString(), url_cookie);
                                hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(v.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Diaphuong_status')\"/>";
                                edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                                del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_del','Bạn có chắc xóa địa phương " + HttpUtility.HtmlEncode(t.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";


                                str += "<tr id='tr_" + id_encr + "'><td class='tcenter f-red'>" + HttpUtility.HtmlEncode(v.CCODE) + "</td><td>" +
                                  "- - - " + HttpUtility.HtmlEncode(v.CTEN) + "</td><td class='tcenter' nowrap>" + v.CTYPE + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                            }

                        }
                    }
                }
            }

            return str;
        }
        public string List_Nghenghiep_search(List<NGHENGHIEP> nghenghiep, int iUser, int id, string url_cookie)
        {
            string str = "";

            int count = 0;
            var list_nghe = nghenghiep.Where(x => x.IDELETE == 0).ToList();
            if (id == 0)
            {
                foreach (var x in list_nghe)
                {
                    string id_encr = HashUtil.Encode_ID(x.INGHENGHIEP.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nghenghiep_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nghenghiep_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Nghenghiep_del','Bạn có chắc xóa nghề nghiệp " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Nghenghiep_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }
            }
            foreach (var x in list_nghe)
            {
                if (id == x.INGHENGHIEP)
                {
                    string id_encr = HashUtil.Encode_ID(x.INGHENGHIEP.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nghenghiep_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nghenghiep_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Nghenghiep_del','Bạn có chắc xóa nghề nghiệp " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Nghenghiep_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";

                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }
            }
            return str;
        }
        public string List_Quoctich_search(List<QUOCTICH> quoctich, int iUser, int id, string url_cookie)
        {
            string str = "";
            int count = 0;
            if (id == 0)
            {
                foreach (var x in quoctich.Where(x => x.IDELETE == 0).OrderBy(v => v.IVITRI))
                {
                    string id_encr = HashUtil.Encode_ID(x.IQUOCTICH.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Quoctich_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Quoctich_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Quoctich_del','Bạn có chắc xóa Quốc tịch " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Quoctich_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";

                }

            }
            foreach (var x in quoctich.OrderBy(v => v.IVITRI))
            {
                if (id == x.IQUOCTICH)
                {
                    string id_encr = HashUtil.Encode_ID(x.IQUOCTICH.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Quoctich_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Quoctich_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Quoctich_del','Bạn có chắc xóa Quốc tịch " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Quoctich_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }
            }
            return str;
        }
        public string List_Vanban(List<VB_LOAI> loaivb, int iUser)
        {
            string str = "";
            int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            var loaivb_nhom = loaivb.Where(x => x.IDELETE == 0).ToList();
            foreach (var x in loaivb_nhom)
            {
                string id_encr = HashUtil.Encode_ID(x.ILOAI.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Vanban_status')\"/>";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Vanban_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Vanban_del','Bạn có chắc xóa văn bản " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Vanban_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";

                count++;
                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            return str;
        }
        public string List_Vanban_search(int iUser, int id, string url_cookie)
        {
            string str = "";
            var vanban = _thietlap.Get_Loaivanban().Where(x => x.IDELETE == 0).ToList();
            int count = 0;
            if (id == 0)
            {
                foreach (var x in vanban)
                {

                    string id_encr = HashUtil.Encode_ID(x.ILOAI.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Vanban_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Vanban_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Vanban_del','Bạn có chắc xóa văn bản " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Vanban_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";

                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";


                }
            }
            foreach (var x in vanban)
            {
                if (id == x.ILOAI)
                {
                    string id_encr = HashUtil.Encode_ID(x.ILOAI.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Vanban_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Vanban_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Vanban_del','Bạn có chắc xóa văn bản " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Vanban_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";

                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";

                }

            }
            return str;
        }
        public string List_Dantoc_search(List<DANTOC> dantoc, int iUser, int id, string url_cookie)
        {
            string str = "";
            int count = 0;
            var list_dantoc = dantoc.Where(x => x.IDELETE == 0).ToList();
            if (id == 0)
            {
                foreach (var x in list_dantoc)
                {
                    string id_encr = HashUtil.Encode_ID(x.IDANTOC.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Dantoc_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Dantoc_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Dantoc_del','Bạn có chắc xóa Dân tộc " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Dantoc_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }
            }
            foreach (var x in list_dantoc)
            {
                if (id == x.IDANTOC)
                {
                    string id_encr = HashUtil.Encode_ID(x.IDANTOC.ToString(), url_cookie);
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Dantoc_status')\"/>";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Dantoc_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Dantoc_del','Bạn có chắc xóa Dân tộc " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Dantoc_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";

                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }
            }
            return str;
        }
        public string Option_LoaiDon_(int id_chucvu = 0)
        {

            string str = "";
            _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IHIENTHI", 1);
            List<KNTC_LOAIDON> chucvu = _thietlap.Get_Loaidon().Where(x => x.IDELETE == 0).ToList();
            foreach (var p in chucvu)
            {
                string id_encr = HashUtil.Encode_ID(p.ILOAIDON.ToString());
                string select = ""; if (p.ILOAIDON == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + id_encr + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_NoiDungDon_(int id_chucvu = 0)
        {

            string str = "";
            _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IHIENTHI", 1);
            List<KNTC_NOIDUNGDON> chucvu = _thietlap.GetBy_List_Noidungdon(_dCondition).Where(x => x.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            foreach (var p in chucvu)
            {
                string id_encr = HashUtil.Encode_ID(p.INOIDUNG.ToString());
                string select = ""; if (p.INOIDUNG == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + id_encr + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_TinhChatDon_(int id_chucvu = 0)
        {

            string str = "";
            _dCondition = new Dictionary<string, object>();
            _dCondition.Add("IHIENTHI", 1);
            List<KNTC_TINHCHAT> chucvu = _thietlap.Get_Tinhchat().Where(x => x.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            foreach (var p in chucvu)
            {
                string id_encr = HashUtil.Encode_ID(p.ITINHCHAT.ToString());
                string select = ""; if (p.ITINHCHAT == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + id_encr + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_Nguondon_(int iUser, int idnguondon = 0)
        {


            string str = "";
            var diaban = _thietlap.Get_Nguondon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            string sel = "";
            foreach (var x in diaban)
            {
                string id_encr = HashUtil.Encode_ID(x.INGUONDON.ToString());
                if (idnguondon == x.INGUONDON && idnguondon != 0)
                {
                    sel = "selected";
                }
                str += "<option value='" + id_encr + "' " + sel + " >" + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_Nguonkiennghi_(int iUser, int idnguondon = 0)
        {


            string str = "";
            var diaban = _thietlap.Get_Nguonkiennghi().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            string sel = "";
            foreach (var x in diaban)
            {
                string id_encr = HashUtil.Encode_ID(x.INGUONDON.ToString());
                if (idnguondon == x.INGUONDON && idnguondon != 0)
                {
                    sel = "selected";
                }
                str += "<option value='" + id_encr + "' " + sel + " >" + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
            }
            return str;
        }

        public string Option_Nghenghiep_(int iUser)
        {


            string str = "";
            var nghenghiep = _thietlap.Get_Nghenghiep().Where(x => x.IDELETE == 0).ToList();
            // int count = 0;

            foreach (var x in nghenghiep)
            {
                string id_encr = HashUtil.Encode_ID(x.INGHENGHIEP.ToString());

                str += "<option value='" + id_encr + "'> - - - " + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_Quoctich_(int iUser)
        {


            string str = "";
            var quoctich = _thietlap.Get_Quoctich().Where(x => x.IDELETE == 0);
            // int count = 0;

            foreach (var x in quoctich)
            {
                string id_encr = HashUtil.Encode_ID(x.IQUOCTICH.ToString());

                str += "<option value='" + id_encr + "'> - - - " + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_Dantoc_(int iUser)
        {
            ;
            string str = "";
            var dantoc = _thietlap.Get_Dantoc().Where(x => x.IDELETE == 0).ToList();
            //   int count = 0;

            foreach (var x in dantoc)
            {
                string id_encr = HashUtil.Encode_ID(x.IDANTOC.ToString());

                str += "<option value='" + id_encr + "'> - - - " + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_QuocTich(List<QUOCTICH> quoctich, int id_chucvu = 233)
        {
            string str = "";
            var chucvu = quoctich.Where(x => x.IHIENTHI == 1).OrderBy(t => t.IVITRI).ToList();
            //var chucvu = _quoctich.GetAll().OrderBy(t => HttpUtility.HtmlEncode(t.CTEN)).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.IQUOCTICH == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.IQUOCTICH + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_DanToc(List<DANTOC> dantoc, int id_chucvu = 1)
        {
            string str = "";
            var chucvu = dantoc.Where(x => x.IHIENTHI == 1).OrderBy(x => x.IDANTOC).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.IDANTOC == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.IDANTOC + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_Nguondon(List<KNTC_NGUONDON> nguondon, int id_chucvu = 0)
        {

            string str = "";
            var chucvu = nguondon.Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.INGUONDON == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.INGUONDON + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_Nguondon_iparent(List<KNTC_NGUONDON> nguondon, int id_chucvu = 0)
        {

            string str = "";
            var chucvu = nguondon.Where(x => x.IHIENTHI == 1 && x.IDELETE == 0 && x.IPARENT == 0).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.INGUONDON == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.INGUONDON + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }

       public string Option_NguonDon_ByID_Parent(int iparent, int id_choice, int type)
        {
            string str = "";          
            //Lấy danh sách nguồn đơn có IPARENT = 0 theo loại option được chọn, không lấy lên giá trị đã chọn trước đó
            List<KNTC_NGUONDON> nguondon = _thietlap.Get_Nguondon().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0 && x.ILOAI == iparent && x.IPARENT == 0 && x.INGUONDON != id_choice).ToList();
            int idGoc = 0;
            //type =1 :màn sửa, type= 0 : màn thêm mới
            if(type == 1)
            {
                //Lấy IPARENT của nguồn đơn đang được chọn để hiển thị mặc định
                KNTC_NGUONDON nguonduocchon = _thietlap.Get_Nguondon().Where(x => x.INGUONDON == id_choice).FirstOrDefault();
                if (nguonduocchon != null)
                {
                    idGoc = (int)nguonduocchon.IPARENT;
                }
            } 
            foreach (var p in nguondon)
            {
                string select = ""; if (idGoc!= 0 && p.INGUONDON == idGoc) { select = "selected "; }
                str += "<option " + select + " value='" + p.INGUONDON + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
                select = "";
            }
            return str;
        }

        public string Option_Nguonkiennghi_iparent(List<KN_NGUONDON> nguondon, int id_chucvu = 0)
        {

            string str = "";
            var chucvu = nguondon.Where(x => x.IHIENTHI == 1 && x.IDELETE == 0 && x.IPARENT == 0).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.INGUONDON == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.INGUONDON + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_LoaiDon(List<KNTC_LOAIDON> loaidon, int id_chucvu = 0)
        {
            string str = "";
            List<KNTC_LOAIDON> chucvu = loaidon.Where(x => x.IDELETE == 0).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.ILOAIDON == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.ILOAIDON + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_NoiDungDon(List<KNTC_NOIDUNGDON> noidungdon, int id_chucvu = 0)
        {
            string str = "";
            List<KNTC_NOIDUNGDON> chucvu = noidungdon.Where(v => v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.INOIDUNG == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.INOIDUNG + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_TinhChatDon(List<KNTC_TINHCHAT> tinhchat, int id_chucvu = 0)
        {
            string str = "";
            List<KNTC_TINHCHAT> chucvu = tinhchat.OrderBy(t => t.IVITRI).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.ITINHCHAT == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.ITINHCHAT + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string Option_QuocHoi_Khoa_KyHop(List<QUOCHOI_KHOA> qhkhoa, List<QUOCHOI_KYHOP> qhkyhop, int id_kyhop = 0)
        {
            var khoa = qhkhoa.Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderByDescending(x => x.DBATDAU).ToList();
            var kyhop = qhkyhop.Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.DBATDAU).ToList();
            string str = "";
            foreach (var d in khoa)
            {
                str += "<optgroup label='" + HttpUtility.HtmlEncode(d.CTEN) + "'>";
                var kyhop1 = kyhop.Where(x => x.IKHOA == (int)d.IKHOA).OrderBy(x => x.DBATDAU).ToList();
                foreach (var k in kyhop1)
                {
                    string select = "";
                    if (id_kyhop == 0 && k.IMACDINH == 1)
                    {
                        select = "selected";
                    }
                    else
                    {
                        if (id_kyhop == k.IKYHOP)
                        {
                            select = "selected";
                        }
                    }
                    str += "<option " + select + " value='" + k.IKYHOP + "'>" + HttpUtility.HtmlEncode(k.CTEN) + "</option>";
                }
                str += "</optgroup>";
            }
            return str;
        }
        public string OptionCoQuan(List<QUOCHOI_COQUAN> quochoicoquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }

            var list = quochoicoquan.Where(v => v.IPARENT == id_parent && v.ICOQUAN != id_donvi_choice && v.IDELETE == 0).ToList();
            foreach (var donvi in list)
            {
                if (quochoicoquan.Where(v => v.IPARENT == donvi.ICOQUAN).Count() > 0)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(donvi.CTEN) + "'>";
                    str += OptionCoQuan(quochoicoquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                }

            }
            return str;
        }
        public string Option_TinhThanh_ByID_Parent(List<DIAPHUONG> diaphuong, int parent = 0, int id = 0)
        {
            string str = "";
            List<DIAPHUONG> chucvu = diaphuong.Where(v => v.IPARENT == parent && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(t => HttpUtility.HtmlEncode(t.CTEN)).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.IDIAPHUONG == id) { select = " selected "; }
                str += "<option " + select + " value='" + p.IDIAPHUONG + "'>" + HttpUtility.HtmlEncode(p.CTEN) + "</option>";
            }
            return str;
        }
        public string OptionCoQuan_ThietLap(List<QUOCHOI_COQUAN> quochoi_coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {

            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            var list = quochoi_coquan.Where(v => v.IPARENT == id_parent && v.ICOQUAN != id_donvi_choice && v.IDELETE == 0).ToList();
            foreach (var donvi in list)
            {
                string select = "";
                if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                var kiemtra = quochoi_coquan.Where(v => v.IPARENT == donvi.ICOQUAN).ToList();
                if (kiemtra.Count() > 0)
                {
                    str += OptionCoQuan_ThietLap(quochoi_coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                }
            }
            return str;
        }
        public TaiKhoan Taikhoan_Detail(USERS u, int id)
        {
            TaiKhoan t = new TaiKhoan();
            int iPhong = (int)u.IPHONGBAN;
            var thongtinphongban = _thietlap.Get_List_Phongban().Where(x => x.IPHONGBAN == (int)u.IPHONGBAN).ToList();
            t.phongban = "";
            if (thongtinphongban.Count() > 0)
            {
                t.phongban = _thietlap.Get_Phongban((int)u.IPHONGBAN).CTEN;
            }
            var thongtinchucvu = _thietlap.Get_List_User_Chucvu().Where(x => x.ICHUCVU == (int)u.ICHUCVU).ToList();
            t.chucvu = "";
            if (thongtinchucvu.Count() > 0)
            {
                t.chucvu = _thietlap.Get_Chucvu((int)u.ICHUCVU).CTEN;
            }
            var thongtincoquan = _thietlap.Get_ListQuochoi_Coquan().Where(x => x.ICOQUAN == (int)u.IDONVI).ToList();
            t.donvi = "";
            if (thongtinchucvu.Count() > 0)
            {
                t.donvi = _thietlap.Get_Quochoi_Coquan((int)u.IDONVI).CTEN;
            }

            t.ten = u.CTEN;
            return t;
        }
        public string CheckBox_Tinh_Huyen_TiepXuc(List<QUOCHOI_COQUAN> coquan, List<DIAPHUONG> diaphuong, int iDonVi, string diaphuongchon)
        {
            string str = "";

            QUOCHOI_COQUAN cq = _thietlap.GetBy_Quochoi_CoquanID(iDonVi);
            var huyen = diaphuong.Where(v => v.IPARENT == (int)cq.IDIAPHUONG && v.IHIENTHI == 1 && v.IDELETE == 0).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList();
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
                str += "<p style='width:50%; float:left;'><input " + check + " type='checkbox' name='huyen' value='" + h.IDIAPHUONG + "' class='nomargin'/> " + h.CTYPE + " " + h.CTEN + "</p>";
            }
            return str;
        }
        public string CheckBox_DaiBieu_TiepXuc(List<DAIBIEU> daibieu, string daibieuchon, int iDonVi)
        {

            QUOCHOI_COQUAN coquan = _thietlap.GetBy_Quochoi_CoquanID(iDonVi);
            List<DAIBIEU> list = daibieu.Where(v => v.IDIAPHUONG == (int)coquan.IDIAPHUONG).ToList();
            string str = "";
            foreach (var d in list)
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
                    "' class='nomargin'/> " + HttpUtility.HtmlEncode(d.CTEN) + " " + truongdoan + "</p>";
            }
            return str;
        }
        public string NguoiCapNhat(int iUser, DateTime time)
        {

            string str = "";
            string tennguoinhap = _thietlap.GetBy_TaikhoanID(iUser).CTEN;
            str += "<p class='tright f-grey'><em>Cập nhật bởi " + tennguoinhap + " ngày " + func.ConvertDateVN(time.ToString()) + "</em></p>";
            return str;
        }
        public string Option_LinhVuc(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {

            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            var list = linhvuc.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).ToList();
            foreach (var donvi in list)
            {
                string select = "";
                if (donvi.ILINHVUC == id_donvi) { select = " selected "; }


                str += "<option " + select + " value='" + donvi.ILINHVUC + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";

                var kiemtra = linhvuc.Where(v => v.IPARENT == donvi.ILINHVUC && v.IDELETE == 0).ToList();
                if (kiemtra.Count() > 0)
                {
                    str += Option_LinhVuc(linhvuc, (int)donvi.ILINHVUC, level + 1, id_donvi, id_donvi_choice);
                }

            }
            return str;
        }
        public string Option_LinhVuc_ChonNoiDung(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {

            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            var list = linhvuc.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).ToList();
            foreach (var donvi in list)
            {
                string select = "";
                if (donvi.ILINHVUC == id_donvi) { select = " selected "; }
                if (donvi.IPARENT == 0)
                {
                    str += "<optgroup  label='" + HttpUtility.HtmlEncode(donvi.CTEN) + "' >";
                }
                else
                {
                    str += "<option " + select + " value='" + donvi.ILINHVUC + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                }
                var kiemtra = linhvuc.Where(v => v.IPARENT == donvi.ILINHVUC && v.IDELETE == 0).ToList();
                if (kiemtra.Count() > 0)
                {
                    str += Option_LinhVuc(linhvuc, (int)donvi.ILINHVUC, level + 1, id_donvi, id_donvi_choice);
                }
                str += "</optgroup>";

            }
            return str;
        }

        public string Option_LinhVuc_ChonNoiDung_Themmoi_NoiDung(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {

            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            var list = linhvuc.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).ToList();
            var listLoaiDon = _thietlap.Get_Loaidon();
            if(listLoaiDon.Count > 0)
            {

            }    
            foreach(var loaiDon in listLoaiDon)
            {
                str += "<optgroup  label='" + HttpUtility.HtmlEncode(loaiDon.CTEN) + "' >";
                var listByLoaiDon = list.Where(v => v.ILOAIDON == loaiDon.ILOAIDON);
                foreach (var donvi in listByLoaiDon)
                {
                    string select = "";
                    string child = "";
                    if (donvi.ILINHVUC == id_donvi) { select = " selected "; }
                    if (donvi.IPARENT == 0)
                        child = "- - -";
                    str += "<option " + select + " value='" + donvi.ILINHVUC + "'>" + child + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                    var kiemtra = linhvuc.Where(v => v.IPARENT == donvi.ILINHVUC && v.IDELETE == 0).ToList();
                }
                str += "</optgroup>";
            }    
                
            return str;
        }


        //public string Option_LinhVuc_NoPARENT(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        //{

        //    string str = "";
        //    string space_level = "";
        //    for (int i = 0; i < level; i++)
        //    {
        //        space_level += "- - - ";
        //    }
        //    var list = linhvuc.Where(v => v.IPARENT == 0 && v.ILINHVUC != id_donvi_choice).ToList();
        //    foreach (var donvi in list)
        //    {
        //        string select = "";
        //        if (donvi.ILINHVUC == id_donvi) { select = " selected "; }
        //        str += "<option " + select + " value='" + donvi.ILINHVUC + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
        //        var kiemtra = linhvuc.Where(v => v.IPARENT == donvi.ILINHVUC).ToList();
        //        if (kiemtra.Count() > 0)
        //        {
        //            str += Option_LinhVuc(linhvuc, (int)donvi.ILINHVUC, level + 1, id_donvi, id_donvi_choice);
        //        }

        //    }
        //    return str;
        //}
        public string Option_Type_Diaphuong(string type = "")
        {
            string str = "";
            string[] diphuong = { "Tỉnh", "Thành Phố", "Quận", "Huyện", "Thị Xã" };
            foreach (var d in diphuong)
            {
                string select = "";
                if (d == type)
                {
                    select = " selected ";
                }
                str += "<option " + select + " value='" + d + "'>" + d + "</option>";
            }
            return str;
        }
        public string OptionDiaphuong_ThietLap(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            var list = coquan.Where(v => v.IPARENT == id_parent && v.IDIAPHUONG != id_donvi_choice && v.IDELETE == 0).ToList();
            foreach (var donvi in list)
            {
                string select = "";
                if (donvi.IDIAPHUONG == id_donvi) { select = " selected "; }
                str += "<option " + select + " value='" + donvi.IDIAPHUONG + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                var kiemtra = coquan.Where(v => v.IPARENT == donvi.IDIAPHUONG).ToList();
                if (kiemtra.Count() > 0)
                {
                    str += OptionDiaphuong_ThietLap(coquan, (int)donvi.IDIAPHUONG, level + 1, id_donvi, id_donvi_choice);
                }

            }
            return str;
        }
        public string List_CheckBox_Group(List<USER_GROUP> user_group, int iUser, TaikhoanAtion act)
        {

            string str = "";
            var group = user_group.Where(x => x.IDELETE == 1).ToList();
            string cArrGroup_UserLogin = _thietlap.GetBy_TaikhoanID(act.iUser).CARRGROUP ?? "";
            foreach (var g in group)
            {
                string check = "", disabled = "";
                string cArrGroup = _thietlap.GetBy_TaikhoanID(iUser).CARRGROUP ?? "";
                if (!act.is_admin)
                {
                    if (cArrGroup_UserLogin.IndexOf("," + g.IGROUP + ",") == -1)
                    {
                        disabled = "disabled";
                    }
                }
                if (cArrGroup.IndexOf("," + g.IGROUP + ",") != -1)
                {
                    check = "checked";
                }
                str += "<p><input " + disabled + " " + check + " type='checkbox' name='group' class='nhom' value='" + g.IGROUP + "'/> " + g.CTEN + "</p>";
            }
            return str;
        }
        public string List_CheckBox_Taikhoan_action(List<ACTION> action, int iUser, TaikhoanAtion act)
        {

            string str = "";
            var action0 = action.Where(v => v.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
            var uat = _thietlap.GetBy_List_User_Action();
            string act_user_login = act.list_action;
            foreach (var a0 in action0)
            {
                str += "<p class=''>" + a0.CTEN + ":</p><ul class='list-chucnang'>";
                var action1 = action.Where(v => v.IPARENT == a0.IACTION).OrderBy(x => x.IVITRI).ToList();
                //var uat = _thietlap.GetBy_List_User_Action();

                foreach (var a1 in action1)
                {
                    string check = "";
                    string disabled = "";

                    List<USER_ACTION> uaction = uat.Where(v => v.IUSER == iUser && v.IACTION == a1.IACTION).ToList();
                    if (uaction.Count() > 0)
                    {
                        check = "checked ";
                    }
                    if (!act.is_admin)
                    {
                        uaction = uat.Where(v => v.IUSER == act.iUser && v.IACTION == a1.IACTION).ToList();
                        if (act_user_login.IndexOf("|" + a1.IACTION + "|") == -1)
                        {
                            disabled = "disabled ";
                        }
                    }
                    str += "<li><input " + check + disabled + " type='checkbox' name='action' value='" + a1.IACTION + "' /> " + a1.CTEN + "</li>";
                }
                str += "</ul>";
            }

            return str;
        }
        // Bổ sung tối 19 

        public string TK_Lichsu(int iUser)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IUSER", iUser);
            var lichsu = _thietlap.GetBy_List_Tracking(_condition).OrderByDescending(x => x.ID).ToList();

            foreach (var x in lichsu)
            {
                int iDon = (int)x.IDON;
                int iTongHop = (int)x.ITONGHOP;
                int iKienNghi = (int)x.IKIENNGHI;
                int itiepdan = (int)x.ITIEPDAN_DINHKY;
                TaiKhoan tk = Taikhoan_Detail(iUser);
                string phongban_donvi = "";
                if (tk.phongban != null) { phongban_donvi = tk.phongban + ", "; }
                if (tk.donvi != null) { phongban_donvi += tk.donvi + "."; }
                string don = "";
                if (iDon != 0)
                {
                    don = "</br><em class='f-grey'>Đơn KNTC: <a href='javascript:void()' onclick=\"ShowPopUp('id=" + iDon + "','/KNTC/Ajax_Don_info')\">Xem chi tiết</a></em>";
                }
                if (iKienNghi != 0)
                {
                    don = "</br><em class='f-grey'>Kiến nghị cử tri: <a href='javascript:void()' onclick=\"ShowPopUp('id=" + iKienNghi + "','/Kiennghi/Ajax_Kiennghi_info')\">Xem chi tiết</a></em>";
                }
                if (iTongHop != 0)
                {
                    don = "</br><em class='f-grey'>Tổng hợp KN cử tri: <a href='javascript:void()' onclick=\"ShowPopUp('id=" + iDon + "','/Kiennghi/Ajax_Tonghop_info')\">Xem chi tiết</a></em>";
                }
                if (itiepdan != 0)
                {
                    string id_encr = HashUtil.Encode_ID(itiepdan.ToString());
                    don = "</br><em class='f-grey'>Tổng hợp KN cử tri: <a href='/Tiepdan/Xemchitiet/?id=" + id_encr + "'  >Xem chi tiết</a></em>";
                }
                str += "<tr><td class='tcenter f-red' nowrap>" + String.Format("{0:dd/MM/yyyy HH:mm}", x.DDATE) +
                    "</td><td><strong>" + tk.ten + "</strong></br>" + phongban_donvi + "</td><td class=''>" + x.CACTION + don + "</td></tr>";

            }
            return str;
        }
        public string TimKiemTracking(List<TRACKING> track, List<USERS> users)
        {
            string str = "";
            foreach (var x in users)
            {
                var track1 = track.Where(t => t.IUSER == 1).OrderByDescending(t => t.DDATE).ToList();

                if (track1.Count() > 0)
                {
                    int dem = 1;
                    str += "<tr><th colspan='4'>" + x.CTEN.ToUpper() + "(" + track1.Count() + ") </th></tr>";
                    foreach (var t in track1)
                    {
                        str += "<tr><td style='text-align:center'>" + dem + "</td><td class='tcenter'>" + String.Format("{0:hh:mm dd/MM/yyyy}", (DateTime)t.DDATE) +
                        "</td><td><strong>" + HttpUtility.HtmlEncode(x.CUSERNAME) + " / " + HttpUtility.HtmlEncode(x.CTEN) + " </strong></td><td>" + HttpUtility.HtmlEncode(t.CACTION) + "</td></tr>";
                        dem++;
                    }

                }

            }
            if (str == "")
            {
                str = "<tr><td colspan=4 class='alert tcenter alert-danger'> Không có kết quả tìm kiếm nào</td></tr>";
            }
            return str;
        }
        public string OptionCoQuanXuLy(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            var list = coquan.Where(x => x.IPARENT == id_parent && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var donvi in list)
            {
                if (coquan.Where(x => x.IPARENT == (int)donvi.ICOQUAN && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).Count() > 0)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(donvi.CTEN) + "'>";
                    str += OptionCoQuanXuLy(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                }

            }
            return str;
        }


        // Bổ sung 
        public string List_Chucvu(List<USER_CHUCVU> Chucvu, int iUser)
        {
            string str = "";
            int count = 0;
            string url_cookie = func.Get_Url_keycookie();
            var phongban = new USER_PHONGBAN();
            var listchucvu = Chucvu.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var x in listchucvu)
            {
                string id_encr = HashUtil.Encode_ID(x.ICHUCVU.ToString(), url_cookie);
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Chucvu_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Chucvu_del','Bạn có chắc xóa tên chức vụ " + HttpUtility.HtmlEncode(x.CTEN) + " khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Chucvu_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";

                count++;
                phongban = _thietlap.Get_Phongban(Convert.ToInt32(x.IPHONGBAN));
                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td>" + (phongban != null ? HttpUtility.HtmlEncode(phongban.CTEN) : "") + "</td><td class=' tcenter'>" + order + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            return str;
        }
        public string Option_Chucvu(List<USER_CHUCVU> Chucvu, int iUser, int idchucvu = 0)
        {
            string str = "";
            var listchucvu = Chucvu.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var x in listchucvu)
            {
                string sel = "";
                if (idchucvu == x.ICHUCVU)
                {
                    sel = "selected";
                }
                str += "<option value='" + x.ICHUCVU + "' " + sel + " >" + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
                sel = "";
            }
            return str;
        }
        public string List_Chucvu_search(List<USER_CHUCVU> Chucvu, int iUser, string url_cookie, int id)
        {
            string str = "";
            int count = 0;
            var listchucvu = Chucvu.Where(x => x.IDELETE == 0).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN)).ToList();
            if (id != 0)
            {
                listchucvu = Chucvu.Where(x => x.IDELETE == 0 && x.ICHUCVU == id).OrderBy(x => x.IVITRI).ToList();
            }
            foreach (var x in listchucvu)
            {
                string id_encr = HashUtil.Encode_ID(x.ICHUCVU.ToString(), url_cookie);
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Chucvu_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Chucvu_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title=''  onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Chucvu_del','Bạn có chắc xóa tên chức vụ " + HttpUtility.HtmlEncode(x.CTEN) + " khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                count++;
                str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td> <td class=' tcenter'>" + order + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
            }
            return str;
        }

        public string Option_linhvuccoquanSorted(List<LINHVUC_COQUAN> Linhvuccoquan, List<QUOCHOI_COQUAN> coquan, int coQuanDuocChon)
        {
            string str = "";
            var thongtincoquan = coquan.Where(v => v.IDELETE == 0 && coQuanDuocChon == (int)v.ICOQUAN).ToList();
            foreach (var v in thongtincoquan)
            {
                var thongtinlinhvuccoquan = Linhvuccoquan.Where(x => x.IDELETE == 0 && x.ICOQUAN == (int)v.ICOQUAN).OrderBy(x => x.IVITRI).ToList();
                if (thongtinlinhvuccoquan.Count() > 0)
                {
                    str += "<optgroup label=" + HttpUtility.HtmlEncode(v.CTEN) + "></optgroup>";
                }
                foreach (var x in thongtinlinhvuccoquan)
                {
                    string sel = "";

                    str += "<option value='" + x.ILINHVUC + "' " + sel + " >";
                    if (x.IPARENT != 0)
                    {
                        str += "- - - ";
                        var linhVucCha = _thietlap.GetBy_Linhvuc_CoquanID((int)(x.IPARENT));
                        while (linhVucCha.IPARENT != 0)
                        {
                            str += "- - - ";
                            linhVucCha = _thietlap.GetBy_Linhvuc_CoquanID((int)(linhVucCha.IPARENT));
                        }
                    }
                    str += x.CCODE + "-" + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
                }
            }

            return str;
        }

        // bổ sung linh vuc co quan
        public string List_linhvuccoquan(List<LINHVUC_COQUAN> Linhvuccoquan, List<QUOCHOI_COQUAN> coquan, int iUser)
        {
            string str = "";
            var thongtincoquan = coquan.Where(v => v.IDELETE == 0).ToList();
            string url_cookie = func.Get_Url_keycookie();
            foreach (var v in thongtincoquan)
            {
                var thongtinlinhvuccoquan = Linhvuccoquan.Where(x => x.IDELETE == 0 && x.ICOQUAN == (int)v.ICOQUAN).OrderBy(x => x.IVITRI).ToList();
                if (thongtinlinhvuccoquan.Count() > 0)
                {
                    str += "<tr><th colspan='6'><strong>" + HttpUtility.HtmlEncode(v.CTEN) + "</strong></th></tr>";

                }
                int count = 0;

                foreach (var x in thongtinlinhvuccoquan)
                {
                    string id_encr = HashUtil.Encode_ID(x.ILINHVUC.ToString(), url_cookie);
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Linhvuc_Coquan_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_Coquan_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_Coquan_del','Bạn có chắc xóa lĩnh vực cơ quan " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Linhvuc_Coquan_status')\"/>";
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter' >" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>";
                    if (x.IPARENT != 0)
                    {
                        str += "- - - ";
                        var linhVucCha = _thietlap.GetBy_Linhvuc_CoquanID((int)(x.IPARENT));
                        while (linhVucCha.IPARENT != 0){
                            str += "- - - ";
                            linhVucCha = _thietlap.GetBy_Linhvuc_CoquanID((int)(linhVucCha.IPARENT));
                        } 
                    }
                    str += HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }
            }

            return str;
        }
        public string Option_linhvuccoquan(List<LINHVUC_COQUAN> Linhvuccoquan, List<QUOCHOI_COQUAN> coquan, int iUser, int id_linhvuc = 0)
        {
            string str = "";
            var thongtincoquan = coquan.Where(v => v.IDELETE == 0).ToList();
            foreach (var v in thongtincoquan)
            {
                var thongtinlinhvuccoquan = Linhvuccoquan.Where(x => x.IDELETE == 0 && x.ICOQUAN == (int)v.ICOQUAN).OrderBy(x => x.IVITRI).ToList();
                if (thongtinlinhvuccoquan.Count() > 0)
                {
                    str += "<optgroup label=" + HttpUtility.HtmlEncode(v.CTEN) + "></optgroup>";
                }
                foreach (var x in thongtinlinhvuccoquan)
                {
                    string sel = "";
                    if (id_linhvuc != 0 && id_linhvuc == x.ILINHVUC)
                    {
                        sel = "selected";
                    }
                    str += "<option value='" + x.ILINHVUC + "' " + sel + " >";
                    if (x.IPARENT != 0)
                    {
                        str += "- - - ";
                        var linhVucCha = _thietlap.GetBy_Linhvuc_CoquanID((int)(x.IPARENT));
                        while (linhVucCha.IPARENT != 0)
                        {
                            str += "- - - ";
                            linhVucCha = _thietlap.GetBy_Linhvuc_CoquanID((int)(linhVucCha.IPARENT));
                        }
                    }
                    str += x.CCODE + "-" + HttpUtility.HtmlEncode(x.CTEN) + "</option>";
                }
            }

            return str;
        }
        public string Option_LinhVuc_Coquan(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, string url_cookie = "", int id = 0)
        {
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            List<QUOCHOI_COQUAN> donvi = coquan.Where(x => x.IPARENT == id_parent && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            string sel = "";
            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {
                    if (t.ICOQUAN == id)
                    {
                        sel = "selected";
                    }

                    str += "<option value='" + t.ICOQUAN + "' " + sel + ">" + space_level + " " + HttpUtility.HtmlEncode(t.CTEN) + "</option>";


                    str += Option_LinhVuc_Coquan(coquan, (int)t.ICOQUAN, level + 1, url_cookie, id);
                    sel = "";
                }
            }
            return str;
        }
        public string List_linhvuccoquan_search(List<LINHVUC_COQUAN> Linhvuccoquan, List<QUOCHOI_COQUAN> coquan, int iUser, int id)
        {
            string str = "";
            var thongtincoquan = coquan.Where(v => v.IDELETE == 0).ToList();
            string url_cookie = func.Get_Url_keycookie();
            foreach (var v in thongtincoquan)
            {
                var thongtinlinhvuccoquan = Linhvuccoquan.Where(x => x.IDELETE == 0 && x.ICOQUAN == (int)v.ICOQUAN).OrderBy(x => x.IVITRI).ToList();
                if (id != 0)
                {
                    thongtinlinhvuccoquan = Linhvuccoquan.Where(x => x.IDELETE == 0 && x.ICOQUAN == (int)v.ICOQUAN && x.ILINHVUC == id).OrderBy(x => x.IVITRI).ToList();
                }
                if (thongtinlinhvuccoquan.Count() > 0)
                {
                    str += "<tr><th colspan='6' ><strong>" + HttpUtility.HtmlEncode(v.CTEN) + "</strong></th></tr>";
                }
                int count = 0;
                foreach (var x in thongtinlinhvuccoquan)
                {
                    string id_encr = HashUtil.Encode_ID(x.ILINHVUC.ToString(), url_cookie);
                    string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Linhvuc_Coquan_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                    string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_Coquan_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Linhvuc_Coquan_del','Bạn có chắc xóa lĩnh vực cơ quan " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Linhvuc_Coquan_status')\"/>";
                    count++;
                    str += "<tr id='tr_" + id_encr + "'><td class=' tcenter'>" + count + "</td><td class=' tcenter' >" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class=''>" + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='tcenter'>" + hienthi + "</td><td class='tcenter'>" + order + "</td><td class='tcenter'>" + edit + del + "</td></tr>";
                }
            }

            return str;
        }



        // Bổ sung ngày 30/12
        public string List_Traloiphanloai(List<KN_TRALOI_PHANLOAI> traloiphanloai, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";
            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }

            var phanloai = traloiphanloai.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            foreach (var t in phanloai)
            {
                string id_encr = HashUtil.Encode_ID(t.IPHANLOAI.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Traloi_Phanloai_status')\"/>";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Traloi_Phanloai_order')\" type=\"text\" value='" + t.IVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Traloi_Phanloai_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";

                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Traloi_Phanloai_del','Bạn có chắc xóa trả lời phân loại " + HttpUtility.HtmlEncode(t.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                if (t.IPHANLOAI == 1 || t.IPHANLOAI == 2 || t.IPHANLOAI == 3)
                {
                    // edit = "";
                    del = "";
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td class='" + bold_level + "'>" +
                     space_level + HttpUtility.HtmlEncode(t.CTEN) + "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                str += List_Traloiphanloai(traloiphanloai, (int)t.IPHANLOAI, level + 1, iUser, url_cookie);
            }
            return str;
        }
        public string Option_Traloiphanloai(List<KN_TRALOI_PHANLOAI> traloiphanloai, int id_parent = 0, int level = 0, int iUser = 0, int id = 0)
        {
            string str = "";
            var phanloai = traloiphanloai.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            foreach (var t in phanloai)
            {
                string sel = "";
                if (id == t.IPHANLOAI)
                {
                    sel = "selected";
                }

                str += "<option value='" + t.IPHANLOAI + "' " + sel + "> " + HttpUtility.HtmlEncode(t.CTEN) + "</option>";
                sel = "";
                str += Option_Traloiphanloai(traloiphanloai, (int)t.IPHANLOAI, level + 1, iUser);
            }
            return str;
        }
        public string List_Traloiphanloai_search(List<KN_TRALOI_PHANLOAI> traloiphanloai, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "", int id = 0)
        {
            string str = "";
            if (url_cookie == "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var phanloai = traloiphanloai.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            if (id != 0)
            {
                phanloai = traloiphanloai.Where(v => v.IDELETE == 0 && v.IPHANLOAI == id).OrderBy(t => t.IVITRI).ToList();
            }
            foreach (var t in phanloai)
            {
                string id_encr = HashUtil.Encode_ID(t.IPHANLOAI.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Traloi_Phanloai_status')\"/>";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Coquan_order')\" type=\"text\" value='" + t.IVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Traloi_Phanloai_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";

                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Traloi_Phanloai_del','Bạn có chắc xóa trả lời phân loại " + HttpUtility.HtmlEncode(t.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                if (t.IPHANLOAI == 1 || t.IPHANLOAI == 2 || t.IPHANLOAI == 3)
                {
                    //  edit = "";
                    del = "";
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CCODE) + "</td><td class='" + bold_level + "'>" +
                     space_level + HttpUtility.HtmlEncode(t.CTEN) + "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                str += List_Traloiphanloai(traloiphanloai, (int)t.IPHANLOAI, level + 1, iUser, url_cookie);
            }
            return str;
        }

        // End


        // Bổ Sung  03/001
        public string List_Nguondon_parent(List<KNTC_NGUONDON> loaidon, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";

            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var thongtinnguondon = loaidon.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            foreach (var x in thongtinnguondon)
            {
                string id_encr = HashUtil.Encode_ID(x.INGUONDON.ToString(), url_cookie);
                string loaiNguonDon = x.ILOAI == 0 ? "Quốc hội" : "Hội đồng nhân dân";
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nguondon_status')\"/>";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Nguondon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nguondon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Nguondon_del','Bạn có chắc xóa nguồn đơn " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class='" + bold_level + "'>" +
                     space_level + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='tcenter' nowrap>" + loaiNguonDon + "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                str += List_Nguondon_parent(loaidon, (int)x.INGUONDON, level + 1, iUser, url_cookie);
            }
            return str;
        }

        public string List_Nguonkiennghi_parent(List<KN_NGUONDON> loaidon, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "")
        {
            string str = "";

            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var thongtinnguondon = loaidon.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            foreach (var x in thongtinnguondon)
            {
                string id_encr = HashUtil.Encode_ID(x.INGUONDON.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nguonkiennghi_status')\"/>";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Nguonkiennghi_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nguonkiennghi_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Nguonkiennghi_del','Bạn có chắc xóa nguồn đơn " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class='" + bold_level + "'>" +
                     space_level + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                str += List_Nguonkiennghi_parent(loaidon, (int)x.INGUONDON, level + 1, iUser, url_cookie);
            }
            return str;
        }

        public string List_Nguondon_parent_Search(List<KNTC_NGUONDON> loaidon, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "", int id = 0)
        {
            string str = "";
            if (url_cookie != "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var thongtinnguondon = loaidon.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            if (id != 0)
            {
                thongtinnguondon = loaidon.Where(v => v.IDELETE == 0 && v.INGUONDON == id).OrderBy(t => t.IVITRI).ToList();
            }
            foreach (var x in thongtinnguondon)
            {
                string id_encr = HashUtil.Encode_ID(x.INGUONDON.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nguondon_status')\"/>";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Nguondon_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nguondon_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Nguondon_del','Bạn có chắc xóa nguồn đơn " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class='" + bold_level + "'>" +
                     space_level + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                str += List_Nguondon_parent_Search(loaidon, (int)x.INGUONDON, level + 1, iUser, url_cookie);
            }
            return str;
        }
        public string List_Nguonkiennghi_parent_Search(List<KN_NGUONDON> loaidon, int id_parent = 0, int level = 0, int iUser = 0, string url_cookie = "", int id = 0)
        {
            string str = "";
            if (url_cookie != "")
            {
                url_cookie = func.Get_Url_keycookie();
            }
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = "  "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var thongtinnguondon = loaidon.Where(v => v.IPARENT == id_parent && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            if (id != 0)
            {
                thongtinnguondon = loaidon.Where(v => v.IDELETE == 0 && v.INGUONDON == id).OrderBy(t => t.IVITRI).ToList();
            }
            foreach (var x in thongtinnguondon)
            {
                string id_encr = HashUtil.Encode_ID(x.INGUONDON.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Nguonkiennghi_status')\"/>";
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Nguonkiennghi_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Nguonkiennghi_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Nguonkiennghi_del','Bạn có chắc xóa nguồn kiến nghị " + HttpUtility.HtmlEncode(x.CTEN) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td class='" + bold_level + "'>" +
                     space_level + HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='tcenter' nowrap>" + order + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                str += List_Nguonkiennghi_parent_Search(loaidon, (int)x.INGUONDON, level + 1, iUser, url_cookie);
            }
            return str;
        }
        // end 


        // Bổ sung ngày 05
        public string Option_LinhVucParent(List<LINHVUC> linhvuc, int idlinhvuc = 0)
        {
            string str = "";
            var thongtinlinhvuc = linhvuc.Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var x in thongtinlinhvuc)
            {
                string sel = "";
                if (idlinhvuc != 0 && x.ILINHVUC == idlinhvuc)
                {
                    sel = "selected";
                }
                str += "<option value='" + x.ILINHVUC + "' " + sel + " > " + HttpUtility.HtmlEncode(x.CTEN) + " </option>";
            }
            return str;
        }
        public string Option_LinhVucParent_edit(List<LINHVUC> linhvuc, int idlinhvuc = 0, int id_parent = 0)
        {
            string str = "";
            var thongtinlinhvuc = linhvuc.Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var x in thongtinlinhvuc)
            {
                string sel = "";
                if (x.ILINHVUC == id_parent)
                {
                    sel = "selected";
                }
                if (x.IPARENT == 0 && x.ILINHVUC == idlinhvuc)
                {
                    str += "";
                }
                else
                {
                    str += "<option value='" + x.ILINHVUC + "' " + sel + " > " + HttpUtility.HtmlEncode(x.CTEN) + " </option>";
                }

            }
            return str;
        }
        // end




        // Bổ sung ngày 09
        public string Option_UserType(int itype = 0)
        {
            string str = "";
            var thongtinhtype = _thietlap.Get_Type().ToList();
            foreach (var x in thongtinhtype)
            {
                string sel = "";
                if (itype != 0 && itype == x.ITYPE)
                {
                    sel = "selected";
                }
                str += "<option value='" + x.ITYPE + "' " + sel + " >" + x.CNAME + "</option>";
            }
            return str;
        }
        // end 


        // 
        public string OptionCoQuan_phongban(int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            //var list = _coquan.GetList("select * from QUOCHOI_COQUAN WHERE IPARENT=" + id_parent + " and ICOQUAN!=" + id_donvi_choice).OrderBy(x => x.IVITRI).ToList();
            var list = _thietlap.Get_List_Quochoi_Coquan().Where(x => x.IPARENT == id_parent && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var donvi in list)
            {


                if (_thietlap.Get_List_Quochoi_Coquan().Where(x => x.IPARENT == (int)donvi.ICOQUAN && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).Count() > 0)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(donvi.CTEN) + "'>";
                    str += OptionCoQuan((int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN) + "</option>";
                }

            }
            return str;
        }


        // Bổ sung ngày 13/04/2017  
        public string Option_Traloiphanloai_parent(List<KN_TRALOI_PHANLOAI> traloiphanloai, int id_parent = 0, int level = 0, int iUser = 0, int id = 0)
        {
            string str = "";
            var phanloai = traloiphanloai.Where(v => v.IPARENT == 0 && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            foreach (var t in phanloai)
            {
                string sel = "";


                str += "<option value='" + t.IPHANLOAI + "' " + sel + "> " + HttpUtility.HtmlEncode(t.CTEN) + "</option>";
                sel = "";
            }
            return str;
        }
        public string Option_Traloiphanloai_parent_edit(List<KN_TRALOI_PHANLOAI> traloiphanloai, int id_parent = 0, int level = 0, int iUser = 0, int id = 0)
        {
            string str = "";
            var phanloai = traloiphanloai.Where(v => v.IPARENT == 0 && v.IDELETE == 0).OrderBy(t => t.IVITRI).ToList();
            foreach (var t in phanloai)
            {
                string sel = "";
                if (_thietlap.GetBy_TraLoi_PhanLoaiID(id).IPARENT == t.IPHANLOAI)
                {
                    sel = "selected";
                }
                if (_thietlap.GetBy_TraLoi_PhanLoaiID(id).IPARENT == 0)
                {
                    if (_thietlap.GetBy_TraLoi_PhanLoaiID(id).IPHANLOAI == t.IPHANLOAI)
                    {
                        str += "";
                    }
                    else
                    {
                        str += "<option value='" + t.IPHANLOAI + "' " + sel + "> " + HttpUtility.HtmlEncode(t.CTEN) + "</option>";
                    }

                }
                else
                {
                    str += "<option value='" + t.IPHANLOAI + "' " + sel + "> " + HttpUtility.HtmlEncode(t.CTEN) + "</option>";
                }
                sel = "";
            }
            return str;
        }

        public string TAIKHOAN_LIST(List<THIETLAP_NGUOIDUNG> donvi)
        {

            //int iDonVi = Convert.ToInt32(db.users.Single(x => x.iUser.Equals(iUser)).iDonVi);
            string str = "";
            string url_cookie = func.Get_Url_keycookie();
            //var donvi1 = donvi.GroupBy(x => x.ID_USER).ToList();
            decimal idonvi = 0;
            decimal id_phongban = -1;
            decimal id_user = 0;
            foreach (var d in donvi)
            {
                if (idonvi != d.USER_ID_DONVI)
                {
                    str += "<thead><tr><th class='' colspan='6'>" + HttpUtility.HtmlEncode(d.USER_TENDONVI) + "</th></tr></thead>";
                    id_phongban = -1;
                }
                if (id_phongban != d.USER_ID_PHONGBAN)
                {
                    string tenphongban = "Phòng ban khác";
                    if (d.USER_ID_PHONGBAN != 0)
                    {
                        tenphongban = HttpUtility.HtmlEncode(d.USER_TENPHONGBAN);
                    }
                    str += "<tr><th class='' colspan='6'> - - - " + tenphongban + "</th></tr>";
                    id_phongban = d.USER_ID_PHONGBAN;

                }
                if (id_user != d.ID_USER)
                {
                    string id_encr = HashUtil.Encode_ID(d.ID_USER.ToString(), url_cookie);
                    str += Row_TaiKhoan_PRC(donvi, d, id_encr);
                    id_user = d.ID_USER;
                }



                idonvi = d.USER_ID_DONVI;
                str += "";

            }

            return str;
        }

        public string COQUAN_LIST(List<THIETLAP_COQUAN> coquangoc, List<THIETLAP_COQUAN> coquankhac)
        {
            string str = "";
            string url_cookie = func.Get_Url_keycookie();
            //  var coquangoc = thietlaplist.GET_THIETLAPCOQUANGOC("PKG_THIETLAP_HETHONG.PKG_COQUANGOC").ToList();
            foreach (var x in coquangoc)
            {
                string id_encr = HashUtil.Encode_ID(x.IDCOQUAN.ToString(), url_cookie);
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Coquan_order')\" type=\"text\" value='" + x.IDVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Coquan_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Coquan_del','Bạn có chắc xóa cơ quan " + HttpUtility.HtmlEncode(x.CTENCOQUAN) + " khỏi dánh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string group = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IDGROUP)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_group')\"/>";
                string use = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IDUSER)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_use')\"/>";
                string tendiaphuong = x.CTENDIAPHUONG;
                if (x.IDCOQUAN == 4 && HttpUtility.HtmlEncode(x.CMACOQUAN) == "BDN")
                {
                    edit = ""; del = "";
                }
                //  string diaphuong = ""; if (t.IDIAPHUONG != 0) { diaphuong = _thietlap.GetBy_DiaphuongID(((int)t.IDIAPHUONG)).CTEN; }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(x.CMACOQUAN) + "</td><td >" +
                     HttpUtility.HtmlEncode(x.CTENCOQUAN) + "</td><td class='tcenter'>" + tendiaphuong + "</td><td class='tcenter'>" + order +
                    "</td><td class='tcenter'>" + use + "</td><td class='tcenter'>" + group + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                // var coquankhac = thietlaplist.GET_THIETLAPCOQUANKHAC("PKG_THIETLAP_HETHONG.PKG_COQUAN_CAPKHAC", (int)x.IDCOQUAN).ToList();
                foreach (var t in coquankhac)
                {
                    id_encr = HashUtil.Encode_ID(t.IDCOQUAN.ToString(), url_cookie);
                    order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Coquan_order')\" type=\"text\" value='" + t.IDVITRI + "' class='input-block-level tcenter' />";
                    edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Coquan_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Coquan_del','Bạn có chắc xóa cơ quan " + HttpUtility.HtmlEncode(t.CTENCOQUAN) + " khỏi dánh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    group = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IDGROUP)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_group')\"/>";
                    use = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IDUSER)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_use')\"/>";
                    tendiaphuong = x.CTENDIAPHUONG;
                    if (t.IDCOQUAN == 4 && HttpUtility.HtmlEncode(t.CMACOQUAN) == "BDN")
                    {
                        edit = ""; del = "";
                    }
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CMACOQUAN) + "</td><td >- - - " +
                     HttpUtility.HtmlEncode(t.CTENCOQUAN) + "</td><td class='tcenter'>" + tendiaphuong + "</td><td class='tcenter'>" + order +
                    "</td><td class='tcenter'>" + use + "</td><td class='tcenter'>" + group + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                }
            }
            return str;
        }
        public string COQUAN_LIST_SEARCH()
        {
            string str = "";
            string url_cookie = func.Get_Url_keycookie();
            var coquangoc = thietlaplist.GET_THIETLAPCOQUANGOC("PKG_THIETLAP_HETHONG.PKG_COQUANGOC").ToList();
            foreach (var x in coquangoc)
            {
                string id_encr = HashUtil.Encode_ID(x.IDCOQUAN.ToString(), url_cookie);
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Coquan_order')\" type=\"text\" value='" + x.IDVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Coquan_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Coquan_del','Bạn có chắc xóa cơ quan " + HttpUtility.HtmlEncode(x.CTENCOQUAN) + " khỏi dánh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string group = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IDGROUP)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_group')\"/>";
                string use = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IDUSER)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_use')\"/>";
                string tendiaphuong = x.CTENDIAPHUONG;
                if (x.IDCOQUAN == 4 && HttpUtility.HtmlEncode(x.CMACOQUAN) == "BDN")
                {
                    edit = ""; del = "";
                }
                //  string diaphuong = ""; if (t.IDIAPHUONG != 0) { diaphuong = _thietlap.GetBy_DiaphuongID(((int)t.IDIAPHUONG)).CTEN; }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(x.CMACOQUAN) + "</td><td >" +
                     HttpUtility.HtmlEncode(x.CTENCOQUAN) + "</td><td class='tcenter'>" + tendiaphuong + "</td><td class='tcenter'>" + order +
                    "</td><td class='tcenter'>" + use + "</td><td class='tcenter'>" + group + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                var coquankhac = thietlaplist.GET_THIETLAPCOQUANKHAC("PKG_THIETLAP_HETHONG.PKG_COQUAN_CAPKHAC", (int)x.IDCOQUAN).ToList();
                foreach (var t in coquankhac)
                {
                    id_encr = HashUtil.Encode_ID(t.IDCOQUAN.ToString(), url_cookie);
                    order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Coquan_order')\" type=\"text\" value='" + t.IDVITRI + "' class='input-block-level tcenter' />";
                    edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Coquan_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Coquan_del','Bạn có chắc xóa cơ quan " + HttpUtility.HtmlEncode(t.CTENCOQUAN) + " khỏi dánh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    group = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IDGROUP)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_group')\"/>";
                    use = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(t.IDUSER)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_use')\"/>";
                    tendiaphuong = x.CTENDIAPHUONG;
                    if (t.IDCOQUAN == 4 && HttpUtility.HtmlEncode(t.CMACOQUAN) == "BDN")
                    {
                        edit = ""; del = "";
                    }
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(t.CMACOQUAN) + "</td><td >- - - " +
                     HttpUtility.HtmlEncode(t.CTENCOQUAN) + "</td><td class='tcenter'>" + tendiaphuong + "</td><td class='tcenter'>" + order +
                    "</td><td class='tcenter'>" + use + "</td><td class='tcenter'>" + group + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                }
            }
            return str;
        }





        public string List_CheckBox_CoQuan(int ilinhvuc = 0)
        {


            string str = "";
            _dCondition = new Dictionary<string, object>();
            var listAllDonVi_LinhVuc = _thietlap.GetBy_List_DonVi_LinhVuc(_dCondition);
            _dCondition.Add("IPARENT", 0);
            var action0 = _thietlap.Get_List_Quochoi_Coquan().Where(x => x.IPARENT == 0 && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var a0 in action0)
            {
                str += "<p class=''>" + a0.CTEN + ":</p><ul class='list-chucnang'>";
                _dCondition = new Dictionary<string, object>();
                _dCondition.Add("IPARENT", a0.ICOQUAN);
                var action1 = _thietlap.GetBy_List_Quochoi_Coquan(_dCondition).Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                foreach (var a1 in action1)
                {
                    string check = "";
                    _dCondition = new Dictionary<string, object>();
                    _dCondition.Add("IDONVI", a1.ICOQUAN);
                    _dCondition.Add("ILINHVUC", ilinhvuc);
                    if (ilinhvuc != 0 && listAllDonVi_LinhVuc.Where(v => v.IDELETE == 0 && v.IDONVI == a1.ICOQUAN && v.ILINHVUC == ilinhvuc).ToList().Count() > 0)
                    {
                        check = "checked";
                    }
                    str += "<li><input " + check + " type='checkbox' name='action' value='" + a1.ICOQUAN + "' id='action" + a1.ICOQUAN + "' /> <a href=\"javascript:void()\" data-original-title='Chọn cơ quan' onclick=\"Checkboxchucnang(" + a1.ICOQUAN + ")\" rel='tooltip' title=''  style='color:black'>" + a1.CTEN + "</a></li>";
                }
                str += "</ul>";
            }
            return str;
        }
        public string List_CheckBox_KyHop(int iDaibieu = 0)
        {


            string str = "";
            var listKhoa = _thietlap.Get_List_Quochoi_Khoa().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var khoa in listKhoa)
            {
                var listKyhop = _thietlap.Get_List_Quochoi_Kyhop().Where(v => v.IKHOA == (int)khoa.IKHOA && khoa.IDELETE == 0).OrderBy(v => v.IVITRI).ToList();
                if (listKyhop != null && listKyhop.Count() > 0)
                {
                    str += "<p class=''>" + khoa.CTEN + ":</p><ul class='list-chucnang'>";
                    foreach (var kyhop in listKyhop)
                    {
                        string check = "";
                        if (_thietlap.Get_DaiBieu_KyHop().Where(t => t.ID_DAIBIEU == iDaibieu && t.ID_KYHOP == kyhop.IKYHOP).ToList().Count() > 0)
                        {
                            check = "checked";
                        }
                        str += "<li><input " + check + " type='checkbox' name='action' value='" + kyhop.IKYHOP + "' id='action" + kyhop.IKYHOP + "' /> <a href=\"javascript:void()\" data-original-title='Chọn kỳ họp' onclick=\"Checkboxchucnang(" + kyhop.IKYHOP + ")\" rel='tooltip' title=''  style='color:black'>" + kyhop.CTEN + "</a></li>";
                    }
                }

                str += "</ul>";
            }
            return str;
        }
        public string Option_LanhDao(int idlanhdao = 0, int idonvi_ = 0)
        {
            var donvi = thietlaplist.GetNguoiDung("PKG_THIETLAP_HETHONG.PRC_NGUOIDUNG", "").Where(x => x.USER_ID_DONVI == idonvi_).ToList();
            //int iDonVi = Convert.ToInt32(db.users.Single(x => x.iUser.Equals(iUser)).iDonVi);
            string str = "";
            string url_cookie = func.Get_Url_keycookie();
            //var donvi1 = donvi.GroupBy(x => x.ID_USER).ToList();
            decimal idonvi = 0;
            decimal id_phongban = -1;
            decimal id_user = 0;
            foreach (var d in donvi)
            {
                if (idonvi != d.USER_ID_DONVI)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(d.USER_TENDONVI) + "' >";
                    id_phongban = -1;
                }
                if (id_phongban != d.USER_ID_PHONGBAN)
                {
                    string tenphongban = "Phòng ban khác";
                    if (d.USER_ID_PHONGBAN != 0)
                    {
                        tenphongban = HttpUtility.HtmlEncode(d.USER_TENPHONGBAN);
                    }
                    id_phongban = d.USER_ID_PHONGBAN;

                }
                if (id_user != d.ID_USER)
                {
                    string sel = "";
                    if (idlanhdao == d.ID_USER)
                    {
                        sel = "selected";
                    }
                    string id_encr = HashUtil.Encode_ID(d.ID_USER.ToString(), url_cookie);
                    str += Row_TaiKhoan_Lanhdao(donvi, d, sel);
                    sel = "";
                    id_user = d.ID_USER;
                }
                str += "</optgroup>";


                idonvi = d.USER_ID_DONVI;
                str += "";

            }

            return str;
        }
        // Phân trang cơ quan thiết lập
        public string COQUANTHIETLAP(List<THIETLAP_COQUAN_PHANTRANG> thongtincoquan)
        {
            string str = "";
            string url_cookie = func.Get_Url_keycookie();
            decimal icoquan = 0;
            decimal icoquan2 = 0;
            int stt = 1;
            foreach (var x in thongtincoquan)
            {
                string id_encr = HashUtil.Encode_ID(x.ICOQUAN.ToString(), url_cookie);
                string order = "<input onchange=\"UpdateOrder('id=" + id_encr + "', this.value, '/Thietlap/Ajax_Coquan_order')\" type=\"text\" value='" + x.IVITRI + "' class='input-block-level tcenter' />";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Coquan_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Coquan_del','Bạn có chắc xóa cơ quan " + HttpUtility.HtmlEncode(x.CTEN) + " khỏi dánh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                string group = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IGROUP)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_group')\"/>";
                string use = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IUSE)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Coquan_use')\"/>";
                string tendiaphuong = "";
                string lever = "";
                if (x.ICOQUAN == 4 && HttpUtility.HtmlEncode(x.CCODE) == "BDN")
                {
                    edit = ""; del = "";
                }
                if(x.IPARENT != 0)
                {
                    lever = " - - - "; 
                    if(x.ICOQUAN > 400)
                    {

                        lever = " - - - - - - "; 
                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter' >" + stt + "</td><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(x.CCODE) + "</td><td >" +lever+
                                   HttpUtility.HtmlEncode(x.CTEN) + "</td><td class='tcenter'>" + tendiaphuong + "</td><td class='tcenter'>" + order +
                                  "</td><td class='tcenter'>" + use + "</td><td class='tcenter'>" + group + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                stt++;

            }
            return str;
        }

        public string DIAPHUONGTHIETLAP(List<THIETLAP_DIAPHUONG_PHANTRANG> thongtindiaphuong)
        {
            string str = "";
            string url_cookie = func.Get_Url_keycookie();
            decimal icoquan = 0;
            foreach (var x in thongtindiaphuong)
            {
                string id_encr = HashUtil.Encode_ID(x.IDIAPHUONG_PARENT.ToString(), url_cookie);
                string hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IDHIENTHI_PARENT)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Diaphuong_status')\"/>";
                string edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_del','Bạn có chắc xóa địa phương " + HttpUtility.HtmlEncode(x.CTENDIAPHUONG_PARENT) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                if (icoquan != x.IDIAPHUONG_PARENT)
                {
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(x.CMADIAPHUONG_PARENT) + "</td><td class='b'>" +
                    HttpUtility.HtmlEncode(x.CTENDIAPHUONG_PARENT) + "</td><td class='tcenter' nowrap>" + x.TYPE_PARENT + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                    icoquan = x.IDIAPHUONG_PARENT;
                    id_encr = HashUtil.Encode_ID(x.IDIAPHUONG.ToString(), url_cookie);
                    hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IDHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Diaphuong_status')\"/>";
                    edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_del','Bạn có chắc xóa địa phương " + HttpUtility.HtmlEncode(x.CTENDIAPHUONG) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(x.CMADIAPHUONG) + "</td><td class=''> - - - " +
                       HttpUtility.HtmlEncode(x.CTENDIAPHUONG) + "</td><td class='tcenter' nowrap>" + x.TYPE_ + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                }
                else
                {
                    id_encr = HashUtil.Encode_ID(x.IDIAPHUONG.ToString(), url_cookie);
                    hienthi = "<input type='checkbox' " + func.Checkbox_Checked(Convert.ToInt32(x.IDHIENTHI)) + " onclick=\"UpdateStatus('id=" + id_encr + "' ,'/Thietlap/Ajax_Diaphuong_status')\"/>";
                    edit = " <a href=\"javascript:void()\" data-original-title='Sửa' rel='tooltip' title='' onclick=\"ShowPopUp('id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_edit')\" class='trans_func'><i class='icon-pencil'></i></a> ";
                    del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Thietlap/Ajax_Diaphuong_del','Bạn có chắc xóa địa phương " + HttpUtility.HtmlEncode(x.CTENDIAPHUONG) + "  khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter  f-red'>" + HttpUtility.HtmlEncode(x.CMADIAPHUONG) + "</td><td class=''> - - - " +
                       HttpUtility.HtmlEncode(x.CTENDIAPHUONG) + "</td><td class='tcenter' nowrap>" + x.TYPE_ + "</td><td class='tcenter' nowrap>" + hienthi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                }

            }
            return str;
        }
        public string PhanTrangTracking(List<THIETLAP_LICHSU> Lichsu)
        {
            string str = "";
            int dem = 1;
            string gan = "";
            foreach (var x in Lichsu)
            {

                if (Lichsu.Count() > 0)
                {

                    if (x.TEN.ToUpper() != gan)
                    {
                        str += "<tr><th colspan='4'>" + x.TEN.ToUpper() + "</th></tr>";
                        gan = x.TEN.ToUpper();
                    }
                    str += "<tr><td style='text-align:center'>" + dem + "</td><td class='tcenter'>" + String.Format("{0:hh:mm dd/MM/yyyy}", (DateTime)x.THOIGIAN) +
                    "</td><td><strong>" + HttpUtility.HtmlEncode(x.USERNAME) + " / " + HttpUtility.HtmlEncode(x.TEN) + " </strong></td><td>" + HttpUtility.HtmlEncode(x.NOIDUNG) + "</td></tr>";
                    dem++;
                }

            }
            if (str == "")
            {
                str = "<tr><td colspan=4 class='alert tcenter alert-danger'> Không có kết quả tìm kiếm nào</td></tr>";
            }
            return str;
        }
        public string OptionCoQuan_Update(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
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
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level +  donvi.CTEN + "</option>";
                    str += OptionCoQuan_Update(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
 
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level +  donvi.CTEN + "</option>";
                    if (coquan_child > 0 && donvi.IGROUP == 0)
                    {
                        str += OptionCoQuan_Update(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    }
                }

            }
            return str;
        }
    }
}