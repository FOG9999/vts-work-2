using DataAccess.Dao;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Entities;
using System.IO;
using Entities.Objects;
using Utilities.Enums;

namespace DataAccess.Busineess
{
    public class BaseBusineess
    {
        //Base _base = new Base();
        UsserRepository _user = new UsserRepository();
        TokenRepository _token = new TokenRepository();
        Quochoi_KhoaRepository _quochoi_khoa = new Quochoi_KhoaRepository();
        Quochoi_KyhopRepository _quochoi_kyhop = new Quochoi_KyhopRepository();
        User_ActionRepository _user_action = new User_ActionRepository();
        TrackingRepository _tracking = new TrackingRepository();
        Quochoi_CoquanRepository _quochoi_coquan = new Quochoi_CoquanRepository();
        KN_KiennghiRepository _kiennghi = new KN_KiennghiRepository();
        public TaikhoanAtion tk_action(int id_user)
        {
            TaikhoanAtion t = new TaikhoanAtion();
            USERS u = GetByIDUser(id_user);
            QUOCHOI_COQUAN user_coquan = _quochoi_coquan.GetByID((int)u.IDONVI);
            User_PhongbanRepository user_phongban = new User_PhongbanRepository();
            USER_PHONGBAN phongban = user_phongban.GetByID((int)u.IPHONGBAN);
            if (phongban != null)
            {
                t.tenphongban = phongban.CTEN;
            }
            t.is_admin = false; t.is_root = false; t.is_lanhdao = false; t.is_chuyenvien = false; t.is_dbqh = false;
            if (u.ITYPE == Convert.ToDecimal(UserType.RootAdmin))
            {
                t.is_root = true;
                t.is_admin = true;
                t.is_lanhdao = true;
            }
            else if (u.ITYPE == Convert.ToDecimal(UserType.Admin))
            {
                t.is_admin = true;
                t.is_lanhdao = true;
            }
            else if (u.ITYPE == Convert.ToDecimal(UserType.LanhDao))
            {
                t.is_lanhdao = true;
            }
            else if (u.ITYPE == Convert.ToDecimal(UserType.ChuyenVienHDND) || u.ITYPE == Convert.ToDecimal(UserType.ChuyenVienDBQH))
            {
                t.is_chuyenvien = true;
            }
            else if (u.ITYPE == Convert.ToDecimal(UserType.ChuyenVienDBQH))
            {
                t.is_dbqh = true;
            }
            
            t.is_groupquochoi = (int)user_coquan.IPARENT;
            t.tendonvi = user_coquan.CTEN;
            t.iUser = id_user;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("IUSER", id_user);

            User_ActionRepository act = new User_ActionRepository();
            List<USER_ACTION> a = act.GetAll(dictionary);
            StringBuilder str = new StringBuilder();
            if (a.Count > 0)
            {
                str.Append("|");
                foreach (var a1 in a)
                {
                    str.Append(a1.IACTION + "|");
                }
            }
            t.list_action = str.ToString();
            return t;
        }
        public USERS GetByIDUser(int iD)
        {
            return _user.GetByID(iD);
        }
        public USERS InsertUser(USERS input)
        {
            return _user.AddNew(input);
        }
        public Boolean UpdateUser(USERS input)
        {
            return _user.Update(input);
        }
        public Boolean DelteUser(USERS input)
        {
            return _user.Delete(input);
        }
        public TOKEN GetByIDToken(int iD)
        {
            return _token.GetByID(iD); ;
        }
        public TOKEN InsertToken(TOKEN input)
        {
            return _token.AddNew(input);
        }
        public Boolean UpdateToken(TOKEN input)
        {
            return _token.Update(input);
        }
        public List<TOKEN> GetAllToken(Dictionary<string, object> dictionary)
        {
            return _token.GetAllWhere(dictionary);
        }
        public List<TOKEN> GetListToken(String sql)
        {
            return _token.GetList(sql);
        }
        public Boolean DeleteToken(TOKEN input)
        {
            return _token.Delete(input);
        }
        public int Get_ID_KyHop_HienTai()
        {
            int ID = 0;
            Dictionary<string, object> dic_ = new Dictionary<string, object>();
            dic_.Add("IMACDINH", 1);
            var kyhop = _quochoi_kyhop.GetAll(dic_);
            if (kyhop != null)
            {
                ID = Convert.ToInt32(kyhop.FirstOrDefault()?.IKYHOP);
            }
            else
            {
                ID = 0;
            }
            return ID;
        }
        public int Get_ID_KhoaHop_HienTai()
        {
            int ID = 0;
            Dictionary<string, object> dic_ = new Dictionary<string, object>();
            dic_.Add("IMACDINH", 1);
            var kyhop = _quochoi_khoa.GetAll(dic_);
            if (kyhop != null)
            {
                ID = Convert.ToInt32(kyhop.FirstOrDefault()?.IKHOA);
            }
            else
            {
                ID = 0;
            }
            return ID;
        }
        public List<USER_ACTION> GetAll_UserAction(Dictionary<string, object> condition)
        {
            return _user_action.GetAll(condition);
        }
        public List<USER_ACTION> GetList_UserAction(String sql)
        {
            return _user_action.GetList(sql);
        }
        public TRACKING InsertTracking(TRACKING input)
        {
            return _tracking.AddNew(input);
        }
        public QUOCHOI_COQUAN GetByIDQuochoiCoquan(int iD)
        {
            return _quochoi_coquan.GetByID(iD);
        }
        public List<KN_KIENNGHI> HienThiDanhSachKienNghi(Dictionary<string, object> condition)
        {
            return _kiennghi.GetAll(condition);
        }

        //begin hàm check quyền mới//
        public Boolean IsAdmin_(TaikhoanAtion act)
        {
            return act.is_admin;
        }
        public Boolean IsRoot_(TaikhoanAtion act)
        {
            return act.is_root;
        }

        public Boolean ActionMulty_Redirect_(string action, TaikhoanAtion act)
        {
            if (!act.is_admin)
            {
                bool check = false;
                foreach (var a in action.Split(','))
                {
                    if (act.list_action.Contains("|" + a + "|"))
                    {
                        check = true;
                        break;
                    }
                }
                return check;
                //if (check == false)
                //{
                //    HttpContext.Current.Response.Redirect("/Home/Error/");
                //}
            }
            return true;
        }
        public Boolean ActionMulty_(string action, TaikhoanAtion act)
        {
            if (act.is_admin)
            {
                return true;
            }
            else
            {
                bool check = false;
                foreach (var a in action.Split(','))
                {
                    if (act.list_action.Contains("|" + a + "|"))
                    {
                        check = true;
                        break;
                    }
                }
                return check;
            }
        }
        public Boolean Action_(int iAction, TaikhoanAtion act)
        {
            if (act.is_admin)
            {
                return true;
            }
            else
            {
                if (act.list_action.Contains("|" + iAction + "|"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //end hàm check quyền mới
        //public void DeleteOld_Token()
        //{
        //    _token.DeleleOld_Token();
        //}

    }
}
