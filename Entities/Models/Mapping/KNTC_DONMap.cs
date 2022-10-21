using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KNTC_DONMap : EntityTypeConfiguration<KNTC_DON>
    {
        public KNTC_DONMap()
        {
            // Primary Key
            this.HasKey(t => t.IDON);

            // Properties
            this.Property(t => t.IDON)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CNGUOIGUI_TEN)
                .HasMaxLength(250);

            this.Property(t => t.CNGUOIGUI_DIACHI)
                .HasMaxLength(250);

            this.Property(t => t.CNGUOIGUI_CMND)
                .HasMaxLength(250);

            this.Property(t => t.CDIACHI_VUVIEC)
                .HasMaxLength(4000);

            this.Property(t => t.CNOIDUNG)
                .HasMaxLength(1000);

            this.Property(t => t.CFILE)
                .HasMaxLength(4000);

            this.Property(t => t.CMADON)
                .HasMaxLength(4000);

            this.Property(t => t.CLUUTHEODOI_LYDO)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("KNTC_DON");
            this.Property(t => t.IDON).HasColumnName("IDON");
            this.Property(t => t.IDOITUONGGUI).HasColumnName("IDOITUONGGUI");
            this.Property(t => t.IDONTRUNG).HasColumnName("IDONTRUNG");
            this.Property(t => t.ILOAIDON).HasColumnName("ILOAIDON");
            this.Property(t => t.ITINHCHAT).HasColumnName("ITINHCHAT");
            this.Property(t => t.INGUONDON).HasColumnName("INGUONDON");
            this.Property(t => t.INOIDUNG).HasColumnName("INOIDUNG");
            this.Property(t => t.ITHAMQUYEN).HasColumnName("ITHAMQUYEN");
            this.Property(t => t.ITHULY).HasColumnName("ITHULY");
            this.Property(t => t.IDONVITHULY).HasColumnName("IDONVITHULY");
            this.Property(t => t.ITINHTRANGXULY).HasColumnName("ITINHTRANGXULY");
            this.Property(t => t.DNGAYNHAN).HasColumnName("DNGAYNHAN");
            this.Property(t => t.IDIAPHUONG_0).HasColumnName("IDIAPHUONG_0");
            this.Property(t => t.IDIAPHUONG_1).HasColumnName("IDIAPHUONG_1");
            this.Property(t => t.IDIAPHUONG_2).HasColumnName("IDIAPHUONG_2");
            this.Property(t => t.CNGUOIGUI_TEN).HasColumnName("CNGUOIGUI_TEN");
            this.Property(t => t.CNGUOIGUI_DIACHI).HasColumnName("CNGUOIGUI_DIACHI");
            this.Property(t => t.CNGUOIGUI_CMND).HasColumnName("CNGUOIGUI_CMND");
            this.Property(t => t.IDOANDONGNGUOI).HasColumnName("IDOANDONGNGUOI");
            this.Property(t => t.ISONGUOI).HasColumnName("ISONGUOI");
            this.Property(t => t.CDIACHI_VUVIEC).HasColumnName("CDIACHI_VUVIEC");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.CFILE).HasColumnName("CFILE");
            this.Property(t => t.ITINHTRANG_NOIBO).HasColumnName("ITINHTRANG_NOIBO");
            this.Property(t => t.ITINHTRANG_DONVIXULY).HasColumnName("ITINHTRANG_DONVIXULY");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.IUSER_GIAOXULY).HasColumnName("IUSER_GIAOXULY");
            this.Property(t => t.CMADON).HasColumnName("CMADON");
            this.Property(t => t.INGUOIGUI_QUOCTICH).HasColumnName("INGUOIGUI_QUOCTICH");
            this.Property(t => t.INGUOIGUI_DANTOC).HasColumnName("INGUOIGUI_DANTOC");
            this.Property(t => t.IUSER_DUOCGIAOXULY).HasColumnName("IUSER_DUOCGIAOXULY");
            this.Property(t => t.IDUDIEUKIEN).HasColumnName("IDUDIEUKIEN");
            this.Property(t => t.IDUDIEUKIEN_KETQUA).HasColumnName("IDUDIEUKIEN_KETQUA");
            this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
            this.Property(t => t.ILUUTHEODOI).HasColumnName("ILUUTHEODOI");
            this.Property(t => t.CLUUTHEODOI_LYDO).HasColumnName("CLUUTHEODOI_LYDO");
            this.Property(t => t.CHITIETLYDO_LUUTHEODOI).HasColumnName("CHITIETLYDO_LUUTHEODOI");
            this.Property(t => t.CGHICHU).HasColumnName("CGHICHU");
            this.Property(t => t.IDONVITIEPNHAN).HasColumnName("IDONVITIEPNHAN");
            this.Property(t => t.IDANHGIA).HasColumnName("IDANHGIA");
            this.Property(t => t.CGHICHUDANHGIA).HasColumnName("CGHICHUDANHGIA");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.IIDIMPORT).HasColumnName("IIDIMPORT");
            this.Property(t => t.INGAYQUYDINH).HasColumnName("INGAYQUYDINH");
            this.Property(t => t.ICANHBAO).HasColumnName("ICANHBAO");
            this.Property(t => t.ICHITIETLYDOLUUDON).HasColumnName("ICHITIETLYDOLUUDON");
            this.Property(t => t.IPLSONGUOI).HasColumnName("IPLSONGUOI");
            this.Property(t => t.CNGUOIGUI_SDT).HasColumnName("CNGUOIGUI_SDT");
        }
    }
}
