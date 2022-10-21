using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class USER_PHONGBANMap : EntityTypeConfiguration<USER_PHONGBAN>
    {
        public USER_PHONGBANMap()
        {
            // Primary Key
            this.HasKey(t => t.IPHONGBAN);

            // Properties
            this.Property(t => t.IPHONGBAN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("USER_PHONGBAN", "KIENNGHI");
            this.Property(t => t.IPHONGBAN).HasColumnName("IPHONGBAN");
            this.Property(t => t.IDONVI).HasColumnName("IDONVI");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.IPARENT).HasColumnName("IPARENT");
        }
    }
}
