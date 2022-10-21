using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class FILE_UPLOADMap : EntityTypeConfiguration<FILE_UPLOAD>
    {
        public FILE_UPLOADMap()
        {
            // Primary Key
            this.HasKey(t => t.ID_FILE);

            // Properties
            this.Property(t => t.ID_FILE)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CFILE)
                .HasMaxLength(4000);

            this.Property(t => t.CTYPE)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FILE_UPLOAD", "KIENNGHI");
            this.Property(t => t.ID_FILE).HasColumnName("ID_FILE");
            this.Property(t => t.CFILE).HasColumnName("CFILE");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CTYPE).HasColumnName("CTYPE");
        }
    }
}
