using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class DAIBIEUMap : EntityTypeConfiguration<DAIBIEU>
    {
        public DAIBIEUMap()
        {
            // Primary Key
            this.HasKey(t => t.IDAIBIEU);

            // Properties
            this.Property(t => t.IDAIBIEU)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(350);

            this.Property(t => t.CEMAIL)
                .HasMaxLength(530);

            this.Property(t => t.CSDT)
                .HasMaxLength(20);
            this.Property(t => t.CCODE)
              .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("DAIBIEU", "KIENNGHI");
            this.Property(t => t.IDAIBIEU).HasColumnName("IDAIBIEU");
            this.Property(t => t.ITRUONGDOAN).HasColumnName("ITRUONGDOAN");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IDIAPHUONG).HasColumnName("IDIAPHUONG");
            this.Property(t => t.CEMAIL).HasColumnName("CEMAIL");
            this.Property(t => t.CSDT).HasColumnName("CSDT");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.CDONVIBAUCUSO).HasColumnName("CDONVIBAUCUSO");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
            this.Property(t => t.IGIOITINH).HasColumnName("IGIOITINH");
            this.Property(t => t.CCHUCVUDAYDU).HasColumnName("CCHUCVUDAYDU");
            this.Property(t => t.CCOQUAN).HasColumnName("CCOQUAN");
            this.Property(t => t.DNGAYSINH).HasColumnName("DNGAYSINH");
            this.Property(t => t.CDOANDB).HasColumnName("CDOANDB");
            this.Property(t => t.CNOILAMVIEC).HasColumnName("CNOILAMVIEC");
            this.Property(t => t.ILOAIDAIBIEU).HasColumnName("ILOAIDAIBIEU");
            this.Property(t => t.ITOTRUONG).HasColumnName("ITOTRUONG");
        }
    }
}
