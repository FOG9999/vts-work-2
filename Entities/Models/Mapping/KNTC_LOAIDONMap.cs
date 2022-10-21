using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KNTC_LOAIDONMap : EntityTypeConfiguration<KNTC_LOAIDON>
    {
        public KNTC_LOAIDONMap()
        {
            // Primary Key
            this.HasKey(t => t.ILOAIDON);

            // Properties
            this.Property(t => t.ILOAIDON)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("KNTC_LOAIDON", "KIENNGHI");
            this.Property(t => t.ILOAIDON).HasColumnName("ILOAIDON");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
        }
    }
}
