using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class TRACKINGMap : EntityTypeConfiguration<TRACKING>
    {
        public TRACKINGMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CACTION)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("TRACKING", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.CACTION).HasColumnName("CACTION");
            this.Property(t => t.IDON).HasColumnName("IDON");
            this.Property(t => t.IKIENNGHI).HasColumnName("IKIENNGHI");
            this.Property(t => t.ITONGHOP).HasColumnName("ITONGHOP");
            this.Property(t => t.ITIEPDAN_DINHKY).HasColumnName("ITIEPDAN_DINHKY");
            this.Property(t => t.ITIEPDAN_THUONGXUYEN).HasColumnName("ITIEPDAN_THUONGXUYEN");
            this.Property(t => t.IVANBAN).HasColumnName("IVANBAN");
        }
    }
}
