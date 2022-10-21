using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Entities.Models.Mapping
{
    public class DAIBIEU_KYHOPMap : EntityTypeConfiguration<DAIBIEU_KYHOP>
    {
        public DAIBIEU_KYHOPMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.ToTable("DAIBIEU_KYHOP", "KIENNGHI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ID_DAIBIEU).HasColumnName("ID_DAIBIEU");
            this.Property(t => t.ID_KYHOP).HasColumnName("ID_KYHOP");
        }

    }
}
