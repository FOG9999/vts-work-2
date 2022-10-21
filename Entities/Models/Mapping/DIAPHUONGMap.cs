using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class DIAPHUONGMap : EntityTypeConfiguration<DIAPHUONG>
    {
        public DIAPHUONGMap()
        {
            // Primary Key
            this.HasKey(t => t.IDIAPHUONG);

            // Properties
            this.Property(t => t.IDIAPHUONG)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            this.Property(t => t.CTYPE)
                .HasMaxLength(50);

            this.Property(t => t.CCODE)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DIAPHUONG", "KIENNGHI");
            this.Property(t => t.IDIAPHUONG).HasColumnName("IDIAPHUONG");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.CTYPE).HasColumnName("CTYPE");
            this.Property(t => t.IPARENT).HasColumnName("IPARENT");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
        }
    }
}
