using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Objects
{
    public class THIETLAP_NGUOIDUNG
    {
        public decimal ID_USER { get; set; }
        public string USERNAME { get; set; }
        public string USER_TEN { get; set; }
        public string USER_EMAIL { get; set; }
        public string USER_SDT { get; set; }
        public decimal USER_STATUS { get; set; }
        public string USER_TENDONVI { get; set; }
        public decimal VITRI_DONVI { get; set; }
        public decimal USER_ID_DONVI { get; set; }
        public string USER_TENPHONGBAN { get; set; }
        public decimal USER_ID_PHONGBAN { get; set; }
        public string USER_TENCHUCVU { get; set; }
        public string USER_NHOM { get; set; }
        public string TENNHOMQUYEN { get; set; }
        public decimal IDNHOMQUYEN { get; set; }
        public decimal TOTAL { get; set; }
    }
    public class THIETLAP_COQUAN
    {
        public decimal IDCOQUAN { get; set; }
        public string CTENCOQUAN { get; set; }
        public decimal IDPARENT { get; set; }
        public string CMACOQUAN { get; set; }
        public decimal IDGROUP { get; set; }
        public decimal IDVITRI { get; set; }
        public decimal IDUSER { get; set; }
        public decimal IDDELETE { get; set; }
        public string CTENDIAPHUONG { get; set; }
        public decimal TOTAL { get; set; }

    }
    public class THIETLAP_DIAPHUONG_PHANTRANG
    {
        public decimal IDIAPHUONG { get; set; }
        public string CTENDIAPHUONG { get; set; }
        public decimal IDPARENT { get; set; }
        public string CMADIAPHUONG { get; set; }
        public string TYPE_ { get; set; }
        public decimal IDHIENTHI { get; set; }
        public decimal IDDELETE { get; set; }

        public decimal TOTAL { get; set; }



        public decimal IDIAPHUONG_PARENT { get; set; }
        public string CTENDIAPHUONG_PARENT { get; set; }
        public decimal IDPARENT_PARENT { get; set; }
        public string CMADIAPHUONG_PARENT { get; set; }
        public string TYPE_PARENT { get; set; }
        public decimal IDHIENTHI_PARENT { get; set; }
        public decimal IDDELETE_PARENT { get; set; }


    }
    public class THIETLAP_COQUAN_PHANTRANG
    {
        public decimal ICOQUAN { get; set; }
        public string CTEN { get; set; }
        public Nullable<decimal> IPARENT { get; set; }
        public string CCODE { get; set; }
        public Nullable<System.DateTime> DKETTHUC { get; set; }
        public Nullable<decimal> IMACDINH { get; set; }
        public Nullable<decimal> IDIAPHUONG { get; set; }
        public decimal IGROUP { get; set; }
        public decimal IVITRI { get; set; }
        public decimal IUSE { get; set; }
        public decimal IHIENTHI { get; set; }
        public Nullable<decimal> IDELETE { get; set; }
        public decimal TOTAL { get; set; }



        

    }

    public class THIETLAP_DAIBIEU_PHANTRANG
    {
        public decimal IDAIBIEU { get; set; }
        public Nullable<decimal> ITRUONGDOAN { get; set; }
        public string CTEN { get; set; }
        public Nullable<decimal> IDIAPHUONG { get; set; }
        public string CEMAIL { get; set; }
        public string CSDT { get; set; }
        public Nullable<decimal> IHIENTHI { get; set; }
        public decimal IDELETE { get; set; }
        public string CCODE { get; set; }
        public string CDONVIBAUCUSO { get; set; }
        public decimal IVITRI { get; set; }
        public decimal IGIOITINH { get; set; }
        public Nullable<System.DateTime> DNGAYSINH { get; set; }
        public string CDOANDB { get; set; }
        public string CNOILAMVIEC { get; set; }
        public string CCHUCVUDAYDU { get; set; }
        public string CCOQUAN { get; set; }
        public decimal ILOAIDAIBIEU { get; set; }
        public decimal TOTAL { get; set; }
    }
    public class THIETLAP_LICHSU
    {
       
        public DateTime THOIGIAN { get; set; }
        public string USERNAME { get; set; }
        public string TEN { get; set; }
        public string NOIDUNG { get; set; }
        public decimal TOTAL { get; set; }
    }



    
}
