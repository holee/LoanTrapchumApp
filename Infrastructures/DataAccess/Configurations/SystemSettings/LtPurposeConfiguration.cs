using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings;

public sealed class LtPurposeConfiguration : IBaseEntityTypeConfigurations<LtPurpose>
{
    public void Configure(EntityTypeBuilder<LtPurpose> builder)
    {
        builder.ToTable("lt_purposes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Title)
                .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                .HasMaxLength(255)
                .IsRequired(false);
    }
}
