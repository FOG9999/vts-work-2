using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using DataAccess.Busineess;
using Utilities;
using System.Text.RegularExpressions;
using Entities.Objects;

namespace KienNghi.App_Code
{
    public class Base
    {
        Funtions func = new Funtions();
        ThietlapBusineess tl = new ThietlapBusineess();
        public int pageSize =20;
        public int ID_Ban_DanNguyen = 4;
        public int ID_Coquan_doandaibieu = 20;
        public int IDDonVi_User(USERS user)
        {
            return (int)user.IDONVI;
        }
        public string Get_TenDonVi(int iUser)
        {
            USERS u = tl.GetBy_TaikhoanID(iUser);
            int iDonVi = IDDonVi_User(u);
            return tl.GetBy_Quochoi_CoquanID(iDonVi).CTEN;
        }
        public string Set_Url_keycookie()
        {
            string cookies = Guid.NewGuid().ToString().Substring(0, 4);
            func.SetCookies("url_key", cookies);
            return cookies;
        }
        public Boolean IsAdmin(int iUser)
        {

            if (IS_Root(iUser)) { return true; }
            var user = tl.GetBy_TaikhoanID(iUser);
            if (user != null && Convert.ToInt32(user.ITYPE) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean IS_Root(int iUser)
        {
            var user = tl.GetBy_TaikhoanID(iUser);
            if (user != null && Convert.ToInt32(user.ITYPE) == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Action(int iAction, int iUser)
        {
            var uat = tl.GetBy_List_User_Action();
            if (IsAdmin(iUser))
            {
                return true;
            }
            else
            {
                List<USER_ACTION> uaction = uat.Where(v => v.IUSER == iUser && v.IACTION == iAction).ToList();
                if (uaction.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Boolean IS_BanDanNguyen(int iUser)
        {
            USERS u = tl.GetBy_TaikhoanID(iUser);
            if (IsAdmin(iUser))
            {
                return true;
            }
            else
            {
                if (IDDonVi_User(u) == ID_Ban_DanNguyen)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string PhanTrang(int total, int post_page, int page, string link)
        {

            string phantrang = "<div class='pagination pagination-medium'>";
            int count_page = (int)Math.Ceiling((double)total / post_page);
            int sodongtrenpage = 0;
            if (total < post_page)
            {
                count_page = 1;
                sodongtrenpage = total;
            }
           
            phantrang += "Trang " + page + " của " + count_page + " |";
            //if (count_page == 1) { return ""; }
            string firtpage = "<a href='" + link + "&page=1'>1</a> |";
            string lastpage = "<a href='" + link + "&page=" + count_page + "&post_per_page=" + post_page+ "'>" + count_page + "</a> |<a href='" + link + "&page=" + count_page + "'>Trang cuối</a> ";
            if (page > 3 && count_page > 5)
            {
                phantrang += firtpage;
                if (page > 4)
                {
                    phantrang += "<a href='javascript:void()'>...</a> |";
                }
            }
            for (int i = 1; i < count_page; i++)
            {
                if (i == page)
                {
                    phantrang += "<a class='active' href='#'>" + page + "</a> |";
                }
                else
                {
                    if (count_page > 5)
                    {

                        if (i - 3 < page && page < i + 3)
                        {
                            phantrang += "<a href='" + link + "&page=" + i + "&post_per_page=" + post_page + "'>" + i + "</a> |";
                        }


                    }
                    else
                    {
                        phantrang += "<a href='" + link + "&page=" + i + "&post_per_page=" + post_page + "'>" + i + "</a> |";
                    }
                }

            }
            if (count_page > 5 && page < count_page - 2)
            {
                if (page < count_page - 3)
                {
                    phantrang += "<a href='javascript:void()'>...</a> |";
                }

            }
            phantrang += lastpage;
            phantrang += "</div>";
            return phantrang;
        }

        public string PhanTrang_Ajax(int total, int post_page, int page, string link, string searchFormId, string containerId)
        {

            string phantrang = "<div class='pagination pagination-medium'>";
            int count_page = (int)Math.Ceiling((double)total / post_page);
            int sodongtrenpage = 0;
            if (total < post_page)
            {
                count_page = 1;
                sodongtrenpage = total;
            }

            phantrang += "Trang " + page + " của " + count_page + " |";
            //if (count_page == 1) { return ""; }
            string firtpage = "<a onclick=\"ChangePage(1," + post_page + ",'" + link + "','" + searchFormId + "','" + containerId + "')\">1</a> |";
            string lastpage = "<a onclick=\"ChangePage(" + count_page + "," + post_page + ",'" + link + "','" + searchFormId + "','" + containerId + "')\">" + count_page +
                            "</a> |<a onclick=\"ChangePage(" + count_page + "," + post_page + ",'" + link + "','" + searchFormId + "','" + containerId + "')\">Trang cuối</a> ";
            if (page > 3 && count_page > 5)
            {
                phantrang += firtpage;
                if (page > 4)
                {
                    phantrang += "<a href='javascript:void()'>...</a> |";
                }
            }
            for (int i = 1; i < count_page; i++)
            {
                if (i == page)
                {
                    phantrang += "<a class='active' href='#'>" + page + "</a> |";
                }
                else
                {
                    if (count_page > 5)
                    {

                        if (i - 3 < page && page < i + 3)
                        {
                            phantrang += "<a onclick=\"ChangePage(" + i + "," + post_page + ",'" + link + "','" + searchFormId + "','" + containerId + "')\">" + i + "</a> |";
                        }
                    }
                    else
                    {
                        phantrang += "<a onclick=\"ChangePage(" + i + "," + post_page + ",'" + link + "','" + searchFormId + "','" + containerId + "')\">" + i + "</a> |";
                    }
                }

            }
            if (count_page > 5 && page < count_page - 2)
            {
                if (page < count_page - 3)
                {
                    phantrang += "<a href='javascript:void()'>...</a> |";
                }

            }
            phantrang += lastpage;
            phantrang += "</div>";
            return phantrang;
        }

        public Boolean Check_Sercurity_Pass(string password)
        {
            bool check = true;
            if (password.Length < 8)
            {
                check = false;
            }
            else
            {
                if (!Regex.IsMatch(password, @"[\d]", RegexOptions.ECMAScript)
                    || !Regex.IsMatch(password, @"[a-z]", RegexOptions.ECMAScript)
                    || !Regex.IsMatch(password, @"[A-Z]", RegexOptions.ECMAScript)
                    || !Regex.IsMatch(password, @"[~`!@#$%\^\&\*\(\)\-_\+=\[\{\]\}\|\\;:'\""<\,>\.\?\/£]", RegexOptions.ECMAScript))
                {
                    check = false;
                }
            }
            return check;
        }
        public UserInfor GetUserInfor()
        {
            UserInfor info = new UserInfor();
            if (System.Web.HttpContext.Current.Session["userInfo"] != null)
            {
                info = (UserInfor)System.Web.HttpContext.Current.Session["userInfo"];
            }
            return info;
        }
    }
}