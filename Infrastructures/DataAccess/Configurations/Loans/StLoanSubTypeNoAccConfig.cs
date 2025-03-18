using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class StLoanSubTypeNoAccConfig : IEntityTypeConfiguration<LoanSubTypeStatus>
    {
        public void Configure(EntityTypeBuilder<LoanSubTypeStatus> builder)
        {
            builder.ToTable("st_loanSubTypeStatus");
            builder.HasKey(x => x.LoanSubTypeId);
            builder.Property(x => x.LoanSubTypeId).ValueGeneratedOnAdd();
            builder.Property(x => x.Description).HasMaxLength(100);
            builder.Property(x => x.LoanTypeStatusRefId);
        }
    }
}
