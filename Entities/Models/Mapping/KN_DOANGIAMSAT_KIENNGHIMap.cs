using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_DOANGIAMSAT_KIENNGHIMap : EntityTypeConfiguration<KN_DOANGIAMSAT_KIENNGHI>
    {
        public KN_DOANGIAMSAT_KIENNGHIMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("KN_DOANGIAMSAT_KIENNGHI", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IKIENNGHI).HasColumnName("IKIENNGHI");
            this.Property(t => t.IDOANGIAMSAT).HasColumnName("IDOANGIAMSAT");
        }
    }
}
