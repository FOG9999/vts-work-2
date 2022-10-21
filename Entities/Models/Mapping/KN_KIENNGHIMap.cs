using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_KIENNGHIMap : EntityTypeConfiguration<KN_KIENNGHI>
    {
        public KN_KIENNGHIMap()
        {
            // Primary Key
            this.HasKey(t => t.IKIENNGHI);

            // Properties
            this.Property(t => t.IKIENNGHI)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CNOIDUNG)
                .HasMaxLength(250);

            this.Property(t => t.CTUKHOA)
                .HasMaxLength(4000);

            this.Property(t => t.CMAKIENNGHI)
                .HasMaxLength(4000);

            this.Property(t => t.CFILE)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("KN_KIENNGHI", "KIENNGHI");
            this.Property(t => t.IKIENNGHI).HasColumnName("IKIENNGHI");
            this.Property(t => t.IKYHOP).HasColumnName("IKYHOP");
            this.Property(t => t.IDONVITIEPNHAN).HasColumnName("IDONVITIEPNHAN");
            this.Property(t => t.ITHAMQUYENDONVI).HasColumnName("ITHAMQUYENDONVI");
            this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
            this.Property(t => t.ICHUONGTRINH).HasColumnName("ICHUONGTRINH");
            this.Property(t => t.INGUONKIENNGHI).HasColumnName("INGUONKIENNGHI");
            this.Property(t => t.IDIAPHUONG0).HasColumnName("IDIAPHUONG0");
            this.Property(t => t.IDIAPHUONG1).HasColumnName("IDIAPHUONG1");
            this.Property(t => t.ITRUOCKYHOP).HasColumnName("ITRUOCKYHOP");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.CDIACHI).HasColumnName("CDIACHI");
            this.Property(t => t.CTUKHOA).HasColumnName("CTUKHOA");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.IKIEMTRATRUNG).HasColumnName("IKIEMTRATRUNG");
            this.Property(t => t.IKIENNGHI_TRUNG).HasColumnName("IKIENNGHI_TRUNG");
            this.Property(t => t.ITINHTRANG).HasColumnName("ITINHTRANG");
            this.Property(t => t.ITONGHOP).HasColumnName("ITONGHOP");
            this.Property(t => t.ITONGHOP_BDN).HasColumnName("ITONGHOP_BDN");
            this.Property(t => t.CMAKIENNGHI).HasColumnName("CMAKIENNGHI");
            this.Property(t => t.CFILE).HasColumnName("CFILE");
            this.Property(t => t.ID_KIENNGHI_PARENT).HasColumnName("ID_KIENNGHI_PARENT");
            this.Property(t => t.IPARENT).HasColumnName("IPARENT");
            this.Property(t => t.ID_GOP).HasColumnName("ID_GOP");
            this.Property(t => t.CNOIDUNG_TRUNG).HasColumnName("CNOIDUNG_TRUNG");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.IDOITUONGGUI).HasColumnName("IDOITUONGGUI");
            this.Property(t => t.INGAYQUYDINH).HasColumnName("INGAYQUYDINH");
            this.Property(t => t.ICANHBAO).HasColumnName("ICANHBAO");
        }
    }
}
