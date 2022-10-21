using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class QUOCTICHMap : EntityTypeConfiguration<QUOCTICH>
    {
        public QUOCTICHMap()
        {
            // Primary Key
            this.HasKey(t => t.IQUOCTICH);

            // Properties
            this.Property(t => t.IQUOCTICH)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("QUOCTICH", "KIENNGHI");
            this.Property(t => t.IQUOCTICH).HasColumnName("IQUOCTICH");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
        }
    }
}
