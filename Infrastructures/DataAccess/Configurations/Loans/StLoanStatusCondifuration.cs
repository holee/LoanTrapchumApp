using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class StLoanStatusCondifuration : IEntityTypeConfiguration<LoanStatus>
    {
        public void Configure(EntityTypeBuilder<LoanStatus> builder)
        {

            builder.ToTable("st_loanStatuses");

            builder.HasKey(x => x.LoanstatusId);
            builder.Property(x => x.Loanstatuscode).HasMaxLength(20).IsRequired(false);
            builder.Property(x => x.Description).HasMaxLength(100).IsRequired(false);
            builder.HasData(Create(1, "Standard", "Standard"));
            builder.HasData(Create(2, "Close", "Close"));
            builder.HasData(Create(3, "Deleted", "Deleted the wrong entry"));
            builder.HasData(Create(4, "Active", "Active Loan"));
            builder.HasData(Create(5, "Lost", "Lost"));
            builder.HasData(Create(6, "Rollover", "Rollover to new loan"));
            builder.HasData(Create(7, "Sub Standard", "Sub Standard"));
            builder.HasData(Create(8, "Doubtful", "Doubtful"));
            builder.HasData(Create(9, "Special Mention", "Special Mention"));
        }


        private LoanStatus Create(int id, string code, string description)
        {
            return new LoanStatus
            {
                LoanstatusId = id,
                Loanstatuscode = code,
                Description = description
            };
        }


    }
}
