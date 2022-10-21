using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_CHUONGTRINH_CHITIETMap : EntityTypeConfiguration<KN_CHUONGTRINH_CHITIET>
    {
        public KN_CHUONGTRINH_CHITIETMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("KN_CHUONGTRINH_DAIBIEU", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ICHUONGTRINH).HasColumnName("ICHUONGTRINH");
            this.Property(t => t.ITODAIBIEU).HasColumnName("ITODAIBIEU");
            this.Property(t => t.IDIAPHUONG).HasColumnName("IDIAPHUONG");
            this.Property(t => t.IDIAPHUONG2).HasColumnName("IDIAPHUONG2");
            this.Property(t => t.CDIACHI).HasColumnName("CDIACHI");
            this.Property(t => t.DNGAYTIEP).HasColumnName("DNGAYTIEP");
        }
    }
}
