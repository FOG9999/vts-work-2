using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class TOKENMap : EntityTypeConfiguration<TOKEN>
    {
        public TOKENMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CONTROLLER)
                .HasMaxLength(50);

            this.Property(t => t.TOKENACTION)
                .HasMaxLength(500);

            this.Property(t => t.CSALT)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("TOKEN", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CONTROLLER).HasColumnName("CONTROLLER");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DTIME).HasColumnName("DTIME");
            this.Property(t => t.TOKENACTION).HasColumnName("TOKENACTION");
            this.Property(t => t.CSALT).HasColumnName("CSALT");
        }
    }
}
