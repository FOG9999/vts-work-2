using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_GIAMSAT_DANHGIAMap : EntityTypeConfiguration<KN_GIAMSAT_DANHGIA>
    {
        public KN_GIAMSAT_DANHGIAMap()
        {
            // Primary Key
            this.HasKey(t => t.IDANHGIA);

            // Properties
            this.Property(t => t.IDANHGIA)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("KN_GIAMSAT_DANHGIA", "KIENNGHI");
            this.Property(t => t.IDANHGIA).HasColumnName("IDANHGIA");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
        }
    }
}
