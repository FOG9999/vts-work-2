using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dao;
using Entities.Models;
using Entities.Objects;
using System.Data;
using Oracle.ManagedDataAccess.Client;
namespace DataAccess.Busineess
{
    public class ThietlapBusineess :BaseRepository
    {
         Quochoi_KhoaRepository _quochoi_khoa = new Quochoi_KhoaRepository();
         //Quochoi_KyhopRepository _quochoi_kyhop = new Quochoi_KyhopRepository();
         User_ChucvuRepository _user_chucvu = new User_ChucvuRepository();
         ActionRepository _action = new ActionRepository();
         Quochoi_KyhopRepository _quochoi_kyhop = new Quochoi_KyhopRepository();
         UsserRepository _user = new UsserRepository();
         Quochoi_CoquanRepository _quochoi_coquan = new Quochoi_CoquanRepository();
         User_PhongbanRepository _phongban = new User_PhongbanRepository();
         User_GroupRepository _usergroup = new User_GroupRepository();
         LinhvucRepository _linhvuc = new LinhvucRepository();
         KNTC_LoaidonRepository _KNTC_loaidon = new KNTC_LoaidonRepository();
         KNTC_NoidungdonRepository _KNTC_noidungdon = new KNTC_NoidungdonRepository();
         KNTC_TinhchatRepository _KNTC_tinhchat = new KNTC_TinhchatRepository();
         KNTC_NguondonRepository _KNTC_nguondon = new KNTC_NguondonRepository();
         KN_NguondonRepository _KN_nguonkiennghi = new KN_NguondonRepository();
         DiaPhuongRepository _diaphuong = new DiaPhuongRepository();
         NghenghiepRepository _nghenghiep = new NghenghiepRepository();
         QuoctichRepository _quoctich = new QuoctichRepository();
         DantocRepository _dantoc = new DantocRepository();
         LoaivanbanRepository _loaivanban = new LoaivanbanRepository();
         UsserRepository _taikhoan = new UsserRepository();
         DaibieuReposity _daibieu = new DaibieuReposity();
         TrackingRepository _tracking = new TrackingRepository();
         User_ActionRepository _user_action = new User_ActionRepository();
         User_Group_ActionRepository _user_gr_action = new User_Group_ActionRepository();
         BaseRepository _base = new BaseRepository();
         UsertypeRepository _user_type = new UsertypeRepository();
         LinhvuccoquanRepository linhvuc_coquan = new LinhvuccoquanRepository();
         KN_TRALOI_PHANLOAIRepository traloi_phanloai = new KN_TRALOI_PHANLOAIRepository();
         DONVILINHVUCRepository donvi_linhvuc = new DONVILINHVUCRepository();
         Daibieu_kyhopReposity daibieu_kyhop = new Daibieu_kyhopReposity();

        // Dai biểu Ky - hop 

         public DAIBIEU_KYHOP Insert_DaiBieu_KyHop(DAIBIEU_KYHOP Input)
         {
             return daibieu_kyhop.AddNew(Input);
         }
         public List<DAIBIEU_KYHOP> Get_DaiBieu_KyHop()
         {
             return daibieu_kyhop.GetAll();
         }


         public DAIBIEU_KYHOP GetBy_DaiBieu_KyHopID(int iD)
         {
             return daibieu_kyhop.GetByID(iD);
         }
         public List<DAIBIEU_KYHOP> GetBy_List_DaiBieu_KyHop(Dictionary<string, object> condition)
         {
             return daibieu_kyhop.GetAll(condition);

         }
         public Boolean Update_DaiBieu_KyHop(DAIBIEU_KYHOP Input)
         {
             return daibieu_kyhop.Update(Input);
         }

         public Boolean Delete_DaiBieu_KyHop(DAIBIEU_KYHOP Input)
         {
             return daibieu_kyhop.Delete(Input);
         }
        // End
        //
         public DONVI_LINHVUC Insert_DonVi_LinhVuc(DONVI_LINHVUC Input)
         {
             return donvi_linhvuc.AddNew(Input);
         }
         public List<DONVI_LINHVUC> Get_DonVi_LinhVuc()
         {
             return donvi_linhvuc.GetAll();
         }


         public DONVI_LINHVUC GetBy_DonVi_LinhVucID(int iD)
         {
             return donvi_linhvuc.GetByID_DONVILINHVUC(iD);
         }
         public List<DONVI_LINHVUC> GetBy_List_DonVi_LinhVuc(Dictionary<string, object> condition)
         {
             return donvi_linhvuc.GetAll(condition);

         }
         public Boolean Update_DonVi_LinhVuc(DONVI_LINHVUC Input)
         {
             return donvi_linhvuc.Update(Input);
         }

         public Boolean Delete_DonVi_LinhVuc(DONVI_LINHVUC Input)
         {
             return donvi_linhvuc.Delete(Input);
         }
        //
        // Bổ sung ngày 30/12/2017
         public KN_TRALOI_PHANLOAI Insert_TraLoi_PhanLoai(KN_TRALOI_PHANLOAI Input)
         {
             return traloi_phanloai.AddNew(Input);
         }
         public List<KN_TRALOI_PHANLOAI> Get_TraLoi_PhanLoai()
         {
             return traloi_phanloai.GetAll();
         }


         public KN_TRALOI_PHANLOAI GetBy_TraLoi_PhanLoaiID(int iD)
         {
             return traloi_phanloai.GetByID(iD);
         }
         public List<KN_TRALOI_PHANLOAI> GetBy_List_TraLoi_PhanLoai(Dictionary<string, object> condition)
         {
             return traloi_phanloai.GetAll(condition);

         }
         public Boolean Update_TraLoi_PhanLoai(KN_TRALOI_PHANLOAI Input)
         {
             return traloi_phanloai.Update(Input);
         }

         public Boolean Delete_TraLoi_PhanLoai(KN_TRALOI_PHANLOAI Input)
         {
             return traloi_phanloai.Delete(Input);
         }
        // END 
         //
         public LINHVUC_COQUAN Insert_Linhvuc_Coquan(LINHVUC_COQUAN Input)
         {
             return linhvuc_coquan.AddNew(Input);
         }
         public List<LINHVUC_COQUAN> Get_Linhvuc_Coquan()
         {
             return linhvuc_coquan.GetAll();
         }
         /* Lấy danh sách lĩnh vực cơ quan sắp xếp theo cha-con
         */
         public List<LINHVUC_COQUAN> Get_Linhvuc_Coquan_Sorted()
         {
            return linhvuc_coquan.GetAllSorted();
         }

         public List<LINHVUC_COQUAN> GetById_Linhvuc_Child(int id)
         {
            return linhvuc_coquan.GetAllChild(id);
         }


        public LINHVUC_COQUAN GetBy_Linhvuc_CoquanID(int iD)
         {
             return linhvuc_coquan.GetByID(iD);
         }
         public List<LINHVUC_COQUAN> GetBy_List_Linhvuc_Coquan(Dictionary<string, object> condition)
         {
             return linhvuc_coquan.GetAll(condition);

         }
         public Boolean Update_Linhvuc_Coquan(LINHVUC_COQUAN Input)
         {
             return linhvuc_coquan.Update(Input);
         }

         public Boolean Delete_Linhvuc_Coquan(LINHVUC_COQUAN Input)
         {
             return linhvuc_coquan.Delete(Input);
         }
        //


         public List<USER_TYPE> Get_Type()
         {
             return _user_type.GetAll();
         }
         public List<USERS> TimKiemTaiKhoan(string cten)
         {
             return _user.GetList_SQLTimkiemten(cten);
         }
         public List<USER_GROUP_ACTION> Get_user_gr_action(Dictionary<string,object> dic)
         {
             return _user_gr_action.GetAll(dic);
         }
         public List<USERS> GetAll_TaiKhoanByDic(Dictionary<string, object> dic)
         {
             return _user.GetlistByCondition(dic);
         }
        public string GetName_KhoaHop_ByKyHop(int iKyHop)
        {
            string str = "";
            QUOCHOI_KYHOP kyhop = _quochoi_kyhop.GetByID(iKyHop);
            QUOCHOI_KHOA khoa = _quochoi_khoa.GetByID((int)kyhop.IKHOA);
            str = khoa.CTEN;

            return str;
        }
        public string GetName_KyHop_KhoaHop(int iKyHop)
        {
            string str = "";
            QUOCHOI_KYHOP kyhop = _quochoi_kyhop.GetByID(iKyHop);
            QUOCHOI_KHOA khoa = _quochoi_khoa.GetByID((int)kyhop.IKHOA);
            str = "<strong>" + kyhop.CTEN + "</strong></br>" + khoa.CTEN;

            return str;
        }
        public string GetName_KyHop_KhoaHopBaoCao(int iKyHop)
        {
            string str = "";
            if(_quochoi_kyhop.GetByID(iKyHop) != null)
            {
                QUOCHOI_KYHOP kyhop = _quochoi_kyhop.GetByID(iKyHop);
                if (_quochoi_khoa.GetByID((int)kyhop.IKHOA) != null)
                {
                    QUOCHOI_KHOA khoa = _quochoi_khoa.GetByID((int)kyhop.IKHOA);
                    str = "" + kyhop.CTEN + " Khóa: " + khoa.CTEN;
                }
            }
          
            return str;
        }
        // Danh mục Tracking
        public TRACKING Insert_Tracking(TRACKING Input)
        {
            return _tracking.AddNew(Input);
        }
        public List<TRACKING> Get_Tracking()
        {
            return _tracking.GetAll();
        }
        public List<USER_CHUCVU> GetBy_List_User_Chucvu(Dictionary<string, object> condition)
        {
            return _user_chucvu.GetAllWhere_Paging(condition);
        }

        public TRACKING GetBy_TrackingID(int iD)
        {
            return _tracking.GetByID(iD);
        }
        public List<TRACKING> GetBy_List_Tracking(Dictionary<string, object> condition)
        {
            return _tracking.GetAll(condition);
        }

        public Boolean Update_Tracking(TRACKING Input)
        {
            return _tracking.Update(Input);
        }

        public Boolean Delete_Tracking(TRACKING Input)
        {
            return _tracking.Delete(Input);
        }
        // End Danh mục Tracking



        // Danh mục action
        public ACTION Insert_Action(ACTION Input)
        {
            return _action.AddNew(Input);
        }
        public List<ACTION> Get_Action()
        {
            return _action.GetAll();
        }
        public List<ACTION> Get_List_Action(String sql)
        {
            return _action.GetList(sql);
        }
        //public List<ACTION> Get_List_Action_Sql(int iParent, int id_nhom)
        //{
        //    string sql = "select action.* from action inner join USER_GROUP_ACTION on " +
        //       " ACTION.IACTION=USER_GROUP_ACTION.IACTION and ACTION.IPARENT =" + iParent + " and USER_GROUP_ACTION.IGROUP=" + id_nhom + "";
        //    return _action.GetList(sql);
        //}
        //public List<USER_GROUP_ACTION> Get_List_US_Gr_Sql(int id)
        //{

        //    return _user_gr_action.GetList_SQL(id);
        //}
        //public List<USER_GROUP_ACTION> GetList_SQL_insert(int gr, string action)
        //{
        //    return _user_gr_action.GetList_SQL_insert(gr, action);
        //}
        
 
        public ACTION GetBy_ActionID(int iD)
        {
            return _action.GetByID(iD);
        }
        public List<ACTION> GetBy_List_Action(Dictionary<string, object> condition)
        {
            return _action.GetAll(condition);
        }
        public List<ACTION> GetBy_List_Action()
        {
            return _action.GetAll();
        }
        public List<USER_GROUP_ACTION> Get_user_Group_Action(Dictionary<string, object> condition = null)
        {
            if(condition == null)
            {
                return _user_gr_action.GetAll();
            }
            else
            {
                return _user_gr_action.GetAll(condition);
            }
        }
       
        
        // End danh mục action



        // Danh mục user_group 
        public Boolean Update_Userchucvu(USER_CHUCVU input)
        {
            return _user_chucvu.Update(input);
        }
        public List<USER_CHUCVU> Get_List_User_Chucvu()
        {
            return _user_chucvu.GetAll();
        }
        public List<USER_CHUCVU> Get_List_User_Chucvu_By_Conditions(Dictionary<string, object> condition)
        {
            return _user_chucvu.GetAll(condition);
        }
        public USER_CHUCVU Insert_chucvu(USER_CHUCVU input)
        {
            return _user_chucvu.AddNew(input);
        }
    


        public QUOCHOI_KHOA Insert_QuocHoi_Khoa(QUOCHOI_KHOA Input)
        {
            return _quochoi_khoa.AddNew(Input);
        }
        public List<QUOCHOI_KHOA> Get_List_Quochoi_Khoa()
        {
            return _quochoi_khoa.GetAll();
        }
        public List<QUOCHOI_KHOA> Get_List_Quochoi_Khoa(Dictionary<string, object> condition)
        {
            return _quochoi_khoa.GetAll(condition);
        }
        public Boolean Update_QuocHoi_Khoa(QUOCHOI_KHOA Input)
        {
            return _quochoi_khoa.Update(Input);
        }
        public QUOCHOI_KHOA Get_Quochoi_Khoa(int id)
        {
           return _quochoi_khoa.GetByID(id);
        }
        // Quốc hội kỳ hop
        public QUOCHOI_KYHOP Insert_QuocHoi_Kyhop(QUOCHOI_KYHOP Input)
        {
            return _quochoi_kyhop.AddNew(Input);
        }
        public List<QUOCHOI_KYHOP> Get_List_Quochoi_Kyhop()
        {
            return _quochoi_kyhop.GetAll();
        }
        public List<QUOCHOI_KYHOP> Get_List_Quochoi_Kyhop(Dictionary<string, object> condition)
        {
            return _quochoi_kyhop.GetAll(condition);
        }
        public Boolean Update_QuocHoi_Kyhop(QUOCHOI_KYHOP Input)
        {
            return _quochoi_kyhop.Update(Input);
        }
        public QUOCHOI_KYHOP Get_Quochoi_Kyhop(int id)
        {
            return _quochoi_kyhop.GetByID(id);
        }
        // Co quan
        public QUOCHOI_COQUAN Insert_Quochoi_Coquan(QUOCHOI_COQUAN Input)
        {
            return _quochoi_coquan.AddNew(Input);
        }
        public List<QUOCHOI_COQUAN> Get_List_Quochoi_Coquan()
        {
            return _quochoi_coquan.GetAll();
        }
        public List<QUOCHOI_COQUAN> Get_ListQuochoi_Coquan()
        {
            return _quochoi_coquan.GetAll();
        }
        public QUOCHOI_COQUAN GetBy_Quochoi_CoquanID(int iD)
        {
            return _quochoi_coquan.GetByID(iD);
        }
        public List<QUOCHOI_COQUAN> GetBy_List_Quochoi_Coquan(Dictionary<string, object> condition)
        {
            return _quochoi_coquan.GetAll(condition);
        }
        public List<QUOCHOI_COQUAN> GetQuocHoiCoQuanTreeList()
        {
            return _quochoi_coquan.GetQuocHoiCoQuanTreeList();
        }
        public Boolean Update_QuocHoi_Coquan(QUOCHOI_COQUAN Input)
        {
            return _quochoi_coquan.Update(Input);
        }
        public QUOCHOI_COQUAN Get_Quochoi_Coquan(int id)
        {
            return _quochoi_coquan.GetByID(id);
        }
        public Boolean Delete_QuocHoi_Coquan(QUOCHOI_COQUAN Input)
        {
            return _quochoi_coquan.Delete(Input);
        }
       
        public List<QUOCHOI_COQUAN> Get_List_CoQuan_SQL(string cCode, int icoquan)
        {
            return _quochoi_coquan.GetList_CheckMaCoQuan_Update(cCode, icoquan);
        }
        public List<QUOCHOI_COQUAN> Get_List_CoQuan_SQL_CheckInsert(string cCode)
        {
            return _quochoi_coquan.GetList_CheckMaCoQuan_Insert(cCode);
        }
        public List<LINHVUC> Get_List_LinhVuc_SQL_CheckInsert(string cCode)
        {
            return _linhvuc.GetList_CheckMaLinhVuc_Insert(cCode);
          
         //   return _linhvuc.GetList(SQL);
        }
        public List<LINHVUC> Get_List_LinhVuc_SQL_CheckUpdate(string cCode,int id)
        {
            return _linhvuc.GetList_CheckMaLinhVuc_Update(cCode,id);

            //   return _linhvuc.GetList(SQL);
        }
        // phong ban
        public USER_PHONGBAN Insert_Phongban(USER_PHONGBAN Input)
        {
            return _phongban.AddNew(Input);
        }
        public List<USER_PHONGBAN> Get_List_Phongban()
        {
            return _phongban.GetAll();
        }
        public List<USER_PHONGBAN> GetToDaiBieuList()
        {
            return _phongban.GetToDaiBieuList();
        }
        public List<USER_PHONGBAN> GetPhongBanTreeList()
        {
            return _phongban.GetPhongBanTreeList();
        }
        public USER_PHONGBAN GetBy_PhongbanID(int iD)
        {
            return _phongban.GetByID(iD);
        }
        public List<USER_PHONGBAN> GetBy_List_Phongban(Dictionary<string, object> condition)
        {
            return _phongban.GetAll(condition);
        }
        public Boolean Update_Phongban(USER_PHONGBAN Input)
        {
            return _phongban.Update(Input);
        }
        public USER_PHONGBAN Get_Phongban(int id)
        {
            return _phongban.GetByID(id);
        }
        public USER_CHUCVU Get_Chucvu(int id)
        {
            return _user_chucvu.GetByID(id);
        }
        // Nhom 
        public USER_GROUP Insert_Usergroup(USER_GROUP Input)
        {
            return _usergroup.AddNew(Input);
        }
        public List<USER_GROUP> GetBy_List_Usergroup(Dictionary<string, object> condition)
        {
            return _usergroup.GetAll(condition);
        }

        public List<USER_GROUP> Get_Usergroup()
        {
            return _usergroup.GetAll();
        }
        public USER_GROUP GetBy_UsergroupID(int iD)
        {
            return _usergroup.GetByID(iD);
        }
        public Boolean Update_Usergroup(USER_GROUP Input)
        {
            return _usergroup.Update(Input);
        }
       
        public List<ACTION> Get_List_UsergroupSql(int id_nhom,int id_parent=0)
        {
            return _action.Get_List_Action_ByIDGroup(id_nhom, id_parent);
        }
        public List<USER_GROUP> Get_All_Usergroup()
        {
            return _usergroup.GetAll();
        }
        
        public List<USER_ACTION> GetBy_List_User_Action(Dictionary<string, object> condition)
        {
            return _user_action.GetAll(condition);
        }
        public List<USER_ACTION> GetBy_List_User_Action()
        {
            return _user_action.GetAll();
        }
        public Boolean Delete_User_Action(USER_ACTION Input)
        {
            return _user_action.Delete(Input);
        }
        public Boolean Delete_User_Action_Multi(Dictionary<string, object> condition)
        {
            return _user_action.DellAll(condition);
        }
        public USER_GROUP_ACTION Insert_Usergroupaction(USER_GROUP_ACTION Input)
        {
            return _user_gr_action.AddNew(Input);
        }
        public USER_ACTION Get_User_Action(int ID)
        {
            return _user_action.GetByID(ID);
        }
        public List<USER_ACTION> Get_SQL_KIEMTRA(string arr, int id)
        {
            return _user_action.GetList_user_action(arr,id);
        }
        
        // END  USER GROUP
        //Taif khoan
        public USERS Insert_Taikhoan(USERS Input)
        {
            return _taikhoan.AddNew(Input);
        }
        public List<USERS> Get_Taikhoan()
        {
            return _taikhoan.GetAll();
        }
       
        public List<USERS> Get_List_Taikhoan_Sql_checkuser(string username, int id_user)
        {
          
            return _taikhoan.GetList_UserCheck(username, id_user);
        }
        
        public USERS Get_User(int id)
        {
            return _user.GetByID(id);
        }
        public USERS GetBy_TaikhoanID(int iD)
        {
            return _taikhoan.GetByID(iD);
        }
        public List<USERS> GetBy_List_Taikhoan(Dictionary<string, object> condition)
        {
            return _taikhoan.GetAll(condition);
        }
        public Boolean Update_Taikhoan(USERS Input)
        {
            return _taikhoan.Update(Input);
        }
        public USER_ACTION Insert_User_Action(USER_ACTION Input)
        {
            return _user_action.AddNew(Input);
        }

        // DDaij bieu

        public DAIBIEU Insert_Daibieu(DAIBIEU Input)
        {
            return _daibieu.AddNew(Input);
        }
        public List<DAIBIEU> Get_Daibieu()
        {
            return _daibieu.GetAll();
        }

        public DAIBIEU GetBy_DaibieuID(int iD)
        {
            return _daibieu.GetByID(iD);
        }

        public Boolean Update_Daibieu(DAIBIEU Input)
        {
            return _daibieu.Update(Input);
        }
        
        public DIAPHUONG Insert_Diaphuong(DIAPHUONG Input)
        {
            return _diaphuong.AddNew(Input);
        }
        public List<DIAPHUONG> Get_Diaphuong()
        {
            return _diaphuong.GetAll();
        }
        public List<DIAPHUONG> Get_List_Diaphuong_Sql(string code, int idiaphuong)
        {
           
            return _diaphuong.GetList_CheckMaDIAPHUONG_Update(code,idiaphuong);
        }
        public DIAPHUONG GetBy_DiaphuongID(int iD)
        {
            return _diaphuong.GetByID(iD);
        }
        public List<DIAPHUONG> GetBy_List_Diaphuong(Dictionary<string, object> condition)
        {
            return _diaphuong.GetAll(condition);
        }

        public Boolean Update_Diaphuong(DIAPHUONG Input)
        {
            return _diaphuong.Update(Input);
        }
        
        public KNTC_NGUONDON Insert_Nguondon(KNTC_NGUONDON Input)
        {
            return _KNTC_nguondon.AddNew(Input);
        }
        public List<KNTC_NGUONDON> Get_Nguondon()
        {
            return _KNTC_nguondon.GetAll();
        }

        public KNTC_NGUONDON GetBy_NguondonID(int iD)
        {
            return _KNTC_nguondon.GetByID(iD);
        }
        public Boolean Update_Nguondon(KNTC_NGUONDON Input)
        {
            return _KNTC_nguondon.Update(Input);
        }

        public KN_NGUONDON Insert_Nguonkiennghi(KN_NGUONDON Input)
        {
            return _KN_nguonkiennghi.AddNew(Input);
        }
        public List<KN_NGUONDON> Get_Nguonkiennghi()
        {
            return _KN_nguonkiennghi.GetAll();
        }

        public KN_NGUONDON GetBy_NguonkiennghiID(int iD)
        {
            return _KN_nguonkiennghi.GetByID(iD);
        }
        public Boolean Update_Nguonkiennghi(KN_NGUONDON Input)
        {
            return _KN_nguonkiennghi.Update(Input);
        }

        //end nhóm nguồn đơn

        // Nhóm tính chất đơn
        public KNTC_TINHCHAT Insert_Tinhchat(KNTC_TINHCHAT Input)
        {
            return _KNTC_tinhchat.AddNew(Input);
        }
        public List<KNTC_TINHCHAT> Get_Tinhchat()
        {
            return _KNTC_tinhchat.GetAll();
        }
      

        public KNTC_TINHCHAT GetBy_TinhchatID(int iD)
        {
            return _KNTC_tinhchat.GetByID(iD);
        }

        public Boolean Update_Tinhchat(KNTC_TINHCHAT Input)
        {
            return _KNTC_tinhchat.Update(Input);
        }
       
        // end nhóm tính chất đơn



        // Nhóm nội dung đơn
        public KNTC_NOIDUNGDON Insert_Noidungdon(KNTC_NOIDUNGDON Input)
        {
            return _KNTC_noidungdon.AddNew(Input);
        }
        public List<KNTC_NOIDUNGDON> Get_Noidungdon()
        {
            return _KNTC_noidungdon.GetAll();
        }
        
        public KNTC_NOIDUNGDON GetBy_NoidungdonID(int iD)
        {
            return _KNTC_noidungdon.GetByID(iD);
        }
        public List<KNTC_NOIDUNGDON> GetBy_List_Noidungdon(Dictionary<string, object> condition)
        {
            return _KNTC_noidungdon.GetAll(condition);
        }
        //public List<USER_GROUP> Get_List_Usergroup(Dictionary<string, object> condition)
        //{
        //    return _usergroup.GetAll(condition);
        //}
        public Boolean Update_Noidungdon(KNTC_NOIDUNGDON Input)
        {
            return _KNTC_noidungdon.Update(Input);
        }
       
        

        // end nhóm nội dung đơn

        // Nhóm loại đơn
        public KNTC_LOAIDON Insert_Loaidon(KNTC_LOAIDON Input)
        {
            return _KNTC_loaidon.AddNew(Input);
        }
        public List<KNTC_LOAIDON> Get_Loaidon()
        {
            return _KNTC_loaidon.GetAll();
        }
        

        public KNTC_LOAIDON GetBy_LoaidonID(int iD)
        {
            return _KNTC_loaidon.GetByID(iD);
        }
        
        //public List<USER_GROUP> Get_List_Usergroup(Dictionary<string, object> condition)
        //{
        //    return _usergroup.GetAll(condition);
        //}
        public Boolean Update_Loaidon(KNTC_LOAIDON Input)
        {
            return _KNTC_loaidon.Update(Input);
        }
      
        // End nhóm loại đơn
        // Nhóm lĩnh vực
        public LINHVUC Insert_Linhvuc(LINHVUC Input)
        {
            return _linhvuc.AddNew(Input);
        }
        public List<LINHVUC> Getby_List_Loaidon(Dictionary<string, object> condition)
        {
            return _linhvuc.GetAll(condition);
        }
        public List<LINHVUC> Get_Linhvuc()
        {
            return _linhvuc.GetAll();
        }
        
        public LINHVUC GetBy_LinhvucID(int iD)
        {
            return _linhvuc.GetByID(iD);
        }
        //public List<USER_GROUP> Get_List_Usergroup(Dictionary<string, object> condition)
        //{
        //    return _usergroup.GetAll(condition);
        //}
        public Boolean Update_Linhvuc(LINHVUC Input)
        {
            return _linhvuc.Update(Input);
        }
        public List<LINHVUC> GetBy_List_Linhvuc(Dictionary<string, object> condition)
        {
            List<LINHVUC> resule = new List<LINHVUC>();
            resule = _linhvuc.GetAll<LINHVUC>(condition);
            return resule;
        }

        public Boolean Delete_LinhVuc(LINHVUC Input)
        {
            return _linhvuc.Delete(Input);
        }
        //end lĩnh vực
        public NGHENGHIEP GetBy_NghenghiepID(int iD)
        {
            return _nghenghiep.GetByID(iD);
        }
        public Boolean Update_Nghenghiep(NGHENGHIEP Input)
        {
            return _nghenghiep.Update(Input);
        }
        public QUOCTICH GetBy_QuoctichID(int iD)
        {
            return _quoctich.GetByID(iD);
        }
        public Boolean Update_Quoctich(QUOCTICH Input)
        {
            return _quoctich.Update(Input);
        }
       

        public List<QUOCTICH> Get_Quoctich()
        {
            List<QUOCTICH> resule = new List<QUOCTICH>();
            resule = _quoctich.GetAll<QUOCTICH>();
            return resule;
        }
        public List<DANTOC> Get_Dantoc()
        {
            List<DANTOC> resule = new List<DANTOC>();
            resule = _dantoc.GetAll<DANTOC>();
            return resule;
        }
       
        public List<NGHENGHIEP> Get_Nghenghiep()
        {
            List<NGHENGHIEP> resule = new List<NGHENGHIEP>();
            resule = _nghenghiep.GetAll<NGHENGHIEP>();
            return resule;
        }
        public NGHENGHIEP Insert_Nghenghiep(NGHENGHIEP Input)
        {
            return _nghenghiep.AddNew(Input);
        }
        public QUOCTICH Insert_Quoctich(QUOCTICH Input)
        {
            return _quoctich.AddNew(Input);
        }
        public VB_LOAI Insert_Loaivanban(VB_LOAI Input)
        {
            return _loaivanban.AddNew(Input);
        }
        public VB_LOAI GetBy_LoaivanbanID(int iD)
        {
            return _loaivanban.GetByID(iD);
        }
        public Boolean Update_Loaivanban(VB_LOAI Input)
        {
            return _loaivanban.Update(Input);
        }
        public List<VB_LOAI> GetBy_List_VB_LOAI(Dictionary<string, object> condition)
        {
            List<VB_LOAI> resule = new List<VB_LOAI>();
            resule = _loaivanban.GetAll<VB_LOAI>(condition);
            return resule;
        }

        public List<VB_LOAI> Get_Loaivanban()
        {
            List<VB_LOAI> resule = new List<VB_LOAI>();
            resule = _loaivanban.GetAll<VB_LOAI>();
            return resule;
        }
        public DANTOC GetBy_DantocID(int iD)
        {
            return _dantoc.GetByID(iD);
        }
        public List< DANTOC> Get_DantocID()
        {
            return _dantoc.GetAll();
        }
        public Boolean Update_Dantoc(DANTOC Input)
        {
            return _dantoc.Update(Input);
        }
        public DANTOC Insert_Dantoc(DANTOC Input)
        {
            return _dantoc.AddNew(Input);
        }

        
        public List<ID_Parent> GetListID_Parent_SQL(int iD)
        {
            return _user.GetListID_Parent_SQL(iD);
        }
        public List<ID_Parent> GetListID_Parent_SQL_CheckAction(int ip, int iD)
        {
            return _user.GetListID_Parent_SQL_CheckAction(ip, iD);
        }
        public bool updateUserGroupFunctionList(int GroupID, List<USER_GROUP_ACTION> ListUserGroupObj)
        {

            bool resule = false;

            OracleTransaction transObj;

            if (base.Conn == null) { base.intConn(); }

            transObj = base.Conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {



                var param = new List<OracleParameter>()

                {

                    new OracleParameter("pram", GroupID)

                };

                String Sql = "delete  USER_GROUP_ACTION  where  IGROUP =:pram ";

                resule = base.ExcuteSQLWithTrans(transObj, Sql, param);

                if (resule)
                {

                    foreach (USER_GROUP_ACTION obj in ListUserGroupObj)
                    {

                        obj.ID = base.GetNextValSeqWithTrans(transObj, "USER_GROUP_ACTION_SEQ");

                        resule = base.InsertItemWithTrans<USER_GROUP_ACTION>(transObj, obj);

                        if (resule == false)
                        {

                            break;

                        }

                    }



                }

                if (resule)
                {

                    transObj.Commit();

                }

                else
                {

                    transObj.Rollback();



                }

            }

            catch (Exception ex)
            {

                transObj.Rollback();

                resule = false;

            }

            finally
            {

                transObj.Dispose();

            }





            return resule;



        }


        public List<TRACKING> TIMKIEMTRACKING(string key, string tungay, string denngay)
        {
            return _tracking.GetList_TRACKING_search(key, tungay, denngay);
        }
        
        public DataTable GetDonViPhongBanChaCon()
        {
            string sql = "select dv.CTEN as TenDonVi, pb.CTEN as TenPhongBanCha, pb.IPHONGBAN from QUOCHOI_COQUAN dv inner join USER_PHONGBAN pb on pb.IDONVI = dv.ICOQUAN where pb.IPARENT = 0 AND dv.IDELETE = 0 AND dv.IHIENTHI = 1";
            return GetList_DataTable(sql, new List<OracleParameter>());
        }
    }
}
