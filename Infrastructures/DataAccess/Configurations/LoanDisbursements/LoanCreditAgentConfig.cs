using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.LoanDisbursements
{
    public class LoanCreditAgentConfig : IEntityTypeConfiguration<LoanCreditAgent>
    {
        public void Configure(EntityTypeBuilder<LoanCreditAgent> builder)
        {
            builder.ToTable("lt_loanCreditAgents");
            builder.HasKey(x => x.EmployeeRefId);
            builder.Property(x => x.EmployeeRefId).HasMaxLength(250);
            builder.Property(x => x.IsRemove).HasColumnType("BIT");
            builder.Property(x => x.CreatedDt).IsRequired(false);
            builder.Property(x => x.ModifiedDt).IsRequired(false);
        }
    }
}
