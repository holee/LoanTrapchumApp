using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public class LoanRepaymentModeConfig : IEntityTypeConfiguration<LoanRepaymentMode>
    {
        public void Configure(EntityTypeBuilder<LoanRepaymentMode> builder)
        {
            builder.ToTable("st_LoanRepaymentModes");
            builder.HasKey(x => x.RepaymentModeId);
            builder.Property(prop => prop.RepaymentModeCode).HasMaxLength(30);
            builder.Property(prop => prop.Description).HasMaxLength(200);
            builder.HasData(new LoanRepaymentMode[]{
                new LoanRepaymentMode
                {
                    RepaymentModeId=1,
                    RepaymentModeCode="Balloon",
                    Description="Balloon"
                },new LoanRepaymentMode
                {
                    RepaymentModeId=2,
                    RepaymentModeCode="Decline",
                    Description="Decline"
                },
                new LoanRepaymentMode
                {
                    RepaymentModeId=3,
                    RepaymentModeCode="Flat",
                    Description="Flat"
                },
                new LoanRepaymentMode
                {
                    RepaymentModeId=4,
                    RepaymentModeCode="Amortization",
                    Description="Amortization"
                },

            });

        }
    }
}

