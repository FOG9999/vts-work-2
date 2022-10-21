using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KNTC_VANBANMap : EntityTypeConfiguration<KNTC_VANBAN>
    {
        public KNTC_VANBANMap()
        {
            // Primary Key
            this.HasKey(t => t.IVANBAN);

            // Properties
            this.Property(t => t.IVANBAN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CSOVANBAN)
                .HasMaxLength(250);

            this.Property(t => t.CNGUOIKY)
                .HasMaxLength(250);

            this.Property(t => t.CFILE)
                .HasMaxLength(4000);

            this.Property(t => t.CLOAI)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("KNTC_VANBAN", "KIENNGHI");
            this.Property(t => t.IVANBAN).HasColumnName("IVANBAN");
            this.Property(t => t.CSOVANBAN).HasColumnName("CSOVANBAN");
            this.Property(t => t.DNGAYBANHANH).HasColumnName("DNGAYBANHANH");
            this.Property(t => t.CNGUOIKY).HasColumnName("CNGUOIKY");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.CFILE).HasColumnName("CFILE");
            this.Property(t => t.ICOQUANBANHANH).HasColumnName("ICOQUANBANHANH");
            this.Property(t => t.ICOQUANNHAN).HasColumnName("ICOQUANNHAN");
            this.Property(t => t.IDON).HasColumnName("IDON");
            this.Property(t => t.CLOAI).HasColumnName("CLOAI");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
        }
    }
}
