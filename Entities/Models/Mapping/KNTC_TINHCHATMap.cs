using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class KNTC_TINHCHATMap : EntityTypeConfiguration<KNTC_TINHCHAT>
    {
        public KNTC_TINHCHATMap()
        {
            // Primary Key
            this.HasKey(t => t.ITINHCHAT);

            // Properties
            this.Property(t => t.ITINHCHAT)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("KNTC_TINHCHAT", "KIENNGHI");
            this.Property(t => t.ITINHCHAT).HasColumnName("ITINHCHAT");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IHIENTHI).HasColumnName("IHIENTHI");
            this.Property(t => t.IDELETE).HasColumnName("IDELETE");
            this.Property(t => t.INHOMNOIDUNG).HasColumnName("INHOMNOIDUNG");
        }
    }
}
