using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_DOANGIAMSATMap : EntityTypeConfiguration<KN_DOANGIAMSAT>
    {
        public KN_DOANGIAMSATMap()
        {
            // Primary Key
            this.HasKey(t => t.IDOAN);

            // Properties
            this.Property(t => t.IDOAN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(2000);

            this.Property(t => t.CNOIDUNG)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("KN_DOANGIAMSAT", "KIENNGHI");
            this.Property(t => t.IDOAN).HasColumnName("IDOAN");
            this.Property(t => t.IDONVI).HasColumnName("IDONVI");
            this.Property(t => t.IKYHOP).HasColumnName("IKYHOP");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.DNGAYBATDAU).HasColumnName("DNGAYBATDAU");
        }
    }
}
