using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Oracle.ManagedDataAccess.Client;
namespace DataAccess.Dao
{
     public interface IUDaibieuRepository
    {
        List<DAIBIEU> GetAll();
        DAIBIEU GetByID(int ID);
        List<DAIBIEU> GetAll(Dictionary<string, object> condition);
        DAIBIEU AddNew(DAIBIEU UserInput);
        Boolean Update(DAIBIEU UserInput);
        Boolean Delete(DAIBIEU UserInput);
        List<DAIBIEU> Getlist(String sql);
    }
     public class DaibieuReposity : BaseRepository, IUDaibieuRepository 
    {
        public DAIBIEU GetByID(int ID) 
        {
            return base.GetItem<DAIBIEU>("IDAIBIEU", ID);        
        }         
        public List<DAIBIEU> GetAll()
        {
            List<DAIBIEU> resule = new List<DAIBIEU>();
            resule = base.GetAll<DAIBIEU>();
            return resule;
        }
        public List<DAIBIEU> GetAll(Dictionary<string,object> condition)
        {
            List<DAIBIEU> resule = new List<DAIBIEU>();
            resule = base.GetAll<DAIBIEU>(condition);
            return resule;
        }
        public List<DAIBIEU> Getlist(String sql)
        {
            List<DAIBIEU> resule = new List<DAIBIEU>();
            resule = base.GetList<DAIBIEU>(sql);
            return resule;
        }
      
        public DAIBIEU AddNew(DAIBIEU UserInput)
        {
            decimal IUse = 0 ;
            IUse = base.GetNextValSeq("DAIBIEU_SEQ"); 
            if ( IUse != 0 )
            {
                UserInput.IDAIBIEU = IUse;
                if (base.InsertItem<DAIBIEU>(UserInput))
                {
                    return UserInput;
                }
                else
                {
                    return null;
                }
            }
            else {
                return null;
            }
        }
        public Boolean Update(DAIBIEU UserInput)
        {

            if (base.UpdateItem<DAIBIEU>(UserInput, "IDAIBIEU"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(DAIBIEU UserInput)
        {

            if (base.DeleteItem<DAIBIEU>(UserInput, "IDAIBIEU"))
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
