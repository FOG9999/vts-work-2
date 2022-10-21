using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class LINHVUCMap : EntityTypeConfiguration<LINHVUC>
    {
        public LINHVUCMap()
        {
            // Primary Key
            this.HasKey(t => t.ILINHVUC);

            // Properties
            this.Property(t => t.ILINHVUC)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            this.Property(t => t.CCODE)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("LINHVUC", "KIENNGHI");
            this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
            this.Property(t => t.INHOM).HasColumnName("INHOM");
            this.Property(t => t.IPARENT).HasColumnName("IPARENT");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
        }
    }
}
