using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class LoanRequestScheduleConfiguration : IEntityTypeConfiguration<LoanRequestSchedule>
    {
        public void Configure(EntityTypeBuilder<LoanRequestSchedule> builder)
        {
            builder.ToTable("tt_loanRequestSchedules");
            builder.HasKey(x => x.LoanRequestScheduleId);
            builder.Property(x => x.LoanRequestScheduleId).ValueGeneratedOnAdd();
            builder.Property(x => x.Principal).HasPrecision(18, 2);
            builder.Property(x => x.InterestAmount).HasPrecision(18, 2);
            builder.Property(x => x.CollectionFeeAmount).HasPrecision(18, 2);
            builder.Property(x => x.TotalPay).HasPrecision(18, 2);
            builder.Property(x => x.LoanOutstanding).HasPrecision(18, 2);
        }
    }

}
