using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class TIEPDAN_DINHKY_VUVIECMap : EntityTypeConfiguration<TIEPDAN_DINHKY_VUVIEC>
    {
        public TIEPDAN_DINHKY_VUVIECMap()
        {
            // Primary Key
            this.HasKey(t => t.IVUVIEC);

            // Properties
            this.Property(t => t.IVUVIEC)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CNGUOITIEP)
                .HasMaxLength(250);

            this.Property(t => t.CNGUOIGUI_TEN)
                .HasMaxLength(250);

            this.Property(t => t.CNGUOIGUI_DIACHI)
                .HasMaxLength(250);

            this.Property(t => t.CNOIDUNG)
                .HasMaxLength(400);

            this.Property(t => t.CTRALOI)
                .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TIEPDAN_DINHKY_VUVIEC", "KIENNGHI");
            this.Property(t => t.IVUVIEC).HasColumnName("IVUVIEC");
            this.Property(t => t.IDINHKY).HasColumnName("IDINHKY");
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
            this.Property(t => t.CTRALOI).HasColumnName("CTRALOI");
        }
    }
}
