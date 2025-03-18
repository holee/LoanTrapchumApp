using Domains.Models.Lists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.EntityNames
{
    public class EntityNameConfig : IEntityTypeConfiguration<EntityName>
    {
        public void Configure(EntityTypeBuilder<EntityName> builder)
        {
            builder.ToTable("lt_entityNames");
            builder.HasKey(x => x.EntId);
            builder.Property(x => x.EntId).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(150).HasColumnType("NVARCHAR");
            builder.Property(x => x.AltName).HasMaxLength(150).HasColumnType("NVARCHAR"); ;
            builder.Property(x => x.FirstName).HasMaxLength(50).HasColumnType("NVARCHAR"); ;
            builder.Property(x => x.MiddleName).HasMaxLength(50).HasColumnType("NVARCHAR"); ;
            builder.Property(x => x.LastName).HasMaxLength(50).HasColumnType("NVARCHAR"); ;
            builder.Property(x => x.EntType).HasMaxLength(50).HasColumnType("NVARCHAR"); ;
            builder.Property(e => e.EmailAddress).HasMaxLength(100);
            builder.Property(e => e.Phone).HasMaxLength(20).IsUnicode();
            builder.Property(e => e.EditSeq).HasDefaultValue(0);
            builder.Property(e => e.IsActive).HasDefaultValue(true);
            builder.Property(e => e.CreatedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.ModifiedDt).HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.Description).HasMaxLength(400);

        }
    }
}
