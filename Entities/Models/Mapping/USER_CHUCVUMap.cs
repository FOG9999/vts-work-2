using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class USER_CHUCVUMap : EntityTypeConfiguration<USER_CHUCVU>
    {
        public USER_CHUCVUMap()
        {
            // Primary Key
            this.HasKey(t => t.ICHUCVU);

            // Properties
            this.Property(t => t.ICHUCVU)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);
            this.Property(t => t.CCODE)
           .HasMaxLength(250);
            // Table & Column Mappings
            this.ToTable("USER_CHUCVU", "KIENNGHI");
            this.Property(t => t.ICHUCVU).HasColumnName("ICHUCVU");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
            this.Property(t => t.IPHONGBAN).HasColumnName("IPHONGBAN");
        }
    }
}
