using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class USER_GROUPMap : EntityTypeConfiguration<USER_GROUP>
    {
        public USER_GROUPMap()
        {
            // Primary Key
            this.HasKey(t => t.IGROUP);

            // Properties
            this.Property(t => t.IGROUP)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("USER_GROUP", "KIENNGHI");
            this.Property(t => t.IGROUP).HasColumnName("IGROUP");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.CMOTA).HasColumnName("CMOTA");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
        }
    }
}
