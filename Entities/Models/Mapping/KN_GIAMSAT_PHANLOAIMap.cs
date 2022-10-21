using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_GIAMSAT_PHANLOAIMap : EntityTypeConfiguration<KN_GIAMSAT_PHANLOAI>
    {
        public KN_GIAMSAT_PHANLOAIMap()
        {
            // Primary Key
            this.HasKey(t => t.IPHANLOAI);

            // Properties
            this.Property(t => t.IPHANLOAI)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("KN_GIAMSAT_PHANLOAI", "KIENNGHI");
            this.Property(t => t.IPHANLOAI).HasColumnName("IPHANLOAI");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
        }
    }
}
