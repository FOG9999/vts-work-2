using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dao;
using Entities.Models;
namespace DataAccess.Busineess
{
    public class VanbanBusineess
    {
      
        public VanbanRepository _vanban = new VanbanRepository();

        public Vanban_FileRepository _vanban_file = new Vanban_FileRepository();

        public FileuploadRepository _file = new FileuploadRepository();

        public Vanban_DonviRepository _vanban_donvi = new Vanban_DonviRepository();
        public LoaivanbanRepository _loaivanban = new LoaivanbanRepository();

        public List<LINHVUC_COQUAN> GetAll_LinhVuc_CoQuan_By_IDCoQuan(int id = 0)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (id != 0)
            {
                dic.Add("ICOQUAN", id);
            }
            dic.Add("IHIENTHI", 1);
            LinhvuccoquanRepository _linhvuc = new LinhvuccoquanRepository();
            return _linhvuc.GetAll(dic);
        }
       
        public List<VB_VANBAN> GetList_SQLTimkiemten(string cten)
        {
            return  _vanban.GetList_vanban_search_ten(cten);
        }
        // danh muc loai van ban
        public VB_LOAI Insert_Loaivanban(VB_LOAI Input)
        {
            return _loaivanban.AddNew(Input);
        }
        public List<VB_LOAI> Get_Loaivanban()
        {
            return _loaivanban.GetAll();
        }
        public List<VB_LOAI> Get_ListVB()
        {
            return _loaivanban.GetAll();
        }

        public VB_LOAI GetBy_Loaivanban(int iD)
        {
            return _loaivanban.GetByID(iD);
        }
       

        public Boolean Update_Loaivanban(VB_LOAI Input)
        {
            return _loaivanban.Update(Input);
        }

        public Boolean Delete_Loaivanban(VB_LOAI Input)
        {
            return _loaivanban.Delete(Input);
        }
        // end 
        // Danh mục văn bản đơn vị
        public VB_DONVI_VANBAN Insert_Vanban_donvi(VB_DONVI_VANBAN Input)
        {
            return _vanban_donvi.AddNew(Input);
        }
        public List<VB_DONVI_VANBAN> Get_Vanban_donvi()
        {
            return _vanban_donvi.GetAll();
        }
        public List<VB_DONVI_VANBAN> Get_List_Vanban_donvi(String sql)
        {
            return _vanban_donvi.GetList(sql);
        }

        public VB_DONVI_VANBAN GetBy_Vanban_donvi(int iD)
        {
            return _vanban_donvi.GetByID(iD);
        }
        public List<VB_DONVI_VANBAN> GetBy_List_Vanban_donvi(Dictionary<string, object> condition)
        {
            return _vanban_donvi.GetAll(condition);
        }

        public Boolean Update_Vanban_donvi(VB_DONVI_VANBAN Input)
        {
            return _vanban_donvi.Update(Input);
        }

        public Boolean Delete_Vanban_donvi(VB_DONVI_VANBAN Input)
        {
            return _vanban_donvi.Delete(Input);
        }
        // End danh mục văn bản đơn vị


        // Danh mục văn bản file
        public VB_FILE_VANBAN Insert_Vanban_file(VB_FILE_VANBAN Input)
        {
            return _vanban_file.AddNew(Input);
        }
        public List<VB_FILE_VANBAN> Get_Vanban_file()
        {
            return _vanban_file.GetAll();
        }
        public List<VB_FILE_VANBAN> Get_List_Vanban_file(String sql)
        {
            return _vanban_file.GetList(sql);
        }
       
        public VB_FILE_VANBAN GetBy_Vanban_fileID(int iD)
        {
            return _vanban_file.GetByID(iD);
        }
        public List<VB_FILE_VANBAN> GetBy_List_Vanban_file(Dictionary<string, object> condition)
        {
            return _vanban_file.GetAll(condition);
        }

        public Boolean Update_Vanban_file(VB_FILE_VANBAN Input)
        {
            return _vanban_file.Update(Input);
        }

        public Boolean Delete_Vanban_file(VB_FILE_VANBAN Input)
        {
            return _vanban_file.Delete(Input);
        }
        // end danh mục văn ban file



        // danh mục văn bản
        public VB_VANBAN Insert_Vanban(VB_VANBAN Input)
        {
            return _vanban.AddNew(Input);
        }
        public List<VB_VANBAN> Get_Vanban()
        {
            return _vanban.GetAll();
        }
        public List<VB_VANBAN> Get_List_Vanban(String sql)
        {
            return _vanban.GetList(sql);
        }
        public List<VB_VANBAN> Get_List_Vanban_Sql(VB_VANBAN d, int idonvi, string tungay, string denngay)
        {
            return _vanban.GetList_vanban_search(d, idonvi, tungay, denngay);
        }
        public VB_VANBAN GetBy_VanbanID(int iD)
        {
            return _vanban.GetByID(iD);
        }
        public List<VB_VANBAN> GetBy_List_Vanban(Dictionary<string, object> condition)
        {
            return _vanban.GetAll(condition);
        }

        public Boolean Update_Vanban(VB_VANBAN Input)
        {
            return _vanban.Update(Input);
        }

        public Boolean Delete_Vanban(VB_VANBAN Input)
        {
            return _vanban.Delete(Input);
        }
        // end danh mục văn bản
    }
}
