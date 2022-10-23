﻿using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;
namespace VIETTEL.Controllers.QLDA
{
    public class QLDA_QuyetToanHoanThanhController : Microsoft.AspNetCore.Mvc.Controller
    {
        //
        // GET: /QLDA_QuyetToanHoanThanh/
        public string sViewPath = "~/Views/QLDA/QuyetToanHoanThanh/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_QuyetToanHoanThanh", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "QLDA_QuyetToanHoanThanh_Dot_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
          
        }
        [Authorize]
        public Microsoft.AspNetCore.Mvc.ActionResult Delete(String iID_MaQuyetToanHoanThanh_SoPhieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_QuyetToanHoanThanh_SoPhieu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            SqlCommand cmd = new SqlCommand("UPDATE QLDA_QuyetToanHoanThanh SET iTrangThai = 0 WHERE iID_MaQuyetToanHoanThanh_SoPhieu=@iID_MaQuyetToanHoanThanh_SoPhieu");
            cmd.Parameters.AddWithValue("@iID_MaQuyetToanHoanThanh_SoPhieu", iID_MaQuyetToanHoanThanh_SoPhieu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            Bang bang = new Bang("QLDA_QuyetToanHoanThanh_SoPhieu");
            bang.GiaTriKhoa = iID_MaQuyetToanHoanThanh_SoPhieu;
            bang.Delete();
            return RedirectToAction("Index", "QLDA_QuyetToanHoanThanh");
        }
        [Authorize]
        public Microsoft.AspNetCore.Mvc.ActionResult Search()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_QuyetToanHoanThanh", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "QLDA_QuyetToanHoanThanh_Search.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public Microsoft.AspNetCore.Mvc.ActionResult SearchSubmit(String ParentID)
        {
            String iID_MaDanhMucDuAn = Request.Form[ParentID + "_iID_MaDanhMucDuAn_Search"];
            String dTuNgay = Request.Form[ParentID + "_vidTuNgay"];
            String dDenNgay = Request.Form[ParentID + "_vidDenNgay"];

            return RedirectToAction("Search", "QLDA_QuyetToanHoanThanh", new { iID_MaDanhMucDuAn = iID_MaDanhMucDuAn, dTuNgay = dTuNgay, dDenNgay = dDenNgay });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public Microsoft.AspNetCore.Mvc.ActionResult AddNewSubmit(String ParentID)
        {
            String ThemMoi = Request.Form[ParentID + "_iThemMoi"];
            if (ThemMoi == "on")
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                String dNgayDotNganSach = Request.Form[ParentID + "_vidNgayQuyetToan"];
                DateTime d = Convert.ToDateTime(CommonFunction.LayNgayTuXau(dNgayDotNganSach));
                Int32 iSoDot = 1;
                if (QLDA_QuyetToanHoanThanhModels.Get_Max_Dot(NamLamViec) != "")
                {
                    iSoDot = Convert.ToInt32(QLDA_QuyetToanHoanThanhModels.Get_Max_Dot(NamLamViec)) + 1;
                };

                Bang bang = new Bang("QLDA_QuyetToanHoanThanh_SoPhieu");
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
                bang.CmdParams.Parameters.AddWithValue("@iSoQuyetToan", iSoDot);
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
                if (dNgayDotNganSach == null || dNgayDotNganSach == "")
                {
                    arrLoi.Add("err_dNgayQuyetToan", "Trùng đợt quyết toán");
                }
                if (arrLoi.Count == 0)
                {
                    bang.Save();
                }
                else
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    ViewData["bThemMoi"] = true;
                    return View(sViewPath + "QLDA_QuyetToanHoanThanh_Dot_Index.aspx");
                }
            }
            return RedirectToAction("Index", "QLDA_QuyetToanHoanThanh");
        }
        [Authorize]
        public Microsoft.AspNetCore.Mvc.ActionResult Detail()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_QuyetToanHoanThanh", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "QLDA_QuyetToanHoanThanh_Index.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public Microsoft.AspNetCore.Mvc.ActionResult DetailSubmit(String iID_MaQuyetToanHoanThanh_SoPhieu)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];
            String TenBangChiTiet = "QLDA_QuyetToanHoanThanh";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaDanhMucDuAnChiTiet;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length-1; i++)
            {
                iID_MaDanhMucDuAnChiTiet = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaDanhMucDuAnChiTiet != "")
                    {
                        //Dữ liệu đã có
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = iID_MaDanhMucDuAnChiTiet;
                        bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.Save();
                    }
                }
                else
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
                        iID_MaDanhMucDuAnChiTiet = arrMaHang[i];
                        if (iID_MaDanhMucDuAnChiTiet == "")
                        {
                            //Du Lieu Moi
                            bang.DuLieuMoi = true;
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaQuyetToanHoanThanh_SoPhieu", iID_MaQuyetToanHoanThanh_SoPhieu);
                            bang.CmdParams.Parameters.AddWithValue("@dNgayLap", QLDA_QuyetToanHoanThanhModels.Get_Row_QuyetToanHoanThanh_SoPhieu(iID_MaQuyetToanHoanThanh_SoPhieu).Rows[0]["dNgayQuyetToan"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaDanhMucDuAnChiTiet;
                            bang.DuLieuMoi = false;
                        }

                        if (arrGiaTri[0] != null && arrGiaTri[0] != "")
                        {
                            String[] arrGiaTriDuAn = arrGiaTri[0].Split('-');
                            //Điền thông tin các trường cho dự án                        
                            bang.CmdParams.Parameters.AddWithValue("@sDeAn", arrGiaTriDuAn[0]);
                            bang.CmdParams.Parameters.AddWithValue("@sDuAn", arrGiaTriDuAn[1]);
                            bang.CmdParams.Parameters.AddWithValue("@sDuAnThanhPhan", arrGiaTriDuAn[2]);
                            bang.CmdParams.Parameters.AddWithValue("@sCongTrinh", arrGiaTriDuAn[3]);
                            bang.CmdParams.Parameters.AddWithValue("@sHangMucCongTrinh", arrGiaTriDuAn[4]);
                            bang.CmdParams.Parameters.AddWithValue("@sHangMucChiTiet", arrGiaTriDuAn[5]);
                            bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa_DuAn", arrGiaTriDuAn[0] + "-" + arrGiaTriDuAn[1] + "-" + arrGiaTriDuAn[2] + "-" + arrGiaTriDuAn[3] + "-" + arrGiaTriDuAn[4] + "-" + arrGiaTriDuAn[5]);
                        }

                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                        //Them tham so
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
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
            }
            //Sự kiện bấn vào nút trình duyệt
            //string idAction = Request.Form["idAction"];
            //if (idAction == "1")
            //{
            //    return RedirectToAction("TuChoi", "KTCT_TienMat_ChungTu", new { iID_MaDanhMucDuAn = iID_MaDanhMucDuAn });
            //}
            //else if (idAction == "2")
            //{
            //    return RedirectToAction("TrinhDuyet", "KTCT_TienMat_ChungTu", new { iID_MaDanhMucDuAn = iID_MaDanhMucDuAn });
            //}
            return RedirectToAction("Detail", new { iID_MaQuyetToanHoanThanh_SoPhieu = iID_MaQuyetToanHoanThanh_SoPhieu});
        }
        [Authorize]
        public JsonResult get_CapPhat(String iID_MaDanhMucDuAn, String iID_MaMucLucNganSach, String iID_MaQuyetToanHoanThanh_SoPhieu)
        {
            String SQL = String.Format(@"SELECT dNgayQuyetToan FROM QLDA_QuyetToanHoanThanh_SoPhieu WHERE iID_MaQuyetToanHoanThanh_SoPhieu=@iID_MaQuyetToanHoanThanh_SoPhieu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaQuyetToanHoanThanh_SoPhieu", iID_MaQuyetToanHoanThanh_SoPhieu);
            String dNgayQuyetToan = Connection.GetValueString(cmd, "01/01/2000");
            SQL = String.Format(@"SELECT SUM(rDeNghiPheDuyetThanhToan)
                                    FROM QLDA_CapPhat
                                    WHERE iTrangThai=1 AND iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn AND sXauNoiMa=(SELECT sXauNoiMa FROM NS_MucLucNganSach WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach) AND  iID_MaDotCapPhat IN (SELECT iID_MaDotCapPhat FROM QLDA_CapPhat_Dot WHERE dNgayLap<=@dNgay AND iTrangThai=1)");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
            cmd.Parameters.AddWithValue("@dNgay", dNgayQuyetToan);
            String CapPhat = Connection.GetValueString(cmd, "0");

            SQL = String.Format(@"SELECT SUM(rSoTienQuyetToan+rSoTienDieuChinh)
                                    FROM QLDA_QuyetToan
                                    WHERE  iTrangThai=1  AND iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn AND sXauNoiMa=(SELECT sXauNoiMa FROM NS_MucLucNganSach WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach) AND  iID_MaQuyetToan_SoPhieu IN (SELECT iID_MaQuyetToan_SoPhieu FROM QLDA_QuyetToanHoanThanh_SoPhieu WHERE dNgayQuyetToan<=@dNgay AND iTrangThai=1)
                                    ");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
            cmd.Parameters.AddWithValue("@dNgay", dNgayQuyetToan);
            String QuyetToan = Connection.GetValueString(cmd, "0");
            cmd.Dispose();
            Object item = new
            {
                CapPhat = CapPhat,
                QuyetToan=QuyetToan

            };
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }
    }

}
