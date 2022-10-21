using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class QUOCHOI_COQUANMap : EntityTypeConfiguration<QUOCHOI_COQUAN>
    {
        public QUOCHOI_COQUANMap()
        {
            // Primary Key
            this.HasKey(t => t.ICOQUAN);

            // Properties
            this.Property(t => t.ICOQUAN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            this.Property(t => t.CCODE)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("QUOCHOI_COQUAN", "KIENNGHI");
            this.Property(t => t.ICOQUAN).HasColumnName("ICOQUAN");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IPARENT).HasColumnName("IPARENT");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.DKETTHUC).HasColumnName("DKETTHUC");
            this.Property(t => t.IMACDINH).HasColumnName("IMACDINH");
            this.Property(t => t.IDIAPHUONG).HasColumnName("IDIAPHUONG");
            this.Property(t => t.IGROUP).HasColumnName("IGROUP");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
            this.Property(t => t.IUSE).HasColumnName("IUSE");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.CTYPE).HasColumnName("CTYPE");
        }
    }
}
