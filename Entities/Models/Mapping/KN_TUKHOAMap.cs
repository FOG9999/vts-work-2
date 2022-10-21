using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_TUKHOAMap : EntityTypeConfiguration<KN_TUKHOA>
    {
        public KN_TUKHOAMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTUKHOA)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("KN_TUKHOA", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CTUKHOA).HasColumnName("CTUKHOA");
        }
    }
}
