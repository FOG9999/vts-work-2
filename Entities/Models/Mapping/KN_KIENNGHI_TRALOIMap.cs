using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_KIENNGHI_TRALOIMap : EntityTypeConfiguration<KN_KIENNGHI_TRALOI>
    {
        public KN_KIENNGHI_TRALOIMap()
        {
            // Primary Key
            this.HasKey(t => t.ITRALOI);

            // Properties
            this.Property(t => t.ITRALOI)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CFILE)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("KN_KIENNGHI_TRALOI", "KIENNGHI");
            this.Property(t => t.ITRALOI).HasColumnName("ITRALOI");
            this.Property(t => t.IPHANLOAI).HasColumnName("IPHANLOAI");
            this.Property(t => t.IKIENNGHI).HasColumnName("IKIENNGHI");
            this.Property(t => t.CTRALOI).HasColumnName("CTRALOI");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.ITINHTRANG).HasColumnName("ITINHTRANG");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.CFILE).HasColumnName("CFILE");
            this.Property(t => t.CLOTRINH).HasColumnName("CLOTRINH");            
            this.Property(t => t.DNGAY_DUKIEN).HasColumnName("DNGAY_DUKIEN");
            this.Property(t => t.CSOVANBAN).HasColumnName("CSOVANBAN");
            this.Property(t => t.DNGAYBANHANH).HasColumnName("DNGAYBANHANH");
            this.Property(t => t.CNGUOIKY).HasColumnName("CNGUOIKY");
        }
    }
}
