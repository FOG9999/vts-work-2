using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    public interface IUKN_Giamsat_PhanloaiRepository
    {
        List<KN_GIAMSAT_PHANLOAI> GetList();
        KN_GIAMSAT_PHANLOAI AddNew(KN_GIAMSAT_PHANLOAI kngsplInput);
        Boolean Update(KN_GIAMSAT_PHANLOAI kngsplInput);
        Boolean Delete(KN_GIAMSAT_PHANLOAI kngsplInput);
    }
    public class KN_Giamsat_PhanloaiRepository : BaseRepository, IUKN_Giamsat_PhanloaiRepository
    {
        public List<KN_GIAMSAT_PHANLOAI> GetList()
        {
            string sql = "SELECT * FROM KN_GIAMSAT_PHANLOAI";
            var param = new List<OracleParameter>();
            var dt = OracleAccess.ExecuteSqlTextDataTable(sql, param);
            if (dt == null || dt.Rows.Count == 0)
                return new List<KN_GIAMSAT_PHANLOAI>();
            var lst = ObjectHelper.ToListData<KN_GIAMSAT_PHANLOAI>(dt);
            dt.Dispose();
            return lst;
        }
        public List<KN_GIAMSAT_PHANLOAI> GetAll()
        {
            List<KN_GIAMSAT_PHANLOAI> resule = new List<KN_GIAMSAT_PHANLOAI>();
            resule = base.GetAll<KN_GIAMSAT_PHANLOAI>();

            return resule;

        }
        public KN_GIAMSAT_PHANLOAI GetByID(int ID)
        {
            return base.GetItem<KN_GIAMSAT_PHANLOAI>("IPHANLOAI", ID);
        }

        public KN_GIAMSAT_PHANLOAI GetByUseName(String kngsplInput)
        {
            return base.GetItem<KN_GIAMSAT_PHANLOAI>("IPHANLOAI", kngsplInput);
        }
        public KN_GIAMSAT_PHANLOAI AddNew(KN_GIAMSAT_PHANLOAI kngsplInput)
        {

            if (base.InsertItem<KN_GIAMSAT_PHANLOAI>(kngsplInput))
            {
                return kngsplInput;
            }
            else
            {
                return null;
            }
        }
        public Boolean Update(KN_GIAMSAT_PHANLOAI kngsplInput)
        {

            if (base.UpdateItem<KN_GIAMSAT_PHANLOAI>(kngsplInput, "IPHANLOAI"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_GIAMSAT_PHANLOAI kngsplInput)
        {

            if (base.DeleteItem<KN_GIAMSAT_PHANLOAI>(kngsplInput, "IPHANLOAI"))
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

