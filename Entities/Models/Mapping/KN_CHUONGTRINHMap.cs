using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_CHUONGTRINHMap : EntityTypeConfiguration<KN_CHUONGTRINH>
    {
        public KN_CHUONGTRINHMap()
        {
            // Primary Key
            this.HasKey(t => t.ICHUONGTRINH);

            // Properties
            this.Property(t => t.ICHUONGTRINH)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CKEHOACH)
                .HasMaxLength(250);

            this.Property(t => t.CFILE)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("KN_CHUONGTRINH", "KIENNGHI");
            this.Property(t => t.ICHUONGTRINH).HasColumnName("ICHUONGTRINH");
            this.Property(t => t.IKYHOP).HasColumnName("IKYHOP");
            this.Property(t => t.IKHOA).HasColumnName("IKHOA");
            this.Property(t => t.ITRUOCKYHOP).HasColumnName("ITRUOCKYHOP");
            this.Property(t => t.DBATDAU).HasColumnName("DBATDAU");
            this.Property(t => t.DKETTHUC).HasColumnName("DKETTHUC");
            this.Property(t => t.CKEHOACH).HasColumnName("CKEHOACH");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.CDIACHI).HasColumnName("CDIACHI");
            this.Property(t => t.CFILE).HasColumnName("CFILE");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.IDONVI).HasColumnName("IDONVI");
            this.Property(t => t.IDOITUONG).HasColumnName("IDOITUONG");
        }
    }
}
