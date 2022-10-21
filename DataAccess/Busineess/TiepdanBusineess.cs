using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dao;
namespace DataAccess.Busineess
{
    public class TiepdanBusineess
    {
        Quochoi_CoquanRepository _quochoi_coquan = new Quochoi_CoquanRepository();
        TD_VuviecRepository _vuviec = new TD_VuviecRepository();
        Tiepdan_DinhkyRepository _tiepdan_dinhky = new Tiepdan_DinhkyRepository();
        KNTC_LoaidonRepository _loaidon = new KNTC_LoaidonRepository();
        Tiepdan_Dinhky_LoaivuviecRepository _tiepdan_dinhky_loaivuviec = new Tiepdan_Dinhky_LoaivuviecRepository();
        TrackingRepository _trachking = new TrackingRepository();
        Tiepdan_ThuongxuyenRepository _tiepdan_thuongxuyen = new Tiepdan_ThuongxuyenRepository();
        Tiepdan_Thuongxuyen_KetquaRepository _tiepdan_thuongxuyen_ketqua = new Tiepdan_Thuongxuyen_KetquaRepository();
        FileuploadRepository _file_upload = new FileuploadRepository();
        TD_VUVIEC_XULYRepository _tiepdan_vuviec_xuly = new TD_VUVIEC_XULYRepository();

        public KNTC_DON InsertDonKNTC(KNTC_DON input)
        {
            KNTC_DonRepository _don = new KNTC_DonRepository();
            return _don.AddNew(input);
        }
        public List<TD_VUVIEC> get_vuviec_kiemtrung(int id)
        {
            return _vuviec.GetList_Vuviec_Trung(id);
        }
        public List<TD_VUVIEC> get_vuviec_kiemtrungnhanh(int idiaphuong,string noidung,string nguoigui)
        {
            return _vuviec.GetList_Vuviec_TrungNhanh(idiaphuong, noidung, nguoigui);
        }
        public List<KNTC_DON> get_vuviec_kiemtrung_kntc(string ctennguoi = "", int idiaphuong = 0, string cnoidung = "")
        {
            return _vuviec.List_DonTrung_KiemTrungKNTC(ctennguoi, idiaphuong, cnoidung);
        }
        public List<TD_VUVIEC> get_list_dinhky()
        {
            return _vuviec.List_Dinhky();
        }
        public List<TD_VUVIEC> Get_TDVUVIEC_LISTAL()
        {
            return _vuviec.GetAll();
        }
        public List<TD_VUVIEC> GetBy_List_TDVuViec(Dictionary<string, object> condition)
        {
            List<TD_VUVIEC> resule = new List<TD_VUVIEC>();
            resule = _vuviec.GetAll<TD_VUVIEC>(condition);
            return resule;
        }
        public List<TD_VUVIEC> Get_search_dinhky(TD_VUVIEC vuviec, string dtungay, string ddenngay)
        {
            return _vuviec.GetList_SQL_searchdinhky(vuviec, dtungay, ddenngay); ;
        }
        public List<TD_VUVIEC> Get_search_dotxuat(TD_VUVIEC vuviec, string dtungay, string ddenngay)
        {
            return _vuviec.GetList_SQL_searchdotxuat(vuviec, dtungay, ddenngay); ;
        }
        public List<TD_VUVIEC> Get_search_dinhky2(TD_VUVIEC vuviec, string dtungay, string ddenngay)
        {
            return _vuviec.GetList_SQL_searchdinhky_search2(vuviec, dtungay, ddenngay); ;
        }


        public List<TD_VUVIEC> delete_vuviec(int id)
        {
            return _vuviec.Delete_Vuviec(id);
        }
        public List<TD_VUVIEC> Get_search_noidungvuviec(string cten)
        {
            return _vuviec.GetList_Vuviec_Trung_search(cten);
        }
        public QUOCHOI_COQUAN Get_Quochoi_Coquan(int id)
        {
            return _quochoi_coquan.GetByID(id);
        }
        public TIEPDAN_DINHKY Insert_TiepDanDinhKy(TIEPDAN_DINHKY input)
        {
            return _tiepdan_dinhky.AddNew(input);
        }
        public List<TIEPDAN_DINHKY> GetBy_List_TiepDanDinhKy(Dictionary<string, object> condition)
        {
            List<TIEPDAN_DINHKY> resule = new List<TIEPDAN_DINHKY>();
            resule = _tiepdan_dinhky.GetAll<TIEPDAN_DINHKY>(condition);
            return resule;
        }
        public List<TIEPDAN_DINHKY> Get_List_TiepDanDinhKy()
        {
            return _tiepdan_dinhky.GetAll();
        }
        public List<TIEPDAN_DINHKY> Get_search_tendinhky(string cten)
        {
            return _tiepdan_dinhky.GetList_SQLTimkiemten(cten);
        }

        public List<TIEPDAN_DINHKY> Check_trung(string dngay, string cdiadiem)
        {
            return _tiepdan_dinhky.GetList_KiemTraTrung(dngay, cdiadiem);
        }
        public TIEPDAN_DINHKY HienThiThongTinTiepDanDinhKy(int input)
        {
            return _tiepdan_dinhky.GetByID(input);
        }
        public Boolean UpdateThongTinTiepDanDinhKy(TIEPDAN_DINHKY input)
        {
            return _tiepdan_dinhky.Update(input);
        }
        public List<KNTC_LOAIDON> HienThiDanhSachLoaiDon()
        {
            return _loaidon.GetAll();
        }
        public List<KNTC_LOAIDON> HienThiDanhSachLoaiDon(Dictionary<string, object> _condition)
        {
            return _loaidon.GetAll(_condition);
        }
        public List<TIEPDAN_DINHKY_LOAIVUVIEC> HienThiDanhSachTiepDanDinhKyLoaiVuViec(Dictionary<string, object> _condition)
        {
            return _tiepdan_dinhky_loaivuviec.GetAll(_condition);
        }
        public TIEPDAN_DINHKY_LOAIVUVIEC HienThiThongTinTiepDanDinhKyLoaiVuViec(int id)
        {
            return _tiepdan_dinhky_loaivuviec.GetByID(id);
        }
        public Boolean UpdateTiepDanDinhKyLoaiVuViec(TIEPDAN_DINHKY_LOAIVUVIEC Input)
        {
            return _tiepdan_dinhky_loaivuviec.Update(Input);
        }
        public TIEPDAN_DINHKY_LOAIVUVIEC InsertTiepDanDinhKyLoaiVuViec(TIEPDAN_DINHKY_LOAIVUVIEC input)
        {
            return _tiepdan_dinhky_loaivuviec.AddNew(input);
        }
        public Boolean DeleteThongTinTiepDanDinhKyLoaiVuViec(TIEPDAN_DINHKY_LOAIVUVIEC Input)
        {
            return _tiepdan_dinhky_loaivuviec.Delete(Input);
        }
        public Boolean DeleteThongTinTiepDanDinhKy(TIEPDAN_DINHKY Input)
        {
            return _tiepdan_dinhky.Delete(Input);
        }
        public List<TRACKING> HienThiDanhSachTracKing(Dictionary<string, object> condition)
        {
            return _trachking.GetAll(condition);
        }
        public TRACKING HienThiThongTinTracKing(int iD)
        {
            return _trachking.GetByID(iD);
        }
        public Boolean DeleteTracKing(TRACKING input)
        {
            return _trachking.Delete(input);
        }
        public TIEPDAN_THUONGXUYEN Insert_TiepDanThuongXuyen(TIEPDAN_THUONGXUYEN input)
        {
            return _tiepdan_thuongxuyen.AddNew(input);
        }
        public TIEPDAN_THUONGXUYEN HienThiThongTinTiepDanThuongXuyen(int input)
        {
            return _tiepdan_thuongxuyen.GetByID(input);
        }
        public Boolean UpdateThongTinTiepDanThuongXuyen(TIEPDAN_THUONGXUYEN input)
        {
            return _tiepdan_thuongxuyen.Update(input);
        }
        public List<TIEPDAN_THUONGXUYEN> HienThiDanhSachTiepDanThuongXuyen()
        {
            return _tiepdan_thuongxuyen.GetAll();
        }
        public TIEPDAN_THUONGXUYEN_KETQUA Insert_TiepDanThuongXuyenLoaivuviec(TIEPDAN_THUONGXUYEN_KETQUA input)
        {
            return _tiepdan_thuongxuyen_ketqua.AddNew(input);
        }
        public FILE_UPLOAD Insert_File_TiepDanThuongXuyenLoaivuviec(FILE_UPLOAD input)
        {
            return _file_upload.AddNew(input);
        }
        public List<FILE_UPLOAD> HienThiDanhSachFile()
        {
            return _file_upload.GetAll();
        }
        public FILE_UPLOAD Get_ByIDFILE(int id)
        {
            return _file_upload.GetByID(id);
        }
        public Boolean DELETE_FILE(FILE_UPLOAD input)
        {
            return _file_upload.Delete(input);
        }
        public List<FILE_UPLOAD> LIST_FILE(Dictionary<string, object> condition)
        {
            List<FILE_UPLOAD> resule = new List<FILE_UPLOAD>();
            resule = _file_upload.GetAll<FILE_UPLOAD>(condition);
            return resule;
        }

        public void Tracking_Tiepdan(int iUser, int iDon, string content)
        {
            TRACKING track = new TRACKING();
            track.IDON = 0;
            track.IUSER = iUser;
            track.CACTION = content;
            track.DDATE = DateTime.Now;
            track.IKIENNGHI = 0; track.ITIEPDAN_DINHKY = iDon; track.ITIEPDAN_THUONGXUYEN = 0; track.ITONGHOP = 0;
            track.IVANBAN = 0;
            _trachking.AddNew(track);
        }
        public void Tracking_TD_dinhky(int iUser, int iDon, string content)
        {
            TRACKING track = new TRACKING();
            track.IDON = 0;
            track.IUSER = iUser;
            track.CACTION = content;
            track.DDATE = DateTime.Now;
            track.IKIENNGHI = 0; track.ITIEPDAN_DINHKY = iDon; track.ITIEPDAN_THUONGXUYEN = 0; track.ITONGHOP = 0;
            track.IVANBAN = 0;
            _trachking.AddNew(track);
        }
        public void Tracking_TD_thuongxuyen(int iUser, int iDon, string content)
        {
            TRACKING track = new TRACKING();
            track.IDON = 0;
            track.IUSER = iUser;
            track.CACTION = content;
            track.DDATE = DateTime.Now;
            track.IKIENNGHI = 0; track.ITIEPDAN_DINHKY = 0; track.ITIEPDAN_THUONGXUYEN = iDon; track.ITONGHOP = 0;
            track.IVANBAN = 0;
            _trachking.AddNew(track);
        }


        // Phần tiếp dân vụ việc
        public TD_VUVIEC Insert_TDVuviec(TD_VUVIEC Input)
        {
            return _vuviec.AddNew(Input);
        }
        public List<TD_VUVIEC> Get_TDVuviec()
        {
            return _vuviec.GetAll();
        }
        public List<TD_VUVIEC> Get_TDVuviec(Dictionary<string, object> condition)
        {
            return _vuviec.GetAll(condition);
        }
        //public int Get_LastID()
        //{
        //    return _vuviec.GetLastID_VuViec();
        //}
        public Boolean Update_TDVuviec(TD_VUVIEC Input)
        {
            return _vuviec.Update(Input);
        }
        public TD_VUVIEC Get_TDVuviecID(int id)
        {
            return _vuviec.GetByID(id);
        }

        public Boolean Delete_TDVuviec(TD_VUVIEC Input)
        {
            return _vuviec.Delete(Input);
        }

        // End phần tiếp dân vụ việc


        // phân tra lời vụ viêc 
        public TD_VUVIEC_XULY Insert_TDVuviecxuly(TD_VUVIEC_XULY Input)
        {
            return _tiepdan_vuviec_xuly.AddNew(Input);
        }
        public List<TD_VUVIEC_XULY> Get_list_TDVuviecxuly()
        {
            return _tiepdan_vuviec_xuly.GetAll();
        }
        public List<TD_VUVIEC_XULY> Get_TDVuviecxuly(Dictionary<string, object> condition)
        {
            return _tiepdan_vuviec_xuly.GetAll(condition);
        }

        public Boolean Update_TDVuviecxuly(TD_VUVIEC_XULY Input)
        {
            return _tiepdan_vuviec_xuly.Update(Input);
        }
        public TD_VUVIEC_XULY Get_TDVuviecxulyID(int id)
        {
            return _tiepdan_vuviec_xuly.GetByID(id);
        }

        public Boolean Delete_TDVuviec(TD_VUVIEC_XULY Input)
        {
            return _tiepdan_vuviec_xuly.Delete(Input);
        }
        // end phân tra lời vụ viêc 
        public Boolean Tracking_dinhky(int iUser, int tiepdan, string action)
        {
            try
            {
                TRACKING t = new TRACKING();
                t.IUSER = iUser;
                //t.IVANBAN = 0;
                t.ITIEPDAN_DINHKY = 0;
                t.IKIENNGHI = 0; t.ITONGHOP = 0; t.IDON = 0; t.ITIEPDAN_THUONGXUYEN = 0;
                t.DDATE = DateTime.Now;
                t.CACTION = action;
                //db.trackings.Add(t); db.SaveChanges();
                BaseBusineess _base = new BaseBusineess();
                _base.InsertTracking(t);
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}
