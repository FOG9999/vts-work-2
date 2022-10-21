using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dao
{
   
    public interface IUVanban_DonviRepository
    {
        VB_DONVI_VANBAN GetByID(int ID);
        List<VB_DONVI_VANBAN> GetAll();
        List<VB_DONVI_VANBAN> GetAll(Dictionary<string, object> dictionary);
        VB_DONVI_VANBAN AddNew(VB_DONVI_VANBAN UserInput);
        Boolean Update(VB_DONVI_VANBAN UserInput);
        Boolean Delete(VB_DONVI_VANBAN UserInput);
        List<VB_DONVI_VANBAN> GetList(String sql);

    }
    public class Vanban_DonviRepository : BaseRepository, IUVanban_DonviRepository
    {
        public List<VB_DONVI_VANBAN> GetAll()
        {
            List<VB_DONVI_VANBAN> resule = new List<VB_DONVI_VANBAN>();
            resule = base.GetAll<VB_DONVI_VANBAN>();

            return resule;

        }
        public List<VB_DONVI_VANBAN> GetAll(Dictionary<string, object> dictionary)
        {
            List<VB_DONVI_VANBAN> resule = new List<VB_DONVI_VANBAN>();
            resule = base.GetAll<VB_DONVI_VANBAN>(dictionary);

            return resule;
        }
        public List<VB_DONVI_VANBAN> GetList(String sql)
        {
            List<VB_DONVI_VANBAN> resule = new List<VB_DONVI_VANBAN>();
            resule = base.GetList<VB_DONVI_VANBAN>(sql);
            return resule;
        }
        public VB_DONVI_VANBAN GetByID(int ID)
        {
            return base.GetItem<VB_DONVI_VANBAN>("ID", ID);
        }
        public VB_DONVI_VANBAN AddNew(VB_DONVI_VANBAN Input)
        {
            decimal ID = 0;
            ID = base.GetNextValSeq("VB_DONVI_VANBAN_SEQ");
            if (ID != 0)
            {
                Input.ID = ID;
                if (base.InsertItem<VB_DONVI_VANBAN>(Input))
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
        public Boolean Update(VB_DONVI_VANBAN actionInput)
        {

            if (base.UpdateItem<VB_DONVI_VANBAN>(actionInput, "ID"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(VB_DONVI_VANBAN actionInput)
        {

            if (base.DeleteItem<VB_DONVI_VANBAN>(actionInput, "ID"))
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
