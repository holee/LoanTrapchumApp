using Domains.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.DataAccess.Configurations.Loans
{
    public class CoLoanRequestConfiguration : IEntityTypeConfiguration<CoLoanRequest>
    {
        public void Configure(EntityTypeBuilder<CoLoanRequest> builder)
        {
            builder.ToTable("tt_CoLoanRequest");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Property(x => x.RequestId).HasMaxLength(250);
            builder.Property(x => x.LoanId).HasMaxLength(250);
            builder.Property(x => x.CuctomerId).HasMaxLength(250);
            builder.Property(x => x.Status).HasDefaultValue(true);
            builder.Property(x => x.CreatedDT).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.ModifiedDT).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.Description).HasMaxLength(400);
        }
    }
}
