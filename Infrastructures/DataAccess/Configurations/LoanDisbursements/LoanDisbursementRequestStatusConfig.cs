using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public class LoanDisbursementRequestStatusConfig : IEntityTypeConfiguration<LoanRequestStatus>
    {
        public void Configure(EntityTypeBuilder<LoanRequestStatus> builder)
        {
            builder.ToTable("st_loanDisbursementRequestStatus");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Status).HasMaxLength(20);
            builder.Property(x => x.Desctiption).HasMaxLength(100);
        }
    }
}
