using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_DOANGIAMSAT_YKIENMap : EntityTypeConfiguration<KN_DOANGIAMSAT_YKIEN>
    {
        public KN_DOANGIAMSAT_YKIENMap()
        {
            // Primary Key
            this.HasKey(t => t.IYKIEN);

            // Properties
            this.Property(t => t.IYKIEN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(2000);

            this.Property(t => t.CNOIDUNG)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("KN_DOANGIAMSAT_YKIEN", "KIENNGHI");
            this.Property(t => t.IYKIEN).HasColumnName("IYKIEN");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IDOANGIAMSAT).HasColumnName("IDOANGIAMSAT");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.IKIENNGHI).HasColumnName("IKIENNGHI");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.DNGAYLAMVIEC).HasColumnName("DNGAYLAMVIEC");
        }
    }
}
