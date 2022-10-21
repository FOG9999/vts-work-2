using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Entities.Objects;
namespace DataAccess.Dao
{
    public interface IUKN_KiennghiRepository
    {
        List<KN_KIENNGHI> GetAll();
        List<KN_KIENNGHI> GetAll(Dictionary<string, object> condition);
        List<ID_ChuongTrinh> GetList_ChuongTrinh(String sql);
        KN_KIENNGHI AddNew(KN_KIENNGHI kngsdgInput);
        Boolean Update(KN_KIENNGHI kngsdgInput);
        Boolean Delete(KN_KIENNGHI kngsdgInput);
        Boolean InsertKienNghi_NguonDon(KIENNGHI_NGUONDON kn_nd);
        List<KIENNGHI_NGUONDON> GetAllListKNNguonDon(int ikn);
        Boolean DeleteKNNguonDon(int ikn);
        List<KIENNGHI_NGUONDONMap> GetAllListNguonDonMap(int ikn);
        
        
    }
    public class KN_KiennghiRepository : BaseRepository, IUKN_KiennghiRepository
    {
        Log log = new Log();
        public List<KN_KIENNGHI> GetAll()
        {
            List<KN_KIENNGHI> resule = new List<KN_KIENNGHI>();
            resule = base.GetAll<KN_KIENNGHI>();
            return resule;
        }
        public List<KN_KIENNGHI> GetAll(Dictionary<string,object> condition)
        {
            List<KN_KIENNGHI> resule = new List<KN_KIENNGHI>();
            resule = base.GetAll<KN_KIENNGHI>(condition);
            return resule;
        }
        public List<KN_KIENNGHI> GetList(String sql)
        {
            List<KN_KIENNGHI> resule = new List<KN_KIENNGHI>();
            resule = base.GetList<KN_KIENNGHI>(sql);
            return resule;
        }
        public List<ID_ChuongTrinh> GetList_ChuongTrinh(String sql)
        {
            List<ID_ChuongTrinh> resule = new List<ID_ChuongTrinh>();
            resule = base.GetList<ID_ChuongTrinh>(sql);
            return resule;
        }
        
        public List<KN_KIENNGHI> GetList_KienNghi_Trung_Doan(KN_KIENNGHI kiennghi)
        {
            List<KN_KIENNGHI> listObj = new List<KN_KIENNGHI>();
            var param = new List<OracleParameter>();
            try { 
                string sql = "select * from KN_KIENNGHI where IKIENNGHI!=" + ":ikiennghi";
                param.Add(new OracleParameter("ikiennghi", kiennghi.IKIENNGHI));
                sql+= " and IDONVITIEPNHAN=" + ":idonvi";
                param.Add(new OracleParameter("idonvi", kiennghi.IDONVITIEPNHAN));
                sql += " and  UPPER(CNOIDUNG) like '%' || upper(:param02) || '%'";
                //OracleParameter noidung = new OracleParameter;
                var noidung = new OracleParameter("param02", OracleDbType.NVarchar2);
                noidung.Value = kiennghi.CNOIDUNG.Trim().ToUpper();
                param.Add(noidung);
                listObj = base.GetList<KN_KIENNGHI>(sql, param);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiến nghị trùng");
                throw;
            }
            return listObj;
        }
        public KN_KIENNGHI GetByID(int ID)
        {
            return base.GetItem<KN_KIENNGHI>("IKIENNGHI", ID);
        }

        public KN_KIENNGHI GetByUseName(String kngsdgInput)
        {
            return base.GetItem<KN_KIENNGHI>("IKIENNGHI", kngsdgInput);
        }
        public KN_KIENNGHI AddNew(KN_KIENNGHI Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KN_KIENNGHI_SEQ");
            if (ID != 0)
            {
                Input.IKIENNGHI = ID;
                if (base.InsertItem<KN_KIENNGHI>(Input))
                {
                    return Input;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
           
        }

        public Boolean InsertKienNghi_NguonDon(KIENNGHI_NGUONDON kn_nd)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KIENNGHI_NGUONDON_SEQ");
            if(ID != 0)
            {
                kn_nd.ID = ID;
                if (base.InsertItem<KIENNGHI_NGUONDON>(kn_nd))
                {
                    return true;
                }
                else
                {
                    return false; ;
                }
            }
            else
            { return false; }    
        }

        public List<KIENNGHI_NGUONDON> GetAllListKNNguonDon(int ikn)
        {
            List<KIENNGHI_NGUONDON> listObj = new List<KIENNGHI_NGUONDON>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "select * from KIENNGHI_NGUONDON where IKIENNGHI =" + ":ikn";
                param.Add(new OracleParameter("ikn", ikn));
                listObj = base.GetList<KIENNGHI_NGUONDON>(sql, param);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Không tìm thấy");
                throw;
            }
            return listObj;
        }

        public List<KIENNGHI_NGUONDONMap> GetAllListNguonDonMap(int ikn)
        {
            List<KIENNGHI_NGUONDONMap> listObj = new List<KIENNGHI_NGUONDONMap>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "select * from KIENNGHI_NGUONDON inner join KN_NGUONDON on kiennghi_nguondon.inguondon = KN_NGUONDON.inguondon where kiennghi_nguondon.ikiennghi =:ikn";
                param.Add(new OracleParameter("ikn", ikn));
                listObj = base.GetList<KIENNGHI_NGUONDONMap>(sql, param);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Không tìm thấy");
                throw;
            }
            return listObj;
        }

        public Boolean DeleteKNNguonDon(int ikn)
        {
            bool result = true;
            try
            {
                if(ikn != 0)
                {
                    var param = new List<OracleParameter>();
                    string sql = "Delete KIENNGHI_NGUONDON where IKIENNGHI =: ikn ";
                    param.Add(new OracleParameter("ikn", ikn));
                    result = base.ExcuteSQL(sql, param);
                }    
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Không xóa được kiến nghị nguồn đơn");
                result = false;
                throw;
            }
            return result;
        }


        public Boolean Update_All_Tinhtrang_Kiennghi_CungNoiDung(int iTinhTrang,int ID_KIENNGHI_PARENT)
        {
            bool result = true;
            try
            {
                if (ID_KIENNGHI_PARENT != 0)
                {
                    var param = new List<OracleParameter>();
                    string sql = "update KN_KIENNGHI SET ITINHTRANG=:tinhtrang where ID_KIENNGHI_PARENT=:id_parent";
                    param.Add(new OracleParameter("tinhtrang", iTinhTrang));
                    param.Add(new OracleParameter("id_parent", ID_KIENNGHI_PARENT));
                    result = base.ExcuteSQL(sql, param);
                }                
            }
            catch(Exception ex)
            {
                log.Log_Error(ex, "update tất cả trạng thái kiến nghị", "Update_All_Tinhtrang_Kiennghi");
                result = false;
                throw;
            }
            return result;
        }
        public Boolean Update(KN_KIENNGHI kngsdgInput)
        {

            if (base.UpdateItem<KN_KIENNGHI>(kngsdgInput, "IKIENNGHI"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        
        public Boolean Delete(KN_KIENNGHI kngsdgInput)
        {

            if (base.DeleteItem<KN_KIENNGHI>(kngsdgInput, "IKIENNGHI"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int Count_KienNghi_BoNganh(int iKyHop,int iDonVi,int iTinhTrang)
        {
            int count = 0;
            try
            {
                var param = new List<OracleParameter>();
                string sql = "select count(*) from KN_KIENNGHI INNER JOIN QUOCHOI_COQUAN ON " +
                            "    KN_KIENNGHI.ITHAMQUYENDONVI = QUOCHOI_COQUAN.ICOQUAN" +
                            "    AND QUOCHOI_COQUAN.IPARENT <> 20";
                if (iKyHop != 0)
                {
                    sql += " and KN_KIENNGHI.IKYHOP = " + ":kyhop";
                    param.Add(new OracleParameter("kyhop", iKyHop));
                }
                if (iDonVi != 0)
                {
                    sql += " and KN_KIENNGHI.ITHAMQUYENDONVI = " + ":donvi";
                    param.Add(new OracleParameter("donvi", iDonVi));
                }
                if (iTinhTrang != -1)
                {
                    sql += " and KN_KIENNGHI.ITINHTRANG = " + ":tinhtrang";
                    param.Add(new OracleParameter("tinhtrang", iTinhTrang));
                }
                count = (int)base.Phuc_GetTotal_(sql, param);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiến nghị bộ ngành");
                throw;
            }
            return count;
        }
        public int Count_KienNghi_BoNganh_DaXuLy(int iKyHop, int iDonVi)
        {
            int count = 0;
            try
            {
                var param = new List<OracleParameter>();
                string sql = "select count(*) from KN_KIENNGHI INNER JOIN QUOCHOI_COQUAN ON " +
                            "    KN_KIENNGHI.ITHAMQUYENDONVI = QUOCHOI_COQUAN.ICOQUAN" +
                            "   AND KN_KIENNGHI.ITINHTRANG>=5 AND QUOCHOI_COQUAN.IPARENT <> 20";
                if (iKyHop != 0)
                {
                    sql += " and KN_KIENNGHI.IKYHOP = " + ":kyhop";
                    param.Add(new OracleParameter("kyhop", iKyHop));
                }
                if (iDonVi != 0)
                {
                    sql += " and KN_KIENNGHI.ITHAMQUYENDONVI = " + ":donvi";
                    param.Add(new OracleParameter("donvi", iDonVi));
                }
                
                count = (int)base.Phuc_GetTotal_(sql, param);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiến nghị đã xử lý");
                throw;
            }
            return count;
        }
        public int Count_KienNghi_BoNganh_KetQua(int iKyHop, int iDonVi,int IPHANLOAI)
        {
            int count = 0;
            try
            {
                var param = new List<OracleParameter>();
                string sql = "select count(*) from KN_KIENNGHI INNER JOIN QUOCHOI_COQUAN ON " +
                            "    KN_KIENNGHI.ITHAMQUYENDONVI = QUOCHOI_COQUAN.ICOQUAN" +
                            "   AND KN_KIENNGHI.ITINHTRANG>=5 AND QUOCHOI_COQUAN.IPARENT <> 20";
                if (iKyHop != 0)
                {
                    sql += " and KN_KIENNGHI.IKYHOP = " + ":kyhop";
                    param.Add(new OracleParameter("kyhop", iKyHop));
                }
                if (iDonVi != 0)
                {
                    sql += " and KN_KIENNGHI.ITHAMQUYENDONVI = " + ":donvi";
                    param.Add(new OracleParameter("donvi", iDonVi));
                }
                sql += " inner join KN_GIAMSAT ON KN_GIAMSAT.IKIENNGHI=KN_KIENNGHI.IKIENNGHI ";
                if (IPHANLOAI == 0)
                {
                    sql += " and KN_GIAMSAT.IPHANLOAI IN (1,2)";
                }
                else
                {
                    sql += " and KN_GIAMSAT.IPHANLOAI = " + ":danhgia";
                    param.Add(new OracleParameter("danhgia", IPHANLOAI));
                }
                
                count = (int)base.Phuc_GetTotal_(sql, param);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiến nghị theo phân loại");
                throw;
            }
            return count;
        }
        public List<KN_KIENNGHI> Tracuu(KN_KIENNGHI input)
        {
            List<KN_KIENNGHI> listObj = new List<KN_KIENNGHI>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from KN_KIENNGHI where ITINHTRANG >= 0 "; // input.ITINHTRANGXULY + "";
                if (input.ITINHTRANG != -1 && input.ITINHTRANG!=null)
                {
                    sql += " and ITINHTRANG = " + ":tinhtrang";
                    param.Add(new OracleParameter("tinhtrang", input.ITINHTRANG));
                }
                if (input.CNOIDUNG != "" && input.CNOIDUNG != null)
                {
                    sql += " and  UPPER(CNOIDUNG) like '%' || upper(:param02) || '%'";
                    //OracleParameter noidung = new OracleParameter;
                    var noidung = new OracleParameter("param02", OracleDbType.NVarchar2);
                    noidung.Value = input.CNOIDUNG.Trim().ToUpper();
                    param.Add(noidung);
                }
                if (input.ILINHVUC != -1 && input.ILINHVUC!=null)
                {
                    sql += " and ILINHVUC = " + ":linhvuc";
                    param.Add(new OracleParameter("linhvuc", input.ILINHVUC));
                }
                if (input.IKYHOP != 0 && input.IKYHOP != null)
                {
                    sql += " and IKYHOP = " + ":kyhop";
                    param.Add(new OracleParameter("kyhop", input.IKYHOP));
                }
                if (input.IDONVITIEPNHAN !=0 && input.IDONVITIEPNHAN != null)
                {
                    sql += " and IDONVITIEPNHAN = " + ":pr3";
                    param.Add(new OracleParameter("pr3", input.IDONVITIEPNHAN));
                }
                if (input.ITHAMQUYENDONVI != -1 && input.ITHAMQUYENDONVI != null)
                {
                    sql += " and ITHAMQUYENDONVI = " + ":pr4";
                    param.Add(new OracleParameter("pr4", input.ITHAMQUYENDONVI));
                }
                if (input.ITRUOCKYHOP != -1 && input.ITRUOCKYHOP != null)
                {
                    sql += " and ITRUOCKYHOP = " + ":pr4";
                    param.Add(new OracleParameter("pr4", input.ITRUOCKYHOP));
                }
                sql += " order by IKIENNGHI desc";
                listObj = base.GetList<KN_KIENNGHI>(sql, param);
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Tìm kiến nghị");
                throw;
            }
            return listObj;

        }
        
    }
}

