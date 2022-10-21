using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_TONGHOPMap : EntityTypeConfiguration<KN_TONGHOP>
    {
        public KN_TONGHOPMap()
        {
            // Primary Key
            this.HasKey(t => t.ITONGHOP);

            // Properties
            this.Property(t => t.ITONGHOP)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CNOIDUNG)
                .HasMaxLength(4000);

            this.Property(t => t.CTUKHOA)
                .HasMaxLength(4000);

            this.Property(t => t.CFILE)
                .HasMaxLength(4000);

            this.Property(t => t.CMATONGHOP)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("KN_TONGHOP", "KIENNGHI");
            this.Property(t => t.ITONGHOP).HasColumnName("ITONGHOP");
            this.Property(t => t.IDONVITONGHOP).HasColumnName("IDONVITONGHOP");
            this.Property(t => t.IKYHOP).HasColumnName("IKYHOP");
            this.Property(t => t.ITRUOCKYHOP).HasColumnName("ITRUOCKYHOP");
            this.Property(t => t.ICHUONGTRINH).HasColumnName("ICHUONGTRINH");
            this.Property(t => t.ITHAMQUYENDONVI).HasColumnName("ITHAMQUYENDONVI");
            this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.CTUKHOA).HasColumnName("CTUKHOA");
            this.Property(t => t.CFILE).HasColumnName("CFILE");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.ITINHTRANG).HasColumnName("ITINHTRANG");
            this.Property(t => t.CMATONGHOP).HasColumnName("CMATONGHOP");
            this.Property(t => t.ID_IMPORT).HasColumnName("ID_IMPORT");
        }
    }
}
