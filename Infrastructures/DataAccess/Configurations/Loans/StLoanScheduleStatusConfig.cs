using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class StLoanScheduleStatusConfig : IEntityTypeConfiguration<LoanScheduleStatus>
    {
        public void Configure(EntityTypeBuilder<LoanScheduleStatus> builder)
        {

            builder.ToTable("st_LoanScheduleStatuses");
            builder.HasKey(k => k.LoanScheduleStatusId);
            builder.Property(x => x.LoanScheduleStatusId).ValueGeneratedOnAdd();
            builder.Property(x => x.LoanScheduleStatusCode).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(200);

            builder.HasData(new LoanScheduleStatus[]
            {
                new LoanScheduleStatus
                {
                    LoanScheduleStatusId=1,
                    LoanScheduleStatusCode="Paid",
                    Description="Paid"
                },
                 new LoanScheduleStatus
                {
                    LoanScheduleStatusId=2,
                    LoanScheduleStatusCode="Not Paid",
                    Description="Not Paid"
                },
                  new LoanScheduleStatus
                {
                    LoanScheduleStatusId=3,
                    LoanScheduleStatusCode="Cancel",
                    Description="Cancel"
                }
            });
        }
    }
}
