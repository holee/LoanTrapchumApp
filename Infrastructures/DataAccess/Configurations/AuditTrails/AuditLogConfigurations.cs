using Domains.Models.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.AuditTrails
{
    public sealed class AuditLogConfigurations : IBaseEntityTypeConfigurations<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");
            builder.HasKey(x => x.Id);
            builder.Property(x =>x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.UserId).HasMaxLength(450);
        }
    }
}
