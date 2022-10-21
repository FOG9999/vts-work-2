using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KNTC_GIAMSATMap : EntityTypeConfiguration<KNTC_GIAMSAT>
    {
        public KNTC_GIAMSATMap()
        {
            // Primary Key
            this.HasKey(t => t.IGIAMSAT);

            // Properties
            this.Property(t => t.IGIAMSAT)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CKEHOACH)
                .HasMaxLength(250);

            this.Property(t => t.CCHUYENDE)
                .HasMaxLength(250);

            this.Property(t => t.CFILE)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("KNTC_GIAMSAT", "KIENNGHI");
            this.Property(t => t.IGIAMSAT).HasColumnName("IGIAMSAT");
            this.Property(t => t.IDON).HasColumnName("IDON");
            this.Property(t => t.IDONVI).HasColumnName("IDONVI");
            this.Property(t => t.CKEHOACH).HasColumnName("CKEHOACH");
            this.Property(t => t.CCHUYENDE).HasColumnName("CCHUYENDE");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.CFILE).HasColumnName("CFILE");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
        }
    }
}
