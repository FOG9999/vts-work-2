using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utilities;
namespace DataAccess.Dao
{

    public interface IKNTC_DONRepository
    {
        List<KNTC_DON> GetList(String sql);
        KNTC_DON GetByID(int ID);
        List<KNTC_DON> Get_IDDonvi_xulydon(int itinhtrang, int id_donvitiepnhan);
        KNTC_DON GetByUseName(String Username);
        KNTC_DON AddNew(KNTC_DON UserInput);
        Boolean Update(KNTC_DON UserInput);
        Boolean Delete(KNTC_DON UserInput);
        List<KNTC_DON> GetList_Don(Dictionary<string, object> dictionary);
        List<KNTC_DON> GetList_Don();
    }
    public class KNTC_DonRepository : BaseRepository, IKNTC_DONRepository
    {
        Log log = new Log();
        public List<KNTC_DON> GetList_Don(Dictionary<string, object> dictionary)
        {
            List<KNTC_DON> resule = new List<KNTC_DON>();
            resule = base.GetAll<KNTC_DON>(dictionary);
            return resule;
        }
        public List<KNTC_DON> GetPageList_Don(Dictionary<string, object> dictionary)
        {
            List<KNTC_DON> resule = new List<KNTC_DON>();
            resule = base.GetAll<KNTC_DON>(dictionary);
            return resule;
        }
        public List<KNTC_DON> GetList_Don()
        {
            List<KNTC_DON> resule = new List<KNTC_DON>();
            resule = base.GetAll<KNTC_DON>();
            return resule;
        }
        public List<KNTC_DON> GetList(String sql)
        {
            List<KNTC_DON> resule = new List<KNTC_DON>();
            resule = base.GetList<KNTC_DON>(sql);
            return resule;
        }
        public KNTC_DON GetByID(int ID)
        {
            return base.GetItem<KNTC_DON>("IDON", ID);
        }
        public List<KNTC_DON> GetByIdImport(int IdImport)
        {
            Dictionary<string, object> donParam = new Dictionary<string, object>();
            donParam.Add("IDELETE", 0);
            donParam.Add("IIDIMPORT", IdImport);
            return base.GetAll<KNTC_DON>(donParam);
        }
        public List<KNTC_DON> List_Don_Trung(int DonID, int idontrung, int idonvi)
        {
            //List<KNTC_DON> listObj = new List<KNTC_DON>();
            //var param = new List<OracleParameter>();
            //string sql = "";
            //if (idontrung == -1)
            //{
            //    sql = "SELECT * FROM KNTC_DON WHERE IDONTRUNG = :param01 AND IDONVITIEPNHAN =:param02 ";
            //    param.Add(new OracleParameter("param01", DonID));
            //    param.Add(new OracleParameter("param02", idonvi));
            //    listObj = base.GetList<KNTC_DON>(sql, param);

            //}
            //else if (idontrung > 0)
            //{
            //    sql = "SELECT KNTC_DON.* FROM KNTC_DON WHERE IDONTRUNG = :param01 AND IDON != :param02 UNION  ALL SELECT KNTC_DON.* FROM KNTC_DON WHERE IDON = :param01  AND IDONVITIEPNHAN =:param03 ";
            //    param.Add(new OracleParameter("param01",idontrung ));
            //    param.Add(new OracleParameter("param02", DonID));
            //    param.Add(new OracleParameter("param03", idonvi));
            //    listObj = base.GetList<KNTC_DON>(sql, param);
            //}
            //else
            //{
            //    listObj = null;
            //}
            List<KNTC_DON> listObj = new List<KNTC_DON>();
            var param = new List<OracleParameter>();
            if (idontrung == -1)
            {
                string sql = "SELECT * FROM KNTC_DON " +
                          "  WHERE   KNTC_DON.IDONTRUNG = :param01";
                param.Add(new OracleParameter("param01", DonID));
                listObj = base.GetList<KNTC_DON>(sql, param);
            } 
            if (idontrung > 0)
            {
                string sql = "SELECT * FROM KNTC_DON  WHERE   KNTC_DON.IDON = :param01" + " Or (KNTC_DON.IDONTRUNG = :param01  and  KNTC_DON.IDON <> :param02)";
                param.Add(new OracleParameter("param01", idontrung));
                param.Add(new OracleParameter("param02", DonID));
                listObj = base.GetList<KNTC_DON>(sql, param);

            }

            //else {

            //    listObj = null; // truong hop khong có don trung

            //}

            return listObj;
        }
        public List<KNTC_DON> Get_IDDonvi_xulydon(int itinhtrang, int id_xuly)
        {

            List<KNTC_DON> listObj = new List<KNTC_DON>();
            var param = new List<OracleParameter>();
            string sql = "SELECT  IDONVITHULY, COUNT(IDONVITHULY) AS IDON FROM KNTC_DON WHERE  IDONVITIEPNHAN = :param03 AND ITINHTRANGXULY = :param01 GROUP BY IDONVITHULY";
            param.Add(new OracleParameter("param01", itinhtrang));
            param.Add(new OracleParameter("param03", id_xuly));
            //if (id_donvi != 0)
            //{
            //    sql += " and IDONVITHULY =" + " :param02";
            //    param.Add(new OracleParameter("param02", id_donvi));
            //}

            listObj = base.GetList<KNTC_DON>(sql, param);
            return listObj;
        }
        public List<KNTC_DON> Get_IDDonvi_TiepNhanDon(int itinhtrang, int id_tiepnhan)
        {

            List<KNTC_DON> listObj = new List<KNTC_DON>();
            var param = new List<OracleParameter>();
            string sql = "SELECT  IDONVITIEPNHAN, COUNT(IDONVITHULY) AS IDON FROM KNTC_DON WHERE  IDONVITHULY = :param03 AND ITINHTRANGXULY = :param01 GROUP BY IDONVITIEPNHAN";
            param.Add(new OracleParameter("param01", itinhtrang));
            param.Add(new OracleParameter("param03", id_tiepnhan));
            //if (id_donvi != 0)
            //{
            //    sql += " and IDONVITHULY =" + " :param02";
            //    param.Add(new OracleParameter("param02", id_donvi));
            //}

            listObj = base.GetList<KNTC_DON>(sql, param);
            return listObj;
        }
        public KNTC_DON GetByUseName(String actionInput)
        {
            return base.GetItem<KNTC_DON>("IDON", actionInput);
        }
        public KNTC_DON AddNew(KNTC_DON actionInput)
        {

            decimal ID = 0;
            ID = base.GetNextValSeq("KNTC_DON_SEQ");
            if (ID != 0)
            {
                actionInput.IDON = ID;
                if (base.InsertItem<KNTC_DON>(actionInput))
                {
                    return actionInput;
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

        public bool AddList(List<KNTC_DON> list)
        {
            bool result = true;
            try
            {
                foreach(var item in list)
                {
                    item.IDON = base.GetNextValSeq("KNTC_DON_SEQ");
                    if (!base.InsertItemWithTrans(item))
                    {
                        result = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }

        public Boolean Update(KNTC_DON actionInput)
        {

            if (base.UpdateItem<KNTC_DON>(actionInput, "IDON"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KNTC_DON actionInput)
        {

            if (base.DeleteItem<KNTC_DON>(actionInput, "IDON"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<KNTC_DON> DonMoiCapNhat(KNTC_DON input, string tungay, string denngay)
        {
            List<KNTC_DON> listObj = new List<KNTC_DON>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from KNTC_DON where 1 = 1 "; // input.ITINHTRANGXULY + "";

                if (input.ITINHTRANGXULY != -1)
                {
                    sql += " and ITINHTRANGXULY =" + " :param01";
                    param.Add(new OracleParameter("param01", input.ITINHTRANGXULY));
                }
                if (input.CNOIDUNG != "")
                {
                    sql += " and ( UPPER(CNOIDUNG) like '%' || UPPER(:param02) || '%'";
                    var noidung = new OracleParameter("param02", OracleDbType.NVarchar2);
                    noidung.Value = input.CNOIDUNG.Trim().ToUpper();
                    param.Add(noidung);
                }
                if (input.CNOIDUNG != "")
                {
                    sql += " OR  UPPER(CNGUOIGUI_DIACHI) like '%' || UPPER(:param03) || '%'";
                    var noidung = new OracleParameter("param03", OracleDbType.NVarchar2);
                    noidung.Value = input.CNGUOIGUI_DIACHI.Trim().ToUpper();
                    param.Add(noidung);
                }
                if (input.CNOIDUNG != "")
                {
                    sql += " OR  UPPER(CNGUOIGUI_TEN) like '%' || UPPER(:param04) || '%' ) ";
                    var noidung = new OracleParameter("param04", OracleDbType.NVarchar2);
                    noidung.Value = input.CNGUOIGUI_TEN.Trim().ToUpper();
                    param.Add(noidung);
                }
                if (input.IDOANDONGNGUOI != 0)
                {
                    sql += " and IDOANDONGNGUOI =" + " :param05";
                    param.Add(new OracleParameter("param05", input.IDOANDONGNGUOI));
                }
                if (input.IDIAPHUONG_0 != 0)
                {
                    sql += " and IDIAPHUONG_0 =" + " :param06";
                    param.Add(new OracleParameter("param06", input.IDIAPHUONG_0));
                }
                if (input.IDONVITHULY != null)
                {
                    if (input.IDONVITHULY != 0)
                    {
                        sql += " and IDONVITHULY =" + " :param07";
                        param.Add(new OracleParameter("param07", input.IDONVITHULY));
                    }

                }
                if (input.ILOAIDON != -1)
                {
                    sql += " and ILOAIDON =" + " :param08";
                    param.Add(new OracleParameter("param08", input.ILOAIDON));
                }

                if (input.ILINHVUC != -1)
                {
                    sql += " and ILINHVUC =" + " :param09";
                    param.Add(new OracleParameter("param09", input.ILINHVUC));
                }
                if (input.INOIDUNG != -1)
                {
                    sql += " and INOIDUNG =" + " :param10";
                    param.Add(new OracleParameter("param10", input.INOIDUNG));
                }
                if (input.ITINHCHAT != -1)
                {
                    sql += " and ITINHCHAT =" + " :param11";
                    param.Add(new OracleParameter("param11", input.ITINHCHAT));
                }
                if (input.INGUONDON != 0)
                {
                    sql += " and INGUONDON =" + " :param12";
                    param.Add(new OracleParameter("param12", input.INGUONDON));
                }
                if (input.INGUOIGUI_DANTOC != 0)
                {
                    sql += " and INGUOIGUI_DANTOC =" + " :param13";
                    param.Add(new OracleParameter("param13", input.INGUOIGUI_DANTOC));
                }
                if (input.INGUOIGUI_QUOCTICH != 0)
                {
                    sql += " and INGUOIGUI_QUOCTICH =" + " :param14";
                    param.Add(new OracleParameter("param14", input.INGUOIGUI_QUOCTICH));
                }
                if (tungay != "")
                {
                    sql += " AND  DNGAYNHAN  >= " + " :param15";
                    var ptungay = new OracleParameter("param15", OracleDbType.Date);
                    ptungay.Value = tungay;
                    param.Add(ptungay);

                    //sql += " and DNGAYNHAN >= '" + " :param15";
                    //param.Add(new OracleParameter("param15", tungay));
                }
                if (denngay != "")
                {
                    sql += " AND  DNGAYNHAN  <= " + " :param16";
                    var pdenngay = new OracleParameter("param16", OracleDbType.Date);
                    pdenngay.Value = denngay;
                    param.Add(pdenngay);

                    //sql += " and DNGAYNHAN <= '" + " :param16";
                    //param.Add(new OracleParameter("param16", denngay));
                }
                sql += " order by IDON desc";
                listObj = base.GetList<KNTC_DON>(sql, param);

            }
            catch (Exception ex)
            {
                // ghi log lỗi tại đây
                log.Log_Error(ex, "Danh sách đơn khiếu nại tố cáo");
                throw;

            }
            return listObj;
        }
        public List<KNTC_DON> DanhSachDon(Dictionary<string, object> d, int page, int pageSize)
        {

            List<KNTC_DON> listObj = new List<KNTC_DON>();
            var param = new List<OracleParameter>();

            try
            {

                listObj = base.GetAll_PageList<KNTC_DON>(d, page, pageSize);

            }
            catch (Exception ex)
            {
                // ghi log lỗi tại đây
                log.Log_Error(ex, "Danh sách đơn khiếu nại tố cáo");
                throw;

            }
            return listObj;
        }
        public List<KNTC_DON> List_DonTrung_ByID(KNTC_DON input)
        {
            List<KNTC_DON> listObj = new List<KNTC_DON>();
            var param = new List<OracleParameter>();
            try
            {
                string sql = "SELECT * from KNTC_DON where 1 =1 AND IDOKHAN = 1 AND IDOMAT = 1 ";
                if (input.IDON != 0)
                {
                    sql += "AND  IDON <> :param01";
                    param.Add(new OracleParameter("param01", input.IDON));
                }

                if (input.CNGUOIGUI_TEN != "")
                {
                    sql += " and  UPPER(CNGUOIGUI_TEN) like '%' || UPPER(:param03) || '%' ";
                    var tennguoigui = new OracleParameter("param03", OracleDbType.NVarchar2);
                    tennguoigui.Value = input.CNGUOIGUI_TEN.Trim();
                    param.Add(tennguoigui);
                }


                if (input.IDIAPHUONG_0 != 0)
                {

                    sql += " and IDIAPHUONG_0 = " + " :param04";
                    param.Add(new OracleParameter("param04", input.IDIAPHUONG_0));
                }
                //if (input.IDONVITIEPNHAN != 0)
                //{

                //    sql += " and IDONVITIEPNHAN = " + " :param06";
                //    param.Add(new OracleParameter("param06", input.IDONVITIEPNHAN));
                //}
                //int trangthai = 5;
                //sql += " and ITINHTRANGXULY <> " + " :param05";
                //param.Add(new OracleParameter("param05", trangthai));
                sql += " order by DDATE";
                listObj = base.GetList<KNTC_DON>(sql, param);
                return listObj;
            }
            catch (Exception ex)
            {
                log.Log_Error(ex, "Danh sách đơn khiếu nại tố cáo trùng");
                return null;
            }
        }

    }
}
