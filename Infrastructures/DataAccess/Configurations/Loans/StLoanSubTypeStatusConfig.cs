using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class StLoanSubTypeStatusConfig : IEntityTypeConfiguration<LoanTypeStatus>
    {
        public void Configure(EntityTypeBuilder<LoanTypeStatus> builder)
        {
            builder.ToTable("st_loanTypeStatus");
            builder.HasKey(x => x.LoanTypeStatusId);
            builder.Property(x => x.LoanTypeStatusId).ValueGeneratedNever();
            builder.Property(x => x.Description).HasMaxLength(200);
        }
    }
}
