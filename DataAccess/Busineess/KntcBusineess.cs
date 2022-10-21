using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dao;
using Entities.Models;
using Entities.Objects;
using Oracle.ManagedDataAccess.Client;
using Utilities;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;

namespace DataAccess.Busineess
{
    public class KntcBusineess : BaseRepository
    {
        Funtions func = new Funtions();
        Log log = new Log();
        //BaseBusineess _base_bussiness = new BaseBusineess();
        UsserRepository _user = new UsserRepository();
        KNTC_GiamsatRepository _giamsat = new KNTC_GiamsatRepository();
        KNTC_DonRepository _don = new KNTC_DonRepository();
        KNTC_DON_IMPORTRepository _donImport = new KNTC_DON_IMPORTRepository();
        DiaPhuongRepository _diaphuong = new DiaPhuongRepository();
        DantocRepository _dantoc = new DantocRepository();
        QuoctichRepository _quoctich = new QuoctichRepository();
        KNTC_LoaidonRepository _loaidon = new KNTC_LoaidonRepository();
        KNTC_NguondonRepository _nguondon = new KNTC_NguondonRepository();
        KN_NguondonRepository _nguonkiennghi = new KN_NguondonRepository();
        KNTC_NoidungdonRepository _noidung = new KNTC_NoidungdonRepository();
        KNTC_TinhchatRepository _tinhchat = new KNTC_TinhchatRepository();
        Quochoi_CoquanRepository _coquan = new Quochoi_CoquanRepository();
        LinhvucRepository _linhvuc = new LinhvucRepository();
        User_ChucvuRepository _chucvu = new User_ChucvuRepository();
        TrackingRepository _track = new TrackingRepository();
        FileuploadRepository _file = new FileuploadRepository();
        KNTC_VanbanRepository _vanban = new KNTC_VanbanRepository();
        User_ActionRepository _user_action = new User_ActionRepository();
        TrackingRepository _tracking = new TrackingRepository();
        KNTC_LuutheodoiRepository _luu = new KNTC_LuutheodoiRepository();
        TD_VuviecRepository _tiepdan = new TD_VuviecRepository();
        KNTC_LichsudonRepository _donlichsu = new KNTC_LichsudonRepository();
        //Dictionary<string, object> dictionary;
        public List<KNTC_DON> Get_List_DonTrung(KNTC_DON don)
        {
            return _don.List_DonTrung_ByID(don);
        }
        public KNTC_GIAMSAT Get_Giamsat(int id)
        {
            return _giamsat.GetByID(id);
        }
        public USERS Get_User(int id)
        {
            return _user.GetByID(id);
        }

        public List<KNTC_GIAMSAT> List_Giamsat(Dictionary<string, object> dictionary)
        {
            return _giamsat.GetAll(dictionary);
        }
        public KNTC_LUUTHEODOI Get_LuuTheoDoi(int id)
        {
            KNTC_LuutheodoiRepository _theodoi = new KNTC_LuutheodoiRepository();
            return _theodoi.GetByID(id);
        }
        public int LastID_Giamsat(int iDon)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("IDON", iDon);
            KNTC_GIAMSAT g = _giamsat.GetAll(dictionary).OrderByDescending(x => x.IGIAMSAT).FirstOrDefault();
            return (int)g.IGIAMSAT;
        }
        public KNTC_GIAMSAT Giamsat_insert(KNTC_GIAMSAT s)
        {
            return _giamsat.AddNew(s);
        }
        public Boolean Giamsat_update(KNTC_GIAMSAT s)
        {
            return _giamsat.Update(s);
        }
        public Boolean Giamsat_delete(KNTC_GIAMSAT s)
        {
            return _giamsat.Delete(s);
        }
        public USERS GetUser(int id)
        {
            return _user.GetByID(id);
        }
        public List<USERS> List_User()
        {
            return _user.GetAll();
        }
        public List<USERS> List_User(Dictionary<string, object> con)
        {
            return _user.GetAll(con);
        }
        public List<USERS> List_NguoiNhap(int iDonvi)
        {
            return _user.GetList_NguoiNhap(iDonvi);
        }
        public KNTC_VANBAN Vanban_insert(KNTC_VANBAN v)
        {
            return _vanban.AddNew(v);
        }
        public QUOCHOI_COQUAN GetDonVi(int id)
        {
            return _coquan.GetByID(id);
        }
        public List<QUOCHOI_COQUAN> List_DonVi(Dictionary<string, object> con)
        {
            return _coquan.GetAll(con);
        }
        public List<KNTC_DON> List_DonMoiCapNhat_search(KNTC_DON d, string tungay, string denngay)
        {
            return _don.DonMoiCapNhat(d, tungay, denngay);
        }
        public List<KNTC_DON> _List_All_Don()
        {
            return _don.GetList_Don();
        }
        public List<KNTC_DON> _List_All_Don_Trung(int DonID,int iDonTrung, int iDonVi)
        {
            return _don.List_Don_Trung(DonID, iDonTrung, iDonVi);
        }
        public List<KNTC_DON> ListAll_Don(Dictionary<string, object> con, int page, int pageSize)
        {
            return _don.DanhSachDon(con, page, pageSize);
        }
        public List<KNTC_DON> ListAll_Don(Dictionary<string, object> con)
        {
            return _don.GetList_Don(con);
        }
        public List<KNTC_DON> List_DonMoiCapNhat()
        {

            Dictionary<string, object> DON = new Dictionary<string, object>();
            DON.Add("ITINHTRANGXULY", 0);
            return _don.GetList_Don(DON);

        }
        public List<KNTC_DON> Get_IDDonvi_xulydon(int itinhtrang, int id_donvitiepnhan)
        {
            return _don.Get_IDDonvi_xulydon(itinhtrang, id_donvitiepnhan);
        }
        public List<KNTC_DON> Get_IDDonvi_Tiepnhan(int itinhtrang, int id_donvitiepnhan)
        {
            return _don.Get_IDDonvi_TiepNhanDon(itinhtrang, id_donvitiepnhan);
        }
        public KNTC_DON TiepNhan_Don(KNTC_DON Input)
        {
            return _don.AddNew(Input);
        }
        public KNTC_DON GetDON(int id)
        {
            return _don.GetByID(id);
        }
        public FILE_UPLOAD GET_FileBYID(int id)
        {
            return _file.GetByID(id);
        }
        public List<FILE_UPLOAD> List_File()
        {
            return _file.GetAll();
        }
        public List<FILE_UPLOAD> List_File(Dictionary<string, object> condition)
        {
            List<FILE_UPLOAD> resule = new List<FILE_UPLOAD>();
            resule = _file.GetAll<FILE_UPLOAD>(condition);
            return resule;
        }
        public Boolean Update_Don(KNTC_DON Input)
        {
            return _don.Update(Input);
        }
        public Boolean Delete_Don(int id_don)
        {
            KNTC_DON Input = GetDON(id_don);
            return _don.Delete(Input);
        }
        public Boolean Delete_File_upload(int id)
        {
            FILE_UPLOAD Input = GET_FileBYID(id);
            return _file.Delete(Input);
        }
        public FILE_UPLOAD Upload_file(FILE_UPLOAD input)
        {
            return _file.AddNew(input);
        }
        public int LastID_Vanban()
        {
            return _vanban.GetLastID_Vanban_KNTC();
        }

        public List<KNTC_DON> List_Don(Dictionary<string, object> condition)
        {
            return _don.GetList_Don(condition);
        }
        public List<KNTC_DON> List_Don()
        {
            return _don.GetList_Don();
        }
        public QUOCTICH Get_QuocTich(int id)
        {
            return _quoctich.GetByID(id);
        }
        public List<QUOCTICH> List_QuocTich()
        {
            return _quoctich.GetAll();
        }
        public DANTOC Get_DanToc(int id)
        {
            return _dantoc.GetByID(id);
        }
        public QUOCHOI_COQUAN GetByID_CoQuan(int id)
        {
            return _coquan.GetByID(id);
        }
        public List<DANTOC> List_DanToc()
        {
            return _dantoc.GetAll();
        }
        public LINHVUC Get_LinhVuc(int id)
        {
            return _linhvuc.GetByID(id);
        }
        public List<LINHVUC> List_LinhVuc()
        {
            return _linhvuc.GetAll();
        }
        public KNTC_TINHCHAT Get_TinhChat(int id)
        {
            return _tinhchat.GetByID(id);
        }
        public List<KNTC_TINHCHAT> List_TinhChat()
        {
            return _tinhchat.GetAll();
        }
        public KNTC_LOAIDON Get_LoaiDon(int id)
        {
            return _loaidon.GetByID(id);
        }
        public List<KNTC_LOAIDON> List_LoaiDon()
        {
            return _loaidon.GetAll();
        }
        public KNTC_NOIDUNGDON Get_NoiDungDon(int id)
        {
            return _noidung.GetByID(id);
        }
        public List<KNTC_NOIDUNGDON> List_NoiDungDon()
        {
            return _noidung.GetAll();
        }
        public KNTC_NGUONDON Get_NguonDon(int id)
        {
            return _nguondon.GetByID(id);
        }
        public List<KNTC_NGUONDON> List_NguonDon()
        {
            return _nguondon.GetAll();
        }
        public List<QUOCHOI_KHOA> GetAll_KhoaHop()
        {
            Quochoi_KhoaRepository _khoa = new Quochoi_KhoaRepository();
            Dictionary<string, object> _condition = new Dictionary<string, object>();
            _condition.Add("IHIENTHI", 1);
            return _khoa.GetAll(_condition);
        }
        public KN_NGUONDON Get_NguonKienNghi(int id)
        {
            return _nguonkiennghi.GetByID(id);
        }
        public List<KN_NGUONDON> List_NguonKienNghi()
        {
            return _nguonkiennghi.GetAll();
        }
        public QUOCHOI_KHOA Get_Khoa_QuocHoi(int id)
        {
            Quochoi_KhoaRepository kn = new Quochoi_KhoaRepository();
            return kn.GetByID(id);
        }
        public DIAPHUONG Get_DiaPhuong(int id)
        {
            return _diaphuong.GetByID(id);
        }
        public List<DIAPHUONG> List_DiaPhuong()
        {
            return _diaphuong.GetAll();
        }
        public List<DIAPHUONG> List_DiaPhuong(Dictionary<string, object> condition)
        {
            return _diaphuong.GetAll(condition);
        }
        public List<KNTC_GIAMSAT> List_GiamSat()
        {
            return _giamsat.GetAll();
        }
        public USER_CHUCVU Get_ChucVu(int id)
        {
            return _chucvu.GetByID(id);
        }
        public List<USER_CHUCVU> List_ChucVu()
        {
            return _chucvu.GetAll();
        }
        public List<USER_ACTION> List_UserAction()
        {
            return _user_action.GetAll();
        }
        public List<USER_ACTION> List_UserAction(Dictionary<string, object> condition)
        {
            return _user_action.GetAll(condition);
        }
        public List<KNTC_VANBAN> List_VanBan()
        {
            return _vanban.GetAll();
        }
        public KNTC_VANBAN GetVB_ByID(int ID)
        {
            return _vanban.GetByID(ID);
        }
        public List<TRACKING> List_TracKing()
        {
            return _tracking.GetAll();
        }

        public Boolean Tracking_KNTC(int iUser, int id, string content)
        {
            try
            {
                TrackingRepository _track = new TrackingRepository();
                TRACKING track = new TRACKING();
                track.IDON = id;
                track.IUSER = iUser;
                track.CACTION = content;
                track.DDATE = DateTime.Now;
                track.IKIENNGHI = 0;
                track.ITIEPDAN_DINHKY = 0; track.ITIEPDAN_THUONGXUYEN = 0; track.ITONGHOP = 0;
                track.IVANBAN = 0;
                _track.AddNew(track);
                return true;
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                return false;
            }
        }
        public List<KNTC_LUUTHEODOI> List_LuuTheoDoi()
        {
            return _luu.GetAll();
        }
        public List<KNTC_DON_LICHSU> List_LichSuDon()
        {
            return _donlichsu.GetAll();
        }
        public TD_VUVIEC Get_DonKNTCByIDTCD(int iD)
        {
            return _tiepdan.GetByIDDon(iD);
        }

        public KNTC_DON_LICHSU InsertLichSuDon(KNTC_DON_LICHSU valueInput)
        {
            return _donlichsu.AddNew(valueInput);
        }
        public List<DONTRACUU> TraCuuDon(String procedurename, string P_CKEY, int P_IDOANDONGNGUOI,
                                           string P_DTUNGAY, string P_DDENNGAY, int P_INGUONDON,
                                                     int P_ITINHTHANH, int P_IQUOCTICH, int P_IDANTOC, int P_ILOAIDON
                                        , int P_ILINHVUC, int P_IDONVITHULY, int P_INOIDUNG, int P_ITINHCHAT, int P_ITINHTRANGDON)
        {

            List<DONTRACUU> resule = new List<DONTRACUU>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (P_DTUNGAY == "")
            {
                lisparam.Add(new OracleParameter("P_DTUNGAY", P_DTUNGAY));
            }
            else
            {
                var tungay = new OracleParameter("P_DTUNGAY", OracleDbType.Date);
                tungay.Value = P_DTUNGAY;
                lisparam.Add(tungay);
            }
            if (P_DDENNGAY == "")
            {
                lisparam.Add(new OracleParameter("P_DDENNGAY", P_DDENNGAY));
            }
            else
            {
                var denngay = new OracleParameter("P_DDENNGAY", OracleDbType.Date);
                denngay.Value = P_DDENNGAY;
                lisparam.Add(denngay);
            }
            lisparam.Add(new OracleParameter("P_CKEY", P_CKEY));
            lisparam.Add(new OracleParameter("P_IDOANDONGNGUOI", P_IDOANDONGNGUOI));
            lisparam.Add(new OracleParameter("P_INGUONDON", P_INGUONDON));
            lisparam.Add(new OracleParameter("P_ITINHTHANH", P_ITINHTHANH));
            lisparam.Add(new OracleParameter("P_IQUOCTICH", P_IQUOCTICH));
            lisparam.Add(new OracleParameter("P_IDANTOC", P_IDANTOC));
            lisparam.Add(new OracleParameter("P_ILOAIDON", P_ILOAIDON));
            lisparam.Add(new OracleParameter("P_ILINHVUC", P_ILINHVUC));
            lisparam.Add(new OracleParameter("P_IDONVITHULY", P_IDONVITHULY));
            lisparam.Add(new OracleParameter("P_INOIDUNG", P_INOIDUNG));
            lisparam.Add(new OracleParameter("P_ITINHCHAT", P_ITINHCHAT));
            lisparam.Add(new OracleParameter("P_ITINHTRANGDON", P_ITINHTRANGDON));
            resule = GetListObjetReport<DONTRACUU>(procedurename, lisparam);
            return resule;

        }
        public List<KNTCDON> ListDon(String procedurename, string P_CKEY, int P_IDOANDONGNGUOI,
                                         string P_DTUNGAY, string P_DDENNGAY, int P_INGUONDON,
                                                   int P_ITINHTHANH, int P_IQUOCTICH, int P_IDANTOC, int P_ILOAIDON
                                      , int P_ILINHVUC, int P_INOIDUNG, int P_ITINHCHAT, int P_IDONVI, int P_ICHUYENXULY)
        {

            List<KNTCDON> resule = new List<KNTCDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (P_DTUNGAY == "")
            {
                lisparam.Add(new OracleParameter("P_DTUNGAY", P_DTUNGAY));
            }
            else
            {
                var tungay = new OracleParameter("P_DTUNGAY", OracleDbType.Date);
                tungay.Value = P_DTUNGAY;
                lisparam.Add(tungay);
            }
            if (P_DDENNGAY == "")
            {
                lisparam.Add(new OracleParameter("P_DDENNGAY", P_DDENNGAY));
            }
            else
            {
                var denngay = new OracleParameter("P_DDENNGAY", OracleDbType.Date);
                denngay.Value = P_DDENNGAY;
                lisparam.Add(denngay);
            }
            lisparam.Add(new OracleParameter("P_CKEY", P_CKEY));
            lisparam.Add(new OracleParameter("P_IDOANDONGNGUOI", P_IDOANDONGNGUOI));
            lisparam.Add(new OracleParameter("P_INGUONDON", P_INGUONDON));
            lisparam.Add(new OracleParameter("P_ITINHTHANH", P_ITINHTHANH));
            lisparam.Add(new OracleParameter("P_IQUOCTICH", P_IQUOCTICH));
            lisparam.Add(new OracleParameter("P_IDANTOC", P_IDANTOC));
            lisparam.Add(new OracleParameter("P_ILOAIDON", P_ILOAIDON));
            lisparam.Add(new OracleParameter("P_ILINHVUC", P_ILINHVUC));
            lisparam.Add(new OracleParameter("P_INOIDUNG", P_INOIDUNG));
            lisparam.Add(new OracleParameter("P_ITINHCHAT", P_ITINHCHAT));
            if (P_IDONVI != 0)
            {
                lisparam.Add(new OracleParameter("P_IDONVI", P_IDONVI));
            }
            if (P_ICHUYENXULY != 0)
            {
                lisparam.Add(new OracleParameter("P_ICHUYENXULY", P_ICHUYENXULY));
            }
            resule = GetListObjetReport<KNTCDON>(procedurename, lisparam);
            return resule;

        }

        public List<KNTCDON> GetListAllDon(string key = "", string tungay = "", string denngay = "", int doandongnguoi = 0, int nguondon = 0, int tinhthanh = 0, int quoctich = 0, int dantoc = 0, int loaidon = 0, int linhvuc = 0, int noidung = 0, int tinhchat = 0)
        {
            return ListDon("PKG_KHIEUNAI_TOCAO.PRO_LISTDON", key, doandongnguoi, tungay, denngay, nguondon, tinhthanh, quoctich, dantoc, loaidon, linhvuc, noidung, tinhchat, 0, 0);
        }

        public List<KNTCDON> GetListLichSuDon(string key = "", string tungay = "", string denngay = "", int donvi = 0, int ichuyen = 0, int doandongnguoi = 0, int nguondon = 0, int tinhthanh = 0, int quoctich = 0, int dantoc = 0, int loaidon = 0, int linhvuc = 0, int noidung = 0, int tinhchat = 0)
        {
            return ListDon("PKG_KHIEUNAI_TOCAO.PRO_LISTLICHSUDON", key, doandongnguoi, tungay, denngay, nguondon, tinhthanh, quoctich, dantoc, loaidon, linhvuc, noidung, tinhchat, donvi, ichuyen);
        }

        public List<KNTCDON> ListDonDaXuLy(String procedurename, string P_CKEY, int P_IDOANDONGNGUOI,
                                         string P_DTUNGAY, string P_DDENNGAY, int P_INGUONDON,
                                                   int P_ITINHTHANH, int P_IQUOCTICH, int P_IDANTOC, int P_ILOAIDON
                                      , int P_ILINHVUC, int P_INOIDUNG, int P_ITINHCHAT, int P_IDONVI, int P_ITRANGTHAI)
        {

            List<KNTCDON> resule = new List<KNTCDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (P_DTUNGAY == "")
            {
                lisparam.Add(new OracleParameter("P_DTUNGAY", P_DTUNGAY));
            }
            else
            {
                var tungay = new OracleParameter("P_DTUNGAY", OracleDbType.Date);
                tungay.Value = P_DTUNGAY;
                lisparam.Add(tungay);
            }
            if (P_DDENNGAY == "")
            {
                lisparam.Add(new OracleParameter("P_DDENNGAY", P_DDENNGAY));
            }
            else
            {
                var denngay = new OracleParameter("P_DDENNGAY", OracleDbType.Date);
                denngay.Value = P_DDENNGAY;
                lisparam.Add(denngay);
            }
            lisparam.Add(new OracleParameter("P_CKEY", P_CKEY));
            lisparam.Add(new OracleParameter("P_IDOANDONGNGUOI", P_IDOANDONGNGUOI));
            lisparam.Add(new OracleParameter("P_INGUONDON", P_INGUONDON));
            lisparam.Add(new OracleParameter("P_ITINHTHANH", P_ITINHTHANH));
            lisparam.Add(new OracleParameter("P_IQUOCTICH", P_IQUOCTICH));
            lisparam.Add(new OracleParameter("P_IDANTOC", P_IDANTOC));
            lisparam.Add(new OracleParameter("P_ILOAIDON", P_ILOAIDON));
            lisparam.Add(new OracleParameter("P_ILINHVUC", P_ILINHVUC));
            lisparam.Add(new OracleParameter("P_INOIDUNG", P_INOIDUNG));
            lisparam.Add(new OracleParameter("P_ITINHCHAT", P_ITINHCHAT));
            if (P_IDONVI != 0)
            {
                lisparam.Add(new OracleParameter("P_IDONVIXULY", P_IDONVI));
            }
            if (P_ITRANGTHAI != 0)
            {
                lisparam.Add(new OracleParameter("P_ITRANGTHAI", P_ITRANGTHAI));
            }
            resule = GetListObjetReport<KNTCDON>(procedurename, lisparam);
            return resule;

        }

        public List<KNTCDON> GetListDonDaXuLy(string key = "", string tungay = "", string denngay = "", int donvi = 0, int itrangthai = 0, int doandongnguoi = 0, int nguondon = 0, int tinhthanh = 0, int quoctich = 0, int dantoc = 0, int loaidon = 0, int linhvuc = 0, int noidung = 0, int tinhchat = 0)
        {
            return ListDonDaXuLy("PKG_KHIEUNAI_TOCAO.PRO_LISTDONDAXULY", key, doandongnguoi, tungay, denngay, nguondon, tinhthanh, quoctich, dantoc, loaidon, linhvuc, noidung, tinhchat, donvi, itrangthai);
        }

        public List<KNTCDON_MOICAPNHAT> ListDonMoiCapNhat(Dictionary<string, object> condition)
        {
            List<KNTCDON_MOICAPNHAT> resule = new List<KNTCDON_MOICAPNHAT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<KNTCDON_MOICAPNHAT>("PKG_KHIEUNAI_TOCAO.PRO_DONMOICAPNHAT", lisparam);
            return resule;
        }
        public List<KNTCDON_MOICAPNHAT> ListDonChoXuLy(Dictionary<string, object> condition)
        {
            List<KNTCDON_MOICAPNHAT> resule = new List<KNTCDON_MOICAPNHAT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<KNTCDON_MOICAPNHAT>("PRC_KNTC_DONCHOXULY", lisparam);
            return resule;
        }
        public List<KNTCDON_MOICAPNHAT> ListDonDaPhanLoai(Dictionary<string, object> condition)
        {
            List<KNTCDON_MOICAPNHAT> resule = new List<KNTCDON_MOICAPNHAT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<KNTCDON_MOICAPNHAT>("PRC_KNTC_DONDAPHANLOAI", lisparam);
            return resule;
        }
        public List<KNTCDON_MOICAPNHAT> ListDonDaNhanXuLy(Dictionary<string, object> condition)
        {
            List<KNTCDON_MOICAPNHAT> resule = new List<KNTCDON_MOICAPNHAT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<KNTCDON_MOICAPNHAT>("PKG_KHIEUNAI_TOCAO.PRO_LISTDONDANHANXULY", lisparam);
            return resule;
        }
        public List<DONTRACUU> ListDonTrung(Dictionary<string, object> condition)
        {
            List<DONTRACUU> resule = new List<DONTRACUU>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<DONTRACUU>("PKG_KHIEUNAI_TOCAO.PRO_LISTDONTRUNG", lisparam);
            return resule;
        }
        public List<DONTRACUU> ListDonTraCuu(Dictionary<string, object> condition)
        {
            List<DONTRACUU> resule = new List<DONTRACUU>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<DONTRACUU>("PKG_KHIEUNAI_TOCAO.PRO_TRACUUDON", lisparam);
            return resule;
        }
        public List<KNTCDON> ListDonDaLuanChuyen(Dictionary<string, object> condition)
        {
            List<KNTCDON> resule = new List<KNTCDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<KNTCDON>("PKG_KHIEUNAI_TOCAO.PRO_LISTDALUANCHUYEN", lisparam);
            return resule;
        }
        public List<KNTCDON> ListDonDaChuyen(Dictionary<string, object> condition)
        {
            List<KNTCDON> resule = new List<KNTCDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<KNTCDON>("PRC_KNTC_LISTDACHUYENXULY", lisparam);
            return resule;
        }
        public List<KNTCDON_MOICAPNHAT> ListDonLuuKhongXuly(Dictionary<string, object> condition)
        {
            List<KNTCDON_MOICAPNHAT> resule = new List<KNTCDON_MOICAPNHAT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<KNTCDON_MOICAPNHAT>("PKG_KHIEUNAI_TOCAO.PRO_DONLUUKHONGXULY", lisparam);
            return resule;
        }
        public List<KNTCDON> ListDonDaChuyenCoTraLoi(Dictionary<string, object> condition)
        {
            List<KNTCDON> resule = new List<KNTCDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<KNTCDON>("PRC_KNTC_LISTDACHUYENXULYCOTRALOI", lisparam);
            return resule;
        }
        public List<KNTCDON_MOICAPNHAT> ListDonDaXuLyGiaiQuyet(Dictionary<string, object> condition)
        {
            List<KNTCDON_MOICAPNHAT> result;
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            result = GetListObjetReport<KNTCDON_MOICAPNHAT>("PRC_KNTC_LISTDONDAXULYGIAIQUYET", lisparam);
            return result;
        }
        public List<DONTRACUU> ListDonTamXoa(Dictionary<string, object> condition)
        {
            List<DONTRACUU> resule = new List<DONTRACUU>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (condition != null && condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var prop = key.Trim().ToUpper();
                    if (prop.Contains("LOWER"))
                    {
                        prop = prop.Replace("LOWER", "").Replace("(", "").Replace(")", "");
                    }
                    if (prop.Contains("UPPER"))
                    {
                        prop = prop.Replace("UPPER", "").Replace("(", "").Replace(")", "");

                    }
                    lisparam.Add(new OracleParameter(prop, value));
                }
            }
            resule = GetListObjetReport<DONTRACUU>("PKG_KHIEUNAI_TOCAO.PRO_LISTDONTAMXOA", lisparam);
            return resule;
        }

        public Document GetDonKntcTemplate()
        {
            var document = new Document();
            string serverPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string templateImportDonPath = serverPath + AppConfig.path_template_import_don_kntc;

            if (File.Exists(templateImportDonPath))
            {
                document.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                document.FileName = Path.GetFileName(AppConfig.path_template_import_don_kntc);
                document.Data = func.FileToByteArray(templateImportDonPath);
            }

            return document;
        }
        
        public KNTC_DON_IMPORT AddKntcDonImport(KNTC_DON_IMPORT item)
        {
            return _donImport.AddNew(item);
        }
        public bool UpdateKntcDonImport(KNTC_DON_IMPORT item)
        {
            return _donImport.Update(item);
        }
        public KNTC_DON_IMPORT GetKntcDonImport(int id)
        {
            return _donImport.GetByID(id);
        }

        public bool DeleteKntcDonImport(KNTC_DON_IMPORT item)
        {
            return _donImport.Delete(item);
        }

        public List<KNTC_DON_IMPORT> GetAll_Import(Dictionary<string, object> dic = null)
        {
            return _donImport.GetAll(dic);
        }

        public KNTC_DON InsertKntcDon(KNTC_DON item)
        {
            return _don.AddNew(item);
        }

        public bool InsertListKntcDon(List<KNTC_DON> list)
        {
            return _don.AddList(list);
        }
        public List<KNTC_DON> GetByIdImport(int id_import)
        {
            return _don.GetByIdImport(id_import);
        }

        public List<PRC_KNTC_IMPORT_LISTDON> PRC_KNTC_IMPORT_LISTDON(int id)
        {//LIST KNTC BY ID_IMPORT
            List<PRC_KNTC_IMPORT_LISTDON> result = new List<PRC_KNTC_IMPORT_LISTDON>();
            List<OracleParameter> lisparam = new List<OracleParameter>();

            lisparam.Add(new OracleParameter("p_ID_IMPORT", id));
            try
            {
                result = GetListObjetReport<PRC_KNTC_IMPORT_LISTDON>("PRC_KNTC_IMPORT_LISTDON", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        
        public List<KNTC_REPORT_CHUATRALOI> getReportBaoCaoMoiTongHop_ChuaTraLoi(string procedureName, DateTime? dtungay, DateTime? ddenngay, int iYear, int iMonth, int iDoiTuong, int iLoaiDon, int iUser)
        {

            List<KNTC_REPORT_CHUATRALOI> result = new List<KNTC_REPORT_CHUATRALOI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("P_TUNGAY", dtungay));
            lisparam.Add(new OracleParameter("P_DENNGAY", ddenngay));
            lisparam.Add(new OracleParameter("P_IYEAR", iYear));
            lisparam.Add(new OracleParameter("P_IMONTH", iMonth));
            lisparam.Add(new OracleParameter("P_IDOITUONG", iDoiTuong));
            lisparam.Add(new OracleParameter("P_ILOAIDON", iLoaiDon));
            lisparam.Add(new OracleParameter("P_IUSER", iUser));
            result = GetListObjetReport<KNTC_REPORT_CHUATRALOI>(procedureName, lisparam);
            return result;

        }
        public List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> getReportBaoCaoMoiTongHop_ChuaTraLoi3D(string procedureName, DateTime? dtungay, DateTime? ddenngay, int iYear, int iMonth, int iDoiTuong, int iLoaiDon, int iUser)
        {

            List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D> result = new List<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("P_TUNGAY", dtungay));
            lisparam.Add(new OracleParameter("P_DENNGAY", ddenngay));
            lisparam.Add(new OracleParameter("P_IYEAR", iYear));
            lisparam.Add(new OracleParameter("P_IMONTH", iMonth));
            lisparam.Add(new OracleParameter("P_IDOITUONG", iDoiTuong));
            lisparam.Add(new OracleParameter("P_ILOAIDON", iLoaiDon));
            lisparam.Add(new OracleParameter("P_IUSER", iUser));
            result = GetListObjetReport<KNTC_REPORT_CHUATRALOI_THEOLINHVUC_3D>(procedureName, lisparam);
            return result;

        }
    }
}
