using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class VB_DONVI_VANBANMap : EntityTypeConfiguration<VB_DONVI_VANBAN>
    {
        public VB_DONVI_VANBANMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VB_DONVI_VANBAN", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IVANBAN).HasColumnName("IVANBAN");
            this.Property(t => t.IDONVI).HasColumnName("IDONVI");
        }
    }
}
