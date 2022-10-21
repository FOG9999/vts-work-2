using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_CHUONGTRINH_DIAPHUONGMap : EntityTypeConfiguration<KN_CHUONGTRINH_DIAPHUONG>
    {
        public KN_CHUONGTRINH_DIAPHUONGMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("KN_CHUONGTRINH_DIAPHUONG", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IDIAPHUONG0).HasColumnName("IDIAPHUONG0");
            this.Property(t => t.IDIAPHUONG1).HasColumnName("IDIAPHUONG1");
            this.Property(t => t.ICHUONGTRINH).HasColumnName("ICHUONGTRINH");
        }
    }
}
