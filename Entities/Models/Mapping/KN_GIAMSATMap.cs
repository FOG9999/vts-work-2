using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_GIAMSATMap : EntityTypeConfiguration<KN_GIAMSAT>
    {
        public KN_GIAMSATMap()
        {
            // Primary Key
            this.HasKey(t => t.IGIAMSAT);

            // Properties
            this.Property(t => t.IGIAMSAT)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("KN_GIAMSAT", "KIENNGHI");
            this.Property(t => t.IGIAMSAT).HasColumnName("IGIAMSAT");
            this.Property(t => t.IKIENNGHI).HasColumnName("IKIENNGHI");
            this.Property(t => t.IPHANLOAI).HasColumnName("IPHANLOAI");
            this.Property(t => t.IDANHGIA).HasColumnName("IDANHGIA");
            this.Property(t => t.IDUNGTIENDO).HasColumnName("IDUNGTIENDO");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.IDONGKIENNGHI).HasColumnName("IDONGKIENNGHI");
            this.Property(t => t.ITRALOI).HasColumnName("ITRALOI");
        }
    }
}
