using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class TIEPDAN_THUONGXUYEN_KETQUAMap : EntityTypeConfiguration<TIEPDAN_THUONGXUYEN_KETQUA>
    {
        public TIEPDAN_THUONGXUYEN_KETQUAMap()
        {
            // Primary Key
            this.HasKey(t => t.IKETQUA);

            // Properties
            this.Property(t => t.IKETQUA)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CKETQUA)
                .HasMaxLength(250);

            this.Property(t => t.CKETQUA_NGUOITRALOI)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("TIEPDAN_THUONGXUYEN_KETQUA", "KIENNGHI");
            this.Property(t => t.IKETQUA).HasColumnName("IKETQUA");
            this.Property(t => t.ITHUONGXUYEN).HasColumnName("ITHUONGXUYEN");
            this.Property(t => t.CKETQUA).HasColumnName("CKETQUA");
            this.Property(t => t.CKETQUA_NGUOITRALOI).HasColumnName("CKETQUA_NGUOITRALOI");
        }
    }
}
