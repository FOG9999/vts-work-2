using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    class KN_KIENNGHI_IMPORTMap : EntityTypeConfiguration<KN_KIENNGHI_IMPORT>
    {
        public KN_KIENNGHI_IMPORTMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            // Table & Column Mappings
            this.ToTable("KN_KIENNGHI_IMPORT", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IKYHOP).HasColumnName("IKYHOP");
            this.Property(t => t.IDONVITHAMQUYEN).HasColumnName("IDONVITHAMQUYEN");
            this.Property(t => t.IDONVITIEPNHAN).HasColumnName("IDONVITIEPNHAN");
            this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
            this.Property(t => t.ITRUOCKYHOP).HasColumnName("ITRUOCKYHOP");
            this.Property(t => t.ID_IMPORT).HasColumnName("ID_IMPORT");
            this.Property(t => t.ID_KIENNGHI).HasColumnName("ID_KIENNGHI");
            this.Property(t => t.ID_TONGHOP_BDN).HasColumnName("ID_TONGHOP_BDN");
            this.Property(t => t.ICHUONGTRINH).HasColumnName("ICHUONGTRINH");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.CSOCONGVAN).HasColumnName("CSOCONGVAN");
            this.Property(t => t.CCONGVAN).HasColumnName("CCONGVAN");
            this.Property(t => t.DNGAYBANHANH).HasColumnName("DNGAYBANHANH");

        }
    }
}
