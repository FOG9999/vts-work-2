using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class LINHVUC_COQUANMap : EntityTypeConfiguration<LINHVUC_COQUAN>
    {
        public LINHVUC_COQUANMap()
        {
            // Primary Key
            this.HasKey(t => t.ILINHVUC);

            // Properties
            this.Property(t => t.ILINHVUC)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(2000);
            this.Property(t => t.CCODE)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("LINHVUC_COQUAN", "KIENNGHI");
            this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
            this.Property(t => t.ICOQUAN).HasColumnName("ICOQUAN");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
            this.Property(t => t.IPARENT).HasColumnName("IPARENT");
        }
    }
}
