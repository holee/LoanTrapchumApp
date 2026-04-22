using Applications.Responses.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.SystemSettings;

public class VUserLoanSizesConfiguration : IEntityTypeConfiguration<VUserLoanSizes>
{
    public void Configure(EntityTypeBuilder<VUserLoanSizes> builder)
    {
        builder.ToView("VUserLoanSizes");
        builder.HasNoKey();
        builder.Property(x => x.Id);
        builder.Property(x => x.UserRefId).HasMaxLength(250);
        builder.Property(x => x.FullName).HasMaxLength(200);
        builder.Property(x => x.BranchId);
        builder.Property(x => x.BranchName).HasMaxLength(200);
        builder.Property(x => x.MinUsdLoanSize).HasPrecision(18, 3);
        builder.Property(x => x.MaxUsdLoanSize).HasPrecision(18, 3);
        builder.Property(x => x.MinKhrLoanSize).HasPrecision(18, 3);
        builder.Property(x => x.MaxKhrLoanSize).HasPrecision(18, 3);
        builder.Property(x => x.StartDate).HasColumnType("DATE");
        builder.Property(x => x.EndDate).HasColumnType("DATE");
        builder.Property(x => x.Description).HasMaxLength(400);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.EditSeq).HasDefaultValue(0);
    }
}
