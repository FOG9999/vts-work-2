using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class TIEPDAN_DINHKYMap : EntityTypeConfiguration<TIEPDAN_DINHKY>
    {
        public TIEPDAN_DINHKYMap()
        {
            // Primary Key
            this.HasKey(t => t.IDINHKY);

            // Properties
            this.Property(t => t.IDINHKY)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TIEPDAN_DINHKY", "KIENNGHI");
            this.Property(t => t.IDINHKY).HasColumnName("IDINHKY");
            this.Property(t => t.ILUOT).HasColumnName("ILUOT");
            this.Property(t => t.IVUVIEC).HasColumnName("IVUVIEC");
            this.Property(t => t.IDOAN).HasColumnName("IDOAN");
            this.Property(t => t.DNGAYTIEP).HasColumnName("DNGAYTIEP");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
        }
    }
}
