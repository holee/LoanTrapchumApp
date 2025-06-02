using Domains.Models.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
namespace Infrastructures.DataAccess.Configurations.AuditTrails;

//public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
//{
//    public void Configure(EntityTypeBuilder<AuditTrail> builder)
//    {
//        builder.ToTable("audit_trails");

//        builder.HasKey(e => e.Id);

//        builder.HasIndex(e => e.EntityName);

//        builder.Property(e => e.Id).IsRequired();

//        builder.Property(e => e.UserId).HasMaxLength(450);

//        builder.Property(e => e.EntityName)
//               .HasMaxLength(100)
//               .IsRequired();

//        builder.Property(e => e.DateUtc)
//               .IsRequired();

//        builder.Property(e => e.PrimaryKey)
//               .HasMaxLength(100);

//        builder.Property(e => e.TrailType)
//               .IsRequired(false)
//               .HasConversion<string>(); // stored as int by default

//        // JSON properties

//        builder.Property(e => e.ChangedColumns)
//               .HasColumnType("NVARCHAR(MAX)");
//        builder.Property(e => e.OldValues)
//               .HasColumnType("NVARCHAR(MAX)");
//        builder.Property(e => e.NewValues)
//               .HasColumnType("NVARCHAR(MAX)");
//        // optional FK to Identity user
//        builder.HasOne(e => e.User)
//               .WithMany()
//               .HasForeignKey(e => e.UserId)
//               .OnDelete(DeleteBehavior.SetNull);
//    }
//}

