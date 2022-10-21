using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KNTC_NOIDUNGDONMap : EntityTypeConfiguration<KNTC_NOIDUNGDON>
    {
        public KNTC_NOIDUNGDONMap()
        {
            // Primary Key
            this.HasKey(t => t.INOIDUNG);

            // Properties
            this.Property(t => t.INOIDUNG)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);
            this.Property(t => t.CCODE)
               .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("KNTC_NOIDUNGDON", "KIENNGHI");
            this.Property(t => t.INOIDUNG).HasColumnName("INOIDUNG");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
        }
    }
}
