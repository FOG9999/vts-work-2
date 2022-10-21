using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_TRALOI_PHANLOAIMap : EntityTypeConfiguration<KN_TRALOI_PHANLOAI>
    {
        public KN_TRALOI_PHANLOAIMap()
        {
            // Primary Key
            this.HasKey(t => t.IPHANLOAI);

            // Properties
            this.Property(t => t.IPHANLOAI)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("KN_TRALOI_PHANLOAI", "KIENNGHI");
            this.Property(t => t.IPHANLOAI).HasColumnName("IPHANLOAI");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.IPARENT).HasColumnName("IPARENT");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
        }
    }
}
