using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class TD_VUVIECMap : EntityTypeConfiguration<TD_VUVIEC>
    {
        public TD_VUVIECMap()
        {
            // Primary Key
            this.HasKey(t => t.IVUVIEC);

            // Properties
            this.Property(t => t.IVUVIEC)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CNGUOIGUI_TEN)
                .HasMaxLength(2000);

            this.Property(t => t.CNGUOIGUI_DIACHI)
                .HasMaxLength(2000);

            this.Property(t => t.CNOIDUNG)
                .HasMaxLength(2000);
            this.Property(t => t.CNOIDUNGCHIDAO)
                .HasMaxLength(2000);

            this.Property(t => t.CMADON)
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("TD_VUVIEC", "KIENNGHI");
            this.Property(t => t.IVUVIEC).HasColumnName("IVUVIEC");
            this.Property(t => t.IVUVIECTRUNG).HasColumnName("IVUVIECTRUNG");
            this.Property(t => t.ILOAIDON).HasColumnName("ILOAIDON");
            this.Property(t => t.ITINHCHAT).HasColumnName("ITINHCHAT");
            this.Property(t => t.INGUONDON).HasColumnName("INGUONDON");
            this.Property(t => t.INOIDUNG).HasColumnName("INOIDUNG");
            this.Property(t => t.IDINHKY).HasColumnName("IDINHKY");
            this.Property(t => t.IDONVITIEPNHAN).HasColumnName("IDONVITIEPNHAN");
            this.Property(t => t.ITINHTRANGXULY).HasColumnName("ITINHTRANGXULY");
            this.Property(t => t.DNGAYNHAN).HasColumnName("DNGAYNHAN");
            this.Property(t => t.IDIAPHUONG_0).HasColumnName("IDIAPHUONG_0");
            this.Property(t => t.IDIAPHUONG_1).HasColumnName("IDIAPHUONG_1");
            this.Property(t => t.CNGUOIGUI_TEN).HasColumnName("CNGUOIGUI_TEN");
            this.Property(t => t.CNGUOIGUI_DIACHI).HasColumnName("CNGUOIGUI_DIACHI");
            this.Property(t => t.IDOANDONGNGUOI).HasColumnName("IDOANDONGNGUOI");
            this.Property(t => t.ISONGUOI).HasColumnName("ISONGUOI");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.CMADON).HasColumnName("CMADON");
            this.Property(t => t.INGUOIGUI_QUOCTICH).HasColumnName("INGUOIGUI_QUOCTICH");
            this.Property(t => t.INGUOIGUI_DANTOC).HasColumnName("INGUOIGUI_DANTOC");
            this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
            this.Property(t => t.IKNTC_DON).HasColumnName("IKNTC_DON");
            this.Property(t => t.IDONVI).HasColumnName("IDONVI");
            this.Property(t => t.ITIEPDOTXUAT).HasColumnName("ITIEPDOTXUAT");
            this.Property(t => t.ILANHDAOTIEP).HasColumnName("ILANHDAOTIEP");
            this.Property(t => t.CNOIDUNGCHIDAO).HasColumnName("CNOIDUNGCHIDAO");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
        }
    }
}
