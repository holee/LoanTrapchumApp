using Applications.Responses.ViewModels.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals;

public class VLoanGuarantorConfiguration : IEntityTypeConfiguration<VLoanGuarantors>
{
    public void Configure(EntityTypeBuilder<VLoanGuarantors> builder)
    {
        builder.ToTable("VLoanGuarators");
        builder.HasNoKey();
    }
}
