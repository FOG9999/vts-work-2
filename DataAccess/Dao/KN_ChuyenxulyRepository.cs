using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Utilities;
namespace DataAccess.Dao
{
    public interface IUKN_KN_ChuyenxulyRepository
    {
        List<KN_CHUYENXULY> GetAll();
        KN_CHUYENXULY GetById(int id);
        KN_CHUYENXULY AddNew(KN_CHUYENXULY UserInput);
        KN_CHUYENXULY GetByITONGHOP(int id);
        Boolean Update(KN_CHUYENXULY UserInput);
        Boolean Delete(KN_CHUYENXULY UserInput);
    }
    public class KN_ChuyenxulyRepository : BaseRepository, IUKN_KN_ChuyenxulyRepository
    {
        Log log = new Log();
        public List<KN_CHUYENXULY> GetAll()
        {
            List<KN_CHUYENXULY> resule = new List<KN_CHUYENXULY>();
            resule = base.GetAll<KN_CHUYENXULY>();

            return resule;
        }

        public KN_CHUYENXULY GetById(int id)
        {
            return base.GetItem<KN_CHUYENXULY>("IKN_CHUYENXULY", id);
        }

        public KN_CHUYENXULY GetByITONGHOP(int id)
        {
            return base.GetItem<KN_CHUYENXULY>("ITONGHOP", id);
        }

        public KN_CHUYENXULY AddNew(KN_CHUYENXULY UserInput)
        {
            decimal IUse = 0;
            IUse = base.GetNextValSeq("KN_CHUYENXULY_SEQ");
            if (IUse != 0)
            {
                UserInput.IKN_CHUYENXULY = IUse;
                if (base.InsertItem<KN_CHUYENXULY>(UserInput))
                {
                    return UserInput;
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
        public Boolean Update(KN_CHUYENXULY UserInput)
        {

            if (base.UpdateItem<KN_CHUYENXULY>(UserInput, "IKN_CHUYENXULY"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Delete(KN_CHUYENXULY UserInput)
        {

            if (base.DeleteItem<KN_CHUYENXULY>(UserInput, "IKN_CHUYENXULY"))
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
