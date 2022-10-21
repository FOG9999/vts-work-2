using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class TD_VUVIEC_XULYMap : EntityTypeConfiguration<TD_VUVIEC_XULY>
    {
        public TD_VUVIEC_XULYMap()
        {
            // Primary Key
            this.HasKey(t => t.IXULY);

            // Properties
            this.Property(t => t.IXULY)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CSOVANBAN)
                .HasMaxLength(2000);

            this.Property(t => t.CNGUOIXULY)
                .HasMaxLength(2000);

            this.Property(t => t.CNOIDUNG)
                .HasMaxLength(3000);

            this.Property(t => t.CLOAI)
                .HasMaxLength(2000);
            this.Property(t => t.CKINHGUI)
                .HasMaxLength(2000);
            this.Property(t => t.CNOINHAN)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("TD_VUVIEC_XULY", "KIENNGHI");
            this.Property(t => t.IXULY).HasColumnName("IXULY");
            this.Property(t => t.CSOVANBAN).HasColumnName("CSOVANBAN");
            this.Property(t => t.DNGAYXULY).HasColumnName("DNGAYXULY");
            this.Property(t => t.CNGUOIXULY).HasColumnName("CNGUOIXULY");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.ICOQUANBANHANH).HasColumnName("ICOQUANBANHANH");
            this.Property(t => t.ICOQUANNHAN).HasColumnName("ICOQUANNHAN");
            this.Property(t => t.IVUVIEC).HasColumnName("IVUVIEC");
            this.Property(t => t.CLOAI).HasColumnName("CLOAI");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DNGAYLUU).HasColumnName("DNGAYLUU");
            this.Property(t => t.CNOINHAN).HasColumnName("CNOINHAN");
            this.Property(t => t.CKINHGUI).HasColumnName("CKINHGUI");

        }
    }
}
