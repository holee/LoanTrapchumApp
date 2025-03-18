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
            builder.HasKey(x => new { x.EmployeeRefId });
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.IsRemove).HasColumnType("BIT");
            builder.Property(x => x.CreatedDt).HasColumnType("DATETIME");
            builder.Property(x => x.ModifiedDt).HasColumnType("DATETIME");
            builder.HasOne(x => x.Employee)
                   .WithOne(x => x.LoanCreditAgent)
                   .HasForeignKey<LoanCreditAgent>(x => x.EmployeeRefId);
            builder.HasMany(x => x.LoanRequests)
                .WithOne(x => x.LoanCreditAgent)
                .HasForeignKey(x => x.CreditAgentRefId);
        }
    }
}
