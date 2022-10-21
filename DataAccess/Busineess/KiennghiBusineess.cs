using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dao;
using Entities.Models;
using Entities.Objects;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using Utilities;

namespace DataAccess.Busineess
{
    public class KiennghiBusineess : BaseRepository
    {
        Quochoi_CoquanRepository _quochoi_coquan = new Quochoi_CoquanRepository();
        KN_KiennghiRepository _kiennghi = new KN_KiennghiRepository();
        DiaPhuongRepository _diaphuong = new DiaPhuongRepository();
        LinhvuccoquanRepository linhvuc_coquan = new LinhvuccoquanRepository();
        UsserRepository _user = new UsserRepository();
        User_ChucvuRepository _user_chucvu = new User_ChucvuRepository();
        KN_ChuongtrinhRepository _chuongtrinh = new KN_ChuongtrinhRepository();
        KN_Chuongtrinh_DaibieuRepository _chuongtrinh_daibieu = new KN_Chuongtrinh_DaibieuRepository();
        KN_Chuongtrinh_diaphuongRepository _chuongtrinh_diaphuong = new KN_Chuongtrinh_diaphuongRepository();
        KN_Chuongtrinh_ChitietRepository k = new KN_Chuongtrinh_ChitietRepository();
        KN_TukhoaRepository _kiennghi_tukhoa = new KN_TukhoaRepository();
        DaibieuReposity _daibieu = new DaibieuReposity();
        KN_ChuyenxulyRepository _chuyenxuly = new KN_ChuyenxulyRepository();
        Log log = new Log();
        public int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["post_per_page"].ToString());

        //public static int post_per_page = Convert.ToInt32(ConfigurationManager.AppSettings["post_per_page"].ToString());

        // phúc PRC_LIST_KIENNGHI_BY_TONGHOP_KIENNGHI//
        //public Boolean ExcuteQuery(string sql)
        //{
        //    return ExcuteSQL(sql);
        //}
        public List<PRC_LIST_USER_CAPNHAT> PRC_LIST_USER_CAPNHAT(int id)
        {//LIST KIẾN NGHỊ BY ID_TONGHOP, ID_TONGHOP_BDN
            List<PRC_LIST_USER_CAPNHAT> result = new List<PRC_LIST_USER_CAPNHAT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("p_ID_DONVI", id));
            try
            {
                result = GetListObjetReport<PRC_LIST_USER_CAPNHAT>("PKG_KIENNGHI_CUTRI.PRC_LIST_USER_CAPNHAT", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_KIENNGHI_DELETE> PRC_KIENNGHI_DELETE(int id)
        {//LIST KIẾN NGHỊ BY ID_TONGHOP, ID_TONGHOP_BDN
            List<PRC_KIENNGHI_DELETE> result = new List<PRC_KIENNGHI_DELETE>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("p_ID_USER", id));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_DELETE>("PKG_KIENNGHI_CUTRI.PRC_KIENNGHI_DELETE", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_KIENNGHI_TAMXOA> PRC_KIENNGHI_TAMXOA(KN_KIENNGHI kn, string listKyHop, string imakiennghi, 
            DateTime? dtungay, DateTime? ddenngay, string lstNguuonKN, string lstLinhVuc, string cnoidung, 
            int ITHAMQUYENDONVI_PARENT = 0, int page = 1, int page_size = 30, int itruockyhop = -1)
        {
            List<PRC_KIENNGHI_TAMXOA> result = new List<PRC_KIENNGHI_TAMXOA>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.IUSER == null) { kn.IUSER = 0; }
            //if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
            //if (kn.INGUONKIENNGHI == null) { kn.INGUONKIENNGHI = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = itruockyhop; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            //if (kn.IDIAPHUONG0 == null) { kn.IDIAPHUONG0 = 0; }
            //if (kn.IDIAPHUONG1 == null) { kn.IDIAPHUONG1 = 0; }
            if (cnoidung == null) { cnoidung = ""; }
            lisparam.Add(new OracleParameter("p_IUSER", (int)kn.IUSER));
            lisparam.Add(new OracleParameter("p_LSTLINHVUC", lstLinhVuc));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
            lisparam.Add(new OracleParameter("p_LSTNGUONKN", lstNguuonKN));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_LISTKYHOP", listKyHop));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", itruockyhop));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            //lisparam.Add(new OracleParameter("p_IDIAPHUONG0", (int)kn.IDIAPHUONG0));
            //lisparam.Add(new OracleParameter("p_IDIAPHUONG1", (int)kn.IDIAPHUONG1));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_MAKIENNGHI", imakiennghi));
            lisparam.Add(new OracleParameter("p_TUNGAY", dtungay));
            lisparam.Add(new OracleParameter("p_DENNGAY", ddenngay));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_TAMXOA>("PRC_KIENNGHI_TAMXOA", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }

        public List<PRC_KIENNGHI_IMPORT> PRC_KIENNGHI_IMPORT(int id)
        {//LIST KIẾN NGHỊ BY ID_TONGHOP, ID_TONGHOP_BDN
            List<PRC_KIENNGHI_IMPORT> result = new List<PRC_KIENNGHI_IMPORT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();

            lisparam.Add(new OracleParameter("p_ID_IMPORT", id));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_IMPORT>("PKG_KIENNGHI_CUTRI.PRC_KIENNGHI_IMPORT", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_TAPHOP_IMPORT> PRC_TAPHOP_IMPORT(int id)
        {//LIST KIẾN NGHỊ BY ID_TONGHOP, ID_TONGHOP_BDN
            List<PRC_TAPHOP_IMPORT> result = new List<PRC_TAPHOP_IMPORT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();

            lisparam.Add(new OracleParameter("p_ID_IMPORT", id));
            try
            {
                result = GetListObjetReport<PRC_TAPHOP_IMPORT>("PKG_KIENNGHI_CUTRI.PRC_TAPHOP_IMPORT", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_CHUONGTRINH_CHITIET> PRC_CHUONGTRINH_CHITIET(int id)
        {//LIST KIẾN NGHỊ BY ID_TONGHOP, ID_TONGHOP_BDN
            List<PRC_CHUONGTRINH_CHITIET> result = new List<PRC_CHUONGTRINH_CHITIET>();
            List<OracleParameter> lisparam = new List<OracleParameter>();

            lisparam.Add(new OracleParameter("p_ICHUONGTRINH", id));
            try
            {
                result = GetListObjetReport<PRC_CHUONGTRINH_CHITIET>("PKG_KIENNGHI_CUTRI.PRC_CHUONGTRINH_CHITIET", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_KIENNGHI_BYTONGHOP> PRC_LIST_KIENNGHI_BY_TONGHOP_KIENNGHI(int ITONGHOP, int ITONGHOP_BDN)
        {//LIST KIẾN NGHỊ BY ID_TONGHOP, ID_TONGHOP_BDN
            List<PRC_KIENNGHI_BYTONGHOP> result = new List<PRC_KIENNGHI_BYTONGHOP>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            
            lisparam.Add(new OracleParameter("p_ITONGHOP", ITONGHOP));
            lisparam.Add(new OracleParameter("p_ITONGHOP_BDN", ITONGHOP_BDN));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_BYTONGHOP>("PRC_LIST_KIENNGHI_BY_TONGHOP", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_KIENNGHI_TRUNG> PRC_KIENNGHI_TRUNG(string noidung_kiemtra)
        {//LIST KIẾN NGHỊ BY ID_TONGHOP, ID_TONGHOP_BDN
            List<PRC_KIENNGHI_TRUNG> result = new List<PRC_KIENNGHI_TRUNG>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (noidung_kiemtra.IndexOf('.')!=-1)
            {
                string[] noidung_kiemtra_split = noidung_kiemtra.Split('.');
                for(int i = 0; i < 5; i++)
                {
                    int j = i + 1;
                    string name_pram = "p_CNOIDUNG" + j;
                    string val = "";
                    if (i < noidung_kiemtra_split.Length)
                    {
                        val = noidung_kiemtra_split[i].Trim();
                    }
                    lisparam.Add(new OracleParameter(name_pram, val));
                }
            }else
            {
                //chỉ có 1 câu
                lisparam.Add(new OracleParameter("p_CNOIDUNG1", noidung_kiemtra));
                lisparam.Add(new OracleParameter("p_CNOIDUNG2", ""));
                lisparam.Add(new OracleParameter("p_CNOIDUNG3", ""));
                lisparam.Add(new OracleParameter("p_CNOIDUNG4", ""));
                lisparam.Add(new OracleParameter("p_CNOIDUNG5", ""));
            }
            //lisparam.Add(new OracleParameter("p_ITONGHOP", ITONGHOP));
            //lisparam.Add(new OracleParameter("p_ITONGHOP_BDN", ITONGHOP_BDN));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_TRUNG>("PKG_KIENNGHI_CUTRI.PRC_KIENNGHI_TRUNG", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_LIST_KN_TRALOI_DANHGIA> PRC_LIST_KN_TRALOI_DANHGIA(KN_KIENNGHI kn,int ITHAMQUYENDONVI_PARENT=0,int ketqua_danhgia=-1, int page=1, int page_size=30)
        {
            List<PRC_LIST_KN_TRALOI_DANHGIA> result = new List<PRC_LIST_KN_TRALOI_DANHGIA>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            //if (kn.IKIENNGHI == null) { kn.IKIENNGHI = 0; }
            if (kn.ITONGHOP == null) { kn.ITONGHOP = 0; }
            if (kn.ITONGHOP_BDN == null) { kn.ITONGHOP_BDN = 0; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_IDKIENNGHI", (int)kn.IKIENNGHI));
            lisparam.Add(new OracleParameter("p_ITONGHOP_BDN", (int)kn.ITONGHOP_BDN));
            lisparam.Add(new OracleParameter("p_ITONGHOP", (int)kn.ITONGHOP));
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_KETQUA_DANHGIA", ketqua_danhgia)); 
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            try
            {
                result = GetListObjetReport<PRC_LIST_KN_TRALOI_DANHGIA>("PKG_KIENNGHI_CUTRI.PRC_LIST_KN_TRALOI_DANHGIA", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_CHUONGTRINH_TXCT> PRC_CHUONGTRINH_TIEPXUC_CUTRI(KN_CHUONGTRINH kn, string listKyHop,int page,int page_size)
        {
            List<PRC_CHUONGTRINH_TXCT> result = new List<PRC_CHUONGTRINH_TXCT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.IDONVI == null) { kn.IDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_LISTKYHOP", listKyHop));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_IDOAN_LAPKEHOACH", (int)kn.IDONVI));
            lisparam.Add(new OracleParameter("p_USER", (int)kn.IUSER));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));

            result = GetListObjetReport<PRC_CHUONGTRINH_TXCT>("PKG_KIENNGHI_CUTRI.PRC_CHUONGTRINH_TIEPXUC_CUTRI", lisparam);
            return result;
        }
        public List<PRC_KIENNGHI_MOICAPNHAT> PRC_KIENNGHI_MOICAPNHAT_PHANTRANG(KN_KIENNGHI kn, string listKyHop, string imakiennghi, DateTime? dtungay, DateTime? ddenngay, string lstNguuonKN, string lstLinhVuc, string cnoidung,int ITHAMQUYENDONVI_PARENT = 0, int page=1,int page_size=30, int itruockyhop = -1)
        {
            List<PRC_KIENNGHI_MOICAPNHAT> result = new List<PRC_KIENNGHI_MOICAPNHAT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.IUSER == null) { kn.IUSER = 0; }
            //if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
            //if (kn.INGUONKIENNGHI == null) { kn.INGUONKIENNGHI = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = itruockyhop; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            //if (kn.IDIAPHUONG0 == null) { kn.IDIAPHUONG0 = 0; }
            //if (kn.IDIAPHUONG1 == null) { kn.IDIAPHUONG1 = 0; }
            if (cnoidung == null) { cnoidung = ""; }
            lisparam.Add(new OracleParameter("p_IUSER", (int)kn.IUSER));
            lisparam.Add(new OracleParameter("p_LSTLINHVUC", lstLinhVuc));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
            lisparam.Add(new OracleParameter("p_LSTNGUONKN", lstNguuonKN));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_LISTKYHOP", listKyHop));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", itruockyhop));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            //lisparam.Add(new OracleParameter("p_IDIAPHUONG0", (int)kn.IDIAPHUONG0));
            //lisparam.Add(new OracleParameter("p_IDIAPHUONG1", (int)kn.IDIAPHUONG1));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", cnoidung));
            lisparam.Add(new OracleParameter("p_MAKIENNGHI", imakiennghi));
            lisparam.Add(new OracleParameter("p_TUNGAY", dtungay));
            lisparam.Add(new OracleParameter("p_DENNGAY", ddenngay));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_MOICAPNHAT>("PRC_KIENNGHI_MOICAPNHAT_CHANGE", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_KIENNGHI_MOICAPNHAT> PRC_KIENNGHI_MOICAPNHAT(KN_KIENNGHI kn,int ITHAMQUYENDONVI_PARENT=0, int page = 1, int page_size = 30)
        {
            List<PRC_KIENNGHI_MOICAPNHAT> result = new List<PRC_KIENNGHI_MOICAPNHAT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.IUSER == null){ kn.IUSER = 0; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_IUSER", (int)kn.IUSER));
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));            
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_MOICAPNHAT>("PKG_KIENNGHI_CUTRI.PRC_KIENNGHI_MOICAPNHAT", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        //public List<PRC_KIENNGHI_CHUYENKYSAU> PRC_KIENNGHI_CHUYENKYSAU(KN_KIENNGHI kn, int ITHAMQUYENDONVI_PARENT = 0)
        //{
        //    List<PRC_KIENNGHI_CHUYENKYSAU> result = new List<PRC_KIENNGHI_CHUYENKYSAU>();
        //    List<OracleParameter> lisparam = new List<OracleParameter>();
        //    if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
        //    if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
        //    if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
        //    if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
        //    if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
        //    if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
        //    lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
        //    lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
        //    lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
        //    lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
        //    lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
        //    lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
        //    lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
        //    try
        //    {
        //        result = GetListObjetReport<PRC_KIENNGHI_CHUYENKYSAU>("PKG_KIENNGHI_CUTRI.PRC_KIENNGHI_CHUYENKYSAU", lisparam);
        //    }
        //    catch
        //    {
        //        result = null;
        //    }
        //    return result;
        //}
        //public List<PRC_KIENNGHI_CHUYENKYSAU> PRC_KIENNGHI_TRALAI(KN_KIENNGHI kn, int ITHAMQUYENDONVI_PARENT = 0)
        //{
        //    List<PRC_KIENNGHI_CHUYENKYSAU> result = new List<PRC_KIENNGHI_CHUYENKYSAU>();
        //    List<OracleParameter> lisparam = new List<OracleParameter>();
        //    if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
        //    if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
        //    if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
        //    if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
        //    if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
        //    if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
        //    lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
        //    lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
        //    lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
        //    lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
        //    lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
        //    lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
        //    lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
        //    try
        //    {
        //        result = GetListObjetReport<PRC_KIENNGHI_CHUYENKYSAU>("PKG_KIENNGHI_CUTRI.PRC_KIENNGHI_TRALAI", lisparam);
        //    }
        //    catch
        //    {
        //        result = null;
        //    }
        //    return result;
        //}
        public List<PRC_KIENNGHI_LIST> PRC_KIENNGHI_LIST(KN_KIENNGHI kn, int ITHAMQUYENDONVI_PARENT = 0,int page=1,int page_size=9999)
        {
            List<PRC_KIENNGHI_LIST> result = new List<PRC_KIENNGHI_LIST>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG= -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ITINHTRANG", (int)kn.ITINHTRANG));
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_LIST>("PKG_KIENNGHI_CUTRI.PRC_KIENNGHI_LIST", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_KIENNGHI_LIST> PRC_KIENNGHI_LIST_TRUNG(KN_KIENNGHI kn, string lstLinhVuc, string listKyHop, string maKienNghi, string listNguonKienNghi, DateTime? tungay, DateTime? denngay, int ITHAMQUYENDONVI_PARENT = 0,  int page = 1, int page_size = 9999)
        {
            List<PRC_KIENNGHI_LIST> result = new List<PRC_KIENNGHI_LIST>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.IUSER == null) { kn.IUSER = 0; }
            if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            //  lisparam.Add(new OracleParameter("p_ITINHTRANG", (int)kn.ITINHTRANG));
            lisparam.Add(new OracleParameter("p_IUSER_CAPNHAT",(int)kn.IUSER));
            lisparam.Add(new OracleParameter("p_LISTLINHVUC", lstLinhVuc));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_LISTKYHOP", listKyHop));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CMAKIENNGHI", maKienNghi));
            lisparam.Add(new OracleParameter("p_INGUONKIENNGHI", listNguonKienNghi));
            lisparam.Add(new OracleParameter("p_DTUNGAY", tungay));
            lisparam.Add(new OracleParameter("p_DDENNGAY", denngay));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_LIST>("PRC_KIENNGHI_LIST_TRUNG", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_DOANGIAMSAT> PRC_DOANGIAMSAT(string noidung, int id_donvi = 0, int iUser = 0, int page = 1, int page_size = 9999)
        {
            List<PRC_DOANGIAMSAT> result = new List<PRC_DOANGIAMSAT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            // if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
         
            lisparam.Add(new OracleParameter("p_IUSER", iUser));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", noidung));
            lisparam.Add(new OracleParameter("p_IDDONVI", id_donvi));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            result = GetListObjetReport<PRC_DOANGIAMSAT>("PKG_KIENNGHI_CUTRI.PRC_DOANGIAMSAT", lisparam);
           
            return result;
        }
        public List<PRC_KIENNGHI_LIST_TRACUU> PRC_KIENNGHI_LIST_TRACUU(KN_KIENNGHI kn, int ITHAMQUYENDONVI_PARENT,
                    DateTime dNgayNhan_from, DateTime dNgayNhan_to, DateTime dNgayTongHop_from, 
                    DateTime dNgayTongHop_to, DateTime dNgayTraLoi_from, DateTime dNgayTraLoi_to,
                    int tinhtrang_from,int tinhtrang_to, int ketqua_giamsat, int datraloi,int page=1, int page_size=30)
        {
            List<PRC_KIENNGHI_LIST_TRACUU> result = new List<PRC_KIENNGHI_LIST_TRACUU>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.IUSER == null) { kn.IUSER = 0; }
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ITINHTRANG", (int)kn.ITINHTRANG));
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_IUSER_CAPNHAT", kn.IUSER));
            lisparam.Add(new OracleParameter("p_NGAYNHAN_FROM", dNgayNhan_from));
            lisparam.Add(new OracleParameter("p_NGAYNHAN_TO", dNgayNhan_to));
            lisparam.Add(new OracleParameter("p_NGAYTONGHOP_FROM", dNgayTongHop_from));
            lisparam.Add(new OracleParameter("p_NGAYTONGHOP_TO", dNgayTongHop_to));
            lisparam.Add(new OracleParameter("p_NGAYTRALOI_FROM", dNgayTraLoi_from));
            lisparam.Add(new OracleParameter("p_NGAYTRALOI_TO", dNgayTraLoi_to));
            lisparam.Add(new OracleParameter("p_TINHTRANG_FROM", tinhtrang_from));
            lisparam.Add(new OracleParameter("p_TINHTRANG_TO", tinhtrang_to));
            lisparam.Add(new OracleParameter("p_KETQUA_GIAMSAT", ketqua_giamsat));
            lisparam.Add(new OracleParameter("p_DA_TRALOI", datraloi));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_LIST_TRACUU>("PKG_KIENNGHI_CUTRI.PRC_KIENNGHI_LIST_TRACUU", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        
        public List<PRC_KIENNGHI_LIST_TRACUU> PRC_KIENNGHI_LIST_TRACUU_CAPNHAT(KN_KIENNGHI kn, int ITHAMQUYENDONVI_PARENT,
                    DateTime dNgayNhan_from, DateTime dNgayNhan_to, DateTime dNgayTongHop_from, 
                    DateTime dNgayTongHop_to, DateTime? dNgayTraLoi_from, DateTime? dNgayTraLoi_to,
                    int tinhtrang_from, int tinhtrang_to, int ketqua_giamsat, int datraloi, string maKienNghi, string listNguonKienNghi, string cNoiDungKienNghi, int page=1, int page_size=30)
        {
            List<PRC_KIENNGHI_LIST_TRACUU> result = new List<PRC_KIENNGHI_LIST_TRACUU>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.IUSER == null) { kn.IUSER = 0; }
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ITINHTRANG", (int)kn.ITINHTRANG));
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_IUSER_CAPNHAT", kn.IUSER));
            lisparam.Add(new OracleParameter("p_NGAYNHAN_FROM", dNgayNhan_from));
            lisparam.Add(new OracleParameter("p_NGAYNHAN_TO", dNgayNhan_to));
            lisparam.Add(new OracleParameter("p_NGAYTONGHOP_FROM", dNgayTongHop_from));
            lisparam.Add(new OracleParameter("p_NGAYTONGHOP_TO", dNgayTongHop_to));
            lisparam.Add(new OracleParameter("p_NGAYTRALOI_FROM", dNgayTraLoi_from));
            lisparam.Add(new OracleParameter("p_NGAYTRALOI_TO", dNgayTraLoi_to));
            lisparam.Add(new OracleParameter("p_TINHTRANG_FROM", tinhtrang_from));
            lisparam.Add(new OracleParameter("p_TINHTRANG_TO", tinhtrang_to));
            lisparam.Add(new OracleParameter("p_KETQUA_GIAMSAT", ketqua_giamsat));
            lisparam.Add(new OracleParameter("p_DA_TRALOI", datraloi));
            lisparam.Add(new OracleParameter("p_CMAKIENNGHI", maKienNghi));
            lisparam.Add(new OracleParameter("p_INGUONKIENNGHI", listNguonKienNghi));
            lisparam.Add(new OracleParameter("p_CNOIDUNGKIENNGHI", cNoiDungKienNghi));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_LIST_TRACUU>("PRC_KIENNGHI_LIST_TRACUU", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_KIENNGHI_LIST_TRACUU> PRC_TRACUU_TRUNG(KN_KIENNGHI kn, int ITHAMQUYENDONVI_PARENT,
                    DateTime dNgayNhan_from, DateTime dNgayNhan_to,int page=1,int page_size=30)
        {
            List<PRC_KIENNGHI_LIST_TRACUU> result = new List<PRC_KIENNGHI_LIST_TRACUU>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_IUSER_CAPNHAT", kn.IUSER));
            lisparam.Add(new OracleParameter("p_NGAYNHAN_FROM", dNgayNhan_from));
            lisparam.Add(new OracleParameter("p_NGAYNHAN_TO", dNgayNhan_to));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_LIST_TRACUU>("PKG_KIENNGHI_CUTRI.PRC_TRACUU_TRUNG", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        
        public List<PRC_LIST_TONGHOP_KIENNGHI> PRC_LIST_TONGHOP_CHUATRALOI( KN_TONGHOP kn,int donvitiepnhan, string makiennghi, string listKyHop, string listNguonKienNghi , int ITHAMQUYENDONVI_PARENT,  DateTime? dtungay, DateTime? ddenngay,
             int page=1,int page_size=30)
        {
            List<PRC_LIST_TONGHOP_KIENNGHI> result = new List<PRC_LIST_TONGHOP_KIENNGHI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            //lisparam.Add(new OracleParameter("pram01", pram01));
            //lisparam.Add(new OracleParameter("pram02", pram02));
            //lisparam.Add(new OracleParameter("pram03", pram03));
            if (kn.IUSER == null) { kn.IUSER = 0; }
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITONGHOP == null) { kn.IDONVITONGHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (donvitiepnhan == null) { donvitiepnhan = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            if (makiennghi == null) { makiennghi = ""; }
            
            lisparam.Add(new OracleParameter("p_USER", (int)kn.IUSER));
            lisparam.Add(new OracleParameter("p_ITINHTRANG", (int)kn.ITINHTRANG));
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITONGHOP", (int)kn.IDONVITONGHOP));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", donvitiepnhan));
            lisparam.Add(new OracleParameter("p_LISTNGUONKN", listNguonKienNghi));
            //p_IKYHOP sẽ k được sử dụng trong procedure nữa, thay vào đó là p_LISTKYHOP

            lisparam.Add(new OracleParameter("p_MAKIENNGHI", makiennghi));
            lisparam.Add(new OracleParameter("p_LISTKYHOP", listKyHop));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_DTUNGAY", dtungay));
            lisparam.Add(new OracleParameter("p_DDENNGAY", ddenngay));
            //lisparam.Add(new OracleParameter("p_TINHTRANG_FROM", tinhtrang_from));
            //lisparam.Add(new OracleParameter("p_TINHTRANG_TO", tinhtrang_to));
            //lisparam.Add(new OracleParameter("p_CHUA_TRALOI", chua_traloi));
            //lisparam.Add(new OracleParameter("p_DA_TRALOI", da_traloi));
            //lisparam.Add(new OracleParameter("p_CHUACO_TRALOI", chuaco_traloi));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            //int row_from = post_per_page * (page - 1);
            //int row_to = post_per_page * (page);
            //lisparam.Add(new OracleParameter("p_ROW_FROM", row_from));
            //lisparam.Add(new OracleParameter("p_ROW_TO", row_to));
            try
            {
                result = GetListObjetReport<PRC_LIST_TONGHOP_KIENNGHI>("PRC_LIST_TONGHOP_CHUATRALOI", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_LIST_TONGHOP_KIENNGHI> PRC_LIST_TONGHOP_KIENNGHI(KN_TONGHOP kn, int ITHAMQUYENDONVI_PARENT,
            int tinhtrang_from, int tinhtrang_to, int chua_traloi, int da_traloi,int traloi_kiennghi,int datraloi_kiennghi, int page, int page_size)
        {
            List<PRC_LIST_TONGHOP_KIENNGHI> result = new List<PRC_LIST_TONGHOP_KIENNGHI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            //lisparam.Add(new OracleParameter("pram01", pram01));
            //lisparam.Add(new OracleParameter("pram02", pram02));
            //lisparam.Add(new OracleParameter("pram03", pram03));
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITONGHOP == null) { kn.IDONVITONGHOP = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ITINHTRANG", (int)kn.ITINHTRANG));
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITONGHOP", (int)kn.IDONVITONGHOP));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_TINHTRANG_FROM", tinhtrang_from));
            lisparam.Add(new OracleParameter("p_TINHTRANG_TO", tinhtrang_to));
            lisparam.Add(new OracleParameter("p_CHUA_TRALOI", chua_traloi));
            lisparam.Add(new OracleParameter("p_DA_TRALOI", da_traloi));
            lisparam.Add(new OracleParameter("p_TRALOI_KIENNGHI", traloi_kiennghi));
            lisparam.Add(new OracleParameter("p_DATRALOI_KIENNGHI", datraloi_kiennghi));
            
            //lisparam.Add(new OracleParameter("p_CHUACO_TRALOI", chuaco_traloi));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            //int row_from = post_per_page * (page - 1);
            //int row_to = post_per_page * (page);
            //lisparam.Add(new OracleParameter("p_ROW_FROM", row_from));
            //lisparam.Add(new OracleParameter("p_ROW_TO", row_to));
            try
            {
                result = GetListObjetReport<PRC_LIST_TONGHOP_KIENNGHI>("PKG_KIENNGHI_CUTRI.PRC_LIST_TONGHOP_KIENNGHI", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }

        public List<PRC_TONGHOP_KIENNGHI> List_PRC_KIENNGHI_TONGHOP_HDNDTINH(KN_TONGHOP kn, string listKyHop, string maKienNghi
            , string tungay, string denngay, string listLinhVuc, string listNguonKienNghi, int iDonViTiepNhan
            , int iUser, string listHuyen_Xa_ThanhPho, int ITHAMQUYENDONVI_PARENT = 0, int page = 1, int page_size = 30)
        {
            List<PRC_TONGHOP_KIENNGHI> result = new List<PRC_TONGHOP_KIENNGHI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            //if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITONGHOP == null) { kn.IDONVITONGHOP = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ITINHTRANG", -1));
            lisparam.Add(new OracleParameter("p_ILINHVUC", listLinhVuc));
            lisparam.Add(new OracleParameter("p_IDONVITONGHOP", (int)kn.IDONVITONGHOP));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_LISTKYHOP", listKyHop));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_CMAKIENNGHI", maKienNghi));
            lisparam.Add(new OracleParameter("p_DTUNGAY", tungay));
            lisparam.Add(new OracleParameter("p_DDENNGAY", denngay));
            lisparam.Add(new OracleParameter("p_INGUONKIENNGHI", listNguonKienNghi));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", iDonViTiepNhan));
            lisparam.Add(new OracleParameter("p_LIST_HUYEN_XA_TP", listHuyen_Xa_ThanhPho));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            lisparam.Add(new OracleParameter("p_IUSER", iUser));
            //int row_from = post_per_page * (page - 1);
            //int row_to = post_per_page * (page);
            //lisparam.Add(new OracleParameter("p_ROW_FROM", row_from));
            //lisparam.Add(new OracleParameter("p_ROW_TO", row_to));
            try
            {
                result = GetListObjetReport<PRC_TONGHOP_KIENNGHI>("PRC_KIENNGHI_TONGHOP_HDNDTINH", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }

        public List<PRC_KIENNGHI_MOICAPNHAT> PRC_KIENNGHI_MOICAPNHAT_DIAPHUONG(KN_KIENNGHI kn, int ITHAMQUYENDONVI_PARENT = 0, int page = 1, int page_size = 30)
        {
            List<PRC_KIENNGHI_MOICAPNHAT> result = new List<PRC_KIENNGHI_MOICAPNHAT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            if (kn.IUSER == null) { kn.IUSER = 0; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITIEPNHAN == null) { kn.IDONVITIEPNHAN = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            if (kn.INGUONKIENNGHI == null) { kn.INGUONKIENNGHI = 0; }   
            lisparam.Add(new OracleParameter("p_IUSER", (int)kn.IUSER));
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", (int)kn.IDONVITIEPNHAN));
            lisparam.Add(new OracleParameter("p_INGUONKIENNGHI", (int)kn.INGUONKIENNGHI));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_MOICAPNHAT>("PKG_KIENNGHI_CUTRI.PRC_KIENNGHI_MOICAPNHAT_DP", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_TONGHOP_KIENNGHI> List_PRC_TONGHOP_KIENNGHI(KN_TONGHOP kn,int ITHAMQUYENDONVI_PARENT=0,int page=1,int page_size=30)
        {
            List<PRC_TONGHOP_KIENNGHI> result = new List<PRC_TONGHOP_KIENNGHI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            //lisparam.Add(new OracleParameter("pram01", pram01));
            //lisparam.Add(new OracleParameter("pram02", pram02));
            //lisparam.Add(new OracleParameter("pram03", pram03));
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITONGHOP == null) { kn.IDONVITONGHOP = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ITINHTRANG", (int)kn.ITINHTRANG));
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITONGHOP", (int)kn.IDONVITONGHOP));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            //int row_from = post_per_page * (page - 1);
            //int row_to = post_per_page * (page);
            //lisparam.Add(new OracleParameter("p_ROW_FROM", row_from));
            //lisparam.Add(new OracleParameter("p_ROW_TO", row_to));
            try
            {
                result = GetListObjetReport<PRC_TONGHOP_KIENNGHI>("PKG_KIENNGHI_CUTRI.PRC_TONGHOP_KIENNGHI", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        
        // kn_tonghop, maKienNghi, listKyHop, iTruocKyHop, tungay, denngay, listNguonKienNghi, iDonViTiepNhan, iDonViXuLy, listLinhVuc, cNoiDungKienNghi, iUser, iDonViXuLy_Parent, page, post_per_page
        
        public List<PRC_TONGHOP_KIENNGHI> List_PRC_TONGHOP_KIENNGHI_BDN(KN_TONGHOP kn, string maKienNghi
            , string listKyHop, string tungay, string denngay, string listNguonKienNghi, int iDonViTiepNhan, string listLinhVuc, string cNoiDungKienNghi, string listHuyen_Xa_ThanhPho,  int iUser, int ITHAMQUYENDONVI_PARENT = 0, int page = 1, int page_size = 30)
        {
            List<PRC_TONGHOP_KIENNGHI> result = new List<PRC_TONGHOP_KIENNGHI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            //lisparam.Add(new OracleParameter("pram01", pram01));
            //lisparam.Add(new OracleParameter("pram02", pram02));
            //lisparam.Add(new OracleParameter("pram03", pram03));
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITONGHOP == null) { kn.IDONVITONGHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ITINHTRANG", -1));
            //lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_MAKIENNGHI", maKienNghi));
            lisparam.Add(new OracleParameter("p_LISTLINHVUC", listLinhVuc));
            lisparam.Add(new OracleParameter("p_IDONVITONGHOP", (int)kn.IDONVITONGHOP));
            // p_IKYHOP sẽ k đc sử dụng trong procedure
            // lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_DTUNGAY", tungay));
            lisparam.Add(new OracleParameter("p_DDENNGAY", denngay));
            lisparam.Add(new OracleParameter("p_LISTNGUONKN", listNguonKienNghi));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", iDonViTiepNhan));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", cNoiDungKienNghi));
            lisparam.Add(new OracleParameter("p_LISTKYHOP", listKyHop));
            lisparam.Add(new OracleParameter("p_LIST_HUYEN_XA_TP", listHuyen_Xa_ThanhPho));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            lisparam.Add(new OracleParameter("p_IUSER", iUser));
            //int row_from = post_per_page * (page - 1);
            //int row_to = post_per_page * (page);
            //lisparam.Add(new OracleParameter("p_ROW_FROM", row_from));
            //lisparam.Add(new OracleParameter("p_ROW_TO", row_to));
            result = GetListObjetReport<PRC_TONGHOP_KIENNGHI>("PRC_TONGHOP_KIENNGHI_BDN", lisparam);
            return result;
        }
        
        /*
        public List<PRC_TONGHOP_KIENNGHI> List_PRC_TONGHOP_KIENNGHI_BDN(KN_TONGHOP kn, string listKyHop, string listHuyen_Xa_ThanhPho, string listLinhVuc, int iUser, int ITHAMQUYENDONVI_PARENT = 0, int page = 1, int page_size = 30)
        {
            List<PRC_TONGHOP_KIENNGHI> result = new List<PRC_TONGHOP_KIENNGHI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            //lisparam.Add(new OracleParameter("pram01", pram01));
            //lisparam.Add(new OracleParameter("pram02", pram02));
            //lisparam.Add(new OracleParameter("pram03", pram03));
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITONGHOP == null) { kn.IDONVITONGHOP = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ITINHTRANG", -1));
            //lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITONGHOP", (int)kn.IDONVITONGHOP));
            // p_IKYHOP sẽ k đc sử dụng trong procedure
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_LISTKYHOP", listKyHop));
            lisparam.Add(new OracleParameter("p_LIST_HUYEN_XA_TP", listHuyen_Xa_ThanhPho));
            lisparam.Add(new OracleParameter("p_LISTLINHVUC", listLinhVuc));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            lisparam.Add(new OracleParameter("p_IUSER", iUser));
            //int row_from = post_per_page * (page - 1);
            //int row_to = post_per_page * (page);
            //lisparam.Add(new OracleParameter("p_ROW_FROM", row_from));
            //lisparam.Add(new OracleParameter("p_ROW_TO", row_to));
            result = GetListObjetReport<PRC_TONGHOP_KIENNGHI>("PKG_KIENNGHI_CUTRI.PRC_TONGHOP_KIENNGHI_BDN", lisparam);
            return result;
        }
        */

        public List<PRC_TONGHOP_KIENNGHI> List_PRC_TONGHOP_KIENNGHI_HUYEN(KN_TONGHOP kn, string listKyHop, string maKienNghi
            , string tungay, string denngay, string listLinhVuc, string listNguonKienNghi, int iDonViTiepNhan, string cNoiDungKienNghi
            , int iUser, string listHuyen_Xa_ThanhPho, int ITHAMQUYENDONVI_PARENT = 0, int page = 1, int page_size = 30)
        {
            List<PRC_TONGHOP_KIENNGHI> result = new List<PRC_TONGHOP_KIENNGHI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            //lisparam.Add(new OracleParameter("pram01", pram01));
            //lisparam.Add(new OracleParameter("pram02", pram02));
            //lisparam.Add(new OracleParameter("pram03", pram03));
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITONGHOP == null) { kn.IDONVITONGHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ITINHTRANG", -1));
            // lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_CMAKIENNGHI", maKienNghi));
            lisparam.Add(new OracleParameter("p_LIST_HUYEN_TP", listHuyen_Xa_ThanhPho));
            lisparam.Add(new OracleParameter("p_DTUNGAY", tungay));
            lisparam.Add(new OracleParameter("p_DDENNGAY", denngay));
            lisparam.Add(new OracleParameter("p_IDONVITONGHOP", (int)kn.IDONVITONGHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI_PARENT", ITHAMQUYENDONVI_PARENT));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            lisparam.Add(new OracleParameter("p_LISTLINHVUC", listLinhVuc));
            lisparam.Add(new OracleParameter("p_LISTKYHOP", listKyHop));
            lisparam.Add(new OracleParameter("p_INGUONKIENNGHI", listNguonKienNghi));
            lisparam.Add(new OracleParameter("p_IDONVITIEPNHAN", iDonViTiepNhan));
            lisparam.Add(new OracleParameter("p_PAGE", page));
            lisparam.Add(new OracleParameter("p_PAGE_SIZE", page_size));
            lisparam.Add(new OracleParameter("p_IUSER", iUser));
            //int row_from = post_per_page * (page - 1);
            //int row_to = post_per_page * (page);
            //lisparam.Add(new OracleParameter("p_ROW_FROM", row_from));
            //lisparam.Add(new OracleParameter("p_ROW_TO", row_to));
            // result = GetListObjetReport<PRC_TONGHOP_KIENNGHI>("PRC_TONGHOP_KIENNGHI_HUYEN_CAPNHAT", lisparam);
            result = GetListObjetReport<PRC_TONGHOP_KIENNGHI>("PRC_TONGHOP_KIENNGHI_HUYEN", lisparam);

            return result;
        }
        public List<PRC_COQUAN_LINHVUC> List_PRC_COQUAN_LINHVUC()
        {
            List<PRC_COQUAN_LINHVUC> result = new List<PRC_COQUAN_LINHVUC>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            
            try
            {
                result = GetListObjetReport<PRC_COQUAN_LINHVUC>("PKG_KIENNGHI_CUTRI.PRC_COQUAN_LINHVUC", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_CHUONGTRINH_TXCT_EXPORT> List_PRC_CHUONGTRINH_TXCT_EXPORT(int id_kyhop)
        {
            List<PRC_CHUONGTRINH_TXCT_EXPORT> result = new List<PRC_CHUONGTRINH_TXCT_EXPORT>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("p_IKYHOP", id_kyhop));
            try
            {
                result = GetListObjetReport<PRC_CHUONGTRINH_TXCT_EXPORT>("PKG_KIENNGHI_CUTRI.PRC_CHUONGTRINH_TXCT_EXPORT", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public List<PRC_TONGHOP_KIENNGHI> List_PRC_TONGHOP_KIENNGHI_CHUYEN_BDN(KN_TONGHOP kn)
        {
            List<PRC_TONGHOP_KIENNGHI> result = new List<PRC_TONGHOP_KIENNGHI>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            //lisparam.Add(new OracleParameter("pram01", pram01));
            //lisparam.Add(new OracleParameter("pram02", pram02));
            //lisparam.Add(new OracleParameter("pram03", pram03));
            if (kn.ITINHTRANG == null) { kn.ITINHTRANG = -1; }
            if (kn.ILINHVUC == null) { kn.ILINHVUC = -1; }
            if (kn.IDONVITONGHOP == null) { kn.IDONVITONGHOP = 0; }
            if (kn.IKYHOP == null) { kn.IKYHOP = 0; }
            if (kn.ITRUOCKYHOP == null) { kn.ITRUOCKYHOP = -1; }
            if (kn.ITHAMQUYENDONVI == null) { kn.ITHAMQUYENDONVI = 0; }
            if (kn.CNOIDUNG == null) { kn.CNOIDUNG = ""; }
            lisparam.Add(new OracleParameter("p_ILINHVUC", (int)kn.ILINHVUC));
            lisparam.Add(new OracleParameter("p_IDONVITONGHOP", (int)kn.IDONVITONGHOP));
            lisparam.Add(new OracleParameter("p_IKYHOP", (int)kn.IKYHOP));
            lisparam.Add(new OracleParameter("p_ITRUOCKYHOP", (int)kn.ITRUOCKYHOP));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", (int)kn.ITHAMQUYENDONVI));
            lisparam.Add(new OracleParameter("p_CNOIDUNG", kn.CNOIDUNG));
            //int row_from = post_per_page * (page - 1);
            //int row_to = post_per_page * (page);
            //lisparam.Add(new OracleParameter("p_ROW_FROM", row_from));
            //lisparam.Add(new OracleParameter("p_ROW_TO", row_to));
            try
            {
                result = GetListObjetReport<PRC_TONGHOP_KIENNGHI>("PKG_KIENNGHI_CUTRI.PRC_TONGHOP_CHUYEN_BDN", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
        public Boolean Delete_Kiennghi_ByID_import(int id)
        {
            KN_KIENNGHI_IMPORTRepository k = new KN_KIENNGHI_IMPORTRepository();
            return k.Delete_Kiennghi_By_ID_Import(id);
        }
        public KN_KIENNGHI_IMPORT KN_KIENNGHI_IMPORT_insert(KN_KIENNGHI_IMPORT input)
        {
            KN_KIENNGHI_IMPORTRepository k = new KN_KIENNGHI_IMPORTRepository();
            return k.AddNew(input);
        }
        public Boolean Huy_Kiennghi_taphop_import(int id_import)
        {
            bool result = false;
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("p_ID_IMPORT", id_import));
            try
            {
                result = ExecuteProcedure("PKG_KIENNGHI_CUTRI.PRC_HUY_KIENNGHI_IMPORT", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = false;
            }
            return result;
        }
        //public Boolean INSERT_IMPORT_KIENNGHI_TMP(List<KN_IMPORT_TMP> TMP)
        //{
        //    bool result = false;
        //    //List<OracleParameter> lisparam = new List<OracleParameter>();
        //    //lisparam.Add(new OracleParameter("p_ID_IMPORT", id_import));
        //    //try
        //    //{
        //    //    result = ExecuteProcedure("PKG_KIENNGHI_CUTRI.PRC_HUY_KIENNGHI_IMPORT", lisparam);
        //    //}
        //    //catch
        //    //{
        //    //    result = false;
        //    //}
        //    return result;
        //}
        //public string Get_MultyQuery(List<KN_IMPORT_TMP> tmp)
        //{
        //    KN_IMPORT_TMPRepository k = new KN_IMPORT_TMPRepository();
        //    return k.Build_MultyQuery_insert(tmp);
        //}
        public Boolean Delete_File_upload(int id)
        {
            FileuploadRepository _file = new FileuploadRepository();
            FILE_UPLOAD Input = _file.GetByID(id);
            return _file.Delete(Input);
        }
        public List<KN_TRALOI_PHANLOAI> GetAll_KN_TRALOI_PHANLOAI(Dictionary<string,object> dic = null)
        {
            KN_TRALOI_PHANLOAIRepository _kn = new KN_TRALOI_PHANLOAIRepository();
            return _kn.GetAll(dic);
        }


        public KN_TRALOI_PHANLOAI Get_KN_TRALOI_PHANLOAI(int id)
        {
            KN_TRALOI_PHANLOAIRepository _kn = new KN_TRALOI_PHANLOAIRepository();
            return _kn.GetByID(id);
        }
        public List<FILE_UPLOAD> GetAll_FileUpload(int id,string type)
        {
            FileuploadRepository _file = new FileuploadRepository();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", id);dic.Add("CTYPE", type);
            return _file.GetAll(dic);
        }
        public FILE_UPLOAD Get_FILE_UPLOAD(int id)
        {
            FileuploadRepository _f = new FileuploadRepository();
            return _f.GetByID(id);
        }
        public Boolean Delete_Traloi_byIDKIENNGHI(int id)
        {
            KN_Kiennghi_TraloiRepository t = new KN_Kiennghi_TraloiRepository();
            return t.Delete_Traloi_byIDKIENNGHI(id);
        }
        public Boolean Delete_All_ChuongTrinh_ChiTiet(int id_chuongtrinh)
        {
            bool result = true;
            KN_Chuongtrinh_ChitietRepository ct = new KN_Chuongtrinh_ChitietRepository();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ICHUONGTRINH", id_chuongtrinh);
            var lichtiep= ct.GetAll(dic);
            foreach(var l in lichtiep)
            {
                ct.Delete(l);
            }
            return result;
        }
         
        public Boolean Update_All_Tinhtrang_Kiennghi_CungNoiDung(int iTinhTrang, int ID_KIENNGHI_PARENT)
        {
            return _kiennghi.Update_All_Tinhtrang_Kiennghi_CungNoiDung(iTinhTrang, ID_KIENNGHI_PARENT);
        }
        public int Count_KienNghi_BoNganh(int iKyHop,int iDonVi,int iTinhTrang)
        {
            return _kiennghi.Count_KienNghi_BoNganh(iKyHop, iDonVi, iTinhTrang);
        }
        public int Count_KienNghi_BoNganh_DaXuLy(int iKyHop, int iDonVi)
        {
            return _kiennghi.Count_KienNghi_BoNganh_DaXuLy(iKyHop, iDonVi);
        }
        public int Count_KienNghi_BoNganh_KetQua(int iKyHop, int iDonVi, int iTinhTrang)
        {
            return _kiennghi.Count_KienNghi_BoNganh_KetQua(iKyHop, iDonVi, iTinhTrang);
        }

        public List<KN_IMPORT> GetAll_Import_Paging(Dictionary<string, object> dic,int page,int page_size)
        {
            KN_IMPORTRepository _im = new KN_IMPORTRepository();
            return _im.GetAll_Paging(dic,page,page_size);
        }
        public List<KN_IMPORT> GetAll_Import(Dictionary<string,object> dic)
        {
            KN_IMPORTRepository _im = new KN_IMPORTRepository();
            return _im.GetAll(dic);
        }
        public KN_IMPORT Insert_Import(KN_IMPORT input)
        {
            KN_IMPORTRepository _im = new KN_IMPORTRepository();
            return _im.AddNew(input);
        }
        public KN_IMPORT Get_Import(int id)
        {
            KN_IMPORTRepository _im = new KN_IMPORTRepository();
            return _im.GetByID(id);
        }
        public Boolean Update_Import(KN_IMPORT im)
        {
            KN_IMPORTRepository _im = new KN_IMPORTRepository();
            return _im.Update(im);
        }
        public Boolean Delete_Import(KN_IMPORT im)
        {
            KN_IMPORTRepository _im = new KN_IMPORTRepository();
            return _im.Delete(im);
        }
        public List<KN_DOANGIAMSAT> GetAll_Search_Doangiamsat(KN_DOANGIAMSAT giamsat)
        {
            KN_DoangiamsatRepository _doan = new KN_DoangiamsatRepository();
            return _doan.Search_Giamsat(giamsat);
        }
        public KN_VANBAN Get_Vanban(int id)
        {
            KN_VanbanRepository _vanban = new KN_VanbanRepository();
            return _vanban.GetByID(id);
        }
        
        public Boolean Delete_vanban(KN_VANBAN v)
        {
            KN_VanbanRepository _vanban = new KN_VanbanRepository();
            return _vanban.Delete(v);
        }
        public List<KN_VANBAN> Get_AllVanban(Dictionary<string, object> param)
        {
            KN_VanbanRepository _vanban = new KN_VanbanRepository();
            return _vanban.GetAll(param);
        }
        public List<KN_DOANGIAMSAT_YKIEN> GetAll_DoanGiamSat_Ykien(Dictionary<string, object> param)
        {
            KN_Doangiamsat_ykienRepository _tonghop = new KN_Doangiamsat_ykienRepository();
            return _tonghop.GetAll(param);
        }
        public List<KN_TONGHOP> Search_Tonghop(KN_TONGHOP t)
        {
            KN_TonghopRepository _t = new KN_TonghopRepository();
            return _t.Tracuu(t);
        }
        public List<KN_KIENNGHI> GetAll_KienNghi_Tracuu(KN_KIENNGHI param)
        {
            return _kiennghi.Tracuu(param);
        }
        public KN_DOANGIAMSAT_YKIEN Get_DoanGiamSat_Ykien(int id)
        {
            KN_Doangiamsat_ykienRepository _tonghop = new KN_Doangiamsat_ykienRepository();
            return _tonghop.GetByID(id);
        }
        public KN_DOANGIAMSAT_YKIEN InsertDoan_ykien(KN_DOANGIAMSAT_YKIEN input)
        {
            KN_Doangiamsat_ykienRepository _tonghop = new KN_Doangiamsat_ykienRepository();
            return _tonghop.AddNew(input);
        }
        public Boolean UpdateDoan_ykien(KN_DOANGIAMSAT_YKIEN input)
        {
            KN_Doangiamsat_ykienRepository kn = new KN_Doangiamsat_ykienRepository();
            return kn.Update(input);
        }
        public Boolean DeleteDoan_ykien(KN_DOANGIAMSAT_YKIEN input)
        {
            KN_Doangiamsat_ykienRepository kn = new KN_Doangiamsat_ykienRepository();
            return kn.Delete(input);
        }
        public List<KN_DOANGIAMSAT> GetAll_DoanGiamSat(Dictionary<string, object> param)
        {
            KN_DoangiamsatRepository _tonghop = new KN_DoangiamsatRepository();
            return _tonghop.GetAll(param);
        }
        public List<KN_DOANGIAMSAT_KIENNGHI> GetAll_DoanGiamSat_Kiengnhi(Dictionary<string, object> param)
        {
            KN_Doangiamsat_kiennghiRepository _tonghop = new KN_Doangiamsat_kiennghiRepository();
            return _tonghop.GetAll(param);
        }
        public KN_DOANGIAMSAT_KIENNGHI InsertDoan_kiennghi(KN_DOANGIAMSAT_KIENNGHI input)
        {
            KN_Doangiamsat_kiennghiRepository kn = new KN_Doangiamsat_kiennghiRepository();
            return kn.AddNew(input);
        }
        public KN_DOANGIAMSAT_KIENNGHI Get_Doan_kiennghi(int id)
        {
            KN_Doangiamsat_kiennghiRepository kn = new KN_Doangiamsat_kiennghiRepository();
            return kn.GetByID(id);
        }
        public Boolean DeleteDoan_kiennghi(KN_DOANGIAMSAT_KIENNGHI input)
        {
            KN_Doangiamsat_kiennghiRepository kn = new KN_Doangiamsat_kiennghiRepository();
            return kn.Delete(input);
        }
        public KN_DOANGIAMSAT Get_DoanGiamSat(int id)
        {
            KN_DoangiamsatRepository _tonghop = new KN_DoangiamsatRepository();
            return _tonghop.GetByID(id);
        }
        public Boolean Delete_DoanGiamSat(KN_DOANGIAMSAT input)
        {
            KN_DoangiamsatRepository _tonghop = new KN_DoangiamsatRepository();
            return _tonghop.Delete(input);
        }
        public List<KN_GIAMSAT_PHANLOAI> GetAll_GiamSat_PhanLoai()
        {
            KN_Giamsat_PhanloaiRepository _tonghop = new KN_Giamsat_PhanloaiRepository();
            return _tonghop.GetAll();
        }
        public KN_GIAMSAT_DANHGIA Get_GiamSat_DanhGia(int id)
        {
            KN_Giamsat_DanhgiaRepository _tonghop = new KN_Giamsat_DanhgiaRepository();
            return _tonghop.GetByID(id);
        }
        public KN_GIAMSAT_PHANLOAI Get_GiamSat_Phanloai(int id)
        {
            KN_Giamsat_PhanloaiRepository _tonghop = new KN_Giamsat_PhanloaiRepository();
            return _tonghop.GetByID(id);
        }
        public List<KN_GIAMSAT_DANHGIA> GetAll_GiamSat_DanhGia()
        {
            KN_Giamsat_DanhgiaRepository _tonghop = new KN_Giamsat_DanhgiaRepository();
            return _tonghop.GetAll();
        }
        public List<KN_VANBAN> GetAll_VanbanByParam(Dictionary<string, object> param)
        {
            KN_VanbanRepository _tonghop = new KN_VanbanRepository();
            return _tonghop.GetAll(param);
        }
        public KN_CHUYENXULY Insert_Chuyenxuly(KN_CHUYENXULY Input)
        {
            return _chuyenxuly.AddNew(Input);
        }
        public Boolean Update_Chuyenxuly(KN_CHUYENXULY Input)
        {
            return _chuyenxuly.Update(Input);
        }
        public KN_CHUYENXULY GetChuyenXuLy_ById(int id)
        {
            return _chuyenxuly.GetById(id);
        }
        public KN_CHUYENXULY GetChuyenXuLy_ByITONGHOP(int id)
        {
            return _chuyenxuly.GetByITONGHOP(id);
        }
        public KN_KIENNGHI_TRALOI InsertTraLoi_KienNghi(KN_KIENNGHI_TRALOI input)
        {
            KN_Kiennghi_TraloiRepository kn = new KN_Kiennghi_TraloiRepository();
            return kn.AddNew(input);
        }
        public Boolean DeleteTraLoi_KienNghi(KN_KIENNGHI_TRALOI input)
        {
            KN_Kiennghi_TraloiRepository kn = new KN_Kiennghi_TraloiRepository();
            return kn.Delete(input);
        }
        public Boolean DeleteGiamSat_KienNghi(KN_GIAMSAT input)
        {
            KN_giamsatRepository kn = new KN_giamsatRepository();
            return kn.Delete(input);
        }
        public KN_DOANGIAMSAT Insert_DoanGiamsat(KN_DOANGIAMSAT input)
        {
            KN_DoangiamsatRepository kn = new KN_DoangiamsatRepository();
            return kn.AddNew(input);
        }
        public Boolean Update_DoanGiamsat(KN_DOANGIAMSAT input)
        {
            KN_DoangiamsatRepository kn = new KN_DoangiamsatRepository();
            return kn.Update(input);
        }
        public Boolean Delele_DoanGiamsat(KN_DOANGIAMSAT input)
        {
            KN_DoangiamsatRepository kn = new KN_DoangiamsatRepository();
            return kn.Delete(input);
        }
        public KN_GIAMSAT Insert_Giamsat_traloi(KN_GIAMSAT input)
        {
            KN_giamsatRepository kn = new KN_giamsatRepository();
            return kn.AddNew(input);
        }
        public Boolean Update_Giamsat_traloi(KN_GIAMSAT input)
        {
            KN_giamsatRepository kn = new KN_giamsatRepository();
            return kn.Update(input);
        }
        public Boolean UpdateTraLoi_KienNghi(KN_KIENNGHI_TRALOI input)
        {
            KN_Kiennghi_TraloiRepository kn = new KN_Kiennghi_TraloiRepository();
            return kn.Update(input);
        }
        public List<KN_KIENNGHI_TRALOI> GetAll_TraLoi_KienNghi()
        {
            KN_Kiennghi_TraloiRepository kn = new KN_Kiennghi_TraloiRepository();
            return kn.GetAll();
        }
        public List<KN_KIENNGHI_TRALOI> GetAll_TraLoi_KienNghi_ByParamt(Dictionary<string, object> dic)
        {
            KN_Kiennghi_TraloiRepository kn = new KN_Kiennghi_TraloiRepository();
            return kn.GetAll(dic);
        }
        public List<KN_GIAMSAT> GetAll_Giamsat_TraLoi_byParam(Dictionary<string,object> dic)
        {
            KN_giamsatRepository kn = new KN_giamsatRepository();
            return kn.GetAll(dic);
        }
        public List<KN_GIAMSAT> GetAll_Giamsat_TraLoi()
        {
            KN_giamsatRepository kn = new KN_giamsatRepository();
            return kn.GetAll();
        }
        public List<KN_GIAMSAT> GetAll_Giamsat_TraLoiByKienNghiID(int kienNghiID)
        {
            KN_giamsatRepository kn = new KN_giamsatRepository();
            return kn.GetByKienNghiID(kienNghiID);
        }
        public KN_GIAMSAT Get_Giamsat_TraLoi(int id)
        {
            KN_giamsatRepository kn = new KN_giamsatRepository();
            return kn.GetByID(id);
        }
        public KN_KIENNGHI_TRALOI Get_TraLoi_KienNghi(int id)
        {
            KN_Kiennghi_TraloiRepository kn = new KN_Kiennghi_TraloiRepository();
            return kn.GetByID(id);
        }
        public QUOCHOI_KYHOP Get_KyHop_QuocHoi(int id)
        {
            Quochoi_KyhopRepository kn = new Quochoi_KyhopRepository();
            return kn.GetByID(id);
        }
        public LINHVUC Get_LinhVuc(int id)
        {
            LinhvucRepository kn = new LinhvucRepository();
            return kn.GetByID(id);
        }
        public QUOCHOI_KHOA Get_Khoa_QuocHoi(int id)
        {
            Quochoi_KhoaRepository kn = new Quochoi_KhoaRepository();
            return kn.GetByID(id);
        }
        public List<DAIBIEU> GetAll_Daibieu(Dictionary<string, object> param)
        {
            DaibieuReposity _tonghop = new DaibieuReposity();
            return _tonghop.GetAll(param);
        }
        public List<USERS> GetAll_Users()
        {
            //_user _tonghop = new DaibieuReposity();
            return _user.GetAll();
        }
        public List<KN_CHUONGTRINH_DAIBIEU> GetAll_ChuongTrinh_DaiBieu(Dictionary<string,object> dic)
        {
            //_user _tonghop = new DaibieuReposity();
            KN_Chuongtrinh_DaibieuRepository _kn = new KN_Chuongtrinh_DaibieuRepository();
            return _kn.GetAll(dic);
        }
        public List<KN_CHUONGTRINH_DIAPHUONG> GetAll_ChuongTrinh_DiaPhuong(Dictionary<string, object> dic)
        {
            //_user _tonghop = new DaibieuReposity();
            KN_Chuongtrinh_diaphuongRepository _kn = new KN_Chuongtrinh_diaphuongRepository();
            return _kn.GetAll(dic);
        }
        public List<KN_CHUONGTRINH> Search_Chuongtrinh(KN_CHUONGTRINH param)
        {
            //DaibieuReposity _tonghop = new DaibieuReposity();
            return _chuongtrinh.Tracuu(param);
        }
        public List<KN_TONGHOP> GetAll_TongHopByParam(Dictionary<string, object> param)
        {
            KN_TonghopRepository _tonghop = new KN_TonghopRepository();
            return _tonghop.GetAll(param);
        }
        public List<KN_TONGHOP> GetAll_TongHopByType(Dictionary<string, object> dict)
        {
            List<KN_TONGHOP> result = new List<KN_TONGHOP>();
            List<OracleParameter> lisparam = new List<OracleParameter>();
            lisparam.Add(new OracleParameter("p_ITINHTRANG", dict["ITINHTRANG"]));
            lisparam.Add(new OracleParameter("p_IDONVITONGHOP", dict["IDONVITONGHOP"]));
            lisparam.Add(new OracleParameter("p_ITHAMQUYENDONVI", dict["ITHAMQUYENDONVI"]));
            lisparam.Add(new OracleParameter("p_IKYHOP", dict["IKYHOP"]));
            lisparam.Add(new OracleParameter("p_IUSER", dict["IUSER"]));
            lisparam.Add(new OracleParameter("p_LOAICQ", dict["LOAICQ"]));
            result = GetListObjetReport<KN_TONGHOP>("PRC_GET_THKIENNGHI", lisparam);
            return result;
        }
        public List<QUOCHOI_COQUAN> GetAll_CoQuanByParam(Dictionary<string, object> param)
        {
            return _quochoi_coquan.GetAll(param);
        }
        public List<QUOCHOI_COQUAN> GetAll_KN_COQUAN_THUOCTHAMQUYEN(Dictionary<string, object> dic = null)
        {
            Quochoi_CoquanRepository _kn = new Quochoi_CoquanRepository();
            return _kn.GetAll(dic);
        }
        public List<KN_CHUONGTRINH> GetAll_ChuongTrinh(int iDonVi = 0, int iKyHop = 0)
        {
            Dictionary<string, object> _condition = new Dictionary<string, object>();
            if (iDonVi != 0)
            {
                _condition.Add("IDONVI", iDonVi);
            }
            if (iKyHop != 0)
            {
                _condition.Add("IKYHOP", iKyHop);
            }
            
            return _chuongtrinh.GetAll(_condition);
        }

        
        public List<QUOCHOI_KHOA> GetAll_KhoaHop()
        {
            Quochoi_KhoaRepository _khoa = new Quochoi_KhoaRepository();
            Dictionary<string, object> _condition = new Dictionary<string, object>();
            _condition.Add("IHIENTHI", 1);
            return _khoa.GetAll(_condition);
        }
        public List<QUOCHOI_KYHOP> GetAll_KyHop()
        {
            Quochoi_KyhopRepository _khoa = new Quochoi_KyhopRepository();
            Dictionary<string, object> _condition = new Dictionary<string, object>();
            _condition.Add("IHIENTHI", 1);
            return _khoa.GetAll(_condition);
        }
        public DAIBIEU Get_DaiBieu(int id)
        {
            return _daibieu.GetByID(id);
        }
        public KN_TONGHOP Get_Tonghop(int id)
        {
            KN_TonghopRepository _tonghop = new KN_TonghopRepository();
            return _tonghop.GetByID(id);
        }
        
        public List<KN_CHUONGTRINH_DIAPHUONG> Get_Diaphuong_ByChuongTrinh(int id_chuongtrinh)
        {
            KN_Chuongtrinh_diaphuongRepository ct = new KN_Chuongtrinh_diaphuongRepository();
            Dictionary<string, object> _condition = new Dictionary<string, object>();
            _condition.Add("ICHUONGTRINH", id_chuongtrinh);
            return ct.GetAll(_condition);
        }
        public Boolean Delete_Diaphuong_ByChuongTrinh(KN_CHUONGTRINH_DIAPHUONG d)
        {
            KN_Chuongtrinh_diaphuongRepository ct = new KN_Chuongtrinh_diaphuongRepository();
            return ct.Delete(d);
        }
        public List<KN_CHUONGTRINH_DAIBIEU> Get_DaiBieu_ByChuongTrinh(int id_chuongtrinh)
        {
            KN_Chuongtrinh_DaibieuRepository ct = new KN_Chuongtrinh_DaibieuRepository();
            Dictionary<string, object>  _condition = new Dictionary<string, object>();
            _condition.Add("ICHUONGTRINH", id_chuongtrinh);
            return ct.GetAll(_condition);
        }
        public Boolean Delete_DaiBieu_ByChuongTrinh(KN_CHUONGTRINH_DAIBIEU d)
        {
            KN_Chuongtrinh_DaibieuRepository ct = new KN_Chuongtrinh_DaibieuRepository();
            return ct.Delete(d);
        }
        public KN_CHUONGTRINH Get_ChuongTrinh_ByID(int id)
        {
            return _chuongtrinh.GetByID(id);
        }
        public Boolean Delete_ChuongTrinh_ByID(KN_CHUONGTRINH ct)
        {
            return _chuongtrinh.Delete(ct);
        }
        public Boolean Tracking_KN(int iUser, int ikiennghi, string content)
        {
            try
            {
                TrackingRepository _track = new TrackingRepository();
                TRACKING track = new TRACKING();
                track.IDON = 0;
                track.IUSER = iUser;
                track.CACTION = content;
                track.DDATE = DateTime.Now;
                track.IKIENNGHI = ikiennghi;
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
        public Boolean Tracking_KN_TongHop(int iUser, int ikiennghi,int iTonghop, string content)
        {
            try
            {
                TrackingRepository _track = new TrackingRepository();
                TRACKING track = new TRACKING();
                track.IDON = 0;
                track.IUSER = iUser;
                track.CACTION = content;
                track.DDATE = DateTime.Now;
                track.IKIENNGHI = ikiennghi;
                track.ITIEPDAN_DINHKY = 0; track.ITIEPDAN_THUONGXUYEN = 0;
                track.ITONGHOP = iTonghop;
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

        public Boolean Tracking(int iUser, string content)
        {
            try
            {
                TrackingRepository _track = new TrackingRepository();
                TRACKING track = new TRACKING();
                track.IDON = 0;
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
        public Boolean Tracking_Tonghop(int iUser, int itonghop, string content)
        {
            try
            {
                TrackingRepository _track = new TrackingRepository();
                TRACKING track = new TRACKING();
                track.IDON = 0;
                track.IUSER = iUser;
                track.CACTION = content;
                track.DDATE = DateTime.Now;
                track.IKIENNGHI = 0;
                track.ITIEPDAN_DINHKY = 0; track.ITIEPDAN_THUONGXUYEN = 0; track.ITONGHOP = itonghop;
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
        public List<DIAPHUONG> GetAll_DiaPhuong()
        {
            return _diaphuong.GetAll();
        }
        public List<DIAPHUONG> GetAll_DiaPhuong(Dictionary<string, object> condition)
        {
            return _diaphuong.GetAll(condition);
        }
        public KN_CHUONGTRINH_CHITIET InsertCHUONGTRINH_CHITIET(KN_CHUONGTRINH_CHITIET input)
        {
            return k.AddNew(input);
        }
        public KN_CHUONGTRINH_CHITIET GetByID_ChuongTrinhChiTiet(int id)
        {
            return k.GetByID(id);
        }
        public Boolean UpdateCHUONGTRINH_CHITIET(KN_CHUONGTRINH_CHITIET input)
        {
            return k.Update(input);
        }
        public List<KN_CHUONGTRINH_CHITIET> GetAll_CHUONGTRINH_CHITIET(Dictionary<string, object> condition)
        {
            List<KN_CHUONGTRINH_CHITIET> resule = new List<KN_CHUONGTRINH_CHITIET>();
            resule = base.GetAll<KN_CHUONGTRINH_CHITIET>(condition);
            return k.GetAll(condition);
        }
        public Boolean DeleteCHUONGTRINH_CHITIET(KN_CHUONGTRINH_CHITIET input)
        {
            return k.Delete(input);
        }

        
        public LINHVUC_COQUAN Get_LinhVuc_CoQuan(int id)
        {
            LinhvuccoquanRepository _linhvuc = new LinhvuccoquanRepository();
            return _linhvuc.GetByID(id);
        }
        public List<LINHVUC_COQUAN> GetAll_Coquan_linhvuc()
        {
            LinhvuccoquanRepository _linhvuc = new LinhvuccoquanRepository();
            return _linhvuc.GetAll();
        }
        /*  Lay het linh vuc co quan tuy nhien sap xep theo dung thu tu phan cap
         */
        public List<LINHVUC_COQUAN> GetAll_Coquan_linhvuc_SortedByParent()
        {
            LinhvuccoquanRepository _linhvuc = new LinhvuccoquanRepository();
            return _linhvuc.GetAllSorted();
        }
        public List<LINHVUC_COQUAN> GetAll_LinhVuc_CoQuan_By_IDCoQuan(int id=0)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (id != 0)
            {
                dic.Add("ICOQUAN", id);
            }            
            dic.Add("IHIENTHI", 1);
            dic.Add("IDELETE", 0);
            LinhvuccoquanRepository _linhvuc = new LinhvuccoquanRepository();
            return _linhvuc.GetAll(dic);
        }
        public List<LINHVUC> GetAll_LinhVuc()
        {
            LinhvucRepository _linhvuc = new LinhvucRepository();
            return _linhvuc.GetAll().Where(x=>x.IHIENTHI==1).ToList();
        }
        public KN_TONGHOP Get_TongHop_KienNghi(int id)
        {
            KN_TonghopRepository t = new KN_TonghopRepository();
            return t.GetByID(id);
        }
        // end phúc
        public List<QUOCHOI_COQUAN> HienThiDanhSachCoQuan()
        {
            return _quochoi_coquan.GetAll();
        }
        public List<QUOCHOI_COQUAN> HienThiDanhSachCoQuan(Dictionary<string, object> condition)
        {
            return _quochoi_coquan.GetAll(condition);
        }
        public QUOCHOI_COQUAN HienThiThongTinCoQuan(int iD)
        {
            return _quochoi_coquan.GetByID(iD); ;
        }
        public QUOCHOI_COQUAN InsertCoQuan(QUOCHOI_COQUAN input)
        {
            return _quochoi_coquan.AddNew(input);
        }
        public Boolean UpdateCoQuan(QUOCHOI_COQUAN input)
        {

            return _quochoi_coquan.Update(input);
        }
        public Boolean DelteCoQuan(QUOCHOI_COQUAN input)
        {
            return _quochoi_coquan.Delete(input);
        }
        public Boolean DelteTongHop(KN_TONGHOP input)
        {
            KN_TonghopRepository _tonghop = new KN_TonghopRepository();
            return _tonghop.Delete(input);
        }
        public Boolean Update_Vanban(KN_VANBAN input)
        {
            KN_VanbanRepository _tonghop = new KN_VanbanRepository();
            return _tonghop.Update(input);
        }
        public KN_VANBAN Insert_Vanban(KN_VANBAN input)
        {
            KN_VanbanRepository _tonghop = new KN_VanbanRepository();
            return _tonghop.AddNew(input);
        }
        public List<KN_KIENNGHI> HienThiDanhSachKienNghi(String sql)
        {
            return _kiennghi.GetList(sql);
        }
        public List<KN_KIENNGHI> HienThiDanhSachKienNghi(Dictionary<string, object> condition)
        {   
            return _kiennghi.GetAll(condition);
        }
        public List<KN_KIENNGHI> GetAll_KienNghi_Trung_Doan(KN_KIENNGHI kiennghi)
        {
            return _kiennghi.GetList_KienNghi_Trung_Doan(kiennghi);
        }
        public Boolean Update_TuKhoa(string tukhoa)
        {
            try
            {
                if (tukhoa != null && tukhoa !="")
                {
                    if (tukhoa.IndexOf(";") != -1)
                    {
                        foreach (var t in tukhoa.Split(';'))
                        {
                            if (t.Trim() != "")
                            {
                                Dictionary<string, object> _condition = new Dictionary<string, object>();
                                _condition.Add("CTUKHOA", t.ToLower().Trim());
                                if (_kiennghi_tukhoa.GetAll(_condition).Count() == 0)
                                {
                                    KN_TUKHOA tk = new KN_TUKHOA();
                                    tk.CTUKHOA = t.ToLower().Trim();
                                    _kiennghi_tukhoa.AddNew(tk);
                                    //  db.Database.ExecuteSqlCommand("insert into kn_tukhoa (cTuKhoa) values (N'" + t.ToLower().Trim() + "')");
                                }
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, object> _condition = new Dictionary<string, object>();
                        _condition.Add("CTUKHOA", tukhoa.ToLower().Trim());
                        if (_kiennghi_tukhoa.GetAll(_condition).Count() == 0)
                        {
                            KN_TUKHOA tk = new KN_TUKHOA();
                            tk.CTUKHOA = tukhoa.ToLower().Trim();
                            _kiennghi_tukhoa.AddNew(tk);
                            //  db.Database.ExecuteSqlCommand("insert into kn_tukhoa (cTuKhoa) values (N'" + t.ToLower().Trim() + "')");
                        }
                   }
                }
                return true;
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                return false;
            }
        }

        public List<TRACKING> HienThiDanhSachTracKing(Dictionary<string, object> condition)
        {
            TrackingRepository trac = new TrackingRepository();
            return trac.GetAll(condition);
        }
        public Boolean Delete_KienNghi_Tracking(TRACKING tr) {
            TrackingRepository trac = new TrackingRepository();
            return trac.Delete(tr);
        }
        //public string List_KienNghi_Chon(string kn_chon)
        //{
        //    string str = ""; int count = 1;
        //    foreach (var k in kn_chon.Split(','))
        //    {
                
        //        if (k != "")
        //        {
        //            int iKienNghi = Convert.ToInt32(k);
        //            KN_KIENNGHI kiennghi = HienThiThongTinKienNghi(iKienNghi);
        //            string del = " <a href=\"javascript:void()\" data-original-title='Hủy' rel='tooltip' title='' onclick=\"BoChonKienNghi('" + iKienNghi + "')\" class='trans_func'><i class='icon-remove'></i></a> ";
        //            str += "<tr id='tr_" + iKienNghi + "'><td class='tcenter b'>" + count + "</td><td class='tcenter b f-red'>" + kiennghi.CMAKIENNGHI +
        //                "</td><td>" + kiennghi.CNOIDUNG + "</td><td class='tcenter' nowrap>" + del + "</td></tr>";
        //            count++;
        //        }
        //    }
        //    return str;
        //}
        public KN_KIENNGHI HienThiThongTinKienNghi(int iD)
        {
            return _kiennghi.GetByID(iD);
        }
        public Boolean UpdateThongTinKienNghi(KN_KIENNGHI input)
        {
            return _kiennghi.Update(input);
        }
        public Boolean DeleteThongTinKienNghi(KN_KIENNGHI input)
        {
            return _kiennghi.Delete(input);
        }
        public DIAPHUONG HienThiThongTinDiaPhuong(int iD)
        {
            return _diaphuong.GetByID(iD); ;
        }
        public USERS HienThiThongTinTaikhoan(int iD)
        {
            return _user.GetByID(iD); ;
        }
        public USER_CHUCVU HienThiThongTinChucVu(int iD)
        {
            return _user_chucvu.GetByID(iD); ;
        }
        public KN_CHUONGTRINH InsertChuongTrinhKienNghi(KN_CHUONGTRINH Input)
        {
            return _chuongtrinh.AddNew(Input);
        }
        public Boolean UpdateChuongTrinhKienNghi(KN_CHUONGTRINH Input)
        {
            return _chuongtrinh.Update(Input);
        }
        public List<KN_CHUONGTRINH_DAIBIEU> HienThiDanhSachChuongTrinhDaiBieu(Dictionary<string, object> condition)
        {
            return _chuongtrinh_daibieu.GetAll(condition);
        }
        public Boolean DelteChuongTrinhDaiBieu(KN_CHUONGTRINH_DAIBIEU input)
        {
            return _chuongtrinh_daibieu.Delete(input);
        }
        public KN_CHUONGTRINH_DAIBIEU HienThiThongTinChuongTrinhDaiBieu(int iD)
        {
            return _chuongtrinh_daibieu.GetByID(iD); ;
        }
        public KN_CHUONGTRINH_DAIBIEU InsertChuongTrinhDaiBieu(KN_CHUONGTRINH_DAIBIEU Input)
        {
            return _chuongtrinh_daibieu.AddNew(Input);
        }
        public List<KN_CHUONGTRINH_DIAPHUONG> HienThiDanhSachChuongTrinhDiaPhuong(Dictionary<string, object> condition)
        {
            return _chuongtrinh_diaphuong.GetAll(condition);
        }
        public Boolean DelteChuongTrinhDiaPhuong(KN_CHUONGTRINH_DIAPHUONG input)
        {
            return _chuongtrinh_diaphuong.Delete(input);
        }
        public KN_CHUONGTRINH_DIAPHUONG HienThiThongTinChuongTrinhDiaPhuong(int iD)
        {
            return _chuongtrinh_diaphuong.GetByID(iD); ;
        }
        public KN_CHUONGTRINH_DIAPHUONG InsertChuongTrinhDiaPhuong(KN_CHUONGTRINH_DIAPHUONG Input)
        {
            return _chuongtrinh_diaphuong.AddNew(Input);
        }
        public KN_CHUONGTRINH HienThiThongTinChuongTrinh(int iD)
        {
            return _chuongtrinh.GetByID(iD); ;
        }
        public KN_KIENNGHI InsertKienNghi(KN_KIENNGHI Input)
        {
            return _kiennghi.AddNew(Input);
        }

        public Boolean InsertKienNghi_NguonDon(KIENNGHI_NGUONDON kn_ng)
        {
            return _kiennghi.InsertKienNghi_NguonDon(kn_ng);
        }
         
        public List<KIENNGHI_NGUONDON> GetAll_KnNguonDon_ByID(int ikn)
        {
            return _kiennghi.GetAllListKNNguonDon(ikn);
        }

        public List<KIENNGHI_NGUONDONMap> GetAll_KnNguonDon_Map(int ikn)
        {
            return _kiennghi.GetAllListNguonDonMap(ikn);
        }

        public Boolean DeleteKNNguonDon(int ikn)
        {
            return _kiennghi.DeleteKNNguonDon(ikn);
        }

        public Boolean UpdateTongHop(KN_TONGHOP Input)
        {
            KN_TonghopRepository t = new KN_TonghopRepository();
            return t.Update(Input);
        }
        public KN_TONGHOP InsertTongHop(KN_TONGHOP Input)
        {
            KN_TonghopRepository t = new KN_TonghopRepository();
            return t.AddNew(Input);
        }
        //public List<KN_TUKHOA> HienThiDanhSachTuKhoa(String sql)
        //{
        //    return _kiennghi_tukhoa.GetList(sql);
        //}
        //public KN_TUKHOA InsertTukhoa(KN_TUKHOA Input)
        //{
        //    return _kiennghi_tukhoa.AddNew(Input);
        //}
        public LINHVUC_COQUAN GetBy_Linhvuc_CoquanID(int iD)
        {
            return linhvuc_coquan.GetByID(iD);
        }
        public List<PRC_KIENNGHI_IMPORT_LISTKN> PRC_KIENNGHI_IMPORT_LISTKN(int id)
        {//LIST KIẾN NGHỊ BY ID_TONGHOP, ID_TONGHOP_BDN
            List<PRC_KIENNGHI_IMPORT_LISTKN> result = new List<PRC_KIENNGHI_IMPORT_LISTKN>();
            List<OracleParameter> lisparam = new List<OracleParameter>();

            lisparam.Add(new OracleParameter("p_ID_IMPORT", id));
            try
            {
                result = GetListObjetReport<PRC_KIENNGHI_IMPORT_LISTKN>("PRC_KIENNGHI_IMPORT_LISTKN", lisparam);
            }
            catch (Exception e)
            {
                log.Log_Error(e);
                result = null;
            }
            return result;
        }
    }
}
