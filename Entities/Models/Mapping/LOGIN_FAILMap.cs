using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class LOGIN_FAILMap : EntityTypeConfiguration<LOGIN_FAIL>
    {
        public LOGIN_FAILMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IP)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("LOGIN_FAIL", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IP).HasColumnName("IP");
            this.Property(t => t.IFAILED).HasColumnName("IFAILED");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
        }
    }
}
