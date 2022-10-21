using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class USER_ACTIONMap : EntityTypeConfiguration<USER_ACTION>
    {
        public USER_ACTIONMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("USER_ACTION", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.IACTION).HasColumnName("IACTION");
        }
    }
}
