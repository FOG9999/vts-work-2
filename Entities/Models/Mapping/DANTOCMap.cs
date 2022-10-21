using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class DANTOCMap : EntityTypeConfiguration<DANTOC>
    {
        public DANTOCMap()
        {
            // Primary Key
            this.HasKey(t => t.IDANTOC);

            // Properties
            this.Property(t => t.IDANTOC)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);
            this.Property(t => t.CCODE)
            .HasMaxLength(250);
            // Table & Column Mappings
            this.ToTable("DANTOC", "KIENNGHI");
            this.Property(t => t.IDANTOC).HasColumnName("IDANTOC");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
        }
    }
}
