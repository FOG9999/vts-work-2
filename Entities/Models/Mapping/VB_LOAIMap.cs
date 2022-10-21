using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class VB_LOAIMap : EntityTypeConfiguration<VB_LOAI>
    {
        public VB_LOAIMap()
        {
            // Primary Key
            this.HasKey(t => t.ILOAI);

            // Properties
            this.Property(t => t.ILOAI)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(400);

            this.Property(t => t.CCODE)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("VB_LOAI", "KIENNGHI");
            this.Property(t => t.ILOAI).HasColumnName("ILOAI");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
           
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
        }
    }
}
