using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KNTC_NGUONDONMap : EntityTypeConfiguration<KNTC_NGUONDON>
    {
        public KNTC_NGUONDONMap()
        {
            // Primary Key
            this.HasKey(t => t.INGUONDON);

            // Properties
            this.Property(t => t.INGUONDON)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("KNTC_NGUONDON", "KIENNGHI");
            this.Property(t => t.INGUONDON).HasColumnName("INGUONDON");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
        }
    }
}
