using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    public interface IULoaivanbanRepository
    {
        VB_LOAI GetByID(int ID);
        VB_LOAI AddNew(VB_LOAI Input);
        Boolean Update(VB_LOAI Input);
        Boolean Delete(VB_LOAI Input);
        List<VB_LOAI> GetAll();
        List<VB_LOAI> GetAll_By(String CCODE);

    }
    public class LoaivanbanRepository : BaseRepository, IULoaivanbanRepository
    {
        public List<VB_LOAI> GetList(String sql)
        {
            List<VB_LOAI> resule = new List<VB_LOAI>();
            resule = base.GetList<VB_LOAI>(sql);
            return resule;
        }
        public List<VB_LOAI> GetAll()
        {
            List<VB_LOAI> resule = new List<VB_LOAI>();
            resule = base.GetAll<VB_LOAI>();
            return resule;
        }
        public List<VB_LOAI> GetAll_By(String CCODE)
        {
            var sql = @"SELECT * FROM VB_LOAI WHERE 1 = 1";
            var param = new List<OracleParameter>();
            if (!string.IsNullOrEmpty(CCODE))
            {
                sql += " and CCODE = :ccode ";
                param.Add(new OracleParameter("ccode", CCODE));
            }
        
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
            if (dt == null || dt.Rows.Count == 0)
                return new List<VB_LOAI>();
            var lstObj = ObjectHelper.ToListData<VB_LOAI>(dt);
            dt.Dispose();
            return lstObj;
        }
        public VB_LOAI GetByID(int ID)
        {
            return base.GetItem<VB_LOAI>("ILOAI", ID);
        }
        public VB_LOAI AddNew(VB_LOAI Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("VB_LOAI_SEQ");
            if (ID != 0)
            {
                Input.ILOAI = ID;
                if (base.InsertItem<VB_LOAI>(Input))
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
        public Boolean Update(VB_LOAI Input)
        {

            if (base.UpdateItem<VB_LOAI>(Input, "ILOAI"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(VB_LOAI Input)
        {

            if (base.DeleteItem<VB_LOAI>(Input, "ILOAI"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
