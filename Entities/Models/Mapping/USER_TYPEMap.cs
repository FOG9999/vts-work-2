using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class USER_TYPEMap : EntityTypeConfiguration<USER_TYPE>
    {
        public USER_TYPEMap()
        {
            // Primary Key
            this.HasKey(t => t.ITYPE);

            // Properties
            this.Property(t => t.ITYPE)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CNAME)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("USER_TYPE", "KIENNGHI");
            this.Property(t => t.ITYPE).HasColumnName("ITYPE");
            this.Property(t => t.CNAME).HasColumnName("CNAME");
        }
    }
}
