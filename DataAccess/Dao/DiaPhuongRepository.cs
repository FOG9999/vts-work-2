using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
   public interface IUDiaphuongRepository
    {
        DIAPHUONG GetByID(int ID);
        DIAPHUONG AddNew(DIAPHUONG Input);
        Boolean Update(DIAPHUONG Input);
        Boolean Delete(DIAPHUONG Input);
        List<DIAPHUONG> GetAll();
        List<DIAPHUONG> GetAll(Dictionary<string, object> dictionary);
       
    }
   public class DiaPhuongRepository : BaseRepository, IUDiaphuongRepository
   {
       public List<DIAPHUONG> GetAll()
       {
           List<DIAPHUONG> resule = new List<DIAPHUONG>();
           resule = base.GetAll<DIAPHUONG>();
           return resule;
       }
       public List<DIAPHUONG> GetList(String sql)
       {
           List<DIAPHUONG> resule = new List<DIAPHUONG>();
           resule = base.GetList<DIAPHUONG>(sql);
           return resule;
       }
       public List<DIAPHUONG> GetList_Diaphuong_IParent(int parent, int id)
       {

           List<DIAPHUONG> listObj = new List<DIAPHUONG>();
           var param = new List<OracleParameter>();
           try
           {
               int idel = id;
               int iparents = parent;
               string sql = "SELECT * from DIAPHUONG where IDIAPHUONG != " + " :param01";
               param.Add(new OracleParameter("param01", id));
               sql += " and  IPARENT =:param02";
               param.Add(new OracleParameter("param02", iparents));
               //sql += " and  ICOQUAN !=:param03";
               //param.Add(new OracleParameter("param03", id));
               listObj = base.GetList<DIAPHUONG>(sql, param);


           }
           catch (Exception)
           {
               throw;
           }
           return listObj;
       }

       public List<DIAPHUONG> GetAll(Dictionary<string, object> dictionary)
       {
           List<DIAPHUONG> resule = new List<DIAPHUONG>();
           resule = base.GetAll<DIAPHUONG>(dictionary);
           return resule;
       }
       public List<DIAPHUONG> GetList_CheckMaDIAPHUONG_Update(string code, int id)
       {
           List<DIAPHUONG> listObj = new List<DIAPHUONG>();
           var param = new List<OracleParameter>();
           try
           {
               int idel = id;
               string sql = "SELECT * from DIAPHUONG where IDIAPHUONG != " + " :param01";
               param.Add(new OracleParameter("param01", idel));
               sql += " and  UPPER(CCODE) =:param02";
               var noidungkey = new OracleParameter("param02", OracleDbType.NVarchar2);
               noidungkey.Value = code.Trim();
               param.Add(noidungkey);
               //sql += " and  ICOQUAN !=:param03";
               //param.Add(new OracleParameter("param03", id));
               listObj = base.GetList<DIAPHUONG>(sql, param);


           }
           catch (Exception)
           {
               throw;
           }
           return listObj;
       }
       public DIAPHUONG GetByID(int ID)
       {
           return base.GetItem<DIAPHUONG>("IDIAPHUONG", ID);
       }
       public DIAPHUONG AddNew(DIAPHUONG Input)
       {
           decimal ID = 0;
           ID = base.GetNextValSeq("DIAPHUONG_SEQ");
           if (ID != 0)
           {
               Input.IDIAPHUONG = ID;
               if (base.InsertItem<DIAPHUONG>(Input))
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
       public Boolean Update(DIAPHUONG Input)
       {

           if (base.UpdateItem<DIAPHUONG>(Input, "IDIAPHUONG"))
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       public Boolean Delete(DIAPHUONG Input)
       {

           if (base.DeleteItem<DIAPHUONG>(Input, "IDIAPHUONG"))
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
