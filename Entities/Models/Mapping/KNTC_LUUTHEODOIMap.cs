using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KNTC_LUUTHEODOIMap : EntityTypeConfiguration<KNTC_LUUTHEODOI>
    {
        public KNTC_LUUTHEODOIMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("KNTC_LUUTHEODOI", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
        }
    }
}
