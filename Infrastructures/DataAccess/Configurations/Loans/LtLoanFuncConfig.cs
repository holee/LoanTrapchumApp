using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class LtLoanFuncConfig : IEntityTypeConfiguration<LoanFund>
    {
        public void Configure(EntityTypeBuilder<LoanFund> builder)
        {
            builder.ToTable("LoanFund");
            builder.HasKey(x => x.LoanFundId);
            builder.Property(x => x.LoanFundId).ValueGeneratedOnAdd();
            builder.Property(x => x.LoanFundDesc).HasMaxLength(100);
            builder.HasData(new LoanFund[]
            {
                new LoanFund
                {
                    LoanFundId=1,
                    LoanFundDesc="Asia Development Bank"
                },
                 new LoanFund
                {
                    LoanFundId=2,
                    LoanFundDesc="World Bank"
                },
                  new LoanFund
                {
                    LoanFundId=3,
                    LoanFundDesc="ACLEDA Bank"
                }
            });
        }
    }
}
