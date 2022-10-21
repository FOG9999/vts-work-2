using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class TIEPDAN_DINHKY_LOAIVUVIECMap : EntityTypeConfiguration<TIEPDAN_DINHKY_LOAIVUVIEC>
    {
        public TIEPDAN_DINHKY_LOAIVUVIECMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TIEPDAN_DINHKY_LOAIVUVIEC", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IDINHKY).HasColumnName("IDINHKY");
            this.Property(t => t.ILOAIDON).HasColumnName("ILOAIDON");
            this.Property(t => t.IVALUE).HasColumnName("IVALUE");
        }
    }
}
