using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class USERMap : EntityTypeConfiguration<USER>
    {
        public USERMap()
        {
            // Primary Key
            this.HasKey(t => t.IUSER);

            // Properties
            this.Property(t => t.IUSER)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CUSERNAME)
                .HasMaxLength(500);

            this.Property(t => t.CPASSWORD)
                .HasMaxLength(4000);

            this.Property(t => t.CTEN)
                .HasMaxLength(250);

            this.Property(t => t.CEMAIL)
                .HasMaxLength(500);

            this.Property(t => t.CSDT)
                .HasMaxLength(500);

            this.Property(t => t.CARRGROUP)
                .HasMaxLength(2000);

            this.Property(t => t.CSALT)
                .HasMaxLength(2000);

            this.Property(t => t.CAUTHTOKEN)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("USERS", "KIENNGHI");
            this.Property(t => t.IUSER).HasColumnName("IUSER");
            this.Property(t => t.CUSERNAME).HasColumnName("CUSERNAME");
            this.Property(t => t.CPASSWORD).HasColumnName("CPASSWORD");
            this.Property(t => t.CTEN).HasColumnName("CTEN");
            this.Property(t => t.IPHONGBAN).HasColumnName("IPHONGBAN");
            this.Property(t => t.CEMAIL).HasColumnName("CEMAIL");
            this.Property(t => t.CSDT).HasColumnName("CSDT");
            this.Property(t => t.ISTATUS).HasColumnName("ISTATUS");
            this.Property(t => t.IDONVI).HasColumnName("IDONVI");
            this.Property(t => t.ICHUCVU).HasColumnName("ICHUCVU");
            this.Property(t => t.CARRGROUP).HasColumnName("CARRGROUP");
            this.Property(t => t.ITYPE).HasColumnName("ITYPE");
            this.Property(t => t.CSALT).HasColumnName("CSALT");
            this.Property(t => t.DLASTCHANGEPASS).HasColumnName("DLASTCHANGEPASS");
            this.Property(t => t.CAUTHTOKEN).HasColumnName("CAUTHTOKEN");
            this.Property(t => t.ILOGINFAIL).HasColumnName("ILOGINFAIL");
        }
    }
}
