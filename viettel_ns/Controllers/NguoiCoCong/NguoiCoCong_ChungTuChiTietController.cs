﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;

namespace VIETTEL.Controllers.NguoiCoCong
{
    public class NguoiCoCong_ChungTuChiTietController : Microsoft.AspNetCore.Mvc.Controller
    {
        //
        // GET: /NguoiCoCong_ChungTuChiTiet/
        public string sViewPath = "~/Views/NguoiCoCong/ChungTuChiTiet/";
        [Authorize]
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return View(sViewPath + "NguoiCoCong_ChungTuChiTiet_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public Microsoft.AspNetCore.Mvc.ActionResult DetailSubmit(String iID_MaChungTu)
        {
            String TenBangChiTiet = "NCC_ChungTuChiTiet";

            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;            
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                Boolean okCoThayDoi = false;
                for (int j = 0; j < arrMaCot.Length; j++)
                {
                    if (arrThayDoi[j] == "1")
                    {
                        okCoThayDoi = true;
                        break;
                    }
                }
                if (okCoThayDoi)
                {
                    Bang bang = new Bang(TenBangChiTiet);
                    bang.GiaTriKhoa = arrMaHang[i];
                    bang.DuLieuMoi = false;
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    //Them tham so
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j]=="1")
                        {
                            String Truong = "@" + arrMaCot[j];
                            if (arrMaCot[j].StartsWith("b"))
                            {
                                //Nhap Kieu checkbox
                                if (arrGiaTri[j] == "1")
                                {
                                    bang.CmdParams.Parameters.AddWithValue(Truong, true);
                                }
                                else
                                {
                                    bang.CmdParams.Parameters.AddWithValue(Truong, false);
                                }
                            }
                            else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                            {
                                //Nhap Kieu so
                                if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                {
                                    bang.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                }
                            }
                            else
                            {
                                //Nhap kieu xau
                                bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                            }
                        }
                    }
                    bang.Save();
                }
            }
            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "NguoiCoCong_ChungTu", new { iID_MaChungTu = iID_MaChungTu });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "NguoiCoCong_ChungTu", new { iID_MaChungTu = iID_MaChungTu });
            }
            return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu });
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public Microsoft.AspNetCore.Mvc.ActionResult SearchSubmit(String ParentID, String iID_MaChungTu)
        {
            String sLNS = Request.Form["Search_sLNS"];
            String sL = Request.Form["Search_sL"];
            String sK = Request.Form["Search_sK"];
            String sM = Request.Form["Search_sM"];
            String sTM = Request.Form["Search_sTM"];
            String sTTM = Request.Form["Search_sTTM"];
            String sNG = Request.Form["Search_sNG"];
            String sTNG = Request.Form["Search_sTNG"];

            return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS, sL = sL, sK = sK, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG, sTNG = sTNG });
        }
    }
}
