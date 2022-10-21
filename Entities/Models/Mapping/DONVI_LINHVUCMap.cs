using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{

     public class DONVI_LINHVUCMap : EntityTypeConfiguration<DONVI_LINHVUC>
    {
         public DONVI_LINHVUCMap()
         {
             // Primary Key
             this.HasKey(t => t.ID);

             // Properties
             this.Property(t => t.ID)
                 .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

           

             // Table & Column Mappings
             this.ToTable("DONVI_LINHVUC", "KIENNGHI");
             this.Property(t => t.ID).HasColumnName("ID");
             this.Property(t => t.IDONVI).HasColumnName("IDONVI");
             this.Property(t => t.ILINHVUC).HasColumnName("ILINHVUC");
             this.Property(t => t.IDELETE).HasColumnName("IDELETE");
         }
    }
}
