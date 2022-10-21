using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Entities.Models.Mapping;

namespace Entities.Models
{
    public partial class Context : DbContext
    {
        static Context()
        {
            Database.SetInitializer<Context>(null);
        }

        public Context()
            : base("Name=Context")
        {
        }

        public DbSet<ACTION> ACTIONs { get; set; }
        public DbSet<DAIBIEU> DAIBIEUx { get; set; }
        public DbSet<DANTOC> DANTOCs { get; set; }
        public DbSet<DIAPHUONG> DIAPHUONGs { get; set; }
        public DbSet<FILE_UPLOAD> FILE_UPLOAD { get; set; }
        public DbSet<KN_CHUONGTRINH> KN_CHUONGTRINH { get; set; }
        public DbSet<KN_CHUONGTRINH_DAIBIEU> KN_CHUONGTRINH_DAIBIEU { get; set; }
        public DbSet<KN_CHUONGTRINH_DIAPHUONG> KN_CHUONGTRINH_DIAPHUONG { get; set; }
        public DbSet<KN_GIAMSAT> KN_GIAMSAT { get; set; }
        public DbSet<KN_GIAMSAT_DANHGIA> KN_GIAMSAT_DANHGIA { get; set; }
        public DbSet<KN_GIAMSAT_PHANLOAI> KN_GIAMSAT_PHANLOAI { get; set; }
        public DbSet<KN_KIENNGHI> KN_KIENNGHI { get; set; }
        public DbSet<KN_KIENNGHI_TRALOI> KN_KIENNGHI_TRALOI { get; set; }
        public DbSet<KN_TONGHOP> KN_TONGHOP { get; set; }
        public DbSet<KN_TUKHOA> KN_TUKHOA { get; set; }
        public DbSet<KN_VANBAN> KN_VANBAN { get; set; }
        public DbSet<KNTC_DON> KNTC_DON { get; set; }
        public DbSet<KNTC_GIAMSAT> KNTC_GIAMSAT { get; set; }
        public DbSet<KNTC_LOAIDON> KNTC_LOAIDON { get; set; }
        public DbSet<KNTC_LUUTHEODOI> KNTC_LUUTHEODOI { get; set; }
        public DbSet<KNTC_NGUONDON> KNTC_NGUONDON { get; set; }
        public DbSet<KN_NGUONDON> KN_NGUONDON { get; set; }
        public DbSet<KNTC_NOIDUNGDON> KNTC_NOIDUNGDON { get; set; }
        public DbSet<KNTC_TINHCHAT> KNTC_TINHCHAT { get; set; }
        public DbSet<KNTC_VANBAN> KNTC_VANBAN { get; set; }
        public DbSet<KN_CHUYENXULY> KN_CHUYENXULY { get; set; }
        public DbSet<LINHVUC> LINHVUCs { get; set; }
        public DbSet<LOGIN_FAIL> LOGIN_FAIL { get; set; }
        public DbSet<NGHENGHIEP> NGHENGHIEPs { get; set; }
        public DbSet<QUOCHOI_COQUAN> QUOCHOI_COQUAN { get; set; }
        public DbSet<QUOCHOI_KHOA> QUOCHOI_KHOA { get; set; }
        public DbSet<QUOCHOI_KYHOP> QUOCHOI_KYHOP { get; set; }
        public DbSet<QUOCTICH> QUOCTICHes { get; set; }
        public DbSet<TD_VUVIEC> TD_VUVIEC { get; set; }
        public DbSet<TD_VUVIEC_XULY> TD_VUVIEC_XULY { get; set; }
        public DbSet<TIEPDAN_DINHKY> TIEPDAN_DINHKY { get; set; }
        public DbSet<TIEPDAN_DINHKY_LOAIVUVIEC> TIEPDAN_DINHKY_LOAIVUVIEC { get; set; }
        public DbSet<TIEPDAN_DINHKY_VUVIEC> TIEPDAN_DINHKY_VUVIEC { get; set; }
        public DbSet<TIEPDAN_THUONGXUYEN> TIEPDAN_THUONGXUYEN { get; set; }
        public DbSet<TIEPDAN_THUONGXUYEN_KETQUA> TIEPDAN_THUONGXUYEN_KETQUA { get; set; }
        public DbSet<TOKEN> TOKENs { get; set; }
        public DbSet<TRACKING> TRACKINGs { get; set; }
        public DbSet<USER_ACTION> USER_ACTION { get; set; }
        public DbSet<USER_CHUCVU> USER_CHUCVU { get; set; }
        public DbSet<USER_GROUP> USER_GROUP { get; set; }
        public DbSet<USER_GROUP_ACTION> USER_GROUP_ACTION { get; set; }
        public DbSet<USER_PHONGBAN> USER_PHONGBAN { get; set; }
        public DbSet<USER_TYPE> USER_TYPE { get; set; }
        public DbSet<USER> USERS { get; set; }
        public DbSet<VB_DONVI_VANBAN> VB_DONVI_VANBAN { get; set; }
        public DbSet<VB_FILE_VANBAN> VB_FILE_VANBAN { get; set; }
        public DbSet<VB_LOAI> VB_LOAI { get; set; }
        public DbSet<VB_VANBAN> VB_VANBAN { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ACTIONMap());
            modelBuilder.Configurations.Add(new DAIBIEUMap());
            modelBuilder.Configurations.Add(new DANTOCMap());
            modelBuilder.Configurations.Add(new DIAPHUONGMap());
            modelBuilder.Configurations.Add(new FILE_UPLOADMap());
            modelBuilder.Configurations.Add(new KN_CHUONGTRINHMap());
            modelBuilder.Configurations.Add(new KN_CHUONGTRINH_DAIBIEUMap());
            modelBuilder.Configurations.Add(new KN_CHUONGTRINH_DIAPHUONGMap());
            modelBuilder.Configurations.Add(new KN_GIAMSATMap());
            modelBuilder.Configurations.Add(new KN_GIAMSAT_DANHGIAMap());
            modelBuilder.Configurations.Add(new KN_GIAMSAT_PHANLOAIMap());
            modelBuilder.Configurations.Add(new KN_KIENNGHIMap());
            modelBuilder.Configurations.Add(new KN_KIENNGHI_TRALOIMap());
            modelBuilder.Configurations.Add(new KN_TONGHOPMap());
            modelBuilder.Configurations.Add(new KN_TUKHOAMap());
            modelBuilder.Configurations.Add(new KN_VANBANMap());
            modelBuilder.Configurations.Add(new KN_CHUYENXULYMap());
            modelBuilder.Configurations.Add(new KNTC_DONMap());
            modelBuilder.Configurations.Add(new KNTC_GIAMSATMap());
            modelBuilder.Configurations.Add(new KNTC_LOAIDONMap());
            modelBuilder.Configurations.Add(new KNTC_LUUTHEODOIMap());
            modelBuilder.Configurations.Add(new KNTC_NGUONDONMap());
            modelBuilder.Configurations.Add(new KN_NGUONDONMap());
            modelBuilder.Configurations.Add(new KNTC_NOIDUNGDONMap());
            modelBuilder.Configurations.Add(new KNTC_TINHCHATMap());
            modelBuilder.Configurations.Add(new KNTC_VANBANMap());
            modelBuilder.Configurations.Add(new LINHVUCMap());
            modelBuilder.Configurations.Add(new LOGIN_FAILMap());
            modelBuilder.Configurations.Add(new NGHENGHIEPMap());
            modelBuilder.Configurations.Add(new QUOCHOI_COQUANMap());
            modelBuilder.Configurations.Add(new QUOCHOI_KHOAMap());
            modelBuilder.Configurations.Add(new QUOCHOI_KYHOPMap());
            modelBuilder.Configurations.Add(new QUOCTICHMap());
            modelBuilder.Configurations.Add(new TD_VUVIECMap());
            modelBuilder.Configurations.Add(new TD_VUVIEC_XULYMap());
            modelBuilder.Configurations.Add(new TIEPDAN_DINHKYMap());
            modelBuilder.Configurations.Add(new TIEPDAN_DINHKY_LOAIVUVIECMap());
            modelBuilder.Configurations.Add(new TIEPDAN_DINHKY_VUVIECMap());
            modelBuilder.Configurations.Add(new TIEPDAN_THUONGXUYENMap());
            modelBuilder.Configurations.Add(new TIEPDAN_THUONGXUYEN_KETQUAMap());
            modelBuilder.Configurations.Add(new TOKENMap());
            modelBuilder.Configurations.Add(new TRACKINGMap());
            modelBuilder.Configurations.Add(new USER_ACTIONMap());
            modelBuilder.Configurations.Add(new USER_CHUCVUMap());
            modelBuilder.Configurations.Add(new USER_GROUPMap());
            modelBuilder.Configurations.Add(new USER_GROUP_ACTIONMap());
            modelBuilder.Configurations.Add(new USER_PHONGBANMap());
            modelBuilder.Configurations.Add(new USER_TYPEMap());
            modelBuilder.Configurations.Add(new USERMap());
            modelBuilder.Configurations.Add(new VB_DONVI_VANBANMap());
            modelBuilder.Configurations.Add(new VB_FILE_VANBANMap());
            modelBuilder.Configurations.Add(new VB_LOAIMap());
            modelBuilder.Configurations.Add(new VB_VANBANMap());
        }
    }
}
