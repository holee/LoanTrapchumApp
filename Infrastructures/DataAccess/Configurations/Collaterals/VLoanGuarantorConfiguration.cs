using Applications.Responses.ViewModels.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Collaterals;

//public class VLoanGuarantorConfiguration : IEntityTypeConfiguration<VLoanGuarantors>
//{
//    public void Configure(EntityTypeBuilder<VLoanGuarantors> builder)
//    {
//        builder.ToTable("VLoanGuarators");
//        builder.HasNoKey();
//        builder.Property(x => x.Principle).HasPrecision(18, 3);
//        builder.Property(x => x.AdminFeeRate).HasPrecision(18, 3);
//        builder.Property(x => x.CollectionFeeRate).HasPrecision(18, 3);
//        builder.Property(x => x.InterestRate).HasPrecision(18, 3);
//    }
//}
