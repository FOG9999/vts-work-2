using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class QUOCHOI_KYHOPMap : EntityTypeConfiguration<QUOCHOI_KYHOP>
    {
        public QUOCHOI_KYHOPMap()
        {
            // Primary Key
            this.HasKey(t => t.IKYHOP);

            // Properties
            this.Property(t => t.IKYHOP)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);
            this.Property(t => t.CCODE)
              .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("QUOCHOI_KYHOP", "KIENNGHI");
            this.Property(t => t.IKYHOP).HasColumnName("IKYHOP");
            this.Property(t => t.IKHOA).HasColumnName("IKHOA");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.DBATDAU).HasColumnName("DBATDAU");
            this.Property(t => t.DKETTHUC).HasColumnName("DKETTHUC");
            this.Property(t => t.IMACDINH).HasColumnName("IMACDINH");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.CCODE).HasColumnName("CCODE");
            this.Property(t => t.IVITRI).HasColumnName("IVITRI");
        }
    }
}
