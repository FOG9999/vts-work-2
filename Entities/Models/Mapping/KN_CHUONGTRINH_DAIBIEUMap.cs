using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_CHUONGTRINH_DAIBIEUMap : EntityTypeConfiguration<KN_CHUONGTRINH_DAIBIEU>
    {
        public KN_CHUONGTRINH_DAIBIEUMap()
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
            this.Property(t => t.IUSER_DAIBIEU).HasColumnName("IUSER_DAIBIEU");
            this.Property(t => t.IUSER_COQUAN).HasColumnName("IUSER_COQUAN");
        }
    }
}
