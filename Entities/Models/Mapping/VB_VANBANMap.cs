using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class VB_VANBANMap : EntityTypeConfiguration<VB_VANBAN>
    {
        public VB_VANBANMap()
        {
            // Primary Key
            this.HasKey(t => t.IVANBAN);

            // Properties
            this.Property(t => t.IVANBAN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTIEUDE)
                .HasMaxLength(250);

            this.Property(t => t.CTRICHYEU)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("VB_VANBAN", "KIENNGHI");
            this.Property(t => t.IVANBAN).HasColumnName("IVANBAN");
            this.Property(t => t.CTIEUDE).HasColumnName("CTIEUDE");
            this.Property(t => t.CTRICHYEU).HasColumnName("CTRICHYEU");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
            this.Property(t => t.ILOAI).HasColumnName("ILOAI");
            this.Property(t => t.DDATE).HasColumnName("DDATE");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.DDATECREATE).HasColumnName("DDATECREATE");
            this.Property(t => t.IUSERDUYET).HasColumnName("IUSERDUYET");
        }
    }
}
