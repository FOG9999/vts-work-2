using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class USER_GROUP_ACTIONMap : EntityTypeConfiguration<USER_GROUP_ACTION>
    {
        public USER_GROUP_ACTIONMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("USER_GROUP_ACTION", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IGROUP).HasColumnName("IGROUP");
            this.Property(t => t.IACTION).HasColumnName("IACTION");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
        }
    }
}
