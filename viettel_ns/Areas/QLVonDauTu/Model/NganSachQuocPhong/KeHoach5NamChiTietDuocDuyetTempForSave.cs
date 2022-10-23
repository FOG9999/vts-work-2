﻿using OfficeOpenXml.Drawing.Slicer.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;
using VIETTEL.Common;
using VIETTEL.Models;

namespace VIETTEL.Areas.QLVonDauTu.Model.NganSachQuocPhong
{
    public class KeHoach5NamChiTietDuocDuyetTempForSave: VDT_KHV_KeHoach5Nam_ChiTiet
    {
        IQLVonDauTuService _iQLVonDauTuService = QLVonDauTuService.Default;
        IDanhMucService _danhMucService = DanhMucService.Default;

        public string sTenLoaiCongTrinh
        {
            get => getTenLoaiCongTrinh(iID_LoaiCongTrinhID);
        }
        
        public string sTenDonViQL
        {
            get => getTenDonVi(iID_DonViQuanLyID);
        }
        
        public string sTenNganSach
        {
            get => getTenNganSach(iID_NguonVonID);
        }

        private string getTenDonVi(Guid? iID_DonViQuanLyID)
        {
            try
            {
                if (iID_DonViQuanLyID.HasValue)
                {
                    VDT_DM_DonViThucHienDuAn dv = _danhMucService.GetDonViThucHienDuAnById(iID_DonViQuanLyID ?? Guid.Empty);
                    if (dv != null)
                    {
                        return dv.sTenDonVi;
                    }
                    return _danhMucService.GetNSDonViById(iID_DonViQuanLyID ?? Guid.Empty).sTen;
                }
                else return "";
            }
            catch(Exception ex)
            {
                return "";
            }            
            
        }

        private string getTenLoaiCongTrinh(Guid? iID_LoaiCongTrinhID)
        {
            if (iID_LoaiCongTrinhID.HasValue)
            {
                if (iID_LoaiCongTrinhID == Guid.Empty || iID_LoaiCongTrinhID == null)
                {
                    return "";
                }
                return _iQLVonDauTuService.GetDMLoaiCongTrinhById(iID_LoaiCongTrinhID ?? Guid.Empty).sTenLoaiCongTrinh;
            }
            else return "";            
        }

        private string getTenNganSach(int? iID_NguonVonID)
        {
            if (iID_NguonVonID.ToString() == "")
            {
                return "";
            }
            return _iQLVonDauTuService.GetNganSachByMa(iID_NguonVonID.ToString()).sTen;
        }

        public string sThoiGianThucHien
        {
            get; set;
        }

        public string sDiaDiem
        {
            get; set;
        }

        public string getDiaDiemByDuAnId()
        {
            try
            {
                VDT_DA_DuAn da = _iQLVonDauTuService.LayThongTinChiTietDuAn(iID_DuAnID);
                return da.sDiaDiem;
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public string getThoiGianThucHienByDuAnId()
        {
            try
            {
                VDT_DA_DuAn da = _iQLVonDauTuService.LayThongTinChiTietDuAn(iID_DuAnID);
                return da.sKhoiCong + " - "+ da.sKetThuc;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
