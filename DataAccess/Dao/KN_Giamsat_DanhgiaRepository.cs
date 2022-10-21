using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
    public interface IUKN_giamsat_danhgiaRepository
    {
       
        List<KN_GIAMSAT_DANHGIA> GetList(String sql);
        KN_GIAMSAT_DANHGIA AddNew(KN_GIAMSAT_DANHGIA kngsdgInput);
        Boolean Update(KN_GIAMSAT_DANHGIA kngsdgInput);
        Boolean Delete(KN_GIAMSAT_DANHGIA kngsdgInput);
    }
    public class KN_Giamsat_DanhgiaRepository : BaseRepository, IUKN_giamsat_danhgiaRepository
    {
     
        public List<KN_GIAMSAT_DANHGIA> GetList(String sql)
        {
            List<KN_GIAMSAT_DANHGIA> resule = new List<KN_GIAMSAT_DANHGIA>();
            resule = base.GetList<KN_GIAMSAT_DANHGIA>(sql);
            return resule;
        }
        public List<KN_GIAMSAT_DANHGIA> GetAll()
        {
            List<KN_GIAMSAT_DANHGIA> resule = new List<KN_GIAMSAT_DANHGIA>();
            resule = base.GetAll<KN_GIAMSAT_DANHGIA>();

            return resule;

        }
        public List<KN_GIAMSAT_DANHGIA> GetAll(Dictionary<string, object> dictionary)
        {
            List<KN_GIAMSAT_DANHGIA> resule = new List<KN_GIAMSAT_DANHGIA>();
            resule = base.GetAll<KN_GIAMSAT_DANHGIA>(dictionary);

            return resule;
        }
        public KN_GIAMSAT_DANHGIA GetByID(int ID)
        {
            return base.GetItem<KN_GIAMSAT_DANHGIA>("IDANHGIA", ID);
        }

        public KN_GIAMSAT_DANHGIA GetByUseName(String kngsdgInput)
        {
            return base.GetItem<KN_GIAMSAT_DANHGIA>("IDANHGIA", kngsdgInput);
        }
        public KN_GIAMSAT_DANHGIA AddNew(KN_GIAMSAT_DANHGIA Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("KN_GIAMSAT_DANHGIA_SEQ");
            if (ID != 0)
            {
                Input.IDANHGIA = ID;
                if (base.InsertItem<KN_GIAMSAT_DANHGIA>(Input))
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
        public Boolean Update(KN_GIAMSAT_DANHGIA kngsdgInput)
        {

            if (base.UpdateItem<KN_GIAMSAT_DANHGIA>(kngsdgInput, "IDANHGIA"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(KN_GIAMSAT_DANHGIA kngsdgInput)
        {

            if (base.DeleteItem<KN_GIAMSAT_DANHGIA>(kngsdgInput, "IDANHGIA"))
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
