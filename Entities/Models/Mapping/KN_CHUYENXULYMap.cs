using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KN_CHUYENXULYMap : EntityTypeConfiguration<KN_CHUYENXULY>
    {
        public KN_CHUYENXULYMap()
        {
            // Primary Key
            this.HasKey(t => t.IKN_CHUYENXULY);

            // Properties
            this.Property(t => t.IKN_CHUYENXULY)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CNOIDUNG)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("KN_CHUYENXULY", "KIENNGHI");
            this.Property(t => t.IKN_CHUYENXULY).HasColumnName("IKN_CHUYENXULY");
            this.Property(t => t.ITONGHOP).HasColumnName("ITONGHOP");
            this.Property(t => t.INGUOICHUYEN).HasColumnName("INGUOICHUYEN");
            this.Property(t => t.INGUOINHAN).HasColumnName("INGUOINHAN");
            this.Property(t => t.IDONVI).HasColumnName("IDONVI");
            this.Property(t => t.CNOIDUNG).HasColumnName("CNOIDUNG");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
        }
    }
}
