using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class VB_FILE_VANBANMap : EntityTypeConfiguration<VB_FILE_VANBAN>
    {
        public VB_FILE_VANBANMap()
        {
            // Primary Key
            this.HasKey(t => t.IFILE);

            // Properties
            this.Property(t => t.IFILE)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CURL)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("VB_FILE_VANBAN", "KIENNGHI");
            this.Property(t => t.IFILE).HasColumnName("IFILE");
            this.Property(t => t.IVANBAN).HasColumnName("IVANBAN");
            this.Property(t => t.CURL).HasColumnName("CURL");
        }
    }
}
