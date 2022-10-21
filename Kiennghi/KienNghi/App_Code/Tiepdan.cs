using System;
using System.Collections.Generic;
using Entities.Objects;
using System.Linq;
using System.Web;
using Entities.Models;
using DataAccess.Dao;
using Utilities;
using DataAccess.Busineess;
using PagedList;
using Utilities.Enums;
namespace KienNghi.App_Code
{
    public class Tiepdan : Base
    {
        TiepdanBusineess _tiepdan = new TiepdanBusineess();
        ThietlapBusineess _thieplap = new ThietlapBusineess();
        TiepdanReport tiepdanreport = new TiepdanReport();
        KntcBusineess _kntc = new KntcBusineess();
        Funtions func = new Funtions();
        Dictionary<string, object> _condition;

        public decimal soluot_dinhky(int idinhky,int idonvi = 0,int iuser = 0,int kiemtra =0)
        {
            decimal tong = 0;
            
            _condition = new Dictionary<string, object>();
            _condition.Add("IDINHKY", idinhky);
            if(idonvi != 0  &&  !IsAdmin(iuser))
            {
                _condition.Add("IDONVI", idonvi);
                
            }
            if (kiemtra == 1)
            {
                _condition.Add("IUSER", iuser);
            }
            var vuviec = _tiepdan.GetBy_List_TDVuViec(_condition).Where(x=>x.ITIEPDOTXUAT == 0).ToList();
            tong = vuviec.Count();
            return tong;
        }

        public string Option_NoiDungDon_ThuocLinhVuc(int id_chucvu = 0, int iLinhvuc = 0)
        {
            string str = "";
            List<KNTC_NOIDUNGDON> chucvu = _thieplap.Get_Noidungdon().Where(x => x.IHIENTHI == 1 && x.ILINHVUC == iLinhvuc &&  x.IDELETE == 0).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN ) ).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.INOIDUNG == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.INOIDUNG + "'>" + HttpUtility.HtmlEncode(p.CTEN )+ "</option>";
            }
            return str;
        }
        public string Option_NoiDungDon_ThuocLinhVuc_BaoCao(int id_chucvu = 0, int iLinhvuc = 0)
        {
            string str = "";
            List<KNTC_NOIDUNGDON> chucvu = _thieplap.Get_Noidungdon().Where(x => x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            if (iLinhvuc != 0)
            {
                chucvu = _thieplap.Get_Noidungdon().Where(x => x.IHIENTHI == 1 && x.ILINHVUC == iLinhvuc && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            }
            foreach (var p in chucvu)
            {
                string select = ""; if (p.INOIDUNG == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.INOIDUNG + "'>" + HttpUtility.HtmlEncode(p.CTEN )+ "</option>";
            }
            return str;
        }
        public string Option_TinhChatDon_ThuocNguonDon(int id_chucvu = 0, int iNoidungdon = 0)
        {
            string str = "";

            List<KNTC_TINHCHAT> chucvu = _thieplap.Get_Tinhchat().Where(x => x.IHIENTHI == 1 && x.INHOMNOIDUNG == iNoidungdon && x.IDELETE == 0).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN ) ).ToList();
            foreach (var p in chucvu)
            {
                string select = ""; if (p.ITINHCHAT == id_chucvu) { select = " selected "; }
                str += "<option " + select + " value='" + p.ITINHCHAT + "'>" + HttpUtility.HtmlEncode(p.CTEN )+ "</option>";
            }
            return str;
        }
        public string Option_LinhVucThuocLoaiDon(List<LINHVUC> linhvuc, int id_parent = 0, int level = 0, int iUser = 0, int iloaidon = 0, int ilinhvuc = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            var linhvuc1 = linhvuc.Where(x => x.IPARENT == id_parent && x.IDELETE == 0 && iloaidon == x.ILOAIDON && x.IPARENT != 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var t in linhvuc1)
            {
                string id_encr = HashUtil.Encode_ID(t.ILINHVUC.ToString());
                string sel = "";
                if (ilinhvuc == t.ILINHVUC)
                {
                    sel = "selected";
                }
                str += "<option value=" + t.ILINHVUC + " " + sel + ">" + space_level + HttpUtility.HtmlEncode(t.CTEN)  + "</option>";
                str += Option_LinhVucThuocLoaiDon(linhvuc, (int)t.ILINHVUC, level + 1, iUser, iloaidon);
            }
            return str;
        }
        public string Option_LinhVuc(int id_chucvu = 0)
        {
            string str = "";
            var chucvu0 = _thieplap.Get_Linhvuc().Where(x => x.IPARENT == 0 && x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(t => HttpUtility.HtmlEncode(t.CTEN) ).ToList();
            foreach (var p in chucvu0)
            {
                string select0 = ""; if (p.ILINHVUC == id_chucvu) { select0 = " selected "; }
                var chucvu1 = _thieplap.Get_Linhvuc().Where(x => x.IPARENT == p.ILINHVUC && x.IHIENTHI == 1).OrderBy(t => HttpUtility.HtmlEncode(t.CTEN) ).ToList();
                if (chucvu1.Count() == 0)
                {
                    str += "<option " + select0 + " value='" + p.ILINHVUC + "'>" + HttpUtility.HtmlEncode(p.CTEN )+ "</option>";
                }
                else
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(p.CTEN )+ "'>";
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
        public decimal doannguoi_dinhky(int idinhky, int idonvi,int iuser = 0,int kiemtra = 0)
        {
            decimal tong = 0;
            _condition = new Dictionary<string, object>();
            _condition.Add("IDINHKY", idinhky);
            if (!IsAdmin(iuser) && iuser != 0 && idonvi!=0)
            {
                _condition.Add("IDONVI", idonvi);
               
            }
            if (kiemtra == 1)
            {
                _condition.Add("IUSER", iuser);
            }
            var vuviec = _tiepdan.GetBy_List_TDVuViec(_condition).ToList();
            foreach (var x in vuviec)
            {
                tong += (decimal)x.IDOANDONGNGUOI;
            }
            return tong;
        }
        public decimal luotnguoi_dinhky(int idinhky,int idonvi,int iuser = 0,int kiemtra = 0)
        {
            decimal tong = 0;
            _condition = new Dictionary<string, object>();
            _condition.Add("IDINHKY", idinhky);
            if (!IsAdmin(iuser) && iuser != 0 && idonvi != 0)
            {
                _condition.Add("IDONVI", idonvi);
                
            }
            if(kiemtra == 1)
            {
                _condition.Add("IUSER", iuser);
            }
            var vuviec = _tiepdan.GetBy_List_TDVuViec(_condition).ToList();
            foreach (var x in vuviec)
            {
                tong += (decimal)x.ISONGUOI;
            }
            return tong;
        }

        public string LIST_LICHTIEPDINHKY(List<TIEPCONGDAN_LICHTIEPDINHKY> thongtinlich,int idonvi=0, int iUser = 0)
        {
            string str = "";
            int count = 1;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var k in thongtinlich)
            {
                string id_encr = HashUtil.Encode_ID(k.IDDINHKY.ToString(), url_cookie);
                string edit = "  <a href='javascript:void()' data-original-title='Sửa' rel='tooltip' title=''  onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_Dinhky_edit')\" ) class='trans_func'><i class='icon-pencil'></i></a>";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                           "','/Tiepdan/Ajax_Dinhky_del','Bạn có muốn xóa lịch tiếp định kỳ')\" class='trans_func' ><i class='icon-trash'></i></a> ";
                string vuviec = "<a href='/Tiepdan/Dinhky_vuviec/?id=" + id_encr + "' data-original-title='Xem vụ việc' rel='tooltip' title=''  class='trans_func'><i class='icon-list-ol'></i><a>";
                if (k.IDUSER != iUser && !IsAdmin((int)k.IDUSER))
                {
                     del = "";
                }
                if(k.SOVUVIEC != 0)
                {
                    del = "";
                }
                string thongke = "<p><strong>Lượt người:</strong> " + k.SOLUOTNGUOI + "</p>" +
                            "<p><strong>Đoàn đông người:</strong> " + k.SODDOANDONGNGUOI + "</p>" +
                            "<p><strong>Số vụ việc:</strong> " + k.SOVUVIEC + "</p>";
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'>" + func.ConvertDateVN(k.NGAYTIEP.ToString()) +
                    "</td> <td class=''> " + HttpUtility.HtmlEncode(k.DIADIEMTIEP) + "," + k.TENHUYEN + "," + k.TENTINH + " </td><td class=''>" + thongke + "</td><td class='tcenter' nowrap>" + vuviec + edit + del + "</td></tr>";
                count++;
            }
            return str;

        }

        public string Tiepdan_dinhky(int iUser, int idonvi = 0,int kiemtra = 0)
        {
            string str = "";
            var kiennghi = _tiepdan.Get_List_TiepDanDinhKy().ToList().OrderByDescending(x => x.DNGAYTIEP);
            if (kiennghi.Count() == 0)
            {
                return "<tr><td colspan='7' class='tcenter alert alert-danger'>Không tìm thấy kết quả nào!</td></tr>";
            }
            int count = 1;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var k in kiennghi)
            {
                // new id 
                string id_encr = HashUtil.Encode_ID(k.IDINHKY.ToString(), url_cookie);

                // end

                string edit = "  <a href='javascript:void()' data-original-title='Sửa' rel='tooltip' title=''  onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_Dinhky_edit')\" ) class='trans_func'><i class='icon-pencil'></i></a>";
                string del = "";
                var kiemtravuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDINHKY == k.IDINHKY).ToList();
                if(kiemtravuviec.Count() == 0 )
                {
                     del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                           "','/Tiepdan/Ajax_Dinhky_del','Bạn có muốn xóa lịch tiếp định kỳ')\" class='trans_func' ><i class='icon-trash'></i></a> ";
                }
           
                string vuviec = "<a href='/Tiepdan/Dinhky_vuviec/?id=" + id_encr + "' class='trans_func'  data-original-title='Xem vụ việc' rel='tooltip' title=''><i class='icon-list-ol'></i><a>";
                _condition = new Dictionary<string, object>();
                _condition.Add("IDINHKY", k.IDINHKY);
                int vuviec_count = _tiepdan.HienThiDanhSachTiepDanDinhKyLoaiVuViec(_condition).ToList().Count();
                if (vuviec_count > 0)
                {
                    del = "";
                    vuviec = "<a href='/Tiepdan/Dinhky_vuviec/?id=" + id_encr + "' class='btn btn-success' data-original-title='Xem vụ việc' rel='tooltip' title=''>" + soluot_dinhky((int)k.IDINHKY) + "<a>";
                }
                if (k.IUSER != iUser && !IsAdmin(iUser))
                {
                    edit = ""; del = "";
                }
                string huyen = "";
                var thongtinhuyen = _thieplap.Get_Diaphuong().Where(v => v.IDIAPHUONG == k.ILUOT).ToList();
                if (k.ILUOT != 0 && thongtinhuyen.Count() > 0)
                {
                    huyen = " " + _thieplap.GetBy_DiaphuongID((int)k.ILUOT).CTEN;
                }
                string thanhpho = "";
                var thongtinthanhpho = _thieplap.Get_Diaphuong().Where(v => v.IDIAPHUONG == k.IDOAN).ToList();
                if (k.IDOAN != 0 && thongtinthanhpho.Count() > 0)
                {
                    thanhpho = "  " + _thieplap.GetBy_DiaphuongID((int)k.IDOAN).CTEN;
                }
                if (idonvi !=0)
                {
                    if (k.IVUVIEC == idonvi || soluot_dinhky((int)k.IDINHKY, idonvi, iUser)!=0)
                    {
                        string thongke = "<p><strong>Lượt người:</strong> " + luotnguoi_dinhky((int)k.IDINHKY, idonvi, iUser, kiemtra) + "</p>" +
                            "<p><strong>Đoàn đông người:</strong> " + doannguoi_dinhky((int)k.IDINHKY, idonvi, iUser, kiemtra) + "</p>" +
                            "<p><strong>Số vụ việc:</strong> " + soluot_dinhky((int)k.IDINHKY, idonvi, iUser, kiemtra) + "</p>";
                        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'>" + func.ConvertDateVN(k.DNGAYTIEP.ToString()) +
                            "</td> <td class=''> " + HttpUtility.HtmlEncode(k.CDIADIEM )  + "," + huyen + "," + thanhpho + " </td><td class=''>" + thongke + "</td><td class='tcenter' nowrap>" + vuviec + edit + del + "</td></tr>";
                        count++;
                    }
                }
                else
                {
                    string thongke = "<p><strong>Lượt người:</strong> " + luotnguoi_dinhky((int)k.IDINHKY, idonvi, iUser, kiemtra) + "</p>" +
                           "<p><strong>Đoàn đông người:</strong> " + doannguoi_dinhky((int)k.IDINHKY, idonvi, iUser, kiemtra) + "</p>" +
                           "<p><strong>Số vụ việc:</strong> " + soluot_dinhky((int)k.IDINHKY, idonvi, iUser, kiemtra) + "</p>";
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'>" + func.ConvertDateVN(k.DNGAYTIEP.ToString()) +
                        "</td> <td class=''> " + HttpUtility.HtmlEncode(k.CDIADIEM )  + "," + huyen + "," + thanhpho + " </td><td class=''>" + thongke + "</td><td class='tcenter' nowrap>" + vuviec + edit + del + "</td></tr>";
                    count++;
                }
                
            }
            return str;
        }



        public string Tiepdan_dinhky_search(int iUser, string url_cookie, int id, int idonvi = 0, int dem = 0, int kiemtra = 0)
        {
            string str = "";
            var kiennghi = _tiepdan.Get_List_TiepDanDinhKy().Where(x => x.IDINHKY == id).ToList().OrderByDescending(x => x.DNGAYTIEP);
            if (id == 0)
            {
                kiennghi = _tiepdan.Get_List_TiepDanDinhKy().ToList().OrderByDescending(x => x.DNGAYTIEP);
            }
            //if (kiennghi.Count() == 0)
            //{
            //    return "<tr><td colspan='7' class='tcenter alert alert-danger'>Không tìm thấy kết quả nào!</td></tr>";
            //}
            int count = 1;

            foreach (var k in kiennghi)
            {
                // new id 
                string id_encr = HashUtil.Encode_ID(k.IDINHKY.ToString(), url_cookie);
                // end
                string edit = "  <a href='javascript:void()' data-original-title='Sửa' rel='tooltip' title=''  onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_Dinhky_edit')\" ) class='trans_func'><i class='icon-pencil'></i></a>";
                string del = "";
                var kiemtravuviec = _tiepdan.Get_TDVuviec().Where(x => x.IDINHKY == k.IDINHKY).ToList();
                if (kiemtravuviec.Count() == 0)
                {
                    del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                          "','/Tiepdan/Ajax_Dinhky_del','Bạn có muốn xóa lịch tiếp định kỳ')\" class='trans_func' ><i class='icon-trash'></i></a> ";
                }
                string vuviec = "<a href='/Tiepdan/Dinhky_vuviec/?id=" + id_encr + "' class='trans_func'  data-original-title='Xem vụ việc' rel='tooltip' title=''><i class='icon-list-ol'></i><a>";
                _condition = new Dictionary<string, object>();
                _condition.Add("IDINHKY", k.IDINHKY);

                int vuviec_count = _tiepdan.HienThiDanhSachTiepDanDinhKyLoaiVuViec(_condition).ToList().Count();
                if (vuviec_count > 0)
                {
                    del = "";
                    vuviec = "<a href='/Tiepdan/Dinhky_vuviec/?id=" + id_encr + "' class='btn btn-success' data-original-title='Xem vụ việc' rel='tooltip' title=''>" + soluot_dinhky((int)k.IDINHKY) + "<a>";
                }
                if (k.IUSER != iUser && !IsAdmin(iUser))
                {
                    edit = ""; del = "";
                }
                string huyen = "";
                var thongtinhuyen = _thieplap.Get_Diaphuong().Where(v => v.IDIAPHUONG == k.ILUOT).ToList();
                if (k.ILUOT != 0 && thongtinhuyen.Count() > 0)
                {
                    huyen = " - " + _thieplap.GetBy_DiaphuongID((int)k.ILUOT).CTEN;
                }
                string thanhpho = "";
                var thongtinthanhpho = _thieplap.Get_Diaphuong().Where(v => v.IDIAPHUONG == k.IDOAN).ToList();
                if (k.IDOAN != 0 && thongtinthanhpho.Count() > 0)
                {
                    thanhpho = "  " + _thieplap.GetBy_DiaphuongID((int)k.IDOAN).CTEN;
                }
                if (idonvi == k.IVUVIEC || soluot_dinhky((int)k.IDINHKY, idonvi, iUser) != 0)
                {
                    string thongke = "<p><strong>Lượt người:</strong> " + luotnguoi_dinhky((int)k.IDINHKY, idonvi, iUser,kiemtra) + "</p>" +
                   "<p><strong>Đoàn đông người:</strong> " + doannguoi_dinhky((int)k.IDINHKY, idonvi, iUser,kiemtra) + "</p>" +
                   "<p><strong>Số vụ việc:</strong> " + soluot_dinhky((int)k.IDINHKY, idonvi, iUser, kiemtra) + "</p>";
                    str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + dem + "</td><td class='tcenter b'>" + func.ConvertDateVN(k.DNGAYTIEP.ToString()) +
                    "</td> <td class=''>" + HttpUtility.HtmlEncode(k.CDIADIEM )  + "," + huyen + "," + thanhpho + "</td><td class=''>" + thongke + "</td><td class='tcenter' nowrap>" + vuviec + edit + del + "</td></tr>";
                    count++;
                }
            }
            return str;
        }

        public string Rown_tiepdan_dinhky_phanloai_vuviec(int id)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IDINHKY", id);
            var vuviec = _tiepdan.HienThiDanhSachTiepDanDinhKyLoaiVuViec(_condition).ToList();
            foreach (var v in vuviec)
            {
                int iLoai = (int)v.ILOAIDON;
                string loaidon = _thieplap.GetBy_LoaidonID(iLoai).CTEN;
                str += "<p>" + loaidon + ": <strong>" + v.IVALUE + "</strong></p>";
            }
            return str;
        }

        //public string Tiepdan_thuongxuyen(int iDonVi_TiepDan, int iUser)
        //{
        //    string str = "";
        //    _condition = new Dictionary<string, object>();
        //    _condition.Add("IDINHKY", 0);
        //    var vuviec = _tiepdan_vuviec.GetAll(_condition).OrderByDescending(x => x.DNGAYNHAN).ToList();

        //    if (vuviec.Count() == 0)
        //    {
        //        return "<tr><td colspan='7' class='tcenter alert alert-danger'>Không tìm thấy kết quả nào!</td></tr>";
        //    }
        //    int count = 1;
        //    string url_cookie = func.Get_Url_keycookie();
        //    foreach (var k in vuviec)
        //    {
        //        string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
        //        ThuongXuyen t = ThuongXuyen_Detail((int)k.IVUVIEC, id_encr);
        //        string edit = " <a href=\"/Tiepdan/Thuongxuyen_edit/?id=" + id_encr + "\" data-original-title='Sửa' rel='tooltip' title='' class='btn btn-warning'><i class='icon-pencil'></i></a> ";
        //        string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage('" + id_encr + "','id=" + id_encr +
        //                        "','/Tiepdan/Ajax_Thuongxuyen_del')\" class='btn btn-danger'><i class='icon-trash'></i></a> ";
        //        if (k.IUSER != iUser && !IsAdmin(iUser))
        //        {
        //            edit = ""; del = "";
        //        }
        //        _condition = new Dictionary<string, object>();
        //        _condition.Add("ITHUONGXUYEN", k.ITHUONGXUYEN);
        //        TIEPDAN_THUONGXUYEN_KETQUA ketqua = _tiepdan_thuongxuyen_ketqua.GetAll(_condition).FirstOrDefault();
        //        string traloi = "";
        //        if (ketqua.CKETQUA_NGUOITRALOI != "")
        //        {
        //            traloi += "<p><strong>Người trả lời:</strong> " + ketqua.CKETQUA_NGUOITRALOI + "</p>";
        //        }
        //        if (ketqua.CKETQUA != "")
        //        {
        //            traloi += "<p><strong>Kết quả trả lời:</strong> " + ketqua.CKETQUA + "</p>";
        //        }
        //        traloi += File_View((int)ketqua.IKETQUA, "thuongxuyen_traloi");
        //        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td></td><td class='tcenter'><p class='b'>" + t.ngaytiep +
        //            "</p>" + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</br><strong>(" + t.coquan_tiep +
        //            ")</strong></td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " " + File_View((int)k.ITHUONGXUYEN, "thuongxuyen") + "</p><p><strong>Người gửi:</strong> " + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
        //            + " (" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + ")</p>" + t.bt_info + "</td><td>" + traloi + "</td><td class='tcenter'>" + kiemtrung_tiepthuongxuyen(k, id_encr, iUser) + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
        //        count++;
        //    }
        //    return str;
        //}
        //public string kiemtrung_tiepthuongxuyen(TIEPDAN_THUONGXUYEN t, string id_encr, int iUser)
        //{
        //    string str = "";
        //    if (t.IKIEMTRUNG == 0)
        //    {
        //        if (t.IUSER == iUser || IsAdmin(iUser))
        //        {
        //            str = "<a href='/Tiepdan/Thuongxuyen_kiemtrung/?id=" + id_encr + "' data-original-title='Kiểm trùng' rel='tooltip' title='' class='btn btn-primary'><i class='icon-search'></i></a>";

        //        }
        //        else
        //        {
        //            str = "Chưa kiểm trùng";
        //        }
        //    }
        //    else
        //    {
        //        if (t.ITHUONGXUYEN_TRUNG == 0)
        //        {
        //            if (t.IUSER == iUser || IsAdmin(iUser))
        //            {
        //                str = "<p>Không tìm thấy </p><a href='/Tiepdan/Thuongxuyen_kiemtrung/?id=" + id_encr + "' data-original-title='Kiểm trùng lại' rel='tooltip' title='' class='btn btn-primary'><i class='icon-search'></i></a>";

        //            }
        //            else
        //            {
        //                str = "Không tìm thấy ";
        //            }
        //        }
        //        else
        //        {
        //            str = " <p><a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_Thuongxuyen_info')\" data-original-title='' rel='tooltip' title='' class='btn btn-warning'><i class='icon-info-sign'></i> Xem vụ việc trùng</a></p>";
        //            if (t.IUSER == iUser || IsAdmin(iUser))
        //            {
        //                str += "<a href='/Tiepdan/Thuongxuyen_kiemtrung/?id=" + id_encr + "' data-original-title='Kiểm trùng lại' rel='tooltip' title='' class='btn btn-primary'><i class='icon-search'></i></a>";

        //            }
        //        }
        //    }
        //    return str;
        //}

        //public string File_View(int id, string type)
        //{
        //    string str = "";
        //    _condition = new Dictionary<string, object>();
        //    _condition.Add("ID", id);
        //    _condition.Add("CTYPE", type);
        //    var file = _tiepdan.LIST_FILE(_condition);
        //    foreach (var f in file)
        //    {
        //        str += " <a href='" + f.CFILE + "' class='f-green'><i class='icon-download-alt'></i></a>";
        //    }
        //    return str;
        //}
        public string File_View(int id, string type)
        {
           /// FileuploadRepository _file = new FileuploadRepository();
            string str = "";
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            _dic.Add("ID", id);
            _dic.Add("CTYPE", type);
            string url_cookie = func.Get_Url_keycookie();
            var file = _tiepdan.LIST_FILE(_dic).ToList();
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
        public string Kiemtrungvuviec(List<TD_VUVIEC> thongtinkiemtrung, string cookie, int id, int id_trung)
        {
            string str = "";
            string check = "";
            int dem = 1;
            if (thongtinkiemtrung.Count() == 0)
            {
                return "<tr><td colspan='6' class='nopadding'><div class='alert alert-success tcenter b nomargin'><i class='icon-ok-sign'></i> Không tìm thấy vụ việc có nội dung tương tự</div></td>" +
                                   "</tr>";
            }
            string id_doncheck_encr = HashUtil.Encode_ID(id.ToString(), cookie);
            foreach (var x in thongtinkiemtrung)
            {
                string tendiaphuong = "";
                var thongtindiaphuong = _thieplap.Get_Diaphuong().Where(v => v.IDIAPHUONG == (int)x.IDIAPHUONG_0).ToList();
                if (x.IDIAPHUONG_0 != -1 && thongtindiaphuong.Count() > 0)
                {
                    tendiaphuong = _thieplap.GetBy_DiaphuongID((int)x.IDIAPHUONG_0).CTEN;
                }
                string id_encr = HashUtil.Encode_ID(x.IVUVIEC.ToString(), cookie);
                string chon = "<a id='btn_" + id_encr + "' data-original-title='Chọn đơn trùng' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonDonTrung('" + id_encr + "','id_trung=" + id_encr + "&id=" + id_doncheck_encr + "','/Tiepdan/Ajax_Vuviec_update')\" class='chontrung f-grey'><i class='icon-ok-sign'></i></a>";
                if (x.IVUVIEC == id_trung)
                {
                    chon = "<a id='btn_" + id_encr + "' data-original-title='Bỏ chọn trùng' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonDonTrung('" + id_encr + "','id_trung=" + id_encr + "&id=" + id_doncheck_encr + "','/Tiepdan/Ajax_Vuviec_update')\" class='trans_func chontrung'><i class='icon-ok-sign'></i></a>";
                }
                str += " <tr>" +
                      " <td style='text-align:center'>" + dem + "</td>" +
                      " <td style='text-align:center'>" + chon + "</td>" +
                      "<td >" + HttpUtility.HtmlEncode(x.CNOIDUNG)  + "</td>" +
                      " <td><p>" + HttpUtility.HtmlEncode(x.CNGUOIGUI_TEN )  + " </p><p>" + tendiaphuong + "</p></td>" +
                      " <td></td>" +
                      "<td style='text-align:center'><a  class='trans_func'  data-original-title='Xem chi tiết' rel='tooltip' title='' href='/Tiepdan/Xemchitiet/?id=" + id_encr + "' ><i class='icon-info-sign'></i></a></td>" +
                      "</tr>";
                dem++;
                check = "";
            }
            return str;
        }
        public string Kiemtrungvuviec2(List<TD_VUVIEC> thongtinkiemtrung, string cookie)
        {
            string str = "";
            string check = "";
            int dem = 1;
            if (thongtinkiemtrung.Count() == 0)
            {
                return "<tr><td colspan='6' class='nopadding'><div class='alert alert-success tcenter b nomargin'><i class='icon-ok-sign'></i> Không tìm thấy vụ việc có nội dung tương tự</div></td>" +
                                   "</tr>";
            }
           
            foreach (var x in thongtinkiemtrung)
            {
                string tendiaphuong = "";
                var thongtindiaphuong = _thieplap.Get_Diaphuong().Where(v => v.IDIAPHUONG == (int)x.IDIAPHUONG_0).ToList();
                if (x.IDIAPHUONG_0 != -1 && thongtindiaphuong.Count() > 0)
                {
                    tendiaphuong = _thieplap.GetBy_DiaphuongID((int)x.IDIAPHUONG_0).CTEN;
                }
                string id_encr = HashUtil.Encode_ID(x.IVUVIEC.ToString(), cookie);
                string chon = "<a id='btn_" + id_encr + "' data-original-title='Chọn đơn trùng' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonDonTrung2('" + id_encr + "','id_trung=" + id_encr + "','/Tiepdan/Ajax_Vuviec_update2')\" class='chontrung f-grey'><i class='icon-ok-sign'></i></a>";
               
                str += " <tr>" +
                      " <td style='text-align:center'>" + dem + "</td>" +
                      " <td style='text-align:center'>" + chon + "</td>" +
                      "<td >" + HttpUtility.HtmlEncode(x.CNOIDUNG) + "</td>" +
                      " <td><p>" + HttpUtility.HtmlEncode(x.CNGUOIGUI_TEN) + " </p><p>" + tendiaphuong + "</p></td>" +
                      " <td></td>" +
                      "<td style='text-align:center'><a  class='trans_func'  data-original-title='Xem chi tiết' rel='tooltip' title='' href='/Tiepdan/Xemchitiet/?id=" + id_encr + "' ><i class='icon-info-sign'></i></a></td>" +
                      "</tr>";
                dem++;
                check = "";
            }
            return str;
        }
        //public ThuongXuyen ThuongXuyen_Detail(int id, string id_encr)
        //{
        //    ThuongXuyen kn = new ThuongXuyen();
        //    TIEPDAN_THUONGXUYEN don = _tiepdan_thuongduyen.GetByID(id);
        //    if (don.ILINHVUC != 0)
        //    {
        //        kn.linhvuc = _linhvuc.GetByID((int)don.ILINHVUC).CTEN;
        //    }
        //    if (don.ITINHCHAT != 0)
        //    {
        //        kn.tinhchat = _tinhchat.GetByID((int)don.ITINHCHAT).CTEN;
        //    }
        //    if (don.ILOAI != 0)
        //    {
        //        // kn.loaivuviec = db.kntc_loaidon.Single(x => x.iLoaiDon.Equals((int)don.ILOAI)).cTen;
        //        kn.loaivuviec = _kntc_loaidon.GetByID((int)don.ILOAI).CTEN;
        //    }
        //    if (don.INOIDUNG != 0)
        //    {
        //        kn.loai_noidung = _noidung.GetByID((int)don.INOIDUNG).CTEN;
        //    }
        //    kn.ngaytiep = func.ConvertDateVN(don.DNGAYTIEP.ToString());
        //    kn.bt_info = " <a href='javascript:void()' onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_Thuongxuyen_info')\" data-original-title='' rel='tooltip' title='' class='btn btn-primary'><i class='icon-info-sign'></i> Xem chi tiết</a>";

        //    kn.coquan_tiep = _quochoi_coquan.GetByID((int)don.ICOQUANTIEPDAN).CTEN;

        //    return kn;
        //}
        public string Option_Coquan_TiepDan(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_select = 0, string url_cookie = "")
        {
            string str = "";
            string space_level = "", bold_level = "";
            if (level == 0) { bold_level = " b "; }
            for (int i = 0; i < level; i++) { space_level += "- - - "; }
            List<QUOCHOI_COQUAN> donvi = coquan.Where(x => x.IPARENT == id_parent && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();

            if (donvi.Count() > 0)
            {
                foreach (var t in donvi)
                {
                    string select = ""; if (t.ICOQUAN == id_select) { select = "selected"; }
                    string id_encr = HashUtil.Encode_ID(t.ICOQUAN.ToString(), url_cookie);
                    str += "<option " + select + " value='" + t.ICOQUAN + "'>" + space_level + " " + HttpUtility.HtmlEncode(t.CTEN)  + "</option>";
                    str += Option_Coquan_TiepDan(coquan, (int)t.ICOQUAN, level + 1, id_select, url_cookie);
                }
            }
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
            _condition = new Dictionary<string, object>();
            _condition.Add("IPARENT", id_parent);
            var list = _thieplap.GetBy_List_Quochoi_Coquan(_condition).ToList().Where(t => t.ICOQUAN != id_donvi_choice).ToList();
            foreach (var donvi in list)
            {
                _condition = new Dictionary<string, object>();
                _condition.Add("IPARENT", donvi.ICOQUAN);
                if (_thieplap.GetBy_List_Quochoi_Coquan(_condition).ToList().Count() > 0)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(donvi.CTEN ) + "'>";
                    str += OptionCoQuan((int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN ) + "</option>";
                }

            }
            return str;
        }
        public string KetQuaXuLy(int ivuviec, int itinhtrang, int iUser, string id_encr)
        {
            string str = "";
            string ketqua = "Chọn hình thức";
            //string id_encr = HashUtil.Encode_ID(iDon.ToString());
            if (!Action(17, iUser))
            {
                return "<p class='tcenter'>Đang nghiên cứu</p>";
            }
            string dangnghiencuu = "<li><a href='#' onclick=\"UpdateTrangthai('id=" + id_encr + "','/Tiepdan/Ajax_Update_ketquaxuly')\">Đang nghiên cứu</a></li>";
            if (itinhtrang == (decimal)TrangThaiXuLy.DangNghienCuu)
            {
                ketqua = "" + StringEnum.GetStringValue(TrangThaiXuLy.DangNghienCuu) + ""; dangnghiencuu = "";
            }
            str = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-primary dropdown-toggle'>" +
                ketqua + " <span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" + dangnghiencuu +
                 "<li><a href='#' onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_Huongdanxuly')\">" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</a></li>" +
                 "<li><a href='/Tiepdan/Ajax_HuongDanTrucTiep_insert/" + id_encr + "' onclick='ShowPageLoading()' >" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</a></li>" +
                "<li><a href='#' onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_Chuyenxuly')\">" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + " (chuyển đơn sang Khiếu nại tố cáo)</a></li>" +
                "<li><a href='#' onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_Chuyenxuly_noibo')\">" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + " (chuyển Cơ quan có thẩm quyền)</a></li>" +
                
                //li><a href='#' onclick=\"ShowPopUp('id=" + id_encr + "','/Kntc/Ajax_Luutheodoi')\">Lưu đơn, theo dõi</a></li>" +
                "</ul></div>";
            return str;
        }
     
        //public string Dinhky_vuviec(List<TD_VUVIEC> vuviec, List<TD_VUVIEC_XULY> Xuly, int iUser, int iDinhKy, string url_cookie, int id_donvi = 0, int page = 0)
        //{

        //    string id_encr_dinhky = HashUtil.Encode_ID(iDinhKy.ToString(), url_cookie);
        //    string str = "";
        //    string trangthaixuly = "";
        //    var thongtinvuviec = vuviec.Where(v => v.IDINHKY == iDinhKy && ( id_donvi == v.IDONVI)).OrderByDescending(x => x.IVUVIEC).ToPagedList(page, pageSize).ToList();
        //    // var thongtinvuviec = vuviec.Where(v => v.IDINHKY == iDinhKy && (id_donvi == 4 || id_donvi == v.IDONVI)).OrderByDescending(x => x.IVUVIEC).ToPagedList(page, pageSize).ToList();
        //    int total = vuviec.Where(v => v.IDINHKY == iDinhKy && (id_donvi == 4 || id_donvi == v.IDONVI)).OrderByDescending(x => x.IVUVIEC).ToPagedList(page, pageSize).ToList().Count();
        //    if (thongtinvuviec.Count() == 0)
        //    {
        //        return "";
        //    }
        //    int count = 1;
        //    foreach (var k in thongtinvuviec)
        //    {
        //        string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
        //        "<ul class='dropdown-menu dropdown-success'>" +
        //         "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IVUVIEC + "' >Hướng dẫn, xử lý</a></li>" +
        //        "<li><a href='/Tiepdan/Word_Nhandon/" + k.IVUVIEC + "'>Nhận đơn</a></li>" +
        //        "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IVUVIEC + "'>Chuyển xử lý</a></li>" +
        //        "</ul></div>";
        //        string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
        //        string diachinguoigui = "";
        //        string traloichuyenxuly = "";
        //        if (HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  != null)
        //        {
        //            diachinguoigui = "" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + " , ";
        //        }
        //        if (k.ITINHTRANGXULY == 0)
        //        {
        //            trangthaixuly = KetQuaXuLy((int)k.IVUVIEC, (int)k.ITINHTRANGXULY, iUser, id_encr);
        //        }
        //        string traloi = "";
        //        if (k.ITINHTRANGXULY != 0)
        //        {

        //            if (k.ITINHTRANGXULY == 1)
        //            {
        //                trangthaixuly = "<p>Hướng dẫn, xử lý</p>";

        //            }
        //            else if (k.ITINHTRANGXULY == 2)
        //            {
        //                trangthaixuly = "<p>Nhận đơn</p>";
        //            }
        //            else if (k.ITINHTRANGXULY == 3)
        //            {

        //                trangthaixuly = "<p>Chuyển xử lý</p>";
        //                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
        //            }
        //            var thongtintraloi = Xuly.Where(x => x.CLOAI != "traloichuyenxuly" && x.IVUVIEC == k.IVUVIEC).ToList();
        //            foreach (var x in thongtintraloi)
        //            {
        //                string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
        //                if (k.ITINHTRANGXULY == 3)
        //                {
        //                    if (thongtintraloi.Count() != 0)
        //                    {
        //                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Trả lời kết quả xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
        //                    }
        //                    else
        //                    {
        //                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title=' Trả lời kết quả xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
        //                    }
        //                }
        //                traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
        //            }
        //        }
        //        DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
        //        string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
        //        string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
        //        string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
        //        string nguoitiep = "<i>" + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</i>";
        //        string vuviectrung = "";
        //        string thongtinhienthitrangthai = "";
        //        string thongtintinh = "";
        //        string thongtinhuyen = "";
        //        var listtinh = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_0).ToList();
        //        if (listtinh.Count() > 0)
        //        {
        //            thongtintinh = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_0).CTEN;
        //        }
        //        var listhuyen = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_1).ToList();
        //        if (listhuyen.Count() > 0)
        //        {
        //            thongtinhuyen = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_1).CTEN+" , ";
        //        }
        //        if (k.IVUVIECTRUNG != 0 && k.IVUVIECTRUNG != null)
        //        {
                 
        //            string id_encr2 = HashUtil.Encode_ID(k.IVUVIECTRUNG.ToString(), url_cookie);
        //            vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
        //        }
        //        thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;
                
        //        string hienthi_chucnang = "";
        //        if (k.ITINHTRANGXULY == 0)
        //        {
        //            string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
        //            if (k.IGIAMSAT != 0)
        //            {
        //                thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sủa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
        //                hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
        //            }
        //            else
        //            {
        //                hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

        //            }
        //        }
        //        else
        //        {
        //            string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
        //            if (k.IGIAMSAT != 0)
        //            {
        //                thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
        //                hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
        //            }
        //            else
        //            {
        //                hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

        //            }
        //        }
        //        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.DNGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
        //       + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + "" +
        //       "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.IGIAMSAT) + "</p><p>" + k.CYKIENGIAMSAT + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
        //        count++;
        //        trangthaixuly = "";
        //        traloi = "";
        //        traloichuyenxuly = "";
        //    }
        //    str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
        //         "/Tiepdan/Dinhky_vuviec/?id=" + id_encr_dinhky + "&") + "</td></tr>";
        //    return str;
        //}
        //public string Dinhky_vuviec_search(List<TD_VUVIEC> thongtinvuviec, List<TD_VUVIEC_XULY> Xuly, int iUser, int iDinhKy, string url_cookie, int idem, int ivuviec, int idonvi)
        //{

        //    string id_encr_dinhky = HashUtil.Encode_ID(iDinhKy.ToString(), url_cookie);
        //    string str = "";
        //    string trangthaixuly = "";
        //    var vuviec = thongtinvuviec.Where(v => v.IDINHKY == iDinhKy && (idonvi == v.IDONVI) && v.IVUVIEC == ivuviec).OrderByDescending(x => x.IVUVIEC).ToList();
        //    if (vuviec.Count() == 0)
        //    {
        //        return "";
        //    }
        //    int count = 1;
        //    foreach (var k in vuviec)
        //    {
        //        string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
        //        "<ul class='dropdown-menu dropdown-success'>" +
        //         "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IVUVIEC + "' >Hướng dẫn, xử lý</a></li>" +
        //        "<li><a href='/Tiepdan/Word_Nhandon/" + k.IVUVIEC + "'>Nhận đơn</a></li>" +
        //        "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IVUVIEC + "'>Chuyển xử lý</a></li>" +
        //        "</ul></div>";
        //        string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
        //        string diachinguoigui = "";
        //        string traloichuyenxuly = "";
        //        if (HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  != null)
        //        {
        //            diachinguoigui = "" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + " , ";
        //        }
        //        if (k.ITINHTRANGXULY == 0)
        //        {
        //            trangthaixuly = KetQuaXuLy((int)k.IVUVIEC, (int)k.ITINHTRANGXULY, iUser, id_encr);
        //        }
        //        string traloi = "";
        //        if (k.ITINHTRANGXULY != 0)
        //        {

        //            if (k.ITINHTRANGXULY == 1)
        //            {
        //                trangthaixuly = "<p>Hướng dẫn, xử lý</p>";

        //            }
        //            else if (k.ITINHTRANGXULY == 2)
        //            {
        //                trangthaixuly = "<p>Nhận đơn</p>";
        //            }
        //            else if (k.ITINHTRANGXULY == 3)
        //            {

        //                trangthaixuly = "<p>Chuyển xử lý</p>";
        //                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
        //            }
        //            var thongtintraloi = Xuly.Where(x => x.CLOAI != "traloichuyenxuly" && x.IVUVIEC == k.IVUVIEC).ToList();
        //            foreach (var x in thongtintraloi)
        //            {
        //                string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
        //                if (k.ITINHTRANGXULY == 3)
        //                {
        //                    if (thongtintraloi.Count() != 0)
        //                    {
        //                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Trả lời kết quả xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
        //                    }
        //                    else
        //                    {
        //                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title=' Trả lời kết quả xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
        //                    }
        //                }
        //                traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
        //            }
        //        }
        //        DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
        //        string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
        //        string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
        //        string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
        //        string nguoitiep = "<i>" + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</i>";
        //        string vuviectrung = "";
        //        string thongtinhienthitrangthai = "";
        //        string thongtintinh = "";
        //        string thongtinhuyen = "";
        //        var listtinh = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_0).ToList();
        //        if (listtinh.Count() > 0)
        //        {
        //            thongtintinh = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_0).CTEN;
        //        }
        //        var listhuyen = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_1).ToList();
        //        if (listhuyen.Count() > 0)
        //        {
        //            thongtinhuyen = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_1).CTEN+" , ";
        //        }
        //        if (k.IVUVIECTRUNG != 0 && k.IVUVIECTRUNG != null)
        //        {
                  
        //            string id_encr2 = HashUtil.Encode_ID(k.IVUVIECTRUNG.ToString(), url_cookie);
        //            vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
        //        }
        //            thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;
                
        //        string hienthi_chucnang = "";
        //        if (k.ITINHTRANGXULY == 0)
        //        {
        //            string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
        //            if (k.IGIAMSAT != 0)
        //            {
        //                thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sủa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
        //                hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
        //            }
        //            else
        //            {
        //                hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

        //            }
        //        }
        //        else
        //        {
        //            string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
        //            if (k.IGIAMSAT != 0)
        //            {
        //                thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
        //                hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
        //            }
        //            else
        //            {
        //                hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

        //            }
        //        }
        //        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + idem + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.DNGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
        //       + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + "" +
        //       "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.IGIAMSAT) + "</p><p>" + k.CYKIENGIAMSAT + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
        //        count++;
        //        trangthaixuly = "";
        //        traloi = "";
        //        traloichuyenxuly = "";
        //    }

        //    return str;
        //}

        public string ketqua_thongtingiamsat(int igiamsat)
        {
            string str = "";
            if (igiamsat == 1)
            {
                str = "Chuyên đề";
            }
            else if (igiamsat == 2)
            {
                str = "Lồng ghép";
            }
            else if (igiamsat == 3)
            {
                str = "Vụ việc cụ thể";
            }
            else
            {
                str = "";
            }
            return str;
        }
        //public string Thuongxuyen_vuviec_search(List<TD_VUVIEC> thongtinvuviec, List<TD_VUVIEC_XULY> Xuly, int iUser, string url_cookie, int idem, int ivuviec, int idonvi)
        //{

        //    string str = "";
        //    _condition = new Dictionary<string, object>();
        //    _condition.Add("IVUVIEC", ivuviec);
        //    string trangthaixuly = "";
        //    var vuviec = thongtinvuviec.Where(v => v.IDINHKY == 0 && (idonvi == v.IDONVI) && v.IVUVIEC == ivuviec).OrderByDescending(x => x.IVUVIEC).ToList();
        //    //  int total = _tiepdan.GetBy_List_TDVuViec(_condition).Where(v => v.IDINHKY == 0 && (idonvi == 4 || idonvi == v.IDONVI)).OrderByDescending(x => x.IVUVIEC).ToPagedList(page, pageSize).ToList().Count();
        //    if (vuviec.Count() == 0)
        //    {
        //        return "";
        //    }
        //    int count = 1;
        //    foreach (var k in vuviec)
        //    {
               
        //        string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
        //       "<ul class='dropdown-menu dropdown-success'>" +
        //        "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IVUVIEC + "' >Hướng dẫn, xử lý</a></li>" +
        //       "<li><a href='/Tiepdan/Word_Nhandon/" + k.IVUVIEC + "'>Nhận đơn</a></li>" +
        //       "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IVUVIEC + "'>Chuyển xử lý</a></li>" +
        //       "</ul></div>";
        //        string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
        //        string diachinguoigui = "";
        //        string traloichuyenxuly = "";
        //        if (HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  != null)
        //        {
        //            diachinguoigui = "" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + " , ";
        //        }
        //        if (k.ITINHTRANGXULY == 0)
        //        {
        //            trangthaixuly = KetQuaXuLy((int)k.IVUVIEC, (int)k.ITINHTRANGXULY, iUser, id_encr);
        //        }
        //        string traloi = "";
        //        if (k.ITINHTRANGXULY != 0)
        //        {

        //            if (k.ITINHTRANGXULY == 1)
        //            {
        //                trangthaixuly = "<p>Hướng dẫn, xử lý</p>";

        //            }
        //            else if (k.ITINHTRANGXULY == 2)
        //            {
        //                trangthaixuly = "<p>Nhận đơn</p>";
        //            }
        //            else if (k.ITINHTRANGXULY == 3)
        //            {

        //                trangthaixuly = "<p>Chuyển xử lý</p>";
        //                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
        //            }
        //            var thongtintraloi = Xuly.Where(x => x.CLOAI != "traloichuyenxuly" && x.IVUVIEC == k.IVUVIEC).ToList();
        //            foreach (var x in thongtintraloi)
        //            {
        //                string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
        //                if (k.ITINHTRANGXULY == 3)
        //                {
        //                    if (thongtintraloi.Count() != 0)
        //                    {
        //                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Trả lời kết quả xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
        //                    }
        //                    else
        //                    {
        //                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title=' Trả lời kết quả xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
        //                    }
        //                }
        //                traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
        //            }
        //        }
        //        DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
        //        string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
        //        string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
        //        string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
        //        string nguoitiep = "<i>" + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</i>";
        //        string vuviectrung = "";
        //        string thongtinhienthitrangthai = "";
        //        string thongtintinh = "";
        //        string thongtinhuyen = "";
        //        var listtinh = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_0).ToList();
        //        if (listtinh.Count() > 0)
        //        {
        //            thongtintinh = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_0).CTEN;
        //        }
        //        var listhuyen = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_1).ToList();
        //        if (listhuyen.Count() > 0)
        //        {
        //            thongtinhuyen = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_1).CTEN+" , ";
        //        }
        //        if (k.IVUVIECTRUNG != 0 && k.IVUVIECTRUNG != null)
        //        {
        //         //   thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
        //            string id_encr2 = HashUtil.Encode_ID(k.IVUVIECTRUNG.ToString(), url_cookie);
        //            vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
        //        }
               
        //            thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;
                
        //        string hienthi_chucnang = "";
        //        if (k.ITINHTRANGXULY == 0)
        //        {
        //            string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
        //            if (k.IGIAMSAT != 0)
        //            {
        //                thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sủa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
        //                hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
        //            }
        //            else
        //            {
        //                hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

        //            }
        //        }
        //        else
        //        {
        //            string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
        //            if (k.IGIAMSAT != 0)
        //            {
        //                thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
        //                hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
        //            }
        //            else
        //            {
        //                hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

        //            }
        //        }
        //        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + idem + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.DNGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
        //       + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + "" +
        //       "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.IGIAMSAT) + "</p><p>" + k.CYKIENGIAMSAT + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
        //        count++;
        //        trangthaixuly = "";
        //        traloi = "";
        //        traloichuyenxuly = "";
        //    }
        //    return str;
        //}
        //public string Thuongxuyen_vuviec(List<TD_VUVIEC> vuviec, List<TD_VUVIEC_XULY> Xuly, int iUser, string url_cookie, int id_donvi = 0, int page = 0)
        //{

        //    string str = "";
        //    string trangthaixuly = "";
        //    var thongtinvuviec = vuviec.Where(v => v.IDINHKY == 0 && (id_donvi == v.IDONVI)).OrderByDescending(x => x.IVUVIEC).ToPagedList(page, pageSize).ToList();
        //    int total = vuviec.Where(v => v.IDINHKY == 0 && ( id_donvi == v.IDONVI)).OrderByDescending(x => x.IVUVIEC).ToList().Count();
        //    if (thongtinvuviec.Count() == 0)
        //    {
        //        return "";
        //    }
        //    int count = 1;
        //    foreach (var k in thongtinvuviec)
        //    {
        //        string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
        //        "<ul class='dropdown-menu dropdown-success'>" +
        //         "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IVUVIEC + "' >Hướng dẫn, xử lý</a></li>" +
        //        "<li><a href='/Tiepdan/Word_Nhandon/" + k.IVUVIEC + "'>Nhận đơn</a></li>" +
        //        "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IVUVIEC + "'>Chuyển xử lý</a></li>" +
        //        "</ul></div>";
        //        string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
        //        string diachinguoigui = "";
        //        string traloichuyenxuly = "";
        //        if (HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  != null)
        //        {
        //            diachinguoigui = "" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + " , ";
        //        }
        //        if (k.ITINHTRANGXULY == 0)
        //        {
        //            trangthaixuly = KetQuaXuLy((int)k.IVUVIEC, (int)k.ITINHTRANGXULY, iUser, id_encr);
        //        }
        //        string traloi = "";
        //        if (k.ITINHTRANGXULY != 0)
        //        {

        //            if (k.ITINHTRANGXULY == 1)
        //            {
        //                trangthaixuly = "<p>Hướng dẫn, xử lý</p>";

        //            }
        //            else if (k.ITINHTRANGXULY == 2)
        //            {
        //                trangthaixuly = "<p>Nhận đơn</p>";
        //            }
        //            else if (k.ITINHTRANGXULY == 3)
        //            {

        //                trangthaixuly = "<p>Chuyển xử lý</p>";
        //                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
        //            }
        //            var thongtintraloi = Xuly.Where(x => x.CLOAI != "traloichuyenxuly" && x.IVUVIEC == k.IVUVIEC).ToList();
        //            foreach (var x in thongtintraloi)
        //            {
        //                string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
        //                if (k.ITINHTRANGXULY == 3)
        //                {
        //                    if (thongtintraloi.Count() != 0)
        //                    {
        //                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Trả lời kết quả xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
        //                    }
        //                    else
        //                    {
        //                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title=' Trả lời kết quả xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
        //                    }
        //                }
        //                traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
        //            }
        //        }
        //        DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
        //        string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
        //        string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
        //        string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
        //        string nguoitiep = "<i>" + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</i>";
        //        string vuviectrung = "";
        //        string thongtinhienthitrangthai = "";
        //        string thongtintinh = "";
        //        string thongtinhuyen = "";
        //        var listtinh = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_0).ToList();
        //        if (listtinh.Count() > 0)
        //        {
        //            thongtintinh = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_0).CTEN;
        //        }
        //        var listhuyen = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_1).ToList();
        //        if (listhuyen.Count() > 0)
        //        {
        //            thongtinhuyen = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_1).CTEN + " , ";
        //        }
        //        if (k.IVUVIECTRUNG != 0 && k.IVUVIECTRUNG != null)
        //        {
        //          //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
        //            string id_encr2 = HashUtil.Encode_ID(k.IVUVIECTRUNG.ToString(), url_cookie);
        //            vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
        //        }
               
        //            thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;
                
        //        string hienthi_chucnang = "";
        //        if (k.ITINHTRANGXULY == 0)
        //        {
        //            string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
        //            if (k.IGIAMSAT != 0)
        //            {
        //                thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sủa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
        //                hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
        //            }
        //            else
        //            {
        //                hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

        //            }
        //        }
        //        else
        //        {
        //            string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
        //            if (k.IGIAMSAT != 0)
        //            {
        //                thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
        //                hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
        //            }
        //            else
        //            {
        //                hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

        //            }
        //        }
        //        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.DNGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
        //       + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + "" +
        //       "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.IGIAMSAT) + "</p><p>" + k.CYKIENGIAMSAT + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
        //        count++;
        //        trangthaixuly = "";
        //        traloi = "";
        //        traloichuyenxuly = "";
        //    }
        //    str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
        //  "/Tiepdan/Thuongxuyen/?") + "</td></tr>";
        //    return str;
        //}
        public string Kydinh_vuviec_search(List<TD_VUVIEC> vuviec, int iUser, string url_cookie)
        {
            string str = "";

            var kiennghi = vuviec.OrderByDescending(x => x.IVUVIEC).ToList();
            if (kiennghi.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in kiennghi)
            {
                string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                if (HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  != null)
                {
                    diachinguoigui = "(" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + ")";
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func'><i class='icon-search'></i></a> ";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del','Bạn muốn xóa vụ việc khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                string nguoitiep = "<p><strong>Người tiếp</strong>: " + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</p>";
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.DNGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " " + File_View((int)k.IVUVIEC, "dinhky_vuviec") +
                    "</p><p>" + nguoitiep + "</p></td><td><p><strong>Người gửi:</strong> " + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
                    + " " + diachinguoigui + "</p></td><td>" + t.traloi + "</td><td class='tcenter' nowrap>" + t.bt_info + kiemtrung + edit + del + "</td></tr>";
                count++;
            }
            return str;
        }


        public string Dinhky_vuviec_list(int iUser, string url_cookie)
        {
            string str = "";
            _condition = new Dictionary<string, object>();

            var kiennghi = _tiepdan.Get_TDVUVIEC_LISTAL().Where(x => x.IDINHKY != 0 && x.IDINHKY != null).OrderByDescending(x => x.IVUVIEC).ToList();
            if (kiennghi.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in kiennghi)
            {
                string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
                string edit = " <a href=\"javascript:void(0)\" onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_edit')\" data-original-title='Sửa' rel='tooltip' title='' class='btn btn-warning'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                                "','/Tiepdan/Ajax_Vuviec_del','Bạn có muốn xóa vụ việc khỏi danh sách?')\" class='btn btn-danger'><i class='icon-trash'></i></a> ";
                string nguoitiep = "<p><strong>Người tiếp</strong>: " + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</p>";
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " " + File_View((int)k.IVUVIEC, "dinhky_vuviec") +
                    "</p><p>" + nguoitiep + t.loaivuviec + t.linhvuc + t.tinhchat + t.loai_noidung + t.doan_dongnguoi + "</p>" + t.bt_info + "</td><td><p><strong>Người gửi:</strong> " + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
                    + " (" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + ")</p></td><td>" + t.traloi + "</td><td class='tcenter' nowrap>" + edit + del + "</td></tr>";
                count++;
            }
            return str;
        }
        public int Kiemtralinhvuc(int iLinhvuc)
        {
            int kiemtra = 0;
            if (iLinhvuc != null)
            {

                var thongtinlinhvuc = _thieplap.Get_Linhvuc().Where(x => x.ILINHVUC != null && x.ILINHVUC == iLinhvuc).ToList();
                if (iLinhvuc != null && iLinhvuc != 0 && thongtinlinhvuc.Count() > 0)
                {
                    kiemtra = 1;
                }
            }

            return kiemtra;
        }
        public int Kiemtratinhchat(int iTinhchat)
        {
            int kiemtra = 0;
            if (iTinhchat != null)
            {
                var thongtintinhchat = _thieplap.Get_Tinhchat().Where(x => x.ITINHCHAT != null && x.ITINHCHAT == iTinhchat).ToList();
                if (iTinhchat != null && iTinhchat != 0 && thongtintinhchat.Count() > 0)
                {
                    kiemtra = 1;
                }
            }
            return kiemtra;
        }
        public int Kiemtraloaidon(int iLoaidon)
        {
            int kiemtra = 0;
            if (iLoaidon != null)
            {
                var thongtinloaidon = _thieplap.Get_Loaidon().Where(x => x.ILOAIDON != null && x.ILOAIDON == iLoaidon).ToList();
                if (iLoaidon != null && iLoaidon != 0 && thongtinloaidon.Count() > 0)
                {
                    kiemtra = 1;
                }
            }
            return kiemtra;
        }
        public int Kiemtranoidung(int iNoidung)
        {
            int kiemtra = 0;
            if (iNoidung != null)
            {
                var thongtinnoidungdon = _thieplap.Get_Noidungdon().Where(x => x.INOIDUNG != null && x.INOIDUNG == iNoidung).ToList();
                if (iNoidung != null && iNoidung != 0 && thongtinnoidungdon.Count() > 0)
                {
                    kiemtra = 1;
                }
            }
            return kiemtra;
        }
        public DinhKy_VuViec DinhKy_VuViec_Detail(int id, string id_encr)
        {
            DinhKy_VuViec kn = new DinhKy_VuViec();

            TD_VUVIEC don = _tiepdan.Get_TDVuviecID(id);
            if (don.ILINHVUC != null && Kiemtralinhvuc((int)don.ILINHVUC) == 1)
            {
                kn.linhvuc = "<strong>Lĩnh vực:</strong> " + _thieplap.GetBy_LinhvucID((int)don.ILINHVUC).CTEN + "; ";
            }
            else
            {
                kn.linhvuc = "<strong>Lĩnh vực:</strong> <span class='f-orangered'>Chưa xác định</span>; ";
            }
            if (don.ITINHCHAT != null && Kiemtratinhchat((int)don.ITINHCHAT) == 1)
            {
                kn.tinhchat = "<strong>Tính chất vụ việc:</strong> " + _thieplap.GetBy_TinhchatID((int)don.ITINHCHAT).CTEN + "; ";
            }
            else
            {
                kn.tinhchat = "<strong>Tính chất vụ việc:</strong> <span class='f-orangered'>Chưa xác định</span>; ";
            }
            if (don.ILOAIDON != null && Kiemtraloaidon((int)don.ILOAIDON) == 1)
            {
                kn.loaivuviec = "<strong>Loại vụ việc:</strong> " + _thieplap.GetBy_LoaidonID((int)don.ILOAIDON).CTEN + "; ";
            }
            else
            {
                kn.loaivuviec = "<strong>Loại vụ việc:</strong> <span class='f-orangered'>Chưa xác định</span>; ";
            }
            if (don.INOIDUNG != null && Kiemtranoidung((int)don.INOIDUNG) == 1)
            {
                kn.loai_noidung = "<strong>Loại nội dung:</strong> " + _thieplap.GetBy_NoidungdonID((int)don.INOIDUNG).CTEN + "; ";
            }
            else
            {
                kn.loai_noidung = "<strong>Loại nội dung:</strong> <span class='f-orangered'>Chưa xác định</span>; ";
            }
            if (don.IDOANDONGNGUOI == 1)
            {
                kn.doan_dongnguoi = "<strong>Đoàn đông người</strong> (" + don.IDOANDONGNGUOI + " người) ";
            }
            kn.bt_info = " <a href='/Tiepdan/Xemchitiet/?id=" + id_encr + "' data-original-title='Xem chi tiết' rel='tooltip' title='' class='trans_func'  onclick=\"ShowPageLoading()\" ><i class='icon-info-sign'></i></a>";

            string traloi = "";

            traloi = "<strong></strong> ";

            traloi += File_View(id, "dinhky_vuviec_traloi");
            kn.traloi = traloi;


            return kn;
        }
       
        public string OptionTiepdan(int id = 0, int idonvi = 0)
        {
            string str = "";
            var list = _tiepdan.Get_List_TiepDanDinhKy().Where(x => x.IVUVIEC == idonvi).ToList();
            foreach (var x in list)
            {
                string sel = "";
                if (id == x.IDINHKY) { sel = "selected"; }
                str += "<option value=" + x.IDINHKY + " " + sel + "> " + func.ConvertDateVN(x.DNGAYTIEP.ToString()) + " - - - " + HttpUtility.HtmlEncode(x.CDIADIEM)  + " </option>";
            }
            return str;
        }

        public tiepdan_cl Vuviec_Detail(int id, string id_encr)
        {
            tiepdan_cl kn = new tiepdan_cl();

            // new id

            // end
            TD_VUVIEC don = _tiepdan.Get_TDVuviecID(id);
            if (don.IVUVIECTRUNG != null && don.IVUVIECTRUNG != 0)
            {
                int imatrung = (int)don.IVUVIECTRUNG;
            }

            int iquoctich = (int)don.INGUOIGUI_QUOCTICH;
            if (iquoctich != 0)
            {
                kn.quoctich = _thieplap.GetBy_QuoctichID(iquoctich).CTEN;
            }
            if (Convert.ToInt32(don.INGUOIGUI_DANTOC) != 0)
            {
                kn.dantoc = _thieplap.GetBy_DantocID(Convert.ToInt32(don.INGUOIGUI_DANTOC)).CTEN;
            }
            if (don.ILINHVUC != null && Kiemtralinhvuc((int)don.ILINHVUC) == 1)
            {
                kn.linhvuc = _thieplap.GetBy_LinhvucID((int)don.ILINHVUC).CTEN;
            }
            else
            {
                kn.linhvuc = "<span class='f-orangered'>Chưa xác định</span> ";
            }
            if (don.ITINHCHAT != null && Kiemtratinhchat((int)don.ITINHCHAT) == 1)
            {
                kn.tinhchat = _thieplap.GetBy_TinhchatID((int)don.ITINHCHAT).CTEN;
            }
            else
            {
                kn.tinhchat = "<span class='f-orangered'>Chưa xác định</span> ";
            }
            if (don.ILOAIDON != null && Kiemtraloaidon((int)don.ILOAIDON) == 1)
            {
                kn.loaidon = _thieplap.GetBy_LoaidonID((int)don.ILOAIDON).CTEN;
            }
            else
            {
                kn.loaidon = " <span class='f-orangered'>Chưa xác định</span> ";
            }
            if (don.INOIDUNG != null && Kiemtranoidung((int)don.INOIDUNG) == 1)
            {
                kn.loai_noidung = _thieplap.GetBy_NoidungdonID((int)don.INOIDUNG).CTEN;
            }
            else
            {
                kn.loai_noidung = "<span class='f-orangered'>Chưa xác định</span>";
            }

            string diachi = "";
            if (don.CNGUOIGUI_DIACHI != "")
            {
                diachi = don.CNGUOIGUI_DIACHI;
            }
            if ((int)don.IDIAPHUONG_1 != 0)
            {
                kn.huyen = _thieplap.GetBy_DiaphuongID((int)don.IDIAPHUONG_1).CTEN;
                diachi += ", " + kn.huyen;
            }
            if ((int)don.IDIAPHUONG_0 > 0)
            {
                kn.tinh = _thieplap.GetBy_DiaphuongID((int)don.IDIAPHUONG_0).CTEN;
                diachi += ", " + kn.tinh;
            }
            else
            {
                kn.tinh = "";
            }
            if ((int)don.ITINHTRANGXULY == 0)
            {
                kn.tinhtrang = "Đang nghiên cứu";
            }
            if ((int)don.ITINHTRANGXULY == 1)
            {
                kn.tinhtrang = "Hướng dẫn, xử lý";

            }
            if ((int)don.ITINHTRANGXULY == 2)
            {
                kn.tinhtrang = "Chuyển xử lý";
            }
            if ((int)don.ITINHTRANGXULY == 3)
            {
                kn.tinhtrang = "Tiếp nhận đơn, xử lý nội bộ";
            }
            if ((int)don.ITINHTRANGXULY == 4)
            {
                kn.tinhtrang = "Vụ việc trùng";
            }
            if ((int)don.IDINHKY == 0)
            {
                kn.loaihinhtiep = "Tiếp thường xuyên";
            }
            else
            {
                kn.loaihinhtiep = "Tiếp định kỳ";
            }
            return kn;
        }


        //public string Dinhky_vuviec_search(TD_VUVIEC vuviec, string tungay, string denngay)
        //{
        //    string str = "";
        //    var kiennghi = _tiepdan.Get_search_dinhky(vuviec, tungay, denngay);
        //    if (kiennghi.Count() == 0)
        //    {
        //        return "<tr><td colspan='4' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào!</td></tr>";
        //    }
        //    int count = 1;
        //    string url_cookie = func.Get_Url_keycookie();
        //    foreach (var k in kiennghi)
        //    {
        //        string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
        //        DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
        //        string nguoitiep = "<p><strong>Người tiếp</strong>: " + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</p>";
        //        str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " " + File_View((int)k.IVUVIEC, "dinhky_vuviec") +
        //            "</p><p>" + nguoitiep + t.loaivuviec + t.linhvuc + t.tinhchat + t.loai_noidung + t.doan_dongnguoi + "</p>" + t.bt_info + "</td><td><p><strong>Người gửi:</strong> " + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
        //            + " (" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + ")</p></td><td>" + t.traloi + "</td></tr>";
        //        count++;
        //    }
        //    return str;
        //}

        public string Vanban_traloi_vuviec(int vuviec, string url_cookie)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IVUVIEC", vuviec);
            _condition.Add("CLOAI", "chuyenxulyvuviec");
            int dem = 1;
            var vanbantraloi = _tiepdan.Get_TDVuviecxuly(_condition).ToList();
            foreach (var x in vanbantraloi)
            {
                string id_encr = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                string print = " <a href=\"javascript:void(0)\" data-original-title='Tải mẫu phiếu trả lời' rel='tooltip' title='' class='trans_func'><i class='icon-file'></i></a> ";
                string edit = " <a href=\"javascript:void(0)\" onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_Traloi_vuviec')\" data-original-title='Sửa' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                                "','/Tiepdan/Ajax_Traloi_del','Bạn có muốn xóa trả lời này khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                str += "  <tr>" +
                "<td class='tcenter'>" + dem + "</td>" +
                "<td class='tcenter'>" + func.ConvertDateVN(x.DNGAYXULY.ToString()) + "</td>" +
                 "<td> " + HttpUtility.HtmlEncode(x.CNOIDUNG)  + " </td>" +
                "<td class='tcenter'>" + edit + del + print + "</td>" +
                                "</tr>";
                dem++;
            }
            return str;
        }
        public string Dinhky_vuviec_search_dinhky(TD_VUVIEC vuviec, string tungay, string denngay,int iUser = 0)
        {
            string str = "";
            var kiennghi = _tiepdan.Get_search_dinhky(vuviec, tungay, denngay);
            if(iUser != 0 )
            {
                kiennghi = _tiepdan.Get_search_dinhky(vuviec, tungay, denngay).Where(x=>x.IUSER == iUser).ToList();
            }
            if (kiennghi.Count() == 0)
            {
                return "<tr><td colspan='4' class='alert tcenter alert-danger'>Không tìm thấy kết quả nào!</td></tr>";
            }
            int count = 1;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var k in kiennghi)
            {
                string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
                string nguoitiep = "<p><strong>Người tiếp</strong>: " + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</p>";
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " " + File_View((int)k.IVUVIEC, "dinhky_vuviec") +
                    "</p><p>" + nguoitiep + t.loaivuviec + t.linhvuc + t.tinhchat + t.loai_noidung + t.doan_dongnguoi + "</p></td><td><p><strong>Người gửi:</strong> " + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
                    + " (" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + ")</p></td><td>" + t.traloi + "</td></tr>";
                count++;
            }
            return str;
        }
        public string Vanban_traloi_vuviec_chuyenxuly(int vuviec, string url_cookie)
        {
            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IVUVIEC", vuviec);
            _condition.Add("CLOAI", "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "");
            int dem = 1;
            var vanbantraloi = _tiepdan.Get_TDVuviecxuly(_condition).ToList();
            if (vanbantraloi.Count() == 0)
            {
                string id_encr = HashUtil.Encode_ID(vuviec.ToString(), url_cookie);
                str += " <tr><td class='nopadding' colspan='5' ><div class='alert alert-success tcenter b nomargin'><i class='icon-ok-sign'></i> Chưa có trả lời từ cơ quan chuyển xử lý </div></td></tr>";

            }
            foreach (var x in vanbantraloi)
            {
                string file = File_View((int)x.IXULY, "tiepdan_vuviec_xuly_traloi");
                string id_encr = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                string edit = " <a href=\"javascript:void(0)\" onclick=\"ShowPopUp('id=" + id_encr + "','/Tiepdan/Ajax_TraLoi_ChuyenXuLyVuViec_edit')\" data-original-title='Sửa' rel='tooltip' title='' class='trans_func'><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr +
                                "','/Tiepdan/Ajax_Traloi_del','Bạn có muốn xóa trả lời vụ việc khỏi danh sách?')\" class='trans_func'><i class='icon-trash'></i></a> ";
                str += "  <tr>" +
                "<td class='tcenter'>" + dem + "</td>" +
                "<td class='tcenter'>" + func.ConvertDateVN(x.DNGAYXULY.ToString()) + "</td>" +
                 "<td> " + HttpUtility.HtmlEncode(x.CNOIDUNG)  + " </td>" +
                 "<td class='tcenter'>" + file + "</td>" +
                "<td class='tcenter'>" + edit + del + "</td>" +
                                "</tr>";
                dem++;
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
            var list = coquan.Where(x => x.IPARENT == id_parent && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).OrderBy(x => x.IVITRI).ToList();
            foreach (var donvi in list)
            {
                if (coquan.Where(x => x.IPARENT == (int)donvi.ICOQUAN && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).Count() > 0)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(donvi.CTEN ) + "'>";
                    str += OptionCoQuanXuLy(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN ) + "</option>";
                }

            }
            return str;
        }
        // Báo cáo 
        public string tachnam(string date)
        {
            string str = "";
            string ngay = Convert.ToString(date);
            string datetime;
            string[] a = ngay.Split(' ');
            datetime = a[0];
            string[] date_split = datetime.Split('-');
            str = date_split[2].Trim();

            return str;
        }

        //public string Thongkephuluc(string tungay, string denngay)
        //{
        //    string str = "";
        //    string namtungay = tachnam(tungay);
        //    string namdenngay = tachnam(denngay);
        //    int hieucacnam = Convert.ToInt32(namdenngay) - Convert.ToInt32(namtungay);
        //    var list = tiepdanreport.getReportBaoBaoThongKeTiepDanPhuLuc("PKG_TCD_BAOCAO.PRO_BAOCAO_TK_PHULUC", tungay, denngay,iLoaiDon,iLinhVuc);
        //    decimal tongso = 0; decimal tcdlinhvuc = 0; decimal tcddbqh = 0; decimal sovuviec = 0; decimal khieunai = 0; decimal tocao = 0; decimal kiennghipa = 0;
        //    decimal hanhchinh = 0; decimal tuphap = 0; decimal doandongnguoi = 0; decimal huongdanbangvanban = 0; decimal huongdangiaithich = 0; decimal chuyencoquan = 0;
        //    if (hieucacnam == 0)
        //    {
        //        if (list.Count() > 0)
        //        {

        //            foreach (var d in list)
        //            {

        //                tongso += d.TONGSO;
        //                tcdlinhvuc += d.TIEPCONGDANTHEOLINHVUC;
        //                tcddbqh += d.TIEPCONGDANCUACANHANDBQH;
        //                sovuviec += d.SOVUVIEC;
        //                khieunai += d.KHIEUNAI;
        //                tocao += d.TOCAO;
        //                kiennghipa += d.KHIENNGHIPHANANH;
        //                hanhchinh += d.HANHCHINH;
        //                tuphap += d.TUPHAP;
        //                doandongnguoi += d.DOANDONGNGUOI;
        //                huongdanbangvanban += d.HUONGDANBANGVANBAN;
        //                huongdangiaithich += d.HUONGDANGIAITHICHTRUCTIEP;
        //                chuyencoquan += d.CHUYENDENCOQUANCOTHAMQUYEN;
        //                str += "<tr><td class='tcenter'>" + d.THOIGIAN + "</td><td class='tright'>" + d.TONGSO + "</td><td class='tright'>" + d.TIEPCONGDANTHEOLINHVUC + "</td><td  class='tright'>" + d.TIEPCONGDANCUACANHANDBQH + "</td>" +
        //                    "<td  class='tright'>" + d.SOVUVIEC + "</td><td  class='tright'>" + d.KHIEUNAI + "</td><td  class='tright'>" + d.TOCAO + "</td><td  class='tright'>" + d.KHIENNGHIPHANANH + "</td><td  class='tright'>" + d.HANHCHINH + "</td><td  class='tright'>" + d.TUPHAP + "</td>" +
        //                    "<td  class='tright'>" + d.DOANDONGNGUOI + "</td><td  class='tright'>" + d.HUONGDANBANGVANBAN + "</td><td  class='tright'>" + d.HUONGDANGIAITHICHTRUCTIEP + "</td><td  class='tright'>" + d.CHUYENDENCOQUANCOTHAMQUYEN + "</td></tr>";

        //            }

        //        }
        //        str += "<tr><td class='tcenter'>TỔNG CỘNG</td><td class='tright'>" + tongso + "</td><td class='tright'>" + tcdlinhvuc + "</td><td  class='tright'>" + tcddbqh + "</td>" +
        //            "<td  class='tright'>" + sovuviec + "</td><td  class='tright'>" + khieunai + "</td><td  class='tright'>" + tocao + "</td><td  class='tright'>" + kiennghipa + "</td><td  class='tright'>" + hanhchinh + "</td><td  class='tright'>" + tuphap + "</td>" +
        //            "<td  class='tright'>" + doandongnguoi + "</td><td  class='tright'>" + huongdanbangvanban + "</td><td  class='tright'>" + huongdangiaithich + "</td><td  class='tright'>" + chuyencoquan + "</td></tr>";
        //    }
        //    else
        //    {
        //        int i = 2017;
        //        int dem = 1;
        //        string tungaycuanam = "";
        //        string denngaycuanam = "";
        //        for (i = Convert.ToInt32(namtungay); i <= Convert.ToInt32(namdenngay); i++)
        //        {
        //            if (dem == 1 && Convert.ToInt32(namdenngay) - i > 0)
        //            {
        //                tungaycuanam = tungay;
        //                denngaycuanam = "31-Dec-" + i + "";
        //            }
        //            else if (Convert.ToInt32(namdenngay) - i == 0)
        //            {
        //                tungaycuanam = "01-Jan-" + i + "";
        //                denngaycuanam = denngay;


        //            }
        //            else
        //            {
        //                tungaycuanam = "01-Jan-" + i + "";
        //                denngaycuanam = "31-Dec-" + i + "";
        //            }
        //            list = tiepdanreport.getReportBaoBaoThongKeTiepDanPhuLuc("PKG_TCD_BAOCAO.PRO_BAOCAO_TK_PHULUC", tungaycuanam, denngaycuanam);
        //            if (list.Count() > 0)
        //            {
        //                foreach (var d in list)
        //                {
        //                    tongso += d.TONGSO;
        //                    tcdlinhvuc += d.TIEPCONGDANTHEOLINHVUC;
        //                    tcddbqh += d.TIEPCONGDANCUACANHANDBQH;
        //                    sovuviec += d.SOVUVIEC;
        //                    khieunai += d.KHIEUNAI;
        //                    tocao += d.TOCAO;
        //                    kiennghipa += d.KHIENNGHIPHANANH;
        //                    hanhchinh += d.HANHCHINH;
        //                    tuphap += d.TUPHAP;
        //                    doandongnguoi += d.DOANDONGNGUOI;
        //                    huongdanbangvanban += d.HUONGDANBANGVANBAN;
        //                    huongdangiaithich += d.HUONGDANGIAITHICHTRUCTIEP;
        //                    chuyencoquan += d.CHUYENDENCOQUANCOTHAMQUYEN;
        //                    str += "<tr><td class='tcenter'>" + d.THOIGIAN + "</td><td class='tright'>" + d.TONGSO + "</td><td class='tright'>" + d.TIEPCONGDANTHEOLINHVUC + "</td><td  class='tright'>" + d.TIEPCONGDANCUACANHANDBQH + "</td>" +
        //                        "<td  class='tright'>" + d.SOVUVIEC + "</td><td  class='tright'>" + d.KHIEUNAI + "</td><td  class='tright'>" + d.TOCAO + "</td><td  class='tright'>" + d.KHIENNGHIPHANANH + "</td><td  class='tright'>" + d.HANHCHINH + "</td><td  class='tright'>" + d.TUPHAP + "</td>" +
        //                        "<td  class='tright'>" + d.DOANDONGNGUOI + "</td><td  class='tright'>" + d.HUONGDANBANGVANBAN + "</td><td  class='tright'>" + d.HUONGDANGIAITHICHTRUCTIEP + "</td><td  class='tright'>" + d.CHUYENDENCOQUANCOTHAMQUYEN + "</td></tr>";
        //                    dem++;
        //                }
        //            }
        //        }
        //        str += "<tr><td class='tcenter'>TỔNG CỘNG</td><td class='tright'>" + tongso + "</td><td class='tright'>" + tcdlinhvuc + "</td><td  class='tright'>" + tcddbqh + "</td>" +
        //        "<td  class='tright'>" + sovuviec + "</td><td  class='tright'>" + khieunai + "</td><td  class='tright'>" + tocao + "</td><td  class='tright'>" + kiennghipa + "</td><td  class='tright'>" + hanhchinh + "</td><td  class='tright'>" + tuphap + "</td>" +
        //        "<td  class='tright'>" + doandongnguoi + "</td><td  class='tright'>" + huongdanbangvanban + "</td><td  class='tright'>" + huongdangiaithich + "</td><td  class='tright'>" + chuyencoquan + "</td></tr>";
        //    }



        //    return str;
        //}
        //public string Thongkesolieutinh(string tungay, string denngay)
        //{
        //    string str = "";
        //    int dem = 1;

        //    var list = tiepdanreport.getReportBaoBaoThongKeSoLieuTinh("PKG_TCD_BAOCAO.PRO_BAOCAO_TK_SOLIEUTINH", tungay, denngay);
        //    if (list.Count() > 0)
        //    {
        //        foreach (var d in list)
        //        {
        //            decimal tyle = 0;
        //            if (d.TYLE == 0)
        //            { tyle = 0; }
        //            else if (d.TYLE == 0 && d.SOVUVIEC == 0)
        //            {
        //                { tyle = 100; }
        //            }
        //            else
        //            {
        //                tyle = (d.TYLE / d.SOVUVIEC) * 100;
        //            }
        //            str += "<tr><td class='tcenter'>" + dem + "</td><td class='tcenter'>" + d.DIAPHUONG + "</td><td class='tright'>" + d.SOBUOITCD + "</td><td class='tright'>" + d.LUOTNGUOI + "</td><td  class='tright'>" + d.SOVUVIEC + "</td>" +
        //                "<td  class='tright'>" + d.DOANDONGNGUOI + "</td><td  class='tright'>" + d.TONGDONNHAN + "</td><td  class='tright'>" + d.KHIEUNAI + "</td><td  class='tright'>" + d.TOCAO + "</td><td  class='tright'>" + d.KIENNGHIPHANANH + "</td><td  class='tright'>" + d.DONTRUNG + "</td>" +
        //                "<td  class='tright'>" + d.DATDAI + "</td><td  class='tright'>" + d.CHINHSACHXH + "</td><td  class='tright'>" + d.VIPHAMPLTHAMNHUNG + "</td><td  class='tright'>" + d.QUANLIKINHTEXH + "</td>" +
        //               " <td class='tright'>" + d.KHAC + "</td><td class='tright'>" + d.TUPHAP + "</td><td  class='tright'>" + d.HANHCHINH + "</td>" +
        //                "<td  class='tright'>" + d.LINHVUCKHAC + "</td><td  class='tright'>" + d.DANGNGHIENCUU + "</td><td  class='tright'>" + d.SODONLUUTHEODOI + "</td><td  class='tright'>" + d.SOVUVIECDACHUYEN + "</td><td  class='tright'>" + d.SOVUVUDATRALOI + "</td><td  class='tright'>" + d.HUONGDANTRALOI + "</td>" +
        //                "<td  class='tright'>" + Math.Round(tyle, 2) + "%</td><td  class='tright'>" + d.DONDOCVUVIECCUTHE + "</td><td  class='tright'>" + d.CHUYENDE + "</td><td  class='tright'>" + d.LONGGHEP + "</td><td  class='tright'>" + d.VUVIECCUTHE + "</td></tr>";
        //            dem++;

        //        }
        //    }
        //    else
        //    {
        //        str += "<tr><td colspan='29' class='tcenter'>Chưa có dữ liệu</td></tr>";
        //    }

        //    return str;
        //}
        public string Tieudebaocaophuluc()
        {
            int demhangtiepnhan = 0;
            string str1 = "";
            var thongtinloaidon = _thieplap.Get_Loaidon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var t in thongtinloaidon)
            {
                str1 += "<th rowspan='2' style='text-align:center'>" + HttpUtility.HtmlEncode(t.CTEN)  + "</th>";
            }
            string str2 = "";
            var thongtinlinhvuc = _thieplap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();

            foreach (var x in thongtinlinhvuc)
            {
                str2 += "<th rowspan='2' style='text-align:center'>" + HttpUtility.HtmlEncode(HttpUtility.HtmlEncode(x.CTEN ) ) + "</th>";
            }
            demhangtiepnhan = 9 + thongtinloaidon.Count() + thongtinlinhvuc.Count();
            string str = "";
            str = " <tr>" +
                  "<th rowspan='4' style='text-align:center'>Thời gian" +
                  "</th>" +
                  "<th colspan='" + (demhangtiepnhan - 4) + "' style='text-align:center'>TÌNH HÌNH TIẾP CÔNG DÂN" +
                  "</th>" +
                  "<th colspan='3' style='text-align:center'>KẾT QUẢ TIẾP CÔNG DÂN" +
                  "</th>" +
                  "</tr>" +
                  "<tr>" +
                  " <th colspan='3' style='text-align:center'>Tổng số lượt tiếp" +
                  "</th>" +
                  " <th colspan=" + (demhangtiepnhan - 7) + " style='text-align:center'>Phân loại qua việc tiếp công dân" +
                  "</th>" +
                  "<th rowspan='3' style='text-align:center'>Hướng dẫn bằng văn bản" +
                  " </th>" +
                  "<th rowspan='3' style='text-align:center'>Hướng dẫn, giải thích trực tiếp" +
                  "</th>" +
                  " <th rowspan='3' style='text-align:center'>Chuyển đơn đến cơ quan có thẩm quyền" +
                  "</th>" +
                  "</tr>" +
                  " <tr>" +
                  " <th rowspan='2' style='text-align:center'>Tổng số </th>" +
                  " <th rowspan='2' style='text-align:center'>Tiếp công dân theo lĩnh vực phụ trách </th>" +
                  "<th rowspan='2' style='text-align:center'>Tiếp công dân của cá nhân đoàn ĐBQH </th>" +
                  " <th rowspan='2' style='text-align:center'>Số vụ việc </th>" +
                  "<th colspan='" + thongtinloaidon.Count() + "' style='text-align:center' >Theo loại đơn </th>" +
                  "<th colspan='" + thongtinlinhvuc.Count() + "' style='text-align:center' >Theo lĩnh vực</th>" +
                  "<th rowspan='2' style='text-align:center'>Đoàn đông người </th>" +
                  " </tr>" +
                  "<tr>" +
                  "" + str1 + str2 + "" +
                  "</tr>";
            return str;

        }
        public string Thongkephuluc2(string tungay, string denngay, List<TD_VUVIEC> tiepcongdan, int iLinhVuc, int iLoaidon, int iDonvi, List<KNTC_LOAIDON> loaidonbaocao, List<LINHVUC> linhvucbaocao)
        {
            string str = "";
            string namtungay = tachnam(tungay);
            string namdenngay = tachnam(denngay);
            int hieucacnam = Convert.ToInt32(namdenngay) - Convert.ToInt32(namtungay);
            var list = tiepdanreport.getReportBaoBaoThongKeTiepDanPhuLuc("PKG_TCD_BAOCAO.PRO_BAOCAO_TK_PHULUC", tungay, denngay, iLoaidon, iLinhVuc, iDonvi);
            decimal tongso = 0; decimal tcdlinhvuc = 0; decimal tcddbqh = 0; decimal sovuviec = 0; decimal khieunai = 0; decimal tocao = 0; decimal kiennghipa = 0;
            decimal hanhchinh = 0; decimal tuphap = 0; decimal doandongnguoi = 0; decimal huongdanbangvanban = 0; decimal huongdangiaithich = 0; decimal chuyencoquan = 0;
            if (hieucacnam == 0)
            {
                if (list.Count() > 0)
                {
                    foreach (var d in list)
                    {
                        string str1 = "";
                        var thongtinloaidon = loaidonbaocao.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                        if (iLoaidon == 0)
                        {
                            foreach (var t in thongtinloaidon)
                            {
                                var thongtintiepcongdanloaidon = tiepcongdan.Where(x => x.ILOAIDON == t.ILOAIDON && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                str1 += "<td class='tright'>" + thongtintiepcongdanloaidon.Count() + "</td>";
                            }
                        }
                        else
                        {
                            foreach (var t in thongtinloaidon)
                            {
                                var thongtintiepcongdanloaidon = tiepcongdan.Where(x => x.ILOAIDON == iLoaidon && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                if (iLoaidon != t.ILOAIDON)
                                {
                                    str1 += "<td class='tright'>0</td>";
                                }
                                else
                                {
                                    str1 += "<td class='tright'>" + thongtintiepcongdanloaidon.Count() + "</td>";
                                }

                            }
                        }
                        string str2 = "";
                        var thongtinlinhvuc = linhvucbaocao.Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
                        if (iLinhVuc == 0)
                        {
                            foreach (var t in thongtinlinhvuc)
                            {
                                var thongtinlinhvuc2 = linhvucbaocao.Where(x => x.IDELETE == 0 && x.IPARENT == t.ILINHVUC).ToList();
                                int tonggiatrilinhvuc = 0;
                                foreach (var k in thongtinlinhvuc2)
                                {
                                    var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == k.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                    tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();

                                }
                                str2 += "<td class='tright'>" + tonggiatrilinhvuc + "</td>";
                                tonggiatrilinhvuc = 0;
                            }
                        }
                        else
                        {

                            foreach (var t in thongtinlinhvuc)
                            {
                                var thongtinlinhvuc2 = linhvucbaocao.Where(x => x.IDELETE == 0 && x.IPARENT == t.ILINHVUC).ToList();
                                int tonggiatrilinhvuc = 0;
                                foreach (var k in thongtinlinhvuc2)
                                {
                                    if (iLinhVuc == k.ILINHVUC)
                                    {
                                        var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == k.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                        tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                                    }

                                }
                                str2 += "<td class='tright'>" + tonggiatrilinhvuc + "</td>";
                                tonggiatrilinhvuc = 0;

                            }
                        }

                        tongso += d.TONGSO;
                        tcdlinhvuc += d.TIEPCONGDANTHEOLINHVUC;
                        tcddbqh += d.TIEPCONGDANCUACANHANDBQH;
                        sovuviec += d.SOVUVIEC;
                        khieunai += d.KHIEUNAI;
                        tocao += d.TOCAO;
                        kiennghipa += d.KHIENNGHIPHANANH;
                        hanhchinh += d.HANHCHINH;
                        tuphap += d.TUPHAP;
                        doandongnguoi += d.DOANDONGNGUOI;
                        huongdanbangvanban += d.HUONGDANBANGVANBAN;
                        huongdangiaithich += d.HUONGDANGIAITHICHTRUCTIEP;
                        chuyencoquan += d.CHUYENDENCOQUANCOTHAMQUYEN;
                        str += "<tr><td class='tcenter'>" + d.THOIGIAN + "</td><td class='tright'>" + d.TONGSO + "</td><td class='tright'>" + d.TIEPCONGDANTHEOLINHVUC + "</td><td  class='tright'>" + d.TIEPCONGDANCUACANHANDBQH + "</td>" +
                            "<td  class='tright'>" + d.SOVUVIEC + "</td>" + str1 + "" + str2 + "" +
                            "<td  class='tright'>" + d.DOANDONGNGUOI + "</td><td  class='tright'>" + d.HUONGDANBANGVANBAN + "</td><td  class='tright'>" + d.HUONGDANGIAITHICHTRUCTIEP + "</td><td  class='tright'>" + d.CHUYENDENCOQUANCOTHAMQUYEN + "</td></tr>";

                        str += "<tr><td class='tcenter'>TỔNG CỘNG</td><td class='tright'>" + tongso + "</td><td class='tright'>" + tcdlinhvuc + "</td><td  class='tright'>" + tcddbqh + "</td>" +
                   "<td  class='tright'>" + sovuviec + "</td>" + str1 + "" + str2 + "" +
                  "<td  class='tright'>" + doandongnguoi + "</td><td  class='tright'>" + huongdanbangvanban + "</td><td  class='tright'>" + huongdangiaithich + "</td><td  class='tright'>" + chuyencoquan + "</td></tr>";
                    }
                }
            }
            else
            {
                int i = 2017;
                int dem = 1;
                string tungaycuanam = "";
                string denngaycuanam = "";
                for (i = Convert.ToInt32(namtungay); i <= Convert.ToInt32(namdenngay); i++)
                {
                    if (dem == 1 && Convert.ToInt32(namdenngay) - i > 0)
                    {
                        tungaycuanam = tungay;
                        denngaycuanam = "31-Dec-" + i + "";
                    }
                    else if (Convert.ToInt32(namdenngay) - i == 0)
                    {
                        tungaycuanam = "01-Jan-" + i + "";
                        denngaycuanam = denngay;
                    }
                    else
                    {
                        tungaycuanam = "01-Jan-" + i + "";
                        denngaycuanam = "31-Dec-" + i + "";
                    }
                    list = tiepdanreport.getReportBaoBaoThongKeTiepDanPhuLuc("PKG_TCD_BAOCAO.PRO_BAOCAO_TK_PHULUC", tungaycuanam, denngaycuanam, iLoaidon, iLinhVuc, iDonvi);
                    if (list.Count() > 0)
                    {
                        foreach (var d in list)
                        {
                            string str1 = "";
                            var thongtinloaidon = loaidonbaocao.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                            if (iLoaidon == 0)
                            {
                                foreach (var t in thongtinloaidon)
                                {
                                    var thongtintiepcongdanloaidon = tiepcongdan.Where(x => x.ILOAIDON == t.ILOAIDON && x.DNGAYNHAN >= Convert.ToDateTime(tungaycuanam) && x.DNGAYNHAN <= Convert.ToDateTime(denngaycuanam) && x.IDONVI == iDonvi).ToList();
                                    str1 += "<td class='tright'>" + thongtintiepcongdanloaidon.Count() + "</td>";
                                }
                            }
                            else
                            {
                                foreach (var t in thongtinloaidon)
                                {
                                    var thongtintiepcongdanloaidon = tiepcongdan.Where(x => x.ILOAIDON == iLoaidon && x.DNGAYNHAN >= Convert.ToDateTime(tungaycuanam) && x.DNGAYNHAN <= Convert.ToDateTime(denngaycuanam) && x.IDONVI == iDonvi).ToList();
                                    if (iLoaidon != t.ILOAIDON)
                                    {
                                        str1 += "<td class='tright'>0</td>";
                                    }
                                    else
                                    {
                                        str1 += "<td class='tright'>" + thongtintiepcongdanloaidon.Count() + "</td>";
                                    }
                                }
                            }
                            string str2 = "";
                            var thongtinlinhvuc = linhvucbaocao.Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
                            if (iLinhVuc == 0)
                            {
                                foreach (var t in thongtinlinhvuc)
                                {
                                    var thongtinlinhvuc2 = linhvucbaocao.Where(x => x.IDELETE == 0 && x.IPARENT == t.ILINHVUC).ToList();
                                    int tonggiatrilinhvuc = 0;
                                    foreach (var k in thongtinlinhvuc2)
                                    {
                                        var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == k.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungaycuanam) && x.DNGAYNHAN <= Convert.ToDateTime(denngaycuanam) && x.IDONVI == iDonvi).ToList();
                                        tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                                    }
                                    str2 += "<td class='tright'>" + tonggiatrilinhvuc + "</td>";
                                    tonggiatrilinhvuc = 0;
                                }
                            }
                            else
                            {

                                foreach (var t in thongtinlinhvuc)
                                {
                                    var thongtinlinhvuc2 = linhvucbaocao.Where(x => x.IDELETE == 0 && x.IPARENT == t.ILINHVUC).ToList();
                                    int tonggiatrilinhvuc = 0;
                                    foreach (var k in thongtinlinhvuc2)
                                    {
                                        if (iLinhVuc == k.ILINHVUC)
                                        {
                                            var thongtintiepcongdanlinhvuc = tiepcongdan.Where(x => x.ILINHVUC == iLinhVuc && x.DNGAYNHAN >= Convert.ToDateTime(tungaycuanam) && x.DNGAYNHAN <= Convert.ToDateTime(denngaycuanam) && x.IDONVI == iDonvi).ToList();
                                            tonggiatrilinhvuc += thongtintiepcongdanlinhvuc.Count();
                                        }

                                    }
                                    str2 += "<td class='tright'>" + tonggiatrilinhvuc + "</td>";
                                    tonggiatrilinhvuc = 0;

                                }
                            }
                            tongso += d.TONGSO;
                            tcdlinhvuc += d.TIEPCONGDANTHEOLINHVUC;
                            tcddbqh += d.TIEPCONGDANCUACANHANDBQH;
                            sovuviec += d.SOVUVIEC;
                            khieunai += d.KHIEUNAI;
                            tocao += d.TOCAO;
                            kiennghipa += d.KHIENNGHIPHANANH;
                            hanhchinh += d.HANHCHINH;
                            tuphap += d.TUPHAP;
                            doandongnguoi += d.DOANDONGNGUOI;
                            huongdanbangvanban += d.HUONGDANBANGVANBAN;
                            huongdangiaithich += d.HUONGDANGIAITHICHTRUCTIEP;
                            chuyencoquan += d.CHUYENDENCOQUANCOTHAMQUYEN;
                            str += "<tr><td class='tcenter'>" + d.THOIGIAN + "</td><td class='tright'>" + d.TONGSO + "</td><td class='tright'>" + d.TIEPCONGDANTHEOLINHVUC + "</td><td  class='tright'>" + d.TIEPCONGDANCUACANHANDBQH + "</td>" +
                             "<td  class='tright'>" + d.SOVUVIEC + "</td>" + str1 + "" + str2 + "" +
                             "<td  class='tright'>" + d.DOANDONGNGUOI + "</td><td  class='tright'>" + d.HUONGDANBANGVANBAN + "</td><td  class='tright'>" + d.HUONGDANGIAITHICHTRUCTIEP + "</td><td  class='tright'>" + d.CHUYENDENCOQUANCOTHAMQUYEN + "</td></tr>";

                        }
                    }
                }
                string strtongcongloaidon = "";
                var thongtinloaidontongcong = loaidonbaocao.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                if (iLoaidon == 0)
                {
                    foreach (var t in thongtinloaidontongcong)
                    {

                        var thongtintiepcongdanloaidontongcong = tiepcongdan.Where(x => x.ILOAIDON == t.ILOAIDON && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                        strtongcongloaidon += "<td class='tright'>" + thongtintiepcongdanloaidontongcong.Count() + "</td>";
                    }
                }
                else
                {
                    foreach (var t in thongtinloaidontongcong)
                    {
                        var thongtintiepcongdanloaidontongcong = tiepcongdan.Where(x => x.ILOAIDON == iLoaidon && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                        if (t.ILOAIDON != iLoaidon)
                        {
                            strtongcongloaidon += "<td class='tright'>0</td>";
                        }
                        else
                        {
                            strtongcongloaidon += "<td class='tright'>" + thongtintiepcongdanloaidontongcong.Count() + "</td>";
                        }

                    }
                }

                string strtongconglinhvuc = "";
                var thongtinlinhvuctong = linhvucbaocao.Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
                if (iLinhVuc == 0)
                {
                    foreach (var t in thongtinlinhvuctong)
                    {
                        var thongtinlinhvuctong2 = linhvucbaocao.Where(x => x.IDELETE == 0 && x.IPARENT == t.ILINHVUC).ToList();
                        int tonggiatrilinhvuc = 0;
                        foreach (var k in thongtinlinhvuctong2)
                        {
                            var thongtintiepcongdanlinhvuctongcong = tiepcongdan.Where(x => x.ILINHVUC == k.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                            tonggiatrilinhvuc += thongtintiepcongdanlinhvuctongcong.Count();
                        }
                        strtongconglinhvuc += "<td class='tright'>" + tonggiatrilinhvuc + "</td>";
                        tonggiatrilinhvuc = 0;
                        //  var thongtintiepcongdanlinhvuctongcong = tiepcongdan.Where(x => x.ILINHVUC == t.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay)).ToList();
                        //   strtongconglinhvuc += "<td class='tright'>" + thongtintiepcongdanlinhvuctongcong.Count() + "</td>";
                    }
                }
                else
                {
                    foreach (var t in thongtinlinhvuctong)
                    {
                        var thongtinlinhvuctong2 = linhvucbaocao.Where(x => x.IDELETE == 0 && x.IPARENT == t.ILINHVUC).ToList();
                        int tonggiatrilinhvuc = 0;
                        foreach (var k in thongtinlinhvuctong2)
                        {
                            if (iLinhVuc == k.ILINHVUC)
                            {
                                var thongtintiepcongdanlinhvuctongcong = tiepcongdan.Where(x => x.ILINHVUC == iLinhVuc && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDONVI == iDonvi).ToList();
                                tonggiatrilinhvuc += thongtintiepcongdanlinhvuctongcong.Count();
                            }

                        }
                        strtongconglinhvuc += "<td class='tright'>" + tonggiatrilinhvuc + "</td>";
                        tonggiatrilinhvuc = 0;

                    }
                }
                if (str != "")
                {
                    str += "<tr><td class='tcenter'>TỔNG CỘNG</td><td class='tright'>" + tongso + "</td><td class='tright'>" + tcdlinhvuc + "</td><td  class='tright'>" + tcddbqh + "</td>" +
           "<td  class='tright'>" + sovuviec + "</td></td>" + strtongcongloaidon + "" + strtongconglinhvuc + "" +
           "<td  class='tright'>" + doandongnguoi + "</td><td  class='tright'>" + huongdanbangvanban + "</td><td  class='tright'>" + huongdangiaithich + "</td><td  class='tright'>" + chuyencoquan + "</td></tr>";
                }
            }
            return str;
        }

        public string Tieudebaocaosolieutinh()
        {
            string str1 = "";
            var thongtinloaidon = _thieplap.Get_Loaidon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            int demhangtiepnhan = 0;
            foreach (var t in thongtinloaidon)
            {
                str1 += "<th rowspan='3' style='text-align:center'>" + HttpUtility.HtmlEncode(t.CTEN)  + "</th>";
            }
            string str2 = "";
            var thongtinnoidungdon = _thieplap.Get_Noidungdon().Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var k in thongtinnoidungdon)
            {

                str2 += "<th rowspan='2' style='text-align:center'>" +HttpUtility.HtmlEncode(k.CTEN )  + "</th>";

            }
            string str3 = "";
            var thongtinlinhvuc = _thieplap.Get_Linhvuc().Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var k in thongtinlinhvuc)
            {

                str3 += "<th rowspan='2' style='text-align:center'>" +HttpUtility.HtmlEncode(k.CTEN )  + "</th>";

            }
            demhangtiepnhan = 1 + thongtinloaidon.Count() + thongtinnoidungdon.Count() + thongtinlinhvuc.Count();
            string str = "<tr>" +
                        "<th rowspan='4' style='text-align:center;width:3%'>STT" +
                         "</th>" +
                         "<th rowspan='4' style='text-align:center;width:5%'>" +
                         "Địa phương" +
                         " </th>" +
                         "<th colspan='4' rowspan='2' style='text-align:center;width:12%'>TIẾP CÔNG DÂN" +
                         "</th>" +
                         "<th colspan='" + demhangtiepnhan + "' style='text-align:center;width:50%'>TIẾP NHẬN ĐƠN THƯ" +
                         "</th>" +
                         "<th colspan='7'  style='text-align:center;width:25%'>KẾT QUẢ XỬ LÝ" +
                         "</th>" +
                         "<th colspan='3' rowspan='2' style='text-align:center;'>GIÁM SÁT" +
                         "</th>" +
                         "</tr>" +
                         "<tr>" +
                         "" + str1 + " " +
                         "<th rowspan='3' style='text-align:center'>Đơn trùng" +
                         "</th>" +
                         "<th  style='text-align:center' colspan='" + (thongtinnoidungdon.Count()) + "'>" +
                         "Phân loại theo nội dung" +
                         "</th>" +
                         "<th  style='text-align:center' colspan='3'>" +
                         " Phân loại theo lĩnh vực" +
                         "</th>" +
                         "<th rowspan='3' style='text-align:center'>Đang nghiên cứu" +
                         "</th>" +
                         "<th rowspan='3' style='text-align:center'>Số đơn lưu theo dõi" +
                         "</th>" +
                         "<th rowspan='3' style='text-align:center'>Số vụ việc đã chuyển" +
                         "</th>" +
                         "<th rowspan='3' style='text-align:center'>Số vụ việc đã được thông tin trả lời" +
                         "</th>" +
                         "<th rowspan='3' style='text-align:center'>Hướng dẫn, giải thích trả lời" +
                         "</th>" +
                         "<th rowspan='3' style='text-align:center'>Tỉ lệ đã xử lý/đơn nhận" +
                         "</th>" +
                         "<th rowspan='3' style='text-align:center'>Đơn đôn đốc vụ việc cụ thể" +
                          "</th>" +
                          " </tr>" +
                          "<tr>" +
                          "<th rowspan='2' style='text-align:center'>Số buổi TCD </th>" +
                          " <th rowspan='2' style='text-align:center'>Lượt người </th>" +
                          "<th rowspan='2' style='text-align:center'>Số vụ viêc</th>" +
                          "<th rowspan='2' style='text-align:center'>Đoàn đông người </th>" +
                          "" + str2 + "" + str3 + "" +
                          "<th rowspan='2' style='text-align:center'>Chuyên đề</th>" +
                          "<th rowspan='2' style='text-align:center' >Lồng ghép</th>" +
                          " <th rowspan='2' style='text-align:center'>Vụ việc cụ thể</th>" +
                          "</tr>" +
                          "<tr>" +
                          "</tr>";
            return str;
        }

        public string Thongkesolieutinh2(string tungay, string denngay, int iloaidonbaocao, int ilinhvucbaocao, int inoidungdungbaocao, int idonvi, List<TD_VUVIEC> vuviec, List<KNTC_LOAIDON> loaidon, List<KNTC_NOIDUNGDON> noidungdon, List<LINHVUC> linhvuc)
        {
            string str = "";
            int dem = 1;
            int tonghang = 0;
            var list = tiepdanreport.getReportBaoBaoThongKeSoLieuTinh("PKG_TCD_BAOCAO.PRO_BAOCAO_TK_SOLIEUTINH", tungay, denngay, iloaidonbaocao, ilinhvucbaocao, idonvi);
            if (list.Count() > 0)
            {
                foreach (var d in list)
                {
                    string str1 = "";
                    var thongtinloaidon = loaidon.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                    foreach (var t in thongtinloaidon)
                    {
                        if (iloaidonbaocao == 0)
                        {
                            var vuviecloaidon = vuviec.Where(v => v.IDIAPHUONG_0 == d.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.ILOAIDON == t.ILOAIDON && v.IDONVI == idonvi && v.IDONVI == idonvi).ToList();
                            int giatri = vuviecloaidon.Count();
                            str1 += "<td  class='tright'>" + giatri + "</td>";
                            giatri = 0;
                        }
                        else
                        {
                            if (iloaidonbaocao == t.ILOAIDON)
                            {
                                var vuviec1 = vuviec.Where(v => v.IDIAPHUONG_0 == d.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.ILOAIDON == iloaidonbaocao && v.IDONVI == idonvi && v.IDONVI == idonvi).ToList();
                                int giatri = vuviec1.Count();
                                str1 += "<td  class='tright'>" + giatri + "</td>";
                                giatri = 0;
                            }
                            else
                            {
                                str1 += "<td  class='tright'>0</td>";
                            }
                        }

                    }

                    string str2 = "";
                    var thongtinnoidungdon = noidungdon.Where(x => x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
                    foreach (var k in thongtinnoidungdon)
                    {
                        if (inoidungdungbaocao == 0)
                        {
                            var vuviecnoidung = vuviec.Where(v => v.IDIAPHUONG_0 == d.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.INOIDUNG == k.INOIDUNG && v.IDONVI == idonvi && v.ILOAIDON == iloaidonbaocao && v.ILINHVUC == ilinhvucbaocao).ToList();
                            if (ilinhvucbaocao == 0 && iloaidonbaocao != 0)
                            {
                                vuviecnoidung = vuviec.Where(v => v.IDIAPHUONG_0 == d.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.INOIDUNG == k.INOIDUNG && v.IDONVI == idonvi && v.ILOAIDON == iloaidonbaocao).ToList();
                            }
                            if (ilinhvucbaocao != 0 && iloaidonbaocao == 0)
                            {
                                vuviecnoidung = vuviec.Where(v => v.IDIAPHUONG_0 == d.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.INOIDUNG == k.INOIDUNG && v.IDONVI == idonvi && v.ILINHVUC == ilinhvucbaocao).ToList();
                            }
                            if (iloaidonbaocao == 0 && ilinhvucbaocao == 0)
                            {
                                vuviecnoidung = vuviec.Where(v => v.IDIAPHUONG_0 == d.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.INOIDUNG == k.INOIDUNG && v.IDONVI == idonvi).ToList();
                            }
                            int giatri = vuviecnoidung.Count();
                            str2 += "<td  class='tright'>" + giatri + "</td>";
                            giatri = 0;
                        }
                        else
                        {
                            if (inoidungdungbaocao == k.INOIDUNG)
                            {
                                var vuviec2 = vuviec.Where(v => v.IDIAPHUONG_0 == d.IDDIAPHUONG && v.DNGAYNHAN >= Convert.ToDateTime(tungay) && v.DNGAYNHAN <= Convert.ToDateTime(denngay) && v.INOIDUNG == inoidungdungbaocao && v.IDONVI == idonvi && v.ILOAIDON == iloaidonbaocao && v.ILINHVUC == ilinhvucbaocao).ToList();
                                int giatri = vuviec2.Count();
                                str2 += "<td  class='tright'>" + giatri + "</td>";
                                giatri = 0;
                            }
                            else
                            {
                                str2 += "<td  class='tright'>0</td>";
                            }
                        }

                    }
                    string str3 = "";
                    var thongtinlinhvuc = linhvuc.Where(x => x.IDELETE == 0 && x.IPARENT == 0).OrderBy(x => x.IVITRI).ToList();
                    foreach (var k in thongtinlinhvuc)
                    {
                        if (ilinhvucbaocao == 0)
                        {
                            var thongtinlinhvuctong2 = linhvuc.Where(x => x.IDELETE == 0 && x.IPARENT == k.ILINHVUC).ToList();
                            int tonggiatrilinhvuc = 0;
                            foreach (var t in thongtinlinhvuctong2)
                            {

                                var vuvieclinhvuc = vuviec.Where(x => x.ILINHVUC == t.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDIAPHUONG_0 == d.IDDIAPHUONG && x.IDONVI == idonvi && x.ILOAIDON == iloaidonbaocao).ToList();
                                if (iloaidonbaocao == 0)
                                {
                                    vuvieclinhvuc = vuviec.Where(x => x.ILINHVUC == t.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDIAPHUONG_0 == d.IDDIAPHUONG && x.IDONVI == idonvi).ToList();
                                }
                                tonggiatrilinhvuc += vuvieclinhvuc.Count();


                            }
                            str3 += "<td class='tright'>" + tonggiatrilinhvuc + "</td>";
                            tonggiatrilinhvuc = 0;

                        }
                        else
                        {
                            var thongtinlinhvuctong2 = linhvuc.Where(x => x.IDELETE == 0 && x.IPARENT == k.ILINHVUC).ToList();
                            int tonggiatrilinhvuc = 0;
                            foreach (var t in thongtinlinhvuctong2)
                            {
                                if (ilinhvucbaocao == t.ILINHVUC)
                                {
                                    var vuvieclinhvuc = vuviec.Where(x => x.ILINHVUC == t.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDIAPHUONG_0 == d.IDDIAPHUONG && x.IDONVI == idonvi && x.ILOAIDON == iloaidonbaocao).ToList();
                                    if (iloaidonbaocao == 0)
                                    {
                                        vuvieclinhvuc = vuviec.Where(x => x.ILINHVUC == t.ILINHVUC && x.DNGAYNHAN >= Convert.ToDateTime(tungay) && x.DNGAYNHAN <= Convert.ToDateTime(denngay) && x.IDIAPHUONG_0 == d.IDDIAPHUONG && x.IDONVI == idonvi).ToList();
                                    }
                                    tonggiatrilinhvuc += vuvieclinhvuc.Count();
                                }
                            }
                            str3 += "<td class='tright'>" + tonggiatrilinhvuc + "</td>";
                            tonggiatrilinhvuc = 0;
                        }


                    }
                    tonghang = 17 + thongtinnoidungdon.Count() + thongtinloaidon.Count() + thongtinlinhvuc.Count();
                    decimal tyle = 0;
                    if (d.TYLE == 0)
                    { tyle = 0; }
                    else if (d.TYLE == 0 && d.SOVUVIEC == 0)
                    {
                        { tyle = 100; }
                    }
                    else
                    {
                        tyle = (d.TYLE / d.SOVUVIEC) * 100;
                    }
                    str += "<tr><td class='tcenter'>" + dem + "</td><td class='tcenter'>" + d.DIAPHUONG + "</td><td class='tright'>" + d.SOBUOITCD + "</td><td class='tright'>" + d.LUOTNGUOI + "</td><td  class='tright'>" + d.SOVUVIEC + "</td>" +
                        "<td  class='tright'>" + d.DOANDONGNGUOI + "</td>" + str1 + "<td  class='tright'>" + d.DONTRUNG + "</td>" +
                        "" + str2 + "" + str3 + "<td  class='tright'>" + d.DANGNGHIENCUU + "</td><td  class='tright'>" + d.SODONLUUTHEODOI + "</td><td  class='tright'>" + d.SOVUVIECDACHUYEN + "</td><td  class='tright'>" + d.SOVUVUDATRALOI + "</td><td  class='tright'>" + d.HUONGDANTRALOI + "</td>" +
                        "<td  class='tright'>" + Math.Round(tyle, 2) + "%</td><td  class='tright'>" + d.DONDOCVUVIECCUTHE + "</td><td  class='tright'>" + d.CHUYENDE + "</td><td  class='tright'>" + d.LONGGHEP + "</td><td  class='tright'>" + d.VUVIECCUTHE + "</td></tr>";
                    dem++;

                }
            }
            else
            {
                str += "<tr><td colspan='" + tonghang + "' class='tcenter'>Chưa có dữ liệu</td></tr>";
            }

            return str;
        }
        public string Option_LinhVuc_LoaiDon(int id_chucvu = 0, int iLoaiDon = 0)
        {
            string str = "";
            List<LINHVUC> chucvu = _thieplap.Get_Linhvuc().Where(v => v.IPARENT == 0 && v.IDELETE == 0 && v.IHIENTHI == 1).ToList();

            foreach (var d in chucvu)
            {
                chucvu = _thieplap.Get_Linhvuc().Where(x => x.IPARENT == d.ILINHVUC).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN ) ).ToList();
                if (iLoaiDon != 0)
                {
                    chucvu = _thieplap.Get_Linhvuc().Where(x => x.ILOAIDON == iLoaiDon && x.IPARENT == d.ILINHVUC).OrderBy(x => HttpUtility.HtmlEncode(x.CTEN ) ).ToList();
                }
                if (chucvu.Count() > 0)
                {
                    str += "<optgroup label='" + d.CTEN + "'>";

                    foreach (var p in chucvu)
                    {
                        string select = ""; if (p.ILINHVUC == id_chucvu) { select = " selected "; }
                        str += "<option " + select + " value='" + p.ILINHVUC + "'>" + HttpUtility.HtmlEncode(p.CTEN )+ "</option>";
                    }
                }

            }

            return str;
        }
        public string OptionCoQuan_BaoCao(List<QUOCHOI_COQUAN> coquan, int id_parent = 0, int level = 0, int id_donvi = 0, int id_donvi_choice = 0)
        {
            string str = "";
            string space_level = "";
            for (int i = 0; i < level; i++)
            {
                space_level += "- - - ";
            }
            //var list = _coquan.GetList("select * from QUOCHOI_COQUAN WHERE IPARENT=" + id_parent + " and ICOQUAN!=" + id_donvi_choice).OrderBy(x => x.IVITRI).ToList();
            var list = coquan.Where(x => x.IPARENT == id_parent && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1 && x.IDELETE == 0).OrderBy(x => x.IVITRI).ToList();
            foreach (var donvi in list)
            {


                if (coquan.Where(x => x.IPARENT == (int)donvi.ICOQUAN && x.ICOQUAN != id_donvi_choice && x.IHIENTHI == 1).Count() > 0)
                {
                    str += "<optgroup label='" + HttpUtility.HtmlEncode(donvi.CTEN ) + "'>";
                    str += OptionCoQuan_BaoCao(coquan, (int)donvi.ICOQUAN, level + 1, id_donvi, id_donvi_choice);
                    str += "</optgroup>";
                }
                else
                {
                    string select = "";
                    if (donvi.ICOQUAN == id_donvi) { select = " selected "; }
                    str += "<option " + select + " value='" + donvi.ICOQUAN + "'>" + space_level + HttpUtility.HtmlEncode(donvi.CTEN ) + "</option>";
                }

            }
            return str;
        }


        // Bổ sung enum tối 05/03/2018
        public string Dinhky_vuviec(List<TD_VUVIEC> vuviec, List<TD_VUVIEC_XULY> Xuly, int iUser, int iDinhKy, string url_cookie, int id_donvi = 0, int page = 0)
        {

            string id_encr_dinhky = HashUtil.Encode_ID(iDinhKy.ToString(), url_cookie);
            string str = "";
            string trangthaixuly = "";
            var thongtinvuviec = vuviec.Where(v => v.IDINHKY == iDinhKy ).OrderByDescending(x => x.IVUVIEC).ToPagedList(page, pageSize).ToList();
            // var thongtinvuviec = vuviec.Where(v => v.IDINHKY == iDinhKy && (id_donvi == 4 || id_donvi == v.IDONVI)).OrderByDescending(x => x.IVUVIEC).ToPagedList(page, pageSize).ToList();
            int total = vuviec.Where(v => v.IDINHKY == iDinhKy ).OrderByDescending(x => x.IVUVIEC).ToPagedList(page, pageSize).ToList().Count();
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  != null)
                {
                    diachinguoigui = "" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + " , ";
                }
                if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IVUVIEC, (int)k.ITINHTRANGXULY, iUser, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.ITINHTRANGXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);

                    }

                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = ""; del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản "+title+"' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
               
                string nguoitiep = "<i>" + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                var listtinh = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_0).ToList();
                if (listtinh.Count() > 0)
                {
                    thongtintinh = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_0).CTEN;
                }
                var listhuyen = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_1).ToList();
                if (listhuyen.Count() > 0)
                {
                    thongtinhuyen = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_1).CTEN + " , ";
                }
                if (k.IVUVIECTRUNG != 0 && k.IVUVIECTRUNG != null)
                {

                    string id_encr2 = HashUtil.Encode_ID(k.IVUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }
                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.IGIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sủa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.IGIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.DNGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.IGIAMSAT) + "</p><p>" + k.CYKIENGIAMSAT + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }
            str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
                 "/Tiepdan/Dinhky_vuviec/?id=" + id_encr_dinhky + "&") + "</td></tr>";
            return str;
        }
        public string Dinhky_vuviec_search(List<TD_VUVIEC> thongtinvuviec, List<TD_VUVIEC_XULY> Xuly, int iUser, int iDinhKy, string url_cookie, int idem, int ivuviec, int idonvi)
        {

            string id_encr_dinhky = HashUtil.Encode_ID(iDinhKy.ToString(), url_cookie);
            string str = "";
            string trangthaixuly = "";
            var vuviec = thongtinvuviec.Where(v => v.IDINHKY == iDinhKy && v.IVUVIEC == ivuviec).OrderByDescending(x => x.IVUVIEC).ToList();
            if (vuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in vuviec)
            {
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
               "<ul class='dropdown-menu dropdown-success'>" +
                "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
               "<li><a href='/Tiepdan/Word_Nhandon/" + k.IVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
               "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
               "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  != null)
                {
                    diachinguoigui = "" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + " , ";
                }
                if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IVUVIEC, (int)k.ITINHTRANGXULY, iUser, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.ITINHTRANGXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = ""; del = "";
                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = ""; del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời  xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời  xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản "+title+"' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
               
                string nguoitiep = "<i>" + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                var listtinh = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_0).ToList();
                if (listtinh.Count() > 0)
                {
                    thongtintinh = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_0).CTEN;
                }
                var listhuyen = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_1).ToList();
                if (listhuyen.Count() > 0)
                {
                    thongtinhuyen = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_1).CTEN + " , ";
                }
                if (k.IVUVIECTRUNG != 0 && k.IVUVIECTRUNG != null)
                {

                    string id_encr2 = HashUtil.Encode_ID(k.IVUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }
                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.IGIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sủa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.IGIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + idem + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.DNGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.IGIAMSAT) + "</p><p>" + k.CYKIENGIAMSAT + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }

            return str;
        }
        public string Thuongxuyen_vuviec_search(List<TD_VUVIEC> thongtinvuviec, List<TD_VUVIEC_XULY> Xuly, int iUser, string url_cookie, int idem, int ivuviec, int idonvi)
        {

            string str = "";
            _condition = new Dictionary<string, object>();
            _condition.Add("IVUVIEC", ivuviec);
            string trangthaixuly = "";
            var vuviec = thongtinvuviec.Where(v => v.IDINHKY == 0 && v.IVUVIEC == ivuviec).OrderByDescending(x => x.IVUVIEC).ToList();
            //  int total = _tiepdan.GetBy_List_TDVuViec(_condition).Where(v => v.IDINHKY == 0 && (idonvi == 4 || idonvi == v.IDONVI)).OrderByDescending(x => x.IVUVIEC).ToPagedList(page, pageSize).ToList().Count();
            if (vuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in vuviec)
            {

                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
               "<ul class='dropdown-menu dropdown-success'>" +
                "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
               "<li><a href='/Tiepdan/Word_Nhandon/" + k.IVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
               "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
               "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  != null)
                {
                    diachinguoigui = "" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + " , ";
                }
                if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IVUVIEC, (int)k.ITINHTRANGXULY, iUser, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.ITINHTRANGXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = ""; del = "";

                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = ""; del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời  xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời  xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title+ "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
               
                string nguoitiep = "<i>" + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                var listtinh = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_0).ToList();
                if (listtinh.Count() > 0)
                {
                    thongtintinh = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_0).CTEN;
                }
                var listhuyen = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_1).ToList();
                if (listhuyen.Count() > 0)
                {
                    thongtinhuyen = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_1).CTEN + " , ";
                }
                if (k.IVUVIECTRUNG != 0 && k.IVUVIECTRUNG != null)
                {
                    //   thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.IVUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.IGIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sủa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.IGIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + idem + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.DNGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.IGIAMSAT) + "</p><p>" + k.CYKIENGIAMSAT + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }
            return str;
        }
        public string Thuongxuyen_vuviec(List<TD_VUVIEC> vuviec, List<TD_VUVIEC_XULY> Xuly, int iUser, string url_cookie, int id_donvi = 0, int page = 0)
        {

            string str = "";
            string trangthaixuly = "";
            var thongtinvuviec = vuviec.Where(v => v.IDINHKY == 0).OrderByDescending(x => x.IVUVIEC).ToPagedList(page, pageSize).ToList();
            int total = vuviec.Where(v => v.IDINHKY == 0 ).OrderByDescending(x => x.IVUVIEC).ToList().Count();
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  != null)
                {
                    diachinguoigui = "" + HttpUtility.HtmlEncode(k.CNGUOIGUI_DIACHI)  + " , ";
                }
                if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IVUVIEC, (int)k.ITINHTRANGXULY, iUser, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.CNOIDUNG)  + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.ITINHTRANGXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = "";
                        del = "";

                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                        edit = "";
                        del = "";
                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = "";
                        del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
              
                string nguoitiep = "<i>" + HttpUtility.HtmlEncode(k.CNGUOITIEP)  + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                var listtinh = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_0).ToList();
                if (listtinh.Count() > 0)
                {
                    thongtintinh = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_0).CTEN;
                }
                var listhuyen = _thieplap.Get_Diaphuong().Where(x => x.IDIAPHUONG == k.IDIAPHUONG_1).ToList();
                if (listhuyen.Count() > 0)
                {
                    thongtinhuyen = _thieplap.GetBy_DiaphuongID((int)k.IDIAPHUONG_1).CTEN + " , ";
                }
                if (k.IVUVIECTRUNG != 0 && k.IVUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.IVUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.ITINHTRANGXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.IGIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.IGIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.DNGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.CNGUOIGUI_TEN) 
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.CNOIDUNG)  + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.IGIAMSAT) + "</p><p>" + k.CYKIENGIAMSAT + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }
            str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
          "/Tiepdan/Thuongxuyen/?") + "</td></tr>";
            return str;
        }

        public string TIEPDANVUVIECTHUONGXUYEN(List<TD_VUVIEC_XULY> Xuly,int iUser = 0, int id_donvi = 0, int page = 0, string url_cookie = "",int iuserkq= 0)
        {

            string str = "";
            string trangthaixuly = "";
            var thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            if(id_donvi == 0 )
            {
                thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == 0 && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            }
            int total = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == 0 && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
            if (iUser != 0 && id_donvi != 0)
            {
                total = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.USER_ID == iUser && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.USER_ID == iUser && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            }
            if (iUser == 0 && id_donvi != 0)
            {
                total = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            }
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
               // int trangthai = (int)k.TRANGTHAIXULY;
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IDVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IDVUVIEC, (int)k.TRANGTHAIXULY, iuserkq, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode( k.NOIDUNGVUVIEC ) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = "";
                        del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IDVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
              
                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if(k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH +"  ";
                }
                if(k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.VUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" +HttpUtility.HtmlEncode( k.TENNGUOIGUI )
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode( k.NOIDUNGVUVIEC ) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.GIAMSAT) + "</p><p>" + HttpUtility.HtmlEncode( k.YKIENGIAMSAT) + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }
            str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
          "/Tiepdan/Thuongxuyen/?") + "</td></tr>";
            return str;
        }

        public string TIEPDANVUVIECTRACUU_THUONGXUYEN(List<TD_VUVIEC_XULY> Xuly, int iUser = 0, int id_donvi = 0, string url_cookie = "", int iuserkq = 0,string tukhoa ="")
        {

            string str = "";
            string trangthaixuly = "";
            var thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.TIEPDOTXUAT != 0).OrderByDescending(x => x.NGAYNHAN).ToList();
           // int total = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC").Where(x => x.DINHKY == 0).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
            if(id_donvi == 0)
            {
                thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.DINHKY == 0 && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (iUser != 0 && id_donvi != 0)
            {
              //  total = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.USER_ID == iUser).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.USER_ID == iUser && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (iUser == 0 && id_donvi != 0)
            {
              //  total = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                // int trangthai = (int)k.TRANGTHAIXULY;
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IDVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IDVUVIEC, (int)k.TRANGTHAIXULY, iuserkq, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode( k.NOIDUNGVUVIEC ) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = "";
                        del = "";

                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }

                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = "";
                        del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IDVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
              
                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "  ";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.VUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" +HttpUtility.HtmlEncode( k.TENNGUOIGUI )
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode( k.NOIDUNGVUVIEC ) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.GIAMSAT) + "</p><p>" + HttpUtility.HtmlEncode( k.YKIENGIAMSAT) + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }

            return "<tr><td colspan='8' class='alert tcenter alert-info'> Có "+ thongtinvuviec.Count()+" kết quả tìm kiếm</td></tr>" + str;
        }
        public string TIEPDANVUVIECTRACUU_DINHKY(List<TD_VUVIEC_XULY> Xuly, int iUser = 0, int id_donvi = 0, string url_cookie = "", int iuserkq = 0, string tukhoa = "",int idinhky=0)
        {

            string str = "";
            string trangthaixuly = "";
            var thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.DINHKY == idinhky && x.IDONVI == id_donvi && x.TIEPDOTXUAT != 0).OrderByDescending(x => x.NGAYNHAN).ToList();
            // int total = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC").Where(x => x.DINHKY == 0).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
            if(id_donvi == 0)
            {
                thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.DINHKY == idinhky && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (iUser != 0 && id_donvi != 0)
            {
                //  total = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.USER_ID == iUser).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.DINHKY == idinhky && x.IDONVI == id_donvi && x.USER_ID == iUser && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (iUser == 0 && id_donvi != 0)
            {
                //  total = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.DINHKY == idinhky && x.IDONVI == id_donvi && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                // int trangthai = (int)k.TRANGTHAIXULY;
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IDVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IDVUVIEC, (int)k.TRANGTHAIXULY, iuserkq, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode( k.NOIDUNGVUVIEC ) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = "";
                        del = "";

                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = "";
                        del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IDVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
               
                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "  ";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.VUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" +HttpUtility.HtmlEncode( k.TENNGUOIGUI )
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode( k.NOIDUNGVUVIEC ) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.GIAMSAT) + "</p><p>" + HttpUtility.HtmlEncode( k.YKIENGIAMSAT) + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }

            return "<tr><td colspan='8' class='alert tcenter alert-info'> Có " + thongtinvuviec.Count() + " kết quả tìm kiếm</td></tr>" + str;
        }
        // TRA CỨU 

        // ĐỊNH KỲ
        public string TIEPDANVUVIECDINHKY(List<TD_VUVIEC_XULY> Xuly, int iUser = 0, int id_donvi = 0, int page = 0, string url_cookie = "", int iuserkq = 0,int idinhky = 0)
        {

            string str = "";
            string trangthaixuly = "";
            var thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == idinhky && x.IDONVI == id_donvi && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            int total = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == idinhky && x.IDONVI == id_donvi && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
            if (id_donvi == 0)
            {
                thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == idinhky && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            }
            if (iUser != 0 && id_donvi != 0)
            {
                total = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == idinhky && x.IDONVI == id_donvi && x.USER_ID == iUser && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == idinhky && x.IDONVI == id_donvi && x.USER_ID == iUser && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            }
            if (iUser == 0 && id_donvi != 0)
            {
                total = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == idinhky && x.IDONVI == id_donvi && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY == idinhky && x.IDONVI == id_donvi && x.TIEPDOTXUAT == 0).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            }
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                // int trangthai = (int)k.TRANGTHAIXULY;
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IDVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IDVUVIEC, (int)k.TRANGTHAIXULY, iuserkq, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode( k.NOIDUNGVUVIEC ) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = "";
                        del = "";

                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                        edit = "";
                        del = "";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IDVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";
             
                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.VUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" +HttpUtility.HtmlEncode( k.TENNGUOIGUI )
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode( k.NOIDUNGVUVIEC ) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.GIAMSAT) + "</p><p>" + HttpUtility.HtmlEncode( k.YKIENGIAMSAT) + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }
            str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
          "/Tiepdan/Thuongxuyen/?") + "</td></tr>";
            return str;
        }





        public string TIEPDAN_TRACUUNANGCAO( TD_VUVIEC vuviec, List<TD_VUVIEC_XULY> Xuly, int iUser = 0, int id_donvi = 0, string url_cookie = "", int iuserkq = 0,string ngaybatdau="", string ngayketthuc="",int idinhky=0)
        {

            string str = "";
            string trangthaixuly = "";
            var thongtinvuviec = tiepdanreport.GetListTraCuu_NangCao("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIEC_TRACUU", vuviec, ngaybatdau, ngayketthuc).Where(x=>x.IDONVI == id_donvi).OrderByDescending(x => x.NGAYNHAN).ToList();
           if(id_donvi == 0)
           {
               thongtinvuviec = tiepdanreport.GetListTraCuu_NangCao("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIEC_TRACUU", vuviec, ngaybatdau, ngayketthuc).OrderByDescending(x => x.NGAYNHAN).ToList();
           }
            if (iUser != 0 && id_donvi != 0)
            {

                thongtinvuviec = tiepdanreport.GetListTraCuu_NangCao("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIEC_TRACUU", vuviec, ngaybatdau, ngayketthuc).Where(x =>x.IDONVI == id_donvi && x.USER_ID == iUser).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (iUser == 0 && id_donvi != 0)
            {

                thongtinvuviec = tiepdanreport.GetListTraCuu_NangCao("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIEC_TRACUU",vuviec, ngaybatdau, ngayketthuc).Where(x => x.IDONVI == id_donvi).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                // int trangthai = (int)k.TRANGTHAIXULY;
               
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                string ketqua ="";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.DangNghienCuu) + "</p>";
                }
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                      
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                      
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI == "traloichuyenxuly" && x.IVUVIEC == k.IDVUVIEC && k.TRANGTHAIXULY == 3).ToList();
                    if(thongtintraloi.Count() > 0)
                    {
                        ketqua = "Đã có trả lời";
                    }
                }

                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    vuviectrung = " <i style='color:red'>[ Vụ việc trùng ]</i> ";
                }
                thongtinhienthitrangthai = trangthaixuly;

               
              
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" +HttpUtility.HtmlEncode( k.TENNGUOIGUI )
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode( k.NOIDUNGVUVIEC ) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p><strong>" + thongtinhienthitrangthai + "</strong></p>" + t.loaivuviec + t.linhvuc + t.tinhchat + t.loai_noidung + t.doan_dongnguoi + "</td><td class='tcenter b'>" + ketqua + " </td></tr>";
                count++;
                trangthaixuly = "";
                traloichuyenxuly = "";
                ketqua = "";
            }

            return "<tr><td colspan='6' class='alert tcenter alert-info'> Có " + thongtinvuviec.Count() + " kết quả tìm kiếm</td></tr>" + str;
        }




        // 
        public string optin_hinhthuc()
        {
           
            string str = "";

            str = "<option value='-1'>- - - Chọn tất cả</option><option value='0'>- - - " + StringEnum.GetStringValue(TrangThaiXuLy.DangNghienCuu) + "</option>" +
                    "<option value='1'>- - - " + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</option>"
                    + "<option value='2'>- - - " + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</option>"
                    + "<option value='3'>- - - " + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</option>";
            return str;
        }


        // Bổ sung ngày 11/04
        public string TIEPDANVUVIECDOTXUAT(List<TD_VUVIEC_XULY> Xuly, int iUser = 0, int id_donvi = 0, int page = 0, string url_cookie = "", int iuserkq = 0)
        {

            string str = "";
            string trangthaixuly = "";
            var thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.TIEPDOTXUAT != 0 && x.IDONVI == id_donvi).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            if (id_donvi == 0)
            {
                thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.TIEPDOTXUAT != 0).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            }
            int total = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.TIEPDOTXUAT != 0).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
            if (iUser != 0 && id_donvi != 0)
            {
                total = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.TIEPDOTXUAT != 0 && x.IDONVI == id_donvi && x.USER_ID == iUser).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.TIEPDOTXUAT != 0 && x.IDONVI == id_donvi && x.USER_ID == iUser).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            }
            if (iUser == 0 && id_donvi != 0)
            {
                total = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY != 0 && x.IDONVI == id_donvi).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.getListDanhSachTiepCongDan("PKG_TD_VUVIEC.PRC_PHANTRANG_TIEPDANVUVIEC").Where(x => x.DINHKY != 0 && x.IDONVI == id_donvi).OrderByDescending(x => x.NGAYNHAN).ToPagedList(page, pageSize).ToList();
            }
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                // int trangthai = (int)k.TRANGTHAIXULY;
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IDVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IDVUVIEC, (int)k.TRANGTHAIXULY, iuserkq, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = "";
                        del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }

                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IDVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";

                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "  ";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.VUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.TENNGUOIGUI)
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.GIAMSAT) + "</p><p>" + HttpUtility.HtmlEncode(k.YKIENGIAMSAT) + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }
            str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
          "/Tiepdan/Thuongxuyen/?") + "</td></tr>";
            return str;
        }


        public string TIEPDANVUVIECTRACUU_DOTXUAT(List<TD_VUVIEC_XULY> Xuly, int iUser = 0, int id_donvi = 0, string url_cookie = "", int iuserkq = 0, string tukhoa = "", int idinhky = 0)
        {

            string str = "";
            string trangthaixuly = "";
            var thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.TIEPDOTXUAT != 0 && x.IDONVI == id_donvi).OrderByDescending(x => x.NGAYNHAN).ToList();
            // int total = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC").Where(x => x.DINHKY == 0).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
            if (id_donvi == 0)
            {
                thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.TIEPDOTXUAT != 0).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (iUser != 0 && id_donvi != 0)
            {
                //  total = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi && x.USER_ID == iUser).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.TIEPDOTXUAT != 0 && x.IDONVI == id_donvi && x.USER_ID == iUser).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (iUser == 0 && id_donvi != 0)
            {
                //  total = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC").Where(x => x.DINHKY == 0 && x.IDONVI == id_donvi).OrderByDescending(x => x.IDVUVIEC).ToList().Count();
                thongtinvuviec = tiepdanreport.GetListTraCuu("PKG_TD_VUVIEC.PRC_TRACUUVUVIEC", tukhoa).Where(x => x.TIEPDOTXUAT != 0 && x.IDONVI == id_donvi).OrderByDescending(x => x.NGAYNHAN).ToList();
            }
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                // int trangthai = (int)k.TRANGTHAIXULY;
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IDVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IDVUVIEC, (int)k.TRANGTHAIXULY, iuserkq, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = "";
                        del = "";

                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = "";
                        del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IDVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";

                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "  ";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.VUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.TENNGUOIGUI)
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.GIAMSAT) + "</p><p>" + HttpUtility.HtmlEncode(k.YKIENGIAMSAT) + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }

            return "<tr><td colspan='8' class='alert tcenter alert-info'> Có " + thongtinvuviec.Count() + " kết quả tìm kiếm</td></tr>" + str;
        }




        // Bổ sung phân trang 
        public string TIEPDANVUVIECTHUONGXUYEN_PHANTRANG(List<TIEPCONGDAN_DANHMUC> thongtinvuviec, List<TD_VUVIEC_XULY> Xuly, string url_cookie = "")
        {

            string str = "";
            string trangthaixuly = "";
            int total = 0;
            //var thongtinvuviec = tiepdanreport.GETLISTTIEPCONGDAN("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIECDANHSACH", ThongTinVuViec, ngaybatdau, ngayketthuc, page, pageSize,ThongTinVuViec.CNOIDUNG, -1, -1,0, iUser, id_donvi).OrderByDescending(x => x.NGAYNHAN).ToList();
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                total = (int)k.TOTAL;
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IDVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IDVUVIEC, (int)k.TRANGTHAIXULY, (int)k.USER_ID, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = "";
                        del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IDVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";

                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "  ";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.VUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.TENNGUOIGUI)
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.GIAMSAT) + "</p><p>" + HttpUtility.HtmlEncode(k.YKIENGIAMSAT) + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }
          //  str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
          //"/Tiepdan/Thuongxuyen/?") + "</td></tr>";
            return str;
        }
        public string TIEPDANVUVIECTHUONGXUYEN_PHANTRANG_XOATAMTHOI(List<TIEPCONGDAN_DANHMUC> thongtinvuviec, List<TD_VUVIEC_XULY> Xuly, string url_cookie = "")
        {

            string str = "";
            string trangthaixuly = "";
            int total = 0;
            //var thongtinvuviec = tiepdanreport.GETLISTTIEPCONGDAN("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIECDANHSACH", ThongTinVuViec, ngaybatdau, ngayketthuc, page, pageSize,ThongTinVuViec.CNOIDUNG, -1, -1,0, iUser, id_donvi).OrderByDescending(x => x.NGAYNHAN).ToList();
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                total = (int)k.TOTAL;
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IDVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.DangNghienCuu) + "</p>";
                }
                string traloi = "";
                string title = "";
                string khoiphuc = " <a href=\"javascript:void()\" data-original-title='Khôi phục' rel='tooltip' title='' onclick=\"Return_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_KhoiPhuc', 'Bạn có muốn khôi phục vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + "')\"  class='trans_func'><i class='icon-signout'></i></a> ";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                      //  edit = "";
                      
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                     //   edit = "";
                      
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        //edit = ""; 
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        //edit = "";
                      
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IDVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";

                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "  ";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.VUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + khoiphuc + edit + del + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + khoiphuc + edit + del + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + khoiphuc +edit+ del + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + khoiphuc+edit + del +  "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.TENNGUOIGUI)
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }
            //  str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
            //"/Tiepdan/Thuongxuyen/?") + "</td></tr>";
            return str;
        }
        public string TIEPDANVUVIECDOTXUAT_PHANTRANG(List<TIEPCONGDAN_DANHMUC> thongtinvuviec, List<TD_VUVIEC_XULY> Xuly, string url_cookie = "")
        {

            string str = "";
            string trangthaixuly = "";
            int total = 0;
            //var thongtinvuviec = tiepdanreport.GETLISTTIEPCONGDAN("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIECDANHSACH", ThongTinVuViec, ngaybatdau, ngayketthuc, page, pageSize,ThongTinVuViec.CNOIDUNG, -1, -1,0, iUser, id_donvi).OrderByDescending(x => x.NGAYNHAN).ToList();
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                total = (int)k.TOTAL;
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IDVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IDVUVIEC, (int)k.TRANGTHAIXULY, (int)k.USER_ID, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = "";
                        del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IDVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";

                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "  ";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.VUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.TENNGUOIGUI)
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.GIAMSAT) + "</p><p>" + HttpUtility.HtmlEncode(k.YKIENGIAMSAT) + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }
            //  str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
            //"/Tiepdan/Thuongxuyen/?") + "</td></tr>";
            return str;
        }
        public string TIEPDANVUVIECDINHKY_PHANTRANG(List<TIEPCONGDAN_DANHMUC> thongtinvuviec, List<TD_VUVIEC_XULY> Xuly, string url_cookie = "")
        {

            string str = "";
            string trangthaixuly = "";
            int total = 0;
            //var thongtinvuviec = tiepdanreport.GETLISTTIEPCONGDAN("PKG_TD_VUVIEC.PRC_TIEPDAN_VUVIECDANHSACH", ThongTinVuViec, ngaybatdau, ngayketthuc, page, pageSize,ThongTinVuViec.CNOIDUNG, -1, -1,0, iUser, id_donvi).OrderByDescending(x => x.NGAYNHAN).ToList();
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            foreach (var k in thongtinvuviec)
            {
                total = (int)k.TOTAL;
                string mauphieutraloi = "<div class='btn-group'><a href='#' data-toggle='dropdown' class='btn btn-success dropdown-toggle'>Chọn mẫu đơn<span class='caret'></span></a>" +
                "<ul class='dropdown-menu dropdown-success'>" +
                 "<li><a href='/Tiepdan/Word_Phieuhuongdan/" + k.IDVUVIEC + "' >Phiếu hướng dẫn</a></li>" +
                "<li><a href='/Tiepdan/Word_Nhandon/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</a></li>" +
                "<li><a href='/Tiepdan/Word_Chuyenxuly/" + k.IDVUVIEC + "'>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</a></li>" +
                "</ul></div>";
                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string diachinguoigui = "";
                string traloichuyenxuly = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = KetQuaXuLy((int)k.IDVUVIEC, (int)k.TRANGTHAIXULY, (int)k.USER_ID, id_encr);
                }
                string traloi = "";
                string title = "";
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string del = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_del', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.NhanDon);
                        edit = "";
                        del = "";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {
                        edit = ""; del = "";
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep);
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        edit = "";
                        del = "";
                        title = StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy);
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                        traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'    data-original-title=' Văn bản xử lý' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI != "" + StringEnum.GetStringValue(TrangThaiChuyenXuLy.TraLoiChuyenXuLy) + "" && x.IVUVIEC == k.IDVUVIEC).ToList();
                    foreach (var x in thongtintraloi)
                    {
                        string id_encr2 = HashUtil.Encode_ID(x.IXULY.ToString(), url_cookie);
                        if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                        {
                            if (thongtintraloi.Count() != 0)
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"><i class='icon-plus-sign'></i></a>";
                            }
                            else
                            {
                                traloichuyenxuly = "<a  href='/Tiepdan/Traloixuly/?id=" + id_encr + "'   data-original-title='Kết quả trả lời xử lý' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\"  ><i class='icon-building'></i></a>";
                            }
                        }
                        traloi = "<a  href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + id_encr2 + "','/Tiepdan/Ajax_Traloi_vuviec')\"   data-original-title='Văn bản " + title + "' rel='tooltip' title=''  class='trans_func'><i class='icon-file'></i></a> ";
                    }
                }
                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string kiemtrung = " <a href='/Tiepdan/Kiemtrung/?id=" + id_encr + "'  data-original-title='kiểm trùng' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-search'></i></a> ";

                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "  ";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    //  thongtinhienthitrangthai = "<p>Vụ việc trùng</p>";
                    string id_encr2 = HashUtil.Encode_ID(k.VUVIECTRUNG.ToString(), url_cookie);
                    vuviectrung = "<a href='/Tiepdan/Xemchitiet/?id=" + id_encr2 + "'  data-original-title='Xem chi tiết vụ việc trùng' rel='tooltip' title=''  class='trans_func' style='font-size:12px;color:red' onclick=\"ShowPageLoading()\" >[ <i class='icon-copy'></i> Xem vụ việc trùng  ]</a> ";
                }

                thongtinhienthitrangthai = trangthaixuly + " " + traloi + traloichuyenxuly;

                string hienthi_chucnang = "";
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    string thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title='Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + kiemtrung + edit + del + thongtingiamsat + "";

                    }
                }
                else
                {
                    string thongtingiamsat = "<a  href=\"javascript:void()\"   onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec')\"  class='trans_func' data-original-title='Giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-in'></i></a>";
                    if (k.GIAMSAT != 0)
                    {
                        thongtingiamsat = "<a   href=\"javascript:void()\"  onclick=\"ShowPopUp('id=" + k.IDVUVIEC + "','/Tiepdan/Giamsat_vuviec_edit')\"  class='trans_func' data-original-title=' Sửa giám sát vụ việc' rel='tooltip' title=''  ><i class='icon-zoom-out'></i></a>";
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";
                    }
                    else
                    {
                        hienthi_chucnang = "" + t.bt_info + edit + del + thongtingiamsat + "";

                    }
                }
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.TENNGUOIGUI)
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p>" + ketqua_thongtingiamsat((int)k.GIAMSAT) + "</p><p>" + HttpUtility.HtmlEncode(k.YKIENGIAMSAT) + "</p></td><td class='tcenter b'>" + thongtinhienthitrangthai + " </td><td class='tcenter' nowrap>" + mauphieutraloi + "</td><td class='tcenter' nowrap>" + hienthi_chucnang + "</td></tr>";
                count++;
                trangthaixuly = "";
                traloi = "";
                traloichuyenxuly = "";
            }
            //  str += "<tr><td colspan='8'>" + PhanTrang(total, pageSize, page, "" +
            //"/Tiepdan/Thuongxuyen/?") + "</td></tr>";
            return str;
        }
        public string TIEPDANTRACUU(List<TIEPCONGDAN_TRACUU_VUVIEC> thongtinvuviec, List<TD_VUVIEC_XULY> Xuly, string ngaybatdau = "", string ngayketthuc = "")
        {

            string str = "";
            string trangthaixuly = "";
            if (thongtinvuviec.Count() == 0)
            {
                return "";
            }
            int count = 1;
            string url_cookie = func.Get_Url_keycookie();
            foreach (var k in thongtinvuviec)
            {
                // int trangthai = (int)k.TRANGTHAIXULY;

                string id_encr = HashUtil.Encode_ID(k.IDVUVIEC.ToString(), url_cookie);
                string edit = " <a href='/Tiepdan/Sua/?id=" + id_encr + "'  data-original-title='Sửa' rel='tooltip' title=''  class='trans_func' onclick=\"ShowPageLoading()\" ><i class='icon-pencil'></i></a> ";
                string diachinguoigui = "";
                string ketqua = "";
                if (k.DIACHINGUOIGUI != null)
                {
                    diachinguoigui = "" + k.DIACHINGUOIGUI + " , ";
                }
                if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.DangNghienCuu)
                {
                    trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.DangNghienCuu) + "</p>";
                }
                if (k.TRANGTHAIXULY != (decimal)TrangThaiXuLy.DangNghienCuu)
                {

                    if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanXuLy) + "</p>";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.NhanDon)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.NhanDon) + "</p>";
                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.HuongDanTrucTiep)
                    {

                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.HuongDanTrucTiep) + "</p>";

                    }
                    else if (k.TRANGTHAIXULY == (decimal)TrangThaiXuLy.ChuyenXuLy)
                    {
                        trangthaixuly = "<p>" + StringEnum.GetStringValue(TrangThaiXuLy.ChuyenXuLy) + "</p>";
                    }
                    var thongtintraloi = Xuly.Where(x => x.CLOAI == "traloichuyenxuly" && x.IVUVIEC == k.IDVUVIEC && k.TRANGTHAIXULY == 3).ToList();
                    if (thongtintraloi.Count() > 0)
                    {
                        ketqua = "Đã có trả lời";
                    }
                }

                DinhKy_VuViec t = DinhKy_VuViec_Detail((int)k.IDVUVIEC, id_encr);
                string nguoitiep = "<i>" + k.TENNGUOITIEP + "</i>";
                string vuviectrung = "";
                string thongtinhienthitrangthai = "";
                string thongtintinh = "";
                string thongtinhuyen = "";
                if (k.TENTINH != null)
                {
                    thongtintinh = k.TENTINH + "";
                }
                if (k.TENHUYEN != null)
                {
                    thongtinhuyen = k.TENHUYEN + " , ";
                }
                if (k.VUVIECTRUNG != 0 && k.VUVIECTRUNG != null)
                {
                    vuviectrung = " <i style='color:red'>[ Vụ việc trùng ]</i> ";
                }
                string del_vuviec = " <a href=\"javascript:void()\" data-original-title='Xóa' rel='tooltip' title='' onclick=\"DeletePage_Confirm_TraCuu('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_deltamthoi', 'Bạn có muốn xóa vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + " khỏi danh sách?')\"  class='trans_func'><i class='icon-trash'></i></a> ";
                if(k.DELVUVIEC == 1)
                {
                     del_vuviec = " <a href=\"javascript:void()\" data-original-title='Khôi phục' rel='tooltip' title='' onclick=\"Return_Confirm('" + id_encr + "','id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_KhoiPhuc', 'Bạn có muốn khôi phục vụ viêc với nội dung  " + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + "')\"  class='trans_func'><i class='icon-signout'></i></a> ";
                     ketqua += "<p>Xóa thạm thời</p>";
                }
                thongtinhienthitrangthai = trangthaixuly;
                str += "<tr id='tr_" + id_encr + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b'><p>" + func.ConvertDateVN(k.NGAYNHAN.ToString()) + "</p></td><td><p>" + HttpUtility.HtmlEncode(k.TENNGUOIGUI)
               + "</p>" + diachinguoigui + "" + thongtinhuyen + "" + thongtintinh + " </td><td><p>" + HttpUtility.HtmlEncode(k.NOIDUNGVUVIEC) + "" +
               "</p>" + nguoitiep + " " + vuviectrung + "  </i></td><td><p><strong>" + thongtinhienthitrangthai + "</strong></p>" + t.loaivuviec + t.linhvuc + t.tinhchat + t.loai_noidung + t.doan_dongnguoi + "</td><td class='tcenter b'>" + ketqua + " </td>"+
               "<td class='tcenter'>" + edit + del_vuviec + " </td></tr>";
                count++;
                trangthaixuly = "";
                ketqua = "";
            }

            return "<tr><td colspan='7' class='alert tcenter alert-info'> Có " + thongtinvuviec.FirstOrDefault().TOTAL + " kết quả tìm kiếm</td></tr>" + str;
        }
        public string List_DonTrung_KTNC(List<KNTC_DON> list, int id_kntc = 0, int id_vuviec = 0, string url_cookie = "")
        {
            string str = "";


            int count = 1;

            foreach (var d in list)
            {
                //string check = "";
                string id_encr = HashUtil.Encode_ID(id_vuviec.ToString(), url_cookie);
                string id_encr_trung = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string chon = "<a id='btnkntc_" + id_encr_trung + "' data-original-title='Chọn đơn trùng' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonDonTrungKNTC('" + id_encr_trung + "','id_trung=" + id_encr_trung + "&id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_update_KNTC')\" class='chontrungkntc f-grey'><i class='icon-ok-sign'></i></a>";
                if (id_kntc == d.IDON)
                {
                    chon = "<a id='btnkntc_" + id_encr_trung + "' data-original-title='Bỏ chọn trùng' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonDonTrungKNTC('" + id_encr_trung + "','id_trung=" + id_encr_trung + "&id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_update_KNTC')\" class='trans_func chontrungkntc'><i class='icon-ok-sign'></i></a>";
                }
                //string chon_trung = "<input type='radio' " + check + " name='dontrung' id='dontrung' value='" + d.IDON + "'/>";
                string tinhtrangdon = "";
                KNTC k = KNTC_Detail((int)d.IDON, id_encr);
                if (k != null)
                {
                    str += "<tr><td class='tcenter b'>" + count + "</td><td class='tcenter'>" + chon + "</td><td>" + d.CNOIDUNG +
                   "</td><td><p><strong>" + d.CNGUOIGUI_TEN + "</strong></p>" + DiaChi_Don((int)d.IDIAPHUONG_0, (int)d.IDIAPHUONG_1, d.CNGUOIGUI_DIACHI) +
                   "</td><td>" + tinhtrangdon + "</td><td class='tcenter'>" + k.bt_info + "</td></tr>";
                    count++;
                }

            }

            return str;
        }
        public string List_DonTrung_KTNC2(List<KNTC_DON> list, int id_kntc = 0, int id_vuviec = 0, string url_cookie = "")
        {
            string str = "";


            int count = 1;

            foreach (var d in list)
            {
                //string check = "";
                string id_encr = HashUtil.Encode_ID(id_vuviec.ToString(), url_cookie);
                string id_encr_trung = HashUtil.Encode_ID(d.IDON.ToString(), url_cookie);
                string chon = "<a id='btnkntc_" + id_encr_trung + "' data-original-title='Chọn đơn trùng' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonDonTrungKNTC2('" + id_encr_trung + "','id_trung=" + id_encr_trung + "&id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_updateKNTC2')\" class='chontrungkntc f-grey'><i class='icon-ok-sign'></i></a>";
                if (id_kntc == d.IDON)
                {
                    chon = "<a id='btnkntc_" + id_encr_trung + "' data-original-title='Bỏ chọn trùng' rel='tooltip' title='' href='javascript:void(0)' onclick=\"ChonDonTrungKNTC2('" + id_encr_trung + "','id_trung=" + id_encr_trung + "&id=" + id_encr + "','/Tiepdan/Ajax_Vuviec_updateKNTC2')\" class='trans_func chontrungkntc'><i class='icon-ok-sign'></i></a>";
                }
                //string chon_trung = "<input type='radio' " + check + " name='dontrung' id='dontrung' value='" + d.IDON + "'/>";
                string tinhtrangdon = "";
                KNTC k = KNTC_Detail((int)d.IDON, id_encr);
                if (k != null)
                {
                    str += "<tr><td class='tcenter b'>" + count + "</td><td class='tcenter'>" + chon + "</td><td>" + d.CNOIDUNG +
                   "</td><td><p><strong>" + d.CNGUOIGUI_TEN + "</strong></p>" + DiaChi_Don((int)d.IDIAPHUONG_0, (int)d.IDIAPHUONG_1, d.CNGUOIGUI_DIACHI) +
                   "</td><td>" + tinhtrangdon + "</td><td class='tcenter'>" + k.bt_info + "</td></tr>";
                    count++;
                }

            }

            return str;
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
                DIAPHUONG d1 = _kntc.Get_DiaPhuong(diaphuong1);
                DIAPHUONG d0 = _kntc.Get_DiaPhuong(diaphuong0);
                if (d0 != null && d0 != null)
                {
                    str += d1.CTYPE + " " + d1.CTEN + ", " + d0.CTYPE + " " + d0.CTEN;
                }
            }
            else
            {
                DIAPHUONG d0 = _kntc.Get_DiaPhuong(diaphuong0);
                if (d0 != null)
                {
                    str += d0.CTYPE + " " + d0.CTEN;
                }

            }
            return str;
        }
        public KNTC KNTC_Detail(int id, string id_encr)
        {
            KNTC kn = new KNTC();
            KNTC_DON don = _kntc.GetDON(id);
            if (don != null)
            {
                int iquoctich = (int)don.INGUOIGUI_QUOCTICH;
                if (iquoctich != 0)
                {
                    var k = _kntc.Get_QuocTich(iquoctich);
                    if (k != null)
                    {
                        kn.quoctich = k.CTEN;
                    }

                }
                if (Convert.ToInt32(don.INGUOIGUI_DANTOC) != 0)
                {
                    kn.dantoc = _kntc.Get_DanToc(Convert.ToInt32(don.INGUOIGUI_DANTOC)).CTEN;
                }
                if (don.ILINHVUC != 0)
                {
                    var k = _kntc.Get_LinhVuc((int)don.ILINHVUC);
                    if (k != null)
                    {
                        kn.linhvuc = k.CTEN;
                    }

                }
                if (don.ITINHCHAT != 0)
                {
                    var k = _kntc.Get_TinhChat((int)don.ITINHCHAT);
                    if (k != null)
                    {
                        kn.tinhchat = k.CTEN;
                    }

                }
                if (don.ILOAIDON != 0)
                {
                    var k = _kntc.Get_LoaiDon((int)don.ILOAIDON);
                    if (k != null)
                    {
                        kn.loaidon = k.CTEN;
                    }
                }
                if (don.INOIDUNG != 0)
                {
                    var k = _kntc.Get_NoiDungDon((int)don.INOIDUNG);
                    if (k != null)
                    {
                        kn.loai_noidung = k.CTEN;
                    }
                }
                if (don.INGUONDON != null)
                {

                    if (don.INGUONDON != 0)
                    {
                        var k = _kntc.Get_NguonDon((int)don.INGUONDON);
                        kn.nguondon = k.CTEN;
                    }
                    else
                    {
                        kn.nguondon = "Chưa xác định";
                    }

                }

                string tu = "";
                TD_VUVIEC vuviec = _kntc.Get_DonKNTCByIDTCD(id);
                if (vuviec != null)
                {
                    kn.nguon = "Tiếp công dân";
                }
                string diachi = "";
                if (don.CNGUOIGUI_DIACHI != null)
                {
                    diachi = don.CNGUOIGUI_DIACHI + ", ";
                }
                if (don.IDIAPHUONG_1 != 0)
                {
                    var k = _kntc.Get_DiaPhuong((int)don.IDIAPHUONG_1);
                    if (k != null)
                    {
                        kn.huyen = k.CTEN;
                    }
                    diachi += kn.huyen + ", ";
                }
                if (don.IDIAPHUONG_0 != 0)
                {
                    if ((int)don.IDIAPHUONG_0 != -1)
                    {
                        var k = _kntc.Get_DiaPhuong((int)don.IDIAPHUONG_0);
                        if (k != null)
                        {
                            kn.tinh = k.CTEN;
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
                if (don.IDONVITHULY != 0)
                {
                    QUOCHOI_COQUAN cq = _kntc.GetDonVi((int)don.IDONVITHULY);
                    if (cq != null)
                    {
                        kn.donvi_thuly = cq.CTEN;
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
                if (don.ISOLUONGTRUNG != null)
                {
                    Dictionary<string, object> _condition = new Dictionary<string, object>();
                    _condition.Add("IDONTRUNG", don.IDON);
                    kn.sodontrung = don.ISOLUONGTRUNG + _kntc.List_Don(_condition).ToList().Count;
                }
                ////  int giamsat = kntc.List_GiamSat().Where(v => v.IDON == id).ToList().Count();
            }
            return kn;
        }

        public string  Option_TaiKhoanDonVi(int idonvi = 0, int iuser = 0)
        {
            string str = "";
            var List = _thieplap.Get_Taikhoan().Where(x => x.IDONVI == idonvi).ToList();
            foreach(var x in List)
            {
                var thongtinvuviec = _tiepdan.Get_TDVuviec().Where(v => v.IUSER == x.IUSER).ToList();
                if(thongtinvuviec != null && thongtinvuviec.Count() > 0)
                {
                    string sel = "";
                    if (x.IUSER == iuser)
                    {
                        sel = "selected";
                    }
                    str += "<option value='" + x.IUSER + "' " + sel + "> " + x.CUSERNAME + " - - - " + x.CTEN + " </option>";
                    sel = "";
                }
            }
            return str;
        }
    }
}
