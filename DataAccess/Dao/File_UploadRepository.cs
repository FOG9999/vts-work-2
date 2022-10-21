using Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Dao
{
  
    public interface IUFileuploadRepository
    {
        FILE_UPLOAD GetByID(int ID);
        List<FILE_UPLOAD> GetAll();
        List<FILE_UPLOAD> GetAll(Dictionary<string, object> condition);
        FILE_UPLOAD AddNew(FILE_UPLOAD fileuploadInput);
        Boolean Update(FILE_UPLOAD fileuploadInput);
        Boolean Delete(FILE_UPLOAD fileuploadInput);
      


    }
    public class FileuploadRepository : BaseRepository, IUFileuploadRepository
    {
       
       

        public List<FILE_UPLOAD> GetAll()
        {
            List<FILE_UPLOAD> resule = new List<FILE_UPLOAD>();
            resule = base.GetAll<FILE_UPLOAD>();
            return resule;
        }
        public List<FILE_UPLOAD> GetAll(Dictionary<string, object> condition)
        {
            List<FILE_UPLOAD> resule = new List<FILE_UPLOAD>();
            resule = base.GetAll<FILE_UPLOAD>(condition);
            return resule;
        }
        public FILE_UPLOAD GetByID(int ID)
        {
            return base.GetItem<FILE_UPLOAD>("ID_FILE", ID);
        }  
        public FILE_UPLOAD AddNew(FILE_UPLOAD Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("FILE_UPLOAD_SEQ");
            if (ID != 0)
            {
                Input.ID_FILE = ID;
                if (base.InsertItem<FILE_UPLOAD>(Input))
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
        public Boolean Update(FILE_UPLOAD fileuploadInput)
        {

            if (base.UpdateItem<FILE_UPLOAD>(fileuploadInput, "ID_FILE"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Delete(FILE_UPLOAD fileuploadInput)
        {

            if (base.DeleteItem<FILE_UPLOAD>(fileuploadInput, "ID_FILE"))
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
