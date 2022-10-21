using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class ACTIONMap : EntityTypeConfiguration<ACTION>
    {
        public ACTIONMap()
        {
            // Primary Key
            this.HasKey(t => t.IACTION);

            // Properties
            this.Property(t => t.IACTION)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("ACTION", "KIENNGHI");
            this.Property(t => t.IACTION).HasColumnName("IACTION");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IPARENT).HasColumnName("IPARENT");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
        }
    }
}
