using Domains.Models.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructures.DataAccess.Configurations.AuditTrails;

public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditEntry>
{
    public void Configure(EntityTypeBuilder<AuditEntry> builder)
    {
        builder.ToTable("audit_trails");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.HasIndex(e => e.TableName);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.UserId).HasMaxLength(450);
        builder.Property(e => e.TableName).HasMaxLength(120)
               .IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();      
        builder.Property(e => e.ActionType).IsRequired(false);
        builder.Property(e => e.CreatedAt).HasColumnType("DATETIME").IsRequired(true);
        builder.Property(e => e.DeletedAt).HasColumnType("DATETIME").IsRequired(false);
        builder.Property(e => e.UpdatedAt).HasColumnType("DATETIME").IsRequired(false);
        // JSON properties
        builder.Property(e => e.KeyValues).HasMaxLength(450);
        builder.Property(e => e.OldValues)
               .HasColumnType("NVARCHAR(MAX)");
        builder.Property(e => e.NewValues)
               .HasColumnType("NVARCHAR(MAX)");
    }
}

