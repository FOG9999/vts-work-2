using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    class KNTC_DON_IMPORTMap : EntityTypeConfiguration<KNTC_DON_IMPORT>
    {
        public KNTC_DON_IMPORTMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.CGHICHU)
                .HasMaxLength(4000);
            this.Property(t => t.CFILE)
                .HasMaxLength(4000);
            // Table & Column Mappings
            this.ToTable("KN_IMPORT", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.CFILE).HasColumnName("CFILE");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.CGHICHU).HasColumnName("CGHICHU");
            this.Property(t => t.ISODON).HasColumnName("ISODON");
            this.Property(t => t.ITINHTRANG).HasColumnName("ITINHTRANG");
        }
    }
}
