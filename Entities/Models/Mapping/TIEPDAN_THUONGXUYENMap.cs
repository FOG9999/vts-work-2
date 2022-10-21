using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class TIEPDAN_THUONGXUYENMap : EntityTypeConfiguration<TIEPDAN_THUONGXUYEN>
    {
        public TIEPDAN_THUONGXUYENMap()
        {
            // Primary Key
            this.HasKey(t => t.ITHUONGXUYEN);

            // Properties
            this.Property(t => t.ITHUONGXUYEN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CDIADIEM)
                .HasMaxLength(4000);

            this.Property(t => t.CNGUOITIEP)
                .HasMaxLength(250);

            this.Property(t => t.CNGUOIGUI_TEN)
                .HasMaxLength(250);

            this.Property(t => t.CNGUOIGUI_DIACHI)
                .HasMaxLength(4000);

            this.Property(t => t.CNOIDUNG)
                .HasMaxLength(4000);

            this.Property(t => t.CCODE)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TIEPDAN_THUONGXUYEN", "KIENNGHI");
            this.Property(t => t.ITHUONGXUYEN).HasColumnName("ITHUONGXUYEN");
            this.Property(t => t.DNGAYTIEP).HasColumnName("DNGAYTIEP");
            this.Property(t => t.CDIADIEM).HasColumnName("CDIADIEM");
            this.Property(t => t.ICOQUANTIEPDAN).HasColumnName("ICOQUANTIEPDAN");
            this.Property(t => t.CNGUOITIEP).HasColumnName("CNGUOITIEP");
            this.Property(t => t.CNGUOIGUI_TEN).HasColumnName("CNGUOIGUI_TEN");
            this.Property(t => t.CNGUOIGUI_DIACHI).HasColumnName("CNGUOIGUI_DIACHI");
            this.Property(t => t.IDOAN).HasColumnName("IDOAN");
            this.Property(t => t.IDOAN_NGUOI).HasColumnName("IDOAN_NGUOI");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.ILOAI).HasColumnName("ILOAI");
            this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
            this.Property(t => t.INOIDUNG).HasColumnName("INOIDUNG");
            this.Property(t => t.ITINHCHAT).HasColumnName("ITINHCHAT");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.ITHUONGXUYEN_TRUNG).HasColumnName("ITHUONGXUYEN_TRUNG");
        }
    }
}
